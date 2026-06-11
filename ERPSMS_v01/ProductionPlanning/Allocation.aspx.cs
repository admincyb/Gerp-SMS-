using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Configuration;
using ERP.Utilities;
using BusinessObject.ILibrary;
using BusinessObject.Sales;
using System.Xml;
using ERPSMS_v01.UserControls;
using DataAccess.ProductionDL;
using BusinessObject;
using BusinessObject.CommonManagement;
using System.Configuration;
using ERP.Utilities.Constants.DA;

namespace ERPSMS_v01.ProductionPlanning
{
    public partial class Allocation : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        #region Properties
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
        private string PageIndexFinishedGoods
        {
            get
            {
                return (string)this.ViewState["PageIndexFinishedGoods"];
            }
            set
            {
                this.ViewState["PageIndexFinishedGoods"] = value;
            }
        }

        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string PageIndexSelectedOrder
        {
            get
            {
                return (string)this.ViewState["PageIndexSelectedOrder"];
            }
            set
            {
                this.ViewState["PageIndexSelectedOrder"] = value;
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
        private int TotalPagesFinishedGoods
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.TotalPagesFinishedGoods];
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPagesFinishedGoods] = value;
            }
        }
        /// <summary>
        /// To maintain the total pages in viewstate
        /// </summary>
        private int TotalPagesSelectedOrder
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.TotalPagesSelectedOrder];
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPagesSelectedOrder] = value;
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
        // For Common Actions
        ActionsEnum commonAction;

        //For FinishedGoods Row Index
        private int FinishedGoods;

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


        //For save 
        private bool saveFlag;
        //For fetch FinishedGoods
        private DataSet dsAllocation;

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
                PageActionHandler();

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
            switch (type)
            {

                case ControlEnum.SELECTEDORDERS:
                    SetFilterXML(ControlsEnum.SELECTEDORDERS);
                    dsAllocation = FetchDbValues(1, P_XML);
                    break;
                case ControlEnum.FINISHEDGOODS:
                    SetFilterXML(ControlsEnum.FINISHEDGOODS);
                    dsAllocation = FetchDbValues(1, P_XML);
                    break;
                case ControlEnum.ALLOCATED:
                    SetFilterXML(ControlsEnum.ALLOCATED);
                    dsAllocationDetails = FetchDbValues(3, P_XML);
                    break;
                case ControlEnum.ORDERDETAILS:
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
                case ControlEnum.FINISHEDGOODS:
                    BindGrid(ControlEnum.FINISHEDGOODS);
                    break;
                case ControlEnum.SELECTEDORDERS:
                    BindGrid(ControlEnum.SELECTEDORDERS);
                    break;
                case ControlEnum.ALLOCATED:
                    BindGrid(ControlEnum.ALLOCATED);
                    break;
                case ControlEnum.ORDERDETAILS:
                    SetOrderDetails();
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
            else
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonAction = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }

            switch (commonAction)
            {
                case ActionsEnum.GRID:
                    GetFieldValues(ControlEnum.FINISHEDGOODS);
                    SetFieldValues(ControlEnum.FINISHEDGOODS);

                    GetFieldValues(ControlEnum.SELECTEDORDERS);
                    SetFieldValues(ControlEnum.SELECTEDORDERS);
                    break;
                case ActionsEnum.ALLOCATION:
                    SetAllocationDetails();
                    SetFieldValuesToXML(1);
                    dsDBMessgaes = SetValuesToDB(1, S_XML);
                    PrintDbMessages(dsDBMessgaes);

                    break;


                case ActionsEnum.ALLOCATED:
                    PageIndexAllocation = Convert.ToInt32(ERP.Utilities.CommonConstants.SELECT_VALUE_ONE);
                    GridViewRow grvFinishedGoods;
                    grvFinishedGoods = ((LinkButton)sender).Parent.Parent as GridViewRow;
                    ProPK = Convert.ToInt32(grdFinishedGoods.DataKeys[grvFinishedGoods.RowIndex].Value.ToString());
                    SetGridValues(grvFinishedGoods.RowIndex);
                    GetFieldValues(ControlEnum.ALLOCATED);
                    SetFieldValues(ControlEnum.ALLOCATED);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAssetType", "ShowContainerDiv('#divAllocatioDetails','" + Resources.PageNameRes.PlannedDetails + "','800','500');", true);
                    break;

                case ActionsEnum.PLAN:
                    GridViewRow grvSelectedOrders;
                    grvSelectedOrders = ((LinkButton)sender).Parent.Parent as GridViewRow;
                    SodPK = grdSelectedOrders.DataKeys[grvSelectedOrders.RowIndex].Value.ToString();
                    GetFieldValues(ControlEnum.ORDERDETAILS);
                    SetFieldValues(ControlEnum.ORDERDETAILS);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAssetType", "ShowContainerDiv('#DivOrderDtlPopup','" + Resources.PageNameRes.PlannedDetails + "','900','400');", true);
                    break;
                case ActionsEnum.EXIT:
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ClosePopup", "ClosePopup();", true);
                    Session.Remove(SessionStrings.AllocatedOrderInfo);
                    Response.Redirect(Resources.PageURL.SaleOrder);
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
                    string[] datakeyarray1;
                    datakeyarray1 = new string[1];
                    datakeyarray1[0] = Resources.DataFieldRes.OrderDtlPK;
                    grdSelectedOrders.DataKeyNames = datakeyarray1;

                    GetFieldValues(ControlEnum.SELECTEDORDERS);
                    SetFieldValues(ControlEnum.SELECTEDORDERS);

                    string[] datakeyarray2;
                    datakeyarray2 = new string[1];
                    datakeyarray2[0] = Resources.DataFieldRes.proPK;
                    grdFinishedGoods.DataKeyNames = datakeyarray2;


                    GetFieldValues(ControlEnum.FINISHEDGOODS);
                    SetFieldValues(ControlEnum.FINISHEDGOODS);



                    PageIndexFinishedGoods = ERP.Utilities.CommonConstants.SELECT_VALUE_ONE;
                    PageIndexSelectedOrder = ERP.Utilities.CommonConstants.SELECT_VALUE_ONE;

                    uclPagingFinishedGoods.TotalPages = TotalPagesFinishedGoods;
                    uclPagingFinishedGoods.CurrentPage = Convert.ToInt32(ERP.Utilities.CommonConstants.SELECT_VALUE_ONE);
                    uclPagingSelectedOrders.TotalPages = TotalPagesSelectedOrder;
                    uclPagingSelectedOrders.CurrentPage = Convert.ToInt32(ERP.Utilities.CommonConstants.SELECT_VALUE_ONE);

                    //uclAllocation.TotalPages = TotalPagesAllocation == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : TotalPagesAllocation;
                    //uclAllocation.CurrentPage = Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE);


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
            // uclPaging.CurrentPage = 1;
            uclPagingSelectedOrders.CurrentPage = 1;
            uclPagingFinishedGoods.CurrentPage = 1;
            uclAllocation.CurrentPage = 1;

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
            this.uclPagingFinishedGoods.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPagingFinishedGoods.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPagingFinishedGoods.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPagingFinishedGoods.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPagingFinishedGoods.PageChanged += new ActionHandler(this.ActionHandler);

            //For Selected orders
            this.uclPagingSelectedOrders.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPagingSelectedOrders.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPagingSelectedOrders.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPagingSelectedOrders.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPagingSelectedOrders.PageChanged += new ActionHandler(this.ActionHandler);

            //For Allocated details
            this.uclAllocation.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclAllocation.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclAllocation.NextPage += new ActionHandler(this.ActionHandler);
            this.uclAllocation.LastPage += new ActionHandler(this.ActionHandler);
            this.uclAllocation.PageChanged += new ActionHandler(this.ActionHandler);



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
                case "uclPagingFinishedGoods":

                    try
                    {
                        switch (e.Action)
                        {
                            case NavigationEnum.PAGECHANGE:
                                uclPagingFinishedGoods.CurrentPage = e.CurrentPage;
                                break;
                            case NavigationEnum.FIRST:
                                // Assign the current page index.
                                if (e.CurrentPage > 1)
                                    uclPagingFinishedGoods.CurrentPage = 1;
                                break;
                            case NavigationEnum.LAST:
                                // Assign the current page index.
                                if (e.CurrentPage <= e.TotalPages)
                                    uclPagingFinishedGoods.CurrentPage = e.TotalPages;
                                break;
                            case NavigationEnum.NEXT:
                                // Increment the current page index.
                                if (e.CurrentPage <= e.TotalPages)
                                    uclPagingFinishedGoods.CurrentPage++;
                                break;
                            case NavigationEnum.PREVIOUS:
                                // Decrement the current page index.
                                if (e.CurrentPage > 1)
                                    uclPagingFinishedGoods.CurrentPage--;
                                break;

                        }

                        PageIndexFinishedGoods = uclPagingFinishedGoods.CurrentPage.ToString();
                        GetFieldValues(ControlEnum.FINISHEDGOODS);
                        SetFieldValues(ControlEnum.FINISHEDGOODS);
                        EnableDisableButtons(e.TotalPages, ControlsEnum.FINISHEDGOODS);


                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
                    }
                    break;
                case "uclPagingSelectedOrders":
                    try
                    {
                        switch (e.Action)
                        {
                            case NavigationEnum.PAGECHANGE:
                                uclPagingSelectedOrders.CurrentPage = e.CurrentPage;
                                break;
                            case NavigationEnum.FIRST:
                                // Assign the current page index.
                                if (e.CurrentPage > 1)
                                    uclPagingSelectedOrders.CurrentPage = 1;
                                break;
                            case NavigationEnum.LAST:
                                // Assign the current page index.
                                if (e.CurrentPage <= e.TotalPages)
                                    uclPagingSelectedOrders.CurrentPage = e.TotalPages;
                                break;
                            case NavigationEnum.NEXT:
                                // Increment the current page index.
                                if (e.CurrentPage <= e.TotalPages)
                                    uclPagingSelectedOrders.CurrentPage++;
                                break;
                            case NavigationEnum.PREVIOUS:
                                // Decrement the current page index.
                                if (e.CurrentPage > 1)
                                    uclPagingSelectedOrders.CurrentPage--;
                                break;

                        }
                        SetAllocationDetails();
                        PageIndexSelectedOrder = uclPagingSelectedOrders.CurrentPage.ToString();
                        GetFieldValues(ControlEnum.SELECTEDORDERS);
                        SetFieldValues(ControlEnum.SELECTEDORDERS);
                        SetGridStatus();
                        EnableDisableButtons(e.TotalPages, ControlsEnum.SELECTEDORDERS);


                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
                    }

                    break;
                case "uclAllocation":
                    try
                    {
                        switch (e.Action)
                        {
                            case NavigationEnum.PAGECHANGE:
                                uclAllocation.CurrentPage = e.CurrentPage;
                                break;
                            case NavigationEnum.FIRST:
                                // Assign the current page index.
                                if (e.CurrentPage > 1)
                                    uclAllocation.CurrentPage = 1;
                                break;
                            case NavigationEnum.LAST:
                                // Assign the current page index.
                                if (e.CurrentPage <= e.TotalPages)
                                    uclAllocation.CurrentPage = e.TotalPages;
                                break;
                            case NavigationEnum.NEXT:
                                // Increment the current page index.
                                if (e.CurrentPage <= e.TotalPages)
                                    uclAllocation.CurrentPage++;
                                break;
                            case NavigationEnum.PREVIOUS:
                                // Decrement the current page index.
                                if (e.CurrentPage > 1)
                                    uclAllocation.CurrentPage--;
                                break;

                        }

                        PageIndexAllocation = Convert.ToInt32(uclAllocation.CurrentPage.ToString());
                        GetFieldValues(ControlEnum.ALLOCATED);
                        SetFieldValues(ControlEnum.ALLOCATED);
                        EnableDisableButtons(e.TotalPages, ControlsEnum.ALLOCATED);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAssetType", "ShowContainerDiv('#divAllocatioDetails','" + Resources.PageNameRes.PlannedDetails + "','600','500');", true);



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
                case ControlsEnum.FINISHEDGOODS:
                    // Should we disable the first link?
                    uclPagingFinishedGoods.FirstButtonEnabled = (uclPagingFinishedGoods.CurrentPage == 1) ? false : true;
                    // Should we disable the previous link?
                    uclPagingFinishedGoods.PreviousButtonEnabled = (uclPagingFinishedGoods.CurrentPage == 1) ? false : true;
                    // Should we enable the next link?
                    uclPagingFinishedGoods.NextButtonEnabled = (uclPagingFinishedGoods.CurrentPage < iTotalPages) ? true : false;
                    // Should we enable the last link?
                    uclPagingFinishedGoods.LastButtonEnabled = (uclPagingFinishedGoods.CurrentPage < iTotalPages) ? true : false;
                    break;
                case ControlsEnum.SELECTEDORDERS:
                    // Should we disable the first link?
                    uclPagingSelectedOrders.FirstButtonEnabled = (uclPagingSelectedOrders.CurrentPage == 1) ? false : true;
                    // Should we disable the previous link?
                    uclPagingSelectedOrders.PreviousButtonEnabled = (uclPagingSelectedOrders.CurrentPage == 1) ? false : true;
                    // Should we enable the next link?
                    uclPagingSelectedOrders.NextButtonEnabled = (uclPagingSelectedOrders.CurrentPage < iTotalPages) ? true : false;
                    // Should we enable the last link?
                    uclPagingSelectedOrders.LastButtonEnabled = (uclPagingSelectedOrders.CurrentPage < iTotalPages) ? true : false;
                    break;
                case ControlsEnum.ALLOCATED:
                    // Should we disable the first link?
                    uclAllocation.FirstButtonEnabled = (uclAllocation.CurrentPage == 1) ? false : true;
                    // Should we disable the previous link?
                    uclAllocation.PreviousButtonEnabled = (uclAllocation.CurrentPage == 1) ? false : true;
                    // Should we enable the next link?
                    uclAllocation.NextButtonEnabled = (uclAllocation.CurrentPage < iTotalPages) ? true : false;
                    // Should we enable the last link?
                    uclAllocation.LastButtonEnabled = (uclAllocation.CurrentPage < iTotalPages) ? true : false;
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

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");

        }

        #endregion


        #region Healper methods
        /// <summary>
        /// Method for set order details 
        /// </summary>
        private void SetOrderDetails()
        {
            if (dsOrderDetails != null)
            {

                lblOrderNo.Text = dsOrderDetails.Tables[0].Rows[0][Resources.DataFieldRes.SaleOrder].ToString();
                lblOrderProduct.Text = dsOrderDetails.Tables[0].Rows[0][Resources.DataFieldRes.ProtCode].ToString();
                lblOrderDescription.Text = dsOrderDetails.Tables[0].Rows[0][Resources.DataFieldRes.Product].ToString();
                lblRequiredBy.Text = Convert.ToDateTime(dsOrderDetails.Tables[0].Rows[0][Resources.DataFieldRes.ReqdBy].ToString()).ToString(ERP.Utilities.CommonConstants.DATEFORMAT);
                lblOrderSize.Text = dsOrderDetails.Tables[0].Rows[0][Resources.DataFieldRes.Size].ToString();
                lblOrderedQty.Text = Convert.ToDecimal(dsOrderDetails.Tables[0].Rows[0][Resources.DataFieldRes.OrderQty]).ToString("n0");
                lblDispatched.Text = dsOrderDetails.Tables[0].Rows[0][Resources.DataFieldRes.Dispatched].ToString();
                lblAllocated.Text = dsOrderDetails.Tables[0].Rows[0][Resources.DataFieldRes.Allocated].ToString();
                lblProduced.Text = dsOrderDetails.Tables[0].Rows[0][Resources.DataFieldRes.Produced].ToString();
                lblPlanned.Text = dsOrderDetails.Tables[0].Rows[0][Resources.DataFieldRes.Planned].ToString();
                lblBalanceToAllocate.Text = Convert.ToDecimal(dsOrderDetails.Tables[0].Rows[0][Resources.DataFieldRes.BalanceToProduce]).ToString("n0");
            }

        }
        /// <summary>
        /// Method for set product details from grid
        /// </summary>
        /// <param name="rowIndex"></param>
        private void SetGridValues(int rowIndex)
        {
            GridViewRow grvFinishedGoods = grdFinishedGoods.Rows[rowIndex];
            lblProduct.Text = ((Label)grvFinishedGoods.FindControl("lblProduct")).Text;
            lblDescription.Text = ((Label)grvFinishedGoods.FindControl("lblDescription")).ToolTip;
            lblSize.Text = ((Label)grvFinishedGoods.FindControl("lblSize")).Text;
        }


        #region Grid Status maintains


        /// <summary>
        /// For sett allocation details
        /// </summary>
        private void SetAllocationDetails()
        {
            if (Session[SessionStrings.AllocatedOrderInfo] != null)
                lstProducts = (List<ProductBO>)Session[SessionStrings.AllocatedOrderInfo];
            else
                lstProducts = new List<ProductBO>();

            TextBox txtQty;
            foreach (GridViewRow item in grdSelectedOrders.Rows)
            {
                txtQty = (TextBox)item.FindControl("txtAllocateNow");
                ProductBO objProduct = new ProductBO();
                objProduct.SodPK = Convert.ToInt32(grdSelectedOrders.DataKeys[item.RowIndex].Value.ToString());
                objProduct.Store = Convert.ToInt32(ERP.Utilities.CommonConstants.DEFAULT_STORE);

                if (txtQty.Text != "")
                {
                    objProduct.ItemAdd = true;
                    objProduct.Qty = Convert.ToDecimal(txtQty.Text);
                    // ((TextBox)item.FindControl("txtAllocateNow")).Text = string.Empty;
                }
                else
                    objProduct.ItemAdd = false;

                bool alreadyExists = lstProducts.Exists(itemLst => itemLst.SodPK == objProduct.SodPK);
                if (alreadyExists)

                    ChangeItem(objProduct);
                else

                    lstProducts.Add(objProduct);

            }

            Session[SessionStrings.AllocatedOrderInfo] = lstProducts;

        }


        /// <summary>
        /// for change the status of checked items
        /// </summary>
        /// <param name="item"></param>
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



        /// <summary>
        /// For Reset grid status
        /// </summary>
        private void SetGridStatus()
        {
            if (Session[SessionStrings.AllocatedOrderInfo] != null)
            {
                lstProducts = (List<ProductBO>)Session[SessionStrings.AllocatedOrderInfo];
                int sodPK;
                foreach (GridViewRow item in grdSelectedOrders.Rows)
                {
                    sodPK = Convert.ToInt32(grdSelectedOrders.DataKeys[item.RowIndex].Value.ToString());
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
                        //strHdrMsg = Resources.Messages.InformationSaved;
                        //BindGrid(ControlEnum.DBMSG);
                        //Session.Remove(SessionStrings.AllocatedOrderInfo);

                        //DiverrorMessages.Attributes.Remove("class");
                        //DiverrorMessages.Attributes.Add("class", "message-sucess");
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAssetType", "ShowContainerDiv('#" + DiverrorMessages.ClientID + "','" + strHdrMsg + "','500','300','" + Resources.PageURL.SaleOrderClient + "');", true);

                        litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Allocation);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                          + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.SaleOrder) + "');", true);


                        //   Response.Redirect(Resources.PageURL.SaleOrder);
                    }
                    else
                    {

                        litErrorMsg.Text = dsMsg.Tables[0].Rows[0][Resources.DataFieldRes.dbRetValTxt].ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);


                        //strHdrMsg = Resources.Messages.ErrorMessage;
                        //BindGrid(ControlEnum.DBMSG);
                        //DiverrorMessages.Attributes.Remove("class");
                        //DiverrorMessages.Attributes.Add("class", "message-error");

                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAssetType", "ShowContainerDiv('#" + DiverrorMessages.ClientID + "','" + strHdrMsg + "','500','300');", true);

                    }


                }

        }
        /// <summary>
        /// Reset form variables
        /// </summary>
        private void ResetForm()
        {

            PageIndexSelectedOrder = ERP.Utilities.CommonConstants.SELECT_VALUE_ONE;
            PageIndexFinishedGoods = ERP.Utilities.CommonConstants.SELECT_VALUE_ONE;

        }

        /// <summary>
        /// For get allocation details
        /// </summary>
        /// <returns></returns>
        private ProductsBO GetAllocationdetails()
        {
            ProductsBO obProducts = new ProductsBO();
            List<ProductBO> ProductsList = new List<ProductBO>();

            if (Session[SessionStrings.AllocatedOrderInfo] != null)
            {
                lstProducts = (List<ProductBO>)Session[SessionStrings.AllocatedOrderInfo];
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
                case ControlEnum.FINISHEDGOODS:
                    #region finished Goods
                    try
                    {

                        if (dsAllocation != null)
                        {

                            if (dsAllocation.Tables[0].Rows.Count > 0)
                                TotalPagesFinishedGoods = Convert.ToInt32(dsAllocation.Tables[0].Rows[0][Resources.DataFieldRes.PageCount].ToString());
                            else
                                TotalPagesFinishedGoods = 0;
                            uclPagingFinishedGoods.TotalPages = TotalPagesFinishedGoods;
                            PageIndexFinishedGoods = PageIndexFinishedGoods == null ? ERP.Utilities.CommonConstants.SELECT_VALUE_ONE : PageIndexFinishedGoods;
                            uclPagingFinishedGoods.CurrentPage = Convert.ToInt32(PageIndexFinishedGoods);

                            grdFinishedGoods.DataSource = dsAllocation.Tables[0];
                            grdFinishedGoods.DataBind();

                            uclPagingFinishedGoods.Visible = true;
                            uclPagingFinishedGoods.BindPager();

                        }


                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    #endregion
                    break;
                case ControlEnum.SELECTEDORDERS:
                    #region Selected Orders
                    try
                    {

                        if (dsAllocation != null)
                        {


                            if (dsAllocation.Tables[1].Rows.Count > 0)
                                TotalPagesSelectedOrder = Convert.ToInt32(dsAllocation.Tables[1].Rows[0][Resources.DataFieldRes.PageCount].ToString());
                            else
                                TotalPagesSelectedOrder = 0;

                            uclPagingSelectedOrders.TotalPages = TotalPagesSelectedOrder;
                            PageIndexSelectedOrder = PageIndexSelectedOrder == null ? ERP.Utilities.CommonConstants.SELECT_VALUE_ONE : PageIndexSelectedOrder;
                            uclPagingSelectedOrders.CurrentPage = Convert.ToInt32(PageIndexSelectedOrder);

                            grdSelectedOrders.DataSource = dsAllocation.Tables[1];
                            grdSelectedOrders.DataBind();

                            uclPagingSelectedOrders.Visible = true;
                            uclPagingSelectedOrders.BindPager();
                            //For focus first text box
                            if (grdSelectedOrders.Rows.Count > 0)
                                grdSelectedOrders.Rows[0].FindControl("txtAllocateNow").Focus();

                        }


                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    #endregion
                    break;
                case ControlEnum.ALLOCATED:
                    try
                    {

                        if (dsAllocationDetails != null)
                        {
                            if (dsAllocationDetails.Tables[0].Rows.Count > 0)
                                TotalPagesAllocation = Convert.ToInt32(dsAllocationDetails.Tables[0].Rows[0][Resources.DataFieldRes.PageCount].ToString());
                            else
                                TotalPagesAllocation = 1;
                            //uclAllocation
                            uclAllocation.TotalPages = TotalPagesAllocation;
                            PageIndexAllocation = PageIndexAllocation == 0 ? Convert.ToInt32(ERP.Utilities.CommonConstants.SELECT_VALUE_ONE) : PageIndexAllocation;
                            uclAllocation.CurrentPage = Convert.ToInt32(PageIndexAllocation);

                            grdAllocation.DataSource = dsAllocationDetails.Tables[0];
                            grdAllocation.DataBind();

                            uclAllocation.Visible = true;
                            uclAllocation.BindPager();

                        }


                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    break;

            }
        }


        /// <summary>
        /// For bind footer values in SelectedOrders grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdSelectedOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer && dsAllocation != null)
            {
                if (dsAllocation.Tables[1].Rows.Count > 0)
                {

                    ((Label)e.Row.FindControl("lblOrderQtyTotal")).Text = dsAllocation.Tables[1].Rows[0][Resources.DataFieldRes.OrderQtyTotal].Equals(DBNull.Value) ? "" :
                        Convert.ToDecimal(dsAllocation.Tables[1].Rows[0][Resources.DataFieldRes.OrderQtyTotal]).ToString("n0");
                    ((Label)e.Row.FindControl("lblBalanceToAllocateTotal")).Text = dsAllocation.Tables[1].Rows[0][Resources.DataFieldRes.BalanceToProduceTotal].Equals(DBNull.Value) ? "" :
                         Convert.ToDecimal(dsAllocation.Tables[1].Rows[0][Resources.DataFieldRes.BalanceToProduceTotal]).ToString("n0");
                    ((Label)e.Row.FindControl("lblAvailableTotal")).Text = dsAllocation.Tables[1].Rows[0][Resources.DataFieldRes.AvailableStockTotal].Equals(DBNull.Value) ? "" :
                        Convert.ToDecimal(dsAllocation.Tables[1].Rows[0][Resources.DataFieldRes.AvailableStockTotal]).ToString("n0");

                }

            }
        }

        /// <summary>
        /// For bind footer values in FinishedGoods grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void grdFinishedGoods_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer && dsAllocation != null)
            {
                if (dsAllocation.Tables[0].Rows.Count > 0)
                {
                    ((Label)e.Row.FindControl("lblTotalPhysicalStock")).Text = dsAllocation.Tables[0].Rows[0][Resources.DataFieldRes.PhysicalStockTotal].Equals(DBNull.Value) ? "" :
                         Convert.ToDecimal(dsAllocation.Tables[0].Rows[0][Resources.DataFieldRes.PhysicalStockTotal]).ToString("n0");
                    ((Label)e.Row.FindControl("lblTotalAllocated")).Text = dsAllocation.Tables[0].Rows[0][Resources.DataFieldRes.PrdAllocatedTotal].Equals(DBNull.Value) ? "" :
                         Convert.ToDecimal(dsAllocation.Tables[0].Rows[0][Resources.DataFieldRes.PrdAllocatedTotal]).ToString("n0");
                    ((Label)e.Row.FindControl("lblTotalAvailable")).Text = dsAllocation.Tables[0].Rows[0][Resources.DataFieldRes.AvailableTotal].Equals(DBNull.Value) ? "" :
                         Convert.ToDecimal(dsAllocation.Tables[0].Rows[0][Resources.DataFieldRes.AvailableTotal]).ToString("n0");

                }

            }
        }

        /// <summary>
        /// For generate filter XML
        /// </summary>
        /// <param name="type"></param>
        private void SetFilterXML(ControlsEnum type)
        {


            CurrentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            XmlDocument xmlDoc;
            P_XML = new string[6];

            switch (type)
            {
                case ControlsEnum.FINISHEDGOODS:

                    FinishedGoodsBO objFinishedGoods = new FinishedGoodsBO();
                    objFinishedGoods.Active = Convert.ToInt32(ERP.Utilities.CommonConstants.ACTIVE);
                    objFinishedGoods.category = Convert.ToInt32(BusinessObject.CommonManagement.ProductCategory.Product);
                    objFinishedGoods.BizUnit = CurrentUser.SBUID;
                    objFinishedGoods.PageNo = PageIndexFinishedGoods == null ? 1 : Convert.ToInt32(PageIndexFinishedGoods);
                    if (Session[SessionStrings.SelectedOrderItem] != null)
                    {
                        objFinishedGoods.OrderItemsList = new List<OrderItemsBO>();
                        objFinishedGoods.OrderItemsList.Add((OrderItemsBO)Session[SessionStrings.SelectedOrderItem]);

                    }
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objFinishedGoods);
                    P_XML[1] = xmlDoc.InnerXml;
                    break;
                case ControlsEnum.SELECTEDORDERS:
                    SalesOrderBO objSaleorder = new SalesOrderBO();
                    objSaleorder.BizUnit = CurrentUser.SBUID;
                    objSaleorder.PageNo = PageIndexSelectedOrder == null ? "1" : PageIndexSelectedOrder;


                    if (Session[SessionStrings.SelectedOrderItem] != null)
                    {
                        objSaleorder.OrderItemsList = new List<OrderItemsBO>();
                        objSaleorder.OrderItemsList.Add((OrderItemsBO)Session[SessionStrings.SelectedOrderItem]);

                    }
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objSaleorder);
                    P_XML[0] = xmlDoc.InnerXml;
                    break;
                case ControlsEnum.ALLOCATED:
                    AllocationdetailsBO objAllocationdetails = new AllocationdetailsBO();
                    objAllocationdetails.ProPk = ProPK;
                    objAllocationdetails.Active = Convert.ToInt32(ERP.Utilities.CommonConstants.ACTIVE);
                    objAllocationdetails.BizUnit = CurrentUser.SBUID;
                    objAllocationdetails.PageNo = PageIndexAllocation == 0 ? Convert.ToInt32(ERP.Utilities.CommonConstants.SELECT_VALUE_ONE) : PageIndexAllocation;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objAllocationdetails);
                    P_XML[2] = xmlDoc.InnerXml;
                    break;

                case ControlsEnum.ORDERDETAILS:
                    SelectedOrderBO objSelectedOrder = new SelectedOrderBO();
                    objSelectedOrder.BizUnit = CurrentUser.SBUID;
                    objSelectedOrder.SodPK = Convert.ToInt32(SodPK);
                    objSelectedOrder.PageNo = Convert.ToInt32(ERP.Utilities.CommonConstants.SELECT_VALUE_ONE);

                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objSelectedOrder);
                    P_XML[0] = xmlDoc.InnerXml;


                    break;


            }


        }

        /// <summary>
        /// Create XML for submit to DB
        /// </summary>
        /// <param name="val"></param>
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
                    objAllocationDetails.Module = Convert.ToInt32(ConfigurationManager.AppSettings["gERPModule"].ToString());
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
            return AllocationDL.GetAllocation(val, P_XML);

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
            CLOSE,
            EXIT

        }
        /// <summary>
        /// To control Page Actions
        /// </summary>
        public enum ControlsEnum
        {
            PRODUCT,
            ORDER,
            SIZE,
            FINISHEDGOODS,
            SELECTEDORDERS,
            ALLOCATED,
            ORDERDETAILS


        }
        #endregion
       
    }
    
}