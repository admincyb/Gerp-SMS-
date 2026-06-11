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
using BusinessLogic.CommonManagement;
using ERPSMS_v01;


namespace HRMS.Payroll
{
    public partial class LoansAndAdvances : ERP.Store.UI.MyBasePage
    {
        #region Variables
        User currentUser;
        private ActionsEnum commonActions;

        DataSet dsEmpLoanList;
        private DataTable dtEmpLoanList;
        private DataTable dtResult;
        private DataTable dtCompany;
        private DataSet dsExchangeRate;
        private EmpLoanDetails objEmpLoanDetails;
        //private List<EmpLoanDetails> empLoanDetailList;       
        private EmpLoanHeader objEmpLoanHeader;
        private int CompanyPk = 0;
        private int pelPK = 0;
        private BusinessObject.HRMS.Employee.EmployeePayDetailsBO selectedEmployeePayDetails;
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

        private List<EmpLoanDetails> empLoanDetailList
        {
            get
            {
                return ViewState[ViewstateStrings.empLoanDetailList] == null ? null : (List<EmpLoanDetails>)ViewState[ViewstateStrings.empLoanDetailList];
            }
            set
            {
                ViewState[ViewstateStrings.empLoanDetailList] = value;
            }
        }

        public double TotalPrincipalAmt
        {
            get
            {
                return Convert.ToDouble(this.ViewState[ViewstateStrings.TotalPrincipalAmt]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPrincipalAmt] = value;
            }
        }

        public double InterestAmount
        {
            get
            {
                return Convert.ToDouble(this.ViewState[ViewstateStrings.InterestAmount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.InterestAmount] = value;
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

        private DateTime? EmpResignedDate
        {
            get
            {
                return this.ViewState[ViewstateStrings.EmpResignedDate] == null ? (DateTime?)null : Convert.ToDateTime((this.ViewState[ViewstateStrings.EmpResignedDate]));
            }
            set
            {
                this.ViewState[ViewstateStrings.EmpResignedDate] = value;
            }
        }
        #endregion

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PageActionHandler();
            }
        }
        #endregion

        #region Functions

        #region PageActionHandler
        private void PageActionHandler()
        {
            try
            {
                hdfNextYearDate.Value = DateTime.Now.AddYears(-1).ToString(Resources.Constants.HRMSDateFormatShort);
                this.PageIndex = 1;
                uclPaging.TotalPages = TotalPages;
                uclPaging.CurrentPage = 1;
                pelPK = 0;
                //set of hidden fields used to format Quantity, Amount, Rate
                string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
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

                GetFieldValues(ControlsEnum.COMPANY);
                SetFieldValues(ControlsEnum.COMPANY);
                GetFieldValues(ControlsEnum.LOANADVANCEITEM);
                SetFieldValues(ControlsEnum.LOANADVANCEFILTERITEM);
                GetFieldValues(ControlsEnum.EMPLOYMENTTYPE);
                SetFieldValues(ControlsEnum.EMPLOYMENTTYPE);
                GetFieldValues(ControlsEnum.BRANCH);
                SetFieldValues(ControlsEnum.BRANCH);
                GetFieldValues(ControlsEnum.EMPLOANLIST);
                SetFieldValues(ControlsEnum.EMPLOANLIST);
                GetFieldValues(ControlsEnum.CURRENCY);
                SetFieldValues(ControlsEnum.CURRENCY);
                GetFieldValues(ControlsEnum.EXCHANGERATE);
                SetFieldValues(ControlsEnum.EXCHANGERATE);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        private void GetFieldValues(ControlsEnum type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    #region EMPLOYEELOANLIST
                    case ControlsEnum.EMPLOANLIST:
                        dsEmpLoanList = LoansAndAdvancesBL.GetLoansAndAdvancesList(
                            new BusinessObject.GridPrams()
                            {
                                SortBy = string.IsNullOrEmpty(SortBy) ? Fields.F_ELM_APPLY_DATE : SortBy,
                                SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                                PageNumber = Convert.ToInt32(PageIndex),
                                PageSize = grdEmpLoanList.PageSize
                                //FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? string.Empty : txtFromDate.Text.Trim(),
                                //ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? string.Empty : txtToDate.Text.Trim(),
                                //SearchBy = "ELM_EMPLOYEE",
                                //SearchValue = string.IsNullOrEmpty(txtInvoiceNumber.Text.Trim()) ? string.Empty : (txtInvoiceNumber.Text.Trim() == "Select/Type" ? string.Empty : txtInvoiceNumber.Text.Trim())
                            }

                            , (ddlBranchLocation.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlBranchLocation.SelectedValue) : 0)
                            , (ddlEmploymentType.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlEmploymentType.SelectedValue) : 0)
                            , (ddlFilterItem.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlFilterItem.SelectedValue) : 0)
                            , Convert.ToInt16(hdfEmployeeSrch.Value)
                            );
                        if (dsEmpLoanList != null)
                        {
                            DataView dvEmpLoanLst = dsEmpLoanList.Tables[0].DefaultView;
                            dtEmpLoanList = dvEmpLoanLst.ToTable();
                        }
                        break;
                    #endregion
                    #region EMPLOYEELOAN HDR / DETAIL
                    case ControlsEnum.EMPLOYEELOANINFO:
                        objEmpLoanHeader = LoansAndAdvancesBL.GetEmpLoanDetails(CurrPK);
                        if (objEmpLoanHeader == null && CurrPK != 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region LOAN / ADVANCE ITEMS
                    case ControlsEnum.LOANADVANCEITEM:

                        dtResult = BusinessLogic.HRMS.Admin.Masters.PayElementsMasterBL.GetParentElement(pelPK, pelPK > 0 ? Convert.ToInt32(DbActiveStatus.HASPK) : Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0, 1, -1);
                        break;
                    #endregion
                    #region LOAN / ADVANCE TYPE
                    case ControlsEnum.LOANADVANCETYPE:
                        int payelementPK = 0;
                        payelementPK = Convert.ToInt32(ddlItem.SelectedValue) > 0 ? Convert.ToInt32(ddlItem.SelectedValue) : 0;
                        dtResult = LoansAndAdvancesBL.GetLoanAdvanceItemType(0, Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, payelementPK);
                        break;
                    #endregion
                    #region SHOW INSTALLMENTS
                    case ControlsEnum.SHOWINSTALLMENTS:
                        if (string.IsNullOrEmpty(txtPrincipalAmount.Text))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Amount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            txtPrincipalAmount.Focus();
                        }
                        else if (string.IsNullOrEmpty(txtEffectiveDate.Text))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_EffectiveDate").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            txtEffectiveDate.Focus();
                        }
                        else if (string.IsNullOrEmpty(txtInstallments.Text))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_InstallmentCount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            txtInstallments.Focus();
                        }
                        else if (int.Parse(hdfEmployee.Value)<=0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Employee").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            txtEmployee.Focus();
                        }
                        else
                        {
                            if (CurrPK > 0)
                            {
                                #region Edit Mode
                                TotalPrincipalAmt = Convert.ToDouble(txtPrincipalAmount.Text) + InterestAmount;
                                txtTotalAmt.Text = GetFormattedCurrency(TotalPrincipalAmt.ToString(hdfCurrencyFormat.Value));
                                List<EmpLoanDetails> objLoanTemList = empLoanDetailList;
                                if (objLoanTemList != null)
                                {
                                    int installmentDiff = 0;
                                    int totInstallments = 0;
                                    double installmentAmnt = 0;
                                    int.TryParse(txtInstallments.Text, out totInstallments);
                                    double.TryParse(txtInstallmentAmt.Text, out installmentAmnt);

                                    if (installmentAmnt > 0) // Installment Calculation from Installment Amount
                                    {
                                        if (totInstallments < objLoanTemList.Where(r => r.ELS_PAID_AMT != 0).Count())
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Err_Installments").ToString();
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, objLoanTemList.Where(r => r.ELS_PAID_AMT != 0).Count().ToString());
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                            return;
                                        }
                                        double installments = Math.Ceiling((TotalPrincipalAmt - objLoanTemList.Sum(r => r.ELS_PAID_AMT)) / installmentAmnt);
                                        txtInstallments.Text = (installments + objLoanTemList.Where(r => r.ELS_PAID_AMT > 0).Count()).ToString();
                                        totInstallments = Convert.ToInt32(txtInstallments.Text);
                                    }

                                    if (totInstallments < objLoanTemList.Where(r => r.ELS_PAID_AMT != 0).Count())
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Err_Installments").ToString();
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, objLoanTemList.Where(r => r.ELS_PAID_AMT != 0).Count().ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                        return;
                                    }
                                    else if (totInstallments < objLoanTemList.Count) // need to remove rows from list
                                    {
                                        installmentDiff = objLoanTemList.Count - totInstallments;
                                        List<EmpLoanDetails> objLoanList = objLoanTemList.Where(r => r.ELS_PAID_AMT == 0).OrderByDescending(s => s.SlNo).Take(installmentDiff).ToList();
                                        foreach (EmpLoanDetails objLoanDet in objLoanList)
                                        {
                                            objLoanTemList.Remove(objLoanDet);
                                        }
                                    }
                                    else if (totInstallments > objLoanTemList.Count) // need to add rows to list
                                    {
                                        installmentDiff = totInstallments - objLoanTemList.Count;
                                        int slNo = 1;
                                        DateTime maxDate = Convert.ToDateTime(txtEffectiveDate.Text);
                                        if (objLoanTemList.Count() > 0)
                                        {
                                            slNo = objLoanTemList.Max(r => r.SlNo) + 1;
                                            maxDate = objLoanTemList.Max(r => r.ELS_INST_DATE).AddMonths(1);
                                        }
                                        for (int i = 1; i <= installmentDiff; i++)
                                        {
                                            objEmpLoanDetails = new EmpLoanDetails();
                                            objEmpLoanDetails.SlNo = slNo;
                                            objEmpLoanDetails.ELS_INST_DATE = maxDate;
                                            objEmpLoanDetails.ELS_INST_AMT = installmentAmnt;
                                            objLoanTemList.Add(objEmpLoanDetails);
                                            slNo++;
                                            maxDate = maxDate.AddMonths(1);
                                        }
                                    }

