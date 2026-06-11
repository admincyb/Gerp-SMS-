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
    public partial class PurchaseOrderInvoiceTrading : ERP.Store.UI.WorkFlowBasePage
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
        /// To maintain keep PO Invoice Header Tax Splitting
        /// </summary>
        private DirectPOInvoiceHeader TempInvoiceHeaderTemp
        {
            get
            {
                return (DirectPOInvoiceHeader)Session[ERP.Utilities.SessionStrings.TempSOInvoiceDtlSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.TempSOInvoiceDtlSession] = value;
            }
        }
        private DirectPOInvoiceHeader POInvoiceHdrSession
        {
            get
            {
                return (DirectPOInvoiceHeader)Session[ERP.Utilities.SessionStrings.POInvoiceHeaderSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.POInvoiceHeaderSession] = value;
            }
        }

        /// <summary>
        /// To maintain keep PO Invoice Header Tax Splitting
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

        /// <summary>
        /// Is invoice cancelled or not
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
        public bool IsTaxProportionate { get; set; }

        /// <summary>
        /// To keep config value for Enable/Disable Header Discount in viewstate
        /// </summary>
        private bool IsHeaderDiscountForTradingPurchase
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsHeaderDiscountForTradingPurchase] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsHeaderDiscountForTradingPurchase].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsHeaderDiscountForTradingPurchase] = value;
            }
        }
        /// <summary>
        /// To keep config value for Enable/Disable Header Tax in viewstate
        /// </summary>
        private bool IsHeaderTaxForTradingPurchase
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsHeaderTaxForTradingPurchase] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsHeaderTaxForTradingPurchase].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsHeaderTaxForTradingPurchase] = value;
            }
        }
        /// <summary>
        /// To keep config value for Enable/Disable Item Discount in viewstate
        /// </summary>
        private bool IsItemwiseDiscountForTradingPurchase
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsItemwiseDiscountForTradingPurchase] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsItemwiseDiscountForTradingPurchase].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsItemwiseDiscountForTradingPurchase] = value;
            }
        }
        /// <summary>
        /// To keep config value for Enable/Disable Item Tax in viewstate  
        /// </summary>
        private bool IsItemwiseTaxForTradingPurchase
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsItemwiseTaxForTradingPurchase] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsItemwiseTaxForTradingPurchase].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsItemwiseTaxForTradingPurchase] = value;
            }
        }
        /// <summary>
        /// Purchase Order PK
        /// </summary>
        private int PoPK
        {
            get
            {
                return this.ViewState["PoPK"] == null ? 0 : Convert.ToInt32(this.ViewState["PoPK"]);
            }
            set
            {
                this.ViewState["PoPK"] = value;
            }
        }
        /// <summary>
        /// Purchase Order Details PK
        /// </summary>
        private int PoDetailsPK
        {
            get
            {
                return this.ViewState["PoDetailsPK"] == null ? 0 : Convert.ToInt32(this.ViewState["PoDetailsPK"]);
            }
            set
            {
                this.ViewState["PoDetailsPK"] = value;
            }
        }
        #endregion

        // Indicates the state as well as action
        private ActionsEnum commonActions;
        User currentUser;          
        private DirectPOHeaderBO PoHeaderObj;
        private DirectPOInvoiceHeader invoiceHeaderObj;   
        private DirectPOInvoiceDetails poInvoiceDetailsObj;
        private DirectPOInvoiceTaxHdr poInvTaxHdrObj;        
        private DirectGRNQTYDetails poGrnQtyObj;
        List<DirectPOAdvDeductionDetails> deductionDtlList;      
        DirectPOInvoiceUploads poUploadObj;
     
        List<DirectPOInvoiceDetails> poInvoiceDetailsList;
        private List<DirectPOInvoiceDetails> invoiceHdrMulDummyLIST;
        List<DirectPOInvoiceMappingDetails> poOtherChargeList;
        private DirectPOInvoiceMappingDetails poInvOtherchargeObj;
        List<DirectGRNQTYDetails> grnList; 
        List<DirectPOInvoiceTaxHdr> taxHdrList;
        DirectPOInvoiceDetails poDtlObj;
        DataTable dtTaxDetails;
        private DataTable dtTaxDetData;
        private DataTable dtAmountDetails;
        DataTable dtTaxSettings;      
        DataSet dsAdsType;
        DataSet dsAdsTypeDtl;   
        int JournalPK;
        DataSet dsPageData;
        private DataTable dtInvoiceList;       
        private DataTable dtPageData;
        private DataTable dtCustomTaxSet;
        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;
        private string refID;
        private string inboxFlag;

        private decimal totalAllocatedTax;       
        private decimal amtAdjAdvDeduction = 0;        
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
        private List<ADM_CONST_MST> admConstMstList;
        private ADM_APP_CONFIG_MST admAppConfigMstObj;
        private List<ADM_APP_CONFIG_MST> admAppConstMstList;
        DataSet dsAlertList;
        private int invPK;
        private int VncPk = 0;
        private string TypeRef;
        private string appType;
        private int vendPK = 0;
        private int purchaseOrderPK = 0;
        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        private FIN_INVOICE_VND_HDR finInvoiceVndHdrObj;
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
                    #region Default Bindings (COMPANY,POTYPE)
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANYSRCH);
                    GetFieldValues(ControlsEnum.CUSTOMTAXSETTINGS);//Enable or disable custom tax 
                    SetControlsVisibility();
                    GetFieldValues(ControlsEnum.POTYPE);
                    SetFieldValues(ControlsEnum.POTYPE);
                    txtFromDate.Text = hdfFromDate.Value = string.Empty;
                    txtToDate.Text = hdfToDate.Value = string.Empty;
                    AST_DOC_MODE.Value = "0";
                    POInvoiceHeaderSession = null;                   
                    #endregion
                    ConfigurationSettings();
                    #region Grid DataKey Settings
                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "DOC_SEQ_NO";
                    grdUploads.DataKeyNames = itemkeyarray;
                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = "IVH_PK";
                    grdInvoiceList.DataKeyNames = datakeyarray;                   
                    #endregion     
                    uclPaging.CurrentPage = 1;
                    uclPOPaging.CurrentPage = 1; 
                    
                    #region Rate,Qty,Amount Setting
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
                    
                    #endregion

                    #region pid,refID,prefID,inboxFlag
                    string pid = Request.QueryString[QueryStrings.PID] != null ? Request.QueryString[QueryStrings.PID]
                                    : Session[ERP.Utilities.SessionStrings.PID] != null ? Session[ERP.Utilities.SessionStrings.PID].ToString().Split('=')[1] : string.Empty;
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                        : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty; 
                    #endregion

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
                            #region Has RefID
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
                            ReferanceID = int.Parse(refID);
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
                                GetFieldValues(ControlsEnum.GETINVOICEPKBYJOURNALPK);
                                if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                                {
                                    CurrPK = (Int32)finTrxHdrList[0].FTH_REF_PK;
                                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                    base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                                }
                            }
                            #endregion
                        }
                        else if (!string.IsNullOrEmpty(prefID))
                        {
                            base.WkfRefID = ucrWrkf.RefID = int.Parse(prefID);
                            CurrPK = GetApplicationID(ucrWrkf.RefID);
                            ReferanceID = int.Parse(prefID);
                        }
                        if (CurrPK > 0)
                        {
                            SetInvoicePODetails();
                        }
                        else
                        {
                            TempPOInvoiceHeaderSession = null;
                            POInvoiceHeaderSession = null;
                            GetFieldValues(ControlsEnum.INVOICELIST);
                            SetFieldValues(ControlsEnum.INVOICELIST);
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                        AST_CODE.Value = POGroup == POInvoiceGroup.Goods ? ApplicationType.TPI: POGroup == POInvoiceGroup.Services ? ApplicationType.TPSI : ApplicationType.EIT;
                        AST_DOC_MODE.Value = GetDOCMODE();
                        lblInvoiceNo.Text = hdfInvoiceNo.Value == string.Empty ? "[NEW]" : hdfInvoiceNo.Value;
                        hdfAppType.Value = AST_CODE.Value;
                        hdfAppSubType.Value = string.Empty;
                        SetEffectiveRate();
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
            #region Purchase Item/Header Tax/Discount Settings
            IsHeaderTaxForTradingPurchase = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsHeaderTaxForTradingPurchase")));
            IsHeaderDiscountForTradingPurchase = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsHeaderDiscountForTradingPurchase")));
            IsItemwiseTaxForTradingPurchase = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsItemwiseTaxForTradingPurchase")));
            IsItemwiseDiscountForTradingPurchase = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsItemwiseDiscountForTradingPurchase")));
            #endregion
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            string xmlDocPO = string.Empty;
            try
            {
                switch (type)
                {
                    #region INVOICELIST
                    case ControlsEnum.INVOICELIST:
                        int cusID = String.IsNullOrEmpty(hdfVendorSearch.Value.Trim()) ? 0 : Convert.ToInt32(hdfVendorSearch.Value);
                        if (hdfVendorSearch.Value != "" && hdfVendorSearch.Value != "0")
                        {
                            Session[ERP.Utilities.SessionStrings.VendorPK] = hdfVendorSearch.Value;
                            Session[ERP.Utilities.SessionStrings.Vendor] = txtVendorSearch.Text;
                        }
                        int InvPk = String.IsNullOrEmpty(hdfIVHPK.Value.Trim()) ? 0 : Convert.ToInt32(hdfIVHPK.Value);
                        int Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        int cmpPk = Convert.ToInt32(ddlCompanySrch.SelectedValue);
                        string vendor = string.IsNullOrEmpty(txtVendorSearch.Text.Trim()) ? string.Empty : (txtVendorSearch.Text.Trim() == "Select/Type" ? string.Empty : txtVendorSearch.Text.Trim());
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
                            }, currentUser, cusID, InvPk, CurrPOPK, vendor, txtpoNo.Text
                            , Resources.PageURL.PurchaseOrderInvoicingTrading.Replace("~", "")
                            , (ddlOrderType.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlOrderType.SelectedValue) : 0)
                            , Status
                            , 0, (byte)POInvoiceCategory.Invoice
                            , chkPending.Checked == true ? (byte)1 : (byte)0
                            , string.IsNullOrEmpty(txtGrnNo.Text.Trim()) ? null : txtGrnNo.Text.Trim()
                            , string.IsNullOrEmpty(txtDueAson.Text.Trim()) ? string.Empty : txtDueAson.Text.Trim()
                            , cmpPk);

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
                        invoiceHeaderObj = BusinessLogic.POInvoicing.POInvoiceBL.GetDirectPurchaseInvoiceHeaderMUL(xmlDocPO, !string.IsNullOrEmpty(xmlDocPO) ? 0 : CurrPK);

                        if (POInvoiceHeaderSession == null)
                            POInvoiceHeaderSession = invoiceHeaderObj.DeepClone();
                        else if (invoiceHeaderObj != null)
                        {
                            List<string> objPoList = POInvoiceHeaderSession.OrderDetail.Select(r => r.VID_PO).Distinct().ToList();
                            List<long> objAdvList = POInvoiceHeaderSession.DeductionDetails.Select(r => r.VAD_PK).ToList();
                            List<int> objOtherChargeSoList = POInvoiceHeaderSession.TaxHdr.Select(r => r.VTL_PO).ToList();
                            POInvoiceHeaderSession.OrderDetail.AddRange(invoiceHeaderObj.OrderDetail.Where(r => !objPoList.Contains(r.VID_PO)).ToList());
                            POInvoiceHeaderSession.DeductionDetails.AddRange(invoiceHeaderObj.DeductionDetails.Where(r => !objAdvList.Contains(r.VAD_PK)).ToList());
                            POInvoiceHeaderSession.TaxHdr.AddRange(invoiceHeaderObj.TaxHdr.Where(r => !objOtherChargeSoList.Contains(r.VTL_PO) && r.VTL_TAX_CATEGORY == (int)TaxType.Shipping).ToList());
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
                    #endregion
                    #region Invoice Hdr By PK
                    case ControlsEnum.LINETAX:
                        dtTaxDetData = BusinessLogic.POInvoicing.POInvoiceBL.GetLineitemTaxList(PaymentMpgPK).Tables[0];//ReceiptMpgPK);
                        break;
                    #endregion

                    #region INVOICEGET
                    case ControlsEnum.INVOICEGET:
                        int GInvPk = Convert.ToInt32(Request.QueryString[QueryStrings.PK]);
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
                                SearchValue = string.IsNullOrEmpty(txtInvoiceNumber.Text.Trim()) ? string.Empty : (txtInvoiceNumber.Text.Trim() == "Select/Type" ? string.Empty : txtInvoiceNumber.Text.Trim())
                            }, currentUser, 0, GInvPk, CurrPOPK, string.Empty, txtpoNo.Text
                            , Resources.PageURL.PurchaseOrderInvoicing.Replace("~", "")
                            , (ddlOrderType.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlOrderType.SelectedValue) : 0)
                            , 0
                            , 0, (byte)POInvoiceCategory.Invoice
                            , chkPending.Checked == true ? (byte)1 : (byte)0
                            , string.IsNullOrEmpty(txtGrnNo.Text.Trim()) ? null : txtGrnNo.Text.Trim()
                            , string.IsNullOrEmpty(txtDueAson.Text.Trim()) ? string.Empty : txtDueAson.Text.Trim());
                        if (dsPageData != null)
                        {
                            DataView dvInvoice = dsPageData.Tables[1].DefaultView;
                            dtInvoiceList = dvInvoice.ToTable();
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
                    #region POTYPE
                    case ControlsEnum.POTYPE:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PURCHASE INVOICE TYPE");
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
                        admConfigMstObj = new ADM_CONFIG_MST();
                        admConfigMstObj.CFG_PK = 0;
                        admConfigMstObj.CFG_TYPE = GetLocalResourceObject("APPLICATION_STATUS").ToString();
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        workflowStatusList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    #endregion
                    #region ADVANCEDTAXSETTINGS
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
                    #region EXCHANGERATE
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
                    #region PENDINGPOLIST
                    case ControlsEnum.PENDINGPOLIST:
                        BindGrid(controlType);
                        break;
                    #endregion
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

                            if (TaxApplcableinLine > 0 || DiscountApplcableinLine > 0)
                            {

                                if (invoiceHeaderObj != null)
                                {
                                    poInvoiceDetailsList = new List<DirectPOInvoiceDetails>();                                   
                                    poInvoiceDetailsList = invoiceHeaderObj.OrderDetail;//.Where(sod => sod.CID_QTY_DISPATCHED > 0).ToList();//To hide items without despatched and shipping qty                                  
                                }

                                decimal LineitemGridSum = 0;
                                decimal LineitemGridSumDiscount = 0;
                                poInvoiceDetailsList.ForEach(dt => { LineitemGridSum = LineitemGridSum + Convert.ToDecimal(dt.VID_TAX); });
                                poInvoiceDetailsList.ForEach(dt => { LineitemGridSumDiscount = LineitemGridSumDiscount + Convert.ToDecimal(dt.VID_DISCOUNT); });

                                if (poInvoiceDetailsList.Count > 0 || poInvoiceDetailsList != null)
                                {
                                    poInvoiceDetailsList.ForEach(dtl =>
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
                                            if (dtl2.VTL_TAX_CATEGORY == (byte)TaxType.Tax && (dtl.VID_TAX > 0))
                                            {
                                                dtl2.VTL_TAX_AMT = ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(dtl2.VTL_TAX_AMT - ((dtl2.VTL_TAX_AMT / dtl.VID_TAX) * applicableAmt)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                            }
                                            else if (dtl2.VTL_TAX_CATEGORY == (byte)TaxType.Discount && (dtl.VID_DISCOUNT > 0))
                                            {
                                                dtl2.VTL_TAX_AMT = ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(dtl2.VTL_TAX_AMT - ((dtl2.VTL_TAX_AMT / dtl.VID_DISCOUNT) * applicableAmtDiscount)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                            }

                                        });                                       
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
                                    poInvoiceDetailsList.SingleOrDefault(dtl => dtl.VID_PK == Convert.ToInt16(hdfInvoiceDtlPK.Value)).VID_QTY_INVOICED = Convert.ToDouble(txtInvNow.Text);                                   
                                }
                                invoiceHeaderObj.OrderDetail = poInvoiceDetailsList;
                                if (poInvoiceDetailsList != null)
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
                    #region GRNQTYSPLIT
                    case ControlsEnum.GRNQTYSPLIT:
                        BindGrid(ControlsEnum.GRNQTYSPLIT);
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
        private void SetInvoicePODetails()
        {
            SetCancelRef(CurrPK);
            ucrWrkf.FillWorkFlowDetails();
            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                ucrWrkf.ViewType = 1;
            else
            {
                ucrWrkf.ViewType = 0;
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
            if (CurrPK == 0 && POInvoiceHeaderSession != null && POInvoiceHeaderSession.OrderDetail != null)
            {
                POInvoiceHeaderSession.IVH_AMOUNT_TC = 0;
                POInvoiceHeaderSession.IVH_AMOUNT_NET_TC_ADJ = 0;
                POInvoiceHeaderSession.IVH_TAX_TC = 0;
                POInvoiceHeaderSession.IVH_DISCOUNT_TC = 0;
                POInvoiceHeaderSession.IVH_AMOUNT_NET_TC = 0;
                POInvoiceHeaderSession.IVH_AMOUNT_NET_BC = 0;
                foreach (DirectPOInvoiceDetails dtl in POInvoiceHeaderSession.OrderDetail)
                {
                    dtl.VID_QTY_INVOICED = 0;
                    dtl.VID_AMOUNT = 0;
                    dtl.VID_TAX = 0;
                    dtl.VID_DISCOUNT = 0;
                    dtl.VID_NET_AMOUNT = 0;

                    if (Convert.ToBoolean(dtl.VID_HAS_GRN))
                    {
                        //For Avoiding Negative Quantity -----------                       
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
            }
            SetFieldValues(ControlsEnum.POINVHEADER);
            SetFieldValues(ControlsEnum.POINVDETAIL);
            SetFieldValues(ControlsEnum.UPLOADEDFILES);
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
            //Settings of TaxPayableDiv
            if (hdfIsTaxPayable.Value.ToString() == "1")
            {
                SetTaxPayableDiv();
            }
            GetFieldValues(ControlsEnum.PENDINGPOLIST);
            SetFieldValues(ControlsEnum.PENDINGPOLIST);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
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
        #region Set BranchCode Visibility
        private void SetBranchCodeVisibility()
        {
            txtBranchCode.Text = string.Empty;
            txtVatTaxId.Text = string.Empty;           
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
                        txtBranchCode.Text = "";
                        vrfBranchCode.Enabled = false;                       
                    }
                    SetFieldValues(ControlsEnum.VENDORCONTACTYPEDETAILS);
                }
            }
        }
        #endregion
      
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
            TextBox txtAmount;
            TextBox txtDiscount;
            TextBox txtTax;
            TextBox txtTotal;
            TextBox txtSubTotal;
            TextBox txtAdjustAmountFooter;
            List<DirectPOInvoiceTaxHdr> rfqTaxHeaderList;
            bool bIsChecked = false;
            AlertBO alertBoObj;
            RadioButton rbtn;
            try
            {
                switch (controlType)
                {
                    #region POINVHEADER
                    case ControlsEnum.POINVHEADER:
                        if (POInvoiceHeaderSession != null)
                        {
                            invoiceHeaderObj = POInvoiceHeaderSession;
                            invoiceHeaderObj.IVH_PK = CurrPK;
                            invoiceHeaderObj.IVH_NO = invoiceHeaderObj.IVH_PK.ToString();
                            invoiceHeaderObj.IVH_VERSION = 1;
                            invoiceHeaderObj.IVH_STATUS = 0;
                            invoiceHeaderObj.IVH_CATEGORY = (byte)POInvoiceCategory.Invoice;

                            invoiceHeaderObj.IVH_VENDOR_NAME = HttpUtility.HtmlEncode(txtVendorHd.Text);
                            //For avoiding XML parsing error : illegal name character (&)
                            invoiceHeaderObj.IVH_VENDOR_TEXT = HttpUtility.HtmlEncode(invoiceHeaderObj.IVH_VENDOR_TEXT);
                            invoiceHeaderObj.IVH_VENDOR_ADDRESS = HttpUtility.HtmlEncode(invoiceHeaderObj.IVH_VENDOR_ADDRESS);
                            invoiceHeaderObj.IVH_VENDOR_COUNTRY_TEXT = HttpUtility.HtmlEncode(invoiceHeaderObj.IVH_VENDOR_COUNTRY_TEXT);
                            invoiceHeaderObj.IVH_CURRENCY_TEXT = HttpUtility.HtmlEncode(invoiceHeaderObj.IVH_CURRENCY_TEXT);

                            invoiceHeaderObj.IVH_DATE = string.IsNullOrEmpty(txtInvoiceDate.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtInvoiceDate.Text.Trim();
                            invoiceHeaderObj.IVH_DATE_PAY_BY = string.IsNullOrEmpty(txtInvoiceDueDate.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtInvoiceDueDate.Text.Trim();
                            invoiceHeaderObj.IVH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                            invoiceHeaderObj.IVH_SHIP_CHARGE_DED = txtDeductOtherCharges.Text;
                            invoiceHeaderObj.IVH_VENDOR_INV_NO = HttpUtility.HtmlDecode(txtSupplierInvNO.Text);
                            //do you want to continue with duplicate vendor invoice no
                            if (hdfIsContDupVenInvNo.Value == "1")
                            {
                                invoiceHeaderObj.IVH_ALLOW_DUP_INV_NO = 1; //allow to save Duplicate vendor invoice no.ie,no need for checking if vendor invoice no already exist or not
                            }
                            else
                            {
                                invoiceHeaderObj.IVH_ALLOW_DUP_INV_NO = 0; //check if vendor invoice no already exist or not
                            }
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
                            #region To remove zero amount entries from adv deduction list
                            if (POInvoiceHeaderSession.DeductionDetails != null && POInvoiceHeaderSession.DeductionDetails.Count > 0)
                            {
                                List<DirectPOAdvDeductionDetails> objDeductionList = POInvoiceHeaderSession.DeductionDetails.Where(r => r.VAD_AMOUNT == 0).ToList();
                                objDeductionList.ForEach(dtl =>
                                {
                                    POInvoiceHeaderSession.DeductionDetails.Remove(dtl);
                                });
                            }
                            #endregion

                            SetUIValuesToObject(ControlsEnum.POINVDETAIL);

                            //For Solving Foreign Key ref issue:If GRNQty is zero (which is applied through popup) then remove that GRN from OrderDetail List
                            invoiceHeaderObj.OrderDetail.ForEach(dtl => dtl.GRNDtl.RemoveAll(grndtl => grndtl.VGL_QTY_INVOICED == 0));
                            invoiceHeaderObj.IVH_TOTAL_QTY = 0;
                            txtSubTotal = (TextBox)grdInvoice.FooterRow.FindControl("txtSubTotalFooter");
                            invoiceHeaderObj.IVH_AMOUNT_TC = txtSubTotal == null ? 0 : string.IsNullOrEmpty(txtSubTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtSubTotal.Text.Trim());
                            txtAdjustAmountFooter = (TextBox)grdInvoice.FooterRow.FindControl("txtAdjustAmountFooter"); //Adjust amount of IVH_AMOUNT_TC
                            invoiceHeaderObj.IVH_AMOUNT_NET_TC_ADJ = txtAdjustAmountFooter == null ? 0 : string.IsNullOrEmpty(txtAdjustAmountFooter.Text.Trim()) ? 0 : Convert.ToDouble(txtAdjustAmountFooter.Text.Trim());

                            #region Application Code,AST_VALUE,AST_DOC_MODE
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
                            //Uploads
                            invoiceHeaderObj.FileList = POUploadList;
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
                            invoiceHeaderObj.IVH_IMP_DECL_NO = txtDeclarationNO.Text == null ? string.Empty : HttpUtility.HtmlEncode(txtDeclarationNO.Text);

                            double InvSubTotal = 0;
                            InvSubTotal = POInvoiceHeaderSession.IVH_AMOUNT_TC + POInvoiceHeaderSession.IVH_AMOUNT_NET_TC_ADJ;
                            //Total Amount against PO
                            foreach (DirectPOInvoiceMappingDetails li in POInvoiceHeaderSession.POMappingDetails)
                            {
                                li.IVM_AMOUNT = POInvoiceHeaderSession.OrderDetail.Where(p => Convert.ToInt32(p.VID_PO) == li.IVM_PO_HDR).Sum(res => res.VID_AMOUNT);
                                if (InvSubTotal > 0)
                                {
                                    li.IVM_TAX_AMOUNT = (li.IVM_AMOUNT * POInvoiceHeaderSession.IVH_TAX_TC) / InvSubTotal;
                                    li.IVM_DISCOUNT_AMOUNT = (li.IVM_AMOUNT * POInvoiceHeaderSession.IVH_DISCOUNT_TC) / InvSubTotal;
                                }
                            }
                        }
                        retObject = invoiceHeaderObj;
                        break;
                    #endregion
                    #region POINVDETAIL
                    case ControlsEnum.POINVDETAIL:
                        rowID = 0;
                        invoiceHdrMulDummyLIST = new List<DirectPOInvoiceDetails>();
                        foreach (GridViewRow grdrow in grdInvoice.Rows)
                        {
                            poInvoiceDetailsObj = new DirectPOInvoiceDetails();

                            hdfRRDPK = (HiddenField)grdInvoice.Rows[rowID].FindControl("hdfInvoiceDtlDummyPK");
                            hdfItemPK = (HiddenField)grdInvoice.Rows[rowID].FindControl("hdfItemPK");
                            hdfPOPK = (HiddenField)grdInvoice.Rows[rowID].FindControl("hdfPOPK");
                            int invDtlPK;
                            int POPK, ItemPk;
                            ItemPk = Convert.ToInt32(hdfItemPK.Value);
                            POPK = Convert.ToInt32(hdfPOPK.Value);
                            double effRate = 0;
                            if (hdfRRDPK != null && int.TryParse(hdfRRDPK.Value, out invDtlPK))
                            {
                                poInvoiceDetailsObj = POInvoiceHeaderSession.OrderDetail.SingleOrDefault(dtl => dtl.VID_SL_UK == invDtlPK);
                                if (poInvoiceDetailsObj != null)
                                {
                                    poInvoiceDetailsObj.VID_SL_NO = rowID + 1;
                                    TextBox txtInvNow = (TextBox)grdInvoice.Rows[rowID].FindControl("txtInvNow");
                                    poInvoiceDetailsObj.VID_QTY_INVOICED = txtInvNow == null ? 0 : string.IsNullOrEmpty(txtInvNow.Text) ? 0 : Convert.ToDouble(txtInvNow.Text);
                                    txtAmount = (TextBox)grdInvoice.Rows[rowID].FindControl("txtAmount");
                                    poInvoiceDetailsObj.VID_AMOUNT = txtAmount == null ? 0 : string.IsNullOrEmpty(txtAmount.Text.Trim()) ? 0 : Convert.ToDouble(txtAmount.Text.Trim());
                                    txtDiscount = (TextBox)grdInvoice.Rows[rowID].FindControl("txtDiscount");
                                    poInvoiceDetailsObj.VID_DISCOUNT = txtDiscount == null ? 0 : string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? 0 : Convert.ToDouble(txtDiscount.Text.Trim());
                                    txtTax = (TextBox)grdInvoice.Rows[rowID].FindControl("txtTax");
                                    poInvoiceDetailsObj.VID_TAX = txtTax == null ? 0 : string.IsNullOrEmpty(txtTax.Text.Trim()) ? 0 : Convert.ToDouble(txtTax.Text.Trim());
                                    txtTotal = (TextBox)grdInvoice.Rows[rowID].FindControl("txtTotal");
                                    poInvoiceDetailsObj.VID_NET_AMOUNT = txtTotal == null ? 0 : string.IsNullOrEmpty(txtTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtTotal.Text.Trim());
                                    hasValidRate = hasValidRate || poInvoiceDetailsObj.VID_QTY_INVOICED >= 0;

                                    if (poInvoiceDetailsObj.TaxDtl != null)
                                    {
                                        rfqTaxHeaderList = poInvoiceDetailsObj.TaxDtl.ToList();
                                        if (rfqTaxHeaderList != null && rfqTaxHeaderList.Count > 0)
                                        {
                                            rfqTaxHeaderList.ForEach(dtl => dtl.VTL_SL_NO = poInvoiceDetailsObj.VID_SL_NO);
                                        }
                                    }

                                    HiddenField hdfEffRate = (HiddenField)grdInvoice.Rows[rowID].FindControl("hdfEffRate");
                                    double.TryParse(hdfEffRate.Value, out effRate);
                                    poInvoiceDetailsObj.VID_RATE_EFCT = effRate;
                                }
                            }
                            invoiceHdrMulDummyLIST.Add(poInvoiceDetailsObj);
                            rowID++;
                        }
                        POInvoiceHeaderSession.OrderDetail.Clear();
                        POInvoiceHeaderSession.OrderDetail = invoiceHdrMulDummyLIST;
                        break;
                    #endregion
                    #region Journalize
                    case ControlsEnum.JOURNALIZE:
                        if (EntryStatus == EntryStatus.LISTMODE)
                        {
                            foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                            {
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
                        }
                        else if (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.VIEWMODE)
                        {
                            bIsChecked = true;
                        }
                        if (bIsChecked)
                        {
                            if (Approved == 2)
                            {
                                GetFieldValues(ControlsEnum.POINVHEADER);
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

                                Invoice = POGroup == POInvoiceGroup.Goods ? ApplicationType.TPIJ
                                    : POGroup == POInvoiceGroup.Services ? ApplicationType.TPSIJ : ApplicationType.EITJ;
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
                                Session[ERP.Utilities.SessionStrings.JournalType] = POGroup == POInvoiceGroup.Goods ? ApplicationType.TPIJ
                                    : POGroup == POInvoiceGroup.Services ? ApplicationType.TPSIJ : ApplicationType.EITJ;
                                ucrWrkf.WrkfSubmit -= ActionHandler;
                                ucrWrkf.Reset();
                                ucrWrkf.ViewType = 1;
                                FillProcessID(2);
                                GetFieldValues(ControlsEnum.FINHEADER);
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
                                }
                                else
                                {
                                    ucrWrkf.ViewType = 0;
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
                                    numberGenerationSubType = 0;
                                }
                                else
                                {
                                    numberGenerationSubType = string.IsNullOrEmpty(hdfPOItemType.Value) ? (int)AppSubTypeCNPurchase.STOCK : Convert.ToInt16(hdfPOItemType.Value) == (Int16)POItemType.Others ?
                                        (int)AppSubTypeCNPurchase.NONSTOCK : (int)AppSubTypeCNPurchase.STOCK;
                                }

                                ucrJournalize.TypeForNumberGenaration = numberGenerationSubType.ToString();
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

                    #region ALERT
                    case ControlsEnum.ALERTSAVE:
                        string Typename = Resources.Constants.SystemAlertType;
                        int AlertPk = 0;
                        alertBoObj = new AlertBO();
                        appType = POGroup == POInvoiceGroup.Goods ? ApplicationType.TPI
                            : POGroup == POInvoiceGroup.Services ? ApplicationType.TPSI : ApplicationType.EIT;
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
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            int dept = 0;
            try
            {
                switch (controlType)
                {
                    #region INVOICEGET
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
                    #region SELECTEDDOC
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
                    #region POINVHEADER
                    case ControlsEnum.POINVHEADER:
                        invoiceHeaderObj = POInvoiceHeaderSession;
                        if (invoiceHeaderObj != null)
                        {
                            POGroup = (POInvoiceGroup)Enum.Parse(typeof(POInvoiceGroup), invoiceHeaderObj.IVH_GROUP.ToString());
                            txtVendorHd.Text = ERP.Utilities.CommonFunctions.GetDecodedString(invoiceHeaderObj.IVH_VENDOR_NAME);
                            hdfVendorHd.Value = invoiceHeaderObj.IVH_VENDOR.ToString();   
                            Approved = invoiceHeaderObj.IVH_STATUS;                           
                            vPK = Convert.ToInt32(invoiceHeaderObj.IVH_VENDOR);                          
                            hdfCrDrStatus.Value = invoiceHeaderObj.IVH_STATUS.ToString();
                            hdfCrDrWKFStatus.Value = Convert.ToInt16(invoiceHeaderObj.IVH_HAS_JRNL_ENTRY).ToString();
                            hdfInvoicePK.Value = invoiceHeaderObj.IVH_PK.ToString();                         
                            hdfInvoiceNo.Value = string.IsNullOrEmpty(invoiceHeaderObj.IVH_NO) ? string.Empty : invoiceHeaderObj.IVH_NO;
                            lblInvoiceNo.Text = string.IsNullOrEmpty(invoiceHeaderObj.IVH_NO) ? Resources.ErpRes.Draft : invoiceHeaderObj.IVH_NO;

                            txtInvoiceDate.Text = invoiceHeaderObj.IVH_DATE;                            
                            txtRemarks.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_REMARKS);
                            txtInvoiceType.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_TYPE_TEXT);
                            txtCurrency.Text = ERP.Utilities.CommonFunctions.GetDecodedString(invoiceHeaderObj.IVH_CURRENCY_TEXT);
                            hdfCurrency.Value = invoiceHeaderObj.IVH_CURRENCY.ToString();
                            //Exchange rate to Textbox.//Not allowed to edit exchange rate while currency same as base currency                            
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
                                          
                            double RemainingOtherChrge = 0;
                            RemainingOtherChrge = invoiceHeaderObj.IVH_SHIP_CHARGE - invoiceHeaderObj.IVH_SHIP_CHARGE_INV;
                            hdfOtherchargePO.Value = RemainingOtherChrge.ToString(hdfCurrencyFormat.Value);
                            txtShipping.Text = invoiceHeaderObj.IVH_SHIP_CHARGE.ToString(hdfCurrencyFormat.Value);//while deduction alredy taken oc will show in txtDeductOtherCharges
                            txtPriceAdj.Text = invoiceHeaderObj.IVH_AMOUNT_ADJUST.ToString(hdfCurrencyFormat.Value);
                            txtHdrNetTotal.Text = invoiceHeaderObj.IVH_AMOUNT_NET_TC.ToString(hdfCurrencyFormat.Value);

                            if (invoiceHeaderObj.DeductionDetails != null && invoiceHeaderObj.DeductionDetails.Count > 0)
                            {
                                //while editing invoice with multiple advances allocated(with discount),the Discount Adjusted texbox getting wrong amount.                               
                                txtDiscDeducted.Text = txtDiscDeducted.ToolTip = invoiceHeaderObj.DeductionDetails.Sum(p => p.VAD_DISC_AMOUNT).ToString(hdfCurrencyFormat.Value); 
                                totalAllocatedDiscount = Convert.ToDecimal(txtDiscDeducted.Text);
                            }

                            txtSupplierInvNO.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_VENDOR_INV_NO);
                            txtCreditDays.Text = invoiceHeaderObj.IVH_CREDIT_DAYS;
                            txtTransport.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_TRANSPORT);
                            txtDeductOtherCharges.Text = string.IsNullOrEmpty(invoiceHeaderObj.IVH_SHIP_CHARGE_DED) ? "0.00" : Convert.ToDouble(invoiceHeaderObj.IVH_SHIP_CHARGE_DED).ToString(hdfCurrencyFormat.Value);
                            txtSupplierInvDate.Text = string.IsNullOrEmpty(invoiceHeaderObj.IVH_VENDOR_INV_DATE) ? string.Empty : Convert.ToDateTime(invoiceHeaderObj.IVH_VENDOR_INV_DATE).ToString(Resources.Constants.DateFormatShort);
                            ddlCompany.SelectedValue = invoiceHeaderObj.IVH_COMPANY.ToString();
                            chkOriginalinvoice.Checked = invoiceHeaderObj.IVH_ORGINAL_RCVD == (byte)1;
                           
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

                            GetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                            SetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                            if (CurrPK > 0)
                            {                               
                                if (ddlAddressType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(invoiceHeaderObj.IVH_VENDOR_CONTACT) > 0)
                                        ddlAddressType.SelectedValue = invoiceHeaderObj.IVH_VENDOR_CONTACT.ToString();
                                }
                                SetBranchCodeVisibility();

                                txtVatTaxId.Text = string.IsNullOrEmpty(invoiceHeaderObj.IVH_TAX_ID) ? string.Empty : HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_TAX_ID.ToString());
                                txtBranchCode.Text = string.IsNullOrEmpty(invoiceHeaderObj.IVH_BRANCH_TEXT) ? string.Empty : invoiceHeaderObj.IVH_BRANCH_TEXT.ToString();
                                hdfVendorContactType.Value = invoiceHeaderObj.IVH_BRANCH_TYPE.ToString();
                                txtInvoiceDueDate.Text = invoiceHeaderObj.IVH_DATE_PAY_BY;
                                ModifiedDatePnl.Visible = true;
                                LastModifiedTime = invoiceHeaderObj.LAST_MOD_DT;
                                lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                            }
                            else
                            {
                                DateTime supplierInvDate;
                                if (DateTime.TryParse(txtSupplierInvDate.Text.Trim(), out supplierInvDate))
                                {
                                    int creditDays = string.IsNullOrEmpty(txtCreditDays.Text.Trim()) ? 0 : Convert.ToInt32(txtCreditDays.Text.Trim());
                                    DateTime dueDate = supplierInvDate.AddDays(creditDays);
                                    txtInvoiceDueDate.Text = dueDate.ToString(Resources.Constants.DateFormatShort);
                                }
                            }
                        }
                        break; 
                    #endregion
                    #region UPLOADEDFILES
                    case ControlsEnum.UPLOADEDFILES:
                        CurrPK = invoiceHeaderObj.IVH_PK;
                        POUploadList = invoiceHeaderObj.FileList;
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
                    }
                    break;
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
                    if (IsCustomTaxEnabled || Convert.ToInt16(hdfTaxCategory.Value) == ((int)TaxType.Discount) || Convert.ToInt16(hdfTaxCategory.Value) == ((int)TaxType.Shipping))
                        ddlPopupTaxType.Items.Add(new ListItem(Resources.Report.Custom, CommonConstants.SELECTVAL));
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
                    #region INVOICELIST
                    case ControlsEnum.INVOICELIST:
                    #region Paging Properties                       
                        pageSize=Convert.ToInt32(GetLocalResourceObject("PageSize_InvList"));
                        rowCount = dsPageData.Tables[0].Rows.Count > 0 ? Convert.ToInt32(dsPageData.Tables[0].Rows[0][0].ToString()) : 0;                       
                        uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                      (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                      (rowCount / pageSize) + 1;
                        PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex); 
	               #endregion
                    if (dtInvoiceList != null)
                    {
                        GetFieldValues(ControlsEnum.FILLWORKFLOWSTATUS);
                        grdInvoiceList.PageIndex = Convert.ToInt32(PageIndex);
                        grdInvoiceList.DataSource = dtInvoiceList.DefaultView;
                        grdInvoiceList.DataBind();

                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                        //For Setting/Resetting Colour of a selected InvoiceNo
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                    }
                    else
                    {
                        grdInvoiceList.DataSource = null;
                        grdInvoiceList.DataBind();
                        uclPaging.Visible = false;
                    }
                        break;
                    #endregion
                    #region POINVDETAIL
                    case ControlsEnum.POINVDETAIL:
                        if (POInvoiceHeaderSession != null)
                        {
                            List<DirectPOInvoiceDetails> poInvoiceTList;
                            poInvoiceDetailsList = new List<DirectPOInvoiceDetails>();

                            poInvoiceTList = new List<DirectPOInvoiceDetails>();
                            poInvoiceTList = POInvoiceHeaderSession.OrderDetail.ToList();
                            poInvoiceDetailsList.AddRange(poInvoiceTList);

                            if (poInvoiceDetailsList != null)
                            {
                                grdInvoice.DataSource = poInvoiceDetailsList;
                                grdInvoice.DataBind();
                            }
                            else
                            {
                                grdInvoice.DataSource = null;
                                grdInvoice.DataBind();
                            }
                        }
                        break;
                    #endregion
                    #region TAXPOPUPGRID
                    case ControlsEnum.TAXPOPUPGRID:
                        if (IsHeaderTax)
                        {                           
                            taxHdrList = TempPOInvoiceHeaderSession.TaxHdr.Where(tax => tax.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                        }
                        else
                        {                          
                            poDtlObj = TempPOInvoiceHeaderSession.OrderDetail.SingleOrDefault(rfq => rfq.VID_PO_DTL == PoDetailsPK.ToString());
                            if (poDtlObj != null)
                            {
                                taxHdrList = poDtlObj.TaxDtl.Where(tax => tax.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                            }
                        }
                        grdTaxDetails.DataSource = taxHdrList;
                        grdTaxDetails.DataBind();
                        break; 
                    #endregion
                    #region DEDUCTIONPOPUPGRID
                    case ControlsEnum.DEDUCTIONPOPUPGRID:
                        deductionDtlList = new List<DirectPOAdvDeductionDetails>();
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
                            }
                        }
                        else
                        {
                            grdDeduction.DataSource = deductionDtlList;
                            grdDeduction.DataBind();
                        }
                        break; 
                    #endregion
                    #region UPLOADEDFILES
                    case ControlsEnum.UPLOADEDFILES:
                        if (POUploadList != null)
                        {
                            grdUploads.DataSource = POUploadList;
                            grdUploads.DataBind();
                        }
                        break; 
                    #endregion
                    #region OTHERCHARGELIST
                    case ControlsEnum.OTHERCHARGELIST:
                        POTotalOtherAmount = 0;
                        POTotalInvOtherAmount = 0;
                        POTotalBalanceOtherAmount = 0;
                        if (POInvoiceHeaderSession != null)
                        {
                            poOtherChargeList = new List<DirectPOInvoiceMappingDetails>();
                            poOtherChargeList = POInvoiceHeaderSession.POMappingDetails;
                            POTotalOtherAmount = poOtherChargeList.Sum(toa => toa.PO_OTHER_AMOUNT);
                            POTotalInvOtherAmount = poOtherChargeList.Sum(tioa => tioa.PO_OTHER_AMOUNT_INVOICED);
                            POTotalBalanceOtherAmount = POTotalOtherAmount - POTotalInvOtherAmount;
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

                    #region GRNQTYSPLIT
                    case ControlsEnum.GRNQTYSPLIT:
                        POTotalGRNQty = 0;
                        POTotalGRNInvdQty = 0;
                        POTotalGRNBalance = 0;
                        if (POInvoiceHeaderSession != null)
                        {
                            grnList = new List<DirectGRNQTYDetails>();
                            invoiceHeaderObj = POInvoiceHeaderSession;
                            poDtlObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.VID_PO) == SelectedPOPK);
                            if (poDtlObj != null)
                            {
                                grnList = poDtlObj.GRNDtl.ToList();
                            }
                            POTotalGRNQty = grnList.Sum(grn => grn.GRD_QTY_APPROVED);
                            POTotalGRNInvdQty = grnList.Sum(bal => bal.GRN_QTY_INVOICED);
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
                #region TAXPOPUPGRID
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
                    SelectedPOPK = 0;
                    PoDetailsPK = 0;
                    PoPK = 0;
                    hdfTaxCategory.Value = string.Empty;
                    break; 
                #endregion
                #region POINVHEADER
                case ControlsEnum.POINVHEADER:
                    POInvoiceHeaderSession = null;
                    TempInvoiceHeaderTemp = null;
                    TempPOInvoiceHeaderSession = null;
                    dtPendingPOList = null;
                    CurrPK = 0;
                    IsDeleted = false;
                    AST_DOC_MODE.Value = GetDOCMODE();
                    AST_CODE.Value = ApplicationType.TPI;
                    hdfInvoiceNo.Value = string.Empty;
                    lblInvoiceNo.Text = Resources.ErpRes.Draft;
                    hdfAppType.Value = ApplicationType.TPI;
                    hdfAppSubType.Value = string.Empty;                    

                    txtInvoiceDate.Text = string.Empty;
                    txtVendorHd.Text = string.Empty;
                    hdfVendorHd.Value = string.Empty;
                    txtSupplierInvNO.Text = string.Empty;                  
                    txtSupplierInvDate.Text = string.Empty;              
                    txtTransport.Text =string.Empty;
                    txtCreditDays.Text = string.Empty; 
                    txtInvoiceDueDate.Text = string.Empty;
                    ddlAddressType.ClearSelection();
                    txtBranchCode.Text = string.Empty;
                    txtVatTaxId.Text = string.Empty;
                    txtInvoiceType.Text = string.Empty;
                    txtDeclarationNO.Text = string.Empty;
                    txtCurrency.Text = string.Empty;
                    hdfCurrency.Value = string.Empty;
                    txtExchangeRate.Text = string.Empty;

                  
                    grdPendingPoList.DataSource = null;
                    grdPendingPoList.DataBind();
                    grdInvoice.DataSource = null;
                    grdInvoice.DataBind();
                    grdUploads.DataSource = null;
                    grdUploads.DataBind();

                    txtHdrDiscount.Text = string.Empty;
                    txtHdrTotal.Text = string.Empty;
                    txtHdrBalBeforeVat.Text = string.Empty;
                    txtShipping.Text = string.Empty;
                    txtHdrTax.Text = string.Empty;
                    txtPriceAdj.Text = string.Empty;                  
                    txtHdrNetTotal.Text = string.Empty;                
                    txtRemarks.Text = string.Empty;             
                    txtHdrDeduction.Text = string.Empty;
                    txtDiscDeducted.Text = string.Empty;  
               
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
                #region INVOICELIST
                case ControlsEnum.INVOICELIST:
                    CurrPK = 0;
                    CurrPOPK = 0;
                    hasValidRate = false;
                    txtInvoiceNumber.Text = "Select/Type";
                    txtVendorSearch.Text = "Select/Type";
                    hdfIVHPK.Value = "";
                    txtpoNo.Text = string.Empty;
                    hdfVendorSearch.Value = "";
                    chkOriginalinvoice.Checked = false;
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
                    txtVendorHd.Text = string.Empty;
                    hdfVendorHd.Value = CommonConstants.SELECTVAL;
                    ResetForm(ControlsEnum.ADDITEM);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ResetRadioButtonSelection", "ResetSelection();", true);
                    break; 
                #endregion
                #region ADDITEM
                case ControlsEnum.ADDITEM:
                    anchorFile.Visible = false;
                    vrfFileUpload.Enabled = true;
                    CurrSlNo = 0;
                    anchorFile.Attributes.Remove("onclick");
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
            HiddenField hdfPODetailPK;

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
                        }
                        if (txtAmount != null)
                        {
                            txtAmount.Text = txtAmount.ToolTip = Math.Round((rate * quantity), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                            txtDiscount = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtDiscount") as TextBox);
                            txtTax = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtTax") as TextBox);
                            txtTotal = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtTotal") as TextBox);

                            hdfRRDPK = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("hdfInvoiceDtlPK") as HiddenField);
                            hdfItemPK = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                            hdfPOPK = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("hdfPOPK") as HiddenField);
                            hdfPODetailPK = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("hdfPODetailPK") as HiddenField);
                            if (hdfRRDPK != null && hdfItemPK != null && hdfPOPK != null)
                            {
                                POInvoicePK = string.IsNullOrEmpty(hdfRRDPK.Value) ? 0 : Convert.ToInt32(hdfRRDPK.Value);
                                SelectedItemPK = string.IsNullOrEmpty(hdfItemPK.Value) ? 0 : Convert.ToInt32(hdfItemPK.Value);
                                PoPK = string.IsNullOrEmpty(hdfPOPK.Value) ? 0 : Convert.ToInt32(hdfPOPK.Value);
                                PoDetailsPK = string.IsNullOrEmpty(hdfPODetailPK.Value) ? 0 : Convert.ToInt32(hdfPODetailPK.Value);
                                SetDetailTax(txtAmount, txtDiscount, txtTax, txtTotal);
                            }
                        }
                    }
                    else
                    {
                        
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
                        hdfPOPK = (gvr.FindControl("hdfPOPK") as HiddenField);
                        hdfPODetailPK = (gvr.FindControl("hdfPODetailPK") as HiddenField);
                        POInvoicePK = Convert.ToInt32(hdfRRDPK.Value);
                        SelectedItemPK = Convert.ToInt32(hdfItemPK.Value);
                        SelectedPOPK = Convert.ToInt32(hdfPOPK.Value);
                        PoPK = Convert.ToInt32(hdfPOPK.Value);
                        PoDetailsPK = Convert.ToInt32(hdfPODetailPK.Value);

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
                        hdfPOPK = (gvr.FindControl("hdfPOPK") as HiddenField);
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
                        poInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.VID_PO) == PoPK && Convert.ToInt32(rfq.VID_PO_DTL) == PoDetailsPK);
                        if (poInvoiceDetailsObj != null)
                        {
                            var discDetail = poInvoiceDetailsObj.TaxDtl.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Discount));
                            foreach (DirectPOInvoiceTaxHdr rfqTaxHdrObj in discDetail)
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
                                        rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                    }
                                }
                            }


                            discount = poInvoiceDetailsObj.TaxDtl.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(rfq => rfq.VTL_TAX_AMT);                            
                            discount = Convert.ToDouble(ERP.Utilities.CommonFunctions.DoubleFormatRound(discount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1).ToString(hdfCurrencyFormat.Value));
                            netAmount = amount - discount;
                            txtDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);

                            var taxDetail = poInvoiceDetailsObj.TaxDtl.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Tax));
                            foreach (DirectPOInvoiceTaxHdr rfqTaxHdrObj in taxDetail)
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

                            }
                            itmTax = poInvoiceDetailsObj.TaxDtl.ToList().Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(rfq => rfq.VTL_TAX_AMT);
                            itmTax = Convert.ToDouble(ERP.Utilities.CommonFunctions.DoubleFormatRound(itmTax, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1).ToString(hdfCurrencyFormat.Value));
                            txtTax.ToolTip = txtTax.Text = itmTax.ToString(hdfCurrencyFormat.Value);
                            poInvoiceDetailsObj.VID_AMOUNT = amount;
                            poInvoiceDetailsObj.VID_DISCOUNT = discount;
                            poInvoiceDetailsObj.VID_TAX = itmTax;
                            poInvoiceDetailsObj.VID_NET_AMOUNT = (amount - discount + itmTax);
                            poInvoiceDetailsObj.VID_NET_AMOUNT = Convert.ToDouble(ERP.Utilities.CommonFunctions.DoubleFormatRound(poInvoiceDetailsObj.VID_NET_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1).ToString(hdfCurrencyFormat.Value));
                            txtTotal.ToolTip = txtTotal.Text = poInvoiceDetailsObj.VID_NET_AMOUNT.ToString(hdfCurrencyFormat.Value);
                            POInvoiceHeaderSession = invoiceHeaderObj;
                        }
                    }
                    return true;
                }
                else
                {                    
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

                foreach (DirectPOInvoiceTaxHdr rfqTaxHdrObj in discHeader)
                {
                    string taxFormula = rfqTaxHdrObj.VTL_TAX_FORMULA;
                    if (!string.IsNullOrEmpty(taxFormula))
                    {

                        taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                        rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);

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
                        poInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.VID_PO) == SelectedPOPK);
                        if (poInvoiceDetailsObj != null)
                        {
                            var discDetail = poInvoiceDetailsObj.TaxDtl.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Discount));
                            foreach (DirectPOInvoiceTaxHdr rfqTaxHdrObj in discDetail)
                            {
                                string taxFormula = rfqTaxHdrObj.VTL_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                                    rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                }
                            }
                            discount = poInvoiceDetailsObj.TaxDtl.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(rfq => rfq.VTL_TAX_AMT);
                            netAmount = amount - discount;                           

                            var taxDetail = poInvoiceDetailsObj.TaxDtl.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Tax));
                            foreach (DirectPOInvoiceTaxHdr rfqTaxHdrObj in taxDetail)
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
                            itmTax = poInvoiceDetailsObj.TaxDtl.ToList().Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(rfq => rfq.VTL_TAX_AMT);
                            poInvoiceDetailsObj.VID_AMOUNT = amount;
                            poInvoiceDetailsObj.VID_DISCOUNT = discount;
                            poInvoiceDetailsObj.VID_TAX = itmTax;
                            poInvoiceDetailsObj.VID_NET_AMOUNT = (amount - discount + itmTax);
                            POInvoiceHeaderSession = invoiceHeaderObj;
                        }
                    }
                    return true;
                }
                else
                {                    
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

            List<DirectPOInvoiceDetails> soInvoiceDetailsTaxPayableLst = new List<DirectPOInvoiceDetails>();
            DirectPOInvoiceHeader invoiceHeaderObjTaxPayable;
            List<DirectPOInvoiceTaxHdr> LstTaxPayable = new List<DirectPOInvoiceTaxHdr>();

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
                        foreach (DirectPOInvoiceTaxHdr rfqTaxHdrObj in taxDetail)
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
                    foreach (DirectPOInvoiceTaxHdr rfqTaxHdrObj in HeaderTax)
                    {
                        rfqTaxHdrObj.VTL_TAX_VID_AMOUNT = taxapplyAmount;
                        LstTaxPayable.Add(rfqTaxHdrObj);
                    }
                }

            }

            if (LstTaxPayable.Count > 0)
            {
                var groupedTaxPayableList = LstTaxPayable.GroupBy(f => f.VTL_TAX)
                    .Select(grp => new DirectPOInvoiceTaxHdr
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
                  
                    txtHdrTotal.Text = ((subTotal + adjamt) - hdrDiscount).ToString(hdfCurrencyFormat.Value);
                    if (IsAdvInvHasTax)
                    {
                        decimal hdrDeduction = string.IsNullOrEmpty(txtHdrDeduction.Text) ? 0 : Convert.ToDecimal(txtHdrDeduction.Text);                      
                        txtHdrBalBeforeVat.Text = ((((subTotal + adjamt) - hdrDiscount) - hdrDeduction) - totalAllocatedDiscount).ToString(hdfCurrencyFormat.Value);
                    }
                    else
                    {
                        txtHdrBalBeforeVat.Text = (((subTotal + adjamt) - hdrDiscount)).ToString(hdfCurrencyFormat.Value);
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
                foreach (DirectPOInvoiceTaxHdr rfqTaxHdrObj in discHeader)
                {
                    isHaveDiscount = 1;
                    string taxFormula = rfqTaxHdrObj.VTL_TAX_FORMULA;
                    if (!string.IsNullOrEmpty(taxFormula))
                    {                       
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
                            rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                        }
                    }
                    else//In case of Custom tax & Discount it doesnot have formula. So we create a formula . 
                    {
                        //Formula :InvoiceNowDiscount=(TotalPODiscount/SubTotalPOAmount)*InvoiceNowAmount
                        if (TempInvoiceHeaderTemp != null)
                        {                           
                            if (Convert.ToDouble(TempInvoiceHeaderTemp.POH_DISCOUNT_TC) > 0)
                            {
                                double invoicenowDiscount = 0, TotalPODiscount = 0;
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
              
                invoiceHeaderObj.IVH_DISCOUNT_TC = discount;
               
                if ((TotalHDRDiscount > 0) || (isDeducted <= 0))
                {
                    txtHdrDiscount.ToolTip = txtHdrDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);
                    txtHdrDiscount.ToolTip = txtHdrDiscount.Text = ((Convert.ToDecimal(txtHdrDiscount.Text) - TotalHDRDiscount) < 0 ? 0 : (Convert.ToDecimal(txtHdrDiscount.Text) - TotalHDRDiscount)).ToString(hdfCurrencyFormat.Value);
                    if (isHaveDiscount == 1)
                    {
                        List<DirectPOInvoiceTaxHdr> LstHeaderDiscount = new List<DirectPOInvoiceTaxHdr>();
                        LstHeaderDiscount = invoiceHeaderObj.TaxHdr.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Discount)).ToList();
                        if (LstHeaderDiscount != null && LstHeaderDiscount.Count > 0)
                        {                           
                            if (discount > 0)
                            {
                                foreach (DirectPOInvoiceTaxHdr disc in LstHeaderDiscount)
                                {                                   
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
                decimal hdrDeduction = string.IsNullOrEmpty(txtHdrDeduction.Text) ? 0 : Convert.ToDecimal(txtHdrDeduction.Text);
                if (IsAdvInvHasTax)
                {
                    txtHdrBalBeforeVat.Text = ((((subTotal - Convert.ToDecimal(txtHdrDiscount.Text)) - hdrDeduction) - totalAllocatedDiscount)).ToString(hdfCurrencyFormat.Value);
                    txtHdrTotal.Text = (subTotal - Convert.ToDecimal(txtHdrDiscount.Text)).ToString(hdfCurrencyFormat.Value);
                }
                else
                {
                    txtHdrBalBeforeVat.Text = txtHdrTotal.Text = (subTotal - Convert.ToDecimal(txtHdrDiscount.Text)).ToString(hdfCurrencyFormat.Value);
                }
                double.TryParse(txtHdrBalBeforeVat.Text, out balBeforeVat);
                amount = balBeforeVat;
              
                #region Other Charge

                // To handle Other charges against multiple PO   
                // Total Other Charge = SUM(All PO other charge amount) - SUM(All PO invoiced other charge amount)

                if (CurrPK > 0) // Edit mode
                {
                    shipping = Convert.ToDouble(invoiceHeaderObj.POMappingDetails.Sum(chrg => chrg.IVM_OTHER_AMOUNT));
                }
                else // New mode
                {
                    if (hdfOtherCharge.Value == "1")
                    {
                        shipping = Convert.ToDouble(invoiceHeaderObj.POMappingDetails.Sum(chrg => chrg.IVM_OTHER_AMOUNT));
                    }
                    else
                    {
                        shipping = Convert.ToDouble(invoiceHeaderObj.POMappingDetails.Sum(chrg => chrg.PO_OTHER_AMOUNT)) - Convert.ToDouble(invoiceHeaderObj.POMappingDetails.Sum(chrg => chrg.PO_OTHER_AMOUNT_INVOICED));
                    }
                }
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
                    _totalTax = invoiceHeaderObj.IVH_TAX_TC; 
                    _totalAmount = _PrevtotalAmount; 
                    _totalCurrAmount = string.IsNullOrEmpty(txtHdrTotal.Text) ? 0 : Convert.ToDouble(txtHdrTotal.Text);
                    _totalCurrTax = (_totalTax / _totalAmount) * _totalCurrAmount;
                }
                ////----End New Section for proportionate tax -------////

                var taxHeader = invoiceHeaderObj.TaxHdr.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Tax));
                foreach (DirectPOInvoiceTaxHdr rfqTaxHdrObj in taxHeader)
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
                                taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                                if (amount >= 0)
                                    rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
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
                                                                     .Sum(rfq => rfq.VTL_TAX_AMT);
                txtHdrTax.ToolTip = txtHdrTax.Text = invoiceHeaderObj.IVH_TAX_TC.ToString(hdfCurrencyFormat.Value);
              
                double.TryParse(txtPriceAdj.Text, out adjust);
                invoiceHeaderObj.IVH_AMOUNT_ADJUST = adjust;
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

        private void PriceAdjustConfiguration()
        {
            DataTable dt = new DataTable();
            dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("PURCHASE INVOICE SETTINGS", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfPriceAdjPercentage.Value = dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "ADJ TOLERANCE")["ACF_VALUE"].ToString();
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
            RadioButton rbtn;
            foreach (GridViewRow item in grdInvoiceList.Rows)
            {
                
                rbtn = (RadioButton)item.FindControl("rbtSelect");                            
                          
                SelectionInfo objSaleOrderInfo = new SelectionInfo();
                objSaleOrderInfo.chkChecked = false;
                objSaleOrderInfo.InvoicePK = Convert.ToInt32(grdInvoiceList.DataKeys[item.RowIndex].Value.ToString());
                if (rbtn.Checked)
                {
                    objSaleOrderInfo.chkChecked = true;
                    objSaleOrderInfo.VendorPK = Convert.ToInt32(((HiddenField)item.FindControl("hdfVendorPK")).Value);//E
                    objSaleOrderInfo.CurrencyPK = Convert.ToInt32(((HiddenField)item.FindControl("hdfPOCurrency")).Value);//E
                    objSaleOrderInfo.ApprovedStatus = Convert.ToInt32(((HiddenField)item.FindControl("hdfApproved")).Value);
                    objSaleOrderInfo.IsPosted = Convert.ToBoolean(((HiddenField)item.FindControl("hdfPosted")).Value);
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
                        ((RadioButton)item.FindControl("rbtSelect")).Checked = true;

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
            try
            {
                #region Variables
                int? result;
                result = 0;
                
                DirectPOInvoiceTaxHdr tempInvTaxSplitObj = null;
                HiddenField hdfInvoiceDtlPK;
                HiddenField hdfItemPK;
                HiddenField hdfPOPK;
                HiddenField hdfPODetailPK;
                HiddenField hdfInOpeningInv;
                TextBox txtAmount;
                TextBox txtDiscount;
                TextBox txtSubTotal;
                TextBox txtSubTotalFooterAmt;
                TextBox txtAdjustAmount;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                bool bIsChecked = false;
                TextBox WrkfComments;
                int selectedItemPK;
                double totalAmt;
                double currentTotal;
                double taxAmt;
                double subTotalAmt = 0;
                bool isValidDisc = true;
                bool isContinue;
                FileInfo tempFileInfoObj;
                string savePath = string.Empty;

                long? DummyResult;
                DummyResult = 0;

                string poPK;
                string grnPK;
                RadioButton rbtn;
                #endregion
                #region commonActions settings
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
                    else if (((TextBox)sender).ID == "txtExchangeRate")
                    {
                        commonActions = ActionsEnum.CHANGEEXRATE;
                    }
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
                #endregion
                switch (commonActions)
                {                   
                    #region NEW
                    case ActionsEnum.NEW:
                        ResetForm(ControlsEnum.POINVHEADER);                         
                        FillProcessID(1);
                        EntryStatus = EntryStatus.NEWMODE;
                        TotalPages = 0;
                        uclPOPaging.CurrentPage = 1;
                        hdfIsPendingPOVisible.Value = "0";
                        PageIndexPO = CommonConstants.SELECT_VALUE_ONE;
                        SetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                        SetFieldValues(ControlsEnum.PENDINGPOLIST);
                        break;
                    #endregion
                    #region ADD TO LIST
                    case ActionsEnum.ADDTOLIST:                   
                        PoHeaderObj = new DirectPOHeaderBO();
                        PoHeaderObj.POList = new List<DirectPOHeaderListBO>();
                        List<DirectPOHeaderListBO> objItemList = new List<DirectPOHeaderListBO>();
                        DirectPOHeaderListBO objPoList;
                        HiddenField hdfPOPk;
                        HiddenField hdfPOVendor;
                        HiddenField hdfPOType;
                        HiddenField hdfPOCurrency;
                        #region grdPendingPoList
		                foreach (GridViewRow grdrow in grdPendingPoList.Rows)
                        {
                            CheckBox chkPoPendSelect = (CheckBox)grdrow.FindControl("chkPoPendSelect");
                            if (chkPoPendSelect.Checked)
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
                                if (objItemList != null && (objItemList.Where(r => r.POH_VENDOR != Convert.ToInt32(hdfPOVendor.Value)).Count() > 0
                                                            || objItemList.Where(r => r.POH_TYPE != Convert.ToInt32(hdfPOType.Value)).Count() > 0
                                                            || objItemList.Where(r => r.POH_CURRENCY != Convert.ToInt32(hdfPOCurrency.Value)).Count() > 0
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
                        //Avoid already added PO.No need to get that PO details again
                        if (objItemList != null && objItemList.Count > 0 && POInvoiceHeaderSession != null && POInvoiceHeaderSession.OrderDetail != null)
                        {                           
                            List<string> objPoPkList = POInvoiceHeaderSession.OrderDetail.Select(r => r.VID_PO).Distinct().ToList();
                            PoHeaderObj.POList = objItemList.Where(r => !objPoPkList.Contains(r.POH_PK.ToString())).ToList();                          
                        }
                        GetFieldValues(ControlsEnum.POINVHEADER);
                        if (invoiceHeaderObj!=null && invoiceHeaderObj.IVH_TYPE == ((byte)PurchaseType.Local).ToString())
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
                            POInvoiceHeaderSession.IVH_AMOUNT_NET_TC = 0;
                            POInvoiceHeaderSession.IVH_AMOUNT_NET_BC = 0;
                            #region Setting VID_QTY_INVOICED
                            foreach (DirectPOInvoiceDetails dtl in POInvoiceHeaderSession.OrderDetail)
                            {
                                dtl.VID_QTY_INVOICED = 0;
                                dtl.VID_AMOUNT = 0;
                                dtl.VID_TAX = 0;
                                dtl.VID_DISCOUNT = 0;
                                dtl.VID_NET_AMOUNT = 0;

                                if (Convert.ToBoolean(dtl.VID_HAS_GRN))
                                {
                                    //For Avoiding Negative Quantity                                    
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
                        } 
	                  #endregion
                        SetFieldValues(ControlsEnum.POINVHEADER);
                        SetFieldValues(ControlsEnum.POINVDETAIL);
                        SetFieldValues(ControlsEnum.UPLOADEDFILES);
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
                        if (hdfIsTaxPayable.Value.ToString() == "1") //Settings of TaxPayableDiv
                        {
                            SetTaxPayableDiv();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                        hdfIsPendingPOVisible.Value = "0";
                        SetFieldValues(ControlsEnum.PENDINGPOLIST);
                        break;
                    #endregion
                    #region Save,Edit,View,Delete,SaveSubmit,Submit,WkfSubmit,EditForCancel,DeleteSubmit
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        { 
                            isContinue = true;
                            #region Head Office/Branch selected or not
                            if (ddlAddressType.Items.Count <= 0)
                            {
                                isContinue = false;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ReqType").ToString()) + "');", true);//Please Select Head Office/Branch
                                return;
                            } 
                            #endregion
                            #region Checking:Deduction allocation
                            if ((hdfSaveWithoutAllocation.Value == "0") || (hdfSaveWithoutAllocation.Value == ""))
                            {
                                TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
                                deductionDtlList = new List<DirectPOAdvDeductionDetails>();
                                deductionDtlList = TempPOInvoiceHeaderSession.DeductionDetails.ToList();
                                if (IsAdvInvHasTax)
                                {
                                    if (Convert.ToDouble(txtHdrDeduction.Text) <= 0 && (deductionDtlList != null && deductionDtlList.Count > 0))
                                    {
                                        isContinue = false;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowSaveWithoutAllocationConfirm('" + (sender as Button).ID + "');", true);//Do you want to save without allocating the deduction
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
                            #endregion
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
                                        if (Convert.ToDecimal(txtInvNow.Text) > RemainingQty)
                                        {
                                            isContinue = false;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InvoiceNowExceeds", "$(document).ready(function(){ShowInvoiceNowQtyExceeds(1);});", true);
                                            break;

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
                                invoiceHeaderObj = new DirectPOInvoiceHeader();
                                invoiceHeaderObj = (DirectPOInvoiceHeader)SetUIValuesToObject(ControlsEnum.POINVHEADER);
                                if (invoiceHeaderObj.IVH_AMOUNT_TC > 0)
                                {
                                    if (hasValidRate)
                                    {
                                        if (invoiceHeaderObj != null && invoiceHeaderObj.OrderDetail != null)
                                        {
                                            invoiceHeaderObj.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                            string xmlDoc = CommonFunctions.XmlSerialize<DirectPOInvoiceHeader>(invoiceHeaderObj);                                        
                                            bool isCont = true;
                                            #region Checking: Is have journalize entry against this invoice
                                            if (Convert.ToInt16(hdfCrDrStatus.Value) > 0)
                                            {
                                                if (Convert.ToInt16(hdfCrDrWKFStatus.Value) == 0)
                                                {
                                                    isCont = true;
                                                }
                                                else
                                                {
                                                    isCont = false;
                                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Already_Journalized").ToString();//There is a journalize entry against this invoice
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);

                                                    EntryStatus = EntryStatus.LISTMODE;
                                                    ResetForm(ControlsEnum.INVOICELIST);
                                                    GetFieldValues(ControlsEnum.INVOICELIST);
                                                    SetFieldValues(ControlsEnum.INVOICELIST);
                                                    break;
                                                }
                                            } 
                                            #endregion
                                            if (isCont)
                                            {                                             
                                                string invNumber = string.Empty;
                                                result = BusinessLogic.POInvoicing.POInvoiceBL.SavePOInvoiceTradingWkf(xmlDoc, out invNumber);//SPFIN_INVOICE_VND_TRADING_WKF_SAVE
                                            }
                                            if (result > 0) // Success !  redirect to listing page
                                            {
                                                #region  ATTACHMENT SAVE
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
                                                #endregion
                                                #region Update dummy entry while modify invoice after approval
                                                if (Convert.ToInt16(hdfCrDrStatus.Value) == (int)DbStatus.APPROVED)
                                                {
                                                    FinTrxService finTrxServiceClient;
                                                    finTrxServiceClient = new FinTrxService();
                                                    string refType = "";
                                                    refType = POGroup == POInvoiceGroup.Goods ? ApplicationType.TPIJ : POGroup == POInvoiceGroup.Services ? ApplicationType.TPSIJ : ApplicationType.EITJ;
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
                                                #endregion  
                                                #region Show Save Message and redired to listing page            
                                                string invoiceNo = string.Empty;
                                                if (string.IsNullOrEmpty(lblInvoiceNo.Text.Trim()) || lblInvoiceNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                                                {
                                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Saved_Success").ToString();
                                                }
                                                else
                                                {
                                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                                                    invoiceNo = lblInvoiceNo.Text.Trim();
                                                    object[] args = new object[2];
                                                    args[0] = Resources.PageNameRes.POInvoice;
                                                    args[1] = invoiceNo;
                                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                                                }
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                EntryStatus = EntryStatus.LISTMODE;
                                                ResetForm(ControlsEnum.INVOICELIST);
                                                GetFieldValues(ControlsEnum.INVOICELIST);
                                                SetFieldValues(ControlsEnum.INVOICELIST);
                                                POInvoiceHeaderSession = null; 
                                                #endregion
                                            }
                                            else if (result == -6)
                                            {                                                
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "VendorInvoiceDup", "$(document).ready(function(){ShowDuplicateVendorInvNoContinue(1);});", true);
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
                            }
                        }
                        break;
                    #endregion
                    #region  EDIT/VIEW/INVOICEDETAIL
                    case ActionsEnum.EDIT:
                    case ActionsEnum.VIEW:
                    case ActionsEnum.INVOICEDETAIL:
                        ResetForm(ControlsEnum.POINVHEADER);
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            #region grdInvoiceList
                            HiddenField hdfDept;
                            int dept;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
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
                                Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                                Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).Text;
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1)
                                {
                                    btnEditforCancel.Visible = false;
                                    btnSave.Visible = false;
                                    btnEdit.Visible = false;
                                    IsDeleted = true;
                                }
                                else
                                {
                                    btnSave.Visible = true;
                                    btnEdit.Visible = true;
                                    IsDeleted = false;
                                }
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                break;
                            } 
                            #endregion
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
                            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
                            hdfApplyTax.Value = "1";// No need to create formula for Tax amount in TAX POPUP. 
                            SetDetailTax(null);
                            SetHdrTax();
                            if (grdInvoice.Rows.Count > 0)
                            {
                                SetSubTotal();
                            }                           
                            #region Settings of TaxPayableDiv
                            if (hdfIsTaxPayable.Value.ToString() == "1")
                            {
                                SetTaxPayableDiv();
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true); 
                            #endregion

                            uclPOPaging.CurrentPage = 1;
                            PageIndexPO = CommonConstants.SELECT_VALUE_ONE;
                            GetFieldValues(ControlsEnum.PENDINGPOLIST);
                            SetFieldValues(ControlsEnum.PENDINGPOLIST);
                            hdfIsPendingPOVisible.Value = "0";
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
                            result = BusinessLogic.POInvoicing.POInvoiceBL.DeletePOInvoiceTradingDetails(CurrPK, LastModifiedTime, ApplicationType.TPI, currentUser.PKUser.ToString());
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Invoice Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.POInvoice);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                FillProcessID(1);
                                ResetForm(ControlsEnum.POINVHEADER);
                                ResetForm(ControlsEnum.INVOICELIST);
                                GetFieldValues(ControlsEnum.INVOICELIST);
                                SetFieldValues(ControlsEnum.INVOICELIST);
                                this.btnNew.Focus();
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else
                            {
                                #region Show Validation/Error Messages
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
                        if (ddlAddressType.Items.Count <= 0)
                        {
                            isContinue = false;

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ReqType").ToString()) + "');", true);
                            return;
                        }
                        //Show WorkFlow Popup
                        isContinue = true;
                        if ((hdfSaveWithoutAllocation.Value == "0") || (hdfSaveWithoutAllocation.Value == ""))
                        {
                            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
                            deductionDtlList = new List<DirectPOAdvDeductionDetails>();
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
                            deductionDtlList = new List<DirectPOAdvDeductionDetails>();
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
                        else//valid
                        {
                            isCancelled = false;
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                invoiceHeaderObj = new DirectPOInvoiceHeader();
                                invoiceHeaderObj = (DirectPOInvoiceHeader)SetUIValuesToObject(ControlsEnum.POINVHEADER);
                                if (hdfExchangeRate.Value != "-1")
                                {
                                    invoiceHeaderObj.WKF_FLAG = 1;
                                    if (invoiceHeaderObj.IVH_AMOUNT_TC > 0)
                                    {
                                        if (hasValidRate)
                                        {
                                            if (invoiceHeaderObj != null && invoiceHeaderObj.OrderDetail != null)
                                            {                                              
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
                                    ResetForm(ControlsEnum.INVOICELIST);
                                    GetFieldValues(ControlsEnum.INVOICELIST);
                                    SetFieldValues(ControlsEnum.INVOICELIST);
                                }
                            }
                            else
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                        }
                        break;
                    #endregion
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {                            
                            HiddenField hdfDept;
                            int dept;
                           rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                hdfInOpeningInv = (HiddenField)grdrow.FindControl("hdfInOpeningInv");
                                if (hdfInOpeningInv.Value == "1")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_Opening_cancel").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                                Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                                Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).Text;                                
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);  
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                HiddenField hdfInvStatus = grdrow.FindControl("hdfInvStatus") as HiddenField;
                                if (!string.IsNullOrEmpty(hdfInvStatus.Value) && Convert.ToInt32(hdfInvStatus.Value) == 0)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_DraftedInv").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                                if (Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1)
                                {
                                    btnSave.Visible = false;
                                    IsDeleted = true;                                   
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_alreadycancelled").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                                else
                                {
                                    btnSave.Visible = true;
                                    btnEdit.Visible = true;
                                    IsDeleted = false;
                                }
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
                    #endregion

                    #region Invoice List
                    case ActionsEnum.INVOICELIST:
                        FillProcessID(1);
                        CurrPK = 0;
                        hdfIVHPK.Value = "";
                        ResetForm(ControlsEnum.INVOICELIST);
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        EntryStatus = EntryStatus.LISTMODE;
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

                    #region TAX /DISC (TaxDetails,DiscDetails,TaxHeader,DiscHeader,TaxApply,TaxAdd,TaxDelete,TaxTypeChanged,CalculatedDTlTax,CalculatedHdrTax)
                    #region TAXDETAILS
                    case ActionsEnum.TAXDETAILS:
                        hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        txtAmount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("txtAmount") as TextBox);
                        txtDiscount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("txtDiscount") as TextBox);
                        hdfInvoiceDtlPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfInvoiceDtlPK") as HiddenField);
                        hdfItemPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                        hdfPOPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfPOPK") as HiddenField);
                        hdfPODetailPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfPODetailPK") as HiddenField);
                        PoDetailsPK = Convert.ToInt32(hdfPODetailPK.Value);
                        if (txtAmount != null && hdfInvoiceDtlPK != null && txtDiscount != null)
                        {
                            txtPopupItemAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? Convert.ToDouble(txtAmount.Text).ToString(hdfCurrencyFormat.Value) : (Convert.ToDouble(txtAmount.Text.Trim()) - Convert.ToDouble(txtDiscount.Text.Trim())).ToString(hdfCurrencyFormat.Value);
                            POInvoicePK = string.IsNullOrEmpty(hdfInvoiceDtlPK.Value) ? 0 : Convert.ToInt32(hdfInvoiceDtlPK.Value);
                            SelectedItemPK = string.IsNullOrEmpty(hdfItemPK.Value) ? 0 : Convert.ToInt32(hdfItemPK.Value);
                            SelectedPOPK = string.IsNullOrEmpty(hdfPOPK.Value) ? 0 : Convert.ToInt32(hdfPOPK.Value);
                            if (POInvoiceHeaderSession != null)
                            {
                                if (POInvoiceHeaderSession.TaxHdr != null && POInvoiceHeaderSession.TaxHdr.Where(r => r.VTL_TAX_CATEGORY == (int)TaxType.Tax).Count() > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_HdrTaxExist").ToString()) + "');", true);
                                    return;
                                }
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
                                            txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
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
                                        if (poInvTaxHdrObj != null)
                                        {
                                            if (poInvTaxHdrObj.VTL_TAX_CATEGORY != (int)TaxType.Shipping)
                                            {
                                                txtPopupAmount.Enabled = false;
                                                txtPopupOther.Enabled = false;
                                            }
                                        }
                                        else
                                        {
                                            txtPopupAmount.Enabled = true;
                                            txtPopupOther.Enabled = true;
                                        }
                                    }
                                }
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','600','300');", true);
                            }
                        }
                        break;
                    #endregion
                    #region DISCDETAILS
                    case ActionsEnum.DISCDETAILS:
                        hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        txtAmount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("txtAmount") as TextBox);
                        txtDiscount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("txtDiscount") as TextBox);
                        hdfInvoiceDtlPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfInvoiceDtlPK") as HiddenField);
                        hdfItemPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                        hdfPOPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfPOPK") as HiddenField);
                        hdfPODetailPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfPODetailPK") as HiddenField);
                        PoDetailsPK = Convert.ToInt32(hdfPODetailPK.Value);
                        if (txtAmount != null && hdfInvoiceDtlPK != null && txtDiscount != null)
                        {
                            txtPopupItemAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtAmount.Text).ToString(hdfCurrencyFormat.Value);                           
                            POInvoicePK = string.IsNullOrEmpty(hdfInvoiceDtlPK.Value) ? 0 : Convert.ToInt32(hdfInvoiceDtlPK.Value);
                            SelectedItemPK = string.IsNullOrEmpty(hdfItemPK.Value) ? 0 : Convert.ToInt32(hdfItemPK.Value);
                            SelectedPOPK = string.IsNullOrEmpty(hdfPOPK.Value) ? 0 : Convert.ToInt32(hdfPOPK.Value);
                            if (POInvoiceHeaderSession != null)
                            {
                                if (POInvoiceHeaderSession.TaxHdr != null && POInvoiceHeaderSession.TaxHdr.Where(r => r.VTL_TAX_CATEGORY == (int)TaxType.Discount).Count() > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_HdrDiscountExist").ToString()) + "');", true);
                                    return;
                                }
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
                                            txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
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
                                        if (poInvTaxHdrObj != null)
                                        {
                                            if (poInvTaxHdrObj.VTL_TAX_CATEGORY != (int)TaxType.Shipping)
                                            {
                                                txtPopupAmount.Enabled = false;
                                                txtPopupOther.Enabled = false;
                                            }
                                        }
                                        else
                                        {
                                            txtPopupAmount.Enabled = true;
                                            txtPopupOther.Enabled = true;
                                        }
                                    }
                                }
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("DiscountDetails").ToString() + "','600','300');", true);
                            }
                        }
                        break;
                    #endregion
                    #region TAXHEADER
                    case ActionsEnum.TAXHEADER:
                        hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (POInvoiceHeaderSession != null && grdInvoice.Rows.Count > 0)
                        {
                            if (POInvoiceHeaderSession.OrderDetail.Where(r => r.TaxDtl.Where(t => t.VTL_TAX_CATEGORY == (int)TaxType.Tax).Count() > 0).Count() > 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_DtlTaxExist").ToString()) + "');", true);
                                return;
                            }
                            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            SetFieldValues(ControlsEnum.TAXTYPES);                            
                            GetFieldValues(ControlsEnum.ADVANCEDTAXSETTINGS);
                            double OtherCharge = string.IsNullOrEmpty(txtShipping.Text.Trim()) ? 0 : Convert.ToDouble(txtShipping.Text.Trim());
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
                                        txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
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
                                    txtPopupOther.Enabled = true;
                                }
                                else
                                {                                  
                                    txtPopupOther.Enabled = false;
                                }
                            }
                            IsEditMode = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','600','300');", true);                           
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoItems").ToString()) + "');", true);
                        }
                        break;
                    #endregion
                    #region DISCHEADER
                    case ActionsEnum.DISCHEADER:
                        hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        double subtotalAdj = 0;
                        if (POInvoiceHeaderSession != null && grdInvoice.Rows.Count > 0)
                        {
                            if (POInvoiceHeaderSession.OrderDetail.Where(r => r.TaxDtl.Where(t => t.VTL_TAX_CATEGORY == (int)TaxType.Discount).Count() > 0).Count() > 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_DtlDiscountExist").ToString()) + "');", true);
                                return;
                            }
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
                                            txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
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
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("DiscountDetails").ToString() + "','600','300');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoItems").ToString()) + "');", true);
                        }
                        break;
                    #endregion
                    #region Other Charge Popup
                    case ActionsEnum.OTHERCHARGEHEADER:
                        if (grdInvoice.Rows.Count > 0)
                        {
                            SetFieldValues(ControlsEnum.OTHERCHARGELIST);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){CalculateTotalOtherCharge();});", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowotherchargePop", "ShowContainerDiv('[id$=divOtherchargeSplitUp]','" + GetLocalResourceObject("OtherchargeDetails").ToString() + "','800','400');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoItems").ToString()) + "');", true);
                        }
                        break;
                    #endregion
                    #region OTHERCHARGEAPPLY
                    case ActionsEnum.OTHERCHARGEAPPLY:
                        if (POInvoiceHeaderSession != null)
                        {
                            if (POInvoiceHeaderSession.POMappingDetails != null && POInvoiceHeaderSession.POMappingDetails.Count > 0)
                            {
                                // To set total other charge details                                
                                decimal totalOtherCharge = 0;
                                List<DirectPOInvoiceMappingDetails> PoOthrChrgLst = new List<DirectPOInvoiceMappingDetails>();
                                hdfOtherCharge.Value = "1";
                                foreach (GridViewRow gvr in grdOtherchargeSplit.Rows)
                                {
                                    if (gvr.RowType == DataControlRowType.DataRow)
                                    {
                                        TextBox txtAdjustNowAmount = gvr.FindControl("txtAdjustNowAmount") as TextBox;
                                        HiddenField hdfPOOtherchargePK = gvr.FindControl("hdfPOOtherchargePK") as HiddenField;
                                        HiddenField hdfPOHeadPK = gvr.FindControl("hdfPOPK") as HiddenField;
                                        if (txtAdjustNowAmount.Text != string.Empty)
                                        {
                                            totalOtherCharge += Convert.ToDecimal(txtAdjustNowAmount.Text);
                                        }

                                        poInvOtherchargeObj = POInvoiceHeaderSession.POMappingDetails.SingleOrDefault(rfq => rfq.IVM_PO_HDR == Convert.ToInt32(hdfPOHeadPK.Value));
                                        if (poInvOtherchargeObj != null)
                                        {
                                            poInvOtherchargeObj.IVM_OTHER_AMOUNT = Convert.ToDouble(txtAdjustNowAmount.Text);
                                            poInvOtherchargeObj.IVM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                        }
                                        PoOthrChrgLst.Add(poInvOtherchargeObj);
                                    }
                                }
                                POInvoiceHeaderSession.POMappingDetails = PoOthrChrgLst;
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
                        IsHeaderTax = false;
                        IsEditMode = false;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        SetEffectiveRate();
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
                                poInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.VID_PO) == SelectedPOPK);
                                if (poInvoiceDetailsObj != null)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        tempInvTaxSplitObj = poInvoiceDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.VTL_TAX == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        tempInvTaxSplitObj = poInvoiceDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.VTL_NAME == txtPopupOther.Text.Trim() && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                }
                            }
                            if (tempInvTaxSplitObj == null)
                            {
                                taxHdrList = new List<DirectPOInvoiceTaxHdr>();
                                poInvTaxHdrObj = new DirectPOInvoiceTaxHdr();
                                try
                                {
                                    poInvTaxHdrObj.VTL_TAX_AMT = string.IsNullOrEmpty(txtPopupAmount.Text.Trim()) ? 0 : Convert.ToDouble(txtPopupAmount.Text.Trim());
                                }
                                catch
                                {
                                    errorTaxAmount = true;
                                }
                                if (!errorTaxAmount)
                                {
                                    poInvTaxHdrObj.VTL_INVOICE_DTL = POInvoicePK;
                                    poInvTaxHdrObj.VTL_SL_NO = 1;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        poInvTaxHdrObj.VTL_TAX = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        poInvTaxHdrObj.VTL_TYPE = 1;
                                    }
                                    else
                                        poInvTaxHdrObj.VTL_TYPE = 2;
                                    poInvTaxHdrObj.VTL_TAX_TEXT = HttpUtility.HtmlEncode(SelectedTaxText);
                                    poInvTaxHdrObj.VTL_NAME = HttpUtility.HtmlEncode(txtPopupOther.Text);
                                    poInvTaxHdrObj.VTL_PK = 0;
                                    //rfqTaxHdrObj.VTL_TAX_CATEGORY_TEXT = "Tax";
                                    poInvTaxHdrObj.VTL_TAX_CATEGORY = Convert.ToInt32(hdfTaxCategory.Value);
                                    poInvTaxHdrObj.VTL_TAX_FORMULA = string.IsNullOrEmpty(hdfTaxFormula.Value) ? string.Empty : hdfTaxFormula.Value;
                                    if (IsHeaderTax)
                                    {
                                        if (poInvTaxHdrObj.VTL_TAX_CATEGORY == (int)TaxType.Discount)
                                        {
                                            totalAmt = 0;
                                            currentTotal = 0;
                                            taxAmt = 0;

                                            totalAmt = Convert.ToDouble(invoiceHeaderObj.IVH_AMOUNT_TC + invoiceHeaderObj.IVH_AMOUNT_NET_TC_ADJ);
                                            currentTotal = invoiceHeaderObj.TaxHdr.Where(quotation => quotation.VTL_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.VTL_TAX_AMT);
                                            if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                            {
                                                taxAmt = CalculateTaxFormula(poInvTaxHdrObj.VTL_TAX_FORMULA, totalAmt);
                                            }
                                            else
                                            {
                                                taxAmt = poInvTaxHdrObj.VTL_TAX_AMT;
                                            }
                                            if (totalAmt >= (currentTotal + taxAmt))
                                            {
                                                taxHdrList = invoiceHeaderObj.TaxHdr.ToList();
                                                taxHdrList.Add(poInvTaxHdrObj);
                                                invoiceHeaderObj.TaxHdr = taxHdrList;
                                            }
                                            else
                                            {
                                                isValidDisc = false;
                                            }
                                        }
                                        else
                                        {
                                            taxHdrList = invoiceHeaderObj.TaxHdr.ToList();
                                            taxHdrList.Add(poInvTaxHdrObj);
                                            invoiceHeaderObj.TaxHdr = taxHdrList;
                                        }
                                    }
                                    else
                                    {
                                        poInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.VID_PO) == SelectedPOPK);
                                        if (poInvoiceDetailsObj != null)
                                        {
                                            if (poInvTaxHdrObj.VTL_TAX_CATEGORY == (int)TaxType.Discount)
                                            {
                                                totalAmt = 0;
                                                currentTotal = 0;
                                                taxAmt = 0;

                                                totalAmt = poInvoiceDetailsObj.VID_AMOUNT;
                                                currentTotal = poInvoiceDetailsObj.TaxDtl.Where(quotation => quotation.VTL_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.VTL_TAX_AMT);
                                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                                {
                                                    taxAmt = CalculateTaxFormula(poInvTaxHdrObj.VTL_TAX_FORMULA, totalAmt);
                                                }
                                                else
                                                {
                                                    taxAmt = poInvTaxHdrObj.VTL_TAX_AMT;
                                                }
                                                if (totalAmt >= (currentTotal + taxAmt))
                                                {
                                                    taxHdrList = poInvoiceDetailsObj.TaxDtl.ToList();
                                                    taxHdrList.Add(poInvTaxHdrObj);
                                                    invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.VID_PO) == SelectedPOPK).TaxDtl = taxHdrList;
                                                }
                                                else
                                                {
                                                    isValidDisc = false;
                                                }
                                            }
                                            else
                                            {
                                                taxHdrList = poInvoiceDetailsObj.TaxDtl.ToList();
                                                taxHdrList.Add(poInvTaxHdrObj);
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
                                taxHdrList = new List<DirectPOInvoiceTaxHdr>();
                                if (IsHeaderTax)
                                {
                                    if (taxPK > 0)
                                    {
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
                                    poInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.VID_PO) == SelectedPOPK);
                                    if (poInvoiceDetailsObj != null)
                                    {
                                        if (taxPK > 0)
                                        {
                                            tempInvTaxSplitObj = poInvoiceDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.VTL_TAX == taxPK && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                        else
                                        {
                                            if (hdfTaxName != null)
                                            {
                                                tempInvTaxSplitObj = poInvoiceDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.VTL_NAME == hdfTaxName.Value && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                            }
                                        }
                                        poInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.VID_PO) == SelectedPOPK);
                                        if (poInvoiceDetailsObj != null)
                                        {
                                            taxHdrList = poInvoiceDetailsObj.TaxDtl.ToList();
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
                                txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
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
                    #endregion

                    #region Recalculate
                    case ActionsEnum.RECALCULATE:
                        #region Reset GRN Allocation Details
                        if (POInvoiceHeaderSession.OrderDetail != null && POInvoiceHeaderSession.OrderDetail.Count > 0)
                        {
                            foreach (DirectPOInvoiceDetails ordrDtl in POInvoiceHeaderSession.OrderDetail)
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

                    #region JOURNALIZE (Save,Update,Submit,Delete,Cancel))
                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
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
                        FillProcessID(1);
                        ResetForm(ControlsEnum.INVOICELIST);                      
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);                       
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
                        FillProcessID(1);
                        ResetForm(ControlsEnum.INVOICELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        break;
                    #endregion
                    #endregion

                    #region Deduction Popup
                    case ActionsEnum.DEDUCTIONHEADER:
                        if (grdInvoice.Rows.Count <= 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoItems").ToString()) + "');", true);
                            return;
                        }
                        if (POInvoiceHeaderSession != null && grdInvoice.Rows.Count > 0)
                        {
                            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
                            lblDedCustomer.Text = ERP.Utilities.CommonFunctions.GetShortString(txtVendorHd.Text, 30);
                            lblDedCustomer.ToolTip = txtVendorHd.Text;
                            lblDedCurrency.Text = ERP.Utilities.CommonFunctions.GetShortString(txtCurrency.Text, 15);                          
                            SetFieldValues(ControlsEnum.DEDUCTIONPOPUPGRID);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TotalSplit", "$(document).ready(function () { CalculateTotalSplit();});", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divDeduction]','" + GetLocalResourceObject("DeductionDetails").ToString() + "','900','400');", true);
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
                                foreach (DirectPOInvoiceDetails dtl in POInvoiceHeaderSession.OrderDetail)
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

                                    List<DirectPOAdvDeductionDetails> TempsoAdvDeductionDetailsList = null;

                                    double grossAmt = 0;
                                    double deduction = 0;
                                    double otherCharges = 0;
                                    double tax = 0;
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
                                    TempsoAdvDeductionDetailsList = new List<DirectPOAdvDeductionDetails>();
                                    if (deduction <= grossAmt)
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
                                            List<DirectPOAdvDeductionDetails> soAdvDeductionDetailsList = null;

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
                                            totalAllocatedDiscount += soAdvDeductionDetailsList[0].VAD_DISC_AMOUNT;                                          

                                            SetLineItemTax();
                                            if (Convert.ToDecimal(lblDedReceiptAmount.Text) > 0)
                                                TotalHDRDiscount += (HDRDiscount / Convert.ToDecimal(lblDedReceiptAmount.Text)) * Convert.ToDecimal(txtDedAllocateNowSplit.Text);
                                            TempsoAdvDeductionDetailsList.Add(soAdvDeductionDetailsList[0]);
                                        }                                       
                                        if (IsAdvInvHasTax)
                                        {
                                            txtDiscDeducted.Text = txtDiscDeducted.ToolTip = totalAllocatedDiscount.ToString(hdfCurrencyFormat.Value);
                                            txtHdrDeduction.Text = txtHdrDeduction.ToolTip = (deduction - (double)amtAdjAdvDeductionTotal).ToString(hdfCurrencyFormat.Value);
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

                  
                    #region ADJUSTAMOUNT
                    case ActionsEnum.ADJUSTAMOUNT:
                        ValidateAdjAmount();
                        break;
                    #endregion

                    #region GRN SPLITUP POPUP
                    case ActionsEnum.GRNDETAILS:
                        hdfInvoiceDtlPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfInvoiceDtlPK") as HiddenField);
                        hdfItemPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                        hdfPOPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfPOPK") as HiddenField);
                        POInvoicePK = string.IsNullOrEmpty(hdfInvoiceDtlPK.Value) ? 0 : Convert.ToInt32(hdfInvoiceDtlPK.Value);
                        SelectedItemPK = string.IsNullOrEmpty(hdfItemPK.Value) ? 0 : Convert.ToInt32(hdfItemPK.Value);
                        SelectedPOPK = string.IsNullOrEmpty(hdfPOPK.Value) ? 0 : Convert.ToInt32(hdfPOPK.Value);
                        hdfGRNExceed.Value = "0";
                        SetFieldValues(ControlsEnum.GRNQTYSPLIT);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalGRN", "$(document).ready(function(){CalculateTotalGRN();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divGRNQtySpilup]','" + GetLocalResourceObject("GRNAllocationDetails").ToString() + "','700','500');", true);

                        break;
                    #endregion
                    #region APPLY GRN SPLITUP POPUP DETAILS
                    case ActionsEnum.GRNAPPLY:
                        //Checking for Inv. Now Qty entered exceeds grn Qty
                        bool grnContinue = true;
                        if (hdfGRNExceed.Value == "0")
                        {
                            foreach (GridViewRow grdrowitem in grdGRNDetails.Rows)
                            {
                                TextBox txtInvNowGRNQty = (TextBox)grdrowitem.FindControl("txtInvNowGRNQty");
                                Label lblGRNBalance = (Label)grdrowitem.FindControl("lblGRNBalance");
                                double balQty = Convert.ToDouble(lblGRNBalance.Text.Replace(",", ""));
                                if (Convert.ToDouble(txtInvNowGRNQty.Text) > balQty)
                                {
                                    grnContinue = false;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowGRNNowQtyExceeds", "$(document).ready(function(){ShowGRNNowQtyExceeds();});", true);
                                    break;
                                }
                            }
                        }
                        if (grnContinue)
                        {
                            if (POInvoiceHeaderSession != null)
                            {
                                invoiceHeaderObj = POInvoiceHeaderSession;
                                grnList = new List<DirectGRNQTYDetails>();
                                poDtlObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK && Convert.ToInt32(rfq.VID_PO) == SelectedPOPK);
                                if (poDtlObj != null)
                                {
                                    grnList = poDtlObj.GRNDtl.ToList();
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
                                                poGrnQtyObj.VGL_INVOICE_DTL = poDtlObj.VID_PK;
                                                poGrnQtyObj.VGL_UOM = poDtlObj.VID_UOM;
                                                poGrnQtyObj.VGL_SL_NO = poDtlObj.VID_SL_NO;
                                            }
                                        }
                                    }
                                    poDtlObj.VID_QTY_INVOICED = totalGrnInvNow;
                                    TempPOInvoiceHeaderSession = invoiceHeaderObj;
                                    SetFieldValues(ControlsEnum.POINVDETAIL);
                                    POInvoicePK = 0;
                                    SelectedItemPK = 0;
                                    SetDetailTax(null);
                                    SetHdrTax();
                                    ResetForm(ControlsEnum.TAXPOPUPGRID);
                                    SetUIValuesToObject(ControlsEnum.POINVDETAIL);
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

                    #region  UPLOAD (ADDITEM,EDITITEM,REMOVEITEM)
                    #region ADDITEM
                    case ActionsEnum.ADDITEM:
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

                                            // poUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                            poUploadObj.DOC_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
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
                    #region ALERT
                    case ActionsEnum.ALERT:
                        ucrAlert.TypeCode = POGroup == POInvoiceGroup.Goods ? ApplicationType.TPI
                            : POGroup == POInvoiceGroup.Services ? ApplicationType.TPSI : ApplicationType.EIT;
                        ucrAlert.TypePK = CurrPK;
                        ucrAlert.TypeRef = lblInvoiceNo.Text.Trim();
                        ucrAlert.TrxDate = string.IsNullOrEmpty(txtInvoiceDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtInvoiceDate.Text.Trim());
                        ucrAlert.TypeText = GetLocalResourceObject("Alert_Type_Text").ToString();                     
                        ucrAlert.GetAlertList();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divAlert]','Alert','800','400');", true);
                        break;
                    #endregion
                    #region DTL SEARCH/VENDOR CHANGE
                    case ActionsEnum.DTLSEARCH:
                    case ActionsEnum.VENDORSELECTED:
                        uclPOPaging.CurrentPage = 1;
                        PageIndexPO = CommonConstants.SELECT_VALUE_ONE;
                        dtPendingPOList = null;
                        GetFieldValues(ControlsEnum.PENDINGPOLIST);
                        SetFieldValues(ControlsEnum.PENDINGPOLIST);
                        hdfIsPendingPOVisible.Value = "1";
                        break;
                    #endregion
                    #region DTL CLEAR SEARCH
                    case ActionsEnum.DTLCLEARSEARCH:
                        txtPONumber.Text = string.Empty;
                        hdfPoPK.Value = string.Empty;
                        txtPendingFromDate.Text = string.Empty;
                        txtPendingToDate.Text = string.Empty;
                        uclPOPaging.CurrentPage = 1;
                        PageIndexPO = CommonConstants.SELECT_VALUE_ONE;
                        dtPendingPOList = null;
                        GetFieldValues(ControlsEnum.PENDINGPOLIST);
                        SetFieldValues(ControlsEnum.PENDINGPOLIST);
                        hdfIsPendingPOVisible.Value = "1";
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
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ResetForm(ControlsEnum.POINVHEADER);    
                        FillProcessID(1);
                        ResetForm(ControlsEnum.INVOICELIST);
                        ResetForm(ControlsEnum.TAXPOPUPGRID);
                        ResetForm(ControlsEnum.ADDITEM);
                        CurrPK = 0;
                        hdfIVHPK.Value = "";
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Show Popup
                    case ActionsEnum.SHOWPOPUP:
                        poPK = ((LinkButton)sender).CommandArgument;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + poPK + "&APPTYPE=" + ApplicationType.PO + "&APPSUBTYPE=") + "');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divNewPOList]','" + GetLocalResourceObject("POList").ToString() + "','900','300');", true);
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
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                Pk = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + Pk.ToString() + "&APPTYPE=" + ApplicationType.TPI + "&APPSUBTYPE=") + "');", true);
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }

                        break;
                    case ActionsEnum.PRINTDT:
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.TPI + "&APPSUBTYPE=1") + "');", true);
                        }
                        break;
                    case ActionsEnum.PRINTLINEITEMPO:
                        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).NamingContainer;
                        HiddenField hdfLineItemPOPK = (HiddenField)gvrow.FindControl("hdfPOPK");
                        if (hdfLineItemPOPK != null && Convert.ToInt32(hdfLineItemPOPK.Value) > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfLineItemPOPK.Value + "&APPTYPE=" + ApplicationType.PO + "&APPSUBTYPE=" + (POGroup == POInvoiceGroup.Services ? ((int)POSubType.Service).ToString() : "")) + "');", true);
                        }
                        break;
                    case ActionsEnum.PRINTDEDINVOICE:
                        GridViewRow gvAdvrow = (GridViewRow)((LinkButton)sender).NamingContainer;
                        HiddenField hdfAdvInvoiceLineItemPK = (HiddenField)gvAdvrow.FindControl("hdfAdvInvoicePK");
                        if (hdfAdvInvoiceLineItemPK != null && Convert.ToInt32(hdfAdvInvoiceLineItemPK.Value) > 0)
                        {                           
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfAdvInvoiceLineItemPK.Value + "&APPTYPE=" + ApplicationType.TPI + "&APPSUBTYPE=13") + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divDeduction]','" + GetLocalResourceObject("DeductionDetails").ToString() + "','800','400');", true);                           
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
                    #region Show GRN Print
                    case ActionsEnum.SHOW:
                        grnPK = ((LinkButton)sender).CommandArgument;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + grnPK + "&APPTYPE=" + ApplicationType.GRN + "&APPSUBTYPE=") + "');", true);
                        SetFieldValues(ControlsEnum.GRNQTYSPLIT);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalGRN", "$(document).ready(function(){CalculateTotalGRN();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divGRNQtySpilup]','" + GetLocalResourceObject("GRNAllocationDetails").ToString() + "','700','500');", true);
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
            }
        }


        #region Workflow Submit
        /// <summary>
        /// Save and submit With workflow 
        /// </summary>
        /// <param name="objSalPayment"></param>
        private void SaveTransaction(DirectPOInvoiceHeader objPOInvoice, int workflowFlag)
        {
            int? result = 0;
            string savePath = string.Empty;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (objPOInvoice == null)
                objPOInvoice = new DirectPOInvoiceHeader();
            #region Application Code
            objPOInvoice.ATL_APP_TYPE = ApplicationType.TPI;
            objPOInvoice.APT_CODE = POGroup == POInvoiceGroup.Goods ? ApplicationType.TPI
                               : POGroup == POInvoiceGroup.Services ? ApplicationType.TPSI : ApplicationType.EIT;
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
            string xmlDoc = CommonFunctions.XmlSerialize<DirectPOInvoiceHeader>(objPOInvoice);//CommonFunctions.ObjectTOXml(rfqResponseHeaderObj);
            string invoiceNumber = string.Empty;
            result = BusinessLogic.POInvoicing.POInvoiceBL.SavePOInvoiceTradingWkf(xmlDoc, out invoiceNumber);
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
                    #endregion
                }
                #endregion
                if (result.HasValue && result.Value > 0)
                {
                    ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();
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
                    if (string.IsNullOrEmpty(invoiceNumber))
                        invoiceNumber = lblInvoiceNo.Text.Trim();
                    object[] args = new object[2];
                    args[0] = Resources.PageNameRes.POInvoice;
                    args[1] = invoiceNumber;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                    // Show Save Message and redired to listing page   
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
                int slno;                
                if (((GridView)sender).ID == "grdDeduction")
                {
                    #region grdDeduction
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        #region DataRow
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
                            if (string.IsNullOrEmpty(CurrPK.ToString()) || Convert.ToInt32(CurrPK) == 0)
                            {
                                txtDedAllocateNowSplit.Text = GetFormattedCurrency(Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].VAD_AMOUNT) > 0 ? deductionDtlList[e.Row.RowIndex].VAD_AMOUNT : (Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].PVH_PO_PAID_AMT) - Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].IVH_AMOUNT_ALLOCATED)));
                                hdfDedAllocateNowSplit.Value = GetFormattedCurrency(Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].VAD_AMOUNT) > 0 ? deductionDtlList[e.Row.RowIndex].VAD_AMOUNT : (Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].PVH_PO_PAID_AMT) - Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].IVH_AMOUNT_ALLOCATED)));

                                hdfAllocNowAmount.Value = GetFormattedCurrency(Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].PVH_PO_PAID_AMT) - Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].IVH_AMOUNT_ALLOCATED));

                                txtOtherAmountSplit.Text = GetFormattedCurrency(Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].VAD_OTHER_AMOUNT) > 0 ? deductionDtlList[e.Row.RowIndex].VAD_OTHER_AMOUNT : (Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].PVH_PO_PAID_OTHER_AMT) - Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].IVH_OTHER_AMT_ALLOCATED)));
                                hdfCurPaidOtherAmount.Value = GetFormattedCurrency(Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].VAD_OTHER_AMOUNT) > 0 ? deductionDtlList[e.Row.RowIndex].VAD_OTHER_AMOUNT : (Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].PVH_PO_PAID_OTHER_AMT) - Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].IVH_OTHER_AMT_ALLOCATED)));

                                txtTaxSplit.Text = GetFormattedCurrency(Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].VAD_TAX_AMOUNT) > 0 ? deductionDtlList[e.Row.RowIndex].VAD_TAX_AMOUNT : (Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].PVH_PO_PAID_TAX_AMT) - Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].IVH_TAX_AMT_ALLOCATED)));
                                hdfCurPaidTax.Value = GetFormattedCurrency(Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].VAD_TAX_AMOUNT) > 0 ? deductionDtlList[e.Row.RowIndex].VAD_TAX_AMOUNT : (Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].PVH_PO_PAID_TAX_AMT) - Convert.ToDecimal(deductionDtlList[e.Row.RowIndex].IVH_TAX_AMT_ALLOCATED)));
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
                                txtTaxSplit.Text = GetFormattedCurrency(deductionDtlList[e.Row.RowIndex].VAD_TAX_AMOUNT.ToString());
                                hdfCurPaidTax.Value = GetFormattedCurrency(deductionDtlList[e.Row.RowIndex].VAD_TAX_AMOUNT.ToString());
                                hdfCurPaidDisc.Value = GetFormattedCurrency(deductionDtlList[e.Row.RowIndex].VAD_DISC_AMOUNT.ToString());
                                DedTotalAllocateNowFooterSplit = DedTotalAllocateNowFooterSplit + Convert.ToDecimal(txtDedAllocateNowSplit.Text);
                                OtherAmountFooterSplit = OtherAmountFooterSplit + Convert.ToDecimal(txtOtherAmountSplit.Text);
                                TaxFooterSplit = TaxFooterSplit + Convert.ToDecimal(txtTaxSplit.Text);
                            }
                        } 
                        #endregion
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        #region Footer
                        Label lblDedTotalAllocateNowFooterSplit = (Label)e.Row.FindControl("lblDedTotalAllocateNowFooterSplit");
                        Label lblOtherAmountFooterSplit = (Label)e.Row.FindControl("lblOtherAmountFooterSplit");
                        Label lblTaxFooterSplit = (Label)e.Row.FindControl("lblTaxFooterSplit");
                        HiddenField hdfDedTotalAllocateNowFooterSplit = (HiddenField)e.Row.FindControl("hdfDedTotalAllocateNowFooterSplit");
                        HiddenField hdfOtherTotalFooterSplit = (HiddenField)e.Row.FindControl("hdfOtherTotalFooterSplit");
                        HiddenField hdfTaxTotalFooterSplit = (HiddenField)e.Row.FindControl("hdfTaxTotalFooterSplit");

                        hdfDedTotalAllocateNowFooterSplit.Value = lblDedTotalAllocateNowFooterSplit.Text = string.Format("{0:c}", DedTotalAllocateNowFooterSplit);
                        hdfOtherTotalFooterSplit.Value = lblOtherAmountFooterSplit.Text = string.Format("{0:c}", OtherAmountFooterSplit);
                        hdfTaxTotalFooterSplit.Value = lblTaxFooterSplit.Text = string.Format("{0:c}", TaxFooterSplit); 
                        #endregion
                    }
                    if (!IsAdvInvHasTax)
                    {
                        grdDeduction.Columns[10].Visible = false;
                        grdDeduction.Columns[11].Visible = false;
                    } 
                    #endregion
                }
                else if (((GridView)sender).ID == "grdInvoice")
                {
                    #region grdInvoice
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        #region DataRow
                        ImageButton imgDiscount = e.Row.FindControl("imgDiscount") as ImageButton;
                        ImageButton imgTax = e.Row.FindControl("imgTax") as ImageButton;
                        System.Web.UI.HtmlControls.HtmlGenericControl divDiscount = e.Row.FindControl("divDiscount") as System.Web.UI.HtmlControls.HtmlGenericControl;
                        System.Web.UI.HtmlControls.HtmlGenericControl divTax = e.Row.FindControl("divTax") as System.Web.UI.HtmlControls.HtmlGenericControl;
                        imgTax.Visible = IsItemwiseTaxForTradingPurchase;
                        imgDiscount.Visible = IsItemwiseDiscountForTradingPurchase;
                        if (IsItemwiseTaxForTradingPurchase)
                        {
                            if (divTax != null)
                                divTax.Attributes.Add("class", "w100");
                        }
                        if (IsItemwiseDiscountForTradingPurchase)
                        {
                            if (divDiscount != null)
                                divDiscount.Attributes.Add("class", "w100");
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
                        #endregion
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        #region Footer
                        TextBox txtAdjustAmountFooter = (TextBox)e.Row.FindControl("txtAdjustAmountFooter");
                        if (!string.IsNullOrEmpty(txtAdjustAmountFooter.ID))
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtAdjustAmountFooter.ID + "", "$('[id$=" + txtAdjustAmountFooter.ID + "]').ForceToNumeric();", true); 
                        #endregion
                    } 
                    #endregion
                }               
                else if (((GridView)sender).ID == "grdUploads")
                {
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
                //taxPayable For Multiplying with exchangerate
                else if (((GridView)sender).ID == "grdTaxPayable")
                {
                    #region grdTaxPayable
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
                    #endregion
                }
                else if (((GridView)sender).ID == "grdOtherchargeSplit")
                {
                    #region grdOtherchargeSplit
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        Label lblTotalOtherCharge = (Label)e.Row.FindControl("lblTotalOtherCharge");
                        Label lblTotalInvOtherCharge = (Label)e.Row.FindControl("lblTotalInvOtherCharge");
                        Label lblTotalBalanceOtherCharge = (Label)e.Row.FindControl("lblTotalBalanceOtherCharge");
                        lblTotalOtherCharge.Text = GetFormattedCurrencyWithSeperation(POTotalOtherAmount);
                        lblTotalInvOtherCharge.Text = GetFormattedCurrencyWithSeperation(POTotalInvOtherAmount);
                        lblTotalBalanceOtherCharge.Text = GetFormattedCurrencyWithSeperation(POTotalBalanceOtherAmount);
                    } 
                    #endregion
                }
                else if (((GridView)sender).ID == "grdGRNDetails")
                {
                    #region grdGRNDetails
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        Label lblTotalGRNQty = (Label)e.Row.FindControl("lblTotalGRNQty");
                        Label lblTotalGRNInvdQty = (Label)e.Row.FindControl("lblTotalGRNInvdQty");
                        Label lblTotalGRNBalance = (Label)e.Row.FindControl("lblTotalGRNBalance");
                        lblTotalGRNQty.Text = GetFormattedNumberWithSeperation(POTotalGRNQty);
                        lblTotalGRNInvdQty.Text = GetFormattedNumberWithSeperation(POTotalGRNInvdQty);
                        lblTotalGRNBalance.Text = GetFormattedNumberWithSeperation(POTotalGRNBalance);
                    } 
                    #endregion
                }
                else if (((GridView)sender).ID == "grdPendingPoList")
                {
                    #region grdPendingPoList
                    //selectedRowColor
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        HiddenField hdfPOPk = e.Row.FindControl("hdfPOPk") as HiddenField;
                        CheckBox chkPoPendSelect = e.Row.FindControl("chkPoPendSelect") as CheckBox;
                        if (POInvoiceHeaderSession != null && POInvoiceHeaderSession.OrderDetail.Where(r => r.VID_PO == hdfPOPk.Value).Count() > 0)
                        {
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }      
        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
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
            uclPOPaging.CurrentPage = 1;
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
            btnSaveDuduction.PreRender += new EventHandler(btnAction_PreRender);
            btnNew.PreRender += new EventHandler(btnAction_PreRender);

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
            lnkList.Load += new EventHandler(btnAction_Load);
            lnkDetail.Load += new EventHandler(btnAction_Load);
            btnApply.Load += new EventHandler(btnAction_Load);
            imgPopupAdd.Load += new EventHandler(btnAction_Load);
            btnSaveDuduction.Load += new EventHandler(btnAction_Load);
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

                hdfInvCategory.Value = Convert.ToByte((byte)POInvoiceCategory.Invoice).ToString();
                hdfSaveWithoutAllocation.Value = "0";
                if (POInvoiceHeaderSession != null)
                {
                    hdfHasTax.Value = ((POInvoiceHeaderSession.TaxHdr == null || POInvoiceHeaderSession.TaxHdr.Count == 0)
                        && POInvoiceHeaderSession.OrderDetail.All(dtl => (dtl.TaxDtl == null || dtl.TaxDtl.Count == 0)))
                        ? CommonConstants.SELECT_VALUE_ZERO : CommonConstants.SELECT_VALUE_ONE;
                }
                #region VIEWMODE\NEWMODE\ENTRYMODE\LISTMODE
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
                #endregion
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);               
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);              
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);

                if (IsDeleted)
                {
                    btnSave.Visible = false;
                    btnDeleteNew.Visible = false;
                    hdfIsInvCancelled.Value = "1";                  
                }
                else
                    hdfIsInvCancelled.Value = "0";

                decimal.TryParse(txtHdrDiscount.Text, out hrdDiscAmnt);
                if (hrdDiscAmnt > 0 && hdfShowEffRateInPI.Value == "1")
                    grdInvoice.Columns[2].Visible = true;
                else
                    grdInvoice.Columns[2].Visible = false;

                if (grdInvoice.Rows.Count > 0)
                {
                    txtVendorSearch.Enabled = false;
                    txtVendorHd.Enabled = false;    
                }
                else
                {
                    txtVendorSearch.Enabled = true;
                    txtVendorHd.Enabled = true;               
                }               
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
            POINVHEADERNEW,
            GRNQTYSPLIT,
            COMPANYSRCH,
            PENDINGPOLIST,
            POINVOICELIST
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
                    maxRoundoff = Convert.ToDouble(txtSubTotalFooter.Text) * Convert.ToDouble(hdfPriceAdjPercentage.Value) / 100;
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
    }
}