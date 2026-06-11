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

namespace ERPSMS_v01.Finance
{
    public partial class FCReverse : ERP.Store.UI.WorkFlowBasePage// ERP.Store.UI.MyBasePage
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
        //private POInvoiceGroup FCRGroup
        //{
        //    get
        //    {
        //        return (this.ViewState[ViewstateStrings.FCRGroup] == null ? (POInvoiceGroup)Enum.Parse(typeof(POInvoiceGroup),
        //            CommonConstants.SELECT_VALUE_ONE) : (POInvoiceGroup)this.ViewState[ViewstateStrings.FCRGroup]);
        //    }
        //    set
        //    {
        //        this.ViewState[ViewstateStrings.FCRGroup] = value;
        //    }
        //}


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
        private string FCReverseV
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.FCReverse];
            }
            set
            {
                this.ViewState[ViewstateStrings.FCReverse] = value;
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
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        User currentUser;
        //page related Entity Object
        private FCHoldReverseBO FCHoldReverseObj;

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
        //private RFQTaxDtl rfqTaxDtlObj;
        List<POInvoiceDetails> soInvoiceDetailsList;
        //List<RFQTaxDtl> rfqTaxDtlList;
        //RFQTaxSplit rfqDtlSplitObj;
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
                    txtFCDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    //GetFieldValues(ControlsEnum.FCRLIST);
                    //SetFieldValues(ControlsEnum.FCRLIST);
                    GetFieldValues(ControlsEnum.FCHold);
                    SetFieldValues(ControlsEnum.FCHold);
                    SetFieldValues(ControlsEnum.HOLDACCOUNT);
                    GetFieldValues(ControlsEnum.CURRENCY);
                    SetFieldValues(ControlsEnum.CURRENCY);
                    GetFieldValues(ControlsEnum.FCHOLDREVERTDTL);
                    SetFieldValues(ControlsEnum.FCHOLDREVERTDTL);
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
                    hdfJournalizeWorkFlow.Value = "0";
                    hdfDecimalFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                    }
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                    }
                    hdfRateFormat.Value = "#0.";
                    int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P]));
                    for (int i = 0; i < rateDecimalDigits; i++)
                    {
                        hdfRateFormat.Value += "0";
                    }

                    hdfExchangeRateFormat.Value = "#0.";
                    int exchrateDecimalDigits = (Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
                    for (int i = 0; i < exchrateDecimalDigits; i++)
                    {
                        hdfExchangeRateFormat.Value += "0";
                    }

                    FillProcessID(1);
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

                    //If Request From External(Report or Other page) otherthan Menu or Inbox
                    if (Request.QueryString[QueryStrings.FromExt] != null && Request.QueryString[QueryStrings.FromExt] == "T")
                    {
                        ucrWrkf.ViewType = 0;
                        EntryStatus = EntryStatus.VIEWMODE;
                        GetFieldValues(ControlsEnum.FCREVERSEGET);
                        SetFieldValues(ControlsEnum.FCREVERSEGET);
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
                            ////start
                            if (string.IsNullOrEmpty(pid) || pid.Equals("1") || pid.Equals("11"))
                            {
                                ucrWrkf.RefID = int.Parse(refID);
                                base.WkfRefID = ucrWrkf.RefID;
                                CurrPK = GetApplicationID(ucrWrkf.RefID);
                                if (pid.Equals("11"))
                                    hdfIsCancelled.Value = "1";//For Showing Cancelled Stamp in Detail Page
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

                            GetFieldValues(ControlsEnum.FCRHEADER);//ControlsEnum.FCRDETAILS
                            SetFieldValues(ControlsEnum.FCRHEADER);
                            GetFieldValues(ControlsEnum.FCRDETAILGRID);//ControlsEnum.FCRDETAILS
                            SetFieldValues(ControlsEnum.FCRDETAILGRID);



                        }
                        else
                        {
                            string[] datakeyarray;
                            datakeyarray = new string[1];
                            datakeyarray[0] = "HRH_PK";
                            grdFCRList.DataKeyNames = datakeyarray;

                            TempPOInvoiceHeaderSession = null;
                            POInvoiceHeaderSession = null;

                            GetFieldValues(ControlsEnum.FCRLIST);
                            SetFieldValues(ControlsEnum.FCRLIST);
                            EntryStatus = EntryStatus.LISTMODE;

                            //btnEdit.Visible = btnJournalize.Visible = btnPickForCrDrNote.Visible = btnPickForPayment.Visible =
                            //   BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(currentUser.PKUser, FillProcessId());
                        }
                        AST_CODE.Value = ApplicationType.FCHR;
                        AST_DOC_MODE.Value = GetDOCMODE();

                        txtFCDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                        lblFcReversalNo.Text = hdfFCReverseNo.Value == string.Empty ? "[NEW]" : hdfFCReverseNo.Value;

                        hdfAppType.Value = ApplicationType.FCHR;
                        hdfAppSubType.Value = string.Empty;
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
            //AdmCompanyMstService admCompanyMstServiceClient;

            DataSet dsTaxDetails;
            FinTrxService finTrxServiceClient;
            ServiceUtility serviceUtilityObj;
            CommonService CommonServiceClient;
            CommonServiceClient = null;

            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    #region  FCREVERSEGET
                    case ControlsEnum.FCREVERSEGET:
                        int GInvPk = Convert.ToInt32(Request.QueryString[QueryStrings.PK]);                      
                        int statusFilter = 0;
                        dsPageData = BusinessLogic.Finance.FCReverseBL.GetFCRList(
                           new BusinessObject.GridPrams()
                           {                              
                               FromDate = DateTime.Now.AddYears(-100).ToString() ,
                               ToDate  =  DateTime.Now.AddYears(10).ToString()                               
                           }, currentUser, string.Empty, 0, GInvPk, Convert.ToByte(DbActiveStatus.HASPK), statusFilter);
                      
                        if (dsPageData != null)
                        {                            
                            DataView dvInvoice = dsPageData.Tables[0].DefaultView;
                            string Fillter = string.Empty;
                            dvInvoice.RowFilter = Fillter;
                            dtInvoiceList = dvInvoice.ToTable();
                        }

                        break;
                    #endregion
                    #region  FCRLIST
                    case ControlsEnum.FCRLIST:
                        //int cusID = String.IsNullOrEmpty(hdfCustomerID.Value.Trim()) ? 0 : Convert.ToInt32(hdfCustomerID.Value);
                        //if (hdfCustomerID.Value != "" && hdfCustomerID.Value != "0")
                        //{
                        //    Session[ERP.Utilities.SessionStrings.VendorPK] = hdfCustomerID.Value;
                        //    Session[ERP.Utilities.SessionStrings.Vendor] = txtCustomer.Text;
                        //}
                        int BankPk = (String.IsNullOrEmpty(ddlHold.SelectedValue.Trim())) || (Convert.ToInt16(ddlHold.SelectedValue.Trim()) <= 0) ? 0 : Convert.ToInt32(ddlHold.SelectedValue);
                        int Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        dsPageData = BusinessLogic.Finance.FCReverseBL.GetFCRList(
                           new BusinessObject.GridPrams()
                           {
                               SortBy = string.IsNullOrEmpty(SortBy) ? "HRH_DATE" : SortBy,
                               SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                               ThenBy = SortBy == ThenBy || SortBy == "HRH_NO" ? string.Empty : string.IsNullOrEmpty(ThenBy) ? "HRH_NO" : ThenBy,
                               ThenDirection = SortBy == ThenBy || SortBy == "HRH_NO" ? string.Empty : string.IsNullOrEmpty(ThenDirection) ? Resources.Report.SortDescending : ThenDirection,
                               //FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? string.Empty : txtFromDate.Text.Trim(),
                               //ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? string.Empty : txtToDate.Text.Trim(),
                               FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? DateTime.Now.AddYears(-100).ToString() : txtFromDate.Text.Trim(),
                               ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? DateTime.Now.AddYears(10).ToString() : txtToDate.Text.Trim(),
                               //SearchBy = "IVH_NO",
                               SearchValue = string.IsNullOrEmpty(txtVoucherNumber.Text.Trim()) ? string.Empty : (txtVoucherNumber.Text.Trim() == "Select/Type" ? string.Empty : txtVoucherNumber.Text.Trim())
                           }, currentUser, txtVno.Text.Trim(), BankPk, 0, Convert.ToByte(DbActiveStatus.ACTIVE), Status);
                        //,0
                        //, 0, (byte)POInvoiceCategory.Invoice
                        //, chkPending.Checked == true ? (byte)1 : (byte)0);
                        if (dsPageData != null)
                        {
                            // string customer = string.IsNullOrEmpty(txtCustomer.Text.Trim()) ? string.Empty : (txtCustomer.Text.Trim() == "Select/Type" ? string.Empty : txtCustomer.Text.Trim());
                            DataView dvInvoice = dsPageData.Tables[0].DefaultView;
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
                    #region  FCHold
                    case ControlsEnum.FCHold:
                        dtPageData = BusinessLogic.Sales.Enquiry.GetBankDetails(0, 1, currentUser.SBUID, (int)CashBankType.Bank, 1);
                        break;
                    #endregion
                    #region Get Fc Hold Revert Details
                    case ControlsEnum.FCHOLDREVERTDTL:
                        dsPageData = BusinessLogic.Finance.FCReverseBL.GetFCHoldRevertDetails(CurrPK, Convert.ToInt32(ddlHoldAccount.SelectedValue), Convert.ToInt16(ddlCurrency.SelectedValue));
                        break;
                    #endregion
                    #region Get Currency
                    case ControlsEnum.CURRENCY:
                        dtCurrency = BusinessLogic.CommonManagement.CommonBL.GetCurrencyHold(Convert.ToInt32(ddlHoldAccount.SelectedValue));
                        break;
                    #endregion
                    #region BANKCURRENCY
                    case ControlsEnum.BANKCURRENCY:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admCurrencyMstObj = new ADM_CURRENCY_MST();
                        admCurrencyMstObj.CUR_PK = ddlCurrency.SelectedValue != "-1" ? Convert.ToInt32(ddlCurrency.SelectedValue) : currentUser.BaseCurrency;
                        admCurrencyMstObj.CUR_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        admCurrencyMstList = CommonServiceClient.GetCurrency(admCurrencyMstObj);
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
                    #region Generate Exchange Rate in base Currency
                    case ControlsEnum.FCREXCHANGERATEINBASECURRENCY:
                        dtExchngrate = BusinessLogic.CommonManagement.CommonBL.GetExchangeRateTable(Convert.ToInt32(ddlCurrency.SelectedValue), Convert.ToInt32(currentUser.BaseCurrency), txtFCDate.Text);
                        break;
                    #endregion
                    #region Generate Exchange currency of Hold
                    case ControlsEnum.FCRCURRENCYHOLD:
                        dtCurrency = BusinessLogic.CommonManagement.CommonBL.GetCurrencyHold(Convert.ToInt32(ddlHold.SelectedValue));
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
                    case ControlsEnum.FCRHEADER:
                        Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        //CurrPK = 76;
                        //CurrPOPK = 0;
                        //invoiceHeaderObj = BusinessLogic.POInvoicing.POInvoiceBL.GetPOInvoiceHeader(CurrPK > 0 ? 0 : CurrPOPK, CurrPK);
                        dsPageData = BusinessLogic.Finance.FCReverseBL.GetFCRList(null, currentUser, null, 0, CurrPK <= 0 ? 0 : CurrPK, Convert.ToByte(DbActiveStatus.HASPK), Status);

                        if (dsPageData == null && CurrPK != 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region EDIT Header
                    case ControlsEnum.FCRDETAILGRID:
                        //CurrPK = 76;
                        //CurrPOPK = 0;
                        //invoiceHeaderObj = BusinessLogic.POInvoicing.POInvoiceBL.GetPOInvoiceHeader(CurrPK > 0 ? 0 : CurrPOPK, CurrPK);
                        dsPageData = BusinessLogic.Finance.FCReverseBL.GetFCHoldRevertDetails(CurrPK <= 0 ? 0 : CurrPK, Convert.ToInt16(ddlHoldAccount.SelectedValue), Convert.ToInt16(ddlCurrency.SelectedValue));

                        if (dsPageData == null && CurrPK != 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "');", true);
                        }
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
                        finTrxHdrObj.FTH_REF_TYPE = ApplicationType.FCHRJ;
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrObj.FTH_VOUCHER_NO = string.Empty;
                        //pdcVoucherList = finTrxServiceClient.GetPDCVoucherList(finTrxHdrObj);
                        break;
                    #endregion
                    #region
                    //case ControlsEnum.POINVHEADER:
                    //    //CurrPK = 76;
                    //    //CurrPOPK = 0;
                    //    //invoiceHeaderObj = BusinessLogic.POInvoicing.POInvoiceBL.GetPOInvoiceHeader(CurrPK > 0 ? 0 : CurrPOPK, CurrPK);
                    //    invoiceHeaderObj = BusinessLogic.POInvoicing.POInvoiceBL.GetPOInvoiceHeader(CurrPK > 0 ? 0 : CurrPOPK, CurrPK);
                    //    POInvoiceHeaderSession = invoiceHeaderObj;
                    //    POInvoiceHdrSession = invoiceHeaderObj;
                    //    if (invoiceHeaderObj == null && CurrPK != 0)
                    //    {
                    //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "');", true);
                    //    }
                    //    break;
                    //case ControlsEnum.TAXTYPES:
                    //    int category = 1;
                    //    int.TryParse(hdfTaxCategory.Value, out category);
                    //    if (TaxPK > 0)
                    //    {
                    //        dsTaxDetails = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetRFQTaxDetails(TaxPK, category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.HASPK), 0);
                    //        if (dsTaxDetails != null && dsTaxDetails.Tables.Count > 0)
                    //        {
                    //            dtTaxDetails = dsTaxDetails.Tables[0];
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if ((int)TaxType.Tax == category)
                    //        {
                    //            dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtInvoiceDate.Text), 0, TaxFilterType.PUR);
                    //        }
                    //        else
                    //        {
                    //            dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtInvoiceDate.Text), 0);
                    //        }
                    //    }
                    //    break;
                    //case ControlsEnum.EXCHANGERATE:
                    //    DataSet dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(Convert.ToInt32(hdfCurrency.Value), currentUser.BaseCurrency, Convert.ToDateTime(txtInvoiceDate.Text.Trim()));
                    //    if (dsExchangeRate != null && dsExchangeRate.Tables[0].Rows.Count > 0)
                    //    {
                    //        hdfExchangeRate.Value = dsExchangeRate.Tables[0].Rows[0][0].ToString();
                    //    }
                    //    else
                    //    {
                    //        hdfExchangeRate.Value = "-1";
                    //    }
                    //    break;


                    //#region FIN HEADER
                    //case ControlsEnum.FINHEADER:
                    //    finTrxServiceClient = new FinTrxService();
                    //    finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                    //    serviceUtilityObj = new ServiceUtility();
                    //    serviceUtilityObj.PageSize = 10;
                    //    finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                    //    finTrxHdrObj.FTH_REF_TYPE = Session[ERP.Utilities.SessionStrings.TransactionType].ToString();
                    //    finTrxHdrObj.FTH_REF_PK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString());
                    //    finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                    //    finTrxHdrList = finTrxServiceClient.GetfinTxtHdrList(finTrxHdrObj, serviceUtilityObj);
                    //    break;
                    //#endregion
                    //case ControlsEnum.POTYPE:
                    //    dtPageData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PURCHASE INVOICE TYPE");
                    //    break;


                    //#region FILLWORKFLOWSTATUS
                    //case ControlsEnum.FILLWORKFLOWSTATUS:
                    //    CommonServiceClient = new CommonService();
                    //    CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                    //    admConfigMstObj = new ADM_CONFIG_MST();
                    //    admConfigMstObj.CFG_PK = 0;
                    //    admConfigMstObj.CFG_TYPE = GetLocalResourceObject("APPLICATION_STATUS").ToString();
                    //    admConfigMstObj.CFG_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                    //    workflowStatusList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                    //    break;
                    //#endregion
                    //case ControlsEnum.ADVANCEDTAXSETTINGS:
                    //    dtTaxSettings = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("ADVANCE INVOICE SETTINGS", string.Empty, currentUser.SBUID);
                    //    if (dtTaxSettings != null && dtTaxSettings.Rows.Count > 0)
                    //    {
                    //        DataRow drTaxSettings = dtTaxSettings.AsEnumerable().SingleOrDefault(aa => aa.Field<string>("ACF_DATA").Trim() == "TAX");
                    //        if (drTaxSettings != null)
                    //        {
                    //            hdfTaxSettings.Value = drTaxSettings["ACF_VALUE"].ToString();
                    //        }
                    //    }
                    //    break;

                    //#region NOTIFICATIONTYPES

                    //case ControlsEnum.NOTIFICATIONTYPES:
                    //    CommonServiceClient = new CommonService();
                    //    CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                    //    admConfigMstObj = new ADM_CONFIG_MST();
                    //    admConfigMstObj.CFG_PK = 0;
                    //    admConfigMstObj.CFG_TYPE = Resources.Constants.ALERT_NOTIFICATION_TYPES;
                    //    admConfigMstObj.CFG_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                    //    admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                    //    break;
                    //#endregion

                    //#region ALERTBASIS
                    //case ControlsEnum.ALERTBASIS:
                    //    CommonServiceClient = new CommonService();
                    //    CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                    //    admConfigMstObj = new ADM_CONFIG_MST();
                    //    admConfigMstObj.CFG_PK = 0;
                    //    admConfigMstObj.CFG_TYPE = Resources.Constants.ALERT_BASIS;
                    //    admConfigMstObj.CFG_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                    //    admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                    //    break;
                    //#endregion

                    //#region ALERTTYPES
                    //case ControlsEnum.ALERTTYPES:
                    //    CommonServiceClient = new CommonService();
                    //    CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                    //    admConstMstList = CommonServiceClient.GetConstMstValues(null, null, null, 16, 1, currentUser.SBUID);
                    //    break;
                    //#endregion

                    //#region NOTIFICATIONDAYS
                    //case ControlsEnum.NOTIFICATIONDAYS:
                    //    CommonServiceClient = new CommonService();
                    //    CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                    //    admAppConfigMstObj = new ADM_APP_CONFIG_MST();
                    //    admAppConfigMstObj.ACF_PK = 0;
                    //    admAppConfigMstObj.ACF_SETTING = Resources.Constants.ALERT_NOTIFY_BEFORE;
                    //    admAppConfigMstObj.ACF_DATA = ApplicationType.PI;
                    //    admAppConfigMstObj.ACF_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                    //    admAppConstMstList = CommonServiceClient.GetAlertNotify(admAppConfigMstObj);
                    //    break;
                    //#endregion

                    //#region ALERTCONFIG
                    //case ControlsEnum.ALERTCONFIG:
                    //    CommonServiceClient = new CommonService();
                    //    CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                    //    admAppConfigMstObj = new ADM_APP_CONFIG_MST();
                    //    admAppConfigMstObj.ACF_PK = 0;
                    //    admAppConfigMstObj.ACF_SETTING = Resources.Constants.AUTO_ALERT_FROM_TRX;
                    //    admAppConfigMstObj.ACF_DATA = "1";
                    //    admAppConfigMstObj.ACF_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                    //    admAppConstMstList = CommonServiceClient.GetAlertNotify(admAppConfigMstObj);
                    //    break;
                    //#endregion

                    //#region ALERTDETAILS
                    //case ControlsEnum.ALERTLIST:
                    //    dsAlertList = BusinessLogic.AlertManagement.Alerts.GetAlertDetails(0, Convert.ToByte(DbActiveStatus.ACTIVE), null, appType, invPK, currentUser.SBUID, currentUser.PKUser, (int)AlertType.System);
                    //    break;
                    //#endregion
                    //#region Company
                    //case ControlsEnum.COMPANY:
                    //    //gets Company List
                    //    admCompanyMstServiceClient = new AdmCompanyMstService();
                    //    admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                    //    admCompanyMstObj.CMP_ACTIVE = 1;
                    //    serviceUtilityObj = new ServiceUtility();
                    //    admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                    //    break;
                    //#endregion
                    //#region VENDORBRANCH
                    //case ControlsEnum.VENDORBRANCH:
                    //    dtPageData = BusinessLogic.CommonManagement.CommonBL.GetVendorContactList(0, Convert.ToByte(DbActiveStatus.ACTIVE), vndPK, currentUser.SBUID);
                    //    break;
                    //#endregion
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
                    case ControlsEnum.FCREVERSEGET:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.FCRHEADER:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.FCRDETAILGRID:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    case ControlsEnum.FCRLIST:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.FCHOLDREVERTDTL:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.FCHold:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.HOLDACCOUNT:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.CURRENCY:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.BANKCURRENCY:
                        ddlBankCharge.Items.Clear();
                        if (admCurrencyMstList != null && admCurrencyMstList.Count > 0)
                        {
                            ddlBankCharge.Items.Insert(0, (new ListItem(admCurrencyMstList[0].CUR_CODE + " - " + admCurrencyMstList[0].CUR_NAME, admCurrencyMstList[0].CUR_PK.ToString())));
                        }
                        break;
                    case ControlsEnum.FCREXCHANGERATEINBASECURRENCY:
                        string Exchngrate = "";
                        if (dtExchngrate != null && dtExchngrate.Rows.Count > 0)
                        {
                            Exchngrate = dtExchngrate.Rows[0][0].ToString();

                        }
                        //txtExchngRate.Text = string.IsNullOrEmpty(Exchngrate) || Convert.ToDecimal(Exchngrate) < 0 ? "0" : GetFormattedRate(Exchngrate).ToString();
                        txtExchngRate.Text = string.IsNullOrEmpty(Exchngrate) || Convert.ToDecimal(Exchngrate) < 0 ? "0" : GetFormattedExchangeRate(Exchngrate).ToString();
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
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.FCHR, 0, DateTime.Now);
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
        private void getFCReverseNo()
        {
            cm = new CommonService();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            string WhtNo = cm.GetTrxDocNo(ApplicationType.FCHR, 0, currentUser.CurrentDeptPK,
                txtFCDate.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(txtFCDate.Text), currentUser.PKUser, true, 0,Convert.ToInt32(ddlCompany.SelectedValue));
            hdfFCReverseNo.Value = WhtNo;
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
                    #region FCRDETAILS
                    case ControlsEnum.FCRDETAILS:
                        DateTime Hrdate = DateTime.Now;
                        DateTime.TryParse(txtFCDate.Text,out Hrdate);
                        FCHoldReverseObj.HrhPK = CurrPK;

                        FCHoldReverseObj.HrhNo = string.IsNullOrEmpty(hdfFCReverseNo.Value) ? "[NEW]" : hdfFCReverseNo.Value;
                        //FCHoldReverseObj.HrhDate = string.IsNullOrEmpty(txtFCDate.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtFCDate.Text.Trim();
                        FCHoldReverseObj.HrhDate = Hrdate.ToString();
                        FCHoldReverseObj.Bank = Convert.ToInt16(ddlHoldAccount.SelectedValue);

                        FCHoldReverseObj.Remarks = txtDescription.Text;
                        FCHoldReverseObj.Currency = Convert.ToInt16(ddlCurrency.SelectedValue);

                        FCHoldReverseObj.AmountTC = Convert.ToDouble(hdfAmtTC.Value);//?
                        FCHoldReverseObj.BankCharge = Convert.ToDouble(txtBankcharge.Text);
                        FCHoldReverseObj.BankChargeCurrency = Convert.ToInt16(ddlBankCharge.SelectedValue);

                        FCHoldReverseObj.ExchangeRate = Convert.ToDouble(txtExchngRate.Text);
                        FCHoldReverseObj.AmountBC = FCHoldReverseObj.AmountTC * FCHoldReverseObj.ExchangeRate;
                        FCHoldReverseObj.HasJournalEntry = 0;
                        FCHoldReverseObj.DelStatus = 0;
                        FCHoldReverseObj.Status = 0;
                        FCHoldReverseObj.Company = Convert.ToInt16(ddlCompany.SelectedValue);
                        FCHoldReverseObj.Department = currentUser.CurrentDeptPK;

                        FCHoldReverseObj.BizUnit = Convert.ToInt16(currentUser.SBUID);
                        FCHoldReverseObj.AptCode = ApplicationType.FCHR;
                        FCHoldReverseObj.Active = Convert.ToByte(DbActiveStatus.ACTIVE);
                        FCHoldReverseObj.UserPK = Convert.ToInt16(currentUser.PKUser);
                        FCHoldReverseObj.LastModDate = LastModifiedTime.ToString();
                        FCHoldReverseObj.DetailsList = new List<Detail>();
                        List<Detail> detailsList = new List<Detail>();
                        Detail objDetail;
                        foreach (GridViewRow inRow in grdFcHoldDetasils.Rows)
                        {
                            if (!String.IsNullOrEmpty((inRow.FindControl("txtReverseNow") as TextBox).Text))
                            {
                                objDetail = new Detail();
                                HiddenField hdfHRDpk = (HiddenField)inRow.FindControl("hdfHRDpk");
                                HiddenField hdfFtrPK = (HiddenField)inRow.FindControl("hdfFtrPK");
                                TextBox txtReverseNow = (TextBox)inRow.FindControl("txtReverseNow");
                                if (Convert.ToDouble(txtReverseNow.Text) > 0.0)
                                {
                                    objDetail.HrdPK = string.IsNullOrEmpty(hdfHRDpk.Value) ? 0 : Convert.ToInt32(hdfHRDpk.Value);
                                    objDetail.TrxDtlPK = string.IsNullOrEmpty(hdfFtrPK.Value) ? 0 : Convert.ToInt32(hdfFtrPK.Value);// Convert.ToInt32((inRow.FindControl("hdfHdrPK") as HiddenField).Value);
                                    objDetail.HrdAmt = string.IsNullOrEmpty(txtReverseNow.Text) ? 0.0 : Convert.ToDouble(txtReverseNow.Text); //Convert.ToInt32((inRow.FindControl("txtReverseNow") as TextBox).Text);
                                    objDetail.HrdActive = Convert.ToByte(DbActiveStatus.ACTIVE);
                                    detailsList.Add(objDetail);
                                }
                            }
                        }
                        FCHoldReverseObj.DetailsList = detailsList;
                        retObject = FCHoldReverseObj;
                        break;
                    #endregion
                    #region Journalize
                    case ControlsEnum.JOURNALIZE:
                        if (EntryStatus == EntryStatus.LISTMODE)
                        {
                            foreach (GridViewRow grdrow in grdFCRList.Rows)
                            {
                                RadioButton rbtn;
                                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                                if (rbtn.Checked)
                                {
                                    bIsChecked = true;
                                    CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfHRHpk")).Value);
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
                                GetFieldValues(ControlsEnum.FCRHEADER);
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

                                FCReverseV = ApplicationType.FCHRJ;
                                ucrJournalize.TransactionType = FCReverseV;
                                Session[ERP.Utilities.SessionStrings.TransactionType] = FCReverseV;
                                ucrJournalize.TransactionPK = CurrPK;
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                                ucrJournalize.JournalizePK = 0;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = dsPageData.Tables[0].Rows[0]["HRH_NO"];
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = dsPageData.Tables[0].Rows[0]["HRH_DATE"].ToString();
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = dsPageData.Tables[0].Rows[0]["HRH_CURRENCY"].ToString();
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.AP;
                                //Session[ERP.Utilities.SessionStrings.AccountPayablePK] = FCHoldReverseObj.IVH_VENDOR;
                                Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.FCHRJ;

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
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
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
                    case ControlsEnum.FCREVERSEGET:
                        if (dtInvoiceList != null && dtInvoiceList.Rows.Count > 0)
                        {
                            CurrPK = Convert.ToInt32(Request.QueryString["PK"].ToString());
                            if (Convert.ToInt16(dtInvoiceList.Rows[0]["HRH_DEL_STATUS"].ToString()) == 1)
                            {
                                btnSave.Visible = false;
                                btnEditforCancel.Visible = false;
                                hdfIsCancelled.Value = "1";
                            }
                            else
                            {
                                btnSave.Visible = true;
                                btnEditforCancel.Visible = true;
                                hdfIsCancelled.Value = "0";
                            }
                            if (!string.IsNullOrEmpty(dtInvoiceList.Rows[0]["HRH_DEPT"].ToString()) && int.TryParse(dtInvoiceList.Rows[0]["HRH_DEPT"].ToString(), out dept))
                            {
                                Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                base.SetUserDept();
                            }
                            setvisibility(ActionsEnum.VIEW);
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
                            GetFieldValues(ControlsEnum.FCRHEADER);
                            SetFieldValues(ControlsEnum.FCRHEADER);
                            GetFieldValues(ControlsEnum.FCRDETAILGRID);
                            SetFieldValues(ControlsEnum.FCRDETAILGRID);
                            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;                           
                        }
                        break;

                    case ControlsEnum.FCRHEADER:
                        if (dsPageData != null)
                        {
                            //POGroup = (POInvoiceGroup)Enum.Parse(typeof(POInvoiceGroup), FCHoldReverseObj.IVH_GROUP.ToString());
                            //Approved = FCHoldReverseObj.Status;
                            //hdf.Value = FCHoldReverseObj.HrhNo == string.Empty ? "" : FCHoldReverseObj.HrhNo;
                            //lblInvoiceNo.Text = FCHoldReverseObj.HrhNo == string.Empty ? "[NEW]" : FCHoldReverseObj.HrhNo;   
                            //txtFCDate.Text = dsPageData.Tables[0].Rows[0]["HRH_DATE"].ToString();// FCHoldReverseObj.HrhDate;
                            txtFCDate.Text = DateTime.Parse(HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["HRH_DATE"].ToString())).ToString(Resources.Constants.DateFormatShort);
                            ddlHoldAccount.SelectedValue = dsPageData.Tables[0].Rows[0]["HRH_BANK"].ToString();

                            txtDescription.Text = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["HRH_REMARKS"].ToString());
                            GetFieldValues(ControlsEnum.CURRENCY);
                            SetFieldValues(ControlsEnum.CURRENCY);
                            ddlCurrency.SelectedValue = dsPageData.Tables[0].Rows[0]["HRH_CURRENCY"].ToString();
                            Approved = WkfStatus = Convert.ToInt32(dsPageData.Tables[0].Rows[0]["HRH_STATUS"].ToString());

                            hdfAmtTC.Value = dsPageData.Tables[0].Rows[0]["HRH_AMOUNT_TC"].ToString();

                            txtBankcharge.Text = GetFormattedCurrency(dsPageData.Tables[0].Rows[0]["HRH_BANK_CHARGE"].ToString());
                            FillddlBankCharge();
                            ddlBankCharge.SelectedValue = dsPageData.Tables[0].Rows[0]["HRH_BANK_CHARGE_CURR"].ToString();

                            //txtExchngRate.Text = GetFormattedRate(dsPageData.Tables[0].Rows[0]["HRH_EXCHG_RATE"].ToString());
                            txtExchngRate.Text = GetFormattedExchangeRate(dsPageData.Tables[0].Rows[0]["HRH_EXCHG_RATE"].ToString());
                            ddlCompany.SelectedValue = dsPageData.Tables[0].Rows[0]["HRH_COMPANY"].ToString();

                            hdfFCReverseNo.Value = dsPageData.Tables[0].Rows[0]["HRH_NO"] == string.Empty ? "" : dsPageData.Tables[0].Rows[0]["HRH_NO"].ToString();
                            lblFcReversalNo.Text = dsPageData.Tables[0].Rows[0]["HRH_NO"] == string.Empty ? "[NEW]" : dsPageData.Tables[0].Rows[0]["HRH_NO"].ToString();
                            if (CurrPK > 0)
                                LastModifiedTime = Convert.ToDateTime(dsPageData.Tables[0].Rows[0]["HRH_MOD_DT"].ToString());

                        }
                        break;
                    case ControlsEnum.FCRDETAILGRID:
                        if (dsPageData != null)
                        {

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


                case ControlsEnum.FCHold:
                    ddlHold.Items.Clear();
                    if (dtPageData != null)
                    {
                        ddlHold.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CBM_NAME");
                        ddlHold.DataTextField = "CBM_NAME";
                        ddlHold.DataValueField = "CBM_PK";
                        ddlHold.DataBind();
                    }
                    ddlHold.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    //if (bankDetailPK > 0 && ddlHold.Items.FindByValue(bankDetailPK.ToString()) != null)
                    //    ddlHold.SelectedValue = bankDetailPK.ToString();
                    break;
                case ControlsEnum.HOLDACCOUNT:
                    ddlHoldAccount.Items.Clear();
                    if (dtPageData != null)
                    {
                        ddlHoldAccount.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CBM_NAME");
                        ddlHoldAccount.DataTextField = "CBM_NAME";
                        ddlHoldAccount.DataValueField = "CBM_PK";
                        ddlHoldAccount.DataBind();
                    }
                    ddlHoldAccount.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.CURRENCY:
                    ddlCurrency.Items.Clear();
                    if (dtCurrency != null && dtCurrency.Rows.Count > 0)
                    {
                        ddlCurrency.DataSource = dtCurrency;
                        ddlCurrency.DataTextField = Resources.DataFieldRes.Currency;
                        ddlCurrency.DataValueField = Resources.DataFieldRes.CurrencyPK;
                        ddlCurrency.DataBind();
                    }
                    if (ddlCurrency.Items.Count <= 0)
                        ddlCurrency.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;


                //    case ControlsEnum.COMPANY:
                //        ddlCompany.Items.Clear();
                //        if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                //        {
                //            ddlCompany.DataSource = admCompanyMstList;
                //            ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                //            ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                //            ddlCompany.DataBind();
                //        }
                //        break;

                //    case ControlsEnum.VENDORBRANCH:
                //        ddlVendorBranch.Items.Clear();
                //        if (dtPageData != null && dtPageData.Rows.Count > 0)
                //        {
                //            ddlVendorBranch.DataSource = dtPageData;
                //            ddlVendorBranch.DataTextField = "VNC_NAME";
                //            ddlVendorBranch.DataValueField = "VNC_PK";
                //            ddlVendorBranch.DataBind();
                //        }
                //        break;

                //    case ControlsEnum.TAXTYPES:
                //        //Bind Tax dropdown
                //        ddlPopupTaxType.Items.Clear();
                //        if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
                //        {
                //            ddlPopupTaxType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtTaxDetails, Resources.DataFieldRes.RFQResponseTaxHead);
                //            ddlPopupTaxType.DataTextField = Resources.DataFieldRes.RFQResponseTaxHead;
                //            ddlPopupTaxType.DataValueField = Resources.DataFieldRes.RFQResponseTaxPK;
                //            ddlPopupTaxType.DataBind();
                //        }
                //        ddlPopupTaxType.Items.Add(new ListItem(Resources.Report.Custom, CommonConstants.SELECTVAL));
                //        break;
                //    case ControlsEnum.POTYPE:
                //        ddlOrderType.Items.Clear();
                //        if (dtPageData != null)
                //        {
                //            ddlOrderType.DataSource = dtPageData;
                //            ddlOrderType.DataTextField = "CFG_DATA";
                //            ddlOrderType.DataValueField = "CFG_VALUE";
                //            ddlOrderType.DataBind();
                //        }
                //        ddlOrderType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                //        break;
                //    default:
                //        break;
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
                    case ControlsEnum.FCHOLDREVERTDTL:
                        grdFcHoldDetasils.DataSource = dsPageData.Tables[0];
                        grdFcHoldDetasils.DataBind();
                        break;
                    case ControlsEnum.FCRLIST:
                        if (dtInvoiceList != null)
                        {
                            //GetFieldValues(ControlsEnum.FILLWORKFLOWSTATUS);
                            PageIndex = PageIndex == null ? "0" : PageIndex;
                            grdFCRList.PageIndex = Convert.ToInt32(PageIndex);
                            grdFCRList.DataSource = dtInvoiceList.DefaultView;
                            grdFCRList.DataBind();
                        }
                        break;
                    case ControlsEnum.FCRDETAILGRID:
                        grdFcHoldDetasils.DataSource = dsPageData.Tables[0];
                        grdFcHoldDetasils.DataBind();
                        break;
                    //        case ControlsEnum.POINVDETAIL:
                    //            if (invoiceHeaderObj != null)
                    //            {
                    //                soInvoiceDetailsList = new List<POInvoiceDetails>();
                    //                soInvoiceDetailsList = invoiceHeaderObj.OrderDetail;
                    //                if (soInvoiceDetailsList != null)
                    //                {
                    //                    grdInvoice.DataSource = soInvoiceDetailsList;
                    //                    grdInvoice.DataBind();
                    //                }
                    //            }
                    //            break;
                    //        case ControlsEnum.TAXPOPUPGRID:

                    //            if (IsHeaderTax)
                    //            {
                    //                taxHdrList = TempPOInvoiceHeaderSession.TaxHdr.Where(tax => tax.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                    //                // taxHdrList = POInvoiceHdrSession.TaxHdr;
                    //            }
                    //            else
                    //            {
                    //                soDtlObj = TempPOInvoiceHeaderSession.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK);
                    //                if (soDtlObj != null)
                    //                {
                    //                    taxHdrList = soDtlObj.TaxDtl.Where(tax => tax.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                    //                    //taxHdrList = soDtlObj.TaxDtl.Where(tax => tax.VTL_INVOICE_DTL == POInvoicePK && tax.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                    //                }
                    //            }
                    //            grdTaxDetails.DataSource = taxHdrList;
                    //            grdTaxDetails.DataBind();
                    //            break;

                    //        case ControlsEnum.DEDUCTIONPOPUPGRID:
                    //            deductionDtlList = new List<POAdvDeductionDetails>();
                    //            deductionDtlList = TempPOInvoiceHeaderSession.DeductionDetails.ToList();
                    //            if (deductionDtlList != null && deductionDtlList.Count > 0)
                    //            {
                    //                grdDeduction.DataSource = deductionDtlList;
                    //                grdDeduction.DataBind();
                    //                Label lblDedTotalAllocateNowFooterSplit = grdDeduction.FooterRow.FindControl("lblDedTotalAllocateNowFooterSplit") as Label;
                    //                HiddenField hdfDedTotalAllocateNowFooterSplit = grdDeduction.FooterRow.FindControl("hdfDedTotalAllocateNowFooterSplit") as HiddenField;
                    //                if (lblDedTotalAllocateNowFooterSplit != null && hdfDedTotalAllocateNowFooterSplit != null)
                    //                {
                    //                    decimal total = deductionDtlList.Sum(aa => aa.VAD_AMOUNT);
                    //                    if (total == 0)
                    //                    {
                    //                        total = deductionDtlList.Sum(aa => aa.IVH_AMOUNT_TC - aa.IVH_AMOUNT_ALLOCATED);
                    //                    }
                    //                    hdfDedTotalAllocateNowFooterSplit.Value = lblDedTotalAllocateNowFooterSplit.Text = total.ToString(hdfCurrencyFormat.Value);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                grdDeduction.DataSource = deductionDtlList;
                    //                grdDeduction.DataBind();
                    //            }
                    //            break;
                    //        case ControlsEnum.UPLOADEDFILES:
                    //            if (POUploadList != null)
                    //            {
                    //                grdUploads.DataSource = POUploadList;
                    //                grdUploads.DataBind();
                    //            }
                    //            break;
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
                case ControlsEnum.NEW:
                    CurrPK = 0;
                    hdfIsCancelled.Value = "0";
                    ddlHoldAccount.ClearSelection();
                    ddlHold.SelectedValue = CommonConstants.SELECTVAL;
                    //ddlCurrency.DataSource = null;
                    //ddlCurrency.DataBind();
                    //  BindDropDown(ControlsEnum.HOLDACCOUNT);
                    BindDropDown(ControlsEnum.CURRENCY);
                    // ddlCurrency.SelectedValue = CommonConstants.SELECTVAL;
                    txtFCDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfInvoicePK.Value = "";

                    //txtExchngRate.Text = GetFormattedRate(0);
                    txtExchngRate.Text = GetFormattedExchangeRate(0);
                    grdFcHoldDetasils.DataSource = null;
                    grdFcHoldDetasils.DataBind();
                    CurrPOPK = 0;
                    txtVoucherNumber.Text = "Select/Type";
                    ddlBankCharge.Items.Clear();
                    txtBankcharge.Text = GetFormattedCurrency(0);
                    txtDescription.Text = "";
                    lblFcReversalNo.Text = Resources.Messages.DocGenerationNew.ToString();                    
                    break;
                case ControlsEnum.FCRHEADER:
                    CurrPK = 0;
                    break;
                case ControlsEnum.FCRLIST:
                    CurrPK = 0;
                    txtExchngRate.Text = string.Empty;
                    txtDescription.Text = string.Empty;
                    txtBankcharge.Text = string.Empty;
                    ModifiedDatePnl.Visible = false;
                    LastModifiedTime = DateTime.Now;
                    //ResetForm(ControlsEnum.ADDITEM);
                    base.WkfRefID = ucrWrkf.RefID = 0;
                    break;
                case ControlsEnum.clearAdvSearch:
                    txtFromDate.Text = string.Empty;
                    txtToDate.Text = string.Empty;
                    ddlHold.ClearSelection();
                    txtVoucherNumber.Text = "Select/Type";
                    hdfFCRPK.Value = string.Empty;
                    ddlStatus.ClearSelection();
                    txtVno.Text = string.Empty;
                    break;
                //case ControlsEnum.ADDITEM:
                //    //ddlType.ClearSelection();
                //    anchorFile.Visible = false;
                //    vrfFileUpload.Enabled = true;
                //    CurrSlNo = 0;
                //    anchorFile.Attributes.Remove("onclick");
                //    break;
            }
        }

        //public double StringToFormula(string expression)
        //{
        //    //List<string> tokens = getTokens(expression);
        //    Stack<double> operandStack = new Stack<double>();
        //    //Stack<string> operatorStack = new Stack<string>();
        //    //int tokenIndex = 0;

        //    //while (tokenIndex < tokens.Count)
        //    //{
        //    //    string token = tokens[tokenIndex];
        //    //    if (token == "(")
        //    //    {
        //    //        string subExpr = getSubExpression(tokens, ref tokenIndex);
        //    //        operandStack.Push(StringToFormula(subExpr));
        //    //        continue;
        //    //    }
        //    //    if (token == ")")
        //    //    {
        //    //        throw new ArgumentException("Mis-matched parentheses in expression");
        //    //    }
        //    //    //If this is an operator  
        //    //    if (Array.IndexOf(_operators, token) >= 0)
        //    //    {
        //    //        while (operatorStack.Count > 0 && Array.IndexOf(_operators, token) < Array.IndexOf(_operators, operatorStack.Peek()))
        //    //        {
        //    //            string op = operatorStack.Pop();
        //    //            double arg2 = operandStack.Pop();
        //    //            double arg1 = operandStack.Pop();
        //    //            operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
        //    //        }
        //    //        operatorStack.Push(token);
        //    //    }
        //    //    else
        //    //    {
        //    //        operandStack.Push(double.Parse(token));
        //    //    }
        //    //    tokenIndex += 1;
        //    //}

        //    //while (operatorStack.Count > 0)
        //    //{
        //    //    string op = operatorStack.Pop();
        //    //    double arg2 = operandStack.Pop();
        //    //    double arg1 = operandStack.Pop();
        //    //    operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
        //    //}
        //    return operandStack.Pop();
        //}

        //private string getSubExpression(List<string> tokens, ref int index)
        //{
        //    StringBuilder subExpr = new StringBuilder();
        //    int parenlevels = 1;
        //    index += 1;
        //    while (index < tokens.Count && parenlevels > 0)
        //    {
        //        string token = tokens[index];
        //        if (tokens[index] == "(")
        //        {
        //            parenlevels += 1;
        //        }

        //        if (tokens[index] == ")")
        //        {
        //            parenlevels -= 1;
        //        }

        //        if (parenlevels > 0)
        //        {
        //            subExpr.Append(token);
        //        }

        //        index += 1;
        //    }

        //    if ((parenlevels > 0))
        //    {
        //        throw new ArgumentException("Mis-matched parentheses in expression");
        //    }
        //    return subExpr.ToString();
        //}

        //private List<string> getTokens(string expression)
        //{
        //    string operators = "()^*/+-";
        //    List<string> tokens = new List<string>();
        //    StringBuilder sb = new StringBuilder();

        //    foreach (char c in expression.Replace(" ", string.Empty))
        //    {
        //        if (operators.IndexOf(c) >= 0)
        //        {
        //            if ((sb.Length > 0))
        //            {
        //                tokens.Add(sb.ToString());
        //                sb.Length = 0;
        //            }
        //            tokens.Add(c.ToString());
        //        }
        //        else
        //        {
        //            sb.Append(c);
        //        }
        //    }

        //    if ((sb.Length > 0))
        //    {
        //        tokens.Add(sb.ToString());
        //    }
        //    return tokens;
        //}

        //private void SetDetailTax(object sender)
        //{
        //    TextBox txtAmount;
        //    TextBox txtDiscount;
        //    TextBox txtTax;
        //    TextBox txtTotal;

        //    TextBox txtRate;
        //    TextBox txtQuantity;

        //    Label lblOrderQuantity;
        //    Label lblInvQuantity;

        //    HiddenField hdfRRDPK;
        //    HiddenField hdfItemPK;

        //    double quantity;
        //    double rate;
        //    quantity = 0;
        //    rate = 0;

        //    double soQty = 0;
        //    double invdQty = 0;

        //    if (sender != null)
        //    {
        //        txtRate = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtRate") as TextBox);
        //        txtQuantity = sender as TextBox;
        //        lblOrderQuantity = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("lblOrderQuantity") as Label);
        //        lblInvQuantity = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("lblInvQuantity") as Label);

        //        txtAmount = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtAmount") as TextBox);
        //        if (txtRate != null && txtQuantity != null)
        //        {
        //            if (double.TryParse(txtRate.Text, out rate) && double.TryParse(txtQuantity.Text, out quantity))
        //            {
        //                if (!(double.TryParse(lblOrderQuantity.Text, out soQty) && double.TryParse(lblInvQuantity.Text, out invdQty) && soQty - invdQty >= rate))
        //                {
        //                    //txtQuantity.Text = CommonConstants.SELECT_VALUE_ZERO;
        //                    //rate = 0;
        //                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_InvQty").ToString() + "','" + Resources.ErpRes.Information + "');", true);
        //                }
        //                if (txtAmount != null)
        //                {
        //                    txtAmount.Text = txtAmount.ToolTip = Math.Round((rate * quantity), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
        //                    txtDiscount = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtDiscount") as TextBox);
        //                    txtTax = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtTax") as TextBox);
        //                    txtTotal = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtTotal") as TextBox);

        //                    hdfRRDPK = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("hdfInvoiceDtlPK") as HiddenField);
        //                    hdfItemPK = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
        //                    if (hdfRRDPK != null && hdfItemPK != null)
        //                    {
        //                        POInvoicePK = string.IsNullOrEmpty(hdfRRDPK.Value) ? 0 : Convert.ToInt32(hdfRRDPK.Value);
        //                        SelectedItemPK = string.IsNullOrEmpty(hdfItemPK.Value) ? 0 : Convert.ToInt32(hdfItemPK.Value);
        //                        SetDetailTax(txtAmount, txtDiscount, txtTax, txtTotal);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Rate_Greater_Discount").ToString() + "','" + Resources.ErpRes.Information + "');", true);
        //            }
        //        }
        //    }
        //    else if (POInvoicePK == 0 && SelectedItemPK == 0)
        //    {
        //        POInvoiceHeaderSession = TempPOInvoiceHeaderSession;
        //        foreach (GridViewRow gvr in grdInvoice.Rows)
        //        {
        //            if (gvr.RowType == DataControlRowType.DataRow)
        //            {
        //                hdfRRDPK = (gvr.FindControl("hdfInvoiceDtlPK") as HiddenField);
        //                hdfItemPK = (gvr.FindControl("hdfItemPK") as HiddenField);
        //                POInvoicePK = Convert.ToInt32(hdfRRDPK.Value);
        //                SelectedItemPK = Convert.ToInt32(hdfItemPK.Value);

        //                txtRate = (gvr.FindControl("txtRate") as TextBox);
        //                txtQuantity = (gvr.FindControl("txtInvNow") as TextBox);
        //                txtAmount = (gvr.FindControl("txtAmount") as TextBox);
        //                txtDiscount = (gvr.FindControl("txtDiscount") as TextBox);
        //                txtTax = (gvr.FindControl("txtTax") as TextBox);
        //                txtTotal = (gvr.FindControl("txtTotal") as TextBox);

        //                if (txtRate != null && txtQuantity != null && txtAmount != null
        //                    && double.TryParse(txtRate.Text, out rate) && double.TryParse(txtQuantity.Text, out quantity))
        //                {
        //                    txtAmount.Text = txtAmount.ToolTip = Math.Round((rate * quantity), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
        //                }
        //                SetDetailTax(txtAmount, txtDiscount, txtTax, txtTotal);
        //            }
        //        }
        //        POInvoicePK = 0;
        //        SelectedItemPK = 0;
        //    }
        //    else
        //    {
        //        POInvoiceHeaderSession = TempPOInvoiceHeaderSession;
        //        foreach (GridViewRow gvr in grdInvoice.Rows)
        //        {
        //            if (gvr.RowType == DataControlRowType.DataRow)
        //            {
        //                hdfRRDPK = (gvr.FindControl("hdfInvoiceDtlPK") as HiddenField);
        //                hdfItemPK = (gvr.FindControl("hdfItemPK") as HiddenField);
        //                if (POInvoicePK == Convert.ToInt32(hdfRRDPK.Value) && SelectedItemPK == Convert.ToInt32(hdfItemPK.Value))
        //                {
        //                    txtAmount = (gvr.FindControl("txtAmount") as TextBox);
        //                    txtDiscount = (gvr.FindControl("txtDiscount") as TextBox);
        //                    txtTax = (gvr.FindControl("txtTax") as TextBox);
        //                    txtTotal = (gvr.FindControl("txtTotal") as TextBox);
        //                    if (!SetDetailTax(txtAmount, txtDiscount, txtTax, txtTotal))
        //                        return;
        //                }
        //            }
        //        }
        //    }
        //    SetSubTotal();
        //}
        //private bool SetDetailTax(TextBox txtAmount, TextBox txtDiscount, TextBox txtTax, TextBox txtTotal)
        //{
        //    double amount;
        //    double discount;
        //    double itmTax;
        //    double netAmount;
        //    amount = 0;
        //    discount = 0;
        //    netAmount = 0;
        //    itmTax = 0;
        //    if (txtAmount != null && txtDiscount != null && txtTax != null && txtTotal != null)
        //    {
        //        Double.TryParse(txtAmount.Text.Trim(), out amount);
        //        if (amount >= 0)
        //        {
        //            if (POInvoiceHeaderSession != null)
        //            {
        //                invoiceHeaderObj = POInvoiceHeaderSession;
        //                soInvoiceDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == POInvoicePK && rfq.VID_ITEM == SelectedItemPK);
        //                if (soInvoiceDetailsObj != null)
        //                {
        //                    var discDetail = soInvoiceDetailsObj.TaxDtl.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Discount));
        //                    foreach (POInvoiceTaxHdr rfqTaxHdrObj in discDetail)
        //                    {
        //                        string taxFormula = rfqTaxHdrObj.VTL_TAX_FORMULA;
        //                        if (!string.IsNullOrEmpty(taxFormula))
        //                        {
        //                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
        //                            rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
        //                        }
        //                    }
        //                    discount = soInvoiceDetailsObj.TaxDtl.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(rfq => rfq.VTL_TAX_AMT);
        //                    netAmount = amount - discount;
        //                    txtDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);

        //                    var taxDetail = soInvoiceDetailsObj.TaxDtl.Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Tax));
        //                    foreach (POInvoiceTaxHdr rfqTaxHdrObj in taxDetail)
        //                    {
        //                        string taxFormula = rfqTaxHdrObj.VTL_TAX_FORMULA;
        //                        if (!string.IsNullOrEmpty(taxFormula))
        //                        {
        //                            taxFormula = taxFormula.Replace("#SUBTOTAL#", netAmount.ToString());
        //                            rfqTaxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
        //                        }
        //                    }
        //                    itmTax = soInvoiceDetailsObj.TaxDtl.ToList().Where(rfq => rfq.VTL_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(rfq => rfq.VTL_TAX_AMT);
        //                    txtTax.Text = itmTax.ToString(hdfCurrencyFormat.Value);
        //                    soInvoiceDetailsObj.VID_AMOUNT = amount;
        //                    soInvoiceDetailsObj.VID_DISCOUNT = discount;
        //                    soInvoiceDetailsObj.VID_TAX = itmTax;
        //                    soInvoiceDetailsObj.VID_NET_AMOUNT = (amount - discount + itmTax);
        //                    txtTotal.Text = soInvoiceDetailsObj.VID_NET_AMOUNT.ToString(hdfCurrencyFormat.Value);
        //                    POInvoiceHeaderSession = invoiceHeaderObj;
        //                }
        //            }
        //            return true;
        //        }
        //        else
        //        {
        //            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Rate_Greater_Discount").ToString() + "','" + Resources.ErpRes.Information + "');", true);
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        private void setvisibility(ActionsEnum ActionsEnum)
        {
            switch (ActionsEnum)
            {

                #region Company
                case ActionsEnum.EDIT:
                    ddlHoldAccount.Enabled = false;
                    ddlCurrency.Enabled = false;
                    txtFCDate.Enabled = false;
                    break;
                #endregion
                #region Company
                case ActionsEnum.VIEW:
                    ddlHoldAccount.Enabled = false;
                    ddlCurrency.Enabled = false;
                    txtFCDate.Enabled = false;
                    txtExchngRate.Enabled = false;

                    foreach (GridViewRow grdRow in grdFcHoldDetasils.Rows)
                    {
                        TextBox txtReverseNow = grdRow.FindControl("txtReverseNow") as TextBox;
                        if (txtReverseNow != null)
                        {
                            txtReverseNow.Enabled = false;
                        }
                    }
                    ddlBankCharge.Enabled = false;
                    txtBankcharge.Enabled = false;
                    txtDescription.Enabled = false;
                    break;
                #endregion
                #region New
                case ActionsEnum.NEW:
                    ddlHoldAccount.Enabled = true;
                    ddlCurrency.Enabled = true;
                    txtFCDate.Enabled = true;
                    txtExchngRate.Enabled = true;

                    foreach (GridViewRow grdRow in grdFcHoldDetasils.Rows)
                    {
                        TextBox txtReverseNow = grdRow.FindControl("txtReverseNow") as TextBox;
                        if (txtReverseNow != null)
                        {
                            txtReverseNow.Enabled = true;
                        }
                    }
                    ddlBankCharge.Enabled = true;
                    txtBankcharge.Enabled = true;
                    txtDescription.Enabled = true;
                    break;
                #endregion
            }
        }

        //private void SetSubTotal()
        //{
        //    TextBox txtSubTotalFooter;
        //    if (grdInvoice.FooterRow != null)
        //    {
        //        txtSubTotalFooter = grdInvoice.FooterRow.FindControl("txtSubTotalFooter") as TextBox;
        //        if (txtSubTotalFooter != null && POInvoiceHeaderSession != null)
        //        {
        //            POInvoiceHeaderSession.IVH_AMOUNT_TC = POInvoiceHeaderSession.OrderDetail.Sum(dtl => dtl.VID_NET_AMOUNT);
        //            txtSubTotalFooter.ToolTip = txtSubTotalFooter.Text = POInvoiceHeaderSession.IVH_AMOUNT_TC.ToString(hdfCurrencyFormat.Value);
        //            decimal subTotal = Convert.ToDecimal(txtSubTotalFooter.Text);
        //            decimal hdrDiscount = string.IsNullOrEmpty(txtHdrDiscount.Text) ? 0 : Convert.ToDecimal(txtHdrDiscount.Text);
        //            double totalTax = 0;
        //            double tax;
        //            foreach (GridViewRow grdRow in grdInvoice.Rows)
        //            {
        //                TextBox txtTax = grdRow.FindControl("txtTax") as TextBox;
        //                if (txtTax != null)
        //                {
        //                    tax = 0;
        //                    Double.TryParse(txtTax.Text, out tax);
        //                    totalTax += tax;
        //                }
        //            }
        //            subTotal -= Convert.ToDecimal(totalTax);
        //            txtHdrTotal.Text = (subTotal - hdrDiscount).ToString(hdfCurrencyFormat.Value);
        //            decimal hdrDeduction = string.IsNullOrEmpty(txtHdrDeduction.Text) ? 0 : Convert.ToDecimal(txtHdrDeduction.Text);
        //            txtHdrBalBeforeVat.Text = ((subTotal - hdrDiscount) - hdrDeduction).ToString(hdfCurrencyFormat.Value);
        //        }
        //    }
        //}

        ///// <summary>
        ///// I exist Po in list
        ///// </summary>
        ///// <param name="lst"></param>
        ///// <param name="pk"></param>
        ///// <returns></returns>
        //private bool IsExixtPk(List<long> lst, long pk)
        //{
        //    bool flag = false;
        //    if (lst != null)
        //        foreach (long item in lst)
        //            if (item == pk)
        //            {
        //                flag = true;
        //                break;
        //            }
        //    return flag;
        //}
        //private double CalculateTaxFormula(string taxFormula, double amount)
        //{
        //    double taxAmt;
        //    taxAmt = 0;
        //    if (!string.IsNullOrEmpty(taxFormula))
        //    {
        //        taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
        //        taxAmt = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
        //    }
        //    return taxAmt;
        //}
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
        public string GetFormattedRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfRateFormat.Value);
        }
        public string GetFormattedExchangeRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfExchangeRateFormat.Value);
        }
        #endregion
        #region WorkFlow Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private void FillddlBankCharge()
        {
            GetFieldValues(ControlsEnum.BANKCURRENCYBASE);

            ddlBankCharge.Items.Clear();
            if (ddlCurrency.Items.Count > 0)
                ddlBankCharge.Items.Insert(0, (new ListItem(admCurrencyMstList[0].CUR_CODE, admCurrencyMstList[0].CUR_PK.ToString())));
            if (currentUser.BaseCurrency.ToString().Trim() != ddlCurrency.SelectedValue)
                ddlBankCharge.Items.Insert(1, new ListItem(ddlCurrency.SelectedItem.Text, ddlCurrency.SelectedValue));
            ddlHoldAccount.DataBind();

        }

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

        private string GetUrl()
        {
            string path = string.Empty;
            //// if one page containes two process (pageurl?PID=1,pageurl?PID=2)
            //if (Request.QueryString[QueryStrings.PID] == null)
            //{
            //    if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
            //        path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?PID=1";
            //    else
            //        path = Request.Url.AbsolutePath.ToLower() + "?PID=1";
            //}
            //else
            //{
            //    if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
            //        path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            //    else
            //        path = Request.Url.AbsolutePath.ToLower();
            //}
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
            try
            {
                int? result;
                bool bIsChecked = false;

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

                            decimal amt = 0;
                            foreach (GridViewRow grdrow in grdFcHoldDetasils.Rows)
                            {
                                TextBox txtReverseNoww;

                                txtReverseNoww = (TextBox)grdrow.FindControl("txtReverseNow");
                                amt = amt + Convert.ToDecimal(txtReverseNoww.Text);
                            }
                            if (amt <= 0)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_msgRevamt").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                break;
                            }


                            FCHoldReverseObj = new FCHoldReverseBO();
                            FCHoldReverseObj = (FCHoldReverseBO)SetUIValuesToObject(ControlsEnum.FCRDETAILS);
                            if (FCHoldReverseObj != null && FCHoldReverseObj.DetailsList != null)
                            {
                                string xmlDoc = CommonFunctions.XmlSerialize<FCHoldReverseBO>(FCHoldReverseObj);//CommonFunctions.ObjectTOXml(rfqResponseHeaderObj);
                                // save Process Control inspection details
                                result = Convert.ToInt32(BusinessLogic.Finance.FCReverseBL.SaveFCHoldRevertDetails(xmlDoc));
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
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.FCReverse);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);

                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.clearAdvSearch);
                                    ResetForm(ControlsEnum.FCRLIST);
                                    GetFieldValues(ControlsEnum.FCRLIST);
                                    SetFieldValues(ControlsEnum.FCRLIST);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }


                        }
                        break;
                    #endregion
                    #region Get FC Hold Revert Details
                    case ActionsEnum.FCHOLDREVERTDTL:

                        //GetFieldValues(ControlsEnum.FCREXCHANGERATEINBASECURRENCY);
                        //SetFieldValues(ControlsEnum.FCREXCHANGERATEINBASECURRENCY);
                        GetFieldValues(ControlsEnum.CURRENCY);
                        SetFieldValues(ControlsEnum.CURRENCY);
                        GetFieldValues(ControlsEnum.FCHOLDREVERTDTL);
                        SetFieldValues(ControlsEnum.FCHOLDREVERTDTL);
                        GetFieldValues(ControlsEnum.FCREXCHANGERATEINBASECURRENCY);
                        SetFieldValues(ControlsEnum.FCREXCHANGERATEINBASECURRENCY);
                        //ddlCurrency.SelectedValue = CommonConstants.SELECTVAL;
                        FillddlBankCharge();
                        break;
                    #endregion
                    #region Bank Currency
                    case ActionsEnum.BANKCURRENCY:
                        GetFieldValues(ControlsEnum.BANKCURRENCY);
                        SetFieldValues(ControlsEnum.BANKCURRENCY);
                        GetFieldValues(ControlsEnum.FCREXCHANGERATEINBASECURRENCY);
                        SetFieldValues(ControlsEnum.FCREXCHANGERATEINBASECURRENCY);
                        GetFieldValues(ControlsEnum.FCHOLDREVERTDTL);
                        SetFieldValues(ControlsEnum.FCHOLDREVERTDTL);
                        FillddlBankCharge();
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.FCRLIST);
                        SetFieldValues(ControlsEnum.FCRLIST);
                        break;
                    #endregion
                    #region Invoice List
                    case ActionsEnum.LIST:
                        FillProcessID(1);
                        CurrPK = 0;
                        ResetForm(ControlsEnum.FCRLIST);
                        GetFieldValues(ControlsEnum.FCRLIST);
                        SetFieldValues(ControlsEnum.FCRLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Invoice Details
                    case ActionsEnum.DETAILS:
                        foreach (GridViewRow grdrow in grdFCRList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfHRHpk")).Value);
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
                            GetFieldValues(ControlsEnum.FCRHEADER);
                            SetFieldValues(ControlsEnum.FCRHEADER);
                            GetFieldValues(ControlsEnum.FCRDETAILGRID);
                            SetFieldValues(ControlsEnum.FCRDETAILGRID);

                            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;

                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        ResetForm(ControlsEnum.NEW);
                        setvisibility(ActionsEnum.NEW);
                        SetUIEditView(commonActions);
                        EntryStatus = EntryStatus.NEWMODE;
                        FillProcessID(1);
                        base.WkfRefID = ucrWrkf.RefID = 0;
                        ucrWrkf.ApplicationID = 0;
                        ucrWrkf.ViewType = 1;
                        SetCancelRef(CurrPK);
                                                ucrWrkf.FillWorkFlowDetails();
                        ddlBankCharge.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    #endregion
                    #region View
                    case ActionsEnum.VIEW:
                        setvisibility(ActionsEnum.VIEW);
                        foreach (GridViewRow grdrow in grdFCRList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfHRHpk")).Value);
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
                            GetFieldValues(ControlsEnum.FCRHEADER);
                            SetFieldValues(ControlsEnum.FCRHEADER);
                            GetFieldValues(ControlsEnum.FCRDETAILGRID);
                            SetFieldValues(ControlsEnum.FCRDETAILGRID);
                            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;

                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Edit
                    case ActionsEnum.EDIT:
                        setvisibility(ActionsEnum.EDIT);
                        foreach (GridViewRow grdrow in grdFCRList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfHRHpk")).Value);
                                //Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                                //Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).Text;
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                //Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                ////
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
                            GetFieldValues(ControlsEnum.FCRHEADER);
                            SetFieldValues(ControlsEnum.FCRHEADER);
                            GetFieldValues(ControlsEnum.FCRDETAILGRID);
                            SetFieldValues(ControlsEnum.FCRDETAILGRID);

                            TempPOInvoiceHeaderSession = POInvoiceHeaderSession;


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
                        decimal amts = 0;
                        foreach (GridViewRow grdrow in grdFcHoldDetasils.Rows)
                        {
                            TextBox txtReverseNoww;

                            txtReverseNoww = (TextBox)grdrow.FindControl("txtReverseNow");
                            amts = amts + Convert.ToDecimal(txtReverseNoww.Text);
                        }
                        if (amts <= 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                          "ClosePopup();", true);
                            litErrorMsg.Text = GetLocalResourceObject("Err_msgRevamt").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            break;

                        }
                        //Show WorkFlow Popup

                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);

                        break;
                    #endregion
                    #region Submit
                    case ActionsEnum.SUBMIT:
                        amts = 0;
                        foreach (GridViewRow grdrow in grdFcHoldDetasils.Rows)
                        {
                            TextBox txtReverseNoww;

                            txtReverseNoww = (TextBox)grdrow.FindControl("txtReverseNow");
                            amts = amts + Convert.ToDecimal(txtReverseNoww.Text);
                        }
                        if (amts <= 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                          "ClosePopup();", true);
                            litErrorMsg.Text = GetLocalResourceObject("Err_msgRevamt").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            break;

                        }
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

                            hasValidRate = false;
                            FCHoldReverseObj = new FCHoldReverseBO();
                            FCHoldReverseObj = (FCHoldReverseBO)SetUIValuesToObject(ControlsEnum.FCRDETAILS);
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                if (hdfExchangeRate.Value != "-1")
                                {
                                    FCHoldReverseObj.WkfFlag = 1;
                                    //if (hasValidRate)
                                    //{

                                    if (FCHoldReverseObj != null && FCHoldReverseObj.DetailsList != null)
                                    {
                                        string xmlDoc = CommonFunctions.XmlSerialize<FCHoldReverseBO>(FCHoldReverseObj);//CommonFunctions.ObjectTOXml(rfqResponseHeaderObj);
                                        // save Process Control inspection details
                                        result = Convert.ToInt32(BusinessLogic.Finance.FCReverseBL.SaveFCHoldRevertDetails(xmlDoc));
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
                                    //}
                                    //else
                                    //{
                                    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Empty_Rate").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                    //    return;
                                    //}
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
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(CurrPK, ApplicationType.FCHR))
                                {
                                    ucrWrkf.ApplicationID = CurrPK;
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_FCR_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        FillProcessID(1);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.FCRLIST);
                                    GetFieldValues(ControlsEnum.FCRLIST);
                                    SetFieldValues(ControlsEnum.FCRLIST);
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
                                        if (string.IsNullOrEmpty(lblFcReversalNo.Text.Trim())
                                            || lblFcReversalNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                                        {
                                            CurrPK = ucrWrkf.ApplicationID;
                                            GetFieldValues(ControlsEnum.FCRHEADER);
                                            invoiceNo = dsPageData.Tables[0].Rows[0]["HRH_NO"].ToString(); //Select voucher no in msg box
                                        }
                                        else
                                        {
                                            invoiceNo = lblFcReversalNo.Text.Trim();
                                        }
                                        object[] args = new object[2];
                                        args[0] = Resources.PageNameRes.FCReverse;
                                        args[1] = invoiceNo;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);


                                        //Show Save success message and reset Contract Entry
                                        //litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                                        //litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.FCReverse);

                                        if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                        {
                                            ResetForm(ControlsEnum.FCRLIST);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                               + "','" + Resources.ErpRes.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                            ResetForm(ControlsEnum.FCRLIST);
                                            GetFieldValues(ControlsEnum.FCRLIST);
                                            SetFieldValues(ControlsEnum.FCRLIST);
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
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.FCReverse);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup3", "ClosePopup();", true);
                        }
                        break;
                    #endregion
                    #region ITEMSELECTED
                    case ActionsEnum.ITEMSELECTED:
                        foreach (GridViewRow grdrow in grdFCRList.Rows)
                        {
                            RadioButton rbtn;
                            HiddenField hdfDept;
                            int selectedPK;
                            int dept;

                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                selectedPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfHRHpk")).Value);


                                if (Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1)
                                {
                                    btnSave.Visible = false;
                                    btnEditforCancel.Visible = false;
                                    hdfIsCancelled.Value = "1";
                                }
                                else
                                {
                                    btnSave.Visible = true;
                                    btnEditforCancel.Visible = true;
                                    hdfIsCancelled.Value = "0";
                                }

                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                base.WkfRefID = workflowCore.GetRefID(selectedPK, PageProcessID);
                                break;
                            }
                        }
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        FillProcessID(1);
                        ResetForm(ControlsEnum.NEW);
                        GetFieldValues(ControlsEnum.FCRLIST);
                        SetFieldValues(ControlsEnum.FCRLIST);
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
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
                        ResetForm(ControlsEnum.FCRLIST);
                        GetFieldValues(ControlsEnum.FCRLIST);
                        SetFieldValues(ControlsEnum.FCRLIST);
                        break;
                    #endregion
                    #region Journalize Save
                    case ActionsEnum.JOURNALIZESAVE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ((Button)sender).CommandName = ActionsEnum.SAVE.ToString();
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.FCRLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.FCRLIST);
                        SetFieldValues(ControlsEnum.FCRLIST);
                        break;
                    #endregion
                    #region Journalize Submit
                    case ActionsEnum.JOURNALIZESUBMIT:
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                            if (Transaction == "SAVE")
                            {
                                //salesInvoiceServiceClient = new SalesInvoiceService();
                                //salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                                //result = (int)salesInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);
                            }
                            else if (Transaction == "DELETE")
                            {
                                //poInvoiceServiceClient = new POInvoiceService();
                                //poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                                //result = (int)poInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                            }
                        }
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.FCRLIST);

                        //if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ReferanceID > 0)
                        //{
                        //    Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        //    Response.Redirect(Resources.PageURL.InboxURL);
                        //}
                        //else
                        //{
                            EntryStatus = EntryStatus.LISTMODE;
                            Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                            GetFieldValues(ControlsEnum.FCRLIST);
                            SetFieldValues(ControlsEnum.FCRLIST);
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
                                //poInvoiceServiceClient = new POInvoiceService();
                                //poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                                //result = (int)poInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);
                            }
                            else if (Transaction == "DELETE")
                            {
                                //poInvoiceServiceClient = new POInvoiceService();
                                //poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                                //result = (int)poInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                            }
                        }
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.FCRLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.FCRLIST);
                        SetFieldValues(ControlsEnum.FCRLIST);
                        break;
                    #endregion
                    #region Journalize Cancel
                    case ActionsEnum.JOURNALIZECANCEL:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);                                    
                        ResetForm(ControlsEnum.FCRLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.FCRLIST);
                        SetFieldValues(ControlsEnum.FCRLIST);
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = BusinessLogic.Finance.FCReverseBL.DeleteFCHoldRevertDetails(CurrPK, LastModifiedTime);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.FCReverse);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm(ControlsEnum.FCRLIST);
                                GetFieldValues(ControlsEnum.FCRLIST);
                                SetFieldValues(ControlsEnum.FCRLIST);
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
                                    litErrorMsg.Text = Resources.PageNameRes.FCReverse + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.FCRLIST);
                                    GetFieldValues(ControlsEnum.FCRLIST);
                                    SetFieldValues(ControlsEnum.FCRLIST);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.FCReverse + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.FCReverse + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.FCRLIST);
                                    GetFieldValues(ControlsEnum.FCRLIST);
                                    SetFieldValues(ControlsEnum.FCRLIST);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.FCReverse);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region CHECK AMT in detail Grid
                    case ActionsEnum.CHECKAMT:
                        bool isValidQty = true;
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
                        if (grdFcHoldDetasils.Rows.Count > 0 || grdFcHoldDetasils != null)
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
                                //else
                                //{ litErrorMsg.Text = GetLocalResourceObject("Msg_InvoiceQty_Zero").ToString(); }

                            }
                        }




                        break;
                    #endregion
                    #region PRINT
                    case ActionsEnum.PRINT:
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.FCHR + "&APPSUBTYPE=1") + "');", true);

                        }
                        break;
                    #endregion
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdFCRList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfHRHpk")).Value);
                                //Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                //Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomer")).Text;
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                int hdfPosted = Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                Posted = Convert.ToBoolean(hdfPosted);
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
                            GetFieldValues(ControlsEnum.FCRHEADER);
                            SetFieldValues(ControlsEnum.FCRHEADER);
                            GetFieldValues(ControlsEnum.FCRDETAILGRID);
                            SetFieldValues(ControlsEnum.FCRDETAILGRID);


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
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.clearAdvSearch);
                        GetFieldValues(ControlsEnum.FCRLIST);
                        SetFieldValues(ControlsEnum.FCRLIST);
                        break;
                    #endregion
                    #region SHOWPOPUP -- Receipt No Print
                    case ActionsEnum.SHOWPOPUP:
                        if (((LinkButton)sender).CommandArgument != null)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" +
                                ((LinkButton)sender).CommandArgument + "&APPTYPE=" + ApplicationType.CRJ + "&APPSUBTYPE=1&TRXTYPE=" + ApplicationType.CRJ + "');", true);
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
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                   //"ShowContainerDiv('[id$=divJournalize]','xyz','1000','550');", true);
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
                //if (((GridView)sender).ID == "grdFCRList")
                //{
                //    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                //    {
                //        Button imgPosted = e.Row.FindControl("imgPosted") as Button;

                //        HiddenField hdfPosted = e.Row.FindControl("hdfPosted") as HiddenField;

                //        if (hdfPosted.Value == "1")
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
            GetFieldValues(ControlsEnum.FCRLIST);
            SetFieldValues(ControlsEnum.FCRLIST);
            EntryStatus = EntryStatus.LISTMODE;
        }
        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            //try
            //{
            //    //if (SortBy == e.SortExpression)
            //    //{
            //    //    //Toggle the sort expression
            //    //    if (SortDirection == Resources.Report.SortAscending)
            //    //        SortDirection = Resources.Report.SortDescending;
            //    //    else
            //    //        SortDirection = Resources.Report.SortAscending;
            //    //}
            //    //else
            //    //{
            //    //    SortBy = e.SortExpression;
            //    //    SortDirection = Resources.Report.SortAscending;

            //    //}
            //    //this.PageIndex = "1";
            //    ////GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
            //    ////SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
            //    //EntryStatus = EntryStatus.LISTMODE;
            //    SortBy = e.SortExpression;
            //    if (SortDirection == Resources.Report.SortAscending)
            //        SortDirection = Resources.Report.SortDescending;
            //    else
            //        SortDirection = Resources.Report.SortAscending;
            //    GetFieldValues(ControlsEnum.INVOICELIST);
            //    SetFieldValues(ControlsEnum.INVOICELIST);
            //    EntryStatus = EntryStatus.LISTMODE;
            //}
            //catch (Exception ex)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            //}
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
            btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnSave.PreRender += new EventHandler(btnAction_PreRender);
            btnNew.PreRender += new EventHandler(btnAction_PreRender);
            btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            btnJournalize.PreRender += new EventHandler(btnAction_PreRender);
            btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            btnView.PreRender += new EventHandler(btnAction_PreRender);
            lnkList.PreRender += new EventHandler(btnAction_PreRender);
            lnkDetail.PreRender += new EventHandler(btnAction_PreRender);
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);


            btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            btnSubmit.Load += new EventHandler(btnAction_Load);
            btnSave.Load += new EventHandler(btnAction_Load);
            btnCancel.Load += new EventHandler(btnAction_Load);
            btnNew.Load += new EventHandler(btnAction_Load);
            btnDelete.Load += new EventHandler(btnAction_Load);
            btnJournalize.Load += new EventHandler(btnAction_Load);
            btnPrint.Load += new EventHandler(btnAction_Load);
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
            FCRLIST,
            FCHold,
            POINVHEADER,
            POINVDETAIL,
            TAXTYPES,
            TAXPOPUPGRID,
            TAXHEADER,
            EXCHANGERATE,
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
            CURRENCY,
            HOLDACCOUNT,
            BANKCURRENCY,
            FCHOLDREVERTDTL,
            FCRDETAILS,
            FCREXCHANGERATEINBASECURRENCY,
            FCRBANKCURRENCY,
            FCRCURRENCYHOLD,
            FCRDETAILSEDIT,
            FCRHEADER,
            FCRDETAILGRID,
            BANKCURRENCYBASE,
            NEW,
            FCRVOUCHERLIST,
            clearAdvSearch,
            FINHEADERBYPK,
            FCREVERSEGET
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