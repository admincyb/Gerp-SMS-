using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using ERP.Utilities;
using System.Data;
using System.Xml;
using BusinessObject.AccountManagement;
using BusinessObject.CommonManagement;
using BusinessObject.HRMS.Admin.Masters;
using ERPSMS_v01.UserControls;

namespace HRMS.Admin.Masters
{
    public partial class DesignationMaster : ERP.Store.UI.MyBasePage   //ERP.Store.UI.MyBasePage System.Web.UI.Page 
    {
        #region PROPERTIES
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

        private DesignationMasterBO.DesignationType DesignationTypeViewState
        {
            get
            {
                return ViewState["DesignationTypeViewState"] == null ? new DesignationMasterBO.DesignationType() : (DesignationMasterBO.DesignationType)ViewState["DesignationTypeViewState"];
            }
            set
            {
                ViewState["DesignationTypeViewState"] = value;
            }
        }

        #endregion

        #region VARIABLES

        private BusinessObject.User currentUser;
        private DataTable dtResult;
        private DataTable dtDocType;
        private DataTable dtCompany;
        private DataSet dsPageData;
        private DataTable dtJobCategory;
        private DataTable dtJobLevel;
        private DesignationMasterBO.DesignationType treeDataObj;
        private int commonPK = 0;
        #endregion

