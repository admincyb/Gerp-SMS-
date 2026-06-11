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
using BusinessLogic.CommonManagement;
using BusinessObject;
using BusinessLogic.Sales;

namespace ERPSMS_v01.Sales
{
    public partial class SaleOrderList : ERP.Store.UI.WorkFlowBasePage
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
        private List<long> SelectedSosForDO
        {
            get
            {
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedSosForDO];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedSosForDO] = value;
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
        private List<decimal> SelectedSOsTax
        {
            get
            {
                return (List<decimal>)this.ViewState["SelectedSOsTax"];
            }
            set
            {
                this.ViewState["SelectedSOsTax"] = value;
            }

        }
        private decimal SOTax
        {
            get
            {
                return (decimal)this.ViewState["SOTax"];
            }
            set
            {
                this.ViewState["SOTax"] = value;
            }

        }
        #endregion


        // Indicates the state as well as action
        private ActionsEnum commonActions;
        private BusinessObject.User currentUser;
        //page related Entity Object
        private ServiceUtility serviceUtilityObj;
        private SAL_ORDER_HDR objSalesOrderHeader;
        private List<SAL_DESPATCH_HDR> salDespatchHdrList;
        private List<SAL_DESPATCH_DTL> salDespatchDetailList;
        private List<ADM_CONFIG_MST> workflowStatusList;
        private List<decimal> SelectedSOsTaxList;

        private SAL_ORDER_HDR salOrderHdrObj;
        private SAL_DESPATCH_DTL salDespatchObj;

        private List<SAL_ORDER_HDR> salesOrderHeaderList;
        private List<SAL_ORDER_DTL> salesOrderDetailsList;
        private List<long> SelectedSOList;
        private List<long> SelectedSOListForAdvInv;
        private List<long> SelectedSOListForDO;
        private List<string> SelectedOrderTypeList;

        private List<long> SelectedCustomerList;
        private List<long> SelectedCustomerListForAdvInv;
        private List<long> SelectedCustomerListForDO;

        private ADM_CONFIG_MST admConfigMstObj;
        private List<ADM_CONFIG_MST> admConfigMstList;
        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;

        private SPCRM_CUSTOMER_USER_GET_Result SPCRM_CUSTOMER_USER_GET_ResultObj;
        private List<SPCRM_CUSTOMER_USER_GET_Result> SPCRM_CUSTOMER_USER_GET_ResultList;
        DataTable dtSOData;
        private DataSet dsList;
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
                    ddlPlantName.Visible = GetConfigData().IsMultiplePlant;
                    lblPlantName.Visible = GetConfigData().IsMultiplePlant;
                    hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
                    SelectedCustomers = null;
                    SelectedSos = null;
                    SelectedSosCount = 0;
                    SelectedSosForDO = null;
                    SelectedCustomersType = null;
                    FillProcessID();
                    Session[ERP.Utilities.SessionStrings.SelectedSos] = null;
                    Session[ERP.Utilities.SessionStrings.SelectedSosForAdvInv] = null;

