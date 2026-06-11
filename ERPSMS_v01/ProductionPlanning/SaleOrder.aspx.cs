using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.ILibrary;
using BusinessObject.Sales;
using System.Data;
using System.Xml;
using ERPSMS_v01.UserControls;
using ERP.Utilities.Constants.DA;
using DataAccess.ProductionDL;
using BusinessObject;


namespace ERPSMS_v01.Sales
{
    public partial class SaleOrder : ERP.Store.UI.MyBasePage
    {
        #region Coding
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// For Sort Direction
        /// </summary>
        public string sortOrder
        {
            get
            {
                if (ViewState[ViewstateStrings.dirState] == null)
                {
                    ViewState[ViewstateStrings.dirState] = sortDirection.Descending;
                }
                return (string)ViewState[ViewstateStrings.dirState];
            }
            set
            {
                ViewState[ViewstateStrings.dirState] = value;
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
        private string SodPK
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SodPK];
            }
            set
            {
                this.ViewState[ViewstateStrings.SodPK] = value;
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
        /// To maintain the total pages in viewstate
        /// </summary>
        private int PlannedTotalPages
        {
            get
            {
                return (int)this.ViewState["PlannedTotalPages"];
            }
            set
            {
                this.ViewState["PlannedTotalPages"] = value;
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

        //For Redirect URL
        string RedirectURL
        {
            get
            {
                return (string)this.ViewState["RedirectURL"];
            }
            set
            {
                this.ViewState["RedirectURL"] = value;
            }
        }

        // For Common Actions
        ActionsEnum commonAction;
        //For Selected Orders
        OrderItemsBO objSelectedOrderItems;
        #endregion



        //For fetch saleorder
        private DataSet dsSaleOrder;

        //For fetch Session Details
        private DataSet dsSessionDtl;

        //For Planned Details
        private DataSet dsPlannedDetails;

        //For Advanced Filter
        private DataSet dsAdvancedFilter;

        //For Filter XML
        private string[] P_XML;

        // For User Identity
        //private IdentityUser CurrentUser;

        //For Sale Order PK;
        private string[][] sodPK;

        //For get db return messages
        private DataSet dsDBMessgaes;

        //For Save XML
        private string[] S_XML;

        User CurrentUser;

        //For Saleorder Information
        List<SaleOrderInfo> lstSaleOrderInfo;
        #endregion

        #region Page Level Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PageActionHandler();

            }
        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_InitComponents", "$(document).ready(function(){InitComponents();});", true);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");

        }


        #endregion

        #region Get Field Values

        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// Assign it to the page level variables
        /// </summary>
        public void GetFieldValues(ControlEnum type)
        {
            switch (type)
            {
                case ControlEnum.ALLOCATION:
                    objSelectedOrderItems = GetSelectedOrders();
                    break;
                case ControlEnum.PLANNING:
                    SetFieldValuesToXML(ControlEnum.PLANNING);
                    dsDBMessgaes = SetValuesToDB(2, S_XML);
                    break;
                case ControlEnum.CLEAR:
                    SetFilterXML(0);
                    dsSaleOrder = FetchDbValues(1, P_XML);
                    break;
                case ControlEnum.NEXT:
                    if (Session[SessionStrings.FilterCriteria] != null)
                    {
                        P_XML = new string[10];
                        SalesOrderBO objSaleorder = new SalesOrderBO();
                        objSaleorder = (SalesOrderBO)Session[SessionStrings.FilterCriteria];
                        objSaleorder.PageNo = PageIndex == null ? "1" : PageIndex;
                        XmlDocument xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objSaleorder);
                        P_XML[0] = xmlDoc.InnerXml;

                    }
                    else
                    {
                        SetFilterXML(1);
                    }

                    dsSaleOrder = FetchDbValues(1, P_XML);
                    break;

                case ControlEnum.GRID:
                    SetFilterXML(1);
                    dsSaleOrder = FetchDbValues(1, P_XML);
                    break;
                case ControlEnum.PLANNED:
                    SetFilterXML(2);
                    dsPlannedDetails = FetchDbValues(2, P_XML);
                    break;
                case ControlEnum.FILTER:
                    ResetForm();
                    SetFilterXML(3);
                    dsAdvancedFilter = FetchDbValues(3, P_XML);
                    break;
                case ControlEnum.DESPATCH:
                    SetFieldValuesToXML(ControlEnum.DESPATCH);
                    dsDBMessgaes = SaleOrderDL.SaveOrderList(2, S_XML);
                    break;
                //case ControlEnum.QUICKPLANNING:
                //     SetFieldValuesToXML(ControlEnum.PLANNING);
                //     dsDBMessgaes=SetValuesToDB(2, S_XML);
                //    break;
            }

        }
        #endregion

