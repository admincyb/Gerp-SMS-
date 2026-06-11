using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject;
using System.Data;
using BusinessLogic.HRMS.Payroll;
using ERP.Utilities;
using GTIService.Constants.HRMS.Payroll;
using BusinessObject.Common;
using BusinessObject.AccountManagement;
using BusinessObject.CommonManagement;
using BusinessObject.HRMS.Payroll;
using System.Threading;
using ERPSMS_v01.UserControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using BusinessLogic.ReportsManagement;


namespace HRMS.Payroll
{
    public partial class PayrollPreprocess : ERP.Store.UI.MyBasePage
    {
        #region Variables
        User currentUser;
        private ActionsEnum commonActions;
        private DataTable dtResult;
        private DataTable dtCompany;
        private PayrollPreprocessFilter objPayrollPreprocess;
        private ReportDocument reportDocument;
        private ParameterField paramField;
        private ParameterFields rptParamFields;
        private ParameterDiscreteValue paramDiscreteValue;

        DataTable dtAppTypeDetails;
        DataSet dsHrms;
        private string RptType;
        private int RptSubType;
        private DateTime AppvdDate = DateTime.Now;
        private int RecPK;
        private int PayrollTypePk = 0;
        #endregion

        #region Properties
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
        private bool PreprocessDisabled
        {
            get
            {
                return this.ViewState[ViewstateStrings.PreprocessDisabled] == null ? false : Convert.ToBoolean((this.ViewState[ViewstateStrings.PreprocessDisabled]));
            }
            set
            {
                this.ViewState[ViewstateStrings.PreprocessDisabled] = value;
            }
        }
        private PayrollPreprocessFilter PayrollPreprocessFilter
        {
            get
            {
                return (PayrollPreprocessFilter)Session[ViewstateStrings.PayrollPreprocessFilter];
            }
            set
            {
                Session[ViewstateStrings.PayrollPreprocessFilter] = value;
            }
        }
        #endregion

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = (BusinessObject.User)HttpContext.Current.User.Identity;
            if (!IsPostBack)
            {
                PageActionHandler();
            }
        }
        #endregion

