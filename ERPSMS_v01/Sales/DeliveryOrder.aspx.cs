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
using System.Linq;
using BusinessObject.CommonManagement;
using System.Data;
using System.Threading;
using CustomControls;
using BusinessObject.SaleOrder;
using BusinessLogic.CommonManagement;
using BusinessObject;

namespace ERPSMS_v01.Sales
{
    public partial class DeliveryOrder : ERP.Store.UI.WorkFlowBasePage
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
        /// Invoice
        /// </summary>
        private string Invoice
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.Invoice];
            }
            set
            {
                this.ViewState[ViewstateStrings.Invoice] = value;
            }
        }
        private int DespatchID
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.DespatchID];
            }
            set
            {
                this.ViewState[ViewstateStrings.DespatchID] = value;
            }
        }
        /// <summary>
        /// Current PK
        /// </summary>
        private int CurrMpgPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrMpgPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrMpgPK] = value;
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
                return (int)this.ViewState[ViewstateStrings.TotalPages];
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
        /// To maintain the SortExpression or then By in viewstate
        /// </summary>
        private string ThenBy
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.ThenBy];
            }
            set
            {
                this.ViewState[ViewstateStrings.ThenBy] = value;
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
        /// To maintain keep selected pos
        /// </summary>
        private List<long> SelectedSosFroDO
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
        /// To maintain keep selected invoices
        /// </summary>
        private List<long> SelectedInvoicesCrDr
        {
            get
            {
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedInvoicesCrDr];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedInvoicesCrDr] = value;
            }

        }

        /// <summary>
        /// To maintain keep PoHeader List
        /// </summary>
        private List<SAL_ORDER_DTL> SalDetailList
        {
            get
            {
                return (List<SAL_ORDER_DTL>)Session[ERP.Utilities.SessionStrings.SalDetailList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SalDetailList] = value;
            }

        }

        /// <summary>
        /// To maintain keep InvoiceMap List
        /// </summary>
        private List<SAL_DESPATCH_DTL> SaleDespatchDtlList
        {
            get
            {
                return (List<SAL_DESPATCH_DTL>)Session[ERP.Utilities.SessionStrings.SaleDespatchDtlList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SaleDespatchDtlList] = value;
            }

        }

        /// <summary>
        /// Approved
        /// </summary>
        private int Approved
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.Approved]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Approved] = value;
            }
        }

        /// <summary>
        /// Approved
        /// </summary>
        private bool Posted
        {
            get
            {
                return Convert.ToBoolean(this.ViewState[ViewstateStrings.Posted]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Posted] = value;
            }
        }

        /// <summary>
        /// Aplication referance ID
        /// </summary>
        private int ReferanceID
        {
            get
            {
                return this.ViewState["ReferanceID"] == null ? 0 : Convert.ToInt32(this.ViewState["ReferanceID"].ToString());
            }
            set
            {
                this.ViewState["ReferanceID"] = value;
            }
        }
        #endregion

        BusinessObject.User currentUser;

        // Indicates the state as well as action
        private ActionsEnum commonActions;
        //page related Entity Object
        private ServiceUtility serviceUtilityObj;
        private SAL_DESPATCH_HDR salDespatchHdrObj;
        private SAL_DESPATCH_DTL salDespatchDtlObj;
        private SAL_ORDER_DTL SalOrderDtlObj;
        private List<SAL_ORDER_DTL> SalOrderDtlList;

        SOHeaderBO objSOitem;

        private SaleOrderService salesOrderServiceClient;
        private SAL_ORDER_HDR objSalesOrderHeader;
        private List<SAL_ORDER_HDR> salesOrderHeaderList;
        private int gonPK;

        private int XmlCount = 0;
        private ADM_CONST_MST admConstMstObj;
        private List<ADM_CONST_MST> admConstMstList;

        private ADM_APP_SUB_TYPE_MST admAppSubTypeMstObj;
        private List<ADM_APP_SUB_TYPE_MST> admAppSubTypeMstList;

        //List for binding details to controls  
        private List<SAL_DESPATCH_HDR> salDespatchHdrList;
        private List<SAL_DESPATCH_DTL> salDespatchDtlList;
        private List<long> SelectedSOListForDO;

        private List<long> SelectedSalesInvoiceList;
        private List<long> SelectedInvoiceCrDrList;

        private ADM_CONFIG_MST admConfigMstObj;
        private List<ADM_CONFIG_MST> admConfigMstList;

        private ADM_APP_CONFIG_MST admAppConfigMstObj;
        private List<ADM_APP_CONFIG_MST> admAppConfigMstList;

        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;

        private List<FIN_INVOICE_CUS_DTL> InvoiceDetailsList;
        FinInvoiceCusDetailService finInvoiceCusDetailServiceClient;

        private string despatchNo;
        private bool updateDespatch;
        private bool isSave;

        private string refID;
        private string inboxFlag;
        private int curDspPK;
        private int scPK;

        DataTable dtFromPort;

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
        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;

                if (!IsPostBack)
                {
                    ConfigurationSettings();
                    Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                    SelectedInvoicesCrDr = null;
                    txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();
                    //txtFromDate.Text = string.Empty;
                    //hdfFromDate.Value = string.Empty;
                    //txtToDate.Text = string.Empty;
                    //hdfToDate.Value = string.Empty; 
                    txtCYDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    //txtFromDate.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    //hdfFromDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    //txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    //hdfToDate.Value = DateTime.Now.ToString();
                    //txtFromDate.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    //hdfFromDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    //txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    //hdfToDate.Value = DateTime.Now.ToString();

                    //Used for Integration purpose
                    FillProcessID();
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;
                    //If Has RefID (from Inbox)
                    if (!string.IsNullOrEmpty(refID))
                    {
                        ReferanceID = int.Parse(refID);
                        if (!string.IsNullOrEmpty(inboxFlag))
                        {
                            ucrWrkf.ViewType = 0;
                            EntryStatus = EntryStatus.VIEWMODE;
                            btnSave.Visible = false;
                            btnSubmit.Visible = false;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        ucrWrkf.RefID = int.Parse(refID);
                        CurrPK = GetApplicationID(ucrWrkf.RefID);
                    }

                    if (CurrPK > 0)
                    {
                        SetUIEditView(commonActions);
                        ModifiedDatePnl.Visible = true;
                        GetFieldValues(ControlsEnum.DELIVERYORDERDETAILS);
                        salDespatchHdrObj = CommonFunctions.Initilize<SAL_DESPATCH_HDR>();
                        salDespatchHdrObj.DPH_PK = CurrPK;
                        GetFieldValues(ControlsEnum.CARRIER);
                        SetFieldValues(ControlsEnum.CARRIER);
                        //SetFieldValues(ControlsEnum.COMPANY);
                        //GetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                        GetUIValuesFromObject(ControlsEnum.DELIVERYORDERDETAILS);
                        SetFieldValues(ControlsEnum.DELIVERYORDERLIST);
                        if (SelectedSosFroDO != null) //if (string.IsNullOrEmpty(lblDeliveryOrderNo.Text))
                        {
                            pnlPrint.Visible = false;
                        }
                        else
                        {
                            pnlPrint.Visible = true;
                        }
                    }
                    else
                    {
                        string[] datakeyarray;
                        datakeyarray = new string[1];
                        datakeyarray[0] = "DPH_PK";
                        grdDeliveryOrderList.DataKeyNames = datakeyarray;
                        //if (Session[ERP.Utilities.SessionStrings.SALEORDERDOPK] != null)
                        //    btnPickForInvoice.Text = GetLocalResourceObject("PickSoForInvoicing").ToString() + "(1)";
                        if (SelectedSosFroDO != null)
                        {
                            SelectedSOListForDO = SelectedSosFroDO;
                            GetFieldValues(ControlsEnum.DELIVERYORDERLIST);
                            SetFieldValues(ControlsEnum.DELIVERYORDERLIST);
                            GetFieldValues(ControlsEnum.CARRIER);
                            SetFieldValues(ControlsEnum.CARRIER);
                            //SetFieldValues(ControlsEnum.COMPANY);
                            GetUIValuesFromObject(ControlsEnum.DELIVERYORDERDETAILS);
                            EntryStatus = EntryStatus.NEWMODE;
                            updateDespatch = false;
                            GetFieldValues(ControlsEnum.INVOICENO);
                            //lblDispInvoiceNo.Text = hdfDespatchNo.Value;
                            if (SelectedSosFroDO != null)
                            {
                                pnlPrint.Visible = false;
                            }
                            else
                            {
                                pnlPrint.Visible = true;
                            }

                            //--- new ---
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false)
                            {
                                EntryStatus = EntryStatus.NEWMODE;
                                ucrWrkf.ViewType = 1;
                                btnSave.Visible = true;
                            }
                            else
                            {
                                SelectedSOListForDO = new List<long>();
                                GetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                                SetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                                EntryStatus = EntryStatus.LISTMODE;
                                ucrWrkf.ViewType = 0;
                                btnSave.Visible = false;
                                btnInspection.Visible = false;
                                btnEdit.Visible = false;
                                btnPrint.Visible = false;
                            }
                            //---- end new ----
                        }
                        else
                        {
                            SelectedSOListForDO = new List<long>();
                            GetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                            SetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                            EntryStatus = EntryStatus.LISTMODE;

                            //--- new ---
                            ucrWrkf.FillWorkFlowDetails();
                            if (!ucrWrkf.HasActions || ucrWrkf.IsWkfCompleted)
                            {
                                ucrWrkf.ViewType = 0;
                                btnSave.Visible = false;
                                btnInspection.Visible = false;
                                btnEdit.Visible = false;
                                btnPrint.Visible = false;
                            }
                            //---- end new ----
                        }
                    }

                    hdfType.Value = ApplicationType.DO;
                }
                hdfCurPk.Value = CurrPK.ToString();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
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
            SaleOrderService saleOrderServiceClient;
            saleOrderServiceClient = null;
            CommonService CommonServiceClient;
            CommonServiceClient = null;

            DeliveryOrderService deliveryOrderServiceClient;
            deliveryOrderServiceClient = null;

            FinTrxService finTrxServiceClient;
            finTrxServiceClient = null;

            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            try
            {
                switch (type)
                {
                    case ControlsEnum.DELIVERYORDERHDR:
                        deliveryOrderServiceClient = new DeliveryOrderService();
                        deliveryOrderServiceClient = CommonFunctions.InitiateClient(deliveryOrderServiceClient);
                        salDespatchHdrObj = CommonFunctions.Initilize<SAL_DESPATCH_HDR>();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdDeliveryOrderList.PageSize;
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? "DPH_DATE" : SortBy;
                        serviceUtilityObj.ThenBy = ThenBy = ThenBy == null ? "DPH_NO" : ThenBy;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;
                        //serviceUtilityObj.FilterBy =HttpUtility.HtmlEncode(string.Empty);// Convert.ToInt32(ddlSearchBy.SelectedValue) > 0 ? ddlSearchBy.Text : HttpUtility.HtmlEncode(string.Empty);
                        //serviceUtilityObj.FilterValue = String.IsNullOrEmpty(txtSearchValue.Text.Trim()) ? HttpUtility.HtmlEncode(string.Empty) : HttpUtility.HtmlEncode(txtSearchValue.Text);


                        salDespatchHdrObj.DPH_COMP = String.IsNullOrEmpty(hdfVendor.Value.Trim()) ? 0 : Convert.ToInt32(hdfVendor.Value);
                        salDespatchHdrObj.DPH_CUSTOMER = String.IsNullOrEmpty(hdfCustomerID.Value.Trim()) ? 0 : Convert.ToInt32(hdfCustomerID.Value);
                        if (hdfCustomerID.Value != null && hdfCustomerID.Value != "0" && hdfCustomerID.Value != "")
                        {
                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = hdfCustomerID.Value;
                            Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = txtCustomer.Text;
                        }
                        salDespatchHdrObj.DPH_PK = String.IsNullOrEmpty(hdfDPHPK.Value.Trim()) ? 0 : Convert.ToInt32(hdfDPHPK.Value);
                        salDespatchHdrObj.DPH_ACTIVE = 1;
                        salDespatchHdrObj.DPH_BIZUNIT = currentUser.SBUID;
                        serviceUtilityObj.FilterDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtFromDate.Text.Trim());
                        serviceUtilityObj.FilterToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtToDate.Text.Trim());
                        serviceUtilityObj.NeedAdvanceFilter = true;
                        salDespatchHdrObj.DPH_STATUS = (byte)WkfStatusEnum.APPROVED;
                        salDespatchHdrList = deliveryOrderServiceClient.GetSaleDespatchHdr(salDespatchHdrObj, serviceUtilityObj);

                        TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                     (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                     (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        break;

                    case ControlsEnum.DELIVERYORDERLIST:
                        saleOrderServiceClient = new SaleOrderService();
                        saleOrderServiceClient = CommonFunctions.InitiateClient(saleOrderServiceClient);
                        SalOrderDtlObj = new SAL_ORDER_DTL();
                        serviceUtilityObj = new ServiceUtility();
                        //SalOrderDtlObj.SOH_ACTIVE = 1;
                        SalOrderDtlList = saleOrderServiceClient.GetSelectedSaleOrderDetails(SelectedSosFroDO, serviceUtilityObj);
                        SalDetailList = SalOrderDtlList;
                        break;
                    case ControlsEnum.DELIVERYORDERDETAILS:
                        deliveryOrderServiceClient = new DeliveryOrderService();
                        deliveryOrderServiceClient = CommonFunctions.InitiateClient(deliveryOrderServiceClient);
                        SalOrderDtlObj = new SAL_ORDER_DTL();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        //SalOrderDtlObj.SOH_ACTIVE = 1;
                        salDespatchDtlList = deliveryOrderServiceClient.GetDespatchedSaleOrders(DespatchID, serviceUtilityObj);
                        SaleDespatchDtlList = salDespatchDtlList;
                        break;
                    case ControlsEnum.DESPATCHNO:
                        //Generate Invoice No
                        deliveryOrderServiceClient = new DeliveryOrderService();
                        deliveryOrderServiceClient = CommonFunctions.InitiateClient(deliveryOrderServiceClient);
                        despatchNo = deliveryOrderServiceClient.GetDespatchNo(ApplicationType.DO, 0, 1,
                            DateTime.Now, currentUser.PKUser, updateDespatch, 0);
                        hdfDespatchNo.Value = despatchNo;
                        break;
                    //case ControlsEnum.EXCHANGERATE:
                    //    //Generate Exchange Rate
                    //    deliveryOrderServiceClient = new DeliveryOrderService();
                    //    deliveryOrderServiceClient = CommonFunctions.InitiateClient(deliveryOrderServiceClient);
                    //    double ExchgRate = deliveryOrderServiceClient.GetConversionFactor(
                    //                                         salDespatchHdrObj.ICH_CURRENCY, salDespatchHdrObj.ICH_BASE_CURR,
                    //                                         salDespatchHdrObj.ICH_DATE, currentUser.SBUID);
                    //    hdfExchangeRate.Value = ExchgRate.ToString();
                    //    break;
                    case ControlsEnum.CARRIER:
                        CommonServiceClient = new CommonService();
                        admConstMstList = CommonServiceClient.GetConstMstValues(null, Convert.ToByte(DbActiveStatus.ACTIVE), null, 13, 5, currentUser.SBUID);
                        break;
                    #region FIN HEADER
                    case ControlsEnum.FINHEADER:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_REF_TYPE = Session[ERP.Utilities.SessionStrings.TransactionType].ToString();
                        finTrxHdrObj.FTH_REF_PK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString());
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrList = finTrxServiceClient.GetfinTxtHdrList(finTrxHdrObj, serviceUtilityObj);
                        break;
                    #endregion
                    case ControlsEnum.PRINTLIST:
                        CommonServiceClient = new CommonService();
                        admAppSubTypeMstObj = CommonFunctions.Initilize<ADM_APP_SUB_TYPE_MST>();
                        admAppSubTypeMstObj.ADM_APP_TYPE_MST = CommonFunctions.Initilize<ADM_APP_TYPE_MST>();
                        admAppSubTypeMstObj.ADM_APP_TYPE_MST.APT_CODE = ApplicationType.DO;
                        admAppSubTypeMstObj.AST_SPL_COND = "RPT";
                        admAppSubTypeMstList = CommonServiceClient.GetADMAPPSUBTYPEMST_Dtls(admAppSubTypeMstObj);
                        admAppSubTypeMstList = admAppSubTypeMstList.OrderBy(c => c.AST_NAME).ToList();
                        break;
                    case ControlsEnum.ENCLOSURECONFIG:
                        CommonServiceClient = new CommonService();
                        admConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONFIG_MST>();
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admConfigMstObj.CFG_TYPE = GetLocalResourceObject("AccountType").ToString();
                        admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    case ControlsEnum.MULTIPLESO:
                        CommonServiceClient = new CommonService();
                        admAppConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_APP_CONFIG_MST>();
                        admAppConfigMstObj.ACF_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admAppConfigMstObj.ACF_DATA = GetLocalResourceObject("MulSO").ToString();
                        admAppConfigMstObj.ACF_SETTING = GetLocalResourceObject("SISetting").ToString();
                        admAppConfigMstList = CommonServiceClient.GetADM_APP_CONFIG_MST(admAppConfigMstObj);
                        break;
                    case ControlsEnum.SOHEADER:
                        salesOrderServiceClient = new SaleOrderService();
                        salesOrderServiceClient = CommonFunctions.InitiateClient(salesOrderServiceClient);
                        objSalesOrderHeader = new SAL_ORDER_HDR();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        serviceUtilityObj.SortBy = Resources.DataFieldRes.SODate;
                        serviceUtilityObj.ThenBy = Resources.DataFieldRes.SONo;
                        serviceUtilityObj.SortDirection = Resources.Report.SortAscending;

                        objSalesOrderHeader.SOH_BIZUNIT = currentUser.SBUID;
                        objSalesOrderHeader.SOH_ACTIVE = 1;
                        if (hdfCustomerID.Value != "")
                            objSalesOrderHeader.SOH_CUSTOMER = Convert.ToInt32(hdfCustomerID.Value);
                        if (gonPK > 0)
                        {
                            salDespatchDtlObj = CommonFunctions.Initilize<SAL_DESPATCH_DTL>();
                            salDespatchDtlObj.DPD_DESPATCH_HDR = gonPK;
                            objSalesOrderHeader.SAL_DESPATCH_DTL = new System.Data.Objects.DataClasses.EntityCollection<SAL_DESPATCH_DTL>();
                            objSalesOrderHeader.SAL_DESPATCH_DTL.Add(salDespatchDtlObj);
                        }
                        salesOrderHeaderList = salesOrderServiceClient.GetSaleOrderHeader(objSalesOrderHeader, serviceUtilityObj, ApplicationSubType.INVOICE, null, PageType.SHIPPING);
                        break;
                    case ControlsEnum.TYPECATEGORY:
                        CommonServiceClient = new CommonService();
                        admConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONFIG_MST>();
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admConfigMstObj.CFG_TYPE = GetLocalResourceObject("SOAccountType").ToString();
                        admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    case ControlsEnum.INVSCQTY:
                        finInvoiceCusDetailServiceClient = new FinInvoiceCusDetailService();
                        finInvoiceCusDetailServiceClient = CommonFunctions.InitiateClient(finInvoiceCusDetailServiceClient);
                        InvoiceDetailsList = finInvoiceCusDetailServiceClient.GetInvoiceCusDtlBySCPK(curDspPK, scPK);
                        break;
                    case ControlsEnum.INVQTY:
                        finInvoiceCusDetailServiceClient = new FinInvoiceCusDetailService();
                        finInvoiceCusDetailServiceClient = CommonFunctions.InitiateClient(finInvoiceCusDetailServiceClient);
                        FIN_INVOICE_CUS_DTL objCusDtl = CommonFunctions.Initilize<FIN_INVOICE_CUS_DTL>();
                        objCusDtl.CID_INVOICE_HDR = Convert.ToInt32(curDspPK);
                        InvoiceDetailsList = finInvoiceCusDetailServiceClient.GetInvoiceCusDtlByPK(objCusDtl);
                        break;

                    #region
                    case ControlsEnum.PORTDETAILS:
                        dtFromPort = BusinessLogic.CommonManagement.CommonBL.GetPortDetailsByPK(Convert.ToInt16(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_FROM_PORT), Convert.ToInt16(DbActiveStatus.HASPK), currentUser.SBUID);
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
                admAppSubTypeMstObj = null;
                CommonServiceClient = null;
                saleOrderServiceClient= null;
                deliveryOrderServiceClient= null;
                finTrxServiceClient= null;
                finInvoiceCusDetailServiceClient = null;
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
                    case ControlsEnum.DELIVERYORDERHDR:
                        BindGrid(ControlsEnum.DELIVERYORDERHDR);
                        break;
                    case ControlsEnum.DELIVERYORDERLIST:
                        BindGrid(ControlsEnum.DELIVERYORDERLIST);
                        break;
                    case ControlsEnum.CARRIER:
                        BindDropDown(ControlsEnum.CARRIER);
                        break;
                    case ControlsEnum.PRINTLIST:
                        BindDropDown(controlType);
                        break;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
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
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int i = 0;
                bool bIsChecked = false;
                int packingSpec = 0;

                switch (controlType)
                {
                    #region Sales Despatch Header
                    case ControlsEnum.SALDESPATCHHDR:

                        salDespatchHdrObj.DPH_PK = CurrPK;
                        //if (CurrPK == 0)
                        //{
                        //    updateDespatch = true;
                        //    GetFieldValues(ControlsEnum.DESPATCHNO);
                        //    lblDeliveryOrderNo.Text = hdfDespatchNo.Value;
                        //}
                        salDespatchHdrObj.DPH_NO = "";
                        if (SalDetailList != null)
                        {
                            SalOrderDtlList = (List<SAL_ORDER_DTL>)SalDetailList;
                            salDespatchHdrObj.DPH_CUSTOMER = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER;
                            salDespatchHdrObj.DPH_REFERENCE = SalOrderDtlList[0].SOD_NO;
                            salDespatchHdrObj.DPH_REF_NO = SalOrderDtlList[0].SOD_NO;

                        }
                        if (SaleDespatchDtlList != null)
                        {
                            salDespatchDtlList = (List<SAL_DESPATCH_DTL>)SaleDespatchDtlList;
                            salDespatchHdrObj.DPH_CUSTOMER = salDespatchDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER;
                            salDespatchHdrObj.DPH_REFERENCE = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_REFERENCE;
                            salDespatchHdrObj.DPH_REF_NO = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_REF_NO;

                        }


                        salDespatchHdrObj.DPH_ENCLOS_TERM_TEXT = txtEnclosure.Text;


                        salDespatchHdrObj.DPH_NO = lblDeliveryOrderNo.Text;
                        salDespatchHdrObj.DPH_TO_PORT = lblDestinationPort.Text;
                        if (!string.IsNullOrEmpty(hdfPortOfLoading.Value))
                        {
                            salDespatchHdrObj.DPH_FROM_PORT = Convert.ToInt32(hdfPortOfLoading.Value);
                        }
                        salDespatchHdrObj.DPH_FINAL_DESTINATION = lblFinalDestination.Text.Trim();
                        if (!string.IsNullOrEmpty(txtCYDate.Text.Trim()))
                        {
                            salDespatchHdrObj.DPH_CY_DATE = Convert.ToDateTime(txtCYDate.Text.Trim());
                            salDespatchHdrObj.DPH_DATE = Convert.ToDateTime(txtCYDate.Text.Trim());
                        }
                        else
                        {
                            salDespatchHdrObj.DPH_DATE = DateTime.Now;
                        }
                        if (!string.IsNullOrEmpty(txtRtnDate.Text.Trim()))
                        {
                            salDespatchHdrObj.DPH_RTN_DATE = Convert.ToDateTime(txtRtnDate.Text.Trim());
                        }
                        if (!string.IsNullOrEmpty(txtETD.Text.Trim()))
                        {
                            salDespatchHdrObj.DPH_ETD = Convert.ToDateTime(txtETD.Text.Trim());
                        }
                        if (!string.IsNullOrEmpty(txtETA.Text.Trim()))
                        {
                            salDespatchHdrObj.DPH_ETA = Convert.ToDateTime(txtETA.Text.Trim());
                        }

                        if (Convert.ToInt32(ddlCarrier.SelectedValue) >= 0)
                        {
                            salDespatchHdrObj.DPH_CARRIER = Convert.ToInt32(ddlCarrier.SelectedValue);
                        }

                        salDespatchHdrObj.DPH_CONTAINER_NO = txtContainer.Text.Trim();
                        salDespatchHdrObj.DPH_SEAL_NO = txtSealNo.Text.Trim();

                        if (!string.IsNullOrEmpty(hdfCompany.Value))
                        {
                            salDespatchHdrObj.DPH_COMP = Convert.ToInt32(hdfCompany.Value);
                        }

                        salDespatchHdrObj.DPH_DRIVER = txtDriver.Text.Trim();
                        salDespatchHdrObj.DPH_LORRY_NO = txtLorryNo.Text.Trim();

                        salDespatchHdrObj.DPH_FEEDER_VESSEL = txtFeederVessel.Text.Trim();
                        salDespatchHdrObj.DPH_MOTHER_VESSEL = txtMotherVessel.Text.Trim();
                        if (!string.IsNullOrEmpty(txtDateOfShipment.Text.Trim()))
                        {
                            salDespatchHdrObj.DPH_SHIPMENT_DATE = Convert.ToDateTime(txtDateOfShipment.Text.Trim());
                        }

                        salDespatchHdrObj.DPH_BOOKING_NO = txtBookingNo.Text.Trim();
                        if (!string.IsNullOrEmpty(txtBookingDate.Text.Trim()))
                        {
                            salDespatchHdrObj.DPH_BOOKING_DATE = Convert.ToDateTime(txtBookingDate.Text.Trim());
                        }
                        salDespatchHdrObj.DPH_SHIPPING_MARK = txtShippingMark.Text.Trim();

                        salDespatchHdrObj.DPH_STATUS = 1;
                        salDespatchHdrObj.DPH_ACTIVE = 1;
                        salDespatchHdrObj.DPH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                        salDespatchHdrObj.DPH_CRTD_DT = DateTime.Now;
                        salDespatchHdrObj.DPH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                        salDespatchHdrObj.DPH_MOD_DT = LastModifiedTime;
                        salDespatchHdrObj.DPH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        salDespatchHdrObj.DPH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        if (!string.IsNullOrEmpty(hdfConsignee.Value))
                            salDespatchHdrObj.DPH_CONSIGNEE = Convert.ToInt16(hdfConsignee.Value);
                        salDespatchHdrObj.DPH_CONSIGNEE_NAME = txtConsignee.Text;
                        if (Session["DPH_CONSIGNEE_ADDRESS"] != null)
                            salDespatchHdrObj.DPH_CONSIGNEE_ADDRESS = Session["DPH_CONSIGNEE_ADDRESS"].ToString();
                        if (Session["DPH_CONSIGNEE_COUNTRY"] != null)
                            salDespatchHdrObj.DPH_CONSIGNEE_COUNTRY = Convert.ToInt16(Session["DPH_CONSIGNEE_COUNTRY"].ToString());
                        if (Session["DPH_CONSIGNEE_FAX"] != null)
                            salDespatchHdrObj.DPH_CONSIGNEE_FAX = Session["DPH_CONSIGNEE_FAX"].ToString();
                        if (Session["DPH_CONSIGNEE_PHONE"] != null)
                            salDespatchHdrObj.DPH_CONSIGNEE_PHONE = Session["DPH_CONSIGNEE_PHONE"].ToString();
                        if (Session["DPH_CONSIGNEE_MOBILE"] != null)
                            salDespatchHdrObj.DPH_CONSIGNEE_MOBILE = Session["DPH_CONSIGNEE_MOBILE"].ToString();
                        if (Session["DPH_CONSIGNEE_EMAIL"] != null)
                            salDespatchHdrObj.DPH_CONSIGNEE_EMAIL = Session["DPH_CONSIGNEE_EMAIL"].ToString();
                        if (Session["DPH_CONSIGNEE_ZIP"] != null)
                            salDespatchHdrObj.DPH_CONSIGNEE_ZIP = Session["DPH_CONSIGNEE_ZIP"].ToString();
                        if (!string.IsNullOrEmpty(hdfNotifyParty.Value))
                            salDespatchHdrObj.DPH_NOTIFY_PARTY = Convert.ToInt16(hdfNotifyParty.Value);
                        salDespatchHdrObj.DPH_NOTIFY_PARTY_NAME = txtNotifyParty.Text;
                        if (Session["DPH_NOTIFY_PARTY_ADDRESS"] != null)
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_ADDRESS = Session["DPH_NOTIFY_PARTY_ADDRESS"].ToString();
                        if (Session["DPH_NOTIFY_PARTY_COUNTRY"] != null)
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_COUNTRY = Convert.ToInt16(Session["DPH_NOTIFY_PARTY_COUNTRY"].ToString());
                        if (Session["DPH_NOTIFY_PARTY_FAX"] != null)
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_FAX = Session["DPH_NOTIFY_PARTY_FAX"].ToString();
                        if (Session["DPH_NOTIFY_PARTY_PHONE"] != null)
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_PHONE = Session["DPH_NOTIFY_PARTY_PHONE"].ToString();
                        if (Session["DPH_NOTIFY_PARTY_MOBILE"] != null)
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_MOBILE = Session["DPH_NOTIFY_PARTY_MOBILE"].ToString();
                        if (Session["DPH_NOTIFY_PARTY_EMAIL"] != null)
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_EMAIL = Session["DPH_NOTIFY_PARTY_EMAIL"].ToString();
                        if (Session["DPH_NOTIFY_PARTY_ZIP"] != null)
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_ZIP = Session["DPH_NOTIFY_PARTY_ZIP"].ToString();
                        if (!string.IsNullOrEmpty(hdfPaymentTerms.Value))
                            salDespatchHdrObj.DPH_PAYMENT_TERM = Convert.ToInt16(hdfPaymentTerms.Value);
                        salDespatchHdrObj.DPH_PAYMENT_TERM_TEXT = txtPaymentTerms.Text;

                        salDespatchHdrObj.DPH_SUPP_DTL = txtSupplimentary.Text;
                        if (!string.IsNullOrEmpty(hdfModeofTransport.Value))
                            salDespatchHdrObj.DPH_SHIP_BY = Convert.ToInt16(hdfModeofTransport.Value);

                        salDespatchHdrObj.DPH_CUSTOMER_NAME = lblDeliveryTo.Text;
                        salDespatchHdrObj.DPH_CUSTOMER_ADDRESS = txtDeliveryAddress.Text.Trim();
                        if (Session["DPH_CUSTOMER_COUNTRY"] != null)
                            salDespatchHdrObj.DPH_CUSTOMER_COUNTRY = Convert.ToInt16(Session["DPH_CUSTOMER_COUNTRY"].ToString());
                        if (Session["DPH_CUSTOMER_FAX"] != null)
                            salDespatchHdrObj.DPH_CUSTOMER_FAX = Session["DPH_CUSTOMER_FAX"].ToString();
                        if (Session["DPH_CUSTOMER_PHONE"] != null)
                            salDespatchHdrObj.DPH_CUSTOMER_PHONE = Session["DPH_CUSTOMER_PHONE"].ToString();
                        if (Session["DPH_CUSTOMER_MOBILE"] != null)
                            salDespatchHdrObj.DPH_CUSTOMER_MOBILE = Session["DPH_CUSTOMER_MOBILE"].ToString();
                        if (Session["DPH_CUSTOMER_EMAIL"] != null)
                            salDespatchHdrObj.DPH_CUSTOMER_EMAIL = Session["DPH_CUSTOMER_EMAIL"].ToString();
                        if (Session["DPH_CUSTOMER_ZIP"] != null)
                            salDespatchHdrObj.DPH_CUSTOMER_ZIP = Session["DPH_CUSTOMER_ZIP"].ToString();



                        salDespatchDtlList = new List<SAL_DESPATCH_DTL>();
                        salDespatchDtlList = (List<SAL_DESPATCH_DTL>)SetUIValuesToObject(ControlsEnum.SALDESPATCHDTL);

                        if (salDespatchDtlList != null && salDespatchDtlList.Count > 0)
                        {
                            salDespatchDtlList.ForEach(dtl => salDespatchHdrObj.SAL_DESPATCH_DTL.Add(dtl));
                        }
                        retObject = salDespatchHdrObj;

                        break;
                    #endregion

                    #region Sales Despatch Header Workflow
                    case ControlsEnum.WRKFSUBMIT:
                        salDespatchHdrObj.DPH_PK = CurrPK;
                        updateDespatch = true;
                        GetFieldValues(ControlsEnum.DESPATCHNO);
                        lblDeliveryOrderNo.Text = hdfDespatchNo.Value;
                        salDespatchHdrObj.DPH_NO = "";
                        if (SalDetailList != null)
                        {
                            SalOrderDtlList = (List<SAL_ORDER_DTL>)SalDetailList;
                            salDespatchHdrObj.DPH_CUSTOMER = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER;
                            salDespatchHdrObj.DPH_REFERENCE = SalOrderDtlList[0].SOD_NO;
                            salDespatchHdrObj.DPH_REF_NO = SalOrderDtlList[0].SOD_NO;
                        }
                        if (SaleDespatchDtlList != null)
                        {
                            salDespatchDtlList = (List<SAL_DESPATCH_DTL>)SaleDespatchDtlList;
                            salDespatchHdrObj.DPH_CUSTOMER = salDespatchDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER;
                            salDespatchHdrObj.DPH_REFERENCE = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_REFERENCE;
                            salDespatchHdrObj.DPH_REF_NO = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_REF_NO;
                        }

                        salDespatchHdrObj.DPH_ENCLOS_TERM_TEXT = txtEnclosure.Text;

                        salDespatchHdrObj.DPH_NO = lblDeliveryOrderNo.Text;
                        salDespatchHdrObj.DPH_TO_PORT = lblDestinationPort.Text;
                        if (!string.IsNullOrEmpty(hdfPortOfLoading.Value))
                        {
                            salDespatchHdrObj.DPH_FROM_PORT = Convert.ToInt32(hdfPortOfLoading.Value);
                        }
                        salDespatchHdrObj.DPH_FINAL_DESTINATION = lblFinalDestination.Text.Trim();
                        if (!string.IsNullOrEmpty(txtCYDate.Text.Trim()))
                        {
                            salDespatchHdrObj.DPH_CY_DATE = Convert.ToDateTime(txtCYDate.Text.Trim());
                            salDespatchHdrObj.DPH_DATE = Convert.ToDateTime(txtCYDate.Text.Trim());
                        }
                        else
                        {
                            salDespatchHdrObj.DPH_DATE = DateTime.Now;
                        }
                        if (!string.IsNullOrEmpty(txtRtnDate.Text.Trim()))
                        {
                            salDespatchHdrObj.DPH_RTN_DATE = Convert.ToDateTime(txtRtnDate.Text.Trim());
                        }
                        if (!string.IsNullOrEmpty(txtETD.Text.Trim()))
                        {
                            salDespatchHdrObj.DPH_ETD = Convert.ToDateTime(txtETD.Text.Trim());
                        }
                        if (!string.IsNullOrEmpty(txtETA.Text.Trim()))
                        {
                            salDespatchHdrObj.DPH_ETA = Convert.ToDateTime(txtETA.Text.Trim());
                        }

                        if (Convert.ToInt32(ddlCarrier.SelectedValue) >= 0)
                        {
                            salDespatchHdrObj.DPH_CARRIER = Convert.ToInt32(ddlCarrier.SelectedValue);
                        }

                        salDespatchHdrObj.DPH_CONTAINER_NO = txtContainer.Text.Trim();
                        salDespatchHdrObj.DPH_SEAL_NO = txtSealNo.Text.Trim();

                        if (!string.IsNullOrEmpty(hdfCompany.Value))
                        {
                            salDespatchHdrObj.DPH_COMP = Convert.ToInt32(hdfCompany.Value);
                        }

                        salDespatchHdrObj.DPH_DRIVER = txtDriver.Text.Trim();
                        salDespatchHdrObj.DPH_LORRY_NO = txtLorryNo.Text.Trim();

                        salDespatchHdrObj.DPH_FEEDER_VESSEL = txtFeederVessel.Text.Trim();
                        salDespatchHdrObj.DPH_MOTHER_VESSEL = txtMotherVessel.Text.Trim();
                        if (!string.IsNullOrEmpty(txtDateOfShipment.Text.Trim()))
                        {
                            salDespatchHdrObj.DPH_SHIPMENT_DATE = Convert.ToDateTime(txtDateOfShipment.Text.Trim());
                        }

                        salDespatchHdrObj.DPH_BOOKING_NO = txtBookingNo.Text.Trim();
                        if (!string.IsNullOrEmpty(txtBookingDate.Text.Trim()))
                        {
                            salDespatchHdrObj.DPH_BOOKING_DATE = Convert.ToDateTime(txtBookingDate.Text.Trim());
                        }
                        salDespatchHdrObj.DPH_SHIPPING_MARK = txtShippingMark.Text.Trim();

                        salDespatchHdrObj.DPH_STATUS = 1;
                        salDespatchHdrObj.DPH_ACTIVE = 1;
                        salDespatchHdrObj.DPH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                        salDespatchHdrObj.DPH_CRTD_DT = DateTime.Now;
                        salDespatchHdrObj.DPH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                        salDespatchHdrObj.DPH_MOD_DT = LastModifiedTime;
                        salDespatchHdrObj.DPH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        salDespatchHdrObj.DPH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);

                        if (!string.IsNullOrEmpty(hdfConsignee.Value))
                            salDespatchHdrObj.DPH_CONSIGNEE = Convert.ToInt16(hdfConsignee.Value);
                        salDespatchHdrObj.DPH_CONSIGNEE_NAME = txtConsignee.Text;
                        if (Session["DPH_CONSIGNEE_ADDRESS"] != null)
                            salDespatchHdrObj.DPH_CONSIGNEE_ADDRESS = Session["DPH_CONSIGNEE_ADDRESS"].ToString();
                        if (Session["DPH_CONSIGNEE_COUNTRY"] != null)
                            salDespatchHdrObj.DPH_CONSIGNEE_COUNTRY = Convert.ToInt16(Session["DPH_CONSIGNEE_COUNTRY"].ToString());
                        if (Session["DPH_CONSIGNEE_FAX"] != null)
                            salDespatchHdrObj.DPH_CONSIGNEE_FAX = Session["DPH_CONSIGNEE_FAX"].ToString();
                        if (Session["DPH_CONSIGNEE_PHONE"] != null)
                            salDespatchHdrObj.DPH_CONSIGNEE_PHONE = Session["DPH_CONSIGNEE_PHONE"].ToString();
                        if (Session["DPH_CONSIGNEE_MOBILE"] != null)
                            salDespatchHdrObj.DPH_CONSIGNEE_MOBILE = Session["DPH_CONSIGNEE_MOBILE"].ToString();
                        if (Session["DPH_CONSIGNEE_EMAIL"] != null)
                            salDespatchHdrObj.DPH_CONSIGNEE_EMAIL = Session["DPH_CONSIGNEE_EMAIL"].ToString();
                        if (Session["DPH_CONSIGNEE_ZIP"] != null)
                            salDespatchHdrObj.DPH_CONSIGNEE_ZIP = Session["DPH_CONSIGNEE_ZIP"].ToString();
                        if (!string.IsNullOrEmpty(hdfNotifyParty.Value))
                            salDespatchHdrObj.DPH_NOTIFY_PARTY = Convert.ToInt16(hdfNotifyParty.Value);
                        salDespatchHdrObj.DPH_NOTIFY_PARTY_NAME = txtNotifyParty.Text;
                        if (Session["DPH_NOTIFY_PARTY_ADDRESS"] != null)
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_ADDRESS = Session["DPH_NOTIFY_PARTY_ADDRESS"].ToString();
                        if (Session["DPH_NOTIFY_PARTY_COUNTRY"] != null)
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_COUNTRY = Convert.ToInt16(Session["DPH_NOTIFY_PARTY_COUNTRY"].ToString());
                        if (Session["DPH_NOTIFY_PARTY_FAX"] != null)
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_FAX = Session["DPH_NOTIFY_PARTY_FAX"].ToString();
                        if (Session["DPH_NOTIFY_PARTY_PHONE"] != null)
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_PHONE = Session["DPH_NOTIFY_PARTY_PHONE"].ToString();
                        if (Session["DPH_NOTIFY_PARTY_MOBILE"] != null)
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_MOBILE = Session["DPH_NOTIFY_PARTY_MOBILE"].ToString();
                        if (Session["DPH_NOTIFY_PARTY_EMAIL"] != null)
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_EMAIL = Session["DPH_NOTIFY_PARTY_EMAIL"].ToString();
                        if (Session["DPH_NOTIFY_PARTY_ZIP"] != null)
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_ZIP = Session["DPH_NOTIFY_PARTY_ZIP"].ToString();
                        if (!string.IsNullOrEmpty(hdfPaymentTerms.Value))
                            salDespatchHdrObj.DPH_PAYMENT_TERM = Convert.ToInt16(hdfPaymentTerms.Value);
                        salDespatchHdrObj.DPH_PAYMENT_TERM_TEXT = txtPaymentTerms.Text;

                        salDespatchHdrObj.DPH_SUPP_DTL = txtSupplimentary.Text;
                        if (!string.IsNullOrEmpty(hdfModeofTransport.Value))
                            salDespatchHdrObj.DPH_SHIP_BY = Convert.ToInt16(hdfModeofTransport.Value);

                        salDespatchHdrObj.DPH_CUSTOMER_NAME = lblDeliveryTo.Text;
                        salDespatchHdrObj.DPH_CUSTOMER_ADDRESS = txtDeliveryAddress.Text.Trim();
                        if (Session["DPH_CUSTOMER_COUNTRY"] != null)
                            salDespatchHdrObj.DPH_CUSTOMER_COUNTRY = Convert.ToInt16(Session["DPH_CUSTOMER_COUNTRY"].ToString());
                        if (Session["DPH_CUSTOMER_FAX"] != null)
                            salDespatchHdrObj.DPH_CUSTOMER_FAX = Session["DPH_CUSTOMER_FAX"].ToString();
                        if (Session["DPH_CUSTOMER_PHONE"] != null)
                            salDespatchHdrObj.DPH_CUSTOMER_PHONE = Session["DPH_CUSTOMER_PHONE"].ToString();
                        if (Session["DPH_CUSTOMER_MOBILE"] != null)
                            salDespatchHdrObj.DPH_CUSTOMER_MOBILE = Session["DPH_CUSTOMER_MOBILE"].ToString();
                        if (Session["DPH_CUSTOMER_EMAIL"] != null)
                            salDespatchHdrObj.DPH_CUSTOMER_EMAIL = Session["DPH_CUSTOMER_EMAIL"].ToString();
                        if (Session["DPH_CUSTOMER_ZIP"] != null)
                            salDespatchHdrObj.DPH_CUSTOMER_ZIP = Session["DPH_CUSTOMER_ZIP"].ToString();


                        salDespatchDtlList = new List<SAL_DESPATCH_DTL>();
                        salDespatchDtlList = (List<SAL_DESPATCH_DTL>)SetUIValuesToObject(ControlsEnum.SALDESPATCHDTL);

                        if (salDespatchDtlList != null && salDespatchDtlList.Count > 0)
                        {
                            salDespatchDtlList.ForEach(dtl => salDespatchHdrObj.SAL_DESPATCH_DTL.Add(dtl));
                        }
                        retObject = salDespatchHdrObj;

                        break;
                    #endregion
                    #region Sale Despatch Dtl
                    case ControlsEnum.SALDESPATCHDTL:
                        int rowID = 0;
                        HiddenField hdfSaleOrderDtlPK;
                        HiddenField hdfSaleOrderHdrPK;
                        TextBox txtDespNow;
                        HiddenField hdfIGPLCode;
                        HiddenField hdfBrandCode;
                        HiddenField hdfUOM;

                        foreach (GridViewRow grdrow in grdDeliveryList.Rows)
                        {
                            txtDespNow = (TextBox)grdDeliveryList.Rows[rowID].FindControl("txtDespNow");
                            if (txtDespNow != null && !string.IsNullOrEmpty(txtDespNow.Text.Trim())
                                && Convert.ToDouble(txtDespNow.Text) > 0)
                            {
                                salDespatchDtlObj = CommonFunctions.Initilize<SAL_DESPATCH_DTL>();

                                salDespatchDtlObj.DPD_PK = CurrMpgPK;
                                salDespatchDtlObj.DPD_VERSION = 1;
                                salDespatchDtlObj.DPD_DESPATCH_HDR = CurrPK;
                                salDespatchDtlObj.DPD_SL_NO = Convert.ToInt16(rowID + 1);
                                salDespatchDtlObj.DPD_DATE = DateTime.Now;
                                salDespatchDtlObj.DPD_NO = lblDeliveryOrderNo.Text;
                                hdfSaleOrderHdrPK = (HiddenField)grdDeliveryList.Rows[rowID].FindControl("hdfSaleOrderHdrPK");
                                hdfSaleOrderDtlPK = (HiddenField)grdDeliveryList.Rows[rowID].FindControl("hdfSaleOrderDtlPK");
                                salDespatchDtlObj.DPD_SALE_ORDER = Convert.ToInt32(hdfSaleOrderHdrPK.Value);
                                salDespatchDtlObj.DPD_SO_DTL = Convert.ToInt32(hdfSaleOrderDtlPK.Value);
                                hdfIGPLCode = (HiddenField)grdDeliveryList.Rows[rowID].FindControl("hdfIGPLCode");
                                hdfBrandCode = (HiddenField)grdDeliveryList.Rows[rowID].FindControl("hdfBrandCode");
                                hdfUOM = (HiddenField)grdDeliveryList.Rows[rowID].FindControl("hdfUOM");
                                if (!string.IsNullOrEmpty(hdfBrandCode.Value))
                                {
                                    salDespatchDtlObj.DPD_CUST_ITEM = Convert.ToInt32(hdfBrandCode.Value);
                                }
                                salDespatchDtlObj.DPD_ITEM = Convert.ToInt32(hdfIGPLCode.Value);
                                salDespatchDtlObj.DPD_QTY_DESPATCHED = Convert.ToDouble(txtDespNow.Text);
                                salDespatchDtlObj.DPD_QTY_APPROVED = 0;
                                salDespatchDtlObj.DPD_UOM = Convert.ToInt32(hdfUOM.Value);
                                salDespatchDtlObj.DPD_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                                if (SalDetailList != null)
                                {
                                    SAL_ORDER_DTL salOrderDtl = SalDetailList.SingleOrDefault(aa => aa.SOD_PK == salDespatchDtlObj.DPD_SO_DTL);
                                    if (salOrderDtl != null)
                                    {
                                        if (salOrderDtl.ADM_PACK_SPEC_MST != null)
                                        {
                                            salDespatchDtlObj.DPD_PACKING_SPEC = salOrderDtl.ADM_PACK_SPEC_MST.APS_PK;
                                        }
                                    }
                                }
                                salDespatchDtlList.Add(salDespatchDtlObj);
                                isSave = true;
                            }
                            rowID++;
                        }
                        retObject = salDespatchDtlList;

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
        /// Is Same Vendor
        /// </summary>
        /// <param name="vendors"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsSameInvoice(List<long> invoices, long pk)
        {

            bool flag = true;
            if (invoices != null)
                foreach (long ven in invoices)
                    if (ven != pk)
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
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {

                switch (controlType)
                {
                    #region Invoice Header
                    case ControlsEnum.DELIVERYORDERDETAILS:
                        if (salDespatchDtlList != null && salDespatchDtlList.Count > 0)
                        {
                            CurrPK = int.Parse(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_PK.ToString());
                            lblDeliveryTo.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.CRM_CUSTOMER_MST.CUS_NAME;
                            lblDeliveryOrderNo.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NO == "" ? "[NEW]" : salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NO;
                            string address = string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CUSTOMER_ADDRESS) ? string.Empty : salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CUSTOMER_ADDRESS;
                            string countryName = salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_COUNTRY_MST1 == null ? string.Empty : salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_COUNTRY_MST1.CNT_NAME;
                            if (string.IsNullOrEmpty(address) && string.IsNullOrEmpty(countryName))
                                txtDeliveryAddress.Text = string.Empty;
                            else if (string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(countryName))
                                txtDeliveryAddress.Text = countryName;
                            else if (!string.IsNullOrEmpty(address) && string.IsNullOrEmpty(countryName))
                                txtDeliveryAddress.Text = address;
                            else
                                txtDeliveryAddress.Text = address + countryName;

                            if (salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CY_DATE.HasValue)
                            {
                                txtCYDate.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CY_DATE.Value.ToString(Resources.Constants.DateFormatShort);
                            }
                            lblDestinationPort.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_TO_PORT;
                            if (salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_RTN_DATE.HasValue)
                            {
                                txtRtnDate.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_RTN_DATE.Value.ToString(Resources.Constants.DateFormatShort);
                            }
                            //if (salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_CONST_MST1 != null)
                            //{
                            //    lblPortOfLoading.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_CONST_MST1.CON_NAME;
                            //    hdfPortOfLoading.Value = salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_CONST_MST1.CON_PK.ToString();
                            //}
                            if (salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_FROM_PORT != null)
                            {
                                GetFieldValues(ControlsEnum.PORTDETAILS);
                                lblPortOfLoading.Text = dtFromPort.Rows[0]["PRM_NAME"].ToString();
                                hdfPortOfLoading.Value = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_FROM_PORT.ToString();
                            }
                            if (salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_ETD.HasValue)
                            {
                                txtETD.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_ETD.Value.ToString(Resources.Constants.DateFormatShort);
                            }
                            if (salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_ETA.HasValue)
                            {
                                txtETA.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_ETA.Value.ToString(Resources.Constants.DateFormatShort);
                            }
                            lblFinalDestination.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_FINAL_DESTINATION;

                            if (salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_CONST_MST1 != null)
                            {
                                ddlCarrier.SelectedValue = salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_CONST_MST1.CON_PK.ToString();
                            }
                            txtContainer.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONTAINER_NO;
                            txtSealNo.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SEAL_NO;

                            if (salDespatchDtlList[0].SAL_DESPATCH_HDR.PUR_VENDOR_MST != null)
                            {
                                hdfCompany.Value = salDespatchDtlList[0].SAL_DESPATCH_HDR.PUR_VENDOR_MST.VEN_PK.ToString();
                                txtCompany.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.PUR_VENDOR_MST.VEN_NAME;
                            }
                            txtDriver.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_DRIVER;
                            txtLorryNo.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_LORRY_NO;

                            txtFeederVessel.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_FEEDER_VESSEL;
                            txtMotherVessel.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_MOTHER_VESSEL;
                            if (salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SHIPMENT_DATE.HasValue)
                            {
                                txtDateOfShipment.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SHIPMENT_DATE.Value.ToString(Resources.Constants.DateFormatShort);
                            }
                            txtBookingNo.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_BOOKING_NO;
                            if (salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_BOOKING_DATE.HasValue)
                            {
                                txtBookingDate.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_BOOKING_DATE.Value.ToString(Resources.Constants.DateFormatShort);
                            }
                            txtShippingMark.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SHIPPING_MARK;

                            LastModifiedTime = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_MOD_DT;
                            lblLastModifiedHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);

                            hdfConsignee.Value = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE == null ? null :
                                                salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE.ToString();
                            txtConsignee.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_NAME;

                            Session["DPH_CONSIGNEE_ADDRESS"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ADDRESS;
                            Session["DPH_CONSIGNEE_COUNTRY"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_COUNTRY;
                            Session["DPH_CONSIGNEE_FAX"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_FAX;
                            Session["DPH_CONSIGNEE_PHONE"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_PHONE;
                            Session["DPH_CONSIGNEE_MOBILE"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_MOBILE;
                            Session["DPH_CONSIGNEE_EMAIL"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_EMAIL;
                            Session["DPH_CONSIGNEE_ZIP"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ZIP;

                            if (salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_COUNTRY == null)
                            {
                                string consigneeDetails = string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ADDRESS) ? "" :
                                    salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ADDRESS + Environment.NewLine;
                                consigneeDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_FAX) ? "" :
                                    GetLocalResourceObject("Fax1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_FAX + Environment.NewLine;
                                consigneeDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_PHONE) ? "" :
                                    GetLocalResourceObject("Phone1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_PHONE + Environment.NewLine;
                                consigneeDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_MOBILE) ? "" :
                                    GetLocalResourceObject("Mobile1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_MOBILE + Environment.NewLine;
                                consigneeDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_EMAIL) ? "" :
                                    GetLocalResourceObject("Email1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_EMAIL + Environment.NewLine;
                                consigneeDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ZIP) ? "" :
                                    GetLocalResourceObject("Zip1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ZIP;
                                txtConsigneeDetails.Text = consigneeDetails;

                                //txtConsigneeDetails.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ADDRESS + Environment.NewLine +
                                //                           salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_FAX + Environment.NewLine +
                                //                           salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_PHONE + Environment.NewLine +
                                //                           salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_MOBILE + Environment.NewLine +
                                //                           salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_EMAIL + Environment.NewLine +
                                //                           salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ZIP;
                            }
                            else
                            {
                                string consigneeDetails = string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ADDRESS) ? "" : salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ADDRESS + Environment.NewLine;
                                consigneeDetails += salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_COUNTRY_MST == null ? "" :
                                                      string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_COUNTRY_MST.CNT_NAME) ? "" : salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_COUNTRY_MST.CNT_NAME + Environment.NewLine;

                                consigneeDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_FAX) ? "" :
                                    GetLocalResourceObject("Fax1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_FAX + Environment.NewLine;
                                consigneeDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_PHONE) ? "" :
                                    GetLocalResourceObject("Phone1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_PHONE + Environment.NewLine;
                                consigneeDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_MOBILE) ? "" :
                                    GetLocalResourceObject("Mobile1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_MOBILE + Environment.NewLine;
                                consigneeDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_EMAIL) ? "" :
                                    GetLocalResourceObject("Email1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_EMAIL + Environment.NewLine;
                                consigneeDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ZIP) ? "" :
                                    GetLocalResourceObject("Zip1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ZIP;
                                txtConsigneeDetails.Text = consigneeDetails;

                                //txtConsigneeDetails.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ADDRESS + Environment.NewLine +
                                //                           salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_COUNTRY_MST.CNT_NAME + Environment.NewLine +
                                //                           salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_FAX + Environment.NewLine +
                                //                           salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_PHONE + Environment.NewLine +
                                //                           salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_MOBILE + Environment.NewLine +
                                //                           salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_EMAIL + Environment.NewLine +
                                //                           salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ZIP;
                            }
                            hdfNotifyParty.Value = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY == null ? null :
                                                   salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY.ToString();
                            txtNotifyParty.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_NAME;

                            Session["DPH_NOTIFY_PARTY_ADDRESS"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ADDRESS;
                            Session["DPH_NOTIFY_PARTY_COUNTRY"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_COUNTRY;
                            Session["DPH_NOTIFY_PARTY_FAX"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_FAX;
                            Session["DPH_NOTIFY_PARTY_PHONE"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_PHONE;
                            Session["DPH_NOTIFY_PARTY_MOBILE"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_MOBILE;
                            Session["DPH_NOTIFY_PARTY_EMAIL"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_EMAIL;
                            Session["DPH_NOTIFY_PARTY_ZIP"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ZIP;

                            if (salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_COUNTRY == null)
                            {
                                string partyDetails = string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ADDRESS) ? "" :
                                    salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ADDRESS + Environment.NewLine;
                                partyDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_FAX) ? "" :
                                    GetLocalResourceObject("Fax1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_FAX + Environment.NewLine;
                                partyDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_PHONE) ? ""
                                    : GetLocalResourceObject("Phone1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_PHONE + Environment.NewLine;
                                partyDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_MOBILE) ? "" :
                                    GetLocalResourceObject("Mobile1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_MOBILE + Environment.NewLine;
                                partyDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_EMAIL) ? "" :
                                    GetLocalResourceObject("Email1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_EMAIL + Environment.NewLine;
                                partyDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ZIP) ? "" :
                                    GetLocalResourceObject("Zip1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ZIP;
                                txtNotifyPartyDetails.Text = partyDetails;

                                //txtNotifyPartyDetails.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ADDRESS + Environment.NewLine +
                                //                             salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_FAX + Environment.NewLine +
                                //                             salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_PHONE + Environment.NewLine +
                                //                             salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_MOBILE + Environment.NewLine +
                                //                             salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_EMAIL + Environment.NewLine +
                                //                             salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ZIP;
                            }
                            else
                            {
                                string partyDetails = string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ADDRESS) ? "" :
                                    salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ADDRESS + Environment.NewLine;
                                partyDetails += salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_COUNTRY_MST2 == null ? "" :
                                                             salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_COUNTRY_MST2.CNT_NAME + Environment.NewLine;
                                partyDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_FAX) ? "" :
                                    GetLocalResourceObject("Fax1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_FAX + Environment.NewLine;
                                partyDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_PHONE) ? ""
                                    : GetLocalResourceObject("Phone1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_PHONE + Environment.NewLine;
                                partyDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_MOBILE) ? "" :
                                    GetLocalResourceObject("Mobile1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_MOBILE + Environment.NewLine;
                                partyDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_EMAIL) ? "" :
                                    GetLocalResourceObject("Email1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_EMAIL + Environment.NewLine;
                                partyDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ZIP) ? "" :
                                    GetLocalResourceObject("Zip1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ZIP;
                                txtNotifyPartyDetails.Text = partyDetails;

                                //txtNotifyPartyDetails.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ADDRESS + Environment.NewLine +
                                //                             salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_COUNTRY_MST2.CNT_NAME + Environment.NewLine +
                                //                             salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_FAX + Environment.NewLine +
                                //                             salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_PHONE + Environment.NewLine +
                                //                             salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_MOBILE + Environment.NewLine +
                                //                             salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_EMAIL + Environment.NewLine +
                                //                             salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ZIP;
                            }

                            hdfPaymentTerms.Value = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_PAYMENT_TERM == null ? null :
                                                   salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_PAYMENT_TERM.ToString();
                            txtPaymentTerms.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_PAYMENT_TERM_TEXT;
                            txtSupplimentary.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SUPP_DTL;

                            hdfModeofTransport.Value = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SHIP_BY == null ? null :
                                                       salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SHIP_BY.ToString();
                            txtModeofTransport.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_TO_PORT == null ? "" :
                                                      salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_TO_PORT.PRM_NAME;


                            Session["DPH_CUSTOMER_COUNTRY"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CUSTOMER_COUNTRY;
                            Session["DPH_CUSTOMER_FAX"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CUSTOMER_FAX;
                            Session["DPH_CUSTOMER_PHONE"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CUSTOMER_PHONE;
                            Session["DPH_CUSTOMER_MOBILE"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CUSTOMER_MOBILE;
                            Session["DPH_CUSTOMER_EMAIL"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CUSTOMER_EMAIL;
                            Session["DPH_CUSTOMER_ZIP"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CUSTOMER_ZIP;
                            txtEnclosure.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_ENCLOS_TERM_TEXT;

                        }
                        else if (SalOrderDtlList != null && SalOrderDtlList.Count > 0)
                        {
                            CurrPK = 0;
                            lblDeliveryTo.Text = SalOrderDtlList[0].SAL_ORDER_HDR.CRM_CUSTOMER_MST.CUS_NAME;

                            string address = string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER_ADDRESS) ? string.Empty : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER_ADDRESS;
                            string countryName = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_COUNTRY_MST1 == null ? string.Empty : SalOrderDtlList[0].SAL_ORDER_HDR.ADM_COUNTRY_MST1.CNT_NAME;
                            if (string.IsNullOrEmpty(address) && string.IsNullOrEmpty(countryName))
                                txtDeliveryAddress.Text = string.Empty;
                            else if (string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(countryName))
                                txtDeliveryAddress.Text = countryName;
                            else if (!string.IsNullOrEmpty(address) && string.IsNullOrEmpty(countryName))
                                txtDeliveryAddress.Text = address;
                            else
                                txtDeliveryAddress.Text = address + countryName;

                            lblDestinationPort.Text = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_TO_PORT;

                            //if (SalOrderDtlList[0].SAL_ORDER_HDR.ADM_CONST_MST != null)
                            //{
                            //    lblPortOfLoading.Text = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_FROM_PORT.PRM_NAME;
                            //    hdfPortOfLoading.Value = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_FROM_PORT.PRM_PK.ToString();
                            //}

                            if (SalOrderDtlList[0].SAL_ORDER_HDR.ADM_FROM_PORT.PRM_PK != null)
                            {
                                hdfPortOfLoading.Value = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_FROM_PORT.PRM_PK.ToString();
                                lblPortOfLoading.Text = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_FROM_PORT.PRM_NAME.ToString();
                            }

                            lblFinalDestination.Text = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_FINAL_DESTINATION;

                            txtShippingMark.Text = "";
                            foreach (SAL_ORDER_DTL Obj in SalOrderDtlList)
                            {
                                if (string.IsNullOrEmpty(txtShippingMark.Text))
                                {
                                    txtShippingMark.Text = Obj.SOD_LOT_NO;
                                }
                                else
                                {
                                    txtShippingMark.Text = txtShippingMark.Text + Environment.NewLine + Obj.SOD_LOT_NO;
                                }
                            }

                            hdfConsignee.Value = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE.ToString();
                            txtConsignee.Text = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_NAME;

                            Session["DPH_CONSIGNEE_ADDRESS"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ADDRESS;
                            Session["DPH_CONSIGNEE_COUNTRY"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_COUNTRY;
                            Session["DPH_CONSIGNEE_FAX"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_FAX;
                            Session["DPH_CONSIGNEE_PHONE"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_PHONE;
                            Session["DPH_CONSIGNEE_MOBILE"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_MOBILE;
                            Session["DPH_CONSIGNEE_EMAIL"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_EMAIL;
                            Session["DPH_CONSIGNEE_ZIP"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ZIP;

                            string consigneeDetails = string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ADDRESS) ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ADDRESS + Environment.NewLine;
                            consigneeDetails += SalOrderDtlList[0].SAL_ORDER_HDR.ADM_COUNTRY_MST == null ? "" :
                                                       string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.ADM_COUNTRY_MST.CNT_NAME) ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.ADM_COUNTRY_MST.CNT_NAME + Environment.NewLine;
                            consigneeDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_FAX) ? "" : GetLocalResourceObject("Fax1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_FAX + Environment.NewLine;
                            consigneeDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_PHONE) ? "" : GetLocalResourceObject("Phone1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_PHONE + Environment.NewLine;
                            consigneeDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_MOBILE) ? "" : GetLocalResourceObject("Mobile1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_MOBILE + Environment.NewLine;
                            consigneeDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_EMAIL) ? "" : GetLocalResourceObject("Email1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_EMAIL + Environment.NewLine;
                            consigneeDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ZIP) ? "" : GetLocalResourceObject("Zip1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ZIP;
                            txtConsigneeDetails.Text = consigneeDetails;
                            //txtConsigneeDetails.Text = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ADDRESS == null ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ADDRESS + Environment.NewLine +
                            //                           SalOrderDtlList[0].SAL_ORDER_HDR.ADM_COUNTRY_MST == null ? "" :
                            //                           SalOrderDtlList[0].SAL_ORDER_HDR.ADM_COUNTRY_MST.CNT_NAME == null ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.ADM_COUNTRY_MST.CNT_NAME + Environment.NewLine +
                            //                           SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_FAX == null ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_FAX + Environment.NewLine +
                            //                           SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_PHONE == null ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_PHONE + Environment.NewLine +
                            //                           SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_MOBILE == null ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_MOBILE + Environment.NewLine +
                            //                           SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_EMAIL == null ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_EMAIL + Environment.NewLine +
                            //                           SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ZIP == null ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ZIP;
                            hdfNotifyParty.Value = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY.ToString();
                            txtNotifyParty.Text = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_NAME;

                            Session["DPH_NOTIFY_PARTY_ADDRESS"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ADDRESS;
                            Session["DPH_NOTIFY_PARTY_COUNTRY"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_COUNTRY;
                            Session["DPH_NOTIFY_PARTY_FAX"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_FAX;
                            Session["DPH_NOTIFY_PARTY_PHONE"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_PHONE;
                            Session["DPH_NOTIFY_PARTY_MOBILE"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_MOBILE;
                            Session["DPH_NOTIFY_PARTY_EMAIL"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_EMAIL;
                            Session["DPH_NOTIFY_PARTY_ZIP"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ZIP;

                            string partyDetails = string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ADDRESS) ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ADDRESS + Environment.NewLine;
                            partyDetails += SalOrderDtlList[0].SAL_ORDER_HDR.ADM_COUNTRY_MST2 == null ? "" :
                                                         SalOrderDtlList[0].SAL_ORDER_HDR.ADM_COUNTRY_MST2.CNT_NAME + Environment.NewLine;
                            partyDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_FAX) ? "" : GetLocalResourceObject("Fax1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_FAX + Environment.NewLine;
                            partyDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_PHONE) ? "" : GetLocalResourceObject("Phone1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_PHONE + Environment.NewLine;
                            partyDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_MOBILE) ? "" : GetLocalResourceObject("Mobile1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_MOBILE + Environment.NewLine;
                            partyDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_EMAIL) ? "" : GetLocalResourceObject("Email1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_EMAIL + Environment.NewLine;
                            partyDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ZIP) ? "" : GetLocalResourceObject("Zip1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ZIP;
                            txtNotifyPartyDetails.Text = partyDetails;
                            //txtNotifyPartyDetails.Text = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ADDRESS == null ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ADDRESS + Environment.NewLine +
                            //                             SalOrderDtlList[0].SAL_ORDER_HDR.ADM_COUNTRY_MST2 == null ? "" :
                            //                             SalOrderDtlList[0].SAL_ORDER_HDR.ADM_COUNTRY_MST2.CNT_NAME + Environment.NewLine +
                            //                             SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_FAX == null ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_FAX + Environment.NewLine +
                            //                             SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_PHONE == null ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_PHONE + Environment.NewLine +
                            //                             SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_MOBILE == null ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_MOBILE + Environment.NewLine +
                            //                             SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_EMAIL == null ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_EMAIL + Environment.NewLine +
                            //                             SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ZIP == null ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ZIP;
                            hdfPaymentTerms.Value = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_PAYMENT_TERM.ToString();
                            txtPaymentTerms.Text = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_PAYMENT_TERM_TEXT;
                            //txtSupplimentary.Text = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SUPP_DTL;

                            hdfModeofTransport.Value = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIP_BY.ToString();
                            txtModeofTransport.Text = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_TO_PORT == null ? "" :
                                                      SalOrderDtlList[0].SAL_ORDER_HDR.ADM_TO_PORT.PRM_NAME;



                            Session["DPH_CUSTOMER_COUNTRY"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER_COUNTRY;
                            Session["DPH_CUSTOMER_FAX"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER_FAX;
                            Session["DPH_CUSTOMER_PHONE"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER_PHONE;
                            Session["DPH_CUSTOMER_MOBILE"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER_MOBILE;
                            Session["DPH_CUSTOMER_EMAIL"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER_EMAIL;
                            Session["DPH_CUSTOMER_ZIP"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER_ZIP;
                            GetFieldValues(ControlsEnum.ENCLOSURECONFIG);
                            if (admConfigMstList != null && admConfigMstList.Count > 0)
                            {
                                txtEnclosure.Text = admConfigMstList[0].CFG_DATA;
                            }

                        }

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
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.CARRIER:
                    ddlCarrier.Items.Clear();
                    if (admConstMstList != null && admConstMstList.Count > 0)
                    {
                        ddlCarrier.DataSource = admConstMstList;
                        ddlCarrier.DataTextField = "CON_NAME";
                        ddlCarrier.DataValueField = "CON_PK";
                        ddlCarrier.DataBind();
                    }
                    ddlCarrier.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.PRINTLIST:
                    ddlPrint.Items.Clear();
                    if (admAppSubTypeMstList != null && admAppSubTypeMstList.Count > 0)
                    {
                        ddlPrint.DataSource = admAppSubTypeMstList;
                        ddlPrint.DataTextField = "AST_NAME";
                        ddlPrint.DataValueField = "AST_VALUE";
                        ddlPrint.DataBind();
                        // btnPrintlist.NavigateUrl = Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DO + "&APPSUBTYPE=" + ddlPrint.SelectedValue);
                        btnPrintlist.Attributes.Add("onClick", "javascript:return OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DO + "&APPSUBTYPE=" + ddlPrint.SelectedValue) + "');");

                    }
                    //ddlPrint.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                default:

                    break;
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
                    case ControlsEnum.DELIVERYORDERHDR:
                        uclPaging.TotalPages = TotalPages;
                        PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        grdDeliveryOrderList.DataSource = salDespatchHdrList;
                        grdDeliveryOrderList.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                        break;
                    case ControlsEnum.DELIVERYORDERLIST:
                        if (salDespatchDtlList != null && salDespatchDtlList.Count > 0)
                        {
                            grdDeliveryList.DataSource = salDespatchDtlList;
                            grdDeliveryList.DataBind();
                        }
                        else if (SalOrderDtlList != null && SalOrderDtlList.Count > 0)
                        {
                            //lblDispCustomerName.Text = SalOrderDtlList[0].SAL_ORDER_HDR.CRM_CUSTOMER_MST.CUS_NAME;
                            grdDeliveryList.DataSource = SalOrderDtlList;
                            grdDeliveryList.DataBind();
                        }
                        else
                        {
                            grdDeliveryList.DataSource = null;
                            grdDeliveryList.DataBind();
                        }

                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            ExtGridView grdSoList;
            Label lblSCDespQty;
            Label lblSCInvQty;
            decimal SCDesQty = 0;
            decimal SCInvQty = 0;
            try
            {
                if (Mode == ActionsEnum.PICKFORINVOICING)
                {
                    objSOitem = new SOHeaderBO();
                    objSOitem.SOList = new List<SOHeaderListBO>();
                    List<SOHeaderListBO> objItemList = new List<SOHeaderListBO>();
                    SOHeaderListBO objSoList;
                    foreach (ExtGridViewRow grdrow in grdDeliveryOrderList.Rows)
                    {
                        grdSoList = grdrow.FindControl("grdSoList") as ExtGridView;
                        if (grdSoList != null)
                        {
                            foreach (ExtGridViewRow egrdrow in grdSoList.Rows)
                            {
                                //RadioButton rbtn;
                                //rbtn = (RadioButton)egrdrow.FindControl("rbtSelect");
                                //if (rbtn.Checked)
                                //{
                                CheckBox chkSCselect;
                                chkSCselect = (CheckBox)egrdrow.FindControl("chkSCselect");
                                if (chkSCselect.Checked)
                                {
                                    Session[ERP.Utilities.SessionStrings.GONPK] = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDespatchID")).Value);

                                    lblSCDespQty = egrdrow.FindControl("lblSCDespQty") as Label;
                                    SCDesQty = lblSCDespQty == null ? 0 : Convert.ToDecimal(lblSCDespQty.Text.Trim().Replace(",", ""));
                                    lblSCInvQty = egrdrow.FindControl("lblSCInvQty") as Label;
                                    SCInvQty = lblSCInvQty == null ? 0 : Convert.ToDecimal(lblSCInvQty.Text.Trim().Replace(",", ""));
                                    if (SCDesQty > SCInvQty)
                                    {
                                        objSoList = new SOHeaderListBO();
                                        objSoList.SOH_PK = Convert.ToInt32(((HiddenField)egrdrow.FindControl("hdfSOID")).Value);
                                        objSoList.DPH_PK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.GONPK]);
                                        objItemList.Add(objSoList);
                                        //btnPickForInvoice.Text = GetLocalResourceObject("PickSoForInvoicing").ToString() + "(1)";
                                    }
                                    else if (SCDesQty == SCInvQty)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Err_EqualQty").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                        EntryStatus = EntryStatus.LISTMODE;
                                        Session[ERP.Utilities.SessionStrings.SALEORDERDOPK] = null;
                                        return;
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Err_Qty").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                        EntryStatus = EntryStatus.LISTMODE;
                                        Session[ERP.Utilities.SessionStrings.SALEORDERDOPK] = null;
                                        return;
                                    }
                                    //return;
                                }
                            }
                        }
                        objSOitem.SOList = objItemList;
                       
                        if (objSOitem != null && objSOitem.SOList != null)
                        {
                            if (objSOitem.SOList.Count > 0)
                            {
                                string xmlDoc = CommonFunctions.XmlSerialize<SOHeaderBO>(objSOitem);
                                XmlCount = objSOitem.SOList.Count;
                                Session[ERP.Utilities.SessionStrings.SALEORDERDOPK] = xmlDoc;
                            }
                        }
                    }
                    //// if no items selected, Show Error Message
                    //litErrorMsg.Text = GetLocalResourceObject("Err_Select_SO").ToString();
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                    EntryStatus = EntryStatus.LISTMODE;
                }
                else
                {
                    if (Mode == ActionsEnum.VIEW)
                    {
                        EntryStatus = EntryStatus.VIEWMODE;
                    }
                    else
                    {
                        EntryStatus = EntryStatus.ENTRYMODE;
                    }
                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                    ucrWrkf.RefID = workflowCore.GetRefID(DespatchID, ucrWrkf.ProcessID);
                    ucrWrkf.FillWorkFlowDetails();
                    if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE)
                    {
                        ucrWrkf.ViewType = 1;
                        btnSave.Visible = true;
                    }
                    else
                    {
                        ucrWrkf.ViewType = 0;
                        ucrWrkf.ViewAction();
                        EntryStatus = EntryStatus.VIEWMODE;
                        btnSave.Visible = false;
                        pnlPrint.Visible = true;
                    }
                    ucrWrkf.ViewAction();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm()
        {
            CurrPK = 0;
            hdfDPHPK.Value = "";
            txtDespatchNumber.Text = "Select/Type";
            txtVendor.Text = "Select/Type";
            hdfVendor.Value = "";
            txtCustomer.Text = "Select/Type";
            hdfCustomerID.Value = "";
            txtConsignee.Text = "";
            hdfConsignee.Value = "";
            txtConsigneeDetails.Text = "";
            txtPaymentTerms.Text = "";
            txtNotifyParty.Text = "";
            hdfNotifyParty.Value = "";
            txtNotifyPartyDetails.Text = "";
            txtSupplimentary.Text = "";
            txtModeofTransport.Text = "";
            hdfModeofTransport.Value = "";
            Session["DPH_CONSIGNEE_ADDRESS"] = null;
            Session["DPH_CONSIGNEE_COUNTRY"] = null;
            Session["DPH_CONSIGNEE_FAX"] = null;
            Session["DPH_CONSIGNEE_PHONE"] = null;
            Session["DPH_CONSIGNEE_MOBILE"] = null;
            Session["DPH_CONSIGNEE_EMAIL"] = null;
            Session["DPH_CONSIGNEE_ZIP"] = null;
            Session["DPH_NOTIFY_PARTY_ADDRESS"] = null;
            Session["DPH_NOTIFY_PARTY_COUNTRY"] = null;
            Session["DPH_NOTIFY_PARTY_FAX"] = null;
            Session["DPH_NOTIFY_PARTY_PHONE"] = null;
            Session["DPH_NOTIFY_PARTY_MOBILE"] = null;
            Session["DPH_NOTIFY_PARTY_EMAIL"] = null;
            Session["DPH_NOTIFY_PARTY_ZIP"] = null;
            Session["DPH_CUSTOMER_COUNTRY"] = null;
            Session["DPH_CUSTOMER_FAX"] = null;
            Session["DPH_CUSTOMER_PHONE"] = null;
            Session["DPH_CUSTOMER_MOBILE"] = null;
            Session["DPH_CUSTOMER_EMAIL"] = null;
            Session["DPH_CUSTOMER_ZIP"] = null;
            SelectedSosFroDO = null;
            Session[ERP.Utilities.SessionStrings.SelectedSosForDO] = null;
            txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
            hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
            txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            hdfToDate.Value = DateTime.Now.ToString();
            //txtFromDate.Text = string.Empty;
            //hdfFromDate.Value = string.Empty;
            //txtToDate.Text = string.Empty;
            //hdfToDate.Value = string.Empty;
            ModifiedDatePnl.Visible = false;
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
        #region ActionHandler
        /// <summary>
        /// Button actions events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            DeliveryOrderService deliveryOrderServiceClient;
            deliveryOrderServiceClient = null;
            try
            {
                GridViewRow gvr;
                bool bIsChecked = false;
                int Status = 1;
                DropDownList ddlWkfAction;
                long result=0;
                string action;
                TextBox WrkfComments;
                isSave = false;
                string soPK;
                string arg;
                ExtGridViewRow extGvr;
                GridView grd;
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
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    commonActions = ActionsEnum.SHOWDETAILS;
                }
                switch (commonActions)
                {
                    #region SAVE
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            if (grdDeliveryList.Rows.Count > 0)
                            {
                                salDespatchHdrList = new List<SAL_DESPATCH_HDR>();
                                deliveryOrderServiceClient = new DeliveryOrderService();
                                deliveryOrderServiceClient = CommonFunctions.InitiateClient(deliveryOrderServiceClient);
                                salDespatchHdrObj = CommonFunctions.Initilize<SAL_DESPATCH_HDR>();
                                salDespatchHdrObj = (SAL_DESPATCH_HDR)SetUIValuesToObject(ControlsEnum.SALDESPATCHHDR);
                                if (isSave)
                                {
                                    if (salDespatchHdrObj != null)
                                    {
                                        salDespatchHdrList.Add(salDespatchHdrObj);
                                        result = deliveryOrderServiceClient.SaveDespatchHdr(salDespatchHdrList);
                                        if (result > 0)
                                        {
                                            litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.DeliveryOrder);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                            ResetForm();
                                            GetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                                            SetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                                            SelectedSosFroDO = null;
                                            Session[ERP.Utilities.SessionStrings.SelectedSosForDO] = null;

                                        }
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("msg_atleast_one_item_hav_quantity").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Invoice").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        if (SelectedSosFroDO != null)
                        {
                            SelectedSOListForDO = SelectedSosFroDO;
                            ModifiedDatePnl.Visible = false;
                            GetFieldValues(ControlsEnum.DELIVERYORDERLIST);
                            SetFieldValues(ControlsEnum.DELIVERYORDERLIST);
                            EntryStatus = EntryStatus.NEWMODE;
                            updateDespatch = false;
                            GetFieldValues(ControlsEnum.INVOICENO);
                            //lblDispInvoiceNo.Text = hdfDespatchNo.Value;
                        }
                        else
                        {
                            SelectedSOListForDO = new List<long>();
                            GetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                            SetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                            EntryStatus = EntryStatus.LISTMODE;

                        }
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                        SetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        ResetForm();
                        GetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                        SetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                        //this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;

                    #endregion
                    #region Invoice Details
                    case ActionsEnum.DELIVERYDETAIL:

                        if (SelectedSosFroDO != null)
                        {
                            SelectedSOListForDO = SelectedSosFroDO;
                            GetFieldValues(ControlsEnum.DELIVERYORDERLIST);
                            SetFieldValues(ControlsEnum.DELIVERYORDERLIST);
                            GetFieldValues(ControlsEnum.CARRIER);
                            SetFieldValues(ControlsEnum.CARRIER);
                            //SetFieldValues(ControlsEnum.COMPANY);
                            GetUIValuesFromObject(ControlsEnum.DELIVERYORDERDETAILS);
                            EntryStatus = EntryStatus.NEWMODE;
                            updateDespatch = false;
                            GetFieldValues(ControlsEnum.INVOICENO);
                            //lblDispInvoiceNo.Text = hdfDespatchNo.Value;
                            if (SelectedSosFroDO != null) //if (string.IsNullOrEmpty(lblDeliveryOrderNo.Text))
                            {
                                pnlPrint.Visible = false;
                            }
                            else
                            {
                                pnlPrint.Visible = true;
                            }
                        }
                        else
                        {
                            foreach (GridViewRow grdrow in grdDeliveryOrderList.Rows)
                            {
                                RadioButton rbtn;
                                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                                if (rbtn.Checked)
                                {
                                    bIsChecked = true;
                                    DespatchID = CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDespatchID")).Value);
                                    break;
                                }
                            }
                            if (bIsChecked)
                            {
                                SetUIEditView(commonActions);
                                ModifiedDatePnl.Visible = true;
                                GetFieldValues(ControlsEnum.DELIVERYORDERDETAILS);
                                SetFieldValues(ControlsEnum.DELIVERYORDERLIST);
                                GetFieldValues(ControlsEnum.CARRIER);
                                //GetFieldValues(ControlsEnum.COMPANY);
                                SetFieldValues(ControlsEnum.CARRIER);
                                //SetFieldValues(ControlsEnum.COMPANY);
                                //finInvoiceVndHdrObj = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                                //finInvoiceVndHdrObj.IVH_PK = CurrPK;
                                //GetFieldValues(ControlsEnum.INVOICEHDR);
                                GetUIValuesFromObject(ControlsEnum.DELIVERYORDERDETAILS);
                                //SetFieldValues(ControlsEnum.POINVOICELIST);
                                if (SelectedSosFroDO != null) //if (string.IsNullOrEmpty(lblDeliveryOrderNo.Text))
                                {
                                    pnlPrint.Visible = false;
                                }
                                else
                                {
                                    pnlPrint.Visible = true;
                                }
                            }
                            else
                            {

                                litErrorMsg.Text = GetLocalResourceObject("msg_select_row").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }

                        break;
                    #endregion
                    #region Edit
                    case ActionsEnum.EDIT:

                        foreach (GridViewRow grdrow in grdDeliveryOrderList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                DespatchID = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDespatchID")).Value);
                                Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomer")).Text;
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            FillProcessID();
                            SetUIEditView(commonActions);
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.DELIVERYORDERDETAILS);
                            GetFieldValues(ControlsEnum.CARRIER);
                            //GetFieldValues(ControlsEnum.COMPANY);
                            SetFieldValues(ControlsEnum.CARRIER);
                            //SetFieldValues(ControlsEnum.COMPANY);
                            salDespatchHdrObj = CommonFunctions.Initilize<SAL_DESPATCH_HDR>();
                            salDespatchHdrObj.DPH_PK = CurrPK;
                            GetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                            GetUIValuesFromObject(ControlsEnum.DELIVERYORDERDETAILS);
                            SetFieldValues(ControlsEnum.DELIVERYORDERLIST);
                        }
                        else
                        {

                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region View
                    case ActionsEnum.VIEW:
                        foreach (GridViewRow grdrow in grdDeliveryOrderList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                DespatchID = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDespatchID")).Value);
                                Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomer")).Text;
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            SetUIEditView(commonActions);
                            GetFieldValues(ControlsEnum.DELIVERYORDERDETAILS);
                            salDespatchHdrObj = CommonFunctions.Initilize<SAL_DESPATCH_HDR>();
                            salDespatchHdrObj.DPH_PK = CurrPK;
                            GetFieldValues(ControlsEnum.CARRIER);
                            //GetFieldValues(ControlsEnum.COMPANY);
                            SetFieldValues(ControlsEnum.CARRIER);
                            //SetFieldValues(ControlsEnum.COMPANY);
                            GetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                            GetUIValuesFromObject(ControlsEnum.DELIVERYORDERDETAILS);
                            SetFieldValues(ControlsEnum.DELIVERYORDERLIST);
                            EntryStatus = EntryStatus.VIEWMODE;
                        }
                        else
                        {

                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }

                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetForm();
                        GetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                        SetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Get Pk
                    case ActionsEnum.SHOWDETAILS:
                        gvr = ((RadioButton)sender).Parent.Parent as GridViewRow;
                        DespatchID = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfDespatchID")).Value);
                        break;
                    #endregion
                    #region PICK ORDERS
                    case ActionsEnum.PICKFORINVOICING:
                        SetUIEditView(commonActions);
                        bool cont = true;
                        GetFieldValues(ControlsEnum.MULTIPLESO);
                        if (admAppConfigMstList != null && admAppConfigMstList.Count > 0)
                        {
                            if (admAppConfigMstList.Select(s => s.ACF_VALUE).FirstOrDefault() != 1)//Multiple for invoicing not allowed
                            {

                                if (XmlCount > 1)
                                {
                                    cont = false;
                                    litErrorMsg.Text = GetLocalResourceObject("Err_mulSC").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else
                                {
                                    cont = true;
                                }
                            }

                        }
                        if (cont)
                        {
                            if (Session[ERP.Utilities.SessionStrings.SALEORDERDOPK] == null)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("NoItemPickforInvoicing").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            int IsDiscountCheckShp = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "SCDiscountRestrictShp").ToString());
                            result = BusinessLogic.Sales.SalesInvoiceBL.GetSOValidityCheck(Session[ERP.Utilities.SessionStrings.SALEORDERDOPK].ToString(), IsDiscountCheckShp);
                            if (result < 0)
                            {

                                if (result == -2) //SOs are of not Same DO
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_SC").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (result == -3) //NO SCs SELECTED
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("NoItemPickforInvoicing").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (result == -7) //DIFF TAX / DISCOUNT / OC 
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_DiffTaxType").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (result == -8) //CUSTOME TAX / DISCOUNT / OC 
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_CustomTaxType").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (result == -9) //MULTIPLE TAX / DISCOUNT / OC 
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_HeaderTaxType").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (result == -10) //DISCOUNT EXISTS
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_DiscountExist").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (result == -11) //Invoice is created and DO modified.
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_DO_Modified").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (result == -12) //DIFF AGENT
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_DiffAgent").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else //Default 
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                //InvoiceMultiplePOPKs = null;
                                Session[ERP.Utilities.SessionStrings.SALEORDERDOPK] = null;
                                btnPickForInvoice.Text = GetLocalResourceObject("PickPoForInvoicing").ToString();
                                
                            }
                            else
                            {
                                //  InvoiceMultiplePOPKs = objPOHeaderItem;
                                //   btnPickForInvoice.Text = GetLocalResourceObject("PickPoForInvoicing").ToString() + "(" + objPOHeaderItem.POList.Count + ")";
                            }
                        }
                        if (result == 1)
                        {
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.Invoicing), false);
                        }
                        break;
                    #endregion
                    #region RESET
                    case ActionsEnum.RESET:
                        Session[ERP.Utilities.SessionStrings.SALEORDERDOPK] = null;
                        btnPickForInvoice.Text = GetLocalResourceObject("PickSoForInvoicing").ToString();
                        break;
                    #endregion
                    #region Tab navigation
                    case ActionsEnum.DEFAULT:
                        //Response.Redirect(Resources.PageURL.SoListingDO);
                        CheckUserRightsAndRedirect(Resources.PageURL.SoListing);
                        //Response.Redirect(Resources.PageURL.SoListing);
                        break;
                    case ActionsEnum.SALESINVOICE:
                        CheckUserRightsAndRedirect(Resources.PageURL.SalesInvoicing);
                        //Response.Redirect(Resources.PageURL.SalesInvoicing);
                        break;
                    case ActionsEnum.INVOICE:
                        CheckUserRightsAndRedirect(Resources.PageURL.Invoicing);
                        //Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.Invoicing), false);
                        break;
                    case ActionsEnum.ADVANCEINVOICE:
                        CheckUserRightsAndRedirect(Resources.PageURL.AdvInvoicing);
                        //Response.Redirect(Resources.PageURL.AdvInvoicing);
                        break;
                    case ActionsEnum.SALESRECEIPT:
                        CheckUserRightsAndRedirect(Resources.PageURL.SalesReceipt);
                        //Response.Redirect(Resources.PageURL.SalesReceipt);
                        break;
                    case ActionsEnum.MISC:
                        CheckUserRightsAndRedirect(Resources.PageURL.MiscellaneousInv);
                        //Response.Redirect(Resources.PageURL.Misc);
                        break;
                    case ActionsEnum.ACRECEIVABLE:
                        foreach (GridViewRow grdrow in grdDeliveryOrderList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomer")).Text;
                                break;
                            }
                        }
                        CheckUserRightsAndRedirect(Resources.PageURL.AccountReceivable);
                        //Response.Redirect(Resources.PageURL.AccountsReceivable);
                        break;
                    case ActionsEnum.CRDRNOTE:
                        CheckUserRightsAndRedirect(Resources.PageURL.DrCrNoteSales);
                        //Response.Redirect(Resources.PageURL.DrCrNoteSales);
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        deliveryOrderServiceClient = new DeliveryOrderService();
                        deliveryOrderServiceClient = CommonFunctions.InitiateClient(deliveryOrderServiceClient);
                        result = deliveryOrderServiceClient.DeleteSalDespatch(DespatchID);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.DeliveryOrder);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            EntryStatus = EntryStatus.LISTMODE;
                            ResetForm();
                            GetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                            SetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                            //btnNew.Focus();
                        }
                        break;
                    #endregion
                    #region Invoice List
                    case ActionsEnum.DELIVERYLIST:
                        CurrPK = 0;
                        hdfDPHPK.Value = "";
                        SelectedSosFroDO = null;
                        Session[ERP.Utilities.SessionStrings.SelectedSosForDO] = null;

                        SelectedSOListForDO = new List<long>();
                        GetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                        SetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Invoice Details
                    case ActionsEnum.INVOICEDETAIL:

                        foreach (GridViewRow grdrow in grdDeliveryOrderList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                DespatchID = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDespatchID")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            SetUIEditView(commonActions);
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.DELIVERYORDERDETAILS);
                            salDespatchHdrObj = CommonFunctions.Initilize<SAL_DESPATCH_HDR>();
                            salDespatchHdrObj.DPH_PK = CurrPK;
                            GetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                            GetUIValuesFromObject(ControlsEnum.DELIVERYORDERDETAILS);
                            SetFieldValues(ControlsEnum.DELIVERYORDERLIST);

                        }
                        else
                        {

                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Print
                    //case ActionsEnum.PRINTINVOICE:
                    //    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DO + "&APPSUBTYPE=1"), false);
                    //    break;
                    //case ActionsEnum.PRINTPACKINGLIST:
                    //    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DO + "&APPSUBTYPE=2"), false);
                    //    break;
                    //case ActionsEnum.PRINTSHIPPINGINSTRUCTION:
                    //    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DO + "&APPSUBTYPE=3"), false);
                    //    break;
                    //case ActionsEnum.PRINTCERTIFICATIONOFORIGIN:
                    //    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DO + "&APPSUBTYPE=4"), false);
                    //    break;
                    //case ActionsEnum.PRINTPOSTSHIPMENTADVICE:
                    //    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DO + "&APPSUBTYPE=5"), false);
                    //    break;
                    //case ActionsEnum.PRINTDELIVERYORDER:
                    //    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DO + "&APPSUBTYPE=6"), false);
                    //    break;

                    case ActionsEnum.PRINT:
                        GetFieldValues(ControlsEnum.PRINTLIST);
                        SetFieldValues(ControlsEnum.PRINTLIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPrint]','" + GetLocalResourceObject("GoodsOutwardNote").ToString() + "','300','200');", true);
                        ddlPrint.Focus();
                        break;

                    case ActionsEnum.ACTIVATE:
                        // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;


                    #endregion
                    #region dropdownchange
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        //Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DO + "&APPSUBTYPE="+ ddlPrint.SelectedValue), false);
                        break;
                    #endregion
                    #region Inspection
                    case ActionsEnum.INSPECTION:
                        foreach (GridViewRow grdrow in grdDeliveryOrderList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                DespatchID = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDespatchID")).Value);
                                Status = (int)WkfStatusEnum.APPROVED;// Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            if (Status == (short)WkfStatusEnum.APPROVED)
                            {
                                Response.Redirect(Resources.PageURL.ContainerInspectionCreate + "?DONO=" + DespatchID);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Select_InvalidInvoice").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {

                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup                       
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    case ActionsEnum.WRKFSUBMIT:
                        //Submit Activity
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            if (grdDeliveryList.Rows.Count > 0)
                            {
                                salDespatchHdrList = new List<SAL_DESPATCH_HDR>();
                                deliveryOrderServiceClient = new DeliveryOrderService();
                                deliveryOrderServiceClient = CommonFunctions.InitiateClient(deliveryOrderServiceClient);
                                salDespatchHdrObj = CommonFunctions.Initilize<SAL_DESPATCH_HDR>();
                                salDespatchHdrObj = (SAL_DESPATCH_HDR)SetUIValuesToObject(ControlsEnum.WRKFSUBMIT);
                                if (isSave)
                                {
                                    if (salDespatchHdrObj != null)
                                    {
                                        salDespatchHdrList.Add(salDespatchHdrObj);
                                        result = deliveryOrderServiceClient.SaveDespatchHdr(salDespatchHdrList);
                                        if (result > 0)// Save Success ! do WorkFlow
                                        {
                                            //Workflow submission
                                            ucrWrkf.ApplicationID = (int)result;
                                            ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                            WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                            //Do WorkFlow if WorkFlow has Actions
                                            if (ddlWkfAction.Items.Count > 0)
                                            {
                                                action = ddlWkfAction.SelectedItem.ToString();
                                                result = ucrWrkf.DoWorkFlow();
                                                if (result > 0)
                                                {
                                                    WrkfComments.Text = "";
                                                    //Show Save success message and reset Contract Entry
                                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                                                    object[] args = new object[2];
                                                    args[0] = Resources.PageNameRes.DeliveryOrder;
                                                    args[1] = lblDeliveryOrderNo.Text;
                                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                                                    if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                                    {
                                                        ResetForm();
                                                        SelectedSosFroDO = null;
                                                        Session[ERP.Utilities.SessionStrings.SelectedSosForDO] = null;
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                                    }
                                                    else
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                                        EntryStatus = EntryStatus.LISTMODE;
                                                        ResetForm();
                                                        GetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                                                        SetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                                                        SelectedSosFroDO = null;
                                                        Session[ERP.Utilities.SessionStrings.SelectedSosForDO] = null;
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            if (result == (int)DbSaveStatus.SQLERROR)
                                            {
                                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                                            {
                                                litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.EditUsedByAnotherUser;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.CODEEXIST)
                                            {
                                                litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else
                                            {
                                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("msg_atleast_one_item_hav_quantity").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Invoice").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region Remove
                    case ActionsEnum.REMOVE:
                        //if (CurrPK == 0)
                        //{
                        //    int DOPk = int.Parse(((Button)sender).CommandArgument.ToString());
                        //    SelectedSosFroDO.Remove(DOPk);
                        //    SelectedSOListForDO = SelectedSosFroDO;
                        //    //salDespatchDtlList = (List<SAL_DESPATCH_HDR>)SaleDespatchDtlList;
                        //    //salDespatchHdrObj = salDespatchDtlList.SingleOrDefault(so => so.DPD_PK == DOPk);
                        //    //PurOrderHdrList.Remove(PurOrderHdrObj);
                        //    //PoHeaderList = PurOrderHdrList;
                        //    //SetFieldValues(ControlsEnum.DELIVERYORDERLIST);//  POINVOICELIST);
                        //}
                        //else
                        //{
                        //    int POPk = int.Parse(((Button)sender).CommandArgument.ToString());
                        //    salDespatchDtlObj = CommonFunctions.Initilize<SAL_DESPATCH_DTL>();
                        //    PurOrderHdrList = (List<PUR_ORDER_HDR>)PoHeaderList;
                        //    finInvoiceVndTrxMpgList = (List<FIN_INVOICE_VND_TRX_MPG>)InvoiceMapList;
                        //    finInvoiceVndTrxMpgObj = finInvoiceVndTrxMpgList.SingleOrDefault(po => po.PUR_ORDER_HDR.POH_PK == POPk);
                        //    finInvoiceVndTrxMpgList.Remove(finInvoiceVndTrxMpgObj);
                        //    InvoiceMapList = finInvoiceVndTrxMpgList;
                        //    SetFieldValues(ControlsEnum.POINVOICELIST);

                        //}
                        break;
                    #endregion
                    #region GONDETAILS
                    case ActionsEnum.GONDETAILS:
                        arg = ((Button)sender).CommandArgument;
                        extGvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                        if (extGvr != null)
                        {
                            grd = extGvr.FindControl("grdSoList") as GridView;
                            if (string.IsNullOrEmpty(arg))
                            {
                                salesOrderHeaderList = null;
                            }
                            else
                            {
                                gonPK = Convert.ToInt32(arg);
                                GetFieldValues(ControlsEnum.SOHEADER);
                            }
                            grd.Visible = true;
                            if (salesOrderHeaderList != null && salesOrderHeaderList.Count > 0)
                            {
                                grd.DataSource = salesOrderHeaderList;
                                grd.DataBind();
                            }
                            (extGvr.FindControl("hdfIsExpandedOrders") as HiddenField).Value = "1";
                        }
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
                    default:
                        break;

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }

            finally
            {
                deliveryOrderServiceClient = null;
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
                    if (SortDirection == Resources.Report.SortAscending)
                        SortDirection = Resources.Report.SortDescending;
                    else
                        SortDirection = Resources.Report.SortAscending;
                }
                else
                {
                    SortBy = e.SortExpression;
                    SortDirection = Resources.Report.SortAscending;

                }
                this.PageIndex = "1";
                GetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                SetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
               
                if (((GridView)sender).ID == "grdDeliveryList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        Label lblSONo = e.Row.FindControl("lblSONo") as Label;
                        Label lblSODate = e.Row.FindControl("lblSODate") as Label;
                        Label lblIGPLCode = e.Row.FindControl("lblIGPLCode") as Label;
                        Label lblBrandCode = e.Row.FindControl("lblBrandCode") as Label;
                        Label lblUOM = e.Row.FindControl("lblUOM") as Label;

                        Label lblOrderQty = e.Row.FindControl("lblOrderQty") as Label;
                        Label lblDespatchedQty = e.Row.FindControl("lblDespatchedQty") as Label;
                        TextBox txtDespNow = e.Row.FindControl("txtDespNow") as TextBox;
                        HiddenField hdfSaleOrderHdrPK = e.Row.FindControl("hdfSaleOrderHdrPK") as HiddenField;
                        HiddenField hdfSaleOrderDtlPK = e.Row.FindControl("hdfSaleOrderDtlPK") as HiddenField;
                        HiddenField hdfIGPLCode = e.Row.FindControl("hdfIGPLCode") as HiddenField;
                        HiddenField hdfBrandCode = e.Row.FindControl("hdfBrandCode") as HiddenField;
                        HiddenField hdfUOM = e.Row.FindControl("hdfUOM") as HiddenField;


                        if (SalOrderDtlList != null && SalOrderDtlList.Count > 0)
                        {
                            hdfSaleOrderHdrPK.Value = SalOrderDtlList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_PK.ToString();
                            hdfSaleOrderDtlPK.Value = SalOrderDtlList[e.Row.RowIndex].SOD_PK.ToString();
                            lblSONo.Text = SalOrderDtlList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NO;
                            lblSONo.ToolTip = SalOrderDtlList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NO;
                            lblSODate.Text = SalOrderDtlList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblSODate.ToolTip = SalOrderDtlList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblIGPLCode.Text = SalOrderDtlList[e.Row.RowIndex].INV_ITEM_MST.ITM_CODE;
                            lblIGPLCode.ToolTip = SalOrderDtlList[e.Row.RowIndex].INV_ITEM_MST.ITM_CODE;
                            hdfIGPLCode.Value = SalOrderDtlList[e.Row.RowIndex].INV_ITEM_MST.ITM_PK.ToString();
                            try
                            {
                                if (SalOrderDtlList[e.Row.RowIndex].CRM_CUST_ITEM_MAP != null)
                                {
                                    lblBrandCode.Text = SalOrderDtlList[e.Row.RowIndex].CRM_CUST_ITEM_MAP.CIM_BRAND_NAME;
                                    lblBrandCode.ToolTip = SalOrderDtlList[e.Row.RowIndex].CRM_CUST_ITEM_MAP.CIM_BRAND_NAME;
                                    hdfBrandCode.Value = SalOrderDtlList[e.Row.RowIndex].CRM_CUST_ITEM_MAP.CIM_PK.ToString();
                                }
                            }
                            catch
                            {
                            }
                            lblUOM.Text = SalOrderDtlList[e.Row.RowIndex].INV_UOM_MST.UOM_CODE;
                            lblUOM.ToolTip = SalOrderDtlList[e.Row.RowIndex].INV_UOM_MST.UOM_CODE;
                            hdfUOM.Value = SalOrderDtlList[e.Row.RowIndex].INV_UOM_MST.UOM_PK.ToString();
                            lblOrderQty.ToolTip = lblOrderQty.Text = SalOrderDtlList[e.Row.RowIndex].SOD_QTY.ToString("N");
                            lblDespatchedQty.ToolTip = lblDespatchedQty.Text = SalOrderDtlList[e.Row.RowIndex].SOD_QTY_DISPATCHED.ToString("N");
                            txtDespNow.Text = (decimal.Parse(lblOrderQty.Text) - decimal.Parse(lblDespatchedQty.Text)).ToString();
                            txtDespNow.Text = (decimal.Parse(SalOrderDtlList[e.Row.RowIndex].SOD_QTY.ToString()) -
                                decimal.Parse(SalOrderDtlList[e.Row.RowIndex].SOD_QTY_DISPATCHED.ToString())).ToString();
                            txtDespNow.Text = Math.Round(decimal.Parse(txtDespNow.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits).ToString();
                        }

                        if (salDespatchDtlList != null && salDespatchDtlList.Count > 0)
                        {
                            hdfSaleOrderHdrPK.Value = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_PK.ToString();
                            hdfSaleOrderDtlPK.Value = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_PK.ToString();
                            lblSONo.Text = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NO;
                            lblSONo.ToolTip = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NO;
                            lblSODate.Text = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblSODate.ToolTip = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblIGPLCode.Text = salDespatchDtlList[e.Row.RowIndex].INV_ITEM_MST.ITM_CODE;
                            lblIGPLCode.ToolTip = salDespatchDtlList[e.Row.RowIndex].INV_ITEM_MST.ITM_CODE;
                            hdfIGPLCode.Value = salDespatchDtlList[e.Row.RowIndex].INV_ITEM_MST.ITM_PK.ToString();
                            try
                            {
                                if (salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.CRM_CUST_ITEM_MAP != null)
                                {
                                    lblBrandCode.Text = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.CRM_CUST_ITEM_MAP.CIM_BRAND_NAME;
                                    lblBrandCode.ToolTip = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.CRM_CUST_ITEM_MAP.CIM_BRAND_NAME;
                                    hdfBrandCode.Value = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.CRM_CUST_ITEM_MAP.CIM_PK.ToString();
                                }
                            }
                            catch
                            {
                            }
                            lblUOM.Text = salDespatchDtlList[e.Row.RowIndex].INV_UOM_MST.UOM_CODE;
                            lblUOM.ToolTip = salDespatchDtlList[e.Row.RowIndex].INV_UOM_MST.UOM_CODE;
                            hdfUOM.Value = salDespatchDtlList[e.Row.RowIndex].INV_UOM_MST.UOM_PK.ToString();
                            lblOrderQty.Text = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY.ToString();
                            lblOrderQty.Text = String.Format("{0:N}", decimal.Parse(lblOrderQty.Text));
                            lblOrderQty.ToolTip = lblOrderQty.Text;

                            lblDespatchedQty.ToolTip = lblDespatchedQty.Text = (salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY_DISPATCHED).ToString("N");// - (salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY_DISPATCHED + Double.Parse(txtDespNow.Text))).ToString();

                            txtDespNow.Text = salDespatchDtlList[e.Row.RowIndex].DPD_QTY_DESPATCHED.ToString();
                            txtDespNow.Text = Math.Round(decimal.Parse(txtDespNow.Text),
                                Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits).ToString();



                            //txtDespNow.Text = salDespatchDtlList[e.Row.RowIndex].DPD_QTY_DESPATCHED.ToString();
                            //txtDespNow.Text = Math.Round(decimal.Parse(txtDespNow.Text),
                            //    Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits).ToString();

                            //lblDespatchedQty.Text = (salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY_DISPATCHED - salDespatchDtlList[e.Row.RowIndex].DPD_QTY_DESPATCHED).ToString();
                            //lblDespatchedQty.Text = String.Format("{0:N}", decimal.Parse(lblDespatchedQty.Text));
                            //lblDespatchedQty.ToolTip = lblDespatchedQty.Text;

                            //lblDespatchedQty.Text = (salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY-(salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY_DISPATCHED + Double.Parse(txtDespNow.Text))).ToString();
                            //lblDespatchedQty.Text = String.Format("{0:N}", decimal.Parse(lblDespatchedQty.Text));
                            //lblDespatchedQty.ToolTip = lblDespatchedQty.Text;

                        }

                    }
                    else if (e.Row.RowType == DataControlRowType.Footer)
                    {


                    }
                }
                else if (((GridView)sender).ID == "grdDeliveryOrderList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        Label lblPrdtUOM = e.Row.FindControl("lblPrdtUOM") as Label;                       
                        Label lblDespQty;
                        lblDespQty = e.Row.FindControl("lblDespQty") as Label;
                        lblDespQty.Text = lblDespQty.ToolTip = String.Format("{0:N}", Convert.ToDecimal(((SAL_DESPATCH_HDR)e.Row.DataItem).SAL_DESPATCH_DTL.Sum(dtl => dtl.DPD_QTY_DESPATCHED).ToString()));
                        //lblDespQty.Text = lblDespQty.ToolTip = String.Format("{0:N}", Convert.ToDecimal((((SAL_DESPATCH_HDR)e.Row.DataItem).SAL_DESPATCH_DTL.Sum(dtl => dtl.DPD_QTY_DESPATCHED / (dtl.DPD_SALE_UOM_CONV.HasValue ? (dtl.DPD_SALE_UOM_CONV.Value > 0 ? dtl.DPD_SALE_UOM_CONV.Value : 1) : 1)))));

                        //List<SAL_DESPATCH_DTL> DESPATCHdtls = ((SAL_DESPATCH_HDR)e.Row.DataItem).SAL_DESPATCH_DTL.ToList();
                        //foreach (SAL_DESPATCH_DTL dtls in   DESPATCHdtls)
                        //{
                        //    invQty = invQty + dtls.SAL_ORDER_DTL.SOD_QTY_INVOICED;
                        //}
                        //lblInvQty.Text = lblInvQty.ToolTip = String.Format("{0:N}", invQty);
                        if (((SAL_DESPATCH_HDR)e.Row.DataItem).SAL_DESPATCH_DTL.FirstOrDefault() != null)
                        {
                            lblPrdtUOM.Text = ((SAL_DESPATCH_HDR)e.Row.DataItem).SAL_DESPATCH_DTL.FirstOrDefault().INV_UOM_MST.UOM_CODE;
                        }
                            double invQty = 0;
                        Label lblInvQty;
                        lblInvQty = e.Row.FindControl("lblInvQty") as Label;
                        HiddenField hfDespId = (HiddenField)e.Row.FindControl("hdfDespatchID");
                        curDspPK = 0;
                        if (!string.IsNullOrEmpty(hfDespId.Value) && hfDespId != null)
                            curDspPK = Convert.ToInt32(hfDespId.Value);
                        GetFieldValues(ControlsEnum.INVQTY);
                        invQty = InvoiceDetailsList.Sum(inv => inv.CID_QTY_INVOICED);                       
                        //invQty = InvoiceDetailsList.Sum(inv => inv.CID_QTY_INVOICED / (inv.CID_SALE_UOM_CONV.HasValue ? (inv.CID_SALE_UOM_CONV.Value > 0 ? inv.CID_SALE_UOM_CONV.Value : 1) : 1));
                        lblInvQty.Text = lblInvQty.ToolTip = String.Format("{0:N}", invQty);

                        //Button imgApproved = e.Row.FindControl("imgApproved") as Button;
                        //Image imgPosted = e.Row.FindControl("imgPosted") as Image;

                        //HiddenField hdfApproved = e.Row.FindControl("hdfApproved") as HiddenField;
                        //HiddenField hdfPosted = e.Row.FindControl("hdfPosted") as HiddenField;

                        //short appstatus = Convert.ToInt16(hdfApproved.Value);
                        //switch (appstatus)
                        //{
                        //    case (short)WkfStatusEnum.APPROVED:
                        //        imgApproved.CssClass = GetLocalResourceObject("mark").ToString();
                        //        imgApproved.ToolTip = Resources.Captions.Approved;
                        //        break;
                        //    case (short)WkfStatusEnum.DRAFTED:
                        //        imgApproved.CssClass = GetLocalResourceObject("close").ToString();
                        //        imgApproved.ToolTip = Resources.Captions.Drafted;
                        //        break;
                        //    case (short)WkfStatusEnum.NEW:
                        //        imgApproved.CssClass = GetLocalResourceObject("close").ToString();
                        //        imgApproved.ToolTip = Resources.Captions.Drafted;
                        //        break;
                        //}

                        //if (Convert.ToBoolean(hdfPosted.Value) == true)
                        //{
                        //    imgPosted.ImageUrl = GetLocalResourceObject("Img_True").ToString();
                        //}
                        //else
                        //{
                        //    imgPosted.ImageUrl = GetLocalResourceObject("Img_False").ToString();
                        //}
                    }
                }
                else if ((sender as GridView).ID == "grdSoList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        ExtGridViewRow gonRow = e.Row.Parent.Parent.Parent.Parent as ExtGridViewRow;
                        SAL_ORDER_HDR sohObj = ((SAL_ORDER_HDR)e.Row.DataItem);
                        if (sohObj != null)
                        {                           
                            Label lblSCDespQty;
                            HiddenField hdfSOID;
                            hdfSOID = e.Row.FindControl("hdfSOID") as HiddenField;
                            lblSCDespQty = e.Row.FindControl("lblSCDespQty") as Label;
                            lblSCDespQty.Text = lblSCDespQty.ToolTip = String.Format("{0:N}", Convert.ToDecimal(sohObj.SAL_DESPATCH_DTL.Where(dtl => dtl.DPD_DESPATCH_HDR == Convert.ToInt32(((HiddenField)gonRow.FindControl("hdfDespatchID")).Value)).Sum(dtl => dtl.DPD_QTY_DESPATCHED).ToString()));
                            //lblSCDespQty.Text = lblSCDespQty.ToolTip = String.Format("{0:N}", Convert.ToDecimal((sohObj.SAL_DESPATCH_DTL.Where(dtl => dtl.DPD_DESPATCH_HDR == Convert.ToInt32(((HiddenField)gonRow.FindControl("hdfDespatchID")).Value)).Sum(dtl => dtl.DPD_QTY_DESPATCHED / (dtl.DPD_SALE_UOM_CONV.HasValue ? (dtl.DPD_SALE_UOM_CONV.Value > 0 ? dtl.DPD_SALE_UOM_CONV.Value : 1) : 1))).ToString()));

                            Label lblSCInvQty;
                            lblSCInvQty = e.Row.FindControl("lblSCInvQty") as Label;
                            //lblSCInvQty.Text = lblSCDespQty.ToolTip = String.Format("{0:N}", Convert.ToDecimal(sohObj.SAL_ORDER_DTL.Where(dtl => dtl.SOD_SO == sohObj.SOH_PK).Sum(dtl => dtl.SOD_QTY_INVOICED).ToString()));
                            curDspPK = 0;
                            double invQty = 0;
                            if (gonPK != null && gonPK > 0)
                                curDspPK = Convert.ToInt32(gonPK);

                            scPK = Convert.ToInt32(hdfSOID.Value);
                            GetFieldValues(ControlsEnum.INVSCQTY);
                            invQty = InvoiceDetailsList.Sum(inv => inv.CID_QTY_INVOICED);                            
                            //invQty = InvoiceDetailsList.Sum(inv => inv.CID_QTY_INVOICED / (inv.CID_SALE_UOM_CONV.HasValue ? (inv.CID_SALE_UOM_CONV.Value > 0 ? inv.CID_SALE_UOM_CONV.Value : 1) : 1));
                            lblSCInvQty.Text = lblSCInvQty.ToolTip = String.Format("{0:N}", invQty);

                        }
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
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
            // --Old Buttons Start--
            this.btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            // --Old Buttons End--
            this.btnPickForInvoice.PreRender += new EventHandler(btnAction_PreRender);
            this.btnResetSelection.PreRender += new EventHandler(btnAction_PreRender);

            //this.lbnSOListing.PreRender += new EventHandler(btnAction_PreRender);
            //this.lnbDeliveryOrder.PreRender += new EventHandler(btnAction_PreRender);
            //this.lnbAdvanceInvoice.PreRender += new EventHandler(btnAction_PreRender);
            //this.lnbSalesInvoice.PreRender += new EventHandler(btnAction_PreRender);
            //this.lbnSalesReceipt.PreRender += new EventHandler(btnAction_PreRender);
            //this.lnbCrDrNote.PreRender += new EventHandler(btnAction_PreRender);
            //this.lnbAcPayables.PreRender += new EventHandler(btnAction_PreRender);
            //this.lnbMiscellaneous.PreRender += new EventHandler(btnAction_PreRender);

            // --Old Buttons Start--
            this.btnDelete.Load += new EventHandler(btnAction_Load);
            this.btnEdit.Load += new EventHandler(btnAction_Load);
            this.btnView.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnCancel.Load += new EventHandler(btnAction_Load);
            // --Old Buttons End--
            this.btnPickForInvoice.Load += new EventHandler(btnAction_Load);
            this.btnResetSelection.Load += new EventHandler(btnAction_Load);

            //this.lbnSOListing.Load += new EventHandler(btnAction_Load);
            //this.lnbDeliveryOrder.Load += new EventHandler(btnAction_Load);
            //this.lnbAdvanceInvoice.Load += new EventHandler(btnAction_Load);
            //this.lnbSalesInvoice.Load += new EventHandler(btnAction_Load);
            //this.lbnSalesReceipt.Load += new EventHandler(btnAction_Load);
            //this.lnbCrDrNote.Load += new EventHandler(btnAction_Load);
            //this.lnbAcPayables.Load += new EventHandler(btnAction_Load);
            //this.lnbMiscellaneous.Load += new EventHandler(btnAction_Load);
        }

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
                GetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                SetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                EnableDisableButtons(e.TotalPages);
                EntryStatus = EntryStatus.LISTMODE;

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
        /// Leave this section if using Master Screens
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
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region WorkFlow Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private int GetApplicationID(int refId)
        {
            int appId = 0;
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtApplication = wrkfService.GetApplicationID(refId);
            if (dtApplication != null)
            {
                if (dtApplication.Rows.Count > 0)
                {
                    appId = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PK] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PK]);
                }
            }
            return appId;
        }
        /// <summary>
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
                PageProcessID = ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                ucrWrkf.PageUrl = base.WkfPageUrl = path;
            }
        }
        #endregion

        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            DELIVERYORDERHDR,
            DELIVERYORDERLIST,
            DELIVERYORDERDETAILS,
            SALDESPATCHHDR,
            SALDESPATCHDTL,
            PICKFORPAYMENT,
            INVOICENO,
            PICKFORCRDRNOTE,
            EXCHANGERATE,
            JOURNALIZE,
            WRKFSUBMIT,
            FINHEADER,
            CARRIER,
            COMPANY,
            DESPATCHNO,
            PRINTLIST,
            NOTIFYPARTY,
            CONSIGNEE,
            ENCLOSURECONFIG,
            SOHEADER,
            TYPECATEGORY,
            INVQTY,
            INVSCQTY,
            MULTIPLESO,
            PORTDETAILS
        }

        /// <summary>
        /// Define Activity Status Enum
        /// </summary>
        public enum WkfStatusEnum
        {
            DRAFTED = 0,
            APPROVED = 2
        }
        #endregion
    }
}