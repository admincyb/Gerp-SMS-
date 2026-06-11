using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject.HRMS.Employee;
using ERP.Utilities;
using BusinessObject.CommonManagement;
using BusinessLogic.HRMS.Employee;

namespace HRMS.Employees.UserControls
{
    public partial class CheckInControl : System.Web.UI.UserControl
    {
        private EmployeeDocumentCheckInCheckOutParameterBinder CheckInParaMeter;
        private BusinessObject.User currentUser;

        private DataTable pageData;
        private int SelectedDocPk;
        public DataTable SelectedDocItems { get; set; }

        public delegate void AfterSaveHandler(ActionsEnum action);
        public event AfterSaveHandler AfterSave;
        public event EventHandler Error;
        public event EventHandler LogClosed;

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
            SetFieldValues(ControlsEnum.DEFAULT);
            if (!IsPostBack)
            {
                txtCheckedInOn.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                txtCheckedInOnTime.Text = DateTime.Now.ToString(CommonConstants.TIMEFORMAT);
                hdfCheckInAutoCheckedInBy.Value = currentUser.PKEmployee.ToString();
                txtCheckInAutoCheckedInBy.Text = currentUser.EmpName;
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
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCheckInControlCheckInCheckOutLog]','" + Resources.PageNameRes.Log + "','800','500');", true);
                        break;
                    #endregion
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
                            AfterSave(ActionsEnum.CHECKIN);
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    case ActionsEnum.CANCEL:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    case ActionsEnum.CHECKINLOGCLOSE:
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
                    btn.CommandName = ActionsEnum.CHECKINERROR.ToString();
                    btn.CommandArgument = ex.Message;
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

        private EmployeeDocumentCheckInCheckOutParameterBinder BindObject()
        {
            DateTime dummyDate;
            int dummyInt;
            DateTime? checkedInOnDate = null;
            TimeSpan? checkedInOnTime = null;

            if (DateTime.TryParse(txtCheckedInOn.Text.Trim(), out dummyDate))
            {
                checkedInOnDate = dummyDate;

                TimeSpan _dummyTimeSpan;
                if (TimeSpan.TryParse(txtCheckedInOnTime.Text.Trim(), out _dummyTimeSpan))
                {
                    checkedInOnTime = _dummyTimeSpan;
                }
                else if (txtCheckedInOnTime.Text.Trim().Length > 0)
                {
                    throw new ApplicationException("Invalid Checked In On Time");
                }
            }
            else if(txtCheckedInOn.Text.Trim().Length > 0)
            {
                throw new ApplicationException("Invalid Checked In On Date");
            }

            if ((hdfCheckInAutoCheckedInBy.Value.Trim() != string.Empty || hdfCheckInAutoCheckedInBy.Value.Trim() != "0") 
                && !Int32.TryParse(hdfCheckInAutoCheckedInBy.Value.Trim(), out dummyInt))
            {
                throw new ApplicationException("Select an Employee");
            }

            EmployeeDocumentCheckInCheckOutParameterBinder retObj = new EmployeeDocumentCheckInCheckOutParameterBinder
            {
                CurrentState = (int)CheckStateStatus.CheckedIn,
                User=currentUser.PKUser,
                CheckedInOnDate = checkedInOnDate,
                DetailsPk = 0,
                Details = this.SelectedPkArray,
                DoneByID = Convert.ToInt32(hdfCheckInAutoCheckedInBy.Value),
                DoneByText = txtCheckInAutoCheckedInBy.Text.Trim() == "Select/Type" 
                                ? string.Empty 
                                : txtCheckInAutoCheckedInBy.Text.Trim(),
                Remarks = txtRemarks.Text.Trim(),
                Active = (short)DbActiveStatus.ACTIVE,
                BizUnit = currentUser.SBUID              
            };

            if (retObj.CheckedInOnDate.HasValue && checkedInOnTime.HasValue) 
                retObj.CheckedInOnDate = retObj.CheckedInOnDate.Value.Add(checkedInOnTime.Value);
            return retObj;
        }

        private void GetFieldValues(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                case ControlsEnum.LOGLIST:
                    pageData = EmployeeDocBL.GetEmployeeDocsLog(this.SelectedDocPk);
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
                    case ControlsEnum.DEFAULT:
                        BindGrid(ControlsEnum.LOGLIST);
                        break;
                    case ControlsEnum.LOGLIST:
                        BindGrid(ControlsEnum.LOGLIST);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitCheckinComponents", "$(document).ready(function(){InitCheckinComponents();});", true);
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
                    })
                    .ToList();
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
            grdEmpDocList.DataSource = null;
            grdEmpDocList.DataBind();

            grdCheckIncheckOutLog.DataSource = null;
            grdCheckIncheckOutLog.DataBind();

            txtCheckedInOn.Text = string.Empty;
            txtCheckedInOnTime.Text = string.Empty;
            txtRemarks.Text = string.Empty;

            hdfCheckInAutoCheckedInBy.Value = currentUser.PKEmployee.ToString();
            txtCheckInAutoCheckedInBy.Text = currentUser.EmpName;

        }
    }
}