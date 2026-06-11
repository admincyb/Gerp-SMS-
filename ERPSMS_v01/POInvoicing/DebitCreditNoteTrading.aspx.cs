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
using BusinessObject.POInvoicing;

#region DB Summary
/*/////  TABLES  //////:
1.FIN_CRDR_NOTE_HDR 
2.FIN_CRDR_NOTE_MPG
3.FIN_CRDR_NOTE_DTL 
4.FIN_CRDR_NOTE_TAX_HDR
5.FIN_CRDR_NOTE_TAX_DTL  

///// STORED PROCEDURES ////:
1.Pending Invoice List => SPFIN_INVOICE_VND_TRADING_CRDR_PENDING_GET
2.Add to list/Get SP   => SPFIN_CRDR_NOTE_VND_GET
3.SAVE SP              => SPFIN_CRDR_VND_WKF_SAVE ,SPFIN_CRDR_VND_SAVE
4.LISTING SP           => SPFIN_CRDR_VND_GET_LIST
5.To get InvoiceNo Auto=> SPFIN_INVOICE_VND_TRADING_CRDR_AUTO
6.DELETE SP            => SPFIN_CRDR_NOTE_HDR_DELETE
7.DRCR No Auto SP      => SPFIN_CRDR_NOTE_NO_AUTO
8.Edit for cancel      => SPFIN_CRDR_NOTE_VND_CAN_CHECK

 * */

#endregion
namespace ERPSMS_v01.POInvoicing
{
    public partial class DebitCreditNoteTrading : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties

        #region Properties       
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
                return this.ViewState[ViewstateStrings.TotalPages] != null ? Convert.ToInt32(this.ViewState[ViewstateStrings.TotalPages]) : 0;
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
        /// To maintain keep Mapping details
        /// </summary>
        private List<DebitCreditNoteInvoiceMappingDetails> CrDrInvMappingDetails
        {
            get
            {
                return (List<DebitCreditNoteInvoiceMappingDetails>)this.ViewState["CrDrInvMappingDetails"];
            }
            set
            {
                this.ViewState["CrDrInvMappingDetails"] = value;
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
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string PageIndexInv
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
        private DataTable dtPendingInvList
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
        /// <summary>
        /// To maintain keep PO Invoice Header Tax Splitting
        /// </summary>
        private DebitCreditTradingBO InvCrDrHeaderSession
        {
            get
            {
                return (DebitCreditTradingBO)Session["DirectInvCrDrHeaderSession"];
            }
            set
            {
                Session["DirectInvCrDrHeaderSession"] = value;
            }
        }

        /// <summary>
        /// To maintain keep PO Invoice Header Tax Splitting
        /// </summary>
        private DebitCreditTradingBO TempInvCrDrHeaderSession
        {
            get
            {
                return (DebitCreditTradingBO)this.ViewState["DirectTempInvCrDrHeaderSession"];
            }
            set
            {
                this.ViewState["DirectTempInvCrDrHeaderSession"] = value;
            }
        }

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
        private List<BusinessObject.POInvoicing.DebitCreditNoteUploads> POUploadList
        {
            get
            {
                return ViewState[ViewstateStrings.POUploadList] == null ? null : (List<BusinessObject.POInvoicing.DebitCreditNoteUploads>)ViewState[ViewstateStrings.POUploadList];
            }
            set
            {
                ViewState[ViewstateStrings.POUploadList] = value;
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
        #endregion
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        User currentUser;

        //page related Entity Object

        private ServiceUtility serviceUtilityObj; 
        private ADM_CONFIG_MST admConfigMstObj;
        private List<ADM_CONFIG_MST> admConfigMstList;      

        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;     
      
        private DebitCreditNoteInvoiceMappingDetails finCrDrNoteMpgObj;     
        private List<DebitCreditNoteDetails> finCrDrNoteDtlList;  
        private List<FIN_INVOICE_VND_HDR> finInvoiceVndHdrListForPaymentSplit;
        private FIN_INVOICE_VND_HDR finCrDrVndHdrObjForPaymentSplit;
        private DebitCreditNoteDetails finCrDrCusSoMpgObj;
        private List<FIN_INVOICE_VND_HDR> objInvoiceDetails;
        private FIN_INVOICE_VND_HDR objInvDetails;
        private List<SOInvoiceTaxHdr> taxTempList;
        private List<SOInvoiceTaxHdr> taxList;

        private DirectInvHeaderBO InvHeaderObj;
        private DebitCreditTradingBO debitcreditHeaderObj;        
        List<DebitCreditNoteInvoiceMappingDetails> drcrInvoiceMappingDetailList;
        private List<DebitCreditNoteDetails> drcrNoteItemDetailList;
        List<DebitCreditNoteInvoiceMappingDetails> tempfinCrDrCusMpgList;
        private DebitCreditNoteInvoiceMappingDetails drcrInvMapDtlObj;

        private DataTable dtInvoiceType;
        private DataTable dtInvoiceCategory;
        private DataTable dtAmountDetails;
        private DataTable dtVendorDetails;
        private List<CrdrAllocations> objCrdrAllocations;
        private long InvPk = 0;
        private long CrDrPk = 0;
        private bool isDrCrUsedInOtherTrns = false;
        private decimal grndTotalTaxSplit = 0;
        private double ExchageRate = 0;
        private bool isSplitChanged = false;
        int JournalPK;      

        int PoDept = 0;
        private string refID;
        private string inboxFlag;
        DataSet dsPageData;
        private DataTable dtInvoiceList;
        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;
        private List<FIN_YEAR_MST> finYearMstList;     

        private int vendPK = 0;
        private int purchaseInvoicePK = 0;
        private long poInvoicePk = 0;
        bool isCancelled = false;
        DebitCreditNoteUploads poUploadObj;

        bool isInvalidRaiseNote = false;

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
                #region Currency,Decimal Format Settings
                hdfDecimalFormat.Value = "#0.";               
                string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;                
                hdfDecimalFormatWithSeperation.Value = "#" + currencysep + "#0.";
                hdfCurrencyFormatWithSeperator.Value = "#" + currencysep + "#0.";
                hdfCurrencyFormat.Value = "#0.";
                int NoDecimalDigitsP2P = Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P].ToString()) : 2;
                for (int i = 0; i < NoDecimalDigitsP2P; i++)
                {
                    hdfDecimalFormat.Value += "0";
                    hdfDecimalFormatWithSeperation.Value += "0";
                }
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
                #endregion

                #region Journalize EventHandler Setting
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
                #endregion

                if (!IsPostBack)
                {
                    ConfigurationSettings();
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    CrDrInvMappingDetails = null;
                    Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                    Session[ERP.Utilities.SessionStrings.SelectedPos] = null;

                    txtSearchDateFrom.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfSearchDateFrom.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    txtSearchDateTo.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfSearchDateTo.Value = DateTime.Now.ToString();

                    hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    uclPaging.CurrentPage = 1;                  
                    uclPendingInvPaging.CurrentPage = 1; 
                    AST_DOC_MODE.Value = "0";
                    InvCrDrHeaderSession = null;         
                    GetFieldValues(ControlsEnum.CRDRTYPE);
                    SetFieldValues(ControlsEnum.CRDRTYPE);
                    GetFieldValues(ControlsEnum.INVOICETYPE);
                    SetFieldValues(ControlsEnum.INVOICETYPE);
                    GetFieldValues(ControlsEnum.INVOICECATEGORY);
                    SetFieldValues(ControlsEnum.INVOICECATEGORY);

                    #region pid,refID,prefID,inboxFlag & DataKeyNames settings
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

                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "DOC_SEQ_NO";
                    grdUploads.DataKeyNames = itemkeyarray; 
                    #endregion

                    FillProcessID(1);
                    //If Request From External(Report or Other page) otherthan Menu or Inbox
                    if (Request.QueryString[QueryStrings.FromExt] != null && Request.QueryString[QueryStrings.FromExt] == "T")
                    {
                        ucrWrkf.ViewType = 0;
                        EntryStatus = EntryStatus.VIEWMODE;  
                        Session[ERP.Utilities.SessionStrings.CrDrType] = ApplicationType.TPI;
                        CurrPK = Convert.ToInt32(Request.QueryString[QueryStrings.PK]);
                        hdfCrDrNumber.Value = CurrPK.ToString();
                        GetFieldValues(ControlsEnum.INVCRDRHEADER);
                        SetFieldValues(ControlsEnum.INVCRDRHEADER);
                        SetFieldValues(ControlsEnum.INVCRDRDETAIL);
                        SetFieldValues(ControlsEnum.UPLOADEDFILES);
                        TempInvCrDrHeaderSession = InvCrDrHeaderSession;
                        GetFieldValues(ControlsEnum.PENDINGINVLIST);
                        SetFieldValues(ControlsEnum.PENDINGINVLIST);
                    }
                    else
                    {
                        #region else region

                        #region If Has RefID (from Inbox)
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
                            }
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
                            #endregion
                        }
                        else if (!string.IsNullOrEmpty(prefID))
                        {
                            base.WkfRefID = ucrWrkf.RefID = int.Parse(prefID);
                            CurrPK = GetApplicationID(ucrWrkf.RefID);
                        } 
                        #endregion
                        FileDetailsList = null;
                        if (CurrPK > 0)
                        {
                            #region CurrPK > 0                           
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            Session[ERP.Utilities.SessionStrings.CrDrType] = ApplicationType.TPI;
                            hdfCrDrNumber.Value = CurrPK.ToString();
                            GetFieldValues(ControlsEnum.INVCRDRHEADER);
                            SetFieldValues(ControlsEnum.INVCRDRHEADER);
                            SetFieldValues(ControlsEnum.INVCRDRDETAIL);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            TempInvCrDrHeaderSession = InvCrDrHeaderSession;                           
                            GetFieldValues(ControlsEnum.PENDINGINVLIST);
                            SetFieldValues(ControlsEnum.PENDINGINVLIST);
                            #endregion
                        }
                        else
                        {
                            Session[ERP.Utilities.SessionStrings.CrDrType] = ApplicationType.TPI;
                            //Sets data key for the gird
                            string[] datakeyarray;
                            datakeyarray = new string[1];
                            datakeyarray[0] = Resources.DataFieldRes.CrDrPk;
                            grdCrDbHdr.DataKeyNames = datakeyarray;

                            TempInvCrDrHeaderSession = null;
                            InvCrDrHeaderSession = null;
                            GetFieldValues(ControlsEnum.DRCRHDRLIST);
                            SetFieldValues(ControlsEnum.DRCRHDRLIST);
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                        EnableDisableDrCrMode();
                        #endregion
                    }
                    #region MultiplePlant
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
                    #endregion
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
            int TotalRecords = 0;
            string xmlDocInv = string.Empty;
            SalesInvoiceService SalesInvoiceClient;
            SalesInvoiceClient = null;
            try
            {
                switch (type)
                {
                   
                    #region Credit Debit Hdr List
                    case ControlsEnum.DRCRHDRLIST:
                        TotalRecords = 0;
                        int vendorID = String.IsNullOrEmpty(hdfVendorID.Value.Trim()) ? 0 : Convert.ToInt32(hdfVendorID.Value);
                        if (hdfVendorID.Value != "" && hdfVendorID.Value != "0")
                        {
                            Session[ERP.Utilities.SessionStrings.VendorPK] = hdfVendorID.Value;
                            Session[ERP.Utilities.SessionStrings.Vendor] = txtVendor.Text;
                        }
                        int crdrPk = String.IsNullOrEmpty(hdfCrDrNumber.Value.Trim()) ? 0 : Convert.ToInt32(hdfCrDrNumber.Value);
                        Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        int cmpPk = Convert.ToInt32(ddlCompanySrch.SelectedValue);
                        invoiceNo = txtInvoiceNo.Text.Trim() != null ? txtInvoiceNo.Text.Trim() : string.Empty;
                        Type = 1;
                        int cdhType= Convert.ToInt32(ddlCreditDebitType.SelectedValue) > 0 ? Convert.ToByte(ddlCreditDebitType.SelectedValue) : (byte)0;
                        string vendor = string.IsNullOrEmpty(txtVendor.Text.Trim()) ? string.Empty : (txtVendor.Text.Trim() == "Select/Type" ? string.Empty : txtVendor.Text.Trim());
                        dsPageData = BusinessLogic.POInvoicing.DebitCreditTradingBL.GetDebitCreditTradingList(
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
                            }, currentUser, vendorID, crdrPk, invoiceNo, Resources.PageURL.DebitCreditNotePurchaseTrading.Replace("~", ""), cdhType, Convert.ToInt32(ddlStatus.SelectedValue), Convert.ToInt32(ddlCompanySrch.SelectedValue));

                        if (dsPageData != null && dsPageData.Tables.Count > 0)
                        {
                            DataView dvInvoice = dsPageData.Tables[1].DefaultView;
                            dtInvoiceList = dvInvoice.ToTable();
                            int pagsize = Convert.ToInt32(GetLocalResourceObject("PageSize_CrDrList"));
                            TotalRecords = dsPageData.Tables[0].Rows.Count > 0 ? Convert.ToInt32(dsPageData.Tables[0].Rows[0][0].ToString()) : 0;  
                            TotalPages = TotalRecords == 0 ? 1 : (TotalRecords <= pagsize) ? 1 :
                                        (TotalRecords % pagsize) == 0 ? (TotalRecords / pagsize) :
                                        (TotalRecords / pagsize) + 1;
                        }
                        break;
                    #endregion
                    #region PENDINGPOLIST
                    case ControlsEnum.PENDINGINVLIST:
                        TotalPages = 0;
                        vendPK = 0;
                        purchaseInvoicePK = 0;
                        int.TryParse(hdfVendorHd.Value, out vendPK);
                        int.TryParse(hdfPurchaseInvPK.Value, out purchaseInvoicePK);                      
                        int currentPage = string.IsNullOrEmpty(PageIndexInv) ? 1 : Convert.ToInt32(PageIndexInv);
                        int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize_InvList"));
                        DateTime? fromDate = string.IsNullOrEmpty(txtPendingFromDate.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtPendingFromDate.Text.Trim());
                        DateTime? todate = string.IsNullOrEmpty(txtPendingToDate.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtPendingToDate.Text.Trim());
                        int invCategory = (ddlPendingInvCategory.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlPendingInvCategory.SelectedValue) : 0);
                        int invType = (ddlPendingInvType.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlPendingInvType.SelectedValue) : 0);                       
                            dtPendingInvList = BusinessLogic.POInvoicing.DebitCreditTradingBL.GetPendingInvList(currentUser, vendPK, purchaseInvoicePK, CurrPK, currentPage, pageSize, fromDate, todate, invCategory, invType);  
                        break;
                    #endregion
                    #region INVCRDRHEADER (Getting Details of added MultipleINVPKs)
                    case ControlsEnum.INVCRDRHEADER:
                        xmlDocInv = string.Empty;
                        if (InvHeaderObj != null && InvHeaderObj.INVList != null && InvHeaderObj.INVList.Count > 0)
                        {
                            xmlDocInv = CommonFunctions.XmlSerialize<DirectInvHeaderBO>(InvHeaderObj);
                        }
                        debitcreditHeaderObj = BusinessLogic.POInvoicing.DebitCreditTradingBL.GetDirectPurchaseInvoiceHeaderMUL(xmlDocInv, !string.IsNullOrEmpty(xmlDocInv) ? 0 : CurrPK,Convert.ToByte(IsTaxForOtherCharge.Value));

                        if (InvCrDrHeaderSession == null)
                            InvCrDrHeaderSession = debitcreditHeaderObj.DeepClone();
                        else if (debitcreditHeaderObj != null)
                        {
                            List<string> objInvList = InvCrDrHeaderSession.InvoiceDetail.Select(r => r.CDM_INVOICE_VND_HDR.ToString()).Distinct().ToList();                         
                            InvCrDrHeaderSession.InvoiceDetail.AddRange(debitcreditHeaderObj.InvoiceDetail.Where(r => !objInvList.Contains(r.CDM_INVOICE_VND_HDR.ToString())).ToList());                                                      
                        }                         
                        break;
                    #endregion  
                  
                    #region Get Exchange rate
                    case ControlsEnum.EXCHANGERATE:                    
                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);                   
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
                    #region Company
                    case ControlsEnum.COMPANY:                     
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
                    #region Tax MYR
                    case ControlsEnum.TAXMYR:
                        SetUIValuesToObject(ControlsEnum.TAXMYR);
                        break;
                    #endregion                                            
                 