                    //txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    //hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    //txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    //hdfToDate.Value = DateTime.Now.ToString();
                    txtFromDate.Text = string.Empty;
                    hdfFromDate.Value = string.Empty;
                    txtToDate.Text = string.Empty;
                    hdfToDate.Value = string.Empty;
                    //Set Selected Orders Count 
                    // Need replace viewstate to session 
                    if (SelectedSosCountForAdvInv != 0)
                        btnPickForAdvInv.Text = GetLocalResourceObject("PickSoForAdvanceInvoicing").ToString() + "(" + SelectedSosCountForAdvInv.ToString() + ")";
                    if (SelectedSosCountForDO != 0)
                        btnPickForDO.Text = GetLocalResourceObject("PickSoForDO").ToString() + "(" + SelectedSosCountForDO.ToString() + ")";
                    //if (Session[ERP.Utilities.SessionStrings.SALEORDERPK] != null)
                    //    btnPickForInvoice.Text = GetLocalResourceObject("PickSoForInvoicing").ToString() + "(1)";
                    GetFieldValues(ControlsEnum.CUSTOMERPK);
                    if (SPCRM_CUSTOMER_USER_GET_ResultList != null && SPCRM_CUSTOMER_USER_GET_ResultList.Count > 0)
                    {
                        hdfCustomerID.Value = SPCRM_CUSTOMER_USER_GET_ResultList[0].CUS_PK.ToString();
                        txtCustomer.Text = SPCRM_CUSTOMER_USER_GET_ResultList[0].CUS_NAME;
                        Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = hdfCustomerID.Value;
                        Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = txtCustomer.Text;
                    }
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    GetFieldValues(ControlsEnum.SOTYPE);
                    SetFieldValues(ControlsEnum.SOTYPE);
                    GetFieldValues(ControlsEnum.COMPANYNAME);
                    SetFieldValues(ControlsEnum.COMPANYNAME);
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                    EntryStatus = EntryStatus.LISTMODE;
                    ConfigurationSettings();
                }
                //ddlPlantName.Enabled = GetConfigData().IsMultiplePlant ? false : true;
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
            AdmCompanyMstService admCompanyMstServiceClient;
            SaleOrderService salesOrderServiceClient;
            salesOrderServiceClient = null;
            SalDespatchHdrService salDespatchHdrServiceClient;
            salDespatchHdrServiceClient = null;
            SalDespatchDtlService salDespatchDtlServiceClient;
            salDespatchDtlServiceClient = null;
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            int Status;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            try
            {
                switch (type)
                {

                    case ControlsEnum.SEARCH:
                        salesOrderServiceClient = new SaleOrderService();
                        salesOrderServiceClient = CommonFunctions.InitiateClient(salesOrderServiceClient);
                        objSalesOrderHeader = new SAL_ORDER_HDR();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdSoList.PageSize;
                        serviceUtilityObj.SortBy = Resources.DataFieldRes.SODate;
                        serviceUtilityObj.ThenBy = Resources.DataFieldRes.SONo;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;

                         ERP.Utilities.FilterUtility objPageFilter = new FilterUtility();
                        BusinessObject.Sales.SalesOrderBO objFields = new BusinessObject.Sales.SalesOrderBO();

                        objPageFilter = new FilterUtility();
                        objFields = new BusinessObject.Sales.SalesOrderBO();
                        objPageFilter.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        objPageFilter.PageSize = grdSoList.PageSize;
                        objPageFilter.SortBy = Resources.DataFieldRes.SODate;
                        objPageFilter.ThenBy = Resources.DataFieldRes.SONo;
                        objPageFilter.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;



                        
                       // objSalesOrderHeader.SOH_BIZUNIT = currentUser.SBUID;
                       // objSalesOrderHeader.SOH_ACTIVE = 1;
                        //objSalesOrderHeader.SOH_PK = Convert.ToInt32(hdfSoPK.Value);
                        // serviceUtilityObj.FilterDate = DateTime.Parse(txtFromDate.Text);
                        objFields.Active = "1";
                        objPageFilter.BizUnit = currentUser.SBUID;
                        bool hasCustomer = HasSelectedCustomer();
                        if (hasCustomer)
                        {
                           // objSalesOrderHeader.SOH_CUSTOMER = Convert.ToInt32(hdfCustomerID.Value);
                            //serviceUtilityObj.FilterToDate = DateTime.Parse(txtToDate.Text);
                            // Status = Convert.ToInt32(ddlStatus.SelectedValue);
                           // serviceUtilityObj.InvoiceType = Convert.ToInt32(ddlInvoiceType.SelectedItem.Value);
                            objFields.CustomerPK = hdfCustomerID.Value;
                        }
                        if (hasCustomer)
                        {
                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = hdfCustomerID.Value;
                            Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = txtCustomer.Text;

                        }
                        //fillter based on SC No
                        if (hdfSoPK.Value != "" && hdfSoPK.Value !="0")
                            //objFields.ScNoPK = hdfSoPK.Value;
                            objFields.PK = hdfSoPK.Value;
                        //fillter based on Cus Po No
                        if (hdfCusPoPK.Value != "" && hdfCusPoPK.Value != "0")
                            objFields.PK = hdfCusPoPK.Value;
                        if (hdfIsMultiplePlant.Value== "1")
                            objFields.SOH_COMPANY = Convert.ToInt32(ddlPlantName.SelectedValue);
                        if (!string.IsNullOrEmpty(txtFromDate.Text))
                            objPageFilter.FilterDate = DateTime.Parse(txtFromDate.Text);
                        if (!string.IsNullOrEmpty(txtToDate.Text))
                            objPageFilter.FilterToDate = DateTime.Parse(txtToDate.Text);

                        //fillter based on status
                        objFields.SOH_STATUS = Convert.ToInt32(ddlStatus.SelectedValue);
                        if (ddlInvoiceType.SelectedIndex != 0)
                            objFields.SOH_TYPE = Convert.ToInt32(ddlInvoiceType.SelectedItem.Value);
                        //objFields.HideConverted = true;
                        dsList = SaleOrderBL.GetSaleOrderList(objPageFilter, objFields);
                        if (dsList != null)
                        {
                            serviceUtilityObj.TotalRecords = dsList.Tables[0].Rows.Count > 0 ? Convert.ToInt32(dsList.Tables[0].Rows[0]["TOTAL_ROW_COUNT"].ToString()) : 0;
                            TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                        (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                        (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        }
                       // salesOrderHeaderList = salesOrderServiceClient.GetSaleOrderHeaderSales(objSalesOrderHeader, serviceUtilityObj, ApplicationSubType.INVOICE, Status, PageType.INVOICE);
                        //TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 : (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) : (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                      
                        
                        
                        
                        break;
                    case ControlsEnum.DEFAULT:
                        salesOrderServiceClient = new SaleOrderService();
                        salesOrderServiceClient = CommonFunctions.InitiateClient(salesOrderServiceClient);
                        objSalesOrderHeader = new SAL_ORDER_HDR();


                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdSoList.PageSize;
                        serviceUtilityObj.SortBy = Resources.DataFieldRes.SODate;
                        serviceUtilityObj.ThenBy = Resources.DataFieldRes.SONo;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;
                        objPageFilter = new FilterUtility();
                        objFields = new BusinessObject.Sales.SalesOrderBO();

                        objPageFilter = new FilterUtility();
                        objFields = new BusinessObject.Sales.SalesOrderBO();
                        objPageFilter.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        objPageFilter.PageSize = grdSoList.PageSize;
                        objPageFilter.SortBy = Resources.DataFieldRes.SODate;
                        objPageFilter.ThenBy = Resources.DataFieldRes.SONo;
                        objPageFilter.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;

                        objPageFilter.BizUnit = currentUser.SBUID;
                        objFields.Active = "1";
                        if (HasSelectedCustomer())
                            objFields.CustomerPK = hdfCustomerID.Value;
                        if (hdfSoPK.Value != "")
                            objFields.PK = hdfSoPK.Value;
                        objFields.SOH_STATUS = Convert.ToInt32(ddlStatus.SelectedValue);
                        //objFields.HideConverted = true;
                        dsList = SaleOrderBL.GetSaleOrderList(objPageFilter, objFields);

                        if (dsList != null)
                        {
                            serviceUtilityObj.TotalRecords = dsList.Tables[0].Rows.Count > 0 ? Convert.ToInt32(dsList.Tables[0].Rows[0]["TOTAL_ROW_COUNT"].ToString()) : 0;
                            TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                        (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                        (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        }

                        //objSalesOrderHeader.SOH_BIZUNIT = currentUser.SBUID;
                        //objSalesOrderHeader.SOH_ACTIVE = 1;
                        //if (hdfCustomerID.Value != "")
                        //    objSalesOrderHeader.SOH_CUSTOMER = Convert.ToInt32(hdfCustomerID.Value);
                        //if (hdfSoPK.Value != "")
                        //    objSalesOrderHeader.SOH_PK = Convert.ToInt32(hdfSoPK.Value);
                        //Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        //salesOrderHeaderList = salesOrderServiceClient.GetSaleOrderHeaderSales(objSalesOrderHeader, serviceUtilityObj, ApplicationSubType.INVOICE, Status, PageType.INVOICE);
                        //TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 : (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) : (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        break;
                    case ControlsEnum.SODETAILS:
                        salesOrderServiceClient = new SaleOrderService();
                        salesOrderServiceClient = CommonFunctions.InitiateClient(salesOrderServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        salesOrderDetailsList = salesOrderServiceClient.GetSaleOrderDetails(SoId, serviceUtilityObj);

                        break;
                    case ControlsEnum.SHOWSOHDR:
                        salDespatchHdrServiceClient = new SalDespatchHdrService();
                        salDespatchHdrServiceClient = CommonFunctions.InitiateClient(salDespatchHdrServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdDOHdr.PageSize;
                        serviceUtilityObj.SortBy = Resources.DataFieldRes.DeliveryOrderDate;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;

                        salOrderHdrObj = new SAL_ORDER_HDR();
                        salOrderHdrObj.SOH_ACTIVE = 1;
                        salOrderHdrObj.SOH_PK = SoId;
                        salDespatchHdrList = salDespatchHdrServiceClient.GetDespatchHdr(salOrderHdrObj, serviceUtilityObj);

                        break;
                    case ControlsEnum.SHOWSODTL:
                        salDespatchDtlServiceClient = new SalDespatchDtlService();
                        salDespatchDtlServiceClient = CommonFunctions.InitiateClient(salDespatchDtlServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        salDespatchObj = new SAL_DESPATCH_DTL();
                        salDespatchObj.DPD_SALE_ORDER = SoId;
                        salDespatchObj.DPD_DESPATCH_HDR = DoId;
                        salDespatchDetailList = salDespatchDtlServiceClient.GetDespatchDtl(salDespatchObj, serviceUtilityObj);
                        break;
                    case ControlsEnum.TYPECATEGORY:
                        CommonServiceClient = new CommonService();
                        admConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONFIG_MST>();
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admConfigMstObj.CFG_TYPE = GetLocalResourceObject("AccountType").ToString();
                        admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    case ControlsEnum.CUSTOMERPK:
                        CommonServiceClient = new CommonService();
                        SPCRM_CUSTOMER_USER_GET_ResultObj = ERP.Utilities.CommonFunctions.Initilize<SPCRM_CUSTOMER_USER_GET_Result>();
                        SPCRM_CUSTOMER_USER_GET_ResultList = CommonServiceClient.GetCustomerDetails(currentUser.PKUser, currentUser.SBUID);
                        break;
                    #region FILLWORKFLOWSTATUS
                    case ControlsEnum.FILLWORKFLOWSTATUS:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admConfigMstObj = new ADM_CONFIG_MST();
                        admConfigMstObj.CFG_PK = 0;
                        admConfigMstObj.CFG_TYPE = GetLocalResourceObject("APPLICATION_STATUS").ToString();
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        workflowStatusList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    #endregion
                    case ControlsEnum.SOTYPE:
                        dtSOData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SO TYPE");
                        break;

                    case ControlsEnum.COMPANYNAME:
                        //gets Company List
                        admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        //  dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
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
                salDespatchHdrServiceClient= null;
                salDespatchDtlServiceClient= null;
                CommonServiceClient= null;
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
                    case ControlsEnum.SEARCH:
                    case ControlsEnum.DEFAULT:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.SHOWSOHDR:
                        BindGrid(ControlsEnum.SHOWSOHDR);
                        break;
                    case ControlsEnum.SOTYPE:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.COMPANYNAME:
                        //Bind Company List in DDL
                        BindDropDownList(controlType);
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
            ExtGridView egrd;
            string arg;
            string soPK;
            string doPK;
            if (sender.GetType().IsEquivalentTo(typeof(Button)))
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
            }
            else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
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
                    GetFieldValues(ControlsEnum.SEARCH);
                    SetFieldValues(ControlsEnum.SEARCH);                                   
                    break;
                #endregion
                #region PICK ORDERS
                case ActionsEnum.PICKFORINVOICING:
                case ActionsEnum.PICKFORADVANCEINVOICING:
                case ActionsEnum.PICKFORDO:
                    SetUIValuesToObject(commonActions);
                    break;
                #endregion
                #region Show Popup
                case ActionsEnum.SHOWPOPUP:
                    soPK = ((LinkButton)sender).CommandArgument;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + soPK + "&APPTYPE=" + ApplicationType.SO + "&APPSUBTYPE=") + "');", true);
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
                    CheckUserRightsAndRedirect(Resources.PageURL.SoListing);
                    //Response.Redirect(Resources.PageURL.SoListing);
                    break;
                case ActionsEnum.SALESINVOICE:
                    CheckUserRightsAndRedirect(Resources.PageURL.SalesInvoicing);
                    //Response.Redirect(Resources.PageURL.SalesInvoicing);
                    break;
                case ActionsEnum.INVOICE:
                    //SetUIEditView(commonActions);
                    CheckUserRightsAndRedirect(Resources.PageURL.Invoicing);
                    //Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.Invoicing), false);
                    break;
                case ActionsEnum.DELIVERYORDER:
                    CheckUserRightsAndRedirect(Resources.PageURL.DeliveryOrder);
                    //Response.Redirect(Resources.PageURL.DeliveryOrder);
                    break;
                case ActionsEnum.SALESRECEIPT:
                    CheckUserRightsAndRedirect(Resources.PageURL.SalesReceipt);
                    //Response.Redirect(Resources.PageURL.SalesReceipt);
                    break;
                case ActionsEnum.ACRECEIVABLE:
                    CheckUserRightsAndRedirect(Resources.PageURL.AccountReceivable);
                    //Response.Redirect(Resources.PageURL.AccountsReceivable);
                    break;
                case ActionsEnum.CRDRNOTE:
                    CheckUserRightsAndRedirect(Resources.PageURL.DrCrNoteSales);
                    //Response.Redirect(Resources.PageURL.DrCrNoteSales);
                    break;
                case ActionsEnum.MISC:
                    CheckUserRightsAndRedirect(Resources.PageURL.MiscellaneousInv);
                    //Response.Redirect(Resources.PageURL.Misc);
                    break;
                case ActionsEnum.RESET:
                    Session[ERP.Utilities.SessionStrings.SALEORDERPK] = null;
                    CustomerIDForAdvInv = 0;
                    SoIdForAdvInv = 0;
                    CurrencyForAdvInv = 0;
                    CustomerIDForDO = 0;
                    SoIdForDO = 0;
                    CurrencyForDO = 0;
                    SelectedSOsTaxList = new List<decimal>();
                    SelectedSOsTax = null;
                    SaleOrderType = string.Empty;
                    SelectedCurrencyForDO = new List<long>();
                    SelectedCustomersForDO = new List<long>();
                    SelectedCustomersType = new List<string>();
                    SelectedSosForDO = new List<long>();
                    SelectedCustomerListForDO = new List<long>();
                    SelectedSOListForDO = new List<long>();
                    SelectedSosCountForDO = 0;
                    SelectedOrderTypeList = new List<string>();
                    SelectedCurrencyForAdvInv = new List<long>();
                    SelectedCustomersForAdvInv = new List<long>();
                    SelectedSosForAdvInv = new List<long>();
                    SelectedCustomerListForAdvInv = new List<long>();
                    SelectedSOListForAdvInv = new List<long>();
                    SelectedSosCountForAdvInv = 0;
                    btnPickForInvoice.Text = GetLocalResourceObject("PickSoForInvoicing").ToString();
                    btnPickForDO.Text = GetLocalResourceObject("PickSoForDO").ToString();
                    btnPickForAdvInv.Text = GetLocalResourceObject("PickSoForAdvanceInvoicing").ToString();
                    SoId = 0;                  
                    grdDOHdr.DataSource = salDespatchHdrList=null;
                    grdDOHdr.DataBind();
                    break;
                #endregion
                #region SO Hdr
                case ActionsEnum.SHOWDETAILS:
                    gvr = ((RadioButton)sender).Parent.Parent as ExtGridViewRow;
                    SoId = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfSOID")).Value);

                    Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)gvr.FindControl("hdfCustomerPK")).Value;
                    Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)gvr.FindControl("lblCustomerName")).Text;
                    SOTax = Convert.ToDecimal(string.IsNullOrEmpty(((HiddenField)gvr.FindControl("hdfSOTax")).Value) ? "0" : ((HiddenField)gvr.FindControl("hdfSOTax")).Value);

                    GetFieldValues(ControlsEnum.SHOWSOHDR);
                    SetFieldValues(ControlsEnum.SHOWSOHDR);
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

                        GetFieldValues(ControlsEnum.TYPECATEGORY);
                        if (admConfigMstList != null && admConfigMstList.Count > 0)
                        {
                            admConfigMstObj = admConfigMstList.SingleOrDefault(con => con.CFG_VALUE == Convert.ToInt32(lblType.Text));
                            if (admConfigMstObj != null)
                            {
                                lblType.Text = ERP.Utilities.CommonFunctions.GetShortString(admConfigMstObj.CFG_DATA, 3, "");

                                lblType.ToolTip = admConfigMstObj.CFG_DATA;
                            }
                            else
                            {
                                lblType.Text = "";
                                lblType.ToolTip = "";
                            }
                        }

                        Button imgApproved = e.Row.FindControl("imgApproved") as Button;
                        HiddenField hdfApproved = e.Row.FindControl("hdfApproved") as HiddenField;
                        HiddenField hdfSOID = e.Row.FindControl("hdfSOID") as HiddenField;
                        short appstatus = Convert.ToInt16(hdfApproved.Value);

                        //Set the Status Icons
                        HiddenField hdfSodQty = e.Row.FindControl("hdfSodQty") as HiddenField;
                        HiddenField hdfSodQtyDispatched = e.Row.FindControl("hdfSodQtyDispatched") as HiddenField;
                        HiddenField hdfSodQtyInvoiced = e.Row.FindControl("hdfSodQtyInvoiced") as HiddenField;

                        int SodQty = Convert.ToInt32(hdfSodQty.Value);
                        int SodQtyDispatched = Convert.ToInt32(hdfSodQtyDispatched.Value);
                        int SodQtyInvoiced = Convert.ToInt32(hdfSodQtyInvoiced.Value);


                       // if (salesOrderHeaderList != null && salesOrderHeaderList.Count > 0)
                       // {
                            //if ((salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.Any(d => d.SOD_QTY_APPROVED > d.SOD_QTY_DISPATCHED)) ||
                            //    (salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.Any(i => i.SOD_QTY_APPROVED > i.SOD_QTY_INVOICED)))
                            //if ((salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.Any(d => d.SOD_QTY > d.SOD_QTY_DISPATCHED)) ||
                            //    (salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.Any(i => i.SOD_QTY > i.SOD_QTY_INVOICED)))
                        if ((SodQty > SodQtyDispatched) || (SodQty > SodQtyInvoiced))
                        {
                            imgApproved.CssClass = GetLocalResourceObject("pending").ToString();
                            imgApproved.ToolTip = Resources.Captions.Pending;
                        }
                        else
                        {
                            imgApproved.CssClass = GetLocalResourceObject("Completed").ToString();
                            imgApproved.ToolTip = Resources.Captions.Completed;
                        }
                       //}

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

                        //if (salesOrderHeaderList[e.Row.RowIndex].SOH_STATUS==0)
                        //{
                        //    imgApproved.CssClass = GetLocalResourceObject("drafted_icon").ToString();
                        //    if (workflowStatusList != null && workflowStatusList.Count > 0)
                        //        imgApproved.ToolTip = workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus) != null ?
                        //            workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus).CFG_DATA : string.Empty;
                        //}
                        //else if ((salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.Any(d => d.SOD_QTY_APPROVED > d.SOD_QTY_DISPATCHED)) ||
                        //    (salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.Any(i => i.SOD_QTY_APPROVED > i.SOD_QTY_INVOICED)))
                        //{
                        //    imgApproved.CssClass = GetLocalResourceObject("pending").ToString();
                        //    imgApproved.ToolTip = workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus) != null ?
                        //          workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus).CFG_DATA : string.Empty;
                        //}
                        //else
                        //{
                        //    imgApproved.CssClass = GetLocalResourceObject("Completed").ToString();
                        //    imgApproved.ToolTip = workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus) != null ?
                        //          workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus).CFG_DATA : string.Empty;
                        //}
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
        private void ConfigurationSettings()
        {
            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "CUSTOMER");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsSBUCustomer.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
            }

        }
        public string GetDivide(object num1, object num2)
        {
            double num = 0;
            double.TryParse(Convert.ToString(num1), out num);
            double.TryParse(Convert.ToString(num2), out num);
            decimal a, b;
            a = Convert.ToInt32(num1);
            b = Convert.ToInt32(num2);
            decimal result = a / b;
            string x = String.Format("{0:C0}", result);
            return x;
        }

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
                int AdvApprv = 0;
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
                                AdvApprv = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);

                                if (AdvApprv != 2)
                                {

                                    litErrorMsg.Text = GetLocalResourceObject("AdvApprv").ToString(); ;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    break;
                                }
                                else
                                {
                                    Session[ERP.Utilities.SessionStrings.SALEORDERPK] = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOID")).Value);
                                    btnPickForInvoice.Text = GetLocalResourceObject("PickSoForInvoicing").ToString() + "(1)";
                                    break;
                                }
                            case ActionsEnum.PICKFORDO:
                                CustomerIDForDO = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value);
                                SoIdForDO = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOID")).Value);
                                CurrencyForDO = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOCurrency")).Value);
                                SaleOrderType = ((Label)grdrow.FindControl("lblType")).Text;
                                break;
                            case ActionsEnum.PICKFORADVANCEINVOICING:
                                Session[ERP.Utilities.SessionStrings.SALEORDERPK] = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOID")).Value);
                                CustomerIDForAdvInv = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value);
                                SOTax = Convert.ToDecimal(string.IsNullOrEmpty(((HiddenField)grdrow.FindControl("hdfSOTax")).Value) ? "0" : ((HiddenField)grdrow.FindControl("hdfSOTax")).Value);
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

                            //    if (!IsSameCurrency(SelectedCurrency, Currency))
                            //    {
                            //        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Currency").ToString();
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            //    }
                            //    else if (!IsSameCustomer(SelectedCustomers, CustomerID))
                            //    {
                            //        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Customer").ToString();
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            //    }
                            //    else if (!IsExixtPk(SelectedSos, SoId))
                            //    {
                            //        //Add Customers
                            //        if (SelectedCustomers != null)
                            //        {
                            //            SelectedCustomerList = SelectedCustomers;
                            //        }
                            //        else
                            //        {
                            //            SelectedCustomerList = new List<long>();
                            //        }
                            //        SelectedCustomerList.Add(CustomerID);
                            //        SelectedCustomers = SelectedCustomerList;
                            //        //Add Sos
                            //        if (SelectedSos != null)
                            //        {
                            //            SelectedSOList = SelectedSos;
                            //        }
                            //        else
                            //        {
                            //            SelectedSOList = new List<long>();
                            //        }
                            //        SelectedSOList.Add(SoId);
                            //        SelectedSos = SelectedSOList;
                            //        SelectedSosCount = SelectedSos.Count;

                            //        btnPickForInvoice.Text = GetLocalResourceObject("PickSoForInvoicing").ToString() + "(" + SelectedSosCount.ToString() + ")";

                            //    }
                            //    else
                            //    {
                            //        litErrorMsg.Text = Resources.ErrorMessages.Msg_Item_Added;                               
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                            //    }
                            break;
                        #endregion
                        #region Pick for DO
                        case ActionsEnum.PICKFORDO:
                            if (!IsSameCurrency(SelectedCurrencyForDO, CurrencyForDO))
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Currency").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                            else if (!IsSameCustomer(SelectedCustomersForDO, CustomerIDForDO))
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Customer").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                            else if (!IsSameType(SelectedCustomersType, SaleOrderType))
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_CustomerType").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                            else if (!IsExixtPk(SelectedSosForDO, SoIdForDO))
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
                                if (SelectedSosForDO != null)
                                {
                                    SelectedSOListForDO = SelectedSosForDO;
                                }
                                else
                                {
                                    SelectedSOListForDO = new List<long>();
                                }

                                SelectedSOListForDO.Add(SoIdForDO);
                                SelectedSosForDO = SelectedSOListForDO;
                                SelectedSosCountForDO = SelectedSosForDO.Count;

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
                                btnPickForDO.Text = GetLocalResourceObject("PickSoForDO").ToString() + "(" + SelectedSosCountForDO.ToString() + ")";

                            }
                            else
                            {
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Item_Added;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                            }
                            break;
                        #endregion
                        #region Pick for Advance Invoicing
                        case ActionsEnum.PICKFORADVANCEINVOICING:
                            if (!IsMatcingTax(SelectedSOsTax, SOTax))
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Tax").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                            else if (!IsSameCurrency(SelectedCurrencyForAdvInv, CurrencyForAdvInv))
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
                                //Add Tax
                                if (SelectedSOsTax != null)
                                {
                                    SelectedSOsTaxList = SelectedSOsTax;
                                }
                                else
                                {
                                    SelectedSOsTaxList = new List<decimal>();
                                }
                                SelectedSOsTaxList.Add(SOTax);
                                SelectedSOsTax = SelectedSOsTaxList;

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

                                btnPickForAdvInv.Text = GetLocalResourceObject("PickSoForAdvanceInvoicing").ToString() + "(" + SelectedSosCountForAdvInv.ToString() + ")";

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
                    case ControlsEnum.SOTYPE:
                        ddlInvoiceType.Items.Clear();
                        if (dtSOData != null)
                        {
                            ddlInvoiceType.DataSource = dtSOData;
                            ddlInvoiceType.DataTextField = "CFG_DATA";
                            ddlInvoiceType.DataValueField = "CFG_VALUE";
                            ddlInvoiceType.DataBind();
                        }
                        ddlInvoiceType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.COMPANYNAME:
                        ddlPlantName.Items.Clear();
                        if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                        {
                            ddlPlantName.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CMP_DISPLAY_CODE);
                            ddlPlantName.DataTextField = Resources.DataFieldRes.CMP_DISPLAY_CODE;
                            ddlPlantName.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                            ddlPlantName.DataBind();
                        }
                        ddlPlantName.Items.Insert(0, new ListItem(Resources.ErpRes.SelectAll, CommonConstants.SELECTVAL));
                        break;
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
        /// Is Matcing Tax
        /// </summary>
        /// <param name="TaxList"></param>
        /// <param name="tax"></param>
        /// <returns></returns>
        private bool IsMatcingTax(List<decimal> TaxList, decimal tax)
        {

            bool flag = true;
            bool BaseType = true;
            bool CurType = true;
            if (TaxList != null)
            {
                BaseType = tax > 0 ? true : false;
                foreach (long var in TaxList)
                {
                    CurType = var > 0 ? true : false;
                    if (BaseType != CurType)
                    {
                        flag = false;
                        break;
                    }
                }
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
            txtCustomer.Text = "Type min 4 characters";
            txtSONumber.Text = string.Empty;
            txtCustomerPo.Text = string.Empty;
            hdfSoPK.Value = "";
            hdfPurPk.Value = "0";
            hdfCustomerID.Value = "";
            hdfCusPoPK.Value = "";
            //txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
            //hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
            //txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            //hdfToDate.Value = DateTime.Now.ToString();
            txtFromDate.Text = string.Empty;
            hdfFromDate.Value = string.Empty;
            txtToDate.Text = string.Empty;
            hdfToDate.Value = string.Empty;
            ddlStatus.SelectedIndex = 1;
            ddlPlantName.SelectedIndex = 0;
            ddlInvoiceType.SelectedIndex = 0;            
        }

        private bool HasSelectedCustomer()
        {
            string customerText = txtCustomer.Text.Trim();
            return !string.IsNullOrEmpty(hdfCustomerID.Value)
                && hdfCustomerID.Value != "0"
                && customerText != "Select/Type"
                && customerText != "Type min 4 characters";
        }

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

        /// <summary>
        /// Redirect to a page if user has permission on that page.
        /// </summary>
        /// <param name="RedirectUrl">Page Url</param>
        private void CheckUserRightsAndRedirect(string RedirectUrl)
        {
            CommonBL userAuth = new CommonBL();
            if (currentUser == null)
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            UserRightsBO UsrRights = userAuth.GetUserPageRights(currentUser.PKUser, RedirectUrl.Replace("~", ""), currentUser.SBUID, currentUser.CurrentDeptPK);
            if (UsrRights != null && UsrRights.Rights.Count() > 0 && UsrRights.Rights[0].UserDeptRight)
                Response.Redirect(RedirectUrl, false);
            else
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErpRes.NoPressionMsg) + "','" + Resources.ErpRes.Information + "');", true);
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
            btnPickForInvoice.PreRender += new EventHandler(btnAction_PreRender);
            btnPickForAdvInv.PreRender += new EventHandler(btnAction_PreRender);
            btnResetSelection.PreRender += new EventHandler(btnAction_PreRender);
            //lbnSOListing.PreRender += new EventHandler(btnAction_PreRender);
            //lnbDeliveryOrder.PreRender += new EventHandler(btnAction_PreRender);
            //lnbSalesInvoice.PreRender += new EventHandler(btnAction_PreRender);
            //lnbAdvanceInvoice.PreRender += new EventHandler(btnAction_PreRender);
            //lbnSalesReceipt.PreRender += new EventHandler(btnAction_PreRender);
            //lnbCrDrNote.PreRender += new EventHandler(btnAction_PreRender);
            //lnbAcPayables.PreRender += new EventHandler(btnAction_PreRender);

            btnPickForDO.Load += new EventHandler(btnAction_Load);
            //lbnSOListing.Load += new EventHandler(btnAction_Load);

            btnPickForDO.Load += new EventHandler(btnAction_Load);
            btnPickForInvoice.Load += new EventHandler(btnAction_Load);
            btnPickForAdvInv.Load += new EventHandler(btnAction_Load);
            btnResetSelection.Load += new EventHandler(btnAction_Load);
            //lbnSOListing.Load += new EventHandler(btnAction_Load);
            //lnbDeliveryOrder.Load += new EventHandler(btnAction_Load);
            //lnbSalesInvoice.Load += new EventHandler(btnAction_Load);
            //lnbAdvanceInvoice.Load += new EventHandler(btnAction_Load);
            //lbnSalesReceipt.Load += new EventHandler(btnAction_Load);
            //lnbCrDrNote.Load += new EventHandler(btnAction_Load);
            //lnbAcPayables.Load += new EventHandler(btnAction_Load);

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
                //GetFieldValues(ControlsEnum.DEFAULT);
                //SetFieldValues(ControlsEnum.DEFAULT);
                GetFieldValues(ControlsEnum.SEARCH);
                SetFieldValues(ControlsEnum.SEARCH);
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
            FILLWORKFLOWSTATUS,
            SOTYPE,
            COMPANYNAME


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
