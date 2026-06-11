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
    public partial class DocumentRevision : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        #region Variables
        User currentUser;
        private DocumentRevisionBO objDocumentRevisionBO;
        //public DataTable dtShift;
        public DataTable dtDocRevList;
        private DataTable dtResult;
        public int result;
        DataSet dsResult;
        DataTable DtEdit;
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
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearch", "ShowHideAdvancedSearch('" + hdfShow.Value + "');", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            AssignLocalBreadCrumb();
            //if (EntryStatus == EntryStatus.EDITMODE)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(3);", true);
            //    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
            //    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            //}
            //else if (EntryStatus == EntryStatus.NEWMODE)
            //{
            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing(0);", true);
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
            //    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
            //    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();

            //}
            //else if (EntryStatus == EntryStatus.ENTRYMODE)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
            //    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
            //    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            //}
            //else if (EntryStatus == EntryStatus.LISTMODE)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
            //    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
            //    lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
            //}
            //if (EntryStatus == EntryStatus.VIEWMODE)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
            //    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
            //    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            //}
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
                //EntryStatus = EntryStatus.LISTMODE;
                ResetForm(ControlEnum.CLEAR);
                ResetForm(ControlEnum.CLEARFILTER);
                GetFieldValues(ControlEnum.LIST);
                SetFieldValues(ControlEnum.LIST);
                //ddlReportFlter.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearch", "ShowHideAdvancedSearch();", true);
                hdfShow.Value = string.Empty;
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
                case ControlEnum.REPORT:
                    dtResult = new DataTable();
                    dtResult = BusinessLogic.Administration.Masters.DocumentRevisionBL.GetReportList(Convert.ToInt32(rdoType.SelectedValue));
                    break;
                case ControlEnum.SHOWREPORTINFILTER:
                    dtResult = new DataTable();
                    dtResult = BusinessLogic.Administration.Masters.DocumentRevisionBL.GetReportList(Convert.ToInt32(rdoTypeFlter.SelectedValue));
                    break;
                case ControlEnum.LIST:
                    int PageNumber = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                    int PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                    string doc_type = rdoTypeFlter.SelectedValue == "" ? string.Empty : rdoTypeFlter.SelectedValue;
                    //objSearch.BizUnit = currentUser.SBUID;
                    //                   ,@P_DOC_NO NVARCHAR(200)	
                    //,@P_DOC_PK INT
                    //   , @P_DOC_TYPE        BIT
                    //,@P_DOC_REVISION INT
                    //   , @P_DOC_REV_DT      DATETIME
                    dsResult = BusinessLogic.Administration.Masters.DocumentRevisionBL.GetRevisionList(PageNumber, PageSize, currentUser.SBUID, txtDocumentNoFlter.Text,
                         Convert.ToInt32(ddlReportFlter.SelectedValue), doc_type, txtRevisionFlter.Text, txtRevisionDateFlter.Text);
                    dtDocRevList = dsResult.Tables[1];
                    break;

                case ControlEnum.EDIT:


                    string Xml = BusinessLogic.Administration.Masters.DocumentRevisionBL.GetReportData(Convert.ToInt32(CurrPK));

                    objDocumentRevisionBO = (DocumentRevisionBO)CommonFunctions.DeserializeObject(Xml, new DocumentRevisionBO());



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
                    case ControlEnum.LIST:
                        BindGrid();
                        break;
                    case ControlEnum.REPORT:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            BindDropDown(ControlEnum.REPORT);
                        }
                        break;
                    case ControlEnum.SHOWREPORTINFILTER:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            BindDropDown(ControlEnum.SHOWREPORTINFILTER);
                        }
                        break;
                    case ControlEnum.EDIT:
                        GetFieldValues(ControlEnum.LIST);
                        SetFieldValues(ControlEnum.LIST);
                        DateTime revisionDate;
                        if (objDocumentRevisionBO.IS_EDITABLE == 1)
                        {
                            rdoType.SelectedValue = objDocumentRevisionBO.DRM_DOC_TYPE.ToString();

                            GetFieldValues(ControlEnum.REPORT);
                            SetFieldValues(ControlEnum.REPORT);

                            CurrPK = Convert.ToInt32(objDocumentRevisionBO.DRM_PK.ToString());
                            ddlReport.SelectedValue = objDocumentRevisionBO.DRM_DOC_PK.ToString();
                            txtDocumentName.Text = ddlReport.SelectedItem.Text;
                            // txtDARNumber.Text = objDocumentRevisionBO.DRM_DAR_NO.ToString();
                            // txtDocumentNo.Text = objDocumentRevisionBO.DRM_DOC_NO.ToString();
                            txtRevision.Text = objDocumentRevisionBO.DRM_REVISION.ToString();
                            revisionDate = objDocumentRevisionBO.DRM_REV_DT;
                            txtRevisionDate.Text = revisionDate.ToString("dd-MMM-yyyy");


                            txtDARNumber.Text = objDocumentRevisionBO.DRM_DAR_NO == null ? "" : objDocumentRevisionBO.DRM_DAR_NO;
                            txtDocumentNo.Text = objDocumentRevisionBO.DRM_DOC_NO == null ? "" : objDocumentRevisionBO.DRM_DOC_NO;
                        }
                        else
                        {
                            ResetForm(ControlEnum.CLEAR);
                            litErrorMsg.Text = GetLocalResourceObject("Err_NonEditable").ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DocumentRevision);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);

                        }

                        //objDocumentRevisionBO.DRM_USER_PK = currentUser.PKUser;

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
        public void BindDropDown(ControlEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlEnum.REPORT:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ddlReport.Items.Clear();
                            txtDocumentName.Text = string.Empty;
                            ddlReport.DataSource = dtResult;
                            ddlReport.DataTextField = "VALUE";
                            ddlReport.DataValueField = "PK";
                            ddlReport.DataBind();
                        }
                        ddlReport.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    case ControlEnum.SHOWREPORTINFILTER:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ddlReportFlter.Items.Clear();
                            ddlReportFlter.DataSource = dtResult;
                            ddlReportFlter.DataTextField = "VALUE";
                            ddlReportFlter.DataValueField = "PK";
                            ddlReportFlter.DataBind();
                        }
                        ddlReportFlter.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Assign breadCrumb
        /// </summary>
        public void AssignLocalBreadCrumb()
        {
            try
            {
                string breadCrumb = string.Empty;

                if (this.GetLocalResourceObject("Breadcrumb") != null)
                {
                    breadCrumb = this.GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                    lblBreadCrum.Text = breadCrumb;
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
                uclPaging.Visible = false;
                if (dtDocRevList != null && dtDocRevList.Rows.Count > 0)
                {
                    int rowCount = 0;
                    rowCount = Convert.ToInt32(dtDocRevList.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                    uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                      (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                      (rowCount / this.PageSize) + 1;
                    PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                    grdRevisionList.DataSource = dtDocRevList;
                    grdRevisionList.DataBind();
                    uclPaging.Visible = true;
                    uclPaging.BindPager();
                }
                else
                {
                    grdRevisionList.DataSource = null;
                    grdRevisionList.DataBind();
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

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearch", "ShowHideAdvancedSearch('" + hdfShow.Value + "');", true);

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

                    commonActions = ActionsEnum.CHANGE;

                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButtonList)))
                {
                    if (((RadioButtonList)sender).ID == "rdoType")
                    {
                        commonActions = ActionsEnum.SHOWREPORT;
                    }
                    else if (((RadioButtonList)sender).ID == "rdoTypeFlter")
                    {
                        commonActions = ActionsEnum.SHOWREPORTINFILTER;
                    }
                }

                switch (commonActions)
                {
                    #region SHOWREPORT
                    case ActionsEnum.SHOWREPORT:
                        GetFieldValues(ControlEnum.REPORT);
                        SetFieldValues(ControlEnum.REPORT);
                        break;
                    #endregion
                    #region CHANGE
                    case ActionsEnum.CHANGE:
                        if (ddlReport.SelectedValue != CommonConstants.SELECTVAL)
                            txtDocumentName.Text = ddlReport.SelectedItem.Text;
                        else
                            txtDocumentName.Text = string.Empty;
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlEnum.CLEAR);
                        //GetFieldValues(ControlEnum.LIST);
                        //SetFieldValues(ControlEnum.LIST);
                        break;
                    #endregion
                    #region SHOWREPORTINFILTER
                    case ActionsEnum.SHOWREPORTINFILTER:
                        GetFieldValues(ControlEnum.SHOWREPORTINFILTER);
                        SetFieldValues(ControlEnum.SHOWREPORTINFILTER);
                        //GetFieldValues(ControlEnum.LIST);
                        //SetFieldValues(ControlEnum.LIST);
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ResetForm(ControlEnum.CLEARFILTER);
                        GetFieldValues(ControlEnum.LIST);
                        SetFieldValues(ControlEnum.LIST);
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlEnum.LIST);
                        SetFieldValues(ControlEnum.LIST);
                        break;
                    #endregion
                    #region NEW
                    //To add new companydetails
                    case ActionsEnum.NEW:
                        CurrPK = 0;
                        EntryStatus = EntryStatus.NEWMODE;
                        ResetForm(ControlEnum.CLEAR);
                        //txtCode.Focus();
                        break;
                    #endregion

                    #region SAVE
                    case ActionsEnum.SAVE:

                        objDocumentRevisionBO = new DocumentRevisionBO();
                        objDocumentRevisionBO = (DocumentRevisionBO)SetUIValuesToObject(ControlEnum.DOCUMENTREVISION);
                        if (objDocumentRevisionBO != null)
                        {
                            string xmlDoc = CommonFunctions.XmlSerialize<DocumentRevisionBO>(objDocumentRevisionBO);
                            result = BusinessLogic.Administration.Masters.DocumentRevisionBL.SaveDocumentRevision(xmlDoc);
                            if (result > 0)
                            {
                                litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DocumentRevision);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                ResetForm(ControlEnum.CLEAR);
                                EntryStatus = EntryStatus.LISTMODE;
                                GetFieldValues(ControlEnum.LIST);
                                SetFieldValues(ControlEnum.LIST);
                            }

                            else
                            {
                                if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                }
                                else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.DATEOVERLAP)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("DateRangeExist").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
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
                    case ActionsEnum.LIST:
                        uclPaging.CurrentPage = 0;
                        this.PageIndex = 1;
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        //ResetForm(ControlEnum.CLEAR);
                        //EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlEnum.LIST);
                        SetFieldValues(ControlEnum.LIST);
                        break;
                    #endregion


                    #region EDIT
                    case ActionsEnum.EDIT:
                        GridViewRow gvrTemplate;
                        currentUser = (User)HttpContext.Current.User.Identity;
                        gvrTemplate = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        CurrPK = Convert.ToInt32(((HiddenField)grdRevisionList.Rows[gvrTemplate.RowIndex].FindControl("hdfDRM_PK")).Value);
                        GetFieldValues(ControlEnum.EDIT);
                        SetFieldValues(ControlEnum.EDIT);
                        break;
                    #endregion


                    #region DELETE
                    case ActionsEnum.REVISIONDELETE:
                        GridViewRow gvrTemplate1;
                        currentUser = (User)HttpContext.Current.User.Identity;
                        gvrTemplate1 = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        CurrPK = Convert.ToInt32(((HiddenField)grdRevisionList.Rows[gvrTemplate1.RowIndex].FindControl("hdfDRM_PK")).Value);

                        result = BusinessLogic.Administration.Masters.DocumentRevisionBL.DeleteDocumentRevision(CurrPK);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.Messages.DeletedSuccessfully;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DocumentRevision);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            ResetForm(ControlEnum.CLEAR);
                            EntryStatus = EntryStatus.LISTMODE;
                            GetFieldValues(ControlEnum.LIST);
                            SetFieldValues(ControlEnum.LIST);
                        }

                        else
                        {
                            if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.DATEOVERLAP)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("DateRangeExist").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_NotExist").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.REFNOEXIST)
                            {

                                litErrorMsg.Text = GetLocalResourceObject("Err_RevisionOnLateDate").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DocumentRevision);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                 
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ShiftSave);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
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

        protected void grdRevisionList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
            #region Set UIValues To Object
            private Object SetUIValuesToObject(ControlEnum controlType)
        {
            object returnObject = new object();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region DOCUMENTREVISION
                    case ControlEnum.DOCUMENTREVISION:
                        //objDocumentRevisionBO.DRM_PK = 0;
                        objDocumentRevisionBO.DRM_PK = CurrPK;
                        objDocumentRevisionBO.DRM_DOC_PK = Convert.ToInt32(ddlReport.SelectedValue);
                        objDocumentRevisionBO.DRM_DOC_NAME = ddlReport.SelectedItem.Text;
                        objDocumentRevisionBO.DRM_DOC_TYPE = Convert.ToInt32(rdoType.SelectedValue);
                        objDocumentRevisionBO.DRM_DAR_NO = txtDARNumber.Text;
                        objDocumentRevisionBO.DRM_DOC_NO = txtDocumentNo.Text;
                        objDocumentRevisionBO.DRM_REVISION = Convert.ToInt32(txtRevision.Text);
                        objDocumentRevisionBO.DRM_REV_DT = Convert.ToDateTime(txtRevisionDate.Text);
                        objDocumentRevisionBO.DRM_USER_PK = currentUser.PKUser;
                        returnObject = objDocumentRevisionBO;
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
                    //LastModifiedTime = DateTime.Now;
                    uclPaging.CurrentPage = 0;
                    PageIndex = 1;
                    ddlReport.Items.Clear();
                    ddlReport.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    txtDARNumber.Text = string.Empty;
                    txtRevision.Text = string.Empty;
                    txtRevisionDate.Text = string.Empty;
                    txtDocumentName.Text = string.Empty;
                    txtDocumentNo.Text = string.Empty;
                    rdoType.ClearSelection();
                    break;
                #endregion

                case ControlEnum.CLEARFILTER:

                    //CurrPK = 0;
                    uclPaging.CurrentPage = 0;
                    this.PageIndex = 1;
                    CurrPK = 0;
                    //this.EntryStatus = EntryStatus.LISTMODE;
                    txtRevisionDateFlter.Text = string.Empty;
                    txtRevisionFlter.Text = string.Empty;
                    txtDocumentNoFlter.Text = string.Empty;
                    ddlReportFlter.Items.Clear();
                    ddlReportFlter.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    rdoTypeFlter.ClearSelection();
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
            REPORT,
            NEW,
            LIST,
            CLEAR,
            CLEARFILTER,
            DOCUMENTREVISION,
            SHOWREPORTINFILTER,
            EDIT,
            DELETE
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
                    GetFieldValues(ControlEnum.LIST);
                    SetFieldValues(ControlEnum.LIST);
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