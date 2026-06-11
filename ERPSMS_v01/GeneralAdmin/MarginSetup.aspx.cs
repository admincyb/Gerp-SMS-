using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using System.Data;
using BusinessObject.AccountManagement;
using System.Xml;
using BusinessObject.Administration.Configurations;
using BusinessObject.CommonManagement;
using ERPManager;
using ERPSMS_v01.UserControls;
using BusinessObject.Common;
using BusinessObject;
using BusinessLogic.CommonManagement;

namespace ERPSMS_v01.GeneralAdmin
{
    public partial class MarginSetup : ERP.Store.UI.MyBasePage
    {
        #region "Variables And Properties"

        #region "Properties"
        /// <summary>
        /// To maintain the Type Collection in viewstate
        /// </summary>
        private DataTable Types
        {
            get
            {
                return (DataTable)this.ViewState["Types"];
            }
            set
            {
                this.ViewState["Types"] = value;
            }
        }/// <summary>
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
                return (string)this.ViewState[ViewstateStrings.PageIndex];
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
                return (int)this.ViewState[ViewstateStrings.TotalPages];
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
        #endregion

        private DataTable dtMarginTypes;
        private DataTable dtResult;
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        XmlDocument xmlDoc;
        private BusinessObject.User currentUser;
        private DataTable dtMarginSetup;
        private GridPrams gridParamObj;

        #endregion

        #region PageLevel Events
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
        #endregion