                    #region BALANCE AMOUNT SPLIT
                    case ControlsEnum.BALANCEAMOUNTSPLIT:
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        objCrdrAllocations = finCrDrHdrNoteServiceClient.GetBalanceAmountSplit(CurrPK);
                        break;
                    #endregion                                 
                  
                    #region PO DEPT
                    case ControlsEnum.PODEPT:
                        PoDept = PurchaseOrderGenerateBL.GetPODepartment(CrDrPk);
                        break;
                    #endregion
                    #region AMOUNTDETAILS
                    case ControlsEnum.AMOUNTDETAILS:
                        dtAmountDetails = new DataTable();
                        dtAmountDetails = BusinessLogic.POInvoicing.POInvoiceBL.GetBalanceAmountDetails(poInvoicePk);
                        break;
                    #endregion                   
                    #region INVOICETYPE
                    case ControlsEnum.INVOICETYPE:
                        dtInvoiceType = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PURCHASE INVOICE TYPE");
                        break; 
                    #endregion
                    #region INVOICECATEGORY (PURCHASE INVOICE LIST TYPE)
                    case ControlsEnum.INVOICECATEGORY:
                        dtInvoiceCategory = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PURCHASE INVOICE LIST TYPE");
                        break;
                    #endregion
                    #region VENDORDETAILSBYPK (For getting vendor default Type(Import/Local))
                    case ControlsEnum.VENDORDETAILSBYPK:
                        vendPK = 0;                      
                        int.TryParse(hdfVendorHd.Value, out vendPK);
                        dtVendorDetails = BusinessLogic.POInvoicing.POInvoiceBL.GetVendorDetails(vendPK);
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
                    #region PENDINGPOLIST
                    case ControlsEnum.PENDINGINVLIST:
                        BindGrid(controlType);
                        break;
                    #endregion
                    #region INVCRDRHEADER
                    case ControlsEnum.INVCRDRHEADER:
                        GetUIValuesFromObject(controlType);
                        break; 
                    #endregion
                    case ControlsEnum.INVCRDRDETAIL:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.DRCRGET:
                        GetUIValuesFromObject(controlType);
                        break;
                    #region Company
                    case ControlsEnum.COMPANY:                       
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
                    case ControlsEnum.UPLOADEDFILES:
                        if (debitcreditHeaderObj != null)
                            GetUIValuesFromObject(ControlsEnum.UPLOADEDFILES);
                        BindGrid(ControlsEnum.UPLOADEDFILES);
                        break;
                    case ControlsEnum.AMOUNTDETAILS:
                        BindGrid(controlType);
                        break;
                    #region INVOICE TYPE
                    case ControlsEnum.INVOICETYPE:
                        BindDropDown(controlType);
                        break;
                    #endregion
                    #region INVOICECATEGORY
                    case ControlsEnum.INVOICECATEGORY:
                        BindDropDown(controlType);
                        break;
                    #endregion
                    #region VENDORDETAILSBYPK
                    case ControlsEnum.VENDORDETAILSBYPK:
                        GetUIValuesFromObject(controlType);                   
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
            if (InvCrDrHeaderSession != null && InvCrDrHeaderSession.InvoiceDetail.Count > 0)
            {
                if (InvCrDrHeaderSession.CDH_STATUS == (int)WkfStatusEnum.DRAFTED)
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
                LinkButton lbnTotalTax;
                bool bIsChecked = false;
                HiddenField hdfDCSplitPK = null;
                HiddenField hdfInvCusDtlPK = null;
                TextBox txtSumSplit;
                TextBox txtQtySplit;
                TextBox txtRateSplit;  

                switch (controlType)
                {
                    #region INVCRDRHEADER
                    case ControlsEnum.INVCRDRHEADER:
                        if (InvCrDrHeaderSession != null)
                        {
                            debitcreditHeaderObj = InvCrDrHeaderSession;
                           
                            debitcreditHeaderObj.CDH_PK = CurrPK;
                            debitcreditHeaderObj.CDH_NO = (string.IsNullOrEmpty(lblDrCrNo.Text) || lblDrCrNo.Text.Trim().Equals("[NEW]")) ? string.Empty : lblDrCrNo.Text.Trim();
                            debitcreditHeaderObj.CDH_DATE = string.IsNullOrEmpty(txtDate.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtDate.Text.Trim();                           
                            debitcreditHeaderObj.CDH_VENDOR = string.IsNullOrEmpty(hdfVendorPK.Value) ? "0" : hdfVendorPK.Value;
                            debitcreditHeaderObj.CDH_VND_CUS_ACCOUNT = string.IsNullOrEmpty(hdfVendorAccountNo.Value) ? 0 : Convert.ToInt32(hdfVendorAccountNo.Value);                                                    
                            //For avoiding XML parsing error : illegal name character (&)
                            debitcreditHeaderObj.CDH_VENDOR_TEXT = HttpUtility.HtmlEncode(debitcreditHeaderObj.CDH_VENDOR_TEXT);
                            debitcreditHeaderObj.CDH_TYPE = Convert.ToByte(ddlMode.SelectedValue);
                            debitcreditHeaderObj.CDH_CURRENCY = string.IsNullOrEmpty(hdfInvoiceCurr.Value) ? 1 : Convert.ToInt32(hdfInvoiceCurr.Value);
                            debitcreditHeaderObj.CDH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                            debitcreditHeaderObj.CDH_BASE_CURR = currentUser.BaseCurrency;
                            debitcreditHeaderObj.CDH_EXCHG_RATE = string.IsNullOrEmpty(hdfExchangeCurr.Value) ? 1 : Convert.ToDouble(hdfExchangeCurr.Value);
                            debitcreditHeaderObj.CDH_REF_NO = HttpUtility.HtmlDecode(txtInstrumentNo.Text.Trim());
                            debitcreditHeaderObj.CDH_IMP_DECL_NO = HttpUtility.HtmlDecode(txtDeclarationNo.Text);
                            if (String.IsNullOrEmpty(txtInstrumentDate.Text.Trim()))
                                debitcreditHeaderObj.CDH_REF_DATE = null;
                            else
                                debitcreditHeaderObj.CDH_REF_DATE = string.IsNullOrEmpty(txtInstrumentDate.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtInstrumentDate.Text.Trim();
                            debitcreditHeaderObj.CDH_STATUS = 0;
                            debitcreditHeaderObj.CDH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            debitcreditHeaderObj.CDH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);
                            debitcreditHeaderObj.CDH_IS_AFFECT_STK = chkAffectStock.Checked ? (byte)1 : (byte)0;
                            debitcreditHeaderObj.CDH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                            debitcreditHeaderObj.CDH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                            debitcreditHeaderObj.USER_PK = Convert.ToInt16(currentUser.PKUser);
                            debitcreditHeaderObj.CDH_MOD_DT = LastModifiedTime;

                            #region Application Code,AST_VALUE,AST_DOC_MODE 
                            int appSubType;
                            appSubType = (!string.IsNullOrEmpty(hdfInvoiceGroup.Value)
                                ? (Convert.ToByte(hdfInvoiceGroup.Value) == (byte)POInvoiceGroup.Services
                                     ? (int)AppSubTypeCNPurchase.NONSTOCK
                                        : (!string.IsNullOrEmpty(hdfPOType.Value)
                                            ? Convert.ToInt16(hdfPOType.Value) == (Int16)POItemType.Others
                                                ? (int)AppSubTypeCNPurchase.NONSTOCK
                                                : (int)AppSubTypeCNPurchase.STOCK
                                            : 0))
                                : 0);
                            appSubType = appSubType > 0 ? appSubType : hdfInvoiceCategory.Value == "1" ? (int)AppSubTypeCNPurchase.STOCK : (int)AppSubTypeCNPurchase.NONSTOCK;  
                               
                            if (ddlMode.SelectedValue == "1")//DEBIT
                            {
                                debitcreditHeaderObj.APT_CODE = ApplicationType.DNT;
                            }
                            else if (ddlMode.SelectedValue == "2")
                            {
                                debitcreditHeaderObj.APT_CODE = ApplicationType.CNT;
                            }                           
                            debitcreditHeaderObj.AST_DOC_MODE = AST_DOC_MODE.Value == "1" ? 1 : 0;
                            debitcreditHeaderObj.AST_VALUE = appSubType.ToString();                                                   
                            #endregion


                            CrDrInvMappingDetails = (List<DebitCreditNoteInvoiceMappingDetails>)SetUIValuesToObject(ControlsEnum.INVCRDRDETAIL);

                            if (CrDrInvMappingDetails != null && CrDrInvMappingDetails.Count > 0 && CrDrInvMappingDetails[0] != null)
                            {
                                debitcreditHeaderObj.CDH_TAX_AMOUNT = Math.Round(CrDrInvMappingDetails.Sum(c => c.CDM_TAX_AMOUNT), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                debitcreditHeaderObj.CDH_AMOUNT_TC = Math.Round(CrDrInvMappingDetails.Sum(c => c.CDM_AMOUNT), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                debitcreditHeaderObj.CDH_AMOUNT_BC = Math.Round(CrDrInvMappingDetails.Sum(c => c.CDM_AMOUNT) * Convert.ToDecimal(debitcreditHeaderObj.CDH_EXCHG_RATE), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                                #region Setting Tax Slno & HtmlEncodding
                                CrDrInvMappingDetails.ForEach(dtl =>
                                                    {
                                                        dtl.IVH_VENDOR_TEXT = HttpUtility.HtmlEncode(dtl.IVH_VENDOR_TEXT);     //For avoiding XML parsing error : illegal name character (&)                           
                                                        if (dtl.ItemDetail != null && dtl.ItemDetail.Count > 0)
                                                        {
                                                            foreach (DebitCreditNoteDetails itm in dtl.ItemDetail)
                                                            {
                                                                itm.CDS_ITEM_TEXT = HttpUtility.HtmlEncode(itm.CDS_ITEM_TEXT);
                                                                itm.CDS_UOM_TEXT = HttpUtility.HtmlEncode(itm.CDS_UOM_TEXT);
                                                                itm.CDS_REMARKS = HttpUtility.HtmlEncode(itm.CDS_REMARKS);
                                                                #region Setting Tax Slno
                                                                if (itm.TaxDetail != null && itm.TaxDetail.Count > 0)
                                                                    itm.TaxDetail.ForEach(f => f.NTD_CDS_SL_NO = itm.CDS_SL_NO);
                                                                #endregion
                                                            }
                                                        }
                                                    });
                                #endregion
                            }

                            debitcreditHeaderObj.InvoiceDetail = CrDrInvMappingDetails;                           
                            debitcreditHeaderObj.FileList = POUploadList;  //Uploads
                        }
                        retObject = debitcreditHeaderObj;
                        break;
                    #endregion   
                    #region INVCRDRDETAIL
                    case ControlsEnum.INVCRDRDETAIL:
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

                            hdfCrDbMpgPK = (HiddenField)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("hdfCrDbMpgPK").ToString());
                            hdfInvoicePK = (HiddenField)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("hdfInvoicePK").ToString());
                            txtAmount = (TextBox)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("txtNoteFor").ToString());
                            HiddenField hdfTotalTax = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTotalTax");
                            HiddenField hdfTaxAmt = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTaxAmt");
                            lbnTotalTax = (LinkButton)grdInvoiceList.Rows[rowID].FindControl("lbnTotalTax");
                            HiddenField hdfItemIncluded = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfItemIncluded");
                            HiddenField hdfTotalAmt = (HiddenField)grdInvoiceList.Rows[rowID].FindControl("hdfTotalAmt");

                            decimal.TryParse(txtAmount.Text, out RaiseNote);
                            if (RaiseNote == 0)
                            {
                                isInvalidRaiseNote = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_RaiseNote").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                break;
                            }
                            int invoicePK = hdfInvoicePK == null ? 0 : Convert.ToInt32(hdfInvoicePK.Value);
                            drcrInvMapDtlObj = CrDrInvMappingDetails.SingleOrDefault(dtl => dtl.CDM_INVOICE_VND_HDR == invoicePK);
                            if (drcrInvMapDtlObj != null)
                            {
                                drcrInvMapDtlObj.CDM_PK = hdfCrDbMpgPK == null ? 0 : Convert.ToInt64(hdfCrDbMpgPK.Value);
                                drcrInvMapDtlObj.CDM_CRDR_NOTE_HDR = CurrPK.ToString();
                                drcrInvMapDtlObj.CDM_INVOICE_VND_HDR = invoicePK;
                                txtAmount = (TextBox)grdInvoiceList.Rows[rowID].FindControl(GetLocalResourceObject("txtNoteFor").ToString());
                                
                                drcrInvMapDtlObj.CDM_AMOUNT = Convert.ToDecimal(txtAmount.Text);
                                drcrInvMapDtlObj.CDM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);

                                hdftotalamt = hdfTotalAmt.Value != string.Empty ? Convert.ToDecimal(hdfTotalAmt.Value) : 0;
                                taxpercentage = hdftax / (hdftotalamt == 0 ? 1 : hdftotalamt);
                                basevalue = (Convert.ToDecimal(txtAmount.Text)) / (1 + taxpercentage);
                                ttaxamt = (Convert.ToDecimal(txtAmount.Text) - basevalue);
                                lbnTotalTax = (LinkButton)grdInvoiceList.Rows[rowID].FindControl("lbnTotalTax");

                                drcrInvMapDtlObj.CDM_TAX_AMOUNT =!string.IsNullOrEmpty(hdfTotalTax.Value)? Math.Round(Convert.ToDecimal(hdfTotalTax.Value), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits):0;

                                int.TryParse(hdfItemIncluded.Value, out ItemIncluded);
                                if (ItemIncluded == 1)
                                {
                                    drcrInvMapDtlObj.CDM_AMOUNT = drcrInvMapDtlObj.CDM_AMOUNT + drcrInvMapDtlObj.CDM_TAX_AMOUNT;
                                }
                            }                          
                            rowID++;                           
                        }
                        retObject = CrDrInvMappingDetails;
                        break;
                    #endregion
                    #region CRDR SPLIT LIST
                    case ControlsEnum.CRDRSPLITLIST:
                        rowID = 0;
                        finCrDrNoteDtlList = new List<DebitCreditNoteDetails>();
                        foreach (GridViewRow grdrow in grdDCSplit.Rows)//
                        {
                            hdfDCSplitPK = (HiddenField)grdDCSplit.Rows[rowID].FindControl("hdfDCSplitPK");
                            hdfInvCusDtlPK = (HiddenField)grdDCSplit.Rows[rowID].FindControl("hdfInvCusDtlPK");
                            txtQtySplit = (TextBox)grdDCSplit.Rows[rowID].FindControl("txtQtySplit");
                            txtRateSplit = (TextBox)grdDCSplit.Rows[rowID].FindControl("txtRateSplit");
                            txtSumSplit = (TextBox)grdDCSplit.Rows[rowID].FindControl("txtSumSplit");
                            TextBox txtTAXSplitTotal = (TextBox)grdDCSplit.Rows[rowID].FindControl("txtTAXSplitTotal");
                            CheckBox chkAffectStkSplit = (CheckBox)grdDCSplit.Rows[rowID].FindControl("chkAffectStkSplit");

                            finCrDrCusSoMpgObj = CrDrInvMappingDetails.SingleOrDefault(dtl => dtl.CDM_INVOICE_VND_HDR == InvoicePK).ItemDetail.SingleOrDefault(f => f.CDS_INVOICE_VND_DTL == Convert.ToInt32(hdfInvCusDtlPK.Value));
                            if (finCrDrCusSoMpgObj != null)
                            {
                                finCrDrCusSoMpgObj.CDS_PK = hdfDCSplitPK == null ? 0 : Convert.ToInt32(hdfDCSplitPK.Value);
                                finCrDrCusSoMpgObj.CDS_CRDR_NOTE_HDR = CurrPK;
                                finCrDrCusSoMpgObj.CDS_CRDR_NOTE_MPG = CrDrMpgPK;
                                finCrDrCusSoMpgObj.CDS_INVOICE_VND_DTL = hdfInvCusDtlPK == null ? 1 : Convert.ToInt32(hdfInvCusDtlPK.Value);
                                finCrDrCusSoMpgObj.CDS_QTY = txtQtySplit == null ? 0 : txtQtySplit.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(txtQtySplit.Text.Trim());
                                finCrDrCusSoMpgObj.CDS_RATE = txtRateSplit == null ? 0 : txtRateSplit.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(txtRateSplit.Text.Trim());
                                finCrDrCusSoMpgObj.CDS_AMOUNT = txtSumSplit == null ? 0 : txtSumSplit.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtSumSplit.Text.Trim());
                                finCrDrCusSoMpgObj.CDS_DISCOUNT = 0;
                                finCrDrCusSoMpgObj.CDS_TAX = string.IsNullOrEmpty(txtTAXSplitTotal.Text.Trim()) ? 0 : Convert.ToDecimal(txtTAXSplitTotal.Text.Trim());
                                finCrDrCusSoMpgObj.CDS_NET_AMOUNT = Convert.ToDecimal(txtSumSplit.Text.Trim()) - (finCrDrCusSoMpgObj.CDS_DISCOUNT + finCrDrCusSoMpgObj.CDS_TAX);
                                finCrDrCusSoMpgObj.CDS_REMARKS = null;
                                finCrDrCusSoMpgObj.CDS_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                finCrDrCusSoMpgObj.CDS_IS_AFFECT_STK = chkAffectStkSplit.Checked ? (byte)1 : (byte)0;

                                #region Tax  Amount Splitup Calculation
                                if (finCrDrCusSoMpgObj.CDS_TAX > 0)
                                {
                                    decimal InvAmount = finCrDrCusSoMpgObj.CDS_VID_AMOUNT;
                                    decimal TotalTaxPercentage = 1;
                                    List<DebitCreditNoteTaxDtl> objInvoiceTaxList = finCrDrCusSoMpgObj.TaxDetail.Where(inv => inv.NTD_TAX_CATEGORY == (byte)TaxType.Tax).ToList();
                                    if (objInvoiceTaxList != null && objInvoiceTaxList.Count > 0)
                                    {
                                        TotalTaxPercentage = objInvoiceTaxList.Sum(tx => (tx.NTD_VTL_TAX_AMT * 100) / InvAmount);

                                        foreach (DebitCreditNoteTaxDtl invtaxdet in objInvoiceTaxList)
                                        {
                                            if (invtaxdet.NTD_VTL_TAX_AMT > 0)
                                            {
                                                decimal IndividualPercentage = (invtaxdet.NTD_VTL_TAX_AMT * 100) / InvAmount;
                                                decimal TaxAmnt = (finCrDrCusSoMpgObj.CDS_TAX * IndividualPercentage) / TotalTaxPercentage;
                                                double Amount = CommonFunctions.DoubleFormatRound(Convert.ToDouble(TaxAmnt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                                                invtaxdet.NTD_AMOUNT = Convert.ToDecimal(Amount);
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            finCrDrNoteDtlList.Add(finCrDrCusSoMpgObj);
                            rowID++;
                        }
                        retObject = finCrDrNoteDtlList;
                        break;
                    #endregion                   
                    #region JOURNALIZE
                    case ControlsEnum.JOURNALIZE:                      
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

                                if (InvCrDrHeaderSession.CDH_TYPE == 1)
                                {
                                    ucrJournalize.TransactionType = ApplicationType.DNTJ;
                                    Session[ERP.Utilities.SessionStrings.TransactionType] = ApplicationType.DNTJ;
                                    Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.DNTJ;
                                    Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.DNTJ;
                                    Session[ERP.Utilities.SessionStrings.JournalHead] = hdfJournalHeader.Value = GetLocalResourceObject("Debit_Note_Journal").ToString();

                                }
                                else if (InvCrDrHeaderSession.CDH_TYPE == 2)
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
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = InvCrDrHeaderSession.CDH_NO;
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = InvCrDrHeaderSession.CDH_DATE;
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = InvCrDrHeaderSession.CDH_CURRENCY;
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.AP;
                                Session[ERP.Utilities.SessionStrings.AccountPayablePK] = InvCrDrHeaderSession.CDH_VENDOR;


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
                                SetCancelRef((int)CurrPK);
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
                                ucrJournalize.TypeForNumberGenaration = appSubType.ToString();
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
            try
            {
                switch (controlType)
                {
                    #region INVCRDRHEADER
                    case ControlsEnum.INVCRDRHEADER:
                        debitcreditHeaderObj = InvCrDrHeaderSession;
                        if (debitcreditHeaderObj != null)
                        {
                            lblDrCrNo.Text = string.IsNullOrEmpty(debitcreditHeaderObj.CDH_NO) ? Resources.ErpRes.Draft : debitcreditHeaderObj.CDH_NO;
                            txtVendorHd.Text = ERP.Utilities.CommonFunctions.GetDecodedString(debitcreditHeaderObj.CDH_VENDOR_TEXT);
                            hdfVendorHd.Value = debitcreditHeaderObj.CDH_VENDOR.ToString();
                            txtDate.Text = debitcreditHeaderObj.CDH_DATE;
                            txtPaymentCurrency.Text = debitcreditHeaderObj.CDH_CURRENCY_TEXT;
                            hdfPaymentCurrency.Value = debitcreditHeaderObj.CDH_CURRENCY.ToString();
                            if (ddlMode.Items.Count > 0)
                            {
                                if (Convert.ToInt32(debitcreditHeaderObj.CDH_TYPE) > 0)
                                    ddlMode.SelectedValue = debitcreditHeaderObj.CDH_TYPE.ToString();
                            }
                            txtPaidAmount.Text = Math.Round(debitcreditHeaderObj.CDH_AMOUNT_TC, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            txtRemarks.Text = HttpUtility.HtmlDecode(debitcreditHeaderObj.CDH_REMARKS);
                            Approved = debitcreditHeaderObj.CDH_STATUS;                          
                            ddlCompany.SelectedValue = debitcreditHeaderObj.CDH_COMPANY.ToString();
                            txtInstrumentNo.Text = HttpUtility.HtmlDecode(debitcreditHeaderObj.CDH_REF_NO);
                            txtInstrumentDate.Text = debitcreditHeaderObj.CDH_REF_DATE;
                            txtDeclarationNo.Text = HttpUtility.HtmlDecode(debitcreditHeaderObj.CDH_IMP_DECL_NO);
                            txtExchangeRate.Text = debitcreditHeaderObj.CDH_EXCHG_RATE.ToString();
                            chkAffectStock.Checked = debitcreditHeaderObj.CDH_IS_AFFECT_STK == 1 ? true : false;
                            #region IsMultiplePlant
                            if (Convert.ToInt32(hdfIsMultiplePlant.Value) == 1)
                            {
                                lblCompanyView.Visible = true;
                                ddlCompanyView.Visible = true;
                                ddlCompanyView.Enabled = true;
                                ddlCompanyView.SelectedValue = debitcreditHeaderObj.CDH_COMPANY.ToString();
                                ddlCompanyView.Enabled = false;
                            }
                            else
                            {
                                lblCompanyView.Visible = false;
                                ddlCompanyView.Visible = false;
                            } 
                            #endregion
                            ModifiedDatePnl.Visible = true;
                            LastModifiedTime = debitcreditHeaderObj.CDH_MOD_DT;
                            lblLastModifiedHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);

                            hdfInvoiceGroup.Value = debitcreditHeaderObj.CDH_IVH_GROUP.ToString();
                            hdfVendorPK.Value = debitcreditHeaderObj.CDH_VENDOR.ToString();
                            hdfVendorAccountNo.Value = debitcreditHeaderObj.CDH_VND_CUS_ACCOUNT.ToString();
                            hdfInvoiceCurr.Value = debitcreditHeaderObj.CDH_CURRENCY.ToString();
                            lblPaidAmount.Text = GetLocalResourceObject("Amount").ToString() + " (" + debitcreditHeaderObj.CDH_CURRENCY_TEXT + ")";
                            GetFieldValues(ControlsEnum.EXCHANGERATE);
                            if (debitcreditHeaderObj.CDH_IVH_TYPE != 1)
                            {
                                txtExchangeRate.CssClass = "input-small numeric input-disabled";
                                txtExchangeRate.Enabled = false;
                            }
                            else
                            {
                                txtExchangeRate.CssClass = "input-small numeric ";
                                txtExchangeRate.Enabled = true;
                            }

                            CrDrInvMappingDetails = debitcreditHeaderObj.InvoiceDetail.ToList();//Setting Invoice mapping details List

                            #region Setting Remarks by default
                            string remarks = string.Empty;
                            int newline = 1;
                            foreach (DebitCreditNoteInvoiceMappingDetails item in CrDrInvMappingDetails)
                            {
                                remarks = remarks + item.IVH_VENDOR_INV_NO + " " + (item.IVH_DATE_RECEIVED != null ? (string.IsNullOrEmpty(item.IVH_DATE_RECEIVED.ToString()) == true ? string.Empty : item.IVH_DATE_RECEIVED.ToString(Resources.Constants.DateFormatShort)) : string.Empty) + (newline == CrDrInvMappingDetails.Count ? string.Empty : Environment.NewLine);
                                newline++;
                            }
                            #endregion
                            txtRemarks.Text = HttpUtility.HtmlDecode(remarks);

                            #region Setting the slno
                            int newCDMslno = 1;
                            CrDrInvMappingDetails.ForEach(dtl =>
                            {
                                dtl.CDM_SL_NO = newCDMslno;
                                if (dtl.ItemDetail != null && dtl.ItemDetail.Count > 0)
                                {
                                    #region Setting Mapping & ItemDetail Linking  Slno
                                    int newCDSslno = 1;
                                    foreach (DebitCreditNoteDetails itm in dtl.ItemDetail)
                                    {
                                        itm.CDS_SL_NO = newCDSslno;
                                        itm.CDS_CDM_SL_NO = newCDMslno;
                                        newCDSslno++;
                                    }
                                    #endregion
                                }
                                newCDMslno++;
                            });
                            #endregion
                        }
                        break;
                    #endregion
                    #region CRDRSPLITLIST
                    case ControlsEnum.CRDRSPLITLIST:                      
                        if (tempfinCrDrCusMpgList != null && tempfinCrDrCusMpgList.Count > 0)
                        {
                            lblDCSplitNo.Text = ERP.Utilities.CommonFunctions.GetShortString(tempfinCrDrCusMpgList[0].IVH_NO, 20);
                            lblDCSplitNo.ToolTip = tempfinCrDrCusMpgList[0].IVH_NO;
                            lblDCSplitDate.Text = lblDCSplitDate.ToolTip = tempfinCrDrCusMpgList[0].IVH_DATE;
                            lblDCSplitSupplier.Text = ERP.Utilities.CommonFunctions.GetShortString(tempfinCrDrCusMpgList[0].IVH_VENDOR_TEXT, 50);
                            lblDCSplitSupplier.ToolTip = tempfinCrDrCusMpgList[0].IVH_VENDOR_TEXT;
                            lblDCSplitAmount.Text = lblDCSplitAmount.ToolTip = String.Format("{0:c}", tempfinCrDrCusMpgList[0].IVH_INVOICE_AMT);  
                            lblDCSplitReceiveNow.Text = lblDCSplitReceiveNow.ToolTip =String.Format("{0:c}", PayNowAmount);                           
                            lbltaxSplitpopup.Text = String.Format("{0:c}", paynowtax);
                            hdfInvGroup.Value = tempfinCrDrCusMpgList[0].IVH_GROUP.ToString();

                            drcrNoteItemDetailList = tempfinCrDrCusMpgList[0].ItemDetail;
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
                    #region UPLOADEDFILES
                    case ControlsEnum.UPLOADEDFILES:
                        CurrPK = debitcreditHeaderObj.CDH_PK;
                        POUploadList = debitcreditHeaderObj.FileList;
                        break;

                    #endregion
                    #region VENDORDETAILSBYPK
                    case ControlsEnum.VENDORDETAILSBYPK:
                        if (dtVendorDetails != null && dtVendorDetails.Rows.Count > 0)
                        {
                            ddlPendingInvType.SelectedIndex = ddlPendingInvType.Items.IndexOf(ddlPendingInvType.Items.FindByValue(dtVendorDetails.Rows[0]["VEN_PO_TYPE"].ToString()));
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

            #region Purchase Item/Header Tax/Discount Settings
            IsHeaderTaxForTradingPurchase = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsHeaderTaxForTradingPurchase")));
            IsHeaderDiscountForTradingPurchase = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsHeaderDiscountForTradingPurchase")));
            IsItemwiseTaxForTradingPurchase = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsItemwiseTaxForTradingPurchase")));
            IsItemwiseDiscountForTradingPurchase = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsItemwiseDiscountForTradingPurchase")));
            #endregion

            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
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
                        if (InvHeaderObj != null && InvHeaderObj.INVList != null && InvHeaderObj.INVList.Count > 1)
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
                    if (ddlPendingInvType.Items.Count > 1)
                        ddlPendingInvType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region INVOICECATEGORY
                case ControlsEnum.INVOICECATEGORY:
                    ddlPendingInvCategory.Items.Clear();
                    if (dtInvoiceCategory != null && dtInvoiceCategory.Rows.Count > 0)
                    {
                        ddlPendingInvCategory.DataSource = dtInvoiceCategory;
                        ddlPendingInvCategory.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlPendingInvCategory.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlPendingInvCategory.DataBind();
                    }                  
                    ddlPendingInvCategory.Items.Cast<ListItem>().Where(i => i.Value == "2").ToList().ForEach(i => ddlPendingInvCategory.Items.Remove(i));//In the case of Advance invoice,Pick for CRDR option is not  available in Old page 
                    if (ddlPendingInvCategory.Items.Count > 1)
                        ddlPendingInvCategory.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
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
                int rowCount = 0;
                int pageSize = 0;
                switch (controlType)
                {
                    #region DRCRHDRLIST
                    case ControlsEnum.DRCRHDRLIST:
                        //Paging Properties                       
                        uclPaging.TotalPages = TotalPages;
                        PageIndex = string.IsNullOrEmpty(PageIndex) ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        if (dtInvoiceList != null)
                        {
                            grdCrDbHdr.PageIndex = Convert.ToInt32(PageIndex);
                            grdCrDbHdr.DataSource = dtInvoiceList.DefaultView;
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
                    #region PENDINGINVLIST
                    case ControlsEnum.PENDINGINVLIST:
                        #region Paging Properties
                        rowCount = 0;
                        pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize_InvList"));
                        rowCount = dtPendingInvList.Rows.Count > 0 ? Convert.ToInt32(dtPendingInvList.Rows[0]["TOTAL_ROW_COUNT"].ToString()) : 0;
                        uclPendingInvPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                      (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                      (rowCount / pageSize) + 1;
                        PageIndexInv = PageIndexInv == null ? CommonConstants.SELECT_VALUE_ONE : PageIndexInv;
                        uclPendingInvPaging.CurrentPage = Convert.ToInt32(PageIndexInv);
                        #endregion
                        if (dtPendingInvList != null && dtPendingInvList.Rows.Count > 0)
                        {
                            grdPendingInvList.DataSource = dtPendingInvList;
                            grdPendingInvList.DataBind();
                            uclPendingInvPaging.Visible = true;
                            uclPendingInvPaging.BindPager();
                        }
                        else
                        {
                            grdPendingInvList.DataSource = null;
                            grdPendingInvList.DataBind();
                            uclPendingInvPaging.Visible = false;
                        }

                        break;
                    #endregion
                    #region INVCRDRDETAIL
                    case ControlsEnum.INVCRDRDETAIL:
                        if (InvCrDrHeaderSession != null)
                        {
                            List<DebitCreditNoteInvoiceMappingDetails> poInvoiceTList;
                            drcrInvoiceMappingDetailList = new List<DebitCreditNoteInvoiceMappingDetails>();

                            poInvoiceTList = new List<DebitCreditNoteInvoiceMappingDetails>();
                            poInvoiceTList = InvCrDrHeaderSession.InvoiceDetail.ToList();
                            drcrInvoiceMappingDetailList.AddRange(poInvoiceTList);

                            if (drcrInvoiceMappingDetailList != null)
                            {
                                grdInvoiceList.DataSource = drcrInvoiceMappingDetailList;
                                grdInvoiceList.DataBind();
                            }
                            else
                            {
                                grdInvoiceList.DataSource = null;
                                grdInvoiceList.DataBind();
                            }
                        }
                        break;
                    #endregion
                    #region CRDRSPLITLIST
                    case ControlsEnum.CRDRSPLITLIST:
                        if (drcrNoteItemDetailList != null)
                        {
                            grdDCSplit.DataSource = drcrNoteItemDetailList;
                            grdDCSplit.DataBind();
                        }
                        else 
                        {
                            grdDCSplit.DataSource = null;
                            grdDCSplit.DataBind();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalSplit", "$(document).ready(function(){CalculateTotalSplit();});", true);
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
                    #region AMOUNTDETAILS
                    case ControlsEnum.AMOUNTDETAILS:
                        if (dtAmountDetails != null)
                        {
                            grdPaidAmntSplitup.DataSource = dtAmountDetails;
                            grdPaidAmntSplitup.DataBind();
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
        private void ResetForm()
        {

            ResetForm(ControlsEnum.INVCRDRHEADER);
            txtSearchDateFrom.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
            hdfSearchDateFrom.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
            txtSearchDateTo.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            hdfSearchDateTo.Value = DateTime.Now.ToString();        
            base.WkfRefID = 0;          
            SortBy = Resources.DataFieldRes.CrDrDate;
            ThenBy = Resources.DataFieldRes.CrDrNo;
            ddlStatus.SelectedIndex = 0;
            ddlCreditDebitType.SelectedIndex = 0;           
            PageIndex = "1";
            ddlCompanySrch.SelectedIndex = -1;          
            txtInvoiceNo.Text = string.Empty;
            ddlMode.SelectedValue = CommonConstants.SELECTVAL;
        }

        private void ResetForm(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                #region ADDITEM
                case ControlsEnum.ADDITEM:
                    anchorFile.Visible = false;
                    vrfFileUpload.Enabled = true;
                    CurrSlNo = 0;
                    anchorFile.Attributes.Remove("onclick");
                    break;
                #endregion
                #region INVCRDRHEADER
                case ControlsEnum.INVCRDRHEADER:
                    CurrPK = 0;
                    InvCrDrHeaderSession = null;
                    CrDrInvMappingDetails = null;
                    drcrNoteItemDetailList = null;
                    POUploadList = null;
                    FileDetailsList = null;
                    IsDeleted = false;
                    dtPendingInvList = null;
                    txtVendor.Text = string.Empty;
                    hdfVendorID.Value = string.Empty;
                    txtCrDrNumber.Text = string.Empty;
                    hdfCrDrNumber.Value = string.Empty;
                    txtRemarks.Text = string.Empty;
                    txtPaidAmount.Text = string.Empty;                 
                    ModifiedDatePnl.Visible = false;
                    lblLastModifiedHDR.Text = string.Empty;

                    grdInvoiceList.DataSource = null;
                    grdInvoiceList.DataBind();  
                    grdPendingInvList.DataSource = null;
                    grdPendingInvList.DataBind();                  
                    grdUploads.DataSource = null;
                    grdUploads.DataBind();
                   
                    txtVendorHd.Text = string.Empty;
                    hdfVendorHd.Value = string.Empty;
                    lblDrCrNo.Text = Resources.ErpRes.Draft;                   
                    ddlMode.SelectedValue = CommonConstants.SELECTVAL;
                    txtPendingFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfPendingFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    txtPendingToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfPendingToDate.Value = DateTime.Now.ToString();
                    ddlPendingInvCategory.ClearSelection();
                    ddlPendingInvType.ClearSelection();
                    base.WkfRefID = ucrWrkf.RefID = 0;
                    SetCancelRef(CurrPK);
                    ucrWrkf.FillWorkFlowDetails();
                    ucrWrkf.ViewType = 1;
                    ucrWrkf.ViewAction();

                    ResetForm(ControlsEnum.ADDITEM);
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
        private void ResetAfterJournalize()
        {
            hdfJournalizeWorkFlow.Value = "0";
            ucrWrkf.Reset();
            FillProcessID(1);
            ResetForm();
            EntryStatus = EntryStatus.LISTMODE;
            Session[ERP.Utilities.SessionStrings.TransactionType] = null;
            GetFieldValues(ControlsEnum.DRCRHDRLIST);
            SetFieldValues(ControlsEnum.DRCRHDRLIST);
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

            #region  Declaration 
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            FinCrDrHdrNoteService finCrDrHdrNoteServiceClient;
            finCrDrHdrNoteServiceClient = null; 
            TextBox WrkfComments;        
            bool bIsChecked = false;   
            Label lblTotalPayNowFooter;
            WorkflowCore.CoreService workflowCore;
            List<DebitCreditNoteInvoiceMappingDetails> tempInvoiceSOSplitList;
            FileInfo tempFileInfoObj;
            string savePath = string.Empty;            
            long? result;
            result = 0;            
            int selectedItemPK;
            #endregion
            try
            {
              
                #region CommonActions Settings
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
                #endregion
                switch (commonActions)
                {                   
                    #region NEW
                    case ActionsEnum.NEW:
                        ResetForm(ControlsEnum.INVCRDRHEADER);                      
                        FillProcessID(1);
                        EntryStatus = EntryStatus.NEWMODE;
                        TotalPages = 0;  
                        uclPendingInvPaging.CurrentPage = 1;
                        hdfIsPendingInvVisible.Value = "0";
                        SetFieldValues(ControlsEnum.PENDINGINVLIST);
                        break;
                    #endregion
                    #region ADD TO LIST
                    case ActionsEnum.ADDTOLIST:
                        InvHeaderObj = new DirectInvHeaderBO();
                        InvHeaderObj.INVList = new List<DirectInvHeaderListBO>();
                        List<DirectInvHeaderListBO> objItemList = new List<DirectInvHeaderListBO>();
                        DirectInvHeaderListBO objPoList;
                        HiddenField hdfInvID;
                        HiddenField hdfPOVendor;
                        HiddenField hdfInvType;
                        HiddenField hdfInvCurrency;
                        HiddenField hdfIVHGroup;
                        #region grdPendingInvList
                        foreach (GridViewRow grdrow in grdPendingInvList.Rows)
                        {
                            CheckBox chkInvPendSelect = (CheckBox)grdrow.FindControl("chkInvPendSelect");
                            if (chkInvPendSelect.Checked)
                            {
                                hdfInvID = (HiddenField)grdrow.FindControl("hdfInvoiceID");
                                hdfPOVendor = (HiddenField)grdrow.FindControl("hdfPOVendorPK");
                                hdfInvType = (HiddenField)grdrow.FindControl("hdfInvType");
                                hdfInvCurrency = (HiddenField)grdrow.FindControl("hdfInvCurrency");
                                hdfIVHGroup = (HiddenField)grdrow.FindControl("hdfIVHGroup");
                                objPoList = new DirectInvHeaderListBO();
                                objPoList.IVH_PK = Convert.ToInt32(hdfInvID.Value);
                                objPoList.IVH_VENDOR = Convert.ToInt32(hdfPOVendor.Value);
                                objPoList.IVH_TYPE = Convert.ToInt32(hdfInvType.Value); //PURCHASE INVOICE TYPE: Import=>1,	Local => 2
                                objPoList.IVH_CURRENCY = Convert.ToInt32(hdfInvCurrency.Value);
                                objPoList.IVH_GROUP = Convert.ToInt32(hdfIVHGroup.Value);
                                if (objItemList != null && (objItemList.Where(r => r.IVH_VENDOR != Convert.ToInt32(hdfPOVendor.Value)).Count() > 0
                                                            || objItemList.Where(r => r.IVH_TYPE != Convert.ToInt32(hdfInvType.Value)).Count() > 0
                                                            || objItemList.Where(r => r.IVH_GROUP != Convert.ToInt32(hdfIVHGroup.Value)).Count() > 0
                                                            || objItemList.Where(r => r.IVH_CURRENCY != Convert.ToInt32(hdfInvCurrency.Value)).Count() > 0
                                                           )
                                    )
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_Muliple_Invoice").ToString()) + "');", true);
                                    return;
                                }
                                if (objItemList != null && objItemList.Where(r => r.IVH_PK == Convert.ToInt32(hdfInvID.Value)).Count() <= 0)
                                    objItemList.Add(objPoList);
                            }
                        }
                        #endregion
                        if (objItemList.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoRecordsSelected").ToString()) + "');", true);
                            return;
                        }
                        InvHeaderObj.INVList = objItemList;
                        //Avoid already added PO.No need to get that PO details again
                        if (objItemList != null && objItemList.Count > 0 && InvCrDrHeaderSession != null && InvCrDrHeaderSession.InvoiceDetail != null)
                        {
                            List<string> objPoPkList = InvCrDrHeaderSession.InvoiceDetail.Select(r => r.CDM_CRDR_NOTE_HDR).Distinct().ToList();
                            InvHeaderObj.INVList = objItemList.Where(r => !objPoPkList.Contains(r.IVH_PK.ToString())).ToList();
                        }
                        GetFieldValues(ControlsEnum.CRDRTYPE);
                        SetFieldValues(ControlsEnum.CRDRTYPE);//Remove Credit while choosing multiple Purchase invoice
                        GetFieldValues(ControlsEnum.INVCRDRHEADER);                                   
                        SetFieldValues(ControlsEnum.INVCRDRHEADER);
                        SetFieldValues(ControlsEnum.INVCRDRDETAIL);
                        SetFieldValues(ControlsEnum.UPLOADEDFILES);
                        TempInvCrDrHeaderSession = InvCrDrHeaderSession;
                      
                        EnableDisableDrCrMode();                      
                        GetFieldValues(ControlsEnum.LINEITEMTAXSETTINGS);
                        SetFieldValues(ControlsEnum.LINEITEMTAXSETTINGS);

                        hdfIsPendingInvVisible.Value = "0";
                        SetFieldValues(ControlsEnum.PENDINGINVLIST);
                        break;
                    #endregion
                    #region REMOVE (Remove Invoice From List)
                    case ActionsEnum.REMOVE:
                        int invPk = int.Parse(((Button)sender).CommandArgument.ToString());                                       
                        if (InvCrDrHeaderSession != null && InvCrDrHeaderSession.InvoiceDetail != null)
                        {
                            DebitCreditNoteInvoiceMappingDetails objMappingDtl = InvCrDrHeaderSession.InvoiceDetail.SingleOrDefault(r => r.CDM_INVOICE_VND_HDR == invPk);
                            InvCrDrHeaderSession.InvoiceDetail.Remove(objMappingDtl);
                            SetFieldValues(ControlsEnum.INVCRDRHEADER);
                            SetFieldValues(ControlsEnum.INVCRDRDETAIL);
                            SetFieldValues(ControlsEnum.PENDINGINVLIST);
                        }
                        break;
                    #endregion
                    #region DTL SEARCH/VENDOR CHANGE
                    case ActionsEnum.DTLSEARCH:
                    case ActionsEnum.VENDORSELECTED:
                        uclPendingInvPaging.CurrentPage = 1;
                        PageIndexInv = CommonConstants.SELECT_VALUE_ONE;
                        dtPendingInvList = null;
                        GetFieldValues(ControlsEnum.VENDORDETAILSBYPK);
                        SetFieldValues(ControlsEnum.VENDORDETAILSBYPK);//Set Type(Import/Local) of the Vendor by default in Pending invoice Search area
                        GetFieldValues(ControlsEnum.PENDINGINVLIST);
                        SetFieldValues(ControlsEnum.PENDINGINVLIST);
                        hdfIsPendingInvVisible.Value = "1";
                        break;
                    #endregion
                    #region DTL CLEAR SEARCH
                    case ActionsEnum.DTLCLEARSEARCH:
                        txtPurchaseInvNumber.Text = string.Empty;
                        hdfPurchaseInvPK.Value = string.Empty;                       
                        txtPendingFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                        hdfPendingFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                        txtPendingToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                        hdfPendingToDate.Value = DateTime.Now.ToString();
                        ddlPendingInvCategory.ClearSelection();
                        ddlPendingInvType.ClearSelection();
                        uclPendingInvPaging.CurrentPage = 1;
                        PageIndexInv = CommonConstants.SELECT_VALUE_ONE;
                        dtPendingInvList = null;
                        GetFieldValues(ControlsEnum.PENDINGINVLIST);
                        SetFieldValues(ControlsEnum.PENDINGINVLIST);
                        hdfIsPendingInvVisible.Value = "1";
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
                            if (grdInvoiceList.Rows.Count >= 1)
                            {                                
                                debitcreditHeaderObj = new  DebitCreditTradingBO();
                                debitcreditHeaderObj = (DebitCreditTradingBO)SetUIValuesToObject(ControlsEnum.INVCRDRHEADER);

                                lblTotalPayNowFooter = (Label)grdInvoiceList.FooterRow.FindControl("lblTotalPayNowFooter");
                                lblTotalPayNowFooter.Text = hdfTotalPayNowFooter.Value;

                                if (debitcreditHeaderObj != null && debitcreditHeaderObj.InvoiceDetail != null && !isInvalidRaiseNote)
                                {
                                    debitcreditHeaderObj.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                    string xmlDoc = CommonFunctions.XmlSerialize<DebitCreditTradingBO>(debitcreditHeaderObj);
                                    bool isCont = true;                                  
                                    if (isCont)
                                    {
                                        string invNumber = string.Empty;
                                        result = BusinessLogic.POInvoicing.DebitCreditTradingBL.SaveCreditDebitTradingWkf(xmlDoc, out invNumber);//SPFIN_CRDR_VND_SAVE
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

                                        foreach (DebitCreditNoteUploads obj in POUploadList)
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
                                       
                                        #region Show Save Message and redired to listing page
                                        string crdrNo = string.Empty;
                                        if (string.IsNullOrEmpty(lblDrCrNo.Text.Trim()) || lblDrCrNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Saved_Success").ToString();
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                                            crdrNo = lblDrCrNo.Text.Trim();
                                            object[] args = new object[2];
                                            args[0] = Resources.PageNameRes.CreditDebitNotesTrading;
                                            args[1] = crdrNo;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                                        }
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                        EntryStatus = EntryStatus.LISTMODE;
                                        ResetForm();
                                        GetFieldValues(ControlsEnum.DRCRHDRLIST);
                                        SetFieldValues(ControlsEnum.DRCRHDRLIST);
                                        InvCrDrHeaderSession = null;
                                        #endregion
                                    }
                                    else if (result == -25)
                                    {                                        
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_RefnoExist").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -26)//Debit Credit Note is referred in other transactions.
                                    {                                        
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_CrdrUsed").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -27)//ie, EMI/EMR is created 
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_CrdrUsed").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    //Checking for EMI/EMR is created or not
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
                        }
                        break;
                    #endregion                       
                    #region  EDIT/VIEW/CREDITDEBITDETAIL
                    case ActionsEnum.EDIT:
                    case ActionsEnum.VIEW:
                    case ActionsEnum.CREDITDEBITDETAIL:
                        ResetForm(ControlsEnum.INVCRDRHEADER);
                        foreach (GridViewRow grdrow in grdCrDbHdr.Rows)
                        {
                            #region grdCrDbHdr
                            RadioButton rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID);
                            HiddenField hdfDept;                            
                            int dept;                          
                            if (rbtn.Checked)  // check row selected or not
                            {
                                bIsChecked = true;
                                // get pk from the grid and assign to CurrPk
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCrDrPk")).Value); 
                                Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                                Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).Text;
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                hdfCrDrNumber.Value = CurrPK.ToString();
                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfDelStatusCrDr")).Value))
                                {
                                    btnEditforCancel.Visible = false;
                                    btnSaveDrcr.Visible = false;
                                    btnEdit.Visible = false;
                                    IsDeleted = true;
                                    hdfIsCancelled.Value = "1";
                                }
                                else
                                {
                                    btnSaveDrcr.Visible = true;
                                    btnEdit.Visible = true;
                                    IsDeleted = false;
                                    hdfIsCancelled.Value = "0";
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
                            workflowCore = new WorkflowCore.CoreService();
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
                            GetFieldValues(ControlsEnum.INVCRDRHEADER);

                            SetFieldValues(ControlsEnum.INVCRDRHEADER);
                            SetFieldValues(ControlsEnum.INVCRDRDETAIL);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            TempInvCrDrHeaderSession = InvCrDrHeaderSession;
                            

                            uclPendingInvPaging.CurrentPage = 1;
                            PageIndexInv = CommonConstants.SELECT_VALUE_ONE;
                            GetFieldValues(ControlsEnum.PENDINGINVLIST);
                            SetFieldValues(ControlsEnum.PENDINGINVLIST);
                            hdfIsPendingInvVisible.Value = "0";
                        }
                        else
                        {
                            // if no items selected, Show Error Message
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion                                      
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            string appType = ddlMode.SelectedValue == "1" ? ApplicationType.DNT : ApplicationType.CNT;
                            result = BusinessLogic.POInvoicing.DebitCreditTradingBL.DeleteCreditDebitTradingDetails(CurrPK, LastModifiedTime, appType, currentUser.PKUser.ToString());
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
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:                      
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
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:                      
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
                        break;
                   #endregion
                    #region WRKFSUBMIT
                    case ActionsEnum.WRKFSUBMIT:
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
                                if (grdInvoiceList.Rows.Count >= 1)
                                {
                                    debitcreditHeaderObj = new DebitCreditTradingBO();
                                    debitcreditHeaderObj = (DebitCreditTradingBO)SetUIValuesToObject(ControlsEnum.INVCRDRHEADER);
                                    debitcreditHeaderObj.WKF_FLAG = 1;
                                    if (debitcreditHeaderObj != null && debitcreditHeaderObj.InvoiceDetail != null && !isInvalidRaiseNote)
                                    {
                                        debitcreditHeaderObj.ATL_ACTION = (byte)LogAction.NEW;
                                        SaveTransaction(debitcreditHeaderObj, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
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
                                    GetFieldValues(ControlsEnum.DRCRHDRLIST);
                                    SetFieldValues(ControlsEnum.DRCRHDRLIST);
                                }
                            }
                            else
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                        }
                        break;
                    #endregion
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdCrDbHdr.Rows)
                        {
                            HiddenField hdfDept;
                            int dept;
                            RadioButton rbtnCrDr;
                            rbtnCrDr = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtnCrDr.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCrDrPk")).Value);
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                HiddenField hdfCrDrStatus = grdrow.FindControl("hdfCrDrStatus") as HiddenField;
                                if (!string.IsNullOrEmpty(hdfCrDrStatus.Value) && Convert.ToInt32(hdfCrDrStatus.Value) == 0)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Drafted").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                                if (Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfDelStatusCrDr")).Value))
                                {
                                    btnSaveDCSplit.Visible = false;
                                    IsDeleted = true;
                                    hdfIsCancelled.Value = "1";
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_alreadycancelled").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                                else
                                {
                                    btnSaveDCSplit.Visible = true;
                                    btnEdit.Visible = true;
                                    IsDeleted = false;
                                    hdfIsCancelled.Value = "0";
                                }
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            if (BusinessLogic.POInvoicing.DebitCreditTradingBL.ValidationForDRCRCancellation(CurrPK))//SP:SPFIN_CRDR_NOTE_VND_CAN_CHECK
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
                            GetFieldValues(ControlsEnum.INVCRDRHEADER);
                            SetFieldValues(ControlsEnum.INVCRDRHEADER);
                            SetFieldValues(ControlsEnum.INVCRDRDETAIL);
                            TempInvCrDrHeaderSession = InvCrDrHeaderSession;

                            uclPendingInvPaging.CurrentPage = 1;
                            PageIndexInv = CommonConstants.SELECT_VALUE_ONE;
                            GetFieldValues(ControlsEnum.PENDINGINVLIST);
                            SetFieldValues(ControlsEnum.PENDINGINVLIST);
                            hdfIsPendingInvVisible.Value = "0";
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
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #endregion

                    #region JOURNALIZE (Save,Update,Submit,Delete,Cancel))
                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
                        GetFieldValues(ControlsEnum.INVCRDRHEADER);
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
                        ResetForm();
                        GetFieldValues(ControlsEnum.DRCRHDRLIST);
                        SetFieldValues(ControlsEnum.DRCRHDRLIST);
                        break;
                    #endregion
                    #region Journalize Save
                    case ActionsEnum.JOURNALIZESAVE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ((Button)sender).CommandName = ActionsEnum.SAVE.ToString();
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.DRCRHDRLIST);
                        SetFieldValues(ControlsEnum.DRCRHDRLIST);
                        break;
                    #endregion
                    #region Journalize Submit
                    case ActionsEnum.JOURNALIZESUBMIT:
                        ResetAfterJournalize();
                        break;
                    #endregion
                    #region Journalize Delete
                    case ActionsEnum.JOURNALIZEDELETE:
                        ResetAfterJournalize();
                        break;
                    #endregion
                    #region Journalize Cancel
                    case ActionsEnum.JOURNALIZECANCEL:
                        ResetAfterJournalize();
                        break;
                    #endregion
                    #endregion
                    #region CREDITDEBITLIST
                    case ActionsEnum.CREDITDEBITLIST:
                        FillProcessID(1);
                        CurrPK = 0;
                        ResetForm();
                        GetFieldValues(ControlsEnum.DRCRHDRLIST);
                        SetFieldValues(ControlsEnum.DRCRHDRLIST);
                        uclPaging.TotalPages = TotalPages;
                        uclPaging.CurrentPage = 1;
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;                    
                    #endregion   
                    #region CRDR Split
                    case ActionsEnum.DCDETAIL:
                        divErrorLabel.Visible = false;
                        HiddenField hdfReceiptMpgPK = (HiddenField)((((Button)sender).Parent).FindControl("hdfCrDbMpgPK"));
                        TextBox txtReceivedNow = (TextBox)((((Button)sender).Parent).FindControl("txtNoteFor"));                      
                        LinkButton lbnTotalTax = (LinkButton)((((Button)sender).Parent).FindControl("lbnTotalTax"));
                        HiddenField hdfTotalTax = (HiddenField)((((Button)sender).Parent).FindControl("hdfTotalTax"));
                        InvRowIndex = ((GridViewRow)((Button)(sender)).Parent.Parent).RowIndex;
                        HiddenField hdfInvoicePK = (HiddenField)((((Button)sender).Parent).FindControl("hdfInvoicePK"));
                        if (txtReceivedNow != null && !string.IsNullOrEmpty(txtReceivedNow.Text.Trim()))
                        {
                            PayNowAmount = Convert.ToDecimal(txtReceivedNow.Text.Trim());
                            paynowtax = Convert.ToDecimal(hdfTotalTax.Value.Trim());
                        }
                        
                        if (hdfInvoicePK != null && !string.IsNullOrEmpty(hdfInvoicePK.Value) && !hdfInvoicePK.Value.Equals("0"))
                        {
                            InvoicePK = Convert.ToInt64(hdfInvoicePK.Value);
                        }
                        else
                            InvoicePK = 0;

                        tempInvoiceSOSplitList = CrDrInvMappingDetails;
                        tempfinCrDrCusMpgList = tempInvoiceSOSplitList.Where(dtl => dtl.CDM_INVOICE_VND_HDR == InvoicePK).ToList();
                        
                        if (grdInvoiceList.Rows.Count > 0)
                        {
                            GetUIValuesFromObject(ControlsEnum.CRDRSPLITLIST);//Header
                            isSplitChanged = false;                          
                            SetFieldValues(ControlsEnum.CRDRSPLITLIST);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCrDrSplitUp]','" + GetLocalResourceObject("ReceiptSplit").ToString() + "','950','300');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoItems").ToString()) + "');", true);
                        }
                        
                        break;
                    #endregion
                    #region DCSPLITSAVE
                    case ActionsEnum.DCSPLITSAVE:
                        decimal TotalPayNow = 0;
                        decimal TotalTax = 0;
                        HiddenField lblTotalPayNow = (HiddenField)grdDCSplit.FooterRow.FindControl("hdfTotalPayNowFooterSplit");
                        HiddenField hdfTotalTaxFooterSplit = (HiddenField)grdDCSplit.FooterRow.FindControl("hdfTotalTaxFooterSplit");
                        TotalPayNow = Convert.ToDecimal(lblTotalPayNow.Value);
                        decimal.TryParse(hdfTotalTaxFooterSplit.Value, out TotalTax);
                        if (InvRowIndex >= 0)
                        {
                            TextBox txtTotalPayNow = (TextBox)grdInvoiceList.Rows[InvRowIndex].FindControl("txtNoteFor");
                            HiddenField hdfItemIncluded = (HiddenField)grdInvoiceList.Rows[InvRowIndex].FindControl("hdfItemIncluded");
                            HiddenField hdfIsDetailTaxExist = (HiddenField)grdInvoiceList.Rows[InvRowIndex].FindControl("hdfIsDetailTaxExist");
                            lbnTotalTax = (LinkButton)grdInvoiceList.Rows[InvRowIndex].FindControl("lbnTotalTax");
                            hdfTotalTax = (HiddenField)grdInvoiceList.Rows[InvRowIndex].FindControl("hdfTotalTax");
                            txtTotalPayNow.Text = Math.Round(TotalPayNow, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            if (hdfIsDetailTaxExist.Value == "1")
                            {
                                lbnTotalTax.Text = GetFormattedCurrencyWithSeperator(TotalTax);
                                hdfTotalTax.Value = hdfTotalTaxFooterSplit.Value;
                            }

                            finCrDrNoteDtlList = new List<DebitCreditNoteDetails>();
                            finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                            finCrDrHdrNoteServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                            finCrDrNoteDtlList = (List<DebitCreditNoteDetails>)SetUIValuesToObject(ControlsEnum.CRDRSPLITLIST);
                            if (finCrDrNoteDtlList != null && finCrDrNoteDtlList.Count() > 0 && finCrDrNoteDtlList.Sum(dtl => dtl.CDS_AMOUNT) > 0)
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

               
                    #region SHOWPOPUP
                    case ActionsEnum.SHOWPOPUP:
                        if (((LinkButton)sender).CommandArgument.ToString() != null)
                        {
                            GridViewRow grdRow = (GridViewRow)((LinkButton)(sender)).Parent.Parent;
                            HiddenField hdfInvoiceGrp = (HiddenField)grdRow.FindControl("hdfInvoiceGrp");
                            int Pk = Convert.ToInt32(((HiddenField)grdRow.FindControl("hdfInvoicePK")).Value);
                            if (Convert.ToInt32(hdfInvoiceGrp.Value) == (int)POInvoiceGroup.Expense)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + Pk.ToString() + "&APPTYPE=" + ApplicationType.EIT + "&APPSUBTYPE=") + "');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ((LinkButton)sender).CommandArgument.ToString() + "&APPTYPE=" + ApplicationType.TPI + "&APPSUBTYPE=1") + "');", true);
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
                        CurrPK = Convert.ToInt32(hdfCrDrPk.Value);
                        GetFieldValues(ControlsEnum.BALANCEAMOUNTSPLIT);
                        SetFieldValues(ControlsEnum.BALANCEAMOUNTSPLIT);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalAmountSplit", "$(document).ready(function(){CalculateTotalAmountSplit();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divBalAmntSplitup]','" + GetLocalResourceObject("TrxDetails").ToString() + "','600','300');", true);
                        break;
                    #endregion;

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
                                                POUploadList = new List<BusinessObject.POInvoicing.DebitCreditNoteUploads>();
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

                                            poUploadObj = new DebitCreditNoteUploads();
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
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);
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
                                BindGrid(ControlsEnum.UPLOADEDFILES);
                                ResetForm(ControlsEnum.ADDITEM);
                            }
                        }
                         ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);
                        break;
                    #endregion
                    #region EDITITEMUPLOAD
                    case ActionsEnum.EDITITEMUPLOAD:
                        if (POUploadList != null && POUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                poUploadObj = POUploadList.SingleOrDefault(row => selectedItemPK == row.DOC_SEQ_NO);
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
                        HiddenField hdfinvGroup = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfListGroup"));
                        if (Convert.ToInt32(hdfinvGroup.Value) == (int)POInvoiceGroup.Expense)//Group 3(Expense Inv)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfinvPK.Value.ToString() + "&APPTYPE=" + ApplicationType.EIT + "&APPSUBTYPE=") + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfinvPK.Value.ToString() + "&APPTYPE=" + ApplicationType.TPI + "&APPSUBTYPE=1") + "');", true);
                        }

                        break;
                    #endregion                  
                    #region AMOUNTDETAILS
                    case ActionsEnum.AMOUNTDETAILS:
                        HiddenField hdfInvoiceID = (HiddenField)((GridViewRow)((LinkButton)(sender)).Parent.Parent).FindControl("hdfInvoiceID");
                        long.TryParse(hdfInvoiceID.Value, out poInvoicePk);
                        GetFieldValues(ControlsEnum.AMOUNTDETAILS);
                        SetFieldValues(ControlsEnum.AMOUNTDETAILS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalAmountSplit", "$(document).ready(function(){CalculateTotalAmountSplit();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPaidAmntSplitup]','" + GetLocalResourceObject("TrxDetails").ToString() + "','600','250');", true);
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
                                HiddenField hdfInvoiceTypePrint = (HiddenField)grdrow.FindControl("hdfInvoiceTypeCrDr");
                                // check row selected or not
                                if (rbtn.Checked)
                                {
                                    // get pk from the grid and assign to CurrPk
                                    CurrPK = Convert.ToInt32(grdCrDbHdr.DataKeys[grdrow.RowIndex].Values[0]);
                                    hdfCrDrNumber.Value = CurrPK.ToString();
                                    Reftype = hdfCreditDebitType.Value == "1" ? ApplicationType.DNT : ApplicationType.CNT;
                                    InvType = Convert.ToInt32(hdfInvoiceTypePrint.Value);
                                    bIsChecked = true;
                                    break;
                                }

                            }
                        }
                        else
                        {
                            Reftype = ddlMode.SelectedValue == "1" ? ApplicationType.DNT : ApplicationType.CNT;
                            bIsChecked = true;
                        }
                        if (bIsChecked)
                        {                          
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
                    #region RESET (Reset Vendor Selection)
                    case ActionsEnum.RESET:
                        ActionHandler(btnNew, EventArgs.Empty);
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
        private void SetUILineItemTaxView(GridViewRow grdRowLineItem)
        {
            
        }
        private void SetUIHeaderTaxView(GridViewRow grdInvRow)
        {
          
            List<SOInvoiceTaxHdr> taxListTemp = new List<SOInvoiceTaxHdr>();
            decimal taxpercentage = 0;
            decimal basevalue = 0;
            decimal ttaxamt = 0;
            decimal hdftax = 0;
            decimal hdftotalamt = 0;
            int ItemIncluded = 0;
            finCrDrNoteMpgObj = new DebitCreditNoteInvoiceMappingDetails();
            HiddenField hdfCrDbMpgPK = (HiddenField)grdInvRow.FindControl(GetLocalResourceObject("hdfCrDbMpgPK").ToString());
            HiddenField hdfItemIncluded = (HiddenField)grdInvRow.FindControl("hdfItemIncluded");
            finCrDrNoteMpgObj.CDM_PK = hdfCrDbMpgPK == null ? 0 : Convert.ToInt64(hdfCrDbMpgPK.Value);
            finCrDrNoteMpgObj.CDM_CRDR_NOTE_HDR = CurrPK.ToString();
            HiddenField hdfInvoicePK = (HiddenField)grdInvRow.FindControl(GetLocalResourceObject("hdfInvoicePK").ToString());
            finCrDrNoteMpgObj.CDM_INVOICE_VND_HDR = hdfInvoicePK == null ? 0 : Convert.ToInt32(hdfInvoicePK.Value);         
            TextBox txtAmount = (TextBox)grdInvRow.FindControl(GetLocalResourceObject("txtNoteFor").ToString());
            finCrDrNoteMpgObj.CDM_AMOUNT = Convert.ToDecimal(txtAmount.Text);
            finCrDrNoteMpgObj.CDM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);

            HiddenField hdfTaxAmt = (HiddenField)grdInvRow.FindControl("hdfTaxAmt");
            hdftax = hdfTaxAmt.Value != string.Empty ? Convert.ToDecimal(hdfTaxAmt.Value) : 0;
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
                            decimal InvAmount = objInvoiceDetails[0].IVH_AMOUNT_TC;
                            decimal TotalTaxPercentage = 1;
                            List<FIN_INVOICE_VND_TAX_HDR> objInvoiceTaxList = objInvoiceDetails[0].FIN_INVOICE_VND_TAX_HDR.Where(inv => inv.VTH_TAX_CATEGORY == (byte)TaxType.Tax).ToList();
                            if (objInvoiceTaxList != null && objInvoiceTaxList.Count > 0)
                            {
                                TotalTaxPercentage = objInvoiceTaxList.Sum(tx => (tx.VTH_TAX_AMT * 100) / InvAmount);
                                foreach (FIN_INVOICE_VND_TAX_HDR invtaxhdr in objInvoiceTaxList)
                                {
                                    SOInvoiceTaxHdr ObjTaxDtl = new SOInvoiceTaxHdr();
                                    decimal IndividualPercentage = (invtaxhdr.VTH_TAX_AMT * 100) / InvAmount;
                                    decimal TaxAmnt = (finCrDrNoteMpgObj.CDM_TAX_AMOUNT * IndividualPercentage) / TotalTaxPercentage;
                                    double Amount = Math.Round(Convert.ToDouble(TaxAmnt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
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

        private void DcsSplitSave()
        {
            if (!IsValid)
            {
                litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
            }
            else
            {
                if (grdDCSplit.Rows.Count >= 1)
                {
                    divErrorLabel.Visible = false;
                    finCrDrNoteDtlList = new List<DebitCreditNoteDetails>();                 
                    finCrDrNoteDtlList = (List<DebitCreditNoteDetails>)SetUIValuesToObject(ControlsEnum.CRDRSPLITLIST);
                    if (finCrDrNoteDtlList != null)
                    {
                        if (finCrDrNoteDtlList.Where(r => r.CDS_IS_AFFECT_STK == 1).Count() > 0)
                            chkAffectStock.Checked = true;
                        else
                            chkAffectStock.Checked = false;
                     
                        CrDrInvMappingDetails.SingleOrDefault(dtl => dtl.CDM_INVOICE_VND_HDR == InvoicePK).ItemDetail = finCrDrNoteDtlList.ToList();            
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                    }
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
            #region GridView Controls Declaration
             decimal taxpercentage;
             decimal basevalue;
             decimal taxamt;
             decimal cdsVidAmount;
             decimal cdsVidTax;
             int InvGroup = 0;
             DebitCreditNoteDetails tempFinReceiptCusSoMpgObj = null;
             List<DebitCreditNoteDetails> tempFinReceiptCusSoMpgList = null;
            #endregion
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    #region grdPendingInvList
                    if (((GridView)sender).ID == "grdPendingInvList")
                    {
                        HiddenField hdfInvoiceID = e.Row.FindControl("hdfInvoiceID") as HiddenField;
                        CheckBox chkInvPendSelect = e.Row.FindControl("chkInvPendSelect") as CheckBox;
                        if (InvCrDrHeaderSession != null && InvCrDrHeaderSession.InvoiceDetail.Where(r => r.CDM_INVOICE_VND_HDR == Convert.ToInt16(hdfInvoiceID.Value)).Count() > 0)
                        {
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("ErpRes", "selectedRowColor").ToString());
                            chkInvPendSelect.Checked = true;
                            chkInvPendSelect.Enabled = false;
                        }

                    }
                    #endregion
                    else if (((GridView)sender).ID == "grdInvoiceList")
                    {
                        #region grdInvoiceList
                        HiddenField hdfTaxPercentage = e.Row.FindControl("hdfTaxPercentage") as HiddenField;
                        TextBox txtNoteFor = e.Row.FindControl("txtNoteFor") as TextBox;
                        LinkButton lbnTotalTax = e.Row.FindControl("lbnTotalTax") as LinkButton;
                        HiddenField hdfTotalTax = e.Row.FindControl("hdfTotalTax") as HiddenField;
                        HiddenField hdfIsDetailTaxExist = e.Row.FindControl("hdfIsDetailTaxExist") as HiddenField;
                        HiddenField hdfInvoiceGrp = e.Row.FindControl("hdfInvoiceGrp") as HiddenField;
                        HiddenField hdfInvCategory = e.Row.FindControl("hdfInvCategory") as HiddenField;
                        HiddenField hdfItemIncluded = e.Row.FindControl("hdfItemIncluded") as HiddenField;


                        if (drcrInvoiceMappingDetailList[e.Row.RowIndex].ItemDetail != null && drcrInvoiceMappingDetailList[e.Row.RowIndex].ItemDetail.Sum(dtl => dtl.CDS_AMOUNT) > 0)
                        {
                            hdfItemIncluded.Value = "1";
                            hdfSplitCount.Value = "1";

                            txtNoteFor.Text = txtNoteFor.ToolTip = Math.Round(drcrInvoiceMappingDetailList[e.Row.RowIndex].CDM_AMOUNT - drcrInvoiceMappingDetailList[e.Row.RowIndex].CDM_TAX_AMOUNT,
                              Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        }
                        else
                        {
                            hdfItemIncluded.Value = "0";
                            hdfSplitCount.Value = "0";
                            txtNoteFor.ToolTip = txtNoteFor.Text = String.Format("{0:c}", Convert.ToDecimal(0));
                        }

                        if (CurrPK > 0)
                        {
                            lbnTotalTax.Text = lbnTotalTax.ToolTip = GetFormattedCurrencyWithSeperator(Math.Round(drcrInvoiceMappingDetailList[e.Row.RowIndex].CDM_TAX_AMOUNT,
                                 Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                            hdfTotalTax.Value = Math.Round(drcrInvoiceMappingDetailList[e.Row.RowIndex].CDM_TAX_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        }
                        else
                        {
                            taxpercentage = string.IsNullOrEmpty(hdfTaxPercentage.Value) ? 0 : Convert.ToDecimal(hdfTaxPercentage.Value);
                            basevalue = (txtNoteFor.Text != string.Empty ? Convert.ToDecimal(txtNoteFor.Text) : 0) / (1 + taxpercentage);
                            taxamt = ((txtNoteFor.Text != string.Empty ? Convert.ToDecimal(txtNoteFor.Text) : 0) - basevalue);
                            lbnTotalTax.Text = GetFormattedCurrencyWithSeperator(Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                            hdfTotalTax.Value = Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        }
                        if (hdfIsDetailTaxExist.Value == "1")
                        {
                            lbnTotalTax.Enabled = false;
                            lbnTotalTax.Attributes.Add("class", "nomargin");
                        }
                        else
                        {
                            lbnTotalTax.Enabled = true;
                            lbnTotalTax.Attributes.Add("class", "text-underline nomargin");
                        }

                        hdfInvoiceCategory.Value = hdfInvCategory.Value.ToString();
                        #endregion
                    }
                    else if (((GridView)sender).ID == "grdCrDbHdr")
                    {
                        #region grdCrDbHdr
                        Button imgAffectStock = e.Row.FindControl("imgAffectStock") as Button;

                        if (imgAffectStock != null)
                        {
                            imgAffectStock.CssClass = GetLocalResourceObject("StockCss").ToString();
                            if (dtInvoiceList != null && dtInvoiceList.Rows.Count > 0)
                            {
                                DebitCreditTradingBO ObjfinCrdrHdr = new DebitCreditTradingBO();
                                ObjfinCrdrHdr.CDH_PK = Convert.ToInt16(dtInvoiceList.Rows[e.Row.RowIndex]["CDH_PK"]);
                                ObjfinCrdrHdr.CDH_TYPE = Convert.ToByte(dtInvoiceList.Rows[e.Row.RowIndex]["CDH_TYPE"]);
                                ObjfinCrdrHdr.CDH_IS_AFFECT_STK = Convert.ToByte(dtInvoiceList.Rows[e.Row.RowIndex]["CDH_IS_AFFECT_STK"]);
                                ObjfinCrdrHdr.CDH_STATUS = Convert.ToByte(dtInvoiceList.Rows[e.Row.RowIndex]["CDH_STATUS"]);
                                ObjfinCrdrHdr.CDH_IS_DELETED = Convert.ToBoolean(dtInvoiceList.Rows[e.Row.RowIndex]["CDH_IS_DELETED"]);

                                if (dtInvoiceList.Rows[e.Row.RowIndex]["CDH_IS_EMR_EXISTS"].ToString() == "1")
                                    imgAffectStock.CssClass = GetLocalResourceObject("StockUpdationCss").ToString();
                                imgAffectStock.Visible = SetStockVisibility(ObjfinCrdrHdr);
                            }

                        }
                        #endregion
                    }
                    else if (((GridView)sender).ID == "grdDCSplit")
                    {
                        #region grdDCSplit
                        #region Declaration
                        TextBox txtQtySplit = e.Row.FindControl("txtQtySplit") as TextBox;
                        TextBox txtRateSplit = e.Row.FindControl("txtRateSplit") as TextBox;
                        TextBox txtSumSplit = e.Row.FindControl("txtSumSplit") as TextBox;
                        HiddenField hdfSumSplit = e.Row.FindControl("hdfSumSplit") as HiddenField;
                        HiddenField hdfInvCusDtlPK = e.Row.FindControl("hdfInvCusDtlPK") as HiddenField;
                        TextBox txtTAXSplitTotal = e.Row.FindControl("txtTAXSplitTotal") as TextBox;
                        HiddenField hdfTAXSplit = e.Row.FindControl("hdfTAXSplit") as HiddenField;
                        HiddenField hdfTAXSplitTotal = e.Row.FindControl("hdfTAXSplitTotal") as HiddenField;
                        CheckBox chkAffectStkSplit = e.Row.FindControl("chkAffectStkSplit") as CheckBox;
                        HiddenField hdfCDSTaxPercentage = e.Row.FindControl("hdfCDSTaxPercentage") as HiddenField;
                        HiddenField hdfNETSplit = e.Row.FindControl("hdfNETSplit") as HiddenField;
                        int.TryParse(hdfInvGroup.Value, out InvGroup);
                        #endregion

                        grdDCSplit.Columns[5].Visible = grdDCSplit.Columns[6].Visible = grdDCSplit.Columns[11].Visible = IsItemwiseTaxForTradingPurchase;

                        if (tempfinCrDrCusMpgList != null)
                        {
                            tempFinReceiptCusSoMpgList = tempfinCrDrCusMpgList[0].ItemDetail.ToList();
                            tempFinReceiptCusSoMpgObj = tempFinReceiptCusSoMpgList.SingleOrDefault(mpg => mpg.CDS_INVOICE_VND_DTL == Convert.ToInt64(hdfInvCusDtlPK.Value));
                        }

                        chkAffectStkSplit.Checked = tempFinReceiptCusSoMpgObj == null ? false : (tempFinReceiptCusSoMpgObj.CDS_IS_AFFECT_STK == 1 ? true : false);
                        chkAffectStkSplit.Enabled = (InvGroup == (int)POInvoiceGroup.Expense) ? false : true;

                        double qty = 0;
                        double rate = 0;
                        Double.TryParse(txtQtySplit.Text, out qty);
                        Double.TryParse(txtRateSplit.Text, out rate);

                        if (tempfinCrDrCusMpgList != null)
                        {
                            txtSumSplit.Text = tempFinReceiptCusSoMpgObj == null ? GetFormattedCurrency(0) : GetFormattedCurrency(tempFinReceiptCusSoMpgObj.CDS_AMOUNT);
                            hdfSumSplit.Value = tempFinReceiptCusSoMpgObj == null ? GetFormattedCurrency(0) : GetFormattedCurrency(tempFinReceiptCusSoMpgObj.CDS_AMOUNT);
                            txtSumSplit.ToolTip = tempFinReceiptCusSoMpgObj == null ? GetFormattedCurrency(0) : GetFormattedCurrency(tempFinReceiptCusSoMpgObj.CDS_AMOUNT);
                        }
                        else
                        {
                            txtSumSplit.Text = Math.Round((qty * rate), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            txtSumSplit.ToolTip = Math.Round((qty * rate), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            hdfSumSplit.Value = Math.Round((qty * rate), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        }
                       // taxpercentage = Convert.ToDecimal(hdfCDSTaxPercentage.Value);
                        cdsVidAmount = string.IsNullOrEmpty(hdfNETSplit.Value) ? 0 : Convert.ToDecimal(hdfNETSplit.Value);//CDS_VID_AMOUNT
                        cdsVidTax = string.IsNullOrEmpty(hdfTAXSplit.Value) ? 0 : Convert.ToDecimal(hdfTAXSplit.Value);//CDS_VID_TAX
                        taxpercentage = Convert.ToDecimal(cdsVidTax) / (cdsVidAmount == 0 ? 1 : cdsVidAmount); 
                        taxamt = ((txtSumSplit.Text != string.Empty ? Convert.ToDecimal(txtSumSplit.Text) : 0) * taxpercentage);

                        txtTAXSplitTotal.Text = txtTAXSplitTotal.ToolTip = Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        grndTotalTaxSplit += Convert.ToDecimal(txtTAXSplitTotal.Text);
                        hdfTAXSplitTotal.Value = Math.Round(taxamt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        if (qty == 0)
                        {
                            chkAffectStkSplit.Enabled = false;
                        }
                        if (InvGroup == (int)POInvoiceGroup.Expense)
                        {
                            ((CheckBox)grdDCSplit.HeaderRow.FindControl("chkAffectStkHdr")).Enabled = false;
                            grdDCSplit.Columns[5].Visible = grdDCSplit.Columns[6].Visible = grdDCSplit.Columns[11].Visible = true;
                        }
                        #endregion
                    }
                    else if (((GridView)sender).ID == "grdUploads")
                    {
                        #region grdUploads
                        int slno;
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
                }
                #region Footer
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    if (((GridView)sender).ID == "grdInvoiceList")
                    {
                       
                    }
                    if (((GridView)sender).ID == "grdDCSplit")
                    {
                        Label lblTotalTaxFooterSplit;
                        HiddenField hdfTotalTaxFooterSplit;
                        Label lblTotalPayNowFooterSplit;
                        HiddenField hdfTotalPayNowFooterSplit;

                        lblTotalPayNowFooterSplit = e.Row.FindControl("lblTotalPayNowFooterSplit") as Label;
                        hdfTotalPayNowFooterSplit = e.Row.FindControl("hdfTotalPayNowFooterSplit") as HiddenField;
                        lblTotalTaxFooterSplit = e.Row.FindControl("lblTotalTaxFooterSplit") as Label;
                        hdfTotalTaxFooterSplit = e.Row.FindControl("hdfTotalTaxFooterSplit") as HiddenField;

                        lblTotalTaxFooterSplit.Text = GetFormattedCurrency(grndTotalTaxSplit);
                        hdfTotalTaxFooterSplit.Value = grndTotalTaxSplit.ToString();
                    }
                }
                #endregion
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
        private bool SetStockVisibility(DebitCreditTradingBO ObjfinCrdrHdr)
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

        /// <summary>
        /// Save and submit With workflow 
        /// </summary>
        /// <param name="objSalPayment"></param>
        private void SaveTransaction(DebitCreditTradingBO objPOInvoice, int workflowFlag)
        {
            int? result = 0;
            string savePath = string.Empty;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (objPOInvoice == null)
                objPOInvoice = new DebitCreditTradingBO();

            #region ATL_APP_TYPE
            if (ddlMode.SelectedValue == "1")//DEBIT
            {         
                objPOInvoice.ATL_APP_TYPE = ApplicationType.DNT;
            }
            else if (ddlMode.SelectedValue == "2")
            {              
                objPOInvoice.ATL_APP_TYPE = ApplicationType.CNT;
            }           
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
            string xmlDoc = CommonFunctions.XmlSerialize<DebitCreditTradingBO>(objPOInvoice);
            string invoiceNumber = string.Empty;
            result = BusinessLogic.POInvoicing.DebitCreditTradingBL.SaveCreditDebitTradingWkf(xmlDoc, out invoiceNumber);//SPFIN_CRDR_VND_WKF_SAVE
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

                    foreach (DebitCreditNoteUploads obj in POUploadList)
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
                    if (string.IsNullOrEmpty(invoiceNumber))
                        invoiceNumber = lblDrCrNo.Text.Trim();
                    object[] args = new object[2];
                    args[0] = Resources.PageNameRes.CreditDebitNotesTrading;
                    args[1] = invoiceNumber;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                    // Show Save Message and redired to listing page   
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CreditDebitNotesTrading);
                    #region Inbox or Listing Page Redirection
                    if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                    {
                        ResetForm();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                    }
                    else
                    {
                        ResetForm();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.DRCRHDRLIST);
                        SetFieldValues(ControlsEnum.DRCRHDRLIST);
                    }
                    #endregion
                }
                ucrWrkf.ApplicationID = result.Value;
            }
            else if (result == -25)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_RefnoExist").ToString()) + "','" + Resources.Messages.Information + "');", true);
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
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CreditDebitNotesTrading);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup(); ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                return;
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
            uclPendingInvPaging.CurrentPage = 1;
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

            lnkList.PreRender += new EventHandler(btnAction_PreRender);
            lnkDetail.PreRender += new EventHandler(btnAction_PreRender);
            btnNew.PreRender += new EventHandler(btnAction_PreRender);

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
         
            lnkList.Load += new EventHandler(btnAction_Load);
            lnkDetail.Load += new EventHandler(btnAction_Load);
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

            this.uclPendingInvPaging.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPendingInvPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPendingInvPaging.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPendingInvPaging.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPendingInvPaging.PageChanged += new ActionHandler(this.ActionHandler);
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
                        // increment the current page index.
                        if (e.CurrentPage <= e.TotalPages)
                            pagerControl.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        // Decrement the current page index.
                        if (e.CurrentPage > 1)
                            pagerControl.CurrentPage--;
                        break;


                }               
                if (senderId == "uclPaging")
                {
                    PageIndex = uclPaging.CurrentPage.ToString();
                    GetFieldValues(ControlsEnum.DRCRHDRLIST);
                    SetFieldValues(ControlsEnum.DRCRHDRLIST);
                    EntryStatus = EntryStatus.LISTMODE;
                    EnableDisableButtons(e.TotalPages, "uclPaging");
                }
                else if (senderId == "uclPendingInvPaging")
                {
                    PageIndexInv = uclPendingInvPaging.CurrentPage.ToString();
                    GetFieldValues(ControlsEnum.PENDINGINVLIST);
                    SetFieldValues(ControlsEnum.PENDINGINVLIST);
                    EnableDisableButtons(e.TotalPages, "uclPendingInvPaging");
                }     
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
            else if (pagerId == "uclPendingInvPaging")
            {
                // Should we disable the first link
                uclPendingInvPaging.FirstButtonEnabled = (uclPendingInvPaging.CurrentPage == 1) ? false : true;
                // Should we disable the previous link
                uclPendingInvPaging.PreviousButtonEnabled = (uclPendingInvPaging.CurrentPage == 1) ? false : true;
                // Should we enable the next link
                uclPendingInvPaging.NextButtonEnabled = (uclPendingInvPaging.CurrentPage < iTotalPages) ? true : false;
                // Should we enable the last link
                uclPendingInvPaging.LastButtonEnabled = (uclPendingInvPaging.CurrentPage < iTotalPages) ? true : false;
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculateTotalSplit();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails();});", true);
                if (grdInvoiceList.Rows.Count > 0)
                {                  
                    txtVendorHd.Enabled = false;
                    if (CurrPK == 0)
                    {
                        btnResetVendorSelection.Visible = true;
                    }
                }
                else
                {                   
                    txtVendorHd.Enabled = true;
                    btnResetVendorSelection.Visible = false;
                }      
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
                if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
                {
                    DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                    if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
                    {
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
                    }
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
            INVOICEDETAILS,
            TAXMYR,
            TAXSPLITUP,
            INVOICEITEMDETAILS,
            TAXSPLITUPLINEITEMWISE,
            INVITEMLIST,
            LINEITEMTAXSETTINGS,
            FINHEADERSTATUS,
            ALLOCATEDINPAYMENT,
            BALANCEAMOUNTSPLIT,
            ADDITEM,
            SELECTEDDOC,
            GETCRDRHDRDETAILS,
            PODEPT,
            DRCRGET,
            PENDINGINVLIST,
            AMOUNTDETAILS,
            INVCRDRHEADER,
            UPLOADEDFILES,
            INVCRDRDETAIL,
            DRCRCANCELCHECK,
            INVOICETYPE,
            INVOICECATEGORY,
            VENDORDETAILSBYPK
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
        }
        #endregion
    }
}