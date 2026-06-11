using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using CustomControls;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using ERPSMS_v01.UserControls;
using System.Threading;
using BusinessLogic.AccountManagement;
using BusinessLogic.Sales;
using ERPSMS_v01;

namespace CustomerPortal.Sales
{
    public partial class SaleOrderListing : ERP.Store.UI.WorkFlowBasePage
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

        private int IsConverted
        {
            get
            {
                return this.ViewState["IsConverted"] == null ? 0 : Convert.ToInt32(this.ViewState["IsConverted"]);
            }
            set
            {
                this.ViewState["IsConverted"] = value;
            }
        }
        /// <summary>
        /// Has Short Close Right
        /// </summary>
        private bool HasShortCloseRight
        {
            get
            {
                return this.ViewState["HasShortCloseRight"] == null ? false : Convert.ToBoolean(this.ViewState["HasShortCloseRight"]);
            }
            set
            {
                this.ViewState["HasShortCloseRight"] = value;
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
        /// SO Detail ID
        /// </summary>
        private int SoDtlId
        {
            get
            {
                return this.ViewState[ViewstateStrings.SoDtlId] == null ? 0 : (int)this.ViewState[ViewstateStrings.SoDtlId];
            }
            set
            {
                this.ViewState[ViewstateStrings.SoDtlId] = value;
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
        /// <summary>
        /// 
        /// </summary>
        private int CurRecordDept
        {
            get
            {
                return this.ViewState["CurRecordDept"] == null ? 0 : (int)this.ViewState["CurRecordDept"];
            }
            set
            {
                this.ViewState["CurRecordDept"] = value;
            }
        }

        /// <summary>
        /// Selected Record PlantPK
        /// </summary>
        private int PlantPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.CompanyPK] == null ? 0 : (int)this.ViewState[ViewstateStrings.CompanyPK];
            }
            set
            {
                this.ViewState[ViewstateStrings.CompanyPK] = value;
            }
        }
        #endregion

        User currentUser;
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        private int listingRefID;

        //page related Entity Object
        private ServiceUtility serviceUtilityObj;
        private SAL_ORDER_HDR objSalesOrderHeader;
        private SAL_ORDER_HDR objSalesOrderHeaderSc;
        private List<SAL_DESPATCH_HDR> salDespatchHdrList;
        private SAL_ORDER_HDR salOrderHdrObj;

        private List<SAL_DESPATCH_DTL> salDespatchDetailList;
        private SAL_DESPATCH_DTL salDespatchObj;
        private List<SPADM_APP_STATUS_CFG_GET_KV_Result> workflowStatusList;

        private List<ADM_COMPANY_MST> admCompanyMstList;
        private ADM_COMPANY_MST admCompanyMstObj;

        private List<SAL_ORDER_HDR> salesOrderHeaderList;
        private List<SAL_ORDER_DTL> salesOrderDetailsList;
        private List<long> SelectedSOList;
        private List<long> SelectedSOListForAdvInv;
        private List<long> SelectedSOListForDO;

        private List<long> SelectedCustomerList;
        private List<long> SelectedCustomerListForAdvInv;
        private List<long> SelectedCustomerListForDO;

        private ADM_CONFIG_MST admConfigMstObj;
        private List<ADM_CONFIG_MST> admConfigMstList;

        private SPCRM_CUSTOMER_USER_GET_Result SPCRM_CUSTOMER_USER_GET_ResultObj;
        private List<SPCRM_CUSTOMER_USER_GET_Result> SPCRM_CUSTOMER_USER_GET_ResultList;
        DataTable dtSOData;
        private DataTable dtSODtls;
        private DataTable dtPackingDtls;
        private DataTable dtCopyEligibleDtls;
        private DataTable dtCompany;
        private int SaleContractPk;
        string scNo = "";
        string scPk = "";
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
                    ConfigurationSettings();
                    GetUserRights();
                    SelectedCustomers = null;
                    SelectedSos = null;
                    SelectedSosCount = 0;

                    Session[ERP.Utilities.SessionStrings.QUOTATIONPK] = null;
                    Session[ERP.Utilities.SessionStrings.SALEORDERPK] = null;
                    Session[ERP.Utilities.SessionStrings.SALEORDERCOPYPK] = null;
                    Session[ERP.Utilities.SessionStrings.SaleOrderMode] = null;

                    hdfRateFormat.Value = "#,0.";
                    int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                    ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                    : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit]));
                    for (int i = 0; i < rateDecimalDigits; i++)
                    {
                        hdfRateFormat.Value += "0";
                    }

                    txtFromDate.Text = string.Empty;//DateTime.Now.AddMonths(-1).AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = string.Empty;//DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();
                    //txtFromDate.Text = string.Empty;
                    //hdfFromDate.Value = string.Empty;
                    //txtToDate.Text = string.Empty;
                    //hdfToDate.Value = string.Empty;

                    //Set Selected Orders Count 
                    // Need replace viewstate to session
                    FillProcessId();
                    GetFieldValues(ControlsEnum.CUSTOMERPK);
                    if (SPCRM_CUSTOMER_USER_GET_ResultList != null && SPCRM_CUSTOMER_USER_GET_ResultList.Count > 0)
                    {
                        hdfCustomerPK.Value = SPCRM_CUSTOMER_USER_GET_ResultList[0].CUS_PK.ToString();
                        hdfCustomerID.Value = SPCRM_CUSTOMER_USER_GET_ResultList[0].CUS_PK.ToString();
                        txtCustomer.Text = HttpUtility.HtmlDecode(SPCRM_CUSTOMER_USER_GET_ResultList[0].CUS_NAME);
                        txtCustomer.Attributes.Add("disabled", "disabled");
                        txtCustomer.CssClass = "input-disabled";
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "DisableAuto", "DisableAuto($('[id$=txtCustomer]'));", true);
                    }
                    //fetch and fill Company
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    GetFieldValues(ControlsEnum.FILLWORKFLOWSTATUS);
                    SetFieldValues(ControlsEnum.FILLWORKFLOWSTATUS);
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    GetFieldValues(ControlsEnum.SOTYPE);
                    SetFieldValues(ControlsEnum.SOTYPE);
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                    EntryStatus = EntryStatus.LISTMODE;
                    //btnEdit.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(currentUser.PKUser, FillProcessId());
                    if (this.GetGlobalResourceObject("ConfigurationsRes", "IsPackingDetailsVisible").ToString() == "1")
                    {
                        divPackingDtls.Visible = true;
                    }
                    else
                    {
                        divDeliveryOrderDtls.Visible = true;
                    }
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
            SaleOrderService salesOrderServiceClient;
            salesOrderServiceClient = null;
            SalDespatchHdrService salDespatchHdrServiceClient;
            CommonService CommonServiceClient;
            AdmCompanyMstService admCompanyMstServiceClient;
            salDespatchHdrServiceClient = null;
            SalDespatchDtlService salDespatchDtlServiceClient;
            salDespatchDtlServiceClient = null;
            int Status;
            int WrkfStatus;
            try
            {
                switch (type)
                {
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
                    case ControlsEnum.SEARCH:
                        if (!string.IsNullOrEmpty(txtSONumber.Text) && (txtSONumber.Text != "Select/Type"))//Check whether is draft deleted
                        {
                            scNo = txtSONumber.Text.Trim();
                            GetFieldValues(ControlsEnum.GETPKFROMSCNO);
                            hdfSoPK.Value = scPk;
                        }

                        salesOrderServiceClient = new SaleOrderService();
                        salesOrderServiceClient = CommonFunctions.InitiateClient(salesOrderServiceClient);
                        objSalesOrderHeader = new SAL_ORDER_HDR();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdSoList.PageSize;
                        serviceUtilityObj.SortBy = Resources.DataFieldRes.SODate;
                        serviceUtilityObj.ThenBy = Resources.DataFieldRes.SONo;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;

                        objSalesOrderHeader.SOH_ACTIVE = 1;
                        objSalesOrderHeader.SOH_CRTD_BY = currentUser.PKUser;
                        objSalesOrderHeader.SOH_BIZUNIT = currentUser.SBUID;
                        objSalesOrderHeader.SOH_COMPANY = Convert.ToInt32(ddlCompanyFilter.SelectedValue);
                        if (hdfCustomerID.Value != "")
                            objSalesOrderHeader.SOH_CUSTOMER = Convert.ToInt32(hdfCustomerID.Value);
                        if ((hdfSoPK.Value != "") && !string.IsNullOrEmpty(txtSONumber.Text) && (txtSONumber.Text != "Select/Type"))
                        {
                            objSalesOrderHeader.SOH_NO = txtSONumber.Text.Trim();
                            objSalesOrderHeader.SOH_PK = Convert.ToInt32(hdfSoPK.Value);
                        }

                        if ((hdfCusPoNumber.Value != "") && !string.IsNullOrEmpty(txtCUSPONumber.Text) && (txtCUSPONumber.Text != "Select/Type"))
                            objSalesOrderHeader.SOH_REFERENCE = hdfCusPoNumber.Value;
                        if (!string.IsNullOrEmpty(txtFromDate.Text))
                            serviceUtilityObj.FilterDate = DateTime.Parse(txtFromDate.Text);
                        if (!string.IsNullOrEmpty(txtToDate.Text))
                            serviceUtilityObj.FilterToDate = DateTime.Parse(txtToDate.Text);
                        if (ddlInvoiceType.SelectedIndex != 0)
                            serviceUtilityObj.InvoiceType = Convert.ToInt32(ddlInvoiceType.SelectedItem.Value);

                        if ((hdfInvNo.Value != "") && !string.IsNullOrEmpty(txtInvNo.Text) && (txtInvNo.Text != "Select/Type"))
                            objSalesOrderHeader.SOH_CONV_INV_NO = txtInvNo.Text;

                        Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        WrkfStatus = Convert.ToInt32(ddlWrokFlowStatus.SelectedValue);
                        salesOrderHeaderList = salesOrderServiceClient.GetSaleOrderHeader(objSalesOrderHeader, serviceUtilityObj, ApplicationSubType.INVOICE, Status, PageType.CUSTOMER, WrkfStatus);
                        TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 : (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) : (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        break;
                    #region Get Pk From Sc No
                    case ControlsEnum.GETPKFROMSCNO:
                        salesOrderServiceClient = new SaleOrderService();
                        salesOrderServiceClient = CommonFunctions.InitiateClient(salesOrderServiceClient);
                        scPk = salesOrderServiceClient.GetPkFromScNo(scNo);
                        scPk = (Convert.ToInt32(scPk) > 0 ? Convert.ToInt32(scPk) : -1).ToString();
                        break;
                    #endregion
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
                        objSalesOrderHeader.SOH_COMPANY = Convert.ToInt32(ddlCompanyFilter.SelectedValue);

                        objSalesOrderHeader.SOH_ACTIVE = 1;
                        objSalesOrderHeader.SOH_CRTD_BY = currentUser.PKUser;
                        objSalesOrderHeader.SOH_BIZUNIT = currentUser.SBUID;
                        if (hdfCustomerPK.Value != "")
                        {
                            objSalesOrderHeader.SOH_CUSTOMER = Convert.ToInt32(hdfCustomerPK.Value);
                            if (hdfSoPK.Value != "")
                                objSalesOrderHeader.SOH_PK = Convert.ToInt32(hdfSoPK.Value);
                            Status = Convert.ToInt32(ddlStatus.SelectedValue);
                            WrkfStatus = Convert.ToInt32(ddlWrokFlowStatus.SelectedValue);
                            salesOrderHeaderList = salesOrderServiceClient.GetSaleOrderHeader(objSalesOrderHeader, serviceUtilityObj, ApplicationSubType.INVOICE, Status, PageType.CUSTOMERUSER, WrkfStatus);
                            TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 : (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) : (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        }
                        else
                        {
                            if (hdfCustomerID.Value != "")
                                objSalesOrderHeader.SOH_CUSTOMER = Convert.ToInt32(hdfCustomerID.Value);
                            if (hdfSoPK.Value != "")
                                objSalesOrderHeader.SOH_PK = Convert.ToInt32(hdfSoPK.Value);
                            if (!string.IsNullOrEmpty(txtFromDate.Text))
                                serviceUtilityObj.FilterDate = DateTime.Parse(txtFromDate.Text);
                            if (!string.IsNullOrEmpty(txtToDate.Text))
                                serviceUtilityObj.FilterToDate = DateTime.Parse(txtToDate.Text);
                            Status = Convert.ToInt32(ddlStatus.SelectedValue);
                            // WrkfStatus = Convert.ToInt32(ddlWrokFlowStatus.SelectedValue);
                            WrkfStatus = -1;
                            // companyPk = Convert.ToInt32(ddlCompanyFilter.SelectedValue);
                            salesOrderHeaderList = salesOrderServiceClient.GetSaleOrderHeader(objSalesOrderHeader, serviceUtilityObj, ApplicationSubType.INVOICE, Status, PageType.CUSTOMER, WrkfStatus);
                            TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 : (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) : (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        }

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
                    case ControlsEnum.SOSTATUS:
                        CommonServiceClient = new CommonService();
                        admConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONFIG_MST>();
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admConfigMstObj.CFG_TYPE = "SO STATUS";
                        admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    #region FILLWORKFLOWSTATUS
                    case ControlsEnum.FILLWORKFLOWSTATUS:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        workflowStatusList = CommonServiceClient.GetWorkFlowStatus(ApplicationType.SO, null, Convert.ToByte(CommonConstants.ACTIVE));
                        break;
                    case ControlsEnum.SOTYPE:
                        dtSOData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SO TYPE");
                        break;
                    #endregion
                    case ControlsEnum.SHOWPACKINGHDR:
                        dtSODtls = BusinessLogic.Sales.SaleOrderBL.GetSaleOrderDetails(SoId);
                        break;
                    case ControlsEnum.SHOWPACKINGDETAILS:
                        dtPackingDtls = BusinessLogic.Sales.SaleOrderBL.GetPackingDtls(Convert.ToInt32(DbActiveStatus.ACTIVE), SoDtlId);
                        break;
                    case ControlsEnum.SOCOPYELIGIBLEDETAILS:
                        salesOrderServiceClient = new SaleOrderService();
                        salesOrderServiceClient = CommonFunctions.InitiateClient(salesOrderServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        dtCopyEligibleDtls = salesOrderServiceClient.GetSaleOrderCopyEligibleDetails(SaleContractPk);
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
                salDespatchDtlServiceClient = null;
                salDespatchHdrServiceClient = null;
                CommonServiceClient = null;
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
                    case ControlsEnum.COMPANY:
                        //Bind Company List in DDL
                        BindDropDownList(ControlsEnum.COMPANY);
                        break;
                    case ControlsEnum.SEARCH:
                    case ControlsEnum.DEFAULT:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.SHOWSOHDR:
                        BindGrid(ControlsEnum.SHOWSOHDR);
                        break;

                    case ControlsEnum.FILLWORKFLOWSTATUS:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.SOTYPE:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.SHOWPACKINGHDR:
                        BindGrid(ControlsEnum.SHOWPACKINGHDR);
                        break;
                    case ControlsEnum.SHOWPACKINGDETAILS:
                        break;
                    #region UPDATECOMPANY
                    case ControlsEnum.UPDATECOMPANY:
                        BindDropDownList(ControlsEnum.UPDATECOMPANY);
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
            GridViewRow gvrTemp;
            GridView grd;
            ExtGridView egrd;
            HiddenField hdfDept;
            HiddenField hdfDelStatus;
            HiddenField hdfWrkfStatus;
            HiddenField hdfisconverted;

            string arg;
            int dept;
            string soPK;
            int soStatus = 0;

            int soStatusTemp = 0;
            int soPKTemp = 0;
            bool flag = false;
            WorkflowCore.CoreService workflowCore;
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
                #region Clear if invalid ID
                case ActionsEnum.SONUM:
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "AfterInvalidSelect", "$(document).ready(function(){AfterInvalidSelect();});", true);
                    break;
                #endregion
                #region New
                case ActionsEnum.NEW:
                    Session[ERP.Utilities.SessionStrings.SaleOrderMode] = EntryStatus.NEWMODE;
                    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.SaleContractDetails), false);
                    break;
                #endregion
                #region extra grid ondemand data population
                case ActionsEnum.SODETAILS:
                    #region Hide cancel SC button for short closed transactions.
                    foreach (GridViewRow grdrow in grdSoList.Rows)
                    {
                        RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                        if (rbtn != null && rbtn.Checked)
                        {
                            flag = true;
                            gvrTemp = (rbtn).Parent.Parent as ExtGridViewRow;
                            soStatus = Convert.ToInt32(((HiddenField)gvrTemp.FindControl("hdfSOStatus")).Value);
                            break;
                        }
                    }
                    if (flag)
                    {
                        if (Convert.ToInt16(soStatus) == Convert.ToInt16(WkfStatusEnum.SHORTCLOSED))
                        {
                            btnEditforCancel.Visible = false;
                        }
                        else
                        {
                            btnEditforCancel.Visible = true;
                        }
                    }
                    #endregion
                    arg = ((Button)sender).CommandArgument;
                    gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                    if (gvr != null)
                    {

                        workflowCore = new WorkflowCore.CoreService();
                        listingRefID = workflowCore.GetRefID(SoId, PageProcessID);
                        soStatus = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfSOStatus")).Value);
                        btnAmend.Visible = soStatus == 2;


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
                    #region Hide cancel SC button for short closed transactions.
                    EntryStatus = EntryStatus.VIEWMODE;
                    foreach (GridViewRow grdrow in grdSoList.Rows)
                    {
                        RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                        if (rbtn != null && rbtn.Checked)
                        {
                            flag = true;
                            gvrTemp = (rbtn).Parent.Parent as ExtGridViewRow;
                            soStatus = Convert.ToInt32(((HiddenField)gvrTemp.FindControl("hdfSOStatus")).Value);
                            break;
                        }
                    }
                    if (flag)
                    {
                        if (Convert.ToInt16(soStatus) == Convert.ToInt16(WkfStatusEnum.SHORTCLOSED))
                        {
                            btnEditforCancel.Visible = false;
                        }
                        else
                        {
                            btnEditforCancel.Visible = true;
                        }
                    }
                    #endregion
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
                case ActionsEnum.PACKINGDETAILS:
                    arg = ((Button)sender).CommandArgument;
                    gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                    if (gvr != null)
                    {
                        grd = gvr.FindControl("grdPackingDetails") as GridView;
                        if (string.IsNullOrEmpty(arg))
                        {
                            dtPackingDtls = null;
                        }
                        else
                        {
                            SoDtlId = Convert.ToInt32(arg);
                            GetFieldValues(ControlsEnum.SHOWPACKINGDETAILS);
                        }
                        grd.Visible = true;
                        if (dtPackingDtls != null && dtPackingDtls.Rows.Count > 0)
                        {
                            grd.DataSource = dtPackingDtls;
                            grd.DataBind();
                        }
                        (gvr.FindControl("hdfIsExpandedPackingItem") as HiddenField).Value = "1";
                    }
                    break;
                #endregion
                #region Search
                case ActionsEnum.SEARCH:
                    PageIndex = "1";
                    GetFieldValues(ControlsEnum.SEARCH);
                    SetFieldValues(ControlsEnum.SEARCH);
                    hdfSoPK.Value = string.Empty;
                    break;
                #endregion
                #region reset form
                case ActionsEnum.CLEAR:
                    ResetForm();
                    PageIndex = "1";
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
                #region Tab navigation
                case ActionsEnum.SALEORDER:
                case ActionsEnum.SALEORDERLISTING:
                case ActionsEnum.EDIT:
                case ActionsEnum.VIEW:
                case ActionsEnum.COPY:
                case ActionsEnum.AMEND:
                case ActionsEnum.EDITFORCANCEL:
                    SetUIEditView(commonActions);
                    break;
                #endregion
                #region Print
                case ActionsEnum.PRINTSO:
                    SetUIEditView(commonActions);
                    workflowCore = new WorkflowCore.CoreService();
                    listingRefID = workflowCore.GetRefID(SoId, PageProcessID);
                    break;
                #endregion
                #region SO Hdr
                case ActionsEnum.SHOWDETAILS:
                    gvr = ((RadioButton)sender).Parent.Parent as ExtGridViewRow;
                    SoId = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfSOID")).Value);
                    hdfDept = gvr.FindControl("hdfDept") as HiddenField;
                    hdfDelStatus = gvr.FindControl("hdfSODELStatus") as HiddenField;
                    hdfWrkfStatus = gvr.FindControl("hdfSOStatus") as HiddenField;
                    //hdfisconverted= gvr.FindControl("hdfSOH_CONVERTED") as HiddenField;
                    hdfDelStatusCurrent.Value = (Convert.ToInt16(hdfDelStatus.Value)).ToString();
                    hdfWrkfStatusCurrent.Value = (Convert.ToInt16(hdfWrkfStatus.Value)).ToString();
                    hdfIsSCConverted.Value = ((HiddenField)gvr.FindControl("hdfSOH_CONVERTED")).Value == string.Empty ? "0" : ((HiddenField)gvr.FindControl("hdfSOH_CONVERTED")).Value;
                    List<string> wkfsatus = new List<string> { "1", "2", "7", "12", "20" };
                    //if (Convert.ToInt16(hdfDelStatus.Value) == 1 || (Convert.ToInt16(hdfWrkfStatus.Value) != 2 && Convert.ToInt16(hdfWrkfStatus.Value) != 12 && Convert.ToInt16(hdfWrkfStatus.Value) != 20 && Convert.ToInt16(hdfWrkfStatus.Value) != 1))
                    if (Convert.ToInt16(hdfDelStatus.Value) == 1 || !(wkfsatus.Contains(hdfWrkfStatus.Value)))
                    {
                        btnEditforCancel.Visible = false;
                    }
                    else
                    {
                        btnEditforCancel.Visible = true;
                    }
                    if (Convert.ToInt16(hdfDelStatus.Value) == 1 || (Convert.ToInt16(hdfWrkfStatus.Value) != 2)) //2=>Verify&GenerateI.O
                    {
                        btnAmend.Visible = false;
                    }
                    else
                    {
                        btnAmend.Visible = true;
                    }


                    if ( (Convert.ToInt16(hdfIsSCConverted.Value) == 1)&& (Convert.ToInt16(hdfIsShowConverted.Value) == 0))
                    {
                        btnAmend.Visible = false;
                    }
                    else
                    {
                        btnAmend.Visible = true;
                    }




                    if (Convert.ToInt16(hdfDelStatus.Value) == 1 && (Convert.ToInt16(hdfWrkfStatus.Value) == 4)) //2=>Verify&GenerateI.O
                    {
                        btnEdit.Visible = false;
                    }
                    else
                    {
                        btnEdit.Visible = true;
                    }

                    if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                    {
                        if (CurRecordDept != dept)
                        {
                            CurRecordDept = dept;
                            Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                            base.SetUserDept();
                        }
                    }
                    workflowCore = new WorkflowCore.CoreService();
                    listingRefID = workflowCore.GetRefID(SoId, PageProcessID);
                    soStatus = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfSOStatus")).Value);
                    // btnAmend.Visible = soStatus == 2;
                    if (this.GetGlobalResourceObject("ConfigurationsRes", "IsPackingDetailsVisible").ToString() == "1")
                    {
                        GetFieldValues(ControlsEnum.SHOWPACKINGHDR);
                        SetFieldValues(ControlsEnum.SHOWPACKINGHDR);
                    }
                    else
                    {
                        GetFieldValues(ControlsEnum.SHOWSOHDR);
                        SetFieldValues(ControlsEnum.SHOWSOHDR);
                    }

                    break;
                #endregion
                #region Show Popup
                case ActionsEnum.SHOWPOPUP:
                    //When an SC(status:Draft) is selected, and on clicking 'SC No.' hyper link , 'Cancel' and 'Amend' buttons are displaying.
                    gvr = ((LinkButton)sender).Parent.Parent as ExtGridViewRow;
                    hdfWrkfStatus = gvr.FindControl("hdfSOStatus") as HiddenField;
                    if (Convert.ToInt16(hdfWrkfStatus.Value) == 0)
                    {
                        btnEditforCancel.Visible = false;
                        btnAmend.Visible = false;
                    }

                    foreach (GridViewRow grdrow in grdSoList.Rows)
                    {
                        RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                        if (rbtn != null && rbtn.Checked)
                        {
                            flag = true;
                            gvrTemp = (rbtn).Parent.Parent as ExtGridViewRow;
                            soPKTemp = Convert.ToInt32(((HiddenField)gvrTemp.FindControl("hdfSOID")).Value);
                            soStatusTemp = Convert.ToInt32(((HiddenField)gvrTemp.FindControl("hdfSOStatus")).Value);
                            break;
                        }
                    }
                    if (flag)
                    {
                        workflowCore = new WorkflowCore.CoreService();
                        listingRefID = workflowCore.GetRefID(soPKTemp, PageProcessID);
                        btnAmend.Visible = soStatusTemp == 2;
                    }
                    soPK = ((LinkButton)sender).CommandArgument;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + soPK + "&APPTYPE=" + ApplicationType.SO + "&APPSUBTYPE=") + "');", true);
                    break;
                case ActionsEnum.SHORTCLOSE:
                    CurrPK = Convert.ToInt32(((Button)sender).CommandArgument);
                    txtRemarks.Text = txtRefNo.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowShortClose", "ShowShortClose();", true);
                    break;
                case ActionsEnum.SHORTCLOSESAVE:
                    int? result = SaleOrderBL.SaveSaleOrderShortClose(CurrPK, txtRemarks.Text, txtRefNo.Text, currentUser.PKUser);
                    if (result > 0)
                    {
                        litErrorMsg.Text = GetLocalResourceObject("Msg_ShortClose_Success").ToString();//Msg_Save_Success
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                    }
                    else
                    {
                        if (result == (int)DbSaveStatus.SQLERROR)
                        {
                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.CONCURRENCY)
                        {
                            litErrorMsg.Text = Resources.PageNameRes.POOpeningInvoice + " " + Resources.Messages.EditUsedByAnotherUser;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.CODEEXIST)
                        {
                            litErrorMsg.Text = Resources.PageNameRes.POOpeningInvoice + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "','" + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.INVOICEEXISTASDRAFT)
                        {
                            litErrorMsg.Text = Resources.Messages.Err_Msg_InvoiceExist;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "','" + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalesOrder);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                    }
                    break;
                #endregion

                #region SHOWPLANTUPDATEPOPUP
                case ActionsEnum.SHOWPLANTUPDATEPOPUP:
                    gvr = ((LinkButton)sender).Parent.Parent as ExtGridViewRow;
                    CurrPK = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                    PlantPK = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfCmpPK")).Value);
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.UPDATECOMPANY);
                    if (PlantPK > 0 && ddlUpdateCompany.Items.FindByValue(PlantPK.ToString()) != null)
                        ddlUpdateCompany.SelectedValue = PlantPK.ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divUpdatePlant]','" + GetLocalResourceObject("UpdatePlant").ToString() + "','300','150');", true);
                    break;
                #endregion
                #region UPDATEPLANT
                case ActionsEnum.UPDATEPLANT:
                    int selectedPlantPk = Convert.ToInt16(ddlUpdateCompany.SelectedValue);
                    int? updateResult = SaleOrderBL.UpdateSaleOrderPlant(CurrPK, currentUser.PKUser, selectedPlantPk);
                    if (updateResult > 0)
                    {
                        litErrorMsg.Text = GetLocalResourceObject("Msg_UpdatePlant_Success").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        CurrPK = 0;
                    }
                    else
                    {
                        if (updateResult == (int)DbSaveStatus.SQLERROR)
                        {
                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (updateResult == (int)DbSaveStatus.CONCURRENCY)
                        {
                            litErrorMsg.Text = Resources.PageNameRes.POOpeningInvoice + " " + Resources.Messages.EditUsedByAnotherUser;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (updateResult == (int)DbSaveStatus.CODEEXIST)
                        {
                            litErrorMsg.Text = Resources.PageNameRes.POOpeningInvoice + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "','" + "');", true);
                        }
                        else if (updateResult == (int)DbSaveStatus.INVOICEEXISTASDRAFT)
                        {
                            litErrorMsg.Text = Resources.Messages.Err_Msg_InvoiceExist;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "','" + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalesOrder);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                    }
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
                        imgApproved.Style.Add("margin-bottom", "2px");
                        Button imgShortClose = e.Row.FindControl("imgShortClose") as Button;
                        imgShortClose.Style.Add("margin-bottom", "2px");
                        LinkButton lnkPlantCode = e.Row.FindControl("lnkPlantCode") as LinkButton;

                        HiddenField hdfApproved = e.Row.FindControl("hdfApproved") as HiddenField;
                        HiddenField hdfSOID = e.Row.FindControl("hdfSOID") as HiddenField;
                        HiddenField hdfIsGlowPr = e.Row.FindControl("hdfIsGlowPr") as HiddenField;
                        HiddenField hdfIsPackPr = e.Row.FindControl("hdfIsPackPr") as HiddenField;
                        HiddenField hdfSOH_CONVERTED = e.Row.FindControl("hdfSOH_CONVERTED") as HiddenField;
                        int IsGlowPr = Convert.ToInt16(hdfIsGlowPr.Value);
                        int IsPackPr = Convert.ToInt16(hdfIsPackPr.Value);

                        


                        if (IsGlowPr > 0 || IsPackPr > 0)
                        {
                            if (IsGlowPr == 1 && IsPackPr == 1)
                            {
                                e.Row.Style["background"] = "#ddf0bc!important";
                            }
                            else
                            {
                                e.Row.Style["background"] = "#d4e8e3!important";
                            }
                        }
                        short appstatus = Convert.ToInt16(hdfApproved.Value);
                        if (salesOrderHeaderList != null && salesOrderHeaderList.Count > 0)
                        {
                            lnkPlantCode.Visible = GetConfigData().IsMultiplePlant;
                            #region Short closing Button visibility
                            int saleContractStatus = salesOrderHeaderList[e.Row.RowIndex].SOH_STATUS;
                            if (HasShortCloseRight == true && saleContractStatus != 0 && saleContractStatus != 4 && saleContractStatus != 104 && saleContractStatus != 3)
                            {
                                imgShortClose.CssClass = GetLocalResourceObject("ShortClosing").ToString();
                                imgShortClose.Visible = true;
                            }
                            else
                                imgShortClose.Visible = false;
                            #endregion
                            if (salesOrderHeaderList[e.Row.RowIndex].SOH_DEL_STATUS == 1)
                            {
                                imgApproved.CssClass = GetLocalResourceObject("Canceling").ToString();
                                imgApproved.ToolTip = workflowStatusList.SingleOrDefault(aa => aa.ASC_VALUE == appstatus) != null ?
                                      workflowStatusList.SingleOrDefault(aa => aa.ASC_VALUE == appstatus).ASC_NAME : string.Empty;
                            }
                            else if (salesOrderHeaderList[e.Row.RowIndex].SOH_STATUS != 2)
                            {
                                if (salesOrderHeaderList[e.Row.RowIndex].SOH_STATUS == 12) //For Accepted Records
                                    imgApproved.CssClass = GetLocalResourceObject("accepted").ToString();
                                else if (salesOrderHeaderList[e.Row.RowIndex].SOH_STATUS == 20)//For finApproved Records
                                    imgApproved.CssClass = GetLocalResourceObject("fin-approve").ToString();
                                else if (salesOrderHeaderList[e.Row.RowIndex].SOH_STATUS == 21)//For Credit approve
                                    imgApproved.CssClass = GetLocalResourceObject("credit-approve").ToString();
                                else if (salesOrderHeaderList[e.Row.RowIndex].SOH_STATUS == 1 || salesOrderHeaderList[e.Row.RowIndex].SOH_STATUS == 7)//For Submitted Record and More info submitted
                                    imgApproved.CssClass = GetLocalResourceObject("grd-submit").ToString();
                                else if (salesOrderHeaderList[e.Row.RowIndex].SOH_STATUS == 0)// for drafted
                                    imgApproved.CssClass = GetLocalResourceObject("draft").ToString();
                                else if (salesOrderHeaderList[e.Row.RowIndex].SOH_STATUS == 11 || salesOrderHeaderList[e.Row.RowIndex].SOH_STATUS == 6)//Request for more info for salescontract 
                                    imgApproved.CssClass = GetLocalResourceObject("ReqMore").ToString();
                                else if (salesOrderHeaderList[e.Row.RowIndex].SOH_STATUS == 104) //For Short Close
                                    imgApproved.CssClass = GetLocalResourceObject("shortClose").ToString();
                                else//for Reviwed
                                    imgApproved.CssClass = GetLocalResourceObject("Review").ToString();
                                if (workflowStatusList != null && workflowStatusList.Count > 0)
                                    imgApproved.ToolTip = workflowStatusList.SingleOrDefault(aa => aa.ASC_VALUE == appstatus) != null ?
                                        workflowStatusList.SingleOrDefault(aa => aa.ASC_VALUE == appstatus).ASC_NAME : string.Empty;

                                #region ToolTip for Shortclose
                                if (salesOrderHeaderList[e.Row.RowIndex].SOH_STATUS == 104) //For Short Close 
                                {
                                    string shortCloseReason = "", shortCloseRefNo = "";
                                    shortCloseReason = !string.IsNullOrEmpty(salesOrderHeaderList[e.Row.RowIndex].SOH_SHORT_CLS_REASON) ? "\nRemark:" + salesOrderHeaderList[e.Row.RowIndex].SOH_SHORT_CLS_REASON : string.Empty;
                                    shortCloseRefNo = !string.IsNullOrEmpty(salesOrderHeaderList[e.Row.RowIndex].SOH_SHORT_CLS_REFNO) ? "\nRef.No.:" + salesOrderHeaderList[e.Row.RowIndex].SOH_SHORT_CLS_REFNO : string.Empty;
                                    imgApproved.ToolTip = imgApproved.ToolTip + shortCloseReason + shortCloseRefNo;
                                };
                                #endregion
                            }

                            ////else if ((salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.Any(d => d.SOD_QTY_APPROVED > d.SOD_QTY_DISPATCHED)) ||
                            ////    (salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.Any(i => i.SOD_QTY_APPROVED > i.SOD_QTY_INVOICED)))
                            ////{
                            ////    imgApproved.CssClass = GetLocalResourceObject("pending").ToString();
                            ////    imgApproved.ToolTip = workflowStatusList.SingleOrDefault(aa => aa.ASC_VALUE == appstatus) != null ?
                            ////          workflowStatusList.SingleOrDefault(aa => aa.ASC_VALUE == appstatus).ASC_NAME : string.Empty;
                            ////}
                            ////else
                            ////{
                            ////    imgApproved.CssClass = GetLocalResourceObject("Completed").ToString();
                            ////    imgApproved.ToolTip = Resources.Captions.Despatched;
                            ////    //imgApproved.ToolTip = workflowStatusList.SingleOrDefault(aa => aa.ASC_VALUE == appstatus) != null ?
                            ////    //      workflowStatusList.SingleOrDefault(aa => aa.ASC_VALUE == appstatus).ASC_NAME : string.Empty;
                            ////}

                            else if ((salesOrderHeaderList[e.Row.RowIndex].SOH_INVOICE_STATUS == 1) && (salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.Any(d => d.SOD_QTY_DISPATCHED > 0)))//for Sales Invoice Approved Sales Cotract Details
                            {
                                imgApproved.CssClass = GetLocalResourceObject("PartiallyInv").ToString();
                                imgApproved.ToolTip = Resources.Captions.PartiallyDespatched;
                            }

                            else if ((salesOrderHeaderList[e.Row.RowIndex].SOH_INVOICE_STATUS == 2) && (salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.Any(d => d.SOD_QTY_DISPATCHED > 0)))//for Sales Invoice Approved Sales Cotract Details
                            {
                                imgApproved.CssClass = GetLocalResourceObject("Completed").ToString();
                                imgApproved.ToolTip = Resources.Captions.Despatched;
                                imgShortClose.Visible = false;//Do not allow to short close sale contract, which are 100% despatched(Bug ID:21731)
                            }
                            else
                            {
                                imgApproved.CssClass = GetLocalResourceObject("pending").ToString();
                                imgApproved.ToolTip = workflowStatusList.SingleOrDefault(aa => aa.ASC_VALUE == appstatus) != null ?
                                workflowStatusList.SingleOrDefault(aa => aa.ASC_VALUE == appstatus).ASC_NAME : string.Empty;
                            }
                        }

                        //hide Shorclose button when converted SC
                        if (hdfSOH_CONVERTED.Value == "1" && hdfIsShowConverted.Value == "0")
                        {
                            imgShortClose.Visible = false;
                        }
                       
                        //if (salesOrderHeaderList != null && salesOrderHeaderList.Count > 0)
                        //{
                        //    if (salesOrderHeaderList[e.Row.RowIndex].SOH_STATUS ==0)
                        //    {
                        //        imgApproved.CssClass = GetLocalResourceObject("draft").ToString();
                        //        GetFieldValues(ControlsEnum.SOSTATUS);
                        //        if (admConfigMstList != null && admConfigMstList.Count > 0)
                        //        {
                        //            admConfigMstObj = admConfigMstList.SingleOrDefault(con => con.CFG_VALUE == salesOrderHeaderList[e.Row.RowIndex].SOH_STATUS);

                        //            if (admConfigMstObj != null)
                        //            {
                        //                imgApproved.ToolTip = admConfigMstObj.CFG_DATA;
                        //            }
                        //            else
                        //            {
                        //                imgApproved.ToolTip = "";
                        //            }
                        //        }
                        //    }
                        //    else if ((salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.Any(d => d.SOD_QTY_APPROVED > d.SOD_QTY_DISPATCHED)) ||
                        //        (salesOrderHeaderList[e.Row.RowIndex].SAL_ORDER_DTL.Any(i => i.SOD_QTY_APPROVED > i.SOD_QTY_INVOICED)))
                        //    {
                        //        imgApproved.CssClass = GetLocalResourceObject("pending").ToString();
                        //        admConfigMstObj = admConfigMstList.SingleOrDefault(con => con.CFG_VALUE == salesOrderHeaderList[e.Row.RowIndex].SOH_STATUS);
                        //        if (admConfigMstObj != null)
                        //        {
                        //            imgApproved.ToolTip = admConfigMstObj.CFG_DATA;
                        //        }
                        //        else
                        //        {
                        //            imgApproved.ToolTip = "";
                        //        }
                        //        //imgApproved.ToolTip = Resources.Captions.Pending;
                        //    }
                        //    else
                        //    {
                        //        imgApproved.CssClass = GetLocalResourceObject("Completed").ToString();
                        //        admConfigMstObj = admConfigMstList.SingleOrDefault(con => con.CFG_VALUE == salesOrderHeaderList[e.Row.RowIndex].SOH_STATUS);
                        //        //imgApproved.ToolTip = Resources.Captions.Completed;
                        //        if (admConfigMstObj != null)
                        //        {
                        //            imgApproved.ToolTip = admConfigMstObj.CFG_DATA;
                        //        }
                        //        else
                        //        {
                        //            imgApproved.ToolTip = "";
                        //        }
                        //    }
                        //}
                    }
                }
                if ((sender as GridView).ID == "grdPoList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
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
            divCompany.Visible = GetConfigData().IsMultiplePlant;
            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "CUSTOMER");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsSBUCustomer.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
            }

            hdfIsShowInvNo.Value = GetGlobalResourceObject("ConfigurationsRes", "IsShowbtnConvertAll").ToString();//GetGlobalResourceObject("ConfigurationsRes", "IsShowInvNo").ToString();
            hdfIsShowConverted.Value = GetGlobalResourceObject("ConfigurationsRes", "IsShowConverted").ToString(); 
            if (hdfIsShowInvNo.Value == "0")
            {
                lblInvNo.Visible = false;
                txtInvNo.Visible = false;
                grdSoList.Columns[11].Visible = false;
            }
        }
        #endregion
        private void GetUserRights()
        {
            string path = GetLocalResourceObject("wkfBaseURL").ToString();
            UserAuthBL userAuth = new UserAuthBL();
            UserRightsBO usrRights = userAuth.GetUserRights(currentUser.PKUser, path, currentUser.SBUID, currentUser.CurrentDeptPK);
            if (usrRights.Rights.Count > 0)
            {
                for (int i = 0; i < usrRights.Rights.Count; i++)
                {
                    if (usrRights.Rights[i].ActionName == "SHORTCLOSURE" && usrRights.Rights[i].HasActionRight == true && usrRights.Rights[i].UserDeptRight == true)
                    {
                        HasShortCloseRight = true;
                        break;
                    }
                }
            }
        }
        public string GetFormattedRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfRateFormat.Value);
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
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int FillProcessId()
        {
            string path;
            int procId = 0;
            path = GetLocalResourceObject("wkfBaseURL").ToString();
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                PageProcessID = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                procId = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                base.WkfPageUrl = path;
                base.WkfPageType = (int)PageTypeEnum.Listing;
            }
            return procId;
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
                                CustomerID = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value);
                                SoId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOID")).Value);
                                Currency = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOCurrency")).Value);
                                break;
                            case ActionsEnum.PICKFORDO:
                                CustomerIDForDO = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value);
                                SoIdForDO = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOID")).Value);
                                CurrencyForDO = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOCurrency")).Value);
                                break;
                            case ActionsEnum.PICKFORADVANCEINVOICING:
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
        /// <param name="mode"></param>
        private void SetUIEditView(ActionsEnum mode)
        {
            try
            {
                if (mode == ActionsEnum.SALEORDERLISTING)
                {
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                    return;
                }
                foreach (GridViewRow grdrow in grdSoList.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                    // check row selected or not
                    if (rbtn != null && rbtn.Checked)
                    {
                        if (mode == ActionsEnum.PRINTSO)
                        {
                            //When an SC(status:Draft) is selected, and on clicking 'SC No.' hyper link , 'Cancel' and 'Amend' buttons are displaying.
                            GridViewRow gvr;
                            HiddenField hdfWrkfStatus;
                            gvr = (rbtn).Parent.Parent as ExtGridViewRow;
                            hdfWrkfStatus = gvr.FindControl("hdfSOStatus") as HiddenField;
                            if (Convert.ToInt16(hdfWrkfStatus.Value) == 0)
                            {
                                btnEditforCancel.Visible = false;
                            }
                            if (Convert.ToInt16(hdfWrkfStatus.Value) == 2 || Convert.ToInt16(hdfWrkfStatus.Value) == 12 || Convert.ToInt16(hdfWrkfStatus.Value) == 20)
                            {
                                btnAmend.Visible = true;
                            }
                            else
                            {
                                btnAmend.Visible = false;
                            }
                            //rbtn.Checked = false;//Edit for Bug ID:  15812
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                ((HiddenField)grdrow.FindControl("hdfSOID")).Value + "&APPTYPE=" + ApplicationType.SO + "&APPSUBTYPE=") + "');", true);
                        }
                        else if (mode == ActionsEnum.COPY)
                        {
                            SaleContractPk = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOID")).Value);
                            GetFieldValues(ControlsEnum.SOCOPYELIGIBLEDETAILS);
                            if (dtCopyEligibleDtls != null && dtCopyEligibleDtls.Rows.Count > 0)
                            {
                                int TotalItemCount = Convert.ToInt32(dtCopyEligibleDtls.Rows[0]["TOTAL_COUNT"]);
                                int TotalInactiveCount = Convert.ToInt32(dtCopyEligibleDtls.Rows[0]["TOTAL_INACTIVE_COUNT"]);
                                if (TotalInactiveCount > 0) // Inactive items exist
                                {
                                    if (TotalInactiveCount == TotalItemCount) // All item are inactive
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_AllItemsInactive").ToString()) + "');", true);
                                        return;
                                    }
                                    else if (hdfCopyYes.Value == "0")
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowInactiveBrandExist", "$(document).ready(function(){ShowInactiveBrandExist();});", true);
                                        return;
                                    }
                                }
                            }
                            Session[ERP.Utilities.SessionStrings.SALEORDERCOPYPK] = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOID")).Value);
                            Session[ERP.Utilities.SessionStrings.SaleOrderMode] = EntryStatus.NEWMODE;
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.SaleContractDetails), false);
                            return;
                        }
                        else
                        {
                            Session[ERP.Utilities.SessionStrings.SALEORDERPK] = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOID")).Value);
                            if (mode == ActionsEnum.VIEW)
                            {
                                Session[ERP.Utilities.SessionStrings.SaleOrderMode] = EntryStatus.VIEWMODE;
                            }
                            else
                                Session[ERP.Utilities.SessionStrings.SaleOrderMode] = EntryStatus.ENTRYMODE;

                            if (mode == ActionsEnum.SALEORDER || mode == ActionsEnum.EDIT || mode == ActionsEnum.VIEW)
                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.SaleContractDetails), false);
                            else if (mode == ActionsEnum.AMEND)
                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.SaleContractAmendmentDetails), false);
                            else if (mode == ActionsEnum.EDITFORCANCEL)
                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.SaleContractCancel), false);
                            if (mode == ActionsEnum.SALEORDERLISTING)
                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.SaleOrderListing), false);
                        }
                        return;
                    }
                }
                // if no items selected, Show Error Message
                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch
            {
                throw;
            }
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
                        grdSoList.DataSource = salesOrderHeaderList;
                        grdSoList.DataBind();
                        grdPackingHdr.DataSource = null;
                        grdPackingHdr.DataBind();

                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                        break;
                    case ControlsEnum.SHOWSOHDR:
                        grdDOHdr.DataSource = salDespatchHdrList;
                        grdDOHdr.DataBind();
                        break;
                    case ControlsEnum.SHOWPACKINGHDR:
                        grdPackingHdr.DataSource = dtSODtls;
                        grdPackingHdr.DataBind();
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

                case ControlsEnum.FILLWORKFLOWSTATUS:
                    ddlWrokFlowStatus.Items.Clear();
                    if (workflowStatusList != null && workflowStatusList.Count > 0)
                    {
                        ddlWrokFlowStatus.DataSource = workflowStatusList;
                        ddlWrokFlowStatus.DataTextField = "ASC_NAME";
                        ddlWrokFlowStatus.DataValueField = "ASC_VALUE";
                        ddlWrokFlowStatus.DataBind();
                    }
                    ddlWrokFlowStatus.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
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

                #region UPDATECOMPANY
                case ControlsEnum.UPDATECOMPANY:
                    ddlUpdateCompany.Items.Clear();
                    if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    {
                        ddlUpdateCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                        ddlUpdateCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlUpdateCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlUpdateCompany.DataBind();
                    }
                    break;
                #endregion
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
            if (txtCustomer.CssClass != "input-disabled")
            {
                txtCustomer.Text = string.Empty;
                hdfCustomerID.Value = "0";
            }
            txtSONumber.Text = string.Empty;
            hdfSoPK.Value = "0";
            txtCUSPONumber.Text = string.Empty;
            hdfCusPoNumber.Value = "0";
            ddlStatus.ClearSelection();
            ddlWrokFlowStatus.ClearSelection();
            txtFromDate.Text = string.Empty;//DateTime.Now.AddMonths(-1).AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
            hdfFromDate.Value = string.Empty;// DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
            txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            hdfToDate.Value = DateTime.Now.ToString();
            ddlInvoiceType.SelectedIndex = 0;
            ddlCompanyFilter.SelectedIndex = 0;
            //hdfFromDate.Value =string.Empty;
            //txtToDate.Text = string.Empty;
            //hdfToDate.Value = string.Empty;
            txtInvNo.Text = string.Empty;
            hdfInvNo.Value = "0";
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnAmend.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCopy.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
            this.btnPrintSO.PreRender += new EventHandler(btnAction_PreRender);
            this.btnUpdatePlant.PreRender += new EventHandler(btnAction_PreRender);

            this.btnNew.Load += new EventHandler(btnAction_Load);
            this.btnEditforCancel.Load += new EventHandler(btnAction_Load);
            this.btnEdit.Load += new EventHandler(btnAction_Load);
            this.btnAmend.Load += new EventHandler(btnAction_Load);
            this.btnCopy.Load += new EventHandler(btnAction_Load);
            this.btnView.Load += new EventHandler(btnAction_Load);
            this.btnPrintSO.Load += new EventHandler(btnAction_Load);
            this.btnUpdatePlant.Load += new EventHandler(btnAction_Load);

        }
        protected void btnAction_Load(object sender, EventArgs e)
        {
            (sender as Control).Visible = true;
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
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
                if (GetGlobalResourceObject("ConfigurationsRes", "ShowCopy").ToString() == "1")
                {
                    btnCopy.Visible = true;
                }
                else
                {
                    btnCopy.Visible = false;
                }

                base.WkfRefID = listingRefID;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideFilter", "$(document).ready(function(){HideFilter();});", true);
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
            SEARCH,
            TYPECATEGORY,
            CUSTOMERPK,
            SOSTATUS,
            FILLWORKFLOWSTATUS,
            SOTYPE,
            GETPKFROMSCNO,
            SHOWPACKINGHDR,
            SHOWPACKINGDETAILS,
            SOCOPYELIGIBLEDETAILS,
            COMPANY,
            UPDATECOMPANY

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
            SHORTCLOSED = 104

        }
        #endregion
    }
}