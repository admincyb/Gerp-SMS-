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
using System.Linq;
using BusinessObject.CommonManagement;
using System.Data;
using BusinessObject.Shipping;
using BusinessLogic.Sales;

namespace ERPSMS_v01.Shipping
{
    public partial class SaleOrderList : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties

        #region Properties
        /// <summary>
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
        /// Sal eOrder Type
        /// </summary>
        private string SaleOrderType
        {
            get
            {
                return this.ViewState[ViewstateStrings.SaleOrderType] == null ? "" : this.ViewState[ViewstateStrings.SaleOrderType].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.SaleOrderType] = value;
            }
        }
        /// <summary>
        /// Customer Pk FOR DO
        /// </summary>
        private int CustomerIDForDO
        {
            get
            {
                return this.ViewState[ViewstateStrings.CustomerIDForDO] == null ? 0 : (int)this.ViewState[ViewstateStrings.CustomerIDForDO];
            }
            set
            {
                this.ViewState[ViewstateStrings.CustomerIDForDO] = value;
            }
        }

        /// <summary>
        /// Customer Pk FOR DO
        /// </summary>
        private int CustomerIDForAdvInv
        {
            get
            {
                return this.ViewState[ViewstateStrings.CustomerIDForAdvInv] == null ? 0 : (int)this.ViewState[ViewstateStrings.CustomerIDForAdvInv];
            }
            set
            {
                this.ViewState[ViewstateStrings.CustomerIDForAdvInv] = value;
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
        /// DO ID
        /// </summary>
        private int DoId
        {
            get
            {
                return this.ViewState[ViewstateStrings.DoId] == null ? 0 : (int)this.ViewState[ViewstateStrings.DoId];
            }
            set
            {
                this.ViewState[ViewstateStrings.DoId] = value;
            }
        }
        /// <summary>
        /// Sale Order ID For DO
        /// </summary>
        private int SoIdForDO
        {
            get
            {
                return this.ViewState[ViewstateStrings.SoIdForDO] == null ? 0 : (int)this.ViewState[ViewstateStrings.SoIdForDO];
            }
            set
            {
                this.ViewState[ViewstateStrings.SoIdForDO] = value;
            }
        }
        /// <summary>
        /// Sale Order ID For Adv Invoicing
        /// </summary>
        private int SoIdForAdvInv
        {
            get
            {
                return this.ViewState[ViewstateStrings.SoIdForAdvInv] == null ? 0 : (int)this.ViewState[ViewstateStrings.SoIdForAdvInv];
            }
            set
            {
                this.ViewState[ViewstateStrings.SoIdForAdvInv] = value;
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
        /// To maintain count of selected pos for Invoicing
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
        /// To maintain count of selected pos for DO
        /// </summary>
        private int SelectedSosCountForDO
        {
            get
            {
                return this.ViewState[ViewstateStrings.SelectedSosCountForDO] == null ? 0 : (int)this.ViewState[ViewstateStrings.SelectedSosCountForDO];

            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedSosCountForDO] = value;
            }
        }
        /// To maintain count of selected pos for Advance Invoicing
        /// </summary>
        private int SelectedSosCountForAdvInv
        {
            get
            {
                return this.ViewState[ViewstateStrings.SelectedSosCountForAdvInv] == null ? 0 : (int)this.ViewState[ViewstateStrings.SelectedSosCountForAdvInv];

            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedSosCountForAdvInv] = value;
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
        /// <summary>
        /// To maintain keep selected pos For Advance Invoicing
        /// </summary>
        private List<long> SelectedSosForAdvInv
        {
            get
            {
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedSosForAdvInv];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedSosForAdvInv] = value;
            }

        }
        /// <summary>
        /// To keep selected pos For DO
        /// </summary>
        private List<long> SelectedSosForSP
        {
            get
            {
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedSosForSP];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedSosForSP] = value;
            }

        }
        /// <summary>
        /// Selected Customers for Invoicing
        /// </summary>
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
        /// <summary>
        /// Selected Customers for Advance Invoicing
        /// </summary>
        private List<long> SelectedCustomersForAdvInv
        {
            get
            {
                return (List<long>)this.ViewState[ViewstateStrings.SelectedCustomersForAdvInv];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedCustomersForAdvInv] = value;
            }

        }
        /// <summary>
        /// Selected Customers for Advance Invoicing
        /// </summary>
        private List<long> SelectedCustomersForDO
        {
            get
            {
                return (List<long>)this.ViewState[ViewstateStrings.SelectedCustomersForDO];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedCustomersForDO] = value;
            }

        }
        /// <summary>
        /// Selected Customers Type
        /// </summary>
        private List<string> SelectedCustomersType
        {
            get
            {
                return (List<string>)this.ViewState[ViewstateStrings.SelectedCustomersType];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedCustomersType] = value;
            }

        }
        /// <summary>
        /// Currency
        /// </summary>
        private int Currency
        {
            get
            {
                return this.ViewState[ViewstateStrings.Currency] == null ? 0 : (int)this.ViewState[ViewstateStrings.Currency];
            }
            set
            {
                this.ViewState[ViewstateStrings.Currency] = value;
            }
        }
        /// <summary>
        /// To Keep Currency For DO
        /// </summary>
        private int CurrencyForDO
        {
            get
            {
                return this.ViewState[ViewstateStrings.CurrencyForDO] == null ? 0 : (int)this.ViewState[ViewstateStrings.CurrencyForDO];
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrencyForDO] = value;
            }
        }
        /// <summary>
        /// To Keep Currency For Adv Invoice
        /// </summary>
        private int CurrencyForAdvInv
        {
            get
            {
                return this.ViewState[ViewstateStrings.CurrencyForAdvInv] == null ? 0 : (int)this.ViewState[ViewstateStrings.CurrencyForAdvInv];
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrencyForAdvInv] = value;
            }
        }
        /// <summary>
        /// To keep selected Currency
        /// </summary>
        private List<long> SelectedCurrency
        {
            get
            {
                return (List<long>)this.ViewState[ViewstateStrings.SelectedCurrency];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedCurrency] = value;
            }

        }
        /// <summary>
        /// To keep selected Currency For DO
        /// </summary>
        private List<long> SelectedCurrencyForDO
        {
            get
            {
                return (List<long>)this.ViewState[ViewstateStrings.SelectedCurrencyForDO];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedCurrencyForDO] = value;
            }

        }
        /// <summary>
        /// To keep selected Currency For DO
        /// </summary>
        private List<long> SelectedCurrencyForAdvInv
        {
            get
            {
                return (List<long>)this.ViewState[ViewstateStrings.SelectedCurrencyForAdvInv];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedCurrencyForAdvInv] = value;
            }

        }
        #endregion

        #region Variables
        // Indicates the state as well as action
        private ActionsEnum commonActions;

        //page related Entity Object
        //Object for ServiceUtility(paging,sorting,fillter etc)
        private ServiceUtility serviceUtilityObj;
        //object for Salecontact Header
        private SAL_ORDER_HDR objSalesOrderHeader;
        //object for Salecontact Deatils
        private SAL_ORDER_HDR salOrderHdrObj;
        //object for GON details
        private SAL_DESPATCH_DTL salDespatchObj;
        //Object for configuration master
        private ADM_CONFIG_MST admConfigMstObj;
        //Object for customer deatisl
        private SPCRM_CUSTOMER_USER_GET_Result SPCRM_CUSTOMER_USER_GET_ResultObj;

        private List<ADM_COMPANY_MST> admCompanyMstList;
        private ADM_COMPANY_MST admCompanyMstObj;

        //List for binding details to controls   
        //List for GON Header
        private List<SAL_DESPATCH_HDR> salDespatchHdrList;
        //List for GON Details
        private List<SAL_DESPATCH_DTL> salDespatchDetailList;
        //List for SalesContract Header
        private List<SAL_ORDER_HDR> salesOrderHeaderList;
        //List for SalesContract Details
        private List<SAL_ORDER_DTL> salesOrderDetailsList;
        //List for Selected Sales Contracts PKs
        private List<long> SelectedSOList;
        //List for Selected Sales Contracts PKs (Advance Invoice)
        private List<long> SelectedSOListForAdvInv;
        //List for Selected Sales Contracts PKs (Shipping Plan)
        private List<long> SelectedSOListForSP;
        //List for Selected Sales Contracts Types
        private List<string> SelectedOrderTypeList;
        //List for Selected customers
        private List<long> SelectedCustomerList;
        //List for Selected customers (Advance Invoice)
        private List<long> SelectedCustomerListForAdvInv;
        //List for Selected customers (GON)
        private List<long> SelectedCustomerListForDO;
        //List for Sales Contract Types From Configuration table
        private List<ADM_CONFIG_MST> admConfigMstList;
        //List for Customer Deatils
        private List<SPCRM_CUSTOMER_USER_GET_Result> SPCRM_CUSTOMER_USER_GET_ResultList;
        //List for WorkFlow Status From Configuration table
        private List<ADM_CONFIG_MST> workflowStatusList;

        //dataset for binding details to controls  
        private DataSet dsShippingList;
        private DataTable dtCompany;
        ShippingPlanOrder ShippingPlanOrderObj;
        private DataSet dsList;
        //Object for user deatils
        private BusinessObject.User currentUser;
        #endregion

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
                    ConfigurationSettings();
                    //clear the selected customer,SC's and Customer Types
                    SelectedCustomers = null;
                    SelectedSos = null;
                    SelectedSosCount = 0;
                    SelectedSosForSP = null;
                    SelectedCustomersType = null;

                    //Used for Integration purpose
                    FillProcessID();

                    //txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    //hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    //txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    //hdfToDate.Value = DateTime.Now.ToString();

                    //selected salecontact count for shipping
                    if (SelectedSosCountForDO != 0)
                        btnPickForDO.Text = GetLocalResourceObject("PickSoForSP").ToString() + "(" + SelectedSosCountForDO.ToString() + ")";

                    //If the user login as cutomer then set the customer details
                    GetFieldValues(ControlsEnum.CUSTOMERPK);
                    if (SPCRM_CUSTOMER_USER_GET_ResultList != null && SPCRM_CUSTOMER_USER_GET_ResultList.Count > 0)
                    {
                        hdfCustomerID.Value = SPCRM_CUSTOMER_USER_GET_ResultList[0].CUS_PK.ToString();
                        txtCustomer.Text = SPCRM_CUSTOMER_USER_GET_ResultList[0].CUS_NAME;
                        Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = hdfCustomerID.Value;
                        Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = txtCustomer.Text;
                    }

                    // for saleContracts Listing
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                    EntryStatus = EntryStatus.LISTMODE;

                }
                if (SelectedSos != null)
                {
                    SelectedSosCount = SelectedSos.Count;
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
            //Service for Salecontracts
            SaleOrderService salesOrderServiceClient;
            salesOrderServiceClient = null;

            //Service for GON Header
            SalDespatchHdrService salDespatchHdrServiceClient;
            salDespatchHdrServiceClient = null;

            //Service for GON Details
            SalDespatchDtlService salDespatchDtlServiceClient;
            salDespatchDtlServiceClient = null;
            AdmCompanyMstService admCompanyMstServiceClient;
            CommonService CommonServiceClient;
            CommonServiceClient = null;

            int Status;
            //set the user details
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            string getxml;

            try
            {
                switch (type)
                {
                    #region SEARCH
                    //For Advance Search
                    case ControlsEnum.SEARCH:
                        //initialize the SaleContract service
                        salesOrderServiceClient = new SaleOrderService();
                        salesOrderServiceClient = CommonFunctions.InitiateClient(salesOrderServiceClient);
                        objSalesOrderHeader = new SAL_ORDER_HDR();

                        //initialize the Service Utility for paging ,sorting ,fillter etc
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdSoList.PageSize;
                        serviceUtilityObj.SortBy = Resources.DataFieldRes.SODate;
                        serviceUtilityObj.ThenBy = Resources.DataFieldRes.SONo;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;

                        //fillter based on active sale contracts
                        //objSalesOrderHeader.SOH_ACTIVE = 1;
                       // objSalesOrderHeader.SOH_COMPANY = Convert.ToInt32(ddlCompanyFilter.SelectedValue);

                        //filter based on bizunit
                        //objSalesOrderHeader.SOH_BIZUNIT = currentUser.SBUID;

                        //fillter based on customer
                        //if (hdfCustomerID.Value != "")
                        //{
                        //    objSalesOrderHeader.SOH_CUSTOMER = Convert.ToInt32(hdfCustomerID.Value);
                        //}
                        if (hdfCustomerID.Value != null && hdfCustomerID.Value != "0" && hdfCustomerID.Value != "")
                        {
                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = hdfCustomerID.Value;
                            Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = txtCustomer.Text;
                        }

                        //fillter based on SC No
                        //if (hdfSoPK.Value != "")
                        //    objSalesOrderHeader.SOH_PK = Convert.ToInt32(hdfSoPK.Value);
                        //if (!string.IsNullOrEmpty(txtFromDate.Text))
                        //    serviceUtilityObj.FilterDate = DateTime.Parse(txtFromDate.Text);
                        //if (!string.IsNullOrEmpty(txtToDate.Text))
                        //    serviceUtilityObj.FilterToDate = DateTime.Parse(txtToDate.Text);

                        //fillter based on status
                        //Status = Convert.ToInt32(ddlStatus.SelectedValue);

                        //get the SC List
                        //salesOrderHeaderList = salesOrderServiceClient.GetSaleOrderHeaderSales(objSalesOrderHeader, serviceUtilityObj, ApplicationSubType.INVOICE, Status, PageType.SHIPPING);

                        //set the total pages count
                    


                        //Convert to SP 
                        ERP.Utilities.FilterUtility objPageFilter = new FilterUtility();
                        BusinessObject.Sales.SalesOrderBO objFields = new BusinessObject.Sales.SalesOrderBO();


                        objPageFilter.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        objPageFilter.PageSize = grdSoList.PageSize;
                        objPageFilter.SortBy = Resources.DataFieldRes.SODate;
                        objPageFilter.ThenBy = Resources.DataFieldRes.SONo;
                        objPageFilter.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;


                        objFields.Active = "1";
                        objFields.SOH_COMPANY = Convert.ToInt32(ddlCompanyFilter.SelectedValue);
                        objPageFilter.BizUnit = currentUser.SBUID;
                        if (hdfCustomerID.Value != "")
                            objFields.CustomerPK = hdfCustomerID.Value;

                        //fillter based on SC No
                        if (hdfSoPK.Value != "")
                            objFields.PK = hdfSoPK.Value;
                        if (!string.IsNullOrEmpty(txtFromDate.Text))
                            objPageFilter.FilterDate = DateTime.Parse(txtFromDate.Text);
                        if (!string.IsNullOrEmpty(txtToDate.Text))
                            objPageFilter.FilterToDate = DateTime.Parse(txtToDate.Text);
                        //fillter based on status
                        objFields.SOH_STATUS = Convert.ToInt32(ddlStatus.SelectedValue);
                        objFields.UserPK = currentUser.PKUser.ToString();
                        dsList = SaleOrderBL.GetSaleOrderList(objPageFilter, objFields);

                        if (dsList != null)
                        {
                            serviceUtilityObj.TotalRecords = dsList.Tables[0].Rows.Count > 0 ? Convert.ToInt32(dsList.Tables[0].Rows[0]["TOTAL_ROW_COUNT"].ToString()) : 0;
                            TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                        (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                        (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        }

                        break;
                    #endregion

                    #region DEFAULT
                    // SaleContract Listing (LIST Mode)
                    case ControlsEnum.DEFAULT:


                        //initialize the SaleContract service
                        salesOrderServiceClient = new SaleOrderService();
                        salesOrderServiceClient = CommonFunctions.InitiateClient(salesOrderServiceClient);
                        objSalesOrderHeader = new SAL_ORDER_HDR();

                        //initialize the Service Utility for paging ,sorting ,fillter etc
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdSoList.PageSize;
                        serviceUtilityObj.SortBy = Resources.DataFieldRes.SODate;
                        serviceUtilityObj.ThenBy = Resources.DataFieldRes.SONo;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;

                        objPageFilter = new FilterUtility();
                        objFields = new BusinessObject.Sales.SalesOrderBO();

                        //fillter based on active sale contracts
                        ////objSalesOrderHeader.SOH_ACTIVE = 1;
                        ////objSalesOrderHeader.SOH_COMPANY = Convert.ToInt32(ddlCompanyFilter.SelectedValue);
                        //filter based on bizunit
                        ////objSalesOrderHeader.SOH_BIZUNIT = currentUser.SBUID;

                        //fillter based on customer
                        ////if (hdfCustomerID.Value != "")
                            ////objSalesOrderHeader.SOH_CUSTOMER = Convert.ToInt32(hdfCustomerID.Value);

                        //fillter based on SC No
                        ////if (hdfSoPK.Value != "")
                        ////    objSalesOrderHeader.SOH_PK = Convert.ToInt32(hdfSoPK.Value);
                        ////if (!string.IsNullOrEmpty(txtFromDate.Text))
                        ////    serviceUtilityObj.FilterDate = DateTime.Parse(txtFromDate.Text);
                        ////if (!string.IsNullOrEmpty(txtToDate.Text))
                        ////    serviceUtilityObj.FilterToDate = DateTime.Parse(txtToDate.Text);
                        //fillter based on status
                        ////Status = Convert.ToInt32(ddlStatus.SelectedValue);

                        //get the SC List
                        // 
                        ////salesOrderHeaderList = salesOrderServiceClient.GetSaleOrderHeaderSales(objSalesOrderHeader, serviceUtilityObj, ApplicationSubType.INVOICE, Status, PageType.SHIPPING);

                        //set the total pages count
                       // TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 : (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) : (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;

                        //Convert to SP 
                        //ERP.Utilities.FilterUtility objPageFilter=new FilterUtility();
                        //BusinessObject.Sales.SalesOrderBO objFields = new BusinessObject.Sales.SalesOrderBO();

                        //Convert to SP 
                        

                        objPageFilter.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        objPageFilter.PageSize = grdSoList.PageSize;
                        objPageFilter.SortBy = Resources.DataFieldRes.SODate;
                        objPageFilter.ThenBy = Resources.DataFieldRes.SONo;
                        objPageFilter.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;

                        objFields.Active = "1";
                        objFields.SOH_COMPANY = Convert.ToInt32(ddlCompanyFilter.SelectedValue);
                        objPageFilter.BizUnit = currentUser.SBUID;
                        if (hdfCustomerID.Value != "")
                            objFields.CustomerPK = hdfCustomerID.Value;

                        //fillter based on SC No
                        if (hdfSoPK.Value != "")
                            objFields.PK = hdfSoPK.Value;
                        if (!string.IsNullOrEmpty(txtFromDate.Text))
                            objPageFilter.FilterDate = DateTime.Parse(txtFromDate.Text);
                        if (!string.IsNullOrEmpty(txtToDate.Text))
                            objPageFilter.FilterToDate = DateTime.Parse(txtToDate.Text);
                        //fillter based on status
                        objFields.SOH_STATUS = Convert.ToInt32(ddlStatus.SelectedValue);
                        objFields.UserPK = currentUser.PKUser.ToString();
                        dsList = SaleOrderBL.GetSaleOrderList(objPageFilter, objFields);

                        if (dsList != null)
                        {
                            serviceUtilityObj.TotalRecords = dsList.Tables[0].Rows.Count > 0 ? Convert.ToInt32(dsList.Tables[0].Rows[0]["TOTAL_ROW_COUNT"].ToString()) : 0;
                            TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                        (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                        (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        }

                        break;
                    #endregion

                    #region SCDEATILS
                    //For SaleContract Details 
                    case ControlsEnum.SODETAILS:
                        //initialize the SaleContract service
                        salesOrderServiceClient = new SaleOrderService();
                        salesOrderServiceClient = CommonFunctions.InitiateClient(salesOrderServiceClient);
                        serviceUtilityObj = new ServiceUtility();

                        //get the SC Details
                        salesOrderDetailsList = salesOrderServiceClient.GetSaleOrderDetails(SoId, serviceUtilityObj);

                        break;
                    #endregion

                    #region GONLIST
                    //For GON Header List
                    case ControlsEnum.SHOWSOHDR:
                        //initialize the GON Header service
                        salDespatchHdrServiceClient = new SalDespatchHdrService();
                        salDespatchHdrServiceClient = CommonFunctions.InitiateClient(salDespatchHdrServiceClient);

                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdDOHdr.PageSize;
                        serviceUtilityObj.SortBy = Resources.DataFieldRes.DeliveryOrderDate;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;

                        //Get the GON Header List Based on active and SC PK
                        salOrderHdrObj = new SAL_ORDER_HDR();
                        salOrderHdrObj.SOH_ACTIVE = 1;
                        salOrderHdrObj.SOH_PK = SoId;
                        salDespatchHdrList = salDespatchHdrServiceClient.GetDespatchHdr(salOrderHdrObj, serviceUtilityObj);

                        break;
                    #endregion

                    #region GONDETAILS
                    //For GON Details 
                    case ControlsEnum.SHOWSODTL:
                        salDespatchDtlServiceClient = new SalDespatchDtlService();
                        salDespatchDtlServiceClient = CommonFunctions.InitiateClient(salDespatchDtlServiceClient);
                        serviceUtilityObj = new ServiceUtility();

                        //get the GON Details based on SC PK and GON Header PK
                        salDespatchObj = new SAL_DESPATCH_DTL();
                        salDespatchObj.DPD_SALE_ORDER = SoId;
                        salDespatchObj.DPD_DESPATCH_HDR = DoId;
                        salDespatchDetailList = salDespatchDtlServiceClient.GetDespatchDtl(salDespatchObj, serviceUtilityObj);
                        break;
                    #endregion

                    #region SC_TYPE
                    //For SaleContacts Types
                    case ControlsEnum.TYPECATEGORY:
                        CommonServiceClient = new CommonService();
                        admConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONFIG_MST>();

                        //Get the SC Types from configuration table based on active and Type='SO TYPE'
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admConfigMstObj.CFG_TYPE = GetLocalResourceObject("AccountType").ToString();
                        admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    #endregion

                    #region CUSTOMERPK
                    //For Customer Details
                    case ControlsEnum.CUSTOMERPK:
                        CommonServiceClient = new CommonService();
                        SPCRM_CUSTOMER_USER_GET_ResultObj = ERP.Utilities.CommonFunctions.Initilize<SPCRM_CUSTOMER_USER_GET_Result>();
                        SPCRM_CUSTOMER_USER_GET_ResultList = CommonServiceClient.GetCustomerDetails(currentUser.PKUser, currentUser.SBUID);
                        break;
                    #endregion

                    #region SHIPPINGPLAN
                    case ControlsEnum.SHIPPINGPLANLIST:
                        ShippingPlanOrderObj = (ShippingPlanOrder)SetUIValuesToObject(type);
                        getxml = CommonFunctions.XmlSerialize<ShippingPlanOrder>(ShippingPlanOrderObj);
                        dsShippingList = BusinessLogic.Shipping.ShippingPlanBL.GetShippingOrderList(getxml);
                        break;
                    #endregion

                    #region FILLWORKFLOWSTATUS
                    case ControlsEnum.FILLWORKFLOWSTATUS:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);

                        //Get the Workflow status from configuration table based on active and type='APPLICATION STATUS'
                        admConfigMstObj = new ADM_CONFIG_MST();
                        admConfigMstObj.CFG_PK = 0;
                        admConfigMstObj.CFG_TYPE = GetLocalResourceObject("APPLICATION_STATUS").ToString();
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        workflowStatusList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    #endregion

                    #region Company
                    case ControlsEnum.COMPANY:
                        //gets Company List
                        admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
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
                objSalesOrderHeader = null;
                serviceUtilityObj = null;
                salesOrderServiceClient = null;
                CommonServiceClient = null;
                salDespatchDtlServiceClient = null;
                salDespatchHdrServiceClient = null;
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
                    #region SCLIST
                    case ControlsEnum.SEARCH:
                    case ControlsEnum.DEFAULT:
                        BindGrid(controlType);
                        break;
                    #endregion

                    #region GONLIST
                    case ControlsEnum.SHOWSOHDR:
                        BindGrid(ControlsEnum.SHOWSOHDR);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        BindDropDownList(ControlsEnum.COMPANY);
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
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            GridViewRow gvr;
            GridView grd;
            ExtGridView egrd;
            string arg;
            string soPK;
            string doPK;
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
                #region extra grid ondemand data population
                case ActionsEnum.SODETAILS:
                    //Get the salecontacts details
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
                            GetFieldValues(ControlsEnum.SHIPPINGPLANLIST);
                        }
                        grd.Visible = true;
                        if (salesOrderDetailsList != null && salesOrderDetailsList.Count > 0)
                        {
                            grd.DataSource = salesOrderDetailsList;
                            grd.DataBind();
                            SetplandQty(grd);
                        }
                        (gvr.FindControl("hdfIsExpandedOrders") as HiddenField).Value = "1";

                        //SetResetColour
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);

                    }
                    break;
                case ActionsEnum.DODETAILS:
                    EntryStatus = EntryStatus.VIEWMODE;
                    arg = ((Button)sender).CommandArgument;
                    gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                    if (gvr != null)
                    {
                        grd = gvr.FindControl("grdDODetails") as GridView;
                        if (string.IsNullOrEmpty(arg))
                        {
                            salDespatchDetailList = null;
                        }
                        else
                        {
                            DoId = Convert.ToInt32(arg);
                            GetFieldValues(ControlsEnum.SHOWSODTL);
                        }
                        grd.Visible = true;
                        if (salDespatchDetailList != null && salDespatchDetailList.Count > 0)
                        {
                            grd.DataSource = salDespatchDetailList;
                            grd.DataBind();
                        }
                        (gvr.FindControl("hdfIsExpandedDOItem") as HiddenField).Value = "1";

                        //SetResetColour
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                    }
                    break;


                #endregion

                #region Search
                case ActionsEnum.SEARCH:
                    PageIndex = "1";
                    GetFieldValues(ControlsEnum.SEARCH);
                    SetFieldValues(ControlsEnum.SEARCH);
                    break;
                #endregion

                #region reset form
                case ActionsEnum.CLEAR:
                    ResetForm();
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    break;
                #endregion

                #region PICK ORDERS
                case ActionsEnum.PICKFORINVOICING:
                case ActionsEnum.PICKFORADVANCEINVOICING:
                case ActionsEnum.PICKFORDO:
                    SetUIValuesToObject(commonActions);
                    //SetResetColour
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);

                    break;
                #endregion

                #region Show SC Popup
                case ActionsEnum.SHOWPOPUP:
                    int TrxSts = 0;
                    GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
                    TrxSts = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfTrxSts")).Value);
                    if (TrxSts != 2)
                    {
                        litErrorMsg.Text = GetLocalResourceObject("TrxSts_msg").ToString(); ;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        break;
                    }
                    else
                    {
                        soPK = ((LinkButton)sender).CommandArgument;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + soPK + "&APPTYPE=" + ApplicationType.IO + "&APPSUBTYPE=") + "');", true);
                    }
                    break;
                #endregion
                #region Show DO Popup
                case ActionsEnum.SHOW:
                    doPK = ((LinkButton)sender).CommandArgument;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + doPK + "&APPTYPE=" + ApplicationType.DO + "&APPSUBTYPE=6") + "');", true);
                    break;
                #endregion

                #region Tab navigation
                case ActionsEnum.DEFAULT:
                    Response.Redirect(Resources.PageURL.ShippingSoListing);
                    break;
                case ActionsEnum.SHIPPINGPLAN:
                    //if (SelectedSosForSP != null)
                    //{

                    Response.Redirect(Resources.PageURL.ShippingPlanCreate);
                    //}
                    //else
                    //{
                    //    litErrorMsg.Text = GetLocalResourceObject("Msg_SelectSP").ToString();
                    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                    //}
                    break;
                case ActionsEnum.RESET:
                    Session[ERP.Utilities.SessionStrings.SALEORDERPK] = null;
                    CustomerIDForAdvInv = 0;
                    SoIdForAdvInv = 0;
                    CurrencyForAdvInv = 0;
                    CustomerIDForDO = 0;
                    SoIdForDO = 0;
                    CurrencyForDO = 0;
                    SaleOrderType = string.Empty;
                    SelectedCurrencyForDO = new List<long>();
                    SelectedCustomersForDO = new List<long>();
                    //SelectedCustomersType = new List<string>();
                    //SelectedSosForSP = new List<long>();
                    SelectedCustomerListForDO = new List<long>();
                    SelectedSOListForSP = new List<long>();
                    SelectedSosCountForDO = 0;
                    SelectedOrderTypeList = new List<string>();
                    SelectedCurrencyForAdvInv = new List<long>();
                    SelectedCustomersForAdvInv = new List<long>();
                    SelectedSosForAdvInv = new List<long>();
                    SelectedCustomerListForAdvInv = new List<long>();
                    SelectedSOListForAdvInv = new List<long>();
                    SelectedSosCountForAdvInv = 0;
                    SelectedCustomers = null;
                    SelectedSos = null;
                    SelectedSosCount = 0;
                    SelectedSosForSP = null;
                    SelectedCustomersType = null;
                    btnPickForDO.Text = GetLocalResourceObject("PickSoForSP").ToString();
                    //Resetting Color
                    hdfSelectedItemPk.Value = "0";
                    hdfIscontYes.Value = "0";
                    break;
                #endregion

                #region SO Hdr
                case ActionsEnum.SHOWDETAILS:
                    gvr = ((RadioButton)sender).Parent.Parent as ExtGridViewRow;
                    SoId = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfSOID")).Value);

                    Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)gvr.FindControl("hdfCustomerPK")).Value;
                    Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)gvr.FindControl("lblCustomerName")).Text;

                    GetFieldValues(ControlsEnum.SHOWSOHDR);
                    SetFieldValues(ControlsEnum.SHOWSOHDR);
                    hdfIscontYes.Value = "0";
                    //SetResetColour
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
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
                if ((sender as GridView).ID == "grdSoList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblType = e.Row.FindControl("lblType") as Label;

                        //Get the SC Type( Eg: Domestic,Export)
                        GetFieldValues(ControlsEnum.TYPECATEGORY);

                        //Set the SC Type
                        if (admConfigMstList != null && admConfigMstList.Count > 0)
                        {
                            admConfigMstObj = admConfigMstList.SingleOrDefault(con => con.CFG_VALUE == Convert.ToInt32(lblType.Text));
                            if (admConfigMstObj != null)
                            {
                                lblType.Text = admConfigMstObj.CFG_DATA;
                                lblType.ToolTip = admConfigMstObj.CFG_DATA;
                            }
                            else
                            {
                                lblType.Text = "";
                                lblType.ToolTip = "";
                            }
                        }

                        Label lblPlantCode = e.Row.FindControl("lblPlantCode") as Label;
                        lblPlantCode.Visible = GetConfigData().IsMultiplePlant;

                        Button imgApproved = e.Row.FindControl("imgApproved") as Button;

                        HiddenField hdfApproved = e.Row.FindControl("hdfApproved") as HiddenField;

                        HiddenField hdfSOID = e.Row.FindControl("hdfSOID") as HiddenField;

                        //Label lblTotalCtn = e.Row.FindControl("lblTotalCtn") as Label;
                        //Label lblTotalPcs = e.Row.FindControl("lblTotalPcs") as Label;


                        short appstatus = Convert.ToInt16(hdfApproved.Value);
                        //Set the Total Cartons and Pcs
                        // Total Pcs= sum of SOD_QTY in SAL_ORDER_DTL table
                        ////// if (salesOrderHeaderList != null && salesOrderHeaderList.Count > 0)
                        ////// {
                        //////List<SAL_ORDER_DTL> lists = salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.ToList();
                        //////int pcs = 0, ctns = 0;
                        //////double totalpcs = 1;
                        //////double Qty = 0;
                        //////foreach (SAL_ORDER_DTL item in lists)
                        //////{
                        //////    totalpcs = item.ADM_PACK_SPEC_MST == null ? 1 : item.ADM_PACK_SPEC_MST.APS_TOTAL_PCS <= 0 ? 1 : item.ADM_PACK_SPEC_MST.APS_TOTAL_PCS;
                        //////    Qty = Qty + Math.Ceiling(item.SOD_QTY / totalpcs);
                        //////    ctns = ctns + Convert.ToInt32(item.SAL_SHIPPING_PLAN_DTL.Sum(c => c.SND_CTN_QTY));
                        //////    //pcs = pcs + Convert.ToInt32(lists[0].SAL_SHIPPING_PLAN_DTL.Sum(c => c.SND_PLAN_QTY));
                        //////}
                        //lblTotalCtn.Text = ctns.ToString();
                        ////lblTotalCtn.Text = Math.Ceiling(Qty).ToString();
                        //lblTotalPcs.Text = pcs.ToString();
                        //// lblTotalPcs.Text = salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.Sum(d => d.SOD_QTY).ToString();
                        ////lblTotalCtn.ToolTip = lblTotalCtn.Text;
                        ////  lblTotalPcs.ToolTip = lblTotalPcs.Text;

                        //Set the Status Icons
                        HiddenField hdfSodQty = e.Row.FindControl("hdfSodQty") as HiddenField;
                        HiddenField hdfSodQtyDispatched = e.Row.FindControl("hdfSodQtyDispatched") as HiddenField;
                        HiddenField hdfSodQtyReturned = e.Row.FindControl("hdfSodQtyReturned") as HiddenField;
                        HiddenField hdfSodQtyInvoiced = e.Row.FindControl("hdfSodQtyInvoiced") as HiddenField;

                        long SodQty = Convert.ToInt64(hdfSodQty.Value);
                        int SodQtyDispatched = Convert.ToInt32(hdfSodQtyDispatched.Value);
                        int SodQtyReturned = Convert.ToInt32(hdfSodQtyReturned.Value);
                        int SodQtyInvoiced = Convert.ToInt32(hdfSodQtyInvoiced.Value);

                        //if ((salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.All(d => d.SOD_QTY_DISPATCHED == 0)))
                        if (SodQtyDispatched == 0)
                        {
                            //notdispatched red
                            imgApproved.CssClass = GetLocalResourceObject("Pending").ToString();
                            imgApproved.ToolTip = GetLocalResourceObject("PendingText").ToString();
                        }
                        else

                            //if ((salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.All(d => d.SOD_BAL_TO_DISPATCH <= 0)))
                            //if ( (salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.All(d => d.SOD_QTY > d.SOD_QTY_DISPATCHED) )||(salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.All(i => i.SOD_QTY > i.SOD_QTY_INVOICED)))//Like in adv search listing
                            //if ((salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.All(d => d.SOD_QTY_INVOICED >= d.SOD_QTY)))
                            ////if ((salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.All(d => (d.SOD_QTY_INVOICED - d.SOD_QTY_RETURNED) >= d.SOD_QTY)))
                            if ((SodQtyInvoiced - SodQtyReturned) >= SodQty)
                            {
                                //Completed green
                                imgApproved.CssClass = GetLocalResourceObject("Completed").ToString();
                                imgApproved.ToolTip = GetLocalResourceObject("CompletedText").ToString();
                            }
                            else
                            {
                                //pending orange
                                imgApproved.CssClass = GetLocalResourceObject("Inprogress").ToString();
                                imgApproved.ToolTip = GetLocalResourceObject("InprogressText").ToString();
                            }
                        //if ((salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.All(d => d.SOD_QTY_PLANNED >= d.SOD_QTY_APPROVED)))
                        //{
                        //    imgApproved.CssClass = GetLocalResourceObject("Completed").ToString();
                        //    imgApproved.ToolTip = GetLocalResourceObject("CompletedText").ToString();
                        //}
                        //else
                        //    if ((salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.All(d => d.SOD_QTY_PLANNED == 0)))
                        //    {
                        //        imgApproved.CssClass = GetLocalResourceObject("Pending").ToString();
                        //        imgApproved.ToolTip = GetLocalResourceObject("PendingText").ToString();
                        //    }
                        //    else
                        //    {
                        //        imgApproved.CssClass = GetLocalResourceObject("Inprogress").ToString();
                        //        imgApproved.ToolTip = GetLocalResourceObject("InprogressText").ToString();
                        //    }
                        ////// }

                        //if (salesOrderHeaderList != null && salesOrderHeaderList.Count > 0)
                        //{
                        //    if (salesOrderHeaderList[e.Row.RowIndex].SOH_STATUS != 2)
                        //    {
                        //        imgApproved.CssClass = GetLocalResourceObject("draft").ToString();
                        //        if (workflowStatusList != null && workflowStatusList.Count > 0)
                        //            imgApproved.ToolTip = workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus) != null ?
                        //                workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus).CFG_DATA : string.Empty;
                        //    }
                        //    else if ((salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.Any(d => d.SOD_QTY_APPROVED > d.SOD_QTY_DISPATCHED)) ||
                        //        (salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.Any(i => i.SOD_QTY_APPROVED > i.SOD_QTY_INVOICED)))
                        //    {
                        //        imgApproved.CssClass = GetLocalResourceObject("pending").ToString();
                        //        imgApproved.ToolTip = workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus) != null ?
                        //              workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus).CFG_DATA : string.Empty;
                        //    }
                        //    else
                        //    {
                        //        imgApproved.CssClass = GetLocalResourceObject("Completed").ToString();
                        //        imgApproved.ToolTip = workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus) != null ?
                        //              workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus).CFG_DATA : string.Empty;
                        //    }

                        //    //if (salesOrderHeaderList[e.Row.RowIndex].SOH_STATUS==0)
                        //    //{
                        //    //    imgApproved.CssClass = GetLocalResourceObject("drafted_icon").ToString();
                        //    //    if (workflowStatusList != null && workflowStatusList.Count > 0)
                        //    //        imgApproved.ToolTip = workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus) != null ?
                        //    //            workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus).CFG_DATA : string.Empty;
                        //    //}
                        //    //else if ((salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.Any(d => d.SOD_QTY_APPROVED > d.SOD_QTY_DISPATCHED)) ||
                        //    //    (salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.Any(i => i.SOD_QTY_APPROVED > i.SOD_QTY_INVOICED)))
                        //    //{
                        //    //    imgApproved.CssClass = GetLocalResourceObject("pending").ToString();
                        //    //    imgApproved.ToolTip = workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus) != null ?
                        //    //          workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus).CFG_DATA : string.Empty;
                        //    //}
                        //    //else
                        //    //{
                        //    //    imgApproved.CssClass = GetLocalResourceObject("Completed").ToString();
                        //    //    imgApproved.ToolTip = workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus) != null ?
                        //    //          workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus).CFG_DATA : string.Empty;
                        //    //}
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
                if ((sender as GridView).ID == "grdDOHdr")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                    }
                }

                if (((GridView)sender).ID == "grdDODetails")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblBal = (e.Row.FindControl("lblBal") as Label);
                        lblBal.Text = String.Format("{0:c}", decimal.Parse(lblBal.Text));


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

        #endregion

        #endregion

        #region Helper Methods
        #region ConfigurationSettings
        /// <summary>
        /// Get Configuration Value
        /// </summary>
        private void ConfigurationSettings()
        {
            ddlCompanyFilter.Visible = GetConfigData().IsMultiplePlant;
            lblCompanyFilter.Visible = GetConfigData().IsMultiplePlant;
            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "CUSTOMER");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsSBUCustomer.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
            }
        }
        #endregion
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessID()
        {
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                PageProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                base.WkfPageUrl = path;
            }
        }
        /// <summary>
        /// Method to set Planned Qty
        /// </summary>
        /// <param name="grd"></param>
        private void SetplandQty(GridView grd)
        {
            int rowID = 0;
            Label lblProduct;
            Label lblplnQty;
            if (dsShippingList != null)
            {
                if (dsShippingList.Tables[0].Rows.Count == grd.Rows.Count)
                {
                    foreach (GridViewRow grdrow in grd.Rows)
                    {
                        lblProduct = (Label)grd.Rows[rowID].FindControl("lblProduct");
                        lblplnQty = (Label)grd.Rows[rowID].FindControl("lblplnQty");
                        string prdcode = CommonFunctions.GetShortString(dsShippingList.Tables[0].Rows[rowID][Resources.DataFieldRes.SPIGPLCode].ToString(), 13);
                        if (lblProduct.Text == prdcode)
                        {
                            decimal plnQty = 0;
                            if (dsShippingList.Tables[0].Rows[rowID][Resources.DataFieldRes.SPDeatilsPlanQty] != null && dsShippingList.Tables[0].Rows[rowID][Resources.DataFieldRes.SPDeatilsPlanQty].ToString() != string.Empty)
                            {
                                plnQty = Convert.ToDecimal(dsShippingList.Tables[0].Rows[rowID][Resources.DataFieldRes.SPDeatilsPlanQty].ToString());
                            }
                            lblplnQty.Text = String.Format("{0:n}", plnQty);
                            lblplnQty.ToolTip = lblplnQty.Text;
                        }
                        rowID++;
                    }
                }
            }
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
                int TrxSts = 0;
                bool bIsChecked = false;




                foreach (GridViewRow grdrow in grdSoList.Rows)
                {
                    RadioButton rbtn;
                    rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                    if (rbtn.Checked)
                    {
                        bIsChecked = true;
                        switch (mode)
                        {
                            case ActionsEnum.PICKFORINVOICING:
                                // Pick for Invoice
                                CustomerID = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value);
                                SoId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOID")).Value);
                                Currency = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOCurrency")).Value);
                                break;
                            case ActionsEnum.PICKFORDO:
                                // Pick for Shipping

                                TrxSts = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfTrxSts")).Value);
                                if (TrxSts == 104)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("ShortClosed_msg").ToString(); ;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    break;
                                }
                                else

                                    if (TrxSts != 2)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("TrxSts_msg").ToString(); ;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        break;
                                    }
                                    else
                                    {
                                        CustomerIDForDO = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value);
                                        SoIdForDO = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOID")).Value);
                                        CurrencyForDO = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOCurrency")).Value);
                                        SaleOrderType = ((Label)grdrow.FindControl("lblType")).Text;
                                        //SoId = SoIdForDO;
                                        //GetFieldValues(ControlsEnum.SHOWSOHDR);
                                        //&& salDespatchHdrList != null && salDespatchHdrList.Count > 0

                                        break;
                                    }
                            case ActionsEnum.PICKFORADVANCEINVOICING:
                                // Pick for Advance Invoice
                                CustomerIDForAdvInv = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value);
                                SoIdForAdvInv = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOID")).Value);
                                CurrencyForAdvInv = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOCurrency")).Value);
                                break;

                        }


                        break;
                    }
                }


                if (bIsChecked)
                    switch (mode)
                    {
                        #region Pick Invoicing
                        case ActionsEnum.PICKFORINVOICING:
                            if (!IsSameCurrency(SelectedCurrency, Currency))
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Currency").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                            else if (!IsSameCustomer(SelectedCustomers, CustomerID))
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

                                //btnPickForInvoice.Text = GetLocalResourceObject("PickSoForInvoicing").ToString() + "(" + SelectedSosCount.ToString() + ")";

                            }
                            else
                            {
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Item_Added;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                            }
                            break;
                        #endregion
                        #region Pick for DO
                        case ActionsEnum.PICKFORDO:
                            // check shipping plan is already created.
                            if ((hdfIscontYes.Value != "1") && BusinessLogic.Shipping.ShippingPlanBL.ShippingPlanAlreadyCreatedCheck(SoIdForDO))
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShippingAlreadyCreated", "$(document).ready(function(){ShippingPlanAlreadyCreated();});", true);
                                break;
                            }
                            // Allow SC for shipping only after the Internal order generated.
                            if (TrxSts != 2)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("TrxSts_msg").ToString(); ;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                break;
                            }

                            if (ValidateShipping())
                            {
                                //Add Customers
                                if (SelectedCustomersForDO != null)
                                {
                                    SelectedCustomerListForDO = SelectedCustomersForDO;
                                }
                                else
                                {
                                    SelectedCustomerListForDO = new List<long>();
                                }
                                SelectedCustomerListForDO.Add(CustomerIDForDO);
                                SelectedCustomersForDO = SelectedCustomerListForDO;
                                //Add Sos
                                if (SelectedSosForSP != null)
                                {
                                    SelectedSOListForSP = SelectedSosForSP;
                                }
                                else
                                {
                                    SelectedSOListForSP = new List<long>();
                                }
                                if (!SelectedSOListForSP.Contains(SoIdForDO))
                                {
                                    SelectedSOListForSP.Add(SoIdForDO);
                                    SelectedSosForSP = SelectedSOListForSP;
                                    SelectedSosCountForDO = SelectedSosForSP.Count;
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("SCAlreadySelected").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return null;
                                }

                                //add order type
                                if (SelectedCustomersType != null)
                                {
                                    SelectedOrderTypeList = SelectedCustomersType;
                                }
                                else
                                {
                                    SelectedOrderTypeList = new List<string>();
                                }
                                SelectedOrderTypeList.Add(SaleOrderType);
                                SelectedCustomersType = SelectedOrderTypeList;
                                btnPickForDO.Text = GetLocalResourceObject("PickSoForSP").ToString() + "(" + SelectedSosCountForDO.ToString() + ")";

                                //For Saving Selected Item PK
                                hdfSelectedItemPk.Value = hdfSelectedItemPk.Value + "," + SoIdForDO.ToString();

                                //For Setting/Resetting Colour of a selected InvoiceNo
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                                //End 

                                #region Old code

                                //// check same currency selected for shipping
                                //if (!IsSameCurrency(SelectedCurrencyForDO, CurrencyForDO))
                                //{
                                //    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Currency").ToString();
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                //}
                                //// check same customer selected for shipping
                                //else if (!IsSameCustomer(SelectedCustomersForDO, CustomerIDForDO))
                                //{
                                //    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Customer").ToString();
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                //}
                                //// check same Type selected for shipping
                                //else if (!IsSameType(SelectedCustomersType, SaleOrderType))
                                //{
                                //    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_CustomerType").ToString();
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                //}
                                //// check duplicate SC Pks
                                //else if (!IsExixtPk(SelectedSosForSP, SoIdForDO))
                                //{                               

                                //}
                                //else
                                //{
                                //    litErrorMsg.Text = Resources.ErrorMessages.Msg_Item_Added;
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                //}
                                #endregion
                            }
                            break;
                        #endregion
                        #region Pick for Advance Invoicing
                        case ActionsEnum.PICKFORADVANCEINVOICING:
                            if (!IsSameCurrency(SelectedCurrencyForAdvInv, CurrencyForAdvInv))
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Currency").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                            else if (!IsSameCustomer(SelectedCustomersForAdvInv, CustomerIDForAdvInv))
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Customer").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                            else if (!IsExixtPk(SelectedSosForAdvInv, SoIdForAdvInv))
                            {
                                //Add Customers
                                if (SelectedCustomersForAdvInv != null)
                                {
                                    SelectedCustomerListForAdvInv = SelectedCustomersForAdvInv;
                                }
                                else
                                {
                                    SelectedCustomerListForAdvInv = new List<long>();
                                }
                                SelectedCustomerListForAdvInv.Add(CustomerIDForAdvInv);
                                SelectedCustomersForAdvInv = SelectedCustomerListForAdvInv;

                                //Add Currencies
                                if (SelectedCurrencyForAdvInv != null)
                                {
                                    SelectedCurrencyForAdvInv = SelectedCurrencyForAdvInv;
                                }
                                else
                                {
                                    SelectedCurrencyForAdvInv = new List<long>();
                                }
                                SelectedCurrencyForAdvInv.Add(CurrencyForAdvInv);


                                //Add Sos
                                if (SelectedSosForAdvInv != null)
                                {
                                    SelectedSOListForAdvInv = SelectedSosForAdvInv;
                                }
                                else
                                {
                                    SelectedSOListForAdvInv = new List<long>();
                                }
                                SelectedSOListForAdvInv.Add(SoIdForAdvInv);
                                SelectedSosForAdvInv = SelectedSOListForAdvInv;
                                SelectedSosCountForAdvInv = SelectedSosForAdvInv.Count;

                                //btnPickForAdvInv.Text = GetLocalResourceObject("PickSoForAdvanceInvoicing").ToString() + "(" + SelectedSosCountForAdvInv.ToString() + ")";

                            }
                            else
                            {
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Item_Added;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                            }
                            break;
                        #endregion
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

        private bool ValidateShipping()
        {
            List<ShippingSO> objSOList = new List<ShippingSO>();
            ShippingHeaderSO objSOitem = new ShippingHeaderSO();
            objSOitem.SOList = new List<ShippingSO>();

            bool pick = true;
            int result = 0;
            ShippingSO objSO = new ShippingSO();
            if (Session[ERP.Utilities.SessionStrings.SelectedSosForSP] != null)
            {
                foreach (long pk in SelectedSosForSP)
                {
                    objSOList.Add(new ShippingSO { SOH_PK = (int)pk });
                }
            }
            if (objSOList.Where(t => t.SOH_PK == SoIdForDO).Count() == 0)
            {
                objSO.SOH_PK = SoIdForDO;
                objSOList.Add(objSO);
            }
            objSOitem.SOList = objSOList.Distinct().ToList();
            objSOitem.IS_DISCOUNT_EXIST = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "SCDiscountRestrictShp").ToString());
           
            string xmlDoc = CommonFunctions.XmlSerialize<ShippingHeaderSO>(objSOitem);
            result = BusinessLogic.Shipping.ShippingPlanBL.CheckforValidShippingSO(xmlDoc);
            if (result < 0)
            {
                if (result == -3) //NO SC SELECTED
                {
                    pick = false;
                    litErrorMsg.Text = GetLocalResourceObject("NoItemPickforShipping").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (result == -4) //DIFF CUSTOMER
                {
                    pick = false;
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Error_Customer").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (result == -5) //DIFF TYPE
                {
                    pick = false;
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Type").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (result == -6) //DIFF CURRENCY
                {
                    pick = false;
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Error_Currency").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (result == -7) //DIFF TAX / DISCOUNT
                {
                    pick = false;
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_DiffTaxType").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (result == -8) //CUSTOM TAX / DISCOUNT
                {
                    pick = false;
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_CustomTaxType").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (result == -9) //MULTIPLE TAX / DISCOUNT
                {
                    pick = false;
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_HeaderTaxType").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (result == -10) //DISCOUNT EXISTS
                {
                    pick = false;
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_DiscountExist").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else //Default 
                {
                    pick = false;
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
            }
            return pick;
        }


        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <param name="controlType">Save Action</param>
        /// <returns></returns>
        private object SetUIValuesToObject(ControlsEnum controlType)
        {

            try
            {
                Object retObject;
                retObject = null;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                List<SaleOrderPK> saleOrderPks;
                switch (controlType)
                {

                    #region ShippingList
                    case ControlsEnum.SHIPPINGPLANLIST:
                        ShippingPlanOrderObj = new ShippingPlanOrder();
                        saleOrderPks = new List<SaleOrderPK>();

                        SaleOrderPK item = new SaleOrderPK();
                        item.SOH_PK = SoId;
                        saleOrderPks.Add(item);

                        ShippingPlanOrderObj.Active = 2;
                        ShippingPlanOrderObj.BizPk = currentUser.SBUID;
                        ShippingPlanOrderObj.SaleOrderPKs = saleOrderPks;
                        retObject = ShippingPlanOrderObj;
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


        /// <summary>
        /// Set values to Session for handling edit and view or navigation
        /// </summary>
        private void SetUIEditView(ActionsEnum mode)
        {
            try
            {
                foreach (GridViewRow grdrow in grdSoList.Rows)
                {
                    RadioButton rbtn;
                    rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                    if (rbtn.Checked)
                    {
                        switch (mode)
                        {
                            case ActionsEnum.INVOICE:
                                Session[ERP.Utilities.SessionStrings.SALEORDERPK] = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOID")).Value);


                                break;
                        }

                    }
                }
                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.Invoicing), false);
                // if no items selected, Show Error Message
                //litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                //EntryStatus = EntryStatus.LISTMODE;
            }
            catch
            {
                throw;
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
                    case ControlsEnum.SEARCH:
                    case ControlsEnum.DEFAULT:
                        GetFieldValues(ControlsEnum.FILLWORKFLOWSTATUS);
                        uclPaging.TotalPages = TotalPages;
                        PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        grdDOHdr.DataSource = null;
                        grdDOHdr.DataBind();
                        // grdSoList.DataSource = salesOrderHeaderList;
                        grdSoList.DataSource = dsList;
                        grdSoList.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();

                        //SetResetColour
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);

                        break;
                    case ControlsEnum.SHOWSOHDR:
                        grdDOHdr.DataSource = salDespatchHdrList;
                        grdDOHdr.DataBind();

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
                    #region Company
                    case ControlsEnum.COMPANY:
                        ddlCompanyFilter.Items.Clear();
                        if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                        {
                            ddlCompanyFilter.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CMP_DISPLAY_CODE);
                            ddlCompanyFilter.DataTextField = Resources.DataFieldRes.CMP_DISPLAY_CODE;
                            ddlCompanyFilter.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                            ddlCompanyFilter.DataBind();
                        }
                        ddlCompanyFilter.Items.Insert(0, new ListItem(Resources.ErpRes.SelectAll, CommonConstants.SELECTVAL));
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
        /// Is Same Currency
        /// </summary>
        /// <param name="Currencies"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsSameCurrency(List<long> Currencies, long pk)
        {

            bool flag = true;
            if (Currencies != null)
                foreach (long ven in Currencies)
                    if (ven != pk)
                    {
                        flag = false;
                        break;
                    }
            return flag;
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
        /// Is Same Type
        /// </summary>
        /// <param name="SoType"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsSameType(List<string> SoType, string type)
        {

            bool flag = true;
            if (SoType != null)
                foreach (string cus in SoType)
                    if (cus != type)
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
            txtPONumber.Text = string.Empty;
            hdfSoPK.Value = "";
            hdfCustomerID.Value = "";
            txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
            //txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
            //hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
            //txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            //hdfToDate.Value = DateTime.Now.ToString();
            hdfIscontYes.Value = "0";
            ddlCompanyFilter.SelectedIndex = 0;
        }

        /// <summary>
        /// Method used to get the Item Spec Name
        /// </summary>
        protected string GetConstName(object invItem)
        {
            string constName = string.Empty;
            INV_ITEM_MST invItemObj = (INV_ITEM_MST)invItem;
            if (invItemObj != null)
            {
                constName = invItemObj.INV_ITEM_SPEC_DTL.Count() > 0 ?
                    HttpUtility.HtmlDecode(invItemObj.INV_ITEM_SPEC_DTL.First().ADM_CONST_MST16.CON_NAME) : string.Empty;
            }
            return constName;
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
            btnPickForDO.PreRender += new EventHandler(btnAction_PreRender);
            btnResetSelection.PreRender += new EventHandler(btnAction_PreRender);
            lbnSOListing.PreRender += new EventHandler(btnAction_PreRender);
            lnbShippingPlan.PreRender += new EventHandler(btnAction_PreRender);

            btnPickForDO.Load += new EventHandler(btnAction_Load);
            btnResetSelection.Load += new EventHandler(btnAction_Load);
            lbnSOListing.Load += new EventHandler(btnAction_Load);
            lnbShippingPlan.Load += new EventHandler(btnAction_Load);
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

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowFilter", "$(document).ready(function(){ShowFilter();});", true);
                    EntryStatus = EntryStatus.LISTMODE;
                }
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
            SODETAILS,
            SHOWSOHDR,
            SHOWSODTL,
            SHOWDETAILS,
            TYPECATEGORY,
            SEARCH,
            CUSTOMERPK,
            SHIPPINGPLANLIST,
            FILLWORKFLOWSTATUS,
            COMPANY
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

        /// <summary>
        /// Define Activity Status Enum
        /// </summary>
        public enum WkfStatusEnum
        {
            DRAFTED = 0,
            APPROVED = 2,
            REQUESTFORMOREINFO = 6,
            SUBMITWITHMOREINFO = 7,
            SUBMITTED = 1,
            REJECTED = 3,
            CLOSED = 5,
            SHORTCLOSED = 4
        }

        #endregion
    }
}