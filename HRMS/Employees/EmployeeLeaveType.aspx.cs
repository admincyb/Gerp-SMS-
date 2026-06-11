using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.HRMS.Employee;
using BusinessObject.AccountManagement;
using BusinessLogic.HRMS.Employee;
using DataAccess.CommonManagement;
using System.Data;
using ERPData;
using ERPService;
using ERPManager;
using BusinessObject.CommonManagement;
using BusinessObject;
using System.IO;
using CustomControls;
using BusinessLogic.CommonManagement;
using ERPSMS_v01;


namespace HRMS.Employees
{
    public partial class EmployeeLeaveType : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        #region  Properties
        public int CurrPK
        {
            get
            {
                return Convert.ToInt32(Session[SessionStrings.CurrentPK]);
            }
            set
            {
                Session[SessionStrings.CurrentPK] = value;
            }
        }
        public DateTime LastModifiedTime
        {
            get
            {
                return ViewState[ViewstateStrings.LastModifiedTime] == null ? System.DateTime.Now : (DateTime)this.ViewState[ViewstateStrings.LastModifiedTime];
            }
            set
            {
                ViewState[ViewstateStrings.LastModifiedTime] = value;
            }
        }
        /// <summary>
        /// Entry State for managing display status
        /// </summary>
        private BusinessObject.Common.EntryStatus EntryStatus
        {
            get
            {
                return this.Session[ViewstateStrings.EntryState] == null ? BusinessObject.Common.EntryStatus.ENTRYMODE : (BusinessObject.Common.EntryStatus)(this.Session[ViewstateStrings.EntryState]);
            }
            set
            {
                this.Session[ViewstateStrings.EntryState] = value;
            }
        }
        //private int categoryId
        //{
        //    get
        //    {
        //        return this.ViewState[ViewstateStrings.categoryId] == null ? 0 : (int)this.ViewState[ViewstateStrings.categoryId];
        //    }
        //    set
        //    {
        //        this.ViewState[ViewstateStrings.categoryId] = value;
        //    }
        //}
        #endregion
        #region  Variables

        private EmployeeBasicInfomtn objEmployeeBasicInfo;
        //DataSet dsSkillCategory;
        private EmployeeSkillsDetails EmployeeSkillsDetailsObj;
        private EmpLeaveType EmployeeLeaveTypeObj;
        //private DataTable dtSkillLevel;
        DropDownList ddlSkillLevel;

        DataTable dtLeaveTypeList;
        DataTable dtLeaveCredit;
        private DataTable dtPayrollTypeMpg;

        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        //private List<ADM_COMPANY_MST> admCompanyMstList;
        //private ADM_COMPANY_MST admCompanyMstObj;
        private double Limit;
        private int EmpPayrollType = 0;
        User currentUser;
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

            if (!IsPostBack)
                PageActionHandler();
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

