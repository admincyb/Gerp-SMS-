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


namespace ERPSMS_v01.Sales
{
    public partial class DebitCreditNote : ERP.Store.UI.WorkFlowBasePage
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
        private List<ADM_DOC_ATTACH> DocAttachList
        {
            get
            {
                return this.ViewState[ViewstateStrings.DocAttachList] == null ? new List<ADM_DOC_ATTACH>() : (List<ADM_DOC_ATTACH>)(this.ViewState[ViewstateStrings.DocAttachList]);
            }
            set
            {
                this.ViewState[ViewstateStrings.DocAttachList] = value;
            }
        }

        /// <summary>
        /// Store Files in Attachment List
        /// </summary>
        private List<BusinessObject.POInvoicing.FileDetails> FileDetailsList
        {
            get
            {
                return this.Session[ERP.Utilities.SessionStrings.FilePODetailsList] == null ? null : (List<BusinessObject.POInvoicing.FileDetails>)this.Session[ERP.Utilities.SessionStrings.FilePODetailsList];
            }
            set
            {
                this.Session[ERP.Utilities.SessionStrings.FilePODetailsList] = value;
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

        #endregion
        DataTable dtTaxDetails;
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        User currentUser;

        //page related Entity Object
        DataTable dtSOData;
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
        private long InvPk = 0;
        private long InvItemPk = 0;
        private string transactionNumber;
        private bool updateCrDr;
        private bool isDrCrUsedInOtherTrns = false;
        int JournalPK;
        int hasjournalized;
        int creditDebitType;
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


        private string[] _operators = { "-", "+", "/", "*", "^" };
        private Func<double, double, double>[] _operations = {
            (a1, a2) => a1 - a2,
            (a1, a2) => a1 + a2,
            (a1, a2) => a1 / a2,
            (a1, a2) => a1 * a2,
            (a1, a2) => Math.Pow(a1, a2)
        };
        ADM_DOC_ATTACH admDocAttachObj;

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

                    CrDrSplitList = null;
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

                    //hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
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
                    if (SelectedInvoicesCrDr != null)
                    {
                        selectedInvoiceList = SelectedInvoicesCrDr;
                        GetFieldValues(ControlsEnum.SELECTEDSIINVOICES);
                    }
                    else
                    {
                        int.TryParse(pid, out InvoiceType);
                    }
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
                        GetFieldValues(ControlsEnum.DRCRGET);
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
                                //   EntryStatus = EntryStatus.VIEWMODE;
                            }
                            //SetUIEditView(commonActions);

                            hdfCrDrNumber.Value = CurrPK.ToString();
                            // Get CrDrDetails


                            GetFieldValues(ControlsEnum.GETCRDRHDRBYPK);
                            GetFieldValues(ControlsEnum.DRCRMPGLIST);
                            GetUIValuesFromObject(ControlsEnum.DRCRHEADERENTRY);
                            SetFieldValues(ControlsEnum.SELECTEDSIINVOICES);
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.FILEUPLOAD);
                            SetFieldValues(ControlsEnum.FILEUPLOAD);
                        }
                        else
                        {
                            //Sets data key for the gird
                            string[] datakeyarray;
                            datakeyarray = new string[1];
                            datakeyarray[0] = Resources.DataFieldRes.CrDrPk;
                            grdCrDbHdr.DataKeyNames = datakeyarray;
                            if (SelectedInvoicesCrDr != null && SelectedInvoicesCrDr.Count > 0)
                            {
                                SetCancelRef(Convert.ToInt32(CurrPK));
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
                                }
                                selectedInvoiceList = SelectedInvoicesCrDr;
                                GetFieldValues(ControlsEnum.SELECTEDSIINVOICES);
                                SetFieldValues(ControlsEnum.SELECTEDSIINVOICES);
                                lblDrCrNo.ToolTip = lblDrCrNo.Text = "[NEW]";
                                //lblDrCrNo.Text = "[NEW]";
                                txtDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                                chkIsReceiptAlcnReq.Checked = false;
                            }
                            else
                            {
                                SelectedInvoicesCrDr = new List<long>();
                                GetFieldValues(ControlsEnum.DRCRHDRLIST);
                                SetFieldValues(ControlsEnum.DRCRHDRLIST);
                                EntryStatus = EntryStatus.LISTMODE;
                                PageIndex = "1";
                                uclPaging.TotalPages = TotalPages;
                                uclPaging.CurrentPage = 1;
                            }
                        }
                        EnableDisableDrCrMode();
                        GetFieldValues(ControlsEnum.TAXMYR);
                        SetFieldValues(ControlsEnum.TAXMYR);
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
            try
            {
                switch (type)
                {
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
                        finCrDrNoteHdrObj.CDH_CUSTOMER = string.IsNullOrEmpty(hdfCustomerID.Value) ? 0 : Convert.ToInt32(hdfCustomerID.Value);
                        //if (hdfCustomerID.Value != null && hdfCustomerID.Value != "0" && hdfCustomerID.Value != "")
                        //{
                        //    Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = hdfCustomerID.Value;
                        //    Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = txtCustomer.Text;
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
                    #region Debit Credit Hdr List
                    case ControlsEnum.DRCRHDRLIST:
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        finCrDrNoteHdrObj = CommonFunctions.Initilize<FIN_CRDR_NOTE_HDR>();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdCrDbHdr.PageSize;
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.CrDrDate : SortBy;
                        serviceUtilityObj.ThenBy = ThenBy = ThenBy == null ? Resources.DataFieldRes.CrDrNo : ThenBy;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;
                        serviceUtilityObj.FilterBy = string.Empty;
                        serviceUtilityObj.FilterValue = string.Empty;
                        finCrDrNoteHdrObj.CDH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finCrDrNoteHdrObj.CDH_VENDOR = 0;
                        finCrDrNoteHdrObj.CDH_BIZUNIT = currentUser.SBUID;
                        finCrDrNoteHdrObj.CDH_CUSTOMER = string.IsNullOrEmpty(hdfCustomerID.Value) ? 0 : Convert.ToInt32(hdfCustomerID.Value);
                        finCrDrNoteHdrObj.CDH_TYPE = Convert.ToInt32(ddlCreditDebitType.SelectedValue) > 0 ? Convert.ToByte(ddlCreditDebitType.SelectedValue) : (byte)0;
                        if (hdfCustomerID.Value != null && hdfCustomerID.Value != "0" && hdfCustomerID.Value != "")
                        {
                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = hdfCustomerID.Value;
                            Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = txtCustomer.Text;
                        }
                        finCrDrNoteHdrObj.CDH_PK = string.IsNullOrEmpty(hdfCrDrNumber.Value) ? 0 : Convert.ToInt64(hdfCrDrNumber.Value);
                        finCrDrNoteHdrObj.CDH_CRTD_BY = currentUser.PKUser;
                        serviceUtilityObj.FilterDate = string.IsNullOrEmpty(txtSearchDateFrom.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtSearchDateFrom.Text.Trim());
                        serviceUtilityObj.FilterToDate = string.IsNullOrEmpty(txtSearchDateTo.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtSearchDateTo.Text.Trim());
                        finCrDrNoteHdrObj.CDH_COMPANY = Convert.ToInt32(ddlCompanySrch.SelectedValue);
                        Type = 2;
                        Status = Convert.ToInt32(ddlStatus.SelectedValue);
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
                        finCrDrNoteHdrObj.CDH_CUSTOMER = string.IsNullOrEmpty(hdfCustomerID.Value) ? 0 : Convert.ToInt32(hdfCustomerID.Value);
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
                    #region Generate Transaction Number DEBIT
                    case ControlsEnum.DEBIT:
                        //Generate Transaction Number
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        int appSubTypeDN = hdfInvoiceType.Value == "1" ? (int)AppSubTypeCNSales.DOMESTIC : (int)AppSubTypeCNSales.EXPORT_COMMERICAL;
                        DateTime drDate = string.IsNullOrEmpty(txtDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtDate.Text.Trim());
                        transactionNumber = poPaymentServiceClient.GetPaymentNo(ApplicationType.DN, appSubTypeDN, currentUser.CurrentDeptPK,
                        drDate, currentUser.PKUser, updateCrDr, 0, Convert.ToInt32(ddlCompany.SelectedItem.Value));
                        hdfCrDrTrxNo.Value = transactionNumber;
                        break;
                    #endregion
                    #region Generate Transaction Number CREDIT
                    case ControlsEnum.CREDIT:
                        //Generate Transaction Number
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        int appSubTypeCN = hdfInvoiceType.Value == "1" ? (int)AppSubTypeCNSales.DOMESTIC : (int)AppSubTypeCNSales.EXPORT_COMMERICAL;
                        DateTime crDate = string.IsNullOrEmpty(txtDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtDate.Text.Trim());
                        transactionNumber = poPaymentServiceClient.GetPaymentNo(ApplicationType.CN, appSubTypeCN, currentUser.CurrentDeptPK,
                        crDate, currentUser.PKUser, updateCrDr, 0, Convert.ToInt32(ddlCompany.SelectedItem.Value));
                        hdfCrDrTrxNo.Value = transactionNumber;
                        break;
                    #endregion
                    #region Get Exchange rate
                    case ControlsEnum.EXCHANGERATE:
                        //Get Exchange rate
                        //finCrDrNoteHdrObj.CDH_DATE
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);

                        if (hdfExRateBizUnit.Value == "1")//Fetch all Exchange rates(ie, No Bizunit Filteration)
                        {
                            ExchageRate = poPaymentServiceClient.GetConversionFactor(
                               string.IsNullOrEmpty(hdfInvoiceCurr.Value) ? 0 : Convert.ToInt32(hdfInvoiceCurr.Value), currentUser.BaseCurrency,
                                                                Convert.ToDateTime(string.IsNullOrEmpty(txtDate.Text) ? DateTime.Now.ToShortDateString() : txtDate.Text), 0);

                        }
                        else//Fetch Exchange rate  w. r. to Current Bizunit
                        {
                            ExchageRate = poPaymentServiceClient.GetConversionFactor(
                              string.IsNullOrEmpty(hdfInvoiceCurr.Value) ? 0 : Convert.ToInt32(hdfInvoiceCurr.Value), currentUser.BaseCurrency,
                                                               Convert.ToDateTime(string.IsNullOrEmpty(txtDate.Text) ? DateTime.Now.ToShortDateString() : txtDate.Text),
                                                               currentUser.SBUID);
                        }
                        if (ExchageRate > 0)
                        {
                            hdfExchangeCurr.Value = ExchageRate.ToString();
                            txtExchangeRate.Text = ExchageRate.ToString();
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
                        workflowStatusList = CommonServiceClient.GetWorkFlowStatus(ApplicationType.CN, (byte)AppSubTypeDebitCreditNote.CUSTOMER, Convert.ToByte(CommonConstants.ACTIVE));
                        workflowStatusExportList = CommonServiceClient.GetWorkFlowStatus(ApplicationType.CN, (byte)AppSubTypeDebitCreditNote.EXPORT_COMMERICAL, Convert.ToByte(CommonConstants.ACTIVE));
                        workflowStatusDomesticList = CommonServiceClient.GetWorkFlowStatus(ApplicationType.CN, (byte)AppSubTypeDebitCreditNote.DOMESTIC, Convert.ToByte(CommonConstants.ACTIVE));
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
                        FinCrDrCusTrxMpgObj.CID_INVOICE_HDR = InvoicePK;//@@
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
                            finTrxHdrObj.FTH_REF_TYPE = ApplicationType.DNJ;
                        else
                            finTrxHdrObj.FTH_REF_TYPE = ApplicationType.CNJ;
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
                    #region FILEUPLOAD
                    case ControlsEnum.FILEUPLOAD:
                        if (CurrPK > 0)
                        {
                            POInvoiceService poInvoiceServiceClient = new POInvoiceService();
                            poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                            ADM_DOC_ATTACH admDocAttachObj = CommonFunctions.Initilize<ADM_DOC_ATTACH>();
                            serviceUtilityObj = new ServiceUtility();
                            //serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                            //serviceUtilityObj.PageSize = grdPOInvoiceList.PageSize;
                            admDocAttachObj.DOC_TASK_ID = Convert.ToInt32(CurrPK);// (int)finInvoiceVndHdrObj.IVH_PK;
                            admDocAttachObj.DOC_TASK = (int)DocTaskEnum.DEBITCREDITNOTE;
                            DocAttachList = poInvoiceServiceClient.GetDocAttachments(admDocAttachObj, serviceUtilityObj);
                            poInvoiceServiceClient = null;
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
                    #region TAX POPUP GRID TEMP
                    case ControlsEnum.TAXPOPUPGRIDTEMP://Biju
                        ddlPopupTaxType.Focus();
                        BindGrid(controlType);
                        break; 
                    #endregion
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
                        BindDropDownList(controlType);
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
                if (finCrDrNoteHdrList != null && finCrDrNoteHdrList.Count > 0)
                {
                    if (finCrDrNoteHdrList[0].CDH_STATUS == (int)WkfStatusEnum.DRAFTED)
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
                TextBox txtSumSplit;
                TextBox txtQtySplit;
                TextBox txtRateSplit;
                SalesInvoice SalesInvoiceClient;
                SalesInvoiceClient = null;
                switch (controlType)
                {
                    #region SOINVHEADER
                    case ControlsEnum.SOINVHEADER:

                        break;
                    #endregion
                    #region Debit Credit Hdr
                    case ControlsEnum.DRCRHEADERENTRY:
                        //if (CrDrSplitList != null && CrDrSplitList.Count > 0)
                        //{
                        //    CrDrSplitList = null;
                        //    DcsSplitSave();
                        //}
                        finCrDrNoteHdrObj = new FIN_CRDR_NOTE_HDR();
                        finCrDrNoteHdrObj.CDH_PK = CurrPK;
                        finCrDrNoteHdrObj.CDH_NO = (string.IsNullOrEmpty(lblDrCrNo.Text) || lblDrCrNo.Text.Trim().Equals("[NEW]")) ? string.Empty
                                                    : lblDrCrNo.Text.Trim();
                        finCrDrNoteHdrObj.CDH_DATE = String.IsNullOrEmpty(txtDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtDate.Text.Trim());
                        finCrDrNoteHdrObj.CDH_VENDOR = null;
                        finCrDrNoteHdrObj.CDH_CUSTOMER = string.IsNullOrEmpty(hdfVendorPK.Value) ? 0 : Convert.ToInt32(hdfVendorPK.Value);
                        finCrDrNoteHdrObj.CDH_VND_CUS_ACCOUNT = string.IsNullOrEmpty(hdfVendorAccountNo.Value) ? null : ERP.Utilities.CommonFunctions.NullableInt(hdfVendorAccountNo.Value);//Convert.ToInt32(ddlAccountNo.SelectedValue);

                        finCrDrNoteHdrObj.CDH_TYPE = Convert.ToByte(ddlMode.SelectedValue);
                        finCrDrNoteHdrObj.CDH_CURRENCY = string.IsNullOrEmpty(hdfInvoiceCurr.Value) ? 1 : Convert.ToInt32(hdfInvoiceCurr.Value);
                        ////finCrDrNoteHdrObj.CDH_AMOUNT_TC = Convert.ToDecimal(txtPaidAmount.Text.Trim());
                        finCrDrNoteHdrObj.CDH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                        finCrDrNoteHdrObj.CDH_REMARKS2 = HttpUtility.HtmlEncode(txtRemarks2.Text.Trim());
                        finCrDrNoteHdrObj.CDH_BASE_CURR = currentUser.BaseCurrency;
                        //GetFieldValues(ControlsEnum.EXCHANGERATE);
                        finCrDrNoteHdrObj.CDH_EXCHG_RATE = string.IsNullOrEmpty(hdfExchangeCurr.Value) ? 1 : Convert.ToDouble(hdfExchangeCurr.Value);
                        //finCrDrNoteHdrObj.CDH_EXCHG_RATE = string.IsNullOrEmpty(hdfExchangeCurr.Value) ? 1 : Convert.ToDouble(txtExchangeRate.Text);
                        ////finCrDrNoteHdrObj.CDH_AMOUNT_BC = (Convert.ToDecimal(txtHdrTotal.Text.Trim())) * Convert.ToDecimal(finCrDrNoteHdrObj.CDH_EXCHG_RATE);
                        finCrDrNoteHdrObj.CDH_STATUS = WkfStatus;
                        finCrDrNoteHdrObj.CDH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finCrDrNoteMpgList = new List<FIN_CRDR_NOTE_MPG>();
                        finCrDrNoteMpgList = (List<FIN_CRDR_NOTE_MPG>)SetUIValuesToObject(ControlsEnum.DRCRMPGENTRY);
                        finCrDrNoteHdrObj.CDH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        finCrDrNoteHdrObj.CDH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        finCrDrNoteHdrObj.CDH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                        finCrDrNoteHdrObj.CDH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                        finCrDrNoteHdrObj.CDH_CRTD_DT = DateTime.Now;
                        finCrDrNoteHdrObj.CDH_MOD_DT = LastModifiedTime;
                        finCrDrNoteHdrObj.CDH_REF_NO = HttpUtility.HtmlDecode(txtInstrumentNo.Text.Trim());
                        finCrDrNoteHdrObj.CDH_SHIP_CHARGE = txtShipCharge.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtShipCharge.Text.Trim());
                        finCrDrNoteHdrObj.CDH_OTHER_CHARGE = Convert.ToDecimal(txtOtherCharge.Text.Trim());
                        finCrDrNoteHdrObj.CDH_IS_DELETED = Convert.ToBoolean(0);
                        finCrDrNoteHdrObj.CDH_NO_RCP_ALLOC = chkIsReceiptAlcnReq.Checked ? (byte)1 : (byte)0;
                        if (String.IsNullOrEmpty(txtInstrumentDate.Text.Trim()))
                            finCrDrNoteHdrObj.CDH_REF_DATE = null;
                        else
                            finCrDrNoteHdrObj.CDH_REF_DATE = Convert.ToDateTime(txtInstrumentDate.Text.Trim());
                        finCrDrNoteHdrObj.CDH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);
                        if (finCrDrNoteMpgList != null && finCrDrNoteMpgList.Count > 0)
                        {
                            finCrDrNoteHdrObj.CDH_TAX_AMOUNT = Math.Round(finCrDrNoteMpgList.Sum(c => c.CDM_TAX_AMOUNT), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            finCrDrNoteHdrObj.CDH_AMOUNT_TC = Math.Round(finCrDrNoteMpgList.Sum(c => c.CDM_AMOUNT), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            finCrDrNoteHdrObj.CDH_AMOUNT_BC = Math.Round(finCrDrNoteMpgList.Sum(c => c.CDM_AMOUNT) * Convert.ToDecimal(finCrDrNoteHdrObj.CDH_EXCHG_RATE), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        }

                        if (finCrDrNoteMpgList != null && finCrDrNoteMpgList.Count > 0)
                        {
                            finCrDrNoteHdrObj.FIN_CRDR_NOTE_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_CRDR_NOTE_MPG>();
                            finCrDrNoteMpgList.ForEach(dtl =>
                                {
                                    finCrDrNoteHdrObj.FIN_CRDR_NOTE_MPG.Add(dtl);
                                    finCrDrCusMpgList = CrDrSplitList.Where(mpg => mpg.FIN_CRDR_NOTE_MPG.CDM_INVOICE_CUS_HDR == dtl.CDM_INVOICE_CUS_HDR).ToList();
                                    if (finCrDrCusMpgList.Count() > 0)
                                    {
                                        finCrDrCusMpgList.ForEach(mpg =>
                                        {
                                            mpg.FIN_CRDR_NOTE_MPG = null;
                                            dtl.FIN_CRDR_NOTE_DTL.Add(mpg);
                                        });
                                    }
                                });

                        }


                        //Biju
                        if (this.FinCrDrNoteTaxHeader != null && FinCrDrNoteTaxHeader.Count > 0)
                        {
                            FIN_CRDR_NOTE_TAX_HDR objTemp;
                            List<FIN_CRDR_NOTE_TAX_HDR> ItemList = new List<FIN_CRDR_NOTE_TAX_HDR>();
                            foreach (FIN_CRDR_NOTE_TAX_HDR objItem in FinCrDrNoteTaxHeader)
                            {
                                objTemp = CommonFunctions.Initilize<FIN_CRDR_NOTE_TAX_HDR>();
                                objTemp.NTH_CRDR_NOTE_HDR = objItem.NTH_CRDR_NOTE_HDR;
                                objTemp.NTH_NAME = objItem.NTH_NAME;
                                objTemp.NTH_PK = objItem.NTH_PK;
                                objTemp.NTH_TAX = objItem.NTH_TAX;
                                objTemp.NTH_TAX_AMT = objItem.NTH_TAX_AMT;
                                objTemp.NTH_TAX_CATEGORY = objItem.NTH_TAX_CATEGORY;
                                objTemp.NTH_TYPE = objItem.NTH_TYPE;
                                ItemList.Add(objTemp);

                            }
                            ItemList.ForEach(dtl => finCrDrNoteHdrObj.FIN_CRDR_NOTE_TAX_HDR.Add(dtl));
                            //if (finCrDrNoteHdrObj.FIN_CRDR_NOTE_TAX_HDR == null)
                            //{
                            //    finCrDrNoteHdrObj.FIN_CRDR_NOTE_TAX_HDR = new System.Data.Objects.DataClasses.EntityCollection<FIN_CRDR_NOTE_TAX_HDR>();
                            //}
                            //FinCrDrNoteTaxHeader.ForEach(dtl =>
                            //{
                            //    finCrDrNoteHdrObj.FIN_CRDR_NOTE_TAX_HDR.Add(dtl);
                            //});

                        }
                        retObject = finCrDrNoteHdrObj;
                        break;
                    #endregion
                    #region Credit Debit Maping
                    case ControlsEnum.DRCRMPGENTRY:
                        rowID = 0;
                        decimal TotalInvCrdrAmnt = 0;
                        if (grdInvoiceList.Rows.Count > 0)
                        {
                            HiddenField hdfCrdrFooter = (HiddenField)grdInvoiceList.FooterRow.FindControl("hdfPayNowFooter");
                            decimal.TryParse(hdfCrdrFooter.Value, out TotalInvCrdrAmnt);
                        }
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            decimal taxpercentage = 0;
                            decimal basevalue = 0;
                            decimal ttaxamt = 0;
                            decimal hdftax = 0;
                            decimal hdftotalamt = 0;
                            int ItemIncluded = 0;
                            decimal RaiseNote = 0;
                            finCrDrNoteMpgObj = CommonFunctions.Initilize<FIN_CRDR_NOTE_MPG>();
                            hdfCrDbMpgPK = (HiddenField)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("hdfCrDbMpgPK").ToString());
                            finCrDrNoteMpgObj.CDM_PK = hdfCrDbMpgPK == null ? 0 : Convert.ToInt64(hdfCrDbMpgPK.Value);
                            finCrDrNoteMpgObj.CDM_CRDR_NOTE_HDR = CurrPK;
                            hdfInvoicePK = (HiddenField)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("hdfInvoicePK").ToString());
                            finCrDrNoteMpgObj.CDM_INVOICE_CUS_HDR = hdfInvoicePK == null ? 0 : Convert.ToInt32(hdfInvoicePK.Value);
                            finCrDrNoteMpgObj.CDM_INVOICE_VND_HDR = null;
                            txtAmount = (TextBox)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("txtNoteFor").ToString());
                            decimal.TryParse(txtAmount.Text, out RaiseNote);
                            if (RaiseNote == 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_RaiseNote").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                break;
                            }
                            finCrDrNoteMpgObj.CDM_AMOUNT = txtAmount.Text != string.Empty ? Convert.ToDecimal(txtAmount.Text) : 0;


                            finCrDrNoteMpgObj.CDM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);

                            HiddenField hdfTotalTax = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTotalTax");

                            HiddenField hdfTaxAmt = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTaxAmt");
                            hdftax = hdfTaxAmt.Value != string.Empty ? Convert.ToDecimal(hdfTaxAmt.Value) : 0;
                            HiddenField hdfTotalAmt = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTotalAmt");
                            hdftotalamt = hdfTotalAmt.Value != string.Empty ? Convert.ToDecimal(hdfTotalAmt.Value) : 0;
                            taxpercentage = hdftax / (hdftotalamt == 0 ? 1 : hdftotalamt);
                            basevalue = (txtAmount.Text != string.Empty ? Convert.ToDecimal(txtAmount.Text) : 0) / (1 + taxpercentage);
                            ttaxamt = (txtAmount.Text != string.Empty ? Convert.ToDecimal(txtAmount.Text) : 0 - basevalue);
                            lbnTotalTax = (LinkButton)grdInvoiceList.Rows[rowID].FindControl("lbnTotalTax");
                            //finCrDrNoteMpgObj.CDM_TAX_AMOUNT = Math.Round(Convert.ToDecimal(ttaxamt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                            finCrDrNoteMpgObj.CDM_TAX_AMOUNT = Math.Round(Convert.ToDecimal(hdfTotalTax.Value), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                            HiddenField hdfItemIncluded = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfItemIncluded");
                            int.TryParse(hdfItemIncluded.Value, out ItemIncluded);
                            if (ItemIncluded == 1)
                            {
                                finCrDrNoteMpgObj.CDM_AMOUNT = finCrDrNoteMpgObj.CDM_AMOUNT + finCrDrNoteMpgObj.CDM_TAX_AMOUNT;
                            }

                            decimal shipCharge = txtShipCharge.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtShipCharge.Text.Trim());
                            decimal otherCharge = txtOtherCharge.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtOtherCharge.Text.Trim());
                            decimal ivnShipCharge = (shipCharge / TotalInvCrdrAmnt) * finCrDrNoteMpgObj.CDM_AMOUNT;
                            decimal ivnOtherCharge = (otherCharge / TotalInvCrdrAmnt) * finCrDrNoteMpgObj.CDM_AMOUNT;
                            finCrDrNoteMpgObj.CDM_SHIP_CHARGE = Math.Round(ivnShipCharge, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            finCrDrNoteMpgObj.CDM_OTHER_CHARGE = Math.Round(ivnOtherCharge, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                            if (finCrDrNoteMpgObj.CDM_AMOUNT > 0)
                            {
                                //adding header tax
                                if (finCrDrNoteMpgObj.CDM_TAX_AMOUNT > 0)
                                {
                                    InvPk = Convert.ToInt64(hdfInvoicePK.Value);
                                    GetFieldValues(ControlsEnum.INVOICEDETAILS);
                                    if (objInvoiceDetails.Count > 0)
                                    {
                                        if (objInvoiceDetails[0] != null)
                                        {
                                            decimal InvAmount = objInvoiceDetails[0].ICH_AMOUNT_TC;
                                            decimal TotalTaxPercentage = objInvoiceDetails[0].FIN_INVOICE_CUS_TAX_HDR.Where(txx => txx.ISH_TAX_CATEGORY == (byte)TaxType.Tax).Sum(tx => (tx.ISH_TAX_AMT * 100) / (InvAmount == 0 ? 1 : InvAmount));
                                            List<FIN_INVOICE_CUS_TAX_HDR> TaxList = objInvoiceDetails[0].FIN_INVOICE_CUS_TAX_HDR.Where(txx => txx.ISH_TAX_CATEGORY == (byte)TaxType.Tax).ToList();
                                            foreach (FIN_INVOICE_CUS_TAX_HDR invtaxhdr in TaxList)
                                            {
                                                FIN_CRDR_NOTE_TAX_DTL ObjTaxDtl = new FIN_CRDR_NOTE_TAX_DTL();
                                                decimal IndividualPercentage = (invtaxhdr.ISH_TAX_AMT * 100) / (InvAmount == 0 ? 1 : InvAmount);
                                                decimal TaxAmnt = (finCrDrNoteMpgObj.CDM_TAX_AMOUNT * IndividualPercentage) / TotalTaxPercentage;
                                                double Amount = CommonFunctions.DoubleFormatRound(Convert.ToDouble(TaxAmnt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                                ObjTaxDtl.NTD_AMOUNT = Convert.ToDecimal(Amount);
                                                ObjTaxDtl.NTD_PK = 0;
                                                ObjTaxDtl.NTD_TAX = invtaxhdr.ISH_TAX;
                                                finCrDrNoteMpgObj.FIN_CRDR_NOTE_TAX_DTL.Add(ObjTaxDtl);
                                            }
                                        }
                                    }
                                }
                                /////////////////
                                finCrDrNoteMpgList.Add(finCrDrNoteMpgObj);
                            }
                            rowID++;



                        }
                        retObject = finCrDrNoteMpgList;
                        break;
                    #endregion
                    #region Journalize
                    case ControlsEnum.JOURNALIZE:
                        ////Start
                        if (EntryStatus == EntryStatus.LISTMODE)
                        {

                            foreach (GridViewRow grdrow in grdCrDbHdr.Rows)
                            {
                                RadioButton rbtn;
                                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                                if (rbtn.Checked)
                                {
                                    bIsChecked = true;
                                    CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCrDrPk")).Value);
                                    hdfCrDrNumber.Value = CurrPK.ToString();
                                    Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                    Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                    Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomerName")).Text;
                                    isPosted = Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfPostedJ")).Value) == 1 ? true : false;
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
                        foreach (GridViewRow grdrowinv in grdInvoiceList.Rows)
                        {
                            HiddenField hdfhasjournalized;
                            hdfhasjournalized = (HiddenField)grdrowinv.FindControl("hdfhasjournalized");
                            if (Convert.ToBoolean(hdfhasjournalized.Value) != true)
                            {
                                hasjournalized = 1;//not Jornalized
                                break;
                            }
                            else
                            {
                                hasjournalized = 0;//Jornalized
                            }
                        }
                        if (bIsChecked)
                        {
                            if ((hasjournalized == 1 && (Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.DEBIT || Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.CREDIT)) || (hasjournalized == 0 && Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.CREDIT) || (hasjournalized == 0 && Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.DEBIT))
                            {
                                if (Approved == 2)
                                {
                                    Session[ERP.Utilities.SessionStrings.JournalMode] = null;
                                    Session[ERP.Utilities.SessionStrings.TrxPK] = null;
                                    GetFieldValues(ControlsEnum.DRCRHDRLISTJOURNAL);
                                    if (finCrDrNoteHdrList[0].CDH_TYPE == 1)
                                    {
                                        ucrJournalize.TransactionType = ApplicationType.DNJ;
                                        Session[ERP.Utilities.SessionStrings.TransactionType] = ApplicationType.DNJ;
                                        Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.DNJ;
                                        Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.DNJ;
                                        Session[ERP.Utilities.SessionStrings.JournalHead] = hdfJournalHeader.Value = GetLocalResourceObject("Debit_Note_Journal").ToString();
                                    }
                                    else if (finCrDrNoteHdrList[0].CDH_TYPE == 2)
                                    {
                                        ucrJournalize.TransactionType = ApplicationType.CNJ;
                                        Session[ERP.Utilities.SessionStrings.TransactionType] = ApplicationType.CNJ;
                                        Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.CNJ;
                                        Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.CNJ;
                                        Session[ERP.Utilities.SessionStrings.JournalHead] = hdfJournalHeader.Value = GetLocalResourceObject("Credit_Note_Journal").ToString();
                                    }
                                    ucrJournalize.TransactionPK = (int)CurrPK;
                                    Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                                    ucrJournalize.JournalizePK = 0;
                                    Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                    Session[ERP.Utilities.SessionStrings.TransactionNo] = finCrDrNoteHdrList[0].CDH_NO;
                                    Session[ERP.Utilities.SessionStrings.TransactionDate] = finCrDrNoteHdrList[0].CDH_DATE;
                                    Session[ERP.Utilities.SessionStrings.TransactionCurrency] = finCrDrNoteHdrList[0].CDH_CURRENCY;
                                    Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.AP;
                                    Session[ERP.Utilities.SessionStrings.AccountPayablePK] = finCrDrNoteHdrList[0].CDH_VENDOR;

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
                                    //EntryStatus = EntryStatus.ENTRYMODE;
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
                                        //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                                    }
                                    else
                                    {
                                        ucrWrkf.ViewType = 0;
                                        //  EntryStatus = EntryStatus.VIEWMODE;
                                        //   Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
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
                                litErrorMsg.Text = GetLocalResourceObject("Msg_InvNotjournalized").ToString();
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
                    #region Debit Credit Hdr workflow
                    case ControlsEnum.WRKFSUBMIT:
                        //if (CrDrSplitList != null && CrDrSplitList.Count > 0)
                        //{
                        //    CrDrSplitList = null;
                        //    DcsSplitSave();
                        //}
                        finCrDrNoteHdrObj = new FIN_CRDR_NOTE_HDR();
                        finCrDrNoteHdrObj.CDH_PK = CurrPK;
                        updateCrDr = true;
                        if (string.IsNullOrEmpty(lblDrCrNo.Text.Trim()) || lblDrCrNo.Text.Trim().Equals("[NEW]"))
                        {

                            if (ddlMode.SelectedValue == "1")
                            {
                                GetFieldValues(ControlsEnum.DEBIT);
                            }
                            else if (ddlMode.SelectedValue == "2")
                            {
                                GetFieldValues(ControlsEnum.CREDIT);
                            }
                            lblDrCrNo.ToolTip = lblDrCrNo.Text = hdfCrDrTrxNo.Value;
                        }
                        finCrDrNoteHdrObj.CDH_NO = lblDrCrNo.Text;
                        finCrDrNoteHdrObj.CDH_DATE = String.IsNullOrEmpty(txtDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtDate.Text.Trim());
                        finCrDrNoteHdrObj.CDH_VENDOR = null;
                        finCrDrNoteHdrObj.CDH_CUSTOMER = string.IsNullOrEmpty(hdfVendorPK.Value) ? 0 : Convert.ToInt32(hdfVendorPK.Value);
                        finCrDrNoteHdrObj.CDH_VND_CUS_ACCOUNT = string.IsNullOrEmpty(hdfVendorAccountNo.Value) ? null : ERP.Utilities.CommonFunctions.NullableInt(hdfVendorAccountNo.Value);//Convert.ToInt32(ddlAccountNo.SelectedValue);

                        finCrDrNoteHdrObj.CDH_TYPE = Convert.ToByte(ddlMode.SelectedValue);
                        finCrDrNoteHdrObj.CDH_CURRENCY = string.IsNullOrEmpty(hdfInvoiceCurr.Value) ? 1 : Convert.ToInt32(hdfInvoiceCurr.Value);


                        //finCrDrNoteHdrObj.CDH_AMOUNT_TC = Convert.ToDecimal(txtPaidAmount.Text.Trim());
                        finCrDrNoteHdrObj.CDH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                        finCrDrNoteHdrObj.CDH_REMARKS2 = HttpUtility.HtmlEncode(txtRemarks2.Text.Trim());
                        finCrDrNoteHdrObj.CDH_BASE_CURR = currentUser.BaseCurrency;
                        // GetFieldValues(ControlsEnum.EXCHANGERATE);
                        finCrDrNoteHdrObj.CDH_EXCHG_RATE = string.IsNullOrEmpty(hdfExchangeCurr.Value) ? 1 : Convert.ToDouble(hdfExchangeCurr.Value);
                        //finCrDrNoteHdrObj.CDH_AMOUNT_BC = (Convert.ToDecimal(txtHdrTotal.Text.Trim())) * Convert.ToDecimal(finCrDrNoteHdrObj.CDH_EXCHG_RATE);
                        finCrDrNoteHdrObj.CDH_STATUS = WkfStatus;
                        finCrDrNoteHdrObj.CDH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finCrDrNoteMpgList = new List<FIN_CRDR_NOTE_MPG>();
                        finCrDrNoteMpgList = (List<FIN_CRDR_NOTE_MPG>)SetUIValuesToObject(ControlsEnum.DRCRMPGENTRY);
                        finCrDrNoteHdrObj.CDH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        finCrDrNoteHdrObj.CDH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        finCrDrNoteHdrObj.CDH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                        finCrDrNoteHdrObj.CDH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                        finCrDrNoteHdrObj.CDH_CRTD_DT = DateTime.Now;
                        finCrDrNoteHdrObj.CDH_MOD_DT = LastModifiedTime;
                        finCrDrNoteHdrObj.CDH_REF_NO = HttpUtility.HtmlDecode(txtInstrumentNo.Text.Trim());
                        finCrDrNoteHdrObj.CDH_SHIP_CHARGE = txtShipCharge.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtShipCharge.Text.Trim());
                        finCrDrNoteHdrObj.CDH_OTHER_CHARGE = Convert.ToDecimal(txtOtherCharge.Text.Trim());
                        finCrDrNoteHdrObj.CDH_IS_DELETED = Convert.ToBoolean(0);
                        finCrDrNoteHdrObj.CDH_NO_RCP_ALLOC = chkIsReceiptAlcnReq.Checked ? (byte)1 : (byte)0;

                        if (String.IsNullOrEmpty(txtInstrumentDate.Text.Trim()))
                            finCrDrNoteHdrObj.CDH_REF_DATE = null;
                        else
                            finCrDrNoteHdrObj.CDH_REF_DATE = Convert.ToDateTime(txtInstrumentDate.Text.Trim());
                        finCrDrNoteHdrObj.CDH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);

                        if (finCrDrNoteMpgList != null && finCrDrNoteMpgList.Count > 0)
                        {
                            finCrDrNoteHdrObj.CDH_TAX_AMOUNT = Math.Round(finCrDrNoteMpgList.Sum(c => c.CDM_TAX_AMOUNT), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            finCrDrNoteHdrObj.CDH_AMOUNT_TC = Math.Round(finCrDrNoteMpgList.Sum(c => c.CDM_AMOUNT), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            finCrDrNoteHdrObj.CDH_AMOUNT_BC = Math.Round(finCrDrNoteMpgList.Sum(c => c.CDM_AMOUNT) * Convert.ToDecimal(finCrDrNoteHdrObj.CDH_EXCHG_RATE), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                        }

                        if (finCrDrNoteMpgList != null && finCrDrNoteMpgList.Count > 0)
                        {
                            finCrDrNoteHdrObj.FIN_CRDR_NOTE_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_CRDR_NOTE_MPG>();
                            finCrDrNoteMpgList.ForEach(dtl =>
                            {
                                finCrDrNoteHdrObj.FIN_CRDR_NOTE_MPG.Add(dtl);
                                finCrDrCusMpgList = CrDrSplitList.Where(mpg => mpg.FIN_CRDR_NOTE_MPG.CDM_INVOICE_CUS_HDR == dtl.CDM_INVOICE_CUS_HDR).ToList();
                                if (finCrDrCusMpgList.Count() > 0)
                                {
                                    finCrDrCusMpgList.ForEach(mpg =>
                                    {
                                        mpg.FIN_CRDR_NOTE_MPG = null;
                                        dtl.FIN_CRDR_NOTE_DTL.Add(mpg);
                                    });
                                }
                            });

                        }
                        if (this.FinCrDrNoteTaxHeader != null && FinCrDrNoteTaxHeader.Count > 0)
                        {
                            FIN_CRDR_NOTE_TAX_HDR objTemp;
                            List<FIN_CRDR_NOTE_TAX_HDR> ItemList = new List<FIN_CRDR_NOTE_TAX_HDR>();
                            foreach (FIN_CRDR_NOTE_TAX_HDR objItem in FinCrDrNoteTaxHeader)
                            {
                                objTemp = CommonFunctions.Initilize<FIN_CRDR_NOTE_TAX_HDR>();
                                objTemp.NTH_CRDR_NOTE_HDR = objItem.NTH_CRDR_NOTE_HDR;
                                objTemp.NTH_NAME = objItem.NTH_NAME;
                                objTemp.NTH_PK = objItem.NTH_PK;
                                objTemp.NTH_TAX = objItem.NTH_TAX;
                                objTemp.NTH_TAX_AMT = objItem.NTH_TAX_AMT;
                                objTemp.NTH_TAX_CATEGORY = objItem.NTH_TAX_CATEGORY;
                                objTemp.NTH_TYPE = objItem.NTH_TYPE;
                                ItemList.Add(objTemp);

                            }
                            ItemList.ForEach(dtl => finCrDrNoteHdrObj.FIN_CRDR_NOTE_TAX_HDR.Add(dtl));
                            //if (finCrDrNoteHdrObj.FIN_CRDR_NOTE_TAX_HDR == null)
                            //{
                            //    finCrDrNoteHdrObj.FIN_CRDR_NOTE_TAX_HDR = new System.Data.Objects.DataClasses.EntityCollection<FIN_CRDR_NOTE_TAX_HDR>();
                            //}
                            //FinCrDrNoteTaxHeader.ForEach(dtl =>
                            //{
                            //    finCrDrNoteHdrObj.FIN_CRDR_NOTE_TAX_HDR.Add(dtl);
                            //});

                        }
                        retObject = finCrDrNoteHdrObj;
                        break;
                    #endregion
                    #region Payment Split
                    case ControlsEnum.CRDRSPLITLIST:
                        rowID = 0;
                        finCrDrCusMpgList = new List<FIN_CRDR_NOTE_DTL>();
                        foreach (GridViewRow grdrow in grdDCSplit.Rows)//
                        {
                            finCrDrCusSoMpgObj = CommonFunctions.Initilize<FIN_CRDR_NOTE_DTL>();
                            hdfDCSplitPK = (HiddenField)grdDCSplit.Rows[rowID].FindControl("hdfDCSplitPK");
                            finCrDrCusSoMpgObj.CDS_PK = hdfDCSplitPK == null ? 0 : Convert.ToInt64(hdfDCSplitPK.Value);
                            finCrDrCusSoMpgObj.CDS_CRDR_NOTE_HDR = CurrPK;
                            finCrDrCusSoMpgObj.CDS_CRDR_NOTE_MPG = CrDrMpgPK;
                            finCrDrCusSoMpgObj.CDS_INVOICE_VND_DTL = null;
                            finCrDrCusSoMpgObj.CDS_PO_DTL = null;
                            //hdfSOPK = (HiddenField)grdDCSplit.Rows[rowID].FindControl("hdfSOPK");
                            hdfInvCusDtlPK = (HiddenField)grdDCSplit.Rows[rowID].FindControl("hdfInvCusDtlPK");
                            finCrDrCusSoMpgObj.CDS_INVOICE_CUS_DTL = hdfInvCusDtlPK == null ? 1 : Convert.ToInt32(hdfInvCusDtlPK.Value);
                            finCrDrCusSoMpgObj.CDS_SO_DTL = null;
                            txtQtySplit = (TextBox)grdDCSplit.Rows[rowID].FindControl("txtQtySplit");
                            finCrDrCusSoMpgObj.CDS_QTY = txtQtySplit == null ? 0 : txtQtySplit.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(txtQtySplit.Text.Trim());
                            finCrDrCusSoMpgObj.CDS_UOM = null;
                            txtRateSplit = (TextBox)grdDCSplit.Rows[rowID].FindControl("txtRateSplit");
                            finCrDrCusSoMpgObj.CDS_RATE = txtRateSplit == null ? 0 : txtRateSplit.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(txtRateSplit.Text.Trim());
                            txtSumSplit = (TextBox)grdDCSplit.Rows[rowID].FindControl("txtSumSplit");
                            finCrDrCusSoMpgObj.CDS_AMOUNT = txtSumSplit == null ? 0 : txtSumSplit.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtSumSplit.Text.Trim());
                            finCrDrCusSoMpgObj.CDS_DISCOUNT = 0;
                            //finCrDrCusSoMpgObj.CDS_TAX = 0;
                            HiddenField hdfTAXSplitTotal = (HiddenField)grdDCSplit.Rows[rowID].FindControl("hdfTAXSplitTotal");
                            finCrDrCusSoMpgObj.CDS_TAX = string.IsNullOrEmpty(hdfTAXSplitTotal.Value) ? 0 : Convert.ToDecimal(hdfTAXSplitTotal.Value.Trim());
                            //finCrDrCusSoMpgObj.CDS_NET_AMOUNT = Convert.ToDecimal(txtSumSplit.Text.Trim()) - (finCrDrCusSoMpgObj.CDS_DISCOUNT + finCrDrCusSoMpgObj.CDS_TAX);
                            decimal SplitSum = txtSumSplit == null ? 0 : txtSumSplit.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtSumSplit.Text.Trim());
                            finCrDrCusSoMpgObj.CDS_NET_AMOUNT = SplitSum - (finCrDrCusSoMpgObj.CDS_DISCOUNT + finCrDrCusSoMpgObj.CDS_TAX);
                            finCrDrCusSoMpgObj.CDS_REMARKS = null;
                            finCrDrCusSoMpgObj.CDS_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            finCrDrCusMpgList.Add(finCrDrCusSoMpgObj);
                            rowID++;
                        }
                        retObject = finCrDrCusMpgList;
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
                    #region DR CR GET
                    case ControlsEnum.DRCRGET:                       
                        if (finCrDrNoteHdrList != null && finCrDrNoteHdrList.Count > 0)
                        {
                            CurrPK = Convert.ToInt32(Request.QueryString["PK"].ToString());
                            if (!string.IsNullOrEmpty(finCrDrNoteHdrList[0].CDH_DEPT.ToString()) && int.TryParse(finCrDrNoteHdrList[0].CDH_DEPT.ToString(), out dept))
                            {
                                Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                base.SetUserDept();
                            }
                            isCancelled = Convert.ToBoolean(finCrDrNoteHdrList[0].CDH_IS_DELETED);
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
                                if (finCrDrNoteHdrList[0].CDH_NO != "[NEW]")
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
                            GetFieldValues(ControlsEnum.FILEUPLOAD);
                            SetFieldValues(ControlsEnum.FILEUPLOAD);


                            FinCrDrNoteTaxHeader = null;
                            FinCrDrNoteTaxHeaderTemp = null;
                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = finCrDrNoteHdrList[0].CDH_CUSTOMER;
                            Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = finCrDrNoteHdrList[0].CRM_CUSTOMER_MST.CUS_NAME;
                            Approved = finCrDrNoteHdrList[0].CDH_STATUS;
                            hdfCancelled.Value = "0";
                            isCancelled = Convert.ToBoolean(finCrDrNoteHdrList[0].CDH_IS_DELETED);
                            if (isCancelled)
                                hdfCancelled.Value = "1";
                            hdfCrDrNumber.Value = CurrPK.ToString();
                            // Get CrDrDetails
                            GetFieldValues(ControlsEnum.DRCRHDRLISTJOURNAL);
                            ActionsEnum Mode = ActionsEnum.VIEW;
                            if (Mode != ActionsEnum.EDITFORCANCEL)
                            {
                                if (InvoiceType == Convert.ToInt32(SalesInvoiceType.Domestic))
                                    FillProcessID(1);
                                else
                                    FillProcessID(3);
                            }
                            GetFieldValues(ControlsEnum.DRCRMPGLIST);
                            SetFieldValues(ControlsEnum.SELECTEDSIINVOICES);
                            GetUIValuesFromObject(ControlsEnum.DRCRHEADERENTRY);

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
                            if (CurrPK > 0 && (CrDrSplitList == null || CrDrSplitList.Count == 0))
                            {
                                GetFieldValues(ControlsEnum.GETCRDRHDRBYPK);
                                if (finCrDrNoteHdrList != null && finCrDrNoteHdrList.Count > 0)
                                {
                                    CrDrSplitList = finCrDrNoteHdrList[0].FIN_CRDR_NOTE_DTL.ToList();
                                }
                            }
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
                        if (finInvoiceVndHdrListForPaymentSplit != null && finInvoiceVndHdrListForPaymentSplit.Count > 0)
                        {
                            //Test
                            lblDCSplitNo.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceVndHdrListForPaymentSplit[0].ICH_NO, 13);
                            lblDCSplitNo.ToolTip = finInvoiceVndHdrListForPaymentSplit[0].ICH_NO;

                            lblDCSplitDate.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceVndHdrListForPaymentSplit[0].ICH_DATE.ToString(Resources.Constants.DateFormatShort), 13);
                            lblDCSplitDate.ToolTip = finInvoiceVndHdrListForPaymentSplit[0].ICH_DATE.ToString(Resources.Constants.DateFormatShort);

                            lblDCSplitSupplier.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceVndHdrListForPaymentSplit[0].CRM_CUSTOMER_MST.CUS_NAME, 45);
                            lblDCSplitSupplier.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceVndHdrListForPaymentSplit[0].CRM_CUSTOMER_MST.CUS_NAME, 300);

                            lblDCSplitAmount.Text = String.Format("{0:c}", finInvoiceVndHdrListForPaymentSplit[0].ICH_AMOUNT_TC);
                            lblDCSplitAmount.ToolTip = String.Format("{0:c}", finInvoiceVndHdrListForPaymentSplit[0].ICH_AMOUNT_TC);
                            //if (finCrDrCusMpgList != null && finCrDrCusMpgList.Count > 0)
                            //{
                            //    lblDCSplitReceived.Text = lblDCSplitReceived.ToolTip = finCrDrCusMpgList[0].RSO_BOUNCED == 0 ? String.Format("{0:c}", finInvoiceVndHdrListForPaymentSplit[0].CDH_AMOUNT_RCVD_TC - PayNowAmount)
                            //        : String.Format("{0:c}", finInvoiceVndHdrListForPaymentSplit[0].CDH_AMOUNT_RCVD_TC);
                            //}
                            //else
                            //{
                            //    lblDCSplitReceived.Text = lblDCSplitReceived.ToolTip = String.Format("{0:c}", finInvoiceVndHdrListForPaymentSplit[0].CDH_AMOUNT_RCVD_TC - PayNowAmount);
                            //}

                            lblDCSplitReceiveNow.Text = String.Format("{0:c}", PayNowAmount);
                            lblDCSplitReceiveNow.ToolTip = String.Format("{0:c}", PayNowAmount);
                            lbltaxSplitpopup.Text = String.Format("{0:c}", paynowtax);

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
            isLineItemTaxEnabled = false;
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
            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "CUSTOMER");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsSBUCustomer.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
            }


        }

        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
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
                    #region DR CR HDR LIST
                    case ControlsEnum.DRCRHDRLIST:
                        if (finCrDrNoteHdrList != null)
                        {
                            GetFieldValues(ControlsEnum.FILLWORKFLOWSTATUS);

                            uclPaging.TotalPages = TotalPages;
                            PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdCrDbHdr.DataSource = finCrDrNoteHdrList;
                            grdCrDbHdr.DataBind();

                            uclPaging.Visible = true;
                            uclPaging.BindPager();
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
                                    grdInvoiceList.Columns[16].Visible = false;
                                    grdInvoiceList.Columns[7].Visible = true;
                                }
                                else
                                {
                                    grdInvoiceList.Columns[16].Visible = true;
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
                                    hdfExchangeCurr.Value = finCrDrNoteMpgList[0].FIN_CRDR_NOTE_HDR.CDH_EXCHG_RATE.ToString();
                                    //Exchange rate to Textbox
                                    txtExchangeRate.Text = hdfExchangeCurr.Value.ToString();
                                    InvCategory = finCrDrNoteMpgList[0].FIN_INVOICE_CUS_HDR.ICH_CATEGORY;
                                    if (InvCategory == (int)SalesInvoiceCategory.Advanced)
                                    {
                                        grdInvoiceList.Columns[16].Visible = false;
                                        grdInvoiceList.Columns[7].Visible = true;
                                    }
                                    else
                                    {
                                        grdInvoiceList.Columns[16].Visible = true;
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
                    #region CR DR SPLIT LIST
                    case ControlsEnum.CRDRSPLITLIST:
                        if (FinCrDrCusTrxMpgList != null)
                        {
                            grdDCSplit.DataSource = FinCrDrCusTrxMpgList;
                            grdDCSplit.DataBind();
                        }
                        else if (finCrDrCusMpgList != null)
                        {
                            grdDCSplit.DataSource = finCrDrCusMpgList;
                            grdDCSplit.DataBind();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalSplit", "$(document).ready(function(){CalculateTotalSplit();});", true);
                        break; 
                    #endregion
                    #region TAX POPUP GRID
                    case ControlsEnum.TAXPOPUPGRID://Biju
                        if (this.FinCrDrNoteTaxHeader != null)
                        {
                            FIN_CRDR_NOTE_TAX_HDR[] tempArry = FinCrDrNoteTaxHeader.ToArray();
                            FinCrDrNoteTaxHeaderTemp = tempArry.ToList();
                        }
                        else
                        {
                            var tmp = finCrDrNoteHdrList.FirstOrDefault(x => x.CDH_PK == CurrPK);
                            if (tmp != null)
                            {
                                FinCrDrNoteTaxHeader = tmp.FIN_CRDR_NOTE_TAX_HDR.ToList();
                                FIN_CRDR_NOTE_TAX_HDR[] tempArry = FinCrDrNoteTaxHeader.ToArray();
                                FinCrDrNoteTaxHeaderTemp = tempArry.ToList();
                            }
                            else
                            {
                                FinCrDrNoteTaxHeader = new List<FIN_CRDR_NOTE_TAX_HDR>(); //tmp.FIN_CRDR_NOTE_TAX_HDR.ToList();
                                FIN_CRDR_NOTE_TAX_HDR[] tempArry = FinCrDrNoteTaxHeader.ToArray();
                                FinCrDrNoteTaxHeaderTemp = tempArry.ToList();
                            }
                        }

                        grdTaxDetails.DataSource = FinCrDrNoteTaxHeaderTemp;
                        grdTaxDetails.DataBind();
                        break; 
                    #endregion
                    #region TAX POPUP GRID TEMP
                    case ControlsEnum.TAXPOPUPGRIDTEMP://Biju
                        grdTaxDetails.DataSource = FinCrDrNoteTaxHeaderTemp;
                        grdTaxDetails.DataBind();
                        break; 
                    #endregion
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
                        grdUploads.DataSource = DocAttachList;
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
        /// Method for Dropdownlist binding
        /// </summary>
        private void BindDropDownList(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.SOTYPE:
                        ddlInvoiceType.Items.Clear();
                        if (dtSOData != null)
                        {
                            ddlInvoiceType.DataSource = dtSOData;
                            ddlInvoiceType.DataTextField = "CFG_DATA";
                            ddlInvoiceType.DataValueField = "CFG_VALUE";
                            ddlInvoiceType.DataBind();
                        }
                        ddlInvoiceType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
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
                foreach (GridViewRow grdrow in grdCrDbHdr.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID);
                    // check row selected or not
                    if (rbtn.Checked)
                    {
                        // get pk from the grid and assign to CurrPk
                        FinCrDrNoteTaxHeader = null;
                        FinCrDrNoteTaxHeaderTemp = null;
                        CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCrDrPk")).Value); //Convert.ToInt32(grdCrDbHdr.DataKeys[grdrow.RowIndex].Values[0]);
                        Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                        Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomerName")).Text;
                        Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                        hdfCancelled.Value = "0";
                        HiddenField hdfDelStatus = grdrow.FindControl("hdfDelStatus") as HiddenField;
                        if (hdfDelStatus != null)
                            if (Convert.ToBoolean(hdfDelStatus.Value) == true)
                                hdfCancelled.Value = "1";
                        hdfCrDrNumber.Value = CurrPK.ToString();
                        // Get CrDrDetails
                        GetFieldValues(ControlsEnum.DRCRHDRLISTJOURNAL);
                        if (Mode != ActionsEnum.EDITFORCANCEL)
                        {
                            if (InvoiceType == Convert.ToInt32(SalesInvoiceType.Domestic))
                                FillProcessID(1);
                            else
                                FillProcessID(3);
                        }
                        GetFieldValues(ControlsEnum.DRCRMPGLIST);
                        SetFieldValues(ControlsEnum.SELECTEDSIINVOICES);
                        GetUIValuesFromObject(ControlsEnum.DRCRHEADERENTRY);

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
                            // btnSave.Visible = true;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            //EntryStatus = EntryStatus.VIEWMODE;
                            //btnSave.Visible = false;

                        }
                        ucrWrkf.ViewAction();

                        ////////////////////
                        if (CurrPK > 0 && (CrDrSplitList == null || CrDrSplitList.Count == 0))
                        {
                            GetFieldValues(ControlsEnum.GETCRDRHDRBYPK);
                            if (finCrDrNoteHdrList != null && finCrDrNoteHdrList.Count > 0)
                            {
                                CrDrSplitList = finCrDrNoteHdrList[0].FIN_CRDR_NOTE_DTL.ToList();
                            }
                        }
                        ////////////////////

                        if (InvoiceType == Convert.ToInt32(SalesInvoiceType.Domestic))//If Invoicetype is Domestic then exchangerate is noneditable                   
                        {
                            txtExchangeRate.Enabled = false;
                        }
                        else
                        {
                            txtExchangeRate.Enabled = true;
                        }

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
            txtCustomer.Text = string.Empty;
            hdfCustomerID.Value = string.Empty;
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
                    #region   ITEMSELECTED
                    case ActionsEnum.ITEMSELECTED:
                        GridViewRow selectedGrdrow = (sender as RadioButton).Parent.Parent as GridViewRow;
                        rbtn = sender as RadioButton;
                        bIsChecked = true;
                        HiddenField hdfPaymentID;
                        HiddenField hdfDelStatus;
                        int pk;
                        int delStatus = 0;
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
                        GetFieldValues(ControlsEnum.FILEUPLOAD);
                        SetFieldValues(ControlsEnum.FILEUPLOAD);
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
                            if (CurrPK > 0)
                            {
                                GetFieldValues(ControlsEnum.CHECKCRDRUSEDINOTHERTRNS);
                                if (isDrCrUsedInOtherTrns)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_CrdrUsed").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                            }
                            // if (!)
                            if ((!Isposted() && (Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.DEBIT || Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.DEBIT)) || (Isposted() && Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.CREDIT) || (Isposted() && Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.DEBIT))
                            {
                                if (grdInvoiceList.Rows.Count >= 1)
                                {
                                    if (Convert.ToInt32(ddlMode.SelectedValue) == (int)DrCrModeEnum.CREDIT && InvCategory == (int)SalesInvoiceCategory.Advanced && !IsValidCNAmount())
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_CN_AmountExceeded").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                        return;
                                    }
                                    finCrDrNoteHdrList = new List<FIN_CRDR_NOTE_HDR>();
                                    finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                                    finCrDrHdrNoteServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                                    finCrDrNoteHdrObj = CommonFunctions.Initilize<FIN_CRDR_NOTE_HDR>();
                                    finCrDrNoteHdrObj = (FIN_CRDR_NOTE_HDR)SetUIValuesToObject(ControlsEnum.DRCRHEADERENTRY);
                                    //finCrDrNoteHdrObj.FIN_CRDR_NOTE_TAX_HDR.Clear();

                                    //foreach (FIN_CRDR_NOTE_TAX_HDR tax in this.FinCrDrNoteTaxHeader)
                                    //{
                                    //    finCrDrNoteHdrObj.FIN_CRDR_NOTE_TAX_HDR.Add(tax);
                                    //}
                                    if (finCrDrHdrNoteServiceClient.IsRefnoExist(finCrDrNoteHdrObj))
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_RefnoExist").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                        return;
                                    }
                                    lblTotalPayNowFooter = (Label)grdInvoiceList.FooterRow.FindControl("lblTotalPayNowFooter");
                                    hdfPayNowFooter = (HiddenField)grdInvoiceList.FooterRow.FindControl("hdfPayNowFooter");
                                    //lblTotalPayNowFooter.Text = GetFormattedCurrency(hdfTotalPayNowFooter.Value);
                                    lblTotalPayNowFooter.Text = GetFormattedCurrencyWithComa(hdfTotalPayNowFooter.Value);
                                    hdfPayNowFooter.Value = GetFormattedCurrency(hdfTotalPayNowFooter.Value);
                                    if (finCrDrNoteHdrObj != null)
                                    {
                                        if (finCrDrNoteHdrObj.FIN_CRDR_NOTE_MPG.ToList().Count > 0)
                                        {
                                            finCrDrNoteHdrList.Add(finCrDrNoteHdrObj);
                                            int Archiveresult = 0;
                                            if (finCrDrNoteHdrObj.CDH_PK > 0 && finCrDrNoteHdrObj.CDH_STATUS > 0)
                                            {
                                                //After getting entry into workflow, for each update keep version details of DN/CN for Audit trail 
                                                Archiveresult = BusinessLogic.POInvoicing.POInvoiceBL.SaveDRCR_Note_ArchiveDetails(finCrDrNoteHdrObj.CDH_PK);
                                            }
                                            if ((Archiveresult > 0 && finCrDrNoteHdrObj.CDH_STATUS > 0) || finCrDrNoteHdrObj.CDH_STATUS == 0)
                                            {
                                                #region Transaction Begin
                                                using (TransactionScope scope = new TransactionScope())
                                                {
                                                    try
                                                    {
                                                        result = finCrDrHdrNoteServiceClient.SaveCrDrNoteHdr(finCrDrNoteHdrList, false);
                                                        // int i= call to document save
                                                        #region ATTACHMENT SAVE
                                                        if (result > 0)
                                                        {
                                                            if (DocAttachList != null && DocAttachList.Count > 0)
                                                            {
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
                                                                        BusinessObject.POInvoicing.FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.DOC_SEQ_NO);
                                                                        if (fileDetailsObj != null)
                                                                        {
                                                                            fileDetailsObj.PoFile.SaveAs(attachedFileInfo.FullName);
                                                                        }
                                                                    }
                                                                }
                                                                POInvoiceService poInvoiceServiceClient = new POInvoiceService();
                                                                poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                                                                long? r = poInvoiceServiceClient.SaveDocAttachemts(DocAttachList, Convert.ToInt32(result.Value));
                                                                poInvoiceServiceClient = null;
                                                            }
                                                            else
                                                            {
                                                                POInvoiceService poInvoiceServiceClient = new POInvoiceService();
                                                                poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                                                                int r = poInvoiceServiceClient.AttachDocumentDelete(0, (int)DocTaskEnum.DEBITCREDITNOTE, Convert.ToInt32(result.Value));
                                                                if (r < 1 && r != (int)DbSaveStatus.ALREADYDELETED)
                                                                {
                                                                    throw new Exception(GetLocalResourceObject("Err_SaveAttachmentDocument").ToString());
                                                                }
                                                                poInvoiceServiceClient = null;
                                                            }
                                                        }
                                                        #endregion

                                                        //Commit Transaction
                                                        scope.Complete();
                                                        //IsSuccess = true;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        //// IsSuccess = false;
                                                        //// Handler for unknown exceptions
                                                        //// Throws a new exception to client with class name - method name - server side exception process result as exception message
                                                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
                                                        result = (int)DbSaveStatus.SQLERROR;
                                                    }
                                                    finally
                                                    {
                                                        //Disposing used objects
                                                        scope.Dispose();
                                                    }
                                                }

                                                #endregion

                                                if (result > 0)
                                                {
                                                    #region Generate dummy entry
                                                    if (CurrPK > 0)
                                                    {
                                                        GetFieldValues(ControlsEnum.GETCRDRHDRBYPK);
                                                        if (finCrDrNoteHdrList != null && finCrDrNoteHdrList.Count > 0)
                                                        {
                                                            if (finCrDrNoteHdrList[0].CDH_STATUS == (byte)DbStatus.APPROVED)
                                                            {
                                                                FinTrxService finTrxServiceClient;
                                                                finTrxServiceClient = new FinTrxService();
                                                                string refType = "";
                                                                if (finCrDrNoteHdrList[0].CDH_TYPE == (byte)DebitCreditModeEnum.DEBIT)
                                                                {
                                                                    refType = ApplicationType.DNJ;

                                                                }
                                                                else
                                                                {
                                                                    refType = ApplicationType.CNJ;

                                                                }
                                                                long DummyResult = 0;
                                                                bool IsDummyEntry = finTrxServiceClient.IsDummyEntry(refType, (int)finCrDrNoteHdrList[0].CDH_PK, 0);
                                                                if (IsDummyEntry == true)
                                                                {
                                                                    DummyResult = finTrxServiceClient.DeleteFinTrx(refType, (int)finCrDrNoteHdrList[0].CDH_PK, 0);
                                                                }
                                                                else
                                                                {
                                                                    DummyResult = (int)finCrDrNoteHdrList[0].CDH_PK;
                                                                }

                                                                if (DummyResult > 0)
                                                                {
                                                                    finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                                                    DummyResult = finTrxServiceClient.GenerateDummyEntry((int)CurrPK, refType);
                                                                }

                                                            }
                                                        }
                                                    }
                                                    #endregion

                                                    SelectedInvoicesCrDr = null;
                                                    litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.CreditDebitNotes);
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg",
                                                        "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                    EntryStatus = EntryStatus.LISTMODE;
                                                    ResetForm();
                                                    SelectedInvoicesCrDr = null;
                                                    GetFieldValues(ControlsEnum.DRCRHDRLIST);
                                                    SetFieldValues(ControlsEnum.DRCRHDRLIST);
                                                    btnNew.Focus();
                                                    GetFieldValues(ControlsEnum.FILEUPLOAD);
                                                    SetFieldValues(ControlsEnum.FILEUPLOAD);
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
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_InvNotjournalized").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region Transaction Number Generation
                    case ActionsEnum.DRCRMODE:
                        if (EntryStatus == EntryStatus.NEWMODE)
                        {
                            switch (String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue))
                            {
                                case (int)DrCrModeEnum.CREDIT:
                                    GetFieldValues(ControlsEnum.CREDIT);
                                    lblDrCrNo.ToolTip = lblDrCrNo.Text = transactionNumber;
                                    break;
                                case (int)DrCrModeEnum.DEBIT:
                                    GetFieldValues(ControlsEnum.DEBIT);
                                    lblDrCrNo.ToolTip = lblDrCrNo.Text = transactionNumber;
                                    break;
                            }
                        }
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        //hdfCrDrNumber.Value = string.Empty;
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
                    #region Edit
                    case ActionsEnum.EDIT:
                        FillProcessID(1);
                        SetUIEditView(commonActions);
                        ModifiedDatePnl.Visible = true;
                        EnableDisableDrCrMode();
                        ConfigurationSettings();
                        GetFieldValues(ControlsEnum.TAXMYR);
                        SetFieldValues(ControlsEnum.TAXMYR);
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
                                HiddenField hdfCrDrType = (HiddenField)grdrow.FindControl("hdfCrDrType");
                                // check row selected or not
                                if (rbtn.Checked)
                                {
                                    // get pk from the grid and assign to CurrPk
                                    CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCrDrPk")).Value);//Convert.ToInt32(grdCrDbHdr.DataKeys[grdrow.RowIndex].Values[0]);
                                    bIsChecked = true;
                                    hdfCrDrNumber.Value = CurrPK.ToString();
                                    Reftype = hdfCreditDebitType.Value == "1" ? ApplicationType.DN : ApplicationType.CN;
                                    InvType = Convert.ToInt32(hdfCrDrType.Value);
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
                                Reftype = finCrDrNoteHdrList[0].CDH_TYPE.ToString() == "1" ? ApplicationType.DN : ApplicationType.CN;
                            }
                            else
                            {
                                bIsChecked = false;
                            }
                        }
                        if (bIsChecked)
                        {
                            if (InvType == 2|| InvType == 4)
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
                    #region View
                    case ActionsEnum.VIEW:
                        SetUIEditView(commonActions);
                        btnCancel.Focus();
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
                    #region Remove
                    case ActionsEnum.REMOVE:
                        int INVPk = int.Parse(((Button)sender).CommandArgument.ToString());
                        //if (CurrPK == 0)
                        {
                            SelectedInvoicesCrDr.Remove(INVPk);
                            selectedInvoiceList = SelectedInvoicesCrDr;
                            RemovedInvoicesCrDr = true;
                        }
                        if (SelectedInvoicesCrDr != null)
                        {
                            if (FinInvoiceCusHdrSelectedList != null && FinInvoiceCusHdrSelectedList.Count > 0)
                            {
                                finInvoiceCusHdrList = FinInvoiceCusHdrSelectedList;
                                finInvoiceCusHdrObj = CommonFunctions.Initilize<ERPData.FIN_INVOICE_CUS_HDR>();
                                finInvoiceCusHdrObj = finInvoiceCusHdrList.SingleOrDefault(ivh => ivh.ICH_PK == INVPk);
                                if (finInvoiceCusHdrObj != null)
                                {
                                    finInvoiceCusHdrList.Remove(finInvoiceCusHdrObj);
                                    FinInvoiceCusHdrSelectedList = finInvoiceCusHdrList;
                                    SetFieldValues(ControlsEnum.SELECTEDSIINVOICES);
                                }
                            }
                            else if (FinInvoiceCrDrSelectedList != null && FinInvoiceCrDrSelectedList.Count > 0)
                            {
                                finCrDrNoteMpgList = FinInvoiceCrDrSelectedList;
                                finCrDrNoteMpgObj = CommonFunctions.Initilize<ERPData.FIN_CRDR_NOTE_MPG>();
                                finCrDrNoteMpgObj = finCrDrNoteMpgList.SingleOrDefault(crdr => crdr.CDM_INVOICE_CUS_HDR == INVPk);
                                if (finCrDrNoteMpgObj != null)
                                {
                                    finCrDrNoteMpgList.Remove(finCrDrNoteMpgObj);
                                    SetFieldValues(ControlsEnum.SELECTEDSIINVOICES);
                                }
                            }
                        }
                        else if (EditedPaymentDtls != null)
                        {
                            finPaymentVndTrxMpgList = EditedPaymentDtls;
                        }

                        break;
                    #endregion
                    #region Tab navigation
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
                    case ActionsEnum.DEFAULT:
                        CheckUserRightsAndRedirect(Resources.PageURL.SoListing);
                        //Response.Redirect(Resources.PageURL.SoListing);
                        break;
                    case ActionsEnum.SALESINVOICE:
                        CheckUserRightsAndRedirect(Resources.PageURL.SalesInvoicing);
                        //Response.Redirect(Resources.PageURL.SalesInvoicing);
                        break;
                    case ActionsEnum.INVOICE:
                        //Response.Redirect(Resources.PageURL.Invoicing);
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
                        foreach (GridViewRow grdrow in grdCrDbHdr.Rows)
                        {

                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomerName")).Text;
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
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup
                        if ((!Isposted() && (Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.DEBIT || Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.CREDIT)) || (Isposted() && Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.CREDIT) || (Isposted() && Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.DEBIT))
                        {
                            if (grdInvoiceList.Rows.Count >= 1)
                            {
                                if (Convert.ToInt32(ddlMode.SelectedValue) == (int)DrCrModeEnum.CREDIT && InvCategory == (int)SalesInvoiceCategory.Advanced && !IsValidCNAmount())
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
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_InvNotjournalized").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        if ((!Isposted() && (Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.DEBIT || Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.CREDIT)) || (Isposted() && Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.CREDIT) || (Isposted() && Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.DEBIT))
                        {  //Show WorkFlow Popup                       
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
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_InvNotjournalized").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
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
                                    finCrDrNoteHdrList = new List<FIN_CRDR_NOTE_HDR>();
                                    finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                                    finCrDrHdrNoteServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                                    finCrDrNoteHdrObj = CommonFunctions.Initilize<FIN_CRDR_NOTE_HDR>();
                                    finCrDrNoteHdrObj = (FIN_CRDR_NOTE_HDR)SetUIValuesToObject(ControlsEnum.WRKFSUBMIT);
                                    if (finCrDrHdrNoteServiceClient.IsRefnoExist(finCrDrNoteHdrObj))
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_RefnoExist").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                        return;
                                    }
                                    lblTotalPayNowFooter = (Label)grdInvoiceList.FooterRow.FindControl("lblTotalPayNowFooter");
                                    hdfPayNowFooter = (HiddenField)grdInvoiceList.FooterRow.FindControl("hdfPayNowFooter");
                                    //lblTotalPayNowFooter.Text = GetFormattedCurrency(hdfTotalPayNowFooter.Value);
                                    lblTotalPayNowFooter.Text = GetFormattedCurrencyWithComa(hdfTotalPayNowFooter.Value);
                                    hdfPayNowFooter.Value = GetFormattedCurrency(hdfTotalPayNowFooter.Value);
                                    if (hdfExchangeCurr.Value != "-1")
                                    {
                                        if (finCrDrNoteHdrObj != null)
                                        {
                                            if (finCrDrNoteHdrObj.FIN_CRDR_NOTE_MPG.ToList().Count > 0)
                                            {
                                                finCrDrNoteHdrList.Add(finCrDrNoteHdrObj);
                                                int Archiveresult = 0;
                                                if (finCrDrNoteHdrObj.CDH_PK > 0 && finCrDrNoteHdrObj.CDH_STATUS > 0)
                                                {
                                                    //After getting entry into workflow, for each update keep version details of DN/CN for Audit trail 
                                                    Archiveresult = BusinessLogic.POInvoicing.POInvoiceBL.SaveDRCR_Note_ArchiveDetails(finCrDrNoteHdrObj.CDH_PK);
                                                }
                                                if ((Archiveresult > 0 && finCrDrNoteHdrObj.CDH_STATUS > 0) || finCrDrNoteHdrObj.CDH_STATUS == 0)
                                                {
                                                    #region Transaction Begin
                                                    using (TransactionScope scope = new TransactionScope())
                                                    {
                                                        result = 0;
                                                        try
                                                        {
                                                            result = finCrDrHdrNoteServiceClient.SaveCrDrNoteHdr(finCrDrNoteHdrList, true);
                                                            // int i= call to document save
                                                            #region ATTACHMENT SAVE
                                                            if (result > 0)
                                                            {
                                                                if (DocAttachList != null && DocAttachList.Count > 0)
                                                                {
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
                                                                            BusinessObject.POInvoicing.FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.DOC_SEQ_NO);
                                                                            if (fileDetailsObj != null)
                                                                            {
                                                                                fileDetailsObj.PoFile.SaveAs(attachedFileInfo.FullName);
                                                                            }
                                                                        }
                                                                    }
                                                                    POInvoiceService poInvoiceServiceClient = new POInvoiceService();
                                                                    poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                                                                    long? r = poInvoiceServiceClient.SaveDocAttachemts(DocAttachList, Convert.ToInt32(result.Value));
                                                                    poInvoiceServiceClient = null;
                                                                }
                                                                else
                                                                {
                                                                    POInvoiceService poInvoiceServiceClient = new POInvoiceService();
                                                                    poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                                                                    int r = poInvoiceServiceClient.AttachDocumentDelete(0, (int)DocTaskEnum.DEBITCREDITNOTE, Convert.ToInt32(result.Value));
                                                                    if (r < 1 && r != (int)DbSaveStatus.ALREADYDELETED)
                                                                    {
                                                                        throw new Exception(GetLocalResourceObject("Err_SaveAttachmentDocument").ToString());
                                                                    }
                                                                    poInvoiceServiceClient = null;
                                                                }
                                                            }
                                                            #endregion

                                                            //Commit Transaction
                                                            scope.Complete();
                                                            //IsSuccess = true;                                                           
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            //// IsSuccess = false;
                                                            //// Handler for unknown exceptions
                                                            //// Throws a new exception to client with class name - method name - server side exception process result as exception message
                                                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
                                                            if (result > 0) result = (int)DbSaveStatus.SQLERROR;
                                                        }
                                                        finally
                                                        {
                                                            //Disposing used objects
                                                            scope.Dispose();
                                                        }
                                                    }

                                                    #endregion

                                                    if (result > 0)// Save Success ! do WorkFlow
                                                    {
                                                        //Workflow submission
                                                        ucrWrkf.ApplicationID = (int)result;
                                                        //ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                                        ////Do WorkFlow if WorkFlow has Actions
                                                        //if (ddlWkfAction.Items.Count > 0)
                                                        //{
                                                        //    action = ddlWkfAction.SelectedItem.ToString();
                                                        //    result = ucrWrkf.DoWorkFlow();
                                                        //    if (result > 0)
                                                        //    {
                                                        //        //Show Save success message and reset Contract Entry
                                                        //        litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                                                        //        object[] args = new object[2];
                                                        //        args[0] = Resources.PageNameRes.CreditDebitNotes;
                                                        //        args[1] = lblDrCrNo.Text;
                                                        //        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                                                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                        //            + "','" + Resources.ErpRes.Information + "');", true);

                                                        //        EntryStatus = EntryStatus.LISTMODE;
                                                        //        ResetForm();
                                                        //        SelectedInvoicesCrDr = null;
                                                        //        GetFieldValues(ControlsEnum.DRCRHDRLIST);
                                                        //        SetFieldValues(ControlsEnum.DRCRHDRLIST);
                                                        //        btnNew.Focus();
                                                        //    }

                                                        //}
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
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);

                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_Amount").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);

                                        litErrorMsg.Text = Resources.Messages.Msg_ErrConversionFactor;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_InvoiceCount").ToString()) + "');", true);
                                }
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                            //|| (Request.QueryString[QueryStrings.PageType] != null &&
                            //Request.QueryString[QueryStrings.PageType] == SCWorkFlowType.Cancel))
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(Convert.ToInt32(CurrPK), ApplicationType.CN))
                                {
                                    ucrWrkf.ApplicationID = Convert.ToInt32(CurrPK);
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
                                ucrWrkf.ApplicationID = Convert.ToInt32(CurrPK);
                            if (ucrWrkf.ApplicationID > 0)
                            {
                                ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                //Do WorkFlow if WorkFlow has Actions
                                if (ddlWkfAction.Items.Count > 0)
                                {
                                    action = ddlWkfAction.SelectedItem.ToString();
                                    //int actionValue = Convert.ToInt32(ddlWkfAction.SelectedValue);
                                    result = ucrWrkf.DoWorkFlow();
                                    if (result > 0)
                                    {
                                        ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();
                                        //Show Save success message and reset Contract Entry
                                        if (ddlWkfAction.SelectedValue == "2727")//checking for action cancel
                                        {
                                            FillProcessID(1);
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Cancel_Success").ToString();
                                            AdmTrxLogDet.ATL_ACTION = (byte)LogAction.DELETE;
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                                            AdmTrxLogDet.ATL_ACTION = (byte)LogAction.SUBMIT;
                                        }
                                        object[] args = new object[2];
                                        args[0] = Resources.PageNameRes.CreditDebitNotes;
                                        args[1] = lblDrCrNo.Text;

                                        #region LOG SAVE
                                        CommonServiceClient = new CommonService();
                                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                                        //finCrDrNoteHdrObj = CommonFunctions.Initilize<FIN_CRDR_NOTE_HDR>();
                                        //finCrDrNoteHdrObj = (FIN_CRDR_NOTE_HDR)SetUIValuesToObject(ControlsEnum.WRKFSUBMIT);
                                        //if (finCrDrNoteHdrObj != null)
                                        //{
                                        List<ADM_APP_TRX_LOG> AdmTrxLogList = new List<ADM_APP_TRX_LOG>();
                                        AdmTrxLogDet.ATL_APP_TRX_CODE = lblDrCrNo.Text;
                                        if (Convert.ToInt32(ddlMode.SelectedValue) == (int)DebitCreditModeEnum.DEBIT)
                                        {
                                            AdmTrxLogDet.ATL_APP_TYPE = ApplicationType.DN;
                                        }
                                        else if (Convert.ToInt32(ddlMode.SelectedValue) == (int)DebitCreditModeEnum.CREDIT)
                                        {
                                            AdmTrxLogDet.ATL_APP_TYPE = ApplicationType.CN;
                                        }
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

                                        if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                        {
                                            ResetForm();
                                            SelectedInvoicesCrDr = null;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                            ResetForm();
                                            SelectedInvoicesCrDr = null;
                                            GetFieldValues(ControlsEnum.DRCRHDRLIST);
                                            SetFieldValues(ControlsEnum.DRCRHDRLIST);
                                            btnNew.Focus();
                                        }
                                    }

                                }
                            }
                        }
                        break;
                    #endregion
                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
                        finCrDrNoteHdrObj = CommonFunctions.Initilize<FIN_CRDR_NOTE_HDR>();
                        finCrDrNoteHdrObj.CDH_PK = CurrPK;
                        GetFieldValues(ControlsEnum.GETCRDRHDRBYPK);
                        SetUIValuesToObject(ControlsEnum.JOURNALIZE);
                        break;
                    #endregion
                    #region Journalize Update
                    case ActionsEnum.JOURNALIZEUPDATE:
                        ucrJournalize.ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        hdfCrDrNumber.Value = string.Empty;
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
                        hdfCrDrNumber.Value = string.Empty;
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
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                            if (Transaction == "SAVE")
                            {
                                finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                                finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                                result = finCrDrHdrNoteServiceClient.UpdateCrDrHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);

                            }
                        }
                        hdfCrDrNumber.Value = string.Empty;
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
                        GetFieldValues(ControlsEnum.DRCRHDRLIST);
                        SetFieldValues(ControlsEnum.DRCRHDRLIST);
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
                                finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                                finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                                result = finCrDrHdrNoteServiceClient.UpdateCrDrHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                            }
                        }
                        hdfCrDrNumber.Value = string.Empty;
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
                        hdfCrDrNumber.Value = string.Empty;
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
                    #region Credit Debit Notes
                    case ActionsEnum.CREDITDEBITLIST:
                        SelectedInvoicesCrDr = null;
                        selectedInvoiceList = null;
                        ResetForm();
                        hdfCrDrNumber.Value = string.Empty;
                        GetFieldValues(ControlsEnum.DRCRHDRLIST);
                        SetFieldValues(ControlsEnum.DRCRHDRLIST);
                        uclPaging.TotalPages = TotalPages;
                        uclPaging.CurrentPage = 1;
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    case ActionsEnum.CREDITDEBITDETAIL:
                        /* commend by juno */
                        //if (SelectedInvoicesCrDr != null && SelectedInvoicesCrDr.Count > 0)
                        //{
                        //    selectedInvoiceList = SelectedInvoicesCrDr;
                        //    GetFieldValues(ControlsEnum.SELECTEDSIINVOICES);
                        //    SetFieldValues(ControlsEnum.SELECTEDSIINVOICES);
                        //    EntryStatus = EntryStatus.NEWMODE;
                        //    lblDrCrNo.Text = "[NEW]";
                        //    txtDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                        //    SelectedInvoicesCrDr = null;
                        //    selectedInvoiceList = null;
                        //}
                        //else
                        //{
                        //    if (RemovedInvoicesCrDr == false)
                        //    {
                        //        SetUIEditView(commonActions);
                        //        ModifiedDatePnl.Visible = true;
                        //        RemovedInvoicesCrDr = false;
                        //    }
                        //}
                        /* end*/
                        //FillProcessID(1);
                        SetUIEditView(commonActions);
                        ModifiedDatePnl.Visible = true;
                        EnableDisableDrCrMode();
                        ConfigurationSettings();
                        GetFieldValues(ControlsEnum.TAXMYR);
                        SetFieldValues(ControlsEnum.TAXMYR);
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (grdInvoiceList.Rows.Count >= 1)
                        {
                            finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                            finCrDrHdrNoteServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                            result = finCrDrHdrNoteServiceClient.DeleteCrDrNoteHdr(CurrPK, int.Parse(ddlMode.SelectedValue));
                            if (result > 0)
                            {
                                litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.CreditDebitNotes);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm();
                                SelectedInvoicesCrDr = null;
                                GetFieldValues(ControlsEnum.DRCRHDRLIST);
                                SetFieldValues(ControlsEnum.DRCRHDRLIST);
                                btnNew.Focus();
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Delete_InvoiceCount").ToString()) + "');", true);
                        }
                        break;
                    #endregion
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
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
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                ////
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
                            //ModifiedDatePnl.Visible = true;
                            //GetFieldValues(ControlsEnum.INVOICETYPE);
                            //SetFieldValues(ControlsEnum.INVOICETYPE);
                            //GetFieldValues(ControlsEnum.SOINVHEADER);
                            //SetFieldValues(ControlsEnum.SOINVHEADER);                            
                            //SetFieldValues(ControlsEnum.SOINVDETAIL);
                            //TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                            //SetDetailTax(null);
                            //SetHdrTax();
                            //if (grdInvoice.Rows.Count > 0)
                            //{
                            //    SetSubTotal();
                            //}

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
                    #region Biju
                    #region TAXADD
                    case ActionsEnum.TAXADD:
                        bool errorTaxNameAlreadyExist = false;
                        bool errorTaxName = false;
                        bool errorTaxAmount = false;
                        string taxName = txtPopupOther.Text.Trim();
                        decimal taxAmount = Convert.ToDecimal(txtPopupAmount.Text);

                        if (!Decimal.TryParse(txtPopupAmount.Text, out taxAmount)) errorTaxAmount = true;
                        if (String.IsNullOrWhiteSpace(taxName)) errorTaxName = true;

                        foreach (FIN_CRDR_NOTE_TAX_HDR tax in this.FinCrDrNoteTaxHeaderTemp)
                        {
                            if (tax.NTH_NAME.ToLower() == taxName.ToLower())
                            {
                                errorTaxNameAlreadyExist = true;
                            }
                        }

                        if (!errorTaxName && !errorTaxAmount && !errorTaxNameAlreadyExist)
                        {
                            byte taxCategory = Convert.ToByte(hdfTaxCategory.Value);
                            this.FinCrDrNoteTaxHeaderTemp.Add(new FIN_CRDR_NOTE_TAX_HDR
                                          {
                                              NTH_PK = 0,
                                              NTH_CRDR_NOTE_HDR = CurrPK,
                                              NTH_TAX_CATEGORY = taxCategory,
                                              NTH_NAME = taxName,
                                              NTH_TAX_AMT = taxAmount
                                          });

                            SetFieldValues(ControlsEnum.TAXPOPUPGRIDTEMP);
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
                        if (this.FinCrDrNoteTaxHeaderTemp != null)
                        {
                            //invoiceHeaderObj = TempSOInvoiceHeaderSession;
                            HiddenField hdfTaxPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxPK") as HiddenField);
                            HiddenField hdfTaxName = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxName") as HiddenField);
                            if (hdfTaxPK != null)
                            {
                                int taxPK = string.IsNullOrWhiteSpace(hdfTaxPK.Value) ? 0 : Convert.ToInt32(hdfTaxPK.Value);
                                string txName = string.IsNullOrWhiteSpace(hdfTaxName.Value) ? String.Empty : hdfTaxName.Value;
                                this.FinCrDrNoteTaxHeaderTemp.Remove(this.FinCrDrNoteTaxHeaderTemp.SingleOrDefault(x => x.NTH_PK == Convert.ToInt32(taxPK) && x.NTH_NAME == txName));
                                SetFieldValues(ControlsEnum.TAXPOPUPGRIDTEMP);

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
                        //if (SOInvoiceHeaderSession != null)
                        //{
                        //TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                        //IsHeaderTax = true;
                        GetFieldValues(ControlsEnum.TAXPOPUPGRID);
                        SetFieldValues(ControlsEnum.TAXPOPUPGRID);

                        GetFieldValues(ControlsEnum.TAXTYPES);
                        SetFieldValues(ControlsEnum.TAXTYPES);

                        //Label txtSubTotal = (Label)grdInvoiceList.FooterRow.FindControl("lblTotalPayNowFooter");
                        decimal subtotal = 0;
                        decimal.TryParse(hdfTotalPayNowFooter.Value, out subtotal);
                        ////if (txtSubTotal != null)
                        ////{
                        ////txtPopupItemAmount.Text = string.IsNullOrEmpty(txtSubTotal.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtSubTotal.Text).ToString(hdfCurrencyFormat.Value);
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
                        ////}
                        //}
                        break;
                    #endregion
                    #endregion                   
                    #region CRDR Split
                    case ActionsEnum.DCDETAIL:
                        divErrorLabel.Visible = false;
                        HiddenField hdfReceiptMpgPK = (HiddenField)((((Button)sender).Parent).FindControl("hdfCrDbMpgPK"));
                        TextBox txtReceivedNow = (TextBox)((((Button)sender).Parent).FindControl("txtNoteFor"));
                        //Label lblTotalTax = (Label)((((Button)sender).Parent).FindControl("lblTotalTax"));
                        LinkButton lbnTotalTax = (LinkButton)((((Button)sender).Parent).FindControl("lbnTotalTax"));
                        HiddenField hdfTotalTax = (HiddenField)((((Button)sender).Parent).FindControl("hdfTotalTax"));

                        InvRowIndex = ((GridViewRow)((Button)(sender)).Parent.Parent).RowIndex;

                        //if (CurrPK > 0 && (CrDrSplitList == null || CrDrSplitList.Count == 0))
                        //{
                        //    GetFieldValues(ControlsEnum.GETCRDRHDRBYPK);
                        //    if (finCrDrNoteHdrList != null && finCrDrNoteHdrList.Count > 0)
                        //    {
                        //        CrDrSplitList = finCrDrNoteHdrList[0].FIN_CRDR_NOTE_DTL.ToList();
                        //    }
                        //}

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
                        tempInvoiceSOSplitList = CrDrSplitList;
                        tempfinCrDrCusMpgList = tempInvoiceSOSplitList.Where(dtl => dtl.FIN_CRDR_NOTE_MPG.CDM_INVOICE_CUS_HDR == InvoicePK).ToList();



                        if (hdfReceiptMpgPK != null && !string.IsNullOrEmpty(hdfReceiptMpgPK.Value) && !hdfReceiptMpgPK.Value.Equals("0"))
                        {
                            CrDrMpgPK = Convert.ToInt64(hdfReceiptMpgPK.Value);
                            GetFieldValues(ControlsEnum.CRDRSPLITLIST);

                            if (tempfinCrDrCusMpgList == null || tempfinCrDrCusMpgList.Count == 0)
                            {
                                if (finCrDrCusMpgList != null && finCrDrCusMpgList.Count > 0)//Edit
                                {
                                    //tempInvoiceSOSplitList.Where(dtl => dtl.FIN_CRDR_NOTE_MPG.CDM_INVOICE_CUS_HDR == InvoicePK)
                                    //                .ToList().ForEach(dtl => tempInvoiceSOSplitList.Remove(dtl));
                                    //finCrDrCusMpgList.ForEach(dtl =>
                                    //{
                                    //    dtl.FIN_CRDR_NOTE_MPG = new FIN_CRDR_NOTE_MPG()
                                    //    {
                                    //        CDM_INVOICE_CUS_HDR = InvoicePK
                                    //    };
                                    //    dtl.CDS_QTY = 0;
                                    //    tempInvoiceSOSplitList.Add(dtl);
                                    //});
                                    //CrDrSplitList = tempInvoiceSOSplitList;
                                }
                                else if (InvoicePK > 0)
                                {
                                    GetFieldValues(ControlsEnum.CRDRVNDMPGLIST);
                                }


                            }
                            else
                            {
                                tempfinCrDrCusMpgList = tempInvoiceSOSplitList;
                                finCrDrCusMpgList = tempfinCrDrCusMpgList.Where(dtl => dtl.FIN_CRDR_NOTE_MPG.CDM_INVOICE_CUS_HDR == InvoicePK).ToList();
                            }
                            if (InvoicePK > 0)
                            {
                                GetFieldValues(ControlsEnum.CRDRVNDHDR);
                                GetUIValuesFromObject(ControlsEnum.CRDRSPLITLIST);
                                isSplitChanged = false;
                                GetFieldValues(ControlsEnum.INVITEMLIST);
                                SetFieldValues(ControlsEnum.CRDRSPLITLIST);
                            }
                        }
                        else if (InvoicePK > 0)//New
                        {
                            GetFieldValues(ControlsEnum.CRDRVNDMPGLIST);
                            GetFieldValues(ControlsEnum.CRDRVNDHDR);
                            GetUIValuesFromObject(ControlsEnum.CRDRSPLITLIST);
                            isSplitChanged = false;
                            GetFieldValues(ControlsEnum.INVITEMLIST);
                            SetFieldValues(ControlsEnum.CRDRSPLITLIST);
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCrDrSplitUp]','" + GetLocalResourceObject("ReceiptSplit").ToString() + "','950','300');", true);
                        break;
                    #endregion
                    #region DCSPLITSAVE
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
                                    lbnTotalPayNowTax.Text = GetFormattedCurrencyWithComa(TotalPayNowTax);
                                    hdfTotalPayNowTax.Value = hdfTotalTaxFooterSplit.Value;
                                }
                            }

                            finCrDrCusMpgList = new List<FIN_CRDR_NOTE_DTL>();
                            finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                            finCrDrHdrNoteServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                            finCrDrCusMpgList = (List<FIN_CRDR_NOTE_DTL>)SetUIValuesToObject(ControlsEnum.CRDRSPLITLIST);
                            if (finCrDrCusMpgList != null && finCrDrCusMpgList.Count() > 0 && finCrDrCusMpgList.Sum(dtl => dtl.CDS_AMOUNT) > 0)
                            {
                                hdfItemIncluded.Value = "1";
                            }
                            else
                            {
                                hdfItemIncluded.Value = "0";
                            }

                        }
                        InvRowIndex = RowIndex = -1;

                        DcsSplitSave();
                        SetSplitCount();
                        GetFieldValues(ControlsEnum.TAXMYR);
                        SetFieldValues(ControlsEnum.TAXMYR);
                        if (Session["event_controle"] != null)
                        {
                            Button controle = (Button)Session["event_controle"];

                            controle.Focus();
                        }

                        break;
                    #endregion
                    #region Split Apply
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

                        //if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                        //{
                        //    hdfInvoicePK = (HiddenField)((((Button)sender).Parent).FindControl("hdfInvoicePK"));
                        //    txtNoteFor = (TextBox)((((Button)sender).Parent).FindControl("txtNoteFor"));
                        //}
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
                                List<FIN_CRDR_NOTE_DTL> finCrDrDtlObj = new List<FIN_CRDR_NOTE_DTL>();
                                tempInvoiceSOSplitList = CrDrSplitList;
                                // finCrDrCusMpgList = (List<FIN_CRDR_NOTE_DTL>)SetUIValuesToObject(ControlsEnum.CRDRSPLITLIST);
                                finCrDrDtlObj = tempInvoiceSOSplitList.Where(ivh => ivh.FIN_CRDR_NOTE_MPG.CDM_INVOICE_CUS_HDR == Convert.ToInt64(hdfInvoicePK.Value)).ToList();



                                if (finCrDrDtlObj != null)
                                {
                                    splitTotal = finCrDrDtlObj.Sum(splt => splt.CDS_AMOUNT);
                                    if (splitTotal != 0) //raiseNote != splitTotal &&
                                    {
                                        if ((hdfIscontYes.Value != "1") && hdfIscontNo.Value == "0")
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlreadyPaid", "$(document).ready(function(){RaiseNoteChanged();});", true);
                                            return;
                                        }
                                        else if (hdfIscontNo.Value == "0")
                                        {
                                            foreach (FIN_CRDR_NOTE_DTL crdrdtl in finCrDrDtlObj)
                                            {
                                                FIN_CRDR_NOTE_DTL finCrdrDtl = new FIN_CRDR_NOTE_DTL();
                                                finCrdrDtl = crdrdtl;
                                                tempInvoiceSOSplitList.Remove(crdrdtl);
                                                finCrdrDtl.CDS_QTY = 0;
                                                finCrdrDtl.CDS_NET_AMOUNT = 0;
                                                finCrdrDtl.CDS_TAX = 0;
                                                finCrdrDtl.CDS_AMOUNT = 0;
                                                tempInvoiceSOSplitList.Add(finCrdrDtl);
                                            }
                                        }
                                    }
                                    if (hdfIscontNo.Value != "0")
                                    {
                                        txtNoteFor.Text = Math.Round(splitTotal, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                    }
                                }
                                if (hdfIscontNo.Value != "0")
                                {
                                    hdfItemIncluded.Value = "1";
                                    hdfSplitCount.Value = "1";
                                }
                                else
                                {
                                    hdfItemIncluded.Value = "0";
                                    hdfSplitCount.Value = "0";
                                }
                                hdfIscontYes.Value = "0";
                                hdfIscontNo.Value = "0";
                                CrDrSplitList = tempInvoiceSOSplitList;
                                SetSplitCount();
                                GetFieldValues(ControlsEnum.TAXMYR);
                                SetFieldValues(ControlsEnum.TAXMYR);
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
                            hdfInvTypeText = (HiddenField)grdrow.FindControl("hdfInvTypeText");
                            HiddenField hdfInvCategory = (HiddenField)grdrow.FindControl("hdfInvCategory");
                            HiddenField hdfInvGroup = (HiddenField)grdrow.FindControl("hdfInvGroup");
                            if (Convert.ToInt32(hdfInvCategory.Value) == (int)SalesInvoiceCategory.Advanced)
                            {
                                if (hdfInvTypeText.Value.ToString().ToLower() == "2" || hdfInvTypeText.Value.ToString().ToLower() == "3"|| hdfInvTypeText.Value.ToString().ToLower() == "4")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                       ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.SIJ + "&APPSUBTYPE=" + Convert.ToInt32(ApplicationSubType.ADVANCEINVOICE) + "&TRXTYPE=" + ApplicationType.SIJ) + "');", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=11") + "');", true);
                                }
                            }
                            else if (Convert.ToInt32(hdfInvGroup.Value) == (int)SalesInvoiceGroup.Miscellaneous)
                            {
                                if (!string.IsNullOrEmpty(hdfInvTypeText.Value))
                                {
                                    if (Convert.ToInt32(hdfInvTypeText.Value) == (int)SalesInvoiceType.Domestic)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.MSI + "&APPSUBTYPE=1") + "');", true);
                                    }
                                    else if (Convert.ToInt32(hdfInvTypeText.Value) == (int)SalesInvoiceType.Export|| Convert.ToInt32(hdfInvTypeText.Value) == (int)SalesInvoiceType.Deemed)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.MSI + "&APPSUBTYPE=2") + "');", true);
                                    }
                                    else if (Convert.ToInt32(hdfInvTypeText.Value) == (int)SalesInvoiceType.Proforma)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.MSI + "&APPSUBTYPE=3") + "');", true);
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
                                            ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=1") + "');", true);
                                    }
                                    else if (hdfInvTypeText.Value.ToString().ToLower() == "2"|| hdfInvTypeText.Value.ToString().ToLower() == "4")
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                            ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=2") + "');", true);
                                    }
                                    else if (hdfInvTypeText.Value.ToString().ToLower() == "3")
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                            ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=3") + "');", true);
                                    }
                                }
                            }
                            return;
                        }
                        break;
                    #endregion
                    #region CHANGEEXCHANGERATE
                    case ActionsEnum.CHANGEEXRATE:
                        hdfExchangeCurr.Value = string.IsNullOrEmpty(txtExchangeRate.Text) ? "1" : txtExchangeRate.Text;
                        GetFieldValues(ControlsEnum.TAXMYR);
                        SetFieldValues(ControlsEnum.TAXMYR);
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
                                        // litErrorMsg.Text = Resources.ErrorMessages.Msg_Valid_File;
                                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        //    + "','" + Resources.ErpRes.Information + "');", true);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Msg_Valid_File) + "','" + Resources.ErpRes.Information + "');", true);
                                        return;
                                    }
                                    admDocAttachObj = DocAttachList.SingleOrDefault(itm => itm.DOC_SEQ_NO == CurrSlNo);
                                    if (admDocAttachObj != null)
                                    {
                                        if (FileDetailsList == null)
                                        {
                                            FileDetailsList = new List<BusinessObject.POInvoicing.FileDetails>();
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
                                            admDocAttachObj.DOC_MODULE = (int)DocModuleEnum.FINANCE;
                                            admDocAttachObj.DOC_TASK = (int)DocTaskEnum.DEBITCREDITNOTE;
                                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                            {
                                                admDocAttachObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                            }
                                            else
                                            {
                                                admDocAttachObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                            }
                                            BusinessObject.POInvoicing.FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == CurrSlNo);
                                            if (fileDetailsObj == null)
                                            {
                                                FileDetailsList.Add(new BusinessObject.POInvoicing.FileDetails() { SlNo = CurrSlNo, PoFile = HttpContext.Current.Request.Files[0] });
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
                                        FileDetailsList = new List<BusinessObject.POInvoicing.FileDetails>();
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
                                    admDocAttachObj.DOC_MODULE = (int)DocModuleEnum.FINANCE;
                                    admDocAttachObj.DOC_TASK = (int)DocTaskEnum.DEBITCREDITNOTE;
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
                                    FileDetailsList.Add(new BusinessObject.POInvoicing.FileDetails() { SlNo = slno, PoFile = HttpContext.Current.Request.Files[0] });
                                    DocAttachList.Add(admDocAttachObj);
                                }
                            }

                            BindGrid(ControlsEnum.FILEUPLOAD);
                            ResetForm(ControlsEnum.ADDITEM);
                            //SetFieldValues(ControlsEnum.POINVOICELIST);
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
                                if (FileDetailsList != null) FileDetailsList = FileDetailsList.Where(row => selectedItemPK != row.SlNo).ToList();
                                //SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                BindGrid(ControlsEnum.FILEUPLOAD);
                                ResetForm(ControlsEnum.ADDITEM);
                            }
                        }
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
                            if (hdfinvType.Value.ToString().ToLower() == "2" || hdfinvType.Value.ToString().ToLower() == "3"|| hdfinvType.Value.ToString().ToLower() == "4")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                   hdfinvPK.Value.ToString() + "&APPTYPE=" + ApplicationType.SIJ + "&APPSUBTYPE=" + Convert.ToInt32(ApplicationSubType.ADVANCEINVOICE) + "&TRXTYPE=" + ApplicationType.SIJ) + "');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                    hdfinvPK.Value.ToString() + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=11") + "');", true);
                            }
                        }
                        else if (Convert.ToInt32(hdfinvGroup.Value) == (int)SalesInvoiceGroup.Miscellaneous)
                        {
                            if (!string.IsNullOrEmpty(hdfinvType.Value))
                            {
                                if (Convert.ToInt32(hdfinvType.Value) == (int)SalesInvoiceType.Domestic)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfinvPK.Value.ToString() + "&APPTYPE=" + ApplicationType.MSI + "&APPSUBTYPE=1") + "');", true);
                                }
                                else if (Convert.ToInt32(hdfinvType.Value) == (int)SalesInvoiceType.Export|| Convert.ToInt32(hdfinvType.Value) == (int)SalesInvoiceType.Deemed)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfinvPK.Value.ToString() + "&APPTYPE=" + ApplicationType.MSI + "&APPSUBTYPE=2") + "');", true);
                                }
                                else if (Convert.ToInt32(hdfinvType.Value) == (int)SalesInvoiceType.Proforma)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfinvPK.Value.ToString() + "&APPTYPE=" + ApplicationType.MSI + "&APPSUBTYPE=3") + "');", true);
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
                                        hdfinvPK.Value.ToString() + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=1") + "');", true);
                                }
                                else if (hdfinvType.Value.ToString().ToLower() == "2"|| hdfinvType.Value.ToString().ToLower() == "4")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        hdfinvPK.Value.ToString() + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=2") + "');", true);
                                }
                                else if (hdfinvType.Value.ToString().ToLower() == "3")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        hdfinvPK.Value.ToString() + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=3") + "');", true);
                                }
                            }
                        }                       
                        break;
                    #endregion
                    #region SHOW INV ITEMS
                    case ActionsEnum.SHOWINVITEMS:
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

        private bool IsValidCNAmount()
        {
            bool isvalid = true;
            if (Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.CREDIT)
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
                if (CrDrSplitList != null && CrDrSplitList.Count() > 0 && CrDrSplitList.Sum(split => split.CDS_AMOUNT) > 0)
                {
                    hdfSplitCount.Value = "1";
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
            //GetFieldValues(ControlsEnum.TAXPOPUPGRID);
            if (this.FinCrDrNoteTaxHeaderTemp != null)
            {
                txtOtherCharge.ToolTip = txtOtherCharge.Text = GetFormattedNumber(this.FinCrDrNoteTaxHeaderTemp.Sum(x => x.NTH_TAX_AMT));
                FinCrDrNoteTaxHeader = this.FinCrDrNoteTaxHeaderTemp;
                this.FinCrDrNoteTaxHeaderTemp = null;
            }
        }


        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            Label lblTotalFooter;
            HiddenField hdfPayNowFooter;
            decimal total;
            decimal taxpercentage;
            decimal basevalue;
            decimal taxamt;
            HiddenField hdfTotalAmt;
            HiddenField hdfTaxAmt;
            HiddenField hdfInvoicePK;
            HiddenField hdfCrDbMpgPK;
            Label lblInvoiceNo;
            LinkButton lnkInvoiceNo;
            HiddenField hdfInvTypeText;
            HiddenField hdfInvCategory;
            HiddenField hdfInvGroup;
            Label lblInvoiceDate;
            Label lblVendorInv;
            Label lblGrossAmount;
            Label lblTax;
            Label lblDiscount;
            Label lblOtherAmount;
            Label lblTotalAmount;
            Label lblPaid;
            Label lblBaltopay;
            TextBox txtNoteFor;
            Button lnkRemove;
            Label lblAdjAmount;
            Label lblTotalTax;
            LinkButton lbnTotalTax;

            SPADM_APP_STATUS_CFG_GET_KV_Result wkfStatus;

            //Split 
            HiddenField hdfInvCusDtlPK;
            Label lblPRODUCTSplit;

            Label lblQTYSplit;
            Label lblRATESplit;

            Label lblAmountSplit;
            Label lblTAXSplit;

            //Label lblTAXSplitTotal;    
            LinkButton lbnTAXSplitTotal;

            Label lblNETSplit;
            TextBox txtPayNowSplit;
            HiddenField hdfTAXSplit;
            HiddenField hdfSumSplit;
            HiddenField hdfReceiptSplitPK;
            HiddenField hdfhasjournalized;
            HiddenField hdfReceiptTRXPK;
            HiddenField hdfTAXSplitTotal;
            HiddenField hdfNETSplit;
            Label lblTotalTaxFooterSplit;
            HiddenField hdfTotalTaxFooterSplit;
            Label lblTotalPayNowFooterSplit;
            Button lnkAllocation;
            HiddenField hdfTotalPayNowFooterSplit;
            TextBox txtQtySplit;
            TextBox txtRateSplit;
            TextBox txtSumSplit;
            HiddenField hdfTotalCNAmount;
            Label lblTotCNAmount;

            TextBox txtAdjustments;
            CustomValidator vcmReceiveNow;

            FIN_CRDR_NOTE_DTL tempFinReceiptCusSoMpgObj = null;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    if (((GridView)sender).ID == "grdInvoiceList")
                    {
                        hdfTotalAmt = e.Row.FindControl("hdfTotalAmt") as HiddenField;
                        hdfTaxAmt = e.Row.FindControl("hdfTaxAmt") as HiddenField;
                        hdfInvoicePK = e.Row.FindControl("hdfInvoicePK") as HiddenField;
                        hdfhasjournalized = e.Row.FindControl("hdfhasjournalized") as HiddenField;
                        hdfCrDbMpgPK = e.Row.FindControl("hdfCrDbMpgPK") as HiddenField;
                        lnkInvoiceNo = e.Row.FindControl("lnkInvoiceNo") as LinkButton;
                        hdfInvTypeText = e.Row.FindControl("hdfInvTypeText") as HiddenField;
                        hdfInvCategory = e.Row.FindControl("hdfInvCategory") as HiddenField;
                        hdfInvGroup = e.Row.FindControl("hdfInvGroup") as HiddenField;
                        //lblInvoiceNo = e.Row.FindControl("lblInvoiceNo") as Label;
                        lblInvoiceDate = e.Row.FindControl("lblInvoiceDate") as Label;
                        lblVendorInv = e.Row.FindControl("lblVendorInv") as Label;
                        lblGrossAmount = e.Row.FindControl("lblGrossAmount") as Label;
                        lblTax = e.Row.FindControl("lblTax") as Label;
                        lblDiscount = e.Row.FindControl("lblDiscount") as Label;
                        lblOtherAmount = e.Row.FindControl("lblOtherAmount") as Label;
                        lblTotalAmount = e.Row.FindControl("lblTotalAmount") as Label;
                        lblPaid = e.Row.FindControl("lblPaid") as Label;
                        lblBaltopay = e.Row.FindControl("lblBaltopay") as Label;
                        txtNoteFor = e.Row.FindControl("txtNoteFor") as TextBox;
                        lnkRemove = e.Row.FindControl("lnkRemove") as Button;
                        lblAdjAmount = e.Row.FindControl("lblAdjAmount") as Label;
                        //lblTotalTax = e.Row.FindControl("lblTotalTax") as Label;
                        lbnTotalTax = e.Row.FindControl("lbnTotalTax") as LinkButton;
                        HiddenField hdfTotalTax = e.Row.FindControl("hdfTotalTax") as HiddenField;
                        HiddenField hdfItemIncluded = e.Row.FindControl("hdfItemIncluded") as HiddenField;
                        Label lblInvoiceAmount = e.Row.FindControl("lblInvoiceAmount") as Label;
                        lnkAllocation = e.Row.FindControl("lnkAllocation") as Button;
                        hdfTotalCNAmount = e.Row.FindControl("hdfTotalCNAmount") as HiddenField;
                        lblTotCNAmount = e.Row.FindControl("lblTotCNAmount") as Label;

                        decimal lineItemTax = 0;
                        decimal lineItemDiscount = 0;
                        decimal TotalCNAmount = 0;
                        decimal RaiseNote = 0;
                        if (finCrDrNoteMpgList != null && finCrDrNoteMpgList.Count > 0)
                        {
                            if (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.FIN_INVOICE_CUS_DTL != null && finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.FIN_INVOICE_CUS_DTL.Count > 0)
                            {
                                lineItemTax = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.FIN_INVOICE_CUS_DTL.Sum(tx => tx.CID_TAX);
                                lineItemDiscount = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.FIN_INVOICE_CUS_DTL.Sum(tx => tx.CID_DISCOUNT);
                            }

                            //hdfInvoiceType.Value = finCrDrNoteMpgList[0].FIN_INVOICE_CUS_HDR.ICH_CATEGORY.ToString();
                            hdfInvoiceType.Value = finCrDrNoteMpgList[0].FIN_INVOICE_CUS_HDR.ICH_TYPE.ToString();
                            hdfPostedJ.Value = finCrDrNoteMpgList[0].FIN_INVOICE_CUS_HDR.ICH_HAS_JRNL_ENTRY.ToString();

                            lblCustomerTxt.Text = ERP.Utilities.CommonFunctions.GetShortString(finCrDrNoteMpgList[0].FIN_INVOICE_CUS_HDR.CRM_CUSTOMER_MST.CUS_NAME, 45);
                            lblCustomerTxt.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(finCrDrNoteMpgList[0].FIN_INVOICE_CUS_HDR.CRM_CUSTOMER_MST.CUS_NAME, 300);
                            hdfCusPK.Value = finCrDrNoteMpgList[0].FIN_INVOICE_CUS_HDR.ICH_CUSTOMER.ToString();
                            txtPaymentCurrency.ToolTip = txtPaymentCurrency.Text = finCrDrNoteMpgList[0].FIN_INVOICE_CUS_HDR.ADM_CURRENCY_MST1.CUR_CODE;
                            hdfPaymentCurrency.Value = finCrDrNoteMpgList[0].FIN_INVOICE_CUS_HDR.ICH_CURRENCY.ToString();



                            //hdfTotalAmt.Value = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_TC -finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DISCOUNT_TC).ToString();
                            ////hdfTotalAmt.Value = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_TAX_TC - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DISCOUNT_TC).ToString();
                            //hdfTotalAmt.Value = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC - (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_TAX_TC + lineItemTax) - (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DISCOUNT_TC + lineItemDiscount)).ToString();
                            if (finCrDrNoteMpgList[0].FIN_INVOICE_CUS_HDR.ICH_TYPE == Convert.ToInt32(SalesInvoiceType.Domestic) && IsAdvInvHasTax)
                            {
                                if (IsTaxForOtherCharge.Value == "1")// No Need for Minus Other charge            
                                {
                                    hdfTotalAmt.Value = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC - (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_TAX_TC + lineItemTax) - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_ADJUST).ToString();//Removed Advance Deduct
                                }
                                else
                                {
                                    hdfTotalAmt.Value = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC - (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_TAX_TC + lineItemTax) - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_SHIP_CHARGE - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_ADJUST).ToString();//Removed Advance Deduct
                                }
                            }
                            else
                            {
                                if (IsTaxForOtherCharge.Value == "1")// No Need for Minus Other charge               
                                {
                                    hdfTotalAmt.Value = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC - (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_TAX_TC + lineItemTax) - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_ADJUST + finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_ADV_DED_TC).ToString();
                                }
                                else
                                {
                                    hdfTotalAmt.Value = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC - (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_TAX_TC + lineItemTax) - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_SHIP_CHARGE - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_ADJUST + finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_ADV_DED_TC).ToString();
                                }
                            }

                            //hdfTaxAmt.Value = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_TAX_TC.ToString() == string.Empty ? "0" : finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_TAX_TC.ToString();
                            hdfInvoicePK.Value = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_PK.ToString();
                            hdfhasjournalized.Value = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_HAS_JRNL_ENTRY.ToString();
                            hdfCrDbMpgPK.Value = finCrDrNoteMpgList[e.Row.RowIndex].CDM_PK.ToString();
                            lnkInvoiceNo.Text = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_NO;
                            lnkInvoiceNo.ToolTip = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_NO;
                            lnkInvoiceNo.CommandArgument = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_PK.ToString();
                            hdfInvTypeText.Value = finCrDrNoteMpgList[0].FIN_INVOICE_CUS_HDR.ICH_TYPE.ToString();
                            hdfInvCategory.Value = finCrDrNoteMpgList[0].FIN_INVOICE_CUS_HDR.ICH_CATEGORY.ToString();
                            hdfInvGroup.Value = finCrDrNoteMpgList[0].FIN_INVOICE_CUS_HDR.ICH_GROUP.ToString();
                            //hdfInvTypeText.Value = finCrDrNoteMpgList[0].FIN_INVOICE_CUS_HDR.ICH_CATEGORY.ToString();
                            //lblInvoiceNo.Text = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_NO;
                            //lblInvoiceNo.ToolTip = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_NO;
                            lblInvoiceDate.Text = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblInvoiceDate.ToolTip = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblVendorInv.Text = ERP.Utilities.CommonFunctions.GetShortString(finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.CRM_CUSTOMER_MST.CUS_NAME, 10);
                            lblVendorInv.ToolTip = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.CRM_CUSTOMER_MST.CUS_NAME;


                            //lblGrossAmount.Text = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_NET_VALUE_TC);
                            //lblGrossAmount.ToolTip = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_NET_VALUE_TC);
                            //lblGrossAmount.Text = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_TC);
                            //lblGrossAmount.ToolTip = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_TC);
                            lblGrossAmount.Text = String.Format("{0:c}", Convert.ToDecimal(hdfTotalAmt.Value));
                            lblGrossAmount.ToolTip = String.Format("{0:c}", Convert.ToDecimal(hdfTotalAmt.Value));

                            lblInvoiceAmount.Text = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC);
                            lblInvoiceAmount.ToolTip = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC);


                            hdfTaxAmt.Value = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_TAX_TC + lineItemTax).ToString();
                            lblTax.Text = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_TAX_TC + lineItemTax);
                            lblTax.ToolTip = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_TAX_TC + lineItemTax);

                            lblDiscount.Text = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DISCOUNT_TC + lineItemDiscount);
                            lblDiscount.ToolTip = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DISCOUNT_TC + lineItemDiscount);

                            lblOtherAmount.Text = lblOtherAmount.ToolTip = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_SHIP_CHARGE);

                            lblTotalAmount.Text = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_NET_VALUE_TC);
                            lblTotalAmount.ToolTip = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_NET_VALUE_TC);
                            decimal paid = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_RCVD_TC;
                            lblPaid.Text = String.Format("{0:c}", paid);
                            lblPaid.ToolTip = String.Format("{0:c}", paid);

                            decimal balToPay = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_NET_TC -
                                              finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_RCVD_TC -
                                              finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_CN_TC +
                                              finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_DN_TC;

                            lblBaltopay.Text = String.Format("{0:c}", balToPay);
                            lblBaltopay.ToolTip = String.Format("{0:c}", balToPay);

                            txtNoteFor.Text = Math.Round(finCrDrNoteMpgList[e.Row.RowIndex].CDM_AMOUNT,
                                Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            txtNoteFor.ToolTip = Math.Round(finCrDrNoteMpgList[e.Row.RowIndex].CDM_AMOUNT,
                                Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                            if (finCrDrNoteMpgList[e.Row.RowIndex].FIN_CRDR_NOTE_DTL != null && finCrDrNoteMpgList[e.Row.RowIndex].FIN_CRDR_NOTE_DTL.Sum(dtl => dtl.CDS_AMOUNT) > 0)
                            {
                                hdfItemIncluded.Value = "1";
                                hdfSplitCount.Value = "1";
                                txtNoteFor.Text = Math.Round(finCrDrNoteMpgList[e.Row.RowIndex].CDM_AMOUNT - finCrDrNoteMpgList[e.Row.RowIndex].CDM_TAX_AMOUNT,
                                  Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                txtNoteFor.ToolTip = Math.Round(finCrDrNoteMpgList[e.Row.RowIndex].CDM_AMOUNT - finCrDrNoteMpgList[e.Row.RowIndex].CDM_TAX_AMOUNT,
                                    Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            }

                            lnkRemove.CommandArgument = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_PK.ToString();
                            lblAdjAmount.Text = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_DN_TC - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_CN_TC);
                            lblAdjAmount.ToolTip = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_DN_TC - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_AMOUNT_CN_TC);

                            //lblTotalTax.Text = Math.Round(finCrDrNoteMpgList[e.Row.RowIndex].CDM_TAX_AMOUNT,
                            //    Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //lblTotalTax.ToolTip = lblTotalTax.Text;
                            lbnTotalTax.Text = String.Format("{0:c}", Math.Round(finCrDrNoteMpgList[e.Row.RowIndex].CDM_TAX_AMOUNT,
                               Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                            lbnTotalTax.ToolTip = lbnTotalTax.Text;
                            hdfTotalTax.Value = Math.Round(finCrDrNoteMpgList[e.Row.RowIndex].CDM_TAX_AMOUNT,
                               Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            if (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_CATEGORY == (int)SalesInvoiceCategory.Advanced)
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
                            TotalCNAmount = GetCNTotalAmount(finCrDrNoteMpgList[e.Row.RowIndex].CDM_INVOICE_CUS_HDR.Value);
                            lblTotCNAmount.Text = String.Format("{0:c}", TotalCNAmount);
                            hdfTotalCNAmount.Value = TotalCNAmount.ToString();

                        }
                        else if (finInvoiceCusHdrList != null && finInvoiceCusHdrList.Count > 0)
                        {
                            if (finInvoiceCusHdrList[e.Row.RowIndex].FIN_INVOICE_CUS_DTL != null && finInvoiceCusHdrList[e.Row.RowIndex].FIN_INVOICE_CUS_DTL.Count > 0)
                            {
                                lineItemTax = finInvoiceCusHdrList[e.Row.RowIndex].FIN_INVOICE_CUS_DTL.Sum(tx => tx.CID_TAX);
                                lineItemDiscount = finInvoiceCusHdrList[e.Row.RowIndex].FIN_INVOICE_CUS_DTL.Sum(tx => tx.CID_DISCOUNT);
                            }

                            hdfInvoiceType.Value = finInvoiceCusHdrList[0].ICH_CATEGORY.ToString();
                            hdfPostedJ.Value = finInvoiceCusHdrList[0].ICH_HAS_JRNL_ENTRY.ToString();
                            lblCustomerTxt.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceCusHdrList[0].CRM_CUSTOMER_MST.CUS_NAME, 45);
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


                            lblCustomerTxt.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceCusHdrList[0].CRM_CUSTOMER_MST.CUS_NAME, 300);
                            hdfCusPK.Value = finInvoiceCusHdrList[0].ICH_CUSTOMER.ToString();
                            txtPaymentCurrency.ToolTip = txtPaymentCurrency.Text = finInvoiceCusHdrList[0].ADM_CURRENCY_MST1.CUR_CODE;
                            hdfPaymentCurrency.Value = finInvoiceCusHdrList[0].ICH_CURRENCY.ToString();

                            //hdfTotalAmt.Value = (finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_TC - finInvoiceCusHdrList[e.Row.RowIndex].ICH_DISCOUNT_TC).ToString();
                            ////hdfTotalAmt.Value = (finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_NET_TC - finInvoiceCusHdrList[e.Row.RowIndex].ICH_TAX_TC - finInvoiceCusHdrList[e.Row.RowIndex].ICH_DISCOUNT_TC).ToString();
                            //hdfTotalAmt.Value = (finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_NET_TC - (finInvoiceCusHdrList[e.Row.RowIndex].ICH_TAX_TC + lineItemTax) - (finInvoiceCusHdrList[e.Row.RowIndex].ICH_DISCOUNT_TC + lineItemDiscount)).ToString();
                            if (finInvoiceCusHdrList[e.Row.RowIndex].ICH_TYPE == Convert.ToInt32(SalesInvoiceType.Domestic) && IsAdvInvHasTax)
                            {
                                if (IsTaxForOtherCharge.Value == "1")// No Need for Minus Other charge            
                                {
                                    hdfTotalAmt.Value = (finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_NET_TC - (finInvoiceCusHdrList[e.Row.RowIndex].ICH_TAX_TC + lineItemTax) - finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_ADJUST).ToString(); // Removed advance deduct
                                }
                                else
                                {
                                    hdfTotalAmt.Value = (finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_NET_TC - (finInvoiceCusHdrList[e.Row.RowIndex].ICH_TAX_TC + lineItemTax) - finInvoiceCusHdrList[e.Row.RowIndex].ICH_SHIP_CHARGE - finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_ADJUST).ToString(); // Removed advance deduct
                                }
                            }
                            else
                            {
                                if (IsTaxForOtherCharge.Value == "1")// No Need for Minus Other charge            
                                {
                                    hdfTotalAmt.Value = (finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_NET_TC - (finInvoiceCusHdrList[e.Row.RowIndex].ICH_TAX_TC + lineItemTax) - finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_ADJUST + finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_ADV_DED_TC).ToString();
                                }
                                else
                                {
                                    hdfTotalAmt.Value = (finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_NET_TC - (finInvoiceCusHdrList[e.Row.RowIndex].ICH_TAX_TC + lineItemTax) - finInvoiceCusHdrList[e.Row.RowIndex].ICH_SHIP_CHARGE - finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_ADJUST + finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_ADV_DED_TC).ToString();
                                }
                            }

                            //hdfTaxAmt.Value = finInvoiceCusHdrList[e.Row.RowIndex].ICH_TAX_TC.ToString() == string.Empty ? "0" : finInvoiceCusHdrList[e.Row.RowIndex].ICH_TAX_TC.ToString();

                            hdfInvoicePK.Value = finInvoiceCusHdrList[e.Row.RowIndex].ICH_PK.ToString();
                            hdfInvoiceType.Value = finInvoiceCusHdrList[e.Row.RowIndex].ICH_TYPE.ToString();
                            InvType = Convert.ToInt32(hdfInvoiceType.Value);
                            hdfhasjournalized.Value = finInvoiceCusHdrList[e.Row.RowIndex].ICH_HAS_JRNL_ENTRY.ToString();
                            hdfCrDbMpgPK.Value = "0";
                            lnkInvoiceNo.Text = finInvoiceCusHdrList[e.Row.RowIndex].ICH_NO;
                            lnkInvoiceNo.ToolTip = finInvoiceCusHdrList[e.Row.RowIndex].ICH_NO;
                            lnkInvoiceNo.CommandArgument = finInvoiceCusHdrList[e.Row.RowIndex].ICH_PK.ToString();
                            hdfInvTypeText.Value = finInvoiceCusHdrList[e.Row.RowIndex].ICH_TYPE.ToString();
                            hdfInvCategory.Value = finInvoiceCusHdrList[e.Row.RowIndex].ICH_CATEGORY.ToString();
                            hdfInvGroup.Value = finInvoiceCusHdrList[e.Row.RowIndex].ICH_GROUP.ToString();
                            //hdfInvTypeText.Value=finInvoiceCusHdrList[e.Row.RowIndex].ICH_TYPE.ToString();
                            //lblInvoiceNo.Text = finInvoiceCusHdrList[e.Row.RowIndex].ICH_NO;
                            //lblInvoiceNo.ToolTip = finInvoiceCusHdrList[e.Row.RowIndex].ICH_NO;
                            lblInvoiceDate.Text = finInvoiceCusHdrList[e.Row.RowIndex].ICH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblInvoiceDate.ToolTip = finInvoiceCusHdrList[e.Row.RowIndex].ICH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblVendorInv.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceCusHdrList[e.Row.RowIndex].CRM_CUSTOMER_MST.CUS_NAME, 10);
                            lblVendorInv.ToolTip = finInvoiceCusHdrList[e.Row.RowIndex].CRM_CUSTOMER_MST.CUS_NAME;

                            //lblGrossAmount.Text = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].ICH_NET_VALUE_TC);
                            //lblGrossAmount.ToolTip = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].ICH_NET_VALUE_TC);
                            //lblGrossAmount.Text = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_TC);
                            //lblGrossAmount.ToolTip = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_TC);
                            lblGrossAmount.Text = String.Format("{0:c}", Convert.ToDecimal(hdfTotalAmt.Value));
                            lblGrossAmount.ToolTip = String.Format("{0:c}", Convert.ToDecimal(hdfTotalAmt.Value));

                            lblInvoiceAmount.Text = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_NET_TC);
                            lblInvoiceAmount.ToolTip = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_NET_TC);

                            hdfTaxAmt.Value = (finInvoiceCusHdrList[e.Row.RowIndex].ICH_TAX_TC + lineItemTax).ToString();
                            lblTax.Text = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].ICH_TAX_TC + lineItemTax);
                            lblTax.ToolTip = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].ICH_TAX_TC + lineItemTax);

                            lblDiscount.Text = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].ICH_DISCOUNT_TC + lineItemDiscount);
                            lblDiscount.ToolTip = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].ICH_DISCOUNT_TC + lineItemDiscount);

                            lblOtherAmount.Text = lblOtherAmount.ToolTip = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].ICH_SHIP_CHARGE);

                            lblTotalAmount.Text = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].ICH_NET_VALUE_TC);
                            lblTotalAmount.ToolTip = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].ICH_NET_VALUE_TC);
                            lblPaid.Text = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_RCVD_TC);
                            lblPaid.ToolTip = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_RCVD_TC);

                            decimal balToPay = finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_NET_TC -
                                               finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_RCVD_TC -
                                               finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_CN_TC +
                                               finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_DN_TC;

                            lblBaltopay.Text = String.Format("{0:c}", balToPay);
                            lblBaltopay.ToolTip = String.Format("{0:c}", balToPay);
                            balToPay = balToPay < 0 ? 0 : balToPay;
                            //txtNoteFor.Text = Math.Round(balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                            TotalCNAmount = GetCNTotalAmount(finInvoiceCusHdrList[e.Row.RowIndex].ICH_PK);
                            lblTotCNAmount.Text = String.Format("{0:c}", TotalCNAmount);
                            hdfTotalCNAmount.Value = TotalCNAmount.ToString();
                            if (finInvoiceCusHdrList[e.Row.RowIndex].ICH_CATEGORY == (int)SalesInvoiceCategory.Advanced)
                            {
                                RaiseNote = finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_NET_TC - TotalCNAmount;
                                txtNoteFor.Text = txtNoteFor.ToolTip = RaiseNote > 0 ? GetFormattedCurrency(RaiseNote) : GetFormattedCurrency(0);
                                //txtNoteFor.Text = txtNoteFor.ToolTip =  Math.Round(finInvoiceCusHdrList[e.Row.RowIndex].FIN_RECEIPT_CUS_TRX_MPG.Where(r => r.FIN_RECEIPT_CUS_HDR.RCH_DEL_STATUS == 0 && r.FIN_RECEIPT_CUS_HDR.RCH_HAS_JRNL_ENTRY).Sum(s => s.RCM_RCVD_AMOUNT), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                lnkAllocation.Visible = false;
                                lbnTotalTax.CssClass = "nomargin";
                                lbnTotalTax.Enabled = false;
                                txtNoteFor.Enabled = true;
                                hdfhasjournalized.Value = "True";
                            }
                            else
                            {
                                txtNoteFor.Text = txtNoteFor.ToolTip = GetFormattedCurrency(0);
                                lbnTotalTax.CssClass = "text-underline nomargin";
                                lbnTotalTax.Enabled = true;
                                txtNoteFor.Enabled = false;
                            }

                            //Math.Round(balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            lnkRemove.CommandArgument = finInvoiceCusHdrList[e.Row.RowIndex].ICH_PK.ToString();
                            lblAdjAmount.Text = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_DN_TC - finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_CN_TC);
                            lblAdjAmount.ToolTip = String.Format("{0:c}", finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_DN_TC - finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_CN_TC);

                            taxpercentage = Convert.ToDecimal(hdfTaxAmt.Value) / (Convert.ToDecimal(finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_TC - finInvoiceCusHdrList[e.Row.RowIndex].ICH_DISCOUNT_TC + lineItemDiscount) == 0 ? 1 : Convert.ToDecimal(finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_TC - finInvoiceCusHdrList[e.Row.RowIndex].ICH_DISCOUNT_TC + lineItemDiscount));
                            basevalue = (txtNoteFor.Text != string.Empty ? Convert.ToDecimal(txtNoteFor.Text) : 0) / (1 + taxpercentage);
                            taxamt = ((txtNoteFor.Text != string.Empty ? Convert.ToDecimal(txtNoteFor.Text) : 0) - basevalue);
                            //lblTotalTax.Text = Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //lblTotalTax.ToolTip = lblTotalTax.Text;
                            lbnTotalTax.Text = String.Format("{0:c}", Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                            lbnTotalTax.ToolTip = lbnTotalTax.Text;
                            hdfTotalTax.Value = Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();


                        }
                        if (isLineItemTaxEnabled)
                        {
                            lbnTotalTax.CssClass = "nomargin";
                            lbnTotalTax.Enabled = false;
                        }

                        //txtNoteFor.Focus();
                    }

                    if (((GridView)sender).ID == "grdCrDbHdr")
                    {
                        Button imgApproved = e.Row.FindControl("imgApproved") as Button;
                        Button imgPosted = e.Row.FindControl("imgPosted") as Button;
                        HiddenField hdfApproved = e.Row.FindControl("hdfApproved") as HiddenField;
                        HiddenField hdfPosted = e.Row.FindControl("hdfPosted") as HiddenField;
                        short appstatus = Convert.ToInt16(hdfApproved.Value);

                        LinkButton lnkInvnos = e.Row.FindControl("lnkInvnos") as LinkButton;
                        Label lblInvDate = e.Row.FindControl("lblInvDate") as Label;
                        HiddenField hdfListinvPK = e.Row.FindControl("hdfListinvPK") as HiddenField;
                        HiddenField hdfListInvType = e.Row.FindControl("hdfListInvType") as HiddenField;
                        HiddenField hdfListinvCategory = e.Row.FindControl("hdfListinvCategory") as HiddenField;
                        HiddenField hdfListGroup = e.Row.FindControl("hdfListGroup") as HiddenField;


                        if (finCrDrNoteHdrList != null && finCrDrNoteHdrList.Count > 0)
                        {
                            List<FIN_CRDR_NOTE_MPG> crDrNoreMapList = finCrDrNoteHdrList[e.Row.RowIndex].FIN_CRDR_NOTE_MPG.ToList();
                            var CrDrtype = crDrNoreMapList[0].FIN_INVOICE_CUS_HDR.ICH_TYPE;
                            HiddenField hdfCrDrType = e.Row.FindControl("hdfCrDrType") as HiddenField;
                            hdfCrDrType.Value = CrDrtype.ToString();
                            Label lblModeofPayment = e.Row.FindControl("lblModeofPayment") as Label;
                            int cfgpk = Convert.ToInt32(lblModeofPayment.Text);
                            //GetFieldValues(ControlsEnum.CRDRTYPE);
                            if (admConfigMstListCrdrType != null && admConfigMstListCrdrType.Count > 0)
                            {
                                lblModeofPayment.Text = admConfigMstListCrdrType.SingleOrDefault(con => con.CFG_VALUE == cfgpk).CFG_DATA;
                                lblModeofPayment.ToolTip = admConfigMstListCrdrType.SingleOrDefault(con => con.CFG_VALUE == cfgpk).CFG_DATA;
                            }
                            if (CrDrtype == 1)//For Domestic
                            {
                                //transaction icon & tooltip
                                if (workflowStatusDomesticList != null && workflowStatusDomesticList.Count > 0)
                                {
                                    wkfStatus = workflowStatusDomesticList.SingleOrDefault(aa => aa.ASC_VALUE == appstatus);
                                    if (wkfStatus != null)
                                    {
                                        imgApproved.ToolTip = wkfStatus.ASC_NAME;
                                        imgApproved.CssClass = wkfStatus.ASC_CSS_CLASS;
                                    }
                                }
                            }
                            else if (CrDrtype == 2 || CrDrtype == 3 || CrDrtype == 4)//For 2=Export, 4=Deemed Export
                            {
                                //transaction icon & tooltip
                                if (workflowStatusExportList != null && workflowStatusExportList.Count > 0)
                                {
                                    wkfStatus = workflowStatusExportList.SingleOrDefault(aa => aa.ASC_VALUE == appstatus);
                                    if (wkfStatus != null)
                                    {
                                        imgApproved.ToolTip = wkfStatus.ASC_NAME;
                                        imgApproved.CssClass = wkfStatus.ASC_CSS_CLASS;
                                    }
                                }
                            }

                            //Show Invoice No & Invoice date instead of Ref no & Ref Date
                            #region Show Invoice No & Invoice date

                            FIN_CRDR_NOTE_HDR crdrHdr = finCrDrNoteHdrList.SingleOrDefault(x => x.CDH_PK == Convert.ToInt32(((HiddenField)e.Row.FindControl("hdfCrDrPk")).Value));

                            if (crdrHdr != null)
                            {
                                string invoices = string.Empty;
                                int invCount = 0;
                                var invNo = from c in crdrHdr.FIN_CRDR_NOTE_MPG select c.FIN_INVOICE_CUS_HDR.ICH_NO;
                                DateTime invDate = Convert.ToDateTime(crdrHdr.FIN_CRDR_NOTE_MPG.ToList()[0].FIN_INVOICE_CUS_HDR.ICH_DATE);
                                int invPk = Convert.ToInt32(crdrHdr.FIN_CRDR_NOTE_MPG.ToList()[0].FIN_INVOICE_CUS_HDR.ICH_PK);
                                int invType = crdrHdr.FIN_CRDR_NOTE_MPG.ToList()[0].FIN_INVOICE_CUS_HDR.ICH_TYPE;
                                int invCategory = crdrHdr.FIN_CRDR_NOTE_MPG.ToList()[0].FIN_INVOICE_CUS_HDR.ICH_CATEGORY;
                                int invGroup = crdrHdr.FIN_CRDR_NOTE_MPG.ToList()[0].FIN_INVOICE_CUS_HDR.ICH_GROUP;

                                if (invNo != null && invNo.Count() > 0)
                                {
                                    foreach (string invNumber in invNo)
                                    {
                                        invoices += invNumber + ",";
                                        invCount++;
                                    }

                                    invoices = invoices.TrimEnd(',');
                                    lnkInvnos.Text = CommonFunctions.GetShortString(invoices, 15);
                                    lblInvDate.Text = invDate.ToString(Resources.Constants.DateFormatShort);
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
                            //
                        }



                        //Voucher Icon
                        HiddenField hdfCreditDebitType = e.Row.FindControl("hdfCreditDebitType") as HiddenField;
                        creditDebitType = Convert.ToInt32(hdfCreditDebitType.Value);
                        CurrPK = Convert.ToInt32(((HiddenField)e.Row.FindControl("hdfCrDrPk")).Value);
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
                        decimal BalAmount = 0;
                        decimal Amount = 0;
                        decimal AllocatedAmount = 0;
                        Label lblBalanceAmount = e.Row.FindControl("lblBalanceAmount") as Label;
                        LinkButton lnkBalanceAmount = e.Row.FindControl("lnkBalanceAmount") as LinkButton;
                        Label lblAmount = e.Row.FindControl("lblAmount") as Label;
                        decimal.TryParse(lblAmount.Text, out Amount);
                        //GetFieldValues(ControlsEnum.CRDRALLOCATEDAMOUNT);
                        GetFieldValues(ControlsEnum.BALANCEAMOUNTSPLIT);
                        if (objCrdrAllocations != null && objCrdrAllocations.Count() > 0)
                        {
                            AllocatedAmount = objCrdrAllocations.Sum(r => r.TRX_AMOUNT);
                            BalAmount = Amount - AllocatedAmount;
                            if (BalAmount < 0)
                                BalAmount = 0;
                            lblBalanceAmount.Text = Math.Round(BalAmount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            lnkBalanceAmount.Text = Math.Round(BalAmount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        }
                        if (Convert.ToDecimal(lblAmount.Text.Replace(",", "")) == Convert.ToDecimal(lnkBalanceAmount.Text.Replace(",", "")))
                        {
                            lblBalanceAmount.Visible = true;
                            lnkBalanceAmount.Visible = false;
                            //lnkBalanceAmount.Attributes.Remove("href");
                            //lnkBalanceAmount.CssClass.Replace("text-underline", "");
                            //lnkBalanceAmount.Enabled = false;
                        }
                        else
                        {
                            lblBalanceAmount.Visible = false;
                            lnkBalanceAmount.Visible = true;
                        }

                        //BalAmount = Amount - CrDrAllocatedAmnt;
                        //if (BalAmount < 0)
                        //    BalAmount = 0;
                        //lblBalanceAmount.Text = Math.Round(BalAmount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                    }
                    if (((GridView)sender).ID == "grdDCSplit")
                    {

                        //grdDCSplit.Columns[4].Visible = grdDCSplit.Columns[9].Visible = EnableItemTax;
                        grdDCSplit.Columns[6].Visible = grdDCSplit.Columns[7].Visible = grdDCSplit.Columns[12].Visible = isLineItemTaxEnabled;


                        hdfInvCusDtlPK = e.Row.FindControl("hdfInvCusDtlPK") as HiddenField;
                        hdfReceiptSplitPK = e.Row.FindControl("hdfDCSplitPK") as HiddenField;
                        hdfReceiptTRXPK = e.Row.FindControl("hdfDCTRXPK") as HiddenField;
                        lblPRODUCTSplit = e.Row.FindControl("lblPRODUCTSplit") as Label;
                        hdfNETSplit = e.Row.FindControl("hdfNETSplit") as HiddenField;

                        lblQTYSplit = e.Row.FindControl("lblQTYSplit") as Label;
                        lblRATESplit = e.Row.FindControl("lblRATESplit") as Label;
                        lblAmountSplit = e.Row.FindControl("lblAmountSplit") as Label;
                        lblTAXSplit = e.Row.FindControl("lblTAXSplit") as Label;
                        lblNETSplit = e.Row.FindControl("lblNETSplit") as Label;
                        txtQtySplit = e.Row.FindControl("txtQtySplit") as TextBox;
                        txtRateSplit = e.Row.FindControl("txtRateSplit") as TextBox;
                        txtSumSplit = e.Row.FindControl("txtSumSplit") as TextBox;
                        hdfSumSplit = e.Row.FindControl("hdfSumSplit") as HiddenField;

                        //lblTAXSplitTotal = e.Row.FindControl("lblTAXSplitTotal") as Label;
                        lbnTAXSplitTotal = e.Row.FindControl("lbnTAXSplitTotal") as LinkButton;
                        hdfTAXSplit = e.Row.FindControl("hdfTAXSplit") as HiddenField;

                        hdfTAXSplitTotal = e.Row.FindControl("hdfTAXSplitTotal") as HiddenField;

                        Label lblUomSales = e.Row.FindControl("lblUomSales") as Label;
                        Label lblQTYSplitUom = e.Row.FindControl("lblQTYSplitUom") as Label;
                        Label lblDiscountSplit = e.Row.FindControl("lblDiscountSplit") as Label;

                        decimal.TryParse(txtExchangeRate.Text, out Exchangerate);
                        if (FinCrDrCusTrxMpgList != null && FinCrDrCusTrxMpgList.Count > 0)
                        {
                            hdfInvCusDtlPK.Value = FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_PK.ToString();
                            hdfReceiptSplitPK.Value = "0";
                            hdfReceiptTRXPK.Value = "0";
                            //lblPRODUCTSplit.Text = FinCrDrCusTrxMpgList[e.Row.RowIndex].INV_ITEM_MST.ITM_NAME;
                            //lblPRODUCTSplit.ToolTip = FinCrDrCusTrxMpgList[e.Row.RowIndex].INV_ITEM_MST.ITM_NAME;
                            lblPRODUCTSplit.Text = lblPRODUCTSplit.ToolTip = HttpUtility.HtmlDecode(FinCrDrCusTrxMpgList[e.Row.RowIndex].INV_ITEM_MST != null ? FinCrDrCusTrxMpgList[e.Row.RowIndex].INV_ITEM_MST.ITM_NAME : FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_INSTRUCTIONS);

                            //lblQTYSplit.Text = String.Format("{0:n}", FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_QTY_INVOICED);
                            //lblQTYSplit.ToolTip = String.Format("{0:n}", FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_QTY_INVOICED);

                            if (FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_SALE_UOM.HasValue)
                            {
                                lblUomSales.ToolTip = lblUomSales.Text = FinCrDrCusTrxMpgList[e.Row.RowIndex].ADM_CONFIG_MST.CFG_DATA;
                            }

                            double InvdQty = FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_QTY_INVOICED / (FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_SALE_UOM_CONV.HasValue ? (FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_SALE_UOM_CONV.Value > 0 ? FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_SALE_UOM_CONV.Value : 1) : 1);
                            lblQTYSplit.ToolTip = lblQTYSplit.Text = String.Format("{0:n}", InvdQty);

                            lblQTYSplitUom.Text = String.Format("{0:n}", FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_QTY_INVOICED);
                            lblQTYSplitUom.ToolTip = String.Format("{0:n}", FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_QTY_INVOICED);


                            lblRATESplit.Text = GetFormattedRate(FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_RATE);
                            lblRATESplit.ToolTip = GetFormattedRate(FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_RATE);

                            lblAmountSplit.Text = String.Format("{0:c}", FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_AMOUNT);
                            lblAmountSplit.ToolTip = String.Format("{0:c}", FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_AMOUNT);

                            if (lblDiscountSplit != null)
                                lblDiscountSplit.Text = lblDiscountSplit.ToolTip = String.Format("{0:c}", FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_DISCOUNT);

                            hdfNETSplit.Value = FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_AMOUNT.ToString();

                            hdfTAXSplit.Value = FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_TAX.ToString();
                            lblTAXSplit.Text = String.Format("{0:c}", FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_TAX);
                            lblTAXSplit.ToolTip = String.Format("{0:c}", FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_TAX);


                            lblNETSplit.Text = String.Format("{0:c}", FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_NET_AMOUNT);
                            lblNETSplit.ToolTip = String.Format("{0:c}", FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_NET_AMOUNT);

                            if (tempfinCrDrCusMpgList != null)
                            {
                                tempFinReceiptCusSoMpgObj = tempfinCrDrCusMpgList.SingleOrDefault(mpg => mpg.CDS_INVOICE_CUS_DTL == Convert.ToInt64(hdfInvCusDtlPK.Value));
                            }

                            txtQtySplit.Text = tempFinReceiptCusSoMpgObj == null ? GetFormattedNumber(0) : GetFormattedNumber(tempFinReceiptCusSoMpgObj.CDS_QTY);
                            txtQtySplit.ToolTip = tempFinReceiptCusSoMpgObj == null ? GetFormattedNumber(0) : GetFormattedNumber(tempFinReceiptCusSoMpgObj.CDS_QTY);
                            //txtQtySplit.ToolTip =
                            //   tempFinReceiptCusSoMpgObj == null ?
                            //   Math.Round(Convert.ToDouble(0), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString()
                            //   : Math.Round(tempFinReceiptCusSoMpgObj.CDS_QTY, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                            //txtRateSplit.Text = tempFinReceiptCusSoMpgObj == null ? GetFormattedRate(FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_RATE) : GetFormattedRate(tempFinReceiptCusSoMpgObj.CDS_RATE);
                            //txtRateSplit.ToolTip = tempFinReceiptCusSoMpgObj == null ? GetFormattedRate(FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_RATE) : GetFormattedRate(tempFinReceiptCusSoMpgObj.CDS_RATE);
                            txtRateSplit.Text = tempFinReceiptCusSoMpgObj == null ? FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_RATE.ToString() : tempFinReceiptCusSoMpgObj.CDS_RATE.ToString();
                            txtRateSplit.ToolTip = tempFinReceiptCusSoMpgObj == null ? FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_RATE.ToString() : tempFinReceiptCusSoMpgObj.CDS_RATE.ToString();


                            //txtRateSplit.ToolTip = tempFinReceiptCusSoMpgObj == null ?
                            //    Math.Round(Convert.ToDouble(0), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString()
                            //    : Math.Round(tempFinReceiptCusSoMpgObj.CDS_RATE, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                            double qty = 0;
                            double rate = 0;
                            Double.TryParse(txtQtySplit.Text, out qty);
                            Double.TryParse(txtRateSplit.Text, out rate);

                            if (tempfinCrDrCusMpgList != null)
                            {
                                txtSumSplit.Text = tempFinReceiptCusSoMpgObj == null ? GetFormattedCurrency(0) : GetFormattedCurrency(tempFinReceiptCusSoMpgObj.CDS_AMOUNT);
                                hdfSumSplit.Value = tempFinReceiptCusSoMpgObj == null ? GetFormattedCurrency(0) : GetFormattedCurrency(tempFinReceiptCusSoMpgObj.CDS_AMOUNT);
                                txtSumSplit.ToolTip = tempFinReceiptCusSoMpgObj == null ? GetFormattedCurrency(0) : GetFormattedCurrency(tempFinReceiptCusSoMpgObj.CDS_AMOUNT);
                                //   txtSumSplit.ToolTip = tempFinReceiptCusSoMpgObj == null ?
                                //Math.Round(Convert.ToDouble(0), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString()
                                //: Math.Round(tempFinReceiptCusSoMpgObj.CDS_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            }
                            else
                            {
                                txtSumSplit.Text = Math.Round((qty * rate), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                hdfSumSplit.Value = Math.Round((qty * rate), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                txtSumSplit.ToolTip = Math.Round((qty * rate), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            }
                            if (tempFinReceiptCusSoMpgObj == null)
                            {
                                taxpercentage = Convert.ToDecimal(hdfTAXSplit.Value) / (Convert.ToDecimal(FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_AMOUNT - FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_DISCOUNT) == 0 ? 1 : Convert.ToDecimal(FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_AMOUNT - FinCrDrCusTrxMpgList[e.Row.RowIndex].CID_DISCOUNT));
                                basevalue = (txtSumSplit.Text != string.Empty ? Convert.ToDecimal(txtSumSplit.Text) : 0) / (1 + taxpercentage);
                                taxamt = ((txtSumSplit.Text != string.Empty ? Convert.ToDecimal(txtSumSplit.Text) : 0) - basevalue);
                            }
                            else
                                taxamt = tempFinReceiptCusSoMpgObj.CDS_TAX;

                            //lblTAXSplitTotal.Text = Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //lblTAXSplitTotal.ToolTip = lblTAXSplitTotal.Text;
                            lbnTAXSplitTotal.Text = GetFormattedCurrencyWithComa(Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                            lbnTAXSplitTotal.ToolTip = lbnTAXSplitTotal.Text;
                            hdfTAXSplitTotal.Value = Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            grndTotalTaxSplit += Convert.ToDecimal(lbnTAXSplitTotal.Text.Replace(",", ""));

                        }
                        else if (finCrDrCusMpgList != null && finCrDrCusMpgList.Count > 0)
                        {

                            if (finInvItemList != null && finInvItemList.Count > 0)
                            {
                                finInvItemDtl = finInvItemList.SingleOrDefault(itm => itm.CID_PK == finCrDrCusMpgList[e.Row.RowIndex].CDS_INVOICE_CUS_DTL);
                                if (finInvItemDtl != null)
                                {
                                    hdfInvCusDtlPK.Value = finInvItemDtl.CID_PK.ToString();
                                    //hdfInvCusDtlPK.Value =
                                    hdfReceiptSplitPK.Value = finCrDrCusMpgList[e.Row.RowIndex].CDS_PK.ToString();
                                    hdfReceiptTRXPK.Value = finCrDrCusMpgList[e.Row.RowIndex].CDS_INVOICE_CUS_DTL.ToString();
                                    //lblPRODUCTSplit.Text = finInvItemDtl.INV_ITEM_MST.ITM_NAME;
                                    //lblPRODUCTSplit.ToolTip = finInvItemDtl.INV_ITEM_MST.ITM_NAME.ToString();

                                    lblPRODUCTSplit.Text = lblPRODUCTSplit.ToolTip = HttpUtility.HtmlDecode(finInvItemDtl.INV_ITEM_MST != null ? finInvItemDtl.INV_ITEM_MST.ITM_NAME : finInvItemDtl.CID_INSTRUCTIONS);


                                    ////// commented for  bug : 2010
                                    ////lblQTYSplit.Text = String.Format("{0:n}", (finCrDrCusMpgList[e.Row.RowIndex].CDS_QTY > 0) ? finCrDrCusMpgList[e.Row.RowIndex].CDS_QTY : finInvItemDtl.CID_QTY_INVOICED);
                                    ////lblQTYSplit.ToolTip = String.Format("{0:n}", (finCrDrCusMpgList[e.Row.RowIndex].CDS_QTY > 0) ? finCrDrCusMpgList[e.Row.RowIndex].CDS_QTY : finInvItemDtl.CID_QTY_INVOICED);

                                    ////lblRATESplit.Text = String.Format("{0:c}", (finCrDrCusMpgList[e.Row.RowIndex].CDS_RATE > 0) ? Convert.ToDouble(finCrDrCusMpgList[e.Row.RowIndex].CDS_RATE) : Convert.ToDouble(finInvItemDtl.CID_RATE));
                                    ////lblRATESplit.ToolTip = String.Format("{0:c}", (finCrDrCusMpgList[e.Row.RowIndex].CDS_RATE > 0) ? Convert.ToDouble(finCrDrCusMpgList[e.Row.RowIndex].CDS_RATE) : Convert.ToDouble(finInvItemDtl.CID_RATE));

                                    ////lblAmountSplit.Text = String.Format("{0:c}", (finCrDrCusMpgList[e.Row.RowIndex].CDS_AMOUNT > 0) ? finCrDrCusMpgList[e.Row.RowIndex].CDS_AMOUNT : finInvItemDtl.CID_AMOUNT);
                                    ////lblAmountSplit.ToolTip = String.Format("{0:c}", (finCrDrCusMpgList[e.Row.RowIndex].CDS_AMOUNT > 0) ? finCrDrCusMpgList[e.Row.RowIndex].CDS_AMOUNT : finInvItemDtl.CID_AMOUNT);
                                    ////hdfNETSplit.Value = ((finCrDrCusMpgList[e.Row.RowIndex].CDS_AMOUNT > 0) ? finCrDrCusMpgList[e.Row.RowIndex].CDS_AMOUNT : finInvItemDtl.CID_AMOUNT).ToString();

                                    ////lblNETSplit.Text = String.Format("{0:c}", (finCrDrCusMpgList[e.Row.RowIndex].CDS_NET_AMOUNT > 0) ? finCrDrCusMpgList[e.Row.RowIndex].CDS_NET_AMOUNT : finInvItemDtl.CID_NET_AMOUNT);
                                    ////lblNETSplit.ToolTip = String.Format("{0:c}", (finCrDrCusMpgList[e.Row.RowIndex].CDS_NET_AMOUNT > 0) ? finCrDrCusMpgList[e.Row.RowIndex].CDS_NET_AMOUNT : finInvItemDtl.CID_NET_AMOUNT);

                                    ////hdfTAXSplit.Value = ((finCrDrCusMpgList[e.Row.RowIndex].CDS_TAX > 0) ? finCrDrCusMpgList[e.Row.RowIndex].CDS_TAX : finInvItemDtl.CID_TAX).ToString();
                                    ////lblTAXSplit.Text = String.Format("{0:c}", (finCrDrCusMpgList[e.Row.RowIndex].CDS_TAX > 0) ? finCrDrCusMpgList[e.Row.RowIndex].CDS_TAX : finInvItemDtl.CID_TAX);
                                    ////lblTAXSplit.ToolTip = String.Format("{0:c}", (finCrDrCusMpgList[e.Row.RowIndex].CDS_TAX > 0) ? finCrDrCusMpgList[e.Row.RowIndex].CDS_TAX : finInvItemDtl.CID_TAX);

                                    //lblQTYSplit.Text = String.Format("{0:n}", finInvItemDtl.CID_QTY_INVOICED);
                                    //lblQTYSplit.ToolTip = String.Format("{0:n}", finInvItemDtl.CID_QTY_INVOICED);
                                    if (finInvItemDtl.CID_SALE_UOM.HasValue)
                                    {
                                        lblUomSales.ToolTip = lblUomSales.Text = finInvItemDtl.ADM_CONFIG_MST.CFG_DATA;
                                    }
                                    double InvdQty = finInvItemDtl.CID_QTY_INVOICED / (finInvItemDtl.CID_SALE_UOM_CONV.HasValue ? (finInvItemDtl.CID_SALE_UOM_CONV.Value > 0 ? finInvItemDtl.CID_SALE_UOM_CONV.Value : 1) : 1);
                                    lblQTYSplit.ToolTip = lblQTYSplit.Text = String.Format("{0:n}", InvdQty);

                                    lblQTYSplitUom.Text = String.Format("{0:n}", finInvItemDtl.CID_QTY_INVOICED);
                                    lblQTYSplitUom.ToolTip = String.Format("{0:n}", finInvItemDtl.CID_QTY_INVOICED);


                                    lblRATESplit.Text = String.Format("{0:c}", Convert.ToDouble(finInvItemDtl.CID_RATE));
                                    lblRATESplit.ToolTip = String.Format("{0:c}", Convert.ToDouble(finInvItemDtl.CID_RATE));

                                    lblAmountSplit.Text = String.Format("{0:c}", finInvItemDtl.CID_AMOUNT);
                                    lblAmountSplit.ToolTip = String.Format("{0:c}", finInvItemDtl.CID_AMOUNT);

                                    if (lblDiscountSplit != null)
                                        lblDiscountSplit.Text = lblDiscountSplit.ToolTip = String.Format("{0:c}", finInvItemDtl.CID_DISCOUNT);

                                    hdfNETSplit.Value = (finInvItemDtl.CID_AMOUNT).ToString();

                                    lblNETSplit.Text = String.Format("{0:c}", finInvItemDtl.CID_NET_AMOUNT);
                                    lblNETSplit.ToolTip = String.Format("{0:c}", finInvItemDtl.CID_NET_AMOUNT);

                                    hdfTAXSplit.Value = (finInvItemDtl.CID_TAX).ToString();
                                    lblTAXSplit.Text = String.Format("{0:c}", finInvItemDtl.CID_TAX);
                                    lblTAXSplit.ToolTip = String.Format("{0:c}", finInvItemDtl.CID_TAX);


                                    if (tempfinCrDrCusMpgList != null)
                                    {
                                        tempFinReceiptCusSoMpgObj = tempfinCrDrCusMpgList.SingleOrDefault(mpg => mpg.CDS_INVOICE_CUS_DTL == Convert.ToInt64(hdfReceiptTRXPK.Value));
                                    }
                                    txtQtySplit.Text = tempFinReceiptCusSoMpgObj == null ? GetFormattedNumber(0) : GetFormattedNumber(tempFinReceiptCusSoMpgObj.CDS_QTY);
                                    txtQtySplit.ToolTip = tempFinReceiptCusSoMpgObj == null ? GetFormattedNumber(0) : GetFormattedNumber(tempFinReceiptCusSoMpgObj.CDS_QTY);
                                    //txtQtySplit.ToolTip =
                                    //  tempFinReceiptCusSoMpgObj == null ?
                                    //  Math.Round(Convert.ToDouble(0), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString()
                                    //  : Math.Round(tempFinReceiptCusSoMpgObj.CDS_QTY, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                                    //txtRateSplit.Text = tempFinReceiptCusSoMpgObj == null ? GetFormattedRate(finInvItemDtl.CID_RATE) : GetFormattedRate(tempFinReceiptCusSoMpgObj.CDS_RATE);
                                    //txtRateSplit.ToolTip = tempFinReceiptCusSoMpgObj == null ? GetFormattedRate(finInvItemDtl.CID_RATE) : GetFormattedRate(tempFinReceiptCusSoMpgObj.CDS_RATE);
                                    txtRateSplit.Text = tempFinReceiptCusSoMpgObj == null ? finInvItemDtl.CID_RATE.ToString() : tempFinReceiptCusSoMpgObj.CDS_RATE.ToString();
                                    txtRateSplit.ToolTip = tempFinReceiptCusSoMpgObj == null ? finInvItemDtl.CID_RATE.ToString() : tempFinReceiptCusSoMpgObj.CDS_RATE.ToString();


                                    //txtRateSplit.ToolTip = tempFinReceiptCusSoMpgObj == null ?
                                    //   Math.Round(Convert.ToDouble(0), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString()
                                    //   : Math.Round(finCrDrCusMpgList[e.Row.RowIndex].CDS_RATE, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                                    double qty = 0;
                                    double rate = 0;
                                    Double.TryParse(txtQtySplit.Text, out qty);
                                    Double.TryParse(txtRateSplit.Text, out rate);

                                    if (tempfinCrDrCusMpgList != null)
                                    {
                                        txtSumSplit.Text = tempFinReceiptCusSoMpgObj == null ? GetFormattedCurrency(0) : GetFormattedCurrency(tempFinReceiptCusSoMpgObj.CDS_AMOUNT);
                                        hdfSumSplit.Value = tempFinReceiptCusSoMpgObj == null ? GetFormattedCurrency(0) : GetFormattedCurrency(tempFinReceiptCusSoMpgObj.CDS_AMOUNT);
                                        txtSumSplit.ToolTip = tempFinReceiptCusSoMpgObj == null ? GetFormattedCurrency(0) : GetFormattedCurrency(tempFinReceiptCusSoMpgObj.CDS_AMOUNT);
                                        //   txtSumSplit.ToolTip = tempFinReceiptCusSoMpgObj == null ?
                                        //Math.Round(Convert.ToDouble(0), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString()
                                        //: Math.Round(tempFinReceiptCusSoMpgObj.CDS_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                    }
                                    else
                                    {
                                        txtSumSplit.Text = Math.Round((qty * rate), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                        hdfSumSplit.Value = Math.Round((qty * rate), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                        txtSumSplit.ToolTip = Math.Round((qty * rate), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                    }
                                    lbnTAXSplitTotal.Text = tempFinReceiptCusSoMpgObj == null ?
                                     GetFormattedCurrency(0)
                                     : GetFormattedCurrencyWithComa(Math.Round(tempFinReceiptCusSoMpgObj.CDS_TAX, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                                    lbnTAXSplitTotal.ToolTip = lbnTAXSplitTotal.Text;
                                    hdfTAXSplitTotal.Value = tempFinReceiptCusSoMpgObj == null ?
                                     GetFormattedCurrency(0)
                                     : Math.Round(tempFinReceiptCusSoMpgObj.CDS_TAX, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();


                                    grndTotalTaxSplit += Convert.ToDecimal(lbnTAXSplitTotal.Text.Replace(",", ""));

                                }
                            }
                        }
                    }
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
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (((GridView)sender).ID == "grdDCSplit")
                    {
                        if (FinCrDrCusTrxMpgList != null && FinCrDrCusTrxMpgList.Count > 0)
                        {
                            e.Row.Cells[3].Text = GetLocalResourceObject("DCQTY").ToString() + " (" + FinCrDrCusTrxMpgList[0].INV_UOM_MST.UOM_CODE + ")";
                        }
                        else if (finCrDrCusMpgList != null && finCrDrCusMpgList.Count > 0) //finInvItemDtl != null
                        {
                            e.Row.Cells[3].Text = GetLocalResourceObject("DCQTY").ToString() + " (" + finCrDrCusMpgList[0].FIN_INVOICE_CUS_DTL.INV_UOM_MST.UOM_CODE + ")"; //finInvItemDtl.INV_UOM_MST.UOM_CODE
                        }
                    }
                }
                if (((GridView)sender).ID != "grdUploads")
                {
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        lblTotalFooter = e.Row.FindControl("lblTotalPayNowFooter") as Label;
                        hdfPayNowFooter = e.Row.FindControl("hdfPayNowFooter") as HiddenField;
                        if (finCrDrNoteMpgList != null && finCrDrNoteMpgList.Count > 0)
                        {
                            total = 0;
                            total = finCrDrNoteMpgList.Sum(dtl => dtl.CDM_AMOUNT);
                            total = total < 0 ? 0 : total;
                            //lblTotalFooter.Text = Math.Round(total,Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            hdfPayNowFooter.Value = lblTotalFooter.Text = total < 0 ? GetFormattedCurrency(0) : GetFormattedCurrency(total);

                            //string.Format("{0:c}", total);
                            hdfTotalPayNowFooter.Value = Math.Round(total, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            txtPaidAmount.Text = Math.Round(total, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                        }
                        else if (finInvoiceCusHdrList != null && finInvoiceCusHdrList.Count > 0)
                        {
                            total = 0;
                            total = finInvoiceCusHdrList.Sum(dtl => dtl.ICH_AMOUNT_NET_TC -
                                                                    dtl.ICH_AMOUNT_RCVD_TC -
                                                                    dtl.ICH_AMOUNT_CN_TC +
                                                                    dtl.ICH_AMOUNT_DN_TC);
                            total = total < 0 ? 0 : total;
                            //lblTotalFooter.Text = Math.Round(total, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            hdfPayNowFooter.Value = lblTotalFooter.Text = total < 0 ? GetFormattedCurrency(0) : GetFormattedCurrency(total);
                            hdfTotalPayNowFooter.Value = Math.Round(total, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            txtPaidAmount.Text = Math.Round(total, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        }

                        if (((GridView)sender).ID == "grdDCSplit")
                        {
                            //Test

                            lblTotalPayNowFooterSplit = e.Row.FindControl("lblTotalPayNowFooterSplit") as Label;
                            hdfTotalPayNowFooterSplit = e.Row.FindControl("hdfTotalPayNowFooterSplit") as HiddenField;

                            lblTotalTaxFooterSplit = e.Row.FindControl("lblTotalTaxFooterSplit") as Label;
                            hdfTotalTaxFooterSplit = e.Row.FindControl("hdfTotalTaxFooterSplit") as HiddenField;
                            if (FinCrDrCusTrxMpgList != null && FinCrDrCusTrxMpgList.Count > 0)
                            {
                                if (FinCrDrCusTrxMpgList.Count == 1)
                                {
                                    total = string.IsNullOrEmpty(lblDCSplitReceiveNow.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(lblDCSplitReceiveNow.Text.Trim().Replace(",", ""));
                                }
                                else
                                {
                                    total = 0;
                                }
                                //lblTotalPayNowFooterSplit.Text = string.Format("{0:c}", total < 0 ? 0 : total);
                                //hdfTotalPayNowFooterSplit.Value = total.ToString();
                                if (tempfinCrDrCusMpgList == null || tempfinCrDrCusMpgList.Count == 0)
                                {
                                    lblTotalPayNowFooterSplit.Text = total < 0 ? GetFormattedCurrency(0) : GetFormattedCurrency(total);//< 0 ? 0 : total);
                                    hdfTotalPayNowFooterSplit.Value = Math.Round(total).ToString();
                                }
                                else
                                {
                                    lblTotalPayNowFooterSplit.Text = GetFormattedCurrency(tempfinCrDrCusMpgList.Sum(mpg => mpg.CDS_NET_AMOUNT));
                                    hdfTotalPayNowFooterSplit.Value = tempfinCrDrCusMpgList.Sum(mpg => mpg.CDS_NET_AMOUNT).ToString();
                                }
                            }
                            else if (finCrDrCusMpgList != null && finCrDrCusMpgList.Count > 0)
                            {
                                //if (!isSplitChanged)
                                //{
                                if (finCrDrCusMpgList.Count == 1)
                                {
                                    total = string.IsNullOrEmpty(lblDCSplitReceiveNow.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(lblDCSplitReceiveNow.Text.Trim().Replace(",", ""));
                                }
                                else
                                {
                                    total = finCrDrCusMpgList.Sum(aa => aa.CDS_NET_AMOUNT);
                                }
                                //}
                                //else
                                //{
                                //    total = 0;
                                //}
                                //lblTotalPayNowFooterSplit.Text = string.Format("{0:c}", total < 0 ? 0 : total);
                                //hdfTotalPayNowFooterSplit.Value = total.ToString();
                                if (tempfinCrDrCusMpgList == null || tempfinCrDrCusMpgList.Count == 0)
                                {
                                    lblTotalPayNowFooterSplit.Text = total < 0 ? GetFormattedCurrency(0) : GetFormattedCurrency(total);//< 0 ? 0 : total);
                                    //lblTotalPayNowFooterSplit.Text = string.Format("{0:c}", total < 0 ? 0 : total);
                                    hdfTotalPayNowFooterSplit.Value = total.ToString();
                                }
                                else
                                {
                                    lblTotalPayNowFooterSplit.Text = GetFormattedCurrency(tempfinCrDrCusMpgList.Sum(mpg => mpg.CDS_NET_AMOUNT));// string.Format("{0:c}", tempfinCrDrCusMpgList.Sum(mpg => mpg.CDS_NET_AMOUNT));
                                    hdfTotalPayNowFooterSplit.Value = tempfinCrDrCusMpgList.Sum(mpg => mpg.CDS_NET_AMOUNT).ToString();
                                }

                            }

                            lblTotalTaxFooterSplit.Text = GetFormattedCurrency(grndTotalTaxSplit);
                            hdfTotalTaxFooterSplit.Value = grndTotalTaxSplit.ToString();


                        }
                    }
                }
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
        public string GetFormattedCurrencyWithComa(object number)
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
                hdfCrDrNumber.Value = string.Empty;
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
        /// 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private bool Isposted()
        {
            int ispostedJ = 0;
            foreach (GridViewRow grdrowinv in grdInvoiceList.Rows)
            {
                HiddenField hdfhasjournalized;
                hdfhasjournalized = (HiddenField)grdrowinv.FindControl("hdfhasjournalized");
                if (Convert.ToBoolean(hdfhasjournalized.Value) != true)
                {
                    ispostedJ = 1;
                }
            }
            if (ispostedJ == 1)
            {
                return false;
            }
            else
                return true;
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
            SOINVHEADER,
            INVOICETYPE,
            //Biju
            TAXTYPES,
            TAXPOPUPGRID,
            TAXPOPUPGRIDTEMP,
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
            DRCRCANCELCHECK
            ,
            FILEUPLOAD
                ,
            ADDITEM
                ,
            SELECTEDDOC
                , DRCRGET
        }
        /// <summary>
        ///Payment Mode Enum 
        /// </summary>
        public enum DrCrModeEnum
        {
            DEBIT = 1,
            CREDIT
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