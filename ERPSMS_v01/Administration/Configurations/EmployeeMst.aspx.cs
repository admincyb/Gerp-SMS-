using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using BusinessObject.Common;
using ERPSMS_v01.UserControls;
using BusinessObject.CommonManagement;
using System.Data;

namespace ERPSMS_v01.Administration.Configurations
{
    public partial class EmployeeMst : ERP.Store.UI.MyBasePage
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
        /// <summary>
        /// Current User Mode
        /// </summary>
        private LoginMode CurrMode
        {
            get
            {
                return this.ViewState[ViewstateStrings.CurrMode] == null ? LoginMode.LOCAL : (LoginMode)this.ViewState[ViewstateStrings.CurrMode];
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrMode] = value;
            }
        }
        #endregion
        private ActionsEnum commonActions;
        //page related class objects
        private ServiceUtility serviceUtilityObj;
        private EmpEmployeeMst EmpEmployeeMstObj;
        //List for binding details to controls
        private List<EmpEmployeeMst> EmpEmployeeMstList;
        private DataTable dtAutMode;
        #endregion

        #region PageLevel Events
        /// <summary>
        /// Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
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
                    EntryStatus = EntryStatus.LISTMODE;
                    //Session.Remove(SessionStrings.WkfUserMst);

                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = Resources.DataFieldRes.EmployeePK;
                    grdFlNo.DataKeyNames = datakeyarray;
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                    btnNew.Focus();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Report.Information + "');", true);
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
            EmployeeService EmployeeServiceClient;
            EmployeeServiceClient = null;
            try
            {
                EmployeeServiceClient = new EmployeeService();
                EmployeeServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(EmployeeServiceClient);
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        EmpEmployeeMstObj = CommonFunctions.Initilize<EmpEmployeeMst>();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdFlNo.PageSize;
                        serviceUtilityObj.FilterBy = ddlFilterBy.SelectedValue == CommonConstants.SELECT_VALUE_ONE ? null : ddlFilterBy.SelectedValue;
                        serviceUtilityObj.FilterValue = HttpUtility.HtmlEncode(txtSearchBy.Text.Trim());
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.EmployeePK : SortBy;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortAscending : SortDirection;
                       // EmpEmployeeMstObj.empBizUnit = (byte)CurrentUser.CurrentSBUPK;
                        EmpEmployeeMstObj.empPK = 0;
                        EmpEmployeeMstObj.empActive = Convert.ToByte(DbActiveStatus.HASPK);
                        EmpEmployeeMstList = EmployeeServiceClient.GetEmployeeMst(EmpEmployeeMstObj, serviceUtilityObj);
                        serviceUtilityObj = EmployeeServiceClient.GetEmployeeMstCount(EmpEmployeeMstObj, serviceUtilityObj);
                        TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 : (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) : (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        break;

                    case ControlsEnum.EMP:
                        EmpEmployeeMstObj = CommonFunctions.Initilize<EmpEmployeeMst>();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        //EmpEmployeeMstObj.empBizUnit = (byte)CurrentUser.CurrentSBUPK;
                        EmpEmployeeMstObj.empPK = (byte)CurrPK;
                        //AD_FUEL_TRADING_RATE_MSTObj. = Convert.ToByte(DbActiveStatus.HASPK);
                        EmpEmployeeMstList = EmployeeServiceClient.GetEmployeeMst(EmpEmployeeMstObj, serviceUtilityObj);
                        break;
                }
                // UserManagementService.Close();
            }
            catch (Exception ex)
            {
                //UserManagementService.Abort();
                throw ex;
            }
            finally
            {
                EmpEmployeeMstObj = null;
                serviceUtilityObj = null;
                EmployeeServiceClient = null;
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
                    case ControlsEnum.EMP:
                        GetUIValuesFromObject();
                        break;
                    case ControlsEnum.DEFAULT:
                        BindGrid();
                        break;
                    case ControlsEnum.AUTMODE:
                        BindDropDown();
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
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject()
        {

            try
            {
                //assigning the UI controls with the corresponding ListObject value
                if (EmpEmployeeMstList != null && EmpEmployeeMstList.Count > 0)
                {
                    CurrPK = EmpEmployeeMstList[0].empPK;
                    txtname.Text = EmpEmployeeMstList[0].empName.ToString();
                    txtcode.Text = EmpEmployeeMstList[0].empCode.ToString();
                    if (EmpEmployeeMstList[0].empActive >= 0)
                        ddlStatus.SelectedIndex = Convert.ToInt32(ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(EmpEmployeeMstList[0].empActive.ToString())));
                    //if (EmpEmployeeMstList[0].empMod_On != null)
                    //{
                    //    LastModifiedTime = EmpEmployeeMstList[0].empMod_On;
                    //    lblLastModifiedHDR.Text = Resources.gComsRes.LastModifiedOn + LastModifiedTime.ToString(Resources.gComsRes.LastModifiedDatetimeFormat);
                    //    ModifiedDatePnl.Visible = true;
                    //}

                }
                //Concurrency Deleted By Another User
                else
                {
                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Employee);

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
        public void BindGrid()
        {
            try
            {
                if (EmpEmployeeMstList != null)
                {
                    uclPaging.TotalPages = TotalPages;
                    PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                    grdFlNo.DataSource = EmpEmployeeMstList;
                    grdFlNo.DataBind();
                    uclPaging.Visible = true;
                    uclPaging.BindPager();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// To Bind the Authentication Mode
        /// </summary>
        public void BindDropDown()
        {
            //ddlUserType.Items.Clear();
            //ddlUserType.DataTextField = gComs.Utilities.Constants.DA.AccountsMgmt.ConstUserAuth.C_AUTTEXT;
            //ddlUserType.DataValueField = gComs.Utilities.Constants.DA.AccountsMgmt.ConstUserAuth.C_AUTVALUE;
            //ddlUserType.DataSource = dtAutMode;
            //ddlUserType.DataBind();
        }
        /// <summary>
        /// Method used to Reset form Controls
        /// </summary>
        private void ResetForm()
        {
            txtcode.Text = string.Empty;
            txtname.Text = string.Empty;

            CurrPK = 0;
            txtSearchBy.Text = string.Empty;
            ddlFilterBy.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            PageIndex = CommonConstants.SELECT_VALUE_ONE;
            ModifiedDatePnl.Visible = false;
            lblLastModifiedHDR.Text = string.Empty;
        }
        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            bool isSelected;
            isSelected = false;
            try
            {
                foreach (GridViewRow grdrow in grdFlNo.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                    //// check row selected or not
                    if (rbtn.Checked)
                    {
                        // get pk from the grid and assign to CurrPk
                        CurrPK = Convert.ToInt16(grdFlNo.DataKeys[grdrow.RowIndex].Values[0]);
                        // Get Details
                        GetFieldValues(ControlsEnum.EMP);

                        SetFieldValues(ControlsEnum.EMP);
                        isSelected = true;
                        txtcode.Focus();
                        if (Mode == ActionsEnum.VIEW)
                            EntryStatus = EntryStatus.VIEWMODE;
                        else
                            EntryStatus = EntryStatus.ENTRYMODE;

                    }
                }

                if (!isSelected)
                {
                    // if no items selected, Show Error Message
                    litErrorMsg.Text = Resources.Messages.Msg_Not_Selected;
                    EntryStatus = EntryStatus.LISTMODE;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                    ////Concurrency Deleted By Another User
                    //litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                    //litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.User);

                    //EntryStatus = EntryStatus.LISTMODE;
                    //ResetForm();
                    //GetFieldValues(ControlsEnum.DEFAULT);
                    //SetFieldValues(ControlsEnum.DEFAULT);
                    //ModifiedDatePnl.Visible = false;
                    //throw new Exception(litErrorMsg.Text);
                }
                //else
                //{
                //    // if no items selected, Show Error Message
                //    litErrorMsg.Text = Resources.gComsRes.Msg_Not_Selected;
                //    EntryStatus = EntryStatus.LISTMODE;
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>        
        private EmpEmployeeMst SetUIValuesToObject()
        {
            try
            {
                EmpEmployeeMstObj.empPK = (byte)CurrPK;

                EmpEmployeeMstObj.empName = HttpUtility.HtmlEncode(txtname.Text);
                EmpEmployeeMstObj.empCode = HttpUtility.HtmlEncode(txtcode.Text);
                EmpEmployeeMstObj.empActive = Convert.ToByte(ddlStatus.SelectedValue);
                //EmpEmployeeMstObj.empMod_By = Convert.ToInt16(CurrentUser.UserPK);
                //EmpEmployeeMstObj.empMod_On = LastModifiedTime;
                //EmpEmployeeMstObj.empCrtd_By = (byte)CurrentUser.UserPK;
               // EmpEmployeeMstObj.empBizUnit = (byte)CurrentUser.CurrentSBUPK;

                return EmpEmployeeMstObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                EmpEmployeeMstObj = null;
            }
        }
        /// <summary>
        /// Validate for invalid entry in Autocomplete fields
        /// </summary>
        /// <returns></returns>
        private bool ValidateForm()
        {
            bool flag;
            try
            {
                flag = true;
                //if (hdfRegion.Value.Trim() == string.Empty || hdfRegion.Value.Trim() == CommonConstants.SELECT_VALUE_ZERO)
                //{
                //    litErrorMsg.Text = this.GetLocalResourceObject("Msg_Region").ToString();
                //    flag = false;
                //}
                //if (hdfCurrency.Value.Trim() == string.Empty || hdfCurrency.Value.Trim() == CommonConstants.SELECT_VALUE_ZERO)
                //{
                //    litErrorMsg.Text = this.GetLocalResourceObject("Err_Currency").ToString();
                //    flag = false;
                //}
                //if (hdfUOM.Value.Trim() == string.Empty || hdfUOM.Value.Trim() == CommonConstants.SELECT_VALUE_ZERO)
                //{
                //    litErrorMsg.Text = this.GetLocalResourceObject("Err_UOM").ToString();
                //    flag = false;
                //}
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            EmployeeService EmployeeServiceClient;
            EmployeeServiceClient = null;
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
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    commonActions = ActionsEnum.CHANGE;
                }

                switch (commonActions)
                {
                    #region SAVE
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        //else if (!ValidateForm())
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        //}
                        else//valid
                        {
                            EmpEmployeeMstList = new List<EmpEmployeeMst>();
                            EmployeeServiceClient = new EmployeeService();
                            EmployeeServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(EmployeeServiceClient);
                            EmpEmployeeMstObj = CommonFunctions.Initilize<EmpEmployeeMst>();
                            EmpEmployeeMstObj = SetUIValuesToObject();
                            EmpEmployeeMstList.Add(EmpEmployeeMstObj);
                            result = EmployeeServiceClient.SaveEmployeeMst(EmpEmployeeMstList);
                            if (result >= 0) // Success ! re-initialize the page
                            {
                                SortBy = Resources.DataFieldRes.EmployeePK;
                                SortDirection = Resources.Report.SortDescending;
                                litErrorMsg.Text = Resources.Report.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Employee);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Report.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm();
                                GetFieldValues(ControlsEnum.DEFAULT);
                                SetFieldValues(ControlsEnum.DEFAULT);
                                btnNew.Focus();
                            }
                            if (result < 0) // Success ! re-initialize the page
                            {
                                SortBy = Resources.DataFieldRes.EmployeePK;
                                SortDirection = Resources.Report.SortDescending;
                                litErrorMsg.Text = this.GetLocalResourceObject("CodeExist").ToString();
                                //litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.FlightMaster);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Report.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm();
                                GetFieldValues(ControlsEnum.DEFAULT);
                                SetFieldValues(ControlsEnum.DEFAULT);
                                btnNew.Focus();
                            }
                        }
                        break;
                    #endregion

                    #region New
                    case ActionsEnum.NEW:


                        this.txtcode.Focus();
                        ResetForm();
                        ModifiedDatePnl.Visible = false;
                        EntryStatus = EntryStatus.NEWMODE;
                        ddlStatus.SelectedIndex = Convert.ToInt32(ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(CommonConstants.SELECT_VALUE_ONE)));
                        break;
                    #endregion

                    #region Search
                    case ActionsEnum.SEARCH:
                        btnSearch.Focus();
                        PageIndex = CommonConstants.SELECT_VALUE_ONE;
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
                        this.txtcode.Focus();
                        SetUIEditView(commonActions);
                        break;
                    #endregion

                    #region Delete
                    case ActionsEnum.DELETE:
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                        EmpEmployeeMstList = new List<EmpEmployeeMst>();
                        EmployeeServiceClient = new EmployeeService();
                        EmployeeServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(EmployeeServiceClient);
                        EmpEmployeeMstObj = CommonFunctions.Initilize<EmpEmployeeMst>();
                        EmpEmployeeMstObj.empPK = (byte)CurrPK;
                        //EmpEmployeeMstObj.empMod_On = LastModifiedTime;
                        EmpEmployeeMstList.Add(EmpEmployeeMstObj);
                        result = EmployeeServiceClient.DeleteEmployeeMst(EmpEmployeeMstList);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Delete_Success;
                            ResetForm();
                            btnNew.Focus();
                            EntryStatus = EntryStatus.LISTMODE;
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            //UserManagementService.Close();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Employee);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        break;
                    #endregion

                    #region View
                    case ActionsEnum.VIEW:
                        SetUIEditView(commonActions);
                        btnCanel.Focus();
                        break;
                    #endregion

                    #region Print
                    case ActionsEnum.PRINT:
                        EntryStatus = EntryStatus.LISTMODE;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + Resources.Report.Msg_Print_Error + "','" + Resources.Report.Information + "');", true);
                        break;
                    #endregion

                }
            }
            catch (Exception ex)
            {
                //if (commonActions == ActionsEnum.SAVE || commonActions == ActionsEnum.DELETE)
                //UserManagementService.Abort();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Report.Information + "');", true);
            }
            finally
            {
                EmpEmployeeMstList = null;
                EmpEmployeeMstObj = null;
                EmployeeServiceClient = null;
            }
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
                    //Toggle the sort expression
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
                this.PageIndex = CommonConstants.SELECT_VALUE_ONE;
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Report.Information + "');", true);
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
            uclPaging.CurrentPage = 1;
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
            this.btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCanel.PreRender += new EventHandler(btnAction_PreRender);
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {
            //base.CheckBtnVisibility(sender);
        }
        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            //this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
            //this.Init += new EventHandler(this.Page_Init);
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
                //switch (e.Action)
                //{
                //    case NavigationEnum.PAGECHANGE:
                //        uclPaging.CurrentPage = e.CurrentPage;
                //        break;
                //    case NavigationEnum.FIRST:
                //        // Assignment the First page index.
                //        if (e.CurrentPage > 1)
                //            uclPaging.CurrentPage = 1;
                //        break;
                //    case NavigationEnum.LAST:
                //        // Assignment the last page index.
                //        if (e.CurrentPage <= e.TotalPages)
                //            uclPaging.CurrentPage = e.TotalPages;
                //        break;
                //    case NavigationEnum.NEXT:
                //        // Increment the next page index.
                //        if (e.CurrentPage <= e.TotalPages)
                //            uclPaging.CurrentPage++;
                //        break;
                //    case NavigationEnum.PREVIOUS:
                //        // Decrement the previous page index.
                //        if (e.CurrentPage > 1)
                //            uclPaging.CurrentPage--;
                //        break;
                //}
                PageIndex = uclPaging.CurrentPage.ToString();
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EnableDisableButtons(e.TotalPages);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Report.Information + "');", true);
            }
        }
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            // Should we disable the first link?
           // uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we disable the previous link?
            //uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we enable the next link?
            //uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            // Should we enable the last link?
            //uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
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
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "InitComponents();", true);

        }
        #endregion

        #region ControlEnum
        /// <summary>
        /// Enum Settings for current page
        /// </summary>
        public enum ControlsEnum
        {
            EMP,
            DEFAULT,
            AUTMODE
        }
        # endregion
    }
}