using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ERP.Utilities;
using BusinessLogic.HRMS.Employee;
using ERP.Utilities.HRMS;
using BusinessObject.CommonManagement;
using BusinessObject.HRMS.Employee;
using HRMS.Employees.UserControls;
using ERPSMS_v01;

namespace HRMS.Employees
{
    public partial class ManageEmpDoc : System.Web.UI.Page
    {
        private int SelectedDocPk;
        private DataTable pageData;
        private BusinessObject.User currentUser;
        private DataTable EmployeeDocumentDetailsList;
        private DataTable SelectedEmployeeDocumentDetailsList;
        ActionsEnum commonActions;

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
        /// Entry State for managing Search Mode
        /// </summary>
        private EmployeeDocumentAction CurrentDocumentAction
        {
            get
            {
                return this.ViewState[ViewstateStrings.DocumentAction] == null ? EmployeeDocumentAction.CheckIn
                    : (EmployeeDocumentAction)(this.ViewState[ViewstateStrings.DocumentAction]);
            }
            set
            {
                this.ViewState[ViewstateStrings.DocumentAction] = value;
            }
        }

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
            if (!IsPostBack)
            {                
                SetFieldValues(ControlsEnum.DEFAULT);
                ddlFilterFor.Focus();
            }
        }
        #endregion

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");

            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitPage", "$(document).ready(function(){InitPage();});", true);

            if (hdfCheckingOrCheckOut.Value == ((int)EmployeeDocumentAction.CheckIn).ToString())
            {
                //CheckIn =1
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideActionButton", "$(document).ready(function(){showHideCheckInCheckOut('1');});", true);
            }
            else if (hdfCheckingOrCheckOut.Value == ((int)EmployeeDocumentAction.CheckOut).ToString())
            {
                //CheckOut =0
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideActionButton", "$(document).ready(function(){showHideCheckInCheckOut('0');});", true);
            }

