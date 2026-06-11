using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject.HRMS.Employee;
using BusinessObject.HRMS.Admin.Masters;
using BusinessObject.CommonManagement;
using BusinessLogic.HRMS.Employee;
using ERPSMS_v01;

namespace HRMS.Employees
{
    public partial class PayDetails : System.Web.UI.Page
    {
        /// <summary>
        /// Current PK
        /// </summary>
        private int CurrPK
        {
            get
            {
                return (int)(Session[ERP.Utilities.SessionStrings.CurrentPK] ?? 0);
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.CurrentPK] = value;
            }
        }

        /// <summary>
        /// Select PK
        /// </summary>
        private int SelectedPK
        {
            get
            {
                return (int)(this.ViewState[ViewstateStrings.SelectedPK] ?? 0);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedPK] = value;
            }
        }

        private DateTime LastModifiedTime
        {
            get
            {
                return (DateTime)(this.ViewState[ViewstateStrings.LastModifiedTime] ?? System.DateTime.Now);
            }
            set
            {
                this.ViewState[ViewstateStrings.LastModifiedTime] = value;
            }
        }

        //private int SelectedDocPk;
        private ActionsEnum commonActions = ActionsEnum.DEFAULT;
        private DataTable pageData;
        private BusinessObject.User currentUser;
        private EmployeePayDetailsBO selectedEmployeePayDetails;
        private int dummyPk=0;
        private List<WorkingHours> _workingHoursList = null;
        bool empTypeChanged = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            this.currentUser = (BusinessObject.User)Context.User.Identity;
            if (this.CurrPK == 0)
            {
                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                Session["SelectMessage"] = litErrorMsg.Text;
                Response.Redirect(Resources.PageURL.EmployeeList);
            }
            else
            {
                if (!IsPostBack)
                {
                    InitializePage();
                    GetFieldValues(ControlsEnum.EMPLOYEEDETAILSHEADER);
                    SetFieldValues(ControlsEnum.EMPLOYEEDETAILSHEADER);
                    GetFieldValues(ControlsEnum.SELECTEDPEYDETAILS);
                    SetFieldValues(ControlsEnum.SELECTEDPEYDETAILS);
                }
            }
        }
        #endregion

        private void InitializePage()
        {
        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "initComponents", "$(document).ready(function(){initComponents();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       

        protected void ActionHandler(object sender, EventArgs e)
        {
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;
            try
            {
                int result;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlPaymentMode")
                    {
                        commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                    }
                    if (((DropDownList)sender).ID == "ddlEmployementType")
                    {
                        commonActions = ActionsEnum.WORKINGHOURS;
                    }
                }
                switch (commonActions)
                {
                    case ActionsEnum.DEFAULT:
                        break;  
                    case ActionsEnum.GOHOME:
                        Session["DocCancelListClicked"] = 1;
                        Session["PayDetailCancelClicked"] = 1;
                        Response.Redirect(Resources.PageURL.EmployeeList, true); // Response.Redirect("~/Employees/EmployeeList.aspx");
                        break;                   
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        //Session["DocCancelClicked"] = 1;
                        Session["PayDetailCancelClicked"] = 1;
                        Response.Redirect(Resources.PageURL.EmployeeList, true); // Response.Redirect("~/Employees/EmployeeList.aspx");
                        break;
                    #endregion
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        DisableBankDetails(ddlPaymentMode.SelectedValue == "4");
                        break;
                    case ActionsEnum.WORKINGHOURS:
                        empTypeChanged = true;
                        GetFieldValues(ControlsEnum.WORKINGDAYS);
                        SetFieldValues(ControlsEnum.WORKINGDAYS);                        
                        break;
                    #region Save
                    case ActionsEnum.SAVE:
                        selectedEmployeePayDetails = (EmployeePayDetailsBO)SetUiValuesToObject(commonActions);
                        DateTime EmpLastModDate;
                        // result = EmployeeDocBL.Save(selectedEmployeeDoc);
                        result = EmployeePayDetailsBL.Save(selectedEmployeePayDetails, out EmpLastModDate);
                        if (result >= 0) // Success ! re-initialize the page
                        {
                            //Session["DocSaveClicked"] = 1;                            
                            Session["PayDetailCancelClicked"] = 1;                      
                            ResetForm(ActionsEnum.SAVE);
                            litErrorMsg.Text = string.Format(Resources.ErrorMessages.Msg_Save_Success, Resources.PageNameRes.EmployeePayDetails);
                            //litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.); // By Biju
                            
                            Response.Redirect(Resources.PageURL.EmployeeList, true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                      + "','" + Resources.ErrorMessages.Msg_Heading_Information + "');", true);
                        }
                        else
                        {
                            #region Db Error Checking
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeePayDetails + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYEXIST)
                            {
                                litErrorMsg.Text = Resources.Messages.PaymentDetailsAlreadyExist;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeePayDetails);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            #endregion
                        }
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (SelectedPK > 0)
                        {
                            result = EmployeePayDetailsBL.DeletePayDetails(this.SelectedPK, this.LastModifiedTime);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeePayDetails);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                Session["DocCancelClicked"] = 1;
                                Response.Redirect("~/Employees/EmployeeList.aspx",false);
                            }
                            else
                            {
                                if (result == (int)DbDeleteStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeType + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                   
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeType + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeType + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeType);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex)
                                          + "','" + Resources.ErpRes.Information + "');", true);
            }
        }
        protected void ResetForm(ActionsEnum action)
        {
            switch (action)
            {
                case ActionsEnum.SAVE:
                    GetFieldValues(ControlsEnum.SELECTEDPEYDETAILS);
                    SetFieldValues(ControlsEnum.SELECTEDPEYDETAILS);
                    break;
                case ActionsEnum.NEW:
                    clearControls();
                    GetFieldValues(ControlsEnum.PAYMENTMODE);
                    SetFieldValues(ControlsEnum.PAYMENTMODE);
                    GetFieldValues(ControlsEnum.SALARYTEMPLATE);
                    SetFieldValues(ControlsEnum.SALARYTEMPLATE);
                    GetFieldValues(ControlsEnum.BANK);
                    SetFieldValues(ControlsEnum.BANK);
                    GetFieldValues(ControlsEnum.EMPLOYEEMENTTYPE);
                    SetFieldValues(ControlsEnum.EMPLOYEEMENTTYPE);
                    GetFieldValues(ControlsEnum.WORKINGDAYS);
                    SetFieldValues(ControlsEnum.WORKINGDAYS);                    
                    GetFieldValues(ControlsEnum.LEAVETEMPLATE);
                    SetFieldValues(ControlsEnum.LEAVETEMPLATE);
                    GetFieldValues(ControlsEnum.OTTEMPLATE);
                    SetFieldValues(ControlsEnum.OTTEMPLATE);
                    break;
            }
        }


        private void clearControls()
        {
            this.SelectedPK = 0;
            lblLastModifiedHDR.Visible = false;            
        }

        

        #region Common UI Methods
        /// <summary>
        /// Getting Data From Db To Fields
        /// </summary>
        /// <param name="controlsEnum">Field</param>
        private void GetFieldValues(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                case ControlsEnum.EMPLOYEEDETAILSHEADER:
                    pageData = EmployeePayDetailsBL.GetEmployeeHeaderInfo(CurrPK);
                    break;              
                case ControlsEnum.PAYMENTMODE:
                    pageData = EmployeePayDetailsBL.GetPaymentMode(currentUser.SBUID);
                    break;
                case ControlsEnum.BANK:
                    int bankType = 2;
                    pageData = EmployeePayDetailsBL.GetBankNames(currentUser.SBUID, bankType, null);
                    break;
                case ControlsEnum.SALARYTEMPLATE:
                    if(this.SelectedPK > 0) 
                        dummyPk = this.selectedEmployeePayDetails.EPD_SALARY_TEMP;
                    pageData = EmployeePayDetailsBL.GetSalaryTemplate(currentUser.SBUID, dummyPk, (int)DbActiveStatus.ACTIVE);
                    break;
                case ControlsEnum.OTTEMPLATE:
                    if (this.SelectedPK > 0)
                        dummyPk = !string.IsNullOrEmpty(selectedEmployeePayDetails.EPD_OT_TEMP) ? Convert.ToInt32(selectedEmployeePayDetails.EPD_OT_TEMP) : 0;
                    pageData = EmployeePayDetailsBL.GetOTTemplateList(currentUser.SBUID, dummyPk, (int)DbActiveStatus.ACTIVE);
                    break;
                case ControlsEnum.LEAVETEMPLATE:
                    if(this.SelectedPK > 0)
                        dummyPk = !string.IsNullOrEmpty(selectedEmployeePayDetails.EPD_LEAVE_TEMP) ? Convert.ToInt32(selectedEmployeePayDetails.EPD_LEAVE_TEMP) : 0;
                    pageData = EmployeePayDetailsBL.GetLeaveTemplateList(currentUser.SBUID, dummyPk, (int)DbActiveStatus.ACTIVE);
                    break;
                case ControlsEnum.EMPLOYEEMENTTYPE:
                    if (this.SelectedPK > 0)
                        dummyPk = this.selectedEmployeePayDetails.EPD_EMP_TYPE;
                    pageData = EmployeePayDetailsBL.GetEmployeeTypeGetKV(dummyPk==0?(int?)null:dummyPk, currentUser.SBUID, (int)DbActiveStatus.ACTIVE);
                    break;
                case ControlsEnum.SELECTEDPEYDETAILS:
                    this.selectedEmployeePayDetails = EmployeePayDetailsBL.GetEmployeePayDetailsByID(this.CurrPK, (int)DbActiveStatus.ACTIVE);
                    break;
                case ControlsEnum.WORKINGDAYS:
                    _workingHoursList = new List<WorkingHours>();
                    _workingHoursList.Add(new WorkingHours { WeekDay = 1, Hours = 0 });
                    _workingHoursList.Add(new WorkingHours { WeekDay = 2, Hours = 0 });
                    _workingHoursList.Add(new WorkingHours { WeekDay = 3, Hours = 0 });
                    _workingHoursList.Add(new WorkingHours { WeekDay = 4, Hours = 0 });
                    _workingHoursList.Add(new WorkingHours { WeekDay = 5, Hours = 0 });
                    _workingHoursList.Add(new WorkingHours { WeekDay = 6, Hours = 0 });
                    _workingHoursList.Add(new WorkingHours { WeekDay = 7, Hours = 0 });

                    if (empTypeChanged)
                    {
                        int empTypePk = Convert.ToInt32(ddlEmployementType.SelectedValue);
                        pageData = EmployeePayDetailsBL.GetWorkingDays(empTypePk);
                        foreach (DataRow row in pageData.Rows)
                        {
                            WorkingHours wh = _workingHoursList.Find(x => x.WeekDay == Convert.ToInt32(row["ESH_WEEK_DAY"]));
                            wh.Pk = Convert.ToInt32(row["ESH_PK"]);
                            wh.Hours = Convert.ToDouble(row["ESH_HOURS"]);
                            wh.EmployeeType = Convert.ToInt32(row["ESH_EMP_TYPE"]);
                            wh.Active = Convert.ToInt32(row["ESH_ACTIVE"]);
                        }
                    }
                    else if (this.SelectedPK > 0)
                    {
                        if (selectedEmployeePayDetails != null)
                        {
                            foreach (var item in selectedEmployeePayDetails.WorkingHours)
                            {
                                WorkingHours wh = _workingHoursList.Find(x => x.WeekDay == item.WeekDay);
                                wh.Pk = item.Pk;
                                wh.Hours = item.Hours;
                                wh.EmployeeType = item.EmployeeType;
                                wh.Active = item.Active;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Used To Setting UI
        /// Calling BindGrid,BindDropDown,GetUIValuesToObject Methods From Here
        /// </summary>
        /// <param name="controlType"></param>
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.EMPLOYEEDETAILSHEADER:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.PAYMENTMODE:
                        BindDropDown(ControlsEnum.PAYMENTMODE);
                        break;
                    case ControlsEnum.BANK:
                        BindDropDown(ControlsEnum.BANK);
                        break;
                    case ControlsEnum.SALARYTEMPLATE:
                        BindDropDown(ControlsEnum.SALARYTEMPLATE);
                        break;
                    case ControlsEnum.LEAVETEMPLATE:
                        BindDropDown(ControlsEnum.LEAVETEMPLATE);
                        break;
                    case ControlsEnum.OTTEMPLATE:
                        BindDropDown(ControlsEnum.OTTEMPLATE);
                        break;
                    case ControlsEnum.EMPLOYEEMENTTYPE:
                        BindDropDown(ControlsEnum.EMPLOYEEMENTTYPE);
                        break;
                    #region Working Days
                    case ControlsEnum.WORKINGDAYS:
                        BindGrid(ControlsEnum.WORKINGDAYS);
                        break;
                    #endregion
                    case ControlsEnum.SELECTEDPEYDETAILS:
                        if (this.selectedEmployeePayDetails != null)
                        {
                            GetUIValuesFromObject(ControlsEnum.SELECTEDPEYDETAILS);
                        }
                        else
                        {
                            ResetForm(ActionsEnum.NEW);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DisableBankDetails(bool canEnable)
        {
            //Labels
            lblBankName.Enabled = canEnable;
            lblAccountCode.Enabled = canEnable;
            lblAccountName.Enabled = canEnable;
            lblIFSCCode.Enabled = canEnable;
            lblPFAccount.Enabled = canEnable;
            lblPFEffectiveDate.Enabled = canEnable;
            lblSOCSOAccount.Enabled = canEnable;
            lblSOCSOEffectiveDate.Enabled = canEnable;
            lblBranch.Enabled = canEnable;

            //Input
            ddlBankName.Enabled = canEnable;
            txtAccountCode.Enabled = canEnable;
            txtAccountName.Enabled = canEnable;
            txtIFSCCode.Enabled = canEnable;
            txtPFAccount.Enabled = canEnable;
            txtPFEffectiveDate.Enabled = canEnable;
            txtSOCSOAccount.Enabled = canEnable;
            txtSOCSOEffectiveDate.Enabled = canEnable;
            txtBranch.Enabled = canEnable;

            //Validators
            rfvBankName.Enabled = canEnable;
            rfvAccountCode.Enabled = canEnable;
            rfvAccountName.Enabled = canEnable;
            rfvBranch.Enabled = canEnable;
            rfvIFSCCode.Enabled = canEnable;
        }

        /// <summary>
        /// Used To Setting UI Components like TextBox
        /// </summary>
        /// <param name="controlsEnum"></param>
        private void GetUIValuesFromObject(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                case ControlsEnum.SELECTEDPEYDETAILS:
                    BindDetailsView();
                    break;
                case ControlsEnum.EMPLOYEEDETAILSHEADER:
                    if (pageData != null)
                    {
                        lblhdrEmployeeNoTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(pageData.Rows[0]["empCode"].ToString()), 20);
                        lblhdrEmployeeNameTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(pageData.Rows[0]["empNameText"].ToString()), 20);
                        lblhdrDOJText.Text = pageData.Rows[0]["empDOJText"].ToString();
                        lblhdrDOBTxt.Text = pageData.Rows[0]["empDOBText"].ToString();
                        lblhdrDesignationTxt.Text = HttpUtility.HtmlDecode(pageData.Rows[0]["empDesignationText"].ToString());
                        lblhdrDepartmentTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(pageData.Rows[0]["empDepartmentText"].ToString()), 20);

                        hdfHdrDOB.Value = pageData.Rows[0]["empDOB"] == DBNull.Value
                                          ? string.Empty
                                          : Convert.ToDateTime(pageData.Rows[0]["empDOB"]).ToString("dd-MMM-yyyy");
                        lblhdrEmployeeNameTxt.ToolTip = HttpUtility.HtmlDecode(pageData.Rows[0]["empNameText"].ToString());
                        lblhdrEmployeeNoTxt.ToolTip = HttpUtility.HtmlDecode(pageData.Rows[0]["empCode"].ToString());
                        lblhdrDepartmentTxt.ToolTip = HttpUtility.HtmlDecode(pageData.Rows[0]["empDepartmentText"].ToString());
                        lblhdrDOJText.ToolTip = pageData.Rows[0]["empDOJText"].ToString();
                        lblhdrDOBTxt.ToolTip = pageData.Rows[0]["empDOBText"].ToString();
                        lblhdrDesignationTxt.ToolTip = HttpUtility.HtmlDecode(pageData.Rows[0]["empDesignationText"].ToString());
                    }
                    break;
                default:
                    break;
            }
        }

        private void BindDetailsView()
        {
            if (selectedEmployeePayDetails != null)
            {
                try
                {
                    this.SelectedPK = selectedEmployeePayDetails.EPD_PK;
                    btnDelete.Visible = this.SelectedPK != 0;
                    txtBasic.Text = selectedEmployeePayDetails.EPD_BASIC_PAY.ToString();
                    txtPANNo.Text = HttpUtility.HtmlDecode(selectedEmployeePayDetails.EPD_PAN_NO);
                    GetFieldValues(ControlsEnum.PAYMENTMODE);
                    SetFieldValues(ControlsEnum.PAYMENTMODE);
                    ddlPaymentMode.SelectedValue = selectedEmployeePayDetails.EPD_PAY_MODE.ToString();

                    GetFieldValues(ControlsEnum.SALARYTEMPLATE);
                    SetFieldValues(ControlsEnum.SALARYTEMPLATE);
                    ddlSalaryTemplate.SelectedValue = selectedEmployeePayDetails.EPD_SALARY_TEMP.ToString();

                    GetFieldValues(ControlsEnum.BANK);
                    SetFieldValues(ControlsEnum.BANK);
                    if (!string.IsNullOrWhiteSpace(selectedEmployeePayDetails.EPD_PAY_BANK))
                    {
                        ddlBankName.SelectedValue = selectedEmployeePayDetails.EPD_PAY_BANK.ToString(); 
                    }

                    txtBranch.Text = HttpUtility.HtmlDecode(selectedEmployeePayDetails.EPD_PAY_BANK_BRANCH);
                    txtAccountCode.Text = HttpUtility.HtmlDecode(selectedEmployeePayDetails.EPD_BANK_AC_NO);
                    txtAccountName.Text = HttpUtility.HtmlDecode(selectedEmployeePayDetails.EPD_BANK_AC_NAME);
                    txtIFSCCode.Text = HttpUtility.HtmlDecode(selectedEmployeePayDetails.EPD_BANK_IFSC);
                    txtPFAccount.Text = HttpUtility.HtmlDecode(selectedEmployeePayDetails.EPD_PF_AC);
                    txtPFEffectiveDate.Text = selectedEmployeePayDetails.EPD_PF_DATE;
                    txtSOCSOAccount.Text = HttpUtility.HtmlDecode(selectedEmployeePayDetails.EPD_SOCSO_AC);
                    txtSOCSOEffectiveDate.Text = selectedEmployeePayDetails.EPD_SOCSO_DATE;

                    GetFieldValues(ControlsEnum.EMPLOYEEMENTTYPE);
                    SetFieldValues(ControlsEnum.EMPLOYEEMENTTYPE);
                    ddlEmployementType.SelectedValue = selectedEmployeePayDetails.EPD_EMP_TYPE.ToString();

                    GetFieldValues(ControlsEnum.WORKINGDAYS);
                    SetFieldValues(ControlsEnum.WORKINGDAYS);

                    GetFieldValues(ControlsEnum.LEAVETEMPLATE);
                    SetFieldValues(ControlsEnum.LEAVETEMPLATE);
                    ddlLeaveTemplate.SelectedValue = selectedEmployeePayDetails.EPD_LEAVE_TEMP.ToString();

                    GetFieldValues(ControlsEnum.OTTEMPLATE);
                    SetFieldValues(ControlsEnum.OTTEMPLATE);
                    ddlOTTemplate.SelectedValue = selectedEmployeePayDetails.EPD_OT_TEMP.ToString();

                    this.LastModifiedTime = selectedEmployeePayDetails.LastModifiedDate;
                    lblLastModifiedHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);
                    lblLastModifiedHDR.Visible = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                lblLastModifiedHDR.Visible = false;
            }
        }

        /// <summary>
        /// Collect All Data From UI (including Complex Object) to Local Field Variables
        /// </summary>
        /// <param name="controlsEnum"></param>
        private object SetUiValuesToObject(ActionsEnum mode)
        {
            object returnObj=null;
            //returnObj = null;

            DateTime dummyDateField;
            int dummyIntField;
            double dummyDouble;
            try
            {
                switch (mode)
                {
                    case ActionsEnum.SAVE:
                        EmployeePayDetailsBO obj = new EmployeePayDetailsBO();
                        obj.EPD_EMPLOYEE = this.CurrPK;
                        obj.EPD_PK = this.SelectedPK;

                        if (Double.TryParse(txtBasic.Text.Trim(), out dummyDouble))
                            obj.EPD_BASIC_PAY = dummyDouble;
                        else
                            throw new InvalidCastException("invalid basic pay");

                        obj.EPD_PAN_NO = txtPANNo.Text.Trim();
                        obj.EPD_SALARY_TEMP = Convert.ToInt32(ddlSalaryTemplate.SelectedValue);
                        obj.EPD_PAY_MODE = Convert.ToInt32(ddlPaymentMode.SelectedValue);
 
                        if (ddlPaymentMode.SelectedValue == "4") // Bank
                        {
                            obj.EPD_PAY_BANK = ddlBankName.SelectedValue;
                            obj.EPD_PAY_BANK_BRANCH = txtBranch.Text.Trim().HtmlEncode();
                            obj.EPD_BANK_AC_NO = txtAccountCode.Text.Trim().HtmlEncode();
                            obj.EPD_BANK_AC_NAME = txtAccountName.Text.Trim().HtmlEncode();
                            obj.EPD_BANK_IFSC = txtIFSCCode.Text.Trim().HtmlEncode();
                            obj.EPD_PF_AC = txtPFAccount.Text.Trim().HtmlEncode();
                            obj.EPD_SOCSO_AC = txtSOCSOAccount.Text.Trim().HtmlEncode();
                            obj.EPD_PF_DATE = txtPFEffectiveDate.Text.Trim().IsNullOrEmptyOrWhitespace() ? string.Empty : txtPFEffectiveDate.Text.Trim();
                            obj.EPD_SOCSO_DATE = txtSOCSOEffectiveDate.Text.Trim().IsNullOrEmptyOrWhitespace() ? string.Empty 
                                : txtSOCSOEffectiveDate.Text.Trim();
                        }

                        obj.EPD_EMP_TYPE = Convert.ToInt32(ddlEmployementType.SelectedValue);
                        obj.EPD_LEAVE_TEMP = ddlLeaveTemplate.SelectedValue;
                        obj.EPD_OT_TEMP = ddlOTTemplate.Text;

                        //Set Working Days Here
                        
                        obj.WorkingHours = (List<WorkingHours>)SetUiValuesToObject(ActionsEnum.DETAIL);

                        obj.BIZUNIT_PK = currentUser.SBUID;
                        obj.ACTIVE = (int)DbActiveStatus.ACTIVE;
                        obj.LastModifiedDate = this.LastModifiedTime;
                        obj.USER_PK = currentUser.PKUser;
                        returnObj = obj;
                        break;
                    case ActionsEnum.DETAIL:
                        List<WorkingHours> workingHrsList = null;

                        if (grdWorkingDays.Rows.Count > 0)
                        {
                            workingHrsList = new List<WorkingHours>();
                            foreach (GridViewRow row in grdWorkingDays.Rows)
                            {
                                double hrs = 0;
                                if (Double.TryParse(((TextBox)row.FindControl("txtHrs")).Text.Trim(), out dummyDouble))
                                    hrs = dummyDouble;
                                if (hrs > 0)
                                {
                                    int pk = 0;
                                    Int32.TryParse(((HiddenField)row.FindControl("hdfWHrsPK")).Value.Trim(), out pk);
                                    int weekDay = Convert.ToInt32(((HiddenField)row.FindControl("hdfWeekDay")).Value.Trim());
                                    int empID = Convert.ToInt32(((HiddenField)row.FindControl("hdfWHrsEmpTypeID")).Value.Trim());

                                    WorkingHours workingHrs = new WorkingHours
                                    {
                                        Pk = pk,
                                        WeekDay = weekDay,
                                        EmployeeType = empID,
                                        Hours = hrs
                                    };
                                    workingHrsList.Add(workingHrs);
                                }
                            }
                        }
                        returnObj = workingHrsList;
                        break;

                }
                return returnObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                returnObj = null;
            }
        }

        private void BindGrid(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                #region Working Days
                case ControlsEnum.WORKINGDAYS:
                    grdWorkingDays.DataSource = _workingHoursList;
                    grdWorkingDays.DataBind();
                    break;
                #endregion
            }
        }

        private void BindDropDown(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                case ControlsEnum.PAYMENTMODE:
                    ddlPaymentMode.DataSource = pageData;
                    ddlPaymentMode.DataTextField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_TEXT;
                    ddlPaymentMode.DataValueField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_VALUE;
                    ddlPaymentMode.DataBind();
                    ddlPaymentMode.Items.HtmlDecode();
                    ddlPaymentMode.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.BANK:
                    ddlBankName.DataSource = pageData;
                    ddlBankName.DataTextField = GTIService.Constants.HRMS.Employee.Fields.CBM_NAME;
                    ddlBankName.DataValueField = GTIService.Constants.HRMS.Employee.Fields.CBM_PK;
                    ddlBankName.DataBind();
                    ddlBankName.Items.HtmlDecode();
                    ddlBankName.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.SALARYTEMPLATE:
                    ddlSalaryTemplate.DataSource = pageData;
                    ddlSalaryTemplate.DataTextField = GTIService.Constants.HRMS.Employee.Fields.STE_NAME;
                    ddlSalaryTemplate.DataValueField = GTIService.Constants.HRMS.Employee.Fields.STE_PK;
                    ddlSalaryTemplate.DataBind();
                    ddlSalaryTemplate.Items.HtmlDecode();
                    ddlSalaryTemplate.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.EMPLOYEEMENTTYPE:
                    ddlEmployementType.DataSource = pageData;
                    ddlEmployementType.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.EMT_NAME;
                    ddlEmployementType.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.EMT_PK;
                    ddlEmployementType.DataBind();
                    ddlEmployementType.Items.HtmlDecode();
                    ddlEmployementType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.LEAVETEMPLATE:
                    ddlLeaveTemplate.DataSource = pageData;
                    ddlLeaveTemplate.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.LTE_NAME;
                    ddlLeaveTemplate.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.LTE_PK;
                    ddlLeaveTemplate.DataBind();
                    ddlLeaveTemplate.Items.HtmlDecode();
                    //ddlLeaveTemplate.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.OTTEMPLATE:
                    ddlOTTemplate.DataSource = pageData;
                    ddlOTTemplate.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.OTE_NAME;
                    ddlOTTemplate.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.OTE_PK;
                    ddlOTTemplate.DataBind();
                    ddlOTTemplate.Items.HtmlDecode();
                    //ddlOTTemplate.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                default:
                    break;
            }
        }
        #endregion
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            DETAIL,
            SELECTEDPEYDETAILS,
            LOCATIONLIST,
            WORKINGDAYS,
            EMPLOYEEDETAILSHEADER,
            PAYMENTMODE,
            BANK,
            SALARYTEMPLATE,
            EMPLOYEEMENTTYPE,
            OTTEMPLATE,
            LEAVETEMPLATE
        }
    }

    
}