        #region PageEvents
        /// <summary>
        /// PageInit Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
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

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            //EntryStatus = EntryStatus.NEWMODE;
            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
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
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch(0);});", true);
        }

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
                    this.PageIndex = 1;
                    uclPaging.CurrentPage = 1;
                    //GetFieldValues(ControlsEnum.BindDocTree);
                    //SetFieldValues(ControlsEnum.BindDocTree);
                    GetFieldValues(ControlsEnum.BindDocTree);
                    SetFieldValues(ControlsEnum.BindDocTree);
                    GetFieldValues(ControlsEnum.JOBCATEGORY);
                    SetFieldValues(ControlsEnum.JOBCATEGORY);
                    SetFieldValues(ControlsEnum.JOBCATEGORYSRCH);
                    GetFieldValues(ControlsEnum.JOBLEVEL);
                    SetFieldValues(ControlsEnum.JOBLEVEL);
                    SetFieldValues(ControlsEnum.JOBLEVELSRCH);
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

        #region Action Handlers

        protected void ActionHandler(object sender, EventArgs e)
        {
            GridViewRow gvrDesignationType;
            XmlDocument xmlDoc;
            bool bIsChecked = false;
            try
            {
                int? result = null;

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

                switch (commonActions)
                {
                    #region NEW
                    case ActionsEnum.NEW:
                        EntryStatus = EntryStatus.NEWMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        ClearTree();
                        break;
                    #endregion

                    #region SAVE
                    case ActionsEnum.SAVE:
                        DesignationTypeViewState = (DesignationMasterBO.DesignationType)SetUIValuesToObject(ControlsEnum.DesignationType);
                        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(DesignationTypeViewState);
                        result = BusinessLogic.HRMS.Admin.Masters.DesignationMasterBL.SaveDesignationMaster(xmlDoc.InnerXml);
                        if (result > 0)
                        {
                            ResetForm(ControlsEnum.CLEAR);
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            this.EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = GetLocalResourceObject("Msg_SavSuccess").ToString(); // Resources.ErrorMessages.Msg_SavSuccess;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DesignationMaster);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.DesignationMaster + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.DesignationMaster + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.DesignationMaster + " " + Resources.Messages.AlreadyExists;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.REFNOEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.DesignationMaster + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DesignationMaster);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
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
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDesignationPkListPage")).Value);
                                //ClearTree();
                                GetFieldValues(ControlsEnum.DesignationType);
                                SetFieldValues(ControlsEnum.DesignationType);
                                GetFieldValues(ControlsEnum.BindDocTree);
                                SetFieldValues(ControlsEnum.BindDocTree);
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

                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = BusinessLogic.HRMS.Admin.Masters.DesignationMasterBL.DeleteDesignationTypeMaster(this.CurrPK, Convert.ToString(this.LastModifiedTime));
                        if (result > 0)
                        {
                            EntryStatus = EntryStatus.LISTMODE;
                            ResetForm(ControlsEnum.CLEAR);
                            if (grdList.Rows.Count == 1 && PageIndex > 1)
                            {
                                PageIndex--;
                            }
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            btnNew.Focus();
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DesignationMaster);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.DesignationMaster;
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
                                litErrorMsg.Text = Resources.PageNameRes.DesignationMaster + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.DesignationMaster + " " +
                                    GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.DesignationMaster + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DesignationMaster);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region FILTER
                    case ActionsEnum.FILTER:
                        uclPaging.CurrentPage = 0;
                        this.PageIndex = 1;
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion

                    #region ACTIVATE
                    // Do Action if click Activate Button
                    case ActionsEnum.ACTIVATE:
                        gvrDesignationType = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        result = BusinessLogic.HRMS.Admin.Masters.DesignationMasterBL.UpdateDesignationTypeMasterStatus(Convert.ToInt32(((HiddenField)grdList.Rows[gvrDesignationType.RowIndex].FindControl("hdfDesignationPkListPage")).Value), 1, currentUser.PKUser, ((HiddenField)grdList.Rows[gvrDesignationType.RowIndex].FindControl("hdfListdsgModOn")).Value);
                        if (result > 0)
                        {
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Activate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DesignationMaster.ToString());
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
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DesignationMaster.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DesignationMaster.ToString());
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
                        gvrDesignationType = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        result = BusinessLogic.HRMS.Admin.Masters.DesignationMasterBL.UpdateDesignationTypeMasterStatus(Convert.ToInt32(((HiddenField)grdList.Rows[gvrDesignationType.RowIndex].FindControl("hdfDesignationPkListPage")).Value), 0, currentUser.PKUser, ((HiddenField)grdList.Rows[gvrDesignationType.RowIndex].FindControl("hdfListdsgModOn")).Value);
                        if (result > 0)
                        {
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_InActivate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DesignationMaster.ToString());
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
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DesignationMaster.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DesignationMaster.ToString());
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

                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        uclPaging.CurrentPage = 0;
                        this.PageIndex = 1;
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);

                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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

        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex;
            //  EntryStatus = EntryStatus.LISTMODE;
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
            try
            {
                switch (type)
                {

                    #region LIST
                    case ControlsEnum.LIST:
                        int? category = 0;
                        int? grade = 0;
                        category = Convert.ToInt32(ddlJobCategSrch.SelectedValue) == 0 ? (int?)null : Convert.ToInt32(ddlJobCategSrch.SelectedValue);
                        grade = Convert.ToInt32(ddlJobGradeSrch.SelectedValue) == 0 ? (int?)null : Convert.ToInt32(ddlJobGradeSrch.SelectedValue);
                        dsPageData = BusinessLogic.HRMS.Admin.Masters.DesignationMasterBL.GetDesignationList(null, Convert.ToInt32(DbActiveStatus.ACTIVE), PageIndex, Convert.ToInt32(GetLocalResourceObject("PageSize")), currentUser.SBUID, category, grade, txtFilterDesigCode.Text.Trim(), Convert.ToString(txtFilterDesigName.Text));
                        break;
                    #endregion

                    #region DesignationType
                    case ControlsEnum.DesignationType:
                        dtResult = BusinessLogic.HRMS.Admin.Masters.DesignationMasterBL.GetSDesignationType(this.CurrPK, Convert.ToInt32(DbActiveStatus.HASPK), currentUser.SBUID);
                        DesignationMasterBO.DesignationType tempDesignationType = new DesignationMasterBO.DesignationType();
                        foreach (DataRow dtRow in dtResult.Rows)
                        {
                            tempDesignationType.dsgPK = this.CurrPK;
                            tempDesignationType.dsgCode = Convert.ToString(dtRow[GTIService.Constants.HRMS.Admin.Masters.Fields.dsgCode]);
                            tempDesignationType.dsgName = Convert.ToString(dtRow[GTIService.Constants.HRMS.Admin.Masters.Fields.dsgName]);
                            tempDesignationType.dsgDesc = Convert.ToString(dtRow[GTIService.Constants.HRMS.Admin.Masters.Fields.dsgDesc]);
                            tempDesignationType.dsgActive = Convert.ToInt32(dtRow[GTIService.Constants.HRMS.Admin.Masters.Fields.dsgActive]);
                            tempDesignationType.LAST_MOD_DT = Convert.ToDateTime(dtRow[GTIService.Constants.HRMS.Admin.Masters.Fields.dsgModOn]);
                            tempDesignationType.dsgJobCategory = Convert.ToString(dtRow[GTIService.Constants.HRMS.Admin.Masters.Fields.dsgJobCategory]);
                            tempDesignationType.dsgJobLevel = Convert.ToString(dtRow[GTIService.Constants.HRMS.Admin.Masters.Fields.dsgJobLevel]);
                        }
                        DesignationTypeViewState = tempDesignationType;
                        break;
                    #endregion

                    #region Doc Type Tree
                    case ControlsEnum.BindDocTree:
                        treeDataObj = new DesignationMasterBO.DesignationType();
                        treeDataObj = BusinessLogic.HRMS.Admin.Masters.DesignationMasterBL.GetDocTypeList(this.CurrPK);
                        break;
                    #endregion
                    #region JOB CATEGORY
                    case ControlsEnum.JOBCATEGORY:
                        dtJobCategory = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetjobCategory(commonPK);
                        break;
                    #endregion
                    #region JOB LEVEL
                    case ControlsEnum.JOBLEVEL:
                        dtJobLevel = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.Getjoblevel(commonPK);
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

                    #region DESIGNATIONTYPE
                    case ControlsEnum.DesignationType:
                        GetUIValuesFromObject(ControlsEnum.DesignationType);
                        break;
                    #endregion

                    #region Doc Type
                    case ControlsEnum.BindDocTree:
                        BindDocTypeTree(controlType);
                        break;
                    #endregion

                    #region Job Category
                    case ControlsEnum.JOBCATEGORY:
                    case ControlsEnum.JOBCATEGORYSRCH:
                        BindDropDown(controlType);
                        break;
                    #endregion

                    #region Job Level
                    case ControlsEnum.JOBLEVEL:
                    case ControlsEnum.JOBLEVELSRCH:
                        BindDropDown(controlType);
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
                        if (dsPageData.Tables[0] != null && dsPageData.Tables[0].Rows.Count > 0)
                        {
                            rowCount = Convert.ToInt32(dsPageData.Tables[0].Rows[0]["REC_COUNT"].ToString());
                        }
                        uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                  (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                  (rowCount / pageSize) + 1;

                        PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        grdList.DataSource = dsPageData.Tables[0];
                        grdList.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();
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

        /// <summary>
        /// for bind drop downs
        /// </summary>
        /// <param name="controlType"></param>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region Job Category
                case ControlsEnum.JOBCATEGORY:
                    ddljobCategory.Items.Clear();
                    if (dtJobCategory != null)
                    {
                        ddljobCategory.DataSource = CommonFunctions.HtmlDecodeDataTable(dtJobCategory, GTIService.Constants.Common.Common.CON_DATA_TEXT_FIELD);
                        ddljobCategory.DataTextField = GTIService.Constants.Common.Common.CON_DATA_TEXT_FIELD;
                        ddljobCategory.DataValueField = GTIService.Constants.Common.Common.CON_PK_FIELD;
                        ddljobCategory.DataBind();
                        ddljobCategory.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    }
                    break;
                #endregion
                #region Job Category Search
                case ControlsEnum.JOBCATEGORYSRCH:
                    ddlJobCategSrch.Items.Clear();
                    if (dtJobCategory != null)
                    {
                        ddlJobCategSrch.DataSource = CommonFunctions.HtmlDecodeDataTable(dtJobCategory, GTIService.Constants.Common.Common.CON_DATA_TEXT_FIELD);
                        ddlJobCategSrch.DataTextField = GTIService.Constants.Common.Common.CON_DATA_TEXT_FIELD;
                        ddlJobCategSrch.DataValueField = GTIService.Constants.Common.Common.CON_PK_FIELD;
                        ddlJobCategSrch.DataBind();
                        ddlJobCategSrch.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    }
                    break;
                #endregion
                #region Job Level
                case ControlsEnum.JOBLEVEL:
                    ddlJobLevel.Items.Clear();
                    if (dtJobLevel != null)
                    {
                        ddlJobLevel.DataSource = CommonFunctions.HtmlDecodeDataTable(dtJobLevel, GTIService.Constants.Common.Common.CON_DATA_TEXT_FIELD);
                        ddlJobLevel.DataTextField = GTIService.Constants.Common.Common.CON_DATA_TEXT_FIELD;
                        ddlJobLevel.DataValueField = GTIService.Constants.Common.Common.CON_PK_FIELD;
                        ddlJobLevel.DataBind();
                        ddlJobLevel.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    }
                    break;
                #endregion
                #region Job Level Search
                case ControlsEnum.JOBLEVELSRCH:
                    ddlJobGradeSrch.Items.Clear();
                    if (dtJobLevel != null)
                    {
                        ddlJobGradeSrch.DataSource = CommonFunctions.HtmlDecodeDataTable(dtJobLevel, GTIService.Constants.Common.Common.CON_DATA_TEXT_FIELD);
                        ddlJobGradeSrch.DataTextField = GTIService.Constants.Common.Common.CON_DATA_TEXT_FIELD;
                        ddlJobGradeSrch.DataValueField = GTIService.Constants.Common.Common.CON_PK_FIELD;
                        ddlJobGradeSrch.DataBind();
                        ddlJobGradeSrch.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    }
                    break;
                #endregion
            }
        }
        //

        #region BindTree
        private void BindDocTypeTree(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region Doc Tree
                case ControlsEnum.BindDocTree:
                    TreeNode root;
                    trvUser.Nodes.Clear();
                    root = new TreeNode(GetLocalResourceObject("DocumentType").ToString());
                    root.ToolTip = GetLocalResourceObject("DocumentType").ToString();
                    TreeNode childNode;
                    foreach (DesignationMasterBO.TreeDocDetails objDet in treeDataObj.DocTreeDetailsList)
                    {
                        childNode = new TreeNode();
                        childNode.Text = objDet.DDM_DOC_TYPE_TEXT;
                        childNode.Value = objDet.DDM_DOC_TYPE.ToString();
                        childNode.SelectAction = TreeNodeSelectAction.Select;
                        childNode.Checked = objDet.IS_MAP_FL == 1 ? true : false;
                        root.ChildNodes.Add(childNode);
                    }
                    root.SelectAction = TreeNodeSelectAction.None;
                    trvUser.Nodes.Add(root);
                    trvUser.ExpandAll();
                    break;
                #endregion
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
                    #region SALARYTEMPLATE
                    case ControlsEnum.DesignationType:
                        if (DesignationTypeViewState != null)
                        {
                            txtDesignationCode.Text = HttpUtility.HtmlDecode(DesignationTypeViewState.dsgCode);
                            txtDesignationName.Text = HttpUtility.HtmlDecode(DesignationTypeViewState.dsgName);
                            txtDescription.Text = HttpUtility.HtmlDecode(DesignationTypeViewState.dsgDesc);
                            chkActive.Checked = DesignationTypeViewState.dsgActive == 1 ? true : false;
                            if (!string.IsNullOrEmpty(DesignationTypeViewState.dsgJobCategory.ToString()))
                                ddljobCategory.SelectedValue = DesignationTypeViewState.dsgJobCategory.ToString();
                            if (!string.IsNullOrEmpty(DesignationTypeViewState.dsgJobLevel.ToString()))
                                ddlJobLevel.SelectedValue = DesignationTypeViewState.dsgJobLevel.ToString();
                            LastModifiedTime = DesignationTypeViewState.LAST_MOD_DT;
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
                #region DESIGNATION
                case ControlsEnum.DesignationType:
                    DesignationMasterBO.DesignationType tempDesignationType = DesignationTypeViewState;
                    tempDesignationType.dsgPK = (tempDesignationType.dsgPK == null ? 0 : tempDesignationType.dsgPK);
                    tempDesignationType.dsgCode = txtDesignationCode.Text.Trim().HtmlEncode();
                    tempDesignationType.dsgName = txtDesignationName.Text.HtmlEncode();
                    tempDesignationType.dsgDesc = txtDescription.Text.HtmlEncode() == string.Empty ? null : txtDescription.Text.HtmlEncode();
                    tempDesignationType.dsgBizUnit = currentUser.SBUID;
                    tempDesignationType.DSG_DEPT = currentUser.CurrentDeptPK;
                    tempDesignationType.dsgActive = Convert.ToInt32(chkActive.Checked == true ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO));
                    tempDesignationType.USER_PK = currentUser.PKUser;
                    tempDesignationType.LAST_MOD_DT = this.LastModifiedTime;
                    tempDesignationType.dsgJobCategory = ddljobCategory.SelectedValue == (CommonConstants.SELECTVAL).ToString() ? null : ddljobCategory.SelectedValue;
                    tempDesignationType.dsgJobLevel = ddlJobLevel.SelectedValue == (CommonConstants.SELECTVAL).ToString() ? null : ddlJobLevel.SelectedValue;

                    List<DesignationMasterBO.TreeDocDetails> DocList = new List<DesignationMasterBO.TreeDocDetails>();

                    foreach (TreeNode parent in trvUser.Nodes)
                    {
                        foreach (TreeNode child in parent.ChildNodes)
                        {
                            if (child.Checked == true)
                            {
                                DocList.Add(
                                              new DesignationMasterBO.TreeDocDetails()
                                              {
                                                  DDM_PK = 0,
                                                  DDM_DESIGNATION = (tempDesignationType.dsgPK == null ? 0 : tempDesignationType.dsgPK),
                                                  DDM_DOC_TYPE = Convert.ToInt32(child.Value),
                                                  IS_MAP_FL = Convert.ToInt16(child.Checked)
                                              });
                            }
                        }
                    }
                    tempDesignationType.DocTreeDetailsList = DocList;
                    returnObject = tempDesignationType;
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
                    txtDesignationCode.Text = txtDesignationName.Text = txtDescription.Text = string.Empty;
                    chkActive.Checked = true;
                    DesignationTypeViewState = null;
                    txtDesignationCode.Focus();
                    ddlJobLevel.SelectedIndex = 0;
                    ddljobCategory.SelectedIndex = 0;
                    CurrPK = 0;
                    txtFilterDesigCode.Text = txtFilterDesigName.Text = string.Empty;
                    ddlJobCategSrch.SelectedIndex = ddlJobGradeSrch.SelectedIndex = 0;
                    break;
                #endregion
                #region CLEARSEARCH
                case ControlsEnum.CLEARSEARCH:
                    txtFilterDesigCode.Text = txtFilterDesigName.Text = string.Empty;
                    ddlJobCategSrch.SelectedIndex = ddlJobGradeSrch.SelectedIndex = 0;
                    break;
                #endregion
            }
        }

        private void ClearTree()
        {

            //Clear Customer
            foreach (TreeNode node in trvUser.Nodes)
            {
                foreach (TreeNode child1 in node.ChildNodes)
                {
                    child1.Checked = false;
                }
            }
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            CLEAR,
            LIST,
            CLEARSEARCH,
            DesignationType,
            BindDocTree,
            BindTreeEdit,
            JOBCATEGORY,
            JOBLEVEL,
            JOBCATEGORYSRCH,
            JOBLEVELSRCH
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
    }
}