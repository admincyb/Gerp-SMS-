using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using ERP.Utilities;
using System.Data;
using BusinessObject.AccountManagement;
using BusinessObject.CommonManagement;
using BusinessObject.HRMS.Admin.Masters;
using ERPSMS_v01.UserControls;

namespace HRMS.Admin.Masters
{
    public partial class LeaveTemplate : ERP.Store.UI.MyBasePage
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

        private int PageSize
        {
            get
            {
                return Convert.ToInt32(GetLocalResourceObject("PageSize").ToString());
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
        /// LeaveTemplateViewState
        /// </summary>
        private LeaveTemplateBO.LeaveTemplate LeaveTemplateViewState
        {
            get
            {
                return ViewState["LeaveTemplateViewState"] == null ? new LeaveTemplateBO.LeaveTemplate() : (LeaveTemplateBO.LeaveTemplate)ViewState["LeaveTemplateViewState"];
            }
            set
            {
                ViewState["LeaveTemplateViewState"] = value;
            }
        }
        #endregion
        private BusinessObject.User currentUser;
        private DataTable dtResult;
        private DataTable dtCompany;
        private DataSet dsPageData;
        private int PayElementPk = 0;
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
            //EntryStatus = EntryStatus.NEWMODE;
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
                ucFormulaMaster.AfterApply += new EventHandler(ucFormulaMaster_AfterApply);
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                InitializeComponent();
                if (!IsPostBack)
                {
                    this.PageIndexList = "1";
                    uclPaging.CurrentPage = 1;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    GetFieldValues(ControlsEnum.PAYELEMENT);
                    SetFieldValues(ControlsEnum.PAYELEMENT);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #endregion

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

        // Formula Popup After Apply
        void ucFormulaMaster_AfterApply(object sender, EventArgs e)
        {
            txtDeductionFormula.Text = ucFormulaMaster.FormulaText;
            hdfFormula.Value = ucFormulaMaster.FormulaValue;
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
        }

        #region ActionHandler
        /// <summary>
        /// Button actions events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            // XmlDocument xmlDoc;
            bool bIsChecked = false;
            try
            {
                 GridViewRow gvrTemplateType;
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
                        LeaveTemplateViewState = (LeaveTemplateBO.LeaveTemplate)SetUIValuesToObject(ControlsEnum.LEAVETEMPLATE);

                        result = BusinessLogic.HRMS.Admin.Masters.LeaveTemplateBL.SaveLeaveTemplate(LeaveTemplateViewState);
                        if (result > 0)
                        {
                            ResetForm(ControlsEnum.CLEAR);
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            this.EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = GetLocalResourceObject("Msg_SavSuccess").ToString(); // Resources.ErrorMessages.Msg_SavSuccess;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.LeaveTemplate);
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
                                litErrorMsg.Text = Resources.PageNameRes.LeaveTemplate + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.LeaveTemplate + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.LeaveTemplate + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.REFNOEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.LeaveTemplate + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.LeaveTemplate);
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
                                GetFieldValues(ControlsEnum.LEAVETEMPLATE);
                                SetFieldValues(ControlsEnum.LEAVETEMPLATE);
                                GetFieldValues(ControlsEnum.PAYELEMENT);
                                SetFieldValues(ControlsEnum.PAYELEMENT);
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
                        GetFieldValues(ControlsEnum.PAYELEMENT);
                        SetFieldValues(ControlsEnum.PAYELEMENT);
                        ResetForm(ControlsEnum.CLEAR);
                       
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = BusinessLogic.HRMS.Admin.Masters.LeaveTemplateBL.DeleteLeaveTemplate(CurrPK, Convert.ToString(this.LastModifiedTime));
                        if (result > 0)
                        {
                            if (grdList.Rows.Count == 1 && Convert.ToInt32(PageIndexList) > 1)
                            {
                                PageIndexList = Convert.ToString(Convert.ToInt32(PageIndexList) - 1);
                            }
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.LeaveTemplate);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            ActionHandler(lnkList, EventArgs.Empty);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.LeaveTemplate;
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
                                litErrorMsg.Text = Resources.PageNameRes.LeaveTemplate + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.LeaveTemplate + " " +
                                    GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.LeaveTemplate + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.LeaveTemplate);
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
                        this.PageIndexList = "1";
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region Show Popup
                    case ActionsEnum.SHOWPOPUP:
                        PayElementPk = Convert.ToInt32(ddlPayElement.SelectedValue);
                        GetFieldValues(ControlsEnum.PAYELEMENTDETAILS);
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ucFormulaMaster.IsDeduction = Convert.ToInt32(dtResult.Rows[0]["PEL_IS_DEDUCTION"]);
                        }
                        ucFormulaMaster.GetFieldValues(Admin.Masters.UserControls.FormulaMaster.ControlsEnum.PAYELEMENT);
                        ucFormulaMaster.SetFieldValues(Admin.Masters.UserControls.FormulaMaster.ControlsEnum.PAYELEMENT);

                        string payelmnt = ddlPayElement.SelectedIndex > 0 ? ddlPayElement.SelectedItem.ToString() : string.Empty;                     
                        ucFormulaMaster.SetData(txtDeductionFormula.Text, payelmnt);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpFormula]','Formula','660','200');", true);
                        break;
                    #endregion
                    #region ADDTOLIST
                    case ActionsEnum.ADDTOLIST:
                        int slNo = GetNullableInt(hdfCurrentSlNo.Value) ?? 0;
                        int ltdPk = GetNullableInt(hdfCurrentLTD_PK.Value) ?? 0;
                        LeaveTemplateBO.LeaveDeductionDetail leaveDeductionDetail = new LeaveTemplateBO.LeaveDeductionDetail();
                        LeaveTemplateBO.LeaveTemplate tempTemplate = LeaveTemplateViewState;
                        if (slNo > 0) // Updation
                        {
                            leaveDeductionDetail = tempTemplate.LeaveDeductionDetails
                                .Where(x => x.SlNo == slNo)
                                .Single();
                            leaveDeductionDetail.LTD_PAY_ELEMENT = GetNullableInt(ddlPayElement.SelectedValue).Value;
                            leaveDeductionDetail.LTD_ELEMENT_TEXT = ddlPayElement.SelectedItem.Text;
                            leaveDeductionDetail.LTD_FORMULA = hdfFormula.Value;
                            leaveDeductionDetail.LTD_FORMULA_TEXT = txtDeductionFormula.Text;
                        }
                        else  // New Entry
                        {
                            if (tempTemplate.LeaveDeductionDetails == null) tempTemplate.LeaveDeductionDetails = new List<LeaveTemplateBO.LeaveDeductionDetail>();
                            if (tempTemplate.LeaveDeductionDetails.Count > 0)
                                leaveDeductionDetail.SlNo = (LeaveTemplateViewState.LeaveDeductionDetails.Max(x => x.SlNo)) + 1;
                            else
                                leaveDeductionDetail.SlNo = 1;
                            leaveDeductionDetail.LTD_PK = 0;
                            leaveDeductionDetail.LTD_PAY_ELEMENT = GetNullableInt(ddlPayElement.SelectedValue).Value;
                            leaveDeductionDetail.LTD_ELEMENT_TEXT = ddlPayElement.SelectedItem.Text;
                            leaveDeductionDetail.LTD_FORMULA = hdfFormula.Value;
                            leaveDeductionDetail.LTD_FORMULA_TEXT = txtDeductionFormula.Text;
                            tempTemplate.LeaveDeductionDetails
                                .Add(leaveDeductionDetail);
                        }
                        LeaveTemplateViewState = tempTemplate;
                        ResetForm(ControlsEnum.CLEARDEDUCTION);
                        BindGrid(ControlsEnum.DEDUCTION);

                        break;
                    #endregion
                    #region Activate
                    // Do Action if click Activate Button
                    case ActionsEnum.ACTIVATE:
                        gvrTemplateType = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        result = BusinessLogic.HRMS.Admin.Masters.LeaveTemplateBL.UpdateLeaveTemplateStatus(Convert.ToInt32(((HiddenField)grdList.Rows[gvrTemplateType.RowIndex].FindControl("hdfTemplatePkListPage")).Value), 1, currentUser.PKUser,((HiddenField)grdList.Rows[gvrTemplateType.RowIndex].FindControl("hdfListLTE_MOD_DT")).Value);
                        if (result > 0)
                        {
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Activate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.LeaveTemplate.ToString());
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
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.LeaveTemplate.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.LeaveTemplate.ToString());
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
                        gvrTemplateType = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        result= BusinessLogic.HRMS.Admin.Masters.LeaveTemplateBL.UpdateLeaveTemplateStatus(Convert.ToInt32(((HiddenField)grdList.Rows[gvrTemplateType.RowIndex].FindControl("hdfTemplatePkListPage")).Value), 0, currentUser.PKUser, ((HiddenField)grdList.Rows[gvrTemplateType.RowIndex].FindControl("hdfListLTE_MOD_DT")).Value);
                        if (result > 0)
                        {
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_InActivate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.LeaveTemplate.ToString());
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
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.LeaveTemplate.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.LeaveTemplate.ToString());
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

        /// <summary>ON SORTING
        ///Handler for Gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
            //List<DirectStockAdmissionBO.StockAdmissionItem> tempStockAdmissionItemList;
            GridView senderGridView = (GridView)sender;
            if (senderGridView.ID == "grdLeaveDedution")
            {
                if (e.CommandName == "EDIT_ACTION")
                {
                    ResetForm(ControlsEnum.CLEARDEDUCTION);
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfSlNo = row.FindControl("hdfSlNo") as HiddenField;
                    HiddenField hdfLTD_PK = row.FindControl("hdfLTD_PK") as HiddenField;
                    HiddenField hdfLTD_PAY_ELEMENT = row.FindControl("hdfLTD_PAY_ELEMENT") as HiddenField;
                    HiddenField hdfLTD_FORMULA = row.FindControl("hdfLTD_FORMULA") as HiddenField;
                    Label lblLTD_FORMULA_TEXT = row.FindControl("lblLTD_FORMULA_TEXT") as Label;
                    hdfCurrentSlNo.Value = hdfSlNo.Value;
                    hdfCurrentLTD_PK.Value = hdfLTD_PK.Value;
                    PayElementPk = Convert.ToInt32(hdfLTD_PAY_ELEMENT.Value);
                    GetFieldValues(ControlsEnum.PAYELEMENT);
                    SetFieldValues(ControlsEnum.PAYELEMENT);
                    ddlPayElement.SelectedIndex = ddlPayElement.Items.IndexOf(ddlPayElement.Items.FindByValue(hdfLTD_PAY_ELEMENT.Value));
                    hdfFormula.Value = hdfLTD_FORMULA.Value;
                    txtDeductionFormula.Text = lblLTD_FORMULA_TEXT.Text;
                }
                else if (e.CommandName == "DELETE_ACTION")
                {
                    ResetForm(ControlsEnum.CLEARDEDUCTION);
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfSlNo = row.FindControl("hdfSlNo") as HiddenField;
                    HiddenField hdfLTD_PK = row.FindControl("hdfLTD_PK") as HiddenField;
                    int slNo = GetNullableInt(hdfSlNo.Value).Value;
                    int ltdPk = GetNullableInt(hdfLTD_PK.Value).Value;
                    LeaveTemplateBO.LeaveTemplate tempLeaveTemplate = LeaveTemplateViewState;
                    LeaveTemplateBO.LeaveDeductionDetail detail = tempLeaveTemplate.LeaveDeductionDetails
                        .Where(x => x.SlNo == slNo && x.LTD_PK == ltdPk)
                        .Single();
                    tempLeaveTemplate.LeaveDeductionDetails.Remove(detail);
                    LeaveTemplateViewState = tempLeaveTemplate;
                    BindGrid(ControlsEnum.DEDUCTION);
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
                        gridParam.PageNumber =Convert.ToInt32( PageIndexList);
                        gridParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        dtResult = BusinessLogic.HRMS.Admin.Masters.LeaveTemplateBL.GetLeaveTemplateListPage(gridParam, currentUser.SBUID, txtFilterCode.Text.Trim(),Convert.ToString(GetLocalResourceObject("SortOderBy")));
                        break;
                    #endregion
                    #region LEAVETEMPLATE
                    case ControlsEnum.LEAVETEMPLATE:
                        string xmlData = BusinessLogic.HRMS.Admin.Masters.LeaveTemplateBL.LeaveTemplateGetXML(CurrPK);
                        LeaveTemplateBO.LeaveTemplate leaveTemplateObj = CommonFunctions.XmlDeserialize<LeaveTemplateBO.LeaveTemplate>(xmlData);
                        for (int i = 0; i < leaveTemplateObj.LeaveDeductionDetails.Count; i++)
                        {
                            leaveTemplateObj.LeaveDeductionDetails[i].SlNo = i + 1;
                        }
                        LeaveTemplateViewState = leaveTemplateObj;

                        break;
                    #endregion
                    #region PAYELEMENT
                    case ControlsEnum.PAYELEMENT:
                        dtResult = BusinessLogic.HRMS.Admin.Masters.PayElementsMasterBL.GetParentElement(PayElementPk, Convert.ToInt32(CommonConstants.ACTIVE), currentUser.SBUID, 0, 0, -1, 0);
                        break;
                    #endregion
                    #region PAY ELEMENT DETAILS
                    case ControlsEnum.PAYELEMENTDETAILS:
                        dtResult = BusinessLogic.HRMS.Admin.Masters.PayElementsMasterBL.GetParentElement(PayElementPk, Convert.ToInt32(DbActiveStatus.HASPK), currentUser.SBUID, 0, 0, -1);
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
                    case ControlsEnum.LEAVETEMPLATE:
                        GetUIValuesFromObject(ControlsEnum.LEAVETEMPLATE);
                        break;
                    #endregion
                    #region PAYELEMENT
                    case ControlsEnum.PAYELEMENT:
                        BindDropDown(ControlsEnum.PAYELEMENT);
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
                #region PAYELEMENT
                case ControlsEnum.PAYELEMENT:
                    ddlPayElement.Items.Clear();
                    ddlPayElement.DataSource = dtResult;
                    ddlPayElement.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_NAME;
                    ddlPayElement.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_PK;
                    ddlPayElement.DataBind();
                    ddlPayElement.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    ddlPayElement.Items.HtmlDecode();
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
                    #region DEDUCTION
                    case ControlsEnum.DEDUCTION:
                        if (LeaveTemplateViewState != null && LeaveTemplateViewState.LeaveDeductionDetails != null)
                            grdLeaveDedution.DataSource = LeaveTemplateViewState.LeaveDeductionDetails
                                                .OrderBy(x => x.SlNo);
                        else
                            grdLeaveDedution.DataSource = null;
                        grdLeaveDedution.DataBind();
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
                    #region LEAVETEMPLATE
                    case ControlsEnum.LEAVETEMPLATE:
                        txtTemplateCode.Text = LeaveTemplateViewState.LTE_CODE.HtmlDecode();
                        txtTemplateName.Text = LeaveTemplateViewState.LTE_NAME.HtmlDecode();
                        txtDescription.Text = LeaveTemplateViewState.LTE_DESC.HtmlDecode();
                        LastModifiedTime = LeaveTemplateViewState.LAST_MOD_DT;
                        chkActive.Checked = LeaveTemplateViewState.ACTIVE == 1 ? true : false;
                        txtTemplateCode.Focus();
                        BindGrid(ControlsEnum.DEDUCTION);
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
                #region LEAVETEMPLATE
                case ControlsEnum.LEAVETEMPLATE:
                    LeaveTemplateBO.LeaveTemplate tempLeaveTemplate = LeaveTemplateViewState;
                    tempLeaveTemplate.LTE_PK = (tempLeaveTemplate.LTE_PK == null ? 0 : tempLeaveTemplate.LTE_PK);
                    tempLeaveTemplate.LTE_CODE = txtTemplateCode.Text.Trim().HtmlEncode();
                    tempLeaveTemplate.LTE_NAME = txtTemplateName.Text.Trim().HtmlEncode();
                    tempLeaveTemplate.LTE_DESC = txtDescription.Text.HtmlEncode();
                    tempLeaveTemplate.LTE_DEPT = currentUser.CurrentDeptPK;
                    GetFieldValues(ControlsEnum.COMPANY);
                    tempLeaveTemplate.LTE_COMPANY = GetNullableInt(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()).Value;
                    tempLeaveTemplate.ACTIVE = Convert.ToInt32(chkActive.Checked == true ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO));
                    tempLeaveTemplate.USER_PK = currentUser.PKUser;
                    tempLeaveTemplate.BIZUNIT_PK = currentUser.SBUID;
                    tempLeaveTemplate.LAST_MOD_DT = LastModifiedTime;
                    List<LeaveTemplateBO.LeaveDeductionDetail> leaveDeductionDetails = new List<LeaveTemplateBO.LeaveDeductionDetail>();
                    foreach (GridViewRow row in grdLeaveDedution.Rows)
                    {
                        LeaveTemplateBO.LeaveDeductionDetail temp = new LeaveTemplateBO.LeaveDeductionDetail();
                        HiddenField hdfLTD_PK = row.FindControl("hdfLTD_PK") as HiddenField;
                        temp.LTD_PK = GetNullableInt(hdfLTD_PK.Value).Value;
                        HiddenField hdfLTD_PAY_ELEMENT = row.FindControl("hdfLTD_PAY_ELEMENT") as HiddenField;
                        temp.LTD_PAY_ELEMENT = GetNullableInt(hdfLTD_PAY_ELEMENT.Value).Value;
                        HiddenField hdfLTD_FORMULA = row.FindControl("hdfLTD_FORMULA") as HiddenField;
                        temp.LTD_FORMULA = hdfLTD_FORMULA.Value;
                        leaveDeductionDetails.Add(temp);
                    }

                    tempLeaveTemplate.LeaveDeductionDetails = leaveDeductionDetails;
                    returnObject = tempLeaveTemplate;
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
                    txtTemplateCode.Text = txtTemplateName.Text = txtDeductionFormula.Text = txtTemplateNameListPage.Text = txtFilterCode.Text = hdfFormula.Value = string.Empty;
                    hdfCurrentSlNo.Value = hdfCurrentLTD_PK.Value = string.Empty;
                    chkActive.Checked = true;
                    ddlPayElement.ClearSelection();
                    LeaveTemplateViewState = null;
                    txtDescription.Text = string.Empty;
                    txtTemplateCode.Focus();
                    CurrPK = 0;
                    BindGrid(ControlsEnum.DEDUCTION);
                    break;
                #endregion
                #region CLEARSEARCH
                case ControlsEnum.CLEARSEARCH:
                    txtTemplateNameListPage.Text = string.Empty;
                    txtFilterCode.Text = string.Empty;
                    break;
                #endregion
                #region CLEARDEDUCTION
                case ControlsEnum.CLEARDEDUCTION:
                    txtDeductionFormula.Text = hdfFormula.Value = hdfCurrentSlNo.Value = hdfCurrentLTD_PK.Value = string.Empty;
                    ddlPayElement.ClearSelection();
                    break;
                #endregion
                #region POPUPCLEAR
                case ControlsEnum.POPUPCLEAR:
                    ucFormulaMaster.FormulaText = ucFormulaMaster.FormulaValue = string.Empty;
                    ucFormulaMaster.ResetForm();
                    break;
                #endregion
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
            POPUPCLEAR,
            CLEAR,
            LIST,
            LEAVETEMPLATE,
            COMPANY,
            CLEARSEARCH,
            PAYELEMENT,
            CLEARDEDUCTION,
            DEDUCTION,
            PAYELEMENTDETAILS
        }
        #endregion
    }
}