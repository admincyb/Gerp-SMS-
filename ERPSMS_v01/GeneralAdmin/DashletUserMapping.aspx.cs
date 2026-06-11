using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject.Administration.Masters;
using BusinessObject;
using System.Data;
using BusinessObject.CommonManagement;
using System.Configuration;
using ERPSMS_v01.UserControls;
using BusinessObject.AccountManagement;

namespace ERPSMS_v01.GeneralAdmin
{
    public partial class DashletUserMapping : ERP.Store.UI.MyBasePage//: System.Web.UI.Page
    {
        #region Variables and Properties
        #region  Properties
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
        private List<DashletUserMappingDetails> DashletUserMappingDetailsList
        {
            get
            {
                return (List<DashletUserMappingDetails>)ViewState["DashletUserMappingDetails"];
            }
            set
            {
                ViewState["DashletUserMappingDetails"] = value;
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

        #endregion
        #region  Variables
        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        User currentUser;
        private DataTable dtResult;
        private DataTable dtDept;
        private DataSet dsDept;
        private DataTable dtUserRoles;
        private DataTable dtType;
        private DataTable dtModule;
        DataTable dtCompany;
        private DashletUserMappingHeader objDashletUserMappingHeader;
        private DashletUserMappingDetails objUserMappingDetails;
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            // InitializeComponent();
            if (!IsPostBack)
            {
                PageActionHandler();
            }
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
                PageIndex = Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE);
                uclPaging.TotalPages = TotalPages;
                uclPaging.CurrentPage = 1;
                dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                if (dtCompany != null && dtCompany.Rows.Count > 0)
                {
                    hdfCompany.Value = dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString();
                }
                hdfAdvSearch.Value = "0";
                EntryStatus = EntryStatus.LISTMODE;
                GetFieldValues(ControlsEnum.TYPE);
                SetFieldValues(ControlsEnum.TYPE);
                GetFieldValues(ControlsEnum.MODULE);
                SetFieldValues(ControlsEnum.MODULE);
                GetFieldValues(ControlsEnum.LIST);
                SetFieldValues(ControlsEnum.LIST);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }



        /// <summary>
        /// To handle OnInit event Used to assign the Event for all the actions used in this page
        /// Leave this section if using Master Screens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
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
                    #region LIST
                    case ControlsEnum.LIST:
                        dtResult = BusinessLogic.Administration.Masters.DashletUserMappingBL.GetDashletUserMappingList(Convert.ToInt16(currentUser.SBUID), PageIndex,
                            Convert.ToInt32(GetLocalResourceObject("PageSize")), txtNameFilterList.Text, Convert.ToInt32(ddlTypeFilter.SelectedValue), Convert.ToInt32(ddlModuleFilter.SelectedValue));
                        break;
                    #endregion

                    #region Get Dashboard Master Details
                    case ControlsEnum.GETUSERMAPPINGBYPK:
                        objDashletUserMappingHeader = BusinessLogic.Administration.Masters.DashletUserMappingBL.DashletUserRolesByPK(CurrPK);
                        DashletUserMappingDetailsList = objDashletUserMappingHeader.DashletUserMappingDetails;
                        break;
                    #endregion
                    #region Department
                    case ControlsEnum.DEPT:
                        dsDept = BusinessLogic.Administration.Configurations.UserManagementBL.GetDepartment(currentUser.SBUID);
                        //dtDept = BusinessLogic.CommonManagement.CommonBL.GetDepartment(currentUser.PKUser, currentUser.SBUID);
                        break;
                    #endregion

                    #region USER ROLES
                    case ControlsEnum.USERROLES:
                        dtUserRoles = BusinessLogic.Administration.Masters.DashletUserMappingBL.GetUserRoles(Convert.ToInt32(ddlDept.SelectedValue));
                        break;
                    #endregion

                    #region TYPE
                    case ControlsEnum.TYPE:
                        dtType = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, GetLocalResourceObject("TypeConfigMenu").ToString());
                        break;
                    #endregion
                    #region MODULE
                    case ControlsEnum.MODULE:
                        dtModule = BusinessLogic.CommonManagement.CommonBL.GetModule(0,1,1,currentUser.SBUID);
                        break;
                    #endregion

                    default:
                        break;
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

