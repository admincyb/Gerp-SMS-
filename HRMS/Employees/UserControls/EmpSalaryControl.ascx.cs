using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using ERP.Utilities;
using BusinessObject.HRMS.Employee;
using BusinessLogic.HRMS.Employee;
using System.Data;
using BusinessObject.CommonManagement;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using BusinessLogic.CommonManagement;
using BusinessObject;

namespace HRMS.Employees.UserControls
{
    public partial class EmpSalaryControl : System.Web.UI.UserControl
    {
        #region Variables
        private EmpSalaryDetails objEmpSalary;
        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        BusinessObject.User currentUser;
        DataTable dtResult;
        DataTable dtCustomSlab;

        List<EmpSalaryDetails> EmployeeSalaryList;
        List<EmpSalaryDetails> TempEmployeeSalaryList;
        EmpSalaryDetails currLine;//For Line Movement
        EmpSalaryDetails nextLine;//For Line Movement
        private EmpTemplateHeader objEmpSalaryDetails;
        int salaryTemplatePk = 0;
        int payElementMode = 0;
        int PayElementPk = 0;
        int payElementErnDeductPK = 0;

        #endregion

        #region Properties
        public int EmployeePK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.EmployeePK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.EmployeePK] = value;
            }
        }

        public int ControlActionMode  //Show or hide Controls in the user control
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.EmpSalaryAction]);
            }
            set
            {
                this.ViewState[ViewstateStrings.EmpSalaryAction] = value;
            }
        }

        public EmpTemplateHeader EmpSalaryHdr
        {
            get
            {
                return (EmpTemplateHeader)ViewState[ViewstateStrings.EmpSalaryHdr];
            }
            set
            {
                ViewState[ViewstateStrings.EmpSalaryHdr] = value;
            }
        }

        private List<EmpSalaryDetails> EmpSalaryList
        {
            get
            {
                return (List<EmpSalaryDetails>)ViewState[ViewstateStrings.empSalaryDetailList];
            }
            set
            {
                ViewState[ViewstateStrings.empSalaryDetailList] = value;
            }
        }

        private int SelectedEarnSeqNo
        {
            get
            {
                return (int)ViewState[ViewstateStrings.SelectedEarnPK];
            }
            set
            {
                ViewState[ViewstateStrings.SelectedEarnPK] = value;
            }
        }

        /// <summary>
        /// Hide Checking
        /// </summary>
        private bool payElementHide
        {
            get
            {
                return this.ViewState["payElementHide"] == null ? false : Convert.ToBoolean(this.ViewState["payElementHide"]);
            }
            set
            {
                this.ViewState["payElementHide"] = value;
            }
        }

        private int SelectedDeductSeqNo
        {
            get
            {
                return (int)ViewState[ViewstateStrings.SelectedDeductPK];
            }
            set
            {
                ViewState[ViewstateStrings.SelectedDeductPK] = value;
            }
        }

        private int IsApplyEarnFormula
        {
            get
            {
                return (int)ViewState[ViewstateStrings.IsApplyEarnFormula];
            }
            set
            {
                ViewState[ViewstateStrings.IsApplyEarnFormula] = value;
            }
        }
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

        private DataTable dtEarningData
        {
            get
            {
                return (DataTable)(this.ViewState[ERP.Utilities.ViewstateStrings.EarningData]);
            }
            set
            {
                this.ViewState[ERP.Utilities.ViewstateStrings.EarningData] = value;
            }
        }
        private DataTable dtDeductionData
        {
            get
            {
                return (DataTable)(this.ViewState[ERP.Utilities.ViewstateStrings.DeductionData]);
            }
            set
            {
                this.ViewState[ERP.Utilities.ViewstateStrings.DeductionData] = value;
            }
        }

        public string EffectTo
        {
            get
            {
                return Convert.ToString(this.ViewState[ViewstateStrings.EffectTo]);
            }
            set
            {
                this.ViewState[ViewstateStrings.EffectTo] = value;
            }
        }
        public string trxDate
        {
            get
            {
                return Convert.ToString(this.ViewState[ViewstateStrings.TrxDate]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TrxDate] = value;
            }
        }

        public int IsSalaryProcessed
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.SalaryProcessed] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.SalaryProcessed];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.SalaryProcessed] = value;
            }
        }

        public int IsEmpAppraisal  //Show or hide Amount Old and Delete Checkbox in both Grid
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.IsEmpAppraisal]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsEmpAppraisal] = value;
            }
        }

        public int MonthlyStartDay
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.MonthlyStartDay]);
            }
            set
            {
                this.ViewState[ViewstateStrings.MonthlyStartDay] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ucFormulaMaster.AfterApply += new EventHandler(ucFormulaMaster_AfterApply);
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            GetUserRights();
            if (!IsPostBack)
            {
                hdfCurrentDepartment.Value = currentUser.CurrentDeptPK.ToString();
                PageActionHandler();
            }
        }
      
        #region PageActionHandler
        private void PageActionHandler()
        {
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
           // GetUserRights();
            SetFieldValues(ActionsEnum.EMPLOYEESALARY);
           
        }
        #endregion

        #region Function

        public void SetUIValuesToObject(ActionsEnum controlType)
        {
            try
            {
                decimal minAmount = 0;
                decimal maxAmount = 0;
                double amount = 0;
                double oldAmount = 0;
                int count = 0;
                switch (controlType)
                {
                    case ActionsEnum.EMPLOYEESALARY:
                        EmpSalaryList = new List<EmpSalaryDetails>();
                        foreach (GridViewRow grdRow in grdEarnings.Rows)  // Earnings
                        {
                            TextBox txtEarnAmount = (TextBox)grdRow.FindControl("txtEarnAmount");
                            HiddenField hdfEarnPK = (HiddenField)grdRow.FindControl("hdfEarnPK");
                            HiddenField hdfEarnPayElement = (HiddenField)grdRow.FindControl("hdfEarnPayElement");
                            HiddenField hdfEarnFormulaCode = (HiddenField)grdRow.FindControl("hdfEarnFormulaCode");
                            HiddenField hdfEarnCalcValue = (HiddenField)grdRow.FindControl("hdfEarnCalcValue");
                            HiddenField hdfIsEarn = (HiddenField)grdRow.FindControl("hdfIsEarn");
                            HiddenField hdfEarnSlNo = (HiddenField)grdRow.FindControl("hdfEarnSlNo");
                            DropDownList ddlEarnPayElement = (DropDownList)grdRow.FindControl("ddlEarnPayElement");
                            HiddenField hdfEarnPayElementPK = (HiddenField)grdRow.FindControl("hdfEarnPayElementPK");
                            HiddenField hdfEarnCalcMode = (HiddenField)grdRow.FindControl("hdfEarnCalcMode");
                            HiddenField hdfErnHasPayroll = (HiddenField)grdRow.FindControl("hdfErnHasPayroll");
                            HiddenField hdfEarnPayElmtInSalary = (HiddenField)grdRow.FindControl("hdfEarnPayElmtInSalary");
                            HiddenField hdfEarnShowPayElement = (HiddenField)grdRow.FindControl("hdfEarnShowPayElement");
                            HiddenField hdfErnPartofCTC = (HiddenField)grdRow.FindControl("hdfErnPartofCTC");
                            HiddenField hdfErnPartofGross = (HiddenField)grdRow.FindControl("hdfErnPartofGross");
                            Label lblEarnFormula = (Label)grdRow.FindControl("lblEarnFormula");
                            HiddenField hdfEarnFormula = (HiddenField)grdRow.FindControl("hdfEarnFormula");
                            Label lblEarnHead = (Label)grdRow.FindControl("lblEarnHead");
                            DropDownList ddlErnSlabCustom = (DropDownList)grdRow.FindControl("ddlErnSlabCustom");
                            HiddenField hdfErnMinAmount = (HiddenField)grdRow.FindControl("hdfErnMinAmount");
                            HiddenField hdfErnMaxAmount = (HiddenField)grdRow.FindControl("hdfErnMaxAmount");

                            Label lblEarnAmount_OLD = (Label)grdRow.FindControl("lblEarnAmount_OLD");
                            HiddenField hdfEID_PK = (HiddenField)grdRow.FindControl("hdfEID_PK");
                            HiddenField hdfEID_APPRAISAL_HDR = (HiddenField)grdRow.FindControl("hdfEID_APPRAISAL_HDR");
                            CheckBox chkErnPayElmDelete = (CheckBox)grdRow.FindControl("chkErnPayElmDelete");

                            objEmpSalary = new EmpSalaryDetails();
                            objEmpSalary.EDP_PK = Convert.ToInt32(hdfEarnPK.Value);
                            objEmpSalary.STS_PK = Convert.ToInt32(hdfEarnPayElementPK.Value);
                            objEmpSalary.EID_PK = Convert.ToInt32(hdfEID_PK.Value);
                            if (objEmpSalary.EDP_PK > 0 || objEmpSalary.EID_PK > 0 || (objEmpSalary.STS_PK > 0 && objEmpSalary.EDP_PK == 0))
                            {
                                objEmpSalary.STS_PAY_ELEMENT = Convert.ToInt32(hdfEarnPayElement.Value);
                            }
                            else  //Add new earnings
                            {
                                objEmpSalary.STS_PAY_ELEMENT = Convert.ToInt32(ddlEarnPayElement.SelectedValue);
                            }
                            objEmpSalary.STS_PAY_ELEMENT_TEXT = lblEarnHead.Text;
                            //objEmpSalary.STS_VALUE_TEXT = lblEarnFormula.Text;
                            objEmpSalary.STS_VALUE_TEXT = hdfEarnFormula.Value;
                            objEmpSalary.STS_FORMULA_CODE = hdfEarnFormulaCode.Value;
                            double.TryParse(txtEarnAmount.Text, out amount);
                            objEmpSalary.STS_VALUE = amount;
                            objEmpSalary.PEL_IS_DEDUCTION = Convert.ToByte(hdfIsEarn.Value);
                            objEmpSalary.STS_SL_NO = Convert.ToByte(hdfEarnSlNo.Value);
                            objEmpSalary.STS_CALC_MODE = Convert.ToInt32(hdfEarnCalcMode.Value);
                            objEmpSalary.STS_CALC_VALUE = hdfEarnCalcValue.Value;
                            if (count%2==0)
                            objEmpSalary.STS_HAS_PAYROLL =1 ;//Convert.ToInt32(hdfErnHasPayroll.Value);
                            count++;
                            objEmpSalary.PEL_IN_SALARY = Convert.ToByte(hdfEarnPayElmtInSalary.Value);
                            objEmpSalary.PEL_IN_CTC = Convert.ToByte(hdfErnPartofCTC.Value);
                            objEmpSalary.PEL_IN_GROSS = Convert.ToByte(hdfErnPartofGross.Value);
                            objEmpSalary.PEL_SHOW_IN_EMPMAST = Convert.ToByte(hdfEarnShowPayElement.Value);
                            if (string.IsNullOrEmpty(hdfEarnCalcValue.Value))
                            {
                                objEmpSalary.STS_CALC_VALUE = ddlErnSlabCustom.SelectedValue;
                                if (!string.IsNullOrEmpty(ddlErnSlabCustom.SelectedValue))
                                    objEmpSalary.STS_VALUE_TEXT = ddlErnSlabCustom.SelectedItem.Text;
                            }
                            decimal.TryParse(hdfErnMinAmount.Value, out minAmount);
                            decimal.TryParse(hdfErnMaxAmount.Value, out maxAmount);
                            objEmpSalary.STS_MIN_AMT = minAmount;
                            objEmpSalary.STS_MAX_AMT = maxAmount;

                            double.TryParse(lblEarnAmount_OLD.Text, out oldAmount);
                            objEmpSalary.STS_VALUE_OLD = oldAmount;
                            objEmpSalary.EID_APPRAISAL_HDR = Convert.ToInt32(hdfEID_APPRAISAL_HDR.Value);
                            if (chkErnPayElmDelete.Checked == true)
                                objEmpSalary.EID_IS_DELETE = 1;

                            //if (objEmpSalary.STS_CALC_MODE == (int)PayElementCalcMode.FixedAmount)
                            //{
                            //    objEmpSalary.STS_CALC_VALUE = txtEarnAmount.Text;
                            //}
                            //else if (objEmpSalary.STS_CALC_MODE == (int)PayElementCalcMode.Formula)
                            //{
                            //    objEmpSalary.STS_CALC_VALUE = hdfEarnCalcValue.Value;
                            //}
                            //else if (objEmpSalary.STS_CALC_MODE == (int)PayElementCalcMode.Slab)
                            //{
                            //    objEmpSalary.STS_CALC_VALUE = string.Empty;
                            //}
                            //else if (objEmpSalary.STS_CALC_MODE == (int)PayElementCalcMode.Custom)
                            //{
                            //    objEmpSalary.STS_CALC_VALUE = string.Empty;
                            //}
                            //else
                            //{
                            //    objEmpSalary.STS_CALC_VALUE = string.Empty;
                            //}
                            EmpSalaryList.Add(objEmpSalary);
                        }
                        foreach (GridViewRow grdRow in grdDeductions.Rows)  // Deductions
                        {
                            TextBox txtDeductAmount = (TextBox)grdRow.FindControl("txtDeductAmount");
                            HiddenField hdfDeductPK = (HiddenField)grdRow.FindControl("hdfDeductPK");
                            HiddenField hdfDeductPayElement = (HiddenField)grdRow.FindControl("hdfDeductPayElement");
                            HiddenField hdfDeductFormulaCode = (HiddenField)grdRow.FindControl("hdfDeductFormulaCode");
                            HiddenField hdfDeductCalcValue = (HiddenField)grdRow.FindControl("hdfDeductCalcValue");
                            HiddenField hdfIsDeduct = (HiddenField)grdRow.FindControl("hdfIsDeduct");
                            HiddenField hdfDeductSlNo = (HiddenField)grdRow.FindControl("hdfDeductSlNo");
                            DropDownList ddlDeductPayElement = (DropDownList)grdRow.FindControl("ddlDeductPayElement");
                            HiddenField hdfDeductPayElementPK = (HiddenField)grdRow.FindControl("hdfDeductPayElementPK");
                            HiddenField hdfDeductCalcMode = (HiddenField)grdRow.FindControl("hdfDeductCalcMode");
                            HiddenField hdfDeductHasPayroll = (HiddenField)grdRow.FindControl("hdfDeductHasPayroll");
                            HiddenField hdfDedPayElmtInSalary = (HiddenField)grdRow.FindControl("hdfDedPayElmtInSalary");
                            HiddenField hdfDedShowPayElement = (HiddenField)grdRow.FindControl("hdfDedShowPayElement");
                            HiddenField hdfDedPartofCTC = (HiddenField)grdRow.FindControl("hdfDedPartofCTC");
                            HiddenField hdfDedPartofGross = (HiddenField)grdRow.FindControl("hdfDedPartofGross");
                            Label lblDeductFormula = (Label)grdRow.FindControl("lblDeductFormula");
                            HiddenField hdfDeductFormula = (HiddenField)grdRow.FindControl("hdfDeductFormula");
                            Label lblDeductHead = (Label)grdRow.FindControl("lblDeductHead");
                            DropDownList ddlDeductSlabCustom = (DropDownList)grdRow.FindControl("ddlDeductSlabCustom");
                            HiddenField hdfDeductMinAmount = (HiddenField)grdRow.FindControl("hdfDeductMinAmount");
                            HiddenField hdfDeductMaxAmount = (HiddenField)grdRow.FindControl("hdfDeductMaxAmount");

                            Label lblDeductAmount_OLD = (Label)grdRow.FindControl("lblDeductAmount_OLD");
                            HiddenField hdfEID_PK = (HiddenField)grdRow.FindControl("hdfDedEID_PK");
                            HiddenField hdfEID_APPRAISAL_HDR = (HiddenField)grdRow.FindControl("hdfDedEID_APPRAISAL_HDR");
                            CheckBox chkDedPayElmDelete = (CheckBox)grdRow.FindControl("chkDedPayElmDelete");

                            objEmpSalary = new EmpSalaryDetails();
                            objEmpSalary.EDP_PK = Convert.ToInt32(hdfDeductPK.Value);
                            objEmpSalary.STS_PK = Convert.ToInt32(hdfDeductPayElementPK.Value);
                            objEmpSalary.EID_PK = Convert.ToInt32(hdfEID_PK.Value);
                            if (objEmpSalary.EDP_PK > 0 || objEmpSalary.EID_PK > 0 || (objEmpSalary.STS_PK > 0 && objEmpSalary.EDP_PK == 0))
                            {
                                objEmpSalary.STS_PAY_ELEMENT = Convert.ToInt32(hdfDeductPayElement.Value);
                            }
                            else  //Add new deduction
                            {
                                objEmpSalary.STS_PAY_ELEMENT = Convert.ToInt32(ddlDeductPayElement.SelectedValue);
                            }
                            objEmpSalary.STS_PAY_ELEMENT_TEXT = lblDeductHead.Text;
                            //objEmpSalary.STS_VALUE_TEXT = lblDeductFormula.Text;
                            objEmpSalary.STS_VALUE_TEXT = hdfDeductFormula.Value;
                            objEmpSalary.STS_FORMULA_CODE = hdfDeductFormulaCode.Value;
                            double.TryParse(txtDeductAmount.Text, out amount);
                            objEmpSalary.STS_VALUE = amount;
                            objEmpSalary.PEL_IS_DEDUCTION = Convert.ToByte(hdfIsDeduct.Value);
                            objEmpSalary.STS_SL_NO = Convert.ToByte(hdfDeductSlNo.Value);
                            objEmpSalary.STS_CALC_MODE = Convert.ToInt32(hdfDeductCalcMode.Value);
                            objEmpSalary.STS_CALC_VALUE = hdfDeductCalcValue.Value;
                            objEmpSalary.STS_HAS_PAYROLL = Convert.ToInt32(hdfDeductHasPayroll.Value);
                            objEmpSalary.PEL_IN_SALARY = Convert.ToByte(hdfDedPayElmtInSalary.Value);
                            objEmpSalary.PEL_SHOW_IN_EMPMAST = Convert.ToByte(hdfDedShowPayElement.Value);
                            objEmpSalary.PEL_IN_CTC = Convert.ToByte(hdfDedPartofCTC.Value);
                            objEmpSalary.PEL_IN_GROSS = Convert.ToByte(hdfDedPartofGross.Value);
                            if (string.IsNullOrEmpty(hdfDeductCalcValue.Value))
                            {
                                objEmpSalary.STS_CALC_VALUE = ddlDeductSlabCustom.SelectedValue;
                                if (!string.IsNullOrEmpty(ddlDeductSlabCustom.SelectedValue))
                                    objEmpSalary.STS_VALUE_TEXT = ddlDeductSlabCustom.SelectedItem.Text;
                            }
                            decimal.TryParse(hdfDeductMinAmount.Value, out minAmount);
                            decimal.TryParse(hdfDeductMaxAmount.Value, out maxAmount);
                            objEmpSalary.STS_MIN_AMT = minAmount;
                            objEmpSalary.STS_MAX_AMT = maxAmount;

                            double.TryParse(lblDeductAmount_OLD.Text, out oldAmount);
                            objEmpSalary.STS_VALUE_OLD = oldAmount;
                            objEmpSalary.EID_APPRAISAL_HDR = Convert.ToInt32(hdfEID_APPRAISAL_HDR.Value);
                            if (chkDedPayElmDelete.Checked == true)
                                objEmpSalary.EID_IS_DELETE = 1;
                            //if (objEmpSalary.STS_CALC_MODE == (int)PayElementCalcMode.FixedAmount)
                            //{
                            //    objEmpSalary.STS_CALC_VALUE = txtDeductAmount.Text;
                            //}
                            //else if (objEmpSalary.STS_CALC_MODE == (int)PayElementCalcMode.Formula)
                            //{
                            //    objEmpSalary.STS_CALC_VALUE = hdfDeductCalcValue.Value;
                            //}
                            //else if (objEmpSalary.STS_CALC_MODE == (int)PayElementCalcMode.Slab)
                            //{
                            //    objEmpSalary.STS_CALC_VALUE = string.Empty;
                            //}
                            //else if (objEmpSalary.STS_CALC_MODE == (int)PayElementCalcMode.Custom)
                            //{
                            //    objEmpSalary.STS_CALC_VALUE = string.Empty;
                            //}
                            //else
                            //{
                            //    objEmpSalary.STS_CALC_VALUE = string.Empty;
                            //}
                            EmpSalaryList.Add(objEmpSalary);
                        }
                        EmpSalaryHdr.SalaryDtl = EmpSalaryList;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetFieldValues(ActionsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ActionsEnum.EMPLOYEESALARY:
                        BindGrid(ActionsEnum.EARNINGS);
                        BindGrid(ActionsEnum.DEDUCTIONS);
                        break;                    
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GetFieldValues(ActionsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ActionsEnum.EMPLOYEESALARY:

                        EmpSalaryHdr = EmployeeSalaryBL.GetEmployeeSalaryDetails(EmployeePK, EffectTo,trxDate);
                        this.EffectTo = string.Empty;
                        IsSalaryProcessed = 0;                           
                        if (EmpSalaryHdr != null && EmpSalaryHdr.SalaryDtl != null && EmpSalaryHdr.SalaryDtl.Count > 0)
                        {
                            if (EmpSalaryHdr.SalaryDtl.Where(r => r.STS_HAS_PAYROLL > 0).Count() > 0)
                                IsSalaryProcessed = 1;
                            MonthlyStartDay =EmpSalaryHdr.PTM_PAYRL_START;
                            EmpSalaryHdr.SalaryDtl[0].STS_HAS_PAYROLL = 1;                           
                        }
                        break;
                    case ActionsEnum.EARNINGSPAYELEMENTS:
                        dtEarningData = BusinessLogic.HRMS.Admin.Masters.PayElementsMasterBL.GetEarnDeductPayElements(payElementErnDeductPK, Convert.ToInt32(CommonConstants.ACTIVE), currentUser.SBUID, Convert.ToInt32(DeductMode.Earn));
                        break;
                    case ActionsEnum.DEDUCTIONPAYELEMENTS:
                        dtDeductionData = BusinessLogic.HRMS.Admin.Masters.PayElementsMasterBL.GetEarnDeductPayElements(payElementErnDeductPK, Convert.ToInt32(CommonConstants.ACTIVE), currentUser.SBUID, Convert.ToInt32(DeductMode.Deduct));
                        break;
                    #region SALARY TEMPLATE DETAILS
                    case ActionsEnum.SALARYTEMPLATEDETAILS:
                        objEmpSalaryDetails = EmployeeSalaryBL.GetSalaryDetails(salaryTemplatePk);
                        break;
                    #endregion
                    #region PAY ELEMENT DETAILS
                    case ActionsEnum.PAYELEMENTDETAILS:
                        dtResult = BusinessLogic.HRMS.Admin.Masters.PayElementsMasterBL.GetParentElement(PayElementPk, Convert.ToInt32(DbActiveStatus.HASPK), currentUser.SBUID, 0, 0, -1);
                        break;
                    #endregion
                    #region PAY ELEMENT SLAB CUSTOM
                    case ActionsEnum.PAYELEMENTSLABCUSTOM:
                        dtCustomSlab = BusinessLogic.HRMS.Admin.Masters.PayElementsMasterBL.GetPayElementValueSingleList(PayElementPk, (int)DbActiveStatus.ACTIVE,0);
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

        private void BindGrid(ActionsEnum controlType)
        {
            try
            {
                Label lblTotalEarnAmount;
                Label lblOLDTotalEarnAmount;
                HiddenField hdfTotalErnCTC;
                HiddenField hdfTotalErnGross;

                Label lblTotalDeductAmount;
                Label lblOLDTotalDeductAmount;
                HiddenField hdfTotalDedCTC;
                HiddenField hdfTotalDedGross;

                TextBox txtEmpNetSalary;
                TextBox txtTotalCTC;
                TextBox txtTotalGross;

                TextBox txtEmpNetSalary_Old;
                TextBox txtTotalCTC_Old;
                TextBox txtTotalGross_Old;

                TextBox txtEmpNetSalary_Diff;
                TextBox txtTotalCTC_Diff;
                TextBox txtTotalGross_Diff;

                switch (controlType)
                {
                    case ActionsEnum.EARNINGS:
                        if (EmpSalaryHdr != null && EmpSalaryHdr.SalaryDtl != null)
                        {
                            hdfIsGridRecord.Value = "1";
                            if (payElementHide == true)
                            {
                                grdEarnings.DataSource = EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 0 && sal.PEL_SHOW_IN_EMPMAST == 1).OrderBy(r => r.STS_SL_NO).ToList();
                            }
                            else
                            {
                                grdEarnings.DataSource = EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 0).OrderBy(r => r.STS_SL_NO).ToList();
                            }
                           
                            grdEarnings.DataBind();
                            if (grdEarnings.FooterRow != null)
                            {
                                lblTotalEarnAmount = grdEarnings.FooterRow.FindControl("lblTotalEarnAmount") as Label;
                                lblOLDTotalEarnAmount = grdEarnings.FooterRow.FindControl("lblOLDTotalEarnAmount") as Label;
                                hdfTotalErnCTC = grdEarnings.FooterRow.FindControl("hdfTotalErnCTC") as HiddenField;
                                hdfTotalErnGross = grdEarnings.FooterRow.FindControl("hdfTotalErnGross") as HiddenField;

                                lblTotalEarnAmount.Text = EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 0 && sal.PEL_IN_SALARY == 1 && sal.EID_IS_DELETE==0).Sum(tot => tot.STS_VALUE).ToString(hdfCurrencyFormatWithComma.Value);
                                lblOLDTotalEarnAmount.Text = EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 0 && sal.PEL_IN_SALARY == 1).Sum(tot => tot.STS_VALUE_OLD).ToString(hdfCurrencyFormatWithComma.Value);

                                hdfTotalErnCTC.Value = EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 0 && sal.PEL_IN_CTC == 1 && sal.EID_IS_DELETE == 0).Sum(tot => tot.STS_VALUE).ToString(hdfCurrencyFormatWithComma.Value);
                                hdfTotalErnGross.Value = EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 0 && sal.PEL_IN_GROSS == 1 && sal.EID_IS_DELETE == 0).Sum(tot => tot.STS_VALUE).ToString(hdfCurrencyFormatWithComma.Value);                         
                                
                                txtEmpNetSalary = (System.Web.UI.WebControls.TextBox)this.Parent.FindControl("txtEmpNetSalary");
                                txtTotalCTC = (System.Web.UI.WebControls.TextBox)this.Parent.FindControl("txtTotalCTC");
                                txtTotalGross = (System.Web.UI.WebControls.TextBox)this.Parent.FindControl("txtTotalGross");

                                txtEmpNetSalary_Old = (System.Web.UI.WebControls.TextBox)this.Parent.FindControl("txtOldNetSalary");
                                txtTotalCTC_Old = (System.Web.UI.WebControls.TextBox)this.Parent.FindControl("txtOldCTC");
                                txtTotalGross_Old = (System.Web.UI.WebControls.TextBox)this.Parent.FindControl("txtOldGrossSalary");

                                txtEmpNetSalary_Diff = (System.Web.UI.WebControls.TextBox)this.Parent.FindControl("txtDiffNetSalary");
                                txtTotalCTC_Diff = (System.Web.UI.WebControls.TextBox)this.Parent.FindControl("txtDiffCTC");
                                txtTotalGross_Diff = (System.Web.UI.WebControls.TextBox)this.Parent.FindControl("txtDiffGrossSalary");


                                if (txtEmpNetSalary != null)
                                {
                                    txtEmpNetSalary.Text = (EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 0 && sal.PEL_IN_SALARY == 1 && sal.EID_IS_DELETE == 0).Sum(tot => tot.STS_VALUE)
                                                          - EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 1 && sal.PEL_IN_SALARY == 1 && sal.EID_IS_DELETE == 0).Sum(tot => tot.STS_VALUE)).ToString(hdfCurrencyFormat.Value);
                                }
                                if (txtTotalCTC != null)
                                {
                                    txtTotalCTC.Text = (EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 0 && sal.PEL_IN_CTC == 1 && sal.EID_IS_DELETE == 0).Sum(tot => tot.STS_VALUE)
                                                          - EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 1 && sal.PEL_IN_CTC == 1 && sal.EID_IS_DELETE == 0).Sum(tot => tot.STS_VALUE)).ToString(hdfCurrencyFormat.Value);
                                }
                                if (txtTotalGross != null)
                                {
                                    txtTotalGross.Text = (EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 0 && sal.PEL_IN_GROSS == 1 && sal.EID_IS_DELETE == 0).Sum(tot => tot.STS_VALUE)
                                                          - EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 1 && sal.PEL_IN_GROSS == 1 && sal.EID_IS_DELETE == 0).Sum(tot => tot.STS_VALUE)).ToString(hdfCurrencyFormat.Value);
                                }


                                if (txtEmpNetSalary_Old != null)
                                {
                                    txtEmpNetSalary_Old.Text = (EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 0 && sal.PEL_IN_SALARY == 1).Sum(tot => tot.STS_VALUE_OLD)
                                                          - EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 1 && sal.PEL_IN_SALARY == 1).Sum(tot => tot.STS_VALUE_OLD)).ToString(hdfCurrencyFormat.Value);
                                }
                                if (txtTotalCTC_Old != null)
                                {
                                    txtTotalCTC_Old.Text = (EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 0 && sal.PEL_IN_CTC == 1).Sum(tot => tot.STS_VALUE_OLD)
                                                          - EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 1 && sal.PEL_IN_CTC == 1).Sum(tot => tot.STS_VALUE_OLD)).ToString(hdfCurrencyFormat.Value);
                                }
                                if (txtTotalGross_Old != null)
                                {
                                    txtTotalGross_Old.Text = (EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 0 && sal.PEL_IN_GROSS == 1).Sum(tot => tot.STS_VALUE_OLD)
                                                          - EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 1 && sal.PEL_IN_GROSS == 1).Sum(tot => tot.STS_VALUE_OLD)).ToString(hdfCurrencyFormat.Value);
                                }

                                if (txtEmpNetSalary_Diff != null && txtEmpNetSalary != null && txtEmpNetSalary_Old != null)
                                {
                                    txtEmpNetSalary_Diff.Text = (Convert.ToDouble(txtEmpNetSalary.Text) - Convert.ToDouble(txtEmpNetSalary_Old.Text)).ToString(hdfCurrencyFormat.Value);                                            
                                }
                                if (txtTotalCTC_Diff != null && txtTotalCTC != null && txtTotalCTC_Old != null)
                                {
                                    txtTotalCTC_Diff.Text = (Convert.ToDouble(txtTotalCTC.Text) - Convert.ToDouble(txtTotalCTC_Old.Text)).ToString(hdfCurrencyFormat.Value);
                                }
                                if (txtTotalGross_Diff != null && txtTotalGross != null && txtTotalGross_Old != null)
                                {
                                    txtTotalGross_Diff.Text = (Convert.ToDouble(txtTotalGross.Text) - Convert.ToDouble(txtTotalGross_Old.Text)).ToString(hdfCurrencyFormat.Value);
                                }
                            }
                        }
                        else
                        {
                            grdEarnings.DataSource = null;
                            grdEarnings.DataBind();
                            hdfIsGridRecord.Value = "0";
                        }
                        break;
                    case ActionsEnum.DEDUCTIONS:
                        if (EmpSalaryHdr != null && EmpSalaryHdr.SalaryDtl != null)
                        {
                            hdfIsGridRecord.Value = "1";

                            if (payElementHide == true)
                            {
                                grdDeductions.DataSource = EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 1 && sal.PEL_SHOW_IN_EMPMAST == 1).OrderBy(r => r.STS_SL_NO).ToList();
                            }
                            else
                            {
                                grdDeductions.DataSource = EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 1).OrderBy(r => r.STS_SL_NO).ToList();
                            }
                            //grdDeductions.DataSource = EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 1).OrderBy(r => r.STS_SL_NO).ToList();
                            grdDeductions.DataBind();
                            if (grdDeductions.FooterRow != null)
                            {
                                lblTotalDeductAmount = grdDeductions.FooterRow.FindControl("lblTotalDeductAmount") as Label;
                                lblOLDTotalDeductAmount = grdDeductions.FooterRow.FindControl("lblOLDTotalDeductAmount") as Label;
                                hdfTotalDedCTC = grdDeductions.FooterRow.FindControl("hdfTotalDedCTC") as HiddenField;
                                hdfTotalDedGross = grdDeductions.FooterRow.FindControl("hdfTotalDedGross") as HiddenField;

                                lblTotalDeductAmount.Text = EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 1 && sal.PEL_IN_SALARY == 1 && sal.EID_IS_DELETE == 0).Sum(tot => tot.STS_VALUE).ToString(hdfCurrencyFormatWithComma.Value);
                                lblOLDTotalDeductAmount.Text = EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 1 && sal.PEL_IN_SALARY == 1).Sum(tot => tot.STS_VALUE_OLD).ToString(hdfCurrencyFormatWithComma.Value);
                                hdfTotalDedCTC.Value = EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 1 && sal.PEL_IN_CTC == 1 && sal.EID_IS_DELETE == 0).Sum(tot => tot.STS_VALUE).ToString(hdfCurrencyFormatWithComma.Value);
                                hdfTotalDedGross.Value = EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 1 && sal.PEL_IN_GROSS == 1 && sal.EID_IS_DELETE == 0).Sum(tot => tot.STS_VALUE).ToString(hdfCurrencyFormatWithComma.Value);
                                
                                txtEmpNetSalary = (System.Web.UI.WebControls.TextBox)this.Parent.FindControl("txtEmpNetSalary");
                                txtTotalCTC = (System.Web.UI.WebControls.TextBox)this.Parent.FindControl("txtTotalCTC");
                                txtTotalGross = (System.Web.UI.WebControls.TextBox)this.Parent.FindControl("txtTotalGross");
                                if (txtEmpNetSalary != null)
                                {
                                    txtEmpNetSalary.Text = (EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 0 && sal.PEL_IN_SALARY == 1 && sal.EID_IS_DELETE == 0).Sum(tot => tot.STS_VALUE)
                                                          - EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 1 && sal.PEL_IN_SALARY == 1 && sal.EID_IS_DELETE == 0).Sum(tot => tot.STS_VALUE)).ToString(hdfCurrencyFormat.Value);
                                }
                                if (txtTotalCTC != null)
                                {
                                    txtTotalCTC.Text = (EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 0 && sal.PEL_IN_CTC == 1 && sal.EID_IS_DELETE == 0).Sum(tot => tot.STS_VALUE)
                                                          - EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 1 && sal.PEL_IN_CTC == 1 && sal.EID_IS_DELETE == 0).Sum(tot => tot.STS_VALUE)).ToString(hdfCurrencyFormat.Value);
                                }
                                if (txtTotalGross != null)
                                {
                                    txtTotalGross.Text = (EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 0 && sal.PEL_IN_GROSS == 1 && sal.EID_IS_DELETE == 0).Sum(tot => tot.STS_VALUE)
                                                          - EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 1 && sal.PEL_IN_GROSS == 1 && sal.EID_IS_DELETE == 0).Sum(tot => tot.STS_VALUE)).ToString(hdfCurrencyFormat.Value);
                                }
                            }
                        }
                        else
                        {
                            hdfIsGridRecord.Value = "0";
                            grdDeductions.DataSource = null;
                            grdDeductions.DataBind();
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindDropDown(ActionsEnum controlType, DropDownList ddlControl)
        {
            try
            {
                switch (controlType)
                {
                    case ActionsEnum.EARNINGSPAYELEMENTS:
                        ddlControl.Items.Clear();
                        if (dtEarningData != null && dtEarningData.Rows.Count > 0)
                        {
                            ddlControl.DataSource = dtEarningData;
                            ddlControl.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_NAME;
                            ddlControl.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_PK;                            
                            ddlControl.Items.HtmlDecode();
                            ddlControl.DataBind();
                        }
                        ddlControl.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                        break;
                    case ActionsEnum.DEDUCTIONPAYELEMENTS:
                        ddlControl.Items.Clear();
                        if (dtDeductionData != null && dtDeductionData.Rows.Count > 0)
                        {
                            ddlControl.DataSource = dtDeductionData;
                            ddlControl.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_NAME;
                            ddlControl.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_PK;
                            ddlControl.Items.HtmlDecode();
                            ddlControl.DataBind();
                        }
                        ddlControl.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                        break;
                    case ActionsEnum.PAYELEMENTSLABCUSTOM:
                        ddlControl.Items.Clear();
                        if (dtCustomSlab != null && dtCustomSlab.Rows.Count > 0)
                        {
                            ddlControl.DataSource = dtCustomSlab;
                            ddlControl.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_VAL_NAME;
                            ddlControl.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_VAL_PK;
                            ddlControl.Items.HtmlDecode();
                            ddlControl.DataBind();
                        }
                        ddlControl.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region FORMULA
        void ucFormulaMaster_AfterApply(object sender, EventArgs e)
        {
            if (EmpSalaryHdr.SalaryDtl != null && EmpSalaryHdr.SalaryDtl.Count > 0)
            {
                if (IsApplyEarnFormula == 1) // For Earnings
                {
                    foreach (GridViewRow gvr in grdEarnings.Rows)
                    {
                        if (gvr.RowType == DataControlRowType.DataRow)
                        {
                            Label lblEarnFormula = gvr.FindControl("lblEarnFormula") as Label;
                            HiddenField hdfEarnFormula = gvr.FindControl("hdfEarnFormula") as HiddenField;   
                            HiddenField hdfEarnCalcValue = gvr.FindControl("hdfEarnCalcValue") as HiddenField;//hdfEarnFormulaCode
                            HiddenField hdfEarnSlNo = gvr.FindControl("hdfEarnSlNo") as HiddenField;
                            HiddenField hdfErnMinAmount = gvr.FindControl("hdfErnMinAmount") as HiddenField;
                            HiddenField hdfErnMaxAmount = gvr.FindControl("hdfErnMaxAmount") as HiddenField;
                             
                            //HiddenField hdfEarnPK = gvr.FindControl("hdfEarnPK") as HiddenField;
                            //objEmpSalary = EmpSalaryHdr.SalaryDtl.SingleOrDefault(pk => pk.STS_PK == SelectedEarnPK && pk.STS_PK == Convert.ToInt32(hdfEarnPK.Value));
                            objEmpSalary = EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 0)
                                                       .SingleOrDefault(pk => pk.STS_SL_NO == SelectedEarnSeqNo && pk.STS_SL_NO == Convert.ToInt32(hdfEarnSlNo.Value));
                            if (objEmpSalary != null)
                            {
                                //objEmpSalary.STS_VALUE_TEXT = lblEarnFormula.Text = lblEarnFormula.ToolTip = hdfEarnFormula.Value = ucFormulaMaster.FormulaText;
                                objEmpSalary.STS_VALUE_TEXT = lblEarnFormula.ToolTip = hdfEarnFormula.Value = ucFormulaMaster.FormulaText;
                                lblEarnFormula.Text = ERP.Utilities.CommonFunctions.GetShortString(ucFormulaMaster.FormulaText, 28);
                                objEmpSalary.STS_CALC_VALUE = hdfEarnCalcValue.Value = ucFormulaMaster.FormulaValue;
                                objEmpSalary.STS_MIN_AMT = ucFormulaMaster.MinAmount;
                                hdfErnMinAmount.Value = ucFormulaMaster.MinAmount.ToString();
                                objEmpSalary.STS_MAX_AMT = ucFormulaMaster.MaxAmount;
                                hdfErnMaxAmount.Value = ucFormulaMaster.MaxAmount.ToString();
                                if (objEmpSalary.STS_MIN_AMT > 0 || objEmpSalary.STS_MAX_AMT > 0)
                                    lblEarnFormula.ToolTip = lblEarnFormula.ToolTip + "(" + Resources.Controls.Min.ToString() + GetFormattedCurrency(objEmpSalary.STS_MIN_AMT) + ", " + Resources.Controls.Min.ToString() + GetFormattedCurrency(objEmpSalary.STS_MAX_AMT) + ")";
                                break;
                            }
                        }
                    }
                }
                else  // For Deductions
                {
                    foreach (GridViewRow gvr in grdDeductions.Rows)
                    {
                        if (gvr.RowType == DataControlRowType.DataRow)
                        {
                            Label lblDeductFormula = gvr.FindControl("lblDeductFormula") as Label;
                            HiddenField hdfDeductFormula = gvr.FindControl("hdfDeductFormula") as HiddenField;
                            HiddenField hdfDeductCalcValue = gvr.FindControl("hdfDeductCalcValue") as HiddenField;//hdfDeductFormulaCode
                            HiddenField hdfDeductSlNo = gvr.FindControl("hdfDeductSlNo") as HiddenField;
                            HiddenField hdfDeductMinAmount = gvr.FindControl("hdfDeductMinAmount") as HiddenField;
                            HiddenField hdfDeductMaxAmount = gvr.FindControl("hdfDeductMaxAmount") as HiddenField;
                            //HiddenField hdfDeductPK = gvr.FindControl("hdfDeductPK") as HiddenField;
                            //objEmpSalary = EmpSalaryHdr.SalaryDtl.SingleOrDefault(pk => pk.STS_PK == SelectedDeductPK && pk.STS_PK == Convert.ToInt32(hdfDeductPK.Value));
                            objEmpSalary = EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 1)
                                                       .SingleOrDefault(pk => pk.STS_SL_NO == SelectedDeductSeqNo && pk.STS_SL_NO == Convert.ToInt32(hdfDeductSlNo.Value));
                            if (objEmpSalary != null)
                            {
                                //objEmpSalary.STS_VALUE_TEXT = lblDeductFormula.Text = lblDeductFormula.ToolTip = hdfDeductFormula.Value = ucFormulaMaster.FormulaText;
                                objEmpSalary.STS_VALUE_TEXT =  lblDeductFormula.ToolTip = hdfDeductFormula.Value = ucFormulaMaster.FormulaText;
                                lblDeductFormula.Text = ERP.Utilities.CommonFunctions.GetShortString(ucFormulaMaster.FormulaText, 28);
                                objEmpSalary.STS_CALC_VALUE = hdfDeductCalcValue.Value = ucFormulaMaster.FormulaValue;
                                objEmpSalary.STS_MIN_AMT = ucFormulaMaster.MinAmount;
                                hdfDeductMinAmount.Value = ucFormulaMaster.MinAmount.ToString();
                                objEmpSalary.STS_MAX_AMT = ucFormulaMaster.MaxAmount;
                                hdfDeductMaxAmount.Value = ucFormulaMaster.MaxAmount.ToString();
                                if (objEmpSalary.STS_MIN_AMT > 0 || objEmpSalary.STS_MAX_AMT > 0)
                                    lblDeductFormula.ToolTip = lblDeductFormula.ToolTip + "(" + Resources.Controls.Min.ToString() + GetFormattedCurrency(objEmpSalary.STS_MIN_AMT) + ", " + Resources.Controls.Min.ToString() + GetFormattedCurrency(objEmpSalary.STS_MAX_AMT) + ")";

                                break;
                            }
                        }
                    }
                }
            }
            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalTotalEarn", "CalculateTotalEarnings();", true);
            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalTotalDeduct", "CalculateTotalDeductions();", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
        }
        #endregion

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

        private int GetCurrSequenceNo()
        {
            if (EmpSalaryHdr != null && EmpSalaryHdr.SalaryDtl != null)
            {
                if (EmpSalaryHdr.SalaryDtl.Count > 0)
                {
                    CurrSlNo = EmpSalaryHdr.SalaryDtl.Max(itm => itm.STS_SL_NO) + 1;

                    //if (EmpSalaryHdr.SalaryDtl.Where(itm => itm.STS_PK == 0).Count() >= 1) //EDP_PK
                    //{
                    //    CurrSlNo = CurrSlNo + 1;
                    //}
                    //else
                    //{
                    //    CurrSlNo = EmpSalaryHdr.SalaryDtl.Max(itm => itm.STS_SL_NO) + 1; //EmpSalaryHdr.SalaryDtl.Max(itm => itm.EDP_PK) + 1;
                    //}
                }
            }
            return CurrSlNo;
        }

        private void SetControlVisibility()
        {
            if (ControlActionMode == Convert.ToInt32(EmpSalaryAction.View))
            {
                btnAddEarnings.Visible = false;
                btnAddDeduction.Visible = false;
                foreach (GridViewRow grdRow in grdEarnings.Rows)  // Earnings
                {
                    ImageButton imbFormula = (ImageButton)grdRow.FindControl("imbFormula");
                    Button lnkRemove = (Button)grdRow.FindControl("lnkRemove");
                    TextBox txtEarnAmount = (TextBox)grdRow.FindControl("txtEarnAmount");
                    imbFormula.Visible = false;
                    lnkRemove.Visible = false;
                    txtEarnAmount.Enabled = false;
                    txtEarnAmount.CssClass = "input-w80 numeric input-disabled";
                }
                grdEarnings.Columns[0].Visible = false;//for hide up and down arrows(column hided bcz avoid space)

                foreach (GridViewRow grdRow in grdDeductions.Rows)  // Deduction
                {
                    ImageButton imbDeductFormula = (ImageButton)grdRow.FindControl("imbDeductFormula");
                    Button lnkRemove = (Button)grdRow.FindControl("lnkRemove");
                    TextBox txtDeductAmount = (TextBox)grdRow.FindControl("txtDeductAmount");
                    imbDeductFormula.Visible = false;
                    lnkRemove.Visible = false;
                    txtDeductAmount.Enabled = false;
                    txtDeductAmount.CssClass = "input-w80 numeric input-disabled";
                }
                grdDeductions.Columns[0].Visible = false;//for hide up and down arrows(column hided bcz avoid space)
            }

            if (IsEmpAppraisal == Convert.ToInt32(VisbleStatusEnum.TRUE))
            {
                grdEarnings.Columns[5].Visible = true;
                grdEarnings.Columns[8].Visible = true;
                grdEarnings.Columns[7].Visible = false;
                grdDeductions.Columns[5].Visible = true;
                grdDeductions.Columns[7].Visible = false;
                grdDeductions.Columns[8].Visible = true;
            }
        }

        public bool ValidatePageDept()
        {
            bool result = true;
            string redirectURL = "../../login.aspx";
            if (hdfCurrentDepartment.Value != "-1" && hdfCurrentDepartment.Value != ((BusinessObject.User)(HttpContext.Current.User.Identity)).CurrentDeptPK.ToString())
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
                if (System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] != null)
                {
                    redirectURL = System.Configuration.ConfigurationManager.AppSettings["ASSETURL"] + "?Logout=1";
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetGlobalResourceObject("ErrorMessages", "Msg_Dept_Session_Expired").ToString()) + "','" + GetGlobalResourceObject("Messages", "Information").ToString() + "','" + redirectURL + "');", true);
                result = false;
            }
            return result;
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
            if (!ValidatePageDept())
                return;
            try
            {
                #region For Line Movement
                int SlNo, lineIndex;
                #endregion
                Label lblEarnFormula;
                HiddenField hdfEarnFormula;
                HiddenField hdfEarnSlNo;
                Label lblDeductFormula;
                HiddenField hdfDeductFormula;
                Label lblEarnHead;
                Label lblDeductHead;
                HiddenField hdfDeductSlNo;
                HiddenField hdfEarnPayElement;
                HiddenField hdfDeductPayElement;
                HiddenField hdfErnMinAmount;
                HiddenField hdfErnMaxAmount;
                HiddenField hdfDeductMinAmount;
                HiddenField hdfDeductMaxAmount;
                DropDownList ddlDeductPayElement;
                DropDownList ddlEarnPayElement;
                GridViewRow grvRow;
                //Image imgPayMode;
                HtmlControl imgPayMode;
                string strPayElement = string.Empty;
                decimal minAmount = 0;
                decimal maxAmount = 0;
                
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (BusinessObject.AccountManagement.ActionsEnum)(Enum.Parse(typeof(BusinessObject.AccountManagement.ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (BusinessObject.AccountManagement.ActionsEnum)(Enum.Parse(typeof(BusinessObject.AccountManagement.ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (BusinessObject.AccountManagement.ActionsEnum)(Enum.Parse(typeof(BusinessObject.AccountManagement.ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlEarnPayElement")
                    {
                        commonActions = BusinessObject.AccountManagement.ActionsEnum.EARNINGCHANGE;
                    }
                    else if (((DropDownList)sender).ID == "ddlDeductPayElement")
                    {
                        commonActions = BusinessObject.AccountManagement.ActionsEnum.DEDUCTIONCHANGE;
                    }
                }

                switch (commonActions)
                {
                    #region Order priority Up and Down
                    case BusinessObject.AccountManagement.ActionsEnum.MOVEUP:
                        SetUIValuesToObject(ActionsEnum.EMPLOYEESALARY);
                        int TypeVal = 0; //for identify earning and deduction
                        if (((ImageButton)sender).ID == "imbRuleUpEarn")
                            TypeVal = 0;//earning
                        else
                            TypeVal = 1;//deduction

                        EmployeeSalaryList = EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == TypeVal).ToList();
                        //for take deduction datas
                        TempEmployeeSalaryList = EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION != TypeVal).ToList();

                        SlNo = Convert.ToInt32((sender as ImageButton).CommandArgument);
                        currLine = EmployeeSalaryList.Where(rl => rl.STS_SL_NO == SlNo).First();
                        lineIndex = EmployeeSalaryList.FindIndex(rl => rl.STS_SL_NO == SlNo);
                        nextLine = lineIndex == 0 ? null : EmployeeSalaryList.ElementAt(lineIndex - 1);
                        if (nextLine != null)
                        {
                            int currentlineNumber = currLine.STS_SL_NO;
                            currLine.STS_SL_NO = nextLine.STS_SL_NO;
                            nextLine.STS_SL_NO = currentlineNumber;
                        }
                        EmployeeSalaryList = EmployeeSalaryList.OrderBy(rl => rl.STS_SL_NO).ToList();
                        EmpSalaryHdr.SalaryDtl = EmployeeSalaryList;
                        if (TempEmployeeSalaryList != null && TempEmployeeSalaryList.Count > 0)
                            EmpSalaryHdr.SalaryDtl.AddRange(TempEmployeeSalaryList.Where(Item => Item.PEL_IS_DEDUCTION != TypeVal).ToList());//for add deduction datas 

                        if (TypeVal == 0)
                            BindGrid(ActionsEnum.EARNINGS);
                        else
                            BindGrid(ActionsEnum.DEDUCTIONS);

                        break;
                    case BusinessObject.AccountManagement.ActionsEnum.MOVEDOWN:
                        SetUIValuesToObject(ActionsEnum.EMPLOYEESALARY);
                        if (((ImageButton)sender).ID == "imbRuleDownEarn")
                            TypeVal = 0;//earning
                        else
                            TypeVal = 1;//deduction

                        EmployeeSalaryList = EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == TypeVal).ToList();
                        //for take earnig datas
                        TempEmployeeSalaryList = EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION != TypeVal).ToList();
                        SlNo = Convert.ToInt32((sender as ImageButton).CommandArgument);
                        currLine = EmployeeSalaryList.Where(rl => rl.STS_SL_NO == SlNo).First();
                        lineIndex = EmployeeSalaryList.FindIndex(rl => rl.STS_SL_NO == SlNo);
                        nextLine = lineIndex == (EmployeeSalaryList.Count - 1) ? null : EmployeeSalaryList.ElementAt(lineIndex + 1);
                        if (nextLine != null)
                        {
                            int currentlineNumber = currLine.STS_SL_NO;
                            currLine.STS_SL_NO = nextLine.STS_SL_NO;
                            nextLine.STS_SL_NO = currentlineNumber;
                        }
                        EmployeeSalaryList = EmployeeSalaryList.OrderBy(rl => rl.STS_SL_NO).ToList();
                        EmpSalaryHdr.SalaryDtl = EmployeeSalaryList;

                        if (TempEmployeeSalaryList != null && TempEmployeeSalaryList.Count > 0)
                            EmpSalaryHdr.SalaryDtl.AddRange(TempEmployeeSalaryList.Where(Item => Item.PEL_IS_DEDUCTION != TypeVal).ToList());//for add deduction datas  

                        if (TypeVal == 0)
                            BindGrid(ActionsEnum.EARNINGS);
                        else
                            BindGrid(ActionsEnum.DEDUCTIONS);
                        break;
                    #endregion

                    #region EARNINGS FORMULA POPUP
                    case BusinessObject.AccountManagement.ActionsEnum.EARNFORMULAPOPUP:
                        IsApplyEarnFormula = 1;
                        lblEarnFormula = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("lblEarnFormula") as Label);
                        hdfEarnFormula = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfEarnFormula") as HiddenField);
                        lblEarnHead = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("lblEarnHead") as Label);
                        hdfEarnSlNo = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfEarnSlNo") as HiddenField);
                        hdfEarnSlNo = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfEarnSlNo") as HiddenField);
                        hdfEarnPayElement = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfEarnPayElement") as HiddenField);
                        ddlEarnPayElement = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("ddlEarnPayElement") as DropDownList);

                        hdfErnMinAmount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfErnMinAmount") as HiddenField);
                        hdfErnMaxAmount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfErnMaxAmount") as HiddenField);

                        SelectedEarnSeqNo = hdfEarnSlNo == null ? 0 : Convert.ToInt32(hdfEarnSlNo.Value);
                        PayElementPk = (ddlEarnPayElement.Visible) ? Convert.ToInt32(ddlEarnPayElement.SelectedValue) : Convert.ToInt32(hdfEarnPayElement.Value);
                        GetFieldValues(ActionsEnum.PAYELEMENTDETAILS);
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ucFormulaMaster.IsDeduction = Convert.ToInt32(dtResult.Rows[0]["PEL_IS_DEDUCTION"]);
                        }
                        decimal.TryParse(hdfErnMinAmount.Value, out minAmount);
                        decimal.TryParse(hdfErnMaxAmount.Value, out maxAmount);
                        ucFormulaMaster.GetFieldValues(Admin.Masters.UserControls.FormulaMaster.ControlsEnum.PAYELEMENT);
                        ucFormulaMaster.SetFieldValues(Admin.Masters.UserControls.FormulaMaster.ControlsEnum.PAYELEMENT);
                        strPayElement = ddlEarnPayElement.Visible ? ddlEarnPayElement.SelectedItem.Text : lblEarnHead.Text;
                        //ucFormulaMaster.SetData(lblEarnFormula.Text, strPayElement, minAmount, maxAmount);      
                        ucFormulaMaster.SetData(hdfEarnFormula.Value, strPayElement, minAmount, maxAmount); 
                        //ucFormulaMaster.SetData(lblEarnFormula.Text, lblEarnHead.Text);                       
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpFormula]','Formula','660','230');", true);
                        ShowEmpFormulaPopup();
                        break;
                    #endregion

                    #region DEDUCTION FORMULA POPUP
                    case BusinessObject.AccountManagement.ActionsEnum.DEDUCTFORMULAPOPUP:
                        IsApplyEarnFormula = 0;
                        lblDeductFormula = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("lblDeductFormula") as Label);
                        hdfDeductFormula = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfDeductFormula") as HiddenField);
                        lblDeductHead = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("lblDeductHead") as Label);
                        hdfDeductSlNo = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfDeductSlNo") as HiddenField);
                        hdfDeductPayElement = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfDeductPayElement") as HiddenField);
                        ddlDeductPayElement = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("ddlDeductPayElement") as DropDownList);
                        hdfDeductMinAmount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfDeductMinAmount") as HiddenField);
                        hdfDeductMaxAmount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfDeductMaxAmount") as HiddenField);

                        SelectedDeductSeqNo = hdfDeductSlNo == null ? 0 : Convert.ToInt32(hdfDeductSlNo.Value);
                        PayElementPk = ddlDeductPayElement.Visible ? Convert.ToInt32(ddlDeductPayElement.SelectedValue) : Convert.ToInt32(hdfDeductPayElement.Value);
                        GetFieldValues(ActionsEnum.PAYELEMENTDETAILS);
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ucFormulaMaster.IsDeduction = Convert.ToInt32(dtResult.Rows[0]["PEL_IS_DEDUCTION"]);
                        }
                        decimal.TryParse(hdfDeductMinAmount.Value, out minAmount);
                        decimal.TryParse(hdfDeductMaxAmount.Value, out maxAmount);
                        ucFormulaMaster.GetFieldValues(Admin.Masters.UserControls.FormulaMaster.ControlsEnum.PAYELEMENT);
                        ucFormulaMaster.SetFieldValues(Admin.Masters.UserControls.FormulaMaster.ControlsEnum.PAYELEMENT);
                        strPayElement = ddlDeductPayElement.Visible ? ddlDeductPayElement.SelectedItem.Text : lblDeductHead.Text;
                        //ucFormulaMaster.SetData(lblDeductFormula.Text, strPayElement, minAmount, maxAmount); 
                        ucFormulaMaster.SetData(hdfDeductFormula.Value, strPayElement, minAmount, maxAmount);
                        //ucFormulaMaster.SetData(lblDeductFormula.Text, lblDeductHead.Text);
                        ShowEmpFormulaPopup();
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpFormula]','Formula','660','200');", true);
                        break;
                    #endregion

                    #region ADD NEW EARNINGS
                    case BusinessObject.AccountManagement.ActionsEnum.ADDNEWEARNINGS:
                        SetUIValuesToObject(ActionsEnum.EMPLOYEESALARY);
                        if (EmpSalaryHdr != null)
                        {
                            if (EmpSalaryHdr.SalaryDtl == null)
                            {
                                EmpSalaryHdr.SalaryDtl = new List<EmpSalaryDetails>();
                            }
                            GetFieldValues(ActionsEnum.EARNINGSPAYELEMENTS);
                            EmpSalaryDetails objDtl = new EmpSalaryDetails();
                            objDtl.EDP_PK = 0;
                            objDtl.STS_FORMULA_CODE = string.Empty;
                            objDtl.STS_VALUE = (double)0;
                            objDtl.PEL_IS_DEDUCTION = Convert.ToByte(DeductMode.Earn);
                            objDtl.STS_SL_NO = GetCurrSequenceNo();
                            objDtl.STS_CALC_MODE = -1;
                            objDtl.STS_HAS_PAYROLL = 0;
                            objDtl.PEL_IN_SALARY = 1;
                            objDtl.PEL_SHOW_IN_EMPMAST = 1;
                            EmpSalaryHdr.SalaryDtl.Add(objDtl);
                            BindGrid(ActionsEnum.EARNINGS);
                        }
                        break;
                    #endregion

                    #region ADD NEW DEDUCTION
                    case BusinessObject.AccountManagement.ActionsEnum.ADDNEWDEDUCTION:
                        SetUIValuesToObject(ActionsEnum.EMPLOYEESALARY);
                        if (EmpSalaryHdr != null)
                        {
                            if (EmpSalaryHdr.SalaryDtl == null)
                            {
                                EmpSalaryHdr.SalaryDtl = new List<EmpSalaryDetails>();
                            }
                            GetFieldValues(ActionsEnum.DEDUCTIONPAYELEMENTS);
                            EmpSalaryDetails objDedDtl = new EmpSalaryDetails();
                            objDedDtl.EDP_PK = 0;
                            objDedDtl.STS_FORMULA_CODE = string.Empty;
                            objDedDtl.STS_VALUE = (double)0;
                            objDedDtl.PEL_IS_DEDUCTION = Convert.ToByte(DeductMode.Deduct);
                            objDedDtl.STS_SL_NO = GetCurrSequenceNo();
                            objDedDtl.STS_CALC_MODE = -1;
                            objDedDtl.STS_HAS_PAYROLL = 0;
                            objDedDtl.PEL_IN_SALARY = 1;
                            objDedDtl.PEL_SHOW_IN_EMPMAST = 1;
                            EmpSalaryHdr.SalaryDtl.Add(objDedDtl);
                            BindGrid(ActionsEnum.DEDUCTIONS);
                        }
                        break;
                    #endregion

                    #region REMOVE EARNINGS
                    case BusinessObject.AccountManagement.ActionsEnum.REMOVEEARNINGS:
                        SetUIValuesToObject(ActionsEnum.EMPLOYEESALARY);
                        if (EmpSalaryHdr.SalaryDtl != null && EmpSalaryHdr.SalaryDtl.Count > 0)
                        {
                            hdfEarnSlNo = (((sender as Button).Parent.Parent as GridViewRow).FindControl("hdfEarnSlNo") as HiddenField);
                            var removeEarn = EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 0).SingleOrDefault(row => row.STS_SL_NO == Convert.ToInt32(hdfEarnSlNo.Value));
                            if (removeEarn != null)
                            {
                                EmpSalaryHdr.SalaryDtl.Remove(removeEarn);
                                BindGrid(ActionsEnum.EARNINGS);                               
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

                    #region REMOVE DEDUCTION
                    case BusinessObject.AccountManagement.ActionsEnum.REMOVEDEDUCTION:
                        SetUIValuesToObject(ActionsEnum.EMPLOYEESALARY);
                        if (EmpSalaryHdr.SalaryDtl != null && EmpSalaryHdr.SalaryDtl.Count > 0)
                        {
                            hdfDeductSlNo = (((sender as Button).Parent.Parent as GridViewRow).FindControl("hdfDeductSlNo") as HiddenField);
                            var removeDeduct = EmpSalaryHdr.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 1).SingleOrDefault(row => row.STS_SL_NO == Convert.ToInt32(hdfDeductSlNo.Value));
                            if (removeDeduct != null)
                            {
                                EmpSalaryHdr.SalaryDtl.Remove(removeDeduct);
                                BindGrid(ActionsEnum.DEDUCTIONS);                               
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

                    #region CHANGE EARNING DROPDOWN
                    case BusinessObject.AccountManagement.ActionsEnum.EARNINGCHANGE:
                        grvRow = (sender as DropDownList).Parent.Parent as GridViewRow;
                        //ddlEarnPayElement = (((sender as DropDownList).Parent.Parent as GridViewRow).FindControl("ddlEarnPayElement") as DropDownList);
                        //hdfEarnSlNo = (((sender as DropDownList).Parent.Parent as GridViewRow).FindControl("hdfEarnSlNo") as HiddenField);
                        ddlEarnPayElement = grvRow.FindControl("ddlEarnPayElement") as DropDownList;
                        hdfEarnSlNo = grvRow.FindControl("hdfEarnSlNo") as HiddenField;
                        //imgPayMode = grvRow.FindControl("imgEarnMode") as Image;
                        imgPayMode = grvRow.FindControl("divEarnMode") as HtmlControl;
                        HiddenField hdfEarnCalcMode = grvRow.FindControl("hdfEarnCalcMode") as HiddenField;
                        HiddenField hdfEarnFormulaCode=    grvRow.FindControl("hdfEarnFormulaCode") as HiddenField;
                        HiddenField hdfEarnCalcValue = grvRow.FindControl("hdfEarnCalcValue") as HiddenField;
                        Label lblErnFormula = grvRow.FindControl("lblEarnFormula") as Label;
                        HiddenField hdfErnFormula = grvRow.FindControl("hdfEarnFormula") as HiddenField;
                        TextBox txtEarnAmount = grvRow.FindControl("txtEarnAmount") as TextBox;
                        DropDownList ddlErnSlabCustom = grvRow.FindControl("ddlErnSlabCustom") as DropDownList;
                        ImageButton imbFormula = grvRow.FindControl("imbFormula") as ImageButton;
                        RequiredFieldValidator rfvErnSlabCustom = grvRow.FindControl("rfvErnSlabCustom") as RequiredFieldValidator;
                        HiddenField hdfEarnPayElmtInSalary = grvRow.FindControl("hdfEarnPayElmtInSalary") as HiddenField;
                        HiddenField hdfEarnShowPayElement = grvRow.FindControl("hdfEarnShowPayElement") as HiddenField;
                        HiddenField hdfErnPartofCTC = grvRow.FindControl("hdfErnPartofCTC") as HiddenField;
                        HiddenField hdfErnPartofGross = grvRow.FindControl("hdfErnPartofGross") as HiddenField;

                        lblEarnHead = grvRow.FindControl("lblEarnHead") as Label;
                        txtEarnAmount.Text = GetFormattedCurrency(0);
                        PayElementPk = Convert.ToInt32(ddlEarnPayElement.SelectedValue);

                        lblErnFormula.Text = string.Empty;
                        hdfErnFormula.Value = string.Empty;
                        hdfEarnFormulaCode.Value = string.Empty;
                        hdfEarnCalcValue.Value = string.Empty;
                        hdfEarnCalcMode.Value = CommonConstants.SELECTVAL;
                        ddlErnSlabCustom.Visible = false;
                        rfvErnSlabCustom.Enabled = false;
                        imbFormula.Visible = false;

                        if (((DropDownList)sender).SelectedValue != CommonConstants.SELECTVAL)
                        {
                            var varSalaryDtlLst = EmpSalaryHdr.SalaryDtl.Where(x => x.STS_PAY_ELEMENT == PayElementPk && x.STS_SL_NO != Convert.ToInt32(hdfEarnSlNo.Value)).ToList();
                            if (varSalaryDtlLst != null && varSalaryDtlLst.Count > 0)
                            {
                                ddlEarnPayElement.SelectedValue = CommonConstants.SELECTVAL;
                                //lblErnFormula.Text = string.Empty;
                                //hdfEarnFormulaCode.Value = string.Empty;
                                //hdfEarnCalcValue.Value = string.Empty;
                                //hdfEarnCalcMode.Value = CommonConstants.SELECTVAL;
                                //ddlErnSlabCustom.Visible = false;
                                //imbFormula.Visible = false;
                                litErrorMsg.Text = string.Format(Resources.ErrorMessages.Msg_PayElementAlreadyExists, varSalaryDtlLst[0].STS_PAY_ELEMENT_TEXT);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" +
                                    CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                            else
                            {
                                EmpSalaryDetails objSalDtl = EmpSalaryHdr.SalaryDtl.Where(row => row.STS_SL_NO == Convert.ToInt32(hdfEarnSlNo.Value) && row.PEL_IS_DEDUCTION == (int)DeductMode.Earn).SingleOrDefault();
                                //EmpSalaryHdr.SalaryDtl.Where(row => row.STS_SL_NO == Convert.ToInt32(hdfEarnSlNo.Value)).SingleOrDefault().STS_PAY_ELEMENT = PayElementPk;
                                if (objSalDtl != null)
                                {
                                    objSalDtl.STS_PAY_ELEMENT = PayElementPk;
                                    salaryTemplatePk = EmpSalaryHdr.STE_PK;
                                    GetFieldValues(ActionsEnum.SALARYTEMPLATEDETAILS);
                                    if (objEmpSalaryDetails != null)
                                    {
                                        ddlErnSlabCustom.Visible = false;
                                        rfvErnSlabCustom.Enabled = false;
                                        EmpSalaryDetails objSalDetail = objEmpSalaryDetails.SalaryDtl.Where(x => x.STS_PAY_ELEMENT == PayElementPk).SingleOrDefault();
                                        if (objSalDetail != null)
                                        {
                                            lblErnFormula.Text = objSalDetail.STS_VALUE_TEXT;
                                            hdfErnFormula.Value = objSalDetail.STS_VALUE_TEXT;
                                            hdfEarnFormulaCode.Value = objSalDetail.STS_FORMULA_CODE;
                                            hdfEarnCalcValue.Value = objSalDetail.STS_CALC_VALUE;
                                            hdfEarnCalcMode.Value = objSalDetail.STS_CALC_MODE.ToString();
                                            lblEarnHead.Text = objSalDetail.STS_PAY_ELEMENT_TEXT;

                                            objSalDtl.STS_VALUE_TEXT = objSalDetail.STS_VALUE_TEXT;
                                            objSalDtl.STS_FORMULA_CODE = objSalDetail.STS_FORMULA_CODE;
                                            objSalDtl.STS_CALC_VALUE = objSalDetail.STS_CALC_VALUE;
                                            objSalDtl.STS_CALC_MODE = objSalDetail.STS_CALC_MODE;
                                            objSalDtl.PEL_IN_SALARY = objSalDetail.PEL_IN_SALARY;
                                            objSalDtl.PEL_IN_CTC = objSalDetail.PEL_IN_CTC;
                                            objSalDtl.PEL_IN_GROSS = objSalDetail.PEL_IN_GROSS;
                                            objSalDtl.PEL_SHOW_IN_EMPMAST = objSalDetail.PEL_SHOW_IN_EMPMAST;

                                            hdfEarnPayElmtInSalary.Value = objSalDetail.PEL_IN_SALARY.ToString();
                                            hdfErnPartofCTC.Value = objSalDetail.PEL_IN_CTC.ToString();
                                            hdfErnPartofGross.Value = objSalDetail.PEL_IN_GROSS.ToString();
                                            hdfEarnShowPayElement.Value = objSalDetail.PEL_SHOW_IN_EMPMAST.ToString();
                                            if (Convert.ToInt32(hdfEarnPayElmtInSalary.Value) == 1)
                                            {
                                                grvRow.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "RowColor").ToString());
                                            }
                                            else
                                            {
                                                grvRow.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "PayElmtNotInSalary").ToString());
                                            }


                                            if (objSalDetail.STS_CALC_MODE == (int)PayElementCalcMode.FixedAmount)
                                                txtEarnAmount.Text = objSalDetail.STS_CALC_VALUE;
                                            else if (objSalDetail.STS_CALC_MODE == (int)PayElementCalcMode.Formula)
                                            {
                                                if (objSalDetail.PEL_IS_FORMULA_EDITABLE == 1)
                                                    imbFormula.Visible = true;
                                                else if (string.IsNullOrEmpty(objSalDetail.STS_CALC_VALUE))
                                                    imbFormula.Visible = true;
                                            }
                                            else if (objSalDetail.STS_CALC_MODE == (int)PayElementCalcMode.Slab || objSalDetail.STS_CALC_MODE == (int)PayElementCalcMode.Custom)
                                            {
                                                lblErnFormula.Text = string.Empty;
                                                hdfErnFormula.Value = string.Empty;
                                                hdfEarnCalcValue.Value = string.Empty;
                                                ddlErnSlabCustom.Visible = true;
                                                rfvErnSlabCustom.Enabled = true;
                                                GetFieldValues(ActionsEnum.PAYELEMENTSLABCUSTOM);
                                                BindDropDown(ActionsEnum.PAYELEMENTSLABCUSTOM, ddlErnSlabCustom);
                                                if (ddlErnSlabCustom.Items.FindByValue(objSalDtl.STS_CALC_VALUE) != null)
                                                    ddlErnSlabCustom.SelectedValue = objSalDtl.STS_CALC_VALUE;
                                            }
                                        }
                                        else
                                        {
                                            //imbFormula.Visible = false;
                                            //lblErnFormula.Text = string.Empty;
                                            //hdfEarnCalcValue.Value = string.Empty;
                                            GetFieldValues(ActionsEnum.PAYELEMENTDETAILS);
                                            if (dtResult != null && dtResult.Rows.Count > 0)
                                            {
                                                hdfEarnPayElmtInSalary.Value = dtResult.Rows[0]["PEL_IN_SALARY"].ToString();
                                                hdfErnPartofCTC.Value = dtResult.Rows[0]["PEL_IN_CTC"].ToString();
                                                hdfErnPartofGross.Value = dtResult.Rows[0]["PEL_IN_Gross"].ToString();
                                                if (Convert.ToInt32(hdfEarnPayElmtInSalary.Value) == 1)
                                                {
                                                    grvRow.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "RowColor").ToString());
                                                }
                                                else
                                                {
                                                    grvRow.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "PayElmtNotInSalary").ToString());
                                                }
                                                hdfEarnCalcMode.Value = dtResult.Rows[0]["PEL_CALC_MODE"].ToString();
                                                if (Convert.ToInt32(dtResult.Rows[0]["PEL_CALC_MODE"]) == (int)PayElementCalcMode.Custom || Convert.ToInt32(dtResult.Rows[0]["PEL_CALC_MODE"]) == (int)PayElementCalcMode.Slab)
                                                {
                                                    ddlErnSlabCustom.Visible = true;
                                                    rfvErnSlabCustom.Enabled = true;
                                                    GetFieldValues(ActionsEnum.PAYELEMENTSLABCUSTOM);
                                                    BindDropDown(ActionsEnum.PAYELEMENTSLABCUSTOM, ddlErnSlabCustom);
                                                }
                                                else if (Convert.ToInt32(dtResult.Rows[0]["PEL_CALC_MODE"]) == (int)PayElementCalcMode.Formula)
                                                {
                                                    imbFormula.Visible = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            EmpSalaryHdr.SalaryDtl.Where(row => row.STS_SL_NO == Convert.ToInt32(hdfEarnSlNo.Value) && row.PEL_IS_DEDUCTION == (int)DeductMode.Earn).SingleOrDefault().STS_PAY_ELEMENT = 0;
                            //lblErnFormula.Text = string.Empty;
                            //hdfEarnFormulaCode.Value = string.Empty;
                            //hdfEarnCalcValue.Value = string.Empty;
                            //hdfEarnCalcMode.Value = CommonConstants.SELECTVAL;
                            //ddlErnSlabCustom.Visible = false;
                            //imbFormula.Visible = false;
                        }                       
                        int.TryParse(hdfEarnCalcMode.Value, out payElementMode);
                        SetPayelementIconCss(payElementMode, imgPayMode);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalTotalEarn", "CalculateTotalEarnings();", true);
                        break;
                    #endregion

                    #region CHANGE DEDUCTION DROPDOWN
                    case BusinessObject.AccountManagement.ActionsEnum.DEDUCTIONCHANGE:
                        grvRow = (sender as DropDownList).Parent.Parent as GridViewRow;
                        //ddlDeductPayElement = (((sender as DropDownList).Parent.Parent as GridViewRow).FindControl("ddlDeductPayElement") as DropDownList);
                        //hdfDeductSlNo = (((sender as DropDownList).Parent.Parent as GridViewRow).FindControl("hdfDeductSlNo") as HiddenField);
                        ddlDeductPayElement = grvRow.FindControl("ddlDeductPayElement") as DropDownList;
                        hdfDeductSlNo = grvRow.FindControl("hdfDeductSlNo") as HiddenField;
                        //imgPayMode = grvRow.FindControl("imgDeductMode") as Image;
                        imgPayMode = grvRow.FindControl("divDeductMode") as HtmlControl;
                        HiddenField hdfDeductCalcMode = grvRow.FindControl("hdfDeductCalcMode") as HiddenField;
                        HiddenField hdfDeductFormulaCode = grvRow.FindControl("hdfDeductFormulaCode") as HiddenField;
                        HiddenField hdfDeductCalcValue = grvRow.FindControl("hdfDeductCalcValue") as HiddenField; 
                        Label lblDedctFormula = grvRow.FindControl("lblDeductFormula") as Label;
                        HiddenField hdfDedctFormula = grvRow.FindControl("hdfDeductFormula") as HiddenField;
                        TextBox txtDeductAmount = grvRow.FindControl("txtDeductAmount") as TextBox;
                        DropDownList ddlDeductSlabCustom = grvRow.FindControl("ddlDeductSlabCustom") as DropDownList;
                        ImageButton imbDeductFormula = grvRow.FindControl("imbDeductFormula") as ImageButton;
                        RequiredFieldValidator rfvDeductSlabCustom = grvRow.FindControl("rfvDeductSlabCustom") as RequiredFieldValidator;
                        HiddenField hdfDedPayElmtInSalary = grvRow.FindControl("hdfDedPayElmtInSalary") as HiddenField;
                        HiddenField hdfDedShowPayElement = grvRow.FindControl("hdfDedShowPayElement") as HiddenField;
                        HiddenField hdfDedPartofCTC = grvRow.FindControl("hdfDedPartofCTC") as HiddenField;
                        HiddenField hdfDedPartofGross = grvRow.FindControl("hdfDedPartofGross") as HiddenField;

                        lblDeductHead = grvRow.FindControl("lblDeductHead") as Label;
                        txtDeductAmount.Text = GetFormattedCurrency(0);
                        PayElementPk = Convert.ToInt32(ddlDeductPayElement.SelectedValue);

                        lblDedctFormula.Text = string.Empty;
                        hdfDedctFormula.Value = string.Empty;
                        hdfDeductFormulaCode.Value = string.Empty;
                        hdfDeductCalcValue.Value = string.Empty;
                        hdfDeductCalcMode.Value = CommonConstants.SELECTVAL;
                        ddlDeductSlabCustom.Visible = false;
                        rfvDeductSlabCustom.Enabled = false;
                        imbDeductFormula.Visible = false;

                        if (((DropDownList)sender).SelectedValue != CommonConstants.SELECTVAL)
                        {
                            var varSalaryDtlLst = EmpSalaryHdr.SalaryDtl.Where(x => x.STS_PAY_ELEMENT == PayElementPk && x.STS_SL_NO != Convert.ToInt32(hdfDeductSlNo.Value)).ToList();
                            if (varSalaryDtlLst != null && varSalaryDtlLst.Count > 0)
                            {
                                ddlDeductPayElement.SelectedValue = CommonConstants.SELECTVAL;
                                //lblDedctFormula.Text = string.Empty;
                                //hdfDeductFormulaCode.Value = string.Empty;
                                //hdfDeductCalcValue.Value = string.Empty;
                                //hdfDeductCalcMode.Value = CommonConstants.SELECTVAL;
                                //ddlDeductSlabCustom.Visible = false;
                                //imbDeductFormula.Visible = false;
                                litErrorMsg.Text = string.Format(Resources.ErrorMessages.Msg_PayElementAlreadyExists, varSalaryDtlLst[0].STS_PAY_ELEMENT_TEXT);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" +
                                    CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                            }
                            else
                            {
                                EmpSalaryDetails objSalDtl = EmpSalaryHdr.SalaryDtl.Where(row => row.STS_SL_NO == Convert.ToInt32(hdfDeductSlNo.Value) && row.PEL_IS_DEDUCTION == (int)DeductMode.Deduct).SingleOrDefault();
                                // EmpSalaryHdr.SalaryDtl.Where(row => row.STS_SL_NO == Convert.ToInt32(hdfDeductSlNo.Value)).SingleOrDefault().STS_PAY_ELEMENT = Convert.ToInt32(ddlDeductPayElement.SelectedValue);
                                if (objSalDtl != null)
                                {
                                    objSalDtl.STS_PAY_ELEMENT = PayElementPk;
                                    salaryTemplatePk = EmpSalaryHdr.STE_PK;
                                    GetFieldValues(ActionsEnum.SALARYTEMPLATEDETAILS);
                                    if (objEmpSalaryDetails != null)
                                    {
                                        ddlDeductSlabCustom.Visible = false;
                                        rfvDeductSlabCustom.Enabled = false;
                                        EmpSalaryDetails objSalDetail = objEmpSalaryDetails.SalaryDtl.Where(x => x.STS_PAY_ELEMENT == PayElementPk).SingleOrDefault();
                                        if (objSalDetail != null)
                                        {
                                            lblDedctFormula.Text = objSalDetail.STS_VALUE_TEXT;
                                            hdfDedctFormula.Value = objSalDetail.STS_VALUE_TEXT;
                                            hdfDeductFormulaCode.Value = objSalDetail.STS_FORMULA_CODE;
                                            hdfDeductCalcValue.Value = objSalDetail.STS_CALC_VALUE;
                                            hdfDeductCalcMode.Value = objSalDetail.STS_CALC_MODE.ToString();
                                            lblDeductHead.Text = objSalDetail.STS_PAY_ELEMENT_TEXT;

                                            objSalDtl.STS_VALUE_TEXT = objSalDetail.STS_VALUE_TEXT;
                                            objSalDtl.STS_FORMULA_CODE = objSalDetail.STS_FORMULA_CODE;
                                            objSalDtl.STS_CALC_VALUE = objSalDetail.STS_CALC_VALUE;
                                            objSalDtl.STS_CALC_MODE = objSalDetail.STS_CALC_MODE;
                                            objSalDtl.PEL_IN_SALARY = objSalDetail.PEL_IN_SALARY;
                                            objSalDtl.PEL_IN_CTC = objSalDetail.PEL_IN_CTC;
                                            objSalDtl.PEL_IN_GROSS = objSalDetail.PEL_IN_GROSS;
                                            objSalDtl.PEL_SHOW_IN_EMPMAST = objSalDetail.PEL_SHOW_IN_EMPMAST;

                                            hdfDedPayElmtInSalary.Value = objSalDetail.PEL_IN_SALARY.ToString();
                                            hdfDedPartofCTC.Value = objSalDetail.PEL_IN_CTC.ToString();
                                            hdfDedPartofGross.Value = objSalDetail.PEL_IN_GROSS.ToString();
                                            hdfDedShowPayElement.Value = objSalDetail.PEL_SHOW_IN_EMPMAST.ToString();
                                            if (Convert.ToInt32(hdfDedPayElmtInSalary.Value) == 1)
                                            {
                                                grvRow.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "RowColor").ToString());
                                            }
                                            else
                                            {
                                                grvRow.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "PayElmtNotInSalary").ToString());
                                            }
                                            

                                            if (objSalDetail.STS_CALC_MODE == (int)PayElementCalcMode.FixedAmount)
                                                txtDeductAmount.Text = objSalDetail.STS_CALC_VALUE;
                                            else if (objSalDetail.STS_CALC_MODE == (int)PayElementCalcMode.Formula)
                                            {
                                                if (objSalDetail.PEL_IS_FORMULA_EDITABLE == 1)
                                                    imbDeductFormula.Visible = true;
                                                 else if (string.IsNullOrEmpty(objSalDetail.STS_CALC_VALUE))
                                                    imbDeductFormula.Visible = true;
                                            }
                                            else if (objSalDetail.STS_CALC_MODE == (int)PayElementCalcMode.Slab || objSalDetail.STS_CALC_MODE == (int)PayElementCalcMode.Custom)
                                            {
                                                lblDedctFormula.Text = string.Empty;
                                                hdfDedctFormula.Value = string.Empty;
                                                hdfDeductCalcValue.Value = string.Empty;
                                                ddlDeductSlabCustom.Visible = true;
                                                rfvDeductSlabCustom.Enabled = true;
                                                GetFieldValues(ActionsEnum.PAYELEMENTSLABCUSTOM);
                                                BindDropDown(ActionsEnum.PAYELEMENTSLABCUSTOM, ddlDeductSlabCustom);
                                                if (ddlDeductSlabCustom.Items.FindByValue(objSalDtl.STS_CALC_VALUE) != null)
                                                    ddlDeductSlabCustom.SelectedValue = objSalDtl.STS_CALC_VALUE;
                                            }
                                        }
                                        else
                                        {
                                            //imbDeductFormula.Visible = false;
                                            //lblDedctFormula.Text = string.Empty;
                                            //hdfDeductCalcValue.Value = string.Empty;
                                            GetFieldValues(ActionsEnum.PAYELEMENTDETAILS);
                                            if (dtResult != null && dtResult.Rows.Count > 0)
                                            {
                                                hdfDedPayElmtInSalary.Value = dtResult.Rows[0]["PEL_IN_SALARY"].ToString();
                                                hdfDedPartofCTC.Value = dtResult.Rows[0]["PEL_IN_CTC"].ToString();
                                                hdfDedPartofGross.Value = dtResult.Rows[0]["PEL_IN_Gross"].ToString();

                                                if (Convert.ToInt32(hdfDedPayElmtInSalary.Value) == 1)
                                                {
                                                    grvRow.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "RowColor").ToString());
                                                }
                                                else
                                                {
                                                    grvRow.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "PayElmtNotInSalary").ToString());
                                                }
                                                hdfDeductCalcMode.Value = dtResult.Rows[0]["PEL_CALC_MODE"].ToString();
                                                if (Convert.ToInt32(dtResult.Rows[0]["PEL_CALC_MODE"]) == (int)PayElementCalcMode.Custom || Convert.ToInt32(dtResult.Rows[0]["PEL_CALC_MODE"]) == (int)PayElementCalcMode.Slab)
                                                {
                                                    ddlDeductSlabCustom.Visible = true;
                                                    rfvDeductSlabCustom.Enabled = true;
                                                    GetFieldValues(ActionsEnum.PAYELEMENTSLABCUSTOM);
                                                    BindDropDown(ActionsEnum.PAYELEMENTSLABCUSTOM, ddlDeductSlabCustom);
                                                }
                                                else if (Convert.ToInt32(dtResult.Rows[0]["PEL_CALC_MODE"]) == (int)PayElementCalcMode.Formula)
                                                {
                                                    imbDeductFormula.Visible = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }                           
                        }
                        else
                        {
                            EmpSalaryHdr.SalaryDtl.Where(row => row.STS_SL_NO == Convert.ToInt32(hdfDeductSlNo.Value) && row.PEL_IS_DEDUCTION == (int)DeductMode.Deduct).SingleOrDefault().STS_PAY_ELEMENT = 0;
                            //lblDedctFormula.Text = string.Empty;
                            //hdfDeductFormulaCode.Value = string.Empty;
                            //hdfDeductCalcValue.Value = string.Empty;
                            //hdfDeductCalcMode.Value = CommonConstants.SELECTVAL;
                            //ddlDeductSlabCustom.Visible = false;
                            //imbDeductFormula.Visible = false;
                        }
                        int.TryParse(hdfDeductCalcMode.Value, out payElementMode);
                        SetPayelementIconCss(payElementMode, imgPayMode);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalTotalDeduct", "CalculateTotalDeductions();", true);
                        break;
                    #endregion

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        private void ShowEmpFormulaPopup()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpFormula]','Formula','660','230');", true);
        }

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (((GridView)sender).ID == "grdEarnings")
                {
                    if (e.Row.RowType == DataControlRowType.Header && IsEmpAppraisal == Convert.ToInt32(VisbleStatusEnum.TRUE))
                    {
                        if (IsEmpAppraisal == Convert.ToInt32(VisbleStatusEnum.TRUE))
                        {
                            e.Row.Cells[5].Text = Resources.Controls.AmountOld.ToString();
                            e.Row.Cells[6].Text = Resources.Controls.AmountNew.ToString();
                        }
                    }
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        DropDownList ddlEarnPayElement = e.Row.FindControl("ddlEarnPayElement") as DropDownList;
                        DropDownList ddlErnSlabCustom = e.Row.FindControl("ddlErnSlabCustom") as DropDownList;
                        HiddenField hdfEarnPK = e.Row.FindControl("hdfEarnPK") as HiddenField;
                        Label lblEarnHead = e.Row.FindControl("lblEarnHead") as Label;
                        Label lblEarnFormula = e.Row.FindControl("lblEarnFormula") as Label;
                        HiddenField hdfEarnFormula = e.Row.FindControl("hdfEarnFormula") as HiddenField;
                        HiddenField hdfEarnSlNo = e.Row.FindControl("hdfEarnSlNo") as HiddenField;
                        HiddenField hdfEarnPayElementPK = e.Row.FindControl("hdfEarnPayElementPK") as HiddenField;
                        HiddenField hdfEarnCalcMode = e.Row.FindControl("hdfEarnCalcMode") as HiddenField;
                        HiddenField hdfEarnCalcValue = e.Row.FindControl("hdfEarnCalcValue") as HiddenField;
                        HiddenField hdfEarnPayElmtInSalary = e.Row.FindControl("hdfEarnPayElmtInSalary") as HiddenField;
                        HiddenField hdfErnEID_IS_DELETE = e.Row.FindControl("hdfErnEID_IS_DELETE") as HiddenField;
                        HiddenField hdfEID_PK = e.Row.FindControl("hdfEID_PK") as HiddenField;    //Appraisal PK
                        //Image imgEarnMode = e.Row.FindControl("imgEarnMode") as Image;
                        HtmlControl divEarnMode = e.Row.FindControl("divEarnMode") as HtmlControl;
                        RequiredFieldValidator rfvErnSlabCustom = e.Row.FindControl("rfvErnSlabCustom") as RequiredFieldValidator;

                        if (Convert.ToInt32(hdfEarnPK.Value) > 0 || (Convert.ToInt32(hdfEarnPayElementPK.Value) > 0 || Convert.ToInt32(hdfEID_PK.Value) > 0 && Convert.ToInt32(hdfEarnPK.Value) == 0))
                        {
                            ddlEarnPayElement.Visible = false;
                            lblEarnHead.Visible = true;
                            ddlErnSlabCustom.Visible = false;
                            rfvErnSlabCustom.Enabled = false;
                        }
                        else
                        {
                            ddlEarnPayElement.Visible = true;
                            lblEarnHead.Visible = false;
                            BindDropDown(ActionsEnum.EARNINGSPAYELEMENTS, ddlEarnPayElement);
                            // ddlEarnPayElement.SelectedValue = EmpSalaryHdr.SalaryDtl.Where(row => row.STS_SL_NO == Convert.ToInt32(hdfEarnSlNo.Value) && row.PEL_IS_DEDUCTION == 0).SingleOrDefault().STS_PAY_ELEMENT.ToString();
                            EmpSalaryDetails objSalDet = EmpSalaryHdr.SalaryDtl.Where(row => row.STS_SL_NO == Convert.ToInt32(hdfEarnSlNo.Value) && row.PEL_IS_DEDUCTION == 0).SingleOrDefault();
                            if (objSalDet != null)
                            {
                                ddlEarnPayElement.SelectedValue = objSalDet.STS_PAY_ELEMENT.ToString();
                                if (objSalDet.STS_CALC_MODE == (int)PayElementCalcMode.Custom || objSalDet.STS_CALC_MODE == (int)PayElementCalcMode.Slab)
                                {
                                    lblEarnFormula.Text = string.Empty;
                                    hdfEarnFormula.Value = string.Empty;
                                    hdfEarnCalcValue.Value = string.Empty;
                                    ddlErnSlabCustom.Visible = true;
                                    rfvErnSlabCustom.Enabled = true;
                                    PayElementPk = objSalDet.STS_PAY_ELEMENT;
                                    GetFieldValues(ActionsEnum.PAYELEMENTSLABCUSTOM);
                                    BindDropDown(ActionsEnum.PAYELEMENTSLABCUSTOM, ddlErnSlabCustom);
                                    ddlErnSlabCustom.SelectedValue = objSalDet.STS_CALC_VALUE;
                                }
                            }
                        }
                        SetPayelementIconCss(Convert.ToInt32(hdfEarnCalcMode.Value), divEarnMode);

                        if (Convert.ToInt32(hdfEarnPayElmtInSalary.Value) == 1)
                        {
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "RowColor").ToString());
                        }
                        else
                        {
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "PayElmtNotInSalary").ToString());
                        }
                       // if (Convert.ToInt32(hdfErnEID_IS_DELETE.Value) == 1)
                           // e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "ElmtDeleteIsAppraisal").ToString());
                    }
                }
                if (((GridView)sender).ID == "grdDeductions")
                {
                    if (e.Row.RowType == DataControlRowType.Header && IsEmpAppraisal == Convert.ToInt32(VisbleStatusEnum.TRUE))
                    {
                        if (IsEmpAppraisal == Convert.ToInt32(VisbleStatusEnum.TRUE))
                        {
                            e.Row.Cells[5].Text = Resources.Controls.AmountOld.ToString();
                            e.Row.Cells[6].Text = Resources.Controls.AmountNew.ToString();
                        }
                    }
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        DropDownList ddlDeductPayElement = e.Row.FindControl("ddlDeductPayElement") as DropDownList;
                        DropDownList ddlDeductSlabCustom = e.Row.FindControl("ddlDeductSlabCustom") as DropDownList;
                        HiddenField hdfDeductPK = e.Row.FindControl("hdfDeductPK") as HiddenField;
                        Label lblDeductHead = e.Row.FindControl("lblDeductHead") as Label;
                        Label lblDeductFormula = e.Row.FindControl("lblDeductFormula") as Label;
                        HiddenField hdfDedctFormula = e.Row.FindControl("hdfDeductFormula") as HiddenField;
                        HiddenField hdfDeductSlNo = e.Row.FindControl("hdfDeductSlNo") as HiddenField;
                        HiddenField hdfDeductPayElementPK = e.Row.FindControl("hdfDeductPayElementPK") as HiddenField;
                        HiddenField hdfDeductCalcMode = e.Row.FindControl("hdfDeductCalcMode") as HiddenField;
                        HiddenField hdfDeductCalcValue = e.Row.FindControl("hdfDeductCalcValue") as HiddenField;
                        HiddenField hdfDedPayElmtInSalary = e.Row.FindControl("hdfDedPayElmtInSalary") as HiddenField;
                        HiddenField hdfDedEID_PK = e.Row.FindControl("hdfDedEID_PK") as HiddenField;   //Appraisal PK
                        HiddenField hdfDedEID_IS_DELETE = e.Row.FindControl("hdfDedEID_IS_DELETE") as HiddenField;
                        //Image imgDeductMode = e.Row.FindControl("imgDeductMode") as Image;
                        HtmlControl divDeductMode = e.Row.FindControl("divDeductMode") as HtmlControl;
                        RequiredFieldValidator rfvDeductSlabCustom = e.Row.FindControl("rfvDeductSlabCustom") as RequiredFieldValidator;
                        if (Convert.ToInt32(hdfDeductPK.Value) > 0 || (Convert.ToInt32(hdfDeductPayElementPK.Value) > 0 || Convert.ToInt32(hdfDedEID_PK.Value) > 0 && Convert.ToInt32(hdfDeductPK.Value) == 0))
                        {
                            ddlDeductPayElement.Visible = false;
                            lblDeductHead.Visible = true;
                            ddlDeductSlabCustom.Visible = false;
                            rfvDeductSlabCustom.Enabled = false;
                        }
                        else
                        {
                            ddlDeductPayElement.Visible = true;
                            lblDeductHead.Visible = false;
                            BindDropDown(ActionsEnum.DEDUCTIONPAYELEMENTS, ddlDeductPayElement);
                            //ddlDeductPayElement.SelectedValue = EmpSalaryHdr.SalaryDtl.Where(row => row.STS_SL_NO == Convert.ToInt32(hdfDeductSlNo.Value) && row.PEL_IS_DEDUCTION != 0).SingleOrDefault().STS_PAY_ELEMENT.ToString();
                            EmpSalaryDetails objSalDet = EmpSalaryHdr.SalaryDtl.Where(row => row.STS_SL_NO == Convert.ToInt32(hdfDeductSlNo.Value) && row.PEL_IS_DEDUCTION != 0).SingleOrDefault();
                            if (objSalDet != null)
                            {
                                ddlDeductPayElement.SelectedValue = objSalDet.STS_PAY_ELEMENT.ToString();
                                if (objSalDet.STS_CALC_MODE == (int)PayElementCalcMode.Custom || objSalDet.STS_CALC_MODE == (int)PayElementCalcMode.Slab)
                                {
                                    lblDeductFormula.Text = string.Empty;
                                    hdfDedctFormula.Value = string.Empty;
                                    hdfDeductCalcValue.Value = string.Empty;
                                    ddlDeductSlabCustom.Visible = true;
                                    rfvDeductSlabCustom.Enabled = true;
                                    PayElementPk = objSalDet.STS_PAY_ELEMENT;
                                    GetFieldValues(ActionsEnum.PAYELEMENTSLABCUSTOM);
                                    BindDropDown(ActionsEnum.PAYELEMENTSLABCUSTOM, ddlDeductSlabCustom);
                                    ddlDeductSlabCustom.SelectedValue = objSalDet.STS_CALC_VALUE;
                                }
                            }
                        }
                        SetPayelementIconCss(Convert.ToInt32(hdfDeductCalcMode.Value), divDeductMode);
                        
                        if (Convert.ToInt32(hdfDedPayElmtInSalary.Value) == 1)
                        {
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "RowColor").ToString());
                        }
                        else
                        {
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "PayElmtNotInSalary").ToString());
                        }
                       // if (Convert.ToInt32(hdfDedEID_IS_DELETE.Value) == 1)
                          //  e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "ElmtDeleteIsAppraisal").ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetUserRights()
        {
           
            CommonBL userAuth = new CommonBL();
            string path = "/Employees/EmployeeSalary.aspx";
            if (currentUser == null)
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            UserRightsBO usrRights = userAuth.GetUserRights(currentUser.PKUser, path, currentUser.CurrentSBUPK, currentUser.CurrentDeptPK);
            if (usrRights.Rights.Count > 0)
            {
                for (int i = 0; i < usrRights.Rights.Count; i++)
                {
                    if (usrRights.Rights[i].ActionName == "HIDE" && usrRights.Rights[i].HasActionRight == true && usrRights.Rights[i].UserDeptRight == true)
                    {
                        payElementHide = true;
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Set image css
        /// </summary>
        /// <param name="PayElementMode"></param>
        /// <param name="imgMode"></param>
        private void SetPayelementIconCss(int PayElementMode, HtmlControl imgMode)
        {
            switch (PayElementMode)
            {
                case (int)PayElementCalcMode.Custom:
                    imgMode.Attributes.Add("class","custom-icon");
                    imgMode.Attributes.Add("title", Resources.Controls.PayElementMode_Custom);                   
                    break;
                case (int)PayElementCalcMode.FixedAmount:
                    imgMode.Attributes.Add("class", "fixed-amount-icon");
                    imgMode.Attributes.Add("title", Resources.Controls.PayElementMode_FixedAmount);                   
                    break;
                case (int)PayElementCalcMode.Formula:
                    imgMode.Attributes.Add("class", "formula-icon");
                    imgMode.Attributes.Add("title", Resources.Controls.PayElementMode_Formula);                   
                    break;
                case (int)PayElementCalcMode.Slab:
                    imgMode.Attributes.Add("class", "slab-icon");
                    imgMode.Attributes.Add("title", Resources.Controls.PayElementMode_Slab);                   
                    break;
                default:
                    imgMode.Attributes.Add("class", "hide");                    
                    break;
            }
        }
        #endregion

        #region Page Events
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            currentUser = (BusinessObject.User)Context.User.Identity;
            if (!IsPostBack)
            {
                hdfCurrentDepartment.Value = currentUser.CurrentDeptPK.ToString();
            }
            SetControlVisibility();
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_SerialNoOrdering", "SerialNoOrdering();", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalTotalEarn", "CalculateTotalEarnings();", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalTotalDeduct", "CalculateTotalDeductions();", true);

        }
        #endregion

        public enum ControlsEnum
        {
            EARNINGS,
            DEDUCTIONS            
        }
        public enum VisibleStatus
        {
            SHOW,
            HIDE
        }
    }
}