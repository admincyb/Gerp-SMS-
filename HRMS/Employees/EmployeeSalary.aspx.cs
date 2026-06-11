using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject;
using BusinessObject.HRMS.Employee;
using System.Data;
using BusinessLogic.HRMS.Employee;
using BusinessObject.CommonManagement;
using System.Threading;
using BusinessLogic.HRMS.Payroll;
using BusinessLogic.CommonManagement;
using BusinessObject.Common;
using ERPSMS_v01;

namespace HRMS.Employees
{
    public partial class EmployeeSalary : ERP.Store.UI.MyBasePage
    {
        #region Variables
        private EmployeeBasicInfomtn objEmployeeBasicInfo;
        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        User currentUser;

        private DataTable pageData;
        private EmpTemplateHeader objEmpSalaryDetails;
        private DataTable dtPayrollType;
        private DataTable dtPayElement;
        private DataTable dtPayrollTypeMpg;
        //private List<EmpSalaryDetails> empSalaryDetails;
        private int PayrollTypePk = 0;
        private int SalaryTemplatePk = 0;
        private int EmpPayrollType = 0;

        #endregion

        #region Properties
        public int CurrPK
        {
            get
            {
                return Convert.ToInt32(Session[ERP.Utilities.SessionStrings.CurrentPK]);
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.CurrentPK] = value;
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
        /// To maintain the IsRevisionSetAsCurrent in viewstate
        /// </summary>
        private bool IsRevisionSetAsCurrent
        {
            get
            {
                return this.ViewState["IsRevisionSetAsCurrent"] == null ? false : (bool)this.ViewState["IsRevisionSetAsCurrent"];
            }
            set
            {
                this.ViewState["IsRevisionSetAsCurrent"] = value;
            }
        }
        private EntryStatus EntryStatus
        {
            get
            {
                return this.Session[ViewstateStrings.EntryState] == null ? EntryStatus.LISTMODE : (EntryStatus)(this.Session[ViewstateStrings.EntryState]);
            }
            set
            {
                this.Session[ViewstateStrings.EntryState] = value;
            }
        }

        #endregion       

        #region Functions
        private void PageActionHandler()
        {
            if (CurrPK > 0)
            {
                hdfCurrencyFormat.Value = "#0.";
                for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                {
                    hdfCurrencyFormat.Value += "0";
                }
                GetFieldValues(ControlsEnum.PAYROLLTYPEUSERMAPPING);
                if (EmpPayrollType == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Msg_PayrollTypeNotMapped)
                                                         + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EmployeeList) + "');", true);
                    return;
                }
                if (dtPayrollTypeMpg == null || dtPayrollTypeMpg.Rows.Count <= 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErpRes.NoPressionMsg)
                                                                       + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EmployeeList) + "');", true);
                    return;
                }
               
                // UCEmpSalary.ControlActionMode = Convert.ToInt32(EmpSalaryAction.View);
                GetFieldValues(ControlsEnum.EMPLOYEEDETAILSHEADER);
                SetFieldValues(ControlsEnum.EMPLOYEEDETAILSHEADER);
                GetFieldValues(ControlsEnum.PAYROLLTYPE);
                SetFieldValues(ControlsEnum.PAYROLLTYPE);
                GetFieldValues(ControlsEnum.SALARYTEMPLATE);
                SetFieldValues(ControlsEnum.SALARYTEMPLATE);
                GetFieldValues(ControlsEnum.EMPLOYEESALARY);
                SetFieldValues(ControlsEnum.SALARYTEMPLATEDETAILS);
                GetUIValuesFromObject(ControlsEnum.EMPSALARYHEADER);                

                // Revision History Details
                //GetFieldValues(ControlsEnum.REVISIONHISTORY);
                //SetFieldValues(ControlsEnum.REVISIONHISTORY);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideSalarySplitUp", "ShowHideSalarySplitUp(0);", true);
            }
            else
            {
                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                Session["SelectMessage"] = litErrorMsg.Text;
                Response.Redirect(Resources.PageURL.EmployeeList, false);
            }
        }

        private void GetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.EMPLOYEEDETAILSHEADER:
                        objEmployeeBasicInfo = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeebyID(CurrPK);
                        break;
                    case ControlsEnum.SALARYTEMPLATE:                       
                        pageData = EmployeePayDetailsBL.GetSalaryTemplate(currentUser.SBUID, SalaryTemplatePk, (int)DbActiveStatus.ACTIVE, Convert.ToInt32(ddlPayrollType.SelectedValue));
                        break;
                    case ControlsEnum.SALARYTEMPLATEDETAILS: // Get salary head from Salary template
                        int salaryTemplatePk = 0;
                        salaryTemplatePk = Convert.ToInt32(ddlSalaryTemplate.SelectedValue) > 0 ? Convert.ToInt32(ddlSalaryTemplate.SelectedValue) : 0;

                        objEmpSalaryDetails = salaryTemplatePk > 0 ? EmployeeSalaryBL.GetSalaryDetails(salaryTemplatePk,CurrPK) : null;
                        if (objEmpSalaryDetails != null)
                        {
                            UCEmpSalary.EmpSalaryHdr = objEmpSalaryDetails;
                            //UCEmpSalary.EmpSalaryHdr.SalaryDtl = objEmpSalaryDetails.SalaryDtl;
                        }
                        else
                        {
                            UCEmpSalary.EmpSalaryHdr.SalaryDtl = null;
                        }
                        //if (UCEmpSalary.EmpSalaryHdr != null && UCEmpSalary.EmpSalaryHdr.SalaryDtl != null)
                        //{
                        //    if (UCEmpSalary.EmpSalaryHdr.SalaryDtl.Count > 0)
                        //    {
                        //        UCEmpSalary.EmpSalaryHdr.SalaryDtl.ForEach(elmt => elmt.STS_SL_NO = elmt.EDP_PK); // setting the sequence no.
                        //    }
                        //}
                        break;
                    case ControlsEnum.EMPLOYEESALARY: // Get employee salary details
                        UCEmpSalary.EmployeePK = CurrPK;
                        UCEmpSalary.GetFieldValues(ActionsEnum.EMPLOYEESALARY);
                        break;
                    //#region REVISIONHISTORY
                    //case ControlsEnum.REVISIONHISTORY:
                    //    dtResult = BusinessLogic.HRMS.Employee.EmployeeSalaryRevisionBL.GetEmployeeSalaryHistory(CurrPK);
                    //    break;
                    //#endregion
                    //#region EMPLOYEESALARYREVISION
                    //case ControlsEnum.EMPLOYEESALARYREVISION: // Get employee salary details
                    //    UCEmpSalaryRevision.EmployeePK = CurrPK;
                    //    UCEmpSalaryRevision.GetFieldValues(ActionsEnum.EMPLOYEESALARY);
                    //    break; 
                    //#endregion
                    case ControlsEnum.PAYROLLTYPE:
                        // dtPayrollType = EmployeePayDetailsBL.GetPayrollType(PayrollTypePk, (int)DbActiveStatus.ACTIVE, currentUser.SBUID);
                        dtPayrollType = PayrollProcessBL.GetPayrollType(PayrollTypePk, currentUser.SBUID, Convert.ToInt32(DbActiveStatus.ACTIVE));
                        break;
                    case ControlsEnum.PAYROLLTYPEUSERMAPPING:
                        EmpPayrollType = 0;
                        dtPayrollTypeMpg = EmployeePayDetailsBL.GetPayrollTypeUserMapping(currentUser.PKUser, CurrPK, out EmpPayrollType);
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

        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.EMPLOYEEDETAILSHEADER:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.SALARYTEMPLATE:
                        BindDropDown(ControlsEnum.SALARYTEMPLATE);
                        break;
                    case ControlsEnum.SALARYTEMPLATEDETAILS:
                        UCEmpSalary.SetFieldValues(ActionsEnum.EMPLOYEESALARY);
                        if (UCEmpSalary.IsSalaryProcessed > 0)
                        {
                            ddlSalaryTemplate.Enabled = false;
                            ddlPayrollType.Enabled = false;
                        }
                        break;
                    case ControlsEnum.PAYROLLTYPE:
                        BindDropDown(ControlsEnum.PAYROLLTYPE);
                        break;
                    //#region REVISIONHISTORY
                    //case ControlsEnum.REVISIONHISTORY:
                    //    BindGrid(ControlsEnum.REVISIONHISTORY);
                    //    break;
                    //#endregion REVISIONHISTORY
                    //#region EMPLOYEESALARYREVISION
                    //case ControlsEnum.EMPLOYEESALARYREVISION:
                    //    UCEmpSalaryRevision.SetFieldValues(ActionsEnum.EMPLOYEESALARY);
                    //    break;
                    //#endregion

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.EMPLOYEEDETAILSHEADER:
                        if (objEmployeeBasicInfo != null)
                        {
                            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                            CurrPK = objEmployeeBasicInfo.empPK;
                            UCempBasicHdr.objEmployeeBasicInfo = objEmployeeBasicInfo;
                            UCempBasicHdr.SetFieldValues();
                            LastModifiedTime = objEmployeeBasicInfo.LAST_MOD_DT;
                            //lblhdrEmployeeNoTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCode), 20);
                            //lblhdrEmployeeNameTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empNameText), 20);
                            //if (!string.IsNullOrEmpty(objEmployeeBasicInfo.empDOJText))
                            //{
                            //    string dojText = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOJText).Trim(); ;
                            //    if (!dojText.IsNullOrEmptyOrWhitespace() && dojText[dojText.Length - 2] == ' ')
                            //    {
                            //        dojText = dojText.Remove(dojText.Length - 2, 1);
                            //    }
                            //    lblhdrDOJText.Text = dojText;
                            //}
                            //if (!string.IsNullOrEmpty(objEmployeeBasicInfo.empDOBText))
                            //{
                            //    string dobText = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOBText).Trim();
                            //    if (!dobText.IsNullOrEmptyOrWhitespace() && dobText[dobText.Length - 2] == ' ')
                            //    {
                            //        dobText = dobText.Remove(dobText.Length - 2, 1);
                            //    }
                            //    lblhdrDOBTxt.Text = dobText;
                            //}
                            //lblhdrDesignationTxt.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDesignationText);
                            //lblhdrDepartmentTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDepartmentText), 20);
                            //lblhdrEmployeeNoTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCode);
                            //lblhdrEmployeeNameTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empNameText);
                            //lblhdrDOJText.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOJText);
                            //lblhdrDOBTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOBText);
                            //lblhdrDesignationTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDesignationText);
                            //lblhdrDepartmentTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDepartmentText);
                        }
                        break;

                    case ControlsEnum.EMPSALARYHEADER:
                        objEmpSalaryDetails = UCEmpSalary.EmpSalaryHdr;
                        txtEmpBasic.Text = objEmpSalaryDetails.EMP_BASIC_PAY.ToString(hdfCurrencyFormat.Value);
                        PayrollTypePk = objEmpSalaryDetails.EMP_PAYROLL_TYPE;                       
                        GetFieldValues(ControlsEnum.PAYROLLTYPE);
                        SetFieldValues(ControlsEnum.PAYROLLTYPE);
                        if (objEmpSalaryDetails.EMP_PAYROLL_TYPE > 0)
                            ddlPayrollType.SelectedValue = objEmpSalaryDetails.EMP_PAYROLL_TYPE.ToString();
                        SalaryTemplatePk = objEmpSalaryDetails.EMP_SALARY_TEMP;
                        GetFieldValues(ControlsEnum.SALARYTEMPLATE);
                        SetFieldValues(ControlsEnum.SALARYTEMPLATE);
                        if (objEmpSalaryDetails.EMP_SALARY_TEMP > 0)
                            ddlSalaryTemplate.SelectedValue = Convert.ToInt32(objEmpSalaryDetails.EMP_SALARY_TEMP).ToString();
                        txtEffectiveFromDate.Text = objEmpSalaryDetails.EMP_EFFECTIVE_FROM.ToString(Resources.Constants.HRMSDateFormatShort);

                        //txtEmpNetSalary.Text = (objEmpSalaryDetails.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 0).Sum(ern => ern.STS_VALUE) 
                        //                      - objEmpSalaryDetails.SalaryDtl.Where(sal => sal.PEL_IS_DEDUCTION == 1).Sum(ern => ern.STS_VALUE)).ToString(hdfCurrencyFormat.Value);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }

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
                    case ControlsEnum.EMPSALARYHEADER:
                        objEmpSalaryDetails.EMP_PK = CurrPK;
                        objEmpSalaryDetails.EMP_SALARY_TEMP = Convert.ToInt32(ddlSalaryTemplate.SelectedValue);
                        objEmpSalaryDetails.EMP_BASIC_PAY = Convert.ToDouble(txtEmpBasic.Text);
                        //objEmpSalaryDetails.EMP_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        //objEmpSalaryDetails.BIZUNIT_PK = Convert.ToInt16(currentUser.SBUID);
                        //objEmpSalaryDetails.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        objEmpSalaryDetails.USER_PK = Convert.ToInt32(currentUser.PKUser);
                        objEmpSalaryDetails.LAST_MOD_DT = LastModifiedTime;
                        objEmpSalaryDetails.EMP_PAYROLL_TYPE = Convert.ToInt32(ddlPayrollType.SelectedValue);
                        UCEmpSalary.SetUIValuesToObject(ActionsEnum.EMPLOYEESALARY);
                        objEmpSalaryDetails.SalaryDtl = UCEmpSalary.EmpSalaryHdr.SalaryDtl;
                        objEmpSalaryDetails.EMP_EFFECTIVE_FROM = DateTime.Parse(txtEffectiveFromDate.Text);
                        objEmpSalaryDetails.empNetSalary=txtEmpNetSalary.Text==string.Empty?0:Convert.ToDouble(txtEmpNetSalary.Text);
                        objEmpSalaryDetails.empCTC = txtTotalCTC.Text == string.Empty ? 0 : Convert.ToDouble(txtTotalCTC.Text);
                        objEmpSalaryDetails.empGrossSalary = txtTotalGross.Text == string.Empty ? 0 : Convert.ToDouble(txtTotalGross.Text);
                        if (IsRevisionSetAsCurrent)
                        {
                            foreach (var item in objEmpSalaryDetails.SalaryDtl)
                            {
                                item.EDP_PK = 0;
                            }
                        }
                        retObject = objEmpSalaryDetails;
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

        private void BindDropDown(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                case ControlsEnum.SALARYTEMPLATE:
                    ddlSalaryTemplate.DataSource = pageData;
                    ddlSalaryTemplate.DataTextField = GTIService.Constants.HRMS.Employee.Fields.STE_NAME;
                    ddlSalaryTemplate.DataValueField = GTIService.Constants.HRMS.Employee.Fields.STE_PK;
                    ddlSalaryTemplate.Items.HtmlDecode();
                    ddlSalaryTemplate.DataBind();
                    ddlSalaryTemplate.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.PAYROLLTYPE:
                    ddlPayrollType.DataSource = dtPayrollType;
                    ddlPayrollType.DataTextField = GTIService.Constants.HRMS.Employee.Fields.PTM_NAME;
                    ddlPayrollType.DataValueField = GTIService.Constants.HRMS.Employee.Fields.PTM_PK;
                    ddlPayrollType.Items.HtmlDecode();
                    ddlPayrollType.DataBind();
                    ddlPayrollType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                default:
                    break;
            }
        }

        #region BindGrid
        /// <summary>
        /// Method for Grid binding
        /// </summary>
        //public void BindGrid(ControlsEnum controlType)
        //{
        //    try
        //    {
        //        switch (controlType)
        //        {
        //            #region REVISIONHISTORY
        //            case ControlsEnum.REVISIONHISTORY:                        
        //                grdRevisionHistory.DataSource = dtResult;
        //                grdRevisionHistory.DataBind();
        //                break;
        //            #endregion                    
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        private int? GetNullableInt(string str)
        {
            int result;
            return (int.TryParse(str, out result) ? (int?)result : null);
        }

        private void ResetForm()
        {
            txtEmpBasic.Text = string.Empty;
            txtEmpNetSalary.Text = string.Empty;
            txtTotalCTC.Text = string.Empty;
            txtTotalGross.Text = string.Empty;
            ddlSalaryTemplate.ClearSelection();
            ddlSalaryTemplate.Items.Clear();
            UCEmpSalary.EmpSalaryHdr = null;
            UCEmpSalary.EmployeePK = 0;
            CurrPK = 0;
        }

        #endregion

        #region Action Handler
        protected void ActionHandler(object sender, EventArgs e)
        {
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;
            try
            {
                int? result;
                result = 0;

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
                    if (((DropDownList)sender).ID == "ddlSalaryTemplate")
                    {
                        commonActions = BusinessObject.AccountManagement.ActionsEnum.TEMPLATECHANGED;
                    }
                    else if (((DropDownList)sender).ID == "ddlPayrollType")
                    {
                        commonActions = BusinessObject.AccountManagement.ActionsEnum.TYPECHANGED;
                    }
                }
                switch (commonActions)
                {
                    #region SAVE
                    case BusinessObject.AccountManagement.ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            objEmpSalaryDetails = new EmpTemplateHeader();
                            objEmpSalaryDetails = (EmpTemplateHeader)SetUIValuesToObject(ControlsEnum.EMPSALARYHEADER);
                            if (objEmpSalaryDetails != null)
                            {
                                if (objEmpSalaryDetails.SalaryDtl != null)
                                {
                                    if (objEmpSalaryDetails.SalaryDtl.GroupBy(pe => pe.STS_PAY_ELEMENT).Any(dup => dup.Count() > 1))
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_DuplicatePayElement").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                        break;
                                    }
                                }
                                string xmlDoc = CommonFunctions.XmlSerialize<EmpTemplateHeader>(objEmpSalaryDetails);
                                result = EmployeeSalaryBL.SaveEmployeeSalaryDetails(xmlDoc);
                                if (result > 0)
                                {
                                    // Show Save Message and redirected to listing page                                        
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryDetails);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EmployeeList) + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.Captions.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Error_Ref;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryDetails);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.Captions.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.SalaryDetails + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.INCORRECT)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_SalaryKeyBaseExists").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.SalaryDetails + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.Captions.Information + "');", true);

                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SlabDefinitions);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.Captions.Information + "');", true);
                                }

                            }
                        }
                        break;
                    #endregion

                    #region SAVEANDCONTINUE
                    case BusinessObject.AccountManagement.ActionsEnum.SAVEANDCONTINUE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            objEmpSalaryDetails = new EmpTemplateHeader();
                            objEmpSalaryDetails = (EmpTemplateHeader)SetUIValuesToObject(ControlsEnum.EMPSALARYHEADER);
                            if (objEmpSalaryDetails != null)
                            {
                                string xmlDoc = CommonFunctions.XmlSerialize<EmpTemplateHeader>(objEmpSalaryDetails);
                                result = EmployeeSalaryBL.SaveEmployeeSalaryDetails(xmlDoc);
                                if (result > 0)
                                {
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryDetails);
                                     CommonBL userAuth = new CommonBL();
                                     string nextPageUrl = BusinessLogic.HRMS.Common.HRMSCommonBL.GetNextTabUrl(Convert.ToInt16(EmpTabEnum.Salary), GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpTabNextURL").ToString().Split(','),
                                                            GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpInactiveTabs").ToString().Split(','), Resources.PageURL.EmployeeList.ToString());
                                     if (userAuth.IsUserHasRights(currentUser.PKUser, nextPageUrl.Replace("~", ""), currentUser.SBUID, currentUser.CurrentDeptPK))
                                     {
                                         ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                         + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(nextPageUrl) + "');", true);
                                     }
                                     else
                                     {
                                         ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                             + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EmployeeList) + "');", true);
                                     }
                                }
                                else if (result == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.Captions.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Error_Ref;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryDetails);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.Captions.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.SalaryDetails + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.SalaryDetails + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.Captions.Information + "');", true);

                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SlabDefinitions);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.Captions.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region TEMPLATECHANGED
                    case BusinessObject.AccountManagement.ActionsEnum.TEMPLATECHANGED:
                        GetFieldValues(ControlsEnum.SALARYTEMPLATEDETAILS);
                        SetFieldValues(ControlsEnum.SALARYTEMPLATEDETAILS);
                        break;
                    #endregion

                    #region PAYROLL TYPE CHANGED
                    case BusinessObject.AccountManagement.ActionsEnum.TYPECHANGED:
                        txtEmpNetSalary.Text = 0.ToString(hdfCurrencyFormat.Value);
                        txtTotalCTC.Text = 0.ToString(hdfCurrencyFormat.Value);
                        txtTotalGross.Text = 0.ToString(hdfCurrencyFormat.Value);
                        GetFieldValues(ControlsEnum.SALARYTEMPLATE);
                        SetFieldValues(ControlsEnum.SALARYTEMPLATE);
                        GetFieldValues(ControlsEnum.SALARYTEMPLATEDETAILS);
                        SetFieldValues(ControlsEnum.SALARYTEMPLATEDETAILS);
                        break;
                    #endregion

                    #region RESETSALARY
                    case BusinessObject.AccountManagement.ActionsEnum.RESETSALARY:
                        GetFieldValues(ControlsEnum.EMPLOYEESALARY);
                        SetFieldValues(ControlsEnum.SALARYTEMPLATEDETAILS);
                        GetUIValuesFromObject(ControlsEnum.EMPSALARYHEADER);
                        break;
                    #endregion

                    #region CANCEL
                    case BusinessObject.AccountManagement.ActionsEnum.CANCEL:
                        ResetForm();
                        Session["SalaryCancelClicked"] = 1;
                        Response.Redirect(Resources.PageURL.EmployeeList, false);
                        break;
                    #endregion
                    #region DELETE
                    case BusinessObject.AccountManagement.ActionsEnum.DELETE:
                        string routeURL = Resources.PageURL.EmployeeList.ToString();
                        if (this.CurrPK > 0)
                        {
                            result = EmployeeSalaryBL.DeleteEmployeeSalary(CurrPK, LastModifiedTime);
                            DbDeleteStatus deleteStatus = (DbDeleteStatus)result;
                            switch (deleteStatus)
                            {
                                case DbDeleteStatus.DELETED://If deletion is success
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryDetails);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EmployeeList) + "');", true);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    //    + "','" + Resources.ErpRes.Information + "','" + routeURL + "');", true);
                                    break;
                                case DbDeleteStatus.REFERRED://If referred to another page                                    
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryDetails);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "','" + routeURL + "');", true);
                                    break;
                                case DbDeleteStatus.SQLERROR://Sql error
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryDetails);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "','" + routeURL + "');", true);
                                    break;
                                case DbDeleteStatus.CONCURRENCY:
                                    litErrorMsg.Text = Resources.PageNameRes.SalaryDetails + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                                           + "','" + Resources.ErpRes.Information + "','" + routeURL + "');", true);
                                    break;
                                case DbDeleteStatus.DELETECONCURRENCY:
                                    litErrorMsg.Text = Resources.PageNameRes.SalaryDetails + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "','" + routeURL + "');", true);
                                    break;
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + routeURL + "');", true);
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>ActionHandler
        /// Action Handler For GridViewCommandEventArgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        //{
        //    GridView senderGridView = (GridView)sender;
        //    int slNo;
        //    if (senderGridView.ID == "grdRevisionHistory")
        //    {
        //        if (e.CommandName == "EDIT_ACTION")
        //        {
        //            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
        //            GetFieldValues(ControlsEnum.EMPLOYEESALARYREVISION);
        //            SetFieldValues(ControlsEnum.EMPLOYEESALARYREVISION);                                    
        //        }
        //        else if (e.CommandName == "SET_ACTION")
        //        {
        //            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
        //            string s = ((HiddenField)row.FindControl("hdfEffectiveFrom")).Value;
        //            UCEmpSalary.EffectTo = s;
        //            GetFieldValues(ControlsEnum.EMPLOYEESALARY);
        //            SetFieldValues(ControlsEnum.SALARYTEMPLATEDETAILS);
        //            IsRevisionSetAsCurrent = true;
        //        }
        //    }
        //}

        #endregion

        #region Page Events
        /// <summary>
        /// To Handle Page Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {
                PageActionHandler();
            }
        }

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            if (EntryStatus == EntryStatus.VIEWMODE)           
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);        
        }

        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSalDelete.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSalDelete.Load += new EventHandler(btnAction_Load);
            this.btnCancel.Load += new EventHandler(btnAction_Load);           
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

        /// <summary>
        /// To handle OnInit event Used to assign the Event for all the actions used in this page        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);            
        }
        #endregion

        //#region grdRevisionHistory_DataBound
        //protected void grdRevisionHistory_DataBound(object sender, EventArgs e)
        //{
        //    if (grdRevisionHistory.Rows.Count > 0) ((Panel)grdRevisionHistory.Rows[0].FindControl("pnlAction")).Visible = true;
        //}
        //#endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            EMPLOYEEDETAILSHEADER,
            SALARYTEMPLATE,
            SALARYTEMPLATEDETAILS,
            EMPLOYEESALARY,
            EMPSALARYHEADER,
            //,REVISIONHISTORY,
            //EMPLOYEESALARYREVISION
            PAYROLLTYPE,
            PAYROLLTYPEUSERMAPPING
        }

        #endregion
    }
}