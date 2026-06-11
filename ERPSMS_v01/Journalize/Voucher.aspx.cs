using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using ERP.Utilities;
using BusinessObject.AccountManagement;
using ERPData;
using ERPManager;
using ERPService;
using BusinessObject.CommonManagement;
using System.Data;
using ERPService.Administration;
using System.Drawing;
using System.Threading;
using System.Text;
using BusinessLogic.CommonManagement;
using System.Diagnostics;
using System.Collections;
using System.Transactions;
using BusinessObject.Journalize;

namespace ERPSMS_v01.Journalize
{
    public partial class Voucher : ERP.Store.UI.WorkFlowBasePage
    {

        #region Variables and Properties
        #region Properties
        /// <summary>
        /// Entry State for managing display status
        /// </summary>
        private EntryStatus EntryStatus
        {
            get
            {
                return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.NEWMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);
            }
            set
            {
                this.ViewState[ViewstateStrings.EntryState] = value;
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
        /// Current PK
        /// </summary>
        private int CurrFtrPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrFtrPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrFtrPK] = value;
            }
        }


        /// <summary>
        /// Voucher Type
        /// </summary>
        private string VoucherType
        {
            get
            {
                return this.ViewState[ViewstateStrings.VoucherType].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.VoucherType] = value;
            }
        }

        /// <summary>
        /// Account Type
        /// </summary>
        private int AccountType
        {
            get
            {
                return this.ViewState[ViewstateStrings.AccountType] == null ? 1 : Convert.ToInt32(this.ViewState[ViewstateStrings.AccountType].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.AccountType] = value;
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
        /// Department Company PK
        /// </summary>
        private int CompanyPK
        {
            get
            {
                return this.ViewState["CompanyPK"] == null ? 0 : (int)this.ViewState["CompanyPK"];
            }
            set
            {
                this.ViewState["CompanyPK"] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private string RecordIdentity
        {
            get
            {
                return this.ViewState["RecordIdentity"] == null ? string.Empty : this.ViewState["RecordIdentity"].ToString();
            }
            set
            {
                this.ViewState["RecordIdentity"] = value;
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
        /// Invoice PO Split List
        /// </summary>
        private List<FIN_PAYMENT_VND_TAX_HDR> WHTTaxDetails
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.WHTTaxDetails] == null ? new List<FIN_PAYMENT_VND_TAX_HDR>()
                    : (List<FIN_PAYMENT_VND_TAX_HDR>)Session[ERP.Utilities.SessionStrings.WHTTaxDetails];
            }
            set
            {
                if (value == null)
                    Session.Remove(ERP.Utilities.SessionStrings.WHTTaxDetails);
                else
                    Session[ERP.Utilities.SessionStrings.WHTTaxDetails] = value;
            }
        }
        private List<FIN_PAYMENT_VND_TAX_HDR> VATTaxDetails
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.VATTaxDetails] == null ? new List<FIN_PAYMENT_VND_TAX_HDR>()
                    : (List<FIN_PAYMENT_VND_TAX_HDR>)Session[ERP.Utilities.SessionStrings.VATTaxDetails];
            }
            set
            {
                if (value == null)
                    Session.Remove(ERP.Utilities.SessionStrings.VATTaxDetails);
                else
                    Session[ERP.Utilities.SessionStrings.VATTaxDetails] = value;
            }
        }
        private List<FIN_PAYMENT_VND_TAX_HDR> TempWHTTaxDetails
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.TempWHTTaxDetails] == null ? new List<FIN_PAYMENT_VND_TAX_HDR>()
                    : (List<FIN_PAYMENT_VND_TAX_HDR>)Session[ERP.Utilities.SessionStrings.TempWHTTaxDetails];
            }
            set
            {
                if (value == null)
                    Session.Remove(ERP.Utilities.SessionStrings.TempWHTTaxDetails);
                else
                    Session[ERP.Utilities.SessionStrings.TempWHTTaxDetails] = value;
            }
        }

        private List<FIN_PAYMENT_VND_TAX_HDR> TempVATTaxDetails
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.TempVATTaxDetails] == null ? new List<FIN_PAYMENT_VND_TAX_HDR>()
                    : (List<FIN_PAYMENT_VND_TAX_HDR>)Session[ERP.Utilities.SessionStrings.TempVATTaxDetails];
            }
            set
            {
                if (value == null)
                    Session.Remove(ERP.Utilities.SessionStrings.TempVATTaxDetails);
                else
                    Session[ERP.Utilities.SessionStrings.TempVATTaxDetails] = value;
            }
        }
        private List<ADM_CONFIG_MST> TempConfigMstDetails
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.TempConfigMstDetails] == null ? new List<ADM_CONFIG_MST>()
                    : (List<ADM_CONFIG_MST>)Session[ERP.Utilities.SessionStrings.TempConfigMstDetails];
            }
            set
            {
                if (value == null)
                    Session.Remove(ERP.Utilities.SessionStrings.TempConfigMstDetails);
                else
                    Session[ERP.Utilities.SessionStrings.TempConfigMstDetails] = value;
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
        /// Gets or sets CostCenter
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
        #endregion

        private ActionsEnum commonActions;
        //page related class objects      
        private FIN_TRX_HDR finTrxHdrObj;
        private FinTrxHeaderBO finTrxHeaderObj;
        private FIN_TRX finTrxObj;
        private FIN_COA_MST finCoaMstObj;
        private ADM_CONFIG_MST admConfigMstObj;
        private FIN_VOUCHER_ENTRY_CFG finVcrEntryCfgObj;
        private FIN_COA_SUB_TYPE_CFG finCoaSubTypeCfgObj;
        private ServiceUtility serviceUtilityObj;
        //List for binding details to controls
        private List<FIN_TRX_HDR> finTrxHdrListJournal;
        private List<FIN_TRX_HDR> finTrxHdrList;
        private List<FIN_TRX> finTrxList;
        private List<FinTrxDetailsBO> finTrxDetailsList;
        private List<FIN_COA_MST> finCoaMstList;
        private List<ADM_CONFIG_MST> admConfigMstList;
        //private List<ADM_CONFIG_MST> admConfigMstVndrCntTypeList;
        private List<FIN_VOUCHER_ENTRY_CFG> finVcrEntryCfgList;
        private List<FIN_COA_SUB_TYPE_CFG> finCoaSubTypeCfgList;
        private List<ADM_CONST_MST> admConstMstList;
        private List<FIN_PAYMENT_VND_TAX_HDR> finPaymentVndTaxHdrList;
        private FIN_PAYMENT_VND_HDR finPaymentVndHdrObj;
        private FIN_PAYMENT_VND_TAX_HDR finVatPaymentDetails;
        private List<FIN_PAYMENT_VND_HDR> finPaymentVndHdrList;
        private List<FIN_PAYMENT_VND_TAX_HDR> finPayemtVndHdrList;
        List<FIN_PAYMENT_VND_TAX_HDR> tempWHTTaxDetails;
        FIN_PAYMENT_VND_TAX_HDR tempWHTTax;
        List<FIN_PAYMENT_VND_TAX_HDR> tempVATTaxDetails;
        FIN_PAYMENT_VND_TAX_HDR tempVATTax;

        FIN_PAYMENT_VND_TAX_HDR finPymntObj;
        List<FIN_PAYMENT_VND_TAX_HDR> finPymntList;

        List<DDLMaster> SubTypesAccounts;
        private List<PUR_VENDOR_MST> purVendorMstList;
        private BusinessObject.User currentUser;
        private CommonService CommonServiceClient;

        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;

        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        private List<CostCenterDetails> CostCenterTempList;

        private string refID;
        private string inboxFlag;
        private string voucherNo;

        private bool postflag = false;
        private bool showPopup = false;
        private bool updateVocher;

        private decimal DrTotal = 0;
        private decimal CrTotal = 0;
        private decimal withHoldTax = 0;
        private decimal VatBuyTax = 0;
        private decimal AmountTotal = 0;
        private decimal TaxTotal = 0;
        private byte selectedModeValue = 0;

        private int ddlAccountType = 0;
        private int ddlAccountSubType = 0;
        private int whtTaxpk;
        private int VatBuyTaxpk;
        private int VatVendorPopupPk = 0;
        private int VncPk = 0;
        int JournalPK;
        int selAccountPk = 0;
        decimal CCAmount = 0;

        DataTable dtCmp;
        private DataTable dtVendorAccount;
        private DataTable dtTaxDetails;
        private DataTable dtPageData;
        private DataTable dtVendorDtl;
        private DataTable dtAdsTypeDtl;
        private DataTable dtAdsType;
        private DataSet dsAdsType;
        private DataSet dsAdsTypeDtl;
        private DataTable dtPaymentTypes;
        private DataTable dtCostCenter;
        private DataTable dtCashAccount;
        private DataTable dtStatus;
        private DataTable dtAuditDetails;



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
        /// page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
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

            CommonService commonService;
            commonService = null;

            AccountMstService AccountMstServiceClient;
            AccountMstServiceClient = null;


            FinTrxService FinTrxServiceClient = null;
            AdmCompanyMstService admCompanyMstServiceClient;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            try
            {
                FinTrxServiceClient = new FinTrxService();
                FinTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(FinTrxServiceClient);
                switch (type)
                {
                    case ControlsEnum.CASHACCOUNT:
                        dtCashAccount = BusinessLogic.Administration.Masters.AccountMapingBL.GetMappingTypeData(0, Convert.ToInt32(DbActiveStatus.ACTIVE), (int)AccountSubType.Cash, 0);
                        break;
                    #region FORMNO
                    case ControlsEnum.FORMNO:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admConstMstList = CommonServiceClient.GetConstMstValues(null, 1, null, Convert.ToInt16(ConstGroupType.WHTFormNo), Convert.ToInt16(ConstGroup.WHTFormnoval), currentUser.SBUID);
                        break;
                    #endregion

                    case ControlsEnum.VOUCHERS:
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        finTrxHdrObj.FTH_PK = CurrPK;
                        finTrxHdrObj.FTH_REF_TYPE = VoucherType;
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrList = FinTrxServiceClient.GetfinTxtHdrList(finTrxHdrObj, serviceUtilityObj);
                        break;
                    case ControlsEnum.BASECURRENCY:
                        CurrencyMstService CurrencyMstServiceClient = new CurrencyMstService();
                        string BaseCurrency = CurrencyMstServiceClient.GetCurrencyCode(hdfCurrency.Value == string.Empty ? 0 : Convert.ToInt32(hdfCurrency.Value));
                        txtCurrency.Text = BaseCurrency;
                        break;
                    case ControlsEnum.EXCHANGERATE:
                        //Generate Exchange Rate
                        CommonServiceClient = new CommonService();
                        double ExchgRate = CommonServiceClient.GetConversionFactor(hdfCurrency.Value == string.Empty ? 0 : Convert.ToInt32(hdfCurrency.Value), currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency,
                                                             Convert.ToDateTime(txtVoucherDate.Text == string.Empty ? DateTime.Now.ToShortDateString() : txtVoucherDate.Text), currentUser.SBUID);
                        txtExchangeRate.Text = ExchgRate.ToString();
                        break;
                    case ControlsEnum.MODE:
                        CommonServiceClient = new CommonService();
                        admConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONFIG_MST>();
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admConfigMstObj.CFG_TYPE = GetLocalResourceObject("PaymentMode").ToString();
                        if (selectedModeValue > 0)
                            admConfigMstObj.CFG_VALUE = selectedModeValue;
                        admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    case ControlsEnum.APLNTYPE:
                        FinVoucherEntryCfgService FinVoucherEntryCfgServiceClient = new FinVoucherEntryCfgService();

                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;

                        finVcrEntryCfgObj = ERP.Utilities.CommonFunctions.Initilize<FIN_VOUCHER_ENTRY_CFG>();
                        finVcrEntryCfgObj.VEC_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finVcrEntryCfgObj.VEC_TYPE = VoucherType;
                        finVcrEntryCfgList = FinVoucherEntryCfgServiceClient.GetVcoucherEntryCfg(finVcrEntryCfgObj, serviceUtilityObj);
                        break;
                    case ControlsEnum.VOUCHERNO:
                        //Generate Voucher No
                        POInvoiceService poInvoiceServiceClient = new POInvoiceService();
                        poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                        //updateVocher
                        DateTime VoucherDate = DateTime.Now;
                        DateTime.TryParse(txtVoucherDate.Text.Trim(), out VoucherDate);
                        voucherNo = poInvoiceServiceClient.GetInvoiceNo(VoucherType, 0, 1, VoucherDate, currentUser.PKUser, true, 0, Convert.ToInt32(ddlCompany.SelectedValue));
                        hdfVoucherNo.Value = voucherNo;
                        poInvoiceServiceClient = null;
                        break;

                    #region FIN COA MST VALUES
                    case ControlsEnum.FINCOAMST:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        AccountMstServiceClient = new AccountMstService();
                        AccountMstServiceClient = CommonFunctions.InitiateClient(AccountMstServiceClient);
                        finCoaMstObj = new FIN_COA_MST();
                        finCoaMstObj = CommonFunctions.Initilize<FIN_COA_MST>();
                        finCoaMstObj.COA_PK = ddlAccountType;
                        finCoaMstObj.COA_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finCoaMstList = AccountMstServiceClient.GetFinCoaMst(finCoaMstObj, serviceUtilityObj);
                        break;
                    #endregion

                    #region FIN COA SUB TYPE CONFIG VALUES
                    case ControlsEnum.FINCOASUBTYPECFG:
                        commonService = new CommonService();
                        commonService = CommonFunctions.InitiateClient(commonService);
                        finCoaSubTypeCfgObj = new FIN_COA_SUB_TYPE_CFG();
                        finCoaSubTypeCfgObj = CommonFunctions.Initilize<FIN_COA_SUB_TYPE_CFG>();
                        finCoaSubTypeCfgObj.CST_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finCoaSubTypeCfgObj.CST_PK = ddlAccountSubType;
                        finCoaSubTypeCfgList = commonService.GetSubTypeCfgValues(finCoaSubTypeCfgObj);
                        break;
                    #endregion

                    case ControlsEnum.COMPANY:
                        //gets Company List
                        admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        //To get the company related to current SBU
                        dtCmp = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                        //dtCmp = BusinessLogic.CommonManagement.CommonBL.GetDeptCompany(currentUser.CurrentDeptPK);
                        break;
                    #region VENDORACCOUNT
                    case ControlsEnum.VENDORACCOUNT:
                        dtVendorAccount = CommonBL.GetTaxMstList(0, (int)TaxType.Tax, (int)TaxSubCategory.WHT, (byte)DbActiveStatus.ACTIVE, currentUser.SBUID);
                        break;
                    #endregion
                    #region VENDORACCOUNTTAXFORMULA
                    case ControlsEnum.VENDORACCOUNTTAX:
                        if (whtTaxpk > 0)
                        {
                            dtVendorAccount = CommonBL.GetTaxMstList(whtTaxpk, (int)TaxType.Tax, (int)TaxSubCategory.WHT, (byte)DbActiveStatus.HASPK, currentUser.SBUID);
                        }
                        break;
                    #endregion
                    #region VENDORACCOUNTVATBUYTAX
                    case ControlsEnum.VENDORACCOUNTVATBUYTAX:
                        if (VatBuyTaxpk > 0)
                        {
                            dtVendorAccount = CommonBL.GetTaxMstList(VatBuyTaxpk, (int)TaxType.Tax, (int)TaxSubCategory.VATBuy, (byte)DbActiveStatus.HASPK, currentUser.SBUID);
                        }
                        break;
                    #endregion
                    #region VENDOR
                    case ControlsEnum.VENDOR:
                        if (!string.IsNullOrEmpty(hdfVendor.Value))
                        {
                            CommonServiceClient = new CommonService();
                            CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                            purVendorMstList = CommonServiceClient.GetVendor(Convert.ToInt32(hdfVendor.Value));
                        }
                        break;
                    #endregion
                    #region VATBUYTAXTYPES
                    case ControlsEnum.VATBUYTAXTYPES:
                        dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue((int)TaxType.Tax, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtVoucherDate.Text), 0, TaxFilterType.PUR, (int)TaxStatus.Include, (int)DbActiveStatus.ACTIVE);
                        break;
                    #endregion
                    #region TAXDETAILS
                    case ControlsEnum.TAXDETAILS:
                        dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetTaxDetails(TaxPK, (int)TaxType.Tax, currentUser.SBUID, Convert.ToByte(DbActiveStatus.HASPK));
                        break;
                    #endregion
                    #region PAYMENTTAXHDR
                    case ControlsEnum.PAYMENTTAXHDR:
                        finPymntObj = ERP.Utilities.CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_HDR>();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        if (!string.IsNullOrEmpty(txtVatTaxInvDate.Text.Trim()))
                            finPymntObj.WTH_TAX_DATE = DateTime.Parse(txtVatTaxInvDate.Text.Trim());
                        finPymntObj.WTH_TAX_INV_NO = txtVatTaxInvNo.Text.Trim();
                        finPymntList = FinTrxServiceClient.GetfinPymntVndTxtHdrList(finPymntObj, serviceUtilityObj);
                        break;
                    #endregion
                    #region VENDORSELECTEDDTL
                    case ControlsEnum.VENDORSELECTEDDTL:
                        //dtPageData = BusinessLogic.VendorManagement.VendorRegistration.GetVendorData(vendorPopupPk);
                        if (string.IsNullOrEmpty(hdfVendorPopup.Value))
                        {
                            dtVendorDtl = BusinessLogic.VendorManagement.VendorMaster.GetVendor(currentUser, 0, Convert.ToInt16(DbActiveStatus.ACTIVE), txtVendorPopup.Text);
                        }
                        break;
                    #endregion

                    #region VENDORCONTACTYPE
                    case ControlsEnum.VENDORCONTACTYPE:
                        int.TryParse(hdfVendorPopup.Value, out VatVendorPopupPk);
                        if (VatVendorPopupPk > 0)
                            dsAdsType = BusinessLogic.POInvoicing.POInvoiceBL.GetVendorAddressTypes(currentUser, Convert.ToInt16(DbActiveStatus.ACTIVE), 0, VatVendorPopupPk, 0);
                        if (dsAdsType != null && dsAdsType.Tables.Count > 0 && dsAdsType.Tables[0].Rows.Count > 0)
                            dtAdsType = dsAdsType.Tables[0];
                        break;
                    #endregion
                    #region VENDORCONTACTYPEDETAILS
                    case ControlsEnum.VENDORCONTACTYPEDETAILS:
                        // if (ddlAddressType.SelectedValue != CommonConstants.SELECTVAL)
                        int.TryParse(hdfAddressType.Value, out VncPk);
                        int.TryParse(hdfVendorPopup.Value, out VatVendorPopupPk);
                        if (VatVendorPopupPk > 0 && VncPk > 0)
                            dsAdsTypeDtl = BusinessLogic.POInvoicing.POInvoiceBL.GetVendorAddressTypes(currentUser, Convert.ToInt16(DbActiveStatus.HASPK), VncPk, VatVendorPopupPk, 0);
                        if (dsAdsTypeDtl != null && dsAdsTypeDtl.Tables.Count > 0 && dsAdsTypeDtl.Tables[0].Rows.Count > 0)
                            dtAdsTypeDtl = dsAdsTypeDtl.Tables[0];
                        break;
                    #endregion

                    #region VENDORTYPES
                    case ControlsEnum.VENDORTYPES:
                        byte[] Values = new byte[] { (int)VendorContactTypeEnum.Branch, (int)VendorContactTypeEnum.HeadOffice };
                        CommonServiceClient = new CommonService();
                        admConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONFIG_MST>();
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admConfigMstObj.CFG_TYPE = GetLocalResourceObject("VendorContactType").ToString();
                        if (selectedModeValue > 0)
                            admConfigMstObj.CFG_VALUE = selectedModeValue;
                        TempConfigMstDetails = CommonServiceClient.GetConfigValues(admConfigMstObj).Where(vcl => Values.Contains(vcl.CFG_VALUE)).ToList();
                        break;
                    #endregion

                    #region VENDORCONTACTFORWHT
                    case ControlsEnum.VENDORCONTACTFORWHT:
                        int.TryParse(hdfWthAddressType.Value, out VncPk);
                        int.TryParse(hdfVendor.Value, out VatVendorPopupPk);
                        if (VatVendorPopupPk > 0 && VncPk > 0)
                            dsAdsTypeDtl = BusinessLogic.POInvoicing.POInvoiceBL.GetVendorAddressTypes(currentUser, Convert.ToInt16(DbActiveStatus.HASPK), VncPk, VatVendorPopupPk, 0);
                        if (dsAdsTypeDtl != null && dsAdsTypeDtl.Tables.Count > 0 && dsAdsTypeDtl.Tables[0].Rows.Count > 0)
                            dtAdsTypeDtl = dsAdsTypeDtl.Tables[0];
                        break;
                    #endregion
                    #region Payment Types
                    case ControlsEnum.PAYMENTTYPE:
                        dtPaymentTypes = BusinessLogic.POInvoicing.POInvoiceBL.GetPaymentTypes(Convert.ToInt32(DbActiveStatus.ACTIVE), "WHT PAYMENT TYPE");
                        break;
                    #endregion
                    #region WHTVENDOR
                    case ControlsEnum.WHTVENDOR:
                        if (string.IsNullOrEmpty(hdfVendor.Value))
                        {
                            dtVendorDtl = BusinessLogic.VendorManagement.VendorMaster.GetVendor(currentUser, 0, Convert.ToInt16(DbActiveStatus.ACTIVE), txtTo.Text);
                        }
                        break;
                    #endregion

                    #region FIN HEADER
                    case ControlsEnum.FINHEADER:
                        //FinTrxServiceClient = new FinTrxService();
                        //FinTrxServiceClient = CommonFunctions.InitiateClient(FinTrxServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_REF_TYPE = Session[ERP.Utilities.SessionStrings.TransactionType].ToString();
                        finTrxHdrObj.FTH_REF_PK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString());
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrList = FinTrxServiceClient.GetfinTxtHdrList(finTrxHdrObj, serviceUtilityObj);
                        break;
                    #endregion
                    #region GETDIRECTPAYMENTPKBYJOURNALPK
                    case ControlsEnum.GETDIRECTPAYMENTPKBYJOURNALPK:
                        FinTrxServiceClient = new FinTrxService();
                        FinTrxServiceClient = CommonFunctions.InitiateClient(FinTrxServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_PK = JournalPK;
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrListJournal = FinTrxServiceClient.GetfinTxtHdrList(finTrxHdrObj, serviceUtilityObj);
                        break;
                    #endregion
                    //#region VENDORCONTACTYPEDETAILS
                    //case ControlsEnum.VENDORCONTACTYPEDETAILS:
                    //    {
                    //        //active,ven_pk,bizunit,type 
                    //        dtAdsTypeDtl = BusinessLogic.POInvoicing.POInvoiceBL.GetAddressTypeDtls(currentUser, Convert.ToInt16(DbActiveStatus.ACTIVE), hdfVendorPopup.Value == "" ? 0 : Convert.ToInt16(hdfVendorPopup.Value), ddlAddressType.SelectedValue == "" ? 0 : Convert.ToInt16(ddlAddressType.SelectedValue)).Tables[0];
                    //    }
                    //    break;
                    //#endregion
                    #region COST CENTER
                    case ControlsEnum.COSTCENTER:
                        dtCostCenter = BusinessLogic.CommonManagement.CommonBL.GetCostCenter(selAccountPk, CCAmount);
                        break;
                    #endregion
                    #region AUDIT LOG STATUS
                    case ControlsEnum.AUDITLOGSTATUS:
                        dtStatus = BusinessLogic.CommonManagement.CommonBL.GetAuditLogDisplayStatus(CurrPK, VoucherType);
                        break;
                    #endregion
                    #region AUDIT LOG DETAILS
                    case ControlsEnum.AUDITLOGDETAILS:
                        dtAuditDetails = BusinessLogic.CommonManagement.CommonBL.GetAuditLogDetails(CurrPK);
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
                //finTrxObj = null;
                serviceUtilityObj = null;
                FinTrxServiceClient = null;
                AccountMstServiceClient = null;
                commonService = null;
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
                    case ControlsEnum.CASHACCOUNT:
                        BindDropDown(ControlsEnum.CASHACCOUNT);
                        break;
                    case ControlsEnum.FORMNO:
                        BindDropDown(ControlsEnum.FORMNO);
                        break;
                    case ControlsEnum.VOUCHERS:
                        GetUIValuesFromObject(ControlsEnum.VOUCHERS);
                        BindGrid();
                        break;
                    case ControlsEnum.MODE:
                        BindModeDropDown();
                        break;
                    case ControlsEnum.APLNTYPE:
                        SetVoucherTypeValues();
                        break;
                    case ControlsEnum.FINCOASUBTYPECFG:
                        BindSubTypeDropDown();
                        break;
                    case ControlsEnum.COMPANY:
                        //if (dtCmp != null && dtCmp.Rows.Count > 0)
                        //    CompanyPK = Convert.ToInt32(dtCmp.Rows[0][Resources.DataFieldRes.DeptCompany].ToString());
                        //else
                        //    CompanyPK = 0;
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    case ControlsEnum.WHTPOPUPGRID:
                        BindGrid(ControlsEnum.WHTPOPUPGRID);
                        break;
                    case ControlsEnum.VATPOPUPGRID:
                        BindGrid(ControlsEnum.VATPOPUPGRID);
                        break;
                    case ControlsEnum.VENDORACCOUNT:
                        BindDropDown(ControlsEnum.VENDORACCOUNT);
                        break;
                    case ControlsEnum.VENDORACCOUNTTAX:
                        GetUIValuesFromObject(ControlsEnum.VENDORACCOUNTTAX);
                        break;
                    case ControlsEnum.VENDOR:
                        GetUIValuesFromObject(ControlsEnum.VENDOR);
                        break;
                    case ControlsEnum.VATBUYVENDOR:
                        GetUIValuesFromObject(ControlsEnum.VATBUYVENDOR);
                        break;
                    case ControlsEnum.VATBUYTAXTYPES:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.VENDORCONTACTYPE:
                        GetUIValuesFromObject(ControlsEnum.VENDORCONTACTYPE);
                        break;
                    case ControlsEnum.VENDORSELECTEDDTL:
                        GetUIValuesFromObject(ControlsEnum.VENDORSELECTEDDTL);
                        break;
                    case ControlsEnum.VENDORCONTACTYPEDETAILS:
                        GetUIValuesFromObject(ControlsEnum.VENDORCONTACTYPEDETAILS);
                        break;
                    case ControlsEnum.PAYMENTTYPE:
                        BindDropDown(ControlsEnum.PAYMENTTYPE);
                        break;
                    case ControlsEnum.VENDORCONTACTFORWHT:
                        GetUIValuesFromObject(ControlsEnum.VENDORCONTACTFORWHT);
                        break;
                    case ControlsEnum.WHTVENDOR:
                        GetUIValuesFromObject(ControlsEnum.WHTVENDOR);
                        break;
                    #region COST CENTER
                    case ControlsEnum.COSTCENTER:
                        BindGrid(ControlsEnum.COSTCENTER);
                        break;
                    #endregion
                    //case ControlsEnum.AUDITLOGDETAILS:
                    //    BindGrid(ControlsEnum.AUDITLOGDETAILS);
                    //    break;
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
        private object SetUIValuesToObject(ActionsEnum actionType, Object srcObj)
        {
            try
            {
                FinTrxHeaderBO objFinTrxHeader = null;
                List<FIN_TRX_COC_DTL> lstCocDtl = null;
                object returnObj;
                returnObj = null;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                int sequence = 0;
                int DebitSeqNo = 0;
                int CreditSeqNo = 1000;
                int slno = 1;

                switch (actionType)
                {
                    #region Save
                    case ActionsEnum.SAVE:
                        objFinTrxHeader = new FinTrxHeaderBO();

                        // finTrxHdrObj = CommonFunctions.Initilize<FIN_TRX_HDR>();
                        objFinTrxHeader.FTH_PK = CurrPK;
                        //FTH_NARRATION FTH_FIN_YEAR FTH_IS_JRNLD FTH_STATUS
                        objFinTrxHeader.FTH_DATE = string.IsNullOrEmpty(txtVoucherDate.Text.Trim()) ? DateTime.Now : DateTime.Parse(txtVoucherDate.Text.Trim());

                        txtVoucherNo.Text = hdfVoucherNo.Value;
                        objFinTrxHeader.FTH_VOUCHER_NO = txtVoucherNo.Text;
                        objFinTrxHeader.FTH_REF_TYPE = VoucherType;
                        objFinTrxHeader.APT_CODE = VoucherType;
                        objFinTrxHeader.AST_VALUE = "0";
                        objFinTrxHeader.FTH_REF_PK = hdfVecPk.Value == string.Empty ? 0 : Convert.ToInt64(hdfVecPk.Value);
                        objFinTrxHeader.FTH_REF_NO = txtRefNo.Text.Trim();
                        objFinTrxHeader.FTH_REF_DATE = string.IsNullOrEmpty(txtRefDate.Text.Trim()) ? DateTime.Now : DateTime.Parse(txtRefDate.Text.Trim());
                        objFinTrxHeader.FTH_NARRATION = txtNarration.Text;
                        objFinTrxHeader.FTH_TRX_CURR = Convert.ToInt32(hdfCurrency.Value);
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        objFinTrxHeader.FTH_EXCHG_RATE = Convert.ToDouble(txtExchangeRate.Text);
                        objFinTrxHeader.FTH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                        objFinTrxHeader.FTH_FIN_YEAR = 0;
                        objFinTrxHeader.FTH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                        objFinTrxHeader.FTH_PARTY_NAME = (txtTo.Text.Equals("Select/Type")) ? "" : HttpUtility.HtmlEncode(txtTo.Text.Trim());
                        objFinTrxHeader.FTH_STATUS = 0;
                        objFinTrxHeader.WKF_FLAG = 0;

                        //if (VoucherType == ApplicationType.OBV)
                        //{
                        //    finTrxHdrObj.FTH_STATUS = 2;
                        //}
                        //else if (VoucherType == ApplicationType.DPVJ)
                        //{
                        //    finTrxHdrObj.FTH_STATUS = 0;
                        //}
                        //For Type OBV
                        objFinTrxHeader.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        objFinTrxHeader.FTH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                        objFinTrxHeader.FTH_CRTD_DT = DateTime.Now;
                        objFinTrxHeader.FTH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                        objFinTrxHeader.FTH_MOD_DT = LastModifiedTime;
                        objFinTrxHeader.FTH_DEPT = Session[BusinessObject.Common.SessionStrings.CurDept] == null ? currentUser.CurrentDeptPK : Convert.ToInt16(Session[BusinessObject.Common.SessionStrings.CurDept].ToString());
                        objFinTrxHeader.FTH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        objFinTrxHeader.FTH_IS_JRNLD = false;
                        objFinTrxHeader.FTH_WHT_CERT_AUTO = Convert.ToInt32(hdfCRTNoAuto.Value);
                        //if (CompanyPK > 0)
                        //{
                        //    finTrxHdrObj.FTH_COMPANY = CompanyPK;
                        //}
                        //else
                        //{
                        //    finTrxHdrObj.FTH_COMPANY = Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE);
                        //}
                        if (!string.IsNullOrEmpty(ddlCompany.SelectedValue))
                        {
                            objFinTrxHeader.FTH_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                        }
                        else
                        {
                            objFinTrxHeader.FTH_COMPANY = Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE);
                        }
                        finTrxList = new List<FIN_TRX>();
                        List<FIN_TRX> ftrList = new List<FIN_TRX>();

                        //finTrxDetailsList = new List<FinTrxDetailsBO>();
                        List<FinTrxDetailsBO> finTrxDetailsList = new List<FinTrxDetailsBO>();

                        if (Session["VoucherDet"] != null) finTrxList = (List<FIN_TRX>)(Session["VoucherDet"]);

                        if (finTrxList != null && finTrxList.Count > 0)
                            finTrxList = (finTrxList.OrderBy(ftr => ftr.FTR_PK)).ToList();
                        slno = 1;
                        foreach (FIN_TRX ftrObj in finTrxList)
                        {
                            //finTrxObj = CommonFunctions.Initilize<FIN_TRX>();
                            FinTrxDetailsBO finTrxDetailsObj = new FinTrxDetailsBO();
                            finTrxDetailsObj.FTR_PK = 0;// CurrFtrPK;  Old Code Comened Date 08-03-2024 By uno
                            //finTrxDetailsObj.FTR_PK = ftrObj.FTR_PK;
                            finTrxDetailsObj.FTR_SL_NO = slno++;
                            finTrxDetailsObj.FTR_TRX_HDR = CurrPK;
                            if (finTrxDetailsObj.FTR_DR_AMT_TC > 0)
                            {
                                sequence = DebitSeqNo;
                                DebitSeqNo++;
                            }
                            else
                            {
                                sequence = CreditSeqNo;
                                CreditSeqNo++;
                            }

                            finTrxDetailsObj.FTR_SEQUENCE = (short)sequence;
                            finTrxDetailsObj.FTR_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                            finTrxDetailsObj.FTR_CRTD_DT = DateTime.Now;
                            finTrxDetailsObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                            finTrxDetailsObj.FTR_MOD_DT = DateTime.Now;
                            finTrxDetailsObj.FTR_REMARKS = string.Empty;
                            finTrxDetailsObj.FTR_ACC_SUB_TYPE = ftrObj.FTR_ACC_SUB_TYPE;
                            finTrxDetailsObj.FTR_ACCOUNT = ftrObj.FTR_ACCOUNT;
                            finTrxDetailsObj.FTR_ACTIVE = ftrObj.FTR_ACTIVE;
                            finTrxDetailsObj.FTR_BIZUNIT = ftrObj.FTR_BIZUNIT;
                            if (ftrObj.FTR_CLEAR_DATE.HasValue)
                            {
                                finTrxDetailsObj.FTR_CLEAR_DATE = Convert.ToString(ftrObj.FTR_CLEAR_DATE);
                            }
                            finTrxDetailsObj.FTR_CR_AMT_BC = ftrObj.FTR_CR_AMT_BC;
                            finTrxDetailsObj.FTR_CR_AMT_TC = ftrObj.FTR_CR_AMT_TC;
                            finTrxDetailsObj.FTR_DEPT = ftrObj.FTR_DEPT;
                            finTrxDetailsObj.FTR_DR_AMT_BC = ftrObj.FTR_DR_AMT_BC;
                            finTrxDetailsObj.FTR_DR_AMT_TC = ftrObj.FTR_DR_AMT_TC;
                            if (ftrObj.FTR_INSTR_DATE.HasValue)
                            {
                                finTrxDetailsObj.FTR_INSTR_DATE = Convert.ToString(ftrObj.FTR_INSTR_DATE);
                            }
                            finTrxDetailsObj.FTR_INSTR_FAVOUR = ftrObj.FTR_INSTR_FAVOUR;
                            finTrxDetailsObj.FTR_INSTR_NO = ftrObj.FTR_INSTR_NO;
                            finTrxDetailsObj.FTR_IS_RECONCILED = ftrObj.FTR_IS_RECONCILED;
                            finTrxDetailsObj.FTR_NARRATION = ftrObj.FTR_NARRATION;
                            if (ftrObj.FTR_PAYMENT_MODE.HasValue && ftrObj.FTR_PAYMENT_MODE >= 0)
                                finTrxDetailsObj.FTR_PAYMENT_MODE = Convert.ToString(ftrObj.FTR_PAYMENT_MODE);
                            finTrxDetailsObj.FTR_TYPE = ftrObj.FTR_TYPE;
                            finTrxDetailsObj.FTR_TYPE_PK = Convert.ToInt32(ftrObj.FTR_TYPE_PK);
                            finTrxDetailsObj.FTR_EXCHG_RATE = Convert.ToDouble(txtExchangeRate.Text);
                            finTrxDetailsObj.FTR_PDC = Convert.ToByte(ftrObj.FTR_PDC);
                            finTrxDetailsObj.FTR_IS_BANK_CHARGE = ftrObj.FTR_IS_BANK_CHARGE;


                            finTrxDetailsObj.FTR_VENDOR_CODE = ftrObj.FTR_VENDOR_CODE;
                            finTrxDetailsObj.FTR_INV_DATE = ftrObj.FTR_INV_DATE;
                            finTrxDetailsObj.FTR_INV_NO = ftrObj.FTR_INV_NO;
                            finTrxDetailsObj.FTR_REF_PO_NUMBER = ftrObj.FTR_REF_PO_NUMBER;
                            finTrxDetailsObj.FTR_AMT_BFR_VAT = ftrObj.FTR_AMT_BFR_VAT;

                            #region Costcenter Mapping
                            if (ftrObj.FIN_TRX_COC_DTL != null && ftrObj.FIN_TRX_COC_DTL.Count > 0)
                            {
                                finTrxDetailsObj.CostCenterList = new List<FinCostCenterBO>();
                                lstCocDtl = ftrObj.FIN_TRX_COC_DTL.ToList();
                                foreach (FIN_TRX_COC_DTL cocDtl in lstCocDtl)
                                {
                                    FinCostCenterBO dataCoc = new FinCostCenterBO();
                                    dataCoc.FTD_SL_NO = finTrxDetailsObj.FTR_SL_NO;
                                    dataCoc.FTD_PK = cocDtl.FTD_PK;
                                    dataCoc.FTD_FTR_PK = cocDtl.FTD_FTR_PK;
                                    dataCoc.FTD_CNM_PK = cocDtl.FTD_CNM_PK;
                                    dataCoc.FTD_AMT_TC = cocDtl.FTD_AMT_TC;
                                    dataCoc.FTD_AMT_BC = cocDtl.FTD_AMT_BC;
                                    finTrxDetailsObj.CostCenterList.Add(dataCoc);
                                }
                            }
                            #endregion

                            finTrxDetailsList.Add(finTrxDetailsObj);
                            //sequence++;

                        }


                        //if (ftrList != null && ftrList.Count > 0)
                        //{
                        //    ftrList.ForEach(dtl => objFinTrxHeader.FIN_TRX.Add(dtl));
                        //    finTrxHdrObj.FTH_PDC = (ftrList.Where(pdc => pdc.FTR_PDC == 1).Count()) > 0 ? (byte)1 : (byte)0;
                        //}

                        if (finTrxDetailsList != null && finTrxDetailsList.Count > 0)
                        {
                            objFinTrxHeader.FinTrxDetails = new List<FinTrxDetailsBO>();
                            finTrxDetailsList.ForEach(dtl => objFinTrxHeader.FinTrxDetails.Add(dtl));
                            objFinTrxHeader.FTH_PDC = (finTrxDetailsList.Where(pdc => pdc.FTR_PDC == 1).Count()) > 0 ? (byte)1 : (byte)0;
                        }


                        TextBox txtWHTAmount = (TextBox)grdVoucher.FooterRow.FindControl("txtWHTAmount");
                        decimal whtAmount = string.IsNullOrEmpty(hdfWithHoldTax.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(hdfWithHoldTax.Value);
                        if (whtAmount > 0 && txtWHTAmount != null && !string.IsNullOrEmpty(txtWHTAmount.Text))
                        {
                            // finPaymentVndTaxHdrList = new List<FIN_PAYMENT_VND_TAX_HDR>();

                            if (WHTTaxDetails != null && WHTTaxDetails.Count > 0)
                            {
                                List<FinPaymentVndTaxHeaderBO> ItemList = new List<FinPaymentVndTaxHeaderBO>();
                                FinPaymentVndTaxHeaderBO objTemp = new FinPaymentVndTaxHeaderBO();
                                if (objFinTrxHeader.FinPaymentVndTaxHdr == null)
                                    objFinTrxHeader.FinPaymentVndTaxHdr = new List<FinPaymentVndTaxHeaderBO>();
                                // FIN_PAYMENT_VND_TAX_HDR objTemp;
                                //  List<FIN_PAYMENT_VND_TAX_HDR> ItemList = new List<FIN_PAYMENT_VND_TAX_HDR>();
                                foreach (FIN_PAYMENT_VND_TAX_HDR objItem in WHTTaxDetails)
                                {
                                    // objTemp = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_HDR>();
                                    objTemp = new FinPaymentVndTaxHeaderBO();
                                    objTemp.WTH_AMOUNT = objItem.WTH_AMOUNT;
                                    objTemp.WTH_TAX_AMT = objItem.WTH_TAX_AMT;

                                    objTemp.WTH_TAX = objItem.WTH_TAX;
                                    objTemp.WTH_PK = 0;
                                    //objTemp.WTH_PAYMENT_HDR = null;
                                    objTemp.WTH_TRX_HDR = objItem.WTH_TRX_HDR;
                                    objTemp.WTH_TYPE = objItem.WTH_TYPE;
                                    objTemp.WTH_TAX_CATEGORY = objItem.WTH_TAX_CATEGORY;
                                    objTemp.WTH_NAME = objItem.WTH_NAME;
                                    objTemp.WTH_DESC = objItem.WTH_DESC;
                                    objTemp.WTH_FORM_NO = objItem.WTH_FORM_NO;
                                    objTemp.WTH_PARTY_NAME = objItem.WTH_PARTY_NAME;
                                    objTemp.WTH_ADDRESS = objItem.WTH_ADDRESS;
                                    objTemp.WTH_ADDRESS = objItem.WTH_ADDRESS;
                                    objTemp.WTH_TAX_ID = objItem.WTH_TAX_ID;
                                    objTemp.WTH_CATEGORY = objItem.WTH_CATEGORY;
                                    objTemp.WTH_TAX_DATE = objItem.WTH_TAX_DATE;
                                    objTemp.WTH_BRANCH = objItem.WTH_BRANCH;
                                    objTemp.WTH_BRANCH_NAME = objItem.WTH_BRANCH_NAME;
                                    objTemp.WTH_BRANCH_TEXT = objItem.WTH_BRANCH_TEXT;
                                    objTemp.WTH_BRANCH_TYPE = objItem.WTH_BRANCH_TYPE;
                                    objTemp.WTH_PAYMENT_TYPE = objItem.WTH_PAYMENT_TYPE;
                                    objTemp.WTH_CERT_NO = objItem.WTH_CERT_NO;
                                    ItemList.Add(objTemp);
                                }
                                ItemList.ForEach(dtl => objFinTrxHeader.FinPaymentVndTaxHdr.Add(dtl));
                            }
                        }

                        TextBox txtVatBuy = (TextBox)grdVoucher.FooterRow.FindControl("txtVatBuy");
                        decimal vatAmount = string.IsNullOrEmpty(hdfVatBuy.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(hdfVatBuy.Value);
                        if (vatAmount > 0 && txtVatBuy != null && !string.IsNullOrEmpty(txtVatBuy.Text))
                        {
                            //finPaymentVndTaxHdrList = new List<FIN_PAYMENT_VND_TAX_HDR>();

                            if (VATTaxDetails != null && VATTaxDetails.Count > 0)
                            {
                                FinPaymentVndTaxHeaderBO objTemp = new FinPaymentVndTaxHeaderBO();
                                List<FinPaymentVndTaxHeaderBO> ItemList = new List<FinPaymentVndTaxHeaderBO>();
                                if (objFinTrxHeader.FinPaymentVndTaxHdr == null)
                                    objFinTrxHeader.FinPaymentVndTaxHdr = new List<FinPaymentVndTaxHeaderBO>();
                                foreach (FIN_PAYMENT_VND_TAX_HDR objItem in VATTaxDetails)
                                {
                                    // objTemp = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_HDR>();
                                    objTemp = new FinPaymentVndTaxHeaderBO();
                                    objTemp.WTH_AMOUNT = objItem.WTH_AMOUNT;
                                    objTemp.WTH_TAX_AMT = objItem.WTH_TAX_AMT;

                                    objTemp.WTH_TAX = objItem.WTH_TAX;
                                    objTemp.WTH_PK = 0;
                                    //objTemp.WTH_PAYMENT_HDR = null;
                                    objTemp.WTH_TRX_HDR = objItem.WTH_TRX_HDR;
                                    objTemp.WTH_TYPE = objItem.WTH_TYPE;
                                    objTemp.WTH_TAX_CATEGORY = objItem.WTH_TAX_CATEGORY;
                                    objTemp.WTH_NAME = objItem.WTH_NAME;
                                    objTemp.WTH_DESC = objItem.WTH_DESC;
                                    //objTemp.WTH_FORM_NO = objItem.WTH_FORM_NO;
                                    objTemp.WTH_PARTY_NAME = objItem.WTH_PARTY_NAME;
                                    //objTemp.WTH_ADDRESS = objItem.WTH_ADDRESS;                                    
                                    objTemp.WTH_TAX_ID = objItem.WTH_TAX_ID;
                                    objTemp.WTH_CATEGORY = objItem.WTH_CATEGORY;
                                    objTemp.WTH_TAX_DATE = objItem.WTH_TAX_DATE;
                                    if (objItem.WTH_REFUND_DATE.HasValue)
                                        objTemp.WTH_REFUND_DATE = Convert.ToString(objItem.WTH_REFUND_DATE);
                                    if (objItem.WTH_VENDOR.HasValue)
                                        objTemp.WTH_VENDOR = Convert.ToString(objItem.WTH_VENDOR);
                                    objTemp.WTH_BRANCH = objItem.WTH_BRANCH;
                                    objTemp.WTH_BRANCH_NAME = objItem.WTH_BRANCH_NAME;
                                    objTemp.WTH_BRANCH_TEXT = objItem.WTH_BRANCH_TEXT;
                                    objTemp.WTH_BRANCH_TYPE = objItem.WTH_BRANCH_TYPE;
                                    objTemp.WTH_ITEM_TEXT = objItem.WTH_ITEM_TEXT;
                                    objTemp.WTH_TAX_INV_NO = objItem.WTH_TAX_INV_NO;
                                    objTemp.WTH_INV_RECEIVED = objItem.WTH_INV_RECEIVED;
                                    objTemp.WTH_CERT_NO = objItem.WTH_CERT_NO;
                                    ItemList.Add(objTemp);
                                }
                                ItemList.ForEach(dtl => objFinTrxHeader.FinPaymentVndTaxHdr.Add(dtl));

                            }
                        }

                        break;
                    #endregion

                    #region Workflow Submit
                    case ActionsEnum.WRKFSUBMIT:
                        objFinTrxHeader = new FinTrxHeaderBO();

                        // finTrxHdrObj = CommonFunctions.Initilize<FIN_TRX_HDR>();
                        objFinTrxHeader.FTH_PK = CurrPK;
                        //FTH_NARRATION FTH_FIN_YEAR FTH_IS_JRNLD FTH_STATUS
                        objFinTrxHeader.FTH_DATE = string.IsNullOrEmpty(txtVoucherDate.Text.Trim()) ? DateTime.Now : DateTime.Parse(txtVoucherDate.Text.Trim());

                        txtVoucherNo.Text = hdfVoucherNo.Value;
                        objFinTrxHeader.FTH_VOUCHER_NO = txtVoucherNo.Text;
                        objFinTrxHeader.FTH_REF_TYPE = VoucherType;
                        //For Generate Number
                        objFinTrxHeader.APT_CODE = VoucherType;
                        objFinTrxHeader.AST_VALUE = "0";

                        objFinTrxHeader.FTH_REF_PK = hdfVecPk.Value == string.Empty ? 0 : Convert.ToInt64(hdfVecPk.Value);
                        objFinTrxHeader.FTH_REF_NO = txtRefNo.Text.Trim();
                        objFinTrxHeader.FTH_REF_DATE = string.IsNullOrEmpty(txtRefDate.Text.Trim()) ? DateTime.Now : DateTime.Parse(txtRefDate.Text.Trim());
                        objFinTrxHeader.FTH_NARRATION = txtNarration.Text;
                        objFinTrxHeader.FTH_TRX_CURR = Convert.ToInt32(hdfCurrency.Value);
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        objFinTrxHeader.FTH_EXCHG_RATE = Convert.ToDouble(txtExchangeRate.Text);
                        objFinTrxHeader.FTH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                        objFinTrxHeader.FTH_FIN_YEAR = 0;
                        objFinTrxHeader.FTH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                        objFinTrxHeader.FTH_PARTY_NAME = (txtTo.Text.Equals("Select/Type")) ? "" : HttpUtility.HtmlEncode(txtTo.Text.Trim());
                        //For Type OBV
                        objFinTrxHeader.FTH_STATUS = 2;
                        objFinTrxHeader.WKF_FLAG = 1;
                        //if (VoucherType == ApplicationType.OBV)
                        //{
                        //    objFinTrxHeader.FTH_STATUS = 2;
                        //}
                        //else
                        if (VoucherType == ApplicationType.DPVJ || VoucherType == ApplicationType.PCVJ || VoucherType == ApplicationType.PCRVJ)
                        {
                            objFinTrxHeader.FTH_STATUS = 1;
                        }
                        objFinTrxHeader.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        objFinTrxHeader.FTH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                        objFinTrxHeader.FTH_CRTD_DT = DateTime.Now;
                        objFinTrxHeader.FTH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                        objFinTrxHeader.FTH_MOD_DT = LastModifiedTime;
                        objFinTrxHeader.FTH_DEPT = Session[BusinessObject.Common.SessionStrings.CurDept] == null ? currentUser.CurrentDeptPK : Convert.ToInt16(Session[BusinessObject.Common.SessionStrings.CurDept].ToString());
                        objFinTrxHeader.FTH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        objFinTrxHeader.FTH_IS_JRNLD = true;
                        if (!string.IsNullOrEmpty(ddlCompany.SelectedValue))
                        {
                            objFinTrxHeader.FTH_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                        }
                        else
                        {
                            objFinTrxHeader.FTH_COMPANY = Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE);
                        }

                        finTrxList = new List<FIN_TRX>();
                        ftrList = new List<FIN_TRX>();
                        List<FinTrxDetailsBO> finTrxDtlList = new List<FinTrxDetailsBO>();
                        if (Session["VoucherDet"] != null) finTrxList = (List<FIN_TRX>)(Session["VoucherDet"]);
                        if (finTrxList != null && finTrxList.Count > 0)
                            finTrxList = (finTrxList.OrderBy(ftr => ftr.FTR_PK)).ToList();
                        slno = 1;
                        foreach (FIN_TRX ftrObj in finTrxList)
                        {
                            FinTrxDetailsBO finTrxDetailsObj = new FinTrxDetailsBO();

                            // finTrxObj = CommonFunctions.Initilize<FIN_TRX>();

                            finTrxDetailsObj.FTR_PK = 0;// CurrFtrPK; //previous code
                          //  finTrxDetailsObj.FTR_PK = ftrObj.FTR_PK;
                            finTrxDetailsObj.FTR_SL_NO = slno++;
                            finTrxDetailsObj.FTR_TRX_HDR = CurrPK;
                            if (ftrObj.FTR_DR_AMT_TC > 0)
                            {
                                sequence = DebitSeqNo;
                                DebitSeqNo++;
                            }
                            else
                            {
                                sequence = CreditSeqNo;
                                CreditSeqNo++;
                            }
                            finTrxDetailsObj.FTR_SEQUENCE = (short)sequence;
                            finTrxDetailsObj.FTR_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                            finTrxDetailsObj.FTR_CRTD_DT = DateTime.Now;
                            finTrxDetailsObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                            finTrxDetailsObj.FTR_MOD_DT = DateTime.Now;
                            finTrxDetailsObj.FTR_REMARKS = string.Empty;
                            finTrxDetailsObj.FTR_ACC_SUB_TYPE = ftrObj.FTR_ACC_SUB_TYPE;
                            finTrxDetailsObj.FTR_ACCOUNT = ftrObj.FTR_ACCOUNT;
                            finTrxDetailsObj.FTR_ACTIVE = ftrObj.FTR_ACTIVE;
                            finTrxDetailsObj.FTR_BIZUNIT = ftrObj.FTR_BIZUNIT;
                            if (ftrObj.FTR_CLEAR_DATE.HasValue)
                                finTrxDetailsObj.FTR_CLEAR_DATE = Convert.ToString(ftrObj.FTR_CLEAR_DATE);
                            finTrxDetailsObj.FTR_CR_AMT_BC = ftrObj.FTR_CR_AMT_BC;
                            finTrxDetailsObj.FTR_CR_AMT_TC = ftrObj.FTR_CR_AMT_TC;
                            finTrxDetailsObj.FTR_DEPT = ftrObj.FTR_DEPT;
                            finTrxDetailsObj.FTR_DR_AMT_BC = ftrObj.FTR_DR_AMT_BC;
                            finTrxDetailsObj.FTR_DR_AMT_TC = ftrObj.FTR_DR_AMT_TC;
                            if (ftrObj.FTR_INSTR_DATE.HasValue)
                            {
                                finTrxDetailsObj.FTR_INSTR_DATE = Convert.ToString(ftrObj.FTR_INSTR_DATE);
                            }
                            finTrxDetailsObj.FTR_INSTR_FAVOUR = ftrObj.FTR_INSTR_FAVOUR;
                            finTrxDetailsObj.FTR_INSTR_NO = ftrObj.FTR_INSTR_NO;
                            finTrxDetailsObj.FTR_IS_RECONCILED = ftrObj.FTR_IS_RECONCILED;
                            finTrxDetailsObj.FTR_NARRATION = ftrObj.FTR_NARRATION;
                            if (ftrObj.FTR_PAYMENT_MODE.HasValue)
                                finTrxDetailsObj.FTR_PAYMENT_MODE = Convert.ToString(ftrObj.FTR_PAYMENT_MODE);
                            finTrxDetailsObj.FTR_TYPE = ftrObj.FTR_TYPE;
                            finTrxDetailsObj.FTR_TYPE_PK = Convert.ToInt32(ftrObj.FTR_TYPE_PK);
                            finTrxDetailsObj.FTR_EXCHG_RATE = Convert.ToDouble(txtExchangeRate.Text);
                            finTrxDetailsObj.FTR_PDC = Convert.ToByte(ftrObj.FTR_PDC);
                            finTrxDetailsObj.FTR_IS_BANK_CHARGE = ftrObj.FTR_IS_BANK_CHARGE;

                            finTrxDetailsObj.FTR_VENDOR_CODE = ftrObj.FTR_VENDOR_CODE;
                            finTrxDetailsObj.FTR_INV_DATE = ftrObj.FTR_INV_DATE;
                            finTrxDetailsObj.FTR_INV_NO = ftrObj.FTR_INV_NO;
                            finTrxDetailsObj.FTR_REF_PO_NUMBER = ftrObj.FTR_REF_PO_NUMBER;
                            finTrxDetailsObj.FTR_AMT_BFR_VAT = ftrObj.FTR_AMT_BFR_VAT;
                            // finTrxDetailsObj.FTR_V
                            #region Costcenter Mapping
                            if (ftrObj.FIN_TRX_COC_DTL != null && ftrObj.FIN_TRX_COC_DTL.Count > 0)
                            {
                                finTrxDetailsObj.CostCenterList = new List<FinCostCenterBO>();
                                lstCocDtl = ftrObj.FIN_TRX_COC_DTL.ToList();
                                foreach (FIN_TRX_COC_DTL cocDtl in lstCocDtl)
                                {
                                    FinCostCenterBO dataCoc = new FinCostCenterBO();
                                    dataCoc.FTD_SL_NO = finTrxDetailsObj.FTR_SL_NO;
                                    dataCoc.FTD_PK = cocDtl.FTD_PK;
                                    dataCoc.FTD_FTR_PK = cocDtl.FTD_FTR_PK;
                                    dataCoc.FTD_CNM_PK = cocDtl.FTD_CNM_PK;
                                    dataCoc.FTD_AMT_TC = cocDtl.FTD_AMT_TC;
                                    dataCoc.FTD_AMT_BC = cocDtl.FTD_AMT_BC;
                                    finTrxDetailsObj.CostCenterList.Add(dataCoc);
                                }
                            }
                            #endregion
                            finTrxDtlList.Add(finTrxDetailsObj);
                            // sequence++;
                        }

                        //if (ftrList != null && ftrList.Count > 0)
                        //{
                        //    ftrList.ForEach(dtl => finTrxHdrObj.FIN_TRX.Add(dtl));
                        //    finTrxHdrObj.FTH_PDC = (ftrList.Where(pdc => pdc.FTR_PDC == 1).Count()) > 0 ? (byte)1 : (byte)0;
                        //}
                        if (finTrxDtlList != null && finTrxDtlList.Count > 0)
                        {
                            objFinTrxHeader.FinTrxDetails = new List<FinTrxDetailsBO>();
                            finTrxDtlList.ForEach(dtl => objFinTrxHeader.FinTrxDetails.Add(dtl));
                            objFinTrxHeader.FTH_PDC = (finTrxDtlList.Where(pdc => pdc.FTR_PDC == 1).Count()) > 0 ? (byte)1 : (byte)0;
                        }

                        TextBox txtWHTAmount1 = (TextBox)grdVoucher.FooterRow.FindControl("txtWHTAmount");
                        decimal whtAmnt = string.IsNullOrEmpty(hdfWithHoldTax.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(hdfWithHoldTax.Value);
                        if (whtAmnt > 0 && txtWHTAmount1 != null && !string.IsNullOrEmpty(txtWHTAmount1.Text))
                        {
                            finPaymentVndTaxHdrList = new List<FIN_PAYMENT_VND_TAX_HDR>();
                            if (WHTTaxDetails != null && WHTTaxDetails.Count > 0)
                            {

                                FinPaymentVndTaxHeaderBO objTemp = new FinPaymentVndTaxHeaderBO();
                                List<FinPaymentVndTaxHeaderBO> ItemList = new List<FinPaymentVndTaxHeaderBO>();
                                if (objFinTrxHeader.FinPaymentVndTaxHdr == null)
                                    objFinTrxHeader.FinPaymentVndTaxHdr = new List<FinPaymentVndTaxHeaderBO>();
                                foreach (FIN_PAYMENT_VND_TAX_HDR objItem in WHTTaxDetails)
                                {
                                    // objTemp = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_HDR>();
                                    objTemp = new FinPaymentVndTaxHeaderBO();
                                    objTemp.WTH_AMOUNT = objItem.WTH_AMOUNT;
                                    objTemp.WTH_TAX_AMT = objItem.WTH_TAX_AMT;

                                    objTemp.WTH_TAX = objItem.WTH_TAX;
                                    objTemp.WTH_PK = 0;
                                    //objTemp.WTH_PAYMENT_HDR = null;
                                    objTemp.WTH_TRX_HDR = objItem.WTH_TRX_HDR;
                                    objTemp.WTH_TYPE = objItem.WTH_TYPE;
                                    objTemp.WTH_TAX_CATEGORY = objItem.WTH_TAX_CATEGORY;
                                    objTemp.WTH_NAME = objItem.WTH_NAME;
                                    objTemp.WTH_DESC = objItem.WTH_DESC;
                                    objTemp.WTH_FORM_NO = objItem.WTH_FORM_NO;
                                    objTemp.WTH_PARTY_NAME = objItem.WTH_PARTY_NAME;
                                    objTemp.WTH_ADDRESS = objItem.WTH_ADDRESS;
                                    objTemp.WTH_ADDRESS = objItem.WTH_ADDRESS;
                                    objTemp.WTH_TAX_ID = objItem.WTH_TAX_ID;
                                    objTemp.WTH_CATEGORY = objItem.WTH_CATEGORY;
                                    objTemp.WTH_TAX_DATE = objItem.WTH_TAX_DATE;
                                    objTemp.WTH_BRANCH = objItem.WTH_BRANCH;
                                    objTemp.WTH_BRANCH_NAME = objItem.WTH_BRANCH_NAME;
                                    objTemp.WTH_BRANCH_TEXT = objItem.WTH_BRANCH_TEXT;
                                    objTemp.WTH_BRANCH_TYPE = objItem.WTH_BRANCH_TYPE;
                                    objTemp.WTH_PAYMENT_TYPE = objItem.WTH_PAYMENT_TYPE;
                                    objTemp.WTH_CERT_NO = objItem.WTH_CERT_NO;
                                    ItemList.Add(objTemp);
                                }

                                ItemList.ForEach(dtl => objFinTrxHeader.FinPaymentVndTaxHdr.Add(dtl));
                            }
                        }

                        TextBox txtVatBuy1 = (TextBox)grdVoucher.FooterRow.FindControl("txtVatBuy");
                        decimal vatAmnt = string.IsNullOrEmpty(hdfVatBuy.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(hdfVatBuy.Value);
                        if (vatAmnt > 0 && txtVatBuy1 != null && !string.IsNullOrEmpty(txtVatBuy1.Text))
                        {
                            finPaymentVndTaxHdrList = new List<FIN_PAYMENT_VND_TAX_HDR>();
                            if (VATTaxDetails != null && VATTaxDetails.Count > 0)
                            {
                                FinPaymentVndTaxHeaderBO objTemp = new FinPaymentVndTaxHeaderBO();
                                List<FinPaymentVndTaxHeaderBO> ItemList = new List<FinPaymentVndTaxHeaderBO>();
                                if (objFinTrxHeader.FinPaymentVndTaxHdr == null)
                                    objFinTrxHeader.FinPaymentVndTaxHdr = new List<FinPaymentVndTaxHeaderBO>();
                                foreach (FIN_PAYMENT_VND_TAX_HDR objItem in VATTaxDetails)
                                {
                                    // objTemp = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_HDR>();
                                    objTemp = new FinPaymentVndTaxHeaderBO();
                                    objTemp.WTH_AMOUNT = objItem.WTH_AMOUNT;
                                    objTemp.WTH_TAX_AMT = objItem.WTH_TAX_AMT;

                                    objTemp.WTH_TAX = objItem.WTH_TAX;
                                    objTemp.WTH_PK = 0;
                                    //objTemp.WTH_PAYMENT_HDR = null;
                                    objTemp.WTH_TRX_HDR = objItem.WTH_TRX_HDR;
                                    objTemp.WTH_TYPE = objItem.WTH_TYPE;
                                    objTemp.WTH_TAX_CATEGORY = objItem.WTH_TAX_CATEGORY;
                                    objTemp.WTH_NAME = objItem.WTH_NAME;
                                    objTemp.WTH_DESC = objItem.WTH_DESC;
                                    //objTemp.WTH_FORM_NO = objItem.WTH_FORM_NO;
                                    objTemp.WTH_PARTY_NAME = objItem.WTH_PARTY_NAME;
                                    //objTemp.WTH_ADDRESS = objItem.WTH_ADDRESS;                                    
                                    objTemp.WTH_TAX_ID = objItem.WTH_TAX_ID;
                                    objTemp.WTH_CATEGORY = objItem.WTH_CATEGORY;
                                    objTemp.WTH_TAX_DATE = objItem.WTH_TAX_DATE;
                                    if (objItem.WTH_REFUND_DATE.HasValue)
                                    {
                                        objTemp.WTH_REFUND_DATE = Convert.ToString(objItem.WTH_REFUND_DATE);
                                    }
                                    if (objItem.WTH_VENDOR.HasValue)
                                    {
                                        objTemp.WTH_VENDOR = Convert.ToString(objItem.WTH_VENDOR);
                                    }
                                    objTemp.WTH_BRANCH = objItem.WTH_BRANCH;
                                    objTemp.WTH_BRANCH_NAME = objItem.WTH_BRANCH_NAME;
                                    objTemp.WTH_BRANCH_TEXT = objItem.WTH_BRANCH_TEXT;
                                    objTemp.WTH_BRANCH_TYPE = objItem.WTH_BRANCH_TYPE;
                                    objTemp.WTH_CERT_NO = objItem.WTH_CERT_NO;
                                    objTemp.WTH_ITEM_TEXT = objItem.WTH_ITEM_TEXT;
                                    objTemp.WTH_TAX_INV_NO = objItem.WTH_TAX_INV_NO;
                                    objTemp.WTH_INV_RECEIVED = objItem.WTH_INV_RECEIVED;
                                    ItemList.Add(objTemp);
                                }
                                ItemList.ForEach(dtl => objFinTrxHeader.FinPaymentVndTaxHdr.Add(dtl));

                            }
                        }
                        //if (vatAmnt > 0)
                        //{
                        //    finPaymentVndTaxHdrList = new List<FIN_PAYMENT_VND_TAX_HDR>();

                        //    if (VATTaxDetails != null && VATTaxDetails.Count > 0)
                        //    {

                        //        FIN_PAYMENT_VND_TAX_HDR objTemp;
                        //        List<FIN_PAYMENT_VND_TAX_HDR> ItemList = new List<FIN_PAYMENT_VND_TAX_HDR>();
                        //        foreach (FIN_PAYMENT_VND_TAX_HDR objItem in VATTaxDetails)
                        //        {
                        //            objTemp = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_HDR>();
                        //            objTemp.WTH_AMOUNT = objItem.WTH_AMOUNT;
                        //            objTemp.WTH_TAX_AMT = objItem.WTH_TAX_AMT;

                        //            objTemp.WTH_TAX = objItem.WTH_TAX;
                        //            objTemp.WTH_PK = 0;
                        //            //objTemp.WTH_PAYMENT_HDR = null;
                        //            objTemp.WTH_TRX_HDR = objItem.WTH_TRX_HDR;
                        //            objTemp.WTH_TYPE = objItem.WTH_TYPE;
                        //            objTemp.WTH_TAX_CATEGORY = objItem.WTH_TAX_CATEGORY;
                        //            objTemp.WTH_NAME = objItem.WTH_NAME;
                        //            objTemp.WTH_DESC = objItem.WTH_DESC;
                        //            //objTemp.WTH_FORM_NO = objItem.WTH_FORM_NO;
                        //            objTemp.WTH_PARTY_NAME = objItem.WTH_PARTY_NAME;
                        //            //objTemp.WTH_ADDRESS = objItem.WTH_ADDRESS;                                    
                        //            objTemp.WTH_TAX_ID = objItem.WTH_TAX_ID;
                        //            objTemp.WTH_CATEGORY = objItem.WTH_CATEGORY;
                        //            objTemp.WTH_TAX_DATE = objItem.WTH_TAX_DATE;
                        //            objTemp.WTH_BRANCH_TEXT = objItem.WTH_BRANCH_TEXT;
                        //            objTemp.WTH_BRANCH_TYPE = objItem.WTH_BRANCH_TYPE;
                        //            objTemp.WTH_ITEM_TEXT = objItem.WTH_ITEM_TEXT;                                     
                        //            ItemList.Add(objTemp);
                        //        }

                        //        ItemList.ForEach(dtl => finTrxHdrObj.FIN_PAYMENT_VND_TAX_HDR.Add(dtl));
                        //    }
                        //}

                        break;
                    #endregion

                    #region REVERSE/CHEQUERETURN
                    case ActionsEnum.JOURNALIZE:
                    case ActionsEnum.REVERSE:
                    case ActionsEnum.CHEQUERETURN:
                        //if (EntryStatus == EntryStatus.LISTMODE)
                        //{
                        //    foreach (GridViewRow grdrow in grdPOPaymentHdr.Rows)
                        //    {
                        //        RadioButton rbtn;
                        //        rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                        //        if (rbtn.Checked)
                        //        {
                        //            bIsChecked = true;
                        //            CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPaymentID")).Value);
                        //            Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                        //            Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                        //            Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).Text;
                        //            break;
                        //        }
                        //    }
                        //}

                        //else if (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.VIEWMODE)
                        //{
                        //    bIsChecked = true;
                        //}
                        if (CurrPK > 0 && finTrxHdrList != null && finTrxHdrList.Count > 0)
                        {
                            if (finTrxHdrList[0].FTH_STATUS != 0)
                            {

                                Session[ERP.Utilities.SessionStrings.TrxPK] = null;

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

                                if (actionType == ActionsEnum.REVERSE)
                                {
                                    Session[ERP.Utilities.SessionStrings.TransactionType] = ucrJournalize.TransactionType = ApplicationType.DPVCJ;
                                    ucrJournalize.JournalType = (int)JournalTypeEnum.Reverse;
                                }
                                else if (actionType == ActionsEnum.CHEQUERETURN)
                                {
                                    Session[ERP.Utilities.SessionStrings.TransactionType] = ucrJournalize.TransactionType = ApplicationType.DPBJ;
                                    ucrJournalize.JournalType = (int)JournalTypeEnum.Return;
                                }

                                ucrJournalize.TransactionPK = (int)CurrPK;
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                                ucrJournalize.JournalizePK = 0;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                //GetFieldValues(ControlsEnum.PAYMENTHDRENTRYBYPK);
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = finTrxHdrList[0].FTH_VOUCHER_NO;
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = finTrxHdrList[0].FTH_DATE;
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = finTrxHdrList[0].FTH_TRX_CURR;
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.VND;
                                Session[ERP.Utilities.SessionStrings.AccountPayablePK] = null;
                                Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.DPVJ;

                                ucrWrkf.WrkfSubmit -= ActionHandler;
                                ucrWrkf.Reset();
                                ucrWrkf.ViewType = 1;


                                if (actionType == ActionsEnum.REVERSE)
                                {
                                    FillProcessID(ApplicationType.DPVCJ, 0);
                                }
                                else if (actionType == ActionsEnum.CHEQUERETURN)
                                {
                                    FillProcessID(ApplicationType.DPBJ, 0);
                                }
                                GetFieldValues(ControlsEnum.FINHEADER);
                                //EntryStatus = EntryStatus.ENTRYMODE;
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                                {

                                    ucrWrkf.RefID = workflowCore.GetRefID((int)finTrxHdrList[0].FTH_PK, ucrWrkf.ProcessID);
                                    base.WkfRefID = ucrWrkf.RefID;
                                }
                                //SetCancelRef((int)CurrPK);
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

                                TextBox txtNarrationUcr = (TextBox)ucrJournalize.FindControl("txtNarration");
                                txtNarrationUcr.Text = "";

                                TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                WrkfComments.Text = "";
                                Session[ERP.Utilities.SessionStrings.RemoveRowIndex] = null;
                                hdfJournalizeWorkFlow.Value = "1";
                                //int mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);
                                //ucrJournalize.TypeForNumberGenaration = mode == (int)PaymentModeEnum.CASH ? "1" : "0";
                                ucrJournalize.CallUserControl();

                                if (actionType == ActionsEnum.REVERSE)
                                {
                                    Session[ERP.Utilities.SessionStrings.JournalHead] = hdfJournalName.Value = GetLocalResourceObject("PDC_Voucher").ToString();
                                }
                                else if (actionType == ActionsEnum.CHEQUERETURN)
                                {
                                    Session[ERP.Utilities.SessionStrings.JournalHead] = hdfJournalName.Value = GetLocalResourceObject("Return_Voucher").ToString();
                                }
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
                            {
                                litErrorMsg.Text = Resources.Report.PmtNtApp;
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    default:
                        break;
                }
                returnObj = objFinTrxHeader;// finTrxHdrObj;
                return returnObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                finTrxHdrObj = null;
            }
        }

        private bool IsValidSession()
        {
            bool result = true;
            if (GetGlobalResourceObject("ConfigurationsRes", "MultiTabRestriction").ToString() == "1")
            {
                finTrxList = Session["VoucherDet"] == null ? new List<FIN_TRX>() : finTrxList = (List<FIN_TRX>)(Session["VoucherDet"]);
                if (finTrxList.Count > 0)
                    result = finTrxList.Count(i => (i.FTR_RECORD_IDENTITY != RecordIdentity && (!string.IsNullOrEmpty(i.FTR_RECORD_IDENTITY)))) == 0;
            }
            return result;
        }

        /// <summary>
        /// For Bind Cancelation comment on workflow user control
        /// </summary>
        /// <param name="curPK"></param>
        private void SetCancelRef(int curPK)
        {
            #region Cancel ref Setting
            //WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            //string TYPE = Request.QueryString[QueryStrings.PageType] != null ? Request.QueryString[QueryStrings.PageType] : string.Empty;
            //if (TYPE != "3")//Type 3 for cancelation
            //{
            //    DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
            //    if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
            //    {
            //        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
            //        ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
            //    }
            //}
            #endregion
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
                    #region VOUCHERS
                    case ControlsEnum.VOUCHERS:
                        if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                        {
                            CurrPK = Convert.ToInt32(finTrxHdrList[0].FTH_PK);

                            hdfStatus.Value = finTrxHdrList[0].FTH_STATUS.ToString();

                            WHTTaxDetails = finTrxHdrList[0].FIN_PAYMENT_VND_TAX_HDR.Where(whtpynt => whtpynt.WTH_CATEGORY == (byte)WHTCategoryEnum.WHT).ToList();
                            VATTaxDetails = finTrxHdrList[0].FIN_PAYMENT_VND_TAX_HDR.Where(whtpynt => whtpynt.WTH_CATEGORY == (byte)WHTCategoryEnum.VATBUY).ToList();
                            finPayemtVndHdrList = finTrxHdrList[0].FIN_PAYMENT_VND_TAX_HDR.ToList();
                            TempWHTTaxDetails = finPayemtVndHdrList.Where(whtpynt => whtpynt.WTH_CATEGORY == (byte)WHTCategoryEnum.WHT).ToList();
                            TempVATTaxDetails = finPayemtVndHdrList.Where(whtpynt => whtpynt.WTH_CATEGORY == (byte)WHTCategoryEnum.VATBUY).ToList();

                            hdfVoucherNo.Value = finTrxHdrList[0].FTH_VOUCHER_NO;
                            txtVoucherNo.Text = string.IsNullOrEmpty(finTrxHdrList[0].FTH_VOUCHER_NO) ? Resources.ErpRes.Draft : finTrxHdrList[0].FTH_VOUCHER_NO;
                            txtVoucherDate.Text = Convert.ToDateTime(finTrxHdrList[0].FTH_DATE).ToString("dd-MMM-yyyy");
                            #region Checking for voucher locked with financial year
                            if (finTrxHdrList[0].FTH_DATE.HasValue)
                            {
                                string LockUptoDate = string.Empty;
                                if (BusinessLogic.Finance.VoucherLockingBL.IsVoucherLocked(finTrxHdrList[0].FTH_DATE.Value, CurrPK, finTrxHdrList[0].FTH_BIZUNIT, ref LockUptoDate))
                                {
                                    txtVoucherDate.Enabled = false;
                                }
                            }
                            #endregion
                            txtRefNo.Text = finTrxHdrList[0].FTH_REF_NO;
                            txtRefDate.Text = finTrxHdrList[0].FTH_REF_DATE.ToString("dd-MMM-yyyy");
                            hdfCurrency.Value = finTrxHdrList[0].FTH_BASE_CURR.ToString();
                            GetFieldValues(ControlsEnum.BASECURRENCY);
                            txtExchangeRate.Text = finTrxHdrList[0].FTH_EXCHG_RATE.ToString();
                            txtRemarks.Text = HttpUtility.HtmlDecode(finTrxHdrList[0].FTH_REMARKS);
                            txtTo.Text = string.IsNullOrEmpty(finTrxHdrList[0].FTH_PARTY_NAME) ? "Select/Type" : HttpUtility.HtmlDecode(finTrxHdrList[0].FTH_PARTY_NAME);
                            LastModifiedTime = finTrxHdrList[0].FTH_MOD_DT;
                            ddlCompany.SelectedValue = finTrxHdrList[0].FTH_COMPANY.ToString();
                            finTrxList = new List<FIN_TRX>();

                            hdfShowPdc.Value = "0";
                            if (finTrxHdrList[0].FTH_IS_JRNLD)
                            {
                                if (VoucherType != ApplicationType.JV && finTrxHdrList[0].FIN_TRX.Where(r => Convert.ToInt32(r.ADM_CONFIG_MST.CFG_VALUE) == (int)PaymentModeEnum.Cheque).Count() > 0)// if (Convert.ToInt16(Mode) == (int)BusinessObject.CommonManagement.PaymentModeEnum.Cheque)
                                {
                                    if (finTrxHdrList[0].FTH_PDC != 0)
                                    {
                                        hdfShowPdc.Value = "1";
                                        if (finTrxHdrList[0].FTH_PDC > 1)
                                            hdfShowChequeReturn.Value = "1";
                                        else
                                            hdfShowChequeReturn.Value = "0";
                                    }
                                    else
                                        hdfShowChequeReturn.Value = "1";
                                }
                                else
                                {
                                    hdfShowChequeReturn.Value = "0";
                                }
                            }

                            foreach (FIN_TRX ftrObj in finTrxHdrList[0].FIN_TRX)
                            {
                                admConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONFIG_MST>();
                                if (AccountType == 1)
                                {
                                    //////ftrObj.ADM_CONFIG_MST.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.HASPK);
                                    admConfigMstObj.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.HASPK);
                                }
                                else if (AccountType == 2)
                                {
                                    //////ftrObj.ADM_CONFIG_MST.CFG_ACTIVE = ftrObj.FTR_DR_AMT_BC == 0 ? Convert.ToByte(DbActiveStatus.HASPK) : Convert.ToByte(DbActiveStatus.ACTIVE);
                                    admConfigMstObj.CFG_ACTIVE = ftrObj.FTR_DR_AMT_BC == 0 ? Convert.ToByte(DbActiveStatus.HASPK) : Convert.ToByte(DbActiveStatus.ACTIVE);
                                }
                                else if (AccountType == 3)
                                {
                                    //////ftrObj.ADM_CONFIG_MST.CFG_ACTIVE = ftrObj.FTR_DR_AMT_BC == 0 ? Convert.ToByte(DbActiveStatus.ACTIVE) : Convert.ToByte(DbActiveStatus.HASPK);
                                    admConfigMstObj.CFG_ACTIVE = ftrObj.FTR_DR_AMT_BC == 0 ? Convert.ToByte(DbActiveStatus.ACTIVE) : Convert.ToByte(DbActiveStatus.HASPK);
                                }
                                if (ftrObj.ADM_CONFIG_MST != null)
                                {
                                    admConfigMstObj.CFG_PK = ftrObj.ADM_CONFIG_MST.CFG_PK;
                                    admConfigMstObj.CFG_DATA = ftrObj.ADM_CONFIG_MST.CFG_DATA;
                                }
                                ftrObj.ADM_CONFIG_MST = admConfigMstObj;

                                finTrxList.Add(ftrObj);
                            }

                            Session["VoucherDet"] = finTrxList;

                        }
                        GetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
                        SetFieldValues(ControlsEnum.VENDORACCOUNTTAX);

                        break;
                    #endregion
                    #region TAXTYPECHANGED
                    case ControlsEnum.TAXTYPECHANGED:
                        if (!string.IsNullOrEmpty(ddlVATAccountPopup.SelectedValue))
                        {
                            if (Convert.ToInt32(ddlVATAccountPopup.SelectedValue) > 0)
                            {
                                TaxPK = Convert.ToInt32(ddlVATAccountPopup.SelectedValue);
                                hdfVATAccountPopup.Value = ddlVATAccountPopup.SelectedValue;
                                //GetFieldValues(ControlsEnum.VATBUYTAXTYPES);
                                GetFieldValues(ControlsEnum.TAXDETAILS);
                                TaxPK = 0;
                                if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
                                {
                                    string taxFormula = dtTaxDetails.Rows[0][Resources.DataFieldRes.TaxFormula].ToString();
                                    hdfTaxformula.Value = taxFormula;
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", txtBeforeTaxAmount.Text.Trim());
                                    txtVATTaxAmountPopup.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                                    //SelectedTaxText = HttpUtility.HtmlEncode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                    //txtPopupOther.Text = ddlVATAccountPopup.SelectedItem.Text;
                                    //txtVATTaxAmountPopup.Enabled = false;
                                    //txtPopupOther.Enabled = false;
                                }
                            }
                            else if (Convert.ToInt32(ddlVATAccountPopup.SelectedValue) == -1)
                            {
                                hdfVATAccountPopup.Value = string.Empty;
                                hdfTaxformula.Value = string.Empty;
                                txtVATTaxAmountPopup.Text = string.Empty;
                                //SelectedTaxText = Resources.Report.Custom;
                                // txtPopupOther.Text = string.Empty;
                                //txtPopupAmount.Enabled = true;
                                //txtPopupOther.Enabled = true;
                            }
                        }
                        break;
                    #endregion
                    #region VENDORCONTACTYPE
                    case ControlsEnum.VENDORCONTACTYPE:

                        if (dtAdsType != null && dtAdsType.Rows.Count > 0)
                        {
                            chkHeadOffice.Checked = false;
                            txtBranchCode.Text = dtAdsType.Rows[0][Resources.DataFieldRes.VncTypeName].ToString();
                            txtVatTaxId.Text = dtAdsType.Rows[0][Resources.DataFieldRes.VncTaxNo].ToString();
                            if (string.IsNullOrEmpty(txtVatTaxId.Text))
                                txtVatTaxId.Text = dtAdsType.Rows[0][Resources.DataFieldRes.VendorTaxId].ToString();
                            hdfAddressType.Value = dtAdsType.Rows[0][Resources.DataFieldRes.VncPk].ToString();
                            txtAddressType.Text = dtAdsType.Rows[0][Resources.DataFieldRes.VncName].ToString();
                            if (Convert.ToInt32(dtAdsType.Rows[0][Resources.DataFieldRes.vncType]) == (int)VendorContactTypeEnum.HeadOffice)
                            {
                                chkHeadOffice.Checked = true;
                                HeadofficeCheckedChanged();
                                if (string.IsNullOrEmpty(txtBranchCode.Text))
                                    txtBranchCode.Text = GetLocalResourceObject("DefaultCodeForHo").ToString();
                            }
                            //hdfVendorContactType.Value = dsAdsTypeDtl.Tables[0].Rows[0][Resources.DataFieldRes.vncType].ToString();
                        }
                        break;
                    #endregion
                    #region VENDORCONTACTYPEDETAILS
                    case ControlsEnum.VENDORCONTACTYPEDETAILS:
                        //if (dtAdsTypeDtl != null && dtAdsTypeDtl.Rows.Count > 0)
                        //{
                        //    txtBranchCode.Text = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VncTypeName].ToString();
                        //    txtVatTaxId.Text = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VncTaxNo].ToString();
                        //}
                        if (dtAdsTypeDtl != null && dtAdsTypeDtl.Rows.Count > 0)
                        {
                            chkHeadOffice.Checked = false;
                            txtBranchCode.Text = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VncTypeName].ToString();
                            txtVatTaxId.Text = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VncTaxNo].ToString();
                            if (string.IsNullOrEmpty(txtVatTaxId.Text))
                                txtVatTaxId.Text = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VendorTaxId].ToString();
                            if (Convert.ToInt32(dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.vncType]) == (int)VendorContactTypeEnum.HeadOffice)
                            {
                                chkHeadOffice.Checked = true;
                                HeadofficeCheckedChanged();
                                if (string.IsNullOrEmpty(txtBranchCode.Text))
                                    txtBranchCode.Text = GetLocalResourceObject("DefaultCodeForHo").ToString();
                            }
                            //hdfVendorContactType.Value = dsAdsTypeDtl.Tables[0].Rows[0][Resources.DataFieldRes.vncType].ToString();
                        }
                        break;
                    #endregion
                    #region VENDORSELECTEDDTL
                    case ControlsEnum.VENDORSELECTEDDTL:
                        if (dtVendorDtl != null && dtVendorDtl.Rows.Count > 0)
                        {
                            hdfVendorPopup.Value = dtVendorDtl.Rows[0][Resources.DataFieldRes.VendorPK].ToString();
                        }

                        break;
                    #endregion
                    #region VATBUYVENDOR
                    case ControlsEnum.VATBUYVENDOR:
                        if (purVendorMstList != null && purVendorMstList.Count > 0)
                        {
                            txtVatTaxId.Text = purVendorMstList[0].VEN_TIN;
                            txtVendorPopup.Text = HttpUtility.HtmlDecode(purVendorMstList[0].VEN_NAME);
                            hdfVendorPopup.Value = purVendorMstList[0].VEN_PK.ToString();
                            //txtAddressType.Text = purVendorMstList[0].VEN_WAREHOUSE_DTL.ToString();  
                            //ddlAddressType.SelectedItem.Text = purVendorMstList[0].VEN_WAREHOUSE_DTL.ToString(); 
                        }
                        else
                        {
                            txtVendorPopup.Text = HttpUtility.HtmlDecode(txtTo.Text);
                        }
                        break;
                    #endregion
                    #region VENDOR
                    case ControlsEnum.VENDOR:
                        if (purVendorMstList != null && purVendorMstList.Count > 0)
                        {
                            txtCustomerTxtWHT.Text = !string.IsNullOrEmpty(purVendorMstList[0].VEN_NAME2) ? purVendorMstList[0].VEN_NAME2 : ERP.Utilities.CommonFunctions.GetShortString(purVendorMstList[0].VEN_NAME, 300);
                            txtCustomerTxtWHT.Text = HttpUtility.HtmlDecode(txtCustomerTxtWHT.Text);
                            //txtCustomerTxtWHT.ToolTip = purVendorMstList[0].VEN_NAME2;
                            hdfvendorWHTPK.Value = purVendorMstList[0].VEN_PK.ToString();
                            txtpartyads.Text = !string.IsNullOrEmpty(purVendorMstList[0].VEN_ADDR3) ? purVendorMstList[0].VEN_ADDR3 : purVendorMstList[0].VEN_ADDR1 + " " + purVendorMstList[0].VEN_ADDR2;
                            txtpartyads.Text = HttpUtility.HtmlDecode(txtpartyads.Text);
                            txtTaxid.Text = purVendorMstList[0].VEN_TIN;
                        }
                        else
                        {
                            txtCustomerTxtWHT.Text = HttpUtility.HtmlDecode(txtTo.Text);
                        }
                        break;
                    #endregion
                    #region VENDORACCOUNTTAX
                    case ControlsEnum.VENDORACCOUNTTAX:
                        if (dtVendorAccount != null && dtVendorAccount.Rows.Count > 0)
                        {
                            //if (chkVendorforpayemnt.Checked)
                            //{
                            //decimal amount = 0;

                            //amount = txtPaidAmount.Text != string.Empty ? Convert.ToDecimal(txtPaidAmount.Text) : 0;
                            //amount = amount + (txtWHTAmount.Text != string.Empty ? Convert.ToDecimal(txtWHTAmount.Text) : 0);
                            string taxFormula = dtVendorAccount.Rows[0][Resources.DataFieldRes.TaxFormula].ToString();
                            hdfTaxformula.Value = taxFormula;
                            //taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());

                            //decimal amt = Convert.ToDecimal(StringToFormula(taxFormula));
                            //txtWHTAmount.Text = Math.Round(amt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //}
                            //else
                            //{
                            //    txtWHTAmount.Text = "0";
                            //}
                        }
                        else
                        {
                            //  txtWHTAmount.Text = "0";
                        }
                        break;
                    #endregion
                    #region VENDORCONTACTFORWHT
                    case ControlsEnum.VENDORCONTACTFORWHT:
                        if (dtAdsTypeDtl != null && dtAdsTypeDtl.Rows.Count > 0)
                        {
                            chkWthHeadOffice.Checked = false;
                            txtWthBranchCode.Text = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VncTypeName].ToString();
                            txtTaxid.Text = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VncTaxNo].ToString();
                            if (string.IsNullOrEmpty(txtTaxid.Text))
                                txtTaxid.Text = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VendorTaxId].ToString();
                            if (Convert.ToInt32(dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.vncType]) == (int)VendorContactTypeEnum.HeadOffice)
                            {
                                chkWthHeadOffice.Checked = true;
                                if (string.IsNullOrEmpty(txtWthBranchCode.Text))
                                    txtWthBranchCode.Text = GetLocalResourceObject("DefaultCodeForHo").ToString();
                            }
                        }
                        else
                        {
                            chkWthHeadOffice.Checked = false;
                            txtWthBranchCode.Text = string.Empty;
                        }
                        break;
                    #endregion
                    #region WHTVENDOR
                    case ControlsEnum.WHTVENDOR:
                        if (dtVendorDtl != null && dtVendorDtl.Rows.Count > 0)
                        {
                            hdfVendor.Value = dtVendorDtl.Rows[0][Resources.DataFieldRes.VendorPK].ToString();
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
        /// Method for Grid binding
        /// </summary>
        public void BindGrid()
        {
            try
            {
                if (finTrxList != null && finTrxList.Count > 0)
                    finTrxList = (finTrxList.OrderBy(ftr => ftr.FTR_PK)).ToList();
                //finTrxList = (finTrxList.OrderByDescending(ftr => ftr.FTR_PK)).ToList();

                grdVoucher.DataSource = finTrxList;
                grdVoucher.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
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

                    case ControlsEnum.DEFAULT:
                        grdVoucher.DataSource = null;
                        grdVoucher.DataBind();
                        break;

                    case ControlsEnum.WHTPOPUPGRID:
                        //if (WHTTaxDetails != null && WHTTaxDetails.Count > 0)
                        //{
                        grdWHTTaxDetails.DataSource = TempWHTTaxDetails;
                        grdWHTTaxDetails.DataBind();
                        // }
                        break;
                    case ControlsEnum.VATPOPUPGRID:
                        //if (WHTTaxDetails != null && WHTTaxDetails.Count > 0)
                        //{
                        grdVATTaxDetails.DataSource = TempVATTaxDetails;
                        grdVATTaxDetails.DataBind();
                        // }
                        break;

                    #region COSTCENTER
                    case ControlsEnum.COSTCENTER:
                        if (CostCenterTempList != null && CostCenterTempList.Count > 0)
                            grdVoucherCC.DataSource = CostCenterTempList;
                        else
                            grdVoucherCC.DataSource = null;
                        grdVoucherCC.DataBind();
                        break;

                    #endregion
                    //case ControlsEnum.AUDITLOGDETAILS:
                    //    grdAuditLog.DataSource = dtAuditDetails;
                    //    grdAuditLog.DataBind();
                    //    break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Method for Mode Dropdown
        /// </summary>
        public void BindModeDropDown()
        {
            ddlMode.Items.Clear();
            if (admConfigMstList != null && admConfigMstList.Count > 0)
            {
                ddlMode.DataSource = admConfigMstList;
                ddlMode.DataTextField = Resources.DataFieldRes.ConfigName;
                ddlMode.DataValueField = Resources.DataFieldRes.ConfigPK;
                ddlMode.DataBind();
            }
            //ddlMode.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));

            ddlMode1.Items.Clear();
            if (admConfigMstList != null && admConfigMstList.Count > 0)
            {
                ddlMode1.DataSource = admConfigMstList;
                ddlMode1.DataTextField = Resources.DataFieldRes.ConfigName;
                ddlMode1.DataValueField = Resources.DataFieldRes.ConfigPK;
                ddlMode1.DataBind();
            }
            if (VoucherType == ApplicationType.JV)
                ddlMode1.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
        }

        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region FormNo
                case ControlsEnum.FORMNO:
                    ddlFormno.Items.Clear();
                    if (admConstMstList != null && admConstMstList.Count > 0)
                    {
                        ddlFormno.DataSource = admConstMstList;
                        ddlFormno.DataTextField = Resources.DataFieldRes.ConstName;
                        ddlFormno.DataValueField = Resources.DataFieldRes.ConstPK;
                        ddlFormno.DataBind();
                    }
                    ddlFormno.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));

                    break;
                #endregion
                //#region VENDORCONTACTYPE
                //case ControlsEnum.VENDORCONTACTYPE:
                //    ddlAddressType.Items.Clear();
                //    if (TempConfigMstDetails != null && TempConfigMstDetails.Count > 0)
                //    {
                //        ddlAddressType.DataSource = TempConfigMstDetails;
                //        ddlAddressType.DataTextField = Resources.DataFieldRes.cfgData;
                //        ddlAddressType.DataValueField = Resources.DataFieldRes.cfgValue;
                //        ddlAddressType.DataBind();
                //    }
                //    ddlAddressType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));

                //    break;
                //#endregion
                case ControlsEnum.CASHACCOUNT:
                    ddlCashAccount.Items.Clear();
                    if (dtCashAccount != null && dtCashAccount.Rows.Count > 0)
                    {
                        ddlCashAccount.DataSource = dtCashAccount;
                        ddlCashAccount.DataTextField = Resources.DataFieldRes.Bank_Name;
                        ddlCashAccount.DataValueField = Resources.DataFieldRes.AccountPK;
                        ddlCashAccount.DataBind();
                    }
                    if (dtCashAccount.Rows.Count > 1)
                        ddlCashAccount.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #region VATBUYTAXTYPES
                case ControlsEnum.VATBUYTAXTYPES:
                    //Bind Tax dropdown
                    ddlVATAccountPopup.Items.Clear();
                    if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
                    {
                        ddlVATAccountPopup.DataSource = CommonFunctions.HtmlDecodeDataTable(dtTaxDetails, Resources.DataFieldRes.RFQResponseTaxHead);
                        ddlVATAccountPopup.DataTextField = Resources.DataFieldRes.RFQResponseTaxHead;
                        ddlVATAccountPopup.DataValueField = Resources.DataFieldRes.RFQResponseTaxPK;
                        ddlVATAccountPopup.DataBind();
                    }
                    //ddlVATAccountPopup.Items.Add(new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Vendor Account
                case ControlsEnum.VENDORACCOUNT:
                    //ddlWHTAccountPopup.Items.Clear();
                    //if (dtVendorAccount != null && dtVendorAccount.Rows.Count > 0)
                    //{
                    //    ddlWHTAccountPopup.DataSource = dtVendorAccount;
                    //    ddlWHTAccountPopup.DataTextField = Resources.DataFieldRes.RFQResponseTaxHead;
                    //    ddlWHTAccountPopup.DataValueField = Resources.DataFieldRes.RFQResponseTaxPK;
                    //    ddlWHTAccountPopup.DataBind();
                    //}
                    //ddlWHTAccountPopup.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
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


                    break;
                #endregion
                #region Payment Types
                case ControlsEnum.PAYMENTTYPE:
                    ddlPayType.DataSource = dtPaymentTypes;
                    ddlPayType.DataTextField = Resources.DataFieldRes.cfgData;
                    ddlPayType.DataValueField = Resources.DataFieldRes.cfgValue;
                    ddlPayType.DataBind();
                    ddlPayType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                    #endregion

            }
        }

        /// <summary>
        /// Method for Sub Type Dropdown
        /// </summary>
        public void BindSubTypeDropDown()
        {
            ddlSubTypeAccount1.Items.Clear();
            ddlSubTypeAccount.Items.Clear();
            if (SubTypesAccounts != null && SubTypesAccounts.Count > 0)
            {
                foreach (DDLMaster itm in SubTypesAccounts)//*****For Decoding*****************      
                {
                    itm.Value = HttpUtility.HtmlDecode(itm.Value);
                }
                ddlSubTypeAccount1.DataSource = SubTypesAccounts;
                ddlSubTypeAccount1.DataTextField = "Value";
                ddlSubTypeAccount1.DataValueField = "PK";
                ddlSubTypeAccount1.DataBind();

                ddlSubTypeAccount.DataSource = SubTypesAccounts;
                ddlSubTypeAccount.DataTextField = "Value";
                ddlSubTypeAccount.DataValueField = "PK";
                ddlSubTypeAccount.DataBind();
            }
            if (SubTypesAccounts != null && SubTypesAccounts.Count != 1)
            {
                ddlSubTypeAccount1.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
            }
            ddlSubTypeAccount1.Visible = true;

            ddlSubTypeAccount.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
            ddlSubTypeAccount.Visible = true;
        }

        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(GridViewRow grw)
        {
            FinTrxService FinTrxServiceClient;
            AccountMstService FinCoaMstServiceClient;
            ADM_CONFIG_MST tempAdmConfigMstObj;
            FinTrxServiceClient = null;
            FinCoaMstServiceClient = null;
            string relquery = string.Empty;

            CommonService commonService;
            commonService = null;

            try
            {
                CurrFtrPK = Convert.ToInt32(grdVoucher.DataKeys[grw.RowIndex].Values[0]);

                finTrxList = (List<FIN_TRX>)(Session["VoucherDet"]);
                finTrxList = (from ftrList in finTrxList
                              where ftrList.FTR_PK == CurrFtrPK
                              select ftrList).ToList();

                ResetForm(2);
                ResetForm(3);

                if (finTrxList.Count > 0)
                {
                    // When adding an item from 1st section the field "finTrxList[0].ADM_CONFIG_MST.CFG_ACTIVE" will be 1
                    //      else the the field "finTrxList[0].ADM_CONFIG_MST.CFG_ACTIVE" will be 2
                    if (finTrxList[0].ADM_CONFIG_MST.CFG_ACTIVE == Convert.ToByte(DbActiveStatus.ACTIVE))
                    {
                        hdfAccount.Value = finTrxList[0].FTR_ACCOUNT.ToString();
                        txtAccount.Text = HttpUtility.HtmlDecode(finTrxList[0].FIN_COA_MST.COA_NAME);
                        ddlMode.SelectedValue = finTrxList[0].FTR_PAYMENT_MODE.ToString();
                        txtNarration.Text = HttpUtility.HtmlDecode(finTrxList[0].FTR_NARRATION);
                        txtAmount.Text = finTrxList[0].FTR_DR_AMT_BC == 0 ? finTrxList[0].FTR_CR_AMT_BC.ToString() : finTrxList[0].FTR_DR_AMT_BC.ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowInstrDetails", "ShowInstrDetails();", true);
                        txtInstrNo.Text = HttpUtility.HtmlDecode(finTrxList[0].FTR_INSTR_NO);
                        txtInstrDate.Text = finTrxList[0].FTR_INSTR_DATE != null ? Convert.ToDateTime(finTrxList[0].FTR_INSTR_DATE).ToString("dd-MMM-yyyy") : string.Empty;
                        txtFavourOf.Text = HttpUtility.HtmlDecode(finTrxList[0].FTR_INSTR_FAVOUR);
                        //chkPDC.Checked = finTrxList[0].FTR_PDC.Equals(DBNull.Value) ? false : finTrxList[0].FTR_PDC == 1 ? true : false;

                        ddlAccountType = hdfAccount.Value == "" ? 0 : Convert.ToInt32(hdfAccount.Value);
                        GetFieldValues(ControlsEnum.FINCOAMST);
                        if (finCoaMstList != null && finCoaMstList.Count > 0)
                        {
                            ddlAccountSubType = Convert.ToInt32(finCoaMstList[0].COA_SUB_TYPE.ToString());
                            //hdfSubTypePk1.Value = finCoaMstList[0].COA_SUB_TYPE.ToString();

                            GetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                            if (finCoaSubTypeCfgList[0].CST_REL_QUERY != null)
                            {
                                if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                                {
                                    relquery = finCoaSubTypeCfgList[0].CST_REL_QUERY == null ? string.Empty : finCoaSubTypeCfgList[0].ADM_QUERIES_CFG1.QRY_QUERY;
                                }

                                if (relquery != string.Empty)
                                {
                                    commonService = new CommonService();
                                    commonService = CommonFunctions.InitiateClient(commonService);
                                    SubTypesAccounts = commonService.ExecuteQuery(relquery);
                                    SetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                                }
                                else
                                {
                                    ddlSubTypeAccount1.Visible = false;
                                    ddlSubTypeAccount.Visible = false;
                                }
                            }
                            else
                            {
                                ddlSubTypeAccount1.Visible = false;
                                ddlSubTypeAccount.Visible = false;
                            }
                        }
                        else
                        {
                            ddlSubTypeAccount1.Visible = false;
                            ddlSubTypeAccount.Visible = false;
                        }

                        if (finTrxList[0].FTR_TYPE_PK != null && Convert.ToInt32(finTrxList[0].FTR_TYPE_PK) > 0 && ddlSubTypeAccount1.Items.Count > 0)
                        {
                            ddlSubTypeAccount1.SelectedValue = finTrxList[0].FTR_TYPE_PK.ToString();
                            ddlSubTypeAccount1.Visible = true;
                        }
                    }
                    else
                    {
                        hdfAccount1.Value = hdfPreviousAccount1Pk.Value = finTrxList[0].FTR_ACCOUNT.ToString();
                        txtAccount1.Text = hdfPreviousAccount1Name.Value = HttpUtility.HtmlDecode(finTrxList[0].FIN_COA_MST.COA_NAME);
                        if (finTrxList[0].FTR_PAYMENT_MODE > 0)
                            ddlMode1.SelectedValue = finTrxList[0].FTR_PAYMENT_MODE.ToString();
                        if (VoucherType == ApplicationType.OBV || VoucherType == ApplicationType.YCV)
                            ddlMode1.Enabled = false;
                        else
                            ddlMode1.Enabled = true;

                        txtNarration1.Text = HttpUtility.HtmlDecode(finTrxList[0].FTR_NARRATION);
                        txtDebit1.Text = finTrxList[0].FTR_DR_AMT_BC == 0 ? string.Empty : finTrxList[0].FTR_DR_AMT_BC.ToString(hdfCurrencyFormat.Value);
                        txtCredit1.Text = finTrxList[0].FTR_CR_AMT_BC == 0 ? string.Empty : finTrxList[0].FTR_CR_AMT_BC.ToString(hdfCurrencyFormat.Value);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowInstrDetails", "ShowInstrDetails1();", true);
                        txtInstrNo1.Text = HttpUtility.HtmlDecode(finTrxList[0].FTR_INSTR_NO);
                        txtInstrDate1.Text = finTrxList[0].FTR_INSTR_DATE != null ? Convert.ToDateTime(finTrxList[0].FTR_INSTR_DATE).ToString("dd-MMM-yyyy") : string.Empty;
                        txtFavourOf1.Text = HttpUtility.HtmlDecode(finTrxList[0].FTR_INSTR_FAVOUR);
                        chkPDC1.Checked = finTrxList[0].FTR_PDC.Equals(DBNull.Value) ? false : finTrxList[0].FTR_PDC >= 1 ? true : false;
                        chkIsBankCharge1.Checked = finTrxList[0].FTR_IS_BANK_CHARGE >= 1 ? true : false;

                        ddlAccountType = hdfAccount1.Value == "" ? 0 : Convert.ToInt32(hdfAccount1.Value);
                        GetFieldValues(ControlsEnum.FINCOAMST);
                        if (finCoaMstList != null && finCoaMstList.Count > 0)
                        {
                            ddlAccountSubType = Convert.ToInt32(finCoaMstList[0].COA_SUB_TYPE.ToString());
                            //hdfSubTypePk1.Value = finCoaMstList[0].COA_SUB_TYPE.ToString();

                            GetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                            if (finCoaSubTypeCfgList[0].CST_REL_QUERY != null)
                            {
                                if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                                {
                                    relquery = finCoaSubTypeCfgList[0].CST_REL_QUERY == null ? string.Empty : finCoaSubTypeCfgList[0].ADM_QUERIES_CFG1.QRY_QUERY;
                                }

                                if (relquery != string.Empty)
                                {
                                    commonService = new CommonService();
                                    commonService = CommonFunctions.InitiateClient(commonService);
                                    relquery = relquery.Replace("@BIZUNITPK@", currentUser.SBUID.ToString());
                                    if (finCoaSubTypeCfgList[0].CST_CODE == "AP" || finCoaSubTypeCfgList[0].CST_CODE == "AR")
                                    {
                                        relquery = relquery.Replace("@COA@", ddlAccountType > 0 ? ddlAccountType.ToString() : "NULL");
                                        relquery = relquery.Replace("@ADV@", "NULL");
                                    }
                                    else if (finCoaSubTypeCfgList[0].CST_CODE == "ADP" || finCoaSubTypeCfgList[0].CST_CODE == "ADR")
                                    {
                                        relquery = relquery.Replace("@COA@", "NULL");
                                        relquery = relquery.Replace("@ADV@", ddlAccountType > 0 ? ddlAccountType.ToString() : "NULL");
                                    }
                                    else
                                    {
                                        relquery = relquery.Replace("@COA@", "NULL");
                                        relquery = relquery.Replace("@ADV@", "NULL");
                                    }
                                    SubTypesAccounts = commonService.ExecuteQuery(relquery);
                                    SetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                                }
                                else
                                {
                                    ddlSubTypeAccount1.Visible = false;
                                    ddlSubTypeAccount.Visible = false;
                                }
                            }
                            else
                            {
                                ddlSubTypeAccount1.Visible = false;
                                ddlSubTypeAccount.Visible = false;
                            }
                        }
                        else
                        {
                            ddlSubTypeAccount1.Visible = false;
                            ddlSubTypeAccount.Visible = false;
                        }
                        if (finTrxList[0].FTR_TYPE_PK != null && Convert.ToInt32(finTrxList[0].FTR_TYPE_PK) > 0 && ddlSubTypeAccount1.Items.Count > 0)
                        {
                            ddlSubTypeAccount1.SelectedValue = finTrxList[0].FTR_TYPE_PK.ToString();
                            ddlSubTypeAccount1.Visible = true;
                        }
                    }
                    #region CostCenter Allocation
                    CostCenterList = new List<CostCenterDetails>();
                    List<FIN_TRX_COC_DTL> lstCocDtl = finTrxList[0].FIN_TRX_COC_DTL.ToList();
                    if (lstCocDtl != null && lstCocDtl.Count > 0)
                    {
                        foreach (FIN_TRX_COC_DTL objCocDtl in lstCocDtl)
                        {
                            CostCenterList.Add(new CostCenterDetails
                            {
                                FCM_CNM_PK = objCocDtl.FTD_CNM_PK,
                                FTD_AMT_TC = objCocDtl.FTD_AMT_TC,
                                FTD_AMT_BC = objCocDtl.FTD_AMT_BC
                            });
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                commonService = null;
            }
        }

        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditViewVatPopup(GridViewRow grw)
        {
            try
            {

                divVatErrorLabel.Visible = false;
                finVatPaymentDetails = TempVATTaxDetails[grw.RowIndex];
                //ResetForm(2);
                //ResetForm(3);
                if (finVatPaymentDetails != null)
                {
                    hdfVATAccountPopup.Value = finVatPaymentDetails.WTH_TAX.ToString();
                    if (finVatPaymentDetails.WTH_BRANCH_TYPE == (byte)VendorContactTypeEnum.HeadOffice)
                        chkHeadOffice.Checked = true;
                    HeadofficeCheckedChanged();
                    SetBranchCodeVisibility();
                    txtVatTaxId.Text = finVatPaymentDetails.WTH_TAX_ID;
                    txtBranchCode.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_BRANCH_TEXT);
                    txtVendorPopup.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_PARTY_NAME);
                    txtCRTNo.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_CERT_NO);
                    if (finVatPaymentDetails.WTH_VENDOR.HasValue)
                    {
                        hdfVendorPopup.Value = finVatPaymentDetails.WTH_VENDOR.ToString();
                    }
                    else
                    {
                        hdfVendorPopup.Value = string.Empty;
                        GetFieldValues(ControlsEnum.VENDORSELECTEDDTL);
                        SetFieldValues(ControlsEnum.VENDORSELECTEDDTL);
                    }

                    if (finVatPaymentDetails.WTH_BRANCH.HasValue)
                    {
                        hdfAddressType.Value = finVatPaymentDetails.WTH_BRANCH.ToString();
                    }
                    else
                    {
                        hdfAddressType.Value = string.Empty;
                    }
                    txtAddressType.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_BRANCH_NAME);
                    txtVatTaxInvDate.Text = Convert.ToDateTime(finVatPaymentDetails.WTH_TAX_DATE).ToString(Resources.Constants.DateFormatShort);
                    if (finVatPaymentDetails.WTH_REFUND_DATE.HasValue)
                    {
                        txtVatRefundDate.Text = Convert.ToDateTime(finVatPaymentDetails.WTH_REFUND_DATE).ToString(Resources.Constants.DateFormatMonthYear);
                    }
                    try
                    {
                        ddlVATAccountPopup.SelectedValue = finVatPaymentDetails.WTH_TAX.ToString();
                        GetUIValuesFromObject(ControlsEnum.TAXTYPECHANGED);
                    }
                    catch { }
                    hdfWHTTaxCategory.Value = finVatPaymentDetails.WTH_TAX_CATEGORY.ToString();
                    hdfWHTTaxName.Value = finVatPaymentDetails.WTH_NAME;
                    //tempVATTax.WTH_DESC = txtDescriptionPopup.Text;
                    //tempVATTax.WTH_FORM_NO = Convert.ToInt32(ddlFormno.SelectedValue);

                    //tempVATTax.WTH_ADDRESS = txtpartyads.Text;
                    txtVatTaxInvNo.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_TAX_INV_NO);
                    txtMaterial.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_ITEM_TEXT);
                    txtBeforeTaxAmount.Text = GetFormattedCurrency(finVatPaymentDetails.WTH_AMOUNT);
                    txtVATTaxAmountPopup.Text = GetFormattedCurrency(finVatPaymentDetails.WTH_TAX_AMT);
                    chkOriginalinvoice.Checked = finVatPaymentDetails.WTH_INV_RECEIVED == 1 ? true : false;

                }
                ViewState["VatDetRowIndex"] = grw.RowIndex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method used to Reset form Controls
        /// </summary>
        private void ResetForm(int Section)
        {
            if (Section == 1)
            {
                txtVoucherNo.Text = Resources.ErpRes.Draft;
                txtVoucherDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                txtRefNo.Text = string.Empty;
                txtRefDate.Text = string.Empty;
                //txtCurrency.Text = string.Empty;
                //txtExchangeRate.Text = string.Empty;
                txtRemarks.Text = string.Empty;
                txtTo.Text = string.Empty;


                CurrPK = 0;
                CurrFtrPK = 0;
                Session["VoucherDet"] = null;

                GetFieldValues(ControlsEnum.APLNTYPE);
                SetFieldValues(ControlsEnum.APLNTYPE);
                hdfSelRowTranPk.Value = "0";
                hdfSelRowVer.Value = "0";

            }

            if (Section == 1 || Section == 2)
            {
                hdfAccount.Value = string.Empty;
                txtAccount.Text = string.Empty;
                //ddlMode.SelectedIndex = 0;
                txtNarration.Text = string.Empty;
                txtAmount.Text = string.Empty;
                ddlSubTypeAccount.Visible = false;
            }

            if (Section == 1 || Section == 3)
            {
                hdfAccount1.Value = string.Empty;
                txtAccount1.Text = string.Empty;
                ddlSubTypeAccount1.Visible = false;
                if (VoucherType == ApplicationType.OBV || VoucherType == ApplicationType.YCV)
                {
                    txtNarration1.Text = Resources.ErpRes.OpeningBalance;
                    if (VoucherType == ApplicationType.YCV)
                        txtNarration1.Text = Resources.ErpRes.ClosingBalance;
                    selectedModeValue = 1;
                    GetFieldValues(ControlsEnum.MODE);
                    if (admConfigMstList != null && admConfigMstList.Count > 0)
                    {
                        admConfigMstObj.CFG_PK = admConfigMstList[0].CFG_PK;
                        admConfigMstObj.CFG_DATA = admConfigMstList[0].CFG_DATA;
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.HASPK);
                        ddlMode1.SelectedIndex = ddlMode1.Items.IndexOf(ddlMode1.Items.FindByValue(admConfigMstList[0].CFG_PK.ToString()));
                        ddlMode1.Enabled = false;
                    }
                    else
                    {
                        ddlMode1.Enabled = true;
                        //ddlMode1.SelectedIndex = 0;
                    }
                }
                else
                {
                    txtNarration1.Text = string.Empty;
                    ddlMode1.Enabled = true;
                    //ddlMode1.SelectedIndex = 0;
                }
                txtDebit1.Text = string.Empty;
                txtCredit1.Text = string.Empty;
            }
            txtInstrNo1.Text = string.Empty;
            txtInstrNo.Text = string.Empty;
            txtInstrDate1.Text = string.Empty;
            txtInstrDate.Text = string.Empty;
            txtFavourOf1.Text = string.Empty;
            txtFavourOf.Text = string.Empty;
            chkPDC1.Checked = false;
            chkIsBankCharge1.Checked = false;
            txtWHTAccountPopup.Text = "Select/Type";
            hdfWHTAccountPopup.Value = "0";
            txtPopupWHTAmount.Text = 0.ToString(hdfCurrencyFormat.Value);
            txtWHTTaxAmountPopup.Text = 0.ToString(hdfCurrencyFormat.Value);
            txtDescriptionPopup.Text = string.Empty;
            //TempWHTTaxDetails = null;
            hdfIscontYes.Value = "0";

            txtWthBranchCode.Text = string.Empty;
            txtWthAddressType.Text = string.Empty;
            //chkWthHeadOffice.Checked = false;
            ddlPayType.SelectedValue = CommonConstants.SELECTVAL;
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPDCforCheque", "$(document).ready(function(){ShowPDCforCheque();});", true);
        }

        private void SetVoucherTypeValues()
        {
            if (finVcrEntryCfgList != null && finVcrEntryCfgList.Count > 0)
            {
                if (VoucherType != ApplicationType.OBV && VoucherType != ApplicationType.DPVJ && VoucherType != ApplicationType.PCVJ && VoucherType != ApplicationType.YCV)
                {
                    Page.Title = finVcrEntryCfgList[0].VEC_DISPLAY_NAME;
                }
                AccountType = finVcrEntryCfgList[0].VEC_MODE;
                hdfVecPk.Value = finVcrEntryCfgList[0].VEC_PK.ToString();

                //////lblVoucherNo.Text = finVcrEntryCfgList[0].VEC_NAME + " " + Resources.Controls.No;
                //////lblVoucherDate.Text = finVcrEntryCfgList[0].VEC_NAME + " " + Resources.Controls.Date;
            }
        }

        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(VoucherType, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }

        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }

        public double StringToFormula(string expression)
        {
            List<string> tokens = getTokens(expression);
            Stack<double> operandStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
            int tokenIndex = 0;
            try
            {
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
            catch
            {
                return 0;
            }
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
        #endregion

        #region ActionHandler
        /// <summary>
        /// Handling control events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            FinTrxService FinTrxServiceClient;
            AccountMstService FinCoaMstServiceClient;
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            ADM_CONFIG_MST tempAdmConfigMstObj;
            FinTrxServiceClient = null;
            FinCoaMstServiceClient = null;
            string relquery = string.Empty;
            long? vatTaxresult;
            string commandArgument = string.Empty;

            //CommonService commonService;
            //commonService = null;
            decimal whtTax = 0;
            decimal vatTax = 0;
            bool IsSuccess = false;
            POPaymentService poPaymentServiceClient;
            poPaymentServiceClient = null;
            bool isContinue;
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int accountPk;
                long? result;
                string action;


                DropDownList ddlWkfAction;
                TextBox WrkfComments;

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
                    commandArgument = ((ImageButton)sender).CommandArgument;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlVATAccountPopup")
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
                    if (((TextBox)sender).ID == "txtVendorPopup")
                    {
                        commonActions = ActionsEnum.VENDORTEXTCHANGED;
                    }
                    if (((TextBox)sender).ID == "txtAddressType")
                    {
                        commonActions = ActionsEnum.VENDORCONTACTTEXTCHANGED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(CheckBox)))
                {
                    commonActions = ActionsEnum.CHECKEDCHANGED;
                }


                switch (commonActions)
                {
                    #region After Bind UcerControl Validation
                    case ActionsEnum.AFTERVALIDATION:
                        GetFieldValues(ControlsEnum.BASECURRENCY);
                        if (grdVoucher.Rows.Count > 0)
                        {
                            BindGrid(ControlsEnum.DEFAULT);
                        }
                        break;
                    #endregion

                    #region Excel Import
                    //Excel Import Section
                    case ActionsEnum.IMPORT:

                        if (grdVoucher.Rows.Count > 0)
                        {
                            BindGrid(ControlsEnum.DEFAULT);
                        }
                        VoucheData obj = new VoucheData();
                        Session["VoucherDet"] = null;
                        List<VoucherTransactionBO> VoucherTrxList = new List<VoucherTransactionBO>();
                        VoucherTransactionBO VoucherTrxObj = new VoucherTransactionBO();
                        obj.VoucherTransactionList = ucrTransactionImport.VoucherDetails;
                        finCoaMstObj = ERP.Utilities.CommonFunctions.Initilize<FIN_COA_MST>();
                        admConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONFIG_MST>();
                        finTrxList = Session["VoucherDet"] == null ? new List<FIN_TRX>() : finTrxList = (List<FIN_TRX>)(Session["VoucherDet"]);
                        foreach (VoucherTransactionBO Item in obj.VoucherTransactionList)
                        {
                            finCoaMstObj = ERP.Utilities.CommonFunctions.Initilize<FIN_COA_MST>();
                            admConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONFIG_MST>();
                            finTrxObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX>();

                            if (CurrFtrPK == 0)
                            {
                                int? maxPk = finTrxList.Max(ftr => (int?)ftr.FTR_PK);
                                finTrxObj.FTR_PK = Convert.ToInt64(maxPk.HasValue ? maxPk + 1 : 1);
                            }
                            else
                            {
                                List<FIN_TRX> ftrList = (from ftr in finTrxList
                                                         where ftr.FTR_PK == CurrFtrPK
                                                         select ftr).ToList();
                                if (ftrList.Count > 0)
                                {
                                    finTrxObj.FTR_PK = CurrFtrPK;
                                    //finTrxObj = ftrList[0];
                                    ////finCoaMstObj = ftrList[0].FIN_COA_MST;
                                    //finTrxList.Remove(finTrxObj);
                                    FIN_TRX finTrxObjOld = ftrList[0];
                                    finTrxList.Remove(finTrxObjOld);
                                }
                            }
                            accountPk = Item.COA_PK;
                            hdfAccount1.Value = accountPk.ToString();

                            // ExcelImport_GetSubTypeAccount();

                            admConfigMstObj.CFG_PK = Item.CFG_PK;  //Convert.ToInt32(ddlMode1.SelectedValue);
                            admConfigMstObj.CFG_DATA = Item.PAYMENT_MODE_TEXT;
                            // CFG_ACTIVE is set as 2 for identifying the item added from 2nd section
                            admConfigMstObj.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.HASPK);



                            finTrxObj.FTR_PAYMENT_MODE = Item.CFG_PK;
                            finTrxObj.FTR_NARRATION = HttpUtility.HtmlEncode(Item.NARRATION.Trim());
                            finTrxObj.FTR_DR_AMT_BC = Item.DR_AMT_BC == null ? 0 : Convert.ToDecimal(Item.DR_AMT_BC);
                            finTrxObj.FTR_DR_AMT_TC = finTrxObj.FTR_DR_AMT_BC * Convert.ToDecimal(txtExchangeRate.Text);
                            finTrxObj.FTR_CR_AMT_BC = Item.CR_AMT_BC == null ? 0 : Convert.ToDecimal(Item.CR_AMT_BC);
                            finTrxObj.FTR_CR_AMT_TC = finTrxObj.FTR_CR_AMT_BC * Convert.ToDecimal(txtExchangeRate.Text);

                            if (!string.IsNullOrEmpty(Item.INSTR_NO.Trim()))
                                finTrxObj.FTR_INSTR_NO = HttpUtility.HtmlEncode(Item.INSTR_NO.Trim());
                            if (!string.IsNullOrEmpty(Item.INSTR_DATE))
                                finTrxObj.FTR_INSTR_DATE = Convert.ToDateTime(Item.INSTR_DATE.Trim());
                            if (!string.IsNullOrEmpty(Item.INSTR_FAVOUR))
                                finTrxObj.FTR_INSTR_FAVOUR = HttpUtility.HtmlEncode(Item.INSTR_FAVOUR.Trim());
                            finTrxObj.FTR_PDC = Convert.ToByte(Item.PDC);
                            finTrxObj.FTR_IS_BANK_CHARGE = chkIsBankCharge1.Checked ? (byte)1 : (byte)0;

                            finTrxObj.FTR_INV_DATE = Item.DATE_INVOICE == null ? string.Empty : HttpUtility.HtmlEncode(Item.DATE_INVOICE.Trim());
                            finTrxObj.FTR_INV_NO = Item.INVOICE_NO == null ? string.Empty : HttpUtility.HtmlEncode(Item.INVOICE_NO.Trim());
                            finTrxObj.FTR_REF_PO_NUMBER = Item.REFER_PUR == null ? string.Empty : HttpUtility.HtmlEncode(Item.REFER_PUR.Trim());
                            finTrxObj.FTR_VENDOR_CODE = Item.VENDOR_CODE == null ? string.Empty : HttpUtility.HtmlEncode(Item.VENDOR_CODE.Trim());
                            finTrxObj.FTR_AMT_BFR_VAT = Item.FTR_AMT_BFR_VAT == null ? 0 : Convert.ToDecimal(Item.FTR_AMT_BFR_VAT);

                            tempAdmConfigMstObj = admConfigMstObj;

                            finCoaMstList = new List<FIN_COA_MST>();
                            FinCoaMstServiceClient = new AccountMstService();
                            FinCoaMstServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(FinCoaMstServiceClient);

                            serviceUtilityObj = new ServiceUtility();
                            serviceUtilityObj.CurrentPage = -1;
                            serviceUtilityObj.PageSize = -1;

                            finCoaMstObj.COA_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            finCoaMstObj.COA_PK = Item.COA_PK;
                            //finCoaMstObj.COA_CODE = string.Empty;
                            //finCoaMstObj.COA_NAME = string.Empty;
                            finCoaMstList = FinCoaMstServiceClient.GetFinCoaMst(finCoaMstObj, serviceUtilityObj);

                            finTrxObj.FTR_ACCOUNT = Item.COA_PK;

                            if (CurrPK == 0)
                            {
                                finTrxObj.FIN_COA_MST = finCoaMstList[0];
                            }
                            else if (CurrPK != 0 && CurrFtrPK == 0)
                            {
                                finTrxObj.FIN_COA_MST = finCoaMstList[0];
                            }
                            if (finCoaMstList.Count > 0)
                            {
                                //commented for while changing an account then all other same accounts are changed
                                //if (finTrxObj.FIN_COA_MST == null)
                                //{                                    
                                finTrxObj.FIN_COA_MST = finCoaMstList[0];
                                //}
                                //finTrxObj.FIN_COA_MST.COA_CODE = finCoaMstList[0].COA_CODE;
                                //finTrxObj.FIN_COA_MST.COA_NAME = finCoaMstList[0].COA_NAME;
                                //finTrxObj.FIN_COA_MST.COA_PK = finCoaMstList[0].COA_PK;
                                finTrxObj.FTR_ACC_SUB_TYPE = finCoaMstList[0].COA_SUB_TYPE;
                            }

                            hdfSubTypePk.Value = finTrxObj.FTR_ACC_SUB_TYPE.ToString();
                            ddlAccountSubType = finTrxObj.FTR_ACC_SUB_TYPE;

                            if (Item.PDC == "1")
                            {
                                if (ddlAccountSubType != (int)AccSubType.PPC)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPDCforCheque", "$(document).ready(function(){ShowPDCforCheque();});", true);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_PDC_Account").ToString()) + "');", true);
                                    return;
                                }
                            }

                            ResetForm(3);
                            this.txtAccount1.Focus();

                            //old code
                            // GetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                            //if (finCoaSubTypeCfgList[0].CST_REL_QUERY != null)
                            //{
                            //    if (ddlSubTypeAccount1.SelectedValue != "-1")
                            //        finTrxObj.FTR_TYPE_PK = Convert.ToInt32(ddlSubTypeAccount1.SelectedValue);
                            //    finTrxObj.FTR_TYPE = finCoaSubTypeCfgList[0].CST_CODE;
                            //}


                            if (Item.SUB_TYPE_PK > 0)
                            {
                                finTrxObj.FTR_TYPE_PK = Convert.ToInt32(Item.SUB_TYPE_PK);
                                finTrxObj.FTR_TYPE = Item.ACC_SUB_TYPE;
                            }
                            else
                            {
                                finTrxObj.FTR_TYPE = null;
                                finTrxObj.FTR_TYPE_PK = null;
                            }

                            finTrxObj.ADM_CONFIG_MST = admConfigMstObj;
                            finTrxObj.FTR_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            finTrxObj.FTR_DEPT = Session[BusinessObject.Common.SessionStrings.CurDept] == null ? currentUser.CurrentDeptPK : Convert.ToInt16(Session[BusinessObject.Common.SessionStrings.CurDept].ToString());// Convert.ToInt16(Session[BusinessObject.Common.SessionStrings.CurDept].ToString());
                            finTrxObj.FTR_BIZUNIT = currentUser.SBUID;

                            #region CostCenter Allocation
                            //if (Item.CostCenterMapList != null && Item.CostCenterMapList.Count > 0)
                            //{
                            //    foreach (CostCenterMapBO objDet in Item.CostCenterMapList)
                            //    {
                            //        finTrxObj.FIN_TRX_COC_DTL.Add(new FIN_TRX_COC_DTL
                            //        {
                            //            FTD_CNM_PK = objDet.CNM_PK,
                            //            FTD_AMT_TC = objDet.AMT_TC,
                            //            FTD_AMT_BC = objDet.AMT_BC
                            //        });
                            //    }
                            //}


                            if (Item.CostCenterMapList != null && Item.CostCenterMapList.Count > 0)
                            {
                                selAccountPk = Convert.ToInt32(finTrxObj.FTR_ACCOUNT);
                                GetFieldValues(ControlsEnum.COSTCENTER);
                                if (dtCostCenter != null && dtCostCenter.Rows.Count > 0)
                                {
                                    foreach (DataRow drRow in dtCostCenter.Rows)
                                    {
                                        CostCenterMapBO CostCenterMapObj =
                                        Item.CostCenterMapList.SingleOrDefault(ac => ac.CNM_PK == Convert.ToInt32(drRow["FCM_CNM_PK"]));
                                        finTrxObj.FIN_TRX_COC_DTL.Add(new FIN_TRX_COC_DTL
                                        {
                                            FTD_CNM_PK = Convert.ToInt32(drRow["FCM_CNM_PK"]),
                                            FTD_AMT_TC = 0.00M,
                                            FTD_AMT_BC = CostCenterMapObj == null ? Convert.ToDecimal(drRow["FCM_AMOUNT"]) : CostCenterMapObj.AMT_BC
                                        });
                                    }
                                }
                            }




                            #endregion
                            finTrxList.Add(finTrxObj);


                            Session["VoucherDet"] = finTrxList;
                            CurrFtrPK = 0;
                            hdfPreviousAccount1Name.Value = string.Empty;
                            hdfPreviousAccount1Pk.Value = "0";
                            //CostCenterList.Clear();
                            hdfIsContCCAllocDeletion.Value = "0";

                            //ShowHideCostCenterAllocButton();
                        }

                        BindGrid();
                        GetFieldValues(ControlsEnum.BASECURRENCY);
                        break;
                    #endregion
                    #region SAVE
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        hdfAmntMissmatch.Value = string.Empty;
                        showPopup = false;
                        if (hdfShowCashAccountDiv.Value == "1" && VoucherType == ApplicationType.PCVJ || VoucherType == ApplicationType.PCRVJ)
                            AddCashAccountCR();
                        if (grdVoucher.Rows.Count < 1)
                        {
                            litErrorMsg.Text = this.GetLocalResourceObject("Err_AccountNos").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            return;
                        }
                        if (!IsValidSession())
                        {
                            litErrorMsg.Text = Resources.Report.Msg_MultipleTab_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            return;
                        }
                        if (hdfDebitTotal.Value != hdfCreditTotal.Value)
                        {
                            litErrorMsg.Text = this.GetLocalResourceObject("Err_Total").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            return;
                        }


                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            if (grdVoucher.Rows.Count > 0)
                            {
                                //finTrxList = new List<FIN_TRX>();
                                //finTrxHdrList = new List<FIN_TRX_HDR>();
                                //FinTrxServiceClient = new FinTrxService();
                                //FinTrxServiceClient = CommonFunctions.InitiateClient(FinTrxServiceClient);
                                //finTrxHdrObj = CommonFunctions.Initilize<FIN_TRX_HDR>();

                                //if (hdfVoucherNo.Value == string.Empty && AST_DOC_MODE.Value == "1")
                                //{
                                //    GetFieldValues(ControlsEnum.VOUCHERNO);
                                //    txtVoucherNo.Text = hdfVoucherNo.Value;
                                //}
                                //FinTrxHeaderBO finTrxHeaderObj;

                                // finTrxHdrObj = (FIN_TRX_HDR)SetUIValuesToObject(ActionsEnum.SAVE, ControlsEnum.FINTRXHDR);


                                finTrxHeaderObj = (FinTrxHeaderBO)SetUIValuesToObject(ActionsEnum.SAVE, ControlsEnum.FINTRXHDR);
                                if (VoucherType == ApplicationType.PCRVJ || VoucherType == ApplicationType.DPVJ || VoucherType == ApplicationType.PCVJ)
                                {
                                    //if (!string.IsNullOrEmpty(finTrxHdrObj.FTH_PARTY_NAME) && !string.IsNullOrEmpty(finTrxHdrObj.FTH_REF_NO))
                                    //{
                                    //   if (FinTrxServiceClient.IsRefereceNoEsixt(finTrxHdrObj))
                                    //    {
                                    //        litErrorMsg.Text = GetLocalResourceObject("Msg_Refno_Exist").ToString();
                                    //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    //        return;
                                    //    }
                                    //}
                                    TextBox txtWHTAmount = (TextBox)grdVoucher.FooterRow.FindControl("txtWHTAmount");
                                    decimal whtAmount = string.IsNullOrEmpty(hdfWithHoldTax.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(hdfWithHoldTax.Value);
                                    if (whtAmount > 0 && txtWHTAmount != null && !string.IsNullOrEmpty(txtWHTAmount.Text))
                                    {
                                        tempWHTTaxDetails = null;
                                        decimal.TryParse(hdfWithHoldTax.Value, out whtTax);
                                        if (TempWHTTaxDetails != null && TempWHTTaxDetails.Count > 0)
                                        {
                                            tempWHTTaxDetails = WHTTaxDetails = TempWHTTaxDetails;
                                            withHoldTax = Convert.ToDecimal(CommonFunctions.DoubleFormatRound(Convert.ToDouble(tempWHTTaxDetails.Sum(aa => aa.WTH_TAX_AMT)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));

                                            if (whtTax != withHoldTax && whtTax > 0 && (hdfIscontYes.Value != "1"))
                                            {
                                                hdfAmntMissmatch.Value = GetLocalResourceObject("WHTNotTallied").ToString();
                                                showPopup = true;
                                            }
                                        }
                                        else if (whtTax > 0)
                                        {
                                            hdfAmntMissmatch.Value = GetLocalResourceObject("Err_WhtAmntNotEntered").ToString();
                                            showPopup = true;
                                        }
                                    }

                                    TextBox txtVatBuy = (TextBox)grdVoucher.FooterRow.FindControl("txtVatBuy");
                                    decimal vatAmount = string.IsNullOrEmpty(hdfVatBuy.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(hdfVatBuy.Value);
                                    if (vatAmount > 0 && txtVatBuy != null && !string.IsNullOrEmpty(txtVatBuy.Text))
                                    {
                                        tempVATTaxDetails = null;
                                        decimal.TryParse(hdfVatBuy.Value, out vatTax);
                                        if (TempVATTaxDetails != null && TempVATTaxDetails.Count > 0)
                                        {
                                            tempVATTaxDetails = VATTaxDetails = TempVATTaxDetails;
                                            VatBuyTax = Convert.ToDecimal(CommonFunctions.DoubleFormatRound(Convert.ToDouble(tempVATTaxDetails.Sum(aa => aa.WTH_TAX_AMT)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                                            if (vatTax != VatBuyTax && vatTax > 0 && (hdfIscontYes.Value != "1"))
                                            {
                                                hdfAmntMissmatch.Value += GetLocalResourceObject("VATNotTallied").ToString();
                                                showPopup = true;
                                            }
                                        }
                                        else if (vatTax > 0)
                                        {
                                            hdfAmntMissmatch.Value += GetLocalResourceObject("Err_VatAmntNotEntered").ToString();
                                            showPopup = true;
                                        }
                                    }
                                    hdfAmntMissmatch.Value += GetLocalResourceObject("DoYouWantToContinue").ToString();
                                    if ((hdfIscontYes.Value != "1") && showPopup)
                                    {
                                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_WHTMissmatch").ToString()) + "');", true);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlreadyPaid", "$(document).ready(function(){WhtAmtMismatch();});", true);
                                        return;
                                    }
                                }

                                if (finTrxHeaderObj != null)
                                {
                                    #region Checking for voucher locked with financial year
                                    //if (finTrxHeaderObj.FTH_DATE.HasValue)
                                    //{
                                    string LockUptoDate = string.Empty;
                                    if (BusinessLogic.Finance.VoucherLockingBL.IsVoucherLocked(finTrxHeaderObj.FTH_DATE, CurrPK, finTrxHeaderObj.FTH_BIZUNIT, ref LockUptoDate))
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Err_Voucher_Locked").ToString() + (!string.IsNullOrEmpty(LockUptoDate) ? DateTime.Parse(LockUptoDate).ToString(Resources.Constants.DateFormatShort) : string.Empty);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        return;
                                    }
                                    //}
                                    #endregion

                                    //finTrxHdrList.Add(finTrxHdrObj);
                                    int retRefID = 0;
                                    string trxNo = string.Empty;
                                    //result = FinTrxServiceClient.SaveDirectFinTrx(finTrxHdrList);
                                    result = 0;

                                    finTrxHeaderObj.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                    string xmlDoc = CommonFunctions.XmlSerialize<FinTrxHeaderBO>(finTrxHeaderObj);//CommonFunctions.ObjectTOXml(FinTrxHeaderBO);
                                    // save Process Control inspection details
                                    result = BusinessLogic.Jouralize.JournalizeBL.SaveVoucher(xmlDoc, out retRefID, out trxNo);

                                    if (result > 0)
                                    {
                                        litErrorMsg.Text = Resources.Messages.Msg_Save_Success;

                                        if (VoucherType == ApplicationType.OBV)
                                        {
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.OpeningBalance);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                                                Page.ResolveClientUrl(Resources.PageURL.OpeningBalanceList) + "');", true);
                                        }
                                        else if (VoucherType == ApplicationType.DPVJ)
                                        {
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.DirectPayment);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                                                Page.ResolveClientUrl(Resources.PageURL.DirectPaymentList) + "');", true);
                                        }
                                        else if (VoucherType == ApplicationType.PCVJ)
                                        {
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.PettyCash);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                                                Page.ResolveClientUrl(Resources.PageURL.PettyCashList) + "');", true);
                                        }


                                        else if (VoucherType == ApplicationType.PCRVJ)
                                        {
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.PettyCashReceipt);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                                                Page.ResolveClientUrl(Resources.PageURL.PettyCashReceiptList) + "');", true);
                                        }



                                        else if (VoucherType == ApplicationType.YCV)
                                        {
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.YearClosing);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                                                Page.ResolveClientUrl(Resources.PageURL.YearClosingVoucherList) + "');", true);
                                        }
                                        //Contra Entry
                                        else if (VoucherType == ApplicationType.CTVJ)
                                        {
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.ContrVoucher);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                                                Page.ResolveClientUrl(Resources.PageURL.ContraEntryList) + "');", true);
                                        }
                                        else if (VoucherType == ApplicationType.JV)
                                        {
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.JournalVoucher);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                                                Page.ResolveClientUrl(Resources.PageURL.JVPath) + "');", true);
                                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                                            //    Page.ResolveClientUrl(Resources.PageURL.JournalVoucherList) + "');", true);
                                        }
                                        GetFieldValues(ControlsEnum.FINTRX);
                                        SetFieldValues(ControlsEnum.FINTRX);
                                        ResetForm(1);
                                    }
                                    else if (result == -111)//If the voucher Date year is selected is not same as the current financial year
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_Financial_Year").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.FINYEARNOTEXIST)//Financial year not entered
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_Financial_Year_Notentered").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -6)//If Referece No Esixt
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Refno_Exist").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        return;
                                    }
                                    else if (result == -800)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Total").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Voucher").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                }

                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Voucher").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        if (VoucherType == ApplicationType.OBV)
                        {
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.OpeningBalanceList), false);
                        }
                        else if (VoucherType == ApplicationType.DPVJ)
                        {
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.DirectPaymentList), false);
                        }
                        else if (VoucherType == ApplicationType.PCVJ)
                        {
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.PettyCashList), false);
                        }

                        else if (VoucherType == ApplicationType.PCRVJ)
                        {
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.PettyCashReceiptList), false);
                        }

                        else if (VoucherType == ApplicationType.YCV)
                        {
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.YearClosingVoucherList), false);
                        }
                        else if (VoucherType == ApplicationType.JV)
                        {
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.JVPath), false);
                        }
                        //Contra Entry
                        else if (VoucherType == ApplicationType.CTVJ)
                        {
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ContraEntryList), false);
                        }
                        ResetForm(1);
                        GetFieldValues(ControlsEnum.MODE);
                        SetFieldValues(ControlsEnum.MODE);
                        this.txtRefNo.Focus();
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (grdVoucher.Rows.Count > 0)
                        {

                            poPaymentServiceClient = new POPaymentService();
                            poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                            result = poPaymentServiceClient.DeleteDirectPaymentHdr(CurrPK);
                            if (result > 0)
                            {
                                litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Voucher);
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                if (VoucherType == ApplicationType.DPVJ)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                                        Page.ResolveClientUrl(Resources.PageURL.DirectPaymentList) + "');", true);
                                }
                                else if (VoucherType == ApplicationType.OBV)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                                            Page.ResolveClientUrl(Resources.PageURL.OpeningBalanceList) + "');", true);
                                }
                                else if (VoucherType == ApplicationType.PCVJ)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                                            Page.ResolveClientUrl(Resources.PageURL.PettyCashList) + "');", true);
                                }
                                else if (VoucherType == ApplicationType.YCV)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                                            Page.ResolveClientUrl(Resources.PageURL.YearClosingVoucherList) + "');", true);
                                }
                                else if (VoucherType == ApplicationType.JV)
                                {
                                    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.JVPath), false);
                                }
                                GetFieldValues(ControlsEnum.FINTRX);
                                SetFieldValues(ControlsEnum.FINTRX);
                                Session["VoucherDet"] = null;
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Voucher").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }

                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Voucher").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideOverlay", "HideOverlay();", true);
                        break;
                    #endregion
                    #region ExchangeRate
                    case ActionsEnum.ACTIVATE:
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        break;
                    #endregion
                    #region Edit Item From Grid
                    case ActionsEnum.GRIDEDIT:
                        GridViewRow grw = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        hdfItemEdit.Value = "1";
                        SetUIEditView(grw);
                        CostCenterList = new List<CostCenterDetails>();
                        ShowHideCostCenterAllocButton();
                        break;
                    #endregion
                    #region Delete Item From Grid
                    case ActionsEnum.GRIDDELETE:
                        GridViewRow grow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;

                        finTrxList = (List<FIN_TRX>)(Session["VoucherDet"]);
                        CurrFtrPK = Convert.ToInt32(grdVoucher.DataKeys[grow.RowIndex].Values[0]);

                        List<FIN_TRX> ftrxList = (from ftr in finTrxList
                                                  where ftr.FTR_PK == CurrFtrPK
                                                  select ftr).ToList();
                        ResetForm(3);
                        if (ftrxList.Count > 0) finTrxList.Remove(ftrxList[0]);

                        Session["VoucherDet"] = finTrxList;
                        CurrFtrPK = 0;

                        BindGrid();
                        ShowHideCostCenterAllocButton();

                        break;
                    #endregion
                    #region Add Item To Grid
                    case ActionsEnum.ADDLITEM:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else if (!IsValidCostCenterSplitAllocation(CostCenterList))
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_CC_SplitMissmatch").ToString()) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            //if (finCoaSubTypeCfgList[0].CST_REL_QUERY != null)
                            //{
                            //    if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                            //    {
                            //        relquery = finCoaSubTypeCfgList[0].CST_REL_QUERY == null ? string.Empty : finCoaSubTypeCfgList[0].ADM_QUERIES_CFG1.QRY_QUERY;
                            //    }

                            //    if (relquery != string.Empty)
                            //    {
                            //        commonService = new CommonService();
                            //        commonService = CommonFunctions.InitiateClient(commonService);
                            //        SubTypesAccounts = commonService.ExecuteQuery(relquery);
                            //        SetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                            //    }

                            //}
                            hdfItemEdit.Value = "0";
                            if (ddlSubTypeAccount1.Visible)
                            {
                                if (ddlSubTypeAccount1.SelectedValue == "-1")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPDCforCheque", "$(document).ready(function(){ShowPDCforCheque();});", true);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_SubType").ToString()) + "');", true);
                                    return;
                                }
                            }
                            finTrxList = Session["VoucherDet"] == null ? new List<FIN_TRX>() : finTrxList = (List<FIN_TRX>)(Session["VoucherDet"]);

                            finCoaMstObj = ERP.Utilities.CommonFunctions.Initilize<FIN_COA_MST>();
                            admConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONFIG_MST>();
                            finTrxObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX>();
                            finTrxObj.FTR_RECORD_IDENTITY = RecordIdentity;

                            if (CurrFtrPK == 0)
                            {
                                int? maxPk = finTrxList.Max(ftr => (int?)ftr.FTR_PK);
                                finTrxObj.FTR_PK = Convert.ToInt64(maxPk.HasValue ? maxPk + 1 : 1);
                            }
                            else
                            {
                                List<FIN_TRX> ftrList = (from ftr in finTrxList
                                                         where ftr.FTR_PK == CurrFtrPK
                                                         select ftr).ToList();

                                if (ftrList.Count > 0)
                                {
                                    finTrxObj.FTR_PK = CurrFtrPK;
                                    //finTrxObj = ftrList[0];
                                    ////finCoaMstObj = ftrList[0].FIN_COA_MST;
                                    //finTrxList.Remove(finTrxObj);
                                    FIN_TRX finTrxObjOld = ftrList[0];
                                    finTrxList.Remove(finTrxObjOld);

                                }
                            }

                            if (((ImageButton)sender).ID == "imbAddItem")
                            {
                                #region Adding items from first section
                                accountPk = Convert.ToInt32(hdfAccount.Value);

                                admConfigMstObj.CFG_PK = Convert.ToInt32(ddlMode.SelectedValue);
                                admConfigMstObj.CFG_DATA = ddlMode.SelectedItem.Text;
                                // CFG_ACTIVE is set as 1 for identifying the item added from 1st section
                                admConfigMstObj.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);


                                finTrxObj.FTR_PAYMENT_MODE = Convert.ToInt32(ddlMode.SelectedValue);
                                finTrxObj.FTR_NARRATION = HttpUtility.HtmlEncode(txtNarration.Text.Trim());
                                if (AccountType == 2)
                                {
                                    finTrxObj.FTR_DR_AMT_BC = Convert.ToDecimal(txtAmount.Text);
                                    finTrxObj.FTR_DR_AMT_TC = finTrxObj.FTR_DR_AMT_BC * Convert.ToDecimal(txtExchangeRate.Text);
                                    finTrxObj.FTR_CR_AMT_BC = 0;
                                    finTrxObj.FTR_CR_AMT_TC = 0;
                                }
                                else
                                {
                                    finTrxObj.FTR_DR_AMT_BC = 0;
                                    finTrxObj.FTR_DR_AMT_TC = 0;
                                    finTrxObj.FTR_CR_AMT_BC = Convert.ToDecimal(txtAmount.Text);
                                    finTrxObj.FTR_CR_AMT_TC = finTrxObj.FTR_CR_AMT_BC * Convert.ToDecimal(txtExchangeRate.Text);
                                }
                                if (!string.IsNullOrEmpty(txtInstrNo.Text))
                                    finTrxObj.FTR_INSTR_NO = HttpUtility.HtmlEncode(txtInstrNo.Text.Trim());
                                if (!string.IsNullOrEmpty(txtInstrDate.Text))
                                    finTrxObj.FTR_INSTR_DATE = Convert.ToDateTime(txtInstrDate.Text.Trim());
                                if (!string.IsNullOrEmpty(txtFavourOf.Text))
                                    finTrxObj.FTR_INSTR_FAVOUR = HttpUtility.HtmlEncode(txtFavourOf.Text.Trim());

                                //ResetForm(2);
                                //this.txtAccount.Focus();
                                #endregion
                            }
                            else
                            {
                                #region Adding items from second section
                                decimal debitAmnt = 0;
                                decimal creditAmnt = 0;
                                decimal.TryParse(txtDebit1.Text, out debitAmnt);
                                decimal.TryParse(txtCredit1.Text, out creditAmnt);
                                if (debitAmnt <= 0 && creditAmnt <= 0)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Amnt_greater_zero").ToString()) + "');", true);
                                    return;
                                }
                                accountPk = Convert.ToInt32(hdfAccount1.Value);

                                admConfigMstObj.CFG_PK = Convert.ToInt32(ddlMode1.SelectedValue);

                                // CFG_ACTIVE is set as 2 for identifying the item added from 2nd section
                                admConfigMstObj.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.HASPK);
                                if (Convert.ToInt32(ddlMode1.SelectedValue) > 0)
                                {
                                    finTrxObj.FTR_PAYMENT_MODE = Convert.ToInt32(ddlMode1.SelectedValue);
                                    admConfigMstObj.CFG_DATA = ddlMode1.SelectedItem.Text;
                                }
                                finTrxObj.FTR_NARRATION = HttpUtility.HtmlEncode(txtNarration1.Text.Trim());
                                finTrxObj.FTR_DR_AMT_BC = txtDebit1.Text == string.Empty ? 0 : Convert.ToDecimal(txtDebit1.Text);
                                finTrxObj.FTR_DR_AMT_TC = finTrxObj.FTR_DR_AMT_BC * Convert.ToDecimal(txtExchangeRate.Text);
                                finTrxObj.FTR_CR_AMT_BC = txtCredit1.Text == string.Empty ? 0 : Convert.ToDecimal(txtCredit1.Text);
                                finTrxObj.FTR_CR_AMT_TC = finTrxObj.FTR_CR_AMT_BC * Convert.ToDecimal(txtExchangeRate.Text);

                                if (!string.IsNullOrEmpty(txtInstrNo1.Text))
                                    finTrxObj.FTR_INSTR_NO = HttpUtility.HtmlEncode(txtInstrNo1.Text.Trim());
                                if (!string.IsNullOrEmpty(txtInstrDate1.Text))
                                    finTrxObj.FTR_INSTR_DATE = Convert.ToDateTime(txtInstrDate1.Text.Trim());
                                if (!string.IsNullOrEmpty(txtFavourOf1.Text))
                                    finTrxObj.FTR_INSTR_FAVOUR = HttpUtility.HtmlEncode(txtFavourOf1.Text.Trim());
                                finTrxObj.FTR_PDC = chkPDC1.Checked == true ? (byte)1 : (byte)0;
                                finTrxObj.FTR_IS_BANK_CHARGE = chkIsBankCharge1.Checked ? (byte)1 : (byte)0;
                                tempAdmConfigMstObj = admConfigMstObj;
                                //ResetForm(3);
                                //  admConfigMstObj = tempAdmConfigMstObj;
                                //this.txtAccount1.Focus();

                                #endregion
                            }

                            finCoaMstList = new List<FIN_COA_MST>();
                            FinCoaMstServiceClient = new AccountMstService();
                            FinCoaMstServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(FinCoaMstServiceClient);

                            serviceUtilityObj = new ServiceUtility();
                            serviceUtilityObj.CurrentPage = -1;
                            serviceUtilityObj.PageSize = -1;

                            finCoaMstObj.COA_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            finCoaMstObj.COA_PK = accountPk;
                            //finCoaMstObj.COA_CODE = string.Empty;
                            //finCoaMstObj.COA_NAME = string.Empty;
                            finCoaMstList = FinCoaMstServiceClient.GetFinCoaMst(finCoaMstObj, serviceUtilityObj);

                            finTrxObj.FTR_ACCOUNT = accountPk;
                            if (CurrPK == 0)
                            {

                                finTrxObj.FIN_COA_MST = finCoaMstList[0];

                            }
                            else if (CurrPK != 0 && CurrFtrPK == 0)
                            {
                                finTrxObj.FIN_COA_MST = finCoaMstList[0];
                            }
                            if (finCoaMstList.Count > 0)
                            {
                                //commented for while changing an account then all other same accounts are changed
                                //if (finTrxObj.FIN_COA_MST == null)
                                //{                                    
                                finTrxObj.FIN_COA_MST = finCoaMstList[0];
                                //}
                                //finTrxObj.FIN_COA_MST.COA_CODE = finCoaMstList[0].COA_CODE;
                                //finTrxObj.FIN_COA_MST.COA_NAME = finCoaMstList[0].COA_NAME;
                                //finTrxObj.FIN_COA_MST.COA_PK = finCoaMstList[0].COA_PK;
                                finTrxObj.FTR_ACC_SUB_TYPE = finCoaMstList[0].COA_SUB_TYPE;
                            }

                            hdfSubTypePk.Value = finTrxObj.FTR_ACC_SUB_TYPE.ToString();
                            ddlAccountSubType = finTrxObj.FTR_ACC_SUB_TYPE;

                            if (chkPDC1.Checked)
                            {
                                if (ddlAccountSubType != (int)AccSubType.PPC)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPDCforCheque", "$(document).ready(function(){ShowPDCforCheque();});", true);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_PDC_Account").ToString()) + "');", true);
                                    return;
                                }
                            }

                            if (((ImageButton)sender).ID == "imbAddItem")
                            {
                                ResetForm(2);
                                this.txtAccount.Focus();
                            }
                            else
                            {
                                ResetForm(3);
                                this.txtAccount1.Focus();
                            }

                            GetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                            if (finCoaSubTypeCfgList[0].CST_REL_QUERY != null)
                            {
                                if (ddlSubTypeAccount1.SelectedValue != "-1")
                                    finTrxObj.FTR_TYPE_PK = Convert.ToInt32(ddlSubTypeAccount1.SelectedValue);
                                finTrxObj.FTR_TYPE = finCoaSubTypeCfgList[0].CST_CODE;
                            }
                            else
                            {
                                finTrxObj.FTR_TYPE = null;
                                finTrxObj.FTR_TYPE_PK = null;
                            }

                            finTrxObj.ADM_CONFIG_MST = admConfigMstObj;
                            finTrxObj.FTR_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            finTrxObj.FTR_DEPT = Session[BusinessObject.Common.SessionStrings.CurDept] == null ? currentUser.CurrentDeptPK : Convert.ToInt16(Session[BusinessObject.Common.SessionStrings.CurDept].ToString());// Convert.ToInt16(Session[BusinessObject.Common.SessionStrings.CurDept].ToString());
                            finTrxObj.FTR_BIZUNIT = currentUser.SBUID;

                            #region CostCenter Allocation
                            if (CostCenterList != null && CostCenterList.Count > 0)
                            {
                                foreach (CostCenterDetails objDet in CostCenterList)
                                {
                                    finTrxObj.FIN_TRX_COC_DTL.Add(new FIN_TRX_COC_DTL
                                    {
                                        FTD_CNM_PK = objDet.FCM_CNM_PK,
                                        FTD_AMT_TC = objDet.FTD_AMT_TC,
                                        FTD_AMT_BC = objDet.FTD_AMT_BC
                                    });
                                }
                            }
                            #endregion

                            finTrxList.Add(finTrxObj);
                            if (hdfShowCashAccountDiv.Value == "1" && VoucherType == ApplicationType.PCVJ && commandArgument != "SaveAction")
                            {
                                if (finTrxList.Count(s => s.FTR_ACCOUNT == Convert.ToInt32(ddlCashAccount.SelectedValue)) == 1 && finTrxList.Count() > 1)
                                {
                                    finTrxObj.FTR_CR_AMT_TC = finTrxObj.FTR_CR_AMT_BC * Convert.ToDecimal(txtExchangeRate.Text);
                                    decimal exchangeRate = string.IsNullOrEmpty(txtExchangeRate.Text) ? 0 : Convert.ToDecimal(txtExchangeRate.Text);
                                    decimal drSum = finTrxList.Sum(s => s.FTR_DR_AMT_BC) - finTrxList.Where(i => i.FTR_ACCOUNT != Convert.ToInt32(ddlCashAccount.SelectedValue)).Sum(s => s.FTR_CR_AMT_BC);
                                    //list.Where(w => w.Name == "height").ToList().ForEach(s => s.Value = 30);
                                    finTrxList.Where(w => w.FTR_ACCOUNT == Convert.ToInt32(ddlCashAccount.SelectedValue)).ToList().ForEach(s => s.FTR_CR_AMT_BC = drSum);
                                    finTrxList.Where(w => w.FTR_ACCOUNT == Convert.ToInt32(ddlCashAccount.SelectedValue)).ToList().ForEach(s => s.FTR_CR_AMT_TC = s.FTR_CR_AMT_BC * exchangeRate);
                                }
                            }
                            else if (hdfShowCashAccountDiv.Value == "1" && VoucherType == ApplicationType.PCRVJ && commandArgument != "SaveAction")
                            {
                                if (finTrxList.Count(s => s.FTR_ACCOUNT == Convert.ToInt32(ddlCashAccount.SelectedValue)) == 1 && finTrxList.Count() > 1)
                                {
                                    finTrxObj.FTR_DR_AMT_TC = finTrxObj.FTR_DR_AMT_BC * Convert.ToDecimal(txtExchangeRate.Text);
                                    decimal exchangeRate = string.IsNullOrEmpty(txtExchangeRate.Text) ? 0 : Convert.ToDecimal(txtExchangeRate.Text);
                                    decimal drSum = finTrxList.Sum(s => s.FTR_CR_AMT_BC) - finTrxList.Where(i => i.FTR_ACCOUNT != Convert.ToInt32(ddlCashAccount.SelectedValue)).Sum(s => s.FTR_DR_AMT_BC);
                                    //list.Where(w => w.Name == "height").ToList().ForEach(s => s.Value = 30);
                                    finTrxList.Where(w => w.FTR_ACCOUNT == Convert.ToInt32(ddlCashAccount.SelectedValue)).ToList().ForEach(s => s.FTR_DR_AMT_BC = drSum);
                                    finTrxList.Where(w => w.FTR_ACCOUNT == Convert.ToInt32(ddlCashAccount.SelectedValue)).ToList().ForEach(s => s.FTR_DR_AMT_TC = s.FTR_DR_AMT_BC * exchangeRate);
                                }
                            }


                            Session["VoucherDet"] = finTrxList;
                            CurrFtrPK = 0;
                            hdfPreviousAccount1Name.Value = string.Empty;
                            hdfPreviousAccount1Pk.Value = "0";
                            CostCenterList.Clear();
                            hdfIsContCCAllocDeletion.Value = "0";
                            BindGrid();
                            ShowHideCostCenterAllocButton();
                        }
                        break;
                    #endregion

                    #region Commented Add Item
                    //case ActionsEnum.ADDLITEM:
                    //    if (!IsValid)
                    //    {
                    //        litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                    //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                    //    }
                    //    else//valid
                    //    {
                    //        //if (finCoaSubTypeCfgList[0].CST_REL_QUERY != null)
                    //        //{
                    //        //    if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                    //        //    {
                    //        //        relquery = finCoaSubTypeCfgList[0].CST_REL_QUERY == null ? string.Empty : finCoaSubTypeCfgList[0].ADM_QUERIES_CFG1.QRY_QUERY;
                    //        //    }

                    //        //    if (relquery != string.Empty)
                    //        //    {
                    //        //        commonService = new CommonService();
                    //        //        commonService = CommonFunctions.InitiateClient(commonService);
                    //        //        SubTypesAccounts = commonService.ExecuteQuery(relquery);
                    //        //        SetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                    //        //    }

                    //        //}
                    //        if (ddlSubTypeAccount1.Visible)
                    //        {
                    //            if (ddlSubTypeAccount1.SelectedValue == "-1")
                    //            {
                    //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_SubType").ToString()) + "');", true);
                    //                return;
                    //            }
                    //        }



                    //        finTrxList = Session["VoucherDet"] == null ? new List<FIN_TRX>() : finTrxList = (List<FIN_TRX>)(Session["VoucherDet"]);

                    //        finCoaMstObj = ERP.Utilities.CommonFunctions.Initilize<FIN_COA_MST>();
                    //        admConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONFIG_MST>();
                    //        finTrxObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX>();

                    //        if (CurrFtrPK == 0)
                    //        {
                    //            int? maxPk = finTrxList.Max(ftr => (int?)ftr.FTR_PK);
                    //            finTrxObj.FTR_PK = Convert.ToInt64(maxPk.HasValue ? maxPk + 1 : 1);
                    //        }
                    //        else
                    //        {
                    //            List<FIN_TRX> ftrList = (from ftr in finTrxList
                    //                                     where ftr.FTR_PK == CurrFtrPK
                    //                                     select ftr).ToList();

                    //            if (ftrList.Count > 0)
                    //            {
                    //                finTrxObj = ftrList[0];
                    //                //finCoaMstObj = ftrList[0].FIN_COA_MST;
                    //                finTrxList.Remove(finTrxObj);

                    //            }
                    //        }   

                    //        if (((ImageButton)sender).ID == "imbAddItem")
                    //        {
                    //            accountPk = Convert.ToInt32(hdfAccount.Value);
                    //        }
                    //        else
                    //        {
                    //            accountPk = Convert.ToInt32(hdfAccount1.Value);
                    //        }
                    //        finCoaMstList = new List<FIN_COA_MST>();
                    //        FinCoaMstServiceClient = new AccountMstService();
                    //        FinCoaMstServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(FinCoaMstServiceClient);

                    //        serviceUtilityObj = new ServiceUtility();
                    //        serviceUtilityObj.CurrentPage = -1;
                    //        serviceUtilityObj.PageSize = -1;

                    //        finCoaMstObj.COA_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                    //        finCoaMstObj.COA_PK = accountPk;
                    //        finCoaMstList = FinCoaMstServiceClient.GetFinCoaMst(finCoaMstObj, serviceUtilityObj);

                    //        finTrxObj.FTR_ACCOUNT = accountPk;
                    //        if (CurrPK == 0)
                    //        {

                    //            finTrxObj.FIN_COA_MST = finCoaMstList[0];

                    //        }
                    //        else if (CurrPK != 0 && CurrFtrPK == 0)
                    //        {
                    //            finTrxObj.FIN_COA_MST = finCoaMstList[0];
                    //        }

                    //        if (finCoaMstList.Count > 0)
                    //        {
                    //            //finTrxObj.FIN_COA_MST = finCoaMstList[0];
                    //            finTrxObj.FIN_COA_MST.COA_CODE = finCoaMstList[0].COA_CODE;
                    //            finTrxObj.FIN_COA_MST.COA_NAME = finCoaMstList[0].COA_NAME;
                    //            finTrxObj.FIN_COA_MST.COA_PK = finCoaMstList[0].COA_PK;
                    //            finTrxObj.FTR_ACC_SUB_TYPE = finCoaMstList[0].COA_SUB_TYPE;
                    //        }
                    //        if (Convert.ToBoolean(finTrxObj.FTR_PDC))
                    //        {
                    //            if (finTrxObj.FTR_ACC_SUB_TYPE != (int)AccSubType.PPC)
                    //            {
                    //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_PDC_Account").ToString()) + "');", true);
                    //                return;
                    //            }
                    //        }





                    //        if (((ImageButton)sender).ID == "imbAddItem")
                    //        {
                    //            #region Adding items from first section
                    //            accountPk = Convert.ToInt32(hdfAccount.Value);

                    //            admConfigMstObj.CFG_PK = Convert.ToInt32(ddlMode.SelectedValue);
                    //            admConfigMstObj.CFG_DATA = ddlMode.SelectedItem.Text;
                    //            // CFG_ACTIVE is set as 1 for identifying the item added from 1st section
                    //            admConfigMstObj.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);

                    //            finTrxObj.FTR_PAYMENT_MODE = Convert.ToInt32(ddlMode.SelectedValue);
                    //            finTrxObj.FTR_NARRATION = HttpUtility.HtmlEncode(txtNarration.Text.Trim());
                    //            if (AccountType == 2)
                    //            {
                    //                finTrxObj.FTR_DR_AMT_BC = Convert.ToDecimal(txtAmount.Text);
                    //                finTrxObj.FTR_DR_AMT_TC = finTrxObj.FTR_DR_AMT_BC * Convert.ToDecimal(txtExchangeRate.Text);
                    //                finTrxObj.FTR_CR_AMT_BC = 0;
                    //                finTrxObj.FTR_CR_AMT_TC = 0;
                    //            }
                    //            else
                    //            {
                    //                finTrxObj.FTR_DR_AMT_BC = 0;
                    //                finTrxObj.FTR_DR_AMT_TC = 0;
                    //                finTrxObj.FTR_CR_AMT_BC = Convert.ToDecimal(txtAmount.Text);
                    //                finTrxObj.FTR_CR_AMT_TC = finTrxObj.FTR_CR_AMT_BC * Convert.ToDecimal(txtExchangeRate.Text);
                    //            }
                    //            if (!string.IsNullOrEmpty(txtInstrNo.Text))
                    //                finTrxObj.FTR_INSTR_NO = HttpUtility.HtmlEncode(txtInstrNo.Text.Trim());
                    //            if (!string.IsNullOrEmpty(txtInstrDate.Text))
                    //                finTrxObj.FTR_INSTR_DATE = Convert.ToDateTime(txtInstrDate.Text.Trim());
                    //            if (!string.IsNullOrEmpty(txtFavourOf.Text))
                    //                finTrxObj.FTR_INSTR_FAVOUR = HttpUtility.HtmlEncode(txtFavourOf.Text.Trim());
                    //            //finTrxObj.FTR_PDC = chkPDC.Checked == true ? (byte)1 : (byte)0;

                    //            ResetForm(2);
                    //            this.txtAccount.Focus();
                    //            #endregion
                    //        }
                    //        else
                    //        {
                    //            #region Adding items from second section
                    //            accountPk = Convert.ToInt32(hdfAccount1.Value);

                    //            admConfigMstObj.CFG_PK = Convert.ToInt32(ddlMode1.SelectedValue);
                    //            admConfigMstObj.CFG_DATA = ddlMode1.SelectedItem.Text;
                    //            // CFG_ACTIVE is set as 2 for identifying the item added from 2nd section
                    //            admConfigMstObj.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.HASPK);

                    //            finTrxObj.FTR_PAYMENT_MODE = Convert.ToInt32(ddlMode1.SelectedValue);
                    //            finTrxObj.FTR_NARRATION = HttpUtility.HtmlEncode(txtNarration1.Text.Trim());
                    //            finTrxObj.FTR_DR_AMT_BC = txtDebit1.Text == string.Empty ? 0 : Convert.ToDecimal(txtDebit1.Text);
                    //            finTrxObj.FTR_DR_AMT_TC = finTrxObj.FTR_DR_AMT_BC * Convert.ToDecimal(txtExchangeRate.Text);
                    //            finTrxObj.FTR_CR_AMT_BC = txtCredit1.Text == string.Empty ? 0 : Convert.ToDecimal(txtCredit1.Text);
                    //            finTrxObj.FTR_CR_AMT_TC = finTrxObj.FTR_CR_AMT_BC * Convert.ToDecimal(txtExchangeRate.Text);

                    //            if (!string.IsNullOrEmpty(txtInstrNo1.Text))
                    //                finTrxObj.FTR_INSTR_NO = HttpUtility.HtmlEncode(txtInstrNo1.Text.Trim());
                    //            if (!string.IsNullOrEmpty(txtInstrDate1.Text))
                    //                finTrxObj.FTR_INSTR_DATE = Convert.ToDateTime(txtInstrDate1.Text.Trim());
                    //            if (!string.IsNullOrEmpty(txtFavourOf1.Text))
                    //                finTrxObj.FTR_INSTR_FAVOUR = HttpUtility.HtmlEncode(txtFavourOf1.Text.Trim());
                    //            finTrxObj.FTR_PDC = chkPDC1.Checked == true ? (byte)1 : (byte)0;

                    //            tempAdmConfigMstObj = admConfigMstObj;
                    //            ResetForm(3);
                    //            admConfigMstObj = tempAdmConfigMstObj;
                    //            this.txtAccount1.Focus();
                    //            #endregion
                    //        }

                    //        //finCoaMstList = new List<FIN_COA_MST>();
                    //        //FinCoaMstServiceClient = new AccountMstService();
                    //        //FinCoaMstServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(FinCoaMstServiceClient);

                    //        //serviceUtilityObj = new ServiceUtility();
                    //        //serviceUtilityObj.CurrentPage = -1;
                    //        //serviceUtilityObj.PageSize = -1;

                    //        //finCoaMstObj.COA_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                    //        //finCoaMstObj.COA_PK = accountPk;
                    //        ////finCoaMstObj.COA_CODE = string.Empty;
                    //        ////finCoaMstObj.COA_NAME = string.Empty;
                    //        //finCoaMstList = FinCoaMstServiceClient.GetFinCoaMst(finCoaMstObj, serviceUtilityObj);

                    //        //finTrxObj.FTR_ACCOUNT = accountPk;
                    //        //if (CurrPK == 0)
                    //        //{

                    //        //    finTrxObj.FIN_COA_MST = finCoaMstList[0];

                    //        //}
                    //        //else if (CurrPK != 0 && CurrFtrPK == 0)
                    //        //{
                    //        //    finTrxObj.FIN_COA_MST = finCoaMstList[0];
                    //        //}
                    //        //if (finCoaMstList.Count > 0)
                    //        //{
                    //        //    //finTrxObj.FIN_COA_MST = finCoaMstList[0];
                    //        //    finTrxObj.FIN_COA_MST.COA_CODE = finCoaMstList[0].COA_CODE;
                    //        //    finTrxObj.FIN_COA_MST.COA_NAME = finCoaMstList[0].COA_NAME;
                    //        //    finTrxObj.FIN_COA_MST.COA_PK = finCoaMstList[0].COA_PK;
                    //        //    finTrxObj.FTR_ACC_SUB_TYPE = finCoaMstList[0].COA_SUB_TYPE;
                    //        //}


                    //        hdfSubTypePk.Value = finTrxObj.FTR_ACC_SUB_TYPE.ToString();
                    //        ddlAccountSubType = finTrxObj.FTR_ACC_SUB_TYPE;
                    //        GetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                    //        if (finCoaSubTypeCfgList[0].CST_REL_QUERY != null)
                    //        {
                    //            if (ddlSubTypeAccount1.SelectedValue != "-1")
                    //                finTrxObj.FTR_TYPE_PK = Convert.ToInt32(ddlSubTypeAccount1.SelectedValue);
                    //            finTrxObj.FTR_TYPE = finCoaSubTypeCfgList[0].CST_CODE;
                    //        }
                    //        else
                    //        {
                    //            finTrxObj.FTR_TYPE = null;
                    //            finTrxObj.FTR_TYPE_PK = null;
                    //        }

                    //        finTrxObj.ADM_CONFIG_MST = admConfigMstObj;
                    //        finTrxObj.FTR_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                    //        finTrxObj.FTR_DEPT = Session[BusinessObject.Common.SessionStrings.CurDept] == null ? currentUser.CurrentDeptPK : Convert.ToInt16(Session[BusinessObject.Common.SessionStrings.CurDept].ToString());// Convert.ToInt16(Session[BusinessObject.Common.SessionStrings.CurDept].ToString());
                    //        finTrxObj.FTR_BIZUNIT = currentUser.SBUID;

                    //        finTrxList.Add(finTrxObj);

                    //        Session["VoucherDet"] = finTrxList;
                    //        CurrFtrPK = 0;


                    //        BindGrid();
                    //    }
                    //    break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDiv('#divWkfSubmit','" + Resources.ErpRes.Submit + "','950','400');", true);
                        break;
                    case ActionsEnum.WRKFSUBMIT:
                        //ucrWrkf.ApplicationID = 0;
                        //if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                        //{
                        if (grdVoucher.Rows.Count < 1)
                        {
                            litErrorMsg.Text = this.GetLocalResourceObject("Err_AccountNos").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            return;
                        }
                        if (hdfShowCashAccountDiv.Value == "1" && VoucherType == ApplicationType.PCVJ || VoucherType == ApplicationType.PCRVJ)
                            AddCashAccountCR();
                        if (hdfDebitTotal.Value != hdfCreditTotal.Value)
                        {
                            litErrorMsg.Text = this.GetLocalResourceObject("Err_Total").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            return;
                        }
                        if (!IsValidSession() && hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_MultipleTab_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            return;
                        }
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                if (grdVoucher.Rows.Count > 0)
                                {


                                    finTrxHeaderObj = (FinTrxHeaderBO)SetUIValuesToObject(ActionsEnum.WRKFSUBMIT, ControlsEnum.FINTRXHDR);
                                    if (finTrxHeaderObj != null && finTrxHeaderObj.FinTrxDetails != null)
                                    {
                                        #region Checking for voucher locked with financial year
                                        string LockUptoDate = string.Empty;
                                        if (BusinessLogic.Finance.VoucherLockingBL.IsVoucherLocked(finTrxHeaderObj.FTH_DATE, CurrPK, finTrxHeaderObj.FTH_BIZUNIT, ref LockUptoDate))
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Err_Voucher_Locked").ToString() + (!string.IsNullOrEmpty(LockUptoDate) ? DateTime.Parse(LockUptoDate).ToString(Resources.Constants.DateFormatShort) : string.Empty);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            return;
                                        }
                                        #endregion
                                        SaveTransaction(finTrxHeaderObj, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Voucher").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                            {
                                #region Checking for voucher locked with financial year
                                if (!string.IsNullOrEmpty(txtVoucherDate.Text))
                                {
                                    string LockUptoDate = string.Empty;
                                    if (BusinessLogic.Finance.VoucherLockingBL.IsVoucherLocked(DateTime.Parse(txtVoucherDate.Text), CurrPK, currentUser.SBUID, ref LockUptoDate))
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Err_Voucher_Locked").ToString() + (!string.IsNullOrEmpty(LockUptoDate) ? DateTime.Parse(LockUptoDate).ToString(Resources.Constants.DateFormatShort) : string.Empty);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        return;
                                    }
                                }
                                #endregion
                                IsSuccess = true;
                                if (VoucherType == ApplicationType.DPVJ)
                                {
                                    if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation((int)CurrPK, ApplicationType.DPVJ))
                                    {
                                        ucrWrkf.ApplicationID = (int)CurrPK;
                                        SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT), 1);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Err_PP_Cancel").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "','" +
                                            Page.ResolveClientUrl(Resources.PageURL.DirectPaymentList) + "');", true);
                                    }
                                }
                                else if (VoucherType == ApplicationType.PCVJ)
                                {
                                    if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation((int)CurrPK, ApplicationType.PCVJ))
                                    {
                                        ucrWrkf.ApplicationID = (int)CurrPK;
                                        SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Err_PP_Cancel").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "','" +
                                            Page.ResolveClientUrl(Resources.PageURL.PettyCashList) + "');", true);
                                    }
                                }

                                else if (VoucherType == ApplicationType.PCRVJ)
                                {
                                    if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation((int)CurrPK, ApplicationType.PCRVJ))
                                    {
                                        ucrWrkf.ApplicationID = (int)CurrPK;
                                        SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Err_PP_Cancel").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "','" +
                                            Page.ResolveClientUrl(Resources.PageURL.PettyCashReceiptList) + "');", true);
                                    }
                                }


                                //Contra Entry
                                else if (VoucherType == ApplicationType.CTVJ)
                                {
                                    if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation((int)CurrPK, ApplicationType.CTVJ))
                                    {
                                        ucrWrkf.ApplicationID = (int)CurrPK;
                                        SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Err_PP_Cancel").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "','" +
                                            Page.ResolveClientUrl(Resources.PageURL.ContraEntryList) + "');", true);
                                    }
                                }
                                else if (VoucherType == ApplicationType.YCV)
                                {
                                    if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation((int)CurrPK, ApplicationType.YCV))
                                    {
                                        ucrWrkf.ApplicationID = (int)CurrPK;
                                        SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Err_YCV_Cancel").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "','" +
                                            Page.ResolveClientUrl(Resources.PageURL.YearClosingVoucherList) + "');", true);
                                    }
                                }
                            }
                            else
                            {
                                ucrWrkf.ApplicationID = CurrPK;
                                IsSuccess = true;
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                            }
                        }

                        //*****
                        //if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                        //{
                        //    if (grdVoucher.Rows.Count > 0)
                        //    {
                        //    }

                        //    if (grdVoucher.Rows.Count > 0)
                        //    {
                        //        //finTrxList = new List<FIN_TRX>();
                        //        //finTrxHdrList = new List<FIN_TRX_HDR>();
                        //        //FinTrxServiceClient = new FinTrxService();
                        //        //FinTrxServiceClient = CommonFunctions.InitiateClient(FinTrxServiceClient);
                        //        //finTrxHdrObj = CommonFunctions.Initilize<FIN_TRX_HDR>();
                        //        //#region Checking for voucher locked with financial year
                        //        //if (finTrxHdrObj != null && !string.IsNullOrEmpty(txtVoucherDate.Text))
                        //        //{
                        //        //    string LockUptoDate = string.Empty;
                        //        //    if (BusinessLogic.Finance.VoucherLockingBL.IsVoucherLocked(DateTime.Parse(txtVoucherDate.Text), CurrPK, currentUser.SBUID, ref LockUptoDate))
                        //        //    {
                        //        //        litErrorMsg.Text = GetLocalResourceObject("Err_Voucher_Locked").ToString() + (!string.IsNullOrEmpty(LockUptoDate) ? DateTime.Parse(LockUptoDate).ToString(Resources.Constants.DateFormatShort) : string.Empty);
                        //        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //        //        return;
                        //        //    }
                        //        //}
                        //        //#endregion
                        //        finTrxHeaderObj = (FinTrxHeaderBO)SetUIValuesToObject(ActionsEnum.WRKFSUBMIT, ControlsEnum.FINTRXHDR);
                        //        if (finTrxHeaderObj != null && finTrxHeaderObj.FinTrxDetails != null)
                        //        {
                        //            SaveTransaction(finTrxHeaderObj, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
                        //        }

                        //        //#region Transaction Begin
                        //        //using (TransactionScope scope = new TransactionScope())
                        //        //{
                        //        //    try
                        //        //    {
                        //        //        //if (hdfVoucherNo.Value == string.Empty)
                        //        //        //{
                        //        //        //    GetFieldValues(ControlsEnum.VOUCHERNO);
                        //        //        //}
                        //        //        //if (hdfVoucherNo.Value == string.Empty && AST_DOC_MODE.Value == "1")
                        //        //        //{
                        //        //        //    GetFieldValues(ControlsEnum.VOUCHERNO);
                        //        //        //    txtVoucherNo.Text = hdfVoucherNo.Value;
                        //        //        //}
                        //        //        //finTrxHdrObj = (FIN_TRX_HDR)SetUIValuesToObject(ActionsEnum.SAVE, ControlsEnum.FINTRXHDR);
                        //        //        //  finTrxHdrObj = (FIN_TRX_HDR)SetUIValuesToObject(ActionsEnum.WRKFSUBMIT, ControlsEnum.FINTRXHDR);

                        //        //        if (finTrxHdrObj != null)
                        //        //        {

                        //        //            //if (!string.IsNullOrEmpty(finTrxHdrObj.FTH_PARTY_NAME) && !string.IsNullOrEmpty(finTrxHdrObj.FTH_REF_NO))
                        //        //            //{
                        //        //            //    if (FinTrxServiceClient.IsRefereceNoEsixt(finTrxHdrObj))
                        //        //            //    {
                        //        //            //        litErrorMsg.Text = GetLocalResourceObject("Msg_Refno_Exist").ToString();
                        //        //            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //        //            //        return;
                        //        //            //    }
                        //        //            //}
                        //        //            finTrxHdrList.Add(finTrxHdrObj);
                        //        //            result = FinTrxServiceClient.SaveDirectFinTrx(finTrxHdrList);
                        //        //            if (result >= 0)
                        //        //            {
                        //        //                ucrWrkf.ApplicationID = (int)result;
                        //        //                litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                        //        //                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Voucher);
                        //        //                GetFieldValues(ControlsEnum.FINTRX);
                        //        //                SetFieldValues(ControlsEnum.FINTRX);

                        //        //            }
                        //        //            else
                        //        //            {
                        //        //                if (result == (int)DbSaveStatus.SQLERROR)
                        //        //                {
                        //        //                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                        //        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        //        //                        + "','" + Resources.ErpRes.Information + "');", true);
                        //        //                }
                        //        //                else if (result == (int)DbSaveStatus.CONCURRENCY)
                        //        //                {
                        //        //                    litErrorMsg.Text = Resources.PageNameRes.miscellaneous + " " + Resources.Messages.EditUsedByAnotherUser;
                        //        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        //        //                    + "','" + Resources.ErpRes.Information + "');", true);
                        //        //                }
                        //        //                else if (result == (int)DbSaveStatus.CODEEXIST)
                        //        //                {
                        //        //                    litErrorMsg.Text = Resources.PageNameRes.miscellaneous + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                        //        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        //        //                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                        //        //                }
                        //        //                else if (result == (int)DbSaveStatus.REFNOEXIST)
                        //        //                {
                        //        //                    litErrorMsg.Text = Resources.PageNameRes.miscellaneous + " " + GetLocalResourceObject("RefNoExist").ToString();
                        //        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        //        //                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                        //        //                }
                        //        //                else if (result == -111)//If the voucher Date year is selected is not same as the current financial year
                        //        //                {
                        //        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_Financial_Year").ToString()) + "','" + Resources.Messages.Information + "');", true);
                        //        //                }
                        //        //                else
                        //        //                {
                        //        //                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                        //        //                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.miscellaneous);
                        //        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        //        //                        + "','" + Resources.ErpRes.Information + "');", true);
                        //        //                }
                        //        //                txtVoucherNo.Text = hdfVoucherNo.Value = string.Empty;
                        //        //                scope.Dispose();
                        //        //                return;
                        //        //            }
                        //        //        }
                        //        //        //Commit Transaction
                        //        //        scope.Complete();
                        //        //        IsSuccess = true;
                        //        //    }
                        //        //    catch (Exception ex)
                        //        //    {
                        //        //        IsSuccess = false;
                        //        //        // Handler for unknown exceptions
                        //        //        // Throws a new exception to client with class name - method name - server side exception process result as exception message
                        //        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
                        //        //    }
                        //        //    finally
                        //        //    {
                        //        //        //Disposing used objects
                        //        //        scope.Dispose();

                        //        //    }
                        //        //}

                        //        //#endregion
                        //    }
                        //    else
                        //    {
                        //        litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Voucher").ToString();
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //    }
                        //}
                        //else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                        //{
                        //    #region Checking for voucher locked with financial year
                        //    if (!string.IsNullOrEmpty(txtVoucherDate.Text))
                        //    {
                        //        string LockUptoDate = string.Empty;
                        //        if (BusinessLogic.Finance.VoucherLockingBL.IsVoucherLocked(DateTime.Parse(txtVoucherDate.Text), CurrPK, currentUser.SBUID, ref LockUptoDate))
                        //        {
                        //            litErrorMsg.Text = GetLocalResourceObject("Err_Voucher_Locked").ToString() + (!string.IsNullOrEmpty(LockUptoDate) ? DateTime.Parse(LockUptoDate).ToString(Resources.Constants.DateFormatShort) : string.Empty);
                        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //            return;
                        //        }
                        //    }
                        //    #endregion

                        //    IsSuccess = true;
                        //    if (VoucherType == ApplicationType.DPVJ)
                        //    {
                        //        if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation((int)CurrPK, ApplicationType.DPVJ))
                        //        {
                        //            ucrWrkf.ApplicationID = (int)CurrPK;
                        //        }
                        //        else
                        //        {
                        //            litErrorMsg.Text = GetLocalResourceObject("Err_PP_Cancel").ToString();
                        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "','" +
                        //                Page.ResolveClientUrl(Resources.PageURL.DirectPaymentList) + "');", true);
                        //        }
                        //    }
                        //    else if (VoucherType == ApplicationType.PCVJ)
                        //    {
                        //        if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation((int)CurrPK, ApplicationType.PCVJ))
                        //        {
                        //            ucrWrkf.ApplicationID = (int)CurrPK;
                        //        }
                        //        else
                        //        {
                        //            litErrorMsg.Text = GetLocalResourceObject("Err_PP_Cancel").ToString();
                        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "','" +
                        //                Page.ResolveClientUrl(Resources.PageURL.PettyCashList) + "');", true);
                        //        }
                        //    }
                        //    else if (VoucherType == ApplicationType.YCV)
                        //    {
                        //        if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation((int)CurrPK, ApplicationType.YCV))
                        //        {
                        //            ucrWrkf.ApplicationID = (int)CurrPK;
                        //        }
                        //        else
                        //        {
                        //            litErrorMsg.Text = GetLocalResourceObject("Err_YCV_Cancel").ToString();
                        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "','" +
                        //                Page.ResolveClientUrl(Resources.PageURL.YearClosingVoucherList) + "');", true);
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    ucrWrkf.ApplicationID = CurrPK;
                        //    IsSuccess = true;
                        //}
                        //if (ucrWrkf.ApplicationID > 0 && IsSuccess)
                        //{
                        //    ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                        //    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                        //    //Do WorkFlow if WorkFlow has Actions
                        //    if (ddlWkfAction.Items.Count > 0)
                        //    {
                        //        //if (hdfVoucherNo.Value == string.Empty)
                        //        //{
                        //        //    finTrxList = new List<FIN_TRX>();
                        //        //    finTrxHdrList = new List<FIN_TRX_HDR>();
                        //        //    FinTrxServiceClient = new FinTrxService();
                        //        //    FinTrxServiceClient = CommonFunctions.InitiateClient(FinTrxServiceClient);
                        //        //    finTrxHdrObj = CommonFunctions.Initilize<FIN_TRX_HDR>();


                        //        //    GetFieldValues(ControlsEnum.VOUCHERNO);
                        //        //    txtVoucherNo.Text = hdfVoucherNo.Value;

                        //        //    finTrxHdrObj = (FIN_TRX_HDR)SetUIValuesToObject(ActionsEnum.WRKFSUBMIT, ControlsEnum.FINTRXHDR);
                        //        //    if (finTrxHdrObj != null)
                        //        //    {
                        //        //        finTrxHdrList.Add(finTrxHdrObj);
                        //        //        result = FinTrxServiceClient.SaveFinTrx(finTrxHdrList);

                        //        //    }
                        //        //}
                        //        action = ddlWkfAction.SelectedItem.ToString();
                        //        result = ucrWrkf.DoWorkFlow();
                        //        if (result > 0)
                        //        {
                        //            ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();
                        //            if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                        //            {
                        //                litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;
                        //                AdmTrxLogDet.ATL_ACTION = (byte)LogAction.CANCEL;
                        //            }
                        //            else
                        //            {
                        //                litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                        //                AdmTrxLogDet.ATL_ACTION = (byte)LogAction.SUBMIT;
                        //            }
                        //            hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        //            object[] args = new object[2];
                        //            args[0] = Resources.PageNameRes.Journalize;
                        //            args[1] = hdfVoucherNo.Value;

                        //            #region LOG SAVE

                        //            CommonServiceClient = new CommonService();
                        //            CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        //            List<ADM_APP_TRX_LOG> AdmTrxLogList = new List<ADM_APP_TRX_LOG>();
                        //            AdmTrxLogDet.ATL_APP_TRX_CODE = hdfVoucherNo.Value;
                        //            AdmTrxLogDet.ATL_APP_TYPE = VoucherType;
                        //            AdmTrxLogDet.ATL_MOD_BY = currentUser.PKUser;
                        //            AdmTrxLogDet.ATL_MOD_DT = DateTime.Now;
                        //            AdmTrxLogDet.ATL_BIZUNIT = currentUser.SBUID;
                        //            AdmTrxLogDet.ATL_APP_TRX_PK = ucrWrkf.ApplicationID;
                        //            AdmTrxLogDet.ATL_PK = 0;
                        //            AdmTrxLogList.Add(AdmTrxLogDet);
                        //            CommonServiceClient.SaveLog(AdmTrxLogList);

                        //            #endregion

                        //            if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ReferanceID > 0)
                        //            {
                        //                if (VoucherType == ApplicationType.OBV)
                        //                {
                        //                    args[0] = Resources.PageNameRes.OpeningBalance;
                        //                }
                        //                else if (VoucherType == ApplicationType.DPVJ)
                        //                {
                        //                    args[0] = Resources.PageNameRes.DirectPayment;
                        //                }
                        //                else if (VoucherType == ApplicationType.PCVJ)
                        //                {
                        //                    args[0] = Resources.PageNameRes.PettyCash;
                        //                }
                        //                else if (VoucherType == ApplicationType.YCV)
                        //                {
                        //                    args[0] = Resources.PageNameRes.YearClosing;
                        //                }

                        //                litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        //                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                        //            }
                        //            else
                        //            {

                        //                if (VoucherType == ApplicationType.OBV)
                        //                {
                        //                    args[0] = Resources.PageNameRes.OpeningBalance;
                        //                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                        //                        Page.ResolveClientUrl(Resources.PageURL.OpeningBalanceList) + "');", true);
                        //                }
                        //                else if (VoucherType == ApplicationType.DPVJ)
                        //                {
                        //                    args[0] = Resources.PageNameRes.DirectPayment;
                        //                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                        //                        Page.ResolveClientUrl(Resources.PageURL.DirectPaymentList) + "');", true);
                        //                }
                        //                else if (VoucherType == ApplicationType.PCVJ)
                        //                {
                        //                    args[0] = Resources.PageNameRes.PettyCash;
                        //                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                        //                        Page.ResolveClientUrl(Resources.PageURL.PettyCashList) + "');", true);
                        //                }
                        //                else if (VoucherType == ApplicationType.YCV)
                        //                {
                        //                    args[0] = Resources.PageNameRes.YearClosing;
                        //                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                        //                        Page.ResolveClientUrl(Resources.PageURL.YearClosingVoucherList) + "');", true);
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    //Trx not saved
                        //    litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Error_NoPK;
                        //    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Voucher);
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        //        + "','" + Resources.ErpRes.Information + "');", true);
                        //}
                        //}

                        //}
                        //else
                        //ucrWrkf.ApplicationID = CurrPK;



                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        showPopup = false;
                        hdfAmntMissmatch.Value = string.Empty;
                        if (grdVoucher.Rows.Count < 1)
                        {
                            litErrorMsg.Text = this.GetLocalResourceObject("Err_AccountNos").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            return;
                        }
                        if (VoucherType == ApplicationType.DPVJ || VoucherType == ApplicationType.PCVJ || VoucherType == ApplicationType.PCRVJ)
                        {

                            TextBox txtWHTAmount = (TextBox)grdVoucher.FooterRow.FindControl("txtWHTAmount");
                            decimal whtAmount = string.IsNullOrEmpty(hdfWithHoldTax.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(hdfWithHoldTax.Value);
                            if (whtAmount > 0 && txtWHTAmount != null && !string.IsNullOrEmpty(txtWHTAmount.Text))
                            {
                                tempWHTTaxDetails = null;
                                decimal.TryParse(hdfWithHoldTax.Value, out whtTax);
                                if (TempWHTTaxDetails != null && TempWHTTaxDetails.Count > 0)
                                {
                                    tempWHTTaxDetails = WHTTaxDetails = TempWHTTaxDetails;
                                    withHoldTax = Convert.ToDecimal(CommonFunctions.DoubleFormatRound(Convert.ToDouble(tempWHTTaxDetails.Sum(aa => aa.WTH_TAX_AMT)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));

                                    if (whtTax != withHoldTax && whtTax > 0 && (hdfIscontYes.Value != "1"))
                                    {
                                        hdfAmntMissmatch.Value = GetLocalResourceObject("WHTNotTallied").ToString();
                                        showPopup = true;
                                    }
                                }
                                else if (whtTax > 0)
                                {
                                    hdfAmntMissmatch.Value = GetLocalResourceObject("Err_WhtAmntNotEntered").ToString();
                                    showPopup = true;
                                }
                            }


                            TextBox txtVatBuy = (TextBox)grdVoucher.FooterRow.FindControl("txtVatBuy");
                            decimal vatAmount = string.IsNullOrEmpty(hdfVatBuy.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(hdfVatBuy.Value);
                            if (vatAmount > 0 && txtVatBuy != null && !string.IsNullOrEmpty(txtVatBuy.Text))
                            {
                                tempVATTaxDetails = null;
                                decimal.TryParse(hdfVatBuy.Value, out vatTax);
                                if (TempVATTaxDetails != null && TempVATTaxDetails.Count > 0)
                                {
                                    tempVATTaxDetails = VATTaxDetails = TempVATTaxDetails;
                                    VatBuyTax = Convert.ToDecimal(CommonFunctions.DoubleFormatRound(Convert.ToDouble(tempVATTaxDetails.Sum(aa => aa.WTH_TAX_AMT)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                                    if (vatTax != VatBuyTax && vatTax > 0 && (hdfIscontYes.Value != "1"))
                                    {
                                        hdfAmntMissmatch.Value += GetLocalResourceObject("VATNotTallied").ToString();
                                        showPopup = true;
                                    }
                                }
                                else if (vatTax > 0)
                                {
                                    hdfAmntMissmatch.Value += GetLocalResourceObject("Err_VatAmntNotEntered").ToString();
                                    showPopup = true;
                                }
                            }
                            hdfAmntMissmatch.Value += GetLocalResourceObject("DoYouWantToContinue").ToString();
                            if ((hdfIscontYes.Value != "1") && showPopup)
                            {
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_WHTMissmatch").ToString()) + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlreadyPaid", "$(document).ready(function(){WhtAmtMismatchSubmit();});", true);
                                return;
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
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
                    #region SUBTYPE
                    case ActionsEnum.CHANGE:

                        isContinue = true;
                        #region CostAllocation Clearing (Do you want to continue?)

                        List<FIN_TRX> finTrxListClear = new List<FIN_TRX>();
                        if (CurrFtrPK > 0)
                        {
                            finTrxListClear = (List<FIN_TRX>)(Session["VoucherDet"]);
                            finTrxListClear = (from ftrList in finTrxListClear
                                               where ftrList.FTR_PK == CurrFtrPK
                                               select ftrList).ToList();
                            if (finTrxListClear[0].FIN_TRX_COC_DTL != null && finTrxListClear[0].FIN_TRX_COC_DTL.Count > 0)
                            {
                                if (hdfIsContCCAllocDeletion.Value != "1")
                                {
                                    isContinue = false;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InvoiceNowExceeds", "$(document).ready(function(){ShowCostCenterAllocationClear(1);});", true);
                                }
                                else
                                {
                                    if (finTrxListClear[0].FIN_TRX_COC_DTL != null && finTrxListClear[0].FIN_TRX_COC_DTL.Count > 0)
                                    {
                                        List<FIN_TRX_COC_DTL> lstCocDtl = finTrxListClear[0].FIN_TRX_COC_DTL.ToList();
                                        foreach (FIN_TRX_COC_DTL cocDtl in lstCocDtl)
                                        {
                                            finTrxListClear[0].FIN_TRX_COC_DTL.Remove(cocDtl);
                                        }
                                    }

                                    CostCenterList.Clear();
                                }
                            }
                        }
                        else
                        {
                            if (CostCenterList != null && CostCenterList.Count > 0)
                            {
                                if (hdfIsContCCAllocDeletion.Value != "1")
                                {
                                    isContinue = false;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InvoiceNowExceeds", "$(document).ready(function(){ShowCostCenterAllocationClear(1);});", true);
                                }
                                else
                                {
                                    CostCenterList.Clear();
                                }
                            }
                        }

                        #endregion

                        if (isContinue)
                        {
                            ShowHideCostCenterAllocButton();
                            if (((Button)sender).ID == "btnAccount")
                            {
                                ddlAccountType = hdfAccount.Value == "" ? 0 : Convert.ToInt32(hdfAccount.Value);
                            }
                            else
                            {
                                ddlAccountType = hdfAccount1.Value == "" ? 0 : Convert.ToInt32(hdfAccount1.Value);
                            }
                            chkIsBankCharge1.Checked = false;
                            GetFieldValues(ControlsEnum.FINCOAMST);
                            if (finCoaMstList != null && finCoaMstList.Count > 0)
                            {
                                ddlAccountSubType = Convert.ToInt32(finCoaMstList[0].COA_SUB_TYPE.ToString());
                                //hdfSubTypePk1.Value = finCoaMstList[0].COA_SUB_TYPE.ToString();
                                if (finCoaMstList[0].FIN_COA_SUB_TYPE_CFG.CST_IS_BANK_CHARGE == 1 && hdfShowOthChrgChkBox.Value == "1")
                                    chkIsBankCharge1.Checked = true;

                                GetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                                if (finCoaSubTypeCfgList[0].CST_REL_QUERY != null)
                                {
                                    if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                                    {
                                        relquery = finCoaSubTypeCfgList[0].CST_REL_QUERY == null ? string.Empty : finCoaSubTypeCfgList[0].ADM_QUERIES_CFG1.QRY_QUERY;
                                    }

                                    if (relquery != string.Empty)
                                    {
                                        CommonServiceClient = new CommonService();
                                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);

                                        if (finCoaSubTypeCfgList[0].CST_CODE == "AP" || finCoaSubTypeCfgList[0].CST_CODE == "AR")
                                        {
                                            relquery = relquery.Replace("@COA@", ddlAccountType > 0 ? ddlAccountType.ToString() : "NULL");
                                            relquery = relquery.Replace("@ADV@", "NULL");
                                        }
                                        else if (finCoaSubTypeCfgList[0].CST_CODE == "ADP" || finCoaSubTypeCfgList[0].CST_CODE == "ADR")
                                        {
                                            relquery = relquery.Replace("@COA@", "NULL");
                                            relquery = relquery.Replace("@ADV@", ddlAccountType > 0 ? ddlAccountType.ToString() : "NULL");
                                        }
                                        else
                                        {
                                            relquery = relquery.Replace("@COA@", "NULL");
                                            relquery = relquery.Replace("@ADV@", "NULL");
                                        }
                                        relquery = relquery.Replace("@PK@", "NULL");
                                        relquery = relquery.Replace("@BIZUNITPK@", currentUser.SBUID.ToString());
                                        SubTypesAccounts = CommonServiceClient.ExecuteQuery(relquery);
                                        SetFieldValues(ControlsEnum.FINCOASUBTYPECFG);


                                        #region Sub Type Selected Value
                                        string defQuery = finCoaSubTypeCfgList[0].CST_DEF_QUERY == null ? string.Empty : finCoaSubTypeCfgList[0].ADM_QUERIES_CFG_DEF_QUERY.QRY_QUERY;
                                        if (defQuery != string.Empty)//Have default Query
                                        {
                                            defQuery = defQuery.Contains(GetLocalResourceObject("DefaultQryCondtion").ToString())
                                                ? defQuery.Replace(GetLocalResourceObject("DefaultQryCondtion").ToString(), ddlAccountType.ToString()) : defQuery;
                                            CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                                            List<DDLMaster> defaultValuesList = CommonServiceClient.ExecuteQuery(defQuery);
                                            if (defaultValuesList != null && defaultValuesList.Count == 1)
                                            {
                                                ddlSubTypeAccount1.SelectedValue = (ListItem)ddlSubTypeAccount1.Items.FindByValue(defaultValuesList[0].PK.ToString()) != null
                                                    ? defaultValuesList[0].PK.ToString() : CommonConstants.SELECTVAL;//Set default value to subledger ddl
                                            }
                                            else
                                            {
                                                ddlSubTypeAccount1.SelectedValue = CommonConstants.SELECTVAL;
                                            }
                                        }
                                        else
                                        {
                                            ddlSubTypeAccount1.SelectedValue = CommonConstants.SELECTVAL;
                                        }

                                        #endregion

                                    }
                                    else
                                    {
                                        ddlSubTypeAccount1.Visible = false;
                                        ddlSubTypeAccount.Visible = false;
                                    }
                                }
                                else
                                {
                                    ddlSubTypeAccount1.Visible = false;
                                    ddlSubTypeAccount.Visible = false;
                                }
                            }
                            else
                            {
                                ddlSubTypeAccount1.Visible = false;
                                ddlSubTypeAccount.Visible = false;
                            }

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPDCforCheque", "$(document).ready(function(){ShowPDCforCheque();});", true);
                        }
                        break;

                    #endregion
                    #region WHTTAXHEADER
                    case ActionsEnum.WHTTAXHEADER:
                        //TempWHTTaxDetails = WHTTaxDetails;                       
                        divErrorLabel.Visible = false;
                        lblSplitErrorMessage.Text = string.Empty;
                        GetFieldValues(ControlsEnum.WHTVENDOR);
                        SetFieldValues(ControlsEnum.WHTVENDOR);
                        GetFieldValues(ControlsEnum.VENDORACCOUNT);
                        SetFieldValues(ControlsEnum.VENDORACCOUNT);
                        GetFieldValues(ControlsEnum.VENDOR);
                        SetFieldValues(ControlsEnum.WHTPOPUPGRID);
                        SetFieldValues(ControlsEnum.VENDOR);
                        GetFieldValues(ControlsEnum.PAYMENTTYPE);
                        SetFieldValues(ControlsEnum.PAYMENTTYPE);

                        ddlFormno.Focus();
                        ShowWhtPopup();
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("WHTTaxDetails").ToString() + "','800','400');", true);

                        break;
                    #endregion
                    #region WHT ACCOUNT Changed
                    case ActionsEnum.WHT_ACCOUNT_INDEX_CHANGED_POPUP:
                        //SetVendorPayment();
                        //whtTaxpk = ddlWHTAccountPopup.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlWHTAccountPopup.SelectedValue) : 0;
                        whtTaxpk = !string.IsNullOrEmpty(hdfWHTAccountPopup.Value) ? Convert.ToInt32(hdfWHTAccountPopup.Value) : 0;
                        //if (chkVendorforpayemnt.Checked)
                        //{
                        GetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
                        //SetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
                        if (dtVendorAccount != null && dtVendorAccount.Rows.Count > 0)
                        {
                            decimal amount = 0;
                            amount = txtPopupWHTAmount.Text != string.Empty ? Convert.ToDecimal(txtPopupWHTAmount.Text) : 0;
                            string taxFormula = dtVendorAccount.Rows[0]["TAX_FORMULA"].ToString();
                            hdfTaxformula.Value = taxFormula;
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            decimal amt = Convert.ToDecimal(StringToFormula(taxFormula));
                            txtWHTTaxAmountPopup.Text = ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(amt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();// Math.Round(amt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            txtDescriptionPopup.Text = dtVendorAccount.Rows[0]["TAX_DESC"].ToString();
                            hdfWHTTaxCategory.Value = dtVendorAccount.Rows[0]["TAX_CATEGORY"].ToString();
                            hdfWHTTaxName.Value = dtVendorAccount.Rows[0]["TAX_HEAD"].ToString();
                        }
                        else
                        {
                            txtWHTTaxAmountPopup.Text = Math.Round(Convert.ToDouble(0), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            txtDescriptionPopup.Text = string.Empty;
                        }
                        ShowWhtPopup();
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("WHTTaxDetails").ToString() + "','800','400');", true);
                        //}
                        //else
                        //{
                        //    txtWHTAmount.Text = "0.00";
                        //}

                        break;
                    #endregion
                    #region WHT TAX ADD
                    case ActionsEnum.WHTTAXADD:
                        bool errorWHTAdd = false;
                        bool errorWHTAmount = false;
                        int WhtDetRowIndex = -1;
                        if (ViewState["WhtDetRowIndex"] != null) WhtDetRowIndex = (int)(ViewState["WhtDetRowIndex"]);

                        int? WHTVendorAddressType = null;
                        if (!string.IsNullOrEmpty(hdfWthAddressType.Value))
                        {
                            WHTVendorAddressType = Convert.ToInt32(hdfWthAddressType.Value);
                        }
                        tempWHTTaxDetails = null;
                        tempWHTTaxDetails = TempWHTTaxDetails;
                        if (tempWHTTaxDetails != null && WhtDetRowIndex >= 0)
                        {
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_TAX = Convert.ToInt32(hdfWHTAccountPopup.Value);
                            //tempWHTTaxDetails.WTH_PK = 0;
                            //tempWHTTaxDetails.WTH_PAYMENT_HDR = CurrPK;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_TRX_HDR = CurrPK;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_TYPE = 1;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_CATEGORY = (byte)WHTCategoryEnum.WHT;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_TAX_CATEGORY = string.IsNullOrEmpty(hdfWHTTaxCategory.Value) ? Convert.ToByte(1) : Convert.ToByte(hdfWHTTaxCategory.Value);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_NAME = hdfWHTTaxName.Value;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_DESC = HttpUtility.HtmlEncode(txtDescriptionPopup.Text);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_FORM_NO = Convert.ToInt32(ddlFormno.SelectedValue);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_PARTY_NAME = HttpUtility.HtmlEncode(txtCustomerTxtWHT.Text);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_ADDRESS = HttpUtility.HtmlEncode(txtpartyads.Text);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_TAX_ID = txtTaxid.Text;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_AMOUNT = Convert.ToDecimal(txtPopupWHTAmount.Text);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_TAX_AMT = Convert.ToDecimal(txtWHTTaxAmountPopup.Text);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_PAYMENT_TYPE = Convert.ToByte(ddlPayType.SelectedValue);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_BRANCH_TEXT = HttpUtility.HtmlEncode(txtWthBranchCode.Text);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_BRANCH = WHTVendorAddressType;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_BRANCH_NAME = HttpUtility.HtmlEncode(txtWthAddressType.Text);
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_BRANCH_TYPE = chkWthHeadOffice.Checked ? (byte)VendorContactTypeEnum.HeadOffice : (byte)VendorContactTypeEnum.Branch;
                            tempWHTTaxDetails[WhtDetRowIndex].WTH_CERT_NO = HttpUtility.HtmlEncode(txtCRTNo.Text);

                            TempWHTTaxDetails = tempWHTTaxDetails;
                            SetFieldValues(ControlsEnum.WHTPOPUPGRID);
                            finVatPaymentDetails = null;
                            ViewState["WhtDetRowIndex"] = null;
                            txtPopupWHTAmount.Text = string.Empty;
                            txtWHTTaxAmountPopup.Text = string.Empty;
                            txtDescriptionPopup.Text = string.Empty;
                            ddlFormno.SelectedIndex = 0;
                            txtWHTAccountPopup.Text = "Select/Type";

                            ddlPayType.SelectedIndex = 0;

                        }
                        else
                        {
                            tempWHTTax = null;
                            tempWHTTax = tempWHTTaxDetails.SingleOrDefault(tax => tax.WTH_TAX == Convert.ToInt32(hdfWHTAccountPopup.Value) && tax.WTH_FORM_NO == Convert.ToInt32(ddlFormno.SelectedValue) && tax.WTH_PARTY_NAME == txtCustomerTxtWHT.Text);
                            if (tempWHTTax == null)
                            {
                                tempWHTTax = new FIN_PAYMENT_VND_TAX_HDR();
                                tempWHTTax = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_HDR>();
                                //try
                                //{
                                tempWHTTax.WTH_AMOUNT = string.IsNullOrEmpty(txtPopupWHTAmount.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtPopupWHTAmount.Text);
                                tempWHTTax.WTH_TAX_AMT = string.IsNullOrEmpty(txtWHTTaxAmountPopup.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtWHTTaxAmountPopup.Text);
                                //}
                                //catch
                                //{
                                //    errorWHTAmount = true;
                                //}
                                if (!errorWHTAmount)
                                {
                                    tempWHTTax.WTH_TAX = Convert.ToInt32(hdfWHTAccountPopup.Value);
                                    tempWHTTax.WTH_PK = 0;
                                    //tempWHTTax.WTH_PAYMENT_HDR = CurrPK;
                                    tempWHTTax.WTH_TRX_HDR = CurrPK;
                                    tempWHTTax.WTH_TYPE = 1;
                                    tempWHTTax.WTH_CATEGORY = (byte)WHTCategoryEnum.WHT;
                                    tempWHTTax.WTH_TAX_CATEGORY = string.IsNullOrEmpty(hdfWHTTaxCategory.Value) ? Convert.ToByte(1) : Convert.ToByte(hdfWHTTaxCategory.Value);
                                    tempWHTTax.WTH_NAME = hdfWHTTaxName.Value;
                                    tempWHTTax.WTH_DESC = HttpUtility.HtmlEncode(txtDescriptionPopup.Text);
                                    tempWHTTax.WTH_FORM_NO = Convert.ToInt32(ddlFormno.SelectedValue);
                                    tempWHTTax.WTH_PARTY_NAME = HttpUtility.HtmlEncode(txtCustomerTxtWHT.Text);
                                    tempWHTTax.WTH_ADDRESS = HttpUtility.HtmlEncode(txtpartyads.Text);
                                    tempWHTTax.WTH_TAX_ID = txtTaxid.Text;
                                    tempWHTTax.WTH_TAX_DATE = string.IsNullOrEmpty(txtVoucherDate.Text.Trim()) ? DateTime.Now : DateTime.Parse(txtVoucherDate.Text.Trim());
                                    tempWHTTax.WTH_PAYMENT_TYPE = Convert.ToByte(ddlPayType.SelectedValue);
                                    tempWHTTax.WTH_BRANCH_TEXT = HttpUtility.HtmlEncode(txtWthBranchCode.Text);
                                    tempWHTTax.WTH_BRANCH = WHTVendorAddressType;
                                    tempWHTTax.WTH_BRANCH_NAME = HttpUtility.HtmlEncode(txtWthAddressType.Text);
                                    tempWHTTax.WTH_BRANCH_TYPE = chkWthHeadOffice.Checked ? (byte)VendorContactTypeEnum.HeadOffice : (byte)VendorContactTypeEnum.Branch;
                                    if (hdfCRTNoAuto.Value == "0")
                                        tempWHTTax.WTH_CERT_NO = HttpUtility.HtmlEncode(txtCRTNo.Text);

                                    tempWHTTaxDetails.Add(tempWHTTax);
                                    TempWHTTaxDetails = tempWHTTaxDetails;
                                    SetFieldValues(ControlsEnum.WHTPOPUPGRID);
                                    finVatPaymentDetails = null;
                                    ViewState["WhtDetRowIndex"] = null;
                                }
                            }
                            else
                            {
                                errorWHTAdd = true;
                            }
                            if (!errorWHTAdd && !errorWHTAmount)
                            {
                                txtPopupWHTAmount.Text = string.Empty;
                                txtWHTTaxAmountPopup.Text = string.Empty;
                                txtDescriptionPopup.Text = string.Empty;
                                //txtTaxid.Text = string.Empty;
                                //txtCustomerTxtWHT.Text = string.Empty;
                                //txtpartyads.Text = string.Empty;
                                ddlFormno.SelectedIndex = 0;
                                txtWHTAccountPopup.Text = "Select/Type";
                            }
                        }
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CloseMsgPopup1", "CloseMsgPopup();", true);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("WHTTaxDetails").ToString() + "','800','400');", true);
                        ShowWhtPopup();
                        if (errorWHTAdd)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Add").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (errorWHTAmount)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Amount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region WHTTAXDELETE
                    case ActionsEnum.WHTTAXDELETE:
                        tempWHTTaxDetails = null;
                        if (TempWHTTaxDetails != null)
                        {
                            tempWHTTax = null;
                            tempWHTTaxDetails = TempWHTTaxDetails;
                            HiddenField hdfTaxPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfWHTTaxPK") as HiddenField);
                            HiddenField hdfTaxName = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfWHTTaxName1") as HiddenField);
                            if (hdfTaxPK != null)
                            {
                                int taxPK = string.IsNullOrEmpty(hdfTaxPK.Value) ? 0 : Convert.ToInt32(hdfTaxPK.Value);
                                if (taxPK > 0)
                                {
                                    tempWHTTax = tempWHTTaxDetails.SingleOrDefault(rfq => rfq.WTH_PK == taxPK);// && rfq.WTH_TAX_CATEGORY == Convert.ToInt32(hdfWHTTaxCategory.Value));
                                }
                                else
                                {
                                    if (hdfTaxName != null)
                                    {
                                        tempWHTTax = tempWHTTaxDetails.SingleOrDefault(rfq => rfq.WTH_NAME == hdfTaxName.Value);// && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                }
                                if (tempWHTTax != null)
                                {
                                    tempWHTTaxDetails.Remove(tempWHTTax);
                                    TempWHTTaxDetails = tempWHTTaxDetails;
                                }
                            }
                            SetFieldValues(ControlsEnum.WHTPOPUPGRID);
                        }
                        ShowWhtPopup();
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("WHTTaxDetails").ToString() + "','800','400');", true);
                        break;
                    #endregion
                    #region TAXAPPLY
                    case ActionsEnum.WHTTAXAPPLY:
                        tempWHTTaxDetails = null;
                        divErrorLabel.Visible = false;
                        //if (TempWHTTaxDetails != null && TempWHTTaxDetails.Count > 0)
                        //{
                        tempWHTTaxDetails = WHTTaxDetails = TempWHTTaxDetails;
                        //withHoldTax = Convert.ToDecimal(tempWHTTaxDetails.Sum(aa => aa.WTH_TAX_AMT));
                        //decimal.TryParse(hdfWithHoldTax.Value, out whtTax);
                        //if (withHoldTax > whtTax)
                        //{
                        //    divErrorLabel.Visible = true;
                        //    lblSplitErrorMessage.Text = GetLocalResourceObject("Err_WHTAmntTotalSplit").ToString() + whtTax.ToString();
                        //    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_WHTAmntTotalSplit").ToString() + whtTax.ToString()) + "');", true);
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        //    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideOverlay", "HideOverlay();", true);
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("WHTTaxDetails").ToString() + "','800','400');", true);
                        //}
                        //else
                        //{
                        //    tempWHTTaxDetails = null;
                        //}
                        //}
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;

                    #endregion
                    #region Edit WHT Item From Grid
                    case ActionsEnum.POPUPGRIDEDITWHT:
                        GridViewRow grwWhtDetails = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        SetUIEditViewWhtPopup(grwWhtDetails);
                        ShowWhtPopup();
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("WHTTaxDetails").ToString() + "','800','400');", true);
                        break;
                    #endregion
                    #region WHTTAXSAVE
                    case ActionsEnum.WHTTAXSAVE:

                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(poPaymentServiceClient);
                        //tempVATTaxDetails = null;
                        divErrorLabel.Visible = false;
                        if (grdWHTTaxDetails.Rows.Count > 0)
                        {
                            tempWHTTaxDetails = null;
                            decimal.TryParse(hdfWithHoldTax.Value, out whtTax);
                            if (TempWHTTaxDetails != null && TempWHTTaxDetails.Count > 0)
                            {
                                tempWHTTaxDetails = WHTTaxDetails = TempWHTTaxDetails;
                                withHoldTax = Convert.ToDecimal(CommonFunctions.DoubleFormatRound(Convert.ToDouble(tempWHTTaxDetails.Sum(aa => aa.WTH_TAX_AMT)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));

                                if (whtTax != withHoldTax && whtTax > 0 && (hdfIscontYes.Value != "1"))
                                {
                                    hdfAmntMissmatch.Value = GetLocalResourceObject("Err_WhtAmntNotTallied").ToString();
                                    showPopup = true;
                                }
                            }
                            else if (whtTax > 0)
                            {
                                hdfAmntMissmatch.Value = GetLocalResourceObject("Err_WhtAmntNotEntered").ToString();
                                showPopup = true;
                            }
                            if ((hdfIscontYesVat.Value != "1") && showPopup)
                            {
                                ShowWhtPopup();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlreadyPaid", "$(document).ready(function(){WhtTaxAmtMismatch();});", true);
                                return;
                            }
                            hdfIscontYesVat.Value = "0";
                            vatTaxresult = poPaymentServiceClient.SaveWhtTaxDirectForDirectPayment(tempWHTTaxDetails);
                            if (vatTaxresult >= 0)
                            {
                                litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), GetLocalResourceObject("Tax").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            }

                            //}
                        }
                        else
                        {
                            try
                            {
                                tempWHTTaxDetails = WHTTaxDetails = TempWHTTaxDetails;
                            }
                            catch { }
                            long? Saveresult = poPaymentServiceClient.DeleteDirectVatTaxForDirectPayment(Convert.ToInt64(CurrPK), (byte)WHTCategoryEnum.WHT);
                            if (Saveresult.HasValue && Saveresult >= 0)
                            {
                                litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), GetLocalResourceObject("Tax").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            }
                            //ShowWhtPopup();
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_VatTaxSave").ToString()) + "','" + Resources.Messages.Information + "');", true);
                        }

                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);

                        break;
                    #endregion
                    #region PrintWHT
                    case ActionsEnum.PRINTWHT:
                        if (CurrPK > 0)
                        {
                            if (VoucherType == ApplicationType.DPVJ || VoucherType == ApplicationType.PCVJ)
                            {
                                if (TempWHTTaxDetails != null && TempWHTTaxDetails.Count > 0)
                                {
                                    //if (TempWHTTaxDetails[0].WTH_FORM_NO == Convert.ToInt32(WHTFormNo.PND54))
                                    //{
                                    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx" + "?ID=" + CurrPK + "&APPTYPE=" + VoucherType +
                                    //      "&APPSUBTYPE=" + Convert.ToInt32(AppSubTypeVP.PND54) + "');", true);
                                    //}
                                    //else
                                    //{
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx" + "?ID=" + CurrPK + "&APPTYPE=" + VoucherType +
                                       "&APPSUBTYPE=" + Convert.ToInt32(AppSubTypeVP.WHTCERTIFICATE) + "');", true);
                                    //}
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_printwht").ToString()) + "','" + Resources.Messages.Information + "');", true);

                                }
                            }
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx" + "?ID=" + CurrPK + "&APPTYPE=" + ApplicationType.DPVJ +
                            //       "&APPSUBTYPE=" + Convert.ToInt32(AppSubTypeVP.WHTCERTIFICATE) + "');", true);
                        }
                        break;
                    #endregion
                    #region PRINT
                    case ActionsEnum.PRINT:
                        if (CurrPK > 0)
                        {
                            if (VoucherType == ApplicationType.DPVJ || VoucherType == ApplicationType.PCVJ || VoucherType == ApplicationType.YCV)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + VoucherType + "&TRXTYPE=" + VoucherType +
                                      "&APPSUBTYPE=0');", true);
                            }
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }

                        break;
                    #endregion
                    #region Cheque Print
                    case ActionsEnum.CHEQUEPRINT:
                        if (CurrPK > 0)
                        {
                            string printURL = string.Empty;
                            bool IsHaveCheque = false;
                            foreach (GridViewRow grdRow in grdVoucher.Rows)
                            {
                                HiddenField hdfFtrPk = (HiddenField)grdRow.FindControl("hdfDtlPK");
                                HiddenField hdfMode = (HiddenField)grdRow.FindControl("hdfMode");
                                if (hdfMode.Value == ((int)PaymentModeID.Cheque).ToString())
                                {
                                    IsHaveCheque = true;
                                    printURL += "../Reports/GenerateReport.aspx" + "?ID=" + CurrPK + "&APPTYPE=" + ApplicationType.DPVJ +
                                               "&APPSUBTYPE= 4 " + "&ChequeID=" + hdfFtrPk.Value + ",";
                                }
                            }
                            if (IsHaveCheque)
                            {
                                hdfPrintCheque.Value = printURL.Remove(printURL.Length - 1);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "PrintCheque();", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("chque_PrintError").ToString()) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region CHNAGETYPE WHTPOPUP
                    case ActionsEnum.WHTCHANGETYPE:
                        txtWthBranchCode.Text = string.Empty;
                        GetFieldValues(ControlsEnum.VENDORCONTACTFORWHT);
                        SetFieldValues(ControlsEnum.VENDORCONTACTFORWHT);
                        ShowWhtPopup();
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("WHTTaxDetails").ToString() + "','800','400');", true);
                        break;
                    #endregion

                    #region VATTAXHEADER
                    case ActionsEnum.VATTAXHEADER:
                        divVatErrorLabel.Visible = false;
                        lblVatSplitErrorMessage.Text = string.Empty;
                        txtVatRefundDate.Text = string.IsNullOrEmpty(txtVoucherDate.Text) ? string.Empty : Convert.ToDateTime(txtVoucherDate.Text).ToString(Resources.Constants.DateFormatMonthYear);

                        ViewState["VatDetRowIndex"] = null;
                        GetFieldValues(ControlsEnum.VENDOR);
                        SetFieldValues(ControlsEnum.VATBUYVENDOR);
                        SetFieldValues(ControlsEnum.VATPOPUPGRID);
                        GetUIValuesFromObject(ControlsEnum.TAXTYPECHANGED);

                        txtVendorPopup.Text = txtTo.Text;
                        hdfVendorPopup.Value = string.Empty;
                        hdfAddressType.Value = string.Empty;
                        GetFieldValues(ControlsEnum.VENDORSELECTEDDTL);
                        SetFieldValues(ControlsEnum.VENDORSELECTEDDTL);
                        GetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                        SetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                        ShowVatbuyPopup();
                        break;
                    #endregion
                    #region VAT ACCOUNT Changed
                    case ActionsEnum.VAT_ACCOUNT_INDEX_CHANGED_POPUP:
                        VatBuyTaxpk = !string.IsNullOrEmpty(hdfVATAccountPopup.Value) ? Convert.ToInt32(hdfVATAccountPopup.Value) : 0;
                        //if (chkVendorforpayemnt.Checked)
                        //{
                        GetFieldValues(ControlsEnum.VENDORACCOUNTVATBUYTAX);
                        //SetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
                        if (dtVendorAccount != null && dtVendorAccount.Rows.Count > 0)
                        {
                            decimal amount = 0;
                            amount = txtBeforeTaxAmount.Text != string.Empty ? Convert.ToDecimal(txtBeforeTaxAmount.Text) : 0;
                            string taxFormula = dtVendorAccount.Rows[0]["TAX_FORMULA"].ToString();
                            hdfTaxformula.Value = taxFormula;
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            decimal amt = Convert.ToDecimal(StringToFormula(taxFormula));
                            txtVATTaxAmountPopup.Text = ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(amt), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();// Math.Round(amt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //txtDescriptionPopup.Text = dtVendorAccount.Rows[0]["TAX_DESC"].ToString();
                            hdfWHTTaxCategory.Value = dtVendorAccount.Rows[0]["TAX_CATEGORY"].ToString();
                            hdfWHTTaxName.Value = dtVendorAccount.Rows[0]["TAX_HEAD"].ToString();
                        }
                        else
                        {
                            txtVATTaxAmountPopup.Text = Math.Round(Convert.ToDouble(0), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //txtDescriptionPopup.Text = string.Empty;
                        }
                        ShowVatbuyPopup();


                        break;
                    #endregion
                    #region VAT TAX ADD
                    case ActionsEnum.VATTAXADD:
                        bool errorVATAdd = false;
                        bool errorVATAmount = false;
                        int VatDetRowIndex = -1;
                        int? VendorAddressType = null;
                        int? VendorPopupPk = null;
                        if (ViewState["VatDetRowIndex"] != null) VatDetRowIndex = (int)(ViewState["VatDetRowIndex"]);
                        if (VatDetRowIndex < 0)
                            GetFieldValues(ControlsEnum.PAYMENTTAXHDR);

                        tempVATTaxDetails = null;
                        tempVATTaxDetails = TempVATTaxDetails;
                        divVatErrorLabel.Visible = false;
                        lblVatSplitErrorMessage.Text = string.Empty;
                        if (!string.IsNullOrEmpty(hdfAddressType.Value))
                        {
                            VendorAddressType = Convert.ToInt32(hdfAddressType.Value);
                        }
                        if (!string.IsNullOrEmpty(hdfVendorPopup.Value))
                        {
                            VendorPopupPk = Convert.ToInt32(hdfVendorPopup.Value);
                        }

                        if (finPymntList != null && finPymntList.Count > 0)
                        {
                            divVatErrorLabel.Visible = true;
                            lblVatSplitErrorMessage.Text = GetLocalResourceObject("TaxAlteadyExist").ToString();
                            ShowVatbuyPopup();
                            return;
                        }
                        if (tempVATTaxDetails != null && VatDetRowIndex >= 0)
                        {

                            tempVATTaxDetails[VatDetRowIndex].WTH_TAX = Convert.ToInt32(hdfVATAccountPopup.Value);
                            //tempVATTaxDetails[VatDetRowIndex].WTH_PK = 0;
                            //tempVATTaxDetails[VatDetRowIndex].WTH_PAYMENT_HDR = CurrPK;
                            tempVATTaxDetails[VatDetRowIndex].WTH_TRX_HDR = CurrPK;
                            tempVATTaxDetails[VatDetRowIndex].WTH_TYPE = 1;
                            tempVATTaxDetails[VatDetRowIndex].WTH_CATEGORY = (byte)WHTCategoryEnum.VATBUY;
                            tempVATTaxDetails[VatDetRowIndex].WTH_TAX_INV_NO = txtVatTaxInvNo.Text.Trim();
                            tempVATTaxDetails[VatDetRowIndex].WTH_VENDOR = VendorPopupPk;
                            tempVATTaxDetails[VatDetRowIndex].WTH_BRANCH = VendorAddressType;
                            tempVATTaxDetails[VatDetRowIndex].WTH_BRANCH_TEXT = HttpUtility.HtmlEncode(txtBranchCode.Text);
                            tempVATTaxDetails[VatDetRowIndex].WTH_BRANCH = VendorAddressType;
                            tempVATTaxDetails[VatDetRowIndex].WTH_BRANCH_NAME = HttpUtility.HtmlEncode(txtAddressType.Text);
                            tempVATTaxDetails[VatDetRowIndex].WTH_BRANCH_TYPE = chkHeadOffice.Checked ? (byte)VendorContactTypeEnum.HeadOffice : (byte)VendorContactTypeEnum.Branch;
                            if (hdfCRTNoAuto.Value == "0")
                                tempVATTaxDetails[VatDetRowIndex].WTH_CERT_NO = HttpUtility.HtmlEncode(txtCRTNo.Text);
                            tempVATTaxDetails[VatDetRowIndex].WTH_TAX_DATE = string.IsNullOrEmpty(txtVatTaxInvDate.Text.Trim()) ? DateTime.Now : DateTime.Parse(txtVatTaxInvDate.Text.Trim());
                            tempVATTaxDetails[VatDetRowIndex].WTH_REFUND_DATE = string.IsNullOrEmpty(txtVatRefundDate.Text.Trim()) ? DateTime.Now : DateTime.Parse(txtVatRefundDate.Text.Trim());
                            tempVATTaxDetails[VatDetRowIndex].WTH_TAX_CATEGORY = string.IsNullOrEmpty(hdfWHTTaxCategory.Value) ? Convert.ToByte(1) : Convert.ToByte(hdfWHTTaxCategory.Value);
                            tempVATTaxDetails[VatDetRowIndex].WTH_NAME = ddlVATAccountPopup.SelectedItem.Text;// hdfWHTTaxName.Value;
                            //tempVATTaxDetails[VatDetRowIndex].WTH_DESC = txtDescriptionPopup.Text;
                            //tempVATTaxDetails[VatDetRowIndex].WTH_FORM_NO = Convert.ToInt32(ddlFormno.SelectedValue);
                            tempVATTaxDetails[VatDetRowIndex].WTH_PARTY_NAME = HttpUtility.HtmlEncode(txtVendorPopup.Text);
                            //tempVATTaxDetails[VatDetRowIndex].WTH_ADDRESS = txtpartyads.Text;
                            tempVATTaxDetails[VatDetRowIndex].WTH_TAX_ID = txtVatTaxId.Text;
                            tempVATTaxDetails[VatDetRowIndex].WTH_ITEM_TEXT = txtMaterial.Text;
                            tempVATTaxDetails[VatDetRowIndex].WTH_AMOUNT = Convert.ToDecimal(txtBeforeTaxAmount.Text);
                            tempVATTaxDetails[VatDetRowIndex].WTH_TAX_AMT = Convert.ToDecimal(txtVATTaxAmountPopup.Text);
                            tempVATTaxDetails[VatDetRowIndex].WTH_INV_RECEIVED = chkOriginalinvoice.Checked ? (byte)1 : (byte)0;
                            TempVATTaxDetails = tempVATTaxDetails;
                            SetFieldValues(ControlsEnum.VATPOPUPGRID);
                            finVatPaymentDetails = null;
                            ViewState["VatDetRowIndex"] = null;

                            txtBeforeTaxAmount.Text = 0.ToString(hdfCurrencyFormat.Value);
                            txtVATTaxAmountPopup.Text = 0.ToString(hdfCurrencyFormat.Value);
                            txtVatTaxInvNo.Text = string.Empty;
                            txtVatTaxInvDate.Text = string.Empty;
                            txtMaterial.Text = string.Empty;
                            chkOriginalinvoice.Checked = false;
                        }
                        else
                        {
                            tempVATTax = null;
                            tempVATTax = tempVATTaxDetails.SingleOrDefault(tax => tax.WTH_TAX_DATE == DateTime.Parse(txtVatTaxInvDate.Text.Trim()) && tax.WTH_TAX_INV_NO == txtVatTaxInvNo.Text);
                            if (tempVATTax == null)
                            {
                                tempVATTax = new FIN_PAYMENT_VND_TAX_HDR();
                                tempVATTax = CommonFunctions.Initilize<FIN_PAYMENT_VND_TAX_HDR>();
                                //try
                                //{
                                tempVATTax.WTH_AMOUNT = string.IsNullOrEmpty(txtBeforeTaxAmount.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtBeforeTaxAmount.Text);
                                tempVATTax.WTH_TAX_AMT = string.IsNullOrEmpty(txtVATTaxAmountPopup.Text) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtVATTaxAmountPopup.Text);
                                //}
                                //catch
                                //{
                                //    errorVATAmount = true;
                                //}
                                if (!errorVATAmount)
                                {
                                    tempVATTax.WTH_TAX = Convert.ToInt32(hdfVATAccountPopup.Value);
                                    tempVATTax.WTH_PK = 0;
                                    //tempVATTax.WTH_PAYMENT_HDR = CurrPK;
                                    tempVATTax.WTH_TRX_HDR = CurrPK;
                                    tempVATTax.WTH_TYPE = 1;
                                    tempVATTax.WTH_CATEGORY = (byte)WHTCategoryEnum.VATBUY;
                                    tempVATTax.WTH_TAX_INV_NO = txtVatTaxInvNo.Text.Trim();
                                    tempVATTax.WTH_VENDOR = VendorPopupPk;
                                    tempVATTax.WTH_BRANCH_TEXT = HttpUtility.HtmlEncode(txtBranchCode.Text);
                                    tempVATTax.WTH_BRANCH = VendorAddressType;
                                    tempVATTax.WTH_BRANCH_NAME = HttpUtility.HtmlEncode(txtAddressType.Text);
                                    tempVATTax.WTH_BRANCH_TYPE = chkHeadOffice.Checked ? (byte)VendorContactTypeEnum.HeadOffice : (byte)VendorContactTypeEnum.Branch;

                                    tempVATTax.WTH_TAX_DATE = string.IsNullOrEmpty(txtVatTaxInvDate.Text.Trim()) ? DateTime.Now : DateTime.Parse(txtVatTaxInvDate.Text.Trim());
                                    tempVATTax.WTH_REFUND_DATE = string.IsNullOrEmpty(txtVatRefundDate.Text.Trim()) ? DateTime.Now : DateTime.Parse(txtVatRefundDate.Text.Trim());
                                    tempVATTax.WTH_TAX_CATEGORY = string.IsNullOrEmpty(hdfWHTTaxCategory.Value) ? Convert.ToByte(1) : Convert.ToByte(hdfWHTTaxCategory.Value);
                                    tempVATTax.WTH_NAME = ddlVATAccountPopup.SelectedItem.Text;// hdfWHTTaxName.Value;
                                    //tempVATTax.WTH_DESC = txtDescriptionPopup.Text;
                                    tempVATTax.WTH_FORM_NO = null;
                                    tempVATTax.WTH_PARTY_NAME = HttpUtility.HtmlEncode(txtVendorPopup.Text);
                                    //tempVATTax.WTH_ADDRESS = txtpartyads.Text;
                                    tempVATTax.WTH_TAX_ID = txtVatTaxId.Text;
                                    tempVATTax.WTH_ITEM_TEXT = txtMaterial.Text;
                                    tempVATTax.WTH_AMOUNT = Convert.ToDecimal(txtBeforeTaxAmount.Text);
                                    tempVATTax.WTH_TAX_AMT = Convert.ToDecimal(txtVATTaxAmountPopup.Text);
                                    tempVATTax.WTH_INV_RECEIVED = chkOriginalinvoice.Checked ? (byte)1 : (byte)0;
                                    tempVATTaxDetails.Add(tempVATTax);
                                    TempVATTaxDetails = tempVATTaxDetails;
                                    SetFieldValues(ControlsEnum.VATPOPUPGRID);
                                    finVatPaymentDetails = null;
                                    ViewState["VatDetRowIndex"] = null;
                                }
                            }
                            else
                            {
                                errorVATAdd = true;
                            }
                            if (!errorVATAdd && !errorVATAmount)
                            {
                                txtBeforeTaxAmount.Text = 0.ToString(hdfCurrencyFormat.Value);
                                txtVATTaxAmountPopup.Text = 0.ToString(hdfCurrencyFormat.Value);
                                txtVatTaxInvNo.Text = string.Empty;
                                txtVatTaxInvDate.Text = string.Empty;
                                txtMaterial.Text = string.Empty;
                                chkOriginalinvoice.Checked = false;
                            }
                        }

                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CloseMsgPopup1", "CloseMsgPopup();", true);
                        ShowVatbuyPopup();

                        if (errorVATAdd)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Add").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (errorVATAmount)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Amount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region VATTAXDELETE
                    case ActionsEnum.VATTAXDELETE:
                        tempVATTaxDetails = null;
                        if (TempVATTaxDetails != null)
                        {
                            tempVATTax = null;
                            tempVATTaxDetails = TempVATTaxDetails;
                            HiddenField hdfTaxPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfVATTaxPK") as HiddenField);
                            HiddenField hdfTaxName = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfVATTaxName") as HiddenField);
                            Label lblVatTaxInvNo = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("lblVatTaxInvNo") as Label);
                            HiddenField hdfTaxDate = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxDate") as HiddenField);
                            if (hdfTaxPK != null)
                            {
                                int taxPK = string.IsNullOrEmpty(hdfTaxPK.Value) ? 0 : Convert.ToInt32(hdfTaxPK.Value);
                                if (taxPK > 0)
                                {
                                    tempVATTax = tempVATTaxDetails.SingleOrDefault(rfq => rfq.WTH_PK == taxPK);// && rfq.WTH_TAX_CATEGORY == Convert.ToInt32(hdfVATTaxCategory.Value));
                                }
                                else
                                {
                                    if (hdfTaxName != null)
                                    {
                                        tempVATTax = tempVATTaxDetails.SingleOrDefault(rfq => rfq.WTH_TAX_DATE == Convert.ToDateTime(hdfTaxDate.Value) && rfq.WTH_TAX_INV_NO == lblVatTaxInvNo.Text);// && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                }
                                if (tempVATTax != null)
                                {
                                    tempVATTaxDetails.Remove(tempVATTax);
                                    TempVATTaxDetails = tempVATTaxDetails;
                                }
                            }
                            SetFieldValues(ControlsEnum.VATPOPUPGRID);
                        }
                        ShowVatbuyPopup();
                        break;
                    #endregion
                    #region VATTAXAPPLY
                    case ActionsEnum.VATTAXAPPLY:
                        tempVATTaxDetails = null;
                        divVatErrorLabel.Visible = false;
                        if (TempVATTaxDetails != null && TempVATTaxDetails.Count > 0)
                        {
                            tempVATTaxDetails = VATTaxDetails = TempVATTaxDetails;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            //withHoldTax = Convert.ToDecimal(tempWHTTaxDetails.Sum(aa => aa.WTH_TAX_AMT));
                            //decimal.TryParse(hdfWithHoldTax.Value, out whtTax);
                            //if (withHoldTax > whtTax)
                            //{
                            //    divErrorLabel.Visible = true;
                            //    lblSplitErrorMessage.Text = GetLocalResourceObject("Err_WHTAmntTotalSplit").ToString() + whtTax.ToString();
                            //    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_WHTAmntTotalSplit").ToString() + whtTax.ToString()) + "');", true);
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            //    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideOverlay", "HideOverlay();", true);
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("WHTTaxDetails").ToString() + "','800','400');", true);
                            //}
                            //else
                            //{
                            //    tempWHTTaxDetails = null;
                            //}
                        }
                        else
                        {
                            ShowVatbuyPopup();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_VatTaxSave").ToString()) + "','" + Resources.Messages.Information + "');", true);
                        }
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;

                    #endregion
                    #region VATTAXTYPECHANGED
                    case ActionsEnum.TAXTYPECHANGED:
                        GetUIValuesFromObject(ControlsEnum.TAXTYPECHANGED);
                        ShowVatbuyPopup();
                        break;
                    #endregion
                    #region CHANGETYPE
                    case ActionsEnum.CHANGETYPE:
                        SetBranchCodeVisibility();
                        ShowVatbuyPopup();
                        break;
                    #endregion
                    #region Edit VAT Item From Grid
                    case ActionsEnum.POPUPGRIDEDIT:
                        GridViewRow grwVatDetails = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        //GetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                        //SetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                        SetUIEditViewVatPopup(grwVatDetails);
                        ShowVatbuyPopup();
                        break;
                    #endregion
                    #region VENDORSELECTEDDTL
                    case ActionsEnum.VENDORSELECTEDDTL:
                        GetFieldValues(ControlsEnum.VENDORSELECTEDDTL);
                        SetFieldValues(ControlsEnum.VENDORSELECTEDDTL);
                        GetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                        SetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                        SetBranchCodeVisibility();
                        ShowVatbuyPopup();
                        break;
                    #endregion
                    #region VATTAXSAVE
                    case ActionsEnum.VATTAXSAVE:

                        poPaymentServiceClient = new POPaymentService();
                        poPaymentServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(poPaymentServiceClient);
                        //tempVATTaxDetails = null;
                        divVatErrorLabel.Visible = false;
                        if (grdVATTaxDetails.Rows.Count > 0)
                        {
                            //if (TempVATTaxDetails != null && TempVATTaxDetails.Count > 0)
                            //{

                            tempVATTaxDetails = null;
                            if (TempVATTaxDetails != null && TempVATTaxDetails.Count > 0)
                            {
                                tempVATTaxDetails = VATTaxDetails = TempVATTaxDetails;
                                VatBuyTax = Convert.ToDecimal(CommonFunctions.DoubleFormatRound(Convert.ToDouble(tempVATTaxDetails.Sum(aa => aa.WTH_TAX_AMT)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                                decimal.TryParse(hdfVatBuy.Value, out vatTax);
                                if (vatTax != VatBuyTax && vatTax > 0 && (hdfIscontYesVat.Value != "1"))
                                {
                                    hdfAmntMissmatch.Value = GetLocalResourceObject("Err_VatAmntNotTallied").ToString();
                                    showPopup = true;
                                }
                            }
                            else
                            {
                                hdfAmntMissmatch.Value = GetLocalResourceObject("Err_VatAmntNotEntered").ToString();
                                showPopup = true;
                            }

                            //hdfAmntMissmatch.Value = hdfAmntMissmatch.Value.TrimEnd(',') + GetLocalResourceObject("Err_nottalliedcontinue").ToString();
                            if ((hdfIscontYesVat.Value != "1") && showPopup)
                            {
                                ShowVatbuyPopup();
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_WHTMissmatch").ToString()) + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlreadyPaid", "$(document).ready(function(){VatAmtMismatch();});", true);
                                return;
                            }
                            hdfIscontYesVat.Value = "0";
                            vatTaxresult = poPaymentServiceClient.SaveWhtTaxDirectForDirectPayment(tempVATTaxDetails);
                            if (vatTaxresult >= 0)
                            {
                                litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), GetLocalResourceObject("Tax").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            }

                            //}
                        }
                        else
                        {
                            try
                            {
                                tempVATTaxDetails = VATTaxDetails = TempVATTaxDetails;
                            }
                            catch { }
                            hdfIscontYesVat.Value = "0";
                            long? Saveresult = poPaymentServiceClient.DeleteDirectVatTaxForDirectPayment(Convert.ToInt64(CurrPK), (byte)WHTCategoryEnum.VATBUY);
                            if (Saveresult.HasValue && Saveresult >= 0)
                            {
                                litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), GetLocalResourceObject("Tax").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            }
                            //ShowVatbuyPopup();
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_VatTaxSave").ToString()) + "','" + Resources.Messages.Information + "');", true);
                        }

                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);

                        break;
                    #endregion
                    #region VENDORTEXTCHANGED
                    case ActionsEnum.VENDORTEXTCHANGED:
                        hdfAddressType.Value = string.Empty;
                        hdfVendorPopup.Value = string.Empty;
                        txtBranchCode.Enabled = true;
                        vrfBranchCode.Enabled = true;
                        chkHeadOffice.Checked = false;
                        txtBranchCode.CssClass = "";
                        SetBranchCodeVisibility();
                        ShowVatbuyPopup();
                        break;
                    #endregion
                    #region VENDORCONTACTTEXTCHANGED
                    case ActionsEnum.VENDORCONTACTTEXTCHANGED:
                        //hdfAddressType.Value = string.Empty;                       
                        txtBranchCode.Enabled = true;
                        vrfBranchCode.Enabled = true;
                        chkHeadOffice.Checked = false;
                        txtBranchCode.CssClass = "";
                        SetBranchCodeVisibility();
                        ShowVatbuyPopup();
                        break;
                    #endregion
                    #region CHECKEDCHANGED
                    case ActionsEnum.CHECKEDCHANGED:
                        HeadofficeCheckedChanged();
                        ShowVatbuyPopup();
                        break;
                    #endregion


                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
                        finPaymentVndHdrObj = CommonFunctions.Initilize<FIN_PAYMENT_VND_HDR>();
                        finPaymentVndHdrObj.PVH_PK = CurrPK;
                        GetFieldValues(ControlsEnum.VOUCHERS);
                        SetUIValuesToObject(ActionsEnum.JOURNALIZE, ControlsEnum.FINTRXHDR);
                        break;
                    #endregion
                    #region Journalize Update
                    case ActionsEnum.JOURNALIZEUPDATE:
                        ucrJournalize.ResetForm();
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(VoucherType, 0);
                        ResetForm(1);
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        EntryStatus = EntryStatus.LISTMODE;
                        RedirectToListPage();
                        break;
                    #endregion
                    #region Journalize Save
                    case ActionsEnum.REVERSESAVE:
                    case ActionsEnum.JOURNALIZESAVE:
                    case ActionsEnum.RETURNSAVE:
                        EntryStatus = EntryStatus.LISTMODE;
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(VoucherType, 0);
                        ResetForm(1);
                        //RedirectToListPage();

                        //litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DirectPayment);
                        //                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                        //                            Page.ResolveClientUrl(Resources.PageURL.DirectPaymentList) + "');", true);
                        break;
                    #endregion
                    #region Journalize Submit
                    case ActionsEnum.JOURNALIZESUBMIT:
                        ////if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        ////{
                        ////    string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                        ////    if (Transaction == "SAVE")
                        ////    {
                        ////        poPaymentServiceClient = new POPaymentService();
                        ////        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        ////        result = poPaymentServiceClient.UpdatePaymentHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);

                        ////    }
                        ////    else if (Transaction == "DELETE")
                        ////    {
                        ////        poPaymentServiceClient = new POPaymentService();
                        ////        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        ////        result = poPaymentServiceClient.UpdatePaymentHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                        ////    }
                        ////}
                        EntryStatus = EntryStatus.LISTMODE;
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(VoucherType, 0);
                        ResetForm(1);
                        //RedirectToListPage();
                        break;
                    #endregion
                    #region Journalize Delete
                    case ActionsEnum.JOURNALIZEDELETE:
                        //if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        //{
                        //    string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                        //    if (Transaction == "SAVE")
                        //    {
                        //        poPaymentServiceClient = new POPaymentService();
                        //        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        //        result = poPaymentServiceClient.UpdatePaymentHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);

                        //    }
                        //    else if (Transaction == "DELETE")
                        //    {
                        //        poPaymentServiceClient = new POPaymentService();
                        //        poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                        //        result = poPaymentServiceClient.UpdatePaymentHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                        //    }
                        //}
                        EntryStatus = EntryStatus.LISTMODE;
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(VoucherType, 0);
                        ResetForm(1);
                        //RedirectToListPage();
                        break;
                    #endregion
                    #region Journalize Cancel
                    case ActionsEnum.REVERSECANCEL:
                    case ActionsEnum.JOURNALIZECANCEL:
                    case ActionsEnum.RETURNCANCEL:
                        EntryStatus = EntryStatus.LISTMODE;
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(VoucherType, 0);
                        ResetForm(1);
                        RedirectToListPage();
                        break;
                    #endregion
                    #region REVERSE Submit
                    case ActionsEnum.REVERSESUBMIT:
                    case ActionsEnum.RETURNSUBMIT:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(VoucherType, 0);
                        ResetForm(1);
                        EntryStatus = EntryStatus.LISTMODE;
                        //RedirectToListPage();
                        break;
                    #endregion
                    #region REVERSE Delete
                    case ActionsEnum.REVERSEDELETE:
                    case ActionsEnum.RETURNDELETE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(VoucherType, 0);
                        ResetForm(1);
                        EntryStatus = EntryStatus.LISTMODE;
                        //RedirectToListPage();
                        break;
                    #endregion

                    #region CHEQUERETURN
                    case ActionsEnum.CHEQUERETURN:
                        GetFieldValues(ControlsEnum.VOUCHERS);
                        if (finTrxHdrList != null && finTrxHdrList.Count >= 1)
                        {
                            if (finTrxHdrList[0].FTH_PDC != 1)
                            {
                                if (finTrxHdrList[0].FTH_IS_JRNLD)
                                {

                                    #region Check whether the cheque return is possible or not
                                    bool IsReturnSuccess = true;
                                    FIN_TRX objTrxLst = finTrxHdrList[0].FIN_TRX.Where(r => Convert.ToInt32(r.ADM_CONFIG_MST.CFG_VALUE) == (int)PaymentModeEnum.Cheque).FirstOrDefault();
                                    if (objTrxLst != null)
                                    {
                                        bool IsDebit = objTrxLst.FTR_DR_AMT_BC > 0 ? true : false;
                                        if (finTrxHdrList[0].FIN_TRX.Where(r => Convert.ToInt32(r.ADM_CONFIG_MST.CFG_VALUE) != (int)PaymentModeEnum.Cheque && (IsDebit ? r.FTR_DR_AMT_BC > 0 : r.FTR_CR_AMT_BC > 0)).Count() > 0)
                                        {
                                            IsReturnSuccess = false;
                                        }
                                        else if (finTrxHdrList[0].FTH_PDC > 0)
                                        {
                                            if (finTrxHdrList[0].FIN_TRX.Where(r => r.FTR_PDC == 0 && (IsDebit ? r.FTR_DR_AMT_BC > 0 : r.FTR_CR_AMT_BC > 0)).Count() > 0)
                                                IsReturnSuccess = false;
                                        }
                                        if (!IsReturnSuccess)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("MsgErr_ReturnEntry_Multi_Mode").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            return;
                                        }
                                    }
                                    #endregion

                                    if (finTrxHdrList[0].FTH_BOUNCED == 0) //commented for multiple cheque finPaymentVndHdrList[0].PVH_BOUNCED == 0
                                    {
                                        FinTrxService finTrxServiceClient;
                                        finTrxServiceClient = new FinTrxService();
                                        finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                        result = finTrxServiceClient.GenerateDummyEntry((int)CurrPK, ApplicationType.DPBJ);
                                        finTrxServiceClient = null;
                                        //if (result > 0 || result == -2)  // -2 already exist
                                        //{
                                        //    poPaymentServiceClient = new POPaymentService();
                                        //    poPaymentServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(poPaymentServiceClient);
                                        //    result = poPaymentServiceClient.UpdatePaymentHdrBounceFlag((int)CurrPK, 1);
                                        //}
                                        //else
                                        //{
                                        //    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_ReturnEntry").ToString();
                                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        //    break;
                                        //}
                                    }
                                    SetUIValuesToObject(ActionsEnum.CHEQUERETURN, ControlsEnum.FINTRXHDR);
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
                        break;
                    #endregion

                    #region REVERSE
                    case ActionsEnum.REVERSE:
                        GetFieldValues(ControlsEnum.VOUCHERS);
                        if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                        {
                            if (finTrxHdrList[0].FTH_PDC >= 1)
                            {
                                if (finTrxHdrList[0].FTH_PDC == 1)
                                {
                                    FinTrxService finTrxServiceClient;
                                    finTrxServiceClient = new FinTrxService();
                                    finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                    result = finTrxServiceClient.GenerateDummyEntry((int)CurrPK, ApplicationType.DPVCJ);
                                    finTrxServiceClient = null;
                                }
                                SetUIValuesToObject(ActionsEnum.REVERSE, ControlsEnum.FINTRXHDR);
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
                    #region LOAD FROM TEMPLATE
                    case ActionsEnum.LOADFROMTEMPLATE:
                        string VoucherDate = txtVoucherDate.Text;
                        string VoucherNo = txtVoucherNo.Text;
                        string RefNo = txtRefNo.Text;
                        string RefDate = txtRefDate.Text;
                        string Remarks = txtRemarks.Text;
                        CurrPK = BusinessLogic.Finance.YearEndVoucherBL.YearClosingSave(DateTime.Parse(txtVoucherDate.Text), currentUser, int.Parse(ddlCompany.SelectedValue));
                        if (CurrPK > 0)
                        {
                            finTrxHdrObj = CommonFunctions.Initilize<FIN_TRX_HDR>();
                            finTrxHdrObj.FTH_PK = CurrPK;
                            GetUIValuesFromObject(ControlsEnum.VOUCHERS);
                            GetFieldValues(ControlsEnum.APLNTYPE);
                            SetFieldValues(ControlsEnum.APLNTYPE);
                            GetFieldValues(ControlsEnum.BASECURRENCY);
                            GetFieldValues(ControlsEnum.EXCHANGERATE);
                            GetFieldValues(ControlsEnum.MODE);
                            SetFieldValues(ControlsEnum.MODE);
                            GetFieldValues(ControlsEnum.VOUCHERS);
                            SetFieldValues(ControlsEnum.VOUCHERS);
                            txtVoucherDate.Text = VoucherDate;
                            txtVoucherNo.Text = VoucherNo;
                            txtRefNo.Text = RefNo;
                            txtRefDate.Text = RefDate;
                            txtRemarks.Text = Remarks;
                            //GetFieldValues(ControlsEnum.WHTVENDOR);
                            //SetFieldValues(ControlsEnum.WHTVENDOR);
                        }
                        else if (CurrPK == (int)DbSaveStatus.CODEEXIST)
                        {
                            //litErrorMsg.Text = Resources.PageNameRes.YearClosing + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("YCV_AlreadyCreated").ToString())
                            + "','" + Resources.ErpRes.Information + "','" + "');", true);
                        }
                        else if (CurrPK == (int)DbSaveStatus.PENDINGEXIST)
                        {
                            //litErrorMsg.Text = Resources.PageNameRes.YearClosing + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("YCV_PendingExist").ToString())
                            + "','" + Resources.ErpRes.Information + "','" + "');", true);
                        }
                        break;
                    #endregion
                    #region COST CENTER(Show Cost Center BreakUp Popup)
                    case ActionsEnum.COSTCENTER:
                        selAccountPk = 0;
                        if (hdfAccount1 != null)
                        {
                            int.TryParse(hdfAccount1.Value, out selAccountPk);
                            hdfPreviousAccount1Pk.Value = selAccountPk.ToString();
                            hdfPreviousAccount1Name.Value = txtAccount1.Text;
                        }
                        if (selAccountPk > 0)
                        {
                            decimal debitAmount = 0, creditAmount = 0;
                            decimal.TryParse(txtDebit1.Text, out debitAmount);
                            decimal.TryParse(txtCredit1.Text, out creditAmount);
                            CCAmount = debitAmount > 0 ? debitAmount : creditAmount;
                            if (CostCenterTempList == null)
                            {
                                CostCenterTempList = new List<CostCenterDetails>();
                            }
                            if (CurrFtrPK == 0)
                            {
                                #region New Mode
                                if (CostCenterList != null && CostCenterList.Count > 0)//CC allocation applied(Apply btn clicks),but not added (Add button not clicks).This time popup should show applied data.
                                {
                                    foreach (CostCenterDetails item in CostCenterList)
                                    {
                                        CostCenterTempList.Add(item);
                                    }
                                }
                                else
                                {
                                    #region New
                                    GetFieldValues(ControlsEnum.COSTCENTER);
                                    if (dtCostCenter != null && dtCostCenter.Rows.Count > 0)
                                    {
                                        foreach (DataRow drRow in dtCostCenter.Rows)
                                        {
                                            CostCenterDetails objCCDtls = new CostCenterDetails();
                                            objCCDtls.FCM_CNM_PK = Convert.ToInt32(drRow["FCM_CNM_PK"]);
                                            objCCDtls.FCM_COST_CENTER_TEXT = CommonFunctions.GetEncodedString(drRow["FCM_COST_CENTER_TEXT"]);
                                            objCCDtls.FTD_AMT_BC = Convert.ToDecimal(drRow["FCM_AMOUNT"]);
                                            CostCenterTempList.Add(objCCDtls);
                                        }
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            else
                            {
                                #region Edit Mode
                                finTrxList = (List<FIN_TRX>)(Session["VoucherDet"]);
                                finTrxList = (from ftrList in finTrxList
                                              where ftrList.FTR_PK == CurrFtrPK
                                              select ftrList).ToList();
                                if (finTrxList[0].FTR_ACCOUNT.ToString() == hdfAccount1.Value && finTrxList[0].FIN_TRX_COC_DTL != null && finTrxList[0].FIN_TRX_COC_DTL.Count > 0)//Selected Account Changed
                                {
                                    if (CostCenterList != null && CostCenterList.Count > 0)//CC allocation applied(Apply btn clicks),but not added (Add button not clicks).This time popup should show applied data.
                                    {
                                        foreach (CostCenterDetails item in CostCenterList)
                                        {
                                            CostCenterTempList.Add(item);
                                        }
                                    }
                                    else
                                    {
                                        #region Get From Added List
                                        GetFieldValues(ControlsEnum.COSTCENTER);
                                        foreach (FIN_TRX_COC_DTL objTrxCoc in finTrxList[0].FIN_TRX_COC_DTL)
                                        {
                                            CostCenterDetails objCCDtls = new CostCenterDetails();
                                            objCCDtls.FCM_CNM_PK = objTrxCoc.FTD_CNM_PK;
                                            List<string> lstResult = (from table in dtCostCenter.AsEnumerable()
                                                                      where table.Field<int>("FCM_CNM_PK") == objTrxCoc.FTD_CNM_PK
                                                                      select table.Field<string>("FCM_COST_CENTER_TEXT")).ToList();
                                            objCCDtls.FCM_COST_CENTER_TEXT = lstResult[0];
                                            objCCDtls.FTD_AMT_BC = objTrxCoc.FTD_AMT_BC;
                                            CostCenterTempList.Add(objCCDtls);
                                        }

                                        #endregion
                                    }
                                }
                                else
                                {
                                    if (CostCenterList != null && CostCenterList.Count > 0)//CC allocation applied(Apply btn clicks),but not added.This time popup should show applied data.
                                    {
                                        foreach (CostCenterDetails item in CostCenterList)
                                        {
                                            CostCenterTempList.Add(item);
                                        }
                                    }
                                    else
                                    {
                                        #region New
                                        GetFieldValues(ControlsEnum.COSTCENTER);
                                        if (dtCostCenter != null && dtCostCenter.Rows.Count > 0)
                                        {
                                            foreach (DataRow drRow in dtCostCenter.Rows)
                                            {
                                                CostCenterDetails objCCDtls = new CostCenterDetails();
                                                objCCDtls.FCM_CNM_PK = Convert.ToInt32(drRow["FCM_CNM_PK"]);
                                                objCCDtls.FCM_COST_CENTER_TEXT = CommonFunctions.GetEncodedString(drRow["FCM_COST_CENTER_TEXT"]);
                                                objCCDtls.FTD_AMT_BC = Convert.ToDecimal(drRow["FCM_AMOUNT"]);
                                                CostCenterTempList.Add(objCCDtls);
                                            }
                                        }
                                        #endregion
                                    }
                                }
                                #endregion
                            }
                            SetFieldValues(ControlsEnum.COSTCENTER);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalCCAmount", "$(document).ready(function(){CalculateTotalCCAmount();});", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divVoucherCC]','" + GetLocalResourceObject("CostCenterBreakup").ToString() + "','450','350');", true);
                        }
                        break;
                    #endregion
                    #region COST CENTER APPLY
                    case ActionsEnum.COSTCENTERAPPLY:
                        if (grdVoucherCC.Rows.Count > 0)
                        {
                            CostCenterTempList = new List<CostCenterDetails>();
                            foreach (GridViewRow grvRow in grdVoucherCC.Rows)
                            {
                                CCAmount = 0;
                                HiddenField hdfVoucherCCPk = (HiddenField)grvRow.FindControl("hdfVoucherCCPk");
                                Label lblVoucherCC = (Label)grvRow.FindControl("lblVoucherCC");
                                TextBox txtVoucherCCAmountTC = (TextBox)grvRow.FindControl("txtVoucherCCAmountTC");
                                decimal.TryParse(txtVoucherCCAmountTC.Text, out CCAmount);

                                CostCenterDetails objCCDtls = new CostCenterDetails();
                                objCCDtls.FCM_CNM_PK = Convert.ToInt32(hdfVoucherCCPk.Value);
                                objCCDtls.FCM_COST_CENTER_TEXT = CommonFunctions.GetEncodedString(lblVoucherCC.ToolTip);
                                objCCDtls.FTD_AMT_BC = CCAmount;
                                CostCenterTempList.Add(objCCDtls);
                            }
                            if (IsValidCostCenterSplitAllocation(CostCenterTempList)) // Validation for cost center split missmatch    
                            {
                                CostCenterList = CostCenterTempList;
                                hdfIsContCCAllocDeletion.Value = "0";
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalCCAmount", "$(document).ready(function(){CalculateTotalCCAmount();});", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divVoucherCC]','" + GetLocalResourceObject("CostCenterBreakup").ToString() + "','450','350');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_CC_SplitMissmatch").ToString()) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        }
                        break;
                    #endregion
                    #region COST CENTER CANCEL
                    case ActionsEnum.COSTCENTERCANCEL:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #endregion
                    #region AUDIT LOG
                    case ActionsEnum.AUDITLOG:
                        //for (int i = 0; i < grdAuditLog.Rows.Count; i++)
                        //{
                        //    grdAuditLog.Rows[i].BackColor = System.Drawing.Color.White;
                        //}
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ResetGridRowColor", "ResetGridRowColor();", true);
                        ucrAuditLogList.ResetGrivRowColor();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divJournalAuditLog]','" + GetLocalResourceObject("EditLog").ToString() + "','605','300');", true);
                        break;
                    #endregion
                    #region COMPARE
                    case ActionsEnum.COMPARE:
                        ucrAuditLogList.CurrentSelectedRowColor(Convert.ToInt32(hdfSelRow.Value));
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ResetSelectedRowColor", "ResetSelectedRowColor('" + hdfSelRow.Value + "');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "RedirectToComparisonPage", "RedirectToComparisonPage();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divJournalAuditLog]','" + GetLocalResourceObject("EditLog").ToString() + "','605','300');", true);
                       
                        //for (int i = 0; i < grdAuditLog.Rows.Count; i++)
                        //{
                        //    int rowIndex = Convert.ToInt32(hdfSelRow.Value);
                        //    if (i == rowIndex)
                        //    {
                        //        grdAuditLog.Rows[i].BackColor = System.Drawing.Color.Yellow;
                        //    }
                        //    else
                        //    {
                        //        grdAuditLog.Rows[i].BackColor = System.Drawing.Color.White;

                        //    }
                        //}
                        break;
                        #endregion

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }

            finally
            {
                finTrxHdrObj = null;
                finTrxHdrList = null;
                finTrxObj = null;
                finTrxList = null;
                finCoaMstObj = null;
                finCoaMstList = null;
                admConfigMstObj = null;
                FinTrxServiceClient = null;

                FinCoaMstServiceClient = null;
                CommonServiceClient = null;
                poPaymentServiceClient = null;
            }
        }

        private void AddCashAccountCR()
        {
            string relquery = string.Empty;
            finTrxList = Session["VoucherDet"] == null ? new List<FIN_TRX>() : finTrxList = (List<FIN_TRX>)(Session["VoucherDet"]);
            if (finTrxList.Count(f => f.FTR_ACCOUNT == Convert.ToInt32(ddlCashAccount.SelectedValue)) == 0)
            {
                ddlAccountType = Convert.ToInt32(ddlCashAccount.SelectedValue); ;
                txtAccount1.Text = ddlCashAccount.SelectedItem.Text;
                hdfAccount1.Value = ddlCashAccount.SelectedValue;
                GetFieldValues(ControlsEnum.FINCOAMST);
                if (finCoaMstList != null && finCoaMstList.Count > 0)
                {
                    ddlAccountSubType = Convert.ToInt32(finCoaMstList[0].COA_SUB_TYPE.ToString());
                    GetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                    if (finCoaSubTypeCfgList[0].CST_REL_QUERY != null)
                    {
                        if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                        {
                            relquery = finCoaSubTypeCfgList[0].CST_REL_QUERY == null ? string.Empty : finCoaSubTypeCfgList[0].ADM_QUERIES_CFG1.QRY_QUERY;
                        }

                        if (relquery != string.Empty)
                        {
                            CommonServiceClient = new CommonService();
                            CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                            relquery = relquery.Replace("@PK@", "NULL");
                            SubTypesAccounts = CommonServiceClient.ExecuteQuery(relquery);
                            SetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                            ddlSubTypeAccount1.Visible = true;

                            #region Sub Type Selected Value
                            string defQuery = finCoaSubTypeCfgList[0].CST_DEF_QUERY == null ? string.Empty : finCoaSubTypeCfgList[0].ADM_QUERIES_CFG_DEF_QUERY.QRY_QUERY;
                            if (defQuery != string.Empty)//Have default Query
                            {
                                defQuery = defQuery.Contains(GetLocalResourceObject("DefaultQryCondtion").ToString())
                                    ? defQuery.Replace(GetLocalResourceObject("DefaultQryCondtion").ToString(), ddlAccountType.ToString()) : defQuery;
                                CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                                List<DDLMaster> defaultValuesList = CommonServiceClient.ExecuteQuery(defQuery);
                                if (defaultValuesList != null && defaultValuesList.Count == 1)
                                {
                                    ddlSubTypeAccount1.SelectedValue = (ListItem)ddlSubTypeAccount1.Items.FindByValue(defaultValuesList[0].PK.ToString()) != null
                                        ? defaultValuesList[0].PK.ToString() : CommonConstants.SELECTVAL;//Set default value to subledger ddl
                                }
                                else
                                {
                                    // ddlSubTypeAccount1.SelectedValue =  CommonConstants.SELECTVAL;
                                }
                            }
                            else
                            {
                                // ddlSubTypeAccount1.SelectedValue = CommonConstants.SELECTVAL;
                            }

                            #endregion

                        }
                        else
                        {
                            ddlSubTypeAccount1.Visible = false;
                            ddlSubTypeAccount.Visible = false;
                        }
                    }
                }
                // if (finTrxList.Count(s => s.FTR_ACCOUNT == Convert.ToInt32(ddlCashAccount.SelectedValue)) > 0)
                txtCredit1.Text = (finTrxList.Sum(s => s.FTR_DR_AMT_BC) - finTrxList.Where(i => i.FTR_ACCOUNT != Convert.ToInt32(ddlCashAccount.SelectedValue)).Sum(s => s.FTR_CR_AMT_BC)).ToString();
                txtNarration1.Text = "To " + finTrxList[0].FIN_COA_MST.COA_NAME + " " + finTrxList[0].FTR_NARRATION;
                ImageButton btnNew = new ImageButton();
                btnNew.CommandName = "ADDLITEM";
                btnNew.CommandArgument = "SaveAction";
                btnNew.OnClientClick = "ActionHandler";
                ActionHandler(btnNew, EventArgs.Empty);
            }

        }
        private void RedirectToListPage()
        {
            if (VoucherType == ApplicationType.OBV)
            {
                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.OpeningBalanceList), false);
            }
            else if (VoucherType == ApplicationType.DPVJ)
            {
                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.DirectPaymentList), false);
            }
            else if (VoucherType == ApplicationType.PCVJ)
            {
                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.PettyCashList), false);
            }
            //Contra Entry
            else if (VoucherType == ApplicationType.CTVJ)
            {
                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ContraEntryList), false);
            }
            else if (VoucherType == ApplicationType.YCV)
            {
                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.YearClosingVoucherList), false);
            }
        }

        private void HeadofficeCheckedChanged()
        {
            if (chkHeadOffice.Checked)
            {
                //txtBranchCode.Enabled = false;
                //txtBranchCode.Text = "";
                vrfBranchCode.Enabled = false;
                //txtBranchCode.CssClass = "input-disabled";
            }
            else
            {
                txtBranchCode.Enabled = true;
                vrfBranchCode.Enabled = true;
                txtBranchCode.CssClass = "";
            }
        }
        #region Workflow Submit
        /// <summary>
        /// Save and submit With workflow 
        /// </summary>
        /// <param name="objSalPayment"></param>
        private void SaveTransaction(FinTrxHeaderBO objFinTrxHeaderBO, int workflowFlag, int IsCancel = 0)
        {
            int retRfID = 0;
            long? result = 0;
            string savePath = string.Empty;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (objFinTrxHeaderBO == null)
                objFinTrxHeaderBO = new FinTrxHeaderBO();
            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            objFinTrxHeaderBO.USER_PK = wkfDetails.UserPK;
            objFinTrxHeaderBO.WKF_APPLICATION = CurrPK;
            objFinTrxHeaderBO.WKF_COMMENTS = wkfDetails.Comments;
            objFinTrxHeaderBO.WKF_TRX_FLAG = workflowFlag;
            objFinTrxHeaderBO.WKF_PROCESS = wkfDetails.ProcessID;
            objFinTrxHeaderBO.WKF_REFERENCE = wkfDetails.ReferenceID;
            objFinTrxHeaderBO.WKF_TASK = wkfDetails.TaskID;
            objFinTrxHeaderBO.WKF_TASK_ACTION = wkfDetails.ActionID;
            //objSaleContract.WKF_MAIL_ATTACH = 0;
            action = wkfDetails.ActionText;
            #endregion
            objFinTrxHeaderBO.FTH_VOUCHER_NO = txtVoucherNo.Text;
            objFinTrxHeaderBO.IS_CANCEL = IsCancel.ToString();
            string xmlDoc = CommonFunctions.XmlSerialize<FinTrxHeaderBO>(objFinTrxHeaderBO);//CommonFunctions.ObjectTOXml(saleOrderHeaderObj);
            // save Process Control inspection details

            result = BusinessLogic.Jouralize.JournalizeBL.SaveVoucher(xmlDoc, out retRfID, out TrxNo);
            if (result.HasValue && result.Value > 0) // Success !  redirect to listing page
            {
                CurrPK = (int)result;
                ucrWrkf.ApplicationID = (int)result;
                hdfVoucherNo.Value = TrxNo;
                if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                {
                    litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;
                    //AdmTrxLogDet.ATL_ACTION = (byte)LogAction.CANCEL;
                }
                else
                {
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                    //AdmTrxLogDet.ATL_ACTION = (byte)LogAction.SUBMIT;
                }
                hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                object[] args = new object[2];
                args[0] = Resources.PageNameRes.Journalize;
                args[1] = hdfVoucherNo.Value;

                if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ReferanceID > 0)
                {
                    if (VoucherType == ApplicationType.OBV)
                    {
                        args[0] = Resources.PageNameRes.OpeningBalance;
                    }
                    else if (VoucherType == ApplicationType.DPVJ)
                    {
                        args[0] = Resources.PageNameRes.DirectPayment;
                    }
                    else if (VoucherType == ApplicationType.PCVJ)
                    {
                        args[0] = Resources.PageNameRes.PettyCash;
                    }
                    else if (VoucherType == ApplicationType.PCRVJ)
                    {
                        args[0] = Resources.PageNameRes.PettyCashReceipt;
                    }
                    else if (VoucherType == ApplicationType.YCV)
                    {
                        args[0] = Resources.PageNameRes.YearClosing;
                    }
                    else if (VoucherType == ApplicationType.JV)
                    {
                        args[0] = Resources.PageNameRes.JournalVoucher;
                    }
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                }
                else
                {

                    if (VoucherType == ApplicationType.OBV)
                    {
                        args[0] = Resources.PageNameRes.OpeningBalance;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                            Page.ResolveClientUrl(Resources.PageURL.OpeningBalanceList) + "');", true);
                    }
                    else if (VoucherType == ApplicationType.DPVJ)
                    {
                        args[0] = Resources.PageNameRes.DirectPayment;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                            Page.ResolveClientUrl(Resources.PageURL.DirectPaymentList) + "');", true);
                    }
                    else if (VoucherType == ApplicationType.JV)
                    {
                        args[0] = Resources.PageNameRes.JournalVoucher;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                            Page.ResolveClientUrl(Resources.PageURL.JVPath) + "');", true);
                    }
                    else if (VoucherType == ApplicationType.PCVJ)
                    {
                        args[0] = Resources.PageNameRes.PettyCash;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                            Page.ResolveClientUrl(Resources.PageURL.PettyCashList) + "');", true);
                    }


                    else if (VoucherType == ApplicationType.PCRVJ)
                    {
                        args[0] = Resources.PageNameRes.PettyCashReceipt;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                            Page.ResolveClientUrl(Resources.PageURL.PettyCashReceiptList) + "');", true);
                    }


                    //Contra Entry
                    else if (VoucherType == ApplicationType.CTVJ)
                    {
                        args[0] = Resources.PageNameRes.ContraEntry;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                            Page.ResolveClientUrl(Resources.PageURL.ContraEntryList) + "');", true);
                    }
                    else if (VoucherType == ApplicationType.YCV)
                    {
                        args[0] = Resources.PageNameRes.YearClosing;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                            Page.ResolveClientUrl(Resources.PageURL.YearClosingVoucherList) + "');", true);
                    }
                }
            }
            else
            {
                if (result == (int)DbSaveStatus.SQLERROR)
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CONCURRENCY)
                {
                    litErrorMsg.Text = GetLocalResourceObject("Invoice").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = GetLocalResourceObject("Invoice").ToString() + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                }
                else if (result == (int)DbSaveStatus.REFNOEXIST)
                {
                    litErrorMsg.Text = GetLocalResourceObject("Invoice").ToString() + " " + GetLocalResourceObject("RefNoExist").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                }
                else if (result == -111)//If the voucher Date year is selected is not same as the current financial year
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_Financial_Year").ToString()) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.FINYEARNOTEXIST)//Financial year not entered
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_Financial_Year_Notentered").ToString()) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (result == -222)//CWIP Asset exist
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_CWIPAssetExist").ToString()) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (result == -800)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Total").ToString()) + "','" + Resources.Messages.Information + "');", true);
                }
                else
                {
                    //Trx not saved
                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Error_NoPK;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Voucher);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                txtVoucherNo.Text = hdfVoucherNo.Value = string.Empty;
                return;
            }

        }
        #endregion
        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditViewWhtPopup(GridViewRow grwWhtDetails)
        {
            try
            {

                divErrorLabel.Visible = false;
                finVatPaymentDetails = TempWHTTaxDetails[grwWhtDetails.RowIndex];
                if (finVatPaymentDetails != null)
                {

                    hdfWHTAccountPopup.Value = finVatPaymentDetails.WTH_TAX.ToString();
                    hdfWHTTaxCategory.Value = finVatPaymentDetails.WTH_TAX_CATEGORY.ToString();
                    hdfWHTTaxName.Value = finVatPaymentDetails.WTH_NAME;
                    txtDescriptionPopup.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_DESC);
                    ddlFormno.SelectedValue = finVatPaymentDetails.WTH_FORM_NO.ToString();
                    txtCustomerTxtWHT.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_PARTY_NAME);
                    txtpartyads.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_ADDRESS);
                    txtTaxid.Text = finVatPaymentDetails.WTH_TAX_ID;
                    txtPopupWHTAmount.Text = GetFormattedCurrency(finVatPaymentDetails.WTH_AMOUNT);
                    txtWHTTaxAmountPopup.Text = GetFormattedCurrency(finVatPaymentDetails.WTH_TAX_AMT);
                    txtWHTAccountPopup.Text = finVatPaymentDetails.WTH_NAME;
                    txtDescriptionPopup.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_DESC);
                    txtWthAddressType.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_BRANCH_NAME);
                    txtWthBranchCode.Text = HttpUtility.HtmlDecode(finVatPaymentDetails.WTH_BRANCH_TEXT);
                    txtCRTNo.Text = string.IsNullOrEmpty(finVatPaymentDetails.WTH_CERT_NO) ? string.Empty : finVatPaymentDetails.WTH_CERT_NO;
                    if (finVatPaymentDetails.WTH_BRANCH.HasValue)
                        hdfWthAddressType.Value = finVatPaymentDetails.WTH_BRANCH.ToString();
                    else
                        hdfWthAddressType.Value = string.Empty;
                    if (finVatPaymentDetails.WTH_BRANCH_TYPE == (byte)VendorContactTypeEnum.HeadOffice)
                    {
                        chkWthHeadOffice.Checked = true;
                    }
                    else
                    {
                        chkWthHeadOffice.Checked = false;
                    }
                    if (finVatPaymentDetails.WTH_PAYMENT_TYPE != 0)
                    {
                        ddlPayType.SelectedValue = Convert.ToString(finVatPaymentDetails.WTH_PAYMENT_TYPE);
                    }
                    else
                    {
                        ddlPayType.SelectedValue = CommonConstants.SELECTVAL;
                    }
                    whtTaxpk = !string.IsNullOrEmpty(hdfWHTAccountPopup.Value) ? Convert.ToInt32(hdfWHTAccountPopup.Value) : 0;

                    GetFieldValues(ControlsEnum.VENDORACCOUNTTAX);
                    if (dtVendorAccount != null && dtVendorAccount.Rows.Count > 0)
                    {
                        decimal amount = 0;
                        amount = txtPopupWHTAmount.Text != string.Empty ? Convert.ToDecimal(txtPopupWHTAmount.Text) : 0;
                        string taxFormula = dtVendorAccount.Rows[0]["TAX_FORMULA"].ToString();
                        hdfTaxformula.Value = taxFormula;
                    }
                }
                ViewState["WhtDetRowIndex"] = grwWhtDetails.RowIndex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Set BranchCode Visibility
        private void SetBranchCodeVisibility()
        {
            txtBranchCode.Text = string.Empty;
            txtVatTaxId.Text = string.Empty;

            GetFieldValues(ControlsEnum.VENDORCONTACTYPEDETAILS);

            if (dtAdsTypeDtl != null && dtAdsTypeDtl.Rows.Count > 0)
            {
                if (Convert.ToInt32(dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.vncType]) == (int)VendorContactTypeEnum.Branch)
                {
                    txtBranchCode.Enabled = true;
                    vrfBranchCode.Enabled = true;
                    txtBranchCode.CssClass = "";
                }
                else
                {
                    //txtBranchCode.Enabled = false;
                    //txtBranchCode.Text = "";
                    vrfBranchCode.Enabled = false;
                    //txtBranchCode.CssClass = "input-disabled";
                }

                SetFieldValues(ControlsEnum.VENDORCONTACTYPEDETAILS);
            }

        }
        #endregion

        #region Show Vat Buy popup
        /// <summary>
        /// Function to show VAT BUY popup.
        /// </summary>
        private void ShowVatbuyPopup()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divVatBuy]','" + GetLocalResourceObject("VatBuyTaxDetails").ToString() + "','916','400');", true);
        }
        #endregion

        #region Show WHT popup
        /// <summary>
        /// Function to show WHT popup.
        /// </summary>
        private void ShowWhtPopup()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("WHTTaxDetails").ToString() + "','916','400');", true);
        }
        #endregion

        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            HiddenField hdfWHTFormNo;
            Label lblformnoGRD;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    if (((GridView)sender).ID == "grdWHTTaxDetails")
                    {
                        hdfWHTFormNo = e.Row.FindControl("hdfWHTFormNo") as HiddenField;
                        HiddenField hdfWhtBranchType = e.Row.FindControl("hdfWhtBranchType") as HiddenField;
                        lblformnoGRD = e.Row.FindControl("lblformnoGRD") as Label;
                        Label lblWhtTye = e.Row.FindControl("lblWhtTye") as Label;
                        if (TempWHTTaxDetails != null && TempWHTTaxDetails.Count > 0)
                        {
                            string tempFormno = ddlFormno.Items.FindByValue(hdfWHTFormNo.Value).Text;
                            lblformnoGRD.Text = tempFormno;
                            //fill WHT popup
                            ddlFormno.SelectedValue = TempWHTTaxDetails[e.Row.RowIndex].WTH_FORM_NO.ToString();
                            txtCustomerTxtWHT.Text = ERP.Utilities.CommonFunctions.GetShortString(TempWHTTaxDetails[e.Row.RowIndex].WTH_PARTY_NAME, 300);
                            //txtCustomerTxtWHT.ToolTip = TempWHTTaxDetails[e.Row.RowIndex].WTH_PARTY_NAME;
                            hdfvendorWHTPK.Value = TempWHTTaxDetails[e.Row.RowIndex].WTH_PK.ToString();
                            txtpartyads.Text = TempWHTTaxDetails[e.Row.RowIndex].WTH_ADDRESS;
                            txtTaxid.Text = TempWHTTaxDetails[e.Row.RowIndex].WTH_TAX_ID;

                            if (TempConfigMstDetails.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(hdfWhtBranchType.Value) && (Convert.ToInt32(hdfWhtBranchType.Value) != Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO)))
                                {
                                    lblWhtTye.Text = TempConfigMstDetails.SingleOrDefault(cnfg => cnfg.CFG_VALUE == Convert.ToByte(hdfWhtBranchType.Value)).CFG_DATA;
                                    if (Convert.ToByte(hdfWhtBranchType.Value) == (byte)VendorContactTypeEnum.Branch)
                                    {
                                        lblWhtTye.ToolTip = lblWhtTye.Text + " (" + TempWHTTaxDetails[e.Row.RowIndex].WTH_BRANCH_TEXT + ")";
                                    }
                                }
                            }
                            else
                            {
                                GetFieldValues(ControlsEnum.VENDORTYPES);
                                if (TempConfigMstDetails.Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(hdfWhtBranchType.Value) && (Convert.ToInt32(hdfWhtBranchType.Value) != Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO)))
                                    {
                                        lblWhtTye.Text = TempConfigMstDetails.SingleOrDefault(cnfg => cnfg.CFG_VALUE == Convert.ToByte(hdfWhtBranchType.Value)).CFG_DATA;
                                        if (Convert.ToByte(hdfWhtBranchType.Value) == (byte)VendorContactTypeEnum.Branch)
                                        {
                                            lblWhtTye.ToolTip = lblWhtTye.Text + " (" + TempWHTTaxDetails[e.Row.RowIndex].WTH_BRANCH_TEXT + ")";
                                        }
                                    }
                                }
                            }
                            // END fill WHT popup
                        }
                    }
                    if (((GridView)sender).ID == "grdVATTaxDetails")
                    {
                        HiddenField hdfBranchType = e.Row.FindControl("hdfBranchType") as HiddenField;
                        HiddenField hdfAmount = e.Row.FindControl("hdfAmount") as HiddenField;
                        HiddenField hdfTaxAmount = e.Row.FindControl("hdfTaxAmount") as HiddenField;
                        Label lblTye = e.Row.FindControl("lblTye") as Label;
                        if (TempVATTaxDetails != null && TempVATTaxDetails.Count > 0)
                        {
                            AmountTotal += Convert.ToDecimal(hdfAmount.Value);
                            TaxTotal += Convert.ToDecimal(hdfTaxAmount.Value);

                            //txtVendorPopup.Text = purVendorMstList[0].VEN_NAME;
                            //hdfVendorPopup.Value = purVendorMstList[0].VEN_PK.ToString();


                            //string tempFormno = ddlFormno.Items.FindByValue(hdfWHTFormNo.Value).Text;
                            //lblformnoGRD.Text = tempFormno;
                            //fill WHT popup
                            //ddlFormno.SelectedValue = TempVATTaxDetails[e.Row.RowIndex].WTH_FORM_NO.ToString();
                            txtVendorPopup.Text = ERP.Utilities.CommonFunctions.GetShortString(TempVATTaxDetails[e.Row.RowIndex].WTH_PARTY_NAME, 300);
                            ////if (TempVATTaxDetails[e.Row.RowIndex].WTH_BRANCH_TYPE == (byte)VendorContactTypeEnum.HeadOffice)
                            ////    chkHeadOffice.Checked = true;
                            ////HeadofficeCheckedChanged();
                            ////SetBranchCodeVisibility();
                            ////if (finVatPaymentDetails.WTH_VENDOR.HasValue)
                            ////{
                            ////    hdfVendorPopup.Value = finVatPaymentDetails.WTH_VENDOR.ToString();
                            ////}
                            ////else
                            ////{
                            ////    hdfVendorPopup.Value = string.Empty;
                            ////    GetFieldValues(ControlsEnum.VENDORSELECTEDDTL);
                            ////    SetFieldValues(ControlsEnum.VENDORSELECTEDDTL);
                            ////}

                            ////if (finVatPaymentDetails.WTH_BRANCH.HasValue)
                            ////{
                            ////    hdfAddressType.Value = finVatPaymentDetails.WTH_BRANCH.ToString();
                            ////}
                            ////else
                            ////{
                            ////    hdfAddressType.Value = string.Empty;
                            ////}

                            ////txtAddressType.Text = finVatPaymentDetails.WTH_BRANCH_NAME;

                            //txtCustomerTxtWHT.ToolTip = TempWHTTaxDetails[e.Row.RowIndex].WTH_PARTY_NAME;
                            //hdfVendorVatPK.Value = TempVATTaxDetails[e.Row.RowIndex].WTH_PK.ToString();
                            //txtpartyads.Text = TempVATTaxDetails[e.Row.RowIndex].WTH_ADDRESS;
                            //txtVatTaxInvDate.Text = Convert.ToDateTime(TempVATTaxDetails[e.Row.RowIndex].WTH_TAX_DATE).ToString(Resources.Constants.DateFormatShort);
                            //txtVatTaxId.Text = TempVATTaxDetails[e.Row.RowIndex].WTH_TAX_ID;
                            //ddlAddressType.SelectedValue = TempVATTaxDetails[e.Row.RowIndex].WTH_BRANCH_TYPE.ToString();

                            if (TempConfigMstDetails.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(hdfBranchType.Value) && (Convert.ToInt32(hdfBranchType.Value) != Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO)))
                                {
                                    lblTye.Text = TempConfigMstDetails.SingleOrDefault(cnfg => cnfg.CFG_VALUE == Convert.ToByte(hdfBranchType.Value)).CFG_DATA;
                                    if (Convert.ToByte(hdfBranchType.Value) == (byte)VendorContactTypeEnum.Branch)
                                    {
                                        lblTye.ToolTip = lblTye.Text + " (" + TempVATTaxDetails[e.Row.RowIndex].WTH_BRANCH_TEXT + ")";
                                    }
                                }
                            }
                            else
                            {
                                GetFieldValues(ControlsEnum.VENDORTYPES);
                                if (TempConfigMstDetails.Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(hdfBranchType.Value) && (Convert.ToInt32(hdfBranchType.Value) != Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO)))
                                    {
                                        lblTye.Text = TempConfigMstDetails.SingleOrDefault(cnfg => cnfg.CFG_VALUE == Convert.ToByte(hdfBranchType.Value)).CFG_DATA;
                                        if (Convert.ToByte(hdfBranchType.Value) == (byte)VendorContactTypeEnum.Branch)
                                        {
                                            lblTye.ToolTip = lblTye.Text + " (" + TempVATTaxDetails[e.Row.RowIndex].WTH_BRANCH_TEXT + ")";
                                        }
                                    }
                                }
                            }
                            // END fill WHT popup
                        }
                    }
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    if (((GridView)sender).ID == "grdVATTaxDetails")
                    {
                        Label lblAmountTotal = (Label)e.Row.FindControl("lblAmountTotal");
                        Label lblTaxTotal = (Label)e.Row.FindControl("lblTaxTotal");
                        lblAmountTotal.Text = AmountTotal.ToString("c");
                        lblTaxTotal.Text = TaxTotal.ToString("c");
                    }
                }
                //if (((GridView)sender).ID == "grdVoucher")
                //{
                //if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                //{
                if (((GridView)sender).ID == "grdVoucher")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblDr = (Label)e.Row.FindControl("lblDebit");
                        DrTotal += Convert.ToDecimal(lblDr.Text);

                        Label lblCr = (Label)e.Row.FindControl("lblCredit");
                        CrTotal += Convert.ToDecimal(lblCr.Text);

                        HiddenField hdfAccountSubType = (HiddenField)e.Row.FindControl("hdfAccountSubType");
                        if (hdfAccountSubType.Value == ((int)AccountSubType.WHT).ToString())
                        {
                            withHoldTax += Convert.ToDecimal(lblCr.Text);
                        }
                        if (hdfAccountSubType.Value == ((int)AccountSubType.VatBuy).ToString())
                        {
                            VatBuyTax += Convert.ToDecimal(lblDr.Text);
                        }


                    }
                    else if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        Panel pnlWHT = (Panel)e.Row.FindControl("pnlWHT");
                        Panel pnlVatBuy = (Panel)e.Row.FindControl("pnlVatBuy");
                        ImageButton imgbtnPrint = (ImageButton)e.Row.FindControl("imgbtnPrint");
                        Label lblDrTotal = (Label)e.Row.FindControl("lblDebitTotal");
                        lblDrTotal.Text = DrTotal.ToString("c");
                        hdfDebitTotal.Value = DrTotal.ToString();

                        Label lblCrTotal = (Label)e.Row.FindControl("lblCreditTotal");
                        lblCrTotal.Text = CrTotal.ToString("c");
                        hdfCreditTotal.Value = CrTotal.ToString();
                        imgbtnPrint.Visible = false;
                        pnlWHT.Visible = false;
                        pnlVatBuy.Visible = false;
                        if (VoucherType == ApplicationType.DPVJ || VoucherType == ApplicationType.PCVJ)
                        {
                            if (withHoldTax > 0)
                            {
                                pnlWHT.Visible = true;
                                TextBox txtWHTAmount = (TextBox)e.Row.FindControl("txtWHTAmount");
                                txtWHTAmount.Text = withHoldTax.ToString("c");
                                hdfWithHoldTax.Value = withHoldTax.ToString();
                                if (CurrPK > 0)
                                    imgbtnPrint.Visible = true;

                            }
                            if (VatBuyTax > 0)
                            {

                                pnlVatBuy.Visible = true;
                                TextBox txtVatBuy = (TextBox)e.Row.FindControl("txtVatBuy");
                                decimal TaxRate = 0;
                                TaxPK = Convert.ToInt32(ddlVATAccountPopup.SelectedValue);
                                if (TaxPK > 0)
                                {
                                    hdfVATAccountPopup.Value = ddlVATAccountPopup.SelectedValue;
                                    GetFieldValues(ControlsEnum.TAXDETAILS);
                                    TaxPK = 0;
                                    if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
                                    {
                                        string txRate = dtTaxDetails.Rows[0][Resources.DataFieldRes.TaxRate].ToString();
                                        decimal.TryParse(txRate, out TaxRate);
                                    }
                                }

                                txtVatBuy.Text = VatBuyTax.ToString("c");
                                hdfVatBuy.Value = VatBuyTax.ToString();
                                //txtBeforeTaxAmount.Text = VatBuyTax.ToString();
                                txtVATTaxAmountPopup.Text = VatBuyTax.ToString();
                                try
                                {
                                    if (TaxRate <= 0)
                                    {
                                        txtBeforeTaxAmount.Text = GetFormattedCurrency(0);
                                    }
                                    else
                                    {
                                        txtBeforeTaxAmount.Text = Math.Round(((VatBuyTax * 100) / TaxRate), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                                    }
                                }
                                catch { }
                                GetUIValuesFromObject(ControlsEnum.TAXTYPECHANGED);

                            }

                        }
                        else
                        {
                            pnlWHT.Visible = false;
                            pnlVatBuy.Visible = false;
                        }
                    }
                }
                //}
                // }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            this.btnVocherDelete.PreRender += new EventHandler(btnAction_PreRender);
            this.btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnVatTaxApply.PreRender += new EventHandler(btnAction_PreRender);
            this.btnVatTaxSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnWhtTaxSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnApply.PreRender += new EventHandler(btnAction_PreRender);
            this.btnReturnDetail.PreRender += new EventHandler(btnAction_PreRender);
            this.btnLoad.PreRender += new EventHandler(btnAction_PreRender);


            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnCancel.Load += new EventHandler(btnAction_Load);
            this.btnVocherDelete.Load += new EventHandler(btnAction_Load);
            this.btnPrint.Load += new EventHandler(btnAction_Load);
            this.btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            this.btnVatTaxApply.Load += new EventHandler(btnAction_Load);
            this.btnVatTaxSave.Load += new EventHandler(btnAction_Load);
            this.btnWhtTaxSave.Load += new EventHandler(btnAction_Load);
            this.btnApply.Load += new EventHandler(btnAction_Load);
            this.btnReturnDetail.Load += new EventHandler(btnAction_Load);
            this.btnLoad.Load += new EventHandler(btnAction_Load);
        }

        protected void btnAction_Load(object sender, EventArgs e)
        {
            (sender as Control).Visible = true;
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
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
        /// To handle OnInit event Used to assign the Event for all the actions used in this page        
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
            //if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
            //{
            //    if (Session[ERP.Utilities.SessionStrings.Transaction].ToString() == "CANCEL")
            //    {
            //        EntryStatus = EntryStatus.LISTMODE;
            //        Session[ERP.Utilities.SessionStrings.Transaction] = null;
            //    }
            //}

            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideSections", "ShowHideSections(" + AccountType + ");", true);

            if (EntryStatus == EntryStatus.VIEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
            }
            else if (EntryStatus == EntryStatus.NEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);
            }
            else if (EntryStatus == EntryStatus.ENTRYMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(4);});", true);

            }
            if (VoucherType == ApplicationType.OBV)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(3);});", true);
            }

            BindPageTitle();

        }

        /// <summary>
        /// To set page title
        /// </summary>
        private void BindPageTitle()
        {
            if (VoucherType == ApplicationType.DPVJ)
            {
                Page.Title = Resources.Captions.Title_DirectPayment;
            }
            else if (VoucherType == ApplicationType.OBV)
            {
                Page.Title = Resources.Captions.Title_OpeningBalance;
            }
            else if (VoucherType == ApplicationType.PCVJ)
            {
                // Page.Title = Resources.Captions.Title_Pettycash;
                Page.Title = this.GetLocalResourceObject("Title_Pettycash").ToString();
            }

            else if (VoucherType == ApplicationType.PCRVJ)
            {
                // Page.Title = Resources.Captions.Title_Pettycash;
                Page.Title = this.GetLocalResourceObject("Title_PettycashReceipt").ToString();
            }

            else if (VoucherType == ApplicationType.CTVJ)
            {
                Page.Title = this.GetLocalResourceObject("Title_Contra").ToString();
            }
            else if (VoucherType == ApplicationType.YCV)
            {
                Page.Title = Resources.Captions.Title_YearClosing;
            }
            else if (VoucherType == ApplicationType.JV)
            {
                Page.Title = Resources.Captions.Title_JournalVoucher;
            }
            string breadCrumb = Page.Title.ToString().Replace(" –  ", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
            lblBreadCrum.Text = breadCrumb;
        }

        /// <summary>
        /// To set  Cost Center Allocation Button Visibility
        /// </summary>       
        private void ShowHideCostCenterAllocButton()
        {
            int selAccPk = 0;
            if (hdfAccount1 != null)
            {
                int.TryParse(hdfAccount1.Value, out selAccPk);
            }
            if (selAccPk > 0)
            {
                DataTable dtCostCenterMpg = BusinessLogic.CommonManagement.CommonBL.GetCostCenter(selAccPk, 0);
                if (dtCostCenterMpg != null && dtCostCenterMpg.Rows.Count > 0)
                {
                    imbShowCostCenterAllocPopup.Visible = true;
                }
                else
                {
                    imbShowCostCenterAllocPopup.Visible = false;
                }
            }
            else
            {
                imbShowCostCenterAllocPopup.Visible = false;
            }
        }

        /// <summary>
        /// Cost center splitup validation checking
        /// </summary>       
        private bool IsValidCostCenterSplitAllocation(List<CostCenterDetails> costCenterAllocationList)
        {

            bool CostSplitValidation = true;
            if (costCenterAllocationList != null && costCenterAllocationList.Count > 0)
            {
                decimal creditAmt = !string.IsNullOrEmpty(txtCredit1.Text) ? Convert.ToDecimal(txtCredit1.Text) : 0;
                decimal debitAmt = !string.IsNullOrEmpty(txtDebit1.Text) ? Convert.ToDecimal(txtDebit1.Text) : 0;
                decimal debitCreditAmnt = creditAmt + debitAmt;
                if (GetGlobalResourceObject("ConfigurationsRes", "CCPercentageRequired").ToString() == "1" || GetGlobalResourceObject("ConfigurationsRes", "EnableCostCenter").ToString() == "1")
                {
                    if (ERP.Utilities.CommonFunctions.DecimalFormat(costCenterAllocationList.Sum(r => r.FTD_AMT_BC), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) != debitCreditAmnt)
                        CostSplitValidation = false;
                }
            }
            else
            {
                #region If user not allocated,set it by default
                selAccountPk = 0;
                int.TryParse(hdfAccount1.Value, out selAccountPk);
                if (selAccountPk > 0)
                {
                    decimal debitAmount = 0, creditAmount = 0;
                    decimal.TryParse(txtDebit1.Text, out debitAmount);
                    decimal.TryParse(txtCredit1.Text, out creditAmount);
                    CCAmount = debitAmount > 0 ? debitAmount : creditAmount;

                    if (Session["VoucherDet"] != null)
                    {
                        finTrxList = (List<FIN_TRX>)(Session["VoucherDet"]);
                        finTrxList = (from ftrList in finTrxList
                                      where ftrList.FTR_PK == CurrFtrPK
                                      select ftrList).ToList();
                    }


                    GetFieldValues(ControlsEnum.COSTCENTER);
                    if (dtCostCenter != null && dtCostCenter.Rows.Count > 0)
                    {
                        CostCenterList = new List<CostCenterDetails>();

                        if (finTrxList != null)
                        {
                            foreach (FIN_TRX_COC_DTL objTrxCoc in finTrxList[0].FIN_TRX_COC_DTL)
                            {
                                CostCenterDetails objCCDtls = new CostCenterDetails();
                                objCCDtls.FCM_CNM_PK = objTrxCoc.FTD_CNM_PK;
                                List<string> lstResult = (from table in dtCostCenter.AsEnumerable()
                                                          where table.Field<int>("FCM_CNM_PK") == objTrxCoc.FTD_CNM_PK
                                                          select table.Field<string>("FCM_COST_CENTER_TEXT")).ToList();
                                objCCDtls.FCM_COST_CENTER_TEXT = lstResult[0];
                                objCCDtls.FTD_AMT_BC = objTrxCoc.FTD_AMT_BC;
                                CostCenterList.Add(objCCDtls);
                            }
                        }
                        else
                        {
                            foreach (DataRow drRow in dtCostCenter.Rows)
                            {
                                CostCenterList.Add(new CostCenterDetails
                                {
                                    FCM_CNM_PK = Convert.ToInt32(drRow["FCM_CNM_PK"]),
                                    FCM_COST_CENTER_TEXT = CommonFunctions.GetEncodedString(drRow["FCM_COST_CENTER_TEXT"]),
                                    FTD_AMT_BC = Convert.ToDecimal(drRow["FCM_AMOUNT"])
                                });
                            }
                        }
                    }
                }
                #endregion
                if (CostCenterList != null && CostCenterList.Count > 0)
                {
                    if (GetGlobalResourceObject("ConfigurationsRes", "CCPercentageRequired").ToString() == "1" || GetGlobalResourceObject("ConfigurationsRes", "EnableCostCenter").ToString() == "1")
                    {
                        if (ERP.Utilities.CommonFunctions.DecimalFormat(CostCenterList.Sum(r => r.FTD_AMT_BC), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) != CCAmount)
                            CostSplitValidation = false;
                    }
                }
            }
            return CostSplitValidation;
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
                //if (Session[ERP.Utilities.SessionStrings.JOURNALIZETAB_SELECTED_PK] == null)
                //{
                //    ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                //}
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;

                //if (Session[ERP.Utilities.SessionStrings.TransactionType] == null)
                //{
                //    ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                //    hdfJournalizeWorkFlow.Value = "0";
                //}     

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

                ucrTransactionImport.TransactionActionHandler += new EventHandler(ActionHandler);
                ucrTransactionImport.AfterValidation += new EventHandler(ActionHandler);



                if (!IsPostBack)
                {
                    ConfigurationSettings();
                    //Sets data key for the gird
                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = Resources.DataFieldRes.FinTrxPk;
                    grdVoucher.DataKeyNames = datakeyarray;
                    TempConfigMstDetails = null;
                    grdVoucher.DataSource = null;
                    grdVoucher.DataBind();

                    Session["VoucherDet"] = null;
                    RecordIdentity = Guid.NewGuid().ToString();
                    hdfPreviousAccount1Name.Value = string.Empty;
                    hdfPreviousAccount1Pk.Value = "0";
                    CostCenterList.Clear();
                    hdfIsContCCAllocDeletion.Value = "0";


                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    //To set company related to current SBU 
                    if (dtCmp != null && dtCmp.Rows.Count > 0)
                    {
                        ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCmp.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()));
                    }
                    GetFieldValues(ControlsEnum.FORMNO);
                    SetFieldValues(ControlsEnum.FORMNO);
                    GetFieldValues(ControlsEnum.VENDORTYPES);
                    //SetFieldValues(ControlsEnum.VENDORCONTACTYPE);                  

                    WHTTaxDetails = null;
                    TempWHTTaxDetails = null;
                    VATTaxDetails = null;
                    TempVATTaxDetails = null;
                    CostCenterList = null;
                    hdfItemEdit.Value = "0";

                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                    }
                    hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                    hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();

                    AST_DOC_MODE.Value = "0";
                    if (Request.QueryString["AppType"] != null)
                        VoucherType = Request.QueryString["AppType"].ToString();
                    else
                        VoucherType = ApplicationType.BNP;
                    hdfJournalType.Value = Request.QueryString["AppType"].ToString();
                    Session[ERP.Utilities.SessionStrings.Type] = VoucherType == ApplicationType.DPBJ ? ApplicationType.DPVJ : VoucherType;
                    currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                    if (VoucherType == ApplicationType.JV && GetGlobalResourceObject("ConfigurationsRes", "IsShowVoucherDataImport").ToString() == "1")
                    {
                        string commandArgument = GetGlobalResourceObject("Constants", "JournalCommandArgument").ToString();
                        btnSave.CommandArgument = commandArgument;
                        btnSaveSubmit.CommandArgument = commandArgument;
                        btnSubmit.CommandArgument = commandArgument;
                        btnCancelSubmit.CommandArgument = commandArgument;
                        btnVocherDelete.CommandArgument = commandArgument;
                        btnPrint.CommandArgument = commandArgument;
                        btnCancel.CommandArgument = commandArgument;
                        btnChequePrint.CommandArgument = commandArgument;
                        btnReverseDetail.CommandArgument = commandArgument;
                        btnReturnDetail.CommandArgument = commandArgument;
                    }

                    if (VoucherType == ApplicationType.DPBJ || VoucherType == ApplicationType.DPVCJ)
                        VoucherType = ApplicationType.DPVJ;

                    if (hdfShowOthChrgChkBox.Value == "1" && VoucherType != ApplicationType.OBV && VoucherType != ApplicationType.BNP && VoucherType != ApplicationType.YCV)
                        chkIsBankCharge1.Visible = true;
                    else
                        chkIsBankCharge1.Visible = false;
                    if (VoucherType == ApplicationType.CTVJ)
                        hdfAccountType.Value = "31";//For Contra Entry Accounts Table: FIN_COA_SUB_TYPE_CFG
                    else
                        hdfAccountType.Value = "0"; //For All Accounts
                    if (hdfShowCRTNo.Value == "1")
                    {
                        lblCRTNo.Visible = true;
                        txtCRTNo.Visible = true;
                    }
                    if (hdfCRTNoAuto.Value == "1")
                    {
                        rfvCRTNO.Visible = false;
                        txtCRTNo.Enabled = false;
                    }
                    else
                    {
                        rfvCRTNO.Visible = true;
                        txtCRTNo.Enabled = true;
                    }

                    //if (hdfShowCashAccountDiv.Value == "1" && VoucherType == ApplicationType.PCVJ)
                    if (hdfShowCashAccountDiv.Value == "1")
                    {
                        GetFieldValues(ControlsEnum.CASHACCOUNT);
                        SetFieldValues(ControlsEnum.CASHACCOUNT);
                    }
                    hdfVoucherType.Value = VoucherType; // == ApplicationType.DPBJ ? ApplicationType.DPVJ : VoucherType;
                    txtVoucherNo.Text = Resources.ErpRes.Draft;
                    txtVoucherDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    txtRefDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfCurrency.Value = (currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency).ToString();

                    GetFieldValues(ControlsEnum.VATBUYTAXTYPES);
                    SetFieldValues(ControlsEnum.VATBUYTAXTYPES);
                    BindPageTitle();
                    Session[ERP.Utilities.SessionStrings.TrxPK] = null;
                    if (Session[ERP.Utilities.SessionStrings.JournalMode] != null)
                    {
                        EntryStatus = (EntryStatus)(Enum.Parse(typeof(EntryStatus), Session[ERP.Utilities.SessionStrings.JournalMode].ToString()));
                        if (EntryStatus == EntryStatus.VIEWMODE)
                        {
                            btnSave.Visible = false;
                        }
                        else
                        {
                            //btnSave.Visible = true;
                        }
                    }
                    if (Session[ERP.Utilities.SessionStrings.TransactionCancel] != null && Session[ERP.Utilities.SessionStrings.TransactionCancel].ToString() == TransactionType.CANCELATION)
                    {
                        //if (Session[ERP.Utilities.SessionStrings.Transaction].ToString() == TransactionType.CANCELATION)
                        FillProcessID(VoucherType, 12);
                        Session[ERP.Utilities.SessionStrings.TransactionCancel] = null;
                    }
                    else
                    {
                        FillProcessID(VoucherType, 0);
                    }

                    WorkflowCore.CoreService workflowCore1 = new WorkflowCore.CoreService();
                    ucrWrkf.RefID = workflowCore1.GetRefID(CurrPK, ucrWrkf.ProcessID);
                    ucrWrkf.FillWorkFlowDetails();

                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                        : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;
                    //If Has RefID (from Inbox)
                    if (!string.IsNullOrEmpty(refID))
                    {
                        ReferanceID = int.Parse(refID);
                        if (!string.IsNullOrEmpty(inboxFlag))
                        {
                            ucrWrkf.ViewType = 0;
                            EntryStatus = EntryStatus.VIEWMODE;
                            btnSave.Visible = false;
                            //btnSubmit.Visible = false;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                            EntryStatus = EntryStatus.ENTRYMODE;
                            btnVocherDelete.Visible = false;
                        }

                        if (Request.QueryString["AppType"] != null && Request.QueryString["AppType"].ToString() == ApplicationType.DPBJ || Request.QueryString["AppType"].ToString() == ApplicationType.DPVCJ) //VoucherType == ApplicationType.DPBJ || VoucherType == ApplicationType.DPVCJ
                        {
                            VoucherType = ApplicationType.DPVJ;
                            FillProcessID(VoucherType, 0);
                            ucrWrkf.RefID = int.Parse(refID);
                            JournalPK = GetApplicationID(ucrWrkf.RefID);
                            GetFieldValues(ControlsEnum.GETDIRECTPAYMENTPKBYJOURNALPK);
                            if (finTrxHdrListJournal != null && finTrxHdrListJournal.Count > 0)
                            {
                                CurrPK = (Int32)finTrxHdrListJournal[0].FTH_REF_PK;
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(Convert.ToInt32(CurrPK), ucrWrkf.ProcessID);
                            }
                        }
                        else
                        {
                            ucrWrkf.RefID = int.Parse(refID);
                            CurrPK = GetApplicationID(ucrWrkf.RefID);
                        }
                    }
                    if (CurrPK > 0)
                    {

                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            //EntryStatus = EntryStatus.VIEWMODE;
                        }
                        AST_DOC_MODE.Value = "0";
                        //////SetUIEditView(commonActions);
                        ModifiedDatePnl.Visible = true;
                        //////GetFieldValues(ControlsEnum.FINTRX);
                        finTrxHdrObj = CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_PK = CurrPK;
                        GetUIValuesFromObject(ControlsEnum.VOUCHERS);
                        GetFieldValues(ControlsEnum.APLNTYPE);
                        SetFieldValues(ControlsEnum.APLNTYPE);
                        GetFieldValues(ControlsEnum.BASECURRENCY);
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        GetFieldValues(ControlsEnum.MODE);
                        SetFieldValues(ControlsEnum.MODE);
                        GetFieldValues(ControlsEnum.VOUCHERS);
                        SetFieldValues(ControlsEnum.VOUCHERS);
                        GetFieldValues(ControlsEnum.WHTVENDOR);
                        SetFieldValues(ControlsEnum.WHTVENDOR);

                        //////GetFieldValues(ControlsEnum.FINTRXHDR);
                        //////GetUIValuesFromObject(ControlsEnum.FINTRX);
                    }
                    else
                    {
                        postflag = true;


                        AST_DOC_MODE.Value = GetDOCMODE();

                        GetFieldValues(ControlsEnum.APLNTYPE);
                        SetFieldValues(ControlsEnum.APLNTYPE);
                        GetFieldValues(ControlsEnum.BASECURRENCY);
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        GetFieldValues(ControlsEnum.MODE);
                        SetFieldValues(ControlsEnum.MODE);
                        //VoucherType = Session[ERP.Utilities.SessionStrings.TransactionType] == null ? "" : Session[ERP.Utilities.SessionStrings.TransactionType].ToString();
                        //CurrPK = Session[ERP.Utilities.SessionStrings.TrxPK] == null ? 0 : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TrxPK].ToString());
                        CurrPK = Session[ERP.Utilities.SessionStrings.VoucherPk] == null ? 0 : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.VoucherPk].ToString());
                        Session[ERP.Utilities.SessionStrings.VoucherPk] = null;
                        if (CurrPK > 0)
                        {

                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                                //EntryStatus = EntryStatus.VIEWMODE;
                            }
                            GetFieldValues(ControlsEnum.VOUCHERS);
                            SetFieldValues(ControlsEnum.VOUCHERS);
                            GetFieldValues(ControlsEnum.AUDITLOGSTATUS);
                            if (dtStatus != null && dtStatus.Rows.Count > 0)
                                lnkAuditLog.Visible = dtStatus.Rows[0][0].ToString() == "1" ? true : false;
                            if (lnkAuditLog.Visible)
                            {
                                GetFieldValues(ControlsEnum.AUDITLOGDETAILS);
                                //SetFieldValues(ControlsEnum.AUDITLOGDETAILS);
                                ucrAuditLogList.FillDetails(dtAuditDetails);
                                lblExchangeRate.CssClass = "middle-lbl-a mg-lft-9";
                            }
                            else
                                lblExchangeRate.CssClass = "middle-lbl-a";
                            GetFieldValues(ControlsEnum.WHTVENDOR);
                            SetFieldValues(ControlsEnum.WHTVENDOR);
                        }
                        if (VoucherType == ApplicationType.OBV || VoucherType == ApplicationType.YCV)
                        {
                            txtNarration1.Text = Resources.ErpRes.OpeningBalance;
                            if (VoucherType == ApplicationType.YCV)
                                txtNarration1.Text = Resources.ErpRes.ClosingBalance;
                            selectedModeValue = 1;
                            GetFieldValues(ControlsEnum.MODE);
                            if (admConfigMstList != null && admConfigMstList.Count > 0)
                            {
                                ddlMode1.SelectedIndex = ddlMode1.Items.IndexOf(ddlMode1.Items.FindByValue(admConfigMstList[0].CFG_PK.ToString()));
                                ddlMode1.Enabled = false;
                            }
                            else
                            {
                                ddlMode1.Enabled = true;
                                ddlMode1.SelectedIndex = 0;
                            }
                        }
                        this.txtVoucherNo.Focus();
                    }

                    if ((EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.VIEWMODE) && (VoucherType == ApplicationType.DPVJ || VoucherType == ApplicationType.PCVJ || VoucherType == ApplicationType.YCV))
                    {
                        pnlPrint.Visible = true;
                    }
                    else
                    {
                        pnlPrint.Visible = false;
                    }
                    //Direct payment voucher.  Set print button in the view mode (Bug ID:  18040)
                    if (EntryStatus == EntryStatus.VIEWMODE && VoucherType == ApplicationType.DPVJ)
                    {
                        pnlPrint.Visible = true;
                    }
                    if (EntryStatus == EntryStatus.ENTRYMODE && VoucherType == ApplicationType.DPVJ)
                    {
                        pnlChequePrint.Visible = true;
                    }
                    else
                    {
                        pnlChequePrint.Visible = false;
                    }

                    if (hdfShowCashAccountDiv.Value == "1" && VoucherType == ApplicationType.PCVJ)
                    {
                        txtCredit1.Enabled = false;
                        txtCredit1.Attributes.Add("class", "input-disabled");
                    }

                    if (hdfShowCashAccountDiv.Value == "1" && VoucherType == ApplicationType.PCRVJ)
                    {
                        txtDebit1.Enabled = false;
                        txtDebit1.Attributes.Add("class", "input-disabled");
                    }

                    if (hdfIsShowVoucherDataImport.Value == "1" &&
                        (VoucherType == ApplicationType.JV ||
                        VoucherType == ApplicationType.DPVJ ||
                        VoucherType == ApplicationType.DRVJ))
                    {
                        divDIRPaymentImport.Visible = true;
                    }
                    else
                    {
                        divDIRPaymentImport.Visible = false;
                    }


                    if (Convert.ToInt32(hdfStatus.Value) > 0)
                    {
                        divDIRPaymentImport.Visible = false;
                    }


                    ucrTransactionImport.VoucherApplicationType = VoucherType;
                }
                Session[ERP.Utilities.SessionStrings.RefID] = null;
                Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Configuration
        /// </summary>
        private void ConfigurationSettings()
        {
            hdfCRTNoAuto.Value = GetGlobalResourceObject("ConfigurationsRes", "CRTNoAuto").ToString();
            hdfShowCRTNo.Value = GetGlobalResourceObject("ConfigurationsRes", "ShowCRTNo").ToString();
            hdfShowOthChrgChkBox.Value = GetGlobalResourceObject("ConfigurationsRes", "IsOtherChargeChkboxVisible").ToString();
            /*Automatically add Cash account entry for petty cash */
            hdfShowCashAccountDiv.Value = GetGlobalResourceObject("ConfigurationsRes", "AutoCashAccountVoucher").ToString();
            hdfIsShowVoucherDataImport.Value = GetGlobalResourceObject("ConfigurationsRes", "IsShowVoucherDataImport").ToString();
            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "VENDOR");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsSBUVendor.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
            }

        }
        #endregion


        private void ExcelImport_GetSubTypeAccount()
        {
            string relquery = string.Empty;
            bool isContinue;
            isContinue = true;
            #region CostAllocation Clearing (Do you want to continue?)

            List<FIN_TRX> finTrxListClear = new List<FIN_TRX>();
            if (CurrFtrPK > 0)
            {
                finTrxListClear = (List<FIN_TRX>)(Session["VoucherDet"]);
                finTrxListClear = (from ftrList in finTrxListClear
                                   where ftrList.FTR_PK == CurrFtrPK
                                   select ftrList).ToList();
                if (finTrxListClear[0].FIN_TRX_COC_DTL != null && finTrxListClear[0].FIN_TRX_COC_DTL.Count > 0)
                {
                    if (hdfIsContCCAllocDeletion.Value != "1")
                    {
                        isContinue = false;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InvoiceNowExceeds", "$(document).ready(function(){ShowCostCenterAllocationClear(1);});", true);
                    }
                    else
                    {
                        if (finTrxListClear[0].FIN_TRX_COC_DTL != null && finTrxListClear[0].FIN_TRX_COC_DTL.Count > 0)
                        {
                            List<FIN_TRX_COC_DTL> lstCocDtl = finTrxListClear[0].FIN_TRX_COC_DTL.ToList();
                            foreach (FIN_TRX_COC_DTL cocDtl in lstCocDtl)
                            {
                                finTrxListClear[0].FIN_TRX_COC_DTL.Remove(cocDtl);
                            }
                        }

                        CostCenterList.Clear();
                    }
                }
            }
            else
            {
                if (CostCenterList != null && CostCenterList.Count > 0)
                {
                    if (hdfIsContCCAllocDeletion.Value != "1")
                    {
                        isContinue = false;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InvoiceNowExceeds", "$(document).ready(function(){ShowCostCenterAllocationClear(1);});", true);
                    }
                    else
                    {
                        CostCenterList.Clear();
                    }
                }
            }

            #endregion

            if (isContinue)
            {
                // ShowHideCostCenterAllocButton();
                //if (((Button)sender).ID == "btnAccount")
                //{
                //    ddlAccountType = hdfAccount.Value == "" ? 0 : Convert.ToInt32(hdfAccount.Value);
                //}
                //else
                // {
                ddlAccountType = hdfAccount1.Value == "" ? 0 : Convert.ToInt32(hdfAccount1.Value);
                // }
                chkIsBankCharge1.Checked = false;
                GetFieldValues(ControlsEnum.FINCOAMST);
                if (finCoaMstList != null && finCoaMstList.Count > 0)
                {
                    ddlAccountSubType = Convert.ToInt32(finCoaMstList[0].COA_SUB_TYPE.ToString());
                    //hdfSubTypePk1.Value = finCoaMstList[0].COA_SUB_TYPE.ToString();
                    if (finCoaMstList[0].FIN_COA_SUB_TYPE_CFG.CST_IS_BANK_CHARGE == 1 && hdfShowOthChrgChkBox.Value == "1")
                        chkIsBankCharge1.Checked = true;

                    GetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                    if (finCoaSubTypeCfgList[0].CST_REL_QUERY != null)
                    {
                        if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                        {
                            relquery = finCoaSubTypeCfgList[0].CST_REL_QUERY == null ? string.Empty : finCoaSubTypeCfgList[0].ADM_QUERIES_CFG1.QRY_QUERY;
                        }

                        if (relquery != string.Empty)
                        {
                            CommonServiceClient = new CommonService();
                            CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);

                            if (finCoaSubTypeCfgList[0].CST_CODE == "AP" || finCoaSubTypeCfgList[0].CST_CODE == "AR")
                            {
                                relquery = relquery.Replace("@COA@", ddlAccountType > 0 ? ddlAccountType.ToString() : "NULL");
                                relquery = relquery.Replace("@ADV@", "NULL");
                            }
                            else if (finCoaSubTypeCfgList[0].CST_CODE == "ADP" || finCoaSubTypeCfgList[0].CST_CODE == "ADR")
                            {
                                relquery = relquery.Replace("@COA@", "NULL");
                                relquery = relquery.Replace("@ADV@", ddlAccountType > 0 ? ddlAccountType.ToString() : "NULL");
                            }
                            else
                            {
                                relquery = relquery.Replace("@COA@", "NULL");
                                relquery = relquery.Replace("@ADV@", "NULL");
                            }
                            relquery = relquery.Replace("@PK@", "NULL");
                            relquery = relquery.Replace("@BIZUNITPK@", currentUser.SBUID.ToString());
                            SubTypesAccounts = CommonServiceClient.ExecuteQuery(relquery);
                            SetFieldValues(ControlsEnum.FINCOASUBTYPECFG);


                            #region Sub Type Selected Value
                            string defQuery = finCoaSubTypeCfgList[0].CST_DEF_QUERY == null ? string.Empty : finCoaSubTypeCfgList[0].ADM_QUERIES_CFG_DEF_QUERY.QRY_QUERY;
                            if (defQuery != string.Empty)//Have default Query
                            {
                                defQuery = defQuery.Contains(GetLocalResourceObject("DefaultQryCondtion").ToString())
                                    ? defQuery.Replace(GetLocalResourceObject("DefaultQryCondtion").ToString(), ddlAccountType.ToString()) : defQuery;
                                CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                                List<DDLMaster> defaultValuesList = CommonServiceClient.ExecuteQuery(defQuery);
                                if (defaultValuesList != null && defaultValuesList.Count == 1)
                                {
                                    ddlSubTypeAccount1.SelectedValue = (ListItem)ddlSubTypeAccount1.Items.FindByValue(defaultValuesList[0].PK.ToString()) != null
                                        ? defaultValuesList[0].PK.ToString() : CommonConstants.SELECTVAL;//Set default value to subledger ddl
                                }
                                else
                                {
                                    //ddlSubTypeAccount1.SelectedValue = CommonConstants.SELECTVAL;
                                }
                            }
                            else
                            {
                                //ddlSubTypeAccount1.SelectedValue = CommonConstants.SELECTVAL;
                            }

                            #endregion

                        }
                        else
                        {
                            ddlSubTypeAccount1.Visible = false;
                            ddlSubTypeAccount.Visible = false;
                        }
                    }
                    else
                    {
                        ddlSubTypeAccount1.Visible = false;
                        ddlSubTypeAccount.Visible = false;
                    }
                }
                else
                {
                    ddlSubTypeAccount1.Visible = false;
                    ddlSubTypeAccount.Visible = false;
                }

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPDCforCheque", "$(document).ready(function(){ShowPDCforCheque();});", true);
            }
        }

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
            if (dtApplication != null && dtApplication.Rows.Count > 0)
            {
                appId = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PK] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PK]);
            }
            return appId;
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessID(string VouchrType, int pid)
        {
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?AppType=" + VouchrType;
            else
                path = Request.Url.AbsolutePath.ToLower() + "?AppType=" + VouchrType;

            if (VoucherType == ApplicationType.JV && GetGlobalResourceObject("ConfigurationsRes", "IsShowVoucherDataImport").ToString() == "1")
            {
                path = GetGlobalResourceObject("PageURL", "JVwkffURL").ToString();
            }

            if (pid == 12)
            {
                path += "&PID=" + pid;
            }
            if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
            {
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {
                    ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();

                    //((HiddenField)this.Master.FindControl("hdfPageID")).Value = dtProcess.Rows[0][CommonConstants.F_PAGE].ToString();

                }
                base.WkfPageUrl = ucrWrkf.PageUrl = path;
            }
        }
        #endregion

        #region Enum
        #region ControlEnum
        public enum ControlsEnum
        {
            VOUCHERS,
            BASECURRENCY,
            EXCHANGERATE,
            MODE,
            FINTRXHDR,
            FINTRX,
            APLNTYPE,
            VOUCHERNO,
            FINCOAMST,
            FINCOASUBTYPECFG,
            COMPANY,
            VENDORACCOUNT,
            WHTPOPUPGRID,
            FORMNO,
            VENDORACCOUNTTAX,
            VENDOR,
            VATBUYVENDOR,
            VENDORACCOUNTVATBUYTAX,
            VATPOPUPGRID,
            VENDORCONTACTYPE,
            VATBUYTAXTYPES,
            TAXTYPECHANGED,
            PAYMENTTAXHDR,
            VENDORSELECTEDDTL,
            VENDORCONTACTYPEDETAILS,
            VENDORTYPES,
            TAXDETAILS,
            VENDORCONTACTFORWHT,
            PAYMENTTYPE,
            WHTVENDOR,
            CHEQUERETURN,
            FINHEADER,
            GETDIRECTPAYMENTPKBYJOURNALPK,
            REVERSE,
            COSTCENTER,
            CASHACCOUNT,
            DEFAULT,
            AUDITLOGSTATUS,
            AUDITLOGDETAILS
        }
        #endregion

        #region SubEnum
        public enum SubEnum
        {
            SubType = -1
        }
        #endregion

        public enum WHTFormNo
        {
            PND54 = 390
        }
        public enum PaymentModeID
        {
            Cash = 201,
            Cheque,
            DD,
            Bank,
            General,
            Others
        }
        #endregion

        //protected void grdAuditLog_RowDataBound(object sender, GridViewRowEventArgs e)
        //{

        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        Label lblAuditVersion = e.Row.FindControl("lblAuditVersion") as Label;
        //        HiddenField hdfFTH_PK = e.Row.FindControl("hdfFTH_PK") as HiddenField;
        //        HiddenField hdfRowIndex = e.Row.FindControl("hdfRowIndex") as HiddenField;
        //        e.Row.Attributes.Add("onclick", "ChangeRowColor(this,'" + lblAuditVersion.Text + "','" + hdfFTH_PK.Value + "','" + hdfRowIndex.Value + "')");

        //        //if (hdfSelRowTranPk.Value == hdfFTH_PK.Value)
        //        //    e.Row.BackColor = System.Drawing.Color.Yellow;
        //    }
        //}
    }
}

