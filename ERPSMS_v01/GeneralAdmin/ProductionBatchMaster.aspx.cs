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
    public partial class ProductionBatchMaster : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        #region Variables
        User currentUser;
        private ProductionBatchBO objProductionBatchBO;
        //public DataTable dtShift;
        public DataTable dtProductionBatchList;
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
                ResetForm(ControlEnum.CLEAR);
                ResetForm(ControlEnum.CLEARFILTER);
                GetFieldValues(ControlEnum.LIST);
                SetFieldValues(ControlEnum.LIST);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearch", "ShowHideAdvancedSearch();", true);
                hdfShow.Value = string.Empty;
                //txtDate.Focus();
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
                case ControlEnum.LIST:
                    int PageNumber = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                    int PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                    dsResult = BusinessLogic.Administration.Masters.ProductionBatchBL.GetProductionBatchList(PageNumber, PageSize, currentUser.SBUID, txtBatchNoFilter.Text);
                    dtProductionBatchList = dsResult.Tables[1];
                    break;

                case ControlEnum.EDIT:
                    string Xml = BusinessLogic.Administration.Masters.ProductionBatchBL.GetProductionBatchDetails(Convert.ToInt32(CurrPK));
                    objProductionBatchBO = (ProductionBatchBO)CommonFunctions.DeserializeObject(Xml, new ProductionBatchBO());
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
                   case ControlEnum.EDIT:
                        CurrPK = Convert.ToInt32(objProductionBatchBO.PBN_PK.ToString());
                        txtDate.Text = Convert.ToDateTime(objProductionBatchBO.PBN_DATE).ToString("MMM-yyyy");
                        txtBatchNo.Text = objProductionBatchBO.PBN_BATCH_NO.ToString();
                        txtDescription.Text = objProductionBatchBO.PBN_BATCH_DESC;
                        chkActive.Checked = objProductionBatchBO.PBN_IS_ACTIVE == 1 ? true : false;
                        LastModifiedTime = Convert.ToDateTime(objProductionBatchBO.PBN_MOD_DT.ToString());
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
        //public void BindDropDown(ControlEnum controlType)
        //{
        //    try
        //    {
        //        switch (controlType)
        //        {
        //            case ControlEnum.REPORT:
        //                if (dtResult != null && dtResult.Rows.Count > 0)
        //                {
        //                    ddlReport.Items.Clear();
        //                    txtDocumentName.Text = string.Empty;
        //                    ddlReport.DataSource = dtResult;
        //                    ddlReport.DataTextField = "VALUE";
        //                    ddlReport.DataValueField = "PK";
        //                    ddlReport.DataBind();
        //                }
        //                ddlReport.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
        //                break;
        //            case ControlEnum.SHOWREPORTINFILTER:
        //                if (dtResult != null && dtResult.Rows.Count > 0)
        //                {
        //                    ddlReportFlter.Items.Clear();
        //                    ddlReportFlter.DataSource = dtResult;
        //                    ddlReportFlter.DataTextField = "VALUE";
        //                    ddlReportFlter.DataValueField = "PK";
        //                    ddlReportFlter.DataBind();
        //                }
        //                ddlReportFlter.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
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
                if (dtProductionBatchList != null && dtProductionBatchList.Rows.Count > 0)
                {
                    int rowCount = 0;
                    rowCount = Convert.ToInt32(dtProductionBatchList.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                    uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                      (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                      (rowCount / this.PageSize) + 1;
                    PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                    grdProductionBatchList.DataSource = dtProductionBatchList;
                    grdProductionBatchList.DataBind();
                    uclPaging.Visible = true;
                    uclPaging.BindPager();
                }
                else
                {
                    grdProductionBatchList.DataSource = null;
                    grdProductionBatchList.DataBind();
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
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlEnum.CLEARFILTER);
                        GetFieldValues(ControlEnum.LIST);
                        SetFieldValues(ControlEnum.LIST);
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ResetForm(ControlEnum.CLEAR);
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlEnum.LIST);
                        SetFieldValues(ControlEnum.LIST);
                        break;
                    #endregion
                    
                    #region SAVE
                    case ActionsEnum.SAVE:

                        objProductionBatchBO = new ProductionBatchBO();
                        objProductionBatchBO = (ProductionBatchBO)SetUIValuesToObject(ControlEnum.PRODUCTIONBATCH);
                        if (objProductionBatchBO != null)
                        {
                            string xmlDoc = CommonFunctions.XmlSerialize<ProductionBatchBO>(objProductionBatchBO);
                            result = BusinessLogic.Administration.Masters.ProductionBatchBL.SaveProductionBatch(xmlDoc);
                            if (result > 0)
                            {
                                litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ProductionBatch);
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
                                    litErrorMsg.Text = Resources.Messages.BatchAlreadyAdded;
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
                        GetFieldValues(ControlEnum.LIST);
                        SetFieldValues(ControlEnum.LIST);
                        break;
                    #endregion

                    #region EDIT
                    case ActionsEnum.EDIT_ACTION:
                        GridViewRow gvrTemplate;
                        currentUser = (User)HttpContext.Current.User.Identity;
                        gvrTemplate = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        CurrPK = Convert.ToInt32(((HiddenField)grdProductionBatchList.Rows[gvrTemplate.RowIndex].FindControl("hdPBN_PK")).Value);
                        GetFieldValues(ControlEnum.EDIT);
                        SetFieldValues(ControlEnum.EDIT);
                        break;
                    #endregion

                    #region DELETE
                    case ActionsEnum.DELETE:
                        GridViewRow gvrTemplate1;
                        currentUser = (User)HttpContext.Current.User.Identity;
                        gvrTemplate1 = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        CurrPK = Convert.ToInt32(((HiddenField)grdProductionBatchList.Rows[gvrTemplate1.RowIndex].FindControl("hdPBN_PK")).Value);

                        result = BusinessLogic.Administration.Masters.ProductionBatchBL.DeleteProductionBatch(CurrPK);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.Messages.DeletedSuccessfully;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ProductionBatch);
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
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ShiftSave);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                            }
                        }
                        break;

                    #endregion

                    #region ACTIVATE
                    // Do Action if click Activate Button
                    case ActionsEnum.ACTIVATE:
                        currentUser = (User)HttpContext.Current.User.Identity;
                        gvrTemplate = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        CurrPK = Convert.ToInt32(((HiddenField)grdProductionBatchList.Rows[gvrTemplate.RowIndex].FindControl("hdPBN_PK")).Value);
                        // check activated or not : Success - Return PK
                        result = BusinessLogic.Administration.Masters.ProductionBatchBL.UpdateProductionBatchStatus(CurrPK, 1, Convert.ToInt32(currentUser.PKUser.ToString()));
                        if (result > 0)
                        {
                            EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Activate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ProductionBatch);
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
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ProductionBatch);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ProductionBatch);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                default:
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                            }
                        }
                        GetFieldValues(ControlEnum.LIST);
                        SetFieldValues(ControlEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region DEACTIVATE
                    // Do Action if click DeActivate Button
                    case ActionsEnum.INACTIVATE:
                        currentUser = (User)HttpContext.Current.User.Identity;
                        gvrTemplate = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        CurrPK = Convert.ToInt32(((HiddenField)grdProductionBatchList.Rows[gvrTemplate.RowIndex].FindControl("hdPBN_PK")).Value);
                        // check activated or not : Success - Return PK
                        result = BusinessLogic.Administration.Masters.ProductionBatchBL.UpdateProductionBatchStatus(CurrPK, 0, Convert.ToInt32(currentUser.PKUser.ToString()));
                        if (result > 0)
                        {
                            EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_InActivate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ProductionBatch);
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
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ProductionBatch);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ProductionBatch);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                default:
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                            }
                        }
                        GetFieldValues(ControlEnum.LIST);
                        SetFieldValues(ControlEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
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

        protected void grdProductionBatchList_RowDeleting(object sender, GridViewDeleteEventArgs e)
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
                    #region ProductionBatch
                    case ControlEnum.PRODUCTIONBATCH:
                        //objProductionBatchBO.DRM_PK = 0;
                        //objProductionBatchBO.DRM_PK = CurrPK;
                        //objProductionBatchBO.DRM_DOC_PK = Convert.ToInt32(ddlReport.SelectedValue);
                        //objProductionBatchBO.DRM_DOC_NAME = ddlReport.SelectedItem.Text;
                        //objProductionBatchBO.DRM_DOC_TYPE = Convert.ToInt32(rdoType.SelectedValue);
                        //objProductionBatchBO.DRM_DAR_NO = txtDARNumber.Text;
                        //objProductionBatchBO.DRM_DOC_NO = txtDocumentNo.Text;
                        //objProductionBatchBO.DRM_REVISION = Convert.ToInt32(txtRevision.Text);
                        //objProductionBatchBO.DRM_REV_DT = Convert.ToDateTime(txtRevisionDate.Text);
                        //objProductionBatchBO.DRM_USER_PK = currentUser.PKUser;
                        objProductionBatchBO.PBN_IS_ACTIVE = chkActive.Checked == true ? 1 : 0;
                        objProductionBatchBO.PBN_BATCH_NO = txtBatchNo.Text;
                        objProductionBatchBO.PBN_BIZUNIT = currentUser.SBUID;
                        objProductionBatchBO.PBN_DATE = Convert.ToDateTime(txtDate.Text);
                        objProductionBatchBO.PBN_USER_PK = currentUser.PKUser;
                        objProductionBatchBO.PBN_PK = CurrPK;
                        objProductionBatchBO.PBN_BATCH_DESC = txtDescription.Text;
                        objProductionBatchBO.PBN_MOD_DT = LastModifiedTime;
                        returnObject = objProductionBatchBO;
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
                    uclPaging.CurrentPage = 0;
                    PageIndex = 1;
                    LastModifiedTime = DateTime.Now;
                    txtBatchNo.Text = string.Empty;
                    txtDescription.Text = string.Empty;
                    txtDate.Text = string.Empty;
                    chkActive.Checked = true;
                    break;
                #endregion

                case ControlEnum.CLEARFILTER:

                    //CurrPK = 0;
                    uclPaging.CurrentPage = 0;
                    this.PageIndex = 1;
                    CurrPK = 0;
                    //this.EntryStatus = EntryStatus.LISTMODE;
                    //txtRevisionDateFlter.Text = string.Empty;
                    //txtRevisionFlter.Text = string.Empty;
                    //txtDocumentNoFlter.Text = string.Empty;
                    //ddlReportFlter.Items.Clear();
                    //ddlReportFlter.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    //rdoTypeFlter.ClearSelection();
                    txtBatchNoFilter.Text = string.Empty;
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
            NEW,
            LIST,
            CLEAR,
            CLEARFILTER,
            PRODUCTIONBATCH,
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