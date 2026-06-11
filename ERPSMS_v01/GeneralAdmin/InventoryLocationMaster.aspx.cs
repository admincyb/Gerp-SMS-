using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Administration.Masters;
using BusinessObject.AccountManagement;
using BusinessObject.Administration.Masters;
using BusinessObject.CommonManagement;
using BusinessObject.Common;
using GTIService;
using GTIService.Constants.Common;
using System.IO;
using System.Data;
using ERPSMS_v01.UserControls;
using DataAccess.CommonManagement;
using DataAccess.SubDepartmentManagement;
using System.Text.RegularExpressions;
using ERPSMS_v01.StoreManagement; 

namespace ERPSMS_v01.GeneralAdmin
{
    public partial class InventoryLocationMaster : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        #region Properties

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
        /// Save Return Value
        /// </summary>
        private int RetVal
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.RetVal]);
            }
            set
            {
                this.ViewState[ViewstateStrings.RetVal] = value;
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
        #endregion

        #region Variables
        private DataTable dtPageData;
        private DataTable dtInventoryStores;
        private DataTable dtInventoryList;

        //Global Private Variables used to maintaion data across methods in the same postback
        private int DeptPK = 0;
        private int TypeValue = 0;



        BusinessObject.User currentUser;
        // List variables for binding details to controls
        private ActionsEnum commonActions;
        //Get Or Save Company detais
        private InventoryLocationMasterBO objInvLoc;

        #endregion
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
          
        }

        #region Page Page_PreRender
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            if (EntryStatus == EntryStatus.EDITMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(3);", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            else if (EntryStatus == EntryStatus.NEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing(0);", true);
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
            if (EntryStatus == EntryStatus.VIEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
        }
        #endregion


     
        #region PageActionHandler
        /// <summary>
        /// Method to handle Page Load Action
        /// </summary>
        public void PageActionHandler()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                InitializeComponent();
                if (!IsPostBack)
                {
                    PageIndex = Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE);
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                    EntryStatus = EntryStatus.LISTMODE;                  
                    GetFieldValues(ControlsEnum.INVENTORYLIST);
                    SetFieldValues(ControlsEnum.FILLGRID);                       
                    hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
            finally
            {
            }
        }
        #endregion

        #region Action Handler
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                int result;
                result = 0;
                bool bIsChecked = false;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else
                {
                    if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
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
                            commonActions = ActionsEnum.EDIT;
                        }
                    }
                    else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                    {
                        if (((DropDownList)sender).ID == "ddlstore")
                        {
                            commonActions = ActionsEnum.CHANGE;
                        }
                    }
                }
                switch (commonActions)
                {
                    #region NEW
                    case ActionsEnum.NEW:
                        CurrPK = 0;
                        EntryStatus = EntryStatus.NEWMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.INVENTORYSTORE);
                        SetFieldValues(ControlsEnum.INVENTORYSTORE);
                        ddlstore.Focus();
                        break;
                    #endregion

                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (IsValid)
                        {

                            objInvLoc  = SetUIValuesToObject();
                            result = InventoryLocationMasterBL.SaveInvStore(CurrPK, objInvLoc, currentUser, currentUser.SBUID);
                            if (result > 0) // Success !  redirect to listing page
                            {

                                // Show Save Message and redired to listing page
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.InventoryLocMaster);

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InventoryLocation) + "');", true);
                            }
                            else
                            {
                                // if any error occur, show error details
                                DbSaveStatus saveStatus = (DbSaveStatus)result;
                                switch (saveStatus)
                                {
                                    case DbSaveStatus.SQLERROR://SQl Error
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                        break;
                                    case DbSaveStatus.CODEEXIST: //Code Exists   
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Name_Already_Exists;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.InventoryLocMaster.ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                   + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PortList) + "');", true);
                                        break;
                                    case DbSaveStatus.CONCURRENCY://Cuncurrency Check
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Error_Concurrent;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.InventoryLocMaster.ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                   + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PortList) + "');", true);
                                        break;
                                    case DbSaveStatus.ALREADYDELETED://Cuncurrency Check
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.InventoryLocMaster.ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                      + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PortList) + "');", true);
                                        break;
                                    case DbSaveStatus.OLDCODEEXIST://Entry already Exists
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Already_Exists;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.InventoryLocMaster.ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                      + "','" + Resources.ErpRes.Information + "');", true);
                                        break;

                                    case DbSaveStatus.INCORRECT://Entry already Exists
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Code_Already_Exists;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.InventoryLocMaster.ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                      + "','" + Resources.ErpRes.Information + "');", true);
                                        break;

                                    default:
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError; //Other Errors
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                        break;
                                }
                            }
                        }
                        break;
                    #endregion

                    #region EDIT
                    case ActionsEnum.EDIT:
                        bIsChecked = false;
                        foreach (GridViewRow grdrow in grdInvType.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                ResetForm(ControlsEnum.CLEAR);
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfGstCfnPk")).Value);
                                EntryStatus = EntryStatus.EDITMODE;
                                GetFieldValues(ControlsEnum.INVEDIT);
                                GetUIValuesFromObject(ControlsEnum.INVEDIT);
                                break;
                            }
                        }
                        if (!bIsChecked)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.GRIDSEARCH);
                        SetFieldValues(ControlsEnum.FILLGRID);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region CANCEL
                    case ActionsEnum.CANCEL:
                    case ActionsEnum.LIST:                       
                    case ActionsEnum.CLEAR:
                        uclPaging.CurrentPage = 0;
                        this.PageIndex = 1;
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        ResetForm(ControlsEnum.CLEARFILTER);
                        GetFieldValues(ControlsEnum.INVENTORYLIST);
                        SetFieldValues(ControlsEnum.FILLGRID);
                        txtCodeFilterList.Focus();
                        break;
                    #endregion

                    #region DELETE
                    case ActionsEnum.DELETE:
                        //bIsChecked = false;
                        result = InventoryLocationMasterBL.DeleteInvLocation(CurrPK);
                       
                         DbDeleteStatus deleteStatus = (DbDeleteStatus)result;
                        switch (deleteStatus)
                        {
                            case DbDeleteStatus.DELETED://If deletion is success
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.InventoryLocMaster);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InventoryLocation) + "');", true);
                                break;
                            case DbDeleteStatus.REFERRED://If referred to another page
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Ref;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.InventoryLocMaster);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InventoryLocation) + "');", true);
                                break;
                            case DbDeleteStatus.SQLERROR://Sql error
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.InventoryLocMaster);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                 + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InventoryLocation) + "');", true);
                                break;
                        }
                        break;
                    #endregion

                  
                    default: break;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }
        #endregion

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum controlType)
        {
            BusinessObject.GridPrams grdInvType;  
            try
            {
                switch (controlType)
                {               
                                    
                    case ControlsEnum.INVENTORYSTORE:
                        dtInventoryStores =BusinessLogic.StoreManagement.StoreMaster.GetInventoryStores(currentUser.SBUID,0,currentUser.PKUser);
                    break;

                    case ControlsEnum.INVENTORYLIST:                       
                    dtInventoryList = new DataTable();
                    grdInvType = new BusinessObject.GridPrams(); 
                    grdInvType.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                    grdInvType.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                    dtInventoryList = BusinessLogic.Administration.Masters.InventoryLocationMasterBL.GetInventoryList(grdInvType,currentUser.SBUID);
                    break;

                    case ControlsEnum.GRIDSEARCH:
                    dtInventoryList = new DataTable();
                    dtInventoryList = BusinessLogic.Administration.Masters.InventoryLocationMasterBL.GetInventoryList(txtCodeFilterList.Text, txtNameFilterList.Text,currentUser.SBUID);
                    break;

                    case ControlsEnum.INVEDIT:
                    dtInventoryList = BusinessLogic.Administration.Masters.InventoryLocationMasterBL.GetInvEdit(this.CurrPK, Convert.ToInt32(DbActiveStatus.ACTIVE));
                    break;
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

                    case ControlsEnum.INVENTORYSTORE:
                        BindDropDown(controlType);
                        break;

                    case ControlsEnum.FILLGRID:
                        BindGrid(controlType);
                        break;

                    case ControlsEnum.INVEDIT:
                        GetUIValuesFromObject(controlType);
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
        #endregion

        #region Helper Methods

        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                int rowCount = 0;
                int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                if (dtInventoryList != null && dtInventoryList.Rows.Count > 0)
                {
                    rowCount = Convert.ToInt32(dtInventoryList.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                }
                uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                      (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                      (rowCount / pageSize) + 1;
                PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                if (dtInventoryList != null && dtInventoryList.Rows.Count > 0)
                    grdInvType.DataSource = dtInventoryList;
                else
                    grdInvType.DataSource = null;
                grdInvType.DataBind();
                uclPaging.Visible = true;
                uclPaging.BindPager();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Bind DropDown  as per type 
        /// </summary>
        /// <param name="drpName"></param>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                // Fill Shift Details to DropDown
                case ControlsEnum.INVENTORYSTORE:
                    if (dtInventoryStores != null)
                    {
                        ddlstore.DataSource = CommonFunctions.HtmlDecodeDataTable(dtInventoryStores, GetLocalResourceObject("DepartmentName").ToString());
                        ddlstore.DataTextField = GetLocalResourceObject("DepartmentName").ToString();
                        ddlstore.DataValueField = GetLocalResourceObject("DeptPK").ToString();
                        ddlstore.DataBind();
                    }
                    ddlstore.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    //if (DeptPK > 0 && ddlstore.Items.FindByValue(DeptPK.ToString()) != null)
                    //    ddlstore.SelectedValue = DeptPK.ToString();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Get Value from Control to object
        /// </summary>
        /// <returns></returns>
        private InventoryLocationMasterBO SetUIValuesToObject()
        {
            //string filePath = string.Empty;
            ERP.Utilities.CryptoServices crypto = new ERP.Utilities.CryptoServices();
            InventoryLocationMasterBO objInvLoc = new InventoryLocationMasterBO();
            objInvLoc.Pk = CurrPK;
            objInvLoc.Name = HttpUtility.HtmlEncode(txtstoreName.Text);
            objInvLoc.Code = HttpUtility.HtmlEncode(txtCode.Text);
            objInvLoc.Desc = "";
            objInvLoc.Parent = Convert.ToInt32(ddlstore.SelectedValue);
            objInvLoc.bizunit = currentUser.CurrentSBUPK;
            objInvLoc.active = currentUser.Active;
            objInvLoc.company = currentUser.SBUID;
            objInvLoc.userPK = currentUser.PKUser;
            return objInvLoc;
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
                    case ControlsEnum.INVEDIT:
                        if (dtInventoryList != null && dtInventoryList.Rows.Count > 0)
                        {
                            ERP.Utilities.CryptoServices crypto = new ERP.Utilities.CryptoServices();
                            GetFieldValues(ControlsEnum.INVENTORYSTORE);
                            BindDropDown(ControlsEnum.INVENTORYSTORE);
                            ddlstore.SelectedIndex = Convert.ToInt32(ddlstore.Items.IndexOf(ddlstore.Items.FindByText(dtInventoryList.Rows[0]["DPT_PARENT_TEXT"].ToString())));
                            txtCode.Text = HttpUtility.HtmlDecode(dtInventoryList.Rows[0]["DPT_CODE"].ToString());
                            txtstoreName.Text = HttpUtility.HtmlDecode(dtInventoryList.Rows[0]["DPT_NAME"].ToString());
                      
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }
        #endregion

        #region Reset Form

        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region CLEAR
                case ControlsEnum.CLEAR:
                    CurrPK = 0;
                    txtstoreName.Text = txtCode.Text = txtCodeFilterList.Text =txtNameFilterList.Text= string.Empty;
                    //chkNonGstItem.Checked = true;
                    ddlstore.ClearSelection();                  
                    LastModifiedTime = DateTime.Now;                    
                    PageIndex = 1;                   
                    break;
                #endregion

                #region CLEAR FILTER
                case ControlsEnum.CLEARFILTER:
                    txtCodeFilterList.Text = txtNameFilterList.Text = string.Empty;
                    CurrPK = 0;
                    uclPaging.CurrentPage = 0;
                    this.PageIndex = 1;
                    this.EntryStatus = EntryStatus.LISTMODE;
                    break;
                #endregion
                default: break;
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
      
        private void InitializeComponent()
        {
            this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
            this.Init += new EventHandler(this.Page_Init);
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
                PageIndex = uclPaging.CurrentPage;
                GetFieldValues(ControlsEnum.INVENTORYLIST);
                SetFieldValues(ControlsEnum.FILLGRID);
                EntryStatus = EntryStatus.LISTMODE;
                EnableDisableButtons(e.TotalPages);
                
               
            }
            catch (Exception ex)
            {
             //   ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
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
      
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            INVENTORYSTORE
            , CLEAR
            , CLEARFILTER
            , INVENTORYLIST
            , FILLGRID
            , GRIDSEARCH
            , INVEDIT
        }

        #endregion

    }
}