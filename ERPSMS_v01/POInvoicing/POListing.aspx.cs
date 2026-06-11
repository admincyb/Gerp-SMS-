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
using System.Threading;
using System.Data;
using BusinessObject.POInvoicing;
using System.Xml;
using BusinessLogic.CommonManagement;
using BusinessObject;

namespace ERPSMS_v01.POInvoicing
{
    public partial class POListing : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        POHeaderBO objPOHeaderItem;

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
        /// isTaxAdded
        /// </summary>
        private string isTaxAdd
        {
            get
            {
                return Convert.ToString(this.ViewState["isTaxAdd"]);
            }
            set
            {
                this.ViewState["isTaxAdd"] = value;
            }
        }
        /// <summary>
        /// isDiscountAdd
        /// </summary>
        private string isDiscountAdd
        {
            get
            {
                return Convert.ToString(this.ViewState["isDiscountAdd"]);
            }
            set
            {
                this.ViewState["isDiscountAdd"] = value;
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
                return this.ViewState[ViewstateStrings.TotalPages] == null ? 1 : (int)this.ViewState[ViewstateStrings.TotalPages];
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPages] = value;
            }
        }
        /// <summary>
        /// PO Type from PO Grid
        /// </summary>
        private byte POGroup
        {
            get
            {
                return (this.ViewState[ViewstateStrings.POGroup] == null ? (byte)0 : Convert.ToByte(this.ViewState[ViewstateStrings.POGroup]));
            }
            set
            {
                this.ViewState[ViewstateStrings.POGroup] = value;
            }
        }

        /// <summary>
        /// For keep Po group for Purchase invoice workflow filling
        /// </summary>
        private POGroup POInvoiceType
        {
            get
            {
                return (Session[ERP.Utilities.SessionStrings.POInvoiceType] == null ? (POGroup)0 : (POGroup)Session[ERP.Utilities.SessionStrings.POInvoiceType]);
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.POInvoiceType] = value;
            }
        }
        /// <summary>
        /// Vendor Pk
        /// </summary>
        private int VendorID
        {
            get
            {
                return this.ViewState[ViewstateStrings.VendorID] == null ? 0 : (int)this.ViewState[ViewstateStrings.VendorID];
            }
            set
            {
                this.ViewState[ViewstateStrings.VendorID] = value;
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
        private int PoId
        {
            get
            {
                return this.ViewState[ViewstateStrings.PoId] == null ? 0 : (int)this.ViewState[ViewstateStrings.PoId];
            }
            set
            {
                this.ViewState[ViewstateStrings.PoId] = value;
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
        private int SelectedPosCount
        {
            get
            {
                return this.ViewState[ViewstateStrings.SelectedPosCount] == null ? 0 : (int)this.ViewState[ViewstateStrings.SelectedPosCount];

            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedPosCount] = value;
            }
        }
        /// <summary>
        /// To maintain keep selected pos
        /// </summary>
        private List<long> SelectedPos
        {
            get
            {
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedPos];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedPos] = value;
            }

        }
        /// <summary>
        /// General Po/ Service PO
        /// </summary>
        private byte SelectedPOGroup
        {
            get
            {
                return (this.ViewState[ViewstateStrings.SelectedPOGroup] == null ? (byte)0 : Convert.ToByte(this.ViewState[ViewstateStrings.SelectedPOGroup]));
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedPOGroup] = value;
            }
        }
        private List<long> SelectedVendors
        {
            get
            {
                return (List<long>)this.ViewState[ViewstateStrings.SelectedVendors];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedVendors] = value;
            }

        }

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

        private List<decimal> SelectedPOsTax
        {
            get
            {
                return (List<decimal>)this.ViewState["SelectedPOsTax"];
            }
            set
            {
                this.ViewState["SelectedPOsTax"] = value;
            }

        }
        private decimal POTax
        {
            get
            {
                return (decimal)this.ViewState["POTax"];
            }
            set
            {
                this.ViewState["POTax"] = value;
            }

        }

        /// <summary>
        /// To keep selected Purchase Order PKs 
        /// </summary>
        private POHeaderBO InvoiceMultiplePOPKs
        {
            set
            {
                this.Session[ERP.Utilities.SessionStrings.InvoiceMultiplePOPKs] = value;
            }
        }
        /// <summary>
        /// keep adm ConfigMst data
        /// </summary>
        private List<ADM_CONFIG_MST> admConfigMstList
        {
            get
            {
                return (List<ADM_CONFIG_MST>)this.ViewState["admConfigMstList"];
            }
            set
            {
                this.ViewState["admConfigMstList"] = value;
            }
        }

        /// <summary>
        /// To maintain Checked PO Pk (multi selection PO from different pages for invoicing)
        /// </summary>
        private List<POHeaderListBO> SelectedPOInfoLst
        {
            get
            {
                return (List<POHeaderListBO>)Session[ERP.Utilities.SessionStrings.SelectedPOInfoLst];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedPOInfoLst] = value;
            }

        }
        /// <summary>
        /// To maintain the pid in Session
        /// </summary>
        private string PIType
        {
            get
            {
                return (string)Session["PIType"];
            }
            set
            {
                Session["PIType"] = value;
            }
        }
        /// <summary>
        /// Is Same PO Issuing Type
        /// </summary>
        private bool IsSamePOIssuingType
        {
            get
            {
                return this.ViewState["IsSamePOIssuingType"] == null ? true : (bool)this.ViewState["IsSamePOIssuingType"];
            }
            set
            {
                this.ViewState["IsSamePOIssuingType"] = value;
            }
        }

        #endregion


        // Indicates the state as well as action
        private ActionsEnum commonActions;

        //page related Entity Object
        private ServiceUtility serviceUtilityObj;
        private PUR_ORDER_HDR objPoHeader;
        private BusinessObject.User currentUser;

        private List<PoHeader> poHeaderList;
        private List<PoDetails> poDetailsList;
        private List<GRNDetails> grnDetailsList;
        private List<GINDetails> ginDetailsList;
        private List<StockTransferDetails> stockTransferList;

        private List<FIN_INVOICE_VND_TRX_MPG> finInvoiceVndTrxMpgList;
        private List<PUR_ORDER_HDR> PurOrderHdrList;
        private List<INV_STK_TRAN_HDR> InvStkTranHdrList;
        private List<INV_GIN_HDR> InvGinHdrList;
        private List<INV_GIN_DTL> InvGinDtlList;
        private List<INV_GRN_DTL> InvGrnDtlList;
        private List<INV_GRN_HDR> InvGrnHdrList;
        private List<PUR_ORDER_DTL> PurOrderDtlList;

        private List<long> SelectedPOList;
        private List<long> SelectedVendorsList;
        private List<long> SelectedCurrencyList;
        private List<decimal> SelectedPOsTaxList;

        private ADM_CONFIG_MST admConfigMstObj;

        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        //private List<ADM_CONFIG_MST> admConfigMstList;

        int grnDtlId;
        int ginId;
        int poItemId;
        private List<FIN_YEAR_MST> finYearMstList;
        private DataTable dtPOList;
        public DataTable dtPoWoDetails;
        private DataTable dtGRNDetails;
        private DataTable dtCompany;
        private DataTable dtDeptbyUserPermission;

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
                    ConfigurationSettings();
                    if (Convert.ToInt32(hdfIsMultiplePlant.Value) == 1)
                    {
                        GetFieldValues(ControlsEnum.PODEPARTMENTSBYUSER);
                        SetFieldValues(ControlsEnum.PODEPARTMENTSBYUSER);
                    }
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);

                    Session[ERP.Utilities.SessionStrings.SelectedPos] = null;
                    InvoiceMultiplePOPKs = null;
                    SelectedPos = null;
                    SelectedPOInfoLst = null;
                    //txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    //hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    //txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    //hdfToDate.Value = DateTime.Now.ToString();
                    txtFromDate.Text = string.Empty;
                    hdfFromDate.Value = string.Empty;
                    txtToDate.Text = string.Empty;
                    hdfToDate.Value = string.Empty;

                    FillProcessID();
                    GetFieldValues(ControlsEnum.TYPECATEGORY);
                    if (hdfServicePORequired.Value == "0")
                        ddlType.Items.Remove(new ListItem(Resources.Captions.Service.ToString(), "2"));
                    if (PIType == "21")
                        ddlType.SelectedIndex = ddlType.Items.IndexOf(ddlType.Items.FindByValue("2"));
                    else
                        ddlType.SelectedIndex = ddlType.Items.IndexOf(ddlType.Items.FindByValue("1"));
                    if (GetGlobalResourceObject("ConfigurationsRes", "EnableWorkOrderItem").ToString() == "1")
                    {
                        ddlType.Items.RemoveAt(1);
                        ddlType.Items.Insert(1, new ListItem(Resources.Captions.WorkOrder, "6"));
                    }
                    else
                        ddlType.Enabled = PIType == null ? true : false;

                    //GetFieldValues(ControlsEnum.FINPERIOD);
                    //SetFieldValues(ControlsEnum.FINPERIOD);

                    //GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    //  BindGrid(ControlsEnum.EMPTYGRID);
                    grdPOItems.DataSource = null; //For resolving Bug ID:  18016
                    grdPOItems.DataBind();
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;

                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;

                    EntryStatus = EntryStatus.LISTMODE;
                }
                //po listing after po invoicing
                if (SelectedPos != null)
                {
                    SelectedPosCount = SelectedPos.Count;
                    if (SelectedPosCount > 0)
                    {
                        btnPickForAdvInv.Text = GetLocalResourceObject("PickForAdvanceInvoicing").ToString() + "(" + SelectedPosCount.ToString() + ")";
                    }
                }
                if (Session["PURCHASEORDERPK"] != null)
                    btnPickForInvoice.Text = GetLocalResourceObject("PickPoForInvoicing").ToString() + "(1)";

                //PLANT Visibility
                if (Convert.ToInt32(hdfIsMultiplePlant.Value) == 1)
                {
                    lblCompany.Visible = true;
                    ddlCompany.Visible = true;
                    lblUserPODepartments.Visible = true;
                    ddlUserPODepartments.Visible = true;
                }
                else
                {
                    lblCompany.Visible = false;
                    ddlCompany.Visible = false;
                    lblUserPODepartments.Visible = false;
                    ddlUserPODepartments.Visible = false;
                }

                if (Convert.ToInt32(hdfIsShowCusPoNo.Value) == 1)
                {
                    grdPoList.Columns[7].Visible = true;
                }
                else
                {
                    grdPoList.Columns[7].Visible = false;
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
            AdmCompanyMstService admCompanyMstServiceClient;

            POListService poListServiceClient;
            poListServiceClient = null;
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            FinTrxService finTrxServiceClient;
            finTrxServiceClient = null;

            try
            {
                switch (type)
                {
                    #region Purchase Order List
                    case ControlsEnum.DEFAULT:
                        int POH_PK = 0, POH_GROUP, POH_VENDOR = 0, TRX_STATUS, POH_BIZUNIT, P_IsEnableWo, POH_COMPANY, POH_DEPT = 0;
                        DateTime? FromDate = null;
                        DateTime? ToDate = null;
                        string SOH_No = "";
                        if (hdfPoPK.Value != "") POH_PK = Convert.ToInt32(hdfPoPK.Value);
                        POH_GROUP = Convert.ToByte(ddlType.SelectedValue);
                        if (hdfVendorID.Value != "") POH_VENDOR = Convert.ToInt32(hdfVendorID.Value);
                        TRX_STATUS = Convert.ToInt32(ddlStatus.SelectedValue);
                        POH_BIZUNIT = currentUser.SBUID;
                        POH_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                        POH_DEPT = string.IsNullOrEmpty(ddlUserPODepartments.SelectedValue) ? 0 : Convert.ToInt32(ddlUserPODepartments.SelectedValue);
                        if (!string.IsNullOrEmpty(hdfFromDate.Value)) FromDate = DateTime.Parse(hdfFromDate.Value);
                        if (!string.IsNullOrEmpty(hdfToDate.Value)) ToDate = DateTime.Parse(hdfToDate.Value);
                        SOH_No = txtIONo.Text.Trim();
                        P_IsEnableWo = Convert.ToInt16(hdfEnableWO.Value);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        serviceUtilityObj.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        serviceUtilityObj.TotalRecords = 0;

                        dtPOList = new DataTable();
                        dtPOList = BusinessLogic.POInvoicing.POInvoiceBL.GetPurchaseOrderList(POH_PK, 1, POH_GROUP, POH_VENDOR, TRX_STATUS, POH_BIZUNIT, FromDate, ToDate, SOH_No, serviceUtilityObj.CurrentPage, serviceUtilityObj.PageSize, POH_COMPANY, P_IsEnableWo, POH_DEPT, currentUser.PKUser);

                        serviceUtilityObj.TotalRecords = dtPOList.Rows.Count > 0 ? Convert.ToInt32(dtPOList.Rows[0]["TOTAL_ROW_COUNT"].ToString()) : 0;
                        TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                    (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                    (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;

                        break;
                    #endregion
                    #region Purchase Order Details List
                    case ControlsEnum.PODETAILS:
                        //old
                        //poListServiceClient = new POListService();
                        //poListServiceClient = CommonFunctions.InitiateClient(poListServiceClient);
                        //serviceUtilityObj = new ServiceUtility();
                        //PurOrderDtlList = poListServiceClient.GetPoDetails(PoId, serviceUtilityObj);
                        dtPoWoDetails= BusinessLogic.POInvoicing.POInvoiceBL.GetOrderDetails(Convert.ToInt16(PoId),POGroup);
                        break;
                    #endregion
                    #region GRN List
                    case ControlsEnum.GRN:
                        //old code
                        //poListServiceClient = new POListService();
                        //poListServiceClient = CommonFunctions.InitiateClient(poListServiceClient);
                        //serviceUtilityObj = new ServiceUtility();
                        //InvGrnDtlList = poListServiceClient.GetGRNDetailsList(PoDtlPK, serviceUtilityObj);
                        dtGRNDetails= BusinessLogic.POInvoicing.POInvoiceBL.GetGRNDetails(Convert.ToInt16(PoDtlPK), POGroup);
                        break;
                    #endregion
                    #region GIN List
                    case ControlsEnum.GIN:
                        poListServiceClient = new POListService();
                        poListServiceClient = CommonFunctions.InitiateClient(poListServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        InvGinDtlList = poListServiceClient.GetGINDetailsList(grnDtlId, serviceUtilityObj);
                        break;
                    #endregion
                    #region Stock Transfer List
                    case ControlsEnum.STOCKTRANSFER:
                        poListServiceClient = new POListService();
                        poListServiceClient = CommonFunctions.InitiateClient(poListServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        InvStkTranHdrList = poListServiceClient.GetStockTransferDetails(ginId, serviceUtilityObj);
                        break;
                    #endregion
                    #region PO Type
                    case ControlsEnum.TYPECATEGORY:
                        CommonServiceClient = new CommonService();
                        admConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONFIG_MST>();
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admConfigMstObj.CFG_TYPE = GetLocalResourceObject("AccountType").ToString();
                        admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    #endregion
                    #region Fin Transaction Year
                    case ControlsEnum.FINPERIOD:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                        finYearMstList = finTrxServiceClient.GetCurrentFinPeriod(DateTime.Now, currentUser.SBUID);
                        break;
                    #endregion                  
                    #region PODEPARTMENTSBYUSER
                    case ControlsEnum.PODEPARTMENTSBYUSER:
                        dtDeptbyUserPermission = BusinessLogic.CommonManagement.CommonBL.GetDepartmentsWithUserPermission(Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID, currentUser.PKUser);
                        break;
                    #endregion
                    #region Company
                    case ControlsEnum.COMPANY:
                        #region Old Code
                        //////gets Company List
                        //admCompanyMstServiceClient = new AdmCompanyMstService();
                        //admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        //admCompanyMstObj.CMP_ACTIVE = 1;
                        //serviceUtilityObj = new ServiceUtility();
                        //admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);

                        ////dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyMappingDetails(0, Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID, Convert.ToInt32(hdfDeptID.Value)); 
                        #endregion
                        int selDeptPk = 0;
                        selDeptPk = string.IsNullOrEmpty(ddlUserPODepartments.SelectedValue) ? 0 : Convert.ToInt32(ddlUserPODepartments.SelectedValue);
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyMappingDetails(0, Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID, selDeptPk);
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
                poListServiceClient = null;
                CommonServiceClient = null;
                finTrxServiceClient = null;
                serviceUtilityObj = null;
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
                    #region Bind PO Grids
                    case ControlsEnum.DEFAULT:
                        BindGrid(controlType);
                        BindGrid(ControlsEnum.GRN);
                        break;
                    #endregion
                    #region GRN Grid
                    case ControlsEnum.GRN:
                        BindGrid(controlType);
                        break;
                    #endregion
                    #region PO Details
                    case ControlsEnum.PODETAILS:
                        BindGrid(controlType);
                        break;
                    #endregion
                    #region Fin Transaction Year
                    case ControlsEnum.FINPERIOD:
                        if (finYearMstList != null && finYearMstList.Count > 0)
                        {
                            txtFromDate.Text = finYearMstList[0].FYR_DATE_FROM.ToString(Resources.Constants.DateFormatShort);
                            hdfFromDate.Value = finYearMstList[0].FYR_DATE_FROM.ToString(Resources.Constants.DateFormatShort);
                            txtToDate.Text = finYearMstList[0].FYR_DATE_TO.ToString(Resources.Constants.DateFormatShort);
                            hdfToDate.Value = finYearMstList[0].FYR_DATE_TO.ToString(Resources.Constants.DateFormatShort);
                        }
                        break;
                    #endregion

                    #region PODEPARTMENTSBYUSER
                    case ControlsEnum.PODEPARTMENTSBYUSER:
                        BindDropDownList(ControlsEnum.PODEPARTMENTSBYUSER);
                        break;
                    #endregion
                    #region Company
                    case ControlsEnum.COMPANY:
                        //Bind Company List in DDL
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
            GridViewRow gvr;
            GridView grd;
            ExtGridView egrd;
            string arg;
            string poPK;
            int RptSubType;

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
            else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
            {
                if (((DropDownList)sender).ID == "ddlUserPODepartments")
                {
                    commonActions = ActionsEnum.CHANGETYPE;
                }
            }
            switch (commonActions)
            {
                #region extra grid ondemand data population
                case ActionsEnum.PODETAILS:
                    arg = ((Button)sender).CommandArgument;
                    gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                    if (gvr != null)
                    {
                        grd = gvr.FindControl("grdOrderDetails") as GridView;
                        POGroup = Convert.ToByte((gvr.FindControl("hdfPOHGROUPVALUE") as HiddenField).Value);
                        if (string.IsNullOrEmpty(arg))
                        {
                            poDetailsList = null;
                        }
                        else
                        {
                            PoId = Convert.ToInt32(arg);
                            GetFieldValues(ControlsEnum.PODETAILS);
                        }
                        grd.Visible = true;
                        if (dtPoWoDetails != null && dtPoWoDetails.Rows.Count > 0)
                        {
                            grd.DataSource = dtPoWoDetails;
                            grd.DataBind();
                        }

                        (gvr.FindControl("hdfIsExpandedOrders") as HiddenField).Value = "1";
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ItemSelect", "ItemListSelection();", true);
                    }
                    break;
                case ActionsEnum.GRNDETAILS:
                    EntryStatus = EntryStatus.VIEWMODE;
                    arg = ((Button)sender).CommandArgument;
                    gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                    if (gvr != null)
                    {
                        grd = gvr.FindControl("grdGRNList") as GridView;
                        if (string.IsNullOrEmpty(arg))
                        {
                            grnDetailsList = null;
                        }
                        else
                        {
                            PoDtlPK = Convert.ToInt32(arg);
                            GetFieldValues(ControlsEnum.GRN);
                        }
                        grd.Visible = true;
                        if (dtGRNDetails != null && dtGRNDetails.Rows.Count > 0)
                        {
                            grd.DataSource = dtGRNDetails;
                            grd.DataBind();
                        }
                        (gvr.FindControl("hdfIsExpandedPOItem") as HiddenField).Value = "1";
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ItemSelect", "ItemListSelection();", true);
                    }
                    break;
                case ActionsEnum.GINDETAILS:
                    arg = ((Button)sender).CommandArgument;
                    gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                    if (gvr != null)
                    {
                        grd = gvr.FindControl("grdGINList") as GridView;
                        if (string.IsNullOrEmpty(arg))
                        {
                            ginDetailsList = null;
                        }
                        else
                        {
                            grnDtlId = Convert.ToInt32(arg);
                            GetFieldValues(ControlsEnum.GIN);
                        }
                        grd.Visible = true;
                        if (InvGinDtlList != null && InvGinDtlList.Count > 0)
                        {
                            grd.DataSource = InvGinDtlList;
                            grd.DataBind();
                        }
                        (gvr.FindControl("hdfIsExpandedGrn") as HiddenField).Value = "1";
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ItemSelect", "ItemListSelection();", true);
                    }
                    break;
                case ActionsEnum.STDETAILS:
                    arg = ((Button)sender).CommandArgument;
                    gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                    if (gvr != null)
                    {
                        grd = gvr.FindControl("grdStockTransfer") as GridView;
                        if (string.IsNullOrEmpty(arg))
                        {
                            stockTransferList = null;
                        }
                        else
                        {
                            ginId = Convert.ToInt32(arg);
                            Session["gnid"] = ginId.ToString();
                            GetFieldValues(ControlsEnum.STOCKTRANSFER);
                        }
                        grd.Visible = true;
                        if (InvStkTranHdrList != null && InvStkTranHdrList.Count > 0)
                        {
                            grd.DataSource = InvStkTranHdrList;
                            grd.DataBind();
                        }
                        (gvr.FindControl("hdfIsExpandedGinList") as HiddenField).Value = "0";
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ItemSelect", "ItemListSelection();", true);
                    }
                    break;
                #endregion
                #region Search
                case ActionsEnum.SEARCH:
                    SelectedPOInfoLst = null;
                    PageIndex = "1";
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    grdPOItems.DataSource = null;
                    grdPOItems.DataBind();
                    break;
                #endregion
                #region Show Grn,Gin St details
                case ActionsEnum.SHOWDETAILS:
                    gvr = ((RadioButton)sender).Parent.Parent as ExtGridViewRow;
                    PoId = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfPOID")).Value);
                    VendorID = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfVendorPK")).Value);
                    Currency = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfPOCurrency")).Value);
                    POGroup = Convert.ToByte(((HiddenField)gvr.FindControl("hdfPOGroup")).Value);
                    POTax = Convert.ToDecimal(string.IsNullOrEmpty(((HiddenField)gvr.FindControl("hdfPOTaxAmount")).Value) ? "0" : ((HiddenField)gvr.FindControl("hdfPOTaxAmount")).Value);
                    Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)gvr.FindControl("hdfVendorPK")).Value;
                    Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)gvr.FindControl("lblVendor")).Text;
                    GetFieldValues(ControlsEnum.PODETAILS);
                    SetFieldValues(ControlsEnum.PODETAILS);
                    break;

                case ActionsEnum.POITEMDETAILS:
                    hdfSelectedItemPOPK.Value = "0";
                    gvr = ((ImageButton)sender).Parent.Parent as ExtGridViewRow;
                    PoId = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfPOID")).Value);
                    VendorID = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfVendorPK")).Value);
                    Currency = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfPOCurrency")).Value);
                    POGroup = Convert.ToByte(((HiddenField)gvr.FindControl("hdfPOGroup")).Value);
                    POTax = Convert.ToDecimal(string.IsNullOrEmpty(((HiddenField)gvr.FindControl("hdfPOTaxAmount")).Value) ? "0" : ((HiddenField)gvr.FindControl("hdfPOTaxAmount")).Value);
                    Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)gvr.FindControl("hdfVendorPK")).Value;
                    Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)gvr.FindControl("lblVendor")).Text;
                    GetFieldValues(ControlsEnum.PODETAILS);
                    SetFieldValues(ControlsEnum.PODETAILS);
                    hdfSelectedItemPOPK.Value = PoId.ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ItemSelect", "ItemListSelection();", true);
                    break;
                #endregion
                #region reset form
                case ActionsEnum.CLEAR:
                    SelectedPOInfoLst = null;
                    ResetForm();
                    PageIndex = "1";
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    grdPOItems.DataSource = null;
                    grdPOItems.DataBind();
                    break;
                #endregion
                #region Tabs
                case ActionsEnum.INVOICE:
                    CheckUserRightsAndRedirect(Resources.PageURL.PoInvoicing);
                    // Response.Redirect(Resources.PageURL.PoInvoicing);
                    break;
                case ActionsEnum.EXPENSES:
                    CheckUserRightsAndRedirect(Resources.PageURL.ExpenseInvoice);
                    //Response.Redirect(Resources.PageURL.ExpenseInvoice);
                    break;
                case ActionsEnum.PAYMENT:
                    CheckUserRightsAndRedirect(Resources.PageURL.PoPayment);
                    //Response.Redirect(Resources.PageURL.PoPayment);
                    break;
                case ActionsEnum.DEFAULT:
                    CheckUserRightsAndRedirect(Resources.PageURL.PoListing);
                    //Response.Redirect(Resources.PageURL.PoListing);
                    break;
                case ActionsEnum.CRDRNOTE:
                    Session[ERP.Utilities.SessionStrings.CrDrType] = ApplicationType.PI;
                    CheckUserRightsAndRedirect(Resources.PageURL.DrCrNote);
                    //Response.Redirect(Resources.PageURL.DrCrNote);
                    break;
                case ActionsEnum.ACPAYABLES:
                    // Response.Redirect(Resources.PageURL.AccountsPayable);
                    foreach (GridViewRow grdrow in grdPoList.Rows)
                    {
                        CheckBox chkPIselect;
                        chkPIselect = (CheckBox)grdrow.FindControl("chkPOselect");
                        if (chkPIselect.Checked)
                        {
                            Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                            Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).ToolTip;//For Showing name in vendorddl of AccountPayable Page Completely 
                            break;
                        }
                    }
                    CheckUserRightsAndRedirect(Resources.PageURL.AccountsPayable);
                    //Response.Redirect(Resources.PageURL.AccountsPayable);
                    break;
                case ActionsEnum.POINVOICE:
                    if (PIType == "21")
                        CheckUserRightsAndRedirect(Resources.PageURL.PurchaseOrderInvoicingService);
                    else
                        CheckUserRightsAndRedirect(Resources.PageURL.PurchaseOrderInvoicing);
                    //Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.PurchaseOrderInvoicing), false);
                    break;
                #endregion
                #region Pick For Invoicing
                case ActionsEnum.PICKFORINVOICING:
                    //Pick for Multiple PO
                    if (hdfIsMultiplePO.Value == "1")
                    {
                        IList<int> MenuType = new List<int>();
                        foreach (GridViewRow item in grdPoList.Rows)
                        {
                            CheckBox chkPOselect;
                            chkPOselect = (CheckBox)item.FindControl("chkPOselect");
                            if (chkPOselect.Checked)
                                MenuType.Add(Convert.ToInt32(((HiddenField)item.FindControl("hdfMenuType")).Value));
                        }

                        if (MenuType.Distinct().Skip(1).Any()) // Check all list item are same
                        {
                            litErrorMsg.Text = GetLocalResourceObject("SelectSameTypePO").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            return;
                        }

                        int result = 0;
                        objPOHeaderItem = (POHeaderBO)SetMultiplePOValuestoObject(commonActions);
                        if (objPOHeaderItem != null && objPOHeaderItem.POList != null)
                        {
                            if (objPOHeaderItem.POList.Count > 0)
                            {
                                if (PIType == "21" && IsSamePOIssuingType == false) // If select PO for service invoice and issuing type is not same
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("SelectSameDeptPO").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    POInvoiceType = BusinessObject.CommonManagement.POGroup.Other;
                                    InvoiceMultiplePOPKs = null;
                                    return;
                                }
                                if (GetGlobalResourceObject("ConfigurationsRes", "EnableWorkOrderItem").ToString() == "1")
                                {
                                    if (objPOHeaderItem.POList != null && objPOHeaderItem.POList.Count > 0)
                                        objPOHeaderItem.POH_GROUP = objPOHeaderItem.POList[0].POHGROUP.ToString();

                                    //objPOHeaderItem.POH_GROUP = Convert.ToInt32(BusinessObject.CommonManagement.POGroup.Workorder).ToString();//POInvoiceType.ToString(),ddlType.SelectedValue.ToString();
                                }

                                string xmlDoc = CommonFunctions.XmlSerialize<POHeaderBO>(objPOHeaderItem);
                                result = BusinessLogic.POInvoicing.POInvoiceBL.CheckforValidPO(xmlDoc);
                                if (result < 0)
                                {
                                    if (result == -3) //NO POs SELECTED
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("NoItemPickforInvoicing").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -4) //DIFF VENDOR
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Vendor").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -13) //DIFF INVESTOR
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Investor").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }

                                    else if (result == -5) //DIFF TYPE
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Type").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -6) //DIFF CURRENCY
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Currency").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -7) //DIFF TAX / DISCOUNT
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_DiffTaxType").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -8) //CUSTOME TAX / DISCOUNT
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_CustomTaxType").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -9) //MULTIPLE TAX / DISCOUNT
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_HeaderTaxType").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -10) //DISCOUNT EXISTS
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_DiscountExist").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -11) //Different Plants
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_MultiplePlant").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -12) //ShortClosed Non stock PO.
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("NonStockPOShortclosed").ToString();//This PO is already short closed,the system will not allow further transaction
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else //Default 
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    POInvoiceType = BusinessObject.CommonManagement.POGroup.Other;
                                    InvoiceMultiplePOPKs = null;
                                    btnPickForInvoice.Text = GetLocalResourceObject("PickPoForInvoicing").ToString();
                                }
                                else
                                {
                                    InvoiceMultiplePOPKs = objPOHeaderItem;
                                    btnPickForInvoice.Text = GetLocalResourceObject("PickPoForInvoicing").ToString() + "(" + objPOHeaderItem.POList.Count + ")";
                                    if (PIType == "21")
                                        Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.PurchaseOrderInvoicingService), false);
                                    else
                                        Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.PurchaseOrderInvoicing), false);
                                }
                            }
                            else //NO POs SELECTED
                            {
                                InvoiceMultiplePOPKs = null;
                                btnPickForInvoice.Text = GetLocalResourceObject("PickPoForInvoicing").ToString();
                                litErrorMsg.Text = GetLocalResourceObject("NoItemPickforInvoicing").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                    }
                    //Pick for Single PO
                    else
                    {
                        SetUIValuesToObject(commonActions);
                    }
                    break;
                case ActionsEnum.PICKFORADVANCEINVOICING:
                    //Adv Invoice Pick for Multiple PO using checkbox
                    if (hdfIsMultiplePO.Value == "1")
                    {
                        int result = 0;
                        SelectedPOList = new List<long>();
                        SelectedPos = null;
                        objPOHeaderItem = (POHeaderBO)SetMultiplePOValuestoObject(commonActions);
                        if (objPOHeaderItem != null && objPOHeaderItem.POList != null)
                        {
                            if (objPOHeaderItem.POList.Count > 0)
                            {
                                int POH_GROUP = 0;
                                if (GetGlobalResourceObject("ConfigurationsRes", "EnableWorkOrderItem").ToString() == "1")
                                {
                                    if (objPOHeaderItem.POList != null && objPOHeaderItem.POList.Count > 0)
                                    {
                                        objPOHeaderItem.POH_GROUP = objPOHeaderItem.POList[0].POHGROUP.ToString();
                                        POH_GROUP = objPOHeaderItem.POH_GROUP == "" ? 0 : Convert.ToInt32(objPOHeaderItem.POH_GROUP);
                                    }
                                    //objPOHeaderItem.POH_GROUP = Convert.ToInt32(BusinessObject.CommonManagement.POGroup.Workorder).ToString();//POInvoiceType.ToString(),ddlType.SelectedValue.ToString();
                                }
                                string xmlDoc = CommonFunctions.XmlSerialize<POHeaderBO>(objPOHeaderItem);
                                result = BusinessLogic.POInvoicing.POInvoiceBL.CheckforValidPO(xmlDoc);
                                if (result < 0)
                                {
                                    if (result == -3) //NO POs SELECTED
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("NoItemPickforInvoicing").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -4) //DIFF VENDOR
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Vendor").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -13) //DIFF INVESTOR
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Investor").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -5) //DIFF TYPE
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Type").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -6) //DIFF CURRENCY
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Currency").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -7) //DIFF TAX / DISCOUNT
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_DiffTaxType").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -8) //CUSTOME TAX / DISCOUNT
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_CustomTaxType").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -9) //MULTIPLE TAX / DISCOUNT
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_HeaderTaxType").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -10) //DISCOUNT EXISTS
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_DiscountExist").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -11) //Different Plants
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_MultiplePlant").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (result == -12) //ShortClosed Non stock PO.
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("NonStockPOShortclosed").ToString();//This PO is already short closed,the system will not allow further transaction
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else //Default 
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }

                                    SelectedPOList = new List<long>();
                                    SelectedPos = null;
                                    btnPickForAdvInv.Text = GetLocalResourceObject("PickForAdvanceInvoicing").ToString();
                                }
                                else
                                {
                                    //SelectedPOList = objPOHeaderItem.POList;
                                    Session[ERP.Utilities.SessionStrings.JOURNALIZETAB_SELECTED_PK] = null;
                                    Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                                    btnPickForAdvInv.Text = GetLocalResourceObject("PickForAdvanceInvoicing").ToString() + "(" + objPOHeaderItem.POList.Count + ")";
                                    string url = Resources.PageURL.PoInvoicing;
                                    if (POH_GROUP > 0)
                                        url += "&POHGROUP=" + POH_GROUP;
                                    Response.Redirect(url, false);
                                }
                            }
                            else //NO POs SELECTED
                            {
                                SelectedPOList = new List<long>();
                                SelectedPos = null;
                                btnPickForAdvInv.Text = GetLocalResourceObject("PickForAdvanceInvoicing").ToString();
                                litErrorMsg.Text = GetLocalResourceObject("NoItemPickforInvoicing").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                    }
                    //Adv Invoice Pick for Multiple PO using radiobutton
                    else
                    {
                        SetUIValuesToObject(commonActions);
                    }
                    break;
                #endregion
                #region Reset Selection
                case ActionsEnum.RESET:
                    SelectedPOInfoLst = null;
                    Session["PURCHASEORDERPK"] = null;
                    Session[ERP.Utilities.SessionStrings.SelectedPos] = null;
                    SelectedCurrency = new List<long>();
                    Currency = 0;
                    SelectedVendors = new List<long>();
                    VendorID = 0;
                    SelectedPOGroup = 0;
                    POGroup = 0;

                    SelectedPos = new List<long>();
                    PoId = 0;
                    SelectedPOsTax = null;
                    SelectedPOsTaxList = new List<decimal>();
                    SelectedCurrencyList = new List<long>();
                    SelectedVendorsList = new List<long>();
                    SelectedPOList = new List<long>();
                    SelectedPosCount = 0;
                    InvoiceMultiplePOPKs = null;
                    btnPickForInvoice.Text = GetLocalResourceObject("PickPoForInvoicing").ToString();
                    btnPickForAdvInv.Text = GetLocalResourceObject("PickForAdvanceInvoicing").ToString();
                    SelectedPos = null;
                    break;
                #endregion
                #region Show Popup
                case ActionsEnum.SHOWPOPUP:
                    poPK = ((LinkButton)sender).CommandArgument;
                    GridViewRow gvRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                    string PohGroup = (gvRow.FindControl("hdfPOHGROUPVALUE") as HiddenField).Value;
                    if (Convert.ToInt32(ddlType.SelectedValue) == 2)
                    {
                        RptSubType = 11;
                    }
                    else { RptSubType = 0; }
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ItemSelect", "ItemListSelection();", true);
                    if (PohGroup == "1"|| PohGroup == "2")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + poPK + "&APPTYPE=" + ApplicationType.PO + "&APPSUBTYPE=" + RptSubType) + "');", true);

                    }
                    else //Poh Group 6 WO
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + poPK + "&APPTYPE=" + ApplicationType.SCWO + "&APPSUBTYPE=1") + "');", true);
                    }

                    break;
                #endregion
                #region PO Department Selected Index Change
                case ActionsEnum.CHANGETYPE:
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch(1);});", true);
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
            int sthPK = 0;
            PUR_ORDER_DTL rowPurOrderItem;
            try
            {
                #region Grid Fixed Columns
                if ((sender as GridView).ID == "grdPoList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        GridView grdOrderDetails = e.Row.FindControl("grdOrderDetails") as GridView;
                        string ItemWidth = string.Empty;
                        string ColmWidth = string.Empty;
                        string[] split;
                        bool IsPer = false;
                        int itemWidth = 0;
                        int colmWidth = 0;
                        if (isTaxAdd == "0")
                        {
                            grdOrderDetails.Columns[7].Visible = false;
                            ItemWidth = grdOrderDetails.Columns[1].ItemStyle.Width.ToString();
                            ColmWidth = grdOrderDetails.Columns[7].ItemStyle.Width.ToString();
                            split = ItemWidth.Split('%');
                            IsPer = split.Count() > 1;
                            itemWidth = Convert.ToInt32(split[0]);
                            colmWidth = Convert.ToInt32(((string[])ColmWidth.Split('%'))[0]);
                            itemWidth += colmWidth;
                            if (IsPer)
                                grdOrderDetails.Columns[1].ItemStyle.Width = Unit.Percentage(itemWidth);
                            else
                                grdOrderDetails.Columns[1].ItemStyle.Width = Unit.Pixel(itemWidth);
                        }
                        if (isDiscountAdd == "0")
                        {
                            grdOrderDetails.Columns[6].Visible = false;
                            ItemWidth = grdOrderDetails.Columns[1].ItemStyle.Width.ToString();
                            ColmWidth = grdOrderDetails.Columns[6].ItemStyle.Width.ToString();
                            split = ItemWidth.Split('%');
                            IsPer = split.Count() > 1;
                            itemWidth = Convert.ToInt32(split[0]);
                            colmWidth = Convert.ToInt32(((string[])ColmWidth.Split('%'))[0]);
                            itemWidth += colmWidth;
                            if (IsPer)
                                grdOrderDetails.Columns[1].ItemStyle.Width = Unit.Percentage(itemWidth);
                            else
                                grdOrderDetails.Columns[1].ItemStyle.Width = Unit.Pixel(itemWidth);

                        }

                        Label lblType = e.Row.FindControl("lblType") as Label;
                        HiddenField hdfMenuTypeText = (HiddenField)e.Row.FindControl("hdfMenuTypeText") as HiddenField;
                        // GetFieldValues(ControlsEnum.TYPECATEGORY);

                        if (admConfigMstList != null && admConfigMstList.Count > 0)
                        {
                            admConfigMstObj = admConfigMstList.SingleOrDefault(con => con.CFG_VALUE == Convert.ToInt32(lblType.Text));
                            if (admConfigMstObj != null)
                            {
                                lblType.Text = ERP.Utilities.CommonFunctions.GetShortString(admConfigMstObj.CFG_DATA, 3, "");

                                lblType.ToolTip = admConfigMstObj.CFG_DATA;
                                if (!string.IsNullOrEmpty(hdfMenuTypeText.Value))
                                    lblType.ToolTip += " (" + hdfMenuTypeText.Value + ")";
                            }
                            else
                            {
                                lblType.Text = "";
                                lblType.ToolTip = "";
                            }
                        }

                        Button imgApproved = e.Row.FindControl("imgApproved") as Button;
                        HiddenField hdfTrxStatus = e.Row.FindControl("hdfTrxStatus") as HiddenField;
                        if (hdfTrxStatus != null)
                        {
                            if (Convert.ToInt32(hdfTrxStatus.Value) == 0) //Pending ="0" Completed ="2"
                            {
                                imgApproved.CssClass = GetLocalResourceObject("pending").ToString();
                                imgApproved.ToolTip = Resources.Captions.Pending;
                            }
                            else if (Convert.ToInt32(hdfTrxStatus.Value) == 2)
                            {
                                imgApproved.CssClass = GetLocalResourceObject("Completed").ToString();
                                imgApproved.ToolTip = Resources.Captions.Completed;
                            }
                        }

                        CheckBox chkPOselect = e.Row.FindControl("chkPOselect") as CheckBox;
                        RadioButton rbtSelect = e.Row.FindControl("rbtSelect") as RadioButton;
                        ImageButton imbPOList = e.Row.FindControl("imbPOList") as ImageButton;
                        if (hdfIsMultiplePO.Value == "1")
                        {
                            chkPOselect.Visible = true;
                            rbtSelect.Visible = false;
                            imbPOList.Visible = true;
                        }
                        else
                        {
                            chkPOselect.Visible = false;
                            rbtSelect.Visible = true;
                            imbPOList.Visible = false;
                        }

                    }
                }

                #endregion

                if ((sender as GridView).ID == "grdStockTransfer")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        sthPK = Convert.ToInt32(((HiddenField)e.Row.FindControl("hdfStockTransferPK")).Value);
                        Label lblTranTo = e.Row.FindControl("lblStTransferTo") as Label;
                        Label lblStQuantityUom = e.Row.FindControl("lblStQuantityUom") as Label;

                        List<INV_STK_TRAN_DTL> objList = InvStkTranHdrList[0].INV_STK_TRAN_DTL.Where(aa => aa.SFD_PK == sthPK).ToList();

                        List<INV_STK_TRAN_GIN_MAP> objInvStkTranGinMapList = InvStkTranHdrList[0].INV_STK_TRAN_GIN_MAP.Where(aa => aa.ISG_ST == sthPK
                            && aa.ISG_GIN_DTL == Convert.ToInt32(Session["gnid"].ToString())).ToList();
                        if (objInvStkTranGinMapList != null)
                        {
                            foreach (INV_STK_TRAN_GIN_MAP objItem in objInvStkTranGinMapList)
                                lblStQuantityUom.Text = objItem.INV_GIN_DTL.GID_QTY_ACCEPTED.ToString("N") + " " + objItem.INV_GIN_DTL.INV_UOM_MST.UOM_CODE;
                            Session["gnid"] = null;
                        }

                    }

                }
                if (((GridView)sender).ID == "grdOrderDetails")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        (e.Row.FindControl("hdfHasChildren") as HiddenField).Value = CommonConstants.SELECT_VALUE_ZERO;
                    }
                }
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

        #endregion

        #endregion
        #region Helper Methods
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

        private object SetMultiplePOValuestoObject(ActionsEnum mode)
        {
            object returnObj = null;
            try
            {
                switch (mode)
                {
                    case ActionsEnum.PICKFORINVOICING:
                        #region Multi selection PO from different pages for invoicing
                        SetAllocationDetails();
                        objPOHeaderItem = new POHeaderBO();
                        objPOHeaderItem.POList = new List<POHeaderListBO>();
                        if (Session[ERP.Utilities.SessionStrings.SelectedPOInfoLst] != null)
                        {
                            SelectedPOInfoLst = (List<POHeaderListBO>)Session[ERP.Utilities.SessionStrings.SelectedPOInfoLst];
                        }
                        objPOHeaderItem.POList = SelectedPOInfoLst;
                        returnObj = objPOHeaderItem;
                        #endregion
                        break;

                    case ActionsEnum.PICKFORADVANCEINVOICING:
                        #region OldCode
                        //objPOHeaderItem = new POHeaderBO();
                        //objPOHeaderItem.POList = new List<POHeaderListBO>();
                        //List<POHeaderListBO> objAdvPOItemList = new List<POHeaderListBO>();
                        //SelectedPOList = new List<long>();
                        //foreach (GridViewRow grdrow in grdPoList.Rows)
                        //{
                        //    CheckBox chkPOselect;
                        //    chkPOselect = (CheckBox)grdrow.FindControl("chkPOselect");
                        //    if (chkPOselect.Checked)
                        //    {
                        //        POHeaderListBO objPoList = new POHeaderListBO();
                        //        objPoList.POPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPOID")).Value);
                        //        objAdvPOItemList.Add(objPoList);

                        //        int poPK = objPoList.POPK;  
                        //        SelectedPOList.Add(poPK);                                
                        //    }
                        //}
                        //objPOHeaderItem.POList = objAdvPOItemList;
                        //SelectedPos = SelectedPOList;
                        //returnObj = objPOHeaderItem; 
                        #endregion
                        #region Multi selection PO from different pages for Advance invoicing
                        SetAllocationDetails();
                        objPOHeaderItem = new POHeaderBO();
                        objPOHeaderItem.POList = new List<POHeaderListBO>();
                        if (Session[ERP.Utilities.SessionStrings.SelectedPOInfoLst] != null)
                        {
                            SelectedPOInfoLst = (List<POHeaderListBO>)Session[ERP.Utilities.SessionStrings.SelectedPOInfoLst];
                        }
                        objPOHeaderItem.POList = SelectedPOInfoLst;
                        #region Need checking :Is the below list assigning is required or not.Prev Code contains this assignment,that's why i assign this
                        SelectedPOList = new List<long>();
                        foreach (var item in SelectedPOInfoLst)
                        {
                            int poPK = item.POPK;
                            SelectedPOList.Add(poPK);
                        }
                        SelectedPos = SelectedPOList;
                        #endregion
                        returnObj = objPOHeaderItem;
                        #endregion
                        break;
                }
                return returnObj;
            }
            catch (Exception ex)
            {
                throw ex;
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

                foreach (GridViewRow grdrow in grdPoList.Rows)
                {
                    RadioButton rbtn;
                    rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                    if (rbtn.Checked)
                    {
                        bIsChecked = true;
                        break;
                    }
                }

                if (bIsChecked)
                    switch (mode)
                    {
                        #region Pick For Invoicing
                        case ActionsEnum.PICKFORINVOICING:
                            Session["PURCHASEORDERPK"] = PoId;
                            btnPickForInvoice.Text = GetLocalResourceObject("PickPoForInvoicing").ToString() + "(1)";
                            break;
                        #endregion
                        #region Pick For Invoicing
                        case ActionsEnum.PICKFORADVANCEINVOICING:
                            if (!IsMatcingTax(SelectedPOsTax, POTax))
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Tax").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                            else if (!IsSameCurrency(SelectedCurrency, Currency))
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Currency").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                            else if (!IsSameVendor(SelectedVendors, VendorID))
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Vendor").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                            else if (SelectedPOGroup > 0 && SelectedPOGroup != POGroup)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_POType").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                            else if (!IsExixtPk(SelectedPos, PoId))
                            {
                                //Add Tax
                                if (SelectedPOsTax != null)
                                {
                                    SelectedPOsTaxList = SelectedPOsTax;
                                }
                                else
                                {
                                    SelectedPOsTaxList = new List<decimal>();
                                }
                                SelectedPOsTaxList.Add(POTax);
                                SelectedPOsTax = SelectedPOsTaxList;

                                //Add Currencies
                                if (SelectedCurrency != null)
                                {
                                    SelectedCurrencyList = SelectedCurrency;
                                }
                                else
                                {
                                    SelectedCurrencyList = new List<long>();
                                }
                                SelectedCurrencyList.Add(Currency);
                                SelectedCurrency = SelectedCurrencyList;

                                //Add Vendors
                                if (SelectedVendors != null)
                                {
                                    SelectedVendorsList = SelectedVendors;
                                }
                                else
                                {
                                    SelectedVendorsList = new List<long>();
                                }
                                SelectedVendorsList.Add(VendorID);
                                SelectedVendors = SelectedVendorsList;
                                SelectedPOGroup = POGroup;
                                //Add Pos
                                if (SelectedPos != null)
                                {
                                    SelectedPOList = SelectedPos;
                                }
                                else
                                {
                                    SelectedPOList = new List<long>();
                                }
                                SelectedPOList.Add(PoId);
                                SelectedPos = SelectedPOList;
                                SelectedPosCount = SelectedPos.Count;
                                Session[ERP.Utilities.SessionStrings.JOURNALIZETAB_SELECTED_PK] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                                btnPickForAdvInv.Text = GetLocalResourceObject("PickForAdvanceInvoicing").ToString() + "(" + SelectedPosCount.ToString() + ")";


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
                    litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
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
                //foreach (GridViewRow grdrow in grdPoList.Rows)
                //{
                //    RadioButton rbtn;
                //    rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                //    if (rbtn.Checked)
                //    {
                //        switch (mode)
                //        {
                //            case ActionsEnum.POINVOICE:
                //                Session["PURCHASEORDERPK"] = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPOID")).Value);
                //                break;
                //        }
                //    }
                //}
                //Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.PurchaseOrderInvoicing), false);
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
                    case ControlsEnum.DEFAULT:
                        GetFieldValues(ControlsEnum.DEFAULT);

                        uclPaging.TotalPages = TotalPages;
                        PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        grdPoList.DataSource = dtPOList;
                        grdPoList.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                        break;
                    case ControlsEnum.GRN:
                        break;
                    case ControlsEnum.PODETAILS:
                        grdPOItems.DataSource = dtPoWoDetails;
                        grdPOItems.DataBind();
                        break;
                    case ControlsEnum.EMPTYGRID:
                        grdPoList.DataSource = null;
                        grdPoList.DataBind();
                        grdPOItems.DataSource = null;
                        grdPOItems.DataBind();
                        uclPaging.Visible = false;
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
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        ddlCompany.Items.Clear();
                        if (dtCompany != null && dtCompany.Rows.Count > 0)
                        {
                            ddlCompany.DataSource = CommonFunctions.HtmlDecodeDataTable(dtCompany, Resources.DataFieldRes.CMP_DISPLAY_CODE);
                            ddlCompany.DataTextField = Resources.DataFieldRes.CMP_DISPLAY_CODE;
                            ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                            ddlCompany.DataBind();
                        }
                        ddlCompany.Items.Insert(0, new ListItem(Resources.Report.SelectAll, CommonConstants.SELECTVAL));
                        break;
                    #endregion
                    #region PODEPARTMENTSBYUSER
                    case ControlsEnum.PODEPARTMENTSBYUSER:
                        ddlUserPODepartments.Items.Clear();
                        if (dtDeptbyUserPermission != null && dtDeptbyUserPermission.Rows.Count > 0)
                        {
                            ddlUserPODepartments.DataSource = CommonFunctions.HtmlDecodeDataTable(dtDeptbyUserPermission, Resources.DataFieldRes.DepartmentName);//CMP_DISPLAY_NAME
                            ddlUserPODepartments.DataTextField = Resources.DataFieldRes.DepartmentName;
                            ddlUserPODepartments.DataValueField = Resources.DataFieldRes.StoreDropdownID;
                            ddlUserPODepartments.DataBind();
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
        /// Is Same Vendor
        /// </summary>
        /// <param name="vendors"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsSameVendor(List<long> vendors, long pk)
        {

            bool flag = true;
            if (vendors != null)
                foreach (long ven in vendors)
                    if (ven != pk)
                    {
                        flag = false;
                        break;
                    }
            return flag;
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
            txtVendor.Text = string.Empty;
            txtPoNumber.Text = string.Empty;

            hdfPoPK.Value = "0";
            hdfVendorID.Value = "0";
            //txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
            //hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
            //txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            //hdfToDate.Value = DateTime.Now.ToString();
            txtFromDate.Text = string.Empty;
            hdfFromDate.Value = string.Empty;
            txtToDate.Text = string.Empty;
            hdfToDate.Value = string.Empty;

            //GetFieldValues(ControlsEnum.FINPERIOD);
            //SetFieldValues(ControlsEnum.FINPERIOD);
            ddlStatus.SelectedIndex = 1;
            ddlCompany.SelectedIndex = -1;
            txtIONo.Text = string.Empty;
            //ddlType.SelectedIndex = 0;
            if (PIType == "21")
                ddlType.SelectedIndex = ddlType.Items.IndexOf(ddlType.Items.FindByValue("2"));
            else
                ddlType.SelectedIndex = ddlType.Items.IndexOf(ddlType.Items.FindByValue("1"));
            if (GetGlobalResourceObject("ConfigurationsRes", "EnableWorkOrderItem").ToString() == "1")
            {
                ddlType.Items.RemoveAt(1);
                ddlType.Items.Insert(1, new ListItem(Resources.Captions.WorkOrder, "4"));
            }
            else
                ddlType.Enabled = PIType == null ? true : false;
            // <asp:ListItem Text="<%$ Resources:Captions,WorkOrder %>" Value="4"></asp:ListItem>
            //    SearchType.Items.Insert(1, new ListItem(Resources.BindValues.WihNo, "WIH_NO"));

            ddlUserPODepartments.SelectedIndex = -1;
        }

        private void ConfigurationSettings()
        {
            DataTable dt = new DataTable();
            dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("PURCHASE ITEM WISE SETTINGS", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                isTaxAdd = dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "TAX")["ACF_VALUE"].ToString();
                isDiscountAdd = dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "DISCOUNT")["ACF_VALUE"].ToString();
            }

            dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("PURCHASE INVOICE SETTINGS", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                //If 1 Show multiple PO checkbox else hide    
                hdfIsMultiplePO.Value = dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "MULTIPLE PO")["ACF_VALUE"].ToString();
            }
            hdfEnableWO.Value = GetGlobalResourceObject("ConfigurationsRes", "EnableWorkOrderItem").ToString();

            hdfServicePORequired.Value = GetGlobalResourceObject("ConfigurationsRes", "ServicePORequired").ToString();

            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";

            hdfIsShowCusPoNo.Value = GetGlobalResourceObject("ConfigurationsRes", "IsShowCusPoNo").ToString();

            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "VENDOR");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsSBUVendor.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
            }
            dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "PONUMBER");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsSBUPO.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
            }
        }

        #region Grd Status maintains
        //For setting allocation details
        private void SetAllocationDetails()
        {
            if (Session[ERP.Utilities.SessionStrings.SelectedPOInfoLst] != null)
                SelectedPOInfoLst = (List<POHeaderListBO>)Session[ERP.Utilities.SessionStrings.SelectedPOInfoLst];
            else
                SelectedPOInfoLst = new List<POHeaderListBO>();
            POHeaderBO objPOHeaderItem = new POHeaderBO();
            objPOHeaderItem.POList = new List<POHeaderListBO>();
            List<POHeaderListBO> objItemList = new List<POHeaderListBO>();
            IsSamePOIssuingType = true;
            string poIssueType = string.Empty;
            string CurPoIssueType = string.Empty;
            foreach (GridViewRow item in grdPoList.Rows)
            {
                CheckBox chkPOselect;
                chkPOselect = (CheckBox)item.FindControl("chkPOselect");
                POHeaderListBO objPoList = new POHeaderListBO();
                objPoList.POPK = Convert.ToInt32(((HiddenField)item.FindControl("hdfPOID")).Value);
                objPoList.POHGROUP = Convert.ToInt32(((HiddenField)item.FindControl("hdfPOHGROUPVALUE")).Value);
                if (chkPOselect.Checked)
                {
                    POInvoiceType = (POGroup)Convert.ToInt32(((HiddenField)item.FindControl("hdfPOGroup")).Value);

                    if (POInvoiceType == BusinessObject.CommonManagement.POGroup.Services) // If select PO for service invoice
                    {
                        CurPoIssueType = ((HiddenField)item.FindControl("hdfPOHIssueDept")).Value;
                        if (!string.IsNullOrEmpty(poIssueType) && CurPoIssueType != poIssueType)  // If issuing type is not same
                            IsSamePOIssuingType = false;
                        poIssueType = CurPoIssueType;
                    }

                    bool alreadyExists = SelectedPOInfoLst.Exists(itemLst => itemLst.POPK == objPoList.POPK && itemLst.POHGROUP == objPoList.POHGROUP);
                    if (!alreadyExists)
                    {
                        SelectedPOInfoLst.Add(objPoList);
                    }
                }
                else
                {
                    var itemToRemove = SelectedPOInfoLst.SingleOrDefault(r => r.POPK == objPoList.POPK && r.POHGROUP == objPoList.POHGROUP);//If already exist and currently unchecked
                    SelectedPOInfoLst.Remove(itemToRemove);
                }
            }
            Session[ERP.Utilities.SessionStrings.SelectedPOInfoLst] = SelectedPOInfoLst;
        }
        private void SetGridStatus()
        {
            if (Session[ERP.Utilities.SessionStrings.SelectedPOInfoLst] != null)
            {
                SelectedPOInfoLst = (List<POHeaderListBO>)Session[ERP.Utilities.SessionStrings.SelectedPOInfoLst];
                int sodPK;
                foreach (GridViewRow item in grdPoList.Rows)
                {
                    sodPK = Convert.ToInt32(((HiddenField)item.FindControl("hdfPOID")).Value);
                    if (SelectedPOInfoLst.Exists(itemLst => itemLst.POPK == sodPK))
                        ((CheckBox)item.FindControl("chkPOselect")).Checked = true;
                }
            }

        }
        #endregion

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

            btnPickForInvoice.PreRender += new EventHandler(btnAction_PreRender);
            btnPickForAdvInv.PreRender += new EventHandler(btnAction_PreRender);
            btnResetSelection.PreRender += new EventHandler(btnAction_PreRender);
            //lbnPOListing.PreRender += new EventHandler(btnAction_PreRender);
            //lnkInvoicing.PreRender += new EventHandler(btnAction_PreRender);
            //lbnPOInvoice.PreRender += new EventHandler(btnAction_PreRender);
            //lbnExpenses.PreRender += new EventHandler(btnAction_PreRender);
            //lnkPayment.PreRender += new EventHandler(btnAction_PreRender);
            //lnbCrDrNote.PreRender += new EventHandler(btnAction_PreRender);
            //lnbAcPayables.PreRender += new EventHandler(btnAction_PreRender);

            btnPickForInvoice.Load += new EventHandler(btnAction_Load);
            btnPickForAdvInv.Load += new EventHandler(btnAction_Load);
            btnResetSelection.Load += new EventHandler(btnAction_Load);
            //lbnPOListing.Load += new EventHandler(btnAction_Load);
            //lnkInvoicing.Load += new EventHandler(btnAction_Load);
            //lbnPOInvoice.Load += new EventHandler(btnAction_Load);
            //lbnExpenses.Load += new EventHandler(btnAction_Load);
            //lnkPayment.Load += new EventHandler(btnAction_Load);
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
                SetAllocationDetails();
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
                SetGridStatus();
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
            if (EntryStatus == EntryStatus.VIEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowFilter", "$(document).ready(function(){ShowFilter();});", true);
                EntryStatus = EntryStatus.LISTMODE;
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
            PODETAILS,
            GRN,
            GIN,
            STOCKTRANSFER,
            TYPECATEGORY,
            FINPERIOD,
            EMPTYGRID,
            COMPANY,
            PODEPARTMENTSBYUSER

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