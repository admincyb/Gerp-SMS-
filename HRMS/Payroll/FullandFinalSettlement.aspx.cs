using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using System.Data;
using BusinessObject.Common;
using System.Threading;
using BusinessLogic.HRMS.Payroll;
using BusinessObject.CommonManagement;
using BusinessObject.HRMS.Payroll;
using ERPSMS_v01.UserControls;
using HRMS.Reports;
using BusinessLogic.CommonManagement;
using ERPData;
using ERPService;
using ERPManager;
using ERP.Utilities.HRMS;
using System.Reflection;
using System.Linq.Expressions;
using ERPSMS_v01;


namespace HRMS.Payroll
{
    public partial class FullandFinalSettlement : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables
        private ActionsEnum commonActions;
        User currentUser;
        private DataTable dtResult;
        private DataTable dtCompany;
        private DataTable dtDept;
        private DataTable dtNonPayroll;
        private FilterEmpDetails objFilterEmpDetails;
        private EmpPayrollHeader objEmpPayrollHeader;
        private EmpPayrollDetails objEmpPayrollDetails;
        private DataTable dtPageData;
        private EmpPayrollPayHeader objEmpPayHeader;
        private EmpPayrollPayDetails objEmpPayDetails;
        private PeriodWorkingDayHdr objWorkdayHdr;
        private EmpWorkingDayDetails objWorkDtls;
        private ERPData.ADM_COMPANY_MST admCompanyMstObj;
        private List<ERPData.ADM_COMPANY_MST> admCompanyMstList;
        private DataSet dsExchangeRate;
        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;

        private int PayrollTypePk = 0;
        private int CompanyPk = 0;
        private int dptType;

        private string refID;
        private string inboxFlag;
        private string prefID;
        private int JournalPK;

        #endregion

        #region Properties

