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
using BusinessObject.Finance;
using System.IO;
using BusinessObject.SaleOrder;


using System.Xml;

using System.Web.UI.HtmlControls;


namespace ERPSMS_v01.Sales
{
    public partial class AgtComm : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties

        /// <summary>
        /// To maintain keep Agt Comm Invoice Header  
        /// </summary>
        private AgentCommInvHeader AgtCommHeaderSession
        {
            get
            {
                return (AgentCommInvHeader)Session[ERP.Utilities.SessionStrings.AgtCommHeaderSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.AgtCommHeaderSession] = value;
            }
        }
        /// <summary>
        /// To maintain keep selected pos from SC tab
        /// </summary>
        private List<long> SelectedSosForInvComm
        {
            get
            {
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedSosForInvComm];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedSosForInvComm] = value;
            }

        }
        /// <summary>
        /// To maintain keep selected pos 
        /// </summary>
        private List<long> SelectedSOListForInvComm
        {
            get
            {
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedSOListForInvComm];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedSOListForInvComm] = value;
            }

        }


        //---------------------------------------------------------

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
        /// WorkFlow RefID
        /// </summary>
        public int WkfRefID
        {
            get
            {
                return (this.ViewState["BaseWkfRefID"] == null ? 0 : (int)this.ViewState["BaseWkfRefID"]);
            }
            set
            {
                this.ViewState["BaseWkfRefID"] = value;
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
        /// Current PK
        /// </summary>
        private int CurrPK
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrPK] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrPK];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrPK] = value;
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
        private string fromType
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.fromType];
            }
            set
            {
                this.ViewState[ViewstateStrings.fromType] = value;
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
        /// Process ID of the Page
        /// </summary>
        private int WkfStatus
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
        private string AGTCommV
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.AGTCommV];
            }
            set
            {
                this.ViewState[ViewstateStrings.AGTCommV] = value;
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

        private AgentCommInvHeader AgtCommHeaderObj;
        private AgentCommInvHeader AgtCommHeaderTemp;
        List<AgentCommInvDetails> AgentCommInvDetailsList;

        private AgtCommGetKVBO AgtCommGetKVBOObj;
        List<AgtCommGetKVBO> AgtCommGetKVList;

        //private List<long> SelectedSOListForInvComm;

        private AgtCommBO invoiceHeaderObjComm;
        private AgentCommInvSaveDetails invoiceAgtDtlObjComm;
        //   ----------------------------------------------


        // Indicates the state as well as action
        private ActionsEnum commonActions;
        User currentUser;
        //page related Entity Object

        private POInvoiceDetails soInvoiceDetailsObj;
        private POInvoiceTaxHdr soInvTaxHdrObj;
        string Exchngrate;
        AdmCompanyMstService admCompanyMstServiceClient;

        TextBox WrkfComments;
        DropDownList ddlWkfAction;
        string action;

        DataTable dtExchngrate;
        DataTable dtCurrency;
        List<POAdvDeductionDetails> deductionDtlList;
        POInvoiceUploads poUploadObj;
        List<POInvoiceTaxHdr> taxHdrList;
        POInvoiceDetails soDtlObj;
        string selectedVendor;
        DataSet dsInvHeader;
        DataTable dtTaxDetails;
        bool hasValidRate;
        DataTable dtTaxSettings;
        int JournalPK;

        DataSet dsPageData;
        private DataTable dtInvoiceList;
        private DataTable dtPageData;

        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;

        private string refID;
        private string inboxFlag;

        private string fromPK;

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
        private string TypeRef;
        private string appType;
        private int voucherPK;

        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;

        private FIN_INVOICE_VND_HDR finInvoiceVndHdrObj;
        private int vndPK;

        private string[] _operators = { "-", "+", "/", "*", "^" };
        private Func<double, double, double>[] _operations = {
        (a1, a2) => a1 - a2,
        (a1, a2) => a1 + a2,
        (a1, a2) => a1 / a2,
        (a1, a2) => a1 * a2,
        (a1, a2) => Math.Pow(a1, a2)
    };

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
                    GetFieldValues(ControlsEnum.AGTLIST);
                    SetFieldValues(ControlsEnum.AGTLIST);


                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);


                    txtFromDate.Text = string.Empty;
                    hdfFromDate.Value = string.Empty;
                    txtToDate.Text = string.Empty;
                    txtVno.Text = string.Empty;
                    hdfToDate.Value = string.Empty;
                    hdfNumberDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
                    hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfDecimalFormatWithSeperator.Value = "#" + currencysep + "#0.";
                    hdfCurrencyFormatWithSeperator.Value = "#" + currencysep + "#0.";
                    hdfJournalizeWorkFlow.Value = "0";
                    hdfDecimalFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                        hdfDecimalFormatWithSeperator.Value += "0";
                    }
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                        hdfCurrencyFormatWithSeperator.Value += "0";
                    }
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

                    hdfAstDocMode.Value = "0";

                    FillProcessID(1);
                    string pid = Request.QueryString[QueryStrings.PID] != null ? Request.QueryString[QueryStrings.PID]
                        : Session[ERP.Utilities.SessionStrings.PID] != null ? Session[ERP.Utilities.SessionStrings.PID].ToString().Split('=')[1] : string.Empty;
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                        : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;
                    fromType = Request.QueryString[QueryStrings.Type] != null ? Request.QueryString[QueryStrings.Type]
                   : string.Empty;
                    fromPK = Request.QueryString[QueryStrings.fromPK] != null ? Request.QueryString[QueryStrings.fromPK]
                 : string.Empty;

                    ReferanceID = string.IsNullOrEmpty(refID)
                                  ? string.IsNullOrEmpty(prefID)
                                      ? 0
                                      : int.Parse(prefID)
                                  : int.Parse(refID);

                    //If Request From External(Report or Other page) other than Menu or Inbox
                    if (Request.QueryString[QueryStrings.FromExt] != null && Request.QueryString[QueryStrings.FromExt] == "T")
                    {
                        ucrWrkf.ViewType = 0;
                        EntryStatus = EntryStatus.VIEWMODE;
                        GetFieldValues(ControlsEnum.INVOICEGET);
                        SetFieldValues(ControlsEnum.INVOICEGET);
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
                                //btnSubmit.Visible = false;
                            }
                            else
                            {
                                ucrWrkf.ViewType = 1;
                                EntryStatus = EntryStatus.ENTRYMODE;
                            }
                            //start
                            if (string.IsNullOrEmpty(pid) || pid.Equals("1") || pid.Equals("11"))
                            {
                                ucrWrkf.RefID = int.Parse(refID);
                                base.WkfRefID = ucrWrkf.RefID;
                                CurrPK = GetApplicationID(ucrWrkf.RefID);
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
                        }

                        SelectedSOListForInvComm = SelectedSosForInvComm;
                        SelectedSosForInvComm = null;
                        if (CurrPOPK > 0 || CurrPK > 0)
                        {
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                                //EntryStatus = EntryStatus.VIEWMODE;
                            }

                            GetFieldValues(ControlsEnum.AGTEDITHEADER);//ControlsEnum.FCRDETAILS
                            SetFieldValues(ControlsEnum.AGTHEADER);
                            SetFieldValues(ControlsEnum.AGTDETAIL);



                        }
                        else
                        {
                            string[] datakeyarray;
                            datakeyarray = new string[1];
                            datakeyarray[0] = "IVH_PK";
                            grdAGTLIST.DataKeyNames = datakeyarray;

                            TempPOInvoiceHeaderSession = null;
                            POInvoiceHeaderSession = null;

                            //if (SelectedSosForInvComm != null && SelectedSosForInvComm.Count > 0)
                            if (SelectedSOListForInvComm != null && SelectedSOListForInvComm.Count > 0)
                            {
                                EntryStatus = EntryStatus.NEWMODE;
                                GetFieldValues(ControlsEnum.AGTHEADER);
                                SetFieldValues(ControlsEnum.AGTHEADER);
                                SetFieldValues(ControlsEnum.AGTDETAIL);
                            }
                            else
                            {
                                GetFieldValues(ControlsEnum.AGTLIST);
                                SetFieldValues(ControlsEnum.AGTLIST);
                                EntryStatus = EntryStatus.LISTMODE;
                            }

                        }
                        if (!string.IsNullOrEmpty(fromType) || fromType != "")
                        {
                            if (Convert.ToInt16(fromType) == (int)POInvoiceGroup.AgtInvoice)
                            {
                                CurrPK = fromPK == null ? 0 : Convert.ToInt16(fromPK);
                                ActionHandler(ActionsEnum.EDIT, new EventArgs());
                            }
                        }
                        hdfAstDocMode.Value = GetDOCMODE();
                        hdfAstCode.Value = ApplicationType.ACI;
                        lblInvoiceNo.Text = hdfInvoiceNoComm.Value == string.Empty ? "[NEW]" : hdfInvoiceNoComm.Value;
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
            //AdmCompanyMstService admCompanyMstServiceClient;

            FinTrxService finTrxServiceClient;
            ServiceUtility serviceUtilityObj;
            CommonService CommonServiceClient;
            CommonServiceClient = null;

            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    case ControlsEnum.AGTHEADER:
                        string xmlDoc;
                        AgtCommGetKVBOObj = (AgtCommGetKVBO)SetUIValuesToObject(ControlsEnum.AGENTGET);
                        if (AgtCommGetKVBOObj != null && AgtCommGetKVBOObj.InvoiceCusComm != null)
                        {
                            xmlDoc = CommonFunctions.XmlSerialize<AgtCommGetKVBO>(AgtCommGetKVBOObj);

                            AgtCommHeaderObj = BusinessLogic.Sales.SaleOrderForAgtCommBL.GetAgentCommInvHeader(xmlDoc, CurrPK);
                            AgtCommHeaderSession = AgtCommHeaderObj;
                            if (AgtCommHeaderObj == null && CurrPK != 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "');", true);
                            }
                        }
                        break;
                    #region  INVOICEGET
                    case ControlsEnum.INVOICEGET:
                        int GInvPk = Convert.ToInt32(Request.QueryString[QueryStrings.PK]);
                        int statusFilter = 3;
                        dsPageData = BusinessLogic.Sales.SaleOrderForAgtCommBL.GetAGTList(
                           new BusinessObject.GridPrams()
                           {                              
                               FromDate =string.Empty ,
                               ToDate =string.Empty,
                               SearchBy = "IVH_PK",
                               SearchValue = GInvPk.ToString()
                           }, currentUser, string.Empty, statusFilter);

                        if (dsPageData != null)
                        {
                            DataView dvInvoice = dsPageData.Tables[1].DefaultView;
                            string Fillter = string.Empty;                          
                            dvInvoice.RowFilter = Fillter;
                            dtInvoiceList = dvInvoice.ToTable();
                        }

                        break;
                    #endregion
                    #region  AGTLIST
                    case ControlsEnum.AGTLIST:
                        // int BankPk = (String.IsNullOrEmpty(ddlHold.SelectedValue.Trim())) || (Convert.ToInt16(ddlHold.SelectedValue.Trim()) <= 0) ? 0 : Convert.ToInt32(ddlHold.SelectedValue);
                        int Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        dsPageData = BusinessLogic.Sales.SaleOrderForAgtCommBL.GetAGTList(
                           new BusinessObject.GridPrams()
                           {
                               SortBy = string.IsNullOrEmpty(SortBy) ? "IVH_DATE" : SortBy,
                               SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                               ThenBy = SortBy == ThenBy || SortBy == Resources.DataFieldRes.INVNo ? string.Empty : string.IsNullOrEmpty(ThenBy) ? Resources.DataFieldRes.INVNo : ThenBy,
                               ThenDirection = SortBy == ThenBy || SortBy == Resources.DataFieldRes.INVNo ? string.Empty : string.IsNullOrEmpty(ThenDirection) ? Resources.Report.SortDescending : ThenDirection,
                               FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? string.Empty : txtFromDate.Text.Trim(),
                               ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? string.Empty : txtToDate.Text.Trim(),
                               SearchBy = "IVH_NO",
                               SearchValue = string.IsNullOrEmpty(txtAgtnInvNo.Text.Trim()) ? string.Empty : (txtAgtnInvNo.Text.Trim() == "Select/Type" ? string.Empty : txtAgtnInvNo.Text.Trim())
                           }, currentUser, (hdfCustomerID.Value == "" ? string.Empty : txtCustomer.Text), Status);

                        if (dsPageData != null)
                        {
                            DataView dvInvoice = dsPageData.Tables[1].DefaultView;
                            string Fillter = string.Empty;
                            //if (customer != string.Empty)
                            //{
                            //    Fillter = Fillter + " IVH_VENDOR_TEXT like '%" + customer + "%'";
                            //}
                            //if (txtpoNo.Text != string.Empty)
                            //{
                            //    Fillter = Fillter + " AND IVH_PO_NO like '%" + txtpoNo.Text + "%'";
                            //}
                            dvInvoice.RowFilter = Fillter;
                            dtInvoiceList = dvInvoice.ToTable();
                        }

                        break;
                    #endregion
                    #region EXCHANGERATE
                    case ControlsEnum.EXCHANGERATE:
                        DataSet dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(Convert.ToInt32(hdfCurrency.Value), currentUser.BaseCurrency, Convert.ToDateTime(txtAgtCommDate.Text.Trim()));
                        if (dsExchangeRate != null && dsExchangeRate.Tables[0].Rows.Count > 0)
                        {
                            hdfExchangeRate.Value = dsExchangeRate.Tables[0].Rows[0][0].ToString();
                        }
                        else
                        {
                            hdfExchangeRate.Value = "-1";
                        }
                        break;
                    #endregion


                    #region BANKCURRENCYBASE
                    case ControlsEnum.BANKCURRENCYBASE:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admCurrencyMstObj = new ADM_CURRENCY_MST();
                        admCurrencyMstObj.CUR_PK = !string.IsNullOrEmpty(currentUser.BaseCurrency.ToString()) ? Convert.ToInt32(currentUser.BaseCurrency) : currentUser.BaseCurrency;
                        admCurrencyMstObj.CUR_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        admCurrencyMstList = CommonServiceClient.GetCurrency(admCurrencyMstObj);
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
                    #region EDIT Header
                    case ControlsEnum.AGTEDITHEADER:

                        AgtCommHeaderObj = BusinessLogic.Sales.SaleOrderForAgtCommBL.GetAgentCommInvHeader("", CurrPK);

                        break;
                    #endregion
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
                    #region FCRVOUCHERLIST
                    case ControlsEnum.FCRVOUCHERLIST:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_REF_TYPE = ApplicationType.ACIJ;
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrObj.FTH_VOUCHER_NO = string.Empty;
                        //pdcVoucherList = finTrxServiceClient.GetPDCVoucherList(finTrxHdrObj);
                        break;
                    #endregion

                    #region FIN HEADER BY PK
                    case ControlsEnum.FINHEADERBYPK:
                        //Gets Fin_TRX_Hdr table values by PK
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrObj.FTH_PK = voucherPK;//Session[ERP.Utilities.SessionStrings.TrxPK] == null ? CurrPK : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TrxPK].ToString());
                        finTrxHdrObj.FTH_IS_DELETED = false;
                        finTrxHdrList = finTrxServiceClient.GetfinTxtHdrListByPK(finTrxHdrObj);
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
                finTrxServiceClient=null;
                serviceUtilityObj=null;
                CommonServiceClient=null;
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
                    case ControlsEnum.AGTHEADER:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.AGTDETAIL:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.AGTDETAILGRID:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    case ControlsEnum.AGTLIST:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.CURRENCY:
                        BindDropDown(controlType);
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
        /// Check the uploaded file is valid
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private bool IsValidExtension(string extension)
        {
            //string BlockedExtensions = "dll";
            //if (System.Configuration.ConfigurationManager.AppSettings["BlockedExtensions"].ToLower() != string.Empty)
            //{
            //    BlockedExtensions = System.Configuration.ConfigurationManager.AppSettings["BlockedExtensions"].ToLower();
            //}
            bool flag = true;
            //string[] extensionList = BlockedExtensions.Split(',');
            //for (int i = 0; i < extensionList.Length; i++)
            //    if (("." + extensionList[i]) == extension)
            //    {
            //        flag = false;
            //        break;
            //    }
            return flag;
        }
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        //
        private int FillProcessId()
        {
            int procId = 0;
            string path;
            path = "/Finance/FCReverse.aspx";
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
            DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
            if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
            {
                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
            }
            #endregion
        }
        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.ACI, 0, DateTime.Now);
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
        /// Funtion used get FC NO
        /// </summary>
        private void getAgtCommNo()
        {
            cm = new CommonService();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            string WhtNo = cm.GetTrxDocNo(ApplicationType.ACI, 0, currentUser.CurrentDeptPK,
                txtAgtCommDate.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(txtAgtCommDate.Text), currentUser.PKUser, true, 0, Convert.ToInt32(ddlCompany.SelectedValue));
            hdfAgtCommNo.Value = WhtNo;
        }
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            bool bIsChecked = false;


            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            try
            {
                switch (controlType)
                {
                    #region AGT COMM Hdr
                    case ControlsEnum.AGTCOMMHDR:
                        string gfg = txtAgtCommDate.Text;
                        invoiceHeaderObjComm.IVH_DATE = string.IsNullOrEmpty(txtAgtCommDate.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtAgtCommDate.Text.Trim();

                        invoiceHeaderObjComm.IVH_AMOUNT_TC = Convert.ToDouble(hdfCommAmtTotalFooter.Value);
                        invoiceHeaderObjComm.IVH_TAX_TC = string.IsNullOrEmpty(txtHdrTax.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrTax.Text.Trim());
                        invoiceHeaderObjComm.IVH_DISCOUNT_TC = 0;
                        //invoiceHeaderObjComm.IVH_AMOUNT_NET_TC = string.IsNullOrEmpty(txtHdrTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrTotal.Text.Trim());
                        invoiceHeaderObjComm.IVH_AMOUNT_NET_TC = string.IsNullOrEmpty(hdfCommAmtTotalFooter.Value.Trim()) ? 0 : Convert.ToDouble(hdfCommAmtTotalFooter.Value.Trim());

                        invoiceHeaderObjComm.IVH_CURRENCY = Convert.ToInt16(hdfCurrency.Value);
                        invoiceHeaderObjComm.IVH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        invoiceHeaderObjComm.IVH_EXCHG_RATE = string.IsNullOrEmpty(hdfExchangeRate.Value) ? 1 : Convert.ToDouble(hdfExchangeRate.Value);
                        invoiceHeaderObjComm.IVH_AMOUNT_NET_BC = invoiceHeaderObjComm.IVH_AMOUNT_NET_TC * invoiceHeaderObjComm.IVH_EXCHG_RATE;

                        invoiceHeaderObjComm.IVH_VENDOR = Convert.ToInt16(hdfAgtComm.Value);
                        invoiceHeaderObjComm.IVH_PK = CurrPK;
                        invoiceHeaderObjComm.IVH_VERSION = 1;
                        invoiceHeaderObjComm.IVH_NO = lblInvoiceNo.Text;
                        invoiceHeaderObjComm.IVH_TYPE = Convert.ToInt16(hdfSoType.Value);//SO type
                        invoiceHeaderObjComm.IVH_GROUP = (byte)POInvoiceGroup.AgtInvoice;

                        invoiceHeaderObjComm.IVH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);
                        invoiceHeaderObjComm.BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        invoiceHeaderObjComm.IVH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        invoiceHeaderObjComm.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        invoiceHeaderObjComm.USER_PK = Convert.ToInt16(currentUser.PKUser);
                        invoiceHeaderObjComm.LAST_MOD_DT = LastModifiedTime;

                        invoiceHeaderObjComm.APT_CODE = ApplicationType.ACI;
                        invoiceHeaderObjComm.AST_DOC_MODE = hdfAstDocMode.Value == "1" ? 1 : 0;
                        invoiceHeaderObjComm.WKF_FLAG = 0;

                        invoiceHeaderObjComm.IVH_BRANCH_TEXT = txtHdrBranchCode.Text;
                        invoiceHeaderObjComm.IVH_TAX_ID = txtHdrTaxId.Text;
                        invoiceHeaderObjComm.IVH_BRANCH_TYPE = HdrchkHO.Checked ? (byte)VendorContactTypeEnum.HeadOffice : (byte)VendorContactTypeEnum.Branch;
                        invoiceHeaderObjComm.IVH_IS_SETTLED = chkIsSetteled.Checked ? (byte)AgCommInvoiceSettledTypeEnum.Settled : (byte)AgCommInvoiceSettledTypeEnum.NonSettled;

                        invoiceHeaderObjComm.IVH_VENDOR_INV_NO = HttpUtility.HtmlDecode(txtVendorInvNO.Text);
                        if (!string.IsNullOrEmpty(txtVendorInvDate.Text))
                            invoiceHeaderObjComm.IVH_VENDOR_INV_DATE = txtVendorInvDate.Text;
                        invoiceHeaderObjComm.IVH_CREDIT_DAYS = txtCreditDays.Text;
                        invoiceHeaderObjComm.IVH_DATE_PAY_BY = string.IsNullOrEmpty(txtInvoiceDueDate.Text.Trim()) ?
                            DateTime.Now.ToString() : txtInvoiceDueDate.Text.Trim();
                        invoiceHeaderObjComm.IVH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                        invoiceHeaderObjComm.IVH_DATE_RECEIVED = txtInvoiceDueDate.Text.Trim();

                        invoiceHeaderObjComm.agtCommInvDetail = new List<AgentCommInvSaveDetails>();
                        List<AgentCommInvSaveDetails> detailsList = new List<AgentCommInvSaveDetails>();
                        AgentCommInvSaveDetails objDetail;
                        int slno = 0;
                        foreach (GridViewRow inRow in grdAgtCommDetails.Rows)
                        {
                            if (!String.IsNullOrEmpty((inRow.FindControl("txtCommAmt") as TextBox).Text))
                            {
                                objDetail = new AgentCommInvSaveDetails();
                                HiddenField hdfCus = (HiddenField)inRow.FindControl("hdfCus");
                                HiddenField hdfItem = (HiddenField)inRow.FindControl("hdfItem");
                                HiddenField hdfDtlPK = (HiddenField)inRow.FindControl("hdfDtlPK");
                                HiddenField hdfCommrate = (HiddenField)inRow.FindControl("hdfCommrate");
                                HiddenField hdfCommType = (HiddenField)inRow.FindControl("hdfCommType");
                                TextBox txtCommAmt = (TextBox)inRow.FindControl("txtCommAmt");
                                Label lblTotPcs = (Label)inRow.FindControl("lblTotPcs");
                                HiddenField hdfTotPcs = (HiddenField)inRow.FindControl("hdfTotPcs");
                                HiddenField hdfUOM = (HiddenField)inRow.FindControl("hdfUOM");

                                Label lblformula = (Label)inRow.FindControl("lblCommrate");
                                HiddenField hdfCommFormulaText = (HiddenField)inRow.FindControl("hdfCommFormulaText");

                                objDetail.AVD_COMMISSION_AMT = string.IsNullOrEmpty(txtCommAmt.Text) ? 0.0 : Convert.ToDouble(txtCommAmt.Text);
                                objDetail.AVD_COMMISSION_RATE = string.IsNullOrEmpty(hdfCommrate.Value) ? 0 : Convert.ToDouble(hdfCommrate.Value);
                                objDetail.AVD_COMMISSION_TYPE = string.IsNullOrEmpty(hdfCommType.Value) ? "" : hdfCommType.Value;
                                objDetail.AVD_CUST_ITEM = string.IsNullOrEmpty(hdfItem.Value) ? "" : hdfItem.Value;
                                objDetail.AVD_CUSTOMER = string.IsNullOrEmpty(hdfCus.Value) ? 0 : Convert.ToInt16(hdfCus.Value);
                                objDetail.AVD_INV_CUS_DTL = string.IsNullOrEmpty(hdfDtlPK.Value) ? 0 : Convert.ToInt32(hdfDtlPK.Value);
                                objDetail.AVD_COMMISSION_FORMULA_TEXT = string.IsNullOrEmpty(lblformula.Text) ? "" : lblformula.Text;
                                objDetail.AVD_COMMISSION_FORMULA = string.IsNullOrEmpty(hdfCommFormulaText.Value) ? "" : hdfCommFormulaText.Value;

                                objDetail.VID_SL_NO = slno + 1;

                                //objDetail.VID_QTY_INVOICED = string.IsNullOrEmpty(lblTotPcs.Text) ? 0 : Convert.ToDouble(lblTotPcs.Text);
                                objDetail.VID_QTY_INVOICED = string.IsNullOrEmpty(hdfTotPcs.Value) ? 0 : Convert.ToDouble(hdfTotPcs.Value);
                                objDetail.VID_UOM = string.IsNullOrEmpty(hdfUOM.Value) ? 0 : Convert.ToInt16(hdfUOM.Value);
                                objDetail.VID_RATE = 0;
                                objDetail.VID_AMOUNT = Math.Round(objDetail.AVD_COMMISSION_AMT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                objDetail.VID_DISCOUNT = 0;
                                objDetail.VID_TAX = 0;
                                objDetail.VID_NET_AMOUNT = Math.Round(objDetail.AVD_COMMISSION_AMT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                objDetail.VID_BRANCH_TYPE = 0;

                                detailsList.Add(objDetail);

                            }
                        }
                        invoiceHeaderObjComm.agtCommInvDetail = detailsList;
                        retObject = invoiceHeaderObjComm;

                        break;
                    #endregion

                    #region Journalize
                    case ControlsEnum.JOURNALIZE:
                        if (EntryStatus == EntryStatus.LISTMODE)
                        {
                            foreach (GridViewRow grdrow in grdAGTLIST.Rows)
                            {
                                RadioButton rbtn;
                                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                                if (rbtn.Checked)
                                {
                                    bIsChecked = true;
                                    CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfAGTpk")).Value);
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
                        ////
                        if (bIsChecked)
                        {
                            if (Approved == 2)
                            {
                                GetFieldValues(ControlsEnum.AGTEDITHEADER);
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

                                AGTCommV = ApplicationType.ACIJ;
                                ucrJournalize.TransactionType = AGTCommV;
                                Session[ERP.Utilities.SessionStrings.TransactionType] = AGTCommV;
                                ucrJournalize.TransactionPK = CurrPK;
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                                ucrJournalize.JournalizePK = 0;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = AgtCommHeaderObj.IVH_NO;
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = AgtCommHeaderObj.IVH_DATE;
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = AgtCommHeaderObj.IVH_CURRENCY;
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.AP;
                                //Session[ERP.Utilities.SessionStrings.AccountPayablePK] = FCHoldReverseObj.IVH_VENDOR;
                                Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.ACIJ;

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
                                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.VIEWMODE) && ucrWrkf.HasPageTaskPermission)
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
                                Session[ERP.Utilities.SessionStrings.JournalHead] = GetLocalResourceObject("Agt_Comm_Journal").ToString();

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
                    #region AGENTGET
                    case ControlsEnum.AGENTGET:
                        AgtCommGetKVBOObj = new AgtCommGetKVBO();
                        InvoiceCus InvoiceCusObj;
                        List<InvoiceCus> InvoiceCusList;
                        InvoiceCusList = new List<InvoiceCus>();
                        //SelectedSOListForInvComm = SelectedSosForInvComm;
                        if (SelectedSOListForInvComm.Count > 0 && SelectedSOListForInvComm != null)
                        {

                            for (int i = 0; i < SelectedSOListForInvComm.Count; i++)
                            {
                                InvoiceCusObj = new InvoiceCus();

                                InvoiceCusObj.ICH_PK = Convert.ToInt32(SelectedSOListForInvComm[i]);
                                InvoiceCusList.Add(InvoiceCusObj);
                            }
                        }
                        AgtCommGetKVBOObj.InvoiceCusComm = InvoiceCusList;
                        retObject = AgtCommGetKVBOObj;
                        break;
                    #endregion
                }
                return retObject;

            }
            catch
            {
                throw;
            }
            finally
            {

            }
        }


        //       return retObject;
        //       }
        //       catch (Exception ex)
        //       {
        //           throw ex;
        //       }
        //       finally
        //       {
        //       }
        //   }
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
                            FillProcessID(1);

                            if (!string.IsNullOrEmpty(dtInvoiceList.Rows[0]["IVH_DEPT"].ToString()) && int.TryParse(dtInvoiceList.Rows[0]["IVH_DEPT"].ToString(), out dept))
                            {
                                Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                base.SetUserDept();
                            }

                            if (Convert.ToInt16(dtInvoiceList.Rows[0]["IVH_DEL_STATUS"].ToString()) == 1)
                            {
                                hdfdelsAl.Value = dtInvoiceList.Rows[0]["IVH_DEL_STATUS"].ToString();
                                btnEdit.Visible = false;
                                btnEditforCancel.Visible = false;
                                IsDeleted = true;
                            }
                            else
                            {
                                IsDeleted = false;
                            }


                            setvisibility(ActionsEnum.VIEW);
                            Approved = Convert.ToInt32(dtInvoiceList.Rows[0]["IVH_STATUS"].ToString());

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
                            GetFieldValues(ControlsEnum.AGTEDITHEADER);
                            SetFieldValues(ControlsEnum.AGTHEADER);
                            SetFieldValues(ControlsEnum.AGTDETAIL);
                            //TempPOInvoiceHeaderSession = POInvoiceHeaderSession; //need to check with shafeek
                        }
                        break; 
                    #endregion
                    case ControlsEnum.AGTHEADER:
                        if (AgtCommHeaderObj != null)
                        {
                            CurrPK = AgtCommHeaderObj.IVH_PK;
                            hdfCurrentPk.Value = CurrPK.ToString();
                            Approved = AgtCommHeaderObj.IVH_STATUS;
                            lblInvoiceNo.Focus();
                            lblAgtCommTxt.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(AgtCommHeaderObj.IVH_VENDOR_NAME), 40);
                            hdfAgtComm.Value = AgtCommHeaderObj.IVH_VENDOR.ToString();
                            hdfSoType.Value = AgtCommHeaderObj.IVH_TYPE.ToString();

                            //custPK = Convert.ToInt32(AgtCommHeaderObj.ICH_CUSTOMER);

                            txtVendorInvNO.Text = HttpUtility.HtmlDecode(AgtCommHeaderObj.IVH_VENDOR_INV_NO);
                            txtVendorInvDate.Text = AgtCommHeaderObj.IVH_VENDOR_INV_DATE;
                            txtCreditDays.Text = AgtCommHeaderObj.IVH_CREDIT_DAYS.ToString();
                            if (CurrPK > 0)
                            {
                                LastModifiedTime = AgtCommHeaderObj.LAST_MOD_DT;
                                txtInvoiceDueDate.Text = Convert.ToDateTime(AgtCommHeaderObj.IVH_DATE_PAY_BY).ToString(Resources.Constants.DateFormatShort);
                                hdfInvoiceDueDate.Value = AgtCommHeaderObj.IVH_DATE.ToString();
                            }
                            else
                            {
                                txtInvoiceDueDate.Text = Convert.ToDateTime(AgtCommHeaderObj.IVH_DATE).AddDays(AgtCommHeaderObj.IVH_CREDIT_DAYS).ToString(Resources.Constants.DateFormatShort);
                                hdfInvoiceDueDate.Value = AgtCommHeaderObj.IVH_DATE.ToString();
                            }
                            txtAgtCommDate.Text = AgtCommHeaderObj.IVH_DATE; //DateTime.Parse(HttpUtility.HtmlDecode(AgtCommHeaderObj.IVH_DATE)).ToString(Resources.Constants.DateFormatShort);

                            hdfCurrency.Value = AgtCommHeaderObj.IVH_CURRENCY.ToString();
                            //GetFieldValues(ControlsEnum.EXCHANGERATE);
                            lblCurrencyTxt.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(AgtCommHeaderObj.IVH_CURRENCY_TEXT), 21);
                            lblCurrencyTxt.ToolTip = HttpUtility.HtmlDecode(AgtCommHeaderObj.IVH_CURRENCY_TEXT);
                            hdfInvoiceNoComm.Value = AgtCommHeaderObj.IVH_NO == string.Empty ? "" : AgtCommHeaderObj.IVH_NO;
                            lblInvoiceNo.Text = AgtCommHeaderObj.IVH_NO == string.Empty ? "[NEW]" : AgtCommHeaderObj.IVH_NO;
                            txtRemarks.Text = HttpUtility.HtmlDecode(AgtCommHeaderObj.IVH_REMARKS);

                            txtHdrAddType.Text = string.IsNullOrEmpty(AgtCommHeaderObj.IVH_BRANCH_NAME) ? "Select/Type" : AgtCommHeaderObj.IVH_BRANCH_NAME;
                            if (Convert.ToInt32(AgtCommHeaderObj.IVH_BRANCH_TYPE) == (int)VendorContactTypeEnum.HeadOffice)
                            {
                                HdrchkHO.Checked = true;
                            }

                            if (Convert.ToInt32(AgtCommHeaderObj.IVH_IS_SETTLED) == (int)AgCommInvoiceSettledTypeEnum.Settled)
                            {
                                chkIsSetteled.Checked = true;
                            }
                            else
                            {
                                chkIsSetteled.Checked = false;
                            }

                            //chkIsSetteled.Checked = Convert.ToBoolean(AgtCommHeaderObj.IVH_IS_SETTLED);

                            txtHdrBranchCode.Text = string.IsNullOrEmpty(AgtCommHeaderObj.IVH_BRANCH_TEXT) ? "00000" : AgtCommHeaderObj.IVH_BRANCH_TEXT;
                            txtHdrTaxId.Text = string.IsNullOrEmpty(AgtCommHeaderObj.IVH_TAX_ID) ? string.Empty : AgtCommHeaderObj.IVH_TAX_ID;
                            hdfAddTypeHdr.Value = AgtCommHeaderObj.IVH_VENDOR_CONTACT == null ? string.Empty : AgtCommHeaderObj.IVH_VENDOR_CONTACT.ToString();
                            ddlCompany.SelectedValue = AgtCommHeaderObj.IVH_COMPANY == null ? "0" : AgtCommHeaderObj.IVH_COMPANY.ToString();
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
                    }
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
                    case ControlsEnum.AGTDETAIL:


                        if (AgtCommHeaderObj != null)
                        {
                            AgentCommInvDetailsList = new List<AgentCommInvDetails>();

                            AgentCommInvDetailsList = AgtCommHeaderObj.agtCommInvDetail;//.Where(sod => sod.CID_QTY_DISPATCHED > 0).ToList();//To hide items without despatched and shipping qty

                            if (AgentCommInvDetailsList != null)
                            {
                                grdAgtCommDetails.DataSource = AgentCommInvDetailsList;
                                grdAgtCommDetails.DataBind();
                            }
                            if (EntryStatus == EntryStatus.NEWMODE)
                            { 
                               ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateInvNow", "$(document).ready(function(){CalculateInvNow();});", true); }
                        }
                        break;
                    case ControlsEnum.AGTLIST:
                        if (dtInvoiceList != null)
                        {
                            //GetFieldValues(ControlsEnum.FILLWORKFLOWSTATUS);
                            PageIndex = PageIndex == null ? "0" : PageIndex;
                            grdAGTLIST.PageIndex = Convert.ToInt32(PageIndex);
                            grdAGTLIST.DataSource = dtInvoiceList.DefaultView;
                            grdAGTLIST.DataBind();
                        }
                        break;


                    case ControlsEnum.UPLOADEDFILES:
                        if (POUploadList != null)
                        {
                            grdUploads.DataSource = POUploadList;
                            grdUploads.DataBind();
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

                case ControlsEnum.AGTEDITHEADER:
                    CurrPK = 0;
                    break;
                case ControlsEnum.AGTLIST:
                    CurrPK = 0;
                    ModifiedDatePnl.Visible = false;
                    LastModifiedTime = DateTime.Now;
                    break;
                case ControlsEnum.clearAdvSearch:
                    txtFromDate.Text = string.Empty;
                    txtToDate.Text = string.Empty;
                    txtAgtnInvNo.Text = "Select/Type";
                    txtCustomer.Text = "Select/Type";
                    hdfCustomerID.Value = string.Empty;
                    hdfFCRPK.Value = "0";
                    ddlStatus.SelectedValue = "3";//For Resolving Bug ID:  1556 
                    txtVno.Text = string.Empty;
                    break;
            }
        }

        private void setvisibility(ActionsEnum ActionsEnum)
        {
            switch (ActionsEnum)
            {
                #region EDIT
                case ActionsEnum.EDIT:
                    if (Convert.ToInt16(hdfdelsAl.Value) == 1)
                    {
                        btnEditforCancel.Visible = false;
                        // btnPrint.Visible = false;
                        btnJournalize.Visible = false;
                        //btnSubmit.Visible = false;
                    }
                    break;
                #endregion
                #region Company
                case ActionsEnum.VIEW:

                    foreach (GridViewRow grdRow in grdAgtCommDetails.Rows)
                    {
                        TextBox txtReverseNow = grdRow.FindControl("txtReverseNow") as TextBox;
                        TextBox txtcommissionAmt = (TextBox)grdRow.FindControl("txtCommAmt");
                        if (txtReverseNow != null)
                        {
                            txtReverseNow.Enabled = false;
                        }
                        txtcommissionAmt.Enabled = false;
                    }
                    if (Convert.ToInt16(hdfdelsAl.Value) == 1)
                    {
                        btnEditforCancel.Visible = false;
                        // btnPrint.Visible = false;
                        btnJournalize.Visible = false;
                        // btnSubmit.Visible = false;
                    }

                    break;
                #endregion
                #region New
                case ActionsEnum.NEW:

                    foreach (GridViewRow grdRow in grdAgtCommDetails.Rows)
                    {
                        TextBox txtReverseNow = grdRow.FindControl("txtReverseNow") as TextBox;
                        if (txtReverseNow != null)
                        {
                            txtReverseNow.Enabled = true;
                        }
                    }

                    break;
                #endregion
            }
        }

        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormat.Value);
        }
        public string GetFormattedNumberWithSeperator(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormatWithSeperator.Value);
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
        #endregion
        #region ActionHandler
        /// <summary>
        /// Button actions events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                BusinessObject.User currentUser;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int? result;
                bool bIsChecked = false;
                FileInfo tempFileInfoObj;
                string savePath = string.Empty;
                int selectedItemPK;

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
                    if (((DropDownList)sender).ID == "ddlCurrency")
                    {
                        commonActions = ActionsEnum.BANKCURRENCY;
                    }
                    if (((DropDownList)sender).ID == "ddlHoldAccount")
                    {
                        commonActions = ActionsEnum.FCHOLDREVERTDTL;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    if (((TextBox)sender).ID == "txtReverseNow")
                    {
                        commonActions = ActionsEnum.CHECKAMT;
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
                    if (((CheckBox)sender).ID == "HdrchkHO")
                    {
                        commonActions = ActionsEnum.CHECKVISIBLE;
                    }
                }
                else
                {
                    fromType = string.IsNullOrEmpty(fromType) ? ((int)POInvoiceGroup.AgtInvoice).ToString() : (fromType);
                    if ((Convert.ToInt16(fromType) == (int)POInvoiceGroup.AgtInvoice) && (CurrPK > 0))
                    {
                        commonActions = ActionsEnum.EDIT;
                    }
                }
                switch (commonActions)
                {
                    #region save
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            invoiceHeaderObjComm = new AgtCommBO();
                            invoiceHeaderObjComm = (AgtCommBO)SetUIValuesToObject(ControlsEnum.AGTCOMMHDR);
                            if (invoiceHeaderObjComm != null && invoiceHeaderObjComm.agtCommInvDetail != null)
                            {
                                string xmlDoc = CommonFunctions.XmlSerialize<AgtCommBO>(invoiceHeaderObjComm);//CommonFunctions.ObjectTOXml(rfqResponseHeaderObj);
                                // save Process Control inspection details
                                result = Convert.ToInt32(BusinessLogic.Sales.SaleOrderForAgtCommBL.SaveSalesInvoiceAgtComm(xmlDoc));
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
                                    // Show Save Message and redired to listing page                                        
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AgtComm);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);

                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.clearAdvSearch);
                                    ResetForm(ControlsEnum.AGTLIST);
                                    GetFieldValues(ControlsEnum.AGTLIST);
                                    SetFieldValues(ControlsEnum.AGTLIST);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }


                        }
                        break;
                    #endregion
                    #region Invoice Comm List
                    case ActionsEnum.AGTLIST:
                        SelectedSosForInvComm = null;
                        SelectedSOListForInvComm = null;
                        if ((Convert.ToInt16(fromType == "" ? "0" : fromType) == (int)POInvoiceGroup.AgtInvoice) && (CurrPK > 0))
                        {
                            Response.Redirect(Resources.PageURL.ExpenseInvoice);
                        }
                        else
                        {
                            FillProcessID(1);
                            CurrPK = 0;
                            ResetForm(ControlsEnum.AGTLIST);
                            GetFieldValues(ControlsEnum.AGTLIST);
                            SetFieldValues(ControlsEnum.AGTLIST);
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                        break;
                    #endregion
                    #region Invoice Details
                    case ActionsEnum.AGTDETAIL:
                        setvisibility(ActionsEnum.EDIT);
                        //setvisibility(ActionsEnum.VIEW);
                        foreach (GridViewRow grdrow in grdAGTLIST.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfAGTpk")).Value);
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
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
                                //EntryStatus = EntryStatus.VIEWMODE;
                            }
                            ucrWrkf.ViewAction();
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.AGTEDITHEADER);
                            SetFieldValues(ControlsEnum.AGTHEADER);
                            GetFieldValues(ControlsEnum.AGTDETAIL);
                            SetFieldValues(ControlsEnum.AGTDETAIL);
                            //TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
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
                        setvisibility(ActionsEnum.VIEW);
                        foreach (GridViewRow grdrow in grdAGTLIST.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfAGTpk")).Value);
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
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
                                //EntryStatus = EntryStatus.VIEWMODE;
                                ucrWrkf.ViewAction();
                            }
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.AGTEDITHEADER);
                            SetFieldValues(ControlsEnum.AGTHEADER);
                            SetFieldValues(ControlsEnum.AGTDETAIL);
                            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;
                            setvisibility(ActionsEnum.VIEW);

                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region CHECKVISIBLE
                    case ActionsEnum.CHECKVISIBLE:
                        if (HdrchkHO.Checked == true)
                        { txtHdrBranchCode.Enabled = false; txtHdrBranchCode.CssClass = "medium"; }
                        else
                        { txtHdrBranchCode.Enabled = true; txtHdrBranchCode.CssClass = "medium input-disabled"; }
                        break;
                    #endregion
                    #region Edit
                    case ActionsEnum.EDIT:
                        setvisibility(ActionsEnum.EDIT);
                        if ((Convert.ToInt16(fromType == "" ? "0" : fromType) != (int)POInvoiceGroup.AgtInvoice)) //&& (CurrPK <= 0)
                        {
                            bIsChecked = false;
                            foreach (GridViewRow grdrow in grdAGTLIST.Rows)
                            {
                                RadioButton rbtn;
                                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                                if (rbtn.Checked)
                                {
                                    bIsChecked = true;
                                    CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfAGTpk")).Value);
                                    ////start
                                    Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                    Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                    ////
                                    break;
                                }
                            }
                        }
                        else
                        {
                            bIsChecked = true;
                        }
                        if (bIsChecked)
                        {
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
                            GetFieldValues(ControlsEnum.AGTEDITHEADER);
                            SetFieldValues(ControlsEnum.AGTHEADER);
                            SetFieldValues(ControlsEnum.AGTDETAIL);

                            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;


                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region ADDITEM
                    case ActionsEnum.ADDITEMUPLOAD:
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

                                        poUploadObj.DOC_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                        FileDetailsList.Add(new FileDetails() { SlNo = slno, PoFile = HttpContext.Current.Request.Files[0] });
                                        POUploadList.Add(poUploadObj);

                                    }
                                }
                                SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                ResetForm(ControlsEnum.ADDITEM);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ScrollDown();", true);
                            }
                        }
                        break;
                    #endregion

                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEMUPLOAD:
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
                        break;
                    #endregion
                    #region EDITITEM
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
                        break;
                    #endregion
                    #region Tab navigation
                    case ActionsEnum.DEFAULT:
                        Response.Redirect(Resources.PageURL.SalAgentComm);
                        break;
                    case ActionsEnum.AGTINVOICE:
                        Response.Redirect(Resources.PageURL.AgtComm);
                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:

                        //Show WorkFlow Popup

                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);

                        break;
                    #endregion
                    #region Submit
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion

                    #region WRKFSUBMIT
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

                            ucrWrkf.ApplicationID = 0;
                            invoiceHeaderObjComm = new AgtCommBO();
                            invoiceHeaderObjComm = (AgtCommBO)SetUIValuesToObject(ControlsEnum.AGTCOMMHDR);

                            hasValidRate = false;

                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                if (hdfExchangeRate.Value != "-1")
                                {
                                    invoiceHeaderObjComm.WKF_FLAG = 1;
                                    //if (hasValidRate)
                                    //{

                                    if (invoiceHeaderObjComm != null && invoiceHeaderObjComm.agtCommInvDetail != null)
                                    {
                                        string xmlDoc = CommonFunctions.XmlSerialize<AgtCommBO>(invoiceHeaderObjComm);//CommonFunctions.ObjectTOXml(rfqResponseHeaderObj);
                                        // save Process Control inspection details
                                        result = Convert.ToInt32(BusinessLogic.Sales.SaleOrderForAgtCommBL.SaveSalesInvoiceAgtComm(xmlDoc));
                                        ResetForm(ControlsEnum.clearAdvSearch);
                                        if (result.HasValue && result.Value > 0) // Success !  redirect to listing page
                                        {
                                            //reset
                                            ucrWrkf.ApplicationID = result.Value;
                                        }
                                        else
                                        {

                                            if (result == (int)DbSaveStatus.SQLERROR)
                                            {
                                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                                            {
                                                litErrorMsg.Text = Resources.PageNameRes.BrnadRates + " " + Resources.Messages.EditUsedByAnotherUser;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.CODEEXIST)
                                            {
                                                litErrorMsg.Text = Resources.PageNameRes.BrnadRates + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                            }
                                            else
                                            {
                                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BrnadRates);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                        }

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
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(CurrPK, ApplicationType.ACI))
                                {
                                    ucrWrkf.ApplicationID = CurrPK;
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_AI_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        FillProcessID(1);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.AGTLIST);
                                    GetFieldValues(ControlsEnum.AGTLIST);
                                    SetFieldValues(ControlsEnum.AGTLIST);
                                }
                            }
                            else
                                ucrWrkf.ApplicationID = CurrPK;
                            if (ucrWrkf.ApplicationID > 0)
                            {
                                ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                //Do WorkFlow if WorkFlow has Actions
                                if (ddlWkfAction.Items.Count > 0)
                                {
                                    action = ddlWkfAction.SelectedItem.ToString();
                                    result = ucrWrkf.DoWorkFlow();
                                    CurrPK = 0;
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

                                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                        string invoiceNo = string.Empty;
                                        if (string.IsNullOrEmpty(lblInvoiceNo.Text.Trim())
                                            || lblInvoiceNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                                        {
                                            CurrPK = ucrWrkf.ApplicationID;
                                            GetFieldValues(ControlsEnum.AGTEDITHEADER);
                                            invoiceNo = AgtCommHeaderObj.IVH_NO;
                                        }
                                        else
                                        {
                                            invoiceNo = lblInvoiceNo.Text.Trim();
                                        }
                                        object[] args = new object[2];
                                        args[0] = Resources.PageNameRes.AgtComm;
                                        args[1] = invoiceNo;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                                        //Show Save success message and reset Contract Entry

                                        if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                              + "','" + Resources.ErpRes.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                            ResetForm(ControlsEnum.AGTLIST);
                                            GetFieldValues(ControlsEnum.AGTLIST);
                                            SetFieldValues(ControlsEnum.AGTLIST);
                                        }
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Wrkflw_Error;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                           + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                            }
                            else
                            {
                                //Trx not saved
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Error_NoPK;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AgtComm);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup3", "ClosePopup();", true);
                        }
                        break;
                    #endregion
                    #region ITEMSELECTED
                    case ActionsEnum.ITEMSELECTED:

                        foreach (GridViewRow grdrow in grdAGTLIST.Rows)
                        {
                            RadioButton rbtn;
                            HiddenField hdfDept;
                            HiddenField hdfdels;
                            int selectedPK;
                            int dept;

                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                FillProcessID(1);
                                bIsChecked = true;
                                selectedPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfAGTpk")).Value);


                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                hdfdels = grdrow.FindControl("hdfdels") as HiddenField;
                                hdfIsCancelled.Value = hdfdels.Value; 
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                if (Convert.ToInt16(hdfdels.Value) == 1)
                                {
                                    hdfdelsAl.Value = hdfdels.Value;
                                    btnEdit.Visible = false;
                                    btnEditforCancel.Visible = false;
                                    IsDeleted = true;
                                }
                                else
                                {
                                    IsDeleted = false;
                                }

                                //base.WkfRefID = workflowCore.GetRefID(selectedPK, PageProcessID);
                                break;
                            }
                        }

                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        SelectedSosForInvComm = null;
                        SelectedSOListForInvComm = null;
                        if ((Convert.ToInt16(fromType == "" ? "0" : fromType) == (int)POInvoiceGroup.AgtInvoice) && (CurrPK > 0))
                        {
                            Response.Redirect(Resources.PageURL.ExpenseInvoice);
                        }
                        else
                        {
                            FillProcessID(1);
                            GetFieldValues(ControlsEnum.AGTLIST);
                            SetFieldValues(ControlsEnum.AGTLIST);
                            EntryStatus = EntryStatus.LISTMODE;

                        }
                        break;
                    #endregion
                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
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
                        ResetForm(ControlsEnum.AGTLIST);
                        GetFieldValues(ControlsEnum.AGTLIST);
                        SetFieldValues(ControlsEnum.AGTLIST);
                        break;
                    #endregion
                    #region Journalize Save
                    case ActionsEnum.JOURNALIZESAVE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ((Button)sender).CommandName = ActionsEnum.SAVE.ToString();
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.AGTLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.AGTLIST);
                        SetFieldValues(ControlsEnum.AGTLIST);
                        break;
                    #endregion
                    #region Journalize Submit
                    case ActionsEnum.JOURNALIZESUBMIT:
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                        }
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.AGTLIST);

                        //if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ReferanceID > 0)
                        //{
                        //    Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        //    Response.Redirect(Resources.PageURL.InboxURL);
                        //}
                        //else
                        //{
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.AGTLIST);
                        SetFieldValues(ControlsEnum.AGTLIST);
                        //}
                        break;
                    #endregion
                    #region Journalize Delete
                    case ActionsEnum.JOURNALIZEDELETE:
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                        }
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.AGTLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.AGTLIST);
                        SetFieldValues(ControlsEnum.AGTLIST);
                        break;
                    #endregion
                    #region Journalize Cancel
                    case ActionsEnum.JOURNALIZECANCEL:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.AGTLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.AGTLIST);
                        SetFieldValues(ControlsEnum.AGTLIST);
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = BusinessLogic.Sales.SaleOrderForAgtCommBL.DeleteAgtCommDetails(CurrPK, LastModifiedTime, ApplicationType.ACI, currentUser.PKUser);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AgtComm);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm(ControlsEnum.AGTLIST);
                                GetFieldValues(ControlsEnum.AGTLIST);
                                SetFieldValues(ControlsEnum.AGTLIST);
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
                                    litErrorMsg.Text = Resources.PageNameRes.AgtComm + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.AGTLIST);
                                    GetFieldValues(ControlsEnum.AGTLIST);
                                    SetFieldValues(ControlsEnum.AGTLIST);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.AgtComm + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.AgtComm + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.AGTLIST);
                                    GetFieldValues(ControlsEnum.AGTLIST);
                                    SetFieldValues(ControlsEnum.AGTLIST);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AgtComm);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region CHECK AMT in detail Grid
                    case ActionsEnum.CHECKAMT:

                        double maxQty = 0;

                        Label lblAmount;
                        lblAmount = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("lblAmount") as Label);


                        Label lblReversed;
                        lblReversed = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("lblReversed") as Label);


                        TextBox txtReverseNow;
                        txtReverseNow = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtReverseNow") as TextBox);


                        Label lblInvQuantity;
                        Label lblInvProformaQuantity;
                        //txtQuantity = sender as TextBox;
                        lblInvQuantity = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("lblInvQuantity") as Label);
                        lblInvProformaQuantity = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("lblInvProformaQuantity") as Label);
                        if (txtReverseNow.Text == null)
                        { txtReverseNow.Text = "0"; }
                        if (grdAgtCommDetails.Rows.Count > 0 || grdAgtCommDetails != null)
                        {
                            if (lblAmount != null && lblReversed != null)
                            {

                                if (txtReverseNow.Text == "")
                                    txtReverseNow.Text = Convert.ToDouble(0).ToString();
                                maxQty = Convert.ToDouble(lblAmount.Text) - Convert.ToDouble(lblReversed.Text);
                                maxQty = ERP.Utilities.CommonFunctions.DoubleFormatRound(maxQty, 2);
                                if (Convert.ToDouble(txtReverseNow.Text) > maxQty)
                                {
                                    txtReverseNow.Text = Convert.ToDouble(0).ToString();
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_InvoiceQty").ToString();
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, maxQty);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                        }




                        break;
                    #endregion
                    #region PRINT
                    case ActionsEnum.PRINT:
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.ACI + "&APPSUBTYPE=0") + "');", true);

                        }
                        break;
                    #endregion
                    #region Printlisting
                    case ActionsEnum.PRINTLISTING:
                        foreach (GridViewRow grdrow in grdAGTLIST.Rows)
                        {
                            HiddenField hdfAGTpk;
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                hdfAGTpk = (HiddenField)grdrow.FindControl("hdfAGTpk");
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfAGTpk.Value.ToString() + "&APPTYPE=" + ApplicationType.ACI + "&APPSUBTYPE=0") + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdAGTLIST.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfAGTpk")).Value);
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                //int hdfPosted = Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                //Posted = Convert.ToBoolean(hdfPosted);
                                ////
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
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            ucrWrkf.ViewAction();

                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.AGTEDITHEADER);
                            SetFieldValues(ControlsEnum.AGTHEADER);
                            SetFieldValues(ControlsEnum.AGTDETAIL);


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
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.AGTLIST);
                        SetFieldValues(ControlsEnum.AGTLIST);
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.clearAdvSearch);
                        GetFieldValues(ControlsEnum.AGTLIST);
                        SetFieldValues(ControlsEnum.AGTLIST);
                        break;
                    #endregion
                    #region SHOWPOPUP -- inv No Print
                    case ActionsEnum.SHOWPOPUP:
                        if (((LinkButton)sender).CommandArgument != null)
                        {

                            if (hdfSoType.Value != string.Empty)
                            {
                                if (hdfSoType.Value.ToString().ToLower() == "1")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=1") + "');", true);
                                }
                                else if (hdfSoType.Value.ToString().ToLower() == "2")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=2") + "');", true);
                                }
                                else if (hdfSoType.Value.ToString().ToLower() == "3")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=3") + "');", true);
                                }
                            }

                        }
                        break;
                    #endregion
                    #region SHOWSC -- SC No Print
                    case ActionsEnum.SHOWSC:
                        if (((LinkButton)sender).CommandArgument != null)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.SO + "&APPSUBTYPE=") + "');", true);
                        }
                        break;
                    #endregion
                    #region SHOW -- Voucher No Print
                    case ActionsEnum.SHOW:
                        if (((LinkButton)sender).CommandArgument != null)
                        {
                            voucherPK = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                            GetFieldValues(ControlsEnum.FINHEADERBYPK);
                            if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                            {
                                long trxPK = finTrxHdrList[0].FTH_REF_PK;//Reference PK
                                string trxType = finTrxHdrList[0].FTH_REF_TYPE;//Reference Type
                                string appSubType = string.Empty;
                                string appType = finTrxHdrList[0].FTH_REF_TYPE;
                                string company = finTrxHdrList[0].FTH_COMPANY.ToString();//Company
                                string _printerMode = "";
                                #region Getting Printer Mode
                                DataSet dsParamSettings = new DataSet();
                                string RptType = appType;//Session[ERP.Utilities.SessionStrings.Type].ToString();
                                int RptSubType = 0;
                                Int32.TryParse(appSubType, out RptSubType);
                                DateTime AppvdDate = DateTime.Now;
                                List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList = new CommonService().GetReportParameters(RptType, RptSubType, AppvdDate);
                                if (AppTypeDetailsList != null && AppTypeDetailsList.Count > 0)
                                {
                                    dsParamSettings.ReadXml(new System.IO.StringReader(Convert.ToString(AppTypeDetailsList[0].AST_RPT_SETTINGS)));
                                    if (dsParamSettings.Tables.Count > 0 && dsParamSettings.Tables[0].Columns.Contains("PRINTER_MODE"))
                                    {
                                        _printerMode = dsParamSettings.Tables[0].Rows[0]["PRINTER_MODE"].ToString();
                                    }
                                }
                                #endregion
                                #region DotMatrix Printing
                                //if (_printerMode == PrinterMode.DOTMATRIX.ToString()) // && Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.JV )
                                //{
                                //    string redirectUrl = string.Empty;
                                //    if (appType.Equals(ApplicationType.JV) || appType.Equals(ApplicationType.PCS))
                                //    {
                                //        redirectUrl = string.Format("../Reports/GenerateReport.aspx?ID={0}&APPTYPE={1}&APPSUBTYPE={2}&TRXTYPE={3}&COMPANY={4}&PRINTERMODE={5}",
                                //                                                                  CurrPK.ToString(), // CurrPK = TrxPk
                                //                                                                  appType,
                                //                                                                  appSubType,
                                //                                                                  trxType,
                                //                                                                  company,
                                //                                                                  _printerMode);
                                //    }
                                //    else
                                //    {
                                //        redirectUrl = string.Format("../Reports/GenerateReport.aspx?ID={0}&APPTYPE={1}&APPSUBTYPE={2}&TRXTYPE={3}&COMPANY={4}&PRINTERMODE={5}",
                                //                                                                 trxPK.ToString(), // trxPK = RefPk
                                //                                                                 appType,
                                //                                                                 appSubType,
                                //                                                                 trxType,
                                //                                                                 company,
                                //                                                                 _printerMode);
                                //    }
                                //    appSubType = "0";
                                //    Response.Redirect(redirectUrl, false);
                                //}
                                #endregion
                                #region Normal Printing
                                //else
                                //{
                                if (appType.Equals(ApplicationType.JV) || appType.Equals(ApplicationType.PCS))
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + appType +
                                        "&APPSUBTYPE=" + appSubType + "&TRXTYPE=" + trxType + "&COMPANY=" + company + "');", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + trxPK.ToString() + "&APPTYPE=" + appType +
                                        "&APPSUBTYPE=" + appSubType + "&TRXTYPE=" + trxType + "&COMPANY=" + company + "');", true);
                                }
                                //}
                                #endregion
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

            }
        }

        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //if (((GridView)sender).ID == "grdAGTLIST")
                //{
                //    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                //    {
                //        Button imgPosted = e.Row.FindControl("imgPosted") as Button;

                //        HiddenField hdfPosted = e.Row.FindControl("hdfPosted") as HiddenField;

                //        if (hdfPosted.Value == "True")
                //        {
                //            imgPosted.CssClass = GetLocalResourceObject("posted").ToString();
                //            imgPosted.ToolTip = Resources.Captions.Posted;
                //        }
                //        else
                //        {
                //            imgPosted.CssClass = GetLocalResourceObject("unposted").ToString();
                //            imgPosted.ToolTip = Resources.Captions.NotPosted;
                //        }
                //    }

                //}
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
            PageIndex = e.NewPageIndex.ToString();
            GetFieldValues(ControlsEnum.AGTLIST);
            SetFieldValues(ControlsEnum.AGTLIST);
            EntryStatus = EntryStatus.LISTMODE;
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
                GetFieldValues(ControlsEnum.AGTLIST);
                SetFieldValues(ControlsEnum.AGTLIST);
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
            btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            //btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnSave.PreRender += new EventHandler(btnAction_PreRender);
            btnDeleteNew.PreRender += new EventHandler(btnAction_PreRender);
            btnJournalize.PreRender += new EventHandler(btnAction_PreRender);
            btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            //btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            btnView.PreRender += new EventHandler(btnAction_PreRender);
            lnkList.PreRender += new EventHandler(btnAction_PreRender);
            lnkDetail.PreRender += new EventHandler(btnAction_PreRender);
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);


            btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            btnSubmit.Load += new EventHandler(btnAction_Load);
            btnSave.Load += new EventHandler(btnAction_Load);
            //btnCancel.Load += new EventHandler(btnAction_Load);
            btnDeleteNew.Load += new EventHandler(btnAction_Load);
            btnJournalize.Load += new EventHandler(btnAction_Load);
            //btnPrint.Load += new EventHandler(btnAction_Load);
            btnEdit.Load += new EventHandler(btnAction_Load);
            btnView.Load += new EventHandler(btnAction_Load);
            lnkList.Load += new EventHandler(btnAction_Load);
            lnkDetail.Load += new EventHandler(btnAction_Load);
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
            this.Init += new EventHandler(this.Page_Init);
        }

        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        {
            //try
            //{
            //    switch (e.Action)
            //    {
            //        //case NavigationEnum.PAGECHANGE:
            //        //    uclPaging.CurrentPage = e.CurrentPage;
            //        //    break;
            //        //case NavigationEnum.FIRST:
            //        //    if (e.CurrentPage > 1)
            //        //        uclPaging.CurrentPage = 1;
            //        //    break;
            //        //case NavigationEnum.LAST:
            //        //    if (e.CurrentPage <= e.TotalPages)
            //        //        uclPaging.CurrentPage = e.TotalPages;
            //        //    break;
            //        //case NavigationEnum.NEXT:
            //        //    // increment the current page index.
            //        //    if (e.CurrentPage <= e.TotalPages)
            //        //        uclPaging.CurrentPage++;
            //        //    break;
            //        //case NavigationEnum.PREVIOUS:
            //        //    // Decrement the current page index.
            //        //    if (e.CurrentPage > 1)
            //        //        uclPaging.CurrentPage--;
            //        //    break;


            //    }

            //    //PageIndex = uclPaging.CurrentPage.ToString();
            //    // Change Code As per the page
            //    //GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
            //    //SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
            //    EntryStatus = EntryStatus.LISTMODE;
            //    //============================
            //    EnableDisableButtons(e.TotalPages);
            //}
            //catch (Exception ex)
            //{
            //    // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            // }

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
            //InitializeComponent();

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
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateInvNow", "$(document).ready(function(){CalculateInvNow();});", true);
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotFooter", "$(document).ready(function(){CalculateTotFooter();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);

                if (IsDeleted)
                {

                    btnEdit.Visible = false;
                    btnEditforCancel.Visible = false;
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
            AGTLIST,
            TAXTYPES,
            TAXPOPUPGRID,
            TAXHEADER,
            EXCHANGERATE,
            JOURNALIZE,
            FINHEADER,
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
            CURRENCY,
            BANKCURRENCYBASE,
            FCRVOUCHERLIST,
            clearAdvSearch,
            FINHEADERBYPK,
            AGTHEADER,
            AGTDETAIL,
            AGENTGET,
            AGTCOMMDETAILS,
            AGTCOMMHDR,
            AGTEDITHEADER,
            AGTDETAILGRID,
            INVOICEGET
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