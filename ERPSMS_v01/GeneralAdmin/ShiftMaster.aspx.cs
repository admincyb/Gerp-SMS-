using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Administration.Masters;
using ERPSMS_v01.Administration.Masters;
using System.Xml;
using BusinessObject.Common;
using ERP.Utilities;
using System.Data;
using BusinessLogic.Administration.Masters;
using BusinessObject;
using BusinessObject.ILibrary;
using BusinessLogic.Administration.Configurations;
using BusinessObject.CommonManagement;
using ERPSMS_v01.UserControls;

namespace ERPSMS_v01.GeneralAdmin
{
    public partial class ShiftMaster : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        #region Variables
        User currentUser;
        private ShiftHeader objShiftHeader;
        public DataTable dtShift;
        public DataTable dtShiftList;
        private DataTable dtResult;
        public int result;
        private ActionsEnum commonActions;
        #endregion
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
        /// 
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
        private ShiftMasterBO.ShiftType ShiftTypeViewState
        {
            get
            {
                return ViewState["ShiftTypeViewState"] == null ? new ShiftMasterBO.ShiftType() : (ShiftMasterBO.ShiftType)ViewState["ShiftTypeViewState"];
            }
            set
            {
                ViewState["ShiftTypeViewState"] = value;
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
        #endregion
        #endregion

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

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PageActionHandler();
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();
        }
        private void PageActionHandler()
        {
            try
            {
                
                PageIndex = Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE);
                uclPaging.TotalPages = TotalPages;
                uclPaging.CurrentPage = 1;
                EntryStatus = EntryStatus.LISTMODE;
                ResetForm(ControlEnum.CLEAR);
                GetFieldValues(ControlEnum.GRID);
                SetFieldValues(ControlEnum.GRID);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }

        #endregion

        #region Get Field Values
        private void GetFieldValues(ControlEnum type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            switch (type)
            {
                case ControlEnum.GRID:
                    dtShiftList = new DataTable();
                    dtShiftList = BusinessLogic.Administration.Masters.ShiftMasterBL.GetShift(currentUser.SBUID, txtCodeFilterList.Text, txtNameFilterList.Text, PageIndex, Convert.ToInt32(GetLocalResourceObject("PageSize")), Convert.ToInt32(ddlShiftType.SelectedValue));
                    break;
                #region Shift Type
                case ControlEnum.ShitType:
                    dtResult = BusinessLogic.Administration.Masters.ShiftMasterBL.GetShiftType(this.CurrPK, Convert.ToInt32(DbActiveStatus.HASPK), currentUser.SBUID);
                    break;
                #endregion
                default:
                    break;
            }
        }
        #endregion

        #region Set Field Values
        private void SetFieldValues(ControlEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlEnum.GRID:
                        BindGrid();
                        break;
                    case ControlEnum.ShitType:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            txtCode.Text = dtResult.Rows[0]["SHF_CODE"].ToString().HtmlDecode();
                            txtDescription.Text = dtResult.Rows[0]["SHF_DESC"].ToString().HtmlDecode();
                            txtFrom.Text = dtResult.Rows[0]["SHF_TIME_FROM"].ToString();
                            txtTo.Text = dtResult.Rows[0]["SHF_TIME_TO"].ToString();
                            txtName.Text = dtResult.Rows[0]["SHF_NAME"].ToString().HtmlDecode();
                            txtSequence.Text = dtResult.Rows[0]["SHF_SEQUENCE"].ToString();
                            LastModifiedTime = Convert.ToDateTime(dtResult.Rows[0]["LAST_MOD_DT"].ToString());
                            int Active = Convert.ToInt32(dtResult.Rows[0]["SHF_ACTIVE"]);
                            if (Active > 0)
                                chkActive.Checked = true;
                            else
                                chkActive.Checked = false;
                            int PrdnShift = Convert.ToInt32(dtResult.Rows[0]["SHF_IS_PRODUCTION"]);
                            if (PrdnShift > 0)
                                chkShift.Checked = true;
                            else
                                chkShift.Checked = false;
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

        #region Helper Methods

        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid()
        {
            try
            {
                uclPaging.Visible = false;
                if (dtShiftList != null && dtShiftList.Rows.Count > 0)
                {
                    int rowCount = 0;
                    rowCount = Convert.ToInt32(dtShiftList.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                    uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                      (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                      (rowCount / this.PageSize) + 1;
                    PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                    grdCompanyList.DataSource = dtShiftList;
                    grdCompanyList.DataBind();
                    uclPaging.Visible = true;
                    uclPaging.BindPager();
                }
                else
                {
                    grdCompanyList.DataSource = null;
                    grdCompanyList.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ActionHandler
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                int result;
                result = 0;

                Session[ERP.Utilities.SessionStrings.TrxPK] = null;
                bool bIsChecked = false;
                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();

                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else
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
                switch (commonActions)
                {

                    #region NEW
                    //To add new companydetails
                    case ActionsEnum.NEW:
                        CurrPK = 0;
                        EntryStatus = EntryStatus.NEWMODE;
                        ResetForm(ControlEnum.CLEAR);
                        txtCode.Focus();
                        break;
                    #endregion

                    #region SAVE
                    case ActionsEnum.SAVE:

                        objShiftHeader = new ShiftHeader();
                        objShiftHeader = (ShiftHeader)SetUIValuesToObject(ControlEnum.SHIFTHDR);
                        if (objShiftHeader != null)
                        {
                            result = BusinessLogic.Administration.Masters.ShiftMasterBL.SaveShiftDetails(objShiftHeader);
                            if (result > 0)
                            {
                                litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ShiftSave);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                ResetForm(ControlEnum.CLEAR);
                                EntryStatus = EntryStatus.LISTMODE;
                                GetFieldValues(ControlEnum.GRID);
                                SetFieldValues(ControlEnum.GRID);
                            }
                            
                            else
                                    {
                                        if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.SQLERROR)
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.CONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.ShiftSave + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)BusinessObject.CommonManagement.DbDeleteStatus.DELETECONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.ShiftSave + " " + Resources.Messages.AlreadyDeleted;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                        }
                                        else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.PENDINGEXIST)
                                        {
                                            litErrorMsg.Text = Resources.Messages.Msg_Already_Exist;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ShiftSave);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                    }

                        }

                        break;
                    #endregion

                    #region LIST,CANCEL
                    case ActionsEnum.CANCEL:
                    case ActionsEnum.LIST:
                        uclPaging.CurrentPage = 0;
                        this.PageIndex = 1;
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        //ResetForm(ControlEnum.CLEAR);
                        //EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlEnum.GRID);
                        SetFieldValues(ControlEnum.GRID);
                        break;
                    #endregion

                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlEnum.CLEARFILTER);
                        GetFieldValues(ControlEnum.GRID);
                        SetFieldValues(ControlEnum.GRID);
                        break;
                    #endregion

                    #region DETAIL
                    case ActionsEnum.EDIT:
                        bIsChecked = false;
                        foreach (GridViewRow grdrow in grdCompanyList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                ResetForm(ControlEnum.CLEAR);
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfShiftPk")).Value);
                                EntryStatus = EntryStatus.EDITMODE;
                                GetFieldValues(ControlEnum.ShitType);
                                SetFieldValues(ControlEnum.ShitType);
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

                    #region DELETE
                    case ActionsEnum.DELETE:
                        bIsChecked = false;
                        result = BusinessLogic.Administration.Masters.ShiftMasterBL.DeleteShiftDetails(CurrPK);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ShiftSave);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            ResetForm(ControlEnum.CLEAR);
                            EntryStatus = EntryStatus.LISTMODE;
                            GetFieldValues(ControlEnum.GRID);
                            SetFieldValues(ControlEnum.GRID);
                        }
                        else
                        {
                            if (result == (int)BusinessObject.CommonManagement.DbDeleteStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)BusinessObject.CommonManagement.DbDeleteStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.ShiftSave + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)BusinessObject.CommonManagement.DbDeleteStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.ShiftSave + " " + Resources.Messages.UsedInAnotherPlace;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)BusinessObject.CommonManagement.DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.ShiftSave + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ShiftSave);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
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
        #endregion

        #region Set UI EditView
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                if (Mode == ActionsEnum.NEW)
                {
                    EntryStatus = EntryStatus.NEWMODE;
                }
                else if (Mode == ActionsEnum.VIEW)
                {
                    EntryStatus = EntryStatus.VIEWMODE;
                }
                else if (Mode == ActionsEnum.EDIT)
                {
                    EntryStatus = EntryStatus.EDITMODE;
                }
                else
                {
                    EntryStatus = EntryStatus.ENTRYMODE;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Set UIValues To Object
        private Object SetUIValuesToObject(ControlEnum controlType)
        {
            object returnObject = new object();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region SHIFTHDR
                    case ControlEnum.SHIFTHDR:
                        //objShiftHeader.SHF_PK = Convert.ToInt16(currentUser.PKUser);
                        //objShiftHeader.SHF_PK = Convert.ToInt16(currentUser.CurrentSHFPK);
                        objShiftHeader.SHF_PK = CurrPK;
                        objShiftHeader.SHF_CODE = txtCode.Text.HtmlEncode();
                        objShiftHeader.SHF_NAME = txtName.Text.HtmlEncode();
                        objShiftHeader.SHF_DESC = txtDescription.Text.HtmlEncode();
                        objShiftHeader.SHF_TIME_FROM = txtFrom.Text;
                        objShiftHeader.SHF_TIME_TO = txtTo.Text;
                        objShiftHeader.SHF_ACTIVE = Convert.ToInt32(chkActive.Checked == true ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO));
                        objShiftHeader.SHF_IS_PRODUCTION = Convert.ToInt32(chkShift.Checked == true ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO));
                        objShiftHeader.SHF_SEQUENCE = txtSequence.Text;
                        //objShiftHeader.SHF_ACTIVE=ddlActive.SelectedValue;
                        objShiftHeader.SHF_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        objShiftHeader.SHF_CRTD_BY = currentUser.PKUser.ToString();
                        objShiftHeader.SHF_MOD_BY = currentUser.PKUser.ToString();
                        objShiftHeader.LAST_MOD_DT = LastModifiedTime;
                        returnObject = objShiftHeader;
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

        #region ResetForm
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlEnum controlType)
        {
            switch (controlType)
            {
                #region CLEAR
                case ControlEnum.CLEAR:
                    CurrPK = 0;
                    txtCode.Text = txtDescription.Text = txtFrom.Text = txtName.Text = txtSequence.Text = txtTo.Text = string.Empty;
                    txtNameFilterList.Text = string.Empty;
                    txtCodeFilterList.Text = string.Empty;
                    chkShift.Checked = true;
                    chkActive.Checked = true;
                    LastModifiedTime = DateTime.Now;
                    uclPaging.CurrentPage = 0;
                    PageIndex = 1;
                    break;
                #endregion

                case ControlEnum.CLEARFILTER:
                    txtNameFilterList.Text = string.Empty;
                    txtCodeFilterList.Text = string.Empty;
                    ddlShiftType.SelectedIndex = -1;
                    CurrPK = 0;
                    uclPaging.CurrentPage = 0;
                    this.PageIndex = 1;
                    this.EntryStatus = EntryStatus.LISTMODE;
                    break;
            }
        }
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

        #region ControlEnum

        public enum ControlEnum
        {
            COMPANYLIST,
            ShitType,
            NEW,
            FILLGRID,
            SHIFTHDR,
            CLEAR,
            GRID,
            DELETE,
            CLEARFILTER
        }

        #endregion

        #region Pager Methods + Init
        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
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
                    GetFieldValues(ControlEnum.GRID);
                    SetFieldValues(ControlEnum.GRID);
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
    }
}