                    #region LIST
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion

                    #region Dept
                    case ControlsEnum.DEPT:
                        BindDropDown(ControlsEnum.DEPT);
                        break;
                    #endregion

                    #region USER ROLES
                    case ControlsEnum.USERROLES:
                        BindGrid(ControlsEnum.USERROLES);
                        break;
                    #endregion

                    #region SAVEUSERMAPPINGTOGGRID
                    case ControlsEnum.SAVEUSERMAPPINGTOGRID:
                        BindGrid(ControlsEnum.SAVEUSERMAPPINGTOGRID);
                        break;
                    #endregion

                    #region TYPE
                    case ControlsEnum.TYPE:
                        BindDropDown(ControlsEnum.TYPE);
                        break;
                    #endregion
                    #region MODULE
                    case ControlsEnum.MODULE:
                        BindDropDown(ControlsEnum.MODULE);
                        break;
                    #endregion

                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion

        #region Helper Methods
        #region Set UIValues To Object
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            object returnObject = new object();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region DASHLET HDR
                    case ControlsEnum.DASHLETHDR:

                        DashletUserMappingHeader tempDashboardHeader = new DashletUserMappingHeader();
                        tempDashboardHeader.DLC_PK = CurrPK;
                        //tempDashboardHeader.IND_MOD_DT = LastModifiedTime;
                        //tempDashboardHeader.IND_USER = currentUser.PKUser;
                        //SetUIValuesToObject(ControlsEnum.DASHLETUSERROLESTOLIST);
                        tempDashboardHeader.DashletUserMappingDetails = DashletUserMappingDetailsList;
                        returnObject = tempDashboardHeader;
                        break;
                    #endregion

                    //#region DASHLET USER ROLES TO LIST
                    //case ControlsEnum.DASHLETUSERROLESTOLIST:
                    //    DashletUserMappingDetailsList = new List<DashletUserMappingDetails>();
                    //    foreach (GridViewRow grdrow in grdUserRolesList.Rows)
                    //    {
                    //        HiddenField hdfDLM_PK = (HiddenField)grdrow.FindControl("hdfDLM_PK");
                    //        HiddenField hdfDLM_DASHLET = (HiddenField)grdrow.FindControl("hdfDLM_DASHLET");
                    //        HiddenField hdfDLM_USER_GROUP = (HiddenField)grdrow.FindControl("hdfDLM_USER_GROUP");
                    //        HiddenField hdfDLM_VIEW = (HiddenField)grdrow.FindControl("hdfDLM_VIEW");
                    //        HiddenField hdfDLM_ACTION = (HiddenField)grdrow.FindControl("hdfDLM_ACTION");

                    //        DashletUserMappingDetails temp_DashletUserDetails = new DashletUserMappingDetails();
                    //        temp_DashletUserDetails.DLM_PK = Convert.ToInt32(hdfDLM_PK.Value);
                    //        temp_DashletUserDetails.DLM_DASHLET = Convert.ToInt32(hdfDLM_DASHLET.Value);
                    //        temp_DashletUserDetails.DLM_USER_GROUP = Convert.ToInt32(hdfDLM_USER_GROUP.Value);
                    //        temp_DashletUserDetails.DLM_VIEW = Convert.ToInt32(hdfDLM_VIEW.Value);
                    //        temp_DashletUserDetails.DLM_ACTION = Convert.ToInt32(hdfDLM_ACTION.Value);
                    //        DashletUserMappingDetailsList.Add(temp_DashletUserDetails);
                    //    }
                    //    break;
                    //#endregion