        #region Pager Methods + Init

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
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
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
        }
        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        // Assignment the first page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        // Assignment the last page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        // Increment the next page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        // Decrement the previous page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage--;
                        break;
                }
                PageIndex = uclPaging.CurrentPage.ToString();
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EnableDisableButtons(e.TotalPages);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            // Should we disable the first link?
            uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we disable the previous link?
            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we enable the next link?
            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            // Should we enable the last link?
            uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
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
        #endregion

        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                if (!IsPostBack)
                {
                    GetFieldValues(ControlsEnum.MARGINTYPE);
                    SetFieldValues(ControlsEnum.MARGINTYPE);
                    GetFieldValues(ControlsEnum.TYPE);
                    SetFieldValues(ControlsEnum.TYPE);
                    GetFieldValues(ControlsEnum.CURRENCY);
                    SetFieldValues(ControlsEnum.CURRENCY);
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);

                    PageIndex = "1";
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                    EntryStatus = EntryStatus.LISTMODE;
                }
            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                switch (type)
                {
                    case ControlsEnum.MARGINTYPE:
                        dtMarginTypes = BusinessLogic.Finance.CommSetupBL.GetMarginSetupType(null, 1, currentUser.SBUID);
                        Types = dtMarginTypes;
                        break;
                    case ControlsEnum.TYPE:
                        dtResult = CommonBL.GetAppConfig(currentUser.SBUID, "MARGIN TYPE", string.Empty);
                        break;
                    case ControlsEnum.CURRENCY:
                        dtResult = CommonBL.GetCurrencyListByBtzuUnit(currentUser.SBUID);
                        break;
                    case ControlsEnum.DEFAULT:
                        gridParamObj = new GridPrams();
                        gridParamObj.PageNumber = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        gridParamObj.PageSize = grdMarginSetup.PageSize;
                        gridParamObj.SearchBy = Convert.ToInt32(ddlSearchType.SelectedValue) > 0 ? ddlSearchType.SelectedValue : string.Empty;// "0";//ddlSearchType.SelectedValue
                        gridParamObj.SearchValue = "";
                        gridParamObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.masFromDate : SortBy;//"MAS_FROM_DT"
                        gridParamObj.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        gridParamObj.FromDate = txtSearchFromDate.Text;
                        gridParamObj.ToDate = txtSearchToDate.Text;
                        gridParamObj.BizUnit = currentUser.SBUID;

                        DataSet dsMarginSetup = new DataSet();
                        dsMarginSetup = BusinessLogic.Administration.Configurations.MarginSetupBL.GetMarginSetup(gridParamObj);
                        dtMarginSetup = dsMarginSetup.Tables[1];
                        gridParamObj.TotalRecords = Convert.ToInt32(dsMarginSetup.Tables[0].Rows[0][0]);
                        TotalPages = (gridParamObj.TotalRecords == 0) ? 1 : (gridParamObj.TotalRecords <= gridParamObj.PageSize) ? 1 : (gridParamObj.TotalRecords % gridParamObj.PageSize) == 0 ? (gridParamObj.TotalRecords / gridParamObj.PageSize) : (gridParamObj.TotalRecords / gridParamObj.PageSize) + 1;
                        BindGrid();
                        break;
                    case ControlsEnum.EDIT:
                        dtMarginSetup = BusinessLogic.Administration.Configurations.MarginSetupBL.GetMarginSetupByPK(CurrPK);
                        break;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region Set Field Values

        private void SetFieldValues(ControlsEnum type)
        {
            try
            {
                switch (type)
                {
                    case ControlsEnum.MARGINTYPE:
                        BindDropDown(ControlsEnum.MARGINTYPE);
                        break;
                    case ControlsEnum.TYPE:
                        BindDropDown(ControlsEnum.TYPE);
                        break;
                    case ControlsEnum.CURRENCY:
                        BindDropDown(ControlsEnum.CURRENCY);
                        break;
                    case ControlsEnum.EDIT:
                        GetUIValuesFromObject();
                        break;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private object SetUIValuesToObject(ActionsEnum mode)
        {
            object returnObj;
            returnObj = null;
            //BusinessObject.User currentUser;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (mode)
                {
                    #region Save
                    case ActionsEnum.ADDITEM:
                        MarginSetupBO marginObj = new MarginSetupBO();
                        if (Convert.ToInt32(ddlMarginType.SelectedValue) > 0)
                            marginObj.Type = Convert.ToInt32(ddlMarginType.SelectedValue);
                        marginObj.FromDate = Convert.ToDateTime(txtFromDate.Text.Trim());
                        marginObj.ToDate = Convert.ToDateTime(txtToDate.Text.Trim());
                        marginObj.Rate = Convert.ToDecimal(txtRate.Text);
                        marginObj.MarginType = Convert.ToInt32(ddlType.SelectedValue.ToString());
                        marginObj.Currency = Convert.ToInt32(ddlCurrency.SelectedValue.ToString());
                        marginObj.UserPk = currentUser.PKUser;
                        marginObj.MarginSetup_PK = CurrPK;
                        marginObj.BIZUnit = currentUser.SBUID;

                        if (CurrPK > 0)
                            marginObj.ModifiedDate = LastModifiedTime;

                        returnObj = marginObj;
                        break;
                        #endregion
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

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private void GetUIValuesFromObject()
        {
            try
            {
                if (dtMarginSetup != null && dtMarginSetup.Rows.Count > 0)
                {
                    LastModifiedTime = Convert.ToDateTime(dtMarginSetup.Rows[0]["MAS_MOD_DT"]);
                    ddlMarginType.SelectedValue = dtMarginSetup.Rows[0]["MAS_TYPE"].ToString();
                    ddlType.SelectedIndex = Convert.ToInt32(ddlType.Items.IndexOf(ddlType.Items.FindByValue(dtMarginSetup.Rows[0]["MARGIN_TYPE"].ToString())));
                    ddlCurrency.SelectedIndex = Convert.ToInt32(ddlCurrency.Items.IndexOf(ddlCurrency.Items.FindByValue(dtMarginSetup.Rows[0]["MAS_CURRENCY"].ToString())));
                    if (!string.IsNullOrEmpty(dtMarginSetup.Rows[0]["MAS_FROM_DT"].ToString()))
                        txtFromDate.Text = Convert.ToDateTime(dtMarginSetup.Rows[0]["MAS_FROM_DT"]).ToString(Resources.Constants.HRMSDateFormatShort);
                    if (!string.IsNullOrEmpty(dtMarginSetup.Rows[0]["MAS_TO_DT"].ToString()))
                        txtToDate.Text = Convert.ToDateTime(dtMarginSetup.Rows[0]["MAS_TO_DT"].ToString()).ToString(Resources.Constants.HRMSDateFormatShort);
                    txtRate.Text = dtMarginSetup.Rows[0]["MAS_RATE"].ToString();
                }
            }
            catch
            {
                throw;
            }
            finally
            {

            }
        }

        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region TYPE
                    case ControlsEnum.MARGINTYPE:
                        ddlMarginType.Items.Clear();
                        if (Types != null && Types.Rows.Count > 0)
                        {
                            ddlMarginType.DataSource = Types;
                            ddlMarginType.DataTextField = Resources.DataFieldRes.cfgData;
                            ddlMarginType.DataValueField = Resources.DataFieldRes.cfgValue;
                            ddlMarginType.DataBind();

                            ddlSearchType.DataSource = Types;
                            ddlSearchType.DataTextField = Resources.DataFieldRes.cfgData;
                            ddlSearchType.DataValueField = Resources.DataFieldRes.cfgValue;
                            ddlSearchType.DataBind();
                        }
                        ddlMarginType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        ddlSearchType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    #endregion
                    case ControlsEnum.TYPE:
                        ddlType.Items.Clear();
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ddlType.DataSource = dtResult;
                            ddlType.DataTextField = Resources.DataFieldRes.cfgData;
                            ddlType.DataValueField = Resources.DataFieldRes.cfgValue;
                            ddlType.DataBind();
                        }
                        ddlType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.CURRENCY:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ddlCurrency.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, GetLocalResourceObject("CurrencyCode").ToString());
                            ddlCurrency.DataTextField = GetLocalResourceObject("CurrencyCode").ToString();
                            ddlCurrency.DataValueField = GetLocalResourceObject("CurrencyPK").ToString();
                            ddlCurrency.DataBind();
                        }
                        ddlCurrency.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void BindGrid()
        {
            try
            {
                if (dtMarginSetup != null)
                {
                    TotalPages = TotalPages;
                    uclPaging.TotalPages = TotalPages;
                    PageIndex = PageIndex == null ? "1" : PageIndex;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                    grdMarginSetup.DataSource = dtMarginSetup;
                    grdMarginSetup.DataBind();
                    uclPaging.Visible = true;
                    uclPaging.BindPager();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ValidateForSave()
        {
            try
            {
                bool result = true;
                string message = string.Empty;

                if (txtRate.Text.Trim() == string.Empty)
                {
                    message = GetLocalResourceObject("SelectCommisionFormula").ToString();
                    result = false;
                }

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Method used to Reset Form Controls
        /// </summary>
        private void ResetForm()
        {
            ddlMarginType.SelectedValue = CommonConstants.SELECTVAL;
            ddlCurrency.SelectedValue = CommonConstants.SELECTVAL;
            ddlType.SelectedValue = CommonConstants.SELECTVAL;
            txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
            txtRate.Text = string.Empty;
            CurrPK = 0;
            PageIndex = "1";
            uclPaging.CurrentPage = 1;

            ddlSearchType.SelectedValue = CommonConstants.SELECTVAL;
            txtSearchFromDate.Text = string.Empty;
            txtSearchToDate.Text = string.Empty;
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
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            try
            {
                int? result;
                GridViewRow gvr;

                if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }

                switch (commonActions)
                {
                    #region Add Item
                    case ActionsEnum.ADDITEM:
                        //validate Page
                        Page.Validate();
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            MarginSetupBO objMarginSetupBO;
                            objMarginSetupBO = (MarginSetupBO)SetUIValuesToObject(ActionsEnum.ADDITEM);
                            xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objMarginSetupBO);
                            result = BusinessLogic.Administration.Configurations.MarginSetupBL.SaveMarginSetup(xmlDoc.InnerXml);
                            if (result > 0)
                            {
                                string msg = GetLocalResourceObject("MarginSetupSavedSuccessfully").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + msg + "','" + Resources.Messages.Information + "');", true);
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup1", "ClosePopup();", true);
                                ResetForm();
                                GetFieldValues(ControlsEnum.DEFAULT);
                            }
                            else
                            {
                                if (result == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.Messages.DateRangeExist;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region EDIT
                    case ActionsEnum.EDITGRID:
                        CurrPK = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                        GetFieldValues(ControlsEnum.EDIT);
                        SetFieldValues(ControlsEnum.EDIT);
                        break;
                    #endregion

                    #region DELETE
                    case ActionsEnum.DELETE:
                        CurrPK = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                        gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        LastModifiedTime = Convert.ToDateTime(((HiddenField)gvr.FindControl("hdfLastModDate")).Value);
                        result = BusinessLogic.Administration.Configurations.MarginSetupBL.DeleteMarginSetup(CurrPK, LastModifiedTime);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            ResetForm();

                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            litErrorMsg.Text = GetLocalResourceObject("DeletedSuccessfully").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
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
                                litErrorMsg.Text = Resources.PageNameRes.AgtComm + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else if (result == (int)DbDeleteStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.AgtComm + " " + Resources.Messages.UsedInAnotherPlace;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.AgtComm + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AgtComm);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region DEFAULT
                    case ActionsEnum.DEFAULT:
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        break;
                    #endregion

                    #region Clear
                    // Do Action for , when click clear button
                    case ActionsEnum.CLEAR:
                        ResetForm();
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                        #endregion

                }
            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        public enum ControlsEnum
        {
            DEFAULT,
            SAVE,
            ADDITEM,
            GET,
            MARGINTYPE,
            EDIT,
            TYPE,
            CURRENCY
        }
    }
}