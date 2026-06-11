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
using BusinessObject.AlertManagement;
using BusinessObject.PurchaseOrderManagement;
using System.IO;
using BusinessLogic.CommonManagement;

namespace ERPSMS_v01.POInvoicing
{
    public partial class PurchaseOrderInvoice : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
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
        /// Payment detail PK
        /// </summary>
        private int PaymentMpgPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.paymentMpgPK] != null ? Convert.ToInt32(this.ViewState[ViewstateStrings.paymentMpgPK]) : 0;
            }
            set
            {
                this.ViewState[ViewstateStrings.paymentMpgPK] = value;
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

        private bool hasValidRate
        {
            get
            {
                return this.ViewState["hasValidRate"] == null ? false : Convert.ToBoolean(this.ViewState["hasValidRate"]);
            }
            set
            {
                this.ViewState["hasValidRate"] = value;
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
        /// To maintain the pid in Session
        /// </summary>
        private string PIType
        {
            get
            {
                return (string)Session["PIType"];
            }
            set
            {
                Session["PIType"] = value;
            }
        }
        /// <summary>
        /// Current Quotation PK
        /// </summary>
        private int CurrPOPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrSOPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrSOPK] = value;
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
        /// Tax PK
        /// </summary>
        private int POdtlPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState["POdtlPK"]);
            }
            set
            {
                this.ViewState["POdtlPK"] = value;
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

        public InvoiceAction InvoiceStatus
        {
            get
            {
                return this.ViewState[ViewstateStrings.InvoiceActionState] == null ? InvoiceAction.ListInvoice : (InvoiceAction)(this.ViewState[ViewstateStrings.InvoiceActionState]);
            }
            set
            {
                this.ViewState[ViewstateStrings.InvoiceActionState] = value;
            }
        }

        /// <summary>
        /// To maintain keep PO Invoice Header Tax Splitting
        /// </summary>
        private POInvoiceHeader POInvoiceHeaderSession
        {
            get
            {
                return (POInvoiceHeader)Session[ERP.Utilities.SessionStrings.SOInvoiceHeaderSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SOInvoiceHeaderSession] = value;
            }
        }
        /// <summary>
        /// To maintain keep PO Invoice Header Tax Splitting
        /// </summary>
        private POInvoiceHeader TempInvoiceHeaderTemp
        {
            get
            {
                return (POInvoiceHeader)Session[ERP.Utilities.SessionStrings.TempSOInvoiceDtlSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.TempSOInvoiceDtlSession] = value;
            }
        }
        private POInvoiceHeader POInvoiceHdrSession
        {
            get
            {
                return (POInvoiceHeader)Session[ERP.Utilities.SessionStrings.POInvoiceHeaderSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.POInvoiceHeaderSession] = value;
            }
        }

        /// <summary>
        /// To maintain keep PO Invoice Header Tax Splitting
        /// </summary>
        private POInvoiceHeader TempPOInvoiceHeaderSession
        {
            get
            {
                return (POInvoiceHeader)this.ViewState[ERP.Utilities.SessionStrings.TempSOInvoiceHeaderSession];
            }
            set
            {
                this.ViewState[ERP.Utilities.SessionStrings.TempSOInvoiceHeaderSession] = value;
            }
        }

        /// <summary>
        /// PO Invoice PK
        /// </summary>
        private int POInvoicePK
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
        private int SelectedPOPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedPOPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedPOPK] = value;
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
        /// For keep Po group for Purchase invoice workflow filling
        /// </summary>
        private POGroup POInvoiceType
        {
            get
            {
                return (Session[ERP.Utilities.SessionStrings.POInvoiceType] == null ? (POGroup)0 : (POGroup)Session[ERP.Utilities.SessionStrings.POInvoiceType]);
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.POInvoiceType] = value;
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
        private long InvCategory
        {
            get
            {
                return Convert.ToInt64(this.ViewState["InvCategory"]);
            }
            set
            {
                this.ViewState["InvCategory"] = value;
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

        private int SelectedPOTypes
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedPOTypes]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedPOTypes] = value;
            }
        }
        //List<POInvoiceDetails> TempsoInvoiceDetailsList;
        private List<POInvoiceDetails> TempsoInvoiceDetailsList
        {
            get
            {
                return (List<POInvoiceDetails>)this.ViewState[ERP.Utilities.SessionStrings.TempSOInvoiceDtlSession];
            }
            set
            {
                this.ViewState[ERP.Utilities.SessionStrings.TempSOInvoiceDtlSession] = value;
            }
        }

        /// <summary>
        /// To keep selected Purchase Order PKs 
        /// </summary>
        private POHeaderBO InvoiceMultiplePOPKs
        {
            get
            {
                return this.Session[ERP.Utilities.SessionStrings.InvoiceMultiplePOPKs] == null ? null : (POHeaderBO)this.Session[ERP.Utilities.SessionStrings.InvoiceMultiplePOPKs];
            }
            set
            {
                this.Session[ERP.Utilities.SessionStrings.InvoiceMultiplePOPKs] = value;
            }
        }

        /// <summary>
        /// To keep Newly adding Purchase Order PKs 
        /// </summary>
        private POHeaderBO InvoiceMultiplePOPKsNew
        {
            get
            {
                return this.Session[ERP.Utilities.SessionStrings.InvoiceMultiplePOPKsNew] == null ? null : (POHeaderBO)this.Session[ERP.Utilities.SessionStrings.InvoiceMultiplePOPKsNew];
            }
            set
            {
                this.Session[ERP.Utilities.SessionStrings.InvoiceMultiplePOPKsNew] = value;
            }
        }

        /// <summary>
        /// To maintain selected PO in view state
        /// </summary>
        private List<long> selectedPOList
        {
            get
            {
                return (List<long>)ViewState[ERP.Utilities.ViewstateStrings.SelectedPO];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.SelectedPO] = value;
            }
        }

        public double POTotalGRNQty
        {
            get
            {
                return Convert.ToDouble(this.ViewState[ViewstateStrings.POTotalGRNQty]);
            }
            set
            {
                this.ViewState[ViewstateStrings.POTotalGRNQty] = value;
            }
        }

        public double POTotalGRNInvdQty
        {
            get
            {
                return Convert.ToDouble(this.ViewState[ViewstateStrings.POTotalGRNInvdQty]);
            }
            set
            {
                this.ViewState[ViewstateStrings.POTotalGRNInvdQty] = value;
            }
        }

        public double GRTotalDmgQty
        {
            get
            {
                return Convert.ToDouble(this.ViewState["GRTotalDmgQty"]);
            }
            set
            {
                this.ViewState["GRTotalDmgQty"] = value;
            }
        }

        public double POTotalGRNBalance
        {
            get
            {
                return Convert.ToDouble(this.ViewState[ViewstateStrings.POTotalGRNBalance]);
            }
            set
            {
                this.ViewState[ViewstateStrings.POTotalGRNBalance] = value;
            }
        }

        public decimal POTotalOtherAmount
        {
            get
            {
                return Convert.ToDecimal(this.ViewState[ViewstateStrings.POTotalOtherAmount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.POTotalOtherAmount] = value;
            }
        }

        public decimal POTotalInvOtherAmount
        {
            get
            {
                return Convert.ToDecimal(this.ViewState[ViewstateStrings.POTotalInvOtherAmount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.POTotalInvOtherAmount] = value;
            }
        }

        public decimal POTotalBalanceOtherAmount
        {
            get
            {
                return Convert.ToDecimal(this.ViewState[ViewstateStrings.POTotalBalanceOtherAmount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.POTotalBalanceOtherAmount] = value;
            }
        }

        public decimal DedTotalAllocateNowFooterSplit
        {
            get
            {
                return Convert.ToDecimal(this.ViewState[ViewstateStrings.DedTotalAllocateNowFooterSplit]);
            }
            set
            {
                this.ViewState[ViewstateStrings.DedTotalAllocateNowFooterSplit] = value;
            }
        }
        public decimal OtherAmountFooterSplit
        {
            get
            {
                return Convert.ToDecimal(this.ViewState[ViewstateStrings.OtherAmountFooterSplit]);
            }
            set
            {
                this.ViewState[ViewstateStrings.OtherAmountFooterSplit] = value;
            }
        }

        public decimal TaxFooterSplit
        {
            get
            {
                return Convert.ToDecimal(this.ViewState[ViewstateStrings.TaxFooterSplit]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TaxFooterSplit] = value;
            }
        }

        public double PrevSubTotalPOAmount
        {
            get
            {
                return Convert.ToDouble(this.ViewState[ViewstateStrings.PrevSubTotalPOAmount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.PrevSubTotalPOAmount] = value;
            }
        }

        public double PrevTotalPOAmountDiscount
        {
            get
            {
                return Convert.ToDouble(this.ViewState[ViewstateStrings.PrevTotalPOAmountDiscount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.PrevTotalPOAmountDiscount] = value;
            }
        }

        public double POTotalTaxAmount
        {
            get
            {
                return Convert.ToDouble(this.ViewState[ViewstateStrings.POTotalTaxAmount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.POTotalTaxAmount] = value;
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
        /// Tax calculation is enabled or disabled for advance invoice .
        /// </summary>
        private bool IsAdvInvHasTax
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsAdvInvHasTax] == null ? true : (bool)this.ViewState[ViewstateStrings.IsAdvInvHasTax];
            }
            set
            {
                this.ViewState[ViewstateStrings.IsAdvInvHasTax] = value;
            }
        }

        /// <summary>
        /// Hide Eff.Rate Column in PO Invoice Details Grid(For COVCO)
        /// </summary>
        private bool IsShowEffRateInPOInvoice
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsShowEffRateInPOInvoice] == null ? true : (bool)this.ViewState[ViewstateStrings.IsShowEffRateInPOInvoice];
            }
            set
            {
                this.ViewState[ViewstateStrings.IsShowEffRateInPOInvoice] = value;
            }
        }
        /// <summary>
        /// Enable or disable Other charges
        /// </summary>
        private bool IsEnableOtherCharges
        {
            get
            {
                return this.ViewState["IsEnableOtherCharges"] == null ? true : (bool)this.ViewState["IsEnableOtherCharges"];
            }
            set
            {
                this.ViewState["IsEnableOtherCharges"] = value;
            }
        }


        /// <summary>
        /// Total allocated discount amount
        /// </summary>
        private decimal totalAllocatedDiscount
        {
            get
            {
                return this.ViewState[ViewstateStrings.AllocatedDiscount] == null ? 0 : Convert.ToDecimal(this.ViewState[ViewstateStrings.AllocatedDiscount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.AllocatedDiscount] = value;
            }
        }

        public bool IsTaxProportionate { get; set; }

        #endregion

        // Indicates the state as well as action
        private ActionsEnum commonActions;
        User currentUser;
        //page related Entity Object
        private MultiplePOInvoiceHeader multipleinvoiceHeaderObj;
        private MultiplePOInvoiceHeader multipleinvoiceHeaderObjNew;
        private POInvoiceHeader invoiceHeaderObj;
        private POInvoiceHeader invoiceHeaderObjNew;
        private POInvoiceHeader invoiceHeaderTemp;
        private POInvoiceDetails soInvoiceDetailsObj;
        private POInvoiceTaxHdr soInvTaxHdrObj;
        private POOtherChargeDetails poInvOtherchargeObj;
        private GRNQTYDetails poGrnQtyObj;
        List<POAdvDeductionDetails> deductionDtlList;
        List<POCostCenterDetails> poCostCenterlst;
        private List<decimal> SelectedINVTaxList;
        POInvoiceUploads poUploadObj;
        //private RFQTaxDtl rfqTaxDtlObj;
        List<POInvoiceDetails> soInvoiceDetailsList;
        List<POOtherChargeDetails> poOtherChargeList;
        List<GRNQTYDetails> grnList;

        //List<RFQTaxDtl> rfqTaxDtlList;
        //RFQTaxSplit rfqDtlSplitObj;
        List<POInvoiceTaxHdr> taxHdrList;
        POInvoiceDetails soDtlObj;
        string selectedVendor;
        DataSet dsInvHeader;
        DataTable dtTaxDetails;

        private DataTable dtTaxDetData;
        private DataTable dtAmountDetails;

        DataTable dtTaxSettings;

        DataTable dtAdsType;
        DataTable dtAdsTypeDtl;

        DataSet dsAdsType;
        DataSet dsAdsTypeDtl;
        private DataSet dsPOList;

        int JournalPK;

        DataSet dsPageData;
        private DataTable dtInvoiceList;
        private DataTable dtPOList;
        private DataTable dtPageData;
        private DataTable dtCustomTaxSet;

        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;

        private string refID;
        private string inboxFlag;

        private decimal totalAllocatedTax;
        //private decimal totalAllocatedDiscount;
        private decimal amtAdjAdvDeduction = 0;
        private decimal totalAllocatedAmtAdjust;
        private decimal AmtAdjustPerInvoice;
        private decimal TotalHDRDiscount = 0;
        private decimal HDRDiscount = 0;

        private long InvoicePk = 0;
        private decimal hrdDiscAmnt = 0;

        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;

        private ADM_CONFIG_MST admConfigMstObj;
        private List<ADM_CONFIG_MST> workflowStatusList;

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
        //private Int16 ivhGroup;

        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        private DataTable dtCompany;

        private FIN_INVOICE_VND_HDR finInvoiceVndHdrObj;
        int ReqDept = 0;
        private int vndPK;
        bool isCancelled = false;
        private string[] _operators = { "-", "+", "/", "*", "^" };
        private Func<double, double, double>[] _operations = {
            (a1, a2) => a1 - a2,
            (a1, a2) => a1 + a2,
            (a1, a2) => a1 / a2,
            (a1, a2) => a1 * a2,
            (a1, a2) => Math.Pow(a1, a2)
        };

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
        private int EnableGST
        {
            get
            {
                return Convert.ToInt32(this.ViewState["EnableGST"]);
            }
            set
            {
                this.ViewState["EnableGST"] = value;
            }
        }

        private int ShowbtnConvertAll
        {
            get
            {
                return Convert.ToInt32(this.ViewState["ShowbtnConvertAll"]);
            }
            set
            {
                this.ViewState["ShowbtnConvertAll"] = value;
            }
        }

        private int EnableCostCenter
        {
            get
            {
                return Convert.ToInt32(this.ViewState["EnableCostCenter"]);
            }
            set
            {
                this.ViewState["EnableCostCenter"] = value;
            }
        }


        /// <summary>
        /// PaymentFlag
        /// </summary>
        private int PaymentFlag
        {
            get
            {
                return Convert.ToInt32(this.Session[ERP.Utilities.SessionStrings.PaymentFlag]);
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.PaymentFlag] = value;
            }
        }

        /// <summary>
        /// CrDrFlag
        /// </summary>
        private int CrDrFlag
        {
            get
            {
                return Convert.ToInt32(this.Session[ERP.Utilities.SessionStrings.CrDrFlag]);
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.CrDrFlag] = value;
            }
        }
        private int IvhGroup
        {
            get
            {
                return Convert.ToInt32(this.ViewState["IvhGroup"]);
            }
            set
            {
                this.ViewState["IvhGroup"] = value;
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
        /// 

        protected override void OnLoadComplete(EventArgs e)
        {
            if (!IsPostBack)
            {
                //assign Page BreadCrumb
                base.OnLoadComplete(e);
                AssignLocalBreadCrumb();
            }
        }

        public void AssignLocalBreadCrumb()
        {
            try
            {
                //Breadcrumb Material Return
                if (Request.QueryString["PID"] != null && Request.QueryString["PID"].ToString() == "1")
                {
                    if (this.GetLocalResourceObject("Breadcrumb_Invoice") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("Breadcrumb_Invoice").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;

                    }
                }
                else if (Request.QueryString["PID"] != null && Request.QueryString["PID"].ToString() == "21")
                {
                    if (this.GetLocalResourceObject("Breadcrumb_Invoice_service") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("Breadcrumb_Invoice_service").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                        // Page.Title = GetLocalResourceObject("MaterialReturnTitle").ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }



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
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANYSRCH);
                    EnableGST = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "EnableGST"));
                    EnableCostCenter = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "EnableCostCenter"));
                    ShowbtnConvertAll = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsShowbtnConvertAll"));

                    if (ShowbtnConvertAll == 0)
                    {
                        btnConvertAll.Visible = false;
                    }

                    if (EnableGST == 1)
                    {
                        lblGSTInvoiceType.Visible = true;
                        ddlGSTInvoiceType.Visible = true;
                        rfvGSTInvType.Enabled = true;
                        GetFieldValues(ControlsEnum.GSTINVOICETYPE);
                        SetFieldValues(ControlsEnum.GSTINVOICETYPE);
                        lblBillAmount.Visible = true; txtBillAmount.Visible = true;
                        divBillDet.Visible = true;
                    }
                    else
                    {
                        lblBillAmount.Visible = false; txtBillAmount.Visible = false;
                        divBillDet.Visible = false;
                    }

                    //Enable or disable custom tax 
                    GetFieldValues(ControlsEnum.CUSTOMTAXSETTINGS);

                    //TaxPayable dIV VISIBILITY sETTING
                    ConfigurationSettings();
                    SetControlsVisibility();
                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "DOC_SEQ_NO";
                    grdUploads.DataKeyNames = itemkeyarray;
                    grdGrnAttchments.DataKeyNames = itemkeyarray;

                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = "IVH_PK";
                    grdInvoiceList.DataKeyNames = datakeyarray;

                    Session[ERP.Utilities.SessionStrings.SelectedPos] = null;
                    // hdfNumberDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
                    hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfDecimalFormatWithSeperation.Value = "#" + currencysep + "#0.";
                    hdfCurrencyFormatWithSeperation.Value = "#" + currencysep + "#0.";
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
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                        hdfCurrencyFormatWithSeperation.Value += "0";
                    }
                    hdfRateFormat.Value = "#0.";
                    int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P]));
                    hdfRateDecimalDigits.Value = rateDecimalDigits.ToString();
                    for (int i = 0; i < rateDecimalDigits; i++)
                    {
                        hdfRateFormat.Value += "0";
                    }
                    hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                    hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();

                    SelectedInvoicesCrDr = null;
                    SelectedInvoices = null;
                    SelectedInvoicesInfoLst = null;

                    //txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    //hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    //txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    //hdfToDate.Value = DateTime.Now.ToString();
                    txtFromDate.Text = string.Empty;
                    hdfFromDate.Value = string.Empty;
                    txtToDate.Text = string.Empty;
                    hdfToDate.Value = string.Empty;


                    GetFieldValues(ControlsEnum.POTYPE);
                    SetFieldValues(ControlsEnum.POTYPE);

                    GetFieldValues(ControlsEnum.PURCHASEORDERTYPE);
                    SetFieldValues(ControlsEnum.PURCHASEORDERTYPE);

                    AST_DOC_MODE.Value = "0";

                    POInvoiceHeaderSession = null;

                    InvoiceStatus = InvoiceAction.ListInvoice;

                    if (Session["PURCHASEORDERPK"] != null) //Invoice for single PO
                    {
                        CurrPOPK = Convert.ToInt32(Session["PURCHASEORDERPK"]);
                        Session["PURCHASEORDERPK"] = null;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateInvNow", "$(document).ready(function () { CalculateInvNow();});", true);
                        ////start
                        EntryStatus = EntryStatus.NEWMODE;
                        ////
                        InvoiceStatus = InvoiceAction.SingleInvoice;
                    }
                    else if (InvoiceMultiplePOPKs != null) // Invoice for multiple PO
                    {
                        EntryStatus = EntryStatus.NEWMODE;
                        POHeaderBO objPOHeaderItem = new POHeaderBO();
                        objPOHeaderItem = InvoiceMultiplePOPKs;
                        InvoiceStatus = InvoiceAction.MultipleInvoice;
                    }

                    ////start
                    string pid = Request.QueryString[QueryStrings.PID] != null ? Request.QueryString[QueryStrings.PID]
                         : Session[ERP.Utilities.SessionStrings.PID] != null ? Session[ERP.Utilities.SessionStrings.PID].ToString().Split('=')[1] : string.Empty;
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                        : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;
                    PIType = pid;
                    //If new Mode, Workflow fill for selected PO group
                    if (string.IsNullOrEmpty(refID))
                    {
                        if (POInvoiceType == BusinessObject.CommonManagement.POGroup.Services)
                        {
                            PIType = "21";
                        }
                        else if (POInvoiceType == BusinessObject.CommonManagement.POGroup.Goods)
                        {
                            PIType = "1";
                        }
                    }
                    //Clear PO Invoice Type
                    POInvoiceType = BusinessObject.CommonManagement.POGroup.Other;

                    if (PIType == "21")
                        FillProcessID(21);
                    else
                        FillProcessID(1);

                    //If Request From External(Report or Other page) otherthan Menu or Inbox
                    if (Request.QueryString[QueryStrings.FromExt] != null && Request.QueryString[QueryStrings.FromExt] == "T")
                    {
                        ucrWrkf.ViewType = 0;
                        EntryStatus = EntryStatus.VIEWMODE;
                        GetFieldValues(ControlsEnum.INVOICEGET);
                        SetFieldValues(ControlsEnum.INVOICEGET);
                    }
                    else
                    {
                        //If Has RefID (from Inbox)
                        if (!string.IsNullOrEmpty(refID))
                        {
                            if (!string.IsNullOrEmpty(inboxFlag))
                            {
                                ucrWrkf.ViewType = 0;
                                EntryStatus = EntryStatus.VIEWMODE;
                                //btnSave.Visible = false;
                                //btnSubmitInv.Visible = false;
                            }
                            else
                            {
                                ucrWrkf.ViewType = 1;
                                EntryStatus = EntryStatus.ENTRYMODE;
                            }
                            ReferanceID = int.Parse(refID);
                            ////start
                            if (string.IsNullOrEmpty(pid) || pid.Equals("1") || pid.Equals("11") || pid.Equals("21") || pid.Equals("31"))
                            {
                                ucrWrkf.RefID = int.Parse(refID);
                                base.WkfRefID = ucrWrkf.RefID;
                                CurrPK = GetApplicationID(ucrWrkf.RefID);
                                if (pid.Equals("11") || pid.Equals("31"))
                                    hdfIsInvCancelled.Value = "1";//For Showing Cancelled Stamp in Detail Page
                            }
                            else if (pid.Equals("2") || pid.Equals("12"))
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
                            ReferanceID = int.Parse(prefID);
                        }
                        if (InvoiceStatus == InvoiceAction.SingleInvoice || InvoiceStatus == InvoiceAction.MultipleInvoice || CurrPOPK > 0 || CurrPK > 0) // if (CurrPOPK > 0 || CurrPK > 0)
                        {
                            SetInvoicePODetails(InvoiceStatus);
                        }
                        else
                        {
                            TempPOInvoiceHeaderSession = null;
                            POInvoiceHeaderSession = null;

                            GetFieldValues(ControlsEnum.INVOICELIST);
                            SetFieldValues(ControlsEnum.INVOICELIST);
                            EntryStatus = EntryStatus.LISTMODE;

                            //btnEdit.Visible = btnJournalize.Visible = btnPickForCrDrNote.Visible = btnPickForPayment.Visible =
                            //   BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(currentUser.PKUser, FillProcessId());
                        }

                        AST_CODE.Value = POGroup == POInvoiceGroup.Goods ? ApplicationType.PI
                            : POGroup == POInvoiceGroup.Services ? ApplicationType.PSI : ApplicationType.EI;
                        AST_DOC_MODE.Value = GetDOCMODE();
                        lblInvoiceNo.Text = hdfInvoiceNo.Value == string.Empty ? "[NEW]" : hdfInvoiceNo.Value;

                        hdfAppType.Value = AST_CODE.Value;
                        hdfAppSubType.Value = string.Empty;
                        SetEffectiveRate();

                        if (POGroup == POInvoiceGroup.WorkOrder)
                        {
                            grdInvoice.Columns[13].Visible = false;
                        }
                        else
                        {
                            grdInvoice.Columns[13].Visible = true;
                        }
                    }

                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideConvert", "$(document).ready(function(){HideConvertTo();});", true);
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideConvert", "ShowHideConvertTo();", true);

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

                if (IsShowEffRateInPOInvoice == false)
                {
                    grdInvoice.Columns[2].Visible = false;
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

        private void SetEffectiveRate()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateEffectiveRate", "$(document).ready(function () { CalculateEffectiveRate();});", true);
        }
        #endregion


        private void SetInvoicePODetails(InvoiceAction Actionenum)
        {
            SetCancelRef(CurrPK);
            GetFieldValues(ControlsEnum.POINVHEADER);
            if (invoiceHeaderObj.IVH_TYPE == ((byte)PurchaseType.Local).ToString())
            {
                divVendorBranch.Visible = true;
                vndPK = Convert.ToInt32(invoiceHeaderObj.IVH_VENDOR);
                GetFieldValues(ControlsEnum.VENDORBRANCH);
                SetFieldValues(ControlsEnum.VENDORBRANCH);
            }
            else
            {
                divVendorBranch.Visible = false;
            }
            if (CurrPK == 0 && POInvoiceHeaderSession != null && POInvoiceHeaderSession.OrderDetail != null)
            {
                POInvoiceHeaderSession.IVH_AMOUNT_TC = 0;
                POInvoiceHeaderSession.IVH_AMOUNT_NET_TC_ADJ = 0;
                POInvoiceHeaderSession.IVH_TAX_TC = 0;
                POInvoiceHeaderSession.IVH_DISCOUNT_TC = 0;
                // POInvoiceHeaderSession.IVH_SHIP_CHARGE = 0;
                //POInvoiceHeaderSession.IVH_AMOUNT_ADJUST = 0;
                POInvoiceHeaderSession.IVH_AMOUNT_NET_TC = 0;
                POInvoiceHeaderSession.IVH_AMOUNT_NET_BC = 0;
                foreach (POInvoiceDetails dtl in POInvoiceHeaderSession.OrderDetail)
                {
                    dtl.VID_QTY_INVOICED = 0;
                    dtl.VID_AMOUNT = 0;
                    dtl.VID_TAX = 0;
                    dtl.VID_DISCOUNT = 0;
                    dtl.VID_NET_AMOUNT = 0;

                    if (Convert.ToBoolean(dtl.VID_HAS_GRN))
                    {
                        //For Avoiding Negative Quantity -----------
                        //dtl.VID_QTY_INVOICED = dtl.VID_ORDERED_QTY - dtl.VID_INV_QTY;
                        if (dtl.VID_INV_QTY > dtl.VID_GRN_QTY)
                        {
                            dtl.VID_QTY_INVOICED = 0;
                        }
                        else
                        {
                            dtl.VID_QTY_INVOICED = dtl.VID_GRN_QTY - dtl.VID_INV_QTY;
                        }

                        dtl.TaxDtl.ForEach(tdl =>
                        {
                            if (tdl.VTL_TYPE == 1)
                                tdl.VTL_TAX_AMT = 0;
                            else
                            {
                                tdl.VTL_TAX_AMT = tdl.VTL_TAX_AMT / dtl.VID_GRN_QTY * dtl.VID_QTY_INVOICED;
                            }
                        });
                    }
                    else
                    {
                        if (dtl.VID_INV_QTY > dtl.VID_ORDERED_QTY)
                        {
                            dtl.VID_QTY_INVOICED = 0;
                        }
                        else
                        {
                            dtl.VID_QTY_INVOICED = dtl.VID_ORDERED_QTY - dtl.VID_INV_QTY;
                        }
                        dtl.TaxDtl.ForEach(tdl =>
                        {
                            if (tdl.VTL_TYPE == 1)
                                tdl.VTL_TAX_AMT = 0;
                            else
                            {
                                tdl.VTL_TAX_AMT = tdl.VTL_TAX_AMT / dtl.VID_ORDERED_QTY * dtl.VID_QTY_INVOICED;
                            }
                        });
                    }
                }
                //POInvoiceHeaderSession.TaxHdr.ForEach(thd => thd.VTL_TAX_AMT = 0);
                //14-05-2014 By Juno
                //POInvoiceHeaderSession.TaxHdr.ForEach(thd =>
                //{
                //    thd.VTL_TAX_AMT = 0;
                //});
            }
            SetFieldValues(ControlsEnum.POINVHEADER);
            SetFieldValues(ControlsEnum.POINVDETAIL);
            SetFieldValues(ControlsEnum.UPLOADEDFILES);
            SetFieldValues(ControlsEnum.GRNATTACHMENTS);
            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
            hdfApplyTax.Value = "0";
            ReqDept = 0;
            int.TryParse(hdfSubDeptPk.Value, out ReqDept);
            ucrWrkf.ReqDeptID = (PIType == "21") ? ReqDept : 0;
            ucrWrkf.FillWorkFlowDetails();
            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                ucrWrkf.ViewType = 1;
            else
            {
                ucrWrkf.ViewType = 0;
                //EntryStatus = EntryStatus.VIEWMODE;
            }
            //If Has RefID (from Inbox)
            if (!string.IsNullOrEmpty(refID))
            {
                hdfApplyTax.Value = "1"; // From Inbox no need to create formula for Tax amount. 
            }
            SetDetailTax(null);
            SetHdrTax();
            if (grdInvoice.Rows.Count > 0)
            {
                SetSubTotal();
            }
            //Settings of TaxPayableDiv
            if (hdfIsTaxPayable.Value.ToString() == "1")
            {
                SetTaxPayableDiv();
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
            //end
        }


        /// <summary>
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetCurrencyConfiguration("CURRENCY SETTINGS", "ShowAmountInBC", currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfIsTaxPayable.Value = dt.Rows[0]["ACF_VALUE"].ToString();//If 1 Show Div taxpayable else hide                           
            }
            //Edit Option Configuration of Other Charges and tax in AdvanceInvoice Allocation Popup           
            DataTable dtEditTaxOtherCharge = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("PURCHASE ADV DED", "EDIT TAX");
            if (dtEditTaxOtherCharge != null && dtEditTaxOtherCharge.Rows.Count > 0)
            {
                hdfIsTaxOCEditable.Value = dtEditTaxOtherCharge.Rows[0]["ACF_VALUE"].ToString();
            }

            // for line item tax & discount
            dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("PURCHASE ITEM WISE SETTINGS", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfisTaxAdd.Value = dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "TAX")["ACF_VALUE"].ToString();
                hdfisDiscountAdd.Value = dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "DISCOUNT")["ACF_VALUE"].ToString();
            }

            IsShowEffRateInPOInvoice = GetGlobalResourceObject("ConfigurationsRes", "IsShowEffRateInPOInvoice").ToString() == "1" ? true : false;
            hdfEnableAddlOtherCharge.Value = GetGlobalResourceObject("ConfigurationsRes", "EnableInvoiceOtherCharges").ToString();

            #region Advance Invoice Tax Settings
            DataTable dtTax = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("ADVANCE INVOICE SETTINGS", "TAX");
            if (dtTax != null && dtTax.Rows.Count > 0)
            {
                IsAdvInvHasTax = dtTax.Rows[0]["ACF_VALUE"].ToString() == "1" ? true : false;
                hdfIsAdvHasTax.Value = dtTax.Rows[0]["ACF_VALUE"].ToString();
            }
            #endregion

            // OtherCharge is needed for Tax Calculation            
            IsTaxForOtherCharge.Value = (GetGlobalResourceObject("ConfigurationsRes", "OtherChargeTaxPurchase")).ToString();

            hdfShowEffRateInPI.Value = (GetGlobalResourceObject("ConfigurationsRes", "ShowEffRateInPI")).ToString();
            

            hdfShowInvestor.Value = (GetGlobalResourceObject("ConfigurationsRes", "ShowInvestor")).ToString();


            if (GetGlobalResourceObject("ConfigurationsRes", "ShowInvestor").ToString() == "1")
            {
                divInvestor.Visible = true;
            }
            else
            {
                divInvestor.Visible = false;
            }

            hdfIsShowAlert.Value = GetGlobalResourceObject("ConfigurationsRes", "IsShowAlertInPO").ToString();//For Setting Visibilty of Alert Button w. r. to client

            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";

            hdfShowTransactionPort.Value = GetGlobalResourceObject("ConfigurationsRes", "ShowTransactionPort").ToString();
            hdfInvDueDateDependsVenInvDate.Value = GetGlobalResourceObject("ConfigurationsRes", "InvDueDateDependsVendorInvDate").ToString();   //1=>Inv. Due Date calculation depends upon Vendor Inv. Date,Otherwise Invoice date
            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "VENDOR");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsSBUVendor.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
            }

            if (GetGlobalResourceObject("ConfigurationsRes", "ShowPOInvoicePOType").ToString() == "1")
            {
                //divType.Visible = true;
                lblPOType.Visible = true;
                ddlPOType.Visible = true;
            }

            hdfInvoiceAssetTypeRequired.Value = GetGlobalResourceObject("ConfigurationsRes", "InvoiceAssetTypeRequired").ToString();
        }
        private void SetControlsVisibility()
        {
            if (IsAdvInvHasTax)
            {
                trDeductionTax.Visible = true;
                trDiscDeductedTax.Visible = true;
                trDeductedOtherChargesTax.Visible = true;

                trTotal.Visible = false;
                trTotalDeduction.Visible = false;
            }
            else
            {
                trDeductionTax.Visible = false;
                trDiscDeductedTax.Visible = false;
                trDeductedOtherChargesTax.Visible = false;

                trTotal.Visible = true;
                trTotalDeduction.Visible = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="multiplePO"></param>
        /// <returns></returns>
        private POInvoiceHeader GetMultiplePODetails(MultiplePOInvoiceHeader multiplePO)
        {
            List<POInvoiceDetails> pOInvoiceDetailsLst = new List<POInvoiceDetails>();
            POInvoiceDetails objPOInvoiceDetails;
            List<POInvoiceTaxHdr> pOInvoiceTaxHdrLst = new List<POInvoiceTaxHdr>();
            POInvoiceTaxHdr objPOInvoiceTaxHdr;
            List<POAdvDeductionDetails> pOAdvDeductionDetailsLst = new List<POAdvDeductionDetails>();
            POAdvDeductionDetails objPOAdvDeductionDetails;
            List<POInvoiceUploads> pOInvoiceUploadsLst = new List<POInvoiceUploads>();
            POInvoiceUploads objPOInvoiceUploads;
            List<POOtherChargeDetails> pOOtherChargeDetailsLst = new List<POOtherChargeDetails>();
            POOtherChargeDetails objPOOtherChargeDetails;
            POInvoiceHeader objPOInvoiceHeaderMultiple = new POInvoiceHeader();

            if (multiplePO != null && multiplePO.MultiplePOList != null && multiplePO.MultiplePOList.Count > 0)
            {
                objPOInvoiceHeaderMultiple = multiplePO.MultiplePOList[0].DeepClone();
                foreach (POInvoiceHeader objPO in multiplePO.MultiplePOList)
                {
                    #region POInvoiceDetails
                    foreach (POInvoiceDetails tDtl in objPO.OrderDetail)
                    {
                        objPOInvoiceDetails = new POInvoiceDetails();
                        objPOInvoiceDetails = tDtl.DeepClone();
                        pOInvoiceDetailsLst.Add(objPOInvoiceDetails);
                    }
                    #endregion

                    #region POInvoiceTaxHdr
                    foreach (POInvoiceTaxHdr tTax in objPO.TaxHdr)
                    {
                        objPOInvoiceTaxHdr = new POInvoiceTaxHdr();
                        objPOInvoiceTaxHdr = tTax.DeepClone();
                        pOInvoiceTaxHdrLst.Add(objPOInvoiceTaxHdr);
                    }

                    #endregion

                    #region POAdvDeductionDetails
                    foreach (POAdvDeductionDetails tAdv in objPO.DeductionDetails)
                    {
                        objPOAdvDeductionDetails = new POAdvDeductionDetails();
                        objPOAdvDeductionDetails = tAdv.DeepClone();
                        pOAdvDeductionDetailsLst.Add(objPOAdvDeductionDetails);
                    }
                    #endregion

                    #region POInvoiceUploads
                    foreach (POInvoiceUploads tUp in objPO.FileList)
                    {
                        objPOInvoiceUploads = new POInvoiceUploads();
                        objPOInvoiceUploads = tUp.DeepClone();
                        pOInvoiceUploadsLst.Add(objPOInvoiceUploads);
                    }
                    #endregion

                    #region POOtherChargeDetails
                    foreach (POOtherChargeDetails tOthr in objPO.OtherChargeDetails)
                    {
                        objPOOtherChargeDetails = new POOtherChargeDetails();
                        objPOOtherChargeDetails = tOthr.DeepClone();
                        pOOtherChargeDetailsLst.Add(objPOOtherChargeDetails);
                    }

                    #endregion
                }
                //Group tax/discount 
                if (pOInvoiceTaxHdrLst.Count > 0)
                {
                    var groupedTaxHdrList = pOInvoiceTaxHdrLst.GroupBy(f => new { f.VTL_TAX, f.VTL_TAX_CATEGORY })
                    .Select(grp => new POInvoiceTaxHdr
                    {
                        VTL_INVOICE_DTL = grp.Min(p => p.VTL_INVOICE_DTL),
                        VTL_TAX_AMT = grp.Sum(p => p.VTL_TAX_AMT),
                        VTL_NAME = grp.Min(p => p.VTL_NAME),
                        VTL_TAX_TEXT = grp.Min(p => p.VTL_TAX_TEXT),
                        VTL_PK = grp.Min(p => p.VTL_PK),
                        VTL_TAX_CODE = grp.Min(p => p.VTL_TAX_CODE),
                        VTL_TAX_RATE = grp.Min(p => p.VTL_TAX_RATE),
                        VTL_TAX_VID_AMOUNT = grp.Sum(p => Convert.ToDecimal(p.VTL_TAX_VID_AMOUNT)).ToString(),
                        VTL_PO_DTL = grp.Min(p => p.VTL_PO_DTL),
                        VTL_SL_NO = grp.Min(p => p.VTL_SL_NO),
                        VTL_TAX = grp.Min(p => p.VTL_TAX),
                        VTL_TAX_CATEGORY = grp.Min(p => p.VTL_TAX_CATEGORY),
                        VTL_TAX_CATEGORY_TEXT = grp.Min(p => p.VTL_TAX_CATEGORY_TEXT),
                        VTL_TAX_FORMULA = grp.Min(p => p.VTL_TAX_FORMULA),
                        VTL_TYPE = grp.Min(p => p.VTL_TYPE),
                        VTL_HAS_SUB_TOTAL = grp.Min(p => p.VTL_HAS_SUB_TOTAL),
                        VTL_HAS_DISCOUNT = grp.Min(p => p.VTL_HAS_DISCOUNT),
                        VTL_HAS_OTHER_CHARGE = grp.Min(p => p.VTL_HAS_OTHER_CHARGE),
                    })
                   .ToList();
                    pOInvoiceTaxHdrLst = groupedTaxHdrList;
                }



                //List<POOtherChargeDetails> objGroupLst = multiplePO.MultiplePOList. othrLst.GroupBy(ch => ch.IVM_PO_HDR).Select(grp => new POOtherChargeDetails
                //{
                //    IVM_ACTIVE = grp.FirstOrDefault().IVM_ACTIVE,
                //    IVM_ADJUST_AMOUNT = grp.Sum(r => r.IVM_ADJUST_AMOUNT),
                //    IVM_AMOUNT = grp.Sum(r => r.IVM_AMOUNT),
                //    IVM_DISCOUNT_AMOUNT = grp.Sum(r => r.IVM_DISCOUNT_AMOUNT),
                //    IVM_OTHER_AMOUNT = grp.Sum(r => r.IVM_OTHER_AMOUNT),
                //    IVM_PK = grp.FirstOrDefault().IVM_PK,
                //    IVM_PO_HDR = grp.FirstOrDefault().IVM_PO_HDR,
                //    IVM_TAX_AMOUNT = grp.Sum(r => r.IVM_TAX_AMOUNT),
                //    PO_OTHER_AMOUNT = grp.Sum(r => r.PO_OTHER_AMOUNT),
                //    PO_OTHER_AMOUNT_INVOICED = grp.Sum(r => r.PO_OTHER_AMOUNT_INVOICED),
                //    POH_DATE = grp.FirstOrDefault().POH_DATE,
                //    POH_NO = grp.FirstOrDefault().POH_NO,
                //}).ToList();

                if (pOOtherChargeDetailsLst != null && pOOtherChargeDetailsLst.Count > 0)
                {
                    List<POOtherChargeDetails> OthrChrgeLst = new List<POOtherChargeDetails>();
                    foreach (POOtherChargeDetails lst in pOOtherChargeDetailsLst)
                    {
                        POInvoiceHeader poLst = multiplePO.MultiplePOList.SingleOrDefault(p => p.IVH_ORDER == lst.IVM_PO_HDR.ToString());
                        if (poLst != null)
                        {
                            lst.IVM_TAX_AMOUNT = poLst.TaxHdr.Where(x => x.VTL_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(t => t.VTL_TAX_AMT);
                            lst.IVM_DISCOUNT_AMOUNT = poLst.TaxHdr.Where(x => x.VTL_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(d => d.VTL_TAX_AMT);
                            lst.IVM_AMOUNT = poLst.OrderDetail.Sum(amt => amt.VID_AMOUNT);
                        }
                        lst.IVM_OTHER_AMOUNT = lst.IVM_PK > 0 ? lst.IVM_OTHER_AMOUNT : lst.PO_OTHER_AMOUNT - lst.PO_OTHER_AMOUNT_INVOICED;
                        lst.IVM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);


                        OthrChrgeLst.Add(lst);
                    }
                    pOOtherChargeDetailsLst = OthrChrgeLst;
                }

                //Setting this for calculating custom tax/ discount/ othercharges in case of changing Inv Now Quantity
                PrevSubTotalPOAmount = objPOInvoiceHeaderMultiple.OrderDetail.Sum(tot => tot.VID_NET_AMOUNT);
                PrevTotalPOAmountDiscount = Convert.ToDouble(objPOInvoiceHeaderMultiple.POH_DISCOUNT_TC);//IVH_DISCOUNT_TC;
                POTotalTaxAmount = pOInvoiceTaxHdrLst.Where(res => res.VTL_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(res => res.VTL_TAX_AMT);

                objPOInvoiceHeaderMultiple.POH_PRICE_ADJUST_TOTAL = multiplePO.MultiplePOList.Sum(x => x.POH_PRICE_ADJUST);
                objPOInvoiceHeaderMultiple.POH_TOTAL_VALUE_TC = multiplePO.MultiplePOList.Sum(x => x.POH_TOTAL_VALUE);
                objPOInvoiceHeaderMultiple.POH_DISC_AMT_TOTAL = multiplePO.MultiplePOList.Sum(x => x.POH_DISC_AMT);

                objPOInvoiceHeaderMultiple.OrderDetail = pOInvoiceDetailsLst;
                objPOInvoiceHeaderMultiple.TaxHdr = pOInvoiceTaxHdrLst;
                objPOInvoiceHeaderMultiple.FileList = pOInvoiceUploadsLst;
                objPOInvoiceHeaderMultiple.DeductionDetails = pOAdvDeductionDetailsLst;
                objPOInvoiceHeaderMultiple.OtherChargeDetails = pOOtherChargeDetailsLst;
                objPOInvoiceHeaderMultiple.DeepClone();
            }
            return objPOInvoiceHeaderMultiple;
        }

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            AdmCompanyMstService admCompanyMstServiceClient;

            DataSet dsTaxDetails;
            FinTrxService finTrxServiceClient;
            ServiceUtility serviceUtilityObj;
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            POInvoiceService poInvoiceServiceClient;
            poInvoiceServiceClient = null;
            int group = 0;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    case ControlsEnum.POINVHEADER:

                        #region Old code to handle only for single PO
                        //invoiceHeaderObj = BusinessLogic.POInvoicing.POInvoiceBL.GetPOInvoiceHeader(CurrPK > 0 ? 0 : CurrPOPK, CurrPK);
                        //invoiceHeaderTemp = BusinessLogic.POInvoicing.POInvoiceBL.GetPOInvoiceHeader(CurrPK > 0 ? 0 : CurrPOPK, CurrPK);
                        //TempInvoiceHeaderTemp = invoiceHeaderTemp;
                        //POInvoiceHeaderSession = invoiceHeaderObj;
                        //POInvoiceHdrSession = invoiceHeaderObj;
                        //if (invoiceHeaderObj == null && CurrPK != 0)
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "');", true);
                        //}
                        #endregion

                        // To handle single/multiple PO
                        string xmlDoc = CommonFunctions.XmlSerialize<POHeaderBO>(InvoiceMultiplePOPKs);
                        multipleinvoiceHeaderObj = BusinessLogic.POInvoicing.POInvoiceBL.GetMultiplePOInvoiceHeader(CurrPK > 0 ? 0 : CurrPOPK, CurrPK, xmlDoc);
                        invoiceHeaderObj = GetMultiplePODetails(multipleinvoiceHeaderObj);
                        invoiceHeaderTemp = invoiceHeaderObj;
                        TempInvoiceHeaderTemp = invoiceHeaderTemp;
                        POInvoiceHeaderSession = invoiceHeaderObj;
                        POInvoiceHdrSession = invoiceHeaderObj;
                        if (invoiceHeaderObj == null && CurrPK != 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "');", true);
                        }
                        break;
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
                                if ((byte)POGroup == (byte)POInvoiceGroup.Services)
                                {
                                    dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtInvoiceDate.Text), 0, TaxFilterType.PUR, (int)TaxStatus.Only, 1, 0);
                                }
                                else
                                {
                                    if (GetGlobalResourceObject("ConfigurationsRes", "IsTaxNotDueForMaterialPO").ToString() == "1")//If 0 exclude Tax not due from tax poup ddl.If 1 include
                                    {
                                        dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtInvoiceDate.Text), 0, TaxFilterType.PUR, (int)TaxStatus.Include, 1, 0);
                                    }
                                    else
                                    {
                                        dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtInvoiceDate.Text), 0, TaxFilterType.PUR, (int)TaxStatus.Exclude, 1, 0);
                                    }
                                }
                            }
                            else
                            {
                                dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtInvoiceDate.Text), 0);
                            }
                        }
                        break;
                    #region Invoice Hdr By PK
                    case ControlsEnum.LINETAX:
                        dtTaxDetData = BusinessLogic.POInvoicing.POInvoiceBL.GetLineitemTaxList(PaymentMpgPK).Tables[0];//ReceiptMpgPK);
                        break;
                    #endregion

                    case ControlsEnum.EXCHANGERATE:
                        DataSet dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(Convert.ToInt32(hdfCurrency.Value), currentUser.BaseCurrency, Convert.ToDateTime(txtInvoiceDate.Text.Trim()));
                        if (dsExchangeRate != null && dsExchangeRate.Tables[0].Rows.Count > 0)
                        {
                            hdfExchangeRate.Value = dsExchangeRate.Tables[0].Rows[0][0].ToString();
                            txtExchangeRate.Text = Convert.ToDouble(dsExchangeRate.Tables[0].Rows[0][0]).ToString();
                        }
                        else
                        {
                            hdfExchangeRate.Value = "-1";
                            txtExchangeRate.Text = "";
                        }

                        break;
                    case ControlsEnum.INVOICEGET:
                        int GInvPk = Convert.ToInt32(Request.QueryString[QueryStrings.PK]);
                        group = CrDrFlag == 1 || PaymentFlag == 1 ? 1 : 0;
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
                                SearchValue = string.IsNullOrEmpty(txtInvoiceNumber.Text.Trim()) ? string.Empty : (txtInvoiceNumber.Text.Trim() == "Select/Type" ? string.Empty : txtInvoiceNumber.Text.Trim())
                            }, currentUser, 0, GInvPk, CurrPOPK, string.Empty, txtpoNo.Text
                            , Resources.PageURL.PurchaseOrderInvoicing.Replace("~", "")
                            , (ddlOrderType.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlOrderType.SelectedValue) : 0)
                            , 0
                            , group == 0 ? (PIType == "21" ? (int)POInvoiceGroup.Services : (int)POInvoiceGroup.Goods) : 0, (byte)POInvoiceCategory.Invoice
                            , chkPending.Checked == true ? (byte)1 : (byte)0
                            , string.IsNullOrEmpty(txtGrnNo.Text.Trim()) ? null : txtGrnNo.Text.Trim()
                            , string.IsNullOrEmpty(txtDueAson.Text.Trim()) ? string.Empty : txtDueAson.Text.Trim());
                        if (dsPageData != null)
                        {
                            DataView dvInvoice = dsPageData.Tables[1].DefaultView;
                            dtInvoiceList = dvInvoice.ToTable();
                        }
                        break;
                    case ControlsEnum.INVOICELIST:
                        int cusID = String.IsNullOrEmpty(hdfCustomerID.Value.Trim()) ? 0 : Convert.ToInt32(hdfCustomerID.Value);
                        group = CrDrFlag == 1 || PaymentFlag == 1 ? 1 : 0;
                        //PO invoice list is not getting filtered based on choosen MENU Credit/Debit note(Vendor) and Purchase Invoice (Service)
                        if (PIType == "21")
                        {
                            group = 0;
                        }
                        if (hdfCustomerID.Value != "" && hdfCustomerID.Value != "0")
                        {
                            Session[ERP.Utilities.SessionStrings.VendorPK] = hdfCustomerID.Value;
                            Session[ERP.Utilities.SessionStrings.Vendor] = txtCustomer.Text;
                        }
                        int InvPk = String.IsNullOrEmpty(hdfIVHPK.Value.Trim()) ? 0 : Convert.ToInt32(hdfIVHPK.Value);
                        int Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        int cmpPk = Convert.ToInt32(ddlCompanySrch.SelectedValue);
                        string customer = string.IsNullOrEmpty(txtCustomer.Text.Trim()) ? string.Empty : (txtCustomer.Text.Trim() == "Select/Type" ? string.Empty : txtCustomer.Text.Trim());

                        group = (group == 0 ? (PIType == "21" ? (int)POInvoiceGroup.Services : (int)POInvoiceGroup.Goods) : 0);
                        if (GetGlobalResourceObject("ConfigurationsRes", "EnableWorkOrderItem").ToString() == "1")
                            if (PIType == "1" || PIType == "2")
                                group = 0;
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
                            SearchValue = string.IsNullOrEmpty(txtInvoiceNumber.Text.Trim()) ? string.Empty : (txtInvoiceNumber.Text.Trim() == "Select/Type" ? string.Empty : txtInvoiceNumber.Text.Trim()),
                            POType = Convert.ToInt32(ddlPOType.SelectedValue) > 0 ? Convert.ToInt32(ddlPOType.SelectedValue) : 0,
                            ConvertTo = Convert.ToInt32(ddlConvertTo.SelectedValue),
                            SCNo = txtSCNo.Text
                        }, currentUser, cusID, InvPk, CurrPOPK, customer, txtpoNo.Text
                        , Resources.PageURL.PurchaseOrderInvoicing.Replace("~", "")
                        , (ddlOrderType.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlOrderType.SelectedValue) : 0)
                        , Status
                        //, group == 0 ? (PIType == "21" ? (int)POInvoiceGroup.Services : (int)POInvoiceGroup.Goods) : 0
                        , group
                        , (byte)POInvoiceCategory.Invoice
                        , chkPending.Checked == true ? (byte)1 : (byte)0
                        , string.IsNullOrEmpty(txtGrnNo.Text.Trim()) ? null : txtGrnNo.Text.Trim()
                        , string.IsNullOrEmpty(txtDueAson.Text.Trim()) ? string.Empty : txtDueAson.Text.Trim()
                        , cmpPk);

                        if (dsPageData != null)
                        {
                            //string customer = string.IsNullOrEmpty(txtCustomer.Text.Trim()) ? string.Empty : (txtCustomer.Text.Trim() == "Select/Type" ? string.Empty : txtCustomer.Text.Trim());
                            DataView dvInvoice = dsPageData.Tables[1].DefaultView;
                            //string Fillter = string.Empty;
                            //if (customer != string.Empty)
                            //{
                            //    Fillter = Fillter + " IVH_VENDOR_TEXT like '%" + customer + "%'";
                            //}
                            //if (txtpoNo.Text != string.Empty)
                            //{

                            //    Fillter = Fillter != string.Empty ? Fillter + " AND IVH_PO_NO like '%" + txtpoNo.Text + "%'" : Fillter + " IVH_PO_NO like '%" + txtpoNo.Text + "%'"; ;
                            //}
                            //dvInvoice.RowFilter = Fillter;
                            dtInvoiceList = dvInvoice.ToTable();
                        }

                        break;

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
                    case ControlsEnum.POTYPE:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PURCHASE INVOICE TYPE");
                        break;
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
                    case ControlsEnum.ADVANCEDTAXSETTINGS:
                        dtTaxSettings = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("ADVANCE INVOICE SETTINGS", string.Empty, currentUser.SBUID);
                        if (dtTaxSettings != null && dtTaxSettings.Rows.Count > 0)
                        {
                            DataRow drTaxSettings = dtTaxSettings.AsEnumerable().SingleOrDefault(aa => aa.Field<string>("ACF_DATA").Trim() == "TAX");
                            if (drTaxSettings != null)
                            {
                                hdfTaxSettings.Value = drTaxSettings["ACF_VALUE"].ToString();
                            }
                        }
                        break;

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
                        ////gets Company List
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

                    #region OTHERCHARGELIST
                    case ControlsEnum.OTHERCHARGELIST:
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

                    #region POLIST
                    case ControlsEnum.POLIST:
                        int VenorPk = String.IsNullOrEmpty(hdfCustomerID.Value.Trim()) ? 0 : Convert.ToInt32(hdfCustomerID.Value);
                        POHeaderBO POHdr = new POHeaderBO();
                        List<POHeaderListBO> POList = new List<POHeaderListBO>();
                        foreach (GridViewRow grvRow in grdInvoice.Rows)
                        {
                            HiddenField hdfPOPK = (HiddenField)grvRow.FindControl("hdfSODtlPK");//POH_PK
                            POHeaderListBO POObj = new POHeaderListBO();
                            POObj.POPK = string.IsNullOrEmpty(hdfPOPK.Value) ? 0 : Convert.ToInt32(hdfPOPK.Value);
                            if (POList.Where(f => f.POPK == POObj.POPK).Count() == 0)//For Distinct
                                POList.Add(POObj);
                        }
                        POHdr.POList = POList;
                        string xmlDocPO = CommonFunctions.XmlSerialize<POHeaderBO>(POHdr);
                        int pohGroup = (PIType == "21") ? (int)BusinessObject.CommonManagement.POGroup.Services : 0;
                        dsPOList = BusinessLogic.POInvoicing.POInvoiceBL.GetPOList(currentUser, xmlDocPO, pohGroup);// Fetching pending PO except currently selected PO's.
                        if (dsPOList != null && dsPOList.Tables.Count > 0)
                        {
                            DataView dvPO = dsPOList.Tables[0].DefaultView;
                            dtPOList = dvPO.ToTable();
                        }

                        break;
                    #endregion

                    #region Getting Newly Adding PO Details w. r. to. MultiplePOPKs
                    case ControlsEnum.POINVHEADERNEW:
                        string xmlDocNew = CommonFunctions.XmlSerialize<POHeaderBO>(InvoiceMultiplePOPKsNew);
                        multipleinvoiceHeaderObjNew = new MultiplePOInvoiceHeader();
                        multipleinvoiceHeaderObjNew = BusinessLogic.POInvoicing.POInvoiceBL.GetMultiplePOInvoiceHeader(0, 0, xmlDocNew);
                        if (multipleinvoiceHeaderObjNew != null)
                        {
                            invoiceHeaderObjNew = GetMultiplePODetails(multipleinvoiceHeaderObjNew);
                            #region Setting Invoice Now Qty By Default
                            foreach (POInvoiceDetails dtl in invoiceHeaderObjNew.OrderDetail)
                            {
                                dtl.VID_PK = 0;
                                dtl.VID_INVOICE_HDR = CurrPK;
                                dtl.VID_QTY_INVOICED = 0;
                                dtl.VID_AMOUNT = 0;
                                dtl.VID_TAX = 0;
                                dtl.VID_DISCOUNT = 0;
                                dtl.VID_NET_AMOUNT = 0;

                                if (Convert.ToBoolean(dtl.VID_HAS_GRN))
                                {
                                    if (dtl.VID_INV_QTY > dtl.VID_GRN_QTY)
                                    {
                                        dtl.VID_QTY_INVOICED = 0;
                                    }
                                    else
                                    {
                                        dtl.VID_QTY_INVOICED = dtl.VID_GRN_QTY - dtl.VID_INV_QTY;
                                    }
                                    dtl.TaxDtl.ForEach(tdl =>
                                    {
                                        if (tdl.VTL_TYPE == 1)
                                            tdl.VTL_TAX_AMT = 0;
                                        else
                                        {
                                            tdl.VTL_TAX_AMT = tdl.VTL_TAX_AMT / dtl.VID_GRN_QTY * dtl.VID_QTY_INVOICED;
                                        }
                                    });
                                }
                                else
                                {
                                    if (dtl.VID_INV_QTY > dtl.VID_ORDERED_QTY)
                                    {
                                        dtl.VID_QTY_INVOICED = 0;
                                    }
                                    else
                                    {
                                        dtl.VID_QTY_INVOICED = dtl.VID_ORDERED_QTY - dtl.VID_INV_QTY;
                                    }
                                    dtl.TaxDtl.ForEach(tdl =>
                                    {
                                        if (tdl.VTL_TYPE == 1)
                                            tdl.VTL_TAX_AMT = 0;
                                        else
                                        {
                                            tdl.VTL_TAX_AMT = tdl.VTL_TAX_AMT / dtl.VID_ORDERED_QTY * dtl.VID_QTY_INVOICED;
                                        }
                                    });
                                }

                            }
                            #endregion
                            invoiceHeaderObjNew.OrderDetail.ForEach(dtl => POInvoiceHeaderSession.OrderDetail.Add(dtl));//Assigning New PODetails to Current Object  
                            invoiceHeaderObjNew.OtherChargeDetails.ForEach(dtl => POInvoiceHeaderSession.OtherChargeDetails.Add(dtl));//Assigning New OtherChargeDetails to Current Object  
                            invoiceHeaderObjNew.DeductionDetails.ForEach(dtl => POInvoiceHeaderSession.DeductionDetails.Add(dtl));//Assigning New PO DeductionDetails to Current Object   
                            invoiceHeaderObj = POInvoiceHeaderSession;
                        }
                        break;
                    #endregion

                    #region
                    case ControlsEnum.GSTINVOICETYPE:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetInvoiceGstType(Convert.ToInt16(CommonConstants.SELECT_VALUE_ZERO), Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID, Convert.ToInt16(CommonConstants.SELECTVAL), Convert.ToInt16(GTIService.Constants.Common.InvoiceType.Purchase));
                        break;
                    #endregion

                    #region PO Cost Center details
                    case ControlsEnum.COSTCENTERDTL:
                        dtPageData = BusinessLogic.POInvoicing.POInvoiceBL.GetPOCostCenterDetails(POdtlPK);
                        break;
                    #endregion

                    #region PURCHASEORDERTYPE
                    case ControlsEnum.PURCHASEORDERTYPE:
                        dtPageData = BusinessLogic.POInvoicing.POInvoiceBL.GetPOTypes(1, 0, "PURCHASE TYPE");
                        break;
                    #endregion
                    #region ASSETTYPE
                    case ControlsEnum.ASSETTYPE:
                        GridPrams gridParamObj = new GridPrams();
                        gridParamObj.PageNumber = 1;
                        gridParamObj.PageSize = 100;
                        gridParamObj.SearchBy = Resources.DataFieldRes.AssetTypeCode;
                        gridParamObj.SearchValue = string.Empty;
                        gridParamObj.SortBy = Resources.DataFieldRes.AssetTypeCode;
                        gridParamObj.SortDirection = Resources.Report.SortAscending;

                        int atpPK = 0;
                        int atpBizUnit = Convert.ToInt16(currentUser.CurrentSBUPK);
                        int atpDept = 8;// currentUser.CurrentDeptPK;
                        int atpActive = Convert.ToByte(DbActiveStatus.ACTIVE);
                        DataSet dsAssetType = new DataSet();
                        dsAssetType = BusinessLogic.CommonManagement.CommonBL.GetAssetTypeDetails(gridParamObj, atpPK, atpBizUnit, atpDept, atpActive);
                        dtPageData = dsAssetType.Tables[1];
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
                admCompanyMstServiceClient = null;
                finTrxServiceClient = null;
                CommonServiceClient = null;
                poInvoiceServiceClient = null;

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
                    case ControlsEnum.INVOICEGET:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.VENDORCONTACTYPEDETAILS:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.VENDORCONTACTYPE:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.POINVHEADER:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.POINVDETAIL:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.AMOUNTDETAILS:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.TAXTYPES:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.TAXPOPUPGRID:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.INVOICELIST:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.POTYPE:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.DEDUCTIONPOPUPGRID:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.COMPANY:
                        //Bind Company List in DDL
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    case ControlsEnum.COMPANYSRCH:
                        BindDropDown(ControlsEnum.COMPANYSRCH);
                        break;
                    case ControlsEnum.UPLOADEDFILES:
                        if (invoiceHeaderObj != null)
                            GetUIValuesFromObject(ControlsEnum.UPLOADEDFILES);
                        BindGrid(ControlsEnum.UPLOADEDFILES);
                        break;
                    case ControlsEnum.VENDORBRANCH:
                        BindDropDown(controlType);
                        break;

                    #region LINETAX
                    case ControlsEnum.LINETAX:
                        if (dtTaxDetData != null & dtTaxDetData.Rows.Count > 0)
                        {

                            decimal Totaltax = 0;
                            decimal TotalLineitemtax = 0;
                            decimal TotalDiscount = 0;
                            decimal TotalLineitemDiscount = 0;

                            for (int i = 0; i < dtTaxDetData.Rows.Count; i++)
                            {
                                if (Convert.ToByte(dtTaxDetData.Rows[i]["PDT_TAX_CATEGORY"]) == (byte)TaxType.Tax)
                                {
                                    if (!string.IsNullOrEmpty(dtTaxDetData.Rows[i]["PDT_POD"].ToString()))
                                    {
                                        TotalLineitemtax = TotalLineitemtax + Convert.ToDecimal(dtTaxDetData.Rows[i]["PDT_AMOUNT"]);
                                    }
                                    Totaltax = Totaltax + Convert.ToDecimal(dtTaxDetData.Rows[i]["PDT_AMOUNT"]);
                                }
                            }
                            //Discount
                            HDRDiscount = 0;
                            for (int i = 0; i < dtTaxDetData.Rows.Count; i++)
                            {
                                if (Convert.ToByte(dtTaxDetData.Rows[i]["PDT_TAX_CATEGORY"]) == (byte)TaxType.Discount)
                                {
                                    if (!string.IsNullOrEmpty(dtTaxDetData.Rows[i]["PDT_POD"].ToString()))
                                    {
                                        TotalLineitemDiscount = TotalLineitemDiscount + Convert.ToDecimal(dtTaxDetData.Rows[i]["PDT_AMOUNT"]);
                                    }
                                    else
                                    {
                                        ////while editing invoice with multiple advances allocated(with discount),the balance amount getting -ve and header discount not reduced.
                                        //TotalHDRDiscount = TotalHDRDiscount + Convert.ToDecimal(dtTaxDetData.Rows[i]["PDT_AMOUNT"]);
                                        HDRDiscount = HDRDiscount + Convert.ToDecimal(dtTaxDetData.Rows[i]["PDT_AMOUNT"]);

                                    }
                                    TotalDiscount = TotalDiscount + Convert.ToDecimal(dtTaxDetData.Rows[i]["PDT_AMOUNT"]);
                                }
                            }


                            decimal TaxApplcableinLine = 0;
                            decimal DiscountApplcableinLine = 0;
                            if (Totaltax > 0)
                            {
                                TaxApplcableinLine = (TotalLineitemtax / Totaltax) * totalAllocatedTax;//Allocate now : In deduction popup
                            }
                            if (TotalDiscount > 0)
                            {
                                DiscountApplcableinLine = (TotalLineitemDiscount / TotalDiscount) * totalAllocatedDiscount;//Allocate now : In deduction popup
                            }


                            invoiceHeaderObj = POInvoiceHeaderSession;

                            //if (TempInvoiceHeaderTemp != null)
                            //{
                            //    soInvoiceDetailsList = GetPrevOrderDetails();
                            //}

                            if (TaxApplcableinLine > 0 || DiscountApplcableinLine > 0)
                            {

                                //GetFieldValues(ControlsEnum.SOINVHEADER);

                                if (invoiceHeaderObj != null)
                                {
                                    soInvoiceDetailsList = new List<POInvoiceDetails>();
                                    //if (EntryStatus == EntryStatus.NEWMODE)
                                    //{
                                    //    soInvoiceDetailsList = invoiceHeaderObj.OrderDetail.Where(sod => sod.CID_QTY_DISPATCHED > 0).ToList();//To hide items without despatched and shipping qty
                                    //}
                                    //elsea
                                    //{
                                    soInvoiceDetailsList = invoiceHeaderObj.OrderDetail;//.Where(sod => sod.CID_QTY_DISPATCHED > 0).ToList();//To hide items without despatched and shipping qty
                                    //}
                                }

                                decimal LineitemGridSum = 0;
                                decimal LineitemGridSumDiscount = 0;
                                soInvoiceDetailsList.ForEach(dt => { LineitemGridSum = LineitemGridSum + Convert.ToDecimal(dt.VID_TAX); });
                                soInvoiceDetailsList.ForEach(dt => { LineitemGridSumDiscount = LineitemGridSumDiscount + Convert.ToDecimal(dt.VID_DISCOUNT); });

                                if (soInvoiceDetailsList.Count > 0 || soInvoiceDetailsList != null)
                                {
                                    soInvoiceDetailsList.ForEach(dtl =>
                                    {
                                        double applicableAmtDiscount = 0;
                                        double applicableAmt = 0;
                                        if (LineitemGridSum > 0)
                                        {
                                            applicableAmt = ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(((dtl.VID_TAX / Convert.ToDouble(LineitemGridSum)) * Convert.ToDouble(TaxApplcableinLine))), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                        }
                                        if (LineitemGridSumDiscount > 0)
                                        {
                                            applicableAmtDiscount = ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(((dtl.VID_DISCOUNT / Convert.ToDouble(LineitemGridSumDiscount)) * Convert.ToDouble(DiscountApplcableinLine))), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                        }
                                        double applicableAmtAdjustment = Convert.ToDouble(AmtAdjustPerInvoice);
                                        dtl.TaxDtl.ForEach(dtl2 =>
                                        {
                                            double a = dtl2.VTL_TAX_AMT / dtl.VID_TAX;
                                            double b = a * applicableAmt;
                                            double c = dtl2.VTL_TAX_AMT - b;
                                            //dtl2.VTL_TAX_AMT = dtl2.VTL_TAX_AMT - ((dtl2.VTL_TAX_AMT / dtl.VID_TAX) * applicableAmt);
                                            if (dtl2.VTL_TAX_CATEGORY == (byte)TaxType.Tax && (dtl.VID_TAX > 0))
                                            {
                                                dtl2.VTL_TAX_AMT = ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(dtl2.VTL_TAX_AMT - ((dtl2.VTL_TAX_AMT / dtl.VID_TAX) * applicableAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                            }
                                            else if (dtl2.VTL_TAX_CATEGORY == (byte)TaxType.Discount && (dtl.VID_DISCOUNT > 0))
                                            {
                                                dtl2.VTL_TAX_AMT = ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(dtl2.VTL_TAX_AMT - ((dtl2.VTL_TAX_AMT / dtl.VID_DISCOUNT) * applicableAmtDiscount)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                            }

                                        });
                                        // dtl.VID_TAX = Math.Round(StringToFormula(Convert.ToDouble(dtl.VID_TAX - applicableAmt).ToString()), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                        //dtl.VID_DISCOUNT = Math.Round(StringToFormula(Convert.ToDouble(dtl.VID_DISCOUNT - applicableAmtDiscount).ToString()), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                        // dtl.VID_NET_AMOUNT = Math.Round(StringToFormula(Convert.ToDouble((dtl.VID_AMOUNT - dtl.VID_DISCOUNT) + dtl.VID_TAX).ToString()), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                        dtl.VID_TAX = ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(dtl.VID_TAX - applicableAmt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                        dtl.VID_DISCOUNT = ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(dtl.VID_DISCOUNT - applicableAmtDiscount), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                        dtl.VID_NET_AMOUNT = ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble((dtl.VID_AMOUNT - dtl.VID_DISCOUNT) + dtl.VID_TAX), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                                    });

                                }

                                foreach (GridViewRow grdrow in grdInvoice.Rows)
                                {
                                    HiddenField hdfInvoiceDtlPK;
                                    hdfInvoiceDtlPK = (HiddenField)grdrow.FindControl("hdfInvoiceDtlPK");
                                    TextBox txtInvNow;
                                    txtInvNow = (TextBox)grdrow.FindControl("txtInvNow");

                                    // soInvoiceDetailsObj = soInvoiceDetailsList.SingleOrDefault(dtl => dtl.VID_PK == Convert.ToInt16(hdfInvoiceDtlPK.Value));
                                    //if (soInvoiceDetailsObj != null)
                                    //{
                                    //soInvoiceDetailsObj.VID_QTY_INVOICED =Convert.ToDouble(txtInvNow.Text);
                                    soInvoiceDetailsList.SingleOrDefault(dtl => dtl.VID_PK == Convert.ToInt16(hdfInvoiceDtlPK.Value)).VID_QTY_INVOICED = Convert.ToDouble(txtInvNow.Text);
                                    //  }
                                }
                                invoiceHeaderObj.OrderDetail = soInvoiceDetailsList;
                                if (soInvoiceDetailsList != null)
                                {
                                    grdInvoice.DataSource = invoiceHeaderObj.OrderDetail;
                                    grdInvoice.DataBind();

                                    btnApply.Visible = false;
                                    imgPopupAdd.Visible = false;
                                    grdTaxDetails.Columns[3].Visible = false;
                                }
                                SetSubTotal();


                            }
                        }

                        break;
                    #endregion

                    #region OTHERCHARGELIST
                    case ControlsEnum.OTHERCHARGELIST:
                        BindGrid(ControlsEnum.OTHERCHARGELIST);
                        break;
                    #endregion

                    #region POLIST
                    case ControlsEnum.POLIST:
                        BindGrid(ControlsEnum.POLIST);
                        break;
                    #endregion

                    #region GRNQTYSPLIT
                    case ControlsEnum.GRNQTYSPLIT:
                        BindGrid(ControlsEnum.GRNQTYSPLIT);
                        break;
                    #endregion

                    #region GSTINVOICETYPE
                    case ControlsEnum.GSTINVOICETYPE:
                        BindDropDown(controlType);
                        break;
                    #endregion

                    #region GRNATTACHMENTS
                    case ControlsEnum.GRNATTACHMENTS:
                        if (invoiceHeaderObj != null)
                            GetUIValuesFromObject(ControlsEnum.GRNATTACHMENTS);
                        BindGrid(controlType);
                        break;
                    #endregion

                    #region PO Cost Center Details
                    case ControlsEnum.COSTCENTERDTL:
                        BindGrid(ControlsEnum.COSTCENTERDTL);
                        break;
                    #endregion

                    #region PURCHASEORDERTYPE
                    case ControlsEnum.PURCHASEORDERTYPE:
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
        //private List<POInvoiceDetails> GetPrevOrderDetails()
        //{
        //    List<POInvoiceDetails> lstOrderDetails = new List<POInvoiceDetails>();
        //    if (TempInvoiceHeaderTemp != null)
        //    {
        //        POInvoiceDetails objPOInvoiceDetails;
        //        foreach (POInvoiceDetails OrDetal in TempInvoiceHeaderTemp.OrderDetail)
        //        {
        //            objPOInvoiceDetails = new POInvoiceDetails();
        //            List<POInvoiceTaxHdr> lstPoInvoiceTax = new List<POInvoiceTaxHdr>();
        //            POInvoiceTaxHdr obj;
        //            foreach (POInvoiceTaxHdr objInvTax in OrDetal.TaxDtl)
        //            {
        //                obj = new POInvoiceTaxHdr();
        //                obj.VTL_NAME = objInvTax.VTL_NAME;
        //                obj.VTL_PK = objInvTax.VTL_PK;
        //                obj.VTL_INVOICE_DTL = objInvTax.VTL_INVOICE_DTL;
        //                obj.VTL_PO_DTL = objInvTax.VTL_PO_DTL;
        //                obj.VTL_SL_NO = objInvTax.VTL_SL_NO;
        //                obj.VTL_TAX = objInvTax.VTL_TAX;
        //                obj.VTL_TAX_AMT = objInvTax.VTL_TAX_AMT;
        //                obj.VTL_TAX_CATEGORY = objInvTax.VTL_TAX_CATEGORY;
        //                obj.VTL_TAX_CATEGORY_TEXT = objInvTax.VTL_TAX_CATEGORY_TEXT;
        //                obj.VTL_TAX_CODE = objInvTax.VTL_TAX_CODE;
        //                obj.VTL_TAX_FORMULA = objInvTax.VTL_TAX_FORMULA;
        //                obj.VTL_TAX_RATE = objInvTax.VTL_TAX_RATE;
        //                obj.VTL_TAX_TEXT = objInvTax.VTL_TAX_TEXT;
        //                obj.VTL_TAX_VID_AMOUNT = objInvTax.VTL_TAX_VID_AMOUNT;
        //                obj.VTL_TYPE = objInvTax.VTL_TYPE;
        //                lstPoInvoiceTax.Add(obj);

        //            }
        //            objPOInvoiceDetails.TaxDtl = lstPoInvoiceTax;
        //            objPOInvoiceDetails.VID_AMOUNT = OrDetal.VID_AMOUNT;

        //            objPOInvoiceDetails.VID_BRANCH = OrDetal.VID_BRANCH;
        //            objPOInvoiceDetails.VID_BRANCH_NAME = OrDetal.VID_BRANCH_NAME;
        //            objPOInvoiceDetails.VID_BRANCH_TEXT = OrDetal.VID_BRANCH_TEXT;
        //            objPOInvoiceDetails.VID_BRANCH_TYPE = OrDetal.VID_BRANCH_TYPE;
        //            objPOInvoiceDetails.VID_CIM_PCS_PER_IP = OrDetal.VID_CIM_PCS_PER_IP;
        //            objPOInvoiceDetails.VID_CIM_PCS_PER_OP = OrDetal.VID_CIM_PCS_PER_OP;
        //            objPOInvoiceDetails.VID_CUST_ITEM = OrDetal.VID_CUST_ITEM;
        //            objPOInvoiceDetails.VID_CUST_ITEM_TEXT = OrDetal.VID_CUST_ITEM_TEXT;
        //            objPOInvoiceDetails.VID_DISCOUNT = OrDetal.VID_DISCOUNT;
        //            objPOInvoiceDetails.VID_INSTRUCTIONS = OrDetal.VID_INSTRUCTIONS;
        //            objPOInvoiceDetails.VID_INV_QTY = OrDetal.VID_INV_QTY;

        //            objPOInvoiceDetails.VID_INVOICE_HDR = OrDetal.VID_INVOICE_HDR;
        //            objPOInvoiceDetails.VID_ITEM = OrDetal.VID_ITEM;
        //            objPOInvoiceDetails.VID_ITEM_TEXT = OrDetal.VID_ITEM_TEXT;
        //            objPOInvoiceDetails.VID_NET_AMOUNT = OrDetal.VID_NET_AMOUNT;
        //            objPOInvoiceDetails.VID_ORDERED_QTY = OrDetal.VID_ORDERED_QTY;
        //            objPOInvoiceDetails.VID_PACKING_SPEC = OrDetal.VID_PACKING_SPEC;
        //            objPOInvoiceDetails.VID_PACKING_SPEC_TEXT = OrDetal.VID_PACKING_SPEC_TEXT;

        //            objPOInvoiceDetails.VID_PK = OrDetal.VID_PK;
        //            objPOInvoiceDetails.VID_PO = OrDetal.VID_PO;
        //            objPOInvoiceDetails.VID_PO_DTL = OrDetal.VID_PO_DTL;
        //            objPOInvoiceDetails.VID_QTY_CARTONS = OrDetal.VID_QTY_CARTONS;
        //            objPOInvoiceDetails.VID_QTY_INVOICED = OrDetal.VID_QTY_INVOICED;
        //            objPOInvoiceDetails.VID_RATE = OrDetal.VID_RATE;
        //            objPOInvoiceDetails.VID_REF_DATE = OrDetal.VID_REF_DATE;
        //            objPOInvoiceDetails.VID_REF_NO = OrDetal.VID_REF_NO;
        //            objPOInvoiceDetails.VID_REMARKS = OrDetal.VID_REMARKS;
        //            objPOInvoiceDetails.VID_SL_NO = OrDetal.VID_SL_NO;
        //            objPOInvoiceDetails.VID_TAX = OrDetal.VID_TAX;
        //            objPOInvoiceDetails.VID_TAX_ID = OrDetal.VID_TAX_ID;
        //            objPOInvoiceDetails.VID_UOM = OrDetal.VID_UOM;
        //            objPOInvoiceDetails.VID_UOM_TEXT = OrDetal.VID_UOM_TEXT;
        //            objPOInvoiceDetails.VID_VENDOR = OrDetal.VID_VENDOR;

        //            objPOInvoiceDetails.VID_VENDOR_TEXT = OrDetal.VID_VENDOR_TEXT;
        //            objPOInvoiceDetails.VID_VERSION = OrDetal.VID_VERSION;

        //            lstOrderDetails.Add(objPOInvoiceDetails);
        //        }
        //    }
        //    return lstOrderDetails;

        //}
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
        #region Set BranchCode Visibility
        private void SetBranchCodeVisibility()
        {
            txtBranchCode.Text = string.Empty;
            txtVatTaxId.Text = string.Empty;

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
                        txtBranchCode.Text = "";
                        vrfBranchCode.Enabled = false;
                        //txtBranchCode.CssClass = "input-disabled";
                    }

                    SetFieldValues(ControlsEnum.VENDORCONTACTYPEDETAILS);
                }
            }

        }
        #endregion


        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        //
        private int FillProcessId()
        {
            int procId = 0;
            string path;
            path = "/POInvoicing/PurchaseOrderInvoice.aspx?PID=1";
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                procId = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
            }
            return procId;
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
                DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), currentUser.CurrentDeptPK);
                if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
                {
                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                    ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
                }
            }
            #endregion
        }
        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(AST_CODE.Value, 0, DateTime.Now);
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
            int rowID;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            HiddenField hdfRRDPK;
            HiddenField hdfPOPK;
            HiddenField hdfItemPK;
            HiddenField hdfUoM;
            //HiddenField hdfCurrency;
            HiddenField hdfRFQDtlPK;
            TextBox txtRate;
            TextBox txtAmount;
            TextBox txtDiscount;
            TextBox txtTax;
            TextBox txtTotal;
            DropDownList ddlAssetType;
            CheckBox chkIsAsset;

            TextBox txtSubTotal;
            TextBox txtAdjustAmountFooter;
            List<POInvoiceTaxHdr> rfqTaxHeaderList;

            bool bIsChecked = false;

            int selectedInvoice;
            int selectedCurrency;

            int selectedPOType;
            int selectedVendor;
            int approvedStatus;
            bool isPosted;

            decimal balamt;
            decimal InvoiceValue = 0;

            selectedInvoice = 0;
            approvedStatus = 0;
            selectedVendor = 0;
            selectedPOType = 0;
            selectedCurrency = 0;
            isPosted = false;
            balamt = 0;
            Label lblBalAmt;
            LinkButton lbnBalAmt;
            AlertBO alertBoObj;
            bool InvalidPaymentItem = false;
            //List<POOtherChargeDetails> othrLst;// = new List<POOtherChargeDetails>();

            try
            {
                switch (controlType)
                {
                    #region Inv HDR
                    case ControlsEnum.POINVHEADER:
                        if (POInvoiceHeaderSession != null)
                        {
                            ////Avoiding Duplicate Tax entry(Multiple Entry To FIN_INVOICE_VND_TAX_HDR Table)
                            //var HeaderTaxAll = POInvoiceHeaderSession.TaxHdr;
                            //var HeaderTaxUnique = HeaderTaxAll.Where(tax => tax.VTL_TAX_CATEGORY == ((int)TaxType.Tax)).GroupBy(test => test.VTL_TAX).Select(grp => grp.First()).ToList();
                            //POInvoiceHeaderSession.TaxHdr = HeaderTaxUnique;
                            ////******

                            //To remove zero amount entries from adv deduction list
                            if (POInvoiceHeaderSession.DeductionDetails != null && POInvoiceHeaderSession.DeductionDetails.Count > 0)
                            {
                                List<POAdvDeductionDetails> objDeductionList = POInvoiceHeaderSession.DeductionDetails.Where(r => r.VAD_AMOUNT == 0).ToList();
                                objDeductionList.ForEach(dtl =>
                                {
                                    POInvoiceHeaderSession.DeductionDetails.Remove(dtl);
                                });
                            }
                            invoiceHeaderObj = POInvoiceHeaderSession;
                            invoiceHeaderObj.IVH_PK = CurrPK;
                            //string.IsNullOrEmpty(hdfResponsePK.Value) ? 0 : Convert.ToInt32(hdfResponsePK.Value);
                            invoiceHeaderObj.IVH_NO = invoiceHeaderObj.IVH_PK.ToString();
                            invoiceHeaderObj.IVH_VERSION = 1;
                            invoiceHeaderObj.IVH_STATUS = 0;
                            invoiceHeaderObj.IVH_CATEGORY = (byte)POInvoiceCategory.Invoice;

                            invoiceHeaderObj.IVH_DATE = string.IsNullOrEmpty(txtInvoiceDate.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtInvoiceDate.Text.Trim();
                            //invoiceHeaderObj.IVH_ = CurrPK;
                            invoiceHeaderObj.IVH_DATE_PAY_BY = string.IsNullOrEmpty(txtInvoiceDueDate.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtInvoiceDueDate.Text.Trim();
                            invoiceHeaderObj.IVH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                            invoiceHeaderObj.IVH_SHIP_CHARGE_DED = txtDeductOtherCharges.Text;
                            invoiceHeaderObj.IVH_VENDOR_INV_NO = HttpUtility.HtmlDecode(txtSupplierInvNO.Text);

                            if (Convert.ToInt32(hdfShowInvestor.Value) == 1)
                            {
                                invoiceHeaderObj.IVH_INVESTOR = string.IsNullOrEmpty(txtInvestor.Text.Trim()) ? null : txtInvestor.Text.Trim();
                            }

                            if (EnableGST == 1 && ddlGSTInvoiceType.Items.Count > 0)
                                invoiceHeaderObj.IVH_GST_TYPE = ddlGSTInvoiceType.SelectedValue != CommonConstants.SELECTVAL ? ddlGSTInvoiceType.SelectedValue : string.Empty;


                            if (!string.IsNullOrEmpty(txtFromPort.Text.Trim()) && txtFromPort.Text.Trim() != Resources.ErpRes.AutoDefaultValue.ToString())
                            {
                                invoiceHeaderObj.IVH_FROM_PORT_TEXT = HttpUtility.HtmlEncode(txtFromPort.Text);
                                int fromPortId = 0;
                                int.TryParse(hdfFromPortID.Value, out fromPortId);
                                invoiceHeaderObj.IVH_FROM_PORT = fromPortId > 0 ? fromPortId.ToString() : null;
                            }
                            else
                            {
                                invoiceHeaderObj.IVH_FROM_PORT_TEXT = null;
                                invoiceHeaderObj.IVH_FROM_PORT = null;
                            }
                            int toPortId = 0;
                            int.TryParse(hdfToPortID.Value, out toPortId);
                            invoiceHeaderObj.IVH_TO_PORT = toPortId > 0 ? toPortId.ToString() : null;

                            //do you want to continue with duplicate vendor invoice no
                            if (hdfIsContDupVenInvNo.Value == "1")
                            {
                                invoiceHeaderObj.IVH_ALLOW_DUP_INV_NO = 1; //allow to save Duplicate vendor invoice no.ie,no need for checking if vendor invoice no already exist or not
                            }
                            else
                            {
                                invoiceHeaderObj.IVH_ALLOW_DUP_INV_NO = 0; //check if vendor invoice no already exist or not
                            }

                            //end

                            invoiceHeaderObj.IVH_CREDIT_DAYS = txtCreditDays.Text;
                            invoiceHeaderObj.IVH_TRANSPORT = HttpUtility.HtmlDecode(txtTransport.Text);
                            if (!string.IsNullOrEmpty(txtSupplierInvDate.Text))
                                invoiceHeaderObj.IVH_VENDOR_INV_DATE = txtSupplierInvDate.Text;

                            invoiceHeaderObj.IVH_DISCOUNT_TC = string.IsNullOrEmpty(txtHdrDiscount.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrDiscount.Text.Trim());
                            if (IsAdvInvHasTax)
                            {
                                invoiceHeaderObj.IVH_AMOUNT_ADV_DED_TC = string.IsNullOrEmpty(txtHdrDeduction.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrDeduction.Text.Trim());
                            }
                            else
                            {
                                invoiceHeaderObj.IVH_AMOUNT_ADV_DED_TC = string.IsNullOrEmpty(txtTotalDeduction.Text.Trim()) ? 0 : Convert.ToDouble(txtTotalDeduction.Text.Trim());

                            }
                            invoiceHeaderObj.IVH_TAX_TC = string.IsNullOrEmpty(txtHdrTax.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrTax.Text.Trim());
                            invoiceHeaderObj.IVH_SHIP_CHARGE = string.IsNullOrEmpty(txtShipping.Text.Trim()) ? 0 : Convert.ToDouble(txtShipping.Text.Trim());
                            invoiceHeaderObj.IVH_AMOUNT_ADJUST = string.IsNullOrEmpty(txtPriceAdj.Text.Trim()) ? 0 : Convert.ToDouble(txtPriceAdj.Text.Trim());
                            invoiceHeaderObj.IVH_AMOUNT_NET_TC = string.IsNullOrEmpty(txtHdrNetTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrNetTotal.Text.Trim());
                            invoiceHeaderObj.IVH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                            //GetFieldValues(ControlsEnum.EXCHANGERATE);
                            invoiceHeaderObj.IVH_EXCHG_RATE = string.IsNullOrEmpty(hdfExchangeRate.Value) ? 1 : Convert.ToDouble(hdfExchangeRate.Value);
                            invoiceHeaderObj.IVH_AMOUNT_NET_BC = invoiceHeaderObj.IVH_AMOUNT_NET_TC * invoiceHeaderObj.IVH_EXCHG_RATE;
                            invoiceHeaderObj.IVH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);
                            int branch = 0;
                            if (int.TryParse(ddlVendorBranch.SelectedValue, out branch))
                            {
                                invoiceHeaderObj.IVH_VENDOR_CONTACT = ddlVendorBranch.SelectedValue;
                            }
                            invoiceHeaderObj.IVH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                            invoiceHeaderObj.IVH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                            invoiceHeaderObj.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            invoiceHeaderObj.USER_PK = Convert.ToInt16(currentUser.PKUser);
                            invoiceHeaderObj.LAST_MOD_DT = LastModifiedTime;
                            invoiceHeaderObj.IVH_ORGINAL_RCVD = chkOriginalinvoice.Checked ? (byte)1 : (byte)0;
                            invoiceHeaderObj.IVH_IS_OPENING = 0;
                            if (!string.IsNullOrEmpty(hdfSubDeptPk.Value) && Convert.ToInt32(hdfSubDeptPk.Value) > 0)
                                invoiceHeaderObj.IVH_ISSUE_DEPT = Convert.ToInt32(hdfSubDeptPk.Value);
                            SetUIValuesToObject(ControlsEnum.POINVDETAIL);

                            //For Solving Foreign Key ref issue:If GRNQty is zero (which is applied through popup) then remove that GRN from OrderDetail List

                            invoiceHeaderObj.OrderDetail.ForEach(dtl => dtl.GRNDtl.RemoveAll(grndtl => grndtl.VGL_QTY_INVOICED == 0));

                            invoiceHeaderObj.IVH_TOTAL_QTY = 0;//Dummy
                            txtSubTotal = (TextBox)grdInvoice.FooterRow.FindControl("txtSubTotalFooter");
                            invoiceHeaderObj.IVH_AMOUNT_TC = txtSubTotal == null ? 0 : string.IsNullOrEmpty(txtSubTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtSubTotal.Text.Trim());
                            txtAdjustAmountFooter = (TextBox)grdInvoice.FooterRow.FindControl("txtAdjustAmountFooter"); //Adjust amount of IVH_AMOUNT_TC
                            invoiceHeaderObj.IVH_AMOUNT_NET_TC_ADJ = txtAdjustAmountFooter == null ? 0 : string.IsNullOrEmpty(txtAdjustAmountFooter.Text.Trim()) ? 0 : Convert.ToDouble(txtAdjustAmountFooter.Text.Trim());

                            int numberGenerationSubType;
                            if ((byte)POGroup == (byte)POInvoiceGroup.Services)
                            {
                                numberGenerationSubType = 2;
                            }
                            else
                            {
                                numberGenerationSubType = string.IsNullOrEmpty(hdfPOItemType.Value) ? 1 : Convert.ToInt16(hdfPOItemType.Value) == (Int16)POItemType.Others ?
                                    2 : 1;
                            }

                            invoiceHeaderObj.IVH_GROUP = ((byte)POGroup) == (byte)0 ? (byte)1 : (byte)POGroup;//
                            invoiceHeaderObj.APT_CODE = POGroup == POInvoiceGroup.Goods ? ApplicationType.PI
                                : POGroup == POInvoiceGroup.Services ? ApplicationType.PSI : ApplicationType.EI;
                            invoiceHeaderObj.AST_DOC_MODE = AST_DOC_MODE.Value == "1" ? 1 : 0;
                            invoiceHeaderObj.AST_VALUE = numberGenerationSubType.ToString();//invoiceHeaderObj.IVH_TYPE;
                            //Uploads
                            invoiceHeaderObj.FileList = POUploadList.Where(Row => Row.DOC_STATUS == 1).ToList();
                            if (invoiceHeaderObj.IVH_PK == 0)
                            {
                                invoiceHeaderObj.OrderDetail.ForEach(dtl =>
                                {
                                    dtl.VID_PK = 0;
                                    dtl.TaxDtl.ForEach(tax => tax.VTL_PK = 0);
                                });
                                invoiceHeaderObj.TaxHdr.ForEach(tax => tax.VTL_PK = 0);
                            }

                            if (ddlAddressType.Items.Count > 0)
                                invoiceHeaderObj.IVH_VENDOR_CONTACT = ddlAddressType.SelectedValue;

                            invoiceHeaderObj.IVH_BRANCH_TYPE = string.IsNullOrEmpty(hdfVendorContactType.Value) ? 0 : Convert.ToInt32(hdfVendorContactType.Value);
                            invoiceHeaderObj.IVH_TAX_ID = txtVatTaxId.Text == null ? string.Empty : HttpUtility.HtmlEncode(txtVatTaxId.Text);
                            invoiceHeaderObj.IVH_BRANCH_TEXT = txtBranchCode.Text == null ? string.Empty : HttpUtility.HtmlEncode(txtBranchCode.Text);

                            //Declaration No
                            invoiceHeaderObj.IVH_IMP_DECL_NO = txtDeclarationNO.Text == null ? string.Empty : HttpUtility.HtmlEncode(txtDeclarationNO.Text);

                            double InvSubTotal = 0;
                            InvSubTotal = POInvoiceHeaderSession.IVH_AMOUNT_TC + POInvoiceHeaderSession.IVH_AMOUNT_NET_TC_ADJ;
                            //Total Amount against PO
                            foreach (POOtherChargeDetails li in POInvoiceHeaderSession.OtherChargeDetails)
                            {
                                li.IVM_AMOUNT = POInvoiceHeaderSession.OrderDetail.Where(p => Convert.ToInt32(p.VID_PO) == li.IVM_PO_HDR).Sum(res => res.VID_AMOUNT);
                                if (InvSubTotal > 0)
                                {
                                    li.IVM_TAX_AMOUNT = (li.IVM_AMOUNT * POInvoiceHeaderSession.IVH_TAX_TC) / InvSubTotal;
                                    li.IVM_DISCOUNT_AMOUNT = (li.IVM_AMOUNT * POInvoiceHeaderSession.IVH_DISCOUNT_TC) / InvSubTotal;
                                }
                            }


                            invoiceHeaderObj.IVH_BILL_ENTRY_NO = string.IsNullOrEmpty(txtBillNo.Text.Trim()) ? null : txtBillNo.Text.Trim();
                            invoiceHeaderObj.IVH_BILL_ENTRY_DATE = string.IsNullOrEmpty(txtBillDate.Text.Trim()) ? null : txtBillDate.Text.Trim();
                            invoiceHeaderObj.IVH_BILL_ENTRY_VALUE = string.IsNullOrEmpty(txtBillAmount.Text.Trim()) ? null : txtBillAmount.Text.Trim();
                            invoiceHeaderObj.PREPARED_BY = string.IsNullOrEmpty(txtPreparedBy.Text.Trim()) ? null : txtPreparedBy.Text.Trim();

                            //List<POOtherChargeDetails> objGroupLst = othrLst.GroupBy(ch => ch.IVM_PO_HDR).Select(grp => new POOtherChargeDetails
                            //    {
                            //        IVM_ACTIVE = grp.FirstOrDefault().IVM_ACTIVE,
                            //        IVM_ADJUST_AMOUNT = grp.Sum(r => r.IVM_ADJUST_AMOUNT),
                            //        IVM_AMOUNT = grp.Sum(r => r.IVM_AMOUNT),
                            //        IVM_DISCOUNT_AMOUNT = grp.Sum(r => r.IVM_DISCOUNT_AMOUNT),
                            //        IVM_OTHER_AMOUNT = grp.Sum(r => r.IVM_OTHER_AMOUNT),
                            //        IVM_PK = grp.FirstOrDefault().IVM_PK,
                            //        IVM_PO_HDR = grp.FirstOrDefault().IVM_PO_HDR,
                            //        IVM_TAX_AMOUNT = grp.Sum(r => r.IVM_TAX_AMOUNT),
                            //        PO_OTHER_AMOUNT = grp.Sum(r => r.PO_OTHER_AMOUNT),
                            //        PO_OTHER_AMOUNT_INVOICED = grp.Sum(r => r.PO_OTHER_AMOUNT_INVOICED),
                            //        POH_DATE = grp.FirstOrDefault().POH_DATE,
                            //        POH_NO = grp.FirstOrDefault().POH_NO,
                            //    }
                            //    ).ToList();

                            //if (objGroupLst != null && objGroupLst.Count > 0)
                            //{
                            //    invoiceHeaderObj.OtherChargeDetails.Clear();
                            //    objGroupLst.ForEach(dtl => invoiceHeaderObj.OtherChargeDetails.Add(dtl));
                            //}


                        }
                        retObject = invoiceHeaderObj;
                        break;
                    case ControlsEnum.POINVDETAIL:
                        rowID = 0;
                        foreach (GridViewRow grdrow in grdInvoice.Rows)
                        {
                            soInvoiceDetailsObj = new POInvoiceDetails();

                            hdfRRDPK = (HiddenField)grdInvoice.Rows[rowID].FindControl("hdfInvoiceDtlPK");
                            hdfItemPK = (HiddenField)grdInvoice.Rows[rowID].FindControl("hdfItemPK");
                            hdfPOPK = (HiddenField)grdInvoice.Rows[rowID].FindControl("hdfSODtlPK");
                            int invDtlPK;
                            int POPK, ItemPk;
                            ItemPk = Convert.ToInt32(hdfItemPK.Value);
                            POPK = Convert.ToInt32(hdfPOPK.Value);
                            double effRate = 0;
                            if (hdfRRDPK != null && int.TryParse(hdfRRDPK.Value, out invDtlPK))
                            {
                                soInvoiceDetailsObj = POInvoiceHeaderSession.OrderDetail.SingleOrDefault(dtl => dtl.VID_PK == invDtlPK && dtl.VID_ITEM == ItemPk && Convert.ToInt32(dtl.VID_PO) == POPK);
                                if (soInvoiceDetailsObj != null)
                                {
                                    //commented : The GRN mapping getting wrong after saving invoice.
                                    //soInvoiceDetailsObj.VID_SL_NO = rowID + 1;
                                    TextBox txtInvNow = (TextBox)grdInvoice.Rows[rowID].FindControl("txtInvNow");
                                    soInvoiceDetailsObj.VID_QTY_INVOICED = txtInvNow == null ? 0 : string.IsNullOrEmpty(txtInvNow.Text) ? 0 : Convert.ToDouble(txtInvNow.Text);
                                    txtAmount = (TextBox)grdInvoice.Rows[rowID].FindControl("txtAmount");
                                    soInvoiceDetailsObj.VID_AMOUNT = txtAmount == null ? 0 : string.IsNullOrEmpty(txtAmount.Text.Trim()) ? 0 : Convert.ToDouble(txtAmount.Text.Trim());
                                    txtDiscount = (TextBox)grdInvoice.Rows[rowID].FindControl("txtDiscount");
                                    soInvoiceDetailsObj.VID_DISCOUNT = txtDiscount == null ? 0 : string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? 0 : Convert.ToDouble(txtDiscount.Text.Trim());
                                    txtTax = (TextBox)grdInvoice.Rows[rowID].FindControl("txtTax");
                                    soInvoiceDetailsObj.VID_TAX = txtTax == null ? 0 : string.IsNullOrEmpty(txtTax.Text.Trim()) ? 0 : Convert.ToDouble(txtTax.Text.Trim());
                                    txtTotal = (TextBox)grdInvoice.Rows[rowID].FindControl("txtTotal");
                                    soInvoiceDetailsObj.VID_NET_AMOUNT = txtTotal == null ? 0 : string.IsNullOrEmpty(txtTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtTotal.Text.Trim());
                                    hasValidRate = hasValidRate || soInvoiceDetailsObj.VID_QTY_INVOICED >= 0;
                                    rfqTaxHeaderList = soInvoiceDetailsObj.TaxDtl.ToList();
                                    if (rfqTaxHeaderList != null && rfqTaxHeaderList.Count > 0)
                                    {
                                        rfqTaxHeaderList.ForEach(dtl => dtl.VTL_SL_NO = soInvoiceDetailsObj.VID_SL_NO);
                                    }

                                    ddlAssetType = (DropDownList)grdInvoice.Rows[rowID].FindControl("ddlAssetType");
                                    chkIsAsset = (CheckBox)grdInvoice.Rows[rowID].FindControl("chkIsAsset");
                                    soInvoiceDetailsObj.IsAsset = Convert.ToInt32(chkIsAsset.Checked);
                                    if (chkIsAsset.Checked)
                                        soInvoiceDetailsObj.AssetTypePK = (ddlAssetType.SelectedValue == "" || ddlAssetType.SelectedValue == CommonConstants.SELECTVAL)
                                                                                ? (int?)null : Convert.ToInt32(ddlAssetType.SelectedValue);

                                    //TextBox txtEffRate = (TextBox)grdInvoice.Rows[rowID].FindControl("txtEffRate");
                                    HiddenField hdfEffRate = (HiddenField)grdInvoice.Rows[rowID].FindControl("hdfEffRate");
                                    double.TryParse(hdfEffRate.Value, out effRate);
                                    soInvoiceDetailsObj.VID_RATE_EFCT = effRate;

                                    //// Set each PO amount details against Invoice to PO Other Charge Details
                                    //POOtherChargeDetails rfqOtherCharge = new POOtherChargeDetails();
                                    //rfqOtherCharge = POInvoiceHeaderSession.OtherChargeDetails.SingleOrDefault(amt => amt.IVM_PO_HDR.ToString() == soInvoiceDetailsObj.VID_PO);
                                    //if (rfqOtherCharge != null)
                                    //{
                                    //    rfqOtherCharge.IVM_AMOUNT = soInvoiceDetailsObj.VID_AMOUNT;
                                    //    if (hdfisTaxAdd.Value == "1") 
                                    //    {
                                    //        rfqOtherCharge.IVM_TAX_AMOUNT = soInvoiceDetailsObj.VID_TAX;
                                    //    }
                                    //    if (hdfisDiscountAdd.Value == "1")
                                    //    {
                                    //        rfqOtherCharge.IVM_DISCOUNT_AMOUNT = soInvoiceDetailsObj.VID_DISCOUNT;
                                    //    }
                                    //    //invoiceHeaderObj.OtherChargeDetails.Where(amt => amt.IVM_PO_HDR.ToString() == soInvoiceDetailsObj.VID_PO && amt.IVM_AMOUNT >0).ToList().ForEach(dtl =>
                                    //    //{
                                    //    //    dtl.IVM_AMOUNT = soInvoiceDetailsObj.VID_AMOUNT;
                                    //    //    dtl.IVM_TAX_AMOUNT = soInvoiceDetailsObj.VID_TAX;
                                    //    //    dtl.IVM_DISCOUNT_AMOUNT = soInvoiceDetailsObj.VID_DISCOUNT;
                                    //    //});                                       
                                    //}
                                    SetCostCenterDetails(soInvoiceDetailsObj);
                                }
                            }

                            rowID++;
                        }


                        break;
                    #endregion
                    #region Journalize
                    case ControlsEnum.JOURNALIZE:
                        ////Start
                        if (EntryStatus == EntryStatus.LISTMODE)
                        {
                            ////
                            foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                            {
                                //RadioButton rbtn;
                                //rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                                //if (rbtn.Checked)
                                //{
                                CheckBox chkPIselect;
                                chkPIselect = (CheckBox)grdrow.FindControl("chkPIselect");
                                if (chkPIselect.Checked)
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
                                GetFieldValues(ControlsEnum.POINVHEADER);

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
                                    : POGroup == POInvoiceGroup.Services ? ApplicationType.PSIJ
                                    : POGroup == POInvoiceGroup.WorkOrder ? ApplicationType.PIJ : ApplicationType.EIJ;
                                ucrJournalize.TransactionType = Invoice;
                                Session[ERP.Utilities.SessionStrings.TransactionType] = Invoice;
                                ucrJournalize.TransactionPK = CurrPK;
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                                ucrJournalize.JournalizePK = 0;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = invoiceHeaderObj.IVH_NO;
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = invoiceHeaderObj.IVH_DATE;
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = invoiceHeaderObj.IVH_CURRENCY;
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.AP;
                                Session[ERP.Utilities.SessionStrings.AccountPayablePK] = invoiceHeaderObj.IVH_VENDOR;
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
                                int numberGenerationSubType;
                                if ((byte)POGroup == (byte)POInvoiceGroup.Services)
                                {
                                    numberGenerationSubType = 0;// (int)AppSubTypeCNPurchase.NONSTOCK;
                                }
                                else
                                {
                                    numberGenerationSubType = string.IsNullOrEmpty(hdfPOItemType.Value) ? (int)AppSubTypeCNPurchase.STOCK : Convert.ToInt16(hdfPOItemType.Value) == (Int16)POItemType.Others ?
                                        (int)AppSubTypeCNPurchase.NONSTOCK : (int)AppSubTypeCNPurchase.STOCK;
                                }

                                ucrJournalize.TypeForNumberGenaration = numberGenerationSubType.ToString();//((int)AppSubTypeCNPurchase.STOCK).ToString();
                                ucrJournalize.CallUserControl();
                                Session[ERP.Utilities.SessionStrings.JournalHead] = GetLocalResourceObject("Purchase_Invoice_Journal").ToString();

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
                    #region Pick Invoice for Paying
                    case ControlsEnum.PICKFORPAYMENT:

                        #region  Pick multiple invoice for payment, in cases like invoice are in different pages
                        SetAllocationDetails();
                        if (Session[ERP.Utilities.SessionStrings.SelectedInvoicesInfoLst] != null)
                        {
                            SelectedInvoicesInfoLst = (List<SelectionInfo>)Session[ERP.Utilities.SessionStrings.SelectedInvoicesInfoLst];
                            InvCategory = 0;
                            foreach (SelectionInfo item in SelectedInvoicesInfoLst)
                            {
                                if (item.chkChecked)
                                {
                                    if (item.ApprovedStatus == 2 && item.IsPosted && item.JournalStatus == 2)
                                    {
                                        if ((item.BalanceAmt > 0) || (Iscont == true && (hdfIscontYes.Value == "1")))
                                        {
                                            if (SelectedCurrency == 0)
                                                SelectedCurrency = item.CurrencyPK;
                                            if (InvCategory == 0)
                                                InvCategory = item.InvCategory;

                                            if (SelectedVendors == 0)
                                                SelectedVendors = item.VendorPK;

                                            //if (InvCategory != item.InvCategory)//Not allowed to invoice multiple PO with different types (Service/Goods)
                                            //{
                                            //    InvalidPaymentItem = true;
                                            //    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Type").ToString();
                                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            //    break;
                                            //}
                                            if (SelectedCurrency != item.CurrencyPK)
                                            {
                                                InvalidPaymentItem = true;
                                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Currency_Payment").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                break;
                                            }
                                            else if (SelectedVendors != item.VendorPK)
                                            {
                                                InvalidPaymentItem = true;
                                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Vendor_Payment").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                break;
                                            }
                                            else if (!IsMatcingTax(SelectedINVTax, item.Tax))
                                            {
                                                InvalidPaymentItem = true;
                                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Tax").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                break;
                                            }
                                            //else if (!IsExixtPk(SelectedInvoicesInfoLst, item.InvoicePK))
                                            //{
                                            // Add Tax
                                            if (SelectedINVTax != null)
                                            {
                                                SelectedINVTaxList = SelectedINVTax;
                                            }
                                            else
                                            {
                                                SelectedINVTaxList = new List<decimal>();
                                            }
                                            SelectedINVTaxList.Add(item.Tax);
                                            SelectedINVTax = SelectedINVTaxList;

                                            ////Add Invoices
                                            //if (SelectedInvoicesInfoLst == null)
                                            //    SelectedInvoicesInfoLst = new List<long>();
                                            //SelectedInvoicesInfoLst.Add(selectedInvoice);

                                            //For Saving Selected Item PK
                                            hdfSelectedItemPk.Value = hdfSelectedItemPk.Value + "," + item.InvoicePK.ToString();
                                            //}
                                            //else
                                            //{
                                            //    InvalidPaymentItem = true;
                                            //    litErrorMsg.Text = Resources.ErrorMessages.Msg_Item_Added;
                                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            //    break;
                                            //}
                                        }
                                        else
                                        {
                                            Iscont = true;
                                            InvalidPaymentItem = true;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlreadyPaid", "$(document).ready(function(){ShowAlreadyPaid();});", true);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (item.ApprovedStatus != 2)
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("MsgApproveforPayment").ToString();
                                        }
                                        else if (!item.IsPosted)  //Posted == false
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("MsgJournalizeforPayment").ToString();
                                        }
                                        else if (item.JournalStatus != 2)  //Not Approved
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("MsgApprovedJVforPayment").ToString();
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
                                    btnPickForPayment.Text = GetLocalResourceObject("PickPoForPayment").ToString();
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

                        #region Old Code with RadioButton selection

                        //------Old Code -------

                        //foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        //{
                        //    RadioButton rbtn;
                        //    rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                        //    //lblBalAmt = grdrow.FindControl("lblBalAmt") as Label;
                        //    lbnBalAmt = grdrow.FindControl("lbnBalAmt") as LinkButton;
                        //    if (rbtn.Checked)
                        //    {
                        //        bIsChecked = true;
                        //        selectedInvoice = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                        //        approvedStatus = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                        //        isPosted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                        //        selectedVendor = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfVendorPK")).Value);
                        //        selectedCurrency = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPOCurrency")).Value);
                        //        INVTax = Convert.ToDecimal(string.IsNullOrEmpty(((HiddenField)grdrow.FindControl("hdfTaxAmount")).Value) ? "0" : ((HiddenField)grdrow.FindControl("hdfTaxAmount")).Value);
                        //        //balamt = Convert.ToDecimal(lblBalAmt.Text);
                        //        balamt = Convert.ToDecimal(lbnBalAmt.Text);
                        //        break;
                        //    }
                        //}
                        //if (bIsChecked)
                        //{
                        //    if (approvedStatus == 2 && isPosted)
                        //    {
                        //        if ((balamt > 0) || (Iscont == true && (hdfIscontYes.Value == "1")))
                        //        {
                        //            Iscont = false;
                        //            if (SelectedCurrency == 0)
                        //                SelectedCurrency = selectedCurrency;
                        //            if (SelectedVendors == 0)
                        //                SelectedVendors = selectedVendor;
                        //            if (SelectedCurrency != selectedCurrency)
                        //            {
                        //                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Currency_Payment").ToString();
                        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //            }
                        //            else if (SelectedVendors != selectedVendor)
                        //            {
                        //                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Vendor_Payment").ToString();
                        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //            }
                        //            else if (!IsMatcingTax(SelectedINVTax, INVTax))
                        //            {
                        //                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Tax").ToString();
                        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //            }
                        //            else if (!IsExixtPk(SelectedInvoices, selectedInvoice))
                        //            {
                        //                //Add Tax
                        //                if (SelectedINVTax != null)
                        //                {
                        //                    SelectedINVTaxList = SelectedINVTax;
                        //                }
                        //                else
                        //                {
                        //                    SelectedINVTaxList = new List<decimal>();
                        //                }
                        //                SelectedINVTaxList.Add(INVTax);
                        //                SelectedINVTax = SelectedINVTaxList;

                        //                //Add Invoices
                        //                if (SelectedInvoices == null)
                        //                    SelectedInvoices = new List<long>();
                        //                SelectedInvoices.Add(selectedInvoice);
                        //                btnPickForPayment.Text = Resources.Controls.PickPoForInvoicing;
                        //                btnPickForPayment.Text = GetLocalResourceObject("PickPoForPayment").ToString() + "(" + SelectedInvoices.Count().ToString() + ")";
                        //                Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        //                Session[ERP.Utilities.SessionStrings.JOURNALIZETAB_SELECTED_PK] = null;


                        //                //For Saving Selected Item PK
                        //                hdfSelectedItemPk.Value = hdfSelectedItemPk.Value + "," + selectedInvoice.ToString();
                        //            }
                        //            else
                        //            {
                        //                litErrorMsg.Text = Resources.ErrorMessages.Msg_Item_Added;
                        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //            }
                        //        }
                        //        else
                        //        {
                        //            Iscont = true;
                        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlreadyPaid", "$(document).ready(function(){ShowAlreadyPaid();});", true);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (approvedStatus != 2)
                        //        {
                        //            litErrorMsg.Text = GetLocalResourceObject("Msg_PickPayment_Msg").ToString();
                        //        }
                        //        else if (!isPosted)  //Posted == false
                        //        {
                        //            litErrorMsg.Text = GetLocalResourceObject("Msg_PickPaymentPost_Msg").ToString();
                        //        }
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //    }

                        //    //For Setting/Resetting Colour of a selected InvoiceNo
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);

                        //    //End
                        //}
                        //else
                        //{
                        //    litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //}
                        #endregion

                        break;
                    #endregion
                    #region Pick Inv & for Cr/Dr. Note
                    case ControlsEnum.PICKFORCRDRNOTE:
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            //RadioButton rbtn;
                            //rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            //if (rbtn.Checked)
                            //{
                            HiddenField hdfDept;
                            int dept;
                            CheckBox chkPIselect;
                            chkPIselect = (CheckBox)grdrow.FindControl("chkPIselect");
                            if (chkPIselect.Checked)
                            {
                                bIsChecked = true;
                                selectedInvoice = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                approvedStatus = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                isPosted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                selectedVendor = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfVendorPK")).Value);
                                selectedCurrency = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPOCurrency")).Value);
                                //selectedPOType = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPOType")).Value); //change for opening invoice
                                HiddenField hdfPOtype = (HiddenField)grdrow.FindControl("hdfPOType");
                                selectedPOType = string.IsNullOrEmpty(hdfPOtype.Value) ? (int)POInvoiceGroup.Goods : Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPOType")).Value);

                                InvoiceValue = Convert.ToDecimal(((Label)grdrow.FindControl("lblInvoiceValue")).Text.Replace(",", ""));

                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                bool IsRequiredJournlForCNDN = GetGlobalResourceObject("ConfigurationsRes", "IsRequiredJournlForCNDN").ToString() == "1" ? true : false;
                                if (IsRequiredJournlForCNDN == false) isPosted = true;//To bypass isposted logic
                                if (approvedStatus == 2 && isPosted)
                                {
                                    //if (InvoiceValue > 0)
                                    //{
                                    if (SelectedCurrency == 0)
                                        SelectedCurrency = selectedCurrency;
                                    if (SelectedVendors == 0)
                                        SelectedVendors = selectedVendor;
                                    if (SelectedPOTypes == 0)
                                        SelectedPOTypes = selectedPOType;
                                    if (SelectedCurrency != selectedCurrency)
                                    {
                                        InvalidPaymentItem = true;
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Currency_CNDN").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        break;
                                    }
                                    else if (SelectedVendors != selectedVendor)
                                    {
                                        InvalidPaymentItem = true;
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Vendor_CNDN").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        break;
                                    }
                                    else if (SelectedPOTypes != selectedPOType)
                                    {
                                        InvalidPaymentItem = true;
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_POType_CNDN").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        break;
                                    }
                                    else if (!IsExixtPk(SelectedInvoicesCrDr, selectedInvoice))
                                    {
                                        //Add Invoices
                                        if (SelectedInvoicesCrDr == null)
                                            SelectedInvoicesCrDr = new List<long>();
                                        SelectedInvoicesCrDr.Add(selectedInvoice);
                                    }
                                    else
                                    {
                                        InvalidPaymentItem = true;
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Item_Added;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        break;
                                    }
                                    //}
                                    //else
                                    //{
                                    //    InvalidPaymentItem = true;
                                    //    litErrorMsg.Text = GetLocalResourceObject("Msg_InvZeroAmount").ToString();
                                    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    //    break;
                                    //}
                                }
                                else
                                {
                                    if (approvedStatus != 2)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("MsgApproveforCNDN").ToString();
                                        InvalidPaymentItem = true;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        break;
                                    }
                                    else if (Posted == false && IsRequiredJournlForCNDN == true)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("MsgJournalizeforCNDN").ToString();
                                        InvalidPaymentItem = true;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        break;
                                    }

                                }
                            }
                        }

                        if (SelectedInvoicesCrDr != null)
                        {
                            if (SelectedInvoicesCrDr.Count > 0)
                            {
                                if (!InvalidPaymentItem)
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
                                    ResetForm(ControlsEnum.RESETPAYMENT);
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        //if (bIsChecked)
                        //{

                        //}
                        //else
                        //{
                        //    litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //}
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
                        alertBoObj.ATH_TRX_DATE = txtInvoiceDate.Text.Trim() == string.Empty ? DateTime.Now : Convert.ToDateTime(txtInvoiceDate.Text.Trim());
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

        private void SetCostCenterDetails(POInvoiceDetails ObjInvDtls)
        {
            if (ObjInvDtls != null)
            {
                poCostCenterlst = new List<POCostCenterDetails>();
                poCostCenterlst = ObjInvDtls.CostCenterDtl;
                POdtlPK = Convert.ToInt32(ObjInvDtls.VID_PO_DTL);
                double rate = 0;
                double OrderQty = 0;
                double qty = 0;
                double TotalAmt = 0;
                foreach (GridViewRow grdrow in grdInvoice.Rows)
                {
                    if (((HiddenField)grdrow.FindControl("hdfPOdtl")).Value == POdtlPK.ToString())
                    {
                        rate = ((TextBox)grdrow.FindControl("txtRate")).Text == string.Empty ? 0 : Convert.ToDouble(((TextBox)grdrow.FindControl("txtRate")).Text);
                        qty = ((TextBox)grdrow.FindControl("txtInvNow")).Text == string.Empty ? 0 : Convert.ToDouble(((TextBox)grdrow.FindControl("txtInvNow")).Text);
                        OrderQty = ((Label)grdrow.FindControl("lblOrderQuantity")).Text == string.Empty ? 0 : Convert.ToDouble(((Label)grdrow.FindControl("lblOrderQuantity")).Text);
                        TotalAmt = ((TextBox)grdrow.FindControl("txtAmount")).Text == string.Empty ? 0 : Convert.ToDouble(((TextBox)grdrow.FindControl("txtAmount")).Text);
                        break;
                    }
                }
                if (poCostCenterlst != null && poCostCenterlst.Count > 0)
                {
                    foreach (var items in poCostCenterlst)
                    {
                        if (qty > 0)
                        {
                            items.IVC_PERCENTAGE = (items.IVC_POR_QTY_ORDERED / OrderQty) * 100;
                            items.IVC_QUANTITY = Convert.ToDouble(GetFormattedNumber((qty * (items.IVC_POR_QTY_ORDERED / OrderQty) * 100) / 100));
                        }
                        else
                        {
                            items.IVC_PERCENTAGE = 0;
                            items.IVC_QUANTITY = 0;
                        }
                        items.IVC_AMOUNT = Convert.ToDouble(GetFormattedCurrency(items.IVC_QUANTITY * rate));
                        items.IVC_PO_DTL = Convert.ToInt32(ObjInvDtls.VID_PO_DTL);
                    }
                    double QtyDiff = poCostCenterlst.Sum(x => x.IVC_QUANTITY) - qty;
                    double AmtDiff = poCostCenterlst.Sum(x => x.IVC_AMOUNT) - TotalAmt;
                    if (QtyDiff != 0)
                        poCostCenterlst[poCostCenterlst.Count - 1].IVC_QUANTITY = poCostCenterlst[poCostCenterlst.Count - 1].IVC_QUANTITY - QtyDiff;
                    if (AmtDiff != 0)
                        poCostCenterlst[poCostCenterlst.Count - 1].IVC_AMOUNT = poCostCenterlst[poCostCenterlst.Count - 1].IVC_AMOUNT - AmtDiff;
                }
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
                    case ControlsEnum.INVOICEGET:
                        if (dtInvoiceList != null && dtInvoiceList.Rows.Count > 0)
                        {
                            CurrPK = Convert.ToInt32(Request.QueryString["PK"].ToString());//  Convert.ToInt32(dtInvoiceList.Rows[0]["IVH_PK"].ToString());
                            if (dtInvoiceList.Rows[0]["IVH_IS_OPENING"].ToString() == "1")
                            {
                                Session[ERP.Utilities.SessionStrings.InvoicePK] = CurrPK.ToString();
                                Response.Redirect(Resources.PageURL.PurchaseOpeningInvoice);
                                break;
                            }
                            Session[ERP.Utilities.SessionStrings.VendorPK] = dtInvoiceList.Rows[0]["IVH_VND_PK"].ToString();
                            Session[ERP.Utilities.SessionStrings.Vendor] = dtInvoiceList.Rows[0]["IVH_VENDOR_TEXT"].ToString();
                            ////start
                            Approved = Convert.ToInt32(dtInvoiceList.Rows[0]["IVH_STATUS"].ToString());
                            Posted = Convert.ToBoolean(dtInvoiceList.Rows[0]["IVH_HAS_JRNL_ENTRY"].ToString());
                            ////
                            if (Convert.ToInt16(dtInvoiceList.Rows[0]["IVH_DEL_STATUS"].ToString()) == 1)
                            {
                                btnSave.Visible = false;
                                hdfIsInvCancelled.Value = "1";
                            }
                            else
                            {
                                btnSave.Visible = true;
                                hdfIsInvCancelled.Value = "0";
                            }
                            if (!string.IsNullOrEmpty(dtInvoiceList.Rows[0]["IVH_DEPT"].ToString()) && int.TryParse(dtInvoiceList.Rows[0]["IVH_DEPT"].ToString(), out dept))
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
                                //EntryStatus = EntryStatus.VIEWMODE;
                                ucrWrkf.ViewAction();
                            }
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.POINVHEADER);
                            if (invoiceHeaderObj.IVH_TYPE == ((byte)PurchaseType.Local).ToString())
                            {
                                divVendorBranch.Visible = true;
                                vndPK = Convert.ToInt32(invoiceHeaderObj.IVH_VENDOR);
                                GetFieldValues(ControlsEnum.VENDORBRANCH);
                                SetFieldValues(ControlsEnum.VENDORBRANCH);
                            }
                            else
                            {
                                divVendorBranch.Visible = false;
                            }
                            SetFieldValues(ControlsEnum.POINVHEADER);
                            SetFieldValues(ControlsEnum.POINVDETAIL);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            SetFieldValues(ControlsEnum.GRNATTACHMENTS);
                            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
                            hdfApplyTax.Value = "1";// No need to use formula for Tax  amount
                            SetDetailTax(null);
                            SetHdrTax();
                            if (grdInvoice.Rows.Count > 0)
                            {
                                SetSubTotal();
                            }
                            //Settings of TaxPayableDiv
                            if (hdfIsTaxPayable.Value.ToString() == "1")
                            {
                                SetTaxPayableDiv();
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);

                        }
                        break;
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

                    case ControlsEnum.POINVHEADER:
                        if (invoiceHeaderObj != null)
                        {
                            //if (invoiceHeaderObj.POH_MENU_TYPE == 3)
                            //{
                            //    pnlConvert.Visible = true;
                            //}
                            POGroup = (POInvoiceGroup)Enum.Parse(typeof(POInvoiceGroup), invoiceHeaderObj.IVH_GROUP.ToString());
                            Approved = invoiceHeaderObj.IVH_STATUS;
                            lblCustomerTxt.Text = invoiceHeaderObj.IVH_VENDOR_NAME;
                            lblCustomerTxt.ToolTip = HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_VENDOR_NAME);
                            lblCustomerCde.Text = invoiceHeaderObj.IVH_VENDOR_TEXT;
                            lblCustomerCde.ToolTip = HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_VENDOR_TEXT);
                            vPK = Convert.ToInt32(invoiceHeaderObj.IVH_VENDOR);
                            lbtnSoNoTxt.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_PO_NO), 21);
                            lbtnSoNoTxt.ToolTip = HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_PO_NO);
                            hdfSoNo.Value = invoiceHeaderObj.IVH_ORDER;

                            hdfCrDrStatus.Value = invoiceHeaderObj.IVH_STATUS.ToString();
                            hdfCrDrWKFStatus.Value = Convert.ToInt16(invoiceHeaderObj.IVH_HAS_JRNL_ENTRY).ToString();

                            lbtnPRNoTxt.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_PR_NO), 21);
                            lbtnPRNoTxt.ToolTip = HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_PR_NO);
                            hdfPRNo.Value = invoiceHeaderObj.IVH_PRH_PK;

                            lblSODateTxt.Text = DateTime.Parse(HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_POH_DT)).ToString(Resources.Constants.DateFormatShort);
                            lblSODateTxt.ToolTip = DateTime.Parse(HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_POH_DT)).ToString(Resources.Constants.DateFormatShort);

                            lblSOAmtTxt.Text = lblSOAmtTxt.ToolTip = String.Format("{0:c}", Convert.ToDecimal(invoiceHeaderObj.IVH_PO_AMOUNT_NET_TC));
                            if (string.IsNullOrEmpty(invoiceHeaderObj.IVH_INV_AMT) || Convert.ToDouble(invoiceHeaderObj.IVH_INV_AMT) < 0)
                            {
                                lblInvAmtTxt.Text = lblInvAmtTxt.ToolTip = String.Format("{0:c}", 0);
                            }
                            else
                            {
                                lblInvAmtTxt.Text = lblInvAmtTxt.ToolTip = String.Format("{0:c}", Convert.ToDecimal(invoiceHeaderObj.IVH_INV_AMT));
                            }

                            hdfCurrency.Value = invoiceHeaderObj.IVH_CURRENCY.ToString();
                            lblCurrencyTxt.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_CURRENCY_TEXT), 21);
                            lblCurrencyTxt.ToolTip = HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_CURRENCY_TEXT);


                            hdfInvoicePK.Value = invoiceHeaderObj.IVH_PK.ToString();
                            hdfInvoiceNo.Value = invoiceHeaderObj.IVH_NO == string.Empty ? "" : invoiceHeaderObj.IVH_NO;
                            lblInvoiceNo.Text = invoiceHeaderObj.IVH_NO == string.Empty ? "[NEW]" : invoiceHeaderObj.IVH_NO;
                            txtInvoiceDate.Text = invoiceHeaderObj.IVH_DATE;
                            txtRemarks.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_REMARKS);
                            if (Convert.ToInt32(hdfShowInvestor.Value) == 1)
                            {
                                txtInvestor.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_INVESTOR);
                            }

                            txtInvoiceType.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_TYPE_TEXT);
                            if (invoiceHeaderObj.IVH_TYPE == ((byte)PurchaseType.Local).ToString())
                                hdfPortType.Value = "1";
                            else
                                hdfPortType.Value = "2";

                            if (EnableGST == 1)
                                ddlGSTInvoiceType.SelectedIndex = ddlGSTInvoiceType.Items.IndexOf(ddlGSTInvoiceType.Items.FindByValue(invoiceHeaderObj.IVH_GST_TYPE));

                            txtFromPort.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_FROM_PORT_TEXT);
                            hdfFromPortID.Value = Convert.ToString(invoiceHeaderObj.IVH_FROM_PORT);
                            txtToPort.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_TO_PORT_TEXT);
                            hdfToPortID.Value = Convert.ToString(invoiceHeaderObj.IVH_TO_PORT);

                            //Exchange rate to Textbox
                            //Not allowed to edit exchange rate while currency same as base currency
                            GetFieldValues(ControlsEnum.EXCHANGERATE);
                            if (CurrPK > 0)
                            {
                                txtExchangeRate.Text = hdfExchangeRate.Value = invoiceHeaderObj.IVH_EXCHG_RATE.ToString();
                            }
                            if (invoiceHeaderObj.IVH_TYPE == ((byte)PurchaseType.Local).ToString() || currentUser.BaseCurrency == invoiceHeaderObj.IVH_CURRENCY)//invoiceHeaderObj.IVH_BASE_CURR
                            {
                                txtExchangeRate.Enabled = false;
                            }
                            else
                            {
                                txtExchangeRate.Enabled = true;
                            }


                            txtHdrDiscount.Text = invoiceHeaderObj.IVH_DISCOUNT_TC.ToString(hdfCurrencyFormat.Value);
                            if (IsAdvInvHasTax)
                            {
                                txtHdrDeduction.ToolTip = txtHdrDeduction.Text = invoiceHeaderObj.IVH_AMOUNT_ADV_DED_TC.ToString(hdfCurrencyFormat.Value);
                                txtTotalDeduction.Text = txtTotalDeduction.ToolTip = ((double)0).ToString(hdfCurrencyFormat.Value);
                            }
                            else
                            {
                                txtHdrDeduction.ToolTip = txtHdrDeduction.Text = ((double)0).ToString(hdfCurrencyFormat.Value);
                                txtTotalDeduction.Text = txtTotalDeduction.ToolTip = invoiceHeaderObj.IVH_AMOUNT_ADV_DED_TC.ToString(hdfCurrencyFormat.Value);
                            }
                            txtHdrTax.Text = invoiceHeaderObj.IVH_TAX_TC.ToString(hdfCurrencyFormat.Value);

                            //Edit 06_08_2014
                            //hdfOtherchargePO.Value = invoiceHeaderObj.IVH_SHIP_CHARGE.ToString(hdfCurrencyFormat.Value);
                            double RemainingOtherChrge = 0;
                            RemainingOtherChrge = invoiceHeaderObj.IVH_SHIP_CHARGE - invoiceHeaderObj.IVH_SHIP_CHARGE_INV;
                            hdfOtherchargePO.Value = RemainingOtherChrge.ToString(hdfCurrencyFormat.Value);
                            txtShipping.Text = invoiceHeaderObj.IVH_SHIP_CHARGE.ToString(hdfCurrencyFormat.Value);//while deduction alredy taken oc will show in txtDeductOtherCharges
                            //txtShipping.Text = RemainingOtherChrge.ToString(hdfCurrencyFormat.Value);
                            //End Edit
                            hdfSubDeptPk.Value = invoiceHeaderObj.IVH_ISSUE_DEPT.ToString();

                            txtPriceAdj.Text = invoiceHeaderObj.IVH_AMOUNT_ADJUST.ToString(hdfCurrencyFormat.Value);
                            txtHdrNetTotal.Text = invoiceHeaderObj.IVH_AMOUNT_NET_TC.ToString(hdfCurrencyFormat.Value);

                            if (invoiceHeaderObj.DeductionDetails != null && invoiceHeaderObj.DeductionDetails.Count > 0)
                            {
                                //while editing invoice with multiple advances allocated(with discount),the Discount Adjusted texbox getting wrong amount.
                                //txtDiscDeducted.Text = txtDiscDeducted.ToolTip = invoiceHeaderObj.DeductionDetails.Select(p => p.VAD_DISC_AMOUNT.ToString(hdfCurrencyFormat.Value)).FirstOrDefault();// totalAllocatedDiscount.ToString(hdfCurrencyFormat.Value);
                                txtDiscDeducted.Text = txtDiscDeducted.ToolTip = invoiceHeaderObj.DeductionDetails.Sum(p => p.VAD_DISC_AMOUNT).ToString(hdfCurrencyFormat.Value);// 
                                totalAllocatedDiscount = Convert.ToDecimal(txtDiscDeducted.Text);
                            }

                            txtSupplierInvNO.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_VENDOR_INV_NO);
                            txtCreditDays.Text = invoiceHeaderObj.IVH_CREDIT_DAYS;
                            txtTransport.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_TRANSPORT);
                            txtDeductOtherCharges.Text = string.IsNullOrEmpty(invoiceHeaderObj.IVH_SHIP_CHARGE_DED) ? "0.00" : Convert.ToDouble(invoiceHeaderObj.IVH_SHIP_CHARGE_DED).ToString(hdfCurrencyFormat.Value);
                            txtSupplierInvDate.Text = string.IsNullOrEmpty(invoiceHeaderObj.IVH_VENDOR_INV_DATE) ? string.Empty : Convert.ToDateTime(invoiceHeaderObj.IVH_VENDOR_INV_DATE).ToString(Resources.Constants.DateFormatShort);
                            ddlCompany.SelectedValue = invoiceHeaderObj.IVH_COMPANY.ToString();
                            chkOriginalinvoice.Checked = invoiceHeaderObj.IVH_ORGINAL_RCVD == (byte)1;

                            if (Convert.ToInt32(hdfIsMultiplePlant.Value) == 1)
                            {
                                lblCompanyView.Visible = true;
                                ddlCompanyView.Visible = true;
                                ddlCompanyView.Enabled = true;
                                ddlCompanyView.SelectedValue = invoiceHeaderObj.IVH_COMPANY.ToString();
                                ddlCompanyView.Enabled = false;
                            }
                            else
                            {
                                lblCompanyView.Visible = false;
                                ddlCompanyView.Visible = false;
                            }

                            //Declaration No
                            txtDeclarationNO.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_IMP_DECL_NO);


                            divVendorBranch.Visible = true;
                            vndPK = Convert.ToInt32(invoiceHeaderObj.IVH_VENDOR);

                            GetFieldValues(ControlsEnum.VENDORBRANCH);
                            SetFieldValues(ControlsEnum.VENDORBRANCH);
                            if (ddlVendorBranch.Items.Count > 0)
                            {
                                if (Convert.ToInt32(invoiceHeaderObj.IVH_VENDOR_CONTACT) > 0)
                                    ddlVendorBranch.SelectedValue = invoiceHeaderObj.IVH_VENDOR_CONTACT.ToString();
                            }
                            hdfPOItemType.Value = invoiceHeaderObj.POH_ITEM_TYPE.ToString();
                            //txtInvoiceDueDate.Text = (Convert.ToDateTime(invoiceHeaderObj.IVH_DATE).AddDays(Convert.ToInt16(invoiceHeaderObj.IVH_CREDIT_DAYS))).ToString(Resources.ErpRes.DateFormat);

                            GetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                            SetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                            if (CurrPK > 0)
                            {
                                //if (invoiceHeaderObj.IVH_BRANCH_TYPE > 0)
                                //    ddlAddressType.SelectedValue = invoiceHeaderObj.IVH_BRANCH_TYPE.ToString();
                                if (ddlAddressType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(invoiceHeaderObj.IVH_VENDOR_CONTACT) > 0)
                                        ddlAddressType.SelectedValue = invoiceHeaderObj.IVH_VENDOR_CONTACT.ToString();
                                }
                                SetBranchCodeVisibility();

                                txtVatTaxId.Text = string.IsNullOrEmpty(invoiceHeaderObj.IVH_TAX_ID) ? string.Empty : HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_TAX_ID.ToString());
                                txtBranchCode.Text = string.IsNullOrEmpty(invoiceHeaderObj.IVH_BRANCH_TEXT) ? string.Empty : invoiceHeaderObj.IVH_BRANCH_TEXT.ToString();
                                hdfVendorContactType.Value = invoiceHeaderObj.IVH_BRANCH_TYPE.ToString();

                                //if (Convert.ToInt32(ddlAddressType.SelectedValue) == (int)VendorContactTypeEnum.Branch)
                                //{
                                //    txtBranchCode.Enabled = true;
                                //    vrfBranchCode.Enabled = true;
                                //    txtBranchCode.CssClass = "";
                                //}
                                //else
                                //{
                                //    txtBranchCode.Enabled = false;
                                //    txtBranchCode.Text = "";
                                //    vrfBranchCode.Enabled = false;
                                //    txtBranchCode.CssClass = "input-disabled";
                                //}
                                txtInvoiceDueDate.Text = invoiceHeaderObj.IVH_DATE_PAY_BY;
                                ModifiedDatePnl.Visible = true;
                                LastModifiedTime = invoiceHeaderObj.LAST_MOD_DT;
                                lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                            }
                            else
                            {
                                DateTime dateForDueDateCaln;
                                int creditDays = string.IsNullOrEmpty(txtCreditDays.Text.Trim()) ? 0 : Convert.ToInt32(txtCreditDays.Text.Trim());
                                if (hdfInvDueDateDependsVenInvDate.Value == "1")
                                {
                                    if (DateTime.TryParse(txtSupplierInvDate.Text.Trim(), out dateForDueDateCaln))
                                    {
                                        DateTime dueDate = dateForDueDateCaln.AddDays(creditDays);
                                        txtInvoiceDueDate.Text = dueDate.ToString(Resources.Constants.DateFormatShort);
                                    }
                                }
                                else
                                {
                                    if (DateTime.TryParse(txtInvoiceDate.Text.Trim(), out dateForDueDateCaln))
                                    {
                                        DateTime dueDate = dateForDueDateCaln.AddDays(creditDays);
                                        txtInvoiceDueDate.Text = dueDate.ToString(Resources.Constants.DateFormatShort);
                                    }
                                }
                            }

                            #region manage header tax and discount

                            if (Convert.ToInt16(hdfisDiscountAdd.Value) == (int)TaxSettingEnum.ITEMWISE)
                            {
                                imgHdrDiscount.Visible = false;
                                txtHdrDiscount.ToolTip = txtHdrDiscount.Text = 0.ToString(hdfCurrencyFormat.Value);
                            }
                            if (Convert.ToInt16(hdfisTaxAdd.Value) == (int)TaxSettingEnum.ITEMWISE)
                            {
                                imgHdrTax.Visible = false;
                                txtHdrTax.ToolTip = txtHdrTax.Text = 0.ToString(hdfCurrencyFormat.Value);
                            }

                            #endregion

                            txtBillNo.Text = string.IsNullOrEmpty(invoiceHeaderObj.IVH_BILL_ENTRY_NO) ? string.Empty : invoiceHeaderObj.IVH_BILL_ENTRY_NO;
                            txtBillDate.Text = string.IsNullOrEmpty(invoiceHeaderObj.IVH_BILL_ENTRY_DATE) ? string.Empty : Convert.ToDateTime(invoiceHeaderObj.IVH_BILL_ENTRY_DATE).ToString(Resources.Constants.DateFormatShort);
                            txtBillAmount.Text = string.IsNullOrEmpty(invoiceHeaderObj.IVH_BILL_ENTRY_VALUE) ? string.Empty : GetFormattedCurrency(invoiceHeaderObj.IVH_BILL_ENTRY_VALUE);
                            txtPreparedBy.Text = string.IsNullOrEmpty(invoiceHeaderObj.PREPARED_BY) ? string.Empty : invoiceHeaderObj.PREPARED_BY;
                        }
                        break;
                    case ControlsEnum.UPLOADEDFILES:
                        CurrPK = invoiceHeaderObj.IVH_PK;
                        POUploadList = invoiceHeaderObj.FileList;
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

                case ControlsEnum.COMPANYSRCH:
                    ddlCompanySrch.Items.Clear();
                    if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    {
                        ddlCompanySrch.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                        ddlCompanySrch.DataTextField = Resources.DataFieldRes.CompanySpecsCode;
                        ddlCompanySrch.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompanySrch.DataBind();
                    }
                    ddlCompanySrch.Items.Insert(0, new ListItem(Resources.Report.SelectAll, CommonConstants.SELECTVAL));
                    break;

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
                    if (InvoiceStatus == InvoiceAction.SingleInvoice)// custom shows only for single PO                    
                    {
                        if (hdfTaxCategory.Value == ((int)TaxType.Tax).ToString())
                        {
                            if (IsCustomTaxEnabled)
                                ddlPopupTaxType.Items.Add(new ListItem(Resources.Report.Custom, CommonConstants.SELECTVAL));
                        }
                        else
                        {
                            ddlPopupTaxType.Items.Add(new ListItem(Resources.Report.Custom, CommonConstants.SELECTVAL));
                        }
                    }
                    else if (InvoiceStatus == InvoiceAction.MultipleInvoice || InvoiceStatus == InvoiceAction.ListInvoice)
                    {
                        if (POInvoiceHeaderSession != null && POInvoiceHeaderSession.OtherChargeDetails != null)
                        {
                            if (POInvoiceHeaderSession.OtherChargeDetails.Count == 1)// custom shows only for single PO
                            {
                                if (hdfTaxCategory.Value == ((int)TaxType.Tax).ToString())
                                {
                                    if (IsCustomTaxEnabled)
                                        ddlPopupTaxType.Items.Add(new ListItem(Resources.Report.Custom, CommonConstants.SELECTVAL));
                                }
                                else
                                {
                                    ddlPopupTaxType.Items.Add(new ListItem(Resources.Report.Custom, CommonConstants.SELECTVAL));
                                }
                            }
                        }
                    }
                    break;
                case ControlsEnum.POTYPE:
                    ddlOrderType.Items.Clear();
                    if (dtPageData != null)
                    {
                        ddlOrderType.DataSource = dtPageData;
                        ddlOrderType.DataTextField = "CFG_DATA";
                        ddlOrderType.DataValueField = "CFG_VALUE";
                        ddlOrderType.DataBind();
                    }
                    ddlOrderType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.GSTINVOICETYPE:
                    ddlGSTInvoiceType.Items.Clear();
                    if (dtPageData != null)
                    {
                        ddlGSTInvoiceType.DataSource = dtPageData;
                        ddlGSTInvoiceType.DataTextField = "FTM_CODE";
                        ddlGSTInvoiceType.DataValueField = "FTM_PK";
                        ddlGSTInvoiceType.DataBind();
                    }
                    ddlGSTInvoiceType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.PURCHASEORDERTYPE:
                    ddlPOType.Items.Clear();
                    if (dtPageData != null)
                    {
                        ddlPOType.DataSource = dtPageData;
                        ddlPOType.DataTextField = "CFG_DATA";
                        ddlPOType.DataValueField = "CFG_VALUE";
                        ddlPOType.DataBind();
                    }
                    ddlPOType.Items.Insert(0, new ListItem(Resources.ErpRes.All_Small, CommonConstants.SELECTVAL));
                    break;
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
                    case ControlsEnum.POINVDETAIL:
                        if (invoiceHeaderObj != null)
                        {
                            soInvoiceDetailsList = new List<POInvoiceDetails>();
                            soInvoiceDetailsList = invoiceHeaderObj.OrderDetail;
                            IvhGroup = invoiceHeaderObj.IVH_GROUP;
                            if (soInvoiceDetailsList != null)
                            {
                                grdInvoice.DataSource = soInvoiceDetailsList;
                                grdInvoice.DataBind();
                            }
                        }
                        break;
                    case ControlsEnum.TAXPOPUPGRID:

                        if (IsHeaderTax)
                        {
                            //taxHdrList = TempPOInvoiceHeaderSession.TaxHdr.Where(tax => tax.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value)).ToList();

                            taxHdrList = TempPOInvoiceHeaderSession.TaxHdr.Where(tax => tax.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value)).GroupBy(test => test.VTL_TAX).Select(grp => grp.First()).ToList();
                            // taxHdrList = POInvoiceHdrSession.TaxHdr;

                        }
                        else
                        {
                            soDtlObj = TempPOInvoiceHeaderSession.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.VID_PO) == SelectedPOPK);
                            if (soDtlObj != null)
                            {
                                taxHdrList = soDtlObj.TaxDtl.Where(tax => tax.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                                //taxHdrList = soDtlObj.TaxDtl.Where(tax => tax.VTL_INVOICE_DTL == POInvoicePK && tax.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                            }
                        }
                        grdTaxDetails.DataSource = taxHdrList;
                        grdTaxDetails.DataBind();
                        break;
                    case ControlsEnum.INVOICELIST:
                        if (dtInvoiceList != null)
                        {
                            GetFieldValues(ControlsEnum.FILLWORKFLOWSTATUS);
                            PageIndex = PageIndex == null ? "0" : PageIndex;
                            grdInvoiceList.PageIndex = Convert.ToInt32(PageIndex);
                            grdInvoiceList.DataSource = dtInvoiceList.DefaultView;
                            grdInvoiceList.DataBind();

                            //For Setting/Resetting Colour of a selected InvoiceNo
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                            //End
                        }
                        break;
                    case ControlsEnum.DEDUCTIONPOPUPGRID:
                        deductionDtlList = new List<POAdvDeductionDetails>();
                        deductionDtlList = TempPOInvoiceHeaderSession.DeductionDetails.ToList();
                        DedTotalAllocateNowFooterSplit = 0;
                        OtherAmountFooterSplit = 0;
                        TaxFooterSplit = 0;
                        if (deductionDtlList != null && deductionDtlList.Count > 0)
                        {
                            grdDeduction.DataSource = deductionDtlList;
                            grdDeduction.DataBind();
                            Label lblDedTotalAllocateNowFooterSplit = grdDeduction.FooterRow.FindControl("lblDedTotalAllocateNowFooterSplit") as Label;
                            HiddenField hdfDedTotalAllocateNowFooterSplit = grdDeduction.FooterRow.FindControl("hdfDedTotalAllocateNowFooterSplit") as HiddenField;
                            Label lblOtherAmountFooterSplit = grdDeduction.FooterRow.FindControl("lblOtherAmountFooterSplit") as Label;
                            Label lblTaxFooterSplit = grdDeduction.FooterRow.FindControl("lblTaxFooterSplit") as Label;
                            HiddenField hdfOtherTotalFooterSplit = grdDeduction.FooterRow.FindControl("hdfOtherTotalFooterSplit") as HiddenField;
                            HiddenField hdfTaxTotalFooterSplit = grdDeduction.FooterRow.FindControl("hdfTaxTotalFooterSplit") as HiddenField;

                            if (lblDedTotalAllocateNowFooterSplit != null && hdfDedTotalAllocateNowFooterSplit != null)
                            {
                                decimal total = deductionDtlList.Sum(aa => aa.VAD_AMOUNT);
                                if (total == 0)
                                {
                                    //total = deductionDtlList.Sum(aa => aa.IVH_AMOUNT_TC - aa.IVH_AMOUNT_ALLOCATED);
                                    total = deductionDtlList.Sum(aa => Convert.ToDecimal(aa.PVH_PO_PAID_AMT) - aa.IVH_AMOUNT_ALLOCATED);
                                }
                                decimal totalamnt = 0;
                                foreach (GridViewRow grvrow in grdDeduction.Rows)
                                {
                                    decimal amnt = 0;
                                    TextBox txtDedAllocateNowSplit = grvrow.FindControl("txtDedAllocateNowSplit") as TextBox;
                                    decimal.TryParse(txtDedAllocateNowSplit.Text, out amnt);
                                    totalamnt += amnt;

                                }
                                hdfDedTotalAllocateNowFooterSplit.Value = lblDedTotalAllocateNowFooterSplit.Text = totalamnt.ToString(hdfCurrencyFormat.Value);
                                //hdfDedTotalAllocateNowFooterSplit.Value = lblDedTotalAllocateNowFooterSplit.Text = total.ToString(hdfCurrencyFormat.Value);
                            }
                        }
                        else
                        {
                            grdDeduction.DataSource = deductionDtlList;
                            grdDeduction.DataBind();
                        }
                        break;
                    case ControlsEnum.UPLOADEDFILES:
                        if (POUploadList != null)
                        {
                            grdUploads.DataSource = POUploadList.Where(row => row.DOC_IS_GRN == 0 && row.DOC_STATUS == 1);
                            grdUploads.DataBind();
                        }
                        break;

                    #region OTHERCHARGELIST
                    case ControlsEnum.OTHERCHARGELIST:
                        POTotalOtherAmount = 0;
                        POTotalInvOtherAmount = 0;
                        POTotalBalanceOtherAmount = 0;
                        if (POInvoiceHeaderSession != null)
                        {
                            poOtherChargeList = new List<POOtherChargeDetails>();
                            poOtherChargeList = POInvoiceHeaderSession.OtherChargeDetails;
                            POTotalOtherAmount = poOtherChargeList.Sum(toa => toa.PO_OTHER_AMOUNT);
                            POTotalInvOtherAmount = poOtherChargeList.Sum(tioa => tioa.PO_OTHER_AMOUNT_INVOICED);
                            POTotalBalanceOtherAmount = POTotalOtherAmount - POTotalInvOtherAmount;//poOtherChargeList.Sum(tba => tba.IVM_OTHER_AMOUNT);
                            grdOtherchargeSplit.DataSource = poOtherChargeList;
                            grdOtherchargeSplit.DataBind();
                        }
                        else
                        {
                            grdOtherchargeSplit.DataSource = null;
                            grdOtherchargeSplit.DataBind();
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

                    #region POLIST
                    case ControlsEnum.POLIST:
                        grdNewPOList.DataSource = dtPOList;
                        grdNewPOList.DataBind();
                        break;
                    #endregion

                    #region GRNQTYSPLIT
                    case ControlsEnum.GRNQTYSPLIT:
                        POTotalGRNQty = 0;
                        POTotalGRNInvdQty = 0;
                        GRTotalDmgQty = 0;
                        POTotalGRNBalance = 0;
                        if (POInvoiceHeaderSession != null)
                        {
                            grnList = new List<GRNQTYDetails>();
                            invoiceHeaderObj = POInvoiceHeaderSession;
                            soDtlObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.VID_PO) == SelectedPOPK);
                            if (soDtlObj != null)
                            {
                                grnList = soDtlObj.GRNDtl.ToList();
                            }
                            POTotalGRNQty = grnList.Sum(grn => grn.GRD_QTY_APPROVED);
                            POTotalGRNInvdQty = grnList.Sum(bal => bal.GRN_QTY_INVOICED);
                            GRTotalDmgQty = grnList.Sum(bal => bal.VGL_DMG_QTY);
                            POTotalGRNBalance = POTotalGRNQty - POTotalGRNInvdQty;
                            grdGRNDetails.DataSource = grnList;
                            grdGRNDetails.DataBind();
                        }
                        else
                        {
                            grdGRNDetails.DataSource = null;
                            grdGRNDetails.DataBind();
                        }
                        break;
                    #endregion

                    #region GRNATTACHMENTS
                    case ControlsEnum.GRNATTACHMENTS:
                        grdGrnAttchments.DataSource = POUploadList.Where(row => row.DOC_IS_GRN == 1 && row.DOC_STATUS == 1);
                        grdGrnAttchments.DataBind();
                        break;
                    #endregion

                    #region PO COST CENTER
                    case ControlsEnum.COSTCENTERDTL:
                        if (poCostCenterlst != null && poCostCenterlst.Count > 0)
                        {
                            grdCostCenterlist.DataSource = poCostCenterlst;
                            grdCostCenterlist.DataBind();
                        }
                        else
                        {
                            grdCostCenterlist.DataSource = null;
                            grdCostCenterlist.DataBind();
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
                case ControlsEnum.TAXPOPUPGRID:
                    txtPopupAmount.Text = string.Empty;
                    txtPopupItemAmount.Text = string.Empty;
                    txtPopupOther.Text = string.Empty;
                    TaxPK = 0;
                    grdTaxDetails.DataSource = null;
                    grdTaxDetails.DataBind();
                    TempPOInvoiceHeaderSession = null;
                    hdfTaxFormula.Value = string.Empty;
                    POInvoicePK = 0;
                    SelectedItemPK = 0;
                    hdfTaxCategory.Value = string.Empty;
                    break;
                case ControlsEnum.POINVHEADER:
                    CurrPK = 0;
                    break;
                case ControlsEnum.INVOICELIST:
                    hdfIscontYesDate.Value = "0";
                    CurrPK = 0;
                    CurrPOPK = 0;
                    hasValidRate = false;
                    txtInvoiceNumber.Text = "Select/Type";
                    txtCustomer.Text = "Select/Type";
                    hdfIVHPK.Value = "";
                    txtpoNo.Text = string.Empty;
                    hdfCustomerID.Value = "";
                    chkOriginalinvoice.Checked = false;
                    // txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    //hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    //txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    //hdfToDate.Value = DateTime.Now.ToString();
                    txtFromDate.Text = string.Empty;
                    txtGrnNo.Text = string.Empty;
                    hdfFromDate.Value = string.Empty;
                    txtToDate.Text = string.Empty;
                    hdfToDate.Value = string.Empty;
                    ddlStatus.ClearSelection();
                    chkPending.Checked = true;
                    ddlOrderType.ClearSelection();
                    ModifiedDatePnl.Visible = false;
                    FileDetailsList = null;
                    POInvoiceHeaderSession = null;
                    POUploadList = null;
                    InvoiceMultiplePOPKs = null;
                    InvoiceMultiplePOPKsNew = null;
                    base.WkfRefID = 0;
                    DedTotalAllocateNowFooterSplit = 0;
                    OtherAmountFooterSplit = 0;
                    TaxFooterSplit = 0;
                    POTotalOtherAmount = 0;
                    POTotalInvOtherAmount = 0;
                    POTotalBalanceOtherAmount = 0;
                    POTotalOtherAmount = 0;
                    POTotalInvOtherAmount = 0;
                    POTotalBalanceOtherAmount = 0;
                    txtDueAson.Text = string.Empty;
                    ddlCompanySrch.SelectedValue = CommonConstants.SELECTVAL;
                    ResetForm(ControlsEnum.ADDITEM);
                    hdfSubDeptPk.Value = "0";
                    txtSCNo.Text = string.Empty;
                    ddlPOType.ClearSelection();
                    ddlConvertTo.ClearSelection();
                    break;
                case ControlsEnum.ADDITEM:
                    //ddlType.ClearSelection();
                    anchorFile.Visible = false;
                    vrfFileUpload.Enabled = true;
                    CurrSlNo = 0;
                    anchorFile.Attributes.Remove("onclick");
                    break;
                case ControlsEnum.RESETPAYMENT:
                    SelectedVendors = 0;
                    SelectedCurrency = 0;
                    InvCategory = 0;
                    SelectedInvoicesCrDr = null;
                    SelectedInvoices = null;
                    SelectedINVTax = null;
                    SelectedINVTaxList = new List<decimal>();
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

        private void SetDetailTax(object sender)
        {
            TextBox txtAmount;
            TextBox txtDiscount;
            TextBox txtTax;
            TextBox txtTotal;

            TextBox txtRate;
            TextBox txtQuantity;

            Label lblOrderQuantity;
            Label lblInvQuantity;

            HiddenField hdfRRDPK;
            HiddenField hdfItemPK;
            HiddenField hdfPOPK;

            double quantity;
            double rate;
            quantity = 0;
            rate = 0;

            double soQty = 0;
            double invdQty = 0;

            if (sender != null)
            {
                txtRate = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtRate") as TextBox);
                txtQuantity = sender as TextBox;
                lblOrderQuantity = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("lblOrderQuantity") as Label);
                lblInvQuantity = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("lblInvQuantity") as Label);

                txtAmount = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtAmount") as TextBox);
                if (txtRate != null && txtQuantity != null)
                {
                    if (double.TryParse(txtRate.Text, out rate) && double.TryParse(txtQuantity.Text, out quantity))
                    {
                        if (!(double.TryParse(lblOrderQuantity.Text, out soQty) && double.TryParse(lblInvQuantity.Text, out invdQty) && soQty - invdQty >= rate))
                        {
                            //txtQuantity.Text = CommonConstants.SELECT_VALUE_ZERO;
                            //rate = 0;
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_InvQty").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        if (txtAmount != null)
                        {
                            txtAmount.Text = txtAmount.ToolTip = Math.Round((rate * quantity), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                            txtDiscount = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtDiscount") as TextBox);
                            txtTax = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtTax") as TextBox);
                            txtTotal = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtTotal") as TextBox);

                            hdfRRDPK = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("hdfInvoiceDtlPK") as HiddenField);
                            hdfItemPK = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                            hdfPOPK = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("hdfSODtlPK") as HiddenField);
                            if (hdfRRDPK != null && hdfItemPK != null && hdfPOPK != null)
                            {
                                POInvoicePK = string.IsNullOrEmpty(hdfRRDPK.Value) ? 0 : Convert.ToInt32(hdfRRDPK.Value);
                                SelectedItemPK = string.IsNullOrEmpty(hdfItemPK.Value) ? 0 : Convert.ToInt32(hdfItemPK.Value);
                                SelectedPOPK = string.IsNullOrEmpty(hdfPOPK.Value) ? 0 : Convert.ToInt32(hdfPOPK.Value);
                                SetDetailTax(txtAmount, txtDiscount, txtTax, txtTotal);
                            }
                        }
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Rate_Greater_Discount").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                    }
                }
            }
            else if (POInvoicePK == 0 && SelectedItemPK == 0)
            {
                POInvoiceHeaderSession = TempPOInvoiceHeaderSession;
                foreach (GridViewRow gvr in grdInvoice.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        hdfRRDPK = (gvr.FindControl("hdfInvoiceDtlPK") as HiddenField);
                        hdfItemPK = (gvr.FindControl("hdfItemPK") as HiddenField);
                        hdfPOPK = (gvr.FindControl("hdfSODtlPK") as HiddenField);
                        POInvoicePK = Convert.ToInt32(hdfRRDPK.Value);
                        SelectedItemPK = Convert.ToInt32(hdfItemPK.Value);
                        SelectedPOPK = Convert.ToInt32(hdfPOPK.Value);

                        txtRate = (gvr.FindControl("txtRate") as TextBox);
                        txtQuantity = (gvr.FindControl("txtInvNow") as TextBox);
                        txtAmount = (gvr.FindControl("txtAmount") as TextBox);
                        txtDiscount = (gvr.FindControl("txtDiscount") as TextBox);
                        txtTax = (gvr.FindControl("txtTax") as TextBox);
                        txtTotal = (gvr.FindControl("txtTotal") as TextBox);

                        if (txtRate != null && txtQuantity != null && txtAmount != null
                            && double.TryParse(txtRate.Text, out rate) && double.TryParse(txtQuantity.Text, out quantity))
                        {
                            txtAmount.Text = txtAmount.ToolTip = Math.Round((rate * quantity), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                        }
                        SetDetailTax(txtAmount, txtDiscount, txtTax, txtTotal);
                    }
                }
                POInvoicePK = 0;
                SelectedItemPK = 0;
                SelectedPOPK = 0;
            }
            else
            {
                POInvoiceHeaderSession = TempPOInvoiceHeaderSession;
                foreach (GridViewRow gvr in grdInvoice.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        hdfRRDPK = (gvr.FindControl("hdfInvoiceDtlPK") as HiddenField);
                        hdfItemPK = (gvr.FindControl("hdfItemPK") as HiddenField);
                        hdfPOPK = (gvr.FindControl("hdfSODtlPK") as HiddenField);
                        if (POInvoicePK == Convert.ToInt32(hdfRRDPK.Value) && SelectedItemPK == Convert.ToInt32(hdfItemPK.Value) && SelectedPOPK == Convert.ToInt32(hdfPOPK.Value))
                        {
                            txtAmount = (gvr.FindControl("txtAmount") as TextBox);
                            txtDiscount = (gvr.FindControl("txtDiscount") as TextBox);
                            txtTax = (gvr.FindControl("txtTax") as TextBox);
                            txtTotal = (gvr.FindControl("txtTotal") as TextBox);
                            if (!SetDetailTax(txtAmount, txtDiscount, txtTax, txtTotal))
                                return;
                        }
                    }
                }
            }
            SetSubTotal();
        }
        private bool SetDetailTax(TextBox txtAmount, TextBox txtDiscount, TextBox txtTax, TextBox txtTotal)
        {
            double amount;
            double discount;
            double itmTax;
            double netAmount;
            amount = 0;
            discount = 0;
            netAmount = 0;
            itmTax = 0;
            if (txtAmount != null && txtDiscount != null && txtTax != null && txtTotal != null)
            {
                Double.TryParse(txtAmount.Text.Trim(), out amount);
                if (amount >= 0)
                {
                    if (POInvoiceHeaderSession != null)
                    {
                        invoiceHeaderObj = POInvoiceHeaderSession;
                        soInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.VID_PO) == SelectedPOPK);
                        if (soInvoiceDetailsObj != null)
                        {
                            var discDetail = soInvoiceDetailsObj.TaxDtl.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Discount));
                            foreach (POInvoiceTaxHdr rfqTaxHdrObj in discDetail)
                            {
                                string taxFormula = rfqTaxHdrObj.VTL_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    int isDeducted = 0;
                                    if (invoiceHeaderObj.DeductionDetails != null || invoiceHeaderObj.DeductionDetails.Count > 0)
                                    {
                                        //calculate tax if advance not deducted
                                        for (int i = 0; i < invoiceHeaderObj.DeductionDetails.Count; i++)
                                        {
                                            if (invoiceHeaderObj.DeductionDetails[i].VAD_DISC_AMOUNT > 0)
                                            {
                                                isDeducted = isDeducted + 1;
                                            }
                                        }
                                    }
                                    if (isDeducted <= 0)
                                    {
                                        taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                                        rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits, MidpointRounding.AwayFromZero);
                                    }
                                }
                                else //In case of Custom Discount it doesnot have formula. So we create a formula . 
                                {
                                    //Formula :InvoiceNowDiscount= PODiscAmt / PO Qty * InvoiceNowQty;
                                    rfqTaxHdrObj.VTL_TAX_AMT = (soInvoiceDetailsObj.VID_ORDERED_DISC / soInvoiceDetailsObj.VID_ORDERED_QTY) * soInvoiceDetailsObj.VID_QTY_INVOICED;
                                }
                            }


                            discount = soInvoiceDetailsObj.TaxDtl.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(rfq => rfq.VTL_TAX_AMT);
                            // discount = Math.Round(StringToFormula(Convert.ToDouble(discount).ToString()), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            discount = Convert.ToDouble(ERP.Utilities.CommonFunctions.DoubleFormatRound(discount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1).ToString(hdfCurrencyFormat.Value));
                            netAmount = amount - discount;
                            txtDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);

                            var taxDetail = soInvoiceDetailsObj.TaxDtl.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Tax));
                            foreach (POInvoiceTaxHdr rfqTaxHdrObj in taxDetail)
                            {
                                string taxFormula = rfqTaxHdrObj.VTL_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    int isDeducted = 0;
                                    if (invoiceHeaderObj.DeductionDetails != null || invoiceHeaderObj.DeductionDetails.Count > 0)
                                    {
                                        //calculate tax if advance not deducted
                                        for (int i = 0; i < invoiceHeaderObj.DeductionDetails.Count; i++)
                                        {
                                            if (invoiceHeaderObj.DeductionDetails[i].VAD_TAX_AMOUNT > 0)
                                            {
                                                isDeducted = isDeducted + 1;
                                            }
                                        }
                                    }
                                    if (isDeducted <= 0)
                                    {
                                        taxFormula = taxFormula.Replace("#SUBTOTAL#", netAmount.ToString());
                                        rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                    }

                                }
                                else //In case of Custom Tax it doesnot have formula. So we create a formula . 
                                {
                                    //Formula :InvoiceNowTax= POTaxAmt / PO Qty * InvoiceNowQty;
                                    rfqTaxHdrObj.VTL_TAX_AMT = (soInvoiceDetailsObj.VID_ORDERED_TAX / soInvoiceDetailsObj.VID_ORDERED_QTY) * soInvoiceDetailsObj.VID_QTY_INVOICED;
                                }

                            }
                            itmTax = soInvoiceDetailsObj.TaxDtl.ToList().Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(rfq => rfq.VTL_TAX_AMT);
                            itmTax = Convert.ToDouble(ERP.Utilities.CommonFunctions.DoubleFormatRound(itmTax, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1).ToString(hdfCurrencyFormat.Value));
                            txtTax.ToolTip = txtTax.Text = itmTax.ToString(hdfCurrencyFormat.Value);
                            soInvoiceDetailsObj.VID_AMOUNT = amount;
                            soInvoiceDetailsObj.VID_DISCOUNT = discount;
                            soInvoiceDetailsObj.VID_TAX = itmTax;
                            soInvoiceDetailsObj.VID_NET_AMOUNT = (amount - discount + itmTax);
                            soInvoiceDetailsObj.VID_NET_AMOUNT = Convert.ToDouble(ERP.Utilities.CommonFunctions.DoubleFormatRound(soInvoiceDetailsObj.VID_NET_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1).ToString(hdfCurrencyFormat.Value));
                            txtTotal.ToolTip = txtTotal.Text = soInvoiceDetailsObj.VID_NET_AMOUNT.ToString(hdfCurrencyFormat.Value);
                            POInvoiceHeaderSession = invoiceHeaderObj;
                        }
                    }
                    return true;
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Rate_Greater_Discount").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool ReSetHdrDisc()
        {
            if (POInvoiceHeaderSession != null)
            {
                double amount;
                double discount;
                discount = 0;
                //reset Header Disc

                amount = POInvoiceHeaderSession.OrderDetail.Sum(dtl => dtl.VID_NET_AMOUNT);
                discount = 0;
                var discHeader = invoiceHeaderObj.TaxHdr.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Discount));

                foreach (POInvoiceTaxHdr rfqTaxHdrObj in discHeader)
                {
                    string taxFormula = rfqTaxHdrObj.VTL_TAX_FORMULA;
                    if (!string.IsNullOrEmpty(taxFormula))
                    {

                        taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                        //rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);//old code commented on 10/12/2024
                        rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits, MidpointRounding.AwayFromZero);

                    }
                }
                discount = invoiceHeaderObj.TaxHdr.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(rfq => rfq.VTL_TAX_AMT);
                invoiceHeaderObj.IVH_DISCOUNT_TC = discount;

                txtHdrDiscount.ToolTip = txtHdrDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);
                POInvoiceHeaderSession = invoiceHeaderObj;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ReSetDetailTax(double amount)
        {

            double discount;
            double itmTax;
            double netAmount;
            //amount = 0;
            discount = 0;
            netAmount = 0;
            itmTax = 0;
            if (amount > 0)
            {

                if (amount >= 0)
                {
                    if (POInvoiceHeaderSession != null)
                    {

                        invoiceHeaderObj = POInvoiceHeaderSession;
                        soInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.VID_PO) == SelectedPOPK);
                        if (soInvoiceDetailsObj != null)
                        {
                            var discDetail = soInvoiceDetailsObj.TaxDtl.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Discount));
                            foreach (POInvoiceTaxHdr rfqTaxHdrObj in discDetail)
                            {
                                string taxFormula = rfqTaxHdrObj.VTL_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                                    rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits, MidpointRounding.AwayFromZero);
                                }
                            }
                            discount = soInvoiceDetailsObj.TaxDtl.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(rfq => rfq.VTL_TAX_AMT);
                            netAmount = amount - discount;
                            //txtDiscount.ToolTip = txtDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);

                            var taxDetail = soInvoiceDetailsObj.TaxDtl.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Tax));
                            foreach (POInvoiceTaxHdr rfqTaxHdrObj in taxDetail)
                            {
                                string taxFormula = rfqTaxHdrObj.VTL_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {

                                    if (invoiceHeaderObj.DeductionDetails != null || invoiceHeaderObj.DeductionDetails.Count > 0)
                                    {

                                        taxFormula = taxFormula.Replace("#SUBTOTAL#", netAmount.ToString());
                                        rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);

                                    }
                                }
                            }
                            itmTax = soInvoiceDetailsObj.TaxDtl.ToList().Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(rfq => rfq.VTL_TAX_AMT);
                            soInvoiceDetailsObj.VID_AMOUNT = amount;
                            soInvoiceDetailsObj.VID_DISCOUNT = discount;
                            soInvoiceDetailsObj.VID_TAX = itmTax;
                            soInvoiceDetailsObj.VID_NET_AMOUNT = (amount - discount + itmTax);
                            POInvoiceHeaderSession = invoiceHeaderObj;
                        }
                    }
                    return true;
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Rate_Greater_Discount").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void SetTaxPayableDiv()
        {

            List<POInvoiceDetails> soInvoiceDetailsTaxPayableLst = new List<POInvoiceDetails>();
            POInvoiceHeader invoiceHeaderObjTaxPayable;
            List<POInvoiceTaxHdr> LstTaxPayable = new List<POInvoiceTaxHdr>();

            if (POInvoiceHeaderSession != null)
            {
                invoiceHeaderObjTaxPayable = POInvoiceHeaderSession;
                soInvoiceDetailsTaxPayableLst = invoiceHeaderObjTaxPayable.OrderDetail;
                //Adding Line Item Tax
                if (soInvoiceDetailsTaxPayableLst != null)
                {
                    foreach (var item in soInvoiceDetailsTaxPayableLst)
                    {
                        double taxapplyAmount = 0;
                        taxapplyAmount = item.VID_AMOUNT - item.VID_DISCOUNT;
                        var taxDetail = item.TaxDtl.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Tax));
                        foreach (POInvoiceTaxHdr rfqTaxHdrObj in taxDetail)
                        {
                            //assign taxapplyamount to VTL_TAX_VID_AMOUNT for only showing in grid under subtotal heading
                            rfqTaxHdrObj.VTL_TAX_VID_AMOUNT = taxapplyAmount.ToString(hdfCurrencyFormat.Value);
                            LstTaxPayable.Add(rfqTaxHdrObj);
                        }
                    }

                }
                //Adding Header tax
                var HeaderTax = invoiceHeaderObjTaxPayable.TaxHdr.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Tax));
                if (HeaderTax != null)
                {
                    string taxapplyAmount = "";
                    if (IsTaxForOtherCharge.Value == "1")  //Is Othercharge is need for Tax Calulation
                    {
                        double HdrBalBeforeVat;
                        double OtherCharge = string.IsNullOrEmpty(txtShipping.Text.Trim()) ? 0 : Convert.ToDouble(txtShipping.Text.Trim());
                        double OtherChargeDeduct = string.IsNullOrEmpty(txtDeductOtherCharges.Text.Trim()) ? 0 : Convert.ToDouble(txtDeductOtherCharges.Text.Trim());
                        HdrBalBeforeVat = string.IsNullOrEmpty(txtHdrBalBeforeVat.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrBalBeforeVat.Text) + (OtherCharge - OtherChargeDeduct);
                        taxapplyAmount = HdrBalBeforeVat.ToString(hdfCurrencyFormat.Value);
                    }
                    else
                    {
                        taxapplyAmount = string.IsNullOrEmpty(txtHdrBalBeforeVat.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtHdrBalBeforeVat.Text).ToString(hdfCurrencyFormat.Value);
                    }
                    foreach (POInvoiceTaxHdr rfqTaxHdrObj in HeaderTax)
                    {
                        rfqTaxHdrObj.VTL_TAX_VID_AMOUNT = taxapplyAmount;
                        LstTaxPayable.Add(rfqTaxHdrObj);
                    }
                }

            }

            if (LstTaxPayable.Count > 0)
            {
                var groupedTaxPayableList = LstTaxPayable.GroupBy(f => f.VTL_TAX)
                    .Select(grp => new POInvoiceTaxHdr
                    {
                        VTL_TAX_AMT = grp.Sum(p => p.VTL_TAX_AMT),
                        VTL_NAME = grp.Min(p => p.VTL_NAME),
                        VTL_TAX_TEXT = grp.Min(p => p.VTL_TAX_TEXT),
                        VTL_PK = grp.Min(p => p.VTL_PK),
                        VTL_TAX_CODE = grp.Min(p => p.VTL_TAX_CODE),
                        VTL_TAX_RATE = grp.Min(p => p.VTL_TAX_RATE),
                        VTL_TAX_VID_AMOUNT = grp.Sum(p => Convert.ToDecimal(p.VTL_TAX_VID_AMOUNT)).ToString()

                    })
                   .ToList();

                //Removing Items have Zero taxAmount
                groupedTaxPayableList = groupedTaxPayableList.Where(f => f.VTL_TAX_AMT > 0).ToList();

                grdTaxPayable.DataSource = groupedTaxPayableList;
                grdTaxPayable.DataBind();
            }
            else
            {
                grdTaxPayable.DataSource = null;
                grdTaxPayable.DataBind();
            }
        }

        private void SetSubTotal()
        {
            TextBox txtSubTotalFooter;
            TextBox txtAdjustAmountFooter;
            if (grdInvoice.FooterRow != null)
            {
                txtSubTotalFooter = grdInvoice.FooterRow.FindControl("txtSubTotalFooter") as TextBox;
                txtAdjustAmountFooter = grdInvoice.FooterRow.FindControl("txtAdjustAmountFooter") as TextBox;
                if (txtSubTotalFooter != null && POInvoiceHeaderSession != null)
                {
                    POInvoiceHeaderSession.IVH_AMOUNT_TC = POInvoiceHeaderSession.OrderDetail.Sum(dtl => dtl.VID_NET_AMOUNT);
                    txtSubTotalFooter.ToolTip = txtSubTotalFooter.Text = POInvoiceHeaderSession.IVH_AMOUNT_TC.ToString(hdfCurrencyFormat.Value);
                    txtAdjustAmountFooter.ToolTip = txtAdjustAmountFooter.Text = POInvoiceHeaderSession.IVH_AMOUNT_NET_TC_ADJ.ToString(hdfCurrencyFormat.Value);
                    double TotalPoValue = (POInvoiceHeaderSession.POH_TOTAL_VALUE_TC - POInvoiceHeaderSession.POH_PRICE_ADJUST_TOTAL) <= 0 ? 1 : (POInvoiceHeaderSession.POH_TOTAL_VALUE_TC - POInvoiceHeaderSession.POH_PRICE_ADJUST_TOTAL);
                    txtPriceAdj.Text = (((POInvoiceHeaderSession.POH_PRICE_ADJUST_TOTAL) / (TotalPoValue)) * POInvoiceHeaderSession.IVH_AMOUNT_TC).ToString(hdfCurrencyFormat.Value); //POInvoiceHeaderSession.IVH_AMOUNT_NET_TC_ADJ.ToString(hdfCurrencyFormat.Value);
                    ValidateAdjAmount();
                    decimal subTotal = Convert.ToDecimal(txtSubTotalFooter.Text);
                    decimal adjamt = Convert.ToDecimal(txtAdjustAmountFooter.Text);
                    decimal hdrDiscount = string.IsNullOrEmpty(txtHdrDiscount.Text) ? 0 : Convert.ToDecimal(txtHdrDiscount.Text);
                    double totalTax = 0;
                    double tax;
                    foreach (GridViewRow grdRow in grdInvoice.Rows)
                    {
                        TextBox txtTax = grdRow.FindControl("txtTax") as TextBox;
                        if (txtTax != null)
                        {
                            tax = 0;
                            Double.TryParse(txtTax.Text, out tax);
                            totalTax += tax;
                        }
                    }
                    //16_10_2014
                    // subTotal -= Convert.ToDecimal(totalTax);
                    txtHdrTotal.Text = ((subTotal + adjamt) - hdrDiscount - totalAllocatedDiscount).ToString(hdfCurrencyFormat.Value);
                    //txtHdrTotal.Text = ((subTotal) - hdrDiscount - totalAllocatedDiscount).ToString(hdfCurrencyFormat.Value);
                    if (IsAdvInvHasTax)
                    {
                        decimal hdrDeduction = string.IsNullOrEmpty(txtHdrDeduction.Text) ? 0 : Convert.ToDecimal(txtHdrDeduction.Text);
                        // txtHdrBalBeforeVat.Text = ((subTotal - hdrDiscount) - hdrDeduction).ToString(hdfCurrencyFormat.Value);
                        txtHdrBalBeforeVat.Text = ((((subTotal + adjamt) - hdrDiscount) - hdrDeduction) - totalAllocatedDiscount).ToString(hdfCurrencyFormat.Value);
                        //txtHdrBalBeforeVat.Text = ((((subTotal) - hdrDiscount) - hdrDeduction) - totalAllocatedDiscount).ToString(hdfCurrencyFormat.Value);
                    }
                    else
                    {
                        //txtHdrBalBeforeVat.Text = (((subTotal + adjamt) - hdrDiscount)).ToString(hdfCurrencyFormat.Value);
                        txtHdrBalBeforeVat.Text = (((subTotal) - hdrDiscount)).ToString(hdfCurrencyFormat.Value);
                    }
                }
            }
        }
        private void SetLineItemTax()
        {
            GetFieldValues(ControlsEnum.LINETAX);
            SetFieldValues(ControlsEnum.LINETAX);

        }

        private bool SetHdrTax()
        {
            //TextBox txtSubTotal;
            double amount;
            double discount;
            double shipping;
            double adjust;
            double balBeforeVat;
            double otherCharges = 0;
            amount = 0;
            shipping = 0;
            adjust = 0;
            balBeforeVat = 0;
            double _PrevtotalAmount = 0;
            if (POInvoiceHeaderSession != null)
            {
                invoiceHeaderObj = POInvoiceHeaderSession;
                amount = invoiceHeaderObj.IVH_AMOUNT_TC + invoiceHeaderObj.IVH_AMOUNT_NET_TC_ADJ;
                discount = 0;
                _PrevtotalAmount = invoiceHeaderObj.IVH_AMOUNT_TC + invoiceHeaderObj.IVH_AMOUNT_NET_TC_ADJ - invoiceHeaderObj.IVH_DISCOUNT_TC;
                var discHeader = invoiceHeaderObj.TaxHdr.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Discount));
                int isDeducted = 0;
                int isHaveDiscount = 0;//only For knowing It have Discount
                foreach (POInvoiceTaxHdr rfqTaxHdrObj in discHeader)
                {
                    isHaveDiscount = 1;
                    string taxFormula = rfqTaxHdrObj.VTL_TAX_FORMULA;
                    if (!string.IsNullOrEmpty(taxFormula))
                    {
                        //taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                        //rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                        if (invoiceHeaderObj.DeductionDetails != null || invoiceHeaderObj.DeductionDetails.Count > 0)
                        {
                            //calculate tax if advance not deducted
                            for (int i = 0; i < invoiceHeaderObj.DeductionDetails.Count; i++)
                            {
                                if (invoiceHeaderObj.DeductionDetails[i].VAD_TAX_AMOUNT > 0)
                                {
                                    isDeducted = isDeducted + 1;
                                }
                            }
                        }
                        if (isDeducted <= 0)
                        {
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            //rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);//commented on 10/12/2024
                            rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits, MidpointRounding.AwayFromZero);
                        }
                    }
                    else//In case of Custom tax & Discount it doesnot have formula. So we create a formula . 
                    {
                        //Formula :InvoiceNowDiscount=(TotalPODiscount/SubTotalPOAmount)*InvoiceNowAmount
                        if (TempInvoiceHeaderTemp != null)
                        {
                            //if (rfqTaxHdrObj.VTL_PK > 0)
                            //{
                            if (Convert.ToDouble(TempInvoiceHeaderTemp.POH_DISCOUNT_TC) > 0)
                            {
                                double invoicenowDiscount = 0, TotalPODiscount = 0;//, SubTotalPOAmount = 0;
                                //SubTotalPOAmount = TempInvoiceHeaderTemp.OrderDetail.Sum(dtl => dtl.VID_NET_AMOUNT);
                                if (EntryStatus == EntryStatus.NEWMODE)
                                {
                                    if (hdfApplyTax.Value == "0")
                                    {
                                        TotalPODiscount = Convert.ToDouble(TempInvoiceHeaderTemp.POH_DISCOUNT_TC);
                                    }
                                    else
                                    {
                                        TotalPODiscount = TempInvoiceHeaderTemp.TaxHdr.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Discount) && string.IsNullOrEmpty(rfq.VTL_TAX_FORMULA)).Sum(rfq => rfq.VTL_TAX_AMT);
                                    }
                                }
                                else
                                {
                                    TotalPODiscount = TempInvoiceHeaderTemp.TaxHdr.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Discount) && string.IsNullOrEmpty(rfq.VTL_TAX_FORMULA)).Sum(rfq => rfq.VTL_TAX_AMT);
                                }
                                //invoicenowDiscount = (TotalPODiscount / SubTotalPOAmount) * amount;
                                double ChangedSubTot = TempInvoiceHeaderTemp.OrderDetail.Sum(r => r.VID_NET_AMOUNT);
                                //(To solve header discount issue)After changing inv qty and apply discount morethan once getting wrong value
                                if (ChangedSubTot > 0 && Convert.ToDecimal(TotalPODiscount) != TempInvoiceHeaderTemp.POH_DISCOUNT_TC)
                                {
                                    invoicenowDiscount = (TotalPODiscount / ChangedSubTot) * amount;
                                }
                                else if (PrevSubTotalPOAmount > 0)
                                {
                                    invoicenowDiscount = (TotalPODiscount / PrevSubTotalPOAmount) * amount;
                                }

                                rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(invoicenowDiscount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                            }
                            //}                            
                        }
                    }
                }
                // group Discount & calculate the sum for multiple PO
                discount = invoiceHeaderObj.TaxHdr.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Discount)).GroupBy(test => test.VTL_TAX).Select(grp => grp.First()).Sum(rfq => rfq.VTL_TAX_AMT);
                //var discountHdr = invoiceHeaderObj.TaxHdr.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Discount)).GroupBy(test => test.VTL_TAX).Select(grp => grp.First()).ToList();
                //discount = discountHdr.Sum(rfq => rfq.VTL_TAX_AMT);

                invoiceHeaderObj.IVH_DISCOUNT_TC = discount;
                //  txtHdrDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);
                if ((TotalHDRDiscount > 0) || (isDeducted <= 0))
                {
                    txtHdrDiscount.ToolTip = txtHdrDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);
                    txtHdrDiscount.ToolTip = txtHdrDiscount.Text = ((Convert.ToDecimal(txtHdrDiscount.Text) - TotalHDRDiscount) < 0 ? 0 : (Convert.ToDecimal(txtHdrDiscount.Text) - TotalHDRDiscount)).ToString(hdfCurrencyFormat.Value);
                    if (isHaveDiscount == 1)
                    {
                        List<POInvoiceTaxHdr> LstHeaderDiscount = new List<POInvoiceTaxHdr>();
                        LstHeaderDiscount = invoiceHeaderObj.TaxHdr.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Discount)).ToList();
                        if (LstHeaderDiscount != null && LstHeaderDiscount.Count > 0)
                        {
                            //invoiceHeaderObj.TaxHdr.LastOrDefault(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Discount)).VTL_TAX_AMT = Convert.ToDouble(txtHdrDiscount.Text);
                            if (discount > 0)
                            {
                                foreach (POInvoiceTaxHdr disc in LstHeaderDiscount)
                                {
                                    //disc.VTL_TAX_AMT = Math.Round(disc.VTL_TAX_AMT - (disc.VTL_TAX_AMT * (double)TotalHDRDiscount) / discount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                    double TaxAmnt = Math.Round(disc.VTL_TAX_AMT - (disc.VTL_TAX_AMT * (double)TotalHDRDiscount) / discount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                    disc.VTL_TAX_AMT = TaxAmnt > 0 ? TaxAmnt : 0;
                                }
                            }
                        }
                    }

                }
                amount = amount - discount;

                ////
                decimal subTotal = Convert.ToDecimal(invoiceHeaderObj.IVH_AMOUNT_TC + invoiceHeaderObj.IVH_AMOUNT_NET_TC_ADJ);
                double totalTax = 0;
                double tax;
                foreach (GridViewRow grdRow in grdInvoice.Rows)
                {
                    TextBox txtTax = grdRow.FindControl("txtTax") as TextBox;
                    if (txtTax != null)
                    {
                        tax = 0;
                        Double.TryParse(txtTax.Text, out tax);
                        totalTax += tax;
                    }
                }
                // subTotal -= Convert.ToDecimal(totalTax);              
                decimal hdrDeduction = string.IsNullOrEmpty(txtHdrDeduction.Text) ? 0 : Convert.ToDecimal(txtHdrDeduction.Text);
                if (IsAdvInvHasTax)
                {
                    txtHdrBalBeforeVat.Text = ((((subTotal - Convert.ToDecimal(txtHdrDiscount.Text)) - hdrDeduction) - totalAllocatedDiscount)).ToString(hdfCurrencyFormat.Value);
                    txtHdrTotal.Text = (subTotal - (Convert.ToDecimal(txtHdrDiscount.Text)) - totalAllocatedDiscount).ToString(hdfCurrencyFormat.Value);
                }
                else
                {
                    txtHdrBalBeforeVat.Text = txtHdrTotal.Text = (subTotal - Convert.ToDecimal(txtHdrDiscount.Text)).ToString(hdfCurrencyFormat.Value);
                }
                double.TryParse(txtHdrBalBeforeVat.Text, out balBeforeVat);
                amount = balBeforeVat;

                //double AdvDeduction = 0;
                //double.TryParse(txtHdrDeduction.Text, out AdvDeduction);
                //if (!IsAdvInvHasTax)
                //    amount = amount + AdvDeduction;

                ////
                #region Other Charge

                // To handle Other charges against multiple PO   
                // Total Other Charge = SUM(All PO other charge amount) - SUM(All PO invoiced other charge amount)

                if (CurrPK > 0) // Edit mode
                {
                    shipping = Convert.ToDouble(POInvoiceHeaderSession.OtherChargeDetails.Sum(chrg => chrg.IVM_OTHER_AMOUNT));

                }
                else // New mode
                {
                    if (hdfOtherCharge.Value == "1")
                    {
                        shipping = Convert.ToDouble(POInvoiceHeaderSession.OtherChargeDetails.Sum(chrg => chrg.IVM_OTHER_AMOUNT));
                    }
                    else
                    {
                        shipping = Convert.ToDouble(POInvoiceHeaderSession.OtherChargeDetails.Sum(chrg => chrg.PO_OTHER_AMOUNT)) - Convert.ToDouble(TempInvoiceHeaderTemp.OtherChargeDetails.Sum(chrg => chrg.PO_OTHER_AMOUNT_INVOICED));
                    }
                }
                //shipping = TempInvoiceHeaderTemp.TaxHdr.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Shipping)).Sum(rfq => rfq.VTL_TAX_AMT);
                invoiceHeaderObj.IVH_SHIP_CHARGE = shipping;
                txtShipping.Text = txtShipping.ToolTip = shipping.ToString(hdfCurrencyFormat.Value);
                double.TryParse(txtDeductOtherCharges.Text, out otherCharges);
                #endregion
                if (IsTaxForOtherCharge.Value == "1")
                {
                    amount = amount + (shipping - otherCharges);//Here Other Charge = (TotalOtherChrage- Allocated otherCharge)
                }

                ////----Start New Section for proportionate tax -------////
                double _totalTax = 0, _totalCurrTax = 0, _totalAmount = 0, _totalCurrAmount = 0;
                if (IsTaxProportionate)
                {
                    _totalTax = invoiceHeaderObj.IVH_TAX_TC; //POTotalTaxAmount; // //POTotalTaxAmount;//invoiceHeaderObj.TaxHdr.Where(res => res.VTL_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(res => res.VTL_TAX_AMT);
                    _totalAmount = _PrevtotalAmount; // invoiceHeaderObj.IVH_AMOUNT_TC - invoiceHeaderObj.IVH_DISCOUNT_TC; // //PrevSubTotalPOAmount - PrevTotalPOAmountDiscount;
                    _totalCurrAmount = string.IsNullOrEmpty(txtHdrTotal.Text) ? 0 : Convert.ToDouble(txtHdrTotal.Text);
                    _totalCurrTax = (_totalTax / _totalAmount) * _totalCurrAmount;
                }
                ////----End New Section for proportionate tax -------////

                var taxHeader = invoiceHeaderObj.TaxHdr.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Tax));
                if (Convert.ToInt16(hdfisTaxAdd.Value) == (int)TaxSettingEnum.BOTHHEADERITEM)
                {
                    #region BOTHHEADERITEM

                    foreach (POInvoiceTaxHdr rfqTaxHdrObj in taxHeader)
                    {
                        if (IsTaxProportionate)
                        {
                            ////----for proportionate tax -------////
                            _totalTax = 0; _totalCurrTax = 0; _totalAmount = 0; _totalCurrAmount = 0;
                            _totalTax = invoiceHeaderObj.IVH_TAX_TC;
                            #region Tax Applicable amount setting
                            double hdrSubTotal = 0;
                            double hdrDisc = 0;
                            double hdrOtherCharge = 0;
                            double _applicableAmount = 0;
                            if (rfqTaxHdrObj.VTL_HAS_SUB_TOTAL == 1) { hdrSubTotal = invoiceHeaderObj.IVH_AMOUNT_TC; }
                            if (rfqTaxHdrObj.VTL_HAS_DISCOUNT == 1) { hdrDisc = invoiceHeaderObj.IVH_DISCOUNT_TC; }
                            if (rfqTaxHdrObj.VTL_HAS_OTHER_CHARGE == 1) { hdrOtherCharge = invoiceHeaderObj.IVH_SHIP_CHARGE; }
                            if (hdrSubTotal == 0)
                                _applicableAmount = hdrDisc + hdrOtherCharge;
                            else
                                _applicableAmount = (hdrSubTotal - hdrDisc) + hdrOtherCharge;
                            #endregion
                            _PrevtotalAmount = _applicableAmount;
                            _totalAmount = _PrevtotalAmount;
                            _totalCurrAmount = _applicableAmount;//string.IsNullOrEmpty(txtHdrTotal.Text) ? 0 : Convert.ToDouble(txtHdrTotal.Text);
                            _totalCurrTax = (_totalTax / _totalAmount) * _totalCurrAmount;

                            ////----End  for proportionate tax -------////

                            double _currTaxAmount = 0;
                            if (_totalTax > 0)
                            {
                                _currTaxAmount = (rfqTaxHdrObj.VTL_TAX_AMT / _totalTax) * _totalCurrTax;
                                rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(_currTaxAmount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                            }
                        }
                        else
                        {
                            string taxFormula = rfqTaxHdrObj.VTL_TAX_FORMULA;
                            #region Tax Applicable amount setting

                            double hdrSubTotal = 0;
                            double hdrDisc = 0;
                            double hdrOtherCharge = 0;
                            amount = 0;

                            if (rfqTaxHdrObj.VTL_HAS_SUB_TOTAL == 1)
                            {
                                hdrSubTotal = invoiceHeaderObj.IVH_AMOUNT_TC;
                            }
                            if (rfqTaxHdrObj.VTL_HAS_DISCOUNT == 1)
                            {
                                hdrDisc = invoiceHeaderObj.IVH_DISCOUNT_TC;
                            }
                            if (rfqTaxHdrObj.VTL_HAS_OTHER_CHARGE == 1)
                            {
                                hdrOtherCharge = invoiceHeaderObj.IVH_SHIP_CHARGE;
                            }
                            if (hdrSubTotal == 0)
                                amount = hdrDisc + hdrOtherCharge;
                            else
                                amount = (hdrSubTotal - hdrDisc) + hdrOtherCharge;

                            #endregion
                            if (!string.IsNullOrEmpty(taxFormula))
                            {
                                //1 :- No need to create formula for manual entry of Tax amount in TAX POPUP.  
                                //0 :- Create tax formula
                                if (hdfApplyTax.Value == "0")
                                {
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                                    if (amount >= 0)
                                        //rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);//commented on 10/12/2024
                                        rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits, MidpointRounding.AwayFromZero);
                                }
                            }
                            else//In case of Custom tax & Discount it doesnot have formula. So we create a formula. 
                            {
                                //Formula :InvoiceNowTax=(TotalPOTax/(SubTotalPOAmount-Discount))*InvoiceNowAmount
                                if (TempInvoiceHeaderTemp != null)
                                {
                                    double invoicenowTax = 0, TotalPOTax = 0, balancePOAmount = 0;
                                    balancePOAmount = PrevSubTotalPOAmount - PrevTotalPOAmountDiscount;
                                    if (EntryStatus == EntryStatus.NEWMODE)
                                    {
                                        TotalPOTax = Convert.ToDouble(TempInvoiceHeaderTemp.POH_TAX_TC);
                                    }
                                    else
                                    {
                                        balancePOAmount = balancePOAmount - TempInvoiceHeaderTemp.IVH_DISCOUNT_TC;
                                        TotalPOTax = TempInvoiceHeaderTemp.TaxHdr.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Tax) && rfq.VTL_TAX_FORMULA == null).Sum(rfq => rfq.VTL_TAX_AMT);
                                    }
                                    //invoicenowTax = (TotalPOTax / balancePOAmount) * amount;
                                    if (balancePOAmount > 0)
                                    {
                                        invoicenowTax = (TotalPOTax / balancePOAmount) * amount;
                                    }

                                    rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(invoicenowTax, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                }
                            }
                        }
                    }
                    hdfApplyTax.Value = "0";

                    // group tax & calculate the sum for multiple PO
                    invoiceHeaderObj.IVH_TAX_TC = invoiceHeaderObj.TaxHdr.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Tax))
                                                                         .GroupBy(test => test.VTL_TAX)
                                                                         .Select(grp => grp.First())
                                                                         .Sum(rfq =>
                                                                          Math.Round(rfq.VTL_TAX_AMT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                    txtHdrTax.ToolTip = txtHdrTax.Text = invoiceHeaderObj.IVH_TAX_TC.ToString(hdfCurrencyFormat.Value);

                    #endregion
                }
                else
                {
                    #region else region
                    foreach (POInvoiceTaxHdr rfqTaxHdrObj in taxHeader)
                    {
                        if (IsTaxProportionate)
                        {
                            double _currTaxAmount = 0;
                            if (_totalTax > 0)
                            {
                                _currTaxAmount = (rfqTaxHdrObj.VTL_TAX_AMT / _totalTax) * _totalCurrTax;
                                rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(_currTaxAmount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                            }
                        }
                        else
                        {
                            string taxFormula = rfqTaxHdrObj.VTL_TAX_FORMULA;
                            if (!string.IsNullOrEmpty(taxFormula))
                            {
                                //1 :- No need to create formula for manual entry of Tax amount in TAX POPUP.  
                                //0 :- Create tax formula
                                if (hdfApplyTax.Value == "0")
                                {
                                    if (amount >= 0)
                                    {
                                        if (amount >= Convert.ToDouble(invoiceHeaderObj.DeductionDetails.Sum(x => x.VAD_TAX_AMOUNT)))
                                        {
                                            //amount = amount - Convert.ToDouble(invoiceHeaderObj.DeductionDetails.Sum(x => x.VAD_TAX_AMOUNT));
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                                            //rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1); 
                                            rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits, MidpointRounding.AwayFromZero);//15-11-2024 Rounding Issue
                                        }
                                        else
                                        {
                                            rfqTaxHdrObj.VTL_TAX_AMT = 0;
                                        }
                                    }
                                    else
                                    {
                                        rfqTaxHdrObj.VTL_TAX_AMT = 0;
                                    }
                                }
                            }
                            else//In case of Custom tax & Discount it doesnot have formula. So we create a formula. 
                            {
                                //Formula :InvoiceNowTax=(TotalPOTax/(SubTotalPOAmount-Discount))*InvoiceNowAmount
                                if (TempInvoiceHeaderTemp != null)
                                {
                                    //if (rfqTaxHdrObj.VTL_PK > 0)
                                    //{
                                    double invoicenowTax = 0, TotalPOTax = 0, balancePOAmount = 0; //SubTotalPOAmount = 0
                                    //SubTotalPOAmount = TempInvoiceHeaderTemp.OrderDetail.Sum(dtl => dtl.VID_NET_AMOUNT);
                                    //balancePOAmount = SubTotalPOAmount - TempInvoiceHeaderTemp.IVH_DISCOUNT_TC;

                                    balancePOAmount = PrevSubTotalPOAmount - PrevTotalPOAmountDiscount;

                                    if (EntryStatus == EntryStatus.NEWMODE)
                                    {
                                        TotalPOTax = Convert.ToDouble(TempInvoiceHeaderTemp.POH_TAX_TC);
                                    }
                                    else
                                    {
                                        balancePOAmount = balancePOAmount - TempInvoiceHeaderTemp.IVH_DISCOUNT_TC;
                                        TotalPOTax = TempInvoiceHeaderTemp.TaxHdr.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Tax) && rfq.VTL_TAX_FORMULA == null).Sum(rfq => rfq.VTL_TAX_AMT);
                                    }
                                    //invoicenowTax = (TotalPOTax / balancePOAmount) * amount;

                                    if (balancePOAmount > 0)
                                    {
                                        invoicenowTax = (TotalPOTax / balancePOAmount) * amount;
                                    }

                                    rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(invoicenowTax, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                }
                                //}
                            }
                        }
                    }
                    hdfApplyTax.Value = "0";

                    #region Old code to handle Other charges for single PO

                    ////Other charges
                    //var shippingHeader = invoiceHeaderObj.TaxHdr.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Shipping));
                    //foreach (POInvoiceTaxHdr rfqTaxHdrObj in shippingHeader)
                    //{
                    //    string taxFormula = rfqTaxHdrObj.VTL_TAX_FORMULA;
                    //    if (!string.IsNullOrEmpty(taxFormula))
                    //    {
                    //        //taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                    //        taxFormula = taxFormula.Replace("#SUBTOTAL#", invoiceHeaderObj.IVH_AMOUNT_TC.ToString());
                    //        rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                    //    }
                    //    else//In case of Custom Other charges it doesnot have formula. So we create a formula. 
                    //    {
                    //        //Formula :InvoiceNowOtherCharge=(TotalPOOtherCharge/(SubTotalPOAmount))*InvoiceNowAmount
                    //        if (TempInvoiceHeaderTemp != null)
                    //        {
                    //            double invoicenowOtherCharge = 0, TotalPOOtherCharge = 0, SubTotalPOAmount = 0;
                    //            SubTotalPOAmount = TempInvoiceHeaderTemp.OrderDetail.Sum(dtl => dtl.VID_NET_AMOUNT);
                    //            if (EntryStatus == EntryStatus.NEWMODE)
                    //            {
                    //                TotalPOOtherCharge = Convert.ToDouble(TempInvoiceHeaderTemp.POH_SHIP_CHARGE);
                    //            }
                    //            else
                    //            {
                    //                TotalPOOtherCharge = TempInvoiceHeaderTemp.TaxHdr.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Shipping)).Sum(rfq => rfq.VTL_TAX_AMT);
                    //            }
                    //            invoicenowOtherCharge = (TotalPOOtherCharge / SubTotalPOAmount) * invoiceHeaderObj.IVH_AMOUNT_TC;
                    //            rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(invoicenowOtherCharge, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                    //        }
                    //    }
                    //}
                    //shipping = invoiceHeaderObj.IVH_SHIP_CHARGE = invoiceHeaderObj.TaxHdr.Where(quotation => quotation.VTL_TAX_CATEGORY == ((int)TaxType.Shipping)).GroupBy(test => test.VTL_TAX).Select(grp => grp.First()).Sum(quotation => quotation.VTL_TAX_AMT);

                    #endregion



                    //string tax_formula = string.Empty;
                    //var customtax = invoiceHeaderObj.TaxHdr.Any(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Tax) && rfq.VTL_TAX_FORMULA == null);//.Any();


                    //if (customtax)
                    //{

                    //}
                    //else
                    //{ 

                    // group tax & calculate the sum for multiple PO
                    invoiceHeaderObj.IVH_TAX_TC = invoiceHeaderObj.TaxHdr.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Tax))
                                                                         .GroupBy(test => test.VTL_TAX)
                                                                         .Select(grp => grp.First())
                                                                         .Sum(rfq => rfq.VTL_TAX_AMT);
                    txtHdrTax.ToolTip = txtHdrTax.Text = invoiceHeaderObj.IVH_TAX_TC.ToString(hdfCurrencyFormat.Value);
                    //}

                    #endregion
                }

                //invoiceHeaderObj.IVH_TAX_TC = TaxHeadr.Sum(rfq => rfq.VTL_TAX_AMT);
                //double.TryParse(txtShipping.Text, out shipping);

                double.TryParse(txtPriceAdj.Text, out adjust);
                invoiceHeaderObj.IVH_AMOUNT_ADJUST = adjust;


                //invoiceHeaderObj.IVH_AMOUNT_NET_TC = invoiceHeaderObj.IVH_AMOUNT_TC - invoiceHeaderObj.IVH_DISCOUNT_TC + invoiceHeaderObj.IVH_TAX_TC
                //    + invoiceHeaderObj.IVH_SHIP_CHARGE + invoiceHeaderObj.IVH_AMOUNT_ADJUST;

                if (IsAdvInvHasTax)
                {
                    invoiceHeaderObj.IVH_AMOUNT_NET_TC = balBeforeVat + invoiceHeaderObj.IVH_TAX_TC + invoiceHeaderObj.IVH_SHIP_CHARGE + invoiceHeaderObj.IVH_AMOUNT_ADJUST - otherCharges;
                    txtHdrNetTotal.ToolTip = txtHdrNetTotal.Text = invoiceHeaderObj.IVH_AMOUNT_NET_TC.ToString(hdfCurrencyFormat.Value);
                }
                else
                {
                    double Total = 0;
                    Total = balBeforeVat + invoiceHeaderObj.IVH_TAX_TC + invoiceHeaderObj.IVH_SHIP_CHARGE + invoiceHeaderObj.IVH_AMOUNT_ADJUST;
                    txtTotal.Text = txtTotal.ToolTip = Total.ToString(hdfCurrencyFormat.Value);

                    double totalDeduction = 0;
                    double.TryParse(txtTotalDeduction.Text, out totalDeduction);
                    invoiceHeaderObj.IVH_AMOUNT_NET_TC = Total - totalDeduction;
                    txtHdrNetTotal.ToolTip = txtHdrNetTotal.Text = invoiceHeaderObj.IVH_AMOUNT_NET_TC.ToString(hdfCurrencyFormat.Value);

                }

                POInvoiceHeaderSession = invoiceHeaderObj;
            }
            return true;
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
                foreach (decimal var in TaxList)
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
        private double CalculateTaxFormula(string taxFormula, double amount)
        {
            double taxAmt;
            taxAmt = 0;
            if (!string.IsNullOrEmpty(taxFormula))
            {
                taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                taxAmt = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits, MidpointRounding.AwayFromZero);
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

        private void PriceAdjustConfiguration()
        {
            DataTable dt = new DataTable();
            dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("PURCHASE INVOICE SETTINGS", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfPriceAdjPercentage.Value = dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "ADJ TOLERANCE")["ACF_VALUE"].ToString();
            }
        }

        private bool ValidateAssetType()
        {
            bool IsValid = true;
            try
            {
                if (hdfIsSaveSubmit.Value != CommonConstants.SELECTVAL)
                {
                    foreach (GridViewRow row in grdInvoice.Rows)
                    {
                        CheckBox chkIsAsset = (CheckBox)row.FindControl("chkIsAsset");
                        DropDownList ddlAssetType = (DropDownList)row.FindControl("ddlAssetType");
                        if (chkIsAsset.Checked && ddlAssetType.SelectedValue == CommonConstants.SELECTVAL)
                        {
                            IsValid = false;
                            break;
                        }
                    }
                }
                return IsValid;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region Grd Status maintains
        //For sett allocation details
        private void SetAllocationDetails()
        {
            if (Session[ERP.Utilities.SessionStrings.SelectedInvoicesInfoLst] != null)
                SelectedInvoicesInfoLst = (List<SelectionInfo>)Session[ERP.Utilities.SessionStrings.SelectedInvoicesInfoLst];
            else
                SelectedInvoicesInfoLst = new List<SelectionInfo>();
            CheckBox chbSelect;
            foreach (GridViewRow item in grdInvoiceList.Rows)
            {
                chbSelect = (CheckBox)item.FindControl("chkPIselect");
                SelectionInfo objSaleOrderInfo = new SelectionInfo();
                objSaleOrderInfo.chkChecked = false;
                objSaleOrderInfo.InvoicePK = Convert.ToInt32(grdInvoiceList.DataKeys[item.RowIndex].Value.ToString());
                if (chbSelect.Checked)
                {
                    objSaleOrderInfo.chkChecked = true;
                    objSaleOrderInfo.VendorPK = Convert.ToInt32(((HiddenField)item.FindControl("hdfVendorPK")).Value);//E
                    objSaleOrderInfo.CurrencyPK = Convert.ToInt32(((HiddenField)item.FindControl("hdfPOCurrency")).Value);//E
                    objSaleOrderInfo.ApprovedStatus = Convert.ToInt32(((HiddenField)item.FindControl("hdfApproved")).Value);
                    objSaleOrderInfo.IsPosted = Convert.ToBoolean(((HiddenField)item.FindControl("hdfPosted")).Value);
                    objSaleOrderInfo.InvCategory = Convert.ToInt32(((HiddenField)item.FindControl("hdfInvCategory")).Value);

                    if (!string.IsNullOrEmpty(((HiddenField)item.FindControl("hdfJournalStatus")).Value))
                        objSaleOrderInfo.JournalStatus = Convert.ToInt32(((HiddenField)item.FindControl("hdfJournalStatus")).Value);
                    else
                        objSaleOrderInfo.JournalStatus = 0;

                    objSaleOrderInfo.Tax = Convert.ToDecimal(string.IsNullOrEmpty(((HiddenField)item.FindControl("hdfTaxAmount")).Value) ? "0" : ((HiddenField)item.FindControl("hdfTaxAmount")).Value);
                    LinkButton lbnBalAmt = item.FindControl("lbnBalAmt") as LinkButton;
                    objSaleOrderInfo.BalanceAmt = Convert.ToDecimal(lbnBalAmt.Text);
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
                        Items.JournalStatus = item.JournalStatus;
                    }
        }

        //For Reset grid status
        private void SetGridStatus()
        {
            if (Session[ERP.Utilities.SessionStrings.SelectedInvoicesInfoLst] != null)
            {
                SelectedInvoicesInfoLst = (List<SelectionInfo>)Session[ERP.Utilities.SessionStrings.SelectedInvoicesInfoLst];
                int sodPK;
                foreach (GridViewRow item in grdInvoiceList.Rows)
                {
                    sodPK = Convert.ToInt32(grdInvoiceList.DataKeys[item.RowIndex].Value.ToString());
                    if (SelectedInvoicesInfoLst.Exists(itemLst => itemLst.InvoicePK == sodPK && itemLst.chkChecked == true))
                        ((CheckBox)item.FindControl("chkPIselect")).Checked = true;

                }
            }

        }

        #endregion


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
            // if one page containes two process (pageurl?PID=1,pageurl?PID=2)
            //if (Request.QueryString[QueryStrings.PID] == null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                    path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?PID=" + pid.ToString();
                else
                    path = Request.Url.AbsolutePath.ToLower() + "?PID=" + pid.ToString();
            }
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
                    if (pid == 1 || pid == 21)
                    {
                        PageProcessID = ucrWrkf.ProcessID;
                        //base.WkfPageUrl = path;
                    }
                    base.WkfPageUrl = path;

                    ((HiddenField)this.Master.FindControl("hdnPageID")).Value = dtProcess.Rows[0]["PAG_PK"].ToString();
                    // Fill workflow details
                    ReqDept = 0;
                    int.TryParse(hdfSubDeptPk.Value, out ReqDept);
                    ucrWrkf.ReqDeptID = (PIType == "21" && pid != 2) ? ReqDept : 0; //pid=2 for journalize, PIType 21 is service PO
                    ucrWrkf.FillWorkFlowDetails();
                }
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

            POInvoiceService poInvoiceServiceClient;
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            try
            {
                int? result;
                result = 0;
                int alertresult;
                string itemAmount;
                List<POInvoiceTaxHdr> tempInvTaxHdrSplit;
                POInvoiceTaxHdr tempInvTaxSplitObj = null;
                HiddenField hdfInvoiceDtlPK;
                HiddenField hdfItemPK;
                HiddenField hdfPOPK;
                HiddenField hdfInOpeningInv;
                HiddenField hdfIsConverted;
                TextBox txtAmount;
                TextBox txtTax;
                TextBox txtDiscount;
                TextBox txtSubTotal;
                TextBox txtSubTotalFooterAmt;
                TextBox txtAdjustAmount;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                double amount;
                double discount;
                int count;
                bool bIsChecked = false;
                DropDownList ddlWkfAction;
                TextBox WrkfComments;
                string action;
                int selectedItemPK;
                double totalAmt;
                double currentTotal;
                double taxAmt;
                double subTotalAmt = 0;
                bool isValidDisc = true;
                bool isContinue;
                FileInfo tempFileInfoObj;
                string savePath = string.Empty;
                int InvoiceCategory = 0;

                long? DummyResult;
                DummyResult = 0;

                bool GenDummy = false;
                string poPK;
                string grnPK;
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

                    //Amount Adjust text changing event
                    else if (((TextBox)sender).ID == "txtAdjustAmountFooter")
                    {
                        commonActions = ActionsEnum.ADJUSTAMOUNT;
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
                    if (((CheckBox)sender).ID == "chkSubTotal" || ((CheckBox)sender).ID == "chkDiscount" || ((CheckBox)sender).ID == "chkOtherCharges")
                    {
                        commonActions = ActionsEnum.SETTAXAPPLICABLEAMOUNT;
                    }
                    if (((CheckBox)sender).ID == "chkIsAsset")
                    {
                        commonActions = ActionsEnum.CHECK_CHANGE;
                    }
                }
                switch (commonActions)
                {
                    #region ITEMSELECTED
                    case ActionsEnum.ITEMSELECTED:
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            RadioButton rbtn;
                            HiddenField hdfDept;
                            int selectedInvPK;
                            int dept;

                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                selectedInvPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;

                                if (Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1)
                                {
                                    btnSave.Visible = false;
                                    hdfIsInvCancelled.Value = "1";
                                }
                                else
                                {
                                    btnSave.Visible = true;
                                    hdfIsInvCancelled.Value = "0";
                                }

                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                base.WkfRefID = workflowCore.GetRefID(selectedInvPK, PageProcessID);

                                break;
                            }
                        }

                        //SetResetColour
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);

                        break;
                    #endregion

                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else if (!ValidateAssetType())
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_AssetType").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            #region Checking :If Other charges entered is exceeding the value given in PO
                            if (HdfIsContYesOtherCharges.Value != "1")
                            {

                                //if (Convert.ToDecimal(txtShipping.Text) <= Convert.ToDecimal(hdfOtherchargePO.Value))
                                //{
                                //}
                                //else
                                //{
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "OtherChargesExceeds", "$(document).ready(function(){ShowOtherChargesExceeds(1);});", true);
                                //    break;
                                //}

                            }
                            #endregion

                            isContinue = true;
                            if (ddlAddressType.Items.Count <= 0)
                            {
                                isContinue = false;

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ReqType").ToString()) + "');", true);
                                return;
                            }
                            if (GetGlobalResourceObject("ConfigurationsRes", "POInvoiceRequiredAdvanceDeduct").ToString() == "1")
                            {
                                TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
                                deductionDtlList = new List<POAdvDeductionDetails>();
                                deductionDtlList = TempPOInvoiceHeaderSession.DeductionDetails.ToList();
                                if ((deductionDtlList != null && deductionDtlList.Count > 0))
                                {
                                    decimal balAllocation = deductionDtlList.Sum(b => (Convert.ToDecimal(b.PVH_PO_PAID_AMT) - b.IVH_AMOUNT_ALLOCATED) - (b.VAD_TAX_AMOUNT + b.VAD_OTHER_AMOUNT));
                                    bool pendingDeduction = false;
                                    if (balAllocation <= Convert.ToDecimal(txtHdrTotal.Text))
                                    {
                                        if (Convert.ToDecimal(txtHdrDeduction.Text) < balAllocation)
                                            pendingDeduction = true;
                                    }

                                    else if (balAllocation > Convert.ToDecimal(txtHdrTotal.Text) + (Convert.ToDecimal(txtPriceAdj.Text.Trim())))
                                    {
                                        if (Convert.ToDecimal(txtHdrDeduction.Text) != Convert.ToDecimal(txtHdrTotal.Text))
                                            pendingDeduction = true;
                                    }
                                    if (pendingDeduction)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_Without_Allocation").ToString()) + "');", true);
                                        return;
                                    }
                                }
                            }
                            if ((hdfSaveWithoutAllocation.Value == "0") || (hdfSaveWithoutAllocation.Value == ""))
                            {
                                TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
                                deductionDtlList = new List<POAdvDeductionDetails>();
                                deductionDtlList = TempPOInvoiceHeaderSession.DeductionDetails.ToList();

                                if (IsAdvInvHasTax)
                                {
                                    if (Convert.ToDouble(txtHdrDeduction.Text) <= 0 && (deductionDtlList != null && deductionDtlList.Count > 0))
                                    {
                                        isContinue = false;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowSaveWithoutAllocationConfirm('" + (sender as Button).ID + "');", true);
                                    }
                                }
                                else
                                {
                                    if (Convert.ToDouble(txtTotalDeduction.Text) <= 0 && (deductionDtlList != null && deductionDtlList.Count > 0))
                                    {
                                        isContinue = false;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowSaveWithoutAllocationConfirm('" + (sender as Button).ID + "');", true);
                                    }
                                }
                            }

                            #region Invoice Now Checking :If Inv.Now Qty entered  exceeds Ordered Qty
                            if (hdfIsContInvoiceNowQty.Value != "1")
                            {
                                foreach (GridViewRow grdrowitem in grdInvoice.Rows)
                                {
                                    TextBox txtInvNow = (TextBox)grdrowitem.FindControl("txtInvNow");
                                    Label lblOrderQuantity = (Label)grdrowitem.FindControl("lblOrderQuantity");
                                    Label lblInvQuantity = (Label)grdrowitem.FindControl("lblInvQuantity");
                                    decimal RemainingQty = Convert.ToDecimal(lblOrderQuantity.Text.Replace(",", "")) - Convert.ToDecimal(lblInvQuantity.Text.Replace(",", ""));

                                    if (RemainingQty < 0 && Convert.ToDecimal(txtInvNow.Text) <= 0)
                                    {
                                    }
                                    else
                                    {
                                        //IF Value = 1 Invoice quantity can exceed order quantity IF Value = 0 Can't exceed order quantity 
                                        if (IsQuantityValidationRequired())
                                        {
                                            if (Convert.ToDecimal(txtInvNow.Text) > RemainingQty)
                                            {
                                                isContinue = false;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InvoiceNowExceeds", "$(document).ready(function(){ShowInvoiceNowQtyExceeds(1);});", true);
                                                break;

                                            }
                                        }
                                    }
                                }

                            }
                            #endregion
                            #region Checking SubTotal amount greater than zero or not
                            txtSubTotalFooterAmt = (TextBox)grdInvoice.FooterRow.FindControl("txtSubTotalFooter");
                            subTotalAmt = txtSubTotalFooterAmt == null ? 0 : string.IsNullOrEmpty(txtSubTotalFooterAmt.Text.Trim()) ? 0 : Convert.ToDouble(txtSubTotalFooterAmt.Text.Trim());
                            if (subTotalAmt == 0)
                            {
                                isContinue = false;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_SubTotal").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            #endregion
                            if (isContinue)
                            {
                                //hasValidRate = false;
                                invoiceHeaderObj = new POInvoiceHeader();
                                invoiceHeaderObj = (POInvoiceHeader)SetUIValuesToObject(ControlsEnum.POINVHEADER);
                                if (invoiceHeaderObj.IVH_AMOUNT_TC > 0)
                                {
                                    if (hasValidRate)
                                    {
                                        if (invoiceHeaderObj != null && invoiceHeaderObj.OrderDetail != null)
                                        {
                                            invoiceHeaderObj.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                            string xmlDoc = CommonFunctions.XmlSerialize<POInvoiceHeader>(invoiceHeaderObj);//CommonFunctions.ObjectTOXml(rfqResponseHeaderObj);
                                            // save Process Control inspection details
                                            bool isCont = true;
                                            if (Convert.ToInt16(hdfCrDrStatus.Value) > 0)
                                            {
                                                if (Convert.ToInt16(hdfCrDrWKFStatus.Value) == 0)
                                                {
                                                    isCont = true;
                                                    GenDummy = true;
                                                }
                                                else
                                                {
                                                    isCont = false;
                                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Already_Journalized").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);

                                                    EntryStatus = EntryStatus.LISTMODE;
                                                    ResetForm(ControlsEnum.INVOICELIST);
                                                    GetFieldValues(ControlsEnum.INVOICELIST);
                                                    SetFieldValues(ControlsEnum.INVOICELIST);
                                                    break;
                                                }

                                            }
                                            if (isCont)
                                            {
                                                // result = BusinessLogic.POInvoicing.POInvoiceBL.SavePOInvoiceHeader(xmlDoc);
                                                string invNumber = string.Empty;
                                                result = BusinessLogic.POInvoicing.POInvoiceBL.SavePOInvoiceWkf(xmlDoc, out invNumber);
                                            }
                                            if (result > 0) // Success !  redirect to listing page
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
                                                //if (GenDummy == true)
                                                //{
                                                if (Convert.ToInt16(hdfCrDrStatus.Value) == (int)DbStatus.APPROVED)
                                                {
                                                    FinTrxService finTrxServiceClient;
                                                    finTrxServiceClient = new FinTrxService();
                                                    string refType = "";
                                                    refType = POGroup == POInvoiceGroup.Goods ? ApplicationType.PIJ : POGroup == POInvoiceGroup.Services ? ApplicationType.PSIJ : ApplicationType.EIJ;
                                                    bool IsDummyEntry = finTrxServiceClient.IsDummyEntry(refType, (int)invoiceHeaderObj.IVH_PK, 0);
                                                    if (IsDummyEntry == true)
                                                    {
                                                        DummyResult = finTrxServiceClient.DeleteFinTrx(refType, (int)invoiceHeaderObj.IVH_PK, 0);
                                                    }
                                                    else
                                                    {
                                                        DummyResult = (int)invoiceHeaderObj.IVH_PK;
                                                    }
                                                    if (DummyResult > 0)
                                                    {
                                                        finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                                        DummyResult = finTrxServiceClient.GenerateDummyEntry((int)CurrPK, refType);
                                                    }
                                                }
                                                //}
                                                // Show Save Message and redired to listing page                                        
                                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.POInvoice);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                                EntryStatus = EntryStatus.LISTMODE;
                                                ResetForm(ControlsEnum.INVOICELIST);
                                                GetFieldValues(ControlsEnum.INVOICELIST);
                                                SetFieldValues(ControlsEnum.INVOICELIST);
                                            }
                                            else if (result == -6)
                                            {
                                                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_DuplicateVendorInvNo").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "VendorInvoiceDup", "$(document).ready(function(){ShowDuplicateVendorInvNoContinue(1);});", true);
                                            }
                                            else if (result == -51)// Gtn qty exeed
                                            {
                                                litErrorMsg.Text = GetLocalResourceObject("GRNQtyExceed").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                            }
                                            else if (result == -52)// Purchase qty exeed
                                            {
                                                litErrorMsg.Text = GetLocalResourceObject("PURQtyExceed").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                            }

                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Empty_Rate").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_SubTotal").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                // hasValidRate = false;
                            }
                        }
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
                            if (fupUpload.HasFile)
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
                                                    poUploadObj.DOC_STATUS = Convert.ToByte(DbActiveStatus.ACTIVE);
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
                                            poUploadObj.DOC_STATUS = Convert.ToByte(DbActiveStatus.ACTIVE);
                                            FileDetailsList.Add(new FileDetails() { SlNo = slno, PoFile = HttpContext.Current.Request.Files[0] });
                                            POUploadList.Add(poUploadObj);

                                        }
                                    }
                                }
                            }
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            ResetForm(ControlsEnum.ADDITEM);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ScrollDown();", true);
                        }
                        break;
                    #endregion

                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEM:
                        if (((Button)sender).ID == "lnkRemoveGRNItem")
                        {
                            if (POUploadList != null && POUploadList.Count > 0)
                            {
                                selectedItemPK = Convert.ToInt32(grdGrnAttchments.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                                if (selectedItemPK > 0)
                                {
                                    //POUploadList = POUploadList.Where(row => selectedItemPK != row.DOC_SEQ_NO).ToList();
                                    POUploadList.RemoveAll(row => row.DOC_SEQ_NO == selectedItemPK && row.DOC_IS_GRN == 1);
                                    BindGrid(ControlsEnum.GRNATTACHMENTS);
                                }
                            }
                        }
                        else
                        {
                            if (POUploadList != null && POUploadList.Count > 0)
                            {
                                selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                                if (selectedItemPK > 0)
                                {
                                    //POUploadList = POUploadList.Where(row => selectedItemPK != row.DOC_SEQ_NO).ToList();
                                    POUploadList.RemoveAll(row => row.DOC_SEQ_NO == selectedItemPK && row.DOC_IS_GRN == 0);
                                    //SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                    BindGrid(ControlsEnum.UPLOADEDFILES);
                                    ResetForm(ControlsEnum.ADDITEM);
                                }
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

                    #region TAXDETAILS
                    case ActionsEnum.TAXDETAILS:
                        divTaxApplicableAmount.Visible = false; //Hide Tax Applicable Amount Checkbox div
                        ResetTaxApplicableCheckbox();
                        hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        txtAmount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("txtAmount") as TextBox);
                        txtDiscount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("txtDiscount") as TextBox);
                        hdfInvoiceDtlPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfInvoiceDtlPK") as HiddenField);
                        hdfItemPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                        hdfPOPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfSODtlPK") as HiddenField);
                        if (txtAmount != null && hdfInvoiceDtlPK != null && txtDiscount != null)
                        {
                            txtPopupItemAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? Convert.ToDouble(txtAmount.Text).ToString(hdfCurrencyFormat.Value) : (Convert.ToDouble(txtAmount.Text.Trim()) - Convert.ToDouble(txtDiscount.Text.Trim())).ToString(hdfCurrencyFormat.Value);
                            POInvoicePK = string.IsNullOrEmpty(hdfInvoiceDtlPK.Value) ? 0 : Convert.ToInt32(hdfInvoiceDtlPK.Value);
                            SelectedItemPK = string.IsNullOrEmpty(hdfItemPK.Value) ? 0 : Convert.ToInt32(hdfItemPK.Value);
                            SelectedPOPK = string.IsNullOrEmpty(hdfPOPK.Value) ? 0 : Convert.ToInt32(hdfPOPK.Value);
                            if (POInvoiceHeaderSession != null)
                            {
                                TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
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
                                            txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits, MidpointRounding.AwayFromZero).ToString(hdfCurrencyFormat.Value);
                                            SelectedTaxText = HttpUtility.HtmlEncode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
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

                                #region manage tax

                                if (Convert.ToInt16(hdfisTaxAdd.Value) == (int)TaxSettingEnum.ITEMWISE || Convert.ToInt16(hdfisTaxAdd.Value) == (int)TaxSettingEnum.BOTHHEADERITEM)
                                {
                                    btnApply.Visible = false;
                                    imgPopupAdd.Visible = false;
                                    grdTaxDetails.Columns[3].Visible = false;
                                    ddlPopupTaxType.Enabled = false;
                                }

                                #endregion

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','600','300');", true);
                            }
                        }
                        break;
                    #endregion

                    #region DISCDETAILS
                    case ActionsEnum.DISCDETAILS:
                        divTaxApplicableAmount.Visible = false; //Hide Tax Applicable Amount Checkbox div
                        ResetTaxApplicableCheckbox();
                        hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        txtAmount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("txtAmount") as TextBox);
                        txtDiscount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("txtDiscount") as TextBox);
                        hdfInvoiceDtlPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfInvoiceDtlPK") as HiddenField);
                        hdfItemPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                        hdfPOPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfSODtlPK") as HiddenField);
                        if (txtAmount != null && hdfInvoiceDtlPK != null && txtDiscount != null)
                        {
                            txtPopupItemAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtAmount.Text).ToString(hdfCurrencyFormat.Value);
                            POInvoicePK = string.IsNullOrEmpty(hdfInvoiceDtlPK.Value) ? 0 : Convert.ToInt32(hdfInvoiceDtlPK.Value);
                            SelectedItemPK = string.IsNullOrEmpty(hdfItemPK.Value) ? 0 : Convert.ToInt32(hdfItemPK.Value);
                            SelectedPOPK = string.IsNullOrEmpty(hdfPOPK.Value) ? 0 : Convert.ToInt32(hdfPOPK.Value);
                            if (POInvoiceHeaderSession != null)
                            {
                                TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
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
                                            txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits, MidpointRounding.AwayFromZero).ToString(hdfCurrencyFormat.Value);
                                            SelectedTaxText = HttpUtility.HtmlEncode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
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

                                #region manage discount

                                if (Convert.ToInt16(hdfisDiscountAdd.Value) == (int)TaxSettingEnum.ITEMWISE || Convert.ToInt16(hdfisDiscountAdd.Value) == (int)TaxSettingEnum.BOTHHEADERITEM)
                                {
                                    btnApply.Visible = false;
                                    imgPopupAdd.Visible = false;
                                    grdTaxDetails.Columns[3].Visible = false;
                                    ddlPopupTaxType.Enabled = false;
                                }

                                #endregion

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("DiscountDetails").ToString() + "','600','300');", true);
                            }
                        }
                        break;
                    #endregion

                    #region TAXHEADER
                    case ActionsEnum.TAXHEADER:
                        ResetTaxApplicableCheckbox();
                        hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (POInvoiceHeaderSession != null)
                        {
                            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            SetFieldValues(ControlsEnum.TAXTYPES);
                            GetFieldValues(ControlsEnum.ADVANCEDTAXSETTINGS);
                            double OtherCharge = string.IsNullOrEmpty(txtShipping.Text.Trim()) ? 0 : Convert.ToDouble(txtShipping.Text.Trim());
                            if (Convert.ToInt16(hdfisTaxAdd.Value) == (int)TaxSettingEnum.BOTHHEADERITEM)
                            {
                                #region
                                double popupItemAmount = 0;
                                popupItemAmount = SetTaxApplicableAmount();
                                txtPopupItemAmount.Text = (popupItemAmount).ToString(hdfCurrencyFormat.Value);
                                divTaxApplicableAmount.Visible = true; //Hide Tax Applicable Amount Checkbox div
                                #endregion
                            }
                            else
                            {
                                #region else region
                                if (hdfTaxSettings.Value.Equals("1"))
                                {
                                    //Is Othercharge is need for Tax Calulation
                                    if (IsTaxForOtherCharge.Value == "1")
                                    {
                                        double HdrBalBeforeVat;
                                        double DeductOtherCharges;
                                        double.TryParse(txtDeductOtherCharges.Text, out DeductOtherCharges);
                                        HdrBalBeforeVat = string.IsNullOrEmpty(txtHdrBalBeforeVat.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrBalBeforeVat.Text) + (OtherCharge - DeductOtherCharges);
                                        txtPopupItemAmount.Text = HdrBalBeforeVat.ToString(hdfCurrencyFormat.Value);
                                    }
                                    else
                                    {
                                        txtPopupItemAmount.Text = string.IsNullOrEmpty(txtHdrBalBeforeVat.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtHdrBalBeforeVat.Text).ToString(hdfCurrencyFormat.Value);
                                    }
                                }
                                else
                                {
                                    //Is Othercharge is need for Tax Calulation
                                    if (IsTaxForOtherCharge.Value == "1")
                                    {
                                        double HdrBalBeforeVat;
                                        HdrBalBeforeVat = string.IsNullOrEmpty(txtHdrTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrTotal.Text) + OtherCharge;
                                        txtPopupItemAmount.Text = HdrBalBeforeVat.ToString(hdfCurrencyFormat.Value);
                                    }
                                    else
                                    {
                                        txtPopupItemAmount.Text = string.IsNullOrEmpty(txtHdrTotal.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtHdrTotal.Text).ToString(hdfCurrencyFormat.Value);
                                    }
                                }
                                #endregion
                            }
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
                                        txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits,MidpointRounding.AwayFromZero).ToString(hdfCurrencyFormat.Value);
                                        SelectedTaxText = HttpUtility.HtmlEncode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
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

                            #region Manage HeaderTaxPopup Control Visibility

                            if (Convert.ToInt16(hdfisTaxAdd.Value) == (int)TaxSettingEnum.BOTHHEADERITEM)
                            {
                                btnApply.Visible = true;
                                imgPopupAdd.Visible = true;
                                grdTaxDetails.Columns[3].Visible = true;
                                ddlPopupTaxType.Enabled = true;
                            }

                            #endregion

                            IsEditMode = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','600','300');", true);
                            //}
                        }
                        break;
                    #endregion

                    #region DISCHEADER
                    case ActionsEnum.DISCHEADER:
                        divTaxApplicableAmount.Visible = false; //Hide Tax Applicable Amount Checkbox div
                        ResetTaxApplicableCheckbox();
                        hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        double subtotalAdj = 0;
                        if (POInvoiceHeaderSession != null)
                        {
                            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            SetFieldValues(ControlsEnum.TAXTYPES);
                            txtSubTotal = (TextBox)grdInvoice.FooterRow.FindControl("txtSubTotalFooter");
                            txtAdjustAmount = (TextBox)grdInvoice.FooterRow.FindControl("txtAdjustAmountFooter");
                            if (txtSubTotal != null)
                            {
                                subtotalAdj = string.IsNullOrEmpty(txtAdjustAmount.Text.Trim()) ? 0 : Convert.ToDouble(txtAdjustAmount.Text);
                                txtPopupItemAmount.Text = string.IsNullOrEmpty(txtSubTotal.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : (Convert.ToDouble(txtSubTotal.Text) + subtotalAdj).ToString(hdfCurrencyFormat.Value);
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
                                            txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits, MidpointRounding.AwayFromZero).ToString(hdfCurrencyFormat.Value);
                                            SelectedTaxText = HttpUtility.HtmlEncode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
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
                                #region Manage HeaderDiscountPopup Control Visibility

                                if (Convert.ToInt16(hdfisDiscountAdd.Value) == (int)TaxSettingEnum.BOTHHEADERITEM)
                                {
                                    btnApply.Visible = true;
                                    imgPopupAdd.Visible = true;
                                    grdTaxDetails.Columns[3].Visible = true;
                                    ddlPopupTaxType.Enabled = true;
                                }

                                #endregion
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("DiscountDetails").ToString() + "','600','300');", true);
                            }
                        }
                        break;
                    #endregion

                    #region TAXAPPLY
                    case ActionsEnum.TAXAPPLY:
                        hdfApplyTax.Value = "1";
                        // To Proportionate Tax after apply discount
                        if (hdfTaxCategory.Value == ((int)TaxType.Discount).ToString())
                        {
                            IsTaxProportionate = true;
                        }
                        else
                        {
                            IsTaxProportionate = false;
                        }
                        SetDetailTax(null);
                        SetHdrTax();
                        ResetForm(ControlsEnum.TAXPOPUPGRID);
                        //Settings of TaxPayableDiv
                        if (hdfIsTaxPayable.Value.ToString() == "1")
                        {
                            SetTaxPayableDiv();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                        //end
                        IsHeaderTax = false;
                        IsEditMode = false;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        SetEffectiveRate();
                        break;
                    #endregion

                    #region Recalculate
                    case ActionsEnum.RECALCULATE:
                        #region Reset GRN Allocation Details
                        if (POInvoiceHeaderSession.OrderDetail != null && POInvoiceHeaderSession.OrderDetail.Count > 0)
                        {
                            foreach (POInvoiceDetails ordrDtl in POInvoiceHeaderSession.OrderDetail)
                            {
                                if (ordrDtl.VID_QTY_INVOICED == 0)
                                    ordrDtl.GRNDtl.ForEach(f => f.VGL_QTY_INVOICED = 0);
                            }
                        }
                        #endregion
                        TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
                        SetDetailTax(null);
                        SetHdrTax();
                        //Settings of TaxPayableDiv
                        if (hdfIsTaxPayable.Value.ToString() == "1")
                        {
                            SetTaxPayableDiv();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                        //end
                        break;
                    #endregion

                    #region TAXADD
                    case ActionsEnum.TAXADD:
                        bool errorTaxAdd = false;
                        bool errorTaxAmount = false;
                        if (TempPOInvoiceHeaderSession != null)
                        {
                            invoiceHeaderObj = TempPOInvoiceHeaderSession;
                            tempInvTaxSplitObj = null;
                            if (IsHeaderTax)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                {
                                    //tempInvTaxSplitObj = invoiceHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.VTL_TAX == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    tempInvTaxSplitObj = invoiceHeaderObj.TaxHdr.FirstOrDefault(rfq => rfq.VTL_TAX == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));

                                }
                                else
                                {
                                    tempInvTaxSplitObj = invoiceHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.VTL_NAME == txtPopupOther.Text.Trim() && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                            }
                            else
                            {
                                soInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.VID_PO) == SelectedPOPK);
                                if (soInvoiceDetailsObj != null)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        tempInvTaxSplitObj = soInvoiceDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.VTL_TAX == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        tempInvTaxSplitObj = soInvoiceDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.VTL_NAME == txtPopupOther.Text.Trim() && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
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
                                    soInvTaxHdrObj.VTL_INVOICE_DTL = POInvoicePK;
                                    soInvTaxHdrObj.VTL_SL_NO = 1;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        soInvTaxHdrObj.VTL_TAX = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        soInvTaxHdrObj.VTL_TYPE = 1;
                                    }
                                    else
                                        soInvTaxHdrObj.VTL_TYPE = 2;
                                    soInvTaxHdrObj.VTL_TAX_TEXT = HttpUtility.HtmlEncode(SelectedTaxText);
                                    soInvTaxHdrObj.VTL_NAME = HttpUtility.HtmlEncode(txtPopupOther.Text);
                                    soInvTaxHdrObj.VTL_PK = 0;
                                    //rfqTaxHdrObj.VTL_TAX_CATEGORY_TEXT = "Tax";
                                    soInvTaxHdrObj.VTL_TAX_CATEGORY = Convert.ToInt32(hdfTaxCategory.Value);
                                    soInvTaxHdrObj.VTL_TAX_FORMULA = string.IsNullOrEmpty(hdfTaxFormula.Value) ? string.Empty : hdfTaxFormula.Value;
                                    if (IsHeaderTax)
                                    {
                                        if (soInvTaxHdrObj.VTL_TAX_CATEGORY == (int)TaxType.Discount)
                                        {
                                            totalAmt = 0;
                                            currentTotal = 0;
                                            taxAmt = 0;

                                            totalAmt = Convert.ToDouble(invoiceHeaderObj.IVH_AMOUNT_TC + invoiceHeaderObj.IVH_AMOUNT_NET_TC_ADJ);
                                            currentTotal = invoiceHeaderObj.TaxHdr.Where(quotation => quotation.VTL_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.VTL_TAX_AMT);
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
                                            if (Convert.ToInt16(hdfisTaxAdd.Value) == (int)TaxSettingEnum.BOTHHEADERITEM)
                                            {
                                                soInvTaxHdrObj.VTL_HAS_SUB_TOTAL = chkSubTotal.Checked ? 1 : 0;
                                                soInvTaxHdrObj.VTL_HAS_DISCOUNT = chkDiscount.Checked ? 1 : 0;
                                                soInvTaxHdrObj.VTL_HAS_OTHER_CHARGE = chkOtherCharges.Checked ? 1 : 0;
                                            }
                                            taxHdrList = invoiceHeaderObj.TaxHdr.ToList();
                                            taxHdrList.Add(soInvTaxHdrObj);
                                            invoiceHeaderObj.TaxHdr = taxHdrList;
                                        }
                                    }
                                    else
                                    {
                                        soInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.VID_PO) == SelectedPOPK);
                                        if (soInvoiceDetailsObj != null)
                                        {
                                            if (soInvTaxHdrObj.VTL_TAX_CATEGORY == (int)TaxType.Discount)
                                            {
                                                totalAmt = 0;
                                                currentTotal = 0;
                                                taxAmt = 0;

                                                totalAmt = soInvoiceDetailsObj.VID_AMOUNT;
                                                currentTotal = soInvoiceDetailsObj.TaxDtl.Where(quotation => quotation.VTL_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.VTL_TAX_AMT);
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
                                                    taxHdrList = soInvoiceDetailsObj.TaxDtl.ToList();
                                                    taxHdrList.Add(soInvTaxHdrObj);
                                                    invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.VID_PO) == SelectedPOPK).TaxDtl = taxHdrList;
                                                }
                                                else
                                                {
                                                    isValidDisc = false;
                                                }
                                            }
                                            else
                                            {
                                                taxHdrList = soInvoiceDetailsObj.TaxDtl.ToList();
                                                taxHdrList.Add(soInvTaxHdrObj);
                                                invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.VID_PO) == SelectedPOPK).TaxDtl = taxHdrList;
                                            }
                                        }
                                    }
                                    TempPOInvoiceHeaderSession = invoiceHeaderObj;
                                    TempInvoiceHeaderTemp = TempPOInvoiceHeaderSession;
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
                                    //txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
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
                        if (TempPOInvoiceHeaderSession != null)
                        {
                            invoiceHeaderObj = TempPOInvoiceHeaderSession;
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
                                        //tempInvTaxSplitObj = invoiceHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.VTL_TAX == taxPK && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        tempInvTaxSplitObj = invoiceHeaderObj.TaxHdr.FirstOrDefault(rfq => rfq.VTL_TAX == taxPK && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        if (hdfTaxName != null)
                                        {
                                            tempInvTaxSplitObj = invoiceHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.VTL_NAME == hdfTaxName.Value && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
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
                                    soInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.VID_PO) == SelectedPOPK);
                                    if (soInvoiceDetailsObj != null)
                                    {
                                        if (taxPK > 0)
                                        {
                                            tempInvTaxSplitObj = soInvoiceDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.VTL_TAX == taxPK && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                        else
                                        {
                                            if (hdfTaxName != null)
                                            {
                                                tempInvTaxSplitObj = soInvoiceDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.VTL_NAME == hdfTaxName.Value && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                            }
                                        }
                                        soInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.VID_PO) == SelectedPOPK);
                                        if (soInvoiceDetailsObj != null)
                                        {
                                            taxHdrList = soInvoiceDetailsObj.TaxDtl.ToList();
                                            taxHdrList.Remove(tempInvTaxSplitObj);
                                            invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.VID_PO) == SelectedPOPK).TaxDtl = taxHdrList;
                                        }
                                    }
                                }

                                TempPOInvoiceHeaderSession = invoiceHeaderObj;
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
                                            txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits, MidpointRounding.AwayFromZero).ToString(hdfCurrencyFormat.Value);
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
                                txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits, MidpointRounding.AwayFromZero).ToString(hdfCurrencyFormat.Value);
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
                        break;
                    #endregion

                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        if (PIType == "21")
                            FillProcessID(21);
                        else
                            FillProcessID(1);
                        ResetForm(ControlsEnum.INVOICELIST);
                        CurrPK = 0;
                        hdfIVHPK.Value = "";
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region CALCULATEDTLTAX
                    case ActionsEnum.CALCULATEDTLTAX:
                        SetDetailTax(sender);
                        SetHdrTax();
                        ResetForm(ControlsEnum.TAXPOPUPGRID);
                        SetUIValuesToObject(ControlsEnum.POINVDETAIL);
                        //Settings of TaxPayableDiv
                        if (hdfIsTaxPayable.Value.ToString() == "1")
                        {
                            SetTaxPayableDiv();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                        //end
                        ValidateAdjAmount();
                        SetEffectiveRate();
                        break;
                    #endregion

                    #region CALCULATEHDRTAX
                    case ActionsEnum.CALCULATEHDRTAX:
                        SetHdrTax();
                        break;
                    #endregion

                    #region Tabs
                    case ActionsEnum.INVOICE:
                        CheckUserRightsAndRedirect(Resources.PageURL.PoInvoicing);
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
                        //To Identify whether Purchase / Sales
                        Session[ERP.Utilities.SessionStrings.CrDrType] = ApplicationType.PI;
                        CheckUserRightsAndRedirect(Resources.PageURL.DrCrNote);
                        //Response.Redirect(Resources.PageURL.DrCrNote);
                        break;
                    case ActionsEnum.ACPAYABLES:
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            //RadioButton rbtn;
                            //rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            //if (rbtn.Checked)
                            //{
                            HiddenField hdfDept;
                            int dept;
                            CheckBox chkPIselect;
                            chkPIselect = (CheckBox)grdrow.FindControl("chkPIselect");
                            if (chkPIselect.Checked)
                            {
                                Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                                Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).ToolTip;//For Showing name in vendorddl of AccountPayable Page Completely 

                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                break;
                            }
                        }
                        CheckUserRightsAndRedirect(Resources.PageURL.AccountsPayable);
                        //Response.Redirect(Resources.PageURL.AccountsPayable);
                        break;
                    case ActionsEnum.POINVOICE:
                        SetUIEditView(commonActions);
                        break;
                    case ActionsEnum.NEW:
                        CheckUserRightsAndRedirect(Resources.PageURL.PurchaseOpeningInvoice);
                        //Response.Redirect(Resources.PageURL.PurchaseOpeningInvoice);
                        break;
                    #endregion

                    #region Edit
                    case ActionsEnum.EDIT:
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            //RadioButton rbtn;
                            //rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            // if (rbtn.Checked)
                            //{

                            HiddenField hdfDept;
                            //int selectedInvPK;
                            int dept;

                            CheckBox chkPIselect;
                            chkPIselect = (CheckBox)grdrow.FindControl("chkPIselect");
                            if (chkPIselect.Checked)
                            {
                                hdfGRNAttachmentsVisible.Value = "0";//for collapse grn attachement section
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);

                                hdfInOpeningInv = (HiddenField)grdrow.FindControl("hdfInOpeningInv");
                                if (hdfInOpeningInv.Value == "1")
                                {
                                    Session[ERP.Utilities.SessionStrings.InvoicePK] = CurrPK.ToString();
                                    Response.Redirect(Resources.PageURL.PurchaseOpeningInvoice);
                                    break;
                                }

                                Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                                Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).Text;
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                ////

                                //selectedInvPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                InvoiceCategory = Convert.ToInt32((grdrow.FindControl("hdfInvCategory") as HiddenField).Value);

                                if (Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1)
                                {
                                    btnSave.Visible = false;
                                    hdfIsInvCancelled.Value = "1";
                                }
                                else
                                {
                                    btnSave.Visible = true;
                                    hdfIsInvCancelled.Value = "0";
                                }

                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }

                                //WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                //base.WkfRefID = workflowCore.GetRefID(selectedInvPK, PageProcessID);

                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            SetUIEditView(commonActions);
                            if (InvoiceCategory == 2)
                                FillProcessID(21);
                            else
                                FillProcessID(1);

                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.POINVHEADER);
                            if (invoiceHeaderObj.IVH_TYPE == ((byte)PurchaseType.Local).ToString())
                            {
                                divVendorBranch.Visible = true;
                                vndPK = Convert.ToInt32(invoiceHeaderObj.IVH_VENDOR);
                                GetFieldValues(ControlsEnum.VENDORBRANCH);
                                SetFieldValues(ControlsEnum.VENDORBRANCH);
                            }
                            else
                            {
                                divVendorBranch.Visible = false;
                            }
                            SetFieldValues(ControlsEnum.POINVHEADER);
                            SetFieldValues(ControlsEnum.POINVDETAIL);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            SetFieldValues(ControlsEnum.GRNATTACHMENTS);
                            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            if (base.WkfRefID == 0)
                            {
                                int.TryParse(hdfSubDeptPk.Value, out ReqDept);
                                ucrWrkf.ReqDeptID = (PIType == "21") ? ReqDept : 0;
                            }
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                                //EntryStatus = EntryStatus.VIEWMODE;
                                ucrWrkf.ViewAction();
                            }
                            hdfApplyTax.Value = "1";// No need to create formula for Tax amount in TAX POPUP. 
                            SetDetailTax(null);
                            SetHdrTax();
                            if (grdInvoice.Rows.Count > 0)
                            {
                                SetSubTotal();
                            }
                            //Settings of TaxPayableDiv
                            if (hdfIsTaxPayable.Value.ToString() == "1")
                            {
                                SetTaxPayableDiv();
                            }
                            if (hdfIsShowAlert.Value == "0")
                                btnAlert.Visible = false;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                            //end                            
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
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            //RadioButton rbtn;
                            //rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            //if (rbtn.Checked)
                            //{
                            HiddenField hdfDept;
                            //int selectedInvPK;
                            int dept;

                            CheckBox chkPIselect;
                            chkPIselect = (CheckBox)grdrow.FindControl("chkPIselect");
                            if (chkPIselect.Checked)
                            {
                                hdfGRNAttachmentsVisible.Value = "0";//for collapse grn attachement section
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                hdfInOpeningInv = (HiddenField)grdrow.FindControl("hdfInOpeningInv");
                                if (hdfInOpeningInv.Value == "1")
                                {
                                    Session[ERP.Utilities.SessionStrings.InvoicePK] = CurrPK.ToString();
                                    Response.Redirect(Resources.PageURL.PurchaseOpeningInvoice);
                                    break;
                                }
                                Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                                Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).Text;
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                ////

                                //selectedInvPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                InvoiceCategory = Convert.ToInt32((grdrow.FindControl("hdfInvCategory") as HiddenField).Value);

                                if (Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1)
                                {
                                    //btnSave.Visible = false;
                                    hdfIsInvCancelled.Value = "1";
                                }
                                else
                                {
                                    //btnSave.Visible = true;
                                    hdfIsInvCancelled.Value = "0";
                                }
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }

                                //WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                //base.WkfRefID = workflowCore.GetRefID(selectedInvPK, PageProcessID);

                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            SetUIEditView(commonActions);
                            if (InvoiceCategory == 2)
                                FillProcessID(21);
                            else
                                FillProcessID(1);
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
                            GetFieldValues(ControlsEnum.POINVHEADER);
                            if (invoiceHeaderObj.IVH_TYPE == ((byte)PurchaseType.Local).ToString())
                            {
                                divVendorBranch.Visible = true;
                                vndPK = Convert.ToInt32(invoiceHeaderObj.IVH_VENDOR);
                                GetFieldValues(ControlsEnum.VENDORBRANCH);
                                SetFieldValues(ControlsEnum.VENDORBRANCH);
                            }
                            else
                            {
                                divVendorBranch.Visible = false;
                            }
                            SetFieldValues(ControlsEnum.POINVHEADER);
                            SetFieldValues(ControlsEnum.POINVDETAIL);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            SetFieldValues(ControlsEnum.GRNATTACHMENTS);
                            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
                            hdfApplyTax.Value = "1";// No need to use formula for Tax  amount
                            SetDetailTax(null);
                            SetHdrTax();
                            if (grdInvoice.Rows.Count > 0)
                            {
                                SetSubTotal();
                            }
                            //Settings of TaxPayableDiv
                            if (hdfIsTaxPayable.Value.ToString() == "1")
                            {
                                SetTaxPayableDiv();
                            }
                            if (hdfIsShowAlert.Value == "0")
                                btnAlert.Visible = false;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                            //end  

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
                            result = BusinessLogic.POInvoicing.POInvoiceBL.DeletePOInvoiceDetails(CurrPK, LastModifiedTime, ApplicationType.PI, currentUser.PKUser.ToString());
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.POInvoice);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm(ControlsEnum.INVOICELIST);
                                GetFieldValues(ControlsEnum.INVOICELIST);
                                SetFieldValues(ControlsEnum.INVOICELIST);
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
                            }
                        }
                        break;
                    #endregion

                    #region Clear
                    case ActionsEnum.CLEAR:
                        SelectedInvoicesInfoLst = null;
                        ResetForm(ControlsEnum.INVOICELIST);
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        break;
                    #endregion

                    #region Invoice List
                    case ActionsEnum.INVOICELIST:
                        if (PIType == "21")
                            FillProcessID(21);
                        else
                            FillProcessID(1);
                        CurrPK = 0;
                        hdfIVHPK.Value = "";
                        ResetForm(ControlsEnum.INVOICELIST);
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Invoice Details
                    case ActionsEnum.INVOICEDETAIL:
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            //RadioButton rbtn;
                            //rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            //if (rbtn.Checked)
                            //{
                            HiddenField hdfDept;
                            int dept;

                            CheckBox chkPIselect;
                            chkPIselect = (CheckBox)grdrow.FindControl("chkPIselect");
                            if (chkPIselect.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                hdfInOpeningInv = (HiddenField)grdrow.FindControl("hdfInOpeningInv");
                                if (hdfInOpeningInv.Value == "1")
                                {
                                    Session[ERP.Utilities.SessionStrings.InvoicePK] = CurrPK.ToString();
                                    Response.Redirect(Resources.PageURL.PurchaseOpeningInvoice);
                                    break;
                                }
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                ////

                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1)
                                {
                                    btnSave.Visible = false;
                                    hdfIsInvCancelled.Value = "1";
                                }
                                else
                                {
                                    btnSave.Visible = true;
                                    hdfIsInvCancelled.Value = "0";
                                }

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


                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.POINVHEADER);
                            hdfIvhGrp.Value = invoiceHeaderObj.IVH_GROUP.ToString();
                            if (invoiceHeaderObj.IVH_TYPE == ((byte)PurchaseType.Local).ToString())
                            {
                                divVendorBranch.Visible = true;
                                vndPK = Convert.ToInt32(invoiceHeaderObj.IVH_VENDOR);
                                GetFieldValues(ControlsEnum.VENDORBRANCH);
                                SetFieldValues(ControlsEnum.VENDORBRANCH);
                            }
                            else
                            {
                                divVendorBranch.Visible = false;
                            }
                            SetFieldValues(ControlsEnum.POINVHEADER);
                            SetFieldValues(ControlsEnum.POINVDETAIL);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            SetFieldValues(ControlsEnum.GRNATTACHMENTS);

                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            if (base.WkfRefID == 0)
                            {
                                int.TryParse(hdfSubDeptPk.Value, out ReqDept);
                                ucrWrkf.ReqDeptID = (PIType == "21") ? ReqDept : 0;
                            }
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                                //EntryStatus = EntryStatus.VIEWMODE;
                                ucrWrkf.ViewAction();
                            }

                            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
                            hdfApplyTax.Value = "1";// No need to use formula for Tax  amount
                            SetDetailTax(null);
                            SetHdrTax();
                            if (grdInvoice.Rows.Count > 0)
                            {
                                SetSubTotal();
                            }
                            //Settings of TaxPayableDiv
                            if (hdfIsTaxPayable.Value.ToString() == "1")
                            {
                                SetTaxPayableDiv();
                            }
                            if (hdfIsShowAlert.Value == "0")
                                btnAlert.Visible = false;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                            //end                            
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

                        #region Checking :If Other charges entered is exceeding the value given in PO
                        if (HdfIsContYesOtherCharges.Value != "1")
                        {

                            //if (Convert.ToDecimal(txtShipping.Text) <= Convert.ToDecimal(hdfOtherchargePO.Value))
                            //{
                            //}
                            //else
                            //{
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "OtherChargesExceeds", "$(document).ready(function(){ShowOtherChargesExceeds(2);});", true);
                            //    break;
                            //}

                        }
                        #endregion
                        if (ddlAddressType.Items.Count <= 0)
                        {
                            isContinue = false;

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ReqType").ToString()) + "');", true);
                            return;
                        }
                        if (GetGlobalResourceObject("ConfigurationsRes", "POInvoiceRequiredAdvanceDeduct").ToString() == "1")
                        {
                            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
                            deductionDtlList = new List<POAdvDeductionDetails>();
                            deductionDtlList = TempPOInvoiceHeaderSession.DeductionDetails.ToList();
                            if ((deductionDtlList != null && deductionDtlList.Count > 0))
                            {
                                decimal balAllocation = deductionDtlList.Sum(b => (Convert.ToDecimal(b.PVH_PO_PAID_AMT) - b.IVH_AMOUNT_ALLOCATED) - (b.VAD_TAX_AMOUNT + b.VAD_OTHER_AMOUNT));

                                bool pendingDeduction = false;
                                if (balAllocation <= Convert.ToDecimal(txtHdrTotal.Text))
                                {
                                    if (Convert.ToDecimal(txtHdrDeduction.Text) < balAllocation)
                                        pendingDeduction = true;
                                }

                                else if (balAllocation > (Convert.ToDecimal(txtHdrTotal.Text) + (Convert.ToDecimal(txtPriceAdj.Text.Trim()))))
                                {
                                    if (Convert.ToDecimal(txtHdrDeduction.Text) != Convert.ToDecimal(txtHdrTotal.Text))
                                        pendingDeduction = true;
                                }
                                if (pendingDeduction)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_Without_Allocation").ToString()) + "');", true);
                                    return;
                                }
                            }
                        }
                        //Show WorkFlow Popup
                        isContinue = true;
                        if ((hdfSaveWithoutAllocation.Value == "0") || (hdfSaveWithoutAllocation.Value == ""))
                        {
                            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
                            deductionDtlList = new List<POAdvDeductionDetails>();
                            deductionDtlList = TempPOInvoiceHeaderSession.DeductionDetails.ToList();

                            if (IsAdvInvHasTax)
                            {
                                if (Convert.ToDouble(txtHdrDeduction.Text) <= 0 && (deductionDtlList != null && deductionDtlList.Count > 0))
                                {
                                    isContinue = false;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowSaveWithoutAllocationConfirm('" + (sender as Button).ID + "');", true);
                                }
                            }
                            else
                            {
                                if (Convert.ToDouble(txtTotalDeduction.Text) <= 0 && (deductionDtlList != null && deductionDtlList.Count > 0))
                                {
                                    isContinue = false;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowSaveWithoutAllocationConfirm('" + (sender as Button).ID + "');", true);
                                }
                            }
                        }

                        #region Invoice Now Checking :If Inv.Now Qty entered  exceeds Ordered Qty
                        if (hdfIsContInvoiceNowQty.Value != "1")
                        {
                            foreach (GridViewRow grdrowitem in grdInvoice.Rows)
                            {
                                TextBox txtInvNow = (TextBox)grdrowitem.FindControl("txtInvNow");
                                Label lblOrderQuantity = (Label)grdrowitem.FindControl("lblOrderQuantity");
                                Label lblInvQuantity = (Label)grdrowitem.FindControl("lblInvQuantity");
                                decimal RemainingQty = Convert.ToDecimal(lblOrderQuantity.Text.Replace(",", "")) - Convert.ToDecimal(lblInvQuantity.Text.Replace(",", ""));

                                if (Convert.ToDecimal(txtInvNow.Text) > RemainingQty)
                                {
                                    isContinue = false;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InvoiceNowExceeds", "$(document).ready(function(){ShowInvoiceNowQtyExceeds(2);});", true);
                                    break;

                                }
                            }

                        }
                        #endregion
                        #region Checking SubTotal amount greater than zero or not
                        txtSubTotalFooterAmt = (TextBox)grdInvoice.FooterRow.FindControl("txtSubTotalFooter");
                        subTotalAmt = txtSubTotalFooterAmt == null ? 0 : string.IsNullOrEmpty(txtSubTotalFooterAmt.Text.Trim()) ? 0 : Convert.ToDouble(txtSubTotalFooterAmt.Text.Trim());
                        if (subTotalAmt == 0)
                        {
                            isContinue = false;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_SubTotal").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        #endregion
                        if (isContinue)
                        {
                            hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                            ucrWrkf.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        }
                        break;
                    #endregion

                    #region Submit
                    case ActionsEnum.SUBMIT:
                        isContinue = true;
                        if ((hdfSaveWithoutAllocation.Value == "0") || (hdfSaveWithoutAllocation.Value == ""))
                        {
                            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
                            deductionDtlList = new List<POAdvDeductionDetails>();
                            deductionDtlList = TempPOInvoiceHeaderSession.DeductionDetails.ToList();
                            if (IsAdvInvHasTax)
                            {
                                if (Convert.ToDouble(txtHdrDeduction.Text) <= 0 && (deductionDtlList != null && deductionDtlList.Count > 0))
                                {
                                    isContinue = false;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowSaveWithoutAllocationConfirm('" + (sender as Button).ID + "');", true);
                                }
                            }
                            else
                            {
                                if (Convert.ToDouble(txtTotalDeduction.Text) <= 0 && (deductionDtlList != null && deductionDtlList.Count > 0))
                                {
                                    isContinue = false;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowSaveWithoutAllocationConfirm('" + (sender as Button).ID + "');", true);
                                }
                            }
                        }
                        if (isContinue)
                        {
                            //Show WorkFlow Popup
                            hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                            ucrWrkf.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        }
                        break;
                    #endregion

                    #region WRKFSubmit
                    case ActionsEnum.WRKFSUBMIT:
                        //Submit Activity
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else if (!ValidateAssetType())
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_AssetType").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            isCancelled = false;
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                //   hasValidRate = false;
                                invoiceHeaderObj = new POInvoiceHeader();
                                invoiceHeaderObj = (POInvoiceHeader)SetUIValuesToObject(ControlsEnum.POINVHEADER);
                                if (hdfExchangeRate.Value != "-1")
                                {
                                    invoiceHeaderObj.WKF_FLAG = 1;
                                    if (invoiceHeaderObj.IVH_AMOUNT_TC > 0)
                                    {
                                        if (hasValidRate)
                                        {
                                            if (invoiceHeaderObj != null && invoiceHeaderObj.OrderDetail != null)
                                            {
                                                if (invoiceHeaderObj.IVH_TYPE == "1" && hdfIscontYesDate.Value == "0")//PO Type Import and before confirmation
                                                {
                                                    bool IsDateChange = false;
                                                    invoiceHeaderObj.OrderDetail.ForEach(a =>
                                                    {
                                                        if (a.GRNDtl != null && a.GRNDtl.Where(d => Convert.ToDateTime(d.VGL_GRN_DATE) != Convert.ToDateTime(txtInvoiceDate.Text)).Count() > 0)
                                                        {
                                                            IsDateChange = true;
                                                        }
                                                    });
                                                    if (IsDateChange)
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDifferentDate", "$(document).ready(function(){ShowDifferentDate();});", true);
                                                        break;
                                                    }
                                                }
                                                #region ALERTSAVE
                                                TypeRef = lblInvoiceNo.Text.Trim();
                                                GetFieldValues(ControlsEnum.ALERTCONFIG);
                                                int isAlert = 0;
                                                invoiceHeaderObj.ALERT_FLAG = 0;
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
                                                        invoiceHeaderObj.ALERT_FLAG = 1;
                                                        invoiceHeaderObj.ATH_NO = alertBoObj.ATH_NO;
                                                        invoiceHeaderObj.ATH_DATE = ((DateTime)alertBoObj.ATH_DATE).ToString(Resources.Constants.DateFormatShort); //alertBoObj.ATH_DATE.ToString(); 
                                                        invoiceHeaderObj.ATH_TRX_TYPE = alertBoObj.ATH_TRX_TYPE;
                                                        invoiceHeaderObj.ATH_TRX_PK = alertBoObj.ATH_TRX_PK.ToString();
                                                        invoiceHeaderObj.ATH_DUE_DATE = ((DateTime)alertBoObj.ATH_DUE_DATE).ToString(Resources.Constants.DateFormatShort); //alertBoObj.ATH_DUE_DATE.ToString();
                                                        invoiceHeaderObj.ATH_NAME = alertBoObj.ATH_NAME.ToString();
                                                        invoiceHeaderObj.ATH_BASIS = alertBoObj.ATH_BASIS.ToString();
                                                        invoiceHeaderObj.ATH_ALERT_TYPE = alertBoObj.ATH_ALERT_TYPE.ToString();
                                                        invoiceHeaderObj.ATH_NOTIFY_BFR = alertBoObj.ATH_NOTIFY_BFR.ToString();
                                                        invoiceHeaderObj.ATH_NOTIFY_BFR_UOM = alertBoObj.ATH_NOTIFY_BFR_UOM.ToString();
                                                        invoiceHeaderObj.ATH_REMARKS = alertBoObj.ATH_REMARKS;
                                                        invoiceHeaderObj.ATH_NOTIFY_MESSAGE = alertBoObj.ATH_NOTIFY_MESSAGE.ToString();
                                                        invoiceHeaderObj.ATH_NOTIFY_EMAIL = alertBoObj.ATH_NOTIFY_EMAIL.ToString();
                                                        invoiceHeaderObj.ATH_NOTIFY_SMS = alertBoObj.ATH_NOTIFY_SMS.ToString();
                                                        invoiceHeaderObj.ATH_TRX_DATE = ((DateTime)alertBoObj.ATH_TRX_DATE).ToString(Resources.Constants.DateFormatShort); //alertBoObj.ATH_TRX_DATE.ToString();
                                                        invoiceHeaderObj.ATH_DUE_DAYS = alertBoObj.ATH_DUE_DAYS.ToString();
                                                        invoiceHeaderObj.ATH_NARRATION = alertBoObj.ATH_NARRATION;
                                                        invoiceHeaderObj.ATH_STATUS = alertBoObj.ATH_STATUS.ToString();
                                                        //alertresult = BusinessLogic.AlertManagement.Alerts.SaveAlertDetails(alertBoObj);
                                                    }
                                                }
                                                #endregion
                                                invoiceHeaderObj.ATL_ACTION = (byte)LogAction.NEW;
                                                SaveTransaction(invoiceHeaderObj, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
                                            }
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup(); ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Empty_Rate").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup(); ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_SubTotal").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                        return;
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
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(CurrPK, ApplicationType.PI))
                                {
                                    // ucrWrkf.ApplicationID = CurrPK;
                                    isCancelled = true;
                                    SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));

                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_PI_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                    {
                                        if (PIType == "21")
                                            FillProcessID(21);
                                        else
                                            FillProcessID(1);
                                    }
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.INVOICELIST);
                                    GetFieldValues(ControlsEnum.INVOICELIST);
                                    SetFieldValues(ControlsEnum.INVOICELIST);
                                }
                            }
                            else
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                            //ucrWrkf.ApplicationID = CurrPK;

                            //if (ucrWrkf.ApplicationID > 0)
                            //{
                            //    ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                            //    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                            //    //Do WorkFlow if WorkFlow has Actions
                            //    if (ddlWkfAction.Items.Count > 0)
                            //    {
                            //        action = ddlWkfAction.SelectedItem.ToString();
                            //        result = ucrWrkf.DoWorkFlow();

                            //    }
                            //}
                        }
                        break;
                    #endregion

                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        SelectedInvoicesInfoLst = null;
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        break;
                    #endregion

                    #region Pick for Paying
                    case ActionsEnum.PICKFORPAYMENT:
                        //ResetForm(ControlsEnum.RESETPAYMENT);
                        SetUIValuesToObject(ControlsEnum.PICKFORPAYMENT);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);

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
                        InvCategory = 0;
                        SelectedInvoicesCrDr = null;
                        SelectedInvoices = null;
                        SelectedINVTax = null;
                        SelectedINVTaxList = new List<decimal>();
                        btnPickForPayment.Text = GetLocalResourceObject("PickPoForPayment").ToString();
                        btnPickForCrDrNote.Text = GetLocalResourceObject("PickForCrDrNote").ToString();
                        //Resetting Color
                        hdfSelectedItemPk.Value = "0";

                        break;
                    #endregion

                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
                        if (!ValidateAssetType())
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_AssetType").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            return;
                        }
                        else
                        {
                            bool IsAssetExist = false;
                            foreach (GridViewRow row in grdInvoice.Rows)
                            {
                                HiddenField hdfItemIsAsset = (HiddenField)row.FindControl("hdfItemIsAsset");
                                if (hdfItemIsAsset.Value == "1")
                                {
                                    IsAssetExist = true;
                                    break;
                                }
                            }
                            if (IsAssetExist)
                            {
                                var itemToPost = (POInvoiceHeader)SetUIValuesToObject(ControlsEnum.POINVHEADER);
                                string strXml = CommonFunctions.XmlSerialize<POInvoiceHeader>(itemToPost);

                                int ValidToPost = BusinessLogic.CommonManagement.CommonBL.ValidationForPosting(strXml);
                                if (ValidToPost < 0)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_Post").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    return;
                                }
                            }
                        }
                        finInvoiceVndHdrObj = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                        finInvoiceVndHdrObj.IVH_PK = CurrPK;
                        GetFieldValues(ControlsEnum.POINVHEADER);
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
                        if (PIType == "21")
                            FillProcessID(21);
                        else
                            FillProcessID(1);
                        ResetForm(ControlsEnum.INVOICELIST);
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        break;
                    #endregion

                    #region Journalize Save
                    case ActionsEnum.JOURNALIZESAVE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ((Button)sender).CommandName = ActionsEnum.SAVE.ToString();
                        ucrWrkf.Reset();
                        if (PIType == "21")
                            FillProcessID(21);
                        else
                            FillProcessID(1);
                        ResetForm(ControlsEnum.INVOICELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
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
                                result = (int)poInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);
                            }
                            else if (Transaction == "DELETE")
                            {
                                poInvoiceServiceClient = new POInvoiceService();
                                poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                                result = (int)poInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                            }
                        }
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        if (PIType == "21")
                            FillProcessID(21);
                        else
                            FillProcessID(1);
                        ResetForm(ControlsEnum.INVOICELIST);
                        //if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ReferanceID > 0)
                        //{
                        //    Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        //    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.InboxURL));
                        //}
                        //else
                        //{
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
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
                                poInvoiceServiceClient = new POInvoiceService();
                                poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                                result = (int)poInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);
                            }
                            else if (Transaction == "DELETE")
                            {
                                poInvoiceServiceClient = new POInvoiceService();
                                poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                                result = (int)poInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                            }
                        }
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        if (PIType == "21")
                            FillProcessID(21);
                        else
                            FillProcessID(1);
                        ResetForm(ControlsEnum.INVOICELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        break;
                    #endregion

                    #region Journalize Cancel
                    case ActionsEnum.JOURNALIZECANCEL:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        if (PIType == "21")
                            FillProcessID(21);
                        else
                            FillProcessID(1);
                        ResetForm(ControlsEnum.INVOICELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        break;
                    #endregion

                    #region Alert
                    case ActionsEnum.ALERT:
                        ucrAlert.TypeCode = POGroup == POInvoiceGroup.Goods ? ApplicationType.PI
                            : POGroup == POInvoiceGroup.Services ? ApplicationType.PSI : ApplicationType.EI;
                        ucrAlert.TypePK = CurrPK;
                        ucrAlert.TypeRef = lblInvoiceNo.Text.Trim();
                        ucrAlert.TrxDate = string.IsNullOrEmpty(txtInvoiceDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtInvoiceDate.Text.Trim());
                        ucrAlert.TypeText = GetLocalResourceObject("Alert_Type_Text").ToString();
                        ucrAlert.TypePartyName = lblCustomerTxt.ToolTip;
                        ucrAlert.GetAlertList();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divAlert]','Alert','800','400');", true);
                        break;
                    #endregion

                    #region Deduction Popup
                    case ActionsEnum.DEDUCTIONHEADER:
                        if (POInvoiceHeaderSession != null)
                        {
                            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
                            //lblDedSaleOrderNo.Text = ERP.Utilities.CommonFunctions.GetShortString(lbtnSoNoTxt.ToolTip, 15);
                            //lblDedSaleOrderNo.ToolTip = lbtnSoNoTxt.ToolTip;
                            lblDedCustomer.Text = ERP.Utilities.CommonFunctions.GetShortString(lblCustomerTxt.ToolTip, 36);
                            lblDedCustomer.ToolTip = lblCustomerTxt.ToolTip;
                            //lblDedSaleOrderDate.Text = ERP.Utilities.CommonFunctions.GetShortString(lblSODateTxt.ToolTip, 15);
                            //lblDedSaleOrderDate.ToolTip = lblSODateTxt.ToolTip;
                            //lblDedInvoiceNo.Text = ERP.Utilities.CommonFunctions.GetShortString(lblInvoiceNo.Text, 15);
                            //lblDedInvoiceNo.ToolTip = lblInvoiceNo.Text;
                            //lblDedSaleInvoiceDate.Text = ERP.Utilities.CommonFunctions.GetShortString(txtInvoiceDate.Text, 15);
                            //lblDedSaleInvoiceDate.ToolTip = txtInvoiceDate.Text;
                            lblDedCurrency.Text = ERP.Utilities.CommonFunctions.GetShortString(lblCurrencyTxt.ToolTip, 15);
                            lblDedCurrency.ToolTip = lblCurrencyTxt.ToolTip;
                            SetFieldValues(ControlsEnum.DEDUCTIONPOPUPGRID);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TotalSplit", "$(document).ready(function () { CalculateTotalSplit();});", true);

                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){CalculateTotalSplit();});", true);                           
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divDeduction]','" + GetLocalResourceObject("DeductionDetails").ToString() + "','1090','400');", true);
                            //EditTaxOtherCharge Setting
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "EditTaxOtherCharge", "$(document).ready(function () { EnableDisableTaxOtherCharge();});", true);
                        }
                        break;
                    #endregion

                    #region DEDUCTIONAPPLY
                    case ActionsEnum.DEDUCTIONAPPLY:
                        bool close = true;
                        if (POInvoiceHeaderSession != null)
                        {
                            hdfIsDedApplyClick.Value = "1";

                            //Reset all line item tax and discount
                            POInvoiceHeaderSession = TempPOInvoiceHeaderSession;
                            if (POInvoiceHeaderSession.OrderDetail != null && POInvoiceHeaderSession.OrderDetail.Count > 0)
                            {
                                foreach (POInvoiceDetails dtl in POInvoiceHeaderSession.OrderDetail)
                                {
                                    POInvoicePK = string.IsNullOrEmpty(dtl.VID_PK.ToString()) ? 0 : Convert.ToInt32(dtl.VID_PK);
                                    SelectedItemPK = string.IsNullOrEmpty(dtl.VID_ITEM.ToString()) ? 0 : Convert.ToInt32(dtl.VID_ITEM);
                                    ReSetDetailTax(dtl.VID_AMOUNT);
                                }
                            }
                            ReSetHdrDisc();
                            totalAllocatedDiscount = 0;
                            if (grdDeduction.Rows.Count > 0)
                            {
                                HiddenField hdfDedTotalAllocateNowFooterSplit = grdDeduction.FooterRow.FindControl("hdfDedTotalAllocateNowFooterSplit") as HiddenField;
                                if (hdfDedTotalAllocateNowFooterSplit != null && !string.IsNullOrEmpty(hdfDedTotalAllocateNowFooterSplit.Value))
                                {
                                    HiddenField hdfOtherTotalFooterSplit = grdDeduction.FooterRow.FindControl("hdfOtherTotalFooterSplit") as HiddenField;
                                    HiddenField hdfTaxTotalFooterSplit = grdDeduction.FooterRow.FindControl("hdfTaxTotalFooterSplit") as HiddenField;

                                    List<POAdvDeductionDetails> TempsoAdvDeductionDetailsList = null;

                                    double grossAmt = 0;
                                    double deduction = 0;
                                    double otherCharges = 0;
                                    double tax = 0;
                                    double PriceAdj = 0;
                                    double.TryParse(txtHdrTotal.Text.Trim(), out grossAmt);
                                    double.TryParse(hdfDedTotalAllocateNowFooterSplit.Value.Replace(",", ""), out deduction);
                                    if (hdfOtherTotalFooterSplit != null && !string.IsNullOrEmpty(hdfOtherTotalFooterSplit.Value))
                                        double.TryParse(hdfOtherTotalFooterSplit.Value.Trim(), out otherCharges);
                                    if (hdfTaxTotalFooterSplit != null && !string.IsNullOrEmpty(hdfTaxTotalFooterSplit.Value))
                                        double.TryParse(hdfTaxTotalFooterSplit.Value.Trim(), out tax);
                                    if (IsAdvInvHasTax)
                                    {
                                        deduction = double.Parse((deduction - (tax + otherCharges)).ToString());
                                    }
                                    else
                                    {
                                        double.TryParse(txtTotal.Text.Trim(), out grossAmt);
                                    }
                                    deduction = Math.Round(deduction, 2);
                                    TempsoAdvDeductionDetailsList = new List<POAdvDeductionDetails>();
                                    PriceAdj = Convert.ToDouble(txtPriceAdj.Text.Trim());
                                    if (deduction <= (grossAmt + PriceAdj))
                                    {
                                        decimal amtAdjAdvDeductionTotal = 0;
                                        foreach (GridViewRow grdRow in grdDeduction.Rows)
                                        {
                                            HiddenField hdfDeductionPK = grdRow.FindControl("hdfDeductionPK") as HiddenField;
                                            HiddenField hdfAdvInvoicePK = grdRow.FindControl("hdfAdvInvoicePK") as HiddenField;
                                            HiddenField hdfPaymentPK = grdRow.FindControl("hdfPaymentPK") as HiddenField;
                                            Label lblTaxSplit = grdRow.FindControl("lblTaxSplit") as Label;
                                            //Label lblOtherAmountSplit = grdRow.FindControl("lblOtherAmountSplit") as Label;
                                            TextBox txtOtherAmountSplit = grdRow.FindControl("txtOtherAmountSplit") as TextBox;
                                            HiddenField hdfDedPOHdr = grdRow.FindControl("hdfDedPOHdr") as HiddenField;
                                            HiddenField hdfCurPaidOtherAmount = grdRow.FindControl("hdfCurPaidOtherAmount") as HiddenField;
                                            HiddenField hdfCurPaidTax = grdRow.FindControl("hdfCurPaidTax") as HiddenField;
                                            HiddenField hdfCurPaidDisc = grdRow.FindControl("hdfCurPaidDisc") as HiddenField;
                                            HiddenField hdfReceiptDTLPK = grdRow.FindControl("hdfPaymentDTLPK") as HiddenField;
                                            long deductionPk = hdfDeductionPK != null && !string.IsNullOrEmpty(hdfDeductionPK.Value.Trim())
                                                ? Convert.ToInt64(hdfDeductionPK.Value) : 0;
                                            long advInvPk = hdfAdvInvoicePK != null && !string.IsNullOrEmpty(hdfAdvInvoicePK.Value.Trim())
                                                ? Convert.ToInt64(hdfAdvInvoicePK.Value) : 0;
                                            long advPaymentPk = hdfPaymentPK != null && !string.IsNullOrEmpty(hdfPaymentPK.Value.Trim())
                                                ? Convert.ToInt64(hdfPaymentPK.Value) : 0;
                                            long PoPk = hdfDedPOHdr != null && !string.IsNullOrEmpty(hdfDedPOHdr.Value.Trim())
                                               ? Convert.ToInt64(hdfDedPOHdr.Value) : 0;

                                            PaymentMpgPK = Convert.ToInt32(hdfReceiptDTLPK.Value);
                                            List<POAdvDeductionDetails> soAdvDeductionDetailsList = null;

                                            TextBox txtDedAllocateNowSplit = grdRow.FindControl("txtDedAllocateNowSplit") as TextBox;
                                            Label lblDedTaxAmount = grdRow.FindControl("lblDedTaxAmount") as Label;
                                            Label lblDedReceiptAmount = grdRow.FindControl("lblDedReceiptAmount") as Label;
                                            if (deductionPk > 0)
                                                soAdvDeductionDetailsList = POInvoiceHeaderSession.DeductionDetails.Where(aa => aa.VAD_PK == deductionPk).ToList();
                                            else
                                                soAdvDeductionDetailsList = POInvoiceHeaderSession.DeductionDetails.Where(aa => aa.VAD_PAYMENT_HDR == advPaymentPk && aa.VAD_INVOICE_ADV == advInvPk && aa.IVM_PO_HDR == PoPk).ToList();
                                            if (soAdvDeductionDetailsList.Count > 0)
                                            {
                                                soAdvDeductionDetailsList[0].VAD_AMOUNT = txtDedAllocateNowSplit != null && !string.IsNullOrEmpty(txtDedAllocateNowSplit.Text.Trim()) ?
                                                      Convert.ToDecimal(txtDedAllocateNowSplit.Text.Trim()) : 0;
                                                soAdvDeductionDetailsList[0].VAD_OTHER_AMOUNT = hdfCurPaidOtherAmount != null && !string.IsNullOrEmpty(hdfCurPaidOtherAmount.Value.Trim()) ?
                                                     Convert.ToDecimal(hdfCurPaidOtherAmount.Value.Trim()) : 0;
                                                soAdvDeductionDetailsList[0].VAD_TAX_AMOUNT = hdfCurPaidTax != null && !string.IsNullOrEmpty(hdfCurPaidTax.Value.Trim()) ?
                                                     Convert.ToDecimal(hdfCurPaidTax.Value.Trim()) : 0;
                                                soAdvDeductionDetailsList[0].VAD_DISC_AMOUNT = hdfCurPaidDisc != null && !string.IsNullOrEmpty(hdfCurPaidDisc.Value.Trim()) ?
                                                    Convert.ToDecimal(hdfCurPaidDisc.Value.Trim()) : 0;
                                                soAdvDeductionDetailsList[0].VAD_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                                if (Convert.ToDecimal(lblDedTaxAmount.Text) > 0)
                                                    amtAdjAdvDeduction = ((soAdvDeductionDetailsList[0].IVM_ADJUST_AMOUNT) / Convert.ToDecimal(lblDedTaxAmount.Text) * Convert.ToDecimal(txtDedAllocateNowSplit.Text));
                                                soAdvDeductionDetailsList[0].VAD_ADJUST_AMOUNT = amtAdjAdvDeduction;
                                                amtAdjAdvDeductionTotal += amtAdjAdvDeduction;
                                            }

                                            totalAllocatedTax = soAdvDeductionDetailsList[0].VAD_TAX_AMOUNT;
                                            //totalAllocatedDiscount = soAdvDeductionDetailsList[0].IVH_DISCOUNT_TC;
                                            totalAllocatedDiscount += soAdvDeductionDetailsList[0].VAD_DISC_AMOUNT;
                                            //totalAllocatedDiscount = soAdvDeductionDetailsList[0].VAD_DISC_AMOUNT == 0 ? soAdvDeductionDetailsList[0].IVM_DISCOUNT_AMOUNT : soAdvDeductionDetailsList[0].VAD_DISC_AMOUNT;// Convert.ToDecimal(hdfCurPaidDisc.Value) == 0 ? soAdvDeductionDetailsList[0].ICM_DISCOUNT_AMOUNT : Convert.ToDecimal(hdfCurPaidDisc.Value);// soAdvDeductionDetailsList[0].ICM_DISCOUNT_AMOUNT;

                                            SetLineItemTax();
                                            if (Convert.ToDecimal(lblDedReceiptAmount.Text) > 0)
                                                TotalHDRDiscount += (HDRDiscount / Convert.ToDecimal(lblDedReceiptAmount.Text)) * Convert.ToDecimal(txtDedAllocateNowSplit.Text);
                                            TempsoAdvDeductionDetailsList.Add(soAdvDeductionDetailsList[0]);
                                        }
                                        // txtPriceAdj.Text = (Convert.ToDecimal(txtPriceAdj.Text) - amtAdjAdvDeduction).ToString(hdfCurrencyFormat.Value);                                        
                                        ////txtDiscDeducted.Text = txtDiscDeducted.ToolTip = totalAllocatedDiscount.ToString(hdfCurrencyFormat.Value);
                                        // txtHdrDeduction.Text = deduction.ToString(hdfCurrencyFormat.Value);
                                        //txtHdrDeduction.Text = txtHdrDeduction.ToolTip = (deduction - (double)amtAdjAdvDeduction).ToString(hdfCurrencyFormat.Value);

                                        if (IsAdvInvHasTax)
                                        {
                                            txtDiscDeducted.Text = txtDiscDeducted.ToolTip = totalAllocatedDiscount.ToString(hdfCurrencyFormat.Value);
                                            //txtHdrDeduction.Text = txtHdrDeduction.ToolTip = (deduction - (double)amtAdjAdvDeductionTotal).ToString(hdfCurrencyFormat.Value);
                                            txtHdrDeduction.Text = txtHdrDeduction.ToolTip = (deduction).ToString(hdfCurrencyFormat.Value);
                                            txtDeductOtherCharges.Text = otherCharges.ToString(hdfCurrencyFormat.Value);
                                        }
                                        else
                                        {
                                            txtTotalDeduction.Text = txtTotalDeduction.ToolTip = (deduction - (double)amtAdjAdvDeductionTotal).ToString(hdfCurrencyFormat.Value);
                                        }
                                        SetHdrTax();
                                        POInvoiceHeaderSession.DeductionDetails = TempsoAdvDeductionDetailsList;


                                        //Settings of TaxPayableDiv
                                        if (hdfIsTaxPayable.Value.ToString() == "1")
                                        {
                                            SetTaxPayableDiv();
                                        }
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                                        //end
                                    }
                                    else
                                    {
                                        close = false;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "CalculateTotalSplit()", true);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divDeduction]','" + GetLocalResourceObject("DeductionDetails").ToString() + "','900','400');", true);
                                        litErrorMsg.Text = GetLocalResourceObject("Err_MsgExceeds_Allocation").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                }
                            }
                        }
                        if (close)
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #endregion

                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            //RadioButton rbtn;
                            //rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            //if (rbtn.Checked)
                            //{
                            HiddenField hdfDept;
                            int dept;
                            CheckBox chkPIselect;
                            chkPIselect = (CheckBox)grdrow.FindControl("chkPIselect");
                            if (chkPIselect.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                hdfInOpeningInv = (HiddenField)grdrow.FindControl("hdfInOpeningInv");
                                hdfIsConverted = (HiddenField)grdrow.FindControl("hdfIsConverted");
                                if (hdfInOpeningInv.Value == "1")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_Opening_cancel").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                                if (hdfIsConverted.Value == "1")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Converted_Inv_Cannot_Cancel").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }

                                Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                                Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).Text;
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
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
                        if (bIsChecked)
                        {
                            if (PIType == "21")
                                FillProcessID(31);
                            else
                                FillProcessID(11);
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
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.POINVHEADER);
                            SetFieldValues(ControlsEnum.POINVHEADER);
                            SetFieldValues(ControlsEnum.POINVDETAIL);
                            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
                            SetDetailTax(null);
                            SetHdrTax();
                            if (grdInvoice.Rows.Count > 0)
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

                    #region PRINT
                    case ActionsEnum.PRINT:
                        int Pk = 0;
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            hdfInOpeningInv = (HiddenField)grdrow.FindControl("hdfInOpeningInv");
                            if (hdfInOpeningInv.Value == "1")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_Opening_Print").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            //RadioButton rbtn;
                            //rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            //if (rbtn.Checked)
                            //{
                            CheckBox chkPIselect;
                            chkPIselect = (CheckBox)grdrow.FindControl("chkPIselect");
                            if (chkPIselect.Checked)
                            {
                                bIsChecked = true;
                                Pk = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + Pk.ToString() + "&APPTYPE=" + ApplicationType.PI + "&APPSUBTYPE=") + "');", true);
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }

                        break;
                    case ActionsEnum.PRINTDT:
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.PI + "&APPSUBTYPE=1") + "');", true);
                        }
                        break;
                    case ActionsEnum.PRINTPO:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfSoNo.Value + "&APPTYPE=" + ApplicationType.PO + "&APPSUBTYPE=") + "');", true);
                        break;
                    case ActionsEnum.PRINTPR:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfPRNo.Value + "&APPTYPE=" + ApplicationType.PR + "&APPSUBTYPE=") + "');", true);
                        break;

                    case ActionsEnum.PRINTLINEITEMPO:
                        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).NamingContainer;
                        HiddenField hdfLineItemPOPK = (HiddenField)gvrow.FindControl("hdfSODtlPK");
                        if (hdfLineItemPOPK != null && Convert.ToInt32(hdfLineItemPOPK.Value) > 0)
                        {
                            if (hdfIvhGrp.Value == "6")
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfLineItemPOPK.Value + "&APPTYPE=" + ApplicationType.SCWO + "&APPSUBTYPE=1") + "');", true);
                            else
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfLineItemPOPK.Value + "&APPTYPE=" + ApplicationType.PO + "&APPSUBTYPE=" + (POGroup == POInvoiceGroup.Services ? ((int)POSubType.Service).ToString() : "")) + "');", true);
                        }
                        break;
                    case ActionsEnum.PRINTDEDINVOICE:
                        GridViewRow gvAdvrow = (GridViewRow)((LinkButton)sender).NamingContainer;
                        HiddenField hdfAdvInvoiceLineItemPK = (HiddenField)gvAdvrow.FindControl("hdfAdvInvoicePK");
                        if (hdfAdvInvoiceLineItemPK != null && Convert.ToInt32(hdfAdvInvoiceLineItemPK.Value) > 0)
                        {
                            // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfAdvInvoiceLineItemPK.Value + "&APPTYPE=" + ApplicationType.PI + "&APPSUBTYPE=13") + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divDeduction]','" + GetLocalResourceObject("DeductionDetails").ToString() + "','800','400');", true);
                            //EditTaxOtherCharge Setting
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "EditTaxOtherCharge", "$(document).ready(function () { EnableDisableTaxOtherCharge();});", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "CalculateTotalSplit()", true);
                        }
                        break;

                    case ActionsEnum.PRINTLINEITEMPR:
                        GridViewRow gvPRrow = (GridViewRow)((LinkButton)sender).NamingContainer;
                        HiddenField hdfPRPKs = (HiddenField)gvPRrow.FindControl("hdfPRPKs");
                        if (hdfPRPKs != null && !hdfPRPKs.Value.Contains(",")) // Print shows only for single PR
                        {
                            if (Convert.ToInt32(hdfPRPKs.Value) > 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfPRPKs.Value + "&APPTYPE=" + ApplicationType.PR + "&APPSUBTYPE=") + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region CHANGETYPE
                    case ActionsEnum.CHANGETYPE:
                        SetBranchCodeVisibility();
                        break;
                    #endregion

                    #region CHANGEEXCHANGERATE
                    case ActionsEnum.CHANGEEXRATE:
                        hdfExchangeRate.Value = string.IsNullOrEmpty(txtExchangeRate.Text) ? "1" : txtExchangeRate.Text;
                        //Settings of TaxPayableDiv
                        if (hdfIsTaxPayable.Value.ToString() == "1")
                        {
                            SetTaxPayableDiv();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                        //end
                        break;
                    #endregion

                    #region AMOUNTDETAILS
                    case ActionsEnum.AMOUNTDETAILS:
                        HiddenField hdfInvoiceID = (HiddenField)((GridViewRow)((LinkButton)(sender)).Parent.Parent).FindControl("hdfInvoiceID");
                        long.TryParse(hdfInvoiceID.Value, out InvoicePk);
                        GetFieldValues(ControlsEnum.AMOUNTDETAILS);
                        SetFieldValues(ControlsEnum.AMOUNTDETAILS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalAmountSplit", "$(document).ready(function(){CalculateTotalAmountSplit();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPaidAmntSplitup]','" + GetLocalResourceObject("TrxDetails").ToString() + "','600','250');", true);
                        break;
                    #endregion

                    #region Other Charge Popup
                    case ActionsEnum.OTHERCHARGEHEADER:
                        SetFieldValues(ControlsEnum.OTHERCHARGELIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowotherchargePop", "ShowContainerDiv('[id$=divOtherchargeSplitUp]','" + GetLocalResourceObject("OtherchargeDetails").ToString() + "','800','400');", true);

                        break;
                    #endregion

                    #region OTHERCHARGEAPPLY
                    case ActionsEnum.OTHERCHARGEAPPLY:
                    case ActionsEnum.CONFIRMOTHERCHARGES:

                        if (POInvoiceHeaderSession != null)
                        {
                            if (POInvoiceHeaderSession.OtherChargeDetails != null && POInvoiceHeaderSession.OtherChargeDetails.Count > 0)
                            {
                                // To set total other charge details                                
                                decimal totalOtherCharge = 0;
                                List<POOtherChargeDetails> PoOthrChrgLst = new List<POOtherChargeDetails>();
                                hdfOtherCharge.Value = "1";
                                bool AddlOthrCharge = true;
                                double balance = 0;
                                double adjustment = 0;
                                foreach (GridViewRow gvr in grdOtherchargeSplit.Rows)
                                {
                                    if (gvr.RowType == DataControlRowType.DataRow)
                                    {
                                        TextBox txtAdjustNowAmount = gvr.FindControl("txtAdjustNowAmount") as TextBox;
                                        HiddenField hdfPOOtherchargePK = gvr.FindControl("hdfPOOtherchargePK") as HiddenField;
                                        HiddenField hdfPOHeadPK = gvr.FindControl("hdfPOPK") as HiddenField;
                                        Label lblBalanceSplit = gvr.FindControl("lblBalanceSplit") as Label;
                                        if (hdfEnableAddlOtherCharge.Value == "1" && hdfConfirmOtherCharges.Value == "0")
                                        {
                                            double.TryParse(txtAdjustNowAmount.Text, out adjustment);
                                            double.TryParse(lblBalanceSplit.Text, out balance);
                                            if (balance < adjustment)
                                            {
                                                AddlOthrCharge = false;
                                                break;
                                            }
                                        }
                                        if (txtAdjustNowAmount.Text != string.Empty)
                                        {
                                            totalOtherCharge += Convert.ToDecimal(txtAdjustNowAmount.Text);
                                        }

                                        poInvOtherchargeObj = POInvoiceHeaderSession.OtherChargeDetails.SingleOrDefault(rfq => rfq.IVM_PO_HDR == Convert.ToInt32(hdfPOHeadPK.Value));
                                        if (poInvOtherchargeObj != null)
                                        {
                                            poInvOtherchargeObj.IVM_OTHER_AMOUNT = Convert.ToDecimal(txtAdjustNowAmount.Text);
                                            poInvOtherchargeObj.IVM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                        }
                                        PoOthrChrgLst.Add(poInvOtherchargeObj);
                                    }
                                }
                                if (hdfEnableAddlOtherCharge.Value == "1" && AddlOthrCharge == false && hdfConfirmOtherCharges.Value == "0")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ConfirmAdditionalCharge", "$(document).ready(function(){ClosePopup();ConfirmAdditionalOtherCharges();});", true);
                                    return;
                                }
                                POInvoiceHeaderSession.OtherChargeDetails = PoOthrChrgLst;
                                txtShipping.Text = totalOtherCharge.ToString(hdfCurrencyFormat.Value);
                                SetHdrTax();
                                //Settings of TaxPayableDiv
                                if (hdfIsTaxPayable.Value.ToString() == "1")
                                {
                                    SetTaxPayableDiv();
                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                                //end
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #endregion

                    #region ADJUSTAMOUNT
                    case ActionsEnum.ADJUSTAMOUNT:
                        ValidateAdjAmount();
                        break;
                    #endregion

                    #region POLIST (Add New PO)
                    case ActionsEnum.POLIST:
                        GetFieldValues(ControlsEnum.POLIST);
                        SetFieldValues(ControlsEnum.POLIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divNewPOList]','" + GetLocalResourceObject("POList").ToString() + "','900','300');", true);
                        break;
                    #endregion

                    #region NEW PO APPLY
                    case ActionsEnum.NEWPOAPPLY:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            bIsChecked = false;
                            POHeaderBO ObjPOHeaderBO = new POHeaderBO();
                            POHeaderListBO ObjPOHeaderListBO;
                            List<POHeaderListBO> POHeaderListBOLst = new List<POHeaderListBO>();
                            foreach (GridViewRow grvRow in grdNewPOList.Rows)
                            {
                                CheckBox chkPOselect = (CheckBox)grvRow.FindControl("chkPOselect");
                                if (chkPOselect.Checked)
                                {
                                    bIsChecked = true;
                                    HiddenField hdfPOID = (HiddenField)grvRow.FindControl("hdfPOID");
                                    int POId = string.IsNullOrEmpty(hdfPOID.Value) ? 0 : Convert.ToInt32(hdfPOID.Value);

                                    if (Convert.ToInt32(hdfShowInvestor.Value) == 1)
                                    {
                                        HiddenField hdfInvestorNew = (HiddenField)grvRow.FindControl("hdfInvestorNew");
                                        string NewInvestor = string.IsNullOrEmpty(hdfInvestorNew.Value) ? null : (hdfInvestorNew.Value);
                                        if (txtInvestor.Text.Trim() != NewInvestor)
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Investor").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            break;
                                        }
                                    }

                                    if (POId > 0)
                                    {
                                        ObjPOHeaderListBO = new POHeaderListBO();
                                        ObjPOHeaderListBO.POPK = POId;
                                        POHeaderListBOLst.Add(ObjPOHeaderListBO);
                                    }
                                }
                            }
                            if (bIsChecked)
                            {
                                ObjPOHeaderBO.POList = POHeaderListBOLst;
                                InvoiceMultiplePOPKsNew = ObjPOHeaderBO;
                                GetFieldValues(ControlsEnum.POINVHEADERNEW);
                                SetFieldValues(ControlsEnum.POINVDETAIL);
                                // SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
                                hdfApplyTax.Value = "0";
                                //If Has RefID (from Inbox)
                                if (!string.IsNullOrEmpty(refID))
                                {
                                    hdfApplyTax.Value = "1"; // From Inbox no need to create formula for Tax amount. 
                                }
                                SetDetailTax(null);
                                SetHdrTax();
                                if (grdInvoice.Rows.Count > 0)
                                {
                                    SetSubTotal();
                                }
                                if (hdfIsTaxPayable.Value.ToString() == "1")
                                {
                                    SetTaxPayableDiv();
                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                            }
                            else
                            {
                                //litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divNewPOList]','" + GetLocalResourceObject("POList").ToString() + "','900','300');", true);
                            }
                        }
                        break;
                    #endregion

                    #region Show Popup
                    case ActionsEnum.SHOWPOPUP:
                        poPK = ((LinkButton)sender).CommandArgument;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + poPK + "&APPTYPE=" + ApplicationType.PO + "&APPSUBTYPE=") + "');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divNewPOList]','" + GetLocalResourceObject("POList").ToString() + "','900','300');", true);
                        break;
                    #endregion

                    #region GRN SPLITUP POPUP
                    case ActionsEnum.GRNDETAILS:
                        hdfInvoiceDtlPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfInvoiceDtlPK") as HiddenField);
                        hdfItemPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                        hdfPOPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfSODtlPK") as HiddenField);
                        POInvoicePK = string.IsNullOrEmpty(hdfInvoiceDtlPK.Value) ? 0 : Convert.ToInt32(hdfInvoiceDtlPK.Value);
                        SelectedItemPK = string.IsNullOrEmpty(hdfItemPK.Value) ? 0 : Convert.ToInt32(hdfItemPK.Value);
                        SelectedPOPK = string.IsNullOrEmpty(hdfPOPK.Value) ? 0 : Convert.ToInt32(hdfPOPK.Value);
                        hdfGRNExceed.Value = "0";
                        SetFieldValues(ControlsEnum.GRNQTYSPLIT);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalGRN", "$(document).ready(function(){CalculateTotalGRN();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divGRNQtySpilup]','" + GetLocalResourceObject("GRNAllocationDetails").ToString() + "','900','500');", true);

                        break;
                    #endregion

                    #region APPLY GRN SPLITUP POPUP DETAILS
                    case ActionsEnum.GRNAPPLY:
                        //Checking for Inv. Now Qty entered exceeds grn Qty
                        bool grnContinue = true;
                        //if (hdfGRNExceed.Value == "0")
                        //{
                        foreach (GridViewRow grdrowitem in grdGRNDetails.Rows)
                        {
                            TextBox txtInvNowGRNQty = (TextBox)grdrowitem.FindControl("txtInvNowGRNQty");
                            Label lblGRNBalance = (Label)grdrowitem.FindControl("lblGRNBalance");
                            double balQty = Convert.ToDouble(lblGRNBalance.Text.Replace(",", ""));
                            if (Convert.ToDouble(txtInvNowGRNQty.Text) > balQty)
                            {
                                grnContinue = false;
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowGRNNowQtyExceeds", "$(document).ready(function(){ShowGRNNowQtyExceeds();});", true);
                                litErrorMsg.Text = GetLocalResourceObject("GRNQtyExceed").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                break;
                            }
                        }
                        //}
                        if (grnContinue)
                        {
                            if (POInvoiceHeaderSession != null)
                            {
                                invoiceHeaderObj = POInvoiceHeaderSession;
                                grnList = new List<GRNQTYDetails>();
                                soDtlObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.VID_PO) == SelectedPOPK);
                                if (soDtlObj != null)
                                {
                                    grnList = soDtlObj.GRNDtl.ToList();
                                }
                                if (grnList != null && grnList.Count > 0)
                                {
                                    // To set total other charge details                                
                                    double totalGrnInvNow = 0;
                                    foreach (GridViewRow gvr in grdGRNDetails.Rows)
                                    {
                                        if (gvr.RowType == DataControlRowType.DataRow)
                                        {
                                            TextBox txtInvNowGRNQty = gvr.FindControl("txtInvNowGRNQty") as TextBox;
                                            HiddenField hdfVGLPOPK = gvr.FindControl("hdfVGLPOPK") as HiddenField;
                                            HiddenField hdfPOGRNPK = gvr.FindControl("hdfPOGRNPK") as HiddenField;
                                            if (txtInvNowGRNQty.Text != string.Empty)
                                            {
                                                totalGrnInvNow += Convert.ToDouble(txtInvNowGRNQty.Text);
                                            }

                                            poGrnQtyObj = grnList.SingleOrDefault(rfq => rfq.VGL_GRN_DTL == Convert.ToInt32(hdfPOGRNPK.Value));
                                            if (poGrnQtyObj != null)
                                            {
                                                poGrnQtyObj.VGL_QTY_INVOICED = Convert.ToDouble(txtInvNowGRNQty.Text);
                                                poGrnQtyObj.VGL_INVOICE_DTL = soDtlObj.VID_PK;
                                                poGrnQtyObj.VGL_UOM = soDtlObj.VID_UOM;
                                                poGrnQtyObj.VGL_SL_NO = soDtlObj.VID_SL_NO;
                                            }
                                        }
                                    }
                                    soDtlObj.VID_QTY_INVOICED = totalGrnInvNow;

                                    TempPOInvoiceHeaderSession = invoiceHeaderObj;
                                    SetFieldValues(ControlsEnum.POINVDETAIL);
                                    POInvoicePK = 0;
                                    SelectedItemPK = 0;
                                    SetDetailTax(null);
                                    SetHdrTax();
                                    ResetForm(ControlsEnum.TAXPOPUPGRID);
                                    SetUIValuesToObject(ControlsEnum.POINVDETAIL);

                                    //invoiceHeaderObj.OrderDetail.ForEach(dtl => { 
                                    //    dtl.GRNDtl.ForEach(Row => {
                                    //        POInvoiceUploads objDtl = POUploadList.SingleOrDefault(r => r.DOC_TASK_ID2 == Row.VGL_GRN_HDR.ToString() && r.DOC_IS_GRN == 1);
                                    //        if (objDtl != null)
                                    //        {
                                    //            if (Row.VGL_QTY_INVOICED == 0)
                                    //                objDtl.DOC_STATUS = 0;
                                    //            else
                                    //                objDtl.DOC_STATUS = 1;
                                    //        }
                                    //    });
                                    //});

                                    invoiceHeaderObj.OrderDetail.ForEach(dtl =>
                                    {
                                        dtl.GRNDtl.ForEach(Row =>
                                        {
                                            if (Row.VGL_QTY_INVOICED == 0)
                                            {
                                                POUploadList.Where(r => r.DOC_TASK_ID2 == Row.VGL_GRN_HDR.ToString()).ToList().ForEach(a =>
                                                {
                                                    a.DOC_STATUS = 0;
                                                });
                                            }
                                            else
                                            {
                                                POUploadList.Where(r => r.DOC_TASK_ID2 == Row.VGL_GRN_HDR.ToString()).ToList().ForEach(a =>
                                                {
                                                    a.DOC_STATUS = 1;
                                                });
                                            }
                                        });
                                    });

                                    SetFieldValues(ControlsEnum.GRNATTACHMENTS);
                                    //var grnpks = dtl.GRNDtl.Where(r => r.VGL_QTY_INVOICED == 0).Select(f => f.VGL_GRN_HDR.ToString()).ToList();
                                    //if (grnpks != null && grnpks.Count > 0)
                                    //    invoiceHeaderObj.FileList.RemoveAll(r => grnpks.Contains(r.DOC_TASK_ID2.ToString()) && r.DOC_IS_GRN == 1);

                                    //POUploadList.ForEach(dtl => 
                                    //{

                                    //};

                                    //Settings of TaxPayableDiv
                                    if (hdfIsTaxPayable.Value.ToString() == "1")
                                    {
                                        SetTaxPayableDiv();
                                    }
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                                }
                            }
                            SetEffectiveRate();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        ValidateAdjAmount();
                        break;
                    #endregion

                    #region Show GRN Print
                    case ActionsEnum.SHOW:
                        grnPK = ((LinkButton)sender).CommandArgument;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + grnPK + "&APPTYPE=" + ApplicationType.GRN + "&APPSUBTYPE=") + "');", true);
                        SetFieldValues(ControlsEnum.GRNQTYSPLIT);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalGRN", "$(document).ready(function(){CalculateTotalGRN();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divGRNQtySpilup]','" + GetLocalResourceObject("GRNAllocationDetails").ToString() + "','900','500');", true);
                        break;
                    #endregion
                    #region Show RMI Print
                    case ActionsEnum.SHOWRMI:

                        LinkButton lnkTIHNo = (LinkButton)sender;

                        string[] commandArgs = lnkTIHNo.CommandArgument.ToString().Split(new char[] { ',' });
                        string RMIPK = commandArgs[0];
                        string RawMaterialPageType = commandArgs[1];//1->Raw Material Inspection,2->Raw Material Inspection(PM)
                        //string RMIPK = ((LinkButton)sender).CommandArgument;
                        //RMI
                        if (RawMaterialPageType == "1")
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + RMIPK + "&APPTYPE=" + ApplicationType.RMI + "&APPSUBTYPE=") + "');", true);
                        else if (RawMaterialPageType == "2")//RMIPM
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                  RMIPK + "&APPTYPE=" + ApplicationType.RMIPM) + "');", true);

                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divGRNQtySpilup]','" + GetLocalResourceObject("GRNAllocationDetails").ToString() + "','900','500');", true);
                        break;
                    #endregion

                    #region SETTAXAPPLICABLEAMOUNT
                    case ActionsEnum.SETTAXAPPLICABLEAMOUNT:
                        double applicableamount = 0;
                        applicableamount = SetTaxApplicableAmount();
                        txtPopupItemAmount.Text = (applicableamount).ToString(hdfCurrencyFormat.Value);
                        // GetFormula(parseInt($("[id$=ChooseTax]").val()));
                        ActionHandler(ddlPopupTaxType, EventArgs.Empty);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','600','300');", true);
                        break;
                    #endregion

                    #region CostCenter PopUp
                    case ActionsEnum.COSTCENTER:
                        POdtlPK = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                        poCostCenterlst = new List<POCostCenterDetails>();
                        poCostCenterlst = POInvoiceHeaderSession.OrderDetail.SingleOrDefault(x => x.VID_PO_DTL == POdtlPK.ToString()).CostCenterDtl;
                        double rate = 0;
                        double OrderQty = 0;
                        double qty = 0;
                        double TotalAmt = 0;
                        double gridQtySum = 0;
                        double gridAmtSum = 0;
                        foreach (GridViewRow grdrow in grdInvoice.Rows)
                        {
                            if (((HiddenField)grdrow.FindControl("hdfPOdtl")).Value == POdtlPK.ToString())
                            {
                                rate = ((TextBox)grdrow.FindControl("txtRate")).Text == string.Empty ? 0 : Convert.ToDouble(((TextBox)grdrow.FindControl("txtRate")).Text);
                                qty = ((TextBox)grdrow.FindControl("txtInvNow")).Text == string.Empty ? 0 : Convert.ToDouble(((TextBox)grdrow.FindControl("txtInvNow")).Text);
                                OrderQty = ((Label)grdrow.FindControl("lblOrderQuantity")).Text == string.Empty ? 0 : Convert.ToDouble(((Label)grdrow.FindControl("lblOrderQuantity")).Text);
                                TotalAmt = ((TextBox)grdrow.FindControl("txtAmount")).Text == string.Empty ? 0 : Convert.ToDouble(((TextBox)grdrow.FindControl("txtAmount")).Text);
                                break;
                            }
                        }
                        if (poCostCenterlst != null && poCostCenterlst.Count > 0)
                        {
                            foreach (var items in poCostCenterlst)
                            {

                                if (qty > 0)
                                {
                                    items.IVC_PERCENTAGE = (items.IVC_POR_QTY_ORDERED / OrderQty) * 100;
                                    items.IVC_QUANTITY = Convert.ToDouble(GetFormattedNumber((qty * (items.IVC_POR_QTY_ORDERED / OrderQty) * 100) / 100));
                                }
                                else
                                {
                                    items.IVC_PERCENTAGE = 0;
                                    items.IVC_QUANTITY = 0;
                                }
                                items.IVC_AMOUNT = Convert.ToDouble(GetFormattedNumber(items.IVC_QUANTITY * rate));
                            }

                            SetFieldValues(ControlsEnum.COSTCENTERDTL);
                            foreach (GridViewRow grdRow in grdCostCenterlist.Rows)
                            {
                                Label lblQty = ((Label)grdRow.FindControl("lblQty"));
                                Label lblAmt = ((Label)grdRow.FindControl("lblAmt"));

                                double cQty = lblQty.Text == string.Empty ? 0 : Convert.ToDouble(lblQty.Text);
                                double cAmt = lblAmt.Text == string.Empty ? 0 : Convert.ToDouble(lblAmt.Text);
                                gridQtySum = gridQtySum + cQty;
                                gridAmtSum = gridAmtSum + cAmt;
                            }
                            ((Label)grdCostCenterlist.FooterRow.FindControl("lblCCQtyTotal")).Text = GetFormattedNumberWithSeperation(qty);
                            ((Label)grdCostCenterlist.FooterRow.FindControl("lblCCAmtTotal")).Text = GetFormattedCurrencyWithSeperation(TotalAmt);

                            double QtyDiff = gridQtySum - qty;
                            double AmtDiff = gridAmtSum - TotalAmt;
                            if (QtyDiff != 0)
                                ((Label)grdCostCenterlist.Rows[grdCostCenterlist.Rows.Count - 1].FindControl("lblQty")).Text = GetFormattedNumberWithSeperation(Convert.ToDouble(((Label)grdCostCenterlist.Rows[grdCostCenterlist.Rows.Count - 1].FindControl("lblQty")).Text) - QtyDiff);
                            if (AmtDiff != 0)
                                ((Label)grdCostCenterlist.Rows[grdCostCenterlist.Rows.Count - 1].FindControl("lblAmt")).Text = GetFormattedCurrencyWithSeperation(Convert.ToDouble(((Label)grdCostCenterlist.Rows[grdCostCenterlist.Rows.Count - 1].FindControl("lblAmt")).Text) - AmtDiff);
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCostCenterDtl]','" + GetLocalResourceObject("POList").ToString() + "','500','300');", true);
                        break;
                    #endregion

                    #region CONVERTALL
                    case ActionsEnum.CONVERTALL:
                        InvoiceConvert invConvert = new InvoiceConvert();
                        InvoiceDetails invDetails;
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            CheckBox chkPIselect;
                            chkPIselect = (CheckBox)grdrow.FindControl("chkPIselect");
                            if (chkPIselect.Checked)
                            {
                                int MenuType = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfMenuType")).Value == "" ? "0" : ((HiddenField)grdrow.FindControl("hdfMenuType")).Value);
                                int IsConverted = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfIsConverted")).Value);
                                if (Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value) != 2) //2 = approved
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("SelectApprovedInvoice").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    return;
                                }
                                if (MenuType != 3)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("NotAllowedToConvert").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    return;
                                }
                                else if (IsConverted == 1)//1=converted
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("AlreadyConverted").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    return;
                                }
                                else
                                {
                                    invDetails = new InvoiceDetails();
                                    invDetails.InvPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                    if (invConvert.InvoicePKList == null)
                                        invConvert.InvoicePKList = new List<InvoiceDetails>();
                                    invConvert.InvoicePKList.Add(invDetails);
                                }
                            }
                        }
                        if (invConvert.InvoicePKList == null)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_SelectItem").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            return;
                        }
                        string invXmlDoc = CommonFunctions.XmlSerialize<InvoiceConvert>(invConvert);
                        int SBUCovco = Convert.ToInt32(GetLocalResourceObject("SBUForPOConvert").ToString());
                        Dictionary<string, object> res = BusinessLogic.POInvoicing.POInvoiceBL.SaveInvoiceConvert(invXmlDoc, SBUCovco, currentUser.SBUID);
                        result = Convert.ToInt32(res["result"]);
                        if (result.HasValue && result.Value > 0) // Success
                        {
                            litErrorMsg.Text = String.Format(GetLocalResourceObject("ConvertedSuccessfully").ToString(), res["PONo"].ToString(), res["SCNo"].ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);

                            EntryStatus = EntryStatus.LISTMODE;
                            ResetForm(ControlsEnum.INVOICELIST);
                            GetFieldValues(ControlsEnum.INVOICELIST);
                            SetFieldValues(ControlsEnum.INVOICELIST);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.SQLERROR || result == (int)DbSaveStatus.DATEOVERLAP)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }

                            if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("AlreadyConverted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }

                            if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("SelectSameSCInvoice").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region CHECK_CHANGE
                    case ActionsEnum.CHECK_CHANGE:
                        foreach (GridViewRow grdrowitem in grdInvoice.Rows)
                        {
                            CheckBox chkIsAsset = (CheckBox)grdrowitem.FindControl("chkIsAsset");
                            DropDownList ddlAssetType = (DropDownList)grdrowitem.FindControl("ddlAssetType");
                            if (chkIsAsset.Checked)
                                ddlAssetType.Enabled = true;
                            else
                            {
                                ddlAssetType.Enabled = false;
                                ddlAssetType.SelectedValue = CommonConstants.SELECTVAL;
                            }
                        }
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
                poInvoiceServiceClient = null;
                CommonServiceClient = null;

            }
        }


        #region Workflow Submit
        /// <summary>
        /// Save and submit With workflow 
        /// </summary>
        /// <param name="objSalPayment"></param>
        private void SaveTransaction(POInvoiceHeader objPOInvoice, int workflowFlag)
        {
            int? result = 0;
            string savePath = string.Empty;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (objPOInvoice == null)
                objPOInvoice = new POInvoiceHeader();
            #region Transaction Log and Application Code

            objPOInvoice.ATL_APP_TYPE = ApplicationType.PI;
            objPOInvoice.APT_CODE = POGroup == POInvoiceGroup.Goods ? ApplicationType.PI
                               : POGroup == POInvoiceGroup.Services ? ApplicationType.PSI : ApplicationType.EI;
            #endregion
            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            objPOInvoice.USER_PK = Convert.ToInt16(wkfDetails.UserPK);
            objPOInvoice.WKF_APPLICATION = CurrPK;
            objPOInvoice.WKF_COMMENTS = wkfDetails.Comments;
            objPOInvoice.WKF_TRX_FLAG = workflowFlag;
            objPOInvoice.WKF_PROCESS = wkfDetails.ProcessID;
            objPOInvoice.WKF_REFERENCE = wkfDetails.ReferenceID;
            objPOInvoice.WKF_TASK = wkfDetails.TaskID;
            objPOInvoice.WKF_TASK_ACTION = wkfDetails.ActionID;
            action = wkfDetails.ActionText;
            if (isCancelled)
                objPOInvoice.ATL_ACTION = (byte)LogAction.CANCEL;
            else
                objPOInvoice.ATL_ACTION = (byte)LogAction.SUBMIT;
            #endregion
            string xmlDoc = CommonFunctions.XmlSerialize<POInvoiceHeader>(objPOInvoice);//CommonFunctions.ObjectTOXml(rfqResponseHeaderObj);
            string invoiceNumber = string.Empty;
            result = BusinessLogic.POInvoicing.POInvoiceBL.SavePOInvoiceWkf(xmlDoc, out invoiceNumber);
            if (result.HasValue && result.Value > 0) // Success !  redirect to listing page
            {
                #region File Upload
                if (workflowFlag == (int)(WorkflowTransactionFlag.SAVEANDSUBMIT))
                {
                    #region File Uploads
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
                    #endregion
                }
                #endregion
                if (result.HasValue && result.Value > 0)
                {
                    ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();
                    if (isCancelled)
                    {
                        if (PIType == "21")
                            FillProcessID(21);
                        else
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
                    if (string.IsNullOrEmpty(invoiceNumber))
                        invoiceNumber = lblInvoiceNo.Text.Trim();
                    object[] args = new object[2];
                    args[0] = Resources.PageNameRes.POInvoice;
                    args[1] = invoiceNumber;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                    // Show Save Message and redired to listing page                                        
                    //litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.POInvoice);
                    #region Inbox or Listing Page Redirection
                    if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                    {
                        ResetForm(ControlsEnum.INVOICELIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                    }
                    else
                    {
                        ResetForm(ControlsEnum.INVOICELIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
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
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.REFNOEXIST)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "VendorInvoiceDup", "$(document).ready(function(){ClosePopup();ShowDuplicateVendorInvNoContinue(2);});", true);
                }
                else if (result == (int)DbSaveStatus.CONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.POInvoice + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.POInvoice + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == -51)// GRN qty exeed
                {
                    litErrorMsg.Text = GetLocalResourceObject("GRNQtyExceed").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "','" + "');", true);
                }
                else if (result == -52)// Purchase qty exeed
                {
                    litErrorMsg.Text = GetLocalResourceObject("PURQtyExceed").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "','" + "');", true);
                }

                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.POInvoice);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup(); ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                return;
            }

        }
        #endregion

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

        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //if (((GridView)sender).ID == "grdInvoiceList")
                //{
                //    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                //    {
                //        Button imgPosted = e.Row.FindControl("imgPosted") as Button;
                //        HiddenField hdfPosted = e.Row.FindControl("hdfPosted") as HiddenField;
                //        if (dtInvoiceList != null)
                //        {
                //            if (dtInvoiceList.Rows[e.Row.RowIndex]["FTH_CSS_CLASS"].ToString() != "" || dtInvoiceList.Rows[e.Row.RowIndex]["FTH_CSS_CLASS"].ToString() != string.Empty)
                //            {
                //                imgPosted.CssClass = dtInvoiceList.Rows[e.Row.RowIndex]["FTH_CSS_CLASS"].ToString();
                //                imgPosted.ToolTip = dtInvoiceList.Rows[e.Row.RowIndex]["FTH_STATUS_TEXT"].ToString();
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
                if (((GridView)sender).ID == "grdDeduction")
                {

                    //Text='<%#GetFormattedCurrency(Convert.ToDecimal(Eval("VAD_AMOUNT")) > 0 ? Eval("VAD_AMOUNT") : (Convert.ToDecimal(Eval("IVH_AMOUNT_TC").ToString())-Convert.ToDecimal(Eval("IVH_AMOUNT_ALLOCATED").ToString()))) %>'>
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        TextBox txtDedAllocateNowSplit = e.Row.FindControl("txtDedAllocateNowSplit") as TextBox;
                        TextBox txtOtherAmountSplit = e.Row.FindControl("txtOtherAmountSplit") as TextBox;
                        TextBox txtTaxSplit = e.Row.FindControl("txtTaxSplit") as TextBox;

                        HiddenField hdfDedAllocateNowSplit = e.Row.FindControl("hdfDedAllocateNowSplit") as HiddenField;
                        HiddenField hdfCurPaidOtherAmount = e.Row.FindControl("hdfCurPaidOtherAmount") as HiddenField;
                        HiddenField hdfDedOtherChargeSplitBalance = e.Row.FindControl("hdfDedOtherChargeSplitBalance") as HiddenField;

                        HiddenField hdfCurPaidTax = e.Row.FindControl("hdfCurPaidTax") as HiddenField;
                        HiddenField hdfDedTaxSplitBalance = e.Row.FindControl("hdfDedTaxSplitBalance") as HiddenField;
                        HiddenField hdfCurPaidDisc = e.Row.FindControl("hdfCurPaidDisc") as HiddenField;
                        if (deductionDtlList != null && deductionDtlList.Count > 0)
                        {
                            //if (deductionDtlList[e.Row.RowIndex].VAD_PK > 0)
                            //{
                            //    txtDedAllocateNowSplit.Text = deductionDtlList[e.Row.RowIndex].VAD_AMOUNT > 0
                            //        ? GetFormattedCurrency(deductionDtlList[e.Row.RowIndex].VAD_AMOUNT).ToString()
                            //        : GetFormattedCurrency(Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].PVH_PO_PAID_AMT) - deductionDtlList[e.Row.RowIndex].IVH_AMOUNT_ALLOCATED).ToString();
                            //}
                            //else
                            //{
                            //    if (deductionDtlList.Where(aa => aa.VAD_PK > 0).Count() > 0)
                            //    {
                            //        txtDedAllocateNowSplit.Text = GetFormattedCurrency(0).ToString();
                            //    }
                            //    else
                            //    {
                            //        txtDedAllocateNowSplit.Text = deductionDtlList[e.Row.RowIndex].VAD_AMOUNT > 0
                            //        ? GetFormattedCurrency(deductionDtlList[e.Row.RowIndex].VAD_AMOUNT).ToString()
                            //        : GetFormattedCurrency(Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].PVH_PO_PAID_AMT) - deductionDtlList[e.Row.RowIndex].IVH_AMOUNT_ALLOCATED).ToString();
                            //    }
                            //}

                            if (string.IsNullOrEmpty(CurrPK.ToString()) || Convert.ToInt32(CurrPK) == 0)
                            {
                                txtDedAllocateNowSplit.Text = GetFormattedCurrency(Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].VAD_AMOUNT) > 0 ? deductionDtlList[e.Row.RowIndex].VAD_AMOUNT : (Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].PVH_PO_PAID_AMT) - Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].IVH_AMOUNT_ALLOCATED)));
                                hdfDedAllocateNowSplit.Value = GetFormattedCurrency(Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].VAD_AMOUNT) > 0 ? deductionDtlList[e.Row.RowIndex].VAD_AMOUNT : (Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].PVH_PO_PAID_AMT) - Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].IVH_AMOUNT_ALLOCATED)));

                                hdfAllocNowAmount.Value = GetFormattedCurrency(Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].PVH_PO_PAID_AMT) - Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].IVH_AMOUNT_ALLOCATED));

                                txtOtherAmountSplit.Text = GetFormattedCurrency(Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].VAD_OTHER_AMOUNT) > 0 ? deductionDtlList[e.Row.RowIndex].VAD_OTHER_AMOUNT : (Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].PVH_PO_PAID_OTHER_AMT) - Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].IVH_OTHER_AMT_ALLOCATED)));
                                hdfCurPaidOtherAmount.Value = GetFormattedCurrency(Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].VAD_OTHER_AMOUNT) > 0 ? deductionDtlList[e.Row.RowIndex].VAD_OTHER_AMOUNT : (Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].PVH_PO_PAID_OTHER_AMT) - Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].IVH_OTHER_AMT_ALLOCATED)));
                                //hdfDedOtherChargeSplitBalance.Value = GetFormattedCurrency(Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].VAD_OTHER_AMOUNT) > 0 ? deductionDtlList[e.Row.RowIndex].VAD_OTHER_AMOUNT : (Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].PVH_PO_PAID_OTHER_AMT) - Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].IVH_OTHER_AMT_ALLOCATED)));

                                txtTaxSplit.Text = GetFormattedCurrency(Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].VAD_TAX_AMOUNT) > 0 ? deductionDtlList[e.Row.RowIndex].VAD_TAX_AMOUNT : (Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].PVH_PO_PAID_TAX_AMT) - Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].IVH_TAX_AMT_ALLOCATED)));
                                hdfCurPaidTax.Value = GetFormattedCurrency(Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].VAD_TAX_AMOUNT) > 0 ? deductionDtlList[e.Row.RowIndex].VAD_TAX_AMOUNT : (Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].PVH_PO_PAID_TAX_AMT) - Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].IVH_TAX_AMT_ALLOCATED)));
                                //hdfDedTaxSplitBalance.Value = GetFormattedCurrency(Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].VAD_TAX_AMOUNT) > 0 ? deductionDtlList[e.Row.RowIndex].VAD_TAX_AMOUNT : (Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].PVH_PO_PAID_TAX_AMT) - Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].IVH_TAX_AMT_ALLOCATED)));

                                //hdfCurPaidDisc        Set Later this
                                hdfCurPaidDisc.Value = GetFormattedCurrency(Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].VAD_DISC_AMOUNT) > 0 ? deductionDtlList[e.Row.RowIndex].VAD_DISC_AMOUNT : Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].IVM_DISCOUNT_AMOUNT));


                                DedTotalAllocateNowFooterSplit = DedTotalAllocateNowFooterSplit + Convert.ToDecimal(txtDedAllocateNowSplit.Text);
                                OtherAmountFooterSplit = OtherAmountFooterSplit + Convert.ToDecimal(txtOtherAmountSplit.Text);
                                TaxFooterSplit = TaxFooterSplit + Convert.ToDecimal(txtTaxSplit.Text);

                            }
                            else
                            {
                                txtDedAllocateNowSplit.Text = GetFormattedCurrency(deductionDtlList[e.Row.RowIndex].VAD_AMOUNT.ToString());
                                hdfDedAllocateNowSplit.Value = GetFormattedCurrency(deductionDtlList[e.Row.RowIndex].VAD_AMOUNT.ToString());

                                hdfAllocNowAmount.Value = GetFormattedCurrency(Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].PVH_PO_PAID_AMT) - Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].IVH_AMOUNT_ALLOCATED));

                                txtOtherAmountSplit.Text = GetFormattedCurrency(deductionDtlList[e.Row.RowIndex].VAD_OTHER_AMOUNT.ToString());
                                hdfCurPaidOtherAmount.Value = GetFormattedCurrency(deductionDtlList[e.Row.RowIndex].VAD_OTHER_AMOUNT.ToString());
                                //hdfDedOtherChargeSplitBalance.Value = GetFormattedCurrency(deductionDtlList[e.Row.RowIndex].VAD_OTHER_AMOUNT.ToString());

                                txtTaxSplit.Text = GetFormattedCurrency(deductionDtlList[e.Row.RowIndex].VAD_TAX_AMOUNT.ToString());
                                hdfCurPaidTax.Value = GetFormattedCurrency(deductionDtlList[e.Row.RowIndex].VAD_TAX_AMOUNT.ToString());
                                hdfCurPaidDisc.Value = GetFormattedCurrency(deductionDtlList[e.Row.RowIndex].VAD_DISC_AMOUNT.ToString());

                                // hdfDedTaxSplitBalance.Value = GetFormattedCurrency(deductionDtlList[e.Row.RowIndex].VAD_TAX_AMOUNT.ToString());

                                //
                                DedTotalAllocateNowFooterSplit = DedTotalAllocateNowFooterSplit + Convert.ToDecimal(txtDedAllocateNowSplit.Text);
                                OtherAmountFooterSplit = OtherAmountFooterSplit + Convert.ToDecimal(txtOtherAmountSplit.Text);
                                TaxFooterSplit = TaxFooterSplit + Convert.ToDecimal(txtTaxSplit.Text);
                            }
                        }
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        Label lblDedTotalAllocateNowFooterSplit = (Label)e.Row.FindControl("lblDedTotalAllocateNowFooterSplit");
                        Label lblOtherAmountFooterSplit = (Label)e.Row.FindControl("lblOtherAmountFooterSplit");
                        Label lblTaxFooterSplit = (Label)e.Row.FindControl("lblTaxFooterSplit");
                        HiddenField hdfDedTotalAllocateNowFooterSplit = (HiddenField)e.Row.FindControl("hdfDedTotalAllocateNowFooterSplit");
                        HiddenField hdfOtherTotalFooterSplit = (HiddenField)e.Row.FindControl("hdfOtherTotalFooterSplit");
                        HiddenField hdfTaxTotalFooterSplit = (HiddenField)e.Row.FindControl("hdfTaxTotalFooterSplit");

                        hdfDedTotalAllocateNowFooterSplit.Value = lblDedTotalAllocateNowFooterSplit.Text = string.Format("{0:c}", DedTotalAllocateNowFooterSplit);
                        hdfOtherTotalFooterSplit.Value = lblOtherAmountFooterSplit.Text = string.Format("{0:c}", OtherAmountFooterSplit);
                        hdfTaxTotalFooterSplit.Value = lblTaxFooterSplit.Text = string.Format("{0:c}", TaxFooterSplit);
                    }

                    if (!IsAdvInvHasTax)
                    {
                        //    grdDeduction.Columns[10].Visible = false;
                        //    grdDeduction.Columns[11].Visible = false;
                        grdDeduction.Columns[11].Visible = false;
                        grdDeduction.Columns[12].Visible = false;

                    }
                }

                else if (((GridView)sender).ID == "grdInvoice")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        ImageButton imgDiscount = e.Row.FindControl("imgDiscount") as ImageButton;
                        ImageButton imgTax = e.Row.FindControl("imgTax") as ImageButton;

                        System.Web.UI.HtmlControls.HtmlGenericControl divDiscount = e.Row.FindControl("divDiscount") as System.Web.UI.HtmlControls.HtmlGenericControl;
                        System.Web.UI.HtmlControls.HtmlGenericControl divTax = e.Row.FindControl("divTax") as System.Web.UI.HtmlControls.HtmlGenericControl;

                        //if (hdfisTaxAdd.Value == "1")
                        if (Convert.ToInt16(hdfisTaxAdd.Value) == (int)TaxSettingEnum.ITEMWISE || Convert.ToInt16(hdfisTaxAdd.Value) == (int)TaxSettingEnum.BOTHHEADERITEM)
                        {
                            imgTax.Visible = true;
                            if (divTax != null)
                                divTax.Attributes.Add("class", "w100");
                        }
                        else
                        {
                            imgTax.Visible = false;
                        }
                        if (Convert.ToInt16(hdfisDiscountAdd.Value) == (int)TaxSettingEnum.ITEMWISE || Convert.ToInt16(hdfisTaxAdd.Value) == (int)TaxSettingEnum.BOTHHEADERITEM)
                        {
                            imgDiscount.Visible = true;
                            if (divDiscount != null)
                                divDiscount.Attributes.Add("class", "w100");
                        }
                        else
                        {
                            imgDiscount.Visible = false;
                        }
                        LinkButton lnkPRNo = (LinkButton)e.Row.FindControl("lnkPRNo");
                        HiddenField hdfPRPKs = (HiddenField)e.Row.FindControl("hdfPRPKs");
                        if (hdfPRPKs != null) // Print shows only for single PR
                        {
                            if (hdfPRPKs.Value.Contains(","))
                            {
                                lnkPRNo.Attributes.Add("onclick", "return false;");
                                lnkPRNo.Attributes.Add("class", "removelinkClass");
                            }
                            else
                            {
                                lnkPRNo.Attributes.Add("onclick", "return true;");
                                lnkPRNo.Attributes.Add("class", "text-underline");
                            }
                        }

                        LinkButton lnkItem = e.Row.FindControl("lnkItem") as LinkButton;
                        if (EnableCostCenter == 1)
                        {
                            lnkItem.Attributes.Add("onclick", "return true;");
                            lnkItem.Attributes.Add("class", "text-underline");
                        }
                        else
                        {
                            lnkItem.Attributes.Add("onclick", "return false;");
                            lnkItem.Attributes.Add("class", "removelinkClass");
                        }

                        CheckBox chkIsAsset = e.Row.FindControl("chkIsAsset") as CheckBox;
                        DropDownList ddlAssetType = e.Row.FindControl("ddlAssetType") as DropDownList;
                        HiddenField hdfIsAsset = (HiddenField)e.Row.FindControl("hdfIsAsset");
                        HiddenField hdfItemIsAsset = (HiddenField)e.Row.FindControl("hdfItemIsAsset");
                        HiddenField hdfIsFormer = (HiddenField)e.Row.FindControl("hdfIsFormer");
                        HiddenField hdfAssetTypePK = (HiddenField)e.Row.FindControl("hdfAssetTypePK");
                        if (hdfInvoiceAssetTypeRequired.Value == "1")
                        {
                            if ((hdfItemIsAsset.Value == "1" || hdfAssetTypePK.Value != "") && hdfIsFormer.Value == "0")
                            {
                                //HiddenField hdfAssetTypePK = (HiddenField)e.Row.FindControl("hdfAssetTypePK");

                                chkIsAsset.Enabled = true;
                                grdInvoice.Columns[14].Visible = true;
                                grdInvoice.Columns[15].Visible = true;
                                GetFieldValues(ControlsEnum.ASSETTYPE);
                                if (dtPageData != null && dtPageData.Rows.Count > 0)
                                {
                                    dtPageData = dtPageData.Select("IsAssetFormer=0").CopyToDataTable();

                                    ddlAssetType.DataSource = dtPageData;
                                    ddlAssetType.DataTextField = "AssetTypeName";
                                    ddlAssetType.DataValueField = "atpPK";
                                    ddlAssetType.DataBind();
                                }
                                ddlAssetType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));

                                if (CurrPK != 0)
                                    ddlAssetType.SelectedIndex = ddlAssetType.Items.IndexOf(ddlAssetType.Items.FindByValue(hdfAssetTypePK.Value));

                                if (hdfIsAsset.Value == "1")
                                    ddlAssetType.Enabled = true;
                                else
                                    ddlAssetType.Enabled = false;
                            }
                            else
                            {
                                //grdInvoice.Columns[14].Visible = false;
                                //grdInvoice.Columns[15].Visible = false;
                                chkIsAsset.Enabled = false;
                                ddlAssetType.DataSource = null;
                                ddlAssetType.DataBind();
                                ddlAssetType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                                ddlAssetType.Enabled = false;
                            }
                        }
                        else
                        {
                            grdInvoice.Columns[14].Visible = false;
                            grdInvoice.Columns[15].Visible = false;
                        }
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        TextBox txtAdjustAmountFooter = (TextBox)e.Row.FindControl("txtAdjustAmountFooter");
                        if (!string.IsNullOrEmpty(txtAdjustAmountFooter.ID))
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtAdjustAmountFooter.ID + "", "$('[id$=" + txtAdjustAmountFooter.ID + "]').ForceToNumeric();", true);
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
                if (((GridView)sender).ID == "grdGrnAttchments")
                {
                    if (EntryStatus == EntryStatus.VIEWMODE)
                    {
                        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
                        {
                            e.Row.Cells[3].Visible = false;
                        }
                    }
                }

                //taxPayable For Multiplying with exchangerate

                if (((GridView)sender).ID == "grdTaxPayable")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        Label lblTaxAmount = e.Row.FindControl("lblTaxAmount") as Label;
                        Label lblAmountBeforeTax = e.Row.FindControl("lblAmountBeforeTax") as Label;
                        Label lblTaxCode = e.Row.FindControl("lblTaxCode") as Label;

                        double ExchangeRate = 0, TaxAmount = 0, AmountBeforTax = 0;
                        double TaxAmountLocal = 0, AmountBeforTaxLocal = 0;
                        ExchangeRate = string.IsNullOrEmpty(txtExchangeRate.Text) ? 1 : Convert.ToDouble(txtExchangeRate.Text);
                        TaxAmount = string.IsNullOrEmpty(lblTaxAmount.Text.Replace(",", "")) ? 1 : Convert.ToDouble(lblTaxAmount.Text.Replace(",", ""));
                        AmountBeforTax = string.IsNullOrEmpty(lblAmountBeforeTax.Text.Replace(",", "")) ? 1 : Convert.ToDouble(lblAmountBeforeTax.Text.Replace(",", ""));

                        //Multiply with Exchangerate                       
                        AmountBeforTaxLocal = AmountBeforTax * ExchangeRate;
                        TaxAmountLocal = TaxAmount * ExchangeRate;

                        lblAmountBeforeTax.Text = GetFormattedCurrencyWithSeperation(AmountBeforTaxLocal);
                        lblTaxAmount.Text = GetFormattedCurrencyWithSeperation(TaxAmountLocal);

                        //For Avoiding Tax code shown as NULL.
                        if (lblTaxCode.Text.ToString() == "null")
                        {
                            lblTaxCode.Text = "";
                        }
                    }
                }

                else if (((GridView)sender).ID == "grdOtherchargeSplit")
                {
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        Label lblTotalOtherCharge = (Label)e.Row.FindControl("lblTotalOtherCharge");
                        Label lblTotalInvOtherCharge = (Label)e.Row.FindControl("lblTotalInvOtherCharge");
                        Label lblTotalBalanceOtherCharge = (Label)e.Row.FindControl("lblTotalBalanceOtherCharge");
                        lblTotalOtherCharge.Text = GetFormattedCurrencyWithSeperation(POTotalOtherAmount);
                        lblTotalInvOtherCharge.Text = GetFormattedCurrencyWithSeperation(POTotalInvOtherAmount);
                        lblTotalBalanceOtherCharge.Text = GetFormattedCurrencyWithSeperation(POTotalBalanceOtherAmount);
                    }
                }
                else if (((GridView)sender).ID == "grdGRNDetails")
                {
                    //if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    //{
                    //    TextBox txtInvNowGRNQty = (TextBox)e.Row.FindControl("txtInvNowGRNQty");

                    //}
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        Label lblTotalGRNQty = (Label)e.Row.FindControl("lblTotalGRNQty");
                        Label lblTotalGRNInvdQty = (Label)e.Row.FindControl("lblTotalGRNInvdQty");
                        Label lblTotalGRNBalance = (Label)e.Row.FindControl("lblTotalGRNBalance");
                        Label lblTotalGrnDmgQty = (Label)e.Row.FindControl("lblTotalGrnDmgQty");
                        lblTotalGRNQty.Text = GetFormattedNumberWithSeperation(POTotalGRNQty);
                        lblTotalGRNInvdQty.Text = GetFormattedNumberWithSeperation(POTotalGRNInvdQty);
                        lblTotalGRNBalance.Text = GetFormattedNumberWithSeperation(POTotalGRNBalance);
                        lblTotalGrnDmgQty.Text = GetFormattedNumberWithSeperation(GRTotalDmgQty);

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
            GetFieldValues(ControlsEnum.INVOICELIST);
            SetFieldValues(ControlsEnum.INVOICELIST);
            EntryStatus = EntryStatus.LISTMODE;
            SetGridStatus();//E
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
                //if (SortBy == e.SortExpression)
                //{
                //    //Toggle the sort expression
                //    if (SortDirection == Resources.Report.SortAscending)
                //        SortDirection = Resources.Report.SortDescending;
                //    else
                //        SortDirection = Resources.Report.SortAscending;
                //}
                //else
                //{
                //    SortBy = e.SortExpression;
                //    SortDirection = Resources.Report.SortAscending;

                //}
                //this.PageIndex = "1";
                ////GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                ////SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                //EntryStatus = EntryStatus.LISTMODE;
                SortBy = e.SortExpression;
                if (SortDirection == Resources.Report.SortAscending)
                    SortDirection = Resources.Report.SortDescending;
                else
                    SortDirection = Resources.Report.SortAscending;
                GetFieldValues(ControlsEnum.INVOICELIST);
                SetFieldValues(ControlsEnum.INVOICELIST);
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
            btnSubmitInv.PreRender += new EventHandler(btnAction_PreRender);
            btnSave.PreRender += new EventHandler(btnAction_PreRender);
            btnDeleteNew.PreRender += new EventHandler(btnAction_PreRender);
            btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnJournalize.PreRender += new EventHandler(btnAction_PreRender);
            btnAlert.PreRender += new EventHandler(btnAction_PreRender);

            btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            btnView.PreRender += new EventHandler(btnAction_PreRender);
            btnPickForPayment.PreRender += new EventHandler(btnAction_PreRender);
            btnPickForCrDrNote.PreRender += new EventHandler(btnAction_PreRender);
            btnResetSelection.PreRender += new EventHandler(btnAction_PreRender);

            btnConvertAll.PreRender += new EventHandler(btnAction_PreRender);

            //lbnPOListing.PreRender += new EventHandler(btnAction_PreRender);
            //lnkInvoicing.PreRender += new EventHandler(btnAction_PreRender);
            //lbnExpenses.PreRender += new EventHandler(btnAction_PreRender);
            //lbnPOInvoice.PreRender += new EventHandler(btnAction_PreRender);
            //lnkPayment.PreRender += new EventHandler(btnAction_PreRender);
            //lnbCrDrNote.PreRender += new EventHandler(btnAction_PreRender);
            //lnbAcPayables.PreRender += new EventHandler(btnAction_PreRender);
            btnSaveDuduction.PreRender += new EventHandler(btnAction_PreRender);

            lnkList.PreRender += new EventHandler(btnAction_PreRender);
            lnkDetail.PreRender += new EventHandler(btnAction_PreRender);

            btnApply.PreRender += new EventHandler(btnAction_PreRender);
            imgPopupAdd.PreRender += new EventHandler(btnAction_PreRender);
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnAddItem.PreRender += new EventHandler(btnAction_PreRender);

            btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            btnSubmitInv.Load += new EventHandler(btnAction_Load);
            btnSave.Load += new EventHandler(btnAction_Load);
            btnDeleteNew.Load += new EventHandler(btnAction_Load);
            btnPrint.Load += new EventHandler(btnAction_Load);
            btnCancel.Load += new EventHandler(btnAction_Load);
            btnJournalize.Load += new EventHandler(btnAction_Load);
            btnAlert.Load += new EventHandler(btnAction_Load);

            btnEdit.Load += new EventHandler(btnAction_Load);
            btnView.Load += new EventHandler(btnAction_Load);
            btnPickForPayment.Load += new EventHandler(btnAction_Load);
            btnPickForCrDrNote.Load += new EventHandler(btnAction_Load);
            btnResetSelection.Load += new EventHandler(btnAction_Load);

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
            btnSaveDuduction.Load += new EventHandler(btnAction_Load);
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
                hdfSaveWithoutAllocation.Value = "0";
                if (POInvoiceHeaderSession != null)
                {
                    hdfHasTax.Value = ((POInvoiceHeaderSession.TaxHdr == null || POInvoiceHeaderSession.TaxHdr.Count == 0)
                        && POInvoiceHeaderSession.OrderDetail.All(dtl => (dtl.TaxDtl == null || dtl.TaxDtl.Count == 0)))
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

                //Settings of TaxPayableDiv              
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                //end
                decimal.TryParse(txtHdrDiscount.Text, out hrdDiscAmnt);
                if (hrdDiscAmnt > 0 && hdfShowEffRateInPI.Value == "1")
                    grdInvoice.Columns[2].Visible = true;
                else
                    grdInvoice.Columns[2].Visible = false;

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
            POINVHEADER,
            POINVDETAIL,
            TAXTYPES,
            TAXPOPUPGRID,
            TAXHEADER,
            EXCHANGERATE,
            INVOICELIST,
            INVOICEGET,
            JOURNALIZE,
            FINHEADER,
            PICKFORPAYMENT,
            PICKFORCRDRNOTE,
            POTYPE,
            GETINVOICEPKBYJOURNALPK,
            FILLWORKFLOWSTATUS,
            DEDUCTIONPOPUPGRID,
            ADVANCEDTAXSETTINGS,
            NOTIFICATIONTYPES,
            ALERTBASIS,
            ALERTTYPES,
            NOTIFICATIONDAYS,
            ALERTCONFIG,
            ALERTLIST,
            ALERTSAVE,
            COMPANY,
            UPLOADEDFILES,
            ADDITEM,
            SELECTEDDOC,
            VENDORBRANCH,
            VENDORCONTACTYPE,
            VENDORCONTACTYPEDETAILS,
            LINETAX,
            AMOUNTDETAILS,
            OTHERCHARGELIST,
            CUSTOMTAXSETTINGS,
            RESETPAYMENT,
            POLIST,
            POINVHEADERNEW,
            GRNQTYSPLIT,
            COMPANYSRCH,
            GSTINVOICETYPE,
            GRNATTACHMENTS,
            COSTCENTERDTL,
            PURCHASEORDERTYPE,
            ASSETTYPE
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

        private void ValidateAdjAmount()
        {
            if (POInvoiceHeaderSession != null)
            {
                TextBox txtAdjustAmountFooter = (TextBox)grdInvoice.FooterRow.FindControl("txtAdjustAmountFooter");
                TextBox txtSubTotalFooter = (TextBox)grdInvoice.FooterRow.FindControl("txtSubTotalFooter");
                double maxRoundoff = 0;
                double minRoundoff = 0;
                if (txtAdjustAmountFooter != null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtAdjustAmountFooter.ID + "", "$('[id$=" + txtAdjustAmountFooter.ID + "]').ForceToNumeric();", true);
                    PriceAdjustConfiguration();
                    // maxRoundoff = Convert.ToDouble(txtSubTotalFooter.Text) * Convert.ToDouble(hdfPriceAdjPercentage.Value) / 100;
                    maxRoundoff = Convert.ToDouble(txtSubTotalFooter.Text);// * Convert.ToDouble(hdfPriceAdjPercentage.Value) / 100;
                    minRoundoff = maxRoundoff * -1;
                    if (!string.IsNullOrEmpty(txtAdjustAmountFooter.Text))
                    {
                        if (Convert.ToDouble(txtAdjustAmountFooter.Text) > maxRoundoff || Convert.ToDouble(txtAdjustAmountFooter.Text) < minRoundoff)
                        {
                            txtAdjustAmountFooter.Text = string.Empty;
                            POInvoiceHeaderSession.IVH_AMOUNT_NET_TC_ADJ = 0;
                            litErrorMsg.Text = string.Format(GetLocalResourceObject("Err_MsgExceeds_Roundoff").ToString(), hdfPriceAdjPercentage.Value);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            POInvoiceHeaderSession.IVH_AMOUNT_NET_TC_ADJ = string.IsNullOrEmpty(txtAdjustAmountFooter.Text.Trim()) ? 0 : Convert.ToDouble(txtAdjustAmountFooter.Text);
                        }
                    }
                    else
                    {
                        POInvoiceHeaderSession.IVH_AMOUNT_NET_TC_ADJ = string.IsNullOrEmpty(txtAdjustAmountFooter.Text.Trim()) ? 0 : Convert.ToDouble(txtAdjustAmountFooter.Text);
                    }
                    SetHdrTax();
                    SetEffectiveRate();
                }
            }
        }

        /// <summary>
        /// Define Activity Status Enum
        /// </summary>
        public enum TaxSettingEnum
        {
            HEADERWISE = 0,
            ITEMWISE = 1,
            BOTHHEADERITEM = 2
        }
        private double SetTaxApplicableAmount()
        {
            double hdrSubTotal = 0;
            double hdrDisc = 0;
            double hdrOtherCharge = 0;
            double amount = 0;

            if (chkSubTotal.Checked)
            {
                TextBox txtSubTotal = (TextBox)grdInvoice.FooterRow.FindControl("txtSubTotalFooter");
                hdrSubTotal = txtSubTotal == null ? 0 : string.IsNullOrEmpty(txtSubTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtSubTotal.Text.Trim());
            }
            if (chkDiscount.Checked)
            {
                hdrDisc = string.IsNullOrEmpty(txtHdrDiscount.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrDiscount.Text.Trim());
            }
            if (chkOtherCharges.Checked)
            {
                hdrOtherCharge = string.IsNullOrEmpty(txtShipping.Text.Trim()) ? 0 : Convert.ToDouble(txtShipping.Text.Trim());
            }
            if (hdrSubTotal == 0)
                amount = hdrDisc + hdrOtherCharge;
            else
                amount = (hdrSubTotal - hdrDisc) + hdrOtherCharge;
            return amount;
        }
        private void ResetTaxApplicableCheckbox()
        {
            chkSubTotal.Checked = false;
            chkDiscount.Checked = false;
            chkOtherCharges.Checked = true;
        }
        private bool IsQuantityValidationRequired()
        {
            if ((GetGlobalResourceObject("ConfigurationsRes", "InvoiceQtyCanExceedOrderQtyINVC").ToString() == "1") && IvhGroup == (int)InvoiceTypeEnum.WORKORDER)
                return false;
            else
                return true;
        }
        public enum InvoiceTypeEnum
        {
            WORKORDER = 6
        }
    }
}
