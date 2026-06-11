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
using BusinessObject.PurchaseOrderManagement;
using BusinessObject.SaleOrder;
using ERPManager.Finance;
using System.Transactions;
using System.IO;
using BusinessLogic.PurchaseOrderManagement;
using BusinessLogic.CommonManagement;

namespace ERPSMS_v01.CrDrNote
{
    public partial class DebitCreditNote : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties

        #region Properties
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
        /// Invoice PK
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
        /// CrDr Mapping PK
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
            //get
            //{
            //    return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedInvoicesCrDr];
            //}
            //set
            //{
            //    Session[ERP.Utilities.SessionStrings.SelectedInvoicesCrDr] = value;
            //}

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
        /// To maintain keep selected Invoices
        /// </summary>
        private List<ERPData.FIN_INVOICE_VND_HDR> FinInvoiceVndHdrSelectedList
        {
            get
            {
                return (List<ERPData.FIN_INVOICE_VND_HDR>)Session[ERP.Utilities.SessionStrings.FinInvoiceVndHdrSelectedList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.FinInvoiceVndHdrSelectedList] = value;
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

        /// <summary>
        /// Pay now amount
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
        /// Tax Amount
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

        /// <summary>
        /// PaymentFlag
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
        #endregion
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        User currentUser;

        //page related Entity Object

        private ServiceUtility serviceUtilityObj;
        private FIN_INVOICE_VND_HDR finInvoiceVndHdrObj;
        private FIN_COA_MST finCoaMstObj;
        private FIN_CASH_BANK_MST finCashBankMstObj;
        private List<long> SelectedInvoiceCrDrList;

        private ADM_CONFIG_MST admConfigMstObj;
        private List<ADM_CONFIG_MST> admConfigMstList;
        private List<SPADM_APP_STATUS_CFG_GET_KV_Result> workflowStatusList;

        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;

        #region Fin Cr Dr
        private FIN_CRDR_NOTE_HDR finCrDrNoteHdrObj;
        private FIN_CRDR_NOTE_MPG finCrDrNoteMpgObj;

        #endregion

        //List for binding details to controls  
        #region Fin Cr Dr List
        private List<FIN_CRDR_NOTE_HDR> finCrDrNoteHdrList;
        private List<FIN_CRDR_NOTE_MPG> finCrDrNoteMpgList;
        private List<FIN_CRDR_NOTE_HDR> ObjfinCrDrNoteHdrList;

        #endregion

        private List<FIN_PAYMENT_VND_TRX_MPG> finPaymentVndTrxMpgList;
        private List<FIN_INVOICE_VND_HDR> finInvoiceVndHdrList;
        private List<FIN_COA_MST> finCoaMstList;
        private List<FIN_CASH_BANK_MST> finCashBankMstList;
        private List<FIN_INVOICE_CUS_HDR> finInvoiceCusHdrList;

        List<FIN_CRDR_NOTE_DTL> tempfinCrDrCusMpgList;
        private List<FIN_CRDR_NOTE_DTL> finCrDrCusMpgList;
        private List<FIN_INVOICE_VND_HDR> objInvoiceDetails;
        private FIN_INVOICE_VND_HDR objInvDetails;
        private List<FIN_INVOICE_VND_HDR> finInvoiceVndHdrListForPaymentSplit;
        private FIN_INVOICE_VND_HDR finCrDrVndHdrObjForPaymentSplit;
        private FIN_CRDR_NOTE_DTL finCrDrCusSoMpgObj;
        private List<FIN_INVOICE_VND_DTL> FinCrDrCusTrxMpgList;
        private List<FIN_CRDR_NOTE_TAX_DTL> finCrDrCusMpgTaxList;
        private FIN_INVOICE_VND_DTL FinCrDrCusTrxMpgObj;
        private List<FIN_INVOICE_VND_DTL> objInvoiceItemDetails;
        private FIN_INVOICE_VND_DTL objInvItemDetails;
        private FIN_INVOICE_VND_DTL finInvItemDtl;
        private List<FIN_INVOICE_VND_DTL> finInvItemList;
        private List<SOInvoiceTaxHdr> taxTempList;
        private List<SOInvoiceTaxHdr> taxList;

        //private List<FIN_PAYMENT_VND_ALCN_DTL> objDnsAllocationList;

        private FIN_PAYMENT_VND_CRDR_MPG finPaymentCRDRmpgObj;
        private List<FIN_PAYMENT_VND_CRDR_MPG> finPaymentCRDRmpgList;

        private List<long> selectedInvoiceList;
        private DataTable dtTaxSettings;
        //private DataTable dtBalAmountSplit;
        private List<CrdrAllocations> objCrdrAllocations;
        private long InvPk = 0;
        private long InvItemPk = 0;
        private long CrDrPk = 0;
        private string transactionNumber;
        private bool updateCrDr;
        private bool isSplitChanged = false;
        private bool isDrCrUsedInOtherTrns = false;

        private decimal grndTotalTaxSplit = 0;
        private double ExchageRate = 0;

        int JournalPK;
        int crDrInvoiceType;
        int creditDebitType;
        int PoDept = 0;
        private string refID;
        private string inboxFlag;

        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;
        private List<FIN_YEAR_MST> finYearMstList;
        private Dictionary<string, decimal> dicTempAmount = new Dictionary<string, decimal>();

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
                hdfDecimalFormat.Value = "#0.";
                int NoDecimalDigitsP2P = Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P].ToString()) : 2;
                for (int i = 0; i < NoDecimalDigitsP2P; i++)
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

                    CrDrSplitList = null;
                    Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                    Session[ERP.Utilities.SessionStrings.SelectedPos] = null;

                    txtSearchDateFrom.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfSearchDateFrom.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    txtSearchDateTo.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfSearchDateTo.Value = DateTime.Now.ToString();


                    //GetFieldValues(ControlsEnum.FINPERIOD);
                    //SetFieldValues(ControlsEnum.FINPERIOD);

                    hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();

                    if (Session[ERP.Utilities.SessionStrings.SelectedInvoicesCrDr] != null)
                    {
                        SelectedInvoicesCrDr = (List<long>)Session[ERP.Utilities.SessionStrings.SelectedInvoicesCrDr];
                        Session[ERP.Utilities.SessionStrings.SelectedInvoicesCrDr] = null;
                        EntryStatus = EntryStatus.NEWMODE;
                    }

