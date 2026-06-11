using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using BusinessObject;
using System.Data;
using BusinessObject.CommonManagement;
using BusinessObject.Common;
using DataAccess.CommonManagement;
using BusinessLogic.Administration.Configurations;
using ERP.Utilities.HRMS;
using ERPData;
using ERPService;
using ERPManager;
using ERPSMS_v01.UserControls;

namespace HRMS.Employees
{
    public partial class EmployeeList : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties

        #region Properties

        //Using Session State
        public int CurrPK
        {
            get
            {
                return Convert.ToInt32(Session[ERP.Utilities.SessionStrings.CurrentPK]);
            }
            set
            {
                this.Session[ERP.Utilities.SessionStrings.CurrentPK] = value;
            }
        }

        public int CurrViewModePK
        {
            get
            {
                return Convert.ToInt32(Session[ERP.Utilities.SessionStrings.CurrentViewModePK]);
            }
            set
            {
                this.Session[ERP.Utilities.SessionStrings.CurrentViewModePK] = value;
            }
        }

        private string ShowAdditionalInfo
        {
            get
            {
                return this.ViewState[ViewstateStrings.ShowAdditionalInfo] == null ? string.Empty : this.ViewState[ViewstateStrings.ShowAdditionalInfo].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.ShowAdditionalInfo] = value;
            }
        }

        private string ShowCostCenterTeamInfo
        {
            get
            {
                return this.ViewState[ViewstateStrings.ShowCostCenterTeamInfo] == null ? string.Empty : this.ViewState[ViewstateStrings.ShowCostCenterTeamInfo].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.ShowCostCenterTeamInfo] = value;
            }
        }

        private int PageIndex
        {
            get
            {
                return this.ViewState[ViewstateStrings.PageIndex] == null ? 0 : (int)this.ViewState[ViewstateStrings.PageIndex];
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

        private int TotalPages
        {
            get
            {
                return this.ViewState[ViewstateStrings.TotalPages] == null ? 1 : (int)this.ViewState[ViewstateStrings.TotalPages];
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPages] = value;
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
        /// <summary>
        /// To maintain the SortExpression or then By in viewstate
        /// </summary>
        private string ThenBy
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.ThenBy];
            }
            set
            {
                this.ViewState[ViewstateStrings.ThenBy] = value;
            }
        }
        /// <summary>
        /// To maintain the Sort Direction in viewstate
        /// </summary>
        //private string SortDirection
        //{
        //    get
        //    {
        //        return (string)this.ViewState[ViewstateStrings.SortDirection];
        //    }
        //    set
        //    {
        //        this.ViewState[ViewstateStrings.SortDirection] = value;
        //    }
        //}
        /// <summary>
        /// To maintain the Then Direction in viewstate
        /// </summary>
        private string ThenDirection
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.ThenDirection];
            }
            set
            {
                this.ViewState[ViewstateStrings.ThenDirection] = value;
            }
        }

        //Using View State
        //private int CurrPK
        //{
        //    get
        //    {
        //        return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrPK]);
        //    }
        //    set
        //    {
        //        this.ViewState[ViewstateStrings.CurrPK] = value;
        //    }
        //}
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

        #endregion
        private List<ADM_COMPANY_MST> admCompanyMstList;
        private ADM_COMPANY_MST admCompanyMstObj;
        User currentUser;
        private DataTable dtEmployeeList;
        private DataTable dtDesignation;
        private DataTable dtCompany;
        private DataTable dtStatus;
        private DataTable dtEmployementType;


        private DataTable dtEmployeeFilterList;
        private int designationPK = 0;
        private int employeeTypePK = 0;
        // List variables for binding details to controls
        private BusinessObject.AccountManagement.ActionsEnum commonActions;

        #endregion
        #region Page level Events
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(Request.Form[btnSearch.UniqueID]))
            {
                Session["SearchEmployeeList"] = 1;
            }
            if (!string.IsNullOrEmpty(Request.Form[btnClear.UniqueID]))
            {
                Session["ClearEmployeeList"] = 1;
            }
            if (!IsPostBack)
            {
                if (Session["SelectMessage"] != null)
                {
                    litErrorMsg.Text = Session["SelectMessage"].ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                    Session["SelectMessage"] = null;
                }
                Session[ViewstateStrings.EntryState] = null;
                //PageActionHandler();
                GetFieldValues(ControlEnum.DESIGNATION);
                SetFieldValues(ControlEnum.DESIGNATION);
                GetFieldValues(ControlEnum.EMPLOYEEMENTTYPE);
                SetFieldValues(ControlEnum.EMPLOYEEMENTTYPE);
                ddlDesignation.SelectedIndex = 0;
                GetFieldValues(ControlEnum.COMPANY);
                SetFieldValues(ControlEnum.COMPANY);
                ddlSearchCompany.SelectedIndex = 0;
                GetFieldValues(ControlEnum.STATUSLIST);
                SetFieldValues(ControlEnum.STATUSLIST);
                ShowAdditionalInfo = GetGlobalResourceObject("ConfigurationsRes", "HrmsInfoHiding").ToString();
                ShowCostCenterTeamInfo = GetGlobalResourceObject("ConfigurationsRes", "HrmsCostCenterTeamVisile").ToString();
                if (Convert.ToInt16(ShowAdditionalInfo) == 0)
                {
                    divHideDtls.Visible = false;
                   
                }
                else
                {
                    divHideDtls.Visible = true;
                }
                if (Convert.ToInt16(ShowCostCenterTeamInfo) == 0)
                {
                    divCostTeam.Visible = true;
                }
                else
                {
                    divCostTeam.Visible = false;
                }
                ddlStatus.SelectedIndex = 0;
                PageActionHandler();
            }
        }
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitPage", "$(document).ready(function(){InitPage();});", true);
        }
        #endregion
        #region ActionHandler
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {

                int result;
                result = 0;

                bool bIsChecked = false;


                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
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
                switch (commonActions)
                {
                    #region EDIT
                    //To edit companydetails
                    case ActionsEnum.EDIT:
                        if (!string.IsNullOrEmpty(Request.Form[btnEdit.UniqueID]))
                        {
                            Session["EditClickedInList"] = 1;
                            
                        }
                        foreach (GridViewRow grdrow in grdEmployeelist.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEmpPk")).Value);
                                break;
                            }

                        }
                        if (bIsChecked)
                        {
                            //Using Session State
                            Response.Redirect(Resources.PageURL.EmployeeBasicDetails);
                            //Using Query String
                            //Response.Redirect(Resources.PageURL.EmployeeBasicDetails + "?EmpPK=" + CurrPK, true);
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region VIEW
                    //To edit companydetails
                    case ActionsEnum.VIEW:
                        if (!string.IsNullOrEmpty(Request.Form[btnView.UniqueID]))
                        {
                            Session["ViewClicked"] = 1;
                     

                        }

                        foreach (GridViewRow grdrow in grdEmployeelist.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrViewModePK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEmpPk")).Value);
                                break;
                            }

                        }
                        if (bIsChecked)
                        {
                            Session[ViewstateStrings.EntryState] = EntryStatus.VIEWMODE;
                            //Using Session State
                            Response.Redirect(Resources.PageURL.EmployeeBasicDetails);
                            //Using Query String
                            //Response.Redirect(Resources.PageURL.EmployeeBasicDetails + "?EmpPK=" + CurrPK, true);
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region ITEMSELECTED
                    case ActionsEnum.ITEMSELECTED:
                        foreach (GridViewRow grdrow in grdEmployeelist.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                Session["RadioButtonChecked"] = 1;

                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEmpPk")).Value);
                                break;
                            }
                            else
                            {

                            }
                        }
                        break;
                    #endregion
                    #region NEW
                    //To add new companydetails
                    case ActionsEnum.NEW:
                        CurrPK = 0;
                        if (!string.IsNullOrEmpty(Request.Form[btnNew.UniqueID]))
                        {
                            Session["NewClicked"] = 1;
                        }
                        Response.Redirect(Resources.PageURL.EmployeeBasicDetails, true);
                        break;
                    #endregion
                    #region DELETE
                    //To delete companydetails
                    case ActionsEnum.DELETE:
                        foreach (GridViewRow grdrow in grdEmployeelist.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfempPk")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            string routeURL = Resources.PageURL.EmployeeList.ToString();

                            result = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.DeleteEmployee(CurrPK);

                            DbDeleteStatus deleteStatus = (DbDeleteStatus)result;
                            switch (deleteStatus)
                            {
                                case DbDeleteStatus.DELETED://If deletion is success
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeDetails);

                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + routeURL + "');", true);
                                    break;
                                case DbDeleteStatus.REFERRED://If referred to another page
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Ref;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeDetails);

                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + routeURL + "');", true);
                                    break;
                                case DbDeleteStatus.SQLERROR://Sql error
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeDetails);

                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + routeURL + "');", true);
                                    break;
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region FILTER
                    case ActionsEnum.FILTER:
                        if (!string.IsNullOrEmpty(Request.Form[btnSearch.UniqueID]))
                        {
                            Session["FilterList"] = 1;
                        }
                        PageIndex = 0;
                        GetFieldValues(ControlEnum.EMPFILTERLIST);
                        SetFieldValues(ControlEnum.EMPFILTERLIST);
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm();
                        //GetFieldValues(ControlEnum.EMPLOYEELIST);
                        //SetFieldValues(ControlEnum.FILLGRID);
                        GetFieldValues(ControlEnum.EMPFILTERLIST);
                        SetFieldValues(ControlEnum.EMPFILTERLIST);
                        break;
                    #endregion
                }

            }
            catch (Exception ex)
            {

            }

            finally
            {

            }
        }


        #endregion
        #region Get Field Values
        private void GetFieldValues(ControlEnum type)
        {
            AdmCompanyMstService admCompanyMstServiceClient;
            ServiceUtility serviceUtilityObj;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            switch (type)
            {
                #region Company
                case ControlEnum.COMPANY:
                    //gets Company List

                    admCompanyMstServiceClient = new AdmCompanyMstService();
                    admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                    admCompanyMstObj.CMP_ACTIVE = 1;
                    serviceUtilityObj = new ServiceUtility();
                    admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                    dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetailsPlantWise(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                    break;
                #endregion
                case ControlEnum.DESIGNATION:
                    dtDesignation = DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmployeeDesignation(designationPK);
                    break;
                case ControlEnum.STATUSLIST:
                    dtStatus = DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmployeeStatus(GTIService.Constants.HRMS.Employee.Constatnts.EMP_EMPLOYEE_STATUS_TYPE);
                    break;
                //case ControlEnum.EMPLOYEELIST:
                //    dtEmployeeList = new DataTable();                   
                //    dtEmployeeList = DataAccess.HRMS.Employee.EmployeeBasicInfoDL.GetEmployeeDetailList();
                //    break;
                case ControlEnum.EMPFILTERLIST:
                    int? nationality = null;
                    int? companyID = null;
                    int? designation = null;
                    int? department = null;
                    int? employeeType = null;
                    int? BrancLocation = null;
                    int? costcenter = null;
                    int? team = null;
                    int dummyInt;
                    int? ChkActive = null;

                    if (Int32.TryParse(hdfAutoNationality.Value.ToString(), out dummyInt)
                          && dummyInt != Convert.ToInt32(CommonConstants.SELECTVAL) && dummyInt != 0)
                        nationality = dummyInt;

                    if (Int32.TryParse(hdfDepartment.Value.ToString(), out dummyInt)
                        && dummyInt != Convert.ToInt32(CommonConstants.SELECTVAL) && dummyInt != 0)
                        department = dummyInt;

                    if (Int32.TryParse(hdfBrLoc.Value.ToString(), out dummyInt)
                        && dummyInt != Convert.ToInt32(CommonConstants.SELECTVAL) && dummyInt != 0)
                        BrancLocation = dummyInt;

                    if (Int32.TryParse(ddlDesignation.SelectedValue.ToString(), out dummyInt)
                               && dummyInt != Convert.ToInt32(CommonConstants.SELECTVAL))
                        designation = dummyInt;

                    if (Int32.TryParse(hdfCostCenter.Value.ToString(), out dummyInt)
                         && dummyInt != Convert.ToInt32(CommonConstants.SELECTVAL) && dummyInt != 0)
                        costcenter = dummyInt;

                    if (Int32.TryParse(hdfTeam.Value.ToString(), out dummyInt)
                       && dummyInt != Convert.ToInt32(CommonConstants.SELECTVAL) && dummyInt != 0)
                        team = dummyInt;

                    if (Int32.TryParse(ddlEmployementType.SelectedValue.ToString(), out dummyInt) && dummyInt != Convert.ToInt32(CommonConstants.SELECTVAL))
                        employeeType = dummyInt;


                    if (chkIncludeInActive.Checked == true)
                    {
                        ChkActive = 0;
                    }
                    else
                    {
                        ChkActive = 1;

                    }

                    if (Int32.TryParse(ddlSearchCompany.SelectedValue.ToString(), out dummyInt)
                         && dummyInt != Convert.ToInt32(CommonConstants.SELECTVAL))

                        companyID = dummyInt;

                    this.dtEmployeeFilterList = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmpListByFilterOptions(new EmpDocumentFilterParameterBinder
                        {
                            EmployeeCode = HttpUtility.HtmlEncode(txtSearchCode.Text.Trim()),
                            EmployeeName = HttpUtility.HtmlEncode(txtName.Text.Trim()),
                            PassportNO = HttpUtility.HtmlEncode(txtSearchPassport.Text.Trim()),
                            PermitNO = HttpUtility.HtmlEncode(txtSearchPermitNo.Text.Trim()),
                            Nationality = nationality,
                            Designation = designation,
                            Department = department,
                            Company = companyID,
                            EmployeeType = employeeType,
                            Active = ChkActive,
                            CurrentStatus = Convert.ToInt32(ddlStatus.SelectedValue),
                            EmpCategory = (int)EmployeeCategory.HRMSEmployee,
                            PageIndex = (PageIndex == 0 ? 1 : PageIndex),
                            PageSize = PageSize,
                            BranchLoc=BrancLocation,
                            CostCenter=costcenter,
                            Team=team
                        });
                    break;

                #region EMPLOYEEMENT TYPE
                case ControlEnum.EMPLOYEEMENTTYPE:
                    dtEmployementType = BusinessLogic.HRMS.Employee.EmployeePayDetailsBL.GetEmployeeTypeGetKV(employeeTypePK, currentUser.SBUID, (int)DbActiveStatus.ACTIVE);
                    break;
                #endregion
                default:
                    break;
            }

        }
        #endregion
        #region Set Field Values

        private void SetFieldValues(ControlEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlEnum.FILLGRID:
                        BindGrid(ControlEnum.FILLGRID);
                        break;
                    case ControlEnum.EMPFILTERLIST:
                        BindGrid(ControlEnum.EMPFILTERLIST);
                        break;
                    case ControlEnum.DESIGNATION:
                        BindDropDown(ControlEnum.DESIGNATION);
                        break;
                    case ControlEnum.COMPANY:
                        BindDropDown(ControlEnum.COMPANY);
                        break;
                    case ControlEnum.STATUSLIST:
                        BindDropDown(ControlEnum.STATUSLIST);
                        break;
                    #region EMPLOYEEMENT TYPE
                    case ControlEnum.EMPLOYEEMENTTYPE:
                        BindDropDown(ControlEnum.EMPLOYEEMENTTYPE);
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
        #region PageActionHandler

        private void PageActionHandler()
        {
            try
            {
                if (!IsPostBack)
                {
                    Session[ERP.Utilities.SessionStrings.CurrentPK] = 0;
                    PageIndex = Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE);
                    uclPaging.CurrentPage = PageIndex;
                    //GetFieldValues(ControlEnum.EMPLOYEELIST);
                    //SetFieldValues(ControlEnum.FILLGRID);

                    GetFieldValues(ControlEnum.EMPFILTERLIST);
                    SetFieldValues(ControlEnum.EMPFILTERLIST);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        #endregion
        #region Helper Methods
        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlEnum controlType)
        {

            switch (controlType)
            {
                case ControlEnum.FILLGRID:
                    if (dtEmployeeList != null && dtEmployeeList.Rows.Count > 0)
                        grdEmployeelist.DataSource = dtEmployeeList;

                    else
                        grdEmployeelist.DataSource = null;
                    PageIndex = PageIndex == null ? 0 : PageIndex;
                    grdEmployeelist.PageIndex = Convert.ToInt32(PageIndex);
                    grdEmployeelist.DataBind();
                    break;
                case ControlEnum.EMPFILTERLIST:
                    //grdEmployeelist.DataSource = this.dtEmployeeFilterList;
                    //PageIndex = PageIndex == null ? 0 : PageIndex;
                    //grdEmployeelist.PageIndex = Convert.ToInt32(PageIndex);
                    //grdEmployeelist.DataBind();
                     uclPaging.Visible = false;
                     if (dtEmployeeFilterList != null && dtEmployeeFilterList.Rows.Count > 0)
                        {
                            int rowCount = 0;

                            rowCount = Convert.ToInt32(dtEmployeeFilterList.Rows[0]["TOT_ROW_COUNT"].ToString());
                            //this.TotalPages = Convert.ToInt32(dtEmployeeFilterList.Rows[0]["ROW_NO"].ToString());

                            uclPaging.TotalPages = TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                              (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                              (rowCount / this.PageSize) + 1;
                            PageIndex = PageIndex == 0 ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdEmployeelist.DataSource = dtEmployeeFilterList;
                            grdEmployeelist.DataBind();

                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            grdEmployeelist.DataSource = null;
                            grdEmployeelist.DataBind();
                        }
                    break;
                default:
                    break;
            }

        }
        private void BindDropDown(ControlEnum controlType)
        {
            switch (controlType)
            {
                // Fill Shift Details to DropDown


                case ControlEnum.DESIGNATION:
                    if (dtDesignation != null)
                    {

                        ddlDesignation.DataSource = CommonFunctions.HtmlDecodeDataTable(dtDesignation, ("dsgName").ToString());
                        ddlDesignation.DataTextField = "dsgName";
                      
                        ddlDesignation.DataValueField = "dsgPK";
                        ddlDesignation.DataBind();
                        ddlDesignation.Items.Insert(0, new ListItem("Select", "-1"));

                    }

                    break;
                case ControlEnum.STATUSLIST:
                    ddlStatus.Items.Clear();
                    if (dtStatus != null)
                    {
                        ddlStatus.DataSource = dtStatus;
                        ddlStatus.DataTextField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_TEXT;
                        ddlStatus.DataValueField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_VALUE;
                        ddlStatus.DataBind();
                    }
                    ddlStatus.Items.Insert(0, new ListItem("Select", "-1"));
                    break;
                #region Company
                case ControlEnum.COMPANY:

                    ddlSearchCompany.Items.Clear();
                    if (dtCompany != null && dtCompany.Rows.Count > 0)
                    {

                        ddlSearchCompany.DataSource = CommonFunctions.HtmlDecodeDataTable(dtCompany, Resources.DataFieldRes.CompanySpecs);
                        ddlSearchCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlSearchCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlSearchCompany.DataBind();
                        ddlSearchCompany.Items.Insert(0, new ListItem("Select", "-1"));
                        //ddlSearchCompany.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    }
                    if (dtCompany != null && dtCompany.Rows.Count > 0)
                    {
                        ddlSearchCompany.SelectedIndex = ddlSearchCompany.Items.IndexOf(ddlSearchCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()));
                    }
                    //if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    //{

                    //    ddlSearchCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                    //    ddlSearchCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                    //    ddlSearchCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                    //    ddlSearchCompany.DataBind();
                    //    ddlSearchCompany.Items.Insert(0, new ListItem("Select", "-1"));
                    //    //ddlSearchCompany.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    //}
                    //if (dtCompany != null && dtCompany.Rows.Count > 0)
                    //{
                    //    ddlSearchCompany.SelectedIndex = ddlSearchCompany.Items.IndexOf(ddlSearchCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()));
                    //}

                    break;
                #endregion
                #region EMPLOYEEMENT TYPE
                case ControlEnum.EMPLOYEEMENTTYPE:
                    ddlEmployementType.DataSource = dtEmployementType;
                    ddlEmployementType.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.EMT_NAME;
                    ddlEmployementType.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.EMT_PK;
                    ddlEmployementType.DataBind();
                    ddlEmployementType.Items.HtmlDecode();
                    ddlEmployementType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                default:
                    break;
            }
        }
        private void ResetForm()
        {
            txtName.Text = string.Empty;
            txtSearchCode.Text = string.Empty;
            txtSearchPassport.Text = string.Empty;
            txtSearchPermitNo.Text = string.Empty;
            ddlSearchCompany.Items.Clear();
            ddlSearchCompany.ClearSelection();
            GetFieldValues(ControlEnum.COMPANY);
            SetFieldValues(ControlEnum.COMPANY);
            ddlSearchCompany.SelectedIndex = 0;
            txtDepartment.Text = "";
            txtTeam.Text = "";
            txtCostCenter.Text = "";
            hdfDepartment.Value = string.Empty;
            hdfCostCenter.Value = string.Empty;
            hdfTeam.Value = string.Empty;
            hdfBrLoc.Value = string.Empty;
            txtSearchNationality.Text = "";
            hdfAutoNationality.Value=string.Empty;
            ddlDesignation.Items.Clear();
            ddlDesignation.ClearSelection();
            GetFieldValues(ControlEnum.DESIGNATION);
            SetFieldValues(ControlEnum.DESIGNATION);
            GetFieldValues(ControlEnum.EMPLOYEEMENTTYPE);
            SetFieldValues(ControlEnum.EMPLOYEEMENTTYPE);
            ddlDesignation.SelectedIndex = 0;
            chkIncludeInActive.Checked = false;
            ddlStatus.SelectedIndex = 0;
            PageIndex = Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE);
            uclPaging.CurrentPage  = Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE);            
           
        }
        public SortDirection dir
        {
            get
            {
                if (ViewState["dirState"] == null)
                {
                    ViewState["dirState"] = SortDirection.Ascending;
                }
                return (SortDirection)ViewState["dirState"];
            }
            set
            {
                ViewState["dirState"] = value;
            }
        }
        #endregion
        #region PagerControl
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {

            //Session["Paging"] =1; 
            //if (Session["FilterList"] != null)
            //{

                PageIndex = e.NewPageIndex;
                GetFieldValues(ControlEnum.EMPFILTERLIST);
                SetFieldValues(ControlEnum.EMPFILTERLIST);
            //}
            //else
            //{

            //    PageIndex = e.NewPageIndex;
            //    GetFieldValues(ControlEnum.EMPLOYEELIST);
            //    SetFieldValues(ControlEnum.FILLGRID);
            //}


        }

        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            try
            {
                Session["Sorting"] = 1; 

                //if (Session["FilterList"] != null)
                //{
                    GetFieldValues(ControlEnum.EMPFILTERLIST);
                    SetFieldValues(ControlEnum.EMPFILTERLIST);
                    DataTable dt = new DataTable();
                    grdEmployeelist.DataSource = dtEmployeeFilterList;
                    {
                        string SortDir = string.Empty;
                        if (dir == SortDirection.Ascending)
                        {
                            dir = SortDirection.Descending;
                            SortDir = "Desc";
                        }
                        else
                        {
                            dir = SortDirection.Ascending;
                            SortDir = "Asc";
                        }
                        DataView sortedView = new DataView(dtEmployeeFilterList);
                        sortedView.Sort = e.SortExpression + " " + SortDir;
                        grdEmployeelist.DataSource = sortedView;
                        grdEmployeelist.DataBind();
                    }

                //}
                //else
                //{
                //    GetFieldValues(ControlEnum.EMPLOYEELIST);
                //    SetFieldValues(ControlEnum.FILLGRID);
                //    DataTable dt = new DataTable();
                //    grdEmployeelist.DataSource = dtEmployeeList;
                //    {
                //        string SortDir = string.Empty;
                //        if (dir == SortDirection.Ascending)
                //        {
                //            dir = SortDirection.Descending;
                //            SortDir = "Desc";
                //        }
                //        else
                //        {
                //            dir = SortDirection.Ascending;
                //            SortDir = "Asc";
                //        }
                //        DataView sortedView = new DataView(dtEmployeeList);
                //        sortedView.Sort = e.SortExpression + " " + SortDir;
                //        grdEmployeelist.DataSource = sortedView;
                //        grdEmployeelist.DataBind();
                //    }
                //}

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
                GetFieldValues(ControlEnum.EMPFILTERLIST);
                SetFieldValues(ControlEnum.EMPFILTERLIST);
                EnableDisableButtons(e.TotalPages, "uclPaging");                
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

     
        #endregion
        #region ControlEnum

        public enum ControlEnum
        {
            EMPLOYEELIST,
            FILLGRID,
            EMPFILTERLIST,
            DESIGNATION,
            COMPANY,
            STATUSLIST,
            EMPLOYEEMENTTYPE
        }

        #endregion

    }

}