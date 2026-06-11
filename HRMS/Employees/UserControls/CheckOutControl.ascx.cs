using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.HRMS.Employee;
using ERP.Utilities;
using BusinessObject.CommonManagement;
using BusinessLogic.HRMS.Employee;
using System.Data;

namespace HRMS.Employees.UserControls
{
    public partial class CheckOutControl : System.Web.UI.UserControl
    {
        private EmployeeDocumentCheckInCheckOutParameterBinder CheckInParaMeter;
        private BusinessObject.User currentUser;

        private int SelectedDocPk;
        private DataTable pageData;
        public DataTable SelectedDocItems { get; set; }

        public delegate void AfterSaveHandler(ActionsEnum action);
        public event AfterSaveHandler AfterSave;
        public event EventHandler Error;
        public event EventHandler LogClosed;

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

        private List<EmployeeDocumentCheckInCheckOutDetailsParameterBinder> SelectedPkArray
        {
            get
            {
                return (List<EmployeeDocumentCheckInCheckOutDetailsParameterBinder>)(this.ViewState[ViewstateStrings.SelectedPkArray]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedPkArray] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.currentUser = (BusinessObject.User)Context.User.Identity;
            if (!IsPostBack)
            {
                txtIssuedOutOn.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                txtIssuedOutOnTime.Text = DateTime.Now.ToString(CommonConstants.TIMEFORMAT);
                txtExpectedReturnDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                SetFieldValues(ControlsEnum.DEFAULT);
            }
        }

        protected void ActionHandler(object sender, EventArgs e)
        {

            ActionsEnum commonActions = ActionsEnum.DEFAULT;
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

                switch (commonActions)
                {
                    #region Show Log
                    case ActionsEnum.SHOWLOG:
                        //this.SelectedDocPk = Convert.ToInt32(((HiddenField)((ImageButton)sender).Parent.Parent.FindControl("hdfDocPk")).Value);
                        this.SelectedDocPk = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                        GetFieldValues(ControlsEnum.LOGLIST);
                        SetFieldValues(ControlsEnum.LOGLIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCheckOutControlCheckInCheckOutLog]','" + Resources.PageNameRes.Log + "','800','500');", true);
                        break;
                    #endregion
                    #region Save
                    case ActionsEnum.SAVE:
                        this.CheckInParaMeter = (EmployeeDocumentCheckInCheckOutParameterBinder)SetUiValuesToObject(ActionsEnum.SAVE);
                        int retVal = EmployeeDocBL.CheckInOrCheckOut(this.CheckInParaMeter);
                        if (retVal < 0)
                        {
                            throw new ApplicationException("Data not saved");
                        }
                        clearControl();
                        if (this.AfterSave != null)
                        {
                            AfterSave(ActionsEnum.CHECKOUT);
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #endregion
                    case ActionsEnum.CANCEL:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    case ActionsEnum.CHECKOUTLOGCLOSE:
                        if (this.LogClosed != null)
                        {
                            LogClosed(sender, EventArgs.Empty);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
                if (Error != null && commonActions == ActionsEnum.SAVE)
                {
                    Button btn = new Button();
                    btn.CommandName = ActionsEnum.CHECKOUTERROR.ToString();
                    Error(btn, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Collect All Data From UI (including Complex Object) to Local Field Variables
        /// </summary>
        /// <param name="controlsEnum"></param>
        private object SetUiValuesToObject(ActionsEnum mode)
        {
            object returnObj;
            returnObj = null;
            try
            {
                switch (mode)
                {
                    case ActionsEnum.SAVE:
                        returnObj = (EmployeeDocumentCheckInCheckOutParameterBinder)BindObject();
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
                returnObj = null;
            }
        }

        private void GetFieldValues(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                case ControlsEnum.INTERNALEXTENALTYPE:
                    pageData = EmployeeDocBL.GetInternalOrExtenalType(new ConfigurationParameterBinder
                    {
                        Active = (short)DbActiveStatus.ACTIVE,
                        ConfigPk = 0,
                        ConfigType = GTIService.Constants.HRMS.Employee.Constatnts.EMP_DOC_USAGE
                    });
                    break;
                case ControlsEnum.ISSUEDFOR:
                    pageData = EmployeeDocBL.GetIssuedFor(new ConfigurationParameterBinder
                    {
                        GroupTypeValue = GTIService.Constants.HRMS.Employee.Constatnts.HRM_GRP_TYPE_VALUE,
                        GroupValue = GTIService.Constants.HRMS.Employee.Constatnts.HRM_GRP_VALUE
                    });
                    break;
                case ControlsEnum.LOGLIST:
                    pageData = EmployeeDocBL.GetEmployeeDocsLog(this.SelectedDocPk);
                    break;
                default:
                    break;
            }
        }

        private EmployeeDocumentCheckInCheckOutParameterBinder BindObject()
        {
            DateTime dummyDate;
            int dummyInt;
            int? doneByID = null;
            DateTime? expectedReturnDate = null;
            DateTime issuedOutOnDate;

            #region Validation Logic
            if (txtPurpose.Text.Trim().IsNullOrEmptyOrWhitespace())
            {
                throw new ApplicationException("Purpose required");
            }

            if (!DateTime.TryParse(txtIssuedOutOn.Text.Trim(), out issuedOutOnDate))
            {
                throw new ApplicationException("Invalid Checked In Date");
            }

            TimeSpan checkedInOnTime;
            if (!TimeSpan.TryParse(txtIssuedOutOnTime.Text.Trim(), out checkedInOnTime))
            {
                throw new ApplicationException("Invalid Checked In Time");
            }

            if (DateTime.TryParse(txtExpectedReturnDate.Text.Trim(), out dummyDate))
            {
                expectedReturnDate = dummyDate;
            }
            else if (txtExpectedReturnDate.Text.Trim().Length > 0)
            {
                throw new ApplicationException("Invalid Expected Return Date");
            }

            if (ddlInternalOrExternal.SelectedValue.ToString() == GTIService.Constants.HRMS.Employee.Constatnts.HRM_EMP_INTERNAL.ToString())
            {
                if (!Int32.TryParse(hdfCheckOutAutoIssuedTo.Value.Trim(), out dummyInt))
                {
                    throw new ApplicationException("Choose an Employee");
                }
                else
                {
                    doneByID = dummyInt;
                }
            }
            #endregion

            EmployeeDocumentCheckInCheckOutParameterBinder retObj = new EmployeeDocumentCheckInCheckOutParameterBinder
            {
                CurrentState = (int)CheckStateStatus.CheckedOut,
                User = currentUser.PKUser,
                CheckedInOnDate = issuedOutOnDate,
                DetailsPk = 0,
                Details = this.SelectedPkArray,
                Purpose = txtPurpose.Text.Trim(),
                IssuedFor = Convert.ToInt32(ddlIssuedFor.SelectedValue),
                Category = Convert.ToInt16(ddlInternalOrExternal.SelectedValue),
                ExpectedReturnDate = expectedReturnDate,
                Remarks = txtRemarks.Text.Trim(),
                Active = (short)DbActiveStatus.ACTIVE,
                BizUnit = currentUser.SBUID
            };

            if (retObj.CheckedInOnDate.HasValue)
                retObj.CheckedInOnDate = retObj.CheckedInOnDate.Value.Add(checkedInOnTime);

            if (ddlInternalOrExternal.SelectedValue.ToString() == GTIService.Constants.HRMS.Employee.Constatnts.HRM_EMP_INTERNAL.ToString())
            {
                //Internal
                retObj.DoneByID = doneByID;
                retObj.DoneByText = txtCheckOutAutoIssuedTo.Text.Trim();
            }
            else
            {
                //External
                retObj.DoneByText = txtCheckOutIssuedTo.Text.Trim();
            }

            return retObj;
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
                    case ControlsEnum.LOGLIST:
                        BindGrid(ControlsEnum.LOGLIST);
                        break;
                    case ControlsEnum.DEFAULT:
                        BindGrid(ControlsEnum.EMPDOCLIST);
                        GetFieldValues(ControlsEnum.INTERNALEXTENALTYPE);
                        SetFieldValues(ControlsEnum.INTERNALEXTENALTYPE);
                        GetFieldValues(ControlsEnum.ISSUEDFOR);
                        SetFieldValues(ControlsEnum.ISSUEDFOR);
                        break;
                    case ControlsEnum.INTERNALEXTENALTYPE:
                        BindDropDown(ControlsEnum.INTERNALEXTENALTYPE);
                        break;
                    case ControlsEnum.ISSUEDFOR:
                        BindDropDown(ControlsEnum.ISSUEDFOR);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindDropDown(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                case ControlsEnum.INTERNALEXTENALTYPE:

                    ddlInternalOrExternal.Items.Clear();

                    ddlInternalOrExternal.DataSource = pageData;
                    ddlInternalOrExternal.DataTextField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_TEXT;
                    ddlInternalOrExternal.DataValueField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_VALUE;
                    ddlInternalOrExternal.DataBind();
                    ddlInternalOrExternal.Items.HtmlDecode();
                    //Commenetd by sarath Due to CR by Syed Sir
                    //ddlInternalOrExternal.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));

                    break;
                case ControlsEnum.ISSUEDFOR:

                    ddlIssuedFor.Items.Clear();

                    ddlIssuedFor.DataSource = pageData;
                    ddlIssuedFor.DataTextField = GTIService.Constants.HRMS.Employee.Fields.HRM_CON_TEXT;
                    ddlIssuedFor.DataValueField = GTIService.Constants.HRMS.Employee.Fields.HRM_CON_VALUE;
                    ddlIssuedFor.DataBind();
                    ddlIssuedFor.Items.HtmlDecode();
                    ddlIssuedFor.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
            }
        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitCheckoutComponents", "$(document).ready(function(){InitCheckoutComponents();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        public void SetProperties()
        {
            if (this.SelectedDocItems != null)
            {
                this.SelectedPkArray = this.SelectedDocItems.AsEnumerable()
                    .Select(x =>
                        new EmployeeDocumentCheckInCheckOutDetailsParameterBinder
                        {
                            DetailsPK = x.Field<int>(Resources.DataFieldRes.EmpDocPk)
                        }
                     ).ToList();
            }
            BindGrid(ControlsEnum.EMPDOCLIST);
        }

        private void BindGrid(ControlsEnum control)
        {
            switch (control)
            {
                case ControlsEnum.DEFAULT:
                    BindGrid(ControlsEnum.EMPDOCLIST);
                    break;
                case ControlsEnum.EMPDOCLIST:
                    this.grdEmpDocList.DataSource = this.SelectedDocItems;
                    this.grdEmpDocList.DataBind();
                    break;
                case ControlsEnum.LOGLIST:
                    this.grdCheckIncheckOutLog.DataSource = pageData;
                    this.grdCheckIncheckOutLog.DataBind();
                    break;
                default:
                    break;
            }
        }

        private void clearControl()
        {
            grdCheckIncheckOutLog.DataSource = null;
            grdCheckIncheckOutLog.DataBind();

            grdEmpDocList.DataSource = null;
            grdEmpDocList.DataBind();

            if (ddlInternalOrExternal.Items.Count > 0) ddlInternalOrExternal.SelectedIndex = 0;
            if (ddlIssuedFor.Items.Count > 0) ddlIssuedFor.SelectedIndex = 0;

            txtIssuedOutOn.Text = string.Empty;
            txtPurpose.Text = string.Empty;
            hdfCheckOutAutoIssuedTo.Value = string.Empty;
            txtCheckOutAutoIssuedTo.Text = string.Empty;
            txtExpectedReturnDate.Text = string.Empty;
            txtRemarks.Text = string.Empty;
        }
    }
}