                    #region SAVE USER MAPPING TO GGRID
                    case ControlsEnum.SAVEUSERMAPPINGTOGRID:
                        if (DashletUserMappingDetailsList != null && DashletUserMappingDetailsList.Count != 0)
                        {
                            // DashboardItemDetailsList.RemoveAll(x => x.DBL_SL_NO == Convert.ToInt32(hdfCurGroupSlNo_PopUp.Value) && x.DBL_DPT == Convert.ToInt32(ddlDeptPopUp.SelectedValue));
                        }
                        else
                        {
                            DashletUserMappingDetailsList = new List<DashletUserMappingDetails>();
                        }
                        foreach (GridViewRow grdrow in grdUserRoles.Rows)
                        {
                            CheckBox chkMenu_popUp = (CheckBox)grdrow.FindControl("chkUserRoles");
                            if (chkMenu_popUp.Checked == true)
                            {
                                DashletUserMappingDetailsList.RemoveAll(x => x.DLM_DASHLET == CurrPK
                                    && x.DLM_USER_GROUP == Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfusgPK")).Value));

                                RadioButton rbtView = (RadioButton)grdrow.FindControl("rbtView");
                                RadioButton rbtAction = (RadioButton)grdrow.FindControl("rbtAction");

                                objUserMappingDetails = new DashletUserMappingDetails();
                                objUserMappingDetails.DLM_DASHLET = CurrPK;
                                objUserMappingDetails.DLM_USER_GROUP = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfusgPK")).Value);
                                objUserMappingDetails.DLM_USER_GROUP_TEXT = ((Label)grdrow.FindControl("lblUserRoleText")).Text;
                                objUserMappingDetails.DLM_VIEW = rbtView.Checked == true ? 1 : 0;
                                objUserMappingDetails.DLM_ACTION = rbtAction.Checked == true ? 1 : 0;
                                DashletUserMappingDetailsList.Add(objUserMappingDetails);
                            }
                        }

                        break;
                    #endregion

                }
                return returnObject;
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
        #region Get UIValues From Object
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.DASHLETUSERROLESDETAILS:
                        if (objDashletUserMappingHeader != null)
                        {
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Bind DropDown
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {

                #region Dept
                case ControlsEnum.DEPT:
                    ddlDept.Items.Clear();
                    if (dsDept != null && dsDept.Tables[0].Rows.Count > 0)
                    {
                        ddlDept.DataSource = dsDept;
                        ddlDept.DataTextField = GTIService.Constants.Common.Common.F_DEPARTMENTNAME;
                        ddlDept.DataValueField = GTIService.Constants.Common.Common.F_DEPARTMENT;
                        ddlDept.DataBind();
                    }
                    ddlDept.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion

                #region TYPE
                case ControlsEnum.TYPE:
                    ddlTypeFilter.Items.Clear();
                    if (dtType != null && dtType.Rows.Count > 0)
                    {
                        ddlTypeFilter.DataSource = dtType;
                        ddlTypeFilter.DataTextField = GTIService.Constants.Common.Common.CFG_DATA_FIELD;
                        ddlTypeFilter.DataValueField = GTIService.Constants.Common.Common.CFG_VALUE_VALUE_FIELD;
                        ddlTypeFilter.DataBind();
                    }
                    ddlTypeFilter.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region MODULE
                case ControlsEnum.MODULE:
                    ddlModuleFilter.Items.Clear();
                    if (dtModule != null && dtModule.Rows.Count > 0)
                    {
                        ddlModuleFilter.DataSource = dtModule;
                        ddlModuleFilter.DataTextField = GTIService.Constants.Common.Common.MOD_NAME;
                        ddlModuleFilter.DataValueField = GTIService.Constants.Common.Common.MOD_PK;
                        ddlModuleFilter.DataBind();
                    }
                    ddlModuleFilter.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                default:
                    break;
            }
        }
        #endregion
        #region BindGrid
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        uclPaging.Visible = false;
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            int rowCount = 0;
                            rowCount = Convert.ToInt32(dtResult.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                            uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                              (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                              (rowCount / this.PageSize) + 1;
                            PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdList.DataSource = dtResult;
                            grdList.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            grdList.DataSource = null;
                            grdList.DataBind();
                        }
                        break;
                    #endregion
                    #region USER ROLES
                    case ControlsEnum.USERROLES:
                        if (dtUserRoles != null)
                        {
                            grdUserRoles.DataSource = dtUserRoles;
                            grdUserRoles.DataBind();
                        }
                        else
                        {
                            grdUserRoles.DataSource = null;
                            grdUserRoles.DataBind();
                        }
                        break;
                    #endregion
                    #region SAVE USER MAPPING TO GRID
                    case ControlsEnum.SAVEUSERMAPPINGTOGRID:
                        if (DashletUserMappingDetailsList != null)
                        {
                            grdUserRolesList.DataSource = DashletUserMappingDetailsList;
                            grdUserRolesList.DataBind();
                        }
                        else
                        {
                            grdUserRolesList.DataSource = null;
                            grdUserRolesList.DataBind();
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
        #region Reset Form
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                # region CLEAR
                case ControlsEnum.CLEAR:
                    CurrPK = 0;
                    txtNameFilterList.Text = string.Empty;
                    if (ddlDept.Items.Count > 0)
                        ddlDept.SelectedIndex = 0;

                    uclPaging.CurrentPage = 0;
                    PageIndex = 1;
                    DashletUserMappingDetailsList = null;
                    break;
                #endregion
                # region CLEAR FILTER
                case ControlsEnum.CLEARFILTER:
                    txtNameFilterList.Text = string.Empty;
                    if (ddlTypeFilter.Items.Count > 0)
                        ddlTypeFilter.SelectedIndex = 0;
                    if (ddlModuleFilter.Items.Count > 0)
                        ddlModuleFilter.SelectedIndex = 0;
                    CurrPK = 0;
                    uclPaging.CurrentPage = 0;
                    this.PageIndex = 1;
                    this.EntryStatus = EntryStatus.LISTMODE;
                    break;
                #endregion
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

        #region ActionHandler
        /// <summary>
        /// Button actions events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                int? result;
                bool bIsChecked = false;
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
                    if (((DropDownList)sender).ID == "ddlDeptPopUp")
                    {
                        commonActions = ActionsEnum.CHANGE;
                    }
                    else if (((DropDownList)sender).ID == "ddlMISRptPopup")
                    {
                        commonActions = ActionsEnum.MISCHANGE;
                    }
                    else if (((DropDownList)sender).ID == "ddlDept")
                    {
                        commonActions = ActionsEnum.VIEW_ACTION;
                    }
                }
                switch (commonActions)
                {
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            int itmCount = 0;
                            foreach (GridViewRow grow in grdUserRoles.Rows)
                            {
                                CheckBox chkUserRoles = (CheckBox)grow.FindControl("chkUserRoles");
                                if (chkUserRoles.Checked)
                                {
                                    itmCount++;
                                }
                            }
                            if (itmCount > 0)
                            {
                                litErrorMsg.Text = Resources.Report.Err_AddRole;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                            else
                            {
                                objDashletUserMappingHeader = new DashletUserMappingHeader();
                                objDashletUserMappingHeader = (DashletUserMappingHeader)SetUIValuesToObject(ControlsEnum.DASHLETHDR);
                                if (objDashletUserMappingHeader != null)
                                {
                                    //if (objDashletUserMappingHeader.DashletUserMappingDetails != null && objDashletUserMappingHeader.DashletUserMappingDetails.Count > 0)
                                    //{
                                    string xmlDoc = CommonFunctions.XmlSerialize<DashletUserMappingHeader>(objDashletUserMappingHeader);
                                    result = BusinessLogic.Administration.Masters.DashletUserMappingBL.SaveDashletUserDetails(xmlDoc);
                                    if (result > 0)
                                    {
                                        litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DashletUserRole);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                        ResetForm(ControlsEnum.CLEAR);
                                        EntryStatus = EntryStatus.LISTMODE;
                                        GetFieldValues(ControlsEnum.LIST);
                                        SetFieldValues(ControlsEnum.LIST);
                                    }
                                    else
                                    {
                                        if (result == (int)DbSaveStatus.SQLERROR)
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.DashletUserRole + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.DashletUserRole + " " + Resources.Messages.AlreadyDeleted;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_NameExist").ToString());
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DashletUserRole);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region DETAIL
                    case ActionsEnum.DETAIL:
                    case ActionsEnum.EDIT:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDLC_PK_List")).Value);
                                lblDashletHd.Text = ((Label)grdrow.FindControl("lblGvNameList")).Text;
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ddlDept.Focus();
                            EntryStatus = EntryStatus.EDITMODE;
                            GetFieldValues(ControlsEnum.DEPT);
                            SetFieldValues(ControlsEnum.DEPT);
                            GetFieldValues(ControlsEnum.USERROLES);
                            SetFieldValues(ControlsEnum.USERROLES);
                            GetFieldValues(ControlsEnum.GETUSERMAPPINGBYPK);
                            SetFieldValues(ControlsEnum.SAVEUSERMAPPINGTOGRID);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region  User Rolles Save To Grid
                    case ActionsEnum.ITEMSAVE:
                        SetUIValuesToObject(ControlsEnum.SAVEUSERMAPPINGTOGRID);
                        SetFieldValues(ControlsEnum.SAVEUSERMAPPINGTOGRID);
                        GetFieldValues(ControlsEnum.USERROLES);
                        SetFieldValues(ControlsEnum.USERROLES);
                        break;
                    #endregion
                    #region VIEW_ACTION
                    case ActionsEnum.VIEW_ACTION:
                        GetFieldValues(ControlsEnum.USERROLES);
                        SetFieldValues(ControlsEnum.USERROLES);
                        btnViewDashlet.Focus();
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                    case ActionsEnum.LIST:
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        CurrPK = 0;
                        string LastModDate = string.Empty;
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDLC_PK_List")).Value);
                                //  LastModDate = (((HiddenField)grdrow.FindControl("hdfModDate_List")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            result = 0;
                            result = BusinessLogic.Administration.Masters.DashboardSetupBL.DeleteRecord(CurrPK, LastModDate);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry 
                                if (grdList.Rows.Count == 1 && PageIndex > 1)
                                {
                                    PageIndex--;
                                }
                                GetFieldValues(ControlsEnum.LIST);
                                SetFieldValues(ControlsEnum.LIST);
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DashletUserRole);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
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
                                    litErrorMsg.Text = Resources.PageNameRes.DashletUserRole + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.DashletUserRole + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.DashletUserRole + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DashletUserRole);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
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
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.CLEARFILTER);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
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
        #endregion

        #region ActionHandler
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
            GridView senderGridView = (GridView)sender;

            if (senderGridView.ID == "grdUserRolesList")
            {
                if (e.CommandName == "DELETE_ACTION")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfDLM_USER_GROUP = row.FindControl("hdfDLM_USER_GROUP") as HiddenField;

                    DashletUserMappingDetails detail = DashletUserMappingDetailsList
                        .Where(x => x.DLM_USER_GROUP == Convert.ToInt32(hdfDLM_USER_GROUP.Value) && x.DLM_DASHLET == CurrPK)
                        .SingleOrDefault();
                    if (detail != null)
                    {
                        DashletUserMappingDetailsList.Remove(detail);
                        SetFieldValues(ControlsEnum.SAVEUSERMAPPINGTOGRID);
                    }
                    else
                    {
                        litErrorMsg.Text = Resources.Messages.ActionFailed;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }

                }

                else if (e.CommandName == "DELETE_ACTION_ALL")
                {
                    //foreach (GridViewRow grdrow in grdUserRolesList.Rows)
                    //{
                    //    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    //    HiddenField hdfDLM_USER_GROUP = row.FindControl("hdfDLM_USER_GROUP") as HiddenField;

                    //    DashletUserMappingDetails detail = DashletUserMappingDetailsList
                    //        .Where(x => x.DLM_USER_GROUP == Convert.ToInt32(hdfDLM_USER_GROUP.Value) && x.DLM_DASHLET == CurrPK)
                    //        .SingleOrDefault();

                    //    if (detail != null)
                    //    {
                    //        DashletUserMappingDetailsList.Remove(detail);
                    //    }
                    //    else
                    //    {
                    //        litErrorMsg.Text = Resources.Messages.ActionFailed;
                    //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    //            + "','" + Resources.ErpRes.Information + "');", true);
                    //    }

                    //}
                    DashletUserMappingDetailsList.Clear();
                    SetFieldValues(ControlsEnum.SAVEUSERMAPPINGTOGRID);
                }
            }
        }

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
                    PageIndex = uclPaging.CurrentPage;
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
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex;
        }
        #endregion

        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            LIST,
            CLEAR,
            EDIT,
            DASHLETHDR,
            DEPT,
            SAVEUSERMAPPINGTOGRID,
            SAVEDASHLETMAPPINGGRID,
            DASHLETMAPPINGGRIDLIST,
            CLEARFILTER,
            DASHLETUSERROLESTOLIST,
            DASHLETUSERROLESDETAILS,
            USERROLES,
            GETUSERMAPPINGBYPK,
            MODULE,
            TYPE,
           
        }
        #endregion
    }
}