                                    if (installmentAmnt > 0) // Installment Calculation from Installment Amount
                                    {

                                        //txtInstallments.Text = Math.Ceiling(TotalPrincipalAmt / installmentAmnt).ToString();
                                        //totInstallments = Convert.ToInt32(txtInstallments.Text);
                                        double amount = TotalPrincipalAmt - objLoanTemList.Sum(r => r.ELS_PAID_AMT);
                                        DateTime maxDate = Convert.ToDateTime(txtEffectiveDate.Text);
                                        foreach (EmpLoanDetails objLnDet in objLoanTemList)
                                        {
                                            if (objLnDet.ELS_PAID_AMT == 0)
                                            {
                                                objLnDet.ELS_INST_DATE = maxDate;
                                                objLnDet.ELS_INST_AMT = (amount > installmentAmnt) ? installmentAmnt : amount;
                                                amount = amount - installmentAmnt;
                                            }

                                            maxDate = maxDate.AddMonths(1);
                                        }
                                    }
                                    else  // Installment Calculation from No. of Installments
                                    {
                                        totInstallments = Convert.ToInt32(txtInstallments.Text);
                                        TotalPrincipalAmt = TotalPrincipalAmt - objLoanTemList.Sum(r => r.ELS_PAID_AMT);
                                        totInstallments = totInstallments - objLoanTemList.Where(r => r.ELS_PAID_AMT > 0).Count();
                                        double lastInstAmt = TotalPrincipalAmt % totInstallments;
                                        int count = 1;
                                        DateTime maxDate = Convert.ToDateTime(txtEffectiveDate.Text);
                                        foreach (EmpLoanDetails objLnDet in objLoanTemList)
                                        {
                                            if (objLnDet.ELS_PAID_AMT == 0)
                                            {
                                                objLnDet.ELS_INST_DATE = maxDate;
                                                objLnDet.ELS_INST_AMT = Math.Floor(TotalPrincipalAmt / totInstallments);
                                                if (count == objLoanTemList.Count) // For Last Installment
                                                {
                                                    objLnDet.ELS_INST_AMT = lastInstAmt == 0 ? TotalPrincipalAmt / totInstallments : Math.Floor(TotalPrincipalAmt / totInstallments) + lastInstAmt;
                                                }
                                            }
                                            count++;
                                            maxDate = maxDate.AddMonths(1);
                                        }

                                    }

