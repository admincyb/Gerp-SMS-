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

namespace ERPSMS_v01.Journalize
{
    public partial class VoucherOld : ERP.Store.UI.MyBasePage
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
        #endregion

        private ActionsEnum commonActions;
        //page related class objects      
        private FIN_TRX_HDR finTrxHdrObj;
        private FIN_TRX finTrxObj;
        private FIN_COA_MST finCoaMstObj;
        private ADM_CONFIG_MST admConfigMstObj;
        private FIN_VOUCHER_ENTRY_CFG finVcrEntryCfgObj;
        private FIN_COA_SUB_TYPE_CFG finCoaSubTypeCfgObj;
        private ServiceUtility serviceUtilityObj;
        //List for binding details to controls
        private List<FIN_TRX_HDR> finTrxHdrList;
        private List<FIN_TRX> finTrxList;
        private List<FIN_COA_MST> finCoaMstList;
        private List<ADM_CONFIG_MST> admConfigMstList;
        private List<FIN_VOUCHER_ENTRY_CFG> finVcrEntryCfgList;
        private List<FIN_COA_SUB_TYPE_CFG> finCoaSubTypeCfgList;

        List<DDLMaster> SubTypesAccounts;

        private BusinessObject.User currentUser;
        private CommonService CommonServiceClient;
        
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;

        private string refID;
        private string inboxFlag;
        private string voucherNo;

        private bool postflag = false;
        private bool updateVocher;

        private double DrTotal = 0;
        private double CrTotal = 0;
        private byte selectedModeValue = 0;

        private int ddlAccountType = 0;
        private int ddlAccountSubType = 0;

