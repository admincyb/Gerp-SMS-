using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ERPSMS_v01.UserControls;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessLogic.CommonManagement;
using BusinessObject.CommonManagement;
using BusinessObject.AccountManagement;
using BusinessLogic.HRMS.Common;
using GTIService.Constants.HRMS.Payroll;
using BusinessObject.HRMS.Payroll;
using BusinessLogic.HRMS.Payroll;
using System.Threading;
using ERPSMS_v01;

namespace HRMS.Payroll
{
    /// <summary>
    /// Page for Manage Appraisal Details
    /// </summary>
    public partial class AppraisalDetails : ERP.Store.UI.MyBasePage
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
                return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.LISTMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);
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

        private List<Details> ObjAppraisalDtls
        {
            get
            {
                return this.ViewState["ObjAppraisalDtls"] == null ? new List<Details>() : (List<Details>)this.ViewState["ObjAppraisalDtls"];
            }
            set
            {
                this.ViewState["ObjAppraisalDtls"] = value;
            }
        }
        /// <summary>
        /// Current Detail PK
        /// </summary>
        private int CurrDetPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState["CurrDetPK"]);
            }
            set
            {
                this.ViewState["CurrDetPK"] = value;
            }
        }
        /// <summary>
        /// Current Detail index
        /// </summary>
        private int CurrDetIndex
        {
            get
            {
                return Convert.ToInt32(this.ViewState["CurrDetIndex"]);
            }
            set
            {
                this.ViewState["CurrDetIndex"] = value;
            }
        }
        #endregion

        private ActionsEnum commonActions;
        private DataTable dtResult;
        private BusinessObject.User currentUser;
        private AppraisalDetailsBO ObjApprisal;
        GridViewRow gvrTemplate;
        private BusinessObject.HRMS.Employee.EmpTemplateHeader objEmpSalaryDetails;
        private int PayElementPk = 0;
        decimal minAmount = 0;
        decimal maxAmount = 0;
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
            dtResult = null;
            try
            {
                switch (type)
                {
                    #region Location
                    case ControlsEnum.LOCATION:
                        dtResult = HRMSCommonBL.GetHrmsCommonConstMst(21, 7, currentUser.SBUID, 0, Convert.ToInt32(DbActiveStatus.ACTIVE));
                        break;
                    #endregion
                    #region Employement Type
                    case ControlsEnum.EMPLOYMENTTYPE:
                        dtResult = HRMSCommonBL.GetHrmsCommonConstMst(21, 8, currentUser.SBUID, 0, Convert.ToInt32(DbActiveStatus.ACTIVE));
                        break;
                    #endregion
                    #region Department
                    case ControlsEnum.DEPARTMENTS:
                        dtResult = HRMSCommonBL.GetHrmsCommonConstMst(21, 9, currentUser.SBUID, 0, Convert.ToInt32(DbActiveStatus.ACTIVE));
                        break;
                    #endregion
                    #region Designation
                    case ControlsEnum.DESIGNATION:
                        dtResult = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeeDesignation(0);
                        break;
                    #endregion
                    #region Appraisal Type
                    case ControlsEnum.APPRAISALTYPES:
                        dtResult = CommonBL.GetAppConfig(currentUser.SBUID, GetLocalResourceObject("AppraisalType").ToString());
                        break;
                    #endregion
                    #region Pay Element
                    case ControlsEnum.PAYELEMENT:
                        dtResult = BusinessLogic.HRMS.Admin.Masters.PayElementsMasterBL.GetParentElement(0, Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                        break;
                    #endregion
                    #region Listing Page
                    case ControlsEnum.LIST:
                        BusinessObject.GridPrams gridParam;
                        gridParam = new BusinessObject.GridPrams();
                        gridParam.PageNumber = (PageIndex == 0 || PageIndex == null) ? 1 : PageIndex;
                        gridParam.PageSize = PageSize;
                        gridParam.FromDate = txtFilterFromDate.Text.Trim();
                        gridParam.ToDate = txtFilterToDate.Text.Trim();
                        int filterType = 0;
                        Int32.TryParse(ddlFilterType.SelectedItem.Value, out filterType);
                        dtResult = AppraisalDetailsBL.GetList(gridParam, currentUser.SBUID, txtFilterName.Text.Trim(), filterType);
                        break;
                    #endregion
                    #region Edit
                    case ControlsEnum.EDIT:
                        string ApprisDtl = AppraisalDetailsBL.GetAppraisalEdit(CurrPK.ToString());
                        if (ApprisDtl != string.Empty)
                        {
                            ObjApprisal = new AppraisalDetailsBO();
                            ObjApprisal = (AppraisalDetailsBO)GTIService.CommonFunctions.DeserializeObject(ApprisDtl, ObjApprisal);
                            ObjAppraisalDtls = ObjApprisal.Details;
                        }
                        break;
                    #endregion
                    #region PAY ELEMENT DETAILS
                    case ControlsEnum.PAYELEMENTDETAILS:
                        dtResult = BusinessLogic.HRMS.Admin.Masters.PayElementsMasterBL.GetParentElement(PayElementPk, Convert.ToInt32(DbActiveStatus.HASPK), currentUser.SBUID, 0, 0, -1);
                        break;
                    #endregion
                    #region Default
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
                    #region Location
                    case ControlsEnum.LOCATION:
                        BindDropDown(ControlsEnum.LOCATION);
                        break;
                    #endregion
                    #region Employment Type
                    case ControlsEnum.EMPLOYMENTTYPE:
                        BindDropDown(ControlsEnum.EMPLOYMENTTYPE);
                        break;
                    #endregion
                    #region Department
                    case ControlsEnum.DEPARTMENTS:
                        BindDropDown(ControlsEnum.DEPARTMENTS);
                        break;
                    #endregion
                    #region Designation
                    case ControlsEnum.DESIGNATION:
                        BindDropDown(ControlsEnum.DESIGNATION);
                        break;
                    #endregion
                    #region Appraisal Type
                    case ControlsEnum.APPRAISALTYPES:
                        BindDropDown(ControlsEnum.APPRAISALTYPES);
                        break;
                    #endregion
                    #region Appraisal Type Filter
                    case ControlsEnum.APPRAISALTYPESFILTER:
                        BindDropDown(ControlsEnum.APPRAISALTYPESFILTER);
                        break;
                    #endregion
                    #region Pay Element
                    case ControlsEnum.PAYELEMENT:
                        BindDropDown(ControlsEnum.PAYELEMENT);
                        break;
                    #endregion
                    #region Details
                    case ControlsEnum.DETAILS:
                        BindGrid(ControlsEnum.DETAILS);
                        break;
                    #endregion
                    #region List
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region Edit
                    case ControlsEnum.EDIT:
                        GetUIValuesFromObject(ControlsEnum.EDIT);
                        break;
                    #endregion
                    #region PAY ELEMENT DETAILS
                    case ControlsEnum.PAYELEMENTDETAILS:
                        GetUIValuesFromObject(ControlsEnum.PAYELEMENTDETAILS);
                        break;
                    #endregion    
                    #region Default
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
        #endregion

        #region Get UI Values From Object
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.EDIT:
                        if (ObjApprisal != null)
                        {
                            ddlBranchLoaction.SelectedValue = ObjApprisal.Branch;
                            ddlEmploymentType.SelectedValue = ObjApprisal.EmpType;
                            if (!string.IsNullOrEmpty(ObjApprisal.TransactDate))
                                txtTransactionDate.Text = Convert.ToDateTime(ObjApprisal.TransactDate).ToString(Resources.Constants.HRMSDateFormatShort);
                            txtTransactionNo.Text = string.IsNullOrEmpty(ObjApprisal.TransactNo) ? Resources.ErpRes.Draft.ToString() : ObjApprisal.TransactNo;
                            txtTransactName.Text = HttpUtility.HtmlDecode(ObjApprisal.TransactName);
                            if (ObjApprisal.Employee != null)
                            {
                                hdfEmployee.Value = ObjApprisal.Employee;
                                txtEmployee.Text = ObjApprisal.EmployeeText;
                                divEmployee.Visible = true;
                                UCEmpSalary.Visible = true;
                            }
                            else
                            {
                                divEmployee.Visible = false;
                                UCEmpSalary.Visible = false;
                            }
                            ddlType.SelectedValue = ObjApprisal.Type;
                            if (!string.IsNullOrEmpty(ObjApprisal.Effectdate))
                                txtEffectiveFrom.Text = Convert.ToDateTime(ObjApprisal.Effectdate).ToString(Resources.Constants.HRMSDateFormatShort);
                            ddlDepartment.SelectedValue = ObjApprisal.EmpDepartMent;
                            ddlDesignation.SelectedValue = ObjApprisal.EmpDesignation;
                            LastModifiedTime = ObjApprisal.LastModDate;
                            SetFieldValues(ControlsEnum.DETAILS);
                        }
                        break;
                    case ControlsEnum.EMPSALARYHEADER:
                        objEmpSalaryDetails = UCEmpSalary.EmpSalaryHdr;
                        if (objEmpSalaryDetails != null)
                        {
                            txtCurrDepartment.Text = objEmpSalaryDetails.EMP_DEPT_TEXT;
                            txtCurrDesignation.Text = objEmpSalaryDetails.EMP_DESIGNATION_TEXT;
                        }
                        break;
                    #region PAY ELEMENT DETAILS
                    case ControlsEnum.PAYELEMENTDETAILS:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ucFormulaMaster.IsDeduction = Convert.ToInt32(dtResult.Rows[0]["PEL_IS_DEDUCTION"]);
                        }
                        ucFormulaMaster.GetFieldValues(Admin.Masters.UserControls.FormulaMaster.ControlsEnum.PAYELEMENT);
                        ucFormulaMaster.SetFieldValues(Admin.Masters.UserControls.FormulaMaster.ControlsEnum.PAYELEMENT);
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

        #region Set UI Values To Object
        private object SetUIValuesToObject(ControlsEnum ControlType)
        {
            object returnObj;
            returnObj = null;
            try
            {
                switch (ControlType)
                {
                    case ControlsEnum.SAVE:
                        AppraisalDetailsBO objAppraisal = new AppraisalDetailsBO();
                        objAppraisal.CurrPk = CurrPK;
                        if (ddlBranchLoaction.SelectedValue != CommonConstants.SELECTVAL)
                            objAppraisal.Branch = ddlBranchLoaction.SelectedValue;
                        if (ddlEmploymentType.SelectedValue != CommonConstants.SELECTVAL)
                            objAppraisal.EmpType = ddlEmploymentType.SelectedValue;
                        if (hdfEmployee.Value != string.Empty && hdfEmployee.Value != CommonConstants.SELECTVAL && hdfEmployee.Value != CommonConstants.SELECT_VALUE_ZERO)
                        {
                            objAppraisal.Employee = hdfEmployee.Value;
                            if (ddlDepartment.SelectedValue != CommonConstants.SELECTVAL)
                                objAppraisal.EmpDepartMent = ddlDepartment.SelectedValue;
                            if (ddlDesignation.SelectedValue != CommonConstants.SELECTVAL)
                                objAppraisal.EmpDesignation = ddlDesignation.SelectedValue;
                        }
                        objAppraisal.TransactNo = (string.IsNullOrEmpty(txtTransactionNo.Text) || txtTransactionNo.Text.Equals(Resources.ErpRes.Draft.ToString())) ? string.Empty : txtTransactionNo.Text;
                        objAppraisal.TransactDate = txtTransactionDate.Text;
                        objAppraisal.TransactName = HttpUtility.HtmlEncode(txtTransactName.Text);
                        objAppraisal.Type = ddlType.SelectedValue;
                        objAppraisal.Effectdate = txtEffectiveFrom.Text == string.Empty ? null : txtEffectiveFrom.Text;
                        objAppraisal.DepartMent = currentUser.CurrentDeptPK;
                        objAppraisal.UserPk = currentUser.PKUser;
                        objAppraisal.BizUnit = currentUser.SBUID;
                        objAppraisal.LastModDate = LastModifiedTime;

                        objAppraisal.Details = ObjAppraisalDtls;
                        returnObj = objAppraisal;
                        break;
                    case ControlsEnum.DETAILS:
                        if (CurrDetIndex < 0)
                        {
                            List<Details> ObjListDtls = ObjAppraisalDtls;
                            Details objDetail = new Details();
                            objDetail.DetPK = 0;
                            objDetail.IncrementOn = ddlPayElement.SelectedValue;
                            objDetail.IncrementOnText = ddlPayElement.SelectedItem.Text;
                            objDetail.Formula = hdfFormula.Value;
                            objDetail.FormulaText = txtAmountOrFormula.Text;
                            objDetail.Remarks = HttpUtility.HtmlEncode(txtRemarks.Text);
                            objDetail.MinAmount = ucFormulaMaster.MinAmount;
                            objDetail.MaxAmount = ucFormulaMaster.MaxAmount;
                            ObjListDtls.Add(objDetail);
                            ObjAppraisalDtls = ObjListDtls;
                        }
                        else
                        {
                            ObjAppraisalDtls[CurrDetIndex].IncrementOn = ddlPayElement.SelectedValue;
                            ObjAppraisalDtls[CurrDetIndex].IncrementOnText = ddlPayElement.SelectedItem.Text;
                            ObjAppraisalDtls[CurrDetIndex].Formula = hdfFormula.Value;
                            ObjAppraisalDtls[CurrDetIndex].FormulaText = txtAmountOrFormula.Text;
                            ObjAppraisalDtls[CurrDetIndex].Remarks = HttpUtility.HtmlEncode(txtRemarks.Text);
                            ObjAppraisalDtls[CurrDetIndex].MinAmount = ucFormulaMaster.MinAmount;
                            ObjAppraisalDtls[CurrDetIndex].MaxAmount = ucFormulaMaster.MaxAmount;
                        }
                        SetFieldValues(ControlsEnum.DETAILS);
                        break;
                }
                return returnObj;
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

        #region Helper Methods
        /// <summary>
        /// for checking radio button selected in main grid
        /// </summary>
        /// <param name="GridRowIndex"></param>
        /// <returns></returns>
        private bool IsRadioButtonSelected(ref int GridRowIndex)
        {
            RadioButton rbtn;
            foreach (GridViewRow grdrow in grdList.Rows)
            {
                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                if (rbtn.Checked)
                {
                    GridRowIndex = grdrow.RowIndex;
                    return true;
                }
            }
            return GridRowIndex == 0 ? false : true;
        }
        /// <summary>
        /// for fill employe details
        /// </summary>
        private void FillEmplyeeDetails()
        {
            txtCurrDepartment.Text = string.Empty;
            txtCurrDesignation.Text = string.Empty;
            if (hdfEmployee.Value != string.Empty && hdfEmployee.Value != CommonConstants.SELECTVAL && hdfEmployee.Value != CommonConstants.SELECT_VALUE_ZERO)
            {
                UCEmpSalary.EmployeePK = Convert.ToInt32(hdfEmployee.Value);
                UCEmpSalary.GetFieldValues(BusinessObject.HRMS.Employee.ActionsEnum.EMPLOYEESALARY);
                UCEmpSalary.SetFieldValues(BusinessObject.HRMS.Employee.ActionsEnum.EMPLOYEESALARY);
                GetUIValuesFromObject(ControlsEnum.EMPSALARYHEADER);
            }
        }
       
       

        // Formula Popup After Apply
        void ucFormulaMaster_AfterApply(object sender, EventArgs e)
        {
            txtAmountOrFormula.Text = ucFormulaMaster.FormulaText;
            hdfFormula.Value = ucFormulaMaster.FormulaValue;
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
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
                    #region LIST
                    case ControlsEnum.LIST:
                        uclPaging.Visible = false;
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            int rowCount = 0;

                            rowCount = Convert.ToInt32(dtResult.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                            this.TotalPages = Convert.ToInt32(dtResult.Rows[0]["ROW_NO"].ToString());

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
                    #region Details
                    case ControlsEnum.DETAILS:
                        grdIncrementDetails.DataSource = ObjAppraisalDtls;
                        grdIncrementDetails.DataBind();
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
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ControlsEnum Mode)
        {
            try
            {
                
                switch (Mode)
                {
                    #region Grid Edit
                    case ControlsEnum.DETAILEDIT:
                        ddlPayElement.SelectedValue = ((grdIncrementDetails.Rows[CurrDetIndex].FindControl("hdfIncrementOn") as HiddenField).Value);
                        txtRemarks.Text = HttpUtility.HtmlDecode(((grdIncrementDetails.Rows[CurrDetIndex].FindControl("lblRemarks") as Label).ToolTip));
                        txtAmountOrFormula.Text = ((grdIncrementDetails.Rows[CurrDetIndex].FindControl("lblFormula") as Label).Text);
                        hdfFormula.Value = ((grdIncrementDetails.Rows[CurrDetIndex].FindControl("hdfFormula") as HiddenField).Value);

                        HiddenField hdfMinAmount = grdIncrementDetails.Rows[CurrDetIndex].FindControl("hdfMinAmount") as HiddenField;
                        HiddenField hdfMaxAmount = grdIncrementDetails.Rows[CurrDetIndex].FindControl("hdfMaxAmount") as HiddenField;
                        decimal.TryParse(hdfMinAmount.Value, out minAmount);
                        decimal.TryParse(hdfMaxAmount.Value, out maxAmount);
                        ucFormulaMaster.SetData(txtAmountOrFormula.Text.Trim(), ddlPayElement.SelectedItem.Text, minAmount, maxAmount);
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
        /// for bind drop downs
        /// </summary>
        /// <param name="controlType"></param>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region Loaction
                case ControlsEnum.LOCATION:
                    ddlBranchLoaction.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlBranchLoaction.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, GTIService.Constants.Common.Common.CON_DATA_TEXT_FIELD);
                        ddlBranchLoaction.DataTextField = GTIService.Constants.Common.Common.CON_DATA_TEXT_FIELD;
                        ddlBranchLoaction.DataValueField = GTIService.Constants.Common.Common.CON_PK_FIELD;
                        ddlBranchLoaction.DataBind();
                    }
                    ddlBranchLoaction.Items.Insert(0, new ListItem(Resources.Report.SelectAll, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Employment Type
                case ControlsEnum.EMPLOYMENTTYPE:
                    ddlEmploymentType.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlEmploymentType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, GTIService.Constants.Common.Common.CON_DATA_TEXT_FIELD);
                        ddlEmploymentType.DataTextField = GTIService.Constants.Common.Common.CON_DATA_TEXT_FIELD;
                        ddlEmploymentType.DataValueField = GTIService.Constants.Common.Common.CON_PK_FIELD;
                        ddlEmploymentType.DataBind();
                    }
                    ddlEmploymentType.Items.Insert(0, new ListItem(Resources.Report.SelectAll, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Department
                case ControlsEnum.DEPARTMENTS:
                    ddlDepartment.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlDepartment.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, GTIService.Constants.Common.Common.CON_DATA_TEXT_FIELD);
                        ddlDepartment.DataTextField = GTIService.Constants.Common.Common.CON_DATA_TEXT_FIELD;
                        ddlDepartment.DataValueField = GTIService.Constants.Common.Common.CON_PK_FIELD;
                        ddlDepartment.DataBind();
                    }
                    ddlDepartment.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Designation
                case ControlsEnum.DESIGNATION:
                    ddlDesignation.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlDesignation.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, Fields.F_DESIN_CODE);
                        ddlDesignation.DataTextField = Fields.F_DESIN_CODE;
                        ddlDesignation.DataValueField = Fields.F_DESIN_PK;
                        ddlDesignation.DataBind();
                    }
                    ddlDesignation.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Appraisal Type
                case ControlsEnum.APPRAISALTYPES:
                    ddlType.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD);
                        ddlType.DataTextField = GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD;
                        ddlType.DataValueField = GTIService.Constants.Common.Common.CFG_VALUE_VALUE_FIELD;
                        ddlType.DataBind();
                    }
                    ddlType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Appraisal Type Filter
                case ControlsEnum.APPRAISALTYPESFILTER:
                    ddlFilterType.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlFilterType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD);
                        ddlFilterType.DataTextField = GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD;
                        ddlFilterType.DataValueField = GTIService.Constants.Common.Common.CFG_VALUE_VALUE_FIELD;
                        ddlFilterType.DataBind();
                    }
                    ddlFilterType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Pay Element
                case ControlsEnum.PAYELEMENT:
                    ddlPayElement.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlPayElement.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_CODE);
                        ddlPayElement.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_NAME;
                        ddlPayElement.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_PK;
                        ddlPayElement.DataBind();
                    }
                    ddlPayElement.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                default:
                    break;
            }
        }
        /// <summary>
        /// for clear controls
        /// </summary>
        /// <param name="controlType"></param>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.DETAILCLEAR:
                    CurrDetIndex = -1;
                    CurrDetPK = 0;
                    ddlPayElement.SelectedIndex = 0;
                    hdfFormula.Value = string.Empty;
                    txtAmountOrFormula.Text = string.Empty;
                    txtRemarks.Text = string.Empty;
                    ResetForm(ControlsEnum.POPUPCLEAR);
                    break;
                case ControlsEnum.CLEAR:
                    dtResult = null;
                    CurrPK = 0;
                    CurrDetIndex = -1;
                    CurrDetPK = 0;
                    ddlBranchLoaction.SelectedIndex = 0;
                    ddlEmploymentType.SelectedIndex = 0;
                    txtTransactionNo.Text = Resources.ErpRes.Draft.ToString();
                    txtTransactionDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                    hdfEmployee.Value = "0";
                    txtEmployee.Text = "Select/Type";
                    ddlType.SelectedIndex = 0;
                    txtEffectiveFrom.Text = string.Empty;
                    txtTransactName.Text = string.Empty;
                    ddlPayElement.SelectedIndex = 0;
                    txtAmountOrFormula.Text = string.Empty;
                    txtRemarks.Text = string.Empty;
                    txtCurrDepartment.Text = string.Empty;
                    txtCurrDesignation.Text = string.Empty;
                    ddlDepartment.SelectedIndex = 0;
                    ddlDesignation.SelectedIndex = 0;
                    divEmployee.Visible = false;
                    UCEmpSalary.Visible = false;
                    ObjAppraisalDtls = null;
                    SetFieldValues(ControlsEnum.DETAILS);
                    txtEmployee.Enabled = true;
                    ddlBranchLoaction.Enabled = true;
                    ddlEmploymentType.Enabled = true;
                    grdIncrementDetails.Columns[grdIncrementDetails.Columns.Count - 1].Visible = true;
                    break;
                case ControlsEnum.POPUPCLEAR:
                    ucFormulaMaster.FormulaText = ucFormulaMaster.FormulaValue = string.Empty;
                    ucFormulaMaster.MaxAmount = 0;
                    ucFormulaMaster.MinAmount = 0;
                    ucFormulaMaster.ResetForm();
                    break;
                case ControlsEnum.EMPDETAILS:
                    txtCurrDepartment.Text = string.Empty;
                    txtCurrDesignation.Text = string.Empty;
                    break;
                #region Filter Clear
                case ControlsEnum.CLEARSEARCH:
                    txtFilterFromDate.Text = txtFilterToDate.Text = txtFilterName.Text = string.Empty;
                    if (ddlFilterType.Items.Count > 0)
                        ddlFilterType.SelectedIndex = 0;
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
        #endregion

        #region ActionHandler
        /// <summary>
        /// Handling control events
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
                int GridRowIndex = 0;
                int? result;
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
                    commonActions = ActionsEnum.CHANGE;
                }
                switch (commonActions)
                {
                    #region Save
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else
                        {
                            AppraisalDetailsBO ObjAppraisal = new AppraisalDetailsBO();
                            ObjAppraisal = (AppraisalDetailsBO)SetUIValuesToObject(ControlsEnum.SAVE);
                            if (ObjAppraisalDtls != null)
                            {
                                string xmlDoc = CommonFunctions.XmlSerialize<AppraisalDetailsBO>(ObjAppraisal);
                                result = AppraisalDetailsBL.SaveAppraisal(xmlDoc);
                                if (result >= 0)
                                {
                                    ResetForm(ControlsEnum.CLEAR);
                                    GetFieldValues(ControlsEnum.LIST);
                                    SetFieldValues(ControlsEnum.LIST);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    btnNew.Focus();
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("AppraisalDetails").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    #region Error Messages
                                    if (result == (int)DbSaveStatus.SQLERROR)
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CONCURRENCY)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("AppraisalDetails").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.REFNOEXIST)
                                    {
                                        litErrorMsg.Text = String.Format(Resources.ErrorMessages.Msg_Name_Already_Exists, GetLocalResourceObject("AppraisalDetails").ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = String.Format(Resources.ErrorMessages.Msg_Code_Already_Exists, GetLocalResourceObject("AppraisalDetails").ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("AppraisalDetails").ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    #endregion
                                }
                            }
                        }
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        txtTransactionDate.Focus();
                        ResetForm(ControlsEnum.CLEAR);
                        EntryStatus = EntryStatus.NEWMODE;
                        break;
                    #endregion
                    #region Edit
                    case ActionsEnum.EDIT:
                    case ActionsEnum.DETAIL:
                    case ActionsEnum.VIEW:
                        if (IsRadioButtonSelected(ref GridRowIndex))
                        {
                            txtTransactionDate.Focus();
                            txtEmployee.Enabled = false;
                            ddlBranchLoaction.Enabled = false;
                            ddlEmploymentType.Enabled = false;
                            CurrPK = Convert.ToInt32((grdList.Rows[GridRowIndex].FindControl("hdfAppraisalPk") as HiddenField).Value);
                            GetFieldValues(ControlsEnum.EDIT);
                            SetFieldValues(ControlsEnum.EDIT);
                            FillEmplyeeDetails();
                            EntryStatus = (commonActions == ActionsEnum.VIEW) ? EntryStatus.VIEWMODE : EntryStatus.EDITMODE;
                            if (commonActions == ActionsEnum.VIEW)
                                grdIncrementDetails.Columns[grdIncrementDetails.Columns.Count - 1].Visible = false;
                            else
                                grdIncrementDetails.Columns[grdIncrementDetails.Columns.Count - 1].Visible = true;
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                    case ActionsEnum.LIST:
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        ResetForm(ControlsEnum.CLEAR);
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.CLEAR);
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        result = AppraisalDetailsBL.DeleteAppraisal(CurrPK, LastModifiedTime);
                        if (result > 0)
                        {
                            if (grdList.Rows.Count == 1 && PageIndex > 1)
                            {
                                PageIndex--;
                            }
                            ResetForm(ControlsEnum.CLEAR);
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            EntryStatus = EntryStatus.LISTMODE;
                            btnNew.Focus();
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("AppraisalDetails").ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        else
                        {
                            #region Error Message
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("AppraisalDetails").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.REFNOEXIST)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("AppraisalDetails").ToString() + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.REFERRED)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("AppraisalDetails").ToString() + " " + Resources.Messages.UsedInAnotherPlace;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("AppraisalDetails").ToString() + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            #endregion
                        }
                        break;

                    #endregion
                    #region After Auto Complete Select
                    case ActionsEnum.AFTERAUTOSELECT:
                        if (((Button)sender).ID == "btnEmployeeSelect")
                        {
                            if (hdfEmployee.Value == string.Empty || hdfEmployee.Value == "-1" || hdfEmployee.Value == "0")
                            {
                                divEmployee.Visible = false;
                                UCEmpSalary.Visible = false;
                                ResetForm(ControlsEnum.EMPDETAILS);
                            }
                            else
                            {
                                divEmployee.Visible = true;
                                UCEmpSalary.Visible = true;
                                FillEmplyeeDetails();
                            }
                        }
                        break;
                    #endregion
                    #region Add New Details
                    case ActionsEnum.ADD:
                        SetUIValuesToObject(ControlsEnum.DETAILS);
                        ResetForm(ControlsEnum.DETAILCLEAR);
                        break;
                    #endregion
                    #region Show Popup
                    case ActionsEnum.SHOWPOPUP:                        
                        if (Convert.ToInt32(ddlPayElement.SelectedValue) > 0)
                            PayElementPk = Convert.ToInt32(ddlPayElement.SelectedValue);
                        GetFieldValues(ControlsEnum.PAYELEMENTDETAILS);
                        SetFieldValues(ControlsEnum.PAYELEMENTDETAILS);
                        ucFormulaMaster.SetData(txtAmountOrFormula.Text, ddlPayElement.SelectedItem.Text, ucFormulaMaster.MinAmount, ucFormulaMaster.MaxAmount);
                        // if (ucFormulaMaster.FormulaText != string.Empty) ucFormulaMaster.FormulaTextFromParent = ucFormulaMaster.FormulaText;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divFormulaPopUp]','Formula','660','230');", true);
                        break;
                    #endregion
                    #region Grid Edit
                    case ActionsEnum.ITEMGRIDEDIT:
                        gvrTemplate = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        CurrDetIndex = gvrTemplate.RowIndex;
                        CurrDetPK = Convert.ToInt32((grdIncrementDetails.Rows[CurrDetIndex].FindControl("hdfDetPk") as HiddenField).Value);
                        SetUIEditView(ControlsEnum.DETAILEDIT);
                        break;
                    #endregion
                    #region Grid Detail Delete
                    case ActionsEnum.DELETE_ACTION:
                        if (((ImageButton)sender).ID == "imbDeleteDetails")
                        {
                            gvrTemplate = ((ImageButton)sender).Parent.Parent as GridViewRow;
                            ObjAppraisalDtls.RemoveAt(gvrTemplate.RowIndex);
                            SetFieldValues(ControlsEnum.DETAILS);
                        }
                        break;
                    #endregion
                    #region Drop Down Change
                    case ActionsEnum.CHANGE:
                        if (((DropDownList)sender).ID == "ddlBranchLoaction")
                        {
                            hdfEmployee.Value = "0";
                            txtEmployee.Text = "Select/Type";
                            ActionHandler(btnEmployeeSelect, EventArgs.Empty);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideAdvancedSearch", "ShowHideAdvancedSearch('1');", true);
                        }
                        else if (((DropDownList)sender).ID == "ddlEmploymentType")
                        {
                            hdfEmployee.Value = "0";
                            txtEmployee.Text = "Select/Type";
                            ActionHandler(btnEmployeeSelect, EventArgs.Empty);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideAdvancedSearch", "ShowHideAdvancedSearch('1');", true);
                        }
                        break;
                    #endregion
                    #region Filter
                    case ActionsEnum.FILTER:
                        PageIndex = 1;
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region Filter Clear
                    case ActionsEnum.CLEARSEARCH:
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex)
                                           + "','" + Resources.ErpRes.Information + "');", true);
            }
            finally
            {

            }
        }

        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex;
            EntryStatus = EntryStatus.LISTMODE;
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (SortBy == e.SortExpression)
                {
                    if (SortDirection == Resources.Report.SortAscending)
                        SortDirection = Resources.Report.SortDescending;
                    else
                        SortDirection = Resources.Report.SortAscending;
                }
                else
                {
                    SortBy = e.SortExpression;
                    SortDirection = Resources.Report.SortAscending;
                }
                this.PageIndex = 1;
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
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
                GetFieldValues(ControlsEnum.LIST);
                SetFieldValues(ControlsEnum.LIST);
                EnableDisableButtons(e.TotalPages, "uclPaging");
                EntryStatus = EntryStatus.LISTMODE;
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
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideAdvancedSearch", "ShowHideAdvancedSearch();", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideAdvancedFilter", "ShowHideAdvancedFilter();", true);
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
                ucFormulaMaster.AfterApply += new EventHandler(ucFormulaMaster_AfterApply);
                if (!IsPostBack)
                {
                    hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfCurrencyFormatWithComma.Value = "#" + currencysep + "#0.";
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                        hdfCurrencyFormatWithComma.Value += "0";
                    }

                    CurrDetIndex = -1;
                    PageIndex = Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE);
                    uclPaging.CurrentPage = PageIndex;
                    //For get and set Appraisal Type in filter dropdown
                    GetFieldValues(ControlsEnum.APPRAISALTYPES);
                    SetFieldValues(ControlsEnum.APPRAISALTYPESFILTER);
                    //For get and set listing grid
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    //For get and set location dropdown
                    GetFieldValues(ControlsEnum.LOCATION);
                    SetFieldValues(ControlsEnum.LOCATION);
                    //For get and set Employement Type dropdown
                    GetFieldValues(ControlsEnum.EMPLOYMENTTYPE);
                    SetFieldValues(ControlsEnum.EMPLOYMENTTYPE);
                    //For get and set Department dropdown
                    GetFieldValues(ControlsEnum.DEPARTMENTS);
                    SetFieldValues(ControlsEnum.DEPARTMENTS);
                    //For get and set Designation dropdown
                    GetFieldValues(ControlsEnum.DESIGNATION);
                    SetFieldValues(ControlsEnum.DESIGNATION);
                    //For get and set Appraisal Type dropdown
                    GetFieldValues(ControlsEnum.APPRAISALTYPES);
                    SetFieldValues(ControlsEnum.APPRAISALTYPES);
                    //For get and set pay element(Increment on) dropdown
                    GetFieldValues(ControlsEnum.PAYELEMENT);
                    SetFieldValues(ControlsEnum.PAYELEMENT);

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
            EDIT,
            LIST,
            CANCEL,
            CLEAR,
            SAVE,
            LOCATION,
            EMPLOYMENTTYPE,
            DEPARTMENTS,
            DESIGNATION,
            APPRAISALTYPES,
            PAYELEMENT,
            DETAILS,
            DETAILEDIT,
            DETAILCLEAR,
            POPUPCLEAR,
            EMPDETAILS,
            EMPSALARYHEADER,
            APPRAISALTYPESFILTER,
            CLEARSEARCH,
            PAYELEMENTDETAILS
        }
        #endregion
    }
}