        #region Set Field Values

        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// </summary>
        public void SetFieldValues(ControlEnum type)
        {
            switch (type)
            {
                case ControlEnum.CLEAR:
                    BindGrid(ControlEnum.GRID);
                    BindDropDown(ControlsEnum.ORDER);
                    BindDropDown(ControlsEnum.PRODUCT);
                    BindDropDown(ControlsEnum.SIZE);
                    BindDropDown(ControlsEnum.PRODUCTGROUP);
                    break;
                case ControlEnum.GRID:
                    BindGrid(ControlEnum.GRID);
                    BindDropDown(ControlsEnum.ORDER);
                    BindDropDown(ControlsEnum.PRODUCT);
                    BindDropDown(ControlsEnum.SIZE);
                    BindDropDown(ControlsEnum.PRODUCTGROUP);
                    BindDropDown(ControlsEnum.PLANSTAGE);

                    setFilterOption();

                    break;
                case ControlEnum.PLANNED:
                    BindGrid(ControlEnum.PLANNED);
                    break;
                case ControlEnum.DESPATCH:
                    PrintDbMessages(dsDBMessgaes, Resources.PageURL.Dispatch + "?dval=1");
                    break;
                //case ControlEnum.PLANNING:
                //    SetFilterXML(7);
                //    dsSessionDtl = FetchDbValues(5, P_XML);
                //    if (dsSessionDtl != null && dsSessionDtl.Tables[0].Rows.Count > 0)
                //        Session[SessionStrings.SessionPK] = dsSessionDtl.Tables[0].Rows[0][Resources.DataFieldRes.SessionPK].ToString(); ;
                //    PrintDbMessages(dsDBMessgaes, Resources.PageURL.QuickPlanning);
                //    break;
                case ControlEnum.FILTER:
                    txtRequiredBy.Text = string.Empty;
                    BindDropDown(ControlsEnum.ORDER);
                    BindDropDown(ControlsEnum.PRODUCT);
                    BindDropDown(ControlsEnum.SIZE);
                    BindDropDown(ControlsEnum.PRODUCTGROUP);
                    BindDropDown(ControlsEnum.PLANSTAGE);

                    break;
                case ControlEnum.ALLOCATION:
                    Session[SessionStrings.SelectedOrderItem] = objSelectedOrderItems;
                    if (objSelectedOrderItems.ItemsList.Count > 0)
                        Response.Redirect(Resources.PageURL.Allocation);
                    else
                    {
                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Select_Order;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Allocation);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" +  CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "','" + Resources.PageURL.SaleOrder + "');", true);
                    }

                    break;

                //case ControlEnum.QUICKPLANNING:
                //    SetFilterXML(7);
                //    dsSessionDtl = FetchDbValues(5, P_XML);
                //    if (dsSessionDtl!=null && dsSessionDtl.Tables[0].Rows.Count > 0)
                //        Session[SessionStrings.SessionPK] = dsSessionDtl.Tables[0].Rows[0][Resources.DataFieldRes.SessionPK].ToString();;
                //    PrintDbMessages(dsDBMessgaes, Resources.PageURL.QuickPlanning);
                //    break;


            }

        }
        #endregion

        #region Action handler
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ActionHandler(object sender, EventArgs e)
        {
            GridViewRow gvrSaleOrder;
            if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
            {
                commonAction = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
            }
            else
                if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    commonAction = ActionsEnum.CHANGE;
                }
                else
                    if (sender.GetType().IsEquivalentTo(typeof(Button)))
                    {
                        commonAction = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                    }


            switch (commonAction)
            {
                case ActionsEnum.CLEAR:
                    GetFieldValues(ControlEnum.CLEAR);
                    SetFieldValues(ControlEnum.CLEAR);
                    //ClearFilter();
                    Session.Remove(SessionStrings.FilterCriteria);
                    txtRequiredBy.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "DateInitPopup", "GrandScriptUtils.DatePicker('txtRequiredBy', false, false)", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAssetType", "ShowContainerDiv('#DivAdvancedFilterPopup','" + GetLocalResourceObject("AdvancedFilter").ToString() + "','640','420');", true);
                    break;
                case ActionsEnum.PLANNED:
                    gvrSaleOrder = ((LinkButton)sender).Parent.Parent as GridViewRow;
                    SodPK = grdSaleOrder.DataKeys[gvrSaleOrder.RowIndex].Value.ToString();
                    GetFieldValues(ControlEnum.PLANNED);
                    SetFieldValues(ControlEnum.PLANNED);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAssetType", "ShowContainerDiv('#DivPlannedPopup','" + Resources.PageNameRes.PlannedDetails + "','450','260');", true);
                    break;
                case ActionsEnum.ADVANCED_FILTER:

                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "DateInitPopup", "GrandScriptUtils.DatePicker('txtRequiredBy', false, false)", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAssetType", "ShowContainerDiv('#DivAdvancedFilterPopup','" + GetLocalResourceObject("AdvancedFilter").ToString() + "','640','420');", true);
                    break;
                case ActionsEnum.FILTER:
                    PageIndex = "1";
                    GetFieldValues(ControlEnum.GRID);
                    SetFieldValues(ControlEnum.GRID);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                    break;
                case ActionsEnum.ALLOCATION:
                    GetFieldValues(ControlEnum.ALLOCATION);
                    SetFieldValues(ControlEnum.ALLOCATION);
                    break;

                case ActionsEnum.PLANNING:
                    GetFieldValues(ControlEnum.PLANNING);
                    SetFieldValues(ControlEnum.PLANNING);

                    break;
                //case ActionsEnum.QUICKPLANNING:
                //    GetFieldValues(ControlEnum.QUICKPLANNING);
                //    SetFieldValues(ControlEnum.QUICKPLANNING);

                //    break;
                case ActionsEnum.CONTINUE:
                    Response.Redirect(RedirectURL);
                    break;
                case ActionsEnum.DISPATCH:
                    if (CountSelectedItems() > 0)
                    {
                        GetFieldValues(ControlEnum.DESPATCH);
                        SetFieldValues(ControlEnum.DESPATCH);
                    }

                    else

                        Response.Redirect(Resources.PageURL.Dispatch);
                    break;

                default:
                    GetFieldValues(ControlEnum.GRID);
                    SetFieldValues(ControlEnum.GRID);

                    break;
                //case ActionsEnum.PRODUCTION:
                //    Response.Redirect(Resources.PageURL.ShiftReport);
                //    break;
                //case ActionsEnum.ORDERREALIZATION:
                //    Response.Redirect(Resources.PageURL.OrderTracking);
                //    break;



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
                    datakeyarray[0] = Resources.DataFieldRes.OrderDtlPK;
                    grdSaleOrder.DataKeyNames = datakeyarray;
                    GetFieldValues(ControlEnum.GRID);
                    SetFieldValues(ControlEnum.GRID);

                    PageIndex = ERP.Utilities.CommonConstants.SELECT_VALUE_ONE;
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                    //GetFieldValues(ControlEnum.FILTER);
                    //SetFieldValues(ControlEnum.FILTER);

                    // SetFilterStatus();

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region Pager Methods + Init
        /// <summary>
        /// Method  For PageInit Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;

        }

        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {
            // base.CheckBtnVisibility(sender);

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
                        // Assign the current page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        // Assign the current page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        // Increment the current page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        // Decrement the current page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage--;
                        break;

                }

                SetAllocationDetails();
                PageIndex = uclPaging.CurrentPage.ToString();
                //  GetFieldValues(ControlEnum.NEXT);
                GetFieldValues(ControlEnum.GRID);
                SetFieldValues(ControlEnum.GRID);
                SetGridStatus();



                EnableDisableButtons(e.TotalPages);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }

        }



        /// <summary>
        /// Method used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            // Should we disable the first link?
            uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we disable the previous link?
            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we enable the next link?
            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            // Should we enable the last link?
            uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
        }

        /// <summary>
        /// To handle OnInit event Used to assign the Event for all the actions used in this page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();
        }

        #endregion

        #region Healper methods

        /// <summary>
        /// Method for Print db messages
        /// </summary>
        private void PrintDbMessages(DataSet dsMsg, string url)
        {
            DbStatus CurrentStatus;
            bool StatusFlag = true;
            string strHdrMsg = string.Empty; ;
            if (dsMsg != null)
            {
                try
                {
                    CurrentStatus = (DbStatus)(Enum.Parse(typeof(DbStatus), dsMsg.Tables[0].Rows[0][Resources.DataFieldRes.DbReturnType].ToString()));
                    switch (CurrentStatus)
                    {
                        case DbStatus.Success:
                            strHdrMsg = Resources.Messages.InformationSaved;
                            DiverrorMessages.Attributes.Remove("class");
                            DiverrorMessages.Attributes.Add("class", "message-sucess");
                            break;
                        case DbStatus.Failure:
                            strHdrMsg = Resources.Messages.ErrorMessage;
                            DiverrorMessages.Attributes.Remove("class");
                            DiverrorMessages.Attributes.Add("class", "message-error");
                            break;
                        case DbStatus.Information:
                            strHdrMsg = Resources.Messages.Information;
                            DiverrorMessages.Attributes.Remove("class");
                            DiverrorMessages.Attributes.Add("class", "message-info");
                            break;
                    }
                }
                catch (Exception Ex)
                {
                    StatusFlag = false;
                }

                if (dsMsg.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(dsMsg.Tables[0].Rows[0][Resources.DataFieldRes.dbRetVal].ToString()) > 0)
                    {
                        Response.Redirect(url);

                    }
                    else
                    {
                        RedirectURL = url;
                        if (!StatusFlag)
                        {
                            strHdrMsg = Resources.Messages.ErrorMessage;
                            DiverrorMessages.Attributes.Remove("class");
                            DiverrorMessages.Attributes.Add("class", "message-error");
                        }

                        if (Convert.ToInt32(dsMsg.Tables[0].Rows[0][Resources.DataFieldRes.dbRetVal].ToString()) == -8)
                            btnContinue.Visible = true;
                        else
                            btnContinue.Visible = false;
                        BindGrid(ControlEnum.DBMSG);


                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAssetType", "ShowContainerDiv('#" + DiverrorMessages.ClientID + "','" + strHdrMsg + "','500','300');", true);

                    }


                }
            }

        }



        #region Grd Status maintains
        //For sett allocation details
        private void SetAllocationDetails()
        {
            if (Session[SessionStrings.SelectedOrderInfo] != null)
                lstSaleOrderInfo = (List<SaleOrderInfo>)Session[SessionStrings.SelectedOrderInfo];
            else
                lstSaleOrderInfo = new List<SaleOrderInfo>();

            CheckBox chbSelect;

            foreach (GridViewRow item in grdSaleOrder.Rows)
            {
                chbSelect = (CheckBox)item.FindControl("chbSelect");
                SaleOrderInfo objSaleOrderInfo = new SaleOrderInfo();
                objSaleOrderInfo.chkChecked = false;
                objSaleOrderInfo.sodPK = Convert.ToInt32(grdSaleOrder.DataKeys[item.RowIndex].Value.ToString());

                if (chbSelect.Checked)
                    objSaleOrderInfo.chkChecked = true;

                bool alreadyExists = lstSaleOrderInfo.Exists(itemLst => itemLst.sodPK == objSaleOrderInfo.sodPK);
                if (alreadyExists)
                    ChangeItem(objSaleOrderInfo);
                else
                    lstSaleOrderInfo.Add(objSaleOrderInfo);

            }

            Session[SessionStrings.SelectedOrderInfo] = lstSaleOrderInfo;


        }

        //for change the status of checked items
        private void ChangeItem(SaleOrderInfo item)
        {
            if (lstSaleOrderInfo.Count > 0)
                foreach (var Items in lstSaleOrderInfo)
                    if (Items.sodPK == item.sodPK)
                    {
                        Items.chkChecked = item.chkChecked;
                    }
        }

        //For Reset grid status
        private void SetGridStatus()
        {
            if (Session[SessionStrings.SelectedOrderInfo] != null)
            {
                lstSaleOrderInfo = (List<SaleOrderInfo>)Session[SessionStrings.SelectedOrderInfo];
                int sodPK;
                foreach (GridViewRow item in grdSaleOrder.Rows)
                {
                    sodPK = Convert.ToInt32(grdSaleOrder.DataKeys[item.RowIndex].Value.ToString());
                    if (lstSaleOrderInfo.Exists(itemLst => itemLst.sodPK == sodPK && itemLst.chkChecked == true))
                        ((CheckBox)item.FindControl("chbSelect")).Checked = true;

                }
            }

        }

        #endregion

        private void ResetForm()
        {
            txtRequiredBy.Text = string.Empty;
            PageIndex = ERP.Utilities.CommonConstants.SELECT_VALUE_ONE;


        }

        private void ClearFilter()
        {
            foreach (var item in chkProdutGroup.Items.Cast<ListItem>().Where(i => i.Selected == true))
                item.Selected = false;
            foreach (var item in chklstOrders.Items.Cast<ListItem>().Where(i => i.Selected == true))
                item.Selected = false;
            ddlProduct.SelectedIndex = Convert.ToInt32(ddlProduct.Items.IndexOf(ddlProduct.Items.FindByValue(ERP.Utilities.CommonConstants.SELECT_ALL_VAL)));
            ddlSize.SelectedIndex = Convert.ToInt32(ddlSize.Items.IndexOf(ddlSize.Items.FindByValue(ERP.Utilities.CommonConstants.SELECT_ALL_VAL)));
            txtRequiredBy.Text = string.Empty;


        }
        /// <summary>
        /// Method for Bind DropDown
        /// </summary>
        public void BindDropDownOld(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {

                    case ControlsEnum.PRODUCTGROUP:
                        chkProdutGroup.Items.Clear();

                        if (dsAdvancedFilter != null && dsAdvancedFilter.Tables.Count > 0)
                        {
                            chkProdutGroup.DataSource = dsAdvancedFilter.Tables[3];
                            chkProdutGroup.DataTextField = Resources.DataFieldRes.ProductGroupCode;
                            chkProdutGroup.DataValueField = Resources.DataFieldRes.ProductGroupPK;
                            chkProdutGroup.DataBind();
                        }
                        //ddlOrder.Items.Insert(0, new ListItem(Resources.gErpProductionRes.All, CommonConstants.SELECT_ALL_VAL));
                        //ddlOrder.SelectedIndex = Convert.ToInt32(ddlOrder.Items.IndexOf(ddlOrder.Items.FindByValue(CommonConstants.SELECT_ALL_VAL)));
                        break;

                    case ControlsEnum.ORDER:
                        chklstOrders.Items.Clear();

                        if (dsAdvancedFilter != null && dsAdvancedFilter.Tables.Count > 0)
                        {
                            chklstOrders.DataSource = dsAdvancedFilter.Tables[1];
                            chklstOrders.DataTextField = Resources.DataFieldRes.SaleOrder;
                            chklstOrders.DataValueField = Resources.DataFieldRes.SaleOrderPK;
                            chklstOrders.DataBind();
                        }
                        //ddlOrder.Items.Insert(0, new ListItem(Resources.gErpProductionRes.All, CommonConstants.SELECT_ALL_VAL));
                        //ddlOrder.SelectedIndex = Convert.ToInt32(ddlOrder.Items.IndexOf(ddlOrder.Items.FindByValue(CommonConstants.SELECT_ALL_VAL)));
                        break;
                    case ControlsEnum.PRODUCT:

                        ddlProduct.Items.Clear();
                        if (dsAdvancedFilter != null && dsAdvancedFilter.Tables.Count > 0)
                        {
                            ddlProduct.DataSource = dsAdvancedFilter.Tables[0];
                            ddlProduct.DataTextField = Resources.DataFieldRes.proCode;
                            ddlProduct.DataValueField = Resources.DataFieldRes.productPK;
                            ddlProduct.DataBind();
                        }
                        ddlProduct.Items.Insert(0, new ListItem(Resources.ErpRes.All, ERP.Utilities.CommonConstants.SELECT_ALL_VAL));
                        ddlProduct.SelectedIndex = Convert.ToInt32(ddlProduct.Items.IndexOf(ddlProduct.Items.FindByValue(ERP.Utilities.CommonConstants.SELECT_ALL_VAL)));
                        break;
                    case ControlsEnum.SIZE:

                        ddlSize.Items.Clear();
                        if (dsAdvancedFilter != null && dsAdvancedFilter.Tables.Count > 0)
                        {
                            ddlSize.DataSource = dsAdvancedFilter.Tables[2];
                            ddlSize.DataTextField = Resources.DataFieldRes.Size;
                            ddlSize.DataValueField = Resources.DataFieldRes.SizePk;
                            ddlSize.DataBind();
                        }
                        ddlSize.Items.Insert(0, new ListItem(Resources.ErpRes.All, ERP.Utilities.CommonConstants.SELECT_ALL_VAL));
                        ddlSize.SelectedIndex = Convert.ToInt32(ddlSize.Items.IndexOf(ddlSize.Items.FindByValue(ERP.Utilities.CommonConstants.SELECT_ALL_VAL)));
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void BindDropDown(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {

                    case ControlsEnum.PRODUCTGROUP:
                        chkProdutGroup.Items.Clear();

                        if (dsSaleOrder != null && dsSaleOrder.Tables.Count > 0)
                        {
                            chkProdutGroup.DataSource = dsSaleOrder.Tables[4];
                            chkProdutGroup.DataTextField = Resources.DataFieldRes.ProductGroupCode;
                            chkProdutGroup.DataValueField = Resources.DataFieldRes.ProductGroupPK;
                            chkProdutGroup.DataBind();
                        }
                        //ddlOrder.Items.Insert(0, new ListItem(Resources.gErpProductionRes.All, CommonConstants.SELECT_ALL_VAL));
                        //ddlOrder.SelectedIndex = Convert.ToInt32(ddlOrder.Items.IndexOf(ddlOrder.Items.FindByValue(CommonConstants.SELECT_ALL_VAL)));
                        break;

                    case ControlsEnum.ORDER:
                        chklstOrders.Items.Clear();

                        if (dsSaleOrder != null && dsSaleOrder.Tables.Count > 0)
                        {
                            chklstOrders.DataSource = dsSaleOrder.Tables[2];
                            chklstOrders.DataTextField = Resources.DataFieldRes.SaleOrder;
                            chklstOrders.DataValueField = Resources.DataFieldRes.SaleOrderPK;
                            chklstOrders.DataBind();
                        }
                        //ddlOrder.Items.Insert(0, new ListItem(Resources.gErpProductionRes.All, CommonConstants.SELECT_ALL_VAL));
                        //ddlOrder.SelectedIndex = Convert.ToInt32(ddlOrder.Items.IndexOf(ddlOrder.Items.FindByValue(CommonConstants.SELECT_ALL_VAL)));
                        break;
                    case ControlsEnum.PRODUCT:

                        ddlProduct.Items.Clear();
                        if (dsSaleOrder != null && dsSaleOrder.Tables.Count > 0)
                        {
                            ddlProduct.DataSource = dsSaleOrder.Tables[1];
                            ddlProduct.DataTextField = Resources.DataFieldRes.ProtCode;
                            ddlProduct.DataValueField = Resources.DataFieldRes.ProtPk;
                            ddlProduct.DataBind();
                        }
                        ddlProduct.Items.Insert(0, new ListItem(Resources.ErpRes.All, ERP.Utilities.CommonConstants.SELECT_ALL_VAL));
                        ddlProduct.SelectedIndex = Convert.ToInt32(ddlProduct.Items.IndexOf(ddlProduct.Items.FindByValue(ERP.Utilities.CommonConstants.SELECT_ALL_VAL)));
                        break;
                    case ControlsEnum.SIZE:

                        ddlSize.Items.Clear();
                        if (dsSaleOrder != null && dsSaleOrder.Tables.Count > 0)
                        {
                            ddlSize.DataSource = dsSaleOrder.Tables[3];
                            ddlSize.DataTextField = Resources.DataFieldRes.Size;
                            ddlSize.DataValueField = Resources.DataFieldRes.SizePk;
                            ddlSize.DataBind();
                        }
                        ddlSize.Items.Insert(0, new ListItem(Resources.ErpRes.All, ERP.Utilities.CommonConstants.SELECT_ALL_VAL));
                        ddlSize.SelectedIndex = Convert.ToInt32(ddlSize.Items.IndexOf(ddlSize.Items.FindByValue(ERP.Utilities.CommonConstants.SELECT_ALL_VAL)));
                        break;
                    case ControlsEnum.PLANSTAGE:

                        ddlPlanStage.Items.Clear();
                        if (dsSaleOrder != null && dsSaleOrder.Tables.Count > 0)
                        {
                            ddlPlanStage.DataSource = dsSaleOrder.Tables[5];
                            ddlPlanStage.DataTextField = Resources.DataFieldRes.cfgData;
                            ddlPlanStage.DataValueField = Resources.DataFieldRes.cfgValue;
                            ddlPlanStage.DataBind();
                        }
                        ddlPlanStage.Items.Insert(0, new ListItem(Resources.ErpRes.All, ERP.Utilities.CommonConstants.SELECT_ALL_VAL));
                        ddlPlanStage.SelectedIndex = Convert.ToInt32(ddlSize.Items.IndexOf(ddlPlanStage.Items.FindByValue(ERP.Utilities.CommonConstants.SELECT_ALL_VAL)));
                        break;
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
        public void BindGrid(ControlEnum type)
        {
            switch (type)
            {
                case ControlEnum.DBMSG:
                    grdError.DataSource = dsDBMessgaes;
                    grdError.DataBind();
                    break;
                case ControlEnum.GRID:
                    try
                    {

                        if (dsSaleOrder != null)
                        {
                            if (dsSaleOrder.Tables[0].Rows.Count > 0)
                                TotalPages = Convert.ToInt32(dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.PageCount].ToString());
                            else
                                TotalPages = Convert.ToInt32(ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO);
                            uclPaging.TotalPages = TotalPages;
                            PageIndex = PageIndex == null ? ERP.Utilities.CommonConstants.SELECT_VALUE_ONE : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);

                            grdSaleOrder.DataSource = dsSaleOrder.Tables[0];
                            grdSaleOrder.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }


                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    break;
                case ControlEnum.PLANNED:
                    try
                    {

                        if (dsPlannedDetails != null)
                        {
                            if (dsPlannedDetails.Tables[0].Rows.Count > 0)
                                PlannedTotalPages = Convert.ToInt32(dsPlannedDetails.Tables[0].Rows[0][Resources.DataFieldRes.PageCount].ToString());
                            //uclPaging.TotalPages = TotalPages;
                            //PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                            //uclPaging.CurrentPage = Convert.ToInt32(PageIndex);

                            grdPlanned.DataSource = dsPlannedDetails;
                            grdPlanned.DataBind();
                            //uclPaging.Visible = true;
                            //uclPaging.BindPager();
                        }


                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                    }
                    break;


            }
        }

        /// <summary>
        /// For Grid Sorting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void grdSaleOrder_Sorting(object sender, GridViewSortEventArgs e)
        {

            if (sortOrder == sortDirection.Descending)
            {
                sortOrder = sortDirection.Ascending;
            }
            else
            {
                sortOrder = sortDirection.Descending;

            }
            SetAllocationDetails();
            GetFieldValues(ControlEnum.GRID);
            DataView sortedView = new DataView(dsSaleOrder.Tables[0]);
            sortedView.Sort = e.SortExpression + " " + sortOrder;
            grdSaleOrder.DataSource = sortedView;
            grdSaleOrder.DataBind();
            SetGridStatus();
        }

        /// <summary>
        /// Method for footer value binding
        /// </summary>
        protected void grdSaleOrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer && dsSaleOrder != null)
            {
                ((Label)e.Row.FindControl("lblTotalOrderQty")).Text = dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.OrderQtyTotal].Equals(DBNull.Value) ? "" :
                    Convert.ToDecimal(dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.OrderQtyTotal]).ToString("n0");
                ((Label)e.Row.FindControl("lblTotalDispatched")).Text = dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.DispatchedTotal].Equals(DBNull.Value) ? "" :
                     Convert.ToDecimal(dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.DispatchedTotal]).ToString("n0");
                ((Label)e.Row.FindControl("lblTotalAllocated")).Text = dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.AllocatedTotal].Equals(DBNull.Value) ? "" :
                     Convert.ToDecimal(dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.AllocatedTotal]).ToString("n0");
                ((Label)e.Row.FindControl("lblTotalProduced")).Text = dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.ProducedTotal].Equals(DBNull.Value) ? "" :
                     Convert.ToDecimal(dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.ProducedTotal]).ToString("n0");
                ((Label)e.Row.FindControl("lblTotalPlanned")).Text = dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.PlannedTotal].Equals(DBNull.Value) ? "" :
                     Convert.ToDecimal(dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.PlannedTotal]).ToString("n0");
                ((Label)e.Row.FindControl("lblTotalBalanceToPlan")).Text = dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.BalanceToPlanTotal].Equals(DBNull.Value) ? "" :
                     Convert.ToDecimal(dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.BalanceToPlanTotal]).ToString("n0");

            }
        }



        /// <summary>
        /// Method for set filter XML
        /// </summary>
        private void SetFilterXML(int val)
        {
            CurrentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            XmlDocument xmlDoc;
            P_XML = new string[10];
            SalesOrderBO objSaleorder;
            int iActivityType;
            switch (val)
            {
                case 0:
                    objSaleorder = new SalesOrderBO();
                    objSaleorder.BizUnit = CurrentUser.SBUID;
                    iActivityType = Convert.ToInt32(ActivityType.Allocation);
                    objSaleorder.ActivityType = iActivityType.ToString();
                    objSaleorder.PageNo = PageIndex == null ? "1" : PageIndex;
                    objSaleorder.IsFrom = Convert.ToInt32(PageName.SaleOrder).ToString();
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objSaleorder);
                    P_XML[0] = xmlDoc.InnerXml;
                    break;

                case 1:
                    //  lblBreadCrum.Text = GetLocalResourceObject("MainGridHed").ToString();

                    string strFilter = "[Filter: ";
                    bool filterFlag = false;
                    objSaleorder = new SalesOrderBO();
                    objSaleorder.BizUnit = CurrentUser.SBUID;
                    objSaleorder.IsFrom = Convert.ToInt32(PageName.SaleOrder).ToString();


                    iActivityType = Convert.ToInt32(ActivityType.Allocation);
                    objSaleorder.ActivityType = iActivityType.ToString();
                    objSaleorder.PageNo = PageIndex == null ? "1" : PageIndex;


                    if (ddlProduct.Items.Count > 0)
                        if (Convert.ToInt32(ddlProduct.SelectedValue) > 0)
                        {
                            objSaleorder.ProductPK = ddlProduct.SelectedValue;
                            filterFlag = true;
                            strFilter += "Product= " + ddlProduct.SelectedItem.Text + ",";

                        }
                    if (ddlSize.Items.Count > 0)
                        if (Convert.ToInt32(ddlSize.SelectedValue) > 0)
                        {
                            objSaleorder.SizePK = ddlSize.SelectedValue;
                            filterFlag = true;
                            strFilter += "Size= " + ddlSize.SelectedItem.Text + ",";
                        }

                    if (txtRequiredBy.Text != string.Empty)
                    {
                        objSaleorder.RequdBy = txtRequiredBy.Text;
                        filterFlag = true;
                        strFilter += "Date<= " + txtRequiredBy.Text + ",";
                    }
                    objSaleorder.PlanStage = ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO;
                    objSaleorder.Orders = new List<OrdersBO>();
                    objSaleorder.Orders.Add(GetCheckedOrders());

                    objSaleorder.ProductGroups = new List<OroductGroupsBO>();
                    objSaleorder.ProductGroups.Add(GetCheckedGroups());
                    objSaleorder.PlanStage = ddlPlanStage.SelectedValue.ToString();

                    // Session[SessionStrings.FilterInfo] = objSaleorder;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objSaleorder);
                    P_XML[0] = xmlDoc.InnerXml;
                    Session[SessionStrings.FilterCriteria] = objSaleorder;
                    if (filterFlag)
                    {
                        strFilter = strFilter.Substring(0, strFilter.Length - 2);
                        //  lblBreadCrum.Text = lblBreadCrum.Text + strFilter+"]";
                    }
                    break;
                case 2:
                    PlannedDetailsBO objPlannedDetails = new PlannedDetailsBO();
                    objPlannedDetails.BizUnit = CurrentUser.SBUID;
                    objPlannedDetails.sodPK = SodPK;
                    objPlannedDetails.PageNo = Convert.ToInt32(ERP.Utilities.CommonConstants.SELECT_VALUE_ONE);
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objPlannedDetails);
                    P_XML[1] = xmlDoc.InnerXml;
                    break;
                case 3:

                    //Product
                    ProductMasterBO objProductMaster = new ProductMasterBO();
                    objProductMaster.Active = Convert.ToInt32(ERP.Utilities.CommonConstants.ACTIVE);
                    objProductMaster.BizUnit = CurrentUser.SBUID;
                    objProductMaster.category = Convert.ToInt32(Category.Product);
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objProductMaster);
                    P_XML[5] = xmlDoc.InnerXml;
                    //Size
                    SizeMasterBO objSizeMaster = new SizeMasterBO();
                    objSizeMaster.BizUnit = CurrentUser.SBUID;
                    objSizeMaster.Active = Convert.ToInt32(ERP.Utilities.CommonConstants.ACTIVE);
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objSizeMaster);
                    P_XML[3] = xmlDoc.InnerXml;

                    //Order Header
                    SaleOrderHeaderBO objSaleOrderHeader = new SaleOrderHeaderBO();
                    objSaleOrderHeader.Active = Convert.ToInt32(ERP.Utilities.CommonConstants.ACTIVE);
                    objSaleOrderHeader.BizUnit = CurrentUser.SBUID;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objSaleOrderHeader);
                    P_XML[6] = xmlDoc.InnerXml;

                    //Product Group
                    ItemMasterBO objProductGroupMaster = new ItemMasterBO();
                    objProductGroupMaster.Active = Convert.ToInt32(ERP.Utilities.CommonConstants.ACTIVE);
                    objProductGroupMaster.BizUnit = CurrentUser.SBUID;
                   // objProductGroupMaster.ProductGroupType = Convert.ToInt32(ProductGroup.ProductGroup);
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objProductGroupMaster);
                    P_XML[8] = xmlDoc.InnerXml;
                    //Plan Stage
                    PlanStageBO objPlanStage = new PlanStageBO();
                    objPlanStage.Active = Convert.ToInt32(ERP.Utilities.CommonConstants.ACTIVE);
                    objPlanStage.BizUnit = CurrentUser.SBUID;
                    objPlanStage.cfgType = ERP.Utilities.CommonConstants.PlanStage;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objPlanStage);
                    P_XML[9] = xmlDoc.InnerXml;


                    break;
                case 7:
                    SessionDtlBO objSessionDtl = new SessionDtlBO();
                    objSessionDtl.BizUnit = CurrentUser.SBUID;
                    objSessionDtl.DeptPK = CurrentUser.CurrentDeptPK;
                    objSessionDtl.Active = Convert.ToInt32(ERP.Utilities.CommonConstants.ACTIVE);
                    objSessionDtl.Status = Convert.ToInt32(SessionStatus.Open);
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objSessionDtl);
                    P_XML[7] = xmlDoc.InnerXml;


                    break;


            }


        }


        /// <summary>
        /// Method for set XML for save
        /// </summary>
        private void SetFieldValuesToXML(ControlEnum type)
        {
            S_XML = new string[2];
            SalesOrderBO objSaleorder;
            XmlDocument xmlDoc;
            OrderItemsBO objSelectedOrders;
            switch (type)
            {
                case ControlEnum.DESPATCH:

                    objSaleorder = new SalesOrderBO();
                    objSaleorder.BizUnit = CurrentUser.SBUID;
                    objSaleorder.UserPK = CurrentUser.PKUser.ToString();
                    objSaleorder.DeptPK = CurrentUser.CurrentDeptPK.ToString();
                    objSaleorder.OrderItemsList = new List<OrderItemsBO>();
                    objSelectedOrders = GetSelectedOrders();
                    objSaleorder.OrderItemsList.Add(objSelectedOrders);
                    Session[SessionStrings.SelectedOrders] = objSelectedOrders;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objSaleorder);
                    S_XML[0] = xmlDoc.InnerXml;

                    break;
                case ControlEnum.PLANNING:

                    objSaleorder = new SalesOrderBO();
                    objSaleorder.BizUnit = CurrentUser.SBUID;
                    objSaleorder.UserPK = CurrentUser.PKUser.ToString();
                    objSaleorder.DeptPK = CurrentUser.CurrentDeptPK.ToString();
                    objSaleorder.OrderItemsList = new List<OrderItemsBO>();
                    objSaleorder.OrderItemsList.Add(GetSelectedOrdersForPlanning());
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objSaleorder);
                    S_XML[0] = xmlDoc.InnerXml;
                    break;

            }
        }


        /// <summary>
        /// Method for get selected orders
        /// </summary>
        private OrderItemsBO GetSelectedOrders()
        {
            OrderItemsBO objOrderItems = new OrderItemsBO();

            List<ItemsBO> ItemsList = new List<ItemsBO>();
            SetAllocationDetails();

            foreach (var Items in lstSaleOrderInfo)
                if (Items.chkChecked == true)
                {
                    ItemsBO objitems = new ItemsBO();
                    objitems.sodPK = Items.sodPK;
                    ItemsList.Add(objitems);
                }

            objOrderItems.ItemsList = ItemsList;
            Session.Remove(SessionStrings.SelectedOrderInfo);
            return objOrderItems;

        }
        /// <summary>
        /// For set filter values to advanced filter popup
        /// </summary>
        private void setFilterOption()
        {
            if (Session[SessionStrings.FilterCriteria] != null)
            {
                SalesOrderBO objSaleorder = new SalesOrderBO();
                objSaleorder = (SalesOrderBO)Session[SessionStrings.FilterCriteria];
                ddlProduct.SelectedIndex = Convert.ToInt32(ddlProduct.Items.IndexOf(ddlProduct.Items.FindByValue(objSaleorder.ProductPK)));
                ddlSize.SelectedIndex = Convert.ToInt32(ddlSize.Items.IndexOf(ddlSize.Items.FindByValue(objSaleorder.SizePK)));
                ddlPlanStage.SelectedIndex = Convert.ToInt32(ddlPlanStage.Items.IndexOf(ddlPlanStage.Items.FindByValue(objSaleorder.PlanStage)));
                txtRequiredBy.Text = objSaleorder.RequdBy;
                if (objSaleorder.ProductGroups[0].ProductGroupList != null)
                {
                    List<OroductGroupBO> GroupList = objSaleorder.ProductGroups[0].ProductGroupList;
                    foreach (var item in chkProdutGroup.Items.Cast<ListItem>())
                    {
                        bool alreadyExists = GroupList.Exists(itemLst => itemLst.ProductGroupPK.ToString() == item.Value);
                        if (alreadyExists) item.Selected = true;
                    }
                }
                if (objSaleorder.Orders[0].OrderList != null)
                {
                    List<OrderBO> ItemList = objSaleorder.Orders[0].OrderList;

                    foreach (var item in chklstOrders.Items.Cast<ListItem>())
                    {
                        bool alreadyExists = ItemList.Exists(itemLst => itemLst.sohPK.ToString() == item.Value);
                        if (alreadyExists) item.Selected = true;
                    }

                }

            }

        }

        //private void SetFilterStatus()
        //{
        //    if (Session[SessionStrings.FilterInfo] != null)
        //    {
        //        SalesOrderBO objSaleOrder = (SalesOrderBO)Session[SessionStrings.FilterInfo];
        //        ddlProduct.SelectedIndex = Convert.ToInt32(ddlProduct.Items.IndexOf(ddlProduct.Items.FindByValue(objSaleOrder.ProductPK)));
        //        ddlSize.SelectedIndex = Convert.ToInt32(ddlSize.Items.IndexOf(ddlSize.Items.FindByValue(objSaleOrder.SizePK)));
        //        txtRequiredBy.Text = objSaleOrder.RequdBy;
        //        // OrdersBO objOrders =objSaleOrder.Orders.
        //        List<OrderBO> ItemsList;
        //        if (Session[SessionStrings.CheckedOrdersInfo] != null)
        //            ItemsList = (List<OrderBO>)Session[SessionStrings.SelectedOrderInfo];
        //        else
        //            ItemsList = new List<OrderBO>();
        //        if(ItemsList!=null)
        //        foreach (var Items in ItemsList)
        //        {
        //            foreach (ListItem listItem in chklstOrders.Items)
        //            {
        //                if (Items.sohPK.ToString() == listItem.Value)
        //                    listItem.Selected = true;
        //            }
        //        }
        //    }

        //}
        private OrdersBO GetCheckedOrders()
        {
            OrdersBO objOrders = new OrdersBO();

            List<OrderBO> ItemsList = new List<OrderBO>();
            foreach (ListItem listItem in chklstOrders.Items)
            {
                if (listItem.Selected == true)
                {
                    OrderBO objOrder = new OrderBO();
                    objOrder.sohPK = Convert.ToInt32(listItem.Value.ToString());
                    ItemsList.Add(objOrder);

                }
            }
            // Session[SessionStrings.CheckedOrdersInfo] = ItemsList;
            objOrders.OrderList = ItemsList;
            return objOrders;


        }

        private OroductGroupsBO GetCheckedGroups()
        {
            OroductGroupsBO objOroductGroups = new OroductGroupsBO();
            List<OroductGroupBO> ItemsList = new List<OroductGroupBO>();
            foreach (ListItem listItem in chkProdutGroup.Items)
            {
                if (listItem.Selected == true)
                {
                    OroductGroupBO objProductGroup = new OroductGroupBO();
                    objProductGroup.ProductGroupPK = Convert.ToInt32(listItem.Value.ToString());
                    ItemsList.Add(objProductGroup);
                }
            }
            objOroductGroups.ProductGroupList = ItemsList;
            return objOroductGroups;
        }


        /// <summary>
        ///For get count of checked items
        /// </summary>

        private int CountSelectedItems()
        {
            SetAllocationDetails();
            int count = 0;
            foreach (var Items in lstSaleOrderInfo)
                if (Items.chkChecked == true)
                {
                    count++;
                }

            return count;
        }

        private OrderItemsBO GetSelectedOrdersForPlanning()
        {
            OrderItemsBO objOrderItems = new OrderItemsBO();

            List<ItemsBO> ItemsList = new List<ItemsBO>();
            SetAllocationDetails();

            foreach (var Items in lstSaleOrderInfo)
                if (Items.chkChecked == true)
                {
                    ItemsBO objitems = new ItemsBO();
                    objitems.sodPK = Items.sodPK;
                    objitems.Qty = ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO;
                    ItemsList.Add(objitems);
                }

            objOrderItems.ItemsList = ItemsList;
            Session.Remove(SessionStrings.SelectedOrderInfo);
            return objOrderItems;

        }



        private void SaveOrderPks()
        {
            CheckBox chbSelect;
            foreach (GridViewRow item in grdSaleOrder.Rows)
            {
                chbSelect = (CheckBox)item.FindControl("chbSelect");
                if (chbSelect.Checked)
                {
                    ItemsBO objitems = new ItemsBO();
                    objitems.sodPK = Convert.ToInt32(grdSaleOrder.DataKeys[item.RowIndex].Value.ToString());
                    // ItemsList.Add(objitems);

                }

            }


        }

        #endregion

        #region Action methods

        public DataSet FetchDbValues(int val, string[] XML)
        {
            return SaleOrderDL.GetSaleOrder(val, P_XML);

        }
        public DataSet SetValuesToDB(int val, string[] XML)
        {
            return SaleOrderDL.SaveSaleorder(val, XML);

        }
        #endregion

        #region Control Enum
        private enum ActionsEnum
        {
            ADVANCED_FILTER,
            BINDGRID,
            CHANGE,
            PLANNED,
            FILTER,
            ALLOCATION,
            PLANNING,
            PRODUCTION,
            DISPATCH,
            ORDERREALIZATION,
            CONTINUE,
            QUICKPLANNING,
            CLEAR
        }
        /// <summary>
        /// To control Page Actions
        /// </summary>
        public enum ControlsEnum
        {
            PRODUCT,
            ORDER,
            SIZE,
            PRODUCTGROUP,
            PLANSTAGE


        }
        #endregion
        #endregion
    }
}