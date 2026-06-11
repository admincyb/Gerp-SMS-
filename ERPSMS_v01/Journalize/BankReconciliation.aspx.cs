using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using System.Data;
using System.Threading;
using System.Web.UI.HtmlControls;
using BusinessLogic.Jouralize;
using BusinessObject.Journalize;

namespace ERPSMS_v01.Journalize
{
    public partial class BankReconciliation : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
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
        /// To maintain Trx PK
        /// </summary>
        private int TrxPK
        {
            get
            {
                if (this.ViewState[ViewstateStrings.TrxPK] == null)
                {
                    return 0;
                }
                else
                {
                    return (int)this.ViewState[ViewstateStrings.TrxPK];
                }
            }
            set
            {
                this.ViewState[ViewstateStrings.TrxPK] = value;
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
        private int RefPK
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.InvoiceId];
            }
            set
            {
                this.ViewState[ViewstateStrings.InvoiceId] = value;
            }
        }
        /// <summary>
        /// Invoice
        /// </summary>
        private string RefType
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.RefType];
            }
            set
            {
                this.ViewState[ViewstateStrings.RefType] = value;
            }
        }
        /// <summary>
        /// To maintain the SortExpression or sort By in viewstate
        /// </summary>
        private string AppType
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.AppType];
            }
            set
            {
                this.ViewState[ViewstateStrings.AppType] = value;
            }
        }

        /// <summary>
        /// Entry State for managing display status
        /// </summary>
        private EntryStatus EntryStatus
        {
            get
            {
                return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.LISTMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);
            }
            set
            {
                this.ViewState[ViewstateStrings.EntryState] = value;
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

        private Decimal BalanceAsPerStatement
        {
            get
            {
                return this.ViewState["BalanceAsPerStatement"] == null ? 0 : Convert.ToDecimal(this.ViewState["BalanceAsPerStatement"].ToString());
            }
            set
            {
                this.ViewState["BalanceAsPerStatement"] = value;
            }
        }
        #endregion
        private FinTrxDetails ObjFinTrxDetails;
        
        private List<FinTransactions> finTrxList;
        private FIN_TRX_HDR finTrxHdrObj;
        private ActionsEnum commonActions;
        //page related class objects      
        private FinTransactions finTrxObj;
        private FIN_CASH_BANK_MST finCashBankMstObj;
        private FIN_COA_SUB_TYPE_CFG finCoaSubTypeCfgObj;
        private ServiceUtility serviceUtilityObj;
        //List for binding details to controls
       // private List<FIN_TRX> finTrxList;
        private List<FIN_TRX_HDR> finTrxHdrList;
        private List<FIN_CASH_BANK_MST> finCashBankMstList;
        private List<FIN_YEAR_MST> finYearMstList;

        private BusinessObject.User currentUser;
        private CommonService CommonServiceClient;

        private ADM_APP_TYPE_MST admAppTypeMstObj;
        private List<ADM_APP_TYPE_MST> admAppTypeMstList;
        private List<FIN_COA_SUB_TYPE_CFG> finCoaSubTypeCfgList;

        private double gridBalance = 0;
        private double gridTotalDebit = 0;
        private double gridTotalCredit = 0;

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
            FinTrxService finTrxServiceClient = null;

            CommonService commonServiceClient;
            commonServiceClient = null;

            CommonService commonService;
            commonService = null;

            try
            {
                finTrxServiceClient = new FinTrxService();
                finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
               // finTrxObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX>();
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        //serviceUtilityObj = new ServiceUtility();
                        //serviceUtilityObj.CurrentPage = -1;
                        //serviceUtilityObj.PageSize = -1;
                        //serviceUtilityObj.FilterDate = Convert.ToDateTime(txtFromDate.Text);
                        //serviceUtilityObj.FilterToDate = Convert.ToDateTime(txtToDate.Text);
                        //serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.FinTrxPk : SortBy;
                        //serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        //int isReconciled = -1;
                        //isReconciled = chbUnReconciled.Checked && chbReconciled.Checked ? isReconciled : chbUnReconciled.Checked ? 0 : chbReconciled.Checked ? 1 : isReconciled;
                        //finTrxObj.FTR_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        //finTrxObj.FTR_TYPE = Resources.Controls.Bank;
                        //finTrxObj.FTR_TYPE_PK = Convert.ToInt32(ddlBankAccount.SelectedValue);
                        //finTrxObj.FTR_INSTR_NO = txtInstrNo.Text;
                        //decimal TotalDebit = 0;
                        //decimal TotalCredit = 0;

                        //finTrxList = finTrxServiceClient.GetBankReconcileList(finTrxObj, isReconciled, serviceUtilityObj, ref TotalDebit, ref TotalCredit);

                        //ViewState["VoucherDet"] = finTrxList;
                        //hdfAddAmount.Value=TotalDebit.ToString();
                        //hdfLessAmount.Value = TotalCredit.ToString();

                        #region SP call
                        decimal TotalDebit = 0;
                        decimal TotalCredit = 0;
                        int isReconciled = -1;
                        FinTransactions objTrx = new FinTransactions();
                        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        objTrx.BIZUNIT = currentUser.CurrentSBUPK;
                        objTrx.FROM_DATE = Convert.ToDateTime(txtFromDate.Text);
                        objTrx.TO_DATE = Convert.ToDateTime(txtToDate.Text);
                        objTrx.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.FinTrxPk : SortBy;
                        objTrx.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        objTrx.FTR_TYPE = Resources.Controls.Bank;
                        objTrx.FTR_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        objTrx.FTR_TYPE_PK = Convert.ToInt32(ddlBankAccount.SelectedValue);
                        objTrx.FTR_INSTR_NO = txtInstrNo.Text;

                        isReconciled = chbUnReconciled.Checked && chbReconciled.Checked ? isReconciled : chbUnReconciled.Checked ? 0 : chbReconciled.Checked ? 1 : isReconciled;
                        ObjFinTrxDetails = JournalizeBL.GetBankReconcileList(objTrx, isReconciled, ref TotalDebit, ref TotalCredit);
                        hdfAddAmount.Value = TotalDebit.ToString();
                        hdfLessAmount.Value = TotalCredit.ToString();
                        ViewState["VoucherDet"] = ObjFinTrxDetails.FinTrx;
                        #endregion

                        break;
                    case ControlsEnum.FINPERIOD:
                        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        finYearMstList = finTrxServiceClient.GetCurrentFinPeriod(DateTime.Now, currentUser.SBUID);
                        break;
                    case ControlsEnum.BANKACCOUNTS:
                        finCashBankMstObj = ERP.Utilities.CommonFunctions.Initilize<FIN_CASH_BANK_MST>();
                        BankMstService bankMstServiceClient = new BankMstService();
                        bankMstServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(bankMstServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        serviceUtilityObj.SortBy = Resources.DataFieldRes.BankCode;
                        serviceUtilityObj.SortDirection = Resources.ErpRes.SortAscending;
                        serviceUtilityObj.FilterValue = string.Empty;
                        finCashBankMstObj.CBM_PK = 0;
                        finCashBankMstObj.CBM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finCashBankMstObj.CBM_TYPE = Convert.ToByte(DbActiveStatus.HASPK);
                        finCashBankMstList = bankMstServiceClient.GetFinCashBankMstAutoCompleteList(finCashBankMstObj, serviceUtilityObj);
                        bankMstServiceClient = null;
                        break;
                    #region FIN HEADER
                    case ControlsEnum.FINHEADER:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);

                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdVouchers.PageSize;
                        serviceUtilityObj.SortBy = Resources.DataFieldRes.VoucherNo;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortAscending : SortDirection;
                        serviceUtilityObj.FilterBy = HttpUtility.HtmlEncode(string.Empty);
                        serviceUtilityObj.FilterDate = (DateTime?)null;
                        serviceUtilityObj.FilterToDate = (DateTime?)null;

                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_REF_TYPE = Session[ERP.Utilities.SessionStrings.Type] == null ? "" : Session[ERP.Utilities.SessionStrings.Type].ToString();
                        finTrxHdrObj.FTH_REF_PK = hdfTrxRefPk.Value == "" ? 0 : Convert.ToInt32(hdfTrxRefPk.Value);
                        finTrxHdrObj.FTH_PK = TrxPK == 0 ? -1 : TrxPK;
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrList = finTrxServiceClient.GetfinTxtHdrList(finTrxHdrObj, serviceUtilityObj);
                        break;
                    #endregion  
                    #region JOURNALIZATION TYPE
                    case ControlsEnum.JOURNALIZATIONTYPE:
                        commonServiceClient = new CommonService();
                        commonServiceClient = CommonFunctions.InitiateClient(commonServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        admAppTypeMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_APP_TYPE_MST>();
                        admAppTypeMstObj.APT_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admAppTypeMstObj.APT_SPL_COND = "J";
                        admAppTypeMstList = commonServiceClient.GetAppTypeValues(admAppTypeMstObj);
                        commonServiceClient = null;
                        break;
                    #endregion  
                    #region FIN COA SUB TYPE CONFIG VALUES
                    case ControlsEnum.FINCOASUBTYPECFG:
                        commonService = new CommonService();
                        commonService = CommonFunctions.InitiateClient(commonService);
                        finCoaSubTypeCfgObj = new FIN_COA_SUB_TYPE_CFG();
                        finCoaSubTypeCfgObj = CommonFunctions.Initilize<FIN_COA_SUB_TYPE_CFG>();
                        finCoaSubTypeCfgObj.CST_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finCoaSubTypeCfgObj.CST_PK = hdfSubTypePk.Value == "" ? 0 : Convert.ToInt32(hdfSubTypePk.Value);
                        finCoaSubTypeCfgList = commonService.GetSubTypeCfgValues(finCoaSubTypeCfgObj);
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
                finTrxObj = null;
                serviceUtilityObj = null;
                finTrxServiceClient = null;
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
                    case ControlsEnum.DEFAULT:
                        BindGrid();
                        CalcClosingBalance();
                        break;
                    case ControlsEnum.FINPERIOD:
                        if (finYearMstList != null && finYearMstList.Count > 0)
                        {
                            txtFromDate.Text = finYearMstList[0].FYR_DATE_FROM.ToString("dd-MMM-yyyy");
                            txtToDate.Text = finYearMstList[0].FYR_DATE_TO.ToString("dd-MMM-yyyy");
                        }
                        break;
                    case ControlsEnum.BANKACCOUNTS:
                        BindDropDown();
                        break;
                    case ControlsEnum.BALANCE:
                        CalcClosingBalance();
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
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessID(string Type, int refPK)
        {
            string path = string.Empty;
            int urlType = 1;
            if (refPK > 0)
            {
                FinTrxService finTrxServiceClient;
                finTrxServiceClient = null;
                finTrxServiceClient = new FinTrxService();
                finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                int retVal = finTrxServiceClient.GetFinType(Type, refPK);
                finTrxServiceClient = null;
                if (retVal > 0)
                {
                    urlType = retVal;
                }
            }
            else
            {
                path = GetLocalResourceObject("JVPath").ToString().ToLower();
            }
            switch (Type)
            {
                case ApplicationType.JV:
                    path = GetLocalResourceObject("JVPath").ToString().ToLower();
                    break;
                case ApplicationType.PCS:
                    path = GetLocalResourceObject("PCSPath").ToString().ToLower();
                    break;
                case ApplicationType.VPJ:
                    path = GetLocalResourceObject("VPJPath").ToString().ToLower();
                    break;
                case ApplicationType.CRJ:
                    path = GetLocalResourceObject("CRJPath").ToString().ToLower();
                    break;
                case ApplicationType.CNJ:
                    if (urlType == 1)
                        path = GetLocalResourceObject("CNJPath1").ToString().ToLower();
                    else if (urlType == 2)
                        path = GetLocalResourceObject("CNJPath2").ToString().ToLower();
                    break;
                case ApplicationType.DNJ:
                    if (urlType == 1)
                        path = GetLocalResourceObject("DNJPath1").ToString().ToLower();
                    else if (urlType == 2)
                        path = GetLocalResourceObject("DNJPath2").ToString().ToLower();
                    break;
                case ApplicationType.PIJ:
                    if (urlType == 1)
                        path = GetLocalResourceObject("PIJPath1").ToString().ToLower();
                    else if (urlType == 2)
                        path = GetLocalResourceObject("PIJPath2").ToString().ToLower();
                    break;
                case ApplicationType.SIJ:
                    if (urlType == 1)
                        path = GetLocalResourceObject("SIJPath1").ToString().ToLower();
                    else if (urlType == 2)
                        path = GetLocalResourceObject("SIJPath2").ToString().ToLower();
                    break;
                case ApplicationType.EIJ:
                    path = GetLocalResourceObject("EIJPath").ToString().ToLower();
                    break;
                case ApplicationType.PDCCJ:
                    path = GetLocalResourceObject("PDCCJPath").ToString().ToLower();
                    break;
                case ApplicationType.PPCCJ:
                    path = GetLocalResourceObject("PPCCJPath").ToString().ToLower();
                    break;
            }
            if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
            {
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {
                    PageProcessID = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                    base.WkfPageUrl = path;
                    ucrWrkf.PageUrl = path;
                    //base.WkfPageType = (int)PageTypeEnum.Listing;
                    ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                }
            }
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
                    ucrWrkf.PageUrl = path;
                    ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                    //if (pid == 1)
                    //{
                    PageProcessID = ucrWrkf.ProcessID;

                    //}
                    base.WkfPageUrl = path;
                }
            }
        }

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>       
        //private List<FIN_TRX> SetUIValuesToObjectOld()
        //{
        //    try
        //    {
        //        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        //        //hdfAddAmount.Value = "0";
        //        //hdfLessAmount.Value = "0";

        //        finTrxList = ViewState["VoucherDet"] != null ? (List<FIN_TRX>)ViewState["VoucherDet"] : finTrxList;

        //        foreach (GridViewRow grw in grdVouchers.Rows)
        //        {
        //            TextBox txtClearingDate = (TextBox)grw.FindControl("txtClearingDate");
        //            CheckBox chbSave = (CheckBox)grw.FindControl("chbSelect");
        //            long ftrPK = Convert.ToInt64(grdVouchers.DataKeys[grw.RowIndex].Values[0].ToString());
        //            finTrxObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX>();
        //            List<FIN_TRX> ftrList = (from ftr in finTrxList
        //                                     where ftr.FTR_PK == ftrPK
        //                                     select ftr).ToList();
        //            finTrxObj = ftrList[0];
        //            finTrxList.Remove(finTrxObj);

        //            finTrxObj.FTR_PK = ftrPK;
        //            if (chbSave.Checked)
        //            {
        //                //hdfAddAmount.Value = (Convert.ToDecimal(hdfAddAmount.Value) + finTrxObj.FTR_DR_AMT_BC).ToString();
        //                //hdfLessAmount.Value = (Convert.ToDecimal(hdfLessAmount.Value) + finTrxObj.FTR_CR_AMT_BC).ToString();
        //                if (!string.IsNullOrEmpty(txtClearingDate.Text.Trim()))
        //                    finTrxObj.FTR_CLEAR_DATE = Convert.ToDateTime(txtClearingDate.Text);
        //                else
        //                    finTrxObj.FTR_CLEAR_DATE = null;
        //            }
        //            else
        //            {
        //                finTrxObj.FTR_CLEAR_DATE = null;
        //            }
        //            finTrxObj.FTR_IS_RECONCILED = chbSave.Checked;
        //            finTrxObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
        //            finTrxObj.FTR_MOD_DT = DateTime.Now;
        //            finTrxList.Add(finTrxObj);
        //        }

        //        return finTrxList;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        finTrxObj = null;
        //    }
        //}
        private List<FinTransactions> SetUIValuesToObject()
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                //hdfAddAmount.Value = "0";
                //hdfLessAmount.Value = "0";

                finTrxList = ViewState["VoucherDet"] != null ? (List<FinTransactions>)ViewState["VoucherDet"] : finTrxList;

                foreach (GridViewRow grw in grdVouchers.Rows)
                {
                    TextBox txtClearingDate = (TextBox)grw.FindControl("txtClearingDate");
                    CheckBox chbSave = (CheckBox)grw.FindControl("chbSelect");
                    long ftrPK = Convert.ToInt64(grdVouchers.DataKeys[grw.RowIndex].Values[0].ToString());
                    finTrxObj = new FinTransactions();
                    List<FinTransactions> ftrList = (from ftr in finTrxList
                                             where ftr.FTR_PK == ftrPK
                                             select ftr).ToList();
                    finTrxObj = ftrList[0];
                    finTrxList.Remove(finTrxObj);

                    finTrxObj.FTR_PK = ftrPK;
                    if (chbSave.Checked)
                    {
                        //hdfAddAmount.Value = (Convert.ToDecimal(hdfAddAmount.Value) + finTrxObj.FTR_DR_AMT_BC).ToString();
                        //hdfLessAmount.Value = (Convert.ToDecimal(hdfLessAmount.Value) + finTrxObj.FTR_CR_AMT_BC).ToString();
                        if (!string.IsNullOrEmpty(txtClearingDate.Text.Trim()))
                            finTrxObj.FTR_CLEAR_DATE = Convert.ToDateTime(txtClearingDate.Text);
                        else
                            finTrxObj.FTR_CLEAR_DATE = null;
                    }
                    else
                    {
                        finTrxObj.FTR_CLEAR_DATE = null;
                    }
                    finTrxObj.FTR_IS_RECONCILED = chbSave.Checked==true ? 1 : 0;
                    finTrxObj.FTR_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                    finTrxObj.FTR_MOD_DT = DateTime.Now;
                    finTrxList.Add(finTrxObj);
                }

                return finTrxList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                finTrxObj = null;
            }
        }

        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        //////private void GetUIValuesFromObject()
        //////{
        //////    try
        //////    {
        //////        //assigning the UI controls with the corresponding ListObject value AsrSovMstList
        //////        if (accountMstList != null && accountMstList.Count() > 0)
        //////        {
        //////            CurrPK = accountMstList[0].COA_PK;
        //////            txtCoaCode.Text = HttpUtility.HtmlDecode(accountMstList[0].COA_CODE);
        //////            ddlCoaParent.SelectedIndex = Convert.ToInt32(ddlCoaParent.Items.IndexOf(ddlCoaParent.Items.FindByValue(accountMstList[0].COA_PARENT.ToString())));
        //////            txtCoaName.Text = HttpUtility.HtmlDecode(accountMstList[0].COA_NAME);
        //////            txtCoaShortName.Text = HttpUtility.HtmlDecode(accountMstList[0].COA_SHORT_NAME);
        //////            txtCoaDesc.Text = HttpUtility.HtmlDecode(accountMstList[0].COA_DESC);
        //////            ddlCoaType.SelectedIndex = Convert.ToInt32(ddlCoaType.Items.IndexOf(ddlCoaType.Items.FindByValue(accountMstList[0].COA_TYPE.ToString())));
        //////            chkIsGroup.Checked = Convert.ToBoolean(accountMstList[0].COA_IS_GROUP.ToString());
        //////            ddlSubType.SelectedIndex = Convert.ToInt32(ddlSubType.Items.IndexOf(ddlSubType.Items.FindByValue(accountMstList[0].COA_SUB_TYPE.ToString())));
        //////            txtSequence.Text = accountMstList[0].COA_SEQUENCE.ToString();
        //////            txtRemarks.Text = HttpUtility.HtmlDecode(accountMstList[0].COA_REMARKS);
        //////            lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
        //////            LastModifiedTime = accountMstList[0].COA_MOD_DT;
        //////        }
        //////        //Concurrency Account details Deleted By Another User
        //////        else
        //////        {
        //////            litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
        //////            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Accounts);
        //////            EntryStatus = EntryStatus.LISTMODE;
        //////            ResetForm();
        //////            GetFieldValues(ControlsEnum.DEFAULT);
        //////            SetFieldValues(ControlsEnum.DEFAULT);
        //////            ModifiedDatePnl.Visible = false;
        //////            throw new Exception(litErrorMsg.Text);
        //////        }
        //////    }
        //////    catch (Exception ex)
        //////    {
        //////        throw ex;
        //////    }
        //////}

        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid()
        {
            try
            {
                hdfOpeningBal.Value = "0";
                hdfRunningBal.Value = "0";
                //hdfAddAmount.Value = "0";
                //hdfLessAmount.Value = "0";
                /* new
                if (finTrxList != null)
                {
                    hdfOpeningBal.Value = (finTrxList[0].FTR_CR_AMT_BC == 0 ? finTrxList[0].FTR_DR_AMT_BC : -finTrxList[0].FTR_CR_AMT_BC).ToString();
                    hdfRunningBal.Value = hdfOpeningBal.Value;
                    gridBalance = Convert.ToDouble(hdfRunningBal.Value);
                    finTrxList.RemoveAt(0);
                }
                grdVouchers.DataSource = finTrxList;
                grdVouchers.DataBind();
                Totalwrap1.Visible = true;
                */
                //if (finTrxList == null || finTrxList.Count == 0)
                //{
                //    Totalwrap1.Visible = true;
                //}
                //else
                //{
                //    Totalwrap1.Visible = true;
                //}


                if (ObjFinTrxDetails != null && ObjFinTrxDetails.FinTrx!=null)
                {
                    hdfOpeningBal.Value = (ObjFinTrxDetails.FinTrx[0].FTR_CR_AMT_BC == 0 ? ObjFinTrxDetails.FinTrx[0].FTR_DR_AMT_BC : -ObjFinTrxDetails.FinTrx[0].FTR_CR_AMT_BC).ToString();
                    hdfRunningBal.Value = hdfOpeningBal.Value;
                    gridBalance = Convert.ToDouble(hdfRunningBal.Value);
                    ObjFinTrxDetails.FinTrx.RemoveAt(0);
                    BalanceAsPerStatement = (ObjFinTrxDetails.FinTrx[0].FTR_CR_AMT_BC == 0 ? ObjFinTrxDetails.FinTrx[0].FTR_DR_AMT_BC : -ObjFinTrxDetails.FinTrx[0].FTR_CR_AMT_BC);
                    ObjFinTrxDetails.FinTrx.RemoveAt(0);
                }
                grdVouchers.DataSource = ObjFinTrxDetails.FinTrx;
                grdVouchers.DataBind();
                Totalwrap1.Visible = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method for BankAccount DropDown
        /// </summary>
        public void BindDropDown()
        {
            ddlBankAccount.Items.Clear();
            if (finCashBankMstList != null && finCashBankMstList.Count > 0)
            {
                ddlBankAccount.DataSource = finCashBankMstList;
                ddlBankAccount.DataTextField = Resources.DataFieldRes.BankName;
                ddlBankAccount.DataValueField = Resources.DataFieldRes.BankPK;
                ddlBankAccount.DataBind();
            }
            ddlBankAccount.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
        }

        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            //////try
            //////{
            //////    PreEntryStatus = EntryStatus;
            //////    foreach (GridViewRow grdrow in grdVouchers.Rows)
            //////    {
            //////        RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
            //////        // check row selected or not
            //////        if (rbtn.Checked)
            //////        {
            //////            // get pk from the grid and assign to CurrPk
            //////            CurrPK = Convert.ToInt16(grdVouchers.DataKeys[grdrow.RowIndex].Values[0]);
            //////            // Get And Set the Location details
            //////            GetFieldValues(ControlsEnum.BANKACCOUNTS);
            //////            SetFieldValues(ControlsEnum.BANKACCOUNTS);
            //////            if (Mode == ActionsEnum.VIEW)
            //////                EntryStatus = EntryStatus.VIEWMODE;
            //////            else
            //////                EntryStatus = EntryStatus.ENTRYMODE;
            //////            return;
            //////        }
            //////    }

            //////    // if no items selected, Show Error Message
            //////    litErrorMsg.Text = Resources.Messages.Msg_Not_Selected;
            //////    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
            //////    EntryStatus = EntryStatus.LISTMODE;
            //////}
            //////catch (Exception ex)
            //////{
            //////    throw ex;
            //////}
        }

        private void CalcClosingBalance()
        {
            //if (txtInstrNo.Text == string.Empty)
            {
                //BalanceAsPerStatement
               // lblOpngBlnc.Text = String.Format(Thread.CurrentThread.CurrentCulture, "{0:c}", Convert.ToDouble(hdfOpeningBal.Value));
                lblOpngBlnc.Text = String.Format(Thread.CurrentThread.CurrentCulture, "{0:c}", BalanceAsPerStatement);

                lblDeposits.Text = hdfAddAmount.Value == string.Empty ? "0" : hdfAddAmount.Value;
                lblUnPrsntdCheques.Text = hdfLessAmount.Value == string.Empty ? "0" : hdfLessAmount.Value;
               // lblClosingBlnc.Text = String.Format(Thread.CurrentThread.CurrentCulture, "{0:c}", (Convert.ToDouble(hdfOpeningBal.Value) + (lblDeposits.Text == string.Empty ? 0 : Convert.ToDouble(lblDeposits.Text)) - (lblUnPrsntdCheques.Text == string.Empty ? 0 : Convert.ToDouble(lblUnPrsntdCheques.Text))));
                lblClosingBlnc.Text = String.Format(Thread.CurrentThread.CurrentCulture, "{0:c}", (Convert.ToDouble(BalanceAsPerStatement) + (lblDeposits.Text == string.Empty ? 0 : Convert.ToDouble(lblDeposits.Text)) - (lblUnPrsntdCheques.Text == string.Empty ? 0 : Convert.ToDouble(lblUnPrsntdCheques.Text))));

                lblUnPrsntdCheques.Text = String.Format(Thread.CurrentThread.CurrentCulture, "{0:c}", Convert.ToDouble(lblUnPrsntdCheques.Text));
                lblDeposits.Text = String.Format(Thread.CurrentThread.CurrentCulture, "{0:c}", Convert.ToDouble(lblDeposits.Text));
            }
        }

        /// <summary>
        /// Method used to Reset form Controls
        /// </summary>
        private void ResetForm()
        {
            // txtFromDate.Text = string.Empty;
            // txtToDate.Text = string.Empty;
            ddlBankAccount.SelectedIndex = 0;
            txtInstrNo.Text = string.Empty;

            lblOpngBlnc.Text = "0";
            lblDeposits.Text = "0";
            lblUnPrsntdCheques.Text = "0";
            lblClosingBlnc.Text = "0";
            chbUnReconciled.Checked = true;
            chbReconciled.Checked = false;
            Totalwrap1.Visible = false;
            GetFieldValues(ControlsEnum.FINPERIOD);
            SetFieldValues(ControlsEnum.FINPERIOD);
            grdVouchers.DataSource = null;
            grdVouchers.DataBind();
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

            FinTrxService finTrxServiceClient;
            finTrxServiceClient = null;
            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
            bool bIsChecked = false;
            Session[ERP.Utilities.SessionStrings.TrxPK] = null;

            try
            {
                long result;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                switch (commonActions)
                {
                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
                        #region Journalize Old
                        //RefPK = 0;
                        //RefType = ApplicationType.JV;
                        //Session[ERP.Utilities.SessionStrings.SubTypeDDr] = null;
                        //Session[ERP.Utilities.SessionStrings.SubTypeCCr] = null;
                        //Session[ERP.Utilities.SessionStrings.SubTypeCr] = null;
                        //Session[ERP.Utilities.SessionStrings.SubTypeDr] = null;
                        //Session[ERP.Utilities.SessionStrings.JournalMode] = null;
                        ////Journalize New sessions start
                        //Session[ERP.Utilities.SessionStrings.DrControls] = null;
                        //Session[ERP.Utilities.SessionStrings.CrControls] = null;
                        //Session[ERP.Utilities.SessionStrings.RemovedControls] = null;
                        //Session[ERP.Utilities.SessionStrings.AccountType] = null;
                        //Session[ERP.Utilities.SessionStrings.ControlInfo] = null;
                        //Session[ERP.Utilities.SessionStrings.FinTrxPk] = null;
                        ////Journalize New sessions End
                        //ucrJournalize.TransactionType = RefType;
                        //Session[ERP.Utilities.SessionStrings.TransactionType] = RefType;
                        //ucrJournalize.TransactionPK = RefPK;
                        //Session[ERP.Utilities.SessionStrings.TransactionPK] = null;
                        //ucrJournalize.JournalizePK = 0;
                        //Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                        //Session[ERP.Utilities.SessionStrings.TransactionNo] = null;
                        //Session[ERP.Utilities.SessionStrings.TransactionDate] = null;
                        //Session[ERP.Utilities.SessionStrings.TransactionCurrency] = null;
                        //Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.VND;
                        //Session[ERP.Utilities.SessionStrings.AccountPayablePK] = "";
                        //Session[ERP.Utilities.SessionStrings.JournalType] = RefType;
                        //ucrWrkf.WrkfSubmit -= ActionHandler;
                        //ucrWrkf.Reset();
                        //ucrWrkf.ViewType = 1;

                        //FillProcessID(ApplicationType.JV,0);                            
                        //EntryStatus = EntryStatus.ENTRYMODE;                           
                        //ucrWrkf.RefID = 0;

                        //ucrWrkf.FillWorkFlowDetails();
                        //if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                        //{
                        //    ucrWrkf.ViewType = 1;
                        //    //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.NEWMODE;
                        //}
                        //else
                        //{
                        //    ucrWrkf.ViewType = 0;
                        //    //EntryStatus = EntryStatus.VIEWMODE;
                        //    //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                        //}
                        //ucrJournalize.HasWkfPermission = ucrWrkf.HasPageTaskPermission;
                        //ucrWrkf.ViewAction();

                        //HiddenField hdfExchangeRateJV = (HiddenField)ucrJournalize.FindControl("hdfExchangeRateJV");
                        //hdfExchangeRateJV.Value = "";

                        //TextBox txtJournalExchangeRate = (TextBox)ucrJournalize.FindControl("txtJournalExchangeRate");
                        //txtJournalExchangeRate.Text = "";

                        //TextBox txtNarration = (TextBox)ucrJournalize.FindControl("txtNarration");
                        //txtNarration.Text = "";


                        //Session[ERP.Utilities.SessionStrings.JournalHead] = GetLocalResourceObject("Journal_Voucher").ToString();
                        //ucrJournalize.CallUserControl();

                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                        #endregion

                        RefPK = 0;
                        ucrJournalize.VoucherTemplatePK = 0;
                        RefType = ApplicationType.JV;
                        Session[ERP.Utilities.SessionStrings.SubTypeDDr] = null;
                        Session[ERP.Utilities.SessionStrings.SubTypeCCr] = null;
                        Session[ERP.Utilities.SessionStrings.SubTypeCr] = null;
                        Session[ERP.Utilities.SessionStrings.SubTypeDr] = null;
                        Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.NEWMODE;
                        //Journalize New sessions start
                        Session[ERP.Utilities.SessionStrings.DrControls] = null;
                        Session[ERP.Utilities.SessionStrings.CrControls] = null;
                        Session[ERP.Utilities.SessionStrings.RemovedControls] = null;
                        Session[ERP.Utilities.SessionStrings.AccountType] = null;
                        Session[ERP.Utilities.SessionStrings.ControlInfo] = null;
                        Session[ERP.Utilities.SessionStrings.FinTrxPk] = null;
                        //Journalize New sessions End
                        ucrJournalize.TransactionType = RefType;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = RefType;
                        ucrJournalize.TransactionPK = RefPK;
                        Session[ERP.Utilities.SessionStrings.TransactionPK] = null;
                        ucrJournalize.JournalizePK = 0;
                        Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                        Session[ERP.Utilities.SessionStrings.TransactionNo] = null;
                        Session[ERP.Utilities.SessionStrings.TransactionDate] = null;
                        Session[ERP.Utilities.SessionStrings.TransactionCurrency] = null;
                        Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.VND;
                        Session[ERP.Utilities.SessionStrings.AccountPayablePK] = "";
                        Session[ERP.Utilities.SessionStrings.JournalType] = RefType;
                        ucrWrkf.WrkfSubmit -= ActionHandler;
                        ucrWrkf.Reset();
                        ucrWrkf.ViewType = 1;

                        FillProcessID(ApplicationType.JV, 0);
                        EntryStatus = EntryStatus.ENTRYMODE;
                        base.WkfRefID = ucrWrkf.RefID = 0;

                        ucrWrkf.FillWorkFlowDetails();

                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                        {
                            ucrWrkf.ViewType = 1;
                            //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.NEWMODE;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            //EntryStatus = EntryStatus.VIEWMODE;
                            //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                        }

                        ucrJournalize.HasWkfPermission = ucrWrkf.HasPageTaskPermission;
                        ucrWrkf.ViewAction();

                        HiddenField hdfExchangeRateJV = (HiddenField)ucrJournalize.FindControl("hdfExchangeRateJV");
                        hdfExchangeRateJV.Value = "";

                        TextBox txtJournalExchangeRate = (TextBox)ucrJournalize.FindControl("txtJournalExchangeRate");
                        txtJournalExchangeRate.Text = "";

                        TextBox txtNarration = (TextBox)ucrJournalize.FindControl("txtNarration");
                        txtNarration.Text = "";

                        TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                        WrkfComments.Text = "";
                        ucrJournalize.CallUserControl();
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.JournalHead] = GetLocalResourceObject("Journal_Voucher").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                        break;
                    #endregion

                    #region Save
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else//valid
                        {
                            finTrxServiceClient = new FinTrxService();
                            finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                            finTrxList = SetUIValuesToObject();
                            ObjFinTrxDetails = new FinTrxDetails();
                            ObjFinTrxDetails.FinTrx = finTrxList;
                            string xmlDoc = CommonFunctions.XmlSerialize<FinTrxDetails>(ObjFinTrxDetails);
                            // save Process Control inspection details
                            result = BusinessLogic.Jouralize.JournalizeBL.SaveBankReconciliation(xmlDoc);
                            if (result >= 0) // Success ! re-initialize the page
                            {
                                litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.BankReconciliation);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                GetFieldValues(ControlsEnum.DEFAULT);
                                SetFieldValues(ControlsEnum.DEFAULT);
                                SetFieldValues(ControlsEnum.BALANCE);
                            }
                        }
                        break;
                    #endregion

                    #region Search
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Cancel
                    case ActionsEnum.CANCEL:
                        ResetForm();
                        //GetFieldValues(ControlsEnum.DEFAULT);
                        //SetFieldValues(ControlsEnum.DEFAULT); 
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Voucher Popup
                    case ActionsEnum.POPUPADD:

                        GridViewRow grw = (GridViewRow)((LinkButton)(sender)).Parent.Parent;
                        if (grw != null)
                        {
                            string company = string.Empty;
                            AppType = ((HiddenField)grw.FindControl("hdfAppType")).Value;
                            RefPK = Convert.ToInt32(((HiddenField)grw.FindControl("hdfRefPK")).Value);
                            TrxPK = Convert.ToInt32(((HiddenField)grw.FindControl("hdfTrxPK")).Value);
                            RefType = ((HiddenField)grw.FindControl("hdfRefType")).Value;
                            company = ((HiddenField)grw.FindControl("hdfCompany")).Value;

                            /////
                            //if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.JV) Commented due to Bug ID:  24282(voucher print is not working for direct payment,direct receipt,petty cash vouchers )
                            if (RefType == ApplicationType.JV || RefType == ApplicationType.DPVJ || RefType == ApplicationType.DRVJ || RefType == ApplicationType.PCVJ)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID="
                                    + TrxPK.ToString() + "&APPTYPE=" + AppType +
                                    "&APPSUBTYPE=" + hdfAppSubType.Value + "&TRXTYPE=" + AppType
                                    + "&COMPANY=" + company + "');", true);
                            }
                            else
                            {

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID="
                                    + RefPK.ToString() + "&APPTYPE=" + AppType +
                                    "&APPSUBTYPE=" + hdfAppSubType.Value + "&TRXTYPE=" + RefType + "&COMPANY=" + company + "');", true);// 

                            }
                        }

                        #region OLD
                        /////

                        //Session[ERP.Utilities.SessionStrings.TrxPK] = TrxPK;
                        //Session[ERP.Utilities.SessionStrings.SubTypeDDr] = null;
                        //Session[ERP.Utilities.SessionStrings.SubTypeCCr] = null;
                        //Session[ERP.Utilities.SessionStrings.SubTypeCr] = null;
                        //Session[ERP.Utilities.SessionStrings.SubTypeDr] = null;
                        //Session[ERP.Utilities.SessionStrings.JournalMode] = null;
                        //ucrJournalize.TransactionType = RefType;
                        //Session[ERP.Utilities.SessionStrings.TransactionType] = RefType;
                        //ucrJournalize.TransactionPK = RefPK;
                        //Session[ERP.Utilities.SessionStrings.Type] = RefType;

                        ////Journalize New sessions start
                        //Session[ERP.Utilities.SessionStrings.DrControls] = null;
                        //Session[ERP.Utilities.SessionStrings.CrControls] = null;
                        //Session[ERP.Utilities.SessionStrings.RemovedControls] = null;
                        //Session[ERP.Utilities.SessionStrings.AccountType] = null;
                        //Session[ERP.Utilities.SessionStrings.ControlInfo] = null;
                        //Session[ERP.Utilities.SessionStrings.FinTrxPk] = null;
                        ////Journalize New sessions End

                        //if (RefPK > 0)
                        //{                                
                        //    Session[ERP.Utilities.SessionStrings.TransactionPK] = RefType== ApplicationType.JV ? null : RefPK.ToString();
                        //    ucrJournalize.JournalizePK = TrxPK;
                        //    Session[ERP.Utilities.SessionStrings.JournalizePK] = TrxPK;
                        //    Session[ERP.Utilities.SessionStrings.TransactionNo] = "";
                        //    Session[ERP.Utilities.SessionStrings.TransactionDate] = "";
                        //    Session[ERP.Utilities.SessionStrings.TransactionCurrency] = "";
                        //    Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.VND;
                        //    Session[ERP.Utilities.SessionStrings.AccountPayablePK] = "";
                        //}
                        //else
                        //{
                        //    Session[ERP.Utilities.SessionStrings.TransactionPK] = null;
                        //    Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                        //    Session[ERP.Utilities.SessionStrings.TransactionNo] = null;
                        //    Session[ERP.Utilities.SessionStrings.TransactionDate] = null;
                        //    Session[ERP.Utilities.SessionStrings.TransactionCurrency] = null;
                        //    Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.VND;
                        //    Session[ERP.Utilities.SessionStrings.AccountPayablePK] = "";
                        //}

                        //if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.OBV)
                        //{
                        //    Response.Redirect(Resources.PageURL.OpeningBalance);
                        //}
                        //else
                        //{
                        //    Session[ERP.Utilities.SessionStrings.JournalType] = RefType;
                        //    ucrWrkf.WrkfSubmit -= ActionHandler;
                        //    ucrWrkf.Reset();
                        //    ucrWrkf.ViewType = 1;

                        //    FillProcessID(Session[ERP.Utilities.SessionStrings.Type].ToString(), RefPK);

                        //    hdfTrxRefPk.Value = RefPK.ToString();

                        //    GetFieldValues(ControlsEnum.FINHEADER);
                        //    EntryStatus = EntryStatus.ENTRYMODE;

                        //    ucrWrkf.RefID = workflowCore.GetRefID((int)finTrxHdrList[0].FTH_PK, ucrWrkf.ProcessID);
                        //    ucrWrkf.FillWorkFlowDetails();
                        //    //if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                        //    //{
                        //    //    ucrWrkf.ViewType = 1;
                        //    //    Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                        //    //}
                        //    //else
                        //    //{
                        //        ucrWrkf.ViewType = 0;
                        //        EntryStatus = EntryStatus.VIEWMODE;
                        //        Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                        //    //}

                        //    Session[ERP.Utilities.SessionStrings.RemoveRowIndex] = null;
                        //    ucrJournalize.CallUserControl();
                        //    EntryStatus = EntryStatus.LISTMODE;

                        //    GetFieldValues(ControlsEnum.JOURNALIZATIONTYPE);
                        //    if (Session[ERP.Utilities.SessionStrings.Type] != null)
                        //    {
                        //        admAppTypeMstObj = admAppTypeMstList.SingleOrDefault(app => app.APT_CODE == Session[ERP.Utilities.SessionStrings.Type].ToString());
                        //        if (admAppTypeMstObj != null)
                        //        {                                    
                        //            Session[ERP.Utilities.SessionStrings.JournalHead] = admAppTypeMstObj.APT_NAME.ToString();
                        //        }
                        //    }

                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowDivNoOverlay('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                        //}
                        #endregion
                        break;
                    #endregion
                    #region Journalize Update
                    case ActionsEnum.JOURNALIZEUPDATE:
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        ucrJournalize.ResetForm();
                        ucrWrkf.Reset();
                        FillProcessID();
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.BALANCE);
                        break;
                    #endregion
                    #region Journalize Save
                    case ActionsEnum.JOURNALIZESAVE:
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        ucrWrkf.Reset();
                        FillProcessID();
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.BALANCE);
                        break;
                    #endregion
                    #region Journalize Submit
                    case ActionsEnum.JOURNALIZESUBMIT:
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        ucrWrkf.Reset();
                        FillProcessID();
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.BALANCE);
                        break;
                    #endregion
                    #region Journalize Delete
                    case ActionsEnum.JOURNALIZEDELETE:
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        ucrWrkf.Reset();
                        FillProcessID();
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.BALANCE);
                        break;
                    #endregion
                    #region Journalize Cancel
                    case ActionsEnum.JOURNALIZECANCEL:
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        ucrWrkf.Reset();
                        FillProcessID();
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.BALANCE);
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
                finTrxServiceClient = null;
            }
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            CommonService commonService;
            commonService = null;
            try
            {
                string relquery = string.Empty;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    #region Enable Required Validator Script

                    CheckBox chkSlct = (CheckBox)e.Row.FindControl("chbSelect");
                    RequiredFieldValidator vrfClrDate = (RequiredFieldValidator)e.Row.FindControl("vrfClearDate");
                    chkSlct.Attributes.Add("onclick", string.Format("setValidationState(this, '{0}');", vrfClrDate.ClientID));
                    // chkSlct.Attributes["onclick"] = string.Format("EnableValidator('{0}', this.checked);", vrfClrDate.ClientID);
                    #endregion
                    Label lblDr = (Label)e.Row.FindControl("lblDebit");
                    Label lblCr = (Label)e.Row.FindControl("lblCredit");
                    Label lblBal = (Label)e.Row.FindControl("lblBalance");
                    CheckBox chbSelect = (CheckBox)e.Row.FindControl("chbSelect");
                    HiddenField hdfRefPK = (HiddenField)e.Row.FindControl("hdfRefPK");

                    if (!chbSelect.Checked)
                    {
                        double balance = Convert.ToDouble(hdfRunningBal.Value) + Convert.ToDouble(lblDr.Text) - Convert.ToDouble(lblCr.Text);
                        hdfRunningBal.Value = balance.ToString();

                        //hdfAddAmount.Value = (Convert.ToDouble(hdfAddAmount.Value) + Convert.ToDouble(lblDr.Text)).ToString();
                        //hdfLessAmount.Value = (Convert.ToDouble(hdfLessAmount.Value) + Convert.ToDouble(lblCr.Text)).ToString();
                    }

                    gridBalance = (gridBalance + Convert.ToDouble(lblDr.Text)) - Convert.ToDouble(lblCr.Text);

                    lblBal.Text = String.Format(Thread.CurrentThread.CurrentCulture, "{0:c}", Convert.ToDecimal(gridBalance));
                    lblCr.Text = String.Format("{0:c}", decimal.Parse(lblCr.Text.Replace(",", "")));
                    lblDr.Text = String.Format("{0:c}", decimal.Parse(lblDr.Text.Replace(",", "")));

                    gridTotalDebit += Convert.ToDouble(lblDr.Text);
                    gridTotalCredit+= Convert.ToDouble(lblCr.Text);

                    //LinkButton lbnVoucherNo = (LinkButton)e.Row.FindControl("lbnVoucherNo");
                    //AppType = ((HiddenField)e.Row.FindControl("hdfAppType")).Value;
                    //lbnVoucherNo.Attributes.Add("OnClick", "ShowPopup('" + AppType + "')");

                    Session[ERP.Utilities.SessionStrings.Type] = ((HiddenField)e.Row.FindControl("hdfRefType")).Value;
                    hdfTrxRefPk.Value = hdfRefPK.Value;
                    TrxPK = 0;
                    //GetFieldValues(ControlsEnum.FINHEADER);

                    //if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                    //{
                    //    foreach (var fintrx in finTrxHdrList[0].FIN_TRX.ToList())
                    //    {
                    //        if (fintrx.FTR_TYPE == ApplicationType.AR || fintrx.FTR_TYPE == ApplicationType.AP || fintrx.FTR_TYPE == ApplicationType.ADP || fintrx.FTR_TYPE == ApplicationType.ADR)
                    //        {
                    //            Label lblParty = (Label)e.Row.FindControl("lblParty");
                    //            hdfSubTypePk.Value = fintrx.FTR_ACC_SUB_TYPE.ToString();
                    //            GetFieldValues(ControlsEnum.FINCOASUBTYPECFG);
                    //            if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
                    //            {
                    //                relquery = finCoaSubTypeCfgList[0].CST_REL_QUERY == null ? string.Empty : finCoaSubTypeCfgList[0].ADM_QUERIES_CFG1.QRY_QUERY;
                    //            }
                    //            if (relquery != string.Empty)
                    //            {
                    //                commonService = new CommonService();
                    //                commonService = CommonFunctions.InitiateClient(commonService);
                    //                List<DDLMaster> ddlChildValues = commonService.ExecuteQuery(relquery);
                    //                lblParty.Text = ERP.Utilities.CommonFunctions.GetShortString(ddlChildValues.SingleOrDefault(val => val.PK == fintrx.FTR_TYPE_PK).Value, 15);
                    //                lblParty.ToolTip = ddlChildValues.SingleOrDefault(val => val.PK == fintrx.FTR_TYPE_PK).Value;
                    //            }
                    //        }
                    //    }
                    //}



                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblBalTotal = (Label)e.Row.FindControl("lblClosBal");
                    Label lblTotalDebit = (Label)e.Row.FindControl("lblTotalDebit");
                    Label lblTotalCredit = (Label)e.Row.FindControl("lblTotalCredit");
                    //lblBalTotal.Text = String.Format(Thread.CurrentThread.CurrentCulture, "{0:c}", (Convert.ToDouble(hdfOpeningBal.Value) +
                    //    Convert.ToDouble(hdfAddAmount.Value) - Convert.ToDouble(hdfLessAmount.Value)));
                    lblBalTotal.Text = String.Format(Thread.CurrentThread.CurrentCulture, "{0:c}", gridBalance);
                    lblTotalDebit.Text = String.Format(Thread.CurrentThread.CurrentCulture, "{0:c}", gridTotalDebit);
                    lblTotalCredit.Text = String.Format(Thread.CurrentThread.CurrentCulture, "{0:c}", gridTotalCredit);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
                commonService = null;
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
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            this.btnAdd.PreRender += new EventHandler(btnAction_PreRender);

            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnCancel.Load += new EventHandler(btnAction_Load);
            this.btnAdd.Load += new EventHandler(btnAction_Load);
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
            string breadCrumb;
            breadCrumb = this.GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
            lblBreadCrum.Text = breadCrumb;
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
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
                ucrWrkf.ViewType = 1;

                ucrJournalize.JournalizeSave += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeSubmit += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeDelete += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeCancel += new EventHandler(ActionHandler);

                if (!IsPostBack)
                {
                    FillProcessID();
                    Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = Resources.DataFieldRes.FinTrxPk;
                    grdVouchers.DataKeyNames = datakeyarray;
                    GetFieldValues(ControlsEnum.FINPERIOD);
                    SetFieldValues(ControlsEnum.FINPERIOD);
                    GetFieldValues(ControlsEnum.BANKACCOUNTS);
                    SetFieldValues(ControlsEnum.BANKACCOUNTS);
                    //GetFieldValues(ControlsEnum.DEFAULT);
                    // SetFieldValues(ControlsEnum.DEFAULT);
                    EntryStatus = EntryStatus.LISTMODE;

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region ControlEnum
        public enum ControlsEnum
        {
            DEFAULT,
            NEWDEFAULT,
            BANKACCOUNTS,
            FINPERIOD,
            BALANCE,
            JOURNALIZE,
            FINHEADER,
            JOURNALIZATIONTYPE,
            FINCOASUBTYPECFG
        }
        #endregion
    }
}