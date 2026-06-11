using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Sales;
using System.Xml;
using ERPSMS_v01.UserControls;
using BusinessObject;
using BusinessObject.CommonManagement;
using System.Configuration;
using ERP.Utilities;
using ERP.Utilities.Constants.DA;
using BusinessObject.Common;
using ERPData;
using ERPService;
using System.Data;
using BusinessObject.Shipping;
using ERPManager;
using CustomControls;

namespace CustomerPortal.Dashboard
{
    public partial class OrderTracker : ERP.Store.UI.MyBasePage
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
        private string CustomerID
        {
            get
            {
                return (string)this.ViewState["CustomerID"];
            }
            set
            {
                this.ViewState["CustomerID"] = value;
            }

        }
        private int HeaderCustomerPK
        {
            get
            {
                return (int)this.ViewState["CustomerPK"];
            }
            set
            {
                this.ViewState["CustomerPK"] = value;
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
        private DateTime FromDate
        {
            get
            {
                return Convert.ToDateTime(this.ViewState[ViewstateStrings.fromDate]);
            }
            set
            {
                this.ViewState[ViewstateStrings.fromDate] = value;
            }
        }
        private DateTime ToDate
        {
            get
            {
                return Convert.ToDateTime(this.ViewState[ViewstateStrings.toDate]);
            }
            set
            {
                this.ViewState[ViewstateStrings.toDate] = value;
            }
        }
        private bool IsExportExcel
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsExportExcel] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsExportExcel].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsExportExcel] = value;
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

        ShippingPlanOrder ShippingPlanOrderObj;
        QustionNaireBO QustionNaireBOObj;

        private DataSet dsShippingList;
        private DataSet dsSPDetails;
        private DataSet dsSPStatus;
        private DataSet dsQuestionnair;
        private DataSet dsQuestionnairData;

        private ServiceUtility serviceUtilityObj;
        private DataSet dsPageData;
        private DataTable dtUserData;
        private DataSet dsOrderTracker;

        private int SPID;
        private int QusPK = 0;
        private string SProcedure = string.Empty;


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

        private string BaseCurrency;

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
            EntryStatus = EntryStatus.LISTMODE;
            CurrentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {                
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
        public void GetFieldValues(ControlsEnum type)
        {
            dsCustomers = null;
            dsAllocation = null;

            int sopk;

            string getxml;
            BusinessObject.User currentUser;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            switch (type)
            {

                case ControlsEnum.CUSTOMERS:
                    SetFilterXML(ControlsEnum.CUSTOMERS);
                    dsCustomers = FetchDbValues(2, P_XML);
                    break;
                case ControlsEnum.ALLOCATED:
                    SetFilterXML(ControlsEnum.ALLOCATED);
                    dsAllocation = FetchDbValues(4, P_XML);
                    break;
                case ControlsEnum.CUSTOMERSORDERS:
                    int cusID = 0;
                    sopk = Convert.ToInt32(SohPK);
                    int Status = Convert.ToInt32(ERP.Utilities.CommonConstants.SELECTVAL);
                    serviceUtilityObj = new ServiceUtility();
                    serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                    serviceUtilityObj.PageSize = grdCustomersOrders.PageSize;
                    serviceUtilityObj.TotalRecords = 0;
                    dsPageData = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanList(
                        new BusinessObject.GridPrams()
                        {
                            SortBy = string.IsNullOrEmpty(SortBy) ? Resources.DataFieldRes.SPDate : SortBy,
                            SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                            FromDate = string.Empty,
                            ToDate = string.Empty,
                            SearchBy = Resources.DataFieldRes.SPStatus,
                            SearchValue = Status.ToString()
                        }, currentUser, 0, Convert.ToInt32(ERP.Utilities.CommonConstants.ACTIVE), Status, cusID, sopk, serviceUtilityObj.CurrentPage, serviceUtilityObj.PageSize, string.Empty, string.Empty,string.Empty, string.Empty, 1);
                    if (dsPageData.Tables[0].Rows.Count > 0)
                    {
                        serviceUtilityObj.TotalRecords = dsPageData.Tables[0].Rows.Count > 0 ? Convert.ToInt32(dsPageData.Tables[0].Rows[0][0].ToString()) : 0;
                        TotalPagesCustomersOrders = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                    }
                    dsOrderTracker = BusinessLogic.Shipping.ShippingPlanBL.GetOrderTrackerList(sopk, Convert.ToInt32(CustomerID), FromDate.ToString(), ToDate.ToString());
                    SetFilterXML(ControlsEnum.CUSTOMERSORDERS);
                    dsAllocation = FetchDbValues(3, P_XML);
                    break;
                case ControlsEnum.PLANNED:
                    SetFilterXML(ControlsEnum.ALLOCATED);
                    dsAllocationDetails = FetchDbValues(3, P_XML);
                    break;
                case ControlsEnum.DESPATCHED:
                    SetFilterXML(ControlsEnum.ORDERDETAILS);
                    dsOrderDetails = FetchDbValues(2, P_XML);
                    break;
                case ControlsEnum.SHIPPINGPLANLIST:

                    break;
                case ControlsEnum.SPDEATILS:
                    dsSPDetails = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanDetails(0, SPID, Convert.ToInt32(ERP.Utilities.CommonConstants.ACTIVE));
                    break;
                case ControlsEnum.STATUSDETAILS:
                    dsSPStatus = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanStatus(SPID);
                    break;
                case ControlsEnum.QUESTIONNAIRE:
                    dsQuestionnair = BusinessLogic.Shipping.ShippingPlanBL.GetQuestionnaire(QusPK, Convert.ToInt32(ERP.Utilities.CommonConstants.ACTIVE));
                    break;
                case ControlsEnum.QUESTIONNAIREDATA:
                    if (SProcedure != string.Empty)
                    {
                        QustionNaireBOObj = (QustionNaireBO)SetUIValuesToObject(type);
                        if (QustionNaireBOObj.CusPK != 0)
                        {
                            getxml = CommonFunctions.XmlSerialize<QustionNaireBO>(QustionNaireBOObj);
                            dsQuestionnairData = BusinessLogic.Shipping.ShippingPlanBL.GetQuestionnaireData(SProcedure, getxml);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowError", "ShowError();", true);
                        }
                    }
                    break;
                case ControlsEnum.USERCUSTOMER:
                    dtUserData = new DataTable();
                    dtUserData = BusinessLogic.CommonManagement.CommonManagement.GetUserCustomer(currentUser.PKUser, currentUser.SBUID);
                    break;
                case ControlsEnum.ORDERTRACKER:
                    sopk = Convert.ToInt32(SohPK);
                    dsOrderTracker = BusinessLogic.Shipping.ShippingPlanBL.GetOrderTrackerList(sopk, Convert.ToInt32(CustomerID), FromDate.ToString(), ToDate.ToString());
                    break;
                #region BASECURRENCY
                case ControlsEnum.BASECURRENCY:
                    //Gets Base Currency
                    DataTable dtcurr = getcurrency();
                    if (dtcurr != null && dtcurr.Rows.Count > 0)
                    {
                        CurrencyMstService CurrencyMstServiceClient = new CurrencyMstService();
                        BaseCurrency = (CurrencyMstServiceClient.GetCurrencyCodeName(Convert.ToInt16(dtcurr.Rows[0]["ACF_DATA"].ToString()))).Split('-')[0].Trim(); ;
                    }
                    break;
                #endregion

            }

        }
        #endregion

        #region Set Field Values

        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// </summary>
        public void SetFieldValues(ControlsEnum type)
        {
            switch (type)
            {
                case ControlsEnum.CUSTOMERS:
                    BindTreeView();
                    ClearGrid();
                    break;
                case ControlsEnum.ORDERS:

                    break;
                case ControlsEnum.CUSTOMERSORDERS:
                    BindGrid(ControlsEnum.CUSTOMERSORDERS);
                    break;
                case ControlsEnum.ALLOCATED:
                    BindGrid(ControlsEnum.ALLOCATED);
                    break;
                case ControlsEnum.PLANNED:
                    BindGrid(ControlsEnum.ALLOCATED);
                    break;
                case ControlsEnum.DESPATCHED:
                    BindGrid(ControlsEnum.DESPATCHED);
                    break;
                case ControlsEnum.STATUSDETAILS:
                    BindGrid(ControlsEnum.STATUSDETAILS);
                    break;
                case ControlsEnum.QUESTIONNAIRE:
                    BindDropDown(ControlsEnum.QUESTIONNAIRE);
                    break;
                case ControlsEnum.QUESTIONNAIREDATA:
                    if (dsQuestionnairData != null && dsQuestionnairData.Tables[0].Rows.Count > 0)
                    {
                        lblTransDetails.Text = HttpUtility.HtmlDecode(dsQuestionnairData.Tables[0].Rows[0]["TRANS_TEXT"].ToString());
                        //TransDetails.Visible = true;
                    }
                    else
                    {
                        lblTransDetails.Text = string.Empty;
                    }

                    break;
                case ControlsEnum.USERCUSTOMER:
                    if (dtUserData.Rows.Count > 0)
                    {
                        TextBox txtCustomer = (TextBox)Page.Master.FindControl("txtCustomerSearch");
                        HiddenField hdfCustomer = (HiddenField)Page.Master.FindControl("hdfCustomerSearch");
                        if (txtCustomer != null && hdfCustomer != null)
                        {
                            txtCustomer.Text = dtUserData.Rows[0]["CUS_NAME"].ToString();
                            hdfCustomer.Value = dtUserData.Rows[0]["CUS_PK"].ToString();
                            txtCustomer.Enabled = false;
                            SohPK = "0";
                            CustomerID = hdfCustomer.Value;
                            GetFieldValues(ControlsEnum.ORDERTRACKER);
                            SetFieldValues(ControlsEnum.ORDERTRACKER);
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_Disableauto", "Disableautocomplete();", true);
                    }
                    break;
                case ControlsEnum.ORDERTRACKER:
                    BindGrid(ControlsEnum.ORDERTRACKER);
                    break;
            }

        }
        #endregion


        #region Action handler
        public void ActionHandler(object sender, EventArgs e)
        {
            GridViewRow gvr;
            GridView grd;
            string arg;

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

                    //hdfCustomerID.Value = "0";
                }
                else
                {
                    //    SodPK = ((ImageButton)sender).CommandArgument;
                    //    GridViewRow clickedRow = ((ImageButton)sender).NamingContainer as GridViewRow;
                    //    Label lblProduct = (Label)clickedRow.FindControl("lblSKU");
                    //    Product = lblProduct.Text;
                    //    lblProductName.Text = Product;
                    //    BalProduce = ((Label)clickedRow.FindControl("lblBalanceToProduce")).Text;
                    //    BalPlan = ((Label)clickedRow.FindControl("lblBalanceToPlan")).Text;
                    //    BalDispatch = ((Label)clickedRow.FindControl("lblBalanceToDispatch")).Text;
                }
            }
            else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
            {
                commonAction = ActionsEnum.SHOWDETAILS;
            }
            else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
            {
                commonAction = ActionsEnum.SELECTINDEXCHANGED;
            }
            switch (commonAction)
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
                            dsSPDetails = null;
                        }
                        else
                        {
                            SPID = Convert.ToInt32(arg);
                            GetFieldValues(ControlsEnum.SPDEATILS);
                        }
                        grd.Visible = true;
                        if (dsSPDetails != null && dsSPDetails.Tables[0].Rows.Count > 0)
                        {
                            grd.DataSource = dsSPDetails.Tables[0];
                            grd.DataBind();
                        }
                        (gvr.FindControl("hdfIsExpandedOrders") as HiddenField).Value = "1";
                    }

                    break;

                #endregion
                case ActionsEnum.SELECT:
                    GetFieldValues(ControlsEnum.ALLOCATED);
                    SetFieldValues(ControlsEnum.ALLOCATED);
                    break;
                case ActionsEnum.SEARCH:
                    GetFieldValues(ControlsEnum.CUSTOMERS);
                    SetFieldValues(ControlsEnum.CUSTOMERS);
                    break;
                case ActionsEnum.VIEW:
                    EntryStatus = EntryStatus.VIEWMODE;
                    arg = ((ImageButton)sender).CommandArgument;
                    SPID = Convert.ToInt32(arg);
                    GetFieldValues(ControlsEnum.STATUSDETAILS);
                    SetFieldValues(ControlsEnum.STATUSDETAILS);
                    break;
                case ActionsEnum.AUTOSEARCH:
                    GetFieldValues(ControlsEnum.CUSTOMERS);
                    SetFieldValues(ControlsEnum.CUSTOMERS);
                    break;
                case ActionsEnum.SELECTINDEXCHANGED:
                    QusPK = Convert.ToInt32(ddlQuestionnaire.SelectedValue);
                    GetFieldValues(ControlsEnum.QUESTIONNAIRE);
                    if (dsQuestionnair != null && dsQuestionnair.Tables[0].Rows.Count > 0)
                    {
                        SProcedure = dsQuestionnair.Tables[0].Rows[0]["QST_QUERY_TEXT"].ToString();
                        GetFieldValues(ControlsEnum.QUESTIONNAIREDATA);
                        SetFieldValues(ControlsEnum.QUESTIONNAIREDATA);
                    }
                    break;
                #region Invoice Print
                case ActionsEnum.PRINT:
                    HiddenField hdfInvType;
                    HiddenField hdfInvCode;
                    HiddenField hdfInvPK;
                    GridViewRow grwInvDetails = (GridViewRow)((LinkButton)(sender)).Parent.Parent;
                    hdfInvType = (HiddenField)grwInvDetails.FindControl("hdfInvType");
                    hdfInvCode = (HiddenField)grwInvDetails.FindControl("hdfAptCode");
                    hdfInvPK = (HiddenField)grwInvDetails.FindControl("hdfInvPK");
                    ConfigurationSettings();
                    if (hdfInvType.Value != string.Empty)
                    {
                        if (Convert.ToInt32(hdfInvType.Value) == (int)SalesInvoiceType.Domestic)
                        {
                            if (IsExportExcel)
                            {
                                Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + hdfInvPK.Value + "&APPTYPE=" + hdfInvCode.Value + "&APPSUBTYPE=1");
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfInvPK.Value + "&APPTYPE=" + hdfInvCode.Value + "&APPSUBTYPE=1") + "');", true);
                            }
                        }
                        else if (Convert.ToInt32(hdfInvType.Value) == (int)SalesInvoiceType.Export)
                        {
                            if (IsExportExcel)
                            {
                                Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + hdfInvPK.Value + "&APPTYPE=" + hdfInvCode.Value + "&APPSUBTYPE=2");
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfInvPK.Value + "&APPTYPE=" + hdfInvCode.Value + "&APPSUBTYPE=2") + "');", true);
                            }
                        }
                        else if (Convert.ToInt32(hdfInvType.Value) == (int)SalesInvoiceType.Proforma)
                        {
                            if (IsExportExcel)
                            {
                                Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + hdfInvPK.Value + "&APPTYPE=" + hdfInvCode.Value + "&APPSUBTYPE=3");
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfInvPK.Value + "&APPTYPE=" + hdfInvCode.Value + "&APPSUBTYPE=3") + "');", true);
                            }
                        }
                    }
                    break;
                #endregion
                #region SC Print
                case ActionsEnum.SCPRINT:
                    string ScPK = ((LinkButton)sender).CommandArgument;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ScPK + "&APPTYPE=" + ApplicationType.SO + "&APPSUBTYPE=") + "');", true);
                    break;
                #endregion
            }

        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (((GridView)sender).ID == "grdCustomersOrders")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Button imgApproved = e.Row.FindControl("imgApproved") as Button;
                        HiddenField hdfTrxStatus = e.Row.FindControl("hdfTrxStatus") as HiddenField;
                        if (Convert.ToInt32(hdfTrxStatus.Value) == (int)ShippingTabsEnum.BillofLoading)
                        {
                            imgApproved.CssClass = GetLocalResourceObject("Completed").ToString();
                            imgApproved.ToolTip = Resources.Captions.Completed;
                            
                        }
                        else
                        {
                            imgApproved.CssClass = GetLocalResourceObject("pending").ToString();
                            imgApproved.ToolTip = Resources.Captions.Pending;
                        }
                    }

                }
                if (((GridView)sender).ID == "grdOrderDetails")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        (e.Row.FindControl("hdfHasChildren") as HiddenField).Value = ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO;
                    }
                }
                if (((GridView)sender).ID == "grdAllocated")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        HiddenField hdfTrxStatus = e.Row.FindControl("hdfTrxStatus") as HiddenField;
                        hdfTrxStatus.Value = hdfTrxStatus.Value != string.Empty ? hdfTrxStatus.Value : "0";
                        Label lblPaymentDone = e.Row.FindControl("lblPaymentDone") as Label;
                        Label lblContainerConditionChecked = e.Row.FindControl("lblContainerConditionChecked") as Label;
                        Label lblContainerInspected = e.Row.FindControl("lblContainerInspected") as Label;
                        Label lblQADocsUploaded = e.Row.FindControl("lblQADocsUploaded") as Label;
                        Label lblExportDocsUploaded = e.Row.FindControl("lblExportDocsUploaded") as Label;
                        Label lblLoadingPlanCompleted = e.Row.FindControl("lblLoadingPlanCompleted") as Label;
                        Label lblPhotographsuploaded = e.Row.FindControl("lblPhotographsuploaded") as Label;
                        Label lblSDgenerated = e.Row.FindControl("lblSDgenerated") as Label;
                        Label lblContainerReleased = e.Row.FindControl("lblContainerReleased") as Label;
                        Label lblBL = e.Row.FindControl("lblBL") as Label;
                        if (lblPaymentDone.Text != string.Empty)
                        {
                            lblPaymentDone.Text = string.Format(Resources.Constants.DateTimeFormatGrid, Convert.ToDateTime(lblPaymentDone.Text));
                        }
                        if (lblContainerConditionChecked.Text != string.Empty)
                        {
                            lblContainerConditionChecked.Text = string.Format(Resources.Constants.DateTimeFormatGrid, Convert.ToDateTime(lblContainerConditionChecked.Text));
                        }
                        if (lblContainerInspected.Text != string.Empty)
                        {
                            lblContainerInspected.Text = string.Format(Resources.Constants.DateTimeFormatGrid, Convert.ToDateTime(lblContainerInspected.Text));
                        }
                        if (lblQADocsUploaded.Text != string.Empty)
                        {
                            lblQADocsUploaded.Text = string.Format(Resources.Constants.DateTimeFormatGrid, Convert.ToDateTime(lblQADocsUploaded.Text));
                        }
                        if (lblExportDocsUploaded.Text != string.Empty)
                        {
                            lblExportDocsUploaded.Text = string.Format(Resources.Constants.DateTimeFormatGrid, Convert.ToDateTime(lblExportDocsUploaded.Text));
                        }
                        if (lblLoadingPlanCompleted.Text != string.Empty)
                        {
                            lblLoadingPlanCompleted.Text = string.Format(Resources.Constants.DateTimeFormatGrid, Convert.ToDateTime(lblLoadingPlanCompleted.Text));
                        }
                        if (lblPhotographsuploaded.Text != string.Empty)
                        {
                            lblPhotographsuploaded.Text = string.Format(Resources.Constants.DateTimeFormatGrid, Convert.ToDateTime(lblPhotographsuploaded.Text));
                        }
                        if (lblSDgenerated.Text != string.Empty)
                        {
                            lblSDgenerated.Text = string.Format(Resources.Constants.DateTimeFormatGrid, Convert.ToDateTime(lblSDgenerated.Text));
                        }
                        if (lblContainerReleased.Text != string.Empty)
                        {
                            lblContainerReleased.Text = string.Format(Resources.Constants.DateTimeFormatGrid, Convert.ToDateTime(lblContainerReleased.Text));
                        }
                        if (lblBL.Text != string.Empty)
                        {
                            lblBL.Text = string.Format(Resources.Constants.DateTimeFormatGrid, Convert.ToDateTime(lblBL.Text));
                        }


                        switch (Convert.ToInt32(hdfTrxStatus.Value))
                        {
                            case (int)ShippingTabsEnum.PaymentCleared:
                                e.Row.Cells[0].Attributes.Add("class", "bggreen");
                                e.Row.Cells[1].Attributes.Add("class", "bgred");
                                lblContainerConditionChecked.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[2].Attributes.Add("class", "bgred");
                                lblContainerInspected.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[3].Attributes.Add("class", "bgred");
                                lblQADocsUploaded.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[4].Attributes.Add("class", "bgred");
                                lblExportDocsUploaded.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[5].Attributes.Add("class", "bgred");
                                lblLoadingPlanCompleted.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[6].Attributes.Add("class", "bgred");
                                lblPhotographsuploaded.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[7].Attributes.Add("class", "bgred");
                                lblSDgenerated.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[8].Attributes.Add("class", "bgred");
                                lblContainerReleased.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[9].Attributes.Add("class", "bgred");
                                lblBL.Text = GetLocalResourceObject("NotProcessed").ToString();
                                break;
                            case (int)ShippingTabsEnum.ContainerEvaluated:
                                e.Row.Cells[0].Attributes.Add("class", "bggreen");
                                e.Row.Cells[1].Attributes.Add("class", "bggreen");
                                e.Row.Cells[2].Attributes.Add("class", "bgred");
                                lblContainerInspected.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[3].Attributes.Add("class", "bgred");
                                lblQADocsUploaded.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[4].Attributes.Add("class", "bgred");
                                lblExportDocsUploaded.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[5].Attributes.Add("class", "bgred");
                                lblLoadingPlanCompleted.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[6].Attributes.Add("class", "bgred");
                                lblPhotographsuploaded.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[7].Attributes.Add("class", "bgred");
                                lblSDgenerated.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[8].Attributes.Add("class", "bgred");
                                lblContainerReleased.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[9].Attributes.Add("class", "bgred");
                                lblBL.Text = GetLocalResourceObject("NotProcessed").ToString();
                                break;
                            case (int)ShippingTabsEnum.ContainerInspected:
                                e.Row.Cells[0].Attributes.Add("class", "bggreen");
                                e.Row.Cells[1].Attributes.Add("class", "bggreen");
                                e.Row.Cells[2].Attributes.Add("class", "bggreen");
                                e.Row.Cells[3].Attributes.Add("class", "bgred");
                                lblQADocsUploaded.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[4].Attributes.Add("class", "bgred");
                                lblExportDocsUploaded.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[5].Attributes.Add("class", "bgred");
                                lblLoadingPlanCompleted.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[6].Attributes.Add("class", "bgred");
                                lblPhotographsuploaded.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[7].Attributes.Add("class", "bgred");
                                lblSDgenerated.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[8].Attributes.Add("class", "bgred");
                                lblContainerReleased.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[9].Attributes.Add("class", "bgred");
                                lblBL.Text = GetLocalResourceObject("NotProcessed").ToString();
                                break;
                            case (int)ShippingTabsEnum.QADocsUploaded:
                                e.Row.Cells[0].Attributes.Add("class", "bggreen");
                                e.Row.Cells[1].Attributes.Add("class", "bggreen");
                                e.Row.Cells[2].Attributes.Add("class", "bggreen");
                                e.Row.Cells[3].Attributes.Add("class", "bggreen");
                                e.Row.Cells[4].Attributes.Add("class", "bgred");
                                lblExportDocsUploaded.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[5].Attributes.Add("class", "bgred");
                                lblLoadingPlanCompleted.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[6].Attributes.Add("class", "bgred");
                                lblPhotographsuploaded.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[7].Attributes.Add("class", "bgred");
                                lblSDgenerated.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[8].Attributes.Add("class", "bgred");
                                lblContainerReleased.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[9].Attributes.Add("class", "bgred");
                                lblBL.Text = GetLocalResourceObject("NotProcessed").ToString();
                                break;
                            case (int)ShippingTabsEnum.ExportDocsUploaded:
                                e.Row.Cells[0].Attributes.Add("class", "bggreen");
                                e.Row.Cells[1].Attributes.Add("class", "bggreen");
                                e.Row.Cells[2].Attributes.Add("class", "bggreen");
                                e.Row.Cells[3].Attributes.Add("class", "bggreen");
                                e.Row.Cells[4].Attributes.Add("class", "bggreen");
                                e.Row.Cells[5].Attributes.Add("class", "bgred");
                                lblLoadingPlanCompleted.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[6].Attributes.Add("class", "bgred");
                                lblPhotographsuploaded.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[7].Attributes.Add("class", "bgred");
                                lblSDgenerated.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[8].Attributes.Add("class", "bgred");
                                lblContainerReleased.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[9].Attributes.Add("class", "bgred");
                                lblBL.Text = GetLocalResourceObject("NotProcessed").ToString();
                                break;
                            case (int)ShippingTabsEnum.LoadingPlanCompleted:
                                e.Row.Cells[0].Attributes.Add("class", "bggreen");
                                e.Row.Cells[1].Attributes.Add("class", "bggreen");
                                e.Row.Cells[2].Attributes.Add("class", "bggreen");
                                e.Row.Cells[3].Attributes.Add("class", "bggreen");
                                e.Row.Cells[4].Attributes.Add("class", "bggreen");
                                e.Row.Cells[5].Attributes.Add("class", "bggreen");
                                e.Row.Cells[6].Attributes.Add("class", "bgred");
                                lblPhotographsuploaded.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[7].Attributes.Add("class", "bgred");
                                lblSDgenerated.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[8].Attributes.Add("class", "bgred");
                                lblContainerReleased.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[9].Attributes.Add("class", "bgred");
                                lblBL.Text = GetLocalResourceObject("NotProcessed").ToString();
                                break;
                            case (int)ShippingTabsEnum.PhotographsUploaded:
                                e.Row.Cells[0].Attributes.Add("class", "bggreen");
                                e.Row.Cells[1].Attributes.Add("class", "bggreen");
                                e.Row.Cells[2].Attributes.Add("class", "bggreen");
                                e.Row.Cells[3].Attributes.Add("class", "bggreen");
                                e.Row.Cells[4].Attributes.Add("class", "bggreen");
                                e.Row.Cells[5].Attributes.Add("class", "bggreen");
                                e.Row.Cells[6].Attributes.Add("class", "bggreen");
                                e.Row.Cells[7].Attributes.Add("class", "bgred");
                                lblSDgenerated.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[8].Attributes.Add("class", "bgred");
                                lblContainerReleased.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[9].Attributes.Add("class", "bgred");
                                lblBL.Text = GetLocalResourceObject("NotProcessed").ToString();
                                break;
                            case (int)ShippingTabsEnum.DeliveryOrderCompleted:
                                e.Row.Cells[0].Attributes.Add("class", "bggreen");
                                e.Row.Cells[1].Attributes.Add("class", "bggreen");
                                e.Row.Cells[2].Attributes.Add("class", "bggreen");
                                e.Row.Cells[3].Attributes.Add("class", "bggreen");
                                e.Row.Cells[4].Attributes.Add("class", "bggreen");
                                e.Row.Cells[5].Attributes.Add("class", "bggreen");
                                e.Row.Cells[6].Attributes.Add("class", "bggreen");
                                e.Row.Cells[7].Attributes.Add("class", "bggreen");
                                e.Row.Cells[8].Attributes.Add("class", "bgred");
                                lblContainerReleased.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[9].Attributes.Add("class", "bgred");
                                lblBL.Text = GetLocalResourceObject("NotProcessed").ToString();
                                break;
                            case (int)ShippingTabsEnum.ContainerReleased:
                                e.Row.Cells[0].Attributes.Add("class", "bggreen");
                                e.Row.Cells[1].Attributes.Add("class", "bggreen");
                                e.Row.Cells[2].Attributes.Add("class", "bggreen");
                                e.Row.Cells[3].Attributes.Add("class", "bggreen");
                                e.Row.Cells[4].Attributes.Add("class", "bggreen");
                                e.Row.Cells[5].Attributes.Add("class", "bggreen");
                                e.Row.Cells[6].Attributes.Add("class", "bggreen");
                                e.Row.Cells[7].Attributes.Add("class", "bggreen");
                                e.Row.Cells[8].Attributes.Add("class", "bggreen");
                                e.Row.Cells[9].Attributes.Add("class", "bgred");
                                lblBL.Text = GetLocalResourceObject("NotProcessed").ToString();
                                break;
                            case (int)ShippingTabsEnum.BillofLoading:
                                e.Row.Cells[0].Attributes.Add("class", "bggreen");
                                e.Row.Cells[1].Attributes.Add("class", "bggreen");
                                e.Row.Cells[2].Attributes.Add("class", "bggreen");
                                e.Row.Cells[3].Attributes.Add("class", "bggreen");
                                e.Row.Cells[4].Attributes.Add("class", "bggreen");
                                e.Row.Cells[5].Attributes.Add("class", "bggreen");
                                e.Row.Cells[6].Attributes.Add("class", "bggreen");
                                e.Row.Cells[7].Attributes.Add("class", "bggreen");
                                e.Row.Cells[8].Attributes.Add("class", "bggreen");
                                e.Row.Cells[9].Attributes.Add("class", "bggreen");
                                break;
                            default:
                                e.Row.Cells[0].Attributes.Add("class", "bgred");
                                lblPaymentDone.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[1].Attributes.Add("class", "bgred");
                                lblContainerConditionChecked.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[2].Attributes.Add("class", "bgred");
                                lblContainerInspected.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[3].Attributes.Add("class", "bgred");
                                lblQADocsUploaded.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[4].Attributes.Add("class", "bgred");
                                lblExportDocsUploaded.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[5].Attributes.Add("class", "bgred");
                                lblLoadingPlanCompleted.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[6].Attributes.Add("class", "bgred");
                                lblPhotographsuploaded.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[7].Attributes.Add("class", "bgred");
                                lblSDgenerated.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[8].Attributes.Add("class", "bgred");
                                lblContainerReleased.Text = GetLocalResourceObject("NotProcessed").ToString();
                                e.Row.Cells[9].Attributes.Add("class", "bgred");
                                lblBL.Text = GetLocalResourceObject("NotProcessed").ToString();
                                break;
                        }

                        //lblPaymentDone.ToolTip = lblPaymentDone.Text;
                        //lblPaymentDone.Text = CommonFunctions.GetShortString(lblPaymentDone.Text, 6);

                        //lblContainerConditionChecked.ToolTip = lblContainerConditionChecked.Text;
                        //lblContainerConditionChecked.Text = CommonFunctions.GetShortString(lblContainerConditionChecked.Text, 6);

                        //lblContainerInspected.ToolTip = lblContainerInspected.Text;
                        //lblContainerInspected.Text = CommonFunctions.GetShortString(lblContainerInspected.Text, 6);

                        //lblQADocsUploaded.ToolTip = lblQADocsUploaded.Text;
                        //lblQADocsUploaded.Text = CommonFunctions.GetShortString(lblQADocsUploaded.Text, 6);

                        //lblExportDocsUploaded.ToolTip = lblExportDocsUploaded.Text;
                        //lblExportDocsUploaded.Text = CommonFunctions.GetShortString(lblExportDocsUploaded.Text, 6);

                        //lblLoadingPlanCompleted.ToolTip = lblLoadingPlanCompleted.Text;
                        //lblLoadingPlanCompleted.Text = CommonFunctions.GetShortString(lblLoadingPlanCompleted.Text, 6);

                        //lblPhotographsuploaded.ToolTip =lblPhotographsuploaded.Text;
                        //lblPhotographsuploaded.Text = CommonFunctions.GetShortString(lblPhotographsuploaded.Text, 6);

                        //lblSDgenerated.ToolTip =lblSDgenerated.Text;
                        //lblSDgenerated.Text = CommonFunctions.GetShortString(lblSDgenerated.Text, 6);

                        //lblContainerReleased.ToolTip = lblContainerReleased.Text;
                        //lblContainerReleased.Text = CommonFunctions.GetShortString(lblContainerReleased.Text, 6);


                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        protected void grdStatement_OnPaging(object sender, GridViewPageEventArgs e)
        {
            grdStatement.PageIndex = e.NewPageIndex;
            GetFieldValues(ControlsEnum.ORDERTRACKER);
            SetFieldValues(ControlsEnum.ORDERTRACKER);
        }

        protected void grdPacking_OnPaging(object sender, GridViewPageEventArgs e)
        {
            grdPacking.PageIndex = e.NewPageIndex;
            GetFieldValues(ControlsEnum.CUSTOMERSORDERS);
            SetFieldValues(ControlsEnum.CUSTOMERSORDERS);
        }

        protected void grdPaymentTerms_OnPaging(object sender, GridViewPageEventArgs e)
        {
            grdPaymentTerms.PageIndex = e.NewPageIndex;
            GetFieldValues(ControlsEnum.CUSTOMERSORDERS);
            SetFieldValues(ControlsEnum.CUSTOMERSORDERS);
        }

        protected void grdInvoice_OnPaging(object sender, GridViewPageEventArgs e)
        {
            grdInvoice.PageIndex = e.NewPageIndex;
            GetFieldValues(ControlsEnum.CUSTOMERSORDERS);
            SetFieldValues(ControlsEnum.CUSTOMERSORDERS);
        }

        #endregion


        #region PageActionHandler

        public void RefreshAllGrids()
        {           
            grdStatement.DataSource = null;
            grdStatement.DataBind();

            TextBox txtFromDate = (TextBox)Page.Master.FindControl("txtFromDate");
            TextBox txtToDate = (TextBox)Page.Master.FindControl("txtToDate");

            HiddenField hdfFromDate = (HiddenField)Page.Master.FindControl("hdfFromDate");
            HiddenField hdfToDate = (HiddenField)Page.Master.FindControl("hdfToDate");

            if (!string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
            {
                FromDate = Convert.ToDateTime(txtFromDate.Text);
                ToDate = Convert.ToDateTime(txtToDate.Text);
                hdfFromDate.Value = Convert.ToDateTime(txtFromDate.Text).ToString();
                hdfToDate.Value = Convert.ToDateTime(txtToDate.Text).ToString();
            }


            TextBox txtCustomer = (TextBox)Page.Master.FindControl("txtCustomerSearch");
            HiddenField hdfCustomer = (HiddenField)Page.Master.FindControl("hdfCustomerSearch");
            //For Resolving Bug ID:  15761
            if (string.IsNullOrEmpty(txtCustomer.Text) || txtCustomer.Text == "Select/Type")
            {
                hdfCustomer.Value = "0";
                HeaderCustomerPK = 0;
            }
            //end
            if (hdfCustomer != null)
            {
               
                if (hdfCustomer.Value != string.Empty)
                {
                    HeaderCustomerPK = Convert.ToInt32(hdfCustomer.Value);
                    SohPK = "0";
                    CustomerID = hdfCustomer.Value;
                    GetFieldValues(ControlsEnum.ORDERTRACKER);
                    SetFieldValues(ControlsEnum.ORDERTRACKER);
                }
            }



            GetFieldValues(ControlsEnum.CUSTOMERS);
            SetFieldValues(ControlsEnum.CUSTOMERS);


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
            if (txtCustomer.Text == "Select/Type" || txtCustomer.Text.Trim()==string.Empty)
            {
                lblOutstanding.Text = "Current Outstanding : " + BaseCurrency + " " + String.Format("{0:c}", 0);
            }

        }

        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                if (!IsPostBack)
                {
                    grdStatement.DataSource = null;
                    grdStatement.DataBind();

                    setDate();

                    GetFieldValues(ControlsEnum.USERCUSTOMER);
                    SetFieldValues(ControlsEnum.USERCUSTOMER);

                    TextBox txtCustomer = (TextBox)Page.Master.FindControl("txtCustomerSearch");
                    HiddenField hdfCustomer = (HiddenField)Page.Master.FindControl("hdfCustomerSearch");
                    if (hdfCustomer != null)
                    {
                        HeaderCustomerPK = hdfCustomer.Value != string.Empty ? Convert.ToInt32(hdfCustomer.Value) : 0;
                    }

                    GetFieldValues(ControlsEnum.QUESTIONNAIRE);
                    SetFieldValues(ControlsEnum.QUESTIONNAIRE);

                    GetFieldValues(ControlsEnum.CUSTOMERS);
                    SetFieldValues(ControlsEnum.CUSTOMERS);


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
                    GetFieldValues(ControlsEnum.BASECURRENCY);
                    string currency = BaseCurrency;
                    decimal outstatnding = 0;
                    lblOutstanding.Text = "Current Outstanding : " + currency + " " + String.Format("{0:c}", outstatnding);

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
            SetStatementGridBalance();
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_InitComponents", "$(document).ready(function(){InitComponents();});", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideCustomersOrdersd", "ShowHideCustomersOrders(1);", true);
            if (grdPaymentTerms.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHidePTermd", "ShowHidePTerm(0);", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHidePTermd", "ShowHidePTerm(1);", true);
            }

            if (grdInvoice.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideInvoiced", "ShowHideInvoice(0);", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideInvoiced", "ShowHideInvoice(1);", true);
            }
            if (grdStatement.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideStatementd", "ShowHideStatement(0);", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideStatementd", "ShowHideStatement(1);", true);
            }
            if (EntryStatus == EntryStatus.VIEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideAllocatedd", "ShowHideAllocated(1);", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideAllocatedd", "ShowHideAllocated(0);", true);
            }
            TextBox txtCustomer = (TextBox)Page.Master.FindControl("txtCustomerSearch");
            if (txtCustomer.Text != string.Empty && txtCustomer.Text != Resources.Messages.AutoDefaultValue)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideQUESTIONNAIREd", "ShowHideQUESTIONNAIRE(1);", true);
                trvCustomers.ExpandAll();
            }
            else
            {
                if (ddlQuestionnaire.SelectedValue == ERP.Utilities.CommonConstants.SELECTVAL)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideQUESTIONNAIREd", "ShowHideQUESTIONNAIRE(0);", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideQUESTIONNAIRE", "ShowHideQUESTIONNAIRE(1);", true);
                }
            }


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
                        GetFieldValues(ControlsEnum.CUSTOMERSORDERS);
                        SetFieldValues(ControlsEnum.CUSTOMERSORDERS);
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


        #region Helper methods


        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private object SetUIValuesToObject(ControlsEnum controlType)
        {

            try
            {
                Object retObject;
                retObject = null;
                BusinessObject.User currentUser;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                List<SaleOrderPK> saleOrderPks;
                switch (controlType)
                {
                    #region ShippingList
                    case ControlsEnum.CUSTOMERSORDERS:
                        ShippingPlanOrderObj = new ShippingPlanOrder();
                        saleOrderPks = new List<SaleOrderPK>();
                        // foreach (long pk in SelectedSOListForSP)
                        //{
                        SaleOrderPK item = new SaleOrderPK();
                        item.SOH_PK = Convert.ToInt64(SohPK);
                        saleOrderPks.Add(item);
                        // }
                        ShippingPlanOrderObj.Active = 2;
                        ShippingPlanOrderObj.BizPk = currentUser.SBUID;
                        ShippingPlanOrderObj.SaleOrderPKs = saleOrderPks;
                        retObject = ShippingPlanOrderObj;
                        break;
                    #endregion

                    #region QUESTIONNAIRE
                    case ControlsEnum.QUESTIONNAIREDATA:
                        QustionNaireBOObj = new QustionNaireBO();
                       // QustionNaireBOObj.FromDate = FromDate.ToString();
                       // QustionNaireBOObj.ToDate = ToDate.ToString();
                        QustionNaireBOObj.BizUnit = currentUser.SBUID;
                        QustionNaireBOObj.UserPK = currentUser.PKUser;
                        QustionNaireBOObj.Dept = currentUser.CurrentDeptPK;
                        QustionNaireBOObj.CusPK = HeaderCustomerPK == 0 ? Convert.ToInt32(CustomerID) : HeaderCustomerPK;
                        QustionNaireBOObj.QstPK = Convert.ToInt32(ddlQuestionnaire.SelectedValue);
                        retObject = QustionNaireBOObj;
                        break;
                    #endregion
                }

                return retObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

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
                        BindGrid(ControlsEnum.DBMSG);
                        Session.Remove(ERP.Utilities.SessionStrings.AllocatedOrderInfo);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAssetType", "ShowContainerDiv('#DiverrorMessages','" + strHdrMsg + "','500','300','" + Resources.PageURL.SaleOrder + "');", true);
                        //   Response.Redirect(Resources.PageURL.SaleOrder);
                    }
                    else
                    {
                        strHdrMsg = Resources.Messages.ErrorMessage;
                        BindGrid(ControlsEnum.DBMSG);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAssetType", "ShowContainerDiv('#DiverrorMessages','" + strHdrMsg + "','500','300');", true);

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
                //lblDespatchedDetails.Text = string.Empty;
                ////lblPlannedDetails.Text = string.Empty;
                //lblAllocatedDetails.Text = string.Empty;
                //lblProductName.Text = string.Empty;
                ////lblPlannedDetails.Text = string.Empty;
                //lblPackedDetail.Text = string.Empty;

                List<Customers> customerList = new List<Customers>();

                int i = -1;
                dsCustomers.Tables[0].DefaultView.Sort = "SOH_CUSTOMER_TEXT asc";
                DataView dv = dsCustomers.Tables[0].DefaultView;
                foreach (DataRow row in dv.ToTable().Rows)
                {
                    Customers customers = new Customers();
                    customers.CustomerID = Convert.ToInt32(row[5]);
                    customers.CustomerCode = row[6] as String;
                    customers.CustomerName = row[7] as String;

                    bool has = customerList.Any(cus => cus.CustomerID == Convert.ToInt32(row[5]));
                    if (has)
                    {

                        customerList[i].OrdersList.Add(new Orders(
                              Convert.ToInt32(row[0]),
                              row[2] as String,
                              Convert.ToInt32(row[4])
                              ));
                    }
                    else
                    {
                        customerList.Add(customers);
                        i++;
                        customerList[i].OrdersList.Add(new Orders(
                           Convert.ToInt32(row[0]),
                           row[2] as String,
                           Convert.ToInt32(row[4])
                           ));
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
                parentNode = new TreeNode(customer.CustomerCode,
                             customer.CustomerID.ToString());
                parentNode.ToolTip = customer.CustomerName;
                parentNode.NavigateUrl = "javascript:void(0)";
                foreach (Orders order in customer.OrdersList)
                {
                    if (order.OrderStatus == 2)
                    {
                        TreeNode childNode = new TreeNode(order.OrderName,
                                             order.OrderID.ToString());
                        childNode.ToolTip = order.OrderName;
                        parentNode.ChildNodes.Add(childNode);
                    }
                }

                parentNode.Collapse();

                // Show all checkboxes
                //trvUtilityTree.ShowCheckBoxes = TreeNodeTypes.All;
                trvCustomers.Nodes.Add(parentNode);
            }
        }

        private void ConfigurationSettings()
        {
            IsExportExcel = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsExportExcel")));
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

            //lblDespatchedDetails.Text = string.Empty;
            ////lblPlannedDetails.Text = string.Empty;
            //lblAllocatedDetails.Text = string.Empty;
            //lblProductName.Text = string.Empty;
            ////lblPlannedDetails.Text = string.Empty;
            //lblPackedDetail.Text = string.Empty;

            SohPK = trvCustomers.SelectedNode.Value.ToString();
            CustomerID = trvCustomers.SelectedNode.Parent.Value.ToString();
            GetFieldValues(ControlsEnum.CUSTOMERSORDERS);
            SetFieldValues(ControlsEnum.CUSTOMERSORDERS);
            GetFieldValues(ControlsEnum.ORDERTRACKER);
            SetFieldValues(ControlsEnum.ORDERTRACKER);
        }





        #endregion


        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.QUESTIONNAIRE:
                    ddlQuestionnaire.Items.Clear();
                    if (dsQuestionnair != null && dsQuestionnair.Tables[0].Rows.Count > 0)
                    {
                        ddlQuestionnaire.DataSource = dsQuestionnair.Tables[0];
                        ddlQuestionnaire.DataTextField = "QST_QUESTION";
                        ddlQuestionnaire.DataValueField = "QST_PK";
                        ddlQuestionnaire.DataBind();
                    }
                    ddlQuestionnaire.Items.Insert(0, new ListItem(GetLocalResourceObject("SelectQuery").ToString(), ERP.Utilities.CommonConstants.SELECTVAL));
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum type)
        {
            switch (type)
            {

                case ControlsEnum.DBMSG:
                    grdError.DataSource = dsDBMessgaes;
                    grdError.DataBind();
                    break;
                case ControlsEnum.CUSTOMERSORDERS:
                    #region finished Goods
                    try
                    {

                        if (dsPageData != null)
                        {

                            uclPagingCustomersOrders.TotalPages = TotalPagesCustomersOrders;
                            PageIndexCustomersOrders = PageIndexCustomersOrders == null ? ERP.Utilities.CommonConstants.SELECT_VALUE_ONE : PageIndexCustomersOrders;
                            uclPagingCustomersOrders.CurrentPage = Convert.ToInt32(PageIndexCustomersOrders);

                            grdCustomersOrders.DataSource = dsPageData.Tables[1];
                            grdCustomersOrders.DataBind();

                            if (dsAllocation != null)
                            {
                                grdPacking.DataSource = dsAllocation.Tables[0];
                                grdPacking.DataBind();
                            }

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
                case ControlsEnum.STATUSDETAILS:
                    #region Status Details
                    try
                    {
                        if (dsSPStatus != null)
                        {
                            grdAllocated.DataSource = dsSPStatus.Tables[0];
                            grdAllocated.DataBind();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    #endregion
                    break;
                case ControlsEnum.ORDERTRACKER:
                    if (dsOrderTracker != null)
                    {
                        grdPaymentTerms.DataSource = dsOrderTracker.Tables[0];
                        grdPaymentTerms.DataBind();

                        if (dsOrderTracker.Tables[0].Rows.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHidePTermd", "ShowHidePTerm(0);", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHidePTermd", "ShowHidePTerm(1);", true);
                        }

                        grdInvoice.DataSource = dsOrderTracker.Tables[1];
                        grdInvoice.DataBind();
                        if (dsOrderTracker.Tables[1].Rows.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideInvoiced", "ShowHideInvoice(0);", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideInvoiced", "ShowHideInvoice(1);", true);
                        }

                        DataColumn dc = new DataColumn("Balance");
                        dc.DataType = System.Type.GetType("System.Decimal");
                        dsOrderTracker.Tables[2].Columns.Add(dc);
                        double runningBalance = 0;
                        //For TRXAmt
                        DataColumn dc1 = new DataColumn("TRXAmt");
                        dc1.DataType = System.Type.GetType("System.String");
                        dsOrderTracker.Tables[2].Columns.Add(dc1);
                        string TRXAmt = "";

                        foreach (DataRow dr in dsOrderTracker.Tables[2].Rows)
                        {
                            double debit = 0;
                            double credit = 0;

                            double.TryParse(Convert.ToString(dr["Debit"]), out debit);
                            double.TryParse(Convert.ToString(dr["Credit"]), out credit);

                            runningBalance = ((runningBalance + debit) - credit);
                            dr["Balance"] = runningBalance;
                            //For TRXAmt
                            double DEBIT_TC = 0;
                            double CREDIT_TC = 0;

                            double.TryParse(Convert.ToString(dr["DEBIT_TC"]), out DEBIT_TC);
                            double.TryParse(Convert.ToString(dr["CREDIT_TC"]), out CREDIT_TC);

                            TRXAmt = dr["BASE_CUR_TEXT"] + " " + (String.Format("{0:c}", DEBIT_TC + CREDIT_TC)).ToString();
                            dr["TRXAmt"] = TRXAmt;
                        }
                        dsOrderTracker.Tables[2].AcceptChanges();
                        //if (Convert.ToDecimal(outstatnding) < 0)
                        //{
                        //    lblOutstanding.Text = "Current Outstanding: " + currency + " " + String.Format("{0:c}", -1 * outstatnding);
                        //}
                        //else
                        //{
                        //    lblOutstanding.Text = "Current Outstanding: " + currency + " " + String.Format("{0:c}", outstatnding);
                        //}
                        GetFieldValues(ControlsEnum.BASECURRENCY);
                        if (Convert.ToInt32(CustomerID) > 0)
                        {
                            grdStatement.DataSource = dsOrderTracker.Tables[2];
                            grdStatement.DataBind();
                        }
                        //lblCurrency.Text = BaseCurrency;
                        //if (dsOrderTracker != null && dsOrderTracker.Tables.Count > 0 && dsOrderTracker.Tables[2].Rows.Count > 1)
                        //{
                        //    lblCurrency.Text = dsOrderTracker.Tables[2].Rows[1]["BASE_CUR_TEXT"].ToString();
                        //}
                        //else
                        //{
                        //    lblCurrency.Text = string.Empty;
                        //}
                        string currency = BaseCurrency;
                        decimal outstatnding = 0;
                        if (dsOrderTracker.Tables[2] != null && dsOrderTracker.Tables[2].Rows.Count > 0)
                        {
                            // currency = dsOrderTracker.Tables[2].Rows[dsOrderTracker.Tables[2].Rows.Count - 1]["base_cur_text"].ToString();
                            outstatnding = Convert.ToDecimal(dsOrderTracker.Tables[2].Rows[dsOrderTracker.Tables[2].Rows.Count - 1]["Balance"].ToString());

                            if (Convert.ToDecimal(outstatnding) < 0)
                            {
                                lblOutstanding.Text = "Current Outstanding : " + currency + " " + String.Format("{0:c}", -1 * outstatnding);
                            }
                            else
                            {
                                lblOutstanding.Text = "Current Outstanding : " + currency + " " + String.Format("{0:c}", outstatnding);
                            }
                        }
                        else
                        {
                            lblOutstanding.Text = string.Empty;
                        }
                        if (dsOrderTracker.Tables[2].Rows.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideStatementd", "ShowHideStatement(0);", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideStatementd", "ShowHideStatement(1);", true);
                        }
                    }
                    else
                    {
                        lblOutstanding.Text = string.Empty;
                    }
                    break;
                #region BASECURRENCY

                case ControlsEnum.BASECURRENCY:
                    //Gets Base Currency
                    DataTable dtcurr = getcurrency();
                    if (dtcurr != null && dtcurr.Rows.Count > 0)
                    {
                        CurrencyMstService CurrencyMstServiceClient = new CurrencyMstService();
                        BaseCurrency = (CurrencyMstServiceClient.GetCurrencyCodeName(Convert.ToInt16(dtcurr.Rows[0]["ACF_DATA"].ToString()))).Split('-')[0].Trim(); ;

                    }
                    break;
                #endregion
            }
        }

        private DataTable getcurrency()
        {
            DataTable dt=null;
            if (CurrentUser != null)
            {
                dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BASE CURRENCY", string.Empty, CurrentUser.SBUID);
            }
            return dt;

        }

        private void ClearGrid()
        {
            grdPacking.DataSource = null;
            grdPacking.DataBind();

            //grdStatement.DataSource = null;
            //grdStatement.DataBind();

            grdAllocated.DataSource = null;
            grdAllocated.DataBind();

            grdPaymentTerms.DataSource = null;
            grdPaymentTerms.DataBind();

            grdCustomersOrders.DataSource = null;
            grdCustomersOrders.DataBind();

            grdInvoice.DataSource = null;
            grdInvoice.DataBind();

        }

        protected void grdPacking_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer && dsAllocation != null)
            {
                if (dsAllocation.Tables[0].Rows.Count > 0)
                {

                    ((Label)e.Row.FindControl("lblTotalToDispatch")).Text = ((Label)e.Row.FindControl("lblTotalToDispatch")).ToolTip = Convert.ToDecimal(dsAllocation.Tables[0].Rows[0][Resources.DataFieldRes.SodQtyDispatchedTotal]).ToString("n0");
                    ((Label)e.Row.FindControl("lblAllocatedTotal")).Text = ((Label)e.Row.FindControl("lblAllocatedTotal")).ToolTip = Convert.ToDecimal(dsAllocation.Tables[0].Rows[0][Resources.DataFieldRes.SodQtyAllocatedTotal]).ToString("n0");
                    //((Label)e.Row.FindControl("lblPlannedTotal")).Text = Convert.ToDecimal(dsAllocation.Tables[0].Rows[0][Resources.DataFieldRes.SodQtyPlannedTotal]).ToString("n0");
                    //((Label)e.Row.FindControl("lblBlaToplanTotal")).Text = Convert.ToDecimal(dsAllocation.Tables[0].Rows[0][Resources.DataFieldRes.SodBalToPlanTotal]).ToString("n0");
                    ((Label)e.Row.FindControl("lblTotalOrderQty")).Text = ((Label)e.Row.FindControl("lblTotalOrderQty")).ToolTip = Convert.ToDecimal(dsAllocation.Tables[0].Rows[0][Resources.DataFieldRes.SodQtyTotal]).ToString("n0");
                    ((Label)e.Row.FindControl("lblTotalPacked")).Text = ((Label)e.Row.FindControl("lblTotalPacked")).ToolTip = Convert.ToDecimal(dsAllocation.Tables[0].Rows[0][Resources.DataFieldRes.SodPackedTotal]).ToString("n0");


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
                    if (HeaderCustomerPK != 0)
                        objCustomers.CustomerPK = HeaderCustomerPK.ToString();
                    //if (txtCustomer.Text != string.Empty && txtCustomer.Text != GetLocalResourceObject("EnterCustomer").ToString())
                    //    objCustomers.CustomerName = txtCustomer.Text;

                    objCustomers.BizUnit = CurrentUser.SBUID;
                    objCustomers.FromDate = FromDate;
                    objCustomers.ToDate = ToDate;

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

        private void SetStatementGridBalance()
        {
            string currency = string.Empty;
            decimal outstatnding = 0;
            foreach (GridViewRow dr in grdStatement.Rows)
            {
                HiddenField hdfStatementBalance = (HiddenField)dr.FindControl("hdfStatementBalance");
                Label lblStatementBalance = (Label)dr.FindControl("lblStatementBalance");
                outstatnding = Convert.ToDecimal(hdfStatementBalance.Value);
                HiddenField hdfBasecurrency = (HiddenField)dr.FindControl("hdfStatementBasecurrency");
                //lblStatementBalance.Text = String.Format("{0:c}", Convert.ToDecimal(hdfStatementBalance.Value)) + " " + (Convert.ToDecimal(hdfStatementBalance.Value) < 0 ? "Cr" : "Dr");

                if (Convert.ToDecimal(hdfStatementBalance.Value) == 0)
                {
                    lblStatementBalance.Text = lblStatementBalance.ToolTip = String.Format("{0:c}", Convert.ToDecimal(hdfStatementBalance.Value));
                }
                else if (Convert.ToDecimal(hdfStatementBalance.Value) < 0)
                {
                    lblStatementBalance.Text = lblStatementBalance.ToolTip = String.Format("{0:c}", -1 * Convert.ToDecimal(hdfStatementBalance.Value)) + " " + (Convert.ToDecimal(hdfStatementBalance.Value) < 0 ? "Cr" : "Dr");
                }
                else
                {   
                    lblStatementBalance.Text = lblStatementBalance.ToolTip = String.Format("{0:c}", Convert.ToDecimal(hdfStatementBalance.Value)) + " " + (Convert.ToDecimal(hdfStatementBalance.Value) < 0 ? "Cr" : "Dr");
                }
                currency = hdfBasecurrency.Value;
            }
            //if (Convert.ToDecimal(outstatnding) < 0)
            //{
            //    lblOutstanding.Text = "Current Outstanding: " + currency + " " + String.Format("{0:c}", -1 * outstatnding);
            //}
            //else
            //{
            //    lblOutstanding.Text = "Current Outstanding: " + currency + " " + String.Format("{0:c}", outstatnding);
            //}
            GetFieldValues(ControlsEnum.BASECURRENCY);
            currency = BaseCurrency;
            if (currency != string.Empty)
            {
                ltStatementCurrency.Text = string.Format(GetLocalResourceObject("StatementCurrency").ToString(), currency);
            }
            else
            {
                ltStatementCurrency.Text = string.Empty;
            }
        }

        private void setDate()
        {
            TextBox txtFromDate = (TextBox)Page.Master.FindControl("txtFromDate");
            TextBox txtToDate = (TextBox)Page.Master.FindControl("txtToDate");

            HiddenField hdfFromDate = (HiddenField)Page.Master.FindControl("hdfFromDate");
            HiddenField hdfToDate = (HiddenField)Page.Master.FindControl("hdfToDate");

            txtToDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormat);
            DateTime dtFromDt = DateTime.Now.AddMonths(-6).AddDays(-((DateTime.Now.AddMonths(-1).Day) - 1));
            txtFromDate.Text = dtFromDt.ToString(Resources.ErpRes.DateFormat);

            if (!string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
            {
                FromDate = Convert.ToDateTime(txtFromDate.Text);
                ToDate = Convert.ToDateTime(txtToDate.Text);
                hdfFromDate.Value = Convert.ToDateTime(txtFromDate.Text).ToString();
                hdfToDate.Value = Convert.ToDateTime(txtToDate.Text).ToString();
            }
        }

        #endregion

        #region Action methods


        public DataSet FetchDbValues(int val, string[] XML)
        {
            return BusinessLogic.Sales.OrderTrackerBL.GetCustomers_Orders(val, P_XML);

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
            SHOWDETAILS,
            SODETAILS,
            VIEW,
            SELECTINDEXCHANGED,
            PRINT,
            SCPRINT

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
            ORDERS,
            DBMSG,
            SHIPPINGPLANLIST,
            SPDEATILS,
            STATUSDETAILS,
            QUESTIONNAIRE,
            QUESTIONNAIREDATA,
            USERCUSTOMER,
            ORDERTRACKER,
            BASECURRENCY
        }
        #endregion
    }
}