using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPSMS_v01.UserControls;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject;
using System.Data;
using BusinessLogic.Finance;
using BusinessObject.CommonManagement;
using BusinessObject.Finance;
namespace ERPSMS_v01.Finance
{
    public partial class COAFinGrpMap : ERP.Store.UI.MyBasePage // ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region  Properties
        public int CurrPK
        {
            get
            {
                return Convert.ToInt32(Session[ERP.Utilities.SessionStrings.CurrentPK]);
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.CurrentPK] = value;
            }
        }
        private string PageIndex
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.PageIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndex] = value;
            }
        }
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
        #region  Variables
        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        User currentUser;
        DataTable dtCOAFinGrpList;
        DataTable dtFinReport;
        DataTable dtFinGroup;
        DataTable dtFilterCOAType;
        DataTable dtFilterFinGroup;
        COAFinGrpMapBO COAFinGrpMapBOObj;
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
            PageActionHandler();
        }
        /// <summary>
        /// PageInit Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
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
                InitializeComponent();
                if (!IsPostBack)
                {
                    uclPaging.CurrentPage = 1;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    GetFieldValues(ControlsEnum.FINREPORT);
                    SetFieldValues(ControlsEnum.FINREPORT);
                    EntryStatus = EntryStatus.LISTMODE;
                    ddlFinGroup.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    if (ddlFinReport.Items.Count > 0)
                    {
                        GetFieldValues(ControlsEnum.FINGROUP);
                        SetFieldValues(ControlsEnum.FINGROUP);
                    }
                    GetFieldValues(ControlsEnum.FILTERCOATYPE);//Filter Type
                    SetFieldValues(ControlsEnum.FILTERCOATYPE);//Filter Type
                    SetFieldValues(ControlsEnum.FILTERFINREPORT);//Filter Fin Report
                    ddlFilterFinGroup.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));//Filter Fin Group
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
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
                    #region List
                    case ControlsEnum.LIST:
                        dtCOAFinGrpList = COAFinGrpMapBL.GetCOAFinGrpList(0, 0, 0, 0, 0, currentUser.SBUID);
                        break;
                    #endregion
                    #region Fin Report
                    case ControlsEnum.FINREPORT:
                        dtFinReport = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "FIN REPORT TEMPLATE");
                        break;
                    #endregion
                    #region Fin Group
                    case ControlsEnum.FINGROUP:
                        int finReportPk = 0;
                        Int32.TryParse(ddlFinReport.SelectedValue, out finReportPk);
                        dtFinGroup = BusinessLogic.Finance.Administration.Masters.FinReportCfgBL
                            .GetKVFinReportCfg(0, Convert.ToByte(DbActiveStatus.ALL), Convert.ToByte(finReportPk), 0, currentUser.SBUID, null, null, null, null, null);
                        break;
                    #endregion
                    #region Filter COA Type
                    case ControlsEnum.FILTERCOATYPE:
                        dtFilterCOAType = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "COA TYPE");
                        break;
                    #endregion
                    #region Filter Fin Group
                    case ControlsEnum.FILTERFINGROUP:
                        int filterFinReportPk = 0;
                        Int32.TryParse(ddlFilterFinReport.SelectedValue, out filterFinReportPk);
                        dtFilterFinGroup = null;
                        if (filterFinReportPk > 0)
                            dtFilterFinGroup = BusinessLogic.Finance.Administration.Masters.FinReportCfgBL
                                .GetKVFinReportCfg(0, Convert.ToByte(DbActiveStatus.ALL), Convert.ToByte(filterFinReportPk), null, currentUser.SBUID, null, null, null, null, null);
                        break;
                    #endregion
                    #region Search
                    case ControlsEnum.SEARCH:
                        dtCOAFinGrpList = COAFinGrpMapBL.GetCOAFinGrpList(ddlFilterType.SelectedIndex > 0 ? Convert.ToInt32(ddlFilterType.SelectedItem.Value) : 0
                            , ddlFilterFinGroup.SelectedIndex > 0 ? Convert.ToInt32(ddlFilterFinGroup.SelectedItem.Value) : 0
                            , ddlFilterFinReport.SelectedIndex > 0 ? Convert.ToInt32(ddlFilterFinReport.SelectedItem.Value) : 0
                            , Convert.ToInt32(ddlFilterMapped.SelectedItem.Value),0, currentUser.SBUID);
                        break;
                    #endregion
                }
            }
            catch (Exception ex) { throw ex; }
            finally { }
        }
        #endregion
        #region Set Field Values
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region List
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region Fin Report
                    case ControlsEnum.FINREPORT:
                        BindDropDown(ControlsEnum.FINREPORT);
                        break;
                    #endregion
                    #region Fin Group
                    case ControlsEnum.FINGROUP:
                        BindDropDown(ControlsEnum.FINGROUP);
                        break;
                    #endregion
                    #region Filter COA Type
                    case ControlsEnum.FILTERCOATYPE:
                        BindDropDown(ControlsEnum.FILTERCOATYPE);
                        break;
                    #endregion
                    #region Filter Fin Report
                    case ControlsEnum.FILTERFINREPORT:
                        BindDropDown(ControlsEnum.FILTERFINREPORT);
                        break;
                    #endregion
                    #region Filter Fin Group
                    case ControlsEnum.FILTERFINGROUP:
                        BindDropDown(ControlsEnum.FILTERFINGROUP);
                        break;
                    #endregion
                }
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
        #region Helper Methods
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region Save
                    case ControlsEnum.SAVE:
                        COAFinGrpMapBOObj = new COAFinGrpMapBO();
                        COAFinGrpMapBOObj.RTC_PK = Convert.ToInt32(ddlFinGroup.SelectedItem.Value);
                        COAFinGrpMapBOObj.USER_PK = currentUser.PKUser;
                        if (grdAccounts.Rows.Count > 0)
                        {
                            CheckBox chkSelect;
                            COAFinGrpMapBOObj.COAFinGrpMapDetailBO = new List<COAFinGrpMapDetailBO>();
                            foreach (GridViewRow gvrFinGroup in grdAccounts.Rows)
                            {
                                chkSelect = (CheckBox)gvrFinGroup.FindControl("chkSelect");
                                if (chkSelect.Checked == true)
                                {
                                    COAFinGrpMapDetailBO COAFinGrpMapDetailObj = new COAFinGrpMapDetailBO();
                                    COAFinGrpMapDetailObj.COA_PK = Convert.ToInt32(((HiddenField)gvrFinGroup.FindControl("hdfPk")).Value);
                                    COAFinGrpMapBOObj.COAFinGrpMapDetailBO.Add(COAFinGrpMapDetailObj);
                                }
                            }
                        }
                        retObject = COAFinGrpMapBOObj;
                        break;
                    #endregion
                }
                return retObject;
            }
            catch { throw; }
            finally { }
        }
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType) { }
            }
            catch (Exception ex) { throw ex; }
        }
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region Fin Report
                case ControlsEnum.FINREPORT:
                    if (dtFinReport != null)
                    {
                        ddlFinReport.Items.Clear();
                        ddlFinReport.DataSource = CommonFunctions.HtmlDecodeDataTable(dtFinReport, Resources.DataFieldRes.cfgData);
                        ddlFinReport.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlFinReport.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlFinReport.DataBind();
                    }
                    break;
                #endregion
                #region Fin Group
                case ControlsEnum.FINGROUP:
                    ddlFinGroup.Items.Clear();
                    if (dtFinGroup != null)
                    {
                        ddlFinGroup.DataSource = CommonFunctions.HtmlDecodeDataTable(dtFinGroup, Resources.DataFieldRes.RTC_NAME);
                        ddlFinGroup.DataTextField = Resources.DataFieldRes.RTC_NAME;
                        ddlFinGroup.DataValueField = Resources.DataFieldRes.RTC_PK;
                        ddlFinGroup.DataBind();
                    }
                    ddlFinGroup.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Filter COA Type
                case ControlsEnum.FILTERCOATYPE:
                    ddlFilterType.Items.Clear();
                    if (dtFilterCOAType != null)
                    {
                        ddlFilterType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtFilterCOAType, Resources.DataFieldRes.cfgData);
                        ddlFilterType.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlFilterType.DataValueField = Resources.DataFieldRes.ConfigPK;
                        ddlFilterType.DataBind();
                    }
                    ddlFilterType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Filter Fin Report
                case ControlsEnum.FILTERFINREPORT:
                    ddlFilterFinReport.Items.Clear();
                    if (dtFinReport != null)
                    {
                        ddlFilterFinReport.DataSource = CommonFunctions.HtmlDecodeDataTable(dtFinReport, Resources.DataFieldRes.cfgData);
                        ddlFilterFinReport.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlFilterFinReport.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlFilterFinReport.DataBind();
                    }
                    ddlFilterFinReport.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Filter Fin Group
                case ControlsEnum.FILTERFINGROUP:
                    ddlFilterFinGroup.Items.Clear();
                    if (dtFilterFinGroup != null)
                    {
                        ddlFilterFinGroup.DataSource = CommonFunctions.HtmlDecodeDataTable(dtFilterFinGroup, Resources.DataFieldRes.RTC_NAME);
                        ddlFilterFinGroup.DataTextField = Resources.DataFieldRes.RTC_NAME;
                        ddlFilterFinGroup.DataValueField = Resources.DataFieldRes.RTC_PK;
                        ddlFilterFinGroup.DataBind();
                    }
                    ddlFilterFinGroup.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
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
                    #region List
                    case ControlsEnum.LIST:
                        grdAccounts.DataSource = null;
                        if (dtCOAFinGrpList.Rows.Count > 0)
                            grdAccounts.DataSource = dtCOAFinGrpList;
                        grdAccounts.DataBind();
                        break;
                    #endregion
                }
            }
            catch (Exception ex) { throw ex; }
        }
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region Clear
                case ControlsEnum.CLEAR:
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    GetFieldValues(ControlsEnum.FINREPORT);
                    SetFieldValues(ControlsEnum.FINREPORT);
                    hdfShowFilter.Value = string.Empty;
                    EntryStatus = EntryStatus.LISTMODE;
                    ddlFinGroup.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    if (ddlFinReport.Items.Count > 0)
                    {
                        GetFieldValues(ControlsEnum.FINGROUP);
                        SetFieldValues(ControlsEnum.FINGROUP);
                    }
                    break;
                #endregion
                #region Clear Search
                case ControlsEnum.CLEARSEARCH:
                    ddlFilterType.SelectedIndex = -1;
                    ddlFilterFinReport.SelectedIndex = -1;
                    ddlFilterFinGroup.Items.Clear();
                    ddlFilterFinGroup.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    ddlFilterFinGroup.SelectedIndex = -1;
                    ddlFilterMapped.SelectedIndex = -1;
                    hdfShowFilter.Value = string.Empty;
                    break;
                #endregion
            }
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
        #region EnableDisableButtons
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages, string pagerId)
        {
            if (pagerId == "uclPaging")
            {
                uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the first link
                uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the previous link
                uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false; // Should we enable the next link
                uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;// Should we enable the last link
            }
        }
        #endregion
        #endregion
        #region ActionHandler
        /// <summary>
        /// Button actions events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;
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
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlFinReport") { commonActions = ActionsEnum.SELECTEDINDEXCHANGED; }
                    if (((DropDownList)sender).ID == "ddlFilterFinReport") { commonActions = ActionsEnum.FILTER; }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(CheckBox))) { }
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox))) { }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton))) { }
                switch (commonActions)
                {
                    #region Fin Report Change
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        GetFieldValues(ControlsEnum.FINGROUP);
                        SetFieldValues(ControlsEnum.FINGROUP);
                        break;
                    #endregion
                    #region Save
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            bool IsAnySelected = false;
                            if (grdAccounts.Rows.Count > 0)
                            {
                                CheckBox chkSelect;
                                foreach (GridViewRow gvrFinGroup in grdAccounts.Rows)
                                {
                                    chkSelect = (CheckBox)gvrFinGroup.FindControl("chkSelect");
                                    if (chkSelect != null)
                                    {
                                        if (chkSelect.Checked == true)
                                        {
                                            IsAnySelected = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (IsAnySelected)
                            {
                                COAFinGrpMapBOObj = new COAFinGrpMapBO();
                                COAFinGrpMapBOObj = (COAFinGrpMapBO)SetUIValuesToObject(ControlsEnum.SAVE);
                                if (COAFinGrpMapBOObj != null)
                                {
                                    result = COAFinGrpMapBL.SaveCOAFinGrp(COAFinGrpMapBOObj);
                                    if (result >= 0)
                                    {
                                        ResetForm(ControlsEnum.CLEAR);
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.COAFinGrpMap);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg"
                                            , "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else
                                    {
                                        #region Error Messages
                                        if (result == (int)DbSaveStatus.SQLERROR)
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.COAFinGrpMap + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.COAFinGrpMap + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.REFNOEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.COAFinGrpMap + " " + GetLocalResourceObject("RefNoExist").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.COAFinGrpMap);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        #endregion
                                    }
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_Account").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region Search
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.SEARCH);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region Clear Search
                    case ActionsEnum.CLEARSEARCH:
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region Filter Fin Report Change
                    case ActionsEnum.FILTER:
                        GetFieldValues(ControlsEnum.FILTERFINGROUP);
                        SetFieldValues(ControlsEnum.FILTERFINGROUP);
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
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try { }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        protected void ActionHandler(object sender, GridViewPageEventArgs e) { }
        protected void ActionHandler(object sender, GridViewSortEventArgs e) { }
        #endregion
        #endregion
        #region Pager Methods + Init
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
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
                    PageIndex = uclPaging.CurrentPage.ToString();
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    EntryStatus = EntryStatus.LISTMODE;
                    EnableDisableButtons(e.TotalPages, "uclPaging");
                }
            }
            catch (Exception ex) { }
        }
        #endregion
        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            LIST,
            FINREPORT,
            FINGROUP,
            SAVE,
            CLEAR,
            FILTERCOATYPE,
            FILTERFINREPORT,
            FILTERFINGROUP,
            SEARCH,
            CLEARSEARCH
        }
        #endregion
    }
}