            if (this.hdfCurrentSearchMode.Value == SearchMode.BASIC.ToString())
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
            }
            else if (this.hdfCurrentSearchMode.Value == SearchMode.ADVANCED.ToString())
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch(1);});", true);
            }
        }

        protected void ActionHandler(object sender, EventArgs e)
        {
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;
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
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                switch (commonActions)
                {
                    case ActionsEnum.DEFAULT:
                        GetFieldValues(ControlsEnum.DOCTYPELIST);
                        break;
                    case ActionsEnum.FILTER:
                        GetFieldValues(ControlsEnum.EMPDOCLIST);
                        SetFieldValues(ControlsEnum.EMPDOCLIST);
                        break;
                    #region Show Log
                    case ActionsEnum.SHOWLOG:
                        this.SelectedDocPk = Convert.ToInt32(((HiddenField)((ImageButton)sender).Parent.Parent.FindControl("hdfDocPk")).Value);
                        GetFieldValues(ControlsEnum.LOGLIST);
                        SetFieldValues(ControlsEnum.LOGLIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCheckInCheckOutLog]','" + Resources.PageNameRes.Log + "','800','500');", true);
                        break;
                    #endregion
                    case ActionsEnum.CHECKIN:
                        GetFieldValues(ControlsEnum.CHECKIN);
                        SetFieldValues(ControlsEnum.CHECKIN);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCheckInControlContainer]','" + Resources.PageNameRes.CheckIn + "','900','520');", true);
                        break;
                    case ActionsEnum.CHECKOUT:
                        GetFieldValues(ControlsEnum.CHECKOUT);
                        SetFieldValues(ControlsEnum.CHECKOUT);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCheckOutControlContainer]','" + Resources.PageNameRes.CheckOut + "','900','520');", true);
                        break;
                    case ActionsEnum.CHECKINLOGCLOSE:
                        GetFieldValues(ControlsEnum.CHECKIN);
                        SetFieldValues(ControlsEnum.CHECKIN);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCheckInControlContainer]','" + Resources.PageNameRes.CheckIn + "','900','520');", true);
                        break;
                    case ActionsEnum.CHECKOUTLOGCLOSE:
                        GetFieldValues(ControlsEnum.CHECKOUT);
                        SetFieldValues(ControlsEnum.CHECKOUT);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCheckOutControlContainer]','" + Resources.PageNameRes.CheckOut + "','900','520');", true);
                        break;
                    case ActionsEnum.CHECKOUTERROR:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCheckOutControlContainer]','" + Resources.PageNameRes.CheckOut + "','900','520');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + ((Button)sender).CommandArgument + "','" + Resources.ErpRes.Information + "');", true);
                        break;
                    case ActionsEnum.CHECKINERROR:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCheckOutControlContainer]','" + Resources.PageNameRes.CheckOut + "','900','520');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + ((Button)sender).CommandArgument + "','" + Resources.ErpRes.Information + "');", true);
                        break;
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

        #region GridView Action Handler
        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (((GridView)sender).ID == "grdEmpDocList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        HiddenField hdf = (HiddenField)e.Row.FindControl("hdfIsCheckedIn");
                        ImageButton lnkChkdOut = (ImageButton)e.Row.FindControl("lnkCheckOut");
                        ImageButton lnkChkdIn = (ImageButton)e.Row.FindControl("lnkCheckIn");
                        if (lnkChkdOut != null && lnkChkdIn != null && hdf != null)
                        {
                            sbyte isCheckedIn = Convert.ToSByte(hdf.Value.Trim());
                            lnkChkdIn.Visible = isCheckedIn == 1 ? true : false;
                            lnkChkdOut.Visible = isCheckedIn == 1 ? false : true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Process Exception and show error message
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
            finally
            {
                //reset all objects
            }
        }

        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex;
            GetFieldValues(ControlsEnum.EMPDOCLIST);
            SetFieldValues(ControlsEnum.EMPDOCLIST);
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
                    //Toggle the sort expression
                    if (SortDirection == Resources.ErpRes.SortAscending)
                        SortDirection = Resources.ErpRes.SortDescending;
                    else
                        SortDirection = Resources.ErpRes.SortAscending;
                }
                else
                {
                    SortBy = e.SortExpression;
                    SortDirection = Resources.ErpRes.SortAscending;
                }

                //this.PageIndex = CommonConstants.SELECT_VALUE_ONE;
                GetFieldValues(ControlsEnum.EMPDOCLIST);
                SetFieldValues(ControlsEnum.EMPDOCLIST);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        protected void ResetForm(ActionsEnum action)
        {
            switch (action)
            {
                case ActionsEnum.SAVE:
                    ResetForm(ActionsEnum.EMPDOCLIST);
                    break;
                case ActionsEnum.EMPDOCLIST:
                    GetFieldValues(ControlsEnum.EMPDOCLIST);
                    SetFieldValues(ControlsEnum.EMPDOCLIST);
                    break;
                case ActionsEnum.CHECKOUT:
                    GetFieldValues(ControlsEnum.EMPDOCLIST);
                    SetFieldValues(ControlsEnum.EMPDOCLIST);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('Document(s) Checked Out','" + Resources.ErpRes.Information + "');", true);
                    break;
                case ActionsEnum.CHECKIN:
                    GetFieldValues(ControlsEnum.EMPDOCLIST);
                    SetFieldValues(ControlsEnum.EMPDOCLIST);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('Document(s) Checked In','" + Resources.ErpRes.Information + "');", true);
                    break;
            }
        }

        /// <summary>
        /// Getting Data From Db To Fields
        /// </summary>
        /// <param name="controlsEnum">Field</param>
        private void GetFieldValues(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                case ControlsEnum.COMPANY:
                    pageData = BusinessLogic.CommonManagement.CommonManagement.GetCompanyList(0, (int)DbActiveStatus.ACTIVE, currentUser.SBUID);
                    break;
                case ControlsEnum.FILTERFOR:
                    pageData = EmployeeDocBL.GetFilterForList(new ConfigurationParameterBinder
                    {
                        Active=(short)DbActiveStatus.ACTIVE,
                        ConfigPk=0,
                        ConfigType="EMP DOC STATUS"
                    });
                    break;
                case ControlsEnum.EMPDOCLIST:
                    short isCheckedFor = (short)EmployeeDocumentAction.CheckIn;
                    int? nationality=null;
                    int? expiringIn = null;
                    int? employeeID = null;
                    int? documentType = null;
                    int? companyID = null;

                    int dummyInt;

                    if (Int32.TryParse(hdfAutoNationality.Value.ToString(), out dummyInt)
                            && dummyInt != Convert.ToInt32(CommonConstants.SELECTVAL) && dummyInt != 0)
                        nationality = dummyInt;

                    if (Int32.TryParse(txtExpiringIn.Text.Trim(), out dummyInt)
                            && dummyInt != Convert.ToInt32(CommonConstants.SELECTVAL))
                        expiringIn = dummyInt;

                    if (Int32.TryParse(ddlDocumentTypeList.SelectedValue.ToString(), out dummyInt) 
                                && dummyInt != Convert.ToInt32(CommonConstants.SELECTVAL))
                        documentType = dummyInt;

                    if (Int32.TryParse(ddlCompany.SelectedValue.ToString(), out dummyInt)
                            && dummyInt != Convert.ToInt32(CommonConstants.SELECTVAL))
                        companyID = dummyInt;

                    short.TryParse(ddlFilterFor.SelectedValue.ToString(), out isCheckedFor);

                    this.EmployeeDocumentDetailsList = EmployeeDocBL.GetEmpDocByFilterOptions(new EmpDocumentFilterParameterBinder
                    {
                        IsCheckedFor=isCheckedFor,//(short)EmployeeDocumentAction.CheckIn,
                        Nationality=nationality,
                        ExpiringIn=expiringIn,
                        EmployeeName=txtEmployeeName.Text.TrimStart(),
                        EmployeeCode=txtEmpCode.Text.Trim(),
                        DocNo=txtDocNo.Text.Trim(),
                        Employee=employeeID,
                        DocumentType=documentType,
                        ExpiresBefore=string.IsNullOrEmpty(txtExpBefore.Text)?string.Empty:Convert.ToDateTime(txtExpBefore.Text).ToString(),
                        Company=companyID,
                        SortBy=this.SortBy,
                        SortDirection=this.SortDirection,
                        PageIndex=this.PageIndex
                    });
                    break;
                case ControlsEnum.DOCTYPELIST:
                    pageData = EmployeeDocBL.GetEmployeeDocTypeList(GTIService.Constants.HRMS.Employee.Constatnts.EMP_DOC_TYPE);
                    BindDropDown(controlsEnum);
                    break;
                case ControlsEnum.LOGLIST:
                    pageData = EmployeeDocBL.GetEmployeeDocsLog(this.SelectedDocPk);
                    break;
                case ControlsEnum.CHECKIN:
                    GetSelectedEmpDocList();
                    //this.EmployeeDocumentDetailsList = GetSelectedEmpDocList
                    break;
                case ControlsEnum.CHECKOUT:
                    GetSelectedEmpDocList();
                    // this.EmployeeDocumentDetailsList = GetSelectedEmpDocList
                    break;
                default:
                    break;
            }
        }

        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.DEFAULT:
                        GetFieldValues(ControlsEnum.DOCTYPELIST);
                        SetFieldValues(ControlsEnum.DOCTYPELIST);

                        GetFieldValues(ControlsEnum.COMPANY);
                        SetFieldValues(ControlsEnum.COMPANY);

                        GetFieldValues(ControlsEnum.FILTERFOR);
                        SetFieldValues(ControlsEnum.FILTERFOR);

                        this.CurrentDocumentAction = EmployeeDocumentAction.CheckIn;
                        GetFieldValues(ControlsEnum.EMPDOCLIST);
                        BindGrid(ControlsEnum.EMPDOCLIST);
                        break;
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    case ControlsEnum.EMPDOCLIST:
                        BindGrid(controlType);
                        ShowHideCheckInCheckOutButton();
                        break;
                    case ControlsEnum.FILTERFOR:
                        BindDropDown(ControlsEnum.FILTERFOR);
                        break;
                    case ControlsEnum.DOCTYPELIST:
                        BindDropDown(ControlsEnum.DOCTYPELIST);
                        break;
                    case ControlsEnum.LOGLIST:
                        BindGrid(ControlsEnum.LOGLIST);
                        break;
                    case ControlsEnum.CHECKIN:
                        BindUserControl(controlType);
                        break;
                    case ControlsEnum.CHECKOUT:
                        BindUserControl(controlType);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ShowHideCheckInCheckOutButton()
        {
            short filterFor = Convert.ToInt16(ddlFilterFor.SelectedValue);

            if (filterFor == (short)EmployeeDocumentAction.CheckIn)
            {
                hdfCheckingOrCheckOut.Value =((int)EmployeeDocumentAction.CheckIn).ToString();
            }
            else if (filterFor == (short)EmployeeDocumentAction.CheckOut)
            {
                hdfCheckingOrCheckOut.Value = ((int)EmployeeDocumentAction.CheckOut).ToString();
            }
        }

        private void GetSelectedEmpDocList()
        {
            bool isChecked = false;
            GetFieldValues(ControlsEnum.EMPDOCLIST);
            List<int> selectedDocPKList = new List<int>();
            foreach (GridViewRow grdrow in grdEmpDocList.Rows)
            {
                CheckBox chk;
                chk = (CheckBox)grdrow.FindControl("chkSelect");
                if (chk.Checked)
                {
                    isChecked = true;
                    selectedDocPKList.Add(Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDocPk")).Value));   
                }
            }

            if (!isChecked)
            {
                throw new ApplicationException("Items Not Selected");
            }

            this.SelectedEmployeeDocumentDetailsList= this.EmployeeDocumentDetailsList.AsEnumerable()
                                                        .Where(x => selectedDocPKList.Contains(x.Field<int>(Resources.DataFieldRes.EmpDocPk)))
                                                        .ToArray()
                                                        .CopyToDataTable();
        }

        private void BindGrid(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                case ControlsEnum.EMPDOCLIST:
                    grdEmpDocList.DataSource = this.EmployeeDocumentDetailsList;
                    PageIndex = PageIndex == null ? 0 : PageIndex;
                    grdEmpDocList.PageIndex = Convert.ToInt32(PageIndex);
                    grdEmpDocList.DataBind();
                    break;
                case ControlsEnum.LOGLIST:
                    grdCheckIncheckOutLog.DataSource = pageData; // Change Grid
                    grdCheckIncheckOutLog.DataBind();
                    break;
                default:
                    break;
            }
        }

        private void BindDropDown(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                case ControlsEnum.FILTERFOR:
                    //EnumHelpers.GetAll<EmployeeDocumentAction>();
                    ddlFilterFor.DataSource = pageData; 
                    ddlFilterFor.DataTextField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_TEXT;
                    ddlFilterFor.DataValueField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_VALUE;
                    ddlFilterFor.DataBind();
                    break;
                case ControlsEnum.DOCTYPELIST:
                    ddlDocumentTypeList.Items.Clear();

                    ddlDocumentTypeList.DataSource = pageData;
                    ddlDocumentTypeList.DataTextField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_TEXT;
                    ddlDocumentTypeList.DataValueField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_VALUE;
                    ddlDocumentTypeList.DataBind();

                    ddlDocumentTypeList.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.COMPANY:
                    ddlCompany.Items.Clear();

                    ddlCompany.DataSource = pageData;
                    ddlCompany.DataTextField = GTIService.Constants.HRMS.Employee.Fields.CMP_NAME;
                    ddlCompany.DataValueField = GTIService.Constants.HRMS.Employee.Fields.CMP_PK;
                    ddlCompany.DataBind();
                    ddlCompany.Items.HtmlDecode();
                    ddlCompany.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
            }
        }

        private void BindUserControl(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.CHECKOUT:
                    CheckOutControl1.SelectedDocItems = this.SelectedEmployeeDocumentDetailsList;
                    CheckOutControl1.SetProperties();
                    break;
                case ControlsEnum.CHECKIN:
                    CheckInControl1.SelectedDocItems = this.SelectedEmployeeDocumentDetailsList;
                    CheckInControl1.SetProperties();
                    break;
            }
        }


    }

}