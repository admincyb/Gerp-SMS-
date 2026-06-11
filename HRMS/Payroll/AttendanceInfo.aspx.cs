using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using ERP.Utilities;
using System.Data;
using BusinessObject.HRMS.Payroll;
using BusinessObject.CommonManagement;
using BusinessObject.AccountManagement;
using ERPSMS_v01;

namespace HRMS.Payroll
{
    public partial class AttendanceInfo : ERP.Store.UI.MyBasePage
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
        //private List<OvertimeCalculatorBO.MonthlyOTData> MonthlyOTDataList
        //{
        //    get
        //    {
        //        return ViewState["MonthlyOTDataList"] == null ? new List<OvertimeCalculatorBO.MonthlyOTData>() : (List<OvertimeCalculatorBO.MonthlyOTData>)ViewState["MonthlyOTDataList"];
        //    }
        //    set
        //    {
        //        ViewState["MonthlyOTDataList"] = value;
        //    }
        //}

        #endregion

        private BusinessObject.User currentUser;
        private DataTable dtResult;
        private DataSet dsPageData;
        //int currPK;
        #endregion

        #region PageEvents
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>",
                "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
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
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (!IsPostBack)
                {
                    //ResetForm(ControlsEnum.CLEAR);
                    GetFieldValues(ControlsEnum.EMPLOYEETYPE);
                    SetFieldValues(ControlsEnum.EMPLOYEETYPE);
                    GetFieldValues(ControlsEnum.EMPLOYEES);
                    SetFieldValues(ControlsEnum.EMPLOYEES);
                    GetFieldValues(ControlsEnum.BRANCHLOCATION);
                    SetFieldValues(ControlsEnum.BRANCHLOCATION);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        /// <summary>
        /// Button actions events
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
                #region Getting Command Action
                ActionsEnum commonActions = ActionsEnum.UNKNOWN;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    //if ((((DropDownList)sender).ID == "ddlEmployee"))
                    //{
                    //    commonActions = ActionsEnum.CHANGE;
                    //}
                    //if ((((DropDownList)sender).ID == "ddlLeaveType"))
                    //{
                    //    commonActions = ActionsEnum.CHANGETYPE;
                    //}
                }
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    //if ((((TextBox)sender).ID == "txtDate"))
                    //{
                    //    commonActions = ActionsEnum.CHANGE;
                    //    ddlEmployee.Enabled = true;
                    //}
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                #endregion

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            try
            {
                switch (type)
                {
                    #region EMPLOYEES
                    case ControlsEnum.EMPLOYEES:
                        dtResult = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeeDetailListHeader(0, GTIService.Constants.Configurations.Employees.Fields.NAME, 1);
                        break;
                    #endregion
                    #region BRANCHLOCATION
                    case ControlsEnum.BRANCHLOCATION:
                        dtResult = BusinessLogic.HRMS.Payroll.AttendanceInfoBL.GetBranchOrLocation(21, 7, this.currentUser.SBUID, 0, (int)DbActiveStatus.ACTIVE);
                        break;
                    #endregion
                    #region EMPLOYEETYPE
                    case ControlsEnum.EMPLOYEETYPE:
                        dtResult = BusinessLogic.HRMS.Admin.Masters.EmployeeTypeBL.GetEmployeeTypeList(currentUser.SBUID, 0, 0);
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
                    #region EMPLOYEES
                    case ControlsEnum.EMPLOYEES:
                        BindDropDown(ControlsEnum.EMPLOYEES);
                        break;
                    #endregion
                    #region EMPLOYEETYPE
                    case ControlsEnum.EMPLOYEETYPE:
                        BindDropDown(ControlsEnum.EMPLOYEETYPE);
                        break;
                    #endregion
                    #region BRANCHLOCATION
                    case ControlsEnum.BRANCHLOCATION:
                        BindDropDown(ControlsEnum.BRANCHLOCATION);
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

        #region BindDropDown
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region EMPLOYEES
                case ControlsEnum.EMPLOYEES:
                    ddlEmployee.Items.Clear();
                    ddlEmployee.DataSource = dtResult;
                    ddlEmployee.DataTextField = GTIService.Constants.Configurations.Employees.Fields.NAME;
                    ddlEmployee.DataValueField = GTIService.Constants.Configurations.Employees.Fields.PK;
                    ddlEmployee.DataBind();
                    ddlEmployee.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    break;
                #endregion
                #region SALARYPKLIST
                case ControlsEnum.EMPLOYEETYPE:
                    ddlEmployeeType.Items.Clear();
                    ddlEmployeeType.DataSource = dtResult;
                    ddlEmployeeType.DataTextField = GTIService.Constants.HRMS.Payroll.Fields.EMT_NAME;
                    ddlEmployeeType.DataValueField = GTIService.Constants.HRMS.Payroll.Fields.EMT_PK;
                    ddlEmployeeType.DataBind();
                    ddlEmployeeType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    break;
                #endregion
                #region BRANCHLOCATION
                case ControlsEnum.BRANCHLOCATION:
                    ddlBranchLocation.Items.Clear();
                    ddlBranchLocation.DataSource = dtResult;
                    ddlBranchLocation.DataTextField = GTIService.Constants.HRMS.Employee.Fields.ADM_CNST_TEXT;
                    ddlBranchLocation.DataValueField = GTIService.Constants.HRMS.Employee.Fields.ADM_CNST_VALUE;
                    ddlBranchLocation.DataBind();
                    ddlBranchLocation.Items.HtmlDecode();
                    ddlBranchLocation.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    break;
                #endregion

            }
        }
        #endregion

        #region BindGrid
        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ResetForm
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {

        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            EMPLOYEES,
            BRANCHLOCATION,
            EMPLOYEETYPE

            //LEAVETYPES,
            //COMPANY,
            //MONTHLYLEAVELIST,
            //ADDTOLIST,
            //AFTERDELETE,
            //MONTHLYLEAVEMASTER,
            //CLEAR,
            //SALARYPKLIST,
            //LIST,
            //CLEARSEARCH
        }
        #endregion
    }
}