        #region Functions

        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                ResConfigurationSettings();
                if (PreprocessDisabled)
                    Response.Redirect(Resources.PageURL.HrmsPayroll);
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
                GetFieldValues(ControlsEnum.PAYROLLTYPE);
                SetFieldValues(ControlsEnum.PAYROLLTYPE);
                hdfShowCrReportDiv.Value = "0";
                hdfMonthlyMode.Value = ((int)PayrollProcessMode.Monthly).ToString();
                ResetForm(ControlsEnum.CLEARSEARCH);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        #endregion


        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    #region PAYRL PRE PROCESS
                    case ControlsEnum.PAYRLPREPROCESS:
                        objPayrollPreprocess = (PayrollPreprocessFilter)SetUIValuesToObject(ControlsEnum.PAYRLPREPROCESS);
                        string xmlPreprocess = CommonFunctions.XmlSerialize<PayrollPreprocessFilter>(objPayrollPreprocess);
                        dsHrms = PayrollProcessBL.GetPayrollPreprocess(xmlPreprocess);
                        break;
                    #endregion
                    #region REPORT CONFIG
                    case ControlsEnum.REPORTCONFIG:
                        dtAppTypeDetails = GenerateReportBL.GetReportParameters(RptType, RptSubType, AppvdDate);
                        break;
                    #endregion
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
                        processmode = Convert.ToInt32(ddlProcessMode.SelectedValue) > 0 ? Convert.ToInt32(ddlProcessMode.SelectedValue) : 0;
                        dtResult = PayrollProcessBL.GetPayrollType(0, currentUser.SBUID, Convert.ToInt32(DbActiveStatus.ACTIVE), processmode);//, currentUser.PKUser
                        break;
                    #endregion
                    #region EMPLOYEE TYPE
                    case ControlsEnum.EMPLOYEETYPE:
                        dtResult = BusinessLogic.HRMS.Employee.EmployeePayDetailsBL.GetEmployeeTypeGetKV(0, currentUser.SBUID, (int)DbActiveStatus.ACTIVE);
                        break;
                    #endregion
                    #region PAYROLL TYPE DETAILS
                    case ControlsEnum.PAYROLLTYPEDETAILS:
                        dtResult = PayrollProcessBL.GetPayrollType(PayrollTypePk, currentUser.SBUID, Convert.ToInt32(DbActiveStatus.HASPK), Convert.ToInt32(ddlProcessMode.SelectedValue));
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), 0, 0);
                        break;
                    #endregion
                    #region DEFAULT
                    default:
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

            }
        }

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
                    #region EMPLOYMENT TYPE
                    case ControlsEnum.EMPLOYMENTTYPE:
                        BindDropDown(ControlsEnum.EMPLOYMENTTYPE);
                        break;
                    #endregion
                    #region BRANCH / LOCATION
                    case ControlsEnum.BRANCH:
                        BindDropDown(ControlsEnum.BRANCH);
                        break;
                    #endregion
                    #region PAYROLLT YPE
                    case ControlsEnum.PAYROLLTYPE:
                        BindDropDown(ControlsEnum.PAYROLLTYPE);
                        break;
                    #endregion
                    #region PROCESS MODE
                    case ControlsEnum.PROCESSMODE:
                        BindDropDown(ControlsEnum.PROCESSMODE);
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
                    #region DEFAULT
                    default:
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
        /// Method for Dropdown binding
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region EMPLOYMENT TYPE
                case ControlsEnum.EMPLOYMENTTYPE:
                    ddlEmploymentType.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlEmploymentType.DataSource = dtResult;
                        ddlEmploymentType.DataTextField = GTIService.Constants.HRMS.Employee.Fields.HRM_CON_TEXT;
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
                    if (dtCompany != null && dtCompany.Rows.Count > 0)
                    {
                        ddlCompany.DataSource = dtCompany;
                        ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompany.DataBind();
                        ddlCompany.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        ddlCompany.Items.HtmlDecode();
                        if (dtCompany != null && dtCompany.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK])))
                        {
                            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK].ToString()));
                        }
                    }
                    break;
                #endregion
                #region DEFAULT
                default:
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
                    #region DEFAULT
                    default:
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
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            int empDept = 0;
            int empPk = 0;
            try
            {
                switch (controlType)
                {
                    #region PAYRL PRE PROCESS
                    case ControlsEnum.PAYRLPREPROCESS:
                        objPayrollPreprocess = new PayrollPreprocessFilter();
                        int.TryParse(hdfDepartment.Value, out empDept);
                        int.TryParse(hdfEmployee.Value, out empPk);
                        objPayrollPreprocess.BIZUNIT_PK = currentUser.SBUID;
                        objPayrollPreprocess.EMP_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue) > 0 ? ddlCompany.SelectedValue : null;
                        objPayrollPreprocess.EMP_BRANCH = Convert.ToInt32(ddlBranchLocation.SelectedValue) > 0 ? ddlBranchLocation.SelectedValue : null;
                        objPayrollPreprocess.EMP_DEPT = empDept > 0 ? empDept.ToString() : null;
                        objPayrollPreprocess.EMP_PAYROLL_MONTH = !string.IsNullOrEmpty(txtSalaryMonth.Text.Trim()) ? Convert.ToDateTime(txtSalaryMonth.Text.Trim()).ToString() : null;
                        objPayrollPreprocess.EMP_FROM_DATE = !string.IsNullOrEmpty(txtProcessFromDate.Text.Trim()) ? txtProcessFromDate.Text.Trim() : null;
                        objPayrollPreprocess.EMP_TO_DATE = !string.IsNullOrEmpty(txtProcessToDate.Text.Trim()) ? txtProcessToDate.Text.Trim() : null;
                        objPayrollPreprocess.EMP_PRC_MODE = Convert.ToInt32(ddlProcessMode.SelectedValue) > 0 ? ddlProcessMode.SelectedValue : null;
                        objPayrollPreprocess.EMP_PAYROLL_TYPE = Convert.ToInt32(ddlPayrollType.SelectedValue) > 0 ? ddlPayrollType.SelectedValue : null;
                        objPayrollPreprocess.EMP_TYPE = Convert.ToInt32(ddlEmployeeType.SelectedValue) > 0 ? ddlEmployeeType.SelectedValue : null;
                        objPayrollPreprocess.EMP_EMPLOYMENT_TYPE = Convert.ToInt32(ddlEmploymentType.SelectedValue) > 0 ? ddlEmploymentType.SelectedValue : null;
                        objPayrollPreprocess.EMP_PK = empPk > 0 ? empPk.ToString() : null;
                        retObject = objPayrollPreprocess;
                        break;
                    #endregion
                    #region DEFAULT
                    default:
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
                    #region DEFAULT
                    default:
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
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region CLEAR SEARCH
                case ControlsEnum.CLEARSEARCH:
                    txtSalaryMonth.Text = string.Empty;
                    txtProcessFromDate.Text = string.Empty;
                    txtProcessToDate.Text = string.Empty;
                    ddlProcessMode.ClearSelection();
                    ddlPayrollType.ClearSelection();
                    ddlEmployeeType.ClearSelection();
                    txtDepartment.Text = string.Empty;
                    hdfDepartment.Value = string.Empty;
                    ddlEmploymentType.ClearSelection();
                    ddlBranchLocation.ClearSelection();
                    txtEmployee.Text = string.Empty;
                    hdfEmployee.Value = string.Empty;
                    ClearCrystalReport();
                    if (reportDocument != null)
                    {
                        reportDocument.Close();
                        reportDocument.Dispose();
                        reportDocument = null;
                        GC.Collect();
                    }
                    hdfShowCrReportDiv.Value = "0";
                    Session[ERP.Utilities.SessionStrings.CRReportData] = null;
                    Session[ERP.Utilities.SessionStrings.CRReportParam] = null;
                    break;
                #endregion
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

        private void ClearCrystalReport()
        {
            hdfShowCrReportDiv.Value = "0";
            GERP_OutputReport.ReportSource = null;
            GERP_OutputReport.RefreshReport();
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

        #endregion

        #region Action Handler
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ActionHandler(object sender, EventArgs e)
        {
            try
            {

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
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    if (((TextBox)sender).ID == "txtSalaryMonth")
                    {
                        commonActions = ActionsEnum.SALMONTHCHANGED;
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

                switch (commonActions)
                {
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                    case ActionsEnum.VIEW:
                        RptType = ApplicationType.PAYRL;
                        RptSubType = Convert.ToInt32(AppSubType.PAYPREPCSRPT);
                        GetFieldValues(ControlsEnum.REPORTCONFIG);
                        if (dtAppTypeDetails == null)
                        {
                            ClearCrystalReport();
                            return;
                        }
                        #region crystal report calling
                        if (reportDocument != null)
                        {
                            ClearCrystalReport();
                            reportDocument.Close();
                            reportDocument.Dispose();
                            reportDocument = null;
                            GC.Collect();
                        }
                        GetFieldValues(ControlsEnum.PAYRLPREPROCESS);
                        SetFieldValuesCrystal(RptType, dtAppTypeDetails.Rows[0]["AST_OP_FILE1"].ToString());
                        #endregion
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        break;
                    #endregion
                    #region SALARY MONTH CHANGED
                    case ActionsEnum.SALMONTHCHANGED:
                        SetPayrollProcessDates();
                        break;
                    #endregion
                    #region PROCESS MODE CHANGED
                    case ActionsEnum.PROCESSMODECHANGED:
                        GetFieldValues(ControlsEnum.PAYROLLTYPE);
                        SetFieldValues(ControlsEnum.PAYROLLTYPE);
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ddlPayrollType.SelectedValue = dtResult.Rows[0][GTIService.Constants.HRMS.Employee.Fields.PTM_PK].ToString();
                        }
                        if (Convert.ToInt32(ddlPayrollType.SelectedValue) > 0)
                            ActionHandler(ddlPayrollType, new EventArgs());
                        break;
                    #endregion
                    #region PAYROLL TYPE CHANGED
                    case ActionsEnum.PAYROLLTYPECHANGED:
                        PayrollTypePk = Convert.ToInt32(ddlPayrollType.SelectedValue);
                        GetFieldValues(ControlsEnum.PAYROLLTYPEDETAILS);
                        SetPayrollProcessDates();
                        break;
                    #endregion
                    #region EXCEL PRINT
                    case ActionsEnum.EXCELPRINT:
                        PayrollPreprocessFilter = null;
                        GetFieldValues(ControlsEnum.PAYRLPREPROCESS);
                        PayrollPreprocessFilter = objPayrollPreprocess;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "PAYRL" + "&APPSUBTYPE= 6&IsExcelPrint=1") + "');", true);
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
                    int offset = Convert.ToInt32(dtResult.Rows[0]["PTM_OFFSET"]);
                    int payrlstart = Convert.ToInt32(dtResult.Rows[0]["PTM_PAYRL_START"]);
                    DateTime tempDate = new DateTime(salMonth.Year, salMonth.Month + offset, 1);
                    if (DateTime.DaysInMonth(tempDate.Year, tempDate.Month) >= payrlstart)
                        processDate = new DateTime(tempDate.Year, tempDate.Month, payrlstart);
                    else
                        processDate = new DateTime(tempDate.Year, tempDate.Month, DateTime.DaysInMonth(tempDate.Year, tempDate.Month));
                    txtProcessFromDate.Text = processDate.ToString(Resources.Constants.HRMSDateFormatShort);
                    if (Convert.ToInt32(ddlProcessMode.SelectedValue) == (int)PayrollProcessMode.Monthly)
                    {
                        DateTime processToDate = processDate.AddMonths(1).AddDays(-1);
                        txtProcessToDate.Text = processToDate.ToString(Resources.Constants.HRMSDateFormatShort);
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

        #region Set Field Values Crystal Report
        private void SetFieldValuesCrystal(string appType, string reportName)
        {
            try
            {
                reportDocument = new ReportDocument();
                reportDocument.Load(Server.MapPath("~/Reports/CrystalReport/" + reportName));
                reportDocument.Refresh();
                ERP.Utilities.CommonFunctions.SetCrystalReportDataBaseConnection(reportDocument);
                //GERP_OutputReport.ReportSource = reportDocument;

                dsHrms.Tables[0].TableName = "dtSalPreprocess_Hdr";
                dsHrms.Tables[1].TableName = "dtSalPreprocess";
                reportDocument.SetDataSource(dsHrms);
                //GERP_OutputReport.ReportSource = reportDocument;
                ParameterFieldDefinitions crParameterdef;
                crParameterdef = reportDocument.DataDefinition.ParameterFields;
                rptParamFields = SetCrystalreportParameters(crParameterdef);
                GERP_OutputReport.ReportSource = reportDocument;
                GERP_OutputReport.ParameterFieldInfo = rptParamFields;
                //rptParamFields = SetCrystalreportCommonParameters(reportDocument, RecPK.ToString());
                hdfShowCrReportDiv.Value = "1";
                //GERP_OutputReport.ParameterFieldInfo = rptParamFields;
                //GERP_OutputReport.ReportSource = reportDocument;
                Session[ERP.Utilities.SessionStrings.CRReportParam] = rptParamFields;
                Session[ERP.Utilities.SessionStrings.CRReportData] = reportDocument;


                //reportDocument.ExportToHttpResponse(ExportFormatType.Excel, Page.Response, false, reportName);

                //reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Page.Response, false, reportName);
                //#region For Crystal report
                ////If any report document exist, need to dispose the object
                //if (!IsPostBack)
                //{
                //    //crReportViewer.Visible = false;
                //    if (reportDocument != null)
                //    {
                //        reportDocument.Close();
                //        reportDocument.Dispose();
                //        reportDocument = null;
                //    }
                //}
                //#endregion
                //reportDocument = (ReportDocument)Session[ERP.Utilities.SessionStrings.CRReportData];
                //if (reportDocument != null)
                //    CommonFunctions.SetCrystalReportDataBaseConnection(reportDocument);
                //ParameterFields locParamFields = (ParameterFields)Session[ERP.Utilities.SessionStrings.CRReportParam];
                //GERP_OutputReport.ParameterFieldInfo = locParamFields;
                //GERP_OutputReport.ReportSource = reportDocument;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        private ParameterFields SetCrystalreportParameters(ParameterFieldDefinitions crParameterdef)
        {
            ParameterFields paramFields = new ParameterFields();
            ParameterField paramField = new ParameterField();
            ParameterDiscreteValue paramDiscreteValue = new ParameterDiscreteValue();

            #region Extra Parameter based on query string
            //if (RptType == ApplicationType.TRACE && RptSubType == 2)
            //{
            //    rptDocument.SetParameterValue("BatchNo", BatchNo.ToString());
            //}
            #endregion

            DataSet dsParamSettings = new DataSet();
            string footer;
            string rptName = string.Empty;
            footer = string.Empty;
            try
            {
                if (dtAppTypeDetails != null && dtAppTypeDetails.Rows.Count > 0)
                {
                    if (dtAppTypeDetails.Rows[0]["AST_RPT_SETTINGS"] != null)
                        dsParamSettings.ReadXml(new System.IO.StringReader(Convert.ToString(dtAppTypeDetails.Rows[0]["AST_RPT_SETTINGS"])));
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    int curdigit = 2;
                    int NoDigit = 2;
                    int ExchRate = 2;
                    int RateDecimal = 2;
                    int RateDecimalPP = 2;
                    int WeightDigit = 2;

                    DataTable dt = ConfigurationSettings();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        curdigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "CurrencyDecimalDigit")["ACF_VALUE"].ToString());
                        NoDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigit")["ACF_VALUE"].ToString());
                        ExchRate = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "ExchRateDecimalDigit")["ACF_VALUE"].ToString());
                        RateDecimal = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigit")["ACF_VALUE"].ToString());
                        RateDecimalPP = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigitP2P")["ACF_VALUE"].ToString());
                        WeightDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "WeightDecimalDigit")["ACF_VALUE"].ToString());

                        paramFields.Add(SetParamValue("CurrencyDigits", curdigit.ToString()));
                        paramFields.Add(SetParamValue("DateFormat", Resources.Constants.ReportDateFormat.ToString()));
                    }

                    if (dsParamSettings.Tables.Count > 0)
                    {
                        if (dsParamSettings.Tables[0].Columns.Contains("LOGO_HIDE"))
                            paramFields.Add(SetParamValue("HideLogo", dsParamSettings.Tables[0].Rows[0]["LOGO_HIDE"].ToString()));
                        if (dsParamSettings.Tables[0].Columns.Contains("HEADING_HIDE"))
                            paramFields.Add(SetParamValue("HideHeadTitle", dsParamSettings.Tables[0].Rows[0]["HEADING_HIDE"].ToString()));
                        if (dsParamSettings.Tables[0].Columns.Contains("SUB_HEADING_HIDE"))
                            paramFields.Add(SetParamValue("HideSubTitle", dsParamSettings.Tables[0].Rows[0]["SUB_HEADING_HIDE"].ToString()));
                        if (dsParamSettings.Tables[0].Columns.Contains("FOOTER_LEFT_HIDE"))
                            paramFields.Add(SetParamValue("HideFooterText", dsParamSettings.Tables[0].Rows[0]["FOOTER_LEFT_HIDE"].ToString()));
                        if (dsParamSettings.Tables[0].Columns.Contains("FOOTER_RIGHT_HIDE"))
                            paramFields.Add(SetParamValue("HidePageNo", dsParamSettings.Tables[0].Rows[0]["FOOTER_RIGHT_HIDE"].ToString()));
                        if (dsParamSettings.Tables[0].Columns.Contains("HEADING"))
                            paramFields.Add(SetParamValue("HeadTitle", dsParamSettings.Tables[0].Rows[0]["HEADING"].ToString()));
                        if (dsParamSettings.Tables[0].Columns.Contains("SUB_HEADING"))
                            paramFields.Add(SetParamValue("SubTitle", dsParamSettings.Tables[0].Rows[0]["SUB_HEADING"].ToString()));
                    }
                }
                paramFields.Add(SetParamValue("Logo", Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogoPDF"])));
                footer = string.Format(GetLocalResourceObject("FooterText").ToString(), currentUser.EmpName, DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"));
                paramFields.Add(SetParamValue("FooterText", footer));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return paramFields;
        }
        #region SetCrystalreportCommonParameters
        private ParameterField SetParamValue(string paramName, string paramValue)
        {
            ParameterField paramField = new ParameterField();
            ParameterDiscreteValue paramDiscreteValue = new ParameterDiscreteValue();
            paramField.Name = paramName;
            paramDiscreteValue.Value = paramValue;
            paramField.CurrentValues.Add(paramDiscreteValue);
            return paramField;
        }
        private ParameterFields SetCrystalreportCommonParameters(ReportDocument rptDocument, string rptPK)
        {
            ParameterFields paramFields = new ParameterFields();
            ParameterField paramField = new ParameterField();
            #region Extra Parameter based on query string
            //if (RptType == ApplicationType.TRACE && RptSubType == 2)
            //{
            //    rptDocument.SetParameterValue("BatchNo", BatchNo.ToString());
            //}
            #endregion

            DataSet dsParamSettings = new DataSet();
            string footer;
            string rptName = string.Empty;
            footer = string.Empty;
            try
            {
                if (dtAppTypeDetails != null && dtAppTypeDetails.Rows.Count > 0)
                {
                    if (dtAppTypeDetails.Rows[0]["AST_RPT_SETTINGS"] != null)
                        dsParamSettings.ReadXml(new System.IO.StringReader(Convert.ToString(dtAppTypeDetails.Rows[0]["AST_RPT_SETTINGS"])));
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    int curdigit = 2;
                    int NoDigit = 2;
                    int ExchRate = 2;
                    int RateDecimal = 2;
                    int RateDecimalPP = 2;
                    int WeightDigit = 2;
                    DataTable dt = ConfigurationSettings();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        curdigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "CurrencyDecimalDigit")["ACF_VALUE"].ToString());
                        NoDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigit")["ACF_VALUE"].ToString());
                        ExchRate = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "ExchRateDecimalDigit")["ACF_VALUE"].ToString());
                        RateDecimal = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigit")["ACF_VALUE"].ToString());
                        RateDecimalPP = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigitP2P")["ACF_VALUE"].ToString());
                        WeightDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "WeightDecimalDigit")["ACF_VALUE"].ToString());

                    }
                    paramFields.Add(SetParamValue("CurrencyDigits", curdigit.ToString()));
                    paramFields.Add(SetParamValue("NumberDigits", NoDigit.ToString()));
                    paramFields.Add(SetParamValue("ExchangeRate", ExchRate.ToString()));
                    paramFields.Add(SetParamValue("RateDigits", RateDecimal.ToString()));
                    paramFields.Add(SetParamValue("WeightDigits", WeightDigit.ToString()));
                    paramFields.Add(SetParamValue("DateFormat", Resources.Constants.ReportDateFormat.ToString()));
                    if (dsParamSettings.Tables.Count > 0)
                    {
                        if (dsParamSettings.Tables[0].Columns.Contains("LOGO_HIDE"))
                        {
                            paramFields.Add(SetParamValue("HideLogo", dsParamSettings.Tables[0].Rows[0]["LOGO_HIDE"].ToString()));
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("HEADING_HIDE"))
                        {
                            paramFields.Add(SetParamValue("HideHeadTitle", dsParamSettings.Tables[0].Rows[0]["HEADING_HIDE"].ToString()));
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("SUB_HEADING_HIDE"))
                        {
                            paramFields.Add(SetParamValue("HideSubTitle", dsParamSettings.Tables[0].Rows[0]["SUB_HEADING_HIDE"].ToString()));
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("FOOTER_LEFT_HIDE"))
                        {
                            paramFields.Add(SetParamValue("HideFooterText", dsParamSettings.Tables[0].Rows[0]["FOOTER_LEFT_HIDE"].ToString()));
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("FOOTER_RIGHT_HIDE"))
                        {
                            paramFields.Add(SetParamValue("HidePageNo", dsParamSettings.Tables[0].Rows[0]["FOOTER_RIGHT_HIDE"].ToString()));
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("HEADING"))
                        {
                            paramFields.Add(SetParamValue("HeadTitle", dsParamSettings.Tables[0].Rows[0]["HEADING"].ToString()));
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("SUB_HEADING"))
                        {
                            paramFields.Add(SetParamValue("SubTitle", dsParamSettings.Tables[0].Rows[0]["SUB_HEADING"].ToString()));
                        }
                    }
                }
                paramFields.Add(SetParamValue("Logo", Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogoPDF"])));
                footer = string.Format(GetLocalResourceObject("FooterText").ToString(), currentUser.EmpName, DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"));
                paramFields.Add(SetParamValue("FooterText", footer));
                return paramFields;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region  ConfigurationSettings
        /// <summary>
        /// ConfigurationSettings for rate,currency,qty,weight
        /// </summary>
        /// <returns></returns>
        private DataTable ConfigurationSettings()
        {
            //Initialze the current logged in user to the currentUser variable
            currentUser = (BusinessObject.User)HttpContext.Current.User.Identity;
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ReportCurrency, string.Empty, currentUser.CurrentSBUPK);
            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        private void ResConfigurationSettings()
        {
            string[] strPayrollInactivetabs = GetGlobalResourceObject("ConfigurationsRes", "HrmsPayrollInactiveTabs").ToString().Split(',');
            if (strPayrollInactivetabs != null && strPayrollInactivetabs.Count() > 0)
            {
                if (strPayrollInactivetabs.Contains(((int)PayrollTabEnum.PREPROCESSDATA).ToString()))
                    PreprocessDisabled = true;
            }

        }

        #endregion
        #endregion
        #endregion


        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {

        }


        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    TextBox txtInstallmentDate = e.Row.FindControl("txtInstallmentDate") as TextBox;
                    TextBox txtInstAmt = e.Row.FindControl("txtInstAmt") as TextBox;
                    Label lblPaidAmt = e.Row.FindControl("lblPaidAmt") as Label;
                    Label lblBalanceAmt = e.Row.FindControl("lblBalanceAmt") as Label;
                    ImageButton imbDeleteDetails = e.Row.FindControl("imbDeleteDetails") as ImageButton;

                    lblBalanceAmt.Text = GetFormattedCurrency((txtInstAmt.Text == string.Empty ? 0 : Convert.ToDouble(txtInstAmt.Text)) - (lblPaidAmt.Text == string.Empty ? 0 : Convert.ToDouble(lblPaidAmt.Text))).ToString();
                    if (Convert.ToDouble(lblPaidAmt.Text) > 0)
                    {
                        txtInstallmentDate.Enabled = false;
                        txtInstAmt.Enabled = false;
                        imbDeleteDetails.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Page Events
        protected void Page_Init(object sender, System.EventArgs e)
        {
            #region For Crystal report
            //If any report document exist, need to dispose the object
            if (!IsPostBack)
            {
                //GERP_MIS_Report.Visible = false;
                if (reportDocument != null)
                {
                    reportDocument.Close();
                    reportDocument.Dispose();
                    reportDocument = null;
                }
            }
            #endregion
            reportDocument = (ReportDocument)Session[ERP.Utilities.SessionStrings.CRReportData];
            if (reportDocument != null)
                CommonFunctions.SetCrystalReportDataBaseConnection(reportDocument);
            ParameterFields locParamFields = (ParameterFields)Session[ERP.Utilities.SessionStrings.CRReportParam];
            GERP_OutputReport.ParameterFieldInfo = locParamFields;
            GERP_OutputReport.ReportSource = reportDocument;
        }

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
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
            //base.CheckBtnVisibility(sender);
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
        /// Leave this section if using Master Screens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();

        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>",
                "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
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
            REPORTCONFIG,
            PAYRLPREPROCESS,
            PROCESSMODE,
            PAYROLLTYPE,
            EMPLOYEETYPE,
            COMPANY,
            CLEARSEARCH,
            PAYROLLTYPEDETAILS
        }
        #endregion
    }
}