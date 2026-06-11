using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using System.Data;
using ERPSMS_v01.UserControls;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using BusinessObject.CommonManagement;
using BusinessObject.HRMS.Admin.Masters;
using System.Xml;


namespace HRMS.Admin.Masters
{
    public partial class OTTemplate : ERP.Store.UI.MyBasePage
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
        private string PageIndexList
        {
            get
            {
                return (string)this.ViewState["PageIndexList"];
            }
            set
            {
                this.ViewState["PageIndexList"] = value;
            }
        }

        /// <summary>
        /// Current PK
        /// </summary>
        private int CurrPK
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrPK] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrPK];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrPK] = value;
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
        /// OTTemplateViewState
        /// </summary>
        private OTTemplateBO.OTTemplate OTTemplateViewState
        {
            get
            {
                return ViewState["OTTemplateViewState"] == null ? new OTTemplateBO.OTTemplate() : (OTTemplateBO.OTTemplate)ViewState["OTTemplateViewState"];
            }
            set
            {
                ViewState["OTTemplateViewState"] = value;
            }
        }

        /// <summary>
        /// WeekDaysViewState
        /// </summary>
        private List<OTTemplateBO.WeekDays> WeekDaysListViewState
        {
            get
            {
                return ViewState["WeekDaysListViewState"] == null ? new List<OTTemplateBO.WeekDays>() : (List<OTTemplateBO.WeekDays>)ViewState["WeekDaysListViewState"];
            }
            set
            {
                ViewState["WeekDaysListViewState"] = value;
            }
        }

        /// <summary>
        /// CurrentWeekDayViewState
        /// </summary>
        private int? CurrentWeekDayViewState
        {
            get
            {
                return ViewState["CurrentWeekDayViewState"] == null ? null : (int?)ViewState["CurrentWeekDayViewState"];
            }
            set
            {
                ViewState["CurrentWeekDayViewState"] = value;
            }
        }

        #endregion
        private BusinessObject.User currentUser;
        private DataTable dtResult;
        private DataTable dtCompany;
        private DataSet dsPageData;
        #endregion

        #region PageEvents
        /// <summary>
        /// PageInit Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
        }

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

        #region Page_PreRender
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
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            else if (EntryStatus == EntryStatus.NEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            else if (EntryStatus == EntryStatus.ENTRYMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch(1);});", true);
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
                ucFormulaMaster.AfterApply += new EventHandler(ucFormulaMaster_AfterApply);
                InitializeComponent();
                if (!IsPostBack)
                {
                    this.PageIndexList = "1";
                    uclPaging.CurrentPage = 1;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    GetFieldValues(ControlsEnum.WEEKDAYS);
                    SetFieldValues(ControlsEnum.WEEKDAYS);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #endregion

        // Formula Popup After Apply
        void ucFormulaMaster_AfterApply(object sender, EventArgs e)
        {
            if (CurrentWeekDayViewState.HasValue)
            {
                List<OTTemplateBO.WeekDays> tempList = WeekDaysListViewState;
                OTTemplateBO.WeekDays day = tempList
                    .Where(x => x.OTD_WEEK_DAY == CurrentWeekDayViewState.Value)
                    .Single();
                day.OTD_FORMULA_TEXT = ucFormulaMaster.FormulaText;
                day.OTD_FORMULA = ucFormulaMaster.FormulaValue;
                WeekDaysListViewState = tempList;
                BindGrid(ControlsEnum.WEEKDAYS);
                CurrentWeekDayViewState = null;
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
        }

        #region InitializeComponent
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
        #endregion

        #region Custom Pager Control Navigated Event
        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        {
            try
            {
                PgerControlNew pagerControl = (PgerControlNew)sender;
                string senderId = pagerControl.ID;
                switch (e.Action)
                {
                    case NavigationEnum.PAGECHANGE:
                        pagerControl.CurrentPage = e.CurrentPage;
                        break;
                    case NavigationEnum.FIRST:
                        if (e.CurrentPage > 1)
                            pagerControl.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        if (e.CurrentPage <= e.TotalPages)
                            pagerControl.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        if (e.CurrentPage <= e.TotalPages)
                            pagerControl.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        if (e.CurrentPage > 1)
                            pagerControl.CurrentPage--;
                        break;
                }
                if (senderId == "uclPaging")
                {
                    PageIndexList = uclPaging.CurrentPage.ToString();
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    EntryStatus = EntryStatus.LISTMODE;
                    EnableDisableButtons(e.TotalPages, "uclPaging");
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
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
            // XmlDocument xmlDoc;
            GridViewRow gvrOTTemplate;
            bool bIsChecked = false;
            try
            {
                int? result = null;
                #region Getting Command Action
                ActionsEnum commonActions = ActionsEnum.UNKNOWN;
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
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    //if ((((DropDownList)sender).ID == "ddlLeaveType"))
                    //{
                    //    commonActions = ActionsEnum.CHANGETYPE;
                    //}
                }

                #endregion
                switch (commonActions)
                {

                    #region SAVE
                    case ActionsEnum.SAVE:
                        OTTemplateViewState = (OTTemplateBO.OTTemplate)SetUIValuesToObject(ControlsEnum.OTTEMPLATE);
                        XmlDocument xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(OTTemplateViewState);
                        result = BusinessLogic.HRMS.Admin.Masters.OTTemplateBL.SaveOTTemplate(xmlDoc.InnerXml);
                        if (result > 0)
                        {
                            ResetForm(ControlsEnum.CLEAR);
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            this.EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = GetLocalResourceObject("Msg_SavSuccess").ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.OTTemplate);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.OTTemplate + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.OTTemplate + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.OTTemplate + " " + Resources.Messages.ItemCodeAlreadyExists;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.REFNOEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.OTTemplate + " " + Resources.Messages.ItemNameAlreadyExists;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.OTTemplate);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region LIST, CANCEL
                    case ActionsEnum.LIST:
                    case ActionsEnum.CANCEL:
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region EDIT, DETAIL
                    case ActionsEnum.EDIT:
                    case ActionsEnum.DETAIL:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                ResetForm(ControlsEnum.CLEAR);
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfTemplatePkListPage")).Value);
                                GetFieldValues(ControlsEnum.OTTEMPLATE);
                                SetFieldValues(ControlsEnum.OTTEMPLATE);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            EntryStatus = EntryStatus.EDITMODE;
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region NEW
                    case ActionsEnum.NEW:
                        EntryStatus = EntryStatus.NEWMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.WEEKDAYS);
                        SetFieldValues(ControlsEnum.WEEKDAYS);
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = BusinessLogic.HRMS.Admin.Masters.OTTemplateBL.DeleteOTTemplate(CurrPK, Convert.ToString(this.LastModifiedTime));
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.OTTemplate);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            if (grdList.Rows.Count == 1 && Convert.ToInt32(PageIndexList) > 1)
                            {
                                PageIndexList = Convert.ToString(Convert.ToInt32(PageIndexList) - 1);
                            }
                            ActionHandler(lnkList, EventArgs.Empty);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.OTTemplate;
                                litErrorMsg.Text += " " + GetGlobalResourceObject("Messages", "UsedInAnotherPlace").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.OTTemplate + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.OTTemplate + " " +
                                    GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.OTTemplate + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.OTTemplate);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region FILTER
                    case ActionsEnum.FILTER:
                        uclPaging.CurrentPage = 0;
                        this.PageIndexList = "1";
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region Activate
                    // Do Action if click Activate Button
                    case ActionsEnum.ACTIVATE:
                        gvrOTTemplate = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        result = BusinessLogic.HRMS.Admin.Masters.OTTemplateBL.UpdateOTTemplateStatus(Convert.ToInt32(((HiddenField)grdList.Rows[gvrOTTemplate.RowIndex].FindControl("hdfTemplatePkListPage")).Value), 1, currentUser.PKUser, ((HiddenField)grdList.Rows[gvrOTTemplate.RowIndex].FindControl("hdfListOTE_MOD_DT")).Value);
                        if (result > 0)
                        {
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Activate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.OTTemplate.ToString());
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
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.OTTemplate.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.OTTemplate.ToString());
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
                        gvrOTTemplate = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        result = BusinessLogic.HRMS.Admin.Masters.OTTemplateBL.UpdateOTTemplateStatus(Convert.ToInt32(((HiddenField)grdList.Rows[gvrOTTemplate.RowIndex].FindControl("hdfTemplatePkListPage")).Value), 0, currentUser.PKUser, ((HiddenField)grdList.Rows[gvrOTTemplate.RowIndex].FindControl("hdfListOTE_MOD_DT")).Value);
                        if (result > 0)
                        {
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_InActivate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.OTTemplate.ToString());
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
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.OTTemplate.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.OTTemplate.ToString());
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }

        }

        /// <summary>ActionHandler
        /// Action Handler For GridViewCommandEventArgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
            GridView senderGridView = (GridView)sender;
            int slNo;
            if (senderGridView.ID == "grdWeekDays")
            {
                if (e.CommandName == "EDIT_ACTION")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfOTD_FORMULA = row.FindControl("hdfOTD_FORMULA") as HiddenField;
                    Label lblFormula = row.FindControl("lblFormula") as Label;
                    HiddenField hdfOTD_WEEK_DAY = row.FindControl("hdfOTD_WEEK_DAY") as HiddenField;
                    Label lblWeekDay = row.FindControl("lblWeekDay") as Label;
                    ucFormulaMaster.SetData(lblFormula.Text, lblWeekDay.Text);                   
                    CurrentWeekDayViewState = GetNullableInt(hdfOTD_WEEK_DAY.Value).Value;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpFormula]','Formula','660','200');", true);
                }
            }
        }
        #endregion

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            BusinessObject.GridPrams gridParam;
            try
            {
                switch (type)
                {
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), 0, 0);
                        break;
                    #endregion
                    #region LIST
                    case ControlsEnum.LIST:
                        gridParam = new BusinessObject.GridPrams();
                        //gridParam.SearchBy = string.Empty;
                        gridParam.SearchValue = txtTemplateNameListPage.Text.Trim();
                        gridParam.PageNumber = GetNullableInt(PageIndexList).HasValue ? GetNullableInt(PageIndexList).Value : 1; //uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        gridParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        // gridParam.Fields = GTIService.Constants.DirectStockTransfer.Parameters.GridParmeters;
                        //"[GRH_PK],[GRH_NO],[GRH_DATE],[GRH_STATUS],[GRH_STATUS_TEXT],[REF_ID],[DPT_NAME],[GRH_VENDOR_TEXT],[GRH_PO_NO],[GRH_VND_REF_NO]";
                        // gridParam.SortBy = GTIService.Constants.DirectStockTransfer.Fields.GRH_PK_SortBy;
                        //  gridParam.SortDirection = "DESC";
                        //gridParam.FromDate = txtFromDate.Text;
                        //gridParam.ToDate = txtToDate.Text;
                        //gridParam.FilterStatus = ddlStatus.SelectedValue;                    

                        dtResult = BusinessLogic.HRMS.Admin.Masters.OTTemplateBL.GetOTTemplateListPage(gridParam, currentUser.SBUID, txtFilterCode.Text.Trim());
                        break;
                    #endregion
                    #region OTTEMPLATE
                    case ControlsEnum.OTTEMPLATE:
                        string xmlData = BusinessLogic.HRMS.Admin.Masters.OTTemplateBL.GetOTTemplate(CurrPK);
                        OTTemplateBO.OTTemplate tempOTTemplate;
                        if (xmlData == "<Root/>")
                        {
                            tempOTTemplate = new OTTemplateBO.OTTemplate();
                        }
                        else
                        {
                            tempOTTemplate = CommonFunctions.XmlDeserialize<OTTemplateBO.OTTemplate>(xmlData);
                        }
                        OTTemplateViewState = tempOTTemplate;
                        GetFieldValues(ControlsEnum.WEEKDAYS);
                        SetFieldValues(ControlsEnum.WEEKDAYS);

                        List<OTTemplateBO.WeekDays> lstWeeks = WeekDaysListViewState;
                        foreach (var item in OTTemplateViewState.WeekDaysList)
                        {
                            OTTemplateBO.WeekDays day = lstWeeks
                                 .Where(x => x.OTD_WEEK_DAY == item.OTD_WEEK_DAY)
                                 .Single();
                            day.OTD_PK = item.OTD_PK;
                            day.OTD_FORMULA = item.OTD_FORMULA;
                            day.OTD_FORMULA_TEXT = item.OTD_FORMULA_TEXT;
                        }
                        WeekDaysListViewState = lstWeeks;
                        break;
                    #endregion
                    #region WEEKDAYS
                    case ControlsEnum.WEEKDAYS:
                        dtResult = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "Week Days");
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
                    #region LIST
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region LEAVETEMPLATE
                    case ControlsEnum.OTTEMPLATE:
                        GetUIValuesFromObject(ControlsEnum.OTTEMPLATE);
                        break;
                    #endregion
                    #region WEEKDAYS
                    case ControlsEnum.WEEKDAYS:
                        if (dtResult == null || dtResult.Rows.Count < 1)
                        {
                            WeekDaysListViewState = null;
                        }
                        else
                        {
                            List<OTTemplateBO.WeekDays> weekDaysList = new List<OTTemplateBO.WeekDays>();
                            for (int i = 0; i < dtResult.Rows.Count; i++)
                            {
                                OTTemplateBO.WeekDays weekDay = new OTTemplateBO.WeekDays();
                                weekDay.OTD_WEEK_DAY = Convert.ToInt32(dtResult.Rows[i]["CFG_VALUE"]);
                                weekDay.OTD_WEEK_DAY_TEXT = Convert.ToString(dtResult.Rows[i]["CFG_DATA"]);
                                weekDaysList.Add(weekDay);
                            }
                            WeekDaysListViewState = weekDaysList;
                        }
                        BindGrid(ControlsEnum.WEEKDAYS);
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
            //switch (controlType)
            //{
            //    #region CLEAR
            //    case ControlsEnum.CLEAR:
            //        // I keep the code just as a template for dropdown binding...
            //        //ddlPayClassification.Items.Clear();
            //        //ddlPayClassification.DataSource = dtResult;
            //        //ddlPayClassification.DataTextField = GTIService.Constants.Common.Fields.ADM_CFG_TEXT;
            //        //ddlPayClassification.DataValueField = GTIService.Constants.Common.Fields.ADM_CFG_VALUE;
            //        //ddlPayClassification.DataBind();
            //        //ddlPayClassification.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
            //        //ddlPayClassification.Items.HtmlDecode();
            //        break;
            //    #endregion
            //}
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
                switch (controlType)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        int rowCount = 0;
                        int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        if (dtResult.Rows.Count > 0)
                        {
                            rowCount = Convert.ToInt32(dtResult.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                        }
                        uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                      (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                      (rowCount / pageSize) + 1;
                        PageIndexList = PageIndexList == null ? CommonConstants.SELECT_VALUE_ONE : PageIndexList;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndexList);
                        grdList.DataSource = dtResult;
                        grdList.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                        break;
                    #endregion
                    case ControlsEnum.WEEKDAYS:
                        grdWeekDays.DataSource = WeekDaysListViewState;
                        grdWeekDays.DataBind();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region "GetUIValuesFromObject"
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region OTTEMPLATE
                    case ControlsEnum.OTTEMPLATE:
                        txtTemplateCode.Text = OTTemplateViewState.OTE_CODE;
                        txtTemplateName.Text = OTTemplateViewState.OTE_NAME;
                        txtDescription.Text = OTTemplateViewState.OTE_DESC;
                        LastModifiedTime = OTTemplateViewState.LAST_MOD_DT;
                        chkActive.Checked = OTTemplateViewState.ACTIVE == 1 ? true : false;
                        BindGrid(ControlsEnum.WEEKDAYS);
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

        #region SetUIValuesToObject
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            object returnObject = new object();
            switch (controlType)
            {
                #region OTTEMPLATE
                case ControlsEnum.OTTEMPLATE:
                    OTTemplateBO.OTTemplate tempOTTemplate = OTTemplateViewState;
                    tempOTTemplate.OTE_PK = (tempOTTemplate.OTE_PK == null ? 0 : tempOTTemplate.OTE_PK);
                    tempOTTemplate.OTE_CODE = txtTemplateCode.Text.Trim().HtmlEncode();
                    tempOTTemplate.OTE_NAME = txtTemplateName.Text.Trim().HtmlEncode();
                    tempOTTemplate.OTE_DESC = txtDescription.Text.HtmlEncode();
                    tempOTTemplate.OTE_DEPT = currentUser.CurrentDeptPK;
                    GetFieldValues(ControlsEnum.COMPANY);
                    tempOTTemplate.OTE_COMPANY = GetNullableInt(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()).Value;
                    tempOTTemplate.BIZUNIT_PK = currentUser.SBUID;
                    tempOTTemplate.ACTIVE = Convert.ToInt32(chkActive.Checked == true ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO));
                    tempOTTemplate.USER_PK = currentUser.PKUser;
                    tempOTTemplate.LAST_MOD_DT = LastModifiedTime;
                    List<OTTemplateBO.WeekDays> tempWeekDays = new List<OTTemplateBO.WeekDays>();
                    foreach (var item in WeekDaysListViewState)
                    {
                        if (!string.IsNullOrWhiteSpace(item.OTD_FORMULA))
                        {
                            OTTemplateBO.WeekDays temp = new OTTemplateBO.WeekDays();
                            temp.OTD_WEEK_DAY = item.OTD_WEEK_DAY;
                            temp.OTD_WEEK_DAY_TEXT = item.OTD_WEEK_DAY_TEXT;
                            temp.OTD_PK = item.OTD_PK;
                            temp.OTD_FORMULA = item.OTD_FORMULA;
                            temp.OTD_FORMULA_TEXT = item.OTD_FORMULA_TEXT;
                            tempWeekDays.Add(temp);
                        }
                    }
                    tempOTTemplate.WeekDaysList = tempWeekDays;
                    returnObject = tempOTTemplate;
                    break;
                #endregion
            }
            return returnObject;
        }
        #endregion

        #region ResetForm
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region CLEAR
                case ControlsEnum.CLEAR:
                    txtTemplateCode.Text = txtTemplateName.Text = txtTemplateNameListPage.Text = txtFilterCode.Text = string.Empty;
                    txtTemplateCode.Focus();
                    txtDescription.Text = string.Empty;
                    chkActive.Checked = true;
                    OTTemplateViewState = null;
                    WeekDaysListViewState = null;
                    CurrentWeekDayViewState = null;
                    CurrPK = 0;
                    PageIndexList = "1";
                    break;
                #endregion
                #region CLEARSEARCH
                case ControlsEnum.CLEARSEARCH:
                    txtTemplateNameListPage.Text = string.Empty;
                    txtFilterCode.Text = string.Empty;
                    break;
                #endregion

                //#region POPUPCLEAR
                //case ControlsEnum.POPUPCLEAR:
                //    //ucFormulaMaster.FormulaText = ucFormulaMaster.FormulaValue = string.Empty;
                //    //ucFormulaMaster.ResetForm();
                //    break;
                //#endregion
            }
        }
        #endregion

        #region EnableDisableButtons
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages, string pagerId)
        {
            if (pagerId == "uclPaging")
            {
                // Should we disable the first link
                uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
                // Should we disable the previous link
                uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
                // Should we enable the next link
                uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
                // Should we enable the last link
                uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            }
        }
        #endregion

        #region UtitlityMethods
        private int? GetNullableInt(string str)
        {
            int result;
            return (int.TryParse(str, out result) ? (int?)result : null);
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            // POPUPCLEAR,
            CLEAR,
            LIST,
            COMPANY,
            CLEARSEARCH,
            WEEKDAYS,
            OTTEMPLATE
        }
        #endregion

    }
}
