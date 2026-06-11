using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject;
using System.Data;
using ERPSMS_v01.UserControls;
using BusinessObject.Administration.Masters;
//using BusinessObject.CommonManagement;
using BusinessLogic.Administration.Masters;
using ERPService;
using ERPData;
using ERPManager;
using ERPSMS_v01.Administration.Masters;


namespace ERPSMS_v01.GeneralAdmin
{
    public partial class ActivityMaster : ERP.Store.UI.MyBasePage
    {
        #region Properties & Variables

        #region Porperties

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

        private int SlNo
        {
            get
            {
                return this.ViewState[ViewstateStrings.SlNo] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.SlNo]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SlNo] = value;
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
        /// 
        /// </summary>
        private bool IsMappedAccounts
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsMappedAccounts] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsMappedAccounts].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsMappedAccounts] = value;
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
        private int PageSize1
        {
            get
            {
                return Convert.ToInt32(GetLocalResourceObject("PageSize1").ToString());
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
        private ActivityMasterBO objActivity
        {
            get
            {
                return Session["ObjActivity"] == null ? null : (ActivityMasterBO)Session["ObjActivity"];
            }
            set
            {
                Session["ObjActivity"] = value;
            }
        }
        #endregion

        #region Variables

        private ActionsEnum commonActions;
        private ControlsEnum controlEnum;
        User currentUser;
        private ADM_COMPANY_MST admCompanyMstObj;
        private DataTable dtActivityList;
        private DataTable dtResult;
        private DataTable dtGroup;
        private DataTable dtDepartment;
        private DataTable dtMainActivity;
        private DataTable dtAllMainActivity;
        private DataTable dtSubDepartment;
        private DataTable dtCostCenterAccounts;
        private DataTable dtCostCenterAccounts_Map;
        //private DataTable admCompanyMstList;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        private CostCenterMasterBO objCostCenter;

        private DataTable dtCompany;
        private int flag = 0;

        #endregion

        #endregion

        #region Page Level Events

        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                if (EntryStatus == EntryStatus.EDITMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkMapping.CssClass = GetLocalResourceObject("TabInActive").ToString();

                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkMapping.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkMapping.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkMapping.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
                else if (EntryStatus == EntryStatus.ALLOCATEMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(2);", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkMapping.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){InitComponent();ShowHideAdvancedSearch(1);});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

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
                    ConfigurationSettings();
                    GetFieldValues(ControlsEnum.ALLMAINACTIVITY);
                    SetFieldValues(ControlsEnum.ALLMAINACTIVITY);
                    GetFieldValues(ControlsEnum.MAINACTIVITY);
                    SetFieldValues(ControlsEnum.MAINACTIVITY);
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(BusinessObject.CommonManagement.DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                    if (dtCompany != null && dtCompany.Rows.Count > 0)
                    {
                        hdfCompany.Value = dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString();
                    }
                    PageIndex = Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

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

        #endregion

        #region Action Handler

        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept("../../login.aspx"))
                return;

            try
            {
                int? result;
                int GridRowIndex = 0;
                GridViewRow gvRow;

                bool bIsChecked = false;

                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(CheckBox)))
                {
                    if (((CheckBox)sender).ID == "chkMainActivity")
                    {
                        commonActions = ActionsEnum.CHANGE;
                    }
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
                    if (((DropDownList)sender).ID == "ddlDepartment")
                    {
                        commonActions = ActionsEnum.DEPTCHANGED;
                    }
                }

                switch (commonActions)
                {
                    #region CHAGNE
                    case ActionsEnum.CHANGE:
                        if (chkMainActivity.Checked == false)
                        {
                            vrfMainActivity.Enabled = true;
                            ddlMainActivity.Visible = true;
                        }
                        else
                        {
                            vrfMainActivity.Enabled = false;
                            ddlMainActivity.Visible = false;
                            ddlMainActivity.SelectedIndex = 0;

                        }
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            objActivity = (ActivityMasterBO)SetUIValuesToObject(ControlsEnum.SAVEACTIVITY);
                            if (objActivity != null)
                            {
                                if (objActivity.EAM_MAIN_ACTIVITY == 1)
                                {
                                    if (objActivity.MapDetails != null)
                                    {
                                        if (objActivity.MapDetails.Count != 0)
                                        {
                                            if (hdfIscontYes.Value == "0")
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowConfirmation", "$(document).ready(function(){ShowConfirmation();});", true);
                                                break;
                                            }
                                            objActivity.MapDetails = null;
                                        }
                                    }
                                }
                                string xmlDoc = CommonFunctions.XmlSerialize<ActivityMasterBO>(objActivity);
                                result = CostCenterMasterBL.SaveActivityMaster(xmlDoc);
                                if (result > 0)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_CostCenterSaveSuccess").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                    ResetForm(ControlsEnum.CLEAR);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    CurrPK = (int)result;
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
                                        litErrorMsg.Text = Resources.PageNameRes.CostCenterMaster + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                    }
                                    else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.CostCenterMaster + " " + Resources.Messages.AlreadyDeleted;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        EntryStatus = EntryStatus.LISTMODE;
                                    }
                                    else if (result == (int)DbSaveStatus.ALREADYDELETED)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.CostCenterMaster + " " + string.Format(GetLocalResourceObject("Msg_AlreadyDeleted").ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_CodeExist").ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.NAMEEXIST)
                                    {
                                        litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_NameExist").ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.DATEOVERLAP)//-4 cost center used in PR
                                    {
                                        litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_CostCenterUsedInPR").ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.ALREADYMAPPED)//-40 Main Activity already mapped to sub activity
                                    {
                                        litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_MainActivityMappedToSUbActivity").ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CostCenterMaster);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                    }
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_AddCostCenter").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                            }
                            // }
                        }
                        break;
                    #endregion
                    #region NEW
                    case ActionsEnum.NEW:
                        EntryStatus = EntryStatus.NEWMODE;
                        objActivity = null;
                        txtCode.Focus();
                        CurrPK = 0;
                        GetFieldValues(ControlsEnum.MAINACTIVITY);
                        SetFieldValues(ControlsEnum.MAINACTIVITY);
                        ResetForm(ControlsEnum.CLEAR);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideACDetails", "HideACDetails();", true);
                        //lnkShowActivityAccounts.Visible = false;
                        break;
                    #endregion
                    #region EDIT
                    #region Mapping
                    case ActionsEnum.MAPPING:
                        if (CurrPK != 0)
                        {
                            if (chkMainActivity.Checked)
                            {
                                litErrorMsg.Text = Resources.Report.Msg_Cannot_Map_To_Team;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                            }
                            else
                            {

                                EntryStatus = EntryStatus.ALLOCATEMODE;
                                GetFieldValues(ControlsEnum.GETGROUP);
                                SetFieldValues(ControlsEnum.GETGROUP);
                                GetFieldValues(ControlsEnum.COMPANY);
                                SetFieldValues(ControlsEnum.COMPANY);
                                GetFieldValues(ControlsEnum.GETDEPARTMENT);
                                SetFieldValues(ControlsEnum.GETDEPARTMENT);
                                BindGrid(ControlsEnum.COSTCENTERMAP);
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideACDetails", "HideACDetails();", true);
                        ddlDepartment.Focus();
                        break;
                    #endregion
                    case ActionsEnum.EDIT:
                    case ActionsEnum.DETAILS:
                        if (IsRadioButtonSelected(ref GridRowIndex))
                        {
                            this.PageIndex = 1;
                            txtCode.Focus();

                            CurrPK = Convert.ToInt32((grdActivityList.Rows[GridRowIndex].FindControl("hdfActivityPk") as HiddenField).Value);
                            GetFieldValues(ControlsEnum.MAINACTIVITY);
                            SetFieldValues(ControlsEnum.MAINACTIVITY);
                            GetFieldValues(ControlsEnum.EDIT);
                            SetFieldValues(ControlsEnum.EDIT);
                            GetFieldValues(ControlsEnum.GETACTIVITY);
                            SetFieldValues(ControlsEnum.GETACTIVITY);
                            EntryStatus = EntryStatus.EDITMODE;

                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideACDetails", "HideACDetails();", true);
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ResetForm(ControlsEnum.CLEAR);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region DEPTCHANGED
                    case ActionsEnum.DEPTCHANGED:
                        GetFieldValues(ControlsEnum.GETSUBDEPARTMENT);
                        SetFieldValues(ControlsEnum.GETSUBDEPARTMENT);
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = CostCenterMasterBL.DeleteActivity(CurrPK, this.LastModifiedTime);
                        if (result > 0)
                        {
                            if (grdActivityList.Rows.Count == 1 && PageIndex > 1)
                            {
                                PageIndex--;
                            }
                            ResetForm(ControlsEnum.CLEAR);
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            EntryStatus = EntryStatus.LISTMODE;
                            btnNew.Focus();
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Activity").ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        else
                        {
                            #region Error Message
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("TeamMapping").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.REFERRED)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("TeamMapping").ToString() + " " + Resources.Messages.UsedInAnotherPlace;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("TeamMapping").ToString() + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYMAPPED)//-40 Main Activity already mapped to sub activity
                            {
                                litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_MainActivityMappedToSUbActivity").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            #endregion
                        }
                        break;
                    #endregion
                    #region ACTIVATE
                    case ActionsEnum.ACTIVATE:
                        gvRow = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        result = CostCenterMasterBL.UpdateActivityStatus(Convert.ToInt32(((HiddenField)gvRow.FindControl("hdfActivityPk")).Value), (int)BusinessObject.CommonManagement.DbActiveStatus.ACTIVE, currentUser.PKUser, null);
                        if (result > 0)
                        {
                            uclPaging.CurrentPage = 0;
                            this.PageIndex = 1;
                            this.CurrPK = 0;
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Activate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ActivityMaster.ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        else
                        {
                            BusinessObject.CommonManagement.DBActiveInactiveStatus dBActiveInactiveStatus = (BusinessObject.CommonManagement.DBActiveInactiveStatus)(result);
                            switch (dBActiveInactiveStatus)
                            {
                                // For Sql Error
                                case BusinessObject.CommonManagement.DBActiveInactiveStatus.SQLERROR:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;

                                case BusinessObject.CommonManagement.DBActiveInactiveStatus.CONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Modified;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CostCenterMaster.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case BusinessObject.CommonManagement.DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CostCenterMaster.ToString());
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
                    #region INACTIVATE
                    case ActionsEnum.INACTIVATE:
                        gvRow = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        result = CostCenterMasterBL.UpdateActivityStatus(Convert.ToInt32(((HiddenField)gvRow.FindControl("hdfActivityPk")).Value), (int)BusinessObject.CommonManagement.DbActiveStatus.INACTIVE, currentUser.PKUser, null);
                        if (result > 0)
                        {
                            uclPaging.CurrentPage = 0;
                            this.PageIndex = 1;
                            this.CurrPK = 0;
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_InActivate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ActivityMaster.ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        else
                        {
                            // if error or exception occur
                            BusinessObject.CommonManagement.DBActiveInactiveStatus dBActiveInactiveStatus = (BusinessObject.CommonManagement.DBActiveInactiveStatus)(result);
                            switch (dBActiveInactiveStatus)
                            {
                                // For Sql Error
                                case BusinessObject.CommonManagement.DBActiveInactiveStatus.SQLERROR:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;

                                case BusinessObject.CommonManagement.DBActiveInactiveStatus.CONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Modified;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CostCenterMaster.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case BusinessObject.CommonManagement.DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CostCenterMaster.ToString());
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
                    #region SEARCH
                    case ActionsEnum.SEARCH:
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
                    case ActionsEnum.LIST:
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        uclPaging.CurrentPage = 0;
                        this.PageIndex = 1;
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region ADD
                    case ActionsEnum.ADD:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            int AddResult = AddCostCenter(ddlDepartment.SelectedValue, ddlCompany.SelectedValue, hdfTeam.Value);
                            if (AddResult > 0)
                            {
                                BindGrid(ControlsEnum.COSTCENTERMAP);
                                ResetForm(ControlsEnum.COSTCENTERMAP);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_CostCenterExist").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region Edit Cost Center
                    case ActionsEnum.EDITCOSTCENTER:
                        SlNo = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                        EditRow(SlNo);
                        break;
                    #endregion
                    #region Clear Cost Center
                    case ActionsEnum.REMOVECOSTCENTER:
                        SlNo = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                        DeleteRow(SlNo);
                        BindGrid(ControlsEnum.COSTCENTERMAP);
                        ResetForm(ControlsEnum.COSTCENTERMAP);
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void DeleteRow(int slNo)
        {
            if (objActivity != null && objActivity.MapDetails != null)
            {
                //MapDetails objMapDetails = objActivity.MapDetails.FirstOrDefault(a => a.SL_NO == slNo);
                objActivity.MapDetails.RemoveAll(a => a.SL_NO == slNo);
            }
        }
        private void EditRow(int slNo)
        {
            if (objActivity != null && objActivity.MapDetails != null)
            {
                MapDetails objMapDetails = objActivity.MapDetails.FirstOrDefault(a => a.SL_NO == slNo);
                ddlCompany.SelectedValue = (objMapDetails.ACM_COMPANY_PK).ToString();
                ddlDepartment.SelectedValue = (objMapDetails.ACM_DEPT_PK).ToString();
                hdfTeam.Value = (objMapDetails.ACM_TEAM_PK).ToString();
                txtTeam.Text = (objMapDetails.ACM_TEAM_TEXT).ToString();
                //ddlGroup.SelectedValue = (objMapDetails.ACM_GROUP_PK).ToString();
            }
        }
        private int AddCostCenter(string department, string location, string team)
        {
            int result = 1;

            if (objActivity.MapDetails != null)
                //Duplicate Item Checking
                objActivity.MapDetails.ForEach(a =>
                {
                    if (a.ACM_DEPT_PK == Convert.ToInt32(department) && a.ACM_COMPANY_PK == Convert.ToInt32(location) && a.ACM_TEAM_PK == Convert.ToInt32(team) && a.SL_NO != SlNo)
                        result = -1;
                });

            if (result == 1)
            {
                int slno = 1;
                if (objActivity.MapDetails == null || objActivity.MapDetails.Count == 0)
                    objActivity.MapDetails = new List<MapDetails>();
                else
                {
                    slno = objActivity.MapDetails.Max(s => s.SL_NO) + 1;
                }
                if (SlNo > 0)
                {
                    //in case of update
                    objActivity.MapDetails.ForEach(e =>
                    {
                        if (e.SL_NO == SlNo)
                        {
                            e.ACM_COMPANY_PK = Convert.ToInt32(location);
                            e.ACM_COMPANY_TEXT = ddlCompany.SelectedItem.Text;
                            e.ACM_DEPT_PK = Convert.ToInt32(ddlDepartment.SelectedValue);
                            e.ACM_DEPT_TEXT = ddlDepartment.SelectedItem.Text;
                            e.ACM_TEAM_PK = Convert.ToInt32(hdfTeam.Value);
                            e.ACM_TEAM_TEXT = txtTeam.Text;
                            //e.ACM_GROUP_PK = Convert.ToInt32(ddlGroup.SelectedValue);
                            //e.ACM_GROUP_TEXT = ddlGroup.SelectedItem.Text;
                        }
                    });
                }
                else
                {
                    //in case of insert
                    objActivity.MapDetails.Add(new MapDetails()
                    {
                        SL_NO = slno,
                        EAM_PK = CurrPK,
                        ACM_COMPANY_PK = Convert.ToInt32(location),
                        ACM_COMPANY_TEXT = ddlCompany.SelectedItem.Text,
                        ACM_DEPT_PK = Convert.ToInt32(ddlDepartment.SelectedValue),
                        ACM_DEPT_TEXT = ddlDepartment.SelectedItem.Text,
                        ACM_TEAM_PK = Convert.ToInt32(hdfTeam.Value),
                        ACM_TEAM_TEXT = txtTeam.Text,
                        // ACM_GROUP_PK = Convert.ToInt32(ddlGroup.SelectedValue),
                        //ACM_GROUP_TEXT = ddlGroup.SelectedItem.Text,
                    });
                }

            }
            return result;
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
                            if (flag == 0)  //Issue Not Find and this method is not a proper way
                            {
                                flag = 1;
                                uclPaging.CurrentPage++;
                            }
                        break;
                    case NavigationEnum.PREVIOUS:
                        if (e.CurrentPage > 1)
                            if (flag == 0)    ////Issue Not Find and this method is not a proper way
                            {
                                flag = 1;
                                uclPaging.CurrentPage--;
                            }
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


        protected void ActionHandler1(object sender, DataNavigatorEventArgs e)
        {
            try
            {
                GetFieldValues(ControlsEnum.GETACTIVITY);
                SetFieldValues(ControlsEnum.GETACTIVITY);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
                this.PageIndex = 1;
                GetFieldValues(ControlsEnum.GETACTIVITY);
                SetFieldValues(ControlsEnum.GETACTIVITY);
                //EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }



        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex;
        }

        #endregion

        #region Get Field Values
        private void GetFieldValues(ControlsEnum type)
        {
            AdmCompanyMstService admCompanyMstServiceClient;
            ServiceUtility serviceUtilityObj;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            int Pk = 0;
            int active = 1;
            int ParentPk = 0;
            int cgt = 10;
            int cng = 1;

            try
            {
                switch (type)
                {
                    case ControlsEnum.LIST:
                        BusinessObject.GridPrams gridParam;
                        gridParam = new BusinessObject.GridPrams();
                        gridParam.PageNumber = (PageIndex == 0) ? 1 : PageIndex;
                        gridParam.PageSize = PageSize;
                        dtActivityList = CostCenterMasterBL.GetActivityList(gridParam, currentUser.SBUID, txtActivityCode.Text.Trim(), txtActivityName.Text.Trim(), Convert.ToInt16(hdfTeamSrch.Value), Convert.ToInt16(ddlMainActivitySrch.SelectedValue));
                        break;
                    case ControlsEnum.EDIT:
                        objActivity = CostCenterMasterBL.GetActivityDetails(CurrPK);
                        break;
                    case ControlsEnum.GETGROUP:
                        dtGroup = CostCenterMasterBL.GetCostCenterGroup(currentUser.CurrentSBUPK, Convert.ToInt32(GroupEnum.GroupType), Convert.ToInt32(GroupEnum.GroupValue));
                        break;
                    case ControlsEnum.GETDEPARTMENT:
                        dtDepartment = DataAccess.Administration.Masters.CompanyMasterDA.GetSubDepartmentsMMT(Pk, active, ParentPk, currentUser.SBUID, cgt, cng);
                        break;
                    case ControlsEnum.GETCOSTCENTER:
                        //dtCostCenterAccounts_Map = DataAccess.Administration.Masters.CostCenterMasterDA.GetCostCenterAccountListByGroup(Convert.ToInt32(ddlGroup.SelectedValue), ddlGroup.SelectedItem.Text, currentUser.SBUID);
                        break;
                    case ControlsEnum.GETACTIVITY:
                        gridParam = new BusinessObject.GridPrams();
                        gridParam.PageNumber = (PageIndex == 0) ? 1 : PageIndex;
                        gridParam.PageSize = PageSize;
                        gridParam.SortBy = string.IsNullOrEmpty(SortBy) ? Resources.DataFieldRes.AccountName : SortBy;
                        gridParam.SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortAscending : SortDirection;
                        dtCostCenterAccounts = CostCenterMasterBL.GetCostCenterAccountList(gridParam, CurrPK);
                        break;
                    case ControlsEnum.MAINACTIVITY:
                        dtMainActivity = CostCenterMasterBL.GetMainActivityList(currentUser.SBUID, CurrPK, 1);
                        break;
                    case ControlsEnum.ALLMAINACTIVITY:
                        dtAllMainActivity = CostCenterMasterBL.GetMainActivityList(currentUser.SBUID, 0, 0);
                        break;
                    case ControlsEnum.COMPANY:
                        admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        //dtCompany = BusinessLogic.Administration.Configurations.CompanyBL.GetCompanyDetails(0, Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID);
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

        #region Set Field Values
        private void SetFieldValues(ControlsEnum type)
        {
            try
            {
                switch (type)
                {
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    case ControlsEnum.EDIT:
                        GetUIValuesFromObject(ControlsEnum.EDIT);
                        break;
                    case ControlsEnum.GETGROUP:
                        BindDropdown(ControlsEnum.GETGROUP);
                        break;
                    case ControlsEnum.MAINACTIVITY:
                        BindDropdown(ControlsEnum.MAINACTIVITY);
                        break;
                    case ControlsEnum.ALLMAINACTIVITY:
                        BindDropdown(ControlsEnum.ALLMAINACTIVITY);
                        break;
                    case ControlsEnum.GETDEPARTMENT:
                        BindDropdown(ControlsEnum.GETDEPARTMENT);
                        break;
                    case ControlsEnum.GETSUBDEPARTMENT:
                        BindDropdown(ControlsEnum.GETSUBDEPARTMENT);
                        break;
                    case ControlsEnum.GETACTIVITY:
                        BindGrid(ControlsEnum.GETACTIVITY);
                        break;

                    case ControlsEnum.COMPANY:
                        BindDropdown(ControlsEnum.COMPANY);
                        break;

                    case ControlsEnum.GETCOSTCENTER:
                        BindDropdown(ControlsEnum.GETCOSTCENTER);
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

        #region Get UI Values From Object

        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.EDIT:
                        if (objActivity != null)
                        {
                            txtCode.Text = HttpUtility.HtmlDecode(objActivity.EAM_CODE);
                            txtName.Text = HttpUtility.HtmlDecode(objActivity.EAM_NAME);
                            txtDescription.Text = HttpUtility.HtmlDecode(objActivity.EAM_DESC);

                            chkActive.Checked = Convert.ToBoolean(objActivity.EAM_ACTIVE);
                            chkMainActivity.Checked = Convert.ToBoolean(objActivity.EAM_MAIN_ACTIVITY);
                            if (chkMainActivity.Checked == false)
                            {
                                ddlMainActivity.SelectedIndex = Convert.ToInt32(ddlMainActivity.Items.IndexOf(ddlMainActivity.Items.FindByValue(objActivity.EAM_MAIN_ACTIVITY_PK.ToString())));
                                ddlMainActivity.Visible = true;
                                vrfMainActivity.Enabled = true;
                            }
                            else
                            {
                                ddlMainActivity.Visible = false;
                                vrfMainActivity.Enabled = false;
                            }
                            LastModifiedTime = Convert.ToDateTime(objActivity.LAST_MOD_DT);
                            lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                            ModifiedDatePnl.Visible = true;
                        }
                        break;
                    default:
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

        #region Set UI Values To Object

        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.SAVEACTIVITY:
                        ActivityMasterBO objSaveActivity = new ActivityMasterBO();
                        objSaveActivity.EAM_PK = CurrPK;
                        objSaveActivity.EAM_CODE = HttpUtility.HtmlEncode(txtCode.Text.Trim());
                        objSaveActivity.EAM_NAME = HttpUtility.HtmlEncode(txtName.Text.Trim());
                        objSaveActivity.EAM_DESC = HttpUtility.HtmlEncode(txtDescription.Text);
                        objSaveActivity.EAM_ACTIVE = chkActive.Checked == true ? 1 : 0;
                        objSaveActivity.EAM_MAIN_ACTIVITY = chkMainActivity.Checked == true ? 1 : 0;
                        if (chkMainActivity.Checked == false)
                            objSaveActivity.EAM_MAIN_ACTIVITY_PK = Convert.ToInt16(ddlMainActivity.SelectedValue);
                        objSaveActivity.EAM_BIZUNIT = currentUser.SBUID;
                        objSaveActivity.USER_PK = currentUser.PKUser;
                        objSaveActivity.LAST_MOD_DT = LastModifiedTime;
                        if (objActivity != null && objActivity.MapDetails != null)
                            objSaveActivity.MapDetails = objActivity.MapDetails;
                        retObject = objSaveActivity;
                        break;
                    default:
                        break;
                }
                return retObject;
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

        #region Bind Grid

        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.COSTCENTERMAP:
                        if (objActivity != null)
                        {
                            GrdCostCenter.DataSource = objActivity.MapDetails;
                            GrdCostCenter.DataBind();
                        }
                        break;
                    case ControlsEnum.LIST:
                        uclPaging.Visible = false;
                        if (dtActivityList != null && dtActivityList.Rows.Count > 0)
                        {
                            int rowCount = 0;

                            rowCount = Convert.ToInt32(dtActivityList.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                            this.TotalPages = Convert.ToInt32(dtActivityList.Rows[0]["ROW_NO"].ToString());

                            uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                             (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                             (rowCount / this.PageSize) + 1;

                            PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdActivityList.DataSource = dtActivityList;
                            grdActivityList.DataBind();

                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            grdActivityList.DataSource = null;
                            grdActivityList.DataBind();
                            uclPaging.BindPager();
                        }
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

        #region Bind Dropdown
        public void BindDropdown(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    //case ControlsEnum.GETGROUP:
                    //    ddlGroup.Items.Clear();
                    //    if (dtGroup != null && dtGroup.Rows.Count > 0)
                    //    {
                    //        dtGroup = CommonFunctions.HtmlDecodeDataTable(dtGroup, "CON_NAME_TEXT"); //Decode DataTable
                    //        ddlGroup.DataTextField = "CON_NAME_TEXT";
                    //        ddlGroup.DataValueField = "CON_PK";
                    //        ddlGroup.DataSource = dtGroup;
                    //        ddlGroup.DataBind();
                    //    }
                    //    ddlGroup.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                    //    break;
                    case ControlsEnum.GETDEPARTMENT:
                        ddlDepartment.Items.Clear();
                        if (dtDepartment != null && dtDepartment.Rows.Count > 0)
                        {
                            dtDepartment = CommonFunctions.HtmlDecodeDataTable(dtDepartment, "CON_NAME"); //Decode DataTable
                            ddlDepartment.DataTextField = "CON_NAME";
                            ddlDepartment.DataValueField = "CON_PK";
                            ddlDepartment.DataSource = dtDepartment;
                            ddlDepartment.DataBind();
                        }
                        ddlDepartment.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.MAINACTIVITY:
                        ddlMainActivity.Items.Clear();
                        if (dtMainActivity != null && dtMainActivity.Rows.Count > 0)
                        {
                            //dtMainActivity = CommonFunctions.HtmlDecodeDataTable(dtDepartment, "CON_NAME"); //Decode DataTable
                            ddlMainActivity.DataTextField = "VALUE";
                            ddlMainActivity.DataValueField = "PK";
                            ddlMainActivity.DataSource = dtMainActivity;
                            ddlMainActivity.DataBind();
                        }
                        ddlMainActivity.Items.Insert(0, new ListItem(GetLocalResourceObject("SELECTTEXT").ToString(), CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.ALLMAINACTIVITY:
                        ddlMainActivitySrch.Items.Clear();
                        if (dtAllMainActivity != null && dtAllMainActivity.Rows.Count > 0)
                        {
                            //dtMainActivity = CommonFunctions.HtmlDecodeDataTable(dtDepartment, "CON_NAME"); //Decode DataTable
                            ddlMainActivitySrch.DataTextField = "VALUE";
                            ddlMainActivitySrch.DataValueField = "PK";
                            ddlMainActivitySrch.DataSource = dtAllMainActivity;
                            ddlMainActivitySrch.DataBind();
                        }
                        ddlMainActivitySrch.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                        break;

                    case ControlsEnum.GETCOSTCENTER:
                        if (dtCostCenterAccounts != null && dtCostCenterAccounts.Rows.Count > 0)
                        {
                            dtCostCenterAccounts = CommonFunctions.HtmlDecodeDataTable(dtCostCenterAccounts, "CON_NAME");
                        }
                        break;

                    case ControlsEnum.COMPANY:
                        ddlCompany.Items.Clear();
                        if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                        {
                            ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                            ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                            ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                            ddlCompany.DataBind();
                        }
                        ddlCompany.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                        break;

                    default: break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Reset
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.CLEAR:
                    hdfIscontYes.Value = "0";
                    txtCode.Text = txtName.Text = txtDescription.Text = lblLastModifiedHDR.Text = string.Empty;
                    txtActivityCode.Text = txtActivityName.Text = string.Empty;
                    chkActive.Checked = true;
                    chkMainActivity.Checked = false;
                    //CurrPK = 0;
                    objActivity = null;
                    ddlMainActivity.SelectedIndex = 0;
                    ddlMainActivity.Visible = true;
                    break;
                case ControlsEnum.CLEARSEARCH:
                    uclPaging.CurrentPage = 0;
                    PageIndex = 1;
                    txtActivityCode.Text = txtActivityName.Text = string.Empty;
                    txtTeamSrch.Text = "Select/Type";
                    ddlMainActivitySrch.SelectedIndex = 0;
                    hdfTeamSrch.Value = "0";
                    break;
                case ControlsEnum.COSTCENTERMAP:
                    //ddlGroup.SelectedIndex = -1;
                    //ddlDepartment.SelectedIndex = -1;
                    //ddlSubDepartment.SelectedIndex = -1;
                    txtTeam.Text = GetLocalResourceObject("AutoDefaultValue").ToString();
                    hdfTeam.Value = "0";
                    SlNo = 0;
                    break;
                default:
                    break;
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
                uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the first link
                uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the previous link
                uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false; // Should we enable the next link
                uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;// Should we enable the last link
            }
        }
        #endregion

        /// <summary>
        /// Set Configuration settings
        /// </summary>
        private void ConfigurationSettings()
        {
            IsMappedAccounts = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "MapedAccounts")));
        }


        #region Enum
        public enum ControlsEnum
        {
            LIST,
            CLEAR,
            CLEARSEARCH,
            TYPE,
            EDIT,
            SAVEACTIVITY,
            ADDCOSTCENTER,
            GETGROUP,
            COMPANY,
            GETDEPARTMENT,
            GETSUBDEPARTMENT,
            GETACTIVITY,
            GETCOSTCENTER,
            COSTCENTERMAP,
            MAINACTIVITY,
            ALLMAINACTIVITY
        }
        public enum GroupEnum
        {
            GroupType = 27,  //P_CGT_VALUE
            GroupValue = 2  //P_CNG_VALUE
        }

        #endregion

        #region Helper Methods
        /// <summary>
        /// for checking radio button selected in main grid
        /// </summary>
        /// <param name="GridRowIndex"></param>
        /// <returns></returns>
        private bool IsRadioButtonSelected(ref int GridRowIndex)
        {
            RadioButton rbtn;
            foreach (GridViewRow grdrow in grdActivityList.Rows)
            {
                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                if (rbtn.Checked)
                {
                    GridRowIndex = grdrow.RowIndex;
                    return true;
                }
            }
            return GridRowIndex == 0 ? false : true;
        }
        #endregion

    }
}