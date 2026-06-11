using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject.AccountManagement;
using System.Data;
using ERPData;
using ERPManager;
using CustomControls;
using BusinessObject.POInvoicing;
using BusinessLogic.CommonManagement;
using BusinessObject;
using ERPSMS_v01.UserControls;
using BusinessObject.CommonManagement;
using BusinessLogic.AccountManagement;
using ERPService;
using System.Threading;
using System.Configuration;

namespace ERPSMS_v01.PurchaseOrderManagement
{
    public partial class PurchaseOrderTradingList : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties

        #region Properties
        /// Process ID of the Page
        /// </summary>
        private int PageProcessID
        {
            get
            {
                return this.ViewState["PageProcessID"] == null ? 0 : Convert.ToInt32(this.ViewState["PageProcessID"]);
            }
            set
            {
                this.ViewState["PageProcessID"] = value;
            }
        }

        /// <summary>
        /// Entry State for managing display status
        /// </summary>
        public EntryStatus EntryStatus
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
        private int POId
        {
            get
            {
                return this.ViewState["POId"] == null ? 0 : (int)this.ViewState["POId"];
            }
            set
            {
                this.ViewState["POId"] = value;
            }
        }
        private int PRDtlPK
        {
            get
            {
                return this.ViewState["PRDtlPK"] == null ? 0 : (int)this.ViewState["PRDtlPK"];
            }
            set
            {
                this.ViewState["PRDtlPK"] = value;
            }
        }
        private int PoDtlPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.PoDtlPK] == null ? 0 : (int)this.ViewState[ViewstateStrings.PoDtlPK];
            }
            set
            {
                this.ViewState[ViewstateStrings.PoDtlPK] = value;
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
                return this.ViewState[ViewstateStrings.TotalPages] == null ? 1 : (int)this.ViewState[ViewstateStrings.TotalPages];
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

        private ActionsEnum commonActions;
        private BusinessObject.User currentUser;
        DataSet dsPageData;
        DataTable dtPageData;
        int transTYPE = 0, transPK = 0;
        private ADM_APP_CONFIG_MST admAppConfigMstObj;
        private List<ADM_APP_CONFIG_MST> admAppConfigMstList;
        #endregion
        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                ucTrxComments.AfterCommentControlEvent += new EventHandler(CommentControlHandler);
                if (!IsPostBack)
                {
                    if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
                        hdfDeptID.Value = Session[BusinessObject.Common.SessionStrings.CurDept].ToString();
                    else
                        hdfDeptID.Value = "0";
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfDecimalFormatWithSeperation.Value = "#" + currencysep + "#0.";
                    int NoDecimalDigitsP2P = Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P].ToString()) : 2;
                    for (int i = 0; i < NoDecimalDigitsP2P; i++)
                    {
                        hdfDecimalFormatWithSeperation.Value += "0";
                    }
                    hdnShortCloseGroup.Value = ConfigurationManager.AppSettings["ShortCloseGroup"].ToString();
                    hdnRoleID.Value = currentUser.Roles;
                    btnAdd.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(currentUser.PKUser, FillProcessId());
                    hdfAppType.Value = ApplicationType.POT;
                    hdfAppSubType.Value = string.Empty;
                    GetUserRights();
                    ConfigurationSettings();

                    FillProcessId();
                    GetFieldValues(ControlsEnum.TRANSACTIONSTATUS);
                    SetFieldValues(ControlsEnum.TRANSACTIONSTATUS);
                    GetFieldValues(ControlsEnum.POTYPES);//Local,Import
                    SetFieldValues(ControlsEnum.POTYPES);
                    if (!string.IsNullOrEmpty(hdfIsMultiplePlant.Value) && hdfIsMultiplePlant.Value == "1")
                    {
                        GetFieldValues(ControlsEnum.PLANT);
                        SetFieldValues(ControlsEnum.PLANT);
                    }
                    SetFieldValues(ControlsEnum.DEFAULT);
                    grdPOItems.DataSource = null;
                    grdPOItems.DataBind();
                        //Hide or Show IO Number 
                    if (hdfShowIONo.Value == "0")
                    {
                        lblIONo.Visible = false;
                        txtIONo.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        #endregion
        #region PageLevel Events
        /// <summary>
        /// Page load event
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
            BusinessObject.GridPrams gridParam;
            int processId;
            string pageUrl = string.Empty;
            try
            {
                switch (type)
                {
                    #region Purchase Request List
                    case ControlsEnum.DEFAULT:
                        gridParam = new BusinessObject.GridPrams();
                        gridParam.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        gridParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        gridParam.Fields = GetLocalResourceObject("GridFields").ToString();
                        gridParam.SortBy = GetLocalResourceObject("GridSortBy").ToString(); //POH_DATE
                        gridParam.SortDirection = GetLocalResourceObject("GridSortDirection").ToString();//DESC;
                        gridParam.FromDate = txtFromDate.Text;
                        gridParam.ToDate = txtToDate.Text;
                        gridParam.FilterStatus = ddlStatus.SelectedValue;
                        gridParam.UserPK = currentUser.PKUser;

                        processId = GetNullableInt(hdfProcId.Value) ?? 0;
                        pageUrl = Resources.PageURL.PurchaseOrderTrading; // "/PurchaseOrderManagement/PurchaseOrderTrading.aspx?TYPE=1"; 
                        int transactionStatus = string.IsNullOrEmpty(ddlTrnStatus.SelectedValue) ? -1 : Convert.ToInt32(ddlTrnStatus.SelectedValue);
                        int reqFor = string.IsNullOrEmpty(hdfReqFor.Value) ? 0 : Convert.ToInt32(hdfReqFor.Value);
                        string itemName = txtItemname.Text.Trim() == "Select/Type" ? string.Empty : txtItemname.Text.Trim();
                        int vendorPk = string.IsNullOrEmpty(hdfVendor.Value) ? 0 : Convert.ToInt32(hdfVendor.Value);
                        int filterStatus = string.IsNullOrEmpty(ddlStatus.SelectedValue) ? 0 : Convert.ToInt32(ddlStatus.SelectedValue);
                        string poNo = txtPONumber.Text.Trim() == "Select/Type" ? string.Empty : txtPONumber.Text.Trim();
                        string prNo = txtPrNo.Text.Trim() == "Select/Type" ? string.Empty : txtPrNo.Text.Trim();
                        string ioNo = txtIONo.Text.Trim() == "Select/Type" ? string.Empty : txtIONo.Text.Trim();
                        byte orderGroup = 1;
                        int cmpPk = string.IsNullOrEmpty(ddlPlantCode.SelectedValue) ? 0 : Convert.ToInt32(ddlPlantCode.SelectedValue);
                        int poType = string.IsNullOrEmpty(ddlPOType.SelectedValue) ? 0 : Convert.ToInt32(ddlPOType.SelectedValue);

                        dsPageData = DataAccess.PurchaseOrderManagement.PurchaseOrderGeneration.GetPurchaseOrderListTrading(gridParam, currentUser.SBUID, pageUrl, filterStatus, transactionStatus, vendorPk, poNo, prNo, ioNo, reqFor, orderGroup, itemName, cmpPk, poType);


                        break;
                    #endregion
                    #region Transaction Status
                    case ControlsEnum.TRANSACTIONSTATUS:
                        dtPageData = CommonBL.GetAppStatus("POT", string.Empty);
                        break;
                    #endregion
                    #region Plant
                    case ControlsEnum.PLANT:
                        string searchBy = GTIService.Constants.Common.CommonConstants.CMP_DISPLAY_CODE;
                        dtPageData = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetDirectGRNAutocomplete(searchBy, string.Empty, currentUser);
                        break;
                    #endregion
                    #region TRANSACTIONDETAILS
                    case ControlsEnum.TRANSACTIONDETAILS:
                        dtPageData = BusinessLogic.PurchaseOrderManagement.PurchaseOrderGeneration.GetPOListItemDetails(transTYPE, transPK);
                        break;
                    #endregion
                    #region POTYPES
                    case ControlsEnum.POTYPES:
                        dtPageData = DataAccess.PurchaseOrderManagement.PurchaseOrderGenerateDL.GetPurchaseOrderTypes(currentUser, 1);
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
                    #region DEFAULT
                    case ControlsEnum.DEFAULT:
                        BindGrid(controlType);
                        BindGrid(ControlsEnum.GRN);
                        break;
                    #endregion
                    case ControlsEnum.TRANSACTIONSTATUS:
                        BindDropDownList(ControlsEnum.TRANSACTIONSTATUS);
                        break;
                    case ControlsEnum.PLANT:
                        BindDropDownList(ControlsEnum.PLANT);
                        break;
                    #region PR Details
                    case ControlsEnum.TRANSACTIONDETAILS:
                        BindGrid(controlType);
                        break;
                    #endregion
                    #region POTYPES
                    case ControlsEnum.POTYPES:
                        BindDropDownList(ControlsEnum.POTYPES);
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
        #region Action Handlers

        #region -- For Buttons ---
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            GridViewRow gvr;
            GridView grd;
            string arg;
            string reportPK = "0";
            string appType = string.Empty;
            string RptSubType = string.Empty;
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
                #region NEW
                case ActionsEnum.NEW:
                    Response.Redirect(GetLocalResourceObject("PurchaseOrderEntryURL").ToString());
                    break;
                #endregion
                #region Search
                case ActionsEnum.SEARCH:
                    PageIndex = "1";
                    SetFieldValues(ControlsEnum.DEFAULT);
                    grdPOItems.DataSource = null;
                    grdPOItems.DataBind();
                    break;
                #endregion
                #region CLEAR
                case ActionsEnum.CLEAR:
                    ResetForm();
                    PageIndex = "1";
                    SetFieldValues(ControlsEnum.DEFAULT);
                    grdPOItems.DataSource = null;
                    grdPOItems.DataBind();
                    break;
                #endregion
                #region POITEMDETAILS
                case ActionsEnum.POITEMDETAILS:
                    hdfSelectedItemPOPK.Value = "0";
                    gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                    POId = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfPOHPK")).Value);
                    transTYPE = 1;//PO
                    transPK = POId;
                    GetFieldValues(ControlsEnum.TRANSACTIONDETAILS);
                    SetFieldValues(ControlsEnum.TRANSACTIONDETAILS);
                    hdfSelectedItemPOPK.Value = POId.ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ItemSelect", "ItemListSelection();", true);
                    break;
                #endregion
                #region extra grid ondemand data population
                #region GRNDETAILS
                case ActionsEnum.GRNDETAILS:
                    arg = ((Button)sender).CommandArgument;
                    gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                    if (gvr != null)
                    {
                        grd = gvr.FindControl("grdGRNList") as GridView;
                        if (string.IsNullOrEmpty(arg))
                        {
                            dtPageData = null;
                        }
                        else
                        {
                            transTYPE = 2;//GRN
                            transPK = Convert.ToInt32(arg);
                            GetFieldValues(ControlsEnum.TRANSACTIONDETAILS);
                        }
                        grd.Visible = true;
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            grd.DataSource = dtPageData;
                            grd.DataBind();
                        }
                        (gvr.FindControl("hdfIsExpandedPOItem") as HiddenField).Value = "1";
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ItemSelect", "ItemListSelection();", true);
                    }
                    break;
                #endregion
                #region GINDETAILS
                case ActionsEnum.GINDETAILS:
                    arg = ((Button)sender).CommandArgument;
                    gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                    if (gvr != null)
                    {
                        grd = gvr.FindControl("grdGINList") as GridView;
                        if (string.IsNullOrEmpty(arg))
                        {
                            dtPageData = null;
                        }
                        else
                        {
                            transTYPE = 3;//GIN
                            transPK = Convert.ToInt32(arg);
                            GetFieldValues(ControlsEnum.TRANSACTIONDETAILS);
                        }
                        grd.Visible = true;
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            grd.DataSource = dtPageData;
                            grd.DataBind();
                        }
                        (gvr.FindControl("hdfIsExpandedGRNList") as HiddenField).Value = "1";
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ItemSelect", "ItemListSelection();", true);
                    }
                    break;
                #endregion
                #region STDETAILS
                case ActionsEnum.STDETAILS:
                    arg = ((Button)sender).CommandArgument;
                    gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                    if (gvr != null)
                    {
                        grd = gvr.FindControl("grdStockTransfer") as GridView;
                        if (string.IsNullOrEmpty(arg))
                        {
                            dtPageData = null;
                        }
                        else
                        {
                            transTYPE = 4;//Stock Admission
                            transPK = Convert.ToInt32(arg);
                            GetFieldValues(ControlsEnum.TRANSACTIONDETAILS);
                        }
                        grd.Visible = true;
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            grd.DataSource = dtPageData;
                            grd.DataBind();
                        }
                        (gvr.FindControl("hdfIsExpandedGinList") as HiddenField).Value = "0";
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ItemSelect", "ItemListSelection();", true);
                    }
                    break;
                #endregion
                #endregion
                #region SHORTCLOSESAVE
                case ActionsEnum.SHORTCLOSESAVE:
                    BusinessObject.PurchaseOrderGeneration.POShortClose poShortClose = new BusinessObject.PurchaseOrderGeneration.POShortClose();
                    poShortClose.POID = Convert.ToInt32(POID.Value);
                    poShortClose.RefNo = RefNo.Text;
                    poShortClose.Remarks = Remarks.Text;
                    poShortClose.UserPk = currentUser.PKUser;
                    int shortCloseResult = DataAccess.PurchaseOrderManagement.PurchaseOrderGeneration.SavePOShortClose(poShortClose);
                    if (shortCloseResult == 1)
                    {
                        POID.Value = "0";
                        litErrorMsg.Text = Resources.Messages.SaveClosePo;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "');", true);
                        ActionHandler(btnClear, EventArgs.Empty);
                    }
                    else
                    {
                        if (shortCloseResult == -27)
                        {
                            litErrorMsg.Text = Resources.Messages.GrnNotApproved;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (shortCloseResult == -28)
                        {
                            litErrorMsg.Text = Resources.Messages.GrnDraftSaveExists;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("PurchaseOrder").ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                    }

                    break;
                #endregion
                #region PRINT GRN,GIN,SA
                #region PRINT GRN
                case ActionsEnum.PRINTGRN:
                    reportPK = ((LinkButton)sender).CommandArgument;
                    appType = ApplicationType.GRN;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ItemSelect", "ItemListSelection();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + reportPK + "&APPTYPE=" + appType + "&APPSUBTYPE=" + RptSubType) + "');", true);
                    break;
                #endregion
                #region PRINT GIN
                case ActionsEnum.PRINTGIN:
                    reportPK = ((LinkButton)sender).CommandArgument;
                    appType = string.Empty;
                    DataTable dt = CommonBL.GetApplicaitonConfiguaration("CLIENT CODE", string.Empty);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["ACF_DATA"].ToString() == GetLocalResourceObject("ClientCode").ToString()) //EKK
                            appType = ApplicationType.GRN;
                        else
                            appType = ApplicationType.GIN;
                    }
                    else
                        appType = ApplicationType.GIN;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ItemSelect", "ItemListSelection();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + reportPK + "&APPTYPE=" + appType + "&APPSUBTYPE=" + RptSubType) + "');", true);
                    break;
                #endregion
                #region PRINT Stock admission
                case ActionsEnum.PRINTSA:
                    reportPK = ((LinkButton)sender).CommandArgument;
                    string StockTransferReportURL = GetLocalResourceObject("StockTransferReportURL").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ItemSelect", "ItemListSelection();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(StockTransferReportURL + "?PK=" + reportPK) + "');", true);
                    break;
                #endregion
                #endregion
            }
        }

        #endregion

        #region --- For Grid Actions----

        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {

            try
            {
                #region Grid Fixed Columns
                if ((sender as GridView).ID == "grdPOList")
                {
                    int isClosePO = Convert.ToInt32(hdnClosePO.Value);
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        ImageButton imbEdit = e.Row.FindControl("imbEdit") as ImageButton;
                        ImageButton imbDelete = e.Row.FindControl("imbDelete") as ImageButton;
                        ImageButton imbView = e.Row.FindControl("imbView") as ImageButton;
                        ImageButton imbPrint = e.Row.FindControl("imbPrint") as ImageButton;
                        ImageButton imbShortClose = e.Row.FindControl("imbShortClose") as ImageButton;
                        ImageButton imbPRCancel = e.Row.FindControl("imbPRCancel") as ImageButton;
                        ImageButton imbNoComments = e.Row.FindControl("imbNoComments") as ImageButton;
                        ImageButton imbComnts = e.Row.FindControl("imbComnts") as ImageButton;
                        Button imgbtnPOHierarchyLevel = e.Row.FindControl("imgbtnPOHierarchyLevel") as Button;


                        int UserStatus = Convert.ToInt32((e.Row.FindControl("hdfUserStatus") as HiddenField).Value);
                        int poStatus = Convert.ToInt32((e.Row.FindControl("hdfPOHStatus") as HiddenField).Value);
                        int isCommented = Convert.ToInt32((e.Row.FindControl("hdfPohIsCommentExist") as HiddenField).Value);

                        #region Show/Hide grdPOList image buttons w.r.to previlege

                        if (UserStatus == 1)
                        {
                            imbEdit.Visible = true;
                            imbDelete.Visible = false;
                            imbView.Visible = false;
                        }
                        else if (UserStatus == 0)
                        {
                            imbEdit.Visible = false;
                            imbDelete.Visible = false;
                        }
                        else if (UserStatus == 2)
                        {
                            imbEdit.Visible = true;
                            imbDelete.Visible = true;
                            imbView.Visible = false;
                        }
                        if (isClosePO == 1 && poStatus != 0 && poStatus != 4 && poStatus != 5 && poStatus != 3)//0-->Draft,4-->Shortclosed,5-->Closed
                        {
                            imbShortClose.Visible = true;
                        }
                        else
                        {
                            imbShortClose.Visible = false;
                        }
                        if (poStatus == 4)
                        {
                            imbEdit.Visible = false;
                            imbDelete.Visible = false;
                            imbView.Visible = true;
                        }
                        if (isCommented == 1)
                        {
                            imbNoComments.Visible = false;
                            imbComnts.Visible = true;
                        }
                        else
                        {
                            imbNoComments.Visible = true;
                            imbComnts.Visible = false;
                        }
                        #endregion

                        string poNumber = (e.Row.FindControl("lblPONo") as Label).Text;
                        if (string.IsNullOrEmpty(poNumber))
                        {
                            (e.Row.FindControl("lblPONo") as Label).Text = "[NEW]";
                            (e.Row.FindControl("hdfPohNo") as HiddenField).Value = "[NEW]";
                        }
                        #region Colour Changing w. r. to hierachy level

                        #endregion
                    }
                }
                #endregion
                if (((GridView)sender).ID == "grdStockTransfer")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        (e.Row.FindControl("hdfHasChildren") as HiddenField).Value = CommonConstants.SELECT_VALUE_ZERO;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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
                    if (SortDirection == Resources.ErpRes.SortAscending)
                        SortDirection = Resources.ErpRes.SortDescending;
                    else
                        SortDirection = Resources.ErpRes.SortAscending;
                }
                else
                {
                    SortBy = e.SortExpression;
                    SortDirection = Resources.ErpRes.SortAscending;
                }

                this.PageIndex = CommonConstants.SELECT_VALUE_ONE;
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        /// <summary>
        /// Method used to Handle all Command actions of gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {

            GridView senderGridView = (GridView)sender;
            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            int poId = 0, refID = 0, UserStatus = 0;
            string pohNo = string.Empty;
            poId = Convert.ToInt32((row.FindControl("hdfPOHPK") as HiddenField).Value);
            refID = Convert.ToInt32((row.FindControl("hdfRefId") as HiddenField).Value);
            UserStatus = Convert.ToInt32((row.FindControl("hdfUserStatus") as HiddenField).Value);
            pohNo = Convert.ToString((row.FindControl("hdfPohNo") as HiddenField).Value);
            int? result;
            if (senderGridView.ID == "grdPOList")
            {
                switch (e.CommandName)
                {
                    case "PERFORMACTION":
                        #region PERFORMACTION
                        if (UserStatus == 1)
                        {
                            Response.Redirect(GetLocalResourceObject("PurchaseOrderEntryURL").ToString() + "?RefID=" + refID);
                        }
                        else if (UserStatus == 2)
                        {
                            Response.Redirect(GetLocalResourceObject("PurchaseOrderEntryURL").ToString() + "?POID=" + poId);
                        }
                        #endregion
                        break;
                    case "DELETEPO":
                        #region DELETEPO
                        result = DataAccess.PurchaseOrderManagement.PurchaseOrderGeneration.DeletePODetails(poId);
                        if (result == 1)
                        {
                            litErrorMsg.Text = Resources.Messages.PODeletedSucessfully;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                            ActionHandler(btnClear, EventArgs.Empty);// SetFieldValues(ControlsEnum.DEFAULT);
                        }
                        else
                        {
                            if (result == 0)
                            {
                                litErrorMsg.Text = Resources.Messages.CannotDelete;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("PurchaseOrder").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        #endregion
                        break;
                    case "SHORTCLOSE":
                        #region SHORTCLOSE
                        ResetShortClose();
                        lblPOH_NO.Text = pohNo;
                        POID.Value = poId.ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divShortClose]','" + Resources.Controls.ShortClose + "','" + "480" + "','" + "230" + "');", true);
                        #endregion
                        break;
                    case "VIEW":
                        #region VIEW
                        if (UserStatus == 1)
                        {
                            Response.Redirect(GetLocalResourceObject("PurchaseOrderEntryURL").ToString() + "?RefID=" + refID + "&Status=1");
                        }
                        else if (UserStatus == 2)
                        {
                            Response.Redirect(GetLocalResourceObject("PurchaseOrderEntryURL").ToString() + "?PK=" + poId + "&Status=1");
                        }
                        else if (UserStatus == 0)
                        {
                            if (refID == 0)
                            {
                                Response.Redirect(GetLocalResourceObject("PurchaseOrderEntryURL").ToString() + "?PK=" + poId + "&Status=1");
                            }
                            else
                            {
                                Response.Redirect(GetLocalResourceObject("PurchaseOrderEntryURL").ToString() + "?RefID=" + refID + "&Status=1");
                            }
                        }
                        #endregion
                        break;
                    case "PRINT":
                        #region PRINT
                        //string url = GetLocalResourceObject("PurchaseOrderReportURL").ToString() + "?ID=" + poId + "&APPTYPE=" + hdfAppType.Value.ToString() + "&APPSUBTYPE=" + hdfAppSubType.Value.ToString();
                        string url = GetLocalResourceObject("PurchaseOrderReportURL").ToString() + "?ID=" + poId + "&APPTYPE=POT" + "&APPSUBTYPE=" + hdfAppSubType.Value.ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "OpenPDF", "OpenPDF('" + url + "');", true);
                        #endregion
                        break;
                    #region COMMENTS
                    case "COMMENTS":
                        ucTrxComments.TrxNo = Convert.ToString((row.FindControl("hdfPohNo") as HiddenField).Value);
                        ucTrxComments.TrxPk = Convert.ToInt32((row.FindControl("hdfPOHPK") as HiddenField).Value);
                        ucTrxComments.AppType = ApplicationType.POT;
                        ucTrxComments.InitializeControl();
                        ShowCommentPopup();
                        break;
                    #endregion
                }
            }
        }

        #endregion

        #endregion
        #region Helper Methods
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int FillProcessId()
        {
            int procId = 0;         
            string path = "/PurchaseOrderManagement/PurchaseOrderTrading.aspx?TYPE=1";
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                procId = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                hdfProcId.Value = procId.ToString();
            }
            return procId;
        }

        private int? GetNullableInt(string str)
        {
            int result;
            return (int.TryParse(str, out result) ? (int?)result : null);
        }
        public string GetFormattedNumberWithSeperation(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormatWithSeperation.Value);
        }

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <param name="mode">Save Action</param>
        /// <returns>Object to Save</returns>
        private object SetUIValuesToObject(ActionsEnum mode)
        {
            object returnObj = null;
            try
            {
                bool bIsChecked = false;

                foreach (GridViewRow grdrow in grdPOList.Rows)
                {
                    RadioButton rbtn;
                    rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                    if (rbtn.Checked)
                    {
                        bIsChecked = true;
                        break;
                    }
                }

                return returnObj;
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
                        GetFieldValues(ControlsEnum.DEFAULT);

                        int rowCount = 0;
                        int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        if (dsPageData != null && dsPageData.Tables[0].Rows.Count > 0)
                        {
                            rowCount = Convert.ToInt32(dsPageData.Tables[0].Rows[0][0].ToString());
                        }
                        uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                      (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                      (rowCount / pageSize) + 1;
                        PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        grdPOList.DataSource = dsPageData.Tables[1];
                        grdPOList.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                        //Hide or Show IO Number Column
                        if (hdfShowIONo.Value == "1")
                            grdPOList.Columns[7].Visible = true;
                        else
                            grdPOList.Columns[7].Visible = false;
                        break;
                    #endregion
                    #region PRDETAILS
                    case ControlsEnum.TRANSACTIONDETAILS:
                        grdPOItems.DataSource = dtPageData;
                        grdPOItems.DataBind();
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
        /// Method for Dropdownlist binding
        /// </summary>
        private void BindDropDownList(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region TRANSACTIONSTATUS
                    case ControlsEnum.TRANSACTIONSTATUS:
                        ddlTrnStatus.Items.Clear();
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            ddlTrnStatus.DataValueField = GTIService.Constants.Common.Fields.ASC_VALUE;
                            ddlTrnStatus.DataTextField = GTIService.Constants.Common.Fields.ASC_NAME;
                            ddlTrnStatus.DataSource = dtPageData;
                            ddlTrnStatus.DataBind();
                        }
                        ddlTrnStatus.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                        foreach (ListItem item in ddlTrnStatus.Items)
                        {
                            item.Text = HttpUtility.HtmlDecode(item.Text);
                        }
                        break;
                    #endregion
                    #region Plant
                    case ControlsEnum.PLANT:
                        ddlPlantCode.Items.Clear();
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            ddlPlantCode.DataValueField = GTIService.Constants.Designation.Fields.PK;
                            ddlPlantCode.DataTextField = GTIService.Constants.Designation.Fields.VALUE;
                            ddlPlantCode.DataSource = dtPageData;
                            ddlPlantCode.DataBind();
                        }
                        ddlPlantCode.Items.Insert(0, new ListItem(Resources.Report.SelectAll, CommonConstants.SELECT_VALUE_ZERO));
                        foreach (ListItem item in ddlPlantCode.Items)
                        {
                            item.Text = HttpUtility.HtmlDecode(item.Text);
                        }
                        break;
                    #endregion
                    #region POTYPES
                    case ControlsEnum.POTYPES:
                        ddlPOType.Items.Clear();
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            ddlPOType.DataValueField = GTIService.Constants.Common.Fields.ADM_CFG_VALUE;
                            ddlPOType.DataTextField = GTIService.Constants.Common.Fields.ADM_CFG_TEXT;
                            ddlPOType.DataSource = dtPageData;
                            ddlPOType.DataBind();
                        }
                        ddlPOType.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                        foreach (ListItem item in ddlPOType.Items)
                        {
                            item.Text = HttpUtility.HtmlDecode(item.Text);
                        }
                        break;
                    #endregion

                    default: break;
                }
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
            txtFromDate.Text = string.Empty;
            hdfFromDate.Value = string.Empty;
            txtToDate.Text = string.Empty;
            txtItemname.Text = string.Empty;
            txtIONo.Text = string.Empty;
            txtVendor.Text = string.Empty;
            hdfVendor.Value = "0";
            txtReqFor.Text = string.Empty;
            hdfReqFor.Value = "0";
            txtPONumber.Text = string.Empty;
            hdfPONumber.Value = "0";
            ddlStatus.SelectedValue = "-5";
            ddlPlantCode.SelectedValue = "0";
            ddlTrnStatus.SelectedValue = "-1";
        }
        private void ConfigurationSettings()
        {
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
            hdfShowIONo.Value = GetGlobalResourceObject("ConfigurationsRes", "IsShowIONoInPR").ToString();
        }
        private void GetUserRights()
        {
            string path = GetLocalResourceObject("POShortClosureURL").ToString();//Purchase Order - Short Closure
            UserAuthBL userAuth = new UserAuthBL();
            UserRightsBO usrRights = userAuth.GetUserRights(currentUser.PKUser, path, currentUser.SBUID, currentUser.CurrentDeptPK);
            if (usrRights.Rights.Count > 0)
            {
                for (int i = 0; i < usrRights.Rights.Count; i++)
                {
                    if (usrRights.Rights[i].ActionName == "SHORTCLOSURE" && usrRights.Rights[i].HasActionRight == true && usrRights.Rights[i].UserDeptRight == true)
                    {
                        hdnClosePO.Value = "1";
                        break;
                    }
                }
            }
        }
        private void ShowCommentPopup()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divTrxComments]','" + Resources.Captions.Comments + "','" + Resources.Constants.TrxComments_Width + "','" + Resources.Constants.TrxComments_Height + "');", true);
        }
        ///<summary>Function To reset the short close details
        ///</summary>
        private void ResetShortClose()
        {
            Remarks.Text = string.Empty;
            RefNo.Text = string.Empty;
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
        /// <summary>
        /// Button Load event
        /// Resets visibility
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_Load(object sender, EventArgs e)
        {
            (sender as Control).Visible = true;
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage
        /// Leave this section if using Master Screens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_PreRender(object sender, EventArgs e)
        {
            base.CheckBtnVisibility(sender);
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ItemSelect", "ItemListSelection();", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
        }

        #endregion

        #region Enum
        /// <summary>
        /// Define Common Enum
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            PRDETAILS,
            PO,
            POT,
            GRN,
            GIN,
            STOCKTRANSFER,
            TYPECATEGORY,
            FINPERIOD,
            EMPTYGRID,
            COMPANY,
            PODEPARTMENTSBYUSER,
            TRANSACTIONSTATUS,
            PLANT,
            TRANSACTIONDETAILS,
            POTYPES
        }

        #endregion
        #region Comment User Control Event Handler
        protected void CommentControlHandler(object sender, EventArgs e)
        {
            ShowCommentPopup();
            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ResetPage", "$(document).ready(function(){ResetPage();});", true);
            ActionHandler(btnClear, EventArgs.Empty);
        }
        #endregion
    }
}