                if (this.CurrPK > 0)
                {
                    GetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);
                    SetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);
                    GetFieldValues(ControlsEnum.LEAVETYPES);
                    SetFieldValues(ControlsEnum.LEAVETYPES);
                }
                else
                {
                    litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    Session["SelectMessage"] = litErrorMsg.Text;
                    Response.Redirect(Resources.PageURL.EmployeeList);
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
            }
        }
        #endregion
        #region Get Field Values
        private void GetFieldValues(ControlsEnum type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    #region Get Employee Details
                    case ControlsEnum.EMPLOYEEDETAILSBYID:
                        objEmployeeBasicInfo = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeebyID(CurrPK);
                        break;
                    #endregion
                    #region LEAVE TYPES
                    case ControlsEnum.LEAVETYPES:
                        dtLeaveTypeList = EmployeeLeaveTypeBL.GetEmpLeaveTypeList(0, CurrPK, (int)DbActiveStatus.ACTIVE, currentUser.SBUID);
                        break;
                    #endregion
                    #region PAYROLL TYPE USER MAPPING
                    case ControlsEnum.PAYROLLTYPEUSERMAPPING:
                        EmpPayrollType = 0;
                        dtPayrollTypeMpg = EmployeePayDetailsBL.GetPayrollTypeUserMapping(currentUser.PKUser, CurrPK, out EmpPayrollType);
                        break;
                    #endregion
                    #region CREDIT DETAILS
                    case ControlsEnum.CREDITDETAILS:
                        int leaveType = 0;
                        int.TryParse(hdfCurLeaveType.Value, out leaveType);
                        dtLeaveCredit = EmployeeLeaveTypeBL.GetEmpLeaveCreditDetails(CurrPK, leaveType, currentUser.SBUID);
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
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region LEAVET YPES
                    case ControlsEnum.LEAVETYPES:
                        BindGrid(controlType);
                        break;
                    #endregion
                    #region EMPLOYEE DETAILS BY ID
                    case ControlsEnum.EMPLOYEEDETAILSBYID:
                        GetUIValuesFromObject(controlType);
                        break;
                    #endregion
                    #region LEAVET CREDIT DETAILS
                    case ControlsEnum.CREDITDETAILS:
                        BindGrid(controlType);
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
        #region Helper Methods
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            int EmpLeavePk;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region EMPLOYEE LEAVE TYPE SAVE
                    case ControlsEnum.EMPLOYEELEAVETYPESAVE:
                        if (CurrPK != 0)
                        {
                            EmployeeLeaveTypeObj.empPK = CurrPK;
                            //EmployeeLeaveTypeObj.BIZUNIT_PK = Convert.ToInt16(currentUser.SBUID);
                            //EmployeeLeaveTypeObj.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            EmployeeLeaveTypeObj.USER_PK = Convert.ToInt16(currentUser.PKUser);
                            EmployeeLeaveTypeObj.LAST_MOD_DT = LastModifiedTime;
                            EmployeeLeaveTypeObj.EmployeeLeaveTypeList = new List<LeaveTypeDetails>();
                            List<LeaveTypeDetails> detailsList = new List<LeaveTypeDetails>();
                            LeaveTypeDetails SkillDetailsObj;
                            foreach (GridViewRow inRowSkill in grdLeaveTypeList.Rows)
                            {
                                CheckBox chkSelect = (CheckBox)inRowSkill.FindControl("chkSelect");
                                if (chkSelect.Checked)
                                {
                                    Limit = 0;
                                    EmpLeavePk = 0;
                                    SkillDetailsObj = new LeaveTypeDetails();
                                    HiddenField hdfActive = (HiddenField)inRowSkill.FindControl("hdfActive");
                                    HiddenField hdfLeaveTypePk = (HiddenField)inRowSkill.FindControl("hdfLeaveTypePk");
                                    HiddenField hdfEmpLeavePk = (HiddenField)inRowSkill.FindControl("hdfEmpLeavePk");
                                    Label lblSkillArea = (Label)inRowSkill.FindControl("lblSkillArea");
                                    TextBox txtSkillYear = (TextBox)inRowSkill.FindControl("txtSkillYear");
                                    DropDownList ddlSkillLevel = (DropDownList)inRowSkill.FindControl("ddlSkillLevel");
                                    TextBox txtLimit = (TextBox)inRowSkill.FindControl("txtLimit");
                                    TextBox txtSkilRemarks = (TextBox)inRowSkill.FindControl("txtSkilRemarks");
                                    double.TryParse(txtLimit.Text, out Limit);
                                    int.TryParse(hdfEmpLeavePk.Value, out EmpLeavePk);
                                    SkillDetailsObj.ELV_ACTIVE = Convert.ToInt16(hdfActive.Value);
                                    SkillDetailsObj.ELV_LEAVE_TYPE = Convert.ToInt32(hdfLeaveTypePk.Value);
                                    SkillDetailsObj.ELV_LIMIT = Limit;
                                    SkillDetailsObj.ELV_PK = EmpLeavePk;
                                    detailsList.Add(SkillDetailsObj);

                                }
                            }

                            EmployeeLeaveTypeObj.EmployeeLeaveTypeList = detailsList;
                            retObject = EmployeeLeaveTypeObj;
                        }
                        break;
                    #endregion;
                }
                return retObject;
            }
            catch
            {
                throw;
            }
            finally { }
        }
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region Get - Employee Details
                    case ControlsEnum.EMPLOYEEDETAILSBYID:
                        if (objEmployeeBasicInfo != null)
                        {
                            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                            CurrPK = objEmployeeBasicInfo.empPK;
                            UCempBasicHdr.objEmployeeBasicInfo = objEmployeeBasicInfo;
                            UCempBasicHdr.SetFieldValues();
                            LastModifiedTime = objEmployeeBasicInfo.LAST_MOD_DT;
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
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region default
                default:
                    break;
                #endregion
            }
        }
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region LEAVET YPES
                    case ControlsEnum.LEAVETYPES:
                        grdLeaveTypeList.DataSource = null;
                        if (dtLeaveTypeList.Rows.Count > 0)
                            grdLeaveTypeList.DataSource = dtLeaveTypeList;
                        grdLeaveTypeList.DataBind();
                        break;
                    #endregion
                    #region LEAVET CREDIT DETAILS
                    case ControlsEnum.CREDITDETAILS:
                        grdLeaveCreditDetails.DataSource = null;
                        if (dtLeaveCredit.Rows.Count > 0)
                            grdLeaveCreditDetails.DataSource = dtLeaveCredit;
                        grdLeaveCreditDetails.DataBind();
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
        #region ActionHandler
        /// <summary>
        /// Button actions events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            try
            {
                int? result;
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
                switch (commonActions)
                {
                    #region Save
                    case BusinessObject.AccountManagement.ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            EmployeeLeaveTypeObj = new EmpLeaveType();
                            EmployeeLeaveTypeObj = (EmpLeaveType)SetUIValuesToObject(ControlsEnum.EMPLOYEELEAVETYPESAVE);
                            if (EmployeeLeaveTypeObj != null && EmployeeLeaveTypeObj.EmployeeLeaveTypeList != null)
                            {
                                string xmlDoc = CommonFunctions.XmlSerialize<EmpLeaveType>(EmployeeLeaveTypeObj);
                                result = Convert.ToInt32(EmployeeLeaveTypeBL.SaveEmployeeLeaveTypes(xmlDoc));
                                if (result > 0) // Success !  redirect to listing page
                                {
                                    //GetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);
                                    //SetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);
                                    //GetFieldValues(ControlsEnum.LEAVETYPES);
                                    //SetFieldValues(ControlsEnum.LEAVETYPES);
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeLeaveType);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EmployeeList) + "');", true);

                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);

                                }
                                else
                                {
                                    if (result == (int)DbSaveStatus.SQLERROR)
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CONCURRENCY)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.EmployeeLeaveType + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeLeaveType);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup(); ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    return;
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Save and continue
                    case BusinessObject.AccountManagement.ActionsEnum.SAVEANDCONTINUE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            EmployeeLeaveTypeObj = new EmpLeaveType();
                            EmployeeLeaveTypeObj = (EmpLeaveType)SetUIValuesToObject(ControlsEnum.EMPLOYEELEAVETYPESAVE);
                            if (EmployeeLeaveTypeObj != null && EmployeeLeaveTypeObj.EmployeeLeaveTypeList != null)
                            {
                                string xmlDoc = CommonFunctions.XmlSerialize<EmpLeaveType>(EmployeeLeaveTypeObj);
                                result = Convert.ToInt32(EmployeeLeaveTypeBL.SaveEmployeeLeaveTypes(xmlDoc));
                                if (result > 0) // Success !  redirect to listing page
                                {
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeLeaveType);
                                    CommonBL userAuth = new CommonBL();
                                    string nextPageUrl = BusinessLogic.HRMS.Common.HRMSCommonBL.GetNextTabUrl(Convert.ToInt16(EmpTabEnum.LeaveType), GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpTabNextURL").ToString().Split(','),
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
                                    //if (userAuth.IsUserHasRights(currentUser.PKUser, Resources.PageURL.HrmsSalary.Replace("~", ""), currentUser.SBUID, currentUser.CurrentDeptPK))
                                    //{                                        
                                    //    GetFieldValues(ControlsEnum.PAYROLLTYPEUSERMAPPING);
                                    //    if (EmpPayrollType == 0)
                                    //    {
                                    //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    //                                             + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EmployeeList) + "');", true); 
                                    //        return;
                                    //    }
                                    //    if (dtPayrollTypeMpg != null && dtPayrollTypeMpg.Rows.Count > 0)
                                    //    {
                                    //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    //                                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.HrmsSalary) + "');", true);
                                    //    }
                                    //    else
                                    //    {
                                    //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    //                                             + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EmployeeList) + "');", true);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    //                                          + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EmployeeList) + "');", true);
                                    //}
                                }
                                else
                                {
                                    if (result == (int)DbSaveStatus.SQLERROR)
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CONCURRENCY)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.EmployeeLeaveType + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeLeaveType);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup(); ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    return;
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Cancel
                    case BusinessObject.AccountManagement.ActionsEnum.CANCEL:
                        CurrPK = 0;
                        Response.Redirect(Resources.PageURL.EmployeeList, true);
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


        #region --- For Grid Actions----

        /// <summary>ActionHandler
        /// Action Handler For GridViewCommandEventArgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
            GridView senderGridView = (GridView)sender;
            if (senderGridView.ID == "grdLeaveTypeList")
            {
                #region LEAVE CREDIT DETAILS
                if (e.CommandName == "DETAILS")
                {
                    GridViewRow grow = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    hdfCurLeaveType = grow.FindControl("hdfLeaveTypePk") as HiddenField;
                    GetFieldValues(ControlsEnum.CREDITDETAILS);
                    SetFieldValues(ControlsEnum.CREDITDETAILS);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divLeaveCreditDetails]','" + GetLocalResourceObject("CreditDeatils").ToString() + "','500','300');", true);
                }
                #endregion

            }

        }

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
        }
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
        }
        #endregion
        #endregion
        #region Pager Methods + Init
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalFooter", "$(document).ready(function(){CalculateTotalFooter();});", true);
                if (EntryStatus == BusinessObject.Common.EntryStatus.VIEWMODE)
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            EMPLOYEEDETAILSBYID,
            SKILLCATEGORY,
            SKILLDETAILS,
            SKILLLEVEL,
            EMPLOYEESKILLDETAILS,
            LEAVETYPES,
            EMPLOYEELEAVETYPESAVE,
            PAYROLLTYPEUSERMAPPING,
            CREDITDETAILS
        }
        #endregion

        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalDigits.Value);
        }
    }
}