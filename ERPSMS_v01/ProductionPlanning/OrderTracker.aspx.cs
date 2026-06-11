using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.ILibrary;
using BusinessObject.Sales;
using System.Xml;
using ERPSMS_v01.UserControls;
using DataAccess.ProductionDL;
using BusinessObject;
using BusinessObject.CommonManagement;
using System.Configuration;
using ERP.Utilities;
using ERP.Utilities.Constants.DA;
using BusinessObject.Common;
using ERPData;
using ERPService;
using System.Data;

namespace ERPSMS_v01.ProductionPlanning
{
    public partial class OrderTracker : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        #region Properties

        /// <summary>
        /// Total Despatched Quantity
        /// </summary>
        private int TotalDespatchedQty
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.TotalDespatchedQty]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalDespatchedQty] = value;
            }
        }

        /// <summary>
        /// Total Despatched Quantity
        /// </summary>
        private int TotalPackedQty
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.TotalPackedQty]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPackedQty] = value;
            }
        }


        /// <summary>
        /// Total Planned Quantity
        /// </summary>
        private int TotalPlannedQty
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.TotalPlannedQty]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPlannedQty] = value;
            }
        }

        /// <summary>
        /// Total Allocated Quantity
        /// </summary>
        private int TotalAllocatedQty
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.TotalAllocatedQty]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalAllocatedQty] = value;
            }
        }


        /// <summary>
        /// Product Code
        /// </summary>
        private string Product
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.Product];
            }
            set
            {
                this.ViewState[ViewstateStrings.Product] = value;
            }
        }

        /// <summary>
        /// Balance To Produce
        /// </summary>
        private string BalProduce
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.BalProduce];
            }
            set
            {
                this.ViewState[ViewstateStrings.BalProduce] = value;
            }
        }

        /// <summary>
        /// Balance To Plan
        /// </summary>
        private string BalPlan
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.BalPlan];
            }
            set
            {
                this.ViewState[ViewstateStrings.BalPlan] = value;
            }
        }

        /// <summary>
        /// Balance To Dispatch
        /// </summary>
        private string BalDispatch
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.BalDispatch];
            }
            set
            {
                this.ViewState[ViewstateStrings.BalDispatch] = value;
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
        private string PageIndexCustomersOrders
        {
            get
            {
                return (string)this.ViewState["PageIndexCustomersOrders"];
            }
            set
            {
                this.ViewState["PageIndexCustomersOrders"] = value;
            }
        }

        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string PageIndexAllocated
        {
            get
            {
                return (string)this.ViewState["PageIndexAllocated"];
            }
            set
            {
                this.ViewState["PageIndexAllocated"] = value;
            }
        }

        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string PageIndexPlanned
        {
            get
            {
                return (string)this.ViewState["PageIndexPlanned"];
            }
            set
            {
                this.ViewState["PageIndexPlanned"] = value;
            }
        }

        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string PageIndexDespatched
        {
            get
            {
                return (string)this.ViewState["PageIndexDespatched"];
            }
            set
            {
                this.ViewState["PageIndexDespatched"] = value;
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

        private string SohPK
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SohPK];
            }
            set
            {
                this.ViewState[ViewstateStrings.SohPK] = value;
            }

        }
        /// <summary>
        /// To maintain the total pages in viewstate
        /// </summary>
        private int TotalPagesCustomersOrders
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.TotalPagesCustomersOrders];
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPagesCustomersOrders] = value;
            }
        }
        /// <summary>
        /// To maintain the total pages in viewstate
        /// </summary>
        private int TotalPagesAllocated
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.TotalPagesAllocated];
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPagesAllocated] = value;
            }
        }

        /// <summary>
        /// To maintain the total pages in viewstate
        /// </summary>
        private int TotalPagesPlanned
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.TotalPagesPlanned];
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPagesPlanned] = value;
            }
        }
        /// <summary>
        /// To maintain the total pages in viewstate
        /// </summary>
        private int TotalPagesDespatched
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.TotalPagesDespatched];
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPagesDespatched] = value;
            }
        }


        private int PageIndexAllocation
        {
            get
            {
                return Convert.ToInt32(this.ViewState["PageIndexAllocation"]);
            }
            set
            {
                this.ViewState["PageIndexAllocation"] = value;
            }
        }

        private int TotalPagesAllocation
        {
            get
            {
                return (int)this.ViewState["TotalPagesAllocation"];
            }
            set
            {
                this.ViewState["TotalPagesAllocation"] = value;
            }
        }


        //For Product PK
        private int ProPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState["ProPK"]);
            }
            set
            {
                this.ViewState["ProPK"] = value;
            }
        }

        //For AuoComplete
        public static int CurrentSBUPK;

        // For Common Actions
        ActionsEnum commonAction;
        public event TreeOnDemand TreePopulationOnDemand;
        public delegate void TreeOnDemand(object sender, TreeNodeEventArgs e);

        public event NodeClicked NodeClickAction;
        public delegate void NodeClicked(object sender, EventArgs e);


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

        #endregion

        //For fetch Customers
        private DataSet dsCustomers;

        //For fetch Order Details
        private DataSet dsAllocation;
        private DataTable dtAllocated;
        private DataTable dtPlanned;
        private DataTable dtDespatched;

        //For fetch AllocationDetails
        private DataSet dsAllocationDetails;

        //For fetch OrderDetails
        private DataSet dsOrderDetails;

        //For get db return messages
        private DataSet dsDBMessgaes;

        //For Filter XML
        private string[] P_XML;

        //For Save XML
        private string[] S_XML;

        //For Order Total
        private Decimal AllocationTotal;

        //For selected products details
        List<ProductBO> lstProducts;

        User CurrentUser;
        #endregion

        #region Page Level Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CurrentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                PageActionHandler();
                CurrentSBUPK = CurrentUser.SBUID;
            }

        }



        #endregion

        #region Get Field Values

        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// Assign it to the page level variables
        /// </summary>
        public void GetFieldValues(ControlEnum type)
        {
            dsCustomers = null;
            dsAllocation = null;

            switch (type)
            {

                case ControlEnum.CUSTOMERS:
                    SetFilterXML(ControlsEnum.CUSTOMERS);
                    dsCustomers = FetchDbValues(2, P_XML);
                    break;
                case ControlEnum.ALLOCATED:
                    SetFilterXML(ControlsEnum.ALLOCATED);
                    dsAllocation = FetchDbValues(4, P_XML);
                    break;
                case ControlEnum.CUSTOMERSORDERS:
                    SetFilterXML(ControlsEnum.CUSTOMERSORDERS);
                    dsAllocation = FetchDbValues(3, P_XML);
                    break;
                case ControlEnum.PLANNED:
                    SetFilterXML(ControlsEnum.ALLOCATED);
                    dsAllocationDetails = FetchDbValues(3, P_XML);
                    break;
                case ControlEnum.DESPATCHED:
                    SetFilterXML(ControlsEnum.ORDERDETAILS);
                    dsOrderDetails = FetchDbValues(2, P_XML);
                    break;

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
                case ControlEnum.CUSTOMERS:
                    BindTreeView();
                    break;
                case ControlEnum.ORDERS:

                    break;
                case ControlEnum.CUSTOMERSORDERS:
                    BindGrid(ControlEnum.CUSTOMERSORDERS);
                    break;
                case ControlEnum.ALLOCATED:
                    BindGrid(ControlEnum.ALLOCATED);
                    break;
                case ControlEnum.PLANNED:
                    BindGrid(ControlEnum.ALLOCATED);
                    break;
                case ControlEnum.DESPATCHED:
                    BindGrid(ControlEnum.DESPATCHED);
                    break;
            }

        }
        #endregion


        #region Action handler
        public void ActionHandler(object sender, EventArgs e)
        {

            if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
            {
                commonAction = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
            }
            else if (sender.GetType().IsEquivalentTo(typeof(Button)))
            {
                commonAction = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
            }
            else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
            {
                commonAction = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                if (commonAction == ActionsEnum.AUTOSEARCH)
                {
                    GetFieldValues(ControlEnum.CUSTOMERS);
                    SetFieldValues(ControlEnum.CUSTOMERS);
                    hdfCustomerID.Value = "0";
                }
                else
                {
                    SodPK = ((ImageButton)sender).CommandArgument;
                    GridViewRow clickedRow = ((ImageButton)sender).NamingContainer as GridViewRow;
                    Label lblProduct = (Label)clickedRow.FindControl("lblSKU");
                    Product = lblProduct.Text;
                    lblProductName.Text = Product;
                    BalProduce = ((Label)clickedRow.FindControl("lblBalanceToProduce")).Text;
                    BalPlan = ((Label)clickedRow.FindControl("lblBalanceToPlan")).Text;
                    BalDispatch = ((Label)clickedRow.FindControl("lblBalanceToDispatch")).Text;
                }
            }

            switch (commonAction)
            {
                case ActionsEnum.VIEW:
                    GetFieldValues(ControlEnum.ALLOCATED);
                    SetFieldValues(ControlEnum.ALLOCATED);
                    break;
                case ActionsEnum.SEARCH:
                    GetFieldValues(ControlEnum.CUSTOMERS);
                    SetFieldValues(ControlEnum.CUSTOMERS);
                    break;
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
                    txtFromDateOrder.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDateOrder.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    txtToDateOrder.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDateOrder.Value = DateTime.Now.ToString();


                    GetFieldValues(ControlEnum.CUSTOMERS);
                    SetFieldValues(ControlEnum.CUSTOMERS);


                    PageIndexCustomersOrders = ERP.Utilities.CommonConstants.SELECT_VALUE_ONE;
                    TotalPagesCustomersOrders = int.Parse(ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO);
                    //PageIndexAllocated = ERP.Utilities.CommonConstants.SELECT_VALUE_ONE;

                    uclPagingCustomersOrders.TotalPages = TotalPagesCustomersOrders;
                    uclPagingCustomersOrders.CurrentPage = Convert.ToInt32(ERP.Utilities.CommonConstants.SELECT_VALUE_ONE);

                    uclPagingCustomersOrders.Visible = false;


                    //uclPagingAllocated.TotalPages = TotalPagesAllocated;
                    //uclPagingAllocated.CurrentPage = Convert.ToInt32(ERP.Utilities.CommonConstants.SELECT_VALUE_ONE);

                    //uclAllocation.TotalPages = TotalPagesAllocation == null ? Convert.ToInt32(ERP.Utilities.CommonConstants.SELECT_VALUE_ONE) : TotalPagesAllocation;
                    //uclAllocation.CurrentPage = Convert.ToInt32(ERP.Utilities.CommonConstants.SELECT_VALUE_ONE);


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

            uclPagingCustomersOrders.CurrentPage = 1;


        }
        /// <summary>
        /// Prerender event of the Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_InitComponents", "$(document).ready(function(){InitComponents();});", true);


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
            //For finished goods
            this.uclPagingCustomersOrders.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPagingCustomersOrders.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPagingCustomersOrders.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPagingCustomersOrders.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPagingCustomersOrders.PageChanged += new ActionHandler(this.ActionHandler);



            this.Init += new EventHandler(this.Page_Init);
        }

        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        {
            switch (((PagerControl)sender).ID)
            {
                case "uclPagingCustomersOrders":

                    try
                    {
                        switch (e.Action)
                        {
                            case NavigationEnum.PAGECHANGE:
                                uclPagingCustomersOrders.CurrentPage = e.CurrentPage;
                                break;
                            case NavigationEnum.FIRST:
                                // Assign the current page index.
                                if (e.CurrentPage > 1)
                                    uclPagingCustomersOrders.CurrentPage = 1;
                                break;
                            case NavigationEnum.LAST:
                                // Assign the current page index.
                                if (e.CurrentPage <= e.TotalPages)
                                    uclPagingCustomersOrders.CurrentPage = e.TotalPages;
                                break;
                            case NavigationEnum.NEXT:
                                // Increment the current page index.
                                if (e.CurrentPage <= e.TotalPages)
                                    uclPagingCustomersOrders.CurrentPage++;
                                break;
                            case NavigationEnum.PREVIOUS:
                                // Decrement the current page index.
                                if (e.CurrentPage > 1)
                                    uclPagingCustomersOrders.CurrentPage--;
                                break;

                        }

                        PageIndexCustomersOrders = uclPagingCustomersOrders.CurrentPage.ToString();
                        GetFieldValues(ControlEnum.CUSTOMERSORDERS);
                        SetFieldValues(ControlEnum.CUSTOMERSORDERS);
                        EnableDisableButtons(e.TotalPages, ControlsEnum.CUSTOMERSORDERS);


                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
                    }
                    break;


            }

        }

        /// <summary>
        /// Method used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages, ControlsEnum type)
        {
            switch (type)
            {
                case ControlsEnum.CUSTOMERSORDERS:
                    // Should we disable the first link?
                    uclPagingCustomersOrders.FirstButtonEnabled = (uclPagingCustomersOrders.CurrentPage == 1) ? false : true;
                    // Should we disable the previous link?
                    uclPagingCustomersOrders.PreviousButtonEnabled = (uclPagingCustomersOrders.CurrentPage == 1) ? false : true;
                    // Should we enable the next link?
                    uclPagingCustomersOrders.NextButtonEnabled = (uclPagingCustomersOrders.CurrentPage < iTotalPages) ? true : false;
                    // Should we enable the last link?
                    uclPagingCustomersOrders.LastButtonEnabled = (uclPagingCustomersOrders.CurrentPage < iTotalPages) ? true : false;
                    break;


            }
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

        #endregion


        #region Healper methods

        private void SetOrderDetails()
        {
            if (dsOrderDetails != null)
            {


            }

        }
        private void SetGridValues(int rowIndex)
        {
            GridViewRow grvFinishedGoods = grdCustomersOrders.Rows[rowIndex];
            lblProduct.Text = ((Label)grvFinishedGoods.FindControl("lblProduct")).Text;
            lblDescription.Text = ((Label)grvFinishedGoods.FindControl("lblDescription")).Text;
            lblSize.Text = ((Label)grvFinishedGoods.FindControl("lblSize")).Text;
        }


        #region Grid Status maintains
        //For sett allocation details
        private void SetAllocationDetails()
        {
            if (Session[ERP.Utilities.SessionStrings.AllocatedOrderInfo] != null)
                lstProducts = (List<ProductBO>)Session[ERP.Utilities.SessionStrings.AllocatedOrderInfo];
            else
                lstProducts = new List<ProductBO>();

            TextBox txtQty;
            foreach (GridViewRow item in grdAllocated.Rows)
            {
                txtQty = (TextBox)item.FindControl("txtAllocateNow");
                ProductBO objProduct = new ProductBO();
                objProduct.SodPK = Convert.ToInt32(grdAllocated.DataKeys[item.RowIndex].Value.ToString());
                objProduct.Store = Convert.ToInt32(ERP.Utilities.CommonConstants.DEFAULT_STORE);

                if (txtQty.Text != "")
                {
                    objProduct.ItemAdd = true;
                    objProduct.Qty = Convert.ToDecimal(txtQty.Text);
                    ((TextBox)item.FindControl("txtAllocateNow")).Text = string.Empty;
                }
                else
                    objProduct.ItemAdd = false;
                bool alreadyExists = lstProducts.Exists(itemLst => itemLst.SodPK == objProduct.SodPK);
                if (alreadyExists)

                    ChangeItem(objProduct);
                else

                    lstProducts.Add(objProduct);

            }

            Session[ERP.Utilities.SessionStrings.AllocatedOrderInfo] = lstProducts;

        }

        //for change the status of checked items
        private void ChangeItem(ProductBO item)
        {
            if (lstProducts.Count > 0)
                foreach (var Items in lstProducts)
                    if (Items.SodPK == item.SodPK)
                    {
                        Items.Qty = item.Qty;
                        Items.Store = item.Store;
                        Items.ItemAdd = item.ItemAdd;
                    }
        }


        //For Reset grid status
        private void SetGridStatus()
        {
            if (Session[ERP.Utilities.SessionStrings.AllocatedOrderInfo] != null)
            {
                lstProducts = (List<ProductBO>)Session[ERP.Utilities.SessionStrings.AllocatedOrderInfo];
                int sodPK;
                foreach (GridViewRow item in grdAllocated.Rows)
                {
                    sodPK = Convert.ToInt32(grdAllocated.DataKeys[item.RowIndex].Value.ToString());
                    if (lstProducts.Exists(itemLst => itemLst.SodPK == sodPK && itemLst.ItemAdd == true))
                    {
                        var value = lstProducts.Where(x => x.SodPK == sodPK).Select(x => x.Qty).FirstOrDefault();

                        ((TextBox)item.FindControl("txtAllocateNow")).Text = value.ToString();
                    }

                }
            }

        }

        #endregion

        /// <summary>
        /// Method for Print db messages
        /// </summary>
        private void PrintDbMessages(DataSet dsMsg)
        {
            string strHdrMsg;
            if (dsMsg != null)
                if (dsMsg.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(dsMsg.Tables[0].Rows[0][Resources.DataFieldRes.dbRetVal].ToString()) > 0)
                    {
                        strHdrMsg = Resources.Messages.InformationSaved;
                        BindGrid(ControlEnum.DBMSG);
                        Session.Remove(ERP.Utilities.SessionStrings.AllocatedOrderInfo);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAssetType", "ShowContainerDiv('#DiverrorMessages','" + strHdrMsg + "','500','300','" + Resources.PageURL.SaleOrder + "');", true);
                        //   Response.Redirect(Resources.PageURL.SaleOrder);
                    }
                    else
                    {
                        litErrorMsg.Text = dsMsg.Tables[0].Rows[0][Resources.DataFieldRes.dbRetValTxt].ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                        //strHdrMsg = Resources.Messages.ErrorMessage;
                        //BindGrid(ControlEnum.DBMSG);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAssetType", "ShowContainerDiv('#DiverrorMessages','" + strHdrMsg + "','500','300');", true);

                    }


                }

        }

        private void ResetForm()
        {

            PageIndexAllocated = ERP.Utilities.CommonConstants.SELECT_VALUE_ONE;
            PageIndexCustomersOrders = ERP.Utilities.CommonConstants.SELECT_VALUE_ONE;

        }

        private ProductsBO GetAllocationdetails()
        {
            ProductsBO obProducts = new ProductsBO();
            List<ProductBO> ProductsList = new List<ProductBO>();

            if (Session[ERP.Utilities.SessionStrings.AllocatedOrderInfo] != null)
            {
                lstProducts = (List<ProductBO>)Session[ERP.Utilities.SessionStrings.AllocatedOrderInfo];
                foreach (var Items in lstProducts)
                    if (Items.ItemAdd)
                    {
                        ProductBO objProduct = new ProductBO();
                        objProduct.Qty = Items.Qty;
                        objProduct.SodPK = Items.SodPK;
                        objProduct.Store = Items.Store;
                        ProductsList.Add(objProduct);
                    }
            }
            obProducts.ProductList = ProductsList;

            return obProducts;
        }

        /// <summary>
        /// Method for TreeView binding
        /// </summary>
        /// <summary>
        /// Method to bind the Role Tree
        /// </summary>
        private void BindTreeView()
        {
            try
            {
                ClearGrid();
                lblDespatchedDetails.Text = string.Empty;
                //lblPlannedDetails.Text = string.Empty;
                lblAllocatedDetails.Text = string.Empty;
                lblProductName.Text = string.Empty;
                //lblPlannedDetails.Text = string.Empty;
                lblPackedDetail.Text = string.Empty;

                List<Customers> customerList = new List<Customers>();

                int i = -1;

                foreach (DataRow row in dsCustomers.Tables[0].Rows)
                {
                    Customers customers = new Customers();
                    customers.CustomerID = Convert.ToInt32(row[5]);
                    customers.CustomerName = row[6] as String;

                    bool has = customerList.Any(cus => cus.CustomerID == Convert.ToInt32(row[5]));
                    if (has)
                    {

                        customerList[i].OrdersList.Add(new Orders(
                              Convert.ToInt32(row[0]),
                              row[2] as String,
                              Convert.ToInt32(row[4])));
                    }
                    else
                    {
                        customerList.Add(customers);
                        i++;
                        customerList[i].OrdersList.Add(new Orders(
                           Convert.ToInt32(row[0]),
                           row[2] as String,
                           Convert.ToInt32(row[4])));
                    }


                }
                PopulateTreeViewControl(customerList);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        // This method is used to populate the TreeView Control
        private void PopulateTreeViewControl(List<Customers> customerList)
        {
            TreeNode parentNode = null;
            trvCustomers.Nodes.Clear();

            foreach (Customers customer in customerList)
            {
                parentNode = new TreeNode(customer.CustomerName,
                             customer.CustomerID.ToString());
                parentNode.NavigateUrl = "javascript:void(0)";
                foreach (Orders order in customer.OrdersList)
                {
                    if (order.OrderStatus == 2)
                    {
                        TreeNode childNode = new TreeNode(order.OrderName,
                                             order.OrderID.ToString());
                        parentNode.ChildNodes.Add(childNode);
                    }
                }

                parentNode.Collapse();

                // Show all checkboxes
                //trvUtilityTree.ShowCheckBoxes = TreeNodeTypes.All;
                trvCustomers.Nodes.Add(parentNode);
            }
        }


        #region Tree Events

        /// <summary>
        /// Delegate call for populate on demand true - style
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PopulateParentsChild(object sender, TreeNodeEventArgs e)
        {
            if (TreePopulationOnDemand != null)
            {
                TreePopulationOnDemand(sender, e);
            }
        }

        /// <summary>
        /// Action Handler to Fired on Tree Node Select
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler_onSelect(object sender, EventArgs e)
        {
            if (NodeClickAction != null)
            {
                NodeClickAction(sender, e);

            }

            ClearGrid();
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideCustomersOrders", "ShowHideCustomersOrders(1);", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideAllocated", "ShowHideAllocated(0);", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHidePlanned", "ShowHidePlanned(0);", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideDespatched", "ShowHideDespatched(0);", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHidePacked", "ShowHidePacked(0);", true);

            lblDespatchedDetails.Text = string.Empty;
            //lblPlannedDetails.Text = string.Empty;
            lblAllocatedDetails.Text = string.Empty;
            lblProductName.Text = string.Empty;
            //lblPlannedDetails.Text = string.Empty;
            lblPackedDetail.Text = string.Empty;

            SohPK = trvCustomers.SelectedNode.Value.ToString();
            GetFieldValues(ControlEnum.CUSTOMERSORDERS);
            SetFieldValues(ControlEnum.CUSTOMERSORDERS);
        }





        #endregion
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
                case ControlEnum.CUSTOMERSORDERS:
                    #region finished Goods
                    try
                    {

                        if (dsAllocation != null)
                        {

                            if (dsAllocation.Tables[0].Rows.Count > 0)
                                TotalPagesCustomersOrders = Convert.ToInt32(dsAllocation.Tables[0].Rows[0][Resources.DataFieldRes.PageCount].ToString());

                            uclPagingCustomersOrders.TotalPages = TotalPagesCustomersOrders;
                            PageIndexCustomersOrders = PageIndexCustomersOrders == null ? ERP.Utilities.CommonConstants.SELECT_VALUE_ONE : PageIndexCustomersOrders;
                            uclPagingCustomersOrders.CurrentPage = Convert.ToInt32(PageIndexCustomersOrders);

                            grdCustomersOrders.DataSource = dsAllocation.Tables[0];
                            grdCustomersOrders.DataBind();

                            grdAllocated.DataSource = null;
                            grdAllocated.DataBind();

                            //grdPlanned.DataSource = null;
                            //grdPlanned.DataBind();

                            grdDespatched.DataSource = null;
                            grdDespatched.DataBind();

                            grdPackedDetails.DataSource = null;
                            grdPackedDetails.DataBind();



                            uclPagingCustomersOrders.Visible = true;
                            uclPagingCustomersOrders.BindPager();

                        }


                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    #endregion
                    break;
                case ControlEnum.ALLOCATED:
                    #region Selected Orders
                    try
                    {
                        if (dsAllocation != null)
                        {
                            TotalAllocatedQty = 0;
                            TotalPlannedQty = 0;
                            TotalDespatchedQty = 0;
                            TotalPackedQty = 0;

                            if (dsAllocation.Tables[0].Rows.Count == 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideAllocated", "ShowHideAllocated(0);", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideAllocated", "ShowHideAllocated(1);", true);
                            }


                            if (dsAllocation.Tables[1].Rows.Count == 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHidePlanned", "ShowHidePlanned(0);", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHidePlanned", "ShowHidePlanned(1);", true);
                            }

                            if (dsAllocation.Tables[2].Rows.Count == 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideDespatched", "ShowHideDespatched(0);", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideDespatched", "ShowHideDespatched(1);", true);
                            }

                            if (dsAllocation.Tables[3].Rows.Count == 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHidePacked", "ShowHidePacked(0);", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideDespatched", "ShowHidePacked(1);", true);
                            }


                            lblAllocatedDetails.Text = string.Empty;
                            //lblPlannedDetails.Text = string.Empty;
                            lblDespatchedDetails.Text = string.Empty;
                            //lblPlannedDetails.Text = string.Empty;
                            lblPackedDetail.Text = string.Empty;

                            grdAllocated.DataSource = dsAllocation.Tables[0];
                            grdAllocated.DataBind();

                            //grdPlanned.DataSource = dsAllocation.Tables[1];
                            //grdPlanned.DataBind();

                            grdDespatched.DataSource = dsAllocation.Tables[2];
                            grdDespatched.DataBind();

                            grdPackedDetails.DataSource = dsAllocation.Tables[3];
                            grdPackedDetails.DataBind();

                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    #endregion
                    break;

            }
        }


        private void ClearGrid()
        {
            grdAllocated.DataSource = null;
            grdAllocated.DataBind();

            //grdPlanned.DataSource = null;
            //grdPlanned.DataBind();

            grdDespatched.DataSource = null;
            grdDespatched.DataBind();

            grdCustomersOrders.DataSource = null;
            grdCustomersOrders.DataBind();

            grdPackedDetails.DataSource = null;
            grdPackedDetails.DataBind();


            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideCustomer", " ShowHideCustomersOrders(0);", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideAllocated", "ShowHideAllocated(0);", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHidePlanned", "ShowHidePlanned(0);", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideDespatched", "ShowHideDespatched(0);", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHidePacked", "ShowHidePacked(0);", true);


        }
        protected void grdAllocated_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && dsAllocation != null)
            {
                if (dsAllocation.Tables[0].Rows.Count > 0)
                {

                    TotalAllocatedQty = TotalAllocatedQty + int.Parse(((HiddenField)e.Row.FindControl("hdfAllocatedQty")).Value);

                    lblAllocatedDetails.Text = " Total Quantity = " + TotalAllocatedQty.ToString() + " Bal. Allocate/Produce = " + BalProduce;
                }

            }

        }

        protected void grdCustomersOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer && dsAllocation != null)
            {
                if (dsAllocation.Tables[0].Rows.Count > 0)
                {

                    ((Label)e.Row.FindControl("lblTotalToDispatch")).Text = Convert.ToDecimal(dsAllocation.Tables[0].Rows[0][Resources.DataFieldRes.SodQtyDispatchedTotal]).ToString("n0");
                    ((Label)e.Row.FindControl("lblAllocatedTotal")).Text = Convert.ToDecimal(dsAllocation.Tables[0].Rows[0][Resources.DataFieldRes.SodQtyAllocatedTotal]).ToString("n0");
                    ((Label)e.Row.FindControl("lblPlannedTotal")).Text = Convert.ToDecimal(dsAllocation.Tables[0].Rows[0][Resources.DataFieldRes.SodQtyPlannedTotal]).ToString("n0");
                    ((Label)e.Row.FindControl("lblBlaToplanTotal")).Text = Convert.ToDecimal(dsAllocation.Tables[0].Rows[0][Resources.DataFieldRes.SodBalToPlanTotal]).ToString("n0");
                    ((Label)e.Row.FindControl("lblTotalOrderQty")).Text = Convert.ToDecimal(dsAllocation.Tables[0].Rows[0][Resources.DataFieldRes.SodQtyTotal]).ToString("n0");
                    ((Label)e.Row.FindControl("lblTotalPacked")).Text = Convert.ToDecimal(dsAllocation.Tables[0].Rows[0][Resources.DataFieldRes.SodPackedTotal]).ToString("n0");


                }

            }
        }

        protected void grdPlanned_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && dsAllocation != null)
            {
                if (dsAllocation.Tables[1].Rows.Count > 0)
                {
                    // TotalPlannedQty = TotalPlannedQty + int.Parse(((HiddenField)e.Row.FindControl("hfPlannedQty")).Value);

                    //lblPlannedDetails.Text = "    " + GetLocalResourceObject("TotalQty").ToString() + " = " + TotalPlannedQty.ToString() + "    " + GetLocalResourceObject("BalanceToPlan").ToString() + " = " + BalPlan;
                }

            }

        }

        protected void grdDespatched_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && dsAllocation != null)
            {
                if (dsAllocation.Tables[2].Rows.Count > 0)
                {
                    TotalDespatchedQty = TotalDespatchedQty + int.Parse(((HiddenField)e.Row.FindControl("hdfDespatchedQty")).Value);

                    lblDespatchedDetails.Text = "    Total Quantity = " + TotalDespatchedQty.ToString() + "    Bal.Dispatch = " + BalDispatch;
                }
            }

        }

        protected void grdPackedDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && dsAllocation != null)
            {
                if (dsAllocation.Tables[3].Rows.Count > 0)
                {
                    TotalPackedQty = TotalPackedQty + int.Parse(((HiddenField)e.Row.FindControl("hdfPackedQty")).Value);
                    lblPackedDetail.Text = "    Total Quantity Packed = " + TotalPackedQty.ToString();
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer && dsAllocation != null)
            {
                if (dsAllocation.Tables[3].Rows.Count > 0)
                {
                    ((Label)e.Row.FindControl("lblTotalPackedQty")).Text = TotalPackedQty.ToString("n0");


                }

            }

        }



        private void SetFilterXML(ControlsEnum type)
        {
            CurrentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            XmlDocument xmlDoc;
            P_XML = new string[4];

            switch (type)
            {
                case ControlsEnum.CUSTOMERS:

                    CustomersBO objCustomers = new CustomersBO();
                    objCustomers.Active = Convert.ToInt32(ActiveStatus.ACTIVE);
                    //if (hdfCustomerID.Value!="0")
                    //    objCustomers.CustomerPK = hdfCustomerID.Value;
                    if (txtCustomer.Text != string.Empty && txtCustomer.Text != GetLocalResourceObject("EnterCustomer").ToString())
                        objCustomers.CustomerName = txtCustomer.Text;

                    objCustomers.BizUnit = CurrentUser.SBUID;
                    objCustomers.FromDate = DateTime.Parse(txtFromDateOrder.Text);
                    objCustomers.ToDate = DateTime.Parse(txtToDateOrder.Text);

                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objCustomers);
                    P_XML[1] = xmlDoc.InnerXml;
                    break;
                case ControlsEnum.CUSTOMERSORDERS:

                    OrderListBO objOrderList = new OrderListBO();
                    objOrderList.Soh_PK = int.Parse(SohPK);
                    objOrderList.BizUnit = CurrentUser.SBUID;
                    objOrderList.PageNo = PageIndexCustomersOrders == null ? 1 : Convert.ToInt32(PageIndexCustomersOrders);

                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objOrderList);
                    P_XML[2] = xmlDoc.InnerXml;
                    break;
                case ControlsEnum.ALLOCATED:
                    OrderDetailsListBO objOrderDetailsList = new OrderDetailsListBO();
                    objOrderDetailsList.BizUnit = CurrentUser.SBUID;
                    objOrderDetailsList.Sod_PK = int.Parse(SodPK);

                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objOrderDetailsList);
                    P_XML[3] = xmlDoc.InnerXml;
                    break;
                case ControlsEnum.PLANNED:
                    AllocationdetailsBO objAllocationdetails = new AllocationdetailsBO();
                    objAllocationdetails.ProPk = ProPK;
                    objAllocationdetails.Active = Convert.ToInt32(ERP.Utilities.CommonConstants.ACTIVE);
                    objAllocationdetails.BizUnit = CurrentUser.SBUID;
                    objAllocationdetails.PageNo = PageIndexAllocation == 0 ? Convert.ToInt32(ERP.Utilities.CommonConstants.SELECT_VALUE_ONE) : PageIndexAllocation;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objAllocationdetails);
                    P_XML[2] = xmlDoc.InnerXml;
                    break;

                case ControlsEnum.DESPATCHED:
                    SelectedOrderBO objSelectedOrder = new SelectedOrderBO();
                    objSelectedOrder.BizUnit = CurrentUser.SBUID;
                    objSelectedOrder.SodPK = Convert.ToInt32(SodPK);
                    objSelectedOrder.PageNo = Convert.ToInt32(ERP.Utilities.CommonConstants.SELECT_VALUE_ONE);

                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objSelectedOrder);
                    P_XML[0] = xmlDoc.InnerXml;


                    break;


            }


        }

        private void SetFieldValuesToXML(int val)
        {
            CurrentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            XmlDocument xmlDoc;
            S_XML = new string[1];
            switch (val)
            {
                case 1:
                    AllocationDetailsBO objAllocationDetails = new AllocationDetailsBO();
                    objAllocationDetails.BizUnit = CurrentUser.SBUID;
                    objAllocationDetails.DepaertmentPK = CurrentUser.CurrentDeptPK;
                    objAllocationDetails.UserPK = CurrentUser.PKUser;
                    objAllocationDetails.Mode = Convert.ToInt32(Mode.Allocation);
                    objAllocationDetails.Module = Convert.ToInt32(ConfigurationManager.AppSettings["MODULE"].ToString());
                    objAllocationDetails.AlhDate = DateTime.Now;
                    objAllocationDetails.AllocationType = Convert.ToInt32(AllocationType.Initial);
                    objAllocationDetails.Active = Convert.ToInt32(ERP.Utilities.CommonConstants.ACTIVE);
                    objAllocationDetails.ProductsList = new List<ProductsBO>();
                    objAllocationDetails.ProductsList.Add(GetAllocationdetails());
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objAllocationDetails);
                    S_XML[0] = xmlDoc.InnerXml;
                    break;
            }

        }



        #endregion

        #region Action methods


        public DataSet FetchDbValues(int val, string[] XML)
        {
            return CustomerOrderTrackerDL.GetCustomers_Orders(val, P_XML);

        }

        public DataSet SetValuesToDB(int val, string[] XML)
        {
            return AllocationDL.SaveSaleorder(val, XML[0]);
        }

        #endregion

        #region Control Enum
        private enum ActionsEnum
        {
            GRID,
            ALLOCATION,
            CANCEL,
            PLAN,
            ALLOCATED,
            SELECT,
            SEARCH,
            AUTOSEARCH,
            VIEW

        }
        /// <summary>
        /// To control Page Actions
        /// </summary>
        public enum ControlsEnum
        {
            PRODUCT,
            ORDER,
            SIZE,
            CUSTOMERSORDERS,
            ALLOCATED,
            ORDERDETAILS,
            PLANNED,
            DESPATCHED,
            CUSTOMERS,
            ORDERS


        }
        #endregion

    }
}