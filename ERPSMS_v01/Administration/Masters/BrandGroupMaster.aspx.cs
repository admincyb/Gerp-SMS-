using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using BusinessObject.CommonManagement;
using BusinessObject.AccountManagement;
using ERPSMS_v01.UserControls;
using System.Data;
using BusinessObject.Administration.Masters;
using BusinessLogic.Administration.Masters;
using System.IO;

namespace ERPSMS_v01.Administration.Masters
{
    public partial class BrandGroupMaster : ERP.Store.UI.MyBasePage
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
       
        #endregion

        private ActionsEnum commonActions;          
        private BusinessObject.User currentUser;
        private CommonService CommonServiceClient;
        private DataTable dtPageData;
        private DataTable dtBrandGrpDet;
        private BrandGroupMasterBO objBrandGroupMasterBO;     
        private ServiceUtility serviceUtilityObj;
        private bool IsPageChanged = false;

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
                    #region DEFAULT
                    case ControlsEnum.DEFAULT:
                        serviceUtilityObj = new ServiceUtility();
                        if (!IsPageChanged)
                        {
                            PageIndex = "1";
                            uclPaging.CurrentPage = 1;
                        }
                        serviceUtilityObj.CurrentPage = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        serviceUtilityObj.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        serviceUtilityObj.TotalRecords = 0;
                        string BrandCode = string.Empty;
                        string BrandName = string.Empty;
                        if (ddlFilterBy.SelectedValue == Resources.DataFieldRes.BrandGroupCode)
                            BrandCode = txtSearchBy.Text.TrimStart().TrimEnd();
                        else if (ddlFilterBy.SelectedValue == Resources.DataFieldRes.BrandGroupName)
                            BrandName = txtSearchBy.Text.TrimStart().TrimEnd();
                        dtPageData = BusinessLogic.Administration.Masters.BrandGroupMasterBL.GetBrandGroupMaster(CurrPK, Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID, BrandCode, BrandName, serviceUtilityObj.CurrentPage, serviceUtilityObj.PageSize);