        DataTable dtCmp;

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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            try
            {
                FinTrxServiceClient = new FinTrxService();
                FinTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(FinTrxServiceClient);
                switch (type)
                {
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
                        string BaseCurrency = CurrencyMstServiceClient.GetCurrencyCodeName(hdfCurrency.Value == string.Empty ? 0 : Convert.ToInt32(hdfCurrency.Value));
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
                        voucherNo = poInvoiceServiceClient.GetInvoiceNo(VoucherType, 0, 1, DateTime.Now, currentUser.PKUser, true, 0);
                        hdfVoucherNo.Value = voucherNo;
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
                        dtCmp = BusinessLogic.CommonManagement.CommonBL.GetDeptCompany(currentUser.CurrentDeptPK);
                        break;

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
                    case ControlsEnum.VOUCHERS:
                        GetUIValuesFromObject();
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
                        if (dtCmp != null && dtCmp.Rows.Count > 0)
                            CompanyPK = Convert.ToInt32(dtCmp.Rows[0][Resources.DataFieldRes.DeptCompany].ToString());
                        else
                            CompanyPK = 0;
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
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private object SetUIValuesToObject(ActionsEnum mode, Object srcObj)
        {
            try
            {
                object returnObj;
                returnObj = null;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                int s = 0;

                switch (mode)
                {
                    #region Save
                    case ActionsEnum.SAVE:
                        finTrxHdrObj = CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_PK = CurrPK;
                        //FTH_NARRATION FTH_FIN_YEAR FTH_IS_JRNLD FTH_STATUS
                        finTrxHdrObj.FTH_DATE = string.IsNullOrEmpty(txtVoucherDate.Text.Trim()) ? DateTime.Now : DateTime.Parse(txtVoucherDate.Text.Trim());
                        
                        txtVoucherNo.Text = hdfVoucherNo.Value;
                        finTrxHdrObj.FTH_VOUCHER_NO = txtVoucherNo.Text;
                        finTrxHdrObj.FTH_REF_TYPE = VoucherType;
                        finTrxHdrObj.FTH_REF_PK = hdfVecPk.Value == string.Empty ? 0 : Convert.ToInt64(hdfVecPk.Value);
                        finTrxHdrObj.FTH_REF_NO = txtRefNo.Text.Trim();
                        finTrxHdrObj.FTH_REF_DATE = string.IsNullOrEmpty(txtRefDate.Text.Trim()) ? DateTime.Now : DateTime.Parse(txtRefDate.Text.Trim());
                        finTrxHdrObj.FTH_NARRATION = txtNarration.Text;
                        finTrxHdrObj.FTH_TRX_CURR = Convert.ToInt32(hdfCurrency.Value);
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        finTrxHdrObj.FTH_EXCHG_RATE = Convert.ToDouble(txtExchangeRate.Text);
                        finTrxHdrObj.FTH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                        finTrxHdrObj.FTH_FIN_YEAR = 0;
                        finTrxHdrObj.FTH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                        //For Type OBV
                        finTrxHdrObj.FTH_STATUS = 2;
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrObj.FTH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                        finTrxHdrObj.FTH_CRTD_DT = DateTime.Now;
                        finTrxHdrObj.FTH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                        finTrxHdrObj.FTH_MOD_DT = LastModifiedTime;
                        finTrxHdrObj.FTH_DEPT = Convert.ToInt16(Session[BusinessObject.Common.SessionStrings.CurDept].ToString());
                        finTrxHdrObj.FTH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        finTrxHdrObj.FTH_IS_JRNLD = true;
                        if (CompanyPK > 0)
                        {
                            finTrxHdrObj.FTH_COMPANY = CompanyPK;
                        }
                        else
                        {
                            finTrxHdrObj.FTH_COMPANY = Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE);
                        }

                        finTrxList = new List<FIN_TRX>();
                        List<FIN_TRX> ftrList=new List<FIN_TRX>();

                        if (ViewState["VoucherDet"] != null) finTrxList = (List<FIN_TRX>)(ViewState["VoucherDet"]);

                        foreach (FIN_TRX ftrObj in finTrxList)
                        {
                            finTrxObj = CommonFunctions.Initilize<FIN_TRX>();

                            finTrxObj.FTR_PK = CurrFtrPK;
                            finTrxObj.FTR_TRX_HDR = CurrPK;
                            finTrxObj.FTR_SEQUENCE = (short)s;
                            finTrxObj.FTR_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                            finTrxObj.FTR_CRTD_DT = DateTime.Now;
                            finTrxObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                            finTrxObj.FTR_MOD_DT = DateTime.Now;
                            finTrxObj.FTR_REMARKS = string.Empty;
                            finTrxObj.FTR_ACC_SUB_TYPE = ftrObj.FTR_ACC_SUB_TYPE;
                            finTrxObj.FTR_ACCOUNT = ftrObj.FTR_ACCOUNT;
                            finTrxObj.FTR_ACTIVE = ftrObj.FTR_ACTIVE;
                            finTrxObj.FTR_BIZUNIT = ftrObj.FTR_BIZUNIT;
                            finTrxObj.FTR_CLEAR_DATE = ftrObj.FTR_CLEAR_DATE;
                            finTrxObj.FTR_CR_AMT_BC = ftrObj.FTR_CR_AMT_BC;
                            finTrxObj.FTR_CR_AMT_TC = ftrObj.FTR_CR_AMT_TC;
                            finTrxObj.FTR_DEPT = ftrObj.FTR_DEPT;
                            finTrxObj.FTR_DR_AMT_BC = ftrObj.FTR_DR_AMT_BC;
                            finTrxObj.FTR_DR_AMT_TC = ftrObj.FTR_DR_AMT_TC;
                            finTrxObj.FTR_INSTR_DATE = ftrObj.FTR_INSTR_DATE;
                            finTrxObj.FTR_INSTR_FAVOUR = ftrObj.FTR_INSTR_FAVOUR;
                            finTrxObj.FTR_INSTR_NO = ftrObj.FTR_INSTR_NO;
                            finTrxObj.FTR_IS_RECONCILED = ftrObj.FTR_IS_RECONCILED;
                            finTrxObj.FTR_NARRATION = ftrObj.FTR_NARRATION;
                            finTrxObj.FTR_PAYMENT_MODE = ftrObj.FTR_PAYMENT_MODE;
                            finTrxObj.FTR_TYPE = ftrObj.FTR_TYPE;
                            finTrxObj.FTR_TYPE_PK = ftrObj.FTR_TYPE_PK;

                            ftrList.Add(finTrxObj);
                            s++;
                        }

                        if (ftrList != null && ftrList.Count > 0)
                        {
                            ftrList.ForEach(dtl => finTrxHdrObj.FIN_TRX.Add(dtl));
                        }
                        break;
                    #endregion

                    #region Workflow Submit
                    case ActionsEnum.WRKFSUBMIT:
                        //finTrxHdrObj = finTrxHdrList[0];
                        finTrxHdrObj = CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_PK = CurrPK;
                        updateVocher = true;
                        txtVoucherNo.Text = hdfVoucherNo.Value;
                        finTrxHdrObj.FTH_VOUCHER_NO = txtVoucherNo.Text;
                        finTrxHdrObj.FTH_DATE = DateTime.Parse(txtVoucherDate.Text);
                        finTrxHdrObj.FTH_REF_TYPE = VoucherType;
                        finTrxHdrObj.FTH_REF_PK = hdfVecPk.Value == string.Empty ? 0 : Convert.ToInt64(hdfVecPk.Value);
                        finTrxHdrObj.FTH_REF_DATE = DateTime.Parse(txtRefDate.Text);
                        finTrxHdrObj.FTH_REF_NO = txtRefNo.Text;
                        finTrxHdrObj.FTH_NARRATION = txtNarration.Text;
                        finTrxHdrObj.FTH_TRX_CURR = Convert.ToInt32(hdfCurrency.Value);
                        finTrxHdrObj.FTH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                        finTrxHdrObj.FTH_FIN_YEAR = 0;
                        finTrxHdrObj.FTH_REMARKS = txtRemarks.Text;
                        finTrxHdrObj.FTH_STATUS = 2;
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrObj.FTH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                        finTrxHdrObj.FTH_CRTD_DT = DateTime.Now;
                        finTrxHdrObj.FTH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                        finTrxHdrObj.FTH_MOD_DT = LastModifiedTime;
                        finTrxHdrObj.FTH_DEPT = currentUser.CurrentDeptPK;
                        finTrxHdrObj.FTH_BIZUNIT = currentUser.SBUID;
                        finTrxHdrObj.FTH_IS_JRNLD = true;
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        finTrxHdrObj.FTH_EXCHG_RATE = double.Parse(txtExchangeRate.Text);
                        if (CompanyPK > 0)
                        {
                            finTrxHdrObj.FTH_COMPANY = CompanyPK;
                        }
                        else
                        {
                            finTrxHdrObj.FTH_COMPANY = Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE);
                        }

                        finTrxList = new List<FIN_TRX>();
                        if (ViewState["VoucherDet"] != null) finTrxList = (List<FIN_TRX>)(ViewState["VoucherDet"]);

                        foreach (FIN_TRX ftrObj in finTrxList)
                        {
                            ftrObj.FTR_PK = CurrFtrPK;
                            ftrObj.FTR_TRX_HDR = CurrPK;
                            ftrObj.FTR_SEQUENCE = (short)s;
                            ftrObj.FTR_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                            ftrObj.FTR_CRTD_DT = DateTime.Now;
                            ftrObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                            ftrObj.FTR_MOD_DT = DateTime.Now;
                            ftrObj.FTR_REMARKS = string.Empty;
                            ftrObj.ADM_CONFIG_MST = null;
                            ftrObj.FIN_COA_MST = null;

                            s++;
                        }

                        if (finTrxList != null && finTrxList.Count > 0)
                        {
                            finTrxList.ForEach(dtl => finTrxHdrObj.FIN_TRX.Add(dtl));
                        }
                        break;
                    #endregion

                    default:
                        break;
                }
                returnObj = finTrxHdrObj;
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

        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject()
        {
            try
            {
                if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                {
                    CurrPK = Convert.ToInt32(finTrxHdrList[0].FTH_PK);

                    hdfVoucherNo.Value = finTrxHdrList[0].FTH_VOUCHER_NO;
                    txtVoucherNo.Text = finTrxHdrList[0].FTH_VOUCHER_NO == string.Empty ? "[NEW]" : finTrxHdrList[0].FTH_VOUCHER_NO;
                    txtVoucherDate.Text = Convert.ToDateTime(finTrxHdrList[0].FTH_DATE).ToString("dd-MMM-yyyy");
                    txtRefNo.Text = finTrxHdrList[0].FTH_REF_NO;
                    txtRefDate.Text = finTrxHdrList[0].FTH_REF_DATE.ToString("dd-MMM-yyyy");
                    hdfCurrency.Value = finTrxHdrList[0].FTH_BASE_CURR.ToString();
                    GetFieldValues(ControlsEnum.BASECURRENCY);
                    txtExchangeRate.Text = finTrxHdrList[0].FTH_EXCHG_RATE.ToString();
                    txtRemarks.Text = finTrxHdrList[0].FTH_REMARKS;
                    LastModifiedTime = finTrxHdrList[0].FTH_MOD_DT;

                    finTrxList = new List<FIN_TRX>();
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
                        admConfigMstObj.CFG_PK = ftrObj.ADM_CONFIG_MST.CFG_PK;
                        admConfigMstObj.CFG_DATA = ftrObj.ADM_CONFIG_MST.CFG_DATA;
                        ftrObj.ADM_CONFIG_MST = admConfigMstObj;

                        finTrxList.Add(ftrObj);
                    }

                    ViewState["VoucherDet"] = finTrxList;
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
            ddlMode.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));

            ddlMode1.Items.Clear();
            if (admConfigMstList != null && admConfigMstList.Count > 0)
            {
                ddlMode1.DataSource = admConfigMstList;
                ddlMode1.DataTextField = Resources.DataFieldRes.ConfigName;
                ddlMode1.DataValueField = Resources.DataFieldRes.ConfigPK;
                ddlMode1.DataBind();
            }
            ddlMode1.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
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
                ddlSubTypeAccount1.DataSource = SubTypesAccounts;
                ddlSubTypeAccount1.DataTextField = "Value";
                ddlSubTypeAccount1.DataValueField = "PK";
                ddlSubTypeAccount1.DataBind();

                ddlSubTypeAccount.DataSource = SubTypesAccounts;
                ddlSubTypeAccount.DataTextField = "Value";
                ddlSubTypeAccount.DataValueField = "PK";
                ddlSubTypeAccount.DataBind();
            }
            ddlSubTypeAccount1.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
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

                finTrxList = (List<FIN_TRX>)(ViewState["VoucherDet"]);
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
                        txtInstrNo.Text = finTrxList[0].FTR_INSTR_NO;
                        txtInstrDate.Text = finTrxList[0].FTR_INSTR_DATE != null ? Convert.ToDateTime(finTrxList[0].FTR_INSTR_DATE).ToString("dd-MMM-yyyy") : string.Empty;
                        txtFavourOf.Text = finTrxList[0].FTR_INSTR_FAVOUR;

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

                        if (finTrxList[0].FTR_TYPE_PK != null)
                        {
                            ddlSubTypeAccount1.SelectedValue = finTrxList[0].FTR_TYPE_PK.ToString();
                            ddlSubTypeAccount1.Visible = true;
                        }
                    }
                    else
                    {
                        hdfAccount1.Value = finTrxList[0].FTR_ACCOUNT.ToString();
                        txtAccount1.Text = HttpUtility.HtmlDecode(finTrxList[0].FIN_COA_MST.COA_NAME);
                        ddlMode1.SelectedValue = finTrxList[0].FTR_PAYMENT_MODE.ToString();
                        if (VoucherType == ApplicationType.OBV)
                            ddlMode1.Enabled = false;
                        else
                            ddlMode1.Enabled = true;

                        txtNarration1.Text = HttpUtility.HtmlDecode(finTrxList[0].FTR_NARRATION);
                        txtDebit1.Text = finTrxList[0].FTR_DR_AMT_BC == 0 ? string.Empty : finTrxList[0].FTR_DR_AMT_BC.ToString();
                        txtCredit1.Text = finTrxList[0].FTR_CR_AMT_BC == 0 ? string.Empty : finTrxList[0].FTR_CR_AMT_BC.ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowInstrDetails", "ShowInstrDetails1();", true);
                        txtInstrNo1.Text = finTrxList[0].FTR_INSTR_NO;
                        txtInstrDate1.Text = finTrxList[0].FTR_INSTR_DATE != null ? Convert.ToDateTime(finTrxList[0].FTR_INSTR_DATE).ToString("dd-MMM-yyyy") : string.Empty;
                        txtFavourOf1.Text = finTrxList[0].FTR_INSTR_FAVOUR;

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
                        if (finTrxList[0].FTR_TYPE_PK != null)
                        {
                            ddlSubTypeAccount1.SelectedValue = finTrxList[0].FTR_TYPE_PK.ToString();
                            ddlSubTypeAccount1.Visible = true;
                        }
                    }
                }
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
                txtVoucherNo.Text = "[NEW]";
                txtVoucherDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                txtRefNo.Text = string.Empty;
                txtRefDate.Text = string.Empty;
                //txtCurrency.Text = string.Empty;
                //txtExchangeRate.Text = string.Empty;
                txtRemarks.Text = string.Empty;

                CurrPK = 0;
                CurrFtrPK = 0;
                ViewState["VoucherDet"] = null;

                GetFieldValues(ControlsEnum.APLNTYPE);
                SetFieldValues(ControlsEnum.APLNTYPE);
            }

            if (Section == 1 || Section == 2)
            {
                hdfAccount.Value = string.Empty;
                txtAccount.Text = string.Empty;
                ddlMode.SelectedIndex = 0;
                txtNarration.Text = string.Empty;
                txtAmount.Text = string.Empty;
                ddlSubTypeAccount.Visible = false;
            }

            if (Section == 1 || Section == 3)
            {
                hdfAccount1.Value = string.Empty;
                txtAccount1.Text = string.Empty;
                ddlSubTypeAccount1.Visible = false;
                if (VoucherType == ApplicationType.OBV)
                {
                    txtNarration1.Text = Resources.ErpRes.OpeningBalance;
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
                else
                {
                    txtNarration1.Text = string.Empty;
                    ddlMode1.Enabled = true;
                    ddlMode1.SelectedIndex = 0;
                }
                txtDebit1.Text = string.Empty;
                txtCredit1.Text = string.Empty;
            }
        }

        private void SetVoucherTypeValues()
        {
            if (finVcrEntryCfgList != null && finVcrEntryCfgList.Count > 0)
            {
                Page.Title = finVcrEntryCfgList[0].VEC_DISPLAY_NAME;
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
        #endregion

        #region ActionHandler
        /// <summary>
        /// Handling control events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            FinTrxService FinTrxServiceClient;
            AccountMstService FinCoaMstServiceClient;
            ADM_CONFIG_MST tempAdmConfigMstObj;
            FinTrxServiceClient = null;
            FinCoaMstServiceClient = null;
            string relquery=string.Empty ;

            CommonService commonService;
            commonService = null;

            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int accountPk;
                long result;
                string action;

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
                switch (commonActions)
                {
                    #region SAVE
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        if (hdfDebitTotal.Value != hdfCreditTotal.Value)
                        {
                            litErrorMsg.Text = this.GetLocalResourceObject("Err_Total").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
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
                                finTrxList = new List<FIN_TRX>();
                                finTrxHdrList = new List<FIN_TRX_HDR>();
                                FinTrxServiceClient = new FinTrxService();
                                FinTrxServiceClient = CommonFunctions.InitiateClient(FinTrxServiceClient);
                                finTrxHdrObj = CommonFunctions.Initilize<FIN_TRX_HDR>();
                                if (hdfVoucherNo.Value == string.Empty && AST_DOC_MODE.Value == "1")
                                {
                                    GetFieldValues(ControlsEnum.VOUCHERNO);
                                    txtVoucherNo.Text = hdfVoucherNo.Value;
                                }
                                finTrxHdrObj = (FIN_TRX_HDR)SetUIValuesToObject(ActionsEnum.SAVE, ControlsEnum.FINTRXHDR);
                                if (finTrxHdrObj != null)
                                {
                                    finTrxHdrList.Add(finTrxHdrObj);
                                    result = FinTrxServiceClient.SaveFinTrx(finTrxHdrList);
                                    if (result >= 0)
                                    {
                                        litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Voucher);
                                        if (VoucherType == ApplicationType.OBV)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" +
                                                Page.ResolveClientUrl(Resources.PageURL.OpeningBalanceList) + "');", true);
                                        }
                                        GetFieldValues(ControlsEnum.FINTRX);
                                        SetFieldValues(ControlsEnum.FINTRX);
                                    }
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Invoice").ToString();
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
                        ResetForm(1);
                        GetFieldValues(ControlsEnum.MODE);
                        SetFieldValues(ControlsEnum.MODE);
                        this.txtRefNo.Focus();
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
                        SetUIEditView(grw);
                        break;
                    #endregion

                    #region Delete Item From Grid
                    case ActionsEnum.GRIDDELETE:
                        GridViewRow grow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;

                        finTrxList = (List<FIN_TRX>)(ViewState["VoucherDet"]);
                        CurrFtrPK = Convert.ToInt32(grdVoucher.DataKeys[grow.RowIndex].Values[0]);

                        List<FIN_TRX> ftrxList = (from ftr in finTrxList
                                                  where ftr.FTR_PK == CurrFtrPK
                                                  select ftr).ToList();

                        if (ftrxList.Count > 0) finTrxList.Remove(ftrxList[0]);

                        ViewState["VoucherDet"] = finTrxList;
                        CurrFtrPK = 0;

                        BindGrid();
                        break;
                    #endregion

                    #region Add Item To Grid
                    case ActionsEnum.ADDLITEM:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
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
                            if (ddlSubTypeAccount1.Visible)
                            {
                                if (ddlSubTypeAccount1.SelectedValue == "-1")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage( GetLocalResourceObject("Err_SubType").ToString() ) + "');", true);
                                    return;
                                }
                            }
                            finTrxList = ViewState["VoucherDet"] == null ? new List<FIN_TRX>() : finTrxList = (List<FIN_TRX>)(ViewState["VoucherDet"]);

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
                                    finTrxObj = ftrList[0];
                                    finCoaMstObj = ftrList[0].FIN_COA_MST;
                                    finTrxList.Remove(finTrxObj);
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

                                ResetForm(2);
                                this.txtAccount.Focus();
                                #endregion
                            }
                            else
                            {
                                #region Adding items from second section
                                accountPk = Convert.ToInt32(hdfAccount1.Value);

                                admConfigMstObj.CFG_PK = Convert.ToInt32(ddlMode1.SelectedValue);
                                admConfigMstObj.CFG_DATA = ddlMode1.SelectedItem.Text;
                                // CFG_ACTIVE is set as 2 for identifying the item added from 2nd section
                                admConfigMstObj.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.HASPK);

                                finTrxObj.FTR_PAYMENT_MODE = Convert.ToInt32(ddlMode1.SelectedValue);
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

                                tempAdmConfigMstObj = admConfigMstObj;
                                ResetForm(3);
                                admConfigMstObj = tempAdmConfigMstObj;
                                this.txtAccount1.Focus();
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
                            finCoaMstList = FinCoaMstServiceClient.GetFinCoaMst(finCoaMstObj, serviceUtilityObj);

                            finTrxObj.FTR_ACCOUNT = accountPk;
                            if (CurrPK == 0)
                            {
                                finTrxObj.FIN_COA_MST = finCoaMstList[0];
                            }
                            finTrxObj.FTR_ACC_SUB_TYPE = finCoaMstList[0].COA_SUB_TYPE;

                            hdfSubTypePk.Value = finTrxObj.FTR_ACC_SUB_TYPE.ToString();
                            ddlAccountSubType = finTrxObj.FTR_ACC_SUB_TYPE;
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
                            finTrxObj.FTR_DEPT = Convert.ToInt16(Session[BusinessObject.Common.SessionStrings.CurDept].ToString());
                            finTrxObj.FTR_BIZUNIT = currentUser.SBUID;

                            finTrxList.Add(finTrxObj);

                            ViewState["VoucherDet"] = finTrxList;
                            CurrFtrPK = 0;

                            BindGrid();
                        }
                        break;
                    #endregion

                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDiv('#divWkfSubmit','" + Resources.ErpRes.Submit + "','950','400');", true);
                        break;
                    case ActionsEnum.WRKFSUBMIT:
                        //Submit Activity 
                        finTrxList = new List<FIN_TRX>();
                        finTrxHdrList = new List<FIN_TRX_HDR>();
                        FinTrxServiceClient = new FinTrxService();
                        FinTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(FinTrxServiceClient);
                        if (hdfVoucherNo.Value == string.Empty)
                        {
                            GetFieldValues(ControlsEnum.VOUCHERNO);
                        }
                        finTrxHdrObj = (FIN_TRX_HDR)SetUIValuesToObject(ActionsEnum.WRKFSUBMIT, new List<FIN_TRX_HDR>());
                        if (finTrxHdrObj != null)
                        {
                            finTrxHdrList.Add(finTrxHdrObj);
                            result = FinTrxServiceClient.SaveFinTrx(finTrxHdrList);
                            if (result > 0)// Save Success ! do WorkFlow
                            {
                                //Workflow submission
                                ucrWrkf.ApplicationID = (int)result;
                                ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                //Do WorkFlow if WorkFlow has Actions
                                if (ddlWkfAction.Items.Count > 0)
                                {
                                    action = ddlWkfAction.SelectedItem.ToString();
                                    result = ucrWrkf.DoWorkFlow();
                                    if (result > 0)
                                    {
                                        Session[ERP.Utilities.SessionStrings.Transaction] = "SAVE";
                                        WrkfComments.Text = "";
                                        //Show Save success message and reset Contract Entry
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                                        object[] args = new object[2];
                                        args[0] = Resources.PageNameRes.Journalize;
                                        args[1] = txtVoucherNo.Text;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divJournalize]','Jounalize','1000','550');", true);

                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
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
                                    litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }

                        break;
                    #endregion

                    #region SUBTYPE
                    case ActionsEnum.CHANGE :
                        if (((Button)sender).ID == "btnAccount")
                        {
                            ddlAccountType = hdfAccount.Value == "" ? 0 : Convert.ToInt32(hdfAccount.Value);
                        }
                        else
                        {
                            ddlAccountType = hdfAccount1.Value == "" ? 0 : Convert.ToInt32(hdfAccount1.Value);
                        }
                               
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
                commonService = null;
            }
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblDr = (Label)e.Row.FindControl("lblDebit");
                    DrTotal += Convert.ToDouble(lblDr.Text);

                    Label lblCr = (Label)e.Row.FindControl("lblCredit");
                    CrTotal += Convert.ToDouble(lblCr.Text);
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblDrTotal = (Label)e.Row.FindControl("lblDebitTotal");
                    lblDrTotal.Text = DrTotal.ToString("c");
                    hdfDebitTotal.Value = DrTotal.ToString();

                    Label lblCrTotal = (Label)e.Row.FindControl("lblCreditTotal");
                    lblCrTotal.Text = CrTotal.ToString("c");
                    hdfCreditTotal.Value = CrTotal.ToString();
                }
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
            //////this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            //////this.btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            //////this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            //////this.btnView.PreRender += new EventHandler(btnAction_PreRender);
            //////this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            //////this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
        }

        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {
            //////base.CheckBtnVisibility(sender);
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
            if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
            {
                if (Session[ERP.Utilities.SessionStrings.Transaction].ToString() == "CANCEL")
                {
                    EntryStatus = EntryStatus.LISTMODE;
                    Session[ERP.Utilities.SessionStrings.Transaction] = null;
                }
            }

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

            lblBreadCrum.Text = Page.Title;
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
                if (Session[ERP.Utilities.SessionStrings.JOURNALIZETAB_SELECTED_PK] == null)
                {
                    ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                }

                ucrWrkf.ViewType = 1;

                if (!IsPostBack)
                {

                    grdVoucher.DataSource = null ;
                    grdVoucher.DataBind();

                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);

                    AST_DOC_MODE.Value = "0";
                    if (Request.QueryString["AppType"] != null)
                        VoucherType = Request.QueryString["AppType"].ToString();
                    else
                        VoucherType = ApplicationType.BNP;
                    currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                    hdfVoucherType.Value = VoucherType;
                    txtVoucherNo.Text = "[NEW]";
                    txtVoucherDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    txtRefDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfCurrency.Value = (currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency).ToString();

                    FillProcessID();
                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                    ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                    ucrWrkf.FillWorkFlowDetails();

                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                        : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;
                    //If Has RefID (from Inbox)
                    if (!string.IsNullOrEmpty(refID))
                    {
                        if (!string.IsNullOrEmpty(inboxFlag))
                        {
                            ucrWrkf.ViewType = 0;
                            EntryStatus = EntryStatus.VIEWMODE;
                            btnSave.Visible = false;
                            btnSubmit.Visible = false;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        ucrWrkf.RefID = int.Parse(refID);
                        CurrPK = GetApplicationID(ucrWrkf.RefID);
                    }

                    if (CurrPK > 0)
                    {
                        AST_DOC_MODE.Value = "0";
                        //////SetUIEditView(commonActions);
                        ModifiedDatePnl.Visible = true;
                        //////GetFieldValues(ControlsEnum.FINTRX);
                        finTrxHdrObj = CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_PK = CurrPK;
                        //////GetFieldValues(ControlsEnum.FINTRXHDR);
                        //////GetUIValuesFromObject(ControlsEnum.FINTRX);
                    }
                    else
                    {
                        postflag = true;
                        //Sets data key for the gird
                        string[] datakeyarray;
                        datakeyarray = new string[1];
                        datakeyarray[0] = Resources.DataFieldRes.FinTrxPk;
                        grdVoucher.DataKeyNames = datakeyarray;

                        AST_DOC_MODE.Value = GetDOCMODE();

                        GetFieldValues(ControlsEnum.APLNTYPE);
                        SetFieldValues(ControlsEnum.APLNTYPE);
                        GetFieldValues(ControlsEnum.BASECURRENCY);
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        GetFieldValues(ControlsEnum.MODE);
                        SetFieldValues(ControlsEnum.MODE);
                        //VoucherType = Session[ERP.Utilities.SessionStrings.TransactionType] == null ? "" : Session[ERP.Utilities.SessionStrings.TransactionType].ToString();
                        CurrPK = Session[ERP.Utilities.SessionStrings.TrxPK] == null ? 0 : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TrxPK].ToString());
                        if (CurrPK > 0)
                        {
                            GetFieldValues(ControlsEnum.VOUCHERS);
                            SetFieldValues(ControlsEnum.VOUCHERS);
                        }
                        if (VoucherType == ApplicationType.OBV)
                        {
                            txtNarration1.Text = Resources.ErpRes.OpeningBalance;
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

                    if (Session[ERP.Utilities.SessionStrings.JournalMode] != null)
                    {
                        EntryStatus = (EntryStatus)(Enum.Parse(typeof(EntryStatus), Session[ERP.Utilities.SessionStrings.JournalMode].ToString()));
                        if (EntryStatus == EntryStatus.VIEWMODE)
                        {
                            btnSave.Visible = false;
                        }
                        else
                        {
                            btnSave.Visible = true;
                        }
                    }
                    

                }
                Session[ERP.Utilities.SessionStrings.RefID] = null;
                Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
            if (dtApplication != null && dtApplication.Rows.Count > 0)
            {
                appId = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PK] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PK]);
            }
            return appId;
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessID()
        {
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();

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
            }
        }
        #endregion

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
            COMPANY
        }
        #endregion

        #region SubEnum
        public enum SubEnum
        {
            SubType = -1
        }
        #endregion
    }
}

