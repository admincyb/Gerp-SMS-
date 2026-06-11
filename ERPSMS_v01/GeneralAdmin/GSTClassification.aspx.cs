using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject;
using System.Data;
using ERPSMS_v01.Administration.Masters;
using BusinessObject.Common;
using ERP.Utilities;
using ERPSMS_v01.UserControls;
using BusinessObject.Administration.Masters;
using BusinessLogic.Administration.Masters;
using BusinessObject.CommonManagement;

namespace ERPSMS_v01.GeneralAdmin
{
    public partial class GSTClassification : System.Web.UI.Page
    {
        #region Variables & Properties

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

        #region Variables

        User currentUser;
        public DataTable dtTaxability;
        public DataTable dtGstClassList;
        private DataTable dtResult;
        public int result;
        private ActionsEnum commonActions;
        private GSTClassificationBO objGSTClassification;

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
                GetFieldValues(ControlEnum.TAXABILITY);
                SetFieldValues(ControlEnum.TAXABILITY);
                GetFieldValues(ControlEnum.GRID);
                SetFieldValues(ControlEnum.GRID);
                txtCodeFilterList.Focus();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
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
                }
                switch (commonActions)
                {
                    #region NEW
                    case ActionsEnum.NEW:
                        CurrPK = 0;
                        EntryStatus = EntryStatus.NEWMODE;
                        ResetForm(ControlEnum.CLEAR);
                        txtGstClassCode.Focus();
                        break;
                    #endregion

                    #region SAVE
                    case ActionsEnum.SAVE:
                        objGSTClassification = new GSTClassificationBO();
                        objGSTClassification = (GSTClassificationBO)SetUIValuesToObject(ControlEnum.GSTCLASSHDR);
                        if (objGSTClassification != null)
                        {
                            result = GSTClassificationBL.SaveGSTClassificationDetails(objGSTClassification);
                            if (result > 0)
                            {
                                litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("GSTClassification").ToString());
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
                                    litErrorMsg.Text = GetLocalResourceObject("GSTClassification").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                }
                                else if (result == (int)BusinessObject.CommonManagement.DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("GSTClassification").ToString() + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                }
                                else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.INCORRECT)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("GSTClassification").ToString() + " " + GetLocalResourceObject("CodeAlreadyExist").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                }
                                else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("GSTClassification").ToString() + " " + GetLocalResourceObject("NameAlreadyExist").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("GSTClassification").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region EDIT
                    case ActionsEnum.EDIT:
                        bIsChecked = false;
                        foreach (GridViewRow grdrow in grdGstClassificationList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                ResetForm(ControlEnum.CLEAR);
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfGstCfnPk")).Value);
                                EntryStatus = EntryStatus.EDITMODE;
                                GetFieldValues(ControlEnum.GSTDETAIL);
                                SetFieldValues(ControlEnum.GSTDETAIL);
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
                        GetFieldValues(ControlEnum.GRID);
                        SetFieldValues(ControlEnum.GRID);
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
                        ResetForm(ControlEnum.CLEARFILTER);
                        GetFieldValues(ControlEnum.GRID);
                        SetFieldValues(ControlEnum.GRID);
                        txtCodeFilterList.Focus();
                        break;
                    #endregion

                    #region DELETE
                    case ActionsEnum.DELETE:
                        bIsChecked = false;
                        result = GSTClassificationBL.DeleteGstDetails(CurrPK,LastModifiedTime);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("GSTClassification").ToString());
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
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("GSTClassification").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)BusinessObject.CommonManagement.DbDeleteStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("GSTClassification").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)BusinessObject.CommonManagement.DbDeleteStatus.REFERRED)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("GSTClassification").ToString() + " " + Resources.Messages.UsedInAnotherPlace;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)BusinessObject.CommonManagement.DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("GSTClassification").ToString() + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("GSTClassification").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
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
        private void GetFieldValues(ControlEnum type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {

                    #region TAXABILITY
                    case ControlEnum.TAXABILITY:
                        dtTaxability = new DataTable();
                        dtTaxability = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "GST TAXABILITY");
                        break;
                    #endregion

                    #region LISTING GRID
                    case ControlEnum.GRID:
                        dtGstClassList = new DataTable();
                        dtGstClassList = GSTClassificationBL.GetGstList(txtCodeFilterList.Text, txtNameFilterList.Text, currentUser.CurrentSBUPK, PageIndex, Convert.ToInt32(GetLocalResourceObject("PageSize")));
                        break;
                    #endregion

                    #region EDIT
                    case ControlEnum.GSTDETAIL:
                        dtResult = GSTClassificationBL.GetGstDetailList(this.CurrPK, Convert.ToInt32(DbActiveStatus.HASPK), currentUser.SBUID);
                        break;
                    #endregion

                    default: break;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { }
        }
        #endregion

        #region Set Field Values
        private void SetFieldValues(ControlEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region TAXABILITY
                    case ControlEnum.TAXABILITY:
                        BindDropdown(controlType);
                        break;
                    #endregion

                    #region GRID LIST
                    case ControlEnum.GRID:
                        BindGrid(controlType);
                        break;
                    #endregion

                    #region GST CLASSIFICATION EDIT
                    case ControlEnum.GSTDETAIL:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            txtGstClassCode.Text = dtResult.Rows[0]["GCM_CODE"].ToString().HtmlDecode();
                            txtGstClassName.Text = dtResult.Rows[0]["GCM_NAME"].ToString().HtmlDecode();
                            int NonGstItem = Convert.ToInt32(dtResult.Rows[0]["GCM_IS_NONGST"]);
                            if (NonGstItem > 0)
                                chkNonGstItem.Checked = true;
                            else
                                chkNonGstItem.Checked = false;
                            ddlGstClassTaxability.SelectedValue = dtResult.Rows[0]["GCM_TAXABILITY"].ToString().HtmlDecode();
                            txtGstClassDescription.Text = dtResult.Rows[0]["GCM_DESC"].ToString().HtmlDecode();
                            LastModifiedTime = Convert.ToDateTime(dtResult.Rows[0]["GCM_MOD_DATE"].ToString());
                        }
                        break;
                    #endregion

                    default: break;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { }
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
                    #region GST CLASSIFICATION DETAILS

                    case ControlEnum.GSTCLASSHDR:
                        objGSTClassification.GCM_PK = CurrPK;
                        objGSTClassification.GCM_NAME = txtGstClassName.Text.HtmlEncode();
                        objGSTClassification.GCM_CODE = txtGstClassCode.Text.HtmlEncode();
                        objGSTClassification.GCM_TAXABILITY = Convert.ToInt16(ddlGstClassTaxability.SelectedValue);
                        objGSTClassification.GCM_IS_NONGST = Convert.ToInt32(chkNonGstItem.Checked == true ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO));
                        objGSTClassification.GCM_DESC = txtGstClassDescription.Text.HtmlEncode();
                        objGSTClassification.GCM_MOD_BY = currentUser.PKUser.ToString();
                        objGSTClassification.GCM_CRTD_BY = currentUser.PKUser.ToString();
                        objGSTClassification.ACTIVE = Convert.ToInt16(DbActiveStatus.ACTIVE);
                        objGSTClassification.LAST_MOD_DT = LastModifiedTime;
                        objGSTClassification.BIZUNIT_PK = currentUser.CurrentSBUPK;

                        returnObject = objGSTClassification;
                        break;

                    #endregion
                    default: break;
                }
                return returnObject;

            }
            catch (Exception ex) { throw ex; }
            finally { }
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

        #region Helper Methods

        #region Bind Dropdown
        private void BindDropdown(ControlEnum controlType)
        {
            switch (controlType)
            {
                case ControlEnum.TAXABILITY:
                    ddlGstClassTaxability.Items.Clear();
                    if (dtTaxability != null && dtTaxability.Rows.Count > 0)
                    {
                        ddlGstClassTaxability.DataSource = dtTaxability;
                        ddlGstClassTaxability.DataTextField = "CFG_DATA";  //Resources.DataFieldRes.cfgData;
                        ddlGstClassTaxability.DataValueField = "CFG_VALUE"; //Resources.DataFieldRes.cfgValue;
                        ddlGstClassTaxability.DataBind();
                    }
                    ddlGstClassTaxability.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;

                default: break;
            }
        }
        #endregion

        #region Bind Grid
        public void BindGrid(ControlEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region LISTING GRID
                    case ControlEnum.GRID:
                        uclPaging.Visible = false;
                        if (dtGstClassList != null && dtGstClassList.Rows.Count > 0)
                        {
                            int rowCount = 0;
                            rowCount = Convert.ToInt32(dtGstClassList.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                            uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                              (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                              (rowCount / this.PageSize) + 1;
                            PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdGstClassificationList.DataSource = dtGstClassList;
                            grdGstClassificationList.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            grdGstClassificationList.DataSource = null;
                            grdGstClassificationList.DataBind();
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

        #region Reset Form

        private void ResetForm(ControlEnum controlType)
        {
            switch (controlType)
            {
                #region CLEAR
                case ControlEnum.CLEAR:
                    CurrPK = 0;
                    txtGstClassCode.Text = txtGstClassName.Text = txtGstClassDescription.Text = string.Empty;
                    chkNonGstItem.Checked = true;
                    ddlGstClassTaxability.ClearSelection();
                    LastModifiedTime = DateTime.Now;
                    uclPaging.CurrentPage = 0;
                    PageIndex = 1;
                    break;
                #endregion

                #region CLEAR FILTER
                case ControlEnum.CLEARFILTER:
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
            GSTCLASSLIST,
            TAXABILITY,
            NEW,
            CLEAR,
            CLEARFILTER,
            GRID,
            GSTCLASSHDR,
            GSTDETAIL
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