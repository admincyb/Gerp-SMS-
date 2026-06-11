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
using BusinessObject.SaleOrder;
using System.Text;
using ERPManager.Finance;
using System.Transactions;
using System.IO;
using BusinessLogic.CommonManagement;
using BusinessObject.Sales;


namespace ERPSMS_v01.Sales
{
    public partial class DebitCreditNoteTrading : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties

        /// <summary>
        /// Invoice PO Split List
        /// </summary>
        private List<FIN_CRDR_NOTE_DTL> CrDrSplitList
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.CrDrSplitList] == null ? new List<FIN_CRDR_NOTE_DTL>()
                    : (List<FIN_CRDR_NOTE_DTL>)Session[ERP.Utilities.SessionStrings.CrDrSplitList];
            }
            set
            {
                if (value == null)
                    Session.Remove(ERP.Utilities.SessionStrings.CrDrSplitList);
                else
                    Session[ERP.Utilities.SessionStrings.CrDrSplitList] = value;
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
        /// Keep Config Value
        /// </summary>
        private List<ADM_CONFIG_MST> admConfigMstListCrdrType
        {
            get
            {
                return this.ViewState[ERP.Utilities.ViewstateStrings.admConfigMstListCrdrType] == null ? new List<ADM_CONFIG_MST>() : (List<ADM_CONFIG_MST>)this.ViewState[ERP.Utilities.ViewstateStrings.admConfigMstListCrdrType];
            }
            set
            {
                this.ViewState[ERP.Utilities.ViewstateStrings.admConfigMstListCrdrType] = value;
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
        /// InvoiceType
        /// </summary>
        private int InvType
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.InvType]);
            }
            set
            {
                this.ViewState[ViewstateStrings.InvType] = value;
            }
        }

        /// <summary>
        /// Invoice Category
        /// </summary>
        private int InvCategory
        {
            get
            {
                return this.ViewState[ViewstateStrings.InvoiceCategory] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.InvoiceCategory]);
            }
            set
            {
                this.ViewState[ViewstateStrings.InvoiceCategory] = value;
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
        /// VendorPK
        /// </summary>
        private long CrDrMpgPK
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.CrDrMpgPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CrDrMpgPK] = value;
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
        private decimal paynowtax
        {
            get
            {
                return Convert.ToDecimal(this.ViewState[ViewstateStrings.paynowtax]);
            }
            set
            {
                this.ViewState[ViewstateStrings.paynowtax] = value;
            }
        }

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
        /// To maintain keep selected pos
        /// </summary>
        private List<long> SelectedInvoicesCrDr
        {
            get
            {
                return this.ViewState[ViewstateStrings.SelectedInvoicesCrDr] != null ? (List<long>)this.ViewState[ViewstateStrings.SelectedInvoicesCrDr] : null;
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedInvoicesCrDr] = value;
            }

        }

        /// <summary>
        /// To check if removed
        /// </summary>
        private Boolean RemovedInvoicesCrDr
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.RemovedInvoicesCrDr] == null ? false : (Boolean)Session[ERP.Utilities.SessionStrings.RemovedInvoicesCrDr];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.RemovedInvoicesCrDr] = value;
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

        private int PID
        {
            get
            {
                return this.ViewState["PID"] == null ? 1 : Convert.ToInt32(this.ViewState["PID"].ToString());
            }
            set
            {
                this.ViewState["PID"] = value;
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
        /// To set Line Item Tax configuration
        /// </summary>
        private bool isLineItemTaxEnabled
        {
            get
            {
                return this.ViewState[ViewstateStrings.LineItemTaxEnabled] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.LineItemTaxEnabled]);
            }
            set
            {
                this.ViewState[ViewstateStrings.LineItemTaxEnabled] = value;
            }
        }

        /// <summary>
        /// Account Type
        /// </summary>
        private int RowIndex
        {
            get
            {
                return this.ViewState[ViewstateStrings.RowIndex] == null ? -1 : Convert.ToInt32(this.ViewState[ViewstateStrings.RowIndex].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.RowIndex] = value;
            }
        }
        /// <summary>
        /// To maintain keep selected Invoices
        /// </summary>
        private List<ERPData.FIN_INVOICE_CUS_HDR> FinInvoiceCusHdrSelectedList
        {
            get
            {
                return (List<ERPData.FIN_INVOICE_CUS_HDR>)Session[ERP.Utilities.SessionStrings.FinInvoiceCusHdrSelectedList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.FinInvoiceCusHdrSelectedList] = value;
            }

        }

        /// <summary>
        /// To maintain keep selected Invoices
        /// </summary>
        private List<ERPData.FIN_CRDR_NOTE_MPG> FinInvoiceCrDrSelectedList
        {
            get
            {
                return (List<ERPData.FIN_CRDR_NOTE_MPG>)Session[ERP.Utilities.SessionStrings.FinInvoiceCrDrSelectedList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.FinInvoiceCrDrSelectedList] = value;
            }

        }


        private List<ERPData.FIN_PAYMENT_VND_TRX_MPG> EditedPaymentDtls
        {
            get
            {
                return (List<ERPData.FIN_PAYMENT_VND_TRX_MPG>)Session[ERP.Utilities.SessionStrings.EditedPaymentDtls];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.EditedPaymentDtls] = value;
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

        /// Biju
        /// </summary>
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


        private List<FIN_CRDR_NOTE_TAX_HDR> FinCrDrNoteTaxHeader
        {
            get
            {
                return (List<FIN_CRDR_NOTE_TAX_HDR>)this.ViewState[ERP.Utilities.SessionStrings.FinCrDrNoteTaxHeader];
            }
            set
            {
                this.ViewState[ERP.Utilities.SessionStrings.FinCrDrNoteTaxHeader] = value;
            }
        }

        private List<FIN_CRDR_NOTE_TAX_HDR> FinCrDrNoteTaxHeaderTemp
        {
            get
            {
                return (List<FIN_CRDR_NOTE_TAX_HDR>)this.ViewState[ERP.Utilities.SessionStrings.FinCrDrNoteTaxHeaderTemp];
            }
            set
            {
                this.ViewState[ERP.Utilities.SessionStrings.FinCrDrNoteTaxHeaderTemp] = value;
            }
        }

        private int InvRowIndex
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
        /// Document Attach List
        /// </summary>
        private List<DebitCreditUploads> DocAttachList
        {
            get
            {
                return this.ViewState[ViewstateStrings.DocAttachList] == null ? new List<DebitCreditUploads>() : (List<DebitCreditUploads>)(this.ViewState[ViewstateStrings.DocAttachList]);
            }
            set
            {
                this.ViewState[ViewstateStrings.DocAttachList] = value;
            }
        }

        /// <summary>
        /// Store Files in Attachment List
        /// </summary>
        private List<BusinessObject.Sales.FileDetails> FileDetailsList
        {
            get
            {
                return this.Session[ERP.Utilities.SessionStrings.FileDetailsList] == null ? null : (List<BusinessObject.Sales.FileDetails>)this.Session[ERP.Utilities.SessionStrings.FileDetailsList];
            }
            set
            {
                this.Session[ERP.Utilities.SessionStrings.FileDetailsList] = value;
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

        /// <summary>
        /// To maintain keep CN/DN details in session
        /// </summary>
        private DebitCreditHeader DebitCreditHeaderSession
        {
            get
            {
                return (DebitCreditHeader)Session[ERP.Utilities.SessionStrings.DebitCreditHeaderSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.DebitCreditHeaderSession] = value;
            }
        }

        /// <summary>
        /// Store Header tax/other charge details in view state
        /// </summary>
        private List<DebitCreditTaxHdr> DebitCreditHeaderTax
        {
            get
            {
                return this.ViewState[ERP.Utilities.ViewstateStrings.HeaderTax] == null ? new List<DebitCreditTaxHdr>() : (List<DebitCreditTaxHdr>)this.ViewState[ERP.Utilities.ViewstateStrings.HeaderTax];
            }
            set
            {
                this.ViewState[ERP.Utilities.ViewstateStrings.HeaderTax] = value;
            }
        }

        /// <summary>
        /// Store Header tax/other charge details in view state
        /// </summary>
        private List<DebitCreditTaxHdr> tempDebitCreditHeaderTax
        {
            get
            {
                return this.ViewState[ERP.Utilities.ViewstateStrings.HeaderTaxTemp] == null ? new List<DebitCreditTaxHdr>() : (List<DebitCreditTaxHdr>)this.ViewState[ERP.Utilities.ViewstateStrings.HeaderTaxTemp];
            }
            set
            {
                this.ViewState[ERP.Utilities.ViewstateStrings.HeaderTaxTemp] = value;
            }
        }
        #endregion
        DataTable dtTaxDetails;
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        User currentUser;

        //page related Entity Object
        DataTable dtSOData;
        private DataTable dtInvCategory;
        private DataTable dtTaxSettings;
        private ServiceUtility serviceUtilityObj;
        private FIN_INVOICE_CUS_HDR finInvoiceCusHdrObj;
        private List<long> SelectedInvoiceCrDrList;

        #region Fin Cr Dr
        private FIN_CRDR_NOTE_HDR finCrDrNoteHdrObj;
        private FIN_CRDR_NOTE_MPG finCrDrNoteMpgObj;
        private FIN_CRDR_NOTE_TAX_HDR finCrDrNoteTaxObj;
        #endregion

        //private ERPData.POInvoice poInvoiceObj;

        //List for binding details to controls  
        #region Fin Cr Dr List
        private List<FIN_CRDR_NOTE_HDR> finCrDrNoteHdrList;
        private List<FIN_CRDR_NOTE_MPG> finCrDrNoteMpgList;
        private List<FIN_CRDR_NOTE_TAX_HDR> finCrDrNoteTaxList;

        #endregion

        private List<FIN_PAYMENT_VND_TRX_MPG> finPaymentVndTrxMpgList;
        private List<FIN_INVOICE_CUS_HDR> finInvoiceCusHdrList;

        List<FIN_CRDR_NOTE_DTL> tempfinCrDrCusMpgList;

        //List<FIN_CRDR_NOTE_DTL> tempRaiseNoteSplitupList;

        private List<FIN_CRDR_NOTE_DTL> finCrDrCusMpgList;
        private List<FIN_INVOICE_CUS_DTL> FinCrDrCusTrxMpgList;
        private List<FIN_CRDR_NOTE_TAX_DTL> finCrDrCusMpgTaxList;
        private SOInvoiceHeader invoiceHeaderObj;
        private List<FIN_INVOICE_CUS_HDR> finInvoiceVndHdrListForPaymentSplit;
        private List<FIN_INVOICE_CUS_HDR> objInvoiceDetails;
        private FIN_INVOICE_CUS_HDR objInvDetails;
        private List<FIN_INVOICE_CUS_DTL> objInvoiceItemDetails;
        private FIN_INVOICE_CUS_DTL objInvItemDetails;
        private FIN_INVOICE_CUS_DTL finInvItemDtl;
        private List<FIN_INVOICE_CUS_DTL> finInvItemList;
        private List<long> selectedInvoiceList;
        private DataTable dtLineItemTaxSettings;
        private DataTable dtPendingInvList;
        private DataTable dtInvoiceType;
        private long InvPk = 0;
        private long InvItemPk = 0;
        private string transactionNumber;
        private bool updateCrDr;
        private bool isDrCrUsedInOtherTrns = false;
        int JournalPK;
        int hasjournalized;
        int creditDebitType;
        int custPK = 0;
        bool isPosted = false;


        private string refID;
        private string inboxFlag;
        private bool isSplitChanged = false;
        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;

        private ADM_CONFIG_MST admConfigMstObj;
        private List<SPADM_APP_STATUS_CFG_GET_KV_Result> workflowStatusList;
        private List<SPADM_APP_STATUS_CFG_GET_KV_Result> workflowStatusDomesticList;
        private List<SPADM_APP_STATUS_CFG_GET_KV_Result> workflowStatusExportList;
        private List<FIN_YEAR_MST> finYearMstList;

        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;

        private FIN_CRDR_NOTE_DTL finCrDrCusSoMpgObj;
        private FIN_INVOICE_CUS_DTL FinCrDrCusTrxMpgObj;
        private FIN_INVOICE_CUS_HDR finCrDrVndHdrObjForPaymentSplit;

        private FIN_RECEIPT_CUS_CRDR_MPG finReceiptCRDRmpgObj;
        private List<FIN_RECEIPT_CUS_CRDR_MPG> finReceiptCRDRmpgList;

        //private List<FIN_RECEIPT_CUS_ALCN_DTL> objCnsAllocationList;
        private List<CrdrAllocations> objCrdrAllocations;

        int InvoiceType = 1;
        int crDrInvoiceType;
        private decimal Exchangerate = 1;
        private decimal grndTotalTaxSplit = 0;
        private decimal grndTotalTax = 0;
        private decimal CrDrAllocatedAmnt = 0;
        private double ExchageRate = 0;
        private List<SOInvoiceTaxHdr> taxTempList;
        private List<SOInvoiceTaxHdr> taxList;
        //private DataTable dtBalAmountSplit;
        private DebitCreditInvBO InvListObj;
        private DebitCreditHeader debitCreditHeaderObj;
        private DebitCreditHeader tempDebitCreditHeader;
        bool isCancelled = false;
        private DataSet dsPageData;
        private DataSet dsCustomerData;
        private DataTable dtDebitCreditList;

        private string[] _operators = { "-", "+", "/", "*", "^" };
        private Func<double, double, double>[] _operations = {
            (a1, a2) => a1 - a2,
            (a1, a2) => a1 + a2,
            (a1, a2) => a1 / a2,
            (a1, a2) => a1 * a2,
            (a1, a2) => Math.Pow(a1, a2)
        };
        DebitCreditUploads admDocAttachObj;

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

                if (!IsPostBack)
                {
                    ConfigurationSettings();
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);

                    GetFieldValues(ControlsEnum.SOTYPE);
                    SetFieldValues(ControlsEnum.SOTYPE);

                    GetFieldValues(ControlsEnum.INVCATEGORY);
                    SetFieldValues(ControlsEnum.INVCATEGORY);

                    CrDrSplitList = null;
                    DebitCreditHeaderSession = null;
                    tempDebitCreditHeader = null;
                    DocAttachList = null;
                    hdfJournalizeWorkFlow.Value = "0";
                    Session[ERP.Utilities.SessionStrings.TransactionType] = null;

                    txtSearchDateFrom.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfSearchDateFrom.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    txtSearchDateTo.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfSearchDateTo.Value = DateTime.Now.ToString();

                    hdfDecimalDigits.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
                    {
                        hdfDecimalDigits.Value += "0";
                    }
                    hdfRateFormat.Value = "#0.";
                    int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit]));
                    for (int i = 0; i < rateDecimalDigits; i++)
                    {
                        hdfRateFormat.Value += "0";
                    }
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                    }

                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfCurrencyFormatWithSeperator.Value = "#" + currencysep + "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormatWithSeperator.Value += "0";
                    }

                    hdfNumberDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
                    hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();

                    hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                    hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();

                    if (Session[ERP.Utilities.SessionStrings.SelectedInvoicesCrDr] != null)
                    {
                        SelectedInvoicesCrDr = (List<long>)Session[ERP.Utilities.SessionStrings.SelectedInvoicesCrDr];
                        Session[ERP.Utilities.SessionStrings.SelectedInvoicesCrDr] = null;
                        EntryStatus = EntryStatus.NEWMODE;
                        ddlCompany.Focus();
                    }
                    GetFieldValues(ControlsEnum.CRDRTYPE);
                    SetFieldValues(ControlsEnum.CRDRTYPE);

                    ////start
                    string pid = Request.QueryString[QueryStrings.PID] != null ? Request.QueryString[QueryStrings.PID]
                        : Session[ERP.Utilities.SessionStrings.PID] != null ? Session[ERP.Utilities.SessionStrings.PID].ToString().Split('=')[1] : string.Empty;
                    ////                   
                    //Process Switching
                    if (InvoiceType == Convert.ToInt32(SalesInvoiceType.Domestic))
                        FillProcessID(1);
                    else
                        FillProcessID(3);

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

                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "DOC_SEQ_NO";
                    grdUploads.DataKeyNames = itemkeyarray;

                    //If Request From External(Report or Other page) otherthan Menu or Inbox
                    if (Request.QueryString[QueryStrings.FromExt] != null && Request.QueryString[QueryStrings.FromExt] == "T")
                    {
                        ucrWrkf.ViewType = 0;
                        EntryStatus = EntryStatus.VIEWMODE;
                        CurrPK = Convert.ToInt32(Request.QueryString[QueryStrings.PK]);
                        GetFieldValues(ControlsEnum.DRCRHEADER);
                        SetFieldValues(ControlsEnum.DRCRGET);
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
                                //  btnSave.Visible = false;                            
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
                                    hdfCancelled.Value = "1";//For Showing Cancelled Stamp in Detail Page
                            }
                            else if (pid.Equals("2") || pid.Equals("12"))
                            {
                                ucrWrkf.RefID = int.Parse(refID);
                                JournalPK = GetApplicationID(ucrWrkf.RefID);
                                GetFieldValues(ControlsEnum.GETDRCRPKBYJOURNALPK);
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
                        Session[ERP.Utilities.SessionStrings.CrDrType] = ApplicationType.SI;
                        FileDetailsList = null;
                        if (CurrPK > 0)
                        {
                            SetCancelRef(Convert.ToInt32(CurrPK));
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }

                            GetFieldValues(ControlsEnum.DRCRHEADER);
                            SetFieldValues(ControlsEnum.DRCRHEADER);
                            SetFieldValues(ControlsEnum.DRCRMPGLIST);
                            ModifiedDatePnl.Visible = true;
                        }
                        else
                        {
                            //Sets data key for the gird
                            string[] datakeyarray;
                            datakeyarray = new string[1];
                            datakeyarray[0] = Resources.DataFieldRes.CrDrPk;
                            grdCrDbHdr.DataKeyNames = datakeyarray;
                            GetFieldValues(ControlsEnum.DRCRHDRLIST);
                            SetFieldValues(ControlsEnum.DRCRHDRLIST);
                            EntryStatus = EntryStatus.LISTMODE;
                            PageIndex = "1";
                            uclPaging.TotalPages = TotalPages;
                            uclPaging.CurrentPage = 1;

                        }
                        EnableDisableDrCrMode();
                        //GetFieldValues(ControlsEnum.TAXMYR);
                        //SetFieldValues(ControlsEnum.TAXMYR);
                        //GetFieldValues(ControlsEnum.LINEITEMTAXSETTINGS);
                        //SetFieldValues(ControlsEnum.LINEITEMTAXSETTINGS);
                        if (InvoiceType == Convert.ToInt32(SalesInvoiceType.Domestic))//If Invoicetype is Domestic then exchangerate is noneditable                   
                        {
                            txtExchangeRate.Enabled = false;
                        }
                        else
                        {
                            txtExchangeRate.Enabled = true;
                        }
                        #endregion
                    }
                    if (Convert.ToInt32(hdfIsMultiplePlant.Value) == 1)
                    {
                        ddlCompanySrch.Visible = true;
                        lblCompany.Visible = true;
                    }
                    else
                    {
                        ddlCompanySrch.Visible = false;
                        lblCompany.Visible = false;
                    }

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
            }
        }

        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private int GetDOCMODE(string APT_CODE, int AST_VALUE)
        {
            CommonService cm = new CommonService();
            List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList = cm.GetReportParameters(APT_CODE, AST_VALUE, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return Convert.ToInt32(AppTypeDetailsList[0].AST_DOC_MODE) == 1 ? 1 : 0;
            }
            else
            {
                return 0;
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
            SaleOrderService saleOrderServiceClient;
            POPaymentService poPaymentServiceClient;
            FinCrDrHdrNoteService finCrDrHdrNoteServiceClient;
            finCrDrHdrNoteServiceClient = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            FinTrxService finTrxServiceClient;
            finTrxServiceClient = null;
            SalesReceiptService salesReceiptServiceClient;
            salesReceiptServiceClient = null;
            int Type = 0;
            int? Status = null;
            string invoiceNo = string.Empty;
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            AdmCompanyMstService admCompanyMstServiceClient;

            SalesInvoiceService SalesInvoiceClient;
            SalesInvoiceClient = null;
            string xmlDoc = string.Empty;
            int TotalRecords = 0;
            try
            {
                switch (type)
                {
                    #region PEDING INV LIST
                    case ControlsEnum.PEDINGINVLIST:
                        custPK = 0;
                        int.TryParse(hdfCustomerPk.Value, out custPK);
                        long.TryParse(hdfPendingInvPk.Value, out InvPk);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = 0;
                        serviceUtilityObj.PageSize = 0;
                        DateTime? fromDate = string.IsNullOrEmpty(txtPendingFromDate.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtPendingFromDate.Text.Trim());
                        DateTime? todate = string.IsNullOrEmpty(txtPendingToDate.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtPendingToDate.Text.Trim());
                        if (custPK > 0)
                            dtPendingInvList = BusinessLogic.Sales.DebitCreditBL.GetPendingInvoiceList(custPK, InvPk, Convert.ToByte(ddlPendingInvCategory.SelectedValue), Convert.ToByte(ddlPendingInvType.SelectedValue), fromDate, todate, currentUser.SBUID, serviceUtilityObj.CurrentPage, serviceUtilityObj.PageSize);
                        break;
                    #endregion                   
                    #region INVCATEGORY
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
                    #region DR/CR HEADER
                    case ControlsEnum.DRCRHEADER:
                        xmlDoc = string.Empty;
                        if (InvListObj != null && InvListObj.InvList != null && InvListObj.InvList.Count > 0)
                        {
                            xmlDoc = CommonFunctions.XmlSerialize<DebitCreditInvBO>(InvListObj);
                        }
                        debitCreditHeaderObj = BusinessLogic.Sales.DebitCreditBL.GetTradingSalesDebitCreditNote(xmlDoc, !string.IsNullOrEmpty(xmlDoc) ? 0 : CurrPK, Convert.ToByte(IsTaxForOtherCharge.Value));
                        if (DebitCreditHeaderSession == null || DebitCreditHeaderSession.DebitCreditTrxMapping == null || DebitCreditHeaderSession.DebitCreditTrxMapping.Count == 0)
                            DebitCreditHeaderSession = debitCreditHeaderObj.DeepClone();
                        else if (debitCreditHeaderObj != null)
                        {
                            List<string> objInvList = DebitCreditHeaderSession.DebitCreditTrxMapping.Select(r => r.CDM_INVOICE_CUS_HDR.ToString()).Distinct().ToList();
                            DebitCreditHeaderSession.DebitCreditTrxMapping.AddRange(debitCreditHeaderObj.DebitCreditTrxMapping.Where(r => !objInvList.Contains(r.CDM_INVOICE_CUS_HDR.ToString())).ToList());
                        }

                        #region Setting serial numbers
                        tempDebitCreditHeader = DebitCreditHeaderSession;
                        if (tempDebitCreditHeader != null)
                        {
                            int rcm_sl_no = 1;
                            tempDebitCreditHeader.DebitCreditTrxMapping.ForEach(dtl =>
                            {
                                dtl.CDM_SL_NO = rcm_sl_no;
                                int rso_sl_no = 1;
                                if (dtl.ItemDetail != null && dtl.ItemDetail.Count > 0)
                                    dtl.ItemDetail.ForEach(sompg =>
                                    {
                                        sompg.CDS_CDM_SL_NO = rcm_sl_no;
                                        sompg.CDS_SL_NO = rso_sl_no;
                                        rso_sl_no++;

                                    });
                                dtl.TaxDetail.ForEach(tax =>
                                {
                                    tax.NTD_CDM_SL_NO = rcm_sl_no;
                                });
                                rcm_sl_no++;
                            });
                        }
                        DebitCreditHeaderSession = tempDebitCreditHeader;
                        #endregion

                        break;
                    #endregion
                    #region Debit Credit Hdr List
                    case ControlsEnum.DRCRHDRLIST:
                        TotalRecords = 0;
                        int CustId = String.IsNullOrEmpty(hdfCustSearchID.Value.Trim()) ? 0 : Convert.ToInt32(hdfCustSearchID.Value);
                        int crdrPk = String.IsNullOrEmpty(hdfCrDrNumber.Value.Trim()) ? 0 : Convert.ToInt32(hdfCrDrNumber.Value);
                        Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        int cmpPk = Convert.ToInt32(ddlCompanySrch.SelectedValue);
                        invoiceNo = txtInvoiceNo.Text.Trim() != null ? txtInvoiceNo.Text.Trim() : string.Empty;
                        int cdhType = Convert.ToInt32(ddlCreditDebitType.SelectedValue) > 0 ? Convert.ToByte(ddlCreditDebitType.SelectedValue) : (byte)0;
                        dsPageData = BusinessLogic.Sales.DebitCreditBL.GetDebitCreditList(
                            new BusinessObject.GridPrams()
                            {
                                SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.CrDrDate : SortBy,
                                SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                                ThenBy = ThenBy = ThenBy == null ? Resources.DataFieldRes.CrDrNo : ThenBy,
                                ThenDirection = SortBy == ThenBy || SortBy == "CDH_NO" ? string.Empty : string.IsNullOrEmpty(ThenDirection) ? Resources.Report.SortDescending : ThenDirection,
                                FromDate = string.IsNullOrEmpty(txtSearchDateFrom.Text.Trim()) ? string.Empty : txtSearchDateFrom.Text.Trim(),
                                ToDate = string.IsNullOrEmpty(txtSearchDateTo.Text.Trim()) ? string.Empty : txtSearchDateTo.Text.Trim(),
                                SearchBy = "CDH_NO",
                                SearchValue = string.IsNullOrEmpty(txtCrDrNumber.Text.Trim()) ? string.Empty : (txtCrDrNumber.Text.Trim() == "Select/Type" ? string.Empty : txtCrDrNumber.Text.Trim()),
                                PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage,
                                PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize_CrDrList"))
                            },
                            currentUser,
                            CustId,
                            crdrPk,
                            invoiceNo,
                            Resources.PageURL.DebitCreditNoteSalesTrading.Replace("~", ""),
                            Convert.ToInt32(ddlInvoiceType.SelectedItem.Value),
                            cdhType, Convert.ToInt32(ddlStatus.SelectedValue),
                            Convert.ToInt32(ddlCompanySrch.SelectedValue));

                        if (dsPageData != null && dsPageData.Tables.Count > 0)
                        {
                            dtDebitCreditList = dsPageData.Tables[1].DefaultView.ToTable();
                            int pagsize = Convert.ToInt32(GetLocalResourceObject("PageSize_CrDrList"));
                            TotalRecords = dsPageData.Tables[0].Rows.Count > 0 ? Convert.ToInt32(dsPageData.Tables[0].Rows[0][0].ToString()) : 0;
                            TotalPages = TotalRecords == 0 ? 1 : (TotalRecords <= pagsize) ? 1 :
                                        (TotalRecords % pagsize) == 0 ? (TotalRecords / pagsize) :
                                        (TotalRecords / pagsize) + 1;
                        }
                        break;
                    #endregion

                    #region DRCRGET
                    case ControlsEnum.DRCRGET:
                        int GInvPk = Convert.ToInt32(Request.QueryString[QueryStrings.PK]);
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        finCrDrNoteHdrObj = CommonFunctions.Initilize<FIN_CRDR_NOTE_HDR>();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.FilterBy = string.Empty;
                        serviceUtilityObj.FilterValue = string.Empty;
                        finCrDrNoteHdrObj.CDH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finCrDrNoteHdrObj.CDH_VENDOR = 0;
                        finCrDrNoteHdrObj.CDH_BIZUNIT = currentUser.SBUID;
                        finCrDrNoteHdrObj.CDH_CUSTOMER = string.IsNullOrEmpty(hdfCustSearchID.Value) ? 0 : Convert.ToInt32(hdfCustSearchID.Value);
                        //if (hdfCustSearchID.Value != null && hdfCustSearchID.Value != "0" && hdfCustSearchID.Value != "")
                        //{
                        //    Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = hdfCustSearchID.Value;
                        //    Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = txtCustSearch.Text;
                        //}
                        finCrDrNoteHdrObj.CDH_PK = GInvPk;
                        finCrDrNoteHdrObj.CDH_CRTD_BY = currentUser.PKUser;
                        serviceUtilityObj.FilterDate = DateTime.MinValue;
                        serviceUtilityObj.FilterToDate = DateTime.MinValue;
                        Type = 2;
                        Status = 3;
                        invoiceNo = txtInvoiceNo.Text.Trim() != null ? txtInvoiceNo.Text.Trim() : string.Empty;
                        if (ddlInvoiceType.SelectedIndex != 0)
                        {
                            serviceUtilityObj.InvoiceType = Convert.ToInt32(ddlInvoiceType.SelectedItem.Value);
                        }
                        finCrDrNoteHdrList = finCrDrHdrNoteServiceClient.GetCrDrNoteVndHdr(finCrDrNoteHdrObj, serviceUtilityObj, Type, invoiceNo, Status);
                        TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 : (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) : (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;


                        break;
                    #endregion

                    #region Debit Credit Hdr List Journal
                    case ControlsEnum.DRCRHDRLISTJOURNAL:
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        finCrDrNoteHdrObj = CommonFunctions.Initilize<FIN_CRDR_NOTE_HDR>();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = 1;
                        serviceUtilityObj.PageSize = grdCrDbHdr.PageSize;
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.CrDrNo : SortBy;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortAscending : SortDirection;
                        serviceUtilityObj.FilterBy = string.Empty;
                        serviceUtilityObj.FilterValue = string.Empty;
                        finCrDrNoteHdrObj.CDH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finCrDrNoteHdrObj.CDH_VENDOR = 0;
                        finCrDrNoteHdrObj.CDH_CUSTOMER = string.IsNullOrEmpty(hdfCustSearchID.Value) ? 0 : Convert.ToInt32(hdfCustSearchID.Value);
                        finCrDrNoteHdrObj.CDH_PK = string.IsNullOrEmpty(hdfCrDrNumber.Value) ? 0 : Convert.ToInt64(hdfCrDrNumber.Value);
                        finCrDrNoteHdrObj.CDH_CRTD_BY = currentUser.PKUser;
                        serviceUtilityObj.FilterDate = string.IsNullOrEmpty(txtSearchDateFrom.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtSearchDateFrom.Text.Trim());
                        serviceUtilityObj.FilterToDate = string.IsNullOrEmpty(txtSearchDateTo.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtSearchDateTo.Text.Trim());
                        Type = 2;
                        invoiceNo = txtInvoiceNo.Text.Trim() != null ? txtInvoiceNo.Text.Trim() : string.Empty;
                        finCrDrNoteHdrList = finCrDrHdrNoteServiceClient.GetCrDrNoteVndHdr(finCrDrNoteHdrObj, serviceUtilityObj, Type, invoiceNo);
                        TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 : (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) : (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        InvoiceType = 1;
                        if (finCrDrNoteHdrList.Count > 0)
                        {
                            List<FIN_CRDR_NOTE_MPG> FIN_CRDR_NOTE_MPGlst = finCrDrNoteHdrList[0].FIN_CRDR_NOTE_MPG.ToList();
                            List<FIN_INVOICE_CUS_TRX_MPG> FIN_INVOICE_CUS_TRX_MPGlst = new List<FIN_INVOICE_CUS_TRX_MPG>();
                            if (finCrDrNoteHdrList.Count > 0)
                            {
                                FIN_INVOICE_CUS_TRX_MPGlst = FIN_CRDR_NOTE_MPGlst[0].FIN_INVOICE_CUS_HDR.FIN_INVOICE_CUS_TRX_MPG.ToList();
                                if (FIN_INVOICE_CUS_TRX_MPGlst.Count > 0)
                                {
                                    InvoiceType = FIN_INVOICE_CUS_TRX_MPGlst[0].SAL_ORDER_HDR.SOH_TYPE;
                                    crDrInvoiceType = Convert.ToInt32(FIN_INVOICE_CUS_TRX_MPGlst[0].FIN_INVOICE_CUS_HDR.ICH_TYPE.ToString());
                                }
                                else
                                {
                                    crDrInvoiceType = Convert.ToInt32(FIN_CRDR_NOTE_MPGlst[0].FIN_INVOICE_CUS_HDR.ICH_TYPE.ToString());
                                    InvoiceType = Convert.ToInt32(FIN_CRDR_NOTE_MPGlst[0].FIN_INVOICE_CUS_HDR.ICH_TYPE.ToString());
                                }
                            }
                        }

                        break;
                    #endregion
                    #region sales Invoice hdr List
                    case ControlsEnum.SELECTEDSIINVOICES:
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        finInvoiceCusHdrList = salesReceiptServiceClient.GetReceiptTrxMpg(selectedInvoiceList);
                        if (finInvoiceCusHdrList.Count > 0)
                        {
                            List<FIN_INVOICE_CUS_TRX_MPG> livoiceCusTrxMpgList = finInvoiceCusHdrList[0].FIN_INVOICE_CUS_TRX_MPG.ToList();
                            if (livoiceCusTrxMpgList.Count > 0)
                                InvoiceType = livoiceCusTrxMpgList[0].SAL_ORDER_HDR.SOH_TYPE;
                            else
                                InvoiceType = finInvoiceCusHdrList[0].ICH_TYPE;
                        }
                        EditedSalesInvoices = finInvoiceCusHdrList;
                        FinInvoiceCusHdrSelectedList = finInvoiceCusHdrList;
                        break;
                    #endregion
                    #region Credit Debit Details List
                    case ControlsEnum.DRCRMPGLIST:
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        finCrDrNoteMpgList = finCrDrHdrNoteServiceClient.GetCrDrTrxMpg(CurrPK);
                        EditedPaymentDtls = finPaymentVndTrxMpgList;
                        break;
                    #endregion

                    #region Get Exchange rate
                    case ControlsEnum.EXCHANGERATE:
                        DataSet dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(Convert.ToInt32(hdfInvoiceCurr.Value), currentUser.BaseCurrency, Convert.ToDateTime(txtDate.Text.Trim()));
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
                    #region Credit Debit Type
                    case ControlsEnum.CRDRTYPE:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admConfigMstObj = new ADM_CONFIG_MST();
                        admConfigMstObj.CFG_PK = 0;
                        admConfigMstObj.CFG_TYPE = "DR CR TYPE";
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        admConfigMstListCrdrType = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        admConfigMstList = admConfigMstListCrdrType.DeepClone();
                        break;
                    #endregion
                    #region Fin Transaction Year
                    case ControlsEnum.FINPERIOD:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                        finYearMstList = finTrxServiceClient.GetCurrentFinPeriod(DateTime.Now, currentUser.SBUID);
                        break;
                    #endregion
                    #region CR/DR hdr by pk
                    case ControlsEnum.GETCRDRHDRBYPK:
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        finCrDrNoteHdrList = finCrDrHdrNoteServiceClient.GetCustomerCrDrNoteHdr(CurrPK);
                        break;
                    #endregion
                    #region GETDRCRPKBYJOURNALPK

                    case ControlsEnum.GETDRCRPKBYJOURNALPK:
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
                        workflowStatusList = CommonServiceClient.GetWorkFlowStatus(ApplicationType.CNT, (byte)AppSubTypeDebitCreditNote.CUSTOMER, Convert.ToByte(CommonConstants.ACTIVE));
                        workflowStatusExportList = CommonServiceClient.GetWorkFlowStatus(ApplicationType.CNT, (byte)AppSubTypeDebitCreditNote.EXPORT_COMMERICAL, Convert.ToByte(CommonConstants.ACTIVE));
                        workflowStatusDomesticList = CommonServiceClient.GetWorkFlowStatus(ApplicationType.CNT, (byte)AppSubTypeDebitCreditNote.DOMESTIC, Convert.ToByte(CommonConstants.ACTIVE));
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
                    #region CRDRSPLITLIST
                    case ControlsEnum.CRDRSPLITLIST:
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        finCrDrCusSoMpgObj = CommonFunctions.Initilize<FIN_CRDR_NOTE_DTL>();
                        finCrDrCusSoMpgObj.CDS_CRDR_NOTE_MPG = CrDrMpgPK;
                        finCrDrCusMpgList = finCrDrHdrNoteServiceClient.GetCrDrSplitList(finCrDrCusSoMpgObj);
                        break;
                    #endregion
                    #region CRDRVNDMPGLIST
                    case ControlsEnum.CRDRVNDMPGLIST:
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        FinCrDrCusTrxMpgObj = CommonFunctions.Initilize<FIN_INVOICE_CUS_DTL>();
                        FinCrDrCusTrxMpgObj.CID_INVOICE_HDR = InvoicePK;//@@
                        FinCrDrCusTrxMpgList = finCrDrHdrNoteServiceClient.GetInvoiceTrxMpg(FinCrDrCusTrxMpgObj);
                        break;
                    #endregion
                    #region CRDRVNDHDR
                    case ControlsEnum.CRDRVNDHDR:
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        finCrDrVndHdrObjForPaymentSplit = CommonFunctions.Initilize<FIN_INVOICE_CUS_HDR>();
                        finCrDrVndHdrObjForPaymentSplit.ICH_PK = InvoicePK;
                        finCrDrVndHdrObjForPaymentSplit.ICH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finInvoiceVndHdrListForPaymentSplit = finCrDrHdrNoteServiceClient.GetInvoiceCusHdrByPK(finCrDrVndHdrObjForPaymentSplit);
                        break;
                    #endregion
                    #region TAXPOPUPGRID
                    case ControlsEnum.TAXPOPUPGRID:
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        finCrDrNoteHdrList = finCrDrHdrNoteServiceClient.GetCustomerCrDrNoteHdr(CurrPK);
                        break;
                    #endregion
                    #region TAXTYPES
                    case ControlsEnum.TAXTYPES:
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
                            if ((int)TaxType.Discount != category)
                            {
                                //Biju == InvoiceDate to Date
                                dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtDate.Text), 0, TaxFilterType.SAL, 0, 0, 1);
                            }
                            else
                            {
                                dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtDate.Text), 0);
                            }
                        }
                        break;
                    #endregion
                    #region SOTYPE
                    case ControlsEnum.SOTYPE:
                        dtSOData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SO TYPE");
                        break;
                    #endregion
                    #region TAXSETTINGS
                    case ControlsEnum.TAXSETTINGS:
                        dtTaxSettings = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ItemWiseTaxSetting, string.Empty, currentUser.SBUID);
                        break;
                    #endregion
                    #region Base Currency
                    case ControlsEnum.BASECURRENCY:
                        CurrencyMstService CurrencyMstServiceClient = new CurrencyMstService();
                        string BaseCurrency = CurrencyMstServiceClient.GetCurrencyCode(currentUser.BaseCurrency);
                        hdfBaseCurrency.Value = BaseCurrency;
                        break;
                    #endregion
                    #region Invoice Hdr By PK
                    case ControlsEnum.INVOICEDETAILS:
                        SalesInvoiceClient = new SalesInvoiceService();
                        SalesInvoiceClient = CommonFunctions.InitiateClient(SalesInvoiceClient);
                        objInvDetails = CommonFunctions.Initilize<FIN_INVOICE_CUS_HDR>();
                        objInvDetails.ICH_PK = InvPk;
                        objInvDetails.ICH_ACTIVE = 1;
                        objInvoiceDetails = SalesInvoiceClient.GetInvoiceCusHdrByPK(objInvDetails);
                        break;
                    #endregion
                    #region Invoice Item Details By Pk
                    case ControlsEnum.INVOICEITEMDETAILS:
                        SalesInvoiceClient = new SalesInvoiceService();
                        SalesInvoiceClient = CommonFunctions.InitiateClient(SalesInvoiceClient);
                        objInvItemDetails = CommonFunctions.Initilize<FIN_INVOICE_CUS_DTL>();
                        objInvItemDetails.CID_PK = InvItemPk;
                        objInvoiceItemDetails = SalesInvoiceClient.GetInvoiceCusDtlByPK(objInvItemDetails);
                        break;
                    #endregion
                    #region Get Tax List
                    case ControlsEnum.GETTAXLIST:
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        finCrDrCusMpgTaxList = finCrDrHdrNoteServiceClient.GetCrDrNoteTaxList(CurrPK);
                        break;
                    #endregion
                    #region Tax MYR
                    case ControlsEnum.TAXMYR:
                        SetUIValuesToObject(ControlsEnum.TAXMYR);
                        break;
                    #endregion
                    #region INVITEMLIST
                    case ControlsEnum.INVITEMLIST:
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        FinCrDrCusTrxMpgObj = CommonFunctions.Initilize<FIN_INVOICE_CUS_DTL>();
                        FinCrDrCusTrxMpgObj.CID_INVOICE_HDR = InvoicePK;
                        finInvItemList = finCrDrHdrNoteServiceClient.GetInvoiceTrxMpg(FinCrDrCusTrxMpgObj);
                        break;
                    #endregion
                    #region CR/DR Used in other transactions
                    case ControlsEnum.CHECKCRDRUSEDINOTHERTRNS:
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        isDrCrUsedInOtherTrns = finCrDrHdrNoteServiceClient.IsDrCrUsedInOtherTrnsactions(CurrPK);
                        break;
                    #endregion
                    #region LINE ITEM TAX SETTINGS
                    //case ControlsEnum.LINEITEMTAXSETTINGS:
                    //    isLineItemTaxEnabled = false;
                    //    dtLineItemTaxSettings = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("SALE ITEM WISE SETTINGS", string.Empty, currentUser.SBUID);
                    //    break;
                    #endregion
                    #region Get Status
                    case ControlsEnum.FINHEADERSTATUS:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        if (creditDebitType == 1)
                            finTrxHdrObj.FTH_REF_TYPE = ApplicationType.DNTJ;
                        else
                            finTrxHdrObj.FTH_REF_TYPE = ApplicationType.CNTJ;
                        finTrxHdrObj.FTH_REF_PK = CurrPK;
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrList = finTrxServiceClient.GetSatusByAppPK(finTrxHdrObj);
                        break;
                    #endregion
                    #region CR/DR ALLOCATED AMOUNT
                    //case ControlsEnum.CRDRALLOCATEDAMOUNT:
                    //    finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                    //    finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                    //    objCnsAllocationList = finCrDrHdrNoteServiceClient.GetCnsAllocationInReceipt(CurrPK);
                    //    //CrDrAllocatedAmnt = finCrDrHdrNoteServiceClient.GetCrDrAllocatedAmount(CurrPK, creditDebitType);
                    //    break;
                    #endregion
                    #region BALANCE AMOUNT SPLIT
                    case ControlsEnum.BALANCEAMOUNTSPLIT:
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        objCrdrAllocations = finCrDrHdrNoteServiceClient.GetBalanceAmountSplitSales(CurrPK);
                        break;
                    #endregion
                    #region CANCEL_DR/CR_CHECK
                    case ControlsEnum.DRCRCANCELCHECK:
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        finReceiptCRDRmpgObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_CRDR_MPG>();
                        finReceiptCRDRmpgObj.RNM_CRDR_HDR = CurrPK;
                        finReceiptCRDRmpgList = finCrDrHdrNoteServiceClient.CheckDRCRforCancel(finReceiptCRDRmpgObj);
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
                poPaymentServiceClient = null;
                finCrDrHdrNoteServiceClient = null;
                finTrxServiceClient = null;
                salesReceiptServiceClient = null;
                CommonServiceClient = null;
                admCompanyMstServiceClient = null;
                SalesInvoiceClient = null;
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
                        BindGrid(controlType);
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
                    #region DR/CR HEADER
                    case ControlsEnum.DRCRHEADER:
                        GetUIValuesFromObject(controlType);
                        break;
                    #endregion
                    #region DR/CR MPG LIST
                    case ControlsEnum.DRCRMPGLIST:
                        BindGrid(controlType);
                        break;
                    #endregion

                    #region DR CR GET
                    case ControlsEnum.DRCRGET:
                        GetUIValuesFromObject(controlType);
                        break;
                    #endregion
                    #region Company
                    case ControlsEnum.COMPANY:
                        //Bind Company List in DDL
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    #endregion
                    #region DR CR HDR LIST
                    case ControlsEnum.DRCRHDRLIST:
                        BindGrid(ControlsEnum.DRCRHDRLIST);
                        break;
                    #endregion
                    #region SELECTED SI INVOICES
                    case ControlsEnum.SELECTEDSIINVOICES:
                        BindGrid(controlType);
                        break;
                    #endregion
                    #region CR DR SPLIT LIST
                    case ControlsEnum.CRDRSPLITLIST:
                        BindGrid(ControlsEnum.CRDRSPLITLIST);
                        break;
                    #endregion
                    #region Credit Debit Type
                    case ControlsEnum.CRDRTYPE:
                        BindDropDown(ControlsEnum.CRDRTYPE);
                        break;
                    #endregion
                    #region TAX POPUP GRID
                    case ControlsEnum.TAXPOPUPGRID://Biju
                        ddlPopupTaxType.Focus();
                        BindGrid(controlType);
                        break;
                    #endregion
                    //#region TAX POPUP GRID TEMP
                    //case ControlsEnum.TAXPOPUPGRIDTEMP://Biju
                    //    ddlPopupTaxType.Focus();
                    //    BindGrid(controlType);
                    //    break;
                    //#endregion
                    #region TAX TYPES
                    case ControlsEnum.TAXTYPES:
                        BindDropDown(controlType);
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
                    #region SO TYPE
                    case ControlsEnum.SOTYPE:
                        BindDropDown(controlType);
                        break;
                    #endregion
                    #region TAX MYR
                    case ControlsEnum.TAXMYR:
                        BindGrid(ControlsEnum.TAXMYR);
                        break;
                    #endregion
                    #region TAX SPLIT UP
                    case ControlsEnum.TAXSPLITUP:
                        BindGrid(ControlsEnum.TAXSPLITUP);
                        break;
                    #endregion
                    #region TAX SPLIT UP LINE ITEM WISE
                    case ControlsEnum.TAXSPLITUPLINEITEMWISE:
                        BindGrid(ControlsEnum.TAXSPLITUPLINEITEMWISE);
                        break;
                    #endregion
                    #region LINE ITEM TAX SETTINGS
                    //case ControlsEnum.LINEITEMTAXSETTINGS:
                    //    GetUIValuesFromObject(ControlsEnum.LINEITEMTAXSETTINGS);
                    //    break;
                    #endregion
                    #region BALANCE AMOUNT SPLIT
                    case ControlsEnum.BALANCEAMOUNTSPLIT:
                        BindGrid(ControlsEnum.BALANCEAMOUNTSPLIT);
                        break;
                    #endregion
                    #region FILEUPLOAD
                    case ControlsEnum.FILEUPLOAD:
                        BindGrid(ControlsEnum.FILEUPLOAD);
                        break;
                    #endregion

                    #region UPDATE GRID VAL TO OBJECT
                    case ControlsEnum.UPDATEGRIDVALTOOBJECT:
                        SetUIValuesToObject(ControlsEnum.DRCRMPGENTRY);
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
        #region EnableDisableDrCrMode
        /// <summary>
        /// Enable or disable dropdown list for Mode (debit/credit)
        /// </summary>
        private void EnableDisableDrCrMode()
        {
            if (InvCategory == (int)SalesInvoiceCategory.Advanced)
            {
                imgShippingCharge.Visible = false;
                txtShipCharge.Enabled = false;
                ddlMode.Enabled = false;
            }
            else
            {
                imgShippingCharge.Visible = true;
                txtShipCharge.Enabled = true;
                ddlMode.Enabled = true;
                if (DebitCreditHeaderSession != null)
                {
                    if (DebitCreditHeaderSession.CDH_STATUS == (int)WkfStatusEnum.DRAFTED)
                    {
                        ddlMode.Enabled = true;
                    }
                    else
                    {
                        ddlMode.Enabled = false;
                    }
                }
            }
        }
        #endregion
        #region Helper Methods

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
                HiddenField hdfCrDbMpgPK;
                hdfCrDbMpgPK = null;
                HiddenField hdfTaxPK;
                hdfTaxPK = null;
                HiddenField hdfInvoicePK;
                hdfInvoicePK = null;
                TextBox txtAmount;
                Label lblTotalTax;
                LinkButton lbnTotalTax;
                bool bIsChecked = false;

                HiddenField hdfDCSplitPK = null;
                HiddenField hdfSOPK = null;
                HiddenField hdfInvCusDtlPK = null;
                HiddenField hdfSODtlPk;
                TextBox txtSumSplit;
                TextBox txtQtySplit;
                TextBox txtRateSplit;
                SalesInvoice SalesInvoiceClient;
                SalesInvoiceClient = null;
                int InvDtlPk = 0;
                int SODtlPk = 0;
                switch (controlType)
                {

                    #region Debit Credit Hdr
                    case ControlsEnum.DRCRHEADERENTRY:
                        if (DebitCreditHeaderSession != null)
                        {
                            debitCreditHeaderObj = new DebitCreditHeader();
                            debitCreditHeaderObj = DebitCreditHeaderSession;

                            debitCreditHeaderObj.CDH_PK = CurrPK;
                            debitCreditHeaderObj.CDH_NO = (string.IsNullOrEmpty(lblDrCrNo.Text) || lblDrCrNo.Text.Trim().Equals("[NEW]")) ? string.Empty
                                                        : lblDrCrNo.Text.Trim();
                            debitCreditHeaderObj.CDH_DATE = txtDate.Text.Trim(); //String.IsNullOrEmpty(txtDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtDate.Text.Trim()); 
                            debitCreditHeaderObj.CDH_VENDOR = null;
                            debitCreditHeaderObj.CDH_CUSTOMER = hdfCustomerPk.Value;
                            debitCreditHeaderObj.CDH_VND_CUS_ACCOUNT = string.IsNullOrEmpty(hdfVendorAccountNo.Value) ? null : hdfVendorAccountNo.Value;
                            debitCreditHeaderObj.CDH_TYPE = Convert.ToByte(ddlMode.SelectedValue);
                            debitCreditHeaderObj.CDH_CURRENCY = string.IsNullOrEmpty(hdfInvoiceCurr.Value) ? 1 : Convert.ToInt32(hdfInvoiceCurr.Value);
                            debitCreditHeaderObj.CDH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                            debitCreditHeaderObj.CDH_REMARKS2 = HttpUtility.HtmlEncode(txtRemarks2.Text.Trim());
                            debitCreditHeaderObj.CDH_BASE_CURR = currentUser.BaseCurrency;
                            debitCreditHeaderObj.CDH_EXCHG_RATE = string.IsNullOrEmpty(hdfExchangeRate.Value) ? 1 : Convert.ToDouble(hdfExchangeRate.Value);
                            debitCreditHeaderObj.CDH_STATUS = WkfStatus;
                            debitCreditHeaderObj.CDH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            debitCreditHeaderObj.TaxHeader = DebitCreditHeaderTax;
                            debitCreditHeaderObj.DebitCreditTrxMapping = (List<DebitCreditTrxMapping>)SetUIValuesToObject(ControlsEnum.DRCRMPGENTRY);
                            debitCreditHeaderObj.CDH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                            debitCreditHeaderObj.CDH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                            debitCreditHeaderObj.USER_PK = Convert.ToInt16(currentUser.PKUser);
                            debitCreditHeaderObj.CDH_MOD_DT = LastModifiedTime;
                            debitCreditHeaderObj.CDH_REF_NO = HttpUtility.HtmlDecode(txtInstrumentNo.Text.Trim());
                            debitCreditHeaderObj.CDH_SHIP_CHARGE = txtShipCharge.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtShipCharge.Text.Trim());
                            debitCreditHeaderObj.CDH_OTHER_CHARGE = Convert.ToDecimal(txtOtherCharge.Text.Trim());
                            debitCreditHeaderObj.CDH_IS_DELETED = Convert.ToBoolean(0);
                            debitCreditHeaderObj.CDH_NO_RCP_ALLOC = chkIsReceiptAlcnReq.Checked ? (byte)1 : (byte)0;
                            debitCreditHeaderObj.AST_VALUE = hdfInvoiceType.Value == "1" ? (int)AppSubTypeCNSales.DOMESTIC : (int)AppSubTypeCNSales.EXPORT_COMMERICAL;
                            if (Convert.ToInt32(ddlMode.SelectedValue) == (int)DebitCreditModeEnum.DEBIT)
                            {
                                debitCreditHeaderObj.APT_CODE = ApplicationType.DNT;
                            }
                            else if (Convert.ToInt32(ddlMode.SelectedValue) == (int)DebitCreditModeEnum.CREDIT)
                            {
                                debitCreditHeaderObj.APT_CODE = ApplicationType.CNT;
                            }
                            debitCreditHeaderObj.AST_DOC_MODE = GetDOCMODE(debitCreditHeaderObj.APT_CODE, debitCreditHeaderObj.AST_VALUE);

                            if (String.IsNullOrEmpty(txtInstrumentDate.Text.Trim()))
                                debitCreditHeaderObj.CDH_REF_DATE = null;
                            else
                                debitCreditHeaderObj.CDH_REF_DATE = txtInstrumentDate.Text.Trim();
                            debitCreditHeaderObj.CDH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);
                            if (debitCreditHeaderObj.DebitCreditTrxMapping != null && debitCreditHeaderObj.DebitCreditTrxMapping.Count > 0)
                            {
                                debitCreditHeaderObj.CDH_TAX_AMOUNT = Math.Round(debitCreditHeaderObj.DebitCreditTrxMapping.Sum(c => c.CDM_TAX_AMOUNT), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                debitCreditHeaderObj.CDH_AMOUNT_TC = Math.Round(debitCreditHeaderObj.DebitCreditTrxMapping.Sum(c => c.CDM_AMOUNT), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                debitCreditHeaderObj.CDH_AMOUNT_BC = Math.Round(debitCreditHeaderObj.DebitCreditTrxMapping.Sum(c => c.CDM_AMOUNT) * Convert.ToDecimal(debitCreditHeaderObj.CDH_EXCHG_RATE), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            }
                            debitCreditHeaderObj.FileList = DocAttachList;
                        }
                        retObject = debitCreditHeaderObj;
                        break;
                    #endregion
                    #region Credit Debit Maping
                    case ControlsEnum.DRCRMPGENTRY:
                        decimal TotalInvCrdrAmnt = 0;
                        if (grdInvoiceList.Rows.Count > 0)
                        {
                            HiddenField hdfCrdrFooter = (HiddenField)grdInvoiceList.FooterRow.FindControl("hdfPayNowFooter");
                            decimal.TryParse(hdfCrdrFooter.Value, out TotalInvCrdrAmnt);

                            tempDebitCreditHeader = DebitCreditHeaderSession;
                            foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                            {
                                decimal taxpercentage = 0;
                                decimal basevalue = 0;
                                decimal ttaxamt = 0;
                                decimal hdftax = 0;
                                decimal hdftotalamt = 0;
                                int ItemIncluded = 0;
                                decimal RaiseNote = 0;
                                hdfInvoicePK = (HiddenField)grdrow.FindControl(GetLocalResourceObject("hdfInvoicePK").ToString());
                                DebitCreditTrxMapping objTrxMapping = tempDebitCreditHeader.DebitCreditTrxMapping.SingleOrDefault(r => r.CDM_INVOICE_CUS_HDR == Convert.ToInt32(hdfInvoicePK.Value));
                                if (objTrxMapping != null)
                                {

                                    hdfCrDbMpgPK = (HiddenField)grdrow.FindControl(GetLocalResourceObject("hdfCrDbMpgPK").ToString());
                                    objTrxMapping.CDM_PK = hdfCrDbMpgPK == null ? 0 : Convert.ToInt64(hdfCrDbMpgPK.Value);
                                    objTrxMapping.CDM_CRDR_NOTE_HDR = CurrPK;
                                    objTrxMapping.CDM_INVOICE_CUS_HDR = hdfInvoicePK == null ? 0 : Convert.ToInt32(hdfInvoicePK.Value);
                                    txtAmount = (TextBox)grdrow.FindControl(GetLocalResourceObject("txtNoteFor").ToString());
                                    decimal.TryParse(txtAmount.Text, out RaiseNote);
                                    //if (RaiseNote == 0)
                                    //{
                                    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_RaiseNote").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    //    break;
                                    //}
                                    objTrxMapping.CDM_AMOUNT = txtAmount.Text != string.Empty ? Convert.ToDecimal(txtAmount.Text) : 0;
                                    objTrxMapping.CDM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                    HiddenField hdfTotalTax = (HiddenField)grdrow.FindControl("hdfTotalTax");
                                    HiddenField hdfTaxAmt = (HiddenField)grdrow.FindControl("hdfTaxAmt");
                                    hdftax = hdfTaxAmt.Value != string.Empty ? Convert.ToDecimal(hdfTaxAmt.Value) : 0;
                                    HiddenField hdfTotalAmt = (HiddenField)grdrow.FindControl("hdfTotalAmt");
                                    hdftotalamt = hdfTotalAmt.Value != string.Empty ? Convert.ToDecimal(hdfTotalAmt.Value) : 0;
                                    taxpercentage = hdftax / (hdftotalamt == 0 ? 1 : hdftotalamt);
                                    basevalue = (txtAmount.Text != string.Empty ? Convert.ToDecimal(txtAmount.Text) : 0) / (1 + taxpercentage);
                                    ttaxamt = (txtAmount.Text != string.Empty ? Convert.ToDecimal(txtAmount.Text) : 0 - basevalue);
                                    lbnTotalTax = (LinkButton)grdrow.FindControl("lbnTotalTax");
                                    objTrxMapping.CDM_TAX_AMOUNT = Math.Round(Convert.ToDecimal(hdfTotalTax.Value), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                    HiddenField hdfItemIncluded = (HiddenField)grdrow.FindControl("hdfItemIncluded");
                                    int.TryParse(hdfItemIncluded.Value, out ItemIncluded);
                                    if (ItemIncluded == 1)
                                    {
                                        objTrxMapping.CDM_AMOUNT = objTrxMapping.CDM_AMOUNT + objTrxMapping.CDM_TAX_AMOUNT;
                                    }

                                    decimal shipCharge = txtShipCharge.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtShipCharge.Text.Trim());
                                    decimal otherCharge = txtOtherCharge.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtOtherCharge.Text.Trim());
                                    decimal ivnShipCharge = 0;
                                    decimal ivnOtherCharge = 0;
                                    if (TotalInvCrdrAmnt > 0)
                                    {
                                        ivnShipCharge = (shipCharge / TotalInvCrdrAmnt) * objTrxMapping.CDM_AMOUNT;
                                        ivnOtherCharge = (otherCharge / TotalInvCrdrAmnt) * objTrxMapping.CDM_AMOUNT;
                                    }
                                    objTrxMapping.CDM_SHIP_CHARGE = Math.Round(ivnShipCharge, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                    objTrxMapping.CDM_OTHER_CHARGE = Math.Round(ivnOtherCharge, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                                    if (objTrxMapping.CDM_AMOUNT > 0)
                                    {
                                        //adding header tax
                                        if (objTrxMapping.CDM_TAX_AMOUNT > 0)
                                        {
                                            decimal InvAmount = objTrxMapping.CDM_AMOUNT;
                                            decimal TotalTaxPercentage = 1;
                                            List<DebitCreditTaxDtl> objInvoiceTaxList = objTrxMapping.TaxDetail.Where(tax => tax.NTD_TAX_CATEGORY == (byte)TaxType.Tax && tax.NTD_VTL_INVOICE_DTL == 0).ToList();
                                            if (objInvoiceTaxList != null && objInvoiceTaxList.Count > 0)
                                            {
                                                TotalTaxPercentage = objInvoiceTaxList.Sum(tx => (tx.NTD_VTL_TAX_AMT * 100) / InvAmount);
                                                foreach (DebitCreditTaxDtl invtaxdet in objInvoiceTaxList)
                                                {
                                                    if (invtaxdet.NTD_VTL_TAX_AMT > 0)
                                                    {
                                                        decimal IndividualPercentage = (invtaxdet.NTD_VTL_TAX_AMT * 100) / InvAmount;
                                                        decimal TaxAmnt = (objTrxMapping.CDM_TAX_AMOUNT * IndividualPercentage) / TotalTaxPercentage;
                                                        double Amount = CommonFunctions.DoubleFormatRound(Convert.ToDouble(TaxAmnt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                                        invtaxdet.NTD_AMOUNT = Convert.ToDecimal(Amount);
                                                        invtaxdet.NTD_CDS_SL_NO = 0;
                                                    }
                                                }
                                            }

                                            //InvPk = Convert.ToInt64(hdfInvoicePK.Value);
                                            //GetFieldValues(ControlsEnum.INVOICEDETAILS);
                                            //if (objInvoiceDetails.Count > 0)
                                            //{
                                            //    if (objInvoiceDetails[0] != null)
                                            //    {
                                            //        decimal InvAmount = objInvoiceDetails[0].ICH_AMOUNT_TC;
                                            //        decimal TotalTaxPercentage = objInvoiceDetails[0].FIN_INVOICE_CUS_TAX_HDR.Where(txx => txx.ISH_TAX_CATEGORY == (byte)TaxType.Tax).Sum(tx => (tx.ISH_TAX_AMT * 100) / (InvAmount == 0 ? 1 : InvAmount));
                                            //        List<FIN_INVOICE_CUS_TAX_HDR> TaxList = objInvoiceDetails[0].FIN_INVOICE_CUS_TAX_HDR.Where(txx => txx.ISH_TAX_CATEGORY == (byte)TaxType.Tax).ToList();
                                            //        foreach (FIN_INVOICE_CUS_TAX_HDR invtaxhdr in TaxList)
                                            //        {
                                            //            FIN_CRDR_NOTE_TAX_DTL ObjTaxDtl = new FIN_CRDR_NOTE_TAX_DTL();
                                            //            decimal IndividualPercentage = (invtaxhdr.ISH_TAX_AMT * 100) / (InvAmount == 0 ? 1 : InvAmount);
                                            //            decimal TaxAmnt = (objTrxMapping.CDM_TAX_AMOUNT * IndividualPercentage) / TotalTaxPercentage;
                                            //            double Amount = CommonFunctions.DoubleFormatRound(Convert.ToDouble(TaxAmnt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                            //            ObjTaxDtl.NTD_AMOUNT = Convert.ToDecimal(Amount);
                                            //            ObjTaxDtl.NTD_PK = 0;
                                            //            ObjTaxDtl.NTD_TAX = invtaxhdr.ISH_TAX;
                                            //            objTrxMapping.FIN_CRDR_NOTE_TAX_DTL.Add(ObjTaxDtl);
                                            //        }
                                            //    }
                                            //}
                                        }
                                    }

                                    //if (objTrxMapping.CDM_AMOUNT > 0)
                                    //{
                                    //    //adding header tax
                                    //    if (objTrxMapping.CDM_TAX_AMOUNT > 0)
                                    //    {
                                    //        InvPk = Convert.ToInt64(hdfInvoicePK.Value);
                                    //        GetFieldValues(ControlsEnum.INVOICEDETAILS);
                                    //        if (objInvoiceDetails.Count > 0)
                                    //        {
                                    //            if (objInvoiceDetails[0] != null)
                                    //            {
                                    //                decimal InvAmount = objInvoiceDetails[0].ICH_AMOUNT_TC;
                                    //                decimal TotalTaxPercentage = objInvoiceDetails[0].FIN_INVOICE_CUS_TAX_HDR.Where(txx => txx.ISH_TAX_CATEGORY == (byte)TaxType.Tax).Sum(tx => (tx.ISH_TAX_AMT * 100) / (InvAmount == 0 ? 1 : InvAmount));
                                    //                List<FIN_INVOICE_CUS_TAX_HDR> TaxList = objInvoiceDetails[0].FIN_INVOICE_CUS_TAX_HDR.Where(txx => txx.ISH_TAX_CATEGORY == (byte)TaxType.Tax).ToList();
                                    //                foreach (FIN_INVOICE_CUS_TAX_HDR invtaxhdr in TaxList)
                                    //                {
                                    //                    FIN_CRDR_NOTE_TAX_DTL ObjTaxDtl = new FIN_CRDR_NOTE_TAX_DTL();
                                    //                    decimal IndividualPercentage = (invtaxhdr.ISH_TAX_AMT * 100) / (InvAmount == 0 ? 1 : InvAmount);
                                    //                    decimal TaxAmnt = (objTrxMapping.CDM_TAX_AMOUNT * IndividualPercentage) / TotalTaxPercentage;
                                    //                    double Amount = CommonFunctions.DoubleFormatRound(Convert.ToDouble(TaxAmnt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                    //                    ObjTaxDtl.NTD_AMOUNT = Convert.ToDecimal(Amount);
                                    //                    ObjTaxDtl.NTD_PK = 0;
                                    //                    ObjTaxDtl.NTD_TAX = invtaxhdr.ISH_TAX;
                                    //                    objTrxMapping.FIN_CRDR_NOTE_TAX_DTL.Add(ObjTaxDtl);
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //    /////////////////
                                    //    finCrDrNoteMpgList.Add(objTrxMapping);
                                    //}
                                }
                            }
                        }
                        DebitCreditHeaderSession = tempDebitCreditHeader;
                        if (tempDebitCreditHeader != null && tempDebitCreditHeader.DebitCreditTrxMapping != null)
                            retObject = tempDebitCreditHeader.DebitCreditTrxMapping.ToList();
                        else
                            retObject = new DebitCreditTrxMapping();
                        break;
                    #endregion
                    #region Journalize
                    case ControlsEnum.JOURNALIZE:
                        if (DebitCreditHeaderSession != null)
                        {
                            if (DebitCreditHeaderSession.CDH_STATUS == 2)
                            {
                                Session[ERP.Utilities.SessionStrings.JournalMode] = null;
                                Session[ERP.Utilities.SessionStrings.TrxPK] = null;
                                GetFieldValues(ControlsEnum.DRCRHDRLISTJOURNAL);
                                if (DebitCreditHeaderSession.CDH_TYPE == (int)DebitCreditModeEnum.DEBIT)
                                {
                                    ucrJournalize.TransactionType = ApplicationType.DNTJ;
                                    Session[ERP.Utilities.SessionStrings.TransactionType] = ApplicationType.DNTJ;
                                    Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.DNTJ;
                                    Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.DNTJ;
                                    Session[ERP.Utilities.SessionStrings.JournalHead] = hdfJournalHeader.Value = GetLocalResourceObject("Debit_Note_Journal").ToString();
                                }
                                else if (DebitCreditHeaderSession.CDH_TYPE == (int)DebitCreditModeEnum.CREDIT)
                                {
                                    ucrJournalize.TransactionType = ApplicationType.CNTJ;
                                    Session[ERP.Utilities.SessionStrings.TransactionType] = ApplicationType.CNTJ;
                                    Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.CNTJ;
                                    Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.CNTJ;
                                    Session[ERP.Utilities.SessionStrings.JournalHead] = hdfJournalHeader.Value = GetLocalResourceObject("Credit_Note_Journal").ToString();
                                }
                                ucrJournalize.TransactionPK = (int)CurrPK;
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                                ucrJournalize.JournalizePK = 0;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = DebitCreditHeaderSession.CDH_NO;
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = DebitCreditHeaderSession.CDH_DATE;
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = DebitCreditHeaderSession.CDH_CURRENCY;
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.AP;
                                Session[ERP.Utilities.SessionStrings.AccountPayablePK] = DebitCreditHeaderSession.CDH_CUSTOMER;

                                //Journalize New sessions start
                                Session[ERP.Utilities.SessionStrings.DrControls] = null;
                                Session[ERP.Utilities.SessionStrings.CrControls] = null;
                                Session[ERP.Utilities.SessionStrings.RemovedControls] = null;
                                Session[ERP.Utilities.SessionStrings.AccountType] = null;
                                Session[ERP.Utilities.SessionStrings.ControlInfo] = null;
                                Session[ERP.Utilities.SessionStrings.FinTrxPk] = null;
                                //Journalize New sessions End

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
                                SetCancelRef(Convert.ToInt32(CurrPK));
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
                                ucrJournalize.TypeForNumberGenaration = hdfInvoiceType.Value == "1" ? ((int)AppSubTypeCNSales.DOMESTIC).ToString() : ((int)AppSubTypeCNSales.EXPORT_COMMERICAL).ToString();
                                ucrJournalize.CallUserControl();

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                            }
                            else
                            {
                                if (isPosted == false)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_PickPaymentPost_Msg").ToString();
                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }

                        }
                        else
                        {
                            if (Approved != 2)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Journalize_Msg").ToString();
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region DR/CR SPLIT LIST
                    case ControlsEnum.CRDRSPLITLIST:
                        //rowID = 0;
                        finCrDrCusMpgList = new List<FIN_CRDR_NOTE_DTL>();
                        tempDebitCreditHeader = DebitCreditHeaderSession;
                        DebitCreditTrxMapping objTrxMpg = tempDebitCreditHeader.DebitCreditTrxMapping.SingleOrDefault(r => r.CDM_INVOICE_CUS_HDR == InvoicePK);
                        if (objTrxMpg != null && objTrxMpg.ItemDetail.Count > 0)
                        {
                            foreach (GridViewRow grdrow in grdDCSplit.Rows)
                            {
                                InvDtlPk = 0;
                                SODtlPk = 0;
                                hdfInvCusDtlPK = (HiddenField)grdrow.FindControl("hdfInvCusDtlPK");
                                hdfSODtlPk = (HiddenField)grdrow.FindControl("hdfSODtlPk");
                                int.TryParse(hdfInvCusDtlPK.Value, out InvDtlPk);
                                int.TryParse(hdfSODtlPk.Value, out SODtlPk);
                                DebitCreditDetails objDtl = objTrxMpg.ItemDetail.SingleOrDefault(r => r.CDS_INVOICE_CUS_DTL == InvDtlPk && r.CDS_SO_DTL == SODtlPk);
                                if (objDtl != null)
                                {
                                    //finCrDrCusSoMpgObj = CommonFunctions.Initilize<FIN_CRDR_NOTE_DTL>();
                                    //hdfDCSplitPK = (HiddenField)grdrow.FindControl("hdfDCSplitPK");
                                    //finCrDrCusSoMpgObj.CDS_PK = hdfDCSplitPK == null ? 0 : Convert.ToInt64(hdfDCSplitPK.Value);
                                    //finCrDrCusSoMpgObj.CDS_CRDR_NOTE_HDR = CurrPK;
                                    //finCrDrCusSoMpgObj.CDS_CRDR_NOTE_MPG = CrDrMpgPK;
                                    //finCrDrCusSoMpgObj.CDS_INVOICE_VND_DTL = null;
                                    //finCrDrCusSoMpgObj.CDS_PO_DTL = null;                                   
                                    //finCrDrCusSoMpgObj.CDS_INVOICE_CUS_DTL = hdfInvCusDtlPK == null ? 1 : Convert.ToInt32(hdfInvCusDtlPK.Value);
                                    //finCrDrCusSoMpgObj.CDS_SO_DTL = null;

                                    txtQtySplit = (TextBox)grdrow.FindControl("txtQtySplit");
                                    objDtl.CDS_QTY = txtQtySplit == null ? 0 : txtQtySplit.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(txtQtySplit.Text.Trim());
                                    //finCrDrCusSoMpgObj.CDS_UOM = null;
                                    txtRateSplit = (TextBox)grdrow.FindControl("txtRateSplit");
                                    objDtl.CDS_RATE = txtRateSplit == null ? 0 : txtRateSplit.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(txtRateSplit.Text.Trim());
                                    txtSumSplit = (TextBox)grdrow.FindControl("txtSumSplit");
                                    objDtl.CDS_AMOUNT = txtSumSplit == null ? 0 : txtSumSplit.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtSumSplit.Text.Trim());
                                    //finCrDrCusSoMpgObj.CDS_DISCOUNT = 0;
                                    //finCrDrCusSoMpgObj.CDS_TAX = 0;
                                    HiddenField hdfTAXSplitTotal = (HiddenField)grdrow.FindControl("hdfTAXSplitTotal");
                                    objDtl.CDS_TAX = string.IsNullOrEmpty(hdfTAXSplitTotal.Value) ? 0 : Convert.ToDecimal(hdfTAXSplitTotal.Value.Trim());
                                    //finCrDrCusSoMpgObj.CDS_NET_AMOUNT = Convert.ToDecimal(txtSumSplit.Text.Trim()) - (finCrDrCusSoMpgObj.CDS_DISCOUNT + finCrDrCusSoMpgObj.CDS_TAX);
                                    decimal SplitSum = txtSumSplit == null ? 0 : txtSumSplit.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtSumSplit.Text.Trim());
                                    objDtl.CDS_NET_AMOUNT = SplitSum - (objDtl.CDS_DISCOUNT + objDtl.CDS_TAX);
                                    objDtl.CDS_REMARKS = null;
                                    objDtl.CDS_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                    //finCrDrCusMpgList.Add(finCrDrCusSoMpgObj);
                                    //rowID++;
                                    #region Tax  Amount Splitup Calculation
                                    if (objDtl.CDS_TAX > 0)
                                    {
                                        decimal InvAmount = objDtl.CDS_CID_AMOUNT;
                                        decimal TotalTaxPercentage = 1;
                                        //List<DebitCreditTaxDtl> objInvoiceTaxList = objDtl.TaxDetail.Where(inv => inv.NTD_TAX_CATEGORY == (byte)TaxType.Tax).ToList();
                                        List<DebitCreditTaxDtl> objInvoiceTaxList = objTrxMpg.TaxDetail.Where(tax => tax.NTD_TAX_CATEGORY == (byte)TaxType.Tax && tax.NTD_VTL_INVOICE_DTL == InvDtlPk).ToList();
                                        if (objInvoiceTaxList != null && objInvoiceTaxList.Count > 0)
                                        {
                                            TotalTaxPercentage = objInvoiceTaxList.Sum(tx => (tx.NTD_VTL_TAX_AMT * 100) / InvAmount);
                                            foreach (DebitCreditTaxDtl invtaxdet in objInvoiceTaxList)
                                            {
                                                if (invtaxdet.NTD_VTL_TAX_AMT > 0)
                                                {
                                                    decimal IndividualPercentage = (invtaxdet.NTD_VTL_TAX_AMT * 100) / InvAmount;
                                                    decimal TaxAmnt = (objDtl.CDS_TAX * IndividualPercentage) / TotalTaxPercentage;
                                                    double Amount = CommonFunctions.DoubleFormatRound(Convert.ToDouble(TaxAmnt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                                    invtaxdet.NTD_AMOUNT = Convert.ToDecimal(Amount);
                                                    invtaxdet.NTD_CDS_SL_NO = objDtl.CDS_SL_NO;
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                        DebitCreditHeaderSession = tempDebitCreditHeader;
                        retObject = tempDebitCreditHeader;
                        break;
                    #endregion
                    #region TAXMYR
                    case ControlsEnum.TAXMYR:

                        FinCrDrHdrNoteService finCrDrHdrNoteServiceClient;
                        finCrDrHdrNoteServiceClient = null;
                        taxTempList = new List<SOInvoiceTaxHdr>();
                        taxList = new List<SOInvoiceTaxHdr>();
                        rowID = 0;
                        decimal ExngeRate = 1;
                        decimal.TryParse(txtExchangeRate.Text.Replace(",", ""), out ExngeRate);
                        bool isItemIncluded = false;
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            decimal taxpercentage = 0;
                            decimal basevalue = 0;
                            decimal ttaxamt = 0;
                            decimal hdftax = 0;
                            decimal hdftotalamt = 0;
                            finCrDrNoteMpgObj = CommonFunctions.Initilize<FIN_CRDR_NOTE_MPG>();
                            hdfCrDbMpgPK = (HiddenField)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("hdfCrDbMpgPK").ToString());
                            finCrDrNoteMpgObj.CDM_PK = hdfCrDbMpgPK == null ? 0 : Convert.ToInt64(hdfCrDbMpgPK.Value);
                            finCrDrNoteMpgObj.CDM_CRDR_NOTE_HDR = CurrPK;
                            hdfInvoicePK = (HiddenField)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("hdfInvoicePK").ToString());
                            finCrDrNoteMpgObj.CDM_INVOICE_CUS_HDR = hdfInvoicePK == null ? 0 : Convert.ToInt32(hdfInvoicePK.Value);
                            finCrDrNoteMpgObj.CDM_INVOICE_VND_HDR = null;
                            txtAmount = (TextBox)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("txtNoteFor").ToString());
                            finCrDrNoteMpgObj.CDM_AMOUNT = txtAmount.Text != string.Empty ? Convert.ToDecimal(txtAmount.Text) : 0;
                            finCrDrNoteMpgObj.CDM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            HiddenField hdfTotalTax = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTotalTax");
                            decimal tax = 0;
                            decimal.TryParse(hdfTotalTax.Value, out tax);

                            HiddenField hdfItemIcluded = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfItemIncluded");

                            HiddenField hdfTaxAmt = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTaxAmt");
                            hdftax = hdfTaxAmt.Value != string.Empty ? Convert.ToDecimal(hdfTaxAmt.Value) : 0;
                            HiddenField hdfTotalAmt = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTotalAmt");
                            hdftotalamt = hdfTotalAmt.Value != string.Empty ? Convert.ToDecimal(hdfTotalAmt.Value) : 0;
                            taxpercentage = hdftax / (hdftotalamt == 0 ? 1 : hdftotalamt);
                            basevalue = Convert.ToDecimal(txtAmount.Text) / (((1 + taxpercentage) == 0) ? 1 : (1 + taxpercentage));
                            ttaxamt = Convert.ToDecimal(txtAmount.Text) - basevalue;
                            //if (string.IsNullOrEmpty(hdfTotalTax.Value))
                            //{
                            tax = ttaxamt;
                            //}

                            int iteminc = 0;
                            int.TryParse(hdfItemIcluded.Value, out iteminc);
                            if (iteminc > 0)
                            {
                                isItemIncluded = true;
                                tax = Convert.ToDecimal(txtAmount.Text) * taxpercentage;
                            }

                            //lblTotalTax = (Label)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("lblTotalTax").ToString());
                            lbnTotalTax = (LinkButton)grdInvoiceList.Rows[rowID].FindControl("lbnTotalTax");
                            //finCrDrNoteMpgObj.CDM_TAX_AMOUNT = Math.Round(Convert.ToDecimal(ttaxamt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            finCrDrNoteMpgObj.CDM_TAX_AMOUNT = Math.Round(tax, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                            if (finCrDrNoteMpgObj.CDM_AMOUNT > 0)
                            {
                                //adding detail tax
                                if (finCrDrNoteMpgObj.CDM_TAX_AMOUNT > 0)
                                {
                                    InvPk = Convert.ToInt64(hdfInvoicePK.Value);
                                    GetFieldValues(ControlsEnum.INVOICEDETAILS);
                                    if (objInvoiceDetails.Count > 0)
                                    {
                                        if (objInvoiceDetails[0] != null)
                                        {
                                            decimal InvAmount = objInvoiceDetails[0].ICH_AMOUNT_TC;
                                            decimal TotalTaxPercentage = 1;
                                            List<FIN_INVOICE_CUS_TAX_HDR> objInvoiceTaxList = objInvoiceDetails[0].FIN_INVOICE_CUS_TAX_HDR.Where(inv => inv.ISH_TAX_CATEGORY == (byte)TaxType.Tax).ToList();
                                            if (objInvoiceTaxList != null && objInvoiceTaxList.Count > 0)
                                            {
                                                TotalTaxPercentage = objInvoiceTaxList.Sum(tx => (tx.ISH_TAX_AMT * 100) / (InvAmount == 0 ? 1 : InvAmount));
                                                foreach (FIN_INVOICE_CUS_TAX_HDR invtaxhdr in objInvoiceTaxList)
                                                {
                                                    SOInvoiceTaxHdr ObjTaxDtl = new SOInvoiceTaxHdr();
                                                    decimal IndividualPercentage = (invtaxhdr.ISH_TAX_AMT * 100) / (InvAmount == 0 ? 1 : InvAmount);
                                                    decimal TaxAmnt = (finCrDrNoteMpgObj.CDM_TAX_AMOUNT * IndividualPercentage) / (TotalTaxPercentage == 0 ? 1 : TotalTaxPercentage);
                                                    TaxAmnt = TaxAmnt * ExngeRate;
                                                    double Amount = Math.Round(Convert.ToDouble(TaxAmnt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                                    ObjTaxDtl.CIT_TAX = Convert.ToInt32(invtaxhdr.ISH_TAX);
                                                    ObjTaxDtl.CIT_TAX_AMT = Amount;
                                                    ObjTaxDtl.CIT_TAX_CATEGORY = invtaxhdr.ISH_TAX_CATEGORY;
                                                    ObjTaxDtl.CIT_TAX_CID_AMOUNT = (finCrDrNoteMpgObj.CDM_AMOUNT * ExngeRate).ToString();
                                                    if (invtaxhdr.FIN_TAX_MST != null)
                                                    {
                                                        ObjTaxDtl.CIT_TAX_CODE = invtaxhdr.FIN_TAX_MST.TAX_CODE;
                                                        ObjTaxDtl.CIT_TAX_RATE = Convert.ToDouble(invtaxhdr.FIN_TAX_MST.TAX_RATE);
                                                        ObjTaxDtl.CIT_TAX_TEXT = invtaxhdr.FIN_TAX_MST.TAX_HEAD;
                                                    }
                                                    taxTempList.Add(ObjTaxDtl);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            rowID++;
                        }

                        if (grdDCSplit.Rows.Count >= 1)
                        {
                            HiddenField lblTotalPayNowFooterSplit = (HiddenField)(grdDCSplit.FooterRow.FindControl("hdfTotalPayNowFooterSplit"));
                            if (lblTotalPayNowFooterSplit != null && !string.IsNullOrEmpty(lblTotalPayNowFooterSplit.Value) && !string.IsNullOrEmpty(lblDCSplitReceiveNow.Text))
                            {
                                //Convert.ToDecimal(lblTotalPayNowFooterSplit.Value) == Convert.ToDecimal(lblDCSplitReceiveNow.Text.Trim().Replace(",", "")) - Convert.ToDecimal(lbltaxSplitpopup.Text.Trim().Replace(",", ""))
                                if (Convert.ToDecimal(lblTotalPayNowFooterSplit.Value) == Convert.ToDecimal(lblDCSplitReceiveNow.Text.Trim().Replace(",", "")))
                                {
                                    divErrorLabel.Visible = false;
                                    finCrDrCusMpgList = new List<FIN_CRDR_NOTE_DTL>();
                                    finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                                    finCrDrHdrNoteServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                                    finCrDrCusMpgList = (List<FIN_CRDR_NOTE_DTL>)SetUIValuesToObject(ControlsEnum.CRDRSPLITLIST);
                                    finCrDrHdrNoteServiceClient = null;
                                }
                            }
                        }
                        else if (CurrPK > 0)
                        {
                            GetFieldValues(ControlsEnum.GETCRDRHDRBYPK);
                            if (finCrDrNoteHdrList != null && finCrDrNoteHdrList.Count > 0)
                            {
                                finCrDrCusMpgList = finCrDrNoteHdrList[0].FIN_CRDR_NOTE_DTL.ToList();
                            }
                        }

                        if (finCrDrCusMpgList != null && finCrDrCusMpgList.Count > 0)
                        {
                            foreach (FIN_CRDR_NOTE_DTL invtaxdethdr in finCrDrCusMpgList)
                            {
                                if (invtaxdethdr.CDS_TAX > 0)
                                {
                                    InvItemPk = Convert.ToInt64(invtaxdethdr.CDS_INVOICE_CUS_DTL);// Invoice Item PK;
                                    GetFieldValues(ControlsEnum.INVOICEITEMDETAILS);
                                    if (objInvoiceItemDetails != null && objInvoiceItemDetails.Count > 0)
                                    {
                                        decimal InvAmount = objInvoiceItemDetails[0].CID_AMOUNT;
                                        decimal TotalTaxPercentage = 1;
                                        List<FIN_INVOICE_CUS_TAX_DTL> objInvoiceTaxList = objInvoiceItemDetails[0].FIN_INVOICE_CUS_TAX_DTL.Where(inv => inv.CIT_TAX_CATEGORY == (byte)TaxType.Tax).ToList();
                                        if (objInvoiceTaxList != null && objInvoiceTaxList.Count > 0)
                                        {
                                            TotalTaxPercentage = objInvoiceTaxList.Sum(tx => (tx.CIT_TAX_AMT * 100) / (InvAmount == 0 ? 1 : InvAmount));
                                            foreach (FIN_INVOICE_CUS_TAX_DTL invtaxdet in objInvoiceTaxList)
                                            {
                                                SOInvoiceTaxHdr ObjTaxDtl = new SOInvoiceTaxHdr();
                                                decimal IndividualPercentage = (invtaxdet.CIT_TAX_AMT * 100) / (InvAmount == 0 ? 1 : InvAmount);
                                                decimal TaxAmnt = (invtaxdethdr.CDS_TAX * IndividualPercentage) / (TotalTaxPercentage == 0 ? 1 : TotalTaxPercentage);
                                                TaxAmnt = TaxAmnt * ExngeRate;
                                                double Amount = Math.Round(Convert.ToDouble(TaxAmnt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                                ObjTaxDtl.CIT_TAX = Convert.ToInt32(invtaxdet.CIT_TAX);
                                                ObjTaxDtl.CIT_TAX_AMT = Amount;
                                                ObjTaxDtl.CIT_TAX_CATEGORY = invtaxdet.CIT_TAX_CATEGORY;
                                                ObjTaxDtl.CIT_TAX_CID_AMOUNT = (invtaxdethdr.CDS_AMOUNT * ExngeRate).ToString();
                                                if (invtaxdet.FIN_TAX_MST != null)
                                                {
                                                    ObjTaxDtl.CIT_TAX_CODE = invtaxdet.FIN_TAX_MST.TAX_CODE;
                                                    ObjTaxDtl.CIT_TAX_RATE = Convert.ToDouble(invtaxdet.FIN_TAX_MST.TAX_RATE);
                                                    ObjTaxDtl.CIT_TAX_TEXT = invtaxdet.FIN_TAX_MST.TAX_HEAD;
                                                }
                                                taxTempList.Add(ObjTaxDtl);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (taxTempList != null && taxTempList.Count > 0)
                        {
                            var finPymntTaxHdr = from finObj in taxTempList
                                                 group finObj by new
                                                 {
                                                     finObj.CIT_TAX
                                                 } into finGrpdObj
                                                 select new SOInvoiceTaxHdr
                                                 {
                                                     CIT_TAX = finGrpdObj.Key.CIT_TAX,
                                                     CIT_TAX_AMT = finGrpdObj.Sum(x => x.CIT_TAX_AMT),
                                                     CIT_TAX_CATEGORY = finGrpdObj.FirstOrDefault().CIT_TAX_CATEGORY,
                                                     CIT_TAX_CID_AMOUNT = finGrpdObj.Sum(x => Convert.ToDecimal(x.CIT_TAX_CID_AMOUNT)).ToString(),
                                                     CIT_TAX_CODE = finGrpdObj.FirstOrDefault().CIT_TAX_CODE,
                                                     CIT_TAX_RATE = finGrpdObj.FirstOrDefault().CIT_TAX_RATE,
                                                     CIT_TAX_TEXT = finGrpdObj.FirstOrDefault().CIT_TAX_TEXT

                                                 };
                            taxList = finPymntTaxHdr.ToList();
                            if (!isItemIncluded)
                            {
                                taxList.ForEach(tx =>
                                                    {
                                                        tx.CIT_TAX_CID_AMOUNT = (Convert.ToDecimal(tx.CIT_TAX_CID_AMOUNT) - Convert.ToDecimal(tx.CIT_TAX_AMT)).ToString();
                                                    }
                                                );
                            }
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
            int dept = 0;
            bool isCancelled = false;
            try
            {
                switch (controlType)
                {
                    #region DR/CR HEADER
                    case ControlsEnum.DRCRHEADER:
                        if (DebitCreditHeaderSession != null && grdInvoiceList.Rows.Count <= 0)
                        {
                            if (CurrPK > 0)
                            {
                                lblDrCrNo.ToolTip = lblDrCrNo.Text = string.IsNullOrEmpty(DebitCreditHeaderSession.CDH_NO) ? Resources.ErpRes.Draft : DebitCreditHeaderSession.CDH_NO;
                                txtDate.Text = Convert.ToDateTime(DebitCreditHeaderSession.CDH_DATE).ToString(Resources.Constants.DateFormatShort);
                                txtCustomer.Text = HttpUtility.HtmlDecode(DebitCreditHeaderSession.CDH_CUSTOMER_TEXT);
                                hdfCustomerPk.Value = DebitCreditHeaderSession.CDH_CUSTOMER.ToString();
                                DocAttachList = DebitCreditHeaderSession.FileList;
                                DebitCreditHeaderTax = DebitCreditHeaderSession.TaxHeader;
                                txtRemarks.Text = HttpUtility.HtmlDecode(DebitCreditHeaderSession.CDH_REMARKS);
                            }

                            InvCategory = DebitCreditHeaderSession.CDH_ICH_CATEGORY;
                            InvoiceType = DebitCreditHeaderSession.CDH_ICH_TYPE;
                            hdfInvoiceType.Value = DebitCreditHeaderSession.CDH_ICH_TYPE.ToString();
                            if (DebitCreditHeaderSession.CDH_TYPE > 0)
                                ddlMode.SelectedValue = DebitCreditHeaderSession.CDH_TYPE.ToString();
                            txtPaidAmount.Text = Math.Round(DebitCreditHeaderSession.CDH_AMOUNT_TC, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            txtInstrumentNo.ToolTip = txtInstrumentNo.Text = DebitCreditHeaderSession.CDH_REF_NO;
                            //txtRemarks.Text = HttpUtility.HtmlDecode(DebitCreditHeaderSession.CDH_REMARKS);
                            txtRemarks2.Text = HttpUtility.HtmlDecode(DebitCreditHeaderSession.CDH_REMARKS2);
                            LastModifiedTime = DebitCreditHeaderSession.CDH_MOD_DT;
                            lblLastModifiedHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);
                            Approved = WkfStatus = DebitCreditHeaderSession.CDH_STATUS;
                            ddlCompany.SelectedValue = DebitCreditHeaderSession.CDH_COMPANY.ToString();
                            txtExchangeRate.Text = DebitCreditHeaderSession.CDH_EXCHG_RATE.ToString();
                            txtShipCharge.ToolTip = txtShipCharge.Text = Math.Round(DebitCreditHeaderSession.CDH_SHIP_CHARGE, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            txtOtherCharge.ToolTip = txtOtherCharge.Text = Math.Round(DebitCreditHeaderSession.CDH_OTHER_CHARGE, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            decimal ShpCharge = 0;
                            decimal OtherCharge = 0;
                            ShpCharge = txtShipCharge.Text == string.Empty ? 0 : Convert.ToDecimal(txtShipCharge.Text);
                            OtherCharge = Convert.ToDecimal(txtOtherCharge.Text);
                            txtHdrTotal.ToolTip = txtHdrTotal.Text = (ShpCharge + OtherCharge + Convert.ToDecimal(txtPaidAmount.Text.Trim())).ToString();
                            txtInstrumentDate.Text = string.IsNullOrEmpty(DebitCreditHeaderSession.CDH_REF_DATE) ? string.Empty : Convert.ToDateTime(DebitCreditHeaderSession.CDH_REF_DATE).ToString(Resources.Constants.DateFormatShort);
                            if (Convert.ToInt32(hdfIsMultiplePlant.Value) == 1)
                            {
                                lblCompanyView.Visible = true;
                                ddlCompanyView.Visible = true;
                                ddlCompanyView.Enabled = true;
                                ddlCompanyView.SelectedValue = DebitCreditHeaderSession.CDH_COMPANY.ToString();
                                ddlCompanyView.Enabled = false;
                            }
                            else
                            {
                                lblCompanyView.Visible = false;
                                ddlCompanyView.Visible = false;
                            }
                            chkIsReceiptAlcnReq.Checked = DebitCreditHeaderSession.CDH_NO_RCP_ALLOC == 1 ? true : false;
                            hdfVendorPK.Value = DebitCreditHeaderSession.CDH_CUSTOMER.ToString();
                            hdfVendorAccountNo.Value = string.IsNullOrEmpty(DebitCreditHeaderSession.CDH_VND_CUS_ACCOUNT) ? string.Empty : DebitCreditHeaderSession.CDH_VND_CUS_ACCOUNT;
                            hdfInvoiceCurr.Value = hdfPaymentCurrency.Value = DebitCreditHeaderSession.CDH_CURRENCY.ToString();
                            txtPaymentCurrency.Text = DebitCreditHeaderSession.CDH_CURRENCY_TEXT;
                            lblPaidAmount.Text = GetLocalResourceObject("Amount").ToString() + " (" + DebitCreditHeaderSession.CDH_CURRENCY_TEXT + ")";
                            GetFieldValues(ControlsEnum.EXCHANGERATE);
                        }
                        break;
                    #endregion

                    #region DR CR GET
                    case ControlsEnum.DRCRGET:
                        if (DebitCreditHeaderSession != null )
                        {
                            CurrPK = Convert.ToInt32(Request.QueryString["PK"].ToString());
                            InvoiceType = DebitCreditHeaderSession.CDH_ICH_TYPE;
                            if (!string.IsNullOrEmpty(DebitCreditHeaderSession.CDH_DEPT.ToString()) && int.TryParse(DebitCreditHeaderSession.CDH_DEPT.ToString(), out dept))
                            {
                                Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                base.SetUserDept();
                            }
                            isCancelled = Convert.ToBoolean(DebitCreditHeaderSession.CDH_IS_DELETED);
                            if (isCancelled)
                            {
                                btnSave.Visible = false;
                                btnEditforCancel.Visible = false;
                                btnEdit.Visible = false;
                                btnSaveSubmit.Visible = false;
                                hdfCancelled.Value = "1";
                            }
                            else
                            {
                                hdfCancelled.Value = "0";
                                btnSave.Visible = true;
                                if (DebitCreditHeaderSession.CDH_NO != "[NEW]")
                                {
                                    btnEditforCancel.Visible = true;
                                }
                                else
                                {
                                    btnEditforCancel.Visible = false;
                                }
                            }
                            //WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            //base.WkfRefID = workflowCore.GetRefID((int)CurrPK, PageProcessID);
                            ////                          

                            FinCrDrNoteTaxHeader = null;
                            FinCrDrNoteTaxHeaderTemp = null;
                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = DebitCreditHeaderSession.CDH_CUSTOMER;
                            Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = DebitCreditHeaderSession.CDH_CUSTOMER_TEXT;
                            Approved = DebitCreditHeaderSession.CDH_STATUS;
                            hdfCancelled.Value = "0";
                            isCancelled = Convert.ToBoolean(DebitCreditHeaderSession.CDH_IS_DELETED);
                            if (isCancelled)
                                hdfCancelled.Value = "1";
                            // Get CrDrDetails
                            //GetFieldValues(ControlsEnum.DRCRHDRLISTJOURNAL);
                            ActionsEnum Mode = ActionsEnum.VIEW;
                            if (Mode != ActionsEnum.EDITFORCANCEL)
                            {
                                if (InvoiceType == Convert.ToInt32(SalesInvoiceType.Domestic))
                                    FillProcessID(1);
                                else
                                    FillProcessID(3);
                            }
                            SetFieldValues(ControlsEnum.DRCRHEADER);
                            SetFieldValues(ControlsEnum.DRCRMPGLIST);
                            BindGrid(ControlsEnum.FILEUPLOAD);

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
                            SetCancelRef(Convert.ToInt32(CurrPK));
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
                           
                            if (InvoiceType == Convert.ToInt32(SalesInvoiceType.Domestic))//If Invoicetype is Domestic then exchangerate is noneditable                   
                            {
                                txtExchangeRate.Enabled = false;
                            }
                            else
                            {
                                txtExchangeRate.Enabled = true;
                            }
                            btnCancel.Focus();
                        }
                        break;
                    #endregion
                    #region Payment Header
                    case ControlsEnum.DRCRHEADERENTRY:
                        if (finCrDrNoteHdrList != null && finCrDrNoteHdrList.Count > 0)
                        {
                            CrDrSplitList = finCrDrNoteHdrList[0].FIN_CRDR_NOTE_DTL.ToList();
                            FinCrDrNoteTaxHeader = finCrDrNoteHdrList[0].FIN_CRDR_NOTE_TAX_HDR.ToList();
                            lblDrCrNo.ToolTip = lblDrCrNo.Text = finCrDrNoteHdrList[0].CDH_NO == "" ? "[NEW]" : finCrDrNoteHdrList[0].CDH_NO;
                            txtDate.Text = finCrDrNoteHdrList[0].CDH_DATE.ToString(Resources.Constants.DateFormatShort);
                            ddlMode.SelectedValue = finCrDrNoteHdrList[0].CDH_TYPE.ToString();
                            txtPaidAmount.Text = Math.Round(finCrDrNoteHdrList[0].CDH_AMOUNT_TC, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            txtRemarks.Text = HttpUtility.HtmlDecode(finCrDrNoteHdrList[0].CDH_REMARKS);
                            txtRemarks2.Text = HttpUtility.HtmlDecode(finCrDrNoteHdrList[0].CDH_REMARKS2);
                            LastModifiedTime = finCrDrNoteHdrList[0].CDH_MOD_DT;
                            lblLastModifiedHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);
                            Approved = WkfStatus = finCrDrNoteHdrList[0].CDH_STATUS;
                            ddlCompany.SelectedValue = finCrDrNoteHdrList[0].CDH_COMPANY.ToString();
                            txtInstrumentNo.ToolTip = txtInstrumentNo.Text = finCrDrNoteHdrList[0].CDH_REF_NO;
                            txtExchangeRate.Text = finCrDrNoteHdrList[0].CDH_EXCHG_RATE.ToString();
                            txtShipCharge.ToolTip = txtShipCharge.Text = Math.Round(finCrDrNoteHdrList[0].CDH_SHIP_CHARGE, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();// finCrDrNoteHdrList[0].CDH_SHIP_CHARGE.ToString();
                            txtOtherCharge.ToolTip = txtOtherCharge.Text = Math.Round(finCrDrNoteHdrList[0].CDH_OTHER_CHARGE, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(); //finCrDrNoteHdrList[0].CDH_OTHER_CHARGE.ToString();
                            decimal ShpCharge = 0;
                            decimal OtherCharge = 0;
                            ShpCharge = txtShipCharge.Text == string.Empty ? 0 : Convert.ToDecimal(txtShipCharge.Text);
                            OtherCharge = Convert.ToDecimal(txtOtherCharge.Text);
                            txtHdrTotal.ToolTip = txtHdrTotal.Text = (ShpCharge + OtherCharge + Convert.ToDecimal(txtPaidAmount.Text.Trim())).ToString();
                            txtInstrumentDate.Text = finCrDrNoteHdrList[0].CDH_REF_DATE.HasValue == false ? "" :
                                finCrDrNoteHdrList[0].CDH_REF_DATE.Value.ToString(Resources.Constants.DateFormatShort);
                            if (Convert.ToInt32(hdfIsMultiplePlant.Value) == 1)
                            {
                                lblCompanyView.Visible = true;
                                ddlCompanyView.Visible = true;
                                ddlCompanyView.Enabled = true;
                                ddlCompanyView.SelectedValue = finCrDrNoteHdrList[0].CDH_COMPANY.ToString();
                                ddlCompanyView.Enabled = false;
                            }
                            else
                            {
                                lblCompanyView.Visible = false;
                                ddlCompanyView.Visible = false;
                            }
                            chkIsReceiptAlcnReq.Checked = finCrDrNoteHdrList[0].CDH_NO_RCP_ALLOC == 1 ? true : false;
                        }
                        break;
                    #endregion
                    #region CRDRSPLITLIST
                    case ControlsEnum.CRDRSPLITLIST:
                        if (DebitCreditHeaderSession != null && DebitCreditHeaderSession.DebitCreditTrxMapping.Count > 0)
                        {
                            DebitCreditTrxMapping objTrxMpg = DebitCreditHeaderSession.DebitCreditTrxMapping.SingleOrDefault(r => r.CDM_INVOICE_CUS_HDR == InvoicePK);
                            if (objTrxMpg != null)
                            {
                                lblDCSplitNo.Text = lblDCSplitNo.ToolTip = ERP.Utilities.CommonFunctions.GetDecodedString(objTrxMpg.ICH_NO);
                                lblDCSplitDate.Text = lblDCSplitDate.ToolTip = objTrxMpg.ICH_DATE;
                                lblDCSplitSupplier.Text = ERP.Utilities.CommonFunctions.GetShortString(objTrxMpg.ICH_CUSTOMER_TEXT, 45);
                                lblDCSplitSupplier.ToolTip = ERP.Utilities.CommonFunctions.GetDecodedString(objTrxMpg.ICH_CUSTOMER_TEXT);
                                lblDCSplitAmount.Text = lblDCSplitAmount.ToolTip = GetFormattedCurrencyWithComma(objTrxMpg.ICH_AMOUNT_TC);
                                lblDCSplitReceiveNow.Text = lblDCSplitReceiveNow.ToolTip = GetFormattedCurrencyWithComma(PayNowAmount);
                                lbltaxSplitpopup.Text = GetFormattedCurrencyWithComma(paynowtax);
                            }
                        }
                        break;
                    #endregion
                    #region LINE ITEM TAX SETTINGS
                    //case ControlsEnum.LINEITEMTAXSETTINGS:
                    //    isLineItemTaxEnabled = false;
                    //    if (dtLineItemTaxSettings != null && dtLineItemTaxSettings.Rows.Count > 0)
                    //    {
                    //        DataRow drTaxSettings = dtLineItemTaxSettings.AsEnumerable().SingleOrDefault(aa => aa.Field<string>("ACF_DATA").Trim() == "TAX");
                    //        if (drTaxSettings != null)
                    //        {
                    //            int configval = Convert.ToInt32(drTaxSettings["ACF_VALUE"]);
                    //            if (configval == 1)
                    //            {
                    //                isLineItemTaxEnabled = true;
                    //            }
                    //        }
                    //    }
                    //    break;
                    #endregion
                    #region SELECTEDDOC
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
                                anchorFile.Attributes.Add("class", "removedownloadClass");
                            }
                            else
                            {
                                anchorFile.Attributes.Add("onclick", "return true;");
                                anchorFile.Attributes.Add("class", "downloadClass");
                            }
                        }
                        break;
                    #endregion
                    #region CUSTOMER DETAILS
                    case ControlsEnum.CUSTOMERDETAILS:
                        if (dsCustomerData != null && dsCustomerData.Tables != null && dsCustomerData.Tables.Count > 0)
                        {
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

        ///// <summary>
        ///// 
        ///// </summary>
        private void ConfigurationSettings()
        {
            int intEnableItemTax = 0;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetCurrencyConfiguration("CURRENCY SETTINGS", "ShowAmountInBC", currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfIsTaxPayable.Value = dt.Rows[0]["ACF_VALUE"].ToString();//If 1 Show Div taxpayable else hide  
                int.TryParse(hdfIsTaxPayable.Value, out intEnableItemTax);
                if (intEnableItemTax == 0)
                {
                    EnableItemTax = false;
                }
                else
                {
                    EnableItemTax = true;
                }
            }
            //**********Enable/Disable common ExchangeRate for all Bizunits*****************
            DataTable dtConfig = BusinessLogic.CommonManagement.CommonBL.GetCurrencyConfiguration("BIZUNIT SETTINGS", "CURRENCY", currentUser.SBUID);
            if (dtConfig != null && dtConfig.Rows.Count > 0)
            {
                hdfExRateBizUnit.Value = dtConfig.Rows[0]["ACF_VALUE"].ToString();//If 1, no BizUnit Filteration
            }

            // Check Whether OtherCharge is needed or not  for Tax Calculation     
            IsTaxForOtherCharge.Value = (GetGlobalResourceObject("ConfigurationsRes", "OtherChargeTaxPurchase")).ToString();

            #region Advance Invoice Tax Settings
            DataTable dtTax = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("ADVANCE INVOICE SETTINGS", "TAX");
            if (dtTax != null && dtTax.Rows.Count > 0)
            {
                IsAdvInvHasTax = dtTax.Rows[0]["ACF_VALUE"].ToString() == "1" ? true : false;
                hdfIsAdvInvHasTax.Value = dtTax.Rows[0]["ACF_VALUE"].ToString();
            }
            #endregion
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";

            #region Line Item Tax Settings
            dtLineItemTaxSettings = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("SALE ITEM WISE SETTINGS", string.Empty, currentUser.SBUID);
            isLineItemTaxEnabled = true;
            if (dtLineItemTaxSettings != null && dtLineItemTaxSettings.Rows.Count > 0)
            {
                DataRow drTaxSettings = dtLineItemTaxSettings.AsEnumerable().SingleOrDefault(aa => aa.Field<string>("ACF_DATA").Trim() == "TAX");
                if (drTaxSettings != null)
                {
                    isLineItemTaxEnabled = Convert.ToString(drTaxSettings["ACF_VALUE"]) == "1" ? true : false;
                    hdfIsLineItemTaxEnabled.Value = Convert.ToString(drTaxSettings["ACF_VALUE"]);
                }
            }
            #endregion

        }

        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region SOTYPE
                case ControlsEnum.SOTYPE:
                    ddlInvoiceType.Items.Clear();
                    ddlPendingInvType.Items.Clear();
                    if (dtSOData != null && dtSOData.Rows.Count > 0)
                    {
                        ddlInvoiceType.DataSource = dtSOData;
                        ddlInvoiceType.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlInvoiceType.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlInvoiceType.DataBind();

                        ddlPendingInvType.DataSource = dtSOData;
                        ddlPendingInvType.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlPendingInvType.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlPendingInvType.DataBind();
                    }
                    ddlInvoiceType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
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
                    ddlPopupTaxType.Items.Add(new ListItem(Resources.Report.Custom, CommonConstants.SELECTVAL));
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

                        ddlCompanySrch.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecsCode);
                        ddlCompanySrch.DataTextField = Resources.DataFieldRes.CompanySpecsCode;
                        ddlCompanySrch.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompanySrch.DataBind();

                        ddlCompanyView.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                        ddlCompanyView.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlCompanyView.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompanyView.DataBind();
                    }
                    ddlCompanySrch.Items.Insert(0, new ListItem(Resources.ErpRes.SelectAll, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Credit Debit Type
                case ControlsEnum.CRDRTYPE:
                    ddlMode.Items.Clear();
                    if (admConfigMstList != null && admConfigMstList.Count > 0)
                    {
                        if (SelectedInvoicesCrDr != null && SelectedInvoicesCrDr.Count > 1)
                        {
                            admConfigMstList.RemoveAll((x => x.CFG_VALUE == (int)DebitCreditModeEnum.DEBIT)); //Remove Debit while choosing multiple sales invoice
                        }
                        ddlMode.DataSource = admConfigMstList;
                        ddlMode.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlMode.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlMode.DataBind();
                    }
                    ddlMode.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
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
                    #region DR/CR MPG LIST
                    case ControlsEnum.DRCRMPGLIST:

                        if (DebitCreditHeaderSession != null
                            && DebitCreditHeaderSession.DebitCreditTrxMapping != null
                            && DebitCreditHeaderSession.DebitCreditTrxMapping.Count > 0)
                        {
                            grdInvoiceList.DataSource = DebitCreditHeaderSession.DebitCreditTrxMapping.ToList();
                            grdInvoiceList.DataBind();

                            if (DebitCreditHeaderSession.CDH_ICH_CATEGORY == (int)SalesInvoiceCategory.Advanced)
                            {
                                ddlMode.SelectedValue = ((int)DebitCreditModeEnum.CREDIT).ToString();
                                //grdInvoiceList.Columns[16].Visible = false;
                                grdInvoiceList.Columns[7].Visible = true;
                            }
                            else
                            {
                                //grdInvoiceList.Columns[16].Visible = true;
                                grdInvoiceList.Columns[7].Visible = false;
                            }

                            string Remarks = string.Empty;
                            int newline = 0;
                            if (CurrPK == 0)
                            {
                                foreach (DebitCreditTrxMapping item in DebitCreditHeaderSession.DebitCreditTrxMapping)
                                {
                                    Remarks = Remarks + item.ICH_REMARKS + ((DebitCreditHeaderSession.DebitCreditTrxMapping.Count == 1 || newline == DebitCreditHeaderSession.DebitCreditTrxMapping.Count - 1) ? string.Empty : Environment.NewLine);
                                    newline++;
                                }
                                txtRemarks.Text = HttpUtility.HtmlDecode(Remarks.Trim());
                            }

                            ddlPendingInvCategory.SelectedValue = DebitCreditHeaderSession.CDH_ICH_GROUP == (int)SalesInvoiceGroup.Miscellaneous ? DebitCreditHeaderSession.CDH_ICH_GROUP.ToString() : DebitCreditHeaderSession.CDH_ICH_CATEGORY.ToString();
                            ddlPendingInvType.SelectedValue = DebitCreditHeaderSession.CDH_ICH_TYPE.ToString();                           
                        }
                        else
                        {
                            grdInvoiceList.DataSource = null;
                            grdInvoiceList.DataBind();                           
                        }
                        break;
                    #endregion

                    #region DR CR HDR LIST
                    case ControlsEnum.DRCRHDRLIST:
                        uclPaging.TotalPages = TotalPages;
                        PageIndex = string.IsNullOrEmpty(PageIndex) ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        if (dtDebitCreditList != null && dtDebitCreditList.Rows.Count > 0)
                        {
                            grdCrDbHdr.PageIndex = Convert.ToInt32(PageIndex);
                            grdCrDbHdr.DataSource = dtDebitCreditList.DefaultView;
                            grdCrDbHdr.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            grdCrDbHdr.DataSource = null;
                            grdCrDbHdr.DataBind();
                            uclPaging.Visible = false;
                        }
                        break;
                    #endregion
                    #region SELECTED SI INVOICES
                    case ControlsEnum.SELECTEDSIINVOICES:
                        if (finInvoiceCusHdrList != null)
                        {
                            grdInvoiceList.DataSource = finInvoiceCusHdrList;
                            grdInvoiceList.DataBind();
                            if (finInvoiceCusHdrList.Count > 0)
                            {
                                hdfVendorPK.Value = finInvoiceCusHdrList[0].ICH_CUSTOMER.ToString();
                                hdfVendorAccountNo.Value = finInvoiceCusHdrList[0].ICH_CUSTOMER_ACCOUNT.ToString();
                                hdfInvoiceCurr.Value = finInvoiceCusHdrList[0].ICH_CURRENCY.ToString();
                                lblPaidAmount.Text = GetLocalResourceObject("Amount").ToString() + " (" + finInvoiceCusHdrList[0].ADM_CURRENCY_MST1.CUR_CODE + ")";
                                GetFieldValues(ControlsEnum.EXCHANGERATE);
                                InvCategory = finInvoiceCusHdrList[0].ICH_CATEGORY;
                                if (InvCategory == (int)SalesInvoiceCategory.Advanced)
                                {
                                    ddlMode.SelectedValue = ((int)DebitCreditModeEnum.CREDIT).ToString();
                                    //grdInvoiceList.Columns[16].Visible = false;
                                    grdInvoiceList.Columns[7].Visible = true;
                                }
                                else
                                {
                                    //grdInvoiceList.Columns[16].Visible = true;
                                    grdInvoiceList.Columns[7].Visible = false;
                                }
                            }
                            else
                            {
                                txtPaidAmount.Text = "0.00";
                            }

                            string Remarks = string.Empty;
                            int newline = 1;
                            foreach (FIN_INVOICE_CUS_HDR item in finInvoiceCusHdrList)
                            {
                                Remarks = Remarks + item.ICH_REFERENCE + " " + (item.ICH_DATE_PAY_BY != null ? (string.IsNullOrEmpty(item.ICH_DATE_PAY_BY.ToString()) == true ? string.Empty : item.ICH_DATE_PAY_BY.ToString(Resources.Constants.DateFormatShort)) : string.Empty) + (newline == finInvoiceCusHdrList.Count ? string.Empty : Environment.NewLine);
                                newline++;
                            }
                            //if (VendorNos != string.Empty)
                            //{
                            //    VendorNos = VendorNos.Remove(VendorNos.Length - 1);
                            //}
                            txtRemarks.Text = HttpUtility.HtmlDecode(Remarks);
                        }
                        else if (finCrDrNoteMpgList != null)
                        {
                            grdInvoiceList.DataSource = finCrDrNoteMpgList;
                            grdInvoiceList.DataBind();
                            if (finCrDrNoteMpgList != null && finCrDrNoteMpgList.Count > 0)
                            {
                                try
                                {
                                    if (finCrDrNoteMpgList.Where(mpg => mpg.FIN_CRDR_NOTE_DTL.Sum(dtl => dtl.CDS_AMOUNT) > 0).Count() > 0)
                                    {
                                        hdfSplitCount.Value = "1";
                                    }
                                    else
                                    {
                                        hdfSplitCount.Value = "0";
                                    }
                                }
                                catch { hdfSplitCount.Value = "0"; }
                                if (finCrDrNoteMpgList[0].FIN_INVOICE_CUS_HDR != null)
                                {
                                    hdfVendorPK.Value = finCrDrNoteMpgList[0].FIN_INVOICE_CUS_HDR.ICH_CUSTOMER.ToString();
                                    hdfVendorAccountNo.Value = finCrDrNoteMpgList[0].FIN_INVOICE_CUS_HDR.ICH_CUSTOMER_ACCOUNT.ToString();
                                    hdfInvoiceCurr.Value = finCrDrNoteMpgList[0].FIN_INVOICE_CUS_HDR.ICH_CURRENCY.ToString();
                                    lblPaidAmount.Text = GetLocalResourceObject("Amount").ToString() + " (" + finCrDrNoteMpgList[0].FIN_INVOICE_CUS_HDR.ADM_CURRENCY_MST1.CUR_CODE + ")";
                                    //GetFieldValues(ControlsEnum.EXCHANGERATE);
                                    hdfExchangeRate.Value = finCrDrNoteMpgList[0].FIN_CRDR_NOTE_HDR.CDH_EXCHG_RATE.ToString();
                                    //Exchange rate to Textbox
                                    txtExchangeRate.Text = hdfExchangeRate.Value.ToString();
                                    InvCategory = finCrDrNoteMpgList[0].FIN_INVOICE_CUS_HDR.ICH_CATEGORY;
                                    if (InvCategory == (int)SalesInvoiceCategory.Advanced)
                                    {
                                        //grdInvoiceList.Columns[16].Visible = false;
                                        grdInvoiceList.Columns[7].Visible = true;
                                    }
                                    else
                                    {
                                        //grdInvoiceList.Columns[16].Visible = true;
                                        grdInvoiceList.Columns[7].Visible = false;
                                    }
                                }

                                foreach (var CrDr in finCrDrNoteMpgList)
                                {
                                    //Add Invoices
                                    if (SelectedInvoicesCrDr != null)
                                    {
                                        SelectedInvoiceCrDrList = SelectedInvoicesCrDr;
                                    }
                                    else
                                    {
                                        SelectedInvoiceCrDrList = new List<long>();
                                    }
                                    SelectedInvoiceCrDrList.Add((long)CrDr.CDM_INVOICE_CUS_HDR);
                                    SelectedInvoicesCrDr = SelectedInvoiceCrDrList;
                                    SelectedInvoicesCrDrCount = SelectedInvoicesCrDr.Count;
                                    FinInvoiceCrDrSelectedList = finCrDrNoteMpgList;
                                }
                            }
                            else
                            {
                                txtPaidAmount.Text = "0.00";
                            }
                        }
                        break;
                    #endregion
                    #region CR/DR SPLIT LIST
                    case ControlsEnum.CRDRSPLITLIST:
                        if (DebitCreditHeaderSession != null && DebitCreditHeaderSession.DebitCreditTrxMapping.Count > 0)
                        {
                            DebitCreditTrxMapping objTrxMpg = DebitCreditHeaderSession.DebitCreditTrxMapping.SingleOrDefault(r => r.CDM_INVOICE_CUS_HDR == InvoicePK);
                            if (objTrxMpg != null && objTrxMpg.ItemDetail != null && objTrxMpg.ItemDetail.Count > 0)
                                grdDCSplit.DataSource = objTrxMpg.ItemDetail.ToList();
                            else
                                grdDCSplit.DataSource = null;
                        }
                        else
                            grdDCSplit.DataSource = null;
                        grdDCSplit.DataBind();
                        grdDCSplit.Columns[6].Visible = grdDCSplit.Columns[7].Visible = grdDCSplit.Columns[12].Visible = isLineItemTaxEnabled;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalSplit", "$(document).ready(function(){CalculateTotalSplit();});", true);
                        break;
                    #endregion
                    #region TAX POPUP GRID
                    case ControlsEnum.TAXPOPUPGRID:
                        if (tempDebitCreditHeaderTax != null && tempDebitCreditHeaderTax.Count > 0)
                            grdTaxDetails.DataSource = tempDebitCreditHeaderTax;
                        else
                            grdTaxDetails.DataSource = null;
                        grdTaxDetails.DataBind();

                        //if (this.FinCrDrNoteTaxHeader != null)
                        //{
                        //    FIN_CRDR_NOTE_TAX_HDR[] tempArry = FinCrDrNoteTaxHeader.ToArray();
                        //    FinCrDrNoteTaxHeaderTemp = tempArry.ToList();
                        //}
                        //else
                        //{
                        //    var tmp = finCrDrNoteHdrList.FirstOrDefault(x => x.CDH_PK == CurrPK);
                        //    if (tmp != null)
                        //    {
                        //        FinCrDrNoteTaxHeader = tmp.FIN_CRDR_NOTE_TAX_HDR.ToList();
                        //        FIN_CRDR_NOTE_TAX_HDR[] tempArry = FinCrDrNoteTaxHeader.ToArray();
                        //        FinCrDrNoteTaxHeaderTemp = tempArry.ToList();
                        //    }
                        //    else
                        //    {
                        //        FinCrDrNoteTaxHeader = new List<FIN_CRDR_NOTE_TAX_HDR>(); //tmp.FIN_CRDR_NOTE_TAX_HDR.ToList();
                        //        FIN_CRDR_NOTE_TAX_HDR[] tempArry = FinCrDrNoteTaxHeader.ToArray();
                        //        FinCrDrNoteTaxHeaderTemp = tempArry.ToList();
                        //    }
                        //}

                        //grdTaxDetails.DataSource = FinCrDrNoteTaxHeaderTemp;
                        //grdTaxDetails.DataBind();
                        break;
                    #endregion
                    //#region TAX POPUP GRID TEMP
                    //case ControlsEnum.TAXPOPUPGRIDTEMP://Biju
                    //    grdTaxDetails.DataSource = FinCrDrNoteTaxHeaderTemp;
                    //    grdTaxDetails.DataBind();
                    //    break;
                    //#endregion
                    #region TAX MYR
                    case ControlsEnum.TAXMYR:
                        if (taxList != null && taxList.Count > 0)
                        {
                            grdTaxPayable.DataSource = taxList;
                            grdTaxPayable.DataBind();
                        }
                        else
                        {
                            grdTaxPayable.DataSource = null;
                            grdTaxPayable.DataBind();
                        }
                        break;
                    #endregion
                    #region TAX SPLIT UP
                    case ControlsEnum.TAXSPLITUP:
                        if (taxList != null && taxList.Count > 0)
                        {
                            grdTaxSplitup.DataSource = taxList;
                            grdTaxSplitup.DataBind();
                        }
                        else
                        {
                            grdTaxSplitup.DataSource = null;
                            grdTaxSplitup.DataBind();
                        }
                        break;
                    #endregion
                    #region TAX SPLITUP LINE ITEM WISE
                    case ControlsEnum.TAXSPLITUPLINEITEMWISE:
                        if (taxList != null && taxList.Count > 0)
                        {
                            grdTaxSplitupLineItem.DataSource = taxList;
                            grdTaxSplitupLineItem.DataBind();
                        }
                        else
                        {
                            grdTaxSplitupLineItem.DataSource = null;
                            grdTaxSplitupLineItem.DataBind();
                        }
                        break;
                    #endregion
                    #region BALANCE AMOUNT SPLIT
                    case ControlsEnum.BALANCEAMOUNTSPLIT:
                        if (objCrdrAllocations != null && objCrdrAllocations.Count > 0)
                        {
                            grdBalAmntSplitup.DataSource = objCrdrAllocations;
                            grdBalAmntSplitup.DataBind();
                        }
                        else
                        {
                            grdBalAmntSplitup.DataSource = null;
                            grdBalAmntSplitup.DataBind();
                        }
                        break;
                    #endregion
                    #region FILEUPLOAD
                    case ControlsEnum.FILEUPLOAD:
                        if (DocAttachList != null && DocAttachList.Count > 0)
                            grdUploads.DataSource = DocAttachList;
                        else
                            grdUploads.DataSource = null;
                        grdUploads.DataBind();
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
                foreach (GridViewRow grdrow in grdCrDbHdr.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID);
                    // check row selected or not
                    if (rbtn.Checked)
                    {
                        // get pk from the grid and assign to CurrPk
                        //FinCrDrNoteTaxHeader = null;
                        //FinCrDrNoteTaxHeaderTemp = null;
                        CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCrDrPk")).Value); //Convert.ToInt32(grdCrDbHdr.DataKeys[grdrow.RowIndex].Values[0]);
                        //Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                        //Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomerName")).Text;
                        Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                        hdfCancelled.Value = "0";
                        HiddenField hdfDelStatus = grdrow.FindControl("hdfDelStatus") as HiddenField;
                        if (hdfDelStatus != null)
                            if (Convert.ToBoolean(hdfDelStatus.Value) == true)
                                hdfCancelled.Value = "1";
                        //GetFieldValues(ControlsEnum.DRCRHDRLISTJOURNAL);
                        if (Mode != ActionsEnum.EDITFORCANCEL)
                        {
                            if (InvoiceType == Convert.ToInt32(SalesInvoiceType.Domestic))
                                FillProcessID(1);
                            else
                                FillProcessID(3);
                        }
                        GetFieldValues(ControlsEnum.DRCRHEADER);
                        SetFieldValues(ControlsEnum.DRCRHEADER);
                        SetFieldValues(ControlsEnum.DRCRMPGLIST);
                        BindGrid(ControlsEnum.FILEUPLOAD);

                        if (Mode == ActionsEnum.VIEW)
                            EntryStatus = EntryStatus.VIEWMODE;
                        else
                            EntryStatus = EntryStatus.ENTRYMODE;
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID((int)CurrPK, ucrWrkf.ProcessID);
                        SetCancelRef(Convert.ToInt32(CurrPK));
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                        {
                            ucrWrkf.ViewType = 1;
                            // btnSave.Visible = true;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            //EntryStatus = EntryStatus.VIEWMODE;
                            //btnSave.Visible = false;
                        }
                        ucrWrkf.ViewAction();

                        //If Invoicetype is Domestic then exchangerate is not editable        
                        if (InvoiceType == Convert.ToInt32(SalesInvoiceType.Domestic))
                            txtExchangeRate.Enabled = false;
                        else
                            txtExchangeRate.Enabled = true;
                        return;
                    }
                }
                // if no items selected, Show Error Message
                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
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
            CrDrSplitList = null;
            DocAttachList = null;
            DebitCreditHeaderSession = null;
            tempDebitCreditHeader = null;
            debitCreditHeaderObj = null;
            SetFieldValues(ControlsEnum.PEDINGINVLIST);
            BindGrid(ControlsEnum.FILEUPLOAD);
            ResetForm(ControlsEnum.ADDITEM);
            hdfIsPendingInvVisible.Value = "0";

            txtCustomer.Text = string.Empty;
            hdfCustomerPk.Value = string.Empty;
            txtDate.Text = string.Empty;
            txtPendingFromDate.Text = string.Empty;
            txtPendingToDate.Text = string.Empty;
            ddlPendingInvCategory.ClearSelection();          
            ddlPendingInvType.ClearSelection();           
            txtPendingInvNumber.Text = string.Empty;
            hdfPendingInvPk.Value = string.Empty;
            ddlMode.ClearSelection();
            txtPaymentCurrency.Text = string.Empty;
            txtExchangeRate.Text = string.Empty;
            txtInstrumentNo.Text = string.Empty;
            txtInstrumentDate.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            txtRemarks2.Text = string.Empty;
            txtSubTotal.Text = txtTax.Text = txtShipCharge.Text = txtOtherCharge.Text = txtHdrTotal.Text = GetFormattedCurrency(0);

            txtCustSearch.Text = string.Empty;
            hdfCustSearchID.Value = string.Empty;
            txtCrDrNumber.Text = string.Empty;
            hdfCrDrNumber.Value = string.Empty;
            txtRemarks.Text = string.Empty;
            txtPaidAmount.Text = string.Empty;
            txtInvoiceNo.Text = string.Empty;
            //SelectedInvoicesCrDr = null;
            EditedPaymentDtls = null;
            ModifiedDatePnl.Visible = false;
            lblLastModifiedHDR.Text = string.Empty;
            grdInvoiceList.DataSource = null;
            grdInvoiceList.DataBind();
            txtSearchDateFrom.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
            hdfSearchDateFrom.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
            txtSearchDateTo.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            hdfSearchDateTo.Value = DateTime.Now.ToString();
            //GetFieldValues(ControlsEnum.FINPERIOD);
            //txtSearchDateFrom.Text = string.Empty;
            //hdfSearchDateFrom.Value = string.Empty;
            //txtSearchDateTo.Text = string.Empty;
            //hdfSearchDateTo.Value = string.Empty;

            //SetFieldValues(ControlsEnum.FINPERIOD);
            SortBy = Resources.DataFieldRes.CrDrDate;
            ThenBy = Resources.DataFieldRes.CrDrNo;
            ddlStatus.SelectedIndex = 0;
            ddlInvoiceType.SelectedIndex = 0;
            ddlCreditDebitType.SelectedIndex = 0;
            this.FinCrDrNoteTaxHeader = null;
            this.FinCrDrNoteTaxHeaderTemp = null;

            base.WkfRefID = 0;
            FileDetailsList = null;
            PageIndex = "1";
            ddlCompanySrch.SelectedIndex = -1;
            chkIsReceiptAlcnReq.Checked = false;
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

            BusinessObject.User currentUser;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            FinCrDrHdrNoteService finCrDrHdrNoteServiceClient;
            finCrDrHdrNoteServiceClient = null;
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            DropDownList ddlWkfAction;
            TextBox WrkfComments;
            string action;

            HiddenField hdfInvoicePK;
            HiddenField hdfInvPK;
            TextBox txtNoteFor;
            List<FIN_CRDR_NOTE_DTL> tempInvoiceSOSplitList;

            List<SOInvoiceTaxHdr> tempInvTaxHdrSplit;
            SOInvoiceTaxHdr tempInvTaxSplitObj = null;



            int bankPK;
            int mode;
            bool bIsChecked = false;
            Label lblTotalPayNowFooter;
            HiddenField hdfPayNowFooter;
            RadioButton rbtn;
            HiddenField hdfDept;
            int selectedInvPK;
            int dept;
            GridViewRow grvRow;
            int delStatus = 0;

            try
            {
                long? result;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlMode")
                    {
                        commonActions = ActionsEnum.DRCRMODE;
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
                    if (((TextBox)sender).ID == "txtNoteFor")
                    {
                        commonActions = ActionsEnum.RAISENOTECHANGE;
                    }
                    //ExchangeRate text changing event
                    else if (((TextBox)sender).ID == "txtExchangeRate")
                    {
                        commonActions = ActionsEnum.CHANGEEXRATE;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    if (((LinkButton)sender).ID == "lnkBalanceAmount")
                    {
                        commonActions = ActionsEnum.BALANCEAMOUNTSPLIT;
                    }
                }
                switch (commonActions)
                {
                    #region NEW
                    case ActionsEnum.NEW:
                        ResetForm();
                        FillProcessID(1);
                        base.WkfRefID = ucrWrkf.RefID = 0;
                        ucrWrkf.FillWorkFlowDetails();
                        ucrWrkf.ViewType = 1;
                        ucrWrkf.ViewAction();
                        ModifiedDatePnl.Visible = false;
                        EntryStatus = EntryStatus.NEWMODE;
                        lblDrCrNo.Text = Resources.ErpRes.Draft;
                        txtDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                        ddlMode.Enabled = true;
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
                        InvListObj = new DebitCreditInvBO();
                        InvListObj.InvList = new List<DebitCreditInv>();
                        List<DebitCreditInv> objItemList = new List<DebitCreditInv>();
                        DebitCreditInv objInvDet;
                        HiddenField hdfInvCustomerPK;
                        HiddenField hdfType;
                        HiddenField hdfInvCurrency;
                        HiddenField hdfPndInvCategory;
                        HiddenField hdfPndInvGroup;
                        if (DebitCreditHeaderSession != null && DebitCreditHeaderSession.DebitCreditTrxMapping != null)
                        {
                            DebitCreditHeaderSession.DebitCreditTrxMapping.ForEach(dtl =>
                            {
                                objInvDet = new DebitCreditInv();
                                objInvDet.ICH_PK = dtl.CDM_INVOICE_CUS_HDR;
                                objInvDet.ICH_CUSTOMER = Convert.ToInt32(DebitCreditHeaderSession.CDH_CUSTOMER);
                                objInvDet.ICH_TYPE = dtl.ICH_TYPE;
                                objInvDet.ICH_CURRENCY = DebitCreditHeaderSession.CDH_CURRENCY;
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
                                objInvDet = new DebitCreditInv();
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
                        if (objItemList != null && objItemList.Count > 0 && DebitCreditHeaderSession != null && DebitCreditHeaderSession.DebitCreditTrxMapping != null)
                        {
                            List<string> objPoPkList = DebitCreditHeaderSession.DebitCreditTrxMapping.Select(r => r.CDM_INVOICE_CUS_HDR.ToString()).Distinct().ToList();
                            InvListObj.InvList = objItemList.Where(r => !objPoPkList.Contains(r.ICH_PK.ToString())).ToList();
                        }
                        GetFieldValues(ControlsEnum.DRCRHEADER);
                        SetFieldValues(ControlsEnum.DRCRHEADER);
                        SetFieldValues(ControlsEnum.DRCRMPGLIST);
                        hdfIsPendingInvVisible.Value = "0";
                        EnableDisableDrCrMode();
                        break;
                    #endregion
                    #region REMOVE FOM LIST
                    case ActionsEnum.REMOVE:
                        grvRow = ((Button)sender).Parent.Parent as GridViewRow;
                        HiddenField hdfInvoice = grvRow.FindControl("hdfInvoicePK") as HiddenField;
                        SetFieldValues(ControlsEnum.UPDATEGRIDVALTOOBJECT);
                        if (DebitCreditHeaderSession != null
                            && DebitCreditHeaderSession.DebitCreditTrxMapping != null
                            && DebitCreditHeaderSession.DebitCreditTrxMapping.Count > 0)
                        {
                            tempDebitCreditHeader = DebitCreditHeaderSession;
                            DebitCreditTrxMapping objTrxMpg = tempDebitCreditHeader.DebitCreditTrxMapping.SingleOrDefault(r => r.CDM_INVOICE_CUS_HDR == Convert.ToInt32(hdfInvoice.Value));
                            tempDebitCreditHeader.DebitCreditTrxMapping.Remove(objTrxMpg);
                            DebitCreditHeaderSession = tempDebitCreditHeader;
                            SetFieldValues(ControlsEnum.DRCRMPGLIST);
                        }

                        break;
                    #endregion
                    #region CR/DR Split
                    case ActionsEnum.DCDETAIL:
                        divErrorLabel.Visible = false;
                        HiddenField hdfReceiptMpgPK = (HiddenField)((((Button)sender).Parent).FindControl("hdfCrDbMpgPK"));
                        TextBox txtReceivedNow = (TextBox)((((Button)sender).Parent).FindControl("txtNoteFor"));
                        //Label lblTotalTax = (Label)((((Button)sender).Parent).FindControl("lblTotalTax"));
                        LinkButton lbnTotalTax = (LinkButton)((((Button)sender).Parent).FindControl("lbnTotalTax"));
                        HiddenField hdfTotalTax = (HiddenField)((((Button)sender).Parent).FindControl("hdfTotalTax"));
                        InvRowIndex = ((GridViewRow)((Button)(sender)).Parent.Parent).RowIndex;
                        if (txtReceivedNow != null && !string.IsNullOrEmpty(txtReceivedNow.Text.Trim()))
                        {
                            PayNowAmount = Convert.ToDecimal(txtReceivedNow.Text.Trim());
                            paynowtax = Convert.ToDecimal(hdfTotalTax.Value.Trim());
                        }

                        hdfInvoicePK = (HiddenField)((((Button)sender).Parent).FindControl("hdfInvoicePK"));
                        if (hdfInvoicePK != null && !string.IsNullOrEmpty(hdfInvoicePK.Value) && !hdfInvoicePK.Value.Equals("0"))
                        {
                            InvoicePK = Convert.ToInt64(hdfInvoicePK.Value);
                        }
                        else
                            InvoicePK = 0;

                        GetUIValuesFromObject(ControlsEnum.CRDRSPLITLIST);
                        isSplitChanged = false;
                        SetFieldValues(ControlsEnum.CRDRSPLITLIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCrDrSplitUp]','" + GetLocalResourceObject("ReceiptSplit").ToString() + "','950','300');", true);
                        break;
                    #endregion
                    #region DR/CR SPLIT SAVE
                    case ActionsEnum.DCSPLITSAVE:
                        decimal TotalPayNow = 0;
                        decimal TotalPayNowTax = 0;
                        HiddenField lblTotalPayNow = (HiddenField)grdDCSplit.FooterRow.FindControl("hdfTotalPayNowFooterSplit");
                        TotalPayNow = Convert.ToDecimal(lblTotalPayNow.Value);
                        if (InvRowIndex >= 0)
                        {
                            TextBox txtTotalPayNow = (TextBox)grdInvoiceList.Rows[InvRowIndex].FindControl("txtNoteFor");
                            Button lnkAllocation = (Button)grdInvoiceList.Rows[InvRowIndex].FindControl("lnkAllocation");
                            Session["event_controle"] = lnkAllocation;
                            txtTotalPayNow.Text = Math.Round(TotalPayNow, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            HiddenField hdfItemIncluded = (HiddenField)grdInvoiceList.Rows[InvRowIndex].FindControl("hdfItemIncluded");

                            if (isLineItemTaxEnabled)
                            {
                                HiddenField hdfTotalTaxFooterSplit = (HiddenField)grdDCSplit.FooterRow.FindControl("hdfTotalTaxFooterSplit");
                                TotalPayNowTax = Convert.ToDecimal(hdfTotalTaxFooterSplit.Value);
                                LinkButton lbnTotalPayNowTax = (LinkButton)grdInvoiceList.Rows[InvRowIndex].FindControl("lbnTotalTax");
                                HiddenField hdfTotalPayNowTax = (HiddenField)grdInvoiceList.Rows[InvRowIndex].FindControl("hdfTotalTax");
                                if (lbnTotalPayNowTax != null && hdfTotalPayNowTax != null)
                                {
                                    lbnTotalPayNowTax.Text = GetFormattedCurrencyWithComma(TotalPayNowTax);
                                    hdfTotalPayNowTax.Value = hdfTotalTaxFooterSplit.Value;
                                }
                            }

                            SetUIValuesToObject(ControlsEnum.CRDRSPLITLIST);
                            DebitCreditTrxMapping objTrxMpg = DebitCreditHeaderSession.DebitCreditTrxMapping.SingleOrDefault(r => r.CDM_INVOICE_CUS_HDR == InvoicePK);
                            if (objTrxMpg != null && objTrxMpg.ItemDetail != null && objTrxMpg.ItemDetail.Count() > 0 && objTrxMpg.ItemDetail.Sum(dtl => dtl.CDS_AMOUNT) > 0)
                            {
                                hdfItemIncluded.Value = "1";
                            }
                            else
                            {
                                hdfItemIncluded.Value = "0";
                            }
                        }
                        InvRowIndex = RowIndex = -1;

                        //DcsSplitSave();
                        SetSplitCount();
                        //GetFieldValues(ControlsEnum.TAXMYR);
                        //SetFieldValues(ControlsEnum.TAXMYR);
                        if (Session["event_controle"] != null)
                        {
                            Button controle = (Button)Session["event_controle"];
                            controle.Focus();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                        break;
                    #endregion
                    #region SAVE
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            //if (CurrPK > 0)
                            //{
                            //    GetFieldValues(ControlsEnum.CHECKCRDRUSEDINOTHERTRNS);
                            //    if (isDrCrUsedInOtherTrns)
                            //    {
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_CrdrUsed").ToString()) + "','" + Resources.Messages.Information + "');", true);
                            //        return;
                            //    }
                            //}
                            // if (!)

                            if (grdInvoiceList.Rows.Count >= 1)
                            {
                                if (Convert.ToInt32(ddlMode.SelectedValue) == (int)DebitCreditModeEnum.CREDIT && InvCategory == (int)SalesInvoiceCategory.Advanced && !IsValidCNAmount())
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_CN_AmountExceeded").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                                //finCrDrNoteHdrList = new List<FIN_CRDR_NOTE_HDR>();
                                //finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                                //finCrDrHdrNoteServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                                //finCrDrNoteHdrObj = CommonFunctions.Initilize<FIN_CRDR_NOTE_HDR>();
                                debitCreditHeaderObj = (DebitCreditHeader)SetUIValuesToObject(ControlsEnum.DRCRHEADERENTRY);

                                //if (finCrDrHdrNoteServiceClient.IsRefnoExist(finCrDrNoteHdrObj))
                                //{
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_RefnoExist").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                //    return;
                                //}
                                lblTotalPayNowFooter = (Label)grdInvoiceList.FooterRow.FindControl("lblTotalPayNowFooter");
                                hdfPayNowFooter = (HiddenField)grdInvoiceList.FooterRow.FindControl("hdfPayNowFooter");
                                lblTotalPayNowFooter.Text = GetFormattedCurrencyWithComma(hdfTotalPayNowFooter.Value);
                                hdfPayNowFooter.Value = GetFormattedCurrency(hdfTotalPayNowFooter.Value);
                                if (debitCreditHeaderObj != null)
                                {
                                    if (debitCreditHeaderObj.DebitCreditTrxMapping.ToList().Count > 0)
                                    {
                                        //finCrDrNoteHdrList.Add(debitCreditHeaderObj);
                                        //int Archiveresult = 0;
                                        //if (debitCreditHeaderObj.CDH_PK > 0 && debitCreditHeaderObj.CDH_STATUS > 0)
                                        //{
                                        //    //After getting entry into workflow, for each update keep version details of DN/CN for Audit trail 
                                        //    Archiveresult = BusinessLogic.POInvoicing.POInvoiceBL.SaveDRCR_Note_ArchiveDetails(finCrDrNoteHdrObj.CDH_PK);
                                        //}

                                        debitCreditHeaderObj.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                        string xmlDoc = CommonFunctions.XmlSerialize<DebitCreditHeader>(debitCreditHeaderObj);
                                        string crdrNo = string.Empty;
                                        result = BusinessLogic.Sales.DebitCreditBL.SaveCreditDebitSales(xmlDoc, out crdrNo);
                                        //result = finCrDrHdrNoteServiceClient.SaveCrDrNoteHdr(finCrDrNoteHdrList, false);                                             

                                        if (result > 0)
                                        {
                                            #region  ATTACHMENT SAVE
                                            string savePath = string.Empty;
                                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                            {
                                                savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                                                if (!Directory.Exists(savePath))
                                                    Directory.CreateDirectory(savePath);
                                                savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\";
                                            }
                                            else
                                            {
                                                savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();
                                            }

                                            foreach (DebitCreditUploads obj in debitCreditHeaderObj.FileList)
                                            {
                                                //string filePath = savePath + obj.DOC_NAME;
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
                                                        fileDetailsObj.SoFile.SaveAs(attachedFileInfo.FullName);

                                                    }
                                                }
                                            }
                                            #endregion

                                            #region Generate dummy entry
                                            if (CurrPK > 0)
                                            {
                                                if (debitCreditHeaderObj.CDH_STATUS == (byte)DbStatus.APPROVED)
                                                {
                                                    FinTrxService finTrxServiceClient;
                                                    finTrxServiceClient = new FinTrxService();
                                                    string refType = "";
                                                    if (debitCreditHeaderObj.CDH_TYPE == (byte)DebitCreditModeEnum.DEBIT)
                                                    {
                                                        refType = ApplicationType.DNTJ;

                                                    }
                                                    else
                                                    {
                                                        refType = ApplicationType.CNTJ;

                                                    }
                                                    long DummyResult = 0;
                                                    bool IsDummyEntry = finTrxServiceClient.IsDummyEntry(refType, (int)CurrPK, 0);
                                                    if (IsDummyEntry == true)
                                                    {
                                                        DummyResult = finTrxServiceClient.DeleteFinTrx(refType, (int)CurrPK, 0);
                                                    }
                                                    else
                                                    {
                                                        DummyResult = (int)CurrPK;
                                                    }

                                                    if (DummyResult > 0)
                                                    {
                                                        finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                                        DummyResult = finTrxServiceClient.GenerateDummyEntry((int)CurrPK, refType);
                                                    }

                                                }

                                            }
                                            #endregion

                                            litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.CreditDebitNotesTrading);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                            ResetForm();
                                            GetFieldValues(ControlsEnum.DRCRHDRLIST);
                                            SetFieldValues(ControlsEnum.DRCRHDRLIST);
                                            btnNew.Focus();
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
                                                litErrorMsg.Text = Resources.PageNameRes.CreditDebitNotesTrading + " " + Resources.Messages.EditUsedByAnotherUser;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.CODEEXIST)
                                            {
                                                litErrorMsg.Text = Resources.PageNameRes.CreditDebitNotesTrading + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.DUPLICATERECORDS)//Ref No already exist
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_RefnoExist").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            }
                                            else if (result == -26)//Debit Credit Note is referred in other transactions.
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_CrdrUsed").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            }
                                            else
                                            {
                                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CreditDebitNotesTrading);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            }

                                        }

                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_Amount").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_InvoiceCount").ToString()) + "');", true);
                            }

                        }
                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        if (grdInvoiceList.Rows.Count >= 1)
                        {
                            if (Convert.ToInt32(ddlMode.SelectedValue) == (int)DebitCreditModeEnum.CREDIT && InvCategory == (int)SalesInvoiceCategory.Advanced && !IsValidCNAmount())
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_CN_AmountExceeded").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                            ucrWrkf.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_InvoiceCount").ToString()) + "');", true);
                        }
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        if (grdInvoiceList.Rows.Count >= 1)
                        {
                            hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                            ucrWrkf.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_InvoiceCount").ToString()) + "');", true);
                        }

                        break;
                    case ActionsEnum.WRKFSUBMIT:
                        //Submit Activity
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {

                                if (grdInvoiceList.Rows.Count >= 1)
                                {
                                    debitCreditHeaderObj = (DebitCreditHeader)SetUIValuesToObject(ControlsEnum.DRCRHEADERENTRY);

                                    lblTotalPayNowFooter = (Label)grdInvoiceList.FooterRow.FindControl("lblTotalPayNowFooter");
                                    hdfPayNowFooter = (HiddenField)grdInvoiceList.FooterRow.FindControl("hdfPayNowFooter");
                                    //lblTotalPayNowFooter.Text = GetFormattedCurrency(hdfTotalPayNowFooter.Value);
                                    lblTotalPayNowFooter.Text = GetFormattedCurrencyWithComma(hdfTotalPayNowFooter.Value);
                                    hdfPayNowFooter.Value = GetFormattedCurrency(hdfTotalPayNowFooter.Value);
                                    if (hdfExchangeRate.Value != "-1")
                                    {
                                        if (debitCreditHeaderObj != null)
                                        {
                                            debitCreditHeaderObj.WKF_FLAG = 1;
                                            debitCreditHeaderObj.ATL_ACTION = (byte)LogAction.NEW;
                                            SaveTransaction(debitCreditHeaderObj, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
                                        }
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.Msg_ErrConversionFactor;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_InvoiceCount").ToString()) + "');", true);
                                }
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(Convert.ToInt32(CurrPK), ApplicationType.CNT))
                                {
                                    ucrWrkf.ApplicationID = Convert.ToInt32(CurrPK);
                                    isCancelled = true;
                                    SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_CRDR_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        FillProcessID(1);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    SelectedInvoicesCrDr = null;
                                    GetFieldValues(ControlsEnum.DRCRHDRLIST);
                                    SetFieldValues(ControlsEnum.DRCRHDRLIST);
                                }
                            }
                            else
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));

                        }
                        break;
                    #endregion

                    #region   ITEMSELECTED
                    case ActionsEnum.ITEMSELECTED:
                        GridViewRow selectedGrdrow = (sender as RadioButton).Parent.Parent as GridViewRow;
                        rbtn = sender as RadioButton;
                        bIsChecked = true;
                        HiddenField hdfPaymentID;
                        HiddenField hdfDelStatus;
                        int pk;
                        delStatus = 0;
                        hdfPaymentID = selectedGrdrow.FindControl("hdfCrDrPk") as HiddenField;
                        if (hdfPaymentID != null && int.TryParse(hdfPaymentID.Value, out pk))
                        {
                            CurrPK = pk;
                        }
                        hdfDept = selectedGrdrow.FindControl("hdfDept") as HiddenField;
                        if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                        {
                            Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                            base.SetUserDept();
                        }
                        hdfDelStatus = selectedGrdrow.FindControl("hdfDelStatus") as HiddenField;
                        if (hdfDelStatus != null)
                            if (Convert.ToBoolean(hdfDelStatus.Value) == true)
                            {
                                btnSave.Visible = false;
                                btnEditforCancel.Visible = false;
                                btnEdit.Visible = false;
                                btnSaveSubmit.Visible = false;
                                hdfCancelled.Value = "1";
                            }
                            else
                            {
                                hdfCancelled.Value = "0";
                                btnSave.Visible = true;
                                Label lblTransactionId = selectedGrdrow.FindControl("lblTransactionId") as Label;
                                if (lblTransactionId.Text != "[NEW]")
                                {
                                    btnEditforCancel.Visible = true;
                                }
                                else
                                {
                                    btnEditforCancel.Visible = false;
                                }
                            }
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = workflowCore.GetRefID((int)CurrPK, PageProcessID);
                        ////

                        break;
                    #endregion


                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        PageIndex = "1";
                        GetFieldValues(ControlsEnum.DRCRHDRLIST);
                        SetFieldValues(ControlsEnum.DRCRHDRLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        uclPaging.TotalPages = TotalPages;
                        uclPaging.CurrentPage = 1;
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        SelectedInvoicesCrDr = null;
                        selectedInvoiceList = null;
                        FillProcessID(1);
                        ResetForm();
                        PageIndex = "1";
                        GetFieldValues(ControlsEnum.DRCRHDRLIST);
                        SetFieldValues(ControlsEnum.DRCRHDRLIST);
                        uclPaging.TotalPages = TotalPages;
                        uclPaging.CurrentPage = 1;
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        uclPaging.TotalPages = TotalPages;
                        uclPaging.CurrentPage = 1;
                        break;
                    #endregion
                    #region EDIT/CREDITDEBITDETAIL/VIEW
                    case ActionsEnum.EDIT:
                    case ActionsEnum.CREDITDEBITDETAIL:
                    case ActionsEnum.VIEW:
                        FillProcessID(1);
                        SetUIEditView(commonActions);
                        ModifiedDatePnl.Visible = true;
                        EnableDisableDrCrMode();
                        //ConfigurationSettings();
                        //GetFieldValues(ControlsEnum.TAXMYR);
                        //SetFieldValues(ControlsEnum.TAXMYR);
                        hdfIsPendingInvVisible.Value = "0";
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = BusinessLogic.Sales.DebitCreditBL.DeleteCreditDebitDetails(CurrPK, LastModifiedTime);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.CreditDebitNotesTrading);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                FillProcessID(1);
                                ResetForm();
                                GetFieldValues(ControlsEnum.DRCRHDRLIST);
                                SetFieldValues(ControlsEnum.DRCRHDRLIST);
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
                                    litErrorMsg.Text = Resources.PageNameRes.CreditDebitNotesTrading + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.DRCRHDRLIST);
                                    SetFieldValues(ControlsEnum.DRCRHDRLIST);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.CreditDebitNotesTrading + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.CreditDebitNotesTrading + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.DRCRHDRLIST);
                                    SetFieldValues(ControlsEnum.DRCRHDRLIST);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CreditDebitNotesTrading);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                #endregion
                            }
                        }
                        break;
                    #endregion

                    #region Print
                    case ActionsEnum.PRINT:
                        HiddenField hdfCreditDebitType = null;
                        string Reftype = "";
                        if (EntryStatus == EntryStatus.LISTMODE)
                        {
                            foreach (GridViewRow grdrow in grdCrDbHdr.Rows)
                            {
                                rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID);
                                hdfCreditDebitType = (HiddenField)grdrow.FindControl("hdfCreditDebitType");
                                HiddenField hdfCrDrInvType = (HiddenField)grdrow.FindControl("hdfCrDrInvType");
                                // check row selected or not
                                if (rbtn.Checked)
                                {
                                    // get pk from the grid and assign to CurrPk
                                    CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCrDrPk")).Value);//Convert.ToInt32(grdCrDbHdr.DataKeys[grdrow.RowIndex].Values[0]);
                                    bIsChecked = true;
                                    hdfCrDrNumber.Value = CurrPK.ToString();
                                    Reftype = hdfCreditDebitType.Value == "1" ? ApplicationType.DNT : ApplicationType.CNT;
                                    InvType = Convert.ToInt32(hdfCrDrInvType.Value);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            bIsChecked = true;
                            hdfCrDrNumber.Value = CurrPK.ToString();
                            GetFieldValues(ControlsEnum.DRCRHDRLISTJOURNAL);
                            InvType = crDrInvoiceType;
                            if (finCrDrNoteHdrList != null && finCrDrNoteHdrList.Count > 0)
                            {
                                Reftype = finCrDrNoteHdrList[0].CDH_TYPE.ToString() == "1" ? ApplicationType.DNT : ApplicationType.CNT;
                            }
                            else
                            {
                                bIsChecked = false;
                            }
                        }
                        if (bIsChecked)
                        {
                            if (InvType == 2)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + Reftype + "&APPSUBTYPE=2" + "');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + Reftype + "&APPSUBTYPE=22" + "');", true);
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            EntryStatus = EntryStatus.LISTMODE;
                            ResetForm();
                            GetFieldValues(ControlsEnum.DRCRHDRLIST);
                            SetFieldValues(ControlsEnum.DRCRHDRLIST);
                        }

                        break;
                    #endregion

                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetForm();
                        GetFieldValues(ControlsEnum.DRCRHDRLIST);
                        SetFieldValues(ControlsEnum.DRCRHDRLIST);
                        uclPaging.TotalPages = TotalPages;
                        uclPaging.CurrentPage = 1;
                        break;
                    #endregion

                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
                        SetUIValuesToObject(ControlsEnum.JOURNALIZE);
                        break;
                    #endregion
                    #region Journalize Update
                    case ActionsEnum.JOURNALIZEUPDATE:
                        ucrJournalize.ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.DRCRHDRLIST);
                        SetFieldValues(ControlsEnum.DRCRHDRLIST);
                        break;
                    #endregion
                    #region Journalize Save
                    case ActionsEnum.JOURNALIZESAVE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.DRCRHDRLIST);
                        SetFieldValues(ControlsEnum.DRCRHDRLIST);
                        break;
                    #endregion
                    #region Journalize Submit
                    case ActionsEnum.JOURNALIZESUBMIT:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.DRCRHDRLIST);
                        SetFieldValues(ControlsEnum.DRCRHDRLIST);
                        break;
                    #endregion
                    #region Journalize Delete
                    case ActionsEnum.JOURNALIZEDELETE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.DRCRHDRLIST);
                        SetFieldValues(ControlsEnum.DRCRHDRLIST);
                        break;
                    #endregion
                    #region Journalize Cancel
                    case ActionsEnum.JOURNALIZECANCEL:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        PageIndex = "1";
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.DRCRHDRLIST);
                        SetFieldValues(ControlsEnum.DRCRHDRLIST);
                        uclPaging.TotalPages = TotalPages;
                        uclPaging.CurrentPage = 1;
                        break;
                    #endregion
                    #region Credit Debit Notes List
                    case ActionsEnum.CREDITDEBITLIST:
                        SelectedInvoicesCrDr = null;
                        selectedInvoiceList = null;
                        ResetForm();
                        GetFieldValues(ControlsEnum.DRCRHDRLIST);
                        SetFieldValues(ControlsEnum.DRCRHDRLIST);
                        uclPaging.TotalPages = TotalPages;
                        uclPaging.CurrentPage = 1;
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        delStatus = 0;
                        foreach (GridViewRow grdrow in grdCrDbHdr.Rows)
                        {
                            RadioButton rbtnCrDr;
                            rbtnCrDr = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtnCrDr.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCrDrPk")).Value);
                                Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomerName")).Text;
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                delStatus = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            GetFieldValues(ControlsEnum.DRCRCANCELCHECK);
                            if (finReceiptCRDRmpgList != null && finReceiptCRDRmpgList.Count > 0)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_CheckforCancelDRCR").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            if (Approved == 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_DraftedRecord").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            if (delStatus == 1)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_alreadycancelled").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }

                            FillProcessID(11);
                            SetUIEditView(commonActions);
                            workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID((int)CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(Convert.ToInt32(CurrPK));
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                                ucrWrkf.ViewType = 0;
                            ucrWrkf.ViewAction();
                            ModifiedDatePnl.Visible = true;
                            SetUIEditView(commonActions);
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

                    #region TAXADD
                    case ActionsEnum.TAXADD:
                        bool errorTaxNameAlreadyExist = false;
                        bool errorTaxName = false;
                        bool errorTaxAmount = false;
                        string taxName = txtPopupOther.Text.Trim();
                        decimal taxAmount = Convert.ToDecimal(txtPopupAmount.Text);

                        if (!Decimal.TryParse(txtPopupAmount.Text, out taxAmount)) errorTaxAmount = true;
                        if (String.IsNullOrWhiteSpace(taxName)) errorTaxName = true;

                        if (tempDebitCreditHeaderTax != null && tempDebitCreditHeaderTax.Where(r => r.NTH_NAME.ToLower() == taxName.ToLower()).Count() > 0)
                            errorTaxNameAlreadyExist = true;
                        //foreach (FIN_CRDR_NOTE_TAX_HDR tax in this.FinCrDrNoteTaxHeaderTemp)
                        //{
                        //    if (tax.NTH_NAME.ToLower() == taxName.ToLower())
                        //    {
                        //        errorTaxNameAlreadyExist = true;
                        //    }
                        //}

                        if (!errorTaxName && !errorTaxAmount && !errorTaxNameAlreadyExist)
                        {
                            byte taxCategory = Convert.ToByte(hdfTaxCategory.Value);
                            if (tempDebitCreditHeaderTax == null)
                                tempDebitCreditHeaderTax = new List<DebitCreditTaxHdr>();
                            this.tempDebitCreditHeaderTax.Add(new DebitCreditTaxHdr
                                          {
                                              NTH_PK = 0,
                                              NTH_CRDR_NOTE_HDR = CurrPK,
                                              NTH_TAX_CATEGORY = taxCategory,
                                              NTH_NAME = taxName,
                                              NTH_TAX_AMT = taxAmount,
                                              NTH_TAX = null
                                          });

                            SetFieldValues(ControlsEnum.TAXPOPUPGRID);
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
                                if (!errorTaxName && !errorTaxAmount)
                                {
                                    txtPopupAmount.Text = string.Empty;
                                    txtPopupOther.Text = string.Empty;
                                }
                            }
                        }
                        txtPopupAmount.Enabled = true;
                        txtPopupOther.Enabled = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Tax) ? GetLocalResourceObject("TaxDetails").ToString() : GetLocalResourceObject("OtherCharges").ToString())) + "','600','300');", true);
                        if (errorTaxNameAlreadyExist)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_TaxNameAlreadyExist").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (errorTaxName)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_TaxName").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (errorTaxAmount)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Amount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        //else if (!isValidDisc)
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Discount_Amount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        break;
                    #endregion
                    #region TAXAPPLY
                    case ActionsEnum.TAXAPPLY:
                        SetHdrTax();
                        ResetForm(ControlsEnum.TAXPOPUPGRID);
                        IsHeaderTax = false;
                        IsEditMode = false;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        txtRemarks.Focus();
                        break;
                    #endregion
                    #region TAXDELETE
                    case ActionsEnum.TAXDELETE:
                        if (this.tempDebitCreditHeaderTax != null)
                        {
                            HiddenField hdfTaxPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxPK") as HiddenField);
                            HiddenField hdfTaxName = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxName") as HiddenField);
                            if (hdfTaxPK != null)
                            {
                                int taxPK = string.IsNullOrWhiteSpace(hdfTaxPK.Value) ? 0 : Convert.ToInt32(hdfTaxPK.Value);
                                string txName = string.IsNullOrWhiteSpace(hdfTaxName.Value) ? String.Empty : hdfTaxName.Value;
                                this.tempDebitCreditHeaderTax.Remove(this.tempDebitCreditHeaderTax.SingleOrDefault(x => x.NTH_PK == Convert.ToInt32(taxPK) && x.NTH_NAME == txName));
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
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                                            SelectedTaxText = HttpUtility.HtmlEncode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    txtPopupAmount.Enabled = false;
                                    txtPopupOther.Enabled = false;
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Shipping) ? GetLocalResourceObject("OtherCharges").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','600','300');", true);
                        break;
                    #endregion
                    #region SHIPPINGHEADER
                    case ActionsEnum.SHIPPINGHEADER:
                        hdfTaxCategory.Value = ((int)TaxType.Shipping).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        divTax.Visible = false;
                        //GetFieldValues(ControlsEnum.TAXPOPUPGRID);
                        tempDebitCreditHeaderTax = DebitCreditHeaderTax.DeepClone();
                        SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                        GetFieldValues(ControlsEnum.TAXTYPES);
                        SetFieldValues(ControlsEnum.TAXTYPES);
                        decimal subtotal = 0;
                        decimal.TryParse(hdfTotalPayNowFooter.Value, out subtotal);
                        txtPopupItemAmount.Text = subtotal.ToString(hdfCurrencyFormat.Value);
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
                        txtPopupOther.Text = GetLocalResourceObject("PopOtherOtherName").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("OtherCharges").ToString() + "','600','300');", true);
                        break;
                    #endregion

                    #region TAXDETAILS
                    case ActionsEnum.TAXDETAILS:

                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCrDrSplitUp]','" + GetLocalResourceObject("ReceiptSplit").ToString() + "','900','300');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divTaxPopup]','" + GetLocalResourceObject("TaxDetails").ToString() + "','600','300');", true);
                        break;
                    #endregion
                    #region TAXHEADERSPLITUP
                    case ActionsEnum.TAXHEADERSPLITUP:
                        decimal Totaltax = 0;
                        decimal RaiseNote = 0;
                        GridViewRow grdRow = (GridViewRow)((LinkButton)(sender)).Parent.Parent;
                        HiddenField hdfTotalHdrTax = (HiddenField)grdRow.FindControl("hdfTotalTax");
                        TextBox txtRaiseNote = (TextBox)grdRow.FindControl("txtNoteFor");
                        decimal.TryParse(txtRaiseNote.Text, out RaiseNote);
                        decimal.TryParse(hdfTotalHdrTax.Value, out Totaltax);
                        taxList = null;
                        if (Totaltax > 0 && RaiseNote > 0)
                        {
                            SetUIHeaderTaxView(grdRow);
                        }
                        SetFieldValues(ControlsEnum.TAXSPLITUP);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divTaxSplitup]','" + GetLocalResourceObject("TaxDetails").ToString() + "','600','300');", true);

                        break;
                    #endregion
                    #region RAISE NOTE CHANGE
                    case ActionsEnum.RAISENOTECHANGE:
                        if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                        {
                            RowIndex = ((GridViewRow)((TextBox)(sender)).Parent.Parent).RowIndex;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(hdfRowIndex.Value))
                            {
                                RowIndex = Convert.ToInt32(hdfRowIndex.Value);
                            }
                        }
                        if (RowIndex >= 0)
                        {
                            hdfInvoicePK = (HiddenField)grdInvoiceList.Rows[RowIndex].FindControl("hdfInvoicePK");
                            HiddenField hdfItemIncluded = (HiddenField)grdInvoiceList.Rows[RowIndex].FindControl("hdfItemIncluded");
                            txtNoteFor = (TextBox)grdInvoiceList.Rows[RowIndex].FindControl("txtNoteFor");
                            Session["event_controle"] = txtNoteFor;

                            decimal raiseNote = 0;
                            decimal splitTotal = 0;
                            decimal.TryParse(txtNoteFor.Text.Replace(",", ""), out raiseNote);

                            if (raiseNote > 0)
                            {
                                //List<FIN_CRDR_NOTE_DTL> finCrDrDtlObj = new List<FIN_CRDR_NOTE_DTL>();
                                //tempInvoiceSOSplitList = CrDrSplitList;
                                //// finCrDrCusMpgList = (List<FIN_CRDR_NOTE_DTL>)SetUIValuesToObject(ControlsEnum.CRDRSPLITLIST);
                                //finCrDrDtlObj = tempInvoiceSOSplitList.Where(ivh => ivh.FIN_CRDR_NOTE_MPG.CDM_INVOICE_CUS_HDR == Convert.ToInt64(hdfInvoicePK.Value)).ToList();

                                //if (finCrDrDtlObj != null)
                                //{
                                //    splitTotal = finCrDrDtlObj.Sum(splt => splt.CDS_AMOUNT);
                                //    if (splitTotal != 0) //raiseNote != splitTotal &&
                                //    {
                                //        if ((hdfIscontYes.Value != "1") && hdfIscontNo.Value == "0")
                                //        {
                                //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlreadyPaid", "$(document).ready(function(){RaiseNoteChanged();});", true);
                                //            return;
                                //        }
                                //        else if (hdfIscontNo.Value == "0")
                                //        {
                                //            foreach (FIN_CRDR_NOTE_DTL crdrdtl in finCrDrDtlObj)
                                //            {
                                //                FIN_CRDR_NOTE_DTL finCrdrDtl = new FIN_CRDR_NOTE_DTL();
                                //                finCrdrDtl = crdrdtl;
                                //                tempInvoiceSOSplitList.Remove(crdrdtl);
                                //                finCrdrDtl.CDS_QTY = 0;
                                //                finCrdrDtl.CDS_NET_AMOUNT = 0;
                                //                finCrdrDtl.CDS_TAX = 0;
                                //                finCrdrDtl.CDS_AMOUNT = 0;
                                //                tempInvoiceSOSplitList.Add(finCrdrDtl);
                                //            }
                                //        }
                                //    }
                                //    if (hdfIscontNo.Value != "0")
                                //    {
                                //        txtNoteFor.Text = Math.Round(splitTotal, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                //    }
                                //}
                                //if (hdfIscontNo.Value != "0")
                                //{
                                //    hdfItemIncluded.Value = "1";
                                //    hdfSplitCount.Value = "1";
                                //}
                                //else
                                //{
                                //    hdfItemIncluded.Value = "0";
                                //    hdfSplitCount.Value = "0";
                                //}
                                //hdfIscontYes.Value = "0";
                                //hdfIscontNo.Value = "0";
                                //CrDrSplitList = tempInvoiceSOSplitList;
                                SetSplitCount();
                                //GetFieldValues(ControlsEnum.TAXMYR);
                                //SetFieldValues(ControlsEnum.TAXMYR);
                            }
                            if (Session["event_controle"] != null)
                            {
                                //TextBox controle = (TextBox)Session["event_controle"];

                                //controle.Focus();
                                txtShipCharge.Focus();
                            }

                        }
                        break;


                    #endregion
                    #region TAXLINEITEMSPLITUP
                    case ActionsEnum.TAXLINEITEMSPLITUP:

                        //Rebind the Grid For Keeping changes
                        // tempRaiseNoteSplitupList = new List<FIN_CRDR_NOTE_DTL>();
                        //end
                        decimal TotaltaxLineItem = 0;
                        GridViewRow grdRowLineItem = (GridViewRow)((LinkButton)(sender)).Parent.Parent;
                        HiddenField hdfTAXSplitTotal = (HiddenField)grdRowLineItem.FindControl("hdfTAXSplitTotal");
                        decimal.TryParse(hdfTAXSplitTotal.Value, out TotaltaxLineItem);
                        taxList = null;
                        if (TotaltaxLineItem > 0)
                        {
                            SetUILineItemTaxView(grdRowLineItem);
                        }
                        SetFieldValues(ControlsEnum.TAXSPLITUPLINEITEMWISE);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divTaxSplitupLIneItem]','" + GetLocalResourceObject("TaxDetails").ToString() + "','600','300');", true);
                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCrDrSplitUp]','" + GetLocalResourceObject("ReceiptSplit").ToString() + "','900','300');", true);
                        //}

                        break;
                    #endregion
                    #region SHOWPOPUP
                    case ActionsEnum.SHOWPOPUP:
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            HiddenField hdfInvTypeText;
                            hdfInvTypeText = (HiddenField)grdrow.FindControl("hdfInvoiceType");
                            HiddenField hdfInvCategory = (HiddenField)grdrow.FindControl("hdfInvCategory");
                            HiddenField hdfInvGroup = (HiddenField)grdrow.FindControl("hdfInvGroup");
                            if (Convert.ToInt32(hdfInvCategory.Value) == (int)SalesInvoiceCategory.Advanced)
                            {
                                if (hdfInvTypeText.Value.ToString().ToLower() == "2" || hdfInvTypeText.Value.ToString().ToLower() == "3")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                       ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.DSIJ + "&APPSUBTYPE=" + Convert.ToInt32(ApplicationSubType.ADVANCEINVOICE) + "&TRXTYPE=" + ApplicationType.DSIJ) + "');", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=11") + "');", true);
                                }
                            }
                            else if (Convert.ToInt32(hdfInvGroup.Value) == (int)SalesInvoiceGroup.Miscellaneous)
                            {
                                if (!string.IsNullOrEmpty(hdfInvTypeText.Value))
                                {
                                    if (Convert.ToInt32(hdfInvTypeText.Value) == (int)SalesInvoiceType.Domestic)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.MSIT + "&APPSUBTYPE=1") + "');", true);
                                    }
                                    else if (Convert.ToInt32(hdfInvTypeText.Value) == (int)SalesInvoiceType.Export)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.MSIT + "&APPSUBTYPE=2") + "');", true);
                                    }
                                    else if (Convert.ToInt32(hdfInvTypeText.Value) == (int)SalesInvoiceType.Proforma)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.MSIT + "&APPSUBTYPE=3") + "');", true);
                                    }
                                }
                            }
                            else
                            {
                                if (hdfInvTypeText.Value != string.Empty)
                                {
                                    if (hdfInvTypeText.Value.ToString().ToLower() == "1")
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                            ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=1") + "');", true);
                                    }
                                    else if (hdfInvTypeText.Value.ToString().ToLower() == "2")
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                            ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=2") + "');", true);
                                    }
                                    else if (hdfInvTypeText.Value.ToString().ToLower() == "3")
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                            ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=3") + "');", true);
                                    }
                                }
                            }
                            return;
                        }
                        break;
                    #endregion
                    #region CHANGEEXCHANGERATE
                    case ActionsEnum.CHANGEEXRATE:
                        hdfExchangeRate.Value = string.IsNullOrEmpty(txtExchangeRate.Text) ? "1" : txtExchangeRate.Text;
                        //GetFieldValues(ControlsEnum.TAXMYR);
                        //SetFieldValues(ControlsEnum.TAXMYR);
                        break;
                    #endregion
                    #region BALANCE AMOUNT SPLIT
                    case ActionsEnum.BALANCEAMOUNTSPLIT:
                        GridViewRow grdCrdrList = (GridViewRow)((LinkButton)(sender)).Parent.Parent;
                        HiddenField hdfCrDrPk = (HiddenField)grdCrdrList.FindControl("hdfCrDrPk");
                        CurrPK = Convert.ToInt64(hdfCrDrPk.Value);
                        GetFieldValues(ControlsEnum.BALANCEAMOUNTSPLIT);
                        SetFieldValues(ControlsEnum.BALANCEAMOUNTSPLIT);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalAmountSplit", "$(document).ready(function(){CalculateTotalAmountSplit();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divBalAmntSplitup]','" + GetLocalResourceObject("TrxDetails").ToString() + "','600','300');", true);
                        break;
                    #endregion;


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
                            FileInfo tempFileInfoObj;
                            if (CurrSlNo != 0)
                            {
                                if (fupUpload.HasFile)// || !string.IsNullOrEmpty(anchorFile.HRef))
                                {
                                    tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                    if (!IsValidExtension(tempFileInfoObj.Extension))
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Msg_Valid_File) + "','" + Resources.ErpRes.Information + "');", true);
                                        return;
                                    }
                                    admDocAttachObj = DocAttachList.SingleOrDefault(itm => itm.DOC_SEQ_NO == CurrSlNo);
                                    if (admDocAttachObj != null)
                                    {
                                        if (FileDetailsList == null)
                                        {
                                            FileDetailsList = new List<BusinessObject.Sales.FileDetails>();
                                        }
                                        if (fupUpload.HasFile)
                                        {

                                            tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                            string attachmentFileFormat = tempFileInfoObj.Extension;
                                            string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;
                                            admDocAttachObj.DOC_NAME = fupUpload.FileName;
                                            admDocAttachObj.DOC_TYPE = tempFileInfoObj.Extension;
                                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                            {
                                                admDocAttachObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                            }
                                            else
                                            {
                                                admDocAttachObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                            }
                                            BusinessObject.Sales.FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == CurrSlNo);
                                            if (fileDetailsObj == null)
                                            {
                                                FileDetailsList.Add(new BusinessObject.Sales.FileDetails() { SlNo = CurrSlNo, SoFile = HttpContext.Current.Request.Files[0] });
                                            }
                                            else
                                            {
                                                fileDetailsObj.SoFile = HttpContext.Current.Request.Files[0];
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
                                        DocAttachList = new List<DebitCreditUploads>();
                                        slno = 1;
                                    }
                                    else
                                    {
                                        slno = DocAttachList.Max(itm => itm.DOC_SEQ_NO);
                                        slno++;
                                    }
                                    if (FileDetailsList == null)
                                    {
                                        FileDetailsList = new List<BusinessObject.Sales.FileDetails>();
                                    }

                                    admDocAttachObj = new DebitCreditUploads();
                                    admDocAttachObj.DOC_PK = 0;
                                    admDocAttachObj.DOC_SEQ_NO = (short)slno;
                                    tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                    string attachmentFileFormat = tempFileInfoObj.Extension;
                                    string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;
                                    admDocAttachObj.DOC_NAME = fupUpload.FileName;
                                    admDocAttachObj.DOC_TYPE = tempFileInfoObj.Extension;
                                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                    {
                                        admDocAttachObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                    }
                                    else
                                    {
                                        admDocAttachObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                    }
                                    admDocAttachObj.DOC_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                    FileDetailsList.Add(new BusinessObject.Sales.FileDetails() { SlNo = slno, SoFile = HttpContext.Current.Request.Files[0] });
                                    DocAttachList.Add(admDocAttachObj);
                                }
                            }

                            BindGrid(ControlsEnum.FILEUPLOAD);
                            ResetForm(ControlsEnum.ADDITEM);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);
                        }
                        break;
                    #endregion
                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEM:
                        if (DocAttachList != null && DocAttachList.Count > 0)
                        {
                            int selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                DocAttachList = DocAttachList.Where(row => selectedItemPK != row.DOC_SEQ_NO).ToList();
                                if (FileDetailsList != null)
                                    FileDetailsList = FileDetailsList.Where(row => selectedItemPK != row.SlNo).ToList();
                            }
                        }
                        BindGrid(ControlsEnum.FILEUPLOAD);
                        ResetForm(ControlsEnum.ADDITEM);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);

                        break;
                    #endregion
                    #region EDITITEMUPLOAD
                    case ActionsEnum.EDITITEMUPLOAD:
                        if (DocAttachList != null && DocAttachList.Count > 0)
                        {
                            int selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                admDocAttachObj = DocAttachList.SingleOrDefault(row => selectedItemPK == row.DOC_SEQ_NO);
                                GetUIValuesFromObject(ControlsEnum.SELECTEDDOC);
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);
                        break;
                    #endregion
                    #region PRINTCRDRNOTE
                    case ActionsEnum.PRINTCRDRNOTE:
                        HiddenField hdfinvPK = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfListinvPK"));
                        HiddenField hdfinvType = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfListInvType"));
                        HiddenField hdfinvCategory = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfListinvCategory"));
                        HiddenField hdfinvGroup = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfListGroup"));

                        if (Convert.ToInt32(hdfinvCategory.Value) == (int)SalesInvoiceCategory.Advanced)
                        {
                            if (hdfinvType.Value.ToString().ToLower() == "2" || hdfinvType.Value.ToString().ToLower() == "3")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                   hdfinvPK.Value.ToString() + "&APPTYPE=" + ApplicationType.DSIJ + "&APPSUBTYPE=" + Convert.ToInt32(ApplicationSubType.ADVANCEINVOICE) + "&TRXTYPE=" + ApplicationType.DSIJ) + "');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                    hdfinvPK.Value.ToString() + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=11") + "');", true);
                            }
                        }
                        else if (Convert.ToInt32(hdfinvGroup.Value) == (int)SalesInvoiceGroup.Miscellaneous)
                        {
                            if (!string.IsNullOrEmpty(hdfinvType.Value))
                            {
                                if (Convert.ToInt32(hdfinvType.Value) == (int)SalesInvoiceType.Domestic)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfinvPK.Value.ToString() + "&APPTYPE=" + ApplicationType.MSIT + "&APPSUBTYPE=1") + "');", true);
                                }
                                else if (Convert.ToInt32(hdfinvType.Value) == (int)SalesInvoiceType.Export)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfinvPK.Value.ToString() + "&APPTYPE=" + ApplicationType.MSIT + "&APPSUBTYPE=2") + "');", true);
                                }
                                else if (Convert.ToInt32(hdfinvType.Value) == (int)SalesInvoiceType.Proforma)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfinvPK.Value.ToString() + "&APPTYPE=" + ApplicationType.MSIT + "&APPSUBTYPE=3") + "');", true);
                                }
                            }
                        }
                        else
                        {
                            if (hdfinvType.Value != string.Empty)
                            {
                                if (hdfinvType.Value.ToString().ToLower() == "1")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        hdfinvPK.Value.ToString() + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=1") + "');", true);
                                }
                                else if (hdfinvType.Value.ToString().ToLower() == "2")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        hdfinvPK.Value.ToString() + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=2") + "');", true);
                                }
                                else if (hdfinvType.Value.ToString().ToLower() == "3")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        hdfinvPK.Value.ToString() + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=3") + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region SHOW INV ITEMS
                    case ActionsEnum.SHOWINVITEMS:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalcTotalSplit", "$(document).ready(function(){CalculateTotalSplit();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCrDrSplitUp]','" + GetLocalResourceObject("ReceiptSplit").ToString() + "','950','300');", true);
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
                finCrDrHdrNoteServiceClient = null;
                CommonServiceClient = null;
            }
        }

        private void SaveTransaction(DebitCreditHeader debitCreditHeaderObj, int workflowFlag)
        {
            long? result = 0;
            string savePath = string.Empty;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (debitCreditHeaderObj == null)
                debitCreditHeaderObj = new DebitCreditHeader();
            #region Transaction Log and Application Code
            if (Convert.ToInt32(ddlMode.SelectedValue) == (int)DebitCreditModeEnum.DEBIT)
            {
                debitCreditHeaderObj.ATL_APP_TYPE = ApplicationType.DNT;
            }
            else if (Convert.ToInt32(ddlMode.SelectedValue) == (int)DebitCreditModeEnum.CREDIT)
            {
                debitCreditHeaderObj.ATL_APP_TYPE = ApplicationType.CNT;
            }
            #endregion
            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            debitCreditHeaderObj.USER_PK = Convert.ToInt16(wkfDetails.UserPK);
            debitCreditHeaderObj.WKF_APPLICATION = (int)CurrPK;
            debitCreditHeaderObj.WKF_COMMENTS = wkfDetails.Comments;
            debitCreditHeaderObj.WKF_TRX_FLAG = workflowFlag;
            debitCreditHeaderObj.WKF_PROCESS = wkfDetails.ProcessID;
            debitCreditHeaderObj.WKF_REFERENCE = wkfDetails.ReferenceID;
            debitCreditHeaderObj.WKF_TASK = wkfDetails.TaskID;
            debitCreditHeaderObj.WKF_TASK_ACTION = wkfDetails.ActionID;
            action = wkfDetails.ActionText;
            if (isCancelled)
                debitCreditHeaderObj.ATL_ACTION = (byte)LogAction.CANCEL;
            else
                debitCreditHeaderObj.ATL_ACTION = (byte)LogAction.SUBMIT;
            #endregion
            string CrDrNo = string.Empty;
            string xmlDoc = CommonFunctions.XmlSerialize<DebitCreditHeader>(debitCreditHeaderObj);
            result = BusinessLogic.Sales.DebitCreditBL.SaveCreditDebitSales(xmlDoc, out CrDrNo);
            if (result > 0)// Save Success ! do WorkFlow
            {
                #region File Upload
                if (workflowFlag == (int)(WorkflowTransactionFlag.SAVEANDSUBMIT))
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
                        savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();
                    }

                    foreach (DebitCreditUploads obj in debitCreditHeaderObj.FileList)
                    {
                        //string filePath = savePath + obj.AttachmentFileName;
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
                                fileDetailsObj.SoFile.SaveAs(attachedFileInfo.FullName);

                            }
                        }
                    }
                    #endregion
                }
                #endregion

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
                if (string.IsNullOrEmpty(CrDrNo))
                    CrDrNo = lblDrCrNo.Text.Trim();
                object[] args = new object[2];
                args[0] = Resources.PageNameRes.CreditDebitNotesTrading;
                args[1] = CrDrNo;
                litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CreditDebitNotesTrading);

                CurrPK = (int)result;
                ucrWrkf.ApplicationID = (int)CurrPK;

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
                    GetFieldValues(ControlsEnum.DRCRHDRLIST);
                    SetFieldValues(ControlsEnum.DRCRHDRLIST);
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
                    litErrorMsg.Text = Resources.PageNameRes.CreditDebitNotesTrading + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.CreditDebitNotesTrading + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.DUPLICATERECORDS)//Ref No already exist
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_RefnoExist").ToString()) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (result == -26)//Debit Credit Note is referred in other transactions.
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_CrdrUsed").ToString()) + "','" + Resources.Messages.Information + "');", true);
                }
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CreditDebitNotesTrading);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
            }
        }

        private bool IsValidCNAmount()
        {
            bool isvalid = true;
            if (Convert.ToByte(ddlMode.SelectedValue) == (int)DebitCreditModeEnum.CREDIT)
            {
                //FinCrDrHdrNoteService finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                foreach (GridViewRow grvRow in grdInvoiceList.Rows)
                {
                    Label lblInvoiceAmount = (Label)grvRow.FindControl("lblInvoiceAmount");
                    TextBox txtNoteFor = (TextBox)grvRow.FindControl("txtNoteFor");
                    HiddenField hdfTotalCNAmount = (HiddenField)grvRow.FindControl("hdfTotalCNAmount");
                    decimal invAmount = 0;
                    decimal RaiseNoreAmnt = 0;
                    decimal TotalCNAmount = 0;
                    decimal.TryParse(lblInvoiceAmount.Text, out invAmount);
                    decimal.TryParse(txtNoteFor.Text, out RaiseNoreAmnt);
                    decimal.TryParse(hdfTotalCNAmount.Value, out TotalCNAmount);
                    if ((invAmount < (TotalCNAmount + RaiseNoreAmnt)))
                    {
                        isvalid = false;
                        txtNoteFor.CssClass = "small-a numeric border-red";
                    }
                    else
                    {
                        txtNoteFor.CssClass = "small-a numeric";
                    }
                }
            }
            return isvalid;
        }

        private void SetSplitCount()
        {
            try
            {
                if (DebitCreditHeaderSession != null)
                {
                    DebitCreditTrxMapping objTrxMpg = DebitCreditHeaderSession.DebitCreditTrxMapping.SingleOrDefault(r => r.CDM_INVOICE_CUS_HDR == InvoicePK);
                    if (objTrxMpg != null && objTrxMpg.ItemDetail != null && objTrxMpg.ItemDetail.Count() > 0 && objTrxMpg.ItemDetail.Sum(split => split.CDS_AMOUNT) > 0)
                    {
                        hdfSplitCount.Value = "1";
                    }
                    else
                    {
                        hdfSplitCount.Value = "0";
                    }
                }
                else
                {
                    hdfSplitCount.Value = "0";
                }
            }
            catch { hdfSplitCount.Value = "0"; }
        }

        private void SetUILineItemTaxView(GridViewRow grdRowLineItem)
        {
            taxList = new List<SOInvoiceTaxHdr>();
            List<SOInvoiceTaxHdr> taxListTemp = new List<SOInvoiceTaxHdr>();
            decimal taxpercentage = 0;
            decimal basevalue = 0;
            decimal ttaxamt = 0;
            decimal hdftax = 0;
            decimal hdftotalamt = 0;

            TextBox txtAmount = (TextBox)grdRowLineItem.FindControl("txtSumSplit");
            HiddenField hdfTaxAmt = (HiddenField)grdRowLineItem.FindControl("hdfTAXSplitTotal");
            hdftax = hdfTaxAmt.Value != string.Empty ? Convert.ToDecimal(hdfTaxAmt.Value) : 0;
            HiddenField hdfTotalAmt = (HiddenField)grdRowLineItem.FindControl("hdfSumSplit");
            hdftotalamt = hdfTotalAmt.Value != string.Empty ? Convert.ToDecimal(hdfTotalAmt.Value) : 0;
            taxpercentage = hdftax / (hdftotalamt == 0 ? 1 : hdftotalamt);
            basevalue = (txtAmount.Text != string.Empty ? Convert.ToDecimal(txtAmount.Text) : 0) / (1 + taxpercentage);
            ttaxamt = (txtAmount.Text != string.Empty ? Convert.ToDecimal(txtAmount.Text) : 0 - basevalue);


            LinkButton lbnTAXSplitTotal = (LinkButton)grdRowLineItem.FindControl("lbnTAXSplitTotal");
            HiddenField hdfInvCusDtlPK = (HiddenField)grdRowLineItem.FindControl("hdfInvCusDtlPK");
            if (Convert.ToDecimal(hdfTaxAmt.Value) > 0)
            {
                //InvPk = InvoicePK;
                //GetFieldValues(ControlsEnum.INVOICEDETAILS);
                InvItemPk = Convert.ToInt64(hdfInvCusDtlPK.Value);
                GetFieldValues(ControlsEnum.INVOICEITEMDETAILS);
                if (objInvoiceItemDetails != null && objInvoiceItemDetails.Count > 0)
                {


                    //FIN_INVOICE_CUS_DTL ObjInvDet = objInvoiceDetails[0].FIN_INVOICE_CUS_DTL.SingleOrDefault(inv => inv.CID_PK ==  Convert.ToInt64(hdfInvCusDtlPK.Value));
                    //if (ObjInvDet != null)
                    //{
                    decimal InvAmount = objInvoiceItemDetails[0].CID_AMOUNT;
                    decimal TotalTaxPercentage = 1;
                    List<FIN_INVOICE_CUS_TAX_DTL> objInvoiceTaxList = objInvoiceItemDetails[0].FIN_INVOICE_CUS_TAX_DTL.Where(inv => inv.CIT_TAX_CATEGORY == (byte)TaxType.Tax).ToList();
                    if (objInvoiceTaxList != null && objInvoiceTaxList.Count > 0)
                    {
                        TotalTaxPercentage = objInvoiceTaxList.Sum(tx => (tx.CIT_TAX_AMT * 100) / InvAmount);
                        foreach (FIN_INVOICE_CUS_TAX_DTL invtaxdet in objInvoiceTaxList)
                        {
                            SOInvoiceTaxHdr ObjTaxDtl = new SOInvoiceTaxHdr();
                            decimal IndividualPercentage = (invtaxdet.CIT_TAX_AMT * 100) / InvAmount;
                            //decimal TaxAmnt = (invtaxdethdr.CDS_TAX * IndividualPercentage) / TotalTaxPercentage;
                            decimal tAxSplitTotalAmount = Convert.ToDecimal(hdftax);
                            decimal TaxAmnt = (tAxSplitTotalAmount * IndividualPercentage) / TotalTaxPercentage;
                            double Amount = Math.Round(Convert.ToDouble(TaxAmnt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                            ObjTaxDtl.CIT_TAX = Convert.ToInt32(invtaxdet.CIT_TAX);
                            ObjTaxDtl.CIT_TAX_AMT = Amount;
                            ObjTaxDtl.CIT_TAX_CATEGORY = invtaxdet.CIT_TAX_CATEGORY;
                            //ObjTaxDtl.CIT_TAX_CID_AMOUNT = (invtaxdethdr.CDS_AMOUNT).ToString();
                            ObjTaxDtl.CIT_TAX_CID_AMOUNT = txtAmount.Text;
                            if (invtaxdet.FIN_TAX_MST != null)
                            {
                                ObjTaxDtl.CIT_TAX_CODE = invtaxdet.FIN_TAX_MST.TAX_CODE;
                                ObjTaxDtl.CIT_TAX_RATE = Convert.ToDouble(invtaxdet.FIN_TAX_MST.TAX_RATE);
                                ObjTaxDtl.CIT_TAX_TEXT = invtaxdet.FIN_TAX_MST.TAX_DISP_NAME;
                                ObjTaxDtl.CIT_NAME = invtaxdet.FIN_TAX_MST.TAX_HEAD;
                            }
                            taxListTemp.Add(ObjTaxDtl);

                        }
                        if (taxListTemp != null && taxListTemp.Count > 0)
                        {
                            var finPymntTaxHdr = from finObj in taxListTemp
                                                 group finObj by new
                                                 {
                                                     finObj.CIT_TAX
                                                 } into finGrpdObj
                                                 select new SOInvoiceTaxHdr
                                                 {
                                                     CIT_TAX = finGrpdObj.Key.CIT_TAX,
                                                     CIT_TAX_AMT = finGrpdObj.Sum(x => x.CIT_TAX_AMT),
                                                     CIT_TAX_CATEGORY = finGrpdObj.FirstOrDefault().CIT_TAX_CATEGORY,
                                                     CIT_TAX_CID_AMOUNT = finGrpdObj.Sum(x => Convert.ToDecimal(x.CIT_TAX_CID_AMOUNT)).ToString(),
                                                     CIT_TAX_CODE = finGrpdObj.FirstOrDefault().CIT_TAX_CODE,
                                                     CIT_TAX_RATE = finGrpdObj.FirstOrDefault().CIT_TAX_RATE,
                                                     CIT_TAX_TEXT = finGrpdObj.FirstOrDefault().CIT_TAX_TEXT,
                                                     CIT_NAME = finGrpdObj.FirstOrDefault().CIT_NAME

                                                 };
                            taxList = finPymntTaxHdr.ToList();
                        }
                    }
                    //}

                }
            }
            //}
            //            }
            //        }
            //    }
            //}
        }


        private void SetUIHeaderTaxView(GridViewRow grdRow)
        {

            taxList = new List<SOInvoiceTaxHdr>();
            List<SOInvoiceTaxHdr> taxListTemp = new List<SOInvoiceTaxHdr>();
            decimal taxpercentage = 0;
            decimal basevalue = 0;
            decimal ttaxamt = 0;
            decimal hdftax = 0;
            decimal hdftotalamt = 0;
            int ItemIncluded = 0;
            finCrDrNoteMpgObj = CommonFunctions.Initilize<FIN_CRDR_NOTE_MPG>();
            HiddenField hdfCrDbMpgPK = (HiddenField)grdRow.FindControl(GetLocalResourceObject("hdfCrDbMpgPK").ToString());
            HiddenField hdfItemIncluded = (HiddenField)grdRow.FindControl("hdfItemIncluded");
            finCrDrNoteMpgObj.CDM_PK = hdfCrDbMpgPK == null ? 0 : Convert.ToInt64(hdfCrDbMpgPK.Value);
            finCrDrNoteMpgObj.CDM_CRDR_NOTE_HDR = CurrPK;
            HiddenField hdfInvoicePK = (HiddenField)grdRow.FindControl(GetLocalResourceObject("hdfInvoicePK").ToString());
            finCrDrNoteMpgObj.CDM_INVOICE_CUS_HDR = hdfInvoicePK == null ? 0 : Convert.ToInt32(hdfInvoicePK.Value);
            finCrDrNoteMpgObj.CDM_INVOICE_VND_HDR = null;
            TextBox txtAmount = (TextBox)grdRow.FindControl(GetLocalResourceObject("txtNoteFor").ToString());
            finCrDrNoteMpgObj.CDM_AMOUNT = txtAmount.Text != string.Empty ? Convert.ToDecimal(txtAmount.Text) : 0;
            finCrDrNoteMpgObj.CDM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);

            HiddenField hdfTaxAmt = (HiddenField)grdRow.FindControl("hdfTaxAmt");
            hdftax = hdfTaxAmt.Value != string.Empty ? Convert.ToDecimal(hdfTaxAmt.Value) : 0;
            HiddenField hdfTotalAmt = (HiddenField)grdRow.FindControl("hdfTotalAmt");
            int.TryParse(hdfItemIncluded.Value, out ItemIncluded);

            hdftotalamt = hdfTotalAmt.Value != string.Empty ? Convert.ToDecimal(hdfTotalAmt.Value) : 0;
            taxpercentage = hdftax / (hdftotalamt == 0 ? 1 : hdftotalamt);
            if (ItemIncluded == 1)
            {
                ttaxamt = (Convert.ToDecimal(txtAmount.Text) * taxpercentage);
            }
            else
            {
                basevalue = (txtAmount.Text != string.Empty ? Convert.ToDecimal(txtAmount.Text) : 0) / (1 + taxpercentage);
                ttaxamt = (Convert.ToDecimal(txtAmount.Text) - basevalue);
            }


            //Label lblTotalTax = (Label)grdRow.FindControl(GetLocalResourceObject("lblTotalTax").ToString());
            finCrDrNoteMpgObj.CDM_TAX_AMOUNT = Math.Round(Convert.ToDecimal(ttaxamt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
            if (finCrDrNoteMpgObj.CDM_AMOUNT > 0)
            {
                //adding detail tax
                if (finCrDrNoteMpgObj.CDM_TAX_AMOUNT > 0)
                {
                    InvPk = Convert.ToInt64(hdfInvoicePK.Value);
                    GetFieldValues(ControlsEnum.INVOICEDETAILS);
                    if (objInvoiceDetails.Count > 0)
                    {
                        if (objInvoiceDetails[0] != null)
                        {
                            decimal InvAmount = objInvoiceDetails[0].ICH_AMOUNT_TC;
                            decimal TotalTaxPercentage = 1;
                            List<FIN_INVOICE_CUS_TAX_HDR> objInvoiceTaxList = objInvoiceDetails[0].FIN_INVOICE_CUS_TAX_HDR.Where(inv => inv.ISH_TAX_CATEGORY == (byte)TaxType.Tax).ToList();
                            if (objInvoiceTaxList != null && objInvoiceTaxList.Count > 0)
                            {
                                TotalTaxPercentage = objInvoiceTaxList.Sum(tx => (tx.ISH_TAX_AMT * 100) / InvAmount);
                                foreach (FIN_INVOICE_CUS_TAX_HDR invtaxhdr in objInvoiceTaxList)
                                {
                                    SOInvoiceTaxHdr ObjTaxDtl = new SOInvoiceTaxHdr();
                                    decimal IndividualPercentage = (invtaxhdr.ISH_TAX_AMT * 100) / InvAmount;
                                    decimal TaxAmnt = (finCrDrNoteMpgObj.CDM_TAX_AMOUNT * IndividualPercentage) / TotalTaxPercentage;
                                    double Amount = Math.Round(Convert.ToDouble(TaxAmnt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                    ObjTaxDtl.CIT_TAX = Convert.ToInt32(invtaxhdr.ISH_TAX);
                                    ObjTaxDtl.CIT_TAX_AMT = Amount;
                                    ObjTaxDtl.CIT_TAX_CATEGORY = invtaxhdr.ISH_TAX_CATEGORY;
                                    ObjTaxDtl.CIT_TAX_CID_AMOUNT = finCrDrNoteMpgObj.CDM_AMOUNT.ToString();
                                    if (invtaxhdr.FIN_TAX_MST != null)
                                    {
                                        ObjTaxDtl.CIT_TAX_CODE = invtaxhdr.FIN_TAX_MST.TAX_CODE;
                                        ObjTaxDtl.CIT_TAX_RATE = Convert.ToDouble(invtaxhdr.FIN_TAX_MST.TAX_RATE);
                                        ObjTaxDtl.CIT_TAX_TEXT = invtaxhdr.FIN_TAX_MST.TAX_DISP_NAME;
                                        ObjTaxDtl.CIT_NAME = invtaxhdr.FIN_TAX_MST.TAX_HEAD;
                                    }
                                    taxListTemp.Add(ObjTaxDtl);
                                }

                            }
                            if (taxListTemp != null && taxListTemp.Count > 0)
                            {
                                var finPymntTaxHdr = from finObj in taxListTemp
                                                     group finObj by new
                                                     {
                                                         finObj.CIT_TAX
                                                     } into finGrpdObj
                                                     select new SOInvoiceTaxHdr
                                                     {
                                                         CIT_TAX = finGrpdObj.Key.CIT_TAX,
                                                         CIT_TAX_AMT = finGrpdObj.Sum(x => x.CIT_TAX_AMT),
                                                         CIT_TAX_CATEGORY = finGrpdObj.FirstOrDefault().CIT_TAX_CATEGORY,
                                                         CIT_TAX_CID_AMOUNT = finGrpdObj.Sum(x => Convert.ToDecimal(x.CIT_TAX_CID_AMOUNT)).ToString(),
                                                         CIT_TAX_CODE = finGrpdObj.FirstOrDefault().CIT_TAX_CODE,
                                                         CIT_TAX_RATE = finGrpdObj.FirstOrDefault().CIT_TAX_RATE,
                                                         CIT_TAX_TEXT = finGrpdObj.FirstOrDefault().CIT_TAX_TEXT,
                                                         CIT_NAME = finGrpdObj.FirstOrDefault().CIT_NAME

                                                     };
                                taxList = finPymntTaxHdr.ToList();
                            }
                        }
                    }
                }
            }
        }

        private void DcsSplitSave()
        {
            FinCrDrHdrNoteService finCrDrHdrNoteServiceClient;
            finCrDrHdrNoteServiceClient = null;
            List<FIN_CRDR_NOTE_DTL> tempInvoiceSOSplitList;
            if (!IsValid)
            {
                litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
            }
            else//valid
            {
                if (grdDCSplit.Rows.Count >= 1)
                {
                    ////HiddenField lblTotalPayNowFooterSplit = (HiddenField)(grdDCSplit.FooterRow.FindControl("hdfTotalPayNowFooterSplit"));

                    ////if (lblTotalPayNowFooterSplit != null && !string.IsNullOrEmpty(lblTotalPayNowFooterSplit.Value) && !string.IsNullOrEmpty(lblDCSplitReceiveNow.Text))
                    ////{
                    //Convert.ToDecimal(lblTotalPayNowFooterSplit.Value) == Convert.ToDecimal(lblDCSplitReceiveNow.Text.Trim().Replace(",", "")) - Convert.ToDecimal(lbltaxSplitpopup.Text.Trim().Replace(",", ""))
                    ////if (Convert.ToDecimal(lblTotalPayNowFooterSplit.Value) == Convert.ToDecimal(lblDCSplitReceiveNow.Text.Trim().Replace(",", "")))
                    ////{
                    divErrorLabel.Visible = false;
                    finCrDrCusMpgList = new List<FIN_CRDR_NOTE_DTL>();
                    finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                    finCrDrHdrNoteServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                    finCrDrCusMpgList = (List<FIN_CRDR_NOTE_DTL>)SetUIValuesToObject(ControlsEnum.CRDRSPLITLIST);
                    if (finCrDrCusMpgList != null)
                    {
                        tempInvoiceSOSplitList = CrDrSplitList;
                        tempInvoiceSOSplitList.Where(dtl => dtl.FIN_CRDR_NOTE_MPG.CDM_INVOICE_CUS_HDR == InvoicePK)
                            .ToList().ForEach(dtl => tempInvoiceSOSplitList.Remove(dtl));
                        //// tax split start
                        //if (finCrDrCusMpgList.Count > 0)
                        //{ 

                        //}
                        //// tax split end
                        finCrDrCusMpgList.ForEach(dtl =>
                        {
                            dtl.FIN_CRDR_NOTE_MPG = new FIN_CRDR_NOTE_MPG()
                            {
                                CDM_INVOICE_CUS_HDR = InvoicePK
                            };
                            ////////
                            if (dtl.CDS_TAX > 0)
                            {
                                InvItemPk = Convert.ToInt64(dtl.CDS_INVOICE_CUS_DTL);
                                GetFieldValues(ControlsEnum.INVOICEITEMDETAILS);
                                if (objInvoiceItemDetails != null && objInvoiceItemDetails.Count > 0)
                                {
                                    decimal InvAmount = objInvoiceItemDetails[0].CID_AMOUNT;
                                    decimal TotalTaxPercentage = 1;
                                    double Amount = 0;
                                    List<FIN_INVOICE_CUS_TAX_DTL> objInvoiceTaxList = objInvoiceItemDetails[0].FIN_INVOICE_CUS_TAX_DTL.Where(inv => inv.CIT_TAX_CATEGORY == (byte)TaxType.Tax).ToList();
                                    if (objInvoiceTaxList != null && objInvoiceTaxList.Count > 0)
                                    {
                                        TotalTaxPercentage = objInvoiceTaxList.Sum(tx => (tx.CIT_TAX_AMT * 100) / InvAmount);
                                        foreach (FIN_INVOICE_CUS_TAX_DTL invtaxdet in objInvoiceTaxList)
                                        {
                                            FIN_CRDR_NOTE_TAX_DTL ObjTaxDtl = new FIN_CRDR_NOTE_TAX_DTL();
                                            decimal IndividualPercentage = (invtaxdet.CIT_TAX_AMT * 100) / InvAmount;
                                            decimal TaxAmnt = (dtl.CDS_TAX * IndividualPercentage) / TotalTaxPercentage;
                                            if (isLineItemTaxEnabled)
                                                Amount = Math.Round(Convert.ToDouble(TaxAmnt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                            else
                                                Amount = Math.Round(Convert.ToDouble(TaxAmnt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                            ObjTaxDtl.NTD_AMOUNT = Convert.ToDecimal(Amount);
                                            ObjTaxDtl.NTD_PK = 0;
                                            ObjTaxDtl.NTD_TAX = invtaxdet.CIT_TAX;
                                            dtl.FIN_CRDR_NOTE_TAX_DTL.Add(ObjTaxDtl);
                                        }
                                    }
                                }
                            }
                            ///////////////
                            tempInvoiceSOSplitList.Add(dtl);
                        });
                        CrDrSplitList = tempInvoiceSOSplitList;


                        //Test
                        //result = finCrDrHdrNoteServiceClient.SaveReceiptSplit(finCrDrCusMpgList);
                        //if (result >= 0)
                        //{
                        //    litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                        //    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), GetLocalResourceObject("Receipt_Allocation").ToString());
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                            "ClosePopup();", true);
                        //}
                    }
                    ////}
                    ////else
                    ////{
                    ////    Label TotalPayNowFooterSplit = (Label)(grdDCSplit.FooterRow.FindControl("lblTotalPayNowFooterSplit"));
                    ////    TotalPayNowFooterSplit.Text = GetFormattedCurrency(lblTotalPayNowFooterSplit.Value);
                    ////    divErrorLabel.Visible = true;
                    ////    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('Total split amount should be equal to Receive now amount','" + Resources.Messages.Information + "');", true);
                    ////    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCrDrSplitUp]','" + GetLocalResourceObject("ReceiptSplit").ToString() + "','900','300');", true);
                    ////}

                    ////}
                    ////else
                    ////{
                    ////    divErrorLabel.Visible = true;
                    ////    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('Total split amount should be equal to Receive now amount','" + Resources.Messages.Information + "');", true);
                    ////    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCrDrSplitUp]','" + GetLocalResourceObject("ReceiptSplit").ToString() + "','900','300');", true);
                    ////}
                    finCrDrHdrNoteServiceClient = null;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                        "ClosePopup();", true);
                }

            }
        }

        private void ResetForm(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                case ControlsEnum.TAXPOPUPGRID:
                    grdTaxDetails.DataSource = null;
                    grdTaxDetails.DataBind();
                    break;
                #region ADDITEM
                case ControlsEnum.ADDITEM:
                    //ddlType.ClearSelection();
                    anchorFile.Visible = false;
                    vrfFileUpload.Enabled = true;
                    CurrSlNo = 0;
                    anchorFile.Attributes.Remove("onclick");
                    break;
                #endregion
            }
        }

        private void SetHdrTax()
        {
            if (this.tempDebitCreditHeaderTax != null)
            {
                txtOtherCharge.ToolTip = txtOtherCharge.Text = GetFormattedNumber(this.tempDebitCreditHeaderTax.Sum(x => x.NTH_TAX_AMT));
                DebitCreditHeaderTax = this.tempDebitCreditHeaderTax;
            }
        }


        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            TextBox txtNoteFor;
            LinkButton lbnTotalTax;
            Button lnkAllocation;
            HiddenField hdfhasjournalized;

            try
            {

                #region Grid Data Row
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    #region grdInvoiceList
                    if (((GridView)sender).ID == "grdInvoiceList")
                    {
                        lnkAllocation = e.Row.FindControl("lnkAllocation") as Button;
                        lbnTotalTax = e.Row.FindControl("lbnTotalTax") as LinkButton;
                        txtNoteFor = e.Row.FindControl("txtNoteFor") as TextBox;
                        hdfhasjournalized = e.Row.FindControl("hdfhasjournalized") as HiddenField;
                        HiddenField hdfItemIncluded = e.Row.FindControl("hdfItemIncluded") as HiddenField;
                        if (DebitCreditHeaderSession != null && DebitCreditHeaderSession.DebitCreditTrxMapping.Count > 0)
                        {
                            if (DebitCreditHeaderSession.DebitCreditTrxMapping[e.Row.RowIndex].ItemDetail != null && DebitCreditHeaderSession.DebitCreditTrxMapping[e.Row.RowIndex].ItemDetail.Sum(dtl => dtl.CDS_AMOUNT) > 0)
                            {
                                hdfItemIncluded.Value = "1";
                                hdfSplitCount.Value = "1";
                                txtNoteFor.Text = txtNoteFor.ToolTip = Math.Round(DebitCreditHeaderSession.DebitCreditTrxMapping[e.Row.RowIndex].CDM_AMOUNT - DebitCreditHeaderSession.DebitCreditTrxMapping[e.Row.RowIndex].CDM_TAX_AMOUNT,
                                  Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            }
                            else
                            {
                                hdfItemIncluded.Value = "0";
                                hdfSplitCount.Value = "0";
                            }

                            if (DebitCreditHeaderSession.CDH_ICH_CATEGORY == (int)SalesInvoiceCategory.Advanced)
                            {
                                lnkAllocation.Visible = false;
                                lbnTotalTax.CssClass = "nomargin";
                                lbnTotalTax.Enabled = false;
                                txtNoteFor.Enabled = true;
                                hdfhasjournalized.Value = "True";
                            }
                            else
                            {
                                lbnTotalTax.CssClass = "text-underline nomargin";
                                lbnTotalTax.Enabled = true;
                                txtNoteFor.Enabled = false;
                            }
                        }
                    }
                    #endregion
                    #region grdUploads
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
                    #endregion
                }
                #endregion
                #region Grid Header Row
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    #region grdDCSplit
                    if (((GridView)sender).ID == "grdDCSplit")
                    {
                        if (DebitCreditHeaderSession != null && DebitCreditHeaderSession.DebitCreditTrxMapping != null && DebitCreditHeaderSession.DebitCreditTrxMapping.Count > 0)
                        {
                            DebitCreditTrxMapping objTrxMpg = DebitCreditHeaderSession.DebitCreditTrxMapping.SingleOrDefault(r => r.CDM_INVOICE_CUS_HDR == InvoicePK);
                            if (objTrxMpg != null && objTrxMpg.ItemDetail != null && objTrxMpg.ItemDetail.Count > 0)
                            {
                                e.Row.Cells[3].Text = GetLocalResourceObject("DCQTY").ToString() + " (" + objTrxMpg.ItemDetail[0].CDS_UOM_TEXT + ")";
                            }
                        }
                    }
                    #endregion
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

        private decimal GetCNTotalAmount(long InvoicePk)
        {
            decimal totalCN = 0;
            FinCrDrHdrNoteService finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
            totalCN = finCrDrHdrNoteServiceClient.GetTotalCNAmount(InvoicePk, CurrPK);
            return Math.Round(totalCN, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

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
                GetFieldValues(ControlsEnum.DRCRHDRLIST);
                SetFieldValues(ControlsEnum.DRCRHDRLIST);
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
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDeleteCrdr.PreRender += new EventHandler(btnAction_PreRender);
            this.btnListPrint.PreRender += new EventHandler(btnAction_PreRender);
            // this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnJournalize.PreRender += new EventHandler(btnAction_PreRender);
            //this.btnAlert.PreRender += new EventHandler(btnAction_PreRender);
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
            this.btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            //this.lbnSOListing.PreRender += new EventHandler(btnAction_PreRender);
            //this.lnbDeliveryOrder.PreRender += new EventHandler(btnAction_PreRender);
            //this.lnbSalesInvoice.PreRender += new EventHandler(btnAction_PreRender);
            //this.lnbAdvanceInvoice.PreRender += new EventHandler(btnAction_PreRender);
            //this.lbnSalesReceipt.PreRender += new EventHandler(btnAction_PreRender);
            //this.lnbCrDrNote.PreRender += new EventHandler(btnAction_PreRender);
            //this.lnbAcPayables.PreRender += new EventHandler(btnAction_PreRender);
            //this.lnbMiscellaneous.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkList.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkDetail.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveDCSplit.PreRender += new EventHandler(btnAction_PreRender);
            this.imgPopupAdd.PreRender += new EventHandler(btnAction_PreRender);
            this.btnApply.PreRender += new EventHandler(btnAction_PreRender);

            //Load
            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnDeleteCrdr.Load += new EventHandler(btnAction_Load);
            this.btnListPrint.Load += new EventHandler(btnAction_Load);
            //  this.btnCancel.Load += new EventHandler(btnAction_Load);
            this.btnEditforCancel.Load += new EventHandler(btnAction_Load);
            this.btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            this.btnJournalize.Load += new EventHandler(btnAction_Load);
            //this.btnAlert.Load += new EventHandler(btnAction_Load);
            this.btnNew.Load += new EventHandler(btnAction_Load);
            this.btnEdit.Load += new EventHandler(btnAction_Load);
            this.btnView.Load += new EventHandler(btnAction_Load);
            this.btnPrint.Load += new EventHandler(btnAction_Load);
            //this.lbnSOListing.Load += new EventHandler(btnAction_Load);
            //this.lnbDeliveryOrder.Load += new EventHandler(btnAction_Load);
            //this.lnbSalesInvoice.Load += new EventHandler(btnAction_Load);
            //this.lnbAdvanceInvoice.Load += new EventHandler(btnAction_Load);
            //this.lbnSalesReceipt.Load += new EventHandler(btnAction_Load);
            //this.lnbCrDrNote.Load += new EventHandler(btnAction_Load);
            //this.lnbMiscellaneous.Load += new EventHandler(btnAction_Load);
            //this.lnbAcPayables.Load += new EventHandler(btnAction_Load);
            this.lnkList.Load += new EventHandler(btnAction_Load);
            this.lnkDetail.Load += new EventHandler(btnAction_Load);
            this.btnSaveDCSplit.Load += new EventHandler(btnAction_Load);
            this.imgPopupAdd.Load += new EventHandler(btnAction_Load);
            this.btnApply.Load += new EventHandler(btnAction_Load);
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
        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalDigits.Value);
        }
        public string GetFormattedRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfRateFormat.Value);
        }
        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }
        public string GetFormattedCurrencyWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithSeperator.Value);

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
                GetFieldValues(ControlsEnum.DRCRHDRLIST);
                SetFieldValues(ControlsEnum.DRCRHDRLIST);
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
                if (grdInvoiceList.Rows != null && (grdInvoiceList.Rows.Count > 0 || CurrPK > 0))
                    txtCustomer.Enabled = false;
                else
                    txtCustomer.Enabled = true;

                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){return ShowListing();});", true);
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
                //if (ucrWrkf.RefID > 0)
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(3);});", true);
                //}
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculateTotal();});", true);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculateAmount();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculateTotalSplit();});", true);

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails();});", true);
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

            PID = pid;
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
                }
                if (pid == 1)
                {
                    PageProcessID = ucrWrkf.ProcessID;
                }
                base.WkfPageUrl = path;
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
            DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), currentUser.CurrentDeptPK);
            if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
            {
                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
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
            PAYMENTHDRLIST,
            PAYMENTHDRENTRY,
            PAYMENTMPGENTRY,
            PAYMENTMPGLIST,
            BANK,
            PAYMENTHDRINVLISTBYPK,
            PAYMENTHDRENTRYBYPK,
            PAYMENTNO,
            EXCHANGERATE,
            //Dr Cr Note
            DEFAULT,
            FORCRDRNOTE,
            SELECTEDPIINVOICES,
            SELECTEDSIINVOICES,
            CREDIT,
            DEBIT,
            DRCRHEADERENTRY,
            DRCRMPGENTRY,
            WRKFSUBMIT,
            DRCRHDRLIST,
            DRCRHDRLISTJOURNAL,
            DRCRMPGLIST,
            JOURNALIZE,
            FINHEADER,
            CRDRTYPE,
            FINPERIOD,
            GETDRCRPKBYJOURNALPK,
            FILLWORKFLOWSTATUS,
            GETCRDRHDRBYPK,
            COMPANY,
            CRDRSPLITLIST,
            CRDRVNDMPGLIST,
            CRDRVNDHDR,        
            //Biju
            TAXTYPES,
            TAXPOPUPGRID,
            //TAXPOPUPGRIDTEMP,
            SOTYPE,
            TAXSETTINGS,
            BASECURRENCY,
            INVOICEDETAILS,
            TAXMYR,
            GETTAXLIST,
            TAXSPLITUP,
            TAXSPLITUPLINEITEMWISE,
            INVOICEITEMDETAILS,
            INVITEMLIST,
            CHECKCRDRUSEDINOTHERTRNS,
            //LINEITEMTAXSETTINGS,
            FINHEADERSTATUS,
            CRDRALLOCATEDAMOUNT,
            BALANCEAMOUNTSPLIT,
            DRCRCANCELCHECK,
            FILEUPLOAD,
            ADDITEM,
            SELECTEDDOC,
            DRCRGET,
            PEDINGINVLIST,
            DRCRHEADER,
            UPDATEGRIDVALTOOBJECT,
            INVCATEGORY,
            CUSTOMERDETAILS
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
        /// Attachment Task Enum
        /// </summary>
        public enum DocTaskEnum
        {
            //PURCHASEINVOICE = 9,
            //EXPENSEINVOICE = 10
            DEBITCREDITNOTE = 20
        }
        /// <summary>
        /// Attachment Module Enum
        /// </summary>
        public enum DocModuleEnum
        {
            FINANCE = 8
        }
        #endregion
    }
}