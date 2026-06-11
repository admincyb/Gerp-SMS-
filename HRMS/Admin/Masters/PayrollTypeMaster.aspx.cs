using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using System.Data;
using BusinessObject.HRMS.Admin.Masters;
using BusinessObject.CommonManagement;
using System.Xml;
using BusinessObject.Common;
using ERPSMS_v01.UserControls;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace HRMS.Admin.Masters
{
    public partial class PayrollTypeMaster : ERP.Store.UI.MyBasePage
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
        private PayrollTypeMasterBO.PayrollType PayrollTypeViewState
        {
            get
            {
                return ViewState["PayrollTypeViewState"] == null ? new PayrollTypeMasterBO.PayrollType() : (PayrollTypeMasterBO.PayrollType)ViewState["PayrollTypeViewState"];
            }
            set
            {
                ViewState["PayrollTypeViewState"] = value;
            }
        }
        /// <summary>
        /// Current PK
        /// </summary>
        private int currPK
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

        private BusinessObject.User currentUser;
        private DataTable dtResult;
        private DataTable dtCompany;
        private DataSet dsPageData;
        private DataTable dtProcessMode;
        private DataSet dsName;
        private PayrollTypeMasterBO.PayrollType treeDataObj; 
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
            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
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
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                    GetFieldValues(ControlsEnum.PROCESSMODE);
                    SetFieldValues(ControlsEnum.PROCESSMODE);
                    GetFieldValues(ControlsEnum.FILTERPROCESSMODE);
                    SetFieldValues(ControlsEnum.FILTERPROCESSMODE);
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    GetFieldValues(ControlsEnum.BINDTREE);
                    SetFieldValues(ControlsEnum.BINDTREE);
                    SetFieldValues(ControlsEnum.PAYROLLSTART);                   
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                    PageIndex = Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
            GridViewRow gvrPayrollType;
            XmlDocument xmlDoc;
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
            
                #endregion
                switch (commonActions)
                {
                    #region SAVE
                    case ActionsEnum.SAVE:
                        PayrollTypeViewState = (PayrollTypeMasterBO.PayrollType)SetUIValuesToObject(ControlsEnum.PayrollType);
                        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(PayrollTypeViewState);
                        result = BusinessLogic.HRMS.Admin.Masters.PayrollTypeMasterBL.SavePayrollTypeMaster(xmlDoc.InnerXml);
                        if (result > 0)
                        {
                            ResetForm(ControlsEnum.CLEAR);
                            GetFieldValues(ControlsEnum.LIST);
                           SetFieldValues(ControlsEnum.LIST);
                            this.EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = GetLocalResourceObject("Msg_SavSuccess").ToString(); // Resources.ErrorMessages.Msg_SavSuccess;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PayrollTypeMaster);
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
                                litErrorMsg.Text = Resources.PageNameRes.PayrollTypeMaster + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.PayrollTypeMaster + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.PayrollTypeMaster + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PayrollTypeMaster);
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
                                currPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPayrollTypePkListPage")).Value);                             
                                GetFieldValues(ControlsEnum.PayrollType);
                                SetFieldValues(ControlsEnum.PayrollType);
                                GetFieldValues(ControlsEnum.BINDTREE);
                                SetFieldValues(ControlsEnum.BINDTREE);
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
                        ClearTree();
                        GetFieldValues(ControlsEnum.BINDTREE);
                        SetFieldValues(ControlsEnum.BINDTREE);
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        while (trvUser.CheckedNodes.Count > 0)
                        {
                            trvUser.CheckedNodes[0].Checked = false;
                        }
                        result = BusinessLogic.HRMS.Admin.Masters.PayrollTypeMasterBL.DeletePayrollTypeMaster(this.currPK, Convert.ToString( this.LastModifiedTime));
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
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PayrollTypeMaster);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.PayrollTypeMaster;
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
                                litErrorMsg.Text = Resources.PageNameRes.PayrollTypeMaster + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.PayrollTypeMaster + " " +
                                    GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.PayrollTypeMaster + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PayrollTypeMaster);
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
                        this.currPK = 0;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region Activate
                    // Do Action if click Activate Button
                    case ActionsEnum.ACTIVATE:
                        gvrPayrollType = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        result = BusinessLogic.HRMS.Admin.Masters.PayrollTypeMasterBL.UpdatePayrollTypeMasterStatus(Convert.ToInt32(((HiddenField)grdList.Rows[gvrPayrollType.RowIndex].FindControl("hdfPayrollTypePkListPage")).Value), 1, currentUser.PKUser, ((HiddenField)grdList.Rows[gvrPayrollType.RowIndex].FindControl("hdfListPTM_MOD_DT")).Value);
                        if (result > 0)
                        {
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Activate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PayrollTypeMaster.ToString());
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
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PayrollTypeMaster.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PayrollTypeMaster.ToString());
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
                        gvrPayrollType = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        result = BusinessLogic.HRMS.Admin.Masters.PayrollTypeMasterBL.UpdatePayrollTypeMasterStatus(Convert.ToInt32(((HiddenField)grdList.Rows[gvrPayrollType.RowIndex].FindControl("hdfPayrollTypePkListPage")).Value), 0, currentUser.PKUser, ((HiddenField)grdList.Rows[gvrPayrollType.RowIndex].FindControl("hdfListPTM_MOD_DT")).Value);
                        if (result > 0)
                        {
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_InActivate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PayrollTypeMaster.ToString());
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
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PayrollTypeMaster.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PayrollTypeMaster.ToString());
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
                        this.currPK = 0;
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
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), 0, 0);
                        break;
                    #endregion

                    #region Modes
                    case ControlsEnum.PROCESSMODE:
                        dtProcessMode = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PAYROLL PROCESS MODE"); 
                        break;
                    case ControlsEnum.FILTERPROCESSMODE:
                        dtProcessMode = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PAYROLL PROCESS MODE");
                        break;
                    #endregion

                    #region LIST
                    case ControlsEnum.LIST:
                        dsPageData = BusinessLogic.HRMS.Admin.Masters.PayrollTypeMasterBL.GetPayrollTypeList(null, Convert.ToInt32(DbActiveStatus.ACTIVE), PageIndex, Convert.ToInt32(GetLocalResourceObject("PageSize")), currentUser.SBUID,Convert.ToInt32( ddlProcessModeSrch.SelectedValue),txtTypeCodeSrch.Text.Trim(), Convert.ToString(txtTypeNameSrch.Text));
                        break;
                    #endregion

                    #region PayrollType
                    case ControlsEnum.PayrollType:
                        dtResult = BusinessLogic.HRMS.Admin.Masters.PayrollTypeMasterBL.GetSPayrollType(this.currPK, Convert.ToInt32(DbActiveStatus.HASPK), currentUser.SBUID);
                        PayrollTypeMasterBO.PayrollType tempPayrollType = new PayrollTypeMasterBO.PayrollType();
                        foreach (DataRow dtRow in dtResult.Rows)
                        {
                            tempPayrollType.PTM_PK = this.currPK;
                            tempPayrollType.PTM_CODE = Convert.ToString(dtRow[GTIService.Constants.HRMS.Admin.Masters.Fields.PTM_CODE]);
                            tempPayrollType.PTM_NAME = Convert.ToString(dtRow[GTIService.Constants.HRMS.Admin.Masters.Fields.PTM_NAME]);
                            tempPayrollType.PTM_DESC = Convert.ToString(dtRow[GTIService.Constants.HRMS.Admin.Masters.Fields.PTM_DESC]);
                            tempPayrollType.PTM_ACTIVE = Convert.ToInt32(dtRow[GTIService.Constants.HRMS.Admin.Masters.Fields.PTM_ACTIVE]);
                            tempPayrollType.PTM_PAYRL_START = Convert.ToInt32(dtRow[GTIService.Constants.HRMS.Admin.Masters.Fields.PTM_PAYRL_START]);
                            tempPayrollType.PTM_OFFSET = Convert.ToInt32(dtRow[GTIService.Constants.HRMS.Admin.Masters.Fields.PTM_OFFSET]);
                            tempPayrollType.PTM_PRC_MODE = dtRow[GTIService.Constants.HRMS.Admin.Masters.Fields.PTM_PRC_MODE] != (object)DBNull.Value ? Convert.ToByte(dtRow[GTIService.Constants.HRMS.Admin.Masters.Fields.PTM_PRC_MODE]) : Convert.ToByte(1);
                            tempPayrollType.LAST_MOD_DT = Convert.ToDateTime(dtRow[GTIService.Constants.HRMS.Admin.Masters.Fields.PTM_MOD_DT]);
                        }
                        
                        PayrollTypeViewState = tempPayrollType;
                        break;
                    #endregion

                    #region BindTree
                    case ControlsEnum.BINDTREE:

                        treeDataObj = new PayrollTypeMasterBO.PayrollType();

                        treeDataObj = BusinessLogic.HRMS.Admin.Masters.PayrollTypeMasterBL.GetUser(currPK);
                        
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

                    #region SALARYTEMPLATE
                    case ControlsEnum.PayrollType:
                        GetUIValuesFromObject(ControlsEnum.PayrollType);
                        break;
                    #endregion

                    #region Modes
                    case ControlsEnum.PROCESSMODE:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.FILTERPROCESSMODE:
                        BindDropDown(controlType);
                        break;
                    #endregion

                    #region PayrollStart
                    case ControlsEnum.PAYROLLSTART:
                        BindDropDown(controlType);
                        break;
                    #endregion

                    #region TreeView
                    case ControlsEnum.BINDTREE:
                        BindTree(controlType);
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
                #region CLEAR
                case ControlsEnum.PROCESSMODE:
                    ddlProcessMode.Items.Clear();
                    ddlProcessMode.DataSource = dtProcessMode;
                    ddlProcessMode.DataTextField = GTIService.Constants.Common.Fields.ADM_CFG_TEXT;
                    ddlProcessMode.DataValueField=GTIService.Constants.Common.Fields.ADM_CFG_VALUE;
                    ddlProcessMode.DataBind();
                    ddlProcessMode.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;

                case ControlsEnum.FILTERPROCESSMODE:
                    ddlProcessModeSrch.Items.Clear();
                    ddlProcessModeSrch.DataSource = dtProcessMode;
                    ddlProcessModeSrch.DataTextField = GTIService.Constants.Common.Fields.ADM_CFG_TEXT;
                    ddlProcessModeSrch.DataValueField = GTIService.Constants.Common.Fields.ADM_CFG_VALUE;
                    ddlProcessModeSrch.DataBind();
                    ddlProcessModeSrch.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region PAYROLL START
                case ControlsEnum.PAYROLLSTART:
                    DataTable dtStart = new DataTable();
                    dtStart.Columns.Add("Days", typeof(int));
                    for (int i = 1; i <= 31; i++)
                    {
                        dtStart.Rows.Add(i);
                    }
                    ddlPayrollStart.Items.Clear();
                    ddlPayrollStart.DataSource = dtStart;
                    ddlPayrollStart.DataTextField = "Days";
                    ddlPayrollStart.DataValueField = "Days";
                    ddlPayrollStart.DataBind();
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
                       // uclPaging.Visible = false;
                        if (dsPageData.Tables[0] != null && dsPageData.Tables[0].Rows.Count > 0)
                        {
                            int rowCount = 0;

                            rowCount = Convert.ToInt32(dsPageData.Tables[0].Rows[0]["REC_COUNT"].ToString());
                            this.TotalPages = Convert.ToInt32(dsPageData.Tables[0].Rows[0]["ROW_NO"].ToString());

                            uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                              (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                              (rowCount / this.PageSize) + 1;
                            PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdList.DataSource = dsPageData.Tables[0];
                            grdList.DataBind();

                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            grdList.DataSource = null;
                            grdList.DataBind();
                            uclPaging.BindPager();
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

        #region BIndTree
        private void BindTree(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region BindTree
                case ControlsEnum.BINDTREE:

                    TreeNode root;
                    trvUser.Nodes.Clear();
                    root = new TreeNode(GetLocalResourceObject("User").ToString());
                    root.ToolTip = GetLocalResourceObject("User").ToString();
                    TreeNode childNode;
                    foreach (PayrollTypeMasterBO.TreeUserDetails objDet in treeDataObj.UsrDetailsList)
                    {
                        childNode = new TreeNode();
                        childNode.Text = objDet.PUM_USER_NAME;
                        childNode.Value = objDet.PUM_PK.ToString() + ',' + objDet.PUM_USER_PK.ToString();
                        childNode.SelectAction = TreeNodeSelectAction.Select;
                        childNode.Checked = objDet.IS_MAP_FL == 1 ? true : false;
                        //childNode.ToolTip = objDet.PUM_USER_NAME;
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
                    case ControlsEnum.PayrollType:
                        if (PayrollTypeViewState != null)
                        {
                            txtPayrollTypeCode.Text = HttpUtility.HtmlDecode(PayrollTypeViewState.PTM_CODE);
                            txtPayrollTypeName.Text = HttpUtility.HtmlDecode(PayrollTypeViewState.PTM_NAME);
                            txtPayrollTypeDesc.Text = HttpUtility.HtmlDecode(PayrollTypeViewState.PTM_DESC);
                            chkPayrollTypeActive.Checked = PayrollTypeViewState.PTM_ACTIVE == 1 ? true : false;
                            if (ddlProcessMode.Items.FindByValue(PayrollTypeViewState.PTM_PRC_MODE.ToString()) != null)
                                ddlProcessMode.SelectedValue = PayrollTypeViewState.PTM_PRC_MODE.ToString();
                            ddlOffset.SelectedValue = PayrollTypeViewState.PTM_OFFSET.ToString();
                            ddlPayrollStart.SelectedValue = PayrollTypeViewState.PTM_PAYRL_START.ToString();
                            LastModifiedTime = PayrollTypeViewState.LAST_MOD_DT;
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
                #region SALARYTEMPLATE
                case ControlsEnum.PayrollType:
                    PayrollTypeMasterBO.PayrollType tempPayrollType = PayrollTypeViewState;
                    tempPayrollType.PTM_PK = (tempPayrollType.PTM_PK == null ? 0 : tempPayrollType.PTM_PK);
                    tempPayrollType.PTM_CODE = txtPayrollTypeCode.Text.Trim().HtmlEncode();
                    tempPayrollType.PTM_NAME = txtPayrollTypeName.Text.HtmlEncode();
                    tempPayrollType.PTM_DESC = txtPayrollTypeDesc.Text.HtmlEncode();
                    tempPayrollType.PTM_BIZUNIT = currentUser.SBUID;
                    tempPayrollType.PTM_ACTIVE = Convert.ToInt32(chkPayrollTypeActive.Checked == true ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO));
                    tempPayrollType.USER_PK = currentUser.PKUser;
                    tempPayrollType.PTM_PRC_MODE = Convert.ToByte(ddlProcessMode.SelectedValue);
                    tempPayrollType.PTM_PAYRL_START = Convert.ToInt32(ddlPayrollStart.SelectedValue);
                    tempPayrollType.PTM_OFFSET = Convert.ToInt32(ddlOffset.SelectedValue);
                    tempPayrollType.LAST_MOD_DT =this.LastModifiedTime;

                    List<PayrollTypeMasterBO.TreeUserDetails> UserList = new List<PayrollTypeMasterBO.TreeUserDetails>();

                    foreach (TreeNode parent in trvUser.Nodes)
                    {
                        foreach (TreeNode child in parent.ChildNodes)
                        {
                            if (child.Checked == true)
                            {
                                UserList.Add(
                                              new PayrollTypeMasterBO.TreeUserDetails()
                                              {
                                                  PUM_PK = Convert.ToInt16(((string[])child.Value.Split(','))[0]),
                                                  PUM_USER_PK = Convert.ToInt16(((string[])child.Value.Split(','))[1]),
                                                  PUM_USER_NAME = child.Text.ToString(),
                                                  IS_MAP_FL = Convert.ToInt16(child.Checked)
                                              }); 
                            }
                        }
                    }

                    //foreach (TreeNode td in trvUser.CheckedNodes)
                    //{
                    //    UserList.Add(
                    //        new PayrollTypeMasterBO.TreeUserDetails() { PUM_PK = Convert.ToInt16(((string[])td.Value.Split(','))[0])
                    //                                                  , PUM_USER_PK = Convert.ToInt16(((string[])td.Value.Split(','))[1])
                    //                                                  , PUM_USER_NAME = td.Text.ToString()
                    //                                                  , IS_MAP_FL = Convert.ToInt16(td.Checked) }); 
                    //}

                    tempPayrollType.UsrDetailsList = UserList;
                    returnObject = tempPayrollType;
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
                    txtPayrollTypeCode.Text = txtPayrollTypeName.Text = txtPayrollTypeDesc.Text = string.Empty;
                    chkPayrollTypeActive.Checked = true;
                    PayrollTypeViewState = null;
                    ddlProcessMode.SelectedIndex = -1;
                    ddlPayrollStart.SelectedIndex = -1;
                    ddlOffset.SelectedIndex = 1;
                    currPK = 0;
                    break;
                #endregion
                #region CLEARSEARCH
                case ControlsEnum.CLEARSEARCH:
                    txtTypeCodeSrch.Text = txtTypeNameSrch.Text = string.Empty;
                    ddlProcessModeSrch.ClearSelection();
                    break;
                #endregion
            }
        }
        #endregion

        /// <summary>
        /// Clear Tree
        /// </summary>
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
        private decimal? GetNullableDecimal(string str)
        {
            decimal result;
            return (decimal.TryParse(str, out result) ? (decimal?)result : null);
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            CLEAR,
            PayrollType,
            COMPANY,
            LIST,
            CLEARSEARCH,
            PROCESSMODE,
            FILTERPROCESSMODE,
            PAYROLLSTART,
            BINDTREE

        }
        #endregion

    }
}