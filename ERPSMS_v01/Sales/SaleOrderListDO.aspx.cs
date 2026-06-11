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
using CustomControls;

namespace ERPSMS_v01.Sales
{
    public partial class SaleOrderListDO : ERP.Store.UI.MyBasePage
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
                return this.ViewState[ViewstateStrings.TotalPages] == null ? 0 : (int)this.ViewState[ViewstateStrings.TotalPages];
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPages] = value;
            }
        }
        /// <summary>
        /// Customer Pk
        /// </summary>
        private int CustomerID
        {
            get
            {
                return this.ViewState[ViewstateStrings.CustomerID] == null ? 0 : (int)this.ViewState[ViewstateStrings.CustomerID];
            }
            set
            {
                this.ViewState[ViewstateStrings.CustomerID] = value;
            }
        }
        /// <summary>
        /// Sale Order ID
        /// </summary>
        private int SoId
        {
            get
            {
                return this.ViewState[ViewstateStrings.SoId] == null ? 0 : (int)this.ViewState[ViewstateStrings.SoId];
            }
            set
            {
                this.ViewState[ViewstateStrings.SoId] = value;
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
        /// To maintain count of selected pos
        /// </summary>
        private int SelectedSosCount
        {
            get
            {
                return this.ViewState[ViewstateStrings.SelectedSosCount] == null ? 0 : (int)this.ViewState[ViewstateStrings.SelectedSosCount];

            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedSosCount] = value;
            }
        }
        /// <summary>
        /// To maintain keep selected pos
        /// </summary>
        private List<long> SelectedSos
        {
            get
            {
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedSos];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedSos] = value;
            }

        }
        private List<long> SelectedCustomers
        {
            get
            {
                return (List<long>)this.ViewState[ViewstateStrings.SelectedCustomers];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedCustomers] = value;
            }

        }


        #endregion


        // Indicates the state as well as action
        private ActionsEnum commonActions;

        //page related Entity Object
        private ServiceUtility serviceUtilityObj;
        private SAL_ORDER_HDR objSalesOrderHeader;

        private List<SAL_ORDER_HDR> salesOrderHeaderList;
        private List<SAL_ORDER_DTL> salesOrderDetailsList;
        private List<long> SelectedSOList;
        private List<long> SelectedCustomerList;

        //List for binding details to controls      

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

                    SelectedCustomers = null;
                    SelectedSos = null;
                    SelectedSosCount = 0;

                    txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();

                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                    EntryStatus = EntryStatus.LISTMODE;
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
            SaleOrderService salesOrderServiceClient;
            salesOrderServiceClient = null;

            try
            {
                switch (type)
                {

                    case ControlsEnum.DEFAULT:
                        salesOrderServiceClient = new SaleOrderService();
                        salesOrderServiceClient = CommonFunctions.InitiateClient(salesOrderServiceClient);
                        objSalesOrderHeader = new SAL_ORDER_HDR();
                        serviceUtilityObj = new ServiceUtility();

                        objSalesOrderHeader.SOH_ACTIVE = 1;
                        if (hdfCustomerID.Value != "")
                            objSalesOrderHeader.SOH_CUSTOMER = Convert.ToInt32(hdfCustomerID.Value);
                        if (hdfSoPK.Value != "")
                            objSalesOrderHeader.SOH_PK = Convert.ToInt32(hdfSoPK.Value);
                        if (!string.IsNullOrEmpty(txtFromDate.Text))
                            serviceUtilityObj.FilterDate = DateTime.Parse(txtFromDate.Text);
                        if (!string.IsNullOrEmpty(txtToDate.Text))
                            serviceUtilityObj.FilterToDate = DateTime.Parse(txtToDate.Text);
                        int status=Convert.ToInt32(ddlStatus.SelectedValue);
                        salesOrderHeaderList = salesOrderServiceClient.GetSaleOrderHeader(objSalesOrderHeader, serviceUtilityObj, ApplicationSubType.DELIVERYORDER, status,PageType.SALE);
                        break;
                    case ControlsEnum.SODETAILS:
                        salesOrderServiceClient = new SaleOrderService();
                        salesOrderServiceClient = CommonFunctions.InitiateClient(salesOrderServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        salesOrderDetailsList = salesOrderServiceClient.GetSaleOrderDetails(SoId, serviceUtilityObj);
                        break;


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objSalesOrderHeader = null;
                serviceUtilityObj = null;
                salesOrderServiceClient = null;
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

                    case ControlsEnum.DEFAULT:
                        BindGrid(controlType);
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
            GridViewRow gvr;
            GridView grd;
            ExtGridView egrd;
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
            switch (commonActions)
            {
                #region extra grid ondemand data population
                case ActionsEnum.SODETAILS:
                    arg = ((Button)sender).CommandArgument;
                    gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                    if (gvr != null)
                    {
                        grd = gvr.FindControl("grdOrderDetails") as GridView;
                        if (string.IsNullOrEmpty(arg))
                        {
                            salesOrderDetailsList = null;
                        }
                        else
                        {
                            SoId = Convert.ToInt32(arg);
                            GetFieldValues(ControlsEnum.SODETAILS);
                        }
                        grd.Visible = true;
                        if (salesOrderDetailsList != null && salesOrderDetailsList.Count > 0)
                        {
                            grd.DataSource = salesOrderDetailsList;
                            grd.DataBind();
                        }
                        (gvr.FindControl("hdfIsExpandedOrders") as HiddenField).Value = "1";
                    }
                    break;


                #endregion
                #region Search
                case ActionsEnum.SEARCH:
                    PageIndex = "1";
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    break;
                #endregion

                #region reset form
                case ActionsEnum.CLEAR:
                    ResetForm();
                    break;
                #endregion
                #region PICKFORINVOICING
                case ActionsEnum.PICKFORINVOICING:
                    SetUIValuesToObject(commonActions);
                    break;
                #endregion
                #region Tab navigation
                case ActionsEnum.DEFAULT:
                    Response.Redirect(Resources.PageURL.SoListingDO);
                    break;
                case ActionsEnum.DELIVERYORDER:
                    Response.Redirect(Resources.PageURL.DeliveryOrder);
                    break;
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
                if ((sender as GridView).ID == "grdPoList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        //PoId = Convert.ToInt32(((HiddenField)e.Row.FindControl("hdfPOID")).Value);
                        //GetFieldValues(ControlsEnum.PODETAILS);
                        //if (poDetailsList != null)
                        //{
                        //    ((GridView)e.Row.FindControl("grdOrderDetails")).DataSource = poDetailsList;
                        //    ((GridView)e.Row.FindControl("grdOrderDetails")).DataBind();
                        //}

                    }
                }
                if (((GridView)sender).ID == "grdOrderDetails")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        (e.Row.FindControl("hdfHasChildren") as HiddenField).Value = CommonConstants.SELECT_VALUE_ZERO;
                    }
                }
                #endregion
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
                    ////Toggle the sort expression
                    //if (SortDirection == Resources.gComsRes.SortAscending)
                    //    SortDirection = Resources.gComsRes.SortDescending;
                    //else
                    //    SortDirection = Resources.gComsRes.SortAscending;
                }
                else
                {
                    //SortBy = e.SortExpression;
                    //SortDirection = Resources.gComsRes.SortAscending;
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

        #endregion

        #endregion
        #region Helper Methods

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

                foreach (GridViewRow grdrow in grdSoList.Rows)
                {
                    RadioButton rbtn;
                    rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                    if (rbtn.Checked)
                    {
                        bIsChecked = true;
                        CustomerID = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value);
                        SoId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOID")).Value);
                        break;
                    }
                }

                if (bIsChecked)
                    switch (mode)
                    {
                        case ActionsEnum.PICKFORINVOICING:
                            if (!IsSameCustomer(SelectedCustomers, CustomerID))
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Customer").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                            else if (!IsExixtPk(SelectedSos, SoId))
                            {
                                //Add Customers
                                if (SelectedCustomers != null)
                                {
                                    SelectedCustomerList = SelectedCustomers;
                                }
                                else
                                {
                                    SelectedCustomerList = new List<long>();
                                }
                                SelectedCustomerList.Add(CustomerID);
                                SelectedCustomers = SelectedCustomerList;
                                //Add Sos
                                if (SelectedSos != null)
                                {
                                    SelectedSOList = SelectedSos;
                                }
                                else
                                {
                                    SelectedSOList = new List<long>();
                                }
                                SelectedSOList.Add(SoId);
                                SelectedSos = SelectedSOList;
                                SelectedSosCount = SelectedSos.Count;

                                btnPickForInvoice.Text = GetLocalResourceObject("PickSoForDO").ToString() + "(" + SelectedSosCount.ToString() + ")";

                            }
                            else
                            {
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Item_Added;

                                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                            }
                            break;
                    }
                else
                {
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Select_PO").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                return returnObj;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        /// <summary>
        /// Set values to Session for handling edit and view or navigation
        /// </summary>
        private void SetUIEditView(ActionsEnum mode)
        {
            try
            {
                //Session[SessionStrings.ActivityPK] = null;
                bool bIsChecked = false;

                //foreach (GridViewRow grdrow in grdActivities.Rows)
                //{
                //    RadioButton rbtn;
                //    rbtn = (RadioButton)grdrow.FindControl("rbtSelect");

                //    if (rbtn.Checked)
                //    {
                //        // get pk from the grid and assign to CurrPk
                //        CurrPK = Convert.ToInt32(grdActivities.DataKeys[grdrow.RowIndex].Values[0]);
                //        Session[SessionStrings.ActivityPK] = CurrPK;
                //        bIsChecked = true;
                //    }
                //}
                if (bIsChecked)
                    switch (mode)
                    {
                        //case ActionsEnum.BASICINFO:
                        //    Response.Redirect(Resources.PageURL.ManageActivity, true);
                        //    break;
                        //case ActionsEnum.DOCUMENTS:
                        //    Response.Redirect(Resources.PageURL.ActivityAttachment, true);
                        //    break;
                        //case ActionsEnum.CHARGES:
                        //    Response.Redirect(Resources.PageURL.ActivityCharges, true);
                        //    break;
                        //case ActionsEnum.FLIGHTINFO:
                        //    Response.Redirect(Resources.PageURL.ActivityFlightInfo, true);
                        //    break;
                        //case ActionsEnum.ACTIVITYINPUTS:
                        //    Response.Redirect(Resources.PageURL.ActivityInputs, true);
                        //    break;
                        //case ActionsEnum.FREEUSAGE:
                        //    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ActivityFreeUsage), false);
                        //    break;
                        //case ActionsEnum.REPORT:
                        //    Response.Redirect(Resources.PageURL.ActivityCostReport, true);
                        //    break;

                    }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + "Please selct a row from the list" + "');", true);
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
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.DEFAULT:
                        grdSoList.DataSource = salesOrderHeaderList;
                        grdSoList.DataBind();
                        break;

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

                    //case ControlsEnum.AIRCRAFTTYPE:
                    //    ddlAcftType.Items.Clear();
                    //    if (adAircraftTypeMstList != null && adAircraftTypeMstList.Count > 0)
                    //    {
                    //        ddlAcftType.DataSource = adAircraftTypeMstList;
                    //        ddlAcftType.DataTextField = Resources.DataFieldRes.AircraftTypeCode;
                    //        ddlAcftType.DataValueField = Resources.DataFieldRes.AircraftTypePK;
                    //        ddlAcftType.DataBind();
                    //    }
                    //    ddlAcftType.Items.Insert(0, new ListItem(Resources.gComsRes.Select, CommonConstants.SELECTVAL));
                    //    break;
                    //case ControlsEnum.ACTIVITYTASKSTATUS:
                    //    ddlStatus.Items.Clear();
                    //    if (adAppConstCfgList != null && adAppConstCfgList.Count > 0)
                    //    {
                    //        ddlStatus.DataSource = adAppConstCfgList;
                    //        ddlStatus.DataTextField = Resources.DataFieldRes.ActivityTaskStatusData;
                    //        ddlStatus.DataValueField = Resources.DataFieldRes.ActivityTaskStatusValue;
                    //        ddlStatus.DataBind();
                    //    }
                    //    ddlStatus.Items.Insert(0, new ListItem(Resources.gComsRes.Select, CommonConstants.SELECTVAL));
                    //    break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Is Same Customer
        /// </summary>
        /// <param name="vendors"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsSameCustomer(List<long> customers, long pk)
        {

            bool flag = true;
            if (customers != null)
                foreach (long cus in customers)
                    if (cus != pk)
                    {
                        flag = false;
                        break;
                    }
            return flag;
        }
        /// <summary>
        /// I exist Po in list
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsExixtPk(List<long> lst, long pk)
        {
            bool flag = false;
            if (lst != null)
                foreach (long item in lst)
                    if (item == pk)
                    {
                        flag = true;
                        break;
                    }
            return flag;
        }

        /// <summary>
        /// Method used to Reset form Controls
        /// </summary>
        private void ResetForm()
        {
            txtCustomer.Text = string.Empty;
            txtSONumber.Text = string.Empty;
            hdfSoPK.Value = "0";
            hdfCustomerID.Value = "0";
            txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
            hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
            txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            hdfToDate.Value = DateTime.Now.ToString();

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
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {

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
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Captions.Information + "');", true);
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
            try
            {
                //if (EntryStatus == EntryStatus.VIEWMODE)
                //{
                //    hdfDefaultSubmit.Value = string.Empty;
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
                //}
                //else if (EntryStatus == EntryStatus.NEWMODE)
                //{
                //    hdfDefaultSubmit.Value = string.Empty;
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);
                //}
                //else if (EntryStatus == EntryStatus.ENTRYMODE)
                //{
                //    hdfDefaultSubmit.Value = string.Empty;
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                //}
                //else if (EntryStatus == EntryStatus.LISTMODE)
                //{
                //    hdfDefaultSubmit.Value = btnSearch.ClientID;
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                //}

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Captions.Information + "');", true);
            }
        }

        #endregion

        #region Enum
        /// <summary>
        /// Define Common Enum
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            SODETAILS

        }

        /// <summary>
        /// Define Activity Status Enum
        /// </summary>
        public enum StatusEnum
        {
            DRAFTED = 0,
            SUBMITTED = 1,
            UNATTENDED = 6
        }


        #endregion
    }
}