                        serviceUtilityObj.TotalRecords = dtPageData.Rows.Count > 0 ? Convert.ToInt32(dtPageData.Rows[0]["REC_COUNT"].ToString()) : 0;
                        TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                    (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                    (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;

                        break; 
                    #endregion
                    #region BRAND GROUP DETAILS
                    case ControlsEnum.BRANDGROUPDETAILS:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = 1;
                        serviceUtilityObj.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        serviceUtilityObj.TotalRecords = 0;
                        dtBrandGrpDet = BusinessLogic.Administration.Masters.BrandGroupMasterBL.GetBrandGroupMaster(CurrPK, Convert.ToInt16(DbActiveStatus.HASPK), currentUser.SBUID, string.Empty, string.Empty, serviceUtilityObj.CurrentPage, serviceUtilityObj.PageSize);
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
                    #region BRAND GROUP DETAILS
                    case ControlsEnum.BRANDGROUPDETAILS:
                        GetUIValuesFromObject();
                        break; 
                    #endregion
                    #region DEFAULT
                    case ControlsEnum.DEFAULT:
                        BindGrid(controlType);
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

        #region Helper Methods

        private object SetUIValuesToObject(ActionsEnum mode)
        {
            object returnObj;
            returnObj = null;
            try
            {
                switch (mode)
                {
                    #region Save
                    case ActionsEnum.SAVE:                       
                        objBrandGroupMasterBO = new BrandGroupMasterBO();
                        objBrandGroupMasterBO.CIG_PK = CurrPK;
                        objBrandGroupMasterBO.CIG_CODE = HttpUtility.HtmlEncode(txtBrandGroupCode.Text);
                        objBrandGroupMasterBO.CIG_NAME = HttpUtility.HtmlEncode(txtBrandGroupName.Text);
                        objBrandGroupMasterBO.CIG_BIZUNIT = currentUser.SBUID;
                        objBrandGroupMasterBO.CIG_ACTIVE = Convert.ToInt16(ddlStatus.SelectedValue);
                        objBrandGroupMasterBO.CIG_CRTD_BY = currentUser.PKUser;
                        objBrandGroupMasterBO.CIG_CRTD_DT = DateTime.Now;
                        objBrandGroupMasterBO.CIG_MOD_BY = currentUser.PKUser;
                        objBrandGroupMasterBO.CIG_MOD_DT = DateTime.Now;
                        objBrandGroupMasterBO.LAST_MOD_DT = LastModifiedTime;
                        objBrandGroupMasterBO.USER_PK = currentUser.PKUser;
                        returnObj = objBrandGroupMasterBO;
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
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject()
        {
            try
            {
                //assigning the UI controls with the corresponding ListObject value AsrSovMstList
                if (dtBrandGrpDet != null && dtBrandGrpDet.Rows.Count > 0)
                {
                    CurrPK = Convert.ToInt32(dtBrandGrpDet.Rows[0]["CIG_PK"].ToString());
                    txtBrandGroupCode.Text = HttpUtility.HtmlDecode(dtBrandGrpDet.Rows[0]["CIG_CODE"].ToString());
                    txtBrandGroupName.Text = HttpUtility.HtmlDecode(dtBrandGrpDet.Rows[0]["CIG_NAME"].ToString());
                    ddlStatus.SelectedValue = dtBrandGrpDet.Rows[0]["CIG_ACTIVE"].ToString();
                    LastModifiedTime = Convert.ToDateTime(dtBrandGrpDet.Rows[0]["CIG_MOD_DT"]);                   
                }
                else
                {
                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.BrandGroup);
                    EntryStatus = EntryStatus.LISTMODE;
                    ResetForm();
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    ModifiedDatePnl.Visible = false;
                    throw new Exception(litErrorMsg.Text);
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
                    #region DEFAULT
                    case ControlsEnum.DEFAULT:
                        if (dtPageData != null)
                        {
                            uclPaging.TotalPages = TotalPages;
                            PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdBrandGroup.PageIndex = Convert.ToInt32(PageIndex);
                            grdBrandGroup.DataSource = dtPageData.DefaultView;
                            grdBrandGroup.DataBind();
                            uclPaging.Visible = true;
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
        
       
        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                foreach (GridViewRow grdrow in grdBrandGroup.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                    // check row selected or not
                    if (rbtn.Checked)
                    {
                        // get pk from the grid and assign to CurrPk
                        CurrPK = Convert.ToInt16(grdBrandGroup.DataKeys[grdrow.RowIndex].Values[0]);
                        GetFieldValues(ControlsEnum.BRANDGROUPDETAILS);
                        SetFieldValues(ControlsEnum.BRANDGROUPDETAILS);                       
                        ModifiedDatePnl.Visible = true;
                        if (Mode == ActionsEnum.VIEW)
                            EntryStatus = EntryStatus.VIEWMODE;
                        else
                            EntryStatus = EntryStatus.ENTRYMODE;
                        return;
                    }
                }

                // if no items selected, Show Error Message
                litErrorMsg.Text = Resources.Messages.Msg_Not_Selected;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method used to Reset form Controls
        /// </summary>
        private void ResetForm()
        {
            CurrPK = 0;            
            txtBrandGroupCode.Text = string.Empty;
            txtBrandGroupName.Text = string.Empty;
            ddlStatus.SelectedValue = "1";        
            PageIndex = ERP.Utilities.CommonConstants.SELECT_VALUE_ONE;
            lblLastModifiedHDR.Text = string.Empty;
            ModifiedDatePnl.Visible = false;
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
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    commonActions = ActionsEnum.SHOWDETAILS;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                switch (commonActions)
                {
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
                            objBrandGroupMasterBO = (BrandGroupMasterBO)SetUIValuesToObject(commonActions);
                            if (objBrandGroupMasterBO != null)
                            {

                                result = BrandGroupMasterBL.SaveBrandGroupMaster(objBrandGroupMasterBO);
                                if (result >= 0) // Success ! re-initialize the page
                                {
                                    
                                    SortBy = Resources.DataFieldRes.CompanyMstPK;
                                    SortDirection = Resources.Report.SortDescending;
                                    litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.BrandGroup);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.DEFAULT);
                                    SetFieldValues(ControlsEnum.DEFAULT);
                                    btnNew.Focus();
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
                                        litErrorMsg.Text = Resources.PageNameRes.BrandGroup + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.BrandGroup) + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.BrandGroup + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.BrandGroup) + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.DATEOVERLAP)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.BrandGroup + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.BrandGroup) + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BrandGroup);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = BusinessLogic.Administration.Masters.BrandGroupMasterBL.DeleteBrandGroupMaster(CurrPK, LastModifiedTime);
                            if (result > 0)
                            {                              

                                litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                                ResetForm();
                                btnNew.Focus();
                                EntryStatus = EntryStatus.LISTMODE;
                                GetFieldValues(ControlsEnum.DEFAULT);
                                SetFieldValues(ControlsEnum.DEFAULT);
                                litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.BrandGroup);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            }
                            else
                            {
                                if (result == (int)DbDeleteStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.BrandGroup + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.BrandGroup) + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.BrandGroup + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.BrandGroup + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.BrandGroup) + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BrandGroup);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        ModifiedDatePnl.Visible = false;
                        ResetForm();
                        EntryStatus = EntryStatus.NEWMODE;
                        break;
                    #endregion                  
                    #region SEARCH
                    case ActionsEnum.SEARCH:                       
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);                       
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion                        
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        ResetForm();
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Edit
                    case ActionsEnum.EDIT:
                        SetUIEditView(commonActions);
                        break;
                    #endregion
                    #region View
                    case ActionsEnum.VIEW:
                        SetUIEditView(commonActions);
                        btnCancel.Focus();
                        break;
                    #endregion                    
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {

            }
        }

        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex.ToString();
            GetFieldValues(ControlsEnum.DEFAULT);
            SetFieldValues(ControlsEnum.DEFAULT);
            EntryStatus = EntryStatus.LISTMODE;

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
                if (SortBy == e.SortExpression)
                {
                    if (SortDirection == Resources.Report.SortAscending)
                        SortDirection = Resources.Report.SortDescending;
                    else
                        SortDirection = Resources.Report.SortAscending;
                }
                else
                {
                    SortBy = e.SortExpression;
                    SortDirection = Resources.Report.SortAscending;
                }
                this.PageIndex = ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO;
                EntryStatus = EntryStatus.LISTMODE;
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
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
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        // increment the current page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        // Decrement the current page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage--;
                        break;


                }
                IsPageChanged = true;
                PageIndex = uclPaging.CurrentPage.ToString();
                // Change Code As per the page
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EntryStatus = EntryStatus.LISTMODE;
                //============================
                EnableDisableButtons(e.TotalPages);
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }

        }

        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
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

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            if (EntryStatus == EntryStatus.VIEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);                
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
            try
            {
                if (!IsPostBack)
                {                    
                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = Resources.DataFieldRes.BrandGroupMstPK;
                    grdBrandGroup.DataKeyNames = datakeyarray;
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    EntryStatus = EntryStatus.LISTMODE;
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
            DEFAULT,      
            BRANDGROUPDETAILS
        }
        #endregion


    }
}