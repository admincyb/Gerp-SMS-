using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject.AccountManagement;
using BusinessObject;
using ERPManager;
using ERPSMS_v01.UserControls;
using System.Data;
using BusinessObject.CommonManagement;
using BusinessObject.SaleOrder;
using System.Xml;
using System.Text;
using System.Threading;
using ERPService;
using ERPData;
using System.Web.UI.HtmlControls;
using BusinessObject.AlertManagement;
using BusinessObject.Finance;
using ERP.Store.UI;



namespace ERPSMS_v01.Finance
{
    public partial class FinancialYearMaster : ERP.Store.UI.MyBasePage
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
        private DataTable dtFinYearDetails;
        private DataTable dtAutoCrType;
        private BusinessObject.User currentUser;
        FinancialYearMasterBO objFinYear;
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
                    #region List
                    case ControlsEnum.LIST:
                        dtFinYearDetails = BusinessLogic.Finance.FinancialYearMasterBL.GetFinYearDetails(  (int)DbActiveStatus.ALL, currentUser.SBUID);
                        break;
                    #endregion
                   
                    #region Edit
                    case ControlsEnum.EDIT:
                        dtFinYearDetails = BusinessLogic.Finance.FinancialYearMasterBL.GetFinYearEditDetails(CurrPK, (int)DbActiveStatus.ACTIVE, currentUser.SBUID);
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
                 
                    #region LIST
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
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
                        if (dtFinYearDetails != null && dtFinYearDetails.Rows.Count > 0)
                        {
                          
                            grdList.DataSource = dtFinYearDetails;
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
                    case ActionsEnum.NEW:
                        ResetForm(ActionsEnum.NEW);
                        EntryStatus = EntryStatus.NEWMODE;
                   //     txtCode.Focus();
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
                            if (dtFinYearDetails != null && dtFinYearDetails.Rows.Count > 0)
                            {
                                txtName.Text = dtFinYearDetails.Rows[0][Field.FYR_NAME].ToString();
                                txtDesc.Text = dtFinYearDetails.Rows[0][Field.FYR_DESC].ToString();
                                txtFromDate.Text = Convert.ToDateTime(dtFinYearDetails.Rows[0][Field.FYR_DATE_FROM]).ToString(Resources.Constants.DateFormatShort);
                                txtToDate.Text = Convert.ToDateTime(dtFinYearDetails.Rows[0][Field.FYR_DATE_TO]).ToString(Resources.Constants.DateFormatShort);               
                                EntryStatus = EntryStatus.EDITMODE;
                                chkActive.Checked = Convert.ToBoolean(dtFinYearDetails.Rows[0][Field.FYR_ACTIVE]);                               
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
            objFinYear = new FinancialYearMasterBO();
            try
            {
                switch (ControlType)
                {
                    case ControlsEnum.FINYEARDETAILS:
                        objFinYear.FYR_PK = this.CurrPK;
                        objFinYear.FYR_NAME = txtName.Text.ToString();
                        objFinYear.FYR_DESC = txtDesc.Text.ToString();                      
                        objFinYear.FYR_DATE_FROM =txtFromDate.Text.ToString();
                        objFinYear.FYR_DATE_TO = txtToDate.Text.ToString();                       
                        objFinYear.FYR_ACTIVE = Convert.ToInt16(chkActive.Checked);                                                              
                        returnObj = objFinYear;
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
                    break;
                case ActionsEnum.CLEAR:                  
                    chkActive.Checked = true;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    break;
            }
        }
        private void clearControls()
        {
           
            txtName.Text = string.Empty;
            txtDesc.Text = string.Empty;
            txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;                 
            chkActive.Checked = false;           
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
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;
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
                            objFinYear = (FinancialYearMasterBO)SetUIValuesToObject(ControlsEnum.FINYEARDETAILS);
                            if (objFinYear != null)
                            {

                                result = BusinessLogic.Finance.FinancialYearMasterBL.SaveFinancialYear(objFinYear, currentUser.CurrentDeptPK, currentUser.PKUser, currentUser.SBUID, string.Empty);
                                if (result >= 0)
                                {
                                    ResetForm(ActionsEnum.CANCEL);                                  
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
                                    else if (result == (int)DbSaveStatus.DATEOVERLAP)
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
                      //  txtCode.Focus();
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
                            result = BusinessLogic.Finance.FinancialYearMasterBL.DeleteFinancialYear(CurrPK, string.Empty);
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
                grdList.DataSource = dtFinYearDetails;
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
                    DataView sortedView = new DataView(dtFinYearDetails);
                    sortedView.Sort = e.SortExpression + " " + SortDir;
                    grdList.DataSource = sortedView;
                    grdList.DataBind();
                }
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
            FINYEARDETAILS,
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