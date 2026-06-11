using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject.HRMS.Employee;
using ERPSMS_v01.UserControls;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessLogic.CommonManagement;
using BusinessObject.HRMS.Admin.Masters;
using BusinessLogic.HRMS.Admin.Masters;
using BusinessObject.CommonManagement;
using GTIService.Constants.HRMS.Admin.Masters;

namespace HRMS.Admin.Masters
{
    public partial class LeaveTypeMaster : ERP.Store.UI.MyBasePage
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

        private ActionsEnum commonActions;
        private DataTable dtAccural;
        private DataTable dtLeaveType;
        private DataTable dtAutoCrType;
        private BusinessObject.User currentUser;
        LeaveTypeMasterBO objLeaveType;
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

        #region Helper Methods

        private void SetCurrPk()
        {
            foreach (GridViewRow grdrow in grdList.Rows)
            {
                RadioButton rbtn;
                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                if (rbtn.Checked)
                {
                    CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfLeavePk")).Value);
                }
            }
        }

        #endregion

        #region Common UI Methods

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
                    #region Accural
                    case ControlsEnum.ACCURAL:
                        dtAccural = CommonBL.GetAppConfig(currentUser.SBUID, GetLocalResourceObject("AccuralType").ToString());
                        break;
                    #endregion
                    #region List
                    case ControlsEnum.LIST:
                        dtLeaveType = LeaveTypeMasterBL.GetLeaveType(null, string.Empty, string.Empty, 0, currentUser.SBUID, PageIndex, Convert.ToInt32(GetLocalResourceObject("PageSize")),
                            -1, Convert.ToString(GetLocalResourceObject("SortOderBy")));
                        break;
                    #endregion
                    #region FilterList
                    case ControlsEnum.FILTERLIST:
                        int accural = Convert.ToInt32(ddlSearchAccural.SelectedValue);
                        dtLeaveType = LeaveTypeMasterBL.GetLeaveType(null, HttpUtility.HtmlEncode(txtSearchCode.Text.Trim()), HttpUtility.HtmlEncode(txtSearchName.Text.Trim()), chkSearchActive.Checked ? 1 : 0, currentUser.SBUID, 1, Convert.ToInt32(GetLocalResourceObject("PageSize")), accural, string.Empty);
                        break;
                    #endregion
                    #region Edit
                    case ControlsEnum.EDIT:
                        dtLeaveType = LeaveTypeMasterBL.GetLeaveType(CurrPK, string.Empty, string.Empty, (int)DbActiveStatus.HASPK, currentUser.SBUID, 1, Convert.ToInt32(GetLocalResourceObject("PageSize")), -1, string.Empty);
                        break;
                    #endregion
                    #region AUTO CREDIT TYPE
                    case ControlsEnum.AUTOCREDITTYPE:
                        dtAutoCrType = CommonBL.GetAppConfig(currentUser.SBUID, GetLocalResourceObject("AutoCreditType").ToString());
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
                    #region ACCURAL
                    case ControlsEnum.ACCURAL:
                        BindDropDown(ControlsEnum.ACCURAL);
                        break;
                    #endregion
                    #region LIST
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region AUTO CREDIT TYPE:
                    case ControlsEnum.AUTOCREDITTYPE:
                        BindDropDown(ControlsEnum.AUTOCREDITTYPE);
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
                        if (dtLeaveType != null && dtLeaveType.Rows.Count > 0)
                        {
                            int rowCount = 0;

                            rowCount = Convert.ToInt32(dtLeaveType.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                            this.TotalPages = Convert.ToInt32(dtLeaveType.Rows[0]["ROW_NO"].ToString());

                            uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                              (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                              (rowCount / this.PageSize) + 1;
                            PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdList.DataSource = dtLeaveType;
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
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                switch (Mode)
                {
                    //case ActionsEnum.DEFAULT:
                    //    GetFieldValues(ControlsEnum.LIST);
                    //    SetFieldValues(ControlsEnum.LIST);
                    //    break;
                    //case ActionsEnum.EMPDOCDETAIL:
                    //case ActionsEnum.EDIT:
                    //    GetFieldValues(ControlsEnum.SELECTEDEMPTYPE);
                    //    SetFieldValues(ControlsEnum.SELECTEDEMPTYPE);
                    //    break;
                    case ActionsEnum.NEW:
                        ResetForm(ActionsEnum.NEW);
                        EntryStatus = EntryStatus.NEWMODE;
                        txtCode.Focus();
                        break;
                    //case ActionsEnum.VIEW:
                    //    SetUIEditView(ActionsEnum.EDIT);
                    //    break;
                    //case ActionsEnum.SAVE:
                    //    break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
                        SetCurrPk();
                        if (CurrPK > 0)
                        {
                            GetFieldValues(ControlsEnum.EDIT);
                            if (dtLeaveType != null && dtLeaveType.Rows.Count > 0)
                            {
                                txtCode.Text = dtLeaveType.Rows[0][Fields.LTM_CODE].ToString();
                                txtName.Text = dtLeaveType.Rows[0][Fields.LTM_NAME].ToString();
                                txtDescription.Text = dtLeaveType.Rows[0][Fields.LTM_DESC].ToString();
                                txtLimit.Text = dtLeaveType.Rows[0][Fields.LTM_CARRY_FWD_LIMIT].ToString();
                                txtMaxEligibility.Text = dtLeaveType.Rows[0][Fields.LTM_LIMIT].ToString();
                                txtRate.Text = dtLeaveType.Rows[0][Fields.LTM_RATE].ToString();
                                txtFactor.Text = dtLeaveType.Rows[0][Fields.LTM_FACTOR].ToString();
                                GetFieldValues(ControlsEnum.ACCURAL);
                                SetFieldValues(ControlsEnum.ACCURAL);
                                ddlAccural.SelectedValue = dtLeaveType.Rows[0][Fields.LTM_ACCURAL].ToString();
                                GetFieldValues(ControlsEnum.AUTOCREDITTYPE);
                                SetFieldValues(ControlsEnum.AUTOCREDITTYPE);
                                ddlAutoCrType.SelectedValue = dtLeaveType.Rows[0][Fields.LTM_AUTO_CREDIT].ToString();
                                EntryStatus = EntryStatus.EDITMODE;
                                chkPaid.Checked = Convert.ToBoolean(dtLeaveType.Rows[0][Fields.LTM_PAID]);
                                chkCarryForward.Checked = Convert.ToBoolean(dtLeaveType.Rows[0][Fields.LTM_CARRY_FWD]);
                                chkEncashable.Checked = Convert.ToBoolean(dtLeaveType.Rows[0][Fields.LTM_ENCASH]);
                                chkCredit.Checked = Convert.ToBoolean(dtLeaveType.Rows[0][Fields.LTM_CREDIT]);
                                chkActive.Checked = Convert.ToBoolean(dtLeaveType.Rows[0][Fields.LTM_ACTIVE]);
                                LastModifiedTime = Convert.ToDateTime(dtLeaveType.Rows[0][Fields.LAST_MOD_DT].ToString());
                            }

                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private object SetUIValuesToObject(ControlsEnum ControlType)
        {
            object returnObj;
            returnObj = null;
            objLeaveType = new LeaveTypeMasterBO();
            try
            {
                switch (ControlType)
                {
                    case ControlsEnum.LEAVETYPEDETAILS:
                        objLeaveType.LTM_PK = this.CurrPK;
                        objLeaveType.LTM_CODE = txtCode.Text;
                        objLeaveType.LTM_NAME = txtName.Text;
                        objLeaveType.LTM_ACCURAL = Convert.ToInt16(ddlAccural.SelectedValue);
                        objLeaveType.LTM_DESC = txtDescription.Text;
                        objLeaveType.LTM_PAID = Convert.ToInt16(chkPaid.Checked);
                        objLeaveType.LTM_ENCASH = Convert.ToInt16(chkEncashable.Checked);
                        objLeaveType.LTM_CREDIT = Convert.ToInt16(chkCredit.Checked);
                        objLeaveType.LTM_CARRY_FWD = Convert.ToInt16(chkCarryForward.Checked);
                        objLeaveType.LTM_ACTIVE = Convert.ToInt16(chkActive.Checked);
                        objLeaveType.LTM_RATE = txtRate.Text.Trim().IsNullOrEmptyOrWhitespace() ? 0 : Convert.ToDouble(txtRate.Text.Trim());
                        objLeaveType.LTM_FACTOR = txtFactor.Text.Trim().IsNullOrEmptyOrWhitespace() ? 0 : Convert.ToDouble(txtFactor.Text.Trim());
                        objLeaveType.LTM_CARRY_FWD_LIMIT = txtLimit.Text.Trim().IsNullOrEmptyOrWhitespace() ? 0 : Convert.ToDouble(txtLimit.Text.Trim());
                        objLeaveType.LTM_LIMIT = txtMaxEligibility.Text.Trim().IsNullOrEmptyOrWhitespace() ? 0 : Convert.ToDouble(txtMaxEligibility.Text.Trim());
                        objLeaveType.LTM_DEPT = currentUser.CurrentDeptPK;
                        objLeaveType.LTM_COMPANY = currentUser.SBUID;
                        objLeaveType.LTM_BIZUNIT = currentUser.SBUID;
                        objLeaveType.USER_PK = currentUser.PKUser;
                        objLeaveType.LTM_AUTO_CREDIT = Convert.ToInt32(ddlAutoCrType.SelectedValue);
                        returnObj = objLeaveType;
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
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region ACCURAL
                case ControlsEnum.ACCURAL:
                    ddlSearchAccural.Items.Clear();
                    ddlSearchAccural.DataSource = dtAccural;
                    ddlSearchAccural.DataTextField = Fields.ADM_CFG_TEXT;
                    ddlSearchAccural.DataValueField = Fields.ADM_CFG_VALUE;
                    ddlSearchAccural.DataBind();
                    ddlSearchAccural.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                    ddlAccural.Items.Clear();
                    ddlAccural.DataSource = dtAccural;
                    ddlAccural.DataTextField = Fields.ADM_CFG_TEXT;
                    ddlAccural.DataValueField = Fields.ADM_CFG_VALUE;
                    ddlAccural.DataBind();
                    break;
                #endregion
                #region AUTO CREDIT TYPE
                case ControlsEnum.AUTOCREDITTYPE:
                    ddlAutoCrType.Items.Clear();
                    ddlAutoCrType.DataSource = dtAutoCrType;
                    ddlAutoCrType.DataTextField = Fields.ADM_CFG_TEXT;
                    ddlAutoCrType.DataValueField = Fields.ADM_CFG_VALUE;
                    ddlAutoCrType.DataBind();                    
                    break;
                #endregion
            }
        }
        protected void ResetForm(ActionsEnum action)
        {
            switch (action)
            {
                case ActionsEnum.SAVE:
                    clearControls();
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    break;
                case ActionsEnum.LIST:
                    clearControls();
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    break;
                case ActionsEnum.NEW:
                    clearControls();
                    break;
                case ActionsEnum.CANCEL:
                    clearControls();
                    //lblLastModifiedHDR.Text = string.Empty;
                    break;
                case ActionsEnum.CLEAR:
                    txtSearchCode.Text = string.Empty;
                    txtSearchName.Text = string.Empty;
                    GetFieldValues(ControlsEnum.ACCURAL);
                    SetFieldValues(ControlsEnum.ACCURAL);
                    ddlSearchAccural.SelectedIndex = -1;
                    chkSearchActive.Checked = true;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    break;
            }
        }
        private void clearControls()
        {
            txtCode.Text = string.Empty;
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtLimit.Text = string.Empty;
            txtMaxEligibility.Text = string.Empty;
            txtRate.Text = string.Empty;
            txtFactor.Text = string.Empty;
            chkPaid.Checked = false;
            chkCarryForward.Checked = false;
            chkEncashable.Checked = false;
            chkCredit.Checked = false;
            chkActive.Checked = true;
            ddlAccural.SelectedIndex = 0;
            EntryStatus = EntryStatus.LISTMODE;
            GetFieldValues(ControlsEnum.LIST);
            SetFieldValues(ControlsEnum.LIST);
            this.CurrPK = 0;

            ////uncomment this
            ////grdWorkingDays.DataSource = null;
            ////grdWorkingDays.DataBind();
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
            GridViewRow gvrTemplate;
            try
            {
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
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {

                    //  commonActions = ActionsEnum.SHOWDETAILS;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    // commonActions = ActionsEnum.TOOLTIP;
                }
                switch (commonActions)
                {
                    #region Default & Listing
                    case ActionsEnum.DEFAULT:
                    case ActionsEnum.LIST:
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region ITEMSELECTED
                    case ActionsEnum.ITEMSELECTED:
                        bool bIsChecked = false;
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                this.CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfLeavePk")).Value);
                            }
                        }
                        if (!bIsChecked)
                        {
                            throw new ApplicationException("Items not selected");
                        }
                        break;
                    #endregion
                    #region Save
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            objLeaveType = (LeaveTypeMasterBO)SetUIValuesToObject(ControlsEnum.LEAVETYPEDETAILS);
                            if (objLeaveType != null)
                            {
                                string xmlDoc = CommonFunctions.XmlSerialize(objLeaveType);
                                result = LeaveTypeMasterBL.SaveLeaveType(xmlDoc);
                                if (result >= 0)
                                {
                                    ResetForm(ActionsEnum.CANCEL);
                                    GetFieldValues(ControlsEnum.ACCURAL);
                                    SetFieldValues(ControlsEnum.ACCURAL);
                                    GetFieldValues(ControlsEnum.LIST);
                                    SetFieldValues(ControlsEnum.LIST);
                                    btnNew.Focus();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup", "ClosePopup();", true);
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("LeaveType").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divDetailList]','" + GetLocalResourceObject("DetailList").ToString() + "','" + GetLocalResourceObject("popWidth") + "','" + GetLocalResourceObject("popHeight") + "');", true);
                                    #region Error Messages
                                    if (result == (int)DbSaveStatus.SQLERROR)
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CONCURRENCY)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("LeaveType").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.REFNOEXIST)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("LeaveType").ToString() + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("LeaveType").ToString() + " " + Resources.Messages.AlreadyExists;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("LeaveType").ToString());
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
                        EntryStatus = EntryStatus.NEWMODE;
                        SetUIEditView(ActionsEnum.NEW);
                        break;
                    #endregion
                    #region Filter
                    case ActionsEnum.FILTER:
                        GetFieldValues(ControlsEnum.FILTERLIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region Edit

                    case ActionsEnum.EDIT:
                    case ActionsEnum.DETAIL:
                        GetUIValuesFromObject(ControlsEnum.EDIT);
                        txtCode.Focus();
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        ResetForm(ActionsEnum.CANCEL);
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetForm(ActionsEnum.CLEAR);
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        // Delete Aircraft Details By Aircraft PK - Return 1 is Success , 0- Fail
                        if (CurrPK > 0)
                        {
                            result = LeaveTypeMasterBL.DeleteLeaveType(CurrPK, this.LastModifiedTime);
                            if (result > 0)
                            {
                                ResetForm(ActionsEnum.CANCEL);
                                if (grdList.Rows.Count == 0 && PageIndex > 1)
                                {
                                    PageIndex--;
                                }
                                GetFieldValues(ControlsEnum.LIST);
                                SetFieldValues(ControlsEnum.LIST);
                                btnNew.Focus();
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("LeaveType").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                            }
                            else
                            {
                                #region Error Message
                                if (result == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("LeaveType").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.REFNOEXIST)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("LeaveType").ToString() + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("LeaveType").ToString() + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("LeaveType").ToString() + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                            litErrorMsg.Text = this.GetGlobalResourceObject("ErpRes", "Msg_Not_Selected").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        break;

                    #endregion
                    #region Activate
                    // Do Action if click Activate Button
                    case ActionsEnum.ACTIVATE:
                        gvrTemplate = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        result = LeaveTypeMasterBL.UpdateLeaveTypeMasterStatus(
                            Convert.ToInt32(((HiddenField)grdList.Rows[gvrTemplate.RowIndex].FindControl("hdfLeavePk")).Value), 1, currentUser.PKUser, string.Empty);

                        if (result > 0)
                        {
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Activate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeType.ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        else
                        {
                            // if error or exception occur
                            DBActiveInactiveStatus dBActiveInactiveStatus = (DBActiveInactiveStatus)(result);
                            switch (dBActiveInactiveStatus)
                            {
                                // For Sql Error
                                case DBActiveInactiveStatus.SQLERROR:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;

                                case DBActiveInactiveStatus.CONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Modified;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeType.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeType.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                default:
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                            }
                        }
                        break;
                    #endregion
                    #region DEACTIVATE
                    // Do Action if click DeActivate Button
                    case ActionsEnum.DEACTIVATE:
                        gvrTemplate = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        result = LeaveTypeMasterBL.UpdateLeaveTypeMasterStatus(
                            Convert.ToInt32(((HiddenField)grdList.Rows[gvrTemplate.RowIndex].FindControl("hdfLeavePk")).Value), 0, currentUser.PKUser, string.Empty);
                        if (result > 0)
                        {
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_InActivate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeType.ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        else
                        {
                            // if error or exception occur
                            DBActiveInactiveStatus dBActiveInactiveStatus = (DBActiveInactiveStatus)(result);
                            switch (dBActiveInactiveStatus)
                            {
                                // For Sql Error
                                case DBActiveInactiveStatus.SQLERROR:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;

                                case DBActiveInactiveStatus.CONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Modified;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeType.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeType.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                default:
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                            }
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(GetLocalResourceObject("DuplicateException").ToString()) && ex.Message.Contains(GetLocalResourceObject("LeaveType").ToString()))
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex).Replace("Code", Resources.Controls.PackingSpecs)) + "','" + Resources.Messages.Information + "');", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {

            }
        }
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex;
            //  EntryStatus = EntryStatus.LISTMODE;
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
                Session["Sorting"] = 1;
                //if (Session["FilterList"] != null)
                //{
                GetFieldValues(ControlsEnum.LIST);
                SetFieldValues(ControlsEnum.LIST);
                DataTable dt = new DataTable();
                grdList.DataSource = dtLeaveType;
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
                    DataView sortedView = new DataView(dtLeaveType);
                    sortedView.Sort = e.SortExpression + " " + SortDir;
                    grdList.DataSource = sortedView;
                    grdList.DataBind();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            //try
            //{
            //    if (SortBy == e.SortExpression)
            //    {
            //        if (SortDirection == Resources.Report.SortAscending)
            //            SortDirection = Resources.Report.SortDescending;
            //        else
            //            SortDirection = Resources.Report.SortAscending;
            //    }
            //    else
            //    {
            //        SortBy = e.SortExpression;
            //        SortDirection = Resources.Report.SortAscending;
            //    }
            //    this.PageIndex = 1;
            //    EntryStatus = EntryStatus.LISTMODE;
            //}
            //catch (Exception ex)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            //}
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
            uclPaging.CurrentPage = 1;
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
            }
            else if (EntryStatus == EntryStatus.NEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
            }
            else if (EntryStatus == EntryStatus.ENTRYMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
            }

        }
        #endregion

        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            this.currentUser = (BusinessObject.User)Context.User.Identity;
            try
            {
                // InitializeComponent();
                if (!IsPostBack)
                {
                    this.PageIndex = 1;
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                    GetFieldValues(ControlsEnum.ACCURAL);
                    SetFieldValues(ControlsEnum.ACCURAL);
                    GetFieldValues(ControlsEnum.AUTOCREDITTYPE);
                    SetFieldValues(ControlsEnum.AUTOCREDITTYPE);
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    PageIndex = Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE);
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
            ACCURAL,
            LEAVETYPEDETAILS,
            CANCEL,
            CLEAR,
            LIST,
            FILTERLIST,
            EDIT,
            AUTOCREDITTYPE

        }
        #endregion
    }
}