                                    empLoanDetailList = objLoanTemList;

                                }
                                #endregion
                            }
                            else
                            {
                                #region New Mode
                                empLoanDetailList = new List<EmpLoanDetails>();
                                int totInstallments = 0;
                                double instAmt = 1;
                                double lastInstAmt = 0;
                                double amount = 0;

                                DateTime NextInstMonth = Convert.ToDateTime(txtEffectiveDate.Text);
                                TotalPrincipalAmt = Convert.ToDouble(txtPrincipalAmount.Text) + InterestAmount;
                                txtTotalAmt.Text = GetFormattedCurrency(TotalPrincipalAmt.ToString(hdfCurrencyFormat.Value));
                                if (!string.IsNullOrEmpty(txtInstallmentAmt.Text) && (txtInstallmentAmt.Text != "0") && (txtInstallmentAmt.Text != "0.00")) // Installment Calculation from Installment Amount
                                {
                                    double.TryParse(txtInstallmentAmt.Text, out instAmt);
                                    txtInstallments.Text = Math.Ceiling(TotalPrincipalAmt / instAmt).ToString();
                                    totInstallments = Convert.ToInt32(txtInstallments.Text);
                                    amount = TotalPrincipalAmt;
                                    for (int inst = 0; inst < totInstallments; inst++)
                                    {
                                        objEmpLoanDetails = new EmpLoanDetails();
                                        objEmpLoanDetails.SlNo = inst + 1;
                                        objEmpLoanDetails.ELS_INST_DATE = NextInstMonth.AddMonths(inst);
                                        objEmpLoanDetails.ELS_INST_AMT = (amount > instAmt) ? instAmt : amount;
                                        empLoanDetailList.Add(objEmpLoanDetails);
                                        amount = amount - instAmt;
                                    }
                                }
                                else  // Installment Calculation from No. of Installments
                                {
                                    totInstallments = Convert.ToInt32(txtInstallments.Text);
                                    lastInstAmt = TotalPrincipalAmt % totInstallments;
                                    for (int inst = 0; inst < totInstallments; inst++)
                                    {
                                        objEmpLoanDetails = new EmpLoanDetails();
                                        objEmpLoanDetails.SlNo = inst + 1;
                                        objEmpLoanDetails.ELS_INST_DATE = NextInstMonth.AddMonths(inst);
                                        objEmpLoanDetails.ELS_INST_AMT = Math.Floor(TotalPrincipalAmt / totInstallments);
                                        if (inst + 1 == totInstallments) // For Last Installment
                                        {
                                            objEmpLoanDetails.ELS_INST_AMT = lastInstAmt == 0 ? TotalPrincipalAmt / totInstallments : Math.Floor(TotalPrincipalAmt / totInstallments) + lastInstAmt;
                                        }
                                        empLoanDetailList.Add(objEmpLoanDetails);
                                    }
                                }
                                #endregion
                            }
                        }
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
                        dtResult = LoansAndAdvancesBL.GetBranchLocation();
                        break;
                    #endregion
                    #region DETAILS
                    case ControlsEnum.DETAILS:
                        dtResult = LoansAndAdvancesBL.GetEMPLoanHistory(Convert.ToInt32(hdfEmployee.Value), currentUser.SBUID);
                        break; 
                    #endregion
                    #region EXCHANGE RATE
                    case ControlsEnum.EXCHANGERATE:
                        if (!string.IsNullOrEmpty(hdfCurrency.Value) && Convert.ToInt32(hdfCurrency.Value) > 0)
                        {
                            DateTime Date = DateTime.Now;
                            DateTime.TryParse((txtApplyDate.Text.Trim() == string.Empty ? DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort) : txtApplyDate.Text.Trim()), out Date);
                            dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(Convert.ToInt32(hdfCurrency.Value), currentUser.BaseCurrency, Date);
                        }
                        break;
                    #endregion
                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        dtResult = CommonBL.GetCurrency(currentUser.BaseCurrency, currentUser.SBUID, (int)DbActiveStatus.HASPK);
                        break;
                    #endregion
                    #region EMP CURRENCY
                    case ControlsEnum.EMPCURRENCY:
                        this.selectedEmployeePayDetails = BusinessLogic.HRMS.Employee.EmployeePayDetailsBL.GetEmployeePayDetailsByID(Convert.ToInt32(hdfEmployee.Value), (int)DbActiveStatus.ACTIVE);
                        EmpResignedDate = this.selectedEmployeePayDetails.EPD_RESIGNED_DATE;
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(BusinessObject.CommonManagement.DbActiveStatus.ACTIVE), 0, CompanyPk);
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

        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region EMPLOANLIST
                    case ControlsEnum.EMPLOANLIST:
                        BindGrid(controlType);
                        dsEmpLoanList.Clear();
                        break; 
                    #endregion
                    #region LOAN ADVANCE ITEM
                    case ControlsEnum.LOANADVANCEITEM:
                        BindDropDown(ControlsEnum.LOANADVANCEITEM);
                        break; 
                    #endregion
                    #region LOAN ADVANCE FILTER ITEM
                    case ControlsEnum.LOANADVANCEFILTERITEM:
                        BindDropDown(ControlsEnum.LOANADVANCEFILTERITEM);
                        break; 
                    #endregion
                    #region LOAN ADVANCE TYPE
                    case ControlsEnum.LOANADVANCETYPE:
                        BindDropDown(ControlsEnum.LOANADVANCETYPE);
                        break; 
                    #endregion
                    #region SHOW INSTALLMENTS
                    case ControlsEnum.SHOWINSTALLMENTS:
                        BindGrid(controlType);
                        break; 
                    #endregion
                    #region EMPLOYEE LOAN INFO
                    case ControlsEnum.EMPLOYEELOANINFO:
                        GetUIValuesFromObject(controlType);
                        break; 
                    #endregion
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
                    #region DETAILS
                    case ControlsEnum.DETAILS:
                        BindGrid(ControlsEnum.DETAILS);
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
                    #region EMP CURRENCY
                    case ControlsEnum.EMPCURRENCY:
                        GetUIValuesFromObject(ControlsEnum.EMPCURRENCY);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    #endregion

                    #region TYPECHANGE
                    case ControlsEnum.TYPECHANGE:
                        GetUIValuesFromObject(ControlsEnum.TYPECHANGE);
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

                #region LOAN / ADVANCE ITEMS
                case ControlsEnum.LOANADVANCEITEM:
                    ddlItem.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlItem.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, Fields.F_PEL_CODE);
                        ddlItem.DataTextField = Fields.F_PEL_NAME;
                        ddlItem.DataValueField = Fields.F_PEL_PK;
                        ddlItem.DataBind();
                    }
                    ddlItem.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region LOAN / ADVANCE ITEMS(Filter)
                case ControlsEnum.LOANADVANCEFILTERITEM:
                    ddlFilterItem.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlFilterItem.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, Fields.F_PEL_CODE);
                        ddlFilterItem.DataTextField = Fields.F_PEL_NAME;
                        ddlFilterItem.DataValueField = Fields.F_PEL_PK;
                        ddlFilterItem.DataBind();
                    }
                    ddlFilterItem.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region LOAN / ADVANCE TYPE
                case ControlsEnum.LOANADVANCETYPE:
                    ddlItemType.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlItemType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, Fields.F_ELT_NAME);
                        ddlItemType.DataTextField = Fields.F_ELT_NAME;
                        ddlItemType.DataValueField = Fields.F_ELT_PK;
                        ddlItemType.DataBind();
                    }
                    ddlItemType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region EMPLOYMENTTYPE
                case ControlsEnum.EMPLOYMENTTYPE:
                    ddlEmploymentType.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlEmploymentType.DataSource = dtResult;
                        ddlEmploymentType.DataTextField = GTIService.Constants.HRMS.Employee.Fields.ADM_CNST_TEXT;
                        ddlEmploymentType.DataValueField = GTIService.Constants.HRMS.Employee.Fields.ADM_CNST_VALUE;
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
                        ddlBranchLocation.DataTextField = GTIService.Constants.HRMS.Employee.Fields.ADM_CNST_TEXT;
                        ddlBranchLocation.DataValueField = GTIService.Constants.HRMS.Employee.Fields.ADM_CNST_VALUE;
                        ddlBranchLocation.DataBind();
                    }
                    ddlBranchLocation.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region COMPANY
                case ControlsEnum.COMPANY:
                    ddlCompany.DataTextField = Resources.DataFieldRes.CMP_NAME;
                    ddlCompany.DataValueField = Resources.DataFieldRes.CMP_PK;
                    ddlCompany.DataSource = dtCompany;
                    ddlCompany.DataBind();
                    ddlCompany.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    ddlCompany.Items.HtmlDecode();
                    if (dtCompany != null && dtCompany.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK])))
                        ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK].ToString()));

                    break;
                #endregion
                default:
                    break;
            }
        }

        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                Label lblFooterTotalInstAmt;
                switch (controlType)
                {
                    case ControlsEnum.EMPLOANLIST:
                        uclPaging.Visible = false;
                        if (dtEmpLoanList != null && dtEmpLoanList.Rows.Count > 0)
                        {
                            int rowCount = 0;

                            rowCount = Convert.ToInt32(dtEmpLoanList.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                            this.TotalPages = Convert.ToInt32(dtEmpLoanList.Rows[0]["ROW_NO"].ToString());

                            uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                              (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                              (rowCount / this.PageSize) + 1;
                            PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdEmpLoanList.DataSource = dtEmpLoanList;
                            grdEmpLoanList.DataBind();

                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            grdEmpLoanList.DataSource = null;
                            grdEmpLoanList.DataBind();
                        }
                        break;

                    case ControlsEnum.SHOWINSTALLMENTS:

                        if (EmpResignedDate != null)
                        {
                            DateTime? LastInstDate = empLoanDetailList.Max(x => x.ELS_INST_DATE);
                            if (LastInstDate > EmpResignedDate)
                            {

                                litErrorMsg.Text = GetLocalResourceObject("Err_EmpResignedDate").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Convert.ToDateTime(EmpResignedDate).ToString(Resources.Constants.HRMSDateFormatShort));
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                txtInstallments.Focus();
                                return;
                            }
                        }
                       if (empLoanDetailList != null)
                        {
                            grdLoanSlabDetails.DataSource = empLoanDetailList;
                            grdLoanSlabDetails.DataBind();
                            if (grdLoanSlabDetails.FooterRow != null)
                            {
                                lblFooterTotalInstAmt = grdLoanSlabDetails.FooterRow.FindControl("lblFooterTotalInstAmt") as Label;
                                lblFooterTotalInstAmt.Text = GetFormattedCurrencyWithComma(empLoanDetailList.Sum(tot => tot.ELS_INST_AMT).ToString(hdfCurrencyFormatWithComma.Value));
                                Label lblFooterTotalPaidAmt = grdLoanSlabDetails.FooterRow.FindControl("lblFooterTotalPaidAmt") as Label;
                                lblFooterTotalPaidAmt.Text = GetFormattedCurrencyWithComma(empLoanDetailList.Sum(tot => tot.ELS_PAID_AMT).ToString(hdfCurrencyFormatWithComma.Value));
                            }
                        }
                        else
                        {
                            grdLoanSlabDetails.DataSource = null;
                            grdLoanSlabDetails.DataBind();
                        }
                        break;
                    default:
                        break;

                    case ControlsEnum.DETAILS:
                        grdStatusHistory.DataSource = dtResult;
                        grdStatusHistory.DataBind();
                        break;
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
            try
            {
                switch (controlType)
                {
                    #region Employee Loan / Advance HDR
                    case ControlsEnum.EMPLOANHEADER:
                        objEmpLoanHeader.ELM_PK = CurrPK;
                        objEmpLoanHeader.ELM_NO = lblTrxNo.Text;
                        objEmpLoanHeader.ELM_EMPLOYEE = Convert.ToInt32(hdfEmployee.Value);
                        objEmpLoanHeader.ELM_PAY_ELEMENT = Convert.ToInt32(ddlItem.SelectedValue);
                        //  objEmpLoanHeader.ELM_LOAN_TYPE = 0;
                        objEmpLoanHeader.ELM_APPLY_DATE = string.IsNullOrEmpty(txtApplyDate.Text) ? string.Empty : txtApplyDate.Text;
                        objEmpLoanHeader.ELM_APPROVED_DATE = string.IsNullOrEmpty(txtApprovedDate.Text) ? string.Empty : txtApprovedDate.Text;
                        objEmpLoanHeader.ELM_EFFECT_DATE = string.IsNullOrEmpty(txtEffectiveDate.Text) ? string.Empty : txtEffectiveDate.Text;
                        objEmpLoanHeader.ELM_PRINCIPAL_AMT = string.IsNullOrEmpty(txtPrincipalAmount.Text.Trim()) ? 0 : Convert.ToDouble(txtPrincipalAmount.Text.Trim());
                        objEmpLoanHeader.ELM_INST_COUNT = string.IsNullOrEmpty(txtInstallments.Text.Trim()) ? 0 : Convert.ToInt32(txtInstallments.Text.Trim());
                        objEmpLoanHeader.ELM_INST_AMT = string.IsNullOrEmpty(txtInstallmentAmt.Text.Trim()) ? 0 : Convert.ToDouble(txtInstallmentAmt.Text.Trim());
                        objEmpLoanHeader.ELM_ROI = string.IsNullOrEmpty(txtRateofInterest.Text.Trim()) ? 0 : Convert.ToDouble(txtRateofInterest.Text.Trim());
                        objEmpLoanHeader.ELM_CURRENCY = Convert.ToInt32(hdfCurrency.Value);
                        objEmpLoanHeader.ELM_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                        objEmpLoanHeader.ELM_EXCHG_RATE = string.IsNullOrEmpty(txtExchangeRate.Text.Trim()) ? 1 : Convert.ToDouble(txtExchangeRate.Text.Trim());
                        objEmpLoanHeader.ELM_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        objEmpLoanHeader.BIZUNIT_PK = Convert.ToInt16(currentUser.SBUID);
                        objEmpLoanHeader.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        objEmpLoanHeader.USER_PK = Convert.ToInt16(currentUser.PKUser);
                        objEmpLoanHeader.LAST_MOD_DT = LastModifiedTime;
                        objEmpLoanHeader.ELM_DESC = txtDescription.Text;
                        objEmpLoanHeader.ELM_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                        SetUIValuesToObject(ControlsEnum.SHOWINSTALLMENTS);
                        SetFieldValues(ControlsEnum.SHOWINSTALLMENTS);
                        // GetFieldValues(ControlsEnum.SHOWINSTALLMENTS);
                        objEmpLoanHeader.LoanSlabDetails = empLoanDetailList;
                        retObject = objEmpLoanHeader;
                        break;
                    #endregion
                    #region SHOW INSTALLMENTS
                    case ControlsEnum.SHOWINSTALLMENTS:
                        if (string.IsNullOrEmpty(txtPrincipalAmount.Text))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Amount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            txtPrincipalAmount.Focus();
                        }
                        else if (string.IsNullOrEmpty(txtEffectiveDate.Text))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_EffectiveDate").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            txtEffectiveDate.Focus();
                        }
                        else if (string.IsNullOrEmpty(txtInstallments.Text))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_InstallmentCount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            txtInstallments.Focus();
                        }
                        else
                        {
                            empLoanDetailList = new List<EmpLoanDetails>();


                            foreach (GridViewRow grdRow in grdLoanSlabDetails.Rows)
                            {
                                Label lblSlNo = (Label)grdRow.FindControl("lblSlNo");
                                TextBox txtInstallmentDate = (TextBox)grdRow.FindControl("txtInstallmentDate");
                                TextBox txtInstAmt = (TextBox)grdRow.FindControl("txtInstAmt");
                                Label lblPaidAmt = (Label)grdRow.FindControl("lblPaidAmt");
                                HiddenField hdfELS_PK = (HiddenField)grdRow.FindControl("hdfELS_PK");

                                objEmpLoanDetails = new EmpLoanDetails();
                                objEmpLoanDetails.SlNo = Convert.ToInt32(lblSlNo.Text);
                                objEmpLoanDetails.ELS_PK = Convert.ToInt32(hdfELS_PK.Value);
                                objEmpLoanDetails.ELS_INST_DATE = Convert.ToDateTime(txtInstallmentDate.Text);
                                objEmpLoanDetails.ELS_INST_AMT = txtInstAmt.Text == string.Empty ? 0 : Convert.ToDouble(txtInstAmt.Text);
                                objEmpLoanDetails.ELS_PAID_AMT = lblPaidAmt.Text == string.Empty ? 0 : Convert.ToDouble(lblPaidAmt.Text);
                                empLoanDetailList.Add(objEmpLoanDetails);
                            }

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
                    #region EMPLOYEE LOAN INFO
                    case ControlsEnum.EMPLOYEELOANINFO:
                        if (objEmpLoanHeader != null)
                        {
                            //ddlEmployee.SelectedValue = objEmpLoanHeader.ELM_EMPLOYEE.ToString();
                            lblTrxNo.Text = string.IsNullOrEmpty(objEmpLoanHeader.ELM_NO) ? Resources.ErpRes.Draft : objEmpLoanHeader.ELM_NO;
                            hdfEmployee.Value = objEmpLoanHeader.ELM_EMPLOYEE.ToString();
                            txtEmployee.Text = HttpUtility.HtmlDecode(objEmpLoanHeader.ELM_EMP_TEXT);
                            ddlItem.SelectedValue = objEmpLoanHeader.ELM_PAY_ELEMENT.ToString();
                            // GetFieldValues(ControlsEnum.LOANADVANCETYPE);
                            // SetFieldValues(ControlsEnum.LOANADVANCETYPE);
                            // ddlItemType.SelectedValue = objEmpLoanHeader.ELM_LOAN_TYPE.ToString();
                            txtApplyDate.Text = string.IsNullOrEmpty(objEmpLoanHeader.ELM_APPLY_DATE) ? string.Empty : Convert.ToDateTime(objEmpLoanHeader.ELM_APPLY_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            txtApprovedDate.Text = string.IsNullOrEmpty(objEmpLoanHeader.ELM_APPROVED_DATE) ? string.Empty : Convert.ToDateTime(objEmpLoanHeader.ELM_APPROVED_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            txtEffectiveDate.Text = string.IsNullOrEmpty(objEmpLoanHeader.ELM_EFFECT_DATE) ? string.Empty : Convert.ToDateTime(objEmpLoanHeader.ELM_EFFECT_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            txtPrincipalAmount.Text = GetFormattedCurrency(objEmpLoanHeader.ELM_PRINCIPAL_AMT.ToString());
                            txtInstallments.Text = objEmpLoanHeader.ELM_INST_COUNT.ToString();
                            txtInstallmentAmt.Text = GetFormattedCurrency(objEmpLoanHeader.ELM_INST_AMT.ToString());
                            txtRateofInterest.Text = GetFormattedCurrency(objEmpLoanHeader.ELM_ROI.ToString());
                            LastModifiedTime = objEmpLoanHeader.LAST_MOD_DT;
                            //TotalPrincipalAmt = CalculateTotalPrincipal(); Gridview coulmn can edit
                            txtTotalAmt.Text = txtTotalAmt.Text = GetFormattedCurrency(objEmpLoanHeader.LoanSlabDetails.Sum(tot => tot.ELS_INST_AMT).ToString(hdfCurrencyFormat.Value));
                            txtCurrency.Text = string.Format(GetLocalResourceObject("CurrencyDisplayFormat").ToString(), Convert.ToString(objEmpLoanHeader.ELM_CURRENCY_CODE_TEXT), HttpUtility.HtmlDecode(objEmpLoanHeader.ELM_CURRENCY_NAME_TEXT));
                            hdfCurrency.Value = objEmpLoanHeader.ELM_CURRENCY.ToString();
                            txtExchangeRate.Text = GetFormattedExchangerate(objEmpLoanHeader.ELM_EXCHG_RATE);

                            txtDescription.Text = objEmpLoanHeader.ELM_DESC;
                            empLoanDetailList = objEmpLoanHeader.LoanSlabDetails;
                            CompanyPk = objEmpLoanHeader.ELM_COMPANY;
                            GetFieldValues(ControlsEnum.COMPANY);
                            SetFieldValues(ControlsEnum.COMPANY);
                            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(CompanyPk.ToString()));

                        }
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
                    #region EMP CURRENCY
                    case ControlsEnum.EMPCURRENCY:
                        if (selectedEmployeePayDetails != null)
                        {
                            txtCurrency.Text = selectedEmployeePayDetails.EPD_CURRENCY_TEXT;
                            hdfCurrency.Value = Convert.ToString(selectedEmployeePayDetails.EPD_CURRENCY);
                        }
                        break;
                    #endregion
                    #region TYPECHANGE 
                    case ControlsEnum.TYPECHANGE:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            if (Convert.ToInt32(dtResult.Rows[0]["PEL_CLASS"].ToString()) == Convert.ToInt32(PelClassEnum.Advances))
                            {
                                txtInstallments.Text = CommonConstants.SELECT_VALUE_ONE;
                                txtRateofInterest.Text = CommonConstants.SELECT_VALUE_ZERO;
                                hdfIsAdvances.Value = CommonConstants.SELECT_VALUE_ONE;
                            }
                            else
                            {
                                txtInstallments.Text = string.Empty;
                                txtRateofInterest.Text = string.Empty;
                                hdfIsAdvances.Value = CommonConstants.SELECT_VALUE_ZERO;
                            }
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
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region EMP LOAN LIST
                case ControlsEnum.EMPLOANLIST:
                    lblTrxNo.Text = Resources.ErpRes.Draft;
                    PageIndex = 1;
                    CurrPK = 0;
                    //ddlEmployee.ClearSelection();
                    hdfEmployee.Value = "0";
                    txtEmployee.Text = string.Empty;
                    ddlItem.ClearSelection();
                    ddlItemType.ClearSelection();
                    txtApplyDate.Text = string.Empty;
                    txtApprovedDate.Text = string.Empty;
                    txtEffectiveDate.Text = string.Empty;
                    txtPrincipalAmount.Text = string.Empty;
                    txtInstallments.Text = string.Empty;
                    txtInstallmentAmt.Text = string.Empty;
                    txtRateofInterest.Text = string.Empty;
                    grdLoanSlabDetails.DataSource = null;
                    grdLoanSlabDetails.DataBind();
                    empLoanDetailList = null;
                    ddlEmploymentType.ClearSelection();
                    ddlBranchLocation.ClearSelection();
                    ddlFilterItem.ClearSelection();
                    txtTotalAmt.Text = string.Empty;
                    TotalPrincipalAmt = 0;
                    InterestAmount = 0;
                    txtDescription.Text = string.Empty;
                    txtEmployeeSrch.Text = string.Empty;
                    hdfEmployeeSrch.Value = "0";
                    pelPK = 0;
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

        private double CalculateTotalPrincipal()
        {
            double result = 0;
            try
            {
                double RateofInterest = 0;
                double PrincipalAmt = 0;
                double Intallments = 0;
                //double IntrAmtPerYear = 0;
                double IntrAmtPerMonth = 0;
                double totAmount = 0;
                double.TryParse(txtRateofInterest.Text, out RateofInterest);
                double.TryParse(txtPrincipalAmount.Text, out PrincipalAmt);
                double.TryParse(txtInstallments.Text, out Intallments);
                //IntrAmtPerYear = (RateofInterest * PrincipalAmt) / 100;
                IntrAmtPerMonth = ((RateofInterest * PrincipalAmt) / 100) / 12;
                InterestAmount = IntrAmtPerMonth * Intallments;
                totAmount = PrincipalAmt + InterestAmount;
                result = totAmount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public string GetFormattedExchangerate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfExchangeRateFormat.Value);
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
            //To check if department is different by opening in new tab
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            try
            {
                int? result;
                result = 0;
                bool bIsChecked = false;

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
                    if (((DropDownList)sender).ID == "ddlItem")
                    {
                        commonActions = ActionsEnum.ITEMCHANGED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    if (((TextBox)sender).ID == "txtRateofInterest")
                    {
                        commonActions = ActionsEnum.CALCULATEINTERESTAMT;
                    }
                }

                switch (commonActions)
                {
                    #region NEW
                    case ActionsEnum.NEW:
                        EntryStatus = EntryStatus.NEWMODE;
                        ResetForm(ControlsEnum.EMPLOANLIST);
                        GetFieldValues(ControlsEnum.COMPANY);
                        SetFieldValues(ControlsEnum.COMPANY);
                        GetFieldValues(ControlsEnum.CURRENCY);
                        SetFieldValues(ControlsEnum.CURRENCY);
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        SetFieldValues(ControlsEnum.EXCHANGERATE);
                        //GetFieldValues(ControlsEnum.EMPLOYEES);
                        //SetFieldValues(ControlsEnum.EMPLOYEES);
                        GetFieldValues(ControlsEnum.LOANADVANCEITEM);
                        SetFieldValues(ControlsEnum.LOANADVANCEITEM);
                        // GetFieldValues(ControlsEnum.LOANADVANCETYPE);
                        // SetFieldValues(ControlsEnum.LOANADVANCETYPE);
                        EnableDisableControls(true);
                        txtEmployee.Focus();
                        break;
                    #endregion

                    #region ITEM CHANGED
                    case ActionsEnum.ITEMCHANGED:
                        pelPK =  Convert.ToInt32(ddlItem.SelectedValue) > 0 ? Convert.ToInt32(ddlItem.SelectedValue) : 0;
                        GetFieldValues(ControlsEnum.LOANADVANCEITEM);
                        SetFieldValues(ControlsEnum.TYPECHANGE);
                        break;
                    #endregion

                    #region SHOW INSTALLMENTS
                    case ActionsEnum.SHOWINSTALLMENTS:
                        GetFieldValues(ControlsEnum.SHOWINSTALLMENTS);
                        SetFieldValues(ControlsEnum.SHOWINSTALLMENTS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowInstallment", "ShowHideInstallmentDetails(1);", true);
                        break;
                    #endregion

                    #region SAVE
                    case ActionsEnum.SAVE:
                        // Label lblFooterTotalInstAmt = grdLoanSlabDetails.FooterRow.FindControl("lblFooterTotalInstAmt") as Label;

                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {


                            objEmpLoanHeader = new EmpLoanHeader();
                            objEmpLoanHeader = (EmpLoanHeader)SetUIValuesToObject(ControlsEnum.EMPLOANHEADER);
                            if (objEmpLoanHeader.LoanSlabDetails.Count == 0)
                            {
                                litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordsForSave")).ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                                return;
                            }
                            if (objEmpLoanHeader != null)
                            {
                                if (Convert.ToDouble(txtTotalAmt.Text) != Convert.ToDouble(objEmpLoanHeader.LoanSlabDetails.Sum(tot => tot.ELS_INST_AMT)))
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_TotalAmount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                    txtTotalAmt.Focus();
                                    break;
                                }
                                else if (Convert.ToDouble(txtInstallments.Text) != Convert.ToDouble(grdLoanSlabDetails.Rows.Count))
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_TotalInstallment").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                    txtInstallments.Focus();
                                    break;
                                }
                                string TrxNo = string.Empty;
                                objEmpLoanHeader.WKF_FLAG = 1;
                                string xmlDoc = CommonFunctions.XmlSerialize<EmpLoanHeader>(objEmpLoanHeader);
                                result = LoansAndAdvancesBL.SaveLoanAdvanceHeader(xmlDoc, out TrxNo);
                                if (result > 0)
                                {
                                    // Show Save Message and redired to listing page                                        
                                    //litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    //litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.LoansAndAdvances);
                                    lblTrxNo.Text = TrxNo;
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                                    object[] args = new object[2];
                                    args[0] = Resources.PageNameRes.LoansAndAdvances;
                                    args[1] = TrxNo;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.EMPLOANLIST);
                                    GetFieldValues(ControlsEnum.EMPLOANLIST);
                                    SetFieldValues(ControlsEnum.EMPLOANLIST);
                                }
                                else
                                {
                                    if (result == (int)DbSaveStatus.SQLERROR)
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.Captions.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CONCURRENCY)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.LoansAndAdvances + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.Captions.Information + "');", true);
                                    }
                                    else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.LoansAndAdvances + " " + Resources.Messages.AlreadyDeleted;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.Captions.Information + "');", true);
                                        EntryStatus = EntryStatus.LISTMODE;
                                    }
                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.LoansAndAdvances + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.Captions.Information + "','" + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.LoansAndAdvances);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.Captions.Information + "');", true);
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region EDIT
                    case ActionsEnum.EDIT:
                        foreach (GridViewRow grdrow in grdEmpLoanList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEmpLoanPK")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            EnableDisableControls(false);
                            SetUIEditView(commonActions);
                            txtEmployee.Focus();
                            GetFieldValues(ControlsEnum.LOANADVANCEITEM);
                            SetFieldValues(ControlsEnum.LOANADVANCEITEM);
                            GetFieldValues(ControlsEnum.EMPLOYEELOANINFO);
                            SetFieldValues(ControlsEnum.EMPLOYEELOANINFO);
                            SetFieldValues(ControlsEnum.SHOWINSTALLMENTS);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region VIEW
                    case ActionsEnum.VIEW:
                        foreach (GridViewRow grdrow in grdEmpLoanList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEmpLoanPK")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            EnableDisableControls(false);
                            SetUIEditView(commonActions);
                            //GetFieldValues(ControlsEnum.EMPLOYEES);
                            //SetFieldValues(ControlsEnum.EMPLOYEES);
                            GetFieldValues(ControlsEnum.LOANADVANCEITEM);
                            SetFieldValues(ControlsEnum.LOANADVANCEITEM);
                            GetFieldValues(ControlsEnum.EMPLOYEELOANINFO);
                            SetFieldValues(ControlsEnum.EMPLOYEELOANINFO);
                            SetFieldValues(ControlsEnum.SHOWINSTALLMENTS);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region DETAIL
                    case ActionsEnum.DETAIL:
                        foreach (GridViewRow grdrow in grdEmpLoanList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEmpLoanPK")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            EnableDisableControls(false);
                            SetUIEditView(commonActions);
                            //GetFieldValues(ControlsEnum.EMPLOYEES);
                            //SetFieldValues(ControlsEnum.EMPLOYEES);
                            GetFieldValues(ControlsEnum.LOANADVANCEITEM);
                            SetFieldValues(ControlsEnum.LOANADVANCEITEM);
                            GetFieldValues(ControlsEnum.EMPLOYEELOANINFO);
                            SetFieldValues(ControlsEnum.EMPLOYEELOANINFO);
                            SetFieldValues(ControlsEnum.SHOWINSTALLMENTS);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region DETAILS
                    case ActionsEnum.DETAILS:
                        if (((ImageButton)sender).ID == "imbHistoryDetailsPopup")
                        {
                            if (hdfEmployee.Value != "0")
                            {
                                GetFieldValues(ControlsEnum.DETAILS);
                                SetFieldValues(ControlsEnum.DETAILS);
                                lblhdrEmployeeNoTxtPopup.Text = txtEmployee.Text;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divHistoryDetails]','" + GetLocalResourceObject("HistoryDeatils").ToString() + "','900','400');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Employee").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region Employee Loan & Advance List
                    case ActionsEnum.LIST:
                        CurrPK = 0;
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.EMPLOANLIST);
                        GetFieldValues(ControlsEnum.EMPLOANLIST);
                        SetFieldValues(ControlsEnum.EMPLOANLIST);
                        break;
                    #endregion

                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.EMPLOANLIST);
                        GetFieldValues(ControlsEnum.EMPLOANLIST);
                        SetFieldValues(ControlsEnum.EMPLOANLIST);
                        break;
                    #endregion

                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.EMPLOANLIST);
                        SetFieldValues(ControlsEnum.EMPLOANLIST);
                        break;
                    #endregion

                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.EMPLOANLIST);
                        GetFieldValues(ControlsEnum.EMPLOANLIST);
                        SetFieldValues(ControlsEnum.EMPLOANLIST);
                        GetFieldValues(ControlsEnum.LOANADVANCEITEM);
                        SetFieldValues(ControlsEnum.LOANADVANCEFILTERITEM);
                        break;
                    #endregion

                    #region CALCULATE INTEREST AMOUNT
                    case ActionsEnum.CALCULATEINTERESTAMT:
                        CalculateTotalPrincipal();
                        GetFieldValues(ControlsEnum.SHOWINSTALLMENTS);
                        SetFieldValues(ControlsEnum.SHOWINSTALLMENTS);
                        break;
                    #endregion

                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = LoansAndAdvancesBL.DeleteLoansAndAdvances(this.CurrPK, Convert.ToString(LastModifiedTime));
                        if (result > 0)
                        {
                            EntryStatus = EntryStatus.LISTMODE;
                            ResetForm(ControlsEnum.EMPLOANLIST);
                            if (grdEmpLoanList.Rows.Count == 1 && PageIndex > 1)
                            {
                                PageIndex--;
                            }
                            GetFieldValues(ControlsEnum.EMPLOANLIST);
                            SetFieldValues(ControlsEnum.EMPLOANLIST);
                            btnNew.Focus();
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.LoansAndAdvances);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.LoansAndAdvances;
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
                                litErrorMsg.Text = Resources.PageNameRes.LoansAndAdvances + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.LoansAndAdvances + " " +
                                    GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.LoansAndAdvances + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.LoansAndAdvances);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region DELETE
                    case ActionsEnum.DELETE_ACTION:
                        SetUIValuesToObject(ControlsEnum.SHOWINSTALLMENTS);
                        if (empLoanDetailList != null && empLoanDetailList.Count > 0)
                        {
                            GridViewRow gvrTemplate = ((ImageButton)sender).Parent.Parent as GridViewRow;
                            Int32 hdfEarnSlNo = Convert.ToInt32(((HiddenField)grdLoanSlabDetails.Rows[gvrTemplate.RowIndex].FindControl("hdfELS_PK")).Value);
                            Int32 Slno = Convert.ToInt32(((Label)grdLoanSlabDetails.Rows[gvrTemplate.RowIndex].FindControl("lblSlNo")).Text);


                            var removeEMI = empLoanDetailList.Where(sal => sal.ELS_PK == hdfEarnSlNo && sal.SlNo == Slno).SingleOrDefault();
                            if (removeEMI != null)
                            {
                                empLoanDetailList.Remove(removeEMI);
                                SetFieldValues(ControlsEnum.SHOWINSTALLMENTS);
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

                    #region CURRENCYSELECTED
                    case ActionsEnum.CURRENCYSELECTED:
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        SetFieldValues(ControlsEnum.EXCHANGERATE);
                        txtApprovedDate.Text = txtEffectiveDate.Text = txtApplyDate.Text;
                        break;
                    #endregion

                    #region CHANGEEMPLOYEE
                    case ActionsEnum.CHANGEEMPLOYEE:
                        GetFieldValues(ControlsEnum.EMPCURRENCY);
                        SetFieldValues(ControlsEnum.EMPCURRENCY);
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        SetFieldValues(ControlsEnum.EXCHANGERATE);
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

        private void EnableDisableControls(bool state)
        {
            txtEmployee.Enabled = state;
            ddlItem.Enabled = state;
        }
        protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        {
            try
            {
                switch (e.Action)
                {
                    case NavigationEnum.PAGECHANGE:
                        uclPaging.CurrentPage = e.CurrentPage;
                        break;
                    case NavigationEnum.FIRST:
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage--;
                        break;
                }
                PageIndex = uclPaging.CurrentPage;
                GetFieldValues(ControlsEnum.EMPLOANLIST);
                SetFieldValues(ControlsEnum.EMPLOANLIST);
                EnableDisableButtons(e.TotalPages, "uclPaging");
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex;
            EntryStatus = EntryStatus.LISTMODE;
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
            uclPaging.CurrentPage = 1;
            btnSave.PreRender += new EventHandler(btnAction_PreRender);
            btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            btnView.PreRender += new EventHandler(btnAction_PreRender);
            btnNew.PreRender += new EventHandler(btnAction_PreRender);

            //lnkList.PreRender += new EventHandler(btnAction_PreRender);
            // lnkDetail.PreRender += new EventHandler(btnAction_PreRender);


            btnSave.Load += new EventHandler(btnAction_Load);
            btnCancel.Load += new EventHandler(btnAction_Load);
            btnEdit.Load += new EventHandler(btnAction_Load);
            btnView.Load += new EventHandler(btnAction_Load);
            btnNew.Load += new EventHandler(btnAction_Load);

            //lnkList.Load += new EventHandler(btnAction_Load);
            //lnkDetail.Load += new EventHandler(btnAction_Load);
        }

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);

            string str = txtInstallments.Text;
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
                lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
            }
            if (EntryStatus == EntryStatus.VIEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateEMI_Details", "CalculateEMI_Details(this)", true);
            if (!txtEmployee.Enabled)
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_Disableauto", "Disableautocomplete();", true);
            if (!MultiCurrencyEnabled)
                txtCurrency.Enabled = false;

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
            this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
            this.Init += new EventHandler(this.Page_Init);
        }

        private void EnableDisableButtons(int iTotalPages, string pagerId)
        {
            if (pagerId == "uclPaging")
            {
                uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
                uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
                uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
                uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
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
            EMPLOANLIST,
            EMPLOYEES,
            LOANADVANCEITEM,
            LOANADVANCEFILTERITEM,
            LOANADVANCETYPE,
            SHOWINSTALLMENTS,
            EMPLOANHEADER,
            EMPLOYEELOANINFO,
            EMPLOYMENTTYPE,
            BRANCH,
            DETAILS,
            EXCHANGERATE,
            CURRENCY,
            EMPCURRENCY,
            COMPANY,
            TYPECHANGE
        }
        #endregion
        #region Enum
        /// <summary>
        ///Pay Elements Clssification Enum 
        /// </summary>
        public enum PelClassEnum
        {
            Advances=131
        }
        #endregion
    }
}