                    GetFieldValues(ControlsEnum.CRDRTYPE);
                    SetFieldValues(ControlsEnum.CRDRTYPE);
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
                                //btnSave.Visible = false;                            
                            }
                            else
                            {
                                ucrWrkf.ViewType = 1;
                                //EntryStatus = EntryStatus.ENTRYMODE;
                            }
                            ////start
                            if (string.IsNullOrEmpty(pid) || pid.Equals("1") || pid.Equals("11"))
                            {
                                base.WkfRefID = ucrWrkf.RefID = int.Parse(refID);
                                CurrPK = GetApplicationID(ucrWrkf.RefID);
                                if (pid.Equals("11"))
                                    hdfIsCancelled.Value = "1";//For Showing Cancelled Stamp in Detail Page
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
                        FileDetailsList = null;
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
                            //SetUIEditView(commonActions);
                            Session[ERP.Utilities.SessionStrings.CrDrType] = ApplicationType.PI;
                            hdfCrDrNumber.Value = CurrPK.ToString();
                            // Get CrDrDetails
                            GetFieldValues(ControlsEnum.GETCRDRHDRBYPK);
                            GetFieldValues(ControlsEnum.DRCRMPGLIST);
                            GetUIValuesFromObject(ControlsEnum.DRCRHEADERENTRY);
                            SetFieldValues(ControlsEnum.SELECTEDPIINVOICES);
                            ModifiedDatePnl.Visible = true;

                            //SetUIEditView(commonActions);
                            //ModifiedDatePnl.Visible = true;
                            //GetFieldValues(ControlsEnum.POINVOICEDETAILS);
                            //finInvoiceVndHdrObj = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                            //finInvoiceVndHdrObj.IVH_PK = CurrPK;
                            //hdfIVHPK.Value = CurrPK.ToString();
                            //GetFieldValues(ControlsEnum.INVOICEHDR);
                            //GetUIValuesFromObject(ControlsEnum.POINVOICEDETAILS);
                            //SetFieldValues(ControlsEnum.POINVOICELIST);
                            GetFieldValues(ControlsEnum.FILEUPLOAD);
                            SetFieldValues(ControlsEnum.FILEUPLOAD);
                        }
                        else
                        {
                            Session[ERP.Utilities.SessionStrings.CrDrType] = ApplicationType.PI;
                            //Sets data key for the gird
                            string[] datakeyarray;
                            datakeyarray = new string[1];
                            datakeyarray[0] = Resources.DataFieldRes.CrDrPk;
                            grdCrDbHdr.DataKeyNames = datakeyarray;
                            if (SelectedInvoicesCrDr != null && SelectedInvoicesCrDr.Count > 0)
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
                                }
                                selectedInvoiceList = SelectedInvoicesCrDr;
                                GetFieldValues(ControlsEnum.SELECTEDPIINVOICES);
                                SetFieldValues(ControlsEnum.SELECTEDPIINVOICES);

                                lblDrCrNo.Text = "[NEW]";
                                txtDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
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
                        GetFieldValues(ControlsEnum.LINEITEMTAXSETTINGS);
                        SetFieldValues(ControlsEnum.LINEITEMTAXSETTINGS);
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
            POPaymentService poPaymentServiceClient;
            FinCrDrHdrNoteService finCrDrHdrNoteServiceClient;
            finCrDrHdrNoteServiceClient = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            FinTrxService finTrxServiceClient;
            finTrxServiceClient = null;

            AdmCompanyMstService admCompanyMstServiceClient;

            int Type = 0;
            int? Status = null;
            string invoiceNo = string.Empty;
            CommonService CommonServiceClient;
            CommonServiceClient = null;
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
                        serviceUtilityObj.FilterBy = string.Empty;
                        serviceUtilityObj.FilterValue = string.Empty;
                        finCrDrNoteHdrObj.CDH_BIZUNIT = currentUser.SBUID;
                        finCrDrNoteHdrObj.CDH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finCrDrNoteHdrObj.CDH_VENDOR = string.IsNullOrEmpty(hdfVendorID.Value) ? 0 : Convert.ToInt32(hdfVendorID.Value);
                        if (hdfVendorID.Value != "" && hdfVendorID.Value != "0")
                        {
                            Session[ERP.Utilities.SessionStrings.VendorPK] = hdfVendorID.Value;
                            Session[ERP.Utilities.SessionStrings.Vendor] = txtVendor.Text;
                        }
                        finCrDrNoteHdrObj.CDH_CUSTOMER = 0;
                        finCrDrNoteHdrObj.CDH_PK = GInvPk;
                        serviceUtilityObj.FilterDate = DateTime.MinValue;
                        serviceUtilityObj.FilterToDate = DateTime.MinValue;
                        finCrDrNoteHdrObj.CDH_CRTD_BY = currentUser.PKUser;
                        Type = 1;
                        Status = 3;
                        invoiceNo = txtInvoiceNo.Text.Trim() != null ? txtInvoiceNo.Text.Trim() : string.Empty;

                        finCrDrNoteHdrList = finCrDrHdrNoteServiceClient.GetCrDrNoteVndHdr(finCrDrNoteHdrObj, serviceUtilityObj, Type, invoiceNo, Status);
                        TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 : (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) : (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        break;
                    #endregion
                    #region Credit Debit Hdr List
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
                        finCrDrNoteHdrObj.CDH_BIZUNIT = currentUser.SBUID;
                        finCrDrNoteHdrObj.CDH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finCrDrNoteHdrObj.CDH_VENDOR = string.IsNullOrEmpty(hdfVendorID.Value) ? 0 : Convert.ToInt32(hdfVendorID.Value);
                        finCrDrNoteHdrObj.CDH_TYPE = Convert.ToInt32(ddlCreditDebitType.SelectedValue) > 0 ? Convert.ToByte(ddlCreditDebitType.SelectedValue) : (byte)0;
                        if (hdfVendorID.Value != "" && hdfVendorID.Value != "0")
                        {
                            Session[ERP.Utilities.SessionStrings.VendorPK] = hdfVendorID.Value;
                            Session[ERP.Utilities.SessionStrings.Vendor] = txtVendor.Text;
                        }
                        finCrDrNoteHdrObj.CDH_CUSTOMER = 0;
                        if (string.IsNullOrEmpty(txtCrDrNumber.Text.Trim()) || txtCrDrNumber.Text.Trim() == "Select/Type")
                        {
                            hdfCrDrNumber.Value = "0";
                        }
                        finCrDrNoteHdrObj.CDH_PK = string.IsNullOrEmpty(hdfCrDrNumber.Value) ? 0 : Convert.ToInt64(hdfCrDrNumber.Value);
                        serviceUtilityObj.FilterDate = string.IsNullOrEmpty(txtSearchDateFrom.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtSearchDateFrom.Text.Trim());
                        serviceUtilityObj.FilterToDate = string.IsNullOrEmpty(txtSearchDateTo.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtSearchDateTo.Text.Trim());
                        finCrDrNoteHdrObj.CDH_CRTD_BY = currentUser.PKUser;
                        Type = 1;
                        Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        invoiceNo = txtInvoiceNo.Text.Trim() != null ? txtInvoiceNo.Text.Trim() : string.Empty;
                        finCrDrNoteHdrObj.CDH_COMPANY = Convert.ToInt32(ddlCompanySrch.SelectedValue);
                        finCrDrNoteHdrList = finCrDrHdrNoteServiceClient.GetCrDrNoteVndHdr(finCrDrNoteHdrObj, serviceUtilityObj, Type, invoiceNo, Status);
                        TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 : (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) : (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        break;
                    #endregion
                    #region CR/DR hdr by pk
                    case ControlsEnum.GETCRDRHDRBYPK:
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        finCrDrNoteHdrList = finCrDrHdrNoteServiceClient.GetCrDrNoteHdr(CurrPK);
                        break;
                    #endregion
                    #region Credit Debit Hdr List
                    case ControlsEnum.DRCRHDRLISTJOURNAL:
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        finCrDrNoteHdrObj = CommonFunctions.Initilize<FIN_CRDR_NOTE_HDR>();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = 1;
                        serviceUtilityObj.PageSize = grdCrDbHdr.PageSize;
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.CrDrNo : SortBy;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;
                        serviceUtilityObj.FilterBy = string.Empty;
                        serviceUtilityObj.FilterValue = string.Empty;
                        finCrDrNoteHdrObj.CDH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finCrDrNoteHdrObj.CDH_VENDOR = string.IsNullOrEmpty(hdfVendorID.Value) ? 0 : Convert.ToInt32(hdfVendorID.Value);
                        finCrDrNoteHdrObj.CDH_CUSTOMER = 0;
                        finCrDrNoteHdrObj.CDH_CRTD_BY = currentUser.PKUser;
                        finCrDrNoteHdrObj.CDH_PK = string.IsNullOrEmpty(hdfCrDrNumber.Value) ? 0 : Convert.ToInt64(hdfCrDrNumber.Value);
                        serviceUtilityObj.FilterDate = string.IsNullOrEmpty(txtSearchDateFrom.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtSearchDateFrom.Text.Trim());
                        serviceUtilityObj.FilterToDate = string.IsNullOrEmpty(txtSearchDateTo.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtSearchDateTo.Text.Trim());
                        Type = 1;
                        invoiceNo = txtInvoiceNo.Text.Trim() != null ? txtInvoiceNo.Text.Trim() : string.Empty;
                        finCrDrNoteHdrList = finCrDrHdrNoteServiceClient.GetCrDrNoteVndHdr(finCrDrNoteHdrObj, serviceUtilityObj, Type, invoiceNo);
                        TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 : (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) : (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;

                        break;
                    #endregion
                    #region Invoice Hdr List
                    case ControlsEnum.SELECTEDPIINVOICES:
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(poPaymentServiceClient);
                        finInvoiceVndHdrList = poPaymentServiceClient.GetPaymentTrxMpg(selectedInvoiceList);
                        FinInvoiceVndHdrSelectedList = finInvoiceVndHdrList;
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
                    #region Generate Transaction Number
                    case ControlsEnum.DEBIT:
                        //Generate Transaction Number
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        int appSubTypeDN;
                        appSubTypeDN = (!string.IsNullOrEmpty(hdfInvoiceGroup.Value)
                            ? (Convert.ToByte(hdfInvoiceGroup.Value) == (byte)POInvoiceGroup.Services
                                 ? (int)AppSubTypeCNPurchase.NONSTOCK
                                    : (!string.IsNullOrEmpty(hdfPOType.Value)
                                        ? Convert.ToInt16(hdfPOType.Value) == (Int16)POItemType.Others
                                            ? (int)AppSubTypeCNPurchase.NONSTOCK
                                            : (int)AppSubTypeCNPurchase.STOCK
                                        : 0))
                            : 0);
                        appSubTypeDN = appSubTypeDN > 0 ? appSubTypeDN : hdfInvoiceCategory.Value == "1" ? (int)AppSubTypeCNPurchase.STOCK : (int)AppSubTypeCNPurchase.NONSTOCK;
                        //appSubTypeDN = hdfInvoiceCategory.Value == "1" ? (int)AppSubTypeCNPurchase.STOCK : (int)AppSubTypeCNPurchase.NONSTOCK;
                        DateTime drDate = string.IsNullOrEmpty(txtDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtDate.Text.Trim());
                        if (Session[ERP.Utilities.SessionStrings.CrDrType].ToString() == ApplicationType.PI)
                        {
                            transactionNumber = poPaymentServiceClient.GetPaymentNo(ApplicationType.DN, appSubTypeDN, currentUser.CurrentDeptPK,
                                drDate, currentUser.PKUser, updateCrDr, 0, Convert.ToInt32(ddlCompany.SelectedItem.Value));
                        }
                        else if (Session[ERP.Utilities.SessionStrings.CrDrType].ToString() == ApplicationType.SI)
                        {
                            transactionNumber = poPaymentServiceClient.GetPaymentNo(ApplicationType.DN, appSubTypeDN, currentUser.CurrentDeptPK,
                                drDate, currentUser.PKUser, updateCrDr, 0, Convert.ToInt32(ddlCompany.SelectedItem.Value));
                        }
                        hdfCrDrTrxNo.Value = transactionNumber;
                        break;
                    #endregion
                    #region Generate Transaction Number
                    case ControlsEnum.CREDIT:
                        //Generate Transaction Number
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        int appSubTypeCN;
                        appSubTypeCN = (!string.IsNullOrEmpty(hdfInvoiceGroup.Value)
                            ? (Convert.ToByte(hdfInvoiceGroup.Value) == (byte)POInvoiceGroup.Services
                                 ? (int)AppSubTypeCNPurchase.NONSTOCK
                                    : (!string.IsNullOrEmpty(hdfPOType.Value)
                                        ? Convert.ToInt16(hdfPOType.Value) == (Int16)POItemType.Others
                                            ? (int)AppSubTypeCNPurchase.NONSTOCK
                                            : (int)AppSubTypeCNPurchase.STOCK
                                        : 0))
                            : 0);
                        appSubTypeCN = appSubTypeCN > 0 ? appSubTypeCN : hdfInvoiceCategory.Value == "1" ? (int)AppSubTypeCNPurchase.STOCK : (int)AppSubTypeCNPurchase.NONSTOCK;
                        //int appSubTypeCN = hdfInvoiceCategory.Value == "1" ? (int)AppSubTypeCNPurchase.STOCK : (int)AppSubTypeCNPurchase.NONSTOCK;
                        DateTime crDate = string.IsNullOrEmpty(txtDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtDate.Text.Trim());
                        if (Session[ERP.Utilities.SessionStrings.CrDrType].ToString() == ApplicationType.PI)
                        {
                            transactionNumber = poPaymentServiceClient.GetPaymentNo(ApplicationType.CN, appSubTypeCN, currentUser.CurrentDeptPK,
                            crDate, currentUser.PKUser, updateCrDr, 0, Convert.ToInt32(ddlCompany.SelectedItem.Value));
                        }
                        else if (Session[ERP.Utilities.SessionStrings.CrDrType].ToString() == ApplicationType.SI)
                        {
                            transactionNumber = poPaymentServiceClient.GetPaymentNo(ApplicationType.CN, appSubTypeCN, currentUser.CurrentDeptPK,
                            crDate, currentUser.PKUser, updateCrDr, 0, Convert.ToInt32(ddlCompany.SelectedItem.Value));
                        }
                        hdfCrDrTrxNo.Value = transactionNumber;
                        break;
                    #endregion
                    #region Get Exchange rate
                    case ControlsEnum.EXCHANGERATE:
                        //Get Exchange rate
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        //hdfExchangeCurr.Value = poPaymentServiceClient.GetConversionFactor(
                        //                                     Convert.ToInt32(hdfInvoiceCurr.Value), currentUser.BaseCurrency,
                        //                                     finCrDrNoteHdrObj.CDH_DATE, currentUser.SBUID).ToString();
                        ExchageRate = poPaymentServiceClient.GetConversionFactor(
                           string.IsNullOrEmpty(hdfInvoiceCurr.Value) ? 0 : Convert.ToInt32(hdfInvoiceCurr.Value), currentUser.BaseCurrency,
                                                            Convert.ToDateTime(string.IsNullOrEmpty(txtDate.Text) ? DateTime.Now.ToShortDateString() : txtDate.Text),
                                                            currentUser.SBUID);
                        if (ExchageRate > 0)
                        {
                            hdfExchangeCurr.Value = ExchageRate.ToString();
                            txtExchangeRate.Text = ExchageRate.ToString();
                        }

                        break;
                    #endregion
                    #region FIN HEADER
                    case ControlsEnum.FINHEADER:
                        //Get Fin Trx List
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
                        workflowStatusList = CommonServiceClient.GetWorkFlowStatus(ApplicationType.CN, (byte)AppSubTypeDebitCreditNote.VENDOR, Convert.ToByte(CommonConstants.ACTIVE));
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

                    #region Invoice Hdr By PK
                    case ControlsEnum.INVOICEDETAILS:
                        SalesInvoiceClient = new SalesInvoiceService();
                        SalesInvoiceClient = CommonFunctions.InitiateClient(SalesInvoiceClient);
                        objInvDetails = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                        objInvDetails.IVH_PK = InvPk;
                        objInvDetails.IVH_ACTIVE = 1;
                        objInvoiceDetails = SalesInvoiceClient.GetInvoiceVndHdrByPK(objInvDetails);
                        break;
                    #endregion

                    #region Invoice Item Details By Pk
                    case ControlsEnum.INVOICEITEMDETAILS:
                        SalesInvoiceClient = new SalesInvoiceService();
                        SalesInvoiceClient = CommonFunctions.InitiateClient(SalesInvoiceClient);
                        objInvItemDetails = CommonFunctions.Initilize<FIN_INVOICE_VND_DTL>();
                        objInvItemDetails.VID_PK = InvItemPk;
                        objInvoiceItemDetails = SalesInvoiceClient.GetInvoiceVndDtlByPK(objInvItemDetails);
                        break;
                    #endregion

                    //CrDr Split
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
                        FinCrDrCusTrxMpgObj = CommonFunctions.Initilize<FIN_INVOICE_VND_DTL>();
                        FinCrDrCusTrxMpgObj.VID_INVOICE_HDR = InvoicePK;//@@
                        FinCrDrCusTrxMpgList = finCrDrHdrNoteServiceClient.GetInvoiceVndTrxMpg(FinCrDrCusTrxMpgObj);
                        break;
                    #endregion

                    #region CRDRVNDHDR
                    case ControlsEnum.CRDRVNDHDR:
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        finCrDrVndHdrObjForPaymentSplit = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                        finCrDrVndHdrObjForPaymentSplit.IVH_PK = InvoicePK;
                        finCrDrVndHdrObjForPaymentSplit.IVH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finInvoiceVndHdrListForPaymentSplit = finCrDrHdrNoteServiceClient.GetInvoiceVndHdrByPK(finCrDrVndHdrObjForPaymentSplit);
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
                        FinCrDrCusTrxMpgObj = CommonFunctions.Initilize<FIN_INVOICE_VND_DTL>();
                        FinCrDrCusTrxMpgObj.VID_INVOICE_HDR = InvoicePK;
                        finInvItemList = finCrDrHdrNoteServiceClient.GetInvoiceVndTrxMpg(FinCrDrCusTrxMpgObj);
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
                    case ControlsEnum.LINEITEMTAXSETTINGS:
                        isLineItemTaxEnabled = false;
                        dtTaxSettings = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("PURCHASE ITEM WISE SETTINGS", string.Empty, currentUser.SBUID);
                        break;
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

                    #region ALLOCATED IN PAYMENT
                    //case ControlsEnum.ALLOCATEDINPAYMENT:
                    //    finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                    //    finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);                       
                    //    objDnsAllocationList = finCrDrHdrNoteServiceClient.GetDnsAllocationInPayment(CurrPK);
                    //    break;
                    #endregion

                    #region BALANCE AMOUNT SPLIT
                    case ControlsEnum.BALANCEAMOUNTSPLIT:
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        objCrdrAllocations = finCrDrHdrNoteServiceClient.GetBalanceAmountSplit(CurrPK);
                        break;
                    #endregion

                    #region CANCEL_DR/CR_CHECK
                    case ControlsEnum.DRCRCANCELCHECK:
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        finPaymentCRDRmpgObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_CRDR_MPG>();
                        finPaymentCRDRmpgObj.PNM_CRDR_HDR = CurrPK;
                        finPaymentCRDRmpgList = finCrDrHdrNoteServiceClient.CheckPaymentDRCRforCancel(finPaymentCRDRmpgObj);
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
                        }


                        break;
                    #endregion
                    #region CR/DR hdr Details
                    case ControlsEnum.GETCRDRHDRDETAILS:
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        ObjfinCrDrNoteHdrList = finCrDrHdrNoteServiceClient.GetCrDrNoteHdr(CurrPK);
                        break;
                    #endregion
                    #region PO DEPT
                    case ControlsEnum.PODEPT:
                        PoDept = PurchaseOrderGenerateBL.GetPODepartment(CrDrPk);
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
                finCrDrHdrNoteServiceClient = null;
                finTrxServiceClient = null;
                admCompanyMstServiceClient = null;
                CommonServiceClient = null;
                SalesInvoiceClient = null;
                poPaymentServiceClient = null;
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
                    case ControlsEnum.DRCRGET:
                        GetUIValuesFromObject(controlType);
                        break;
                    #region Company
                    case ControlsEnum.COMPANY:
                        //Bind Company List in DDL
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    #endregion
                    #region DrCrHdrList
                    case ControlsEnum.DRCRHDRLIST:
                        BindGrid(ControlsEnum.DRCRHDRLIST);
                        break;
                    #endregion
                    #region Selected PI Invoices
                    case ControlsEnum.SELECTEDPIINVOICES:
                        BindGrid(controlType);
                        break;
                    #endregion
                    #region Credit Debit Type
                    case ControlsEnum.CRDRTYPE:
                        BindDropDown(ControlsEnum.CRDRTYPE);
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

                    case ControlsEnum.CRDRSPLITLIST:
                        BindGrid(ControlsEnum.CRDRSPLITLIST);
                        break;
                    case ControlsEnum.TAXMYR:
                        BindGrid(ControlsEnum.TAXMYR);
                        break;
                    case ControlsEnum.TAXSPLITUP:
                        BindGrid(ControlsEnum.TAXSPLITUP);
                        break;
                    case ControlsEnum.TAXSPLITUPLINEITEMWISE:
                        BindGrid(ControlsEnum.TAXSPLITUPLINEITEMWISE);
                        break;
                    #region LINE ITEM TAX SETTINGS
                    case ControlsEnum.LINEITEMTAXSETTINGS:
                        GetUIValuesFromObject(ControlsEnum.LINEITEMTAXSETTINGS);
                        break;
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
        #endregion
        #region Helper Methods
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            FinCrDrHdrNoteService finCrDrHdrNoteServiceClient;
            finCrDrHdrNoteServiceClient = null;
            try
            {
                Object retObject;
                retObject = null;
                int rowID;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                HiddenField hdfCrDbMpgPK;
                hdfCrDbMpgPK = null;
                HiddenField hdfInvoicePK;
                hdfInvoicePK = null;
                TextBox txtAmount;
                Label lblTotalTax;
                LinkButton lbnTotalTax;
                bool bIsChecked = false;

                HiddenField hdfDCSplitPK = null;
                HiddenField hdfInvCusDtlPK = null;
                TextBox txtSumSplit;
                TextBox txtQtySplit;
                TextBox txtRateSplit;



                switch (controlType)
                {
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
                        if (Session[ERP.Utilities.SessionStrings.CrDrType].ToString() == ApplicationType.PI)
                        {
                            finCrDrNoteHdrObj.CDH_VENDOR = string.IsNullOrEmpty(hdfVendorPK.Value) ? 0 : Convert.ToInt32(hdfVendorPK.Value);
                            finCrDrNoteHdrObj.CDH_VND_CUS_ACCOUNT = string.IsNullOrEmpty(hdfVendorAccountNo.Value) ? null : ERP.Utilities.CommonFunctions.NullableInt(hdfVendorAccountNo.Value);//Convert.ToInt32(ddlAccountNo.SelectedValue);
                            finCrDrNoteHdrObj.CDH_CUSTOMER = null;
                        }
                        else if (Session[ERP.Utilities.SessionStrings.CrDrType].ToString() == ApplicationType.SI)
                        {
                            finCrDrNoteHdrObj.CDH_VENDOR = null;
                            finCrDrNoteHdrObj.CDH_CUSTOMER = string.IsNullOrEmpty(hdfVendorPK.Value) ? 0 : Convert.ToInt32(hdfVendorPK.Value);
                            finCrDrNoteHdrObj.CDH_VND_CUS_ACCOUNT = string.IsNullOrEmpty(hdfVendorAccountNo.Value) ? null : ERP.Utilities.CommonFunctions.NullableInt(hdfVendorAccountNo.Value);//Convert.ToInt32(ddlAccountNo.SelectedValue);
                        }

                        finCrDrNoteHdrObj.CDH_TYPE = Convert.ToByte(ddlMode.SelectedValue);
                        finCrDrNoteHdrObj.CDH_CURRENCY = string.IsNullOrEmpty(hdfInvoiceCurr.Value) ? 1 : Convert.ToInt32(hdfInvoiceCurr.Value);
                        if (!string.IsNullOrEmpty(hdfShipCharge.Value))
                            finCrDrNoteHdrObj.CDH_SHIP_CHARGE = Convert.ToDecimal(hdfShipCharge.Value);
                        ////finCrDrNoteHdrObj.CDH_AMOUNT_TC = Convert.ToDecimal(txtPaidAmount.Text.Trim());
                        finCrDrNoteHdrObj.CDH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                        finCrDrNoteHdrObj.CDH_BASE_CURR = currentUser.BaseCurrency;
                        //GetFieldValues(ControlsEnum.EXCHANGERATE);
                        finCrDrNoteHdrObj.CDH_EXCHG_RATE = string.IsNullOrEmpty(hdfExchangeCurr.Value) ? 1 : Convert.ToDouble(hdfExchangeCurr.Value);
                        ////finCrDrNoteHdrObj.CDH_AMOUNT_BC = Convert.ToDecimal(txtPaidAmount.Text.Trim()) * Convert.ToDecimal(finCrDrNoteHdrObj.CDH_EXCHG_RATE);
                        finCrDrNoteHdrObj.CDH_STATUS = 0;
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
                        finCrDrNoteHdrObj.CDH_IMP_DECL_NO = HttpUtility.HtmlDecode(txtDeclarationNo.Text);
                        if (String.IsNullOrEmpty(txtInstrumentDate.Text.Trim()))
                            finCrDrNoteHdrObj.CDH_REF_DATE = null;
                        else
                            finCrDrNoteHdrObj.CDH_REF_DATE = Convert.ToDateTime(txtInstrumentDate.Text.Trim());
                        finCrDrNoteHdrObj.CDH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);

                        finCrDrNoteHdrObj.CDH_IS_AFFECT_STK = chkAffectStock.Checked ? (byte)1 : (byte)0;

                        if (finCrDrNoteMpgList != null && finCrDrNoteMpgList.Count > 0)
                        {
                            finCrDrNoteHdrObj.CDH_TAX_AMOUNT = Math.Round(finCrDrNoteMpgList.Sum(c => c.CDM_TAX_AMOUNT), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            finCrDrNoteHdrObj.CDH_AMOUNT_TC = Math.Round(finCrDrNoteMpgList.Sum(c => c.CDM_AMOUNT), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            finCrDrNoteHdrObj.CDH_AMOUNT_BC = Math.Round(finCrDrNoteMpgList.Sum(c => c.CDM_AMOUNT) * Convert.ToDecimal(finCrDrNoteHdrObj.CDH_EXCHG_RATE), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                            finCrDrNoteHdrObj.FIN_CRDR_NOTE_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_CRDR_NOTE_MPG>();
                            finCrDrNoteMpgList.ForEach(dtl => finCrDrNoteHdrObj.FIN_CRDR_NOTE_MPG.Add(dtl));
                        }
                        //if (finCrDrNoteMpgList != null && finCrDrNoteMpgList.Count > 0)
                        //{
                        //    finCrDrNoteHdrObj.FIN_CRDR_NOTE_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_CRDR_NOTE_MPG>();
                        //    finCrDrNoteMpgList.ForEach(dtl =>
                        //    {
                        //        finCrDrNoteHdrObj.FIN_CRDR_NOTE_MPG.Add(dtl);
                        //        finCrDrCusMpgList = CrDrSplitList.Where(mpg => mpg.FIN_CRDR_NOTE_MPG.CDM_INVOICE_VND_HDR == dtl.CDM_INVOICE_VND_HDR).ToList();
                        //        if (finCrDrCusMpgList.Count() > 0)
                        //        {
                        //            finCrDrCusMpgList.ForEach(mpg =>
                        //            {
                        //                mpg.FIN_CRDR_NOTE_MPG = null;
                        //                dtl.FIN_CRDR_NOTE_DTL.Add(mpg);
                        //            });
                        //        }
                        //    });

                        //}
                        retObject = finCrDrNoteHdrObj;
                        break;
                    #endregion
                    #region Credit Debit Maping
                    case ControlsEnum.DRCRMPGENTRY:
                        rowID = 0;
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
                            finCrDrNoteMpgObj.CDM_INVOICE_VND_HDR = hdfInvoicePK == null ? 0 : Convert.ToInt32(hdfInvoicePK.Value);
                            finCrDrNoteMpgObj.CDM_INVOICE_CUS_HDR = null;

                            TextBox txtOtherCharges = (TextBox)grdInvoiceList.Rows[rowID].FindControl("txtOtherCharges");
                            finCrDrNoteMpgObj.CDM_OTHER_CHARGE = txtOtherCharges == null ? 0 : txtOtherCharges.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtOtherCharges.Text.Trim());
                            hdfShipchargeNew.Value = finCrDrNoteMpgObj.CDM_OTHER_CHARGE.ToString();
                            //if (Session[ERP.Utilities.SessionStrings.CrDrType].ToString() == ApplicationType.PI)
                            //{
                            //    finCrDrNoteMpgObj.CDM_INVOICE_VND_HDR = hdfInvoicePK == null ? 0 : Convert.ToInt32(hdfInvoicePK.Value);
                            //    finCrDrNoteMpgObj.CDM_INVOICE_CUS_HDR = null;
                            //}
                            //else if (Session[ERP.Utilities.SessionStrings.CrDrType].ToString() == ApplicationType.SI)
                            //{
                            //    finCrDrNoteMpgObj.CDM_INVOICE_CUS_HDR = hdfInvoicePK == null ? 0 : Convert.ToInt32(hdfInvoicePK.Value);
                            //    finCrDrNoteMpgObj.CDM_INVOICE_VND_HDR = null;
                            //}
                            txtAmount = (TextBox)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("txtNoteFor").ToString());
                            decimal.TryParse(txtAmount.Text, out RaiseNote);
                            if (RaiseNote == 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_RaiseNote").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                break;
                            }
                            finCrDrNoteMpgObj.CDM_AMOUNT = Convert.ToDecimal(txtAmount.Text);
                            finCrDrNoteMpgObj.CDM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);

                            HiddenField hdfTotalTax = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTotalTax");

                            HiddenField hdfTaxAmt = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTaxAmt");
                            hdftax = hdfTaxAmt.Value != string.Empty ? Convert.ToDecimal(hdfTaxAmt.Value) : 0;
                            HiddenField hdfTotalAmt = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTotalAmt");
                            hdftotalamt = hdfTotalAmt.Value != string.Empty ? Convert.ToDecimal(hdfTotalAmt.Value) : 0;
                            taxpercentage = hdftax / (hdftotalamt == 0 ? 1 : hdftotalamt);
                            basevalue = (Convert.ToDecimal(txtAmount.Text)) / (1 + taxpercentage);
                            ttaxamt = (Convert.ToDecimal(txtAmount.Text) - basevalue);
                            lbnTotalTax = (LinkButton)grdInvoiceList.Rows[rowID].FindControl("lbnTotalTax");
                            //finCrDrNoteMpgObj.CDM_TAX_AMOUNT = Math.Round(Convert.ToDecimal(ttaxamt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                            finCrDrNoteMpgObj.CDM_TAX_AMOUNT = Math.Round(Convert.ToDecimal(hdfTotalTax.Value), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                            HiddenField hdfItemIncluded = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfItemIncluded");
                            int.TryParse(hdfItemIncluded.Value, out ItemIncluded);
                            if (ItemIncluded == 1)
                            {
                                finCrDrNoteMpgObj.CDM_AMOUNT = finCrDrNoteMpgObj.CDM_AMOUNT + finCrDrNoteMpgObj.CDM_TAX_AMOUNT;
                            }

                            if (finCrDrNoteMpgObj.CDM_AMOUNT > 0)
                            {
                                //adding detail tax
                                if (finCrDrNoteMpgObj.CDM_TAX_AMOUNT > 0)
                                {
                                    InvPk = Convert.ToInt64(hdfInvoicePK.Value);
                                    GetFieldValues(ControlsEnum.INVOICEDETAILS);
                                    if (objInvoiceDetails != null && objInvoiceDetails.Count > 0)
                                    {
                                        decimal InvAmount = objInvoiceDetails[0].IVH_AMOUNT_TC;
                                        decimal TotalTaxPercentage = 1;
                                        //decimal ApplicableHdrTax = 0;                                       
                                        //decimal InvTotalTax = 0;
                                        //decimal LineItemTax = 0;
                                        List<FIN_INVOICE_VND_TAX_HDR> objInvoiceTaxList = objInvoiceDetails[0].FIN_INVOICE_VND_TAX_HDR.Where(inv => inv.VTH_TAX_CATEGORY == (byte)TaxType.Tax).ToList();

                                        //if (objInvoiceDetails[0].FIN_INVOICE_VND_DTL != null && objInvoiceDetails[0].FIN_INVOICE_VND_DTL.Count > 0)
                                        //    {
                                        //        LineItemTax=objInvoiceDetails[0].FIN_INVOICE_VND_DTL.Sum(tx => tx.VID_TAX);
                                        //    }
                                        //    InvTotalTax=objInvoiceDetails[0].IVH_TAX_TC+LineItemTax;
                                        //    ApplicableHdrTax = (objInvoiceDetails[0].IVH_TAX_TC / (InvTotalTax == 0 ? 1 : InvTotalTax)) * hdftax;
                                        List<FIN_INVOICE_VND_TAX_HDR> TaxList = objInvoiceDetails[0].FIN_INVOICE_VND_TAX_HDR.Where(txx => txx.VTH_TAX_CATEGORY == (byte)TaxType.Tax).ToList();
                                        if (objInvoiceTaxList != null && objInvoiceTaxList.Count > 0)
                                        {
                                            TotalTaxPercentage = objInvoiceTaxList.Sum(tx => (tx.VTH_TAX_AMT * 100) / (InvAmount == 0 ? 1 : InvAmount));
                                            foreach (FIN_INVOICE_VND_TAX_HDR invtaxhdr in TaxList)
                                            {
                                                FIN_CRDR_NOTE_TAX_DTL ObjTaxDtl = new FIN_CRDR_NOTE_TAX_DTL();
                                                ObjTaxDtl = CommonFunctions.Initilize<FIN_CRDR_NOTE_TAX_DTL>();
                                                decimal IndividualPercentage = (invtaxhdr.VTH_TAX_AMT * 100) / (InvAmount == 0 ? 1 : InvAmount);
                                                decimal TaxAmnt = (finCrDrNoteMpgObj.CDM_TAX_AMOUNT * IndividualPercentage) / TotalTaxPercentage;
                                                //decimal TaxAmnt = (ApplicableHdrTax * IndividualPercentage) / TotalTaxPercentage;
                                                double Amount = Math.Round(Convert.ToDouble(TaxAmnt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                                ObjTaxDtl.NTD_AMOUNT = Convert.ToDecimal(Amount);
                                                ObjTaxDtl.NTD_PK = 0;
                                                ObjTaxDtl.NTD_TAX = invtaxhdr.VTH_TAX;
                                                finCrDrNoteMpgObj.FIN_CRDR_NOTE_TAX_DTL.Add(ObjTaxDtl);
                                            }
                                        }
                                    }
                                }

                                finCrDrCusMpgList = CrDrSplitList.Where(dtl => dtl.FIN_CRDR_NOTE_MPG.CDM_INVOICE_VND_HDR == Convert.ToInt64(hdfInvoicePK.Value)).ToList();
                                finCrDrCusMpgList.ForEach(dtl =>
                                {
                                    finCrDrNoteMpgObj.FIN_CRDR_NOTE_DTL.Add(dtl);
                                });
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
                            ////

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

                                Session[ERP.Utilities.SessionStrings.SubTypeDDr] = null;
                                Session[ERP.Utilities.SessionStrings.SubTypeCCr] = null;
                                Session[ERP.Utilities.SessionStrings.SubTypeCr] = null;
                                Session[ERP.Utilities.SessionStrings.SubTypeDr] = null;
                                Session[ERP.Utilities.SessionStrings.JournalMode] = null;
                                Session[ERP.Utilities.SessionStrings.JournalMode] = null;

                                //Journalize New sessions start
                                Session[ERP.Utilities.SessionStrings.DrControls] = null;
                                Session[ERP.Utilities.SessionStrings.CrControls] = null;
                                Session[ERP.Utilities.SessionStrings.RemovedControls] = null;
                                Session[ERP.Utilities.SessionStrings.AccountType] = null;
                                Session[ERP.Utilities.SessionStrings.ControlInfo] = null;
                                Session[ERP.Utilities.SessionStrings.FinTrxPk] = null;
                                //Journalize New sessions End

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
                                SetCancelRef((int)CurrPK);
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
                                int appSubType = (!string.IsNullOrEmpty(hdfInvoiceGroup.Value)
                                    ? (Convert.ToByte(hdfInvoiceGroup.Value) == (byte)POInvoiceGroup.Services
                                         ? (int)AppSubTypeCNPurchase.NONSTOCK
                                            : (!string.IsNullOrEmpty(hdfPOType.Value)
                                                ? Convert.ToInt16(hdfPOType.Value) == (Int16)POItemType.Others
                                                    ? (int)AppSubTypeCNPurchase.NONSTOCK
                                                    : (int)AppSubTypeCNPurchase.STOCK
                                                : 0))
                                    : 0);
                                appSubType = appSubType > 0 ? appSubType : hdfInvoiceCategory.Value == "1" ? (int)AppSubTypeCNPurchase.STOCK : (int)AppSubTypeCNPurchase.NONSTOCK;
                                ucrJournalize.TypeForNumberGenaration = appSubType.ToString();// hdfInvoiceCategory.Value == "1" ? ((int)AppSubTypeCNPurchase.STOCK).ToString() : ((int)AppSubTypeCNPurchase.NONSTOCK).ToString();
                                ucrJournalize.CallUserControl();

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
                            lblDrCrNo.Text = hdfCrDrTrxNo.Value;
                        }
                        finCrDrNoteHdrObj.CDH_NO = lblDrCrNo.Text;
                        finCrDrNoteHdrObj.CDH_DATE = String.IsNullOrEmpty(txtDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtDate.Text.Trim());
                        if (Session[ERP.Utilities.SessionStrings.CrDrType].ToString() == ApplicationType.PI)
                        {
                            finCrDrNoteHdrObj.CDH_VENDOR = string.IsNullOrEmpty(hdfVendorPK.Value) ? 0 : Convert.ToInt32(hdfVendorPK.Value);
                            finCrDrNoteHdrObj.CDH_VND_CUS_ACCOUNT = string.IsNullOrEmpty(hdfVendorAccountNo.Value) ? null : ERP.Utilities.CommonFunctions.NullableInt(hdfVendorAccountNo.Value);//Convert.ToInt32(ddlAccountNo.SelectedValue);
                            finCrDrNoteHdrObj.CDH_CUSTOMER = null;
                        }
                        else if (Session[ERP.Utilities.SessionStrings.CrDrType].ToString() == ApplicationType.SI)
                        {
                            finCrDrNoteHdrObj.CDH_VENDOR = null;
                            finCrDrNoteHdrObj.CDH_CUSTOMER = string.IsNullOrEmpty(hdfVendorPK.Value) ? 0 : Convert.ToInt32(hdfVendorPK.Value);
                            finCrDrNoteHdrObj.CDH_VND_CUS_ACCOUNT = string.IsNullOrEmpty(hdfVendorAccountNo.Value) ? null : ERP.Utilities.CommonFunctions.NullableInt(hdfVendorAccountNo.Value);//Convert.ToInt32(ddlAccountNo.SelectedValue);
                        }
                        finCrDrNoteHdrObj.CDH_TYPE = Convert.ToByte(ddlMode.SelectedValue);
                        finCrDrNoteHdrObj.CDH_CURRENCY = string.IsNullOrEmpty(hdfInvoiceCurr.Value) ? 1 : Convert.ToInt32(hdfInvoiceCurr.Value);
                        finCrDrNoteHdrObj.CDH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);

                        //finCrDrNoteHdrObj.CDH_AMOUNT_TC = Convert.ToDecimal(txtPaidAmount.Text.Trim());
                        finCrDrNoteHdrObj.CDH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                        finCrDrNoteHdrObj.CDH_BASE_CURR = currentUser.BaseCurrency;
                        //  GetFieldValues(ControlsEnum.EXCHANGERATE);
                        finCrDrNoteHdrObj.CDH_EXCHG_RATE = string.IsNullOrEmpty(hdfExchangeCurr.Value) ? 1 : Convert.ToDouble(hdfExchangeCurr.Value);
                        //finCrDrNoteHdrObj.CDH_AMOUNT_BC = Convert.ToDecimal(txtPaidAmount.Text.Trim()) * Convert.ToDecimal(finCrDrNoteHdrObj.CDH_EXCHG_RATE);
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
                        finCrDrNoteHdrObj.CDH_IMP_DECL_NO = HttpUtility.HtmlDecode(txtDeclarationNo.Text);
                        if (String.IsNullOrEmpty(txtInstrumentDate.Text.Trim()))
                            finCrDrNoteHdrObj.CDH_REF_DATE = null;
                        else
                            finCrDrNoteHdrObj.CDH_REF_DATE = Convert.ToDateTime(txtInstrumentDate.Text.Trim());

                        finCrDrNoteHdrObj.CDH_IS_AFFECT_STK = chkAffectStock.Checked ? (byte)1 : (byte)0;
                        //if (finCrDrNoteMpgList != null && finCrDrNoteMpgList.Count > 0)
                        //{
                        //    finCrDrNoteHdrObj.CDH_TAX_AMOUNT = Math.Round(finCrDrNoteMpgList.Sum(c => c.CDM_TAX_AMOUNT), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        //}

                        if (finCrDrNoteMpgList != null && finCrDrNoteMpgList.Count > 0)
                        {
                            finCrDrNoteHdrObj.CDH_TAX_AMOUNT = Math.Round(finCrDrNoteMpgList.Sum(c => c.CDM_TAX_AMOUNT), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            finCrDrNoteHdrObj.CDH_AMOUNT_TC = Math.Round(finCrDrNoteMpgList.Sum(c => c.CDM_AMOUNT), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            finCrDrNoteHdrObj.CDH_AMOUNT_BC = Math.Round(finCrDrNoteMpgList.Sum(c => c.CDM_AMOUNT) * Convert.ToDecimal(finCrDrNoteHdrObj.CDH_EXCHG_RATE), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                            finCrDrNoteHdrObj.FIN_CRDR_NOTE_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_CRDR_NOTE_MPG>();
                            finCrDrNoteMpgList.ForEach(dtl => finCrDrNoteHdrObj.FIN_CRDR_NOTE_MPG.Add(dtl));
                        }
                        //if (finCrDrNoteMpgList != null && finCrDrNoteMpgList.Count > 0)
                        //{
                        //    finCrDrNoteHdrObj.FIN_CRDR_NOTE_MPG = new System.Data.Objects.DataClasses.EntityCollection<FIN_CRDR_NOTE_MPG>();
                        //    finCrDrNoteMpgList.ForEach(dtl =>
                        //    {
                        //        finCrDrNoteHdrObj.FIN_CRDR_NOTE_MPG.Add(dtl);
                        //        finCrDrCusMpgList = CrDrSplitList.Where(mpg => mpg.FIN_CRDR_NOTE_MPG.CDM_INVOICE_VND_HDR == dtl.CDM_INVOICE_VND_HDR).ToList();
                        //        if (finCrDrCusMpgList.Count() > 0)
                        //        {
                        //            finCrDrCusMpgList.ForEach(mpg =>
                        //            {
                        //                mpg.FIN_CRDR_NOTE_MPG = null;
                        //                dtl.FIN_CRDR_NOTE_DTL.Add(mpg);
                        //            });
                        //        }
                        //    });

                        //}
                        retObject = finCrDrNoteHdrObj;
                        break;
                    #endregion

                    #region CRDR SPLIT LIST
                    case ControlsEnum.CRDRSPLITLIST:
                        rowID = 0;
                        TextBox txtDiscountSplit;
                        finCrDrCusMpgList = new List<FIN_CRDR_NOTE_DTL>();
                        foreach (GridViewRow grdrow in grdDCSplit.Rows)//
                        {
                            finCrDrCusSoMpgObj = CommonFunctions.Initilize<FIN_CRDR_NOTE_DTL>();
                            hdfDCSplitPK = (HiddenField)grdDCSplit.Rows[rowID].FindControl("hdfDCSplitPK");
                            finCrDrCusSoMpgObj.CDS_PK = hdfDCSplitPK == null ? 0 : Convert.ToInt64(hdfDCSplitPK.Value);
                            finCrDrCusSoMpgObj.CDS_CRDR_NOTE_HDR = CurrPK;
                            finCrDrCusSoMpgObj.CDS_CRDR_NOTE_MPG = CrDrMpgPK;
                            finCrDrCusSoMpgObj.CDS_INVOICE_CUS_DTL = null;
                            finCrDrCusSoMpgObj.CDS_PO_DTL = null;
                            //hdfSOPK = (HiddenField)grdDCSplit.Rows[rowID].FindControl("hdfSOPK");
                            hdfInvCusDtlPK = (HiddenField)grdDCSplit.Rows[rowID].FindControl("hdfInvCusDtlPK");
                            finCrDrCusSoMpgObj.CDS_INVOICE_VND_DTL = hdfInvCusDtlPK == null ? 1 : Convert.ToInt32(hdfInvCusDtlPK.Value);
                            finCrDrCusSoMpgObj.CDS_SO_DTL = null;
                            txtQtySplit = (TextBox)grdDCSplit.Rows[rowID].FindControl("txtQtySplit");
                            finCrDrCusSoMpgObj.CDS_QTY = txtQtySplit == null ? 0 : txtQtySplit.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(txtQtySplit.Text.Trim());
                            finCrDrCusSoMpgObj.CDS_UOM = null;
                            txtRateSplit = (TextBox)grdDCSplit.Rows[rowID].FindControl("txtRateSplit");
                            finCrDrCusSoMpgObj.CDS_RATE = txtRateSplit == null ? 0 : txtRateSplit.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(txtRateSplit.Text.Trim());
                            txtSumSplit = (TextBox)grdDCSplit.Rows[rowID].FindControl("txtSumSplit");
                            finCrDrCusSoMpgObj.CDS_AMOUNT = txtSumSplit == null ? 0 : txtSumSplit.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtSumSplit.Text.Trim());
                            txtDiscountSplit = (TextBox)grdDCSplit.Rows[rowID].FindControl("txtDiscountSplit");
                            finCrDrCusSoMpgObj.CDS_DISCOUNT = txtDiscountSplit == null ? 0 : txtDiscountSplit.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtDiscountSplit.Text.Trim());
                            //finCrDrCusSoMpgObj.CDS_TAX = 0;
                            //HiddenField hdfTAXSplitTotal = (HiddenField)grdDCSplit.Rows[rowID].FindControl("hdfTAXSplitTotal");
                            //finCrDrCusSoMpgObj.CDS_TAX = string.IsNullOrEmpty(hdfTAXSplitTotal.Value) ? 0 : Convert.ToDecimal(hdfTAXSplitTotal.Value.Trim());
                            TextBox txtTAXSplitTotal = (TextBox)grdDCSplit.Rows[rowID].FindControl("txtTAXSplitTotal");
                            finCrDrCusSoMpgObj.CDS_TAX = string.IsNullOrEmpty(txtTAXSplitTotal.Text.Trim()) ? 0 : Convert.ToDecimal(txtTAXSplitTotal.Text.Trim());
                            finCrDrCusSoMpgObj.CDS_NET_AMOUNT = Convert.ToDecimal(txtSumSplit.Text.Trim()) - (finCrDrCusSoMpgObj.CDS_DISCOUNT + finCrDrCusSoMpgObj.CDS_TAX);
                            finCrDrCusSoMpgObj.CDS_REMARKS = null;
                            finCrDrCusSoMpgObj.CDS_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);

                            CheckBox chkAffectStkSplit = (CheckBox)grdDCSplit.Rows[rowID].FindControl("chkAffectStkSplit");
                            finCrDrCusSoMpgObj.CDS_IS_AFFECT_STK = chkAffectStkSplit.Checked ? (byte)1 : (byte)0;

                            finCrDrCusMpgList.Add(finCrDrCusSoMpgObj);
                            rowID++;
                        }
                        retObject = finCrDrCusMpgList;
                        break;
                    #endregion
                    #region TAXMYR
                    case ControlsEnum.TAXMYR:
                        TextBox txtOtherCharges2;

                        List<FIN_CRDR_NOTE_DTL> tempInvoiceSOSplitList;
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
                            finCrDrNoteMpgObj.CDM_INVOICE_VND_HDR = hdfInvoicePK == null ? 0 : Convert.ToInt32(hdfInvoicePK.Value);
                            finCrDrNoteMpgObj.CDM_INVOICE_CUS_HDR = null;
                            txtAmount = (TextBox)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("txtNoteFor").ToString());
                            finCrDrNoteMpgObj.CDM_AMOUNT = Convert.ToDecimal(txtAmount.Text);
                            finCrDrNoteMpgObj.CDM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            HiddenField hdfTotalTax = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTotalTax");
                            txtOtherCharges2 = (TextBox)grdInvoiceList.Rows[rowID].FindControl("txtOtherCharges");
                            finCrDrNoteMpgObj.CDM_OTHER_CHARGE = txtOtherCharges2 == null ? 0 : txtOtherCharges2.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtOtherCharges2.Text.Trim());
                            decimal tax = 0;
                            decimal.TryParse(hdfTotalTax.Value, out tax);

                            HiddenField hdfItemIcluded = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfItemIncluded");

                            HiddenField hdfTaxAmt = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTaxAmt");
                            hdftax = hdfTaxAmt.Value != string.Empty ? Convert.ToDecimal(hdfTaxAmt.Value) : 0;
                            HiddenField hdfTotalAmt = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTotalAmt");
                            hdftotalamt = hdfTotalAmt.Value != string.Empty ? Convert.ToDecimal(hdfTotalAmt.Value) : 0;
                            taxpercentage = hdftax / (hdftotalamt == 0 ? 1 : hdftotalamt);
                            basevalue = (Convert.ToDecimal(txtAmount.Text)) / (1 + taxpercentage);
                            ttaxamt = (Convert.ToDecimal(txtAmount.Text) - basevalue);
                            //if (string.IsNullOrEmpty(hdfTotalTax.Value))
                            //{
                            tax = ttaxamt;
                            //}


                            int iteminc = 0;
                            int.TryParse(hdfItemIcluded.Value, out iteminc);
                            if (iteminc > 0)
                            {
                                isItemIncluded = true;
                                tax = Convert.ToDecimal(txtAmount.Text) * Math.Round(taxpercentage,4);
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
                                    if (objInvoiceDetails != null && objInvoiceDetails.Count > 0)
                                    {
                                        List<FIN_INVOICE_VND_TAX_HDR> objInvoiceTaxList = objInvoiceDetails[0].FIN_INVOICE_VND_TAX_HDR.Where(inv => inv.VTH_TAX_CATEGORY == (byte)TaxType.Tax).ToList();
                                        decimal InvAmount = objInvoiceDetails[0].IVH_AMOUNT_TC;
                                        decimal TotalTaxPercentage = 1;
                                        if (objInvoiceTaxList != null && objInvoiceTaxList.Count > 0)
                                        {
                                            TotalTaxPercentage = objInvoiceTaxList.Sum(tx => (tx.VTH_TAX_AMT * 100) / (InvAmount == 0 ? 1 : InvAmount));
                                            foreach (FIN_INVOICE_VND_TAX_HDR invtaxhdr in objInvoiceTaxList)
                                            {
                                                SOInvoiceTaxHdr ObjTaxDtl = new SOInvoiceTaxHdr();
                                                decimal IndividualPercentage = (invtaxhdr.VTH_TAX_AMT * 100) / (InvAmount == 0 ? 1 : InvAmount);
                                                decimal TaxAmnt = (finCrDrNoteMpgObj.CDM_TAX_AMOUNT * IndividualPercentage) / ((TotalTaxPercentage == 0) ? 1 : TotalTaxPercentage);
                                                TaxAmnt = TaxAmnt * ExngeRate;
                                                double Amount = Math.Round(Convert.ToDouble(TaxAmnt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                                ObjTaxDtl.CIT_TAX = Convert.ToInt32(invtaxhdr.VTH_TAX);
                                                ObjTaxDtl.CIT_TAX_AMT = Amount;
                                                ObjTaxDtl.CIT_TAX_CATEGORY = invtaxhdr.VTH_TAX_CATEGORY;
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
                            rowID++;

                        }

                        if (grdDCSplit.Rows.Count > 0)
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

                                    InvItemPk = Convert.ToInt64(invtaxdethdr.CDS_INVOICE_VND_DTL);// Invoice Item PK;
                                    GetFieldValues(ControlsEnum.INVOICEITEMDETAILS);
                                    if (objInvoiceItemDetails != null && objInvoiceItemDetails.Count > 0)
                                    {
                                        decimal InvAmount = objInvoiceItemDetails[0].VID_AMOUNT;
                                        decimal TotalTaxPercentage = 1;
                                        List<FIN_INVOICE_VND_TAX_DTL> objInvoiceTaxList = objInvoiceItemDetails[0].FIN_INVOICE_VND_TAX_DTL.Where(inv => inv.VTL_TAX_CATEGORY == (byte)TaxType.Tax).ToList();
                                        if (objInvoiceTaxList != null && objInvoiceTaxList.Count > 0)
                                        {
                                            TotalTaxPercentage = objInvoiceTaxList.Sum(tx => (tx.VTL_TAX_AMT * 100) / (InvAmount == 0 ? 1 : InvAmount));
                                            foreach (FIN_INVOICE_VND_TAX_DTL invtaxdet in objInvoiceTaxList)
                                            {
                                                SOInvoiceTaxHdr ObjTaxDtl = new SOInvoiceTaxHdr();
                                                decimal IndividualPercentage = (invtaxdet.VTL_TAX_AMT * 100) / (InvAmount == 0 ? 1 : InvAmount);
                                                decimal TaxAmnt = (invtaxdethdr.CDS_TAX * IndividualPercentage) / (TotalTaxPercentage == 0 ? 1 : TotalTaxPercentage);
                                                TaxAmnt = TaxAmnt * ExngeRate;
                                                double Amount = Math.Round(Convert.ToDouble(TaxAmnt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                                ObjTaxDtl.CIT_TAX = Convert.ToInt32(invtaxdet.VTL_TAX);
                                                ObjTaxDtl.CIT_TAX_AMT = Amount;
                                                ObjTaxDtl.CIT_TAX_CATEGORY = invtaxdet.VTL_TAX_CATEGORY;
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
                finCrDrHdrNoteServiceClient = null;
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
                    case ControlsEnum.DRCRGET:
                        #region DRCRGET
                        if (finCrDrNoteHdrList != null && finCrDrNoteHdrList.Count > 0)
                        {
                            CurrPK = Convert.ToInt32(Request.QueryString["PK"].ToString());
                            FillProcessID(1);
                            isCancelled = Convert.ToBoolean(finCrDrNoteHdrList[0].CDH_IS_DELETED);
                            if (isCancelled)
                            {
                                btnSaveDrcr.Visible = false;
                                btnEditforCancel.Visible = false;
                                hdfIsCancelled.Value = "1";
                            }
                            else
                            {
                                btnSaveDrcr.Visible = true;
                                btnEditforCancel.Visible = true;
                                hdfIsCancelled.Value = "0";
                            }
                            if (!string.IsNullOrEmpty(finCrDrNoteHdrList[0].CDH_DEPT.ToString()) && int.TryParse(finCrDrNoteHdrList[0].CDH_DEPT.ToString(), out dept))
                            {
                                Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                base.SetUserDept();
                            }
                            //workflowCore = new WorkflowCore.CoreService();
                            //base.WkfRefID = workflowCore.GetRefID((int)CurrPK, PageProcessID);
                            GetFieldValues(ControlsEnum.FILEUPLOAD);
                            SetFieldValues(ControlsEnum.FILEUPLOAD);

                            Session[ERP.Utilities.SessionStrings.VendorPK] = finCrDrNoteHdrList[0].CDH_VENDOR;
                            Session[ERP.Utilities.SessionStrings.Vendor] = finCrDrNoteHdrList[0].PUR_VENDOR_MST.VEN_NAME;
                            Approved = finCrDrNoteHdrList[0].CDH_STATUS;
                            hdfCrDrNumber.Value = CurrPK.ToString();
                            // Get CrDrDetails
                            GetFieldValues(ControlsEnum.DRCRHDRLISTJOURNAL);
                            GetFieldValues(ControlsEnum.DRCRMPGLIST);
                            GetUIValuesFromObject(ControlsEnum.DRCRHEADERENTRY);
                            SetFieldValues(ControlsEnum.SELECTEDPIINVOICES);

                            EntryStatus = EntryStatus.VIEWMODE;

                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID((int)CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef((int)CurrPK);
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

                            ////////////////////
                            if (CurrPK > 0 && (CrDrSplitList == null || CrDrSplitList.Count == 0))
                            {
                                GetFieldValues(ControlsEnum.GETCRDRHDRBYPK);
                                if (finCrDrNoteHdrList != null && finCrDrNoteHdrList.Count > 0)
                                {
                                    CrDrSplitList = finCrDrNoteHdrList[0].FIN_CRDR_NOTE_DTL.ToList();
                                }
                            }
                            ModifiedDatePnl.Visible = true;
                            EnableDisableDrCrMode();
                            ConfigurationSettings();
                            GetFieldValues(ControlsEnum.TAXMYR);
                            SetFieldValues(ControlsEnum.TAXMYR);
                        }
                        #endregion
                        break;
                    #region Payment Header
                    case ControlsEnum.DRCRHEADERENTRY:
                        if (finCrDrNoteHdrList != null && finCrDrNoteHdrList.Count > 0)
                        {
                            CrDrSplitList = finCrDrNoteHdrList[0].FIN_CRDR_NOTE_DTL.ToList();
                            lblDrCrNo.Text = finCrDrNoteHdrList[0].CDH_NO == "" ? "[NEW]" : finCrDrNoteHdrList[0].CDH_NO;
                            txtDate.Text = finCrDrNoteHdrList[0].CDH_DATE.ToString(Resources.Constants.DateFormatShort);
                            ddlMode.SelectedValue = finCrDrNoteHdrList[0].CDH_TYPE.ToString();
                            txtPaidAmount.Text = Math.Round(finCrDrNoteHdrList[0].CDH_AMOUNT_TC, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            txtRemarks.Text = HttpUtility.HtmlDecode(finCrDrNoteHdrList[0].CDH_REMARKS);
                            LastModifiedTime = finCrDrNoteHdrList[0].CDH_MOD_DT;
                            lblLastModifiedHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);
                            Approved = WkfStatus = finCrDrNoteHdrList[0].CDH_STATUS;
                            ddlCompany.SelectedValue = finCrDrNoteHdrList[0].CDH_COMPANY.ToString();
                            txtInstrumentNo.Text = HttpUtility.HtmlDecode(finCrDrNoteHdrList[0].CDH_REF_NO);
                            txtInstrumentDate.Text = finCrDrNoteHdrList[0].CDH_REF_DATE.HasValue == false ? "" :
                            finCrDrNoteHdrList[0].CDH_REF_DATE.Value.ToString(Resources.Constants.DateFormatShort);
                            txtDeclarationNo.Text = HttpUtility.HtmlDecode(finCrDrNoteHdrList[0].CDH_IMP_DECL_NO);
                            txtExchangeRate.Text = finCrDrNoteHdrList[0].CDH_EXCHG_RATE.ToString();
                            chkAffectStock.Checked = finCrDrNoteHdrList[0].CDH_IS_AFFECT_STK == 1 ? true : false;
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
                        }
                        break;
                    #endregion
                    #region CRDRSPLITLIST
                    case ControlsEnum.CRDRSPLITLIST:
                        if (finInvoiceVndHdrListForPaymentSplit != null && finInvoiceVndHdrListForPaymentSplit.Count > 0)
                        {
                            //Test
                            lblDCSplitNo.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceVndHdrListForPaymentSplit[0].IVH_NO, 13);
                            lblDCSplitNo.ToolTip = finInvoiceVndHdrListForPaymentSplit[0].IVH_NO;

                            lblDCSplitDate.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceVndHdrListForPaymentSplit[0].IVH_DATE.ToString(Resources.Constants.DateFormatShort), 13);
                            lblDCSplitDate.ToolTip = finInvoiceVndHdrListForPaymentSplit[0].IVH_DATE.ToString(Resources.Constants.DateFormatShort);

                            lblDCSplitSupplier.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceVndHdrListForPaymentSplit[0].PUR_VENDOR_MST.VEN_NAME, 45);
                            lblDCSplitSupplier.ToolTip = finInvoiceVndHdrListForPaymentSplit[0].PUR_VENDOR_MST.VEN_NAME;

                            lblDCSplitAmount.Text = String.Format("{0:c}", finInvoiceVndHdrListForPaymentSplit[0].IVH_AMOUNT_TC);
                            lblDCSplitAmount.ToolTip = String.Format("{0:c}", finInvoiceVndHdrListForPaymentSplit[0].IVH_AMOUNT_TC);
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

                            hdfInvGroup.Value = finInvoiceVndHdrListForPaymentSplit[0].IVH_GROUP.ToString();
                        }
                        break;
                    #endregion
                    #region LINE ITEM TAX SETTINGS
                    case ControlsEnum.LINEITEMTAXSETTINGS:
                        isLineItemTaxEnabled = false;
                        if (dtTaxSettings != null && dtTaxSettings.Rows.Count > 0)
                        {
                            DataRow drTaxSettings = dtTaxSettings.AsEnumerable().SingleOrDefault(aa => aa.Field<string>("ACF_DATA").Trim() == "TAX");
                            if (drTaxSettings != null)
                            {
                                int configval = Convert.ToInt32(drTaxSettings["ACF_VALUE"]);
                                if (configval == 1)
                                {
                                    isLineItemTaxEnabled = true;
                                }
                            }
                        }
                        break;
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
        ///// Tax Details Configurations
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
            // Check Whether OtherCharge is needed or not  for Tax Calculation     
            IsTaxForOtherCharge.Value = (GetGlobalResourceObject("ConfigurationsRes", "OtherChargeTaxPurchase")).ToString();

            #region Advance Invoice Tax Settings
            DataTable dtTax = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("ADVANCE INVOICE SETTINGS", "TAX");
            if (dtTax != null && dtTax.Rows.Count > 0)
            {
                IsAdvInvHasTax = dtTax.Rows[0]["ACF_VALUE"].ToString() == "1" ? true : false;
            }
            #endregion

            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";

            #region  Set Applicaiton Configuaration for Tax & Discount (HeaderWise,ItemWise, Both HeaderAndItemWise)
            dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("PURCHASE ITEM WISE SETTINGS", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfisTaxAdd.Value = dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "TAX")["ACF_VALUE"].ToString();
                hdfisDiscountAdd.Value = dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "DISCOUNT")["ACF_VALUE"].ToString();
            }
            #endregion
            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "VENDOR");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsSBUVendor.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
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
                    ddlCompanySrch.Items.Clear();
                    ddlCompanySrch.Items.Clear();
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
                            admConfigMstList.RemoveAll((x => x.CFG_VALUE == (int)DebitCreditModeEnum.CREDIT)); //Remove Credit while choosing multiple Purchase invoice
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
                    case ControlsEnum.SELECTEDPIINVOICES:
                        if (finInvoiceVndHdrList != null)
                        {
                            grdInvoiceList.DataSource = finInvoiceVndHdrList;
                            grdInvoiceList.DataBind();
                            if (finInvoiceVndHdrList.Count > 0)
                            {
                                if (finInvoiceVndHdrList[0].FIN_INVOICE_VND_TRX_MPG != null && finInvoiceVndHdrList[0].FIN_INVOICE_VND_TRX_MPG.Count > 0
                                    && finInvoiceVndHdrList[0].IVH_IS_WORK_ORDER == 0)
                                {
                                    hdfPOType.Value = finInvoiceVndHdrList[0].FIN_INVOICE_VND_TRX_MPG.ToList()[0].PUR_ORDER_HDR.POH_ITEM_TYPE.ToString();
                                }
                                else
                                    hdfPOType.Value = ((int)POInvoiceGroup.Goods).ToString();
                                ddlCompany.SelectedValue = finInvoiceVndHdrList[0].IVH_COMPANY.ToString();

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

                                hdfInvoiceGroup.Value = finInvoiceVndHdrList[0].IVH_GROUP.ToString();
                                hdfVendorPK.Value = finInvoiceVndHdrList[0].IVH_VENDOR.ToString();
                                hdfVendorAccountNo.Value = finInvoiceVndHdrList[0].IVH_VENDOR_ACCOUNT.ToString();
                                hdfInvoiceCurr.Value = finInvoiceVndHdrList[0].IVH_CURRENCY.ToString();
                                lblPaidAmount.Text = GetLocalResourceObject("Amount").ToString() + " (" + finInvoiceVndHdrList[0].ADM_CURRENCY_MST1.CUR_CODE + ")";
                                GetFieldValues(ControlsEnum.EXCHANGERATE);
                                if (finInvoiceVndHdrList[0].IVH_TYPE != 1)
                                {
                                    txtExchangeRate.CssClass = "input-small numeric input-disabled";
                                    txtExchangeRate.Enabled = false;
                                }
                                else
                                {
                                    txtExchangeRate.CssClass = "input-small numeric ";
                                    txtExchangeRate.Enabled = true;
                                }
                                string remarks = string.Empty;
                                int newline = 1;
                                foreach (FIN_INVOICE_VND_HDR item in finInvoiceVndHdrList)
                                {
                                    remarks = remarks + item.IVH_VENDOR_INV_NO + " " + (item.IVH_DATE_RECEIVED != null ? (string.IsNullOrEmpty(item.IVH_DATE_RECEIVED.ToString()) == true ? string.Empty : item.IVH_DATE_RECEIVED.ToString(Resources.Constants.DateFormatShort)) : string.Empty) + (newline == finInvoiceVndHdrList.Count ? string.Empty : Environment.NewLine);
                                    newline++;
                                }

                                txtRemarks.Text = HttpUtility.HtmlDecode(remarks);
                                //GetFieldValues(ControlsEnum.EXCHANGERATE);
                            }
                            else
                            {
                                txtPaidAmount.Text = "0.00";
                            }
                        }
                        else if (finCrDrNoteMpgList != null)
                        {
                            grdInvoiceList.DataSource = finCrDrNoteMpgList;
                            grdInvoiceList.DataBind();
                            if (finCrDrNoteMpgList != null && finCrDrNoteMpgList.Count > 0)
                            {
                                if (finCrDrNoteMpgList[0].FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_TRX_MPG != null && finCrDrNoteMpgList[0].FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_TRX_MPG.Count > 0)
                                {
                                    hdfPOType.Value = finCrDrNoteMpgList[0].FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_TRX_MPG.ToList()[0].PUR_ORDER_HDR.POH_ITEM_TYPE.ToString();
                                }
                                else
                                    hdfPOType.Value = ((int)POInvoiceGroup.Goods).ToString();
                                hdfInvoiceGroup.Value = finCrDrNoteMpgList[0].FIN_INVOICE_VND_HDR.IVH_GROUP.ToString();
                                if (finCrDrNoteMpgList[0].FIN_INVOICE_VND_HDR != null)
                                {
                                    hdfVendorPK.Value = finCrDrNoteMpgList[0].FIN_INVOICE_VND_HDR.IVH_VENDOR.ToString();
                                    hdfVendorAccountNo.Value = finCrDrNoteMpgList[0].FIN_INVOICE_VND_HDR.IVH_VENDOR_ACCOUNT.ToString();
                                    hdfInvoiceCurr.Value = finCrDrNoteMpgList[0].FIN_INVOICE_VND_HDR.IVH_CURRENCY.ToString();
                                    lblPaidAmount.Text = GetLocalResourceObject("Amount").ToString() + " (" + finCrDrNoteMpgList[0].FIN_INVOICE_VND_HDR.ADM_CURRENCY_MST1.CUR_CODE + ")";
                                    hdfExchangeCurr.Value = finCrDrNoteMpgList[0].FIN_CRDR_NOTE_HDR.CDH_EXCHG_RATE.ToString();
                                    //Exchange rate to Textbox
                                    txtExchangeRate.Text = hdfExchangeCurr.Value.ToString();
                                    if (finCrDrNoteMpgList[0].FIN_INVOICE_VND_HDR.IVH_TYPE != 1)
                                    {
                                        txtExchangeRate.CssClass = "input-small numeric input-disabled";
                                        txtExchangeRate.Enabled = false;
                                    }
                                    else
                                    {
                                        txtExchangeRate.CssClass = "input-small numeric ";
                                        txtExchangeRate.Enabled = true;
                                    }
                                    // GetFieldValues(ControlsEnum.EXCHANGERATE);
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
                                    SelectedInvoiceCrDrList.Add((long)CrDr.CDM_INVOICE_VND_HDR);
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
                        CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCrDrPk")).Value); //Convert.ToInt32(grdCrDbHdr.DataKeys[grdrow.RowIndex].Values[0]); //
                        Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                        Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).Text;
                        Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                        hdfCrDrNumber.Value = CurrPK.ToString();
                        // Get CrDrDetails
                        GetFieldValues(ControlsEnum.DRCRHDRLISTJOURNAL);
                        GetFieldValues(ControlsEnum.DRCRMPGLIST);
                        GetUIValuesFromObject(ControlsEnum.DRCRHEADERENTRY);
                        SetFieldValues(ControlsEnum.SELECTEDPIINVOICES);


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
            hdfShipCharge.Value = "0";
            hdfShipchargeNew.Value = "0";
            CurrPK = 0;
            CrDrSplitList = null;
            txtVendor.Text = string.Empty;
            hdfVendorID.Value = string.Empty;
            txtCrDrNumber.Text = string.Empty;
            hdfCrDrNumber.Value = string.Empty;
            txtRemarks.Text = string.Empty;
            txtPaidAmount.Text = string.Empty;
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
            //SetFieldValues(ControlsEnum.FINPERIOD);
            base.WkfRefID = 0;
            SelectedInvoicesCrDr = null;
            selectedInvoiceList = null;
            SortBy = Resources.DataFieldRes.CrDrDate;
            ThenBy = Resources.DataFieldRes.CrDrNo;
            ddlStatus.SelectedIndex = 0;
            ddlCreditDebitType.SelectedIndex = 0;
            FileDetailsList = null;
            PageIndex = "1";
            ddlCompanySrch.SelectedIndex = -1;
        }

        private void ResetForm(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
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
        private void CalculateOtherChargeTax()
        {
            HiddenField hdfHdrTax;
            Label lblOtherAmount;
            LinkButton lbnTotTax;
            HiddenField hdfTotTax;
            HiddenField hdfTaxApplayValue;
            TextBox txtOtherCharges;
            decimal hdrTaxPer = 0;
            decimal hdrOtherCharge = 0;
            decimal hdrTax = 0;
            decimal otherCharge = 0;
            decimal othrChargeTax = 0;
            decimal totalTax = 0;

            foreach (GridViewRow grdrow in grdInvoiceList.Rows)
            {
                txtOtherCharges = (TextBox)grdrow.FindControl("txtOtherCharges");
                if (txtOtherCharges.Text.Trim() != string.Empty && decimal.TryParse(txtOtherCharges.Text, out otherCharge))
                {
                    hdfHdrTax = (HiddenField)grdrow.FindControl("hdfHdrTax");
                    hdfTotTax = (HiddenField)grdrow.FindControl("hdfTotalTax");
                    lblOtherAmount = (Label)grdrow.FindControl("lblOtherAmount");
                    lbnTotTax = (LinkButton)grdrow.FindControl("lbnTotalTax");
                    decimal.TryParse(hdfTotTax.Value, out totalTax);
                    decimal.TryParse(hdfHdrTax.Value, out hdrTax);
                    decimal.TryParse(lblOtherAmount.Text, out hdrOtherCharge);
                    if (hdrTax > 0 && hdrOtherCharge > 0)
                    {
                        hdrTaxPer = hdrTax / hdrOtherCharge;
                        othrChargeTax = otherCharge * hdrTaxPer;
                        lbnTotTax.Text = GetFormattedCurrencyWithSeperator((totalTax + othrChargeTax).ToString());
                    }
                }
                else
                {
                    txtOtherCharges.Text = GetFormattedCurrencyWithSeperator("0.00");
                    ((LinkButton)grdrow.FindControl("lbnTotalTax")).Text = GetFormattedCurrencyWithSeperator(((HiddenField)grdrow.FindControl("hdfTaxApplayValue")).Value);
                }
            }
        }
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

            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            FinCrDrHdrNoteService finCrDrHdrNoteServiceClient;
            finCrDrHdrNoteServiceClient = null;
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            DropDownList ddlWkfAction;
            TextBox WrkfComments;
            string action;
            HiddenField hdfInvPK;
            TextBox txtNoteFor;
            bool isCancelled = false;
            bool bIsChecked = false;
            bool bIsInoicePosted = true;
            int bankPK;
            int mode;
            Label lblTotalPayNowFooter;
            WorkflowCore.CoreService workflowCore;
            List<FIN_CRDR_NOTE_DTL> tempInvoiceSOSplitList;
            try
            {
                long? result;
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
                    case ActionsEnum.OTHERCHARGETAX:
                        CalculateOtherChargeTax();
                        break;
                    #region ItemSelected
                    case ActionsEnum.ITEMSELECTED:
                        GridViewRow gvr;
                        HiddenField hdfDept;
                        int dept;
                        HiddenField hdfPaymentID;
                        int pk;
                        gvr = ((RadioButton)sender).Parent.Parent as GridViewRow;
                        hdfPaymentID = gvr.FindControl("hdfCrDrPk") as HiddenField;
                        //isCancelled = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfDelStatus")).Value) == 1 ? true : false;
                        //if (System.Text.RegularExpressions.Regex.IsMatch(((HiddenField)gvr.FindControl("hdfDelStatus")).Value, @"\d")) //Check whether it is int or string
                        HiddenField hdfDelStatus = (HiddenField)gvr.FindControl("hdfDelStatus");
                        FillProcessID(1);
                        if (hdfDelStatus != null)
                            isCancelled = Convert.ToBoolean(hdfDelStatus.Value);
                        if (isCancelled)
                        {
                            btnSaveDrcr.Visible = false;
                            btnEditforCancel.Visible = false;
                            hdfIsCancelled.Value = "1";
                        }
                        else
                        {
                            btnSaveDrcr.Visible = true;
                            btnEditforCancel.Visible = true;
                            hdfIsCancelled.Value = "0";
                        }

                        if (hdfPaymentID != null && int.TryParse(hdfPaymentID.Value, out pk))
                        {
                            CurrPK = pk;
                        }
                        hdfDept = gvr.FindControl("hdfDept") as HiddenField;// grdShippingPlanList.FindControl("hdfDept") as HiddenField;
                        if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                        {
                            Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                            base.SetUserDept();
                        }
                        workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = workflowCore.GetRefID((int)CurrPK, PageProcessID);
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
                                #region Checking for EMI/EMR is created or not
                                GetFieldValues(ControlsEnum.GETCRDRHDRDETAILS);
                                if (ObjfinCrDrNoteHdrList != null && ObjfinCrDrNoteHdrList.Count > 0)
                                {
                                    if (ObjfinCrDrNoteHdrList[0].INV_ITEM_CONS_HDR.Where(r => r.ICH_DEL_STATUS == 0).Count() > 0)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_CrdrUsed").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                        return;
                                    }
                                }
                                #endregion
                            }

                            //if ((!Isposted() && Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.CREDIT) || (Isposted() && Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.DEBIT) || (Isposted() && Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.CREDIT))
                            //{
                            if (grdInvoiceList.Rows.Count >= 1)
                            {
                                finCrDrNoteHdrList = new List<FIN_CRDR_NOTE_HDR>();
                                finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                                finCrDrHdrNoteServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                                finCrDrNoteHdrObj = CommonFunctions.Initilize<FIN_CRDR_NOTE_HDR>();
                                finCrDrNoteHdrObj = (FIN_CRDR_NOTE_HDR)SetUIValuesToObject(ControlsEnum.DRCRHEADERENTRY);

                                lblTotalPayNowFooter = (Label)grdInvoiceList.FooterRow.FindControl("lblTotalPayNowFooter");
                                lblTotalPayNowFooter.Text = hdfTotalPayNowFooter.Value;
                                if (finCrDrHdrNoteServiceClient.IsRefnoExist(finCrDrNoteHdrObj))
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_RefnoExist").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                                if (finCrDrNoteHdrObj != null)
                                {
                                    if (finCrDrNoteHdrObj.FIN_CRDR_NOTE_MPG.ToList().Count > 0)
                                    {
                                        finCrDrNoteHdrList.Add(finCrDrNoteHdrObj);
                                        int Archiveresult = 0;
                                        if (finCrDrNoteHdrObj.CDH_PK > 0 && finCrDrNoteHdrObj.CDH_STATUS > 0)
                                        {
                                            //After getting entry into workflow, for each update keep version details of voucher for Audit trail 
                                            Archiveresult = BusinessLogic.POInvoicing.POInvoiceBL.SaveDRCR_Note_ArchiveDetails(finCrDrNoteHdrObj.CDH_PK);
                                        }
                                        if (!string.IsNullOrEmpty(hdfShipCharge.Value) && !string.IsNullOrEmpty(hdfShipchargeNew.Value))
                                        {
                                            if (Convert.ToDecimal(hdfShipchargeNew.Value) > Convert.ToDecimal(hdfShipCharge.Value))
                                            {
                                                litErrorMsg.Text = GetLocalResourceObject("Err_Msg_ShipCharge").ToString();
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text, hdfShipCharge.Value);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrMsgShpCharge",
                                                    "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                return;
                                            }
                                        }
                                        if ((Archiveresult > 0 && finCrDrNoteHdrObj.CDH_STATUS > 0) || finCrDrNoteHdrObj.CDH_STATUS == 0)
                                        {
                                            #region Transaction Begin
                                            using (TransactionScope scope = new TransactionScope())
                                            {
                                                try
                                                {
                                                    result = finCrDrHdrNoteServiceClient.SaveCrDrNoteHdr(finCrDrNoteHdrList, false);
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
                                                        }
                                                        else
                                                        {
                                                            POInvoiceService poInvoiceServiceClient = new POInvoiceService();
                                                            poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                                                            int r = poInvoiceServiceClient.AttachDocumentDelete(0, (int)DocTaskEnum.DEBITCREDITNOTE, Convert.ToInt32(result.Value));
                                                            //int delStatus= BusinessLogic.Finance.CNDNBL.DeleteAttachmentDocuments(0, (int)DocTaskEnum.DEBITCREDITNOTE, Convert.ToInt32(result.Value));
                                                            if (r < 1 && r != (int)DbSaveStatus.ALREADYDELETED)
                                                            {
                                                                throw new Exception(GetLocalResourceObject("Err_SaveAttachmentDocument").ToString());
                                                            }
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
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_Amount").ToString()) + "','" + Resources.Messages.Information + "');", true);
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
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_InvoiceCount").ToString()) + "');", true);
                            }
                            //}
                            //else
                            //{
                            //    litErrorMsg.Text = GetLocalResourceObject("Msg_InvNotjournalized").ToString();
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            //}
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
                                    lblDrCrNo.Text = transactionNumber;
                                    break;
                                case (int)DrCrModeEnum.DEBIT:
                                    GetFieldValues(ControlsEnum.DEBIT);
                                    lblDrCrNo.Text = transactionNumber;
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
                        GetFieldValues(ControlsEnum.DRCRHDRLIST);
                        SetFieldValues(ControlsEnum.DRCRHDRLIST);
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        PageIndex = "1";
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
                                RadioButton rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID);
                                hdfCreditDebitType = (HiddenField)grdrow.FindControl("hdfCreditDebitType");
                                HiddenField hdfInvType = (HiddenField)grdrow.FindControl("hdfInvoiceType");
                                // check row selected or not
                                if (rbtn.Checked)
                                {
                                    // get pk from the grid and assign to CurrPk
                                    CurrPK = Convert.ToInt32(grdCrDbHdr.DataKeys[grdrow.RowIndex].Values[0]);
                                    hdfCrDrNumber.Value = CurrPK.ToString();
                                    Reftype = hdfCreditDebitType.Value == "1" ? ApplicationType.DN : ApplicationType.CN;
                                    InvType = Convert.ToInt32(hdfInvType.Value);
                                    bIsChecked = true;
                                    break;
                                }

                            }
                        }
                        else
                        {
                            Reftype = ddlMode.SelectedValue == "1" ? ApplicationType.DN : ApplicationType.CN;
                            bIsChecked = true;
                        }
                        if (bIsChecked)
                        {
                            // InvType = 1;
                            if (InvType == (int)PurchaseType.Import)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + Reftype + "&APPSUBTYPE=12" + "');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + Reftype + "&APPSUBTYPE=11" + "');", true);
                            }
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                        break;
                    #endregion
                    #region View
                    case ActionsEnum.VIEW:
                        SetUIEditView(commonActions);
                        btnCancel.Focus();
                        ConfigurationSettings();
                        GetFieldValues(ControlsEnum.TAXMYR);
                        SetFieldValues(ControlsEnum.TAXMYR);
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetForm();
                        PageIndex = "1";
                        GetFieldValues(ControlsEnum.DRCRHDRLIST);
                        SetFieldValues(ControlsEnum.DRCRHDRLIST);
                        uclPaging.TotalPages = TotalPages;
                        uclPaging.CurrentPage = 1;
                        break;
                    #endregion
                    #region Remove
                    case ActionsEnum.REMOVE:
                        int INVPk = int.Parse(((Button)sender).CommandArgument.ToString());

                        HiddenField hdfInvoicePK = null;
                        TextBox txtNote = null;
                        foreach (GridViewRow grdRow in grdInvoiceList.Rows)
                        {
                            hdfInvoicePK = grdRow.FindControl("hdfInvoicePK") as HiddenField;
                            txtNote = grdRow.FindControl("txtNoteFor") as TextBox;
                            if (hdfInvoicePK != null && txtNote != null && !string.IsNullOrEmpty(hdfInvoicePK.Value) && !string.IsNullOrEmpty(txtNote.Text))
                            {
                                decimal d = 0;
                                decimal.TryParse(txtNote.Text, out d);
                                if (!dicTempAmount.ContainsKey(hdfInvoicePK.Value)
                                    && hdfInvoicePK.Value != INVPk.ToString())
                                {
                                    dicTempAmount.Add(hdfInvoicePK.Value, d);
                                }
                            }
                        }


                        if (CurrPK == 0)
                        {
                            SelectedInvoicesCrDr.Remove(INVPk);
                            selectedInvoiceList = SelectedInvoicesCrDr;
                        }
                        if (SelectedInvoicesCrDr != null)
                        {
                            if (FinInvoiceVndHdrSelectedList != null && FinInvoiceVndHdrSelectedList.Count > 0)
                            {
                                finInvoiceVndHdrList = FinInvoiceVndHdrSelectedList;
                                finInvoiceVndHdrObj = CommonFunctions.Initilize<ERPData.FIN_INVOICE_VND_HDR>();
                                finInvoiceVndHdrObj = finInvoiceVndHdrList.SingleOrDefault(ivh => ivh.IVH_PK == INVPk);
                                if (finInvoiceVndHdrObj != null)
                                {
                                    finInvoiceVndHdrList.Remove(finInvoiceVndHdrObj);
                                    FinInvoiceVndHdrSelectedList = finInvoiceVndHdrList;
                                    SetFieldValues(ControlsEnum.SELECTEDPIINVOICES);
                                }
                            }
                            else if (FinInvoiceCrDrSelectedList != null && FinInvoiceCrDrSelectedList.Count > 0)
                            {
                                finCrDrNoteMpgList = FinInvoiceCrDrSelectedList;
                                finCrDrNoteMpgObj = CommonFunctions.Initilize<ERPData.FIN_CRDR_NOTE_MPG>();
                                finCrDrNoteMpgObj = finCrDrNoteMpgList.SingleOrDefault(crdr => crdr.CDM_INVOICE_VND_HDR == INVPk);
                                if (finCrDrNoteMpgObj != null)
                                {
                                    finCrDrNoteMpgList.Remove(finCrDrNoteMpgObj);
                                    SetFieldValues(ControlsEnum.SELECTEDPIINVOICES);
                                }
                            }
                        }
                        else if (EditedPaymentDtls != null)
                        {
                            finPaymentVndTrxMpgList = EditedPaymentDtls;
                        }

                        break;
                    #endregion
                    #region Tabs
                    case ActionsEnum.POINVOICE:
                        CrDrFlag = 1;
                        CheckUserRightsAndRedirect(Resources.PageURL.PurchaseOrderInvoicing);
                        //Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.PurchaseOrderInvoicing), false);
                        break;
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
                        foreach (GridViewRow grdrow in grdCrDbHdr.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                                Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).Text;
                                break;
                            }
                        }
                        CheckUserRightsAndRedirect(Resources.PageURL.AccountsPayable);
                        //Response.Redirect(Resources.PageURL.AccountsPayable);
                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        //if ((!Isposted() && Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.CREDIT) || (Isposted() && Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.DEBIT) || (Isposted() && Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.CREDIT))
                        //{
                        //Show WorkFlow Popup   
                        if (grdInvoiceList.Rows.Count >= 1)
                        {
                            hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                            ucrWrkf.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_InvoiceCount").ToString()) + "');", true);
                        }
                        //}
                        //else
                        //{
                        //    litErrorMsg.Text = GetLocalResourceObject("Msg_InvNotjournalized").ToString();
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //}
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        //if ((!Isposted() && Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.CREDIT) || (Isposted() && Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.DEBIT) || (Isposted() && Convert.ToByte(ddlMode.SelectedValue) == (int)DrCrModeEnum.CREDIT))
                        //{
                        //Show WorkFlow Popup   
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
                        //}
                        //else
                        //{
                        //    litErrorMsg.Text = GetLocalResourceObject("Msg_InvNotjournalized").ToString();
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //}
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
                                    lblTotalPayNowFooter.Text = hdfTotalPayNowFooter.Value;
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
                                                    //After getting entry into workflow, for each update keep version details of voucher for Audit trail 
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
                                                                }
                                                                else
                                                                {
                                                                    POInvoiceService poInvoiceServiceClient = new POInvoiceService();
                                                                    poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                                                                    int delStatus = poInvoiceServiceClient.AttachDocumentDelete(0, (int)DocTaskEnum.DEBITCREDITNOTE, Convert.ToInt32(result.Value));
                                                                    if (delStatus < 1 && delStatus != (int)DbSaveStatus.ALREADYDELETED)
                                                                    {
                                                                        throw new Exception(GetLocalResourceObject("Err_SaveAttachmentDocument").ToString());
                                                                    }
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
                                                        #region WkfSummarySave
                                                        int resultSummary = BusinessLogic.CommonManagement.CommonBL.SaveSummary(result.Value, Convert.ToInt32(hdfProcessID.Value));
                                                        if (resultSummary <= 0)
                                                        {

                                                            litErrorMsg.Text = Resources.Messages.SummaryActionFailed;
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                                        }
                                                        #endregion
                                                        ucrWrkf.ApplicationID = (int)result;
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
                                ucrWrkf.ApplicationID = (int)CurrPK;

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
                                        ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();
                                        if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        {
                                            AdmTrxLogDet.ATL_ACTION = (byte)LogAction.CANCEL;
                                        }
                                        else
                                        {
                                            AdmTrxLogDet.ATL_ACTION = (byte)LogAction.SUBMIT;
                                        }

                                        //Show Save success message and reset Contract Entry
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                                        object[] args = new object[2];
                                        args[0] = Resources.PageNameRes.CreditDebitNotes;
                                        args[1] = lblDrCrNo.Text;

                                        #region LOG SAVE
                                        CommonServiceClient = new CommonService();
                                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);

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

                                        #endregion
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        //    + "','" + Resources.ErpRes.Information + "');", true);

                                        if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                        {
                                            ResetForm();
                                            //Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.InboxURL));
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


                            //if (grdInvoiceList.Rows.Count >= 1)
                            //{
                            //finCrDrNoteHdrList = new List<FIN_CRDR_NOTE_HDR>();
                            //finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                            //finCrDrHdrNoteServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                            //finCrDrNoteHdrObj = CommonFunctions.Initilize<FIN_CRDR_NOTE_HDR>();
                            //finCrDrNoteHdrObj = (FIN_CRDR_NOTE_HDR)SetUIValuesToObject(ControlsEnum.WRKFSUBMIT);

                            //lblTotalPayNowFooter = (Label)grdInvoiceList.FooterRow.FindControl("lblTotalPayNowFooter");
                            //lblTotalPayNowFooter.Text = hdfTotalPayNowFooter.Value;

                            //if (hdfExchangeCurr.Value != "-1")
                            //{
                            //if (finCrDrNoteHdrObj != null)
                            //{
                            //if (finCrDrNoteHdrObj.FIN_CRDR_NOTE_MPG.ToList().Count > 0)
                            //{
                            //finCrDrNoteHdrList.Add(finCrDrNoteHdrObj);
                            //result = finCrDrHdrNoteServiceClient.SaveCrDrNoteHdr(finCrDrNoteHdrList);
                            //if (result > 0)// Save Success ! do WorkFlow
                            //{
                            //Workflow submission
                            //ucrWrkf.ApplicationID = (int)result;
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
                            //}
                            //else
                            //{
                            //    if (result == (int)DbSaveStatus.SQLERROR)
                            //    {
                            //        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            //            + "','" + Resources.ErpRes.Information + "');", true);
                            //    }
                            //    else if (result == (int)DbSaveStatus.CONCURRENCY)
                            //    {
                            //        litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.EditUsedByAnotherUser;
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            //        + "','" + Resources.ErpRes.Information + "');", true);
                            //    }
                            //    else if (result == (int)DbSaveStatus.CODEEXIST)
                            //    {
                            //        litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            //        + "','" + Resources.ErpRes.Information + "');", true);
                            //    }
                            //    else
                            //    {
                            //        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            //        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            //            + "','" + Resources.ErpRes.Information + "');", true);
                            //    }
                            //}
                            //}
                            //else
                            //{
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);

                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_Amount").ToString()) + "','" + Resources.Messages.Information + "');", true);
                            //}
                            //}
                            //}
                            //else
                            //{
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);

                            //    litErrorMsg.Text = Resources.Messages.Msg_ErrConversionFactor;
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            //}
                            //}
                            //else
                            //{
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_InvoiceCount").ToString()) + "');", true);
                            //}
                        }
                        break;
                    #endregion
                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
                        // Check fro all invoice is posted or not
                        foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                        {
                            HiddenField hdfhasjournalized = (HiddenField)grdrow.FindControl("hdfhasjournalized");
                            if (!string.IsNullOrEmpty(hdfhasjournalized.Value))
                            {
                                if (!Convert.ToBoolean(hdfhasjournalized.Value))
                                {
                                    bIsInoicePosted = false;
                                    break;
                                }
                            }
                            else
                            {
                                bIsInoicePosted = false;
                                break;
                            }
                        }
                        if (!bIsInoicePosted)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_InvoiceNotPosted").ToString()) + "');", true);
                            return;
                        }
                        ////////////////////////////////////////
                        finCrDrNoteHdrObj = CommonFunctions.Initilize<FIN_CRDR_NOTE_HDR>();
                        finCrDrNoteHdrObj.CDH_PK = CurrPK;
                        GetFieldValues(ControlsEnum.GETCRDRHDRBYPK);
                        SetUIValuesToObject(ControlsEnum.JOURNALIZE);
                        break;
                    #endregion
                    #region Journalize Update
                    case ActionsEnum.JOURNALIZEUPDATE:

                        ucrJournalize.ResetForm();

                        hdfCrDrNumber.Value = "";
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();

                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.DRCRHDRLIST);
                        SetFieldValues(ControlsEnum.DRCRHDRLIST);
                        break;
                    #endregion
                    #region Journalize Save
                    case ActionsEnum.JOURNALIZESAVE:
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                            if (Transaction == "SAVE")
                            {
                                finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                                finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                                result = finCrDrHdrNoteServiceClient.UpdateCrDrHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);

                            }
                            else if (Transaction == "DELETE")
                            {
                                finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                                finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                                result = finCrDrHdrNoteServiceClient.UpdateCrDrHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                            }
                        }
                        ((Button)sender).CommandName = ActionsEnum.SAVE.ToString();
                        hdfCrDrNumber.Value = "";
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
                            else if (Transaction == "DELETE")
                            {
                                finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                                finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                                result = finCrDrHdrNoteServiceClient.UpdateCrDrHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                            }
                        }
                        ((Button)sender).CommandName = ActionsEnum.SUBMIT.ToString();
                        hdfCrDrNumber.Value = "";
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);

                        //if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ReferanceID > 0)
                        //{
                        //    ResetForm();
                        //    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.InboxURL));
                        //}
                        //else
                        //{
                        ResetForm();
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
                            if (Transaction == "SAVE")
                            {
                                finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                                finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                                result = finCrDrHdrNoteServiceClient.UpdateCrDrHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);

                            }
                            else if (Transaction == "DELETE")
                            {
                                finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                                finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                                result = finCrDrHdrNoteServiceClient.UpdateCrDrHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                            }
                        }
                        hdfCrDrNumber.Value = "";
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
                        hdfCrDrNumber.Value = "";
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.DRCRHDRLIST);
                        SetFieldValues(ControlsEnum.DRCRHDRLIST);
                        break;
                    #endregion
                    #region Credit Debit Notes
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
                    case ActionsEnum.CREDITDEBITDETAIL:
                        if (SelectedInvoicesCrDr != null && SelectedInvoicesCrDr.Count > 0)
                        {
                            selectedInvoiceList = SelectedInvoicesCrDr;
                            GetFieldValues(ControlsEnum.SELECTEDPIINVOICES);
                            SetFieldValues(ControlsEnum.SELECTEDPIINVOICES);
                            EntryStatus = EntryStatus.NEWMODE;
                            lblDrCrNo.Text = "[NEW]";
                            txtDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                        }
                        else
                        {
                            SetUIEditView(commonActions);
                            ModifiedDatePnl.Visible = true;
                        }
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
                                //Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                //Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomerName")).Text;
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
                            if (finPaymentCRDRmpgList != null && finPaymentCRDRmpgList.Count > 0)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_CheckforCancelDRCR").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            FillProcessID(11);
                            SetUIEditView(commonActions);
                            workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID((int)CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef((int)CurrPK);
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
                            //GetFieldValues(ControlsEnum.TAXSETTINGS);
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
                        tempfinCrDrCusMpgList = tempInvoiceSOSplitList.Where(dtl => dtl.FIN_CRDR_NOTE_MPG.CDM_INVOICE_VND_HDR == InvoicePK).ToList();



                        if (hdfReceiptMpgPK != null && !string.IsNullOrEmpty(hdfReceiptMpgPK.Value) && !hdfReceiptMpgPK.Value.Equals("0"))
                        {
                            CrDrMpgPK = Convert.ToInt64(hdfReceiptMpgPK.Value);
                            GetFieldValues(ControlsEnum.CRDRSPLITLIST);

                            if (tempfinCrDrCusMpgList == null || tempfinCrDrCusMpgList.Count == 0)
                            {
                                if (finCrDrCusMpgList != null && finCrDrCusMpgList.Count > 0)//Edit
                                {
                                    //tempInvoiceSOSplitList.Where(dtl => dtl.FIN_CRDR_NOTE_MPG.CDM_INVOICE_VND_HDR == InvoicePK)
                                    //                .ToList().ForEach(dtl => tempInvoiceSOSplitList.Remove(dtl));
                                    //finCrDrCusMpgList.ForEach(dtl =>
                                    //{
                                    //    dtl.FIN_CRDR_NOTE_MPG = new FIN_CRDR_NOTE_MPG()
                                    //    {
                                    //        CDM_INVOICE_VND_HDR = InvoicePK
                                    //    };
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
                                finCrDrCusMpgList = tempfinCrDrCusMpgList.Where(dtl => dtl.FIN_CRDR_NOTE_MPG.CDM_INVOICE_VND_HDR == InvoicePK).ToList();

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
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowDrCrSplit();", true);
                        break;
                    #endregion
                    #region DCSPLITSAVE
                    case ActionsEnum.DCSPLITSAVE:
                        decimal TotalPayNow = 0;
                        decimal TotalTax = 0;
                        decimal discount = 0;
                        HiddenField lblTotalPayNow = (HiddenField)grdDCSplit.FooterRow.FindControl("hdfTotalPayNowFooterSplit");
                        HiddenField hdfTotalTaxFooterSplit = (HiddenField)grdDCSplit.FooterRow.FindControl("hdfTotalTaxFooterSplit");
                        HiddenField hdfTotalDiscountFooterSplit = (HiddenField)grdDCSplit.FooterRow.FindControl("hdfTotalDiscountFooterSplit");
                        TotalPayNow = Convert.ToDecimal(lblTotalPayNow.Value);
                        decimal.TryParse(hdfTotalTaxFooterSplit.Value, out TotalTax);

                        if (InvRowIndex >= 0)
                        {
                            if (hdfisDiscountAdd.Value == "1")
                            {
                                decimal.TryParse(hdfTotalDiscountFooterSplit.Value, out discount);
                                HiddenField hdfDiscoutntTotalSplitApplay = (HiddenField)grdInvoiceList.Rows[InvRowIndex].FindControl("hdfDiscoutntTotalSplitApplay");
                                hdfDiscoutntTotalSplitApplay.Value = discount.ToString();
                            }

                            TextBox txtTotalPayNow = (TextBox)grdInvoiceList.Rows[InvRowIndex].FindControl("txtNoteFor");
                            HiddenField hdfItemIncluded = (HiddenField)grdInvoiceList.Rows[InvRowIndex].FindControl("hdfItemIncluded");
                            HiddenField hdfIsDetailTaxExist = (HiddenField)grdInvoiceList.Rows[InvRowIndex].FindControl("hdfIsDetailTaxExist");
                            lbnTotalTax = (LinkButton)grdInvoiceList.Rows[InvRowIndex].FindControl("lbnTotalTax");
                            hdfTotalTax = (HiddenField)grdInvoiceList.Rows[InvRowIndex].FindControl("hdfTotalTax");
                            HiddenField hdfTaxApplayValue = (HiddenField)grdInvoiceList.Rows[InvRowIndex].FindControl("hdfTaxApplayValue");
                            txtTotalPayNow.Text = Math.Round(TotalPayNow, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            if (hdfIsDetailTaxExist.Value == "1")
                            {
                                lbnTotalTax.Text = GetFormattedCurrencyWithSeperator(TotalTax);
                                hdfTaxApplayValue.Value = hdfTotalTax.Value = hdfTotalTaxFooterSplit.Value;
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
                        hdfSplitCount.Value = "1";
                        //TextBox txtOthrChrg= (TextBox)grdInvoiceList.Rows[InvRowIndex].FindControl("txtOtherCharges");
                        //decimal othCharge=0;
                        //if (decimal.TryParse(txtOthrChrg.Text, out othCharge))
                        //{
                        //    CalculateOtherChargeTax();
                        //}
                        DcsSplitSave();
                        GetFieldValues(ControlsEnum.TAXMYR);
                        SetFieldValues(ControlsEnum.TAXMYR);
                        break;
                    #endregion
                    #region TAXHEADERSPLITUP
                    case ActionsEnum.TAXHEADERSPLITUP:
                        decimal Totaltax = 0;
                        decimal RaiseNote = 0;
                        GridViewRow grdInvRow = (GridViewRow)((LinkButton)(sender)).Parent.Parent;
                        HiddenField hdfTotalHdrTax = (HiddenField)grdInvRow.FindControl("hdfTotalTax");
                        TextBox txtRaiseNote = (TextBox)grdInvRow.FindControl("txtNoteFor");
                        decimal.TryParse(txtRaiseNote.Text, out RaiseNote);
                        decimal.TryParse(hdfTotalHdrTax.Value, out Totaltax);
                        taxList = null;
                        if (Totaltax > 0 && RaiseNote > 0)
                        {
                            SetUIHeaderTaxView(grdInvRow);
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
                            hdfInvPK = (HiddenField)grdInvoiceList.Rows[RowIndex].FindControl("hdfInvoicePK");
                            HiddenField hdfItemIncluded = (HiddenField)grdInvoiceList.Rows[RowIndex].FindControl("hdfItemIncluded");
                            txtNoteFor = (TextBox)grdInvoiceList.Rows[RowIndex].FindControl("txtNoteFor");
                            InvoicePK = Convert.ToInt64(hdfInvPK.Value);
                            decimal raiseNote = 0;
                            decimal splitTotal = 0;
                            decimal.TryParse(txtNoteFor.Text.Replace(",", ""), out raiseNote);
                            if (raiseNote > 0)
                            {
                                List<FIN_CRDR_NOTE_DTL> finCrDrDtlObj = new List<FIN_CRDR_NOTE_DTL>();
                                tempInvoiceSOSplitList = CrDrSplitList;
                                // finCrDrCusMpgList = (List<FIN_CRDR_NOTE_DTL>)SetUIValuesToObject(ControlsEnum.CRDRSPLITLIST);
                                finCrDrDtlObj = tempInvoiceSOSplitList.Where(ivh => ivh.FIN_CRDR_NOTE_MPG.CDM_INVOICE_VND_HDR == InvoicePK).ToList();



                                if (finCrDrDtlObj != null)
                                {
                                    splitTotal = finCrDrDtlObj.Sum(splt => splt.CDS_AMOUNT);
                                    if (splitTotal != 0)  //raiseNote != splitTotal &&
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
                                                finCrdrDtl.CDS_IS_AFFECT_STK = (byte)0;
                                                tempInvoiceSOSplitList.Add(finCrdrDtl);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        finCrDrDtlObj.ForEach(dtl =>
                                        {
                                            dtl.CDS_IS_AFFECT_STK = (byte)0;
                                        });
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
                                finCrDrCusMpgList = tempInvoiceSOSplitList.Where(dtl => dtl.FIN_CRDR_NOTE_MPG.CDM_INVOICE_VND_HDR == InvoicePK).ToList();
                                if (tempInvoiceSOSplitList.Where(r => r.CDS_IS_AFFECT_STK == 1).Count() > 0)
                                    chkAffectStock.Checked = true;
                                else
                                    chkAffectStock.Checked = false;
                                //GetFieldValues(ControlsEnum.INVITEMLIST);
                                //SetFieldValues(ControlsEnum.CRDRSPLITLIST);
                                GetFieldValues(ControlsEnum.TAXMYR);
                                SetFieldValues(ControlsEnum.TAXMYR);
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
                        if (((LinkButton)sender).CommandArgument.ToString() != null)
                        {
                            GridViewRow grdRow = (GridViewRow)((LinkButton)(sender)).Parent.Parent;
                            HiddenField hdfInvoiceGrp = (HiddenField)grdRow.FindControl("hdfInvoiceGrp");
                            int Pk = Convert.ToInt32(((HiddenField)grdRow.FindControl("hdfInvoicePK")).Value);
                            if (Convert.ToInt32(hdfInvoiceGrp.Value) == (int)POInvoiceGroup.Expense)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + Pk.ToString() + "&APPTYPE=" + ApplicationType.EI + "&APPSUBTYPE=") + "');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ((LinkButton)sender).CommandArgument.ToString() + "&APPTYPE=" + ApplicationType.PI + "&APPSUBTYPE=1") + "');", true);
                            }

                        }
                        break;
                    #endregion;
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

                    #region STOCK UPDATION
                    case ActionsEnum.STOCKUPDATION:
                        GridViewRow gvrCrDrHdr = ((Button)sender).Parent.Parent as GridViewRow;
                        HiddenField HdfCrDrPk = gvrCrDrHdr.FindControl("hdfCrDrPk") as HiddenField;
                        HiddenField hdfCrDrType = gvrCrDrHdr.FindControl("hdfCreditDebitType") as HiddenField;
                        CrDrPk = Convert.ToInt64(HdfCrDrPk.Value);
                        GetFieldValues(ControlsEnum.PODEPT);
                        if (Convert.ToInt16(hdfCrDrType.Value) == (int)DebitCreditModeEnum.DEBIT)
                            Response.Redirect(GetLocalResourceObject("StkUpdationUrl_EMI").ToString() + HdfCrDrPk.Value + "&Dep=" + PoDept.ToString());
                        else
                            Response.Redirect(GetLocalResourceObject("StkUpdationUrl_EMR").ToString() + HdfCrDrPk.Value + "&Dep=" + PoDept.ToString());
                        break;
                    #endregion

                    #region PRINTCRDRNOTE
                    case ActionsEnum.PRINTCRDRNOTE:
                        HiddenField hdfinvPK = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfListinvPK"));
                        HiddenField hdfinvType = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfListInvType"));  //To Sep: type(Dom,Exp,Per)
                        HiddenField hdfinvCategory = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfListinvCategory"));  //To sep: Adv Inv & Inv
                        HiddenField hdfinvGroup = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfListGroup")); //Group 3(Misc Inv)

                        if (Convert.ToInt32(hdfinvGroup.Value) == (int)POInvoiceGroup.Expense)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfinvPK.Value.ToString() + "&APPTYPE=" + ApplicationType.EI + "&APPSUBTYPE=") + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfinvPK.Value.ToString() + "&APPTYPE=" + ApplicationType.PI + "&APPSUBTYPE=1") + "');", true);
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
                finCrDrHdrNoteServiceClient = null;
                CommonServiceClient = null;

            }
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
            //HiddenField hdfTaxAmt = (HiddenField)grdRowLineItem.FindControl("hdfTAXSplitTotal");
            //hdftax = hdfTaxAmt.Value != string.Empty ? Convert.ToDecimal(hdfTaxAmt.Value) : 0;
            TextBox txtTaxAmt = (TextBox)grdRowLineItem.FindControl("txtTAXSplitTotal");
            hdftax = txtTaxAmt.Text != string.Empty ? Convert.ToDecimal(txtTaxAmt.Text) : 0;
            HiddenField hdfTotalAmt = (HiddenField)grdRowLineItem.FindControl("hdfSumSplit");
            hdftotalamt = hdfTotalAmt.Value != string.Empty ? Convert.ToDecimal(hdfTotalAmt.Value) : 0;
            taxpercentage = hdftax / (hdftotalamt == 0 ? 1 : hdftotalamt);
            basevalue = (Convert.ToDecimal(txtAmount.Text)) / (1 + taxpercentage);
            ttaxamt = (Convert.ToDecimal(txtAmount.Text) - basevalue);


            //LinkButton lbnTAXSplitTotal = (LinkButton)grdRowLineItem.FindControl("lbnTAXSplitTotal");
            //TextBox txtTAXSplitTotal = (TextBox)grdRowLineItem.FindControl("txtTAXSplitTotal");
            HiddenField hdfInvCusDtlPK = (HiddenField)grdRowLineItem.FindControl("hdfInvCusDtlPK");
            if (!string.IsNullOrEmpty(txtTaxAmt.Text.Trim()) && Convert.ToDecimal(txtTaxAmt.Text) > 0)//Convert.ToDecimal(hdfTaxAmt.Value) > 0
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
                    decimal InvAmount = objInvoiceItemDetails[0].VID_AMOUNT;
                    decimal TotalTaxPercentage = 1;
                    List<FIN_INVOICE_VND_TAX_DTL> objInvoiceTaxList = objInvoiceItemDetails[0].FIN_INVOICE_VND_TAX_DTL.Where(inv => inv.VTL_TAX_CATEGORY == (byte)TaxType.Tax).ToList();
                    if (objInvoiceTaxList != null && objInvoiceTaxList.Count > 0)
                    {
                        TotalTaxPercentage = objInvoiceTaxList.Sum(tx => (tx.VTL_TAX_AMT * 100) / InvAmount);
                        foreach (FIN_INVOICE_VND_TAX_DTL invtaxdet in objInvoiceTaxList)
                        {
                            SOInvoiceTaxHdr ObjTaxDtl = new SOInvoiceTaxHdr();
                            decimal IndividualPercentage = (invtaxdet.VTL_TAX_AMT * 100) / InvAmount;
                            //decimal TaxAmnt = (invtaxdethdr.CDS_TAX * IndividualPercentage) / TotalTaxPercentage;
                            decimal tAxSplitTotalAmount = Convert.ToDecimal(hdftax);
                            decimal TaxAmnt = (tAxSplitTotalAmount * IndividualPercentage) / TotalTaxPercentage;
                            double Amount = Math.Round(Convert.ToDouble(TaxAmnt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                            ObjTaxDtl.CIT_TAX = Convert.ToInt32(invtaxdet.VTL_TAX);
                            ObjTaxDtl.CIT_TAX_AMT = Amount;
                            ObjTaxDtl.CIT_TAX_CATEGORY = invtaxdet.VTL_TAX_CATEGORY;
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
                }
            }
        }

        private void SetUIHeaderTaxView(GridViewRow grdInvRow)
        {
            taxList = new List<SOInvoiceTaxHdr>();
            List<SOInvoiceTaxHdr> taxListTemp = new List<SOInvoiceTaxHdr>();
            decimal taxpercentage = 0;
            decimal basevalue = 0;
            decimal ttaxamt = 0;
            decimal hdftax = 0;
            decimal hdftotalamt = 0;
            int ItemIncluded = 0;
            decimal discount = 0;
            decimal othercharges = 0;
            if (hdfisDiscountAdd.Value == "1")
            {
                HiddenField hdfDiscoutntTotalSplitApplay = (HiddenField)grdInvRow.FindControl("hdfDiscoutntTotalSplitApplay");
                decimal.TryParse(hdfDiscoutntTotalSplitApplay.Value, out discount);
            }

            finCrDrNoteMpgObj = CommonFunctions.Initilize<FIN_CRDR_NOTE_MPG>();
            HiddenField hdfCrDbMpgPK = (HiddenField)grdInvRow.FindControl(GetLocalResourceObject("hdfCrDbMpgPK").ToString());
            HiddenField hdfItemIncluded = (HiddenField)grdInvRow.FindControl("hdfItemIncluded");
            HiddenField hdfLineItemTax = (HiddenField)grdInvRow.FindControl("hdfLineItemTax");
            HiddenField hdfHdrTax = (HiddenField)grdInvRow.FindControl("hdfHdrTax");
            TextBox txtOtherCharges = (TextBox)grdInvRow.FindControl("txtOtherCharges");

            decimal.TryParse(txtOtherCharges.Text, out othercharges);

            finCrDrNoteMpgObj.CDM_PK = hdfCrDbMpgPK == null ? 0 : Convert.ToInt64(hdfCrDbMpgPK.Value);
            finCrDrNoteMpgObj.CDM_CRDR_NOTE_HDR = CurrPK;
            HiddenField hdfInvoicePK = (HiddenField)grdInvRow.FindControl(GetLocalResourceObject("hdfInvoicePK").ToString());
            finCrDrNoteMpgObj.CDM_INVOICE_VND_HDR = hdfInvoicePK == null ? 0 : Convert.ToInt32(hdfInvoicePK.Value);
            finCrDrNoteMpgObj.CDM_INVOICE_CUS_HDR = null;
            TextBox txtAmount = (TextBox)grdInvRow.FindControl(GetLocalResourceObject("txtNoteFor").ToString());
            finCrDrNoteMpgObj.CDM_AMOUNT = Convert.ToDecimal(txtAmount.Text) - discount;
            finCrDrNoteMpgObj.CDM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);

            HiddenField hdfTaxAmt = (HiddenField)grdInvRow.FindControl("hdfTaxAmt");
            if (othercharges > 0)
                hdftax = hdfTaxAmt.Value != string.Empty ? Convert.ToDecimal(hdfTaxAmt.Value) : 0;
            else
                hdftax = hdfLineItemTax.Value != string.Empty ? Convert.ToDecimal(hdfLineItemTax.Value) : 0;

            HiddenField hdfTotalAmt = (HiddenField)grdInvRow.FindControl("hdfTotalAmt");
            int.TryParse(hdfItemIncluded.Value, out ItemIncluded);

            hdftotalamt = hdfTotalAmt.Value != string.Empty ? Convert.ToDecimal(hdfTotalAmt.Value) : 0;
            taxpercentage = hdftax / (hdftotalamt == 0 ? 1 : hdftotalamt);
            if (ItemIncluded == 1)
            {
                ttaxamt = (Convert.ToDecimal(txtAmount.Text) * taxpercentage);
            }
            else
            {
                basevalue = (Convert.ToDecimal(txtAmount.Text)) / (1 + taxpercentage);
                ttaxamt = (Convert.ToDecimal(txtAmount.Text) - basevalue);
            }

            //Label lblTotalTax = (Label)grdInvRow.FindControl(GetLocalResourceObject("lblTotalTax").ToString());
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
                        decimal InvAmount = 0;
                        decimal TotalTaxPercentage = 1;
                        decimal IndividualPercentage = 1;
                        decimal TaxAmnt = 0;
                        double Amount = 0;
                        SOInvoiceTaxHdr ObjTaxDtl;
                        if (objInvoiceDetails[0] != null)
                        {
                            //if (othercharges == 0)//If Other charge Not Added 
                            if (false)//If Other charge Not Added 
                            {
                               // lineItemTax = finInvoiceVndHdrList[e.Row.RowIndex].FIN_INVOICE_VND_DTL.Sum(tx => tx.VID_TAX);
                                InvAmount = objInvoiceDetails[0].IVH_AMOUNT_TC;
                                List<FIN_INVOICE_VND_DTL> lstinvDetalsList = objInvoiceDetails[0].FIN_INVOICE_VND_DTL.ToList();

                                List<FIN_INVOICE_VND_TAX_DTL> objInvoiceTaxDtlList = objInvoiceDetails[0].FIN_INVOICE_VND_DTL.ToList()[0].FIN_INVOICE_VND_TAX_DTL.Where(inv => inv.VTL_TAX_CATEGORY == (byte)TaxType.Tax).ToList();

                            
                            }//If Other charge Not Added
                            else
                            {
                                InvAmount = objInvoiceDetails[0].IVH_AMOUNT_TC;
                                List<FIN_INVOICE_VND_TAX_HDR> objInvoiceTaxList = objInvoiceDetails[0].FIN_INVOICE_VND_TAX_HDR.Where(inv => inv.VTH_TAX_CATEGORY == (byte)TaxType.Tax).ToList();


                                //   

                                if (objInvoiceTaxList != null && objInvoiceTaxList.Count > 0)
                                {
                                    TotalTaxPercentage = objInvoiceTaxList.Sum(tx => (tx.VTH_TAX_AMT * 100) / InvAmount);
                                    IndividualPercentage = 1;
                                    TaxAmnt = 0;
                                    Amount = 0;
                                    foreach (FIN_INVOICE_VND_TAX_HDR invtaxhdr in objInvoiceTaxList)
                                    {
                                        ObjTaxDtl = new SOInvoiceTaxHdr();
                                        IndividualPercentage = (invtaxhdr.VTH_TAX_AMT * 100) / InvAmount;
                                        TaxAmnt = (finCrDrNoteMpgObj.CDM_TAX_AMOUNT * IndividualPercentage) / TotalTaxPercentage;
                                        Amount = Math.Round(Convert.ToDouble(TaxAmnt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                        ObjTaxDtl.CIT_TAX = Convert.ToInt32(invtaxhdr.VTH_TAX);
                                        ObjTaxDtl.CIT_TAX_AMT = Amount;
                                        ObjTaxDtl.CIT_TAX_CATEGORY = invtaxhdr.VTH_TAX_CATEGORY;
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
                        if (finCrDrCusMpgList.Where(r => r.CDS_IS_AFFECT_STK == 1).Count() > 0)
                            chkAffectStock.Checked = true;
                        else
                            chkAffectStock.Checked = false;
                        tempInvoiceSOSplitList = CrDrSplitList;
                        tempInvoiceSOSplitList.Where(dtl => dtl.FIN_CRDR_NOTE_MPG.CDM_INVOICE_VND_HDR == InvoicePK)
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
                                CDM_INVOICE_VND_HDR = InvoicePK
                            };
                            ////////
                            if (dtl.CDS_TAX > 0)
                            {
                                InvItemPk = Convert.ToInt64(dtl.CDS_INVOICE_VND_DTL);
                                GetFieldValues(ControlsEnum.INVOICEITEMDETAILS);
                                if (objInvoiceItemDetails != null && objInvoiceItemDetails.Count > 0)
                                {
                                    decimal InvAmount = objInvoiceItemDetails[0].VID_AMOUNT;
                                    decimal TotalTaxPercentage = 1;
                                    List<FIN_INVOICE_VND_TAX_DTL> objInvoiceTaxList = objInvoiceItemDetails[0].FIN_INVOICE_VND_TAX_DTL.Where(inv => inv.VTL_TAX_CATEGORY == (byte)TaxType.Tax).ToList();
                                    if (objInvoiceTaxList != null && objInvoiceTaxList.Count > 0)
                                    {
                                        TotalTaxPercentage = objInvoiceTaxList.Sum(tx => (tx.VTL_TAX_AMT * 100) / InvAmount);
                                        foreach (FIN_INVOICE_VND_TAX_DTL invtaxdet in objInvoiceTaxList)
                                        {
                                            FIN_CRDR_NOTE_TAX_DTL ObjTaxDtl = new FIN_CRDR_NOTE_TAX_DTL();
                                            decimal IndividualPercentage = (invtaxdet.VTL_TAX_AMT * 100) / InvAmount;
                                            decimal TaxAmnt = (dtl.CDS_TAX * IndividualPercentage) / TotalTaxPercentage;
                                            double Amount = CommonFunctions.DoubleFormatRound(Convert.ToDouble(TaxAmnt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                            ObjTaxDtl.NTD_AMOUNT = Convert.ToDecimal(Amount);
                                            ObjTaxDtl.NTD_PK = 0;
                                            ObjTaxDtl.NTD_TAX = invtaxdet.VTL_TAX;
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
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                }

            }
        }

        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            Label lblTotalFooter;
            decimal total;
            decimal taxpercentage;
            decimal basevalue;
            decimal taxamt;
            int InvGroup = 0;
            HiddenField hdfTotalAmt;
            HiddenField hdfTaxAmt;
            HiddenField hdfInvoicePK;
            HiddenField hdfhasjournalized;
            HiddenField hdfCrDbMpgPK;

            HiddenField hdfHdrTax;
            HiddenField hdfLineItemTax;
            //HiddenField hdfInvoiceCurr;
            //Label lblInvoiceNo;
            LinkButton lnkInvoiceNo;
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
            TextBox txtOtherCharges;
            Button lnkRemove;
            Label lblAdjAmount;
            Label lblTotalTax;
            LinkButton lbnTotalTax;

            //Split 
            HiddenField hdfInvCusDtlPK;
            HiddenField hdfActualDiscountSplit;
            TextBox txtTotalSplit;
            Label lblPRODUCTSplit;

            Label lblQTYSplit;
            Label lblActualDiscountSplit;
            Label lblRATESplit;

            Label lblAmountSplit;
            Label lblTAXSplit;
            Label lblPONo;
            Label lblTAXSplitTotal;
            LinkButton lbnTAXSplitTotal;
            TextBox txtTAXSplitTotal;
            Label lblNETSplit;
            TextBox txtPayNowSplit;
            HiddenField hdfTAXSplit;
            HiddenField hdfSumSplit;
            HiddenField hdfReceiptSplitPK;
            //HiddenField hdfhasjournalized;
            HiddenField hdfReceiptTRXPK;
            HiddenField hdfTAXSplitTotal;
            HiddenField hdfNETSplit;
            Label lblTotalTaxFooterSplit;
            HiddenField hdfTotalTaxFooterSplit;
            Label lblTotalPayNowFooterSplit;
            Label lblTotalDiscountFooterSplit;
            HiddenField hdfTotalDiscountFooterSplit;
            Button lnkAllocation;
            HiddenField hdfTotalPayNowFooterSplit;
            TextBox txtQtySplit;
            TextBox txtRateSplit;
            TextBox txtSumSplit;

            CheckBox chkAffectStkSplit;

            SPADM_APP_STATUS_CFG_GET_KV_Result wkfStatus;
            FIN_CRDR_NOTE_DTL tempFinReceiptCusSoMpgObj = null;

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    if (((GridView)sender).ID == "grdInvoiceList")
                    {
                        hdfTotalAmt = e.Row.FindControl("hdfTotalAmt") as HiddenField;
                        hdfHdrTax = e.Row.FindControl("hdfHdrTax") as HiddenField;
                        hdfLineItemTax = e.Row.FindControl("hdfLineItemTax") as HiddenField;

                        hdfTaxAmt = e.Row.FindControl("hdfTaxAmt") as HiddenField;
                        hdfInvoicePK = e.Row.FindControl("hdfInvoicePK") as HiddenField;
                        hdfCrDbMpgPK = e.Row.FindControl("hdfCrDbMpgPK") as HiddenField;
                        hdfhasjournalized = e.Row.FindControl("hdfhasjournalized") as HiddenField;
                        //lblInvoiceNo = e.Row.FindControl("lblInvoiceNo") as Label;
                        lnkInvoiceNo = e.Row.FindControl("lnkInvoiceNo") as LinkButton;
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
                        HiddenField hdfIsDetailTaxExist = e.Row.FindControl("hdfIsDetailTaxExist") as HiddenField;
                        HiddenField hdfInvoiceGrp = e.Row.FindControl("hdfInvoiceGrp") as HiddenField;
                        txtOtherCharges = e.Row.FindControl("txtOtherCharges") as TextBox;


                        decimal lineItemTax = 0;
                        decimal lineItemDiscount = 0;

                        if (hdfisDiscountAdd.Value == "1")
                        {
                            grdInvoiceList.Columns[(int)InvoiceColumn.OthreChages].Visible = false;//need to change after complete the work
                            grdInvoiceList.Columns[(int)InvoiceColumn.ActualOtherchage].Visible = true;
                        }
                        else
                        {
                            grdInvoiceList.Columns[(int)InvoiceColumn.OthreChages].Visible = false;
                            grdInvoiceList.Columns[(int)InvoiceColumn.ActualOtherchage].Visible = false;
                        }

                        if (finInvoiceVndHdrList != null && finInvoiceVndHdrList.Count > 0)
                        {
                            if (finInvoiceVndHdrList[e.Row.RowIndex].FIN_INVOICE_VND_DTL != null && finInvoiceVndHdrList[e.Row.RowIndex].FIN_INVOICE_VND_DTL.Count > 0)
                            {
                                lineItemTax = finInvoiceVndHdrList[e.Row.RowIndex].FIN_INVOICE_VND_DTL.Sum(tx => tx.VID_TAX);
                                lineItemDiscount = finInvoiceVndHdrList[e.Row.RowIndex].FIN_INVOICE_VND_DTL.Sum(tx => tx.VID_DISCOUNT);
                            }
                            if (lineItemTax > 0)
                                hdfIsDetailTaxExist.Value = "1";
                            else
                                hdfIsDetailTaxExist.Value = "0";
                            hdfInvoiceCategory.Value = finInvoiceVndHdrList[0].IVH_CATEGORY.ToString();
                            lblCustomerTxt.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceVndHdrList[0].PUR_VENDOR_MST.VEN_NAME, 45);
                            lblCustomerTxt.ToolTip = finInvoiceVndHdrList[0].PUR_VENDOR_MST.VEN_NAME;
                            hdfCusPK.Value = finInvoiceVndHdrList[0].IVH_VENDOR.ToString();
                            txtPaymentCurrency.Text = finInvoiceVndHdrList[0].ADM_CURRENCY_MST1.CUR_CODE;
                            hdfPaymentCurrency.Value = finInvoiceVndHdrList[0].IVH_CURRENCY.ToString();


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


                            // hdfTotalAmt.Value = (finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_TC - finInvoiceVndHdrList[e.Row.RowIndex].IVH_DISCOUNT_TC).ToString();
                            ////hdfTotalAmt.Value = (finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_NET_TC - finInvoiceVndHdrList[e.Row.RowIndex].IVH_TAX_TC - finInvoiceVndHdrList[e.Row.RowIndex].IVH_DISCOUNT_TC).ToString();
                            //hdfTotalAmt.Value = (finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_NET_TC - (finInvoiceVndHdrList[e.Row.RowIndex].IVH_TAX_TC + lineItemTax) - (finInvoiceVndHdrList[e.Row.RowIndex].IVH_DISCOUNT_TC + lineItemDiscount)).ToString();

                            //if (finInvoiceVndHdrList[e.Row.RowIndex].IVH_TYPE == Convert.ToInt32(PurchaseType.Local))
                            //{
                            //    hdfTotalAmt.Value = (finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_NET_TC - (finInvoiceVndHdrList[e.Row.RowIndex].IVH_TAX_TC + lineItemTax) - finInvoiceVndHdrList[e.Row.RowIndex].IVH_SHIP_CHARGE - finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_ADJUST).ToString(); //Removed advance deduct
                            //}
                            //else
                            //{
                            //    hdfTotalAmt.Value = (finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_NET_TC - (finInvoiceVndHdrList[e.Row.RowIndex].IVH_TAX_TC + lineItemTax) - finInvoiceVndHdrList[e.Row.RowIndex].IVH_SHIP_CHARGE - finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_ADJUST + finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_ADV_DED_TC).ToString();
                            //}  
                            if (IsAdvInvHasTax)
                            {
                                if (IsTaxForOtherCharge.Value == "1")
                                {
                                    hdfTotalAmt.Value = (finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_NET_TC - (finInvoiceVndHdrList[e.Row.RowIndex].IVH_TAX_TC + lineItemTax) - finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_ADJUST).ToString(); //Removed advance deduct for both local and import
                                }
                                else
                                {
                                    hdfTotalAmt.Value = (finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_NET_TC - (finInvoiceVndHdrList[e.Row.RowIndex].IVH_TAX_TC + lineItemTax) - finInvoiceVndHdrList[e.Row.RowIndex].IVH_SHIP_CHARGE - finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_ADJUST).ToString(); //Removed advance deduct for both local and import
                                }
                            }
                            else
                            {
                                if (IsTaxForOtherCharge.Value == "1")
                                {
                                    hdfTotalAmt.Value = (finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_NET_TC - (finInvoiceVndHdrList[e.Row.RowIndex].IVH_TAX_TC + lineItemTax) - finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_ADJUST + finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_ADV_DED_TC).ToString(); //Removed advance deduct for both local and import
                                }
                                else
                                {
                                    hdfTotalAmt.Value = (finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_NET_TC - (finInvoiceVndHdrList[e.Row.RowIndex].IVH_TAX_TC + lineItemTax) - finInvoiceVndHdrList[e.Row.RowIndex].IVH_SHIP_CHARGE - finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_ADJUST + finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_ADV_DED_TC).ToString(); //Removed advance deduct for both local and import
                                }
                            }

                            //hdfTaxAmt.Value = finInvoiceVndHdrList[e.Row.RowIndex].IVH_TAX_TC.ToString() == string.Empty ? "0" : finInvoiceVndHdrList[e.Row.RowIndex].IVH_TAX_TC.ToString();

                            hdfInvoicePK.Value = finInvoiceVndHdrList[e.Row.RowIndex].IVH_PK.ToString();
                            hdfhasjournalized.Value = finInvoiceVndHdrList[e.Row.RowIndex].IVH_HAS_JRNL_ENTRY.ToString();
                            hdfCrDbMpgPK.Value = "0";
                            lnkInvoiceNo.Text = finInvoiceVndHdrList[e.Row.RowIndex].IVH_NO;
                            lnkInvoiceNo.ToolTip = finInvoiceVndHdrList[e.Row.RowIndex].IVH_NO;
                            lnkInvoiceNo.CommandArgument = finInvoiceVndHdrList[e.Row.RowIndex].IVH_PK.ToString();
                            hdfInvoiceGrp.Value = finInvoiceVndHdrList[e.Row.RowIndex].IVH_GROUP.ToString();
                            //lblInvoiceNo.Text = finInvoiceVndHdrList[e.Row.RowIndex].IVH_NO;
                            //lblInvoiceNo.ToolTip = finInvoiceVndHdrList[e.Row.RowIndex].IVH_NO;
                            lblInvoiceDate.Text = finInvoiceVndHdrList[e.Row.RowIndex].IVH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblInvoiceDate.ToolTip = finInvoiceVndHdrList[e.Row.RowIndex].IVH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblVendorInv.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceVndHdrList[e.Row.RowIndex].PUR_VENDOR_MST.VEN_NAME, 15);
                            lblVendorInv.ToolTip = finInvoiceVndHdrList[e.Row.RowIndex].PUR_VENDOR_MST.VEN_NAME;

                            //lblGrossAmount.Text = String.Format("{0:c}", finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_TC);
                            //lblGrossAmount.ToolTip = String.Format("{0:c}", finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_TC);
                            lblGrossAmount.Text = String.Format("{0:c}", Convert.ToDecimal(hdfTotalAmt.Value));
                            lblGrossAmount.ToolTip = String.Format("{0:c}", Convert.ToDecimal(hdfTotalAmt.Value));


                            hdfTaxAmt.Value = (finInvoiceVndHdrList[e.Row.RowIndex].IVH_TAX_TC + lineItemTax).ToString();
                            lblTax.Text = String.Format("{0:c}", finInvoiceVndHdrList[e.Row.RowIndex].IVH_TAX_TC + lineItemTax);
                            lblTax.ToolTip = String.Format("{0:c}", finInvoiceVndHdrList[e.Row.RowIndex].IVH_TAX_TC + lineItemTax);
                            hdfShipCharge.Value = finInvoiceVndHdrList[e.Row.RowIndex].IVH_SHIP_CHARGE.ToString();

                            hdfLineItemTax.Value = lineItemTax.ToString();
                            hdfHdrTax.Value = finInvoiceVndHdrList[e.Row.RowIndex].IVH_TAX_TC.ToString();

                            lblDiscount.Text = String.Format("{0:c}", finInvoiceVndHdrList[e.Row.RowIndex].IVH_DISCOUNT_TC + lineItemDiscount);
                            lblDiscount.ToolTip = String.Format("{0:c}", finInvoiceVndHdrList[e.Row.RowIndex].IVH_DISCOUNT_TC + lineItemDiscount);

                            lblOtherAmount.Text = lblOtherAmount.ToolTip = String.Format("{0:c}", finInvoiceVndHdrList[e.Row.RowIndex].IVH_SHIP_CHARGE);

                            if (IsAdvInvHasTax)
                            {
                                lblTotalAmount.Text = String.Format("{0:c}", finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_NET_TC);
                                lblTotalAmount.ToolTip = String.Format("{0:c}", finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_NET_TC);
                            }
                            else
                            {
                                decimal InvAmountNetValue = 0;
                                decimal GrossAmnt = 0;
                                decimal.TryParse(hdfTotalAmt.Value, out GrossAmnt);
                                InvAmountNetValue = GrossAmnt + finInvoiceVndHdrList[e.Row.RowIndex].IVH_TAX_TC + lineItemTax
                                                    + finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_ADJUST;
                                lblTotalAmount.Text = lblTotalAmount.ToolTip = String.Format("{0:c}", Math.Round(InvAmountNetValue, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                            }

                            //lblTotalAmount.Text = String.Format("{0:c}", ((finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_TC - finInvoiceVndHdrList[e.Row.RowIndex].IVH_DISCOUNT_TC) + finInvoiceVndHdrList[e.Row.RowIndex].IVH_TAX_TC));
                            //lblTotalAmount.ToolTip = lblTotalAmount.Text;
                            lblPaid.Text = String.Format("{0:c}", finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_PAID_TC);
                            lblPaid.Text = String.Format("{0:c}", finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_PAID_TC);
                            lblPaid.ToolTip = String.Format("{0:c}", finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_PAID_TC);

                            decimal balToPay = finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_NET_TC -
                                              finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_PAID_TC +
                                              finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_CN_TC -
                                              finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_DN_TC;

                            lblBaltopay.Text = String.Format(Thread.CurrentThread.CurrentCulture, "{0:c}", balToPay);
                            lblBaltopay.ToolTip = String.Format(Thread.CurrentThread.CurrentCulture, "{0:c}", balToPay);
                            balToPay = balToPay < 0 ? 0 : balToPay;

                            if (dicTempAmount != null && dicTempAmount.Count > 0 && dicTempAmount.ContainsKey(hdfInvoicePK.Value))
                            {
                                txtNoteFor.Text = dicTempAmount.SingleOrDefault(aa => aa.Key == hdfInvoicePK.Value).Value.ToString();
                            }
                            else
                            {
                                //txtNoteFor.ToolTip = txtNoteFor.Text = Math.Round(balToPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                txtNoteFor.ToolTip = txtNoteFor.Text = String.Format("{0:c}", Convert.ToDecimal(0));
                            }


                            lnkRemove.CommandArgument = finInvoiceVndHdrList[e.Row.RowIndex].IVH_PK.ToString();
                            lblAdjAmount.Text = String.Format("{0:c}", finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_CN_TC - finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_DN_TC);

                            taxpercentage = Convert.ToDecimal(hdfTaxAmt.Value) / (Convert.ToDecimal(finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_TC - finInvoiceVndHdrList[e.Row.RowIndex].IVH_DISCOUNT_TC + lineItemDiscount) == 0 ? 1 : Convert.ToDecimal(finInvoiceVndHdrList[e.Row.RowIndex].IVH_AMOUNT_TC - finInvoiceVndHdrList[e.Row.RowIndex].IVH_DISCOUNT_TC + lineItemDiscount));
                            basevalue = (txtNoteFor.Text != string.Empty ? Convert.ToDecimal(txtNoteFor.Text) : 0) / (1 + taxpercentage);
                            taxamt = ((txtNoteFor.Text != string.Empty ? Convert.ToDecimal(txtNoteFor.Text) : 0) - basevalue);
                            //lblTotalTax.Text = Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //lblTotalTax.ToolTip = lblTotalTax.Text;
                            lbnTotalTax.Text = GetFormattedCurrencyWithSeperator(Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                            lbnTotalTax.ToolTip = lbnTotalTax.Text;
                            hdfTotalTax.Value = Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            if (finInvoiceVndHdrList[e.Row.RowIndex].IVH_GROUP == (int)POInvoiceGroup.Expense && lineItemTax > 0)
                            {
                                lbnTotalTax.Enabled = false;
                                lbnTotalTax.Attributes.Add("class", "nomargin");
                            }
                            else
                            {
                                lbnTotalTax.Enabled = true;
                                lbnTotalTax.Attributes.Add("class", "text-underline nomargin");
                            }


                            hdfInvoiceType.Value = finInvoiceVndHdrList[e.Row.RowIndex].IVH_TYPE.ToString();
                            InvType = Convert.ToInt32(hdfInvoiceType.Value);

                        }
                        else if (finCrDrNoteMpgList != null && finCrDrNoteMpgList.Count > 0)
                        {
                            if (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_DTL != null && finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_DTL.Count > 0)
                            {
                                lineItemTax = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_DTL.Sum(tx => tx.VID_TAX);
                                lineItemDiscount = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.FIN_INVOICE_VND_DTL.Sum(tx => tx.VID_DISCOUNT);

                            }
                            if (lineItemTax > 0)
                                hdfIsDetailTaxExist.Value = "1";
                            else
                                hdfIsDetailTaxExist.Value = "0";

                            hdfInvoiceCategory.Value = finCrDrNoteMpgList[0].FIN_INVOICE_VND_HDR.IVH_CATEGORY.ToString();
                            hdfShipCharge.Value = finCrDrNoteMpgList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.CDH_SHIP_CHARGE.ToString();

                            txtOtherCharges.Text = txtOtherCharges.ToolTip = String.Format("{0:c}", finCrDrNoteMpgList[0].CDM_OTHER_CHARGE);
                            lblCustomerTxt.Text = ERP.Utilities.CommonFunctions.GetShortString(finCrDrNoteMpgList[0].FIN_INVOICE_VND_HDR.PUR_VENDOR_MST.VEN_NAME, 45);
                            lblCustomerTxt.ToolTip = finCrDrNoteMpgList[0].FIN_INVOICE_VND_HDR.PUR_VENDOR_MST.VEN_NAME;
                            hdfCusPK.Value = finCrDrNoteMpgList[0].FIN_INVOICE_VND_HDR.IVH_VENDOR.ToString();
                            //ddlCompany.SelectedValue = finInvoiceCusHdrList[0].ICH_COMPANY.ToString();
                            txtPaymentCurrency.Text = finCrDrNoteMpgList[0].FIN_INVOICE_VND_HDR.ADM_CURRENCY_MST1.CUR_CODE;
                            hdfPaymentCurrency.Value = finCrDrNoteMpgList[0].FIN_INVOICE_VND_HDR.IVH_CURRENCY.ToString();
                            //hdfTotalAmt.Value = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_TC - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_DISCOUNT_TC).ToString();
                            ////hdfTotalAmt.Value = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_DISCOUNT_TC).ToString();
                            //hdfTotalAmt.Value = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC - (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC + lineItemTax) - (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_DISCOUNT_TC + lineItemDiscount)).ToString();

                            //if (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TYPE == Convert.ToInt32(PurchaseType.Local))
                            //{
                            //    hdfTotalAmt.Value = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC - (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC + lineItemTax) - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_SHIP_CHARGE - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_ADJUST).ToString();  // Removed advance deduction
                            //}
                            //else
                            //{
                            //    hdfTotalAmt.Value = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC - (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC + lineItemTax) - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_SHIP_CHARGE - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_ADJUST + finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_ADV_DED_TC).ToString();
                            //}
                            if (IsAdvInvHasTax)
                            {
                                if (IsTaxForOtherCharge.Value == "1")// // OtherCharge is needed for Tax Calculation            
                                {
                                    hdfTotalAmt.Value = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC - (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC + lineItemTax) - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_ADJUST).ToString();  // Removed advance deduction for both local and import.
                                }
                                else
                                {
                                    hdfTotalAmt.Value = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC - (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC + lineItemTax) - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_SHIP_CHARGE - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_ADJUST).ToString();  // Removed advance deduction for both local and import.
                                }
                            }
                            else
                            {
                                if (IsTaxForOtherCharge.Value == "1")// // OtherCharge is needed for Tax Calculation            
                                {
                                    hdfTotalAmt.Value = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC - (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC + lineItemTax) - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_ADJUST + finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_ADV_DED_TC).ToString();
                                }
                                else
                                {
                                    hdfTotalAmt.Value = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC - (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC + lineItemTax) - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_SHIP_CHARGE - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_ADJUST + finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_ADV_DED_TC).ToString();
                                }
                            }


                            //hdfTaxAmt.Value = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC.ToString() == string.Empty ? "0" : finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC.ToString();

                            hdfInvoicePK.Value = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_PK.ToString();
                            hdfhasjournalized.Value = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_HAS_JRNL_ENTRY.ToString();
                            hdfCrDbMpgPK.Value = finCrDrNoteMpgList[e.Row.RowIndex].CDM_PK.ToString();
                            //lblInvoiceNo.Text = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_NO;
                            //lblInvoiceNo.ToolTip = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_NO;
                            lnkInvoiceNo.Text = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_NO;
                            lnkInvoiceNo.ToolTip = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_NO;
                            lnkInvoiceNo.CommandArgument = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_PK.ToString();
                            hdfInvoiceGrp.Value = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_GROUP.ToString();

                            lblInvoiceDate.Text = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblInvoiceDate.ToolTip = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblVendorInv.Text = ERP.Utilities.CommonFunctions.GetShortString(finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.PUR_VENDOR_MST.VEN_NAME, 15);
                            lblVendorInv.ToolTip = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.PUR_VENDOR_MST.VEN_NAME;

                            //lblGrossAmount.Text = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_TC);
                            //lblGrossAmount.ToolTip = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_TC);
                            lblGrossAmount.Text = String.Format("{0:c}", Convert.ToDecimal(hdfTotalAmt.Value));
                            lblGrossAmount.ToolTip = String.Format("{0:c}", Convert.ToDecimal(hdfTotalAmt.Value));

                            hdfTaxAmt.Value = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC + lineItemTax).ToString();
                            lblTax.Text = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC + lineItemTax);
                            lblTax.ToolTip = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC + lineItemTax);

                            hdfLineItemTax.Value = lineItemTax.ToString();
                            hdfHdrTax.Value = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC.ToString();

                            lblDiscount.Text = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_DISCOUNT_TC + lineItemDiscount);
                            lblDiscount.ToolTip = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_DISCOUNT_TC + lineItemDiscount);

                            lblOtherAmount.Text = lblOtherAmount.ToolTip = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_SHIP_CHARGE);

                            if (IsAdvInvHasTax)
                            {
                                lblTotalAmount.Text = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC);
                                lblTotalAmount.ToolTip = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC);
                            }
                            else
                            {
                                decimal InvAmountNetValue = 0;
                                decimal GrossAmnt = 0;
                                decimal.TryParse(hdfTotalAmt.Value, out GrossAmnt);
                                InvAmountNetValue = GrossAmnt + finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC + lineItemTax
                                                    + finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_ADJUST;
                                lblTotalAmount.Text = lblTotalAmount.ToolTip = String.Format("{0:c}", Math.Round(InvAmountNetValue, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                            }


                            //lblTotalAmount.Text = String.Format("{0:c}", ((finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_TC - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_DISCOUNT_TC) + finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TAX_TC));
                            //lblTotalAmount.ToolTip = lblTotalAmount.Text;
                            decimal paid = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_PAID_TC;
                            lblPaid.Text = String.Format(Thread.CurrentThread.CurrentCulture, "{0:c}", paid);
                            lblPaid.ToolTip = String.Format(Thread.CurrentThread.CurrentCulture, "{0:c}", paid);

                            if (dicTempAmount != null && dicTempAmount.Count > 0 && dicTempAmount.ContainsKey(hdfInvoicePK.Value))
                            {
                                txtNoteFor.Text = dicTempAmount.SingleOrDefault(aa => aa.Key == hdfInvoicePK.Value).Value.ToString();
                            }
                            else
                            {
                                txtNoteFor.ToolTip = txtNoteFor.Text = Math.Round(finCrDrNoteMpgList[e.Row.RowIndex].CDM_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            }
                            if (finCrDrNoteMpgList[e.Row.RowIndex].FIN_CRDR_NOTE_DTL != null && finCrDrNoteMpgList[e.Row.RowIndex].FIN_CRDR_NOTE_DTL.Sum(dtl => dtl.CDS_AMOUNT) > 0)
                            {
                                hdfItemIncluded.Value = "1";
                                hdfSplitCount.Value = "1";

                                txtNoteFor.Text = Math.Round(finCrDrNoteMpgList[e.Row.RowIndex].CDM_AMOUNT - finCrDrNoteMpgList[e.Row.RowIndex].CDM_TAX_AMOUNT,
                                  Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                txtNoteFor.ToolTip = Math.Round(finCrDrNoteMpgList[e.Row.RowIndex].CDM_AMOUNT - finCrDrNoteMpgList[e.Row.RowIndex].CDM_TAX_AMOUNT,
                                    Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            }
                            else
                            {
                                hdfItemIncluded.Value = "0";
                                hdfSplitCount.Value = "0";
                            }
                            decimal balToPay;

                            //commented for solving -ve/wrong balance issue
                            //if (finCrDrNoteMpgList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.CDH_TYPE == 1)
                            //    balToPay = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC -
                            //                       finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_PAID_TC +
                            //                       finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_CN_TC -
                            //                       finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_DN_TC); //+ (WkfStatus > 0 ? decimal.Parse(txtNoteFor.Text.Trim()) : 0);
                            //else
                            //    balToPay = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC -
                            //                  finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_PAID_TC +
                            //                  finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_CN_TC -
                            //                  finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_DN_TC) - (WkfStatus > 0 ? decimal.Parse(txtNoteFor.Text.Trim()) : 0);


                            if (finCrDrNoteMpgList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.CDH_TYPE == 1) // debit
                            {
                                balToPay = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC -
                                                   finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_PAID_TC +
                                                   finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_CN_TC -
                                                   finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_DN_TC); //+ (WkfStatus > 0 ? decimal.Parse(txtNoteFor.Text.Trim()) : 0);
                            }
                            else
                            {
                                //commented for solving -ve/wrong balance issue
                                //balToPay = (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_NET_TC -
                                //              finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_PAID_TC +
                                //              finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_CN_TC -
                                //              finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_DN_TC) - (WkfStatus > 0 ? decimal.Parse(txtNoteFor.Text.Trim()) : 0);
                                decimal CreditPaidAmnt = finCrDrNoteMpgList[e.Row.RowIndex].FIN_PAYMENT_VND_CRDR_MPG.Where(r => r.FIN_PAYMENT_VND_HDR.PVH_DEL_STATUS == 0 && r.FIN_PAYMENT_VND_HDR.PVH_BOUNCED == 0).Sum(sm => sm.PNM_ADJ_AMOUNT + sm.PNM_PAID_AMOUNT);
                                decimal InvPaidAmnt = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_PAID_TC - CreditPaidAmnt;
                                balToPay = InvPaidAmnt - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_PAID_TC + CreditPaidAmnt + (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_CN_TC - CreditPaidAmnt);
                            }

                            lblBaltopay.Text = String.Format(Thread.CurrentThread.CurrentCulture, "{0:c}", balToPay);
                            lblBaltopay.ToolTip = String.Format(Thread.CurrentThread.CurrentCulture, "{0:c}", balToPay);



                            lnkRemove.CommandArgument = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_PK.ToString();
                            //lblAdjAmount.Text = String.Format("{0:c}", finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_CN_TC - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_DN_TC);
                            if (finCrDrNoteMpgList[e.Row.RowIndex].FIN_CRDR_NOTE_HDR.CDH_TYPE == 1)
                                lblAdjAmount.Text = String.Format("{0:c}", (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_CN_TC - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_DN_TC) + (WkfStatus > 0 ? decimal.Parse(txtNoteFor.Text.Trim()) : 0));
                            else
                                lblAdjAmount.Text = String.Format("{0:c}", (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_CN_TC - finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_AMOUNT_DN_TC) - (WkfStatus > 0 ? decimal.Parse(txtNoteFor.Text.Trim()) : 0));

                            //lblTotalTax.Text = Math.Round(finCrDrNoteMpgList[e.Row.RowIndex].CDM_TAX_AMOUNT,
                            //   Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //lblTotalTax.ToolTip = lblTotalTax.Text;
                            lbnTotalTax.Text = GetFormattedCurrencyWithSeperator(Math.Round(finCrDrNoteMpgList[e.Row.RowIndex].CDM_TAX_AMOUNT,
                              Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                            lbnTotalTax.ToolTip = lbnTotalTax.Text;
                            hdfTotalTax.Value = Math.Round(finCrDrNoteMpgList[e.Row.RowIndex].CDM_TAX_AMOUNT,
                              Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            if (finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_GROUP == (int)POInvoiceGroup.Expense && lineItemTax > 0)
                            {
                                lbnTotalTax.Enabled = false;
                                lbnTotalTax.Attributes.Add("class", "nomargin");
                            }
                            else
                            {
                                lbnTotalTax.Enabled = true;
                                lbnTotalTax.Attributes.Add("class", "text-underline nomargin");
                            }

                            // hdfInvoiceType.Value = finInvoiceCusHdrList[e.Row.RowIndex].ICH_TYPE.ToString();
                            hdfInvoiceType.Value = finCrDrNoteMpgList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_TYPE.ToString();
                            InvType = Convert.ToInt32(hdfInvoiceType.Value);
                        }

                        //txtNoteFor.Focus();
                    }

                    if (((GridView)sender).ID == "grdCrDbHdr")
                    {
                        if (finCrDrNoteHdrList != null && finCrDrNoteHdrList.Count > 0)
                        {
                            List<FIN_CRDR_NOTE_MPG> crDrNoreMapList = finCrDrNoteHdrList[e.Row.RowIndex].FIN_CRDR_NOTE_MPG.ToList();
                            var Invtype = crDrNoreMapList[0].FIN_INVOICE_VND_HDR.IVH_TYPE;
                            HiddenField hdfInvoiceType = e.Row.FindControl("hdfInvoiceType") as HiddenField;
                            hdfInvoiceType.Value = Invtype.ToString();

                            Label lblModeofPayment = e.Row.FindControl("lblModeofPayment") as Label;
                            int cfgpk = Convert.ToInt32(lblModeofPayment.Text);
                            GetFieldValues(ControlsEnum.CRDRTYPE);
                            if (admConfigMstList != null && admConfigMstList.Count > 0)
                            {
                                lblModeofPayment.Text = admConfigMstList.SingleOrDefault(con => con.CFG_VALUE == cfgpk).CFG_DATA;
                                lblModeofPayment.ToolTip = admConfigMstList.SingleOrDefault(con => con.CFG_VALUE == cfgpk).CFG_DATA;
                            }

                            #region Invoice Number
                            FIN_CRDR_NOTE_HDR crdrHdr = finCrDrNoteHdrList.SingleOrDefault(x => x.CDH_PK == Convert.ToInt32(((HiddenField)e.Row.FindControl("hdfCrDrPk")).Value));
                            LinkButton lnkInvnos = e.Row.FindControl("lnkInvnos") as LinkButton;
                            Label lblInvDate = e.Row.FindControl("lblInvDate") as Label;
                            HiddenField hdfListinvPK = e.Row.FindControl("hdfListinvPK") as HiddenField;
                            HiddenField hdfListInvType = e.Row.FindControl("hdfListInvType") as HiddenField;
                            HiddenField hdfListinvCategory = e.Row.FindControl("hdfListinvCategory") as HiddenField;
                            HiddenField hdfListGroup = e.Row.FindControl("hdfListGroup") as HiddenField;
                            if (crdrHdr != null)
                            {
                                string invoices = string.Empty;
                                int invCount = 0;
                                var invNo = from c in crdrHdr.FIN_CRDR_NOTE_MPG select c.FIN_INVOICE_VND_HDR.IVH_NO;
                                DateTime invDate = Convert.ToDateTime(crdrHdr.FIN_CRDR_NOTE_MPG.ToList()[0].FIN_INVOICE_VND_HDR.IVH_DATE);
                                int invPk = Convert.ToInt32(crdrHdr.FIN_CRDR_NOTE_MPG.ToList()[0].FIN_INVOICE_VND_HDR.IVH_PK);
                                int invType = crdrHdr.FIN_CRDR_NOTE_MPG.ToList()[0].FIN_INVOICE_VND_HDR.IVH_TYPE;
                                int invCategory = crdrHdr.FIN_CRDR_NOTE_MPG.ToList()[0].FIN_INVOICE_VND_HDR.IVH_CATEGORY;
                                int invGroup = crdrHdr.FIN_CRDR_NOTE_MPG.ToList()[0].FIN_INVOICE_VND_HDR.IVH_GROUP;

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
                        }

                        Button imgApproved = e.Row.FindControl("imgApproved") as Button;
                        Button imgPosted = e.Row.FindControl("imgPosted") as Button;
                        Button imgAffectStock = e.Row.FindControl("imgAffectStock") as Button;

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

                        if (imgAffectStock != null)
                        {
                            imgAffectStock.CssClass = GetLocalResourceObject("StockCss").ToString();
                            GetFieldValues(ControlsEnum.GETCRDRHDRDETAILS);
                            if (ObjfinCrDrNoteHdrList != null && ObjfinCrDrNoteHdrList.Count > 0)
                            {
                                if (ObjfinCrDrNoteHdrList[0].INV_ITEM_CONS_HDR.Where(r => r.ICH_DEL_STATUS == 0).Count() > 0)
                                    imgAffectStock.CssClass = GetLocalResourceObject("StockUpdationCss").ToString();
                                imgAffectStock.Visible = SetStockVisibility(ObjfinCrDrNoteHdrList[0]);
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
                        //GetFieldValues(ControlsEnum.ALLOCATEDINPAYMENT);
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
                        if (Convert.ToDecimal(lblAmount.Text) == Convert.ToDecimal(lnkBalanceAmount.Text))
                        {
                            lblBalanceAmount.Visible = true;
                            lnkBalanceAmount.Visible = false;
                            //lnkBalanceAmount.Attributes.Remove("href");
                            //lnkBalanceAmount.CssClass.Replace("text-underline","");                           
                            //lnkBalanceAmount.Enabled = false;
                        }
                        else
                        {
                            lblBalanceAmount.Visible = false;
                            lnkBalanceAmount.Visible = true;
                        }
                    }
                    if (((GridView)sender).ID == "grdDCSplit")
                    {

                        //grdDCSplit.Columns[5].Visible = grdDCSplit.Columns[10].Visible = isLineItemTaxEnabled;
                        if (Convert.ToInt16(hdfisTaxAdd.Value) == (int)TaxSettingEnum.ITEMWISE || Convert.ToInt16(hdfisTaxAdd.Value) == (int)TaxSettingEnum.BOTHHEADERITEM)
                        {
                            // grdDCSplit.Columns[5].Visible = grdDCSplit.Columns[11].Visible = true;
                            grdDCSplit.Columns[(int)PopupGridColumn.ActualTax].Visible = grdDCSplit.Columns[(int)PopupGridColumn.FinalTax].Visible = true;

                        }

                        if (hdfisDiscountAdd.Value == "1")
                        {
                            grdDCSplit.Columns[(int)PopupGridColumn.Discount].Visible = true;
                            grdDCSplit.Columns[(int)PopupGridColumn.ActualDiscount].Visible = true;
                        }
                        else
                        {
                            grdDCSplit.Columns[(int)PopupGridColumn.Discount].Visible = false;
                            grdDCSplit.Columns[(int)PopupGridColumn.ActualDiscount].Visible = true;
                        }

                        txtTotalSplit = e.Row.FindControl("txtTotalSplit") as TextBox;
                        hdfActualDiscountSplit = e.Row.FindControl("hdfActualDiscountSplit") as HiddenField;
                        lblActualDiscountSplit = e.Row.FindControl("lblActualDiscountSplit") as Label;
                        hdfInvCusDtlPK = e.Row.FindControl("hdfInvCusDtlPK") as HiddenField;
                        hdfReceiptSplitPK = e.Row.FindControl("hdfDCSplitPK") as HiddenField;
                        hdfReceiptTRXPK = e.Row.FindControl("hdfDCTRXPK") as HiddenField;
                        lblPRODUCTSplit = e.Row.FindControl("lblPRODUCTSplit") as Label;
                        hdfNETSplit = e.Row.FindControl("hdfNETSplit") as HiddenField;

                        lblQTYSplit = e.Row.FindControl("lblQTYSplit") as Label;
                        lblRATESplit = e.Row.FindControl("lblRATESplit") as Label;
                        lblAmountSplit = e.Row.FindControl("lblAmountSplit") as Label;
                        lblTAXSplit = e.Row.FindControl("lblTAXSplit") as Label;
                        lblPONo = e.Row.FindControl("lblPONo") as Label;
                        lblNETSplit = e.Row.FindControl("lblNETSplit") as Label;
                        txtQtySplit = e.Row.FindControl("txtQtySplit") as TextBox;
                        txtRateSplit = e.Row.FindControl("txtRateSplit") as TextBox;
                        txtSumSplit = e.Row.FindControl("txtSumSplit") as TextBox;
                        hdfSumSplit = e.Row.FindControl("hdfSumSplit") as HiddenField;

                        //lblTAXSplitTotal = e.Row.FindControl("lblTAXSplitTotal") as Label;
                        //lbnTAXSplitTotal = e.Row.FindControl("lbnTAXSplitTotal") as LinkButton;
                        txtTAXSplitTotal = e.Row.FindControl("txtTAXSplitTotal") as TextBox;
                        hdfTAXSplit = e.Row.FindControl("hdfTAXSplit") as HiddenField;
                        TextBox txtDiscountSplit = e.Row.FindControl("txtDiscountSplit") as TextBox;
                        HiddenField hdfDiscountSplit = e.Row.FindControl("hdfDiscountSplit") as HiddenField;

                        hdfTAXSplitTotal = e.Row.FindControl("hdfTAXSplitTotal") as HiddenField;

                        chkAffectStkSplit = e.Row.FindControl("chkAffectStkSplit") as CheckBox;
                        int.TryParse(hdfInvGroup.Value, out InvGroup);
                        if (FinCrDrCusTrxMpgList != null && FinCrDrCusTrxMpgList.Count > 0)
                        {
                            hdfInvCusDtlPK.Value = FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_PK.ToString();
                            hdfReceiptSplitPK.Value = "0";
                            hdfReceiptTRXPK.Value = "0";
                            //lblPRODUCTSplit.Text = FinCrDrCusTrxMpgList[e.Row.RowIndex].INV_ITEM_MST.ITM_NAME;
                            //lblPRODUCTSplit.ToolTip = FinCrDrCusTrxMpgList[e.Row.RowIndex].INV_ITEM_MST.ITM_NAME;
                            lblPRODUCTSplit.ToolTip = lblPRODUCTSplit.Text = HttpUtility.HtmlDecode(FinCrDrCusTrxMpgList[e.Row.RowIndex].INV_ITEM_MST != null ? FinCrDrCusTrxMpgList[e.Row.RowIndex].INV_ITEM_MST.ITM_NAME : FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_INSTRUCTIONS);
                            //lblPRODUCTSplit.ToolTip = FinCrDrCusTrxMpgList[e.Row.RowIndex].INV_ITEM_MST.ITM_NAME;

                            //lblPONo.Text = FinCrDrCusTrxMpgList[e.Row.RowIndex].PUR_ORDER_DTL.PUR_ORDER_HDR.POH_NO;
                            //lblPONo.ToolTip = FinCrDrCusTrxMpgList[e.Row.RowIndex].PUR_ORDER_DTL.PUR_ORDER_HDR.POH_NO;
                            lblPONo.ToolTip = lblPONo.Text = FinCrDrCusTrxMpgList[e.Row.RowIndex].PUR_ORDER_DTL != null ? FinCrDrCusTrxMpgList[e.Row.RowIndex].PUR_ORDER_DTL.PUR_ORDER_HDR.POH_NO : FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_NO;
                            //lblPONo.ToolTip = FinCrDrCusTrxMpgList[e.Row.RowIndex].PUR_ORDER_DTL.PUR_ORDER_HDR.POH_NO;

                            lblActualDiscountSplit.Text = GetFormattedNumber(FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_DISCOUNT);
                            hdfActualDiscountSplit.Value = FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_DISCOUNT.ToString();

                            lblQTYSplit.Text = GetFormattedNumber(FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_QTY_INVOICED);
                            lblQTYSplit.ToolTip = GetFormattedNumber(FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_QTY_INVOICED);

                            lblRATESplit.Text = GetFormattedRate(FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_RATE);
                            lblRATESplit.ToolTip = GetFormattedRate(FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_RATE);

                            lblAmountSplit.Text = String.Format("{0:c}", FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_AMOUNT);
                            lblAmountSplit.ToolTip = String.Format("{0:c}", FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_AMOUNT);
                            hdfNETSplit.Value = FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_AMOUNT.ToString();

                            hdfTAXSplit.Value = FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_TAX.ToString();
                            lblTAXSplit.Text = String.Format("{0:c}", FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_TAX);
                            lblTAXSplit.ToolTip = String.Format("{0:c}", FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_TAX);


                            lblNETSplit.Text = String.Format("{0:c}", FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_NET_AMOUNT);
                            lblNETSplit.ToolTip = String.Format("{0:c}", FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_NET_AMOUNT);



                            if (tempfinCrDrCusMpgList != null)
                            {
                                tempFinReceiptCusSoMpgObj = tempfinCrDrCusMpgList.SingleOrDefault(mpg => mpg.CDS_INVOICE_VND_DTL == Convert.ToInt64(hdfInvCusDtlPK.Value));
                            }

                            txtQtySplit.Text = tempFinReceiptCusSoMpgObj == null ? GetFormattedNumber(0) : GetFormattedNumber(tempFinReceiptCusSoMpgObj.CDS_QTY);
                            txtQtySplit.ToolTip = tempFinReceiptCusSoMpgObj == null ? GetFormattedNumber(0) : GetFormattedNumber(tempFinReceiptCusSoMpgObj.CDS_QTY);

                            if (tempFinReceiptCusSoMpgObj != null)
                            {
                                txtTotalSplit.Text = GetFormattedCurrency(tempFinReceiptCusSoMpgObj.CDS_QTY * tempFinReceiptCusSoMpgObj.CDS_RATE);
                                txtTotalSplit.ToolTip = GetFormattedCurrency(tempFinReceiptCusSoMpgObj.CDS_QTY * tempFinReceiptCusSoMpgObj.CDS_RATE);
                                txtDiscountSplit.Text = txtDiscountSplit.ToolTip = GetFormattedCurrency(tempFinReceiptCusSoMpgObj.CDS_DISCOUNT);
                                hdfDiscountSplit.Value = tempFinReceiptCusSoMpgObj.CDS_DISCOUNT.ToString();

                            }
                            //txtQtySplit.ToolTip =
                            //   tempFinReceiptCusSoMpgObj == null ?
                            //   Math.Round(Convert.ToDouble(0), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString()
                            //   : Math.Round(tempFinReceiptCusSoMpgObj.CDS_QTY, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                            //txtRateSplit.Text = tempFinReceiptCusSoMpgObj == null ? GetFormattedRate(FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_RATE) : GetFormattedRate(tempFinReceiptCusSoMpgObj.CDS_RATE);
                            //txtRateSplit.ToolTip =tempFinReceiptCusSoMpgObj == null ? GetFormattedRate(FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_RATE) : GetFormattedRate(tempFinReceiptCusSoMpgObj.CDS_RATE);
                            txtRateSplit.Text = tempFinReceiptCusSoMpgObj == null ? FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_RATE.ToString() : tempFinReceiptCusSoMpgObj.CDS_RATE.ToString();
                            txtRateSplit.ToolTip = tempFinReceiptCusSoMpgObj == null ? FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_RATE.ToString() : tempFinReceiptCusSoMpgObj.CDS_RATE.ToString();

                            chkAffectStkSplit.Checked = tempFinReceiptCusSoMpgObj == null ? false : (tempFinReceiptCusSoMpgObj.CDS_IS_AFFECT_STK == 1 ? true : false);
                            chkAffectStkSplit.Enabled = (InvGroup == (int)POInvoiceGroup.Expense) ? false : true;
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
                                txtSumSplit.ToolTip = Math.Round((qty * rate), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                hdfSumSplit.Value = Math.Round((qty * rate), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            }

                            taxpercentage = Convert.ToDecimal(hdfTAXSplit.Value) / (Convert.ToDecimal(FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_AMOUNT - FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_DISCOUNT) == 0 ? 1 : Convert.ToDecimal(FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_AMOUNT - FinCrDrCusTrxMpgList[e.Row.RowIndex].VID_DISCOUNT));
                            basevalue = (txtSumSplit.Text != string.Empty ? Convert.ToDecimal(txtSumSplit.Text) : 0) / (1 + taxpercentage);
                            taxamt = ((txtSumSplit.Text != string.Empty ? Convert.ToDecimal(txtSumSplit.Text) : 0) - basevalue);
                            //lblTAXSplitTotal.Text = Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //lblTAXSplitTotal.ToolTip = lblTAXSplitTotal.Text;
                            //grndTotalTaxSplit += Convert.ToDecimal(lblTAXSplitTotal.Text);
                            //lbnTAXSplitTotal.Text = Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //lbnTAXSplitTotal.ToolTip = lbnTAXSplitTotal.Text;
                            //grndTotalTaxSplit += Convert.ToDecimal(lbnTAXSplitTotal.Text);
                            if (tempfinCrDrCusMpgList != null)
                            {
                                txtTAXSplitTotal.Text = txtTAXSplitTotal.ToolTip = GetFormattedCurrency(tempFinReceiptCusSoMpgObj.CDS_TAX);
                            }
                            //txtTAXSplitTotal.Text = txtTAXSplitTotal.ToolTip = Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            grndTotalTaxSplit += Convert.ToDecimal(txtTAXSplitTotal.Text);
                            hdfTAXSplitTotal.Value = Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            if (qty == 0)
                            {
                                chkAffectStkSplit.Enabled = false;
                            }
                        }
                        else if (finCrDrCusMpgList != null && finCrDrCusMpgList.Count > 0)
                        {
                            if (finInvItemList != null && finInvItemList.Count > 0)
                            {
                                finInvItemDtl = finInvItemList.SingleOrDefault(itm => itm.VID_PK == finCrDrCusMpgList[e.Row.RowIndex].CDS_INVOICE_VND_DTL);
                                if (finInvItemDtl != null)
                                {
                                    txtDiscountSplit.Text = txtDiscountSplit.ToolTip = GetFormattedCurrency(finCrDrCusMpgList[e.Row.RowIndex].CDS_DISCOUNT);
                                    hdfDiscountSplit.Value = tempFinReceiptCusSoMpgObj == null ? "0" : finCrDrCusMpgList[e.Row.RowIndex].CDS_DISCOUNT.ToString();

                                    hdfInvCusDtlPK.Value = finInvItemDtl.VID_PK.ToString();
                                    //hdfInvCusDtlPK.Value =
                                    hdfReceiptSplitPK.Value = finCrDrCusMpgList[e.Row.RowIndex].CDS_PK.ToString();
                                    hdfReceiptTRXPK.Value = finCrDrCusMpgList[e.Row.RowIndex].CDS_INVOICE_VND_DTL.ToString();
                                    //lblPRODUCTSplit.Text = finInvItemDtl.INV_ITEM_MST.ITM_NAME;
                                    //lblPRODUCTSplit.ToolTip = finInvItemDtl.INV_ITEM_MST.ITM_NAME.ToString();
                                    lblPRODUCTSplit.ToolTip = lblPRODUCTSplit.Text = HttpUtility.HtmlDecode(finInvItemDtl.INV_ITEM_MST != null ? finInvItemDtl.INV_ITEM_MST.ITM_NAME : finInvItemDtl.VID_INSTRUCTIONS);
                                    //lblPRODUCTSplit.ToolTip = finInvItemDtl.INV_ITEM_MST.ITM_NAME.ToString();

                                    //lblPONo.Text = finInvItemDtl.PUR_ORDER_DTL.PUR_ORDER_HDR.POH_NO;
                                    //lblPONo.ToolTip = finInvItemDtl.PUR_ORDER_DTL.PUR_ORDER_HDR.POH_NO;
                                    lblPONo.ToolTip = lblPONo.Text = finInvItemDtl.PUR_ORDER_DTL != null ? finInvItemDtl.PUR_ORDER_DTL.PUR_ORDER_HDR.POH_NO : finInvItemDtl.VID_NO;
                                    //lblPONo.ToolTip = finInvItemDtl.PUR_ORDER_DTL.PUR_ORDER_HDR.POH_NO;

                                    ////// commented for  bug : 2010
                                    ////lblQTYSplit.Text = String.Format("{0:n}", (finCrDrCusMpgList[e.Row.RowIndex].CDS_QTY > 0) ? finCrDrCusMpgList[e.Row.RowIndex].CDS_QTY : finInvItemDtl.VID_QTY_INVOICED);
                                    ////lblQTYSplit.ToolTip = String.Format("{0:n}", (finCrDrCusMpgList[e.Row.RowIndex].CDS_QTY > 0) ? finCrDrCusMpgList[e.Row.RowIndex].CDS_QTY : finInvItemDtl.VID_QTY_INVOICED);

                                    ////lblRATESplit.Text = String.Format("{0:c}", (finCrDrCusMpgList[e.Row.RowIndex].CDS_RATE > 0) ? Convert.ToDouble(finCrDrCusMpgList[e.Row.RowIndex].CDS_RATE) : Convert.ToDouble(finInvItemDtl.VID_RATE));
                                    ////lblRATESplit.ToolTip = String.Format("{0:c}", (finCrDrCusMpgList[e.Row.RowIndex].CDS_RATE > 0) ? Convert.ToDouble(finCrDrCusMpgList[e.Row.RowIndex].CDS_RATE) : Convert.ToDouble(finInvItemDtl.VID_RATE));

                                    ////lblAmountSplit.Text = String.Format("{0:c}", (finCrDrCusMpgList[e.Row.RowIndex].CDS_AMOUNT > 0) ? finCrDrCusMpgList[e.Row.RowIndex].CDS_AMOUNT : finInvItemDtl.VID_AMOUNT);
                                    ////lblAmountSplit.ToolTip = String.Format("{0:c}", (finCrDrCusMpgList[e.Row.RowIndex].CDS_AMOUNT > 0) ? finCrDrCusMpgList[e.Row.RowIndex].CDS_AMOUNT : finInvItemDtl.VID_AMOUNT);
                                    ////hdfNETSplit.Value = ((finCrDrCusMpgList[e.Row.RowIndex].CDS_AMOUNT > 0) ? finCrDrCusMpgList[e.Row.RowIndex].CDS_AMOUNT : finInvItemDtl.VID_AMOUNT).ToString();

                                    ////lblNETSplit.Text = String.Format("{0:c}", (finCrDrCusMpgList[e.Row.RowIndex].CDS_NET_AMOUNT > 0) ? finCrDrCusMpgList[e.Row.RowIndex].CDS_NET_AMOUNT : finInvItemDtl.VID_NET_AMOUNT);
                                    ////lblNETSplit.ToolTip = String.Format("{0:c}", (finCrDrCusMpgList[e.Row.RowIndex].CDS_NET_AMOUNT > 0) ? finCrDrCusMpgList[e.Row.RowIndex].CDS_NET_AMOUNT : finInvItemDtl.VID_NET_AMOUNT);

                                    ////hdfTAXSplit.Value = ((finCrDrCusMpgList[e.Row.RowIndex].CDS_TAX > 0) ? finCrDrCusMpgList[e.Row.RowIndex].CDS_TAX : finInvItemDtl.VID_TAX).ToString();
                                    ////lblTAXSplit.Text = String.Format("{0:c}", ((finCrDrCusMpgList[e.Row.RowIndex].CDS_TAX > 0) ? finCrDrCusMpgList[e.Row.RowIndex].CDS_TAX : finInvItemDtl.VID_TAX));
                                    ////lblTAXSplit.ToolTip = String.Format("{0:c}", ((finCrDrCusMpgList[e.Row.RowIndex].CDS_TAX > 0) ? finCrDrCusMpgList[e.Row.RowIndex].CDS_TAX : finInvItemDtl.VID_TAX));

                                    lblQTYSplit.Text = GetFormattedNumber(finInvItemDtl.VID_QTY_INVOICED);
                                    lblQTYSplit.ToolTip = GetFormattedNumber(finInvItemDtl.VID_QTY_INVOICED);

                                    lblRATESplit.Text = String.Format("{0:c}", Convert.ToDouble(finInvItemDtl.VID_RATE));
                                    lblRATESplit.ToolTip = String.Format("{0:c}", Convert.ToDouble(finInvItemDtl.VID_RATE));

                                    lblAmountSplit.Text = String.Format("{0:c}", finInvItemDtl.VID_AMOUNT);
                                    lblAmountSplit.ToolTip = String.Format("{0:c}", finInvItemDtl.VID_AMOUNT);
                                    hdfNETSplit.Value = (finInvItemDtl.VID_AMOUNT).ToString();

                                    lblNETSplit.Text = String.Format("{0:c}", finInvItemDtl.VID_NET_AMOUNT);
                                    lblNETSplit.ToolTip = String.Format("{0:c}", finInvItemDtl.VID_NET_AMOUNT);

                                    hdfTAXSplit.Value = (finInvItemDtl.VID_TAX).ToString();
                                    lblTAXSplit.Text = String.Format("{0:c}", finInvItemDtl.VID_TAX);
                                    lblTAXSplit.ToolTip = String.Format("{0:c}", finInvItemDtl.VID_TAX);

                                    hdfActualDiscountSplit.Value = (finInvItemDtl.VID_DISCOUNT).ToString();
                                    lblActualDiscountSplit.Text = String.Format("{0:c}", finInvItemDtl.VID_DISCOUNT);



                                    chkAffectStkSplit.Checked = finCrDrCusMpgList[e.Row.RowIndex].CDS_IS_AFFECT_STK == 1 ? true : false;
                                    chkAffectStkSplit.Enabled = (InvGroup == (int)POInvoiceGroup.Expense) ? false : true;
                                    if (tempfinCrDrCusMpgList != null)
                                    {
                                        tempFinReceiptCusSoMpgObj = tempfinCrDrCusMpgList.SingleOrDefault(mpg => mpg.CDS_INVOICE_VND_DTL == Convert.ToInt64(hdfReceiptTRXPK.Value));
                                    }
                                    txtQtySplit.Text = tempFinReceiptCusSoMpgObj == null ? GetFormattedNumber(0) : GetFormattedNumber(tempFinReceiptCusSoMpgObj.CDS_QTY);
                                    txtQtySplit.ToolTip = tempFinReceiptCusSoMpgObj == null ? GetFormattedNumber(0) : GetFormattedNumber(tempFinReceiptCusSoMpgObj.CDS_QTY);
                                    //txtQtySplit.ToolTip =
                                    //  tempFinReceiptCusSoMpgObj == null ?
                                    //  Math.Round(Convert.ToDouble(0), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString()
                                    //  : Math.Round(tempFinReceiptCusSoMpgObj.CDS_QTY, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                                    //txtRateSplit.Text = tempFinReceiptCusSoMpgObj == null ? GetFormattedRate(finInvItemDtl.VID_RATE) : GetFormattedRate(tempFinReceiptCusSoMpgObj.CDS_RATE);
                                    //txtRateSplit.ToolTip = tempFinReceiptCusSoMpgObj == null ? GetFormattedRate(finInvItemDtl.VID_RATE) : GetFormattedRate(tempFinReceiptCusSoMpgObj.CDS_RATE);
                                    txtRateSplit.Text = tempFinReceiptCusSoMpgObj == null ? finInvItemDtl.VID_RATE.ToString() : tempFinReceiptCusSoMpgObj.CDS_RATE.ToString();
                                    txtRateSplit.ToolTip = tempFinReceiptCusSoMpgObj == null ? finInvItemDtl.VID_RATE.ToString() : tempFinReceiptCusSoMpgObj.CDS_RATE.ToString();

                                    if (tempFinReceiptCusSoMpgObj != null)
                                    {
                                        txtTotalSplit.Text = GetFormattedCurrency(tempFinReceiptCusSoMpgObj.CDS_QTY * tempFinReceiptCusSoMpgObj.CDS_RATE);
                                        txtTotalSplit.ToolTip = GetFormattedCurrency(tempFinReceiptCusSoMpgObj.CDS_QTY * tempFinReceiptCusSoMpgObj.CDS_RATE);
                                    }
                                    //txtRateSplit.ToolTip = tempFinReceiptCusSoMpgObj == null ?
                                    //   Math.Round(Convert.ToDouble(0), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString()
                                    //   : Math.Round(tempFinReceiptCusSoMpgObj.CDS_RATE, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

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
                                        txtSumSplit.ToolTip = Math.Round((qty * rate), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                        hdfSumSplit.Value = Math.Round((qty * rate), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                    }
                                    //lblTAXSplitTotal.Text = Math.Round(finCrDrCusMpgList[e.Row.RowIndex].CDS_TAX, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                    //lblTAXSplitTotal.ToolTip = lblTAXSplitTotal.Text;
                                    //grndTotalTaxSplit += Convert.ToDecimal(lblTAXSplitTotal.Text);
                                    //lbnTAXSplitTotal.Text = (tempFinReceiptCusSoMpgObj == null) ? GetFormattedCurrency(0) : Math.Round(tempFinReceiptCusSoMpgObj.CDS_TAX, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                    //lbnTAXSplitTotal.ToolTip = lbnTAXSplitTotal.Text;
                                    //grndTotalTaxSplit += Convert.ToDecimal(lbnTAXSplitTotal.Text);
                                    txtTAXSplitTotal.Text = txtTAXSplitTotal.ToolTip = (tempFinReceiptCusSoMpgObj == null) ? GetFormattedCurrency(0) : Math.Round(tempFinReceiptCusSoMpgObj.CDS_TAX, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                    grndTotalTaxSplit += Convert.ToDecimal(txtTAXSplitTotal.Text);
                                    hdfTAXSplitTotal.Value = (tempFinReceiptCusSoMpgObj == null) ? GetFormattedCurrency(0) : Math.Round(tempFinReceiptCusSoMpgObj.CDS_TAX, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                    if (qty == 0)
                                    {
                                        chkAffectStkSplit.Enabled = false;
                                    }
                                }
                            }
                        }

                        if (InvGroup == (int)POInvoiceGroup.Expense)
                        {
                            ((CheckBox)grdDCSplit.HeaderRow.FindControl("chkAffectStkHdr")).Enabled = false;
                            grdDCSplit.Columns[(int)PopupGridColumn.ActualTax].Visible = grdDCSplit.Columns[(int)PopupGridColumn.FinalTax].Visible = true;
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

                }
                if (((GridView)sender).ID != "grdUploads")
                {
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        if (((GridView)sender).ID == "grdInvoiceList")
                        {
                            lblTotalFooter = e.Row.FindControl("lblTotalPayNowFooter") as Label;
                            if (finInvoiceVndHdrList != null && finInvoiceVndHdrList.Count > 0)
                            {
                                total = 0;
                                total = finInvoiceVndHdrList.Sum(dtl => dtl.IVH_AMOUNT_NET_TC -
                                                                        dtl.IVH_AMOUNT_PAID_TC +
                                                                        dtl.IVH_AMOUNT_CN_TC -
                                                                        dtl.IVH_AMOUNT_DN_TC);
                                total = total < 0 ? 0 : total;
                                lblTotalFooter.Text = string.Format("{0:c}", total);
                                txtPaidAmount.Text = Math.Round(total, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            }
                            else if (finCrDrNoteMpgList != null && finCrDrNoteMpgList.Count > 0)
                            {
                                total = 0;
                                total = finCrDrNoteMpgList.Sum(dtl => dtl.CDM_AMOUNT);
                                total = total < 0 ? 0 : total;
                                lblTotalFooter.Text = string.Format("{0:c}", total);
                                txtPaidAmount.Text = Math.Round(total, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            }
                        }
                        if (((GridView)sender).ID == "grdDCSplit")
                        {

                            lblTotalDiscountFooterSplit = e.Row.FindControl("lblTotalDiscountFooterSplit") as Label;
                            lblTotalPayNowFooterSplit = e.Row.FindControl("lblTotalPayNowFooterSplit") as Label;
                            hdfTotalPayNowFooterSplit = e.Row.FindControl("hdfTotalPayNowFooterSplit") as HiddenField;
                            hdfTotalDiscountFooterSplit = e.Row.FindControl("hdfTotalDiscountFooterSplit") as HiddenField;

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
                                    lblTotalDiscountFooterSplit.Text = GetFormattedCurrency(0);
                                    hdfTotalDiscountFooterSplit.Value = "0";
                                }
                                else
                                {
                                    lblTotalPayNowFooterSplit.Text = GetFormattedCurrency(tempfinCrDrCusMpgList.Sum(mpg => mpg.CDS_NET_AMOUNT));
                                    hdfTotalPayNowFooterSplit.Value = tempfinCrDrCusMpgList.Sum(mpg => mpg.CDS_NET_AMOUNT).ToString();
                                    lblTotalDiscountFooterSplit.Text = GetFormattedCurrency(tempfinCrDrCusMpgList.Sum(mpg => mpg.CDS_DISCOUNT));
                                    hdfTotalDiscountFooterSplit.Value = tempfinCrDrCusMpgList.Sum(mpg => mpg.CDS_DISCOUNT).ToString();
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
                                    lblTotalDiscountFooterSplit.Text = GetFormattedCurrency(0);
                                    hdfTotalDiscountFooterSplit.Value = "0";
                                }
                                else
                                {
                                    lblTotalPayNowFooterSplit.Text = GetFormattedCurrency(tempfinCrDrCusMpgList.Sum(mpg => mpg.CDS_NET_AMOUNT));// string.Format("{0:c}", tempfinCrDrCusMpgList.Sum(mpg => mpg.CDS_NET_AMOUNT));
                                    hdfTotalPayNowFooterSplit.Value = tempfinCrDrCusMpgList.Sum(mpg => mpg.CDS_NET_AMOUNT).ToString();
                                    lblTotalDiscountFooterSplit.Text = GetFormattedCurrency(tempfinCrDrCusMpgList.Sum(mpg => mpg.CDS_DISCOUNT));
                                    hdfTotalDiscountFooterSplit.Value = tempfinCrDrCusMpgList.Sum(mpg => mpg.CDS_DISCOUNT).ToString();
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

        /// <summary>
        /// To set the stock button(for EMI or EMR) visibility
        /// </summary>
        /// <param name="CrdrPk"></param>
        /// <param name="Status"></param>
        /// <param name="IsAffectStk"></param>
        /// <returns></returns>
        private bool SetStockVisibility(FIN_CRDR_NOTE_HDR ObjfinCrdrHdr)
        {
            BusinessLogic.AccountManagement.UserAuthBL userAuth = new BusinessLogic.AccountManagement.UserAuthBL();
            CrDrPk = ObjfinCrdrHdr.CDH_PK;
            GetFieldValues(ControlsEnum.PODEPT);
            string pageURL = string.Empty;
            if (ObjfinCrdrHdr.CDH_TYPE == (int)DebitCreditModeEnum.DEBIT)
                pageURL = GetLocalResourceObject("Url_EMI").ToString();
            else
                pageURL = GetLocalResourceObject("Url_EMR").ToString();

            UserRightsBO UserRights = userAuth.GetUserRights(currentUser.PKUser, pageURL, currentUser.SBUID, PoDept);
            if (ObjfinCrdrHdr.CDH_IS_AFFECT_STK == 0 || ObjfinCrdrHdr.CDH_STATUS == 0 || ObjfinCrdrHdr.CDH_IS_DELETED || UserRights == null || UserRights.Rights.Count < 1) //If user has no rights defined, prevent page access            
                return false;
            else
                return true;
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
            btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnSaveDrcr.PreRender += new EventHandler(btnAction_PreRender);
            btnDeleteDrcr.PreRender += new EventHandler(btnAction_PreRender);
            btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnJournalize.PreRender += new EventHandler(btnAction_PreRender);
            btnListPrint.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);

            btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            btnView.PreRender += new EventHandler(btnAction_PreRender);

            //lbnPOListing.PreRender += new EventHandler(btnAction_PreRender);
            //lnkInvoicing.PreRender += new EventHandler(btnAction_PreRender);
            //lbnExpenses.PreRender += new EventHandler(btnAction_PreRender);
            //lbnPOInvoice.PreRender += new EventHandler(btnAction_PreRender);
            //lnkPayment.PreRender += new EventHandler(btnAction_PreRender);
            //lnbCrDrNote.PreRender += new EventHandler(btnAction_PreRender);
            //lnbAcPayables.PreRender += new EventHandler(btnAction_PreRender);

            lnkList.PreRender += new EventHandler(btnAction_PreRender);
            lnkDetail.PreRender += new EventHandler(btnAction_PreRender);

            btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            btnSubmit.Load += new EventHandler(btnAction_Load);
            btnSaveDrcr.Load += new EventHandler(btnAction_Load);
            btnDeleteDrcr.Load += new EventHandler(btnAction_Load);
            btnPrint.Load += new EventHandler(btnAction_Load);
            btnCancel.Load += new EventHandler(btnAction_Load);
            btnJournalize.Load += new EventHandler(btnAction_Load);
            //btnAlert.Load += new EventHandler(btnAction_Load);
            btnListPrint.Load += new EventHandler(btnAction_Load);
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            btnEditforCancel.Load += new EventHandler(btnAction_Load);


            btnEdit.Load += new EventHandler(btnAction_Load);
            btnView.Load += new EventHandler(btnAction_Load);

            //lbnPOListing.Load += new EventHandler(btnAction_Load);
            //lnkInvoicing.Load += new EventHandler(btnAction_Load);
            //lbnExpenses.Load += new EventHandler(btnAction_Load);
            //lbnPOInvoice.Load += new EventHandler(btnAction_Load);
            //lnkPayment.Load += new EventHandler(btnAction_Load);
            //lnbCrDrNote.Load += new EventHandler(btnAction_Load);
            //lnbAcPayables.Load += new EventHandler(btnAction_Load);


            lnkList.Load += new EventHandler(btnAction_Load);
            lnkDetail.Load += new EventHandler(btnAction_Load);

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
        private enum PopupGridColumn
        {
            ActualDiscount = 4,
            ActualTax = 6,
            Total = 10,
            Discount = 12,
            FinalTax = 13

        }
        private enum InvoiceColumn
        {
            ActualOtherchage = 6,
            OthreChages = 14
        }
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
            INVOICEDETAILS,
            TAXMYR,
            TAXSPLITUP,
            INVOICEITEMDETAILS,
            TAXSPLITUPLINEITEMWISE,
            INVITEMLIST,
            CHECKCRDRUSEDINOTHERTRNS,
            LINEITEMTAXSETTINGS,
            FINHEADERSTATUS,
            ALLOCATEDINPAYMENT,
            BALANCEAMOUNTSPLIT,
            DRCRCANCELCHECK,
            FILEUPLOAD,
            ADDITEM,
            SELECTEDDOC,
            GETCRDRHDRDETAILS,
            PODEPT,
            DRCRGET
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
            DEBITCREDITNOTE = 20
        }
        /// <summary>
        /// Attachment Module Enum
        /// </summary>
        public enum DocModuleEnum
        {
            FINANCE = 8
            //  SALES = 5
        }
        /// <summary>
        /// Define Tax Setting Enum
        /// </summary>
        public enum TaxSettingEnum
        {
            HEADERWISE = 0,
            ITEMWISE = 1,
            BOTHHEADERITEM = 2
        }
        #endregion
    }
}