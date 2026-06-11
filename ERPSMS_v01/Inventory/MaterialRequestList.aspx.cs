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

namespace ERPSMS_v01.Inventory
{
    public partial class MaterialRequestList : ERP.Store.UI.MyBasePage
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
        private string ShowPRtype
        {
            get
            {
                return this.ViewState["ShowPRtype"] == null ? "0" : this.ViewState["ShowPRtype"].ToString();
            }
            set
            {
                this.ViewState["ShowPRtype"] = value;
            }
        }
        private string ShowItemCategory
        {
            get
            {
                return this.ViewState["ShowItemCategory"] == null ? "0" : this.ViewState["ShowItemCategory"].ToString();
            }
            set
            {
                this.ViewState["ShowItemCategory"] = value;
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
        private int PRId
        {
            get
            {
                return this.ViewState["PRId"] == null ? 0 : (int)this.ViewState["PRId"];
            }
            set
            {
                this.ViewState["PRId"] = value;
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
        int grnDtlId;
        int ginId;
        DataTable dtType;
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
                    if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
                        hdfDeptID.Value = Session[BusinessObject.Common.SessionStrings.CurDept].ToString();
                    else
                        hdfDeptID.Value = "0";
                    hdfAppType.Value = ApplicationType.MTR;
                    hdfAppSubType.Value = string.Empty;
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfDecimalFormatWithSeperation.Value = "#" + currencysep + "#0.";
                    int NoDecimalDigitsP2P = Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P].ToString()) : 2;
                    for (int i = 0; i < NoDecimalDigitsP2P; i++)
                    {
                        hdfDecimalFormatWithSeperation.Value += "0";
                    }
                    if (Request.QueryString["TYPE"] != null && Convert.ToInt32(Request.QueryString["TYPE"]) > 0)
                        hdfMenuType.Value = Request.QueryString["TYPE"];

                    btnAdd.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(currentUser.PKUser, FillProcessId());
                    if (Request.QueryString["Type"] != null)
                    {
                        ITM_SET.Value = Request.QueryString["Type"].ToString();
                    }
                    else
                        ITM_SET.Value = "1";
                    GetUserRights();
                    ConfigurationSettings();
                    FillProcessId();
                    GetFieldValues(ControlsEnum.TRANSACTIONSTATUS);
                    SetFieldValues(ControlsEnum.TRANSACTIONSTATUS);
                    if (!string.IsNullOrEmpty(hdfIsMultiplePlant.Value) && hdfIsMultiplePlant.Value == "1")
                    {
                        GetFieldValues(ControlsEnum.PLANT);
                        SetFieldValues(ControlsEnum.PLANT);
                    }
                    SetFieldValues(ControlsEnum.DEFAULT);
                    grdPRItems.DataSource = null;
                    grdPRItems.DataBind();
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
        protected override void OnLoadComplete(EventArgs e)
        {
            if (!IsPostBack)
            {
                //assign Page BreadCrumb
                base.OnLoadComplete(e);
                AssignLocalBreadCrumb();
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
            BusinessObject.GridPrams gridParam;
            int processId;
            string pageUrl = string.Empty;
            try
            {
                switch (type)
                {
                    case ControlsEnum.PRTYPE:
                        dtType = DataAccess.Administration.Masters.CompanyMasterDA.GetConstantValue(0, 1, 0, 10, 3, currentUser.SBUID);
                        break;
                    #region Purchase Request List
                    case ControlsEnum.DEFAULT:
                        int prGroup = 0;
                        gridParam = new BusinessObject.GridPrams();
                        gridParam.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        gridParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        gridParam.Fields = GetLocalResourceObject("GridFields").ToString();
                        gridParam.SortBy = GetLocalResourceObject("GridSortBy").ToString(); //PRH_DATE
                        gridParam.SortDirection = GetLocalResourceObject("GridSortDirection").ToString();//DESC;
                        gridParam.FromDate = txtFromDate.Text;
                        gridParam.ToDate = txtToDate.Text;
                      //  gridParam.FilterStatus = ddlStatus.SelectedValue;
                        gridParam.UserPK = currentUser.PKUser;

                        processId = GetNullableInt(hdfProcId.Value) ?? 0;
                        pageUrl = "/Inventory/MaterialRequest.aspx";
                         int MenuType = 0;
                        if ((hdfMenuType.Value == "" ? 0 : Convert.ToInt32(hdfMenuType.Value)) > 0)
                        {
                            pageUrl += "?TYPE=" + hdfMenuType.Value;
                            MenuType = hdfMenuType.Value == "" ? 0 : Convert.ToInt32(hdfMenuType.Value);
                        }
                        gridParam.POH_MENU_TYPE = MenuType;

                        //if (ShowItemCategory == "1")
                        //    prGroup = Convert.ToInt32(ddlItemCategory.SelectedValue);
                        int transactionStatus = string.IsNullOrEmpty(ddlTrnStatus.SelectedValue) ? -1 : Convert.ToInt32(ddlTrnStatus.SelectedValue);
                        int reqStore = string.IsNullOrEmpty(hdfReqStore.Value) ? 0 : Convert.ToInt32(hdfReqStore.Value);
                        int itemCode = string.IsNullOrEmpty(hdfItemcode.Value) ? 0 : Convert.ToInt32(hdfItemcode.Value);
                        hdfItemName.Value = txtItemname.Text;
                        int reqDept = string.IsNullOrEmpty(hdfReqDept.Value) ? 0 : Convert.ToInt32(hdfReqDept.Value);
                      //  int filterStatus = string.IsNullOrEmpty(ddlStatus.SelectedValue) ? 0 : Convert.ToInt32(ddlStatus.SelectedValue);
                        string prNo = txtPRNumber.Text.Trim() == "Select/Type" ? string.Empty : txtPRNumber.Text.Trim();
                      //  string ioNo = txtIONo.Text.Trim() == "Select/Type" ? string.Empty : txtIONo.Text.Trim();
                        string reqBy = txtReqBy.Text.Trim() == "Select/Type" || txtReqBy.Text.Trim() == string.Empty ? "" : txtReqBy.Text.Trim();
                        int cmpPk = string.IsNullOrEmpty(ddlPlantCode.SelectedValue) ? 0 : Convert.ToInt32(ddlPlantCode.SelectedValue);
                        int POCategory = string.IsNullOrEmpty(ddlPOHType.SelectedValue) ? 0 : Convert.ToInt32(ddlPOHType.SelectedValue);
                        dsPageData = DataAccess.PurchaseRequestManagement.PurchaseRequestListingDL.GetMaterialRequestList(gridParam, currentUser, pageUrl, processId, transactionStatus, reqStore, null
                            , itemCode, reqDept, reqBy, 0, prNo, cmpPk, POCategory, prGroup);

                        break;
                    #endregion
                    #region Transaction Status
                    case ControlsEnum.TRANSACTIONSTATUS:
                        dtPageData = CommonBL.GetAppStatus("MTR", string.Empty);
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
                        dtPageData = BusinessLogic.PurchaseRequestManagement.PurchaseRequestCreation.GetPRListItemDetails(transTYPE, transPK);
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
                    #region MTR Details
                    case ControlsEnum.TRANSACTIONDETAILS:
                        BindGrid(controlType);
                        break;
                    #endregion
                    case ControlsEnum.PRTYPE:
                        BindDropDownList(ControlsEnum.PRTYPE);
                        break;
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
                    string url = GetLocalResourceObject("MaterialRequestEntryURL").ToString();
                    if (Request.QueryString["TYPE"] != null && Convert.ToInt32(Request.QueryString["TYPE"]) > 0)
                        url += "?TYPE=" + Convert.ToInt32(Request.QueryString["TYPE"]);
                    Response.Redirect(url);
                    break;
                #endregion
                #region Search
                case ActionsEnum.SEARCH:
                    PageIndex = "1";
                    SetFieldValues(ControlsEnum.DEFAULT);
                    grdPRItems.DataSource = null;
                    grdPRItems.DataBind();
                    break;
                #endregion
                #region CLEAR
                case ActionsEnum.CLEAR:
                    ResetForm();
                    PageIndex = "1";
                    SetFieldValues(ControlsEnum.DEFAULT);
                    grdPRItems.DataSource = null;
                    grdPRItems.DataBind();
                    break;
                #endregion
                #region PRITEMDETAILS
                case ActionsEnum.PRITEMDETAILS:
                    hdfSelectedItemPRPK.Value = "0";
                    gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                    PRId = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfPRHPK")).Value);
                    transTYPE = 1;//MTR
                    transPK = PRId;
                    GetFieldValues(ControlsEnum.TRANSACTIONDETAILS);
                    SetFieldValues(ControlsEnum.TRANSACTIONDETAILS);
                    hdfSelectedItemPRPK.Value = PRId.ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ItemSelect", "ItemListSelection();", true);
                    break;
                #endregion
                #region extra grid ondemand data population
                #region PODETAILS
                case ActionsEnum.PODETAILS:
                    arg = ((Button)sender).CommandArgument;
                    gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                    if (gvr != null)
                    {
                        grd = gvr.FindControl("grdPOList") as GridView;
                        if (string.IsNullOrEmpty(arg))
                        {
                            dtPageData = null;
                        }
                        else
                        {
                            PRDtlPK = Convert.ToInt32(arg);
                            transTYPE = 2;//PO
                            transPK = Convert.ToInt32(arg);
                            GetFieldValues(ControlsEnum.TRANSACTIONDETAILS);
                        }
                        grd.Visible = true;
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            grd.DataSource = dtPageData;
                            grd.DataBind();
                        }
                        (gvr.FindControl("hdfIsExpandedPRItem") as HiddenField).Value = "1";
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ItemSelect", "ItemListSelection();", true);
                    }
                    break;
                #endregion
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
                            transTYPE = 3;//GRN
                            transPK = Convert.ToInt32(arg);
                            GetFieldValues(ControlsEnum.TRANSACTIONDETAILS);
                        }
                        grd.Visible = true;
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            grd.DataSource = dtPageData;
                            grd.DataBind();
                        }
                        (gvr.FindControl("hdfIsExpandedPO") as HiddenField).Value = "1";
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
                            transTYPE = 4;//GIN
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
                            transTYPE = 5;//Stock Admission
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
                if ((sender as GridView).ID == "grdPRList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        ImageButton imbEdit = e.Row.FindControl("imbEdit") as ImageButton;
                        ImageButton imbDelete = e.Row.FindControl("imbDelete") as ImageButton;
                        ImageButton imbView = e.Row.FindControl("imbView") as ImageButton;
                        ImageButton imbPrint = e.Row.FindControl("imbPrint") as ImageButton;
                        ImageButton imbPRShorClose = e.Row.FindControl("imbPRShorClose") as ImageButton;
                        ImageButton imbPRCancel = e.Row.FindControl("imbPRCancel") as ImageButton;
                        Button imgbtnPRHierarchyLevel = e.Row.FindControl("imgbtnPRHierarchyLevel") as Button;
                        if (ShowPRtype == "1")
                        {
                            string catCode = GetLocalResourceObject("POHLocalCategoryCode").ToString();
                            string catText = GetLocalResourceObject("POHLocalCategoryCode").ToString();
                            HiddenField hdfCatCode = e.Row.FindControl("hdfPohCategoryCode") as HiddenField;
                            Label lblGrdCmpny = e.Row.FindControl("lblCmpny") as Label;
                            HiddenField hdfPohGrdCategoryText = e.Row.FindControl("hdfPohCategoryText") as HiddenField;
                            if (!string.IsNullOrEmpty(hdfCatCode.Value))
                                catCode = hdfCatCode.Value;
                            if (!string.IsNullOrEmpty(hdfPohGrdCategoryText.Value))
                                catText = hdfPohGrdCategoryText.Value;
                            lblGrdCmpny.Text = catCode;
                            lblGrdCmpny.ToolTip = catText;
                            if (catCode == GetLocalResourceObject("POHLocalCategoryCode").ToString())
                                lblGrdCmpny.CssClass = GetLocalResourceObject("CategoryLClass").ToString();
                            else
                                lblGrdCmpny.CssClass = GetLocalResourceObject("CategoryCClass").ToString();
                        }

                        int isModifyPR = Convert.ToInt32(hdnModifyPR.Value);
                        int isCancelPR = Convert.ToInt32(hdnCancelPR.Value);
                        int UserStatus = Convert.ToInt32((e.Row.FindControl("hdfUserStatus") as HiddenField).Value);
                        int DELSTATUS = Convert.ToInt32((e.Row.FindControl("hdfPrhDelStatus") as HiddenField).Value);
                        int PRH_STATUS = Convert.ToInt32((e.Row.FindControl("hdfPrhStatus") as HiddenField).Value);

                        int prhLinkStatus = Convert.ToInt32((e.Row.FindControl("hdfprhLinkStatus") as HiddenField).Value);//We can Use this flwg instead of following 3 flag ie,isPOExist,isPOWkfExist,grnWkfExist
                        //prhLinkStatus : 0 => MTR, 1=> PO draft, 2=> PO wkf started, 12 => GRN wkf Started.
                        #region Show/Hide grdPRlist image buttons w.r.to previlege
                        if (UserStatus == 1)
                        {//Action To perform for the logged in user
                            imbEdit.Visible = true;
                            imbDelete.Visible = false;
                            imbView.Visible = false;
                        }
                        else if (UserStatus == 0)
                        {//No Action to perform but he is a participent in the work flow
                            imbEdit.Visible = false;
                            imbDelete.Visible = false;
                        }
                        else if (UserStatus == 2)
                        {//Draft will have this status
                            imbEdit.Visible = true;
                            imbDelete.Visible = true;
                            imbView.Visible = false;
                        }
                        if (PRH_STATUS == 104)
                        { //Cancelled
                            imbEdit.Visible = false;
                            imbView.Visible = true;
                            imbDelete.Visible = false;
                            imbPrint.Visible = true;
                        }
                        //if (isModifyPR == 1 && UserStatus == 0 && prhLinkStatus >= 2 && PRH_STATUS != 4 && PRH_STATUS != 5)
                        if (isModifyPR == 1 && UserStatus == 0 && PRH_STATUS != 4 && PRH_STATUS != 5 && PRH_STATUS != 3 && PRH_STATUS != 104)
                        {   //PRH_STATUS=> 4 (ShortClosure),5(Closed)
                            imbPRShorClose.Visible = true;
                        }
                        else
                        {
                            imbPRShorClose.Visible = false;
                        }

                        //Cancel MTR
                        if (isCancelPR == 1 && UserStatus != 2 && prhLinkStatus == 0 && DELSTATUS != 1 && PRH_STATUS != 4 && PRH_STATUS != 3)
                        { //UserStatus: 2=>draft
                            imbPRCancel.Visible = true;
                        }
                        else
                        {
                            imbPRCancel.Visible = false;
                        }
                        #endregion

                        string prNumber = (e.Row.FindControl("lblPRNo") as Label).Text;
                        if (string.IsNullOrEmpty(prNumber))
                            (e.Row.FindControl("lblPRNo") as Label).Text = "[NEW]";

                        #region Colour Changing w. r. to hierachy level

                        if (prhLinkStatus == 2) //PO exists against this MTR
                        {
                            imgbtnPRHierarchyLevel.CssClass = GetLocalResourceObject("POSubmittedCss").ToString();//pink-icon
                            imgbtnPRHierarchyLevel.ToolTip = GetLocalResourceObject("POSubmitted").ToString();
                        }
                        else if (prhLinkStatus == 12) //GRN exists against this MTR
                        {
                            imgbtnPRHierarchyLevel.CssClass = GetLocalResourceObject("GRNSubmittedCss").ToString();//green-icon
                            imgbtnPRHierarchyLevel.ToolTip = GetLocalResourceObject("GRNSubmitted").ToString();
                        }
                        else
                        {
                            if (DELSTATUS != 1 && PRH_STATUS != 4 && PRH_STATUS != 3)//Not cancelled && Not ShortClosed(4) && Not Rejected(3)
                            {
                                //if (PRH_STATUS == 2)//MTR Approved
                                //{
                                //    imgbtnPRHierarchyLevel.CssClass = GetLocalResourceObject("PendingForPOCss").ToString();//orange-icon
                                //    imgbtnPRHierarchyLevel.ToolTip = GetLocalResourceObject("PendingForPO").ToString();
                                //}
                                //else
                                //{
                                //    imgbtnPRHierarchyLevel.CssClass = GetLocalResourceObject("PendingCss").ToString();//cyan-icon
                                //    imgbtnPRHierarchyLevel.ToolTip = GetLocalResourceObject("Pending").ToString();
                                //}
                                imgbtnPRHierarchyLevel.CssClass = GetLocalResourceObject("PendingCss").ToString();//grey-icon
                                imgbtnPRHierarchyLevel.ToolTip = GetLocalResourceObject("PendingForPO").ToString();
                            }
                            else
                            {
                                imgbtnPRHierarchyLevel.Visible = false;
                            }
                        }
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
            int pk = 0, refID = 0, UserStatus = 0;
            pk = Convert.ToInt32((row.FindControl("hdfPRHPK") as HiddenField).Value);
            refID = Convert.ToInt32((row.FindControl("hdfRefId") as HiddenField).Value);
            UserStatus = Convert.ToInt32((row.FindControl("hdfUserStatus") as HiddenField).Value);
            int? result;
            string remarks = "";
            if (senderGridView.ID == "grdPRList")
            {
                int MenuType = 0;
                if (!string.IsNullOrEmpty(hdfMenuType.Value) && hdfMenuType.Value != "0")
                    MenuType = Convert.ToInt32(hdfMenuType.Value);
                string returnUrl = GetLocalResourceObject("MaterialRequestEntryURL").ToString();
                if (MenuType > 0)
                    returnUrl += "?TYPE=" + MenuType;
                switch (e.CommandName)
                {
                    case "PERFORMACTION":
                        #region PERFORMACTION
                        int storePK = Convert.ToInt32((row.FindControl("hdfPrhDept") as HiddenField).Value);
                        if (UserStatus == 1)
                        {
                            if (MenuType > 0)
                                returnUrl += "&RefID=" + refID;
                            else
                                returnUrl += "?RefID=" + refID;
                            Response.Redirect(returnUrl);
                        }
                        else if (UserStatus == 2)
                        {
                            if (MenuType > 0)
                                returnUrl += "&PK=" + pk + "&Dep=" + storePK;
                            else
                                returnUrl += "?PK=" + pk + "&Dep=" + storePK;
                            Response.Redirect(returnUrl);
                        }
                        #endregion
                        break;
                    case "DELETEPR":
                        #region DELETEPR
                        result = DataAccess.PurchaseRequestManagement.PurchaseRequestListingDL.DeleteMaterialRequestDtls(pk, remarks);
                        if (result == 1)
                        {
                            litErrorMsg.Text = Resources.Messages.MaterialRequestDeletedSuccessfully;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                            SetFieldValues(ControlsEnum.DEFAULT);
                        }
                        else
                        {
                            if (result == 0)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Assigned").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("PurchaseRequest").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        #endregion
                        break;
                    case "SHORTCLOSEPR":
                        #region SHORTCLOSEPR
                        if (MenuType > 0)
                            returnUrl += "&RefID=" + refID + "&Status=1&IsModify=1";
                        else
                            returnUrl += "?RefID=" + refID + "&Status=1&IsModify=1";
                        Response.Redirect(returnUrl);
                        #endregion
                        break;
                    case "CANCELPR":
                        #region CANCELPR
                        if (hdfMenuType.Value == "4")
                            result = DataAccess.PurchaseRequestManagement.PurchaseRequestListingDL.DeleteMRDtlsPlantToPlant(pk, remarks);
                        else
                            result = DataAccess.PurchaseRequestManagement.PurchaseRequestListingDL.DeleteMRDtls(pk, remarks);
                        if (result == 1)
                        {
                            litErrorMsg.Text = Resources.Messages.MRCanceledSuccessfully;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                            SetFieldValues(ControlsEnum.DEFAULT);
                        }
                        else
                        {
                            if (result == 0)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Assigned").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == -10)
                            {
                                litErrorMsg.Text = Resources.Messages.UnableToCancelMR;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("PurchaseRequest").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        #endregion
                        break;
                    case "VIEW":
                        #region VIEW
                        if (UserStatus == 1)
                        {
                            if (MenuType > 0)
                                returnUrl += "&RefID=" + refID + "&Status=1";
                            else
                                returnUrl += "?RefID=" + refID + "&Status=1";
                            Response.Redirect(returnUrl);
                        }
                        else if (UserStatus == 2)
                        {
                            if (MenuType > 0)
                                returnUrl += "&PK=" + pk + "&Status=1";
                            else
                                returnUrl += "?PK=" + pk + "&Status=1";
                            Response.Redirect(returnUrl);
                        }
                        else if (UserStatus == 0)
                        {
                            if (refID == 0)
                            {
                                if (MenuType > 0)
                                    returnUrl += "&PK=" + pk + "&Status=1";
                                else
                                    returnUrl += "?PK=" + pk + "&Status=1";
                                Response.Redirect(returnUrl);
                            }
                            else
                            {
                                if (MenuType > 0)
                                    returnUrl += "&RefID=" + refID + "&Status=1";
                                else
                                    returnUrl += "?RefID=" + refID + "&Status=1";
                                Response.Redirect(returnUrl);
                            }
                        }
                        #endregion
                        break;
                    case "PRINT":
                        #region PRINT
                        string url = GetLocalResourceObject("PurchaseRequestReportURL").ToString() + "?ID=" + pk + "&APPTYPE=" + hdfAppType.Value.ToString() + "&APPSUBTYPE=" + hdfAppSubType.Value.ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "OpenPDF", "OpenPDF('" + url + "');", true);
                        #endregion
                        break;
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
            string path = "/Inventory/MaterialRequest.aspx";
            if (Request.QueryString["TYPE"] != null && Convert.ToInt32(Request.QueryString["TYPE"]) == 4)
                path += "?TYPE=" + Request.QueryString["TYPE"];
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

                foreach (GridViewRow grdrow in grdPRList.Rows)
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
                        if (dsPageData.Tables[0].Rows.Count > 0)
                        {
                            rowCount = Convert.ToInt32(dsPageData.Tables[0].Rows[0][0].ToString());
                        }
                        uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                      (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                      (rowCount / pageSize) + 1;
                        PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        grdPRList.DataSource = dsPageData.Tables[1];
                        grdPRList.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                        break;
                    #endregion
                    #region PRDETAILS
                    case ControlsEnum.TRANSACTIONDETAILS:
                        grdPRItems.DataSource = dtPageData;
                        grdPRItems.DataBind();
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
                    case ControlsEnum.PRTYPE:
                        if (dtType != null && dtType.Rows.Count > 0)
                        {
                            ddlPOHType.DataValueField = GTIService.Constants.Common.Fields.CON_VALUE;
                            ddlPOHType.DataTextField = GTIService.Constants.Common.Fields.CON_NAME;
                            ddlPOHType.DataSource = dtType;
                            ddlPOHType.DataBind();
                        }
                        ddlPOHType.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                        break;
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
            hdfItemName.Value = string.Empty;
            txtFromDate.Text = string.Empty;
            hdfFromDate.Value = string.Empty;
            txtToDate.Text = string.Empty;
            txtItemname.Text = string.Empty;
            txtReqStore.Text = string.Empty;
            hdfReqStore.Value = "0";
            hdfItemcode.Value = "0";
            //txtIONo.Text = string.Empty;
            //hdfIONo.Value = "0";
            txtReqDept.Text = string.Empty;
            hdfReqDept.Value = "0";
            txtReqBy.Text = string.Empty;
            hdfReqBy.Value = "0";
            txtPRNumber.Text = string.Empty;
            hdfPRNumber.Value = "0";
           // ddlItemCategory.SelectedValue = "0";
           // ddlStatus.SelectedValue = "-5";
            ddlPlantCode.SelectedValue = "0";
            ddlTrnStatus.SelectedValue = "-1";
        }
        private void ConfigurationSettings()
        {
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
            ShowPRtype = GetGlobalResourceObject("ConfigurationsRes", "ShowPRtype").ToString();
            ShowItemCategory = GetGlobalResourceObject("ConfigurationsRes", "EnableServicePR").ToString();
            //if (ShowItemCategory == "1")//For filter Goods, service and all item filteration
            //{
            //    lblPrNumber.CssClass = "lbl-12-5perc";
            //   // ddlItemCategory.Visible = true;
            //}
            //else
            //{
            //    lblPrNumber.CssClass = "middle-lbl-d";
            //    //ddlItemCategory.Visible = false;
            //}
            lblPrNumber.CssClass = "middle-lbl-d";
            if (ShowPRtype == "1")
            {

                lblPohType.Visible = true;
                ddlPOHType.Visible = true;

                //lblIONo.Visible = false;
                //txtIONo.Visible = false;

                GetFieldValues(ControlsEnum.PRTYPE);
            }
            else
            {

                lblPohType.Visible = false;
                ddlPOHType.Visible = false;

                //lblIONo.Visible = true;
                //txtIONo.Visible = true;
            }
            SetFieldValues(ControlsEnum.PRTYPE);
        }
        private void GetUserRights()
        {
            string path = "/Inventory/MaterialRequest.aspx";
            UserAuthBL userAuth = new UserAuthBL();
            UserRightsBO usrRights = userAuth.GetUserRights(currentUser.PKUser, path, currentUser.SBUID);
            if (usrRights.Rights.Count > 0)
            {
                foreach (var item in usrRights.Rights)
                {
                    if (item.ActionName == "EDIT" && item.HasActionRight == true)
                    {
                        hdnModifyPR.Value = "1";
                    }
                    if (item.ActionName == "CANCEL" && item.HasActionRight == true)//For Cancelling MTR
                    {
                        hdnCancelPR.Value = "1";
                    }
                }

            }
        }

        public void AssignLocalBreadCrumb()
        {
            try
            {
                string _breadCrumb = string.Empty;
                string breadCrumb = string.Empty;
                if (GetGlobalResourceObject("ConfigurationsRes", "HasPlantwiseBreadcrumb").ToString() == "1")
                {
                    string plant = string.Empty;
                    if (currentUser.CurrentDept.Split('-').ElementAtOrDefault(1) != null)
                        plant = currentUser.CurrentDept.Split('-')[1].ToString().Trim();

                    if (Request.QueryString["TYPE"] != null && Request.QueryString["TYPE"].ToString() == "4")
                        _breadCrumb = string.Format(this.GetLocalResourceObject("BreadcrumbP2P_PName").ToString(), plant);
                    else
                        _breadCrumb = string.Format(this.GetLocalResourceObject("Breadcrumb_Internal").ToString(), plant);

                    breadCrumb = _breadCrumb.Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                    lblBreadCrum.Text = breadCrumb;
                }
            }
            catch (Exception ex)
            {

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
            PRTYPE
        }

        #endregion
    }
}