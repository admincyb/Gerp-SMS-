using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject;
using BusinessObject.AccountManagement;
using ERPData;
using ERPManager;
using gComs.WorkFlow;
using ERPService;
using ERPService.Administration;
using BusinessObject.CommonManagement;
using System.Web.UI.HtmlControls;
using System.Threading;
using ERP.Store.UI;
using System.Data;
using BusinessObject.Journalize;
using ERP.Utilities.Validations;
using System.Drawing;
using System.Web.Security;


namespace ERPSMS_v01.Journalize.UserControls
{
    public partial class JournalizeControlNew : System.Web.UI.UserControl
    {
        #region EventHandlers
        public event EventHandler JournalizeSubmit;
        public event EventHandler JournalizeSave;
        public event EventHandler JournalizeDelete;
        public event EventHandler JournalizeCancel;

        public event EventHandler ReverseDelete;
        public event EventHandler ReverseSubmit;
        public event EventHandler ReverseSave;
        public event EventHandler ReverseCancel;

        public event EventHandler ReturnDelete;
        public event EventHandler ReturnSubmit;
        public event EventHandler ReturnSave;
        public event EventHandler ReturnCancel;
        #endregion
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// Gets or sets the transaction type eg: SIJ for sales invoice
        /// </summary>
        public string TransactionType
        {
            get
            {
                if (this.ViewState[ViewstateStrings.TransactionType] == null)
                {
                    return null;
                }
                else
                {
                    return this.ViewState[ViewstateStrings.TransactionType].ToString();
                }

            }
            set
            {
                this.ViewState[ViewstateStrings.TransactionType] = value;
            }
        }
        /// <summary>
        /// Gets or sets the Transaction PK
        /// </summary>
        public long TransactionPK
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.TransactionPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TransactionPK] = value;
            }
        }
        /// <summary>
        /// Journalize PK
        /// </summary>
        public int JournalizePK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.JournalizePK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.JournalizePK] = value;
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
        /// Current PK
        /// </summary>
        private int JurCurrPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.JurCurrPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.JurCurrPK] = value;
            }
        }
        /// <summary>
        /// JournalizeRefPK
        /// </summary>
        public int JournalizeRefPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.JournalizeRefPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.JournalizeRefPK] = value;
            }
        }
        /// <summary>
        /// Sets or gets the page HasWkfPermission
        /// </summary>
        public bool HasWkfPermission
        {
            get
            {
                return this.ViewState[ViewstateStrings.HasWkfPermission] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.HasWkfPermission]);
            }
            set
            {
                this.ViewState[ViewstateStrings.HasWkfPermission] = value;
            }
        }
        /// <summary>
        /// Gets or sets the TabIndex of Dr controls
        /// </summary>
        private short TabIndexDr
        {
            get
            {
                return Convert.ToInt16(this.ViewState[ViewstateStrings.TabIndexDr]) <= 0 ? Convert.ToInt16(100) : Convert.ToInt16(this.ViewState[ViewstateStrings.TabIndexDr]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TabIndexDr] = value;
            }
        }
        /// <summary>
        /// Gets or sets the TabIndex Cr Controls
        /// </summary>
        private short TabIndexCr
        {
            get
            {
                return Convert.ToInt16(this.ViewState[ViewstateStrings.TabIndexCr]) <= 0 ? Convert.ToInt16(300) : Convert.ToInt16(this.ViewState[ViewstateStrings.TabIndexCr]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TabIndexCr] = value;
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
        /// Gets or sets the JVTemplate BusinessClass Obj
        /// </summary>
        private JVTemplateBO JVTemplateBOObj
        {
            get
            {
                return (JVTemplateBO)this.ViewState[ERP.Utilities.SessionStrings.jvTemplateBOHeaderObj];
            }
            set
            {
                this.ViewState[ERP.Utilities.SessionStrings.jvTemplateBOHeaderObj] = value;
            }
        }
        /// <summary>
        /// Gets or sets the VoucherTemplatePK
        /// </summary>
        public int VoucherTemplatePK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.VoucherTemplatePK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.VoucherTemplatePK] = value;
            }
        }
        /// <summary>
        /// Check the currency is BaseCurrency or not
        /// </summary>
        public bool IsBaseCurrency
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsBaseCurrency] == null ? true : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsBaseCurrency]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsBaseCurrency] = value;
            }
        }
        /// <summary>
        /// Gets or sets the JournalType 1 is for voucher and 2 is for reverse voucher
        /// </summary>
        public int JournalType
        {
            get
            {
                return this.ViewState[ViewstateStrings.JournalType] == null ? (int)JournalTypeEnum.Voucher : Convert.ToInt32(this.ViewState[ViewstateStrings.JournalType]);
            }
            set
            {
                this.ViewState[ViewstateStrings.JournalType] = value;
            }
        }
        /// <summary>
        /// Gets or sets the G/L Calculation Mode 1 is Actual and 2 is DiffOfBC
        /// </summary>
        private byte GainLossCalculationMode
        {
            get
            {
                return this.ViewState[ViewstateStrings.GainLossCalculationMode] == null ? Convert.ToByte(1) : Convert.ToByte(this.ViewState[ViewstateStrings.GainLossCalculationMode]);
            }
            set
            {
                this.ViewState[ViewstateStrings.GainLossCalculationMode] = value;
            }
        }
        /// <summary>
        /// Gets or sets BCEnable
        /// </summary>
        private bool BCEnable
        {
            get
            {
                return this.ViewState[ViewstateStrings.BCEnable] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.BCEnable]);
            }
            set
            {
                this.ViewState[ViewstateStrings.BCEnable] = value;
            }
        }
        /// <summary>
        /// Gets or sets IsNewDummy
        /// </summary>
        private bool IsNewDummy
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsNewDummy] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsNewDummy]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsNewDummy] = value;
            }
        }
        /// <summary>
        /// Type For Voucher Number Generation
        /// </summary>
        public string TypeForNumberGenaration
        {
            get
            {
                if (this.ViewState[ViewstateStrings.TypeForNumberGenaration] != null)
                    return this.ViewState[ViewstateStrings.TypeForNumberGenaration].ToString();
                else
                    return null;
            }
            set
            {
                this.ViewState[ViewstateStrings.TypeForNumberGenaration] = value;
            }
        }
        /// <summary>
        /// Gets or sets CloseVoucherPopup
        /// </summary>
        public bool CloseVoucherPopup
        {
            get
            {
                return this.ViewState[ViewstateStrings.CloseVoucherPopup] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.CloseVoucherPopup]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CloseVoucherPopup] = value;
            }
        }
        /// <summary>
        /// Gets or sets VoucherDeleteStatus
        /// </summary>
        public bool VoucherDeleteStatus
        {
            get
            {
                return this.ViewState[ViewstateStrings.VoucherDeleteStatus] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.VoucherDeleteStatus]);
            }
            set
            {
                this.ViewState[ViewstateStrings.VoucherDeleteStatus] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDummyAdd
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsDummyAdd] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsDummyAdd]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsDummyAdd] = value;
            }
        }

        /// <summary>
        /// External User Department : 
        /// Set user department if transaction and voucher departments are different 
        /// </summary>
        public int ExternalUserDept
        {
            get
            {
                return (this.ViewState["ExternalUserDept"] == null ? 0 : (int)this.ViewState["ExternalUserDept"]);
            }
            set
            {
                this.ViewState["ExternalUserDept"] = value;
            }
        }
        /// <summary>
        /// Gets or sets Control Id(Journal account hiddenfield control id)
        /// </summary>
        public string ControlId
        {
            get
            {
                return this.ViewState[ViewstateStrings.ControlId] == null ? string.Empty : Convert.ToString(this.ViewState[ViewstateStrings.ControlId]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ControlId] = value;
            }
        }

        /// <summary>
        /// Gets or sets Control Id
        /// </summary>
        public List<CostCenterDetails> CostCenterList
        {
            get
            {
                return this.Session[ViewstateStrings.CostCenterList] == null ? new List<CostCenterDetails>() : (List<CostCenterDetails>)this.Session[ViewstateStrings.CostCenterList];
            }
            set
            {
                this.Session[ViewstateStrings.CostCenterList] = value;
            }
        }
        /// <summary>
        /// Gets or sets cost center split amount missmatch Control Id
        /// </summary>
        public List<CostCenterMissmatch> CCInvalidSplitControlList
        {
            get
            {
                return this.ViewState[ViewstateStrings.CCInvalidSplitControlList] == null ? new List<CostCenterMissmatch>() : (List<CostCenterMissmatch>)this.ViewState[ViewstateStrings.CCInvalidSplitControlList];
            }
            set
            {
                this.ViewState[ViewstateStrings.CCInvalidSplitControlList] = value;
            }
        }
        /// <summary>
        /// Gets or sets the selected accounts amount
        /// </summary>
        private decimal SelAccountCCAmount
        {
            get
            {
                return (this.ViewState["SelAccountCCAmount"] == null ? 0 : (decimal)this.ViewState["SelAccountCCAmount"]);
            }
            set
            {
                this.ViewState["SelAccountCCAmount"] = value;
            }
        }
        #endregion
        // Indicates the state as well as action
        private User currentUser;
        private ActionsEnum commonActions;
        private FIN_TRX finTrxObj;
        private FIN_TRX_HDR finTrxHdrObj;
        private FIN_COA_SUB_TYPE_CFG finCoaSubTypeCfgObj;
        private ADM_CONFIG_MST admConfigMstObj;
        private ServiceUtility serviceUtilityObj;
        private FIN_COA_MST finCoaMstObj;
        private ADM_APP_CONFIG_MST admAppConfigMstObj;
        private ADM_COMPANY_MST admCompanyMstObj;
        private ADM_DEPT_MST admDeptMstObj;

        private List<ADM_APP_CONFIG_MST> admAppConfigMstList;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        private List<ADM_DEPT_MST> admDeptMstList;
        private List<FIN_TRX> finTrxList;
        private List<FIN_COA_SUB_TYPE_CFG> finCoaSubTypeCfgList;
        private List<FIN_TRX_HDR> finTrxHdrList;
        private List<FIN_COA_MST> finCoaMstList;
        private List<ADM_CONST_MST> admTemplateCategoryList;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private List<CostCenterDetails> CostCenterTempList;
        private List<CostCenterMissmatch> CCInvalidSplitControlTempList;

        private int transactionCurrency;
        int AccountPk = 0;
        long VoucherPk;
        decimal CCAmount = 0;
        decimal CostCenterAmnt = 0;
        private string voucherNo;
        private bool updateVocher;
        private string journalScript = string.Empty;
        private WorkflowUserComments ucrWrkf;
        private VoucherGainLossHeader voucherGainLossObj;
        private DataSet dsVoucherTemplateList;
        private DataTable dtCompany;
        private DataTable dtExtDept;
        private DataTable dtCostCenter;
        private DataTable dtStatus;
        private DataTable dtAuditDetails;
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
            try
            {
                if (!IsPostBack)
                {
                    //Set decimal formats
                    hdfDecimalFormatVoucher.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
                    {
                        hdfDecimalFormatVoucher.Value += "0";
                    }
                    hdfCurrencyFormatVoucher.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormatVoucher.Value += "0";
                    }
                    hdfRateFormatVoucher.Value = "#0.";
                    int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit]));
                    for (int i = 0; i < rateDecimalDigits; i++)
                    {
                        hdfRateFormatVoucher.Value += "0";
                    }
                    hdfExchRateFormatVoucher.Value = "#0.";
                    int exchRateDecimalDigits = (Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
                    for (int i = 0; i < exchRateDecimalDigits; i++)
                    {
                        hdfExchRateFormatVoucher.Value += "0";
                    }
                    hdfNumberDigitsVoucher.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
                    hdfCurrencyDigitsVoucher.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    hdfExchRateDigitsVoucher.Value = exchRateDecimalDigits.ToString();

                    hdfJournalCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                    hdfJournalCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();

                    GetFieldValues(ControlsEnum.DISABLEROUND);
                    SetFiledValues(ControlsEnum.DISABLEROUND);

                }
                ValidatePageDept();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        /// 
        private void GetFieldValues(ControlsEnum type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            int exchRateDecimalDigits = (Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                       ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                       : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
            FinTrxService finTrxServiceClient;
            finTrxServiceClient = null;
            CommonService commonService;
            commonService = null;
            AccountMstService AccountMstServiceClient;
            AccountMstServiceClient = null;
            POInvoiceService poInvoiceServiceClient;
            poInvoiceServiceClient = null;
            CommonService commonServiceClient;
            AdmCompanyMstService admCompanyMstServiceClient;
            admCompanyMstServiceClient = null;
            AdmDeptMstService admDeptMstServiceClient;
            admDeptMstServiceClient = null;
            try
            {
                switch (type)
                {
                    #region FIN HEADER
                    case ControlsEnum.FINHEADER:
                        //Gets FinHeader Table Values
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_REF_TYPE = Session[ERP.Utilities.SessionStrings.TransactionType].ToString();
                        finTrxHdrObj.FTH_REF_PK = Session[ERP.Utilities.SessionStrings.TransactionPK] == null ? JurCurrPK :
                            Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString());
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrObj.FTH_IS_DELETED = VoucherDeleteStatus;
                        finTrxHdrObj.FTH_PK = Session[ERP.Utilities.SessionStrings.TrxPK] == null ? -1 : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TrxPK].ToString());
                        finTrxHdrList = finTrxServiceClient.GetfinTxtHdrList(finTrxHdrObj, serviceUtilityObj);
                        break;
                    #endregion
                    #region BASECURRENCY
                    case ControlsEnum.BASECURRENCY:
                        //Gets Base Currency
                        CurrencyMstService CurrencyMstServiceClient = new CurrencyMstService();
                        string BaseCurrency = CurrencyMstServiceClient.GetCurrencyCodeName(currentUser.BaseCurrency);
                        hdfJournalBaseCurrency.Value = BaseCurrency.Split('-')[0].Trim(); ;
                        break;
                    #endregion
                    #region FIN COA SUB TYPE CONFIG VALUES
                    case ControlsEnum.FINCOASUBTYPECFG:
                        //Gets FINCOASUBTYPECFG table values which stores the related query and default query to get the subledger for the selected Account
                        commonService = new CommonService();
                        commonService = CommonFunctions.InitiateClient(commonService);
                        finCoaSubTypeCfgObj = new FIN_COA_SUB_TYPE_CFG();
                        finCoaSubTypeCfgObj = CommonFunctions.Initilize<FIN_COA_SUB_TYPE_CFG>();
                        finCoaSubTypeCfgObj.CST_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finCoaSubTypeCfgObj.CST_PK = hdfSubTypePk.Value == string.Empty ? 0 : Convert.ToInt32(hdfSubTypePk.Value);
                        finCoaSubTypeCfgList = commonService.GetSubTypeCfgValues(finCoaSubTypeCfgObj);
                        break;
                    #endregion
                    #region FIN COA MST VALUES
                    case ControlsEnum.FINCOAMST:
                        //Gets SubtypePK for the selected Account
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        AccountMstServiceClient = new AccountMstService();
                        AccountMstServiceClient = CommonFunctions.InitiateClient(AccountMstServiceClient);
                        finCoaMstObj = new FIN_COA_MST();
                        finCoaMstObj = CommonFunctions.Initilize<FIN_COA_MST>();
                        finCoaMstObj.COA_PK = string.IsNullOrEmpty(hdfCoaPk.Value) ? 0 : Convert.ToInt32(hdfCoaPk.Value);
                        finCoaMstObj.COA_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finCoaMstList = AccountMstServiceClient.GetFinCoaMst(finCoaMstObj, serviceUtilityObj);
                        break;
                    #endregion
                    #region Generate Exchange Rate
                    case ControlsEnum.EXCHANGERATE:
                        //Gets the exchange rate for the selected currency
                        double ExchgRate = 0;
                        poInvoiceServiceClient = new POInvoiceService();
                        poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                        if (finTrxHdrObj != null)
                        {
                            ExchgRate = poInvoiceServiceClient.GetConversionFactor(finTrxHdrObj.FTH_TRX_CURR, finTrxHdrObj.FTH_BASE_CURR,
                                                                 (DateTime)finTrxHdrObj.FTH_DATE, currentUser.SBUID);
                        }
                        else
                        {
                            int toCurrency = string.IsNullOrEmpty(hdfJournalCurr.Value) ? currentUser.BaseCurrency : Convert.ToInt32(hdfJournalCurr.Value);
                            ExchgRate = poInvoiceServiceClient.GetConversionFactor(toCurrency, currentUser.BaseCurrency,
                                                                 Convert.ToDateTime(txtPVDate.Text), currentUser.SBUID);
                        }
                        hdfExchangeRateJV.Value = ERP.Utilities.CommonFunctions.DoubleFormat(ExchgRate, exchRateDecimalDigits).ToString();
                        break;
                    #endregion
                    #region Voucher No Generation
                    case ControlsEnum.VOUCHERNO:
                        //Generate voucher number
                        poInvoiceServiceClient = new POInvoiceService();
                        poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                        int appSubType;
                        if (Session[ERP.Utilities.SessionStrings.CrDrType] == null)
                        {

                            appSubType = !string.IsNullOrEmpty(TypeForNumberGenaration) ? Convert.ToInt32(TypeForNumberGenaration) : 0;
                            voucherNo = poInvoiceServiceClient.GetInvoiceNo(Session[ERP.Utilities.SessionStrings.JournalType].ToString(),
                                appSubType, 1, DateTime.Parse(txtPVDate.Text) == null ? DateTime.Now : DateTime.Parse(txtPVDate.Text), currentUser.PKUser, updateVocher, 0, Convert.ToInt32(ddlVoucherCompany.SelectedValue));
                        }
                        else
                        {
                            if (Session[ERP.Utilities.SessionStrings.CrDrType].ToString() == ApplicationType.PI)
                            {
                                appSubType = !string.IsNullOrEmpty(TypeForNumberGenaration) ? Convert.ToInt32(TypeForNumberGenaration) : 1;
                                voucherNo = poInvoiceServiceClient.GetInvoiceNo(Session[ERP.Utilities.SessionStrings.JournalType].ToString(),
                                    appSubType, 1, DateTime.Parse(txtPVDate.Text) == null ? DateTime.Now : DateTime.Parse(txtPVDate.Text), currentUser.PKUser, updateVocher, 0, Convert.ToInt32(ddlVoucherCompany.SelectedValue));
                            }
                            else if (Session[ERP.Utilities.SessionStrings.CrDrType].ToString() == ApplicationType.SI)
                            {
                                appSubType = !string.IsNullOrEmpty(TypeForNumberGenaration) ? Convert.ToInt32(TypeForNumberGenaration) : 2;
                                voucherNo = poInvoiceServiceClient.GetInvoiceNo(Session[ERP.Utilities.SessionStrings.JournalType].ToString(),
                                    appSubType, 1, DateTime.Parse(txtPVDate.Text) == null ? DateTime.Now : DateTime.Parse(txtPVDate.Text), currentUser.PKUser, updateVocher, 0, Convert.ToInt32(ddlVoucherCompany.SelectedValue));
                            }
                        }
                        hdfVoucherNo.Value = voucherNo;
                        break;
                    #endregion
                    #region PettyCashVoucherLimit
                    case ControlsEnum.PETTYCASHVOUCHERLIMIT:
                        //Gets the voucher limit for pettycash
                        commonServiceClient = new CommonService();
                        admAppConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_APP_CONFIG_MST>();
                        admAppConfigMstObj.ACF_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admAppConfigMstObj.ACF_SETTING = GetLocalResourceObject("PETTY_CASH_VOUCHER_LIMIT").ToString();
                        admAppConfigMstList = commonServiceClient.GetADM_APP_CONFIG_MST(admAppConfigMstObj);
                        break;
                    #endregion
                    #region FIN HEADER BY PK
                    case ControlsEnum.FINHEADERBYPK:
                        //Gets Fin_TRX_Hdr table values by PK
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrObj.FTH_PK = Session[ERP.Utilities.SessionStrings.TrxPK] == null ? JurCurrPK : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TrxPK].ToString());
                        finTrxHdrObj.FTH_IS_DELETED = false;
                        finTrxHdrList = finTrxServiceClient.GetfinTxtHdrListByPK(finTrxHdrObj);
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

                        //To get the company related to current SBU
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                        break;
                    #endregion
                    #region Department
                    case ControlsEnum.DEPARTMENT:
                        //Gets Department List
                        admDeptMstServiceClient = new AdmDeptMstService();
                        admDeptMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_DEPT_MST>();
                        admDeptMstObj.DPT_PK = currentUser.SBUID;
                        admDeptMstObj.DPT_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admDeptMstList = admDeptMstServiceClient.GetDepartmentList(admDeptMstObj, serviceUtilityObj);
                        break;
                    #endregion
                    #region Template Category
                    case ControlsEnum.TEMPLATECATEGORY:
                        //Gets Voucher Template Category
                        commonServiceClient = new CommonService();
                        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        admTemplateCategoryList = commonServiceClient.GetConstMstValues(null, Convert.ToByte(DbActiveStatus.ACTIVE), null, (int)ConstGroupType.VoucherTemplateType, ERP.Utilities.CommonConstants.VoucherTemplate, currentUser.SBUID);
                        break;
                    #endregion
                    #region Voucher Templates
                    case ControlsEnum.VOUCHERTEMPLATE:
                        //Gets Voucher Template List
                        dsVoucherTemplateList = BusinessLogic.Jouralize.JournalizeBL.GetVoucherTemplateDetails(VoucherTemplatePK);
                        break;
                    #endregion
                    #region G/L Calculation Mode
                    case ControlsEnum.CALCULATIONMODE:
                        //Gets G?L Calculation Mode
                        commonServiceClient = new CommonService();
                        admAppConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_APP_CONFIG_MST>();
                        admAppConfigMstObj.ACF_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admAppConfigMstObj.ACF_SETTING = GetLocalResourceObject("G/L_CALC_MODE").ToString();
                        admAppConfigMstList = commonServiceClient.GetADM_APP_CONFIG_MST(admAppConfigMstObj);
                        break;
                    #endregion
                    #region BC Enable/Disable
                    case ControlsEnum.BCENABLEDISABLE:
                        //Gets Base Currency is Enable or not
                        commonServiceClient = new CommonService();
                        admAppConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_APP_CONFIG_MST>();
                        admAppConfigMstObj.ACF_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admAppConfigMstObj.ACF_SETTING = GetLocalResourceObject("ENABLE_VOUCHER_BC_AMT").ToString();
                        admAppConfigMstList = commonServiceClient.GetADM_APP_CONFIG_MST(admAppConfigMstObj);
                        break;
                    #endregion
                    #region Round Enable/Disable
                    case ControlsEnum.DISABLEROUND:
                        //Gets Base Currency is Enable or not
                        commonServiceClient = new CommonService();
                        admAppConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_APP_CONFIG_MST>();
                        admAppConfigMstObj.ACF_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admAppConfigMstObj.ACF_SETTING = GetLocalResourceObject("VOUCHER_SETTINGS").ToString();
                        admAppConfigMstList = commonServiceClient.GetADM_APP_CONFIG_MST(admAppConfigMstObj);
                        break;
                    #endregion
                    #region EXT DEPARTMENT
                    case ControlsEnum.EXTDEPARTMENT:
                        dtExtDept = BusinessLogic.CommonManagement.CommonBL.GetDepartment(currentUser.SBUID, null, (int)DeptTypeEnum.FINANCE, (int)DbActiveStatus.ACTIVE);
                        break;
                    #endregion
                    #region COST CENTER
                    case ControlsEnum.COSTCENTER:
                        dtCostCenter = BusinessLogic.CommonManagement.CommonBL.GetCostCenter(AccountPk, CCAmount, Session[ERP.Utilities.SessionStrings.TransactionPK] == null ? JurCurrPK :
                            Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), JurCurrPK);
                        break;
                    #endregion
                    #region ALL COSTCENTERS
                    case ControlsEnum.ALLCOSTCENTERS:
                        dtCostCenter = BusinessLogic.CommonManagement.CommonBL.GetAllCostCenter(VoucherPk);
                        break;
                    #endregion
                    #region AUDIT LOG STATUS
                    case ControlsEnum.AUDITLOGSTATUS:
                        dtStatus = BusinessLogic.CommonManagement.CommonBL.GetAuditLogDisplayStatus(JurCurrPK, Session[ERP.Utilities.SessionStrings.TransactionType].ToString());
                        break;
                    #endregion
                    #region AUDIT LOG DETAILS
                    case ControlsEnum.AUDITLOGDETAILS:
                        dtAuditDetails = BusinessLogic.CommonManagement.CommonBL.GetAuditLogDetails(JurCurrPK);
                        break;
                        #endregion
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                finTrxServiceClient = null;
                commonService = null;
                AccountMstServiceClient = null;
                poInvoiceServiceClient = null;
                commonServiceClient = null;
                admCompanyMstServiceClient = null;
                admDeptMstServiceClient = null;

            }
        }
        #endregion
        #region SetFieldValues
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        private void SetFiledValues(ControlsEnum mode)
        {
            try
            {
                switch (mode)
                {
                    #region Company
                    case ControlsEnum.COMPANY:
                        //Bind Company List in DDL
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    #endregion
                    #region TEMPLATECATEGORY
                    case ControlsEnum.TEMPLATECATEGORY:
                        //Bind Voucher Template Category List in DDL
                        BindDropDown(ControlsEnum.TEMPLATECATEGORY);
                        break;
                    #endregion
                    #region Round Enable/Disable
                    case ControlsEnum.DISABLEROUND:
                        if (admAppConfigMstList != null && admAppConfigMstList.Count > 0)
                            hdfEnableRound.Value = admAppConfigMstList[0].ACF_VALUE.ToString();
                        break;
                    #endregion
                    #region COST CENTER
                    case ControlsEnum.COSTCENTER:
                        BindGrid(ControlsEnum.COSTCENTER);
                        break;
                    #endregion
                    #region ALL COST CENTERS
                    case ControlsEnum.ALLCOSTCENTERS:
                        GetUIValuesFromObject(ControlsEnum.ALLCOSTCENTERS);
                        break;
                        #endregion
                }
            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }
        #endregion

        #region ActionHandler
        /// <summary>
        /// Handling control events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            if (!ValidatePageDept())
                return;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            CommonService commonService;
            commonService = null;
            POInvoiceService poInvoiceServiceClient;
            poInvoiceServiceClient = null;
            FinTrxService finTrxServiceClient;
            finTrxServiceClient = null;

            CommonService CommonServiceClient;
            CommonServiceClient = null;
            try
            {
                int exchRateDecimalDigits = (Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                       ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                       : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
                Dictionary<string, string> dicDrControls;
                Dictionary<string, string> dicCrControls;
                Dictionary<string, string> dicControlinfo;
                Dictionary<string, string> dicAccountType;
                List<string> removedControlsList;
                string divColStyle = "";
                HtmlGenericControl div;//Used to store controls
                HtmlGenericControl divValidation;//Used to store validation controls
                HtmlGenericControl divClear;//Used to break the line
                Table tbControls = new Table();
                TableRow trControls = new TableRow();
                TableCell tcControl = new TableCell();
                Table table;

                string id;
                string group;
                long? jurResult = 0;
                string relquery = string.Empty;
                string selectedAccountVal;
                bool isValidAmount = true;
                decimal pettyCashAmountLimit = 0;
                int drCount;
                int crCount;
                string xmlDoc;

                DropDownList ddlWkfAction;
                TextBox WrkfComments;
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
                }

                switch (commonActions)
                {
                    #region Add Debit
                    case ActionsEnum.ADDDEBIT:
                        drCount = 1;
                        dicDrControls = new Dictionary<string, string>();
                        if (Session[ERP.Utilities.SessionStrings.AccountType] != null)//Used to store Account Names
                        {
                            dicAccountType = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.AccountType];
                        }
                        else
                        {
                            dicAccountType = new Dictionary<string, string>();
                        }
                        if (Session[ERP.Utilities.SessionStrings.ControlInfo] != null)//Used to store any special informations
                        {
                            dicControlinfo = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.ControlInfo];
                        }
                        else
                        {
                            dicControlinfo = new Dictionary<string, string>();
                        }
                        #region getID
                        //Generate the format of new Debit section id
                        //get the maximum group no from dr section. create the next id. check whether this is in removed session or not. if yes create next one . check again and again. after all this steps we will get the new id.
                        if (Session[ERP.Utilities.SessionStrings.DrControls] != null)
                        {
                            dicDrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.DrControls];//All active Dr Controls 
                            var drControls = from pair in dicDrControls
                                             orderby pair.Key.Substring(0, 13) descending //orderby pair.Key.Substring(0, 9) descending
                                             select pair;
                            if (drControls != null && drControls.Count() > 0)//if the UI contains drcontrols
                            {
                                //Get the current maximum drcount
                                drCount = Convert.ToInt32(string.IsNullOrEmpty(drControls.First().Key.Substring(3, 4)) ? "0" : drControls.First().Key.Substring(3, 4));
                                drCount++;
                                //check if drCount is in deleted Dr controls list
                                if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)
                                {
                                    //Gets the removed controls list
                                    removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];
                                    while (removedControlsList.Contains("dic" + drCount.ToString("0000") + "01" + "set" + "1" + "01" + "id" + drCount.ToString("0000") + "dr"))
                                    {
                                        //If drcount is in removed controls list den increment drcount by 1
                                        drCount++;
                                    }
                                }
                            }
                            else//if the UI contains no drcontrols
                            {
                                //check if drCount is in deleted Dr controls list
                                if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)
                                {
                                    //Gets the removed controls list
                                    removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];
                                    while (removedControlsList.Contains("dic" + drCount.ToString("0000") + "01" + "set" + "1" + "01" + "id" + drCount.ToString("0000") + "dr"))
                                    {
                                        //If drcount is in removed controls list den increment drcount by 1
                                        drCount++;
                                    }
                                }
                            }
                        }
                        #endregion
                        selectedAccountVal = "-1";
                        divColStyle = "divcolmiddle-S1 input-margin2";
                        div = new HtmlGenericControl("div");//This div will hold the drcontrols that we are going to create.
                        div.Attributes.Add("class", divColStyle);//set the style for the div
                        divValidation = new HtmlGenericControl("div");//This div will hold all the validation controls that we are going to create.
                        divValidation.Attributes.Add("class", "starwrap");//set the style for the div
                        tbControls = new Table();
                        tbControls.CssClass = "";
                        trControls = new TableRow();
                        tcControl = new TableCell();
                        #region SubType
                        Label txtSubType = new Label()//Account Type
                        {
                            Text = "",
                            ClientIDMode = ClientIDMode.Static
                        };
                        txtSubType.ID = "dic" + drCount.ToString("0000") + "01" + "set" + "1" + "01" + "id" + drCount.ToString("0000") + "dr";
                        txtSubType.Text = GetLocalResourceObject("New_Debit_Account").ToString();
                        //Associate control id will be the id of Account Textbox. We can get the this id by just incrementing the control index in the set from the Account type ControlID
                        txtSubType.AssociatedControlID = "dic" + drCount.ToString("0000") + "02" + "set" + "1" + "02" + "id" + drCount.ToString("0000") + "dr";
                        div.Controls.Add(txtSubType);
                        if (txtSubType != null)
                        {
                            if (!dicDrControls.ContainsKey(txtSubType.ID))
                                dicDrControls.Add(txtSubType.ID, "SubType");//Add to Active dr Controls List
                            if (!dicAccountType.ContainsKey(txtSubType.ID))
                                dicAccountType.Add(txtSubType.ID, txtSubType.Text);//Add Account Name in dictionary
                        }
                        #endregion
                        #region Account
                        //It is an autocomplte control with postback. So we need Textbox,Hiddenfield and a Button.
                        TextBox txtAccount = new TextBox()
                        {
                            Text = "",
                            MaxLength = 100,
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexDr,
                            Enabled = true
                        };
                        HiddenField hdfAccount = new HiddenField
                        {
                            ClientIDMode = ClientIDMode.Static
                        };
                        Button btnAccount = new Button
                        {
                            ClientIDMode = ClientIDMode.Static,
                            CommandName = "JOURNALACCOUNTINDEXCHANGED",
                            EnableTheming = false
                        };
                        btnAccount.Attributes.Add("style", "display:none;");
                        txtAccount.ID = "dic" + drCount.ToString("0000") + "02" + "set" + "1" + "02" + "id" + drCount.ToString("0000") + "dr";
                        hdfAccount.ID = "dic" + drCount.ToString("0000") + "03" + "set" + "1" + "03" + "id" + drCount.ToString("0000") + "dr";
                        btnAccount.ID = "dic" + drCount.ToString("0000") + "04" + "set" + "1" + "04" + "id" + drCount.ToString("0000") + "dr";
                        hdfSubTypePk.Value = "0";
                        if (!dicControlinfo.ContainsKey(txtAccount.ID + "SubTypePk"))//Store Subtype PK
                            dicControlinfo.Add(txtAccount.ID + "SubTypePk", hdfSubTypePk.Value);
                        //Register Autocomplete script
                        journalScript = journalScript + "GrandScriptUtils.MakeAutoCompleteDDL('" + txtAccount.ID + "', (url1.indexOf('?') != -1 ? url1+'&' :  url1+'?') + 'AccType=" + hdfSubTypePk.Value + "', '" + hdfAccount.ID + "', true, true, 'JOURNALACCOUNT',false,false,false);";
                        selectedAccountVal = hdfAccount.Value;
                        //Register Button Event
                        btnAccount.Click += new EventHandler(ActionHandler);
                        //Add controls to div
                        div.Controls.Add(txtAccount);
                        div.Controls.Add(hdfAccount);
                        div.Controls.Add(btnAccount);
                        RequiredFieldValidator vrfAccount = new RequiredFieldValidator()
                        {
                            ID = "vrf" + txtAccount.ID.Replace("dic", ""),
                            ControlToValidate = txtAccount.ID,
                            Text = "*",
                            CssClass = "star",
                            ValidationGroup = "voucher",
                            Display = ValidatorDisplay.Dynamic,
                            EnableClientScript = true,
                            InitialValue = Resources.Messages.AutoDefaultValue,
                            SetFocusOnError = true,
                            ErrorMessage = GetLocalResourceObject("Err_Account").ToString(),
                            ClientIDMode = ClientIDMode.Static
                        };
                        //Add validation control to validation div
                        divValidation.Controls.Add(vrfAccount);
                        TabIndexDr++;
                        if (txtAccount != null)
                            if (!dicDrControls.ContainsKey(txtAccount.ID))
                                dicDrControls.Add(txtAccount.ID, "Account");//Add to Active dr Controls List
                        if (hdfAccount != null)
                            if (!dicDrControls.ContainsKey(hdfAccount.ID))
                                dicDrControls.Add(hdfAccount.ID, "Account");//Add to Active dr Controls List
                        if (btnAccount != null)
                            if (!dicDrControls.ContainsKey(btnAccount.ID))
                                dicDrControls.Add(btnAccount.ID, "Account");//Add to Active dr Controls List
                        #endregion
                        #region Narration
                        //Narration Textbox Control
                        TextBox txtNarration = new TextBox()
                        {
                            Text = "",
                            MaxLength = 400,
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexDr,
                            Enabled = true,
                            Width = IsSameCurrency() == true ? new Unit(GetLocalResourceObject("NarationWidthWithoutAmountBC").ToString()) : new Unit(GetLocalResourceObject("NarationWidthNormal").ToString())
                        };
                        txtNarration.ID = "dic" + drCount.ToString("0000") + "05" + "set" + "1" + "05" + "id" + drCount.ToString("0000") + "dr";
                        div.Controls.Add(txtNarration);////Add controls to div
                        TabIndexDr++;
                        if (txtNarration != null)
                        {
                            if (!dicDrControls.ContainsKey(txtNarration.ID))
                                dicDrControls.Add(txtNarration.ID, "Narration");//Add to Active dr Controls List
                        }
                        #endregion
                        #region AmountTC
                        //Amount in Transaction Currency
                        TextBox txtAmountTC = new TextBox()
                        {
                            Text = "",
                            MaxLength = 15,
                            ClientIDMode = ClientIDMode.Static,
                            Enabled = true,
                            TabIndex = TabIndexDr,
                            CssClass = "input-w8per numeric amounttcdr tempcss"
                        };
                        txtAmountTC.ID = "dic" + drCount.ToString("0000") + "06" + "set" + "1" + "06" + "id" + drCount.ToString("0000") + "dr";
                        txtAmountTC.Text = ((decimal)0).ToString(hdfCurrencyFormatVoucher.Value);
                        txtAmountTC.Attributes.Add("onkeyup", "CalculateBCAmt(this);");//script registration for Calculate BC
                        div.Controls.Add(txtAmountTC);
                        AmountValidation vamAmountTC = new AmountValidation()
                        {
                            ID = "vam" + txtAmountTC.ID.Replace("dic", ""),
                            ControlToValidate = txtAmountTC.ID,
                            ErrorMessage = GetLocalResourceObject("MsgErr_AmountTc").ToString(),
                            NumberDigits = 11,
                            Display = ValidatorDisplay.Dynamic,
                            Text = "*",
                            EnableClientScript = true,
                            CssClass = "star",
                            ValidationGroup = "voucher",
                            NonZero = true
                        };
                        divValidation.Controls.Add(vamAmountTC);
                        if (!string.IsNullOrEmpty(txtAmountTC.ID))
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtAmountTC.ID + "", "$('[id$=" + txtAmountTC.ID + "]').ForceNumericOnly();", true);
                        TabIndexDr++;
                        if (txtAmountTC != null)
                        {
                            if (!dicDrControls.ContainsKey(txtAmountTC.ID))
                                dicDrControls.Add(txtAmountTC.ID, "AmountTC");
                        }
                        #endregion
                        #region ExchangeRate
                        //Exchange Rate
                        TextBox txtExchangeRate = new TextBox()
                        {
                            Text = "",
                            MaxLength = 9,
                            ClientIDMode = ClientIDMode.Static,
                            Enabled = true,
                            TabIndex = TabIndexDr,
                            CssClass = "numeric input-w6per"
                        };
                        txtExchangeRate.ID = "dic" + drCount.ToString("0000") + "07" + "set" + "1" + "07" + "id" + drCount.ToString("0000") + "dr";
                        txtExchangeRate.Text = !string.IsNullOrEmpty(txtJournalExchangeRate.Text.Trim())
                            ? ERP.Utilities.CommonFunctions.DoubleFormat(Convert.ToDouble(txtJournalExchangeRate.Text.Trim()), exchRateDecimalDigits).ToString(hdfExchRateFormatVoucher.Value)
                            : 1.ToString(hdfExchRateFormatVoucher.Value);
                        HiddenField hdfEntryMode = new HiddenField()
                        {
                            ClientIDMode = ClientIDMode.Static
                        };
                        hdfEntryMode.ID = txtExchangeRate.ID + "EntryMode";//Used for to keep the entrymode of ExchangeRate
                        txtExchangeRate.Attributes.Add("onkeyup", "CalculateBCWithER(this);");//Register script for calculating BC with Exchange Rate
                        if (!string.IsNullOrEmpty(txtExchangeRate.ID))
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtExchangeRate.ID + "", "$('[id$=" + txtExchangeRate.ID + "]').ForceNumericOnly();", true);
                        TabIndexDr++;
                        if (!dicControlinfo.ContainsKey(txtExchangeRate.ID + "EntryMode"))
                            dicControlinfo.Add(txtExchangeRate.ID + "EntryMode", ((byte)VoucherEntryMode.Editable).ToString());//Keep the entrymode for the curresponding Exchange Rate in Dictionary
                        hdfEntryMode.Value = ((byte)VoucherEntryMode.Editable).ToString();//Default EntryMode is Editable
                        if (txtExchangeRate != null)
                        {
                            if (!dicDrControls.ContainsKey(txtExchangeRate.ID))
                                dicDrControls.Add(txtExchangeRate.ID, "ExchangeRate");
                            if (hdfEntryMode != null)
                            {
                                if (!dicDrControls.ContainsKey(hdfEntryMode.ID))
                                    dicDrControls.Add(hdfEntryMode.ID, "EntryMode");
                            }
                        }
                        transactionCurrency = 0;
                        if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                        {
                            transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                        }
                        else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                        {
                            transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);
                        }
                        if (transactionCurrency > 0 && transactionCurrency == currentUser.BaseCurrency)
                        {
                            txtExchangeRate.Visible = false;
                        }
                        div.Controls.Add(txtExchangeRate);
                        div.Controls.Add(hdfEntryMode);
                        ExchangeRateValidation vreExchangeRate = new ExchangeRateValidation()
                        {
                            ID = "vre" + txtExchangeRate.ID,
                            ControlToValidate = txtExchangeRate.ID,
                            ErrorMessage = GetLocalResourceObject("MsgErr_ExchangeRate").ToString(),
                            NumberDigits = 5,
                            Display = ValidatorDisplay.Dynamic,
                            Text = "*",
                            EnableClientScript = true,
                            CssClass = "star",
                            ValidationGroup = "voucher",
                            NonZero = true
                        };
                        divValidation.Controls.Add(vreExchangeRate);
                        #endregion
                        #region AmountBC
                        //Amount in Base Currency
                        TextBox txtAmountBC = new TextBox()
                        {
                            Text = "",
                            ClientIDMode = ClientIDMode.Static,
                            Enabled = true,
                            MaxLength = 15,
                            CssClass = "input-normalb input-w8per numeric"
                        };
                        txtAmountBC.ID = "dic" + drCount.ToString("0000") + "08" + "set" + "1" + "08" + "id" + drCount.ToString("0000") + "dr";
                        txtAmountBC.Text = ((decimal)0).ToString(hdfCurrencyFormatVoucher.Value);
                        if ((BCEnable && hdfEntryMode.Value == ((byte)VoucherEntryMode.Editable).ToString()) || IsYearEndVoucher() || IsEditableForApplication())//If BC is Enable and Entrymode is Editable
                        {
                            txtAmountBC.CssClass = "input-w8per numeric";
                            txtAmountBC.Attributes.Add("onkeyup", "CalculateTCAmt(this,event);");//script registration for Calculate TC
                            if (!string.IsNullOrEmpty(txtAmountBC.ID))
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtAmountBC.ID + "", "$('[id$=" + txtAmountBC.ID + "]').ForceNumericOnly();", true);
                        }
                        else//Disable Amount BC
                        {
                            txtAmountBC.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                            txtAmountBC.Attributes.Add("onpaste", "return false;");
                        }
                        div.Controls.Add(txtAmountBC);
                        AmountValidation vamAmountBC = new AmountValidation()
                        {
                            ID = "vam" + txtAmountBC.ID.Replace("dic", ""),
                            ControlToValidate = txtAmountBC.ID,
                            ErrorMessage = GetLocalResourceObject("MsgErr_AmountBc").ToString(),
                            NumberDigits = 11,
                            Display = ValidatorDisplay.Dynamic,
                            Text = "*",
                            EnableClientScript = true,
                            CssClass = "star",
                            ValidationGroup = "voucher"
                        };
                        divValidation.Controls.Add(vamAmountBC);
                        if (txtAmountBC != null)
                        {
                            if (!dicDrControls.ContainsKey(txtAmountBC.ID))
                                dicDrControls.Add(txtAmountBC.ID, "AmountBC");
                        }
                        transactionCurrency = 0;
                        if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                        {
                            transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                        }
                        else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                        {
                            transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);
                        }
                        if (transactionCurrency > 0 && transactionCurrency == currentUser.BaseCurrency)
                        {
                            txtAmountBC.Visible = false;
                        }
                        #endregion
                        #region Delete
                        //Delete button for deleting the Curresponding DR section
                        Button btnDelete = new Button()
                        {
                            Text = "",
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexDr,
                            Enabled = true,
                            SkinID = "delete-icon",
                            ToolTip = "Delete",
                            OnClientClick = "return ShowDeleteConfirm(this);"
                        };
                        btnDelete.ID = "dic" + drCount.ToString("0000") + "09" + "set" + "1" + "09" + "id" + drCount.ToString("0000") + "dr";

                        btnDelete.CommandName = ActionsEnum.REMOVEDEBIT.ToString();
                        btnDelete.Click += new EventHandler(ActionHandler);
                        if (EntryStatus == EntryStatus.VIEWMODE)
                        {
                            btnDelete.Visible = false;
                        }
                        div.Controls.Add(btnDelete);

                        ////divValidation contains all the Validation Controls in a group. It will be added to the group after delete button. So if any validation catch it will show * after delete button 
                        //div.Controls.Add(divValidation);
                        if (btnDelete != null)
                        {
                            if (!dicDrControls.ContainsKey(btnDelete.ID))
                                dicDrControls.Add(btnDelete.ID, "Delete");
                        }
                        TabIndexDr++;
                        #endregion
                        #region Cost Center
                        //Delete button for deleting the Curresponding DR section
                        Button btnCostCenter = new Button()
                        {
                            Text = "",
                            ClientIDMode = ClientIDMode.Static,
                            Visible = false,
                            TabIndex = TabIndexDr,
                            Enabled = true,
                            SkinID = "costcenter-icon",
                            ToolTip = GetLocalResourceObject("CCButtonToolTip").ToString()
                        };
                        HiddenField hdfIsCostCenter = new HiddenField
                        {
                            ClientIDMode = ClientIDMode.Static,
                            Value = "0"
                        };
                        btnCostCenter.ID = "dic" + drCount.ToString("0000") + "10" + "set" + "1" + "10" + "id" + drCount.ToString("0000") + "dr";
                        hdfIsCostCenter.ID = "dic" + drCount.ToString("0000") + "11" + "set" + "1" + "11" + "id" + drCount.ToString("0000") + "dr";
                        btnCostCenter.CommandName = ActionsEnum.COSTCENTER.ToString();
                        btnCostCenter.Click += new EventHandler(ActionHandler);

                        div.Controls.Add(btnCostCenter);

                        //divValidation contains all the Validation Controls in a group. It will be added to the group after delete button. So if any validation catch it will show * after delete button 
                        div.Controls.Add(divValidation);

                        div.Controls.Add(hdfIsCostCenter);
                        TabIndexDr++;
                        if (btnCostCenter != null)
                            if (!dicDrControls.ContainsKey(btnCostCenter.ID))
                                dicDrControls.Add(btnCostCenter.ID, "CostCenter");
                        if (hdfIsCostCenter != null)
                            if (!dicDrControls.ContainsKey(hdfIsCostCenter.ID))
                                dicDrControls.Add(hdfIsCostCenter.ID, "CostCenter");

                        #endregion
                        #region SubledgerLabel
                        //After Delete Button, the rest of the controls will comes in the next line. So the divClear will use to break the first line
                        divClear = new HtmlGenericControl("div");
                        divClear.Attributes.Add("class", "clear");
                        div.Controls.Add(divClear);
                        //It is a dummy label to adjust the style
                        Label lblSubledgerLabel = new Label()
                        {
                            ClientIDMode = ClientIDMode.Static,
                            Text = "&nbsp",
                            Visible = false
                        };
                        lblSubledgerLabel.ID = "dic" + drCount.ToString("0000") + "50" + "set" + "2" + "01" + "id" + drCount.ToString("0000") + "dr";
                        //The Associate control id is the id of subledger dropdownlist
                        lblSubledgerLabel.AssociatedControlID = "dic" + drCount.ToString("0000") + "51" + "set" + "2" + "02" + "id" + drCount.ToString("0000") + "dr";
                        div.Controls.Add(lblSubledgerLabel);
                        if (lblSubledgerLabel != null)
                        {
                            if (!dicDrControls.ContainsKey(lblSubledgerLabel.ID))
                                dicDrControls.Add(lblSubledgerLabel.ID, "SubledgerLabel");
                        }
                        #endregion
                        #region SubLedger
                        //Subledger ddl control
                        DropDownList ddlSubLedger = new DropDownList()
                        {
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexDr,
                            Visible = false
                        };
                        ddlSubLedger.ID = "dic" + drCount.ToString("0000") + "51" + "set" + "2" + "02" + "id" + drCount.ToString("0000") + "dr";
                        div.Controls.Add(ddlSubLedger);
                        RequiredFieldValidator vrfSubLedger = new RequiredFieldValidator()
                        {
                            ID = "vrf" + ddlSubLedger.ID.Replace("dic", ""),
                            ControlToValidate = ddlSubLedger.ID,
                            Text = "*",
                            CssClass = "star",
                            ValidationGroup = "voucher",
                            Display = ValidatorDisplay.Dynamic,
                            EnableClientScript = true,
                            InitialValue = CommonConstants.SELECTVAL,
                            SetFocusOnError = true,
                            ErrorMessage = GetLocalResourceObject("Err_SubLedger").ToString(),
                            ClientIDMode = ClientIDMode.Static
                        };
                        divValidation.Controls.Add(vrfSubLedger);
                        vrfSubLedger.Enabled = ddlSubLedger.Visible ? true : false;
                        TabIndexDr++;
                        if (ddlSubLedger != null)
                        {
                            if (!dicDrControls.ContainsKey(ddlSubLedger.ID))
                                dicDrControls.Add(ddlSubLedger.ID, "SubLedger");
                        }
                        #endregion
                        #region InstrumentNo
                        //Instrument No Text box
                        TextBox txtInstrumentNo = new TextBox()
                        {
                            Text = "",
                            MaxLength = 70,
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexDr,
                            Enabled = true,
                            Visible = false,
                            CssClass = "input16"
                        };
                        txtInstrumentNo.ID = "dic" + drCount.ToString("0000") + "52" + "set" + "2" + "03" + "id" + drCount.ToString("0000") + "dr";
                        string instrumentNo = GetLocalResourceObject("InstrumentNo").ToString();
                        //Set "Instrument No" if their is no text in the textbox with a watermark style. Otherwise remove this style
                        txtInstrumentNo.Attributes.Remove("onblur");
                        txtInstrumentNo.Attributes.Remove("onfocus");
                        txtInstrumentNo.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + instrumentNo + "';$(this).addClass('input-watermark');}");
                        txtInstrumentNo.Attributes.Add("onfocus", "if (this.value == '" + instrumentNo + "') {this.value = '';$(this).removeClass('input-watermark');}");
                        txtInstrumentNo.Text = instrumentNo;
                        journalScript = journalScript + "if ($('#" + txtInstrumentNo.ID + "').val() == '" + instrumentNo + "') {$('#" + txtInstrumentNo.ID + "').addClass('input-watermark');}";
                        div.Controls.Add(txtInstrumentNo);
                        TabIndexDr++;
                        if (txtInstrumentNo != null)
                        {
                            if (!dicDrControls.ContainsKey(txtInstrumentNo.ID))
                                dicDrControls.Add(txtInstrumentNo.ID, "InstrumentNo");
                        }
                        #endregion
                        #region Date
                        //Date Picker Control
                        TextBox txtDate = new TextBox()
                        {
                            Text = "",
                            MaxLength = 11,
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexDr,
                            Enabled = true,
                            Visible = false,
                            CssClass = "input-w8-5per"
                        };
                        txtDate.ID = "dic" + drCount.ToString("0000") + "53" + "set" + "2" + "04" + "id" + drCount.ToString("0000") + "dr";
                        txtDate.Attributes.Add("onkeydown", "return CheckKey(event);");
                        txtDate.Attributes.Add("onpaste", "return false;");
                        string date = GetLocalResourceObject("Date").ToString();
                        //Set "Date" if their is no text in the textbox with a watermark style. Otherwise remove this style
                        txtDate.Attributes.Remove("onblur");
                        txtDate.Attributes.Remove("onfocus");
                        txtDate.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + date + "';$(this).addClass('input-watermark');}");
                        txtDate.Attributes.Add("onfocus", "if (this.value == '" + date + "') {this.value = '';$(this).removeClass('input-watermark');}");
                        txtDate.Text = date;
                        journalScript = journalScript + "if ($('#" + txtDate.ID + "').val() == '" + date + "') {$('#" + txtDate.ID + "').addClass('input-watermark');}";
                        div.Controls.Add(txtDate);
                        if (txtDate != null)
                        {
                            if (!dicDrControls.ContainsKey(txtDate.ID))
                                dicDrControls.Add(txtDate.ID, "Date");
                        }
                        TabIndexDr++;
                        #endregion
                        #region FavourOf
                        //Favourof Textbox
                        TextBox txtFavourOf = new TextBox()
                        {
                            MaxLength = 150,
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexDr,
                            Enabled = true,
                            Visible = false,
                            CssClass = "input-w23-6per"
                        };
                        txtFavourOf.ID = "dic" + drCount.ToString("0000") + "54" + "set" + "2" + "05" + "id" + drCount.ToString("0000") + "dr";
                        string favourOf = GetLocalResourceObject("FavourOf").ToString();
                        //Set "FavourOf" if their is no text in the textbox with a watermark style. Otherwise remove this style
                        txtFavourOf.Attributes.Remove("onblur");
                        txtFavourOf.Attributes.Remove("onfocus");
                        txtFavourOf.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + favourOf + "';$(this).addClass('input-watermark');}");
                        txtFavourOf.Attributes.Add("onfocus", "if (this.value == '" + favourOf + "') {this.value = '';$(this).removeClass('input-watermark');}");
                        txtFavourOf.Text = favourOf;
                        journalScript = journalScript + "if ($('#" + txtFavourOf.ID + "').val() == '" + favourOf + "') {$('#" + txtFavourOf.ID + "').addClass('input-watermark');}";
                        div.Controls.Add(txtFavourOf);
                        TabIndexDr++;
                        if (txtFavourOf != null)
                        {
                            if (!dicDrControls.ContainsKey(txtFavourOf.ID))
                                dicDrControls.Add(txtFavourOf.ID, "FavourOf");
                        }
                        #endregion
                        //Now the div contains the new added Debit controls. Now we are going to add it to our page
                        tcControl.Controls.Add(div);//Adding to Table Cell
                        trControls.Cells.Add(tcControl);//Adding to Table Row
                        tbControls.Rows.Add(trControls);//Adding to Table
                        divGroupDr.Controls.Add(tbControls);//Adding to Masater Div that holds all the Debit Groups
                        if (dicDrControls != null && dicDrControls.Count > 0)
                        {
                            Session[ERP.Utilities.SessionStrings.DrControls] = dicDrControls;//Updating the Debit Control Session
                        }
                        if (dicControlinfo != null && dicControlinfo.Count > 0)
                        {
                            Session[ERP.Utilities.SessionStrings.ControlInfo] = dicControlinfo;//Updating the Control Info. Session
                        }
                        if (dicAccountType != null && dicAccountType.Count > 0)
                        {
                            Session[ERP.Utilities.SessionStrings.AccountType] = dicAccountType;//Updating the Account Name Session
                        }
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                        //                "ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                        "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                        break;
                    #endregion
                    #region Add Credit
                    case ActionsEnum.ADDCREDIT:
                        crCount = 1;
                        dicCrControls = new Dictionary<string, string>();
                        dicAccountType = new Dictionary<string, string>();
                        if (Session[ERP.Utilities.SessionStrings.AccountType] != null)
                        {
                            dicAccountType = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.AccountType];
                        }
                        else
                        {
                            dicAccountType = new Dictionary<string, string>();
                        }
                        if (Session[ERP.Utilities.SessionStrings.ControlInfo] != null)
                        {
                            dicControlinfo = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.ControlInfo];
                        }
                        else
                        {
                            dicControlinfo = new Dictionary<string, string>();
                        }

                        #region getID
                        //Generate the format of new Credit section id
                        //get the maximum group no from Cr section. create the next id. check whether this is in removed session or not. if yes create next one . check again and again. after all this steps we will get the new id.
                        if (Session[ERP.Utilities.SessionStrings.CrControls] != null)
                        {
                            dicCrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.CrControls];
                            var crControls = from pair in dicCrControls
                                             orderby pair.Key.Substring(0, 13) descending//orderby pair.Key.Substring(0, 9) descending
                                             select pair;
                            if (crControls != null && crControls.Count() > 0)
                            {
                                //Get the current maximum crcount
                                crCount = Convert.ToInt32(string.IsNullOrEmpty(crControls.First().Key.Substring(3, 4)) ? "0" : crControls.First().Key.Substring(3, 4));
                                crCount++;
                                //check if crCount is in deleted Cr controls list
                                if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)
                                {
                                    //Gets the removed controls list
                                    removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];
                                    while (removedControlsList.Contains("dic" + crCount.ToString("0000") + "01" + "set" + "1" + "01" + "id" + crCount.ToString("0000") + "cr"))
                                    {
                                        //If crcount is in removed controls list den increment drcount by 1
                                        crCount++;
                                    }
                                }
                            }
                            else//if the UI contains no crcontrols
                            {
                                //check if crCount is in deleted Cr controls list
                                if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)
                                {
                                    //Gets the removed controls list
                                    removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];
                                    while (removedControlsList.Contains("dic" + crCount.ToString("0000") + "01" + "set" + "1" + "01" + "id" + crCount.ToString("0000") + "cr"))
                                    {
                                        //If crcount is in removed controls list den increment crcount by 1
                                        crCount++;
                                    }
                                }
                            }
                        }
                        #endregion
                        selectedAccountVal = "-1";
                        tbControls.CssClass = "";
                        divColStyle = "divcolmiddle-S1 input-margin2";
                        div = new HtmlGenericControl("div");//This div will hold the crcontrols that we are going to create.
                        div.Attributes.Add("class", divColStyle);//set the style for the div
                        divValidation = new HtmlGenericControl("div");//This div will hold all the validation controls that we are going to create.
                        divValidation.Attributes.Add("class", "starwrap");//set the style for the div
                        tbControls = new Table();
                        tbControls.CssClass = "";
                        trControls = new TableRow();
                        tcControl = new TableCell();
                        #region SubType
                        //Account Type
                        Label txtSubTypeCr = new Label()
                        {
                            Text = "",
                            ClientIDMode = ClientIDMode.Static
                        };
                        txtSubTypeCr.ID = "dic" + crCount.ToString("0000") + "01" + "set" + "1" + "01" + "id" + crCount.ToString("0000") + "cr";
                        txtSubTypeCr.Text = GetLocalResourceObject("New_Credit_Account").ToString();
                        //Associate control id will be the id of Account Textbox. We can get the this id by just incrementing the control index in the set from the Account type ControlID
                        txtSubTypeCr.AssociatedControlID = "dic" + crCount.ToString("0000") + "02" + "set" + "1" + "02" + "id" + crCount.ToString("0000") + "cr";
                        div.Controls.Add(txtSubTypeCr);
                        if (txtSubTypeCr != null)
                        {
                            if (!dicCrControls.ContainsKey(txtSubTypeCr.ID))
                                dicCrControls.Add(txtSubTypeCr.ID, "SubType");//Add to Active dr Controls List
                            if (!dicAccountType.ContainsKey(txtSubTypeCr.ID))
                                dicAccountType.Add(txtSubTypeCr.ID, txtSubTypeCr.Text);//Add Account Name in dictionary
                        }
                        #endregion
                        #region Account
                        //It is an autocomplte control with postback. So we need Textbox,Hiddenfield and a Button.
                        TextBox txtAccountCr = new TextBox()
                        {
                            Text = "",
                            MaxLength = 100,
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexCr,
                            Enabled = true
                        };
                        HiddenField hdfAccountCr = new HiddenField
                        {
                            ClientIDMode = ClientIDMode.Static
                        };
                        Button btnAccountCr = new Button
                        {
                            ClientIDMode = ClientIDMode.Static,
                            CommandName = "JOURNALACCOUNTINDEXCHANGED",
                            EnableTheming = false
                        };
                        btnAccountCr.Attributes.Add("style", "display:none;");

                        txtAccountCr.ID = "dic" + crCount.ToString("0000") + "02" + "set" + "1" + "02" + "id" + crCount.ToString("0000") + "cr";
                        hdfAccountCr.ID = "dic" + crCount.ToString("0000") + "03" + "set" + "1" + "03" + "id" + crCount.ToString("0000") + "cr";
                        btnAccountCr.ID = "dic" + crCount.ToString("0000") + "04" + "set" + "1" + "04" + "id" + crCount.ToString("0000") + "cr";

                        hdfSubTypePk.Value = "0";

                        if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.PCS)//If it is Pettycash voucher
                        {
                            hdfSubTypePk.Value = FINCOASUBTYPECFGEnum.Cash.GetHashCode().ToString();
                        }

                        if (!dicControlinfo.ContainsKey(txtAccountCr.ID + "SubTypePk"))//Store Subtype PK
                            dicControlinfo.Add(txtAccountCr.ID + "SubTypePk", hdfSubTypePk.Value);
                        //Register Autocomplete script
                        journalScript = journalScript + "GrandScriptUtils.MakeAutoCompleteDDL('" + txtAccountCr.ID + "', (url1.indexOf('?') != -1 ? url1+'&' :  url1+'?') + 'AccType=" + hdfSubTypePk.Value + "', '" + hdfAccountCr.ID + "', true, true, 'JOURNALACCOUNT',false,false,false);";
                        selectedAccountVal = hdfAccountCr.Value;
                        //Register Button Event
                        btnAccountCr.Click += new EventHandler(ActionHandler);
                        //Add controls to div
                        div.Controls.Add(txtAccountCr);
                        div.Controls.Add(hdfAccountCr);
                        div.Controls.Add(btnAccountCr);

                        RequiredFieldValidator vrfAccountCr = new RequiredFieldValidator()
                        {
                            ID = "vrf" + txtAccountCr.ID.Replace("dic", ""),
                            ControlToValidate = txtAccountCr.ID,
                            Text = "*",
                            CssClass = "star",
                            ValidationGroup = "voucher",
                            Display = ValidatorDisplay.Dynamic,
                            EnableClientScript = true,
                            InitialValue = Resources.Messages.AutoDefaultValue,
                            SetFocusOnError = true,
                            ErrorMessage = GetLocalResourceObject("Err_Account").ToString(),
                            ClientIDMode = ClientIDMode.Static
                        };
                        //Add validation control to validation div
                        divValidation.Controls.Add(vrfAccountCr);

                        TabIndexCr++;
                        if (txtAccountCr != null)
                            if (!dicCrControls.ContainsKey(txtAccountCr.ID))
                                dicCrControls.Add(txtAccountCr.ID, "Account");//Add to Active cr Controls List
                        if (hdfAccountCr != null)
                            if (!dicCrControls.ContainsKey(hdfAccountCr.ID))
                                dicCrControls.Add(hdfAccountCr.ID, "Account");//Add to Active cr Controls List
                        if (btnAccountCr != null)
                            if (!dicCrControls.ContainsKey(btnAccountCr.ID))
                                dicCrControls.Add(btnAccountCr.ID, "Account");//Add to Active cr Controls List

                        #endregion
                        #region Narration
                        //Narration Textbox Control
                        TextBox txtNarrationCr = new TextBox()
                        {
                            Text = "",
                            MaxLength = 400,
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexCr,
                            Enabled = true,
                            Width = IsSameCurrency() == true ? new Unit(GetLocalResourceObject("NarationWidthWithoutAmountBC").ToString()) : new Unit(GetLocalResourceObject("NarationWidthNormal").ToString())
                        };
                        txtNarrationCr.ID = "dic" + crCount.ToString("0000") + "05" + "set" + "1" + "05" + "id" + crCount.ToString("0000") + "cr";
                        div.Controls.Add(txtNarrationCr);
                        TabIndexCr++;
                        if (txtNarrationCr != null)
                        {
                            if (!dicCrControls.ContainsKey(txtNarrationCr.ID))
                                dicCrControls.Add(txtNarrationCr.ID, "Narration");//Add to Active cr Controls List
                        }
                        #endregion
                        #region AmountTC
                        //Amount in Transaction Currency
                        TextBox txtAmountTCCr = new TextBox()
                        {
                            Text = "",
                            MaxLength = 15,
                            ClientIDMode = ClientIDMode.Static,
                            Enabled = true,
                            TabIndex = TabIndexCr,
                            CssClass = "input-w8per numeric amounttccr tempcss"
                        };
                        txtAmountTCCr.ID = "dic" + crCount.ToString("0000") + "06" + "set" + "1" + "06" + "id" + crCount.ToString("0000") + "cr";
                        txtAmountTCCr.Text = ((decimal)0).ToString(hdfCurrencyFormatVoucher.Value);
                        txtAmountTCCr.Attributes.Add("onkeyup", "CalculateBCAmt(this);");//script registration for Calculate BC
                        div.Controls.Add(txtAmountTCCr);

                        AmountValidation vamAmountTCCr = new AmountValidation()
                        {
                            ID = "vam" + txtAmountTCCr.ID.Replace("dic", ""),
                            ControlToValidate = txtAmountTCCr.ID,
                            ErrorMessage = GetLocalResourceObject("MsgErr_AmountTc").ToString(),
                            NumberDigits = 11,
                            Display = ValidatorDisplay.Dynamic,
                            Text = "*",
                            EnableClientScript = true,
                            CssClass = "star",
                            ValidationGroup = "voucher",
                            NonZero = true
                        };
                        divValidation.Controls.Add(vamAmountTCCr);
                        if (!string.IsNullOrEmpty(txtAmountTCCr.ID))
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtAmountTCCr.ID + "", "$('[id$=" + txtAmountTCCr.ID + "]').ForceNumericOnly();", true);
                        TabIndexCr++;
                        if (txtAmountTCCr != null)
                        {
                            if (!dicCrControls.ContainsKey(txtAmountTCCr.ID))
                                dicCrControls.Add(txtAmountTCCr.ID, "AmountTC");
                        }
                        #endregion
                        #region ExchangeRate
                        //Exchange Rate
                        TextBox txtExchangeRateCr = new TextBox()
                        {
                            Text = "",
                            MaxLength = 9,
                            ClientIDMode = ClientIDMode.Static,
                            Enabled = true,
                            TabIndex = TabIndexCr,
                            CssClass = "numeric input-w6per"
                        };
                        txtExchangeRateCr.ID = "dic" + crCount.ToString("0000") + "07" + "set" + "1" + "07" + "id" + crCount.ToString("0000") + "cr";
                        txtExchangeRateCr.Text = !string.IsNullOrEmpty(txtJournalExchangeRate.Text.Trim())
                            ? ERP.Utilities.CommonFunctions.DoubleFormat(Convert.ToDouble(txtJournalExchangeRate.Text.Trim()), exchRateDecimalDigits).ToString(hdfExchRateFormatVoucher.Value)
                            : 1.ToString(hdfExchRateFormatVoucher.Value);
                        HiddenField hdfEntryModeCr = new HiddenField()
                        {
                            ClientIDMode = ClientIDMode.Static
                        };
                        hdfEntryModeCr.ID = txtExchangeRateCr.ID + "EntryMode";//Used for to keep the entrymode of ExchangeRate in the credit detail section
                        txtExchangeRateCr.Attributes.Add("onkeyup", "CalculateBCWithER(this);");//Register script for calculate the BC with ExchangeRate
                        if (!string.IsNullOrEmpty(txtExchangeRateCr.ID))
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtExchangeRateCr.ID + "", "$('[id$=" + txtExchangeRateCr.ID + "]').ForceNumericOnly();", true);
                        TabIndexCr++;
                        if (!dicControlinfo.ContainsKey(txtExchangeRateCr.ID + "EntryMode"))
                            dicControlinfo.Add(txtExchangeRateCr.ID + "EntryMode", ((byte)VoucherEntryMode.Editable).ToString());//Keep the entrymode for the curresponding Exchange Rate in Dictionary
                        hdfEntryModeCr.Value = ((byte)VoucherEntryMode.Editable).ToString();//Default EntryMode is Editable
                        if (txtExchangeRateCr != null)
                        {
                            if (!dicCrControls.ContainsKey(txtExchangeRateCr.ID))
                                dicCrControls.Add(txtExchangeRateCr.ID, "ExchangeRate");
                            if (hdfEntryModeCr != null)
                            {
                                if (!dicCrControls.ContainsKey(hdfEntryModeCr.ID))
                                    dicCrControls.Add(hdfEntryModeCr.ID, "EntryMode");
                            }
                        }

                        transactionCurrency = 0;
                        if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                        {
                            transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                        }
                        else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                        {
                            transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);
                        }

                        if (transactionCurrency > 0
                            &&
                            transactionCurrency == currentUser.BaseCurrency)
                        {
                            txtExchangeRateCr.Visible = false;
                        }
                        div.Controls.Add(txtExchangeRateCr);
                        div.Controls.Add(hdfEntryModeCr);
                        ExchangeRateValidation vreExchangeRateCr = new ExchangeRateValidation()
                        {
                            ID = "vre" + txtExchangeRateCr.ID,
                            ControlToValidate = txtExchangeRateCr.ID,
                            ErrorMessage = GetLocalResourceObject("MsgErr_ExchangeRate").ToString(),
                            NumberDigits = 5,
                            Display = ValidatorDisplay.Dynamic,
                            Text = "*",
                            EnableClientScript = true,
                            CssClass = "star",
                            ValidationGroup = "voucher",
                            NonZero = true
                        };
                        divValidation.Controls.Add(vreExchangeRateCr);
                        #endregion
                        #region AmountBC
                        //Amount in Base Currency
                        TextBox txtAmountBCCr = new TextBox()
                        {
                            Text = "",
                            ClientIDMode = ClientIDMode.Static,
                            Enabled = true,
                            MaxLength = 15,
                            CssClass = "input-normalb input-w8per numeric tempcss"
                        };

                        txtAmountBCCr.ID = "dic" + crCount.ToString("0000") + "08" + "set" + "1" + "08" + "id" + crCount.ToString("0000") + "cr";
                        txtAmountBCCr.Text = ((decimal)0).ToString(hdfCurrencyFormatVoucher.Value);

                        if (BCEnable && hdfEntryModeCr.Value == ((byte)VoucherEntryMode.Editable).ToString())//If BC is Enable and Entrymode is Editable
                        {
                            txtAmountBCCr.CssClass = "input-w8per numeric tempcss";
                            txtAmountBCCr.Attributes.Add("onkeyup", "CalculateTCAmt(this,event);");
                            if (!string.IsNullOrEmpty(txtAmountBCCr.ID))
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtAmountBCCr.ID + "", "$('[id$=" + txtAmountBCCr.ID + "]').ForceNumericOnly();", true);
                        }
                        else//Disable Amount BC
                        {
                            txtAmountBCCr.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                            txtAmountBCCr.Attributes.Add("onpaste", "return false;");
                        }
                        div.Controls.Add(txtAmountBCCr);
                        AmountValidation vamAmountBCCr = new AmountValidation()
                        {
                            ID = "vam" + txtAmountBCCr.ID.Replace("dic", ""),
                            ControlToValidate = txtAmountBCCr.ID,
                            ErrorMessage = GetLocalResourceObject("MsgErr_AmountBc").ToString(),
                            NumberDigits = 11,
                            Display = ValidatorDisplay.Dynamic,
                            Text = "*",
                            EnableClientScript = true,
                            CssClass = "star",
                            ValidationGroup = "voucher"
                        };
                        divValidation.Controls.Add(vamAmountBCCr);
                        if (txtAmountBCCr != null)
                        {
                            if (!dicCrControls.ContainsKey(txtAmountBCCr.ID))
                                dicCrControls.Add(txtAmountBCCr.ID, "AmountBC");
                        }

                        transactionCurrency = 0;
                        if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                        {
                            transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                        }
                        else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                        {
                            transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);
                        }

                        if (transactionCurrency > 0
                            &&
                            transactionCurrency == currentUser.BaseCurrency)
                        {
                            txtAmountBCCr.Visible = false;
                        }
                        #endregion
                        #region Delete
                        //Delete button for deleting the Curresponding DR section
                        Button btnDeleteCr = new Button()
                        {
                            Text = "",
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexCr,
                            Enabled = true,
                            SkinID = "delete-icon",
                            CssClass = "margntop2",
                            ToolTip = "Delete",
                            OnClientClick = "return ShowDeleteConfirm(this);"
                        };
                        btnDeleteCr.ID = "dic" + crCount.ToString("0000") + "09" + "set" + "1" + "09" + "id" + crCount.ToString("0000") + "cr";
                        btnDeleteCr.CommandName = ActionsEnum.REMOVECREDIT.ToString();

                        btnDeleteCr.Click += new EventHandler(ActionHandler);
                        if (EntryStatus == EntryStatus.VIEWMODE)
                        {
                            btnDeleteCr.Visible = false;
                        }
                        div.Controls.Add(btnDeleteCr);
                        ////divValidation contains all the Validation Controls in a group. It will be added to the group after delete button. So if any validation catch it will show * after delete button 
                        //div.Controls.Add(divValidation);
                        TabIndexCr++;
                        if (btnDeleteCr != null)
                        {
                            if (!dicCrControls.ContainsKey(btnDeleteCr.ID))
                                dicCrControls.Add(btnDeleteCr.ID, "Delete");
                        }
                        #endregion
                        #region Cost Center
                        //Delete button for deleting the Curresponding DR section
                        Button btnCostCenterCr = new Button()
                        {
                            Text = "",
                            ClientIDMode = ClientIDMode.Static,
                            Visible = false,
                            TabIndex = TabIndexCr,
                            Enabled = true,
                            SkinID = "costcenter-icon",
                            ToolTip = GetLocalResourceObject("CCButtonToolTip").ToString()
                        };
                        HiddenField hdfIsCostCenterCr = new HiddenField
                        {
                            ClientIDMode = ClientIDMode.Static,
                            Value = "0"
                        };
                        btnCostCenterCr.ID = "dic" + crCount.ToString("0000") + "10" + "set" + "1" + "10" + "id" + crCount.ToString("0000") + "cr";
                        hdfIsCostCenterCr.ID = "dic" + crCount.ToString("0000") + "11" + "set" + "1" + "11" + "id" + crCount.ToString("0000") + "cr";
                        btnCostCenterCr.CommandName = ActionsEnum.COSTCENTER.ToString();
                        btnCostCenterCr.Click += new EventHandler(ActionHandler);
                        div.Controls.Add(btnCostCenterCr);

                        //divValidation contains all the Validation Controls in a group. It will be added to the group after delete button. So if any validation catch it will show * after delete button 
                        div.Controls.Add(divValidation);

                        div.Controls.Add(hdfIsCostCenterCr);

                        TabIndexCr++;
                        if (btnCostCenterCr != null)
                            if (!dicCrControls.ContainsKey(btnCostCenterCr.ID))
                                dicCrControls.Add(btnCostCenterCr.ID, "CostCenter");
                        if (hdfIsCostCenterCr != null)
                            if (!dicCrControls.ContainsKey(hdfIsCostCenterCr.ID))
                                dicCrControls.Add(hdfIsCostCenterCr.ID, "CostCenter");

                        #endregion
                        #region SubledgerLabel
                        //After Delete Button, the rest of the controls will comes in the next line. So the divClear will use to break the first line
                        divClear = new HtmlGenericControl("div");
                        divClear.Attributes.Add("class", "clear");
                        div.Controls.Add(divClear);
                        //It is a dummy label
                        Label lblSubledgerLabelCr = new Label()
                        {
                            ClientIDMode = ClientIDMode.Static,
                            Text = "&nbsp",
                            Visible = false
                        };

                        lblSubledgerLabelCr.ID = "dic" + crCount.ToString("0000") + "50" + "set" + "2" + "01" + "id" + crCount.ToString("0000") + "cr";
                        //The Associate control id is the id of subledger dropdownlist
                        lblSubledgerLabelCr.AssociatedControlID = "dic" + crCount.ToString("0000") + "51" + "set" + "2" + "02" + "id" + crCount.ToString("0000") + "cr";

                        div.Controls.Add(lblSubledgerLabelCr);
                        if (lblSubledgerLabelCr != null)
                        {
                            if (!dicCrControls.ContainsKey(lblSubledgerLabelCr.ID))
                                dicCrControls.Add(lblSubledgerLabelCr.ID, "SubledgerLabel");
                        }
                        #endregion
                        #region SubLedger
                        //Subledger ddl control
                        DropDownList ddlSubLedgerCr = new DropDownList()
                        {
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexCr,
                            Visible = false
                        };

                        ddlSubLedgerCr.ID = "dic" + crCount.ToString("0000") + "51" + "set" + "2" + "02" + "id" + crCount.ToString("0000") + "cr";
                        div.Controls.Add(ddlSubLedgerCr);
                        RequiredFieldValidator vrfSubLedgerCr = new RequiredFieldValidator()
                        {
                            ID = "vrf" + ddlSubLedgerCr.ID.Replace("dic", ""),
                            ControlToValidate = ddlSubLedgerCr.ID,
                            Text = "*",
                            CssClass = "star",
                            ValidationGroup = "voucher",
                            Display = ValidatorDisplay.Dynamic,
                            EnableClientScript = true,
                            InitialValue = CommonConstants.SELECTVAL,
                            SetFocusOnError = true,
                            ErrorMessage = GetLocalResourceObject("Err_SubLedger").ToString(),
                            ClientIDMode = ClientIDMode.Static
                        };
                        divValidation.Controls.Add(vrfSubLedgerCr);
                        vrfSubLedgerCr.Enabled = ddlSubLedgerCr.Visible ? true : false;
                        TabIndexCr++;
                        if (ddlSubLedgerCr != null)
                        {
                            if (!dicCrControls.ContainsKey(ddlSubLedgerCr.ID))
                                dicCrControls.Add(ddlSubLedgerCr.ID, "SubLedger");
                        }
                        #endregion
                        #region InstrumentNo
                        //Instrument No Text box
                        TextBox txtInstrumentNoCr = new TextBox()
                        {
                            Text = "",
                            MaxLength = 70,
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexCr,
                            Enabled = true,
                            Visible = false,
                            CssClass = "input16"
                        };
                        txtInstrumentNoCr.ID = "dic" + crCount.ToString("0000") + "52" + "set" + "2" + "03" + "id" + crCount.ToString("0000") + "cr";
                        string instrumentNoCr = GetLocalResourceObject("InstrumentNo").ToString();
                        //Set "Instrument No" if their is no text in the textbox with a watermark style. Otherwise remove this style
                        txtInstrumentNoCr.Attributes.Remove("onblur");
                        txtInstrumentNoCr.Attributes.Remove("onfocus");
                        txtInstrumentNoCr.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + instrumentNoCr + "';$(this).addClass('input-watermark');}");
                        txtInstrumentNoCr.Attributes.Add("onfocus", "if (this.value == '" + instrumentNoCr + "') {this.value = '';$(this).removeClass('input-watermark');}");
                        txtInstrumentNoCr.Text = instrumentNoCr;
                        journalScript = journalScript + "if ($('#" + txtInstrumentNoCr.ID + "').val() == '" + instrumentNoCr + "') {$('#" + txtInstrumentNoCr.ID + "').addClass('input-watermark');}";
                        div.Controls.Add(txtInstrumentNoCr);
                        TabIndexCr++;
                        if (txtInstrumentNoCr != null)
                        {
                            if (!dicCrControls.ContainsKey(txtInstrumentNoCr.ID))
                                dicCrControls.Add(txtInstrumentNoCr.ID, "InstrumentNo");
                        }
                        #endregion
                        #region Date
                        //Date Picker Control
                        TextBox txtDateCr = new TextBox()
                        {
                            Text = "",
                            MaxLength = 11,
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexCr,
                            Enabled = true,
                            Visible = false,
                            CssClass = "input-w8-5per"
                        };

                        txtDateCr.ID = "dic" + crCount.ToString("0000") + "53" + "set" + "2" + "04" + "id" + crCount.ToString("0000") + "cr";
                        txtDateCr.Attributes.Add("onkeydown", "return CheckKey(event);");
                        txtDateCr.Attributes.Add("onpaste", "return false;");

                        string dateCr = GetLocalResourceObject("Date").ToString();
                        //Set "Date" if their is no text in the textbox with a watermark style. Otherwise remove this style
                        txtDateCr.Attributes.Remove("onblur");
                        txtDateCr.Attributes.Remove("onfocus");
                        txtDateCr.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + dateCr + "';$(this).addClass('input-watermark');}");
                        txtDateCr.Attributes.Add("onfocus", "if (this.value == '" + dateCr + "') {this.value = '';$(this).removeClass('input-watermark');}");
                        txtDateCr.Text = dateCr;
                        journalScript = journalScript + "if ($('#" + txtDateCr.ID + "').val() == '" + dateCr + "') {$('#" + txtDateCr.ID + "').addClass('input-watermark');}";

                        div.Controls.Add(txtDateCr);
                        TabIndexCr++;
                        if (txtDateCr != null)
                        {
                            if (!dicCrControls.ContainsKey(txtDateCr.ID))
                                dicCrControls.Add(txtDateCr.ID, "Date");
                        }
                        #endregion
                        #region FavourOf
                        //Favourof Textbox
                        TextBox txtFavourOfCr = new TextBox()
                        {
                            MaxLength = 150,
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexCr,
                            Enabled = true,
                            Visible = false,
                            CssClass = "input-w23-6per"
                        };

                        txtFavourOfCr.ID = "dic" + crCount.ToString("0000") + "54" + "set" + "2" + "05" + "id" + crCount.ToString("0000") + "cr";

                        string favourOfCr = GetLocalResourceObject("FavourOf").ToString();
                        //Set "FavourOf" if their is no text in the textbox with a watermark style. Otherwise remove this style
                        txtFavourOfCr.Attributes.Remove("onblur");
                        txtFavourOfCr.Attributes.Remove("onfocus");
                        txtFavourOfCr.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + favourOfCr + "';$(this).addClass('input-watermark');}");
                        txtFavourOfCr.Attributes.Add("onfocus", "if (this.value == '" + favourOfCr + "') {this.value = '';$(this).removeClass('input-watermark');}");
                        txtFavourOfCr.Text = favourOfCr;
                        journalScript = journalScript + "if ($('#" + txtFavourOfCr.ID + "').val() == '" + favourOfCr + "') {$('#" + txtFavourOfCr.ID + "').addClass('input-watermark');}";
                        div.Controls.Add(txtFavourOfCr);
                        TabIndexCr++;
                        if (txtFavourOfCr != null)
                        {
                            if (!dicCrControls.ContainsKey(txtFavourOfCr.ID))
                                dicCrControls.Add(txtFavourOfCr.ID, "FavourOf");
                        }
                        #endregion
                        //Now the div contains the new added Credit controls. Now we are going to add it to our page
                        tcControl.Controls.Add(div);//Adding to Table Cell
                        trControls.Cells.Add(tcControl);//Adding to Table Row
                        tbControls.Rows.Add(trControls);//Adding to Table
                        divGroupCr.Controls.Add(tbControls);//Adding to Masater Div that holds all the Credit Groups
                        if (dicCrControls != null && dicCrControls.Count > 0)
                            Session[ERP.Utilities.SessionStrings.CrControls] = dicCrControls;//Updating the Credit Control Session
                        if (dicControlinfo != null && dicControlinfo.Count > 0)
                            Session[ERP.Utilities.SessionStrings.ControlInfo] = dicControlinfo;//Updating the Control Info. Session
                        if (dicAccountType != null && dicAccountType.Count > 0)
                            Session[ERP.Utilities.SessionStrings.AccountType] = dicAccountType;//Updating the Account Name Session
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                        //                "ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                        "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                        break;
                    #endregion
                    #region Delete Debit
                    case ActionsEnum.REMOVEDEBIT:
                        id = ((Button)sender).ID;
                        group = id.Substring(0, 7);//Get the Group ID
                        if (Session[ERP.Utilities.SessionStrings.DrControls] != null)
                        {
                            if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)
                            {
                                removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];
                            }
                            else
                            {
                                removedControlsList = new List<string>();
                            }

                            dicDrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.DrControls];
                            foreach (KeyValuePair<string, string> pair in dicDrControls)
                            {
                                if (pair.Key.StartsWith(group))
                                {
                                    if (!removedControlsList.Contains(pair.Key))//If the controls in the group is not in the removed control list, then add to it
                                        removedControlsList.Add(pair.Key);
                                }
                            }
                            Session[ERP.Utilities.SessionStrings.RemovedControls] = removedControlsList;
                            if (dicDrControls != null && dicDrControls.Count > 0)
                            {
                                foreach (string key in removedControlsList)
                                {
                                    if (dicDrControls.ContainsKey(key))//Remove the removed controls from the actual control list
                                        dicDrControls.Remove(key);
                                }
                            }
                            Session[ERP.Utilities.SessionStrings.DrControls] = dicDrControls;
                            if (Session[ERP.Utilities.SessionStrings.AccountType] != null)
                            {
                                dicAccountType = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.AccountType];
                                if (dicAccountType != null && dicAccountType.Count > 0)
                                {
                                    foreach (string key in removedControlsList)
                                    {
                                        if (dicAccountType.ContainsKey(key))//Remove the Removed controls from account type list
                                            dicAccountType.Remove(key);
                                    }
                                }
                                Session[ERP.Utilities.SessionStrings.AccountType] = dicAccountType;
                            }

                            #region Remove Cost Center Details
                            string AccountCtrlID = id.Replace("09set109", "03set103");//Account hidden field control id
                            RemoveCostCenter(AccountCtrlID);
                            #endregion
                        }
                        table = (Table)((Button)sender).Parent.Parent.Parent.Parent;
                        if (table != null)
                            divGroupDr.Controls.Remove(table);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                        //                "ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                        "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                        break;
                    #endregion
                    #region Delete Credit
                    case ActionsEnum.REMOVECREDIT:
                        id = ((Button)sender).ID;
                        group = id.Substring(0, 7);//Get the Group ID
                        if (Session[ERP.Utilities.SessionStrings.CrControls] != null)
                        {
                            if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)
                            {
                                removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];
                            }
                            else
                            {
                                removedControlsList = new List<string>();
                            }

                            dicCrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.CrControls];
                            foreach (KeyValuePair<string, string> pair in dicCrControls)
                            {
                                if (pair.Key.StartsWith(group))
                                {
                                    if (!removedControlsList.Contains(pair.Key))//If the controls in the group is not in the removed control list, then add to it
                                        removedControlsList.Add(pair.Key);
                                }
                            }
                            Session[ERP.Utilities.SessionStrings.RemovedControls] = removedControlsList;
                            if (dicCrControls != null && dicCrControls.Count > 0)
                            {
                                foreach (string key in removedControlsList)
                                {
                                    if (dicCrControls.ContainsKey(key))//Remove the removed controls from the actual control list
                                        dicCrControls.Remove(key);
                                }
                            }
                            Session[ERP.Utilities.SessionStrings.CrControls] = dicCrControls;
                            if (Session[ERP.Utilities.SessionStrings.AccountType] != null)
                            {
                                dicAccountType = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.AccountType];
                                if (dicAccountType != null && dicAccountType.Count > 0)
                                {
                                    foreach (string key in removedControlsList)
                                    {
                                        if (dicAccountType.ContainsKey(key))//Remove the removed controls from the Account Type list
                                            dicAccountType.Remove(key);
                                    }
                                }
                                Session[ERP.Utilities.SessionStrings.AccountType] = dicAccountType;
                            }

                            #region Remove Cost Center Details
                            string AccountCtrlID = id.Replace("09set109", "03set103");
                            RemoveCostCenter(AccountCtrlID);
                            #endregion
                        }

                        table = (Table)((Button)sender).Parent.Parent.Parent.Parent;
                        if (table != null)
                            divGroupCr.Controls.Remove(table);

                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                        //                "ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                        "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                        break;
                    #endregion
                    #region JOURNALACCOUNTINDEXCHANGED
                    case ActionsEnum.JOURNALACCOUNTINDEXCHANGED:
                        string senderId = ((Button)sender).ID;//Get AutoComplete Button
                        string txtAccountCtrlID = senderId.Replace("04set104", "02set102");
                        string hdfAccountCtrlID = senderId.Replace("04set104", "03set103");
                        string btnCostCenterCtrlID = senderId.Replace("04set104", "10set110");
                        string hdfCostCenterCtrlID = senderId.Replace("04set104", "11set111");
                        TextBox txtAccountCtrl = (TextBox)pnlControls.FindControl(txtAccountCtrlID);//Get AutoComplete TextBox
                        HiddenField hdfAccountCtrl = (HiddenField)pnlControls.FindControl(hdfAccountCtrlID);//Get AutoComplete Hiddenfield

                        Button btnCostCenterCtrl = (Button)pnlControls.FindControl(btnCostCenterCtrlID);//Get Cost Center button
                        HiddenField hdfCostCenterCtrl = (HiddenField)pnlControls.FindControl(hdfCostCenterCtrlID);//Get Cost Center Hiddenfield
                        btnCostCenterCtrl.Visible = false;
                        hdfCostCenterCtrl.Value = "0";

                        if (txtAccountCtrl != null && hdfAccountCtrl != null && !string.IsNullOrEmpty(hdfAccountCtrl.Value))
                        {
                            finTrxObj = CommonFunctions.Initilize<FIN_TRX>();
                            int itemPK = Convert.ToInt32(hdfAccountCtrl.Value);

                            string txtID = txtAccountCtrl.ID;
                            hdfCoaPk.Value = hdfAccountCtrl.Value;

                            if (Session[ERP.Utilities.SessionStrings.ControlInfo] != null)
                            {
                                dicControlinfo = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.ControlInfo];
                            }
                            else
                            {
                                dicControlinfo = new Dictionary<string, string>();
                            }

                            if (!dicControlinfo.ContainsKey(txtAccountCtrl.ID + "CoaPk"))
                                dicControlinfo.Add(txtAccountCtrl.ID + "CoaPk", hdfCoaPk.Value);
                            Session[ERP.Utilities.SessionStrings.ControlInfo] = dicControlinfo;

                            GetFieldValues(ControlsEnum.FINCOAMST);

                            if (finCoaMstList != null && finCoaMstList.Count > 0)
                            {
                                hdfSubTypePk.Value = finCoaMstList[0].COA_SUB_TYPE.ToString();

                                #region Check whether the cost center mapped against the chart of account
                                if (finCoaMstList[0].FIN_COA_COST_CENTER_MPG != null && finCoaMstList[0].FIN_COA_COST_CENTER_MPG.Where(r => r.ADM_COST_CENTER_MST.CNM_ACTIVE == 1).Count() > 0)
                                {
                                    btnCostCenterCtrl.Visible = true;
                                    hdfCostCenterCtrl.Value = "1";
                                }
                                else
                                {
                                    btnCostCenterCtrl.Visible = false;
                                    hdfCostCenterCtrl.Value = "0";
                                }
                                #endregion
                            }

                            GetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                            if (finCoaSubTypeCfgList[0].CST_REL_QUERY != null)
                            {
                                string childDdlID = txtID.Replace("02set1", "51set2");//Get Subledger Control ID
                                DropDownList ddlChildDrop = (DropDownList)pnlControls.FindControl(childDdlID);//Get Subledger DropDownList
                                RequiredFieldValidator tempVrfSubLedger = pnlControls.FindControl("vrf" + ddlChildDrop.ID.Replace("dic", "")) as RequiredFieldValidator;//Get Validation Control of DDl Subledger
                                if (ddlChildDrop != null)
                                {
                                    if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                                    {
                                        //Get Related Query
                                        relquery = finCoaSubTypeCfgList[0].CST_REL_QUERY == null ? string.Empty : finCoaSubTypeCfgList[0].ADM_QUERIES_CFG1.QRY_QUERY;
                                    }

                                    if (relquery != string.Empty)//Have Related Query
                                    {
                                        ddlChildDrop.Items.Clear();
                                        ddlChildDrop.DataSource = null;
                                        commonService = new CommonService();
                                        commonService = CommonFunctions.InitiateClient(commonService);

                                        if (finCoaSubTypeCfgList[0].CST_CODE == "AP" || finCoaSubTypeCfgList[0].CST_CODE == "AR")
                                        {
                                            relquery = relquery.Replace("@COA@", hdfCoaPk.Value == "" ? "NULL" : hdfCoaPk.Value);
                                            relquery = relquery.Replace("@ADV@", "NULL");
                                        }
                                        else if (finCoaSubTypeCfgList[0].CST_CODE == "ADP" || finCoaSubTypeCfgList[0].CST_CODE == "ADR")
                                        {
                                            relquery = relquery.Replace("@COA@", "NULL");
                                            relquery = relquery.Replace("@ADV@", hdfCoaPk.Value == "" ? "NULL" : hdfCoaPk.Value);
                                        }
                                        else
                                        {
                                            relquery = relquery.Replace("@COA@", "NULL");
                                            relquery = relquery.Replace("@ADV@", "NULL");
                                        }
                                        relquery = relquery.Replace("@PK@", "0");
                                        relquery = relquery.Replace("@BIZUNITPK@", currentUser.SBUID.ToString());
                                        List<DDLMaster> ddlChildValues = commonService.ExecuteQuery(relquery);//Execute Qry
                                        DDLMaster Childlst = new DDLMaster();
                                        Childlst.PK = Convert.ToInt32(CommonConstants.SELECTVAL);
                                        Childlst.Value = Resources.Report.Select;
                                        ddlChildValues.Insert(0, Childlst);//Insert Select
                                        ddlChildDrop.DataTextField = "Value";
                                        ddlChildDrop.DataValueField = "PK";
                                        ddlChildDrop.DataSource = CommonFunctions.HtmlDecode(ddlChildValues, "Value");//Bind Subledger Dropdown
                                        ddlChildDrop.DataBind();
                                        Dictionary<string, long> dicFinTrxPK = null;
                                        FIN_TRX tempFinTrxObj = null;
                                        if (Session[ERP.Utilities.SessionStrings.FinTrxPk] != null)
                                            dicFinTrxPK = (Dictionary<string, long>)Session[ERP.Utilities.SessionStrings.FinTrxPk];
                                        string key = txtID.Substring(0, 7);
                                        key = key + (txtID.EndsWith("dr") ? "dr" : "cr");//Set Key for dicFinTrxPK
                                        if (dicFinTrxPK != null && dicFinTrxPK.Count > 0
                                            && dicFinTrxPK.ContainsKey(key))
                                        {
                                            long finTrxPk = dicFinTrxPK.SingleOrDefault(aa => aa.Key == key).Value;
                                            GetFieldValues(ControlsEnum.FINHEADER);
                                            if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                                            {
                                                if (finTrxHdrList[0].FIN_TRX != null && finTrxHdrList[0].FIN_TRX.Count > 0)//hav dummy row
                                                {
                                                    tempFinTrxObj = finTrxHdrList[0].FIN_TRX.SingleOrDefault(aa => aa.FTR_PK == finTrxPk);
                                                }
                                            }
                                        }
                                        if (ddlChildDrop.Items.Count == 1)
                                        {
                                            ddlChildDrop.SelectedValue = CommonConstants.SELECTVAL;
                                        }
                                        else if (ddlChildDrop.Items.Count == 2)//If ddl have only 2 items then set default value
                                        {
                                            ddlChildDrop.SelectedValue = ddlChildDrop.Items[1].Value;
                                        }
                                        else
                                        {
                                            if (tempFinTrxObj != null)
                                            {
                                                string selVal = tempFinTrxObj.FTR_TYPE_PK != null ? tempFinTrxObj.FTR_TYPE_PK.ToString() : string.Empty;
                                                if (!string.IsNullOrEmpty(selVal) && ddlChildDrop.Items.FindByValue(selVal) != null)
                                                {
                                                    ddlChildDrop.SelectedValue = selVal;
                                                }
                                                else
                                                {
                                                    string defQuery = finCoaSubTypeCfgList[0].CST_DEF_QUERY == null ? string.Empty : finCoaSubTypeCfgList[0].ADM_QUERIES_CFG_DEF_QUERY.QRY_QUERY;
                                                    if (defQuery != string.Empty)//Have default Query
                                                    {
                                                        defQuery = defQuery.Contains(GetLocalResourceObject("DefaultQryCondtion").ToString())
                                                            ? defQuery.Replace(GetLocalResourceObject("DefaultQryCondtion").ToString(), hdfCoaPk.Value) : defQuery;
                                                        commonService = CommonFunctions.InitiateClient(commonService);
                                                        List<DDLMaster> defaultValuesList = commonService.ExecuteQuery(defQuery);
                                                        if (defaultValuesList != null && defaultValuesList.Count == 1)
                                                        {
                                                            ddlChildDrop.SelectedValue = (ListItem)ddlChildDrop.Items.FindByValue(defaultValuesList[0].PK.ToString()) != null
                                                                ? defaultValuesList[0].PK.ToString() : CommonConstants.SELECTVAL;//Set default value to subledger ddl
                                                        }
                                                        else
                                                        {
                                                            ddlChildDrop.SelectedValue = CommonConstants.SELECTVAL;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ddlChildDrop.SelectedValue = CommonConstants.SELECTVAL;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                string defQuery = finCoaSubTypeCfgList[0].CST_DEF_QUERY == null ? string.Empty : finCoaSubTypeCfgList[0].ADM_QUERIES_CFG_DEF_QUERY.QRY_QUERY;
                                                if (defQuery != string.Empty)
                                                {
                                                    defQuery = defQuery.Contains(GetLocalResourceObject("DefaultQryCondtion").ToString())
                                                           ? defQuery.Replace(GetLocalResourceObject("DefaultQryCondtion").ToString(), hdfCoaPk.Value) : defQuery;
                                                    commonService = CommonFunctions.InitiateClient(commonService);
                                                    List<DDLMaster> defaultValuesList = commonService.ExecuteQuery(defQuery);
                                                    if (defaultValuesList != null && defaultValuesList.Count == 1)
                                                    {
                                                        ddlChildDrop.SelectedValue = (ListItem)ddlChildDrop.Items.FindByValue(defaultValuesList[0].PK.ToString()) != null
                                                            ? defaultValuesList[0].PK.ToString() : CommonConstants.SELECTVAL;//Set default value to subledger ddl
                                                    }
                                                    else
                                                    {
                                                        ddlChildDrop.SelectedValue = CommonConstants.SELECTVAL;
                                                    }
                                                }
                                                else
                                                {
                                                    ddlChildDrop.SelectedValue = CommonConstants.SELECTVAL;
                                                }
                                            }
                                        }
                                        ddlChildDrop.Visible = true;

                                        if (tempVrfSubLedger != null)
                                            tempVrfSubLedger.Enabled = true;//Enable Subledger Validation Control
                                        string childLabelId = ddlChildDrop.ID.Replace("51set202", "50set201");
                                        Label lblChild = (Label)pnlControls.FindControl(childLabelId);
                                        lblChild.Visible = true;

                                        if (finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("Bank").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PDC").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PPC").ToString()))
                                        {
                                            string instID = ddlChildDrop.ID.Replace("51set202", "52set203");
                                            TextBox txtInsNo = (TextBox)pnlControls.FindControl(instID);//Find Instrument No Textbox

                                            txtInsNo.Visible = true;
                                            string dateID = ddlChildDrop.ID.Replace("51set202", "53set204");
                                            TextBox txtdat = (TextBox)pnlControls.FindControl(dateID);//Find Date Textbox
                                            txtdat.Visible = true;
                                            string favourID = ddlChildDrop.ID.Replace("51set202", "54set205");
                                            TextBox txtfavour = (TextBox)pnlControls.FindControl(favourID);//Find Favour of Textbox
                                            txtfavour.Visible = true;
                                            if (!string.IsNullOrEmpty(txtdat.ID))
                                            {
                                                string PageScript = CommonFunctions.GenerateDynamicScript("Date", txtdat.ID, null, null, null);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtdat.ID + "", PageScript, true);//Register Datepicker script
                                            }
                                            if (tempFinTrxObj != null)
                                            {
                                                txtInsNo.Text = HttpUtility.HtmlDecode(tempFinTrxObj.FTR_INSTR_NO);
                                                if (string.IsNullOrEmpty(txtInsNo.Text.Trim()))
                                                    txtInsNo.Text = GetLocalResourceObject("InstrumentNo").ToString();
                                                txtdat.Text = tempFinTrxObj.FTR_INSTR_DATE == null ?
                                                    DateTime.Now.ToString(Resources.Constants.DateFormatShort) :
                                                    ((DateTime)tempFinTrxObj.FTR_INSTR_DATE).ToString(Resources.Constants.DateFormatShort);
                                                if (string.IsNullOrEmpty(txtdat.Text.Trim()))
                                                    txtdat.Text = GetLocalResourceObject("Date").ToString();
                                                txtfavour.Text = HttpUtility.HtmlDecode(tempFinTrxObj.FTR_INSTR_FAVOUR);
                                                if (string.IsNullOrEmpty(txtfavour.Text.Trim()))
                                                    txtfavour.Text = GetLocalResourceObject("FavourOf").ToString();
                                            }
                                            else
                                            {
                                                if (string.IsNullOrEmpty(txtInsNo.Text.Trim()))
                                                    txtInsNo.Text = GetLocalResourceObject("InstrumentNo").ToString();
                                                if (string.IsNullOrEmpty(txtdat.Text.Trim()))
                                                    txtdat.Text = GetLocalResourceObject("Date").ToString();
                                                if (string.IsNullOrEmpty(txtfavour.Text.Trim()))
                                                    txtfavour.Text = GetLocalResourceObject("FavourOf").ToString();
                                            }
                                        }
                                        else
                                        {
                                            string instID = ddlChildDrop.ID.Replace("51set202", "52set203");
                                            TextBox txtInsNo = (TextBox)pnlControls.FindControl(instID);//Find Instrument No Textbox
                                            txtInsNo.Visible = false;
                                            string dateID = ddlChildDrop.ID.Replace("51set202", "53set204");
                                            TextBox txtdat = (TextBox)pnlControls.FindControl(dateID);//Find Date Textbox
                                            txtdat.Visible = false;
                                            string favourID = ddlChildDrop.ID.Replace("51set202", "54set205");
                                            TextBox txtfavour = (TextBox)pnlControls.FindControl(favourID);//Find Favour of Textbox
                                            txtfavour.Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        string childLabelId = ddlChildDrop.ID.Replace("51set202", "50set201");
                                        Label lblChild = (Label)pnlControls.FindControl(childLabelId);//Find Subledger Label
                                        lblChild.Visible = false;
                                        ddlChildDrop.Visible = false;//Hide Subledger ddl
                                        if (tempVrfSubLedger != null)
                                            tempVrfSubLedger.Enabled = false;//Disable Subledger validation Control
                                        string instID = ddlChildDrop.ID.Replace("51set202", "52set203");
                                        TextBox txtInsNo = (TextBox)pnlControls.FindControl(instID);//Find Instrument No Textbox
                                        txtInsNo.Visible = false;
                                        string dateID = ddlChildDrop.ID.Replace("51set202", "53set204");
                                        TextBox txtdat = (TextBox)pnlControls.FindControl(dateID);//Find Date Textbox
                                        txtdat.Visible = false;
                                        string favourID = ddlChildDrop.ID.Replace("51set202", "54set205");
                                        TextBox txtfavour = (TextBox)pnlControls.FindControl(favourID);//Find Favour of Textbox
                                        txtfavour.Visible = false;
                                    }
                                }
                            }
                            else
                            {
                                string childDdlID = txtID.Replace("02set1", "51set2");
                                DropDownList ddlChildDrop = (DropDownList)pnlControls.FindControl(childDdlID);//Find Subledger ddl
                                RequiredFieldValidator tempVrfSubLedger = pnlControls.FindControl("vrf" + ddlChildDrop.ID.Replace("dic", "")) as RequiredFieldValidator;//Find Subledger validation Control
                                if (ddlChildDrop != null)
                                {
                                    string childLabelId = ddlChildDrop.ID.Replace("51set202", "50set201");
                                    Label lblChild = (Label)pnlControls.FindControl(childLabelId);//Find Subledger Label
                                    lblChild.Visible = false;
                                    ddlChildDrop.Visible = false;//Hide Subledger ddl
                                    if (tempVrfSubLedger != null)
                                        tempVrfSubLedger.Enabled = false;//Disable Subledger validation Control
                                    string instID = ddlChildDrop.ID.Replace("51set202", "52set203");
                                    TextBox txtInsNo = (TextBox)pnlControls.FindControl(instID);//Find Instrument No Textbox
                                    txtInsNo.Visible = false;
                                    string dateID = ddlChildDrop.ID.Replace("51set202", "53set204");
                                    TextBox txtdat = (TextBox)pnlControls.FindControl(dateID);//Find Date Textbox
                                    txtdat.Visible = false;
                                    string favourID = ddlChildDrop.ID.Replace("51set202", "54set205");
                                    TextBox txtfavour = (TextBox)pnlControls.FindControl(favourID);//Find Favour of Textbox
                                    txtfavour.Visible = false;
                                }
                            }
                        }
                        else if (txtAccountCtrl != null && hdfAccountCtrl != null && string.IsNullOrEmpty(hdfAccountCtrl.Value))
                        {
                            string childDdlID = txtAccountCtrlID.Replace("02set1", "51set2");
                            DropDownList ddlChildDrop = (DropDownList)pnlControls.FindControl(childDdlID);//Find Subledger ddl
                            RequiredFieldValidator tempVrfSubLedger = pnlControls.FindControl("vrf" + ddlChildDrop.ID.Replace("dic", "")) as RequiredFieldValidator;//Find Subledger validation Control
                            if (ddlChildDrop != null)
                            {
                                string childLabelId = ddlChildDrop.ID.Replace("51set202", "50set201");
                                Label lblChild = (Label)pnlControls.FindControl(childLabelId);
                                lblChild.Visible = false;
                                ddlChildDrop.Visible = false;//Hide Subledger ddl
                                if (tempVrfSubLedger != null)
                                    tempVrfSubLedger.Enabled = false;//Disable Subledger validation Control
                                string instID = ddlChildDrop.ID.Replace("51set202", "52set203");
                                TextBox txtInsNo = (TextBox)pnlControls.FindControl(instID);//Find Instrument No Textbox
                                txtInsNo.Visible = false;
                                string dateID = ddlChildDrop.ID.Replace("51set202", "53set204");
                                TextBox txtdat = (TextBox)pnlControls.FindControl(dateID);//Find Date Textbox
                                txtdat.Visible = false;
                                string favourID = ddlChildDrop.ID.Replace("51set202", "54set205");
                                TextBox txtfavour = (TextBox)pnlControls.FindControl(favourID);//Find Favour of Textbox
                                txtfavour.Visible = false;
                            }
                        }

                        #region Remove Cost Center Details
                        if (hdfAccountCtrl != null)
                        {
                            RemoveCostCenter(hdfAccountCtrl.ID);
                        }
                        #endregion

                        if (txtAccountCtrl != null)
                            txtAccountCtrl.Focus();
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                        //                "ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                        "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                        break;
                    #endregion
                    #region Change Date
                    case ActionsEnum.CHANGEDATE:
                        DateTime trxDate;
                        if (!DateTime.TryParse(txtTaxDate.Text.Trim(), out trxDate))
                            throw new InvalidCastException("Invalid Date");

                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                        try
                        {
                            finTrxHdrObj = (FIN_TRX_HDR)SetUIValuesToObject(ActionsEnum.CHANGEDATE);
                            DateTime? lastModDate = finTrxServiceClient.ChangeTaxDate(finTrxHdrObj);
                            if (lastModDate.HasValue)
                            {
                                this.LastModifiedTime = lastModDate.Value;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop01",
                                                   "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                                litErrorMsg.Text = GetLocalResourceObject("Change_Date_Success").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup();ShowErrorMsg01", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Concurency Exception Handles Here
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region SAVE JOURNALIZE
                    case ActionsEnum.SAVE:
                        if (!this.Page.IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else
                        {
                            dicDrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.DrControls];//Get Debit Controls
                            dicCrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.CrControls];//Get Credit Controls
                            if (dicDrControls != null && dicDrControls.Count() > 0 && dicCrControls != null && dicCrControls.Count() > 0)
                            {
                                CCInvalidSplitControlList = new List<CostCenterMissmatch>();
                                finTrxList = new List<FIN_TRX>();
                                finTrxHdrList = new List<FIN_TRX_HDR>();
                                finTrxServiceClient = new FinTrxService();
                                finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                finTrxHdrObj = (FIN_TRX_HDR)SetUIValuesToObject(ActionsEnum.SAVE);
                                if (finTrxHdrObj != null)
                                {
                                    #region Validation for cost center split missmatch
                                    if (GetGlobalResourceObject("ConfigurationsRes", "CCPercentageRequired").ToString() == "1" || GetGlobalResourceObject("ConfigurationsRes", "EnableCostCenter").ToString() == "1")
                                    {
                                        if (CCInvalidSplitControlList != null && CCInvalidSplitControlList.Count > 0)
                                        {
                                            CCInvalidSplitControlList.ForEach(dtl =>
                                            {
                                                TextBox txtamnttc = (TextBox)divGroupHeadCr.FindControl(dtl.AMT_TC_CONTROL_ID);
                                                if (txtamnttc != null)
                                                {
                                                    txtamnttc.CssClass = txtamnttc.CssClass.Replace("tempcss", "border-red");// "input-w8per numeric border-red";
                                                }
                                            });

                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                                         "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_CC_SplitMissmatch").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            return;
                                        }
                                    }
                                    #endregion

                                    #region Checking for voucher locked with financial year
                                    if (finTrxHdrObj.FTH_DATE.HasValue)
                                    {
                                        string LockUptoDate = string.Empty;
                                        if (BusinessLogic.Finance.VoucherLockingBL.IsVoucherLocked(finTrxHdrObj.FTH_DATE.Value, JurCurrPK, finTrxHdrObj.FTH_BIZUNIT, ref LockUptoDate))
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                                     "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                                            litErrorMsg.Text = GetLocalResourceObject("Err_Voucher_Locked").ToString() + (!string.IsNullOrEmpty(LockUptoDate) ? DateTime.Parse(LockUptoDate).ToString(Resources.Constants.DateFormatShort) : string.Empty);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            return;
                                        }
                                    }
                                    #endregion

                                    #region Check same account exist in debit and credit
                                    var objTrxList = from c in finTrxHdrObj.FIN_TRX
                                                     group c by new { c.FTR_ACCOUNT, c.FTR_TYPE_PK } into g
                                                     select new
                                                     {
                                                         Account = g.Key.FTR_ACCOUNT,
                                                         SubTypePk = g.Key.FTR_TYPE_PK,
                                                         Amount = g.Sum(r => r.FTR_DR_AMT_BC) - g.Sum(r => r.FTR_CR_AMT_BC)
                                                     };
                                    if (objTrxList != null && objTrxList.Where(r => r.Amount == 0).Count() > 0)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                              "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                                        litErrorMsg.Text = GetLocalResourceObject("Err_Same_Account").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        return;
                                    }
                                    #endregion

                                    if (finTrxHdrObj.FTH_REF_TYPE == ApplicationType.SIJ && finTrxHdrObj.FTH_PK > 0 && finTrxHdrObj.FTH_STATUS != 0)
                                    {
                                        if (finTrxServiceClient.CheckForReceiptVoucherCreated(finTrxHdrObj))
                                        {
                                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                            //    "ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                                "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                                            litErrorMsg.Text = GetLocalResourceObject("Err_Voucher_Cancel").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            return;
                                        }
                                    }

                                    //Check for year end voucher created 
                                    if (finTrxServiceClient.CheckForYearendVoucherCreated(finTrxHdrObj))
                                    {
                                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                        //    "ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                            "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                                        litErrorMsg.Text = GetLocalResourceObject("Err_Voucher_Cancel").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        return;
                                    }
                                    if (finTrxHdrObj.FIN_TRX.Count > 0)
                                    {
                                        finTrxHdrList.Add(finTrxHdrObj);
                                        decimal tcDr = 0;
                                        decimal tcCr = 0;
                                        decimal bcDr = 0;
                                        decimal bcCr = 0;
                                        bool IsContinue = false;
                                        decimal.TryParse(txtDrTotal.Text.Trim(), out tcDr);//Get Debit Total in TC
                                        decimal.TryParse(txtCrTotal.Text.Trim(), out tcCr);//Get Credit Total in TC
                                        if (txtDrTotalBC.Visible && txtCrTotalBC.Visible)//If TC is Foreign currency
                                        {
                                            decimal.TryParse(txtDrTotalBC.Text.Trim(), out bcDr);//Get Debit Total in BC
                                            decimal.TryParse(txtCrTotalBC.Text.Trim(), out bcCr);//Get Credit Total in BC
                                        }
                                        string AppType = Session[ERP.Utilities.SessionStrings.TransactionType].ToString();
                                        //if (AppType == ApplicationType.PIJYE || AppType == ApplicationType.VPJYE || AppType == ApplicationType.SIJYE || AppType == ApplicationType.CRJYE || AppType == ApplicationType.EIJYE || AppType == ApplicationType.PSIJYE || AppType == ApplicationType.EIPJYE || AppType == ApplicationType.SIPJYE || AppType == ApplicationType.MSIJYE || AppType == ApplicationType.MSIRJYE || AppType == ApplicationType.FCHRJYE || AppType == ApplicationType.CNSJYE || AppType == ApplicationType.DNSJYE || AppType == ApplicationType.CNPJYE || AppType == ApplicationType.DNPJYE)
                                        if (IsYearEndVoucher())
                                        {
                                            if ((tcDr == tcCr)
                                               && (txtDrTotalBC.Visible && txtCrTotalBC.Visible ? (bcDr == bcCr && bcDr > 0 && bcCr > 0) : true))//If Total Amount in Debit and credit are equal and allow zero(ie transaction currency) for year end vouchers
                                            {
                                                IsContinue = true;
                                            }
                                        }
                                        else
                                        {
                                            if ((tcDr == tcCr && tcDr > 0 && tcCr > 0)
                                               && (txtDrTotalBC.Visible && txtCrTotalBC.Visible ? (bcDr == bcCr && bcDr > 0 && bcCr > 0) : true))//If Total Amount in Debit and credit are equal
                                            {
                                                IsContinue = true;
                                            }
                                        }

                                        //if ((tcDr == tcCr && tcDr > 0 && tcCr > 0)
                                        //   && (txtDrTotalBC.Visible && txtCrTotalBC.Visible ? (bcDr == bcCr && bcDr > 0 && bcCr > 0) : true))//If Total Amount in Debit and credit are equal


                                        if (IsContinue)//If Total Amount in Debit and credit are equal
                                        {
                                            pettyCashAmountLimit = 0;
                                            if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.PCS)//If Transaction Type is PettyCash
                                            {
                                                GetFieldValues(ControlsEnum.PETTYCASHVOUCHERLIMIT);//Get Pettycash voucher limit
                                                if (admAppConfigMstList != null && admAppConfigMstList.Count == 1)
                                                {
                                                    pettyCashAmountLimit = Convert.ToDecimal(admAppConfigMstList[0].ACF_DATA);
                                                    if (Convert.ToDecimal(txtDrTotal.Text.Trim()) > pettyCashAmountLimit)
                                                        isValidAmount = false;//If total amount is greater than pettycash voucher limit, then set isValidAmount as false
                                                }
                                            }
                                            if (isValidAmount)
                                            {
                                                #region Manual Voucher Number
                                                if (AST_DOC_MODE.Value == ((int)DOCMODE.Manual).ToString())
                                                {
                                                    hdfVoucherNo.Value = txtDispPVNo.Text.Trim();
                                                    finTrxHdrList[0].FTH_VOUCHER_NO = HttpUtility.HtmlEncode(hdfVoucherNo.Value);
                                                }
                                                #endregion

                                                bool isValidVoucherNo = true;
                                                if (AST_DOC_MODE.Value == ((int)DOCMODE.Manual).ToString())
                                                {
                                                    //Check for voucher no duplication
                                                    isValidVoucherNo = finTrxServiceClient.CheckForVoucherNoDuplication(JurCurrPK, finTrxHdrList[0].FTH_VOUCHER_NO);
                                                }
                                                if (isValidVoucherNo)
                                                {
                                                    int Archiveresult = 1;
                                                    //if (JurCurrPK > 0)
                                                    //{
                                                    //    //For each update keep version details of voucher for Audit trail 
                                                    //    Archiveresult = BusinessLogic.Jouralize.JournalizeBL.SaveVoucherArchiveDetails(JurCurrPK);
                                                    //}
                                                    if ((Archiveresult == 1 && JurCurrPK > 0) || JurCurrPK == 0)// For journal voucher(CurrPK is zero in mode).
                                                    {

                                                        int retRefID = 0;
                                                        string trxNo = string.Empty;
                                                        string AsrCode = string.Empty;
                                                        FinTrxHeaderBO finTrxHeaderObj = GetFinTrxHdrObject(finTrxHdrList[0]);
                                                        finTrxHeaderObj.WKF_FLAG = 0;
                                                        finTrxHeaderObj.FTH_STATUS = 0;
                                                        finTrxHeaderObj.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                                        if (finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.PPCCJ)
                                                        {
                                                            finTrxHeaderObj.PDCFLAG = "2";
                                                        }
                                                        if (finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.PPCCTJ)
                                                        {
                                                            finTrxHeaderObj.PDCFLAG = "2";
                                                        }
                                                        if (finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.RCBJ
                                                            || finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.RCBTJ)
                                                        {
                                                            finTrxHeaderObj.BOUNCEFLAG = "1";
                                                        }
                                                        if (finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.PDCCJ
                                                            || finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.PDCCTJ)
                                                        {
                                                            finTrxHeaderObj.PDCFLAG = "2";
                                                        }
                                                        if (finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.DPVCJ)
                                                        {
                                                            finTrxHeaderObj.PDCFLAG = "2";
                                                        }
                                                        if (finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.PIJ || finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.EIJ || finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.PSIJ)
                                                        {
                                                            finTrxHeaderObj.UPDATE_INV_STOCK = "1";
                                                        }
                                                        string xmlDocSave = CommonFunctions.XmlSerialize<FinTrxHeaderBO>(finTrxHeaderObj);
                                                        jurResult = BusinessLogic.Jouralize.JournalizeBL.SaveJournalVoucher(xmlDocSave, out retRefID, out trxNo, out AsrCode);
                                                        // jurResult = finTrxServiceClient.SaveFinTrx(finTrxHdrList);
                                                        if (jurResult > 0) // Success ! re-initialize the page
                                                        {

                                                            if (JournalType == (int)JournalTypeEnum.Voucher)//If Journal Type is Voucher
                                                            {
                                                                ((Button)sender).CommandName = ActionsEnum.JOURNALIZESAVE.ToString();//Assign new Action
                                                                JournalizeSave(sender, e);//Call action in the popup calling page
                                                            }
                                                            else if (JournalType == (int)JournalTypeEnum.Reverse)//If Journal Type is Reverse Voucher
                                                            {
                                                                ((Button)sender).CommandName = ActionsEnum.REVERSESAVE.ToString();//Assign new Action
                                                                ReverseSave(sender, e);//Call action in the popup calling page
                                                            }
                                                            else if (JournalType == (int)JournalTypeEnum.Return)//If Journal Type is Return Voucher
                                                            {
                                                                ((Button)sender).CommandName = ActionsEnum.RETURNSAVE.ToString();//Assign new Action
                                                                ReturnSave(sender, e);//Call action in the popup calling page
                                                            }
                                                            ((Button)sender).CommandName = ActionsEnum.SAVE.ToString();//Re-Assign original Action
                                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                                                            divGroupDrHdr.Controls.Clear();//Clear Debit Hdr section
                                                            divGroupCrHdr.Controls.Clear();//Clear Credit Hdr section
                                                            divGroupDr.Controls.Clear();//Clear Debit Controls
                                                            divGroupCr.Controls.Clear();//Clear Credit Controls
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                "ClosePopup();", true);
                                                            //Session[ERP.Utilities.SessionStrings.JournalHead].ToString();

                                                            //Listing and detail pages are different
                                                            string appType = string.Empty;
                                                            if (Session[ERP.Utilities.SessionStrings.TransactionType] != null)
                                                                appType = Session[ERP.Utilities.SessionStrings.TransactionType].ToString();
                                                            switch (appType)
                                                            {
                                                                case ApplicationType.DPVCJ:
                                                                case ApplicationType.DPBJ:
                                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                                           + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.DirectPaymentList) + "');", true);
                                                                    break;
                                                                default:
                                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError",
                                                                "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                                    break;
                                                            }
                                                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError",
                                                            //    "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                            ClearVoucher();
                                                        }
                                                        else if (jurResult == -111)//If the voucher Date year is selected is not same as the current financial year
                                                        {
                                                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                                                            //    "ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                                                                "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Financial_Year").ToString();
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                        }
                                                        else if (jurResult == -113)//Cr amount and Dr amount not tally
                                                        {
                                                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                                                            //    "ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                                                                "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                                                            litErrorMsg.Text = GetLocalResourceObject("Msg_ErrAmount_NotValid").ToString();
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                        }
                                                        else if (jurResult == (int)DbSaveStatus.FINYEARNOTEXIST)//Financial year not entered
                                                        {
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                                                                "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Financial_Year_Notentered").ToString();
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                        }
                                                        else if (jurResult == (int)DbSaveStatus.ALREADYREFERRED)//Alredy referred in another place
                                                        {
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                                            "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                                                            litErrorMsg.Text = GetLocalResourceObject("Err_Voucher_Cancel").ToString();
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                                    }
                                                }//If not a valid voucher no
                                                else
                                                {
                                                    // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                                                    //"ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                                                   "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Duplicate_VoucherNo").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                }
                                            }//If not a valid amount. It wil be only in the case of PCS
                                            else
                                            {
                                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                                                //    "ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                                                    "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                                                litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_ErrAmount_Exceed").ToString(), pettyCashAmountLimit);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            }
                                        }//If total amount is not tallied
                                        else
                                        {
                                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                            //    "ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                                "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                                            litErrorMsg.Text = GetLocalResourceObject("Msg_ErrAmount_NotValid").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        }
                                    }
                                    else
                                    {
                                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                        //    "ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                            "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                                        litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Journal").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                }
                            }
                            else
                            {
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                                //                "ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                                                "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                                litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_ErrSave_Journal").ToString(), pettyCashAmountLimit);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region Delete Journalize
                    case ActionsEnum.DELETE:
                        dicDrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.DrControls];//Get Debit Controls
                        dicCrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.CrControls];//Get Credit Controls
                        if (dicDrControls != null && dicDrControls.Count() > 0 && dicCrControls != null && dicCrControls.Count() > 0)//If DR and CR section Have controls
                        {
                            finTrxList = new List<FIN_TRX>();
                            finTrxHdrList = new List<FIN_TRX_HDR>();
                            finTrxServiceClient = new FinTrxService();
                            finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                            finTrxHdrObj = (FIN_TRX_HDR)SetUIValuesToObject(ActionsEnum.SAVE);
                            if (finTrxHdrObj != null)
                            {
                                if (finTrxHdrObj.FIN_TRX.Count > 0)
                                {
                                    finTrxServiceClient = new FinTrxService();
                                    finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                    if (Session[ERP.Utilities.SessionStrings.JournalizePK] != null)
                                    {
                                        //Delete Voucher
                                        jurResult = finTrxServiceClient.DeleteFinTrx(Session[ERP.Utilities.SessionStrings.TransactionType].ToString(), 0,
                                                            Convert.ToInt32(Session[ERP.Utilities.SessionStrings.JournalizePK].ToString()));
                                    }
                                    else
                                    {
                                        //Delete Voucher
                                        jurResult = finTrxServiceClient.DeleteFinTrx(Session[ERP.Utilities.SessionStrings.TransactionType].ToString(),
                                                            Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), 0);
                                    }
                                    if (jurResult > 0)//Deletion Success
                                    {
                                        if (Session[ERP.Utilities.SessionStrings.TransactionPK] != null)//If voucher was taken from any transaction
                                        {
                                            poInvoiceServiceClient = new POInvoiceService();
                                            poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                                            jurResult = poInvoiceServiceClient.SaveFinTrxDetails(
                                                Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()),
                                                Session[ERP.Utilities.SessionStrings.TransactionType].ToString());//Create Dummy Voucher
                                        }
                                        if (finTrxHdrObj.FTH_REF_TYPE == ApplicationType.DPVCJ)
                                        {
                                            UpdateDirectPaymentHdrPDC((int)finTrxHdrObj.FTH_REF_PK, 1);
                                        }
                                        if (finTrxHdrObj.FTH_REF_TYPE == ApplicationType.PPCCJ)
                                        {
                                            UpdatePdc((int)finTrxHdrObj.FTH_REF_PK, 1);
                                        }
                                        if (finTrxHdrObj.FTH_REF_TYPE == ApplicationType.PPCCTJ)
                                        {
                                            UpdatePdc((int)finTrxHdrObj.FTH_REF_PK, 1);
                                        }
                                        if (finTrxHdrObj.FTH_REF_TYPE == ApplicationType.PDCCJ
                                            || finTrxHdrObj.FTH_REF_TYPE == ApplicationType.PDCCTJ)
                                        {
                                            UpdateReceiptHdrPDC((int)finTrxHdrObj.FTH_REF_PK, 1);
                                        }

                                        Session[ERP.Utilities.SessionStrings.Transaction] = "DELETE";
                                        if (JournalType == (int)JournalTypeEnum.Voucher)
                                        {
                                            ((Button)sender).CommandName = ActionsEnum.JOURNALIZEDELETE.ToString();//Assign new Action
                                            JournalizeDelete(sender, e);//Call action in the popup calling page
                                        }
                                        else if (JournalType == (int)JournalTypeEnum.Reverse)
                                        {
                                            ((Button)sender).CommandName = ActionsEnum.REVERSEDELETE.ToString();//Assign new Action
                                            ReverseDelete(sender, e);//Call action in the popup calling page
                                        }
                                        else if (JournalType == (int)JournalTypeEnum.Return)
                                        {
                                            ((Button)sender).CommandName = ActionsEnum.RETURNDELETE.ToString();//Assign new Action
                                            ReturnDelete(sender, e);//Call action in the popup calling page
                                        }
                                        ((Button)sender).CommandName = ActionsEnum.DELETE.ToString();//Re-Assign original Action
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Delete_Success").ToString();
                                        divGroupDrHdr.Controls.Clear();//Clear Debit Hdr section
                                        divGroupCrHdr.Controls.Clear();//Clear Credit Hdr section
                                        divGroupDr.Controls.Clear();//Clear Debit Controls
                                        divGroupCr.Controls.Clear();//Clear Credit Controls

                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                   "ClosePopup();", true);

                                        //Listing and detail pages are different
                                        string appType = string.Empty;
                                        if (Session[ERP.Utilities.SessionStrings.TransactionType] != null)
                                            appType = Session[ERP.Utilities.SessionStrings.TransactionType].ToString();
                                        switch (appType)
                                        {
                                            case ApplicationType.DPVCJ:
                                            case ApplicationType.DPBJ:
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                       + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.DirectPaymentList) + "');", true);
                                                break;
                                            default:
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError",
                                           "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                break;
                                        }
                                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError",
                                        //    "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);

                                        ClearVoucher();
                                    }
                                }
                                else
                                {
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                    //    "ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                        "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                                    litErrorMsg.Text = GetLocalResourceObject("Msg_ErrDelete_Journal").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                            //                "ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                                           "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                            litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_ErrDelete_Journal").ToString(), pettyCashAmountLimit);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region CANCEL JOURNALIZE
                    case ActionsEnum.CANCEL:
                        Session[ERP.Utilities.SessionStrings.Transaction] = "CANCEL";
                        if (JournalType == (int)JournalTypeEnum.Voucher)
                        {
                            #region Cancel Voucher
                            //While click on cancel button, need to cancel that voucher if it is a drafted year end voucher 
                            if (IsYearEndVoucher() && (string.IsNullOrEmpty(txtDispPVNo.Text) || txtDispPVNo.Text.Equals(GetLocalResourceObject("NewLabel").ToString())))
                            {
                                int? CancelResult = BusinessLogic.Finance.YearEndVoucherBL.CancelVoucher(JurCurrPK, 0);
                            }
                            #endregion

                            ((Button)sender).CommandName = ActionsEnum.JOURNALIZECANCEL.ToString();//Assign new Action
                            JournalizeCancel(sender, e);//Call action in the popup calling page
                        }
                        else if (JournalType == (int)JournalTypeEnum.Reverse)
                        {
                            ((Button)sender).CommandName = ActionsEnum.REVERSECANCEL.ToString();//Assign new Action
                            ReverseCancel(sender, e);//Call action in the popup calling page
                        }
                        else if (JournalType == (int)JournalTypeEnum.Return)
                        {
                            ((Button)sender).CommandName = ActionsEnum.RETURNCANCEL.ToString();//Assign new Action
                            ReturnCancel(sender, e);//Call action in the popup calling page
                        }
                        ((Button)sender).CommandName = ActionsEnum.CANCEL.ToString();//Re-Assign original Action
                        divGroupDrHdr.Controls.Clear();//Clear Debit Hdr section
                        divGroupCrHdr.Controls.Clear();//Clear Credit Hdr section
                        divGroupDr.Controls.Clear();//Clear Debit Controls
                        divGroupCr.Controls.Clear();//Clear Credit Controls
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                      "ClosePopup();", true);
                        ClearVoucher();
                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        dicDrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.DrControls];//Get Debit Controls
                        dicCrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.CrControls];//Get Credit Controls
                        if (dicDrControls != null && dicDrControls.Count() > 0 && dicCrControls != null && dicCrControls.Count() > 0)
                        {
                            if ((!string.IsNullOrEmpty(txtDrTotal.Text.Trim()) && !string.IsNullOrEmpty(txtCrTotal.Text.Trim()))
                                && (Convert.ToDecimal(txtDrTotal.Text.Trim()) == Convert.ToDecimal(txtCrTotal.Text.Trim()))
                                && (Convert.ToDecimal(txtDrTotal.Text.Trim()) > 0 && Convert.ToDecimal(txtCrTotal.Text.Trim()) > 0)
                                && (txtDrTotalBC.Visible && txtCrTotalBC.Visible) ?
                                ((!string.IsNullOrEmpty(txtDrTotalBC.Text.Trim()) && !string.IsNullOrEmpty(txtCrTotalBC.Text.Trim()))
                                && (Convert.ToDecimal(txtDrTotalBC.Text.Trim()) == Convert.ToDecimal(txtCrTotalBC.Text.Trim()))
                                && (Convert.ToDecimal(txtDrTotalBC.Text.Trim()) > 0 && Convert.ToDecimal(txtCrTotalBC.Text.Trim()) > 0))
                                : true)//If total amounts of Debit and Credit Sections are tallied
                            {
                                pettyCashAmountLimit = 0;
                                if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.PCS)
                                {
                                    GetFieldValues(ControlsEnum.PETTYCASHVOUCHERLIMIT);//Get Pettycash voucher limit
                                    if (admAppConfigMstList != null && admAppConfigMstList.Count == 1)
                                    {
                                        pettyCashAmountLimit = Convert.ToDecimal(admAppConfigMstList[0].ACF_DATA);
                                        if (Convert.ToDecimal(txtDrTotal.Text.Trim()) > pettyCashAmountLimit)
                                            isValidAmount = false;//If total amount is greater than pettycash voucher limit, then set isValidAmount as false
                                    }
                                }
                                if (isValidAmount)
                                {
                                    //Show WorkFlow Popup
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                                    this.Parent.FindControl("ucrWrkf").Visible = true;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit",
                                        "ClosePopup();ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                                }
                                else
                                {
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                    //    "ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                        "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                                    litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_ErrAmount_Exceed").ToString(), pettyCashAmountLimit);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            else//If total amounts of Debit and Credit Sections are not tallied
                            {
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                //"ClosePopup();ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                "ClosePopup();ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                                if (Convert.ToDecimal(txtDrTotal.Text.Trim()) == 0 && Convert.ToDecimal(txtCrTotal.Text.Trim()) == 0)//If amounts are tallied but amount is 0
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Journal").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_ErrAmount_NotValid").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                            //                "ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                                            "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                            litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_ErrSave_Journal").ToString(), pettyCashAmountLimit);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region DELETESUBMIT popup
                    case ActionsEnum.DELETESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        this.Parent.FindControl("ucrWrkf").Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit",
                                        "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        this.Parent.FindControl("ucrWrkf").Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit",
                            "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    case ActionsEnum.WRKFSUBMIT:
                        //Submit Activity 
                        ucrWrkf.ApplicationID = 0;
                        #region New Workflow Submit
                        if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)//If Save & Submit
                        {
                            #region Save & Submit
                            if (Convert.ToDecimal(txtDrTotal.Text.Trim()) == Convert.ToDecimal(txtCrTotal.Text.Trim())
                                       && (txtDrTotalBC.Visible && txtCrTotalBC.Visible ? (Convert.ToDecimal(txtDrTotalBC.Text.Trim()) == Convert.ToDecimal(txtCrTotalBC.Text.Trim())) : true))//If Total Amount in Debit and credit are equal
                            {
                                CCInvalidSplitControlList = new List<CostCenterMissmatch>();
                                finTrxList = new List<FIN_TRX>();
                                finTrxHdrList = new List<FIN_TRX_HDR>();
                                finTrxHdrObj = (FIN_TRX_HDR)SetUIValuesToObject(ActionsEnum.WRKFSUBMIT);//Set values from UI
                                if (finTrxHdrObj != null)
                                {
                                    #region Validation for cost center split missmatch
                                    if (GetGlobalResourceObject("ConfigurationsRes", "CCPercentageRequired").ToString() == "1" || GetGlobalResourceObject("ConfigurationsRes", "EnableCostCenter").ToString() == "1")
                                    {
                                        if (CCInvalidSplitControlList != null && CCInvalidSplitControlList.Count > 0)
                                        {
                                            CCInvalidSplitControlList.ForEach(dtl =>
                                            {
                                                TextBox txtamnttc = (TextBox)divGroupHeadCr.FindControl(dtl.AMT_TC_CONTROL_ID);
                                                if (txtamnttc != null)
                                                {
                                                    txtamnttc.CssClass = txtamnttc.CssClass.Replace("tempcss", "border-red"); //"input-w8per numeric border-red";
                                                }
                                            });

                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                                         "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_CC_SplitMissmatch").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            return;
                                        }
                                    }
                                    #endregion

                                    #region Checking for voucher is locked or not
                                    if (finTrxHdrObj.FTH_DATE.HasValue)
                                    {
                                        string LockUptoDate = string.Empty;
                                        if (BusinessLogic.Finance.VoucherLockingBL.IsVoucherLocked(finTrxHdrObj.FTH_DATE.Value, JurCurrPK, finTrxHdrObj.FTH_BIZUNIT, ref LockUptoDate))
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                                     "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                                            litErrorMsg.Text = GetLocalResourceObject("Err_Voucher_Locked").ToString() + (!string.IsNullOrEmpty(LockUptoDate) ? DateTime.Parse(LockUptoDate).ToString(Resources.Constants.DateFormatShort) : string.Empty);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            return;
                                        }
                                    }
                                    #endregion

                                    #region Check same account exist in debit and credit
                                    var objTrxList = from c in finTrxHdrObj.FIN_TRX
                                                     group c by new { c.FTR_ACCOUNT, c.FTR_TYPE_PK } into g
                                                     select new
                                                     {
                                                         Account = g.Key.FTR_ACCOUNT,
                                                         SubTypePk = g.Key.FTR_TYPE_PK,
                                                         Amount = g.Sum(r => r.FTR_DR_AMT_BC) - g.Sum(r => r.FTR_CR_AMT_BC)
                                                     };
                                    if (objTrxList != null && objTrxList.Where(r => r.Amount == 0).Count() > 0)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                              "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                                        litErrorMsg.Text = GetLocalResourceObject("Err_Same_Account").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        return;
                                    }
                                    #endregion

                                    #region New Workflow Integration
                                    if (finTrxHdrObj.FIN_TRX.Count > 0)
                                    {
                                        finTrxHdrList.Add(finTrxHdrObj);
                                        if (Convert.ToDecimal(txtDrTotal.Text.Trim()) == Convert.ToDecimal(txtCrTotal.Text.Trim())
                                           && (txtDrTotalBC.Visible && txtCrTotalBC.Visible ? (Convert.ToDecimal(txtDrTotalBC.Text.Trim()) == Convert.ToDecimal(txtCrTotalBC.Text.Trim())) : true))//If Total Amount in Debit and credit are equal
                                        {
                                            pettyCashAmountLimit = 0;
                                            if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.PCS)//If Transaction Type is PettyCash
                                            {
                                                GetFieldValues(ControlsEnum.PETTYCASHVOUCHERLIMIT);//Get Pettycash voucher limit
                                                if (admAppConfigMstList != null && admAppConfigMstList.Count == 1)
                                                {
                                                    pettyCashAmountLimit = Convert.ToDecimal(admAppConfigMstList[0].ACF_DATA);
                                                    if (Convert.ToDecimal(txtDrTotal.Text.Trim()) > pettyCashAmountLimit)
                                                        isValidAmount = false;//If total amount is greater than pettycash voucher limit, then set isValidAmount as false
                                                }
                                            }
                                            if (isValidAmount)
                                            {
                                                #region Voucher Number Generation
                                                if (AST_DOC_MODE.Value == ((int)DOCMODE.Manual).ToString())
                                                {
                                                    hdfVoucherNo.Value = txtDispPVNo.Text.Trim();
                                                    finTrxHdrList[0].FTH_VOUCHER_NO = HttpUtility.HtmlEncode(hdfVoucherNo.Value);
                                                }

                                                #endregion
                                                bool isValidVoucherNo = true;
                                                if (AST_DOC_MODE.Value == ((int)DOCMODE.Manual).ToString())
                                                {
                                                    //Check for voucher no duplication
                                                    isValidVoucherNo = finTrxServiceClient.CheckForVoucherNoDuplication(JurCurrPK, finTrxHdrList[0].FTH_VOUCHER_NO);
                                                }
                                                if (isValidVoucherNo)
                                                {
                                                    FinTrxHeaderBO finTrxHeaderObj = GetFinTrxHdrObject(finTrxHdrList[0]);
                                                    finTrxHeaderObj.WKF_FLAG = 1;
                                                    finTrxHeaderObj.IS_CANCEL = "0";
                                                    if (finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.PPCCJ)
                                                    {
                                                        finTrxHeaderObj.PDCFLAG = "2";//UpdatePdc
                                                        if (GetGlobalResourceObject("ConfigurationsRes", "PPCReconciliation").ToString() == "1")
                                                        {
                                                            finTrxHeaderObj.PPC_RECONCILIATION = "1";//Update PPC Reconciliation Details
                                                        }
                                                    }
                                                    if (finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.PPCCTJ)
                                                    {
                                                        finTrxHeaderObj.PDCFLAG = "2";//UpdatePdc
                                                        if (GetGlobalResourceObject("ConfigurationsRes", "PPCReconciliation").ToString() == "1")
                                                        {
                                                            finTrxHeaderObj.PPC_RECONCILIATION = "1";//Update PPC Reconciliation Details
                                                        }
                                                    }
                                                    if (finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.RCBJ
                                                        || finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.RCBTJ)
                                                    {
                                                        finTrxHeaderObj.BOUNCEFLAG = "1";//UpdateBounce
                                                    }
                                                    if (finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.PDCCJ
                                                        || finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.PDCCTJ)
                                                    {
                                                        finTrxHeaderObj.PDCFLAG = "2";//UpdateReceiptHdrPDC
                                                        if (GetGlobalResourceObject("ConfigurationsRes", "PDCReconciliation").ToString() == "1")
                                                        {
                                                            finTrxHeaderObj.PDC_RECONCILIATION = "1";//Update PDC Reconciliation Details
                                                        }
                                                    }
                                                    if (finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.DPVCJ)
                                                    {
                                                        finTrxHeaderObj.PDCFLAG = "2";//UpdateDirectPaymentHdrPDC

                                                        if (GetGlobalResourceObject("ConfigurationsRes", "PDCReconciliation").ToString() == "1")
                                                        {
                                                            finTrxHeaderObj.PDC_RECONCILIATION = "1";//Update PDC Reconciliation Details
                                                        }
                                                    }
                                                    if (finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.PIJ || finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.EIJ || finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.PSIJ)
                                                    {
                                                        finTrxHeaderObj.UPDATE_INV_STOCK = "1"; //UpdateInvoiceStock
                                                    }

                                                    SaveTransaction(finTrxHeaderObj, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT), sender, e);
                                                    ClearVoucher();

                                                }//If not a valid voucher no
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                                                   "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Duplicate_VoucherNo").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                }
                                            }//If not a valid amount. It wil be only in the case of PCS
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                                                    "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                                                litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_ErrAmount_Exceed").ToString(), pettyCashAmountLimit);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            }

                                        }//If total amount is not tallied
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                            "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                                            litErrorMsg.Text = GetLocalResourceObject("Msg_ErrAmount_NotValid").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        }
                                        return;
                                    }
                                    #endregion

                                }
                            }//If total amount is not tallied
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                                litErrorMsg.Text = GetLocalResourceObject("Msg_ErrAmount_NotValid").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                            #endregion

                        }
                        else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)//If Delete & Submit
                        {
                            #region Checking for voucher is locked or not
                            if (!string.IsNullOrEmpty(hdfPVDate.Value))
                            {
                                string LockUptoDate = string.Empty;
                                if (BusinessLogic.Finance.VoucherLockingBL.IsVoucherLocked(DateTime.Parse(hdfPVDate.Value), JurCurrPK, currentUser.SBUID, ref LockUptoDate))
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                             "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                                    litErrorMsg.Text = GetLocalResourceObject("Err_Voucher_Locked").ToString() + (!string.IsNullOrEmpty(LockUptoDate) ? DateTime.Parse(LockUptoDate).ToString(Resources.Constants.DateFormatShort) : string.Empty);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                            }
                            #endregion
                            if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(JurCurrPK, Session[ERP.Utilities.SessionStrings.JournalType].ToString()))
                            {
                                FinTrxHeaderBO objFinTrxHeaderCancel = new FinTrxHeaderBO();
                                objFinTrxHeaderCancel.FTH_PK = JurCurrPK;
                                objFinTrxHeaderCancel.IS_CANCEL = "1";

                                if (Session[ERP.Utilities.SessionStrings.JournalType].ToString() == ApplicationType.PPCCJ)
                                {
                                    if (GetGlobalResourceObject("ConfigurationsRes", "PPCReconciliation").ToString() == "1")
                                    {
                                        objFinTrxHeaderCancel.PPC_RECONCILIATION = "1";//Update PPC Reconciliation Details
                                    }
                                }
                                if (Session[ERP.Utilities.SessionStrings.JournalType].ToString() == ApplicationType.PPCCTJ)
                                {
                                    if (GetGlobalResourceObject("ConfigurationsRes", "PPCReconciliation").ToString() == "1")
                                    {
                                        objFinTrxHeaderCancel.PPC_RECONCILIATION = "1";//Update PPC Reconciliation Details
                                    }
                                }
                                if (Session[ERP.Utilities.SessionStrings.JournalType].ToString() == ApplicationType.PDCCJ
                                    || Session[ERP.Utilities.SessionStrings.JournalType].ToString() == ApplicationType.PDCCTJ)
                                {
                                    if (GetGlobalResourceObject("ConfigurationsRes", "PDCReconciliation").ToString() == "1")
                                    {
                                        objFinTrxHeaderCancel.PDC_RECONCILIATION = "1";//Update PDC Reconciliation Details
                                    }
                                }
                                if (finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.DPVCJ)
                                {
                                    if (GetGlobalResourceObject("ConfigurationsRes", "PDCReconciliation").ToString() == "1")
                                    {
                                        objFinTrxHeaderCancel.PDC_RECONCILIATION = "1";//Update PDC Reconciliation Details

                                    }
                                }
                                SaveTransaction(objFinTrxHeaderCancel, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT), sender, e);
                                ClearVoucher();
                            }
                            else//If the voucher is already used somewhere
                            {
                                ((Button)sender).CommandName = ActionsEnum.WRKFSUBMIT.ToString();
                                WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                WrkfComments.Text = "";
                                Session[ERP.Utilities.SessionStrings.Transaction] = "CANCEL";
                                if (JournalType == (int)JournalTypeEnum.Voucher)
                                {
                                    ((Button)sender).CommandName = ActionsEnum.JOURNALIZECANCEL.ToString();//Assign new Action
                                    JournalizeCancel(sender, e);//Call action in the popup calling page
                                }
                                else if (JournalType == (int)JournalTypeEnum.Reverse)
                                {
                                    ((Button)sender).CommandName = ActionsEnum.REVERSECANCEL.ToString();//Assign new Action
                                    ReverseCancel(sender, e);//Call action in the popup calling page
                                }
                                else if (JournalType == (int)JournalTypeEnum.Return)
                                {
                                    ((Button)sender).CommandName = ActionsEnum.RETURNCANCEL.ToString();//Assign new Action
                                    ReturnCancel(sender, e);//Call action in the popup calling page
                                }
                                ((Button)sender).CommandName = ActionsEnum.WRKFSUBMIT.ToString();//Re-Assign original Action
                                divGroupDrHdr.Controls.Clear();//Clear Debit Hdr section
                                divGroupCrHdr.Controls.Clear();//Clear Credit Hdr section
                                divGroupDr.Controls.Clear();//Clear Debit Controls
                                divGroupCr.Controls.Clear();//Clear Credit Controls

                                litErrorMsg.Text = GetLocalResourceObject("Err_Voucher_Cancel").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);

                                ClearVoucher();
                            }
                        }
                        else
                        {
                            SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT), sender, e);
                            ClearVoucher();
                        }
                        #endregion
                        break;
                    #endregion
                    #region ExchangeRate
                    case ActionsEnum.CALCURRENCY:
                        if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                        {
                            Session[ERP.Utilities.SessionStrings.TransactionCurrency] = hdfJournalCurr.Value;
                            string strCurID = hdfJournalCurr.Value;
                            string strCurTxt = txtJournalCurrency.Text;
                            ////this.OnInit(new EventArgs());
                            divGroupDrHdr.Controls.Clear();
                            divGroupDr.Controls.Clear();
                            divGroupCrHdr.Controls.Clear();
                            divGroupCr.Controls.Clear();
                            CallUserControl();
                            hdfJournalCurr.Value = strCurID;
                            txtJournalCurrency.Text = strCurTxt;
                            //juno

                            finTrxHdrObj = null;
                            GetFieldValues(ControlsEnum.EXCHANGERATE);//get Exchange rate
                            double exchangeRate = string.IsNullOrEmpty(hdfExchangeRateJV.Value) ? 1 : Convert.ToDouble(hdfExchangeRateJV.Value);
                            if (exchangeRate > 0)
                            {
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = hdfJournalCurr.Value;
                                //If the Voucher currency is Base Currency. Then disable Exchange rate text box , hide BC Amount and G/L button Section
                                if (Convert.ToInt32(hdfJournalCurr.Value) == currentUser.BaseCurrency)
                                {
                                    txtJournalExchangeRate.Enabled = false;
                                    txtJournalExchangeRate.Attributes.Remove("class");
                                    txtJournalExchangeRate.Attributes.Add("class", "input-w8-5per numeric input-disabled");
                                    divTotalDr.Attributes.Remove("class");
                                    divTotalDr.Attributes.Add("class", "divcolmiddle-journaltotal ");
                                    divTotalCr.Attributes.Remove("class");
                                    divTotalCr.Attributes.Add("class", "divcolmiddle-journaltotal ");
                                    txtDrTotalBC.Visible = false;
                                    txtCrTotalBC.Visible = false;
                                    imbGainLoss.Visible = false;
                                    IsBaseCurrency = true;
                                }
                                else//If the Voucher currency is Foreign Currency. Then Enable Exchange rate text box , show BC Amount and G/L button Section
                                {
                                    txtJournalExchangeRate.Enabled = true;
                                    txtJournalExchangeRate.Attributes.Remove("class");
                                    txtJournalExchangeRate.Attributes.Add("class", "input-w8-5per numeric");
                                    divTotalDr.Attributes.Remove("class");
                                    divTotalDr.Attributes.Add("class", "divcolmiddle-journaltotal");
                                    divTotalCr.Attributes.Remove("class");
                                    divTotalCr.Attributes.Add("class", "divcolmiddle-journaltotal");
                                    txtDrTotalBC.Visible = true;
                                    txtCrTotalBC.Visible = true;
                                    imbGainLoss.Visible = true;
                                    IsBaseCurrency = false;
                                }

                                txtJournalExchangeRate.Text = exchangeRate.ToString(hdfExchRateFormatVoucher.Value);
                                litJournalCurrencyCr.Text = litJournalCurrencyDr.Text = string.Format(GetLocalResourceObject("AmountHdrTC").ToString(), txtJournalCurrency.Text);

                                divGroupDrHdr.Controls.Clear();//Clear Debit Hdr section
                                SetDebitHdr();//Reset Debit Hdr section
                                divGroupCrHdr.Controls.Clear();//Clear Credit Hdr section
                                SetCreditHdr();//Reset Credit Hdr section

                                if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null
                                    && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString())
                                    && Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()) == currentUser.BaseCurrency)
                                //If TC is BC
                                {
                                    dicDrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.DrControls];//Get Debit Controls
                                    foreach (KeyValuePair<string, string> pair in dicDrControls)
                                    {
                                        if (pair.Key.Substring(7, 8).Equals("07set107") && !pair.Key.EndsWith("EntryMode"))
                                        {
                                            TextBox txtExchangeRateDR = (TextBox)pnlControls.FindControl(pair.Key);//Find Exchange rate textbox in the details Section
                                            if (txtExchangeRateDR != null)
                                            {
                                                txtExchangeRateDR.Text = "1";
                                                txtExchangeRateDR.Visible = false;
                                            }
                                        }
                                        else if (pair.Key.Substring(7, 8).Equals("08set108"))
                                        {
                                            TextBox txtAmountBCDR = (TextBox)pnlControls.FindControl(pair.Key);//Find BC textbox in the details Section
                                            if (txtAmountBCDR != null)
                                            {
                                                string tcId = pair.Key.Replace(pair.Key.Substring(7, 8), "06set106");
                                                TextBox txtAmountTCDR = (TextBox)pnlControls.FindControl(tcId);//Find TC textbox in the details Section
                                                if (txtAmountTCDR != null)
                                                {
                                                    txtAmountBCDR.Text = txtAmountTCDR.Text;
                                                    txtAmountBCDR.Visible = false;
                                                }
                                            }
                                        }
                                    }
                                    dicCrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.CrControls];//Get Credit Controls
                                    foreach (KeyValuePair<string, string> pair in dicCrControls)
                                    {
                                        if (pair.Key.Substring(7, 8).Equals("07set107") && !pair.Key.EndsWith("EntryMode"))
                                        {
                                            TextBox txtExchangeRateCR = (TextBox)pnlControls.FindControl(pair.Key);//Find Exchange rate textbox in the details Section
                                            if (txtExchangeRateCR != null)
                                            {
                                                txtExchangeRateCR.Text = "1";
                                                txtExchangeRateCR.Visible = false;
                                            }
                                        }
                                        else if (pair.Key.Substring(7, 8).Equals("08set108"))
                                        {
                                            TextBox txtAmountBCCR = (TextBox)pnlControls.FindControl(pair.Key);//Find BC textbox in the details Section
                                            if (txtAmountBCCR != null)
                                            {
                                                string tcId = pair.Key.Replace(pair.Key.Substring(7, 8), "06set106");
                                                TextBox txtAmountTCCR = (TextBox)pnlControls.FindControl(tcId);//Find TC textbox in the details Section
                                                if (txtAmountTCCR != null)
                                                {
                                                    txtAmountBCCR.Text = txtAmountTCCR.Text;
                                                    txtAmountBCCR.Visible = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else//If TC is Foreign Currency
                                {
                                    if (Session[ERP.Utilities.SessionStrings.ControlInfo] != null)
                                        dicControlinfo = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.ControlInfo];
                                    else
                                        dicControlinfo = new Dictionary<string, string>();

                                    dicDrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.DrControls];//Get Debit Controls
                                    foreach (KeyValuePair<string, string> pair in dicDrControls)
                                    {
                                        if (pair.Key.Substring(7, 8).Equals("07set107") && !pair.Key.EndsWith("EntryMode"))
                                        {
                                            TextBox txtExchangeRateDR = (TextBox)pnlControls.FindControl(pair.Key);//Find Exchange rate textbox in the details Section
                                            if (txtExchangeRateDR != null)
                                            {
                                                txtExchangeRateDR.Visible = true;
                                            }
                                        }
                                        else if (pair.Key.Substring(7, 8).Equals("08set108"))
                                        {
                                            TextBox txtAmountBCDR = (TextBox)pnlControls.FindControl(pair.Key);//Find BC textbox in the details Section
                                            if (txtAmountBCDR != null)
                                                txtAmountBCDR.Visible = true;
                                        }
                                    }
                                    dicCrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.CrControls];//Get Credit Controls
                                    foreach (KeyValuePair<string, string> pair in dicCrControls)
                                    {
                                        if (pair.Key.Substring(7, 8).Equals("07set107") && !pair.Key.EndsWith("EntryMode"))
                                        {
                                            TextBox txtExchangeRateCR = (TextBox)pnlControls.FindControl(pair.Key);//Find Exchange rate textbox in the details Section
                                            if (txtExchangeRateCR != null)
                                            {
                                                txtExchangeRateCR.Visible = true;
                                            }
                                        }
                                        else if (pair.Key.Substring(7, 8).Equals("08set108"))
                                        {
                                            TextBox txtAmountBCCR = (TextBox)pnlControls.FindControl(pair.Key);//Find BC textbox in the details Section
                                            if (txtAmountBCCR != null)
                                                txtAmountBCCR.Visible = true;
                                        }
                                    }
                                }
                                int exchangeRateDigit = Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit].ToString()) : Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits;
                                txtJournalExchangeRate.Text = Math.Round(decimal.Parse(txtJournalExchangeRate.Text), exchangeRateDigit).ToString(hdfExchRateFormatVoucher.Value);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateAllTCAmt", "$(document).ready(function(){CalculateAllTCAmt();});", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                                           "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                            }
                            else//If No Exchange Rate
                            {
                                txtJournalCurrency.Text = Resources.ErpRes.AutoDefaultValue;
                                hdfJournalCurr.Value = "0";
                                txtJournalExchangeRate.Text = "";
                                hdfExchangeRateJV.Value = "";
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                   "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_ExchangeRate").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }

                        break;
                    #endregion
                    #region PRINT
                    case ActionsEnum.PRINTJORNALPOPUP:
                        if (Session[ERP.Utilities.SessionStrings.TrxPK] != null || JurCurrPK > 0)
                        {
                            GetFieldValues(ControlsEnum.FINHEADERBYPK);
                            if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                            {
                                long trxPK = finTrxHdrList[0].FTH_REF_PK;//Reference PK
                                string trxType = finTrxHdrList[0].FTH_REF_TYPE;//Reference Type
                                string appSubType = string.Empty;
                                string appType = finTrxHdrList[0].FTH_REF_TYPE;
                                string company = finTrxHdrList[0].FTH_COMPANY.ToString();//Company
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                    "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                                string _printerMode = "";

                                //#region Getting Printer Mode
                                //DataSet dsParamSettings = new DataSet();
                                //string RptType = appType;
                                ////string RptType = Session[ERP.Utilities.SessionStrings.Type].ToString();
                                //int RptSubType = 0;
                                //Int32.TryParse(appSubType, out RptSubType);
                                //DateTime AppvdDate = DateTime.Now;

                                //List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList = new CommonService().GetReportParameters(RptType, RptSubType, AppvdDate);

                                //if (AppTypeDetailsList != null && AppTypeDetailsList.Count > 0)
                                //{
                                //    dsParamSettings.ReadXml(new System.IO.StringReader(Convert.ToString(AppTypeDetailsList[0].AST_RPT_SETTINGS)));

                                //    if (dsParamSettings.Tables.Count > 0 && dsParamSettings.Tables[0].Columns.Contains("PRINTER_MODE"))
                                //    {
                                //        _printerMode = dsParamSettings.Tables[0].Rows[0]["PRINTER_MODE"].ToString();
                                //    }
                                //}
                                //#endregion
                                //#region DotMatrix Printing

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
                                //#endregion
                                //#region Normal Printing
                                //else
                                //{
                                if (appType.Equals(ApplicationType.JV) || appType.Equals(ApplicationType.PCS))
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + JurCurrPK.ToString() + "&APPTYPE=" + appType +
                                        "&APPSUBTYPE=" + appSubType + "&TRXTYPE=" + trxType + "&COMPANY=" + company + "');", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + trxPK.ToString() + "&APPTYPE=" + appType +
                                        "&APPSUBTYPE=" + appSubType + "&TRXTYPE=" + trxType + "&COMPANY=" + company + "');", true);
                                }
                                //}
                                //#endregion
                                #region Commented By Biju
                                //if (appType.Equals(ApplicationType.JV) || appType.Equals(ApplicationType.PCS))
                                //{
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + appType +
                                //        "&APPSUBTYPE=" + appSubType + "&TRXTYPE=" + trxType + "&COMPANY=" + company + "');", true);
                                //}
                                //else
                                //{
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + trxPK.ToString() + "&APPTYPE=" + appType +
                                //        "&APPSUBTYPE=" + appSubType + "&TRXTYPE=" + trxType + "&COMPANY=" + company + "');", true);

                                //} 
                                #endregion
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                    "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                    "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                        }
                        break;
                    #endregion.
                    #region GAINLOSS
                    case ActionsEnum.GAINLOSS:
                        //Delete current gain loss
                        decimal drTotal = 0;
                        decimal crTotal = 0;
                        decimal.TryParse(txtDrTotal.Text.Trim(), out drTotal);//Get Debit Total TC
                        decimal.TryParse(txtCrTotal.Text.Trim(), out crTotal);//Get Credit Total TC
                        if (drTotal > 0 && crTotal > 0 && drTotal == crTotal)//If DR TC and CR TC is equal
                        {
                            List<HiddenField> hdfList = null;
                            hdfList = new List<HiddenField>();
                            #region Delete DR Gain Loss If any
                            if (Session[ERP.Utilities.SessionStrings.DrControls] != null)
                            {
                                if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)//Get Removed Control List
                                    removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];
                                else
                                    removedControlsList = new List<string>();

                                dicDrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.DrControls];//Get Debit Controls
                                foreach (KeyValuePair<string, string> pair in dicDrControls)
                                {

                                    string txtEntryModeId = pair.Value != "EntryMode" ? pair.Key.Replace(pair.Key.Substring(7, 8), "07set107") + "EntryMode" : pair.Key;//Get Entry Mode ID
                                    HiddenField hdfMode = (HiddenField)pnlControls.FindControl(txtEntryModeId);//Get EntryMode Control
                                    if (hdfMode != null && !string.IsNullOrEmpty(hdfMode.Value)
                                        && Convert.ToByte(hdfMode.Value) == (byte)VoucherEntryMode.GainOrLoss)//If Entry Mode is G/L
                                    {
                                        if (!removedControlsList.Contains(pair.Key))
                                            removedControlsList.Add(pair.Key);//Add it to Removed Control List
                                        if (pair.Key.Equals(hdfMode.ID) && !hdfList.Contains(hdfMode))
                                            hdfList.Add(hdfMode);//Add EntryMode Control to List , it will be used to remove G/L section from UI
                                    }
                                }
                                Session[ERP.Utilities.SessionStrings.RemovedControls] = removedControlsList;//Update removedControlsList session
                                if (dicDrControls != null && dicDrControls.Count > 0)
                                {
                                    foreach (string key in removedControlsList)
                                    {
                                        if (dicDrControls.ContainsKey(key))
                                            dicDrControls.Remove(key);//Delete All controls from Debit control list, if any of them is in removed control list
                                    }
                                }
                                Session[ERP.Utilities.SessionStrings.DrControls] = dicDrControls;//Update dicDrControls list session
                                if (Session[ERP.Utilities.SessionStrings.AccountType] != null)
                                {
                                    dicAccountType = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.AccountType];
                                    if (dicAccountType != null && dicAccountType.Count > 0)
                                    {
                                        foreach (string key in removedControlsList)
                                        {
                                            if (dicAccountType.ContainsKey(key))
                                                dicAccountType.Remove(key);//Reset Account type list
                                        }
                                    }
                                    Session[ERP.Utilities.SessionStrings.AccountType] = dicAccountType;//Update Account type list session
                                }
                            }

                            foreach (HiddenField hdf in hdfList)//Remove all G/L Groups from Debit UI section
                            {
                                table = (Table)(hdf.Parent.Parent.Parent.Parent);
                                if (table != null && divGroupDr.Controls.Contains(table))
                                    divGroupDr.Controls.Remove(table);
                            }

                            #endregion
                            hdfList = null;
                            hdfList = new List<HiddenField>();
                            #region Delete CR Gain Loss If any
                            if (Session[ERP.Utilities.SessionStrings.CrControls] != null)
                            {
                                if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)//Get Removed Control List
                                    removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];
                                else
                                    removedControlsList = new List<string>();

                                dicCrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.CrControls];//Get cREDIT Controls
                                foreach (KeyValuePair<string, string> pair in dicCrControls)
                                {
                                    string txtEntryModeId = pair.Value != "EntryMode" ? pair.Key.Replace(pair.Key.Substring(7, 8), "07set107") + "EntryMode" : pair.Key;//Get Entry Mode ID
                                    HiddenField hdfMode = (HiddenField)pnlControls.FindControl(txtEntryModeId);//Get EntryMode Control
                                    if (hdfMode != null && !string.IsNullOrEmpty(hdfMode.Value)
                                        && Convert.ToByte(hdfMode.Value) == (byte)VoucherEntryMode.GainOrLoss)//If Entry Mode is G/L
                                    {
                                        if (!removedControlsList.Contains(pair.Key))
                                            removedControlsList.Add(pair.Key);//Add it to Removed Control List
                                        if (pair.Key.Equals(hdfMode.ID) && !hdfList.Contains(hdfMode))
                                            hdfList.Add(hdfMode);//Add EntryMode Control to List , it will be used to remove G/L section from UI

                                    }
                                }
                                Session[ERP.Utilities.SessionStrings.RemovedControls] = removedControlsList;//Update removedControlsList session
                                if (dicCrControls != null && dicCrControls.Count > 0)
                                {
                                    foreach (string key in removedControlsList)
                                    {
                                        if (dicCrControls.ContainsKey(key))
                                            dicCrControls.Remove(key);//Delete All controls from Credit control list, if any of them is in removed control list
                                    }
                                }
                                Session[ERP.Utilities.SessionStrings.CrControls] = dicCrControls;//Update dicCrControls list session
                                if (Session[ERP.Utilities.SessionStrings.AccountType] != null)
                                {
                                    dicAccountType = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.AccountType];
                                    if (dicAccountType != null && dicAccountType.Count > 0)
                                    {
                                        foreach (string key in removedControlsList)
                                        {
                                            if (dicAccountType.ContainsKey(key))
                                                dicAccountType.Remove(key);//Reset Account type list
                                        }
                                    }
                                    Session[ERP.Utilities.SessionStrings.AccountType] = dicAccountType;//Update Account type list session
                                }
                            }

                            foreach (HiddenField hdf in hdfList)//Remove all G/L Groups from Credit UI section
                            {
                                table = (Table)(hdf.Parent.Parent.Parent.Parent);
                                if (table != null && divGroupCr.Controls.Contains(table))
                                    divGroupCr.Controls.Remove(table);
                            }
                            #endregion
                            voucherGainLossObj = (VoucherGainLossHeader)SetUIValuesToObject(ActionsEnum.GAINLOSS);
                            if (voucherGainLossObj != null && voucherGainLossObj.GainLossDetails != null && voucherGainLossObj.GainLossDetails.Count > 0)
                            {
                                xmlDoc = CommonFunctions.XmlSerialize<VoucherGainLossHeader>(voucherGainLossObj);//Generate XML
                                DataSet dsGainLoss = BusinessLogic.Jouralize.JournalizeBL.GetJournalGainLoss(xmlDoc);//Get G/L account after tally
                                if (dsGainLoss != null)
                                {
                                    DataTable dtGainLoss = dsGainLoss.Tables[0];
                                    if (dtGainLoss.Rows.Count > 0)
                                    {
                                        foreach (DataRow row in dtGainLoss.Rows)
                                        {
                                            if (row["FTR_ENTRY_MODE"] != null && Convert.ToByte(row["FTR_ENTRY_MODE"].ToString()) == (byte)VoucherEntryMode.GainOrLoss)//If Entry Mode is G/L
                                            {
                                                if ((row["FTR_DR_AMT_TC"] != null && row["FTR_DR_AMT_BC"] != null) && Convert.ToDecimal(row["FTR_DR_AMT_TC"].ToString()) > 0 || Convert.ToDecimal(row["FTR_DR_AMT_BC"].ToString()) > 0)
                                                //G/L will be add to Debit Section
                                                {
                                                    #region Add Debit
                                                    drCount = 1;
                                                    dicDrControls = new Dictionary<string, string>();

                                                    if (Session[ERP.Utilities.SessionStrings.AccountType] != null)//Used to store Account Names
                                                        dicAccountType = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.AccountType];
                                                    else
                                                        dicAccountType = new Dictionary<string, string>();

                                                    if (Session[ERP.Utilities.SessionStrings.ControlInfo] != null)//Used to store any special informations
                                                        dicControlinfo = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.ControlInfo];
                                                    else
                                                        dicControlinfo = new Dictionary<string, string>();

                                                    #region getID
                                                    //Generate the format of new Debit section id
                                                    //get the maximum group no from dr section. create the next id. check whether this is in removed session or not. if yes create next one . check again and again. after all this steps we will get the new id.
                                                    if (Session[ERP.Utilities.SessionStrings.DrControls] != null)
                                                    {
                                                        dicDrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.DrControls];//All active Dr Controls
                                                        var drControls = from pair in dicDrControls
                                                                         orderby pair.Key.Substring(0, 13) descending//orderby pair.Key.Substring(0, 9) descending
                                                                         select pair;
                                                        if (drControls != null && drControls.Count() > 0)//if the UI contains drcontrols
                                                        {
                                                            //Get the current maximum drcount
                                                            drCount = Convert.ToInt32(string.IsNullOrEmpty(drControls.First().Key.Substring(3, 4)) ? "0" : drControls.First().Key.Substring(3, 4));
                                                            drCount++;
                                                            //check if drCount is in deleted Dr controls list
                                                            if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)
                                                            {
                                                                //Gets the removed controls list
                                                                removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];
                                                                while (removedControlsList.Contains("dic" + drCount.ToString("0000") + "01" + "set" + "1" + "01" + "id" + drCount.ToString("0000") + "dr"))
                                                                {
                                                                    //If drcount is in removed controls list den increment drcount by 1
                                                                    drCount++;
                                                                }
                                                            }
                                                        }
                                                        else//if the UI contains no drcontrols
                                                        {
                                                            //check if drCount is in deleted Dr controls list
                                                            if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)
                                                            {
                                                                //Gets the removed controls list
                                                                removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];
                                                                while (removedControlsList.Contains("dic" + drCount.ToString("0000") + "01" + "set" + "1" + "01" + "id" + drCount.ToString("0000") + "dr"))
                                                                {
                                                                    //If drcount is in removed controls list den increment drcount by 1
                                                                    drCount++;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                    selectedAccountVal = "-1";
                                                    tbControls.CssClass = "";
                                                    divColStyle = "divcolmiddle-S1 input-margin2";
                                                    div = new HtmlGenericControl("div");//This div will hold the drcontrols that we are going to create.
                                                    div.Attributes.Add("class", divColStyle);//set the style for the div
                                                    divValidation = new HtmlGenericControl("div");//This div will hold all the validation controls that we are going to create.
                                                    divValidation.Attributes.Add("class", "starwrap");//set the style for the div
                                                    tbControls = new Table();
                                                    tbControls.CssClass = "";
                                                    trControls = new TableRow();
                                                    tcControl = new TableCell();
                                                    #region SubType
                                                    Label txtSubTypeGL = new Label()//Account Type
                                                    {
                                                        Text = "",
                                                        ClientIDMode = ClientIDMode.Static
                                                    };

                                                    txtSubTypeGL.ID = "dic" + drCount.ToString("0000") + "01" + "set" + "1" + "01" + "id" + drCount.ToString("0000") + "dr";
                                                    txtSubTypeGL.Text = row["FTR_TYPE"] != null ? row["FTR_TYPE"].ToString() : GetLocalResourceObject("Gain_Loss_Account").ToString();
                                                    //Associate control id will be the id of Account Textbox. We can get the this id by just incrementing the control index in the set from the Account type ControlID
                                                    txtSubTypeGL.AssociatedControlID = "dic" + drCount.ToString("0000") + "02" + "set" + "1" + "02" + "id" + drCount.ToString("0000") + "dr";
                                                    div.Controls.Add(txtSubTypeGL);
                                                    if (txtSubTypeGL != null)
                                                    {
                                                        if (!dicDrControls.ContainsKey(txtSubTypeGL.ID))
                                                            dicDrControls.Add(txtSubTypeGL.ID, "SubType");//Add to Active dr Controls List
                                                        if (!dicAccountType.ContainsKey(txtSubTypeGL.ID))
                                                            dicAccountType.Add(txtSubTypeGL.ID, txtSubTypeGL.Text);//Add Account Name in dictionary
                                                    }
                                                    #endregion
                                                    #region Account
                                                    //It is an autocomplte control with postback. So we need Textbox,Hiddenfield and a Button.
                                                    TextBox txtAccountGL = new TextBox()
                                                    {
                                                        Text = "",
                                                        MaxLength = 100,
                                                        ClientIDMode = ClientIDMode.Static,
                                                        TabIndex = TabIndexDr,
                                                        Enabled = true
                                                    };
                                                    HiddenField hdfAccountGL = new HiddenField
                                                    {
                                                        ClientIDMode = ClientIDMode.Static
                                                    };
                                                    Button btnAccountGL = new Button
                                                    {
                                                        ClientIDMode = ClientIDMode.Static,
                                                        CommandName = "JOURNALACCOUNTINDEXCHANGED",
                                                        EnableTheming = false
                                                    };
                                                    btnAccountGL.Attributes.Add("style", "display:none;");

                                                    txtAccountGL.ID = "dic" + drCount.ToString("0000") + "02" + "set" + "1" + "02" + "id" + drCount.ToString("0000") + "dr";
                                                    hdfAccountGL.ID = "dic" + drCount.ToString("0000") + "03" + "set" + "1" + "03" + "id" + drCount.ToString("0000") + "dr";
                                                    btnAccountGL.ID = "dic" + drCount.ToString("0000") + "04" + "set" + "1" + "04" + "id" + drCount.ToString("0000") + "dr";
                                                    hdfSubTypePk.Value = row["FTR_ACC_SUB_TYPE"] != null ? row["FTR_ACC_SUB_TYPE"].ToString() : "0";


                                                    if (!dicControlinfo.ContainsKey(txtAccountGL.ID + "SubTypePk"))//Store Subtype PK
                                                        dicControlinfo.Add(txtAccountGL.ID + "SubTypePk", hdfSubTypePk.Value);
                                                    //Register Autocomplete script
                                                    journalScript = journalScript + "GrandScriptUtils.MakeAutoCompleteDDL('" + txtAccountGL.ID + "', (url1.indexOf('?') != -1 ? url1+'&' :  url1+'?') + 'AccType=" + hdfSubTypePk.Value + "', '" + hdfAccountGL.ID + "', true, true, 'JOURNALACCOUNT',false,false,false);";

                                                    hdfAccountGL.Value = row["FTR_ACCOUNT"] != null ? row["FTR_ACCOUNT"].ToString() : string.Empty;

                                                    selectedAccountVal = hdfAccountGL.Value;
                                                    hdfCoaPk.Value = selectedAccountVal;

                                                    if (!dicControlinfo.ContainsKey(txtAccountGL.ID + "CoaPk"))
                                                        dicControlinfo.Add(txtAccountGL.ID + "CoaPk", hdfCoaPk.Value);

                                                    GetFieldValues(ControlsEnum.FINCOAMST);
                                                    if (finCoaMstList != null && finCoaMstList.Count == 1)
                                                    {
                                                        txtAccountGL.Text = HttpUtility.HtmlDecode(finCoaMstList[0].COA_CODE + " - " + finCoaMstList[0].COA_NAME);
                                                    }
                                                    //Register Button Event
                                                    btnAccountGL.Click += new EventHandler(ActionHandler);
                                                    //Add controls to div
                                                    div.Controls.Add(txtAccountGL);
                                                    div.Controls.Add(hdfAccountGL);
                                                    div.Controls.Add(btnAccountGL);

                                                    RequiredFieldValidator vrfAccountGL = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + txtAccountGL.ID.Replace("dic", ""),
                                                        ControlToValidate = txtAccountGL.ID,
                                                        Text = "*",
                                                        CssClass = "star",
                                                        ValidationGroup = "voucher",
                                                        Display = ValidatorDisplay.Dynamic,
                                                        EnableClientScript = true,
                                                        InitialValue = Resources.Messages.AutoDefaultValue,
                                                        SetFocusOnError = true,
                                                        ErrorMessage = GetLocalResourceObject("Err_Account").ToString(),
                                                        ClientIDMode = ClientIDMode.Static
                                                    };
                                                    //Add validation control to validation div
                                                    divValidation.Controls.Add(vrfAccountGL);

                                                    TabIndexDr++;
                                                    if (txtAccountGL != null)
                                                        if (!dicDrControls.ContainsKey(txtAccountGL.ID))
                                                            dicDrControls.Add(txtAccountGL.ID, "Account");//Add to Active dr Controls List
                                                    if (hdfAccountGL != null)
                                                        if (!dicDrControls.ContainsKey(hdfAccountGL.ID))
                                                            dicDrControls.Add(hdfAccountGL.ID, "Account");//Add to Active dr Controls List
                                                    if (btnAccountGL != null)
                                                        if (!dicDrControls.ContainsKey(btnAccountGL.ID))
                                                            dicDrControls.Add(btnAccountGL.ID, "Account");//Add to Active dr Controls List
                                                    #endregion
                                                    #region Narration
                                                    //Narration Textbox Control
                                                    TextBox txtNarrationGL = new TextBox()
                                                    {
                                                        Text = "",
                                                        MaxLength = 400,
                                                        ClientIDMode = ClientIDMode.Static,
                                                        TabIndex = TabIndexDr,
                                                        Enabled = true,
                                                        Width = IsSameCurrency() == true ? new Unit(GetLocalResourceObject("NarationWidthWithoutAmountBC").ToString()) : new Unit(GetLocalResourceObject("NarationWidthNormal").ToString())
                                                    };
                                                    txtNarrationGL.ID = "dic" + drCount.ToString("0000") + "05" + "set" + "1" + "05" + "id" + drCount.ToString("0000") + "dr";
                                                    div.Controls.Add(txtNarrationGL);////Add controls to div
                                                    TabIndexDr++;
                                                    if (txtNarrationGL != null)
                                                    {
                                                        if (!dicDrControls.ContainsKey(txtNarrationGL.ID))
                                                            dicDrControls.Add(txtNarrationGL.ID, "Narration");//Add to Active dr Controls List
                                                    }
                                                    #endregion
                                                    #region AmountTC
                                                    //Amount in Transaction Currency
                                                    TextBox txtAmountTCGL = new TextBox()
                                                    {
                                                        Text = "",
                                                        MaxLength = 15,
                                                        ClientIDMode = ClientIDMode.Static,
                                                        Enabled = true,
                                                        TabIndex = TabIndexDr,
                                                        CssClass = "input-normalb input-w8per numeric amounttcdr tempcss"
                                                    };
                                                    txtAmountTCGL.ID = "dic" + drCount.ToString("0000") + "06" + "set" + "1" + "06" + "id" + drCount.ToString("0000") + "dr";
                                                    txtAmountTCGL.Text = string.Empty;
                                                    txtAmountTCGL.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                                                    txtAmountTCGL.Attributes.Add("onpaste", "return false;");
                                                    div.Controls.Add(txtAmountTCGL);

                                                    AmountValidation vamAmountTCGL = new AmountValidation()
                                                    {
                                                        ID = "vam" + txtAmountTCGL.ID.Replace("dic", ""),
                                                        ControlToValidate = txtAmountTCGL.ID,
                                                        ErrorMessage = GetLocalResourceObject("MsgErr_AmountTc").ToString(),
                                                        NumberDigits = 11,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        EnableClientScript = true,
                                                        CssClass = "star",
                                                        ValidationGroup = "voucher",
                                                        NonZero = true
                                                    };
                                                    divValidation.Controls.Add(vamAmountTCGL);

                                                    TabIndexDr++;
                                                    if (txtAmountTCGL != null)
                                                    {
                                                        if (!dicDrControls.ContainsKey(txtAmountTCGL.ID))
                                                            dicDrControls.Add(txtAmountTCGL.ID, "AmountTC");
                                                    }
                                                    #endregion
                                                    #region ExchangeRate
                                                    //Exchange Rate
                                                    TextBox txtExchangeRateGL = new TextBox()
                                                    {
                                                        Text = "",
                                                        MaxLength = 9,
                                                        ClientIDMode = ClientIDMode.Static,
                                                        Enabled = true,
                                                        TabIndex = TabIndexDr,
                                                        CssClass = "input-normalb numeric input-w6per"
                                                    };
                                                    txtExchangeRateGL.ID = "dic" + drCount.ToString("0000") + "07" + "set" + "1" + "07" + "id" + drCount.ToString("0000") + "dr";
                                                    txtExchangeRateGL.Text = string.Empty;

                                                    HiddenField hdfEntryModeGL = new HiddenField()
                                                    {
                                                        ClientIDMode = ClientIDMode.Static
                                                    };
                                                    hdfEntryModeGL.ID = txtExchangeRateGL.ID + "EntryMode";//Used for to keep the entrymode of ExchangeRate
                                                    txtExchangeRateGL.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                                                    txtExchangeRateGL.Attributes.Add("onpaste", "return false;");

                                                    TabIndexDr++;
                                                    hdfEntryModeGL.Value = row["FTR_ENTRY_MODE"] != null ? row["FTR_ENTRY_MODE"].ToString() : ((byte)VoucherEntryMode.Editable).ToString();
                                                    if (!dicControlinfo.ContainsKey(txtExchangeRateGL.ID + "EntryMode"))
                                                        dicControlinfo.Add(txtExchangeRateGL.ID + "EntryMode", hdfEntryModeGL.Value);//Keep the entrymode for the curresponding Exchange Rate in Dictionary
                                                    if (txtExchangeRateGL != null)
                                                    {
                                                        if (!dicDrControls.ContainsKey(txtExchangeRateGL.ID))
                                                            dicDrControls.Add(txtExchangeRateGL.ID, "ExchangeRate");
                                                        if (hdfEntryModeGL != null)
                                                        {
                                                            if (!dicDrControls.ContainsKey(hdfEntryModeGL.ID))
                                                                dicDrControls.Add(hdfEntryModeGL.ID, "EntryMode");
                                                        }
                                                    }
                                                    transactionCurrency = 0;
                                                    if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                                                    {
                                                        transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                                                    }
                                                    else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                                                    {
                                                        transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);
                                                    }

                                                    if (transactionCurrency > 0
                                                        &&
                                                        transactionCurrency == currentUser.BaseCurrency)
                                                    {
                                                        txtExchangeRateGL.Visible = false;
                                                    }
                                                    div.Controls.Add(txtExchangeRateGL);
                                                    div.Controls.Add(hdfEntryModeGL);
                                                    ExchangeRateValidation vreExchangeRateGL = new ExchangeRateValidation()
                                                    {
                                                        ID = "vre" + txtExchangeRateGL.ID,
                                                        ControlToValidate = txtExchangeRateGL.ID,
                                                        ErrorMessage = GetLocalResourceObject("MsgErr_ExchangeRate").ToString(),
                                                        NumberDigits = 5,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        EnableClientScript = true,
                                                        CssClass = "star",
                                                        ValidationGroup = "voucher",
                                                        NonZero = true
                                                    };
                                                    divValidation.Controls.Add(vreExchangeRateGL);
                                                    #endregion
                                                    #region AmountBC
                                                    //Amount in Base Currency

                                                    TextBox txtAmountBCGL = new TextBox()
                                                    {
                                                        Text = "",
                                                        ClientIDMode = ClientIDMode.Static,
                                                        Enabled = true,
                                                        MaxLength = 15,
                                                        CssClass = "input-bgRed input-normalb input-w8per numeric tempcss"
                                                    };

                                                    txtAmountBCGL.ID = "dic" + drCount.ToString("0000") + "08" + "set" + "1" + "08" + "id" + drCount.ToString("0000") + "dr";
                                                    txtAmountBCGL.Text = ((decimal)0).ToString(hdfCurrencyFormatVoucher.Value);
                                                    if (IsEditableForApplication())
                                                    {
                                                        //txtAmountBCGL.CssClass = "input-w8per numeric";
                                                        txtAmountBCGL.Attributes.Add("onkeyup", "CalculateTCAmt(this,event);");//script registration for Calculate TC
                                                    }
                                                    else
                                                    {
                                                        txtAmountBCGL.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                                                        txtAmountBCGL.Attributes.Add("onpaste", "return false;");
                                                    }
                                                    txtAmountBCGL.Text = row["FTR_DR_AMT_BC"] != null ?
                                                        Math.Round(Convert.ToDecimal(row["FTR_DR_AMT_BC"].ToString()), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString() :
                                                        Convert.ToDecimal("0").ToString();
                                                    if (!dicControlinfo.ContainsKey(txtAmountBCGL.ID + "GL"))
                                                        dicControlinfo.Add(txtAmountBCGL.ID + "GL", GainLossMode.Loss + "");
                                                    div.Controls.Add(txtAmountBCGL);
                                                    AmountValidation vamAmountBCGL = new AmountValidation()
                                                    {
                                                        ID = "vam" + txtAmountBCGL.ID.Replace("dic", ""),
                                                        ControlToValidate = txtAmountBCGL.ID,
                                                        ErrorMessage = GetLocalResourceObject("MsgErr_AmountBc").ToString(),
                                                        NumberDigits = 11,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        EnableClientScript = true,
                                                        CssClass = "star",
                                                        ValidationGroup = "voucher"
                                                    };
                                                    divValidation.Controls.Add(vamAmountBCGL);
                                                    if (txtAmountBCGL != null)
                                                    {
                                                        if (!dicDrControls.ContainsKey(txtAmountBCGL.ID))
                                                            dicDrControls.Add(txtAmountBCGL.ID, "AmountBC");
                                                    }

                                                    transactionCurrency = 0;
                                                    if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                                                    {
                                                        transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                                                    }
                                                    else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                                                    {
                                                        transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);
                                                    }

                                                    if (transactionCurrency > 0
                                                        &&
                                                        transactionCurrency == currentUser.BaseCurrency)
                                                    {
                                                        txtAmountBCGL.Visible = false;
                                                    }
                                                    #endregion
                                                    #region Delete
                                                    //Delete button for deleting the Curresponding DR section
                                                    Button btnDeleteGL = new Button()
                                                    {
                                                        Text = "",
                                                        ClientIDMode = ClientIDMode.Static,
                                                        TabIndex = TabIndexDr,
                                                        Enabled = true,
                                                        SkinID = "delete-icon",
                                                        ToolTip = "Delete",
                                                        OnClientClick = "return ShowDeleteConfirm(this);"
                                                    };
                                                    btnDeleteGL.ID = "dic" + drCount.ToString("0000") + "09" + "set" + "1" + "09" + "id" + drCount.ToString("0000") + "dr";
                                                    btnDeleteGL.CommandName = ActionsEnum.REMOVEDEBIT.ToString();

                                                    btnDeleteGL.Click += new EventHandler(ActionHandler);
                                                    if (EntryStatus == EntryStatus.VIEWMODE)
                                                    {
                                                        btnDeleteGL.Visible = false;
                                                    }
                                                    div.Controls.Add(btnDeleteGL);
                                                    ////divValidation contains all the Validation Controls in a group. It will be added to the group after delete button. So if any validation catch it will show * after delete button
                                                    //div.Controls.Add(divValidation);
                                                    if (btnDeleteGL != null)
                                                    {
                                                        if (!dicDrControls.ContainsKey(btnDeleteGL.ID))
                                                            dicDrControls.Add(btnDeleteGL.ID, "Delete");
                                                    }
                                                    TabIndexDr++;
                                                    #endregion
                                                    #region Cost Center
                                                    Button btnCostCenterGL = new Button()
                                                    {
                                                        Text = "",
                                                        ClientIDMode = ClientIDMode.Static,
                                                        Visible = false,
                                                        TabIndex = TabIndexDr,
                                                        Enabled = true,
                                                        SkinID = "costcenter-icon",
                                                        ToolTip = GetLocalResourceObject("CCButtonToolTip").ToString()
                                                    };
                                                    HiddenField hdfIsCostCenterGL = new HiddenField
                                                    {
                                                        ClientIDMode = ClientIDMode.Static,
                                                        Value = "0"
                                                    };


                                                    btnCostCenterGL.ID = "dic" + drCount.ToString("0000") + "10" + "set" + "1" + "10" + "id" + drCount.ToString("0000") + "dr";
                                                    hdfIsCostCenterGL.ID = "dic" + drCount.ToString("0000") + "11" + "set" + "1" + "11" + "id" + drCount.ToString("0000") + "dr";
                                                    btnCostCenterGL.CommandName = ActionsEnum.COSTCENTER.ToString();

                                                    btnCostCenterGL.Click += new EventHandler(ActionHandler);

                                                    div.Controls.Add(btnCostCenterGL);

                                                    //divValidation contains all the Validation Controls in a group. It will be added to the group after delete button. So if any validation catch it will show * after delete button
                                                    div.Controls.Add(divValidation);

                                                    div.Controls.Add(hdfIsCostCenterGL);
                                                    if (btnCostCenterGL != null)
                                                    {
                                                        if (!dicDrControls.ContainsKey(btnCostCenterGL.ID))
                                                            dicDrControls.Add(btnCostCenterGL.ID, "CostCenter");
                                                    }
                                                    if (hdfIsCostCenterGL != null)
                                                    {
                                                        if (!dicDrControls.ContainsKey(hdfIsCostCenterGL.ID))
                                                            dicDrControls.Add(hdfIsCostCenterGL.ID, "CostCenter");
                                                    }
                                                    TabIndexDr++;
                                                    if (!string.IsNullOrEmpty(hdfCoaPk.Value)
                                                        && finCoaMstList != null
                                                        && finCoaMstList.Count == 1
                                                        && finCoaMstList[0].FIN_COA_COST_CENTER_MPG != null
                                                        && finCoaMstList[0].FIN_COA_COST_CENTER_MPG.Where(r => r.ADM_COST_CENTER_MST.CNM_ACTIVE == 1).Count() > 0)
                                                    {
                                                        hdfIsCostCenterGL.Value = "1";
                                                        btnCostCenterGL.Visible = true;
                                                    }
                                                    else
                                                    {
                                                        hdfIsCostCenterGL.Value = "0";
                                                        btnCostCenterGL.Visible = false;
                                                    }
                                                    #endregion
                                                    #region SubledgerLabel
                                                    //After Delete Button, the rest of the controls will comes in the next line. So the divClear will use to break the first line
                                                    divClear = new HtmlGenericControl("div");
                                                    divClear.Attributes.Add("class", "clear");
                                                    div.Controls.Add(divClear);
                                                    //It is a dummy label to adjust the style
                                                    Label lblSubledgerLabelGL = new Label()
                                                    {
                                                        ClientIDMode = ClientIDMode.Static,
                                                        Text = "&nbsp",
                                                        Visible = false
                                                    };

                                                    lblSubledgerLabelGL.ID = "dic" + drCount.ToString("0000") + "50" + "set" + "2" + "01" + "id" + drCount.ToString("0000") + "dr";
                                                    //The Associate control id is the id of subledger dropdownlist
                                                    lblSubledgerLabelGL.AssociatedControlID = "dic" + drCount.ToString("0000") + "51" + "set" + "2" + "02" + "id" + drCount.ToString("0000") + "dr";

                                                    div.Controls.Add(lblSubledgerLabelGL);
                                                    if (lblSubledgerLabelGL != null)
                                                    {
                                                        if (!dicDrControls.ContainsKey(lblSubledgerLabelGL.ID))
                                                            dicDrControls.Add(lblSubledgerLabelGL.ID, "SubledgerLabel");
                                                    }
                                                    #endregion
                                                    #region SubLedger
                                                    //Subledger ddl control
                                                    DropDownList ddlSubLedgerGL = new DropDownList()
                                                    {
                                                        ClientIDMode = ClientIDMode.Static,
                                                        TabIndex = TabIndexDr,
                                                        Visible = false
                                                    };
                                                    ddlSubLedgerGL.ID = "dic" + drCount.ToString("0000") + "51" + "set" + "2" + "02" + "id" + drCount.ToString("0000") + "dr";
                                                    div.Controls.Add(ddlSubLedgerGL);
                                                    RequiredFieldValidator vrfSubLedgerGL = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + ddlSubLedgerGL.ID.Replace("dic", ""),
                                                        ControlToValidate = ddlSubLedgerGL.ID,
                                                        Text = "*",
                                                        CssClass = "star",
                                                        ValidationGroup = "voucher",
                                                        Display = ValidatorDisplay.Dynamic,
                                                        EnableClientScript = true,
                                                        InitialValue = CommonConstants.SELECTVAL,
                                                        SetFocusOnError = true,
                                                        ErrorMessage = GetLocalResourceObject("Err_SubLedger").ToString(),
                                                        ClientIDMode = ClientIDMode.Static
                                                    };
                                                    divValidation.Controls.Add(vrfSubLedgerGL);
                                                    vrfSubLedgerGL.Enabled = ddlSubLedgerGL.Visible ? true : false;
                                                    TabIndexDr++;
                                                    if (ddlSubLedgerGL != null)
                                                    {
                                                        if (!dicDrControls.ContainsKey(ddlSubLedgerGL.ID))
                                                            dicDrControls.Add(ddlSubLedgerGL.ID, "SubLedger");
                                                    }
                                                    #endregion
                                                    #region InstrumentNo
                                                    //Instrument No Text box
                                                    TextBox txtInstrumentNoGL = new TextBox()
                                                    {
                                                        Text = "",
                                                        MaxLength = 70,
                                                        ClientIDMode = ClientIDMode.Static,
                                                        TabIndex = TabIndexDr,
                                                        Enabled = true,
                                                        Visible = false,
                                                        CssClass = "input16"
                                                    };

                                                    txtInstrumentNoGL.ID = "dic" + drCount.ToString("0000") + "52" + "set" + "2" + "03" + "id" + drCount.ToString("0000") + "dr";
                                                    string instrumentNoGL = GetLocalResourceObject("InstrumentNo").ToString();
                                                    //Set "Instrument No" if their is no text in the textbox with a watermark style. Otherwise remove this style
                                                    txtInstrumentNoGL.Attributes.Remove("onblur");
                                                    txtInstrumentNoGL.Attributes.Remove("onfocus");
                                                    txtInstrumentNoGL.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + instrumentNoGL + "';$(this).addClass('input-watermark');}");
                                                    txtInstrumentNoGL.Attributes.Add("onfocus", "if (this.value == '" + instrumentNoGL + "') {this.value = '';$(this).removeClass('input-watermark');}");
                                                    txtInstrumentNoGL.Text = instrumentNoGL;
                                                    journalScript = journalScript + "if ($('#" + txtInstrumentNoGL.ID + "').val() == '" + instrumentNoGL + "') {$('#" + txtInstrumentNoGL.ID + "').addClass('input-watermark');}";
                                                    div.Controls.Add(txtInstrumentNoGL);
                                                    TabIndexDr++;
                                                    if (txtInstrumentNoGL != null)
                                                    {
                                                        if (!dicDrControls.ContainsKey(txtInstrumentNoGL.ID))
                                                            dicDrControls.Add(txtInstrumentNoGL.ID, "InstrumentNo");
                                                    }
                                                    #endregion
                                                    #region Date
                                                    //Date Picker Control
                                                    TextBox txtDateGL = new TextBox()
                                                    {
                                                        Text = "",
                                                        MaxLength = 11,
                                                        ClientIDMode = ClientIDMode.Static,
                                                        TabIndex = TabIndexDr,
                                                        Enabled = true,
                                                        Visible = false,
                                                        CssClass = "input-w8-5per"
                                                    };

                                                    txtDateGL.ID = "dic" + drCount.ToString("0000") + "53" + "set" + "2" + "04" + "id" + drCount.ToString("0000") + "dr";

                                                    txtDateGL.Attributes.Add("onkeydown", "return CheckKey(event);");
                                                    txtDateGL.Attributes.Add("onpaste", "return false;");

                                                    string dateGL = GetLocalResourceObject("Date").ToString();
                                                    //Set "Date" if their is no text in the textbox with a watermark style. Otherwise remove this style
                                                    txtDateGL.Attributes.Remove("onblur");
                                                    txtDateGL.Attributes.Remove("onfocus");
                                                    txtDateGL.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + dateGL + "';$(this).addClass('input-watermark');}");
                                                    txtDateGL.Attributes.Add("onfocus", "if (this.value == '" + dateGL + "') {this.value = '';$(this).removeClass('input-watermark');}");
                                                    txtDateGL.Text = dateGL;
                                                    journalScript = journalScript + "if ($('#" + txtDateGL.ID + "').val() == '" + dateGL + "') {$('#" + txtDateGL.ID + "').addClass('input-watermark');}";
                                                    div.Controls.Add(txtDateGL);

                                                    if (txtDateGL != null)
                                                    {
                                                        if (!dicDrControls.ContainsKey(txtDateGL.ID))
                                                            dicDrControls.Add(txtDateGL.ID, "Date");
                                                    }
                                                    TabIndexDr++;
                                                    #endregion
                                                    #region FavourOf
                                                    //Favourof Textbox
                                                    TextBox txtFavourOfGL = new TextBox()
                                                    {
                                                        MaxLength = 150,
                                                        ClientIDMode = ClientIDMode.Static,
                                                        TabIndex = TabIndexDr,
                                                        Enabled = true,
                                                        Visible = false,
                                                        CssClass = "input-w23-6per"
                                                    };

                                                    txtFavourOfGL.ID = "dic" + drCount.ToString("0000") + "54" + "set" + "2" + "05" + "id" + drCount.ToString("0000") + "dr";
                                                    string favourOfGL = GetLocalResourceObject("FavourOf").ToString();
                                                    //Set "FavourOf" if their is no text in the textbox with a watermark style. Otherwise remove this style
                                                    txtFavourOfGL.Attributes.Remove("onblur");
                                                    txtFavourOfGL.Attributes.Remove("onfocus");
                                                    txtFavourOfGL.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + favourOfGL + "';$(this).addClass('input-watermark');}");
                                                    txtFavourOfGL.Attributes.Add("onfocus", "if (this.value == '" + favourOfGL + "') {this.value = '';$(this).removeClass('input-watermark');}");
                                                    txtFavourOfGL.Text = favourOfGL;
                                                    journalScript = journalScript + "if ($('#" + txtFavourOfGL.ID + "').val() == '" + favourOfGL + "') {$('#" + txtFavourOfGL.ID + "').addClass('input-watermark');}";
                                                    div.Controls.Add(txtFavourOfGL);
                                                    TabIndexDr++;
                                                    if (txtFavourOfGL != null)
                                                    {
                                                        if (!dicDrControls.ContainsKey(txtFavourOfGL.ID))
                                                            dicDrControls.Add(txtFavourOfGL.ID, "FavourOf");
                                                    }
                                                    #endregion
                                                    //Now the div contains the new added Debit controls. Now we are going to add it to our page
                                                    tcControl.Controls.Add(div);//Adding to Table Cell
                                                    trControls.Cells.Add(tcControl);//Adding to Table Row
                                                    tbControls.Rows.Add(trControls);//Adding to Table
                                                    divGroupDr.Controls.Add(tbControls);//Adding to Masater Div that holds all the Debit Groups
                                                    if (dicDrControls != null && dicDrControls.Count > 0)
                                                        Session[ERP.Utilities.SessionStrings.DrControls] = dicDrControls;//Updating the Debit Control Session
                                                    if (dicControlinfo != null && dicControlinfo.Count > 0)
                                                        Session[ERP.Utilities.SessionStrings.ControlInfo] = dicControlinfo;//Updating the Control Info. Session
                                                    if (dicAccountType != null && dicAccountType.Count > 0)
                                                        Session[ERP.Utilities.SessionStrings.AccountType] = dicAccountType;//Updating the Account Name Session
                                                    #endregion
                                                }
                                                else if ((row["FTR_CR_AMT_TC"] != null && row["FTR_DR_AMT_BC"] != null) && Convert.ToDecimal(row["FTR_CR_AMT_TC"].ToString()) > 0 || Convert.ToDecimal(row["FTR_CR_AMT_BC"].ToString()) > 0)
                                                {
                                                    //G/L will be add to Credit Section
                                                    #region Add Credit
                                                    crCount = 1;
                                                    dicCrControls = new Dictionary<string, string>();
                                                    dicAccountType = new Dictionary<string, string>();
                                                    if (Session[ERP.Utilities.SessionStrings.AccountType] != null)
                                                    {
                                                        dicAccountType = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.AccountType];
                                                    }
                                                    else
                                                    {
                                                        dicAccountType = new Dictionary<string, string>();
                                                    }
                                                    if (Session[ERP.Utilities.SessionStrings.ControlInfo] != null)
                                                    {
                                                        dicControlinfo = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.ControlInfo];
                                                    }
                                                    else
                                                    {
                                                        dicControlinfo = new Dictionary<string, string>();
                                                    }

                                                    #region getID
                                                    //Generate the format of new Credit section id
                                                    //get the maximum group no from Cr section. create the next id. check whether this is in removed session or not. if yes create next one . check again and again. after all this steps we will get the new id.
                                                    if (Session[ERP.Utilities.SessionStrings.CrControls] != null)
                                                    {
                                                        dicCrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.CrControls];
                                                        var crControls = from pair in dicCrControls
                                                                         orderby pair.Key.Substring(0, 13) descending//orderby pair.Key.Substring(0, 9) descending
                                                                         select pair;
                                                        if (crControls != null && crControls.Count() > 0)
                                                        {
                                                            //Get the current maximum crcount
                                                            crCount = Convert.ToInt32(string.IsNullOrEmpty(crControls.First().Key.Substring(3, 4)) ? "0" : crControls.First().Key.Substring(3, 4));
                                                            crCount++;
                                                            //check if crCount is in deleted Cr controls list
                                                            if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)
                                                            {
                                                                //Gets the removed controls list
                                                                removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];
                                                                while (removedControlsList.Contains("dic" + crCount.ToString("0000") + "01" + "set" + "1" + "01" + "id" + crCount.ToString("0000") + "cr"))
                                                                {
                                                                    //If crcount is in removed controls list den increment drcount by 1
                                                                    crCount++;
                                                                }
                                                            }
                                                        }
                                                        else//if the UI contains no crcontrols
                                                        {
                                                            //check if crCount is in deleted Cr controls list
                                                            if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)
                                                            {
                                                                //Gets the removed controls list
                                                                removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];
                                                                while (removedControlsList.Contains("dic" + crCount.ToString("0000") + "01" + "set" + "1" + "01" + "id" + crCount.ToString("0000") + "cr"))
                                                                {
                                                                    //If crcount is in removed controls list den increment crcount by 1
                                                                    crCount++;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    #endregion

                                                    selectedAccountVal = "-1";
                                                    tbControls.CssClass = "";
                                                    divColStyle = "divcolmiddle-S1 input-margin2";
                                                    div = new HtmlGenericControl("div");//This div will hold the crcontrols that we are going to create.
                                                    div.Attributes.Add("class", divColStyle);//set the style for the div
                                                    divValidation = new HtmlGenericControl("div");//This div will hold all the validation controls that we are going to create.
                                                    divValidation.Attributes.Add("class", "starwrap");//set the style for the div
                                                    tbControls = new Table();
                                                    tbControls.CssClass = "";
                                                    trControls = new TableRow();
                                                    tcControl = new TableCell();
                                                    #region SubType
                                                    //Account Type
                                                    Label txtSubTypeCrGL = new Label()
                                                    {
                                                        Text = "",
                                                        ClientIDMode = ClientIDMode.Static

                                                    };
                                                    txtSubTypeCrGL.ID = "dic" + crCount.ToString("0000") + "01" + "set" + "1" + "01" + "id" + crCount.ToString("0000") + "cr";
                                                    txtSubTypeCrGL.Text = row["FTR_TYPE"] != null ? row["FTR_TYPE"].ToString() : GetLocalResourceObject("Gain_Loss_Account").ToString();
                                                    //Associate control id will be the id of Account Textbox. We can get the this id by just incrementing the control index in the set from the Account type ControlID
                                                    txtSubTypeCrGL.AssociatedControlID = "dic" + crCount.ToString("0000") + "02" + "set" + "1" + "02" + "id" + crCount.ToString("0000") + "cr";
                                                    div.Controls.Add(txtSubTypeCrGL);
                                                    if (txtSubTypeCrGL != null)
                                                    {
                                                        if (!dicCrControls.ContainsKey(txtSubTypeCrGL.ID))
                                                            dicCrControls.Add(txtSubTypeCrGL.ID, "SubType");//Add to Active dr Controls List
                                                        if (!dicAccountType.ContainsKey(txtSubTypeCrGL.ID))
                                                            dicAccountType.Add(txtSubTypeCrGL.ID, txtSubTypeCrGL.Text);//Add Account Name in dictionary
                                                    }
                                                    #endregion
                                                    #region Account
                                                    //It is an autocomplte control with postback. So we need Textbox,Hiddenfield and a Button.
                                                    TextBox txtAccountCrGL = new TextBox()
                                                    {
                                                        Text = "",
                                                        MaxLength = 100,
                                                        ClientIDMode = ClientIDMode.Static,
                                                        TabIndex = TabIndexCr,
                                                        Enabled = true
                                                    };
                                                    HiddenField hdfAccountCrGL = new HiddenField
                                                    {
                                                        ClientIDMode = ClientIDMode.Static
                                                    };
                                                    Button btnAccountCrGL = new Button
                                                    {
                                                        ClientIDMode = ClientIDMode.Static,
                                                        CommandName = "JOURNALACCOUNTINDEXCHANGED",
                                                        EnableTheming = false
                                                    };
                                                    btnAccountCrGL.Attributes.Add("style", "display:none;");

                                                    txtAccountCrGL.ID = "dic" + crCount.ToString("0000") + "02" + "set" + "1" + "02" + "id" + crCount.ToString("0000") + "cr";
                                                    hdfAccountCrGL.ID = "dic" + crCount.ToString("0000") + "03" + "set" + "1" + "03" + "id" + crCount.ToString("0000") + "cr";
                                                    btnAccountCrGL.ID = "dic" + crCount.ToString("0000") + "04" + "set" + "1" + "04" + "id" + crCount.ToString("0000") + "cr";
                                                    hdfSubTypePk.Value = row["FTR_ACC_SUB_TYPE"] != null ? row["FTR_ACC_SUB_TYPE"].ToString() : "0";

                                                    if (!dicControlinfo.ContainsKey(txtAccountCrGL.ID + "SubTypePk"))//Store Subtype PK
                                                        dicControlinfo.Add(txtAccountCrGL.ID + "SubTypePk", hdfSubTypePk.Value);
                                                    //Register Autocomplete script
                                                    journalScript = journalScript + "GrandScriptUtils.MakeAutoCompleteDDL('" + txtAccountCrGL.ID + "', (url1.indexOf('?') != -1 ? url1+'&' :  url1+'?') + 'AccType=" + hdfSubTypePk.Value + "', '" + hdfAccountCrGL.ID + "', true, true, 'JOURNALACCOUNT',false,false,false);";

                                                    hdfAccountCrGL.Value = row["FTR_ACCOUNT"] != null ? row["FTR_ACCOUNT"].ToString() :
                                                        !string.IsNullOrEmpty(hdfAccountCrGL.Value) ? hdfAccountCrGL.Value : "";

                                                    selectedAccountVal = hdfAccountCrGL.Value;
                                                    hdfCoaPk.Value = selectedAccountVal;

                                                    if (!dicControlinfo.ContainsKey(txtAccountCrGL.ID + "CoaPk"))
                                                        dicControlinfo.Add(txtAccountCrGL.ID + "CoaPk", hdfCoaPk.Value);

                                                    GetFieldValues(ControlsEnum.FINCOAMST);
                                                    if (finCoaMstList != null && finCoaMstList.Count == 1)
                                                    {
                                                        txtAccountCrGL.Text = HttpUtility.HtmlDecode(finCoaMstList[0].COA_CODE + " - " + finCoaMstList[0].COA_NAME);
                                                    }
                                                    //Register Button Event
                                                    btnAccountCrGL.Click += new EventHandler(ActionHandler);
                                                    //Add controls to div
                                                    div.Controls.Add(txtAccountCrGL);
                                                    div.Controls.Add(hdfAccountCrGL);
                                                    div.Controls.Add(btnAccountCrGL);
                                                    RequiredFieldValidator vrfAccountCrGL = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + txtAccountCrGL.ID.Replace("dic", ""),
                                                        ControlToValidate = txtAccountCrGL.ID,
                                                        Text = "*",
                                                        CssClass = "star",
                                                        ValidationGroup = "voucher",
                                                        Display = ValidatorDisplay.Dynamic,
                                                        EnableClientScript = true,
                                                        InitialValue = Resources.Messages.AutoDefaultValue,
                                                        SetFocusOnError = true,
                                                        ErrorMessage = GetLocalResourceObject("Err_Account").ToString(),
                                                        ClientIDMode = ClientIDMode.Static
                                                    };
                                                    //Add validation control to validation div
                                                    divValidation.Controls.Add(vrfAccountCrGL);


                                                    TabIndexCr++;
                                                    if (txtAccountCrGL != null)
                                                        if (!dicCrControls.ContainsKey(txtAccountCrGL.ID))
                                                            dicCrControls.Add(txtAccountCrGL.ID, "Account");//Add to Active cr Controls List
                                                    if (hdfAccountCrGL != null)
                                                        if (!dicCrControls.ContainsKey(hdfAccountCrGL.ID))
                                                            dicCrControls.Add(hdfAccountCrGL.ID, "Account");//Add to Active cr Controls List
                                                    if (btnAccountCrGL != null)
                                                        if (!dicCrControls.ContainsKey(btnAccountCrGL.ID))
                                                            dicCrControls.Add(btnAccountCrGL.ID, "Account");//Add to Active cr Controls List

                                                    #endregion
                                                    #region Narration
                                                    //Narration Textbox Control
                                                    TextBox txtNarrationCrGL = new TextBox()
                                                    {
                                                        Text = "",
                                                        MaxLength = 400,
                                                        ClientIDMode = ClientIDMode.Static,
                                                        TabIndex = TabIndexCr,
                                                        Enabled = true,
                                                        Width = IsSameCurrency() == true ? new Unit(GetLocalResourceObject("NarationWidthWithoutAmountBC").ToString()) : new Unit(GetLocalResourceObject("NarationWidthNormal").ToString())
                                                    };
                                                    txtNarrationCrGL.ID = "dic" + crCount.ToString("0000") + "05" + "set" + "1" + "05" + "id" + crCount.ToString("0000") + "cr";
                                                    div.Controls.Add(txtNarrationCrGL);
                                                    TabIndexCr++;
                                                    if (txtNarrationCrGL != null)
                                                    {
                                                        if (!dicCrControls.ContainsKey(txtNarrationCrGL.ID))
                                                            dicCrControls.Add(txtNarrationCrGL.ID, "Narration");//Add to Active cr Controls List
                                                    }
                                                    #endregion
                                                    #region AmountTC
                                                    //Amount in Transaction Currency
                                                    TextBox txtAmountTCCrGL = new TextBox()
                                                    {
                                                        Text = "",
                                                        MaxLength = 15,
                                                        ClientIDMode = ClientIDMode.Static,
                                                        Enabled = true,
                                                        TabIndex = TabIndexCr,
                                                        CssClass = "input-normalb input-w8per numeric amounttccr tempcss"
                                                    };
                                                    txtAmountTCCrGL.ID = "dic" + crCount.ToString("0000") + "06" + "set" + "1" + "06" + "id" + crCount.ToString("0000") + "cr";
                                                    txtAmountTCCrGL.Text = string.Empty;

                                                    txtAmountTCCrGL.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                                                    txtAmountTCCrGL.Attributes.Add("onpaste", "return false;");

                                                    div.Controls.Add(txtAmountTCCrGL);

                                                    AmountValidation vamAmountTCCrGL = new AmountValidation()
                                                    {
                                                        ID = "vam" + txtAmountTCCrGL.ID.Replace("dic", ""),
                                                        ControlToValidate = txtAmountTCCrGL.ID,
                                                        ErrorMessage = GetLocalResourceObject("MsgErr_AmountTc").ToString(),
                                                        NumberDigits = 11,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        EnableClientScript = true,
                                                        CssClass = "star",
                                                        ValidationGroup = "voucher",
                                                        NonZero = true
                                                    };
                                                    divValidation.Controls.Add(vamAmountTCCrGL);

                                                    TabIndexCr++;
                                                    if (txtAmountTCCrGL != null)
                                                    {
                                                        if (!dicCrControls.ContainsKey(txtAmountTCCrGL.ID))
                                                            dicCrControls.Add(txtAmountTCCrGL.ID, "AmountTC");
                                                    }
                                                    #endregion
                                                    #region ExchangeRate
                                                    //Exchange Rate
                                                    TextBox txtExchangeRateCrGL = new TextBox()
                                                    {
                                                        Text = "",
                                                        MaxLength = 9,
                                                        ClientIDMode = ClientIDMode.Static,
                                                        Enabled = true,
                                                        TabIndex = TabIndexCr,
                                                        CssClass = "input-normalb numeric input-w6per"
                                                    };
                                                    txtExchangeRateCrGL.ID = "dic" + crCount.ToString("0000") + "07" + "set" + "1" + "07" + "id" + crCount.ToString("0000") + "cr";
                                                    txtExchangeRateCrGL.Text = string.Empty;
                                                    HiddenField hdfEntryModeCrGL = new HiddenField()
                                                    {
                                                        ClientIDMode = ClientIDMode.Static
                                                    };
                                                    hdfEntryModeCrGL.ID = txtExchangeRateCrGL.ID + "EntryMode";//Used for to keep the entrymode of ExchangeRate in the credit detail section

                                                    TabIndexCr++;
                                                    hdfEntryModeCrGL.Value = row["FTR_ENTRY_MODE"] != null ? row["FTR_ENTRY_MODE"].ToString() : ((byte)VoucherEntryMode.Editable).ToString();
                                                    if (!dicControlinfo.ContainsKey(txtExchangeRateCrGL.ID + "EntryMode"))
                                                        dicControlinfo.Add(txtExchangeRateCrGL.ID + "EntryMode", hdfEntryModeCrGL.Value);//Keep the entrymode for the curresponding Exchange Rate in Dictionary

                                                    txtExchangeRateCrGL.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                                                    txtExchangeRateCrGL.Attributes.Add("onpaste", "return false;");

                                                    if (txtExchangeRateCrGL != null)
                                                    {
                                                        if (!dicCrControls.ContainsKey(txtExchangeRateCrGL.ID))
                                                            dicCrControls.Add(txtExchangeRateCrGL.ID, "ExchangeRate");
                                                        if (hdfEntryModeCrGL != null)
                                                        {
                                                            if (!dicCrControls.ContainsKey(hdfEntryModeCrGL.ID))
                                                                dicCrControls.Add(hdfEntryModeCrGL.ID, "EntryMode");
                                                        }
                                                    }

                                                    transactionCurrency = 0;
                                                    if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                                                    {
                                                        transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                                                    }
                                                    else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                                                    {
                                                        transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);
                                                    }

                                                    if (transactionCurrency > 0
                                                        &&
                                                        transactionCurrency == currentUser.BaseCurrency)
                                                    {
                                                        txtExchangeRateCrGL.Visible = false;
                                                    }
                                                    div.Controls.Add(txtExchangeRateCrGL);
                                                    div.Controls.Add(hdfEntryModeCrGL);
                                                    ExchangeRateValidation vreExchangeRateCrGL = new ExchangeRateValidation()
                                                    {
                                                        ID = "vre" + txtExchangeRateCrGL.ID,
                                                        ControlToValidate = txtExchangeRateCrGL.ID,
                                                        ErrorMessage = GetLocalResourceObject("MsgErr_ExchangeRate").ToString(),
                                                        NumberDigits = 5,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        EnableClientScript = true,
                                                        CssClass = "star",
                                                        ValidationGroup = "voucher",
                                                        NonZero = true
                                                    };
                                                    divValidation.Controls.Add(vreExchangeRateCrGL);
                                                    #endregion
                                                    #region AmountBC
                                                    //Amount in Base Currency
                                                    TextBox txtAmountBCCrGL = new TextBox()
                                                    {
                                                        Text = "",
                                                        ClientIDMode = ClientIDMode.Static,
                                                        Enabled = true,
                                                        MaxLength = 15,
                                                        CssClass = "input-bgGreen input-normalb input-w8per numeric tempcss"
                                                    };
                                                    txtAmountBCCrGL.ID = "dic" + crCount.ToString("0000") + "08" + "set" + "1" + "08" + "id" + crCount.ToString("0000") + "cr";
                                                    txtAmountBCCrGL.Text = ((decimal)0).ToString(hdfCurrencyFormatVoucher.Value);
                                                    if (IsEditableForApplication())
                                                    {
                                                        //txtAmountBCCrGL.CssClass = "input-w8per numeric";
                                                        txtAmountBCCrGL.Attributes.Add("onkeyup", "CalculateTCAmt(this,event);");
                                                    }
                                                    else
                                                    {
                                                        txtAmountBCCrGL.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                                                        txtAmountBCCrGL.Attributes.Add("onpaste", "return false;");
                                                    }
                                                    txtAmountBCCrGL.ForeColor = Color.Red;
                                                    txtAmountBCCrGL.Text = row["FTR_CR_AMT_BC"] != null ?
                                                        Math.Round(Convert.ToDecimal(row["FTR_CR_AMT_BC"].ToString()), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString() :
                                                        Convert.ToDecimal("0").ToString();
                                                    if (!dicControlinfo.ContainsKey(txtAmountBCCrGL.ID + "GL"))
                                                        dicControlinfo.Add(txtAmountBCCrGL.ID + "GL", GainLossMode.Gain + "");
                                                    div.Controls.Add(txtAmountBCCrGL);
                                                    AmountValidation vamAmountBCCrGL = new AmountValidation()
                                                    {
                                                        ID = "vam" + txtAmountBCCrGL.ID.Replace("dic", ""),
                                                        ControlToValidate = txtAmountBCCrGL.ID,
                                                        ErrorMessage = GetLocalResourceObject("MsgErr_AmountBc").ToString(),
                                                        NumberDigits = 11,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        EnableClientScript = true,
                                                        CssClass = "star",
                                                        ValidationGroup = "voucher"
                                                    };
                                                    divValidation.Controls.Add(vamAmountBCCrGL);
                                                    if (txtAmountBCCrGL != null)
                                                    {
                                                        if (!dicCrControls.ContainsKey(txtAmountBCCrGL.ID))
                                                            dicCrControls.Add(txtAmountBCCrGL.ID, "AmountBC");
                                                    }

                                                    transactionCurrency = 0;
                                                    if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                                                    {
                                                        transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                                                    }
                                                    else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                                                    {
                                                        transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);
                                                    }

                                                    if (transactionCurrency > 0
                                                        &&
                                                        transactionCurrency == currentUser.BaseCurrency)
                                                    {
                                                        txtAmountBCCrGL.Visible = false;
                                                    }
                                                    #endregion
                                                    #region Delete
                                                    //Delete button for deleting the Curresponding DR section
                                                    Button btnDeleteCrGL = new Button()
                                                    {
                                                        Text = "",
                                                        ClientIDMode = ClientIDMode.Static,
                                                        TabIndex = TabIndexCr,
                                                        Enabled = true,
                                                        SkinID = "delete-icon",
                                                        ToolTip = "Delete",
                                                        OnClientClick = "return ShowDeleteConfirm(this);"
                                                    };
                                                    btnDeleteCrGL.ID = "dic" + crCount.ToString("0000") + "09" + "set" + "1" + "09" + "id" + crCount.ToString("0000") + "cr";
                                                    btnDeleteCrGL.CommandName = ActionsEnum.REMOVECREDIT.ToString();

                                                    btnDeleteCrGL.Click += new EventHandler(ActionHandler);


                                                    if (EntryStatus == EntryStatus.VIEWMODE)
                                                    {
                                                        btnDeleteCrGL.Visible = false;
                                                    }

                                                    div.Controls.Add(btnDeleteCrGL);
                                                    //divValidation contains all the Validation Controls in a group. It will be added to the group after delete button. So if any validation catch it will show * after delete button
                                                    div.Controls.Add(divValidation);
                                                    TabIndexCr++;
                                                    if (btnDeleteCrGL != null)
                                                    {
                                                        if (!dicCrControls.ContainsKey(btnDeleteCrGL.ID))
                                                            dicCrControls.Add(btnDeleteCrGL.ID, "Delete");
                                                    }
                                                    #endregion
                                                    #region Cost Center
                                                    //Delete button for deleting the Curresponding DR section
                                                    Button btnCostCenterCrGL = new Button()
                                                    {
                                                        Text = "",
                                                        ClientIDMode = ClientIDMode.Static,
                                                        TabIndex = TabIndexCr,
                                                        Enabled = true,
                                                        SkinID = "costcenter-icon",
                                                        ToolTip = GetLocalResourceObject("CCButtonToolTip").ToString()
                                                    };

                                                    HiddenField hdfIsCostCenterCrGL = new HiddenField
                                                    {
                                                        ClientIDMode = ClientIDMode.Static,
                                                        Value = "0"
                                                    };

                                                    btnCostCenterCrGL.ID = "dic" + crCount.ToString("0000") + "10" + "set" + "1" + "10" + "id" + crCount.ToString("0000") + "cr";
                                                    hdfIsCostCenterCrGL.ID = "dic" + crCount.ToString("0000") + "11" + "set" + "1" + "11" + "id" + crCount.ToString("0000") + "cr";
                                                    btnCostCenterCrGL.CommandName = ActionsEnum.COSTCENTER.ToString();

                                                    btnCostCenterCrGL.Click += new EventHandler(ActionHandler);

                                                    div.Controls.Add(btnCostCenterCrGL);

                                                    //divValidation contains all the Validation Controls in a group. It will be added to the group after delete button. So if any validation catch it will show * after delete button
                                                    div.Controls.Add(divValidation);

                                                    div.Controls.Add(hdfIsCostCenterCrGL);
                                                    TabIndexCr++;
                                                    if (btnCostCenterCrGL != null)
                                                    {
                                                        if (!dicCrControls.ContainsKey(btnCostCenterCrGL.ID))
                                                            dicCrControls.Add(btnCostCenterCrGL.ID, "CostCenter");
                                                    }
                                                    if (hdfIsCostCenterCrGL != null)
                                                    {
                                                        if (!dicCrControls.ContainsKey(hdfIsCostCenterCrGL.ID))
                                                            dicCrControls.Add(hdfIsCostCenterCrGL.ID, "CostCenter");
                                                    }

                                                    if (!string.IsNullOrEmpty(hdfCoaPk.Value)
                                                       && finCoaMstList != null
                                                       && finCoaMstList.Count == 1
                                                       && finCoaMstList[0].FIN_COA_COST_CENTER_MPG != null
                                                       && finCoaMstList[0].FIN_COA_COST_CENTER_MPG.Where(r => r.ADM_COST_CENTER_MST.CNM_ACTIVE == 1).Count() > 0)
                                                    {
                                                        hdfIsCostCenterCrGL.Value = "1";
                                                        btnCostCenterCrGL.Visible = true;
                                                    }
                                                    else
                                                    {
                                                        hdfIsCostCenterCrGL.Value = "0";
                                                        btnCostCenterCrGL.Visible = false;
                                                    }
                                                    #endregion
                                                    #region SubledgerLabel
                                                    //After Delete Button, the rest of the controls will comes in the next line. So the divClear will use to break the first line
                                                    divClear = new HtmlGenericControl("div");
                                                    divClear.Attributes.Add("class", "clear");
                                                    div.Controls.Add(divClear);
                                                    //It is a dummy label
                                                    Label lblSubledgerLabelCrGL = new Label()
                                                    {
                                                        ClientIDMode = ClientIDMode.Static,
                                                        Text = "&nbsp",
                                                        Visible = false
                                                    };

                                                    lblSubledgerLabelCrGL.ID = "dic" + crCount.ToString("0000") + "50" + "set" + "2" + "01" + "id" + crCount.ToString("0000") + "cr";
                                                    //The Associate control id is the id of subledger dropdownlist
                                                    lblSubledgerLabelCrGL.AssociatedControlID = "dic" + crCount.ToString("0000") + "51" + "set" + "2" + "02" + "id" + crCount.ToString("0000") + "cr";

                                                    div.Controls.Add(lblSubledgerLabelCrGL);
                                                    if (lblSubledgerLabelCrGL != null)
                                                    {
                                                        if (!dicCrControls.ContainsKey(lblSubledgerLabelCrGL.ID))
                                                            dicCrControls.Add(lblSubledgerLabelCrGL.ID, "SubledgerLabel");
                                                    }
                                                    #endregion
                                                    #region SubLedger
                                                    //Subledger ddl control
                                                    DropDownList ddlSubLedgerCrGL = new DropDownList()
                                                    {
                                                        ClientIDMode = ClientIDMode.Static,
                                                        TabIndex = TabIndexCr,
                                                        Visible = false
                                                    };

                                                    ddlSubLedgerCrGL.ID = "dic" + crCount.ToString("0000") + "51" + "set" + "2" + "02" + "id" + crCount.ToString("0000") + "cr";

                                                    div.Controls.Add(ddlSubLedgerCrGL);
                                                    RequiredFieldValidator vrfSubLedgerCrGL = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + ddlSubLedgerCrGL.ID.Replace("dic", ""),
                                                        ControlToValidate = ddlSubLedgerCrGL.ID,
                                                        Text = "*",
                                                        CssClass = "star",
                                                        ValidationGroup = "voucher",
                                                        Display = ValidatorDisplay.Dynamic,
                                                        EnableClientScript = true,
                                                        InitialValue = CommonConstants.SELECTVAL,
                                                        SetFocusOnError = true,
                                                        ErrorMessage = GetLocalResourceObject("Err_SubLedger").ToString(),
                                                        ClientIDMode = ClientIDMode.Static
                                                    };
                                                    divValidation.Controls.Add(vrfSubLedgerCrGL);
                                                    vrfSubLedgerCrGL.Enabled = ddlSubLedgerCrGL.Visible ? true : false;
                                                    TabIndexCr++;
                                                    if (ddlSubLedgerCrGL != null)
                                                    {
                                                        if (!dicCrControls.ContainsKey(ddlSubLedgerCrGL.ID))
                                                            dicCrControls.Add(ddlSubLedgerCrGL.ID, "SubLedger");
                                                    }
                                                    #endregion
                                                    #region InstrumentNo
                                                    //Instrument No Text box
                                                    TextBox txtInstrumentNoCrGL = new TextBox()
                                                    {
                                                        Text = "",
                                                        MaxLength = 70,
                                                        ClientIDMode = ClientIDMode.Static,
                                                        TabIndex = TabIndexCr,
                                                        Enabled = true,
                                                        Visible = false,
                                                        CssClass = "input16"
                                                    };

                                                    txtInstrumentNoCrGL.ID = "dic" + crCount.ToString("0000") + "52" + "set" + "2" + "03" + "id" + crCount.ToString("0000") + "cr";

                                                    string instrumentNoCrGL = GetLocalResourceObject("InstrumentNo").ToString();
                                                    //Set "Instrument No" if their is no text in the textbox with a watermark style. Otherwise remove this style
                                                    txtInstrumentNoCrGL.Attributes.Remove("onblur");
                                                    txtInstrumentNoCrGL.Attributes.Remove("onfocus");
                                                    txtInstrumentNoCrGL.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + instrumentNoCrGL + "';$(this).addClass('input-watermark');}");
                                                    txtInstrumentNoCrGL.Attributes.Add("onfocus", "if (this.value == '" + instrumentNoCrGL + "') {this.value = '';$(this).removeClass('input-watermark');}");
                                                    txtInstrumentNoCrGL.Text = instrumentNoCrGL;
                                                    journalScript = journalScript + "if ($('#" + txtInstrumentNoCrGL.ID + "').val() == '" + instrumentNoCrGL + "') {$('#" + txtInstrumentNoCrGL.ID + "').addClass('input-watermark');}";

                                                    div.Controls.Add(txtInstrumentNoCrGL);
                                                    TabIndexCr++;
                                                    if (txtInstrumentNoCrGL != null)
                                                    {
                                                        if (!dicCrControls.ContainsKey(txtInstrumentNoCrGL.ID))
                                                            dicCrControls.Add(txtInstrumentNoCrGL.ID, "InstrumentNo");
                                                    }
                                                    #endregion
                                                    #region Date
                                                    //Date Picker Control
                                                    TextBox txtDateCrGL = new TextBox()
                                                    {
                                                        Text = "",
                                                        MaxLength = 11,
                                                        ClientIDMode = ClientIDMode.Static,
                                                        TabIndex = TabIndexCr,
                                                        Enabled = true,
                                                        Visible = false,
                                                        CssClass = "input-w8-5per"
                                                    };

                                                    txtDateCrGL.ID = "dic" + crCount.ToString("0000") + "53" + "set" + "2" + "04" + "id" + crCount.ToString("0000") + "cr";
                                                    txtDateCrGL.Attributes.Add("onkeydown", "return CheckKey(event);");
                                                    txtDateCrGL.Attributes.Add("onpaste", "return false;");

                                                    string dateCrGL = GetLocalResourceObject("Date").ToString();
                                                    //Set "Date" if their is no text in the textbox with a watermark style. Otherwise remove this style
                                                    txtDateCrGL.Attributes.Remove("onblur");
                                                    txtDateCrGL.Attributes.Remove("onfocus");
                                                    txtDateCrGL.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + dateCrGL + "';$(this).addClass('input-watermark');}");
                                                    txtDateCrGL.Attributes.Add("onfocus", "if (this.value == '" + dateCrGL + "') {this.value = '';$(this).removeClass('input-watermark');}");
                                                    txtDateCrGL.Text = dateCrGL;
                                                    journalScript = journalScript + "if ($('#" + txtDateCrGL.ID + "').val() == '" + dateCrGL + "') {$('#" + txtDateCrGL.ID + "').addClass('input-watermark');}";

                                                    div.Controls.Add(txtDateCrGL);
                                                    TabIndexCr++;
                                                    if (txtDateCrGL != null)
                                                    {
                                                        if (!dicCrControls.ContainsKey(txtDateCrGL.ID))
                                                            dicCrControls.Add(txtDateCrGL.ID, "Date");
                                                    }
                                                    #endregion
                                                    #region FavourOf
                                                    //Favourof Textbox
                                                    TextBox txtFavourOfCrGL = new TextBox()
                                                    {
                                                        MaxLength = 150,
                                                        ClientIDMode = ClientIDMode.Static,
                                                        TabIndex = TabIndexCr,
                                                        Enabled = true,
                                                        Visible = false,
                                                        CssClass = "input-w23-6per"
                                                    };

                                                    txtFavourOfCrGL.ID = "dic" + crCount.ToString("0000") + "54" + "set" + "2" + "05" + "id" + crCount.ToString("0000") + "cr";

                                                    string favourOfCrGL = GetLocalResourceObject("FavourOf").ToString();
                                                    //Set "FavourOf" if their is no text in the textbox with a watermark style. Otherwise remove this style
                                                    txtFavourOfCrGL.Attributes.Remove("onblur");
                                                    txtFavourOfCrGL.Attributes.Remove("onfocus");
                                                    txtFavourOfCrGL.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + favourOfCrGL + "';$(this).addClass('input-watermark');}");
                                                    txtFavourOfCrGL.Attributes.Add("onfocus", "if (this.value == '" + favourOfCrGL + "') {this.value = '';$(this).removeClass('input-watermark');}");
                                                    txtFavourOfCrGL.Text = favourOfCrGL;
                                                    journalScript = journalScript + "if ($('#" + txtFavourOfCrGL.ID + "').val() == '" + favourOfCrGL + "') {$('#" + txtFavourOfCrGL.ID + "').addClass('input-watermark');}";
                                                    div.Controls.Add(txtFavourOfCrGL);
                                                    TabIndexCr++;
                                                    if (txtFavourOfCrGL != null)
                                                    {
                                                        if (!dicCrControls.ContainsKey(txtFavourOfCrGL.ID))
                                                            dicCrControls.Add(txtFavourOfCrGL.ID, "FavourOf");
                                                    }
                                                    #endregion
                                                    //Now the div contains the new added Credit controls. Now we are going to add it to our page
                                                    tcControl.Controls.Add(div);//Adding to Table Cell
                                                    trControls.Cells.Add(tcControl);//Adding to Table Row
                                                    tbControls.Rows.Add(trControls);//Adding to Table
                                                    divGroupCr.Controls.Add(tbControls);//Adding to Masater Div that holds all the Credit Groups
                                                    if (dicCrControls != null && dicCrControls.Count > 0)
                                                        Session[ERP.Utilities.SessionStrings.CrControls] = dicCrControls;//Updating the Credit Control Session
                                                    if (dicControlinfo != null && dicControlinfo.Count > 0)
                                                        Session[ERP.Utilities.SessionStrings.ControlInfo] = dicControlinfo;//Updating the Control Info. Session
                                                    if (dicAccountType != null && dicAccountType.Count > 0)
                                                        Session[ERP.Utilities.SessionStrings.AccountType] = dicAccountType;//Updating the Account Name Session
                                                    #endregion
                                                }
                                                else
                                                {

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            hdfExchangeRateChange.Value = "1";
                            //Calculate new total
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "calculateTotal", "$(document).ready(function(){calculateTotal();});", true);

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                            "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                           "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                            if (drTotal == 0 && crTotal == 0)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_ErrZero_Amount_GL").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_ErrAmount_NotValid").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region SAVEASTEMPLATE
                    case ActionsEnum.SAVEASTEMPLATE:
                        //Used to Save selected accounts as template for future use
                        JVTemplateBOObj = (JVTemplateBO)SetUIValuesToObject(ActionsEnum.SAVEASTEMPLATE);
                        if (JVTemplateBOObj != null && JVTemplateBOObj.Detail != null && JVTemplateBOObj.Detail.Count > 0)
                        {
                            GetFieldValues(ControlsEnum.TEMPLATECATEGORY);//Geta Template Categories
                            SetFiledValues(ControlsEnum.TEMPLATECATEGORY);//Set Template Categories
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                      "ClosePopup();", true);
                            //Show Template POPUP
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divTemplate]','" + GetLocalResourceObject("Voucher_Template").ToString() + "','650','150');", true);

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                                                  "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                            litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSelect_Account_4_Template").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region TEMPLATEOK
                    case ActionsEnum.TEMPLATEOK:
                        JVTemplateBOObj.VLH_NAME = HttpUtility.HtmlEncode(txtTemplateName.Text.Trim());//Template name
                        JVTemplateBOObj.VLH_TYPE = Convert.ToInt32(ddlTemplateCategory.SelectedValue);//Template Category
                        JVTemplateBOObj.VLH_DESC = string.Empty;//Description
                        xmlDoc = CommonFunctions.XmlSerialize<JVTemplateBO>(JVTemplateBOObj);//generate XML
                        jurResult = BusinessLogic.Jouralize.JournalizeBL.SaveVoucherTemplate(xmlDoc);//Save Voucher template
                        if (jurResult > 0)//Success
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("Voucher_Template").ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop2",
                                      "ClosePopup();", true);
                            //Show Voucher Popup
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                    "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//Failed
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop2",
                                      "ClosePopup();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                    "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Template_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        JVTemplateBOObj = null;
                        break;
                    #endregion
                    #region TEMPLATECANCEL
                    case ActionsEnum.TEMPLATECANCEL:
                        JVTemplateBOObj = null;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop2",
                                      "ClosePopup();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                    "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                        break;
                    #endregion
                    #region COST CENTER
                    case ActionsEnum.COSTCENTER:
                        senderId = ((Button)sender).ID;
                        hdfAccountCtrlID = senderId.Replace("10set110", "03set103");
                        HiddenField hdfAccountPk = (HiddenField)pnlControls.FindControl(hdfAccountCtrlID);//Get AutoComplete Hiddenfield
                        AccountPk = 0;
                        ControlId = string.Empty;
                        if (hdfAccountPk != null)
                        {
                            int.TryParse(hdfAccountPk.Value, out AccountPk);
                            ControlId = hdfAccountCtrlID;
                        }
                        string txtAmountTCId = senderId.Replace("10set110", "06set106");
                        TextBox txtTCAmount = (TextBox)pnlControls.FindControl(txtAmountTCId);//Get Amount text box for transaction currency
                        if (txtTCAmount != null)
                            decimal.TryParse(txtTCAmount.Text, out CCAmount);
                        SelAccountCCAmount = CCAmount;
                        CostCenterTempList = CostCenterList;
                        List<CostCenterDetails> objCostCentrDtls = CostCenterList.Where(r => r.CONTROL_ID == ControlId).ToList();
                        bool ccPercentageRequired = (GetGlobalResourceObject("ConfigurationsRes", "CCPercentageRequired").ToString() == "1") ? true : false;
                        if (ccPercentageRequired && objCostCentrDtls != null && objCostCentrDtls.Count > 0 && (ERP.Utilities.CommonFunctions.DecimalFormat(objCostCentrDtls.Sum(r => r.FTD_AMT_BC), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) != CCAmount))
                        {
                            #region If the amount was changed, reallocate Cost center allocation
                            if (objCostCentrDtls != null)
                            {
                                objCostCentrDtls.ForEach(dtl =>
                                {
                                    CostCenterTempList.Remove(dtl);
                                });
                            }
                            GetFieldValues(ControlsEnum.COSTCENTER);
                            if (dtCostCenter != null && dtCostCenter.Rows.Count > 0)
                            {
                                foreach (DataRow drRow in dtCostCenter.Rows)
                                {
                                    CostCenterDetails objCCDtls = new CostCenterDetails();
                                    objCCDtls.FCM_CNM_PK = Convert.ToInt32(drRow["FCM_CNM_PK"]);
                                    objCCDtls.FCM_COST_CENTER_TEXT = CommonFunctions.GetEncodedString(drRow["FCM_COST_CENTER_TEXT"]);
                                    objCCDtls.FTD_AMT_BC = Convert.ToDecimal(drRow["FCM_AMOUNT"]);
                                    objCCDtls.CONTROL_ID = ControlId;
                                    CostCenterTempList.Add(objCCDtls);
                                }
                            }
                            #endregion
                        }
                        else if (objCostCentrDtls == null || objCostCentrDtls.Count == 0 || objCostCentrDtls.Sum(r => r.FTD_AMT_BC) == 0)// Before code alteration
                        {
                            #region MyRegion
                            if (objCostCentrDtls != null && objCostCentrDtls.Sum(r => r.FTD_AMT_BC) == 0)
                            {
                                objCostCentrDtls.ForEach(dtl =>
                                {
                                    CostCenterTempList.Remove(dtl);
                                });
                            }

                            GetFieldValues(ControlsEnum.COSTCENTER);
                            if (dtCostCenter != null && dtCostCenter.Rows.Count > 0)
                            {
                                foreach (DataRow drRow in dtCostCenter.Rows)
                                {
                                    CostCenterDetails objCCDtls = new CostCenterDetails();
                                    objCCDtls.FCM_CNM_PK = Convert.ToInt32(drRow["FCM_CNM_PK"]);
                                    objCCDtls.FCM_COST_CENTER_TEXT = CommonFunctions.GetEncodedString(drRow["FCM_COST_CENTER_TEXT"]);
                                    objCCDtls.FTD_AMT_BC = Convert.ToDecimal(drRow["FCM_AMOUNT"]);
                                    objCCDtls.CONTROL_ID = ControlId;
                                    CostCenterTempList.Add(objCCDtls);
                                }
                            }
                            #endregion
                        }
                        CostCenterList = CostCenterTempList;
                        SetFiledValues(ControlsEnum.COSTCENTER);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalCostCenterAmount", "$(document).ready(function(){CalculateTotalCostCenterAmount();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divCostCenters]','" + GetLocalResourceObject("CostCenterBreakup").ToString() + "','450','350');", true);
                        break;
                    #endregion
                    #region COST CENTER APPLY
                    case ActionsEnum.COSTCENTERAPPLY:
                        int flag = 0;
                        if (grdCostCenter.Rows.Count > 0)
                        {
                            CostCenterTempList = CostCenterList.DeepClone();
                            foreach (GridViewRow grvRow in grdCostCenter.Rows)
                            {
                                CCAmount = 0;
                                HiddenField hdfCCPk = (HiddenField)grvRow.FindControl("hdfCCPk");
                                Label lblCC = (Label)grvRow.FindControl("lblCC");
                                TextBox txtCCAmountTC = (TextBox)grvRow.FindControl("txtCCAmountTC");
                                decimal.TryParse(txtCCAmountTC.Text, out CCAmount);
                                if (!string.IsNullOrEmpty(ControlId))
                                {
                                    CostCenterDetails objCCDtls = CostCenterTempList.SingleOrDefault(r => r.CONTROL_ID == ControlId && r.FCM_CNM_PK == Convert.ToInt32(hdfCCPk.Value));
                                    if (objCCDtls != null)
                                    {
                                        objCCDtls.FCM_CNM_PK = Convert.ToInt32(hdfCCPk.Value);
                                        objCCDtls.FCM_COST_CENTER_TEXT = CommonFunctions.GetEncodedString(lblCC.ToolTip);
                                        objCCDtls.FTD_AMT_BC = CCAmount;
                                    }
                                    else
                                    {
                                        objCCDtls = new CostCenterDetails();
                                        objCCDtls.FCM_CNM_PK = Convert.ToInt32(hdfCCPk.Value);
                                        objCCDtls.FCM_COST_CENTER_TEXT = CommonFunctions.GetEncodedString(lblCC.ToolTip);
                                        objCCDtls.FTD_AMT_BC = CCAmount;
                                        objCCDtls.CONTROL_ID = ControlId;
                                        CostCenterTempList.Add(objCCDtls);
                                    }
                                }
                            }
                            if (IsValidCostCenterSplitAllocation(CostCenterTempList.Where(r => r.CONTROL_ID == ControlId).ToList(), SelAccountCCAmount)) // Validation for cost center split missmatch    
                            {
                                CostCenterList = CostCenterTempList;
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalCostCenterAmount", "$(document).ready(function(){CalculateTotalCostCenterAmount();});", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divCostCenters]','" + GetLocalResourceObject("CostCenterBreakup").ToString() + "','450','350');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_CC_SplitMissmatch").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                flag = 1;
                            }
                        }
                        if (flag == 0)
                        {
                            ControlId = string.Empty;
                            SelAccountCCAmount = 0;
                            ShowVoucherPopup();
                        }
                        break;
                    #endregion
                    #region COST CENTER CANCEL
                    case ActionsEnum.COSTCENTERCANCEL:
                        ControlId = string.Empty;
                        SelAccountCCAmount = 0;
                        ShowVoucherPopup();
                        break;
                    #endregion
                    #region AUDIT LOG
                    case ActionsEnum.AUDITLOG:
                        if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() != "DPVJ")
                        {
                            ucAuditLogList.ResetGrivRowColor();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divAuditLog]','" + GetLocalResourceObject("EditLog").ToString() + "','605','300');", true);
                        }
                        break;
                    #endregion
                    #region COMPARE
                    case ActionsEnum.COMPARE:
                        if (Session[ERP.Utilities.SessionStrings.TransactionType] != null)
                        {

                            if (!string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionType].ToString()))
                            {
                                if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() != "DPVJ")
                                {
                                    HiddenField hdfSelRow = (HiddenField)ucAuditLogList.FindControl("hdfSelRow");
                                    ucAuditLogList.CurrentSelectedRowColor(Convert.ToInt32(hdfSelRow.Value));
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "RedirectToComparisonPageFromJoural", "RedirectToComparisonPageFromJoural();", true);
                                    //CallUserControl();
                                    //EntryStatus = EntryStatus.LISTMODE;

                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divAuditLog]','" + GetLocalResourceObject("EditLog").ToString() + "','605','300');", true);
                                }
                            }
                        }
                        break;
                        #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                string Error = CommonFunctions.ProcessException(ex);
                if (Error == "547")
                {
                    if (finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.CRJ)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "changePage('" + GetLocalResourceObject("PathSalesReceipt") + "','" + GetLocalResourceObject("Msg_Save_Err_Ref") + "','" + Resources.Messages.Information + "');", true);

                    }
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + GetLocalResourceObject("Msg_Save_Err_Ref").ToString() + "','" + Resources.Messages.Information + "');", true);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
                }
            }
            finally
            {
                commonService = null;
                poInvoiceServiceClient = null;
                finTrxServiceClient = null;
                CommonServiceClient = null;
            }
        }

        /// <summary>
        /// Method to clear data to avoid conflit between user control and transaction page
        /// </summary>
        private void ClearVoucher()
        {
            Session[ERP.Utilities.SessionStrings.TransactionType] = null;
        }

        /// <summary>
        /// Method to remove Cost Center details
        /// </summary>
        /// <param name="AccountControlId"></param>
        private void RemoveCostCenter(string AccountControlId)
        {
            if (!string.IsNullOrEmpty(AccountControlId) && CostCenterList != null && CostCenterList.Count > 0)
            {
                CostCenterTempList = CostCenterList;
                List<CostCenterDetails> objCCDtlsList = CostCenterTempList.Where(r => r.CONTROL_ID == AccountControlId).ToList();
                if (objCCDtlsList != null && objCCDtlsList.Count > 0)
                {
                    objCCDtlsList.ForEach(objDet =>
                    {
                        CostCenterTempList.Remove(objDet);
                    });
                    CostCenterList = CostCenterTempList;
                }
            }
        }

        /// <summary>
        /// Method to show voucher popup
        /// </summary>
        private void ShowVoucherPopup()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                                       "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
        }

        private bool IsEditableForApplication()
        {
            bool IsEditable = false;
            string AppType = Session[ERP.Utilities.SessionStrings.TransactionType].ToString();
            string[] EditableVouchers = GetGlobalResourceObject("ConfigurationsRes", "GLEditableVouchers").ToString().Split(',');
            //if (AppType == ApplicationType.FCHRJ)
            if (EditableVouchers != null && EditableVouchers.Count() > 0 && EditableVouchers.Contains(AppType))
            {
                IsEditable = true;
                hdfIsYearEnd.Value = "1";
            }
            return IsEditable;
        }
        #endregion
        #region Helper Methods
        #region Workflow Submit
        /// <summary>
        /// Save and submit With workflow 
        /// </summary>
        /// <param name="objSalPayment"></param>
        private void SaveTransaction(FinTrxHeaderBO objFinTrxHeaderBO, int workflowFlag, object sender, EventArgs e)
        {
            int retRfID = 0;
            long? jurResult = 0;
            string savePath = string.Empty;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string retAsrCode = string.Empty;
            string action = string.Empty;
            if (objFinTrxHeaderBO == null)
                objFinTrxHeaderBO = new FinTrxHeaderBO();
            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            objFinTrxHeaderBO.USER_PK = wkfDetails.UserPK;
            objFinTrxHeaderBO.WKF_APPLICATION = JurCurrPK;
            objFinTrxHeaderBO.WKF_COMMENTS = wkfDetails.Comments;
            objFinTrxHeaderBO.WKF_TRX_FLAG = workflowFlag;
            objFinTrxHeaderBO.WKF_PROCESS = wkfDetails.ProcessID;
            objFinTrxHeaderBO.WKF_REFERENCE = wkfDetails.ReferenceID;
            objFinTrxHeaderBO.WKF_TASK = wkfDetails.TaskID;
            objFinTrxHeaderBO.WKF_TASK_ACTION = wkfDetails.ActionID;
            //objSaleContract.WKF_MAIL_ATTACH = 0;
            action = wkfDetails.ActionText;
            #endregion
            // objFinTrxHeaderBO.FTH_VOUCHER_NO = txtVoucherNo.Text;
            string xmlDoc = CommonFunctions.XmlSerialize<FinTrxHeaderBO>(objFinTrxHeaderBO);//CommonFunctions.ObjectTOXml(saleOrderHeaderObj);
            // save Process Control inspection details
            jurResult = BusinessLogic.Jouralize.JournalizeBL.SaveJournalVoucher(xmlDoc, out retRfID, out TrxNo, out retAsrCode);
            if (jurResult > 0)// Save Success ! do WorkFlow
            {

                if (hdfIsSaveSubmit.Value != CommonConstants.SELECTVAL)//If not Delete and Submit
                {
                    Session[ERP.Utilities.SessionStrings.Transaction] = "SAVE";
                }
                else
                {
                    Session[ERP.Utilities.SessionStrings.Transaction] = "DELETE";

                }
                if (JournalType == (int)JournalTypeEnum.Voucher)
                {
                    ((Button)sender).CommandName = ActionsEnum.JOURNALIZESUBMIT.ToString();//Assign new Action
                    JournalizeSubmit(sender, e);//Call action in the popup calling page
                }
                else if (JournalType == (int)JournalTypeEnum.Reverse)
                {
                    ((Button)sender).CommandName = ActionsEnum.REVERSESUBMIT.ToString();//Assign new Action
                    ReverseSubmit(sender, e);//Call action in the popup calling page
                }
                else if (JournalType == (int)JournalTypeEnum.Return)
                {
                    ((Button)sender).CommandName = ActionsEnum.RETURNSUBMIT.ToString();//Assign new Action
                    ReturnSubmit(sender, e);//Call action in the popup calling page
                }
                ((Button)sender).CommandName = ActionsEnum.WRKFSUBMIT.ToString();//Re-Assign original Action
                TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                WrkfComments.Text = "";
                if (hdfIsSaveSubmit.Value != CommonConstants.SELECTVAL)//If not Delete and Submit
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                else
                    litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;
                object[] args = new object[2];
                args[0] = Resources.PageNameRes.Journalize;
                args[1] = TrxNo;
                litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                divGroupDrHdr.Controls.Clear();//Clear Debit Hdr section
                divGroupCrHdr.Controls.Clear();//Clear Credit Hdr section
                divGroupDr.Controls.Clear();//Clear Debit Controls
                divGroupCr.Controls.Clear();//Clear Credit Controls
                if (!string.IsNullOrEmpty(retAsrCode))
                {
                    litErrorMsg.Text += ". " + string.Format(GetLocalResourceObject("ChangeAsset").ToString(), retAsrCode);
                }
                if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && JournalizeRefPK > 0)
                {
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    // + "','" + Resources.ErpRes.Information + "');", true);
                    ClearVoucher();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                   + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                }
                else
                {
                    //Listing and detail pages are different,need to show msg and then redirect to listing page
                    string appType = string.Empty;
                    if (Session[ERP.Utilities.SessionStrings.TransactionType] != null)
                        appType = Session[ERP.Utilities.SessionStrings.TransactionType].ToString();
                    switch (appType)
                    {
                        case ApplicationType.DPVCJ:
                        case ApplicationType.DPBJ:
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                   + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.DirectPaymentList) + "');", true);
                            break;
                        default:
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "');", true);
                            break;
                    }
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    //    + "','" + Resources.ErpRes.Information + "');", true);
                    ClearVoucher();
                }
                ucrWrkf.ApplicationID = (int)jurResult;
            }
            else//Error
            {
                if (jurResult == (int)DbSaveStatus.SQLERROR)
                {
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                    //   "ClosePopup();ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                      "ClosePopup();ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (jurResult == (int)DbSaveStatus.CONCURRENCY)
                {
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                    //   "ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                       "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                    litErrorMsg.Text = Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (jurResult == (int)DbSaveStatus.CODEEXIST)
                {
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                    //   "ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                       "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                    litErrorMsg.Text = Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (jurResult == -111)//If the voucher Date year is selected is not same as the current financial year
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                        "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Financial_Year").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (jurResult == -114)//Chart of Account Inactive or Invalid
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                        "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_ChartofAccount").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (jurResult == (int)DbSaveStatus.FINYEARNOTEXIST)//If the voucher Date year is selected is not same as the current financial year
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                        "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Financial_Year_Notentered").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (jurResult == (int)DbSaveStatus.CHECKMCPRINTER)//If depreciation is calculated - CWIP - Asset
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                        "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Depreciation_Calculated").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (jurResult == (int)DbSaveStatus.CHECKZBPRINTER)//If asset exist
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                        "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                    litErrorMsg.Text = GetLocalResourceObject("Msg_AssetExist").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (jurResult == -222)//CWIP Asset exist
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                        "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                    litErrorMsg.Text = GetLocalResourceObject("Msg_CWIPAssetExist").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (jurResult == (int)DbSaveStatus.ALREADYREFERRED)//Alredy referred in another place
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1",
                    "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                    litErrorMsg.Text = GetLocalResourceObject("Err_Voucher_Cancel").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2",
                       "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
            }


        }
        #endregion

        /// <summary>
        /// Method for Call UserControl
        /// </summary>
        public long? UpdateBounce(int rch_pk)
        {
            long? jurResult;
            jurResult = 0;
            SalesReceiptService salesReceiptServiceClient;
            salesReceiptServiceClient = null;

            salesReceiptServiceClient = new SalesReceiptService();
            salesReceiptServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(salesReceiptServiceClient);
            jurResult = salesReceiptServiceClient.UpdateReceiptHdrBounceFlag(rch_pk, 1);
            salesReceiptServiceClient = null;
            return jurResult;
        }
        /// <summary>
        /// Method for Call UserControl
        /// </summary>
        public long? UpdateReceiptHdrPDC(int rch_pk, byte pdcFlag = 2)
        {
            long? jurResult;
            jurResult = 0;
            SalesReceiptService salesReceiptServiceClient;
            salesReceiptServiceClient = null;

            salesReceiptServiceClient = new SalesReceiptService();
            salesReceiptServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(salesReceiptServiceClient);
            jurResult = salesReceiptServiceClient.UpdateReceiptHdrPDCFlag(rch_pk, pdcFlag);
            salesReceiptServiceClient = null;
            return jurResult;
        }

        public long? UpdateDirectPaymentHdrPDC(int fth_pk, byte pdcFlag = 2)
        {
            long? jurResult;
            jurResult = 0;
            FinTrxService finTrxServiceClient;
            finTrxServiceClient = null;

            finTrxServiceClient = new FinTrxService();
            finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
            jurResult = finTrxServiceClient.UpdateDirectPaymentHdrPDC(fth_pk, pdcFlag);
            finTrxServiceClient = null;
            return jurResult;
        }


        public long? UpdatePdc(int rch_pk, byte pdcFlag = 2)
        {
            long? jurResult;
            jurResult = 0;
            POPaymentService poPaymentServiceClient;
            poPaymentServiceClient = null;

            poPaymentServiceClient = new POPaymentService();
            poPaymentServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(poPaymentServiceClient);
            jurResult = poPaymentServiceClient.UpdatePaymentHdrPDCFlag((int)rch_pk, pdcFlag);
            poPaymentServiceClient = null;
            return jurResult;
        }
        /// <summary>
        /// Method for Call UserControl
        /// </summary>
        public void CallUserControl()
        {
            try
            {
                if (Session[ERP.Utilities.SessionStrings.TransactionType] != null)//have Transaction Type
                {
                    if (Session[ERP.Utilities.SessionStrings.JournalMode] != null)
                        EntryStatus = (EntryStatus)(Enum.Parse(typeof(EntryStatus), Session[ERP.Utilities.SessionStrings.JournalMode].ToString()));//Get Journal Mode
                    JurCurrPK = 0;
                    GetFieldValues(ControlsEnum.COMPANY);//Get Company List
                    SetFiledValues(ControlsEnum.COMPANY);
                    if (dtCompany != null && dtCompany.Rows.Count > 0)
                    {
                        ddlVoucherCompany.SelectedIndex = ddlVoucherCompany.Items.IndexOf(ddlVoucherCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()));
                    }
                    if (EntryStatus != EntryStatus.NEWMODE)
                    {
                        GetFieldValues(ControlsEnum.FINHEADER);//Get Voucher Header
                    }

                    GetUIValuesFromObject(ControlsEnum.FINHEADER);//Set Voucher Header
                    if (AST_DOC_MODE.Value == ((int)DOCMODE.Manual).ToString())//User can Manually enter Voucher No
                        txtDispPVNo.Enabled = true;
                    else//User can't Manually enter Voucher No
                        txtDispPVNo.Enabled = false;
                    if (!HasWkfPermission && Convert.ToInt32(hdfVoucherStatus.Value) != 2)
                    {
                        btnJournalSave.Visible = false;
                        btnAddCredit.Visible = false;
                        btnAddDebit.Visible = false;
                    }
                    GetFieldValues(ControlsEnum.CALCULATIONMODE);//Get Calculation Mode

                    if (admAppConfigMstList != null && admAppConfigMstList.Count > 0)
                        GainLossCalculationMode = admAppConfigMstList[0].ACF_VALUE;//Set G/L Calculation Mode

                    GetFieldValues(ControlsEnum.BCENABLEDISABLE);//Get Settings for BC is Enable or not

                    if (admAppConfigMstList != null && admAppConfigMstList.Count > 0)
                        BCEnable = admAppConfigMstList[0].ACF_VALUE == 0 ? false : true;
                    //Load Controls to UI
                    LoadControls();
                    ucrWrkf = (WorkflowUserComments)this.Parent.FindControl("ucrWrkf");
                    ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                    CloseVoucherPopup = false;
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Method for Load the dynamic controls
        /// </summary>
        public void LoadControls()
        {
            try
            {
                divGroupDrHdr.Controls.Clear();//Clear Debit Hdr section
                divGroupDr.Controls.Clear();//Clear Debit Controls
                divGroupCrHdr.Controls.Clear();//Clear Credit Hdr section
                divGroupCr.Controls.Clear();//Clear Credit Controls
                BindControls();//Bind Controls to UI
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controlType"></param>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            int exchRateDecimalDigits = (Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
            try
            {
                switch (controlType)
                {
                    #region FINHEADER
                    case ControlsEnum.FINHEADER:
                        if (finTrxHdrList != null && finTrxHdrList.Count > 0)//Edit
                        {
                            Session[ERP.Utilities.SessionStrings.TransactionNo] = finTrxHdrList[0].FTH_REF_NO;
                            Session[ERP.Utilities.SessionStrings.TransactionDate] = finTrxHdrList[0].FTH_REF_DATE;
                            if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] == null)
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = finTrxHdrList[0].FTH_TRX_CURR;
                            else if (string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = finTrxHdrList[0].FTH_TRX_CURR;

                            if (finTrxHdrList[0].FTH_VOUCHER_NO == null || finTrxHdrList[0].FTH_VOUCHER_NO == string.Empty)//Doensn't have voucher no
                            {
                                txtDispPVNo.Text = GetLocalResourceObject("NewLabel").ToString();
                                hdfPVDate.Value = txtPVDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                                hdfVoucherNo.Value = string.Empty;
                            }
                            else//Have voucher no
                            {
                                txtDispPVNo.Text = HttpUtility.HtmlDecode(finTrxHdrList[0].FTH_VOUCHER_NO);
                                hdfVoucherNo.Value = HttpUtility.HtmlDecode(finTrxHdrList[0].FTH_VOUCHER_NO);
                            }
                            hdfVoucherStatus.Value = finTrxHdrList[0].FTH_STATUS.ToString();
                            txtJournalCurrency.Text = finTrxHdrList[0].ADM_CURRENCY_MST1.CUR_CODE;
                            litJournalCurrencyCr.Text = litJournalCurrencyDr.Text = string.Format(GetLocalResourceObject("AmountHdrTC").ToString(), txtJournalCurrency.Text);
                            hdfJournalCurr.Value = finTrxHdrList[0].FTH_TRX_CURR.ToString();
                            if (finTrxHdrList[0].FTH_COMPANY > 0)//Set Company ddl
                            {
                                ddlVoucherCompany.SelectedValue = finTrxHdrList[0].FTH_COMPANY.ToString();
                            }
                            else
                            {
                                GetFieldValues(ControlsEnum.DEPARTMENT);
                                if (admDeptMstList != null && admDeptMstList.Count > 0)
                                {
                                    ddlVoucherCompany.SelectedValue = admDeptMstList[0].ADM_COMPANY_MST.CMP_PK.ToString();
                                }
                            }
                            if (finTrxHdrList[0].FTH_TRX_CURR == currentUser.BaseCurrency)//If TC ==BC, then disable Exchange Rate, Hide BC section and hide G/L button
                            {
                                txtJournalExchangeRate.Enabled = false;
                                txtJournalExchangeRate.Attributes.Remove("class");
                                txtJournalExchangeRate.Attributes.Add("class", "input-w8-5per numeric input-disabled");
                                divTotalDr.Attributes.Remove("class");
                                divTotalDr.Attributes.Add("class", "divcolmiddle-journaltotal ");
                                divTotalCr.Attributes.Remove("class");
                                divTotalCr.Attributes.Add("class", "divcolmiddle-journaltotal ");
                                txtDrTotalBC.Visible = false;
                                txtCrTotalBC.Visible = false;
                                imbGainLoss.Visible = false;
                                IsBaseCurrency = true;
                            }
                            else
                                IsBaseCurrency = false;
                            //Currency can change only for JV
                            if (Session[ERP.Utilities.SessionStrings.TransactionPK] != null || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.PCS)
                            {
                                txtJournalCurrency.Enabled = false;
                                txtJournalCurrency.Attributes.Remove("class");
                                txtJournalCurrency.Attributes.Add("class", "input-disabled input-w70");
                            }
                            hdfExchangeRateJV.Value = hdfExchangeRateJV.Value == string.Empty ? ERP.Utilities.CommonFunctions.DoubleFormat(finTrxHdrList[0].FTH_EXCHG_RATE, exchRateDecimalDigits).ToString()
                                : hdfExchangeRateJV.Value;
                            txtJournalExchangeRate.Text = txtJournalExchangeRate.Text == string.Empty ? ERP.Utilities.CommonFunctions.DoubleFormat(finTrxHdrList[0].FTH_EXCHG_RATE, exchRateDecimalDigits).ToString()
                                : txtJournalExchangeRate.Text;
                            IsNewDummy = finTrxHdrList[0].FTH_DATE == null ? true : false;
                            //txtPVDate.Text = finTrxHdrList[0].FTH_DATE == null ?
                            //    DateTime.Now.ToString(Resources.Constants.DateFormatShort) :
                            //    ((DateTime)finTrxHdrList[0].FTH_DATE).ToString(Resources.Constants.DateFormatShort);

                            hdfPVDate.Value = txtPVDate.Text = finTrxHdrList[0].FTH_DATE == null ?
                                finTrxHdrList[0].FTH_REF_DATE.ToString(Resources.Constants.DateFormatShort) :
                                ((DateTime)finTrxHdrList[0].FTH_DATE).ToString(Resources.Constants.DateFormatShort);

                            #region Checking for voucher locked with financial year
                            txtPVDate.Enabled = true;
                            if (!string.IsNullOrEmpty(txtPVDate.Text) && !string.IsNullOrEmpty(txtDispPVNo.Text) && !txtDispPVNo.Text.Equals(GetLocalResourceObject("NewLabel").ToString()))
                            {
                                string LockUptoDate = string.Empty;
                                if (BusinessLogic.Finance.VoucherLockingBL.IsVoucherLocked(Convert.ToDateTime(txtPVDate.Text), JurCurrPK, finTrxHdrList[0].FTH_BIZUNIT, ref LockUptoDate))
                                {
                                    txtPVDate.Enabled = false;
                                }
                            }
                            #endregion

                            txtNarration.Text = HttpUtility.HtmlDecode(finTrxHdrList[0].FTH_NARRATION);
                            txtRemarks.Text = HttpUtility.HtmlDecode(finTrxHdrList[0].FTH_REMARKS);
                            JurCurrPK = (int)finTrxHdrList[0].FTH_PK;//Get Current PK
                            LastModifiedTime = finTrxHdrList[0].FTH_MOD_DT;
                            Session[ERP.Utilities.SessionStrings.JournalizePK] = JurCurrPK.ToString();
                            txtRefNo.Text = HttpUtility.HtmlDecode(finTrxHdrList[0].FTH_REF_NO);
                            txtRefDate.Text = finTrxHdrList[0].FTH_REF_DATE.ToString(Resources.Constants.DateFormatShort);
                            if (finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.PIJ || finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.PSIJ || finTrxHdrList[0].FTH_REF_TYPE == ApplicationType.EIJ)
                            {
                                divTaxDate.Visible = true;
                                if (finTrxHdrList[0].FTH_TAX_DATE.HasValue)
                                {
                                    txtTaxDate.Text = Convert.ToDateTime(finTrxHdrList[0].FTH_TAX_DATE).ToString(Resources.Constants.DateFormatShort);
                                }
                                else
                                {
                                    txtTaxDate.Text = txtRefDate.Text;
                                }
                            }
                            else
                            {
                                divTaxDate.Visible = false;
                            }

                            //Enable refno and refdate for vouchers except jv and pcs
                            if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() != ApplicationType.JV && Session[ERP.Utilities.SessionStrings.TransactionType].ToString() != ApplicationType.PCS)
                            {
                                txtRefNo.Enabled = false;
                                txtRefDate.Enabled = false;
                            }
                            if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() != "DPVJ")
                            {
                                GetFieldValues(ControlsEnum.AUDITLOGSTATUS);
                                if (dtStatus != null && dtStatus.Rows.Count > 0)
                                    lnkAuditLog.Visible = dtStatus.Rows[0][0].ToString() == "1" ? true : false;
                                if (lnkAuditLog.Visible)
                                {
                                    GetFieldValues(ControlsEnum.AUDITLOGDETAILS);
                                    ucAuditLogList.FillDetails(dtAuditDetails);
                                }
                            }
                        }
                        else//New
                        {
                            txtDispPVNo.Text = GetLocalResourceObject("NewLabel").ToString();
                            hdfVoucherNo.Value = string.Empty;
                            hdfPVDate.Value = txtPVDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                            txtRefNo.Text = string.Empty;
                            txtRefDate.Text = string.Empty;
                            txtTaxDate.Text = string.Empty;
                            if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() != ApplicationType.JV && Session[ERP.Utilities.SessionStrings.TransactionType].ToString() != ApplicationType.PCS)
                            {
                                txtRefNo.Enabled = false;
                                txtRefDate.Enabled = false;
                            }
                            hdfJournalCurr.Value = currentUser.BaseCurrency.ToString();
                            GetFieldValues(ControlsEnum.BASECURRENCY);
                            txtJournalCurrency.Text = hdfJournalBaseCurrency.Value;
                            //Currency can change only for JV
                            if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.PCS)
                            {
                                txtJournalCurrency.Enabled = false;
                                txtJournalCurrency.Attributes.Remove("class");
                                txtJournalCurrency.Attributes.Add("class", "input-disabled input-w70");
                            }
                            litJournalCurrencyCr.Text = litJournalCurrencyDr.Text = string.Format(GetLocalResourceObject("AmountHdrTC").ToString(), txtJournalCurrency.Text);
                            GetFieldValues(ControlsEnum.DEPARTMENT);
                            //if (admDeptMstList != null && admDeptMstList.Count > 0)
                            //{
                            //    ddlVoucherCompany.SelectedValue = admDeptMstList[0].ADM_COMPANY_MST.CMP_PK.ToString();
                            //}
                            GetFieldValues(ControlsEnum.COMPANY);
                            if (dtCompany != null && dtCompany.Rows.Count > 0)
                            {
                                ddlVoucherCompany.SelectedIndex = ddlVoucherCompany.Items.IndexOf(ddlVoucherCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()));
                            }
                            if (txtJournalExchangeRate.Text == string.Empty)
                            {
                                finTrxHdrObj = null;
                                GetFieldValues(ControlsEnum.EXCHANGERATE);
                                txtJournalExchangeRate.Text = hdfExchangeRateJV.Value;
                                txtJournalExchangeRate.Text = ERP.Utilities.CommonFunctions.DoubleFormat(double.Parse(txtJournalExchangeRate.Text), exchRateDecimalDigits).ToString();
                            }
                            divTotalDr.Attributes.Remove("class");
                            divTotalDr.Attributes.Add("class", "divcolmiddle-journaltotal ");
                            divTotalCr.Attributes.Remove("class");
                            divTotalCr.Attributes.Add("class", "divcolmiddle-journaltotal ");
                            txtDrTotalBC.Visible = false;
                            txtCrTotalBC.Visible = false;
                            imbGainLoss.Visible = false;
                            IsBaseCurrency = true;
                        }
                        AST_DOC_MODE.Value = GetDOCMODE();//Get document mode for voucher no generation
                        break;
                    #endregion
                    #region ALL COST CENTERS
                    case ControlsEnum.ALLCOSTCENTERS:
                        if (dtCostCenter != null && dtCostCenter.Rows.Count > 0)
                        {
                            CostCenterTempList = dtCostCenter.AsEnumerable().Select(r =>
                               new CostCenterDetails
                               {
                                   FTD_AMT_BC = r.Field<decimal>("FTD_AMT_BC"),
                                   FCM_COST_CENTER_TEXT = r.Field<string>("FCM_COST_CENTER_TEXT"),
                                   FCM_CNM_PK = r.Field<int>("FCM_CNM_PK"),
                                   FTR_PK = r.Field<long>("FTR_PK")

                               }).ToList();
                            CostCenterList = CostCenterTempList;
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
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private object SetUIValuesToObject(ActionsEnum mode)
        {
            object returnObj;
            returnObj = null;
            CommonService commonServiceClient;
            commonServiceClient = null;
            Dictionary<string, string> dicDrControls;
            dicDrControls = null;
            int drCount;
            short count;
            short drSequence = 1000;
            short crSequence = 2000;
            List<string> removedControlsList;
            Dictionary<string, string> dicCrControls;
            Dictionary<string, long> dicFinTrxPK = null;
            dicCrControls = null;
            int crCount;
            DateTime? TaxDate = null;
            List<CostCenterDetails> objCostCenterList;
            try
            {
                switch (mode)
                {
                    case ActionsEnum.CHANGEDATE:
                        finTrxHdrObj = CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_PK = JurCurrPK;
                        finTrxHdrObj.FTH_TAX_DATE = DateTime.Parse(txtTaxDate.Text.Trim());
                        finTrxHdrObj.FTH_MOD_DT = this.LastModifiedTime;
                        finTrxHdrObj.FTH_MOD_BY = this.currentUser.PKUser;
                        returnObj = finTrxHdrObj;
                        break;
                    #region Save
                    case ActionsEnum.SAVE:
                        #region Header
                        commonServiceClient = new CommonService();
                        commonServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(commonServiceClient);
                        admConfigMstObj = CommonFunctions.Initilize<ADM_CONFIG_MST>();
                        finTrxHdrObj = CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_PK = JurCurrPK;

                        //if (hdfVoucherNo.Value.Equals(string.Empty) && AST_DOC_MODE.Value == ((int)DOCMODE.Draft).ToString())//If configuration is for voucher no generate in draft mode
                        //{
                        //    updateVocher = true;
                        //    GetFieldValues(ControlsEnum.VOUCHERNO);
                        //}
                        //else if (AST_DOC_MODE.Value == ((int)DOCMODE.Manual).ToString())
                        //{
                        //    hdfVoucherNo.Value = txtDispPVNo.Text.Trim();
                        //}
                        //finTrxHdrObj.FTH_VOUCHER_NO = HttpUtility.HtmlEncode(hdfVoucherNo.Value);
                        finTrxHdrObj.FTH_DATE = DateTime.Parse(txtPVDate.Text);
                        finTrxHdrObj.FTH_TRX_DATE = DateTime.Parse(txtPVDate.Text);
                        finTrxHdrObj.FTH_REF_TYPE = Session[ERP.Utilities.SessionStrings.TransactionType].ToString();
                        finTrxHdrObj.FTH_REF_PK = Session[ERP.Utilities.SessionStrings.TransactionPK] == null ? JurCurrPK :
                            Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString());
                        if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.JV || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.PCS)
                        {
                            finTrxHdrObj.FTH_REF_NO = HttpUtility.HtmlEncode(txtRefNo.Text.Trim());
                            finTrxHdrObj.FTH_REF_DATE = string.IsNullOrEmpty(txtRefDate.Text.Trim()) ? DateTime.Parse(txtPVDate.Text) :
                                 DateTime.Parse(txtRefDate.Text.Trim());
                        }
                        else
                        {
                            finTrxHdrObj.FTH_REF_NO = Session[ERP.Utilities.SessionStrings.TransactionNo] == null ? HttpUtility.HtmlEncode(txtRefNo.Text.Trim()) :
                                Session[ERP.Utilities.SessionStrings.TransactionNo].ToString();
                            finTrxHdrObj.FTH_REF_DATE = Session[ERP.Utilities.SessionStrings.TransactionDate] == null ? DateTime.Parse(txtPVDate.Text) :
                                DateTime.Parse(Session[ERP.Utilities.SessionStrings.TransactionDate].ToString());
                        }
                        if (!string.IsNullOrEmpty(txtTaxDate.Text.Trim()) && (finTrxHdrObj.FTH_REF_TYPE == ApplicationType.PIJ || finTrxHdrObj.FTH_REF_TYPE == ApplicationType.PSIJ || finTrxHdrObj.FTH_REF_TYPE == ApplicationType.EIJ))
                        {
                            TaxDate = DateTime.Parse(txtTaxDate.Text.Trim());
                        }
                        finTrxHdrObj.FTH_TAX_DATE = TaxDate;
                        finTrxHdrObj.FTH_NARRATION = HttpUtility.HtmlEncode(txtNarration.Text);
                        finTrxHdrObj.FTH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                        finTrxHdrObj.FTH_TRX_CURR = Session[ERP.Utilities.SessionStrings.TransactionCurrency] == null ? int.Parse(hdfJournalCurr.Value) :
                            int.Parse(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                        finTrxHdrObj.FTH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                        finTrxHdrObj.FTH_COMPANY = Convert.ToInt32(ddlVoucherCompany.SelectedValue);
                        finTrxHdrObj.FTH_FIN_YEAR = 0;
                        finTrxHdrObj.FTH_STATUS = Convert.ToByte(hdfVoucherStatus.Value);
                        finTrxHdrObj.FTH_ACTIVE = 1;
                        finTrxHdrObj.FTH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                        finTrxHdrObj.FTH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                        finTrxHdrObj.FTH_MOD_DT = LastModifiedTime;
                        finTrxHdrObj.FTH_DEPT = ExternalUserDept > 0 ? ExternalUserDept : currentUser.CurrentDeptPK;
                        finTrxHdrObj.FTH_BIZUNIT = currentUser.SBUID;
                        if (finTrxHdrObj.FTH_STATUS > 0)
                            finTrxHdrObj.FTH_IS_JRNLD = true;
                        else
                            finTrxHdrObj.FTH_IS_JRNLD = false;
                        if (hdfExchangeRateJV.Value == "")
                        {
                            GetFieldValues(ControlsEnum.EXCHANGERATE);
                        }
                        finTrxHdrObj.FTH_EXCHG_RATE = double.Parse(hdfExchangeRateJV.Value);
                        #endregion
                        #region Debit
                        finTrxObj = CommonFunctions.Initilize<FIN_TRX>();
                        dicDrControls = new Dictionary<string, string>();
                        if (Session[ERP.Utilities.SessionStrings.DrControls] != null)
                            dicDrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.DrControls];//Get Debit Controls
                        var drControlsSave = from pair in dicDrControls
                                             orderby pair.Key.Substring(0, 13) ascending//orderby pair.Key.Substring(0, 9) ascending
                                             select pair;//Order debit controls in ascending order
                        drCount = 1;
                        count = 1;
                        removedControlsList = null;

                        if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)
                        {
                            removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];//Get Removed Control List
                        }
                        if (removedControlsList != null && removedControlsList.Count > 0)
                        {
                            //Get the first valid drCount value.
                            while (!string.IsNullOrEmpty(removedControlsList.FirstOrDefault(aa => aa.StartsWith("dic" + drCount.ToString("0000")) && aa.EndsWith("dr"))))
                            {
                                drCount++;
                            }
                        }

                        foreach (KeyValuePair<string, string> pair in drControlsSave)
                        {
                            if (!pair.Key.Substring(7, 6).Equals("03set1") || !pair.Key.Substring(7, 6).Equals("04set1"))//Not Account aoutocomplete Hiddenfield or Account aoutocomplete Button
                            {
                                if (!drCount.ToString("0000").Equals(pair.Key.Substring(3, 4)))
                                {
                                    drCount++;
                                    if (removedControlsList != null && removedControlsList.Count > 0)//Find next drcount
                                    {
                                        while (!string.IsNullOrEmpty(removedControlsList.FirstOrDefault(aa => aa.StartsWith("dic" + drCount.ToString("0000")) && aa.EndsWith("dr"))))
                                        {
                                            drCount++;
                                        }
                                    }
                                    //finTrxObj.FTR_SEQUENCE = count;
                                    finTrxObj.FTR_SEQUENCE = drSequence++;
                                    finTrxObj.FTR_TRX_HDR = JurCurrPK;
                                    finTrxObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                                    finTrxObj.FTR_MOD_DT = DateTime.Now;
                                    finTrxObj.FTR_REMARKS = "";
                                    finTrxObj.FTR_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                    finTrxObj.FTR_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                                    finTrxObj.FTR_CRTD_DT = DateTime.Now;
                                    finTrxObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                                    finTrxObj.FTR_MOD_DT = LastModifiedTime;
                                    finTrxObj.FTR_DEPT = currentUser.CurrentDeptPK;
                                    finTrxObj.FTR_BIZUNIT = currentUser.SBUID;
                                    if (finTrxObj.FTR_DR_AMT_TC > -1 && finTrxObj.FTR_CR_AMT_TC > -1)
                                    {
                                        finTrxList.Add(finTrxObj);
                                    }
                                    finTrxObj = CommonFunctions.Initilize<FIN_TRX>();
                                    count++;
                                }
                            }
                            switch ((ControlCategory)(Enum.Parse(typeof(ControlCategory), pair.Value)))//Get Data from each Debit control
                            {
                                #region SubType
                                case ControlCategory.SubType:

                                    break;
                                #endregion
                                #region Account
                                case ControlCategory.Account:
                                    if (pair.Key.Substring(7, 6).Equals("02set1"))//Acount Textbox
                                    {
                                        hdfSubTypePk.Value = string.Empty;
                                        TextBox txtAccount = (TextBox)divGroupDr.FindControl(pair.Key);//Get Account Textbox
                                        string hdfCtrlId = pair.Key.Replace("02set102", "03set103");
                                        HiddenField hdfAccount = (HiddenField)divGroupDr.FindControl(hdfCtrlId);//Get Account Hiddenfield
                                        if (txtAccount != null && hdfAccount != null && !string.IsNullOrEmpty(hdfAccount.Value))
                                        {
                                            hdfCoaPk.Value = hdfAccount.Value;
                                            GetFieldValues(ControlsEnum.FINCOAMST);
                                            if (finCoaMstList != null && finCoaMstList.Count > 0)
                                            {
                                                hdfSubTypePk.Value = finCoaMstList[0].COA_SUB_TYPE.ToString();
                                            }
                                            Label txtSubType = (Label)pnlControls.FindControl(txtAccount.ID.Replace("02set102", "01set101"));//Get Account Label
                                            //if (txtSubType != null && (txtSubType.Text == GetLocalResourceObject("New_Credit_Account").ToString() || txtSubType.Text == GetLocalResourceObject("New_Debit_Account").ToString()
                                            //    || txtSubType.Text == GetLocalResourceObject("Credit_Account").ToString() || txtSubType.Text == GetLocalResourceObject("Debit_Account").ToString()))
                                            //{
                                            //    finTrxObj.FTR_ACC_SUB_TYPE = 0;
                                            //}
                                            //else
                                            //{
                                            finTrxObj.FTR_ACC_SUB_TYPE = Convert.ToInt32(hdfSubTypePk.Value);
                                            //}

                                            GetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                                            if (finCoaSubTypeCfgList[0].CST_REL_QUERY != null)//If have related qry
                                            {
                                                finTrxObj.FTR_TYPE = finCoaSubTypeCfgList[0].CST_CODE;
                                                DropDownList ddlSubLedger = (DropDownList)pnlControls.FindControl(txtAccount.ID.Replace("02set102", "51set202"));//Get Subledger DDL
                                                if (ddlSubLedger != null && !string.IsNullOrEmpty(ddlSubLedger.SelectedValue) && ddlSubLedger.SelectedValue != "-1")//Have subledger value
                                                {
                                                    finTrxObj.FTR_TYPE_PK = Convert.ToInt32(ddlSubLedger.SelectedValue);
                                                }
                                                TextBox txtInsNo = (TextBox)pnlControls.FindControl(ddlSubLedger.ID.Replace("51set202", "52set203"));//Get Instrument TextBox
                                                if (txtInsNo != null && !txtInsNo.Text.Trim().Equals(GetLocalResourceObject("InstrumentNo").ToString()))
                                                    finTrxObj.FTR_INSTR_NO = HttpUtility.HtmlEncode(txtInsNo.Text);
                                                TextBox txtdat = (TextBox)pnlControls.FindControl(ddlSubLedger.ID.Replace("51set202", "53set204"));//Get Date TextBox
                                                if (txtdat != null && !txtdat.Text.Trim().Equals(GetLocalResourceObject("Date").ToString()))
                                                    finTrxObj.FTR_INSTR_DATE = string.IsNullOrEmpty(txtdat.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtdat.Text.Trim());
                                                TextBox favourOf = (TextBox)pnlControls.FindControl(ddlSubLedger.ID.Replace("51set202", "54set205"));//Get favourOf TextBox
                                                if (favourOf != null && !favourOf.Text.Trim().Equals(GetLocalResourceObject("FavourOf").ToString()))
                                                    finTrxObj.FTR_INSTR_FAVOUR = HttpUtility.HtmlEncode(favourOf.Text);
                                            }
                                            else
                                            {
                                                finTrxObj.FTR_TYPE = null;
                                                finTrxObj.FTR_TYPE_PK = null;
                                            }
                                            finTrxObj.FTR_ACCOUNT = Convert.ToInt16(hdfAccount.Value);

                                            #region Add Cost Center Details (Debit)
                                            string hdfCostCenterCtrlId = pair.Key.Replace("02set102", "11set111");
                                            HiddenField hdfCostCenterCtr = (HiddenField)divGroupDr.FindControl(hdfCostCenterCtrlId);//Get Cost center Hiddenfield
                                            if (hdfCostCenterCtr != null && hdfCostCenterCtr.Value == "1")
                                            {
                                                if (GetGlobalResourceObject("ConfigurationsRes", "CCPercentageRequired").ToString() == "1")
                                                {
                                                    #region If the transaction amount was changed, reallocate cost center allocation(Debit)

                                                    //string txtTransAmountCtrlId = pair.Key.Replace("02set102", "06set106");
                                                    //For New change based Base Curreny for  amount -- for MMT for cost center allocation check--Nidhin
                                                    string txtTransAmountCtrlId = string.Empty;
                                                    int TrxCurrency = Session[ERP.Utilities.SessionStrings.TransactionCurrency] == null ? int.Parse(hdfJournalCurr.Value) :
                                                                        int.Parse(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                                                    if (TrxCurrency == currentUser.BaseCurrency)
                                                        txtTransAmountCtrlId = pair.Key.Replace("02set102", "06set106");
                                                    else
                                                        txtTransAmountCtrlId = pair.Key.Replace("02set102", "08set108");
                                                    TextBox txtTransAmt = (TextBox)divGroupDr.FindControl(txtTransAmountCtrlId);//Get Transaction Amount text box
                                                    CCAmount = 0; AccountPk = 0;
                                                    if (txtTransAmt != null) decimal.TryParse(txtTransAmt.Text, out CCAmount);
                                                    if (hdfAccount != null) int.TryParse(hdfAccount.Value, out AccountPk);
                                                    CostCenterTempList = CostCenterList.DeepClone();
                                                    List<CostCenterDetails> objCostCentrDtls = CostCenterList.Where(r => r.CONTROL_ID == hdfCtrlId).ToList();
                                                    if (objCostCentrDtls != null && objCostCentrDtls.Count > 0 && (ERP.Utilities.CommonFunctions.DecimalFormat(objCostCentrDtls.Sum(r => r.FTD_AMT_BC), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) != CCAmount))
                                                    {
                                                        #region reallocating cost center allocation
                                                        if (objCostCentrDtls != null)
                                                        {
                                                            CostCenterTempList.RemoveAll(f => f.CONTROL_ID == hdfCtrlId);
                                                        }
                                                        GetFieldValues(ControlsEnum.COSTCENTER);
                                                        if (dtCostCenter != null && dtCostCenter.Rows.Count > 0)
                                                        {
                                                            foreach (DataRow drRow in dtCostCenter.Rows)
                                                            {
                                                                CostCenterDetails objCCDtls = new CostCenterDetails();
                                                                objCCDtls.FCM_CNM_PK = Convert.ToInt32(drRow["FCM_CNM_PK"]);
                                                                objCCDtls.FCM_COST_CENTER_TEXT = CommonFunctions.GetEncodedString(drRow["FCM_COST_CENTER_TEXT"]);
                                                                objCCDtls.FTD_AMT_BC = Convert.ToDecimal(drRow["FCM_AMOUNT"]);
                                                                objCCDtls.CONTROL_ID = ControlId;
                                                                CostCenterTempList.Add(objCCDtls);
                                                            }
                                                        }
                                                        #endregion
                                                        CostCenterList = CostCenterTempList.DeepClone();
                                                    }

                                                    #endregion
                                                }

                                                List<FIN_TRX_COC_DTL> objCocDtlList = new List<FIN_TRX_COC_DTL>();
                                                objCostCenterList = new List<CostCenterDetails>();
                                                objCostCenterList = CostCenterList.Where(r => r.CONTROL_ID == hdfCtrlId && r.FTD_AMT_BC > 0).ToList();
                                                if (objCostCenterList != null && objCostCenterList.Count > 0)
                                                {
                                                    foreach (CostCenterDetails objDet in objCostCenterList)
                                                    {
                                                        FIN_TRX_COC_DTL objCocDtl = new FIN_TRX_COC_DTL();
                                                        objCocDtl.FTD_CNM_PK = objDet.FCM_CNM_PK;
                                                        objCocDtl.FTD_AMT_BC = objDet.FTD_AMT_BC;
                                                        objCocDtlList.Add(objCocDtl);
                                                    }
                                                }

                                                bool CostSplitValidation = true;
                                                //string txtAmountTcCtrlId = pair.Key.Replace("02set102", "06set106");
                                                //string hdfAccountContrlId = pair.Key.Replace("02set102", "03set103");
                                                //For New change based Base Curreny for  amount -- for MMT for cost center allocation check--Nidhin
                                                string txtAmountTcCtrlId = string.Empty;
                                                int TrsnCurrency = Session[ERP.Utilities.SessionStrings.TransactionCurrency] == null ? int.Parse(hdfJournalCurr.Value) :
                                                                    int.Parse(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                                                if (TrsnCurrency == currentUser.BaseCurrency)
                                                    txtAmountTcCtrlId = pair.Key.Replace("02set102", "06set106");
                                                else
                                                    txtAmountTcCtrlId = pair.Key.Replace("02set102", "08set108");
                                                TextBox txtAmountTc = (TextBox)divGroupDr.FindControl(txtAmountTcCtrlId);//Get Transaction Amount text box
                                                if (txtAmountTc != null)
                                                {
                                                    txtAmountTc.CssClass = txtAmountTc.CssClass.Replace("border-red", "tempcss");// tempcss"input-w8per numeric";

                                                    CostCenterAmnt = 0;
                                                    decimal.TryParse(txtAmountTc.Text, out CostCenterAmnt);
                                                    if (objCocDtlList != null && objCocDtlList.Count > 0 && objCocDtlList.Sum(r => r.FTD_AMT_BC) > 0
                                                        && ERP.Utilities.CommonFunctions.DecimalFormat(objCocDtlList.Sum(r => r.FTD_AMT_BC), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) != CostCenterAmnt)
                                                    {
                                                        CostSplitValidation = false;
                                                    }
                                                    else if (objCocDtlList != null && objCocDtlList.Count > 0
                                                        && ERP.Utilities.CommonFunctions.DecimalFormat(objCocDtlList.Sum(r => r.FTD_AMT_BC), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) == CostCenterAmnt)
                                                    {
                                                        CostSplitValidation = true;
                                                    }
                                                    else
                                                    {
                                                        AccountPk = finTrxObj.FTR_ACCOUNT.Value;
                                                        CCAmount = CostCenterAmnt;
                                                        GetFieldValues(ControlsEnum.COSTCENTER);
                                                        if (dtCostCenter != null && dtCostCenter.Rows.Count > 0)
                                                        {
                                                            CostCenterTempList = CostCenterList;
                                                            objCocDtlList = new List<FIN_TRX_COC_DTL>();
                                                            objCostCenterList = CostCenterTempList.Where(r => r.CONTROL_ID == hdfCtrlId).ToList();
                                                            objCostCenterList.ForEach(dtl =>
                                                            {
                                                                CostCenterTempList.Remove(dtl);
                                                            });

                                                            foreach (DataRow drRow in dtCostCenter.Rows)
                                                            {
                                                                CostCenterDetails objCCDtls = new CostCenterDetails();
                                                                objCCDtls.FCM_CNM_PK = Convert.ToInt32(drRow["FCM_CNM_PK"]);
                                                                objCCDtls.FCM_COST_CENTER_TEXT = CommonFunctions.GetEncodedString(drRow["FCM_COST_CENTER_TEXT"]);
                                                                objCCDtls.FTD_AMT_BC = ERP.Utilities.CommonFunctions.DecimalFormat(Convert.ToDecimal(drRow["FCM_AMOUNT"]), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                                                objCCDtls.CONTROL_ID = hdfCtrlId;
                                                                CostCenterTempList.Add(objCCDtls);

                                                                FIN_TRX_COC_DTL objCocDtl = new FIN_TRX_COC_DTL();
                                                                objCocDtl.FTD_CNM_PK = Convert.ToInt32(drRow["FCM_CNM_PK"]);
                                                                objCocDtl.FTD_AMT_BC = ERP.Utilities.CommonFunctions.DecimalFormat(Convert.ToDecimal(drRow["FCM_AMOUNT"]), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                                                objCocDtlList.Add(objCocDtl);
                                                            }
                                                            CostCenterList = CostCenterTempList;
                                                            if (ERP.Utilities.CommonFunctions.DecimalFormat(CostCenterTempList.Where(r => r.CONTROL_ID == hdfCtrlId).Sum(r => r.FTD_AMT_BC), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) != CostCenterAmnt)
                                                                CostSplitValidation = false;
                                                        }
                                                        else
                                                            CostSplitValidation = false;
                                                    }
                                                }
                                                else
                                                    CostSplitValidation = false;
                                                if (!CostSplitValidation)
                                                {
                                                    CCInvalidSplitControlTempList = CCInvalidSplitControlList;
                                                    CCInvalidSplitControlTempList.Add(new CostCenterMissmatch { AMT_TC_CONTROL_ID = txtAmountTcCtrlId });
                                                    CCInvalidSplitControlList = CCInvalidSplitControlTempList;
                                                }

                                                objCocDtlList.ForEach(dtl =>
                                                {
                                                    if (dtl.FTD_AMT_BC > 0)
                                                        finTrxObj.FIN_TRX_COC_DTL.Add(dtl);
                                                });

                                            }
                                            #endregion
                                        }
                                    }
                                    break;
                                #endregion
                                #region Narration
                                case ControlCategory.Narration:
                                    TextBox txtNarr = (TextBox)divGroupDr.FindControl(pair.Key);//Get Narration Textbox
                                    if (txtNarr != null && !string.IsNullOrEmpty(txtNarr.Text.Trim()))
                                        finTrxObj.FTR_NARRATION = HttpUtility.HtmlEncode(txtNarr.Text.Trim());
                                    break;
                                #endregion
                                #region AmountTC
                                case ControlCategory.AmountTC:
                                    TextBox txtAmountTCDr = (TextBox)divGroupDr.FindControl(pair.Key);//Get Amount in TC Textbox Textbox
                                    if (txtAmountTCDr != null && !string.IsNullOrEmpty(txtAmountTCDr.Text.Trim())
                                        && Convert.ToDecimal(txtAmountTCDr.Text.Trim()) > 0)
                                    {
                                        txtAmountTCDr.Text = string.IsNullOrEmpty(txtAmountTCDr.Text.Trim()) ? "0" : txtAmountTCDr.Text.Trim();
                                        finTrxObj.FTR_DR_AMT_TC = Convert.ToDecimal(txtAmountTCDr.Text);
                                        finTrxObj.FTR_CR_AMT_TC = 0;
                                        finTrxObj.FTR_PK = 0;
                                    }
                                    else
                                    {
                                        finTrxObj.FTR_DR_AMT_TC = 0;
                                        finTrxObj.FTR_CR_AMT_TC = 0;
                                        finTrxObj.FTR_PK = 0;
                                    }

                                    break;
                                #endregion
                                #region ExchangeRate
                                case ControlCategory.ExchangeRate:
                                    double exchangeRate = 1;
                                    TextBox txtExchangeRate = (TextBox)divGroupDr.FindControl(pair.Key);//Get Exchange Rate Textbox
                                    if (txtExchangeRate != null && !string.IsNullOrEmpty(txtExchangeRate.Text.Trim())
                                        && Convert.ToDecimal(txtExchangeRate.Text.Trim()) > 0)
                                    {
                                        double.TryParse(txtExchangeRate.Text.Trim(), out exchangeRate);
                                        finTrxObj.FTR_EXCHG_RATE = exchangeRate;
                                    }
                                    else
                                    {
                                        finTrxObj.FTR_EXCHG_RATE = exchangeRate;
                                    }
                                    break;
                                #endregion
                                #region EntryMode
                                case ControlCategory.EntryMode:
                                    byte entryMode = 0;
                                    HiddenField hdfEntryMode = (HiddenField)divGroupDr.FindControl(pair.Key);//Get EntryMode Hiddenfield
                                    if (hdfEntryMode != null && !string.IsNullOrEmpty(hdfEntryMode.Value))
                                    {
                                        byte.TryParse(hdfEntryMode.Value, out entryMode);
                                        finTrxObj.FTR_ENTRY_MODE = entryMode;
                                    }
                                    else
                                    {
                                        finTrxObj.FTR_ENTRY_MODE = entryMode;
                                    }
                                    break;
                                #endregion
                                #region AmountBC
                                case ControlCategory.AmountBC:
                                    if (finTrxObj.FTR_ENTRY_MODE != (byte)VoucherEntryMode.GainOrLoss)
                                    {
                                        TextBox txtAmountBCDr = (TextBox)divGroupDr.FindControl(pair.Key);//Get Amount in BC Textbox
                                        if (txtAmountBCDr != null && !string.IsNullOrEmpty(txtAmountBCDr.Text.Trim()))
                                        {
                                            finTrxObj.FTR_DR_AMT_BC = Convert.ToDecimal(txtAmountBCDr.Text);
                                            finTrxObj.FTR_CR_AMT_BC = 0;
                                        }
                                        else
                                        {
                                            string tcId = pair.Key.Replace(pair.Key.Substring(7, 8), "06set106");//Get Amount in TC Textbox
                                            TextBox txtTempAmountTCDr = (TextBox)pnlControls.FindControl(tcId);
                                            if (txtTempAmountTCDr != null)
                                            {
                                                finTrxObj.FTR_DR_AMT_BC = Convert.ToDecimal(txtTempAmountTCDr.Text);
                                                finTrxObj.FTR_CR_AMT_BC = 0;
                                            }
                                        }
                                    }
                                    else//Entry mode is G/L
                                    {
                                        TextBox txtAmountBCDr = (TextBox)divGroupDr.FindControl(pair.Key);//Get Amount in BC Textbox
                                        if (txtAmountBCDr != null && !string.IsNullOrEmpty(txtAmountBCDr.Text.Trim())
                                            && Convert.ToDecimal(txtAmountBCDr.Text.Trim()) > 0)
                                        {
                                            txtAmountBCDr.Text = string.IsNullOrEmpty(txtAmountBCDr.Text.Trim()) ? "0" : txtAmountBCDr.Text.Trim();
                                            finTrxObj.FTR_DR_AMT_BC = Convert.ToDecimal(txtAmountBCDr.Text);
                                            finTrxObj.FTR_CR_AMT_BC = 0;
                                            finTrxObj.FTR_DR_AMT_TC = 0;
                                            finTrxObj.FTR_CR_AMT_TC = 0;
                                            finTrxObj.FTR_EXCHG_RATE = 0;
                                            finTrxObj.FTR_PK = 0;
                                        }
                                    }
                                    break;
                                #endregion
                                #region Delete
                                case ControlCategory.Delete:
                                    break;
                                #endregion
                                #region CostCenter
                                case ControlCategory.CostCenter:
                                    break;
                                #endregion
                                #region SubLedgerLabel
                                case ControlCategory.SubledgerLabel:
                                    break;
                                #endregion
                                #region SubLedger
                                case ControlCategory.SubLedger:
                                    break;
                                #endregion
                                #region InstrumentNo
                                case ControlCategory.InstrumentNo:
                                    break;
                                #endregion
                                #region Date
                                case ControlCategory.Date:
                                    break;
                                #endregion
                                #region FavourOf
                                case ControlCategory.FavourOf:
                                    break;
                                    #endregion


                            }
                        }
                        if (drControlsSave.Count() > 0)
                        {
                            //finTrxObj.FTR_SEQUENCE = count;
                            finTrxObj.FTR_SEQUENCE = drSequence++;
                            finTrxObj.FTR_TRX_HDR = JurCurrPK;
                            finTrxObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                            finTrxObj.FTR_MOD_DT = DateTime.Now;
                            finTrxObj.FTR_REMARKS = string.Empty;
                            finTrxObj.FTR_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            finTrxObj.FTR_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                            finTrxObj.FTR_CRTD_DT = DateTime.Now;
                            finTrxObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                            finTrxObj.FTR_MOD_DT = LastModifiedTime;
                            finTrxObj.FTR_DEPT = currentUser.CurrentDeptPK;
                            finTrxObj.FTR_BIZUNIT = currentUser.SBUID;
                            if (finTrxObj.FTR_DR_AMT_TC > -1 && finTrxObj.FTR_CR_AMT_TC > -1)
                            {
                                finTrxList.Add(finTrxObj);
                            }
                        }

                        #endregion
                        #region credit
                        finTrxObj = CommonFunctions.Initilize<FIN_TRX>();
                        dicCrControls = new Dictionary<string, string>();
                        if (Session[ERP.Utilities.SessionStrings.CrControls] != null)
                            dicCrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.CrControls];//Get Credit Controls
                        var crControlsSave = from pair in dicCrControls
                                             orderby pair.Key.Substring(0, 13) ascending//orderby pair.Key.Substring(0, 9) ascending
                                             select pair;//Order credit controls in ascending order
                        crCount = 1;
                        removedControlsList = null;

                        if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)
                        {
                            removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];//Get Removed Control List
                        }
                        if (removedControlsList != null && removedControlsList.Count > 0)
                        {
                            //Get the first valid crCount value.
                            while (!string.IsNullOrEmpty(removedControlsList.FirstOrDefault(aa => aa.StartsWith("dic" + crCount.ToString("0000")) && aa.EndsWith("cr"))))
                            {
                                crCount++;
                            }
                        }

                        foreach (KeyValuePair<string, string> pair in crControlsSave)
                        {
                            if (!pair.Key.Substring(7, 6).Equals("03set1") || !pair.Key.Substring(7, 6).Equals("04set1"))//Not Account aoutocomplete Hiddenfield or Account aoutocomplete Button
                            {
                                if (!crCount.ToString("0000").Equals(pair.Key.Substring(3, 4)))
                                {
                                    crCount++;
                                    if (removedControlsList != null && removedControlsList.Count > 0)//Find next crCount
                                    {
                                        while (!string.IsNullOrEmpty(removedControlsList.FirstOrDefault(aa => aa.StartsWith("dic" + crCount.ToString("0000")) && aa.EndsWith("cr"))))
                                        {
                                            crCount++;
                                        }
                                    }
                                    //finTrxObj.FTR_SEQUENCE = count;
                                    finTrxObj.FTR_SEQUENCE = crSequence++;
                                    finTrxObj.FTR_TRX_HDR = JurCurrPK;
                                    finTrxObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                                    finTrxObj.FTR_MOD_DT = DateTime.Now;
                                    finTrxObj.FTR_REMARKS = "";
                                    finTrxObj.FTR_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                    finTrxObj.FTR_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                                    finTrxObj.FTR_CRTD_DT = DateTime.Now;
                                    finTrxObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                                    finTrxObj.FTR_MOD_DT = LastModifiedTime;
                                    finTrxObj.FTR_DEPT = currentUser.CurrentDeptPK;
                                    finTrxObj.FTR_BIZUNIT = currentUser.SBUID;
                                    if (finTrxObj.FTR_DR_AMT_TC > -1 && finTrxObj.FTR_CR_AMT_TC > -1)
                                    {
                                        finTrxList.Add(finTrxObj);
                                    }
                                    finTrxObj = CommonFunctions.Initilize<FIN_TRX>();
                                    count++;
                                }
                            }
                            switch ((ControlCategory)(Enum.Parse(typeof(ControlCategory), pair.Value)))//Get Data from each Credit control
                            {
                                #region SubType
                                case ControlCategory.SubType:

                                    break;
                                #endregion
                                #region Account
                                case ControlCategory.Account:
                                    if (pair.Key.Substring(7, 6).Equals("02set1"))//Acount Textbox
                                    {
                                        hdfSubTypePk.Value = string.Empty;
                                        TextBox txtAccount = (TextBox)divGroupCr.FindControl(pair.Key);//Get Account Textbox
                                        string hdfCtrlId = pair.Key.Replace("02set102", "03set103");
                                        HiddenField hdfAccount = (HiddenField)divGroupCr.FindControl(hdfCtrlId);//Get Account Hiddenfield
                                        if (txtAccount != null && hdfAccount != null && !string.IsNullOrEmpty(hdfAccount.Value))
                                        {
                                            hdfCoaPk.Value = hdfAccount.Value;
                                            GetFieldValues(ControlsEnum.FINCOAMST);

                                            if (finCoaMstList != null && finCoaMstList.Count > 0)
                                            {
                                                hdfSubTypePk.Value = finCoaMstList[0].COA_SUB_TYPE.ToString();
                                            }
                                            Label txtSubType = (Label)pnlControls.FindControl(txtAccount.ID.Replace("02set102", "01set101"));//Get Account Label
                                            if (txtSubType != null && (txtSubType.Text == GetLocalResourceObject("New_Credit_Account").ToString() || txtSubType.Text == GetLocalResourceObject("New_Debit_Account").ToString()
                                                || txtSubType.Text == GetLocalResourceObject("Credit_Account").ToString() || txtSubType.Text == GetLocalResourceObject("Debit_Account").ToString()))
                                            {
                                                finTrxObj.FTR_ACC_SUB_TYPE = 0;
                                            }
                                            else
                                            {
                                                finTrxObj.FTR_ACC_SUB_TYPE = Convert.ToInt32(hdfSubTypePk.Value);
                                            }

                                            GetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                                            if (finCoaSubTypeCfgList[0].CST_REL_QUERY != null)//If have related qry
                                            {
                                                finTrxObj.FTR_TYPE = finCoaSubTypeCfgList[0].CST_CODE;
                                                DropDownList ddlSubLedger = (DropDownList)pnlControls.FindControl(txtAccount.ID.Replace("02set102", "51set202"));//Get Subledger DDL
                                                if (ddlSubLedger != null && !string.IsNullOrEmpty(ddlSubLedger.SelectedValue) && ddlSubLedger.SelectedValue != "-1")
                                                {
                                                    finTrxObj.FTR_TYPE_PK = Convert.ToInt32(ddlSubLedger.SelectedValue);
                                                }
                                                TextBox txtInsNo = (TextBox)pnlControls.FindControl(ddlSubLedger.ID.Replace("51set202", "52set203"));//Get Instrument TextBox
                                                if (txtInsNo != null && !txtInsNo.Text.Trim().Equals(GetLocalResourceObject("InstrumentNo").ToString()))
                                                    finTrxObj.FTR_INSTR_NO = HttpUtility.HtmlEncode(txtInsNo.Text);
                                                TextBox txtdat = (TextBox)pnlControls.FindControl(ddlSubLedger.ID.Replace("51set202", "53set204"));//Get Date TextBox
                                                if (txtdat != null && !txtdat.Text.Trim().Equals(GetLocalResourceObject("Date").ToString()))
                                                    finTrxObj.FTR_INSTR_DATE = string.IsNullOrEmpty(txtdat.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtdat.Text.Trim());
                                                TextBox favourOf = (TextBox)pnlControls.FindControl(ddlSubLedger.ID.Replace("51set202", "54set205"));//Get favourOf TextBox
                                                if (favourOf != null && !favourOf.Text.Trim().Equals(GetLocalResourceObject("FavourOf").ToString()))
                                                    finTrxObj.FTR_INSTR_FAVOUR = HttpUtility.HtmlEncode(favourOf.Text);
                                            }
                                            else
                                            {
                                                finTrxObj.FTR_TYPE = null;
                                                finTrxObj.FTR_TYPE_PK = null;
                                            }
                                            finTrxObj.FTR_ACCOUNT = Convert.ToInt16(hdfAccount.Value);

                                            #region Add Cost Center Details (credit)

                                            string hdfCostCenterCtrlId = pair.Key.Replace("02set102", "11set111");
                                            HiddenField hdfCostCenterCtr = (HiddenField)divGroupDr.FindControl(hdfCostCenterCtrlId);//Get Cost center Hiddenfield
                                            if (hdfCostCenterCtr != null && hdfCostCenterCtr.Value == "1")
                                            {
                                                if (GetGlobalResourceObject("ConfigurationsRes", "CCPercentageRequired").ToString() == "1")
                                                {
                                                    #region If the transaction amount was changed, reallocate Cost center allocation (Credit)

                                                    //string txtTransAmountCtrlId = pair.Key.Replace("02set102", "06set106");
                                                    //For New change based Base Curreny for  amount -- for MMT for cost center allocation check--Nidhin
                                                    string txtTransAmountCtrlId = string.Empty;
                                                    int TrxCurrency = Session[ERP.Utilities.SessionStrings.TransactionCurrency] == null ? int.Parse(hdfJournalCurr.Value) :
                                                                  int.Parse(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                                                    if (TrxCurrency == currentUser.BaseCurrency)
                                                        txtTransAmountCtrlId = pair.Key.Replace("02set102", "06set106");
                                                    else
                                                        txtTransAmountCtrlId = pair.Key.Replace("02set102", "08set108");
                                                    TextBox txtTransAmt = (TextBox)divGroupDr.FindControl(txtTransAmountCtrlId);//Get Transaction Amount text box
                                                    CCAmount = 0; AccountPk = 0;
                                                    if (txtTransAmt != null) decimal.TryParse(txtTransAmt.Text, out CCAmount);
                                                    if (hdfAccount != null) int.TryParse(hdfAccount.Value, out AccountPk);
                                                    CostCenterTempList = CostCenterList.DeepClone();
                                                    List<CostCenterDetails> objCostCentrDtls = CostCenterList.Where(r => r.CONTROL_ID == hdfCtrlId).ToList();
                                                    if (objCostCentrDtls != null && objCostCentrDtls.Count > 0 && (ERP.Utilities.CommonFunctions.DecimalFormat(objCostCentrDtls.Sum(r => r.FTD_AMT_BC), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) != CCAmount))
                                                    {
                                                        #region reallocating cost center allocation
                                                        if (objCostCentrDtls != null)
                                                        {
                                                            CostCenterTempList.RemoveAll(f => f.CONTROL_ID == hdfCtrlId);
                                                        }
                                                        GetFieldValues(ControlsEnum.COSTCENTER);
                                                        if (dtCostCenter != null && dtCostCenter.Rows.Count > 0)
                                                        {
                                                            foreach (DataRow drRow in dtCostCenter.Rows)
                                                            {
                                                                CostCenterDetails objCCDtls = new CostCenterDetails();
                                                                objCCDtls.FCM_CNM_PK = Convert.ToInt32(drRow["FCM_CNM_PK"]);
                                                                objCCDtls.FCM_COST_CENTER_TEXT = CommonFunctions.GetEncodedString(drRow["FCM_COST_CENTER_TEXT"]);
                                                                objCCDtls.FTD_AMT_BC = Convert.ToDecimal(drRow["FCM_AMOUNT"]);
                                                                objCCDtls.CONTROL_ID = ControlId;
                                                                CostCenterTempList.Add(objCCDtls);
                                                            }
                                                        }
                                                        #endregion
                                                        CostCenterList = CostCenterTempList.DeepClone();
                                                    }

                                                    #endregion
                                                }

                                                List<FIN_TRX_COC_DTL> objCocDtlList = new List<FIN_TRX_COC_DTL>();
                                                objCostCenterList = new List<CostCenterDetails>();
                                                objCostCenterList = CostCenterList.Where(r => r.CONTROL_ID == hdfCtrlId && r.FTD_AMT_BC > 0).ToList();
                                                if (objCostCenterList != null && objCostCenterList.Count > 0)
                                                {
                                                    foreach (CostCenterDetails objDet in objCostCenterList)
                                                    {
                                                        FIN_TRX_COC_DTL objCocDtl = new FIN_TRX_COC_DTL();
                                                        objCocDtl.FTD_CNM_PK = objDet.FCM_CNM_PK;
                                                        objCocDtl.FTD_AMT_BC = objDet.FTD_AMT_BC;
                                                        objCocDtlList.Add(objCocDtl);
                                                    }
                                                }

                                                bool CostSplitValidation = true;
                                                //string txtAmountTcCtrlId = pair.Key.Replace("02set102", "06set106");
                                                //string hdfAccountContrlId = pair.Key.Replace("02set102", "03set103");
                                                //For New change based Base Curreny for  amount -- for MMT for cost center allocation check--Nidhin
                                                string txtAmountTcCtrlId = string.Empty;
                                                int TrnsCurrency = Session[ERP.Utilities.SessionStrings.TransactionCurrency] == null ? int.Parse(hdfJournalCurr.Value) :
                                                                  int.Parse(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                                                if (TrnsCurrency == currentUser.BaseCurrency)
                                                    txtAmountTcCtrlId = pair.Key.Replace("02set102", "06set106");
                                                else
                                                    txtAmountTcCtrlId = pair.Key.Replace("02set102", "08set108");
                                                TextBox txtAmountTc = (TextBox)divGroupDr.FindControl(txtAmountTcCtrlId);//Get Transaction Amount text box
                                                if (txtAmountTc != null)
                                                {
                                                    txtAmountTc.CssClass = txtAmountTc.CssClass.Replace("border-red", "tempcss"); //"input-w8per numeric";

                                                    CostCenterAmnt = 0;
                                                    decimal.TryParse(txtAmountTc.Text, out CostCenterAmnt);
                                                    if (objCocDtlList != null && objCocDtlList.Count > 0 && objCocDtlList.Sum(r => r.FTD_AMT_BC) > 0
                                                        && ERP.Utilities.CommonFunctions.DecimalFormat(objCocDtlList.Sum(r => r.FTD_AMT_BC), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) != CostCenterAmnt)
                                                    {
                                                        CostSplitValidation = false;
                                                    }
                                                    else if (objCocDtlList != null && objCocDtlList.Count > 0
                                                        && ERP.Utilities.CommonFunctions.DecimalFormat(objCocDtlList.Sum(r => r.FTD_AMT_BC), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) == CostCenterAmnt)
                                                    {
                                                        CostSplitValidation = true;
                                                    }
                                                    else
                                                    {
                                                        AccountPk = finTrxObj.FTR_ACCOUNT.Value;
                                                        CCAmount = CostCenterAmnt;
                                                        GetFieldValues(ControlsEnum.COSTCENTER);
                                                        if (dtCostCenter != null && dtCostCenter.Rows.Count > 0)
                                                        {
                                                            CostCenterTempList = CostCenterList;
                                                            objCocDtlList = new List<FIN_TRX_COC_DTL>();
                                                            objCostCenterList = CostCenterTempList.Where(r => r.CONTROL_ID == hdfCtrlId).ToList();
                                                            objCostCenterList.ForEach(dtl =>
                                                            {
                                                                CostCenterTempList.Remove(dtl);
                                                            });

                                                            foreach (DataRow drRow in dtCostCenter.Rows)
                                                            {
                                                                CostCenterDetails objCCDtls = new CostCenterDetails();
                                                                objCCDtls.FCM_CNM_PK = Convert.ToInt32(drRow["FCM_CNM_PK"]);
                                                                objCCDtls.FCM_COST_CENTER_TEXT = CommonFunctions.GetEncodedString(drRow["FCM_COST_CENTER_TEXT"]);
                                                                objCCDtls.FTD_AMT_BC = ERP.Utilities.CommonFunctions.DecimalFormat(Convert.ToDecimal(drRow["FCM_AMOUNT"]), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                                                objCCDtls.CONTROL_ID = hdfCtrlId;
                                                                CostCenterTempList.Add(objCCDtls);

                                                                FIN_TRX_COC_DTL objCocDtl = new FIN_TRX_COC_DTL();
                                                                objCocDtl.FTD_CNM_PK = Convert.ToInt32(drRow["FCM_CNM_PK"]);
                                                                objCocDtl.FTD_AMT_BC = ERP.Utilities.CommonFunctions.DecimalFormat(Convert.ToDecimal(drRow["FCM_AMOUNT"]), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                                                objCocDtlList.Add(objCocDtl);
                                                            }
                                                            CostCenterList = CostCenterTempList;
                                                            if (ERP.Utilities.CommonFunctions.DecimalFormat(CostCenterTempList.Where(r => r.CONTROL_ID == hdfCtrlId).Sum(r => r.FTD_AMT_BC), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) != CostCenterAmnt)
                                                                CostSplitValidation = false;
                                                        }
                                                        else
                                                            CostSplitValidation = false;

                                                    }
                                                }
                                                else
                                                    CostSplitValidation = false;
                                                if (!CostSplitValidation)
                                                {
                                                    CCInvalidSplitControlTempList = CCInvalidSplitControlList;
                                                    CCInvalidSplitControlTempList.Add(new CostCenterMissmatch { AMT_TC_CONTROL_ID = txtAmountTcCtrlId });
                                                    CCInvalidSplitControlList = CCInvalidSplitControlTempList;
                                                }

                                                objCocDtlList.ForEach(dtl =>
                                                {
                                                    if (dtl.FTD_AMT_BC > 0)
                                                        finTrxObj.FIN_TRX_COC_DTL.Add(dtl);
                                                });

                                            }
                                            #endregion
                                        }
                                    }
                                    break;
                                #endregion
                                #region Narration
                                case ControlCategory.Narration:
                                    TextBox txtNarr = (TextBox)divGroupCr.FindControl(pair.Key);//Get Narration Textbox
                                    if (txtNarr != null && !string.IsNullOrEmpty(txtNarr.Text.Trim()))
                                        finTrxObj.FTR_NARRATION = HttpUtility.HtmlEncode(txtNarr.Text.Trim());
                                    break;
                                #endregion
                                #region AmountTC
                                case ControlCategory.AmountTC:
                                    TextBox txtAmountTCDr = (TextBox)divGroupCr.FindControl(pair.Key);//Get Amount in TC Textbox Textbox
                                    if (txtAmountTCDr != null && !string.IsNullOrEmpty(txtAmountTCDr.Text.Trim())
                                        && Convert.ToDecimal(txtAmountTCDr.Text.Trim()) > 0)
                                    {
                                        txtAmountTCDr.Text = string.IsNullOrEmpty(txtAmountTCDr.Text.Trim()) ? "0" : txtAmountTCDr.Text.Trim();
                                        finTrxObj.FTR_CR_AMT_TC = Convert.ToDecimal(txtAmountTCDr.Text);
                                        finTrxObj.FTR_DR_AMT_TC = 0;
                                        finTrxObj.FTR_PK = 0;
                                    }
                                    else
                                    {
                                        finTrxObj.FTR_CR_AMT_TC = 0;
                                        finTrxObj.FTR_DR_AMT_TC = 0;
                                        finTrxObj.FTR_PK = 0;
                                    }
                                    break;
                                #endregion
                                #region ExchangeRate
                                case ControlCategory.ExchangeRate:
                                    double exchangeRate = 1;
                                    TextBox txtExchangeRate = (TextBox)divGroupCr.FindControl(pair.Key);//Get Exchange Rate Textbox
                                    if (txtExchangeRate != null && !string.IsNullOrEmpty(txtExchangeRate.Text.Trim())
                                        && Convert.ToDecimal(txtExchangeRate.Text.Trim()) > 0)
                                    {
                                        double.TryParse(txtExchangeRate.Text.Trim(), out exchangeRate);
                                        finTrxObj.FTR_EXCHG_RATE = exchangeRate;
                                    }
                                    else
                                    {
                                        finTrxObj.FTR_EXCHG_RATE = exchangeRate;
                                    }
                                    break;
                                #endregion
                                #region EntryMode
                                case ControlCategory.EntryMode:
                                    byte entryMode = 0;
                                    HiddenField hdfEntryMode = (HiddenField)divGroupCr.FindControl(pair.Key);//Get EntryMode Hiddenfield
                                    if (hdfEntryMode != null && !string.IsNullOrEmpty(hdfEntryMode.Value))
                                    {
                                        byte.TryParse(hdfEntryMode.Value, out entryMode);
                                        finTrxObj.FTR_ENTRY_MODE = entryMode;
                                    }
                                    else
                                    {
                                        finTrxObj.FTR_ENTRY_MODE = entryMode;
                                    }
                                    break;
                                #endregion
                                #region AmountBC
                                case ControlCategory.AmountBC:
                                    if (finTrxObj.FTR_ENTRY_MODE != (byte)VoucherEntryMode.GainOrLoss)
                                    {
                                        TextBox txtAmountBCCr = (TextBox)divGroupDr.FindControl(pair.Key);//Get Amount in BC Textbox
                                        if (txtAmountBCCr != null && !string.IsNullOrEmpty(txtAmountBCCr.Text.Trim()))
                                        {
                                            finTrxObj.FTR_CR_AMT_BC = Convert.ToDecimal(txtAmountBCCr.Text);
                                            finTrxObj.FTR_DR_AMT_BC = 0;
                                        }
                                        else
                                        {
                                            string tcId = pair.Key.Replace(pair.Key.Substring(7, 8), "06set106");//Get Amount in TC Textbox
                                            TextBox txtTempAmountTcCr = (TextBox)pnlControls.FindControl(tcId);
                                            if (txtTempAmountTcCr != null)
                                            {
                                                finTrxObj.FTR_CR_AMT_BC = Convert.ToDecimal(txtTempAmountTcCr.Text);
                                                finTrxObj.FTR_DR_AMT_BC = 0;
                                            }
                                        }
                                    }
                                    else//Entry mode is G/L
                                    {
                                        TextBox txtAmountBCCr = (TextBox)divGroupDr.FindControl(pair.Key);//Get Amount in BC Textbox
                                        if (txtAmountBCCr != null && !string.IsNullOrEmpty(txtAmountBCCr.Text.Trim())
                                            && Convert.ToDecimal(txtAmountBCCr.Text.Trim()) > 0)
                                        {
                                            txtAmountBCCr.Text = string.IsNullOrEmpty(txtAmountBCCr.Text.Trim()) ? "0" : txtAmountBCCr.Text.Trim();
                                            finTrxObj.FTR_CR_AMT_BC = Convert.ToDecimal(txtAmountBCCr.Text);
                                            finTrxObj.FTR_DR_AMT_BC = 0;
                                            finTrxObj.FTR_DR_AMT_TC = 0;
                                            finTrxObj.FTR_CR_AMT_TC = 0;
                                            finTrxObj.FTR_PK = 0;
                                        }
                                    }
                                    break;
                                #endregion
                                #region Delete
                                case ControlCategory.Delete:
                                    break;
                                #endregion
                                #region CostCenter
                                case ControlCategory.CostCenter:
                                    break;
                                #endregion
                                #region SubLedgerLabel
                                case ControlCategory.SubledgerLabel:
                                    break;
                                #endregion
                                #region SubLedger
                                case ControlCategory.SubLedger:
                                    break;
                                #endregion
                                #region InstrumentNo
                                case ControlCategory.InstrumentNo:
                                    break;
                                #endregion
                                #region Date
                                case ControlCategory.Date:
                                    break;
                                #endregion
                                #region FavourOf
                                case ControlCategory.FavourOf:
                                    break;
                                    #endregion


                            }
                        }
                        if (crControlsSave.Count() > 0)
                        {
                            //finTrxObj.FTR_SEQUENCE = count;
                            finTrxObj.FTR_SEQUENCE = crSequence++;
                            finTrxObj.FTR_TRX_HDR = JurCurrPK;
                            finTrxObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                            finTrxObj.FTR_MOD_DT = DateTime.Now;
                            finTrxObj.FTR_REMARKS = string.Empty;
                            finTrxObj.FTR_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            finTrxObj.FTR_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                            finTrxObj.FTR_CRTD_DT = DateTime.Now;
                            finTrxObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                            finTrxObj.FTR_MOD_DT = LastModifiedTime;
                            finTrxObj.FTR_DEPT = currentUser.CurrentDeptPK;
                            finTrxObj.FTR_BIZUNIT = currentUser.SBUID;
                            if (finTrxObj.FTR_DR_AMT_TC > -1 && finTrxObj.FTR_CR_AMT_TC > -1)
                            {
                                finTrxList.Add(finTrxObj);
                            }
                        }
                        #endregion
                        if (finTrxList != null && finTrxList.Count > 0)
                        {
                            finTrxList.ForEach(dtl => finTrxHdrObj.FIN_TRX.Add(dtl));
                        }

                        break;
                    #endregion
                    #region Submit
                    case ActionsEnum.WRKFSUBMIT:
                        #region Header
                        commonServiceClient = new CommonService();
                        commonServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(commonServiceClient);
                        admConfigMstObj = CommonFunctions.Initilize<ADM_CONFIG_MST>();
                        finTrxHdrObj = CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_PK = JurCurrPK;
                        //Commented:While submiting a record,if any validation caught then vaoucher number generated and we can't get that number. 
                        //if (hdfVoucherNo.Value.Equals(string.Empty) || (txtDispPVNo.Text == GetLocalResourceObject("NewLabel").ToString()
                        //    && AST_DOC_MODE.Value != ((int)DOCMODE.Manual).ToString()))//If configuration is for voucher no generate in draft mode
                        //{
                        //    updateVocher = true;
                        //    GetFieldValues(ControlsEnum.VOUCHERNO);
                        //    txtDispPVNo.Text = hdfVoucherNo.Value;
                        //}
                        //else if (AST_DOC_MODE.Value == ((int)DOCMODE.Manual).ToString())
                        //{
                        //    hdfVoucherNo.Value = txtDispPVNo.Text.Trim();
                        //}
                        //finTrxHdrObj.FTH_VOUCHER_NO = HttpUtility.HtmlEncode(hdfVoucherNo.Value);
                        finTrxHdrObj.FTH_DATE = DateTime.Parse(txtPVDate.Text);
                        finTrxHdrObj.FTH_TRX_DATE = DateTime.Parse(txtPVDate.Text);
                        finTrxHdrObj.FTH_REF_TYPE = Session[ERP.Utilities.SessionStrings.TransactionType].ToString();
                        finTrxHdrObj.FTH_REF_PK = Session[ERP.Utilities.SessionStrings.TransactionPK] == null ? JurCurrPK :
                            Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString());
                        if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.JV || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.PCS)
                        {
                            finTrxHdrObj.FTH_REF_NO = HttpUtility.HtmlEncode(txtRefNo.Text.Trim());
                            finTrxHdrObj.FTH_REF_DATE = string.IsNullOrEmpty(txtRefDate.Text.Trim()) ? DateTime.Parse(txtPVDate.Text) :
                                 DateTime.Parse(txtRefDate.Text.Trim());
                        }
                        else
                        {
                            finTrxHdrObj.FTH_REF_NO = Session[ERP.Utilities.SessionStrings.TransactionNo] == null ? HttpUtility.HtmlEncode(txtRefNo.Text.Trim()) :
                                Session[ERP.Utilities.SessionStrings.TransactionNo].ToString();
                            finTrxHdrObj.FTH_REF_DATE = Session[ERP.Utilities.SessionStrings.TransactionDate] == null ? DateTime.Parse(txtPVDate.Text) :
                            DateTime.Parse(Session[ERP.Utilities.SessionStrings.TransactionDate].ToString());
                        }

                        if (!string.IsNullOrEmpty(txtTaxDate.Text.Trim()) && (finTrxHdrObj.FTH_REF_TYPE == ApplicationType.PIJ || finTrxHdrObj.FTH_REF_TYPE == ApplicationType.PSIJ || finTrxHdrObj.FTH_REF_TYPE == ApplicationType.EIJ))
                        {
                            TaxDate = DateTime.Parse(txtTaxDate.Text.Trim());
                        }
                        finTrxHdrObj.FTH_TAX_DATE = TaxDate;

                        finTrxHdrObj.FTH_NARRATION = HttpUtility.HtmlEncode(txtNarration.Text);
                        finTrxHdrObj.FTH_TRX_CURR = Session[ERP.Utilities.SessionStrings.TransactionCurrency] == null ? int.Parse(hdfJournalCurr.Value) :
                            int.Parse(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                        finTrxHdrObj.FTH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                        finTrxHdrObj.FTH_COMPANY = Convert.ToInt32(ddlVoucherCompany.SelectedValue);
                        finTrxHdrObj.FTH_FIN_YEAR = 0;
                        finTrxHdrObj.FTH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                        finTrxHdrObj.FTH_STATUS = Convert.ToByte(hdfVoucherStatus.Value);
                        finTrxHdrObj.FTH_ACTIVE = 1;
                        finTrxHdrObj.FTH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                        finTrxHdrObj.FTH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                        finTrxHdrObj.FTH_MOD_DT = LastModifiedTime;
                        finTrxHdrObj.FTH_DEPT = ExternalUserDept > 0 ? ExternalUserDept : currentUser.CurrentDeptPK;
                        finTrxHdrObj.FTH_BIZUNIT = currentUser.SBUID;
                        finTrxHdrObj.FTH_IS_JRNLD = true;
                        finTrxHdrObj.FTH_EXCHG_RATE = double.Parse(txtJournalExchangeRate.Text);
                        #endregion
                        #region Debit
                        finTrxObj = CommonFunctions.Initilize<FIN_TRX>();
                        dicDrControls = new Dictionary<string, string>();
                        if (Session[ERP.Utilities.SessionStrings.DrControls] != null)
                            dicDrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.DrControls];//Get Debit Controls
                        var drControlsSubmit = from pair in dicDrControls
                                               orderby pair.Key.Substring(0, 13) ascending//orderby pair.Key.Substring(0, 9) ascending
                                               select pair;//Order debit controls in ascending order
                        drCount = 1;
                        count = 1;

                        removedControlsList = null;

                        if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)
                        {
                            removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];//Get Removed Control List
                        }
                        if (removedControlsList != null && removedControlsList.Count > 0)
                        {
                            //Get the first valid drCount value.
                            while (!string.IsNullOrEmpty(removedControlsList.FirstOrDefault(aa => aa.StartsWith("dic" + drCount.ToString("0000")) && aa.EndsWith("dr"))))
                            {
                                drCount++;
                            }
                        }

                        foreach (KeyValuePair<string, string> pair in drControlsSubmit)
                        {
                            if (!pair.Key.Substring(7, 6).Equals("03set1") || !pair.Key.Substring(7, 6).Equals("04set1"))//Not Account aoutocomplete Hiddenfield or Account aoutocomplete Button
                            {
                                if (!drCount.ToString("0000").Equals(pair.Key.Substring(3, 4)))
                                {
                                    drCount++;
                                    if (removedControlsList != null && removedControlsList.Count > 0)//Find next drcount
                                    {
                                        while (!string.IsNullOrEmpty(removedControlsList.FirstOrDefault(aa => aa.StartsWith("dic" + drCount.ToString("0000")) && aa.EndsWith("dr"))))
                                        {
                                            drCount++;
                                        }
                                    }
                                    //finTrxObj.FTR_SEQUENCE = count;
                                    finTrxObj.FTR_SEQUENCE = drSequence;
                                    finTrxObj.FTR_TRX_HDR = JurCurrPK;
                                    finTrxObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                                    finTrxObj.FTR_MOD_DT = DateTime.Now;
                                    finTrxObj.FTR_REMARKS = "";
                                    finTrxObj.FTR_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                    finTrxObj.FTR_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                                    finTrxObj.FTR_CRTD_DT = DateTime.Now;
                                    finTrxObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                                    finTrxObj.FTR_MOD_DT = LastModifiedTime;
                                    finTrxObj.FTR_DEPT = currentUser.CurrentDeptPK;
                                    finTrxObj.FTR_BIZUNIT = currentUser.SBUID;

                                    if (finTrxObj.FTR_DR_AMT_TC > -1 && finTrxObj.FTR_CR_AMT_TC > -1)
                                    {
                                        finTrxList.Add(finTrxObj);
                                    }
                                    finTrxObj = CommonFunctions.Initilize<FIN_TRX>();
                                    count++;
                                }
                            }
                            switch ((ControlCategory)(Enum.Parse(typeof(ControlCategory), pair.Value)))//Get Data from each Debit control
                            {
                                #region SubType
                                case ControlCategory.SubType:
                                    break;
                                #endregion
                                #region Account
                                case ControlCategory.Account:
                                    if (pair.Key.Substring(7, 6).Equals("02set1"))//Acount Textbox
                                    {
                                        hdfSubTypePk.Value = string.Empty;
                                        TextBox txtAccount = (TextBox)divGroupDr.FindControl(pair.Key);//Get Account Textbox
                                        string hdfCtrlId = pair.Key.Replace("02set102", "03set103");
                                        HiddenField hdfAccount = (HiddenField)divGroupDr.FindControl(hdfCtrlId);//Get Account Hiddenfield
                                        if (txtAccount != null && hdfAccount != null && !string.IsNullOrEmpty(hdfAccount.Value))
                                        {
                                            hdfCoaPk.Value = hdfAccount.Value;
                                            GetFieldValues(ControlsEnum.FINCOAMST);

                                            if (finCoaMstList != null && finCoaMstList.Count > 0)
                                            {
                                                hdfSubTypePk.Value = finCoaMstList[0].COA_SUB_TYPE.ToString();
                                            }
                                            Label txtSubType = (Label)pnlControls.FindControl(txtAccount.ID.Replace("02set102", "01set101"));//Get Account Label
                                            if (txtSubType != null && (txtSubType.Text == GetLocalResourceObject("New_Credit_Account").ToString() || txtSubType.Text == GetLocalResourceObject("New_Debit_Account").ToString()
                                                || txtSubType.Text == GetLocalResourceObject("Credit_Account").ToString() || txtSubType.Text == GetLocalResourceObject("Debit_Account").ToString()))
                                            {
                                                finTrxObj.FTR_ACC_SUB_TYPE = 0;
                                            }
                                            else
                                            {
                                                finTrxObj.FTR_ACC_SUB_TYPE = Convert.ToInt32(hdfSubTypePk.Value);
                                            }

                                            GetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                                            if (finCoaSubTypeCfgList[0].CST_REL_QUERY != null)//If have related qry
                                            {
                                                finTrxObj.FTR_TYPE = finCoaSubTypeCfgList[0].CST_CODE;
                                                DropDownList ddlSubLedger = (DropDownList)pnlControls.FindControl(txtAccount.ID.Replace("02set102", "51set202"));//Get Subledger DDL
                                                if (ddlSubLedger != null && !string.IsNullOrEmpty(ddlSubLedger.SelectedValue) && ddlSubLedger.SelectedValue != "-1")//Have subledger value
                                                {
                                                    finTrxObj.FTR_TYPE_PK = Convert.ToInt32(ddlSubLedger.SelectedValue);
                                                }
                                                TextBox txtInsNo = (TextBox)pnlControls.FindControl(ddlSubLedger.ID.Replace("51set202", "52set203"));//Get Instrument TextBox
                                                if (txtInsNo != null && !txtInsNo.Text.Trim().Equals(GetLocalResourceObject("InstrumentNo").ToString()))
                                                    finTrxObj.FTR_INSTR_NO = txtInsNo.Text;
                                                TextBox txtdat = (TextBox)pnlControls.FindControl(ddlSubLedger.ID.Replace("51set202", "53set204"));//Get Date TextBox
                                                if (txtdat != null && !txtdat.Text.Trim().Equals(GetLocalResourceObject("Date").ToString()))
                                                    finTrxObj.FTR_INSTR_DATE = string.IsNullOrEmpty(txtdat.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtdat.Text.Trim());
                                                TextBox favourOf = (TextBox)pnlControls.FindControl(ddlSubLedger.ID.Replace("51set202", "54set205"));//Get favourOf TextBox
                                                if (favourOf != null && !favourOf.Text.Trim().Equals(GetLocalResourceObject("FavourOf").ToString()))
                                                    finTrxObj.FTR_INSTR_FAVOUR = HttpUtility.HtmlEncode(favourOf.Text);
                                            }
                                            else
                                            {
                                                finTrxObj.FTR_TYPE = null;
                                                finTrxObj.FTR_TYPE_PK = null;
                                            }
                                            finTrxObj.FTR_ACCOUNT = Convert.ToInt16(hdfAccount.Value);

                                            #region Add Cost Center Details
                                            string hdfCostCenterCtrlId = pair.Key.Replace("02set102", "11set111");
                                            HiddenField hdfCostCenterCtr = (HiddenField)divGroupDr.FindControl(hdfCostCenterCtrlId);//Get Cost center Hiddenfield
                                            if (hdfCostCenterCtr != null && hdfCostCenterCtr.Value == "1")
                                            {
                                                List<FIN_TRX_COC_DTL> objCocDtlList = new List<FIN_TRX_COC_DTL>();
                                                objCostCenterList = new List<CostCenterDetails>();
                                                objCostCenterList = CostCenterList.Where(r => r.CONTROL_ID == hdfCtrlId && r.FTD_AMT_BC > 0).ToList();
                                                if (objCostCenterList != null && objCostCenterList.Count > 0)
                                                {
                                                    foreach (CostCenterDetails objDet in objCostCenterList)
                                                    {
                                                        FIN_TRX_COC_DTL objCocDtl = new FIN_TRX_COC_DTL();
                                                        objCocDtl.FTD_CNM_PK = objDet.FCM_CNM_PK;
                                                        objCocDtl.FTD_AMT_BC = objDet.FTD_AMT_BC;
                                                        objCocDtlList.Add(objCocDtl);
                                                    }
                                                }

                                                bool CostSplitValidation = true;
                                                //string txtAmountTcCtrlId = pair.Key.Replace("02set102", "06set106");
                                                //string hdfAccountContrlId = pair.Key.Replace("02set102", "03set103");
                                                //For New change based Base Curreny for  amount -- for MMT for cost center allocation check--Nidhin
                                                string txtAmountTcCtrlId = string.Empty;
                                                int TrxCurrency = Session[ERP.Utilities.SessionStrings.TransactionCurrency] == null ? int.Parse(hdfJournalCurr.Value) :
                                                                  int.Parse(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                                                if (TrxCurrency == currentUser.BaseCurrency)
                                                    txtAmountTcCtrlId = pair.Key.Replace("02set102", "06set106");
                                                else
                                                    txtAmountTcCtrlId = pair.Key.Replace("02set102", "08set108");
                                                TextBox txtAmountTc = (TextBox)divGroupDr.FindControl(txtAmountTcCtrlId);//Get Transaction Amount text box
                                                if (txtAmountTc != null)
                                                {
                                                    txtAmountTc.CssClass = txtAmountTc.CssClass.Replace("border-red", "tempcss"); //"input-w8per numeric";

                                                    CostCenterAmnt = 0;
                                                    decimal.TryParse(txtAmountTc.Text, out CostCenterAmnt);
                                                    if (objCocDtlList != null && objCocDtlList.Count > 0 && objCocDtlList.Sum(r => r.FTD_AMT_BC) > 0
                                                        && ERP.Utilities.CommonFunctions.DecimalFormat(objCocDtlList.Sum(r => r.FTD_AMT_BC), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) != CostCenterAmnt)
                                                    {
                                                        CostSplitValidation = false;
                                                    }
                                                    else if (objCocDtlList != null && objCocDtlList.Count > 0
                                                        && ERP.Utilities.CommonFunctions.DecimalFormat(objCocDtlList.Sum(r => r.FTD_AMT_BC), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) == CostCenterAmnt)
                                                    {
                                                        CostSplitValidation = true;
                                                    }
                                                    else
                                                    {
                                                        AccountPk = finTrxObj.FTR_ACCOUNT.Value;
                                                        CCAmount = CostCenterAmnt;
                                                        GetFieldValues(ControlsEnum.COSTCENTER);
                                                        if (dtCostCenter != null && dtCostCenter.Rows.Count > 0)
                                                        {
                                                            CostCenterTempList = CostCenterList;
                                                            objCocDtlList = new List<FIN_TRX_COC_DTL>();
                                                            objCostCenterList = CostCenterTempList.Where(r => r.CONTROL_ID == hdfCtrlId).ToList();
                                                            objCostCenterList.ForEach(dtl =>
                                                            {
                                                                CostCenterTempList.Remove(dtl);
                                                            });

                                                            foreach (DataRow drRow in dtCostCenter.Rows)
                                                            {
                                                                CostCenterDetails objCCDtls = new CostCenterDetails();
                                                                objCCDtls.FCM_CNM_PK = Convert.ToInt32(drRow["FCM_CNM_PK"]);
                                                                objCCDtls.FCM_COST_CENTER_TEXT = CommonFunctions.GetEncodedString(drRow["FCM_COST_CENTER_TEXT"]);
                                                                objCCDtls.FTD_AMT_BC = ERP.Utilities.CommonFunctions.DecimalFormat(Convert.ToDecimal(drRow["FCM_AMOUNT"]), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                                                objCCDtls.CONTROL_ID = hdfCtrlId;
                                                                CostCenterTempList.Add(objCCDtls);

                                                                FIN_TRX_COC_DTL objCocDtl = new FIN_TRX_COC_DTL();
                                                                objCocDtl.FTD_CNM_PK = Convert.ToInt32(drRow["FCM_CNM_PK"]);
                                                                objCocDtl.FTD_AMT_BC = ERP.Utilities.CommonFunctions.DecimalFormat(Convert.ToDecimal(drRow["FCM_AMOUNT"]), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                                                objCocDtlList.Add(objCocDtl);
                                                            }
                                                            CostCenterList = CostCenterTempList;
                                                            if (ERP.Utilities.CommonFunctions.DecimalFormat(CostCenterTempList.Where(r => r.CONTROL_ID == hdfCtrlId).Sum(r => r.FTD_AMT_BC), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) != CostCenterAmnt)
                                                                CostSplitValidation = false;
                                                        }
                                                        else
                                                            CostSplitValidation = false;
                                                    }
                                                }
                                                else
                                                    CostSplitValidation = false;
                                                if (!CostSplitValidation)
                                                {
                                                    CCInvalidSplitControlTempList = CCInvalidSplitControlList;
                                                    CCInvalidSplitControlTempList.Add(new CostCenterMissmatch { AMT_TC_CONTROL_ID = txtAmountTcCtrlId });
                                                    CCInvalidSplitControlList = CCInvalidSplitControlTempList;
                                                }

                                                objCocDtlList.ForEach(dtl =>
                                                {
                                                    if (dtl.FTD_AMT_BC > 0)
                                                        finTrxObj.FIN_TRX_COC_DTL.Add(dtl);
                                                });

                                            }
                                            #endregion
                                        }
                                    }
                                    break;
                                #endregion
                                #region Narration
                                case ControlCategory.Narration:
                                    TextBox txtNarr = (TextBox)divGroupDr.FindControl(pair.Key);//Get Narration Textbox
                                    if (txtNarr != null && !string.IsNullOrEmpty(txtNarr.Text.Trim()))
                                        finTrxObj.FTR_NARRATION = HttpUtility.HtmlEncode(txtNarr.Text.Trim());
                                    break;
                                #endregion
                                #region AmountTC
                                case ControlCategory.AmountTC:
                                    TextBox txtAmountTCDr = (TextBox)divGroupDr.FindControl(pair.Key);//Get Amount in TC Textbox Textbox
                                    if (txtAmountTCDr != null && !string.IsNullOrEmpty(txtAmountTCDr.Text.Trim())
                                        && Convert.ToDecimal(txtAmountTCDr.Text.Trim()) > 0)
                                    {
                                        txtAmountTCDr.Text = string.IsNullOrEmpty(txtAmountTCDr.Text.Trim()) ? "0" : txtAmountTCDr.Text.Trim();
                                        finTrxObj.FTR_DR_AMT_TC = Convert.ToDecimal(txtAmountTCDr.Text);
                                        finTrxObj.FTR_CR_AMT_TC = 0;
                                        finTrxObj.FTR_PK = 0;
                                    }
                                    else
                                    {
                                        finTrxObj.FTR_DR_AMT_TC = 0;
                                        finTrxObj.FTR_CR_AMT_TC = 0;
                                        finTrxObj.FTR_PK = 0;
                                    }
                                    break;
                                #endregion
                                #region ExchangeRate
                                case ControlCategory.ExchangeRate:
                                    double exchangeRate = 1;
                                    TextBox txtExchangeRate = (TextBox)divGroupDr.FindControl(pair.Key);//Get Exchange Rate Textbox
                                    if (txtExchangeRate != null && !string.IsNullOrEmpty(txtExchangeRate.Text.Trim())
                                        && Convert.ToDecimal(txtExchangeRate.Text.Trim()) > 0)
                                    {
                                        double.TryParse(txtExchangeRate.Text.Trim(), out exchangeRate);
                                        finTrxObj.FTR_EXCHG_RATE = exchangeRate;
                                    }
                                    else
                                    {
                                        finTrxObj.FTR_EXCHG_RATE = exchangeRate;
                                    }
                                    break;
                                #endregion
                                #region EntryMode
                                case ControlCategory.EntryMode:
                                    byte entryMode = 0;
                                    HiddenField hdfEntryMode = (HiddenField)divGroupDr.FindControl(pair.Key);//Get EntryMode Hiddenfield
                                    if (hdfEntryMode != null && !string.IsNullOrEmpty(hdfEntryMode.Value))
                                    {
                                        byte.TryParse(hdfEntryMode.Value, out entryMode);
                                        finTrxObj.FTR_ENTRY_MODE = entryMode;
                                    }
                                    else
                                    {
                                        finTrxObj.FTR_ENTRY_MODE = entryMode;
                                    }
                                    break;
                                #endregion
                                #region AmountBC
                                case ControlCategory.AmountBC:
                                    if (finTrxObj.FTR_ENTRY_MODE != (byte)VoucherEntryMode.GainOrLoss)
                                    {
                                        TextBox txtAmountBCDr = (TextBox)divGroupDr.FindControl(pair.Key);//Get Amount in BC Textbox
                                        if (txtAmountBCDr != null && !string.IsNullOrEmpty(txtAmountBCDr.Text.Trim()))
                                        {
                                            finTrxObj.FTR_DR_AMT_BC = Convert.ToDecimal(txtAmountBCDr.Text);
                                            finTrxObj.FTR_CR_AMT_BC = 0;
                                        }
                                        else
                                        {
                                            string tcId = pair.Key.Replace(pair.Key.Substring(7, 8), "06set106");//Get Amount in TC Textbox
                                            TextBox txtTempAmountTCDR = (TextBox)pnlControls.FindControl(tcId);
                                            if (txtTempAmountTCDR != null)
                                            {
                                                finTrxObj.FTR_DR_AMT_BC = Convert.ToDecimal(txtTempAmountTCDR.Text);
                                                finTrxObj.FTR_CR_AMT_BC = 0;
                                            }
                                        }
                                    }
                                    else//Entry mode is G/L
                                    {
                                        TextBox txtAmountBCDr = (TextBox)divGroupDr.FindControl(pair.Key);//Get Amount in BC Textbox
                                        if (txtAmountBCDr != null && !string.IsNullOrEmpty(txtAmountBCDr.Text.Trim())
                                            && Convert.ToDecimal(txtAmountBCDr.Text.Trim()) > 0)
                                        {
                                            txtAmountBCDr.Text = string.IsNullOrEmpty(txtAmountBCDr.Text.Trim()) ? "0" : txtAmountBCDr.Text.Trim();
                                            finTrxObj.FTR_DR_AMT_BC = Convert.ToDecimal(txtAmountBCDr.Text);
                                            finTrxObj.FTR_CR_AMT_BC = 0;
                                            finTrxObj.FTR_DR_AMT_TC = 0;
                                            finTrxObj.FTR_CR_AMT_TC = 0;
                                            finTrxObj.FTR_EXCHG_RATE = 0;
                                            finTrxObj.FTR_PK = 0;
                                        }
                                    }
                                    break;
                                #endregion
                                #region Delete
                                case ControlCategory.Delete:
                                    break;
                                #endregion
                                #region CostCenter
                                case ControlCategory.CostCenter:
                                    break;
                                #endregion
                                #region SubLedgerLabel
                                case ControlCategory.SubledgerLabel:
                                    break;
                                #endregion
                                #region SubLedger
                                case ControlCategory.SubLedger:
                                    break;
                                #endregion
                                #region InstrumentNo
                                case ControlCategory.InstrumentNo:
                                    break;
                                #endregion
                                #region Date
                                case ControlCategory.Date:
                                    break;
                                #endregion
                                #region FavourOf
                                case ControlCategory.FavourOf:
                                    break;
                                    #endregion
                            }
                        }
                        if (drControlsSubmit.Count() > 0)
                        {
                            //finTrxObj.FTR_SEQUENCE = count;
                            finTrxObj.FTR_SEQUENCE = drSequence++;
                            finTrxObj.FTR_TRX_HDR = JurCurrPK;
                            finTrxObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                            finTrxObj.FTR_MOD_DT = DateTime.Now;
                            finTrxObj.FTR_REMARKS = "";
                            finTrxObj.FTR_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            finTrxObj.FTR_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                            finTrxObj.FTR_CRTD_DT = DateTime.Now;
                            finTrxObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                            finTrxObj.FTR_MOD_DT = LastModifiedTime;
                            finTrxObj.FTR_DEPT = currentUser.CurrentDeptPK;
                            finTrxObj.FTR_BIZUNIT = currentUser.SBUID;
                            if (finTrxObj.FTR_DR_AMT_TC > -1 && finTrxObj.FTR_CR_AMT_TC > -1)
                            {
                                finTrxList.Add(finTrxObj);
                            }
                        }
                        #endregion
                        #region credit
                        finTrxObj = CommonFunctions.Initilize<FIN_TRX>();
                        dicCrControls = new Dictionary<string, string>();
                        if (Session[ERP.Utilities.SessionStrings.CrControls] != null)
                            dicCrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.CrControls];//Get Credit Controls
                        var crControlsSubmit = from pair in dicCrControls
                                               orderby pair.Key.Substring(0, 13) ascending //orderby pair.Key.Substring(0, 9) ascending
                                               select pair;//Order credit controls in ascending order
                        crCount = 1;
                        removedControlsList = null;

                        if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)
                        {
                            removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];//Get Removed Control List
                        }
                        if (removedControlsList != null && removedControlsList.Count > 0)
                        {
                            //Get the first valid crCount value.
                            while (!string.IsNullOrEmpty(removedControlsList.FirstOrDefault(aa => aa.StartsWith("dic" + crCount.ToString("0000")) && aa.EndsWith("cr"))))
                            {
                                crCount++;
                            }
                        }

                        foreach (KeyValuePair<string, string> pair in crControlsSubmit)
                        {
                            if (!pair.Key.Substring(7, 6).Equals("03set1") || !pair.Key.Substring(7, 6).Equals("04set1"))//Not Account aoutocomplete Hiddenfield or Account aoutocomplete Button
                            {
                                if (!crCount.ToString("0000").Equals(pair.Key.Substring(3, 4)))
                                {
                                    crCount++;
                                    if (removedControlsList != null && removedControlsList.Count > 0)//Find next crCount
                                    {
                                        while (!string.IsNullOrEmpty(removedControlsList.FirstOrDefault(aa => aa.StartsWith("dic" + crCount.ToString("0000")) && aa.EndsWith("cr"))))
                                        {
                                            crCount++;
                                        }
                                    }
                                    //finTrxObj.FTR_SEQUENCE = count;
                                    finTrxObj.FTR_SEQUENCE = crSequence;
                                    finTrxObj.FTR_TRX_HDR = JurCurrPK;
                                    finTrxObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                                    finTrxObj.FTR_MOD_DT = DateTime.Now;
                                    finTrxObj.FTR_REMARKS = "";
                                    finTrxObj.FTR_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                    finTrxObj.FTR_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                                    finTrxObj.FTR_CRTD_DT = DateTime.Now;
                                    finTrxObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                                    finTrxObj.FTR_MOD_DT = LastModifiedTime;
                                    finTrxObj.FTR_DEPT = currentUser.CurrentDeptPK;
                                    finTrxObj.FTR_BIZUNIT = currentUser.SBUID;
                                    if (finTrxObj.FTR_DR_AMT_TC > -1 && finTrxObj.FTR_CR_AMT_TC > -1)
                                    {
                                        finTrxList.Add(finTrxObj);
                                    }
                                    finTrxObj = CommonFunctions.Initilize<FIN_TRX>();
                                    count++;
                                }
                            }
                            switch ((ControlCategory)(Enum.Parse(typeof(ControlCategory), pair.Value)))//Get Data from each Credit control
                            {
                                #region SubType
                                case ControlCategory.SubType:
                                    break;
                                #endregion
                                #region Account
                                case ControlCategory.Account:
                                    if (pair.Key.Substring(7, 6).Equals("02set1"))//Acount Textbox
                                    {
                                        hdfSubTypePk.Value = string.Empty;
                                        TextBox txtAccount = (TextBox)divGroupCr.FindControl(pair.Key);//Get Account Textbox
                                        string hdfCtrlId = pair.Key.Replace("02set102", "03set103");
                                        HiddenField hdfAccount = (HiddenField)divGroupCr.FindControl(hdfCtrlId);//Get Account Hiddenfield
                                        if (txtAccount != null && hdfAccount != null && !string.IsNullOrEmpty(hdfAccount.Value))
                                        {
                                            hdfCoaPk.Value = hdfAccount.Value;
                                            GetFieldValues(ControlsEnum.FINCOAMST);

                                            if (finCoaMstList != null && finCoaMstList.Count > 0)
                                            {
                                                hdfSubTypePk.Value = finCoaMstList[0].COA_SUB_TYPE.ToString();
                                            }

                                            Label txtSubType = (Label)pnlControls.FindControl(txtAccount.ID.Replace("02set102", "01set101"));//Get Account Label
                                            if (txtSubType != null && (txtSubType.Text == GetLocalResourceObject("New_Credit_Account").ToString() || txtSubType.Text == GetLocalResourceObject("New_Debit_Account").ToString()
                                                || txtSubType.Text == GetLocalResourceObject("Credit_Account").ToString() || txtSubType.Text == GetLocalResourceObject("Debit_Account").ToString()))
                                            {
                                                finTrxObj.FTR_ACC_SUB_TYPE = 0;
                                            }
                                            else
                                            {
                                                finTrxObj.FTR_ACC_SUB_TYPE = Convert.ToInt32(hdfSubTypePk.Value);
                                            }

                                            GetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                                            if (finCoaSubTypeCfgList[0].CST_REL_QUERY != null)//If have related qry
                                            {
                                                finTrxObj.FTR_TYPE = finCoaSubTypeCfgList[0].CST_CODE;
                                                DropDownList ddlSubLedger = (DropDownList)pnlControls.FindControl(txtAccount.ID.Replace("02set102", "51set202"));//Get Subledger DDL
                                                if (ddlSubLedger != null && !string.IsNullOrEmpty(ddlSubLedger.SelectedValue) && ddlSubLedger.SelectedValue != "-1")//Have Subledger
                                                {
                                                    finTrxObj.FTR_TYPE_PK = Convert.ToInt32(ddlSubLedger.SelectedValue);
                                                }
                                                TextBox txtInsNo = (TextBox)pnlControls.FindControl(ddlSubLedger.ID.Replace("51set202", "52set203"));//Get Instrument TextBox
                                                if (txtInsNo != null && !txtInsNo.Text.Trim().Equals(GetLocalResourceObject("InstrumentNo").ToString()))
                                                    finTrxObj.FTR_INSTR_NO = txtInsNo.Text;
                                                TextBox txtdat = (TextBox)pnlControls.FindControl(ddlSubLedger.ID.Replace("51set202", "53set204"));//Get Date TextBox
                                                if (txtdat != null && !txtdat.Text.Trim().Equals(GetLocalResourceObject("Date").ToString()))
                                                    finTrxObj.FTR_INSTR_DATE = string.IsNullOrEmpty(txtdat.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtdat.Text.Trim());
                                                TextBox favourOf = (TextBox)pnlControls.FindControl(ddlSubLedger.ID.Replace("51set202", "54set205"));//Get favourOf TextBox
                                                if (favourOf != null && !favourOf.Text.Trim().Equals(GetLocalResourceObject("FavourOf").ToString()))
                                                    finTrxObj.FTR_INSTR_FAVOUR = HttpUtility.HtmlEncode(favourOf.Text);
                                            }
                                            else
                                            {
                                                finTrxObj.FTR_TYPE = null;
                                                finTrxObj.FTR_TYPE_PK = null;
                                            }
                                            finTrxObj.FTR_ACCOUNT = Convert.ToInt16(hdfAccount.Value);

                                            #region Add Cost Center Details
                                            string hdfCostCenterCtrlId = pair.Key.Replace("02set102", "11set111");
                                            HiddenField hdfCostCenterCtr = (HiddenField)divGroupDr.FindControl(hdfCostCenterCtrlId);//Get Cost center Hiddenfield
                                            if (hdfCostCenterCtr != null && hdfCostCenterCtr.Value == "1")
                                            {
                                                List<FIN_TRX_COC_DTL> objCocDtlList = new List<FIN_TRX_COC_DTL>();
                                                objCostCenterList = new List<CostCenterDetails>();
                                                objCostCenterList = CostCenterList.Where(r => r.CONTROL_ID == hdfCtrlId && r.FTD_AMT_BC > 0).ToList();
                                                if (objCostCenterList != null && objCostCenterList.Count > 0)
                                                {
                                                    foreach (CostCenterDetails objDet in objCostCenterList)
                                                    {
                                                        FIN_TRX_COC_DTL objCocDtl = new FIN_TRX_COC_DTL();
                                                        objCocDtl.FTD_CNM_PK = objDet.FCM_CNM_PK;
                                                        objCocDtl.FTD_AMT_BC = objDet.FTD_AMT_BC;
                                                        objCocDtlList.Add(objCocDtl);
                                                    }
                                                }

                                                bool CostSplitValidation = true;
                                                //string txtAmountTcCtrlId = pair.Key.Replace("02set102", "06set106");
                                                //string hdfAccountContrlId = pair.Key.Replace("02set102", "03set103");
                                                //For New change based Base Curreny for  amount -- for MMT for cost center allocation check--Nidhin
                                                string txtAmountTcCtrlId = string.Empty;
                                                int TrxCurrency = Session[ERP.Utilities.SessionStrings.TransactionCurrency] == null ? int.Parse(hdfJournalCurr.Value) :
                                                                 int.Parse(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                                                if (TrxCurrency == currentUser.BaseCurrency)
                                                    txtAmountTcCtrlId = pair.Key.Replace("02set102", "06set106");
                                                else
                                                    txtAmountTcCtrlId = pair.Key.Replace("02set102", "08set108");
                                                TextBox txtAmountTc = (TextBox)divGroupDr.FindControl(txtAmountTcCtrlId);//Get Transaction Amount text box
                                                if (txtAmountTc != null)
                                                {
                                                    txtAmountTc.CssClass = txtAmountTc.CssClass.Replace("border-red", "tempcss"); //"input-w8per numeric";

                                                    CostCenterAmnt = 0;
                                                    decimal.TryParse(txtAmountTc.Text, out CostCenterAmnt);
                                                    if (objCocDtlList != null && objCocDtlList.Count > 0 && objCocDtlList.Sum(r => r.FTD_AMT_BC) > 0
                                                        && ERP.Utilities.CommonFunctions.DecimalFormat(objCocDtlList.Sum(r => r.FTD_AMT_BC), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) != CostCenterAmnt)
                                                    {
                                                        CostSplitValidation = false;
                                                    }
                                                    else if (objCocDtlList != null && objCocDtlList.Count > 0
                                                        && ERP.Utilities.CommonFunctions.DecimalFormat(objCocDtlList.Sum(r => r.FTD_AMT_BC), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) == CostCenterAmnt)
                                                    {
                                                        CostSplitValidation = true;
                                                    }
                                                    else
                                                    {
                                                        AccountPk = finTrxObj.FTR_ACCOUNT.Value;
                                                        CCAmount = CostCenterAmnt;
                                                        GetFieldValues(ControlsEnum.COSTCENTER);
                                                        if (dtCostCenter != null && dtCostCenter.Rows.Count > 0)
                                                        {
                                                            CostCenterTempList = CostCenterList;
                                                            objCocDtlList = new List<FIN_TRX_COC_DTL>();
                                                            objCostCenterList = CostCenterTempList.Where(r => r.CONTROL_ID == hdfCtrlId).ToList();
                                                            objCostCenterList.ForEach(dtl =>
                                                            {
                                                                CostCenterTempList.Remove(dtl);
                                                            });

                                                            foreach (DataRow drRow in dtCostCenter.Rows)
                                                            {
                                                                CostCenterDetails objCCDtls = new CostCenterDetails();
                                                                objCCDtls.FCM_CNM_PK = Convert.ToInt32(drRow["FCM_CNM_PK"]);
                                                                objCCDtls.FCM_COST_CENTER_TEXT = CommonFunctions.GetEncodedString(drRow["FCM_COST_CENTER_TEXT"]);
                                                                objCCDtls.FTD_AMT_BC = ERP.Utilities.CommonFunctions.DecimalFormat(Convert.ToDecimal(drRow["FCM_AMOUNT"]), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                                                objCCDtls.CONTROL_ID = hdfCtrlId;
                                                                CostCenterTempList.Add(objCCDtls);

                                                                FIN_TRX_COC_DTL objCocDtl = new FIN_TRX_COC_DTL();
                                                                objCocDtl.FTD_CNM_PK = Convert.ToInt32(drRow["FCM_CNM_PK"]);
                                                                objCocDtl.FTD_AMT_BC = ERP.Utilities.CommonFunctions.DecimalFormat(Convert.ToDecimal(drRow["FCM_AMOUNT"]), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                                                objCocDtlList.Add(objCocDtl);
                                                            }
                                                            CostCenterList = CostCenterTempList;
                                                            if (ERP.Utilities.CommonFunctions.DecimalFormat(CostCenterTempList.Where(r => r.CONTROL_ID == hdfCtrlId).Sum(r => r.FTD_AMT_BC), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) != CostCenterAmnt)
                                                                CostSplitValidation = false;
                                                        }
                                                        else
                                                            CostSplitValidation = false;
                                                    }
                                                }
                                                else
                                                    CostSplitValidation = false;
                                                if (!CostSplitValidation)
                                                {
                                                    CCInvalidSplitControlTempList = CCInvalidSplitControlList;
                                                    CCInvalidSplitControlTempList.Add(new CostCenterMissmatch { AMT_TC_CONTROL_ID = txtAmountTcCtrlId });
                                                    CCInvalidSplitControlList = CCInvalidSplitControlTempList;
                                                }

                                                objCocDtlList.ForEach(dtl =>
                                                {
                                                    if (dtl.FTD_AMT_BC > 0)
                                                        finTrxObj.FIN_TRX_COC_DTL.Add(dtl);
                                                });

                                            }
                                            #endregion
                                        }
                                    }
                                    break;
                                #endregion
                                #region Narration
                                case ControlCategory.Narration:
                                    TextBox txtNarr = (TextBox)divGroupCr.FindControl(pair.Key);//Get Narration Textbox
                                    if (txtNarr != null && !string.IsNullOrEmpty(txtNarr.Text.Trim()))
                                        finTrxObj.FTR_NARRATION = HttpUtility.HtmlEncode(txtNarr.Text.Trim());
                                    break;
                                #endregion
                                #region AmountTC
                                case ControlCategory.AmountTC:
                                    TextBox txtAmountTCDr = (TextBox)divGroupCr.FindControl(pair.Key);//Get Amount in TC Textbox Textbox
                                    if (txtAmountTCDr != null && !string.IsNullOrEmpty(txtAmountTCDr.Text.Trim())
                                        && Convert.ToDecimal(txtAmountTCDr.Text.Trim()) > 0)
                                    {
                                        txtAmountTCDr.Text = string.IsNullOrEmpty(txtAmountTCDr.Text.Trim()) ? "0" : txtAmountTCDr.Text.Trim();
                                        finTrxObj.FTR_CR_AMT_TC = Convert.ToDecimal(txtAmountTCDr.Text);
                                        finTrxObj.FTR_DR_AMT_TC = 0;
                                        finTrxObj.FTR_PK = 0;
                                    }
                                    else
                                    {
                                        finTrxObj.FTR_CR_AMT_TC = 0;
                                        finTrxObj.FTR_DR_AMT_TC = 0;
                                        finTrxObj.FTR_PK = 0;
                                    }
                                    break;
                                #endregion
                                #region ExchangeRate
                                case ControlCategory.ExchangeRate:
                                    double exchangeRate = 1;
                                    TextBox txtExchangeRate = (TextBox)divGroupCr.FindControl(pair.Key);//Get Exchange Rate Textbox
                                    if (txtExchangeRate != null && !string.IsNullOrEmpty(txtExchangeRate.Text.Trim())
                                        && Convert.ToDecimal(txtExchangeRate.Text.Trim()) > 0)
                                    {
                                        double.TryParse(txtExchangeRate.Text.Trim(), out exchangeRate);
                                        finTrxObj.FTR_EXCHG_RATE = exchangeRate;
                                    }
                                    else
                                    {
                                        finTrxObj.FTR_EXCHG_RATE = exchangeRate;
                                    }
                                    break;
                                #endregion
                                #region EntryMode
                                case ControlCategory.EntryMode:
                                    byte entryMode = 0;
                                    HiddenField hdfEntryMode = (HiddenField)divGroupCr.FindControl(pair.Key);//Get EntryMode Hiddenfield
                                    if (hdfEntryMode != null && !string.IsNullOrEmpty(hdfEntryMode.Value))
                                    {
                                        byte.TryParse(hdfEntryMode.Value, out entryMode);
                                        finTrxObj.FTR_ENTRY_MODE = entryMode;
                                    }
                                    else
                                    {
                                        finTrxObj.FTR_ENTRY_MODE = entryMode;
                                    }
                                    break;
                                #endregion
                                #region AmountBC
                                case ControlCategory.AmountBC:
                                    if (finTrxObj.FTR_ENTRY_MODE != (byte)VoucherEntryMode.GainOrLoss)
                                    {
                                        TextBox txtAmountBCCr = (TextBox)divGroupDr.FindControl(pair.Key);//Get Amount in BC Textbox
                                        if (txtAmountBCCr != null && !string.IsNullOrEmpty(txtAmountBCCr.Text.Trim()))
                                        {
                                            finTrxObj.FTR_CR_AMT_BC = Convert.ToDecimal(txtAmountBCCr.Text);
                                            finTrxObj.FTR_DR_AMT_BC = 0;
                                        }
                                        else
                                        {
                                            string tcId = pair.Key.Replace(pair.Key.Substring(7, 8), "06set106");
                                            TextBox txtTempAmountTCCR = (TextBox)pnlControls.FindControl(tcId);//Get Amount in TC Textbox
                                            if (txtTempAmountTCCR != null)
                                            {
                                                finTrxObj.FTR_CR_AMT_BC = Convert.ToDecimal(txtTempAmountTCCR.Text);
                                                finTrxObj.FTR_DR_AMT_BC = 0;
                                            }
                                        }
                                    }
                                    else//Entry mode is G/L
                                    {
                                        TextBox txtAmountBCCr = (TextBox)divGroupDr.FindControl(pair.Key);//Get Amount in BC Textbox
                                        if (txtAmountBCCr != null && !string.IsNullOrEmpty(txtAmountBCCr.Text.Trim())
                                            && Convert.ToDecimal(txtAmountBCCr.Text.Trim()) > 0)
                                        {
                                            txtAmountBCCr.Text = string.IsNullOrEmpty(txtAmountBCCr.Text.Trim()) ? "0" : txtAmountBCCr.Text.Trim();
                                            finTrxObj.FTR_CR_AMT_BC = Convert.ToDecimal(txtAmountBCCr.Text);
                                            finTrxObj.FTR_DR_AMT_BC = 0;
                                            finTrxObj.FTR_DR_AMT_TC = 0;
                                            finTrxObj.FTR_CR_AMT_TC = 0;
                                            finTrxObj.FTR_PK = 0;
                                        }

                                    }
                                    break;
                                #endregion
                                #region Delete
                                case ControlCategory.Delete:
                                    break;
                                #endregion
                                #region CostCenter
                                case ControlCategory.CostCenter:
                                    break;
                                #endregion
                                #region SubLedgerLabel
                                case ControlCategory.SubledgerLabel:
                                    break;
                                #endregion
                                #region SubLedger
                                case ControlCategory.SubLedger:
                                    break;
                                #endregion
                                #region InstrumentNo
                                case ControlCategory.InstrumentNo:
                                    break;
                                #endregion
                                #region Date
                                case ControlCategory.Date:
                                    break;
                                #endregion
                                #region FavourOf
                                case ControlCategory.FavourOf:
                                    break;
                                    #endregion
                            }
                        }
                        if (crControlsSubmit.Count() > 0)
                        {
                            //finTrxObj.FTR_SEQUENCE = count;
                            finTrxObj.FTR_SEQUENCE = crSequence++; ;
                            finTrxObj.FTR_TRX_HDR = JurCurrPK;
                            finTrxObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                            finTrxObj.FTR_MOD_DT = DateTime.Now;
                            finTrxObj.FTR_REMARKS = "";
                            finTrxObj.FTR_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            finTrxObj.FTR_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                            finTrxObj.FTR_CRTD_DT = DateTime.Now;
                            finTrxObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                            finTrxObj.FTR_MOD_DT = LastModifiedTime;
                            finTrxObj.FTR_DEPT = currentUser.CurrentDeptPK;
                            finTrxObj.FTR_BIZUNIT = currentUser.SBUID;
                            if (finTrxObj.FTR_DR_AMT_TC > -1 && finTrxObj.FTR_CR_AMT_TC > -1)
                            {
                                finTrxList.Add(finTrxObj);
                            }
                        }
                        #endregion

                        if (finTrxList != null && finTrxList.Count > 0)
                        {
                            finTrxList.ForEach(dtl => finTrxHdrObj.FIN_TRX.Add(dtl));
                        }
                        break;
                    #endregion
                    #region GainLoss
                    case ActionsEnum.GAINLOSS:
                        voucherGainLossObj = new VoucherGainLossHeader();
                        voucherGainLossObj.FTH_PK = JurCurrPK;
                        voucherGainLossObj.FTH_EXCHG_RATE = double.Parse(hdfExchangeRateJV.Value);
                        SetUIValuesToObject(ActionsEnum.GAINLOSSDETAILS);//Get Gain loss details
                        break;
                    #endregion
                    #region GainLossDetails
                    case ActionsEnum.GAINLOSSDETAILS:
                        VoucherGainLossDetails gainLossDetailsObj = new VoucherGainLossDetails();
                        #region Debit
                        dicDrControls = new Dictionary<string, string>();
                        if (Session[ERP.Utilities.SessionStrings.DrControls] != null)
                            dicDrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.DrControls];//Get Debit Controls
                        var drControlsGainLoss = from pair in dicDrControls
                                                 orderby pair.Key.Substring(0, 13) ascending//orderby pair.Key.Substring(0, 9) ascending
                                                 select pair;//Order debit controls in ascending order
                        drCount = 1;
                        count = 1;
                        removedControlsList = null;

                        if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)
                        {
                            removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];//Get Removed Control List
                        }
                        if (removedControlsList != null && removedControlsList.Count > 0)
                        {
                            //Get the first valid drCount value.
                            while (!string.IsNullOrEmpty(removedControlsList.FirstOrDefault(aa => aa.StartsWith("dic" + drCount.ToString("0000")) && aa.EndsWith("dr"))))
                            {
                                drCount++;
                            }
                        }

                        foreach (KeyValuePair<string, string> pair in drControlsGainLoss)
                        {
                            if (!pair.Key.Substring(7, 6).Equals("03set1") || !pair.Key.Substring(7, 6).Equals("04set1"))//Not Account aoutocomplete Hiddenfield or Account aoutocomplete Button
                            {
                                if (!drCount.ToString("0000").Equals(pair.Key.Substring(3, 4)))
                                {
                                    drCount++;
                                    if (removedControlsList != null && removedControlsList.Count > 0)
                                    {
                                        //Find Next valid drCount
                                        while (!string.IsNullOrEmpty(removedControlsList.FirstOrDefault(aa => aa.StartsWith("dic" + drCount.ToString("0000")) && aa.EndsWith("dr"))))
                                        {
                                            drCount++;
                                        }
                                    }
                                    if ((gainLossDetailsObj.FTR_DR_AMT_TC > -1 && gainLossDetailsObj.FTR_CR_AMT_TC > -1) ||
                                        (gainLossDetailsObj.FTR_DR_AMT_BC > -1 && gainLossDetailsObj.FTR_CR_AMT_BC > -1))
                                    {
                                        if (voucherGainLossObj.GainLossDetails == null)
                                        {
                                            voucherGainLossObj.GainLossDetails = new List<VoucherGainLossDetails>();
                                        }
                                        voucherGainLossObj.GainLossDetails.Add(gainLossDetailsObj);
                                        gainLossDetailsObj = new VoucherGainLossDetails();
                                    }
                                    count++;
                                }
                            }
                            switch ((ControlCategory)(Enum.Parse(typeof(ControlCategory), pair.Value)))//Get Data from each Debit control
                            {
                                #region SubType
                                case ControlCategory.SubType:
                                    dicFinTrxPK = null;
                                    if (Session[ERP.Utilities.SessionStrings.FinTrxPk] != null)
                                        dicFinTrxPK = (Dictionary<string, long>)Session[ERP.Utilities.SessionStrings.FinTrxPk];
                                    string key = pair.Key.Substring(0, 7) + "dr";
                                    if (dicFinTrxPK != null && dicFinTrxPK.Count > 0 && dicFinTrxPK.ContainsKey(key))
                                    {
                                        gainLossDetailsObj.FTR_PK = dicFinTrxPK.SingleOrDefault(aa => aa.Key == key).Value;
                                    }
                                    else
                                    {
                                        gainLossDetailsObj.FTR_PK = 0;
                                    }
                                    gainLossDetailsObj.FTR_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                    break;
                                #endregion
                                #region Account
                                case ControlCategory.Account:
                                    if (pair.Key.Substring(7, 6).Equals("02set1"))//Acount Textbox
                                    {
                                        hdfSubTypePk.Value = string.Empty;
                                        TextBox txtAccount = (TextBox)divGroupDr.FindControl(pair.Key);//Get Account Textbox
                                        string hdfCtrlId = pair.Key.Replace("02set102", "03set103");
                                        HiddenField hdfAccount = (HiddenField)divGroupDr.FindControl(hdfCtrlId);//Get Account Hiddenfield
                                        if (txtAccount != null && hdfAccount != null && !string.IsNullOrEmpty(hdfAccount.Value))
                                        {
                                            gainLossDetailsObj.FTR_ACCOUNT = Convert.ToInt16(hdfAccount.Value);
                                        }
                                    }
                                    break;
                                #endregion
                                #region Narration
                                case ControlCategory.Narration:
                                    break;
                                #endregion
                                #region AmountTC
                                case ControlCategory.AmountTC:
                                    TextBox txtAmountTCDr = (TextBox)divGroupDr.FindControl(pair.Key);//Get Amount in TC Textbox Textbox
                                    if (txtAmountTCDr != null && !string.IsNullOrEmpty(txtAmountTCDr.Text.Trim())
                                        && Convert.ToDecimal(txtAmountTCDr.Text.Trim()) > 0)
                                    {
                                        txtAmountTCDr.Text = string.IsNullOrEmpty(txtAmountTCDr.Text.Trim()) ? "0" : txtAmountTCDr.Text.Trim();
                                        gainLossDetailsObj.FTR_DR_AMT_TC = Convert.ToDecimal(txtAmountTCDr.Text);
                                        gainLossDetailsObj.FTR_CR_AMT_TC = 0;
                                    }
                                    break;
                                #endregion
                                #region ExchangeRate
                                case ControlCategory.ExchangeRate:
                                    double exchangeRate = 1;
                                    TextBox txtExchangeRate = (TextBox)divGroupDr.FindControl(pair.Key);//Get Exchange Rate Textbox
                                    if (txtExchangeRate != null && !string.IsNullOrEmpty(txtExchangeRate.Text.Trim())
                                        && Convert.ToDecimal(txtExchangeRate.Text.Trim()) > 0)
                                    {
                                        double.TryParse(txtExchangeRate.Text.Trim(), out exchangeRate);
                                        gainLossDetailsObj.FTR_EXCHG_RATE = exchangeRate;
                                    }
                                    else
                                    {
                                        gainLossDetailsObj.FTR_EXCHG_RATE = exchangeRate;
                                    }
                                    break;
                                #endregion
                                #region EntryMode
                                case ControlCategory.EntryMode:
                                    byte entryMode = 0;
                                    HiddenField hdfEntryMode = (HiddenField)divGroupDr.FindControl(pair.Key);//Get EntryMode Hiddenfield
                                    if (hdfEntryMode != null && !string.IsNullOrEmpty(hdfEntryMode.Value))
                                    {
                                        byte.TryParse(hdfEntryMode.Value, out entryMode);
                                        gainLossDetailsObj.FTR_ENTRY_MODE = entryMode;
                                    }
                                    else
                                    {
                                        gainLossDetailsObj.FTR_ENTRY_MODE = entryMode;
                                    }
                                    break;
                                #endregion
                                #region AmountBC
                                case ControlCategory.AmountBC:
                                    TextBox txtAmountBCDr = (TextBox)divGroupDr.FindControl(pair.Key);//Get Amount in BC Textbox
                                    if (txtAmountBCDr != null && !string.IsNullOrEmpty(txtAmountBCDr.Text.Trim()))
                                    {
                                        txtAmountBCDr.Text = string.IsNullOrEmpty(txtAmountBCDr.Text.Trim()) ? "0" : txtAmountBCDr.Text.Trim();
                                        gainLossDetailsObj.FTR_DR_AMT_BC = Convert.ToDecimal(txtAmountBCDr.Text);
                                        gainLossDetailsObj.FTR_CR_AMT_BC = 0;
                                    }
                                    break;
                                #endregion
                                #region Delete
                                case ControlCategory.Delete:
                                    break;
                                #endregion
                                #region Cost Center
                                case ControlCategory.CostCenter:
                                    break;
                                #endregion
                                #region SubLedgerLabel
                                case ControlCategory.SubledgerLabel:
                                    break;
                                #endregion
                                #region SubLedger
                                case ControlCategory.SubLedger:
                                    break;
                                #endregion
                                #region InstrumentNo
                                case ControlCategory.InstrumentNo:
                                    break;
                                #endregion
                                #region Date
                                case ControlCategory.Date:
                                    break;
                                #endregion
                                #region FavourOf
                                case ControlCategory.FavourOf:
                                    break;
                                    #endregion
                            }
                        }
                        if (drControlsGainLoss.Count() > 0)
                        {
                            if ((gainLossDetailsObj.FTR_DR_AMT_TC > -1 && gainLossDetailsObj.FTR_CR_AMT_TC > -1)
                                || (gainLossDetailsObj.FTR_DR_AMT_BC > -1 && gainLossDetailsObj.FTR_CR_AMT_BC > -1))
                            {
                                if (voucherGainLossObj.GainLossDetails == null)
                                {
                                    voucherGainLossObj.GainLossDetails = new List<VoucherGainLossDetails>();
                                }
                                voucherGainLossObj.GainLossDetails.Add(gainLossDetailsObj);
                            }
                        }
                        #endregion
                        #region credit
                        gainLossDetailsObj = new VoucherGainLossDetails();
                        dicCrControls = new Dictionary<string, string>();
                        if (Session[ERP.Utilities.SessionStrings.CrControls] != null)
                            dicCrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.CrControls];//Get Credit Controls
                        var crControlsGainLoss = from pair in dicCrControls
                                                 orderby pair.Key.Substring(0, 13) ascending//orderby pair.Key.Substring(0, 9) ascending
                                                 select pair;//Order credit controls in ascending order
                        crCount = 1;
                        removedControlsList = null;

                        if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)
                        {
                            removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];//Get Removed Control List
                        }
                        if (removedControlsList != null && removedControlsList.Count > 0)
                        {
                            //Get the first valid crCount value.
                            while (!string.IsNullOrEmpty(removedControlsList.FirstOrDefault(aa => aa.StartsWith("dic" + crCount.ToString("0000")) && aa.EndsWith("cr"))))
                            {
                                crCount++;
                            }
                        }

                        foreach (KeyValuePair<string, string> pair in crControlsGainLoss)
                        {
                            if (!pair.Key.Substring(7, 6).Equals("03set1") || !pair.Key.Substring(7, 6).Equals("04set1"))//Not Account aoutocomplete Hiddenfield or Account aoutocomplete Button
                            {
                                if (!crCount.ToString("0000").Equals(pair.Key.Substring(3, 4)))
                                {
                                    crCount++;
                                    if (removedControlsList != null && removedControlsList.Count > 0)
                                    {
                                        //Find Next valid crCount
                                        while (!string.IsNullOrEmpty(removedControlsList.FirstOrDefault(aa => aa.StartsWith("dic" + crCount.ToString("0000")) && aa.EndsWith("cr"))))
                                        {
                                            crCount++;
                                        }
                                    }

                                    if ((gainLossDetailsObj.FTR_DR_AMT_TC > -1 && gainLossDetailsObj.FTR_CR_AMT_TC > -1) ||
                                        (gainLossDetailsObj.FTR_DR_AMT_BC > -1 && gainLossDetailsObj.FTR_CR_AMT_BC > -1))
                                    {
                                        if (voucherGainLossObj.GainLossDetails == null)
                                        {
                                            voucherGainLossObj.GainLossDetails = new List<VoucherGainLossDetails>();
                                        }
                                        voucherGainLossObj.GainLossDetails.Add(gainLossDetailsObj);
                                        gainLossDetailsObj = new VoucherGainLossDetails();
                                    }
                                    count++;
                                }
                            }
                            switch ((ControlCategory)(Enum.Parse(typeof(ControlCategory), pair.Value)))//Get Data from each Debit control
                            {
                                #region SubType
                                case ControlCategory.SubType:
                                    dicFinTrxPK = null;
                                    if (Session[ERP.Utilities.SessionStrings.FinTrxPk] != null)
                                        dicFinTrxPK = (Dictionary<string, long>)Session[ERP.Utilities.SessionStrings.FinTrxPk];
                                    string key = pair.Key.Substring(0, 7) + "cr";
                                    if (dicFinTrxPK != null && dicFinTrxPK.Count > 0 && dicFinTrxPK.ContainsKey(key))
                                    {
                                        gainLossDetailsObj.FTR_PK = dicFinTrxPK.SingleOrDefault(aa => aa.Key == key).Value;
                                    }
                                    else
                                    {
                                        gainLossDetailsObj.FTR_PK = 0;
                                    }

                                    gainLossDetailsObj.FTR_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                    break;
                                #endregion
                                #region Account
                                case ControlCategory.Account:
                                    if (pair.Key.Substring(7, 6).Equals("02set1"))//Acount Textbox
                                    {
                                        hdfSubTypePk.Value = string.Empty;
                                        TextBox txtAccount = (TextBox)divGroupCr.FindControl(pair.Key);//Get Account Textbox
                                        string hdfCtrlId = pair.Key.Replace("02set102", "03set103");
                                        HiddenField hdfAccount = (HiddenField)divGroupCr.FindControl(hdfCtrlId);//Get Account Hiddenfield
                                        if (txtAccount != null && hdfAccount != null && !string.IsNullOrEmpty(hdfAccount.Value))
                                        {
                                            gainLossDetailsObj.FTR_ACCOUNT = Convert.ToInt16(hdfAccount.Value);
                                        }
                                    }
                                    break;
                                #endregion
                                #region Narration
                                case ControlCategory.Narration:
                                    break;
                                #endregion
                                #region AmountTC
                                case ControlCategory.AmountTC:
                                    TextBox txtAmountTCDr = (TextBox)divGroupCr.FindControl(pair.Key);//Get Amount in TC Textbox Textbox
                                    if (txtAmountTCDr != null && !string.IsNullOrEmpty(txtAmountTCDr.Text.Trim())
                                        && Convert.ToDecimal(txtAmountTCDr.Text.Trim()) > 0)
                                    {
                                        txtAmountTCDr.Text = string.IsNullOrEmpty(txtAmountTCDr.Text.Trim()) ? "0" : txtAmountTCDr.Text.Trim();
                                        gainLossDetailsObj.FTR_CR_AMT_TC = Convert.ToDecimal(txtAmountTCDr.Text);
                                        gainLossDetailsObj.FTR_DR_AMT_TC = 0;
                                    }
                                    break;
                                #endregion
                                #region ExchangeRate
                                case ControlCategory.ExchangeRate:
                                    double exchangeRate = 1;
                                    TextBox txtExchangeRate = (TextBox)divGroupCr.FindControl(pair.Key);//Get Exchange Rate Textbox
                                    if (txtExchangeRate != null && !string.IsNullOrEmpty(txtExchangeRate.Text.Trim())
                                        && Convert.ToDecimal(txtExchangeRate.Text.Trim()) > 0)
                                    {
                                        double.TryParse(txtExchangeRate.Text.Trim(), out exchangeRate);
                                        gainLossDetailsObj.FTR_EXCHG_RATE = exchangeRate;
                                    }
                                    else
                                    {
                                        gainLossDetailsObj.FTR_EXCHG_RATE = exchangeRate;
                                    }
                                    break;
                                #endregion
                                #region EntryMode
                                case ControlCategory.EntryMode:
                                    byte entryMode = 0;
                                    HiddenField hdfEntryMode = (HiddenField)divGroupCr.FindControl(pair.Key);//Get EntryMode Hiddenfield
                                    if (hdfEntryMode != null && !string.IsNullOrEmpty(hdfEntryMode.Value))
                                    {
                                        byte.TryParse(hdfEntryMode.Value, out entryMode);
                                        gainLossDetailsObj.FTR_ENTRY_MODE = entryMode;
                                    }
                                    else
                                    {
                                        gainLossDetailsObj.FTR_ENTRY_MODE = entryMode;
                                    }
                                    break;
                                #endregion
                                #region AmountBC
                                case ControlCategory.AmountBC:
                                    TextBox txtAmountBCCr = (TextBox)divGroupCr.FindControl(pair.Key);//Get Amount in BC Textbox
                                    if (txtAmountBCCr != null && !string.IsNullOrEmpty(txtAmountBCCr.Text.Trim()))
                                    {
                                        txtAmountBCCr.Text = string.IsNullOrEmpty(txtAmountBCCr.Text.Trim()) ? "0" : txtAmountBCCr.Text.Trim();
                                        gainLossDetailsObj.FTR_CR_AMT_BC = Convert.ToDecimal(txtAmountBCCr.Text);
                                        gainLossDetailsObj.FTR_DR_AMT_BC = 0;
                                    }
                                    break;
                                #endregion
                                #region Delete
                                case ControlCategory.Delete:
                                    break;
                                #endregion
                                #region Cost Center
                                case ControlCategory.CostCenter:
                                    break;
                                #endregion
                                #region SubLedgerLabel
                                case ControlCategory.SubledgerLabel:
                                    break;
                                #endregion
                                #region SubLedger
                                case ControlCategory.SubLedger:
                                    break;
                                #endregion
                                #region InstrumentNo
                                case ControlCategory.InstrumentNo:
                                    break;
                                #endregion
                                #region Date
                                case ControlCategory.Date:
                                    break;
                                #endregion
                                #region FavourOf
                                case ControlCategory.FavourOf:
                                    break;
                                    #endregion
                            }
                        }
                        if (crControlsGainLoss.Count() > 0)
                        {
                            gainLossDetailsObj.FTR_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            if ((gainLossDetailsObj.FTR_DR_AMT_TC > -1 && gainLossDetailsObj.FTR_CR_AMT_TC > -1)
                                || (gainLossDetailsObj.FTR_DR_AMT_BC > -1 && gainLossDetailsObj.FTR_CR_AMT_BC > -1))
                            {
                                if (voucherGainLossObj.GainLossDetails == null)
                                {
                                    voucherGainLossObj.GainLossDetails = new List<VoucherGainLossDetails>();
                                }
                                voucherGainLossObj.GainLossDetails.Add(gainLossDetailsObj);
                            }
                        }
                        #endregion
                        break;
                    #endregion
                    #region Template Header
                    case ActionsEnum.SAVEASTEMPLATE:
                        JVTemplateBOObj = new JVTemplateBO();
                        JVTemplateBOObj.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        JVTemplateBOObj.LAST_MOD_DT = LastModifiedTime.ToString();
                        JVTemplateBOObj.USER_PK = currentUser.PKUser;
                        SetUIValuesToObject(ActionsEnum.TEMPLATEDETAILS);
                        break;
                    #endregion
                    #region Template Details
                    case ActionsEnum.TEMPLATEDETAILS:
                        JVTemplateDetails jvTemplateDetailsObj = new JVTemplateDetails();
                        #region Debit
                        dicDrControls = new Dictionary<string, string>();
                        if (Session[ERP.Utilities.SessionStrings.DrControls] != null)
                            dicDrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.DrControls];//Get Debit Controls
                        var drControlsTemplate = from pair in dicDrControls
                                                 orderby pair.Key.Substring(0, 13) ascending//orderby pair.Key.Substring(0, 9) ascending
                                                 select pair;//Order debit controls in ascending order
                        drCount = 1;
                        count = 1;
                        removedControlsList = null;

                        if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)
                        {
                            removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];//Get Removed Control List
                        }
                        if (removedControlsList != null && removedControlsList.Count > 0)
                        {
                            //Get the first valid drCount value.
                            while (!string.IsNullOrEmpty(removedControlsList.FirstOrDefault(aa => aa.StartsWith("dic" + drCount.ToString("0000")) && aa.EndsWith("dr"))))
                            {
                                drCount++;
                            }
                        }
                        foreach (KeyValuePair<string, string> pair in drControlsTemplate)
                        {
                            if (!pair.Key.Substring(7, 6).Equals("03set1") || !pair.Key.Substring(7, 6).Equals("04set1"))//Not Account aoutocomplete Hiddenfield or Account aoutocomplete Button
                            {

                                if (!drCount.ToString("0000").Equals(pair.Key.Substring(3, 4)))
                                {
                                    drCount++;
                                    if (removedControlsList != null && removedControlsList.Count > 0)
                                    {
                                        //Find Next valid drCount
                                        while (!string.IsNullOrEmpty(removedControlsList.FirstOrDefault(aa => aa.StartsWith("dic" + drCount.ToString("0000")) && aa.EndsWith("dr"))))
                                        {
                                            drCount++;
                                        }
                                    }
                                    if (JVTemplateBOObj.Detail == null)
                                    {
                                        JVTemplateBOObj.Detail = new List<JVTemplateDetails>();
                                    }
                                    if (jvTemplateDetailsObj.VLD_ACCOUNT > 0)
                                        JVTemplateBOObj.Detail.Add(jvTemplateDetailsObj);
                                    jvTemplateDetailsObj = new JVTemplateDetails();
                                    count++;
                                }
                            }
                            switch ((ControlCategory)(Enum.Parse(typeof(ControlCategory), pair.Value)))//Get Data from each Debit control
                            {
                                #region SubType
                                case ControlCategory.SubType:
                                    Label lblSubType = (Label)divGroupDr.FindControl(pair.Key);//get Account Label
                                    if (lblSubType != null && !string.IsNullOrEmpty(lblSubType.Text.Trim()))
                                    {
                                        jvTemplateDetailsObj.VLD_REF_TYPE = lblSubType.Text.Trim();
                                    }
                                    break;
                                #endregion
                                #region Account
                                case ControlCategory.Account:
                                    if (pair.Key.Substring(7, 6).Equals("02set1"))//Acount Textbox
                                    {
                                        hdfSubTypePk.Value = string.Empty;
                                        TextBox txtAccount = (TextBox)divGroupDr.FindControl(pair.Key);//Get Account Textbox
                                        string hdfCtrlId = pair.Key.Replace("02set102", "03set103");
                                        HiddenField hdfAccount = (HiddenField)divGroupDr.FindControl(hdfCtrlId);//Get Account Hiddenfield
                                        if (txtAccount != null && hdfAccount != null && !string.IsNullOrEmpty(hdfAccount.Value))
                                        {
                                            jvTemplateDetailsObj.VLD_PK = 0;
                                            jvTemplateDetailsObj.VLD_ACCOUNT = !string.IsNullOrEmpty(hdfAccount.Value) ? Convert.ToInt16(hdfAccount.Value) : 0;
                                            jvTemplateDetailsObj.VLD_MODE = (int)TemplateMode.Debit;
                                            jvTemplateDetailsObj.VLD_SEQUENCE = 1;
                                        }
                                    }
                                    break;
                                #endregion
                                #region Narration
                                case ControlCategory.Narration:
                                    break;
                                #endregion
                                #region AmountTC
                                case ControlCategory.AmountTC:

                                    break;
                                #endregion
                                #region ExchangeRate
                                case ControlCategory.ExchangeRate:

                                    break;
                                #endregion
                                #region EntryMode
                                case ControlCategory.EntryMode:

                                    break;
                                #endregion
                                #region AmountBC
                                case ControlCategory.AmountBC:

                                    break;
                                #endregion
                                #region Delete
                                case ControlCategory.Delete:
                                    break;
                                #endregion
                                #region Cost Center
                                case ControlCategory.CostCenter:
                                    break;
                                #endregion
                                #region SubLedgerLabel
                                case ControlCategory.SubledgerLabel:
                                    break;
                                #endregion
                                #region SubLedger
                                case ControlCategory.SubLedger:
                                    DropDownList ddlSubLedger = (DropDownList)divGroupDr.FindControl(pair.Key);//Get Subledger DDL
                                    if (ddlSubLedger != null)
                                    {
                                        int selectedSubLedger = 0;
                                        if (int.TryParse(ddlSubLedger.SelectedValue, out selectedSubLedger))
                                        {
                                            if (selectedSubLedger > 0)
                                                jvTemplateDetailsObj.VLD_REF_TYPE_PK = selectedSubLedger;
                                        }
                                    }
                                    break;
                                #endregion
                                #region InstrumentNo
                                case ControlCategory.InstrumentNo:
                                    break;
                                #endregion
                                #region Date
                                case ControlCategory.Date:
                                    break;
                                #endregion
                                #region FavourOf
                                case ControlCategory.FavourOf:
                                    break;
                                    #endregion
                            }
                        }
                        if (drControlsTemplate.Count() > 0)
                        {
                            if (JVTemplateBOObj.Detail == null)
                            {
                                JVTemplateBOObj.Detail = new List<JVTemplateDetails>();
                            }
                            if (jvTemplateDetailsObj.VLD_ACCOUNT > 0)
                                JVTemplateBOObj.Detail.Add(jvTemplateDetailsObj);
                        }
                        #endregion
                        #region credit
                        jvTemplateDetailsObj = new JVTemplateDetails();
                        dicCrControls = new Dictionary<string, string>();
                        if (Session[ERP.Utilities.SessionStrings.CrControls] != null)
                            dicCrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.CrControls];//Get Credit Controls
                        var crControlsTemplate = from pair in dicCrControls
                                                 orderby pair.Key.Substring(0, 13) ascending//orderby pair.Key.Substring(0, 9) ascending
                                                 select pair;//Order credit controls in ascending order
                        crCount = 1;
                        removedControlsList = null;

                        if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)
                        {
                            removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];//Get Removed Control List
                        }
                        if (removedControlsList != null && removedControlsList.Count > 0)
                        {
                            //Get the first valid crCount value.
                            while (!string.IsNullOrEmpty(removedControlsList.FirstOrDefault(aa => aa.StartsWith("dic" + crCount.ToString("0000")) && aa.EndsWith("cr"))))
                            {
                                crCount++;
                            }
                        }
                        foreach (KeyValuePair<string, string> pair in crControlsTemplate)
                        {
                            if (!pair.Key.Substring(7, 6).Equals("03set1") || !pair.Key.Substring(7, 6).Equals("04set1"))//Not Account aoutocomplete Hiddenfield or Account aoutocomplete Button
                            {
                                if (!crCount.ToString("0000").Equals(pair.Key.Substring(3, 4)))
                                {
                                    crCount++;
                                    if (removedControlsList != null && removedControlsList.Count > 0)
                                    {
                                        //Find Next valid drCount
                                        while (!string.IsNullOrEmpty(removedControlsList.FirstOrDefault(aa => aa.StartsWith("dic" + crCount.ToString("0000")) && aa.EndsWith("cr"))))
                                        {
                                            crCount++;
                                        }
                                    }

                                    if (JVTemplateBOObj.Detail == null)
                                    {
                                        JVTemplateBOObj.Detail = new List<JVTemplateDetails>();
                                    }
                                    if (jvTemplateDetailsObj.VLD_ACCOUNT > 0)
                                        JVTemplateBOObj.Detail.Add(jvTemplateDetailsObj);
                                    jvTemplateDetailsObj = new JVTemplateDetails();
                                    count++;
                                }
                            }
                            switch ((ControlCategory)(Enum.Parse(typeof(ControlCategory), pair.Value)))//Get Data from each Credit control
                            {
                                #region SubType
                                case ControlCategory.SubType:
                                    Label lblSubType = (Label)divGroupDr.FindControl(pair.Key);//get Account Label
                                    if (lblSubType != null && !string.IsNullOrEmpty(lblSubType.Text.Trim()))
                                    {
                                        jvTemplateDetailsObj.VLD_REF_TYPE = lblSubType.Text.Trim();
                                    }
                                    break;
                                #endregion
                                #region Account
                                case ControlCategory.Account:
                                    if (pair.Key.Substring(7, 6).Equals("02set1"))//Acount Textbox
                                    {
                                        hdfSubTypePk.Value = string.Empty;
                                        TextBox txtAccount = (TextBox)divGroupCr.FindControl(pair.Key);//Get Account Textbox
                                        string hdfCtrlId = pair.Key.Replace("02set102", "03set103");
                                        HiddenField hdfAccount = (HiddenField)divGroupCr.FindControl(hdfCtrlId);//Get Account Hiddenfield
                                        if (txtAccount != null && hdfAccount != null && !string.IsNullOrEmpty(hdfAccount.Value))
                                        {
                                            jvTemplateDetailsObj.VLD_PK = 0;
                                            jvTemplateDetailsObj.VLD_ACCOUNT = !string.IsNullOrEmpty(hdfAccount.Value) ? Convert.ToInt16(hdfAccount.Value) : 0;
                                            jvTemplateDetailsObj.VLD_MODE = (int)TemplateMode.Credit;
                                            jvTemplateDetailsObj.VLD_SEQUENCE = 1;
                                        }
                                    }
                                    break;
                                #endregion
                                #region Narration
                                case ControlCategory.Narration:
                                    break;
                                #endregion
                                #region AmountTC
                                case ControlCategory.AmountTC:
                                    break;
                                #endregion
                                #region ExchangeRate
                                case ControlCategory.ExchangeRate:
                                    break;
                                #endregion
                                #region EntryMode
                                case ControlCategory.EntryMode:
                                    break;
                                #endregion
                                #region AmountBC
                                case ControlCategory.AmountBC:
                                    break;
                                #endregion
                                #region Delete
                                case ControlCategory.Delete:
                                    break;
                                #endregion
                                #region Cost Center
                                case ControlCategory.CostCenter:
                                    break;
                                #endregion
                                #region SubLedgerLabel
                                case ControlCategory.SubledgerLabel:
                                    break;
                                #endregion
                                #region SubLedger
                                case ControlCategory.SubLedger:
                                    DropDownList ddlSubLedger = (DropDownList)divGroupDr.FindControl(pair.Key);//Get Subledger DDL
                                    if (ddlSubLedger != null)
                                    {
                                        int selectedSubLedger = 0;
                                        if (int.TryParse(ddlSubLedger.SelectedValue, out selectedSubLedger))
                                        {
                                            if (selectedSubLedger > 0)
                                                jvTemplateDetailsObj.VLD_REF_TYPE_PK = selectedSubLedger;
                                        }
                                    }
                                    break;
                                #endregion
                                #region InstrumentNo
                                case ControlCategory.InstrumentNo:
                                    break;
                                #endregion
                                #region Date
                                case ControlCategory.Date:
                                    break;
                                #endregion
                                #region FavourOf
                                case ControlCategory.FavourOf:
                                    break;
                                    #endregion
                            }
                        }
                        if (crControlsTemplate.Count() > 0)
                        {
                            if (JVTemplateBOObj.Detail == null)
                            {
                                JVTemplateBOObj.Detail = new List<JVTemplateDetails>();
                            }
                            if (jvTemplateDetailsObj.VLD_ACCOUNT > 0)
                                JVTemplateBOObj.Detail.Add(jvTemplateDetailsObj);
                        }
                        #endregion
                        break;
                    #endregion

                    default:
                        break;
                }
                if (mode == ActionsEnum.GAINLOSS)
                {
                    returnObj = voucherGainLossObj;
                }
                else if (mode == ActionsEnum.SAVEASTEMPLATE)
                {
                    returnObj = JVTemplateBOObj;
                }
                else
                {
                    returnObj = finTrxHdrObj;
                }
                return returnObj;
            }
            catch
            {
                throw;
            }
            finally
            {
                commonServiceClient = null;
            }
        }

        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            if (Session[ERP.Utilities.SessionStrings.TransactionType] != null)
            {
                CommonService commonServiceObj;
                commonServiceObj = new CommonService();
                AppTypeDetailsList = commonServiceObj.GetReportParameters(Session[ERP.Utilities.SessionStrings.TransactionType].ToString(), 0, DateTime.Now);
                if (AppTypeDetailsList.Count > 0)
                {
                    return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
                }
                else
                {
                    return "0";
                }
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// Bind the dynamic controls 
        /// </summary>
        /// <returns></returns>  
        private void BindControls()
        {
            //CostCenterList = null;
            //CostCenterTempList = null;
            int Cols;
            string divColStyle = "";

            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            int exchRateDecimalDigits = (Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
            Cols = 1;
            if (Cols == 1)
            {
                divColStyle = "divcolmiddle-S";
            }
            else
            {
                divColStyle = "div2col-M";
            }
            Table tbControls;
            TableRow trControls;
            TableCell tcControl;

            #region Debit Captions
            SetDebitHdr();
            #endregion

            #region Credit Captions
            SetCreditHdr();
            #endregion

            bool hasDR = false;
            bool hasCR = false;
            bool hasDRCR = false;

            int drCount = 1;
            int crCount = 1;
            HtmlGenericControl div;
            HtmlGenericControl divValidation;
            HtmlGenericControl divClear;
            string query;
            query = string.Empty;
            string relquery;
            relquery = string.Empty;

            CommonService commonService;
            commonService = null;
            Dictionary<string, string> dicDrControls;
            dicDrControls = new Dictionary<string, string>();
            Dictionary<string, string> dicCrControls;
            dicCrControls = new Dictionary<string, string>();
            Dictionary<string, string> dicAccountType;
            dicAccountType = new Dictionary<string, string>();
            Dictionary<string, string> dicControlinfo;
            Dictionary<string, long> dicFinTrxPK;
            dicFinTrxPK = new Dictionary<string, long>();

            if (Session[ERP.Utilities.SessionStrings.ControlInfo] != null)
                dicControlinfo = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.ControlInfo];
            else
                dicControlinfo = new Dictionary<string, string>();

            #region drcontrols

            if (Session[ERP.Utilities.SessionStrings.DrControls] != null)
            {
                hasDR = true;//Has Debit Controls
                dicDrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.DrControls];//Get Debit Controls
                if (dicDrControls != null && dicDrControls.Count > 0)
                {
                    var drControls = from pair in dicDrControls
                                     orderby pair.Key.Substring(0, 13) ascending //orderby pair.Key.Substring(0, 9) ascending
                                     select pair;//Order debit controls in ascending order
                    drCount = 1;
                    divColStyle = "divcolmiddle-S1 input-margin2";
                    div = new HtmlGenericControl("div");//This div will hold the drcontrols that we are going to create.
                    div.Attributes.Add("class", divColStyle);//set the style for the div
                    divValidation = new HtmlGenericControl("div");//This div will hold all the validation controls that we are going to create.
                    divValidation.Attributes.Add("class", "starwrap");//set the style for the div
                    tbControls = new Table();
                    tbControls.CssClass = "";
                    trControls = new TableRow();
                    tcControl = new TableCell();
                    TextBox txtAccount;
                    txtAccount = null;
                    HiddenField hdfAccount;
                    hdfAccount = null;
                    Label lblSubledgerLabel;
                    lblSubledgerLabel = null;
                    TextBox txtAmountTC;
                    TextBox txtExchangeRate;
                    txtExchangeRate = null;
                    byte entryMode;
                    entryMode = 0;
                    txtAmountTC = null;

                    Button btnCostCenter;
                    btnCostCenter = null;
                    HiddenField hdfIsCostCenter;
                    hdfIsCostCenter = null;

                    if (Session[ERP.Utilities.SessionStrings.FinTrxPk] != null)
                        dicFinTrxPK = (Dictionary<string, long>)Session[ERP.Utilities.SessionStrings.FinTrxPk];
                    else
                        dicFinTrxPK = null;
                    List<string> removedControlsList;
                    removedControlsList = null;
                    Label txtSubType = null;
                    commonService = new CommonService();
                    commonService = CommonFunctions.InitiateClient(commonService);
                    if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)
                    {
                        removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];//Get Removed Control List
                    }
                    if (removedControlsList != null && removedControlsList.Count > 0)
                    {
                        //Get the first valid drCount value.
                        while (!string.IsNullOrEmpty(removedControlsList.FirstOrDefault(aa => aa.StartsWith("dic" + drCount.ToString("0000")) && aa.EndsWith("dr"))))
                        {
                            drCount++;
                        }
                    }
                    foreach (KeyValuePair<string, string> pair in drControls)
                    {
                        if (!pair.Key.Substring(7, 6).Equals("03set1") || !pair.Key.Substring(7, 6).Equals("04set1"))//Not Account aoutocomplete Hiddenfield or Account aoutocomplete Button
                        {
                            if (!drCount.ToString("0000").Equals(pair.Key.Substring(3, 4)))
                            {
                                tcControl.Controls.Add(div);
                                trControls.Cells.Add(tcControl);
                                tbControls.Rows.Add(trControls);
                                divGroupDr.Controls.Add(tbControls);
                                drCount++;
                                if (removedControlsList != null && removedControlsList.Count > 0)
                                {
                                    //Find Next valid drCount
                                    while (!string.IsNullOrEmpty(removedControlsList.FirstOrDefault(aa => aa.StartsWith("dic" + drCount.ToString("0000")) && aa.EndsWith("dr"))))
                                    {
                                        //If drcount is in removed controls list den increment drcount by 1
                                        drCount++;
                                    }
                                }
                                div = new HtmlGenericControl("div");
                                div.Attributes.Add("class", divColStyle);
                                divValidation = new HtmlGenericControl("div");
                                divValidation.Attributes.Add("class", "starwrap");
                                tbControls = new Table();
                                tbControls.CssClass = "";
                                trControls = new TableRow();
                                tcControl = new TableCell();
                            }
                        }
                        switch ((ControlCategory)(Enum.Parse(typeof(ControlCategory), pair.Value)))//Get Data from each Debit control
                        {
                            #region SubType
                            case ControlCategory.SubType:
                                //SubType Label
                                txtSubType = new Label()
                                {
                                    ClientIDMode = ClientIDMode.Static
                                };
                                txtSubType.ID = pair.Key;
                                if (Session[ERP.Utilities.SessionStrings.AccountType] != null)
                                {
                                    dicAccountType = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.AccountType];
                                    if (dicAccountType != null && dicAccountType.Count > 0)
                                    {
                                        txtSubType.Text = dicAccountType.ContainsKey(txtSubType.ID) ? dicAccountType.AsEnumerable().SingleOrDefault(aa => aa.Key == txtSubType.ID).Value : string.Empty;
                                    }
                                }
                                div.Controls.Add(txtSubType);//Add to Active dr Controls List
                                break;
                            #endregion
                            #region Account
                            case ControlCategory.Account:
                                //It is an autocomplte control with postback. So we need Textbox,Hiddenfield and a Button.
                                if (pair.Key.Substring(7, 6).Equals("02set1"))//Acount Textbox
                                {
                                    txtAccount = new TextBox()
                                    {
                                        MaxLength = 100,
                                        ClientIDMode = ClientIDMode.Static,
                                    };
                                    txtAccount.ID = pair.Key;
                                    hdfSubTypePk.Value = dicControlinfo.ContainsKey(txtAccount.ID + "SubTypePk") ? dicControlinfo.SingleOrDefault(aa => aa.Key == txtAccount.ID + "SubTypePk").Value : "";
                                    div.Controls.Add(txtAccount);
                                    if (txtSubType != null)
                                    {
                                        txtSubType.AssociatedControlID = txtAccount.ID;
                                    }
                                    RequiredFieldValidator vrfAccount = new RequiredFieldValidator()
                                    {
                                        ID = "vrf" + txtAccount.ID.Replace("dic", ""),
                                        ControlToValidate = txtAccount.ID,
                                        Text = "*",
                                        CssClass = "star",
                                        ValidationGroup = "voucher",
                                        Display = ValidatorDisplay.Dynamic,
                                        EnableClientScript = true,
                                        InitialValue = Resources.Messages.AutoDefaultValue,
                                        SetFocusOnError = true,
                                        ErrorMessage = GetLocalResourceObject("Err_Account").ToString(),
                                        ClientIDMode = ClientIDMode.Static
                                    };
                                    divValidation.Controls.Add(vrfAccount);
                                }
                                else if (pair.Key.Substring(7, 6).Equals("03set1"))//Account Hiddenfield
                                {
                                    hdfAccount = new HiddenField
                                    {
                                        ClientIDMode = ClientIDMode.Static
                                    };
                                    hdfAccount.ID = pair.Key;
                                    div.Controls.Add(hdfAccount);
                                }
                                else if (pair.Key.Substring(7, 6).Equals("04set1"))//Account Button
                                {
                                    Button btnAccount = new Button
                                    {
                                        ClientIDMode = ClientIDMode.Static,
                                        CommandName = "JOURNALACCOUNTINDEXCHANGED",
                                        EnableTheming = false
                                    };
                                    btnAccount.ID = pair.Key;
                                    btnAccount.Attributes.Add("style", "display:none;");
                                    btnAccount.Click += new EventHandler(ActionHandler);
                                    div.Controls.Add(btnAccount);
                                    if (!string.IsNullOrEmpty(hdfSubTypePk.Value) && txtAccount != null && hdfAccount != null)
                                    {
                                        //Register Autocomplete script
                                        journalScript = journalScript + "GrandScriptUtils.MakeAutoCompleteDDL('" + txtAccount.ID
                                            + "', (url1.indexOf('?') != -1 ? url1+'&' :  url1+'?') + 'AccType="
                                            + hdfSubTypePk.Value + "', '" + hdfAccount.ID + "', true, true, 'JOURNALACCOUNT',false,false,false);";

                                    }
                                }
                                break;
                            #endregion
                            #region Narration
                            case ControlCategory.Narration:
                                TextBox txtNarration = new TextBox()//Narration Textbox Control
                                {
                                    MaxLength = 400,
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    Width = IsSameCurrency() == true ? new Unit(GetLocalResourceObject("NarationWidthWithoutAmountBC").ToString()) : new Unit(GetLocalResourceObject("NarationWidthNormal").ToString())
                                };
                                txtNarration.ID = pair.Key;
                                div.Controls.Add(txtNarration);
                                break;
                            #endregion
                            #region AmountTC
                            case ControlCategory.AmountTC:
                                //Amount in Transaction Currency
                                txtAmountTC = new TextBox()
                                {
                                    MaxLength = 15,
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    CssClass = "input-w8per numeric amounttcdr tempcss"
                                };
                                txtAmountTC.ID = pair.Key;
                                div.Controls.Add(txtAmountTC);
                                AmountValidation vamAmountTC = new AmountValidation()
                                {
                                    ID = "vam" + txtAmountTC.ID.Replace("dic", ""),
                                    ControlToValidate = txtAmountTC.ID,
                                    ErrorMessage = GetLocalResourceObject("MsgErr_AmountTc").ToString(),
                                    NumberDigits = 11,
                                    Display = ValidatorDisplay.Dynamic,
                                    Text = "*",
                                    EnableClientScript = true,
                                    CssClass = "star",
                                    ValidationGroup = "voucher",
                                    NonZero = true
                                };
                                divValidation.Controls.Add(vamAmountTC);
                                break;
                            #endregion
                            #region ExchangeRate
                            case ControlCategory.ExchangeRate:
                                txtExchangeRate = new TextBox()
                                {
                                    MaxLength = 9,
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    CssClass = "numeric input-w6per"
                                };
                                txtExchangeRate.ID = pair.Key;

                                HiddenField hdfEntryMode = new HiddenField()
                                {
                                    ClientIDMode = ClientIDMode.Static
                                };
                                hdfEntryMode.ID = txtExchangeRate.ID + "EntryMode";

                                if (dicControlinfo.ContainsKey(txtExchangeRate.ID + "EntryMode"))
                                {
                                    entryMode = Convert.ToByte(dicControlinfo.SingleOrDefault(aa => aa.Key == txtExchangeRate.ID + "EntryMode").Value);
                                    if (entryMode == (byte)VoucherEntryMode.NonEditable)//Non Editable exchange Rate
                                    {
                                        if (GainLossCalculationMode == (byte)GLCalculationMode.Actual)
                                        {
                                            txtExchangeRate.CssClass = "numeric input-w6per input-normalb";
                                            txtExchangeRate.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                                            txtExchangeRate.Attributes.Add("onpaste", "return false;");
                                        }
                                        else
                                        {
                                            txtExchangeRate.Attributes.Add("onkeyup", "CalculateBCWithER(this);");//script registration for Calculate BC
                                            if (!string.IsNullOrEmpty(txtExchangeRate.ID))
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtExchangeRate.ID + "", "$('[id$=" + txtExchangeRate.ID + "]').ForceNumericOnly();", true);

                                        }
                                        txtAmountTC.Attributes.Add("onkeyup", "CalculateBCAmt(this);");//script registration for Calculate BC
                                        if (!string.IsNullOrEmpty(txtAmountTC.ID))
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtAmountTC.ID + "", "$('[id$=" + txtAmountTC.ID + "]').ForceNumericOnly();", true);

                                    }
                                    else if (entryMode == (byte)VoucherEntryMode.GainOrLoss)
                                    {
                                        txtExchangeRate.CssClass = "numeric input-w6per input-normalb";
                                        txtAmountTC.CssClass = "input-w8per numeric input-normalb amounttcdr tempcss";
                                        txtExchangeRate.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                                        txtExchangeRate.Attributes.Add("onpaste", "return false;");
                                        txtAmountTC.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                                        txtAmountTC.Attributes.Add("onpaste", "return false;");
                                    }
                                    else//Editable
                                    {
                                        txtAmountTC.Attributes.Add("onkeyup", "CalculateBCAmt(this);");//script registration for Calculate BC
                                        if (!string.IsNullOrEmpty(txtExchangeRate.ID))
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtExchangeRate.ID + "", "$('[id$=" + txtExchangeRate.ID + "]').ForceNumericOnly();", true);
                                        txtExchangeRate.Attributes.Add("onkeyup", "CalculateBCWithER(this);");//script registration for Calculate BC
                                        if (!string.IsNullOrEmpty(txtAmountTC.ID))
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtAmountTC.ID + "", "$('[id$=" + txtAmountTC.ID + "]').ForceNumericOnly();", true);

                                    }
                                }

                                transactionCurrency = 0;
                                if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                                    transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                                else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                                    transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);

                                if (transactionCurrency > 0 && transactionCurrency == currentUser.BaseCurrency)
                                    txtExchangeRate.Visible = false;
                                else
                                    txtExchangeRate.Visible = true;

                                div.Controls.Add(txtExchangeRate);
                                div.Controls.Add(hdfEntryMode);
                                ExchangeRateValidation vreExchangeRate = new ExchangeRateValidation()
                                {
                                    ID = "vre" + txtExchangeRate.ID,
                                    ControlToValidate = txtExchangeRate.ID,
                                    ErrorMessage = GetLocalResourceObject("MsgErr_ExchangeRate").ToString(),
                                    NumberDigits = 5,
                                    Display = ValidatorDisplay.Dynamic,
                                    Text = "*",
                                    EnableClientScript = true,
                                    CssClass = "star",
                                    ValidationGroup = "voucher",
                                    NonZero = true
                                };
                                divValidation.Controls.Add(vreExchangeRate);
                                break;
                            #endregion
                            #region EntryMode
                            case ControlCategory.EntryMode:

                                break;
                            #endregion
                            #region AmountBC
                            case ControlCategory.AmountBC:
                                TextBox txtAmountBC = new TextBox()
                                {
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    MaxLength = 15,
                                    CssClass = "input-normalb input-w8per numeric tempcss"
                                };

                                txtAmountBC.ID = pair.Key;
                                if ((BCEnable && entryMode != (byte)VoucherEntryMode.GainOrLoss) || IsYearEndVoucher() || IsEditableForApplication())
                                {
                                    txtAmountBC.CssClass = "input-w8per numeric tempcss";
                                    txtAmountBC.Attributes.Add("onkeyup", "CalculateTCAmt(this,event);");//script registration for Calculate TC
                                    if (!string.IsNullOrEmpty(txtAmountBC.ID))
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtAmountBC.ID + "", "$('[id$=" + txtAmountBC.ID + "]').ForceNumericOnly();", true);
                                }
                                else
                                {
                                    if (dicControlinfo.ContainsKey(txtAmountBC.ID + "GL"))
                                        if ((dicControlinfo.SingleOrDefault(aa => aa.Key == txtAmountBC.ID + "GL").Value) == GainLossMode.Gain + "")
                                            txtAmountBC.CssClass = "input-bgGreen input-normalb input-w8per numeric tempcss";
                                        else
                                            txtAmountBC.CssClass = "input-bgRed input-normalb input-w8per numeric tempcss";
                                    txtAmountBC.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                                    txtAmountBC.Attributes.Add("onpaste", "return false;");
                                }
                                div.Controls.Add(txtAmountBC);
                                AmountValidation vamAmountBC = new AmountValidation()
                                {
                                    ID = "vam" + txtAmountBC.ID.Replace("dic", ""),
                                    ControlToValidate = txtAmountBC.ID,
                                    ErrorMessage = GetLocalResourceObject("MsgErr_AmountBc").ToString(),
                                    NumberDigits = 11,
                                    Display = ValidatorDisplay.Dynamic,
                                    Text = "*",
                                    EnableClientScript = true,
                                    CssClass = "star",
                                    ValidationGroup = "voucher"
                                };
                                divValidation.Controls.Add(vamAmountBC);
                                transactionCurrency = 0;
                                if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                                    transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                                else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                                    transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);

                                if (transactionCurrency > 0 && transactionCurrency == currentUser.BaseCurrency)
                                    txtAmountBC.Visible = false;
                                else
                                    txtAmountBC.Visible = true;

                                break;
                            #endregion
                            #region Delete
                            case ControlCategory.Delete:
                                Button btnDelete = new Button()
                                {
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    SkinID = "delete-icon",
                                    ToolTip = "Delete",
                                    OnClientClick = "return ShowDeleteConfirm(this);"
                                };
                                btnDelete.ID = pair.Key;
                                btnDelete.CommandName = ActionsEnum.REMOVEDEBIT.ToString();
                                btnDelete.Click += new EventHandler(ActionHandler);
                                if (EntryStatus == EntryStatus.VIEWMODE)
                                {
                                    btnDelete.Visible = false;
                                }

                                div.Controls.Add(btnDelete);
                                ////divValidation contains all the Validation Controls in a group. It will be added to the group after delete button. So if any validation catch it will show * after delete button 
                                //div.Controls.Add(divValidation);
                                break;
                            #endregion
                            #region Cost Center
                            case ControlCategory.CostCenter:
                                if (pair.Key.Substring(7, 6).Equals("10set1"))//Cost center button
                                {
                                    btnCostCenter = new Button()
                                    {
                                        Text = "",
                                        ClientIDMode = ClientIDMode.Static,
                                        Visible = false,
                                        Enabled = true,
                                        SkinID = "costcenter-icon",
                                        ToolTip = GetLocalResourceObject("CCButtonToolTip").ToString()
                                    };
                                    btnCostCenter.ID = pair.Key;
                                    btnCostCenter.CommandName = ActionsEnum.COSTCENTER.ToString();
                                    btnCostCenter.Click += new EventHandler(ActionHandler);
                                    div.Controls.Add(btnCostCenter);
                                    //divValidation contains all the Validation Controls in a group. It will be added to the group after delete button. So if any validation catch it will show * after delete button 
                                    div.Controls.Add(divValidation);
                                }
                                else if (pair.Key.Substring(7, 6).Equals("11set1"))//Cost center Hiddenfield
                                {
                                    hdfIsCostCenter = new HiddenField
                                    {
                                        ClientIDMode = ClientIDMode.Static,
                                        Value = "0"
                                    };
                                    hdfIsCostCenter.ID = pair.Key;
                                    div.Controls.Add(hdfIsCostCenter);
                                }

                                break;
                            #endregion
                            #region SubLedgerLabel
                            case ControlCategory.SubledgerLabel:
                                divClear = new HtmlGenericControl("div");
                                divClear.Attributes.Add("class", "clear");
                                div.Controls.Add(divClear);
                                lblSubledgerLabel = new Label()
                                {
                                    ClientIDMode = ClientIDMode.Static,
                                    Visible = false,
                                    Text = "&nbsp"
                                };

                                lblSubledgerLabel.ID = pair.Key;
                                div.Controls.Add(lblSubledgerLabel);
                                break;
                            #endregion
                            #region SubLedger
                            case ControlCategory.SubLedger:
                                DropDownList ddlSubLedger = new DropDownList()
                                {
                                    ClientIDMode = ClientIDMode.Static,
                                    Visible = false
                                };

                                ddlSubLedger.ID = pair.Key;
                                //Get Account PK
                                hdfCoaPk.Value = dicControlinfo.ContainsKey(txtAccount.ID + "CoaPk") ? dicControlinfo.SingleOrDefault(aa => aa.Key == txtAccount.ID + "CoaPk").Value : "";
                                GetFieldValues(ControlsEnum.FINCOAMST);
                                finCoaSubTypeCfgList = null;
                                if (finCoaMstList != null && finCoaMstList.Count > 0)
                                {
                                    hdfSubTypePk.Value = finCoaMstList[0].COA_SUB_TYPE.ToString();
                                    if (!string.IsNullOrEmpty(hdfSubTypePk.Value))
                                    {
                                        GetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                                        if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                                        {
                                            relquery = finCoaSubTypeCfgList[0].CST_REL_QUERY == null ? string.Empty : finCoaSubTypeCfgList[0].ADM_QUERIES_CFG1.QRY_QUERY;
                                        }

                                        if (relquery != string.Empty)//Have related qry
                                        {
                                            if (finCoaSubTypeCfgList[0].CST_CODE == "AP" || finCoaSubTypeCfgList[0].CST_CODE == "AR")
                                            {
                                                relquery = relquery.Replace("@COA@", hdfCoaPk.Value == "" ? "NULL" : hdfCoaPk.Value);
                                                relquery = relquery.Replace("@ADV@", "NULL");
                                            }
                                            else if (finCoaSubTypeCfgList[0].CST_CODE == "ADP" || finCoaSubTypeCfgList[0].CST_CODE == "ADR")
                                            {
                                                relquery = relquery.Replace("@COA@", "NULL");
                                                relquery = relquery.Replace("@ADV@", hdfCoaPk.Value == "" ? "NULL" : hdfCoaPk.Value);
                                            }
                                            else
                                            {
                                                relquery = relquery.Replace("@COA@", "NULL");
                                                relquery = relquery.Replace("@ADV@", "NULL");
                                            }
                                            relquery = relquery.Replace("@PK@", "0");
                                            relquery = relquery.Replace("@BIZUNITPK@", currentUser.SBUID.ToString());
                                            List<DDLMaster> ddlChildValues = commonService.ExecuteQuery(relquery);
                                            ddlSubLedger.DataTextField = "Value";
                                            ddlSubLedger.DataValueField = "PK";
                                            ddlSubLedger.DataSource = CommonFunctions.HtmlDecode(ddlChildValues, "Value");
                                            ddlSubLedger.DataBind();//Bind subledger ddl
                                            ddlSubLedger.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                                            ddlSubLedger.Visible = true;
                                            lblSubledgerLabel.Visible = true;
                                        }
                                    }

                                    #region Set cost center allocation button visibility
                                    if ((!string.IsNullOrEmpty(hdfCoaPk.Value)
                                        && finCoaMstList[0].FIN_COA_COST_CENTER_MPG != null
                                        && finCoaMstList[0].FIN_COA_COST_CENTER_MPG.Where(r => r.ADM_COST_CENTER_MST.CNM_ACTIVE == 1).Count() > 0)
                                        || (CostCenterList != null && CostCenterList.Where(r => r.CONTROL_ID == pair.Key.Replace("51set202", "03set103")).Count() > 0))
                                    {
                                        hdfIsCostCenter.Value = "1";
                                        btnCostCenter.Visible = true;
                                    }
                                    else
                                    {
                                        hdfIsCostCenter.Value = "0";
                                        btnCostCenter.Visible = false;
                                    }
                                    #endregion
                                }

                                div.Controls.Add(ddlSubLedger);
                                RequiredFieldValidator vrfSubLedger = new RequiredFieldValidator()
                                {
                                    ID = "vrf" + ddlSubLedger.ID.Replace("dic", ""),
                                    ControlToValidate = ddlSubLedger.ID,
                                    Text = "*",
                                    CssClass = "star",
                                    ValidationGroup = "voucher",
                                    Display = ValidatorDisplay.Dynamic,
                                    EnableClientScript = true,
                                    InitialValue = CommonConstants.SELECTVAL,
                                    SetFocusOnError = true,
                                    ErrorMessage = GetLocalResourceObject("Err_SubLedger").ToString(),
                                    ClientIDMode = ClientIDMode.Static
                                };
                                divValidation.Controls.Add(vrfSubLedger);
                                vrfSubLedger.Enabled = ddlSubLedger.Visible ? true : false;
                                if (lblSubledgerLabel != null)
                                    lblSubledgerLabel.AssociatedControlID = ddlSubLedger.ID;
                                break;
                            #endregion
                            #region InstrumentNo
                            case ControlCategory.InstrumentNo:
                                TextBox txtInstrumentNo = new TextBox()
                                {
                                    MaxLength = 70,
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    Visible = false,
                                    CssClass = "input16"
                                };

                                txtInstrumentNo.ID = pair.Key;
                                string instrumentNo = GetLocalResourceObject("InstrumentNo").ToString();
                                txtInstrumentNo.Attributes.Remove("onblur");
                                txtInstrumentNo.Attributes.Remove("onfocus");
                                //Set "Instrument No" if their is no text in the textbox with a watermark style. Otherwise remove this style
                                txtInstrumentNo.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + instrumentNo + "';$(this).addClass('input-watermark');}");
                                txtInstrumentNo.Attributes.Add("onfocus", "if (this.value == '" + instrumentNo + "') {this.value = '';$(this).removeClass('input-watermark');}");
                                journalScript = journalScript + "if ($('#" + txtInstrumentNo.ID + "').val() == '" + instrumentNo + "') {$('#" + txtInstrumentNo.ID + "').addClass('input-watermark');}";

                                if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                                {
                                    if (finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("Bank").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PDC").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PPC").ToString()))
                                    {
                                        txtInstrumentNo.Visible = true;
                                    }
                                }
                                div.Controls.Add(txtInstrumentNo);
                                break;
                            #endregion
                            #region Date
                            //Date Picker Control
                            case ControlCategory.Date:
                                TextBox txtDate = new TextBox()
                                {
                                    MaxLength = 11,
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    Visible = false,
                                    CssClass = "input-w8-5per"
                                };

                                txtDate.ID = pair.Key;
                                if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                                {
                                    if (finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("Bank").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PDC").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PPC").ToString()))
                                    {
                                        txtDate.Visible = true;
                                    }
                                }
                                string date = GetLocalResourceObject("Date").ToString();
                                //Set "Date" if their is no text in the textbox with a watermark style. Otherwise remove this style
                                txtDate.Attributes.Remove("onblur");
                                txtDate.Attributes.Remove("onfocus");
                                txtDate.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + date + "';$(this).addClass('input-watermark');}");
                                txtDate.Attributes.Add("onfocus", "if (this.value == '" + date + "') {this.value = '';$(this).removeClass('input-watermark');}");
                                journalScript = journalScript + "if ($('#" + txtDate.ID + "').val() == '" + date + "') {$('#" + txtDate.ID + "').addClass('input-watermark');}";
                                txtDate.Attributes.Add("onkeydown", "return CheckKey(event);");
                                txtDate.Attributes.Add("onpaste", "return false;");

                                if (txtDate.Visible == true)
                                {
                                    if (!string.IsNullOrEmpty(txtDate.ID))
                                    {
                                        string PageScript = CommonFunctions.GenerateDynamicScript("Date", txtDate.ID, null, null, null);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtDate.ID + "", PageScript, true);
                                    }
                                }
                                div.Controls.Add(txtDate);
                                break;
                            #endregion
                            #region FavourOf
                            //Favourof Textbox
                            case ControlCategory.FavourOf:
                                TextBox txtFavourOf = new TextBox()
                                {
                                    MaxLength = 150,
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    Visible = false,
                                    CssClass = "input-w23-6per"
                                };

                                txtFavourOf.ID = pair.Key;
                                if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                                {
                                    if (finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("Bank").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PDC").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PPC").ToString()))
                                    {
                                        txtFavourOf.Visible = true;
                                    }
                                }
                                string favourOf = GetLocalResourceObject("FavourOf").ToString();
                                //Set "FavourOf" if their is no text in the textbox with a watermark style. Otherwise remove this style
                                txtFavourOf.Attributes.Remove("onblur");
                                txtFavourOf.Attributes.Remove("onfocus");
                                txtFavourOf.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + favourOf + "';$(this).addClass('input-watermark');}");
                                txtFavourOf.Attributes.Add("onfocus", "if (this.value == '" + favourOf + "') {this.value = '';$(this).removeClass('input-watermark');}");
                                journalScript = journalScript + "if ($('#" + txtFavourOf.ID + "').val() == '" + favourOf + "') {$('#" + txtFavourOf.ID + "').addClass('input-watermark');}";
                                div.Controls.Add(txtFavourOf);
                                break;
                                #endregion

                        }
                    }

                    if (drControls.Count() > 0)
                    {
                        tcControl.Controls.Add(div);//Adding to Table Cell
                        trControls.Cells.Add(tcControl);//Adding to Table Row
                        tbControls.Rows.Add(trControls);//Adding to Table
                        divGroupDr.Controls.Add(tbControls);//Adding to Masater Div that holds all the Debit Groups
                    }
                }
            }
            #endregion
            #region crcontrols
            if (Session[ERP.Utilities.SessionStrings.CrControls] != null)
            {
                hasCR = true;//Has Credit Controls
                dicCrControls = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.CrControls];//Get Credit Controls
                if (dicCrControls != null && dicCrControls.Count > 0)
                {
                    var crControls = from pair in dicCrControls
                                     orderby pair.Key.Substring(0, 13) ascending // orderby pair.Key.Substring(0, 9) ascending
                                     select pair;//Order Credit controls
                    crCount = 1;
                    divColStyle = "divcolmiddle-S1 input-margin2";
                    div = new HtmlGenericControl("div");//This div will hold the crcontrols that we are going to create.
                    div.Attributes.Add("class", divColStyle);//set the style for the div
                    divValidation = new HtmlGenericControl("div");//This div will hold all the validation controls that we are going to create.
                    divValidation.Attributes.Add("class", "starwrap");//set the style for the div
                    tbControls = new Table();
                    tbControls.CssClass = "";
                    trControls = new TableRow();
                    tcControl = new TableCell();
                    TextBox txtAccount;
                    txtAccount = null;
                    HiddenField hdfAccount;
                    hdfAccount = null;
                    Label lblSubledgerLabel;
                    lblSubledgerLabel = null;
                    TextBox txtAmountTC;
                    txtAmountTC = null;
                    TextBox txtExchangeRate;
                    txtExchangeRate = null;
                    byte entryMode;
                    entryMode = 0;
                    Label txtSubType = null;
                    Button btnCostCenter;
                    btnCostCenter = null;
                    HiddenField hdfIsCostCenter;
                    hdfIsCostCenter = null;
                    if (Session[ERP.Utilities.SessionStrings.FinTrxPk] != null)
                        dicFinTrxPK = (Dictionary<string, long>)Session[ERP.Utilities.SessionStrings.FinTrxPk];
                    else
                        dicFinTrxPK = null;
                    List<string> removedControlsList;
                    removedControlsList = null;
                    commonService = new CommonService();
                    commonService = CommonFunctions.InitiateClient(commonService);
                    if (Session[ERP.Utilities.SessionStrings.RemovedControls] != null)
                    {
                        removedControlsList = (List<string>)Session[ERP.Utilities.SessionStrings.RemovedControls];//get Removed Controls List
                    }
                    if (removedControlsList != null && removedControlsList.Count > 0)
                    {
                        while (!string.IsNullOrEmpty(removedControlsList.FirstOrDefault(aa => aa.StartsWith("dic" + crCount.ToString("0000")) && aa.EndsWith("cr"))))
                        {
                            //If crcount is in removed controls list den increment drcount by 1
                            crCount++;
                        }
                    }
                    foreach (KeyValuePair<string, string> pair in crControls)
                    {
                        if (!pair.Key.Substring(7, 6).Equals("03set1") || !pair.Key.Substring(7, 6).Equals("04set1"))//Not Account aoutocomplete Hiddenfield or Account aoutocomplete Button
                        {
                            if (!crCount.ToString("0000").Equals(pair.Key.Substring(3, 4)))
                            {
                                tcControl.Controls.Add(div);
                                trControls.Cells.Add(tcControl);
                                tbControls.Rows.Add(trControls);
                                divGroupCr.Controls.Add(tbControls);
                                crCount++;
                                if (removedControlsList != null && removedControlsList.Count > 0)
                                {
                                    while (!string.IsNullOrEmpty(removedControlsList.FirstOrDefault(aa => aa.StartsWith("dic" + crCount.ToString("0000")) && aa.EndsWith("cr"))))
                                    {
                                        //If crcount is in removed controls list den increment crcount by 1
                                        crCount++;
                                    }
                                }
                                div = new HtmlGenericControl("div");
                                div.Attributes.Add("class", divColStyle);
                                divValidation = new HtmlGenericControl("div");
                                divValidation.Attributes.Add("class", "starwrap");
                                tbControls = new Table();
                                tbControls.CssClass = "";
                                trControls = new TableRow();
                                tcControl = new TableCell();
                            }
                        }
                        switch ((ControlCategory)(Enum.Parse(typeof(ControlCategory), pair.Value)))
                        {

                            #region SubType
                            //Account Type
                            case ControlCategory.SubType:
                                txtSubType = new Label()
                                {
                                    ClientIDMode = ClientIDMode.Static
                                };
                                txtSubType.ID = pair.Key;
                                if (Session[ERP.Utilities.SessionStrings.AccountType] != null)
                                {
                                    dicAccountType = (Dictionary<string, string>)Session[ERP.Utilities.SessionStrings.AccountType];
                                    if (dicAccountType != null && dicAccountType.Count > 0)
                                    {
                                        txtSubType.Text = dicAccountType.ContainsKey(txtSubType.ID) ? dicAccountType.AsEnumerable().SingleOrDefault(aa => aa.Key == txtSubType.ID).Value : string.Empty;
                                    }
                                }
                                div.Controls.Add(txtSubType);
                                break;
                            #endregion
                            #region Account
                            case ControlCategory.Account:
                                if (pair.Key.Substring(7, 6).Equals("02set1"))//Account Textbox
                                {
                                    txtAccount = new TextBox()
                                    {
                                        MaxLength = 100,
                                        ClientIDMode = ClientIDMode.Static,
                                    };
                                    txtAccount.ID = pair.Key;
                                    //Get Account PK
                                    hdfSubTypePk.Value = dicControlinfo.ContainsKey(txtAccount.ID + "SubTypePk") ? dicControlinfo.SingleOrDefault(aa => aa.Key == txtAccount.ID + "SubTypePk").Value : "";
                                    div.Controls.Add(txtAccount);
                                    if (txtSubType != null)
                                    {
                                        txtSubType.AssociatedControlID = txtAccount.ID;
                                    }

                                    RequiredFieldValidator vrfAccount = new RequiredFieldValidator()
                                    {
                                        ID = "vrf" + txtAccount.ID.Replace("dic", ""),
                                        ControlToValidate = txtAccount.ID,
                                        Text = "*",
                                        CssClass = "star",
                                        ValidationGroup = "voucher",
                                        Display = ValidatorDisplay.Dynamic,
                                        EnableClientScript = true,
                                        InitialValue = Resources.Messages.AutoDefaultValue,
                                        SetFocusOnError = true,
                                        ErrorMessage = GetLocalResourceObject("Err_Account").ToString(),
                                        ClientIDMode = ClientIDMode.Static
                                    };
                                    divValidation.Controls.Add(vrfAccount);

                                }
                                else if (pair.Key.Substring(7, 6).Equals("03set1"))//Account HiddenField
                                {
                                    hdfAccount = new HiddenField
                                    {
                                        ClientIDMode = ClientIDMode.Static
                                    };
                                    hdfAccount.ID = pair.Key;
                                    div.Controls.Add(hdfAccount);
                                }
                                else if (pair.Key.Substring(7, 6).Equals("04set1"))//Account Button
                                {
                                    Button btnAccount = new Button
                                    {
                                        ClientIDMode = ClientIDMode.Static,
                                        CommandName = "JOURNALACCOUNTINDEXCHANGED",
                                        EnableTheming = false
                                    };
                                    btnAccount.ID = pair.Key;
                                    btnAccount.Attributes.Add("style", "display:none;");
                                    btnAccount.Click += new EventHandler(ActionHandler);
                                    div.Controls.Add(btnAccount);
                                    if (!string.IsNullOrEmpty(hdfSubTypePk.Value) && txtAccount != null && hdfAccount != null)
                                    {
                                        //Register Autocomplete
                                        journalScript = journalScript + "GrandScriptUtils.MakeAutoCompleteDDL('" + txtAccount.ID + "', (url1.indexOf('?') != -1 ? url1+'&' :  url1+'?') + 'AccType=" + hdfSubTypePk.Value + "', '" + hdfAccount.ID + "', true, true, 'JOURNALACCOUNT',false,false,false);";
                                    }
                                }
                                break;
                            #endregion
                            #region Narration
                            case ControlCategory.Narration:
                                TextBox txtNarration = new TextBox()
                                {
                                    MaxLength = 400,
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    Width = IsSameCurrency() == true ? new Unit(GetLocalResourceObject("NarationWidthWithoutAmountBC").ToString()) : new Unit(GetLocalResourceObject("NarationWidthNormal").ToString())
                                };

                                txtNarration.ID = pair.Key;
                                div.Controls.Add(txtNarration);
                                break;
                            #endregion
                            #region AmountTC
                            case ControlCategory.AmountTC:
                                //Amount in Transaction Currency
                                txtAmountTC = new TextBox()
                                {
                                    MaxLength = 15,
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    CssClass = "input-w8per numeric amounttccr tempcss"
                                };
                                txtAmountTC.ID = pair.Key;

                                div.Controls.Add(txtAmountTC);
                                AmountValidation vamAmountTC = new AmountValidation()
                                {
                                    ID = "vam" + txtAmountTC.ID.Replace("dic", ""),
                                    ControlToValidate = txtAmountTC.ID,
                                    ErrorMessage = GetLocalResourceObject("MsgErr_AmountTc").ToString(),
                                    NumberDigits = 11,
                                    Display = ValidatorDisplay.Dynamic,
                                    Text = "*",
                                    EnableClientScript = true,
                                    CssClass = "star",
                                    ValidationGroup = "voucher",
                                    NonZero = true
                                };
                                divValidation.Controls.Add(vamAmountTC);
                                break;
                            #endregion
                            #region ExchangeRate
                            case ControlCategory.ExchangeRate:
                                txtExchangeRate = new TextBox()
                                {
                                    MaxLength = 9,
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    CssClass = "numeric input-w6per"
                                };
                                txtExchangeRate.ID = pair.Key;
                                HiddenField hdfEntryMode = new HiddenField()
                                {
                                    ClientIDMode = ClientIDMode.Static
                                };
                                hdfEntryMode.ID = txtExchangeRate.ID + "EntryMode";//Used for to keep the entrymode of ExchangeRate in the credit detail section
                                if (dicControlinfo.ContainsKey(txtExchangeRate.ID + "EntryMode"))
                                {
                                    entryMode = Convert.ToByte(dicControlinfo.SingleOrDefault(aa => aa.Key == txtExchangeRate.ID + "EntryMode").Value);
                                    if (entryMode == (byte)VoucherEntryMode.NonEditable)
                                    {
                                        if (GainLossCalculationMode == (byte)GLCalculationMode.Actual)
                                        {
                                            txtExchangeRate.CssClass = "numeric input-w6per input-normalb";
                                            txtExchangeRate.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                                            txtExchangeRate.Attributes.Add("onpaste", "return false;");
                                        }
                                        else
                                        {
                                            txtExchangeRate.Attributes.Add("onkeyup", "CalculateBCWithER(this);");//Register script for calculate the BC with ExchangeRate
                                            if (!string.IsNullOrEmpty(txtExchangeRate.ID))
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtExchangeRate.ID + "", "$('[id$=" + txtExchangeRate.ID + "]').ForceNumericOnly();", true);

                                        }
                                        txtAmountTC.Attributes.Add("onkeyup", "CalculateBCAmt(this);");//Register script for calculate the BC

                                        if (!string.IsNullOrEmpty(txtAmountTC.ID))
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtAmountTC.ID + "", "$('[id$=" + txtAmountTC.ID + "]').ForceNumericOnly();", true);

                                    }
                                    else if (entryMode == (byte)VoucherEntryMode.GainOrLoss)
                                    {
                                        txtExchangeRate.CssClass = "numeric input-w6per input-normalb";
                                        txtAmountTC.CssClass = "input-w8per numeric input-normalb amounttccr tempcss";
                                        txtExchangeRate.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                                        txtExchangeRate.Attributes.Add("onpaste", "return false;");
                                        txtAmountTC.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                                        txtAmountTC.Attributes.Add("onpaste", "return false;");
                                    }
                                    else//Editable
                                    {

                                        txtAmountTC.Attributes.Add("onkeyup", "CalculateBCAmt(this);");//Register script for calculate the BC
                                        if (!string.IsNullOrEmpty(txtExchangeRate.ID))
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtExchangeRate.ID + "", "$('[id$=" + txtExchangeRate.ID + "]').ForceNumericOnly();", true);
                                        txtExchangeRate.Attributes.Add("onkeyup", "CalculateBCWithER(this);");//Register script for calculate the BC with ExchangeRate
                                        if (!string.IsNullOrEmpty(txtAmountTC.ID))
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtAmountTC.ID + "", "$('[id$=" + txtAmountTC.ID + "]').ForceNumericOnly();", true);

                                    }
                                }
                                transactionCurrency = 0;
                                if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                                    transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                                else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                                    transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);

                                if (transactionCurrency > 0 && transactionCurrency == currentUser.BaseCurrency)
                                    txtExchangeRate.Visible = false;
                                else
                                    txtExchangeRate.Visible = true;

                                div.Controls.Add(txtExchangeRate);
                                div.Controls.Add(hdfEntryMode);
                                ExchangeRateValidation vreExchangeRate = new ExchangeRateValidation()
                                {
                                    ID = "vre" + txtExchangeRate.ID,
                                    ControlToValidate = txtExchangeRate.ID,
                                    ErrorMessage = GetLocalResourceObject("MsgErr_ExchangeRate").ToString(),
                                    NumberDigits = 5,
                                    Display = ValidatorDisplay.Dynamic,
                                    Text = "*",
                                    EnableClientScript = true,
                                    CssClass = "star",
                                    ValidationGroup = "voucher",
                                    NonZero = true
                                };
                                divValidation.Controls.Add(vreExchangeRate);
                                break;
                            #endregion
                            #region EntryMode
                            case ControlCategory.EntryMode:
                                break;
                            #endregion
                            #region AmountBC
                            //Amount in Base Currency
                            case ControlCategory.AmountBC:
                                TextBox txtAmountBC = new TextBox()
                                {
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    MaxLength = 15,
                                    CssClass = "input-normalb input-w8per numeric tempcss"
                                };

                                txtAmountBC.ID = pair.Key;

                                if ((BCEnable && entryMode != (byte)VoucherEntryMode.GainOrLoss) || IsYearEndVoucher() || IsEditableForApplication())//If BC is Enable
                                {
                                    txtAmountBC.CssClass = "input-w8per numeric tempcss";
                                    txtAmountBC.Attributes.Add("onkeyup", "CalculateTCAmt(this,event);");//Register script for calculate the TC
                                    if (!string.IsNullOrEmpty(txtAmountBC.ID))
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtAmountBC.ID + "", "$('[id$=" + txtAmountBC.ID + "]').ForceNumericOnly();", true);
                                }
                                else
                                {
                                    if (dicControlinfo.ContainsKey(txtAmountBC.ID + "GL"))
                                        if ((dicControlinfo.SingleOrDefault(aa => aa.Key == txtAmountBC.ID + "GL").Value) == GainLossMode.Gain + "")
                                            txtAmountBC.CssClass = "input-bgGreen input-normalb input-w8per numeric tempcss";
                                        else
                                            txtAmountBC.CssClass = "input-bgRed input-normalb input-w8per numeric tempcss";
                                    txtAmountBC.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                                    txtAmountBC.Attributes.Add("onpaste", "return false;");
                                }

                                div.Controls.Add(txtAmountBC);
                                AmountValidation vamAmountBC = new AmountValidation()
                                {
                                    ID = "vam" + txtAmountBC.ID.Replace("dic", ""),
                                    ControlToValidate = txtAmountBC.ID,
                                    ErrorMessage = GetLocalResourceObject("MsgErr_AmountBc").ToString(),
                                    NumberDigits = 11,
                                    Display = ValidatorDisplay.Dynamic,
                                    Text = "*",
                                    EnableClientScript = true,
                                    CssClass = "star",
                                    ValidationGroup = "voucher"
                                };
                                divValidation.Controls.Add(vamAmountBC);
                                transactionCurrency = 0;
                                if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                                    transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                                else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                                    transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);

                                if (transactionCurrency > 0 && transactionCurrency == currentUser.BaseCurrency)
                                    txtAmountBC.Visible = false;
                                else
                                    txtAmountBC.Visible = true;
                                break;
                            #endregion
                            #region Delete
                            case ControlCategory.Delete:
                                Button btnDelete = new Button()
                                {
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    SkinID = "delete-icon",
                                    ToolTip = "Delete",
                                    OnClientClick = "return ShowDeleteConfirm(this);"
                                };
                                btnDelete.ID = pair.Key;
                                btnDelete.CommandName = ActionsEnum.REMOVECREDIT.ToString();
                                btnDelete.Click += new EventHandler(ActionHandler);
                                if (EntryStatus == EntryStatus.VIEWMODE)
                                {
                                    btnDelete.Visible = false;
                                }
                                div.Controls.Add(btnDelete);
                                ////divValidation contains all the Validation Controls in a group. It will be added to the group after delete button. So if any validation catch it will show * after delete button
                                //div.Controls.Add(divValidation);
                                break;
                            #endregion
                            #region Cost Center
                            case ControlCategory.CostCenter:
                                if (pair.Key.Substring(7, 6).Equals("10set1"))//Cost center button
                                {
                                    btnCostCenter = new Button()
                                    {
                                        Text = "",
                                        ClientIDMode = ClientIDMode.Static,
                                        Visible = false,
                                        Enabled = true,
                                        SkinID = "costcenter-icon",
                                        ToolTip = GetLocalResourceObject("CCButtonToolTip").ToString()
                                    };
                                    btnCostCenter.ID = pair.Key;
                                    btnCostCenter.CommandName = ActionsEnum.COSTCENTER.ToString();
                                    btnCostCenter.Click += new EventHandler(ActionHandler);
                                    div.Controls.Add(btnCostCenter);
                                    //divValidation contains all the Validation Controls in a group. It will be added to the group after delete button. So if any validation catch it will show * after delete button
                                    div.Controls.Add(divValidation);
                                }
                                else if (pair.Key.Substring(7, 6).Equals("11set1"))//Cost center HiddenField
                                {
                                    hdfIsCostCenter = new HiddenField
                                    {
                                        ClientIDMode = ClientIDMode.Static,
                                        Value = "0"
                                    };
                                    hdfIsCostCenter.ID = pair.Key;
                                    div.Controls.Add(hdfIsCostCenter);
                                }
                                break;
                            #endregion
                            #region SubLedgerLabel
                            case ControlCategory.SubledgerLabel:
                                //After Delete Button, the rest of the controls will comes in the next line. So the divClear will use to break the first line
                                divClear = new HtmlGenericControl("div");
                                divClear.Attributes.Add("class", "clear");
                                div.Controls.Add(divClear);
                                //It is a dummy label
                                lblSubledgerLabel = new Label()
                                {
                                    ClientIDMode = ClientIDMode.Static,
                                    Visible = false,
                                    Text = "&nbsp"
                                };
                                lblSubledgerLabel.ID = pair.Key;
                                div.Controls.Add(lblSubledgerLabel);
                                break;
                            #endregion
                            #region SubLedger
                            case ControlCategory.SubLedger:
                                DropDownList ddlSubLedger = new DropDownList()
                                {
                                    ClientIDMode = ClientIDMode.Static,
                                    Visible = false
                                };

                                ddlSubLedger.ID = pair.Key;
                                finCoaSubTypeCfgList = null;
                                //Get Account PK                              
                                hdfCoaPk.Value = dicControlinfo.ContainsKey(txtAccount.ID + "CoaPk") ? dicControlinfo.SingleOrDefault(aa => aa.Key == txtAccount.ID + "CoaPk").Value : "";
                                GetFieldValues(ControlsEnum.FINCOAMST);

                                if (finCoaMstList != null && finCoaMstList.Count > 0)
                                {
                                    hdfSubTypePk.Value = finCoaMstList[0].COA_SUB_TYPE.ToString();
                                    if (!string.IsNullOrEmpty(hdfSubTypePk.Value))
                                    {
                                        GetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                                        if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                                        {
                                            relquery = finCoaSubTypeCfgList[0].CST_REL_QUERY == null ? string.Empty : finCoaSubTypeCfgList[0].ADM_QUERIES_CFG1.QRY_QUERY;
                                        }

                                        if (relquery != string.Empty)//have related qry
                                        {
                                            if (finCoaSubTypeCfgList[0].CST_CODE == "AP" || finCoaSubTypeCfgList[0].CST_CODE == "AR")
                                            {
                                                relquery = relquery.Replace("@COA@", hdfCoaPk.Value == "" ? "NULL" : hdfCoaPk.Value);
                                                relquery = relquery.Replace("@ADV@", "NULL");
                                            }
                                            else if (finCoaSubTypeCfgList[0].CST_CODE == "ADP" || finCoaSubTypeCfgList[0].CST_CODE == "ADR")
                                            {
                                                relquery = relquery.Replace("@COA@", "NULL");
                                                relquery = relquery.Replace("@ADV@", hdfCoaPk.Value == "" ? "NULL" : hdfCoaPk.Value);
                                            }
                                            else
                                            {
                                                relquery = relquery.Replace("@COA@", "NULL");
                                                relquery = relquery.Replace("@ADV@", "NULL");
                                            }
                                            relquery = relquery.Replace("@PK@", "0");
                                            relquery = relquery.Replace("@BIZUNITPK@", currentUser.SBUID.ToString());
                                            List<DDLMaster> ddlChildValues = commonService.ExecuteQuery(relquery);
                                            ddlSubLedger.DataTextField = "Value";
                                            ddlSubLedger.DataValueField = "PK";
                                            ddlSubLedger.DataSource = CommonFunctions.HtmlDecode(ddlChildValues, "Value");
                                            ddlSubLedger.DataBind();//bind subledger ddl
                                            ddlSubLedger.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                                            ddlSubLedger.Visible = true;
                                            lblSubledgerLabel.Visible = true;
                                        }
                                    }
                                    #region Set cost center allocation button visibility
                                    if ((!string.IsNullOrEmpty(hdfCoaPk.Value)
                                        && finCoaMstList[0].FIN_COA_COST_CENTER_MPG != null
                                        && finCoaMstList[0].FIN_COA_COST_CENTER_MPG.Where(r => r.ADM_COST_CENTER_MST.CNM_ACTIVE == 1).Count() > 0)
                                        || (CostCenterList != null && CostCenterList.Where(r => r.CONTROL_ID == pair.Key.Replace("51set202", "03set103")).Count() > 0))
                                    {
                                        hdfIsCostCenter.Value = "1";
                                        btnCostCenter.Visible = true;
                                    }
                                    else
                                    {
                                        hdfIsCostCenter.Value = "0";
                                        btnCostCenter.Visible = false;
                                    }
                                    #endregion
                                }
                                div.Controls.Add(ddlSubLedger);
                                RequiredFieldValidator vrfSubLedger = new RequiredFieldValidator()
                                {
                                    ID = "vrf" + ddlSubLedger.ID.Replace("dic", ""),
                                    ControlToValidate = ddlSubLedger.ID,
                                    Text = "*",
                                    CssClass = "star",
                                    ValidationGroup = "voucher",
                                    Display = ValidatorDisplay.Dynamic,
                                    EnableClientScript = true,
                                    InitialValue = CommonConstants.SELECTVAL,
                                    SetFocusOnError = true,
                                    ErrorMessage = GetLocalResourceObject("Err_SubLedger").ToString(),
                                    ClientIDMode = ClientIDMode.Static
                                };
                                divValidation.Controls.Add(vrfSubLedger);
                                vrfSubLedger.Enabled = ddlSubLedger.Visible ? true : false;
                                if (lblSubledgerLabel != null)
                                    lblSubledgerLabel.AssociatedControlID = ddlSubLedger.ID;
                                break;
                            #endregion
                            #region InstrumentNo
                            case ControlCategory.InstrumentNo:
                                TextBox txtInstrumentNo = new TextBox()
                                {
                                    MaxLength = 70,
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    Visible = false,
                                    CssClass = "input16"
                                };

                                txtInstrumentNo.ID = pair.Key;
                                if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                                {
                                    if (finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("Bank").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PDC").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PPC").ToString()))
                                    {
                                        txtInstrumentNo.Visible = true;
                                    }
                                }

                                string instrumentNo = GetLocalResourceObject("InstrumentNo").ToString();
                                //Set "Instrument No" if their is no text in the textbox with a watermark style. Otherwise remove this style
                                txtInstrumentNo.Attributes.Remove("onblur");
                                txtInstrumentNo.Attributes.Remove("onfocus");
                                txtInstrumentNo.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + instrumentNo + "';$(this).addClass('input-watermark');}");
                                txtInstrumentNo.Attributes.Add("onfocus", "if (this.value == '" + instrumentNo + "') {this.value = '';$(this).removeClass('input-watermark');}");
                                journalScript = journalScript + "if ($('#" + txtInstrumentNo.ID + "').val() == '" + instrumentNo + "') {$('#" + txtInstrumentNo.ID + "').addClass('input-watermark');}";
                                div.Controls.Add(txtInstrumentNo);
                                break;
                            #endregion
                            #region Date
                            case ControlCategory.Date:
                                TextBox txtDate = new TextBox()
                                {
                                    MaxLength = 11,
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    Visible = false,
                                    CssClass = "input-w8-5per"
                                };

                                txtDate.ID = pair.Key;
                                if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                                {
                                    if (finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("Bank").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PDC").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PPC").ToString()))
                                    {
                                        txtDate.Visible = true;
                                    }
                                }
                                txtDate.Attributes.Add("onkeydown", "return CheckKey(event);");
                                txtDate.Attributes.Add("onpaste", "return false;");

                                string date = GetLocalResourceObject("Date").ToString();
                                //Set "Date" if their is no text in the textbox with a watermark style. Otherwise remove this style
                                txtDate.Attributes.Remove("onblur");
                                txtDate.Attributes.Remove("onfocus");
                                txtDate.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + date + "';$(this).addClass('input-watermark');}");
                                txtDate.Attributes.Add("onfocus", "if (this.value == '" + date + "') {this.value = '';$(this).removeClass('input-watermark');}");
                                journalScript = journalScript + "if ($('#" + txtDate.ID + "').val() == '" + date + "') {$('#" + txtDate.ID + "').addClass('input-watermark');}";

                                if (txtDate.Visible == true)
                                {
                                    if (!string.IsNullOrEmpty(txtDate.ID))
                                    {
                                        string PageScript = CommonFunctions.GenerateDynamicScript("Date", txtDate.ID, null, null, null);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtDate.ID + "", PageScript, true);
                                    }
                                }
                                div.Controls.Add(txtDate);
                                break;
                            #endregion
                            #region FavourOf
                            case ControlCategory.FavourOf:
                                TextBox txtFavourOf = new TextBox()
                                {
                                    MaxLength = 150,
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    Visible = false,
                                    CssClass = "input-w23-6per"
                                };

                                txtFavourOf.ID = pair.Key;
                                if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                                {
                                    if (finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("Bank").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PDC").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PPC").ToString()))
                                    {
                                        txtFavourOf.Visible = true;
                                    }
                                }

                                string favourOf = GetLocalResourceObject("FavourOf").ToString();
                                //Set "FavourOf" if their is no text in the textbox with a watermark style. Otherwise remove this style
                                txtFavourOf.Attributes.Remove("onblur");
                                txtFavourOf.Attributes.Remove("onfocus");
                                txtFavourOf.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + favourOf + "';$(this).addClass('input-watermark');}");
                                txtFavourOf.Attributes.Add("onfocus", "if (this.value == '" + favourOf + "') {this.value = '';$(this).removeClass('input-watermark');}");
                                journalScript = journalScript + "if ($('#" + txtFavourOf.ID + "').val() == '" + favourOf + "') {$('#" + txtFavourOf.ID + "').addClass('input-watermark');}";
                                div.Controls.Add(txtFavourOf);
                                break;
                                #endregion
                        }

                    }
                    if (crControls.Count() > 0)
                    {
                        tcControl.Controls.Add(div);//Adding to Table Cell
                        trControls.Cells.Add(tcControl);//Adding to Table Row
                        tbControls.Rows.Add(trControls);//Adding to Table
                        divGroupCr.Controls.Add(tbControls);//Adding to Masater Div that holds all the Credit Groups
                    }

                }
            }

            #endregion

            if (hasDR && hasCR)
                hasDRCR = true;//Has dr cr controls. means it is postback

            if (!hasDRCR)//true only for first time
            {
                #region First time binding
                if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                {
                    #region Edit
                    #region Setting all cost center details against this voucher to a custom list
                    VoucherPk = finTrxHdrList[0].FTH_PK;
                    CostCenterList = null;
                    CostCenterTempList = null;
                    GetFieldValues(ControlsEnum.ALLCOSTCENTERS);
                    SetFiledValues(ControlsEnum.ALLCOSTCENTERS);
                    #endregion
                    if (finTrxHdrList[0].FIN_TRX != null && finTrxHdrList[0].FIN_TRX.Count > 0)//hav dummy row
                    {
                        string selectedAccountVal;
                        foreach (FIN_TRX finTrxObj in finTrxHdrList[0].FIN_TRX)
                        {
                            if ((finTrxObj.FTR_DR_AMT_TC > 0 || finTrxObj.FTR_CR_AMT_TC > 0 || finTrxObj.FTR_DR_AMT_BC > 0 || finTrxObj.FTR_CR_AMT_BC > 0) || finTrxObj.FTR_ENTRY_MODE == (byte)VoucherEntryMode.GainOrLoss)
                            {
                                selectedAccountVal = "-1";
                                divColStyle = "divcolmiddle-S1 input-margin2";
                                div = new HtmlGenericControl("div");
                                div.Attributes.Add("class", divColStyle);
                                divValidation = new HtmlGenericControl("div");
                                divValidation.Attributes.Add("class", "starwrap");
                                tbControls = new Table();
                                tbControls.CssClass = "";
                                trControls = new TableRow();
                                tcControl = new TableCell();
                                #region SubType
                                Label txtSubType = new Label()
                                {
                                    Text = "",
                                    ClientIDMode = ClientIDMode.Static
                                };

                                if (finTrxObj.FTR_DR_AMT_TC > 0 || finTrxObj.FTR_DR_AMT_BC > 0)//Debit
                                {
                                    txtSubType.ID = "dic" + drCount.ToString("0000") + "01" + "set" + "1" + "01" + "id" + drCount.ToString("0000") + "dr";
                                    txtSubType.Text = finTrxObj.FIN_COA_SUB_TYPE_CFG == null ? GetLocalResourceObject("New_Debit_Account").ToString() : finTrxObj.FIN_COA_SUB_TYPE_CFG.CST_CODE == "All" ? GetLocalResourceObject("New_Debit_Account").ToString() : finTrxObj.FIN_COA_SUB_TYPE_CFG.CST_CODE;
                                    //Associate control id will be the id of Account Textbox. We can get the this id by just incrementing the control index in the set from the Account type ControlID
                                    txtSubType.AssociatedControlID = "dic" + drCount.ToString("0000") + "02" + "set" + "1" + "02" + "id" + drCount.ToString("0000") + "dr";
                                }
                                else if (finTrxObj.FTR_CR_AMT_TC > 0 || finTrxObj.FTR_CR_AMT_BC > 0)//Credit
                                {
                                    txtSubType.ID = "dic" + crCount.ToString("0000") + "01" + "set" + "1" + "01" + "id" + crCount.ToString("0000") + "cr";
                                    txtSubType.Text = finTrxObj.FIN_COA_SUB_TYPE_CFG == null ? GetLocalResourceObject("New_Credit_Account").ToString() : finTrxObj.FIN_COA_SUB_TYPE_CFG.CST_CODE == "All" ? GetLocalResourceObject("New_Credit_Account").ToString() : finTrxObj.FIN_COA_SUB_TYPE_CFG.CST_CODE;
                                    //Associate control id will be the id of Account Textbox. We can get the this id by just incrementing the control index in the set from the Account type ControlID
                                    txtSubType.AssociatedControlID = "dic" + crCount.ToString("0000") + "02" + "set" + "1" + "02" + "id" + crCount.ToString("0000") + "cr";
                                }

                                div.Controls.Add(txtSubType);
                                if (txtSubType != null && !string.IsNullOrEmpty(txtSubType.ID))
                                {
                                    if (txtSubType.ID.EndsWith("dr"))
                                    {
                                        if (!dicDrControls.ContainsKey(txtSubType.ID))
                                            dicDrControls.Add(txtSubType.ID, "SubType");//Add to Active dr Controls List
                                    }
                                    else if (txtSubType.ID.EndsWith("cr"))
                                    {
                                        if (!dicCrControls.ContainsKey(txtSubType.ID))
                                            dicCrControls.Add(txtSubType.ID, "SubType");//Add to Active cr Controls List
                                    }
                                    if (!dicAccountType.ContainsKey(txtSubType.ID))
                                        dicAccountType.Add(txtSubType.ID, txtSubType.Text);//Add Account Name in dictionary
                                }
                                #endregion
                                #region Account
                                //It is an autocomplte control with postback. So we need Textbox,Hiddenfield and a Button.
                                TextBox txtAccount = new TextBox()
                                {
                                    Text = "",
                                    MaxLength = 100,
                                    ClientIDMode = ClientIDMode.Static,
                                    TabIndex = TabIndexDr,
                                    Enabled = true
                                };
                                HiddenField hdfAccount = new HiddenField
                                {
                                    ClientIDMode = ClientIDMode.Static
                                };
                                Button btnAccount = new Button
                                {
                                    ClientIDMode = ClientIDMode.Static,
                                    CommandName = "JOURNALACCOUNTINDEXCHANGED",
                                    EnableTheming = false
                                };
                                btnAccount.Attributes.Add("style", "display:none;");
                                if (finTrxObj.FTR_DR_AMT_TC > 0 || finTrxObj.FTR_DR_AMT_BC > 0)//Debit
                                {
                                    txtAccount.ID = "dic" + drCount.ToString("0000") + "02" + "set" + "1" + "02" + "id" + drCount.ToString("0000") + "dr";
                                    hdfAccount.ID = "dic" + drCount.ToString("0000") + "03" + "set" + "1" + "03" + "id" + drCount.ToString("0000") + "dr";
                                    btnAccount.ID = "dic" + drCount.ToString("0000") + "04" + "set" + "1" + "04" + "id" + drCount.ToString("0000") + "dr";
                                    txtAccount.TabIndex = TabIndexDr;
                                    TabIndexDr++;
                                }
                                else if (finTrxObj.FTR_CR_AMT_TC > 0 || finTrxObj.FTR_CR_AMT_BC > 0)//Credit
                                {
                                    txtAccount.ID = "dic" + crCount.ToString("0000") + "02" + "set" + "1" + "02" + "id" + crCount.ToString("0000") + "cr";
                                    hdfAccount.ID = "dic" + crCount.ToString("0000") + "03" + "set" + "1" + "03" + "id" + crCount.ToString("0000") + "cr";
                                    btnAccount.ID = "dic" + crCount.ToString("0000") + "04" + "set" + "1" + "04" + "id" + crCount.ToString("0000") + "cr";
                                    txtAccount.TabIndex = TabIndexCr;
                                    TabIndexCr++;
                                }
                                if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.PCS && finTrxObj.FTR_CR_AMT_TC > 0)
                                    hdfSubTypePk.Value = FINCOASUBTYPECFGEnum.Cash.GetHashCode().ToString();
                                else
                                    hdfSubTypePk.Value = finTrxObj.FTR_ACC_SUB_TYPE.ToString();

                                if (!dicControlinfo.ContainsKey(txtAccount.ID + "SubTypePk"))//Store Subtype PK
                                    dicControlinfo.Add(txtAccount.ID + "SubTypePk", hdfSubTypePk.Value);
                                //Register Autocomplete script
                                if (!string.IsNullOrEmpty(txtAccount.ID))
                                    journalScript = journalScript + "GrandScriptUtils.MakeAutoCompleteDDL('" + txtAccount.ID + "', (url1.indexOf('?') != -1 ? url1+'&' :  url1+'?') + 'AccType=" + hdfSubTypePk.Value + "', '" + hdfAccount.ID + "', true, true, 'JOURNALACCOUNT',false,false,false);";

                                hdfAccount.Value = finTrxObj.FTR_ACCOUNT.HasValue ? finTrxObj.FTR_ACCOUNT.ToString() :
                                    !string.IsNullOrEmpty(hdfAccount.Value) ? hdfAccount.Value : "";
                                selectedAccountVal = hdfAccount.Value;
                                hdfCoaPk.Value = selectedAccountVal;
                                GetFieldValues(ControlsEnum.FINCOAMST);
                                if (finCoaMstList != null && finCoaMstList.Count == 1)
                                {
                                    txtAccount.Text = HttpUtility.HtmlDecode(finCoaMstList[0].COA_CODE + " - " + finCoaMstList[0].COA_NAME);
                                }
                                hdfCoaPk.Value = "";

                                #region Updating Control ID in Cost Center list
                                if (CostCenterList != null && CostCenterList.Count > 0)
                                {
                                    List<CostCenterDetails> objCostCntrLst = CostCenterList.Where(r => r.FTR_PK == finTrxObj.FTR_PK).ToList();
                                    if (objCostCntrLst != null && objCostCntrLst.Count > 0)
                                    {
                                        objCostCntrLst.ForEach(dtl => dtl.CONTROL_ID = hdfAccount.ID);
                                    }
                                }
                                #endregion

                                //Register Button Event
                                btnAccount.Click += new EventHandler(ActionHandler);

                                div.Controls.Add(txtAccount);
                                div.Controls.Add(hdfAccount);
                                div.Controls.Add(btnAccount);

                                RequiredFieldValidator vrfAccount = new RequiredFieldValidator()
                                {
                                    ID = "vrf" + txtAccount.ID.Replace("dic", ""),
                                    ControlToValidate = txtAccount.ID,
                                    Text = "*",
                                    CssClass = "star",
                                    ValidationGroup = "voucher",
                                    Display = ValidatorDisplay.Dynamic,
                                    EnableClientScript = true,
                                    InitialValue = Resources.Messages.AutoDefaultValue,
                                    SetFocusOnError = true,
                                    ErrorMessage = GetLocalResourceObject("Err_Account").ToString(),
                                    ClientIDMode = ClientIDMode.Static
                                };
                                //Add validation control to validation div
                                divValidation.Controls.Add(vrfAccount);
                                if (txtAccount != null && !string.IsNullOrEmpty(txtAccount.ID))
                                {
                                    if (txtAccount.ID.EndsWith("dr"))
                                    {
                                        if (!dicDrControls.ContainsKey(txtAccount.ID))
                                            dicDrControls.Add(txtAccount.ID, "Account");//Add to Active dr Controls List
                                        if (hdfAccount != null)
                                            if (!dicDrControls.ContainsKey(hdfAccount.ID))
                                                dicDrControls.Add(hdfAccount.ID, "Account");//Add to Active dr Controls List
                                        if (btnAccount != null)
                                            if (!dicDrControls.ContainsKey(btnAccount.ID))
                                                dicDrControls.Add(btnAccount.ID, "Account");//Add to Active dr Controls List

                                    }
                                    else if (txtAccount.ID.EndsWith("cr"))
                                    {
                                        if (!dicCrControls.ContainsKey(txtAccount.ID))
                                            dicCrControls.Add(txtAccount.ID, "Account");//Add to Active cr Controls List
                                        if (hdfAccount != null)
                                            if (!dicCrControls.ContainsKey(hdfAccount.ID))
                                                dicCrControls.Add(hdfAccount.ID, "Account");//Add to Active cr Controls List
                                        if (btnAccount != null)
                                            if (!dicCrControls.ContainsKey(btnAccount.ID))
                                                dicCrControls.Add(btnAccount.ID, "Account");//Add to Active cr Controls List
                                    }
                                }
                                #endregion
                                #region Narration
                                //Narration Textbox Control
                                TextBox txtNarration = new TextBox()
                                {
                                    Text = "",
                                    MaxLength = 400,
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    Width = IsSameCurrency() == true ? new Unit(GetLocalResourceObject("NarationWidthWithoutAmountBC").ToString()) : new Unit(GetLocalResourceObject("NarationWidthNormal").ToString())
                                };
                                txtNarration.Text = HttpUtility.HtmlDecode(finTrxObj.FTR_NARRATION);
                                if (finTrxObj.FTR_DR_AMT_TC > 0 || finTrxObj.FTR_DR_AMT_BC > 0)
                                {
                                    txtNarration.ID = "dic" + drCount.ToString("0000") + "05" + "set" + "1" + "05" + "id" + drCount.ToString("0000") + "dr";
                                    txtNarration.TabIndex = TabIndexDr;
                                    TabIndexDr++;
                                }
                                else if (finTrxObj.FTR_CR_AMT_TC > 0 || finTrxObj.FTR_CR_AMT_BC > 0)
                                {
                                    txtNarration.ID = "dic" + crCount.ToString("0000") + "05" + "set" + "1" + "05" + "id" + crCount.ToString("0000") + "cr";
                                    txtNarration.TabIndex = TabIndexCr;
                                    TabIndexCr++;
                                }
                                div.Controls.Add(txtNarration);//Add controls to div
                                if (txtNarration != null && txtNarration.ID != null)
                                {
                                    if (txtNarration.ID.EndsWith("dr"))
                                    {
                                        if (!dicDrControls.ContainsKey(txtNarration.ID))
                                            dicDrControls.Add(txtNarration.ID, "Narration");//Add to Active dr Controls List
                                    }
                                    else if (txtNarration.ID.EndsWith("cr"))
                                    {
                                        if (!dicCrControls.ContainsKey(txtNarration.ID))
                                            dicCrControls.Add(txtNarration.ID, "Narration");//Add to Active cr Controls List
                                    }
                                }
                                #endregion
                                #region AmountTC
                                //Amount in Transaction Currency
                                TextBox txtAmountTC = new TextBox()
                                {
                                    Text = "",
                                    MaxLength = 15,
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    CssClass = "input-w8per numeric tempcss"
                                };
                                if (finTrxObj.FTR_DR_AMT_TC > 0 || finTrxObj.FTR_DR_AMT_BC > 0)//Debit
                                {
                                    txtAmountTC.ID = "dic" + drCount.ToString("0000") + "06" + "set" + "1" + "06" + "id" + drCount.ToString("0000") + "dr";
                                    txtAmountTC.Text = finTrxObj.FTR_ACCOUNT == -1 ?
                                        ((decimal)0).ToString(hdfCurrencyFormatVoucher.Value)
                                        : ERP.Utilities.CommonFunctions.DecimalFormat(finTrxObj.FTR_DR_AMT_TC, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormatVoucher.Value);

                                    txtAmountTC.CssClass = "input-w8per numeric amounttcdr tempcss";
                                    txtAmountTC.TabIndex = TabIndexDr;
                                    TabIndexDr++;
                                }
                                else if (finTrxObj.FTR_CR_AMT_TC > 0 || finTrxObj.FTR_CR_AMT_BC > 0)//Credit
                                {
                                    txtAmountTC.ID = "dic" + crCount.ToString("0000") + "06" + "set" + "1" + "06" + "id" + crCount.ToString("0000") + "cr";
                                    txtAmountTC.Text = finTrxObj.FTR_ACCOUNT == -1 ?
                                        ((decimal)0).ToString(hdfCurrencyFormatVoucher.Value)
                                        : ERP.Utilities.CommonFunctions.DecimalFormat(finTrxObj.FTR_CR_AMT_TC, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormatVoucher.Value);

                                    txtAmountTC.CssClass = "input-w8per numeric amounttccr tempcss";
                                    txtAmountTC.TabIndex = TabIndexCr;
                                    TabIndexCr++;
                                }

                                if (finTrxObj.FTR_ENTRY_MODE == (byte)VoucherEntryMode.GainOrLoss)
                                {
                                    txtAmountTC.Text = string.Empty;
                                    if (finTrxObj.FTR_DR_AMT_TC > 0 || finTrxObj.FTR_DR_AMT_BC > 0)
                                        txtAmountTC.CssClass = "input-w8per numeric input-normalb amounttcdr tempcss";
                                    else if (finTrxObj.FTR_CR_AMT_TC > 0 || finTrxObj.FTR_CR_AMT_BC > 0)
                                        txtAmountTC.CssClass = "input-w8per numeric input-normalb amounttccr tempcss";
                                    else
                                        txtAmountTC.CssClass = "input-w8per numeric input-normalb tempcss";
                                    txtAmountTC.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                                    txtAmountTC.Attributes.Add("onpaste", "return false;");
                                }
                                else//Not G/L
                                {
                                    if (!string.IsNullOrEmpty(txtAmountTC.ID))
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtAmountTC.ID + "", "$('[id$=" + txtAmountTC.ID + "]').ForceNumericOnly();", true);
                                    txtAmountTC.Attributes.Add("onkeyup", "CalculateBCAmt(this);");//script registration for Calculate BC
                                }

                                div.Controls.Add(txtAmountTC);

                                AmountValidation vamAmountTC = new AmountValidation()
                                {
                                    ID = "vam" + txtAmountTC.ID.Replace("dic", ""),
                                    ControlToValidate = txtAmountTC.ID,
                                    ErrorMessage = GetLocalResourceObject("MsgErr_AmountTc").ToString(),
                                    NumberDigits = 11,
                                    Display = ValidatorDisplay.Dynamic,
                                    Text = "*",
                                    EnableClientScript = true,
                                    CssClass = "star",
                                    ValidationGroup = "voucher",
                                    NonZero = true
                                };
                                divValidation.Controls.Add(vamAmountTC);

                                if (txtAmountTC != null && txtAmountTC.ID != null)
                                {
                                    if (txtAmountTC.ID.EndsWith("dr"))
                                    {
                                        if (!dicDrControls.ContainsKey(txtAmountTC.ID))
                                            dicDrControls.Add(txtAmountTC.ID, "AmountTC");
                                    }
                                    else if (txtAmountTC.ID.EndsWith("cr"))
                                    {
                                        if (!dicCrControls.ContainsKey(txtAmountTC.ID))
                                            dicCrControls.Add(txtAmountTC.ID, "AmountTC");
                                    }
                                }
                                #endregion
                                #region ExchangeRate
                                TextBox txtExchangeRate = new TextBox()
                                {
                                    Text = "",
                                    MaxLength = 9,
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    CssClass = "numeric input-w6per"

                                };
                                HiddenField hdfEntryMode = new HiddenField()
                                {
                                    ClientIDMode = ClientIDMode.Static
                                };

                                if (finTrxObj.FTR_DR_AMT_TC > 0 || finTrxObj.FTR_DR_AMT_BC > 0)//Debit
                                {
                                    txtExchangeRate.ID = "dic" + drCount.ToString("0000") + "07" + "set" + "1" + "07" + "id" + drCount.ToString("0000") + "dr";
                                    txtExchangeRate.TabIndex = TabIndexDr;
                                    TabIndexDr++;
                                }
                                else if (finTrxObj.FTR_CR_AMT_TC > 0 || finTrxObj.FTR_CR_AMT_BC > 0)//Credit
                                {
                                    txtExchangeRate.ID = "dic" + crCount.ToString("0000") + "07" + "set" + "1" + "07" + "id" + crCount.ToString("0000") + "cr";
                                    txtExchangeRate.TabIndex = TabIndexCr;
                                    TabIndexCr++;
                                }
                                hdfEntryMode.ID = txtExchangeRate.ID + "EntryMode";//Used for to keep the entrymode of ExchangeRate
                                //txtExchangeRate.Text = ERP.Utilities.CommonFunctions.DoubleFormat(finTrxObj.FTR_EXCHG_RATE, exchRateDecimalDigits).ToString(hdfExchRateFormatVoucher.Value);
                                txtExchangeRate.Text = ERP.Utilities.CommonFunctions.DoubleFormatRound(finTrxObj.FTR_EXCHG_RATE, exchRateDecimalDigits).ToString(hdfExchRateFormatVoucher.Value);
                                //Change By Juno
                                // txtExchangeRate.Text = finTrxObj.FTR_EXCHG_RATE.ToString();
                                if (finTrxObj.FTR_ENTRY_MODE == (byte)VoucherEntryMode.NonEditable)
                                {
                                    if (GainLossCalculationMode == (byte)GLCalculationMode.Actual)
                                    {
                                        txtExchangeRate.CssClass = "numeric input-w6per input-normalb";
                                        txtExchangeRate.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                                        txtExchangeRate.Attributes.Add("onpaste", "return false;");
                                    }
                                    else
                                    {
                                        txtExchangeRate.Attributes.Add("onkeyup", "CalculateBCWithER(this);");//Register script for calculating BC with Exchange Rate
                                        if (!string.IsNullOrEmpty(txtExchangeRate.ID))
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtExchangeRate.ID + "", "$('[id$=" + txtExchangeRate.ID + "]').ForceNumericOnly();", true);

                                    }
                                }
                                else if (finTrxObj.FTR_ENTRY_MODE == (byte)VoucherEntryMode.GainOrLoss)
                                {
                                    txtExchangeRate.Text = string.Empty;
                                    txtExchangeRate.CssClass = "input-normalb numeric input-w6per";
                                    txtExchangeRate.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                                    txtExchangeRate.Attributes.Add("onpaste", "return false;");
                                }
                                else//Editable
                                {
                                    if (!string.IsNullOrEmpty(txtExchangeRate.ID))
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtExchangeRate.ID + "", "$('[id$=" + txtExchangeRate.ID + "]').ForceNumericOnly();", true);
                                    txtExchangeRate.Attributes.Add("onkeyup", "CalculateBCWithER(this);");
                                }

                                hdfEntryMode.Value = finTrxObj.FTR_ENTRY_MODE.ToString();

                                if (!dicControlinfo.ContainsKey(txtExchangeRate.ID + "EntryMode"))
                                    dicControlinfo.Add(txtExchangeRate.ID + "EntryMode", finTrxObj.FTR_ENTRY_MODE.ToString());//Keep the entrymode for the curresponding Exchange Rate in Dictionary

                                div.Controls.Add(txtExchangeRate);
                                div.Controls.Add(hdfEntryMode);

                                ExchangeRateValidation vreExchangeRate = new ExchangeRateValidation()
                                {
                                    ID = "vre" + txtExchangeRate.ID,
                                    ControlToValidate = txtExchangeRate.ID,
                                    ErrorMessage = GetLocalResourceObject("MsgErr_ExchangeRate").ToString(),
                                    NumberDigits = 5,
                                    Display = ValidatorDisplay.Dynamic,
                                    Text = "*",
                                    EnableClientScript = true,
                                    CssClass = "star",
                                    ValidationGroup = "voucher",
                                    NonZero = true
                                };
                                divValidation.Controls.Add(vreExchangeRate);

                                if (txtExchangeRate != null && txtExchangeRate.ID != null)
                                {
                                    if (txtExchangeRate.ID.EndsWith("dr"))
                                    {
                                        if (!dicDrControls.ContainsKey(txtExchangeRate.ID))
                                            dicDrControls.Add(txtExchangeRate.ID, "ExchangeRate");
                                        if (!dicDrControls.ContainsKey(hdfEntryMode.ID))
                                            dicDrControls.Add(hdfEntryMode.ID, "EntryMode");
                                    }
                                    else if (txtExchangeRate.ID.EndsWith("cr"))
                                    {
                                        if (!dicCrControls.ContainsKey(txtExchangeRate.ID))
                                            dicCrControls.Add(txtExchangeRate.ID, "ExchangeRate");
                                        if (!dicCrControls.ContainsKey(hdfEntryMode.ID))
                                            dicCrControls.Add(hdfEntryMode.ID, "EntryMode");
                                    }
                                }
                                transactionCurrency = 0;
                                if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                                {
                                    transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                                }
                                else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                                {
                                    transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);
                                }

                                if (transactionCurrency > 0
                                    &&
                                    transactionCurrency == currentUser.BaseCurrency)
                                {

                                    txtExchangeRate.Visible = false;
                                }
                                #endregion
                                #region AmountBC
                                //Amount in Base Currency
                                TextBox txtAmountBC = new TextBox()
                                {
                                    Text = "",
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    MaxLength = 15,
                                    CssClass = "input-normalb input-w8per numeric tempcss"
                                };

                                if (finTrxObj.FTR_DR_AMT_TC > 0 || finTrxObj.FTR_DR_AMT_BC > 0)//Debit
                                {
                                    txtAmountBC.ID = "dic" + drCount.ToString("0000") + "08" + "set" + "1" + "08" + "id" + drCount.ToString("0000") + "dr";
                                    txtAmountBC.Text = finTrxObj.FTR_ACCOUNT == -1 ?
                                       ((decimal)0).ToString(hdfCurrencyFormatVoucher.Value)
                                       : ERP.Utilities.CommonFunctions.DecimalFormat(finTrxObj.FTR_DR_AMT_BC, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormatVoucher.Value);
                                    txtAmountBC.TabIndex = TabIndexDr;
                                    TabIndexDr++;
                                }
                                else if (finTrxObj.FTR_CR_AMT_TC > 0 || finTrxObj.FTR_CR_AMT_BC > 0)//Credit
                                {
                                    txtAmountBC.ID = "dic" + crCount.ToString("0000") + "08" + "set" + "1" + "08" + "id" + crCount.ToString("0000") + "cr";
                                    txtAmountBC.Text = finTrxObj.FTR_ACCOUNT == -1 ?
                                       ((decimal)0).ToString(hdfCurrencyFormatVoucher.Value)
                                       : ERP.Utilities.CommonFunctions.DecimalFormat(finTrxObj.FTR_CR_AMT_BC, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormatVoucher.Value);
                                    txtAmountBC.TabIndex = TabIndexCr;
                                    TabIndexCr++;
                                }

                                if ((BCEnable && finTrxObj.FTR_ENTRY_MODE == (byte)VoucherEntryMode.Editable) || IsYearEndVoucher() || IsEditableForApplication())
                                {
                                    txtAmountBC.CssClass = "input-w8per numeric tempcss";
                                    txtAmountBC.Attributes.Add("onkeyup", "CalculateTCAmt(this,event);");//script registration for Calculate TC
                                    if (!string.IsNullOrEmpty(txtAmountBC.ID))
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtAmountBC.ID + "", "$('[id$=" + txtAmountBC.ID + "]').ForceNumericOnly();", true);
                                }
                                else
                                {
                                    txtAmountBC.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                                    txtAmountBC.Attributes.Add("onpaste", "return false;");
                                }

                                div.Controls.Add(txtAmountBC);

                                AmountValidation vamAmountBC = new AmountValidation()
                                {
                                    ID = "vam" + txtAmountBC.ID.Replace("dic", ""),
                                    ControlToValidate = txtAmountBC.ID,
                                    ErrorMessage = GetLocalResourceObject("MsgErr_AmountBc").ToString(),
                                    NumberDigits = 11,
                                    Display = ValidatorDisplay.Dynamic,
                                    Text = "*",
                                    EnableClientScript = true,
                                    CssClass = "star",
                                    ValidationGroup = "voucher"
                                };
                                divValidation.Controls.Add(vamAmountBC);

                                if (txtAmountBC != null && txtAmountBC.ID != null)
                                {
                                    if (txtAmountBC.ID.EndsWith("dr"))
                                    {
                                        if (!dicDrControls.ContainsKey(txtAmountBC.ID))
                                            dicDrControls.Add(txtAmountBC.ID, "AmountBC");
                                    }
                                    else if (txtAmountBC.ID.EndsWith("cr"))
                                    {
                                        if (!dicCrControls.ContainsKey(txtAmountBC.ID))
                                            dicCrControls.Add(txtAmountBC.ID, "AmountBC");
                                    }
                                }

                                transactionCurrency = 0;
                                if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                                    transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                                else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                                    transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);

                                if (transactionCurrency > 0 && transactionCurrency == currentUser.BaseCurrency)
                                    txtAmountBC.Visible = false;

                                #endregion
                                #region Delete
                                //Delete button for deleting
                                Button btnDelete = new Button()
                                {
                                    Text = "",
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    SkinID = "delete-icon",
                                    ToolTip = "Delete",
                                    OnClientClick = "return ShowDeleteConfirm(this);"
                                };
                                if (finTrxObj.FTR_DR_AMT_TC > 0 || finTrxObj.FTR_DR_AMT_BC > 0)//Debit
                                {
                                    btnDelete.ID = "dic" + drCount.ToString("0000") + "09" + "set" + "1" + "09" + "id" + drCount.ToString("0000") + "dr";
                                    btnDelete.CommandName = ActionsEnum.REMOVEDEBIT.ToString();
                                    btnDelete.TabIndex = TabIndexDr;
                                    TabIndexDr++;
                                }
                                else if (finTrxObj.FTR_CR_AMT_BC > 0 || finTrxObj.FTR_CR_AMT_TC > 0)//Credit
                                {
                                    btnDelete.ID = "dic" + crCount.ToString("0000") + "09" + "set" + "1" + "09" + "id" + crCount.ToString("0000") + "cr";
                                    btnDelete.CommandName = ActionsEnum.REMOVECREDIT.ToString();
                                    btnDelete.TabIndex = TabIndexCr;
                                    TabIndexCr++;
                                }

                                btnDelete.Click += new EventHandler(ActionHandler);
                                if (EntryStatus == EntryStatus.VIEWMODE)
                                    btnDelete.Visible = false;
                                div.Controls.Add(btnDelete);
                                ////divValidation contains all the Validation Controls in a group. It will be added to the group after delete button. So if any validation catch it will show * after delete button 
                                //div.Controls.Add(divValidation);
                                if (btnDelete != null && btnDelete.ID != null)
                                {
                                    if (btnDelete.ID.EndsWith("dr"))
                                    {
                                        if (!dicDrControls.ContainsKey(btnDelete.ID))
                                            dicDrControls.Add(btnDelete.ID, "Delete");
                                    }
                                    else if (btnDelete.ID.EndsWith("cr"))
                                    {
                                        if (!dicCrControls.ContainsKey(btnDelete.ID))
                                            dicCrControls.Add(btnDelete.ID, "Delete");
                                    }
                                }
                                #endregion
                                #region Cost Center
                                //Delete button for deleting
                                Button btnCostCenter = new Button()
                                {
                                    Text = "",
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    SkinID = "costcenter-icon",
                                    ToolTip = GetLocalResourceObject("CCButtonToolTip").ToString()
                                };
                                HiddenField hdfIsCostCenter = new HiddenField
                                {
                                    ClientIDMode = ClientIDMode.Static
                                };
                                if (finTrxObj.FTR_DR_AMT_TC > 0 || finTrxObj.FTR_DR_AMT_BC > 0)//Debit
                                {
                                    btnCostCenter.ID = "dic" + drCount.ToString("0000") + "10" + "set" + "1" + "10" + "id" + drCount.ToString("0000") + "dr";
                                    hdfIsCostCenter.ID = "dic" + drCount.ToString("0000") + "11" + "set" + "1" + "11" + "id" + drCount.ToString("0000") + "dr";
                                    btnCostCenter.CommandName = ActionsEnum.COSTCENTER.ToString();
                                    btnCostCenter.TabIndex = TabIndexDr;
                                    TabIndexDr++;
                                }
                                else if (finTrxObj.FTR_CR_AMT_BC > 0 || finTrxObj.FTR_CR_AMT_TC > 0)//Credit
                                {
                                    btnCostCenter.ID = "dic" + crCount.ToString("0000") + "10" + "set" + "1" + "10" + "id" + crCount.ToString("0000") + "cr";
                                    hdfIsCostCenter.ID = "dic" + crCount.ToString("0000") + "11" + "set" + "1" + "11" + "id" + crCount.ToString("0000") + "cr";
                                    btnCostCenter.CommandName = ActionsEnum.COSTCENTER.ToString();
                                    btnCostCenter.TabIndex = TabIndexCr;
                                    TabIndexCr++;
                                }
                                btnCostCenter.Click += new EventHandler(ActionHandler);
                                div.Controls.Add(btnCostCenter);

                                //divValidation contains all the Validation Controls in a group. It will be added to the group after delete button. So if any validation catch it will show * after delete button 
                                div.Controls.Add(divValidation);

                                div.Controls.Add(hdfIsCostCenter);

                                if (btnCostCenter != null && btnCostCenter.ID != null)
                                {
                                    if (btnCostCenter.ID.EndsWith("dr"))
                                    {
                                        if (!dicDrControls.ContainsKey(btnCostCenter.ID))
                                            dicDrControls.Add(btnCostCenter.ID, "CostCenter");
                                        if (hdfIsCostCenter != null)
                                            if (!dicDrControls.ContainsKey(hdfIsCostCenter.ID))
                                                dicDrControls.Add(hdfIsCostCenter.ID, "CostCenter");
                                    }
                                    else if (btnCostCenter.ID.EndsWith("cr"))
                                    {
                                        if (!dicCrControls.ContainsKey(btnCostCenter.ID))
                                            dicCrControls.Add(btnCostCenter.ID, "CostCenter");
                                        if (hdfIsCostCenter != null)
                                            if (!dicCrControls.ContainsKey(hdfIsCostCenter.ID))
                                                dicCrControls.Add(hdfIsCostCenter.ID, "CostCenter");
                                    }
                                }
                                if ((finTrxObj.FIN_COA_MST != null
                                    && finTrxObj.FIN_COA_MST.FIN_COA_COST_CENTER_MPG != null
                                    && finTrxObj.FIN_COA_MST.FIN_COA_COST_CENTER_MPG.Where(r => r.ADM_COST_CENTER_MST.CNM_ACTIVE == 1).Count() > 0)
                                    || (finTrxObj.FIN_TRX_COC_DTL != null && finTrxObj.FIN_TRX_COC_DTL.Count > 0))
                                {
                                    hdfIsCostCenter.Value = "1";
                                    btnCostCenter.Visible = true;
                                }
                                else
                                {
                                    hdfIsCostCenter.Value = "0";
                                    btnCostCenter.Visible = false;
                                }


                                #endregion
                                #region SubledgerLabel
                                //After Delete Button, the rest of the controls will comes in the next line. So the divClear will use to break the first line
                                divClear = new HtmlGenericControl("div");
                                divClear.Attributes.Add("class", "clear");
                                div.Controls.Add(divClear);
                                //It is a dummy label to adjust the style
                                Label lblSubledgerLabel = new Label()
                                {
                                    ClientIDMode = ClientIDMode.Static,
                                    Text = "&nbsp",
                                    Visible = false
                                };
                                //The Associate control id is the id of subledger dropdownlist
                                if (finTrxObj.FTR_DR_AMT_TC > 0 || finTrxObj.FTR_DR_AMT_BC > 0)//Debit
                                {
                                    lblSubledgerLabel.ID = "dic" + drCount.ToString("0000") + "50" + "set" + "2" + "01" + "id" + drCount.ToString("0000") + "dr";
                                    lblSubledgerLabel.AssociatedControlID = "dic" + drCount.ToString("0000") + "51" + "set" + "2" + "02" + "id" + drCount.ToString("0000") + "dr"; ;
                                }
                                else if (finTrxObj.FTR_CR_AMT_TC > 0 || finTrxObj.FTR_CR_AMT_BC > 0)//Credit
                                {
                                    lblSubledgerLabel.ID = "dic" + crCount.ToString("0000") + "50" + "set" + "2" + "01" + "id" + crCount.ToString("0000") + "cr";
                                    lblSubledgerLabel.AssociatedControlID = "dic" + crCount.ToString("0000") + "51" + "set" + "2" + "02" + "id" + crCount.ToString("0000") + "cr";
                                }

                                div.Controls.Add(lblSubledgerLabel);
                                if (lblSubledgerLabel != null && lblSubledgerLabel.ID != null)
                                {
                                    if (lblSubledgerLabel.ID.EndsWith("dr"))
                                    {
                                        if (!dicDrControls.ContainsKey(lblSubledgerLabel.ID))
                                            dicDrControls.Add(lblSubledgerLabel.ID, "SubledgerLabel");
                                    }
                                    else if (lblSubledgerLabel.ID.EndsWith("cr"))
                                    {
                                        if (!dicCrControls.ContainsKey(lblSubledgerLabel.ID))
                                            dicCrControls.Add(lblSubledgerLabel.ID, "SubledgerLabel");
                                    }
                                }
                                #endregion
                                #region SubLedger
                                //Subledger ddl control
                                DropDownList ddlSubLedger = new DropDownList()
                                {
                                    ClientIDMode = ClientIDMode.Static,
                                    Visible = false
                                };

                                if (finTrxObj.FTR_DR_AMT_TC > 0 || finTrxObj.FTR_DR_AMT_BC > 0)//Debit
                                {
                                    ddlSubLedger.ID = "dic" + drCount.ToString("0000") + "51" + "set" + "2" + "02" + "id" + drCount.ToString("0000") + "dr";

                                    ddlSubLedger.TabIndex = TabIndexDr;
                                    TabIndexDr++;
                                }
                                else if (finTrxObj.FTR_CR_AMT_TC > 0 || finTrxObj.FTR_CR_AMT_BC > 0)//Credit
                                {
                                    ddlSubLedger.ID = "dic" + crCount.ToString("0000") + "51" + "set" + "2" + "02" + "id" + crCount.ToString("0000") + "cr";

                                    ddlSubLedger.TabIndex = TabIndexCr;
                                    TabIndexCr++;
                                }

                                hdfCoaPk.Value = selectedAccountVal;
                                if (!dicControlinfo.ContainsKey(txtAccount.ID + "CoaPk"))
                                    dicControlinfo.Add(txtAccount.ID + "CoaPk", hdfCoaPk.Value);

                                finCoaSubTypeCfgList = null;
                                GetFieldValues(ControlsEnum.FINCOAMST);
                                if (finCoaMstList != null && finCoaMstList.Count > 0)
                                {
                                    hdfSubTypePk.Value = finCoaMstList[0].COA_SUB_TYPE.ToString();
                                    if (!string.IsNullOrEmpty(hdfSubTypePk.Value))
                                    {
                                        GetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                                        if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                                        {
                                            relquery = finCoaSubTypeCfgList[0].CST_REL_QUERY == null ? string.Empty : finCoaSubTypeCfgList[0].ADM_QUERIES_CFG1.QRY_QUERY;
                                        }

                                        if (relquery != string.Empty)//Have related qry. Then show ddl
                                        {
                                            int selectedSubLedger = 0;
                                            string typePk = finTrxObj.FTR_TYPE_PK.HasValue == true ? finTrxObj.FTR_TYPE_PK.ToString().Trim() : "";
                                            int.TryParse(typePk, out selectedSubLedger);

                                            ddlSubLedger.Visible = true;
                                            lblSubledgerLabel.Visible = true;
                                            commonService = new CommonService();
                                            commonService = CommonFunctions.InitiateClient(commonService);
                                            if (finCoaSubTypeCfgList[0].CST_CODE == "AP" || finCoaSubTypeCfgList[0].CST_CODE == "AR")
                                            {
                                                relquery = relquery.Replace("@COA@", hdfCoaPk.Value == "" ? "NULL" : hdfCoaPk.Value);
                                                relquery = relquery.Replace("@ADV@", "NULL");
                                            }
                                            else if (finCoaSubTypeCfgList[0].CST_CODE == "ADP" || finCoaSubTypeCfgList[0].CST_CODE == "ADR")
                                            {
                                                relquery = relquery.Replace("@COA@", "NULL");
                                                relquery = relquery.Replace("@ADV@", hdfCoaPk.Value == "" ? "NULL" : hdfCoaPk.Value);
                                            }
                                            else
                                            {
                                                relquery = relquery.Replace("@COA@", "NULL");
                                                relquery = relquery.Replace("@ADV@", "NULL");
                                            }
                                            relquery = relquery.Replace("@PK@", selectedSubLedger.ToString());
                                            relquery = relquery.Replace("@BIZUNITPK@", currentUser.SBUID.ToString());
                                            List<DDLMaster> ddlChildValues = commonService.ExecuteQuery(relquery);
                                            ddlSubLedger.DataTextField = "Value";
                                            ddlSubLedger.DataValueField = "PK";
                                            ddlSubLedger.DataSource = CommonFunctions.HtmlDecode(ddlChildValues, "Value");
                                            ddlSubLedger.DataBind();
                                            ddlSubLedger.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));

                                            ddlSubLedger.SelectedValue = (ListItem)ddlSubLedger.Items.FindByValue(selectedSubLedger.ToString()) != null ?
                                                selectedSubLedger.ToString() : CommonConstants.SELECTVAL;

                                            if (ddlSubLedger.Items.Count == 2)//If subledger have only 1 value other than "Select" then set selected value with that one
                                            {
                                                ddlSubLedger.SelectedValue = ddlSubLedger.Items[1].Value;
                                            }
                                        }
                                    }
                                }
                                div.Controls.Add(ddlSubLedger);

                                RequiredFieldValidator vrfSubLedger = new RequiredFieldValidator()
                                {
                                    ID = "vrf" + ddlSubLedger.ID.Replace("dic", ""),
                                    ControlToValidate = ddlSubLedger.ID,
                                    Text = "*",
                                    CssClass = "star",
                                    ValidationGroup = "voucher",
                                    Display = ValidatorDisplay.Dynamic,
                                    EnableClientScript = true,
                                    InitialValue = CommonConstants.SELECTVAL,
                                    SetFocusOnError = true,
                                    ErrorMessage = GetLocalResourceObject("Err_SubLedger").ToString(),
                                    ClientIDMode = ClientIDMode.Static
                                };
                                divValidation.Controls.Add(vrfSubLedger);
                                vrfSubLedger.Enabled = ddlSubLedger.Visible ? true : false;

                                if (ddlSubLedger != null && ddlSubLedger.ID != null)
                                {
                                    if (ddlSubLedger.ID.EndsWith("dr"))
                                    {
                                        if (!dicDrControls.ContainsKey(ddlSubLedger.ID))
                                            dicDrControls.Add(ddlSubLedger.ID, "SubLedger");
                                    }
                                    else if (ddlSubLedger.ID.EndsWith("cr"))
                                    {
                                        if (!dicCrControls.ContainsKey(ddlSubLedger.ID))
                                            dicCrControls.Add(ddlSubLedger.ID, "SubLedger");
                                    }
                                }
                                #endregion
                                #region InstrumentNo
                                TextBox txtInstrumentNo = new TextBox()
                                {
                                    Text = "",
                                    MaxLength = 70,
                                    ClientIDMode = ClientIDMode.Static,
                                    Visible = false,
                                    CssClass = "input16"
                                };
                                if (finTrxObj.FTR_DR_AMT_TC > 0 || finTrxObj.FTR_DR_AMT_BC > 0)//Debit
                                {
                                    txtInstrumentNo.ID = "dic" + drCount.ToString("0000") + "52" + "set" + "2" + "03" + "id" + drCount.ToString("0000") + "dr";

                                    txtInstrumentNo.TabIndex = TabIndexDr;
                                    TabIndexDr++;
                                }
                                else if (finTrxObj.FTR_CR_AMT_TC > 0 || finTrxObj.FTR_CR_AMT_BC > 0)//Credit
                                {
                                    txtInstrumentNo.ID = "dic" + crCount.ToString("0000") + "52" + "set" + "2" + "03" + "id" + crCount.ToString("0000") + "cr";

                                    txtInstrumentNo.TabIndex = TabIndexCr;
                                    TabIndexCr++;
                                }

                                txtInstrumentNo.Text = HttpUtility.HtmlDecode(finTrxObj.FTR_INSTR_NO);
                                string instrumentNo = GetLocalResourceObject("InstrumentNo").ToString();
                                //Set "Instrument No" if their is no text in the textbox with a watermark style. Otherwise remove this style
                                txtInstrumentNo.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + instrumentNo + "';$(this).addClass('input-watermark');}");
                                txtInstrumentNo.Attributes.Add("onfocus", "if (this.value == '" + instrumentNo + "') {this.value = '';$(this).removeClass('input-watermark');}");
                                if (string.IsNullOrEmpty(txtInstrumentNo.Text.Trim()))
                                    txtInstrumentNo.Text = instrumentNo;
                                journalScript = journalScript + "if ($('#" + txtInstrumentNo.ID + "').val() == '" + instrumentNo + "') {$('#" + txtInstrumentNo.ID + "').addClass('input-watermark');}";

                                if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                                {
                                    if (finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("Bank").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PDC").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PPC").ToString()))
                                    {
                                        txtInstrumentNo.Visible = true;
                                    }
                                }

                                div.Controls.Add(txtInstrumentNo);
                                if (txtInstrumentNo != null && txtInstrumentNo.ID != null)
                                {
                                    if (txtInstrumentNo.ID.EndsWith("dr"))
                                    {
                                        if (!dicDrControls.ContainsKey(txtInstrumentNo.ID))
                                            dicDrControls.Add(txtInstrumentNo.ID, "InstrumentNo");
                                    }
                                    else if (txtInstrumentNo.ID.EndsWith("cr"))
                                    {
                                        if (!dicCrControls.ContainsKey(txtInstrumentNo.ID))
                                            dicCrControls.Add(txtInstrumentNo.ID, "InstrumentNo");
                                    }
                                }
                                #endregion
                                #region Date
                                //Date Picker Control
                                TextBox txtDate = new TextBox()
                                {
                                    Text = "",
                                    MaxLength = 11,
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    Visible = false,
                                    CssClass = "input-w8-5per"
                                };

                                if (finTrxObj.FTR_DR_AMT_TC > 0 || finTrxObj.FTR_DR_AMT_BC > 0)//Debit
                                {
                                    txtDate.ID = "dic" + drCount.ToString("0000") + "53" + "set" + "2" + "04" + "id" + drCount.ToString("0000") + "dr";

                                    txtDate.TabIndex = TabIndexDr;
                                    TabIndexDr++;
                                }
                                else if (finTrxObj.FTR_CR_AMT_TC > 0 || finTrxObj.FTR_CR_AMT_BC > 0)//Credit
                                {
                                    txtDate.ID = "dic" + crCount.ToString("0000") + "53" + "set" + "2" + "04" + "id" + crCount.ToString("0000") + "cr";

                                    txtDate.TabIndex = TabIndexCr;
                                    TabIndexCr++;
                                }

                                txtDate.Text = finTrxObj.FTR_INSTR_DATE.ToString() == "" ? "" : ((DateTime)finTrxObj.FTR_INSTR_DATE).ToString(Resources.Constants.DateFormatShort);
                                txtDate.Attributes.Add("onkeydown", "return CheckKey(event);");
                                txtDate.Attributes.Add("onpaste", "return false;");
                                //Set "Date" if their is no text in the textbox with a watermark style. Otherwise remove this style
                                string date = GetLocalResourceObject("Date").ToString();
                                txtDate.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + date + "';$(this).addClass('input-watermark');}");
                                txtDate.Attributes.Add("onfocus", "if (this.value == '" + date + "') {this.value = '';$(this).removeClass('input-watermark');}");
                                if (string.IsNullOrEmpty(txtDate.Text.Trim()))
                                    txtDate.Text = date;
                                journalScript = journalScript + "if ($('#" + txtDate.ID + "').val() == '" + date + "') {$('#" + txtDate.ID + "').addClass('input-watermark');}";

                                if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                                {
                                    if (finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("Bank").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PDC").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PPC").ToString()))
                                    {
                                        txtDate.Visible = true;
                                    }
                                }

                                if (txtDate.Visible == true)
                                {
                                    if (!string.IsNullOrEmpty(txtDate.ID))
                                    {
                                        string PageScript = CommonFunctions.GenerateDynamicScript("Date", txtDate.ID, null, null, null);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtDate.ID + "", PageScript, true);
                                    }
                                }
                                div.Controls.Add(txtDate);

                                if (txtDate != null && txtDate.ID != null)
                                {
                                    if (txtDate.ID.EndsWith("dr"))
                                    {
                                        if (!dicDrControls.ContainsKey(txtDate.ID))
                                            dicDrControls.Add(txtDate.ID, "Date");
                                    }
                                    else if (txtDate.ID.EndsWith("cr"))
                                    {
                                        if (!dicCrControls.ContainsKey(txtDate.ID))
                                            dicCrControls.Add(txtDate.ID, "Date");
                                    }
                                }

                                #endregion
                                #region FavourOf
                                //Favourof Textbox
                                TextBox txtFavourOf = new TextBox()
                                {
                                    Text = "",
                                    MaxLength = 150,
                                    ClientIDMode = ClientIDMode.Static,
                                    Enabled = true,
                                    Visible = false,
                                    CssClass = "input-w23-6per"
                                };

                                if (finTrxObj.FTR_DR_AMT_TC > 0 || finTrxObj.FTR_DR_AMT_BC > 0)//Debit
                                {
                                    txtFavourOf.ID = "dic" + drCount.ToString("0000") + "54" + "set" + "2" + "05" + "id" + drCount.ToString("0000") + "dr";

                                    txtFavourOf.TabIndex = TabIndexDr;
                                    TabIndexDr++;
                                }
                                else if (finTrxObj.FTR_CR_AMT_TC > 0 || finTrxObj.FTR_CR_AMT_BC > 0)//Credit
                                {
                                    txtFavourOf.ID = "dic" + crCount.ToString("0000") + "54" + "set" + "2" + "05" + "id" + crCount.ToString("0000") + "cr";

                                    txtFavourOf.TabIndex = TabIndexCr;
                                    TabIndexCr++;
                                }

                                txtFavourOf.Text = HttpUtility.HtmlDecode(finTrxObj.FTR_INSTR_FAVOUR);

                                string favourOf = GetLocalResourceObject("FavourOf").ToString();
                                //Set "FavourOf" if their is no text in the textbox with a watermark style. Otherwise remove this style
                                txtFavourOf.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + favourOf + "';$(this).addClass('input-watermark');}");
                                txtFavourOf.Attributes.Add("onfocus", "if (this.value == '" + favourOf + "') {this.value = '';$(this).removeClass('input-watermark');}");
                                if (string.IsNullOrEmpty(txtFavourOf.Text.Trim()))
                                    txtFavourOf.Text = favourOf;
                                journalScript = journalScript + "if ($('#" + txtFavourOf.ID + "').val() == '" + favourOf + "') {$('#" + txtFavourOf.ID + "').addClass('input-watermark');}";

                                if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                                {
                                    if (finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("Bank").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PDC").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PPC").ToString()))
                                    {
                                        txtFavourOf.Visible = true;
                                    }
                                }

                                div.Controls.Add(txtFavourOf);
                                if (txtFavourOf != null && txtFavourOf.ID != null)
                                {
                                    if (txtFavourOf.ID.EndsWith("dr"))
                                    {
                                        if (!dicDrControls.ContainsKey(txtFavourOf.ID))
                                            dicDrControls.Add(txtFavourOf.ID, "FavourOf");
                                    }
                                    else if (txtFavourOf.ID.EndsWith("cr"))
                                    {
                                        if (!dicCrControls.ContainsKey(txtFavourOf.ID))
                                            dicCrControls.Add(txtFavourOf.ID, "FavourOf");
                                    }
                                }
                                #endregion

                                if (finTrxObj.FTR_DR_AMT_TC > 0 || finTrxObj.FTR_DR_AMT_BC > 0)
                                {
                                    if (!dicFinTrxPK.ContainsKey("dic" + drCount.ToString("0000") + "dr"))
                                        dicFinTrxPK.Add("dic" + drCount.ToString("0000") + "dr", finTrxObj.FTR_PK);
                                }
                                else if (finTrxObj.FTR_CR_AMT_TC > 0 || finTrxObj.FTR_CR_AMT_BC > 0)
                                {
                                    if (!dicFinTrxPK.ContainsKey("dic" + crCount.ToString("0000") + "cr"))
                                        dicFinTrxPK.Add("dic" + crCount.ToString("0000") + "cr", finTrxObj.FTR_PK);
                                }

                                tcControl.Controls.Add(div);//Adding to Table Cell
                                trControls.Cells.Add(tcControl);//Adding to Table Row
                                tbControls.Rows.Add(trControls);//Adding to Table
                                if ((finTrxObj.FTR_DR_AMT_TC > 0 || finTrxObj.FTR_DR_AMT_BC > 0) && !hasDR)
                                {
                                    divGroupDr.Controls.Add(tbControls);//Adding to Masater Div that holds all the Debit Groups
                                    drCount++;
                                }
                                else if ((finTrxObj.FTR_CR_AMT_TC > 0 || finTrxObj.FTR_CR_AMT_BC > 0) && !hasCR)
                                {
                                    divGroupCr.Controls.Add(tbControls);//Adding to Masater Div that holds all the Credit Groups
                                    crCount++;
                                }
                            }
                        }

                        if (dicDrControls != null && dicDrControls.Count > 0)
                        {
                            Session[ERP.Utilities.SessionStrings.DrControls] = dicDrControls;//Updating the Debit Control Session
                        }
                        if (dicCrControls != null && dicCrControls.Count > 0)
                        {
                            Session[ERP.Utilities.SessionStrings.CrControls] = dicCrControls;//Updating the Credit Control Session
                        }
                        if (dicControlinfo != null && dicControlinfo.Count > 0)
                        {
                            Session[ERP.Utilities.SessionStrings.ControlInfo] = dicControlinfo;//Updating the Control Info. Session
                        }
                        if (dicFinTrxPK != null && dicFinTrxPK.Count > 0)
                        {
                            Session[ERP.Utilities.SessionStrings.FinTrxPk] = dicFinTrxPK;//Updating the Fin Transaction PKs
                        }
                        if (dicAccountType != null && dicAccountType.Count > 0)
                        {
                            Session[ERP.Utilities.SessionStrings.AccountType] = dicAccountType;//Updating the Account Name Session
                        }

                    }
                    #endregion
                }
                else
                {
                    if (VoucherTemplatePK > 0)
                    {
                        #region Load From Template
                        CostCenterList = null;
                        CostCenterTempList = null;
                        GetFieldValues(ControlsEnum.VOUCHERTEMPLATE);//Get Voucher Templates
                        if (dsVoucherTemplateList != null && dsVoucherTemplateList.Tables[0].Rows.Count > 0)
                        {
                            string selectedAccountVal;
                            foreach (DataRow templateRow in dsVoucherTemplateList.Tables[0].Rows)
                            {
                                if (templateRow["VLD_ACCOUNT"] != null && Convert.ToInt32(templateRow["VLD_ACCOUNT"].ToString()) > 0)
                                {
                                    selectedAccountVal = "-1";
                                    divColStyle = "divcolmiddle-S1 input-margin2";
                                    div = new HtmlGenericControl("div");
                                    div.Attributes.Add("class", divColStyle);
                                    divValidation = new HtmlGenericControl("div");
                                    divValidation.Attributes.Add("class", "starwrap");
                                    tbControls = new Table();
                                    tbControls.CssClass = "";
                                    trControls = new TableRow();
                                    tcControl = new TableCell();
                                    #region SubType
                                    Label txtSubType = new Label()//Account Type
                                    {
                                        Text = "",
                                        ClientIDMode = ClientIDMode.Static
                                    };

                                    if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 2)//1:Debit  //In JV Template Debit=2 
                                    {
                                        txtSubType.ID = "dic" + drCount.ToString("0000") + "01" + "set" + "1" + "01" + "id" + drCount.ToString("0000") + "dr";
                                        txtSubType.Text = templateRow["VLD_REF_TYPE"] != null && !string.IsNullOrEmpty(templateRow["VLD_REF_TYPE"].ToString()) ?
                                            templateRow["VLD_REF_TYPE"].ToString().Trim() : GetLocalResourceObject("New_Debit_Account").ToString();
                                        txtSubType.AssociatedControlID = "dic" + drCount.ToString("0000") + "02" + "set" + "1" + "02" + "id" + drCount.ToString("0000") + "dr";
                                    }
                                    else if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 1)//2:Credit  //In JV Template Credit=1
                                    {
                                        txtSubType.ID = "dic" + crCount.ToString("0000") + "01" + "set" + "1" + "01" + "id" + crCount.ToString("0000") + "cr";
                                        txtSubType.Text = templateRow["VLD_REF_TYPE"] != null && !string.IsNullOrEmpty(templateRow["VLD_REF_TYPE"].ToString()) ?
                                            templateRow["VLD_REF_TYPE"].ToString().Trim() : GetLocalResourceObject("New_Credit_Account").ToString();
                                        txtSubType.AssociatedControlID = "dic" + crCount.ToString("0000") + "02" + "set" + "1" + "02" + "id" + crCount.ToString("0000") + "cr";
                                    }

                                    div.Controls.Add(txtSubType);
                                    if (txtSubType != null && !string.IsNullOrEmpty(txtSubType.ID))
                                    {
                                        if (txtSubType.ID.EndsWith("dr"))
                                        {
                                            if (!dicDrControls.ContainsKey(txtSubType.ID))
                                                dicDrControls.Add(txtSubType.ID, "SubType");//Add to Active dr Controls List
                                        }
                                        else if (txtSubType.ID.EndsWith("cr"))
                                        {
                                            if (!dicCrControls.ContainsKey(txtSubType.ID))
                                                dicCrControls.Add(txtSubType.ID, "SubType");//Add to Active cr Controls List
                                        }
                                        if (!dicAccountType.ContainsKey(txtSubType.ID))
                                            dicAccountType.Add(txtSubType.ID, txtSubType.Text);//Add Account Name in dictionary
                                    }
                                    #endregion
                                    #region Account
                                    //It is an autocomplte control with postback. So we need Textbox,Hiddenfield and a Button.
                                    TextBox txtAccount = new TextBox()
                                    {
                                        Text = "",
                                        MaxLength = 100,
                                        ClientIDMode = ClientIDMode.Static,
                                        TabIndex = TabIndexDr,
                                        Enabled = true
                                    };
                                    HiddenField hdfAccount = new HiddenField
                                    {
                                        ClientIDMode = ClientIDMode.Static
                                    };
                                    Button btnAccount = new Button
                                    {
                                        ClientIDMode = ClientIDMode.Static,
                                        CommandName = "JOURNALACCOUNTINDEXCHANGED",
                                        EnableTheming = false
                                    };
                                    btnAccount.Attributes.Add("style", "display:none;");
                                    if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 2)//1:Debit //In JV Template Debit=2 
                                    {
                                        txtAccount.ID = "dic" + drCount.ToString("0000") + "02" + "set" + "1" + "02" + "id" + drCount.ToString("0000") + "dr";
                                        hdfAccount.ID = "dic" + drCount.ToString("0000") + "03" + "set" + "1" + "03" + "id" + drCount.ToString("0000") + "dr";
                                        btnAccount.ID = "dic" + drCount.ToString("0000") + "04" + "set" + "1" + "04" + "id" + drCount.ToString("0000") + "dr";
                                        txtAccount.TabIndex = TabIndexDr;
                                        TabIndexDr++;
                                    }
                                    else if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 1)//2:Credit //In JV Template Credit=1
                                    {
                                        txtAccount.ID = "dic" + crCount.ToString("0000") + "02" + "set" + "1" + "02" + "id" + crCount.ToString("0000") + "cr";
                                        hdfAccount.ID = "dic" + crCount.ToString("0000") + "03" + "set" + "1" + "03" + "id" + crCount.ToString("0000") + "cr";
                                        btnAccount.ID = "dic" + crCount.ToString("0000") + "04" + "set" + "1" + "04" + "id" + crCount.ToString("0000") + "cr";
                                        txtAccount.TabIndex = TabIndexCr;
                                        TabIndexCr++;
                                    }

                                    if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.PCS &&
                                        Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 2)
                                    {
                                        hdfSubTypePk.Value = FINCOASUBTYPECFGEnum.Cash.GetHashCode().ToString();
                                    }
                                    else
                                    {
                                        hdfSubTypePk.Value = "0";
                                    }

                                    if (!dicControlinfo.ContainsKey(txtAccount.ID + "SubTypePk"))//Store Subtype PK
                                        dicControlinfo.Add(txtAccount.ID + "SubTypePk", hdfSubTypePk.Value);
                                    //register AutoComlete
                                    if (!string.IsNullOrEmpty(txtAccount.ID))
                                        journalScript = journalScript + "GrandScriptUtils.MakeAutoCompleteDDL('" + txtAccount.ID + "', (url1.indexOf('?') != -1 ? url1+'&' :  url1+'?') + 'AccType=" + hdfSubTypePk.Value + "', '" + hdfAccount.ID + "', true, true, 'JOURNALACCOUNT',false,false,false);";

                                    hdfAccount.Value = templateRow["VLD_ACCOUNT"].ToString();
                                    selectedAccountVal = hdfAccount.Value;
                                    hdfCoaPk.Value = selectedAccountVal;
                                    GetFieldValues(ControlsEnum.FINCOAMST);
                                    if (finCoaMstList != null && finCoaMstList.Count == 1)
                                    {
                                        txtAccount.Text = HttpUtility.HtmlDecode(finCoaMstList[0].COA_CODE + " - " + finCoaMstList[0].COA_NAME);
                                    }
                                    hdfCoaPk.Value = "";
                                    //Register Button Event
                                    btnAccount.Click += new EventHandler(ActionHandler);


                                    div.Controls.Add(txtAccount);
                                    div.Controls.Add(hdfAccount);
                                    div.Controls.Add(btnAccount);

                                    RequiredFieldValidator vrfAccount = new RequiredFieldValidator()
                                    {
                                        ID = "vrf" + txtAccount.ID.Replace("dic", ""),
                                        ControlToValidate = txtAccount.ID,
                                        Text = "*",
                                        CssClass = "star",
                                        ValidationGroup = "voucher",
                                        Display = ValidatorDisplay.Dynamic,
                                        EnableClientScript = true,
                                        InitialValue = Resources.Messages.AutoDefaultValue,
                                        SetFocusOnError = true,
                                        ErrorMessage = GetLocalResourceObject("Err_Account").ToString(),
                                        ClientIDMode = ClientIDMode.Static
                                    };
                                    //Add validation control to validation div
                                    divValidation.Controls.Add(vrfAccount);
                                    if (txtAccount != null && !string.IsNullOrEmpty(txtAccount.ID))
                                    {
                                        if (txtAccount.ID.EndsWith("dr"))
                                        {
                                            if (!dicDrControls.ContainsKey(txtAccount.ID))
                                                dicDrControls.Add(txtAccount.ID, "Account");//Add to Active dr Controls List
                                            if (hdfAccount != null)
                                                if (!dicDrControls.ContainsKey(hdfAccount.ID))
                                                    dicDrControls.Add(hdfAccount.ID, "Account");//Add to Active dr Controls List
                                            if (btnAccount != null)
                                                if (!dicDrControls.ContainsKey(btnAccount.ID))
                                                    dicDrControls.Add(btnAccount.ID, "Account");//Add to Active dr Controls List

                                        }
                                        else if (txtAccount.ID.EndsWith("cr"))
                                        {
                                            if (!dicCrControls.ContainsKey(txtAccount.ID))
                                                dicCrControls.Add(txtAccount.ID, "Account");//Add to Active cr Controls List
                                            if (hdfAccount != null)
                                                if (!dicCrControls.ContainsKey(hdfAccount.ID))
                                                    dicCrControls.Add(hdfAccount.ID, "Account");//Add to Active cr Controls List
                                            if (btnAccount != null)
                                                if (!dicCrControls.ContainsKey(btnAccount.ID))
                                                    dicCrControls.Add(btnAccount.ID, "Account");//Add to Active cr Controls List
                                        }
                                    }
                                    #endregion
                                    #region Narration
                                    //Narration Textbox Control
                                    TextBox txtNarration = new TextBox()
                                    {
                                        Text = "",
                                        MaxLength = 400,
                                        ClientIDMode = ClientIDMode.Static,
                                        Enabled = true,
                                        Width = IsSameCurrency() == true ? new Unit(GetLocalResourceObject("NarationWidthWithoutAmountBC").ToString()) : new Unit(GetLocalResourceObject("NarationWidthNormal").ToString())
                                    };

                                    txtNarration.Text = string.Empty;

                                    if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 2)//1:Debit  //In JV Template Debit=2 
                                    {
                                        txtNarration.ID = "dic" + drCount.ToString("0000") + "05" + "set" + "1" + "05" + "id" + drCount.ToString("0000") + "dr";
                                        txtNarration.TabIndex = TabIndexDr;
                                        TabIndexDr++;
                                    }
                                    else if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 1)//2:Credit  //In JV Template Credit=1
                                    {
                                        txtNarration.ID = "dic" + crCount.ToString("0000") + "05" + "set" + "1" + "05" + "id" + crCount.ToString("0000") + "cr";
                                        txtNarration.TabIndex = TabIndexCr;
                                        TabIndexCr++;

                                    }
                                    div.Controls.Add(txtNarration);//Add controls to div
                                    if (txtNarration != null && txtNarration.ID != null)
                                    {
                                        if (txtNarration.ID.EndsWith("dr"))
                                        {
                                            if (!dicDrControls.ContainsKey(txtNarration.ID))
                                                dicDrControls.Add(txtNarration.ID, "Narration");//Add to Active dr Controls List
                                        }
                                        else if (txtNarration.ID.EndsWith("cr"))
                                        {
                                            if (!dicCrControls.ContainsKey(txtNarration.ID))
                                                dicCrControls.Add(txtNarration.ID, "Narration");//Add to Active cr Controls List
                                        }
                                    }
                                    #endregion
                                    #region AmountTC
                                    //Amount in Transaction Currency
                                    TextBox txtAmountTC = new TextBox()
                                    {
                                        Text = "",
                                        MaxLength = 15,
                                        ClientIDMode = ClientIDMode.Static,
                                        Enabled = true,
                                        CssClass = "input-w8per numeric tempcss"
                                    };
                                    if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 2)//1:Debit //In JV Template Debit=2 
                                    {
                                        txtAmountTC.ID = "dic" + drCount.ToString("0000") + "06" + "set" + "1" + "06" + "id" + drCount.ToString("0000") + "dr";
                                        txtAmountTC.Text = ((decimal)0).ToString(hdfCurrencyFormatVoucher.Value);
                                        txtAmountTC.CssClass = "input-w8per numeric amounttcdr tempcss";
                                        txtAmountTC.TabIndex = TabIndexDr;
                                        TabIndexDr++;
                                    }
                                    else if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 1)//2:Credit //In JV Template Credit=1 
                                    {
                                        txtAmountTC.ID = "dic" + crCount.ToString("0000") + "06" + "set" + "1" + "06" + "id" + crCount.ToString("0000") + "cr";
                                        txtAmountTC.Text = ((decimal)0).ToString(hdfCurrencyFormatVoucher.Value);
                                        txtAmountTC.CssClass = "input-w8per numeric amounttccr tempcss";
                                        txtAmountTC.TabIndex = TabIndexCr;
                                        TabIndexCr++;
                                    }

                                    if (!string.IsNullOrEmpty(txtAmountTC.ID))
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtAmountTC.ID + "", "$('[id$=" + txtAmountTC.ID + "]').ForceNumericOnly();", true);
                                    txtAmountTC.Attributes.Add("onkeyup", "CalculateBCAmt(this);");//script registration for Calculate BC

                                    div.Controls.Add(txtAmountTC);
                                    AmountValidation vamAmountTC = new AmountValidation()
                                    {
                                        ID = "vam" + txtAmountTC.ID.Replace("dic", ""),
                                        ControlToValidate = txtAmountTC.ID,
                                        ErrorMessage = GetLocalResourceObject("MsgErr_AmountTc").ToString(),
                                        NumberDigits = 11,
                                        Display = ValidatorDisplay.Dynamic,
                                        Text = "*",
                                        EnableClientScript = true,
                                        CssClass = "star",
                                        ValidationGroup = "voucher",
                                        NonZero = true
                                    };
                                    divValidation.Controls.Add(vamAmountTC);

                                    if (txtAmountTC != null && txtAmountTC.ID != null)
                                    {
                                        if (txtAmountTC.ID.EndsWith("dr"))
                                        {
                                            if (!dicDrControls.ContainsKey(txtAmountTC.ID))
                                                dicDrControls.Add(txtAmountTC.ID, "AmountTC");//Add to Active dr Controls List
                                        }
                                        else if (txtAmountTC.ID.EndsWith("cr"))
                                        {
                                            if (!dicCrControls.ContainsKey(txtAmountTC.ID))
                                                dicCrControls.Add(txtAmountTC.ID, "AmountTC");//Add to Active cr Controls List
                                        }
                                    }
                                    #endregion
                                    #region ExchangeRate
                                    TextBox txtExchangeRate = new TextBox()
                                    {
                                        Text = "",
                                        MaxLength = 9,
                                        ClientIDMode = ClientIDMode.Static,
                                        Enabled = true,
                                        CssClass = "numeric input-w6per"
                                    };
                                    HiddenField hdfEntryMode = new HiddenField()
                                    {
                                        ClientIDMode = ClientIDMode.Static
                                    };

                                    if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 2)//1:Debit //In JV Template Debit=2
                                    {
                                        txtExchangeRate.ID = "dic" + drCount.ToString("0000") + "07" + "set" + "1" + "07" + "id" + drCount.ToString("0000") + "dr";
                                        txtExchangeRate.TabIndex = TabIndexDr;
                                        TabIndexDr++;
                                    }
                                    else if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 1)//2:Credit //In JV Template Credit=1
                                    {
                                        txtExchangeRate.ID = "dic" + crCount.ToString("0000") + "07" + "set" + "1" + "07" + "id" + crCount.ToString("0000") + "cr";
                                        txtExchangeRate.TabIndex = TabIndexCr;
                                        TabIndexCr++;
                                    }
                                    hdfEntryMode.ID = txtExchangeRate.ID + "EntryMode";//Used for to keep the entrymode of ExchangeRate

                                    txtExchangeRate.Text = !string.IsNullOrEmpty(txtJournalExchangeRate.Text.Trim()) ?
                                        ERP.Utilities.CommonFunctions.DoubleFormat(Convert.ToDouble(txtJournalExchangeRate.Text.Trim()), exchRateDecimalDigits).ToString(hdfExchRateFormatVoucher.Value)
                                        : 1.ToString(hdfExchRateFormatVoucher.Value);

                                    txtExchangeRate.Attributes.Add("onkeyup", "CalculateBCWithER(this);");//Register script for calculating BC with Exchange Rate
                                    if (!string.IsNullOrEmpty(txtExchangeRate.ID))
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtExchangeRate.ID + "", "$('[id$=" + txtExchangeRate.ID + "]').ForceNumericOnly();", true);

                                    hdfEntryMode.Value = ((byte)VoucherEntryMode.Editable).ToString();//Default EntryMode is Editable

                                    if (!dicControlinfo.ContainsKey(txtExchangeRate.ID + "EntryMode"))
                                        dicControlinfo.Add(txtExchangeRate.ID + "EntryMode", hdfEntryMode.Value);

                                    div.Controls.Add(txtExchangeRate);
                                    div.Controls.Add(hdfEntryMode);

                                    ExchangeRateValidation vreExchangeRate = new ExchangeRateValidation()
                                    {
                                        ID = "vre" + txtExchangeRate.ID,
                                        ControlToValidate = txtExchangeRate.ID,
                                        ErrorMessage = GetLocalResourceObject("MsgErr_ExchangeRate").ToString(),
                                        NumberDigits = 5,
                                        Display = ValidatorDisplay.Dynamic,
                                        Text = "*",
                                        EnableClientScript = true,
                                        CssClass = "star",
                                        ValidationGroup = "voucher",
                                        NonZero = true
                                    };
                                    divValidation.Controls.Add(vreExchangeRate);

                                    if (txtExchangeRate != null && txtExchangeRate.ID != null)
                                    {
                                        if (txtExchangeRate.ID.EndsWith("dr"))
                                        {
                                            if (!dicDrControls.ContainsKey(txtExchangeRate.ID))
                                                dicDrControls.Add(txtExchangeRate.ID, "ExchangeRate");//Add to Active dr Controls List
                                            if (!dicDrControls.ContainsKey(hdfEntryMode.ID))
                                                dicDrControls.Add(hdfEntryMode.ID, "EntryMode");//Add to Active dr Controls List
                                        }
                                        else if (txtExchangeRate.ID.EndsWith("cr"))
                                        {
                                            if (!dicCrControls.ContainsKey(txtExchangeRate.ID))
                                                dicCrControls.Add(txtExchangeRate.ID, "ExchangeRate");//Add to Active cr Controls List
                                            if (!dicCrControls.ContainsKey(hdfEntryMode.ID))
                                                dicCrControls.Add(hdfEntryMode.ID, "EntryMode");//Add to Active cr Controls List
                                        }
                                    }
                                    transactionCurrency = 0;
                                    if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                                    {
                                        transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                                    }
                                    else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                                    {
                                        transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);
                                    }

                                    if (transactionCurrency > 0
                                        &&
                                        transactionCurrency == currentUser.BaseCurrency)
                                    {

                                        txtExchangeRate.Visible = false;
                                    }
                                    #endregion
                                    #region AmountBC
                                    //Amount in Base Currency
                                    TextBox txtAmountBC = new TextBox()
                                    {
                                        Text = "",
                                        ClientIDMode = ClientIDMode.Static,
                                        Enabled = true,
                                        MaxLength = 15,
                                        CssClass = "input-normalb input-w8per numeric tempcss"
                                    };

                                    if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 2)//1:Debit //In JV Template Debit=2
                                        txtAmountBC.ID = "dic" + drCount.ToString("0000") + "08" + "set" + "1" + "08" + "id" + drCount.ToString("0000") + "dr";
                                    else if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 1)//2:Credit  //In JV Template Credit=1
                                        txtAmountBC.ID = "dic" + crCount.ToString("0000") + "08" + "set" + "1" + "08" + "id" + crCount.ToString("0000") + "cr";

                                    decimal temp = 0;
                                    decimal.TryParse(txtAmountTC.Text, out temp);
                                    txtAmountBC.Text = temp == 0 ?
                                        ((decimal)0).ToString(hdfCurrencyFormatVoucher.Value)
                                        : ERP.Utilities.CommonFunctions.DecimalFormat(temp * decimal.Parse(txtExchangeRate.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormatVoucher.Value);

                                    if ((BCEnable && hdfEntryMode.Value == ((byte)VoucherEntryMode.Editable).ToString()) || IsYearEndVoucher() || IsEditableForApplication())//If BC is Enable and Entrymode is Editable
                                    {
                                        txtAmountBC.CssClass = "input-w8per numeric tempcss";
                                        txtAmountBC.Attributes.Add("onkeyup", "CalculateTCAmt(this,event);");//script registration for Calculate TC
                                        if (!string.IsNullOrEmpty(txtAmountBC.ID))
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtAmountBC.ID + "", "$('[id$=" + txtAmountBC.ID + "]').ForceNumericOnly();", true);
                                    }
                                    else//Disable Amount BC
                                    {
                                        txtAmountBC.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                                        txtAmountBC.Attributes.Add("onpaste", "return false;");
                                    }

                                    div.Controls.Add(txtAmountBC);

                                    AmountValidation vamAmountBC = new AmountValidation()
                                    {
                                        ID = "vam" + txtAmountBC.ID.Replace("dic", ""),
                                        ControlToValidate = txtAmountBC.ID,
                                        ErrorMessage = GetLocalResourceObject("MsgErr_AmountBc").ToString(),
                                        NumberDigits = 11,
                                        Display = ValidatorDisplay.Dynamic,
                                        Text = "*",
                                        EnableClientScript = true,
                                        CssClass = "star",
                                        ValidationGroup = "voucher"
                                    };
                                    divValidation.Controls.Add(vamAmountBC);

                                    if (txtAmountBC != null && txtAmountBC.ID != null)
                                    {
                                        if (txtAmountBC.ID.EndsWith("dr"))
                                        {
                                            if (!dicDrControls.ContainsKey(txtAmountBC.ID))
                                                dicDrControls.Add(txtAmountBC.ID, "AmountBC");//Add to Active dr Controls List
                                        }
                                        else if (txtAmountBC.ID.EndsWith("cr"))
                                        {
                                            if (!dicCrControls.ContainsKey(txtAmountBC.ID))
                                                dicCrControls.Add(txtAmountBC.ID, "AmountBC");//Add to Active cr Controls List
                                        }
                                    }

                                    transactionCurrency = 0;
                                    if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                                        transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                                    else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                                        transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);

                                    if (transactionCurrency > 0 && transactionCurrency == currentUser.BaseCurrency)
                                        txtAmountBC.Visible = false;

                                    #endregion
                                    #region Delete
                                    //Delete button for deleting
                                    Button btnDelete = new Button()
                                    {
                                        Text = "",
                                        ClientIDMode = ClientIDMode.Static,
                                        Enabled = true,
                                        SkinID = "delete-icon",
                                        ToolTip = "Delete",
                                        OnClientClick = "return ShowDeleteConfirm(this);"
                                    };
                                    if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 2)//1:Debit //In JV Template Debit=2
                                    {
                                        btnDelete.ID = "dic" + drCount.ToString("0000") + "09" + "set" + "1" + "09" + "id" + drCount.ToString("0000") + "dr";
                                        btnDelete.CommandName = ActionsEnum.REMOVEDEBIT.ToString();

                                        btnDelete.TabIndex = TabIndexDr;
                                        TabIndexDr++;
                                    }
                                    else if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 1)//2:Credit //In JV Template Credit=1
                                    {
                                        btnDelete.ID = "dic" + crCount.ToString("0000") + "09" + "set" + "1" + "09" + "id" + crCount.ToString("0000") + "cr";
                                        btnDelete.CommandName = ActionsEnum.REMOVECREDIT.ToString();

                                        btnDelete.TabIndex = TabIndexCr;
                                        TabIndexCr++;
                                    }

                                    btnDelete.Click += new EventHandler(ActionHandler);
                                    if (EntryStatus == EntryStatus.VIEWMODE)
                                    {
                                        btnDelete.Visible = false;
                                    }
                                    div.Controls.Add(btnDelete);
                                    ////divValidation contains all the Validation Controls in a group. It will be added to the group after delete button. So if any validation catch it will show * after delete button 
                                    //div.Controls.Add(divValidation);
                                    if (btnDelete != null && btnDelete.ID != null)
                                    {
                                        if (btnDelete.ID.EndsWith("dr"))
                                        {
                                            if (!dicDrControls.ContainsKey(btnDelete.ID))
                                                dicDrControls.Add(btnDelete.ID, "Delete");//Add to Active dr Controls List
                                        }
                                        else if (btnDelete.ID.EndsWith("cr"))
                                        {
                                            if (!dicCrControls.ContainsKey(btnDelete.ID))
                                                dicCrControls.Add(btnDelete.ID, "Delete");//Add to Active cr Controls List
                                        }
                                    }
                                    #endregion
                                    #region Cost Center
                                    //Delete button for deleting
                                    Button btnCostCenter = new Button()
                                    {
                                        Text = "",
                                        ClientIDMode = ClientIDMode.Static,
                                        Enabled = true,
                                        SkinID = "costcenter-icon",
                                        ToolTip = GetLocalResourceObject("CCButtonToolTip").ToString()
                                    };
                                    HiddenField hdfIsCostCenter = new HiddenField
                                    {
                                        ClientIDMode = ClientIDMode.Static
                                    };
                                    if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 2)//1:Debit //In JV Template Debit=2
                                    {
                                        btnCostCenter.ID = "dic" + drCount.ToString("0000") + "10" + "set" + "1" + "10" + "id" + drCount.ToString("0000") + "dr";
                                        hdfIsCostCenter.ID = "dic" + drCount.ToString("0000") + "11" + "set" + "1" + "11" + "id" + drCount.ToString("0000") + "dr";
                                        btnCostCenter.CommandName = ActionsEnum.COSTCENTER.ToString();
                                        btnCostCenter.TabIndex = TabIndexDr;
                                        TabIndexDr++;
                                    }
                                    else if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 1)//2:Credit //In JV Template Credit=1
                                    {
                                        btnCostCenter.ID = "dic" + crCount.ToString("0000") + "10" + "set" + "1" + "10" + "id" + crCount.ToString("0000") + "cr";
                                        hdfIsCostCenter.ID = "dic" + crCount.ToString("0000") + "11" + "set" + "1" + "11" + "id" + crCount.ToString("0000") + "cr";
                                        btnCostCenter.CommandName = ActionsEnum.COSTCENTER.ToString();
                                        btnCostCenter.TabIndex = TabIndexCr;
                                        TabIndexCr++;
                                    }

                                    btnCostCenter.Click += new EventHandler(ActionHandler);
                                    div.Controls.Add(btnCostCenter);

                                    //divValidation contains all the Validation Controls in a group. It will be added to the group after delete button. So if any validation catch it will show * after delete button 
                                    div.Controls.Add(divValidation);

                                    div.Controls.Add(hdfIsCostCenter);

                                    if (btnCostCenter != null && btnCostCenter.ID != null)
                                    {
                                        if (btnCostCenter.ID.EndsWith("dr"))
                                        {
                                            if (!dicDrControls.ContainsKey(btnCostCenter.ID))
                                                dicDrControls.Add(btnCostCenter.ID, "CostCenter");//Add to Active dr Controls List
                                            if (hdfIsCostCenter != null)
                                                if (!dicDrControls.ContainsKey(hdfIsCostCenter.ID))
                                                    dicDrControls.Add(hdfIsCostCenter.ID, "CostCenter");//Add to Active dr Controls List
                                        }
                                        else if (btnCostCenter.ID.EndsWith("cr"))
                                        {
                                            if (!dicCrControls.ContainsKey(btnCostCenter.ID))
                                                dicCrControls.Add(btnCostCenter.ID, "CostCenter");//Add to Active cr Controls List
                                            if (hdfIsCostCenter != null)
                                                if (!dicCrControls.ContainsKey(hdfIsCostCenter.ID))
                                                    dicCrControls.Add(hdfIsCostCenter.ID, "CostCenter");//Add to Active cr Controls List
                                        }
                                    }
                                    if (finCoaMstList != null
                                       && finCoaMstList.Count == 1
                                       && finCoaMstList[0].FIN_COA_COST_CENTER_MPG != null
                                       && finCoaMstList[0].FIN_COA_COST_CENTER_MPG.Where(r => r.ADM_COST_CENTER_MST.CNM_ACTIVE == 1).Count() > 0)
                                    {
                                        hdfIsCostCenter.Value = "1";
                                        btnCostCenter.Visible = true;
                                    }
                                    else
                                    {
                                        hdfIsCostCenter.Value = "0";
                                        btnCostCenter.Visible = false;
                                    }

                                    #endregion
                                    #region SubledgerLabel
                                    //After Delete Button, the rest of the controls will comes in the next line. So the divClear will use to break the first line
                                    divClear = new HtmlGenericControl("div");
                                    divClear.Attributes.Add("class", "clear");
                                    div.Controls.Add(divClear);
                                    //It is a dummy label to adjust the style
                                    Label lblSubledgerLabel = new Label()
                                    {
                                        ClientIDMode = ClientIDMode.Static,
                                        Text = "&nbsp",
                                        Visible = false
                                    };

                                    if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 2)//1:Debit //In JV Template Debit=2
                                    {
                                        lblSubledgerLabel.ID = "dic" + drCount.ToString("0000") + "50" + "set" + "2" + "01" + "id" + drCount.ToString("0000") + "dr";
                                        lblSubledgerLabel.AssociatedControlID = "dic" + drCount.ToString("0000") + "51" + "set" + "2" + "02" + "id" + drCount.ToString("0000") + "dr"; ;
                                    }
                                    else if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 1)//2:Credit //In JV Template Credit=1
                                    {
                                        lblSubledgerLabel.ID = "dic" + crCount.ToString("0000") + "50" + "set" + "2" + "01" + "id" + crCount.ToString("0000") + "cr";
                                        lblSubledgerLabel.AssociatedControlID = "dic" + crCount.ToString("0000") + "51" + "set" + "2" + "02" + "id" + crCount.ToString("0000") + "cr";
                                    }

                                    div.Controls.Add(lblSubledgerLabel);
                                    if (lblSubledgerLabel != null && lblSubledgerLabel.ID != null)
                                    {
                                        if (lblSubledgerLabel.ID.EndsWith("dr"))
                                        {
                                            if (!dicDrControls.ContainsKey(lblSubledgerLabel.ID))
                                                dicDrControls.Add(lblSubledgerLabel.ID, "SubledgerLabel");//Add to Active dr Controls List
                                        }
                                        else if (lblSubledgerLabel.ID.EndsWith("cr"))
                                        {
                                            if (!dicCrControls.ContainsKey(lblSubledgerLabel.ID))
                                                dicCrControls.Add(lblSubledgerLabel.ID, "SubledgerLabel");//Add to Active cr Controls List
                                        }
                                    }
                                    #endregion
                                    #region SubLedger
                                    //Subledger ddl control
                                    DropDownList ddlSubLedger = new DropDownList()
                                    {
                                        ClientIDMode = ClientIDMode.Static,
                                        Visible = false
                                    };

                                    if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 2)//1:Debit //In JV Template Debit=2
                                    {
                                        ddlSubLedger.ID = "dic" + drCount.ToString("0000") + "51" + "set" + "2" + "02" + "id" + drCount.ToString("0000") + "dr";
                                        ddlSubLedger.TabIndex = TabIndexDr;
                                        TabIndexDr++;
                                    }
                                    else if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 1)//2:Credit //In JV Template Credit=1
                                    {
                                        ddlSubLedger.ID = "dic" + crCount.ToString("0000") + "51" + "set" + "2" + "02" + "id" + crCount.ToString("0000") + "cr";
                                        ddlSubLedger.TabIndex = TabIndexCr;
                                        TabIndexCr++;
                                    }

                                    hdfCoaPk.Value = selectedAccountVal;
                                    if (!dicControlinfo.ContainsKey(txtAccount.ID + "CoaPk"))
                                        dicControlinfo.Add(txtAccount.ID + "CoaPk", hdfCoaPk.Value);

                                    finCoaSubTypeCfgList = null;
                                    GetFieldValues(ControlsEnum.FINCOAMST);
                                    if (finCoaMstList != null && finCoaMstList.Count > 0)
                                    {
                                        hdfSubTypePk.Value = finCoaMstList[0].COA_SUB_TYPE.ToString();
                                        if (!string.IsNullOrEmpty(hdfSubTypePk.Value))
                                        {
                                            GetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                                            if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                                            {
                                                relquery = finCoaSubTypeCfgList[0].CST_REL_QUERY == null ? string.Empty : finCoaSubTypeCfgList[0].ADM_QUERIES_CFG1.QRY_QUERY;
                                            }

                                            if (relquery != string.Empty)//have Related Qry
                                            {
                                                int selectedSubLedger = 0;
                                                string typePk = templateRow["VLD_REF_TYPE_PK"] != null ? templateRow["VLD_REF_TYPE_PK"].ToString().Trim() : "";
                                                int.TryParse(typePk, out selectedSubLedger);
                                                ddlSubLedger.Visible = true;
                                                lblSubledgerLabel.Visible = true;
                                                commonService = new CommonService();
                                                commonService = CommonFunctions.InitiateClient(commonService);

                                                if (finCoaSubTypeCfgList[0].CST_CODE == "AP" || finCoaSubTypeCfgList[0].CST_CODE == "AR")
                                                {
                                                    relquery = relquery.Replace("@COA@", hdfCoaPk.Value == "" ? "NULL" : hdfCoaPk.Value);
                                                    relquery = relquery.Replace("@ADV@", "NULL");
                                                }
                                                else if (finCoaSubTypeCfgList[0].CST_CODE == "ADP" || finCoaSubTypeCfgList[0].CST_CODE == "ADR")
                                                {
                                                    relquery = relquery.Replace("@COA@", "NULL");
                                                    relquery = relquery.Replace("@ADV@", hdfCoaPk.Value == "" ? "NULL" : hdfCoaPk.Value);
                                                }
                                                else
                                                {
                                                    relquery = relquery.Replace("@COA@", "NULL");
                                                    relquery = relquery.Replace("@ADV@", "NULL");
                                                }
                                                relquery = relquery.Replace("@PK@", selectedSubLedger.ToString());
                                                relquery = relquery.Replace("@BIZUNITPK@", currentUser.SBUID.ToString());

                                                List<DDLMaster> ddlChildValues = commonService.ExecuteQuery(relquery);
                                                ddlSubLedger.DataTextField = "Value";
                                                ddlSubLedger.DataValueField = "PK";
                                                ddlSubLedger.DataSource = CommonFunctions.HtmlDecode(ddlChildValues, "Value");
                                                ddlSubLedger.DataBind();
                                                ddlSubLedger.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));

                                                ddlSubLedger.SelectedValue = (ListItem)ddlSubLedger.Items.FindByValue(selectedSubLedger.ToString()) != null ?
                                                    selectedSubLedger.ToString() : CommonConstants.SELECTVAL;
                                                if (ddlSubLedger.Items.Count == 2)
                                                {
                                                    ddlSubLedger.SelectedValue = ddlSubLedger.Items[1].Value;
                                                }
                                            }
                                        }
                                    }

                                    div.Controls.Add(ddlSubLedger);
                                    RequiredFieldValidator vrfSubLedger = new RequiredFieldValidator()
                                    {
                                        ID = "vrf" + ddlSubLedger.ID.Replace("dic", ""),
                                        ControlToValidate = ddlSubLedger.ID,
                                        Text = "*",
                                        CssClass = "star",
                                        ValidationGroup = "voucher",
                                        Display = ValidatorDisplay.Dynamic,
                                        EnableClientScript = true,
                                        InitialValue = CommonConstants.SELECTVAL,
                                        SetFocusOnError = true,
                                        ErrorMessage = GetLocalResourceObject("Err_SubLedger").ToString(),
                                        ClientIDMode = ClientIDMode.Static
                                    };
                                    divValidation.Controls.Add(vrfSubLedger);
                                    vrfSubLedger.Enabled = ddlSubLedger.Visible ? true : false;
                                    if (ddlSubLedger != null && ddlSubLedger.ID != null)
                                    {
                                        if (ddlSubLedger.ID.EndsWith("dr"))
                                        {
                                            if (!dicDrControls.ContainsKey(ddlSubLedger.ID))
                                                dicDrControls.Add(ddlSubLedger.ID, "SubLedger");//Add to Active dr Controls List
                                        }
                                        else if (ddlSubLedger.ID.EndsWith("cr"))
                                        {
                                            if (!dicCrControls.ContainsKey(ddlSubLedger.ID))
                                                dicCrControls.Add(ddlSubLedger.ID, "SubLedger");//Add to Active cr Controls List
                                        }
                                    }
                                    #endregion
                                    #region InstrumentNo
                                    //Instrument No Text box
                                    TextBox txtInstrumentNo = new TextBox()
                                    {
                                        Text = "",
                                        MaxLength = 70,
                                        ClientIDMode = ClientIDMode.Static,
                                        Visible = false,
                                        CssClass = "input16"
                                    };

                                    if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 2)//1:Debit //In JV Template Debit=2
                                    {
                                        txtInstrumentNo.ID = "dic" + drCount.ToString("0000") + "52" + "set" + "2" + "03" + "id" + drCount.ToString("0000") + "dr";
                                        txtInstrumentNo.TabIndex = TabIndexDr;
                                        TabIndexDr++;
                                    }
                                    else if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 1)//2:Credit //In JV Template Credit=1
                                    {
                                        txtInstrumentNo.ID = "dic" + crCount.ToString("0000") + "52" + "set" + "2" + "03" + "id" + crCount.ToString("0000") + "cr";
                                        txtInstrumentNo.TabIndex = TabIndexCr;
                                        TabIndexCr++;
                                    }

                                    txtInstrumentNo.Text = string.Empty;
                                    string instrumentNo = GetLocalResourceObject("InstrumentNo").ToString();
                                    //Set "Instrument No" if their is no text in the textbox with a watermark style. Otherwise remove this style
                                    txtInstrumentNo.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + instrumentNo + "';$(this).addClass('input-watermark');}");
                                    txtInstrumentNo.Attributes.Add("onfocus", "if (this.value == '" + instrumentNo + "') {this.value = '';$(this).removeClass('input-watermark');}");
                                    if (string.IsNullOrEmpty(txtInstrumentNo.Text.Trim()))
                                        txtInstrumentNo.Text = instrumentNo;
                                    journalScript = journalScript + "if ($('#" + txtInstrumentNo.ID + "').val() == '" + instrumentNo + "') {$('#" + txtInstrumentNo.ID + "').addClass('input-watermark');}";

                                    if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                                    {
                                        if (finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("Bank").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PDC").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PPC").ToString()))
                                        {
                                            txtInstrumentNo.Visible = true;
                                        }
                                    }

                                    div.Controls.Add(txtInstrumentNo);
                                    if (txtInstrumentNo != null && txtInstrumentNo.ID != null)
                                    {
                                        if (txtInstrumentNo.ID.EndsWith("dr"))
                                        {
                                            if (!dicDrControls.ContainsKey(txtInstrumentNo.ID))
                                                dicDrControls.Add(txtInstrumentNo.ID, "InstrumentNo");
                                        }
                                        else if (txtInstrumentNo.ID.EndsWith("cr"))
                                        {
                                            if (!dicCrControls.ContainsKey(txtInstrumentNo.ID))
                                                dicCrControls.Add(txtInstrumentNo.ID, "InstrumentNo");
                                        }
                                    }
                                    #endregion
                                    #region Date
                                    //Date Picker Control
                                    TextBox txtDate = new TextBox()
                                    {
                                        Text = "",
                                        MaxLength = 11,
                                        ClientIDMode = ClientIDMode.Static,
                                        Enabled = true,
                                        Visible = false,
                                        CssClass = "input-w8-5per"
                                    };

                                    if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 2)//1:Debit //In JV Template Debit=2
                                    {
                                        txtDate.ID = "dic" + drCount.ToString("0000") + "53" + "set" + "2" + "04" + "id" + drCount.ToString("0000") + "dr";
                                        txtDate.TabIndex = TabIndexDr;
                                        TabIndexDr++;
                                    }
                                    else if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 1)//2:Credit //In JV Template Credit=1
                                    {
                                        txtDate.ID = "dic" + crCount.ToString("0000") + "53" + "set" + "2" + "04" + "id" + crCount.ToString("0000") + "cr";
                                        txtDate.TabIndex = TabIndexCr;
                                        TabIndexCr++;
                                    }

                                    txtDate.Text = string.Empty;
                                    txtDate.Attributes.Add("onkeydown", "return CheckKey(event);");
                                    txtDate.Attributes.Add("onpaste", "return false;");

                                    string date = GetLocalResourceObject("Date").ToString();
                                    //Set "Date" if their is no text in the textbox with a watermark style. Otherwise remove this style
                                    txtDate.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + date + "';$(this).addClass('input-watermark');}");
                                    txtDate.Attributes.Add("onfocus", "if (this.value == '" + date + "') {this.value = '';$(this).removeClass('input-watermark');}");
                                    if (string.IsNullOrEmpty(txtDate.Text.Trim()))
                                        txtDate.Text = date;
                                    journalScript = journalScript + "if ($('#" + txtDate.ID + "').val() == '" + date + "') {$('#" + txtDate.ID + "').addClass('input-watermark');}";

                                    if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                                    {
                                        if (finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("Bank").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PDC").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PPC").ToString()))
                                        {
                                            txtDate.Visible = true;
                                        }
                                    }


                                    if (txtDate.Visible == true)
                                    {
                                        if (!string.IsNullOrEmpty(txtDate.ID))
                                        {
                                            string PageScript = CommonFunctions.GenerateDynamicScript("Date", txtDate.ID, null, null, null);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtDate.ID + "", PageScript, true);
                                        }
                                    }
                                    div.Controls.Add(txtDate);

                                    if (txtDate != null && txtDate.ID != null)
                                    {
                                        if (txtDate.ID.EndsWith("dr"))
                                        {
                                            if (!dicDrControls.ContainsKey(txtDate.ID))
                                                dicDrControls.Add(txtDate.ID, "Date");
                                        }
                                        else if (txtDate.ID.EndsWith("cr"))
                                        {
                                            if (!dicCrControls.ContainsKey(txtDate.ID))
                                                dicCrControls.Add(txtDate.ID, "Date");
                                        }
                                    }

                                    #endregion
                                    #region FavourOf
                                    //Favourof Textbox
                                    TextBox txtFavourOf = new TextBox()
                                    {
                                        Text = "",
                                        MaxLength = 150,
                                        ClientIDMode = ClientIDMode.Static,
                                        Enabled = true,
                                        Visible = false,
                                        CssClass = "input-w23-6per"
                                    };

                                    if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 2)//1:Debit //In JV Template Debit=2
                                    {
                                        txtFavourOf.ID = "dic" + drCount.ToString("0000") + "54" + "set" + "2" + "05" + "id" + drCount.ToString("0000") + "dr";
                                        txtFavourOf.TabIndex = TabIndexDr;
                                        TabIndexDr++;
                                    }
                                    else if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 1)//2:Credit //In JV Template Credit=1
                                    {
                                        txtFavourOf.ID = "dic" + crCount.ToString("0000") + "54" + "set" + "2" + "05" + "id" + crCount.ToString("0000") + "cr";
                                        txtFavourOf.TabIndex = TabIndexCr;
                                        TabIndexCr++;
                                    }

                                    txtFavourOf.Text = string.Empty;

                                    string favourOf = GetLocalResourceObject("FavourOf").ToString();
                                    //Set "FavourOf" if their is no text in the textbox with a watermark style. Otherwise remove this style
                                    txtFavourOf.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + favourOf + "';$(this).addClass('input-watermark');}");
                                    txtFavourOf.Attributes.Add("onfocus", "if (this.value == '" + favourOf + "') {this.value = '';$(this).removeClass('input-watermark');}");
                                    if (string.IsNullOrEmpty(txtFavourOf.Text.Trim()))
                                        txtFavourOf.Text = favourOf;
                                    journalScript = journalScript + "if ($('#" + txtFavourOf.ID + "').val() == '" + favourOf + "') {$('#" + txtFavourOf.ID + "').addClass('input-watermark');}";

                                    if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                                    {
                                        if (finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("Bank").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PDC").ToString()) || finCoaSubTypeCfgList[0].CST_CODE.Equals(GetLocalResourceObject("PPC").ToString()))
                                        {
                                            txtFavourOf.Visible = true;
                                        }
                                    }

                                    div.Controls.Add(txtFavourOf);
                                    if (txtFavourOf != null && txtFavourOf.ID != null)
                                    {
                                        if (txtFavourOf.ID.EndsWith("dr"))
                                        {
                                            if (!dicDrControls.ContainsKey(txtFavourOf.ID))
                                                dicDrControls.Add(txtFavourOf.ID, "FavourOf");
                                        }
                                        else if (txtFavourOf.ID.EndsWith("cr"))
                                        {
                                            if (!dicCrControls.ContainsKey(txtFavourOf.ID))
                                                dicCrControls.Add(txtFavourOf.ID, "FavourOf");
                                        }
                                    }
                                    #endregion

                                    tcControl.Controls.Add(div);//Adding to Table Cell
                                    trControls.Cells.Add(tcControl);//Adding to Table Row
                                    tbControls.Rows.Add(trControls);//Adding to Table

                                    if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 2 && !hasDR) //ToString()) == 1 && !hasDR //In JV Template Debit=2 
                                    {
                                        divGroupDr.Controls.Add(tbControls);//Adding to Masater Div that holds all the Debit Groups
                                        drCount++;
                                    }
                                    else if (Convert.ToInt32(templateRow["VLD_MODE"].ToString()) == 1 && !hasCR) //ToString()) == 2 && !hasCR //In JV Template Credit=1
                                    {
                                        divGroupCr.Controls.Add(tbControls);//Adding to Masater Div that holds all the Credit Groups
                                        crCount++;
                                    }
                                }

                            }

                            if (dicDrControls != null && dicDrControls.Count > 0)
                            {
                                Session[ERP.Utilities.SessionStrings.DrControls] = dicDrControls;//Updating the Debit Control Session
                            }
                            if (dicCrControls != null && dicCrControls.Count > 0)
                            {
                                Session[ERP.Utilities.SessionStrings.CrControls] = dicCrControls;//Updating the Credit Control Session
                            }
                            if (dicControlinfo != null && dicControlinfo.Count > 0)
                            {
                                Session[ERP.Utilities.SessionStrings.ControlInfo] = dicControlinfo;//Updating the Control Info. Session
                            }
                            if (dicAccountType != null && dicAccountType.Count > 0)
                            {
                                Session[ERP.Utilities.SessionStrings.AccountType] = dicAccountType;//Updating the Account Name Session
                            }

                        }
                        #endregion
                    }
                    else
                    {
                        #region New
                        #region Add Debit
                        CostCenterList = null;
                        CostCenterTempList = null;
                        //get the maximum group no from dr section. create the next id. check whether this is in removed session or not. if yes create next one . check again and again. after all this create new id.
                        divColStyle = "divcolmiddle-S1 input-margin2";
                        div = new HtmlGenericControl("div");//This div will hold the drcontrols that we are going to create.
                        div.Attributes.Add("class", divColStyle);//set the style for the div
                        divValidation = new HtmlGenericControl("div");//This div will hold all the validation controls that we are going to create.
                        divValidation.Attributes.Add("class", "starwrap");//set the style for the div
                        tbControls = new Table();
                        tbControls.CssClass = "";
                        trControls = new TableRow();
                        tcControl = new TableCell();
                        #region SubType
                        Label txtSubType = new Label()//Account Type
                        {
                            Text = "",
                            ClientIDMode = ClientIDMode.Static
                        };

                        txtSubType.ID = "dic" + drCount.ToString("0000") + "01" + "set" + "1" + "01" + "id" + drCount.ToString("0000") + "dr";
                        txtSubType.Text = GetLocalResourceObject("Debit_Account").ToString();
                        //Associate control id will be the id of Account Textbox. We can get the this id by just incrementing the control index in the set from the Account type ControlID
                        txtSubType.AssociatedControlID = "dic" + drCount.ToString("0000") + "02" + "set" + "1" + "02" + "id" + drCount.ToString("0000") + "dr";
                        div.Controls.Add(txtSubType);
                        if (txtSubType != null)
                        {
                            if (!dicDrControls.ContainsKey(txtSubType.ID))
                                dicDrControls.Add(txtSubType.ID, "SubType");//Add to Active dr Controls List
                            if (!dicAccountType.ContainsKey(txtSubType.ID))
                                dicAccountType.Add(txtSubType.ID, txtSubType.Text);//Add Account Name in dictionary
                        }
                        #endregion
                        #region Account
                        //It is an autocomplte control with postback. So we need Textbox,Hiddenfield and a Button.
                        TextBox txtAccount = new TextBox()
                        {
                            Text = "",
                            MaxLength = 100,
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexDr,
                            Enabled = true
                        };
                        HiddenField hdfAccount = new HiddenField
                        {
                            ClientIDMode = ClientIDMode.Static
                        };
                        Button btnAccount = new Button
                        {
                            ClientIDMode = ClientIDMode.Static,
                            CommandName = "JOURNALACCOUNTINDEXCHANGED",
                            EnableTheming = false
                        };
                        btnAccount.Attributes.Add("style", "display:none;");

                        txtAccount.ID = "dic" + drCount.ToString("0000") + "02" + "set" + "1" + "02" + "id" + drCount.ToString("0000") + "dr";
                        hdfAccount.ID = "dic" + drCount.ToString("0000") + "03" + "set" + "1" + "03" + "id" + drCount.ToString("0000") + "dr";
                        btnAccount.ID = "dic" + drCount.ToString("0000") + "04" + "set" + "1" + "04" + "id" + drCount.ToString("0000") + "dr";
                        hdfSubTypePk.Value = "0";

                        if (!dicControlinfo.ContainsKey(txtAccount.ID + "SubTypePk"))//Store Subtype PK
                            dicControlinfo.Add(txtAccount.ID + "SubTypePk", hdfSubTypePk.Value);
                        //Register Autocomplete script
                        journalScript = journalScript + "GrandScriptUtils.MakeAutoCompleteDDL('" + txtAccount.ID
                                           + "', (url1.indexOf('?') != -1 ? url1+'&' :  url1+'?') + 'AccType="
                                           + hdfSubTypePk.Value + "', '" + hdfAccount.ID + "', true, true, 'JOURNALACCOUNT',false,false,false);";
                        //Register Button Event
                        btnAccount.Click += new EventHandler(ActionHandler);
                        div.Controls.Add(txtAccount);
                        div.Controls.Add(hdfAccount);
                        div.Controls.Add(btnAccount);

                        RequiredFieldValidator vrfAccount = new RequiredFieldValidator()
                        {
                            ID = "vrf" + txtAccount.ID.Replace("dic", ""),
                            ControlToValidate = txtAccount.ID,
                            Text = "*",
                            CssClass = "star",
                            ValidationGroup = "voucher",
                            Display = ValidatorDisplay.Dynamic,
                            EnableClientScript = true,
                            InitialValue = Resources.Messages.AutoDefaultValue,
                            SetFocusOnError = true,
                            ErrorMessage = GetLocalResourceObject("Err_Account").ToString(),
                            ClientIDMode = ClientIDMode.Static
                        };
                        //Add validation control to validation div
                        divValidation.Controls.Add(vrfAccount);


                        TabIndexDr++;
                        if (txtAccount != null)
                            if (!dicDrControls.ContainsKey(txtAccount.ID))
                                dicDrControls.Add(txtAccount.ID, "Account");//Add to Active dr Controls List
                        if (hdfAccount != null)
                            if (!dicDrControls.ContainsKey(hdfAccount.ID))
                                dicDrControls.Add(hdfAccount.ID, "Account");//Add to Active dr Controls List
                        if (btnAccount != null)
                            if (!dicDrControls.ContainsKey(btnAccount.ID))
                                dicDrControls.Add(btnAccount.ID, "Account");//Add to Active dr Controls List
                        #endregion
                        #region Narration
                        //Narration Textbox Control
                        TextBox txtNarration = new TextBox()
                        {
                            Text = "",
                            MaxLength = 400,
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexDr,
                            Enabled = true,
                            Width = IsSameCurrency() == true ? new Unit(GetLocalResourceObject("NarationWidthWithoutAmountBC").ToString()) : new Unit(GetLocalResourceObject("NarationWidthNormal").ToString())
                        };

                        txtNarration.ID = "dic" + drCount.ToString("0000") + "05" + "set" + "1" + "05" + "id" + drCount.ToString("0000") + "dr";
                        div.Controls.Add(txtNarration);//Add controls to div
                        TabIndexDr++;
                        if (txtNarration != null)
                        {
                            if (!dicDrControls.ContainsKey(txtNarration.ID))
                                dicDrControls.Add(txtNarration.ID, "Narration");//Add to Active dr Controls List
                        }
                        #endregion
                        #region AmountTC
                        //Amount in Transaction Currency
                        TextBox txtAmountTC = new TextBox()
                        {
                            Text = "",
                            MaxLength = 15,
                            ClientIDMode = ClientIDMode.Static,
                            Enabled = true,
                            TabIndex = TabIndexDr,
                            CssClass = "input-w8per numeric amounttcdr tempcss"
                        };
                        txtAmountTC.ID = "dic" + drCount.ToString("0000") + "06" + "set" + "1" + "06" + "id" + drCount.ToString("0000") + "dr";
                        txtAmountTC.Text = ((decimal)0).ToString(hdfCurrencyFormatVoucher.Value);
                        txtAmountTC.Attributes.Add("onkeyup", "CalculateBCAmt(this);");//script registration for Calculate BC
                        div.Controls.Add(txtAmountTC);

                        AmountValidation vamAmountTC = new AmountValidation()
                        {
                            ID = "vam" + txtAmountTC.ID.Replace("dic", ""),
                            ControlToValidate = txtAmountTC.ID,
                            ErrorMessage = GetLocalResourceObject("MsgErr_AmountTc").ToString(),
                            NumberDigits = 11,
                            Display = ValidatorDisplay.Dynamic,
                            Text = "*",
                            EnableClientScript = true,
                            CssClass = "star",
                            ValidationGroup = "voucher",
                            NonZero = true
                        };
                        divValidation.Controls.Add(vamAmountTC);
                        if (!string.IsNullOrEmpty(txtAmountTC.ID))
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtAmountTC.ID + "", "$('[id$=" + txtAmountTC.ID + "]').ForceNumericOnly();", true);
                        TabIndexDr++;
                        if (txtAmountTC != null)
                        {
                            if (!dicDrControls.ContainsKey(txtAmountTC.ID))
                                dicDrControls.Add(txtAmountTC.ID, "AmountTC");
                        }
                        #endregion
                        #region ExchangeRate
                        //Exchange Rate
                        TextBox txtExchangeRate = new TextBox()
                        {
                            Text = "",
                            MaxLength = 9,
                            ClientIDMode = ClientIDMode.Static,
                            Enabled = true,
                            TabIndex = TabIndexDr,
                            CssClass = "numeric input-w6per"
                        };
                        HiddenField hdfEntryMode = new HiddenField()
                        {
                            ClientIDMode = ClientIDMode.Static
                        };
                        txtExchangeRate.ID = "dic" + drCount.ToString("0000") + "07" + "set" + "1" + "07" + "id" + drCount.ToString("0000") + "dr";
                        txtExchangeRate.Text = !string.IsNullOrEmpty(txtJournalExchangeRate.Text.Trim())
                            ? ERP.Utilities.CommonFunctions.DoubleFormat(Convert.ToDouble(txtJournalExchangeRate.Text.Trim()), exchRateDecimalDigits).ToString()
                            : 1.ToString(hdfExchRateFormatVoucher.Value);
                        hdfEntryMode.ID = txtExchangeRate.ID + "EntryMode";//Used for to keep the entrymode of ExchangeRate
                        txtExchangeRate.Attributes.Add("onkeyup", "CalculateBCWithER(this);");//Register script for calculating BC with Exchange Rate
                        if (!string.IsNullOrEmpty(txtExchangeRate.ID))
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtExchangeRate.ID + "", "$('[id$=" + txtExchangeRate.ID + "]').ForceNumericOnly();", true);

                        TabIndexDr++;
                        if (!dicControlinfo.ContainsKey(txtExchangeRate.ID + "EntryMode"))
                            dicControlinfo.Add(txtExchangeRate.ID + "EntryMode", ((byte)VoucherEntryMode.Editable).ToString());//Keep the entrymode for the curresponding Exchange Rate in Dictionary
                        hdfEntryMode.Value = ((byte)VoucherEntryMode.Editable).ToString();//Default EntryMode is Editable
                        if (txtExchangeRate != null)
                        {
                            if (!dicDrControls.ContainsKey(txtExchangeRate.ID))
                                dicDrControls.Add(txtExchangeRate.ID, "ExchangeRate");
                            if (hdfEntryMode != null)
                            {
                                if (!dicDrControls.ContainsKey(hdfEntryMode.ID))
                                    dicDrControls.Add(hdfEntryMode.ID, "EntryMode");
                            }
                        }
                        transactionCurrency = 0;
                        if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                            transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                        else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                            transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);

                        if (transactionCurrency > 0 && transactionCurrency == currentUser.BaseCurrency)
                            txtExchangeRate.Visible = false;
                        div.Controls.Add(txtExchangeRate);
                        div.Controls.Add(hdfEntryMode);
                        ExchangeRateValidation vreExchangeRate = new ExchangeRateValidation()
                        {
                            ID = "vre" + txtExchangeRate.ID,
                            ControlToValidate = txtExchangeRate.ID,
                            ErrorMessage = GetLocalResourceObject("MsgErr_ExchangeRate").ToString(),
                            NumberDigits = 5,
                            Display = ValidatorDisplay.Dynamic,
                            Text = "*",
                            EnableClientScript = true,
                            CssClass = "star",
                            ValidationGroup = "voucher",
                            NonZero = true
                        };
                        divValidation.Controls.Add(vreExchangeRate);
                        #endregion
                        #region AmountBC
                        //Amount in Base Currency
                        TextBox txtAmountBC = new TextBox()
                        {
                            Text = "",
                            ClientIDMode = ClientIDMode.Static,
                            Enabled = true,
                            MaxLength = 15,
                            CssClass = "input-normalb input-w8per numeric tempcss"
                        };

                        txtAmountBC.ID = "dic" + drCount.ToString("0000") + "08" + "set" + "1" + "08" + "id" + drCount.ToString("0000") + "dr";
                        txtAmountBC.Text = ((decimal)0).ToString(hdfCurrencyFormatVoucher.Value);

                        if ((BCEnable && hdfEntryMode.Value == ((byte)VoucherEntryMode.Editable).ToString()) || IsYearEndVoucher() || IsEditableForApplication())//If BC is Enable and Entrymode is Editable
                        {
                            txtAmountBC.CssClass = "input-w8per numeric tempcss";
                            txtAmountBC.Attributes.Add("onkeyup", "CalculateTCAmt(this,event);");//script registration for Calculate TC
                            if (!string.IsNullOrEmpty(txtAmountBC.ID))
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtAmountBC.ID + "", "$('[id$=" + txtAmountBC.ID + "]').ForceNumericOnly();", true);
                        }
                        else//Disable Amount BC
                        {
                            txtAmountBC.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                            txtAmountBC.Attributes.Add("onpaste", "return false;");
                        }

                        div.Controls.Add(txtAmountBC);

                        AmountValidation vamAmountBC = new AmountValidation()
                        {
                            ID = "vam" + txtAmountBC.ID.Replace("dic", ""),
                            ControlToValidate = txtAmountBC.ID,
                            ErrorMessage = GetLocalResourceObject("MsgErr_AmountBc").ToString(),
                            NumberDigits = 11,
                            Display = ValidatorDisplay.Dynamic,
                            Text = "*",
                            EnableClientScript = true,
                            CssClass = "star",
                            ValidationGroup = "voucher"
                        };
                        divValidation.Controls.Add(vamAmountBC);

                        if (txtAmountBC != null)
                        {
                            if (!dicDrControls.ContainsKey(txtAmountBC.ID))
                                dicDrControls.Add(txtAmountBC.ID, "AmountBC");
                        }
                        transactionCurrency = 0;
                        if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                            transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                        else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                            transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);

                        if (transactionCurrency > 0 && transactionCurrency == currentUser.BaseCurrency)
                            txtAmountBC.Visible = false;
                        #endregion
                        #region Delete
                        //Delete button for deleting the Curresponding DR section
                        Button btnDelete = new Button()
                        {
                            Text = "",
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexDr,
                            Enabled = true,
                            SkinID = "delete-icon",
                            ToolTip = "Delete",
                            OnClientClick = "return ShowDeleteConfirm(this);"
                        };
                        btnDelete.ID = "dic" + drCount.ToString("0000") + "09" + "set" + "1" + "09" + "id" + drCount.ToString("0000") + "dr";
                        btnDelete.CommandName = ActionsEnum.REMOVEDEBIT.ToString();
                        TabIndexDr++;
                        btnDelete.Click += new EventHandler(ActionHandler);
                        if (EntryStatus == EntryStatus.VIEWMODE)
                        {
                            btnDelete.Visible = false;
                        }
                        div.Controls.Add(btnDelete);
                        ////divValidation contains all the Validation Controls in a group. It will be added to the group after delete button. So if any validation catch it will show * after delete button
                        //div.Controls.Add(divValidation);
                        if (btnDelete != null)
                        {
                            if (!dicDrControls.ContainsKey(btnDelete.ID))
                                dicDrControls.Add(btnDelete.ID, "Delete");
                        }
                        #endregion
                        #region Cost Center
                        //Delete button for deleting the Curresponding DR section
                        Button btnCostCenter = new Button()
                        {
                            Text = "",
                            ClientIDMode = ClientIDMode.Static,
                            Visible = false,
                            TabIndex = TabIndexDr,
                            Enabled = true,
                            SkinID = "costcenter-icon",
                            ToolTip = GetLocalResourceObject("CCButtonToolTip").ToString()
                        };
                        HiddenField hdfIsCostCenter = new HiddenField
                        {
                            ClientIDMode = ClientIDMode.Static,
                            Value = "0"
                        };
                        btnCostCenter.ID = "dic" + drCount.ToString("0000") + "10" + "set" + "1" + "10" + "id" + drCount.ToString("0000") + "dr";
                        hdfIsCostCenter.ID = "dic" + drCount.ToString("0000") + "11" + "set" + "1" + "11" + "id" + drCount.ToString("0000") + "dr";
                        btnCostCenter.CommandName = ActionsEnum.COSTCENTER.ToString();
                        TabIndexDr++;
                        btnCostCenter.Click += new EventHandler(ActionHandler);
                        div.Controls.Add(btnCostCenter);

                        //divValidation contains all the Validation Controls in a group. It will be added to the group after delete button. So if any validation catch it will show * after delete button
                        div.Controls.Add(divValidation);

                        div.Controls.Add(hdfIsCostCenter);
                        if (btnCostCenter != null)
                            if (!dicDrControls.ContainsKey(btnCostCenter.ID))
                                dicDrControls.Add(btnCostCenter.ID, "CostCenter");
                        if (hdfIsCostCenter != null)
                            if (!dicDrControls.ContainsKey(hdfIsCostCenter.ID))
                                dicDrControls.Add(hdfIsCostCenter.ID, "CostCenter");

                        #endregion
                        #region SubledgerLabel
                        //After Delete Button, the rest of the controls will comes in the next line. So the divClear will use to break the first line
                        divClear = new HtmlGenericControl("div");
                        divClear.Attributes.Add("class", "clear");
                        div.Controls.Add(divClear);
                        //It is a dummy label to adjust the style
                        Label lblSubledgerLabel = new Label()
                        {
                            ClientIDMode = ClientIDMode.Static,
                            Text = "&nbsp",
                            Visible = false
                        };

                        lblSubledgerLabel.ID = "dic" + drCount.ToString("0000") + "50" + "set" + "2" + "01" + "id" + drCount.ToString("0000") + "dr";
                        //The Associate control id is the id of subledger dropdownlist
                        lblSubledgerLabel.AssociatedControlID = "dic" + drCount.ToString("0000") + "51" + "set" + "2" + "02" + "id" + drCount.ToString("0000") + "dr";

                        div.Controls.Add(lblSubledgerLabel);
                        if (lblSubledgerLabel != null)
                        {
                            if (!dicDrControls.ContainsKey(lblSubledgerLabel.ID))
                                dicDrControls.Add(lblSubledgerLabel.ID, "SubledgerLabel");
                        }
                        #endregion
                        #region SubLedger
                        //Subledger ddl control
                        DropDownList ddlSubLedger = new DropDownList()
                        {
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexDr,
                            Visible = false
                        };

                        ddlSubLedger.ID = "dic" + drCount.ToString("0000") + "51" + "set" + "2" + "02" + "id" + drCount.ToString("0000") + "dr";
                        div.Controls.Add(ddlSubLedger);
                        RequiredFieldValidator vrfSubLedger = new RequiredFieldValidator()
                        {
                            ID = "vrf" + ddlSubLedger.ID.Replace("dic", ""),
                            ControlToValidate = ddlSubLedger.ID,
                            Text = "*",
                            CssClass = "star",
                            ValidationGroup = "voucher",
                            Display = ValidatorDisplay.Dynamic,
                            EnableClientScript = true,
                            InitialValue = CommonConstants.SELECTVAL,
                            SetFocusOnError = true,
                            ErrorMessage = GetLocalResourceObject("Err_SubLedger").ToString(),
                            ClientIDMode = ClientIDMode.Static
                        };
                        divValidation.Controls.Add(vrfSubLedger);
                        vrfSubLedger.Enabled = ddlSubLedger.Visible ? true : false;
                        TabIndexDr++;
                        if (ddlSubLedger != null)
                        {
                            if (!dicDrControls.ContainsKey(ddlSubLedger.ID))
                                dicDrControls.Add(ddlSubLedger.ID, "SubLedger");
                        }
                        #endregion
                        #region InstrumentNo
                        //Instrument No Text box
                        TextBox txtInstrumentNo = new TextBox()
                        {
                            Text = "",
                            MaxLength = 70,
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexDr,
                            Enabled = true,
                            Visible = false,
                            CssClass = "input16"
                        };
                        txtInstrumentNo.ID = "dic" + drCount.ToString("0000") + "52" + "set" + "2" + "03" + "id" + drCount.ToString("0000") + "dr";
                        string instrumentNo = GetLocalResourceObject("InstrumentNo").ToString();
                        //Set "Instrument No" if their is no text in the textbox with a watermark style. Otherwise remove this style
                        txtInstrumentNo.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + instrumentNo + "';$(this).addClass('input-watermark');}");
                        txtInstrumentNo.Attributes.Add("onfocus", "if (this.value == '" + instrumentNo + "') {this.value = '';$(this).removeClass('input-watermark');}");
                        journalScript = journalScript + "if ($('#" + txtInstrumentNo.ID + "').val() == '" + instrumentNo + "') {$('#" + txtInstrumentNo.ID + "').addClass('input-watermark');}";
                        txtInstrumentNo.Text = instrumentNo;

                        div.Controls.Add(txtInstrumentNo);
                        TabIndexDr++;
                        if (txtInstrumentNo != null)
                        {
                            if (!dicDrControls.ContainsKey(txtInstrumentNo.ID))
                                dicDrControls.Add(txtInstrumentNo.ID, "InstrumentNo");
                        }
                        #endregion
                        #region Date
                        //Date Picker Control
                        TextBox txtDate = new TextBox()
                        {
                            Text = "",
                            MaxLength = 11,
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexDr,
                            Enabled = true,
                            Visible = false,
                            CssClass = "input-w8-5per"
                        };
                        txtDate.ID = "dic" + drCount.ToString("0000") + "53" + "set" + "2" + "04" + "id" + drCount.ToString("0000") + "dr";
                        string date = GetLocalResourceObject("Date").ToString();
                        //Set "Date" if their is no text in the textbox with a watermark style. Otherwise remove this style
                        txtDate.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + date + "'; $(this).addClass('input-watermark');}");
                        txtDate.Attributes.Add("onfocus", "if (this.value == '" + date + "') {this.value = ''; $(this).removeClass('input-watermark');}");
                        txtDate.Text = date;
                        journalScript = journalScript + "if ($('#" + txtDate.ID + "').val() == '" + date + "') {$('#" + txtDate.ID + "').addClass('input-watermark');}";


                        txtDate.Attributes.Add("onkeydown", "return CheckKey(event);");
                        txtDate.Attributes.Add("onpaste", "return false;");
                        if (txtDate.Visible == true)
                        {
                            if (!string.IsNullOrEmpty(txtDate.ID))
                            {
                                string PageScript = CommonFunctions.GenerateDynamicScript("Date", txtDate.ID, null, null, null);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtDate.ID + "", PageScript, true);
                            }
                        }
                        div.Controls.Add(txtDate);
                        TabIndexDr++;
                        if (txtDate != null)
                        {
                            if (!dicDrControls.ContainsKey(txtDate.ID))
                                dicDrControls.Add(txtDate.ID, "Date");
                        }
                        #endregion
                        #region FavourOf
                        //Favourof Textbox
                        TextBox txtFavourOf = new TextBox()
                        {
                            MaxLength = 150,
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexDr,
                            Enabled = true,
                            Visible = false,
                            CssClass = "input-w23-6per"
                        };
                        txtFavourOf.ID = "dic" + drCount.ToString("0000") + "54" + "set" + "2" + "05" + "id" + drCount.ToString("0000") + "dr";
                        string favourOf = GetLocalResourceObject("FavourOf").ToString();
                        //Set "FavourOf" if their is no text in the textbox with a watermark style. Otherwise remove this style
                        txtFavourOf.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + favourOf + "';$(this).addClass('input-watermark');}");
                        txtFavourOf.Attributes.Add("onfocus", "if (this.value == '" + favourOf + "') {this.value = '';$(this).removeClass('input-watermark');}");
                        txtFavourOf.Text = favourOf;
                        journalScript = journalScript + "if ($('#" + txtFavourOf.ID + "').val() == '" + favourOf + "') {$('#" + txtFavourOf.ID + "').addClass('input-watermark');}";


                        div.Controls.Add(txtFavourOf);
                        TabIndexDr++;
                        if (txtFavourOf != null)
                        {
                            if (!dicDrControls.ContainsKey(txtFavourOf.ID))
                                dicDrControls.Add(txtFavourOf.ID, "FavourOf");
                        }
                        #endregion
                        //Now the div contains the new added Debit controls. Now we are going to add it to our page
                        tcControl.Controls.Add(div);//Adding to Table Cell
                        trControls.Cells.Add(tcControl);//Adding to Table Row
                        tbControls.Rows.Add(trControls);//Adding to Table
                        divGroupDr.Controls.Add(tbControls);//Adding to Masater Div that holds all the Debit Groups
                        if (dicDrControls != null && dicDrControls.Count > 0)
                        {
                            Session[ERP.Utilities.SessionStrings.DrControls] = dicDrControls;//Updating the Debit Control Session
                        }
                        if (dicControlinfo != null && dicControlinfo.Count > 0)
                        {
                            Session[ERP.Utilities.SessionStrings.ControlInfo] = dicControlinfo;//Updating the Control Info. Session
                        }
                        if (dicAccountType != null && dicAccountType.Count > 0)
                        {
                            Session[ERP.Utilities.SessionStrings.AccountType] = dicAccountType;//Updating the Account Name Session
                        }
                        #endregion
                        #region Add Credit
                        divColStyle = "divcolmiddle-S1 input-margin2";
                        div = new HtmlGenericControl("div");//This div will hold the crcontrols that we are going to create.
                        div.Attributes.Add("class", divColStyle);//set the style for the div
                        divValidation = new HtmlGenericControl("div");//This div will hold all the validation controls that we are going to create.
                        divValidation.Attributes.Add("class", "starwrap");//set the style for the div
                        tbControls = new Table();
                        tbControls.CssClass = "";
                        trControls = new TableRow();
                        tcControl = new TableCell();
                        #region SubType
                        //Account Type
                        Label txtSubTypeCr = new Label()
                        {
                            Text = "",
                            ClientIDMode = ClientIDMode.Static
                        };

                        txtSubTypeCr.ID = "dic" + crCount.ToString("0000") + "01" + "set" + "1" + "01" + "id" + crCount.ToString("0000") + "cr";
                        txtSubTypeCr.Text = GetLocalResourceObject("Credit_Account").ToString();
                        //Associate control id will be the id of Account Textbox. We can get the this id by just incrementing the control index in the set from the Account type ControlID
                        txtSubTypeCr.AssociatedControlID = "dic" + crCount.ToString("0000") + "02" + "set" + "1" + "02" + "id" + crCount.ToString("0000") + "cr";
                        div.Controls.Add(txtSubTypeCr);
                        if (txtSubTypeCr != null)
                        {
                            if (!dicCrControls.ContainsKey(txtSubTypeCr.ID))
                                dicCrControls.Add(txtSubTypeCr.ID, "SubType");//Add to Active dr Controls List
                            if (!dicAccountType.ContainsKey(txtSubTypeCr.ID))
                                dicAccountType.Add(txtSubTypeCr.ID, txtSubTypeCr.Text);//Add Account Name in dictionary
                        }
                        #endregion
                        #region Account
                        //It is an autocomplte control with postback. So we need Textbox,Hiddenfield and a Button.
                        TextBox txtAccountCr = new TextBox()
                        {
                            Text = "",
                            MaxLength = 100,
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexCr,
                            Enabled = true
                        };
                        HiddenField hdfAccountCr = new HiddenField
                        {
                            ClientIDMode = ClientIDMode.Static
                        };
                        Button btnAccountCr = new Button
                        {
                            ClientIDMode = ClientIDMode.Static,
                            CommandName = "JOURNALACCOUNTINDEXCHANGED",
                            EnableTheming = false
                        };
                        btnAccountCr.Attributes.Add("style", "display:none;");

                        txtAccountCr.ID = "dic" + crCount.ToString("0000") + "02" + "set" + "1" + "02" + "id" + crCount.ToString("0000") + "cr";
                        hdfAccountCr.ID = "dic" + crCount.ToString("0000") + "03" + "set" + "1" + "03" + "id" + crCount.ToString("0000") + "cr";
                        btnAccountCr.ID = "dic" + crCount.ToString("0000") + "04" + "set" + "1" + "04" + "id" + crCount.ToString("0000") + "cr";


                        hdfSubTypePk.Value = "0";
                        if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.PCS)//If it is Pettycash voucher
                        {
                            hdfSubTypePk.Value = FINCOASUBTYPECFGEnum.Cash.GetHashCode().ToString();
                        }

                        if (!dicControlinfo.ContainsKey(txtAccountCr.ID + "SubTypePk"))//Store Subtype PK
                            dicControlinfo.Add(txtAccountCr.ID + "SubTypePk", hdfSubTypePk.Value);
                        //Register Autocomplete script
                        journalScript = journalScript + "GrandScriptUtils.MakeAutoCompleteDDL('" + txtAccountCr.ID + "', (url1.indexOf('?') != -1 ? url1+'&' :  url1+'?') + 'AccType=" + hdfSubTypePk.Value + "', '" + hdfAccountCr.ID + "', true, true, 'JOURNALACCOUNT',false,false,false);";
                        //Register Button Event
                        btnAccount.Click += new EventHandler(ActionHandler);
                        //Add controls to div
                        div.Controls.Add(txtAccountCr);
                        div.Controls.Add(hdfAccountCr);
                        div.Controls.Add(btnAccountCr);

                        RequiredFieldValidator vrfAccountCr = new RequiredFieldValidator()
                        {
                            ID = "vrf" + txtAccountCr.ID.Replace("dic", ""),
                            ControlToValidate = txtAccountCr.ID,
                            Text = "*",
                            CssClass = "star",
                            ValidationGroup = "voucher",
                            Display = ValidatorDisplay.Dynamic,
                            EnableClientScript = true,
                            InitialValue = Resources.Messages.AutoDefaultValue,
                            SetFocusOnError = true,
                            ErrorMessage = GetLocalResourceObject("Err_Account").ToString(),
                            ClientIDMode = ClientIDMode.Static
                        };
                        //Add validation control to validation div
                        divValidation.Controls.Add(vrfAccountCr);

                        TabIndexCr++;
                        if (txtAccountCr != null)
                            if (!dicCrControls.ContainsKey(txtAccountCr.ID))
                                dicCrControls.Add(txtAccountCr.ID, "Account");//Add to Active cr Controls List
                        if (hdfAccountCr != null)
                            if (!dicCrControls.ContainsKey(hdfAccountCr.ID))
                                dicCrControls.Add(hdfAccountCr.ID, "Account");//Add to Active cr Controls List
                        if (btnAccountCr != null)
                            if (!dicCrControls.ContainsKey(btnAccountCr.ID))
                                dicCrControls.Add(btnAccountCr.ID, "Account");//Add to Active cr Controls List
                        #endregion
                        #region Narration
                        //Narration Textbox Control
                        TextBox txtNarrationCr = new TextBox()
                        {
                            Text = "",
                            MaxLength = 400,
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexCr,
                            Enabled = true,
                            Width = IsSameCurrency() == true ? new Unit(GetLocalResourceObject("NarationWidthWithoutAmountBC").ToString()) : new Unit(GetLocalResourceObject("NarationWidthNormal").ToString())
                        };
                        txtNarrationCr.ID = "dic" + crCount.ToString("0000") + "05" + "set" + "1" + "05" + "id" + crCount.ToString("0000") + "cr";
                        div.Controls.Add(txtNarrationCr);
                        TabIndexCr++;
                        if (txtNarrationCr != null)
                        {
                            if (!dicCrControls.ContainsKey(txtNarrationCr.ID))
                                dicCrControls.Add(txtNarrationCr.ID, "Narration");//Add to Active cr Controls List
                        }
                        #endregion
                        #region AmountTC
                        //Amount in Transaction Currency
                        TextBox txtAmountTCCr = new TextBox()
                        {
                            Text = "",
                            MaxLength = 15,
                            ClientIDMode = ClientIDMode.Static,
                            Enabled = true,
                            TabIndex = TabIndexCr,
                            CssClass = "input-w8per numeric amounttccr tempcss"
                        };
                        txtAmountTCCr.ID = "dic" + crCount.ToString("0000") + "06" + "set" + "1" + "06" + "id" + crCount.ToString("0000") + "cr";
                        txtAmountTCCr.Text = ((decimal)0).ToString(hdfCurrencyFormatVoucher.Value);
                        txtAmountTCCr.Attributes.Add("onkeyup", "CalculateBCAmt(this);");//script registration for Calculate BC
                        div.Controls.Add(txtAmountTCCr);

                        AmountValidation vamAmountTCCr = new AmountValidation()
                        {
                            ID = "vam" + txtAmountTCCr.ID.Replace("dic", ""),
                            ControlToValidate = txtAmountTCCr.ID,
                            ErrorMessage = GetLocalResourceObject("MsgErr_AmountTc").ToString(),
                            NumberDigits = 11,
                            Display = ValidatorDisplay.Dynamic,
                            Text = "*",
                            EnableClientScript = true,
                            CssClass = "star",
                            ValidationGroup = "voucher",
                            NonZero = true
                        };
                        divValidation.Controls.Add(vamAmountTCCr);

                        if (!string.IsNullOrEmpty(txtAmountTCCr.ID))
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtAmountTCCr.ID + "", "$('[id$=" + txtAmountTCCr.ID + "]').ForceNumericOnly();", true);
                        TabIndexCr++;
                        if (txtAmountTCCr != null)
                        {
                            if (!dicCrControls.ContainsKey(txtAmountTCCr.ID))
                                dicCrControls.Add(txtAmountTCCr.ID, "AmountTC");
                        }
                        #endregion
                        #region ExchangeRate
                        //Exchange Rate
                        TextBox txtExchangeRateCr = new TextBox()
                        {
                            Text = "",
                            MaxLength = 9,
                            ClientIDMode = ClientIDMode.Static,
                            Enabled = true,
                            TabIndex = TabIndexCr,
                            CssClass = "numeric input-w6per"
                        };
                        HiddenField hdfEntryModeCr = new HiddenField()
                        {
                            ClientIDMode = ClientIDMode.Static
                        };
                        txtExchangeRateCr.ID = "dic" + crCount.ToString("0000") + "07" + "set" + "1" + "07" + "id" + crCount.ToString("0000") + "cr";
                        txtExchangeRateCr.Text = !string.IsNullOrEmpty(txtJournalExchangeRate.Text.Trim())
                            ? ERP.Utilities.CommonFunctions.DoubleFormat(Convert.ToDouble(txtJournalExchangeRate.Text.Trim()), exchRateDecimalDigits).ToString(hdfExchRateFormatVoucher.Value)
                            : 1.ToString(hdfExchRateFormatVoucher.Value);

                        hdfEntryModeCr.ID = txtExchangeRateCr.ID + "EntryMode";//Used for to keep the entrymode of ExchangeRate in the credit detail section
                        txtExchangeRateCr.Attributes.Add("onkeyup", "CalculateBCWithER(this);");//Register script for calculate the BC with ExchangeRate
                        if (!string.IsNullOrEmpty(txtExchangeRate.ID))
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtExchangeRate.ID + "", "$('[id$=" + txtExchangeRate.ID + "]').ForceNumericOnly();", true);
                        TabIndexCr++;
                        if (!dicControlinfo.ContainsKey(txtExchangeRateCr.ID + "EntryMode"))
                            dicControlinfo.Add(txtExchangeRateCr.ID + "EntryMode", ((byte)VoucherEntryMode.Editable).ToString());//Keep the entrymode for the curresponding Exchange Rate in Dictionary
                        hdfEntryModeCr.Value = ((byte)VoucherEntryMode.Editable).ToString();//Default EntryMode is Editable
                        if (txtExchangeRateCr != null)
                        {
                            if (!dicCrControls.ContainsKey(txtExchangeRateCr.ID))
                                dicCrControls.Add(txtExchangeRateCr.ID, "ExchangeRate");
                            if (hdfEntryMode != null)
                            {
                                if (!dicCrControls.ContainsKey(hdfEntryMode.ID))
                                    dicCrControls.Add(hdfEntryMode.ID, "EntryMode");
                            }
                        }
                        transactionCurrency = 0;
                        if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                            transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                        else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                            transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);
                        if (transactionCurrency > 0 && transactionCurrency == currentUser.BaseCurrency)
                            txtExchangeRateCr.Visible = false;
                        div.Controls.Add(txtExchangeRateCr);
                        div.Controls.Add(hdfEntryModeCr);
                        ExchangeRateValidation vreExchangeRateCr = new ExchangeRateValidation()
                        {
                            ID = "vre" + txtExchangeRateCr.ID,
                            ControlToValidate = txtExchangeRateCr.ID,
                            ErrorMessage = GetLocalResourceObject("MsgErr_ExchangeRate").ToString(),
                            NumberDigits = 5,
                            Display = ValidatorDisplay.Dynamic,
                            Text = "*",
                            EnableClientScript = true,
                            CssClass = "star",
                            ValidationGroup = "voucher",
                            NonZero = true
                        };
                        divValidation.Controls.Add(vreExchangeRateCr);
                        #endregion
                        #region AmountBC
                        //Amount in Base Currency
                        TextBox txtAmountBCCr = new TextBox()
                        {
                            Text = "",
                            ClientIDMode = ClientIDMode.Static,
                            Enabled = true,
                            MaxLength = 15,
                            CssClass = "input-normalb input-w8per numeric tempcss"
                        };

                        txtAmountBCCr.ID = "dic" + crCount.ToString("0000") + "08" + "set" + "1" + "08" + "id" + crCount.ToString("0000") + "cr";
                        txtAmountBCCr.Text = ((decimal)0).ToString(hdfCurrencyFormatVoucher.Value);

                        if (BCEnable && hdfEntryModeCr.Value == ((byte)VoucherEntryMode.Editable).ToString())//If BC is Enable and Entrymode is Editable
                        {
                            txtAmountBCCr.CssClass = "input-w8per numeric";
                            txtAmountBCCr.Attributes.Add("onkeyup", "CalculateTCAmt(this,event);");
                            if (!string.IsNullOrEmpty(txtAmountBCCr.ID))
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtAmountBCCr.ID + "", "$('[id$=" + txtAmountBCCr.ID + "]').ForceNumericOnly();", true);
                        }
                        else//Disable Amount BC
                        {
                            txtAmountBCCr.Attributes.Add("onkeydown", "return EnableArrowKey(event);");
                            txtAmountBCCr.Attributes.Add("onpaste", "return false;");
                        }

                        div.Controls.Add(txtAmountBCCr);

                        AmountValidation vamAmountBCCr = new AmountValidation()
                        {
                            ID = "vam" + txtAmountBCCr.ID.Replace("dic", ""),
                            ControlToValidate = txtAmountBCCr.ID,
                            ErrorMessage = GetLocalResourceObject("MsgErr_AmountBc").ToString(),
                            NumberDigits = 11,
                            Display = ValidatorDisplay.Dynamic,
                            Text = "*",
                            EnableClientScript = true,
                            CssClass = "star",
                            ValidationGroup = "voucher"
                        };
                        divValidation.Controls.Add(vamAmountBCCr);
                        if (txtAmountBCCr != null)
                        {
                            if (!dicCrControls.ContainsKey(txtAmountBCCr.ID))
                                dicCrControls.Add(txtAmountBCCr.ID, "AmountBC");
                        }

                        transactionCurrency = 0;
                        if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                            transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
                        else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                            transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);

                        if (transactionCurrency > 0 && transactionCurrency == currentUser.BaseCurrency)
                            txtAmountBCCr.Visible = false;
                        #endregion
                        #region Delete
                        //Delete button for deleting the Curresponding DR section
                        Button btnDeleteCr = new Button()
                        {
                            Text = "",
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexCr,
                            Enabled = true,
                            SkinID = "delete-icon",
                            ToolTip = "Delete",
                            OnClientClick = "return ShowDeleteConfirm(this);"
                        };
                        btnDeleteCr.ID = "dic" + crCount.ToString("0000") + "09" + "set" + "1" + "09" + "id" + crCount.ToString("0000") + "cr";
                        btnDeleteCr.CommandName = ActionsEnum.REMOVECREDIT.ToString();

                        btnDeleteCr.Click += new EventHandler(ActionHandler);
                        if (EntryStatus == EntryStatus.VIEWMODE)
                        {
                            btnDeleteCr.Visible = false;
                        }
                        div.Controls.Add(btnDeleteCr);
                        ////divValidation contains all the Validation Controls in a group. It will be added to the group after delete button. So if any validation catch it will show * after delete button
                        //div.Controls.Add(divValidation);
                        TabIndexCr++;
                        if (btnDeleteCr != null)
                        {
                            if (!dicCrControls.ContainsKey(btnDeleteCr.ID))
                                dicCrControls.Add(btnDeleteCr.ID, "Delete");
                        }
                        #endregion
                        #region Cost Center
                        //Delete button for deleting the Curresponding DR section
                        Button btnCostCenterCr = new Button()
                        {
                            Text = "",
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexCr,
                            Enabled = true,
                            SkinID = "costcenter-icon",
                            ToolTip = GetLocalResourceObject("CCButtonToolTip").ToString()
                        };
                        HiddenField hdfIsCostCenterCr = new HiddenField
                        {
                            ClientIDMode = ClientIDMode.Static
                        };
                        btnCostCenterCr.ID = "dic" + crCount.ToString("0000") + "10" + "set" + "1" + "10" + "id" + crCount.ToString("0000") + "cr";
                        hdfIsCostCenterCr.ID = "dic" + crCount.ToString("0000") + "11" + "set" + "1" + "11" + "id" + crCount.ToString("0000") + "cr";
                        btnCostCenterCr.CommandName = ActionsEnum.COSTCENTER.ToString();
                        btnCostCenterCr.Click += new EventHandler(ActionHandler);
                        div.Controls.Add(btnCostCenterCr);

                        //divValidation contains all the Validation Controls in a group. It will be added to the group after delete button. So if any validation catch it will show * after delete button
                        div.Controls.Add(divValidation);

                        div.Controls.Add(hdfIsCostCenterCr);
                        TabIndexCr++;
                        if (btnCostCenterCr != null)
                            if (!dicCrControls.ContainsKey(btnCostCenterCr.ID))
                                dicCrControls.Add(btnCostCenterCr.ID, "CostCenter");
                        if (hdfIsCostCenterCr != null)
                            if (!dicCrControls.ContainsKey(hdfIsCostCenterCr.ID))
                                dicCrControls.Add(hdfIsCostCenterCr.ID, "CostCenter");
                        btnCostCenterCr.Visible = false;
                        hdfIsCostCenterCr.Value = "0";
                        #endregion
                        #region SubledgerLabel
                        //After Delete Button, the rest of the controls will comes in the next line. So the divClear will use to break the first line
                        divClear = new HtmlGenericControl("div");
                        divClear.Attributes.Add("class", "clear");
                        div.Controls.Add(divClear);
                        //It is a dummy label
                        Label lblSubledgerLabelCr = new Label()
                        {
                            ClientIDMode = ClientIDMode.Static,
                            Text = "&nbsp",
                            Visible = false
                        };

                        lblSubledgerLabelCr.ID = "dic" + crCount.ToString("0000") + "50" + "set" + "2" + "01" + "id" + crCount.ToString("0000") + "cr";
                        //The Associate control id is the id of subledger dropdownlist
                        lblSubledgerLabelCr.AssociatedControlID = "dic" + crCount.ToString("0000") + "51" + "set" + "2" + "02" + "id" + crCount.ToString("0000") + "cr";

                        div.Controls.Add(lblSubledgerLabelCr);
                        if (lblSubledgerLabelCr != null)
                        {
                            if (!dicCrControls.ContainsKey(lblSubledgerLabelCr.ID))
                                dicCrControls.Add(lblSubledgerLabelCr.ID, "SubledgerLabel");
                        }
                        #endregion
                        #region SubLedger
                        //Subledger ddl control
                        DropDownList ddlSubLedgerCr = new DropDownList()
                        {
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexCr,
                            Visible = false
                        };

                        ddlSubLedgerCr.ID = "dic" + crCount.ToString("0000") + "51" + "set" + "2" + "02" + "id" + crCount.ToString("0000") + "cr";

                        div.Controls.Add(ddlSubLedgerCr);
                        RequiredFieldValidator vrfSubLedgerCr = new RequiredFieldValidator()
                        {
                            ID = "vrf" + ddlSubLedgerCr.ID.Replace("dic", ""),
                            ControlToValidate = ddlSubLedgerCr.ID,
                            Text = "*",
                            CssClass = "star",
                            ValidationGroup = "voucher",
                            Display = ValidatorDisplay.Dynamic,
                            EnableClientScript = true,
                            InitialValue = CommonConstants.SELECTVAL,
                            SetFocusOnError = true,
                            ErrorMessage = GetLocalResourceObject("Err_SubLedger").ToString(),
                            ClientIDMode = ClientIDMode.Static
                        };
                        divValidation.Controls.Add(vrfSubLedgerCr);
                        vrfSubLedgerCr.Enabled = ddlSubLedgerCr.Visible ? true : false;
                        TabIndexCr++;
                        if (ddlSubLedgerCr != null)
                        {
                            if (!dicCrControls.ContainsKey(ddlSubLedgerCr.ID))
                                dicCrControls.Add(ddlSubLedgerCr.ID, "SubLedger");
                        }
                        #endregion
                        #region InstrumentNo
                        //Instrument No Text box
                        TextBox txtInstrumentNoCr = new TextBox()
                        {
                            Text = "",
                            MaxLength = 70,
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexCr,
                            Enabled = true,
                            Visible = false,
                            CssClass = "input16"
                        };

                        txtInstrumentNoCr.ID = "dic" + crCount.ToString("0000") + "52" + "set" + "2" + "03" + "id" + crCount.ToString("0000") + "cr";

                        string instrumentNoCr = GetLocalResourceObject("InstrumentNo").ToString();
                        //Set "Instrument No" if their is no text in the textbox with a watermark style. Otherwise remove this style
                        txtInstrumentNoCr.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + instrumentNoCr + "';$(this).addClass('input-watermark');}");
                        txtInstrumentNoCr.Attributes.Add("onfocus", "if (this.value == '" + instrumentNoCr + "') {this.value = '';$(this).removeClass('input-watermark');}");
                        txtInstrumentNoCr.Text = instrumentNoCr;
                        journalScript = journalScript + "if ($('#" + txtInstrumentNoCr.ID + "').val() == '" + instrumentNoCr + "') {$('#" + txtInstrumentNoCr.ID + "').addClass('input-watermark');}";
                        div.Controls.Add(txtInstrumentNoCr);
                        TabIndexCr++;
                        if (txtInstrumentNoCr != null)
                        {
                            if (!dicCrControls.ContainsKey(txtInstrumentNoCr.ID))
                                dicCrControls.Add(txtInstrumentNoCr.ID, "InstrumentNo");
                        }
                        #endregion
                        #region Date
                        //Date Picker Control
                        TextBox txtDateCr = new TextBox()
                        {
                            Text = "",
                            MaxLength = 11,
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexCr,
                            Enabled = true,
                            Visible = false,
                            CssClass = "input-w8-5per"
                        };

                        txtDateCr.ID = "dic" + crCount.ToString("0000") + "53" + "set" + "2" + "04" + "id" + crCount.ToString("0000") + "cr";
                        txtDateCr.Attributes.Add("onkeydown", "return CheckKey(event);");
                        txtDateCr.Attributes.Add("onpaste", "return false;");
                        //Set "Date" if their is no text in the textbox with a watermark style. Otherwise remove this style
                        string dateCr = GetLocalResourceObject("Date").ToString();
                        txtDateCr.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + dateCr + "';$(this).addClass('input-watermark');}");
                        txtDateCr.Attributes.Add("onfocus", "if (this.value == '" + dateCr + "') {this.value = '';$(this).removeClass('input-watermark');}");
                        txtDateCr.Text = dateCr;
                        journalScript = journalScript + "if ($('#" + txtDateCr.ID + "').val() == '" + dateCr + "') {$('#" + txtDateCr.ID + "').addClass('input-watermark');}";

                        if (txtDateCr.Visible == true)
                        {
                            if (!string.IsNullOrEmpty(txtDateCr.ID))
                            {
                                string PageScript = CommonFunctions.GenerateDynamicScript("Date", txtDateCr.ID, null, null, null);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtDateCr.ID + "", PageScript, true);
                            }
                        }
                        div.Controls.Add(txtDateCr);
                        TabIndexCr++;
                        if (txtDateCr != null)
                        {
                            if (!dicCrControls.ContainsKey(txtDateCr.ID))
                                dicCrControls.Add(txtDateCr.ID, "Date");
                        }
                        #endregion
                        #region FavourOf
                        //Favourof Textbox
                        TextBox txtFavourOfCr = new TextBox()
                        {
                            MaxLength = 150,
                            ClientIDMode = ClientIDMode.Static,
                            TabIndex = TabIndexCr,
                            Enabled = true,
                            Visible = false,
                            CssClass = "input-w23-6per"
                        };

                        txtFavourOfCr.ID = "dic" + crCount.ToString("0000") + "54" + "set" + "2" + "05" + "id" + crCount.ToString("0000") + "cr";


                        string favourOfCr = GetLocalResourceObject("FavourOf").ToString();
                        //Set "FavourOf" if their is no text in the textbox with a watermark style. Otherwise remove this style
                        txtFavourOfCr.Attributes.Add("onblur", "if (this.value == '') {this.value = '" + favourOfCr + "';$(this).addClass('input-watermark');}");
                        txtFavourOfCr.Attributes.Add("onfocus", "if (this.value == '" + favourOfCr + "') {this.value = '';$(this).removeClass('input-watermark');}");
                        txtFavourOfCr.Text = favourOfCr;
                        journalScript = journalScript + "if ($('#" + txtFavourOfCr.ID + "').val() == '" + favourOfCr + "') {$('#" + txtFavourOfCr.ID + "').addClass('input-watermark');}";

                        div.Controls.Add(txtFavourOfCr);
                        TabIndexCr++;
                        if (txtFavourOfCr != null)
                        {
                            if (!dicCrControls.ContainsKey(txtFavourOfCr.ID))
                                dicCrControls.Add(txtFavourOfCr.ID, "FavourOf");
                        }
                        #endregion
                        //Now the div contains the new added Credit controls. Now we are going to add it to our page
                        tcControl.Controls.Add(div);//Adding to Table Cell
                        trControls.Cells.Add(tcControl);//Adding to Table Row
                        tbControls.Rows.Add(trControls);//Adding to Table
                        divGroupCr.Controls.Add(tbControls);//Adding to Masater Div that holds all the Credit Groups
                        if (dicCrControls != null && dicCrControls.Count > 0)
                        {
                            Session[ERP.Utilities.SessionStrings.CrControls] = dicCrControls;//Updating the Credit Control Session
                        }
                        if (dicControlinfo != null && dicControlinfo.Count > 0)
                        {
                            Session[ERP.Utilities.SessionStrings.ControlInfo] = dicControlinfo;//Updating the Control Info. Session
                        }
                        if (dicAccountType != null && dicAccountType.Count > 0)
                        {
                            Session[ERP.Utilities.SessionStrings.AccountType] = dicAccountType;//Updating the Account Name Session
                        }
                        #endregion
                        #endregion
                    }
                }
                #endregion
            }
            #region TotalAmount Css Change
            txtCrTotal.Attributes.Remove("class");
            txtCrTotalBC.Attributes.Remove("class");
            txtDrTotal.Attributes.Remove("class");
            txtDrTotalBC.Attributes.Remove("class");
            txtCrTotal.Attributes.Add("class", IsSameCurrency() == true ? GetLocalResourceObject("txtSameCurCSS").ToString() : GetLocalResourceObject("txtDiffCurCSS").ToString());
            txtCrTotalBC.Attributes.Add("class", IsSameCurrency() == true ? GetLocalResourceObject("txtSameCurCSS").ToString() : GetLocalResourceObject("txtDiffCurCSS").ToString());
            txtDrTotal.Attributes.Add("class", IsSameCurrency() == true ? GetLocalResourceObject("txtSameCurCSS").ToString() : GetLocalResourceObject("txtDiffCurCSS").ToString());
            txtDrTotalBC.Attributes.Add("class", IsSameCurrency() == true ? GetLocalResourceObject("txtSameCurCSS").ToString() : GetLocalResourceObject("txtDiffCurCSS").ToString());

            #endregion
            commonService = null;
        }
        /// <summary>
        /// Get FinTrxHeaderBO Object
        /// </summary>
        /// <param name="uiDataObject"></param>
        /// <returns></returns>
        private FinTrxHeaderBO GetFinTrxHdrObject(FIN_TRX_HDR uiDataObject)
        {
            FinTrxHeaderBO resultObject = new FinTrxHeaderBO();
            //resultObject.AST_VALUE
            int appSubType = -1;
            if (Session[ERP.Utilities.SessionStrings.CrDrType] == null)
                appSubType = !string.IsNullOrEmpty(TypeForNumberGenaration) ? Convert.ToInt32(TypeForNumberGenaration) : 0;
            else
            {
                if (Session[ERP.Utilities.SessionStrings.CrDrType].ToString() == ApplicationType.PI || Session[ERP.Utilities.SessionStrings.CrDrType].ToString() == ApplicationType.TPI)
                    appSubType = !string.IsNullOrEmpty(TypeForNumberGenaration) ? Convert.ToInt32(TypeForNumberGenaration) : 1;
                else if (Session[ERP.Utilities.SessionStrings.CrDrType].ToString() == ApplicationType.SI)
                    appSubType = !string.IsNullOrEmpty(TypeForNumberGenaration) ? Convert.ToInt32(TypeForNumberGenaration) : 2;
            }
            resultObject.APT_CODE = Session[ERP.Utilities.SessionStrings.TransactionType].ToString();// uiDataObject.FTH_REF_TYPE;
            resultObject.AST_VALUE = Convert.ToString(appSubType);

            resultObject.FTH_ACTIVE = uiDataObject.FTH_ACTIVE;
            resultObject.FTH_BASE_CURR = uiDataObject.FTH_BASE_CURR;
            resultObject.FTH_BIZUNIT = uiDataObject.FTH_BIZUNIT;
            resultObject.FTH_COMPANY = uiDataObject.FTH_COMPANY;
            resultObject.FTH_CRTD_BY = uiDataObject.FTH_CRTD_BY;
            resultObject.FTH_CRTD_DT = uiDataObject.FTH_CRTD_DT;
            if (uiDataObject.FTH_DATE.HasValue)
                resultObject.FTH_DATE = Convert.ToDateTime(uiDataObject.FTH_DATE);
            resultObject.FTH_DEPT = uiDataObject.FTH_DEPT;
            resultObject.FTH_EXCHG_RATE = uiDataObject.FTH_EXCHG_RATE;
            resultObject.FTH_FIN_YEAR = uiDataObject.FTH_FIN_YEAR;
            resultObject.FTH_IS_JRNLD = uiDataObject.FTH_IS_JRNLD;
            resultObject.FTH_MOD_BY = uiDataObject.FTH_MOD_BY;
            resultObject.FTH_MOD_DT = uiDataObject.FTH_MOD_DT;
            resultObject.FTH_NARRATION = uiDataObject.FTH_NARRATION;
            resultObject.FTH_PARTY_NAME = uiDataObject.FTH_PARTY_NAME;
            if (uiDataObject.FTH_PDC.HasValue)
                resultObject.FTH_PDC = Convert.ToByte(uiDataObject.FTH_PDC);
            resultObject.FTH_PK = uiDataObject.FTH_PK;
            resultObject.FTH_REF_DATE = uiDataObject.FTH_REF_DATE;
            resultObject.FTH_REF_NO = uiDataObject.FTH_REF_NO;
            resultObject.FTH_REF_PK = uiDataObject.FTH_REF_PK;
            resultObject.FTH_REF_TYPE = uiDataObject.FTH_REF_TYPE;
            resultObject.FTH_REMARKS = uiDataObject.FTH_REMARKS;
            resultObject.FTH_STATUS = uiDataObject.FTH_STATUS;
            if (uiDataObject.FTH_TRX_DATE.HasValue)
                resultObject.FTH_TAX_DATE = ((DateTime)uiDataObject.FTH_TRX_DATE).ToString(Resources.Constants.DateFormatShort); ;

            resultObject.FTH_TRX_CURR = uiDataObject.FTH_TRX_CURR;
            if (uiDataObject.FTH_TRX_DATE.HasValue)
                resultObject.FTH_TRX_DATE = Convert.ToDateTime(uiDataObject.FTH_TRX_DATE);
            resultObject.FTH_VOUCHER_NO = uiDataObject.FTH_VOUCHER_NO;
            resultObject.USER_PK = currentUser.PKUser;
            #region Add Transaction Details
            if (uiDataObject.FIN_TRX != null && uiDataObject.FIN_TRX.Count > 0)
            {
                resultObject.FinTrxDetails = new List<FinTrxDetailsBO>();
                int slno = 1;
                foreach (FIN_TRX trxData in uiDataObject.FIN_TRX)
                {
                    FinTrxDetailsBO data = new FinTrxDetailsBO();
                    data.FTR_SL_NO = slno++;
                    data.FTR_PK = trxData.FTR_PK;
                    data.FTR_TRX_HDR = trxData.FTR_TRX_HDR;

                    data.FTR_SEQUENCE = trxData.FTR_SEQUENCE;
                    data.FTR_ACCOUNT = trxData.FTR_ACCOUNT;
                    data.FTR_ACC_SUB_TYPE = trxData.FTR_ACC_SUB_TYPE;
                    if (trxData.FTR_PAYMENT_MODE.HasValue)
                        data.FTR_PAYMENT_MODE = Convert.ToString(trxData.FTR_PAYMENT_MODE);
                    data.FTR_INSTR_NO = trxData.FTR_INSTR_NO;
                    if (trxData.FTR_INSTR_DATE.HasValue)
                        data.FTR_INSTR_DATE = ((DateTime)trxData.FTR_INSTR_DATE).ToString(Resources.Constants.DateFormatShort);
                    data.FTR_INSTR_FAVOUR = trxData.FTR_INSTR_FAVOUR;
                    data.FTR_NARRATION = trxData.FTR_NARRATION;
                    data.FTR_DR_AMT_TC = trxData.FTR_DR_AMT_TC;
                    data.FTR_CR_AMT_TC = trxData.FTR_CR_AMT_TC;
                    data.FTR_DR_AMT_BC = trxData.FTR_DR_AMT_BC;
                    data.FTR_CR_AMT_BC = trxData.FTR_CR_AMT_BC;
                    data.FTR_TYPE = trxData.FTR_TYPE;
                    data.FTR_TYPE_PK = Convert.ToInt32(trxData.FTR_TYPE_PK);
                    if (trxData.FTR_CLEAR_DATE.HasValue)
                        data.FTR_CLEAR_DATE = Convert.ToString(trxData.FTR_CLEAR_DATE);
                    data.FTR_IS_RECONCILED = trxData.FTR_IS_RECONCILED;
                    data.FTR_REMARKS = trxData.FTR_REMARKS;
                    data.FTR_ACTIVE = trxData.FTR_ACTIVE;
                    data.FTR_CRTD_BY = trxData.FTR_CRTD_BY;
                    data.FTR_CRTD_DT = trxData.FTR_CRTD_DT;
                    data.FTR_MOD_BY = trxData.FTR_MOD_BY;
                    data.FTR_MOD_DT = trxData.FTR_MOD_DT;
                    data.FTR_DEPT = trxData.FTR_DEPT;
                    data.FTR_BIZUNIT = trxData.FTR_BIZUNIT;
                    data.FTR_EXCHG_RATE = trxData.FTR_EXCHG_RATE;
                    data.FTR_ENTRY_MODE = Convert.ToString(trxData.FTR_ENTRY_MODE);
                    data.FTR_EXCHG_RATE_YE = Convert.ToString(trxData.FTR_EXCHG_RATE_YE);
                    data.FTR_PDC = Convert.ToByte(trxData.FTR_PDC);
                    data.FTR_IS_BANK_CHARGE = trxData.FTR_IS_BANK_CHARGE;
                    if (trxData.FIN_TRX_COC_DTL != null && trxData.FIN_TRX_COC_DTL.Count > 0)
                    {
                        data.CostCenterList = new List<FinCostCenterBO>();
                        foreach (FIN_TRX_COC_DTL cocDtl in trxData.FIN_TRX_COC_DTL)
                        {
                            FinCostCenterBO dataCoc = new FinCostCenterBO();
                            dataCoc.FTD_SL_NO = data.FTR_SL_NO;
                            dataCoc.FTD_PK = cocDtl.FTD_PK;
                            dataCoc.FTD_FTR_PK = cocDtl.FTD_FTR_PK;
                            dataCoc.FTD_CNM_PK = cocDtl.FTD_CNM_PK;
                            dataCoc.FTD_AMT_BC = cocDtl.FTD_AMT_BC;
                            dataCoc.FTD_AMT_BC = cocDtl.FTD_AMT_BC;
                            data.CostCenterList.Add(dataCoc);
                        }
                    }
                    resultObject.FinTrxDetails.Add(data);

                }
            }
            #endregion
            if (uiDataObject.FIN_PAYMENT_VND_TAX_HDR != null && uiDataObject.FIN_PAYMENT_VND_TAX_HDR.Count > 0)
            {
                resultObject.FinPaymentVndTaxHdr = new List<FinPaymentVndTaxHeaderBO>();
                foreach (FIN_PAYMENT_VND_TAX_HDR paymentVndData in uiDataObject.FIN_PAYMENT_VND_TAX_HDR)
                {
                    FinPaymentVndTaxHeaderBO objpaymentVnd = new FinPaymentVndTaxHeaderBO();
                    objpaymentVnd.WTH_AMOUNT = paymentVndData.WTH_AMOUNT;
                    objpaymentVnd.WTH_TAX_AMT = paymentVndData.WTH_TAX_AMT;
                    objpaymentVnd.WTH_TAX = paymentVndData.WTH_TAX;
                    objpaymentVnd.WTH_PK = 0;
                    objpaymentVnd.WTH_TRX_HDR = paymentVndData.WTH_TRX_HDR;
                    objpaymentVnd.WTH_TYPE = paymentVndData.WTH_TYPE;
                    objpaymentVnd.WTH_TAX_CATEGORY = paymentVndData.WTH_TAX_CATEGORY;
                    objpaymentVnd.WTH_NAME = paymentVndData.WTH_NAME;
                    objpaymentVnd.WTH_DESC = paymentVndData.WTH_DESC;
                    objpaymentVnd.WTH_FORM_NO = paymentVndData.WTH_FORM_NO;
                    objpaymentVnd.WTH_PARTY_NAME = paymentVndData.WTH_PARTY_NAME;
                    objpaymentVnd.WTH_ADDRESS = paymentVndData.WTH_ADDRESS;
                    objpaymentVnd.WTH_ADDRESS = paymentVndData.WTH_ADDRESS;
                    objpaymentVnd.WTH_TAX_ID = paymentVndData.WTH_TAX_ID;
                    objpaymentVnd.WTH_CATEGORY = paymentVndData.WTH_CATEGORY;
                    objpaymentVnd.WTH_TAX_DATE = paymentVndData.WTH_TAX_DATE;
                    objpaymentVnd.WTH_BRANCH = paymentVndData.WTH_BRANCH;
                    objpaymentVnd.WTH_BRANCH_NAME = HttpUtility.HtmlEncode(paymentVndData.WTH_BRANCH_NAME);
                    objpaymentVnd.WTH_BRANCH_TEXT = HttpUtility.HtmlEncode(paymentVndData.WTH_BRANCH_TEXT);
                    objpaymentVnd.WTH_BRANCH_TYPE = paymentVndData.WTH_BRANCH_TYPE;
                    objpaymentVnd.WTH_PAYMENT_TYPE = paymentVndData.WTH_PAYMENT_TYPE;
                }

            }

            return resultObject;
        }

        /// <summary>
        /// Function used to check whether the voucher is Year end voucher or not 
        /// </summary>
        /// <returns></returns>
        private bool IsYearEndVoucher()
        {

            bool IsYearend = false;
            string AppType = string.Empty;
            if (Session[ERP.Utilities.SessionStrings.TransactionType] != null)
                AppType = Session[ERP.Utilities.SessionStrings.TransactionType].ToString();
            string[] YearEndVoucherTypes = GetLocalResourceObject("YearEndVoucherTypes").ToString().Split(',');
            //missed FC reverse
            //if (AppType == ApplicationType.PIJYE || AppType == ApplicationType.VPJYE || AppType == ApplicationType.SIJYE || AppType == ApplicationType.CRJYE || AppType == ApplicationType.EIJYE || AppType == ApplicationType.PSIJYE || AppType == ApplicationType.EIPJYE || AppType == ApplicationType.SIPJYE || AppType == ApplicationType.MSIJYE || AppType == ApplicationType.MSIRJYE || AppType == ApplicationType.CNSJYE || AppType == ApplicationType.DNSJYE || AppType == ApplicationType.CNPJYE || AppType == ApplicationType.DNPJYE)
            if (YearEndVoucherTypes.Contains(AppType))
            {
                IsYearend = true;
                hdfIsYearEnd.Value = "1";
            }
            return IsYearend;
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
                    ddlVoucherCompany.Items.Clear();
                    if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    {
                        ddlVoucherCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                        ddlVoucherCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlVoucherCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlVoucherCompany.SelectedIndex = -1;
                        ddlVoucherCompany.DataBind();
                    }
                    ////To set company related to current SBU 
                    //if (dtCompany != null && dtCompany.Rows.Count > 0)
                    //{
                    //    ddlVoucherCompany.SelectedIndex = ddlVoucherCompany.Items.IndexOf(ddlVoucherCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()));
                    //}

                    break;
                #endregion
                #region TEMPLATECATEGORY
                case ControlsEnum.TEMPLATECATEGORY:
                    ddlTemplateCategory.Items.Clear();
                    if (admTemplateCategoryList != null && admTemplateCategoryList.Count > 0)
                    {
                        ddlTemplateCategory.DataSource = CommonFunctions.HtmlDecode(admTemplateCategoryList, Resources.DataFieldRes.ConstName);
                        ddlTemplateCategory.DataTextField = Resources.DataFieldRes.ConstName;
                        ddlTemplateCategory.DataValueField = Resources.DataFieldRes.ConstPK;
                        ddlTemplateCategory.DataBind();
                    }
                    ddlTemplateCategory.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
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
                    case ControlsEnum.COSTCENTER:
                        if (CostCenterList != null && CostCenterList.Count > 0)
                            grdCostCenter.DataSource = CostCenterList.Where(r => r.CONTROL_ID == ControlId).ToList();
                        else
                            grdCostCenter.DataSource = null;
                        grdCostCenter.DataBind();
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Used to Set Credit Section Hdr
        /// </summary>
        private void SetCreditHdr()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            Table tbControls = new Table();
            tbControls.CssClass = "";
            TableRow trControls = new TableRow();
            TableCell tcControl = new TableCell();

            TableCell tcControl1 = new TableCell();
            TableCell tcControl2 = new TableCell();
            TableCell tcControl3 = new TableCell();
            TableCell tcControl4 = new TableCell();
            TableCell tcControl5 = new TableCell();
            TableCell tcControl6 = new TableCell();
            TableCell tcControl7 = new TableCell();

            HtmlGenericControl h1 = new HtmlGenericControl("h4");
            h1.InnerText = "";
            tcControl1.Width = new Unit("9.5%");
            tcControl1.Controls.Add(h1);

            h1 = new HtmlGenericControl("h4");
            h1.InnerText = GetLocalResourceObject("CrAccounts").ToString();
            tcControl2.Width = new Unit("30.9%");
            tcControl2.Controls.Add(h1);

            h1 = new HtmlGenericControl("h4");
            h1.InnerText = GetLocalResourceObject("Narration").ToString();
            //tcControl3.Width = new Unit("34%");
            tcControl3.Width = IsSameCurrency() == true ? new Unit(GetLocalResourceObject("NarationCaptionWidthBC").ToString()) : new Unit(GetLocalResourceObject("NarationCaptionWidthNormal").ToString());
            tcControl3.Controls.Add(h1);

            h1 = new HtmlGenericControl("h4");
            h1.Controls.Add(litJournalCurrencyCr);
            h1.Attributes.Add("align", "right");
            tcControl4.Width = 79;
            tcControl4.Controls.Add(h1);

            transactionCurrency = 0;
            if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
            {
                transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
            }
            else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
            {
                transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);
            }
            if (transactionCurrency <= 0
                || transactionCurrency != currentUser.BaseCurrency)
            {
                h1 = new HtmlGenericControl("h4");
                h1.InnerText = "";
                tcControl5.Width = 0;
                tcControl5.Controls.Add(h1);

                h1 = new HtmlGenericControl("h4");
                h1.InnerText = GetLocalResourceObject("Amount").ToString().Trim() + "(" + hdfJournalBaseCurrency.Value.Split('-')[0].Trim() + ")";
                h1.Attributes.Add("align", "right");
                //tcControl6.Width = new Unit("14%");
                tcControl6.Width = IsSameCurrency() == true ? new Unit(GetLocalResourceObject("BCAmountCaptionWidthBC").ToString()) : new Unit(GetLocalResourceObject("BCAmountCaptionWidthNormal").ToString());
                tcControl6.Controls.Add(h1);
            }


            h1 = new HtmlGenericControl("h4");
            h1.InnerText = "";
            tcControl7.Controls.Add(h1);

            trControls.Cells.Add(tcControl1);
            trControls.Cells.Add(tcControl2);
            trControls.Cells.Add(tcControl3);
            trControls.Cells.Add(tcControl4);
            trControls.Cells.Add(tcControl5);
            trControls.Cells.Add(tcControl6);
            trControls.Cells.Add(tcControl7);

            tbControls.Rows.Add(trControls);
            divGroupCrHdr.Controls.Add(tbControls);
        }

        /// <summary>
        /// Used to Set Debit section Hdr
        /// </summary>
        private void SetDebitHdr()
        {
            Table tbControls = new Table();
            tbControls.CssClass = "";
            TableRow trControls = new TableRow();
            TableCell tcControl = new TableCell();

            TableCell tcControl1 = new TableCell();
            TableCell tcControl2 = new TableCell();
            TableCell tcControl3 = new TableCell();
            TableCell tcControl4 = new TableCell();
            TableCell tcControl5 = new TableCell();
            TableCell tcControl6 = new TableCell();
            TableCell tcControl7 = new TableCell();

            HtmlGenericControl h1 = new HtmlGenericControl("h4");
            h1.InnerText = "";
            tcControl1.Width = new Unit("9.5%");
            tcControl1.Controls.Add(h1);

            h1 = new HtmlGenericControl("h4");
            h1.InnerText = GetLocalResourceObject("DrAccounts").ToString();
            tcControl2.Width = new Unit("30.9%");
            tcControl2.Controls.Add(h1);

            h1 = new HtmlGenericControl("h4");
            h1.InnerText = GetLocalResourceObject("Narration").ToString();
            //tcControl3.Width = new Unit("34%");
            tcControl3.Width = IsSameCurrency() == true ? new Unit(GetLocalResourceObject("NarationCaptionWidthBC").ToString()) : new Unit(GetLocalResourceObject("NarationCaptionWidthNormal").ToString());
            tcControl3.Controls.Add(h1);


            h1 = new HtmlGenericControl("h4");
            h1.Controls.Add(litJournalCurrencyDr);
            h1.Attributes.Add("align", "right");
            tcControl4.Width = 79;
            tcControl4.Controls.Add(h1);

            GetFieldValues(ControlsEnum.BASECURRENCY);
            transactionCurrency = 0;
            if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
            {
                transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
            }
            else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
            {
                transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);
            }

            if (transactionCurrency <= 0
                || transactionCurrency != currentUser.BaseCurrency)
            {
                h1 = new HtmlGenericControl("h4");
                h1.InnerText = "";
                tcControl5.Width = 0;
                tcControl5.Controls.Add(h1);

                h1 = new HtmlGenericControl("h4");
                h1.InnerText = GetLocalResourceObject("Amount").ToString().Trim() + " (" + hdfJournalBaseCurrency.Value.Split('-')[0].Trim() + ")";
                h1.Attributes.Add("align", "right");
                //tcControl6.Width = new Unit("14%");
                tcControl6.Width = IsSameCurrency() == true ? new Unit(GetLocalResourceObject("BCAmountCaptionWidthBC").ToString()) : new Unit(GetLocalResourceObject("BCAmountCaptionWidthNormal").ToString());
                tcControl6.Controls.Add(h1);
            }

            h1 = new HtmlGenericControl("h4");
            h1.InnerText = "";
            tcControl7.Controls.Add(h1);

            trControls.Cells.Add(tcControl1);
            trControls.Cells.Add(tcControl2);
            trControls.Cells.Add(tcControl3);
            trControls.Cells.Add(tcControl4);
            trControls.Cells.Add(tcControl5);
            trControls.Cells.Add(tcControl6);
            trControls.Cells.Add(tcControl7);

            tbControls.Rows.Add(trControls);
            divGroupDrHdr.Controls.Add(tbControls);
        }

        /// <summary>
        /// Is Transaction Currency Same as Base Currency
        /// </summary>
        /// <returns></returns>
        private bool IsSameCurrency()
        {
            bool jurResult = false;
            transactionCurrency = 0;
            if (Session[ERP.Utilities.SessionStrings.TransactionCurrency] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString()))
                transactionCurrency = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionCurrency].ToString());
            else if (!string.IsNullOrEmpty(hdfJournalCurr.Value))
                transactionCurrency = Convert.ToInt32(hdfJournalCurr.Value);

            if (transactionCurrency > 0 && transactionCurrency == currentUser.BaseCurrency)
                jurResult = true;
            else
                jurResult = false;
            return jurResult;
        }
        public void ResetForm()
        {
            hdfVoucherStatus.Value = "0";
            divGroupDrHdr.Controls.Clear();
            divGroupCrHdr.Controls.Clear();
            divGroupDr.Controls.Clear();
            divGroupCr.Controls.Clear();
            hdfExchangeRateJV.Value = string.Empty;
            txtJournalExchangeRate.Text = string.Empty;
            hdfPVDate.Value = txtPVDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            txtNarration.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            JournalizeRefPK = 0;
            VoucherTemplatePK = 0;
            IsBaseCurrency = true;
            JournalType = 1;
            TypeForNumberGenaration = null;
            VoucherDeleteStatus = false;
            CloseVoucherPopup = false;
        }

        /// <summary>
        /// Method to get formattde currency
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string GetFormattedCurrency(object number)
        {
            decimal num = 0;
            decimal.TryParse(Convert.ToString(number), out num);
            return ERP.Utilities.CommonFunctions.DecimalFormat(num, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormatVoucher.Value);
        }
        /// <summary>
        /// Cost center splitup validation checking
        /// </summary>       
        private bool IsValidCostCenterSplitAllocation(List<CostCenterDetails> costCenterAllocationList, decimal selAccountCCAmount)
        {
            bool CostSplitValidation = true;
            if (costCenterAllocationList != null && costCenterAllocationList.Count > 0)
            {
                decimal debitCreditAmnt = selAccountCCAmount;
                if (GetGlobalResourceObject("ConfigurationsRes", "CCPercentageRequired").ToString() == "1")
                {
                    if (ERP.Utilities.CommonFunctions.DecimalFormat(costCenterAllocationList.Sum(r => r.FTD_AMT_BC), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) != debitCreditAmnt)
                        CostSplitValidation = false;
                }
            }
            return CostSplitValidation;
        }

        public bool ValidatePageDept()
        {
            bool result = true;
            string redirectURL = GetLocalResourceObject("loginURL").ToString();// "~/../login.aspx";
            if (hdfCurrentDepartment.Value != "-1" && hdfCurrentDepartment.Value != ((BusinessObject.User)(HttpContext.Current.User.Identity)).CurrentDeptPK.ToString())
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
                if (System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] != null)
                {
                    redirectURL = System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] + "?Logout=1";
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetGlobalResourceObject("ErrorMessages", "Msg_Dept_Session_Expired").ToString()) + "','" + GetGlobalResourceObject("Messages", "Information").ToString() + "','" + redirectURL + "');", true);
                result = false;
            }
            return result;
        }

        #endregion
        #region Pager Methods + Init
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            btnJournalSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnJournalSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnAddDebit.PreRender += new EventHandler(btnAction_PreRender);
            btnAddCredit.PreRender += new EventHandler(btnAction_PreRender);
            btnJournalSave.PreRender += new EventHandler(btnAction_PreRender);
            btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            btnJournalPrint.PreRender += new EventHandler(btnAction_PreRender);
            btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnSaveAsTemplate.PreRender += new EventHandler(btnAction_PreRender);
            imbGainLoss.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            imbChangeTaxEffDate.PreRender += new EventHandler(btnAction_PreRender);

            btnJournalSubmit.Load += new EventHandler(btnAction_Load);
            btnJournalSaveSubmit.Load += new EventHandler(btnAction_Load);
            btnAddDebit.Load += new EventHandler(btnAction_Load);
            btnAddCredit.Load += new EventHandler(btnAction_Load);
            btnJournalSave.Load += new EventHandler(btnAction_Load);
            btnDelete.Load += new EventHandler(btnAction_Load);
            btnJournalPrint.Load += new EventHandler(btnAction_Load);
            btnCancel.Load += new EventHandler(btnAction_Load);
            btnSaveAsTemplate.Load += new EventHandler(btnAction_Load);
            imbGainLoss.Load += new EventHandler(btnAction_Load);
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            imbChangeTaxEffDate.Load += new EventHandler(btnAction_Load);
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
            WorkFlowBasePage basePage = (WorkFlowBasePage)this.Page;
            basePage.CheckBtnVisibility(sender);
        }

        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.Init += new EventHandler(this.Page_Init);
        }
        /// <summary>
        /// To handle OnInit event Used to assign the Event for all the actions used in this page        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {

            if (!IsPostBack)
                Session[ERP.Utilities.SessionStrings.TransactionType] = null;
            base.OnInit(e);
            InitializeComponent();
            divGroupDrHdr.Controls.Clear();
            divGroupDr.Controls.Clear();
            divGroupCrHdr.Controls.Clear();
            divGroupCr.Controls.Clear();
            CallUserControl();
        }
        /// Gets the ID of the post back control.
        /// 
        /// See: http://geekswithblogs.net/mahesh/archive/2006/06/27/83264.aspx
        /// </summary>
        /// <param name = "page">The page.</param>
        /// <returns></returns>
        public static string GetPostBackControlId(Page page)
        {
            if (!page.IsPostBack)
                return string.Empty;

            Control control = null;
            // first we will check the "__EVENTTARGET" because if post back made by the controls
            // which used "_doPostBack" function also available in Request.Form collection.
            string controlName = page.Request.Params["__EVENTTARGET"];
            if (!String.IsNullOrEmpty(controlName))
            {
                control = page.FindControl(controlName);
            }
            else
            {
                // if __EVENTTARGET is null, the control is a button type and we need to
                // iterate over the form collection to find it

                // ReSharper disable TooWideLocalVariableScope
                string controlId;
                Control foundControl;
                // ReSharper restore TooWideLocalVariableScope

                foreach (string ctl in page.Request.Form)
                {
                    // handle ImageButton they having an additional "quasi-property" 
                    // in their Id which identifies mouse x and y coordinates
                    if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                    {
                        controlId = ctl.Substring(0, ctl.Length - 2);
                        foundControl = page.FindControl(controlId);
                    }
                    else
                    {
                        foundControl = page.FindControl(ctl);
                    }

                    if (!(foundControl is Button || foundControl is ImageButton)) continue;

                    control = foundControl;
                    break;
                }
            }

            return control == null ? String.Empty : control.ID;
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
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                hdfCurrentDepartment.Value = currentUser.CurrentDeptPK.ToString();
                if (IsBaseCurrency)
                    imbGainLoss.Visible = false;
                if (IsNewDummy || VoucherDeleteStatus)
                    btnJournalPrint.Visible = false;
                //if (GetGlobalResourceObject("ConfigurationsRes", "ShowDivJournalCompany").ToString() == "1")
                //    divJournalCompany.Visible = true;
                //else
                //    divJournalCompany.Visible = false;
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "JournalHideListing", "$(document).ready(function(){ShowUserControlListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "JournalViewMode", "$(document).ready(function(){UserControlViewMode(1);});", true);
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "JournalHideListing", "$(document).ready(function(){ShowUserControlListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "JournalViewMode", "$(document).ready(function(){UserControlViewMode(2);});", true);
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "JournalHideListing", "$(document).ready(function(){ShowUserControlListing();});", true);
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "JournalScript", "$(document).ready(function(){" + journalScript + "});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "VoucherUserControlInitComponents", "$(document).ready(function(){VoucherUserControlInitComponents();});", true);
                if (!HasWkfPermission && Convert.ToInt32(hdfVoucherStatus.Value) != 2)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "JournalViewMode", "$(document).ready(function(){UserControlViewMode(1);});", true);
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
        /// Define Controltype Enum
        /// </summary>
        enum ControlTypes
        {
            Page,
            Label,
            Text,
            DropDown,
            DateTime,
            Numeric,
            Button,
            Spacer,
            GridView,
            CheckBox,
            TextArea,
            TimePicker,
            Header,
            Table,
            Iframe,
            HiddenField,
            HourText,
            FileUpload,
            Date,
            DropDownList,
            TextBox
        }

        enum ControlCategory
        {
            SubType,
            Account,
            Narration,
            AmountTC,
            AmountBC,
            Delete,
            SubledgerLabel,
            SubLedger,
            InstrumentNo,
            Date,
            FavourOf,
            ExchangeRate,
            EntryMode,
            CostCenter
        }


        /// <summary>
        /// Define Common Enum
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            DYNAMICTABS,
            CUSTOMER,
            NEWMODE,
            FINHEADER,
            FINDETAILS,
            EXCHANGERATE,
            INVOICEHDR,
            VOUCHERNO,
            FINCOASUBTYPECFG,
            FINCOAMST,
            BASECURRENCY,
            PETTYCASHVOUCHERLIMIT,
            FINHEADERBYPK,
            COMPANY,
            DEPARTMENT,
            TEMPLATECATEGORY,
            VOUCHERTEMPLATE,
            CALCULATIONMODE,
            BCENABLEDISABLE,
            DISABLEROUND,
            EXTDEPARTMENT,
            COSTCENTER,
            ALLCOSTCENTERS,
            AUDITLOGSTATUS,
            AUDITLOGDETAILS

        }

        /// <summary>
        /// Define FINCOASUBTYPECFG Enum
        /// </summary>
        public enum FINCOASUBTYPECFGEnum
        {
            Cash = 10
        }

        public enum VoucherEntryMode
        {
            Editable = 0,
            NonEditable = 1,
            GainOrLoss = 2
        }

        private enum GainLossMode
        {
            Gain = 1,
            Loss = 2
        }

        public enum TemplateMode
        {
            Debit = 2, //Debit = 1, //In JV Template Debit=2 
            Credit = 1 //Credit = 2 //In JV Template Credit=1
        }

        public enum GLCalculationMode
        {
            Actual = 1,
            DiffOfBC = 2
        }
        #endregion
    }
}

#region Notes
//ID format for Debit Controls ="dic" + drCount.ToString("0000") + control index in the group + "set" + set no(1/2) + control index in the set + "id" + drCount.ToString("0000") + "dr";
//ID format for Credit Controls ="dic" + crCount.ToString("0000") + control index in the group + "set" + set no(1/2) + control index in the set + "id" + crCount.ToString("0000") + "cr";
#endregion