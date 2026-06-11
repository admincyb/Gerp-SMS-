using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.HRMS.Employee;
using BusinessObject.HRMS.Admin.Masters;
using BusinessLogic.HRMS.Employee;
using System.Data;
using BusinessObject.Common;
using ERP.Utilities;
using BusinessObject.CommonManagement;
using System.IO;
using ERP.Utilities.HRMS;
using System.Web.UI.HtmlControls;
using BusinessLogic.HRMS.Admin.Masters;
using ERPSMS_v01.UserControls;
using BusinessLogic.CommonManagement;
using System.Threading;

namespace HRMS.Admin.Masters
{
    public partial class EmployeeType : ERP.Store.UI.MyBasePage //System.Web.UI.Page
    {
        #region Properties & variables

        #region Properties
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
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string PageIndex
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.PageIndex] ?? "1";
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndex] = value;
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
        /// To maintain the PageSize in viewstate
        /// </summary>
        private int PageSize
        {
            get
            {
                return (int)(this.ViewState[ViewstateStrings.PageSize] ?? Convert.ToInt32(GetLocalResourceObject("PageSize").ToString()));
            }
            set
            {
                this.ViewState[ViewstateStrings.PageSize] = value;
            }
        }

        /// <summary>
        /// To maintain the SortExpression or Sort By in viewstate
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
        /// To maintain the SortExpression or Then By in viewstate
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
        /// Entry Status
        /// </summary>
        private int ItemStatus
        {
            get
            {
                return (int)(ViewState[ERP.Utilities.ViewstateStrings.ItemStatus] ?? 1);
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.ItemStatus] = value;
            }
        }
        #endregion

        //private int SelectedDocPk;
        private ActionsEnum commonActions = ActionsEnum.DEFAULT;
        private DataTable pageData;
        DataTable dtResult;
        private BusinessObject.User currentUser;
        private EmployeeTypeBO selectedEmployeeType;
        // private List<EmployeeTypeBO> employeeTypeList;
        private List<WorkingHours> _workingHoursList = null;
        private int dummyPk = 0;
        #endregion

        #region Page Events

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

        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitPage", "$(document).ready(function(){InitPage();});", true);
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInactive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                    ModifiedDatePnl.Visible = true;
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInactive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                    ModifiedDatePnl.Visible = false;
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInactive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                    ModifiedDatePnl.Visible = false;
                }
                else if (EntryStatus == EntryStatus.EDITMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInactive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                    ModifiedDatePnl.Visible = true;
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabInactive").ToString();
                    ModifiedDatePnl.Visible = false;
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch(1);});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "WorkingDayTypeChange", "$(document).ready(function(){WorkingDayTypeChange();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        #endregion

        #region All Grid Events
        #region Custom Pager Control Navigated Event
        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void aActionHandler(object sender, DataNavigatorEventArgs e)
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

                PageIndex = uclPaging.CurrentPage.ToString();
                GetFieldValues(ControlsEnum.LIST);
                SetFieldValues(ControlsEnum.LIST);
                EntryStatus = EntryStatus.LISTMODE;
                EnableDisableButtons(e.TotalPages);
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }

        }
        #endregion

        #region OnPageIndexChanging Event
        /// <summary>
        /// Page Index Handler for grd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex.ToString();
            //EntryStatus = EntryStatus.LISTMODE;
        }
        #endregion

        #region OnSorting Event
        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            try
            {
                SortBy = e.SortExpression;
                if (SortDirection == Resources.Report.SortAscending)
                    SortDirection = Resources.Report.SortDescending;
                else
                    SortDirection = Resources.Report.SortAscending;

                EntryStatus = EntryStatus.LISTMODE;
                this.CurrPK = 0;
                GetFieldValues(ControlsEnum.LIST);
                SetFieldValues(ControlsEnum.LIST);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #endregion

        #region Action Handlers
        protected void ActionHandler(object sender, EventArgs e)
        {
            GridViewRow gvrTemplate;
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
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
                }
                if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlBatchOrLocation")
                    {
                        commonActions = ActionsEnum.BRANCHCHANGED;
                    }
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
                        //bool bIsChecked = false;
                        //foreach (GridViewRow grdrow in grdList.Rows)
                        //{
                        //    RadioButton rbtn;
                        //    rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                        //    if (rbtn.Checked)
                        //    {
                        //        bIsChecked = true;
                        //        this.CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEmpTypePK")).Value);
                        //    }
                        //}
                        //if (!bIsChecked)
                        //{
                        //    throw new ApplicationException("Items not selected");
                        //}
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        this.EntryStatus = EntryStatus.NEWMODE;
                        SetUIEditView(ActionsEnum.NEW);
                        GetFieldValues(ControlsEnum.WEEKDAYS);
                        SetFieldValues(ControlsEnum.WEEKDAYS);
                        SetFieldValues(ControlsEnum.WORKINGDAYS);
                        break;
                    #endregion
                    #region View
                    case ActionsEnum.VIEW:
                        if (this.CurrPK > 0)
                        {
                            EntryStatus = EntryStatus.VIEWMODE;
                            SetUIEditView(ActionsEnum.VIEW);
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.Report.Msg_Not_Selected) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Edit & Details
                    case ActionsEnum.EDIT:
                    case ActionsEnum.DETAIL:
                        bool bIsChecked = false;
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                this.CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEmpTypePK")).Value);
                            }
                        }                       
                        if (this.CurrPK > 0 && bIsChecked)
                        {
                            EntryStatus = EntryStatus.EDITMODE;
                            SetUIEditView(ActionsEnum.EDIT);
                            txtCode.Focus();
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.Report.Msg_Not_Selected) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Branch Or Location Changed
                    case ActionsEnum.BRANCHCHANGED:
                        GetFieldValues(ControlsEnum.WORKINGDAYS);
                        SetFieldValues(ControlsEnum.WORKINGDAYS);
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ActionsEnum.LIST);
                        break;
                    #endregion
                    #region Save
                    case ActionsEnum.SAVE:
                        selectedEmployeeType = (EmployeeTypeBO)SetUiValuesToObject(commonActions);
                        result = EmployeeTypeBL.Save(this.selectedEmployeeType);
                        if (result >= 0) // Success ! re-initialize the page
                        {
                            ResetForm(ActionsEnum.SAVE);
                            EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = string.Format(Resources.ErrorMessages.Msg_Save_Success, Resources.PageNameRes.EmployeeType);
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
                                litErrorMsg.Text = Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_Msg_CodeAlreadyExist").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeType + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.REFNOEXIST)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_Msg_NameAlreadyExist").ToString();
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
                            #endregion
                        }
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = EmployeeTypeBL.DeleteEmployeeType(this.CurrPK, this.LastModifiedTime);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeType);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm(ActionsEnum.SAVE);
                                GetFieldValues(ControlsEnum.LIST);
                                SetFieldValues(ControlsEnum.LIST);
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
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ActionsEnum.SAVE);
                                    GetFieldValues(ControlsEnum.LIST);
                                    SetFieldValues(ControlsEnum.LIST);
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
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ActionsEnum.SAVE);
                                    GetFieldValues(ControlsEnum.LIST);
                                    SetFieldValues(ControlsEnum.LIST);
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
                    #region Activate
                    // Do Action if click Activate Button
                    case ActionsEnum.ACTIVATE:
                        gvrTemplate = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        result = EmployeeTypeBL.UpdateEmployeeTypeStatus(Convert.ToInt32(((HiddenField)grdList.Rows[gvrTemplate.RowIndex].FindControl("hdfEmpTypePK")).Value), 1, currentUser.PKUser, string.Empty);
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
                        result = EmployeeTypeBL.UpdateEmployeeTypeStatus(Convert.ToInt32(((HiddenField)grdList.Rows[gvrTemplate.RowIndex].FindControl("hdfEmpTypePK")).Value), 0, currentUser.PKUser, string.Empty);
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
                    #region default
                    default:
                        break;
                    #endregion
                    #region FILTER
                    case ActionsEnum.FILTER:
                        uclPaging.CurrentPage = 0;
                        this.PageIndex = "1";
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ActionsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex)
                                          + "','" + Resources.ErpRes.Information + "');", true);
            }
        }
        #endregion

        #region Helper Methods
        private void InitializeComponent()
        {
            this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
            this.Init += new EventHandler(this.Page_Init);
        }

        private void PageActionHandler()
        {
            this.currentUser = (BusinessObject.User)Context.User.Identity;

            string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
            hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
            hdfCurrencyFormatWithComma.Value = "#" + currencysep + "#0.";
            hdfCurrencyFormat.Value = "#0.";
            for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
            {
                hdfCurrencyFormat.Value += "0";
                hdfCurrencyFormatWithComma.Value += "0";
            }

            if (!IsPostBack)
            {
                InitializeComponent();

                GetFieldValues(ControlsEnum.LIST);
                SetFieldValues(ControlsEnum.LIST);

                uclPaging.TotalPages = TotalPages;
                uclPaging.CurrentPage = 1;
                ddlOTTemplate.Enabled = false;
                //GetFieldValues(ControlsEnum.LOCATIONLIST);
                //SetFieldValues(ControlsEnum.LOCATIONLIST);
                GetFieldValues(ControlsEnum.WORKINGDAYS);
                SetFieldValues(ControlsEnum.WORKINGDAYS);
                GetFieldValues(ControlsEnum.LEAVETEMPLATE);
                SetFieldValues(ControlsEnum.LEAVETEMPLATE);
                GetFieldValues(ControlsEnum.OTTEMPLATE);
                SetFieldValues(ControlsEnum.OTTEMPLATE);
                GetFieldValues(ControlsEnum.WORKINGDAYSTYPE);
                SetFieldValues(ControlsEnum.WORKINGDAYSTYPE);
                PageIndex = CommonConstants.SELECT_VALUE_ONE;
            }
            //ConfigurationSettings();
        }

        private void EnableDisableButtons(int iTotalPages)
        {
            uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
        }
        #endregion

        #region Common UI Methods
        /// <summary>
        /// Getting Data From Db To Fields
        /// </summary>
        /// <param name="controlsEnum">Field</param>
        private void GetFieldValues(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                #region List
                case ControlsEnum.LIST:
                    pageData = EmployeeTypeBL.GetEmployeeTypeList(currentUser.SBUID, Convert.ToInt32(this.PageIndex), this.PageSize
                        , txtFilterName.Text.Trim(), txtFilterCode.Text.Trim());
                    break;
                #endregion
                #region Selected Employee Type
                case ControlsEnum.SELECTEDEMPTYPE:
                    selectedEmployeeType = EmployeeTypeBL.GetEmployeeTypeByID(this.CurrPK, (int)DbActiveStatus.ACTIVE);
                    break;
                #endregion
                #region Batch / Location
                case ControlsEnum.LOCATIONLIST:
                    if (this.CurrPK > 0 && selectedEmployeeType != null)
                        dummyPk = selectedEmployeeType.BranchOrLocation.IsNullOrEmptyOrWhitespace()
                                    ? 0
                                    : Convert.ToInt32(selectedEmployeeType.BranchOrLocation);
                    //  21    7  =>  HRMS   Locations
                    pageData = EmployeeTypeBL.GetBranchOrLocation(21, 7, this.currentUser.SBUID, dummyPk, (int)DbActiveStatus.ACTIVE);
                    break;
                #endregion
                #region Working Days
                case ControlsEnum.WORKINGDAYS:     
                     GetFieldValues(ControlsEnum.WEEKDAYS);
                     SetFieldValues(ControlsEnum.WEEKDAYS);
                    //_workingHoursList = new List<WorkingHours>();
                    //_workingHoursList.Add(new WorkingHours { WeekDay = 1, Hours = 0 });
                    //_workingHoursList.Add(new WorkingHours { WeekDay = 2, Hours = 0 });
                    //_workingHoursList.Add(new WorkingHours { WeekDay = 3, Hours = 0 });
                    //_workingHoursList.Add(new WorkingHours { WeekDay = 4, Hours = 0 });
                    //_workingHoursList.Add(new WorkingHours { WeekDay = 5, Hours = 0 });
                    //_workingHoursList.Add(new WorkingHours { WeekDay = 6, Hours = 0 });
                    //_workingHoursList.Add(new WorkingHours { WeekDay = 7, Hours = 0 });
                    if (this.CurrPK > 0)
                    {
                        if (selectedEmployeeType != null)
                        {
                            foreach (var item in selectedEmployeeType.WorkingHours)
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
                #endregion
                #region WEEKDAYS
                case ControlsEnum.WEEKDAYS:
                    dtResult = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "Week Days","WD");
                    break;
                #endregion
                #region Leave Template
                case ControlsEnum.LEAVETEMPLATE:
                    if (this.CurrPK > 0 && selectedEmployeeType != null)
                        dummyPk = !string.IsNullOrEmpty(selectedEmployeeType.LeaveTemplate) ? Convert.ToInt32(selectedEmployeeType.LeaveTemplate) : 0;
                    pageData = EmployeeTypeBL.GetLeaveTemplateList(this.currentUser.SBUID, dummyPk, (int)DbActiveStatus.ACTIVE);
                    break;
                #endregion
                #region OT Template
                case ControlsEnum.OTTEMPLATE:
                    if (this.CurrPK > 0 && selectedEmployeeType != null && selectedEmployeeType.OTAvailable == "1")
                        dummyPk = Convert.ToInt32(selectedEmployeeType.OTTemplate);
                    pageData = EmployeeTypeBL.GetOTTemplateList(this.currentUser.SBUID, dummyPk, (int)DbActiveStatus.ACTIVE);
                    break;
                #endregion
                #region Company
                case ControlsEnum.COMPANY:
                    pageData = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), 0, 0);
                    break;
                #endregion
                #region WORKING DAYS TYPE
                case ControlsEnum.WORKINGDAYSTYPE:
                  //  pageData = EmployeeTypeBL.GetWorkingDaysType(this.CurrPK, (int)DbActiveStatus.ALL, this.currentUser.SBUID, GetLocalResourceObject("ConfigType_WorkingDaysType").ToString(),null);
                    pageData = CommonBL.GetAppConfig(currentUser.SBUID, GetLocalResourceObject("ConfigType_WorkingDaysType").ToString());
                    break;
                #endregion
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
                    #region List
                    case ControlsEnum.LIST:
                        BindGrid(controlType);
                        break;
                    #endregion
                    #region Batch / Location
                    case ControlsEnum.LOCATIONLIST:
                        BindDropDown(ControlsEnum.LOCATIONLIST);
                        break;
                    #endregion
                    #region Selected Employee
                    case ControlsEnum.SELECTEDEMPTYPE:
                        GetUIValuesFromObject(ControlsEnum.SELECTEDEMPTYPE);
                        break;
                    #endregion
                    #region Working Days
                    case ControlsEnum.WORKINGDAYS:
                        BindGrid(ControlsEnum.WORKINGDAYS);
                        break;
                    #endregion
                    #region WEEKDAYS
                    case ControlsEnum.WEEKDAYS:
                        if (dtResult == null || dtResult.Rows.Count < 1)
                        {
                            _workingHoursList = null;
                        }
                        else
                        {
                            List<WorkingHours> weekDaysList = new List<WorkingHours>();
                            for (int i = 0; i < dtResult.Rows.Count; i++)
                            {
                                WorkingHours weekDay = new WorkingHours();
                                weekDay.WeekDay = Convert.ToInt32(dtResult.Rows[i]["CFG_VALUE"]);
                                weekDay.WeekDayTest = Convert.ToString(dtResult.Rows[i]["CFG_DATA"]);
                                weekDaysList.Add(weekDay);
                            }
                            _workingHoursList = weekDaysList;
                        }                        
                        break;
                    #endregion
                    #region Leave Template
                    case ControlsEnum.LEAVETEMPLATE:
                        BindDropDown(ControlsEnum.LEAVETEMPLATE);
                        break;
                    #endregion
                    #region OT Template
                    case ControlsEnum.OTTEMPLATE:
                        BindDropDown(ControlsEnum.OTTEMPLATE);
                        break;
                    #endregion
                    #region WORKING DAYS TYPE
                    case ControlsEnum.WORKINGDAYSTYPE:
                        BindDropDown(ControlsEnum.WORKINGDAYSTYPE);
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
        /// Used To Setting UI Components like TextBox
        /// </summary>
        /// <param name="controlsEnum"></param>
        private void GetUIValuesFromObject(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                #region Selected Employee
                case ControlsEnum.SELECTEDEMPTYPE:
                    BindDetailsView();
                    break;
                #endregion
                default:
                    break;
            }
        }

        private void BindDetailsView()
        {
            if (selectedEmployeeType != null)
            {
                try
                {
                    txtCode.Text = selectedEmployeeType.TypeCode.HtmlDecode();
                    txtName.Text = selectedEmployeeType.TypeName.HtmlDecode();

                    chkOTAvailable.Checked = selectedEmployeeType.OTAvailable == "1" ? true : false;
                    if (chkOTAvailable.Checked)
                    {
                        
                        //divOtTemplate.Style["display"] = "block";
                        rfvOTTemplate.Enabled = true;
                        ddlOTTemplate.Enabled = true;
                    }
                    else
                    {
                        
                        //divOtTemplate.Style["display"] = "none";
                        rfvOTTemplate.Enabled = false;
                        ddlOTTemplate.Enabled = false;
                    }

                    //ddlBatchOrLocation.SelectedValue = selectedEmployeeType.BranchOrLocation.ToString();
                    GetFieldValues(ControlsEnum.LEAVETEMPLATE);
                    SetFieldValues(ControlsEnum.LEAVETEMPLATE);
                    if (!string.IsNullOrEmpty(selectedEmployeeType.LeaveTemplate))
                        ddlLeaveTemplate.SelectedValue = selectedEmployeeType.LeaveTemplate;
                    else
                        ddlLeaveTemplate.SelectedValue = CommonConstants.SELECTVAL;

                    GetFieldValues(ControlsEnum.OTTEMPLATE);
                    SetFieldValues(ControlsEnum.OTTEMPLATE);
                    if (selectedEmployeeType.OTAvailable == "1")
                    {
                        ddlOTTemplate.SelectedValue = selectedEmployeeType.OTTemplate;
                    }
                    else
                    {
                        ddlOTTemplate.SelectedValue = CommonConstants.SELECTVAL;
                    }

                    GetFieldValues(ControlsEnum.WORKINGDAYSTYPE);
                    SetFieldValues(ControlsEnum.WORKINGDAYSTYPE);
                    ddlWorkingDayType.SelectedValue = selectedEmployeeType.WorkingDayType.ToString();
                    txtWorkingDays.Text = !string.IsNullOrEmpty(selectedEmployeeType.WorkingDays) ? selectedEmployeeType.WorkingDays : string.Empty;
                    if (Convert.ToInt32(ddlWorkingDayType.SelectedValue) == 1)
                        vrfWorkingDays.Enabled = true;
                    else
                        vrfWorkingDays.Enabled = false;
                    //Bind WorkingDatys
                    GetFieldValues(ControlsEnum.WORKINGDAYS);
                    SetFieldValues(ControlsEnum.WORKINGDAYS);

                    txtNormalWorkingHrs.Text =  selectedEmployeeType.NormalWorkingHrs.HtmlDecode();
                    txtOTRate.Text = GetFormattedCurrency(selectedEmployeeType.OTRate.HtmlDecode());
                    txtBreakTime.Text = selectedEmployeeType.BreakTime.HtmlDecode();

                    this.LastModifiedTime = selectedEmployeeType.LastModifiedDate;
                    ItemStatus = selectedEmployeeType.Active;
                    lblLastModifiedHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);

                    //chkActive.Checked = selectedEmployeeType.Active == 1 ? true : false; ;
                
                }
                catch (Exception ex)
                {
                    throw ex;
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

            DateTime dummyDateField;
            double dummyDoubleField;

            try
            {
                switch (mode)
                {
                    case ActionsEnum.SAVE:
                        EmployeeTypeBO retObj = new EmployeeTypeBO();

                        retObj.PK = this.CurrPK;

                        retObj.TypeCode = txtCode.Text.Trim().HtmlEncode();
                        retObj.TypeName = txtName.Text.Trim().HtmlEncode();

                        retObj.OTAvailable = chkOTAvailable.Checked ? "1" : "0";
                       
                        //retObj.BranchOrLocation = Convert.ToInt32(ddlBatchOrLocation.SelectedValue);

                        retObj.LeaveTemplate = Convert.ToInt32(ddlLeaveTemplate.SelectedValue) > 0 ? ddlLeaveTemplate.SelectedValue : null;
                        if (retObj.OTAvailable == "1")
                        {
                            retObj.OTTemplate = ddlOTTemplate.SelectedValue;
                        }
                        
                        retObj.WorkingDayType = Convert.ToInt16(ddlWorkingDayType.SelectedValue);
                        if (Convert.ToInt16(ddlWorkingDayType.SelectedValue) == 1 && !string.IsNullOrEmpty(txtWorkingDays.Text.Trim()))
                            retObj.WorkingDays = txtWorkingDays.Text.Trim();
                        else
                            retObj.WorkingDays = null;

                        retObj.WorkingHours = (List<WorkingHours>)SetUiValuesToObject(ActionsEnum.DETAIL);

                        retObj.NormalWorkingHrs = txtNormalWorkingHrs.Text.Trim().HtmlEncode();
                        retObj.OTRate = txtOTRate.Text.Trim() != string.Empty ? txtOTRate.Text.Trim().HtmlEncode() : CommonConstants.SELECT_VALUE_ZERO;

                        retObj.BreakTime = txtBreakTime.Text.Trim().HtmlEncode();

                        retObj.DeptPk = currentUser.CurrentDeptPK;
                        GetFieldValues(ControlsEnum.COMPANY);
                        retObj.Company = GetNullableInt(pageData.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()).Value;
                        retObj.BizUnit = this.currentUser.SBUID;
                        retObj.User = this.currentUser.PKUser;
                        retObj.LastModifiedDate = this.LastModifiedTime;
                        retObj.Active = (short)ItemStatus;
                       // retObj.Active = Convert.ToInt16(chkActive.Checked);
                        returnObj = retObj;
                        break;
                    case ActionsEnum.DETAIL:
                        List<WorkingHours> workingHrsList = null;

                        if (grdWorkingDays.Rows.Count > 0)
                        {
                            workingHrsList = new List<WorkingHours>();
                            foreach (GridViewRow row in grdWorkingDays.Rows)
                            {
                                double hrs = 0;
                                if (Double.TryParse(((TextBox)row.FindControl("txtHrs")).Text.Trim(), out dummyDoubleField))
                                    hrs = dummyDoubleField;
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
                                        Hours = hrs,
                                        Active = (int)CommonStatus.Active
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

        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                switch (Mode)
                {
                    case ActionsEnum.DEFAULT:
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    case ActionsEnum.EMPDOCDETAIL:
                    case ActionsEnum.EDIT:
                        GetFieldValues(ControlsEnum.SELECTEDEMPTYPE);
                        SetFieldValues(ControlsEnum.SELECTEDEMPTYPE);
                        break;
                    case ActionsEnum.NEW:
                        ResetForm(ActionsEnum.NEW);
                        EntryStatus = EntryStatus.NEWMODE;
                        txtOTRate.Text = GetFormattedCurrency(txtOTRate.Text);
                        ConfigurationSettings();
                        txtCode.Focus();
                        break;
                    case ActionsEnum.VIEW:
                        SetUIEditView(ActionsEnum.EDIT);
                        break;
                    case ActionsEnum.SAVE:
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

        private void BindGrid(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                #region List
                case ControlsEnum.LIST:
                    int rowCount = 0;
                    if (pageData != null)
                    {
                        if (pageData.Rows.Count > 0)
                        {
                            rowCount = Convert.ToInt32(pageData.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                        }
                        if (this.pageData.Rows.Count > 0)
                            this.TotalPages = Convert.ToInt32(pageData.AsEnumerable().FirstOrDefault().Field<long>("ROW_NO"));
                    }

                    uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                      (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                      (rowCount / this.PageSize) + 1;
                    PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);

                    grdList.DataSource = pageData;
                    grdList.DataBind();

                    uclPaging.Visible = true;
                    uclPaging.BindPager();
                    break;
                #endregion
                #region Working Days
                case ControlsEnum.WORKINGDAYS:
                    grdWorkingDays.DataSource = _workingHoursList;
                    grdWorkingDays.DataBind();
                    break;
                #endregion
                default:
                    break;
            }
        }

        private void BindDropDown(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                #region Batch / Location
                case ControlsEnum.LOCATIONLIST:
                    //ddlBatchOrLocation.Items.Clear();
                    //ddlBatchOrLocation.DataSource = pageData;
                    //ddlBatchOrLocation.DataTextField = GTIService.Constants.HRMS.Employee.Fields.ADM_CNST_TEXT;
                    //ddlBatchOrLocation.DataValueField = GTIService.Constants.HRMS.Employee.Fields.ADM_CNST_VALUE;
                    //ddlBatchOrLocation.DataBind();
                    //ddlBatchOrLocation.Items.HtmlDecode();
                    //ddlBatchOrLocation.Items.Insert(0, new ListItem(CommonConstants.ALL, CommonConstants.SELECT_ALL_VAL));
                    break;
                #endregion
                #region Leave Template
                case ControlsEnum.LEAVETEMPLATE:
                    ddlLeaveTemplate.Items.Clear();
                    ddlLeaveTemplate.DataSource = pageData;
                    ddlLeaveTemplate.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.LTE_NAME;
                    ddlLeaveTemplate.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.LTE_PK;
                    ddlLeaveTemplate.DataBind();
                    ddlLeaveTemplate.Items.HtmlDecode();
                    ddlLeaveTemplate.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region OT Template
                case ControlsEnum.OTTEMPLATE:
                    ddlOTTemplate.Items.Clear();
                    ddlOTTemplate.DataSource = pageData;
                    ddlOTTemplate.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.OTE_NAME;
                    ddlOTTemplate.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.OTE_PK;
                    ddlOTTemplate.DataBind();
                    ddlOTTemplate.Items.HtmlDecode();
                    ddlOTTemplate.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region WORKING DAYS TYPE
                case ControlsEnum.WORKINGDAYSTYPE:
                    ddlWorkingDayType.Items.Clear();
                    ddlWorkingDayType.DataSource = pageData;
                    ddlWorkingDayType.DataTextField = GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD;
                    ddlWorkingDayType.DataValueField = GTIService.Constants.Common.Common.CFG_VALUE_VALUE_FIELD;
                    ddlWorkingDayType.DataBind();
                    ddlWorkingDayType.Items.HtmlDecode();
                    ddlWorkingDayType.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
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
                    GetFieldValues(ControlsEnum.LEAVETEMPLATE);
                    SetFieldValues(ControlsEnum.LEAVETEMPLATE);
                    GetFieldValues(ControlsEnum.OTTEMPLATE);
                    SetFieldValues(ControlsEnum.OTTEMPLATE);
                    GetFieldValues(ControlsEnum.WORKINGDAYS);
                    SetFieldValues(ControlsEnum.WORKINGDAYS);
                    break;
                case ActionsEnum.LIST:
                    clearControls();
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    break;
                case ActionsEnum.NEW:
                    clearControls();
                    break;
                case ActionsEnum.CLEAR:
                    txtFilterCode.Text = string.Empty;
                    txtFilterName.Text = string.Empty;
                    break;
            }
        }

        private void clearControls()
        {
            txtFilterCode.Text = string.Empty;
            txtFilterName.Text = string.Empty;

            ItemStatus = 1;
            this.CurrPK = 0;
            this.selectedEmployeeType = null;

            txtCode.Text = string.Empty;
            txtName.Text = string.Empty;

            txtNormalWorkingHrs.Text = string.Empty;
            txtOTRate.Text = string.Empty;
            txtBreakTime.Text = string.Empty;

            chkOTAvailable.Checked = false;
            ddlOTTemplate.Enabled = false;
            //divOtTemplate.Style["display"] = "none";
            rfvOTTemplate.Enabled = false;

            //ddlBatchOrLocation.ClearSelection();
            ddlLeaveTemplate.ClearSelection();
            ddlOTTemplate.ClearSelection();

            ddlWorkingDayType.ClearSelection();
            vrfWorkingDays.Enabled = false;
            //uncomment this
            //grdWorkingDays.DataSource = null;
            //grdWorkingDays.DataBind();
        }

        private int? GetNullableInt(string str)
        {
            int result;
            return (int.TryParse(str, out result) ? (int?)result : null);
        }
        #endregion

        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            LIST,
            DETAIL,
            SELECTEDEMPTYPE,
            LOCATIONLIST,
            SELECTEDDOC,
            WORKINGDAYS,
            LEAVETEMPLATE,
            OTTEMPLATE,
            COMPANY,
            WORKINGDAYSTYPE,
            WEEKDAYS
        }

        private void ConfigurationSettings()
        {
            txtNormalWorkingHrs.Text = GetGlobalResourceObject("ConfigurationsRes", "NormalWorkingHours").ToString();
        }

        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }

        public string GetFormattedCurrencyWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithComma.Value);
        }
    }
}