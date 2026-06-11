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
using BusinessObject.AlertManagement;
using BusinessObject.SaleOrder;
using System.Threading;
using ERPManager.Sales;
using BusinessLogic.CommonManagement;

namespace ERPSMS_v01.Sales
{
    public partial class SalesReceipt : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// Invoice PO Split List
        /// </summary>


        private List<FIN_RECEIPT_CUS_SO_MPG> InvoiceSOSplitList
        {

            get
            {
                return this.ViewState[ViewstateStrings.InvoiceSOSplitList] != null ? (List<FIN_RECEIPT_CUS_SO_MPG>)this.ViewState[ViewstateStrings.InvoiceSOSplitList] : new List<FIN_RECEIPT_CUS_SO_MPG>();
            }
            set
            {
                if (value == null)
                    ViewState.Remove(ViewstateStrings.InvoiceSOSplitList);
                else
                    this.ViewState[ViewstateStrings.InvoiceSOSplitList] = value;
            }

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
        private long CurrPK
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.CurrPK]);
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
        private List<long> SelectedSalesInvoices
        {
            //get
            //{
            //    return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedSalesInvoices];
            //}
            //set
            //{
            //    Session[ERP.Utilities.SessionStrings.SelectedSalesInvoices] = value;
            //}

            get
            {
                return this.ViewState[ViewstateStrings.SelectedSalesInvoices] != null ? (List<long>)this.ViewState[ViewstateStrings.SelectedSalesInvoices] : null;
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedSalesInvoices] = value;
            }

        }
        /// <summary>
        /// To maintain keep selected pos
        /// </summary>
        private List<FIN_RECEIPT_CUS_ALCN_DTL> ReceiptAdjnList
        {

            get
            {
                return this.ViewState[ViewstateStrings.ReceiptAdjnList] != null ? (List<FIN_RECEIPT_CUS_ALCN_DTL>)this.ViewState[ViewstateStrings.ReceiptAdjnList] : new List<FIN_RECEIPT_CUS_ALCN_DTL>();
            }
            set
            {
                if (value == null)
                    ViewState.Remove(ViewstateStrings.ReceiptAdjnList);
                else
                    this.ViewState[ViewstateStrings.ReceiptAdjnList] = value;
            }

        }

        private List<FIN_RECEIPT_CUS_SUSP_DTL> finReceiptSuspList
        {
            get
            {
                return this.ViewState["finReceiptSuspList"] != null ? (List<FIN_RECEIPT_CUS_SUSP_DTL>)this.ViewState["finReceiptSuspList"] : new List<FIN_RECEIPT_CUS_SUSP_DTL>();
            }
            set
            {
                if (value == null)
                    ViewState.Remove("finReceiptSuspList");
                else
                    this.ViewState["finReceiptSuspList"] = value;
            }
        }
        /// <summary>
        /// To maintain keep selected pos
        /// </summary>
        private List<ERPData.FIN_INVOICE_CUS_HDR> EditedSalesInvoices
        {
            get
            {
                return (List<ERPData.FIN_INVOICE_CUS_HDR>)Session[ERP.Utilities.SessionStrings.EditedSalesInvoices];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.EditedSalesInvoices] = value;
            }

        }
        private List<ERPData.FIN_RECEIPT_CUS_TRX_MPG> EditedReceiptDtls
        {
            get
            {
                return (List<ERPData.FIN_RECEIPT_CUS_TRX_MPG>)Session[ERP.Utilities.SessionStrings.EditedReceiptDtls];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.EditedReceiptDtls] = value;
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
        #endregion
        private DataTable dtDiscountTypes;
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        User currentUser;
        //page related Entity Object
        private ServiceUtility serviceUtilityObj;
        private FIN_RECEIPT_CUS_HDR finReceiptCusHdrObj;
        private FIN_RECEIPT_CUS_TRX_MPG finReceiptCusTrxMpgObj;
        private FIN_INVOICE_CUS_HDR finInvoiceCusHdrObj;
        private FIN_COA_MST finCoaMstObj;
        private FIN_CASH_BANK_MST finCashBankMstObj;
        private ReceiptAdjnAllocation ReceiptAdjnObj;

        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;

        private FIN_RECEIPT_CUS_SO_MPG finReceiptCusSoMpgObj;
        private FIN_INVOICE_CUS_TRX_MPG FinInvoiceCusTrxMpgObj;
        private FIN_RECEIPT_CUS_ALCN_DTL FinReceiptCusAllocationObj;
        private FIN_INVOICE_CUS_HDR finInvoiceVndHdrObjForPaymentSplit;
        private FIN_CRDR_NOTE_HDR FinCrDrAllocationObj;


        private FIN_RECEIPT_CUS_TAX_DTL finReceiptCusTaxDtlObj;
        private List<FIN_RECEIPT_CUS_TAX_DTL> finReceiptCusTaxDtlList;

        //List for binding details to controls  
        private List<FIN_RECEIPT_CUS_HDR> finReceiptCusHdrList;
        private List<FIN_RECEIPT_CUS_TRX_MPG> finReceiptCusTrxMpgList;
        private List<FIN_INVOICE_CUS_HDR> finInvoiceCusHdrList;
        private List<FIN_INVOICE_CUS_HDR> ObjFinInvoiceCusHdrList;
        private List<FIN_COA_MST> finCoaMstList;
        private List<FIN_CASH_BANK_MST> finCashBankMstList;
        private List<long> selectedInvoiceList;
        private List<FIN_CASH_BANK_MST> crmCustomerMstList;
        //private List<FIN_RECEIPT_CUS_SUSP_DTL> finReceiptSuspList;

        private List<FIN_RECEIPT_CUS_SO_MPG> finReceiptCusSoMpgList;
        private List<FIN_INVOICE_CUS_TRX_MPG> FinInvoiceCusTrxMpgList;
        private List<FIN_INVOICE_CUS_TRX_MPG> FinInvoiceCusTrxMpgListForAutoAlcn;
        private List<FIN_RECEIPT_CUS_ALCN_DTL> FinReceiptCusAllocationList;
        private List<FIN_RECEIPT_CUS_ALCN_DTL> FinReceiptCusAdjnDupCheckList;

        private FIN_RECEIPT_CUS_TAX_DTL finRecCusTaxDtlObj;
        private List<FIN_RECEIPT_CUS_TAX_DTL> finRecCusTaxDtlList;


        private List<FIN_INVOICE_CUS_HDR> finInvoiceVndHdrListForPaymentSplit;
        private List<ReceiptAdjnAllocation> CrDrAdjnList = null;

        private List<ReceiptCrdrMpg> FinReceiptCusCrdrMpgList;
        private List<ReceiptCrdrMpg> FinReceiptCusCrdrAllocationList;
        private FIN_RECEIPT_CUS_CRDR_MPG FinReceiptCusCrdrAllocationObj;
        private List<FIN_CRDR_NOTE_MPG> FinCrdrMpgList;

        private bool isSplitChanged = false;
        private decimal totAllocateAdjn = 0;
        private decimal totBalanceAdjn = 0;

        private decimal totalAmtFooter = 0;
        private decimal totalTaxFooter = 0;
        private decimal totalOtheramtFooter = 0;
        private decimal totalReceivedFooter = 0;

        private decimal balRecieveFooter = 0;

        int isSplitApply = 0;
        int JournalPK;
        int ValidateId = 0;
        string PKXml = "";
        private bool updateReceipt;
        private string refID;
        private string inboxFlag;
        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;

        private List<FIN_TRX_HDR> finTrxHdrPDCList;

        private List<FIN_TRX_HDR> pdcVoucherList;

        private ADM_CONFIG_MST admConfigMstObj;

        private List<SPADM_APP_STATUS_CFG_GET_KV_Result> workflowStatusList;
        private List<FIN_YEAR_MST> finYearMstList;

        private ADM_CURRENCY_MST admCurrencyMstObj;
        private List<ADM_CURRENCY_MST> admCurrencyMstList;
        private List<ADM_CONST_MST> admConstMstList;
        private ADM_APP_CONFIG_MST admAppConfigMstObj;
        private List<ADM_APP_CONFIG_MST> admAppConstMstList;
        DataSet dsAlertList;
        private int invPK;
        private string appType;
        private string TypeRef;
        private bool InstrNoResult;
        private bool IsContReturn = false;

        private long ICH_PK;

        DataTable dtCompany = new DataTable();
        DataTable dtReceivedAmt = new DataTable();

        List<FIN_RECEIPT_CUS_SO_MPG> tempFinReceiptCusSoMpgList;

        private Dictionary<string, decimal> dicTempAmount = new Dictionary<string, decimal>();
        private int recipetTypePDC;
        DataTable dtReceiptAlloc;
        DataTable dtSuspenceList;
        DataTable dtCurrency;
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
                    //  InvoiceSOSplitList = null;
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
                    //txtSearchDateFrom.Text = string.Empty;
                    //hdfSearchDateFrom.Value = string.Empty;
                    //txtSearchDateTo.Text = string.Empty;
                    //hdfSearchDateTo.Value = string.Empty;
                    //GetFieldValues(ControlsEnum.FINPERIOD);
                    //SetFieldValues(ControlsEnum.FINPERIOD);

                    hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                    hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();

                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                    }


                    GetFieldValues(ControlsEnum.PAYMODE);
                    SetFieldValues(ControlsEnum.PAYMODE);

                    if (Session[ERP.Utilities.SessionStrings.SelectedSalesInvoices] != null)
                    {
                        SelectedSalesInvoices = (List<long>)Session[ERP.Utilities.SessionStrings.SelectedSalesInvoices];
                        Session[ERP.Utilities.SessionStrings.SelectedSalesInvoices] = null;
                        EntryStatus = EntryStatus.NEWMODE;
                    }


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
                        GetFieldValues(ControlsEnum.RECEIPTGET);
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
                            int mode;
                            GetFieldValues(ControlsEnum.RECEIPTHDRENTRYBYPK);
                            GetUIValuesFromObject(ControlsEnum.RECEIPTHDRENTRY);
                            GetFieldValues(ControlsEnum.RECEIPTHDRINVLISTBYPK);
                            //GetFieldValues(ControlsEnum.CRDRALLOCATION);
                            SetFieldValues(ControlsEnum.RECEIPTMPGLIST);

                            //for adj allocation;need all saved val's in ReceiptAdjnList
                            foreach (GridViewRow gvrw in grdInvoiceList.Rows)
                            {
                                HiddenField hdfReceiptMpgPK = gvrw.FindControl("hdfReceiptMpgPK") as HiddenField;
                                ReceiptMpgPK = Convert.ToInt64(hdfReceiptMpgPK.Value);
                                HiddenField hdfInvoicePK = gvrw.FindControl("hdfInvoicePK") as HiddenField;

                                InvoicePK = Convert.ToInt32(hdfInvoicePK.Value);

                                GetFieldValues(ControlsEnum.RECEIPTADJNLIST);
                            }
                            GetFieldValues(ControlsEnum.CRDRALLOCATION);

                            ModifiedDatePnl.Visible = true;
                            mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);

                            switch (mode)
                            {
                                case (int)ReceiptModeEnum.CASH:
                                    vrfBankName.Enabled = false;
                                    vrfBranch.Enabled = false;
                                    vrfAccountNo.Enabled = false;
                                    //vrfInstrumentNo.Enabled = false;
                                    vrfInstrumentDate.Enabled = false;
                                    txtInstrumentNo.Enabled = false;
                                    txtInstrumentNo.CssClass = "input-small input-disabled ";
                                    txtInstrumentDate.Enabled = false;
                                    txtInstrumentDate.CssClass = "input-small input-disabled";
                                    txtbankOfCheque.Enabled = false;
                                    txtbankOfCheque.CssClass = "input-half input-disabled";
                                    break;
                                case (int)ReceiptModeEnum.BANK:
                                    vrfBankName.Enabled = true;
                                    vrfBranch.Enabled = true;
                                    vrfAccountNo.Enabled = true;
                                    //vrfInstrumentNo.Enabled = false;
                                    vrfInstrumentDate.Enabled = false;
                                    txtInstrumentNo.Enabled = true;
                                    txtInstrumentNo.CssClass = "input-small medium";
                                    txtInstrumentDate.Enabled = true;
                                    txtInstrumentDate.CssClass = "input-small";
                                    txtbankOfCheque.Enabled = false;
                                    txtbankOfCheque.CssClass = "input-half input-disabled";
                                    break;
                                case (int)ReceiptModeEnum.CHEQUE:
                                    vrfBankName.Enabled = true;
                                    vrfBranch.Enabled = true;
                                    vrfAccountNo.Enabled = true;
                                    //vrfInstrumentNo.Enabled = true;
                                    vrfInstrumentDate.Enabled = true;
                                    txtInstrumentNo.Enabled = true;
                                    txtInstrumentNo.CssClass = "input-small";
                                    txtInstrumentDate.Enabled = true;
                                    txtInstrumentDate.CssClass = "input-small";
                                    txtbankOfCheque.Enabled = true;
                                    txtbankOfCheque.CssClass = "input-half";
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
                                    break;
                                default:
                                    vrfBankName.Enabled = true;
                                    vrfBranch.Enabled = true;
                                    vrfAccountNo.Enabled = true;
                                    //vrfInstrumentNo.Enabled = true;
                                    vrfInstrumentDate.Enabled = true;
                                    txtInstrumentNo.Enabled = true;
                                    txtInstrumentNo.CssClass = "input-small";
                                    txtInstrumentDate.Enabled = true;
                                    txtInstrumentDate.CssClass = "input-small";
                                    txtbankOfCheque.Enabled = false;
                                    txtbankOfCheque.CssClass = "input-half input-disabled";
                                    break;
                            }
                        }
                        else
                        {
                            //Sets data key for the gird
                            string[] datakeyarray;
                            datakeyarray = new string[1];
                            datakeyarray[0] = Resources.DataFieldRes.SalesReceiptPK;
                            grdPOReceiptHdr.DataKeyNames = datakeyarray;
                            if (SelectedSalesInvoices != null && SelectedSalesInvoices.Count > 0)
                            {
                                SetCancelRef((int)CurrPK);
                                ucrWrkf.FillWorkFlowDetails();
                                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                                {
                                    ucrWrkf.ViewType = 1;
                                    EntryStatus = EntryStatus.NEWMODE;
                                }
                                else
                                {
                                    ucrWrkf.ViewType = 0;
                                    //EntryStatus = EntryStatus.VIEWMODE;
                                    //btnSave.Visible = false;
                                    //divbtnSavePaymentSplit.Visible = false;
                                }
                                selectedInvoiceList = SelectedSalesInvoices;
                                GetFieldValues(ControlsEnum.RECEIPTMPGLIST);
                                if (finInvoiceCusHdrList != null && finInvoiceCusHdrList.Count > 0)
                                {
                                    SOGroup = (SalesInvoiceGroup)Enum.Parse(typeof(SalesInvoiceGroup), finInvoiceCusHdrList.First().ICH_GROUP.ToString());
                                    SICategory = (SalesInvoiceCategory)Enum.Parse(typeof(SalesInvoiceCategory), finInvoiceCusHdrList.First().ICH_CATEGORY.ToString());
                                }
                                GetFieldValues(ControlsEnum.CRDRALLOCATION);
                                SetFieldValues(ControlsEnum.RECEIPTMPGLIST);
                                //GetFieldValues(ControlsEnum.EXCHANGERATEINBASECURRENCY);
                                //decimal exchangeRate = !string.IsNullOrEmpty(hdfExchangeCurrBC.Value) ? Convert.ToDecimal(hdfExchangeCurrBC.Value) : 0;
                                //txtExchangeRate.Text = String.Format("{0:c}", exchangeRate);
                                //decimal receivedAmount = !string.IsNullOrEmpty(txtReceivedAmount.Text.Trim()) ? Convert.ToDecimal(txtReceivedAmount.Text.Trim()) : 0;
                                //txtTotalAmountBC.Text = String.Format("{0:c}", exchangeRate * receivedAmount);

                                lblReceiptNo.Text = "[NEW]";
                                txtReceiptDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                                int mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);

                                switch (mode)
                                {
                                    case (int)ReceiptModeEnum.CASH:
                                        vrfBankName.Enabled = false;
                                        vrfBranch.Enabled = false;
                                        vrfAccountNo.Enabled = false;
                                        //vrfInstrumentNo.Enabled = false;
                                        vrfInstrumentDate.Enabled = false;
                                        txtInstrumentNo.Enabled = false;
                                        txtInstrumentNo.CssClass = "input-small input-disabled";
                                        txtInstrumentDate.Enabled = false;
                                        txtInstrumentDate.CssClass = "input-small input-disabled";
                                        vrfBankOfCheque.Enabled = false;
                                        break;
                                    case (int)ReceiptModeEnum.BANK:
                                        vrfBankName.Enabled = true;
                                        vrfBranch.Enabled = true;
                                        vrfAccountNo.Enabled = true;
                                        //vrfInstrumentNo.Enabled = false;
                                        vrfInstrumentDate.Enabled = false;
                                        txtInstrumentNo.Enabled = true;
                                        txtInstrumentNo.CssClass = "input-small";
                                        txtInstrumentDate.Enabled = true;
                                        txtInstrumentDate.CssClass = "input-small";
                                        vrfBankOfCheque.Enabled = false;
                                        break;
                                    case (int)ReceiptModeEnum.CHEQUE:
                                        vrfBankName.Enabled = true;
                                        vrfBranch.Enabled = true;
                                        vrfAccountNo.Enabled = true;
                                        //vrfInstrumentNo.Enabled = true;
                                        vrfInstrumentDate.Enabled = true;
                                        txtInstrumentNo.Enabled = true;
                                        txtInstrumentNo.CssClass = "input-small";
                                        txtInstrumentDate.Enabled = true;
                                        txtInstrumentDate.CssClass = "input-small";
                                        vrfBankOfCheque.Enabled = true;
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
                                        break;
                                    default:
                                        vrfBankName.Enabled = true;
                                        vrfBranch.Enabled = true;
                                        vrfAccountNo.Enabled = true;
                                        //vrfInstrumentNo.Enabled = true;
                                        vrfInstrumentDate.Enabled = true;
                                        txtInstrumentNo.Enabled = true;
                                        txtInstrumentNo.CssClass = "input-small";
                                        txtInstrumentDate.Enabled = true;
                                        txtInstrumentDate.CssClass = "input-small";
                                        vrfBankOfCheque.Enabled = false;
                                        break;
                                }
                            }
                            else
                            {
                                selectedInvoiceList = new List<long>();
                                GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                GetFieldValues(ControlsEnum.PDCVOUCHERLIST);
                                SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                EntryStatus = EntryStatus.LISTMODE;
                                PageIndex = "1";
                                uclPaging.TotalPages = TotalPages;
                                uclPaging.CurrentPage = 1;
                            }
                            hdfReceiptReturnHide.Value = "1";
                        }
                        //GetFieldValues(ControlsEnum.CRDRALLOCATION); 
                        #endregion
                    }
                    if (hdfMultiCurrencyInReceipt.Value == "1")
                    {
                        DivReceiptCurrency.Visible = true;
                        lblddlReceiptCurrency.Visible = true;
                        ddlReceiptCurrency.Visible = true;
                        GetFieldValues(ControlsEnum.CURRENCY);
                        SetFieldValues(ControlsEnum.CURRENCY);
                        SetMultiCurrencyConfiguration();
                        if (!string.IsNullOrEmpty(hdfReceiptCurrency.Value) && hdfMultiCurrency.Value == "1")
                        {
                            // ddlReceiptCurrency.SelectedValue = hdfReceiptCurrency.Value.ToString();
                            GetFieldValues(ControlsEnum.EXCHANGERATEINRECEIPTCURRENCY);
                            double exchangeCurrReceipt = hdfExchangeCurrReceipt.Value == "" ? 0.00 : Convert.ToDouble(hdfExchangeCurrReceipt.Value);
                            txtExchangeRate.Text = Math.Round(exchangeCurrReceipt == -1 ? 0.00 : exchangeCurrReceipt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //ddlReceiptCurrency.SelectedIndex = ddlReceiptCurrency.Items.IndexOf(ddlCompany.Items.FindByValue(hdfReceiptCurrency.Value.ToString()));
                            GetFieldValues(ControlsEnum.GETMULTIPLEEXCHANGERATES);
                        }
                        else
                        {
                            txtExchangeRate.Text = "1";
                        }
                        // txtBaseCurrency.Text = currentUser.BaseCurrency.ToString();
                    }
                    else
                    {
                        DivReceiptCurrency.Visible = false;
                        lblddlReceiptCurrency.Visible = false;
                        ddlReceiptCurrency.Visible = false;
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
            int? Status = null;
            int? PDCStatus = null;
            DataTable dtConfig = null;
            int sbuID = -1;
            CommonService CommonServiceClient;
            CommonServiceClient = null;

            SalesInvoiceService salesInvoiceServiceClient;
            salesInvoiceServiceClient = null;

            AdmCompanyMstService admCompanyMstServiceClient;

            FIN_RECEIPT_CUS_HDR tempFinReceiptCusHdrObj;
            tempFinReceiptCusHdrObj = null;


            try
            {
                switch (type)
                {
                    #region RECEIPT GET
                    case ControlsEnum.RECEIPTGET:
                        int GInvPk = Convert.ToInt32(Request.QueryString[QueryStrings.PK]);
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        tempFinReceiptCusHdrObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_HDR>();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.FilterBy = string.Empty;
                        serviceUtilityObj.FilterValue = string.Empty;
                        tempFinReceiptCusHdrObj.RCH_CUSTOMER = 0;
                        tempFinReceiptCusHdrObj.RCH_BIZUNIT = currentUser.SBUID;
                        tempFinReceiptCusHdrObj.RCH_PK = GInvPk;
                        tempFinReceiptCusHdrObj.RCH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        tempFinReceiptCusHdrObj.RCH_CRTD_BY = currentUser.PKUser;
                        serviceUtilityObj.FilterDate = DateTime.MinValue;
                        serviceUtilityObj.FilterToDate = DateTime.MinValue;
                        Status = 3;
                        PDCStatus = 0;
                        finReceiptCusHdrList = salesReceiptServiceClient.GetReceiptHdr(tempFinReceiptCusHdrObj, serviceUtilityObj, Status, "", PDCStatus);
                        TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 : (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) : (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        break;
                    #endregion
                    #region Receipt Hdr List
                    case ControlsEnum.RECEIPTHDRLIST:
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        tempFinReceiptCusHdrObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_HDR>();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdPOReceiptHdr.PageSize;
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.ReceiptDate : SortBy;
                        serviceUtilityObj.ThenBy = ThenBy = ThenBy == null ? Resources.DataFieldRes.ReceiptNo : ThenBy;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;
                        serviceUtilityObj.FilterBy = string.Empty;
                        serviceUtilityObj.FilterValue = string.Empty;
                        tempFinReceiptCusHdrObj.RCH_CUSTOMER = string.IsNullOrEmpty(hdfCustomerID.Value) ? 0 : Convert.ToInt32(hdfCustomerID.Value);
                        if (hdfCustomerID.Value != null && hdfCustomerID.Value != "0" && hdfCustomerID.Value != "")
                        {
                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = hdfCustomerID.Value;
                            Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = txtCustomer.Text;
                        }

                        tempFinReceiptCusHdrObj.RCH_BIZUNIT = currentUser.SBUID;


                        tempFinReceiptCusHdrObj.RCH_PK = string.IsNullOrEmpty(hdfReceiptPK.Value) ? 0 : Convert.ToInt64(hdfReceiptPK.Value);
                        tempFinReceiptCusHdrObj.RCH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        tempFinReceiptCusHdrObj.RCH_CRTD_BY = currentUser.PKUser;
                        serviceUtilityObj.FilterDate = string.IsNullOrEmpty(txtSearchDateFrom.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtSearchDateFrom.Text.Trim());
                        serviceUtilityObj.FilterToDate = string.IsNullOrEmpty(txtSearchDateTo.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtSearchDateTo.Text.Trim());
                        Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        PDCStatus = Convert.ToInt32(ddlPDCStatus.SelectedValue);
                        string invNo = null;
                        if (txtSINo.Text != null)
                        { invNo = txtSINo.Text.Trim(); }
                        else { invNo = ""; }

                        finReceiptCusHdrList = salesReceiptServiceClient.GetReceiptHdr(tempFinReceiptCusHdrObj, serviceUtilityObj, Status, invNo, PDCStatus);
                        TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 : (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) : (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        break;
                    #endregion
                    #region RECEIPT MPG LIST
                    case ControlsEnum.RECEIPTMPGLIST:
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        finInvoiceCusHdrList = salesReceiptServiceClient.GetReceiptTrxMpg(selectedInvoiceList);
                        EditedSalesInvoices = finInvoiceCusHdrList;
                        break;
                    #endregion
                    #region IS ADV DEDUCTED
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
                    #region CUSTOMER BANK
                    case ControlsEnum.CUSTOMERBANK:
                        crmCustomerMstList = new List<FIN_CASH_BANK_MST>();
                        CommonService cm = new CommonService();
                        int CusPk = hdfCusPK.Value != string.Empty ? Convert.ToInt32(hdfCusPK.Value) : 0;
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
                    #region Receipt Details List
                    case ControlsEnum.RECEIPTHDRINVLISTBYPK:
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        finReceiptCusTrxMpgList = salesReceiptServiceClient.GetReceiptTrxMpg(CurrPK);
                        EditedReceiptDtls = finReceiptCusTrxMpgList;
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
                        hdfReceiptNo.Value = salesReceiptServiceClient.GetReceiptNo(ApplicationType.CR, 0, currentUser.CurrentDeptPK,
                            string.IsNullOrEmpty(txtReceiptDate.Text) ? DateTime.Now : Convert.ToDateTime(txtReceiptDate.Text), currentUser.PKUser, updateReceipt, 0, Convert.ToInt32(ddlCompany.SelectedValue), currentUser.CurrentSBUPK);
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
                    case ControlsEnum.EXCHANGERATEINRECEIPTCURRENCY:
                        #region Get Exchange Rate in Receipt Currency
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        int fromReceiptCurrency = Convert.ToInt32(ddlReceiptCurrency.SelectedValue);
                        receiptDate = string.IsNullOrEmpty(txtReceiptDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtReceiptDate.Text.Trim());
                        //Get Receipt Currency to Base currency Converrsion factor
                        hdfExchangeCurrReceipt.Value = salesReceiptServiceClient.GetConversionFactor(
                                                             fromReceiptCurrency, currentUser.BaseCurrency,
                                                             receiptDate, SBUID).ToString();
                        hdfExchangeRate_RecCurrToBaseCurr.Value = hdfExchangeCurrReceipt.Value;
                        break;
                    #endregion

                    case ControlsEnum.GETMULTIPLEEXCHANGERATES:
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        int ReceiptCurrency = Convert.ToInt32(ddlReceiptCurrency.SelectedValue);
                        int TransactionCurrency = Convert.ToInt32(hdfReceiptCurrency.Value);
                        receiptDate = string.IsNullOrEmpty(txtReceiptDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtReceiptDate.Text.Trim());
                        //Get Receipt Currency to Transaction currency Converrsion factor
                        hdfExchangeRate_RecCurrToTrxCurr.Value = salesReceiptServiceClient.GetConversionFactor(
                                                             ReceiptCurrency, TransactionCurrency,
                                                             receiptDate, SBUID).ToString();
                        //Get Transaction currency to Receipt Currency Converrsion factor
                        hdfExchangeRate_TrxCurrToRecCurr.Value = salesReceiptServiceClient.GetConversionFactor(
                                                             TransactionCurrency, ReceiptCurrency,
                                                             receiptDate, SBUID).ToString();
                        break;

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
                            finTrxHdrObj.FTH_REF_TYPE = ApplicationType.CRJ;
                        }
                        else if (recipetType == 3)
                        {
                            finTrxHdrObj.FTH_REF_TYPE = ApplicationType.MSIRJ;
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
                            finTrxHdrObj.FTH_REF_TYPE = ApplicationType.PDCCJ;
                            finTrxHdrObj.FTH_REF_PK = CurrPK;
                        }

                        //finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrPDCList = finTrxServiceClient.GetSatusByAppPK(finTrxHdrObj);
                        break;
                    #endregion
                    #region RECEIPTADJNLIST DUMMY LIST
                    case ControlsEnum.RECEIPTADJNDUMMYLIST:

                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        FinReceiptCusAllocationObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_ALCN_DTL>();
                        FinReceiptCusAllocationList = salesReceiptServiceClient.GetFinReceiptAlcnListContext(ReceiptAdjnList.Where(dtl => dtl.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK).ToList());


                        break;
                    #endregion
                    #region PAYMENT SPLIT LIST
                    case ControlsEnum.PAYMENTSPLITLIST:
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        finReceiptCusSoMpgObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_SO_MPG>();
                        finReceiptCusSoMpgObj.RSO_RECEIPT_TRX_MPG = ReceiptMpgPK;
                        finReceiptCusSoMpgList = salesReceiptServiceClient.GetFinReceiptCusSoMpgList(finReceiptCusSoMpgObj);
                        break;
                    #endregion
                    #region RECEIPT ADJN LIST
                    case ControlsEnum.RECEIPTADJNLIST:
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        FinReceiptCusAllocationObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_ALCN_DTL>();
                        FinReceiptCusAllocationObj.RAD_RECEIPT_TRX = ReceiptMpgPK;
                        FinReceiptCusAllocationList = salesReceiptServiceClient.GetFinReceiptAlcnList(FinReceiptCusAllocationObj);
                        //ReceiptAdjnList = FinReceiptCusAllocationList;
                        if (FinReceiptCusAllocationList != null && FinReceiptCusAllocationList.Count > 0)
                        {
                            List<FIN_RECEIPT_CUS_ALCN_DTL> tempPaymentAdjnList = ReceiptAdjnList;
                            tempPaymentAdjnList.Where(dtl => dtl.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK)
                                .ToList().ForEach(dtl => tempPaymentAdjnList.Remove(dtl));
                            FinReceiptCusAllocationList.ForEach(dtl =>
                            {
                                //dtl.FIN_PAYMENT_VND_TRX_MPG = new FIN_PAYMENT_VND_TRX_MPG()
                                //{
                                //    PVM_PK = PaymentMpgPK,
                                //    PVM_INVOICE_HDR = InvoicePK
                                //};
                                tempPaymentAdjnList.Add(dtl);
                            });
                            ReceiptAdjnList = tempPaymentAdjnList;
                        }
                        break;
                    #endregion
                    #region INVOICE VND MPGLIST
                    case ControlsEnum.INVOICEVNDMPGLIST:
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        FinInvoiceCusTrxMpgObj = CommonFunctions.Initilize<FIN_INVOICE_CUS_TRX_MPG>();
                        FinInvoiceCusTrxMpgObj.ICM_INVOICE_HDR = InvoicePK;
                        FinInvoiceCusTrxMpgList = salesReceiptServiceClient.GetInvoiceTrxMpg(FinInvoiceCusTrxMpgObj);
                        break;
                    #endregion
                    #region INVOICE VND MPG LIST FOR AUTO ALCN
                    case ControlsEnum.INVOICEVNDMPGLISTFORAUTOALCN:
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        FinInvoiceCusTrxMpgObj = CommonFunctions.Initilize<FIN_INVOICE_CUS_TRX_MPG>();
                        FinInvoiceCusTrxMpgObj.ICM_INVOICE_HDR = InvoicePK;
                        FinInvoiceCusTrxMpgListForAutoAlcn = salesReceiptServiceClient.GetInvoiceTrxMpg(FinInvoiceCusTrxMpgObj);
                        break;
                    #endregion
                    #region RECEIPT CUS ADJN
                    case ControlsEnum.RECEIPTCUSADJN:
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);

                        CrDrAdjnList = salesReceiptServiceClient.GetCrDrAdjn(Convert.ToInt16(hdfCusPK.Value), CurrPK);
                        if (CrDrAdjnList != null)
                        {
                            FinReceiptCusAdjnDupCheckList = salesReceiptServiceClient.GetReceiptCusAdjn(Convert.ToInt16(hdfCusPK.Value));
                            if (FinReceiptCusAdjnDupCheckList != null && FinReceiptCusAdjnDupCheckList.Count > 0)
                            {
                                List<long?> lstInvNos = new List<long?>();
                                ReceiptAdjnList.ForEach(rr =>
                                {
                                    lstInvNos.Add(rr.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR);
                                });

                                List<long?> lstTRXNos = new List<long?>();
                                ReceiptAdjnList.ForEach(rr =>
                                {
                                    if (rr.RAD_RECEIPT_TRX > 0)
                                        lstTRXNos.Add(rr.RAD_RECEIPT_TRX);
                                });

                                CrDrAdjnList.ForEach(CrDtl =>
                                {
                                    if (FinReceiptCusAdjnDupCheckList.Where(recT => recT.RAD_ALCN_RECEIPT_TRX == CrDtl.RAA_TRXPK).Sum(a => a.RAD_AMOUNT) > 0)
                                    {
                                        CrDtl.RAA_AMOUNT_RCVD = lstTRXNos.Count > 0 ? FinReceiptCusAdjnDupCheckList
                                            .Where(d => !lstInvNos.Contains(d.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR)
                                                && !lstTRXNos.Contains(d.RAD_RECEIPT_TRX)).ToList()
                                            .Count > 0 ? (FinReceiptCusAdjnDupCheckList
                                            .Where(d => !lstInvNos.Contains(d.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR) && !lstTRXNos.Contains(d.RAD_RECEIPT_TRX)).ToList()
                                            .Select(ss => ss.RAD_ALCN_RECEIPT_TRX == CrDtl.RAA_TRXPK).ToList().Count > 0 ? FinReceiptCusAdjnDupCheckList
                                            .Where(d => !lstInvNos.Contains(d.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR) && !lstTRXNos.Contains(d.RAD_RECEIPT_TRX)).ToList()
                                            .Where(recT => recT.RAD_ALCN_RECEIPT_TRX == CrDtl.RAA_TRXPK).Sum(a => a.RAD_AMOUNT) : 0) : FinReceiptCusAdjnDupCheckList.Where(recT => recT.RAD_ALCN_RECEIPT_TRX == CrDtl.RAA_TRXPK)
                                            .Sum(a => a.RAD_AMOUNT) : FinReceiptCusAdjnDupCheckList.Where(recT => recT.RAD_ALCN_RECEIPT_TRX == CrDtl.RAA_TRXPK)
                                            .Sum(a => a.RAD_AMOUNT);

                                        CrDtl.RAA_AMOUNT_BAL = CrDtl.RAA_AMOUNT - CrDtl.RAA_AMOUNT_RCVD;
                                    }
                                    else if (FinReceiptCusAdjnDupCheckList.Where(recT => recT.RAD_ALCN_CDH == CrDtl.RAA_CRDRPK).Sum(a => a.RAD_AMOUNT) > 0)
                                    {
                                        //FinReceiptCusAdjnDupCheckList = FinReceiptCusAdjnDupCheckList.Where(d => !lstInvNos.Contains(d.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR) && !lstTRXNos.Contains(d.RAD_RECEIPT_TRX)).ToList();
                                        CrDtl.RAA_AMOUNT_RCVD = lstTRXNos.Count > 0 ? FinReceiptCusAdjnDupCheckList
                                            .Where(d => !lstInvNos.Contains(d.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR)
                                            && !lstTRXNos.Contains(d.RAD_RECEIPT_TRX)).ToList()
                                            .Count > 0 ? (FinReceiptCusAdjnDupCheckList
                                            .Where(d => !lstInvNos.Contains(d.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR) && !lstTRXNos.Contains(d.RAD_RECEIPT_TRX)).ToList()
                                            .Select(ss => ss.RAD_ALCN_CDH == CrDtl.RAA_CRDRPK).ToList().Count > 0 ? FinReceiptCusAdjnDupCheckList
                                            .Where(d => !lstInvNos.Contains(d.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR) && !lstTRXNos.Contains(d.RAD_RECEIPT_TRX)).ToList()
                                            .Where(recT => recT.RAD_ALCN_CDH == CrDtl.RAA_CRDRPK).Sum(a => a.RAD_AMOUNT) : 0) : FinReceiptCusAdjnDupCheckList
                                            .Where(recT => recT.RAD_ALCN_CDH == CrDtl.RAA_CRDRPK).Sum(a => a.RAD_AMOUNT) : FinReceiptCusAdjnDupCheckList
                                            .Where(recT => recT.RAD_ALCN_CDH == CrDtl.RAA_CRDRPK).Sum(a => a.RAD_AMOUNT);

                                        CrDtl.RAA_AMOUNT_BAL = CrDtl.RAA_AMOUNT - CrDtl.RAA_AMOUNT_RCVD;
                                    }
                                    else
                                    {
                                        CrDtl.RAA_AMOUNT_RCVD = 0;
                                        CrDtl.RAA_AMOUNT_BAL = CrDtl.RAA_AMOUNT;
                                    }
                                });
                            }
                            else
                            {
                                CrDrAdjnList.ForEach(CrDtl =>
                                {
                                    CrDtl.RAA_AMOUNT_RCVD = 0;
                                    CrDtl.RAA_AMOUNT_BAL = CrDtl.RAA_AMOUNT;
                                });
                            }
                        }
                        break;
                    #endregion
                    #region INVOICE VND HDR
                    case ControlsEnum.INVOICEVNDHDR:
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        finInvoiceVndHdrObjForPaymentSplit = CommonFunctions.Initilize<FIN_INVOICE_CUS_HDR>();
                        finInvoiceVndHdrObjForPaymentSplit.ICH_PK = InvoicePK;
                        finInvoiceVndHdrObjForPaymentSplit.ICH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finInvoiceVndHdrListForPaymentSplit = salesReceiptServiceClient.GetInvoiceCusHdrByPK(finInvoiceVndHdrObjForPaymentSplit);
                        break;
                    #endregion
                    #region RECEIPT SPLIT LIST BY RECEIPT PK
                    case ControlsEnum.RECEIPTSPLITLISTBYRECEIPTPK:
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        finReceiptCusSoMpgObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_SO_MPG>();
                        finReceiptCusSoMpgObj.RSO_RECEIPT_HDR = CurrPK;
                        finReceiptCusSoMpgList = salesReceiptServiceClient.GetFinReceiptCusSoMpgList(finReceiptCusSoMpgObj);
                        break;
                    #endregion
                    #region Payment Mode
                    case ControlsEnum.PAYMODE:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admConfigMstObj = new ADM_CONFIG_MST();
                        admConfigMstObj.CFG_PK = 0;
                        admConfigMstObj.CFG_TYPE = GetLocalResourceObject("PAYMENT_MODE").ToString();
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    #endregion
                    #region Fin Transaction Year
                    case ControlsEnum.FINPERIOD:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                        finYearMstList = finTrxServiceClient.GetCurrentFinPeriod(DateTime.Now, currentUser.SBUID);
                        break;
                    #endregion
                    #region GET RECEIPT PK BY JOURNAL PK
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
                    #region FILL WORKFLOW STATUS
                    case ControlsEnum.FILLWORKFLOWSTATUS:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        workflowStatusList = CommonServiceClient.GetWorkFlowStatus(ApplicationType.CR, null, Convert.ToByte(CommonConstants.ACTIVE));
                        break;
                    #endregion
                    #region ADJ TYPE
                    case ControlsEnum.DISCOUNTTYPE:
                        dtDiscountTypes = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.ReceiptAdjustments, (int)Adjustments.Receipt, 1, currentUser.SBUID);
                        break;
                    #endregion
                    #region NOTIFICATION TYPES
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
                    #region ALERT BASIS
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
                    #region ALERT TYPES
                    case ControlsEnum.ALERTTYPES:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admConstMstList = CommonServiceClient.GetConstMstValues(null, null, null, 16, 1, currentUser.SBUID);
                        break;
                    #endregion
                    #region NOTIFICATION DAYS
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
                    #region ALERT CONFIG
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
                    #region ALERT DETAILS
                    case ControlsEnum.ALERTLIST:
                        dsAlertList = BusinessLogic.AlertManagement.Alerts.GetAlertDetails(0, Convert.ToByte(DbActiveStatus.ACTIVE), null, appType, invPK, currentUser.SBUID, currentUser.PKUser, (int)AlertType.System);
                        break;
                    #endregion
                    #region BANK CURRENCY
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
                    #region PDC VOUCHER LIST
                    case ControlsEnum.PDCVOUCHERLIST:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_REF_TYPE = "PDCCJ";
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrObj.FTH_VOUCHER_NO = string.Empty;
                        pdcVoucherList = finTrxServiceClient.GetPDCVoucherList(finTrxHdrObj);
                        break;
                    #endregion
                    #region CHECK INSTR NO
                    case ControlsEnum.CHKINSTRNO:
                        //Check Instr No
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        string instrNo = HttpUtility.HtmlEncode(txtInstrumentNo.Text.Trim());
                        InstrNoResult = Convert.ToBoolean(salesReceiptServiceClient.GetReceiptInstrNo(instrNo, CurrPK));
                        break;
                    #endregion
                    #region CR/DR ALLOCATION
                    case ControlsEnum.CRDRALLOCATION:
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        List<long> SelectedInvoicePks = new List<long>();
                        if (CurrPK <= 0)
                        {
                            //foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                            //{
                            //    HiddenField hdfInvoicePK = (HiddenField)grdrow.FindControl("hdfInvoicePK");
                            //    long InvoicePk = 0;
                            //    long.TryParse(hdfInvoicePK.Value, out InvoicePk);
                            //    SelectedInvoicePks.Add(InvoicePk);
                            //}
                            if (finInvoiceCusHdrList != null)
                            {
                                foreach (FIN_INVOICE_CUS_HDR objInvoiceDtl in finInvoiceCusHdrList)
                                {
                                    SelectedInvoicePks.Add(objInvoiceDtl.ICH_PK);
                                }
                            }
                        }
                        ReceiptCrdrList = salesReceiptServiceClient.GetCrDrAllocations(SelectedInvoicePks, CurrPK);
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
                    #region Check Receipt Allocation Invoice
                    case ControlsEnum.CheckReceiptAllocation:
                        dtReceiptAlloc = BusinessLogic.Sales.SaleOrderBL.CheckReceiptAllocationInvoice(CurrPK);
                        break;
                    #endregion
                    #region SUSPENCE LIST
                    case ControlsEnum.SUSPENCELIST:
                        dtSuspenceList = BusinessLogic.Sales.SaleOrderBL.GetSuspenceListforReciept(Convert.ToInt32(hdfBank.Value), CurrPK);
                        break;
                    #endregion

                    #region Currency
                    case ControlsEnum.CURRENCY:
                        dtCurrency = BusinessLogic.Sales.SaleOrderBL.GetCurrency(currentUser, currentUser.SBUID);
                        break;
                    #endregion
                    #region GetDebitCreditPost
                    case ControlsEnum.CHECKCREDITDEBITPOST:
                        ValidateId = BusinessLogic.Sales.SaleOrderBL.GetDebitCreditPost(PKXml);
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
                    #region RECEIPT MPG LIST
                    case ControlsEnum.RECEIPTMPGLIST:
                        BindGrid(ControlsEnum.RECEIPTMPGLIST);
                        break;
                    #endregion
                    #region PAYMENT SPLIT LIST
                    case ControlsEnum.PAYMENTSPLITLIST:
                        BindGrid(ControlsEnum.PAYMENTSPLITLIST);
                        break;
                    #endregion
                    #region RECEIPT CUS ADJN
                    case ControlsEnum.RECEIPTCUSADJN:
                        BindGrid(ControlsEnum.RECEIPTCUSADJN);
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
                    #region CR DR ALLOCATION
                    case ControlsEnum.CRDRALLOCATION:
                        BindGrid(ControlsEnum.CRDRALLOCATION);
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
                    #region GET CURRENCY
                    case ControlsEnum.CURRENCY:
                        BindDropDown(ControlsEnum.CURRENCY);
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
                AlertBO alertBoObj;
                TextBox txtOtherChargesSplit;
                HiddenField hdfTaxSplit;
                HiddenField hdfOtherChargesSplit;
                DateTime? InstrDate = null;

                switch (controlType)
                {
                    #region Receipt Hdr
                    case ControlsEnum.RECEIPTHDRENTRY:
                        finReceiptCusHdrObj.RCH_PK = CurrPK;
                        finReceiptCusHdrObj.RCH_NO = (string.IsNullOrEmpty(lblReceiptNo.Text) || lblReceiptNo.Text.Trim().Equals("[NEW]")) ? string.Empty
                                                    : lblReceiptNo.Text.Trim();
                        finReceiptCusHdrObj.RCH_DATE = String.IsNullOrEmpty(txtReceiptDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtReceiptDate.Text.Trim());
                        finReceiptCusHdrObj.RCH_CUSTOMER = string.IsNullOrEmpty(hdfCustomerPK.Value) ? 1 : Convert.ToInt32(hdfCustomerPK.Value);
                        finReceiptCusHdrObj.RCH_CATEGORY = (byte)SICategory;
                        finReceiptCusHdrObj.RCH_GROUP = (byte)SOGroup;
                        finReceiptCusHdrObj.RCH_CUSTOMER_ACCOUNT = string.IsNullOrEmpty(hdfCustomerAccountNo.Value) ? null : ERP.Utilities.CommonFunctions.NullableInt(hdfCustomerAccountNo.Value);//Convert.ToInt32(ddlAccountNo.SelectedValue);
                        finReceiptCusHdrObj.RCH_MODE = ddlMode.SelectedValue == "-1" ? Convert.ToByte(0) : Convert.ToByte(ddlMode.SelectedValue);
                        finReceiptCusHdrObj.RCH_BANK = string.IsNullOrEmpty(hdfBank.Value) ? (short?)null : Convert.ToInt16(hdfBank.Value);
                        finReceiptCusHdrObj.RCH_OTHER_AMOUNT = Convert.ToDecimal(hdfTotalOtherCharges.Value.Trim());
                        if (Convert.ToInt32(ddlMode.SelectedValue) != (int)ReceiptModeEnum.CASH)
                        {

                            finReceiptCusHdrObj.RCH_INSTR_NO = HttpUtility.HtmlEncode(txtInstrumentNo.Text.Trim());
                            finReceiptCusHdrObj.RCH_INSTR_DATE = String.IsNullOrEmpty(txtInstrumentDate.Text.Trim()) ? ((Convert.ToInt32(ddlMode.SelectedValue) == (int)ReceiptModeEnum.OTHERS) ? InstrDate : DateTime.Now) : Convert.ToDateTime(txtInstrumentDate.Text.Trim());
                        }
                        finReceiptCusHdrObj.RCH_BANK_CASH_ACCOUNT = string.IsNullOrEmpty(hdfBankAccount.Value) ? (int?)null : Convert.ToInt32(hdfBankAccount.Value);//1;//Convert.ToInt32(ddlAccountNo.SelectedValue);
                        finReceiptCusHdrObj.RCH_CURRENCY = string.IsNullOrEmpty(hdfReceiptCurrency.Value) ? 1 : Convert.ToInt32(hdfReceiptCurrency.Value);
                        finReceiptCusHdrObj.RCH_RCVD_AMOUNT = Convert.ToDecimal(txtReceivedAmount.Text.Trim());
                        finReceiptCusHdrObj.RCH_BANK_OF_CHEQUE = HttpUtility.HtmlEncode(txtbankOfCheque.Text.Trim());
                        finReceiptCusHdrObj.RCH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                        finReceiptCusHdrObj.RCH_BASE_CURR = currentUser.BaseCurrency;
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        GetFieldValues(ControlsEnum.EXCHANGERATEINBASECURRENCY);
                        finReceiptCusHdrObj.RCH_EXCHG_RATE = string.IsNullOrEmpty(hdfExchangeCurrBC.Value) ? 1 : Convert.ToDouble(hdfExchangeCurrBC.Value);
                        finReceiptCusHdrObj.RCH_RCVD_AMOUNT_BC = Convert.ToDecimal(txtReceivedAmount.Text.Trim()) * Convert.ToDecimal(finReceiptCusHdrObj.RCH_EXCHG_RATE);
                        finReceiptCusHdrObj.RCH_STATUS = WkfStatus;
                        finReceiptCusHdrObj.RCH_DEL_STATUS = Convert.ToByte(hdfDelStatus.Value);
                        finReceiptCusHdrObj.RCH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finReceiptCusTrxMpgList = new List<FIN_RECEIPT_CUS_TRX_MPG>();
                        finReceiptCusTrxMpgList = (List<FIN_RECEIPT_CUS_TRX_MPG>)SetUIValuesToObject(ControlsEnum.RECEIPTMPGENTRY);

                        ////////////// For default split allocaation apply ////////////////
                        foreach (FIN_RECEIPT_CUS_TRX_MPG receiptmpg in finReceiptCusTrxMpgList)
                        {

                            if (AppliedInvPkList == null || !AppliedInvPkList.Contains(Convert.ToInt64(receiptmpg.RCM_INVOICE_HDR)))
                            {
                                InvoiceDetails(Convert.ToInt64(receiptmpg.RCM_INVOICE_HDR));
                                ReceiptSplitSave(false);
                            }

                        }
                        //////////////////////////////////////////////////////////////////

                        finReceiptCusHdrObj.RCH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        finReceiptCusHdrObj.RCH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        finReceiptCusHdrObj.RCH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                        finReceiptCusHdrObj.RCH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                        finReceiptCusHdrObj.RCH_CRTD_DT = DateTime.Now;
                        finReceiptCusHdrObj.RCH_MOD_DT = LastModifiedTime;
                        finReceiptCusHdrObj.RCH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);
                        //
                        if (ddlAdjType.SelectedValue == CommonConstants.SELECTVAL)
                            finReceiptCusHdrObj.RCH_DISCOUNT = null;
                        else
                            finReceiptCusHdrObj.RCH_DISCOUNT = Convert.ToInt32(ddlAdjType.SelectedValue);
                        if (Convert.ToInt32(ddlMode.SelectedValue) == (int)ReceiptModeEnum.CHEQUE)
                        {
                            finReceiptCusHdrObj.RCH_PDC = chkPDC.Checked == true ? (byte)1 : (byte)0;
                        }
                        else
                        {
                            finReceiptCusHdrObj.RCH_PDC = 0;
                        }
                        finReceiptCusHdrObj.RCH_DISC_AMOUNT = txtAdjAmount.Text != string.Empty ? Convert.ToDecimal(txtAdjAmount.Text) : 0;
                        finReceiptCusHdrObj.RCH_TAX_AMOUNT = hdfTottaxHDR.Value != string.Empty ? Convert.ToDecimal(hdfTottaxHDR.Value) : 0;
                        if (ddlBankChargeCurrency.Items.Count > 0)
                            finReceiptCusHdrObj.RCH_BANK_CHARGE_CURR = Convert.ToInt32(ddlBankChargeCurrency.SelectedValue);
                        else
                            finReceiptCusHdrObj.RCH_BANK_CHARGE_CURR = null;
                        finReceiptCusHdrObj.RCH_BANK_CHARGE = txtBankCharge.Text != string.Empty ? Convert.ToDecimal(txtBankCharge.Text) : 0;

                        //****************Return Region*****************
                        finReceiptCusHdrObj.RCH_RETURN_STATUS = Convert.ToByte(chkReturn.Checked);
                        finReceiptCusHdrObj.RCH_RETURN_DATE = String.IsNullOrEmpty(txtReturnDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtReturnDate.Text.Trim());
                        finReceiptCusHdrObj.RCH_RETURN_REMARKS = HttpUtility.HtmlEncode(txtReturnRemarks.Text.Trim());
                        //**********End return*******************************

                        if (finReceiptCusTrxMpgList != null && finReceiptCusTrxMpgList.Count > 0)
                        {
                            finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_RECEIPT_CUS_TRX_MPG>();
                            //finReceiptCusTrxMpgList.ForEach(dtl => finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG.Add(dtl));
                            finReceiptCusTrxMpgList.ForEach(dtl =>
                            {
                                ///finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG.Add(dtl);
                                finReceiptCusSoMpgList = InvoiceSOSplitList.Where(mpg => mpg.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == dtl.RCM_INVOICE_HDR).ToList();
                                if (finReceiptCusSoMpgList.Count() > 0)
                                {
                                    finReceiptCusSoMpgList.ForEach(mpg =>
                                    {
                                        mpg.FIN_RECEIPT_CUS_TRX_MPG = null;
                                        dtl.FIN_RECEIPT_CUS_SO_MPG.Add(mpg);
                                        //if (finReceiptCusSoMpgList.Count == 1 && mpg.RSO_RECEIVED_AMOUNT == 0)
                                        //{
                                        //    mpg.RSO_RECEIVED_AMOUNT = dtl.RCM_RCVD_AMOUNT;
                                        //    mpg.RSO_TAX_AMOUNT = dtl.RCM_TAX_AMOUNT;
                                        //    mpg.RSO_OTHER_AMOUNT = dtl.RCM_OTHER_AMOUNT;
                                        //}
                                        //else if (finReceiptCusSoMpgList.Count > 1 && (mpg.FIN_RECEIPT_CUS_TRX_MPG.RCM_RCVD_AMOUNT != finReceiptCusSoMpgList.Sum(d => d.RSO_RECEIVED_AMOUNT)))
                                        //{
                                        //    isSplitApply = 5;//Should click apply
                                        //}
                                    });
                                }
                                else if (CurrPK == 0 && dtl.RCM_INVOICE_HDR.HasValue)
                                {
                                    InvoicePK = dtl.RCM_INVOICE_HDR.Value;
                                    GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);

                                    FIN_RECEIPT_CUS_SO_MPG finReceiptCusSoMpgObj = new FIN_RECEIPT_CUS_SO_MPG();

                                    if (FinInvoiceCusTrxMpgList != null && FinInvoiceCusTrxMpgList.Count == 1)
                                    {
                                        isSplitApply = 0;
                                        dtl.FIN_RECEIPT_CUS_SO_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_RECEIPT_CUS_SO_MPG>();
                                        finReceiptCusSoMpgObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_SO_MPG>();
                                        finReceiptCusSoMpgObj.RSO_PK = 0;
                                        finReceiptCusSoMpgObj.RSO_SO_HDR = FinInvoiceCusTrxMpgList.First().ICM_SO_HDR;
                                        finReceiptCusSoMpgObj.RSO_RECEIVED_AMOUNT = dtl.RCM_RCVD_AMOUNT;
                                        finReceiptCusSoMpgObj.RSO_TAX_AMOUNT = dtl.RCM_TAX_AMOUNT;
                                        finReceiptCusSoMpgObj.RSO_OTHER_AMOUNT = dtl.RCM_OTHER_AMOUNT;
                                        finReceiptCusSoMpgObj.RSO_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);

                                        finRecCusTaxDtlObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_TAX_DTL>();
                                        finRecCusTaxDtlObj.RDT_PK = 0;
                                        finReceiptCusSoMpgObj.FIN_RECEIPT_CUS_TAX_DTL.Add(finRecCusTaxDtlObj);

                                        dtl.FIN_RECEIPT_CUS_SO_MPG.Add(finReceiptCusSoMpgObj);
                                        finReceiptCusSoMpgList = dtl.FIN_RECEIPT_CUS_SO_MPG.ToList();// finReceiptCusHdrObj.FIN_RECEIPT_CUS_SO_MPG.ToList();// finReceiptCusHdrObj.FIN_RECEIPT_CUS_SO_MPG.ToList();
                                    }
                                    //else if (FinInvoiceCusTrxMpgList != null && FinInvoiceCusTrxMpgList.Count > 1 )
                                    //{
                                    //    isSplitApply = 5;//Should click apply
                                    //}
                                }

                                finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG.Add(dtl);
                            });
                        }

                        //Adjn and CR/DR Allocation
                        if (finReceiptCusTrxMpgList != null && finReceiptCusTrxMpgList.Count > 0)
                        {
                            finReceiptCusTrxMpgList.ForEach(dtl =>
                            {
                                finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG.Add(dtl);
                                #region Adjustment Allocation
                                FinReceiptCusAllocationList = ReceiptAdjnList.Where(mpg => mpg.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == dtl.RCM_INVOICE_HDR).ToList();
                                if (FinReceiptCusAllocationList.Count() > 0)
                                {
                                    FinReceiptCusAllocationList.ForEach(mpg =>
                                    {
                                        //if (mpg.RAD_AMOUNT > 0)
                                        //{
                                        FinReceiptCusAllocationObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_ALCN_DTL>();
                                        FinReceiptCusAllocationObj.RAD_PK = mpg.RAD_PK;
                                        FinReceiptCusAllocationObj.RAD_RECEIPT_TRX = mpg.RAD_RECEIPT_TRX;
                                        FinReceiptCusAllocationObj.RAD_ALCN_CDH = mpg.RAD_ALCN_CDH;
                                        FinReceiptCusAllocationObj.RAD_ALCN_RECEIPT_TRX = mpg.RAD_ALCN_RECEIPT_TRX;
                                        FinReceiptCusAllocationObj.RAD_AMOUNT = mpg.RAD_AMOUNT;
                                        FinReceiptCusAllocationObj.RAD_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                        //  }
                                        dtl.FIN_RECEIPT_CUS_ALCN_DTL.Add(FinReceiptCusAllocationObj);

                                    });
                                }
                                #endregion

                                #region CR/DR Allocation
                                FinReceiptCusCrdrAllocationList = ReceiptCrdrList.Where(mpg => mpg.RNM_INVOICE_HDR == dtl.RCM_INVOICE_HDR && (mpg.RNM_PAID_AMOUNT > 0 || mpg.RNM_ADJ_AMOUNT > 0)).ToList();
                                if (FinReceiptCusCrdrAllocationList != null && FinReceiptCusCrdrAllocationList.Count() > 0)
                                {
                                    FinReceiptCusCrdrAllocationList.ForEach(mpg =>
                                    {
                                        FinReceiptCusCrdrAllocationObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_CRDR_MPG>();
                                        FinReceiptCusCrdrAllocationObj.RNM_ACTIVE = mpg.RNM_ACTIVE;
                                        FinReceiptCusCrdrAllocationObj.RNM_ADJ_AMOUNT = mpg.RNM_ADJ_AMOUNT;
                                        FinReceiptCusCrdrAllocationObj.RNM_CRDR_HDR = mpg.RNM_CRDR_HDR;
                                        FinReceiptCusCrdrAllocationObj.RNM_PAID_AMOUNT = mpg.RNM_PAID_AMOUNT;
                                        FinReceiptCusCrdrAllocationObj.RNM_RECEIPT_HDR = mpg.RNM_RECEIPT_HDR;
                                        FinReceiptCusCrdrAllocationObj.RNM_RECEIPT_TRX_MPG = mpg.RNM_RECEIPT_TRX_MPG;
                                        FinReceiptCusCrdrAllocationObj.RNM_PK = mpg.RNM_PK;
                                        FinReceiptCusCrdrAllocationObj.RNM_CRDR_MPG = mpg.RNM_CRDR_MPG;
                                        dtl.FIN_RECEIPT_CUS_CRDR_MPG.Add(FinReceiptCusCrdrAllocationObj);
                                    });
                                }
                                #endregion
                            });
                        }


                        if (finReceiptCusHdrObj != null)
                        {
                            finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG.ToList().ForEach(dtl =>
                            {
                                dtl.FIN_RECEIPT_CUS_SO_MPG.ToList().ForEach(mpg =>
                                {
                                    finReceiptCusTaxDtlObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_TAX_DTL>();
                                    finReceiptCusTaxDtlObj.RDT_PK = 0;
                                    if (mpg.FIN_RECEIPT_CUS_TAX_DTL.Count == 0)
                                        mpg.FIN_RECEIPT_CUS_TAX_DTL.Add(finReceiptCusTaxDtlObj);// finReceiptCusTaxDtlList.Add(finReceiptCusTaxDtlObj);
                                });
                            });
                        }

                        //if (finReceiptCusSoMpgList != null && finReceiptCusSoMpgList.Count > 0)
                        //{
                        //    //finReceiptCusHdrObj.FIN_RECEIPT_CUS_SO_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_RECEIPT_CUS_SO_MPG>();
                        //    //finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_RECEIPT_CUS_TRX_MPG>(); 
                        //    finReceiptCusSoMpgList.ForEach(dtl =>
                        //    {
                        //        //finReceiptCusHdrObj.FIN_RECEIPT_CUS_SO_MPG.Add(dtl);
                        //        // dtl.FIN_RECEIPT_CUS_TAX_DTL = new System.Data.Objects.DataClasses.EntityCollection<FIN_RECEIPT_CUS_TAX_DTL>();
                        //        finReceiptCusTaxDtlObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_TAX_DTL>();
                        //        finReceiptCusTaxDtlObj.RDT_PK = 0;
                        //        dtl.FIN_RECEIPT_CUS_TAX_DTL.Add(finReceiptCusTaxDtlObj);// finReceiptCusTaxDtlList.Add(finReceiptCusTaxDtlObj);
                        //    });
                        //}

                        if (finReceiptSuspList != null && finReceiptSuspList.Count > 0)
                        {
                            finReceiptSuspList.ForEach(x =>
                            {
                                finReceiptCusHdrObj.FIN_RECEIPT_CUS_SUSP_DTL.Add(x);
                            });
                        }
                        retObject = finReceiptCusHdrObj;
                        break;
                    #endregion
                    #region Receipt Maping
                    case ControlsEnum.RECEIPTMPGENTRY:
                        rowID = 0;
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            decimal taxpercentage = 0;
                            decimal basevalue = 0;
                            decimal ttaxamt = 0;
                            decimal hdftax = 0;
                            decimal hdftotalamt = 0;

                            TextBox txtOtherCharges;


                            finReceiptCusTrxMpgObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_TRX_MPG>();
                            hdfReceiptMpgPK = (HiddenField)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("hdfReceiptMpgPK").ToString());
                            hdfInvoicePK = (HiddenField)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("hdfInvoicePK").ToString());
                            HiddenField hdfTaxAmt = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTaxAmt");
                            HiddenField hdfTotalAmt = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTotalAmt");
                            HiddenField hdfTotalTax = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTotalTax");

                            Label lblBaltoReceive = (Label)grdInvoiceList.Rows[rowID].FindControl("lblBaltoReceive");
                            Label lblTotalAmount = (Label)grdInvoiceList.Rows[rowID].FindControl("lblTotalAmount");
                            lblTax = (Label)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("lblTax").ToString());
                            lblAdjAmount = (Label)grdInvoiceList.Rows[rowID].FindControl("lblAdjAmount");
                            lblOtherchargesFooter = (Label)grdInvoiceList.Rows[rowID].FindControl("lblOtherchargesFooter");

                            txtOtherCharges = (TextBox)grdInvoiceList.Rows[rowID].FindControl("txtOtherCharges");
                            txtAmount = (TextBox)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("txtReceivedNow").ToString());
                            txtAdjustments = (TextBox)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("txtAdjustments").ToString());


                            finReceiptCusTrxMpgObj.RCM_PK = hdfReceiptMpgPK == null ? 0 : Convert.ToInt64(hdfReceiptMpgPK.Value);
                            finReceiptCusTrxMpgObj.RCM_RECEIPT_HDR = CurrPK;
                            finReceiptCusTrxMpgObj.RCM_INVOICE_HDR = hdfInvoicePK == null ? (long?)null : Convert.ToInt32(hdfInvoicePK.Value);
                            finReceiptCusTrxMpgObj.RCM_RCVD_AMOUNT = txtAmount == null ? 0 : txtAmount.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtAmount.Text.Trim());
                            finReceiptCusTrxMpgObj.RCM_DISC_AMOUNT = txtAdjustments == null ? 0 : txtAdjustments.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtAdjustments.Text.Trim());


                            hdftax = hdfTaxAmt.Value != string.Empty ? Convert.ToDecimal(hdfTaxAmt.Value) : 0;
                            hdftotalamt = hdfTotalAmt.Value != string.Empty ? Convert.ToDecimal(hdfTotalAmt.Value) : 0;
                            taxpercentage = hdftax / (hdftotalamt == 0 ? 1 : hdftotalamt);
                            ttaxamt = Convert.ToDecimal(hdfTotalTax.Value);
                            finReceiptCusTrxMpgObj.RCM_TAX_AMOUNT = Math.Round(ttaxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            finReceiptCusTrxMpgObj.RCM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            finReceiptCusTrxMpgObj.RCM_OTHER_AMOUNT = txtOtherCharges == null ? 0 : txtOtherCharges.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtOtherCharges.Text.Trim());
                            finReceiptCusTrxMpgObj.RCM_ADJUST_AMOUNT = lblAdjAmount == null ? 0 : lblAdjAmount.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(lblAdjAmount.Text.Trim());
                            lblBaltoReceive.Text = lblBaltoReceive.Text.Replace(",", "");
                            decimal excessAmt = (finReceiptCusTrxMpgObj.RCM_RCVD_AMOUNT) - Convert.ToDecimal(lblBaltoReceive.Text);
                            finReceiptCusTrxMpgObj.RCM_EXCESS_AMOUNT = excessAmt > 0 ? excessAmt : 0;

                            //if (finReceiptCusTrxMpgObj.RCM_RCVD_AMOUNT > 0)
                            //{
                            finReceiptCusTrxMpgList.Add(finReceiptCusTrxMpgObj);
                            //}
                            rowID++;
                        }
                        retObject = finReceiptCusTrxMpgList;
                        break;
                    #endregion
                    #region WRKFSUBMIT
                    case ControlsEnum.WRKFSUBMIT:
                        finReceiptCusHdrObj.RCH_PK = CurrPK;
                        updateReceipt = true;

                        finReceiptCusHdrObj.RCH_DATE = String.IsNullOrEmpty(txtReceiptDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtReceiptDate.Text.Trim());
                        finReceiptCusHdrObj.RCH_CATEGORY = (byte)SICategory;
                        finReceiptCusHdrObj.RCH_GROUP = (byte)SOGroup;
                        finReceiptCusHdrObj.RCH_OTHER_AMOUNT = Convert.ToDecimal(hdfTotalOtherCharges.Value.Trim());

                        finReceiptCusHdrObj.RCH_CUSTOMER = string.IsNullOrEmpty(hdfCustomerPK.Value) ? 1 : Convert.ToInt32(hdfCustomerPK.Value);
                        finReceiptCusHdrObj.RCH_CUSTOMER_ACCOUNT = string.IsNullOrEmpty(hdfCustomerAccountNo.Value) ? null : ERP.Utilities.CommonFunctions.NullableInt(hdfCustomerAccountNo.Value);//Convert.ToInt32(ddlAccountNo.SelectedValue);
                        finReceiptCusHdrObj.RCH_MODE = ddlMode.SelectedValue == "-1" ? Convert.ToByte(0) : Convert.ToByte(ddlMode.SelectedValue);
                        finReceiptCusHdrObj.RCH_BANK = string.IsNullOrEmpty(hdfBank.Value) ? (short?)null : Convert.ToInt16(hdfBank.Value);
                        if (Convert.ToInt32(ddlMode.SelectedValue) != (int)ReceiptModeEnum.CASH)
                        {

                            finReceiptCusHdrObj.RCH_INSTR_NO = HttpUtility.HtmlEncode(txtInstrumentNo.Text.Trim());
                            finReceiptCusHdrObj.RCH_INSTR_DATE = String.IsNullOrEmpty(txtInstrumentDate.Text.Trim()) ? ((Convert.ToInt32(ddlMode.SelectedValue) == (int)ReceiptModeEnum.OTHERS) ? InstrDate : DateTime.Now) : Convert.ToDateTime(txtInstrumentDate.Text.Trim());
                        }
                        finReceiptCusHdrObj.RCH_BANK_CASH_ACCOUNT = string.IsNullOrEmpty(hdfBankAccount.Value) ? (int?)null : Convert.ToInt32(hdfBankAccount.Value);//1;//Convert.ToInt32(ddlAccountNo.SelectedValue);
                        finReceiptCusHdrObj.RCH_CURRENCY = string.IsNullOrEmpty(hdfReceiptCurrency.Value) ? 1 : Convert.ToInt32(hdfReceiptCurrency.Value);
                        finReceiptCusHdrObj.RCH_RCVD_AMOUNT = Convert.ToDecimal(txtReceivedAmount.Text.Trim());
                        finReceiptCusHdrObj.RCH_BANK_OF_CHEQUE = HttpUtility.HtmlEncode(txtbankOfCheque.Text.Trim());
                        finReceiptCusHdrObj.RCH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                        finReceiptCusHdrObj.RCH_BASE_CURR = currentUser.BaseCurrency;
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        GetFieldValues(ControlsEnum.EXCHANGERATEINBASECURRENCY);
                        finReceiptCusHdrObj.RCH_EXCHG_RATE = string.IsNullOrEmpty(hdfExchangeCurrBC.Value) ? 1 : Convert.ToDouble(hdfExchangeCurrBC.Value);
                        finReceiptCusHdrObj.RCH_RCVD_AMOUNT_BC = Convert.ToDecimal(txtReceivedAmount.Text.Trim()) * Convert.ToDecimal(finReceiptCusHdrObj.RCH_EXCHG_RATE);
                        finReceiptCusHdrObj.RCH_STATUS = WkfStatus;
                        finReceiptCusHdrObj.RCH_DEL_STATUS = Convert.ToByte(hdfDelStatus.Value);
                        finReceiptCusHdrObj.RCH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finReceiptCusTrxMpgList = new List<FIN_RECEIPT_CUS_TRX_MPG>();
                        finReceiptCusTrxMpgList = (List<FIN_RECEIPT_CUS_TRX_MPG>)SetUIValuesToObject(ControlsEnum.RECEIPTMPGENTRY);

                        ////////////// For default split allocaation apply ////////////////
                        foreach (FIN_RECEIPT_CUS_TRX_MPG receiptmpg in finReceiptCusTrxMpgList)
                        {

                            if (AppliedInvPkList == null || !AppliedInvPkList.Contains(Convert.ToInt64(receiptmpg.RCM_INVOICE_HDR)))
                            {
                                InvoiceDetails(Convert.ToInt64(receiptmpg.RCM_INVOICE_HDR));
                                ReceiptSplitSave(false);
                            }

                        }
                        //////////////////////////////////////////////////////////////////

                        finReceiptCusHdrObj.RCH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        finReceiptCusHdrObj.RCH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        finReceiptCusHdrObj.RCH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                        finReceiptCusHdrObj.RCH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                        finReceiptCusHdrObj.RCH_CRTD_DT = DateTime.Now;
                        finReceiptCusHdrObj.RCH_MOD_DT = LastModifiedTime;

                        finReceiptCusHdrObj.RCH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);

                        if (ddlAdjType.SelectedValue == CommonConstants.SELECTVAL)
                            finReceiptCusHdrObj.RCH_DISCOUNT = null;
                        else
                            finReceiptCusHdrObj.RCH_DISCOUNT = Convert.ToInt32(ddlAdjType.SelectedValue);
                        if (Convert.ToInt32(ddlMode.SelectedValue) == (int)ReceiptModeEnum.CHEQUE)
                        {
                            finReceiptCusHdrObj.RCH_PDC = chkPDC.Checked == true ? (byte)1 : (byte)0;
                        }
                        else
                        {
                            finReceiptCusHdrObj.RCH_PDC = 0;
                        }
                        finReceiptCusHdrObj.RCH_DISC_AMOUNT = txtAdjAmount.Text != string.Empty ? Convert.ToDecimal(txtAdjAmount.Text) : 0;
                        finReceiptCusHdrObj.RCH_TAX_AMOUNT = hdfTottaxHDR.Value != string.Empty ? Convert.ToDecimal(hdfTottaxHDR.Value) : 0;
                        if (ddlBankChargeCurrency.Items.Count > 0)
                            finReceiptCusHdrObj.RCH_BANK_CHARGE_CURR = Convert.ToInt32(ddlBankChargeCurrency.SelectedValue);
                        else
                            finReceiptCusHdrObj.RCH_BANK_CHARGE_CURR = null;
                        finReceiptCusHdrObj.RCH_BANK_CHARGE = txtBankCharge.Text != string.Empty ? Convert.ToDecimal(txtBankCharge.Text) : 0;

                        //****************Return Region*****************
                        finReceiptCusHdrObj.RCH_RETURN_STATUS = Convert.ToByte(chkReturn.Checked);
                        finReceiptCusHdrObj.RCH_RETURN_DATE = String.IsNullOrEmpty(txtReturnDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtReturnDate.Text.Trim());
                        finReceiptCusHdrObj.RCH_RETURN_REMARKS = HttpUtility.HtmlEncode(txtReturnRemarks.Text.Trim());
                        //**********End return*******************************

                        if (finReceiptCusTrxMpgList != null && finReceiptCusTrxMpgList.Count > 0)
                        {
                            finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_RECEIPT_CUS_TRX_MPG>();
                            //finReceiptCusTrxMpgList.ForEach(dtl => finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG.Add(dtl));
                            finReceiptCusTrxMpgList.ForEach(dtl =>
                            {
                                //finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG.Add(dtl);
                                finReceiptCusSoMpgList = InvoiceSOSplitList.Where(mpg => mpg.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == dtl.RCM_INVOICE_HDR).ToList();
                                if (finReceiptCusSoMpgList.Count() > 0)
                                {
                                    finReceiptCusSoMpgList.ForEach(mpg =>
                                    {
                                        mpg.FIN_RECEIPT_CUS_TRX_MPG = null;
                                        dtl.FIN_RECEIPT_CUS_SO_MPG.Add(mpg);
                                        //if (finReceiptCusSoMpgList.Count == 1)// && mpg.RSO_RECEIVED_AMOUNT == 0) bcz while edit the amt in RSO_RECEIVED_AMOUNT is diff from RCM_RCVD_AMOUNT,than we should replace the RSO_RECEIVED_AMOUNT,so commented 0 amt checking//bugid:1963
                                        //{
                                        //    mpg.RSO_RECEIVED_AMOUNT = dtl.RCM_RCVD_AMOUNT;
                                        //    mpg.RSO_TAX_AMOUNT = dtl.RCM_TAX_AMOUNT;
                                        //    mpg.RSO_OTHER_AMOUNT = dtl.RCM_OTHER_AMOUNT;
                                        //}

                                    });
                                }
                                else if (CurrPK == 0 && dtl.RCM_INVOICE_HDR.HasValue)
                                {
                                    InvoicePK = dtl.RCM_INVOICE_HDR.Value;
                                    GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);
                                    if (FinInvoiceCusTrxMpgList != null && FinInvoiceCusTrxMpgList.Count == 1)
                                    {
                                        isSplitApply = 0;
                                        dtl.FIN_RECEIPT_CUS_SO_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_RECEIPT_CUS_SO_MPG>();
                                        finReceiptCusSoMpgObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_SO_MPG>();
                                        finReceiptCusSoMpgObj.RSO_PK = 0;
                                        finReceiptCusSoMpgObj.RSO_SO_HDR = FinInvoiceCusTrxMpgList.First().ICM_SO_HDR;
                                        finReceiptCusSoMpgObj.RSO_RECEIVED_AMOUNT = dtl.RCM_RCVD_AMOUNT;
                                        finReceiptCusSoMpgObj.RSO_TAX_AMOUNT = dtl.RCM_TAX_AMOUNT;
                                        finReceiptCusSoMpgObj.RSO_OTHER_AMOUNT = dtl.RCM_OTHER_AMOUNT;
                                        finReceiptCusSoMpgObj.RSO_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);

                                        finRecCusTaxDtlObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_TAX_DTL>();
                                        finRecCusTaxDtlObj.RDT_PK = 0;
                                        finReceiptCusSoMpgObj.FIN_RECEIPT_CUS_TAX_DTL.Add(finRecCusTaxDtlObj);

                                        dtl.FIN_RECEIPT_CUS_SO_MPG.Add(finReceiptCusSoMpgObj);
                                        finReceiptCusSoMpgList = dtl.FIN_RECEIPT_CUS_SO_MPG.ToList();
                                    }
                                    //else if (FinInvoiceCusTrxMpgList != null && FinInvoiceCusTrxMpgList.Count > 1)
                                    //{
                                    //    isSplitApply = 5;//Should click apply
                                    //}
                                }
                                finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG.Add(dtl);
                            });
                        }

                        //Adjn and CR/DR Allocation
                        if (finReceiptCusTrxMpgList != null && finReceiptCusTrxMpgList.Count > 0)
                        {
                            finReceiptCusTrxMpgList.ForEach(dtl =>
                            {
                                finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG.Add(dtl);
                                #region Adjustment Allocation
                                FinReceiptCusAllocationList = ReceiptAdjnList.Where(mpg => mpg.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == dtl.RCM_INVOICE_HDR).ToList();
                                if (FinReceiptCusAllocationList.Count() > 0)
                                {
                                    FinReceiptCusAllocationList.ForEach(mpg =>
                                    {
                                        FinReceiptCusAllocationObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_ALCN_DTL>();
                                        FinReceiptCusAllocationObj.RAD_PK = mpg.RAD_PK;
                                        FinReceiptCusAllocationObj.RAD_RECEIPT_TRX = mpg.RAD_RECEIPT_TRX;
                                        FinReceiptCusAllocationObj.RAD_ALCN_CDH = mpg.RAD_ALCN_CDH;
                                        FinReceiptCusAllocationObj.RAD_ALCN_RECEIPT_TRX = mpg.RAD_ALCN_RECEIPT_TRX;
                                        FinReceiptCusAllocationObj.RAD_AMOUNT = mpg.RAD_AMOUNT;
                                        FinReceiptCusAllocationObj.RAD_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                        dtl.FIN_RECEIPT_CUS_ALCN_DTL.Add(FinReceiptCusAllocationObj);
                                    });
                                }
                                #endregion

                                #region CR/DR Allocation
                                FinReceiptCusCrdrAllocationList = ReceiptCrdrList.Where(mpg => mpg.RNM_INVOICE_HDR == dtl.RCM_INVOICE_HDR && (mpg.RNM_PAID_AMOUNT > 0 || mpg.RNM_ADJ_AMOUNT > 0)).ToList();
                                if (FinReceiptCusCrdrAllocationList != null && FinReceiptCusCrdrAllocationList.Count() > 0)
                                {
                                    FinReceiptCusCrdrAllocationList.ForEach(mpg =>
                                    {
                                        FinReceiptCusCrdrAllocationObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_CRDR_MPG>();
                                        FinReceiptCusCrdrAllocationObj.RNM_ACTIVE = mpg.RNM_ACTIVE;
                                        FinReceiptCusCrdrAllocationObj.RNM_ADJ_AMOUNT = mpg.RNM_ADJ_AMOUNT;
                                        FinReceiptCusCrdrAllocationObj.RNM_CRDR_HDR = mpg.RNM_CRDR_HDR;
                                        FinReceiptCusCrdrAllocationObj.RNM_PAID_AMOUNT = mpg.RNM_PAID_AMOUNT;
                                        FinReceiptCusCrdrAllocationObj.RNM_RECEIPT_HDR = mpg.RNM_RECEIPT_HDR;
                                        FinReceiptCusCrdrAllocationObj.RNM_RECEIPT_TRX_MPG = mpg.RNM_RECEIPT_TRX_MPG;
                                        FinReceiptCusCrdrAllocationObj.RNM_PK = mpg.RNM_PK;
                                        FinReceiptCusCrdrAllocationObj.RNM_CRDR_MPG = mpg.RNM_CRDR_MPG;
                                        dtl.FIN_RECEIPT_CUS_CRDR_MPG.Add(FinReceiptCusCrdrAllocationObj);
                                    });
                                }
                                #endregion
                            });
                        }

                        if (finReceiptCusHdrObj != null)
                        {
                            finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG.ToList().ForEach(dtl =>
                            {
                                dtl.FIN_RECEIPT_CUS_SO_MPG.ToList().ForEach(mpg =>
                                {
                                    finReceiptCusTaxDtlObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_TAX_DTL>();
                                    finReceiptCusTaxDtlObj.RDT_PK = 0;
                                    if (mpg.FIN_RECEIPT_CUS_TAX_DTL.Count == 0)
                                        mpg.FIN_RECEIPT_CUS_TAX_DTL.Add(finReceiptCusTaxDtlObj);// finReceiptCusTaxDtlList.Add(finReceiptCusTaxDtlObj);
                                });
                            });
                        }
                        //if (finReceiptCusSoMpgList != null && finReceiptCusSoMpgList.Count > 0)
                        //{
                        //    //finReceiptCusHdrObj.FIN_RECEIPT_CUS_SO_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_RECEIPT_CUS_SO_MPG>();
                        //    //finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_RECEIPT_CUS_TRX_MPG>(); 
                        //    finReceiptCusSoMpgList.ForEach(dtl =>
                        //    {
                        //        //finReceiptCusHdrObj.FIN_RECEIPT_CUS_SO_MPG.Add(dtl);
                        //        // dtl.FIN_RECEIPT_CUS_TAX_DTL = new System.Data.Objects.DataClasses.EntityCollection<FIN_RECEIPT_CUS_TAX_DTL>();
                        //        finReceiptCusTaxDtlObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_TAX_DTL>();
                        //        finReceiptCusTaxDtlObj.RDT_PK = 0;
                        //        if (dtl.FIN_RECEIPT_CUS_TAX_DTL.Count < 1)
                        //            dtl.FIN_RECEIPT_CUS_TAX_DTL.Add(finReceiptCusTaxDtlObj);// finReceiptCusTaxDtlList.Add(finReceiptCusTaxDtlObj);
                        //    });
                        //}
                        if (finReceiptSuspList != null && finReceiptSuspList.Count > 0)
                        {
                            finReceiptSuspList.ForEach(x =>
                            {
                                finReceiptCusHdrObj.FIN_RECEIPT_CUS_SUSP_DTL.Add(x);
                            });
                        }
                        retObject = finReceiptCusHdrObj;
                        break;
                    #endregion
                    #region Journalize
                    case ControlsEnum.REVERSE:
                    case ControlsEnum.JOURNALIZE:
                    case ControlsEnum.CHEQUERETURN:
                        if (EntryStatus == EntryStatus.LISTMODE)
                        {
                            foreach (GridViewRow grdrow in grdPOReceiptHdr.Rows)
                            {
                                RadioButton rbtn;
                                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                                if (rbtn.Checked)
                                {
                                    bIsChecked = true;
                                    CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPaymentID")).Value);
                                    Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                    Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                    Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomer")).Text;
                                    break;
                                }
                            }
                        }
                        else if (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.VIEWMODE)
                        {
                            bIsChecked = true;
                        }
                        if (bIsChecked)
                        {
                            if (Approved == 2)
                            {
                                if (finReceiptCusHdrList != null && finReceiptCusHdrList.Count() > 0)
                                {
                                    if (finReceiptCusHdrList[0].RCH_RCVD_AMOUNT == 0)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Journalize_Zero_Amnt").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                        return null;
                                    }
                                }

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
                                    //Session[ERP.Utilities.SessionStrings.TransactionType] = ucrJournalize.TransactionType = ApplicationType.CRJ;
                                    //ucrJournalize.JournalType = (int)JournalTypeEnum.Voucher;
                                    Session[ERP.Utilities.SessionStrings.TransactionType] = ucrJournalize.TransactionType = SOGroup == SalesInvoiceGroup.Miscellaneous ? ApplicationType.MSIRJ
                                      : ApplicationType.CRJ;
                                    //Session[ERP.Utilities.SessionStrings.TransactionType] = SOGroup == SalesInvoiceGroup.Goods ? ApplicationType.CRJ
                                    //  : ApplicationType.MSIRJ;
                                    ucrJournalize.JournalType = (int)JournalTypeEnum.Voucher;
                                }
                                else if (controlType == ControlsEnum.REVERSE)
                                {
                                    Session[ERP.Utilities.SessionStrings.TransactionType] = ucrJournalize.TransactionType = ApplicationType.PDCCJ;
                                    ucrJournalize.JournalType = (int)JournalTypeEnum.Reverse;
                                }
                                else if (controlType == ControlsEnum.CHEQUERETURN)
                                {
                                    Session[ERP.Utilities.SessionStrings.TransactionType] = ucrJournalize.TransactionType = ApplicationType.RCBJ;
                                    ucrJournalize.JournalType = (int)JournalTypeEnum.Return;
                                }
                                ucrJournalize.TransactionPK = (int)CurrPK;
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                                ucrJournalize.JournalizePK = 0;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                GetFieldValues(ControlsEnum.RECEIPTHDRENTRYBYPK);
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = finReceiptCusHdrList[0].RCH_NO;
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = finReceiptCusHdrList[0].RCH_DATE;
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = finReceiptCusHdrList[0].RCH_CURRENCY;
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.CR;
                                Session[ERP.Utilities.SessionStrings.AccountPayablePK] = finReceiptCusHdrList[0].RCH_CUSTOMER;
                                Session[ERP.Utilities.SessionStrings.JournalType] = SOGroup == SalesInvoiceGroup.Miscellaneous ? ApplicationType.MSIRJ
                                      : ApplicationType.CRJ;


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
                        rowID = 0;
                        finReceiptCusSoMpgList = new List<FIN_RECEIPT_CUS_SO_MPG>();
                        foreach (GridViewRow grdrow in grdReceiptSplit.Rows)//
                        {
                            hdfTaxSplit = (HiddenField)grdReceiptSplit.Rows[rowID].FindControl("hdfTaxSplit");
                            hdfOtherChargesSplit = (HiddenField)grdReceiptSplit.Rows[rowID].FindControl("hdfOtherChargesSplit");
                            txtOtherChargesSplit = (TextBox)grdReceiptSplit.Rows[rowID].FindControl("txtOtherChargesSplit");

                            finReceiptCusSoMpgObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_SO_MPG>();
                            hdfReceiptSplitPK = (HiddenField)grdReceiptSplit.Rows[rowID].FindControl("hdfReceiptSplitPK");
                            finReceiptCusSoMpgObj.RSO_PK = hdfReceiptSplitPK == null ? 0 : Convert.ToInt64(hdfReceiptSplitPK.Value);
                            finReceiptCusSoMpgObj.RSO_RECEIPT_HDR = CurrPK;
                            finReceiptCusSoMpgObj.RSO_RECEIPT_TRX_MPG = ReceiptMpgPK;
                            hdfSOPK = (HiddenField)grdReceiptSplit.Rows[rowID].FindControl("hdfSOPK");
                            finReceiptCusSoMpgObj.RSO_SO_HDR = hdfSOPK == null ? 1 : Convert.ToInt32(hdfSOPK.Value);
                            txtPayNowSplit = (TextBox)grdReceiptSplit.Rows[rowID].FindControl("txtPayNowSplit");
                            finReceiptCusSoMpgObj.RSO_RECEIVED_AMOUNT = txtPayNowSplit == null ? 0 : txtPayNowSplit.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtPayNowSplit.Text.Trim());
                            finReceiptCusSoMpgObj.RSO_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            finReceiptCusSoMpgObj.RSO_TAX_AMOUNT = hdfTaxSplit == null ? 0 : hdfTaxSplit.Value.Trim() == string.Empty ? 0 : Convert.ToDecimal(hdfTaxSplit.Value.Trim());
                            //finReceiptCusSoMpgObj.RSO_TAX_AMOUNT = hdfTottaxSplit.Value == null ? 0 : hdfTottaxSplit.Value.Trim() == string.Empty ? 0 : Convert.ToDecimal(hdfTottaxSplit.Value.Trim());
                            //finReceiptCusSoMpgObj.RSO_OTHER_AMOUNT = hdfOtherChargesSplit.Value == null ? 0 : hdfOtherChargesSplit.Value.Trim() == string.Empty ? 0 : Convert.ToDecimal(hdfOtherChargesSplit.Value.Trim());
                            finReceiptCusSoMpgObj.RSO_OTHER_AMOUNT = hdfOtherChargesSplit.Value == null ? 0 : hdfOtherChargesSplit.Value.Trim() == string.Empty ? 0 : Convert.ToDecimal(hdfOtherChargesSplit.Value.Trim());
                            finReceiptCusSoMpgList.Add(finReceiptCusSoMpgObj);
                            rowID++;
                        }
                        retObject = finReceiptCusSoMpgList;
                        break;
                    #endregion
                    #region ADJN SPLIT LIST
                    case ControlsEnum.ADJNSPLITLIST:
                        rowID = 0;
                        HiddenField hdfReceiptTRXAdjnPK;
                        HiddenField hdfCrDrPK;
                        HiddenField hdfReceiptAdjnPK;
                        HiddenField hdfAdjnPK;

                        TextBox txtAllocateAdjn;

                        FinReceiptCusAllocationList = new List<FIN_RECEIPT_CUS_ALCN_DTL>();
                        foreach (GridViewRow grdrow in grdReceiptSplitAdjn.Rows)//
                        {
                            hdfReceiptTRXAdjnPK = (HiddenField)grdReceiptSplitAdjn.Rows[rowID].FindControl("hdfReceiptTRXAdjnPK");
                            hdfCrDrPK = (HiddenField)grdReceiptSplitAdjn.Rows[rowID].FindControl("hdfCrDrPK");
                            hdfReceiptAdjnPK = (HiddenField)grdReceiptSplitAdjn.Rows[rowID].FindControl("hdfReceiptAdjnPK");
                            hdfAdjnPK = (HiddenField)grdReceiptSplitAdjn.Rows[rowID].FindControl("hdfAdjnPK");

                            txtAllocateAdjn = (TextBox)grdReceiptSplitAdjn.Rows[rowID].FindControl("txtAllocateAdjn");

                            FinReceiptCusAllocationObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_ALCN_DTL>();
                            //hdfReceiptSplitPK = (HiddenField)grdReceiptSplit.Rows[rowID].FindControl("hdfReceiptSplitPK");
                            FinReceiptCusAllocationObj.RAD_PK = string.IsNullOrEmpty(hdfAdjnPK.Value) ? 0 : Convert.ToInt64(hdfAdjnPK.Value);
                            FinReceiptCusAllocationObj.RAD_RECEIPT_TRX = string.IsNullOrEmpty(hdfReceiptTRXAdjnPK.Value) ? 0 : Convert.ToInt32(hdfReceiptTRXAdjnPK.Value);
                            FinReceiptCusAllocationObj.RAD_ALCN_CDH = string.IsNullOrEmpty(hdfCrDrPK.Value) ? 0 : Convert.ToInt32(hdfCrDrPK.Value);
                            FinReceiptCusAllocationObj.RAD_ALCN_RECEIPT_TRX = string.IsNullOrEmpty(hdfReceiptAdjnPK.Value) ? 0 : Convert.ToInt32(hdfReceiptAdjnPK.Value);
                            FinReceiptCusAllocationObj.RAD_AMOUNT = txtAllocateAdjn == null ? 0 : txtAllocateAdjn.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtAllocateAdjn.Text.Trim());
                            FinReceiptCusAllocationObj.RAD_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            //if (FinReceiptCusAllocationObj.RAD_AMOUNT > 0)
                            //{
                            FinReceiptCusAllocationList.Add(FinReceiptCusAllocationObj);
                            //}
                            rowID++;
                        }
                        retObject = FinReceiptCusAllocationList;
                        break;
                    #endregion
                    #region ALERT
                    case ControlsEnum.ALERTSAVE:
                        string Typename = Resources.Constants.SystemAlertType;
                        int AlertPk = 0;
                        alertBoObj = new AlertBO();
                        appType = ApplicationType.SI;
                        appType = SOGroup == SalesInvoiceGroup.Miscellaneous ? ApplicationType.MSIR
                              : ApplicationType.SI;


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
                        alertBoObj.ATH_TRX_DATE = txtReceiptDate.Text.Trim() == string.Empty ? DateTime.Now : Convert.ToDateTime(txtReceiptDate.Text.Trim());
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
                    #region SUSPENSE LIST
                    case ControlsEnum.SUSPENCELIST:
                        List<FIN_RECEIPT_CUS_SUSP_DTL> finRecSuspList = new List<FIN_RECEIPT_CUS_SUSP_DTL>();
                        foreach (GridViewRow grdrow in grdSuspenceList.Rows)
                        {
                            FIN_RECEIPT_CUS_SUSP_DTL finReceiptSuspObj = new FIN_RECEIPT_CUS_SUSP_DTL();
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
                                finRecSuspList.Add(finReceiptSuspObj);
                            }
                        }
                        retObject = finRecSuspList;
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
            int dept = 0;
            bool posted;
            try
            {
                switch (controlType)
                {
                    #region RECEIPT GET
                    case ControlsEnum.RECEIPTGET:
                        if (finReceiptCusHdrList != null && finReceiptCusHdrList.Count > 0)
                        {
                            CurrPK = Convert.ToInt32(Request.QueryString["PK"].ToString());
                            hdfShowCancel.Value = finReceiptCusHdrList[0].RCH_STATUS.ToString();
                            recipetType = Convert.ToInt32(finReceiptCusHdrList[0].RCH_GROUP);

                            if (!string.IsNullOrEmpty(finReceiptCusHdrList[0].RCH_DEPT.ToString()) && int.TryParse(finReceiptCusHdrList[0].RCH_DEPT.ToString(), out dept))
                            {
                                Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                base.SetUserDept();
                            }
                            hdfShowPDC.Value = "0";
                            hdfShowChequeReturn.Value = "1";
                            hdfPDCStatusWKF.Value = "0";
                            if (Convert.ToInt16(finReceiptCusHdrList[0].RCH_DEL_STATUS.ToString()) == 1)
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
                            posted = Convert.ToBoolean(finReceiptCusHdrList[0].RCH_HAS_JRNL_ENTRY.ToString());
                            if (posted)
                            {
                                int pdc = 0;
                                int payMode = 0;
                                bool app = ucrWrkf.IsWkfCompleted;
                                GetFieldValues(ControlsEnum.FINHEADERSTATUS);
                                btnReturn.Visible = false;
                                btnReturnDetail.Visible = false;
                                if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                                {
                                    if (finReceiptCusHdrList[0].RCH_PDC.ToString() != null && int.TryParse(finReceiptCusHdrList[0].RCH_PDC.ToString(), out pdc))
                                        if (finTrxHdrList[0].FTH_STATUS == 2)
                                        {
                                            hdfShowPDC.Value = pdc.ToString();
                                        }
                                        else
                                            hdfShowPDC.Value = "0";
                                    else
                                        hdfShowPDC.Value = "0";
                                }
                                if (finReceiptCusHdrList[0].RCH_MODE.ToString() != null && int.TryParse(finReceiptCusHdrList[0].RCH_MODE.ToString(), out payMode))
                                {
                                    if (payMode == (int)PaymentModeEnum.Cheque)
                                    {
                                        if (finReceiptCusHdrList[0].RCH_PDC.ToString() != null && int.TryParse(finReceiptCusHdrList[0].RCH_PDC.ToString(), out pdc))
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
                        //workflowCore = new WorkflowCore.CoreService();
                        //base.WkfRefID = workflowCore.GetRefID((int)CurrPK, PageProcessID); 

                        Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = finReceiptCusHdrList[0].RCH_CUSTOMER;
                        Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = finReceiptCusHdrList[0].CRM_CUSTOMER_MST.CUS_NAME;
                        Approved = Convert.ToInt32(finReceiptCusHdrList[0].RCH_STATUS);

                        // Get ReceiptDetails
                        GetFieldValues(ControlsEnum.RECEIPTHDRENTRYBYPK);
                        GetUIValuesFromObject(ControlsEnum.RECEIPTHDRENTRY);
                        GetFieldValues(ControlsEnum.RECEIPTHDRINVLISTBYPK);
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
                        //for adj allocation;need all saved val's in ReceiptAdjnList
                        foreach (GridViewRow gvrw in grdInvoiceList.Rows)
                        {
                            HiddenField hdfReceiptMpgPK = gvrw.FindControl("hdfReceiptMpgPK") as HiddenField;
                            ReceiptMpgPK = Convert.ToInt64(hdfReceiptMpgPK.Value);
                            HiddenField hdfInvoicePK = gvrw.FindControl("hdfInvoicePK") as HiddenField;

                            InvoicePK = Convert.ToInt32(hdfInvoicePK.Value);

                            GetFieldValues(ControlsEnum.RECEIPTADJNLIST);
                        }
                        GetFieldValues(ControlsEnum.CRDRALLOCATION);

                        break;
                    #endregion
                    #region Receipt Header
                    case ControlsEnum.RECEIPTHDRENTRY:
                        if (finReceiptCusHdrList != null && finReceiptCusHdrList.Count > 0)
                        {
                            InvoiceSOSplitList = finReceiptCusHdrList[0].FIN_RECEIPT_CUS_SO_MPG.ToList();
                            lblReceiptNo.Text = finReceiptCusHdrList[0].RCH_NO == "" ? "[NEW]" : finReceiptCusHdrList[0].RCH_NO;
                            SOGroup = (SalesInvoiceGroup)Enum.Parse(typeof(SalesInvoiceGroup), finReceiptCusHdrList[0].RCH_GROUP.ToString());
                            SICategory = (SalesInvoiceCategory)Enum.Parse(typeof(SalesInvoiceCategory), finReceiptCusHdrList[0].RCH_CATEGORY.ToString());
                            txtReceiptDate.Text = finReceiptCusHdrList[0].RCH_DATE.ToString(Resources.Constants.DateFormatShort);
                            ddlMode.SelectedValue = finReceiptCusHdrList[0].RCH_MODE == 0 ? CommonConstants.SELECTVAL : finReceiptCusHdrList[0].RCH_MODE.ToString();
                            hdfRecStatus.Value = finReceiptCusHdrList[0].RCH_STATUS.ToString();
                            hdfRecWKFStatus.Value = Convert.ToInt16(finReceiptCusHdrList[0].RCH_HAS_JRNL_ENTRY).ToString();
                            //When coming from inbox managing return button,Otherwise its in editmode,and itemselect
                            string hdfMode;
                            int pdc = 0;
                            pdc = finReceiptCusHdrList[0].RCH_PDC;
                            recipetType = finReceiptCusHdrList[0].RCH_GROUP;

                            int payMode = 0;
                            bool app = ucrWrkf.IsWkfCompleted;

                            payMode = Convert.ToInt16(finReceiptCusHdrList[0].RCH_MODE);
                            GetFieldValues(ControlsEnum.FINHEADERSTATUS);
                            if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                            {
                                if (pdc != null)
                                    if (finTrxHdrList[0].FTH_STATUS == 2)
                                    {
                                        hdfShowPDC.Value = pdc.ToString();
                                    }
                                    else
                                        hdfShowPDC.Value = "0";
                                else
                                    hdfShowPDC.Value = "0";
                            }



                            if (payMode != null)
                            {
                                if (payMode == (int)PaymentModeEnum.Cheque)
                                {
                                    if (pdc != null)
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
                            //if (finReceiptCusHdrList[0].RCH_PDC <= 0) { btnReturn.Visible = false; btnReturnDetail.Visible = false; } else { btnReturn.Visible = true; btnReturnDetail.Visible = true; }

                            //if (Convert.ToInt32(ddlMode.SelectedValue) != (int)ReceiptModeEnum.CHEQUE)
                            //{
                            //    vrfBankOfCheque.Enabled = true;
                            //    vrfInstrumentNo.Enabled = true;
                            //    vrfInstrumentDate.Enabled = true;
                            //}
                            //else if (Convert.ToInt32(ddlMode.SelectedValue) != (int)ReceiptModeEnum.BANK)
                            //{
                            //    vrfBankOfCheque.Enabled = false;
                            //    vrfInstrumentNo.Enabled = false;
                            //    vrfInstrumentDate.Enabled = false;
                            //}
                            //else
                            //{
                            //    vrfBankOfCheque.Enabled = false;
                            //    vrfInstrumentNo.Enabled = true;
                            //    vrfInstrumentDate.Enabled = true;
                            //}
                            txtBank.Text = finReceiptCusHdrList[0].FIN_CASH_BANK_MST == null ? string.Empty : finReceiptCusHdrList[0].FIN_CASH_BANK_MST.CBM_NAME; //RCH_BANK_TEXT;
                            hdfBank.Value = hdfSavedBankPk.Value = finReceiptCusHdrList[0].RCH_BANK.ToString();
                            txtReceiptCurrency.Text = finReceiptCusHdrList[0].ADM_CURRENCY_MST1.CUR_CODE;
                            hdfReceiptCurrency.Value = finReceiptCusHdrList[0].RCH_CURRENCY.ToString();
                            ddlCompany.SelectedValue = finReceiptCusHdrList[0].RCH_COMPANY.ToString();
                            if (!string.IsNullOrEmpty(hdfReceiptCurrency.Value) && hdfMultiCurrencyInReceipt.Value == "1")
                            {
                                ddlReceiptCurrency.SelectedValue = hdfReceiptCurrency.Value.ToString();
                                GetFieldValues(ControlsEnum.EXCHANGERATEINRECEIPTCURRENCY);
                                double exchangeCurrReceipt = hdfExchangeCurrReceipt.Value == "" ? 0 : Convert.ToDouble(hdfExchangeCurrReceipt.Value);
                                txtExchangeRate.Text = Math.Round(exchangeCurrReceipt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                //ddlReceiptCurrency.SelectedIndex = ddlReceiptCurrency.Items.IndexOf(ddlCompany.Items.FindByValue(hdfReceiptCurrency.Value.ToString()));
                                GetFieldValues(ControlsEnum.GETMULTIPLEEXCHANGERATES);
                            }

                            GetFieldValues(ControlsEnum.EXCHANGERATEBANK);
                            if (ddlBankChargeCurrency.Items[0].Value != finReceiptCusHdrList[0].RCH_CURRENCY.ToString())
                            {
                                ddlBankChargeCurrency.Items.Insert(1, (new ListItem(finReceiptCusHdrList[0].ADM_CURRENCY_MST1.CUR_CODE + " - " + finReceiptCusHdrList[0].ADM_CURRENCY_MST1.CUR_NAME, finReceiptCusHdrList[0].RCH_CURRENCY.ToString())));
                            }
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
                            txtInstrumentNo.Text = HttpUtility.HtmlDecode(finReceiptCusHdrList[0].RCH_INSTR_NO);
                            txtInstrumentDate.Text = finReceiptCusHdrList[0].RCH_INSTR_DATE.HasValue == false ? "" :
                                finReceiptCusHdrList[0].RCH_INSTR_DATE.Value.ToString(Resources.Constants.DateFormatShort);
                            txtbankOfCheque.Text = HttpUtility.HtmlDecode(finReceiptCusHdrList[0].RCH_BANK_OF_CHEQUE);
                            txtRemarks.Text = HttpUtility.HtmlDecode(finReceiptCusHdrList[0].RCH_REMARKS);

                            LastModifiedTime = finReceiptCusHdrList[0].RCH_MOD_DT;
                            lblLastModifiedHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);
                            Approved = WkfStatus = finReceiptCusHdrList[0].RCH_STATUS;
                            hdfDelStatus.Value = finReceiptCusHdrList[0].RCH_DEL_STATUS.ToString();
                            ddlAdjType.SelectedIndex = Convert.ToInt32(ddlAdjType.Items.IndexOf(ddlAdjType.Items.FindByValue(finReceiptCusHdrList[0].RCH_DISCOUNT.ToString())));
                            txtAdjAmount.Text = Math.Round(finReceiptCusHdrList[0].RCH_DISC_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            txtTaxAmount.Text = Math.Round(finReceiptCusHdrList[0].RCH_TAX_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            chkPDC.Checked = finReceiptCusHdrList[0].RCH_PDC >= 1 ? true : false;
                            if (GetGlobalResourceObject("ConfigurationsRes", "PDCReconciliation").ToString() == "1")
                            {
                                chkPDC.Checked = true;
                                chkPDC.Disabled = true;
                            }

                            if (finReceiptCusHdrList[0].RCH_BANK_CHARGE_CURR != null)
                            {
                                ddlBankChargeCurrency.SelectedIndex = Convert.ToInt32(ddlBankChargeCurrency.Items.IndexOf(ddlBankChargeCurrency.Items.FindByValue(finReceiptCusHdrList[0].RCH_BANK_CHARGE_CURR.ToString())));
                            }
                            txtBankCharge.Text = Math.Round(finReceiptCusHdrList[0].RCH_BANK_CHARGE, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                            chkReturn.Checked = Convert.ToBoolean(finReceiptCusHdrList[0].RCH_RETURN_STATUS);
                            txtReturnDate.Text = Convert.ToDateTime(finReceiptCusHdrList[0].RCH_RETURN_DATE).ToString(Resources.Constants.DateFormatShort);
                            txtReturnRemarks.Text = finReceiptCusHdrList[0].RCH_RETURN_REMARKS;
                            finReceiptSuspList = finReceiptCusHdrList[0].FIN_RECEIPT_CUS_SUSP_DTL.ToList();
                            txtSuspenceAmt.Text = GetFormattedCurrency(finReceiptSuspList.Sum(x => x.RSC_AMOUNT));
                        }
                        break;
                    #endregion
                    #region PAYMENT SPLIT LIST
                    case ControlsEnum.PAYMENTSPLITLIST:
                        if (finInvoiceVndHdrListForPaymentSplit != null && finInvoiceVndHdrListForPaymentSplit.Count > 0)
                        {
                            //Test
                            lblInvSplitNo.Text = lblInvSplitNo_CrdrAlcn.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceVndHdrListForPaymentSplit[0].ICH_NO, 13);
                            lblInvSplitNo.ToolTip = lblInvSplitNo_CrdrAlcn.ToolTip = finInvoiceVndHdrListForPaymentSplit[0].ICH_NO;

                            lblInvSplitDate.Text = lblInvSplitDate_CrdrAlcn.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceVndHdrListForPaymentSplit[0].ICH_DATE.ToString(Resources.Constants.DateFormatShort), 13);
                            lblInvSplitDate.ToolTip = lblInvSplitDate_CrdrAlcn.ToolTip = finInvoiceVndHdrListForPaymentSplit[0].ICH_DATE.ToString(Resources.Constants.DateFormatShort);

                            lblInvSplitSupplier.Text = lblInvSplitSupplier_CrdrAlcn.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceVndHdrListForPaymentSplit[0].CRM_CUSTOMER_MST.CUS_NAME, 13);
                            lblInvSplitSupplier.ToolTip = lblInvSplitSupplier_CrdrAlcn.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceVndHdrListForPaymentSplit[0].CRM_CUSTOMER_MST.CUS_NAME, 300);

                            lblInvSplitAmount.Text = String.Format("{0:c}", finInvoiceVndHdrListForPaymentSplit[0].ICH_AMOUNT_NET_TC);
                            lblInvSplitAmount.ToolTip = String.Format("{0:c}", finInvoiceVndHdrListForPaymentSplit[0].ICH_AMOUNT_NET_TC);
                            if (finReceiptCusSoMpgList != null && finReceiptCusSoMpgList.Count > 0)
                            {
                                if (EntryStatus != EntryStatus.NEWMODE)
                                {
                                    lblInvSplitReceived.Text = lblInvSplitReceived.ToolTip = finReceiptCusSoMpgList[0].RSO_BOUNCED == 0 ? String.Format("{0:c}", finInvoiceVndHdrListForPaymentSplit[0].ICH_AMOUNT_RCVD_TC - PayNowAmount)
                                        : String.Format("{0:c}", finInvoiceVndHdrListForPaymentSplit[0].ICH_AMOUNT_RCVD_TC);
                                }
                                else
                                {
                                    //lblInvSplitReceived.Text = lblInvSplitReceived.ToolTip = String.Format("{0:c}", 0);
                                    lblInvSplitReceived.Text = lblInvSplitReceived.ToolTip = String.Format("{0:c}", finInvoiceVndHdrListForPaymentSplit[0].ICH_AMOUNT_RCVD_TC);
                                }
                            }
                            else
                            {
                                if (EntryStatus != EntryStatus.NEWMODE)
                                {

                                    lblInvSplitReceived.Text = lblInvSplitReceived.ToolTip = String.Format("{0:c}", finInvoiceVndHdrListForPaymentSplit[0].ICH_AMOUNT_RCVD_TC - PayNowAmount);
                                }
                                else
                                {
                                    //lblInvSplitReceived.Text = lblInvSplitReceived.ToolTip = String.Format("{0:c}", 0);
                                    lblInvSplitReceived.Text = lblInvSplitReceived.ToolTip = String.Format("{0:c}", finInvoiceVndHdrListForPaymentSplit[0].ICH_AMOUNT_RCVD_TC);
                                }
                            }

                            lblInvSplitReceiveNow.Text = String.Format("{0:c}", PayNowAmount);
                            lblInvSplitReceiveNow.ToolTip = String.Format("{0:c}", PayNowAmount);
                        }
                        break;
                    #endregion
                    #region SPLIT PAY NOW FOR MULI SO
                    case ControlsEnum.SPLITPAYNOWFORMULISO:
                        if (grdReceiptSplit.Rows.Count > 0 && InvoicePK > 0)
                        {
                            //GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);
                            GetFieldValues(ControlsEnum.INVOICEVNDMPGLISTFORAUTOALCN);

                            //HiddenField hdfFooterBalSplit = (HiddenField)grdPaymentSplit.FooterRow.FindControl("hdfTotalBalFooterSplit");
                            decimal InvoiceSubTotal = 0;
                            //decimal.TryParse(hdfFooterBalSplit.Value, out InvoiceSubTotal);
                            decimal PayNowSplit = 0;
                            decimal advDeductAmnt = 0;
                            decimal ExcessAmount = 0;
                            decimal InvTotalOtherCharge = 0;
                            decimal TotalOtherCharge = InvOtherCharge;
                            decimal InvAllocatedCNAmnt = 0;


                            if (FinInvoiceCusTrxMpgListForAutoAlcn != null && FinInvoiceCusTrxMpgListForAutoAlcn.Count > 0)
                            {
                                InvoiceSubTotal = FinInvoiceCusTrxMpgListForAutoAlcn.Sum(r => r.ICM_AMOUNT);
                                ////InvoiceSubTotal = FinInvoiceVndTrxMpgList[0].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC;
                                //if (FinInvoiceCusTrxMpgListForAutoAlcn[0].FIN_INVOICE_CUS_HDR.ICH_CATEGORY == (byte)POInvoiceCategory.Invoice)
                                //{
                                //    //InvoiceSubTotal = FinInvoiceVndTrxMpgList[0].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC;
                                //    InvoiceSubTotal = FinInvoiceCusTrxMpgListForAutoAlcn.Sum(r => r.ICM_AMOUNT);
                                //    //InvPkSplit = InvoicePK;
                                //    //PoPkSplit = null;
                                //    //GetFieldValues(ControlsEnum.ADVINVOICELIST);
                                //    //if (finAdvDeductList != null && finAdvDeductList.Count > 0)
                                //    //{
                                //    //    advDeductAmnt = finAdvDeductList.Sum(adv => adv.VAD_AMOUNT);
                                //    //    if ((InvoiceSubTotal - advDeductAmnt)>0)
                                //    //    InvoiceSubTotal = InvoiceSubTotal - advDeductAmnt;
                                //    //}
                                //}
                                if (ReceiptCrdrList != null && ReceiptCrdrList.Count > 0)
                                {
                                    long InvPk = FinInvoiceCusTrxMpgListForAutoAlcn[0].ICM_INVOICE_HDR;
                                    InvAllocatedCNAmnt = ReceiptCrdrList.Where(r => r.RNM_INVOICE_HDR == InvPk).Sum(sm => sm.RNM_PAID_AMOUNT + sm.RNM_ADJ_AMOUNT);
                                    //InvoiceSubTotal = InvoiceSubTotal - InvAllocatedCNAmnt;
                                    PayNow = (PayNow - InvAllocatedCNAmnt) < 0 ? 0 : (PayNow - InvAllocatedCNAmnt);
                                }
                                if (InvoiceSubTotal > 0)
                                {
                                    foreach (GridViewRow grdRow in grdReceiptSplit.Rows)
                                    {
                                        HiddenField hdfSOPK = (HiddenField)grdRow.FindControl("hdfSOPK");
                                        Label lblBalanceSplit = (Label)grdRow.FindControl("lblBalanceSplit");
                                        decimal InvAmount = 0;
                                        decimal AdvInvAmount = 0;
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

                                        //InvPkSplit = InvoicePK;
                                        //PoPkSplit = PoPk;
                                        //GetFieldValues(ControlsEnum.ADVINVOICELIST);
                                        //if (finAdvDeductList != null && finAdvDeductList.Count > 0)
                                        //{
                                        //    AdvInvAmount = finAdvDeductList.Sum(adv => adv.VAD_AMOUNT);
                                        //}
                                        //if ((InvAmount - AdvInvAmount) > 0)
                                        //{
                                        //    InvAmount = InvAmount - AdvInvAmount;
                                        //}
                                        TextBox txtPayNow = (TextBox)grdRow.FindControl("txtPayNowSplit");
                                        TextBox txtOtherChargesSplit = (TextBox)grdRow.FindControl("txtOtherChargesSplit");
                                        HiddenField hdfOtherChargesSplit = (HiddenField)grdRow.FindControl("hdfOtherChargesSplit");
                                        Label lblTotalTaxSplit = (Label)grdRow.FindControl("lblTotalTaxSplit");
                                        HiddenField hdfTaxSplit = (HiddenField)grdRow.FindControl("hdfTaxSplit");

                                        if (IsDebitNoteApplied || InvAllocatedCNAmnt > 0 || (string.IsNullOrEmpty(txtPayNow.Text) || Convert.ToDecimal(txtPayNow.Text) == 0))
                                        {
                                            //Label lblBalanceSplit = (Label)grdRow.FindControl("lblBalanceSplit");
                                            //PayNowSplit = Convert.ToDecimal(lblBalanceSplit.Text.Replace(",", "")) * (PayNow / InvoiceSubTotal);
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
                                        decimal AdvInvAmount = 0;
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
            SalesReceiptService salesReceiptServiceClient;
            salesReceiptServiceClient = null;
            try
            {
                if (finReceiptCusHdrObj.RCH_STATUS != 0)
                {
                    if (Convert.ToInt32(ddlMode.SelectedValue) != (int)ReceiptModeEnum.CASH)
                    {
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        success = salesReceiptServiceClient.CheckReceiptHdr(finReceiptHdrObj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                salesReceiptServiceClient = null;
            }
            return success;
        }

        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region Currency
                case ControlsEnum.CURRENCY:
                    ddlReceiptCurrency.Items.Clear();
                    if (dtCurrency != null && dtCurrency.Rows.Count > 0)
                    {
                        ddlReceiptCurrency.DataSource = dtCurrency;
                        ddlReceiptCurrency.DataTextField = Resources.DataFieldRes.CurrencyCode;
                        ddlReceiptCurrency.DataValueField = Resources.DataFieldRes.CurrencyPK;
                        ddlReceiptCurrency.DataBind();

                        DataRow drr = dtCurrency.AsEnumerable().Where(dr => (int)dr["CUR_PK"] == currentUser.BaseCurrency).First();
                        txtBaseCurrency.Text = drr.ItemArray[1].ToString();
                    }
                    if (!string.IsNullOrEmpty(hdfReceiptCurrency.Value))
                    {
                        ddlReceiptCurrency.SelectedIndex = ddlReceiptCurrency.Items.IndexOf(ddlCompany.Items.FindByValue(hdfReceiptCurrency.Value.ToString()));
                    }
                    else
                    {
                        ddlReceiptCurrency.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTTEXT));
                    }
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
                #region Payment Mode
                case ControlsEnum.PAYMODE:
                    ddlMode.Items.Clear();
                    if (admConfigMstList != null && admConfigMstList.Count > 0)
                    {
                        ddlMode.DataSource = admConfigMstList;
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
                    #region Bind Receipt Hdr List
                    case ControlsEnum.RECEIPTHDRLIST:
                        if (finReceiptCusHdrList != null)
                        {
                            GetFieldValues(ControlsEnum.FILLWORKFLOWSTATUS);
                            uclPaging.TotalPages = TotalPages;
                            PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdPOReceiptHdr.DataSource = finReceiptCusHdrList;
                            grdPOReceiptHdr.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        break;
                    #endregion
                    #region Bind Invoice List
                    case ControlsEnum.RECEIPTMPGLIST:
                        if (finInvoiceCusHdrList != null)
                        {
                            grdInvoiceList.DataSource = finInvoiceCusHdrList;
                            grdInvoiceList.DataBind();
                            if (finInvoiceCusHdrList.Count > 0)
                            {
                                ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(finInvoiceCusHdrList[0].ICH_COMPANY.ToString()));
                                hdfCustomerPK.Value = finInvoiceCusHdrList[0].ICH_CUSTOMER.ToString();
                                hdfCustomerAccountNo.Value = finInvoiceCusHdrList[0].ICH_CUSTOMER_ACCOUNT.ToString();
                                hdfInvoiceCurr.Value = finInvoiceCusHdrList[0].ICH_CURRENCY.ToString();
                                txtReceiptCurrency.Text = finInvoiceCusHdrList[0].ADM_CURRENCY_MST1.CUR_CODE.ToString();
                                hdfReceiptCurrency.Value = finInvoiceCusHdrList[0].ICH_CURRENCY.ToString();
                                if (ddlBankChargeCurrency.Items[0].Value != finInvoiceCusHdrList[0].ICH_CURRENCY.ToString())
                                {
                                    if (ddlBankChargeCurrency.Items.Count < 2)
                                        ddlBankChargeCurrency.Items.Insert(1, (new ListItem(finInvoiceCusHdrList[0].ADM_CURRENCY_MST1.CUR_CODE + " - " + finInvoiceCusHdrList[0].ADM_CURRENCY_MST1.CUR_NAME, finInvoiceCusHdrList[0].ICH_CURRENCY.ToString())));

                                    //bool  IsCurrencyAdded=false;
                                    //for (int i = 1; i < ddlBankChargeCurrency.Items.Count; i++)
                                    //{
                                    //    if (ddlBankChargeCurrency.Items[i].Value == finInvoiceCusHdrList[0].ICH_CURRENCY.ToString())
                                    //        IsCurrencyAdded = true;
                                    //}
                                    //if(!IsCurrencyAdded)
                                    //ddlBankChargeCurrency.Items.Insert(1, (new ListItem(finInvoiceCusHdrList[0].ADM_CURRENCY_MST1.CUR_CODE + " - " + finInvoiceCusHdrList[0].ADM_CURRENCY_MST1.CUR_NAME, finInvoiceCusHdrList[0].ICH_CURRENCY.ToString())));
                                }
                                GetFieldValues(ControlsEnum.EXCHANGERATEBANK);
                            }
                            else
                            {
                                txtReceivedAmount.Text = "0.00";
                            }
                        }
                        else if (finReceiptCusTrxMpgList != null)
                        {
                            grdInvoiceList.DataSource = finReceiptCusTrxMpgList;
                            grdInvoiceList.DataBind();
                            if (finReceiptCusTrxMpgList.Count > 0)
                            {
                                if (finReceiptCusTrxMpgList[0].FIN_INVOICE_CUS_HDR != null)
                                {
                                    hdfCustomerPK.Value = finReceiptCusTrxMpgList[0].FIN_INVOICE_CUS_HDR.ICH_CUSTOMER.ToString();
                                    hdfCustomerAccountNo.Value = finReceiptCusTrxMpgList[0].FIN_INVOICE_CUS_HDR.ICH_CUSTOMER_ACCOUNT.ToString();
                                    hdfInvoiceCurr.Value = finReceiptCusTrxMpgList[0].FIN_INVOICE_CUS_HDR.ICH_CURRENCY.ToString();
                                    txtReceiptCurrency.Text = finReceiptCusTrxMpgList[0].FIN_INVOICE_CUS_HDR.ADM_CURRENCY_MST1.CUR_CODE.ToString();
                                    hdfReceiptCurrency.Value = finReceiptCusTrxMpgList[0].FIN_INVOICE_CUS_HDR.ICH_CURRENCY.ToString();
                                    GetFieldValues(ControlsEnum.EXCHANGERATEBANK);
                                    //if (ddlBankChargeCurrency.Items[0].Value != finReceiptCusTrxMpgList[0].FIN_INVOICE_CUS_HDR.ICH_CURRENCY.ToString())
                                    //{
                                    //    ddlBankChargeCurrency.Items.Insert(1, (new ListItem(finReceiptCusTrxMpgList[0].FIN_INVOICE_CUS_HDR.ADM_CURRENCY_MST1.CUR_CODE + " - " + finReceiptCusTrxMpgList[0].FIN_INVOICE_CUS_HDR.ADM_CURRENCY_MST1.CUR_NAME, finReceiptCusTrxMpgList[0].FIN_INVOICE_CUS_HDR.ICH_CURRENCY.ToString())));
                                    //}
                                }
                            }
                            else
                            {
                                txtReceivedAmount.Text = "0.00";
                            }
                        }
                        break;
                    #endregion
                    #region PAYMENT SPLIT LIST
                    case ControlsEnum.PAYMENTSPLITLIST:
                        if (FinInvoiceCusTrxMpgList != null)
                        {
                            grdReceiptSplit.DataSource = FinInvoiceCusTrxMpgList;
                            grdReceiptSplit.DataBind();
                        }
                        else if (finReceiptCusSoMpgList != null)
                        {
                            grdReceiptSplit.DataSource = finReceiptCusSoMpgList;
                            grdReceiptSplit.DataBind();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalSplit", "$(document).ready(function(){CalculateTotalSplit();});", true);
                        break;
                    #endregion
                    #region RECEIPT CUS ADJN
                    case ControlsEnum.RECEIPTCUSADJN:
                        if (CrDrAdjnList != null)
                        {
                            grdReceiptSplitAdjn.DataSource = CrDrAdjnList;
                            grdReceiptSplitAdjn.DataBind();
                        }
                        else if (FinReceiptCusAllocationList != null)
                        {
                            grdReceiptSplitAdjn.DataSource = FinReceiptCusAllocationList;
                            grdReceiptSplitAdjn.DataBind();
                        }
                        if (totBalanceAdjn <= 0)
                        {
                            grdReceiptSplitAdjn.DataSource = null;
                            grdReceiptSplitAdjn.DataBind();
                        }
                        break;
                    #endregion
                    #region CR DR ALLOCATION
                    case ControlsEnum.CRDRALLOCATION:
                        grdCrdrAllocation.DataSource = FinReceiptCusCrdrMpgList;
                        grdCrdrAllocation.DataBind();
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
                        if (dtSuspenceList != null && dtSuspenceList.Rows.Count > 0)
                        {
                            if (finReceiptSuspList != null && finReceiptSuspList.Count > 0)
                            {
                                foreach (DataRow dr in dtSuspenceList.Rows)
                                {
                                    if (finReceiptSuspList.Where(x => x.RSC_VOUCHER_HDR == Convert.ToInt64(dr["FTH_PK"])).Count() > 0)
                                        dr["FTH_SUSP_FLAG"] = 1;
                                }
                            }
                            else
                            {
                                foreach (DataRow dr in dtSuspenceList.Rows)
                                {
                                    dr["FTH_SUSP_FLAG"] = 0;
                                }
                            }
                            grdSuspenceList.DataSource = dtSuspenceList;
                            grdSuspenceList.DataBind();
                        }
                        else
                        {
                            grdSuspenceList.DataSource = null;
                            grdSuspenceList.DataBind();
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
                HiddenField hdfPosted;
                HiddenField hdfMode;
                bool posted;
                HiddenField hdfPDC;
                int pdc = 0;
                int payMode = 0;
                foreach (GridViewRow grdrow in grdPOReceiptHdr.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID);
                    // check row selected or not
                    if (rbtn.Checked)
                    {
                        // get pk from the grid and assign to CurrPk
                        CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPaymentID")).Value);//Convert.ToInt32(grdPOReceiptHdr.DataKeys[grdrow.RowIndex].Values[0]);
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
                        GetFieldValues(ControlsEnum.RECEIPTHDRENTRYBYPK);
                        GetUIValuesFromObject(ControlsEnum.RECEIPTHDRENTRY);
                        GetFieldValues(ControlsEnum.RECEIPTHDRINVLISTBYPK);
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
                        ucrWrkf.ViewAction();
                        //for adj allocation;need all saved val's in ReceiptAdjnList
                        foreach (GridViewRow gvrw in grdInvoiceList.Rows)
                        {
                            HiddenField hdfReceiptMpgPK = gvrw.FindControl("hdfReceiptMpgPK") as HiddenField;
                            ReceiptMpgPK = Convert.ToInt64(hdfReceiptMpgPK.Value);
                            HiddenField hdfInvoicePK = gvrw.FindControl("hdfInvoicePK") as HiddenField;

                            InvoicePK = Convert.ToInt32(hdfInvoicePK.Value);

                            GetFieldValues(ControlsEnum.RECEIPTADJNLIST);
                        }
                        GetFieldValues(ControlsEnum.CRDRALLOCATION);
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
            CurrPK = 0;
            InvoiceSOSplitList = null;
            adjAmtFooter = 0;
            CrDrAdjnList = null;
            FinReceiptCusAllocationList = null;
            ReceiptAdjnList = null;
            finReceiptSuspList = null;

            txtCustomer.Text = string.Empty;
            hdfCustomerID.Value = string.Empty;
            txtReceiptNumber.Text = string.Empty;
            hdfReceiptPK.Value = string.Empty;
            txtReceiptDate.Text = string.Empty;
            hdfBankAccount.Value = string.Empty;
            txtSINo.Text = string.Empty;
            txtBranch.Text = string.Empty;
            txtAccountNo.Text = string.Empty;
            txtBank.Text = string.Empty;
            hdfBank.Value = string.Empty;
            txtRemarks.Text = string.Empty;
            txtbankOfCheque.Text = string.Empty;
            txtReceivedAmount.Text = string.Empty;
            txtSuspenceAmt.Text = string.Empty;
            hdfExchangeCurr.Value = string.Empty;
            hdfExchangeCurrBC.Value = string.Empty;
            hdfInvoiceCurr.Value = string.Empty;
            hdfCustomerAccountNo.Value = string.Empty;
            hdfCustomerID.Value = string.Empty;
            hdfCustomerPK.Value = string.Empty;
            hdfCustomerID.Value = string.Empty;
            hdfReceiptCurrency.Value = string.Empty;
            EditedSalesInvoices = null;
            EditedReceiptDtls = null;
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
            ddlMode.Enabled = true;
            txtBank.Enabled = true;
            txtBank.CssClass = "input-half";
            ddlAdjType.Enabled = true;
            ddlBankChargeCurrency.Enabled = true;
            txtBankCharge.Enabled = true;
            txtRemarks.Enabled = true;
            hdfMultiCurrency.Value = "0";

            Session[ERP.Utilities.SessionStrings.TransactionType] = null;
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
            TextBox txtReceivedNow;
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


            bool isContinue;
            List<FIN_RECEIPT_CUS_SO_MPG> tempInvoiceSOSplitList;
            List<FIN_RECEIPT_CUS_ALCN_DTL> tempReceiptAdjnList;
            List<ReceiptCrdrMpg> tempReceiptCrdrList;
            decimal BalReceiveTot = 0;
            decimal ReceiveNowTot = 0;
            decimal TotalAdjAmt = 0;
            decimal TotalDebitAmnt = 0;

            bool GenDummy = false;


            try
            {
                long? result;
                result = 0;
                long? DummyResult;
                DummyResult = 0;

                int? alertresult;

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
                    else if (((DropDownList)sender).ID == "ddlReceiptCurrency")
                    {
                        commonActions = ActionsEnum.RECEIPT_CURRENCY_MODE_INDEX_CHANGED;
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
                //txtBank.CssClass = "";
                mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);
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
                        imbSuspencelist.Visible = false;
                        break;
                    case (int)ReceiptModeEnum.BANK:
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
                        imbSuspencelist.Visible = Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "ShowSuspenceListButton"));
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
                        imbSuspencelist.Visible = false;
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
                        imbSuspencelist.Visible = false;
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
                        imbSuspencelist.Visible = false;
                        break;
                }
                switch (commonActions)
                {
                    #region ItemSelected
                    case ActionsEnum.ITEMSELECTED:
                        GridViewRow gvr;
                        HiddenField hdfDept;
                        int dept;
                        HiddenField hdfPaymentID;
                        HiddenField hdfPosted;
                        HiddenField hdfDelStatus;
                        bool posted;
                        int pk;
                        gvr = ((RadioButton)sender).Parent.Parent as GridViewRow;
                        hdfPaymentID = gvr.FindControl("hdfPaymentID") as HiddenField;
                        HiddenField hdfRcptStauts = gvr.FindControl("hdfRcptStatus") as HiddenField;
                        HiddenField hdfReceiptStatus = gvr.FindControl("hdfReceiptStatus") as HiddenField;
                        HiddenField hdfSalesInvoiceCategory = gvr.FindControl("hdfSalesInvoiceCategory") as HiddenField;
                        HiddenField hdfReturnStatus = gvr.FindControl("hdfReturnStatus") as HiddenField;
                        hdfShowCancel.Value = hdfReceiptStatus.Value;
                        recipetType = Convert.ToInt32(hdfRcptStauts.Value);
                        hdfCurrStatus.Value = hdfReceiptStatus.Value;
                        if (hdfPaymentID != null && int.TryParse(hdfPaymentID.Value, out pk))
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
                    #region SAVE
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
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
                                                finReceiptCusHdrList = new List<FIN_RECEIPT_CUS_HDR>();
                                                salesReceiptServiceClient = new SalesReceiptService();
                                                salesReceiptServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(salesReceiptServiceClient);
                                                finReceiptCusHdrObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_HDR>();
                                                finReceiptCusHdrObj = (FIN_RECEIPT_CUS_HDR)SetUIValuesToObject(ControlsEnum.RECEIPTHDRENTRY);
                                                if (finReceiptCusHdrObj != null)
                                                {
                                                    TypeRef = finReceiptCusHdrObj.RCH_NO;
                                                    receiptMpgCount = 0;
                                                    receiptMpgCount = finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG.ToList().Count;
                                                    if (receiptMpgCount > 0)
                                                    {
                                                        if (CheckReceipt(finReceiptCusHdrObj))
                                                        {
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



                                                                            SelectedSalesInvoices = null;

                                                                            EntryStatus = EntryStatus.LISTMODE;
                                                                            ResetForm();
                                                                            GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                                                            GetFieldValues(ControlsEnum.PDCVOUCHERLIST);
                                                                            SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                                                            btnNew.Focus();



                                                                            break;
                                                                        }
                                                                    }

                                                                    foreach (FIN_RECEIPT_CUS_TRX_MPG trxObj in finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG)
                                                                    {
                                                                        ICH_PK = trxObj.RCM_INVOICE_HDR.Value;
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
                                                                                if (ReceiptCrdrList != null && ReceiptCrdrList.Count > 0)
                                                                                {
                                                                                    TotalDebitAmnt = ReceiptCrdrList.Where(r => r.RNM_INVOICE_HDR == trxObj.RCM_INVOICE_HDR).Sum(sm => sm.RNM_PAID_AMOUNT + sm.RNM_ADJ_AMOUNT);
                                                                                    if ((trxObj.RCM_RCVD_AMOUNT + trxObj.RCM_ADJUST_AMOUNT) - TotalDebitAmnt <= 0)
                                                                                    {
                                                                                        isCont = true;
                                                                                        break;
                                                                                    }
                                                                                }
                                                                                if (trxObj.FIN_RECEIPT_CUS_SO_MPG != null && trxObj.FIN_RECEIPT_CUS_SO_MPG.Count > 0)
                                                                                {
                                                                                    if (trxObj.FIN_RECEIPT_CUS_SO_MPG.Sum(dtl => dtl.RSO_RECEIVED_AMOUNT) == 0)
                                                                                    {
                                                                                        isCont = false;
                                                                                        break;
                                                                                    }
                                                                                    if (trxObj.FIN_RECEIPT_CUS_SO_MPG.Sum(dtl => dtl.RSO_RECEIVED_AMOUNT) > ((trxObj.RCM_RCVD_AMOUNT + trxObj.RCM_ADJUST_AMOUNT) - TotalDebitAmnt))
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
                                                                    finReceiptCusHdrList.Add(finReceiptCusHdrObj);
                                                                    result = salesReceiptServiceClient.SaveReceiptHdr(finReceiptCusHdrList);

                                                                }
                                                            }
                                                            else
                                                            {

                                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                                                litErrorMsg.Text = GetLocalResourceObject("SplitMand").ToString();
                                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                                                break;
                                                            }
                                                            
                                                            if (result >= 0)
                                                            {
                                                                //Bug ID:  16197 
                                                                if (GenDummy == true)
                                                                {
                                                                    FinTrxService finTrxServiceClient;
                                                                    finTrxServiceClient = new FinTrxService();
                                                                    string refType = "";
                                                                    if (recipetType == 0 || recipetType == 1)
                                                                    {
                                                                        refType = ApplicationType.CRJ;

                                                                    }
                                                                    else if (recipetType == 3)
                                                                    {
                                                                        refType = ApplicationType.MSIRJ;
                                                                    }
                                                                    bool IsDummyEntry = finTrxServiceClient.IsDummyEntry(refType, (int)finReceiptCusHdrObj.RCH_PK, 0);
                                                                    if (IsDummyEntry == true)
                                                                    {
                                                                        DummyResult = finTrxServiceClient.DeleteFinTrx(refType, (int)finReceiptCusHdrObj.RCH_PK, 0);
                                                                    }
                                                                    else
                                                                    {
                                                                        DummyResult = (int)finReceiptCusHdrObj.RCH_PK;
                                                                    }
                                                                    if (DummyResult > 0)
                                                                    {
                                                                        finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                                                        DummyResult = finTrxServiceClient.GenerateDummyEntry((int)CurrPK, refType);
                                                                        finTrxServiceClient = null;
                                                                    }
                                                                }
                                                                CurrPK = (long)result;
                                                                GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                                                FIN_RECEIPT_CUS_HDR tempFinReceiptCusHdrObj = null;
                                                                if (finReceiptCusHdrList != null && finReceiptCusHdrList.Count > 0)
                                                                {
                                                                    LastModifiedTime = finReceiptCusHdrList.FirstOrDefault(aa => aa.RCH_PK == CurrPK) == null ? DateTime.Now
                                                                        : finReceiptCusHdrList.FirstOrDefault(aa => aa.RCH_PK == CurrPK).RCH_MOD_DT;
                                                                    tempFinReceiptCusHdrObj = finReceiptCusHdrList.FirstOrDefault(aa => aa.RCH_PK == CurrPK) == null ? finReceiptCusHdrObj
                                                                        : finReceiptCusHdrList.FirstOrDefault(aa => aa.RCH_PK == CurrPK);
                                                                }



                                                                SelectedSalesInvoices = null;
                                                                litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.SalesReceipt);
                                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                                EntryStatus = EntryStatus.LISTMODE;
                                                                ResetForm();
                                                                GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                                                GetFieldValues(ControlsEnum.PDCVOUCHERLIST);
                                                                SetFieldValues(ControlsEnum.RECEIPTHDRLIST);

                                                            }
                                                        }
                                                        else
                                                        {
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_ReceiptDuplicate").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                        }
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
                    #region New
                    case ActionsEnum.NEW:
                        if (SelectedSalesInvoices != null && SelectedSalesInvoices.Count > 0)
                        {
                            selectedInvoiceList = SelectedSalesInvoices;
                            GetFieldValues(ControlsEnum.RECEIPTMPGLIST);
                            SetFieldValues(ControlsEnum.RECEIPTMPGLIST);
                            ModifiedDatePnl.Visible = false;
                            EntryStatus = EntryStatus.NEWMODE;
                            updateReceipt = false;
                            GetFieldValues(ControlsEnum.RECEIPTNO);
                            lblReceiptNo.Text = hdfReceiptNo.Value;
                            txtReceiptDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                            mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);

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
                                    vrfBankOfCheque.Enabled = false;
                                    break;
                                case (int)ReceiptModeEnum.BANK:
                                    vrfBankName.Enabled = true;
                                    vrfBranch.Enabled = true;
                                    vrfAccountNo.Enabled = true;
                                    vrfInstrumentNo.Enabled = true;
                                    vrfInstrumentDate.Enabled = false;
                                    txtInstrumentNo.Enabled = true;
                                    txtInstrumentNo.CssClass = "input-small";
                                    txtInstrumentDate.Enabled = true;
                                    txtInstrumentDate.CssClass = "input-small";
                                    vrfBankOfCheque.Enabled = false;
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
                                    vrfBankOfCheque.Enabled = true;
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
                                    vrfBankOfCheque.Enabled = false;
                                    break;
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_New_Receipt").ToString()) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        this.PageIndex = "1";
                        GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        GetFieldValues(ControlsEnum.PDCVOUCHERLIST);
                        SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        FillProcessID(1);
                        ResetForm();
                        GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        GetFieldValues(ControlsEnum.PDCVOUCHERLIST);
                        SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Edit
                    case ActionsEnum.EDIT:
                        FillProcessID(1);
                        SetUIEditView(commonActions);
                        ModifiedDatePnl.Visible = true;
                        SetMultiCurrencyConfiguration();
                        mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);
                        //if(hdfMultiCurrencyInReceipt.Value=="1")
                        //{
                        //    GetFieldValues(ControlsEnum.CURRENCY);
                        //    SetFieldValues(ControlsEnum.CURRENCY);
                        //}

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
                                vrfBankOfCheque.Enabled = false;
                                imbSuspencelist.Visible = false;
                                break;
                            case (int)ReceiptModeEnum.BANK:
                                vrfBankName.Enabled = true;
                                vrfBranch.Enabled = true;
                                vrfAccountNo.Enabled = true;
                                vrfInstrumentNo.Enabled = true;
                                vrfInstrumentDate.Enabled = false;
                                txtInstrumentNo.Enabled = true;
                                txtInstrumentNo.CssClass = "input-small";
                                txtInstrumentDate.Enabled = true;
                                txtInstrumentDate.CssClass = "input-small";
                                vrfBankOfCheque.Enabled = false;
                                imbSuspencelist.Visible = Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "ShowSuspenceListButton"));
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
                                vrfBankOfCheque.Enabled = true;
                                imbSuspencelist.Visible = false;
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
                                imbSuspencelist.Visible = false;
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
                                vrfBankOfCheque.Enabled = false;
                                imbSuspencelist.Visible = false;
                                break;
                        }
                        break;
                    #endregion
                    #region View
                    case ActionsEnum.VIEW:
                        SetUIEditView(commonActions);
                        btnCancel.Focus();
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetForm();
                        GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        GetFieldValues(ControlsEnum.PDCVOUCHERLIST);
                        SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        break;
                    #endregion
                    #region Remove
                    case ActionsEnum.REMOVE:

                        int INVPk = int.Parse(((Button)sender).CommandArgument.ToString());

                        HiddenField hdfInvoice = null;
                        TextBox txtReceived = null;
                        foreach (GridViewRow grdRow in grdInvoiceList.Rows)
                        {
                            hdfInvoice = grdRow.FindControl("hdfInvoicePK") as HiddenField;
                            txtReceived = grdRow.FindControl("txtReceivedNow") as TextBox;
                            if (hdfInvoice != null && txtReceived != null && !string.IsNullOrEmpty(hdfInvoice.Value) && !string.IsNullOrEmpty(txtReceived.Text))
                            {
                                decimal d = 0;
                                decimal.TryParse(txtReceived.Text, out d);
                                if (!dicTempAmount.ContainsKey(hdfInvoice.Value)
                                    && hdfInvoice.Value != INVPk.ToString())
                                {
                                    dicTempAmount.Add(hdfInvoice.Value, d);
                                }
                            }
                        }

                        if (CurrPK == 0)
                        {
                            SelectedSalesInvoices.Remove(INVPk);
                            selectedInvoiceList = SelectedSalesInvoices;
                        }
                        if (EditedSalesInvoices != null)
                        {
                            finInvoiceCusHdrList = EditedSalesInvoices;
                            finInvoiceCusHdrObj = CommonFunctions.Initilize<ERPData.FIN_INVOICE_CUS_HDR>();
                            finInvoiceCusHdrObj = finInvoiceCusHdrList.SingleOrDefault(ivh => ivh.ICH_PK == INVPk);
                            if (finInvoiceCusHdrObj != null)
                            {
                                finInvoiceCusHdrList.Remove(finInvoiceCusHdrObj);
                                EditedSalesInvoices = finInvoiceCusHdrList;
                                SetFieldValues(ControlsEnum.RECEIPTMPGLIST);
                            }
                        }
                        else if (EditedReceiptDtls != null)
                        {
                            finReceiptCusTrxMpgList = EditedReceiptDtls;
                            finReceiptCusTrxMpgObj = CommonFunctions.Initilize<ERPData.FIN_RECEIPT_CUS_TRX_MPG>();
                            finReceiptCusTrxMpgObj = finReceiptCusTrxMpgList.SingleOrDefault(rcm => rcm.RCM_INVOICE_HDR == INVPk);
                            if (finReceiptCusTrxMpgObj != null)
                            {
                                finReceiptCusTrxMpgList.Remove(finReceiptCusTrxMpgObj);
                                EditedReceiptDtls = finReceiptCusTrxMpgList;
                                SetFieldValues(ControlsEnum.RECEIPTMPGLIST);
                            }
                        }

                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (grdInvoiceList.Rows.Count > 0)
                        {
                            salesReceiptServiceClient = new SalesReceiptService();
                            salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                            result = salesReceiptServiceClient.DeleteSalesReceiptHdr(CurrPK);
                            if (result >= 0)
                            {
                                litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.SalesReceipt);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm();
                                GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                GetFieldValues(ControlsEnum.PDCVOUCHERLIST);
                                SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                btnNew.Focus();

                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_Delete_InvoiceCount").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Account Number
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

                    #region  Receipt Currency Mode Change
                    case ActionsEnum.RECEIPT_CURRENCY_MODE_INDEX_CHANGED:
                        SetMultiCurrencyConfiguration();
                        if (!string.IsNullOrEmpty(hdfReceiptCurrency.Value) && hdfMultiCurrency.Value == "1")
                        {
                            // ddlReceiptCurrency.SelectedValue = hdfReceiptCurrency.Value.ToString();
                            GetFieldValues(ControlsEnum.EXCHANGERATEINRECEIPTCURRENCY);
                            GetFieldValues(ControlsEnum.GETMULTIPLEEXCHANGERATES);
                            double exchangeCurrReceipt = hdfExchangeCurrReceipt.Value == "" ? 0.00 : Convert.ToDouble(hdfExchangeCurrReceipt.Value);
                            // txtExchangeRate.Text=GetFormattedCurrency(0)
                            txtExchangeRate.Text = Math.Round(exchangeCurrReceipt == -1 ? 0.00 : exchangeCurrReceipt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //ddlReceiptCurrency.SelectedIndex = ddlReceiptCurrency.Items.IndexOf(ddlCompany.Items.FindByValue(hdfReceiptCurrency.Value.ToString()));
                        }
                        break;
                    #endregion

                    #region Payment Mode Change
                    case ActionsEnum.PAYMENT_MODE_INDEX_CHANGED:
                        mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);
                        txtBank.Text = string.Empty;
                        hdfBank.Value = string.Empty;
                        txtBranch.Text = string.Empty;
                        txtAccountNo.Text = string.Empty;
                        txtInstrumentNo.Text = string.Empty;
                        txtInstrumentDate.Text = string.Empty;
                        txtSuspenceAmt.Text = "0.00";
                        finReceiptSuspList = null;
                        chkPDC.Checked = false;
                        if (GetGlobalResourceObject("ConfigurationsRes", "PDCReconciliation").ToString() == "1")
                        {
                            chkPDC.Checked = true;
                            chkPDC.Disabled = true;
                        }
                        hdfBankAccount.Value = string.Empty;
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
                                vrfBankOfCheque.Enabled = false;
                                imbSuspencelist.Visible = false;
                                break;
                            case (int)ReceiptModeEnum.BANK:
                                vrfBankName.Enabled = true;
                                vrfBranch.Enabled = true;
                                vrfAccountNo.Enabled = true;
                                vrfInstrumentNo.Enabled = true;
                                vrfInstrumentDate.Enabled = false;
                                vrfBankOfCheque.Enabled = false;
                                txtInstrumentNo.Enabled = true;
                                txtInstrumentNo.CssClass = "input-small";
                                txtInstrumentDate.Enabled = true;
                                txtInstrumentDate.CssClass = "input-small";
                                imbSuspencelist.Visible = Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "ShowSuspenceListButton"));
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
                                vrfBankOfCheque.Enabled = true;
                                imbSuspencelist.Visible = false;
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
                                imbSuspencelist.Visible = false;
                                ////GetFieldValues(ControlsEnum.CUSTOMERBANK);
                                ////if (crmCustomerMstList != null && crmCustomerMstList.Count > 0)
                                ////{
                                ////    txtBank.Text = crmCustomerMstList[0].CBM_NAME == null ? string.Empty : crmCustomerMstList[0].CBM_CODE + " - " + crmCustomerMstList[0].CBM_NAME; //RCH_BANK_TEXT;
                                ////    hdfBank.Value = crmCustomerMstList[0].CBM_PK.ToString() == null ? string.Empty : crmCustomerMstList[0].CBM_PK.ToString();
                                ////    txtAccountNo.Text = crmCustomerMstList[0].CBM_ACC_NO;
                                ////    txtBranch.Text = crmCustomerMstList[0].CBM_BRANCH;
                                ////    hdfBankAccount.Value = crmCustomerMstList[0].CBM_ACCOUNT.ToString();
                                ////}
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
                                vrfBankOfCheque.Enabled = false;
                                imbSuspencelist.Visible = false;
                                break;
                        }
                        break;

                    #endregion
                    #region ExchangeRate
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
                    #region Tabs
                    //case ActionsEnum.DEFAULT:
                    //    Response.Redirect(Resources.PageURL.SoListing);
                    //    break;
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
                    //case ActionsEnum.RECEIPTLIST:
                    //    ResetForm();
                    //    GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                    //    SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                    //    this.btnNew.Focus();
                    //    EntryStatus = EntryStatus.LISTMODE;
                    //    break;
                    //case ActionsEnum.RECEIPTDETAIL:
                    //    SetUIEditView(commonActions);
                    //    ModifiedDatePnl.Visible = true;
                    //    mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);
                    //    switch (mode)
                    //    {
                    //        case (int)ReceiptModeEnum.CASH:
                    //            vrfBranch.Enabled = false;
                    //            vrfAccountNo.Enabled = false;
                    //            vrfInstrumentNo.Enabled = false;
                    //            txtInstrumentNo.Enabled = false;
                    //            txtInstrumentDate.Enabled = false;
                    //            vrfInstrumentDate.Enabled = false;
                    //            break;
                    //        case (int)ReceiptModeEnum.BANK:
                    //            txtInstrumentNo.Enabled = true;
                    //            vrfBranch.Enabled = true;
                    //            vrfAccountNo.Enabled = true;
                    //            vrfInstrumentNo.Enabled = true;
                    //            txtInstrumentDate.Enabled = true;
                    //            vrfInstrumentDate.Enabled = true;
                    //            break;
                    //    }
                    //    break;
                    case ActionsEnum.DEFAULT:
                        CheckUserRightsAndRedirect(Resources.PageURL.SoListing);
                        //Response.Redirect(Resources.PageURL.SoListing);
                        break;
                    case ActionsEnum.SALESINVOICE:
                        CheckUserRightsAndRedirect(Resources.PageURL.SalesInvoicing);
                        //Response.Redirect(Resources.PageURL.SalesInvoicing);
                        break;
                    case ActionsEnum.INVOICE:
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
                        foreach (GridViewRow grdrow in grdPOReceiptHdr.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
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
                    case ActionsEnum.RECEIPTLIST:
                        FillProcessID(1);
                        ResetForm();
                        GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        GetFieldValues(ControlsEnum.PDCVOUCHERLIST);
                        SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    case ActionsEnum.RECEIPTDETAIL:
                        FillProcessID(1);
                        SetUIEditView(commonActions);
                        ModifiedDatePnl.Visible = true;
                        mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);
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
                                vrfBankOfCheque.Enabled = false;
                                imbSuspencelist.Visible = false;
                                break;
                            case (int)ReceiptModeEnum.BANK:
                                vrfBankName.Enabled = true;
                                vrfBranch.Enabled = true;
                                vrfAccountNo.Enabled = true;
                                vrfInstrumentNo.Enabled = true;
                                vrfInstrumentDate.Enabled = false;
                                txtInstrumentNo.Enabled = true;
                                txtInstrumentNo.CssClass = "input-small";
                                txtInstrumentDate.Enabled = true;
                                txtInstrumentDate.CssClass = "input-small";
                                vrfBankOfCheque.Enabled = false;
                                imbSuspencelist.Visible = Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "ShowSuspenceListButton"));
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
                                vrfBankOfCheque.Enabled = true;
                                imbSuspencelist.Visible = false;
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
                                imbSuspencelist.Visible = false;
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
                                vrfBankOfCheque.Enabled = false;
                                imbSuspencelist.Visible = false;
                                break;
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
                        TextBox txtAdjustments;
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
                        finReceiptCusHdrObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_HDR>();
                        finReceiptCusHdrObj.RCH_PK = CurrPK;
                        GetFieldValues(ControlsEnum.RECEIPTHDRENTRYBYPK);
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
                        GetFieldValues(ControlsEnum.PDCVOUCHERLIST);
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
                        GetFieldValues(ControlsEnum.PDCVOUCHERLIST);
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
                        GetFieldValues(ControlsEnum.PDCVOUCHERLIST);
                        SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        break;
                    #endregion
                    #region Journalize Submit
                    case ActionsEnum.JOURNALIZESUBMIT:
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                            if (Transaction == "SAVE")
                            {
                                salesReceiptServiceClient = new SalesReceiptService();
                                salesReceiptServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(salesReceiptServiceClient);
                                result = salesReceiptServiceClient.UpdateReceiptHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);

                            }
                            else if (Transaction == "DELETE")
                            {
                                salesReceiptServiceClient = new SalesReceiptService();
                                salesReceiptServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(salesReceiptServiceClient);
                                result = salesReceiptServiceClient.UpdateReceiptHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                            }
                        }
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();

                        //if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ReferanceID > 0)
                        //{
                        //   Response.Redirect(Resources.PageURL.InboxURL,false);
                        //}
                        //else
                        //{
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        GetFieldValues(ControlsEnum.PDCVOUCHERLIST);
                        SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        // }
                        break;
                    #endregion
                    #region Journalize Delete
                    case ActionsEnum.JOURNALIZEDELETE:
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                            if (Transaction == "DELETE")
                            {
                                salesReceiptServiceClient = new SalesReceiptService();
                                salesReceiptServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(salesReceiptServiceClient);
                                result = salesReceiptServiceClient.UpdateReceiptHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                            }
                        }
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        GetFieldValues(ControlsEnum.PDCVOUCHERLIST);
                        SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        break;
                    #endregion
                    #region REVERSE/Return Submit
                    case ActionsEnum.REVERSESUBMIT:
                    case ActionsEnum.RETURNSUBMIT:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        //if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ReferanceID > 0)
                        //{
                        //    Response.Redirect(Resources.PageURL.InboxURL);
                        //}
                        //else
                        //{
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        GetFieldValues(ControlsEnum.PDCVOUCHERLIST);
                        SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        //}
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
                        GetFieldValues(ControlsEnum.PDCVOUCHERLIST);
                        SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
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
                                                finReceiptCusHdrList = new List<FIN_RECEIPT_CUS_HDR>();
                                                salesReceiptServiceClient = new SalesReceiptService();
                                                salesReceiptServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(salesReceiptServiceClient);
                                                finReceiptCusHdrObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_HDR>();
                                                finReceiptCusHdrObj = (FIN_RECEIPT_CUS_HDR)SetUIValuesToObject(ControlsEnum.WRKFSUBMIT);
                                                if (hdfExchangeCurrBC.Value != "-1")
                                                {
                                                    if (finReceiptCusHdrObj != null)
                                                    {
                                                        if (CheckReceipt(finReceiptCusHdrObj))
                                                        {
                                                            TypeRef = finReceiptCusHdrObj.RCH_NO;
                                                            receiptMpgCount = 0;
                                                            receiptMpgCount = finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG.ToList().Count;
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
                                                                    foreach (FIN_RECEIPT_CUS_TRX_MPG trxObj in finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG)
                                                                    {
                                                                        ICH_PK = trxObj.RCM_INVOICE_HDR.Value;
                                                                        GetFieldValues(ControlsEnum.INVOICEHDRBYPK);
                                                                        if (ObjFinInvoiceCusHdrList != null && ObjFinInvoiceCusHdrList.Count > 0 && ObjFinInvoiceCusHdrList[0].ICH_IS_OPENING == 1)
                                                                        {
                                                                            tally = true;
                                                                        }
                                                                        else
                                                                        {


                                                                            TotalDebitAmnt = 0;
                                                                            if ((trxObj.RCM_RCVD_AMOUNT > 0) || ((zeroCount <= 0) || (Iscont == true && (hdfIscontYes.Value == "1"))))// && tempFinPaymentVndPoMpg != null && tempFinPaymentVndPoMpg.Count > 0)
                                                                            {
                                                                                if (ReceiptCrdrList != null && ReceiptCrdrList.Count > 0)
                                                                                {
                                                                                    TotalDebitAmnt = ReceiptCrdrList.Where(r => r.RNM_INVOICE_HDR == trxObj.RCM_INVOICE_HDR).Sum(sm => sm.RNM_PAID_AMOUNT + sm.RNM_ADJ_AMOUNT);
                                                                                    if ((trxObj.RCM_RCVD_AMOUNT + trxObj.RCM_ADJUST_AMOUNT) - TotalDebitAmnt <= 0)
                                                                                    {
                                                                                        tally = true;
                                                                                        break;
                                                                                    }
                                                                                }

                                                                                if ((trxObj.FIN_RECEIPT_CUS_SO_MPG != null && trxObj.FIN_RECEIPT_CUS_SO_MPG.Count > 0))
                                                                                {
                                                                                    //if ((trxObj.RCM_RCVD_AMOUNT - TotalDebitAmnt) != trxObj.FIN_RECEIPT_CUS_SO_MPG.Sum(dtl => dtl.RSO_RECEIVED_AMOUNT))
                                                                                    //{
                                                                                    //    tally = false;
                                                                                    //    break;
                                                                                    //}
                                                                                    if (trxObj.FIN_RECEIPT_CUS_SO_MPG.Sum(dtl => dtl.RSO_RECEIVED_AMOUNT) > ((trxObj.RCM_RCVD_AMOUNT + trxObj.RCM_ADJUST_AMOUNT) - TotalDebitAmnt))
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
                                                        }
                                                        else
                                                        {
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_ReceiptDuplicate").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                        }
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

                                finReceiptCusHdrList = new List<FIN_RECEIPT_CUS_HDR>();
                                salesReceiptServiceClient = new SalesReceiptService();
                                salesReceiptServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(salesReceiptServiceClient);
                                finReceiptCusHdrObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_HDR>();
                                finReceiptCusHdrObj = (FIN_RECEIPT_CUS_HDR)SetUIValuesToObject(ControlsEnum.WRKFSUBMIT);

                                //Check Credit\Debit is posted or not
                                #region Check Credit\Debit is posted or not
                                //AppliedInvPkList
                                PKXml = String.Join(",", SelectedSalesInvoices.Select(x => x.ToString()).ToArray());
                                GetFieldValues(ControlsEnum.CHECKCREDITDEBITPOST);
                                if (ValidateId == -100)
                                {
                                    EntryStatus = EntryStatus.ENTRYMODE;
                                    if (SelectedSalesInvoices != null)
                                    {
                                        selectedInvoiceList = SelectedSalesInvoices;
                                        GetFieldValues(ControlsEnum.RECEIPTMPGLIST);
                                        SetFieldValues(ControlsEnum.RECEIPTMPGLIST);
                                    }
                                    litErrorMsg.Text = GetLocalResourceObject("ReceiptCD_Msg").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');ClosePopup();", true);
                                    break;
                                }
                                #endregion


                                //Create invoice no only after validations
                                if (string.IsNullOrEmpty(lblReceiptNo.Text.Trim()) || lblReceiptNo.Text.Trim().Equals("[NEW]"))
                                {
                                    GetFieldValues(ControlsEnum.RECEIPTNO);
                                    lblReceiptNo.Text = hdfReceiptNo.Value;
                                }
                                finReceiptCusHdrObj.RCH_NO = lblReceiptNo.Text;

                                if (hdfExchangeCurrBC.Value != "-1")
                                {
                                    if (finReceiptCusHdrObj != null)
                                    {
                                        receiptMpgCount = 0;
                                        receiptMpgCount = finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG.ToList().Count;
                                        if (receiptMpgCount > 0)
                                        {
                                            if (CheckReceipt(finReceiptCusHdrObj))
                                            {
                                                if (isSplitApply != 5)
                                                {
                                                    finReceiptCusHdrList.Add(finReceiptCusHdrObj);
                                                    result = salesReceiptServiceClient.SaveReceiptHdr(finReceiptCusHdrList);
                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                                          "ClosePopup();", true);
                                                    litErrorMsg.Text = GetLocalResourceObject("SplitMand").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                    break;
                                                }
                                                if (result > 0)// Save Success ! do WorkFlow
                                                {
                  
                                                    CurrPK = (long)result;
                                                    ucrWrkf.ApplicationID = (int)CurrPK;
                                                    #region ALERTSAVE
                                                    GetFieldValues(ControlsEnum.ALERTCONFIG);
                                                    int isAlert = 0;
                                                    if (admAppConstMstList != null && admAppConstMstList.Count > 0)
                                                    {
                                                        isAlert = admAppConstMstList[0].ACF_VALUE;
                                                    }
                                                    if (chkPDC.Checked && isAlert == 1)
                                                    {
                                                        foreach (FIN_RECEIPT_CUS_TRX_MPG receiptTrxObj in finReceiptCusHdrObj.FIN_RECEIPT_CUS_TRX_MPG)
                                                        {
                                                            invPK = Convert.ToInt32(receiptTrxObj.FIN_INVOICE_CUS_HDR.ICH_PK);
                                                            AlertBO alertBoObj = new AlertBO();
                                                            alertBoObj = (AlertBO)SetUIValuesToObject(ControlsEnum.ALERTSAVE);
                                                            if (alertBoObj != null)
                                                            {
                                                                alertresult = BusinessLogic.AlertManagement.Alerts.SaveAlertDetails(alertBoObj);
                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                    #region WkfSummarySave
                                                    int resultSummary = BusinessLogic.CommonManagement.CommonBL.SaveSummary(result.Value, Convert.ToInt32(hdfProcessID.Value));
                                                    if (resultSummary <= 0)
                                                    {

                                                        litErrorMsg.Text = Resources.Messages.SummaryActionFailed;
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                                    }
                                                    #endregion
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
                                                        litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.EditUsedByAnotherUser;
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                        + "','" + Resources.ErpRes.Information + "');", true);
                                                    }
                                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                                    {
                                                        litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                        + "','" + Resources.ErpRes.Information + "');", true);
                                                    }
                                                    else
                                                    {
                                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                            + "','" + Resources.ErpRes.Information + "');", true);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_ReceiptDuplicate").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            }
                                        }
                                    }
                                }
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation((int)CurrPK, ApplicationType.CR))
                                {
                                    ucrWrkf.ApplicationID = (int)CurrPK;
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
                                    SelectedSalesInvoices = null;
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                    GetFieldValues(ControlsEnum.PDCVOUCHERLIST);
                                    SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                    btnNew.Focus();
                                }
                            }
                            else
                                ucrWrkf.ApplicationID = (int)CurrPK;
                            if (isSplitApply != 5)
                            {
                                if (ucrWrkf.ApplicationID > 0)
                                {
                                    ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                    //Do WorkFlow if WorkFlow has Actions
                                    if (ddlWkfAction.Items.Count > 0)
                                    {
                                        action = ddlWkfAction.SelectedItem.ToString();
                                        result = ucrWrkf.DoWorkFlow();
                                        if (result > 0)
                                        {
                                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                            {
                                                FillProcessID(1);
                                                litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;
                                            }
                                            //Show Save success message and reset Contract Entry
                                            else
                                                litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                                            object[] args = new object[2];
                                            args[0] = Resources.PageNameRes.SalesReceipt;
                                            args[1] = lblReceiptNo.Text;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                                            SelectedSalesInvoices = null;

                                            if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                            {
                                                ResetForm();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                                EntryStatus = EntryStatus.LISTMODE;
                                                ResetForm();
                                                GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                                GetFieldValues(ControlsEnum.PDCVOUCHERLIST);
                                                SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                                btnNew.Focus();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region ADJNINVOICEDETAIL
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

                        lblCusname.Text = lblCusname.ToolTip = lblCustomerTxt.ToolTip;
                        lblcurrencyname.Text = lblcurrencyname.ToolTip = txtReceiptCurrency.Text;
                        ReceiptMpgPK = Convert.ToInt64(hdfReceiptMpgPK.Value);
                        IsApply = Convert.ToInt32(hdfIsApply.Value);
                        if (hdfReceiptMpgPK != null && !string.IsNullOrEmpty(hdfReceiptMpgPK.Value) && !hdfReceiptMpgPK.Value.Equals("0"))
                        {
                            //if (hdfIsApply.Value != "1")
                            //    GetFieldValues(ControlsEnum.RECEIPTADJNLIST);
                            //else
                            GetFieldValues(ControlsEnum.RECEIPTADJNDUMMYLIST);
                            if (FinReceiptCusAllocationList != null && FinReceiptCusAllocationList.Count > 0)
                            {
                                //GetFieldValues(ControlsEnum.RECEIPTCUSADJN);
                                SetFieldValues(ControlsEnum.RECEIPTCUSADJN);


                                //FinReceiptCusAllocationList.ForEach(SavAdj =>   
                                //                                 { 
                                //                                     CrDrAdjnList.Where(fulAdj =>fulAdj.RAA_CRDRPK==null?fulAdj.RAA_TRXPK==SavAdj.RAD_ALCN_RECEIPT_TRX:fulAdj.RAA_CRDRPK==SavAdj.RAD_ALCN_CDH)
                                //                                         .ToList().ForEach()
                                //{
                                //    dtl.FIN_RECEIPT_CUS_TRX_MPG = new FIN_RECEIPT_CUS_TRX_MPG()
                                //    {
                                //        RCM_INVOICE_HDR = InvoicePK
                                //    };
                                //    tempInvoiceSOSplitList.Add(dtl);
                                //});

                                //});

                            }
                            else
                            {
                                GetFieldValues(ControlsEnum.RECEIPTCUSADJN);
                                SetFieldValues(ControlsEnum.RECEIPTCUSADJN);
                            }

                        }
                        else//new
                        {
                            GetFieldValues(ControlsEnum.RECEIPTCUSADJN);
                            SetFieldValues(ControlsEnum.RECEIPTCUSADJN);
                            //Header
                            //GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                            //GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                            //isSplitChanged = false;
                        }

                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUpAdjn]','" + GetLocalResourceObject("ReceiptAlloc").ToString() + "','800','300');", true);
                        break;
                    #endregion
                    #region Po Invoices
                    case ActionsEnum.INVOICEDETAIL:
                        hdfMultiCurrency.Value = "0";
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

                        //divErrorLabel.Visible = false;
                        //hdfReceiptMpgPK = (HiddenField)((((Button)sender).Parent).FindControl("hdfReceiptMpgPK"));
                        //txtReceivedNow = (TextBox)((((Button)sender).Parent).FindControl("txtReceivedNow"));
                        //HiddenField hdfTotalTax = (HiddenField)((((Button)sender).Parent).FindControl("hdfTotalTax"));
                        //TextBox txtOtherCharges = (TextBox)((((Button)sender).Parent).FindControl("txtOtherCharges"));
                        //hdfTaxPer.Value = "1";
                        //hdfOtherPer.Value = "1";
                        //if (hdfTotalTax != null && !string.IsNullOrEmpty(hdfTotalTax.Value.Trim()) && txtReceivedNow != null && !string.IsNullOrEmpty(txtReceivedNow.Text.Trim()))
                        //{
                        //    if (Convert.ToDecimal(txtReceivedNow.Text.Trim()) > 0)
                        //        hdfTaxPer.Value = (Convert.ToDecimal(hdfTotalTax.Value.Trim()) / Convert.ToDecimal(txtReceivedNow.Text.Trim())).ToString();
                        //}
                        //if (txtOtherCharges != null && !string.IsNullOrEmpty(txtOtherCharges.Text.Trim()) && txtReceivedNow != null && !string.IsNullOrEmpty(txtReceivedNow.Text.Trim()))
                        //{
                        //    if (Convert.ToDecimal(txtReceivedNow.Text.Trim()) > 0)
                        //        hdfOtherPer.Value = (Convert.ToDecimal(txtOtherCharges.Text.Trim()) / Convert.ToDecimal(txtReceivedNow.Text.Trim())).ToString();
                        //}
                        //if (txtReceivedNow != null && !string.IsNullOrEmpty(txtReceivedNow.Text.Trim()))
                        //{
                        //    PayNowAmount = Convert.ToDecimal(txtReceivedNow.Text.Trim());
                        //}

                        //hdfInvoicePK = (HiddenField)((((Button)sender).Parent).FindControl("hdfInvoicePK"));
                        //if (hdfInvoicePK != null && !string.IsNullOrEmpty(hdfInvoicePK.Value) && !hdfInvoicePK.Value.Equals("0"))
                        //{
                        //    InvoicePK = Convert.ToInt64(hdfInvoicePK.Value);
                        //}
                        //else
                        //    InvoicePK = 0;
                        //tempInvoiceSOSplitList = InvoiceSOSplitList;
                        //tempFinReceiptCusSoMpgList = tempInvoiceSOSplitList.Where(dtl => dtl.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK).ToList();



                        //if (hdfReceiptMpgPK != null && !string.IsNullOrEmpty(hdfReceiptMpgPK.Value) && !hdfReceiptMpgPK.Value.Equals("0"))
                        //{
                        //    ReceiptMpgPK = Convert.ToInt64(hdfReceiptMpgPK.Value);
                        //    GetFieldValues(ControlsEnum.PAYMENTSPLITLIST);

                        //    if (tempFinReceiptCusSoMpgList == null || tempFinReceiptCusSoMpgList.Count == 0)
                        //    {
                        //        if (finReceiptCusSoMpgList != null && finReceiptCusSoMpgList.Count > 0)//Edit
                        //        {
                        //            tempInvoiceSOSplitList.Where(dtl => dtl.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK)
                        //                            .ToList().ForEach(dtl => tempInvoiceSOSplitList.Remove(dtl));
                        //            finReceiptCusSoMpgList.ForEach(dtl =>
                        //            {
                        //                dtl.FIN_RECEIPT_CUS_TRX_MPG = new FIN_RECEIPT_CUS_TRX_MPG()
                        //                {
                        //                    RCM_INVOICE_HDR = InvoicePK
                        //                };
                        //                tempInvoiceSOSplitList.Add(dtl);
                        //            });
                        //            InvoiceSOSplitList = tempInvoiceSOSplitList;
                        //            //HiddenField hdfInvoicePK = (HiddenField)((((Button)sender).Parent).FindControl("hdfInvoicePK"));
                        //            //if (hdfInvoicePK != null && !string.IsNullOrEmpty(hdfInvoicePK.Value) && !hdfInvoicePK.Value.Equals("0"))
                        //            //{
                        //            //    InvoicePK = Convert.ToInt64(hdfInvoicePK.Value);
                        //            //    GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                        //            //    GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                        //            //    isSplitChanged = false;
                        //            //    SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);
                        //            //}
                        //        }
                        //        else if (InvoicePK > 0)
                        //        {
                        //            GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);
                        //        }
                        //        //else//New
                        //        //{
                        //        //    HiddenField hdfInvoicePK = (HiddenField)((((Button)sender).Parent).FindControl("hdfInvoicePK"));
                        //        //    if (hdfInvoicePK != null && !string.IsNullOrEmpty(hdfInvoicePK.Value) && !hdfInvoicePK.Value.Equals("0"))
                        //        //    {
                        //        //        InvoicePK = Convert.ToInt64(hdfInvoicePK.Value);
                        //        //        GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);
                        //        //        if (FinInvoiceCusTrxMpgList != null && FinInvoiceCusTrxMpgList.Count > 0)
                        //        //        {
                        //        //            GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                        //        //            GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                        //        //            isSplitChanged = false;
                        //        //            SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);
                        //        //        }
                        //        //    }
                        //        //}
                        //    }
                        //    else
                        //    {
                        //        tempFinReceiptCusSoMpgList = tempInvoiceSOSplitList;
                        //    }
                        //    if (InvoicePK > 0)
                        //    {
                        //        GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                        //        GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                        //        isSplitChanged = false;
                        //        SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);
                        //    }
                        //}
                        //else if (InvoicePK > 0)//New
                        //{
                        //    GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);
                        //    GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                        //    GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                        //    isSplitChanged = false;
                        //    SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);
                        //}
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                        //                         "ClosePopup();", true);

                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUp]','" + GetLocalResourceObject("ReceiptSplit").ToString() + "','800','300');", true);
                        break;
                    #endregion
                    #region Currency Exchange
                    case ActionsEnum.RECEIVEDCURRENCY:
                        // hdfMultiCurrency.Value = "1";
                        SetMultiCurrencyConfiguration();
                        hdfInvoicePK = (HiddenField)((((Button)sender).Parent).FindControl("hdfInvoicePK"));
                        if (hdfInvoicePK != null && !string.IsNullOrEmpty(hdfInvoicePK.Value) && !hdfInvoicePK.Value.Equals("0"))
                        {
                            InvoicePK = Convert.ToInt64(hdfInvoicePK.Value);
                        }
                        else
                            InvoicePK = 0;
                        InvoiceDetails(InvoicePK);
                        // hdfMultiCurrency.Value = "0";
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUp]','" + GetLocalResourceObject("ReceiveOtherCurrency").ToString() + "','1050','300');", true);

                        break;
                    #endregion

                    #region PAYMENTSPLITSAVE
                    case ActionsEnum.PAYMENTSPLITSAVE:

                        ReceiptSplitSave(true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotalSplit", "$(document).ready(function(){CalculateTotalSplit();});", true);
                        //if (!IsValid)
                        //{
                        //    litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //}
                        //else//valid
                        //{
                        //    if (grdReceiptSplit.Rows.Count >= 1)
                        //    {
                        //        HiddenField lblTotalPayNowFooterSplit = (HiddenField)(grdReceiptSplit.FooterRow.FindControl("hdfTotalPayNowFooterSplit"));
                        //        if (lblTotalPayNowFooterSplit != null && !string.IsNullOrEmpty(lblTotalPayNowFooterSplit.Value) && !string.IsNullOrEmpty(lblInvSplitReceiveNow.Text))
                        //        {
                        //            if (Convert.ToDecimal(lblTotalPayNowFooterSplit.Value) == Convert.ToDecimal(lblInvSplitReceiveNow.Text.Trim().Replace(",", "")))
                        //            {
                        //                divErrorLabel.Visible = false;
                        //                finReceiptCusSoMpgList = new List<FIN_RECEIPT_CUS_SO_MPG>();
                        //                salesReceiptServiceClient = new SalesReceiptService();
                        //                salesReceiptServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        //                finReceiptCusSoMpgList = (List<FIN_RECEIPT_CUS_SO_MPG>)SetUIValuesToObject(ControlsEnum.PAYMENTSPLITLIST);
                        //                if (finReceiptCusSoMpgList != null)
                        //                {

                        //                    tempInvoiceSOSplitList = InvoiceSOSplitList;
                        //                    tempInvoiceSOSplitList.Where(dtl => dtl.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK)
                        //                        .ToList().ForEach(dtl => tempInvoiceSOSplitList.Remove(dtl));
                        //                    finReceiptCusSoMpgList.ForEach(dtl =>
                        //                    {
                        //                        dtl.FIN_RECEIPT_CUS_TRX_MPG = new FIN_RECEIPT_CUS_TRX_MPG()
                        //                        {
                        //                            RCM_INVOICE_HDR = InvoicePK
                        //                        };
                        //                        tempInvoiceSOSplitList.Add(dtl);
                        //                    });
                        //                    InvoiceSOSplitList = tempInvoiceSOSplitList;


                        //                    //Test
                        //                    //result = salesReceiptServiceClient.SaveReceiptSplit(finReceiptCusSoMpgList);
                        //                    //if (result >= 0)
                        //                    //{
                        //                    //    litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                        //                    //    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), GetLocalResourceObject("Receipt_Allocation").ToString());
                        //                    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                        //                                                                   "ClosePopup();", true);
                        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateAdjnFooter", "$(document).ready(function(){CalculateAdjnFooter();});", true);
                        //                    //}
                        //                }
                        //            }
                        //            else
                        //            {
                        //                SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);
                        //                //Label TotalPayNowFooterSplit = (Label)(grdReceiptSplit.FooterRow.FindControl("lblTotalPayNowFooterSplit"));
                        //                //TotalPayNowFooterSplit.Text = lblTotalPayNowFooterSplit.Value;
                        //                divErrorLabel.Visible = true;
                        //                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('Total split amount should be equal to Receive now amount','" + Resources.Messages.Information + "');", true);
                        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                        //                                                                    "ClosePopup();", true);
                        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUp]','" + GetLocalResourceObject("ReceiptSplit").ToString() + "','800','300');", true);
                        //            }

                        //        }
                        //        else
                        //        {

                        //            divErrorLabel.Visible = true;
                        //            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('Total split amount should be equal to Receive now amount','" + Resources.Messages.Information + "');", true);
                        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                        //                                                                        "ClosePopup();", true);
                        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUp]','" + GetLocalResourceObject("ReceiptSplit").ToString() + "','800','300');", true);
                        //        }
                        //    }
                        //    else
                        //    {

                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                        //                            "ClosePopup();", true);
                        //    }
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                        //                                                                     "ClosePopup();", true);
                        //}
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
                                Label lblBalanceAdjn = (Label)(grdReceiptSplitAdjn.FooterRow.FindControl("lblBalanceAdjn"));

                                HiddenField hdfTotalAllocateAdjn = (HiddenField)(grdReceiptSplitAdjn.FooterRow.FindControl("hdfTotalAllocateAdjn"));
                                HiddenField hdfBalanceAdjn = (HiddenField)(grdReceiptSplitAdjn.FooterRow.FindControl("hdfBalanceAdjn"));
                                HiddenField hdfReceivedNow = (HiddenField)(grdReceiptSplitAdjn.FooterRow.FindControl("hdfReceivedNow"));
                                if (Convert.ToDecimal(hdfBalanceAdjn.Value) >= Convert.ToDecimal(hdfTotalAllocateAdjn.Value))
                                {
                                    if ( (Convert.ToDecimal(hdfTotalAmtDtl.Value) + Convert.ToDecimal(hdfCrdrAlcnAmount.Value))
                                        >= Convert.ToDecimal(hdfTotalAllocateAdjn.Value))
                                    {
                                        if (Convert.ToDecimal(hdfBaltoAlloc.Value) >= Convert.ToDecimal(hdfTotalAllocateAdjn.Value))
                                        {
                                            if (lblTotalAllocateAdjn != null && !string.IsNullOrEmpty(lblTotalAllocateAdjn.Text))
                                            {
                                                divErrorLabel.Visible = false;
                                                FinReceiptCusAllocationList = new List<FIN_RECEIPT_CUS_ALCN_DTL>();
                                                salesReceiptServiceClient = new SalesReceiptService();
                                                salesReceiptServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(salesReceiptServiceClient);
                                                FinReceiptCusAllocationList = (List<FIN_RECEIPT_CUS_ALCN_DTL>)SetUIValuesToObject(ControlsEnum.ADJNSPLITLIST);
                                                int rowID = 0;
                                                #region check any other trx have already taken the amount or not
                                                HiddenField hdfCrDrPK;
                                                HiddenField hdfReceiptAdjnPK;

                                                TextBox txtAllocateAdjn;
                                                string wrongEntries = "";
                                                bool isCon = true;
                                                decimal AllocatedAmt = 0;
                                                foreach (GridViewRow grdrow in grdReceiptSplitAdjn.Rows)//
                                                {
                                                    hdfCrDrPK = (HiddenField)grdReceiptSplitAdjn.Rows[rowID].FindControl("hdfCrDrPK");
                                                    hdfReceiptAdjnPK = (HiddenField)grdReceiptSplitAdjn.Rows[rowID].FindControl("hdfReceiptAdjnPK");
                                                    lblBaltoRec = (Label)grdReceiptSplitAdjn.Rows[rowID].FindControl("lblBalanceAdjn");
                                                    txtAllocateAdjn = (TextBox)grdReceiptSplitAdjn.Rows[rowID].FindControl("txtAllocateAdjn");
                                                    if (hdfCrDrPK.Value == null || Convert.ToInt32(hdfCrDrPK.Value) == 0)
                                                    {
                                                        AllocatedAmt = ReceiptAdjnList.Where(sa => sa.RAD_ALCN_RECEIPT_TRX == Convert.ToInt32(hdfReceiptAdjnPK.Value) && sa.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR != InvoicePK).Sum(a => a.RAD_AMOUNT);
                                                        if (!string.IsNullOrEmpty(txtAllocateAdjn.Text))
                                                        {
                                                            if (Convert.ToDecimal(txtAllocateAdjn.Text) > 0)
                                                                if ((Convert.ToDecimal(lblBaltoRec.Text.Replace(",", "")) - AllocatedAmt) < (string.IsNullOrEmpty(txtAllocateAdjn.Text) ? 0 : Convert.ToDecimal(txtAllocateAdjn.Text)))
                                                                {
                                                                    isCon = false;
                                                                    //wrongEntries = wrongEntries + "," + ReceiptAdjnList.Where(sa => sa.RAD_ALCN_RECEIPT_TRX == Convert.ToInt32(hdfReceiptAdjnPK.Value)).FirstOrDefault().FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_NO;
                                                                }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        AllocatedAmt = ReceiptAdjnList.Where(sa => sa.RAD_ALCN_CDH == Convert.ToInt32(hdfCrDrPK.Value) && sa.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR != InvoicePK).Sum(a => a.RAD_AMOUNT);
                                                        if (!string.IsNullOrEmpty(txtAllocateAdjn.Text))
                                                        {
                                                            if (Convert.ToDecimal(txtAllocateAdjn.Text) > 0)
                                                                if ((Convert.ToDecimal(lblBaltoRec.Text.Replace(",", "")) - AllocatedAmt) < (string.IsNullOrEmpty(txtAllocateAdjn.Text) ? 0 : Convert.ToDecimal(txtAllocateAdjn.Text)))
                                                                {
                                                                    isCon = false;
                                                                    //wrongEntries = wrongEntries + "," + ReceiptAdjnList.Where(sa => sa.RAD_ALCN_CDH == Convert.ToInt32(hdfCrDrPK.Value)).FirstOrDefault().FIN_CRDR_NOTE_HDR.CDH_NO;
                                                                }
                                                        }
                                                    }
                                                    rowID++;
                                                }
                                                #endregion
                                                if (isCon == true)
                                                {
                                                    if (FinReceiptCusAllocationList != null && FinReceiptCusAllocationList.Count > 0)
                                                    {

                                                        tempReceiptAdjnList = ReceiptAdjnList;
                                                        tempReceiptAdjnList.Where(dtl => dtl.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK)
                                                            .ToList().ForEach(dtl => tempReceiptAdjnList.Remove(dtl));
                                                        FinReceiptCusAllocationList.ForEach(dtl =>
                                                        {
                                                            dtl.RAD_RECEIPT_TRX = ReceiptMpgPK;
                                                            dtl.FIN_RECEIPT_CUS_TRX_MPG = new FIN_RECEIPT_CUS_TRX_MPG()
                                                            {

                                                                RCM_PK = ReceiptMpgPK,
                                                                RCM_INVOICE_HDR = InvoicePK
                                                            };
                                                            tempReceiptAdjnList.Add(dtl);
                                                        });
                                                        ReceiptAdjnList = tempReceiptAdjnList;

                                                        if (TrxIndex >= 0)
                                                        {
                                                            Label lblAdjAmount = (Label)grdInvoiceList.Rows[TrxIndex].FindControl("lblAdjAmount");
                                                            Label BalancetoPay = (Label)grdInvoiceList.Rows[TrxIndex].FindControl("lblBaltoReceive");
                                                            hdfBaltoReceive = (HiddenField)grdInvoiceList.Rows[TrxIndex].FindControl("hdfBaltoReceive");
                                                            hdfIsApply = (HiddenField)grdInvoiceList.Rows[TrxIndex].FindControl("hdfIsApply");
                                                            hdfInvoicePK = (HiddenField)grdInvoiceList.Rows[TrxIndex].FindControl("hdfInvoicePK");
                                                            txtReceivedNow = (TextBox)grdInvoiceList.Rows[TrxIndex].FindControl("txtReceivedNow");
                                                            lblCrdrAlcnAmount = (Label)grdInvoiceList.Rows[TrxIndex].FindControl("lblCrdrAlcnAmount");

                                                            //HiddenField hdfReceivedNow = (HiddenField)grdInvoiceList.Rows[TrxIndex].FindControl("hdfReceivedNow");

                                                            lblAdjAmount.Text = lblAdjAmount.ToolTip = String.Format("{0:c}", Convert.ToDecimal(hdfTotalAllocateAdjn.Value));
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
                                                                        ////tempReceiptAdjnList = ReceiptAdjnList;
                                                                        ////tempReceiptAdjnList.Where(dtl => dtl.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK)
                                                                        ////    .ToList();

                                                                        //////tempInvoiceSOSplitList.Where(dtl => dtl.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK)
                                                                        //////  .ToList().ForEach(dtl => tempInvoiceSOSplitList.c(dtl));

                                                                        ////ReceiptAdjnList = tempReceiptAdjnList;
                                                                    }
                                                                }
                                                                BalancetoPay.Text = BalancetoPay.ToolTip = String.Format("{0:c}", (BalPay < 0 ? 0 : BalPay));
                                                                txtReceivedNow.Text = txtReceivedNow.ToolTip = Math.Round(BalPay < 0 ? 0 : BalPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();// AdjnNowAmount > 0 ? Math.Round(BalPay < 0 ? 0 : BalPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString() : txtReceivedNow.Text;
                                                            }

                                                            #region Reset Receipt Split after adj apply
                                                            tempInvoiceSOSplitList = InvoiceSOSplitList;
                                                            if (tempInvoiceSOSplitList != null && tempInvoiceSOSplitList.Count > 0)
                                                            {
                                                                tempInvoiceSOSplitList.Where(dtl => dtl.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK)
                                                                   .ToList().ForEach(dtl =>
                                                                   {
                                                                       dtl.RSO_RECEIVED_AMOUNT = 0;
                                                                       dtl.RSO_TAX_AMOUNT = 0;
                                                                       dtl.RSO_OTHER_AMOUNT = 0;
                                                                   });

                                                            }
                                                            InvoiceSOSplitList = tempInvoiceSOSplitList;
                                                            #endregion

                                                            #region Reset CR/DR Allocation
                                                            tempReceiptCrdrList = ReceiptCrdrList;
                                                            if (tempReceiptCrdrList != null && tempReceiptCrdrList.Where(r => r.RNM_INVOICE_HDR == InvoicePK).ToList().Count > 0)
                                                            {
                                                                tempReceiptCrdrList.Where(r => r.RNM_INVOICE_HDR == InvoicePK).ToList().ForEach(dtl =>
                                                                {
                                                                    dtl.RNM_ADJ_AMOUNT = 0;
                                                                    dtl.RNM_PAID_AMOUNT = 0;
                                                                });
                                                                ReceiptCrdrList = tempReceiptCrdrList;
                                                                decimal TotalCrdrAmnt = 0;
                                                                TotalCrdrAmnt = ReceiptCrdrList.Where(r => r.RNM_INVOICE_HDR == InvoicePK).Sum(r => r.RNM_PAID_AMOUNT + r.RNM_ADJ_AMOUNT);
                                                                lblCrdrAlcnAmount.Text = lblCrdrAlcnAmount.ToolTip = string.Format("{0:c}", TotalCrdrAmnt);
                                                                hdfCrdrAlcnAmount.Value = TotalCrdrAmnt.ToString();
                                                            }
                                                            #endregion

                                                            #region Reset Receipt SC Allocation
                                                            InvoiceDetails(InvoicePK);
                                                            ReceiptSplitSave(false);
                                                            #endregion
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

                                                //divErrorLabel.Visible = true;
                                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUp]','" + GetLocalResourceObject("ReceiptSplit").ToString() + "','800','300');", true);
                                            }
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
                    #region PRINT LISTING
                    case ActionsEnum.PRINTLISTING:
                        if (CurrPK == 0)
                        {
                            foreach (GridViewRow grdrow in grdPOReceiptHdr.Rows)
                            {
                                RadioButton rbtn;
                                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                                if (rbtn.Checked)
                                {
                                    bIsChecked = true;
                                    CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPaymentID")).Value);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            foreach (GridViewRow grdrow in grdPOReceiptHdr.Rows)
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
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.CRJ +
                                   "&APPSUBTYPE=1&TRXTYPE=" + ApplicationType.CRJ + "');", true);
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
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.CRJ +
                                   "&APPSUBTYPE=1&TRXTYPE=" + ApplicationType.CRJ + "');", true);
                        }

                        break;
                    #endregion
                    #region REVERSE
                    case ActionsEnum.REVERSE:
                        finReceiptCusHdrObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_HDR>();
                        finReceiptCusHdrObj.RCH_PK = CurrPK;
                        GetFieldValues(ControlsEnum.RECEIPTHDRENTRYBYPK);
                        if (finReceiptCusHdrList != null && finReceiptCusHdrList.Count == 1)
                        {
                            if (finReceiptCusHdrList[0].RCH_PDC >= 1)
                            {
                                if (finReceiptCusHdrList[0].RCH_HAS_JRNL_ENTRY)
                                {
                                    if (finReceiptCusHdrList[0].RCH_PDC == 1)
                                    {
                                        #region Update dummy entry
                                        FinTrxService finTrxServiceClient;
                                        finTrxServiceClient = new FinTrxService();
                                        string refType = string.Empty;
                                        refType = ApplicationType.PDCCJ;
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

                                        //FinTrxService finTrxServiceClient;
                                        //finTrxServiceClient = new FinTrxService();
                                        //finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                        //result = finTrxServiceClient.GenerateDummyEntry((int)CurrPK, ApplicationType.PDCCJ);
                                        ////if (result > 0)
                                        ////{
                                        ////    salesReceiptServiceClient = new SalesReceiptService();
                                        ////    salesReceiptServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(salesReceiptServiceClient);
                                        ////    result = salesReceiptServiceClient.UpdateReceiptHdrPDCFlag((int)CurrPK, 2);
                                        ////}
                                        ////else
                                        ////{
                                        ////    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_ReverseEntry").ToString();
                                        ////    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        ////    break;
                                        ////}
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
                    #region PayNow Change
                    case ActionsEnum.CHANGERECEIVENOW:
                        hdfInvoicePK = (HiddenField)(((sender as Button).Parent.Parent as GridViewRow).FindControl("hdfInvoicePK"));
                        lblCrdrAlcnAmount = (Label)(((sender as Button).Parent.Parent as GridViewRow).FindControl("lblCrdrAlcnAmount"));
                        if (hdfInvoicePK != null && !string.IsNullOrEmpty(hdfInvoicePK.Value) || !hdfInvoicePK.Value.Equals("0"))
                        {
                            InvoicePK = Convert.ToInt64(hdfInvoicePK.Value);
                            tempInvoiceSOSplitList = InvoiceSOSplitList;
                            //tempInvoiceSOSplitList.Where(dtl => dtl.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK)
                            //    .ToList().ForEach(dtl => dtl.RSO_RECEIVED_AMOUNT = 0);

                            //tempInvoiceSOSplitList.Where(dtl => dtl.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK)
                            //  .ToList().ForEach(dtl => tempInvoiceSOSplitList.c(dtl));

                            tempInvoiceSOSplitList.Where(dtl => dtl.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK)
                               .ToList().ForEach(dtl =>
                               {
                                   dtl.RSO_RECEIVED_AMOUNT = 0;
                                   dtl.RSO_TAX_AMOUNT = 0;
                                   dtl.RSO_OTHER_AMOUNT = 0;
                               });

                            InvoiceSOSplitList = tempInvoiceSOSplitList;

                            if (AppliedInvPkList != null && AppliedInvPkList.Count > 0)
                                AppliedInvPkList.Remove(InvoicePK);

                            #region Reset CR/DR Allocation
                            tempReceiptCrdrList = ReceiptCrdrList;
                            if (tempReceiptCrdrList != null && tempReceiptCrdrList.Where(r => r.RNM_INVOICE_HDR == InvoicePK).ToList().Count > 0)
                            {
                                tempReceiptCrdrList.Where(r => r.RNM_INVOICE_HDR == InvoicePK).ToList().ForEach(dtl =>
                                {
                                    dtl.RNM_ADJ_AMOUNT = 0;
                                    dtl.RNM_PAID_AMOUNT = 0;
                                });
                                ReceiptCrdrList = tempReceiptCrdrList;
                                decimal TotalCrdrAmnt = 0;
                                TotalCrdrAmnt = ReceiptCrdrList.Where(r => r.RNM_INVOICE_HDR == InvoicePK).Sum(r => r.RNM_PAID_AMOUNT + r.RNM_ADJ_AMOUNT);
                                lblCrdrAlcnAmount.Text = lblCrdrAlcnAmount.ToolTip = string.Format("{0:c}", TotalCrdrAmnt);
                                hdfCrdrAlcnAmount.Value = TotalCrdrAmnt.ToString();
                            }
                            #endregion
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CloseMsgPopup1",
                                                "CloseMsgPopup();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateAdjnFooter", "$(document).ready(function(){CalculateAdjnFooter();});", true);

                        break;
                    #endregion
                    #region CHEQUERETURN
                    case ActionsEnum.CHEQUERETURN:
                        finReceiptCusHdrObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_HDR>();
                        finReceiptCusHdrObj.RCH_PK = CurrPK;
                        //Check if adv deducted
                        GetFieldValues(ControlsEnum.ISADVDEDUCTED);
                        if (IsContReturn == true)
                        {
                            GetFieldValues(ControlsEnum.RECEIPTHDRENTRYBYPK);
                            if (finReceiptCusHdrList != null && finReceiptCusHdrList.Count == 1)
                            {
                                if (finReceiptCusHdrList[0].RCH_PDC != 1)
                                {
                                    if (finReceiptCusHdrList[0].RCH_HAS_JRNL_ENTRY)
                                    {
                                        if (finReceiptCusHdrList[0].RCH_BOUNCED == 0)
                                        {
                                            FinTrxService finTrxServiceClient;
                                            finTrxServiceClient = new FinTrxService();
                                            finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                            result = finTrxServiceClient.GenerateDummyEntry((int)CurrPK, ApplicationType.RCBJ);
                                            //if (result > 0)
                                            //{
                                            //    salesReceiptServiceClient = new SalesReceiptService();
                                            //    salesReceiptServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(salesReceiptServiceClient);
                                            //    //The following code is updated after cheque return voucher complete.
                                            //    //result = salesReceiptServiceClient.UpdateReceiptHdrBounceFlag((int)CurrPK, 1);
                                            //}
                                            //else if (result == -2)
                                            //{
                                            //    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_ReturnEntry").ToString();
                                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            //    break;
                                            //}
                                            //else
                                            //{
                                            //    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_ReturnEntry").ToString();
                                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            //    break;
                                            //}
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
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        //FillProcessID(11);//This line is included in SetUIEditView function.Issue => When 'Cancel Receipt' button is clicked without selecting any receipt a pop up msg appears. After clicking OK, select a receipt and then the page does not show any record.
                        SetUIEditView(commonActions);
                        ModifiedDatePnl.Visible = true;
                        mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);

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
                                vrfBankOfCheque.Enabled = false;
                                break;
                            case (int)ReceiptModeEnum.BANK:
                                vrfBankName.Enabled = true;
                                vrfBranch.Enabled = true;
                                vrfAccountNo.Enabled = true;
                                vrfInstrumentNo.Enabled = true;
                                vrfInstrumentDate.Enabled = false;
                                txtInstrumentNo.Enabled = true;
                                txtInstrumentNo.CssClass = "input-small";
                                txtInstrumentDate.Enabled = true;
                                txtInstrumentDate.CssClass = "input-small";
                                vrfBankOfCheque.Enabled = false;
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
                                vrfBankOfCheque.Enabled = true;
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
                                vrfBankOfCheque.Enabled = false;
                                break;
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
                    #region SHOWPOPUP
                    case ActionsEnum.SHOWPOPUP:
                        if (sender is LinkButton)
                        {
                            if (((LinkButton)sender).ID == "lnkInvoiceNo")
                            {
                                foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                                {
                                    HiddenField hdfCategory;
                                    HiddenField hdfInvTypeText;
                                    HiddenField hdfGroup;
                                    hdfCategory = (HiddenField)grdrow.FindControl("hdfCategory");//To sep: Adv Inv & Inv
                                    hdfInvTypeText = (HiddenField)grdrow.FindControl("hdfInvTypeText");//To Sep: type(Dom,Exp,Per)
                                    hdfGroup = (HiddenField)grdrow.FindControl("hdfGroup");//Group 3(Misc Inv)
                                    if (hdfGroup.Value == "3")// Misc Invoice
                                    {
                                        if (hdfInvTypeText.Value != string.Empty)
                                        {
                                            if (Convert.ToInt32(hdfInvTypeText.Value.ToString()) == (int)SalesInvoiceType.Domestic)
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                                    ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.MSI + "&APPSUBTYPE= " + (int)SalesInvoiceType.Domestic) + "');", true);
                                            }
                                            else if (Convert.ToInt32(hdfInvTypeText.Value.ToString()) == (int)SalesInvoiceType.Export || Convert.ToInt32(hdfInvTypeText.Value.ToString()) == (int)SalesInvoiceType.Deemed)
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                                    ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.MSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Export) + "');", true);
                                            }
                                            else if (Convert.ToInt32(hdfInvTypeText.Value.ToString()) == (int)SalesInvoiceType.Proforma)
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                                    ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.MSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Proforma) + "');", true);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (hdfCategory.Value == "1")//Invoice Category
                                        {
                                            if (hdfInvTypeText.Value != string.Empty)
                                            {
                                                if (Convert.ToInt32(hdfInvTypeText.Value.ToString()) == (int)SalesInvoiceType.Domestic)
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                                        ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Domestic) + "');", true);
                                                }
                                                else if (Convert.ToInt32(hdfInvTypeText.Value.ToString()) == (int)SalesInvoiceType.Export || Convert.ToInt32(hdfInvTypeText.Value.ToString()) == (int)SalesInvoiceType.Deemed)
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                                        ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Export) + "');", true);
                                                }
                                                else if (Convert.ToInt32(hdfInvTypeText.Value.ToString()) == (int)SalesInvoiceType.Proforma)
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                                        ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Proforma) + "');", true);
                                                }
                                            }
                                        }
                                        else if (hdfCategory.Value == "2")//Advance invoice Category
                                        {
                                            if (hdfInvTypeText.Value == "2" || hdfInvTypeText.Value == "3")
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" +
                                                    ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.SIJ + "&APPSUBTYPE=" +
                                                    Convert.ToInt32(ApplicationSubType.ADVANCEINVOICE) + "&TRXTYPE=" + ApplicationType.SIJ + "');", true);
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                                    ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=11") + "');", true);
                                            }
                                        }
                                    }
                                    return;
                                }
                            }
                        }
                        else if (sender is ImageButton)
                        {
                            if (((ImageButton)sender).ID == "imbSuspencelist")
                            {
                                btnListApply.Visible = EntryStatus == EntryStatus.VIEWMODE ? false : true;
                                GetFieldValues(ControlsEnum.SUSPENCELIST);
                                SetFieldValues(ControlsEnum.SUSPENCELIST);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSuspenceList]','" + GetLocalResourceObject("SuspenceList").ToString() + "','600','300');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateSuspenceGridTotal", "CalculateSuspenceGridTotal();", true);
                            }
                        }
                        break;
                    #endregion
                    #region CR/DR ALLOCATION
                    case ActionsEnum.CRDRALLOCATION:
                        divCrdrErrorMsg.Visible = false;
                        FinReceiptCusCrdrMpgList = null;
                        hdfInvoicePK = (HiddenField)((((Button)sender).Parent).FindControl("hdfInvoicePK"));
                        lblTotalAmount = (Label)((((Button)sender).Parent).FindControl("lblTotalAmount"));
                        txtReceivedNow = (TextBox)((((Button)sender).Parent).FindControl("txtReceivedNow"));
                        //LinkButton lnkInvNo = (LinkButton)((((Button)sender).Parent).FindControl("lnkInvoiceNo"));
                        //Label lblReceived = (Label)((((Button)sender).Parent).FindControl("lblReceived"));
                        LinkButton lnkReceived = (LinkButton)((((Button)sender).Parent).FindControl("lnkReceived"));
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
                            //lblInvSplitNo_CrdrAlcn.Text = lblInvSplitNo_CrdrAlcn.ToolTip = lnkInvNo.Text;
                            lblInvSplitAmount_CrdrAlcn.Text = lblInvSplitAmount_CrdrAlcn.ToolTip = lblTotalAmount.Text;
                            lblInvSplitReceived_CrdrAlcn.Text = lblInvSplitReceived_CrdrAlcn.ToolTip = lnkReceived.Text;
                            lblInvSplitReceiveNow_CrdrAlcn.Text = lblInvSplitReceiveNow_CrdrAlcn.ToolTip = String.Format("{0:c}", PayNowAmount);
                            GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                            GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                        }
                        if (ReceiptCrdrList != null && ReceiptCrdrList.Count > 0)
                            FinReceiptCusCrdrMpgList = ReceiptCrdrList.Where(dtl => dtl.RNM_INVOICE_HDR == InvoicePK).ToList();
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
                                    tempReceiptCrdrList = ReceiptCrdrList.DeepClone();
                                    FinReceiptCusCrdrMpgList.ForEach(dtl =>
                                    {
                                        ReceiptCrdrMpg objReceiptCrdrMpg = tempReceiptCrdrList.SingleOrDefault(r => r.RNM_INVOICE_HDR == InvoicePK && r.RNM_CRDR_MPG == dtl.RNM_CRDR_MPG);
                                        objReceiptCrdrMpg.RNM_ADJ_AMOUNT = dtl.RNM_ADJ_AMOUNT;
                                        objReceiptCrdrMpg.RNM_PAID_AMOUNT = dtl.RNM_PAID_AMOUNT;
                                        CrAlcnAmount += (dtl.RNM_ADJ_AMOUNT + dtl.RNM_PAID_AMOUNT);
                                        TotalCrReceiveNow += dtl.RNM_PAID_AMOUNT;
                                        TotalCrAdj += dtl.RNM_ADJ_AMOUNT;
                                    });

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
                                            hdfCrdrAlcnAmount.Value = CrAlcnAmount.ToString();
                                            ReceiptCrdrList = tempReceiptCrdrList;
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
                    #region EDITFORRETURN
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
                    #region PRINTINVOICE
                    case ActionsEnum.PRINTINVOICE:
                        HiddenField hdfinvPK = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfListinvPK"));
                        HiddenField hdfInvType = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfListInvType"));  //To Sep: type(Dom,Exp,Per)
                        HiddenField hdfinvCategory = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfListinvCategory"));  //To sep: Adv Inv & Inv
                        HiddenField hdfinvGroup = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfListGroup")); //Group 3(Misc Inv)
                        if (hdfinvGroup.Value == "3")// Misc Invoice
                        {
                            if (hdfInvType.Value != string.Empty)
                            {
                                if (Convert.ToInt32(hdfInvType.Value.ToString()) == (int)SalesInvoiceType.Domestic)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        hdfinvPK.Value + "&APPTYPE=" + ApplicationType.MSI + "&APPSUBTYPE= " + (int)SalesInvoiceType.Domestic) + "');", true);
                                }
                                else if (Convert.ToInt32(hdfInvType.Value.ToString()) == (int)SalesInvoiceType.Export || Convert.ToInt32(hdfInvType.Value.ToString()) == (int)SalesInvoiceType.Deemed)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        hdfinvPK.Value + "&APPTYPE=" + ApplicationType.MSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Export) + "');", true);
                                }
                                else if (Convert.ToInt32(hdfInvType.Value.ToString()) == (int)SalesInvoiceType.Proforma)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        hdfinvPK.Value + "&APPTYPE=" + ApplicationType.MSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Proforma) + "');", true);
                                }
                            }
                        }
                        else
                        {
                            if (hdfinvCategory.Value == "1")//Invoice Category
                            {
                                if (hdfInvType.Value != string.Empty)
                                {
                                    if (Convert.ToInt32(hdfInvType.Value.ToString()) == (int)SalesInvoiceType.Domestic)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                            hdfinvPK.Value + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Domestic) + "');", true);
                                    }
                                    else if (Convert.ToInt32(hdfInvType.Value.ToString()) == (int)SalesInvoiceType.Export || Convert.ToInt32(hdfInvType.Value.ToString()) == (int)SalesInvoiceType.Deemed)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                            hdfinvPK.Value + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Export) + "');", true);
                                    }
                                    else if (Convert.ToInt32(hdfInvType.Value.ToString()) == (int)SalesInvoiceType.Proforma)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                            hdfinvPK.Value + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Proforma) + "');", true);
                                    }
                                }
                            }
                            else if (hdfinvCategory.Value == "2")//Advance invoice Category
                            {
                                if (hdfInvType.Value == "2" || hdfInvType.Value == "3")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" +
                                        hdfinvPK.Value + "&APPTYPE=" + ApplicationType.SIJ + "&APPSUBTYPE=" +
                                        Convert.ToInt32(ApplicationSubType.ADVANCEINVOICE) + "&TRXTYPE=" + ApplicationType.SIJ + "');", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        hdfinvPK.Value + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=11") + "');", true);
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
                            finReceiptSuspList = new List<FIN_RECEIPT_CUS_SUSP_DTL>();
                            finReceiptSuspList = (List<FIN_RECEIPT_CUS_SUSP_DTL>)SetUIValuesToObject(ControlsEnum.SUSPENCELIST);
                            txtSuspenceAmt.Text = GetFormattedCurrency(finReceiptSuspList.Sum(x => x.RSC_AMOUNT));
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

        /// <summary>
        /// Receipt split apply
        /// </summary>
        private void ReceiptSplitSave(bool ShowMsg)
        {
            SalesReceiptService salesReceiptServiceClient;
            salesReceiptServiceClient = null;
            List<FIN_RECEIPT_CUS_SO_MPG> tempInvoiceSOSplitList;

            if (!IsValid)
            {
                litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
            }
            else//valid
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
                    if (ReceiptCrdrList != null && ReceiptCrdrList.Count > 0)
                    {
                        CrAllocatedAmnt = ReceiptCrdrList.Where(r => r.RNM_INVOICE_HDR == InvoicePK).Sum(sm => sm.RNM_PAID_AMOUNT + sm.RNM_ADJ_AMOUNT);

                    }
                    lblTotalPayNowFooterSplit.Value = TotalReceiveNowSplit.ToString();
                    if (lblTotalPayNowFooterSplit != null && !string.IsNullOrEmpty(lblTotalPayNowFooterSplit.Value) && !string.IsNullOrEmpty(lblInvSplitReceiveNow.Text))
                    {
                        InvReceiveNow = Convert.ToDecimal(lblInvSplitReceiveNow.Text.Trim().Replace(",", ""));
                        InvReceiveNow += AdjustmentAmount;
                        InvReceiveNow = (InvReceiveNow - CrAllocatedAmnt) < 0 ? 0 : (InvReceiveNow - CrAllocatedAmnt);
                        if (Convert.ToDecimal(lblTotalPayNowFooterSplit.Value) <= InvReceiveNow) //if (Convert.ToDecimal(lblTotalPayNowFooterSplit.Value) <= Convert.ToDecimal(lblInvSplitReceiveNow.Text.Trim().Replace(",", "")))
                        {
                            divErrorLabel.Visible = false;
                            finReceiptCusSoMpgList = new List<FIN_RECEIPT_CUS_SO_MPG>();
                            salesReceiptServiceClient = new SalesReceiptService();
                            salesReceiptServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(salesReceiptServiceClient);
                            finReceiptCusSoMpgList = (List<FIN_RECEIPT_CUS_SO_MPG>)SetUIValuesToObject(ControlsEnum.PAYMENTSPLITLIST);
                            if (finReceiptCusSoMpgList != null)
                            {

                                tempInvoiceSOSplitList = InvoiceSOSplitList;
                                tempInvoiceSOSplitList.Where(dtl => dtl.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK)
                                    .ToList().ForEach(dtl => tempInvoiceSOSplitList.Remove(dtl));
                                finReceiptCusSoMpgList.ForEach(dtl =>
                                {
                                    dtl.FIN_RECEIPT_CUS_TRX_MPG = new FIN_RECEIPT_CUS_TRX_MPG()
                                    {
                                        RCM_INVOICE_HDR = InvoicePK
                                    };
                                    tempInvoiceSOSplitList.Add(dtl);
                                });
                                InvoiceSOSplitList = tempInvoiceSOSplitList;

                                if (AppliedInvPkList == null)
                                    AppliedInvPkList = new List<long>();
                                if (!AppliedInvPkList.Contains(InvoicePK))
                                {
                                    AppliedInvPkList.Add(InvoicePK);  // To keep applied invoice Pks
                                }

                                //Test
                                //result = salesReceiptServiceClient.SaveReceiptSplit(finReceiptCusSoMpgList);
                                //if (result >= 0)
                                //{
                                //    litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                //    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), GetLocalResourceObject("Receipt_Allocation").ToString());
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                salesReceiptServiceClient = null;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateAdjnFooter", "$(document).ready(function(){CalculateAdjnFooter();});", true);
                                //}
                            }
                        }
                        else
                        {
                            ////SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);
                            //Label TotalPayNowFooterSplit = (Label)(grdReceiptSplit.FooterRow.FindControl("lblTotalPayNowFooterSplit"));
                            //TotalPayNowFooterSplit.Text = lblTotalPayNowFooterSplit.Value;
                            if (ShowMsg)
                            {
                                divErrorLabel.Visible = true;
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('Total split amount should be equal to Receive now amount','" + Resources.Messages.Information + "');", true);
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
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('Total split amount should be equal to Receive now amount','" + Resources.Messages.Information + "');", true);
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
            List<FIN_RECEIPT_CUS_SO_MPG> tempInvoiceSOSplitList = new List<FIN_RECEIPT_CUS_SO_MPG>();
            decimal InvTotalOtherAmnt = 0;
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

            //divErrorLabel.Visible = false;
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

            //hdfInvoicePK = (HiddenField)((((Button)sender).Parent).FindControl("hdfInvoicePK"));
            //if (hdfInvoicePK != null && !string.IsNullOrEmpty(hdfInvoicePK.Value) && !hdfInvoicePK.Value.Equals("0"))
            //{
            //    InvoicePK = Convert.ToInt64(hdfInvoicePK.Value);
            //}
            //else
            //    InvoicePK = 0;
            tempInvoiceSOSplitList = InvoiceSOSplitList;
            tempFinReceiptCusSoMpgList = tempInvoiceSOSplitList.Where(dtl => dtl.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK).ToList();



            if (hdfReceiptMpgPK != null && !string.IsNullOrEmpty(hdfReceiptMpgPK.Value) && !hdfReceiptMpgPK.Value.Equals("0"))
            {
                ReceiptMpgPK = Convert.ToInt64(hdfReceiptMpgPK.Value);
                GetFieldValues(ControlsEnum.PAYMENTSPLITLIST);

                if (tempFinReceiptCusSoMpgList == null || tempFinReceiptCusSoMpgList.Count == 0)
                {
                    if (finReceiptCusSoMpgList != null && finReceiptCusSoMpgList.Count > 0)//Edit
                    {
                        tempInvoiceSOSplitList.Where(dtl => dtl.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK)
                                        .ToList().ForEach(dtl => tempInvoiceSOSplitList.Remove(dtl));
                        finReceiptCusSoMpgList.ForEach(dtl =>
                        {
                            dtl.FIN_RECEIPT_CUS_TRX_MPG = new FIN_RECEIPT_CUS_TRX_MPG()
                            {
                                RCM_INVOICE_HDR = InvoicePK
                            };
                            tempInvoiceSOSplitList.Add(dtl);
                        });
                        InvoiceSOSplitList = tempInvoiceSOSplitList;
                        //HiddenField hdfInvoicePK = (HiddenField)((((Button)sender).Parent).FindControl("hdfInvoicePK"));
                        //if (hdfInvoicePK != null && !string.IsNullOrEmpty(hdfInvoicePK.Value) && !hdfInvoicePK.Value.Equals("0"))
                        //{
                        //    InvoicePK = Convert.ToInt64(hdfInvoicePK.Value);
                        //    GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                        //    GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                        //    isSplitChanged = false;
                        //    SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);
                        //}
                    }
                    else if (InvoicePK > 0)
                    {
                        GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);
                    }
                    //else//New
                    //{
                    //    HiddenField hdfInvoicePK = (HiddenField)((((Button)sender).Parent).FindControl("hdfInvoicePK"));
                    //    if (hdfInvoicePK != null && !string.IsNullOrEmpty(hdfInvoicePK.Value) && !hdfInvoicePK.Value.Equals("0"))
                    //    {
                    //        InvoicePK = Convert.ToInt64(hdfInvoicePK.Value);
                    //        GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);
                    //        if (FinInvoiceCusTrxMpgList != null && FinInvoiceCusTrxMpgList.Count > 0)
                    //        {
                    //            GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                    //            GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                    //            isSplitChanged = false;
                    //            SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);
                    //        }
                    //    }
                    //}
                }
                else
                {
                    tempFinReceiptCusSoMpgList = tempInvoiceSOSplitList;
                }
                if (InvoicePK > 0)
                {
                    GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                    GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                    isSplitChanged = false;
                    SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);

                    GetUIValuesFromObject(ControlsEnum.SPLITPAYNOWFORMULISO);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalSplit", "$(document).ready(function(){CalculateTotalSplit();});", true);
                }
            }
            else if (InvoicePK > 0)//New
            {
                GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);
                GetFieldValues(ControlsEnum.INVOICEVNDHDR);
                GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                isSplitChanged = false;
                SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);

                GetUIValuesFromObject(ControlsEnum.SPLITPAYNOWFORMULISO);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalSplit", "$(document).ready(function(){CalculateTotalSplit();});", true);

            }
            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
            //                         "ClosePopup();", true);

            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUp]','" + GetLocalResourceObject("ReceiptSplit").ToString() + "','800','300');", true);

        }

        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            Label lblTotalFooter;
            Label lblTotalAllocateAdjn;


            decimal total;
            decimal taxpercentage;
            decimal basevalue;
            decimal taxamt = 0;
            HiddenField hdfCategory;
            HiddenField hdfType;
            HiddenField hdfTotalAllocateAdjn;
            HiddenField hdfBalanceAdjn;
            HiddenField hdfTotalAmt;
            HiddenField hdfTaxAmt;
            HiddenField hdfGroup;
            HiddenField hdfInvoicePK;
            HiddenField hdfReceiptMpgPK;
            LinkButton lnkInvoiceNo;
            HiddenField hdfBaltoReceive;
            HiddenField hdfInvTypeText;
            //Label lblInvoiceNo;
            Label lblInvoiceDate;
            Label lblCustomerInv;
            Label lblInvCurrency;
            Label lblCmpDisplayCode;
            Label lblTotalAmount;
            Label lblReceived;
            Label lblTotalTaxAmount;
            Label lblBaltoReceive;
            TextBox txtReceivedNow;
            Button lnkRemove;
            // Button lnkRecCurAllocation;
            HiddenField hdfReceivedNow;
            //Splitup
            HiddenField hdfSOPK;
            Label lblPONOSplit;
            Label lblPODateSplit;
            Label lblCurrSplit;
            Label lblAmountSplit;
            Label lblPaidSplit;
            Label lblBalanceSplit;
            TextBox txtPayNowSplit;
            TextBox txtReceivedInBaseCurrency;
            TextBox txtAllocatedReceiptAmount;
            TextBox txtEquivalentInvoiceAmount;
            TextBox txtGainOrLoss;
            HiddenField hdfPayNowSplit;
            HiddenField hdfReceiptSplitPK;
            HiddenField hdfReceiptTRXPK;
            Label lblTotalPayNowFooterSplit;

            Button lnkAllocation;
            Button btnCrdrAllocation;
            HiddenField hdfTotalPayNowFooterSplit;
            Label lblAdjAmount;
            SPADM_APP_STATUS_CFG_GET_KV_Result wkfStatus;
            Label lblTax;
            TextBox txtAdjustments;
            TextBox txtOthercharges;
            CustomValidator vcmReceiveNow;

            Label lblOtherAmount;
            HiddenField hdfOtherchargeOLD;
            TextBox txtOtherChargesSplit;
            Label lblTaxSplit;
            Label lblDiscountSplit;
            HiddenField hdfShippingrChargesSplitPercent;

            int category = 0;
            int type = 0;

            FIN_RECEIPT_CUS_SO_MPG tempFinReceiptCusSoMpgObj = null;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    if (((GridView)sender).ID == "grdInvoiceList")
                    {

                        string sodate;
                        hdfCategory = e.Row.FindControl("hdfCategory") as HiddenField;
                        hdfType = e.Row.FindControl("hdfType") as HiddenField;
                        hdfGroup = e.Row.FindControl("hdfGroup") as HiddenField;
                        hdfTotalAmt = e.Row.FindControl("hdfTotalAmt") as HiddenField;
                        hdfBaltoReceive = e.Row.FindControl("hdfBaltoReceive") as HiddenField;
                        hdfTaxAmt = e.Row.FindControl("hdfTaxAmt") as HiddenField;
                        hdfInvoicePK = e.Row.FindControl("hdfInvoicePK") as HiddenField;
                        hdfReceiptMpgPK = e.Row.FindControl("hdfReceiptMpgPK") as HiddenField;
                        lnkInvoiceNo = e.Row.FindControl("lnkInvoiceNo") as LinkButton;
                        hdfInvTypeText = e.Row.FindControl("hdfInvTypeText") as HiddenField;
                        //lblInvoiceNo = e.Row.FindControl("lblInvoiceNo") as Label;
                        lblInvoiceDate = e.Row.FindControl("lblInvoiceDate") as Label;
                        lblCustomerInv = e.Row.FindControl("lblCustomerInv") as Label;
                        lblInvCurrency = e.Row.FindControl("lblInvCurrency") as Label;
                        lblCmpDisplayCode = e.Row.FindControl("lblCmpDisplayCode") as Label;
                        lblTotalAmount = e.Row.FindControl("lblTotalAmount") as Label;
                        lblTotalTaxAmount = e.Row.FindControl("lblTotalTaxAmount") as Label;
                        lblReceived = e.Row.FindControl("lblReceived") as Label;
                        LinkButton lnkReceived = e.Row.FindControl("lnkReceived") as LinkButton;
                        lblBaltoReceive = e.Row.FindControl("lblBaltoReceive") as Label;
                        txtReceivedNow = e.Row.FindControl("txtReceivedNow") as TextBox;
                        lnkAllocation = e.Row.FindControl("lnkAllocation") as Button;
                        btnCrdrAllocation = e.Row.FindControl("btnCrdrAllocation") as Button;
                        vcmReceiveNow = e.Row.FindControl("vcmReceiveNow") as CustomValidator;
                        lnkRemove = e.Row.FindControl("lnkRemove") as Button;
                        //     lnkRecCurAllocation = e.Row.FindControl("lnkRecCurAllocation") as Button;
                        hdfReceivedNow = e.Row.FindControl("hdfReceivedNow") as HiddenField;
                        lblAdjAmount = e.Row.FindControl("lblAdjAmount") as Label;

                        lblTax = e.Row.FindControl("lblTax") as Label;
                        txtAdjustments = e.Row.FindControl("txtAdjustments") as TextBox;
                        lblOtherAmount = e.Row.FindControl("lblOtherAmount") as Label;
                        hdfOtherchargeOLD = e.Row.FindControl("hdfOtherchargeOLD") as HiddenField;

                        txtOthercharges = e.Row.FindControl("txtOthercharges") as TextBox;
                        Label lblCrdrAlcnAmount = e.Row.FindControl("lblCrdrAlcnAmount") as Label;

                        if (finInvoiceCusHdrList != null && finInvoiceCusHdrList.Count > 0)
                        {
                            lblCustomerTxt.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceCusHdrList[0].CRM_CUSTOMER_MST.CUS_NAME, 35);
                            lblCustomerTxt.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceCusHdrList[0].CRM_CUSTOMER_MST.CUS_NAME, 300);
                            hdfCusPK.Value = finInvoiceCusHdrList[0].ICH_CUSTOMER.ToString();

                            decimal lineItemTax = finInvoiceCusHdrList[e.Row.RowIndex].FIN_INVOICE_CUS_DTL.Sum(ss => ss.CID_TAX);
                            decimal lineItemDiscount = finInvoiceCusHdrList[e.Row.RowIndex].FIN_INVOICE_CUS_DTL.Sum(ss => ss.CID_DISCOUNT);

                            hdfCategory.Value = finInvoiceCusHdrList[e.Row.RowIndex].ICH_CATEGORY.ToString();
                            hdfGroup.Value = finInvoiceCusHdrList[e.Row.RowIndex].ICH_GROUP.ToString();
                            hdfType.Value = finInvoiceCusHdrList[e.Row.RowIndex].ICH_TYPE.ToString();
                            hdfTotalAmt.Value = (finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_TC - (finInvoiceCusHdrList[e.Row.RowIndex].ICH_DISCOUNT_TC + lineItemDiscount)).ToString();
                            hdfTaxAmt.Value = finInvoiceCusHdrList[e.Row.RowIndex].ICH_TAX_TC.ToString() == string.Empty ? (0 + lineItemTax).ToString() : (finInvoiceCusHdrList[e.Row.RowIndex].ICH_TAX_TC + lineItemTax).ToString();
                            hdfInvoicePK.Value = finInvoiceCusHdrList[e.Row.RowIndex].ICH_PK.ToString();
                            hdfReceiptMpgPK.Value = "0";
                            sodate = finInvoiceCusHdrList[e.Row.RowIndex].ICH_DATE_PAY_BY.ToString(Resources.Constants.DateFormatShort);

                            lnkInvoiceNo.Text = finInvoiceCusHdrList[e.Row.RowIndex].ICH_NO;
                            lnkInvoiceNo.ToolTip = finInvoiceCusHdrList[e.Row.RowIndex].ICH_NO + "  " + sodate;
                            lnkInvoiceNo.CommandArgument = finInvoiceCusHdrList[e.Row.RowIndex].ICH_PK.ToString();
                            hdfInvTypeText.Value = finInvoiceCusHdrList[e.Row.RowIndex].ICH_TYPE.ToString();

                            //lblInvoiceNo.Text = finInvoiceCusHdrList[e.Row.RowIndex].ICH_NO;
                            //lblInvoiceNo.ToolTip = finInvoiceCusHdrList[e.Row.RowIndex].ICH_NO + "  " + sodate;
                            //lblInvoiceDate.Text = finInvoiceCusHdrList[e.Row.RowIndex].ICH_DATE.ToString(Resources.Constants.DateFormatShort);
                            //lblInvoiceDate.ToolTip = finInvoiceCusHdrList[e.Row.RowIndex].ICH_DATE.ToString(Resources.Constants.DateFormatShort);

                            lblInvoiceDate.Text = finInvoiceCusHdrList[e.Row.RowIndex].ICH_DATE_PAY_BY.ToString(Resources.Constants.DateFormatShort);
                            lblInvoiceDate.ToolTip = finInvoiceCusHdrList[e.Row.RowIndex].ICH_DATE_PAY_BY.ToString(Resources.Constants.DateFormatShort);


                            lblCustomerInv.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceCusHdrList[e.Row.RowIndex].CRM_CUSTOMER_MST.CUS_NAME, 18);
                            lblCustomerInv.ToolTip = finInvoiceCusHdrList[e.Row.RowIndex].CRM_CUSTOMER_MST.CUS_NAME;
                            lblInvCurrency.Text = finInvoiceCusHdrList[e.Row.RowIndex].ADM_CURRENCY_MST1.CUR_CODE;
                            lblInvCurrency.ToolTip = finInvoiceCusHdrList[e.Row.RowIndex].ADM_CURRENCY_MST1.CUR_CODE;

                            lblCmpDisplayCode.Text = lblCmpDisplayCode.ToolTip = finInvoiceCusHdrList[e.Row.RowIndex].ADM_COMPANY_MST.CMP_DISPLAY_CODE;
                            lblCmpDisplayCode.CssClass = finInvoiceCusHdrList[e.Row.RowIndex].ADM_COMPANY_MST.CMP_LINE_COLOUR;

                            lblTotalAmount.Text = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_NET_TC);
                            totalAmtFooter = totalAmtFooter + Convert.ToDecimal(lblTotalAmount.Text);
                            lblTotalAmount.ToolTip = Math.Round(finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_NET_TC,
                                Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();



                            lblTotalTaxAmount.Text = String.Format("{0:c}", Convert.ToDecimal(finInvoiceCusHdrList[e.Row.RowIndex].ICH_TAX_TC.ToString() == string.Empty ? (0 + lineItemTax).ToString() : (finInvoiceCusHdrList[e.Row.RowIndex].ICH_TAX_TC + lineItemTax).ToString()));
                            totalTaxFooter = totalTaxFooter + Convert.ToDecimal(lblTotalTaxAmount.Text);

                            lblTotalTaxAmount.ToolTip = lblTotalTaxAmount.Text;
                            lblReceived.Text = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_RCVD_TC);
                            lblReceived.ToolTip = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_RCVD_TC);
                            lnkReceived.Text = lnkReceived.ToolTip = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_RCVD_TC + finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_CN_TC);
                            totalReceivedFooter = totalReceivedFooter + Convert.ToDecimal(lnkReceived.Text.Replace(",", ""));//lblReceived.Text

                            finReceiptCusTrxMpgList = null;
                            finReceiptCusTrxMpgList = finInvoiceCusHdrList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG != null ? finInvoiceCusHdrList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG.ToList() : null;


                            decimal balToPay = 0;
                            decimal prevOtherCharges = 0;
                            if ((finReceiptCusTrxMpgList != null && finReceiptCusTrxMpgList.Count > 1) || finReceiptCusTrxMpgList == null || finReceiptCusTrxMpgList.Count == 0)
                            {
                                balToPay = finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_NET_TC -
                                              finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_RCVD_TC -
                                              finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_CN_TC +
                                              finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_DN_TC;

                            }
                            else if (finReceiptCusTrxMpgList != null && finReceiptCusTrxMpgList.Count == 1)
                            {
                                balToPay = finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_NET_TC -
                                               finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_RCVD_TC -
                                               finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_CN_TC +
                                               finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_DN_TC;
                                prevOtherCharges = finReceiptCusTrxMpgList[0].FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0 ? finReceiptCusTrxMpgList[0].RCM_OTHER_AMOUNT : 0;
                            }

                            //decimal balToPay = finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_NET_TC - finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_RCVD_TC;
                            lblBaltoReceive.Text = String.Format("{0:c}", balToPay);
                            hdfBaltoReceive.Value = balToPay.ToString();
                            lblBaltoReceive.ToolTip = String.Format("{0:c}", balToPay);
                            balRecieveFooter = balRecieveFooter + Convert.ToDecimal(lblBaltoReceive.Text);

                            balToPay = balToPay < 0 ? 0 : balToPay;
                            if (dicTempAmount != null && dicTempAmount.Count > 0 && dicTempAmount.ContainsKey(hdfInvoicePK.Value))
                            {
                                balToPay = dicTempAmount.SingleOrDefault(aa => aa.Key == hdfInvoicePK.Value).Value;
                            }
                            else
                            {
                                balToPay = balToPay < 0 ? 0 : balToPay;
                            }
                            txtReceivedNow.Text = Math.Round(balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            txtReceivedNow.ToolTip = Math.Round(balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            hdfReceivedNow.Value = Math.Round(balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            lnkRemove.CommandArgument = finInvoiceCusHdrList[e.Row.RowIndex].ICH_PK.ToString();
                            //lnkAllocation.Visible = false;
                            lnkAllocation.Visible = SOGroup != SalesInvoiceGroup.Miscellaneous; //vcmReceiveNow.Enabled = 
                            //btnCrdrAllocation.Visible = SOGroup != SalesInvoiceGroup.Miscellaneous;
                            lblAdjAmount.Text = lblAdjAmount.ToolTip = "0.00";

                            txtAdjustments.Text = "0.00";
                            txtAdjustments.ToolTip = txtAdjustments.Text;


                            taxpercentage = Convert.ToDecimal(hdfTaxAmt.Value) / (Convert.ToDecimal(finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_TC - finInvoiceCusHdrList[e.Row.RowIndex].ICH_DISCOUNT_TC) == 0 ? 1 : Convert.ToDecimal(finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_TC - finInvoiceCusHdrList[e.Row.RowIndex].ICH_DISCOUNT_TC));
                            basevalue = (txtReceivedNow.Text != string.Empty ? Convert.ToDecimal(txtReceivedNow.Text) : 0) / (1 + taxpercentage);
                            taxamt = ((txtReceivedNow.Text != string.Empty ? Convert.ToDecimal(txtReceivedNow.Text) : 0) - basevalue);

                            lblTax.Text = String.Format("{0:c}", taxamt);
                            lblTax.ToolTip = lblTax.Text;
                            //new
                            decimal otherChargesPrev = 0;
                            //OtherCharges
                            long? ICH_PK = finInvoiceCusHdrList[e.Row.RowIndex].ICH_PK;
                            if (finInvoiceCusHdrList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG != null)
                            {
                                otherChargesPrev = (finInvoiceCusHdrList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG.Where(pp => pp.RCM_RECEIPT_HDR == ICH_PK && pp.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0).ToList()).Sum(pv => pv.RCM_OTHER_AMOUNT);
                            }
                            decimal otherCharges = finInvoiceCusHdrList[e.Row.RowIndex].ICH_DEL_STATUS == 0 ? finInvoiceCusHdrList[e.Row.RowIndex].ICH_SHIP_CHARGE : 0;
                            lblOtherAmount.Text = String.Format("{0:c}", otherCharges);
                            lblOtherAmount.ToolTip = String.Format("{0:c}", otherCharges);

                            totalOtheramtFooter = totalOtheramtFooter + Convert.ToDecimal(lblOtherAmount.Text);
                            otherCharges -= otherChargesPrev;
                            otherCharges = otherCharges < 0 ? 0 : otherCharges;
                            txtOthercharges.Text = String.Format("{0:c}", otherCharges - prevOtherCharges);
                            txtOthercharges.ToolTip = String.Format("{0:c}", otherCharges - prevOtherCharges);
                            hdfOtherchargeOLD.Value = prevOtherCharges.ToString();
                            //End new

                            //debit note allocated amount                            
                            //lblCrdrAlcnAmount.Text = lblCrdrAlcnAmount.ToolTip = String.Format("{0:c}", 0);

                            //debit note allocated amount
                            decimal CrdrAlcnAmnt = 0;
                            if (ReceiptCrdrList != null && ReceiptCrdrList.Count > 0)
                            {
                                CrdrAlcnAmnt = ReceiptCrdrList.Where(r => r.RNM_INVOICE_HDR == finInvoiceCusHdrList[e.Row.RowIndex].ICH_PK).Sum(sm => sm.RNM_PAID_AMOUNT + sm.RNM_ADJ_AMOUNT);
                            }
                            lblCrdrAlcnAmount.Text = lblCrdrAlcnAmount.ToolTip = String.Format("{0:c}", CrdrAlcnAmnt);
                            hdfCrdrAlcnAmount.Value = CrdrAlcnAmnt.ToString();

                            //lblOtherAmount.Text = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].FIN_INVOICE_CUS_TRX_MPG.Where(tm => tm.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0).Sum(tm => tm.ICM_OTHER_AMOUNT));//@@@
                            //lblOtherAmount.ToolTip = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].FIN_INVOICE_CUS_TRX_MPG.Where(tm => tm.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0).Sum(tm => tm.ICM_OTHER_AMOUNT));//@@@
                            //txtOthercharges.Text = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].FIN_INVOICE_CUS_TRX_MPG.Where(tm => tm.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0).Sum(tm => tm.ICM_OTHER_AMOUNT));//@@@
                            //hdfOtherchargeOLD.Value = String.Format("{0:c}",finInvoiceCusHdrList[e.Row.RowIndex].
                            //txtOthercharges.Text = String.Format("{0:c}", Convert.ToDecimal(SalOrderHdrList[e.Row.RowIndex].SOH_TOTAL_SHIP_CHARGE) - Convert.ToDecimal(hdfOtherchargeOLD.Value));
                        }
                        else if (finReceiptCusTrxMpgList != null && finReceiptCusTrxMpgList.Count > 0)
                        {

                            lblCustomerTxt.Text = ERP.Utilities.CommonFunctions.GetShortString(finReceiptCusTrxMpgList[0].FIN_RECEIPT_CUS_HDR.CRM_CUSTOMER_MST.CUS_NAME, 35);
                            lblCustomerTxt.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(finReceiptCusTrxMpgList[0].FIN_RECEIPT_CUS_HDR.CRM_CUSTOMER_MST.CUS_NAME, 300);
                            hdfCusPK.Value = finReceiptCusTrxMpgList[0].FIN_RECEIPT_CUS_HDR.RCH_CUSTOMER.ToString();

                            decimal lineItemTax = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.FIN_INVOICE_CUS_DTL.Sum(o => o.CID_TAX);
                            decimal lineItemDiscount = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.FIN_INVOICE_CUS_DTL.Sum(ss => ss.CID_DISCOUNT);


                            hdfCategory.Value = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_CATEGORY.ToString();
                            hdfType.Value = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_TYPE.ToString();
                            hdfGroup.Value = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_GROUP.ToString();
                            hdfTotalAmt.Value = (finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_TC - (finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DISCOUNT_TC + lineItemDiscount)).ToString();
                            if (finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR != null)
                            {
                                hdfTaxAmt.Value = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_TAX_TC.ToString() == string.Empty ? (0 + lineItemTax).ToString() : (finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_TAX_TC + lineItemTax).ToString();
                            }
                            else
                            {
                                hdfTaxAmt.Value = "0";
                            }
                            hdfInvoicePK.Value = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_PK.ToString();
                            hdfReceiptMpgPK.Value = finReceiptCusTrxMpgList[e.Row.RowIndex].RCM_PK.ToString();
                            sodate = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DATE_PAY_BY.ToString(Resources.Constants.DateFormatShort);
                            lnkInvoiceNo.Text = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_NO;
                            lnkInvoiceNo.ToolTip = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_NO + "  " + sodate;
                            lnkInvoiceNo.CommandArgument = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_PK.ToString();
                            hdfInvTypeText.Value = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_TYPE.ToString();
                            //lblInvoiceNo.Text = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_NO;
                            //lblInvoiceNo.ToolTip = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_NO + "  " + sodate;
                            //lblInvoiceDate.Text = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DATE.ToString(Resources.Constants.DateFormatShort);
                            //lblInvoiceDate.ToolTip = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblInvoiceDate.Text = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DATE_PAY_BY.ToString(Resources.Constants.DateFormatShort);
                            lblInvoiceDate.ToolTip = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DATE_PAY_BY.ToString(Resources.Constants.DateFormatShort);

                            lblCustomerInv.Text = ERP.Utilities.CommonFunctions.GetShortString(finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.CRM_CUSTOMER_MST.CUS_NAME, 18);
                            lblCustomerInv.ToolTip = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.CRM_CUSTOMER_MST.CUS_NAME;
                            lblInvCurrency.Text = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ADM_CURRENCY_MST1.CUR_CODE;
                            lblInvCurrency.ToolTip = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ADM_CURRENCY_MST1.CUR_CODE;

                            lblCmpDisplayCode.Text = lblCmpDisplayCode.ToolTip = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ADM_COMPANY_MST.CMP_DISPLAY_CODE;
                            lblCmpDisplayCode.CssClass = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ADM_COMPANY_MST.CMP_LINE_COLOUR;

                            lblTotalAmount.Text = String.Format("{0:c}", finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC);
                            totalAmtFooter = totalAmtFooter + Convert.ToDecimal(lblTotalAmount.Text);
                            lblTotalAmount.ToolTip = String.Format("{0:c}", finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC);


                            lblTotalTaxAmount.Text = String.Format("{0:c}", Convert.ToDecimal(finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_TAX_TC.ToString() == string.Empty ? 0 + lineItemTax : finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_TAX_TC + lineItemTax));
                            totalTaxFooter = totalTaxFooter + Convert.ToDecimal(lblTotalTaxAmount.Text);
                            lblTotalTaxAmount.ToolTip = lblTotalTaxAmount.Text;
                            decimal adjAlcn = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_RECEIPT_CUS_ALCN_DTL.Sum(s => s.RAD_AMOUNT);
                            decimal adjAlcnRec = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_RECEIPT_CUS_ALCN_DTL.Where(a => a.RAD_ALCN_RECEIPT_TRX != null).Sum(s => s.RAD_AMOUNT);
                            //while allocating excess amount,the received and balance amount getting wrong
                            //decimal paid = (finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0 && finReceiptCusTrxMpgList[e.Row.RowIndex].RCM_BOUNCED == 0)
                            //    ? finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_RCVD_TC - (finReceiptCusTrxMpgList[e.Row.RowIndex].RCM_RCVD_AMOUNT + adjAlcnRec)
                            //     : finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_RCVD_TC;

                            decimal paid = (finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0 && finReceiptCusTrxMpgList[e.Row.RowIndex].RCM_BOUNCED == 0)
                                ? finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_RCVD_TC - (finReceiptCusTrxMpgList[e.Row.RowIndex].RCM_RCVD_AMOUNT)
                                 : finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_RCVD_TC;

                            //while allocating excess amount,the received and balance amount getting wrong
                            //lblReceived.Text = String.Format("{0:c}", paid);
                            //lblReceived.ToolTip = String.Format("{0:c}", paid);
                            if (finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0 && finReceiptCusTrxMpgList[e.Row.RowIndex].RCM_BOUNCED == 0)
                            {
                                //lnkReceived.Text = lnkReceived.ToolTip = String.Format("{0:c}", paid + finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_CN_TC - adjAlcn);
                                lblReceived.ToolTip = lblReceived.Text = lnkReceived.Text = lnkReceived.ToolTip = String.Format("{0:c}", paid + finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_CN_TC - adjAlcn);
                            }
                            else
                            {
                                lblReceived.ToolTip = lblReceived.Text = lnkReceived.Text = lnkReceived.ToolTip = String.Format("{0:c}", paid + finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_CN_TC);
                            }
                            totalReceivedFooter = totalReceivedFooter + Convert.ToDecimal(lnkReceived.Text.Replace(",", ""));//lblReceived.Text
                            //-------------------------------------------
                            //finReceiptCusTrxMpgList = null;

                            // GetFieldValues(ControlsEnum.INVOICEVNDMPGLIST);//GetFieldValues(ControlsEnum.RECEIPTHDRINVLISTBYPK);
                            decimal balToPay = 0;
                            decimal balToRecWithoutAdjn = 0;
                            //if ((finReceiptCusTrxMpgList != null && finReceiptCusTrxMpgList.Count > 1) || finReceiptCusTrxMpgList == null || finReceiptCusTrxMpgList.Count == 0)
                            //{
                            //    balToPay = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC -
                            //                 finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_RCVD_TC -
                            //                 finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_CN_TC +
                            //                 finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_DN_TC +
                            //                 (finReceiptCusTrxMpgList[e.Row.RowIndex].RCM_BOUNCED == 1 ? 0 : finReceiptCusTrxMpgList[e.Row.RowIndex].RCM_RCVD_AMOUNT);
                            //    balToRecWithoutAdjn = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC;
                            //}
                            //else if (finReceiptCusTrxMpgList != null && finReceiptCusTrxMpgList.Count == 1)
                            //{

                            decimal adj = (finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_RECEIPT_CUS_ALCN_DTL.Where(r => (r.FIN_CRDR_NOTE_HDR != null ? r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED == false : true) && (r.FIN_RECEIPT_CUS_TRX_MPG1 != null ? r.FIN_RECEIPT_CUS_TRX_MPG1.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED == 0 : true) && (r.FIN_RECEIPT_CUS_TRX_MPG1 != null ? r.FIN_RECEIPT_CUS_TRX_MPG1.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0 : true)).Sum(c => c.RAD_AMOUNT));

                            if (finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0)// Not Cancelled //Bug ID:  2394
                            {
                                //commented for adjustment allocation amount missing in received amount
                                //balToPay = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC -
                                //   finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_RCVD_TC + (finReceiptCusTrxMpgList[e.Row.RowIndex].RCM_RCVD_AMOUNT + (adjAlcnRec > 0 ? adjAlcnRec : 0)) - //
                                //               adj +
                                //    ////finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_CN_TC +
                                //               finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_DN_TC;


                                //while allocating excess amount,the received and balance amount getting wrong
                                //balToPay = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC -
                                //    (finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_RCVD_TC + finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_CN_TC) + (finReceiptCusTrxMpgList[e.Row.RowIndex].RCM_RCVD_AMOUNT + (adjAlcnRec > 0 ? adjAlcnRec : 0))  //-
                                //                //adj 
                                //                +
                                //    ////finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_CN_TC +
                                //                finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_DN_TC;

                                balToPay = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC -
                                   (finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_RCVD_TC + finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_CN_TC) +
                                   (finReceiptCusTrxMpgList[e.Row.RowIndex].RCM_BOUNCED == 1 ? 0 : finReceiptCusTrxMpgList[e.Row.RowIndex].RCM_RCVD_AMOUNT) +
                                    finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_DN_TC;
                            }
                            else
                            {
                                //commented for adjustment allocation amount missing in received amount
                                ////In case of cancelled record there is no nedd to add RCM_RCVD_AMOUNT.Hence it is removed from the formula
                                //balToPay = (finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC - finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_RCVD_TC)
                                //            + (adjAlcnRec > 0 ? adjAlcnRec : 0) - (adj + finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_DN_TC);




                                //In case of cancelled record there is no nedd to add RCM_RCVD_AMOUNT.Hence it is removed from the formula
                                //balToPay = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC - (finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_RCVD_TC-finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_CN_TC)
                                //            + (adjAlcnRec > 0 ? adjAlcnRec : 0) - (adj + finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_DN_TC);
                                balToPay = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC -
                                    (finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_RCVD_TC + finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_CN_TC)
                                           + (adjAlcnRec > 0 ? adjAlcnRec : 0) - (finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_DN_TC);
                            }
                            balToRecWithoutAdjn = balToPay + (finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_RECEIPT_CUS_ALCN_DTL.Where(r => (r.FIN_CRDR_NOTE_HDR != null ? r.FIN_CRDR_NOTE_HDR.CDH_IS_DELETED == false : true) && (r.FIN_RECEIPT_CUS_TRX_MPG1 != null ? r.FIN_RECEIPT_CUS_TRX_MPG1.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED == 0 : true) && (r.FIN_RECEIPT_CUS_TRX_MPG1 != null ? r.FIN_RECEIPT_CUS_TRX_MPG1.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0 : true)).Sum(c => c.RAD_AMOUNT));
                            //    //balToRecWithoutAdjn = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC;
                            //}

                            decimal otherCharges = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_SHIP_CHARGE;
                            decimal prevotherCharges = finReceiptCusTrxMpgList[e.Row.RowIndex].RCM_OTHER_AMOUNT;
                            lblOtherAmount.Text = String.Format("{0:c}", otherCharges);
                            lblOtherAmount.ToolTip = String.Format("{0:c}", otherCharges);
                            totalOtheramtFooter = totalOtheramtFooter + Convert.ToDecimal(lblOtherAmount.Text);
                            decimal balOtherCharges = otherCharges - prevotherCharges;
                            balOtherCharges = balOtherCharges < 0 ? 0 : balOtherCharges;
                            txtOthercharges.Text = String.Format("{0:c}", prevotherCharges);
                            txtOthercharges.ToolTip = String.Format("{0:c}", prevotherCharges);
                            hdfOtherchargeOLD.Value = balOtherCharges.ToString();


                            //-------------------------------------------

                            //decimal balToPay = (finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC - finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_RCVD_TC) + finReceiptCusTrxMpgList[e.Row.RowIndex].RCM_RCVD_AMOUNT;
                            lblBaltoReceive.Text = String.Format("{0:c}", balToPay);
                            hdfBaltoReceive.Value = balToRecWithoutAdjn.ToString();
                            lblBaltoReceive.ToolTip = String.Format("{0:c}", balToPay);
                            balRecieveFooter = balRecieveFooter + Convert.ToDecimal(lblBaltoReceive.Text);

                            if (dicTempAmount != null && dicTempAmount.Count > 0 && dicTempAmount.ContainsKey(hdfInvoicePK.Value))
                            {
                                hdfReceivedNow.Value = txtReceivedNow.ToolTip = txtReceivedNow.Text = dicTempAmount.SingleOrDefault(aa => aa.Key == hdfInvoicePK.Value).Value.ToString();
                            }
                            else
                            {
                                hdfReceivedNow.Value = txtReceivedNow.ToolTip = txtReceivedNow.Text = Math.Round(finReceiptCusTrxMpgList[e.Row.RowIndex].RCM_RCVD_AMOUNT,
                                    Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            }


                            lnkRemove.CommandArgument = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_PK.ToString();
                            //lnkAllocation.Visible = true;
                            lblAdjAmount.Text = lblAdjAmount.ToolTip = String.Format("{0:c}", adjAlcn);
                            adjAmtFooter = adjAmtFooter + Convert.ToDecimal(lblAdjAmount.Text);
                            //hdfReceivedNow.Value = txtReceivedNow.ToolTip = txtReceivedNow.Text = (Convert.ToDecimal(txtReceivedNow.Text) - Convert.ToDecimal(lblAdjAmount.Text)).ToString();
                            txtAdjustments.Text = Math.Round(finReceiptCusTrxMpgList[e.Row.RowIndex].RCM_DISC_AMOUNT,
                            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            txtAdjustments.ToolTip = txtAdjustments.Text;
                            if (Convert.ToInt32(hdfCategory.Value) == (int)SalesInvoiceCategory.Advanced)
                            {
                                lblTax.Text = String.Format("{0:c}", finReceiptCusTrxMpgList[e.Row.RowIndex].RCM_TAX_AMOUNT);
                            }
                            else
                            {
                                lblTax.Text = String.Format("{0:c}", taxamt);
                            }
                            lblTax.ToolTip = lblTax.Text;

                            //debit note allocated amount
                            decimal CrdrAlcnAmnt = 0;
                            if (finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_RECEIPT_CUS_CRDR_MPG != null && finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_RECEIPT_CUS_CRDR_MPG.Count > 0)
                            {
                                CrdrAlcnAmnt = finReceiptCusTrxMpgList[e.Row.RowIndex].FIN_RECEIPT_CUS_CRDR_MPG.Sum(r => r.RNM_PAID_AMOUNT + r.RNM_ADJ_AMOUNT);
                            }
                            lblCrdrAlcnAmount.Text = lblCrdrAlcnAmount.ToolTip = String.Format("{0:c}", CrdrAlcnAmnt);
                            hdfCrdrAlcnAmount.Value = CrdrAlcnAmnt.ToString();
                        }
                        //if (Convert.ToInt32(hdfCategory.Value) == (int)SalesInvoiceCategory.Advanced)
                        //{
                        //    e.Row.Cells[5].Visible = false;
                        //    e.Row.Cells[6].Visible = false;
                        //}
                        //else
                        //{
                        //    e.Row.Cells[5].Visible = true;
                        //    e.Row.Cells[6].Visible = true;
                        //}

                        //Visibility of adjn Coloumns
                        hdfCategoryDtl.Value = hdfCategory.Value;
                        hdfTypeDtl.Value = hdfType.Value;
                        if (Convert.ToInt32(hdfCategory.Value) == (int)SalesInvoiceCategory.Advanced)
                        {
                            if (Convert.ToInt32(hdfType.Value) == (int)SalesInvoiceType.Domestic)
                            {
                                grdInvoiceList.Columns[9].Visible = false;//{Allocation}
                                grdInvoiceList.Columns[10].Visible = false; //lnkAllocationAdjn
                                grdInvoiceList.Columns[17].Visible = true;//{Oth. Charges}
                                grdInvoiceList.Columns[18].Visible = true;//{Tax}
                                divTax.Visible = true;
                            }
                            else
                            {
                                grdInvoiceList.Columns[9].Visible = false;
                                grdInvoiceList.Columns[10].Visible = false;
                                grdInvoiceList.Columns[17].Visible = false;
                                grdInvoiceList.Columns[18].Visible = false;
                                divTax.Visible = false;
                            }
                            grdInvoiceList.Columns[15].Visible = false;
                            grdInvoiceList.Columns[16].Visible = false;
                        }
                        else
                        {
                            if (!ShowAdjColumn)//Convert.ToInt32(hdfGroup.Value) == (int)SalesInvoiceGroup.Miscellaneous &&
                            {
                                grdInvoiceList.Columns[9].Visible = false;
                                grdInvoiceList.Columns[10].Visible = false;
                            }
                            else
                            {
                                grdInvoiceList.Columns[9].Visible = true;
                                grdInvoiceList.Columns[10].Visible = true;
                            }
                            grdInvoiceList.Columns[17].Visible = false;
                            grdInvoiceList.Columns[15].Visible = true;
                            grdInvoiceList.Columns[16].Visible = true;
                            grdInvoiceList.Columns[18].Visible = false;
                            divTax.Visible = false;
                            if (ShowTaxForMiscInv && Convert.ToInt32(hdfGroup.Value) == (int)SalesInvoiceGroup.Miscellaneous)
                            {
                                grdInvoiceList.Columns[18].Visible = true;
                                divTax.Visible = true;
                            }
                        }

                        if (hdfMultiCurrencyInReceipt.Value == "1")
                        {
                            txtReceivedNow.Enabled = false;
                            txtReceivedNow.CssClass = "input-disabled";
                            //lnkRecCurAllocation.Visible = true;
                            // grdInvoiceList.Columns[14].Visible = true;
                        }
                        else
                        {
                            txtReceivedNow.Enabled = true;
                            txtReceivedNow.CssClass = "input-w70 numeric";
                            //lnkRecCurAllocation.Visible = false;
                            // grdInvoiceList.Columns[14].Visible = false;
                        }
                    }

                    else if (((GridView)sender).ID == "grdPOReceiptHdr")
                    {
                        if (finReceiptCusHdrList != null && finReceiptCusHdrList.Count > 0)
                        {
                            Label lblModeofReceipt = e.Row.FindControl("lblModeofReceipt") as Label;
                            int cfgpk = Convert.ToInt32(lblModeofReceipt.Text);
                            HiddenField hdfRcptStatus = e.Row.FindControl("hdfRcptStatus") as HiddenField;
                            recipetType = Convert.ToInt32(hdfRcptStatus.Value);
                            GetFieldValues(ControlsEnum.PAYMODE);
                            if (admConfigMstList != null && admConfigMstList.Count > 0)
                            {
                                lblModeofReceipt.Text = cfgpk == 0 ? "" : admConfigMstList.SingleOrDefault(con => con.CFG_VALUE == cfgpk).CFG_DATA;
                                lblModeofReceipt.ToolTip = cfgpk == 0 ? "" : admConfigMstList.SingleOrDefault(con => con.CFG_VALUE == cfgpk).CFG_DATA;
                            }


                            Button btnPDCFlag = e.Row.FindControl("btnPDCFlag") as Button;
                            if (finReceiptCusHdrList[e.Row.RowIndex].RCH_PDC != 0)
                            {
                                btnPDCFlag.Visible = true;
                                if (finReceiptCusHdrList[e.Row.RowIndex].RCH_PDC == 1)
                                {
                                    btnPDCFlag.CssClass = "flaggrey-icon";
                                    btnPDCFlag.ToolTip = GetLocalResourceObject("PDC_Cheque").ToString();
                                }
                                else
                                {
                                    if (pdcVoucherList != null && pdcVoucherList.Count > 0)
                                    {
                                        if (pdcVoucherList.SingleOrDefault(aa => aa.FTH_REF_PK == finReceiptCusHdrList[e.Row.RowIndex].RCH_PK) != null)
                                        {
                                            btnPDCFlag.CssClass = "flaggrey-icon";
                                            btnPDCFlag.ToolTip = GetLocalResourceObject("PDC_Cheque").ToString();
                                        }
                                        else
                                        {
                                            btnPDCFlag.CssClass = "flaggreen-icon";
                                            btnPDCFlag.ToolTip = GetLocalResourceObject("Cheque_Reversed").ToString();
                                        }
                                    }
                                    else
                                    {
                                        btnPDCFlag.CssClass = "flaggreen-icon";
                                        btnPDCFlag.ToolTip = GetLocalResourceObject("Cheque_Reversed").ToString();
                                    }
                                }

                            }
                            else
                            {
                                btnPDCFlag.CssClass = "";
                                btnPDCFlag.ToolTip = "";
                                btnPDCFlag.Visible = false;
                            }

                            #region Invoice Number
                            FIN_RECEIPT_CUS_HDR ReceiptHdr = finReceiptCusHdrList.SingleOrDefault(x => x.RCH_PK == Convert.ToInt32(((HiddenField)e.Row.FindControl("hdfPaymentID")).Value));
                            LinkButton lnkInvnos = e.Row.FindControl("lnkInvnos") as LinkButton;
                            HiddenField hdfListinvPK = e.Row.FindControl("hdfListinvPK") as HiddenField;
                            HiddenField hdfListInvType = e.Row.FindControl("hdfListInvType") as HiddenField;
                            HiddenField hdfListinvCategory = e.Row.FindControl("hdfListinvCategory") as HiddenField;
                            HiddenField hdfListGroup = e.Row.FindControl("hdfListGroup") as HiddenField;
                            if (ReceiptHdr != null)
                            {
                                string invoices = string.Empty;
                                int invCount = 0;
                                var invNo = from c in ReceiptHdr.FIN_RECEIPT_CUS_TRX_MPG select c.FIN_INVOICE_CUS_HDR.ICH_NO;
                                int invPk = Convert.ToInt32(ReceiptHdr.FIN_RECEIPT_CUS_TRX_MPG.ToList()[0].FIN_INVOICE_CUS_HDR.ICH_PK);
                                int invType = ReceiptHdr.FIN_RECEIPT_CUS_TRX_MPG.ToList()[0].FIN_INVOICE_CUS_HDR.ICH_TYPE;
                                int invCategory = ReceiptHdr.FIN_RECEIPT_CUS_TRX_MPG.ToList()[0].FIN_INVOICE_CUS_HDR.ICH_CATEGORY;
                                int invGroup = ReceiptHdr.FIN_RECEIPT_CUS_TRX_MPG.ToList()[0].FIN_INVOICE_CUS_HDR.ICH_GROUP;

                                if (invNo != null && invNo.Count() > 0)
                                {
                                    foreach (string invNumber in invNo)
                                    {
                                        invoices += invNumber + ",";
                                        invCount++;
                                    }

                                    invoices = invoices.TrimEnd(',');
                                    lnkInvnos.Text = CommonFunctions.GetShortString(invoices, 15);
                                    lnkInvnos.ToolTip = invoices;
                                    hdfListinvPK.Value = invPk.ToString();
                                    hdfListInvType.Value = invType.ToString();
                                    hdfListinvCategory.Value = invCategory.ToString();
                                    hdfListGroup.Value = invGroup.ToString();
                                    if (invCount > 1)
                                    {
                                        lnkInvnos.Attributes.Add("onClick", "return false");
                                        lnkInvnos.CssClass = "removelinkPopup";
                                    }
                                    else
                                    {
                                        lnkInvnos.Attributes.Add("onclick", "return true;");
                                    }
                                }
                            }
                            #endregion
                        }
                        //return or not
                        //btnReturnFlag 
                        //if (finReceiptCusHdrList[e.Row.RowIndex].RCH_PDC != 0)
                        if (finReceiptCusHdrList[e.Row.RowIndex].RCH_MODE == (int)PaymentModeEnum.Cheque)
                        {
                            if (finReceiptCusHdrList[e.Row.RowIndex].RCH_BOUNCED != 0)
                            {
                                Button btnReturnFlag = e.Row.FindControl("btnReturnFlag") as Button;
                                if (finReceiptCusHdrList[e.Row.RowIndex].RCH_BOUNCED == 1)
                                {
                                    btnReturnFlag.Visible = true;
                                    btnReturnFlag.CssClass = "return-icon";
                                    btnReturnFlag.ToolTip = GetLocalResourceObject("ChequeReturned").ToString();
                                }
                                else
                                {
                                    btnReturnFlag.Visible = false;

                                }
                            }
                        }
                        #region Set an Indication flag for Return Receipt
                        Button btnReceiptReturnFlag = e.Row.FindControl("btnReceiptReturnFlag") as Button;
                        if (finReceiptCusHdrList[e.Row.RowIndex].RCH_RETURN_STATUS == 1)
                        {
                            btnReceiptReturnFlag.Visible = true;
                            btnReceiptReturnFlag.CssClass = "receiptreturn-icon";
                            btnReceiptReturnFlag.ToolTip = GetLocalResourceObject("ReceiptReturned").ToString();
                        }
                        else
                        {
                            btnReceiptReturnFlag.Visible = false;
                        }
                        #endregion
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
                        CurrPK = Convert.ToInt32(((HiddenField)e.Row.FindControl("hdfPaymentID")).Value);
                        GetFieldValues(ControlsEnum.FINHEADERSTATUS);
                        short status = 0;
                        if (finTrxHdrList.Count > 0)
                        {
                            status = finTrxHdrList[0].FTH_STATUS;
                        }
                        if (workflowStatusList != null && workflowStatusList.Count > 0)
                        {
                            wkfStatus = workflowStatusList.SingleOrDefault(aa => aa.ASC_VALUE == status);
                            if (wkfStatus != null)
                            {
                                if (status != 0)
                                {
                                    imgPosted.ToolTip = wkfStatus.ASC_NAME;
                                    imgPosted.CssClass = wkfStatus.ASC_CSS_CLASS;
                                }
                                else
                                {
                                    imgPosted.CssClass = GetLocalResourceObject("unposted").ToString();
                                    imgPosted.ToolTip = Resources.Captions.NotPosted;
                                }
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
                    else if (((GridView)sender).ID == "grdReceiptSplitAdjn")
                    {
                        Label lblCrDrNOSplit;
                        Label lblTotAmountAdjn;
                        Label lblAllocatedAdjn;
                        Label lblBalanceAdjn;
                        Label lblPageType;
                        Label lblDate;

                        TextBox txtAllocateAdjn;

                        HiddenField hdfReceiptTRXAdjnPK;
                        HiddenField hdfCrDrPK;
                        HiddenField hdfReceiptAdjnPK;
                        HiddenField hdfAdjnPK;


                        //grdReceiptSplitAdjn
                        lblCrDrNOSplit = e.Row.FindControl("lblCrDrNOAdjn") as Label;
                        lblTotAmountAdjn = e.Row.FindControl("lblTotAmountAdjn") as Label;
                        lblAllocatedAdjn = e.Row.FindControl("lblAllocatedAdjn") as Label;
                        lblBalanceAdjn = e.Row.FindControl("lblBalanceAdjn") as Label;
                        lblPageType = e.Row.FindControl("lblPageType") as Label;
                        lblDate = e.Row.FindControl("lblDate") as Label;

                        txtAllocateAdjn = e.Row.FindControl("txtAllocateAdjn") as TextBox;

                        hdfReceiptTRXAdjnPK = e.Row.FindControl("hdfReceiptTRXAdjnPK") as HiddenField;
                        hdfCrDrPK = e.Row.FindControl("hdfCrDrPK") as HiddenField;
                        hdfReceiptAdjnPK = e.Row.FindControl("hdfReceiptAdjnPK") as HiddenField;
                        hdfAdjnPK = e.Row.FindControl("hdfAdjnPK") as HiddenField;

                        List<long?> lstInvNos = new List<long?>();
                        ReceiptAdjnList.ForEach(rr =>
                        {
                            lstInvNos.Add(rr.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR);
                        });

                        if (CrDrAdjnList != null && CrDrAdjnList.Count > 0)
                        {
                            lblCrDrNOSplit.Text = lblCrDrNOSplit.ToolTip = CrDrAdjnList[e.Row.RowIndex].RAA_NO;
                            lblPageType.Text = lblPageType.ToolTip = CrDrAdjnList[e.Row.RowIndex].RAA_TYPE;
                            lblDate.Text = lblDate.ToolTip = CrDrAdjnList[e.Row.RowIndex].RAA_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblTotAmountAdjn.Text = lblTotAmountAdjn.ToolTip = String.Format("{0:c}", Convert.ToDecimal(CrDrAdjnList[e.Row.RowIndex].RAA_AMOUNT));
                            lblAllocatedAdjn.Text = lblAllocatedAdjn.ToolTip = String.Format("{0:c}", Convert.ToDecimal(CrDrAdjnList[e.Row.RowIndex].RAA_AMOUNT_RCVD));


                            hdfReceiptTRXAdjnPK.Value = ReceiptMpgPK.ToString();
                            hdfCrDrPK.Value = CrDrAdjnList[e.Row.RowIndex].RAA_CRDRPK.ToString();
                            hdfReceiptAdjnPK.Value = CrDrAdjnList[e.Row.RowIndex].RAA_TRXPK.ToString();

                            if (ReceiptAdjnList != null && ReceiptAdjnList.Count > 0)//Edit before save
                            {
                                if (CrDrAdjnList[e.Row.RowIndex].RAA_CRDRPK > 0)
                                {
                                    lblBalanceAdjn.Text = lblBalanceAdjn.ToolTip = String.Format("{0:c}", Convert.ToDecimal(CrDrAdjnList[e.Row.RowIndex].RAA_AMOUNT_BAL) > 0 ? (ReceiptAdjnList.Where(fd => fd.RAD_ALCN_CDH == CrDrAdjnList[e.Row.RowIndex].RAA_CRDRPK && !lstInvNos.Contains(fd.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR)).ToList().Count > 0 ? Convert.ToDecimal(lblTotAmountAdjn.Text.Replace(",", "")) : Convert.ToDecimal(CrDrAdjnList[e.Row.RowIndex].RAA_AMOUNT_BAL)) : Convert.ToDecimal(CrDrAdjnList[e.Row.RowIndex].RAA_AMOUNT_BAL));
                                    if (ReceiptAdjnList.Where(fd => fd.RAD_ALCN_CDH == CrDrAdjnList[e.Row.RowIndex].RAA_CRDRPK && fd.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK).ToList() != null && ReceiptAdjnList.Where(fd => fd.RAD_ALCN_CDH == CrDrAdjnList[e.Row.RowIndex].RAA_CRDRPK && fd.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK).ToList().Count > 0)
                                    {
                                        txtAllocateAdjn.ToolTip = txtAllocateAdjn.Text = ReceiptAdjnList.Where(fd => fd.RAD_ALCN_CDH == CrDrAdjnList[e.Row.RowIndex].RAA_CRDRPK && fd.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK).FirstOrDefault().RAD_AMOUNT.ToString(hdfCurrencyFormat.Value);
                                    }
                                    else
                                    {
                                        txtAllocateAdjn.ToolTip = txtAllocateAdjn.Text = "0.00";
                                    }
                                }
                                else
                                {
                                    lblBalanceAdjn.Text = lblBalanceAdjn.ToolTip = String.Format("{0:c}", Convert.ToDecimal(CrDrAdjnList[e.Row.RowIndex].RAA_AMOUNT_BAL) > 0 ? (ReceiptAdjnList.Where(fd => fd.RAD_ALCN_RECEIPT_TRX == CrDrAdjnList[e.Row.RowIndex].RAA_TRXPK && !lstInvNos.Contains(fd.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR)).ToList().Count > 0 ? Convert.ToDecimal(lblTotAmountAdjn.Text.Replace(",", "")) : Convert.ToDecimal(CrDrAdjnList[e.Row.RowIndex].RAA_AMOUNT_BAL)) : Convert.ToDecimal(CrDrAdjnList[e.Row.RowIndex].RAA_AMOUNT_BAL));
                                    if (ReceiptAdjnList.Where(fd => fd.RAD_ALCN_RECEIPT_TRX == CrDrAdjnList[e.Row.RowIndex].RAA_TRXPK && fd.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK).ToList() != null && ReceiptAdjnList.Where(fd => fd.RAD_ALCN_RECEIPT_TRX == CrDrAdjnList[e.Row.RowIndex].RAA_TRXPK && fd.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK).ToList().Count > 0)
                                    {
                                        txtAllocateAdjn.ToolTip = txtAllocateAdjn.Text = ReceiptAdjnList.Where(fd => fd.RAD_ALCN_RECEIPT_TRX == CrDrAdjnList[e.Row.RowIndex].RAA_TRXPK && fd.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK).FirstOrDefault().RAD_AMOUNT.ToString(hdfCurrencyFormat.Value);
                                    }
                                    else
                                    {
                                        txtAllocateAdjn.ToolTip = txtAllocateAdjn.Text = "0.00";
                                    }
                                }
                            }
                            else
                            {
                                lblBalanceAdjn.Text = lblBalanceAdjn.ToolTip = String.Format("{0:c}", Convert.ToDecimal(CrDrAdjnList[e.Row.RowIndex].RAA_AMOUNT_BAL));
                                txtAllocateAdjn.ToolTip = txtAllocateAdjn.Text = "0.00";// String.Format("{0:c}", Convert.ToDecimal(CrDrAdjnList[e.Row.RowIndex].RAA_AMOUNT_BAL.ToString()));
                            }

                            totAllocateAdjn = totAllocateAdjn + Convert.ToDecimal(txtAllocateAdjn.Text);
                            totBalanceAdjn = totBalanceAdjn + Convert.ToDecimal(lblBalanceAdjn.Text.Replace(",", ""));
                        }
                        else if (FinReceiptCusAllocationList != null && FinReceiptCusAllocationList.Count > 0)
                        {
                            bool isCr = true;
                            decimal alocNowCR = 0;
                            decimal allocated = 0;

                            long RcptPK = FinReceiptCusAllocationList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_PK;
                            long InvPk = Convert.ToInt64(FinReceiptCusAllocationList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR);

                            if (FinReceiptCusAllocationList[e.Row.RowIndex].RAD_ALCN_CDH == null || FinReceiptCusAllocationList[e.Row.RowIndex].RAD_ALCN_CDH == 0)
                            {
                                isCr = false;
                                lblCrDrNOSplit.Text = FinReceiptCusAllocationList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG1.FIN_RECEIPT_CUS_HDR.RCH_NO;
                                lblPageType.Text = "REC";
                                lblDate.Text = FinReceiptCusAllocationList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG1.FIN_RECEIPT_CUS_HDR.RCH_DATE.ToString(Resources.Constants.DateFormatShort);
                                lblTotAmountAdjn.Text = String.Format("{0:c}", Convert.ToDecimal(FinReceiptCusAllocationList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG1.RCM_EXCESS_AMOUNT));

                                //lblAllocatedAdjn.Text = String.Format("{0:c}", FinReceiptCusAllocationList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG1.FIN_RECEIPT_CUS_ALCN_DTL1
                                //    .Where(rad => rad.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0 && rad.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED == 0
                                //        && rad.RAD_PK != FinReceiptCusAllocationList[e.Row.RowIndex].RAD_PK).Where(d => !lstInvNos.Contains(d.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR)).Sum(ra => ra.RAD_AMOUNT));


                                //No records showing while editing receipt with excess allocations
                                //decimal TotalAllocated = FinReceiptCusAllocationList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG1.RCM_EXCESS_AMOUNT;
                                //decimal CurrentInvAlcn = FinReceiptCusAllocationList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG1.FIN_RECEIPT_CUS_ALCN_DTL1
                                //        .Where(rad => rad.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0 
                                //        && rad.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED == 0
                                //        && rad.RAD_PK != FinReceiptCusAllocationList[e.Row.RowIndex].RAD_PK)
                                //        .Where(d => d.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvPk && d.FIN_RECEIPT_CUS_TRX_MPG.RCM_RECEIPT_HDR == RcptPK).Sum(ra => ra.RAD_AMOUNT);
                                decimal TotalAllocated = FinReceiptCusAllocationList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG1.FIN_RECEIPT_CUS_ALCN_DTL1
                                                        .Where(rad => rad.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                                                    && rad.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED == 0
                                                                    && rad.RAD_PK != FinReceiptCusAllocationList[e.Row.RowIndex].RAD_PK
                                                              ).Sum(ra => ra.RAD_AMOUNT);

                                decimal CurrentInvAlcn = FinReceiptCusAllocationList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG1.FIN_RECEIPT_CUS_ALCN_DTL1
                                                        .Where(rad => rad.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                                                    && rad.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED == 0
                                                                    && rad.RAD_PK != FinReceiptCusAllocationList[e.Row.RowIndex].RAD_PK
                                                         )
                                                        .Where(d => d.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvPk
                                                                 && d.FIN_RECEIPT_CUS_TRX_MPG.RCM_RECEIPT_HDR == RcptPK
                                                               ).Sum(ra => ra.RAD_AMOUNT);

                                lblAllocatedAdjn.Text = String.Format("{0:c}", (TotalAllocated - CurrentInvAlcn < 0 ? 0 : TotalAllocated - CurrentInvAlcn));

                                hdfReceiptAdjnPK.Value = FinReceiptCusAllocationList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG1.RCM_PK.ToString();
                                allocated = Convert.ToDecimal(lblTotAmountAdjn.Text.Replace(",", "")) - Convert.ToDecimal(lblAllocatedAdjn.Text.Replace(",", ""));
                            }
                            else
                            {
                                isCr = true;
                                lblCrDrNOSplit.Text = FinReceiptCusAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.CDH_NO;//.Where(s=>s.FIN_CRDR_NOTE_HDR.CDH_PK==FinReceiptCusAllocationList[e.Row.RowIndex].RAD_ALCN_CDH).ToList()[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.CDH_NO;
                                lblPageType.Text = "CN";
                                lblDate.Text = FinReceiptCusAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.CDH_DATE.ToString(Resources.Constants.DateFormatShort);
                                lblTotAmountAdjn.Text = String.Format("{0:c}", Convert.ToDecimal(FinReceiptCusAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.CDH_AMOUNT_TC + FinReceiptCusAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.CDH_OTHER_CHARGE + FinReceiptCusAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.CDH_SHIP_CHARGE));
                                alocNowCR = ReceiptAdjnList.Where(fd => fd.RAD_ALCN_CDH == FinReceiptCusAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.CDH_PK && fd.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK).FirstOrDefault().RAD_AMOUNT;

                                //lblAllocatedAdjn.Text = String.Format("{0:c}", FinReceiptCusAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.FIN_RECEIPT_CUS_ALCN_DTL
                                //    .Where(v => v.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0 && v.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED == 0
                                //        && v.RAD_PK != FinReceiptCusAllocationList[e.Row.RowIndex].RAD_PK).ToList().Where(d => !lstInvNos.Contains(d.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR)).Sum(ra => ra.RAD_AMOUNT));

                                decimal TotalAllocated = FinReceiptCusAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.FIN_RECEIPT_CUS_ALCN_DTL
                                        .Where(v => v.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                        && v.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED == 0
                                        && v.RAD_PK != FinReceiptCusAllocationList[e.Row.RowIndex].RAD_PK).Sum(ra => ra.RAD_AMOUNT);
                                decimal CurrentInvAlcn = FinReceiptCusAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.FIN_RECEIPT_CUS_ALCN_DTL
                                        .Where(v => v.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0
                                        && v.FIN_RECEIPT_CUS_TRX_MPG.FIN_RECEIPT_CUS_HDR.RCH_BOUNCED == 0
                                        && v.RAD_PK != FinReceiptCusAllocationList[e.Row.RowIndex].RAD_PK).ToList()
                                        .Where(d => d.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvPk && d.FIN_RECEIPT_CUS_TRX_MPG.RCM_RECEIPT_HDR == RcptPK).Sum(ra => ra.RAD_AMOUNT);
                                lblAllocatedAdjn.Text = String.Format("{0:c}", (TotalAllocated - CurrentInvAlcn < 0 ? 0 : TotalAllocated - CurrentInvAlcn));

                                hdfCrDrPK.Value = FinReceiptCusAllocationList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.CDH_PK.ToString();
                                allocated = Convert.ToDecimal(lblTotAmountAdjn.Text.Replace(",", "")) - Convert.ToDecimal(lblAllocatedAdjn.Text.Replace(",", ""));
                            }

                            //if (AdjnNowAmount > 0) { txtAllocateAdjn.Text = String.Format("{0:c}", AdjnNowAmount); } else { txtAllocateAdjn.Text = String.Format("{0:c}", Convert.ToDecimal(FinReceiptCusAllocationList[e.Row.RowIndex].RAD_AMOUNT) ); }
                            if (ReceiptAdjnList != null && ReceiptAdjnList.Count > 0)//Edit before save
                            {
                                if (isCr)
                                {
                                    txtAllocateAdjn.ToolTip = txtAllocateAdjn.Text = alocNowCR.ToString(hdfCurrencyFormat.Value);
                                }
                                else
                                {
                                    txtAllocateAdjn.ToolTip = txtAllocateAdjn.Text = ReceiptAdjnList.Where(fd => fd.RAD_ALCN_RECEIPT_TRX == FinReceiptCusAllocationList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG1.RCM_PK && fd.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK).FirstOrDefault().RAD_AMOUNT.ToString(hdfCurrencyFormat.Value);
                                }
                            }
                            else
                            {
                                txtAllocateAdjn.ToolTip = txtAllocateAdjn.Text = FinReceiptCusAllocationList[e.Row.RowIndex].RAD_AMOUNT.ToString(hdfCurrencyFormat.Value);
                            }

                            lblAllocatedAdjn.Text = String.Format("{0:c}", (Convert.ToDecimal(lblAllocatedAdjn.Text)));// - Convert.ToDecimal(txtAllocateAdjn.Text)
                            lblBalanceAdjn.Text = String.Format("{0:c}", allocated);// (allocated).ToString(hdfCurrencyFormat.Value);//+ Convert.ToDecimal(txtAllocateAdjn.Text)

                            totAllocateAdjn = totAllocateAdjn + Convert.ToDecimal(txtAllocateAdjn.Text);
                            totBalanceAdjn = totBalanceAdjn + Convert.ToDecimal(lblBalanceAdjn.Text.Replace(",", ""));
                            hdfAdjnPK.Value = FinReceiptCusAllocationList[e.Row.RowIndex].RAD_PK.ToString();

                        }
                        if (Convert.ToDecimal(lblBalanceAdjn.Text.Replace(",", "")) <= 0)
                        {
                            e.Row.Visible = false;
                        }
                    }
                    else if (((GridView)sender).ID == "grdReceiptSplit")
                    {


                        //Test
                        hdfSOPK = e.Row.FindControl("hdfSOPK") as HiddenField;
                        hdfReceiptSplitPK = e.Row.FindControl("hdfReceiptSplitPK") as HiddenField;
                        hdfReceiptTRXPK = e.Row.FindControl("hdfReceiptTRXPK") as HiddenField;
                        lblPONOSplit = e.Row.FindControl("lblPONOSplit") as Label;
                        lblPODateSplit = e.Row.FindControl("lblPODateSplit") as Label;
                        lblCurrSplit = e.Row.FindControl("lblCurrSplit") as Label;
                        lblAmountSplit = e.Row.FindControl("lblAmountSplit") as Label;
                        lblPaidSplit = e.Row.FindControl("lblPaidSplit") as Label;
                        lblBalanceSplit = e.Row.FindControl("lblBalanceSplit") as Label;
                        txtPayNowSplit = e.Row.FindControl("txtPayNowSplit") as TextBox;
                        txtOtherChargesSplit = e.Row.FindControl("txtOtherChargesSplit") as TextBox;
                        hdfPayNowSplit = e.Row.FindControl("hdfPayNowSplit") as HiddenField;
                        txtReceivedInBaseCurrency = e.Row.FindControl("txtReceivedInBaseCurrency") as TextBox;

                        txtEquivalentInvoiceAmount = e.Row.FindControl("txtEquivalentInvoiceAmount") as TextBox;
                        txtGainOrLoss = e.Row.FindControl("txtGainOrLoss") as TextBox;

                        lblTaxSplit = e.Row.FindControl("lblTaxSplit") as Label;
                        lblDiscountSplit = e.Row.FindControl("lblDiscountSplit") as Label;

                        HiddenField hdfTaxSplit = (HiddenField)e.Row.FindControl("hdfTaxSplit");
                        Label lblTotalTaxSplit = (Label)e.Row.FindControl("lblTotalTaxSplit");

                        if (FinInvoiceCusTrxMpgList != null && FinInvoiceCusTrxMpgList.Count > 0)
                        {
                            hdfSOPK.Value = FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_PK.ToString();
                            hdfReceiptSplitPK.Value = "0";
                            lblPONOSplit.Text = FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NO;
                            lblPONOSplit.ToolTip = FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NO + "  " + FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblPODateSplit.Text = FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblPODateSplit.ToolTip = FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblCurrSplit.Text = FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.ADM_CURRENCY_MST.CUR_CODE;
                            lblCurrSplit.ToolTip = FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.ADM_CURRENCY_MST.CUR_CODE;
                            lblAmountSplit.Text = String.Format("{0:c}", FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NET_AMOUNT);
                            lblAmountSplit.ToolTip = String.Format("{0:c}", FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NET_AMOUNT);
                            //lblPaidSplit.Text = String.Format("{0:c}", FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_RECEIVED);
                            //lblPaidSplit.ToolTip = String.Format("{0:c}", FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_RECEIVED);

                            decimal paid = Convert.ToDecimal(FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_RECEIVED == null ? 0 : FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_RECEIVED);
                            decimal tax = FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_TOTAL_TAX + FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SAL_ORDER_DTL.Sum(o => o.SOD_TAX);
                            decimal disc = FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_TOTAL_DISCOUNT + FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SAL_ORDER_DTL.Sum(o => o.SOD_DISCOUNT);
                            //- FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_RECEIVED;
                            lblPaidSplit.Text = String.Format("{0:c}", paid);
                            lblPaidSplit.ToolTip = String.Format("{0:c}", paid);
                            lblTaxSplit.Text = String.Format("{0:c}", tax);
                            lblTaxSplit.ToolTip = String.Format("{0:c}", tax);
                            lblDiscountSplit.Text = String.Format("{0:c}", disc);
                            lblDiscountSplit.ToolTip = String.Format("{0:c}", disc);


                            decimal balToPay = Convert.ToDecimal(FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NET_AMOUNT == null ? 0 : FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NET_AMOUNT)
                                - paid;
                            lblBalanceSplit.Text = String.Format("{0:c}", balToPay < 0 ? 0 : balToPay);
                            lblBalanceSplit.ToolTip = String.Format("{0:c}", balToPay < 0 ? 0 : balToPay);
                            decimal OtherCharges = 0;
                            if (FinInvoiceCusTrxMpgList.Count == 1)
                            {
                                balToPay = string.IsNullOrEmpty(lblInvSplitReceiveNow.ToolTip) ? Convert.ToDecimal(0) : Convert.ToDecimal(lblInvSplitReceiveNow.ToolTip.Trim().Replace(",", ""));
                                OtherCharges = string.IsNullOrEmpty(hdfTotalOtherCharges.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(hdfTotalOtherCharges.Value);
                            }
                            else
                            {
                                balToPay = 0;
                            }

                            if (tempFinReceiptCusSoMpgList != null)
                            {
                                //tempFinReceiptCusSoMpgObj = tempFinReceiptCusSoMpgList.SingleOrDefault(mpg => mpg.RSO_SO_HDR == Convert.ToInt64(hdfSOPK.Value)); //same sc for multiple invoice
                                tempFinReceiptCusSoMpgObj = tempFinReceiptCusSoMpgList.SingleOrDefault(mpg => mpg.RSO_SO_HDR == Convert.ToInt64(hdfSOPK.Value) && mpg.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK);

                            }

                            txtOtherChargesSplit.Text = String.Format("{0:c}", OtherCharges);
                            txtOtherChargesSplit.ToolTip = String.Format("{0:c}", OtherCharges);


                            hdfShippingrChargesSplitPercent = e.Row.FindControl("hdfShippingrChargesSplitPercent") as HiddenField;
                            decimal NetAmountSplit = FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NET_AMOUNT;
                            decimal OtherChargeSplit = FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_TOTAL_SHIP_CHARGE;
                            hdfShippingrChargesSplitPercent.Value = (OtherChargeSplit / NetAmountSplit).ToString();

                            category = FinInvoiceCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_CATEGORY;
                            type = FinInvoiceCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_TYPE;

                            //commented for auto allocation of amount and tax
                            //txtPayNowSplit.Text = txtPayNowSplit.ToolTip = hdfPayNowSplit.Value =
                            //    tempFinReceiptCusSoMpgObj == null ?
                            //    Math.Round(balToPay < 0 ? 0 : balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString()
                            //    : Math.Round(tempFinReceiptCusSoMpgObj.RSO_RECEIVED_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();


                            //txtPayNowSplit.Text = Math.Round(balToPay < 0 ? 0 : balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //txtPayNowSplit.ToolTip = Math.Round(balToPay < 0 ? 0 : balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //hdfPayNowSplit.Value = Math.Round(balToPay < 0 ? 0 : balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        }
                        else if (finReceiptCusSoMpgList != null && finReceiptCusSoMpgList.Count > 0)
                        {
                            hdfSOPK.Value = finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_PK.ToString();
                            hdfReceiptSplitPK.Value = finReceiptCusSoMpgList[e.Row.RowIndex].RSO_PK.ToString();
                            string sodate = finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_DATE.ToString(Resources.Constants.DateFormatShort);
                            hdfReceiptTRXPK.Value = finReceiptCusSoMpgList[e.Row.RowIndex].RSO_RECEIPT_TRX_MPG.ToString();
                            lblPONOSplit.Text = finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NO;
                            lblPONOSplit.ToolTip = finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NO + "  " + sodate;
                            lblPODateSplit.Text = finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblPODateSplit.ToolTip = finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblCurrSplit.Text = finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.ADM_CURRENCY_MST.CUR_CODE;
                            lblCurrSplit.ToolTip = finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.ADM_CURRENCY_MST.CUR_CODE;
                            lblAmountSplit.Text = String.Format("{0:c}", finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NET_AMOUNT);
                            lblAmountSplit.ToolTip = String.Format("{0:c}", finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NET_AMOUNT);

                            //decimal paid = finReceiptCusSoMpgList[e.Row.RowIndex].RSO_BOUNCED == 0 ? finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_RECEIVED - ((finReceiptCusSoMpgList[e.Row.RowIndex].RSO_RECEIVED_AMOUNT) - finReceiptCusSoMpgList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG.RCM_EXCESS_AMOUNT) : finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_RECEIVED;
                            decimal paid = (finReceiptCusSoMpgList[e.Row.RowIndex].RSO_BOUNCED == 0 && finReceiptCusSoMpgList[e.Row.RowIndex].FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0) ? finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_RECEIVED - ((finReceiptCusSoMpgList[e.Row.RowIndex].RSO_RECEIVED_AMOUNT) - finReceiptCusSoMpgList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG.RCM_EXCESS_AMOUNT) : finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_RECEIVED;
                            lblPaidSplit.Text = String.Format("{0:c}", paid);
                            lblPaidSplit.ToolTip = String.Format("{0:c}", paid);


                            decimal tax = finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_TOTAL_TAX + finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SAL_ORDER_DTL.Sum(o => o.SOD_TAX);//.FIN_RECEIPT_CUS_TRX_MPG.FIN_INVOICE_CUS_HDR.ICH_TAX_TC;
                            decimal disc = finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_TOTAL_DISCOUNT + finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SAL_ORDER_DTL.Sum(o => o.SOD_DISCOUNT);//.FIN_RECEIPT_CUS_TRX_MPG.FIN_INVOICE_CUS_HDR.ICH_DISCOUNT_TC;
                            //- FinInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_RECEIVED;
                            lblTaxSplit.Text = String.Format("{0:c}", tax);
                            lblTaxSplit.ToolTip = String.Format("{0:c}", tax);
                            lblDiscountSplit.Text = String.Format("{0:c}", disc);
                            lblDiscountSplit.ToolTip = String.Format("{0:c}", disc);


                            hdfShippingrChargesSplitPercent = e.Row.FindControl("hdfShippingrChargesSplitPercent") as HiddenField;
                            decimal NetAmountSplit = finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NET_AMOUNT;
                            decimal OtherChargeSplit = finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_TOTAL_SHIP_CHARGE;
                            hdfShippingrChargesSplitPercent.Value = (OtherChargeSplit / NetAmountSplit).ToString();


                            decimal balToPay = (Convert.ToDecimal(finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NET_AMOUNT == null ? 0 : finReceiptCusSoMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NET_AMOUNT)
                                - paid);

                            lblBalanceSplit.Text = String.Format("{0:c}", balToPay < 0 ? 0 : balToPay);
                            lblBalanceSplit.ToolTip = String.Format("{0:c}", balToPay < 0 ? 0 : balToPay);


                            balToPay = finReceiptCusSoMpgList[e.Row.RowIndex].RSO_RECEIVED_AMOUNT;
                            decimal tempValue = finReceiptCusSoMpgList.Count <= 1 ? string.IsNullOrEmpty(lblInvSplitReceiveNow.ToolTip) ? Convert.ToDecimal(0) : Convert.ToDecimal(lblInvSplitReceiveNow.ToolTip.Trim().Replace(",", "")) : balToPay;
                            if (balToPay != tempValue)
                            {
                                isSplitChanged = true;
                                balToPay = tempValue;
                            }

                            if (tempFinReceiptCusSoMpgList != null)
                            {
                                //tempFinReceiptCusSoMpgObj = tempFinReceiptCusSoMpgList.SingleOrDefault(mpg => mpg.RSO_SO_HDR == Convert.ToInt64(hdfSOPK.Value));//same sc for multiple invoice
                                tempFinReceiptCusSoMpgObj = tempFinReceiptCusSoMpgList.SingleOrDefault(mpg => mpg.RSO_SO_HDR == Convert.ToInt64(hdfSOPK.Value) && mpg.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == InvoicePK);
                                //tempFinReceiptCusSoMpgObj = tempFinReceiptCusSoMpgList.SingleOrDefault(mpg => mpg.RSO_SO_HDR == Convert.ToInt64(hdfReceiptTRXPK.Value));
                            }
                            txtPayNowSplit.Text = txtPayNowSplit.ToolTip = hdfPayNowSplit.Value =
                                tempFinReceiptCusSoMpgObj == null ?
                                Math.Round(balToPay < 0 ? 0 : balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString()
                                : Math.Round(tempFinReceiptCusSoMpgObj.RSO_RECEIVED_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(); ;


                            if (hdfMultiCurrency.Value == "1")
                            {
                                // txtReceivedInBaseCurrency.Text = txtPayNowSplit.Text;
                                // txtEquivalentInvoiceAmount.Text = "0.00";
                                txtGainOrLoss.Text = "0.00";
                                double PayNow = txtPayNowSplit.Text == string.Empty ? 0 : Convert.ToDouble(txtPayNowSplit.Text);
                                double ExchangeRate = txtExchangeRate.Text == string.Empty ? 0 : Convert.ToDouble(txtExchangeRate.Text);
                                double ReceivedAmount = PayNow * ExchangeRate;
                                txtPayNowSplit.Text = txtPayNowSplit.ToolTip = hdfPayNowSplit.Value =
                                     Math.Round(ReceivedAmount < 0 ? 0 : ReceivedAmount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                                txtEquivalentInvoiceAmount.Text = lblBalanceSplit.Text;
                            }

                            category = finReceiptCusSoMpgList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG.FIN_INVOICE_CUS_HDR.ICH_CATEGORY;
                            type = finReceiptCusSoMpgList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG.FIN_INVOICE_CUS_HDR.ICH_TYPE;

                            lblTotalTaxSplit.Text = lblTotalTaxSplit.ToolTip = hdfTaxSplit.Value =
                                tempFinReceiptCusSoMpgObj == null ?
                               String.Format("{0:c}", 0)
                                : Math.Round(tempFinReceiptCusSoMpgObj.RSO_TAX_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                            //txtPayNowSplit.Text = Math.Round(balToPay < 0 ? 0 : balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //txtPayNowSplit.ToolTip = Math.Round(balToPay < 0 ? 0 : balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //hdfPayNowSplit.Value = Math.Round(balToPay < 0 ? 0 : balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        }

                        if (hdfMultiCurrency.Value == "1")
                        {

                            grdReceiptSplit.Columns[9].Visible = true;
                            grdReceiptSplit.Columns[10].Visible = true;
                            grdReceiptSplit.Columns[11].Visible = true;

                        }
                        else
                        {

                            grdReceiptSplit.Columns[9].Visible = false;
                            grdReceiptSplit.Columns[10].Visible = false;
                            grdReceiptSplit.Columns[11].Visible = false;

                        }


                        if (category == (int)SalesInvoiceCategory.Advanced)
                        {
                            if (type == (int)SalesInvoiceType.Domestic)
                            {
                                grdReceiptSplit.Columns[12].Visible = true;
                                grdReceiptSplit.Columns[13].Visible = true;
                            }
                            else
                            {
                                grdReceiptSplit.Columns[12].Visible = false;
                                grdReceiptSplit.Columns[13].Visible = false;
                            }
                        }
                        else
                        {
                            grdReceiptSplit.Columns[12].Visible = false;
                            grdReceiptSplit.Columns[13].Visible = false;
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (((GridView)sender).ID == "grdReceiptSplitAdjn")
                    {

                    }
                    else if (((GridView)sender).ID == "grdReceiptSplit")
                    {
                        if (e.Row.RowType == DataControlRowType.Header)
                        {
                            if (hdfMultiCurrency.Value == "1")
                            {
                                //e.Row.Cells[12].Text = GetLocalResourceObject("RecievedIn").ToString() + "(" + ddlReceiptCurrency.SelectedItem + ")";
                                e.Row.Cells[8].Text = GetLocalResourceObject("AllocatedReceiptAmount").ToString() + "(" + ddlReceiptCurrency.SelectedItem + ")";
                            }
                            else
                            {
                                e.Row.Cells[8].Text = GetLocalResourceObject("AllocateNow").ToString();
                                // e.Row.Cells[12].Text = GetLocalResourceObject("AllocateNow").ToString();
                            }
                            //  e.Row.Cells[9].Text = GetLocalResourceObject("AllocatedReceiptAmount").ToString() + "(" + ddlReceiptCurrency.SelectedItem + ")";
                            e.Row.Cells[11].Text = GetLocalResourceObject("ReceivedInBaseCurrency").ToString() + "(" + txtBaseCurrency.Text + ")";
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    if (((GridView)sender).ID == "grdReceiptSplitAdjn")
                    {

                        lblTotalAllocateAdjn = e.Row.FindControl("lblTotalAllocateAdjn") as Label;
                        hdfTotalAllocateAdjn = e.Row.FindControl("hdfTotalAllocateAdjn") as HiddenField;
                        hdfBalanceAdjn = e.Row.FindControl("hdfBalanceAdjn") as HiddenField;
                        //if (CrDrAdjnList != null && CrDrAdjnList.Count > 0)
                        //{
                        lblTotalAllocateAdjn.Text = String.Format("{0:c}", totAllocateAdjn);
                        hdfTotalAllocateAdjn.Value = totAllocateAdjn.ToString();
                        hdfBalanceAdjn.Value = totBalanceAdjn.ToString();
                        //}

                    }
                    else if (((GridView)sender).ID == "grdInvoiceList")
                    {
                        lblTotalFooter = e.Row.FindControl("lblTotalReceivedFooter") as Label;

                        Label lblTotalAmountFooter = e.Row.FindControl("lblTotalAmountFooter") as Label;
                        Label lblTaxAmountFooter = e.Row.FindControl("lblTaxAmountFooter") as Label;
                        Label lblOtherAmountFooter = e.Row.FindControl("lblOtherAmountFooter") as Label;
                        Label lblReceivedFooter = e.Row.FindControl("lblReceivedFooter") as Label;
                        Label lblAdjAmountFooter = e.Row.FindControl("lblAdjAmountFooter") as Label;
                        Label lblBaltoReceiveFooter = e.Row.FindControl("lblBaltoReceiveFooter") as Label;

                        if (finInvoiceCusHdrList != null && finInvoiceCusHdrList.Count > 0)
                        {


                            total = 0;
                            total = finInvoiceCusHdrList.Sum(dtl => dtl.ICH_AMOUNT_NET_TC - dtl.ICH_AMOUNT_RCVD_TC);
                            total = total < 0 ? 0 : total;
                            lblTotalFooter.Text = string.Format("{0:c}", total);
                            txtReceivedAmount.Text = Math.Round(total, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        }
                        else if (finReceiptCusTrxMpgList != null && finReceiptCusTrxMpgList.Count > 0)
                        {
                            total = 0;
                            total = finReceiptCusTrxMpgList.Sum(dtl => dtl.RCM_RCVD_AMOUNT);
                            lblTotalFooter.Text = string.Format("{0:c}", total);
                            txtReceivedAmount.Text = Math.Round(total, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        }
                        lblTotalAmountFooter.Text = String.Format("{0:c}", totalAmtFooter);
                        lblTaxAmountFooter.Text = String.Format("{0:c}", totalTaxFooter);
                        lblOtherAmountFooter.Text = String.Format("{0:c}", totalOtheramtFooter);
                        lblReceivedFooter.Text = String.Format("{0:c}", totalReceivedFooter);
                        lblAdjAmountFooter.Text = adjAmtFooter == 0 ? "0.00" : String.Format("{0:c}", adjAmtFooter);
                        lblBaltoReceiveFooter.Text = String.Format("{0:c}", balRecieveFooter);
                    }
                    else if (((GridView)sender).ID == "grdReceiptSplit")
                    {
                        //Test
                        lblTotalPayNowFooterSplit = e.Row.FindControl("lblTotalPayNowFooterSplit") as Label;
                        hdfTotalPayNowFooterSplit = e.Row.FindControl("hdfTotalPayNowFooterSplit") as HiddenField;
                        if (FinInvoiceCusTrxMpgList != null && FinInvoiceCusTrxMpgList.Count > 0)
                        {
                            if (FinInvoiceCusTrxMpgList.Count == 1)
                            {
                                total = string.IsNullOrEmpty(lblInvSplitReceiveNow.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(lblInvSplitReceiveNow.Text.Trim().Replace(",", ""));
                            }
                            else
                            {
                                total = 0;
                            }
                            //lblTotalPayNowFooterSplit.Text = string.Format("{0:c}", total < 0 ? 0 : total);
                            //hdfTotalPayNowFooterSplit.Value = total.ToString();
                            if (tempFinReceiptCusSoMpgList == null || tempFinReceiptCusSoMpgList.Count == 0)
                            {
                                lblTotalPayNowFooterSplit.Text = string.Format("{0:c}", total < 0 ? 0 : total);
                                hdfTotalPayNowFooterSplit.Value = total.ToString();
                            }
                            else
                            {
                                lblTotalPayNowFooterSplit.Text = string.Format("{0:c}", tempFinReceiptCusSoMpgList.Sum(mpg => mpg.RSO_RECEIVED_AMOUNT));
                                hdfTotalPayNowFooterSplit.Value = tempFinReceiptCusSoMpgList.Sum(mpg => mpg.RSO_RECEIVED_AMOUNT).ToString();
                            }
                        }
                        else if (finReceiptCusSoMpgList != null && finReceiptCusSoMpgList.Count > 0)
                        {
                            //if (!isSplitChanged)
                            //{
                            if (finReceiptCusSoMpgList.Count == 1)
                            {
                                total = string.IsNullOrEmpty(lblInvSplitReceiveNow.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(lblInvSplitReceiveNow.Text.Trim().Replace(",", ""));
                            }
                            else
                            {
                                total = finReceiptCusSoMpgList.Sum(aa => aa.RSO_RECEIVED_AMOUNT);
                            }
                            //}
                            //else
                            //{
                            //    total = 0;
                            //}
                            //lblTotalPayNowFooterSplit.Text = string.Format("{0:c}", total < 0 ? 0 : total);
                            //hdfTotalPayNowFooterSplit.Value = total.ToString();
                            if (tempFinReceiptCusSoMpgList == null || tempFinReceiptCusSoMpgList.Count == 0)
                            {
                                lblTotalPayNowFooterSplit.Text = string.Format("{0:c}", total < 0 ? 0 : total);
                                hdfTotalPayNowFooterSplit.Value = total.ToString();
                            }
                            else
                            {
                                lblTotalPayNowFooterSplit.Text = string.Format("{0:c}", tempFinReceiptCusSoMpgList.Sum(mpg => mpg.RSO_RECEIVED_AMOUNT));
                                hdfTotalPayNowFooterSplit.Value = tempFinReceiptCusSoMpgList.Sum(mpg => mpg.RSO_RECEIVED_AMOUNT).ToString();
                            }

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
                GetFieldValues(ControlsEnum.PDCVOUCHERLIST);
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
                GetFieldValues(ControlsEnum.PDCVOUCHERLIST);
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

                hdfSaveWithoutBankCharge.Value = "0";
                if (grdInvoiceList.Rows != null)
                {
                    foreach (GridViewRow gvr in grdInvoiceList.Rows)
                    {
                        HiddenField hdfInvoicePK = gvr.FindControl("hdfInvoicePK") as HiddenField;
                        if (hdfInvoicePK != null)
                        {
                            (gvr.FindControl("hdfHasSplit") as HiddenField).Value = InvoiceSOSplitList.Any(dtl => dtl.FIN_RECEIPT_CUS_TRX_MPG.RCM_INVOICE_HDR == Convert.ToInt64(hdfInvoicePK.Value))
                                ? CommonConstants.SELECT_VALUE_ONE : CommonConstants.SELECT_VALUE_ZERO;
                        }
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
        ///<summary>
        ///Set Visibility for Multi Currency Button
        ///</summary>
        private void SetMultiCurrencyConfiguration()
        {
            if (hdfMultiCurrencyInReceipt.Value == "1")
            {
                if (ddlReceiptCurrency.SelectedValue == hdfReceiptCurrency.Value)
                {
                    txtExchangeRate.Text = "1";
                    hdfMultiCurrency.Value = "0";
                    grdInvoiceList.Columns[14].Visible = false;
                }
                else
                {
                    hdfMultiCurrency.Value = "1";
                    grdInvoiceList.Columns[14].Visible = true;
                }
            }
            else
            {
                hdfMultiCurrency.Value = "0";
                grdInvoiceList.Columns[14].Visible = false;
            }
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
            lblSuspAmt.Visible = Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "ShowSuspenceListButton"));
            txtSuspenceAmt.Visible = Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "ShowSuspenceListButton"));
            customSuspAmt.Enabled = Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "ShowSuspenceListButton"));
            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "CUSTOMER");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsSBUCustomer.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
            }
            dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "BANK");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsSBUBank.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
            }
            hdfMultiCurrencyInReceipt.Value = GetGlobalResourceObject("ConfigurationsRes", "MultiCurrencyInReceipt").ToString();
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
                        if (ReceiptCrdrList != null)
                        {
                            //PaidCNAmount = ReceiptCrdrList.Where(r => r.RNM_INVOICE_HDR == Convert.ToInt64(hdfInvoicePK.Value)).Sum(sm => sm.RNM_BALANCE_AMOUNT);
                            BalanceDN = ReceiptCrdrList.Where(r => r.RNM_INVOICE_HDR == Convert.ToInt64(hdfInvoicePK.Value)).Sum(sm => sm.RNM_BALANCE_AMOUNT - (sm.RNM_ADJ_AMOUNT + sm.RNM_PAID_AMOUNT));
                        }
                        //decimal InvoicePayable = Payable - (Paid - PaidCNAmount);
                        //if (Convert.ToDecimal(txtPayNow.Text) > (InvoicePayable + AllocatedCN))
                        //{
                        //    litErrorMsg.Text = GetLocalResourceObject("Err_msg_DNAmount").ToString();
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //    result = false;
                        //    break;
                        //}
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
                    if (ReceiptCrdrList != null)
                    {
                        //PaidCNAmount = ReceiptCrdrList.Where(r => r.RNM_INVOICE_HDR == Convert.ToInt64(hdfInvoicePK.Value)).Sum(sm => sm.RNM_BALANCE_AMOUNT);
                        BalanceDN = ReceiptCrdrList.Where(r => r.RNM_INVOICE_HDR == Convert.ToInt64(hdfInvoicePK.Value)).Sum(sm => sm.RNM_BALANCE_AMOUNT - (sm.RNM_ADJ_AMOUNT + sm.RNM_PAID_AMOUNT));
                    }
                    //decimal InvoicePayable = Payable - (Paid - PaidCNAmount);
                    //if (Convert.ToDecimal(txtPayNow.Text) > (InvoicePayable + AllocatedCN))
                    //{
                    //    litErrorMsg.Text = GetLocalResourceObject("Err_msg_DNAmount").ToString();
                    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    //    result = false;
                    //    break;
                    //}
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
            RECEIPTHDRENTRY,
            RECEIPTMPGENTRY,
            RECEIPTMPGLIST,
            BANK,
            RECEIPTHDRINVLISTBYPK,
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
            RECEIPTSPLITLISTBYRECEIPTPK,
            PAYMODE,
            FINPERIOD,
            GETRECEIPTPKBYJOURNALPK,
            FILLWORKFLOWSTATUS,
            DISCOUNTTYPE,
            REVERSE,
            NOTIFICATIONDAYS,
            ALERTCONFIG,
            ALERTLIST,
            ALERTTYPES,
            ALERTBASIS,
            NOTIFICATIONTYPES,
            ALERTSAVE,
            BANKCURRENCY,
            EXCHANGERATEBANK,
            CHEQUERETURN,
            COMPANY,
            CUSTOMERBANK,
            PDCVOUCHERLIST,
            FINHEADERSTATUS,
            CHKINSTRNO,
            FINHEADERSTATUSREVERSE,
            RECEIPTCUSADJN,
            ADJNSPLITLIST,
            RECEIPTADJNLIST,
            ISADVDEDUCTED,
            RECEIPTADJNDUMMYLIST,
            SPLITPAYNOWFORMULISO,
            INVOICEVNDMPGLISTFORAUTOALCN,
            CRDRALLOCATION,
            CRDRSPLITLIST,
            RECEIVEDAMTSPLITUP,
            CRDRMPGLIST,
            INVOICEHDRBYPK,
            RECEIPTGET,
            CheckReceiptAllocation,
            SUSPENCELIST,
            CURRENCY,
            EXCHANGERATEINRECEIPTCURRENCY,
            GETMULTIPLEEXCHANGERATES,
            CHECKCREDITDEBITPOST

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