        private int CurrPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.CurrPK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.CurrPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrPK] = value;
            }
        }

        private int CurrEmployeePayrollPK
        {
            get
            {
                return Convert.ToInt32(this.Session[ERP.Utilities.SessionStrings.CurrEmployeePayrollPK]);
            }
            set
            {
                this.Session[ERP.Utilities.SessionStrings.CurrEmployeePayrollPK] = value;
            }
        }
        private int EmployeePayrollPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrEmployeePayrollPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrEmployeePayrollPK] = value;
            }
        }
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

        private List<EmpPayrollDetails> empPayrollDetailList
        {
            get
            {
                return ViewState[ViewstateStrings.empPayrollDetailList] == null ? null : (List<EmpPayrollDetails>)ViewState[ViewstateStrings.empPayrollDetailList];
            }
            set
            {
                ViewState[ViewstateStrings.empPayrollDetailList] = value;
            }
        }

        private List<EmpPayrollPayDetails> empPayDtlList
        {
            get
            {
                return ViewState[ViewstateStrings.empPayDtlList] == null ? null : (List<EmpPayrollPayDetails>)ViewState[ViewstateStrings.empPayDtlList];
            }
            set
            {
                ViewState[ViewstateStrings.empPayDtlList] = value;
            }
        }

        private List<EmpWorkingDayDetails> workdayDetailList
        {
            get
            {
                return (List<EmpWorkingDayDetails>)ViewState[ViewstateStrings.workdayDetailList];
            }
            set
            {
                ViewState[ViewstateStrings.workdayDetailList] = value;
            }
        }

        private GridViewRow prvRow
        {
            get
            {
                return (GridViewRow)Session[ViewstateStrings.prvRow];
            }
            set
            {
                Session[ViewstateStrings.prvRow] = value;
            }
        }

        private GridViewRow curRow
        {
            get
            {
                return (GridViewRow)Session[ViewstateStrings.curRow];
            }
            set
            {
                Session[ViewstateStrings.curRow] = value;
            }
        }
        private EmpPaySlipHeader EmpPaySlipHeaderSession
        {
            get
            {
                return (EmpPaySlipHeader)Session[ViewstateStrings.EmpPaySlipHeaderSession];
            }
            set
            {
                Session[ViewstateStrings.EmpPaySlipHeaderSession] = value;
            }
        }
        private bool ShowSalPartPayElement
        {
            get
            {
                return this.ViewState[ViewstateStrings.ShowSalPartPayElement] == null ? false : Convert.ToBoolean((this.ViewState[ViewstateStrings.ShowSalPartPayElement]));
            }
            set
            {
                this.ViewState[ViewstateStrings.ShowSalPartPayElement] = value;
            }
        }

        private bool ShowIncomeTax
        {
            get
            {
                return this.ViewState[ViewstateStrings.ShowIncomeTax] == null ? true : Convert.ToBoolean((this.ViewState[ViewstateStrings.ShowIncomeTax]));
            }
            set
            {
                this.ViewState[ViewstateStrings.ShowIncomeTax] = value;
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

        private int NegativeSalExist
        {
            get
            {
                return this.ViewState[ViewstateStrings.NegativeSalExist] == null ? 0 : Convert.ToInt32((this.ViewState[ViewstateStrings.NegativeSalExist]));
            }
            set
            {
                this.ViewState[ViewstateStrings.NegativeSalExist] = value;
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
        /// HR Department Pk
        /// </summary>
        private int HRDeptPK
        {
            get
            {
                return this.ViewState["HRDeptPK"] == null ? 0 : Convert.ToInt32(this.ViewState["HRDeptPK"]);
            }
            set
            {
                this.ViewState["HRDeptPK"] = value;
            }
        }

        private SortDirection sortDirection
        {
            get
            {
                return this.ViewState["sortDirection"] == null ? SortDirection.Ascending : (SortDirection)ViewState["sortDirection"];
            }
            set
            {
                this.ViewState["sortDirection"] = value;
            }
        }

        private string sortExpression
        {
            get
            {
                return this.ViewState["sortExpression"] == null ? GetLocalResourceObject("sortExpression").ToString() : ViewState["sortExpression"].ToString();
            }
            set
            {
                this.ViewState["sortExpression"] = value;
            }
        }
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            //InitializeComponent();
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

            if (!IsPostBack)
            {
                PageActionHandler();
            }
        }
        #endregion

        #region Page Action Handler
        private void PageActionHandler()
        {
            try
            {
                SetWaitForServerResponse();
                ConfigurationSettings();
                //set of hidden fields used to format Quantity, Amount, Rate
                hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
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

                Session[ViewstateStrings.EntryState] = null;
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
                EntryStatus = EntryStatus.LISTMODE;
                txtProcessDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                txtResignDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                txtSalaryMonth.Text = DateTime.Now.ToString(Resources.Constants.DateFormatMonthYear);
                hdfMonthlyMode.Value = ((int)PayrollProcessMode.Monthly).ToString();
                GetFieldValues(ControlsEnum.EMPLOYMENTTYPE);
                SetFieldValues(ControlsEnum.EMPLOYMENTTYPE);
                GetFieldValues(ControlsEnum.EMPLOYEETYPE);
                SetFieldValues(ControlsEnum.EMPLOYEETYPE);
                GetFieldValues(ControlsEnum.COMPANY);
                SetFieldValues(ControlsEnum.COMPANY);
                GetFieldValues(ControlsEnum.BRANCH);
                SetFieldValues(ControlsEnum.BRANCH);
                GetFieldValues(ControlsEnum.PROCESSMODE);
                SetFieldValues(ControlsEnum.PROCESSMODE);
                ddlProcessMode.SelectedIndex = ddlProcessMode.Items.IndexOf(ddlProcessMode.Items.FindByText(PayrollProcessMode.Monthly.ToString()));//Monthly
                if (Convert.ToInt32(ddlProcessMode.SelectedValue) > 0)
                    ActionHandler(ddlProcessMode, new EventArgs());
                if (Convert.ToInt32(ddlPayrollType.SelectedValue) > 0)
                    ActionHandler(ddlPayrollType, new EventArgs());
                GetFieldValues(ControlsEnum.CURRENCY);
                SetFieldValues(ControlsEnum.CURRENCY);
                GetFieldValues(ControlsEnum.EXCHANGERATE);
                SetFieldValues(ControlsEnum.EXCHANGERATE);

                dptType = (int)DeptTypeEnum.HRMS;
                GetFieldValues(ControlsEnum.DEPARTMENTBYTYPE);
                if (dtDept != null && dtDept.Rows.Count > 0)
                {
                    HRDeptPK = Convert.ToInt32(dtDept.Rows[0]["DPT_PK"]);
                    Session[BusinessObject.Common.SessionStrings.CurDept] = HRDeptPK;
                    //base.SetUserDept();
                }
                //If Request From External(Report or Other page) other than Menu or Inbox
                if (Request.QueryString[QueryStrings.FromExt] != null && Request.QueryString[QueryStrings.FromExt] == "T")
                {
                    CurrPK = Convert.ToInt32(Request.QueryString[QueryStrings.PK]);
                    EntryStatus = EntryStatus.LISTMODE;
                    FillProcessID(1);
                    WorkflowCore.CoreService objWorkflowCore = new WorkflowCore.CoreService();
                    base.WkfRefID = ucrWrkf.RefID = objWorkflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                    SetCancelRef(CurrPK);
                    ucrWrkf.FillWorkFlowDetails();
                    ucrWrkf.ViewType = 0;
                    GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                    GetUIValuesFromObject(ControlsEnum.PROCESSPAYROLLHDR);
                    grdPeriodList.SelectedIndex = -1;
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    uclPaging.CurrentPage = 1;
                    GetFieldValues(ControlsEnum.PERIODLIST);
                    SetFieldValues(ControlsEnum.PERIODLIST);
                    SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                    GetFieldValues(ControlsEnum.WORKINGDAYSDETAILS);
                    EnableDisablePayrollHdr(false);//Disable Header Controls
                }
                else
                {
                    #region else Region
                    //SetFieldValues(ControlsEnum.WORKINGDAYSDETAILS);
                    if (CurrEmployeePayrollPK > 0)  // Maintain navigate from Incometax page to Payroll Detail 
                    {
                        SetUIEditView(commonActions);
                        ResetForm(ControlsEnum.EMPPAYROLLLISTTAX);
                        GetFieldValues(ControlsEnum.EMPPAYROLLDETAILLIST);
                        GetUIValuesFromObject(ControlsEnum.EMPPAYROLLDETAILLIST);
                        SetFieldValues(ControlsEnum.PAYEARNINGSLIST);
                        SetFieldValues(ControlsEnum.PAYDEDUCTIONLIST);
                        SetFieldValues(ControlsEnum.PAYROLLHISTORY);
                        hdfLastModDate.Value = objEmpPayHeader.EPH_MOD_DT.ToString();
                        CurrPK = objEmpPayHeader.EPS_PAYROLL_HDR;       //For Saving
                        LastModifiedTime = Convert.ToDateTime(hdfLastModDate.Value);
                        if (ShowIncomeTax)
                            lnkIncomeTax.Visible = true;
                        else
                            lnkIncomeTax.Visible = false;
                    }

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
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                        ////start
                        if (string.IsNullOrEmpty(pid) || pid.Equals("1") || pid.Equals("11"))
                        {
                            ResetForm(ControlsEnum.CLEARPROCESSDETAILS);
                            ResetForm(ControlsEnum.CLEARSEARCH);
                            ucrWrkf.RefID = int.Parse(refID);
                            base.WkfRefID = ucrWrkf.RefID;
                            CurrPK = GetApplicationID(ucrWrkf.RefID);
                            //if (pid.Equals("11"))
                            //    hdfIsInvCancelled.Value = "1";//For Showing Cancelled Stamp in Detail Page
                        }
                        else if (pid.Equals("2") || pid.Equals("12"))
                        {
                            ucrWrkf.RefID = int.Parse(refID);
                            JournalPK = GetApplicationID(ucrWrkf.RefID);
                            GetFieldValues(ControlsEnum.GETPAYROLLBYJOURNALPK);
                            if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                            {
                                CurrPK = (Int32)finTrxHdrList[0].FTH_REF_PK;
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            }
                            SetHRDepartment();
                        }
                    }
                    else if (!string.IsNullOrEmpty(prefID))
                    {
                        ResetForm(ControlsEnum.CLEARPROCESSDETAILS);
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        base.WkfRefID = ucrWrkf.RefID = int.Parse(prefID);
                        CurrPK = GetApplicationID(ucrWrkf.RefID);
                        SetHRDepartment();
                    }
                    if (CurrPK > 0)
                    {
                        EntryStatus = EntryStatus.LISTMODE;
                        FillProcessID(1);
                        WorkflowCore.CoreService objWorkflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = objWorkflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.LISTMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            ucrWrkf.ViewAction();
                        }
                        GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                        GetUIValuesFromObject(ControlsEnum.PROCESSPAYROLLHDR);
                        grdPeriodList.SelectedIndex = -1;
                        PageIndex = CommonConstants.SELECT_VALUE_ONE;
                        uclPaging.CurrentPage = 1;
                        GetFieldValues(ControlsEnum.PERIODLIST);
                        SetFieldValues(ControlsEnum.PERIODLIST);
                        SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                        GetFieldValues(ControlsEnum.WORKINGDAYSDETAILS);
                        EnableDisablePayrollHdr(false);//Disable Header Controls

                    }
                    else
                    {
                        uclPaging.CurrentPage = 1;
                        GetFieldValues(ControlsEnum.PAYROLLTYPE);
                        SetFieldValues(ControlsEnum.PAYROLLTYPE);
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ddlPayrollType.SelectedValue = dtResult.Rows[0][GTIService.Constants.HRMS.Employee.Fields.PTM_PK].ToString();
                            ActionHandler(ddlPayrollType, new EventArgs());
                        }
                        //GetFieldValues(ControlsEnum.PERIODLIST);
                        else
                        {
                            TotalPages = 0;
                            SetFieldValues(ControlsEnum.PERIODLIST);
                        }

                        SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                        //PageIndex = CommonConstants.SELECT_VALUE_ONE;
                        //uclPaging.TotalPages = TotalPages;
                        //uclPaging.CurrentPage = 1;
                        GetFieldValues(ControlsEnum.WORKINGDAYSDETAILS);
                    }



                    //// Maintain navigate from Incometax page to Payroll Detail 
                    //if (CurrEmployeePayrollPK > 0)
                    //{
                    //    SetUIEditView(commonActions);
                    //    ResetForm(ControlsEnum.EMPPAYROLLLISTTAX);
                    //    GetFieldValues(ControlsEnum.EMPPAYROLLDETAILLIST);
                    //    GetUIValuesFromObject(ControlsEnum.EMPPAYROLLDETAILLIST);
                    //    SetFieldValues(ControlsEnum.PAYEARNINGSLIST);
                    //    SetFieldValues(ControlsEnum.PAYDEDUCTIONLIST);
                    //    SetFieldValues(ControlsEnum.PAYROLLHISTORY);
                    //    hdfLastModDate.Value = objEmpPayHeader.EPH_MOD_DT.ToString();
                    //    CurrPK = objEmpPayHeader.EPS_PAYROLL_HDR;       //For Saving
                    //    LastModifiedTime = Convert.ToDateTime(hdfLastModDate.Value);
                    //    if (ShowIncomeTax)
                    //        lnkIncomeTax.Visible = true;
                    //    else
                    //        lnkIncomeTax.Visible = false;
                    //}
                    SetPrintVisibility();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        private void SetHRDepartment()
        {
            Session[BusinessObject.Common.SessionStrings.CurDept] = HRDeptPK;
            //base.SetUserDept();
        }



        private void SetWaitForServerResponse()
        {
            try
            {
                HiddenField hdfWaitForServerResponse = (HiddenField)Page.Master.FindControl("hdfWaitForServerResponse");
                hdfWaitForServerResponse.Value = "1";
            }
            catch { }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {
            ShowSalPartPayElement = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "HrmsShowSalPartPayElement")));
            string[] strInactivetabs = GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpInactiveTabs").ToString().Split(',');
            if (strInactivetabs != null && strInactivetabs.Count() > 0)
            {
                if (strInactivetabs.Contains(((int)EmpTabEnum.ITDeclaration).ToString()))
                    ShowIncomeTax = false;
            }
            MultiCurrencyEnabled = CommonFunctions.IsMultyCurrencyEnabled();
            if (Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "BranchLocationVisibility")) == false)
                ddlBranchLocation.Visible = lblBranchLocation.Visible = false;
            if (Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "DepartmentVisibility")) == false)
                txtDepartment.Visible = lblDepartment.Visible = false;
            if (Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "EmployementTypeVisibility")) == false)
                ddlEmploymentType.Visible = lblEmploymentType.Visible = false;
            if (Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "EmpTypeVisibility")) == false)
                ddlEmployeeType.Visible = lblEmployeeType.Visible = false;


        }
        #endregion

        #region --- Action Handler----
        protected void ActionHandler(object sender, EventArgs e)
        {

            try
            {
                int? result;
                result = 0;
                HiddenField hdfItemPK;
                HiddenField hdfPayrollLastModDate;
                HiddenField hdfProcessLastModDate;
                HiddenField hdfEphPK;
                DropDownList ddlWkfAction;
                TextBox WrkfComments;
                string action;
                HiddenField hdfDept;
                int dept;
                GridViewRow gdRow;
                double totProcessDays = 0;
                string TrxNo = string.Empty;
                DataTable dtErrorList = new DataTable();
                string redirectUrl = Resources.PageURL.HrmsEmpListing;
                EmpPaySlipHeader objPayDet;
                List<EmpPaySlipDetails> objPayDetList;
                string xmlDoc = string.Empty;
                string strError = string.Empty;


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
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlPayrollType")
                    {
                        commonActions = ActionsEnum.PAYROLLTYPECHANGED;
                    }
                    else if (((DropDownList)sender).ID == "ddlProcessMode")
                    {
                        commonActions = ActionsEnum.PROCESSMODECHANGED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    if (((TextBox)sender).ID == "txtSalaryMonth")
                    {
                        commonActions = ActionsEnum.SALMONTHCHANGED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(GridView)))
                {
                    //if ((sender as GridView).ID == "grdPeriodList")
                    //{
                    //    commonActions = ActionsEnum.PROCESSEDIT;
                    //}
                }

                ////To check if department is different by opening in new tab

                if (!(this.Master as ERPSMS_2).ValidatePageDept())
                    return;
                switch (commonActions)
                {
                    case ActionsEnum.REBIND:
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.PERIODLIST);
                        SetFieldValues(ControlsEnum.PERIODLIST);
                        GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                        SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                        GetUIValuesFromObject(ControlsEnum.PROCESSPAYROLLHDR);
                        break;
                    #region PROCESS PAYROLL
                    case ActionsEnum.PROCESSPAYROLL:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Payroll_Process_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            objEmpPayrollHeader = new EmpPayrollHeader();
                            objEmpPayrollHeader = (EmpPayrollHeader)SetUIValuesToObject(ControlsEnum.PROCESSPAYROLLHDR);
                            if (objEmpPayrollHeader != null)
                            {
                                objEmpPayrollHeader.IS_REPROCESS = Convert.ToByte(ProcessAction.Process);
                                if (objEmpPayrollHeader.EmpPayrollDtl == null || objEmpPayrollHeader.EmpPayrollDtl.Count > 0)
                                {
                                    TrxNo = string.Empty;
                                    objEmpPayrollHeader.WKF_FLAG = 0;
                                    xmlDoc = CommonFunctions.XmlSerialize<EmpPayrollHeader>(objEmpPayrollHeader);
                                    result = FullandFinalSettlementBL.ProcessPayrollDetails(xmlDoc, out TrxNo, out dtErrorList);
                                    if (result > 0)
                                    {
                                        #region Update dummy entry while modify payroll after approval
                                        if (Status == (int)DbStatus.APPROVED)
                                        {
                                            FinTrxService finTrxServiceClient;
                                            finTrxServiceClient = new FinTrxService();
                                            string refType = string.Empty;
                                            refType = ApplicationType.PAYRLJ;
                                            long DummyResult = objEmpPayrollHeader.EPH_PK;
                                            bool IsDummyEntry = finTrxServiceClient.IsDummyEntry(refType, objEmpPayrollHeader.EPH_PK, 0);
                                            if (IsDummyEntry == true)
                                            {
                                                DummyResult = finTrxServiceClient.DeleteFinTrx(refType, objEmpPayrollHeader.EPH_PK, 0);
                                            }
                                            if (DummyResult > 0)
                                            {
                                                finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                                DummyResult = finTrxServiceClient.GenerateDummyEntry((int)CurrPK, refType);
                                                finTrxServiceClient = null;
                                            }
                                        }
                                        #endregion

                                        //litErrorMsg.Text = Resources.Messages.SuccessProcessPayroll;
                                        lblTrxNo.Text = TrxNo;
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                                        object[] args = new object[2];
                                        args[0] = Resources.PageNameRes.Payroll;
                                        args[1] = TrxNo;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                                        CurrPK = (int)result;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                         + "','" + Resources.ErpRes.Information + "','',true);", true);
                                        EntryStatus = EntryStatus.LISTMODE;

                                        //GetFieldValues(ControlsEnum.PERIODLIST);
                                        //SetFieldValues(ControlsEnum.PERIODLIST);
                                        //GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                                        //SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                                        //GetUIValuesFromObject(ControlsEnum.PROCESSPAYROLLHDR);
                                        //hdfLastModDate.Value = objEmpPayrollHeader.LAST_MOD_DT.ToString();
                                        //LastModifiedTime = Convert.ToDateTime(hdfLastModDate.Value);


                                    }
                                    else
                                    {
                                        if (result == (int)DbSaveStatus.REFERRED)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.Payroll;
                                            litErrorMsg.Text += " " + GetGlobalResourceObject("Messages", "UsedInAnotherPlace").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.SQLERROR)
                                        {
                                            litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Err_AlreadyExistProcessPayroll").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.Payroll + " " + GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.ALREADYDELETED)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.Payroll + " " + GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.REFNOEXIST)// not in current salary year
                                        {
                                            litErrorMsg.Text = Resources.Messages.Err_CheckSalaryYear;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.ALREADYCREATED)// Working Days not found for employee type
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Err_WorkingDaysNotforEmpType").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.PENDINGEXIST)// Working Days not found for late joiners
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Err_WorkingDaysNotforLateJoiner").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.NEGATIVESAL)// Getting negative salary
                                        {
                                            strError = string.Empty;
                                            if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                            {
                                                foreach (DataRow dr in dtErrorList.Rows)
                                                {
                                                    strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["empText"]));
                                                }
                                            }
                                            litErrorMsg.Text = GetLocalResourceObject("Err_NegativeSal").ToString() + strError;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CHECKMCPRINTER)// Appraisal Exists in between this salary period
                                        {
                                            strError = string.Empty;
                                            if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                            {
                                                foreach (DataRow dr in dtErrorList.Rows)
                                                {
                                                    strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["empText"]));
                                                }
                                            }
                                            litErrorMsg.Text = GetLocalResourceObject("Err_AppraisalExist").ToString() + strError;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CHECKZBPRINTER)// Employee have leave after relieved date
                                        {
                                            strError = string.Empty;
                                            if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                            {
                                                foreach (DataRow dr in dtErrorList.Rows)
                                                {
                                                    strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["empText"]));
                                                }
                                            }
                                            litErrorMsg.Text = GetLocalResourceObject("Err_LeaveAfterRelieved").ToString() + strError;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CHECKPOUCHPRINTER)// Leave Template Missing
                                        {
                                            strError = string.Empty;
                                            if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                            {
                                                foreach (DataRow dr in dtErrorList.Rows)
                                                {
                                                    strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["empText"]));
                                                }
                                            }
                                            litErrorMsg.Text = GetLocalResourceObject("Err_LeaveTemplate").ToString() + strError;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CHECKBOXPRINTER)// Working Day Type Not Found
                                        {
                                            strError = string.Empty;
                                            if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                            {
                                                foreach (DataRow dr in dtErrorList.Rows)
                                                {
                                                    strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["empText"]));
                                                }
                                            }
                                            litErrorMsg.Text = GetLocalResourceObject("Err_WorkingDayType").ToString() + strError;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Err_ProcessPayroll").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("NoItemPickforProcessing").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region RE-PROCESS PAYROLL
                    case ActionsEnum.REPROCESSPAYROLL:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Payroll_ReProcess_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            objEmpPayrollHeader = new EmpPayrollHeader();
                            objEmpPayrollHeader = (EmpPayrollHeader)SetUIValuesToObject(ControlsEnum.PROCESSPAYROLLHDR);
                            if (objEmpPayrollHeader != null)
                            {
                                objEmpPayrollHeader.IS_REPROCESS = Convert.ToByte(ProcessAction.Reprocess);
                                if (objEmpPayrollHeader.EPH_PK > 0)
                                {
                                    if (objEmpPayrollHeader.EmpPayrollDtl == null || objEmpPayrollHeader.EmpPayrollDtl.Count > 0)
                                    {
                                        TrxNo = string.Empty;
                                        objEmpPayrollHeader.WKF_FLAG = 0;
                                        xmlDoc = CommonFunctions.XmlSerialize<EmpPayrollHeader>(objEmpPayrollHeader);
                                        result = FullandFinalSettlementBL.ProcessPayrollDetails(xmlDoc, out TrxNo, out dtErrorList);
                                        if (result > 0)
                                        {
                                            #region Update dummy entry while modify payroll after approval
                                            if (Status == (int)DbStatus.APPROVED)
                                            {
                                                FinTrxService finTrxServiceClient;
                                                finTrxServiceClient = new FinTrxService();
                                                string refType = string.Empty;
                                                refType = ApplicationType.PAYRLJ;
                                                long DummyResult = objEmpPayrollHeader.EPH_PK;
                                                bool IsDummyEntry = finTrxServiceClient.IsDummyEntry(refType, objEmpPayrollHeader.EPH_PK, 0);
                                                if (IsDummyEntry == true)
                                                {
                                                    DummyResult = finTrxServiceClient.DeleteFinTrx(refType, objEmpPayrollHeader.EPH_PK, 0);
                                                }
                                                if (DummyResult > 0)
                                                {
                                                    finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                                    DummyResult = finTrxServiceClient.GenerateDummyEntry((int)CurrPK, refType);
                                                    finTrxServiceClient = null;
                                                }
                                            }
                                            #endregion

                                            strError = string.Empty;
                                            if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                            {
                                                strError = "<br />" + GetLocalResourceObject("MsgNegativeSalaryEmployees").ToString();
                                                foreach (DataRow dr in dtErrorList.Rows)
                                                {
                                                    strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["empText"]));
                                                }
                                            }

                                            lblTrxNo.Text = TrxNo;
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString() + strError;
                                            object[] args = new object[2];
                                            args[0] = Resources.PageNameRes.Payroll;
                                            args[1] = TrxNo;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                                            EntryStatus = EntryStatus.LISTMODE;
                                            CurrPK = (int)result;
                                            //GetFieldValues(ControlsEnum.PERIODLIST);
                                            //SetFieldValues(ControlsEnum.PERIODLIST);
                                            //GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                                            //SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                                            //GetUIValuesFromObject(ControlsEnum.PROCESSPAYROLLHDR);
                                            //hdfLastModDate.Value = objEmpPayrollHeader.LAST_MOD_DT.ToString();
                                            //LastModifiedTime = Convert.ToDateTime(hdfLastModDate.Value);

                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                              + "','" + Resources.ErpRes.Information + "','',true);", true);
                                        }
                                        else
                                        {
                                            if (result == (int)DbSaveStatus.REFERRED)
                                            {
                                                litErrorMsg.Text = Resources.PageNameRes.Payroll;
                                                litErrorMsg.Text += " " + GetGlobalResourceObject("Messages", "UsedInAnotherPlace").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.SQLERROR)
                                            {
                                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.CODEEXIST)
                                            {
                                                litErrorMsg.Text = GetLocalResourceObject("Err_AlreadyExistProcessPayroll").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                                            {
                                                litErrorMsg.Text = Resources.PageNameRes.Payroll + " " +
                                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                                            {
                                                litErrorMsg.Text = Resources.PageNameRes.Payroll + " " + GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.REFNOEXIST)// not in current salary year
                                            {
                                                litErrorMsg.Text = Resources.Messages.Err_CheckSalaryYear;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.ALREADYCREATED)// Working Days not found for employee type
                                            {
                                                litErrorMsg.Text = GetLocalResourceObject("Err_WorkingDaysNotforEmpType").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.PENDINGEXIST)// Working Days not found for late joiners
                                            {
                                                litErrorMsg.Text = GetLocalResourceObject("Err_WorkingDaysNotforLateJoiner").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.NEGATIVESAL)// Getting negative salary
                                            {
                                                strError = string.Empty;
                                                if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                                {
                                                    foreach (DataRow dr in dtErrorList.Rows)
                                                    {
                                                        strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["empText"]));
                                                    }
                                                }
                                                litErrorMsg.Text = GetLocalResourceObject("Err_NegativeSal").ToString() + strError;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.CHECKMCPRINTER)// Appraisal Exists in between this salary period
                                            {
                                                strError = string.Empty;
                                                if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                                {
                                                    foreach (DataRow dr in dtErrorList.Rows)
                                                    {
                                                        strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["empText"]));
                                                    }
                                                }
                                                litErrorMsg.Text = GetLocalResourceObject("Err_AppraisalExist").ToString() + strError;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.CHECKZBPRINTER)// Employee have leave after relieved date
                                            {
                                                strError = string.Empty;
                                                if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                                {
                                                    foreach (DataRow dr in dtErrorList.Rows)
                                                    {
                                                        strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["empText"]));
                                                    }
                                                }
                                                litErrorMsg.Text = GetLocalResourceObject("Err_LeaveAfterRelieved").ToString() + strError;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.CHECKPOUCHPRINTER)// Leave Template Missing
                                            {
                                                strError = string.Empty;
                                                if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                                {
                                                    foreach (DataRow dr in dtErrorList.Rows)
                                                    {
                                                        strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["empText"]));
                                                    }
                                                }
                                                litErrorMsg.Text = GetLocalResourceObject("Err_LeaveTemplate").ToString() + strError;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.CHECKBOXPRINTER)// Working Day Type Not Found
                                            {
                                                strError = string.Empty;
                                                if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                                {
                                                    foreach (DataRow dr in dtErrorList.Rows)
                                                    {
                                                        strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["empText"]));
                                                    }
                                                }
                                                litErrorMsg.Text = GetLocalResourceObject("Err_WorkingDayType").ToString() + strError;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else
                                            {
                                                litErrorMsg.Text = GetLocalResourceObject("Err_ReProcessPayroll").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("NoItemPickforProcessing").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_SelectPeriod").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup   
                        if (NegativeSalExist > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NegativeSalary").ToString())
                                               + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                            ucrWrkf.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        }
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
                                objEmpPayrollHeader = new EmpPayrollHeader();
                                objEmpPayrollHeader = (EmpPayrollHeader)SetUIValuesToObject(ControlsEnum.PROCESSPAYROLLHDR);
                                if (objEmpPayrollHeader != null)
                                {
                                    objEmpPayrollHeader.IS_REPROCESS = Convert.ToByte(ProcessAction.Reprocess);
                                    if (objEmpPayrollHeader.EPH_PK > 0)
                                    {
                                        if (objEmpPayrollHeader.EmpPayrollDtl == null || objEmpPayrollHeader.EmpPayrollDtl.Count > 0)
                                        {
                                            TrxNo = string.Empty;
                                            objEmpPayrollHeader.WKF_FLAG = 0;
                                            xmlDoc = CommonFunctions.XmlSerialize<EmpPayrollHeader>(objEmpPayrollHeader);
                                            result = FullandFinalSettlementBL.ProcessPayrollDetails(xmlDoc, out TrxNo, out dtErrorList);
                                            if (result > 0)
                                            {
                                                ////litErrorMsg.Text = Resources.Messages.SuccessReProcessPayroll;
                                                //lblTrxNo.Text = TrxNo;
                                                //litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                                                //object[] args = new object[2];
                                                //args[0] = Resources.PageNameRes.Payroll;
                                                //args[1] = TrxNo;
                                                //litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                                                lblTrxNo.Text = TrxNo;
                                                ucrWrkf.ApplicationID = result.Value;

                                                //EntryStatus = EntryStatus.LISTMODE;
                                                //CurrPK = (int)result;
                                                //GetFieldValues(ControlsEnum.PERIODLIST);
                                                //SetFieldValues(ControlsEnum.PERIODLIST);
                                                //GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                                                //SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                                                //GetUIValuesFromObject(ControlsEnum.PROCESSPAYROLLHDR);
                                                ////hdfLastModDate.Value = objEmpPayrollHeader.LAST_MOD_DT.ToString();
                                                ////LastModifiedTime = Convert.ToDateTime(hdfLastModDate.Value);

                                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                //  + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else
                                            {
                                                if (result == (int)DbSaveStatus.REFERRED)
                                                {
                                                    litErrorMsg.Text = Resources.PageNameRes.Payroll;
                                                    litErrorMsg.Text += " " + GetGlobalResourceObject("Messages", "UsedInAnotherPlace").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                        + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                                else if (result == (int)DbSaveStatus.SQLERROR)
                                                {
                                                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                        + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                                {
                                                    litErrorMsg.Text = GetLocalResourceObject("Err_AlreadyExistProcessPayroll").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                        + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                                {
                                                    litErrorMsg.Text = Resources.PageNameRes.Payroll + " " +
                                                        GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                                else if (result == (int)DbSaveStatus.ALREADYDELETED)
                                                {
                                                    litErrorMsg.Text = Resources.PageNameRes.Payroll + " " + GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                                else if (result == (int)DbSaveStatus.REFNOEXIST)// not in current salary year
                                                {
                                                    litErrorMsg.Text = Resources.Messages.Err_CheckSalaryYear;
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                        + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                                else if (result == (int)DbSaveStatus.ALREADYCREATED)// Working Days not found for employee type
                                                {
                                                    litErrorMsg.Text = GetLocalResourceObject("Err_WorkingDaysNotforEmpType").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                        + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                                else if (result == (int)DbSaveStatus.PENDINGEXIST)// Working Days not found for late joiners
                                                {
                                                    litErrorMsg.Text = GetLocalResourceObject("Err_WorkingDaysNotforLateJoiner").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                        + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                                else if (result == (int)DbSaveStatus.NEGATIVESAL)// Getting negative salary
                                                {
                                                    strError = string.Empty;
                                                    if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                                    {
                                                        foreach (DataRow dr in dtErrorList.Rows)
                                                        {
                                                            strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["empText"]));
                                                        }
                                                    }
                                                    litErrorMsg.Text = GetLocalResourceObject("Err_NegativeSal").ToString() + strError;
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                        + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                                else if (result == (int)DbSaveStatus.CHECKMCPRINTER)// Appraisal Exists in between this salary period
                                                {
                                                    strError = string.Empty;
                                                    if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                                    {
                                                        foreach (DataRow dr in dtErrorList.Rows)
                                                        {
                                                            strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["empText"]));
                                                        }
                                                    }
                                                    litErrorMsg.Text = GetLocalResourceObject("Err_AppraisalExist").ToString() + strError;
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                                else if (result == (int)DbSaveStatus.CHECKZBPRINTER)// Employee have leave after relieved date
                                                {
                                                    strError = string.Empty;
                                                    if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                                    {
                                                        foreach (DataRow dr in dtErrorList.Rows)
                                                        {
                                                            strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["empText"]));
                                                        }
                                                    }
                                                    litErrorMsg.Text = GetLocalResourceObject("Err_LeaveAfterRelieved").ToString() + strError;
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                        + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                                else if (result == (int)DbSaveStatus.CHECKPOUCHPRINTER)// Leave Template Missing
                                                {
                                                    strError = string.Empty;
                                                    if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                                    {
                                                        foreach (DataRow dr in dtErrorList.Rows)
                                                        {
                                                            strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["empText"]));
                                                        }
                                                    }
                                                    litErrorMsg.Text = GetLocalResourceObject("Err_LeaveTemplate").ToString() + strError;
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                        + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                                else if (result == (int)DbSaveStatus.CHECKBOXPRINTER)// Working Day Type Not Found
                                                {
                                                    strError = string.Empty;
                                                    if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                                    {
                                                        foreach (DataRow dr in dtErrorList.Rows)
                                                        {
                                                            strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["empText"]));
                                                        }
                                                    }
                                                    litErrorMsg.Text = GetLocalResourceObject("Err_WorkingDayType").ToString() + strError;
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                        + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                                else
                                                {
                                                    litErrorMsg.Text = GetLocalResourceObject("Err_ReProcessPayroll").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("NoItemPickforProcessing").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Err_SelectPeriod").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }


                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(CurrPK, ApplicationType.PAYRL))
                                {
                                    ucrWrkf.ApplicationID = CurrPK;
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_Payroll_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        FillProcessID(1);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    EntryStatus = EntryStatus.LISTMODE;
                                    GetFieldValues(ControlsEnum.PERIODLIST);
                                    SetFieldValues(ControlsEnum.PERIODLIST);
                                    GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                                    SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                                    GetUIValuesFromObject(ControlsEnum.PROCESSPAYROLLHDR);

                                }
                            }
                            else
                                ucrWrkf.ApplicationID = CurrPK;
                            if (ucrWrkf.ApplicationID > 0)
                            {
                                ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                //Do WorkFlow if WorkFlow has Actions
                                if (ddlWkfAction.Items.Count > 0)
                                {
                                    action = ddlWkfAction.SelectedItem.ToString();
                                    result = ucrWrkf.DoWorkFlow();
                                    if (result.HasValue && result.Value > 0)
                                    {

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
                                        WrkfComments.Text = "";

                                        object[] args = new object[2];
                                        args[0] = Resources.PageNameRes.Payroll;
                                        args[1] = lblTrxNo.Text.Trim();
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                                        EntryStatus = EntryStatus.LISTMODE;
                                        GetFieldValues(ControlsEnum.PERIODLIST);
                                        SetFieldValues(ControlsEnum.PERIODLIST);
                                        GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                                        SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                                        GetUIValuesFromObject(ControlsEnum.PROCESSPAYROLLHDR);
                                        #region Reset Workflow
                                        FillProcessID(1);
                                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
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

                                        // Show Save Message and redired to listing page                                      
                                        if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region DELETESUBMIT
                    case ActionsEnum.DELETESUBMIT:
                        ucrWrkf.Reset();
                        FillProcessID(11);
                        WorkflowCore.CoreService workflowCore1 = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore1.GetRefID((int)CurrPK, ucrWrkf.ProcessID);
                        SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.LISTMODE && ucrWrkf.HasPageTaskPermission)
                        {
                            ucrWrkf.ViewType = 1;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 0;
                        }
                        ucrWrkf.ViewAction();
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region NEW
                    case ActionsEnum.NEW:
                        ResetForm(ControlsEnum.CLEARPROCESSDETAILS);
                        EnableDisablePayrollHdr(true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "RemoveGridRowColor", "RemoveGridRowColor('" + grdPeriodList.ClientID + "');", true);
                        grdPeriodList.SelectedIndex = -1;
                        GetFieldValues(ControlsEnum.COMPANY);
                        SetFieldValues(ControlsEnum.COMPANY);

                        GetFieldValues(ControlsEnum.CURRENCY);
                        SetFieldValues(ControlsEnum.CURRENCY);
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        SetFieldValues(ControlsEnum.EXCHANGERATE);
                        // GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                        SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                        GetFieldValues(ControlsEnum.WORKINGDAYSDETAILS);
                        //SetFieldValues(ControlsEnum.WORKINGDAYSDETAILS);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideProcessDetail", "ShowHideProcessDetail(1)", true);
                        base.WkfRefID = ucrWrkf.RefID = 0;
                        ucrWrkf.FillWorkFlowDetails();
                        ucrWrkf.ViewType = 1;
                        ucrWrkf.ViewAction();
                        break;
                    #endregion
                    #region SAVE EMPLOYEE PAYROLL
                    case ActionsEnum.SAVEEMPLOYEEPAYROLL:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            objEmpPayHeader = new EmpPayrollPayHeader();
                            objEmpPayHeader = (EmpPayrollPayHeader)SetUIValuesToObject(ControlsEnum.EMPPAYROLLHDR);

                            if (objEmpPayHeader != null)
                            {
                                xmlDoc = CommonFunctions.XmlSerialize<EmpPayrollPayHeader>(objEmpPayHeader);
                                result = FullandFinalSettlementBL.SaveEmployeePayroll(xmlDoc);
                                if (result > 0)
                                {
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmpPayrollDetails);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                                    SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                                    hdfLastModDate.Value = objEmpPayrollHeader.LAST_MOD_DT.ToString();
                                    LastModifiedTime = Convert.ToDateTime(hdfLastModDate.Value);
                                    NegativeSalExist = objEmpPayrollHeader.EPH_FAILED_COUNT;
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_EmpPayroll").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region PROCESS EDIT
                    case ActionsEnum.PROCESSEDIT:
                        //GridViewRow row = grdPeriodList.SelectedRow;
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEARPROCESSDETAILS);
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        gdRow = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        hdfEphPK = gdRow.FindControl("hdfEphPK") as HiddenField;
                        CurrPK = Convert.ToInt32(hdfEphPK.Value);
                        hdfDept = gdRow.FindControl("hdfDept") as HiddenField;
                        if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                        {
                            Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                            base.SetUserDept();
                        }
                        HiddenField hdfStatus = (HiddenField)gdRow.FindControl("hdfStatus");
                        HiddenField hdfDelStatus = (HiddenField)gdRow.FindControl("hdfDelStatus");
                        HiddenField hdfJournalStatus = (HiddenField)gdRow.FindControl("hdfJournalStatus");
                        hdfIsCancelled.Value = hdfDelStatus.Value;
                        Status = Convert.ToInt32(hdfStatus.Value);
                        JournalStatus = Convert.ToInt32(hdfJournalStatus.Value);
                        FillProcessID(1);
                        WorkflowCore.CoreService objWorkflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = objWorkflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.LISTMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            ucrWrkf.ViewAction();
                        }
                        GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                        GetUIValuesFromObject(ControlsEnum.PROCESSPAYROLLHDR);
                        SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                        EnableDisablePayrollHdr(false);//Disable Header Controls

                        #region SET & RESET ROW COLOR
                        curRow = ((sender as ImageButton).Parent.Parent as GridViewRow);
                        //curRow.BackColor = System.Drawing.ColorTranslator.FromHtml(GetLocalResourceObject("ProcessedRowColor").ToString());
                        //if (prvRow != null)
                        //{
                        foreach (GridViewRow row in grdPeriodList.Rows)
                        {
                            //if (((HiddenField)row.FindControl("hdfEphPK")).Value != ((HiddenField)curRow.FindControl("hdfEphPK")).Value
                            //        && ((HiddenField)row.FindControl("hdfEphPK")).Value == ((HiddenField)prvRow.FindControl("hdfEphPK")).Value)
                            //{
                            row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetLocalResourceObject("RowColor").ToString());
                            //    break;
                            //}
                        }
                        //}
                        //prvRow = ((sender as ImageButton).Parent.Parent as GridViewRow);
                        curRow.BackColor = System.Drawing.ColorTranslator.FromHtml(GetLocalResourceObject("ProcessedRowColor").ToString());
                        #endregion
                        break;
                    #endregion
                    #region VIEW EMPLOYEE PAYROLL DETAILS
                    case ActionsEnum.VIEWPAYROLL:
                        SetUIEditView(commonActions);
                        ResetForm(ControlsEnum.EMPPAYROLLDETAILLIST);
                        hdfItemPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                        if (hdfItemPK != null)
                        {
                            CurrEmployeePayrollPK = Convert.ToInt32(hdfItemPK.Value);
                            GetFieldValues(ControlsEnum.EMPPAYROLLDETAILLIST);
                            GetUIValuesFromObject(ControlsEnum.EMPPAYROLLDETAILLIST);
                            SetFieldValues(ControlsEnum.PAYEARNINGSLIST);
                            SetFieldValues(ControlsEnum.PAYDEDUCTIONLIST);
                        }
                        break;
                    #endregion
                    #region EDIT EMPLOYEE PAYROLL DETAILS
                    case ActionsEnum.EDITPAYROLL:
                        SetUIEditView(commonActions);
                        ResetForm(ControlsEnum.EMPPAYROLLDETAILLIST);
                        hdfItemPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                        if (hdfItemPK != null)
                        {
                            CurrEmployeePayrollPK = Convert.ToInt32(hdfItemPK.Value);
                            GetFieldValues(ControlsEnum.EMPPAYROLLDETAILLIST);
                            GetUIValuesFromObject(ControlsEnum.EMPPAYROLLDETAILLIST);
                            SetFieldValues(ControlsEnum.PAYEARNINGSLIST);
                            SetFieldValues(ControlsEnum.PAYDEDUCTIONLIST);
                            SetFieldValues(ControlsEnum.PAYROLLHISTORY);
                            hdfLastModDate.Value = objEmpPayHeader.EPH_MOD_DT.ToString();
                            LastModifiedTime = Convert.ToDateTime(hdfLastModDate.Value);
                            if (ShowIncomeTax)
                                lnkIncomeTax.Visible = true;
                            else
                                lnkIncomeTax.Visible = false;
                        }
                        if (ShowIncomeTax)
                            btnIncomeTax.Visible = true;
                        else
                            btnIncomeTax.Visible = false;
                        break;
                    #endregion
                    #region INCOME TAX
                    case ActionsEnum.INCOMETAX:
                        redirectUrl = Resources.PageURL.HrmsIncomeTax;
                        CheckUserRightsAndRedirect(redirectUrl, commonActions);
                        //CheckUserRightsAndRedirect(redirectUrl);
                        //Response.Redirect(redirectUrl, false);
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        EntryStatus = EntryStatus.LISTMODE;
                        hdfFilter.Value = "1";
                        GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                        SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                        EnableDisablePayrollHdr(false);//Disable Header Controls
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        EntryStatus = EntryStatus.LISTMODE;
                        hdfFilter.Value = "1";
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "RemoveGridRowColor", "RemoveGridRowColor('" + grdPeriodList.ClientID + "');", true);
                        // GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                        SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                        EnableDisablePayrollHdr(true);//Enable Header Controls
                        break;
                    #endregion
                    #region EMPLOYEE PROCESS PAYROLL LIST
                    case ActionsEnum.LIST:
                        CurrPK = 0;
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.EMPPAYROLLLIST);
                        GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                        SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                        lnkIncomeTax.Visible = false;
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        EntryStatus = EntryStatus.LISTMODE;
                        CurrEmployeePayrollPK = 0;
                        ViewState[ViewstateStrings.LastModifiedTime] = null;
                        GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                        SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                        GetUIValuesFromObject(ControlsEnum.PROCESSPAYROLLHDR);
                        lnkIncomeTax.Visible = false;
                        break;
                    #endregion
                    #region PRINT
                    case ActionsEnum.PRINT:
                        EmpPaySlipHeaderSession = null;
                        EmployeePayrollPK = CurrEmployeePayrollPK;
                        EmpPaySlipHeader ObjEmpPaySlipHeaderSingle = new EmpPaySlipHeader();
                        ObjEmpPaySlipHeaderSingle = (EmpPaySlipHeader)SetUIValuesToObject(ControlsEnum.PRINTPAYSLIPSINGLE);
                        EmpPaySlipHeaderSession = ObjEmpPaySlipHeaderSingle;
                        if (Convert.ToInt32(ddlProcessMode.SelectedValue) == (int)PayrollProcessMode.Monthly)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "FSMT" + "&APPSUBTYPE= 0") + "');", true);//HRMS Pay Slip Monthly
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "PAYRL" + "&APPSUBTYPE= 1") + "');", true);//(Daily)HRMS Pay Slip
                        }
                        break;
                    #endregion
                    #region PRINTINCOMETAX
                    case ActionsEnum.PRINTINCOMETAX:
                        EmpPaySlipHeaderSession = null;
                        //EmployeePayrollPK = 
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=" + hdfPayrollEmployee.Value + "&APPTYPE=" + "ITC" + "&APPSUBTYPE= 0" + "&CurPK=" + CurrEmployeePayrollPK) + "');", true);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "PAYRL" + "&APPSUBTYPE= 1" + "&CurPK=" + CurrEmployeePayrollPK) + "');", true);
                        break;
                    #endregion
                    #region PRINT PAY SLIP
                    case ActionsEnum.PRINTPAYSLIP:
                        EmpPaySlipHeaderSession = null;
                        HiddenField hdfDetPkHistory = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfDetPk") as HiddenField);
                        EmployeePayrollPK = Convert.ToInt32(hdfDetPkHistory.Value);
                        EmpPaySlipHeader ObjEmpPaySlipHeaderHistory = new EmpPaySlipHeader();
                        ObjEmpPaySlipHeaderSingle = (EmpPaySlipHeader)SetUIValuesToObject(ControlsEnum.PRINTPAYSLIPSINGLE);
                        EmpPaySlipHeaderSession = ObjEmpPaySlipHeaderSingle;
                        if (Convert.ToInt32(ddlProcessMode.SelectedValue) == (int)PayrollProcessMode.Monthly)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "PAYRL" + "&APPSUBTYPE= 9") + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "PAYRL" + "&APPSUBTYPE= 1") + "');", true);
                        }
                        break;
                    #endregion
                    #region PRINT ALL
                    case ActionsEnum.PRINTALL:
                        if (Convert.ToInt32(ddlProcessMode.SelectedValue) == (int)PayrollProcessMode.Monthly)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "FSMT" + "&APPSUBTYPE= 0" + "&CurPK=" + CurrPK) + "');", true);
                        }
                        else if (Convert.ToInt32(ddlProcessMode.SelectedValue) == (int)PayrollProcessMode.Periodic)
                        {
                          //  ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "PAYRL" + "&APPSUBTYPE= 2" + "&CurPK=" + CurrPK) + "');", true);
                        }
                        break;
                    #endregion
                    #region PRINT PAYROLL
                    case ActionsEnum.PRINTPAYROLL:
                        String originalPath = new Uri(HttpContext.Current.Request.Url.AbsoluteUri).OriginalString;
                        String parentDirectory = originalPath.Substring(0, originalPath.LastIndexOf("/"));
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "PAYRL" + "&APPSUBTYPE= 5" + "&CurPK=" + CurrPK) + "&HLURL=" + parentDirectory + "');", true);
                        break;
                    #endregion
                    #region PRINT MULTIPLE PAY SLIP
                    case ActionsEnum.PRINTMULTIPLE:
                        EmpPaySlipHeaderSession = null;
                        EmpPaySlipHeader ObjEmpPaySlipHeader = new EmpPaySlipHeader();
                        ObjEmpPaySlipHeader = (EmpPaySlipHeader)SetUIValuesToObject(ControlsEnum.PRINTMULTIPLE);
                        EmpPaySlipHeaderSession = ObjEmpPaySlipHeader;
                        if (Convert.ToInt32(ddlProcessMode.SelectedValue) == (int)PayrollProcessMode.Monthly)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "FSMT" + "&APPSUBTYPE= 0") + "');", true);//HRMS Pay Slip Monthly
                        }
                        else
                        {
                           // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "FSMT" + "&APPSUBTYPE= 0") + "');", true);//Daily
                        }
                        break;
                    #endregion
                    #region PRINT PAY SLIP GRID
                    case ActionsEnum.PRINTGRID:
                        EmpPaySlipHeaderSession = null;
                        HiddenField hdfDetPkGrid = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                        EmployeePayrollPK = Convert.ToInt32(hdfDetPkGrid.Value);
                        EmpPaySlipHeader ObjEmpPaySlipHeaderGrid = new EmpPaySlipHeader();
                        ObjEmpPaySlipHeaderSingle = (EmpPaySlipHeader)SetUIValuesToObject(ControlsEnum.PRINTPAYSLIPSINGLE);
                        EmpPaySlipHeaderSession = ObjEmpPaySlipHeaderSingle;
                        if (Convert.ToInt32(ddlProcessMode.SelectedValue) == (int)PayrollProcessMode.Monthly)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "FSMT" + "&APPSUBTYPE= 0") + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "PAYRL" + "&APPSUBTYPE= 1") + "');", true);
                        }
                        break;
                    #endregion
                    #region FILTER
                    case ActionsEnum.FILTER:
                        //uclPaging.CurrentPage = 0;
                        //this.PageIndexList = "1";
                        this.EntryStatus = EntryStatus.LISTMODE;
                        //this.currPK = 0;
                        GetFieldValues(ControlsEnum.PERIODLIST);
                        SetFieldValues(ControlsEnum.PERIODLIST);
                        break;
                    #endregion
                    #region REMOVE EARNINGS PAY
                    case ActionsEnum.REMOVEEARNINGSPAY:
                        if (empPayDtlList != null && empPayDtlList.Count > 0)
                        {
                            HiddenField hdfEarnPayPK = (((sender as Button).Parent.Parent as GridViewRow).FindControl("hdfEarnPayPK") as HiddenField);
                            var removeEarn = empPayDtlList.SingleOrDefault(row => row.EPP_PK == Convert.ToInt32(hdfEarnPayPK.Value));
                            if (removeEarn != null)
                            {
                                empPayDtlList.Remove(removeEarn);
                                objEmpPayHeader = new EmpPayrollPayHeader();
                                objEmpPayHeader.EmpPayrollPayDtl = empPayDtlList;
                                BindGrid(ControlsEnum.PAYEARNINGSLIST);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailed;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region REMOVE DEDUCTION PAY
                    case ActionsEnum.REMOVEDEDUCTPAY:
                        if (empPayDtlList != null && empPayDtlList.Count > 0)
                        {
                            HiddenField hdfDeductPayPK = (((sender as Button).Parent.Parent as GridViewRow).FindControl("hdfDeductPayPK") as HiddenField);
                            var removeDeduct = empPayDtlList.SingleOrDefault(row => row.EPP_PK == Convert.ToInt32(hdfDeductPayPK.Value));
                            if (removeDeduct != null)
                            {
                                empPayDtlList.Remove(removeDeduct);
                                objEmpPayHeader = new EmpPayrollPayHeader();
                                objEmpPayHeader.EmpPayrollPayDtl = empPayDtlList;
                                BindGrid(ControlsEnum.PAYDEDUCTIONLIST);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailed;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region PAYROLL TYPE CHANGED
                    case ActionsEnum.PAYROLLTYPECHANGED:
                        this.EntryStatus = EntryStatus.LISTMODE;
                        txtMonthFrom.Text = string.Empty;
                        txtMonthTo.Text = string.Empty;
                        ResetForm(ControlsEnum.CLEARPROCESSDETAILS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "RemoveGridRowColor", "RemoveGridRowColor('" + grdPeriodList.ClientID + "');", true);
                        grdPeriodList.SelectedIndex = -1;
                        PageIndex = CommonConstants.SELECT_VALUE_ONE;
                        uclPaging.CurrentPage = 1;
                        GetFieldValues(ControlsEnum.PERIODLIST);
                        SetFieldValues(ControlsEnum.PERIODLIST);
                        //GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                        SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                        GetFieldValues(ControlsEnum.WORKINGDAYSDETAILS);
                        PayrollTypePk = Convert.ToInt32(ddlPayrollType.SelectedValue);
                        GetFieldValues(ControlsEnum.PAYROLLTYPEDETAILS);
                        SetFieldValues(ControlsEnum.PAYROLLTYPEDETAILS);
                        SetPayrollCaption();
                        break;
                    #endregion
                    #region PROCESS MODE CHANGED
                    case ActionsEnum.PROCESSMODECHANGED:
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEARPROCESSDETAILS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "RemoveGridRowColor", "RemoveGridRowColor('" + grdPeriodList.ClientID + "');", true);
                        grdPeriodList.SelectedIndex = -1;
                        PageIndex = CommonConstants.SELECT_VALUE_ONE;
                        uclPaging.CurrentPage = 1;
                        GetFieldValues(ControlsEnum.PAYROLLTYPE);
                        SetFieldValues(ControlsEnum.PAYROLLTYPE);
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ddlPayrollType.SelectedValue = dtResult.Rows[0][GTIService.Constants.HRMS.Employee.Fields.PTM_PK].ToString();
                        }
                        GetFieldValues(ControlsEnum.PERIODLIST);
                        SetFieldValues(ControlsEnum.PERIODLIST);
                        //GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                        SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                        GetFieldValues(ControlsEnum.WORKINGDAYSDETAILS);
                        if (Convert.ToInt32(ddlPayrollType.SelectedValue) > 0)
                            ActionHandler(ddlPayrollType, new EventArgs());
                        SetPayrollCaption();
                        break;
                    #endregion
                    #region WORKING DAYS POPUP
                    case ActionsEnum.WORKINGDAYSPOPUP:
                        if (workdayDetailList == null)
                        {
                            GetFieldValues(ControlsEnum.WORKINGDAYSDETAILS);
                        }
                        SetFieldValues(ControlsEnum.WORKINGDAYSDETAILS);
                        totProcessDays = ((Convert.ToDateTime(txtProcessToDate.Text) - Convert.ToDateTime(txtProcessFromDate.Text)).TotalDays) + 1;
                        hdfTotalProcessDays.Value = totProcessDays.ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpWorkingDays]','" + GetLocalResourceObject("WorkDayDetails").ToString() + "','660','330');", true);
                        break;
                    #endregion
                    #region WORKING DAYS APPLY
                    case ActionsEnum.WORKDAYSAPPLY:
                        foreach (GridViewRow grdRow in grdWorkingDays.Rows)
                        {
                            TextBox txtWorkDays = (TextBox)grdRow.FindControl("txtWorkDays");
                            TextBox txtPaidHolidays = (TextBox)grdRow.FindControl("txtPaidHolidays");
                            HiddenField hdfEmploymentTypePK = (HiddenField)grdRow.FindControl("hdfEmploymentTypePK");
                            objWorkDtls = workdayDetailList.SingleOrDefault(dtl => dtl.EPW_EMP_TYPE == Convert.ToInt32(hdfEmploymentTypePK.Value));
                            if (objWorkDtls != null)
                            {
                                objWorkDtls.EPW_HOLIDAYS = string.IsNullOrEmpty(txtPaidHolidays.Text) ? 0 : Convert.ToDouble(txtPaidHolidays.Text);
                                objWorkDtls.EPW_WORK_DAYS = string.IsNullOrEmpty(txtWorkDays.Text) ? 0 : Convert.ToDouble(txtWorkDays.Text);
                                objWorkDtls.EPW_ACTIVE = (int)DbActiveStatus.ACTIVE;
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
                        break;
                    #endregion
                    #region DELETE PERIOD
                    case ActionsEnum.DELETEPERIOD:
                        hdfEphPK = ((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfEphPK") as HiddenField;
                        hdfProcessLastModDate = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfProcessLastModDate") as HiddenField);

                        if (hdfEphPK != null)
                        {
                            CurrPK = Convert.ToInt32(hdfEphPK.Value);
                            result = FullandFinalSettlementBL.DeleteProcessPeriod(this.CurrPK, Convert.ToDateTime(hdfProcessLastModDate.Value).ToString());
                        }
                        if (result > 0)
                        {
                            EntryStatus = EntryStatus.LISTMODE;
                            ResetForm(ControlsEnum.CLEARPROCESSDETAILS);

                            if (grdPeriodList.Rows.Count == 1 && Convert.ToInt32(PageIndex) > 1)
                            {
                                PageIndex = (Convert.ToInt32(PageIndex) - 1).ToString();
                            }
                            GetFieldValues(ControlsEnum.PERIODLIST);
                            SetFieldValues(ControlsEnum.PERIODLIST);
                            GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                            SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ProcessPeriod);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.ProcessPeriod;
                                litErrorMsg.Text += " " + GetGlobalResourceObject("Messages", "UsedInAnotherPlace").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.ProcessPeriod + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            //else if (result == (int)DbSaveStatus.CODEEXIST)
                            //{
                            //    litErrorMsg.Text = Resources.PageNameRes.ProcessPeriod + " " +
                            //        GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            //    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            //}
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.ProcessPeriod + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ProcessPeriod);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region DELETE PAYROLL
                    case ActionsEnum.DELETEPAYROLL:
                        hdfItemPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                        hdfPayrollLastModDate = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfPayrollLastModDate") as HiddenField);
                        objPayDet = new EmpPaySlipHeader();
                        objPayDetList = new List<EmpPaySlipDetails>();
                        if (hdfItemPK != null)
                        {
                            EmpPaySlipDetails DetPk = new EmpPaySlipDetails();
                            //CurrEmployeePayrollPK = Convert.ToInt32(hdfItemPK.Value);
                            objPayDet.EPH_PK = CurrPK;
                            objPayDet.LAST_MOD_DT = LastModifiedTime; // Convert.ToDateTime(hdfPayrollLastModDate.Value);
                            DetPk.EPS_PK = Convert.ToInt32(hdfItemPK.Value).ToString();
                            objPayDetList.Add(DetPk);
                            objPayDet.EmpPaySlipDetails = objPayDetList;
                            xmlDoc = CommonFunctions.XmlSerialize<EmpPaySlipHeader>(objPayDet);
                            result = FullandFinalSettlementBL.DeleteEmployeePayroll(xmlDoc);
                        }
                        if (result > 0)
                        {
                            EntryStatus = EntryStatus.LISTMODE;
                            GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                            SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                            if (objEmpPayrollHeader != null)
                                lblCurrProcessCount.Text = objEmpPayrollHeader.EmpPayrollDtl.Count.ToString() + "/" + objEmpPayrollHeader.EPH_PROCESSED_COUNT.ToString();
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Payroll);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.Payroll;
                                litErrorMsg.Text += " " + GetGlobalResourceObject("Messages", "UsedInAnotherPlace").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.Payroll + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            //else if (result == (int)DbSaveStatus.CODEEXIST)
                            //{
                            //    litErrorMsg.Text = Resources.PageNameRes.Payroll + " " +
                            //        GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            //    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            //}
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.Payroll + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Payroll);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region DELETE ALL
                    case ActionsEnum.DELETEALL:
                        objPayDet = new EmpPaySlipHeader();
                        objPayDet = (EmpPaySlipHeader)SetUIValuesToObject(ControlsEnum.DELETEALL);
                        if (objPayDet == null || objPayDet.EmpPaySlipDetails == null || objPayDet.EmpPaySlipDetails.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoEmpSelected").ToString())
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }
                        xmlDoc = CommonFunctions.XmlSerialize<EmpPaySlipHeader>(objPayDet);
                        result = FullandFinalSettlementBL.DeleteEmployeePayroll(xmlDoc);
                        if (result > 0)
                        {
                            EntryStatus = EntryStatus.LISTMODE;
                            GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                            SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Payroll);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.Payroll;
                                litErrorMsg.Text += " " + GetGlobalResourceObject("Messages", "UsedInAnotherPlace").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.Payroll + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            //else if (result == (int)DbSaveStatus.CODEEXIST)
                            //{
                            //    litErrorMsg.Text = Resources.PageNameRes.ProcessPeriod + " " +
                            //        GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            //    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            //}
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.Payroll + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Payroll);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region EMPLOYEE SETTINGS POPUP
                    case ActionsEnum.EMPSETTINGSPOPUP:
                        gdRow = (sender as ImageButton).Parent.Parent as GridViewRow;
                        HiddenField hdfEmpPK = gdRow.FindControl("hdfEmpPK") as HiddenField;
                        hdfSettingsEmpPK.Value = hdfEmpPK.Value;
                        Label lblEmployeeText = gdRow.FindControl("lblEmployeeText") as Label;
                        lblEmpName.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(lblEmployeeText.ToolTip), 40);
                        lblEmpName.ToolTip = lblEmployeeText.ToolTip;
                        HiddenField hdfEmpDOJ = gdRow.FindControl("hdfEmpDOJ") as HiddenField;
                        if (!string.IsNullOrEmpty(hdfEmpDOJ.Value))
                        {
                            lblEmpDOJ.Text = lblEmpDOJ.ToolTip = Convert.ToDateTime(hdfEmpDOJ.Value).ToString(Resources.Constants.HRMSDateDisplayFormat);
                        }
                        HiddenField hdfWorkDays = gdRow.FindControl("hdfWorkDays") as HiddenField;
                        if (!string.IsNullOrEmpty(hdfWorkDays.Value))
                        {
                            txtEmpWorkDays.Text = hdfWorkDays.Value;
                        }
                        totProcessDays = ((Convert.ToDateTime(txtProcessToDate.Text) - Convert.ToDateTime(txtProcessFromDate.Text)).TotalDays) + 1;
                        hdfTotalProcessDays.Value = totProcessDays.ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopupEmpSettings]','" + GetLocalResourceObject("EmpSettings").ToString() + "','550','130');", true);

                        break;
                    #endregion
                    #region EMPLOYEE SETTINGS APPLY
                    case ActionsEnum.EMPSETTINGSAPPLY:
                        foreach (GridViewRow gvrow in grdEmpPayrollList.Rows)
                        {
                            int empPK = Convert.ToInt32(((HiddenField)grdEmpPayrollList.Rows[gvrow.RowIndex].FindControl("hdfEmpPK")).Value);
                            if (empPK == Convert.ToInt32(hdfSettingsEmpPK.Value))
                            {
                                ((HiddenField)grdEmpPayrollList.Rows[gvrow.RowIndex].FindControl("hdfWorkDays")).Value = txtEmpWorkDays.Text;
                                if (Convert.ToDouble(txtEmpWorkDays.Text) > 0)
                                {
                                    grdEmpPayrollList.Rows[gvrow.RowIndex].BackColor = System.Drawing.ColorTranslator.FromHtml(GetLocalResourceObject("RowColor").ToString());
                                }
                                else
                                {
                                    grdEmpPayrollList.Rows[gvrow.RowIndex].BackColor = System.Drawing.ColorTranslator.FromHtml(GetLocalResourceObject("NoWorkDaysRowColor").ToString());
                                }
                                break;
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup", "ClosePopup();", true);
                        break;
                    #endregion
                    #region SALARY MONTH CHANGED
                    case ActionsEnum.SALMONTHCHANGED:
                        SetPayrollProcessDates();
                        SetPayrollCaption();
                        break;
                    #endregion
                    #region EXCEL EXPORT
                    case ActionsEnum.EXCELPRINT:
                        string Url = string.Empty;
                        Url = System.Configuration.ConfigurationManager.AppSettings["ReportPageUrl"];
                        if (string.IsNullOrEmpty(Url))
                        {
                          //  ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "PAYRL" + "&APPSUBTYPE= 7" + "&CurPK=" + CurrPK) + "&IsExcelPrint=1');", true);
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(System.Configuration.ConfigurationManager.AppSettings["ReportPageUrl"] + GetGlobalResourceObject("Report", "ReportPageUrl").ToString() + "?ID=0" + "&APPTYPE=" + "PAYRL" + "&APPSUBTYPE= 7" + "&CurPK=" + CurrPK) + "&IsExcelPrint=1');", true);
                        }
                        break;
                    #endregion
                    #region CURRENCYSELECTED
                    case ActionsEnum.CURRENCYSELECTED:
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        SetFieldValues(ControlsEnum.EXCHANGERATE);
                        break;
                    #endregion
                    #region PERIOD SEARCH POPUP
                    case ActionsEnum.PERIODSEARCHPOPUP:
                        txtMonthFrom.Text = string.Empty;
                        txtMonthTo.Text = string.Empty;
                        ddlStatus.ClearSelection();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPeriodSearch]','" + GetLocalResourceObject("Search").ToString() + "','500','220');", true);
                        break;
                    #endregion
                    #region PERIOD SEARCH
                    case ActionsEnum.PERIODSEARCH:
                        uclPaging.CurrentPage = 0;
                        PageIndex = "1";
                        GetFieldValues(ControlsEnum.PERIODLIST);
                        SetFieldValues(ControlsEnum.PERIODLIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
                        break;
                    #endregion
                    #region PERIOD CLEAR
                    case ActionsEnum.PERIODCLEAR:
                        uclPaging.CurrentPage = 0;
                        PageIndex = "1";
                        txtMonthFrom.Text = string.Empty;
                        txtMonthTo.Text = string.Empty;
                        ddlStatus.ClearSelection();
                        GetFieldValues(ControlsEnum.PERIODLIST);
                        SetFieldValues(ControlsEnum.PERIODLIST);
                        break;
                    #endregion
                    #region Non-Payroll Employees
                    case ActionsEnum.DETAILS:
                        GetFieldValues(ControlsEnum.NONPAYROLL);
                        SetFieldValues(ControlsEnum.NONPAYROLL);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divNonPayrollDetails]','" + GetLocalResourceObject("NonPayrolllEmpHd").ToString() + "','800','450');", true);
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
                        SetHRDepartment();
                        ResetPageAndWorkflow();
                        break;
                    #endregion
                    #region Journalize Save
                    case ActionsEnum.JOURNALIZESAVE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ((Button)sender).CommandName = ActionsEnum.SAVE.ToString();
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        SetHRDepartment();
                        ResetPageAndWorkflow();
                        break;
                    #endregion
                    #region Journalize Submit
                    case ActionsEnum.JOURNALIZESUBMIT:
                        ucrJournalize.ResetForm();
                        hdfJournalizeWorkFlow.Value = "0";
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        SetHRDepartment();
                        ResetPageAndWorkflow();
                        break;
                    #endregion
                    #region Journalize Delete
                    case ActionsEnum.JOURNALIZEDELETE:
                        hdfJournalizeWorkFlow.Value = "0";
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        SetHRDepartment();
                        ResetPageAndWorkflow();
                        break;
                    #endregion
                    #region Journalize Cancel
                    case ActionsEnum.JOURNALIZECANCEL:
                        hdfJournalizeWorkFlow.Value = "0";
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        SetHRDepartment();
                        ResetPageAndWorkflow();
                        break;
                        #endregion

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        private void ResetPageAndWorkflow()
        {
            EntryStatus = EntryStatus.LISTMODE;
            GetFieldValues(ControlsEnum.PERIODLIST);
            SetFieldValues(ControlsEnum.PERIODLIST);
            GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
            SetFieldValues(ControlsEnum.EMPPAYROLLLIST);
            GetUIValuesFromObject(ControlsEnum.PROCESSPAYROLLHDR);
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




        #endregion

        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                #region grdEmpPayrollList
                if ((sender as GridView).ID == "grdEmpPayrollList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        HiddenField hdfIsLJ = e.Row.FindControl("hdfIsLJ") as HiddenField;
                        Label lblEmployeeText = e.Row.FindControl("lblEmployeeText") as Label;
                        HiddenField hdfWorkDays = e.Row.FindControl("hdfWorkDays") as HiddenField;
                        ImageButton imbEmpSettings = e.Row.FindControl("imbEmpSettings") as ImageButton;
                        ImageButton imbEditPayroll = e.Row.FindControl("imbEditPayroll") as ImageButton;
                        ImageButton imbDeletePayroll = e.Row.FindControl("imbDeletePayroll") as ImageButton;
                        Label lblNetSalary = e.Row.FindControl("lblNetSalary") as Label;
                        imbEmpSettings.Attributes.Add("OnClick", "javascript:return showSettingsPopup(this,'" + hdfWorkDays.ClientID + "');");

                        imbEmpSettings.Visible = false;
                        if (hdfIsLJ != null && !string.IsNullOrEmpty(hdfIsLJ.Value))
                        {
                            if (Convert.ToInt32(hdfIsLJ.Value) == 1)
                            {
                                if (Convert.ToInt32(ddlProcessMode.SelectedValue) != (int)PayrollProcessMode.Periodic)
                                    imbEmpSettings.Visible = true;
                                lblEmployeeText.ForeColor = System.Drawing.Color.Red;
                                if (Convert.ToDouble(hdfWorkDays.Value) <= 0)
                                {
                                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetLocalResourceObject("NoWorkDaysRowColor").ToString());
                                }
                            }
                        }

                        if (lblNetSalary != null && !string.IsNullOrEmpty(lblNetSalary.Text))
                        {
                            if (Convert.ToDouble(lblNetSalary.Text) < 0)
                            {
                                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetLocalResourceObject("-VeSalaryRowColor").ToString());
                            }
                        }

                        if (hdfIsCancelled.Value == "1")
                        {
                            imbEditPayroll.Visible = false;
                            imbDeletePayroll.Visible = false;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.Header)
                    {
                        ImageButton imbDeleteAll = e.Row.FindControl("imbDeleteAll") as ImageButton;
                        if (hdfIsCancelled.Value == "1")
                            imbDeleteAll.Visible = false;
                    }
                }
                #endregion
                #region grdPeriodList
                if ((sender as GridView).ID == "grdPeriodList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        HiddenField hdfEphPK = e.Row.FindControl("hdfEphPK") as HiddenField;
                        if (CurrPK == Convert.ToInt32(hdfEphPK.Value))
                        {
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetLocalResourceObject("ProcessedRowColor").ToString());
                        }
                        else
                        {
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetLocalResourceObject("RowColor").ToString());
                        }
                    }
                }
                #endregion
                #region grdEarnPayDetails
                else if ((sender as GridView).ID == "grdEarnPayDetails")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        HiddenField hdfPayElmtInSalary = e.Row.FindControl("hdfPayElmtInSalary") as HiddenField;
                        if (Convert.ToInt32(hdfPayElmtInSalary.Value) == 1)
                        {
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "RowColor").ToString());
                        }
                        else
                        {
                            if (ShowSalPartPayElement)
                                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "PayElmtNotInSalary").ToString());
                            else
                                e.Row.Visible = false;
                        }
                    }
                }
                #endregion
                #region grdDeductPayDetails
                else if ((sender as GridView).ID == "grdDeductPayDetails")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        HiddenField hdfPayElmtInSalary = e.Row.FindControl("hdfPayElmtInSalary") as HiddenField;
                        if (Convert.ToInt32(hdfPayElmtInSalary.Value) == 1)
                        {
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "RowColor").ToString()); //  GetLocalResourceObject("RowColor")
                        }
                        else
                        {
                            if (ShowSalPartPayElement)
                                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "PayElmtNotInSalary").ToString());  //GetLocalResourceObject("PayElmtNotInSalary")
                            else
                                e.Row.Visible = false;
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (sortDirection == SortDirection.Ascending)
                    sortDirection = SortDirection.Descending;
                else
                    sortDirection = SortDirection.Ascending;
                sortExpression = e.SortExpression.ToString();
                GetFieldValues(ControlsEnum.EMPPAYROLLLIST);
                SetFieldValues(ControlsEnum.PAYROLLSORT);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        #endregion

        #region --- Functions----
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region PROCESS PAYROLL HDR
                    case ControlsEnum.PROCESSPAYROLLHDR:
                        objEmpPayrollHeader.EPH_PK = CurrPK;
                        objEmpPayrollHeader.EPH_NO = lblTrxNo.Text;
                        string fromDate = string.Empty;
                        string toDate = string.Empty;
                        fromDate = string.IsNullOrEmpty(txtProcessFromDate.Text) ? string.Empty : txtProcessFromDate.Text;
                        toDate = string.IsNullOrEmpty(txtProcessToDate.Text) ? string.Empty : txtProcessToDate.Text;

                        objEmpPayrollHeader.EPH_FROM_DATE = fromDate;
                        objEmpPayrollHeader.EPH_TO_DATE = toDate;
                        objEmpPayrollHeader.EPH_PRC_MODE = Convert.ToByte(ddlProcessMode.SelectedValue);
                        objEmpPayrollHeader.EPH_PAYROLL_TYPE = Convert.ToByte(ddlPayrollType.SelectedValue);
                        objEmpPayrollHeader.EPH_PRC_NAME = txtCaption.Text;
                        objEmpPayrollHeader.EPH_DATE = string.IsNullOrEmpty(txtProcessDate.Text) ? string.Empty : txtProcessDate.Text;
                        objEmpPayrollHeader.EPH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        objEmpPayrollHeader.BIZUNIT_PK = Convert.ToInt16(currentUser.SBUID);
                        objEmpPayrollHeader.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        objEmpPayrollHeader.USER_PK = Convert.ToInt16(currentUser.PKUser);
                        objEmpPayrollHeader.LAST_MOD_DT = LastModifiedTime;
                        objEmpPayrollHeader.EPH_PAYROLL_MONTH = DateTime.Parse(txtSalaryMonth.Text);
                        objEmpPayrollHeader.EPH_CURRENCY = Convert.ToInt32(hdfCurrency.Value);
                        objEmpPayrollHeader.EPH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                        objEmpPayrollHeader.EPH_EXCHG_RATE = string.IsNullOrEmpty(txtExchangeRate.Text.Trim()) ? 1 : Convert.ToDouble(txtExchangeRate.Text.Trim());
                        objEmpPayrollHeader.EPH_COMPANY = Convert.ToInt32(ddlCompanyHdr.SelectedValue);
                        objEmpPayrollHeader.EPH_IS_FINAL_SETTLEMENT = Convert.ToInt32(hdfIsFinalize.Value);
                        objEmpPayrollHeader.EPH_RESIGNATION_DATE = txtResignDate.Text;
                        SetUIValuesToObject(ControlsEnum.PROCESSPAYROLLDTL);
                        objEmpPayrollHeader.EmpPayrollDtl = empPayrollDetailList;
                        objEmpPayrollHeader.EmpWorkingDaysDtl = workdayDetailList;
                        retObject = objEmpPayrollHeader;
                        break;
                    #endregion
                    #region PROCESS PAYROLL DTL
                    case ControlsEnum.PROCESSPAYROLLDTL:
                        empPayrollDetailList = new List<EmpPayrollDetails>();
                        foreach (GridViewRow grdrow in grdEmpPayrollList.Rows)
                        {
                            CheckBox chkEmpselect;
                            chkEmpselect = (CheckBox)grdrow.FindControl("chkEmpselect");
                            if (chkEmpselect.Checked)
                            {
                                objEmpPayrollDetails = new EmpPayrollDetails();
                                objEmpPayrollDetails.EPS_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfItemPK")).Value);
                                objEmpPayrollDetails.EPS_EMPLOYEE = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEmpPK")).Value);
                                objEmpPayrollDetails.EPS_WORK_DAYS = Convert.ToDouble(((HiddenField)grdrow.FindControl("hdfWorkDays")).Value);
                                objEmpPayrollDetails.EPS_IS_LJ = Convert.ToByte(((HiddenField)grdrow.FindControl("hdfIsLJ")).Value);
                                empPayrollDetailList.Add(objEmpPayrollDetails);
                            }
                        }
                        break;
                    #endregion
                    #region EMPPAYROLL HDR
                    case ControlsEnum.EMPPAYROLLHDR:
                        objEmpPayHeader.EPS_PK = CurrEmployeePayrollPK;
                        objEmpPayHeader.EPS_PAYROLL_HDR = CurrPK;
                        objEmpPayHeader.USER_PK = Convert.ToInt32(currentUser.PKUser);
                        objEmpPayHeader.LAST_MOD_DT = LastModifiedTime;
                        SetUIValuesToObject(ControlsEnum.EMPPAYROLLDTL);
                        objEmpPayHeader.EmpPayrollPayDtl = empPayDtlList;
                        retObject = objEmpPayHeader;
                        break;
                    #endregion
                    #region EMPPAYROLL DTL
                    case ControlsEnum.EMPPAYROLLDTL:
                        empPayDtlList = new List<EmpPayrollPayDetails>();
                        foreach (GridViewRow grdrow in grdEarnPayDetails.Rows)
                        {
                            objEmpPayDetails = new EmpPayrollPayDetails();
                            objEmpPayDetails.EPP_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEarnPayPK")).Value);
                            objEmpPayDetails.EPP_PAY_ELEMENT = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEarnPayElementPK")).Value);
                            objEmpPayDetails.EPP_ACT_PAY_AMT = Convert.ToDouble(((Label)grdrow.FindControl("lblActualEarnings")).Text);
                            objEmpPayDetails.EPP_PAY_AMT = Convert.ToDouble(((TextBox)grdrow.FindControl("txtEarnEligibleAmount")).Text);
                            objEmpPayDetails.EPP_IS_DEDUCTION = Convert.ToByte(((HiddenField)grdrow.FindControl("hdfIsEarn")).Value);
                            empPayDtlList.Add(objEmpPayDetails);
                        }
                        foreach (GridViewRow grdrow in grdDeductPayDetails.Rows)
                        {
                            objEmpPayDetails = new EmpPayrollPayDetails();
                            objEmpPayDetails.EPP_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDeductPayPK")).Value);
                            objEmpPayDetails.EPP_PAY_ELEMENT = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDeductPayElementPK")).Value);
                            objEmpPayDetails.EPP_ACT_PAY_AMT = Convert.ToDouble(((Label)grdrow.FindControl("lblActualDeduct")).Text);
                            objEmpPayDetails.EPP_PAY_AMT = Convert.ToDouble(((TextBox)grdrow.FindControl("txtDeductEligibleAmount")).Text);
                            objEmpPayDetails.EPP_IS_DEDUCTION = Convert.ToByte(((HiddenField)grdrow.FindControl("hdfIsDeduct")).Value);
                            empPayDtlList.Add(objEmpPayDetails);
                        }
                        break;
                    #endregion
                    #region PRINT MULTIPLE PAY SLIP
                    case ControlsEnum.PRINTMULTIPLE:
                        EmpPaySlipHeader ObjEmpPaySlipHeader = new EmpPaySlipHeader();
                        List<EmpPaySlipDetails> lstEmpPaySlipDetails = new List<EmpPaySlipDetails>();
                        foreach (GridViewRow row in grdEmpPayrollList.Rows)
                        {
                            CheckBox chkBx = (CheckBox)row.FindControl("chkEmpselect");
                            if (chkBx.Checked == true)
                            {
                                EmpPaySlipDetails ObjEmpPaySlipDetails = new EmpPaySlipDetails();
                                HiddenField hdfItemPK1 = (HiddenField)row.FindControl("hdfItemPK");
                                ObjEmpPaySlipDetails.EPS_PK = hdfItemPK1.Value;
                                lstEmpPaySlipDetails.Add(ObjEmpPaySlipDetails);
                            }
                        }
                        ObjEmpPaySlipHeader.EmpPaySlipDetails = lstEmpPaySlipDetails;
                        retObject = ObjEmpPaySlipHeader;
                        break;
                    #endregion
                    #region PRINT SINGLE PAY SLIP
                    case ControlsEnum.PRINTPAYSLIPSINGLE:
                        EmpPaySlipHeader ObjEmpPaySlipHeaderSingle = new EmpPaySlipHeader();
                        List<EmpPaySlipDetails> lstEmpPaySlipDetailsSingle = new List<EmpPaySlipDetails>();
                        EmpPaySlipDetails ObjEmpPaySlipDetailsSingle = new EmpPaySlipDetails();
                        ObjEmpPaySlipDetailsSingle.EPS_PK = EmployeePayrollPK.ToString();
                        lstEmpPaySlipDetailsSingle.Add(ObjEmpPaySlipDetailsSingle);
                        ObjEmpPaySlipHeaderSingle.EmpPaySlipDetails = lstEmpPaySlipDetailsSingle;
                        retObject = ObjEmpPaySlipHeaderSingle;
                        break;
                    #endregion
                    #region DELETE ALL
                    case ControlsEnum.DELETEALL:
                        EmpPaySlipHeader objPayDethdr = new EmpPaySlipHeader();
                        List<EmpPaySlipDetails> objPayDetList = new List<EmpPaySlipDetails>();
                        objPayDethdr.EPH_PK = CurrPK;
                        objPayDethdr.LAST_MOD_DT = LastModifiedTime;
                        foreach (GridViewRow grdrow in grdEmpPayrollList.Rows)
                        {
                            int detPk = 0;
                            EmpPaySlipDetails objPayDetails = new EmpPaySlipDetails();
                            CheckBox chkEmpselect = ((CheckBox)grdrow.FindControl("chkEmpselect"));
                            HiddenField hdfItemPK = ((HiddenField)grdrow.FindControl("hdfItemPK"));
                            int.TryParse(hdfItemPK.Value, out detPk);
                            if (chkEmpselect.Checked && chkEmpselect.Enabled && detPk > 0)
                            {
                                objPayDetails.EPS_PK = detPk.ToString();
                                objPayDetList.Add(objPayDetails);
                            }
                        }
                        objPayDethdr.EmpPaySlipDetails = objPayDetList;
                        retObject = objPayDethdr;
                        break;
                    #endregion

                    #region JOURNALIZE
                    case ControlsEnum.JOURNALIZE:

                        if (CurrPK > 0)
                        {
                            if (Status == (int)DbStatus.APPROVED)
                            {
                                GetFieldValues(ControlsEnum.EMPPAYROLLLIST);

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


                                ucrJournalize.TransactionType = ApplicationType.PAYRL;
                                Session[ERP.Utilities.SessionStrings.TransactionType] = ApplicationType.PAYRLJ;
                                ucrJournalize.TransactionPK = CurrPK;
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                                ucrJournalize.JournalizePK = 0;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = objEmpPayrollHeader.EPH_NO;
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = objEmpPayrollHeader.EPH_DATE;
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = objEmpPayrollHeader.EPH_CURRENCY;
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.AP;
                                //Session[ERP.Utilities.SessionStrings.AccountPayablePK] = objEmpPayrollHeader.ICH_CUSTOMER;
                                Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.PAYRLJ;
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
                                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.LISTMODE && ucrWrkf.HasPageTaskPermission)
                                {
                                    ucrJournalize.JournalizeRefPK = ucrWrkf.RefID;
                                    ucrWrkf.ViewType = 1;
                                }
                                else
                                {
                                    ucrWrkf.ViewType = 0;
                                }
                                ucrJournalize.HasWkfPermission = ucrWrkf.HasPageTaskPermission;
                                Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus == EntryStatus.LISTMODE ? EntryStatus.ENTRYMODE : EntryStatus.VIEWMODE;
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
                                Session[ERP.Utilities.SessionStrings.JournalHead] = GetLocalResourceObject("Payroll_Journal").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_PayrollNotapproved").ToString();
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

        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region PROCESS PAYROLL HDR
                    case ControlsEnum.PROCESSPAYROLLHDR:
                        if (objEmpPayrollHeader != null)
                        {
                            if (objEmpPayrollHeader.EPH_PRC_MODE > 0)
                            {
                                ddlProcessMode.SelectedValue = objEmpPayrollHeader.EPH_PRC_MODE.ToString();
                            }
                            PayrollTypePk = objEmpPayrollHeader.EPH_PAYROLL_TYPE;
                            GetFieldValues(ControlsEnum.PAYROLLTYPE);
                            SetFieldValues(ControlsEnum.PAYROLLTYPE);
                            if (objEmpPayrollHeader.EPH_PAYROLL_TYPE > 0)
                            {
                                ddlPayrollType.SelectedValue = objEmpPayrollHeader.EPH_PAYROLL_TYPE.ToString();
                            }

                            lblTrxNo.Text = string.IsNullOrEmpty(objEmpPayrollHeader.EPH_NO) ? Resources.ErpRes.Draft : objEmpPayrollHeader.EPH_NO;
                            txtCaption.Text = HttpUtility.HtmlDecode(objEmpPayrollHeader.EPH_PRC_NAME);
                            lblCaptionHdr.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmpPayrollHeader.EPH_PRC_NAME), 75);
                            lblCaptionHdr.ToolTip = HttpUtility.HtmlDecode(objEmpPayrollHeader.EPH_PRC_NAME);
                            txtProcessDate.Text = Convert.ToDateTime(objEmpPayrollHeader.EPH_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            lblProcessDateHdr.Text = Convert.ToDateTime(objEmpPayrollHeader.EPH_DATE).ToString(Resources.Constants.HRMSDateDisplayFormat);

                            txtProcessFromDate.Text = Convert.ToDateTime(objEmpPayrollHeader.EPH_FROM_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            txtProcessToDate.Text = Convert.ToDateTime(objEmpPayrollHeader.EPH_TO_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            hdfLastModDate.Value = objEmpPayrollHeader.LAST_MOD_DT.ToString();
                            LastModifiedTime = Convert.ToDateTime(hdfLastModDate.Value);
                            lblPeriodHdr.Text = Convert.ToDateTime(objEmpPayrollHeader.EPH_FROM_DATE).ToString(Resources.Constants.HRMSDateDisplayFormat) + " to " + Convert.ToDateTime(objEmpPayrollHeader.EPH_TO_DATE).ToString(Resources.Constants.HRMSDateDisplayFormat);
                            txtSalaryMonth.Text = objEmpPayrollHeader.EPH_PAYROLL_MONTH.ToString(Resources.Constants.DateFormatMonthYear);
                            txtCurrency.Text = HttpUtility.HtmlDecode(objEmpPayrollHeader.EPH_CURRENCY_TEXT);
                            hdfCurrency.Value = objEmpPayrollHeader.EPH_CURRENCY.ToString();
                            txtExchangeRate.Text = GetFormattedExchangerate(objEmpPayrollHeader.EPH_EXCHG_RATE);
                            txtResignDate.Text = Convert.ToDateTime(objEmpPayrollHeader.EPH_RESIGNATION_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                        
                            //  Exchange rate field is not editable.ie,If selected currency is same as SBU base currency
                            if (currentUser.BaseCurrency == Convert.ToInt32(objEmpPayrollHeader.EPH_CURRENCY))
                                txtExchangeRate.Enabled = false;
                            else
                                txtExchangeRate.Enabled = true;

                            GetFieldValues(ControlsEnum.WORKINGDAYSDETAILS);
                            SetFieldValues(ControlsEnum.WORKINGDAYSDETAILS);
                            NegativeSalExist = objEmpPayrollHeader.EPH_FAILED_COUNT;
                            Status = objEmpPayrollHeader.EPH_STATUS;
                            JournalStatus = objEmpPayrollHeader.EPH_HAS_JRNL_ENTRY;
                            CompanyPk = objEmpPayrollHeader.EPH_COMPANY;
                            GetFieldValues(ControlsEnum.COMPANY);
                            SetFieldValues(ControlsEnum.COMPANY);
                            ddlCompanyHdr.SelectedIndex = ddlCompanyHdr.Items.IndexOf(ddlCompanyHdr.Items.FindByValue(CompanyPk.ToString()));
                            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(CompanyPk.ToString()));
                            if (Convert.ToInt32(ddlPayrollType.SelectedValue) < 0)
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(string.Format(GetLocalResourceObject("Err_PayrollTypeNotMapped").ToString(), HttpUtility.HtmlDecode(objEmpPayrollHeader.EPH_PAYROLL_TYPE_TEXT)))
                                               + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region EMP PAYROLL DETAIL LIST
                    case ControlsEnum.EMPPAYROLLDETAILLIST:
                        if (objEmpPayHeader != null)
                        {
                            //lblPayrollProcess.Text = Convert.ToDateTime(objEmpPayHeader.EPH_FROM_DATE).ToString(Resources.Constants.HRMSDateDisplayFormat) + " to " + Convert.ToDateTime(objEmpPayHeader.EPH_TO_DATE).ToString(Resources.Constants.HRMSDateDisplayFormat)
                            //                            + " " + ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmpPayHeader.EPH_PRC_NAME), 20);
                            //lblPayrollProcess.ToolTip = Convert.ToDateTime(objEmpPayHeader.EPH_FROM_DATE).ToString(Resources.Constants.HRMSDateDisplayFormat) + " to " + Convert.ToDateTime(objEmpPayHeader.EPH_TO_DATE).ToString(Resources.Constants.HRMSDateDisplayFormat)
                            //                            + " " + HttpUtility.HtmlDecode(objEmpPayHeader.EPH_PRC_NAME);
                            lblPayrollProcess.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmpPayHeader.EPH_PRC_NAME), 40);
                            lblPayrollProcess.ToolTip = HttpUtility.HtmlDecode(objEmpPayHeader.EPH_PRC_NAME);

                            lblPayrollProcessDate.Text = lblPayrollProcessDate.ToolTip = Convert.ToDateTime(objEmpPayHeader.EPH_DATE).ToString(Resources.Constants.HRMSDateDisplayFormat);
                            lblPayrollEmployee.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmpPayHeader.empText), 60);
                            lblPayrollEmployee.ToolTip = HttpUtility.HtmlDecode(objEmpPayHeader.empText);
                            lblPaymentMode.Text = lblPaymentMode.ToolTip = objEmpPayHeader.EPD_PAY_MODE_TEXT;
                            lblCurrency.Text = lblCurrency.ToolTip = objEmpPayHeader.EPD_CURR_TEXT;
                            lblBankName.Text = lblBankName.ToolTip = objEmpPayHeader.EPD_PAY_BANK_TEXT;
                            lblBranch.Text = ERP.Utilities.CommonFunctions.GetShortString(objEmpPayHeader.EPD_PAY_BANK_BRANCH, 25);
                            lblBranch.ToolTip = objEmpPayHeader.EPD_PAY_BANK_BRANCH;
                            lblAccountName.Text = ERP.Utilities.CommonFunctions.GetShortString(objEmpPayHeader.EPD_BANK_AC_NAME, 28);
                            lblAccountName.ToolTip = objEmpPayHeader.EPD_BANK_AC_NAME;
                            lblAccountNO.Text = ERP.Utilities.CommonFunctions.GetShortString(objEmpPayHeader.EPD_BANK_AC_NO, 15);
                            lblAccountNO.ToolTip = objEmpPayHeader.EPD_BANK_AC_NO;
                            lblPFNo.Text = ERP.Utilities.CommonFunctions.GetShortString(objEmpPayHeader.EPD_PF_AC, 20);
                            lblPFNo.ToolTip = objEmpPayHeader.EPD_PF_AC;
                            lblSOSCONo.Text = ERP.Utilities.CommonFunctions.GetShortString(objEmpPayHeader.EPD_SOCSO_AC, 20);
                            lblSOSCONo.ToolTip = objEmpPayHeader.EPD_SOCSO_AC;
                            lblWorkingDays.Text = lblWorkingDays.ToolTip = objEmpPayHeader.EPD_WORK_DAYS;
                            lblLOP.Text = lblLOP.ToolTip = objEmpPayHeader.EPD_LOP;
                            lblEmpGrossSalary.Text = lblEmpGrossSalary.ToolTip = objEmpPayHeader.EPS_GROSS_AMT.ToString(hdfCurrencyFormatWithComma.Value);
                            lblEmpNetSalary.Text = lblEmpNetSalary.ToolTip = objEmpPayHeader.EPS_NET_AMT.ToString(hdfCurrencyFormatWithComma.Value);
                            lblEmpCTC.Text = lblEmpCTC.ToolTip = objEmpPayHeader.EPS_CTC_AMT.ToString(hdfCurrencyFormatWithComma.Value);
                            lblEmpTotEarnings.Text = lblEmpTotEarnings.ToolTip = objEmpPayHeader.EPS_ALW_AMT.ToString(hdfCurrencyFormatWithComma.Value);
                            lblEmpTotDeduction.Text = lblEmpTotDeduction.ToolTip = objEmpPayHeader.EPS_DED_AMT.ToString(hdfCurrencyFormatWithComma.Value);
                            hdfPayrollEmployee.Value = objEmpPayHeader.empPK;
                            empPayDtlList = objEmpPayHeader.EmpPayrollPayDtl;
                        }
                        break;
                    #endregion
                    #region PAYROLL TYPE DETAILS
                    case ControlsEnum.PAYROLLTYPEDETAILS:
                        SetPayrollProcessDates();
                        break;
                    #endregion
                    #region EXCHANGE RATE
                    case ControlsEnum.EXCHANGERATE:
                        if (dsExchangeRate != null && dsExchangeRate.Tables[0].Rows.Count > 0)
                        {
                            if (Convert.ToDouble(dsExchangeRate.Tables[0].Rows[0][0]) > 0)
                                txtExchangeRate.Text = GetFormattedExchangerate(dsExchangeRate.Tables[0].Rows[0][0].ToString());
                            else
                                txtExchangeRate.Text = string.Empty;
                        }
                        else
                        {
                            txtExchangeRate.Text = string.Empty;
                        }
                        //  Exchange rate field is not editable.ie,If selected currency is same as SBU base currency
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

        /// <summary>
        /// Set Payroll Caption
        /// </summary>
        private void SetPayrollCaption()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetPayrollCaption", "SetPayrollCaption();", true);
        }
        /// <summary>
        /// Method to set process from date and to date
        /// </summary>
        private void SetPayrollProcessDates()
        {
            PayrollTypePk = Convert.ToInt32(ddlPayrollType.SelectedValue);
            if (dtResult == null || dtResult.Rows.Count == 0)
                GetFieldValues(ControlsEnum.PAYROLLTYPEDETAILS);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(txtSalaryMonth.Text.Trim()))
                {
                    DateTime salMonth = Convert.ToDateTime(txtSalaryMonth.Text.Trim());
                    DateTime processDate = salMonth;
                    DateTime tempDate;
                    int offset = Convert.ToInt32(dtResult.Rows[0]["PTM_OFFSET"]);
                    int payrlstart = Convert.ToInt32(dtResult.Rows[0]["PTM_PAYRL_START"]);
                    int month = salMonth.Month;
                    if (month > 0)
                        tempDate = new DateTime(salMonth.Year, salMonth.Month, 1);
                    else
                        tempDate = new DateTime(salMonth.Year - 1, 12, 1);
                    if (DateTime.DaysInMonth(tempDate.Year, tempDate.Month) >= payrlstart)
                        processDate = new DateTime(tempDate.Year, tempDate.Month, payrlstart);
                    else
                        processDate = new DateTime(tempDate.Year, tempDate.Month, DateTime.DaysInMonth(tempDate.Year, tempDate.Month));
                    txtProcessFromDate.Text = processDate.ToString(Resources.Constants.HRMSDateFormatShort);
                    if (Convert.ToInt32(ddlProcessMode.SelectedValue) == (int)PayrollProcessMode.Monthly)
                    {
                        string  processToDate = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                        txtProcessToDate.Text = processToDate.ToString();
                    }
                    else
                        txtProcessToDate.Text = string.Empty;
                }
                else
                {
                    txtProcessFromDate.Text = string.Empty;
                    txtProcessToDate.Text = string.Empty;
                }
            }
        }

        private void GetFieldValues(ControlsEnum type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            //GridPrams gridParam;
            FilterParameters filterParams;
            ServiceUtility serviceUtilityObj;
            FinTrxService finTrxServiceClient;
            try
            {
                switch (type)
                {
                    #region EMPLOYMENTTYPE
                    case ControlsEnum.EMPLOYMENTTYPE:
                        int commonPK = 0;
                        dtResult = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmploymentType(commonPK);
                        break;
                    #endregion
                    #region BRANCH / LOCATION
                    case ControlsEnum.BRANCH:
                        dtResult = BusinessLogic.HRMS.Payroll.LoansAndAdvancesBL.GetBranchLocation();
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
                        dtResult = FullandFinalSettlementBL.GetPayrollType(PayrollTypePk, currentUser.SBUID, Convert.ToInt32(DbActiveStatus.ACTIVE), processmode, currentUser.PKUser);
                        break;
                    #endregion
                    #region PAYROLL TYPE DETAILS
                    case ControlsEnum.PAYROLLTYPEDETAILS:
                        dtResult = FullandFinalSettlementBL.GetPayrollType(PayrollTypePk, currentUser.SBUID, Convert.ToInt32(DbActiveStatus.HASPK), Convert.ToInt32(ddlProcessMode.SelectedValue));
                        break;
                    #endregion
                    #region EMPLOYEE PAYROLL LIST
                    case ControlsEnum.EMPPAYROLLLIST:
                        int empPK = 0;
                        int typePK = 0;
                        int branchPK = 0;
                        int empPayrollTypePK = 0;
                        int cmpPK = 0;
                        int employeeTypePK = 0;
                        int empDeptPK = 0;
                        int currencyPk = 0;
                        if (!string.IsNullOrEmpty(txtEmployee.Text.Trim()) && !txtEmployee.Text.Equals(GetGlobalResourceObject("ErpRes", "AutoDefaultValue").ToString()))
                            int.TryParse(hdfEmployee.Value, out empPK);
                        if (!string.IsNullOrEmpty((ddlEmploymentType.SelectedValue)))
                        {
                            typePK = Convert.ToInt32(ddlEmploymentType.SelectedValue) > 0 ? Convert.ToInt32(ddlEmploymentType.SelectedValue) : 0;
                        }
                        if (!string.IsNullOrEmpty((ddlBranchLocation.SelectedValue)))
                        {
                            branchPK = Convert.ToInt32(ddlBranchLocation.SelectedValue) > 0 ? Convert.ToInt32(ddlBranchLocation.SelectedValue) : 0;
                        }
                        if (!string.IsNullOrEmpty((ddlPayrollType.SelectedValue)))
                        {
                            empPayrollTypePK = Convert.ToInt32(ddlPayrollType.SelectedValue) > 0 ? Convert.ToInt32(ddlPayrollType.SelectedValue) : 0;
                        }
                        if (!string.IsNullOrEmpty((ddlCompany.SelectedValue)))
                        {
                            cmpPK = Convert.ToInt32(ddlCompany.SelectedValue) > 0 ? Convert.ToInt32(ddlCompany.SelectedValue) : 0;
                        }
                        if (!string.IsNullOrEmpty((ddlEmployeeType.SelectedValue)))
                        {
                            employeeTypePK = Convert.ToInt32(ddlEmployeeType.SelectedValue) > 0 ? Convert.ToInt32(ddlEmployeeType.SelectedValue) : 0;
                        }
                        int.TryParse(hdfDepartment.Value, out empDeptPK);
                        int.TryParse(hdfCurrency.Value, out currencyPk);
                        objFilterEmpDetails = new FilterEmpDetails();
                        objFilterEmpDetails.EPH_PK = CurrPK > 0 ? CurrPK.ToString() : null;
                        objFilterEmpDetails.EPH_FROM_DATE = txtProcessFromDate.Text;
                        objFilterEmpDetails.EPH_TO_DATE = txtProcessToDate.Text;
                        objFilterEmpDetails.EMP_PK = empPK > 0 ? empPK.ToString() : null;
                        objFilterEmpDetails.EMP_TYPE = employeeTypePK > 0 ? employeeTypePK.ToString() : null;
                        objFilterEmpDetails.EMP_BRANCH = branchPK > 0 ? branchPK.ToString() : null;
                        objFilterEmpDetails.EMP_PAYROLL_TYPE = CurrPK > 0 ? null : empPayrollTypePK.ToString();
                        objFilterEmpDetails.empCompany = CurrPK > 0 ? null : cmpPK.ToString();
                        objFilterEmpDetails.empEmploymentType = typePK > 0 ? typePK.ToString() : null;
                        objFilterEmpDetails.empDepartment = empDeptPK > 0 ? empDeptPK.ToString() : null;
                        objFilterEmpDetails.EPH_CURRENCY = currencyPk > 0 ? currencyPk.ToString() : null;
                        objFilterEmpDetails.EPH_IS_FINAL_SETTLEMENT = Convert.ToInt32(hdfIsFinalize.Value);
                       // objFilterEmpDetails.EPH_TASK1_BY =Convert.ToString(currentUser.SBUID);
                       // objFilterEmpDetails.EPH_RESIGNATION_DATE = txtResignDate.Text;

                        if (objFilterEmpDetails != null)
                        {
                            string xmlDoc = CommonFunctions.XmlSerialize<FilterEmpDetails>(objFilterEmpDetails);
                            objEmpPayrollHeader = FullandFinalSettlementBL.GetEmployeePayrollList(xmlDoc);

                            //objEmpPayrollHeader = PayrollProcessBL.GetEmployeePayrollList(empPK, typePK, branchPK, CurrPK, empPayrollTypePK, cmpPK, employeeTypePK);
                        }
                        break;
                    #endregion
                    #region PERIOD LIST
                    case ControlsEnum.PERIODLIST:
                        int TotalRecords = 0;
                        //gridParam = new BusinessObject.GridPrams();
                        filterParams = new FilterParameters();
                        filterParams.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        filterParams.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        filterParams.FromDate = string.IsNullOrEmpty(txtMonthFrom.Text) ? (DateTime?)null : Convert.ToDateTime(txtMonthFrom.Text);
                        filterParams.ToDate = string.IsNullOrEmpty(txtMonthTo.Text) ? (DateTime?)null : Convert.ToDateTime(txtMonthTo.Text);
                        filterParams.Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        int processMode = 0;
                        if (!string.IsNullOrEmpty(ddlPayrollType.SelectedValue))
                        {
                            processMode = Convert.ToInt32(ddlPayrollType.SelectedValue) > 0 ? Convert.ToInt32(ddlPayrollType.SelectedValue) : 0;
                        }
                        
                        dtPageData = FullandFinalSettlementBL.GetPayrollProcessPeriodList(filterParams, currentUser.SBUID, processMode);
                        if (dtPageData != null)
                        {
                            //set Total Page Count
                            TotalRecords = dtPageData.Rows.Count > 0 ? Convert.ToInt32(dtPageData.Rows[0]["TOTAL_ROW_COUNT"].ToString()) : 0;
                            TotalPages = TotalRecords == 0 ? 1 : (TotalRecords <= filterParams.PageSize) ? 1 :
                                        (TotalRecords % filterParams.PageSize) == 0 ? (TotalRecords / filterParams.PageSize) :
                                        (TotalRecords / filterParams.PageSize) + 1;
                        }
                        break;
                    #endregion
                    #region EMPLOYEE PAYROLL LIST
                    case ControlsEnum.EMPPAYROLLDETAILLIST:
                        objEmpPayHeader = FullandFinalSettlementBL.GetEmployeePayrollDetailList(CurrEmployeePayrollPK);
                        break;
                    #endregion
                    #region WORKING DAYS DETAILS
                    case ControlsEnum.WORKINGDAYSDETAILS:
                        objWorkdayHdr = FullandFinalSettlementBL.GetWorkingDaysDetails(CurrPK, currentUser.SBUID, Convert.ToInt32(DbActiveStatus.ACTIVE), 0);
                        workdayDetailList = new List<EmpWorkingDayDetails>();
                        objWorkdayHdr.EmpWorkingDaysDtl.ToList().ForEach(wrk => wrk.EPW_ACTIVE = 1);
                        workdayDetailList = objWorkdayHdr.EmpWorkingDaysDtl;
                        break;
                    #endregion
                    #region EMPLOYEE TYPE
                    case ControlsEnum.EMPLOYEETYPE:
                        dtResult = BusinessLogic.HRMS.Employee.EmployeePayDetailsBL.GetEmployeeTypeGetKV(0, currentUser.SBUID, (int)DbActiveStatus.ACTIVE);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(BusinessObject.CommonManagement.DbActiveStatus.ACTIVE), 0, CompanyPk);
                        break;
                    #endregion
                    #region EXCHANGE RATE
                    case ControlsEnum.EXCHANGERATE:
                        if (!string.IsNullOrEmpty(hdfCurrency.Value) && Convert.ToInt32(hdfCurrency.Value) > 0)
                        {
                            DateTime processDate = DateTime.Now;
                            DateTime.TryParse(txtProcessDate.Text.Trim(), out processDate);
                            dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(Convert.ToInt32(hdfCurrency.Value), currentUser.BaseCurrency, processDate);
                        }
                        break;
                    #endregion
                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        dtResult = CommonBL.GetCurrency(currentUser.BaseCurrency, currentUser.SBUID, (int)DbActiveStatus.HASPK);
                        break;
                    #endregion
                    #region GET PAYROLL BY JOURNAL PK
                    case ControlsEnum.GETPAYROLLBYJOURNALPK:
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
                    #region DEPARTMENT BY TYPE
                    case ControlsEnum.DEPARTMENTBYTYPE:
                        dtDept = BusinessLogic.CommonManagement.CommonBL.GetDepartment(currentUser.SBUID, null, dptType, (int)DbActiveStatus.ACTIVE);
                        break;
                    #endregion
                    #region Non Payroll Employees
                    case ControlsEnum.NONPAYROLL:
                        dtNonPayroll = FullandFinalSettlementBL.GetNonPayrollEmployees(Convert.ToInt32(ddlPayrollType.SelectedValue), txtProcessFromDate.Text, txtProcessToDate.Text);
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
            }
        }


        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region EMPLOYMENT TYPE
                    case ControlsEnum.EMPLOYMENTTYPE:
                        BindDropDown(ControlsEnum.EMPLOYMENTTYPE);
                        break;
                    #endregion
                    #region BRANCH
                    case ControlsEnum.BRANCH:
                        BindDropDown(ControlsEnum.BRANCH);
                        break;
                    #endregion
                    #region EMP PAYROLL LIST
                    case ControlsEnum.EMPPAYROLLLIST:
                        BindGrid(ControlsEnum.EMPPAYROLLLIST);
                        break;
                    #endregion
                    #region PAYROLL TYPE
                    case ControlsEnum.PAYROLLTYPE:
                        BindDropDown(ControlsEnum.PAYROLLTYPE);
                        break;
                    #endregion
                    #region PROCESS MODE
                    case ControlsEnum.PROCESSMODE:
                        BindDropDown(ControlsEnum.PROCESSMODE);
                        break;
                    #endregion
                    #region PERIOD LIST
                    case ControlsEnum.PERIODLIST:
                        BindGrid(ControlsEnum.PERIODLIST);
                        break;
                    #endregion
                    #region PAY EARNINGS LIST
                    case ControlsEnum.PAYEARNINGSLIST:
                        BindGrid(ControlsEnum.PAYEARNINGSLIST);
                        break;
                    #endregion
                    #region PAY DEDUCTION LIST
                    case ControlsEnum.PAYDEDUCTIONLIST:
                        BindGrid(ControlsEnum.PAYDEDUCTIONLIST);
                        break;
                    #endregion
                    #region WORKING DAYS DETAILS
                    case ControlsEnum.WORKINGDAYSDETAILS:
                        BindGrid(ControlsEnum.WORKINGDAYSDETAILS);
                        break;
                    #endregion
                    #region EMPLOYEE TYPE
                    case ControlsEnum.EMPLOYEETYPE:
                        BindDropDown(ControlsEnum.EMPLOYEETYPE);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    #endregion
                    #region PAYROLL HISTORY
                    case ControlsEnum.PAYROLLHISTORY:
                        BindGrid(ControlsEnum.PAYROLLHISTORY);
                        break;
                    #endregion
                    #region PAYROLL TYPE DETAILS
                    case ControlsEnum.PAYROLLTYPEDETAILS:
                        GetUIValuesFromObject(ControlsEnum.PAYROLLTYPEDETAILS);
                        break;
                    #endregion
                    #region EXCHANGE RATE
                    case ControlsEnum.EXCHANGERATE:
                        GetUIValuesFromObject(ControlsEnum.EXCHANGERATE);
                        break;
                    #endregion
                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        GetUIValuesFromObject(ControlsEnum.CURRENCY);
                        break;
                    #endregion
                    #region EMP PAYROLL SORT LIST
                    case ControlsEnum.PAYROLLSORT:
                        BindGrid(ControlsEnum.PAYROLLSORT);
                        break;
                    #endregion
                    #region Non-Payroll Employees
                    case ControlsEnum.NONPAYROLL:
                        BindGrid(ControlsEnum.NONPAYROLL);
                        break;
                        #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region EMPLOYMENTTYPE
                case ControlsEnum.EMPLOYMENTTYPE:
                    ddlEmploymentType.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlEmploymentType.DataSource = dtResult;
                        ddlEmploymentType.DataTextField = GTIService.Constants.HRMS.Employee.Fields.HRM_CON_TEXT.HtmlDecode();
                        ddlEmploymentType.DataValueField = GTIService.Constants.HRMS.Employee.Fields.HRM_CON_PK;
                        ddlEmploymentType.DataBind();
                    }
                    ddlEmploymentType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region BRANCH / LOCATION
                case ControlsEnum.BRANCH:
                    ddlBranchLocation.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlBranchLocation.DataSource = dtResult;
                        ddlBranchLocation.DataTextField = GTIService.Constants.HRMS.Employee.Fields.HRM_CON_TEXT;
                        ddlBranchLocation.DataValueField = GTIService.Constants.HRMS.Employee.Fields.HRM_CON_PK;
                        ddlBranchLocation.DataBind();
                    }
                    ddlBranchLocation.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
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
                #region EMPLOYEE TYPE
                case ControlsEnum.EMPLOYEETYPE:
                    ddlEmployeeType.Items.Clear();
                    ddlEmployeeType.DataSource = dtResult;
                    ddlEmployeeType.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.EMT_NAME;
                    ddlEmployeeType.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.EMT_PK;
                    ddlEmployeeType.DataBind();
                    ddlEmployeeType.Items.HtmlDecode();
                    ddlEmployeeType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region COMPANY
                case ControlsEnum.COMPANY:
                    ddlCompany.Items.Clear();
                    ddlCompany.DataTextField = Resources.DataFieldRes.CMP_NAME;
                    ddlCompany.DataValueField = Resources.DataFieldRes.CMP_PK;
                    ddlCompany.DataSource = dtCompany;
                    ddlCompany.DataBind();
                    ddlCompany.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    ddlCompany.Items.HtmlDecode();
                    ddlCompanyHdr.Items.Clear();
                    ddlCompanyHdr.DataTextField = Resources.DataFieldRes.CMP_NAME;
                    ddlCompanyHdr.DataValueField = Resources.DataFieldRes.CMP_PK;
                    ddlCompanyHdr.DataSource = dtCompany;
                    ddlCompanyHdr.DataBind();
                    ddlCompanyHdr.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    ddlCompanyHdr.Items.HtmlDecode();
                    if (dtCompany != null && dtCompany.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK])))
                    {
                        ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK].ToString()));
                        ddlCompanyHdr.SelectedIndex = ddlCompanyHdr.Items.IndexOf(ddlCompanyHdr.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK].ToString()));
                    }

                    break;
                #endregion
                default:
                    break;
            }
        }



        public void BindGrid(ControlsEnum controlType)
        {
            Label lblTotalEarnPay;
            Label lblTotalDeductPay;
            Label lblTotalActualDeduct;
            Label lblTotalActualEarn;
            try
            {
                switch (controlType)
                {
                    #region EMP PAYROLL LIST
                    case ControlsEnum.EMPPAYROLLLIST:
                        if (objEmpPayrollHeader != null && objEmpPayrollHeader.EmpPayrollDtl != null)
                        {
                            grdEmpPayrollList.DataSource = objEmpPayrollHeader.EmpPayrollDtl;
                            grdEmpPayrollList.DataBind();
                            lblCurrProcessCount.Text = objEmpPayrollHeader.EmpPayrollDtl.Count.ToString() + "/" + objEmpPayrollHeader.EPH_PROCESSED_COUNT.ToString();
                        }
                        else
                        {
                            grdEmpPayrollList.DataSource = null;
                            grdEmpPayrollList.DataBind();
                            lblCurrProcessCount.Text = "0/0";
                        }
                        if (objEmpPayrollHeader != null)
                            lblTotalEmployees.Text = objEmpPayrollHeader.TOTAL_EMP_COUNT.ToString() + "/" + objEmpPayrollHeader.TOTAL_PRC_COUNT.ToString();
                        break;
                    #endregion
                    #region PERIOD LIST
                    case ControlsEnum.PERIODLIST:
                        uclPaging.TotalPages = TotalPages;
                        PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        //dtPageData = null;
                        grdPeriodList.DataSource = dtPageData;// dsPageData.Tables[0];
                        grdPeriodList.DataBind();
                        if (grdPeriodList.Rows.Count > 0)
                        {
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            uclPaging.Visible = false;
                        }
                        break;
                    #endregion
                    #region PAY EARNINGS LIST
                    case ControlsEnum.PAYEARNINGSLIST:
                        if (objEmpPayHeader != null && objEmpPayHeader.EmpPayrollPayDtl != null)
                        {
                            grdEarnPayDetails.DataSource = objEmpPayHeader.EmpPayrollPayDtl.Where(pay => pay.EPP_IS_DEDUCTION == 0).ToList();
                            grdEarnPayDetails.DataBind();
                            if (grdEarnPayDetails.FooterRow != null)
                            {
                                lblTotalEarnPay = grdEarnPayDetails.FooterRow.FindControl("lblTotalEarnPay") as Label;
                                lblTotalActualEarn = grdEarnPayDetails.FooterRow.FindControl("lblTotalActualEarn") as Label;
                                lblTotalEarnPay.Text = objEmpPayHeader.EmpPayrollPayDtl.Where(sal => sal.EPP_IS_DEDUCTION == 0 && sal.PEL_IN_SALARY == 1).Sum(tot => tot.EPP_PAY_AMT).ToString(hdfCurrencyFormatWithComma.Value);
                                lblTotalActualEarn.Text = objEmpPayHeader.EmpPayrollPayDtl.Where(act => act.EPP_IS_DEDUCTION == 0 && act.PEL_IN_SALARY == 1).Sum(actot => actot.EPP_ACT_PAY_AMT).ToString(hdfCurrencyFormatWithComma.Value);
                            }
                        }
                        else
                        {
                            grdEarnPayDetails.DataSource = null;
                            grdEarnPayDetails.DataBind();
                        }
                        break;
                    #endregion
                    #region PAY DEDUCTION LIST
                    case ControlsEnum.PAYDEDUCTIONLIST:
                        if (objEmpPayHeader != null && objEmpPayHeader.EmpPayrollPayDtl != null)
                        {
                            grdDeductPayDetails.DataSource = objEmpPayHeader.EmpPayrollPayDtl.Where(pay => pay.EPP_IS_DEDUCTION == 1).ToList();
                            grdDeductPayDetails.DataBind();
                            if (grdDeductPayDetails.FooterRow != null)
                            {
                                lblTotalDeductPay = grdDeductPayDetails.FooterRow.FindControl("lblTotalDeductPay") as Label;
                                lblTotalActualDeduct = grdDeductPayDetails.FooterRow.FindControl("lblTotalActualDeduct") as Label;
                                lblTotalDeductPay.Text = objEmpPayHeader.EmpPayrollPayDtl.Where(sal => sal.EPP_IS_DEDUCTION == 1 && sal.PEL_IN_SALARY == 1).Sum(tot => tot.EPP_PAY_AMT).ToString(hdfCurrencyFormatWithComma.Value);
                                lblTotalActualDeduct.Text = objEmpPayHeader.EmpPayrollPayDtl.Where(act => act.EPP_IS_DEDUCTION == 1 && act.PEL_IN_SALARY == 1).Sum(actot => actot.EPP_ACT_PAY_AMT).ToString(hdfCurrencyFormatWithComma.Value);
                            }
                        }
                        else
                        {
                            grdDeductPayDetails.DataSource = null;
                            grdDeductPayDetails.DataBind();
                        }
                        break;
                    #endregion
                    #region WORKING DAYS DETAILS
                    case ControlsEnum.WORKINGDAYSDETAILS:
                        if (workdayDetailList != null)
                        {
                            grdWorkingDays.DataSource = workdayDetailList;
                            grdWorkingDays.DataBind();
                        }
                        else
                        {
                            grdWorkingDays.DataSource = null;
                            grdWorkingDays.DataBind();
                        }
                        break;
                    #endregion
                    #region PAYROLL HISTORY
                    case ControlsEnum.PAYROLLHISTORY:
                        if (objEmpPayHeader != null)
                            grdPayrollHistory.DataSource = objEmpPayHeader.EmpSalHistory.ToList();
                        else
                            grdPayrollHistory.DataSource = null;
                        grdPayrollHistory.DataBind();
                        break;
                    #endregion
                    #region PAYROLL SORT
                    case ControlsEnum.PAYROLLSORT:
                        if (objEmpPayrollHeader != null && objEmpPayrollHeader.EmpPayrollDtl != null)
                        {
                            grdEmpPayrollList.DataSource = objEmpPayrollHeader.EmpPayrollDtl.ListSort(sortExpression, sortDirection).ToList();
                            grdEmpPayrollList.DataBind();
                        }
                        else
                        {
                            grdEmpPayrollList.DataSource = null;
                            grdEmpPayrollList.DataBind();
                        }
                        break;
                    #endregion
                    #region NON PAYROLL Employees
                    case ControlsEnum.NONPAYROLL:
                        if (dtNonPayroll != null)
                        {
                            grdNonPayrollEmps.DataSource = dtNonPayroll;
                            grdNonPayrollEmps.DataBind();
                        }
                        else
                        {
                            grdNonPayrollEmps.DataSource = null;
                            grdNonPayrollEmps.DataBind();
                        }
                        break;
                    #endregion

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                if (Mode == ActionsEnum.VIEW || Mode == ActionsEnum.VIEWPAYROLL)
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

        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region EM PPAYROLL LIST
                case ControlsEnum.EMPPAYROLLLIST:
                    CurrPK = 0;
                    CurrEmployeePayrollPK = 0;
                    base.WkfRefID = 0;
                    //ddlEmployee.ClearSelection();
                    txtEmployee.Text = string.Empty;
                    hdfEmployee.Value = string.Empty;
                    txtDepartment.Text = string.Empty;
                    hdfDepartment.Value = string.Empty;
                    ddlBranchLocation.ClearSelection();
                    ddlEmploymentType.ClearSelection();
                    txtCaption.Text = string.Empty;
                    //ddlPayrollType.ClearSelection();
                    //txtMonth.Text = string.Empty;
                    txtProcessDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                    txtResignDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                    txtProcessFromDate.Text = string.Empty;
                    txtProcessToDate.Text = string.Empty;
                    ViewState[ViewstateStrings.LastModifiedTime] = null;
                    empPayrollDetailList = null;
                    empPayDtlList = null;
                    workdayDetailList = null;
                    grdPeriodList.SelectedIndex = -1;
                    //ProcessMode = 0;
                    ddlProcessMode.SelectedIndex = ddlProcessMode.Items.IndexOf(ddlProcessMode.Items.FindByText(PayrollProcessMode.Monthly.ToString()));//Monthly
                    if (Convert.ToInt32(ddlProcessMode.SelectedValue) > 0)
                        ActionHandler(ddlProcessMode, new EventArgs());
                    break;
                #endregion
                #region CLEAR SEARCH
                case ControlsEnum.CLEARSEARCH:
                    CurrPK = 0;
                    CurrEmployeePayrollPK = 0;
                    //ddlEmployee.ClearSelection();
                    txtEmployee.Text = string.Empty;
                    hdfEmployee.Value = string.Empty;
                    txtDepartment.Text = string.Empty;
                    hdfDepartment.Value = string.Empty;
                    ddlBranchLocation.ClearSelection();
                    ddlEmploymentType.ClearSelection();
                    ddlEmployeeType.ClearSelection();
                    ddlCompany.ClearSelection();
                    //grdEmpPayrollList.DataSource = null;
                    //grdEmpPayrollList.DataBind();
                    break;
                #endregion
                #region EMP PAYROLL DETAIL LIST
                case ControlsEnum.EMPPAYROLLDETAILLIST:
                    lblPayrollProcess.Text = string.Empty;
                    lblPayrollProcessDate.Text = string.Empty;
                    lblPayrollEmployee.Text = string.Empty;
                    lblPaymentMode.Text = string.Empty;
                    lblCurrency.Text = string.Empty;
                    lblBankName.Text = string.Empty;
                    lblBranch.Text = string.Empty;
                    lblAccountName.Text = string.Empty;
                    lblAccountNO.Text = string.Empty;
                    lblPFNo.Text = string.Empty;
                    lblSOSCONo.Text = string.Empty;
                    lblWorkingDays.Text = string.Empty;
                    lblLOP.Text = string.Empty;
                    lblEmpGrossSalary.Text = string.Empty;
                    lblEmpNetSalary.Text = string.Empty;
                    lblEmpCTC.Text = string.Empty;
                    lblEmpTotEarnings.Text = string.Empty;
                    lblEmpTotDeduction.Text = string.Empty;
                    empPayDtlList = null;
                    break;
                #endregion
                #region CLEAR PROCESS DETAILS
                case ControlsEnum.CLEARPROCESSDETAILS:
                    CurrPK = 0;
                    // CurrEmployeePayrollPK = 0;
                    lblTrxNo.Text = Resources.ErpRes.Draft;
                    txtCaption.Text = string.Empty;
                    //txtMonth.Text = string.Empty;
                    txtProcessDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                    txtResignDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                    txtProcessFromDate.Text = string.Empty;
                    txtProcessToDate.Text = string.Empty;
                    ViewState[ViewstateStrings.LastModifiedTime] = null;
                    empPayrollDetailList = null;
                    empPayDtlList = null;
                    workdayDetailList = null;
                    lblPeriodHdr.Text = string.Empty;
                    lblCaptionHdr.Text = string.Empty;
                    lblProcessDateHdr.Text = string.Empty;
                    lblCurrProcessCount.Text = "0/0";
                    hdfIsCancelled.Value = "0";
                    lblTotalEmployees.Text = "0/0";
                    Status = 0;
                    JournalStatus = 0;
                    break;
                #endregion
                #region EMP PAYROLL LIST TAX
                case ControlsEnum.EMPPAYROLLLISTTAX:
                    CurrPK = 0;
                    txtEmployee.Text = string.Empty;
                    hdfEmployee.Value = string.Empty;
                    txtDepartment.Text = string.Empty;
                    hdfDepartment.Value = string.Empty;
                    ddlBranchLocation.ClearSelection();
                    ddlEmploymentType.ClearSelection();
                    txtCaption.Text = string.Empty;
                    txtProcessDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                    txtResignDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                    txtProcessFromDate.Text = string.Empty;
                    txtProcessToDate.Text = string.Empty;
                    ViewState[ViewstateStrings.LastModifiedTime] = null;
                    empPayrollDetailList = null;
                    empPayDtlList = null;
                    workdayDetailList = null;
                    grdPeriodList.SelectedIndex = -1;
                    break;
                    #endregion
            }
        }

        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }

        public string GetFormattedCurrencyWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithComma.Value);
        }
        public string GetFormattedExchangerate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfExchangeRateFormat.Value);
        }
        public DateTime FirstDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public DateTime LastDayOfMonth(DateTime dateTime)
        {
            DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }

        private void EnableDisablePayrollHdr(bool val)
        {
            ddlProcessMode.Enabled = val;
            txtProcessFromDate.Enabled = val;
            txtProcessToDate.Enabled = val;
            ddlPayrollType.Enabled = val;
            txtCurrency.Enabled = val;
            //ddlCompanyHdr.Enabled = val;
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>",
                "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
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

        #region EnableDisableButtons
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
        }
        #endregion
        #endregion

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

        #region Custom Pager Control Navigated Event
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
                    PageIndex = uclPaging.CurrentPage.ToString();
                    GetFieldValues(ControlsEnum.PERIODLIST);
                    SetFieldValues(ControlsEnum.PERIODLIST);
                    EntryStatus = EntryStatus.LISTMODE;
                    EnableDisableButtons(e.TotalPages, "uclPaging");
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





        #endregion
        #region Page Events
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();
        }

        /// <summary>
        /// PageInit Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///  
        protected void Page_Init(object sender, System.EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            uclPaging.CurrentPage = 1;
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            this.btnIncomeTax.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnReProcessPayroll.PreRender += new EventHandler(btnAction_PreRender);
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnProcessPayroll.PreRender += new EventHandler(btnAction_PreRender);
            this.btnPrintAll.PreRender += new EventHandler(btnAction_PreRender);
            this.btnPrintAllNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnPrintMultiple.PreRender += new EventHandler(btnAction_PreRender);
            this.btnExport.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnJournalize.PreRender += new EventHandler(btnAction_PreRender);

            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnPrint.Load += new EventHandler(btnAction_Load);
            this.btnIncomeTax.Load += new EventHandler(btnAction_Load);
            this.btnCancel.Load += new EventHandler(btnAction_Load);
            this.btnNew.Load += new EventHandler(btnAction_Load);
            this.btnReProcessPayroll.Load += new EventHandler(btnAction_Load);
            this.btnNew.Load += new EventHandler(btnAction_Load);
            this.btnProcessPayroll.Load += new EventHandler(btnAction_Load);
            this.btnPrintAll.Load += new EventHandler(btnAction_Load);
            this.btnPrintAllNew.Load += new EventHandler(btnAction_Load);
            this.btnPrintMultiple.Load += new EventHandler(btnAction_Load);
            this.btnExport.Load += new EventHandler(btnAction_Load);
            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            this.btnJournalize.Load += new EventHandler(btnAction_Load);

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

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);

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
                grdEarnPayDetails.Columns[3].Visible = true;
                grdDeductPayDetails.Columns[3].Visible = true;
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                btnReProcessPayroll.Visible = CurrPK > 0 ? true : false;
                btnPrintAll.Visible = CurrPK > 0 ? true : false;
                btnPrintMultiple.Visible = CurrPK > 0 ? true : false;
                btnPrintAllNew.Visible = CurrPK > 0 ? true : false;
                btnExport.Visible = CurrPK > 0 ? true : false;
                //btnIncomeTax.Visible = true;
                if (!string.IsNullOrEmpty(ddlProcessMode.SelectedValue))
                {
                    if (Convert.ToInt32(ddlProcessMode.SelectedValue) != ((int)PayrollProcessMode.Periodic))
                        imbWorkingDays.Visible = Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "HrmsPayrollWorkingDayShow"));
                    else
                        imbWorkingDays.Visible = false;

                }
            }
            if (EntryStatus == EntryStatus.VIEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "EnableDisableChanges", "EnableDisableChanges();", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                grdEarnPayDetails.Columns[3].Visible = false;
                grdDeductPayDetails.Columns[3].Visible = false;
            }
            if (CurrPK == 0)
                btnSubmit.Visible = false;
            if (CurrPK == 0 || Status == 0)
                btnCancelSubmit.Visible = false;
            if (!MultiCurrencyEnabled)
                txtCurrency.Enabled = false;
            hdfIsPosted.Value = JournalStatus.ToString();
            if (JournalStatus > 0)
            {
                btnCancelSubmit.Visible = false;
                btnProcessPayroll.Visible = false;
                btnReProcessPayroll.Visible = false;
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "hideProgress", "hideProgress();", true);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Redirect to a page if user has permission on that page.
        /// </summary>
        /// <param name="RedirectUrl">Page Url</param>
        private void CheckUserRightsAndRedirect(string RedirectUrl, ActionsEnum commonActions)
        {
            CommonBL userAuth = new CommonBL();
            if (currentUser == null)
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            UserRightsBO UsrRights = userAuth.GetUserPageRights(currentUser.PKUser, RedirectUrl.Replace("~", ""), currentUser.SBUID, currentUser.CurrentDeptPK);
            if (UsrRights != null && UsrRights.Rights.Count() > 0 && UsrRights.Rights[0].UserDeptRight)
            {
                Response.Redirect(RedirectUrl, false);
            }
            else
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErpRes.NoPressionMsg) + "','" + Resources.ErpRes.Information + "');", true);
        }

        private void SetPrintVisibility()
        {
            string[] strInactivetabs = GetGlobalResourceObject("ConfigurationsRes", "HrmsPayrollInactiveTabs").ToString().Split(',');
            if (strInactivetabs != null && strInactivetabs.Count() > 0)
            {
                foreach (string strTab in strInactivetabs)
                {
                    int emptab = 0;
                    int.TryParse(strTab, out emptab);
                    switch (emptab)
                    {
                        #region PRE PROCESS DATA
                        case (int)PayrollTabEnum.PREPROCESSDATA:
                            pnlPrintAll.Visible = true;
                            pnlPrintAllNew.Visible = false;
                            break;
                        #endregion

                        default:
                            break;
                    }
                }
            }
        }

        #endregion
        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            EMPLOYMENTTYPE,
            BRANCH,
            PAYROLLTYPE,
            PROCESSMODE,
            EMPPAYROLLLIST,
            CLEARSEARCH,
            PROCESSPAYROLLHDR,
            PROCESSPAYROLLDTL,
            PERIODLIST,
            EMPPAYROLLDETAILLIST,
            PAYEARNINGSLIST,
            PAYDEDUCTIONLIST,
            EMPPAYROLLHDR,
            EMPPAYROLLDTL,
            CLEARPROCESSDETAILS,
            WORKINGDAYSDETAILS,
            EMPLOYEETYPE,
            COMPANY,
            PAYROLLHISTORY,
            PAYROLLTYPEDETAILS,
            PRINTMULTIPLE,
            PRINTPAYSLIPSINGLE,
            DELETEALL,
            EMPPAYROLLLISTTAX,
            EXCHANGERATE,
            CURRENCY,
            GETPAYROLLBYJOURNALPK,
            JOURNALIZE,
            FINHEADER,
            DEPARTMENTBYTYPE,
            PAYROLLSORT,
            NONPAYROLL

        }
        #endregion
    }
}