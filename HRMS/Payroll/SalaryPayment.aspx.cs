using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPSMS_v01.UserControls;
using BusinessObject.AccountManagement;
using System.Data;
using BusinessObject.HRMS.Payroll;
using BusinessObject.CommonManagement;
using BusinessLogic.HRMS.Admin.Masters;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject;
using BusinessLogic.HRMS.Payroll;
using BusinessLogic.HRMS.Employee;
using System.Threading;
using System.Text;
using System.IO;
using ERP.Utilities.HRMS;
using BusinessLogic.CommonManagement;
using ERPData;
using ERPManager;
using ERPService;
using HRMS.BackgroundTasks;
using ERPSMS_v01;

namespace HRMS.Payroll
{
    public partial class SalaryPayment : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region  Properties
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
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private int PageIndex
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.PageIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndex] = value;
            }
        }
        private int PageSize
        {
            get
            {
                return Convert.ToInt32(GetLocalResourceObject("PageSize").ToString());
            }
        }
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
        /// To keep details in view satate
        /// </summary>
        private List<SalaryPaymentModeDetails> SalaryPaymentModeDetailList
        {
            get
            {
                return (List<SalaryPaymentModeDetails>)ViewState[ViewstateStrings.SalaryPaymentModeDetailList];
            }
            set
            {
                ViewState[ViewstateStrings.SalaryPaymentModeDetailList] = value;
            }
        }

        /// <summary>
        /// To keep details in view satate
        /// </summary>
        private List<SalaryPaymentDetails> EmpSalaryDetailsPopupList
        {
            get
            {
                return (List<SalaryPaymentDetails>)ViewState[ViewstateStrings.EmpSalaryDetailsPopupList];
            }
            set
            {
                ViewState[ViewstateStrings.EmpSalaryDetailsPopupList] = value;
            }
        }

        /// <summary>
        /// To maintain the total pages in viewstate
        /// </summary>
        private int TotalPages
        {
            get
            {
                return (int)(this.ViewState[ViewstateStrings.TotalPages] ?? 1);
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPages] = value;
            }
        }

        /// <summary>
        /// To keep RowIndex in view state
        /// </summary>
        private int RowIndex
        {
            get
            {
                return this.ViewState[ViewstateStrings.RowIndex] == null ? -1 : (int)this.ViewState[ViewstateStrings.RowIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.RowIndex] = value;
            }
        }

        /// <summary>
        /// To keep popup RowIndex in view state
        /// </summary>
        private int RowIndexPopup
        {
            get
            {
                return this.ViewState[ViewstateStrings.RowIndexPopup] == null ? -1 : (int)this.ViewState[ViewstateStrings.RowIndexPopup];
            }
            set
            {
                this.ViewState[ViewstateStrings.RowIndexPopup] = value;
            }
        }

        /// <summary>
        /// To keep popup display mode
        /// </summary>
        private bool PopupViewMode
        {
            get
            {
                return this.ViewState[ViewstateStrings.PopupViewMode] == null ? false : (bool)this.ViewState[ViewstateStrings.PopupViewMode];
            }
            set
            {
                this.ViewState[ViewstateStrings.PopupViewMode] = value;
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
        /// To keep journal status in view state
        /// </summary>
        private int JournalStatus
        {
            get
            {
                return this.ViewState[ViewstateStrings.JournalStatus] == null ? 0 : Convert.ToInt32((this.ViewState[ViewstateStrings.JournalStatus]));
            }
            set
            {
                this.ViewState[ViewstateStrings.JournalStatus] = value;
            }
        }

        private bool MultiCurrencyEnabled
        {
            get
            {
                return this.ViewState[ViewstateStrings.MultiCurrencyEnabled] == null ? false : Convert.ToBoolean((this.ViewState[ViewstateStrings.MultiCurrencyEnabled]));
            }
            set
            {
                this.ViewState[ViewstateStrings.MultiCurrencyEnabled] = value;
            }
        }
        private int Status
        {
            get
            {
                return this.ViewState[ViewstateStrings.Status] == null ? 0 : Convert.ToInt32((this.ViewState[ViewstateStrings.Status]));
            }
            set
            {
                this.ViewState[ViewstateStrings.Status] = value;
            }
        }

        #endregion

        #region  Variables

        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        User currentUser;
        private BackgroundTaskService MailQ;
        private DataTable dtResult;
        private DataTable dtCompany;
        private DataSet dsExchangeRate;
        private DataTable dtDept;
        private SalaryPaymentHeader objSalaryPaymentHeader;
        private SalaryPaymentHeader_PopUp objSalaryPaymentHeaderPopUp;
        private List<SalaryPaymentDetails> SalaryPaymentDetailList = new List<SalaryPaymentDetails>();
        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;
        private int PymntModDetPk = 0;
        private string strResult = string.Empty;
        private string refID;
        private string inboxFlag;
        private string prefID;
        private int CompanyPk = 0;
        private int JournalPK;
        private int dptType;
        #endregion
        #endregion

        #region PageLevel Events
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            // InitializeComponent();
            // if (!IsPostBack)
            // {
            PageActionHandler();
            // }
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
                if (Session[ERP.Utilities.SessionStrings.TransactionType] == null)
                {
                    ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                    hdfJournalizeWorkFlow.Value = "0";
                }
                ucrJournalize.JournalizeSave += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeSubmit += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeDelete += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeCancel += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;
                // InitializeComponent();
                if (!IsPostBack)
                {

                    PageIndex = Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE);
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    txtDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;

                    hdfCurrencyFormatWithComma.Value = "#" + currencysep + "#0.";
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                        hdfCurrencyFormatWithComma.Value += "0";
                    }
                    hdfExchangeRateFormat.Value = "#0.";
                    int exchrateDecimalDigits = (Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
                    for (int i = 0; i < exchrateDecimalDigits; i++)
                    {
                        hdfExchangeRateFormat.Value += "0";
                    }
                    MultiCurrencyEnabled = CommonFunctions.IsMultyCurrencyEnabled();
                    if (MultiCurrencyEnabled == true)
                        hdfCurrencyMode.Value = "1";
                    else
                        hdfCurrencyMode.Value = "0";
                    //hdfCurrencyMode.Value = MultiCurrencyEnabled.ToString();
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
                    FillProcessID(1);

                    hdfAdvSearch.Value = "0";
                    EntryStatus = EntryStatus.LISTMODE;
                    GetFieldValues(ControlsEnum.PAYMENTMODE);
                    SetFieldValues(ControlsEnum.PAYMENTMODE);
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    GetFieldValues(ControlsEnum.EMPLOYEETYPE);
                    SetFieldValues(ControlsEnum.EMPLOYEETYPE);
                    GetFieldValues(ControlsEnum.BRANCH);
                    SetFieldValues(ControlsEnum.BRANCH);
                    GetFieldValues(ControlsEnum.EMPLOYMENTTYPE);
                    SetFieldValues(ControlsEnum.EMPLOYMENTTYPE);
                    GetFieldValues(ControlsEnum.BANK);
                    SetFieldValues(ControlsEnum.BANK);
                    GetFieldValues(ControlsEnum.CURRENCY);
                    SetFieldValues(ControlsEnum.CURRENCY);
                    GetFieldValues(ControlsEnum.EXCHANGERATE);
                    SetFieldValues(ControlsEnum.EXCHANGERATE);
                    //If Request From External(Report or Other page) other than Menu or Inbox
                    if (Request.QueryString[QueryStrings.FromExt] != null && Request.QueryString[QueryStrings.FromExt] == "T")
                    {
                        CurrPK = Convert.ToInt32(Request.QueryString[QueryStrings.PK]);
                        EntryStatus = EntryStatus.ENTRYMODE;
                        FillProcessID(1);
                        WorkflowCore.CoreService objWorkflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = objWorkflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        ucrWrkf.ViewType = 0;
                        SetUIEditView(commonActions);
                        GetFieldValues(ControlsEnum.SALARYPAYMENTDETAILS);
                        SetFieldValues(ControlsEnum.SALARYPAYMENTDETAILS);
                    }
                    else
                    {
                        #region else Region
                        #region Work Flow
                        //If Has RefID (from Inbox)
                        if (!string.IsNullOrEmpty(refID))
                        {
                            if (!string.IsNullOrEmpty(inboxFlag))
                            {
                                ucrWrkf.ViewType = 0;
                                EntryStatus = EntryStatus.VIEWMODE;
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
                                dptType = (int)DeptTypeEnum.HRMS;
                                GetFieldValues(ControlsEnum.DEPARTMENTBYTYPE);
                                if (dtDept != null && dtDept.Rows.Count > 0)
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = Convert.ToInt32(dtDept.Rows[0]["DPT_PK"]);
                                    base.SetUserDept();
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(prefID))
                        {
                            ResetForm(ControlsEnum.CLEAR);
                            base.WkfRefID = ucrWrkf.RefID = int.Parse(prefID);
                            CurrPK = GetApplicationID(ucrWrkf.RefID);
                            dptType = (int)DeptTypeEnum.HRMS;
                            GetFieldValues(ControlsEnum.DEPARTMENTBYTYPE);
                            if (dtDept != null && dtDept.Rows.Count > 0)
                            {
                                Session[BusinessObject.Common.SessionStrings.CurDept] = Convert.ToInt32(dtDept.Rows[0]["DPT_PK"]);
                                base.SetUserDept();
                            }
                        }
                        if (CurrPK > 0)
                        {
                            EntryStatus = EntryStatus.ENTRYMODE;
                            FillProcessID(1);
                            WorkflowCore.CoreService objWorkflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = objWorkflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            SetUIEditView(commonActions);
                            GetFieldValues(ControlsEnum.SALARYPAYMENTDETAILS);
                            SetFieldValues(ControlsEnum.SALARYPAYMENTDETAILS);
                        }
                        else
                        {
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                        #endregion
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }
        #endregion

        #region Helper Methods
        #region Get Field Values
        private void GetFieldValues(ControlsEnum type)
        {
            ERPService.AdmCompanyMstService admCompanyMstServiceClient;
            ERPManager.ServiceUtility serviceUtilityObj;
            FinTrxService finTrxServiceClient;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            FilterParameters objFilterParams;
            try
            {
                switch (type)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        //int TotalRecords = 0;
                        objFilterParams = new FilterParameters();
                        //gridParam = new BusinessObject.GridPrams();
                        int addDedPk = 0;
                        int.TryParse(hdfTrxPk.Value, out addDedPk);
                        objFilterParams.PageNumber = PageIndex; //uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        objFilterParams.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        objFilterParams.PaymentMode = Convert.ToInt32(ddlListPaymentMode.SelectedValue);
                        objFilterParams.FromDate = string.IsNullOrEmpty(txtListFromDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtListFromDate.Text);
                        objFilterParams.ToDate = string.IsNullOrEmpty(txtListToDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtListToDate.Text);
                        objFilterParams.Name = txtChequeSrch.Text.Trim();
                        objFilterParams.UserPK = currentUser.PKUser;
                        objFilterParams.Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        objFilterParams.BizUnit = currentUser.SBUID;
                        objFilterParams.Active = Convert.ToInt32(DbActiveStatus.ACTIVE);
                        objFilterParams.PK = addDedPk > 0 ? addDedPk : (int?)null;
                        dtResult = SalaryPaymentBL.GetSalaryPaymentList(objFilterParams, Convert.ToInt32(ddlListBankName.SelectedValue));
                        //dtResult = SalaryPaymentBL.GetSalaryPaymentList(txtListFromDate.Text, txtListToDate.Text, Convert.ToInt32(ddlListPaymentMode.SelectedValue), 
                        //, currentUser.SBUID, Convert.ToInt32(DbActiveStatus.ACTIVE),
                        //PageIndex, Convert.ToInt32(GetLocalResourceObject("PageSize")), txtChequeSrch.Text.Trim());
                        break;
                    #endregion
                    #region Edit
                    case ControlsEnum.EDIT:
                        dtResult = PayElementsMasterBL.GetParentElement(CurrPK, Convert.ToInt32(DbActiveStatus.HASPK), currentUser.SBUID, 0, 0, -1);
                        break;
                    #endregion
                    #region Type
                    case ControlsEnum.PAYMENTMODE:
                        dtResult = EmployeePayDetailsBL.GetPaymentModeSpl(currentUser.SBUID);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(BusinessObject.CommonManagement.DbActiveStatus.ACTIVE), 0, CompanyPk);
                        break;
                    #endregion
                    #region EMPLOYEE TYPE
                    case ControlsEnum.EMPLOYEETYPE:
                        dtResult = BusinessLogic.HRMS.Employee.EmployeePayDetailsBL.GetEmployeeTypeGetKV(0, currentUser.SBUID, (int)DbActiveStatus.ACTIVE);
                        break;
                    #endregion
                    #region BRANCH / LOCATION
                    case ControlsEnum.BRANCH:
                        dtResult = BusinessLogic.HRMS.Payroll.LoansAndAdvancesBL.GetBranchLocation();
                        break;
                    #endregion
                    #region EMPLOYMENTTYPE
                    case ControlsEnum.EMPLOYMENTTYPE:
                        int commonPK = 0;
                        dtResult = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmploymentType(commonPK);
                        break;
                    #endregion
                    #region BANK
                    case ControlsEnum.BANK:
                        int bankType = 2;
                        dtResult = EmployeePayDetailsBL.GetBankNames(currentUser.SBUID, bankType, null);
                        break;
                    #endregion
                    #region PAYMENT DETAILS POPUP
                    case ControlsEnum.PAYMENTDETAILSPOPUP:
                        objFilterParams = new FilterParameters();
                        if (!string.IsNullOrEmpty((ddlFilterEmploymentType.SelectedValue)))
                            objFilterParams.EmploymentType = Convert.ToInt32(ddlFilterEmploymentType.SelectedValue) > 0 ? Convert.ToInt32(ddlFilterEmploymentType.SelectedValue) : (int?)null;
                        if (!string.IsNullOrEmpty((ddlFilterBranchLocation.SelectedValue)))
                            objFilterParams.BranchLocation = Convert.ToInt32(ddlFilterBranchLocation.SelectedValue) > 0 ? Convert.ToInt32(ddlFilterBranchLocation.SelectedValue) : (int?)null;
                        if (!string.IsNullOrEmpty((ddlFilterCompany.SelectedValue)))
                            objFilterParams.Company = Convert.ToInt32(ddlFilterCompany.SelectedValue) > 0 ? Convert.ToInt32(ddlFilterCompany.SelectedValue) : (int?)null;
                        if (!string.IsNullOrEmpty((ddlFilterEmployeeType.SelectedValue)))
                            objFilterParams.Employee = Convert.ToInt32(ddlFilterEmployeeType.SelectedValue) > 0 ? Convert.ToInt32(ddlFilterEmployeeType.SelectedValue) : (int?)null;
                        if (!string.IsNullOrEmpty((ddlFilterBankName.SelectedValue)))
                            objFilterParams.EmpBank = Convert.ToInt32(ddlFilterBankName.SelectedValue) > 0 ? Convert.ToInt32(ddlFilterBankName.SelectedValue) : (int?)null;
                        if (!string.IsNullOrEmpty((ddlFilterPaymentMode.SelectedValue)))
                            objFilterParams.PaymentMode = Convert.ToInt32(ddlFilterPaymentMode.SelectedValue) > 0 ? Convert.ToInt32(ddlFilterPaymentMode.SelectedValue) : (int?)null;
                        if (!string.IsNullOrEmpty((ddlProcessMode.SelectedValue)))
                            objFilterParams.ProcessMode = Convert.ToInt32(ddlProcessMode.SelectedValue) > 0 ? Convert.ToInt32(ddlProcessMode.SelectedValue) : (int?)null;
                        if (!string.IsNullOrEmpty((ddlPayrollType.SelectedValue)))
                            objFilterParams.payrollType = Convert.ToInt32(ddlPayrollType.SelectedValue) > 0 ? Convert.ToInt32(ddlPayrollType.SelectedValue) : (int?)null;
                        objFilterParams.EmpCurrency = !string.IsNullOrEmpty(hdfCurrency.Value) ? Convert.ToInt32(hdfCurrency.Value) : (int?)null;
                        objFilterParams.SalaryMonth = string.IsNullOrEmpty(txtSalaryMonth.Text) ? (DateTime?)null : Convert.ToDateTime(txtSalaryMonth.Text);
                        objFilterParams.BizUnit = Convert.ToInt32(currentUser.SBUID);
                        objFilterParams.Active = (int)DbActiveStatus.ACTIVE;
                        objSalaryPaymentHeaderPopUp = SalaryPaymentBL.GetSalaryPaymentDetails(objFilterParams);
                        if (objSalaryPaymentHeaderPopUp != null)
                            EmpSalaryDetailsPopupList = objSalaryPaymentHeaderPopUp.SalaryEmployee_PopUPDtl;
                        break;
                    #endregion
                    #region GET SALARY PAYMENT DETAILS
                    case ControlsEnum.SALARYPAYMENTDETAILS:
                        objSalaryPaymentHeader = SalaryPaymentBL.GetSalaryPaymentByPK(currentUser.SBUID, Convert.ToInt32(CommonConstants.ACTIVE), CurrPK);
                        break;
                    #endregion
                    #region PROCESS MODE
                    case ControlsEnum.PROCESSMODE:
                        dtResult = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PAYROLL PROCESS MODE");
                        break;
                    #endregion
                    #region PAYROLL TYPE
                    case ControlsEnum.PAYROLLTYPE:
                        int processmode = 0;
                        processmode = Convert.ToInt32(ddlProcessMode.SelectedValue);
                        dtResult = PayrollProcessBL.GetPayrollType(0, currentUser.SBUID, Convert.ToInt32(DbActiveStatus.ACTIVE), processmode, currentUser.PKUser);
                        break;
                    #endregion
                    #region DAT DETAILS
                    case ControlsEnum.DATDETAILS:
                        strResult = string.Empty;
                        dtResult = SalaryPaymentBL.GetDATDetails(PymntModDetPk, out strResult);
                        break;
                    #endregion
                    #region EXCHANGE RATE
                    case ControlsEnum.EXCHANGERATE:
                        if (!string.IsNullOrEmpty(hdfCurrency.Value) && Convert.ToInt32(hdfCurrency.Value) > 0)
                        {
                            DateTime Date = DateTime.Now;
                            DateTime.TryParse(txtDate.Text.Trim(), out Date);
                            dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(Convert.ToInt32(hdfCurrency.Value), currentUser.BaseCurrency, Date);
                        }
                        break;
                    #endregion
                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        dtResult = CommonBL.GetCurrency(currentUser.BaseCurrency, currentUser.SBUID, (int)DbActiveStatus.HASPK);
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
                    #region GETINVOICEPKBYJOURNALPK
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
                    #endregion
                    #region DEPARTMENT BY TYPE
                    case ControlsEnum.DEPARTMENTBYTYPE:
                        dtDept = BusinessLogic.CommonManagement.CommonBL.GetDepartment(currentUser.SBUID, null, dptType, (int)DbActiveStatus.ACTIVE);
                        break;
                    #endregion

                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { }
        }
        #endregion
        #region Set Field Values
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region PAYMENTMODE
                    case ControlsEnum.PAYMENTMODE:
                        BindDropDown(ControlsEnum.PAYMENTMODE);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    #endregion
                    #region EMPLOYEETYPE
                    case ControlsEnum.EMPLOYEETYPE:
                        BindDropDown(ControlsEnum.EMPLOYEETYPE);
                        break;
                    #endregion
                    #region BRANCH
                    case ControlsEnum.BRANCH:
                        BindDropDown(ControlsEnum.BRANCH);
                        break;
                    #endregion
                    #region EMPLOYMENTTYPE
                    case ControlsEnum.EMPLOYMENTTYPE:
                        BindDropDown(ControlsEnum.EMPLOYMENTTYPE);
                        break;
                    #endregion
                    #region BANK
                    case ControlsEnum.BANK:
                        BindDropDown(ControlsEnum.BANK);
                        break;
                    #endregion
                    #region LIST
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region PAYMENT DETAILS POPUP
                    case ControlsEnum.PAYMENTDETAILSPOPUP:
                        BindGrid(ControlsEnum.PAYMENTDETAILSPOPUP);
                        break;
                    #endregion
                    #region SALARYPAYMENTDETAILS
                    case ControlsEnum.SALARYPAYMENTDETAILS:
                        GetUIValuesFromObject(ControlsEnum.SALARYPAYMENTDETAILS);
                        break;
                    #endregion
                    #region PAYMENT DETAILS
                    case ControlsEnum.PAYMENTDETAILLIST:
                        BindGrid(ControlsEnum.PAYMENTDETAILLIST);
                        break;
                    #endregion
                    #region PROCESSMODE
                    case ControlsEnum.PROCESSMODE:
                        BindDropDown(ControlsEnum.PROCESSMODE);
                        break;
                    #endregion
                    #region PAYROLL TYPE
                    case ControlsEnum.PAYROLLTYPE:
                        BindDropDown(ControlsEnum.PAYROLLTYPE);
                        break;
                    #endregion
                    #region GRID EDIT
                    case ControlsEnum.GRIDEDIT:
                        GetUIValuesFromObject(ControlsEnum.GRIDEDIT);
                        break;
                    #endregion
                    #region GRID DELETE
                    case ControlsEnum.GRIDDELETE:
                        GetUIValuesFromObject(ControlsEnum.GRIDDELETE);
                        break;
                    #endregion
                    #region EXCHANGERATE
                    case ControlsEnum.EXCHANGERATE:
                        GetUIValuesFromObject(ControlsEnum.EXCHANGERATE);
                        break;
                    #endregion
                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        GetUIValuesFromObject(ControlsEnum.CURRENCY);
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
        #region Set UIValues To Object
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
                    #region SALARY PAYMENT HDR
                    case ControlsEnum.SALARYPAYMENTHDR:
                        objSalaryPaymentHeader.PSH_PK = CurrPK;
                        objSalaryPaymentHeader.PSH_NO = lblTrxNo.Text;
                        objSalaryPaymentHeader.PSH_DATE = Convert.ToDateTime(txtDate.Text);
                        //objSalaryPaymentHeader.PSH_PAYMNT_MODE = Convert.ToInt32(ddlPaymentMode.SelectedValue);
                        //objSalaryPaymentHeader.PSH_BANK = Convert.ToInt32(ddlBankName.SelectedValue) > 0 ? ddlBankName.SelectedValue : null;
                        //objSalaryPaymentHeader.PSH_CHEQUE_NO = txtChequeNo.Text;
                        //objSalaryPaymentHeader.PSH_CHEQUE_DATE = txtChequeDate.Text;
                        objSalaryPaymentHeader.PSH_REMARKS = string.IsNullOrEmpty(txtRemarksHdr.Text.Trim()) ? null : txtRemarksHdr.Text.Trim();
                        objSalaryPaymentHeader.PSH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        objSalaryPaymentHeader.PSH_BIZUNIT = Convert.ToInt32(currentUser.SBUID);
                        objSalaryPaymentHeader.PSH_ACTIVE = Convert.ToInt32(DbActiveStatus.ACTIVE);
                        objSalaryPaymentHeader.USER_PK = Convert.ToInt32(currentUser.PKUser);
                        objSalaryPaymentHeader.LAST_MOD_DT = LastModifiedTime;
                        objSalaryPaymentHeader.PSH_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                        objSalaryPaymentHeader.PSH_CURRENCY = Convert.ToInt32(hdfCurrency.Value);
                        objSalaryPaymentHeader.PSH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                        objSalaryPaymentHeader.PSH_EXCHG_RATE = string.IsNullOrEmpty(txtExchangeRate.Text.Trim()) ? 1 : Convert.ToDouble(txtExchangeRate.Text.Trim());
                        //SetUIValuesToObject(ControlsEnum.SALARYPAYMENTDTL);
                        int rowNo = 1;
                        SalaryPaymentModeDetailList.ForEach(dtl =>
                        {
                            dtl.ROW_NO = rowNo;
                            if (dtl.SalaryPaymentDtl != null && dtl.SalaryPaymentDtl.Count > 0)
                            {
                                dtl.SalaryPaymentDtl.ForEach(sldet => { sldet.ROW_NO = rowNo; });
                                dtl.PSP_AMOUNT = dtl.SalaryPaymentDtl.Sum(r => r.PSL_NET_SAL);
                            }
                            rowNo++;


                        });
                        // double sd = double.Parse(((grdEmpPaymentList.FooterRow.FindControl("lblgrdFooterTotalSalary") as Label).Text);
                        objSalaryPaymentHeader.SalaryPaymentModeDtl = SalaryPaymentModeDetailList;
                        retObject = objSalaryPaymentHeader;
                        break;
                    #endregion
                    #region Salary Payment DTL
                    case ControlsEnum.SALARYPAYMENTDTL:
                        //SalaryPaymentModeDetailList = new List<SalaryPaymentDetails>();
                        //foreach (GridViewRow grdrow in grdEmpPaymentList.Rows)
                        //{
                        //    SalaryPaymentDetails objMenuMappingDetails = new SalaryPaymentDetails();
                        //    objMenuMappingDetails.PSL_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPSL_PK")).Value);
                        //    objMenuMappingDetails.ROW_NO = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfROW_NO")).Value);
                        //    objMenuMappingDetails.PSL_PSH_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPSL_PSH_PK")).Value);
                        //    objMenuMappingDetails.PSL_PAYROLL_MONTH = (((HiddenField)grdrow.FindControl("hdfPSL_PAYROLL_MONTH")).Value);
                        //    objMenuMappingDetails.PSL_FROM_DATE = (((HiddenField)grdrow.FindControl("hdfPSL_FROM_DATE")).Value);
                        //    objMenuMappingDetails.PSL_TO_DATE = (((HiddenField)grdrow.FindControl("hdfPSL_TO_DATE")).Value);
                        //    objMenuMappingDetails.PSL_EMPLOYEE = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPSL_EMPLOYEE_PK")).Value);
                        //    objMenuMappingDetails.PSL_EMPLOYEE_TEXT = ((Label)grdrow.FindControl("lblEmployeeText")).Text.HtmlEncode();
                        //    objMenuMappingDetails.PSL_NET_SAL = Convert.ToDecimal(((Label)grdrow.FindControl("lblPSL_NET_SAL")).Text);
                        //    objMenuMappingDetails.PSL_BANK = (((HiddenField)grdrow.FindControl("hdfPSL_BANK")).Value);
                        //    objMenuMappingDetails.PSL_BANK_TEXT = ((Label)grdrow.FindControl("lblPSL_BANK_TEXT")).Text.HtmlEncode();
                        //    objMenuMappingDetails.PSL_ACCOUNT_NO = (((Label)grdrow.FindControl("lblPSL_ACCOUNT_NO")).ToolTip);
                        //    objMenuMappingDetails.PSL_BANK_IFSC = (((Label)grdrow.FindControl("lblPSL_BANK_IFSC")).ToolTip);
                        //    objMenuMappingDetails.PSL_EPS_PK = (((HiddenField)grdrow.FindControl("hdfPSL_EPS_PK")).Value);
                        //    SalaryPaymentModeDetailList.Add(objMenuMappingDetails);
                        //}
                        break;
                    #endregion
                    #region SAVE EMPLOYEE DETAILS POPUP TO GRID
                    case ControlsEnum.SAVEMPLOYEEDETAILSPOPUP:
                        List<SalaryPaymentDetails> objSalaryPaymentDetailList = new List<SalaryPaymentDetails>();
                        //if (RowIndex >= 0 && SalaryPaymentModeDetailList != null && SalaryPaymentModeDetailList.Count > 0)
                        //{
                        //    if (SalaryPaymentModeDetailList[RowIndex].SalaryPaymentDtl != null && !PopupViewMode)
                        //        SalaryPaymentDetailList = SalaryPaymentModeDetailList[RowIndex].SalaryPaymentDtl.DeepClone();// = SalaryPaymentDetailList;
                        foreach (GridViewRow grdrow in grdEmpPayment_PopUp.Rows)
                        {
                            CheckBox chkEmpselect_PopUp = (CheckBox)grdrow.FindControl("chkEmpselect_PopUp");
                            if (chkEmpselect_PopUp.Checked || PopupViewMode)
                            {
                                SalaryPaymentDetails objSalryEmpDetails = new SalaryPaymentDetails();
                                HiddenField hdfPSL_BANK_IFSC_PopUp = (HiddenField)grdrow.FindControl("hdfPSL_BANK_IFSC_PopUp");
                                HiddenField hdfPSL_ACCOUNT_NO_PopUp = (HiddenField)grdrow.FindControl("hdfPSL_ACCOUNT_NO_PopUp");
                                HiddenField hdfPSL_BANK_PopUp = (HiddenField)grdrow.FindControl("hdfPSL_BANK_PopUp");
                                //objMenuMappingDetails.ROW_NO = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfROW_NO_PopUp")).Value);                               
                                objSalryEmpDetails.PSL_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPSL_PK_PopUp")).Value);
                                objSalryEmpDetails.PSL_PSH_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPSL_PSH_PK_PopUp")).Value);
                                objSalryEmpDetails.PSL_PAYROLL_MONTH = (((HiddenField)grdrow.FindControl("hdfPSL_PAYROLL_MONTH_PopUp")).Value);
                                objSalryEmpDetails.PSL_FROM_DATE = (((HiddenField)grdrow.FindControl("hdfPSL_FROM_DATE_PopUp")).Value);
                                objSalryEmpDetails.PSL_TO_DATE = (((HiddenField)grdrow.FindControl("hdfPSL_TO_DATE_PopUp")).Value);
                                objSalryEmpDetails.PSL_EMPLOYEE = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPSL_EMPLOYEE_PK_PopUp")).Value);
                                objSalryEmpDetails.PSL_EMPLOYEE_TEXT = (((Label)grdrow.FindControl("lblEmployeeText_PopUp")).ToolTip);
                                objSalryEmpDetails.PSL_NET_SAL = Convert.ToDecimal(((Label)grdrow.FindControl("lblPSL_NET_SAL_PopUp")).ToolTip);
                                objSalryEmpDetails.PSL_BANK_TEXT = (((HiddenField)grdrow.FindControl("hdfPSL_BANK_TEXT_PopUp")).Value).HtmlDecode();
                                objSalryEmpDetails.PSL_BANK = string.IsNullOrEmpty(hdfPSL_BANK_PopUp.Value) ? null : hdfPSL_BANK_PopUp.Value;
                                objSalryEmpDetails.PSL_ACCOUNT_NO = string.IsNullOrEmpty(hdfPSL_ACCOUNT_NO_PopUp.Value) ? null : hdfPSL_ACCOUNT_NO_PopUp.Value;
                                objSalryEmpDetails.PSL_BANK_IFSC = string.IsNullOrEmpty(hdfPSL_BANK_IFSC_PopUp.Value) ? null : hdfPSL_BANK_IFSC_PopUp.Value;
                                objSalryEmpDetails.PSL_EPS_PK = (((HiddenField)grdrow.FindControl("hdfPSL_EPS_PK_PopUp")).Value);
                                objSalryEmpDetails.empDesignationText = ((Label)grdrow.FindControl("lblDesignation_PopUp")).Text;
                                objSalaryPaymentDetailList.Add(objSalryEmpDetails);
                            }
                        }
                        //SalaryPaymentModeDetailList[RowIndex].SalaryPaymentDtl = SalaryPaymentDetailList;
                        //}
                        retObject = objSalaryPaymentDetailList;
                        break;
                    #endregion
                    #region JOURNALIZE
                    case ControlsEnum.JOURNALIZE:

                        if (CurrPK > 0)
                        {
                            if (Status == (int)DbStatus.APPROVED)
                            {
                                GetFieldValues(ControlsEnum.SALARYPAYMENTDETAILS);

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


                                ucrJournalize.TransactionType = ApplicationType.SALPYMT;
                                Session[ERP.Utilities.SessionStrings.TransactionType] = ApplicationType.SALPYMTJ;
                                ucrJournalize.TransactionPK = CurrPK;
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                                ucrJournalize.JournalizePK = 0;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = objSalaryPaymentHeader.PSH_NO;
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = objSalaryPaymentHeader.PSH_DATE;
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = objSalaryPaymentHeader.PSH_CURRENCY;
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.AP;
                                //Session[ERP.Utilities.SessionStrings.AccountPayablePK] = objEmpPayrollHeader.ICH_CUSTOMER;
                                Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.SALPYMTJ;
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
                                SetCancelRef(CurrPK);
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
                                ucrJournalize.CallUserControl();
                                Session[ERP.Utilities.SessionStrings.JournalHead] = GetLocalResourceObject("Salary_Payment_Journal").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_salarypaymentNotapproved").ToString();
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        #endregion
        #region Get UIValues From Object
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region SALARY PAYMENT DETAILS
                    case ControlsEnum.SALARYPAYMENTDETAILS:
                        if (objSalaryPaymentHeader != null)
                        {
                            GetFieldValues(ControlsEnum.PAYMENTMODE);
                            SetFieldValues(ControlsEnum.PAYMENTMODE);
                            GetFieldValues(ControlsEnum.BANK);
                            SetFieldValues(ControlsEnum.BANK);
                            txtDate.Text = Convert.ToDateTime(objSalaryPaymentHeader.PSH_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            lblTrxNo.Text = string.IsNullOrEmpty(objSalaryPaymentHeader.PSH_NO) ? Resources.ErpRes.Draft : objSalaryPaymentHeader.PSH_NO;
                            txtRemarksHdr.Text = HttpUtility.HtmlDecode(objSalaryPaymentHeader.PSH_REMARKS);
                            LastModifiedTime = objSalaryPaymentHeader.LAST_MOD_DT;
                            CompanyPk = objSalaryPaymentHeader.PSH_COMPANY;
                            Status = objSalaryPaymentHeader.PSH_STATUS;
                            JournalStatus = objSalaryPaymentHeader.PSH_HAS_JRNL_ENTRY;
                            GetFieldValues(ControlsEnum.COMPANY);
                            SetFieldValues(ControlsEnum.COMPANY);
                            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(CompanyPk.ToString()));
                            txtCurrency.Text = string.Format(GetLocalResourceObject("CurrencyDisplayFormat").ToString(), Convert.ToString(objSalaryPaymentHeader.PSH_CURRENCY_CODE_TEXT), HttpUtility.HtmlDecode(objSalaryPaymentHeader.PSH_CURRENCY_NAME_TEXT));
                            hdfCurrency.Value = objSalaryPaymentHeader.PSH_CURRENCY.ToString();
                            txtExchangeRate.Text = GetFormattedExchangerate(objSalaryPaymentHeader.PSH_EXCHG_RATE);
                            hdfIsCancelled.Value = Convert.ToString(objSalaryPaymentHeader.PSH_DEL_STATUS);
                            SalaryPaymentModeDetailList = objSalaryPaymentHeader.SalaryPaymentModeDtl;
                            SetFieldValues(ControlsEnum.PAYMENTDETAILLIST);
                        }
                        break;
                    #endregion
                    #region GRID EDIT
                    case ControlsEnum.GRIDEDIT:
                        if (SalaryPaymentModeDetailList != null && SalaryPaymentModeDetailList.Count > 0 && RowIndex >= 0)
                        {
                            SalaryPaymentModeDetails objDet = SalaryPaymentModeDetailList[RowIndex];
                            if (objDet != null)
                            {
                                if (!string.IsNullOrEmpty(objDet.PSP_BANK))
                                    ddlBankName.SelectedValue = objDet.PSP_BANK;
                                ddlPaymentMode.SelectedValue = objDet.PSP_MODE.ToString();
                                txtChequeNo.Text = HttpUtility.HtmlDecode(objDet.PSP_INSTR_NO);
                                if (!string.IsNullOrEmpty(objDet.PSP_DATE))
                                    txtChequeDate.Text = Convert.ToDateTime(objDet.PSP_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                                txtRemarks.Text = HttpUtility.HtmlDecode(objDet.PSP_REMARK);
                            }
                        }
                        break;
                    #endregion
                    #region GRID DELETE
                    case ControlsEnum.GRIDDELETE:
                        if (SalaryPaymentModeDetailList != null && SalaryPaymentModeDetailList.Count > 0 && RowIndex >= 0)
                        {
                            List<SalaryPaymentModeDetails> objTempDetalisList = SalaryPaymentModeDetailList;
                            SalaryPaymentModeDetails objTemp = SalaryPaymentModeDetailList[RowIndex];
                            if (objTemp != null)
                            {
                                objTempDetalisList.Remove(objTemp);
                                SalaryPaymentModeDetailList = objTempDetalisList;
                            }
                        }
                        break;
                    #endregion
                    #region EXCHANGE RATE
                    case ControlsEnum.EXCHANGERATE:
                        if (dsExchangeRate != null && dsExchangeRate.Tables[0].Rows.Count > 0)
                        {
                            if (Convert.ToDouble(dsExchangeRate.Tables[0].Rows[0][0]) > 0)
                                txtExchangeRate.Text = GetFormattedExchangerate(dsExchangeRate.Tables[0].Rows[0][0].ToString());
                        }
                        else
                        {
                            txtExchangeRate.Text = string.Empty;
                        }
                        //  Exchange rate field is not editable(Domestic).ie,If selected currency is same as SBU base currency
                        if (currentUser.BaseCurrency == Convert.ToInt32(hdfCurrency.Value))
                            txtExchangeRate.Enabled = false;
                        else
                            txtExchangeRate.Enabled = true;
                        break;
                    #endregion
                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            txtCurrency.Text = string.Format(GetLocalResourceObject("CurrencyDisplayFormat").ToString(), Convert.ToString(dtResult.Rows[0]["CUR_CODE"]), Convert.ToString(dtResult.Rows[0]["CUR_NAME"]));
                            hdfCurrency.Value = Convert.ToString(dtResult.Rows[0]["CUR_PK"]);
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
        #endregion
        #region Bind DropDown
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region PAYMENTMODE
                case ControlsEnum.PAYMENTMODE:
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlPaymentMode.DataSource = dtResult;
                        ddlPaymentMode.DataTextField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_TEXT;
                        ddlPaymentMode.DataValueField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_VALUE;
                        ddlPaymentMode.DataBind();
                        ddlPaymentMode.Items.HtmlDecode();

                        //For List Filteration 
                        ddlListPaymentMode.Items.Clear();
                        ddlListPaymentMode.DataSource = dtResult;
                        ddlListPaymentMode.DataTextField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_TEXT;
                        ddlListPaymentMode.DataValueField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_VALUE;
                        ddlListPaymentMode.DataBind();

                        //For List Filteration popup
                        ddlFilterPaymentMode.Items.Clear();
                        ddlFilterPaymentMode.DataSource = dtResult;
                        ddlFilterPaymentMode.DataTextField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_TEXT;
                        ddlFilterPaymentMode.DataValueField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_VALUE;
                        ddlFilterPaymentMode.DataBind();
                    }
                    ddlPaymentMode.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    ddlListPaymentMode.Items.Insert(0, new ListItem(CommonConstants.ALL, CommonConstants.SELECTVAL));
                    ddlFilterPaymentMode.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region COMPANY
                case ControlsEnum.COMPANY:
                    ddlFilterCompany.Items.Clear();
                    ddlFilterCompany.DataTextField = Resources.DataFieldRes.CMP_NAME;
                    ddlFilterCompany.DataValueField = Resources.DataFieldRes.CMP_PK;
                    ddlFilterCompany.DataSource = dtCompany;
                    ddlFilterCompany.DataBind();
                    ddlFilterCompany.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    ddlFilterCompany.Items.HtmlDecode();

                    ddlCompany.DataTextField = Resources.DataFieldRes.CMP_NAME;
                    ddlCompany.DataValueField = Resources.DataFieldRes.CMP_PK;
                    ddlCompany.DataSource = dtCompany;
                    ddlCompany.DataBind();
                    ddlCompany.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    ddlCompany.Items.HtmlDecode();
                    if (dtCompany != null && dtCompany.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK])))
                    {
                        ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK].ToString()));
                        ddlFilterCompany.SelectedIndex = ddlFilterCompany.Items.IndexOf(ddlFilterCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK].ToString()));
                    }

                    break;
                #endregion
                #region EMPLOYEE TYPE
                case ControlsEnum.EMPLOYEETYPE:
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlFilterEmployeeType.Items.Clear();
                        ddlFilterEmployeeType.DataSource = dtResult;
                        ddlFilterEmployeeType.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.EMT_NAME;
                        ddlFilterEmployeeType.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.EMT_PK;
                        ddlFilterEmployeeType.DataBind();
                        ddlFilterEmployeeType.Items.HtmlDecode();
                    }
                    ddlFilterEmployeeType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region BRANCH / LOCATION
                case ControlsEnum.BRANCH:
                    ddlFilterBranchLocation.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlFilterBranchLocation.DataSource = dtResult;
                        ddlFilterBranchLocation.DataTextField = GTIService.Constants.HRMS.Employee.Fields.HRM_CON_TEXT;
                        ddlFilterBranchLocation.DataValueField = GTIService.Constants.HRMS.Employee.Fields.HRM_CON_PK;
                        ddlFilterBranchLocation.DataBind();
                    }
                    ddlFilterBranchLocation.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region EMPLOYMENTTYPE
                case ControlsEnum.EMPLOYMENTTYPE:
                    ddlFilterEmploymentType.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlFilterEmploymentType.DataSource = dtResult;
                        ddlFilterEmploymentType.DataTextField = GTIService.Constants.HRMS.Employee.Fields.HRM_CON_TEXT;
                        ddlFilterEmploymentType.DataValueField = GTIService.Constants.HRMS.Employee.Fields.HRM_CON_PK;
                        ddlFilterEmploymentType.DataBind();
                    }
                    ddlFilterEmploymentType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region BANK
                case ControlsEnum.BANK:
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlBankName.DataSource = dtResult;
                        ddlBankName.DataTextField = GTIService.Constants.HRMS.Employee.Fields.CBM_NAME;
                        ddlBankName.DataValueField = GTIService.Constants.HRMS.Employee.Fields.CBM_PK;
                        ddlBankName.DataBind();
                        ddlBankName.Items.HtmlDecode();
                        // For List Filter Bank Name
                        ddlListBankName.DataSource = dtResult;
                        ddlListBankName.DataTextField = GTIService.Constants.HRMS.Employee.Fields.CBM_NAME;
                        ddlListBankName.DataValueField = GTIService.Constants.HRMS.Employee.Fields.CBM_PK;
                        ddlListBankName.DataBind();
                        ddlListBankName.Items.HtmlDecode();
                        // For Payment Filter 
                        ddlFilterBankName.DataSource = dtResult;
                        ddlFilterBankName.DataTextField = GTIService.Constants.HRMS.Employee.Fields.CBM_NAME;
                        ddlFilterBankName.DataValueField = GTIService.Constants.HRMS.Employee.Fields.CBM_PK;
                        ddlFilterBankName.DataBind();
                        ddlFilterBankName.Items.HtmlDecode();
                    }
                    ddlBankName.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    ddlListBankName.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    ddlFilterBankName.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region PROCESS MODE
                case ControlsEnum.PROCESSMODE:
                    ddlProcessMode.Items.Clear();
                    ddlProcessMode.DataSource = dtResult;
                    ddlProcessMode.DataTextField = GTIService.Constants.Common.Fields.ADM_CFG_TEXT;
                    ddlProcessMode.DataValueField = GTIService.Constants.Common.Fields.ADM_CFG_VALUE;
                    ddlProcessMode.DataBind();
                    ddlProcessMode.Items.HtmlDecode();
                    //ddlProcessMode.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region PAYROLL TYPE
                case ControlsEnum.PAYROLLTYPE:
                    ddlPayrollType.Items.Clear();
                    ddlPayrollType.DataSource = dtResult;
                    ddlPayrollType.DataTextField = GTIService.Constants.HRMS.Employee.Fields.PTM_NAME;
                    ddlPayrollType.DataValueField = GTIService.Constants.HRMS.Employee.Fields.PTM_PK;
                    ddlPayrollType.DataBind();
                    ddlPayrollType.Items.HtmlDecode();
                    ddlPayrollType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlPayrollType.SelectedValue = dtResult.Rows[0][GTIService.Constants.HRMS.Employee.Fields.PTM_PK].ToString();
                    }
                    break;
                #endregion
                default:
                    break;
            }
        }
        #endregion
        #region BindGrid
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        uclPaging.Visible = false;
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            int rowCount = 0;
                            rowCount = Convert.ToInt32(dtResult.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                            uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                              (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                              (rowCount / this.PageSize) + 1;
                            PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdList.DataSource = dtResult;
                            grdList.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            grdList.DataSource = null;
                            grdList.DataBind();
                        }
                        break;
                    #endregion
                    #region PAYMENTDETAILLIST
                    case ControlsEnum.PAYMENTDETAILLIST:
                        if (SalaryPaymentModeDetailList != null)
                        {
                            grdEmpPaymentList.DataSource = SalaryPaymentModeDetailList;//.Where(x => x.IS_DELETED == 0);
                            grdEmpPaymentList.DataBind();
                            (grdEmpPaymentList.FooterRow.FindControl("lblgrdFooterTotalSalary") as Label).Text = GetFormattedCurrencyWithComma(SalaryPaymentModeDetailList.Where(mod => mod.SalaryPaymentDtl != null && mod.SalaryPaymentDtl.Count > 0).Sum(od => od.SalaryPaymentDtl.Sum(r => r.PSL_NET_SAL)));
                            (grdEmpPaymentList.FooterRow.FindControl("lblgrdFooterEmpCount") as Label).Text = SalaryPaymentModeDetailList.Where(mod => mod.SalaryPaymentDtl != null && mod.SalaryPaymentDtl.Count > 0).Sum(od => od.SalaryPaymentDtl.Count).ToString();

                        }
                        else
                        {
                            grdEmpPaymentList.DataSource = null;
                            grdEmpPaymentList.DataBind();
                        }
                        break;
                    #endregion
                    #region PAYMENT DETAILS
                    case ControlsEnum.PAYMENTDETAILSPOPUP:
                        if (EmpSalaryDetailsPopupList != null)
                        {
                            grdEmpPayment_PopUp.DataSource = EmpSalaryDetailsPopupList;//.Where(x => x.IS_DELETED == 0);
                            grdEmpPayment_PopUp.DataBind();

                            if (PopupViewMode)
                            {
                                grdEmpPayment_PopUp.Columns[grdEmpPayment_PopUp.Columns.Count - 1].Visible = true;
                                grdEmpPayment_PopUp.Columns[0].Visible = false;
                            }
                            else
                            {
                                grdEmpPayment_PopUp.Columns[grdEmpPayment_PopUp.Columns.Count - 1].Visible = false;
                                grdEmpPayment_PopUp.Columns[0].Visible = true;
                            }
                        }
                        else
                        {
                            grdEmpPayment_PopUp.DataSource = null;
                            grdEmpPayment_PopUp.DataBind();
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
        #region Reset Form
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                # region CLEAR
                case ControlsEnum.CLEAR:
                    JournalStatus = 0;
                    CurrPK = 0;
                    txtDate.Text = string.Empty;
                    lblTrxNo.Text = Resources.ErpRes.Draft;
                    if (ddlPaymentMode.Items.Count > 0)
                        ddlPaymentMode.SelectedIndex = 0;
                    if (ddlBankName.Items.Count > 0)
                        ddlBankName.SelectedIndex = 0;
                    txtChequeNo.Text = string.Empty;
                    txtChequeDate.Text = string.Empty;
                    txtRemarks.Text = string.Empty;
                    ddlFilterEmployeeType.SelectedIndex = 0;
                    ddlFilterBranchLocation.SelectedIndex = 0;
                    ddlFilterEmploymentType.SelectedIndex = 0;
                    ddlFilterBankName.SelectedIndex = 0;
                    ddlFilterPaymentMode.ClearSelection();
                    txtListFromDate.Text = string.Empty;
                    txtListToDate.Text = string.Empty;
                    txtChequeSrch.Text = string.Empty;
                    if (ddlListPaymentMode.Items.Count > 0)
                        ddlListPaymentMode.SelectedIndex = 0;
                    if (ddlListBankName.Items.Count > 0)
                        ddlListBankName.SelectedIndex = 0;
                    ddlStatus.ClearSelection();
                    uclPaging.CurrentPage = 0;
                    PageIndex = 1;
                    SalaryPaymentModeDetailList = null;
                    txtTrxNo.Text = string.Empty;
                    hdfTrxPk.Value = string.Empty;
                    txtDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                    txtRemarksHdr.Text = string.Empty;
                    Status = 0;
                    //txtCurrency.Enabled = false;
                    break;
                #endregion
                # region CLEAR PAYMENT DETAILS EDIT
                case ControlsEnum.CLEARPOPUPDETAILS:
                    ddlFilterEmployeeType.SelectedIndex = 0;
                    ddlFilterBranchLocation.SelectedIndex = 0;
                    ddlFilterEmploymentType.SelectedIndex = 0;
                    ddlFilterBankName.SelectedIndex = 0;
                    ddlFilterPaymentMode.ClearSelection();
                    EmpSalaryDetailsPopupList = null;
                    txtTrxNo.Text = string.Empty;
                    hdfTrxPk.Value = string.Empty;
                    txtSalaryMonth.Text = string.Empty;
                    break;
                #endregion
                # region CLEAR ADD
                case ControlsEnum.CLEARADD:
                    ddlPaymentMode.ClearSelection();
                    ddlBankName.ClearSelection();
                    txtChequeNo.Text = string.Empty;
                    txtChequeDate.Text = string.Empty;
                    txtRemarks.Text = string.Empty;
                    RowIndex = -1;
                    txtTrxNo.Text = string.Empty;
                    hdfTrxPk.Value = string.Empty;
                    break;
                #endregion
            }
        }
        #endregion
        #region Get Formatted Currency With Comma
        public string GetFormattedCurrencyWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithComma.Value);
        }
        #endregion
        #region Get Formatted Exchange rate
        public string GetFormattedExchangerate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfExchangeRateFormat.Value);
        }
        #endregion
        #region Add SMS List
        /// <summary>
        /// Method to add SMS list from resource to an object
        /// </summary>
        /// <param name="objSalaryPaymentHeader"></param>
        private void AddSMSList(SalaryPaymentHeader objSalaryPaymentHeader)
        {
            string xmlSms = GetGlobalResourceObject("ConfigurationsRes", "SmsTemplateSalaryPayment").ToString();
            SmsHeader objSmsDtls = new SmsHeader();
            if (!string.IsNullOrEmpty(xmlSms))
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(xmlSms);
                objSmsDtls = (SmsHeader)CommonFunctions.DeserializeObject(xmlDoc.InnerXml, objSmsDtls);
                objSalaryPaymentHeader.SmsList = objSmsDtls.SmsList;
            }
        }
        #endregion

        #region Add Mail List
        /// <summary>
        /// Method to add EMAIL list from resource to an object
        /// </summary>
        /// <param name="objSalaryPaymentHeader"></param>
        private void AddMailList(SalaryPaymentHeader objSalaryPaymentHeader)
        {
            string xmlMail = GetGlobalResourceObject("ConfigurationsRes", "HrmsMailTemplateSalaryPayment").ToString();
            MailHeader objMailDtls = new MailHeader();
            if (!string.IsNullOrEmpty(xmlMail))
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(xmlMail);
                objMailDtls = (MailHeader)CommonFunctions.DeserializeObject(xmlDoc.InnerXml, objMailDtls);
                objSalaryPaymentHeader.MailList = objMailDtls.MailList;
            }
        }
        #endregion
        #endregion

        #region EnableDisableButtons
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages, string pagerId)
        {
            if (pagerId == "uclPaging")
            {
                uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the first link
                uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the previous link
                uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false; // Should we enable the next link
                uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;// Should we enable the last link
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
            //To check if department is different by opening in new tab
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            try
            {
                int? result;
                bool bIsChecked = false;
                GridViewRow grvRow;
                TextBox WrkfComments;
                string TrxNo = string.Empty;

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
                    if (((DropDownList)sender).ID == "ddlPaymentMode")
                    {
                        commonActions = ActionsEnum.PAYMENTCHANGED;
                    }
                    else if (((DropDownList)sender).ID == "ddlProcessMode")
                    {
                        commonActions = ActionsEnum.PROCESSMODECHANGED;
                    }
                }
                switch (commonActions)
                {
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {

                            objSalaryPaymentHeader = new SalaryPaymentHeader();
                            objSalaryPaymentHeader = (SalaryPaymentHeader)SetUIValuesToObject(ControlsEnum.SALARYPAYMENTHDR);
                            if (objSalaryPaymentHeader != null)
                            {
                                if (objSalaryPaymentHeader.SalaryPaymentModeDtl != null && objSalaryPaymentHeader.SalaryPaymentModeDtl.Count > 0)
                                {
                                    bool IsSuccess = true;

                                    foreach (SalaryPaymentModeDetails objDet in objSalaryPaymentHeader.SalaryPaymentModeDtl)
                                    {
                                        if (objDet.SalaryPaymentDtl == null || objDet.SalaryPaymentDtl.Count == 0)
                                        {
                                            IsSuccess = false;
                                            break;
                                        }
                                    }
                                    if (!IsSuccess)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoEmp").ToString()) + "');", true);
                                        return;
                                    }
                                    objSalaryPaymentHeader.WKF_FLAG = 0;
                                    objSalaryPaymentHeader.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                    string trxNo = string.Empty;
                                    #region Add SMS List
                                    AddSMSList(objSalaryPaymentHeader);
                                    #endregion
                                    #region Add MAIL List
                                    AddMailList(objSalaryPaymentHeader);
                                    #endregion
                                    string xmlDoc = CommonFunctions.XmlSerialize<SalaryPaymentHeader>(objSalaryPaymentHeader);
                                    result = SalaryPaymentBL.SaveSalaryPaymentDetails(xmlDoc, out trxNo);
                                    if (result > 0)
                                    {
                                        #region Update dummy entry while modify salary payment after approval
                                        if (Status == (int)DbStatus.APPROVED)
                                        {
                                            FinTrxService finTrxServiceClient;
                                            finTrxServiceClient = new FinTrxService();
                                            string refType = string.Empty;
                                            refType = ApplicationType.SALPYMTJ;
                                            long DummyResult = objSalaryPaymentHeader.PSH_PK;
                                            bool IsDummyEntry = finTrxServiceClient.IsDummyEntry(refType, objSalaryPaymentHeader.PSH_PK, 0);
                                            if (IsDummyEntry == true)
                                            {
                                                DummyResult = finTrxServiceClient.DeleteFinTrx(refType, objSalaryPaymentHeader.PSH_PK, 0);
                                            }
                                            if (DummyResult > 0)
                                            {
                                                finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                                DummyResult = finTrxServiceClient.GenerateDummyEntry((int)CurrPK, refType);
                                                finTrxServiceClient = null;
                                            }
                                        }
                                        #endregion

                                        lblTrxNo.Text = trxNo;
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Salary_Save_Success").ToString();
                                        // object[] args = new object[2];
                                        // args[0] = Resources.PageNameRes.SalaryPayment;
                                        // args[1] = trxNo;
                                        //litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                        ResetForm(ControlsEnum.CLEAR);
                                        EntryStatus = EntryStatus.LISTMODE;
                                        CurrPK = (int)result;
                                        GetFieldValues(ControlsEnum.LIST);
                                        SetFieldValues(ControlsEnum.LIST);


                                    }
                                    else
                                    {
                                        if (result == (int)DbSaveStatus.SQLERROR)
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.SalaryPayment + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.SalaryPayment + " " + Resources.Messages.AlreadyDeleted;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.SalaryPayment + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.REFNOEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.SalaryPayment + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryPayment);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.Err_AddSalaryPaymentDtl;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                }
                            }
                        }
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
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup   
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            HiddenField hdfDept;
                            int dept;
                            if (rbtn.Checked)
                            {
                                if (Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Delete_Record").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    break;
                                }
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPSH_PKListPage")).Value);
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                if (Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            FillProcessID(11);
                            SetUIEditView(commonActions);
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.SALARYPAYMENTDETAILS);
                            SetFieldValues(ControlsEnum.SALARYPAYMENTDETAILS);
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID((int)CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            //if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            //{
                            ucrWrkf.ViewType = 1;
                            //    //btnSave.Visible = true;
                            //    //divbtnSavePaymentSplit.Visible = true;
                            //}
                            //else
                            //{
                            //    ucrWrkf.ViewType = 0;
                            //    //EntryStatus = EntryStatus.VIEWMODE;                           
                            //    //btnSave.Visible = false;
                            //    //divbtnSavePaymentSplit.Visible = false;
                            //}
                            //btnPrint.Visible = true;
                            ucrWrkf.ViewAction();
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
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
                    #region WRKF SUBMIT
                    case ActionsEnum.WRKFSUBMIT:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                objSalaryPaymentHeader = new SalaryPaymentHeader();
                                objSalaryPaymentHeader = (SalaryPaymentHeader)SetUIValuesToObject(ControlsEnum.SALARYPAYMENTHDR);
                                if (objSalaryPaymentHeader != null)
                                {
                                    if (objSalaryPaymentHeader.SalaryPaymentModeDtl != null && objSalaryPaymentHeader.SalaryPaymentModeDtl.Count > 0)
                                    {
                                        TrxNo = string.Empty;
                                        objSalaryPaymentHeader.WKF_FLAG = 1;
                                        SaveTransaction(objSalaryPaymentHeader, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.Err_AddSalaryPaymentDtl;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                    }
                                }
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)//Cancel Salary Payment
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(CurrPK, ApplicationType.SALPYMT))
                                {
                                    SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_SalaryPayment_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        FillProcessID(1);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    ResetForm(ControlsEnum.CLEAR);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    GetFieldValues(ControlsEnum.LIST);
                                    SetFieldValues(ControlsEnum.LIST);

                                }
                            }
                            else//Submit
                            {
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                            }

                        }
                        break;
                    #endregion
                    #region NEW
                    case ActionsEnum.NEW:
                        EntryStatus = EntryStatus.NEWMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.CURRENCY);
                        SetFieldValues(ControlsEnum.CURRENCY);
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        SetFieldValues(ControlsEnum.EXCHANGERATE);
                        GetFieldValues(ControlsEnum.COMPANY);
                        SetFieldValues(ControlsEnum.COMPANY);

                        SetFieldValues(ControlsEnum.PAYMENTDETAILLIST);
                        if (ddlPaymentMode.Items.Count > 0)
                            ddlPaymentMode.SelectedValue = "4";

                        FillProcessID(1);
                        base.WkfRefID = ucrWrkf.RefID = 0;
                        //SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        ucrWrkf.ViewType = 1;
                        ucrWrkf.ViewAction();
                        hdfIsCancelled.Value = "0";
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                    case ActionsEnum.LIST:
                        FillProcessID(1);
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);

                        break;
                    #endregion
                    #region DETAIL/EDIT/VIEW
                    case ActionsEnum.DETAIL:
                    case ActionsEnum.EDIT:
                    case ActionsEnum.VIEW:
                        JournalStatus = 0;
                        Status = 0;
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                ResetForm(ControlsEnum.CLEAR);
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPSH_PKListPage")).Value);
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                HiddenField hdfDelStatus = (HiddenField)grdrow.FindControl("hdfDelStatus");
                                HiddenField hdfStatus = (HiddenField)grdrow.FindControl("hdfStatus");
                                HiddenField hdfJournalStatus = (HiddenField)grdrow.FindControl("hdfJournalStatus");
                                Status = Convert.ToInt32(hdfStatus.Value);
                                hdfIsCancelled.Value = hdfDelStatus.Value;
                                JournalStatus = Convert.ToInt32(hdfJournalStatus.Value);
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
                                ucrWrkf.ViewAction();
                            }
                            GetFieldValues(ControlsEnum.SALARYPAYMENTDETAILS);
                            SetFieldValues(ControlsEnum.SALARYPAYMENTDETAILS);
                            ResetForm(ControlsEnum.CLEARADD);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = SalaryPaymentBL.DeleteSalaryPayment(CurrPK, Convert.ToString(this.LastModifiedTime));
                        if (result > 0)
                        {
                            if (grdList.Rows.Count == 1 && Convert.ToInt32(PageIndex) > 1)
                            {
                                PageIndex = Convert.ToInt32(PageIndex) - 1;
                            }
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryPayment);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            EntryStatus = EntryStatus.LISTMODE;
                            ResetForm(ControlsEnum.CLEAR);
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SalaryPayment;
                                litErrorMsg.Text += " " + GetGlobalResourceObject("Messages", "UsedInAnotherPlace").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SalaryPayment + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SalaryPayment + " " +
                                    GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SalaryPayment + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryPayment);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region FILTER
                    case ActionsEnum.FILTER:
                        uclPaging.CurrentPage = 0;
                        this.PageIndex = 1;
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        uclPaging.CurrentPage = 0;
                        this.PageIndex = 1;
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEARDETAIL:
                        ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                        SetFieldValues(ControlsEnum.PAYMENTDETAILSPOPUP);
                        ShowEmpFilterPopup();
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpSalaryPaymentDetails]','" + GetLocalResourceObject("EmployeeFilter").ToString() + "','860','485');", true);
                        break;
                    #endregion
                    #region PRINT
                    //case ActionsEnum.PRINT:
                    //    foreach (GridViewRow grdrow in grdList.Rows)
                    //    {
                    //        RadioButton rbtn;
                    //        rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                    //        if (rbtn.Checked)
                    //        {
                    //            bIsChecked = true;
                    //            CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPSH_PKListPage")).Value);
                    //            break;
                    //        }
                    //    }
                    //    if (bIsChecked)
                    //    {
                    //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "SALPYMT" + "&APPSUBTYPE= 0" + "&CurPK=" + CurrPK) + "');", true);
                    //    }
                    //    else
                    //    {
                    //        litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                    //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    //    }

                    //    break;
                    #endregion
                    #region  CANCEL POPUP
                    case ActionsEnum.CANCELPOPUP:
                        ResetForm(ControlsEnum.CLEARADD);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
                        break;
                    #endregion
                    #region PROCESS MODE CHANGED
                    case ActionsEnum.PROCESSMODECHANGED:
                        GetFieldValues(ControlsEnum.PAYROLLTYPE);
                        SetFieldValues(ControlsEnum.PAYROLLTYPE);
                        ShowEmpFilterPopup();
                        break;
                    #endregion
                    #region DAT
                    //case ActionsEnum.DAT:
                    //    sb = new StringBuilder();
                    //    string output = "Output";
                    //    sb.Append(output);
                    //    sb.Append("\r\n");

                    //    string text = sb.ToString();

                    //    Response.Clear();
                    //    Response.ClearHeaders();

                    //    Response.AddHeader("Content-Length", text.Length.ToString());
                    //    Response.ContentType = "text/plain";
                    //    string fileName = GetLocalResourceObject("UPAYDAT").ToString();
                    //    //  Response.AppendHeader("content-disposition", "attachment;filename=\"output.dat\"");
                    //    Response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
                    //    Response.Write(text);
                    //    Response.Flush();
                    //    Response.End();
                    //    break;
                    #endregion
                    #region ADD
                    case ActionsEnum.ADD:
                        if (SalaryPaymentModeDetailList == null)
                            SalaryPaymentModeDetailList = new List<SalaryPaymentModeDetails>();
                        List<SalaryPaymentModeDetails> objTempList = SalaryPaymentModeDetailList;
                        if (RowIndex >= 0 && SalaryPaymentModeDetailList.Count > 0)// update
                        {
                            SalaryPaymentModeDetails objTempattn = objTempList[RowIndex];
                            objTempattn.PSP_MODE = Convert.ToInt32(ddlPaymentMode.SelectedValue);
                            objTempattn.PSP_MODE_TEXT = HttpUtility.HtmlEncode(ddlPaymentMode.SelectedItem.Text);
                            objTempattn.PSP_BANK = Convert.ToInt32(ddlBankName.SelectedValue) > 0 ? ddlBankName.SelectedValue : null;
                            objTempattn.PSP_BANK_TEXT = Convert.ToInt32(ddlBankName.SelectedValue) > 0 ? HttpUtility.HtmlEncode(ddlBankName.SelectedItem.Text) : null;
                            objTempattn.PSP_DATE = string.IsNullOrEmpty(txtChequeDate.Text.Trim()) ? null : txtChequeDate.Text.Trim();
                            objTempattn.PSP_INSTR_NO = string.IsNullOrEmpty(txtChequeNo.Text.Trim()) ? null : HttpUtility.HtmlEncode(txtChequeNo.Text.Trim());
                            objTempattn.PSP_REMARK = string.IsNullOrEmpty(txtRemarks.Text.Trim()) ? null : HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                        }
                        else // new
                        {
                            SalaryPaymentModeDetails objTempattn = new SalaryPaymentModeDetails();
                            objTempattn.PSP_MODE = Convert.ToInt32(ddlPaymentMode.SelectedValue);
                            objTempattn.PSP_MODE_TEXT = HttpUtility.HtmlEncode(ddlPaymentMode.SelectedItem.Text);
                            objTempattn.PSP_BANK = Convert.ToInt32(ddlBankName.SelectedValue) > 0 ? ddlBankName.SelectedValue : null;
                            objTempattn.PSP_BANK_TEXT = Convert.ToInt32(ddlBankName.SelectedValue) > 0 ? HttpUtility.HtmlEncode(ddlBankName.SelectedItem.Text) : null;
                            objTempattn.PSP_DATE = string.IsNullOrEmpty(txtChequeDate.Text.Trim()) ? null : txtChequeDate.Text.Trim();
                            objTempattn.PSP_INSTR_NO = string.IsNullOrEmpty(txtChequeNo.Text.Trim()) ? null : HttpUtility.HtmlEncode(txtChequeNo.Text.Trim());
                            objTempattn.PSP_REMARK = string.IsNullOrEmpty(txtRemarks.Text.Trim()) ? null : HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                            objTempList.Add(objTempattn);
                        }
                        SalaryPaymentModeDetailList = objTempList;
                        SetFieldValues(ControlsEnum.PAYMENTDETAILLIST);
                        ResetForm(ControlsEnum.CLEARADD);
                        break;
                    #endregion
                    #region GRID EDIT
                    case ActionsEnum.GRIDEDIT:
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        RowIndex = grvRow.RowIndex;
                        SetFieldValues(ControlsEnum.GRIDEDIT);
                        break;
                    #endregion
                    #region GRID DELETE
                    case ActionsEnum.GRIDDELETE:
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        RowIndex = grvRow.RowIndex;
                        SetFieldValues(ControlsEnum.GRIDDELETE);
                        SetFieldValues(ControlsEnum.PAYMENTDETAILLIST);
                        break;
                    #endregion
                    #region CLEAR ADD
                    case ActionsEnum.CLEARADD:
                        ResetForm(ControlsEnum.CLEARADD);
                        break;
                    #endregion
                    #region SHOWPOPUP
                    case ActionsEnum.SHOWPOPUP:
                        PopupViewMode = false;
                        ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                        SetFieldValues(ControlsEnum.PAYMENTDETAILSPOPUP);
                        GetFieldValues(ControlsEnum.PROCESSMODE);
                        SetFieldValues(ControlsEnum.PROCESSMODE);
                        GetFieldValues(ControlsEnum.PAYROLLTYPE);
                        SetFieldValues(ControlsEnum.PAYROLLTYPE);
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        RowIndex = grvRow.RowIndex;
                        HiddenField hdfPaymentMode = (HiddenField)grvRow.FindControl("hdfPaymentMode");
                        HiddenField hdfBank = (HiddenField)grvRow.FindControl("hdfBank");
                        ddlFilterPaymentMode.SelectedValue = hdfPaymentMode.Value;
                        if (Convert.ToInt32(hdfPaymentMode.Value) == (int)PaymentModeEnum.Cash)
                        {
                            ddlFilterBankName.ClearSelection();
                            ddlFilterBankName.Enabled = false;
                        }
                        else if (!string.IsNullOrEmpty(hdfBank.Value))
                        {
                            ddlFilterBankName.Enabled = true;
                            ddlFilterBankName.SelectedValue = hdfBank.Value;
                        }
                        ShowEmpFilterPopup();
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpSalaryPaymentDetails]','" + GetLocalResourceObject("EmployeeFilter").ToString() + "','860','485');", true);
                        break;
                    #endregion
                    #region POPUP SEARCH
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.PAYMENTDETAILSPOPUP);
                        SetFieldValues(ControlsEnum.PAYMENTDETAILSPOPUP);
                        ShowEmpFilterPopup();
                        break;
                    #endregion
                    #region SAVE POPUP TO LIST
                    case ActionsEnum.SAVE_ACTIONPOPUP:
                        if (grdEmpPayment_PopUp.Rows.Count > 0)
                        {
                            SalaryPaymentDetailList = new List<SalaryPaymentDetails>();
                            List<SalaryPaymentDetails> objEmpSalaryTempList = (List<SalaryPaymentDetails>)SetUIValuesToObject(ControlsEnum.SAVEMPLOYEEDETAILSPOPUP);
                            if (objEmpSalaryTempList != null && RowIndex >= 0)
                            {
                                List<SalaryPaymentDetails> objEmpAddedList = new List<SalaryPaymentDetails>();
                                if (SalaryPaymentModeDetailList[RowIndex].SalaryPaymentDtl != null && !PopupViewMode)
                                {
                                    objEmpSalaryTempList.ForEach(dtl =>
                                    {
                                        if (SalaryPaymentModeDetailList[RowIndex].SalaryPaymentDtl.Where(r => r.PSL_EMPLOYEE == dtl.PSL_EMPLOYEE && r.PSL_PAYROLL_MONTH == dtl.PSL_PAYROLL_MONTH).Count() > 0)
                                        {
                                            objEmpAddedList.Add(dtl);
                                        }
                                    });

                                    if (objEmpAddedList.Count > 0)
                                    {
                                        string errMsg = GetLocalResourceObject("Err_AlreadyAdded").ToString() + "<br />";
                                        objEmpAddedList.ForEach(dtl =>
                                        {
                                            errMsg = errMsg + Convert.ToDateTime(dtl.PSL_PAYROLL_MONTH).ToString(Resources.Constants.DateFormatMonthYear) + "  " + dtl.PSL_EMPLOYEE_TEXT + "<br />";
                                        });
                                        ShowEmpFilterPopup();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(errMsg) + "');", true);
                                        return;
                                    }
                                }
                                if (RowIndex >= 0 && SalaryPaymentModeDetailList != null && SalaryPaymentModeDetailList.Count > 0)
                                {
                                    if (SalaryPaymentModeDetailList[RowIndex].SalaryPaymentDtl != null && !PopupViewMode)
                                        SalaryPaymentDetailList = SalaryPaymentModeDetailList[RowIndex].SalaryPaymentDtl.DeepClone();// = SalaryPaymentDetailList;
                                    SalaryPaymentDetailList.AddRange(objEmpSalaryTempList);
                                }
                                SalaryPaymentModeDetailList[RowIndex].SalaryPaymentDtl = SalaryPaymentDetailList;
                                SalaryPaymentModeDetailList[RowIndex].PSP_AMOUNT = SalaryPaymentModeDetailList[RowIndex].SalaryPaymentDtl.Sum(r => r.PSL_NET_SAL);
                            }
                            SetFieldValues(ControlsEnum.PAYMENTDETAILLIST);
                            ResetForm(ControlsEnum.CLEARADD);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
                        }
                        else
                        {
                            ShowEmpFilterPopup();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoRecords").ToString()) + "');", true);
                        }
                        break;
                    #endregion
                    #region GRID VIEW
                    case ActionsEnum.GRIDVIEW:
                        PopupViewMode = true;
                        ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        RowIndex = grvRow.RowIndex;
                        if (SalaryPaymentModeDetailList != null && SalaryPaymentModeDetailList.Count > 0)
                        {
                            EmpSalaryDetailsPopupList = new List<SalaryPaymentDetails>();
                            EmpSalaryDetailsPopupList = SalaryPaymentModeDetailList[RowIndex].SalaryPaymentDtl;
                        }
                        SetFieldValues(ControlsEnum.PAYMENTDETAILSPOPUP);
                        ShowEmpFilterPopup();
                        break;
                    #endregion
                    #region GRID PRINT
                    case ActionsEnum.GRIDPRINT:
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "SALPYMT" + "&APPSUBTYPE= 0" + "&CurPK=" + ((HiddenField)grvRow.FindControl("hdfPaymentModPk")).Value) + "&ISEXCELPRINT=" + GetGlobalResourceObject("ConfigurationsRes", "hrmsIsSalaryPymtBankPrintExcel").ToString() + "');", true);
                        break;
                    #endregion

                    #region EXCELPRINT
                    case ActionsEnum.EXCELPRINT:
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "SALPYMT" + "&APPSUBTYPE= 1" + "&ISEXCELPRINT=1" + "&CurPK=" + ((HiddenField)grvRow.FindControl("hdfPaymentModPk")).Value) + "');", true);

                        break;
                    #endregion

                    #region GRIDDAT
                    case ActionsEnum.GRIDDAT:
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        HiddenField hdfPaymentModPk = (HiddenField)grvRow.FindControl("hdfPaymentModPk");
                        int.TryParse(hdfPaymentModPk.Value, out PymntModDetPk);
                        GetFieldValues(ControlsEnum.DATDETAILS);
                        string datOutput = strResult;
                        //if (dtResult != null && dtResult.Rows.Count > 0)                      
                        //    datOutput = Convert.ToString(dtResult.Rows[0][0]);                      
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Length", datOutput.Length.ToString());
                        Response.ContentType = "text/plain";
                        Response.AppendHeader("content-disposition", "attachment; filename=" + GetLocalResourceObject("UPAYDAT").ToString());
                        Response.Write(datOutput);
                        Response.Flush();
                        Response.End();

                        break;
                    #endregion

                    #region GRIDSIF
                    case ActionsEnum.GRIDSIF:
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "SALPYMT" + "&APPSUBTYPE= 2" + "&ISEXCELPRINT=2" + "&CurPK=" + ((HiddenField)grvRow.FindControl("hdfPaymentModPk")).Value) + "');", true);
                        break;
                    #endregion

                    #region CURRENCYSELECTED
                    case ActionsEnum.CURRENCYSELECTED:
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        SetFieldValues(ControlsEnum.EXCHANGERATE);
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
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        ResetPageAndWorkflow();
                        break;
                    #endregion
                    #region Journalize Save
                    case ActionsEnum.JOURNALIZESAVE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ((Button)sender).CommandName = ActionsEnum.SAVE.ToString();
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetPageAndWorkflow();
                        break;
                    #endregion
                    #region Journalize Submit
                    case ActionsEnum.JOURNALIZESUBMIT:
                        ucrJournalize.ResetForm();
                        hdfJournalizeWorkFlow.Value = "0";
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        ResetPageAndWorkflow();
                        break;
                    #endregion
                    #region Journalize Delete
                    case ActionsEnum.JOURNALIZEDELETE:
                        hdfJournalizeWorkFlow.Value = "0";
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        ResetPageAndWorkflow();
                        break;
                    #endregion
                    #region Journalize Cancel
                    case ActionsEnum.JOURNALIZECANCEL:
                        hdfJournalizeWorkFlow.Value = "0";
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        ResetPageAndWorkflow();
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }

        /// <summary>
        /// Method to start Pay Slip mail service
        /// </summary>
        private void StartMailBackgroundTask()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (MailQ == null || !MailQ.IsStarted)
            {
                if (MailQ == null)
                {
                    MailQ = new BackgroundTaskService();
                }
                MailQ.currentUser = currentUser;
                MailQ.StartBackgroundService();
            }
        }


        private void ResetPageAndWorkflow()
        {
            ResetForm(ControlsEnum.CLEAR);
            EntryStatus = EntryStatus.LISTMODE;
            GetFieldValues(ControlsEnum.LIST);
            SetFieldValues(ControlsEnum.LIST);
            GetUIValuesFromObject(ControlsEnum.SALARYPAYMENTDETAILS);
            #region Reset Workflow
            base.ExtUserDept = 0;
            ucrWrkf.Reset();
            FillProcessID(1);
            WorkflowCore.CoreService objworkflowCore = new WorkflowCore.CoreService();
            base.WkfRefID = ucrWrkf.RefID = objworkflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
            SetCancelRef(CurrPK);
            ucrWrkf.FillWorkFlowDetails();
            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.LISTMODE && ucrWrkf.HasPageTaskPermission)
                ucrWrkf.ViewType = 1;
            else
            {
                ucrWrkf.ViewType = 0;
                ucrWrkf.ViewAction();
            }
            #endregion
        }

        /// <summary>
        /// Save With workflow submition
        /// </summary>
        /// <param name="objSalPayment"></param>
        private void SaveTransaction(SalaryPaymentHeader objSalPayment, int workflowFlag)
        {
            DropDownList ddlWkfAction;
            int? result = 0;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (objSalPayment == null)
                objSalPayment = new SalaryPaymentHeader();
            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            objSalPayment.USER_PK = wkfDetails.UserPK;
            objSalPayment.WKF_APPLICATION = CurrPK;
            objSalPayment.WKF_COMMENTS = wkfDetails.Comments;
            objSalPayment.WKF_TRX_FLAG = workflowFlag;
            objSalPayment.WKF_PROCESS = wkfDetails.ProcessID;
            objSalPayment.WKF_REFERENCE = wkfDetails.ReferenceID;
            objSalPayment.WKF_TASK = wkfDetails.TaskID;
            objSalPayment.WKF_TASK_ACTION = wkfDetails.ActionID;
            action = wkfDetails.ActionText;
            #endregion
            #region Add SMS List
            AddSMSList(objSalPayment);
            #endregion
            #region Add MAIL List
            AddMailList(objSalPayment);
            #endregion
            string xmlDoc = CommonFunctions.XmlSerialize<SalaryPaymentHeader>(objSalPayment);
            result = SalaryPaymentBL.SaveSalaryPaymentDetails(xmlDoc, out TrxNo);
            if (result > 0)
            {
                if (!string.IsNullOrEmpty(TrxNo))
                    lblTrxNo.Text = TrxNo;
                //ucrWrkf.ApplicationID = result.Value;
                if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                {
                    FillProcessID(1);
                    litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;
                }
                else
                {
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                }
                hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                ((TextBox)ucrWrkf.FindControl("WrkfComments")).Text = string.Empty;//Clear Workflow comments

                object[] args = new object[2];
                args[0] = Resources.PageNameRes.SalaryPayment;
                args[1] = lblTrxNo.Text.Trim();
                litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                //if (isMailStart == Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE))
                //    StartMailBackgroundTask();

                // Show Save Message and redired to listing page                                      
                if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                {
                    ResetForm(ControlsEnum.CLEAR);
                    EntryStatus = EntryStatus.LISTMODE;
                    CurrPK = (int)result;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                    ResetForm(ControlsEnum.CLEAR);
                    EntryStatus = EntryStatus.LISTMODE;
                    CurrPK = (int)result;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                }

            }
            else
            {
                if (result == (int)DbSaveStatus.SQLERROR)
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.SalaryPayment + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                }
                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.SalaryPayment + " " + Resources.Messages.AlreadyDeleted;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                    EntryStatus = EntryStatus.LISTMODE;
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.SalaryPayment + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else if (result == (int)DbSaveStatus.REFNOEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.SalaryPayment + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryPayment);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                }
            }
        }

        private void ShowEmpFilterPopup()
        {
            if (PopupViewMode)
                divPaymentFilterDetails.Visible = false;
            else
                divPaymentFilterDetails.Visible = true;
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpSalaryPaymentDetails]','" + GetLocalResourceObject("EmployeeFilter").ToString() + "','950','550');", true);
        }
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                if (Mode == ActionsEnum.NEW)
                {
                    EntryStatus = EntryStatus.NEWMODE;
                }
                else if (Mode == ActionsEnum.VIEW)
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
        #endregion

        #region ActionHandler
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
            GridView senderGridView = (GridView)sender;
            if (senderGridView.ID == "grdEmpPayment_PopUp")
            {
                if (e.CommandName == "DELETE_ACTION")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    if (EmpSalaryDetailsPopupList != null && EmpSalaryDetailsPopupList.Count > 0)
                    {
                        SalaryPaymentDetails objDetails = EmpSalaryDetailsPopupList[row.RowIndex];
                        EmpSalaryDetailsPopupList.Remove(objDetails);
                    }
                    SetFieldValues(ControlsEnum.PAYMENTDETAILSPOPUP);
                    ShowEmpFilterPopup();
                    //HiddenField hdfPSL_PK = row.FindControl("hdfPSL_PK") as HiddenField;
                    //HiddenField hdfROW_NO = row.FindControl("hdfROW_NO") as HiddenField;
                    //SalaryPaymentDetails detail = SalaryPaymentModeDetailList
                    //    .Where(x => x.ROW_NO == Convert.ToInt32(hdfROW_NO.Value) && x.PSL_PK == Convert.ToInt32(hdfPSL_PK.Value))
                    //    .SingleOrDefault();
                    //if (detail != null)
                    //{
                    //    SalaryPaymentModeDetailList.Remove(detail);
                    //    SetFieldValues(ControlsEnum.PAYMENTDETAILLIST);
                    //}
                    //else
                    //{
                    //    litErrorMsg.Text = Resources.Messages.ActionFailed;
                    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    //        + "','" + Resources.ErpRes.Information + "');", true);
                    //}
                }
            }
        }

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridView senderGridView = (GridView)sender;
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    if (senderGridView.ID == "grdEmpPaymentList")
                    {
                        Label lblgrdEmpCount = e.Row.FindControl("lblgrdEmpCount") as Label;
                        Label lblgrdTotalSalary = e.Row.FindControl("lblgrdTotalSalary") as Label;
                        if (SalaryPaymentModeDetailList != null && SalaryPaymentModeDetailList.Count >= e.Row.RowIndex
                            && SalaryPaymentModeDetailList[e.Row.RowIndex].SalaryPaymentDtl != null)
                        {
                            lblgrdEmpCount.Text = lblgrdEmpCount.ToolTip = SalaryPaymentModeDetailList[e.Row.RowIndex].SalaryPaymentDtl.Count.ToString();
                            lblgrdTotalSalary.Text = lblgrdTotalSalary.ToolTip = GetFormattedCurrencyWithComma(SalaryPaymentModeDetailList[e.Row.RowIndex].SalaryPaymentDtl.Sum(r => r.PSL_NET_SAL));
                        }
                        else
                        {
                            lblgrdEmpCount.Text = lblgrdEmpCount.ToolTip = "0";
                            lblgrdTotalSalary.Text = lblgrdTotalSalary.ToolTip = GetFormattedCurrencyWithComma(0);
                        }
                        ImageButton imbDat = e.Row.FindControl("imbDat") as ImageButton;
                        ScriptManager.GetCurrent(this).RegisterPostBackControl(imbDat);
                        //ScriptManager NewScriptManager = (ScriptManager)Page.Master.FindControl("scrMenu");
                        //NewScriptManager.RegisterPostBackControl(imbDat);
                        //((ScriptManager)Page.Master.FindControl("scrMenu")).RegisterPostBackControl(imbDat);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Pager Methods + Init
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

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetBankEnable", "SetBankEnable();", true);
                if (EntryStatus == EntryStatus.EDITMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideAdvance", "ShowHideAdvancedSearch();", true);
                    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
                else if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch(1);});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                if (grdEmpPaymentList.Rows.Count > 0 && CurrPK > 0)
                {
                    foreach (GridViewRow grdRow in grdEmpPaymentList.Rows)
                    {
                        ImageButton imbDat = grdRow.FindControl("imbDat") as ImageButton;
                        ScriptManager.GetCurrent(this).RegisterPostBackControl(imbDat);
                    }
                }
                if (CurrPK == 0)
                    btnSubmit.Visible = false;
                if (CurrPK == 0 || Status == 0)
                    btnCancelSubmit.Visible = false;
                if (!MultiCurrencyEnabled)
                    txtCurrency.Enabled = false;
                if (JournalStatus > 0)
                    btnSave.Visible = false;               
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
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
                        if (e.CurrentPage <= e.TotalPages)
                            pagerControl.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        if (e.CurrentPage > 1)
                            pagerControl.CurrentPage--;
                        break;
                }
                if (senderId == "uclPaging")
                {
                    PageIndex = uclPaging.CurrentPage;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    EntryStatus = EntryStatus.LISTMODE;
                    EnableDisableButtons(e.TotalPages, "uclPaging");
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }
        #region InitializeComponent
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
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?PID=" + pid.ToString();
            else
                path = Request.Url.AbsolutePath.ToLower() + "?PID=" + pid.ToString();
            if (pid == 2)//voucher
            {
                dptType = (int)DeptTypeEnum.FINANCE;
                GetFieldValues(ControlsEnum.DEPARTMENTBYTYPE);
                if (dtDept != null && dtDept.Rows.Count > 0)
                {
                    ucrJournalize.ExternalUserDept = base.ExtUserDept = Convert.ToInt32(dtDept.Rows[0]["DPT_PK"]);
                }
            }
            if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
            {
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtProcess = wrkfService.GetProcessID(path, base.ExtUserDept > 0 ? base.ExtUserDept : Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept]));
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), currentUser.CurrentDeptPK);
            if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
            {
                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
            }
            #endregion
        }
        private string GetUrl()
        {
            string path = string.Empty;
            // if one page containes two process (pageurl?PID=1,pageurl?PID=2)
            if (Request.QueryString[QueryStrings.PID] == null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                    path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?PID=1";
                else
                    path = Request.Url.AbsolutePath.ToLower() + "?PID=1";
            }
            else
            {
                if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                    path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
                else
                    path = Request.Url.AbsolutePath.ToLower();
            }
            return path;
        }
        #endregion
        #region Page Events
        /// <summary>
        /// PageInit Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            uclPaging.CurrentPage = 1;
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDeleteNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnJournalize.PreRender += new EventHandler(btnAction_PreRender);

            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnDeleteNew.Load += new EventHandler(btnAction_Load);
            this.btnNew.Load += new EventHandler(btnAction_Load);
            this.btnEdit.Load += new EventHandler(btnAction_Load);
            this.btnView.Load += new EventHandler(btnAction_Load);
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            btnEditforCancel.Load += new EventHandler(btnAction_Load);
            btnJournalize.Load += new EventHandler(btnAction_Load);
            
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>",
                "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
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
        protected void btnAction_PreRender(object sender, EventArgs e)
        {
            base.CheckBtnVisibility(sender);
        }
        #endregion
        #endregion

        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            LIST,
            CLEAR,
            PAYMENTMODE,
            EDIT,
            SALARYPAYMENTHDR,
            PAYMENTDETAILLIST,
            PAYMENTDETAILSPOPUP,
            SALARYPAYMENTDETAILS,
            CLEARPOPUPDETAILS,
            COMPANY,
            EMPLOYEETYPE,
            BRANCH,
            EMPLOYMENTTYPE,
            BANK,
            SALARYPAYMENTDTL,
            SAVEMPLOYEEDETAILSPOPUP,
            PROCESSMODE,
            PAYROLLTYPE,
            CLEARADD,
            GRIDEDIT,
            GRIDDELETE,
            DATDETAILS,
            EXCHANGERATE,
            CURRENCY,
            GETINVOICEPKBYJOURNALPK,
            JOURNALIZE,
            FINHEADER,
            DEPARTMENTBYTYPE
        }
        #endregion
    }
}